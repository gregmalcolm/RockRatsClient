Imports System.IO
Imports System.Globalization

Module Files
    Private currentJournal As String = "NotInitalized"
    Private lastMaxOffset As Long = 0
    Private line As String = ""
    Private idleCounter As Integer = 0
    Private coords(3) As Integer
    Private waitForCords As String = ""

    Friend Function IdLastJournal() As Boolean
        Dim JournalDir As String = getParameter("JournalDirectory")

        If Not Directory.Exists(JournalDir) Then
            RockRatsClient.FileStatus.ForeColor = Color.DarkRed
            RockRatsClient.FileStatus.Text = "ERROR - Directory not found"
            Return False
        Else
            RockRatsClient.FileStatus.ForeColor = Color.DarkCyan
            RockRatsClient.FileStatus.Text = "Folder Found"
        End If

        Try
            Dim tmpJournal As String = Directory.GetFiles(JournalDir, "Journal*.log").OrderByDescending(Function(f) New FileInfo(f).LastWriteTime).First()
            Dim fileName As String = Right(tmpJournal, (Len(tmpJournal) - InStrRev(tmpJournal, "\")))
            If tmpJournal <> currentJournal Then
                currentJournal = tmpJournal
                lastMaxOffset = 0
                RockRatsClient.LogOutput("Tailing: " + fileName)
            End If
            RockRatsClient.FileStatus.ForeColor = Color.DarkGreen
            RockRatsClient.FileStatus.Text = "Tailing: " + fileName
        Catch ex As Exception
            RockRatsClient.FileStatus.ForeColor = Color.DarkRed
            RockRatsClient.FileStatus.Text = "ERROR - Journals Not Found"
            Return False
        End Try
        Return True
    End Function

    Friend Sub StopJournal()
        currentJournal = "NotInitalized"
        idleCounter = 0
    End Sub

    Public Function TailJournal() As Boolean
        Dim waitForCompletion As Boolean
        If Not File.Exists(currentJournal) Then
            RockRatsClient.FileStatus.ForeColor = Color.DarkRed
            RockRatsClient.FileStatus.Text = "ERROR - File not found"
            Return False
        End If

        Try
            Dim JournalFileStream = New FileStream(currentJournal, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Using reader As New StreamReader(JournalFileStream)
                If lastMaxOffset = 0 Then
                    waitForCompletion = TailJournalReadLines(reader, 1)
                ElseIf reader.BaseStream.Length <> lastMaxOffset Then
                    waitForCompletion = TailJournalReadLines(reader, lastMaxOffset)
                    idleCounter = 0
                ElseIf idleCounter > 20 Then
                    IdLastJournal()
                    idleCounter = 0
                Else
                    idleCounter = idleCounter + 1
                End If
            End Using
            Return True
        Catch ex As Exception
            RockRatsClient.FileStatus.ForeColor = Color.DarkRed
            RockRatsClient.FileStatus.Text = "ERROR - File could not be read"
            Return False
        End Try
    End Function

    Private Function TailJournalReadLines(reader As StreamReader, Offset As Long) As Boolean
        Try
            Dim waitForCompletion As Boolean
            reader.BaseStream.Seek(Offset, SeekOrigin.Begin)
            Do
                line = reader.ReadLine()
                waitForCompletion = FilterJournalLine(line)
            Loop Until line Is Nothing
            lastMaxOffset = reader.BaseStream.Length
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function FilterJournalLine(line As String) As Boolean
        ' Codes expected by the server
        '
        ' Codes
        ' -----
        ' # 2 : System
        ' # 3 : System Faction
        ' # 4 : System Faction State
        ' # 5 : Station
        ' # 6 : Activity
        ' # 7 : Chat
        '
        ' Sub-Codes
        ' ---------
        ' # 0 : Logon
        ' # 1 : FSDJump
        ' # 2 : LoadGame
        ' # 3 : Docked
        ' # 4 : ShipyardSwap
        ' # 5 : ShipyardNew
        ' # 6 : SendText (Also Activity Point of Interest - Called from RockRats client)
        ' # 7 : ReceiveText
        ' # 8 : Promotion
        '

        Try
            Dim curLine As String = Replace(line, """", "|")
            Dim waitForCompletion As Boolean = Nothing
            Dim procActivity As String = getParameter("UpdateSiteActivity")
            curLine = Replace(curLine, "{", "")
            curLine = Trim(Replace(curLine, "}", ""))
            If InStr(curLine, "|event|:|LoadGame|,") > 0 Then
                waitForCompletion = ProcessJournalLine(curLine, "6", "2")
            End If
            Return True
        Catch ex As Exception

        End Try
        Return False
    End Function

    Private Function ProcessJournalLine(line As String, uType As String, uSubType As String) As Boolean
        Try
            Dim waitForCompletion As Boolean
            Dim elapsedMinutes As Double = 100
            Dim elements() As String
            Dim stringSeparators() As String = {", "}
            Dim sTimeStamp As String = ""
            elements = line.Split(stringSeparators, StringSplitOptions.None)
            For Each s As String In elements
                If InStr(s, "|timestamp|:") > 0 Then  ' Timestamp is the first element, so lets get this and exit the for loop, then only proceed if its recent
                    sTimeStamp = Trim(Replace(Mid(s, 14), "|", ""))
                    sTimeStamp = Replace(sTimeStamp, "T", " ")
                    sTimeStamp = Replace(sTimeStamp, "Z", "")
                    Dim datePattern As String = "yyyy-MM-dd HH:mm:ss"
                    Dim dateParsed As Date
                    If DateTime.TryParseExact(sTimeStamp, datePattern, Nothing, DateTimeStyles.None, dateParsed) Then
                        Dim elapsedTime As TimeSpan = DateTime.UtcNow.Subtract(dateParsed)
                        elapsedMinutes = elapsedTime.TotalMinutes
                        Exit For
                    End If
                End If
            Next

            waitForCompletion = ProcessJournalActivityLine(line, uType, uSubType, sTimeStamp)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ProcessJournalActivityLine(line As String, uType As String, uSubType As String, sTimeStamp As String) As Boolean
        Try
            Dim waitForCompletion As Boolean
            Dim elements() As String
            Dim stringSeparators() As String = {", "}
            Dim sShip As String = ""
            Dim promoteRank As String = ""
            Dim promoteType As String = ""

            elements = line.Split(stringSeparators, StringSplitOptions.None)
            For Each s As String In elements
                If InStr(s, "|Commander|:") > 0 And uSubType = "2" Then
                    Dim commanderName As String = Trim(Replace(Mid(s, 14), "|", ""))
                    RockRatsClient.CommanderName.Text = commanderName
                    DataCache.SetDataCache("Store", "LastCommander", commanderName)

                End If
            Next

        Catch ex As Exception
            Return False
        End Try
    End Function


    Private Function DistFromChertan(x As Integer, y As Integer, z As Integer) As Integer
        Dim chertanX As Integer = 58  '  58.34375
        Dim chertanY As Integer = 150 ' 149.5625
        Dim chertanZ As Integer = -38 ' -38.34375
        Dim dist As Integer = CInt(Math.Round(Math.Sqrt(Math.Pow((chertanX - x), 2) + Math.Pow((chertanY - y), 2) + Math.Pow((chertanZ - z), 2))))
        Return dist
    End Function

    Private Function ProcessActivity(cKey As String, uType As String, uSubType As String, sTimeStamp As String, uExtra As String) As Boolean
        Try
            If cKey <> "" Then
                Dim DataRow As String = cKey
                Dim cCat As String = "Activity"

                If uExtra <> "" Then
                    DataRow = DataRow + uExtra
                End If
                Dim waitForCompletion As Boolean = ProcessUpdate(DataRow, sTimeStamp, cCat, cKey, uType, uSubType)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function ProcessUpdate(DataRow As String, sTimeStamp As String, cCat As String, cKey As String, uType As String, uSubType As String) As Boolean
        Dim localCache As String = DataRow + ":" + sTimeStamp
        If DataCache.GetDataCache(cCat, cKey) <> localCache Then
            If DataCache.SetDataCache(cCat, cKey, localCache) Then
                Dim waitForCompletion As Boolean = Comms.SendUpdate(uType, uSubType, DataRow, "")
                RockRatsClient.LogOutput("Sending " + cCat + " Update for " + cKey)
            End If
        Else
            RockRatsClient.LogOutput("Skipping " + cCat + " Update for " + cKey + " - Duplicate Data")
        End If
        Return True
    End Function


End Module
