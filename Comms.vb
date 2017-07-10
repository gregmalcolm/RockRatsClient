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
4 : 11 :   CHERTAN : VARPAS : SAVINCATES : BJIRUP : FIRBON : HIP 54844 : HIP 56321 : HIP 54346 : HIP 55823 : AAKUMAN : HIP 53737
7 : 5 : CHERTAN :   ROCK RATS : ORDER OF CHERTAN : CHERTAN DOMINION : CHERTAN TRAVEL INC : UNION OF CHERTAN INDEPENDANTS
7 : 5 : VARPAS :   ROCK RATS : VARPAS CONFEDERACY : VARPAS DRUG EMPIRE : VARPAS PURPLE POWER CO : VARPAS DYNASTY
7 : 6 : SAVINCATES :   ROCK RATS : NATURAL MEHIT LIBERTY PARTY : SAVINCATES BLUE MAJOR & CO : SAVINCATES NOBLEMENT : SAVINCATES CO-OPERATIVE : SAVINCATES COUNCIL
7 : 7 : BJIRUP :   ROCK RATS : REVOLUTIONARY PARTY OF BJIRUP : UNIFIED ATIUS : HIP 55118 GENERAL CORP : NATURAL MEHIT LIBERTY PARTY : BJIRUP CRIMSON LIFE LTD : NOBLES OF BJIRUP
7 : 5 : FIRBON :   ROCK RATS : FIRBON VISION COMPANY : CO-OP OF FIRBON : DRUG EMPIRE OF FIRBON : HIP 54346 SILVER COMMS
7 : 6 : HIP 54844 :   ROCK RATS : BUREAU OF HIP 53923 MOVEMENT : ALLIED HIP 53923 AUTOCRACY : NATURAL MEHIT LIBERTY PARTY : SAVINCATES BLUE MAJOR & CO : HIP 53923 JET DYNAMIC SOLUTIONS
7 : 5 : HIP 56321 :   ROCK RATS : PEOPLE'S HIP 56321 CONFEDERACY : ARAWOTYAN GALACTIC LIMITED : SOCIAL NIU HSING LEAGUE : DEMOCRATS OF HIP 57080
7 : 6 : HIP 54346 :   ROCK RATS : HIP 54346 SILVER BRIDGE COMMS : FIRBON VISION COMPANY : NUNGGUL CONFEDERACY : SOCIAL PATAKAKA LABOUR : 80 LEONIS VALUES PARTY
7 : 5 : HIP 55823 :   ROCK RATS : ARAWOTYAN GALACTIC LIMITED : TSIM BIKO BLUE FORTUNE PARTNERS : HIP 57112 BLUE ALLIED EXCHANGE : ARAWOTYAN CRIMSON VISION CORP.
7 : 6 : AAKUMAN :   ROCK RATS : AAKUMAN PRISON COLONY : AAKUMAN CRIMSON RAIDERS : 4 A1 VIRGINIS BLUE MAFIA : CO-OPERATIVE OF AAKUMAN : AAKUMAN HOLDINGS
7 : 6 : HIP 53737 :   ROCK RATS : YIN YIN PURPLE CREW : MANT SILVER MAJOR LTD : YIN YIN MONARCHY : YIN YIN BLUE ADVANCED LTD : MANT SOCIETY"
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
            Debug.WriteLine(ex.Message)
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
                Debug.WriteLine(ex.Message)
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
                    Debug.WriteLine(ex.Message)
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
        RockRatsClient.logOutput("Transmitting to AWS: " & data)

        Dim utc = DateTime.UtcNow
        Dim tickTime = DateTime.UtcNow - New TimeSpan(17, 0, 0)
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

        Dim response = Await awsClient.PutItemAsync(
            tableName:="rock-rat-factions",
            item:=attributes)

        If response.HttpStatusCode >= 300 Then
            RockRatsClient.logOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            RockRatsClient.StatusBox.Text = "FAILED! Couldn't send '" & system & "' data"
        Else
            RockRatsClient.logOutput("SUCCESS! (HTTP CODE = " & response.HttpStatusCode & ")")
            RockRatsClient.StatusBox.Text = "Success! Sent '" & system & "' data"
        End If
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
End Module