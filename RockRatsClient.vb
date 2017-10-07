Imports System.IO
Imports System.Deployment.Application

Public Class RockRatsClient
    Public Enum ColumnTypes
        Faction = 0
        Influence = 1
        State = 2
        PrevInfluence = 3
        InfluenceDiff = 4
        PrevState = 5
        Found = 6
    End Enum

    Private AppDataDir As String = Environment.GetEnvironmentVariable("USERPROFILE") + "\AppData\Local\RockRatsClient"
    Private clientVersion As String = Application.ProductVersion
    Private noLogDups As String = ""

    Private Sub tailTimer_Tick(sender As Object, e As EventArgs) Handles tailTimer.Tick
        tailTimer.Enabled = False
        Dim waitForCompletion As Boolean = Files.TailJournal()
        tailTimer.Enabled = True
    End Sub

    Private Sub RockRatsClient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Directory.Exists(AppDataDir) Then
            My.Computer.FileSystem.CreateDirectory(AppDataDir)
        End If
        Parameters.initDefaultParameters()  ' Call this first to set default values
        Comms.InitCommsCodes()
        JournalFolder.Text = Parameters.getParameter("JournalDirectory")
        Dim bw As String = Parameters.getParameter("BlackAndWhile")
        If bw = "True" Then
            BlackAndWhile.Checked = True
        End If
        Dim logOcrText As String = Parameters.getParameter("LogOcrText")
        If logOcrText = "True" Then
            LogOcrCheckbox.Checked = True
        End If
        ScanMarginLeft.Text = Parameters.getParameter("ScanMarginLeft")

        resizeSlider.Value = CInt(Parameters.getParameter("resizeValue"))
        resizeValue.Text = "Resize: " + CType((resizeSlider.Value / 4) + 1, String) + "x"
        SystemsList.Items.Clear()
        SystemName.Text = DataCache.GetDataCache("Store", "LastSystem")
        ShipName.Text = DataCache.GetDataCache("Store", "LastShip")
        CommanderName.Text = DataCache.GetDataCache("Store", "LastCommander")
        If String.IsNullOrEmpty(CommanderName.Text) Then
            CommanderName.Text = "Jameson"
        End If
        LogOutput("Version: " & getVersion())
        LogOutput("AppData: " & AppDataDir)
        Me.Refresh()                     ' Ensure the app is fully loaded before 
        LoadTimer.Enabled = True         ' opening comms - avoids possible exceptions.
    End Sub

    Private Sub LoadTimer_Tick(sender As Object, e As EventArgs) Handles LoadTimer.Tick
        LoadTimer.Enabled = False
        ConnStatus.Text = "Connecting..."
        commsTimer.Enabled = True
    End Sub

    Private Sub BrowserForDir_Click(sender As Object, e As EventArgs) Handles BrowserForDir.Click
        If FolderBrowser.ShowDialog() = DialogResult.OK Then
            JournalFolder.Text = FolderBrowser.SelectedPath
        End If
    End Sub

    Friend Sub toggleTailLog()
        If tailTimer.Enabled = False Then
            LogOutput("Startup Journal Monitor")
            If Files.IdLastJournal() Then
                tailTimer.Enabled = True
                tailLogs.Text = "Stop"
            Else
                LogOutput("Unable to identify Journal Logs")
            End If
        Else
            tailTimer.Enabled = False
            tailLogs.Text = "Run"
            FileStatus.Text = "Idle"
            Files.StopJournal()
            LogOutput("Shutdown Journal Monitor")
        End If
        tailLogs.Enabled = True
    End Sub

    Private Sub JournalFolder_TextChanged(sender As Object, e As EventArgs) Handles JournalFolder.TextChanged
        Parameters.setParameter("JournalDirectory", JournalFolder.Text)
    End Sub

    Private Sub tailLogs_Click_1(sender As Object, e As EventArgs) Handles tailLogs.Click
        toggleTailLog()
    End Sub

    Public Sub LogEverywhere(message As String)
        StatusLog(message)
        LogOutput(message)
    End Sub
    Public Sub StatusLog(message As String)
        If Not String.IsNullOrEmpty(StatusBox.Text) Then
            message = vbNewLine & message
        End If
        StatusBox.AppendText(message)
    End Sub

    Friend Sub LogOutput(logText As String)
        Debug.WriteLine(logText)
        Try
            If noLogDups <> logText Then
                logOut.AppendText(Now().ToString + " - " + logText + vbNewLine)
                If Tabs.SelectedTab Is LogTab Then
                    LogTab.Text = "Log"
                Else
                    LogTab.Text = "Log *"
                End If
            End If
        Catch ex As Exception
            logOut.Text = "" ' Guess it's full - emptying is a harsh workaround but lets see if it ever happens
        End Try
    End Sub
    Private Sub CaptureEDScreen_Click(sender As Object, e As EventArgs) Handles CaptureEDScreen.Click
        Dim bounds As Rectangle
        Dim screenshot As System.Drawing.Bitmap
        Dim graph As Graphics
        Dim scanMarginPercentage As Integer = 25

        StatusLog("OCR Scan in progress...")

        bounds = Screen.PrimaryScreen.Bounds
        Try
            scanMarginPercentage = Integer.Parse(Parameters.getParameter("ScanMarginLeft"))
        Catch ex As Exception
        End Try
        Dim scanMargin As Integer = CInt(bounds.Width * (scanMarginPercentage / 100))

        screenshot = New System.Drawing.Bitmap(scanMargin, bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb) ' Format32bppArgb
        graph = Graphics.FromImage(screenshot)
        graph.CopyFromScreen(bounds.X, bounds.Y, 0, 0, New Size(scanMargin, bounds.Height), CopyPixelOperation.SourceCopy)

        completeEDScreen(screenshot)
    End Sub

    Private Sub PasteEDScreen_Click(sender As Object, e As EventArgs)
        StatusLog("OCR Scan in progress...")
        Dim screenshot As System.Drawing.Bitmap
        If My.Computer.Clipboard.ContainsImage Then
            screenshot = CType(My.Computer.Clipboard.GetImage, Bitmap)
            '        EDCapture.Image = My.Computer.Clipboard.GetImage
            completeEDScreen(screenshot)
        End If
    End Sub

    Private Sub completeEDScreen(screenshot As System.Drawing.Bitmap)
        statusLabel.Text = "Working..."
        ocrWorking.Visible = True
        ocrWorking.Refresh()
        Dim resize As Double = (resizeSlider.Value / 4) + 1
        Dim procBitmap As New Bitmap(CInt(screenshot.Width * resize), CInt(screenshot.Height * resize))
        Dim grBitmap As Graphics = Graphics.FromImage(procBitmap)
        grBitmap.DrawImage(screenshot, 0, 0, procBitmap.Width, procBitmap.Height)
        If BlackAndWhile.Checked Then
            Dim procImg As Bitmap = toGrayScale(procBitmap)
            Call Global.RockRatsClient.ProcEDScreen(procImg)
            EDCapture.Image = procImg
        Else
            Call Global.RockRatsClient.ProcEDScreen(procBitmap)
            EDCapture.Image = procBitmap
        End If

        EDCapture.Refresh()
        ocrWorking.Visible = False
        StatusLog("OCR Finished")
        Call Global.RockRatsClient.ProcessOCRTextChg()
    End Sub

    Private Function toGrayScale(ByVal bmp As Bitmap) As Bitmap
        Dim grayscale As New Imaging.ColorMatrix(New Single()() _
        {
              New Single() {0.299F, 0.299F, 0.299F, 0, 0},
              New Single() {0.587F, 0.587F, 0.587F, 0, 0},
              New Single() {0.114F, 0.114F, 0.114F, 0, 0},
              New Single() {0, 0, 0, 1, 0},
              New Single() {0, 0, 0, 0, 1}
        })

        Dim imgattr As New Imaging.ImageAttributes()
        imgattr.SetColorMatrix(grayscale)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.DrawImage(bmp, New Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, bmp.Width, bmp.Height,
                    GraphicsUnit.Pixel, imgattr)
        End Using

        toGrayScale = bmp
    End Function

    Private Sub UpdSoftData_Click(sender As Object, e As EventArgs) Handles UpdSoftData.Click
        If selSystem.SelectedItem IsNot Nothing Then
            If Not SoftData.HasUserFinishedOCRing() Then
                Dim ok = MsgBox("The influence total looks off. Are you sure you want update the server?", MsgBoxStyle.OkCancel)
                If ok <> MsgBoxResult.Ok Then
                    Return
                End If
            End If
        End If
        StatusLog("Sending '" & selSystem.SelectedItem.ToString & "' data to server...")
        Dim factionsSent As Integer = 0
        Dim done As Boolean = False
        For Each row As DataGridViewRow In SoftDataGrid.Rows
            If row.Cells(ColumnTypes.Faction).Value IsNot Nothing Then
                If row.Cells(ColumnTypes.Influence).Value IsNot Nothing Then
                    If row.Cells(ColumnTypes.Faction).Value.ToString.Trim <> "" Then
                        If IsNumeric(row.Cells(ColumnTypes.Influence).Value.ToString) Then
                            If row.Cells(ColumnTypes.State).Value Is Nothing Then
                                row.Cells(ColumnTypes.State).Value = ""
                            End If
                            Dim cwaitForCompletion As Boolean = Comms.SendUpdate("", "", "",
                                selSystem.SelectedItem.ToString & ":" &
                                row.Cells(ColumnTypes.Faction).Value.ToString.ToUpper & ":" &
                                row.Cells(ColumnTypes.State).Value.ToString & ":" &
                                row.Cells(ColumnTypes.Influence).Value.ToString & ":OCR v" & getVersion())
                            factionsSent += 1
                        Else
                            LogEverywhere("Skipping " & row.Cells(ColumnTypes.Faction).Value.ToString & " because the infuence given is not a number")
                        End If
                    End If
                Else
                    LogEverywhere("Skipping " & row.Cells(ColumnTypes.Faction).Value.ToString & " because no influence was present")
                End If
            End If
        Next
        LogOutput("Updated " & factionsSent & "/" & (SoftDataGrid.Rows.Count - 1).ToString & " Factions in " & selSystem.SelectedItem.ToString)
        'Call Global.RockRatsClient.procOCRTextChg()
    End Sub

    Private Async Sub commsTimer_Tick(sender As Object, e As EventArgs) Handles commsTimer.Tick
        Try
            Await Comms.ProcUpdate()
        Catch ex As Exception
            LogEverywhere("update went boom")
        End Try
    End Sub

    Private Sub selSystem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles selSystem.SelectedIndexChanged
        If selSystem.SelectedItem.ToString <> "" Then
            ResetSystemNameBox(selSystem.SelectedItem.ToString)
            My.Computer.Clipboard.SetText(SystemNameBox.Text)
            StatusLog("Copied '" & selSystem.SelectedItem.ToString & "' to clipboard!")
            Call Global.RockRatsClient.ProcessSystemChange(selSystem.SelectedItem.ToString)
            SoftDataGrid.Select()
        End If
    End Sub

    Private Sub EDCapture_Click(sender As Object, e As EventArgs) Handles EDCapture.Click
        If Not EDCapture.Image Is Nothing Then
            viewImage.imgBox.Width = EDCapture.Image.Width
            viewImage.imgBox.Height = EDCapture.Image.Height
            viewImage.imgBox.Image = EDCapture.Image
            viewImage.Show()
        End If
    End Sub

    Private Sub SoftDataGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles SoftDataGrid.CellValueChanged
        Call Global.RockRatsClient.ProcessOCRTextChg()
    End Sub

    Private Sub resizeSlider_Scroll(sender As Object, e As EventArgs) Handles resizeSlider.Scroll
        resizeValue.Text = "Resize: " + CType((resizeSlider.Value / 4) + 1, String) + "x"
        Parameters.setParameter("resizeValue", resizeSlider.Value.ToString)
    End Sub

    Private Sub BlackAndWhile_CheckedChanged(sender As Object, e As EventArgs) Handles BlackAndWhile.CheckedChanged
        If BlackAndWhile.Checked Then
            Parameters.setParameter("BlackAndWhile", "True")
        Else
            Parameters.setParameter("BlackAndWhile", "False")
        End If
    End Sub

    Private Sub onTop_CheckedChanged(sender As Object, e As EventArgs) Handles onTop.CheckedChanged
        If onTop.Checked Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If
    End Sub

    Friend Function getVersion() As String
        If ApplicationDeployment.IsNetworkDeployed Then
            Return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString
        Else
            Return Application.ProductVersion
        End If
    End Function

    Private Sub SoftDataGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles SoftDataGrid.CellContentClick

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub SoftDataTab_Click(sender As Object, e As EventArgs) Handles SoftDataTab.Click

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub Version_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub StatusBox_TextChanged(sender As Object, e As EventArgs) Handles StatusBox.TextChanged

    End Sub

    Private Sub viewWebTracker_Click(sender As Object, e As EventArgs) Handles viewWebTracker.Click
        Dim webAddress As String = "http://rock-rats-bgs-tracker.s3-website-us-east-1.amazonaws.com/"
        Process.Start(webAddress)
    End Sub

    Private Sub statusLabel_Click(sender As Object, e As EventArgs) Handles statusLabel.Click

    End Sub

    Private Sub NextSystem_Click(sender As Object, e As EventArgs) Handles NextSystem.Click

        If selSystem.SelectedIndex < 0 Then
            selSystem.SelectedIndex = 1
        ElseIf selSystem.SelectedIndex = selSystem.Items.Count - 1 Then
            StatusLog("No more systems!")
        Else
            selSystem.SelectedIndex = selSystem.SelectedIndex + 1
        End If
    End Sub

    Private Sub ResetSystemNameBox(systemName As String)
        SystemNameBox.Text = systemName
        AddButton.Enabled = False
        RemoveButton.Enabled = True
    End Sub

    Private Async Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Dim systemName As String
        systemName = SystemNameBox.Text.ToUpper()

        StatusLog("Adding the System Name to the server...")
        If MessageBox.Show("Add " & systemName & " to the list of systems we're updating?", "Orly?", MessageBoxButtons.OKCancel) = DialogResult.OK Then
            If Await Comms.AddSystemToRoster(systemName) Then
                StatusLog("Successfully added " & systemName & "to the Rock Rat known systems list!")
                selSystem.SelectedIndex = selSystem.Items.Count - 1
            Else
                StatusLog("Failed to add the system, sorry!")
            End If
        End If
    End Sub

    Private Sub SystemNameBox_TextChanged(sender As Object, e As EventArgs) Handles SystemNameBox.TextChanged
        AddButton.Enabled = True
        RemoveButton.Enabled = False
    End Sub

    Private Async Sub RemoveButton_Click(sender As Object, e As EventArgs) Handles RemoveButton.Click
        Dim systemName As String
        systemName = SystemNameBox.Text.ToUpper()

        StatusLog("Removing the System Name to the server...")
        If MessageBox.Show("So... you want to remove " & systemName & " from the list of systems we're updating?", "Huh?", MessageBoxButtons.OKCancel) = DialogResult.OK Then
            If Await Comms.RemoveSystemFromRoster(systemName) Then
                StatusLog("Successfully removed " & systemName & "from the Rock Rat known systems list!")
            Else
                StatusLog("Failed to remove the system, sorry!")
            End If
        End If
    End Sub

    Private Sub SoftDataGrid_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles SoftDataGrid.CellEndEdit
        Dim cell = SoftDataGrid(e.ColumnIndex, e.RowIndex)
        Select Case e.ColumnIndex
            Case ColumnTypes.Faction
                Try
                    Dim faction = cell.Value.ToString
                    cell.Value = Trim(UCase(SoftData.WhitelistChars(faction)))
                Catch ex As Exception
                End Try
            Case ColumnTypes.Influence
                Try
                    Dim influence = Decimal.Parse(cell.Value.ToString)
                    Dim prevInfluenceCell = SoftDataGrid(ColumnTypes.PrevInfluence, e.RowIndex)
                    Dim InfluenceDiffCell = SoftDataGrid(ColumnTypes.InfluenceDiff, e.RowIndex)
                    Dim diff = SoftData.CalcInfluenceDiff(prevInfluenceCell.Value.ToString, cell.Value.ToString)
                    InfluenceDiffCell.Value = diff
                Catch ex As Exception
                    cell.Value = "0"
                End Try
        End Select
    End Sub

    Private Sub SoftDataGrid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles SoftDataGrid.KeyPress
        Dim cell = SoftDataGrid.CurrentCell
        If e.KeyChar.Equals(vbBack) Then
            cell.Value = Nothing
            SoftDataGrid.NotifyCurrentCellDirty(True)
        End If
    End Sub

    Private Sub SoftDataGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles SoftDataGrid.KeyDown
        Dim cell = SoftDataGrid.CurrentCell
        If e.KeyCode.Equals(Keys.Delete) Then
            cell.Value = Nothing
        End If

        If e.KeyCode = Keys.C AndAlso e.Modifiers = Keys.Control Then
            My.Computer.Clipboard.SetText(cell.Value.ToString)
        End If

        If e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control Then
            cell.Value = My.Computer.Clipboard.GetText()
            SoftDataGrid.NotifyCurrentCellDirty(True)
        End If

        If e.KeyCode = Keys.X AndAlso e.Modifiers = Keys.Control Then
            My.Computer.Clipboard.SetText(cell.Value.ToString)
            cell.Value = Nothing
            SoftDataGrid.NotifyCurrentCellDirty(True)
        End If

    End Sub

    Private Sub LogOcrCheckbox_CheckedChanged(sender As Object, e As EventArgs) Handles LogOcrCheckbox.CheckedChanged
        If LogOcrCheckbox.Checked Then
            Parameters.setParameter("LogOcrText", "True")
        Else
            Parameters.setParameter("LogOcrText", "False")
        End If
    End Sub

    Private Sub ScanMarginLeft_TextChanged(sender As Object, e As EventArgs) Handles ScanMarginLeft.TextChanged
        Try
            Dim value = Integer.Parse(Trim(ScanMarginLeft.Text))
            Parameters.setParameter("ScanMarginLeft", ScanMarginLeft.Text)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CommanderName_TextChanged(sender As Object, e As EventArgs) Handles CommanderName.TextChanged
        DataCache.SetDataCache("Store", "LastCommander", CommanderName.Text)
    End Sub
End Class
