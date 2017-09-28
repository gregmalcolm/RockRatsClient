Imports System.Net.Sockets
Imports System.Text
Imports Amazon
Imports Amazon.DynamoDBv2
Imports System.Configuration
Imports Amazon.Runtime
Imports Amazon.DynamoDBv2.Model

Public Enum Transmission
    Authenticated = 1
    UpdateNotRequired = 2
    SystemUpdated = 3
    RockRatSystems = 4
    StationUpdated = 5
    ActivityRecorded = 6
    RockRatsSystemFaction = 7
    SoftDataUpdated = 8
    UnableToAuthenticate = 9
End Enum

Module Comms
    Private gotSystems As Boolean = False
    Private responceCodes As New Hashtable()
    Private systemFactions As New Hashtable()
    Private sendQueue As New Queue()
    Private sendCmdQueue As New Queue()
    Private sendSoftDataQueue As New Queue()
    Private lastSendTime As DateTime = DateTime.Now
    Private doRecv As Boolean = False
    'Private tcpClient As New Global.System.Net.Sockets.TcpClient()
    Private authenticated As Boolean = False
    Private bytesSent As Long = 0
    Private bytesRecv As Long = 0
    Private dataIsLoaded As Boolean = False
    Private awsClient As AmazonDynamoDBClient

    Private Function phoneyTCPData() As String
        Dim transmission =
