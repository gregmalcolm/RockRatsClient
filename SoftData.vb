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
            Dim factionFirstLine As String = ""
            Dim factionName As String = ""
            Dim influence As String = ""
            Dim influenceVal As Decimal = 0
            For Each line As String In elements
                ' Uncommit to see OCR text
                ' RockRatsClient.LogEverywhere(line)

                ' line = whitelistChars(line)
                If factionFirstLine <> "" Then
                    LogOcrLine(line)
                    If Not isGovermentText(line) Then
                        factionFirstLine = factionFirstLine + " " + Trim(line)
                    End If
                    factionName = MatchFaction(factionFirstLine)
                    ' MsgBox(catchFaction + vbNewLine + factionName)
                    factionFirstLine = ""
                    influence = ""
                    influenceVal = 0

                End If
                If IsFactionText(line) Then
                    factionFirstLine = Mid(line, 10, Len(line) - 9)
                    LogOcrLine(line)
                End If
                If factionName <> "" AndAlso isInfluenceText(line) Then
                    influenceVal = MatchInfluence(Trim(line))
                    influence = Replace(influenceVal.ToString, ",", ".")
                    LogOcrLine(line)
                End If
                If influence <> "" AndAlso isStateText(line) Then
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
        If Parameters.GetParameter("logOcrText") = "True" Then
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
            RockRatsClient.InfTotalVal.Text = influenceAccountedFor.ToString
            If HasUserFinishedOCRing() Then
                RockRatsClient.InfTotal.ForeColor = Color.DarkGreen
                RockRatsClient.InfTotalVal.ForeColor = Color.DarkGreen
            Else
                RockRatsClient.InfTotal.ForeColor = Color.DarkRed
                RockRatsClient.InfTotalVal.ForeColor = Color.DarkRed
            End If
        End If
    End Sub

    Public Function HasUserFinishedOCRing() As Boolean
        Return influenceAccountedFor > 99.8 And influenceAccountedFor <= 100.1
    End Function

    Friend Sub ProcessSystemChange(systemName As String)
        If systemName <> selectedSystem Then
            RockRatsClient.SoftDataGrid.Enabled = False
            ProcessOCRTextChg()
            SaveSystemFactions()
            LoadSystemFactions(systemName)
            selectedSystem = systemName
            RockRatsClient.SoftDataGrid.Enabled = True
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

    Private Function MatchInfluence(influence As String) As Decimal
        Dim val As Decimal = 0
        Dim pattern As String = ".*\b(\d{1,2})\.?(\d).*"
        Try
            If Regex.Match(influence, pattern).Success Then
                influence = Regex.Replace(influence, pattern, "$1.$2")
                val = Decimal.Parse(influence)
            End If
        Catch ex As Exception
            val = 0
        End Try
        Return val
    End Function

    Private Function MatchState(stateText As String) As String
        Dim text As String = Replace(stateText, "_", "")
        text = Regex.Replace(text, "^[^ ]* +(\w.+)$", "$1")

        If WordMatchScore(text, "CIVILUNREST", noOfWords:=2) > 7 Then
            Return "Civil unrest"
        End If
        If WordMatchScore(text, "CIVILWAR", noOfWords:=2) >= 6 Then
            Return "Civil war"
        End If
        If WordMatchScore(text, "INVESTMENT") >= 6 Then
            Return "Investment"
        End If
        If WordMatchScore(text, "EXPANSION") >= 6 Then
            Return "Expansion"
        End If
        If WordMatchScore(text, "LOCKDOWN") >= 5 Then
            Return "Lockdown"
        End If
        If WordMatchScore(text, "ELECTION") >= 5 Then
            Return "Election"
        End If
        If WordMatchScore(text, "OUTBREAK") >= 5 Then
            Return "Outbreak"
        End If
        If WordMatchScore(text, "RETREAT") >= 4 Then
            Return "Retreat"
        End If
        If WordMatchScore(text, "FAMINE") >= 4 Then
            Return "Famine"
        End If
        If WordMatchScore(text, "BOOM") >= 3 Then
            Return "Boom"
        End If
        If WordMatchScore(text, "BUST") >= 3 Then
            Return "Bust"
        End If
        If WordMatchScore(text, "WAR") >= 2 Then
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
        RockRatsClient.SelectedSystem.Items.Add(cleanSystemName)

        Return cleanSystemName
    End Function
    Public Function RemoveSystem(systemName As String) As String
        Dim cleanSystemName As String = SoftData.WhitelistChars(Trim(systemName))
        RockRatsClient.SelectedSystem.Items.Remove(cleanSystemName)

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

    Function isFactionText(line As String) As Boolean
        Return LabelMatchScore(line, "FACTION", ExtraChars:=3) >= 4
    End Function

    Function isInfluenceText(line As String) As Boolean
        Return LabelMatchScore(line, "INFLUENCE", ExtraChars:=2) >= 5
    End Function

    Function isStateText(line As String) As Boolean
        Return LabelMatchScore(line, "STATE", ExtraChars:=3) >= 4
    End Function

    Function isGovermentText(line As String) As Boolean
        Return LabelMatchScore(line, "GOVERMENT", ExtraChars:=3) >= 5
    End Function

    Function LabelMatchScore(line As String, matchWord As String, Optional ExtraChars As Integer = 0) As Integer
        Dim found As Boolean = False
        If Len(line) >= matchWord.Count + ExtraChars And line.IndexOf(" ") > -1 Then
            Dim label = line.Split(" "c)(0)
            Return WordMatchScore(label, matchWord)
        End If
        Return 0
    End Function

    Function WordMatchScore(line As String, matchWord As String, Optional noOfWords As Integer = 1) As Integer
        Dim found As Boolean = False
        Dim space = New Regex(" ")
        Dim Candidate = space.Replace(line, "", noOfWords - 1)
        Candidate = Regex.Replace(Candidate, "[\W]+", "")
        If Len(Candidate) >= matchWord.Count Then
            Dim score = 0
            Dim n As Integer
            For n = 0 To matchWord.Count - 1
                Dim pattern = MatchGlpyhsPatternFor(matchWord.Chars(n))

                If Regex.Match(Candidate.ElementAt(n), pattern).Success Then
                    score += 1
                End If
            Next
            Return score
        End If
        Return 0
    End Function


    Function MatchGlpyhsPatternFor(c As Char) As String
        Select Case c.ToString
            Case "B"
                Return "[BRE]"
            Case "C"
                Return "[CE]"
            Case "F"
                Return "[FP]"
            Case "O"
                Return "[OD]"
            Case Else
                Return c.ToString
        End Select
    End Function
End Module