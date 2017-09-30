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
    Private isReceivingData As Boolean = False
    Private authenticated As Boolean = False
    Private dataIsLoaded As Boolean = False
    Private awsClient As AmazonDynamoDBClient

    Private Function Connect() As Boolean
        Dim accessKey = ConfigurationManager.AppSettings("awsAccessKeyId")
        Dim secretKey = ConfigurationManager.AppSettings("awsSecretAccessKey")

        awsClient = BuildClient(accessKey, secretKey)
        authenticated = True
        Return True
    End Function

    Friend Function SendUpdate(type As String, subtype As String, data As String, softData As String) As Boolean
        Try
            If softData <> "" Then
                sendSoftDataQueue.Enqueue("7:" + softData)
            Else
                sendQueue.Enqueue(type + ":" + subtype + ":" + data + "!" + getParameter("UpdateSiteActivity"))
            End If
            Return True
        Catch ex As Exception
            RockRatsClient.LogOutput("Update failed: " & ex.Message)
            Return False
        End Try
    End Function

    Friend Async Function ProcUpdate() As Task
        If Not authenticated Then
            Try
                Connect()
            Catch ex As Exception
                RockRatsClient.LogOutput("Connect failed: " & ex.Message)
            End Try
        ElseIf isReceivingData Then
            isReceivingData = False
            Try
                Await ReceiveData()
            Catch ex As Exception
                RockRatsClient.LogOutput("Recv failed: " & ex.Message)
            End Try
        Else
            isReceivingData = True
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
                        Await SendDataUpdate(sendData)
                    End If
                Catch ex As Exception
                    RockRatsClient.LogOutput("SendData failed: " & ex.Message)
                End Try
            End If
        End If
    End Function

    Private Sub GetSystems()
        sendCmdQueue.Enqueue("1")
    End Sub

    Private Async Function SendDataUpdate(data As String) As Task
        ' 
        If data(0) = CStr(Transmission.RockRatsSystemFaction) Then
            Await SendFactionItemToAws(data)
        End If
        '.AppSettings["awsAccessKeyId"]
    End Function

    Private Async Function SendFactionItemToAws(data As String) As Task
        RockRatsClient.LogOutput("Transmitting Report to AWS:   " & data)

        Dim utc = DateTime.UtcNow
        Dim tickTime = DateTime.UtcNow - New TimeSpan(18, 0, 0)
        Dim entryDate = String.Format("{0:yyyy-MM-dd}", tickTime)

        Dim items = Split(data, ":")

        If items.Length < 6 Then
            RockRatsClient.LogOutput("Error: bad packet")

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
            RockRatsClient.LogOutput("Error: data missing")

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
                RockRatsClient.LogOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            Else
                RockRatsClient.LogEverywhere("SUCCESS! Sent '" & system & " - " & faction & "' data")
                RockRatsClient.LogOutput("(HTTP CODE = " & response.HttpStatusCode & ")")
            End If
        Catch ex As Exception
            RockRatsClient.LogEverywhere("FAILED! Couldn't send '" & system & " - " & faction & "' data")
            RockRatsClient.LogOutput("Error: " & ex.Message)
        End Try

    End Function
    Private Async Function LoadSystemsFromAws() As Task
        RockRatsClient.LogEverywhere("Downloading Systems...")

        Dim request = New ScanRequest() With {
            .TableName = "rock-rat-systems"
        }
        Dim response = Await awsClient.ScanAsync(request)

        RockRatsClient.selSystem.Items.Clear()
        For Each item In response.Items.OrderBy(Function(x) x("datetime").S)
            SoftData.AddSystem(item("system").S)
        Next
    End Function

    Private Async Function AddFactionNameToAws(systemName As String) As Task
        RockRatsClient.LogOutput("Transmitting Add System Name ('" + systemName + "') to AWS: ")

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
                RockRatsClient.LogOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            Else
                RockRatsClient.LogEverywhere("SUCCESS! Added the new System Name to AWS")
                RockRatsClient.LogOutput("(HTTP CODE = " & response.HttpStatusCode & ")")
            End If
        Catch ex As Exception
            RockRatsClient.LogEverywhere("FAILED! Couldn't add the new System Name to AWS")
            RockRatsClient.LogOutput("Error: " & ex.Message)
        End Try
    End Function
    Private Async Function RemoveFactionNameFromAws(systemName As String) As Task
        RockRatsClient.LogOutput("Transmitting Add System Name ('" + systemName + "') to AWS: ")

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
                RockRatsClient.LogOutput("FAILED! (HTTP CODE = " & response.HttpStatusCode & ")")
            Else
                RockRatsClient.LogEverywhere("SUCCESS! Removed the System Name from AWS")
                RockRatsClient.LogOutput("(HTTP CODE = " & response.HttpStatusCode & ")")
            End If
        Catch ex As Exception
            RockRatsClient.LogEverywhere("FAILED! Couldn't remove the System Name from AWS")
            RockRatsClient.LogOutput("Error: " & ex.Message)
        End Try
    End Function

    Private Function BuildClient(accessKey As String, secretKey As String) As AmazonDynamoDBClient
        Dim credentials = New BasicAWSCredentials(accessKey:=accessKey, secretKey:=secretKey)
        Dim config = New AmazonDynamoDBConfig()
        config.RegionEndpoint = RegionEndpoint.USEast1
        Return New AmazonDynamoDBClient(credentials, config)
    End Function

    Private Async Function ReceiveData() As Task
        If (Not dataIsLoaded) Then
            Await LoadSystemsFromAws()
            'SoftData.AddSystem("CHERTAN")
            dataIsLoaded = True
        End If
    End Function

    Private Sub ProcReturnData(rData As String)
        Dim elements() As String
        Dim stringSeparators() As String = {vbCrLf}
        elements = rData.Split(stringSeparators, StringSplitOptions.None)
        For Each line As String In elements
            If Left(line, 1) = "9" Then
                RockRatsClient.ConnStatus.Text = "Unable to Authenticate"
                RockRatsClient.LogOutput("Connection Failed - Invalid Username or Site Key")
                RockRatsClient.ConnStatus.ForeColor = Color.DarkRed
            ElseIf Left(line, 1) = "1" Then
                authenticated = True
                RockRatsClient.ConnStatus.Text = "Connected"
                RockRatsClient.ConnStatus.ForeColor = Color.DarkGreen
                RockRatsClient.toggleTailLog()
                GetSystems()
            ElseIf Left(line, 1) = "4" Then
                Files.setRockRatsSystems(line)
            ElseIf Left(line, 1) = "7" Then
                SetSystemFaction(Trim(line))
            ElseIf Len(line) > 0 And Left(line, 1) <> "K" Then
                Dim otherResponce As String = Trim(line)
                If Len(otherResponce) > 0 Then
                    RockRatsClient.LogOutput("    Server responded: " + GetResponceDesc(otherResponce))
                End If
            End If
        Next
    End Sub

    Private Function GetResponceDesc(rCode As String) As String
        Dim retValue As String = "Unknown Responce"
        For Each de As DictionaryEntry In responceCodes
            If CType(de.Key, String) = Left(rCode, 1) Then
                retValue = CType(de.Value, String)
                Exit For
            End If
        Next de
        Return retValue
    End Function

    Private Sub SetSystemFaction(line As String)
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
        RockRatsClient.LogOutput("Downloaded " + elements(1).ToString + " " + systemName + " Factions")

        SoftData.setFactions(systemName, factionData)
    End Sub

    Friend Sub GetSystemFactions(systemName As String)
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

    Friend Sub InitCommsCodes()
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
            SoftData.RemoveSystem(systemName)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

End Module