"1
4 : 12 :   CHERTAN : VARPAS : SAVINCATES : BJIRUP : FIRBON : HIP 54844 : HIP 54346 : HIP 55823 : AAKUMAN : HIP 53737 : YU DI WANDY
7 : 5 : CHERTAN :   ROCK RATS : UNION OF CHERTAN INDEPENDENTS : ORDER OF CHERTAN : CHERTAN DOMINION : CHERTAN TRAVEL INC
7 : 5 : VARPAS :   VARPAS CONFEDERACY : ROCK RATS : VARPAS PURPLE POWER CO : VARPAS DYNASTY : VARPAS DRUG EMPIRE
7 : 6 : SAVINCATES :   NATURAL MEHIT LIBERTY PARTY : ROCK RATS : SAVINCATES BLUE MAJOR & CO : SAVINCATES NOBLEMENT : SAVINCATES COUNCIL : SAVINCATES CO-OPERATIVE : VARPAS CONFEDERACY
7 : 7 : BJIRUP :   REVOLUTIONARY PARTY OF BJIRUP : HIP 55118 GENERAL CORP : UNIFIED ATIUS : ROCK RATS : BJIRUP CRIMSON LIFE LTD : NOBLES OF BJIRUP : NATURAL MEHIT LIBERTY PARTY
7 : 5 : FIRBON :   CO-OP OF FIRBON : HIP 54346 SILVER COMMS : DRUG EMPIRE OF FIRBON : FIRBON VISION COMPANY : ROCK RATS
7 : 6 : HIP 54844 :   NATURAL MEHIT LIBERTY PARTY : SAVINCATES BLUE MAJOR & CO : BUREAU OF HIP 53923 MOVEMENT : HIP 53923 JET DYNAMIC SOLUTIONS : ROCK RATS : ALLIED HIP 53923 AUTOCRACY
7 : 6 : HIP 54346 :   HIP 54346 SILVER BRIDGE COMMS : FIRBON VISION COMPANY : 80 LEONIS VALUES PARTY : ROCK RATS : SOCIAL PATAKAKA LABOUR : NUNGGUL CONFEDERACY
7 : 5 : HIP 55823 :   ARAWOTYAN GALACTIC LIMITED : TSIM BIKO BLUE FORTUNE PARTNERS : ROCK RATS : ARAWOTYAN CRIMSON VISION CORP. : HIP 57112 BLUE ALLIED EXCHANGE
7 : 6 : AAKUMAN :   AAKUMAN PRISON COLONY : AAKUMAN CRIMSON RAIDERS : ROCK RATS : 4 A1 VIRGINIS BLUE MAFIA : CO-OPERATIVE OF AAKUMAN : AAKUMAN HOLDINGS
7 : 6 : HIP 53737 :   YIN YIN PURPLE CREW : ROCK RATS : YIN YIN BLUE ADVANCED LTD : MANT SILVER MAJOR LTD : YIN YIN MONARCHY : MANT SOCIETY
7 : 7 : YU DI WANDY :   YU DI WANDY PRISON COLONY : HIP 54346 SILVER BRIDGE COMMS : ROCK RATS : CO-OP OF YU DI WANDY : JOLANJI COMPANY : YIN YIN PURPLE CREW : MANT SOCIETY
7 : 5 : HIP 56321 :   PEOPLE'S HIP 56321 CONFEDERACY : DEMOCRATS OF HIP 57080 : TSIM BIKO BLUE FORTUNE PARTNERS : SOCIAL NIU HSING LEAGUE : ARAWOTYAN GALACTIC LIMITED : ROCK RATS"
        Return transmission
    End Function

    Friend Async Function TestConn() As Task(Of Boolean)
        'If Await connect() Then
        '    Await SendTCP("Hi:" + RockRatsClient.getVersion, False)
        'End If
        'Return False
        Return True
    End Function

    Private Function connect() As Boolean
        Dim accessKey = ConfigurationManager.AppSettings("awsAccessKeyId")
        Dim secretKey = ConfigurationManager.AppSettings("awsSecretAccessKey")

        awsClient = BuildClient(accessKey, secretKey)
        authenticated = True
        Return True
    End Function

    Friend Function sendUpdate(type As String, subtype As String, data As String, softData As String) As Boolean
        Try
            If softData <> "" Then
                sendSoftDataQueue.Enqueue("7:" + softData)
            Else
                sendQueue.Enqueue(type + ":" + subtype + ":" + data + "!" + getParameter("UpdateSiteActivity"))
            End If
            Return True
        Catch ex As Exception
            RockRatsClient.logOutput("Update failed: " & ex.Message)
            Return False
        End Try
    End Function

    Friend Async Function procUpdate() As Task
        If Not authenticated Then
            Try
                connect()
            Catch ex As Exception

            End Try
        ElseIf doRecv Then
            doRecv = False
            Try
                Await recvTCP()
            Catch ex As Exception
                RockRatsClient.logOutput("Recv failed: " & ex.Message)
            End Try
        Else
            doRecv = True
            If authenticated Then
                Try
                    Dim sendData As String = ""
                    Dim elapsedTime As TimeSpan = DateTime.Now.Subtract(lastSendTime)
                    Dim elapsedMilliSecs As Double = elapsedTime.TotalMilliseconds

                    If sendCmdQueue.Count > 0 Then
                        sendData = CType(sendCmdQueue.Peek(), String)
                        sendCmdQueue.Dequeue()
                    ElseIf sendSoftDataQueue.Count > 0 Then
                        sendData = CType(sendSoftDataQueue.Peek(), String)
                        sendSoftDataQueue.Dequeue()
                    ElseIf elapsedMilliSecs > 1499 And sendQueue.Count > 0 Then
                        sendData = CType(sendQueue.Peek(), String)
                        sendQueue.Dequeue()
                        lastSendTime = DateTime.Now
                    End If
                    If sendData <> "" Then
                        Await sendDataUpdate(sendData)
                    End If
                Catch ex As Exception
                    RockRatsClient.logOutput("SendData failed: " & ex.Message)
                End Try
            End If
        End If
    End Function

    Private Sub getSystems()
        sendCmdQueue.Enqueue("1")
    End Sub

    Private Async Function sendDataUpdate(data As String) As Task
        ' 
        If data(0) = CStr(Transmission.RockRatsSystemFaction) Then
            Await SendFactionItemToAws(data)
        End If
        '.AppSettings["awsAccessKeyId"]
    End Function

    Private Async Function SendFactionItemToAws(data As String) As Task
        RockRatsClient.logOutput("Transmitting Report to AWS:   " & data)

        Dim utc = DateTime.UtcNow
        Dim tickTime = DateTime.UtcNow - New TimeSpan(18, 0, 0)
        Dim entryDate = String.Format("{0:yyyy-MM-dd}", tickTime)

        Dim items = Split(data, ":")

        If items.Length < 6 Then
            RockRatsClient.logOutput("Error: bad packet")

            Throw New System.Exception("Bad packet")
        End If

        Dim system = Trim(items(1))
        Dim faction = Trim(items(2))
        Dim state = Trim(items(3))
        Dim influence = Trim(items(4))
        Dim updateType = Trim(items(5))
        Dim commander = RockRatsClient.CommanderName.Text

        Dim id = "" & system & "-" & faction & "-" & entryDate

        If (String.IsNullOrWhiteSpace(system) Or
        String.IsNullOrWhiteSpace(faction) Or
        String.IsNullOrWhiteSpace(entryDate) Or
        String.IsNullOrWhiteSpace(influence)) Then
            RockRatsClient.logOutput("Error: data missing")

            Throw New System.Exception("Data missing")
        End If

        Dim attributes = New Dictionary(Of String, AttributeValue)() From {
            {"id", New AttributeValue() With {
                .S = id
            }},
            {"system", New AttributeValue() With {
                .S = system
            }},
            {"faction", New AttributeValue() With {
                .S = faction
            }},
            {"date", New AttributeValue() With {
                .S = entryDate
            }},
            {"influence", New AttributeValue() With {
                .N = influence
            }},
            {"updateType", New AttributeValue() With {
                .S = updateType
            }}
        }

        If Not String.IsNullOrWhiteSpace(state) Then
            attributes.Add("state", New AttributeValue() With {
                .S = state
            })
        End If

        If Not String.IsNullOrWhiteSpace(commander) Then
            attributes.Add("commander", New AttributeValue() With {
                .S = commander
            })
        End If

        Try
            Dim response = Await awsClient.PutItemAsync(
                tableName:="rock-rat-factions",
                item:=attributes)

            If response.HttpStatusCode >= 300 Then
                RockRatsClient.LogEverywhere("FAILED! Couldn't send '" & system & " - " & faction & "' data")
                RockRatsClient.logOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            Else
                RockRatsClient.LogEverywhere("SUCCESS! Sent '" & system & " - " & faction & "' data")
                RockRatsClient.logOutput("(HTTP CODE = " & response.HttpStatusCode & ")")
            End If
        Catch ex As Exception
            RockRatsClient.LogEverywhere("FAILED! Couldn't send '" & system & " - " & faction & "' data")
            RockRatsClient.logOutput("Error: " & ex.Message)
        End Try

    End Function
    Private Async Function AddFactionNameToAws(systemName As String) As Task
        RockRatsClient.logOutput("Transmitting Add System Name ('" + systemName + "') to AWS: ")

        Dim utc = DateTime.UtcNow
        Dim entryDateTime = String.Format("{0:yyyy-MM-dd hh:mm:ss}", utc)
        Dim commander = RockRatsClient.CommanderName.Text

        Try
            Dim attributes = New Dictionary(Of String, AttributeValue)() From {
                {"system", New AttributeValue() With {
                    .S = systemName
                }},
                {"datetime", New AttributeValue() With {
                    .S = entryDateTime
                }}
            }

            If Not String.IsNullOrWhiteSpace(commander) Then
                attributes.Add("commander", New AttributeValue() With {
                .S = commander
            })
            End If

            Dim response = Await awsClient.PutItemAsync(
                tableName:="rock-rat-systems",
                item:=attributes)

            If response.HttpStatusCode >= 300 Then
                RockRatsClient.LogEverywhere("FAILED! Couldn't add the new System Name to AWS")
                RockRatsClient.logOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            Else
                RockRatsClient.LogEverywhere("SUCCESS! Added the new System Name to AWS")
                RockRatsClient.logOutput("(HTTP CODE = " & response.HttpStatusCode & ")")
            End If
        Catch ex As Exception
            RockRatsClient.LogEverywhere("FAILED! Couldn't add the new System Name to AWS")
            RockRatsClient.logOutput("Error: " & ex.Message)
        End Try
    End Function

    Private Async Function RemoveFactionNameFromAws(systemName As String) As Task
        RockRatsClient.logOutput("Transmitting Add System Name ('" + systemName + "') to AWS: ")

        Dim utc = DateTime.UtcNow
        Dim entryDateTime = String.Format("{0:yyyy-MM-dd hh:mm:ss}", utc)
        Dim commander = RockRatsClient.CommanderName.Text

        Try
            Dim attributes = New Dictionary(Of String, AttributeValue)() From {
                {"system", New AttributeValue() With {
                    .S = systemName
                }}
            }

            Dim response = Await awsClient.DeleteItemAsync(
                tableName:="rock-rat-systems",
                key:=attributes)

            If response.HttpStatusCode >= 300 Then
                RockRatsClient.LogEverywhere("FAILED! Couldn't remove the System Name from AWS")
                RockRatsClient.logOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            Else
                RockRatsClient.LogEverywhere("SUCCESS! Removed the System Name from AWS")
                RockRatsClient.logOutput("(HTTP CODE = " & response.HttpStatusCode & ")")
            End If
        Catch ex As Exception
            RockRatsClient.LogEverywhere("FAILED! Couldn't remove the System Name from AWS")
            RockRatsClient.logOutput("Error: " & ex.Message)
        End Try
    End Function

    Private Function BuildClient(accessKey As String, secretKey As String) As AmazonDynamoDBClient
        Dim credentials = New BasicAWSCredentials(accessKey:=accessKey, secretKey:=secretKey)
        Dim config = New AmazonDynamoDBConfig()
        config.RegionEndpoint = RegionEndpoint.USEast1
        Return New AmazonDynamoDBClient(credentials, config)
    End Function

    Private Async Function recvTCP() As Task
        If (Not dataIsLoaded) Then
            Dim data As String
            data = phoneyTCPData()
            procReturnData(data)
            dataIsLoaded = True
        End If
    End Function

    Private Sub procReturnData(rData As String)
        Dim elements() As String
        Dim stringSeparators() As String = {vbCrLf}
        elements = rData.Split(stringSeparators, StringSplitOptions.None)
        For Each line As String In elements
            If Left(line, 1) = "9" Then
                RockRatsClient.ConnStatus1.Text = "Unable to Authenticate"
                RockRatsClient.logOutput("Connection Failed - Invalid Username or Site Key")
                RockRatsClient.ConnStatus1.ForeColor = Color.DarkRed
            ElseIf Left(line, 1) = "1" Then
                authenticated = True
                RockRatsClient.ConnStatus1.Text = "Connected"
                RockRatsClient.ConnStatus1.ForeColor = Color.DarkGreen
                RockRatsClient.toggleTailLog()
                getSystems()
            ElseIf Left(line, 1) = "4" Then
                Files.setRockRatsSystems(line)
            ElseIf Left(line, 1) = "7" Then
                setSystemFaction(Trim(line))
            ElseIf Len(line) > 0 And Left(line, 1) <> "K" Then
                Dim otherResponce As String = Trim(line)
                If Len(otherResponce) > 0 Then
                    RockRatsClient.logOutput("    Server responded: " + getResponceDesc(otherResponce))
                End If
            End If
        Next
        If authenticated Then
            updBytesTx()
        End If
    End Sub

    Private Sub updBytesTx()
        Dim hSent As String
        Dim hRecv As String
        If bytesSent > 1500 Then
            hSent = CType(Math.Round(bytesSent / 1024), String) + "K"
        Else
            hSent = CType(bytesSent, String) + "B"
        End If
        If bytesRecv > 1500 Then
            hRecv = CType(Math.Round(bytesRecv / 1024), String) + "K"
        Else
            hRecv = CType(bytesRecv, String) + "B"
        End If
    End Sub
    Private Function getResponceDesc(rCode As String) As String
        Dim retValue As String = "Unknown Responce"
        For Each de As DictionaryEntry In responceCodes
            If CType(de.Key, String) = Left(rCode, 1) Then
                retValue = CType(de.Value, String)
                Exit For
            End If
        Next de
        Return retValue
    End Function

    Private Sub setSystemFaction(line As String)
        Dim elements() As String
        Dim stringSeparators() As String = {":"}
        elements = line.Split(stringSeparators, StringSplitOptions.None)
        Dim systemName As String = Trim(UCase(elements(2)))
        Dim factionData As String = elements(1).ToString
        For i = 3 To elements.GetUpperBound(0)
            factionData = factionData + ":" + Trim(UCase(elements(i)))
        Next

        If systemFactions.ContainsKey(systemName) Then
            systemFactions.Remove(systemName)
        End If
        systemFactions.Add(systemName, factionData)
        RockRatsClient.logOutput("Downloaded " + elements(1).ToString + " " + systemName + " Factions")

        SoftData.setFactions(systemName, factionData)
    End Sub

    Friend Sub getSystemFactions(systemName As String)
        Dim queueFaction As Boolean = True
        For Each de As DictionaryEntry In systemFactions
            If CType(de.Key, String) = UCase(systemName) Then
                queueFaction = False
                SoftData.setFactions(systemName, de.Value.ToString)
                Exit For
            End If
        Next de
        If queueFaction Then
            sendCmdQueue.Enqueue("8:" + systemName)
        End If

    End Sub

    Friend Sub initCommsCodes()
        responceCodes.Add("9", "Unable to Authenticate")
        responceCodes.Add("1", "Authenticated")
        responceCodes.Add("2", "Update not required")
        responceCodes.Add("3", "System Updated")
        responceCodes.Add("4", "RockRats Systems")
        responceCodes.Add("5", "Station Updated")
        responceCodes.Add("6", "Activity Recorded")
        responceCodes.Add("7", "RockRats System Faction")
        responceCodes.Add("8", "Soft Data Updated")
        responceCodes.Add("-", "Unknown command")
    End Sub

    Public Async Function AddSystemToRoster(systemName As String) As Task(Of Boolean)
        Try
            Await AddFactionNameToAws(systemName)
            SoftData.AddSystem(systemName)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Public Async Function RemoveSystemFromRoster(systemName As String) As Task(Of Boolean)
        Try
            Await RemoveFactionNameFromAws(systemName)
            SoftData.removeSystem(systemName)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

End Module