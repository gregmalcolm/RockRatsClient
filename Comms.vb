Imports System.Net.Sockets
Imports System.Text
Imports Amazon
Imports Amazon.DynamoDBv2
Imports System.Configuration
Imports Amazon.Runtime

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
    Private keepAliveCount As Integer = 0
    Private dataIsLoaded As Boolean = False
    Private awsClient As AmazonDynamoDBClient

    Private Function phoneyTCPData() As String
        Dim transmission =
"1
4 : 11 :   Chertan : Varpas : Savincates : Bjirup : Firbon : HIP 54844 : HIP 56321 : HIP 54346 : HIP 55823 : Aakuman : HIP 53737
7 : 5 : Chertan :   Rock Rats : Order of Chertan : Chertan Dominion : Chertan Travel Inc : Union of Chertan Independants
7 : 5 : Varpas :   Rock Rats : Varpas Confederacy : Varpas Drug Empire : Varpas Purple Power Co : Varpas Dynasty
7 : 6 : Savincates :   Rock Rats : Natural Mehit Liberty Party : Savincates Blue Major & Co : Savincates Noblement : Savincates Co-operative : Savincates Council
7 : 7 : Bjirup :   Rock Rats : Revolutionary Party of Bjirup : Unified Atius : HIP 55118 General Corp : Natural Mehit Liberty Party : Bjirup Crimson Life Ltd : Nobles of Bjirup
7 : 5 : Firbon :   Rock Rats : Firbon Vision Company : Co-op of Firbon : Drug Empire of Firbon : HIP 54346 Silver Comms
7 : 6 : HIP 54844 :   Rock Rats : Bureau of HIP 53923 Movement : Allied HIP 53923 Autocracy : Natural Mehit Liberty Party : Savincates Blue Major & Co : HIP 53923 Jet Dynamic Solutions
7 : 5 : HIP 56321 :   Rock Rats : People's HIP 56321 Confederacy : Arawotyan Galactic Limited : Social Niu Hsing League : Democrats of HIP 57080
7 : 6 : HIP 54346 :   Rock Rats : HIP 54346 Silver Bridge Comms : Firbon Vision Company : Nunggul Confederacy : Social Patakaka Labour : 80 Leonis Values Party
7 : 5 : HIP 55823 :   Rock Rats : Arawotyan Galactic Limited : Tsim Biko Blue Fortune Pa'nrs : HIP 57112 Blue Allied Exchange : Arawotyan Crimson Vision Corp
7 : 6 : Aakuman :   Rock Rats : Aakuman Prison Colony : Aakuman Crimson Raiders : 4 A1 Virginis Blue Mafia : Co-operative of Aakuman : Aakuman Holdings
7 : 6 : HIP 53737 :   Rock Rats : Yin Yin Purple Crew : Mant Silver Major Ltd : Yin Yin Monarchy : Yin Yin Blue Advanced Ltd : Mant Society"
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

            End Try
        Else
            doRecv = True
            If authenticated Then
                Try
                    Dim sendData As String = ""
                    Dim keepAlive As Boolean = False
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
                    Else
                        keepAliveCount = keepAliveCount + 1
                        If keepAliveCount > 99 Then
                            sendData = "K"
                            keepAlive = True
                        End If
                    End If
                    If sendData <> "" Then
                        Await SendTCP(sendData, keepAlive)
                    End If
                Catch ex As Exception

                End Try
            End If
        End If
    End Function

    Private Sub getSystems()
        sendCmdQueue.Enqueue("1")
    End Sub

    Private Async Function SendTCP(sendData As String, keepAlive As Boolean) As Task
        '.AppSettings["awsAccessKeyId"]
        'If tcpClient.Connected Then
        '    Try
        '        Dim sendText As String
        '        If keepAlive Then
        '            sendText = sendData
        '        Else
        '            sendText = Parameters.getParameter("Username") + ":" + Parameters.getParameter("SiteKey") + "!" + sendData
        '        End If
        '        Dim networkStream As NetworkStream = TcpClient.GetStream()
        '        If networkStream.CanWrite And networkStream.CanRead Then
        '            bytesSent = bytesSent + Len(sendText)
        '            Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(sendText & vbNewLine)
        '            Await networkStream.WriteAsync(sendBytes, 0, sendBytes.Length)
        '        Else
        '            streamError(networkStream)
        '        End If
        '    Catch ex As Exception

        '    End Try
        '    keepAliveCount = 0
        'End If
    End Function


    Private Function BuildClient(accessKey As String, secretKey As String) As AmazonDynamoDBClient
        Dim credentials = New BasicAWSCredentials(accessKey:=accessKey, secretKey:=secretKey)
        Dim config = New AmazonDynamoDBConfig()
        config.RegionEndpoint = RegionEndpoint.USEast1
        Return New AmazonDynamoDBClient(credentials, config)
    End Function

    Private Async Function recvTCP() As Task
        'If tcpClient.Connected And tcpClient.ReceiveBufferSize > 0 Then
        '    Dim networkStream As NetworkStream = tcpClient.GetStream()
        '    If networkStream.CanWrite And networkStream.CanRead Then
        '        Dim returndata As String = ""
        '        Try
        '            Dim bytes(tcpClient.ReceiveBufferSize) As Byte
        '            Await networkStream.ReadAsync(bytes, 0, CInt(tcpClient.ReceiveBufferSize))
        '            returndata = Trim(SoftData.whitelistChars(Encoding.ASCII.GetString(bytes)))
        '            bytesRecv = bytesRecv + Len(returndata)
        '            procReturnData(returndata)
        '        Catch ex As Exception
        '        End Try
        '    Else
        '        streamError(networkStream)
        '    End If
        'End If
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
                RockRatsClient.ConnStatus2.Text = "Check Username and Site Key"
                RockRatsClient.logOutput("Connection Failed - Invalid Username or Site Key")
                RockRatsClient.ConnStatus1.ForeColor = Color.DarkRed
                RockRatsClient.ConnStatus2.ForeColor = Color.DarkRed
            ElseIf Left(line, 1) = "1" Then
                authenticated = True
                RockRatsClient.logOutput("Connected to " + getParameter("HostAddress"))
                RockRatsClient.ConnStatus1.Text = "Connected"
                RockRatsClient.ConnStatus1.ForeColor = Color.DarkGreen
                RockRatsClient.ConnStatus2.ForeColor = Color.DarkGreen
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
        RockRatsClient.ConnStatus2.Text = "Sent: " + hSent + "  Recv: " + hRecv
    End Sub

    Private Sub streamError(networkStream As NetworkStream)
        'RockRatsClient.logOutput("Connection Failed - Issue with stream")
        'If Not networkStream.CanRead Then
        '    RockRatsClient.ConnStatus1.Text = "cannot not write data to this stream"
        '    RockRatsClient.ConnStatus1.ForeColor = Color.DarkRed
        '    tcpClient.Close()
        'Else
        '    If Not networkStream.CanWrite Then
        '        RockRatsClient.ConnStatus1.Text = "cannot read data from this stream"
        '        RockRatsClient.ConnStatus1.ForeColor = Color.DarkRed
        '        tcpClient.Close()
        '    End If
        'End If
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