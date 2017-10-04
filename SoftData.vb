Imports Emgu.CV
Imports Emgu.CV.CvEnum
Imports Emgu.CV.OCR
Imports System.Text.RegularExpressions

Module SoftData
    Private rockRatsOcr As Tesseract
    Private processingOcrTextChange As Boolean = True
    Private selectedSystem As String = ""
    Private allStates As New Hashtable()
    Private exeDir As String = AppDomain.CurrentDomain.BaseDirectory
    Private influenceAccountedFor As Decimal = 0
    Private systemFactions As New Dictionary(Of String, List(Of Faction))
    Private factions As List(Of Faction)

    Friend Sub ProcEDScreen(bitmapImage As System.Drawing.Bitmap)
        Try
            updateFactionsData()
            processingOcrTextChange = True
            rockRatsOcr = New Tesseract() ' OcrEngineMode.TesseractCubeCombined
            rockRatsOcr.SetVariable("tessedit_char_whitelist", "QWERTYUIOPASDFGHJKLZXCVBNM.0987654321%:")
            rockRatsOcr.Init(exeDir + "\tessdata", "eng", OcrEngineMode.TesseractOnly)
            rockRatsOcr.Recognize(New Image(Of [Structure].Gray, Byte)(bitmapImage))
            Dim elements() As String
            Dim stringSeparators() As String = {vbCrLf}
            elements = rockRatsOcr.GetText.Split(stringSeparators, StringSplitOptions.None)
            Dim catchFaction As String = ""
            Dim factionName As String = ""
            Dim influence As String = ""
            Dim influenceVal As Decimal = 0
            For Each line As String In elements
                ' Uncommit to see OCR text
                ' RockRatsClient.LogEverywhere(line)

                ' line = whitelistChars(line)
                If catchFaction <> "" Then
                    LogOcrLine(line)
                    If InStr(line, "RNMENT") = 0 Then
                        catchFaction = catchFaction + " " + Trim(line)
                    End If
                    factionName = MatchFaction(catchFaction)
                    ' MsgBox(catchFaction + vbNewLine + factionName)
                    catchFaction = ""
                    influence = ""
                    influenceVal = 0

                End If
                If (Strings.Left(line, 2) = "FA" Or Strings.Mid(line, 3, 2) = "CT") And Len(line) > 10 Then
                    catchFaction = Mid(line, 10, Len(line) - 9)
                    LogOcrLine(line)
                End If
                If (Strings.Left(line, 2) = "IN" Or Strings.Mid(line, 3, 2) = "FL") And factionName <> "" Then
                    influenceVal = MatchInfluence(Trim(line))
                    influence = Replace(influenceVal.ToString, ",", ".")
                    LogOcrLine(line)
                End If
                If (Strings.Left(line, 2) = "ST" Or Strings.Mid(line, 3, 2) = "AT") And influence <> "" And factionName <> "" Then
                    Dim s As String = MatchState(Trim(line))
                    AddEDCaptureText(factionName, influence, s, influenceVal)
                    factionName = ""
                    influence = ""
                    influenceVal = 0
                    LogOcrLine(line)
                End If
            Next
        Catch ex As Exception
            MsgBox("Something went wrong with OCR - Try Again")
        End Try
        processingOcrTextChange = False
    End Sub
    Private Sub LogOcrLine(line As String)
        If Parameters.getParameter("logOcrText") = "True" Then
            RockRatsClient.LogOutput(line)
        End If
    End Sub


    Private Sub AddEDCaptureText(factionName As String, influence As String, state As String, influenceVal As Decimal)
        Try
            Dim faction = factions.First(Function(f) f.FactionName.Equals(factionName))
            If faction IsNot Nothing Then
                faction.Influence = influenceVal
                faction.State = state
                If influenceVal > 0 Then
                    faction.Found = True
                End If
                UpdateDataGridRow(faction)
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub UpdateDataGridRow(faction As Faction)
        Dim doInsert As Boolean = True
        Dim influenceDiff = CalcInfluenceDiff(faction.PrevInfluence.ToString, faction.Influence.ToString)
        For Each row As DataGridViewRow In RockRatsClient.SoftDataGrid.Rows
            If Not row.IsNewRow Then
                If row.Cells(RockRatsClient.ColumnTypes.Faction).Value IsNot Nothing Then
                    If row.Cells(RockRatsClient.ColumnTypes.Faction).Value.ToString = faction.FactionName Then
                        row.Cells(RockRatsClient.ColumnTypes.Influence).Value = faction.Influence
                        row.Cells(RockRatsClient.ColumnTypes.State).Value = faction.State
                        row.Cells(RockRatsClient.ColumnTypes.PrevInfluence).Value = faction.PrevInfluence
                        row.Cells(RockRatsClient.ColumnTypes.InfluenceDiff).Value = influenceDiff
                        row.Cells(RockRatsClient.ColumnTypes.PrevState).Value = faction.PrevState
                        row.Cells(RockRatsClient.ColumnTypes.Found).Value = faction.Found
                        doInsert = False
                        Exit For
                    End If
                End If
            End If
        Next
        If doInsert Then
            RockRatsClient.SoftDataGrid.Rows.Add(
                faction.FactionName,
                faction.Influence,
                faction.State,
                faction.PrevInfluence,
                influenceDiff,
                faction.PrevState,
                faction.Found)
        End If
    End Sub
    Public Function CalcInfluenceDiff(prevInfluence As String, influence As String) As String
        Try
            Dim influenceVal = Decimal.Parse(influence)
            Dim prevInfluenceVal = Decimal.Parse(prevInfluence)
            If prevInfluenceVal > 0 And influenceVal > 0 Then
                Dim diff = (influenceVal - prevInfluenceVal).ToString
                If Decimal.Parse(diff) = 0 Then
                    diff = ""
                ElseIf Decimal.Parse(diff) > 0 Then
                    diff = "+" & diff
                End If
                Return diff
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return ""
    End Function

    Friend Sub ProcessOCRTextChg()
        If Not processingOcrTextChange Then
            updateFactionsData()
            Dim i As Decimal = 0
            For Each row As DataGridViewRow In RockRatsClient.SoftDataGrid.Rows
                If Not row.IsNewRow Then
                    If Not row.IsNewRow Then
                        Dim n As Decimal = 0
                        Try
                            n = Decimal.Parse(row.Cells(RockRatsClient.ColumnTypes.Influence).Value.ToString)
                        Catch ex As Exception
                            ' Don't care
                        End Try
                        i = i + n
                    End If
                End If
            Next

            influenceAccountedFor = i
            RockRatsClient.infTotalVal.Text = influenceAccountedFor.ToString
            If HasUserFinishedOCRing() Then
                RockRatsClient.infTotal.ForeColor = Color.DarkGreen
                RockRatsClient.infTotalVal.ForeColor = Color.DarkGreen
            Else
                RockRatsClient.infTotal.ForeColor = Color.DarkRed
                RockRatsClient.infTotalVal.ForeColor = Color.DarkRed
            End If
        End If
    End Sub

    Public Function HasUserFinishedOCRing() As Boolean
        Return influenceAccountedFor > 99.8 And influenceAccountedFor <= 100.1
    End Function

    Friend Sub ProcessSystemChange(systemName As String)
        If systemName <> selectedSystem Then
            ProcessOCRTextChg()
            SaveSystemFactions()
            LoadSystemFactions(systemName)
            selectedSystem = systemName
        End If
    End Sub

    Private Function MatchFaction(targetFaction As String) As String
        Dim factionName As String = ""
        targetFaction = Trim(UCase(targetFaction))
        Dim factionMatched As Boolean = False
        targetFaction = Regex.Replace(targetFaction, "[,'\.]", String.Empty)
        targetFaction = Trim(Replace(targetFaction, "  ", " "))
        If factions IsNot Nothing Then
            If factions.FirstOrDefault(Function(faction) faction.Equals(targetFaction)) IsNot Nothing Then
                factionMatched = True
            Else
                Dim nameLength As Integer = Len(targetFaction)
                Dim x As Integer = CInt(Math.Round(nameLength * 0.95))
                Dim y As Integer = CInt(Math.Round(nameLength * 0.85))
                Dim z As Integer = CInt(Math.Round(nameLength * 0.7))
                Dim e As Integer = CInt(Math.Round(nameLength * 0.3))
                Dim scores = New Dictionary(Of String, Integer)
                For Each faction In factions
                    Dim score As Integer = 0
                    If InStr(faction.FactionName, Left(targetFaction, x)) > 0 Then
                        score = score + 9
                    End If
                    If InStr(faction.FactionName, Right(targetFaction, x)) > 0 Then
                        score = score + 9
                    End If
                    If InStr(faction.FactionName, Left(targetFaction, y)) > 0 Then
                        score = score + 7
                    End If
                    If InStr(faction.FactionName, Right(targetFaction, y)) > 0 Then
                        score = score + 7
                    End If
                    If InStr(faction.FactionName, Left(targetFaction, z)) > 0 Then
                        score = score + 5
                    End If
                    If InStr(faction.FactionName, Right(targetFaction, z)) > 0 Then
                        score = score + 5
                    End If
                    If Left(faction.FactionName, e) = Left(targetFaction, e) Then
                        score = score + 3
                    End If
                    If Right(faction.FactionName, e) = Right(targetFaction, e) Then
                        score = score + 3
                    End If
                    scores.Add(faction.FactionName, score)
                Next
                Dim maxValue As Integer = 0
                For Each score In scores
                    If score.Value > maxValue Then
                        maxValue = score.Value
                        factionName = score.Key
                    End If
                Next
            End If
        End If
        Return factionName
    End Function

    Private Function MatchInfluence(edInfluence As String) As Decimal
        Dim s As String = Strings.Left(edInfluence, Len(edInfluence) - 1)
        s = Trim(Mid(s, 12))
        Dim n As Decimal
        Try
            n = Decimal.Parse(s)
        Catch ex As Exception
            n = 0
        End Try
        Return n
    End Function

    Private Function MatchState(edState As String) As String
        Dim s As String = Replace(edState, "_", "")
        s = Replace(s, "'", "")
        s = Replace(s, ".", "")
        s = Replace(s, "-", "",)
        s = Replace(s, "D", "O", 2, 1)
        s = Replace(s, "D", "O", 3, 1)
        s = Replace(s, "ROOM", "BOOM")
        s = Replace(s, "EOOM", "BOOM")
        s = Replace(s, "EUST", "BUST")
        s = Replace(s, "RUST", "BUST")
        s = Replace(s, "LDCK", "LOCK")
        s = Replace(s, "DDWN", "DOWN")
        s = Replace(s, "IDN", "ION")

        If InStr(s, "BOOM") > 0 Then
            Return "Boom"
        End If
        If InStr(s, "BUST") > 0 Then
            Return "Bust"
        End If
        If InStr(s, "UNREST") > 0 Then
            Return "Civil unrest"
        End If
        If InStr(s, "CIVIL WAR") > 0 Then
            Return "Civil war"
        End If
        If InStr(s, "ELEC") > 0 Then
            Return "Election"
        End If
        If InStr(s, "EXPA") > 0 Then
            Return "Expansion"
        End If
        If InStr(s, "FAMI") > 0 Then
            Return "Famine"
        End If
        If InStr(s, "INVE") > 0 Then
            Return "Investment"
        End If
        If InStr(s, "LOCK") > 0 Then
            Return "Lockdown"
        End If
        If InStr(s, "OUTB") > 0 Then
            Return "Outbreak"
        End If
        If InStr(s, "RETR") > 0 Then
            Return "Retreat"
        End If
        If InStr(s, "WAR") > 0 Then
            Return "War"
        End If
        Return " "
    End Function

    Public Sub AddFactions(systemName As String, factions As List(Of Faction))
        systemFactions.Add(systemName, factions)
    End Sub
    Public Sub SaveSystemFactions()
        updateFactionsData()

        If factions IsNot Nothing And systemFactions IsNot Nothing Then
            If Not String.IsNullOrEmpty(selectedSystem) And systemFactions.ContainsKey(selectedSystem) Then
                systemFactions(selectedSystem) = factions
            End If
        End If
    End Sub
    Public Sub LoadSystemFactions(systemName As String)
        RockRatsClient.SoftDataGrid.Rows.Clear()

        If Not systemFactions.ContainsKey(systemName) Then
            systemFactions.Add(systemName, New List(Of Faction))
        End If
        factions = systemFactions(systemName)
        For Each faction In factions
            UpdateDataGridRow(faction)
        Next
    End Sub
    Public Function WhitelistChars(cleanString As String) As String
        Try
            Dim ch As Char, ln As Integer, x As Integer = 0
            ln = cleanString.Length
            Do While x < ln
                ch = cleanString.Chars(x)
                If Not (Char.IsLetterOrDigit(ch)) And Not (ch = " ") And Not (ch = "-") And Not (ch = ":") Then
                    cleanString = cleanString.Replace(ch, "")
                    ln -= 1
                    x -= 1
                End If
                x += 1
            Loop
        Catch ex As Exception

        End Try
        Return cleanString
    End Function

    Public Function AddSystem(systemName As String) As String
        Dim cleanSystemName As String = SoftData.WhitelistChars(Trim(systemName))
        RockRatsClient.SystemsList.Items.Add(cleanSystemName)
        RockRatsClient.selSystem.Items.Add(cleanSystemName)

        Return cleanSystemName
    End Function
    Public Function RemoveSystem(systemName As String) As String
        Dim cleanSystemName As String = SoftData.WhitelistChars(Trim(systemName))
        RockRatsClient.selSystem.Items.Remove(cleanSystemName)
        RockRatsClient.SystemsList.Items.Remove(cleanSystemName)

        Return cleanSystemName
    End Function

    Public Sub updateFactionsData()
        If systemFactions IsNot Nothing Then
            If Not systemFactions.ContainsKey(selectedSystem) Then
                systemFactions.Add(selectedSystem, New List(Of Faction))
            End If
            Dim updatedFactions = systemFactions(selectedSystem)
            For Each row As DataGridViewRow In RockRatsClient.SoftDataGrid.Rows
                If updatedFactions IsNot Nothing And Not row.IsNewRow Then
                    Dim gridFaction As String = Trim(UCase(WhitelistChars(row.Cells(RockRatsClient.ColumnTypes.Faction).Value.ToString)))
                    If Not String.IsNullOrEmpty(gridFaction) Then
                        If factions.FirstOrDefault(Function(f) f.FactionName.Equals(gridFaction)) Is Nothing Then
                            Dim faction As New Faction() With {
                                .FactionName = gridFaction
                            }
                            updatedFactions.Add(faction)
                        End If
                    End If
                End If
            Next
            For Each row As DataGridViewRow In RockRatsClient.SoftDataGrid.Rows
                If Not row.IsNewRow Then
                    Dim gridFaction As String = Trim(UCase(WhitelistChars(row.Cells(RockRatsClient.ColumnTypes.Faction).Value.ToString)))
                    If Not String.IsNullOrEmpty(gridFaction) Then
                        Dim faction = updatedFactions.First(Function(f) f.FactionName.Equals(gridFaction))
                        Try
                            faction.Influence = Decimal.Parse(row.Cells(RockRatsClient.ColumnTypes.Influence).Value.ToString)
                        Catch ex As Exception
                        End Try
                        faction.State = SafeString(row.Cells(RockRatsClient.ColumnTypes.State).Value)
                        Try
                            faction.Found = CBool(row.Cells(RockRatsClient.ColumnTypes.Found).Value)
                        Catch ex As Exception
                        End Try
                    End If
                End If
            Next
        End If
    End Sub

    Function SafeString(str As Object) As String
        Try
            Return str.ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Module