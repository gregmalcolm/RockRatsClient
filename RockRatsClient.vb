Imports System.IO
Imports System.Deployment.Application

Public Class RockRatsClient
    Private AppDataDir As String = Environment.GetEnvironmentVariable("USERPROFILE") + "\AppData\Local\RockRatsClient"
    Private clientVersion As String = Application.ProductVersion
    Private noLogDups As String = ""

    Private Sub tailTimer_Tick(sender As Object, e As EventArgs) Handles tailTimer.Tick
        tailTimer.Enabled = False
        Dim waitForCompletion As Boolean = Files.tailJournal()
        tailTimer.Enabled = True
    End Sub

    Private Sub RockRatsClient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Directory.Exists(AppDataDir) Then
            My.Computer.FileSystem.CreateDirectory(AppDataDir)
        End If
        Parameters.initDefaultParameters()  ' Call this first to set default values
        Files.initJournalCodes()
        Comms.initCommsCodes()
        Username.Text = Parameters.getParameter("Username")
        SiteKey.Text = Parameters.getParameter("SiteKey")
        JournalFolder.Text = Parameters.getParameter("JournalDirectory")
        Dim bw As String = Parameters.getParameter("BlackAndWhile")
        If bw = "True" Then
            BlackAndWhile.Checked = True
        End If
        resizeSlider.Value = CInt(Parameters.getParameter("resizeValue"))
        resizeValue.Text = "Resize: " + CType((resizeSlider.Value / 4) + 1, String) + "x"
        SaveConnDetails.Enabled = False
        SaveJournalDir.Enabled = False
        SystemsList.Items.Clear()
        SystemName.Text = DataCache.getDataCache("Store", "LastSystem")
        ShipName.Text = DataCache.getDataCache("Store", "LastShip")
        CommanderName.Text = DataCache.getDataCache("Store", "LastCommander")
        RockRatsActivity.Items.Add("Logon + All RockRats System Activity")
        RockRatsActivity.Items.Add("Logon + Jumps in RockRats Systems")
        RockRatsActivity.Items.Add("Logon + Docks in RockRats Systems")
        RockRatsActivity.Items.Add("Logon location only")
        RockRatsActivity.Items.Add("No Update")
        Dim procActivity As String = getParameter("UpdateSiteActivity")
        If procActivity = "A" Then
            RockRatsActivity.SelectedIndex = 0
        ElseIf procActivity = "N" Then
            RockRatsActivity.SelectedIndex = 4
        ElseIf procActivity = "J" Then
            RockRatsActivity.SelectedIndex = 1
        ElseIf procActivity = "D" Then
            RockRatsActivity.SelectedIndex = 2
        Else
            RockRatsActivity.SelectedIndex = 3
        End If
        Version.Text = "Version: " & clientVersion
        logOutput("Version: " & clientVersion)
        logOutput("AppData: " & AppDataDir)
        Me.Refresh()                     ' Ensure the app is fully loaded before 
        LoadTimer.Enabled = True         ' opening comms - avoids possible exceptions.
    End Sub

    Private Async Sub LoadTimer_Tick(sender As Object, e As EventArgs) Handles LoadTimer.Tick
        LoadTimer.Enabled = False
        ConnStatus1.Text = "Connecting..."
        ConnStatus1.Refresh()
        Me.Refresh()
        Dim CanConnect As Boolean = Await Comms.TestConn()
        commsTimer.Enabled = True
    End Sub

    Private Sub Username_TextChanged(sender As Object, e As EventArgs) Handles Username.TextChanged
        SaveConnDetails.Enabled = True
    End Sub

    Private Sub SiteKey_TextChanged(sender As Object, e As EventArgs) Handles SiteKey.TextChanged
        SaveConnDetails.Enabled = True
    End Sub

    Private Sub SaveConnDetails_Click(sender As Object, e As EventArgs) Handles SaveConnDetails.Click
        Parameters.setParameter("Username", Username.Text)
        Parameters.setParameter("SiteKey", SiteKey.Text)
        SaveConnDetails.Enabled = False
        TestConnection.Enabled = True
    End Sub

    Private Async Sub TestConnection_Click(sender As Object, e As EventArgs) Handles TestConnection.Click
        Dim CanConnect As Boolean = Await TestConn()
        TestConnection.Enabled = False
    End Sub

    Private Sub BrowserForDir_Click(sender As Object, e As EventArgs) Handles BrowserForDir.Click
        If FolderBrowser.ShowDialog() = DialogResult.OK Then
            JournalFolder.Text = FolderBrowser.SelectedPath
        End If
    End Sub

    Friend Sub toggleTailLog()
        If tailTimer.Enabled = False Then
            logOutput("Startup Journal Monitor")
            If Files.idLastJournal() Then
                tailTimer.Enabled = True
                tailLogs.Text = "Stop"
            Else
                logOutput("Unable to identify Journal Logs")
            End If
        Else
            tailTimer.Enabled = False
            tailLogs.Text = "Run"
            FileStatus.Text = "Idle"
            Files.stopJournal()
            logOutput("Shutdown Journal Monitor")
        End If
        tailLogs.Enabled = True
    End Sub

    Private Sub JournalFolder_TextChanged(sender As Object, e As EventArgs) Handles JournalFolder.TextChanged
        SaveJournalDir.Enabled = True
    End Sub

    Private Sub SaveJournalDir_Click(sender As Object, e As EventArgs) Handles SaveJournalDir.Click
        Parameters.setParameter("JournalDirectory", JournalFolder.Text)
        Dim procActivity As String = "O"
        If RockRatsActivity.SelectedIndex = 0 Then
            procActivity = "A"
        ElseIf RockRatsActivity.SelectedIndex = 4 Then
            procActivity = "N"
        ElseIf RockRatsActivity.SelectedIndex = 1 Then
            procActivity = "J"
        ElseIf RockRatsActivity.SelectedIndex = 2 Then
            procActivity = "D"
        End If
        setParameter("UpdateSiteActivity", procActivity)
        SaveJournalDir.Enabled = False
    End Sub

    Private Sub tailLogs_Click_1(sender As Object, e As EventArgs) Handles tailLogs.Click
        toggleTailLog()
    End Sub

    Public Sub LogEverywhere(message As String)
        StatusLog(message)
        logOutput(message)
    End Sub
    Public Sub StatusLog(message As String)
        If Not String.IsNullOrEmpty(StatusBox.Text) Then
            message = vbNewLine & message
        End If
        StatusBox.AppendText(message)
    End Sub

    Friend Sub logOutput(logText As String)
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
        StatusLog("OCR Scan in progress...")
        Dim bounds As Rectangle
        Dim screenshot As System.Drawing.Bitmap
        Dim graph As Graphics
        Dim overScan As Double = CInt(Parameters.getParameter("overScan")) / 100
        bounds = Screen.PrimaryScreen.Bounds
        screenshot = New System.Drawing.Bitmap(CInt((bounds.Width * overScan) / 2.5), CInt(bounds.Height * overScan), System.Drawing.Imaging.PixelFormat.Format32bppArgb) ' Format32bppArgb
        graph = Graphics.FromImage(screenshot)
        graph.CopyFromScreen(CInt(bounds.X * overScan), CInt(bounds.Y * overScan), 0, 0, bounds.Size, CopyPixelOperation.SourceCopy)
        completeEDScreen(screenshot)
    End Sub

    Private Sub PasteEDScreen_Click(sender As Object, e As EventArgs) Handles PasteEDScreen.Click
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
        If resize > 1 Then
            Dim procBitmap As New Bitmap(CInt(screenshot.Width * resize), CInt(screenshot.Height * resize))
            Dim grBitmap As Graphics = Graphics.FromImage(procBitmap)
            grBitmap.DrawImage(screenshot, 0, 0, procBitmap.Width + 1, procBitmap.Height + 1)
            If BlackAndWhile.Checked Then
                Dim procImg As Bitmap = toGrayScale(procBitmap)
                Call Global.RockRatsClient.procEDScreen(procImg)
                EDCapture.Image = procImg
            Else
                Call Global.RockRatsClient.procEDScreen(procBitmap)
                EDCapture.Image = procBitmap
            End If
        ElseIf BlackAndWhile.Checked Then
            Dim procImg As Bitmap = toGrayScale(screenshot)
            Call Global.RockRatsClient.procEDScreen(procImg)
            EDCapture.Image = procImg
        Else
            Call Global.RockRatsClient.procEDScreen(screenshot)
            EDCapture.Image = screenshot
        End If
        EDCapture.Refresh()
        ocrWorking.Visible = False
        StatusLog("OCR Finished")
        Call Global.RockRatsClient.procOCRTextChg()
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
            If Not SoftData.hasUserFinishedOCRing() Then
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
            If row.Cells(0).Value IsNot Nothing Then
                If row.Cells(1).Value IsNot Nothing Then
                    If row.Cells(0).Value.ToString.Trim <> "" Then
                        If IsNumeric(row.Cells(1).Value.ToString) Then
                            If row.Cells(2).Value Is Nothing Then
                                row.Cells(2).Value = ""
                            End If
                            Dim cwaitForCompletion As Boolean = Comms.sendUpdate("", "", "", selSystem.SelectedItem.ToString + ":" + row.Cells(0).Value.ToString.ToUpper + ":" + row.Cells(2).Value.ToString + ":" + row.Cells(1).Value.ToString + ":OCR")
                            factionsSent += 1
                        Else
                            LogEverywhere("Skipping " & row.Cells(0).Value.ToString & " because the infuence given is not a number")
                        End If
                    End If
                Else
                    LogEverywhere("Skipping " & row.Cells(0).Value.ToString & " because no influence was present")
                End If
            End If
        Next
        logOutput("Updated " & factionsSent & "/" & (SoftDataGrid.Rows.Count - 1).ToString & " Factions in " & selSystem.SelectedItem.ToString)
        'Call Global.RockRatsClient.procOCRTextChg()
    End Sub

    Private Async Sub commsTimer_Tick(sender As Object, e As EventArgs) Handles commsTimer.Tick
        Try
            Await Comms.procUpdate()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub selSystem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles selSystem.SelectedIndexChanged
        If selSystem.SelectedItem.ToString <> "" Then
            ResetSystemNameBox(selSystem.SelectedItem.ToString)
            My.Computer.Clipboard.SetText(SystemNameBox.Text)
            StatusLog("Copied '" & selSystem.SelectedItem.ToString & "' to clipboard!")
            Call Global.RockRatsClient.procSystemChange(selSystem.SelectedItem.ToString)
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
        Call Global.RockRatsClient.procOCRTextChg()
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

    Private Sub RockRatsActivity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RockRatsActivity.SelectedIndexChanged
        SaveJournalDir.Enabled = True
    End Sub

    Private Sub SoftDataGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles SoftDataGrid.CellContentClick

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub SoftDataTab_Click(sender As Object, e As EventArgs) Handles SoftDataTab.Click

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub Version_Click(sender As Object, e As EventArgs) Handles Version.Click

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
End Class
