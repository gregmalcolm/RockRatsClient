<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RockRatsClient
    Inherits Global.System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <Global.System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As Global.System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <Global.System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RockRatsClient))
        Me.tailTimer = New System.Windows.Forms.Timer(Me.components)
        Me.LoadTimer = New System.Windows.Forms.Timer(Me.components)
        Me.JourneyDir = New System.DirectoryServices.DirectoryEntry()
        Me.FolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.commsTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.HelpTab = New System.Windows.Forms.TabPage()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Version = New System.Windows.Forms.Label()
        Me.LogTab = New System.Windows.Forms.TabPage()
        Me.logOut = New System.Windows.Forms.RichTextBox()
        Me.SettingsTab = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SaveConnDetails = New System.Windows.Forms.Button()
        Me.Username = New System.Windows.Forms.TextBox()
        Me.SiteKey = New System.Windows.Forms.TextBox()
        Me.TestConnection = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.JournalFolder = New System.Windows.Forms.TextBox()
        Me.BrowserForDir = New System.Windows.Forms.Button()
        Me.SaveJournalDir = New System.Windows.Forms.Button()
        Me.RockRatsActivity = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.ChatTab = New System.Windows.Forms.TabPage()
        Me.chatOut = New System.Windows.Forms.RichTextBox()
        Me.SoftDataTab = New System.Windows.Forms.TabPage()
        Me.EDCapture = New System.Windows.Forms.PictureBox()
        Me.CaptureEDScreen = New System.Windows.Forms.Button()
        Me.UpdSoftData = New System.Windows.Forms.Button()
        Me.selSystem = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.infTotal = New System.Windows.Forms.Label()
        Me.PasteEDScreen = New System.Windows.Forms.Button()
        Me.ocrWorking = New System.Windows.Forms.Panel()
        Me.statusLabel = New System.Windows.Forms.Label()
        Me.infTotalVal = New System.Windows.Forms.Label()
        Me.SoftDataGrid = New System.Windows.Forms.DataGridView()
        Me.Found = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.State = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Influence = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Faction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BlackAndWhile = New System.Windows.Forms.CheckBox()
        Me.resizeSlider = New System.Windows.Forms.TrackBar()
        Me.resizeValue = New System.Windows.Forms.Label()
        Me.onTop = New System.Windows.Forms.CheckBox()
        Me.StatusTab = New System.Windows.Forms.TabPage()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ConnStatus1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.FileStatus = New System.Windows.Forms.Label()
        Me.tailLogs = New System.Windows.Forms.Button()
        Me.ConnStatus2 = New System.Windows.Forms.Label()
        Me.CommanderLabel = New System.Windows.Forms.Label()
        Me.ShipLabel = New System.Windows.Forms.Label()
        Me.SystemLabel = New System.Windows.Forms.Label()
        Me.CommanderName = New System.Windows.Forms.Label()
        Me.ShipName = New System.Windows.Forms.Label()
        Me.SystemName = New System.Windows.Forms.Label()
        Me.SystemsList = New System.Windows.Forms.ListBox()
        Me.Tabs = New System.Windows.Forms.TabControl()
        Me.HelpTab.SuspendLayout()
        Me.LogTab.SuspendLayout()
        Me.SettingsTab.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ChatTab.SuspendLayout()
        Me.SoftDataTab.SuspendLayout()
        CType(Me.EDCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ocrWorking.SuspendLayout()
        CType(Me.SoftDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.resizeSlider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusTab.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Tabs.SuspendLayout()
        Me.SuspendLayout()
        '
        'tailTimer
        '
        Me.tailTimer.Interval = 4000
        '
        'LoadTimer
        '
        Me.LoadTimer.Interval = 2400
        '
        'commsTimer
        '
        Me.commsTimer.Interval = 250
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 400
        Me.ToolTip1.ReshowDelay = 100
        '
        'HelpTab
        '
        Me.HelpTab.Controls.Add(Me.Version)
        Me.HelpTab.Controls.Add(Me.Label15)
        Me.HelpTab.Controls.Add(Me.Label16)
        Me.HelpTab.Controls.Add(Me.Label13)
        Me.HelpTab.Controls.Add(Me.Label14)
        Me.HelpTab.Controls.Add(Me.Label12)
        Me.HelpTab.Controls.Add(Me.Label11)
        Me.HelpTab.Location = New System.Drawing.Point(4, 22)
        Me.HelpTab.Name = "HelpTab"
        Me.HelpTab.Size = New System.Drawing.Size(622, 371)
        Me.HelpTab.TabIndex = 4
        Me.HelpTab.Text = "Help"
        Me.HelpTab.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(264, 14)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(46, 18)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "About"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(57, 37)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(553, 91)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = resources.GetString("Label12.Text")
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(248, 143)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(84, 18)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Connection"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(60, 166)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(86, 13)
        Me.Label13.TabIndex = 3
        Me.Label13.Text = "Not implemented"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(253, 224)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(70, 18)
        Me.Label16.TabIndex = 4
        Me.Label16.Text = "Soft Data"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(39, 247)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(491, 78)
        Me.Label15.TabIndex = 5
        Me.Label15.Text = resources.GetString("Label15.Text")
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Version
        '
        Me.Version.AutoSize = True
        Me.Version.Location = New System.Drawing.Point(245, 336)
        Me.Version.Name = "Version"
        Me.Version.Size = New System.Drawing.Size(78, 13)
        Me.Version.TabIndex = 21
        Me.Version.Text = "Version 2.5.0.4"
        '
        'LogTab
        '
        Me.LogTab.Controls.Add(Me.logOut)
        Me.LogTab.Location = New System.Drawing.Point(4, 22)
        Me.LogTab.Name = "LogTab"
        Me.LogTab.Padding = New System.Windows.Forms.Padding(3)
        Me.LogTab.Size = New System.Drawing.Size(622, 371)
        Me.LogTab.TabIndex = 1
        Me.LogTab.Text = "Log"
        Me.LogTab.UseVisualStyleBackColor = True
        '
        'logOut
        '
        Me.logOut.Dock = System.Windows.Forms.DockStyle.Fill
        Me.logOut.Location = New System.Drawing.Point(3, 3)
        Me.logOut.Name = "logOut"
        Me.logOut.ReadOnly = True
        Me.logOut.Size = New System.Drawing.Size(616, 365)
        Me.logOut.TabIndex = 0
        Me.logOut.Text = ""
        '
        'SettingsTab
        '
        Me.SettingsTab.Controls.Add(Me.Panel2)
        Me.SettingsTab.Controls.Add(Me.Panel1)
        Me.SettingsTab.Location = New System.Drawing.Point(4, 22)
        Me.SettingsTab.Name = "SettingsTab"
        Me.SettingsTab.Size = New System.Drawing.Size(622, 371)
        Me.SettingsTab.TabIndex = 2
        Me.SettingsTab.Text = "Settings"
        Me.SettingsTab.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TestConnection)
        Me.Panel1.Controls.Add(Me.SiteKey)
        Me.Panel1.Controls.Add(Me.Username)
        Me.Panel1.Controls.Add(Me.SaveConnDetails)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(73, 24)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(476, 149)
        Me.Panel1.TabIndex = 14
        Me.Panel1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(182, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Connection Details"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(26, 34)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "User:"
        Me.ToolTip1.SetToolTip(Me.Label4, "Your RockRats Site username")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 60)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Site Key:"
        Me.ToolTip1.SetToolTip(Me.Label5, "Get your Site Key at www.sepp.space" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "On the site, edit your profile and go to you" &
        "r Account tab")
        '
        'SaveConnDetails
        '
        Me.SaveConnDetails.Enabled = False
        Me.SaveConnDetails.Location = New System.Drawing.Point(86, 89)
        Me.SaveConnDetails.Name = "SaveConnDetails"
        Me.SaveConnDetails.Size = New System.Drawing.Size(132, 41)
        Me.SaveConnDetails.TabIndex = 3
        Me.SaveConnDetails.Text = "Save"
        Me.ToolTip1.SetToolTip(Me.SaveConnDetails, "Save your Connection details to the ini file" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Location is reported in the Log")
        Me.SaveConnDetails.UseVisualStyleBackColor = True
        '
        'Username
        '
        Me.Username.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Username.Location = New System.Drawing.Point(58, 31)
        Me.Username.Name = "Username"
        Me.Username.Size = New System.Drawing.Size(397, 21)
        Me.Username.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.Username, "Your RockRats Site username")
        '
        'SiteKey
        '
        Me.SiteKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SiteKey.Location = New System.Drawing.Point(58, 57)
        Me.SiteKey.Name = "SiteKey"
        Me.SiteKey.Size = New System.Drawing.Size(397, 21)
        Me.SiteKey.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.SiteKey, "Get your Site Key at www.sepp.space" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "On the site, edit your profile and go to you" &
        "r Account tab")
        '
        'TestConnection
        '
        Me.TestConnection.Enabled = False
        Me.TestConnection.Location = New System.Drawing.Point(259, 89)
        Me.TestConnection.Name = "TestConnection"
        Me.TestConnection.Size = New System.Drawing.Size(132, 41)
        Me.TestConnection.TabIndex = 10
        Me.TestConnection.Text = "Connect"
        Me.ToolTip1.SetToolTip(Me.TestConnection, "Connect to the RockRats site")
        Me.TestConnection.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label20)
        Me.Panel2.Controls.Add(Me.RockRatsActivity)
        Me.Panel2.Controls.Add(Me.SaveJournalDir)
        Me.Panel2.Controls.Add(Me.BrowserForDir)
        Me.Panel2.Controls.Add(Me.JournalFolder)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Location = New System.Drawing.Point(73, 195)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(476, 128)
        Me.Panel2.TabIndex = 22
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(197, 8)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 15)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Journal Folder"
        '
        'JournalFolder
        '
        Me.JournalFolder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.JournalFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.JournalFolder.Location = New System.Drawing.Point(15, 34)
        Me.JournalFolder.Multiline = True
        Me.JournalFolder.Name = "JournalFolder"
        Me.JournalFolder.ReadOnly = True
        Me.JournalFolder.Size = New System.Drawing.Size(387, 28)
        Me.JournalFolder.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.JournalFolder, "Set the location of your ED Journal")
        Me.JournalFolder.WordWrap = False
        '
        'BrowserForDir
        '
        Me.BrowserForDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BrowserForDir.Location = New System.Drawing.Point(408, 34)
        Me.BrowserForDir.Name = "BrowserForDir"
        Me.BrowserForDir.Size = New System.Drawing.Size(47, 28)
        Me.BrowserForDir.TabIndex = 24
        Me.BrowserForDir.Text = "..."
        Me.ToolTip1.SetToolTip(Me.BrowserForDir, "Select the location of your ED Journal")
        Me.BrowserForDir.UseVisualStyleBackColor = True
        '
        'SaveJournalDir
        '
        Me.SaveJournalDir.Enabled = False
        Me.SaveJournalDir.Location = New System.Drawing.Point(259, 72)
        Me.SaveJournalDir.Name = "SaveJournalDir"
        Me.SaveJournalDir.Size = New System.Drawing.Size(132, 41)
        Me.SaveJournalDir.TabIndex = 26
        Me.SaveJournalDir.Text = "Save"
        Me.ToolTip1.SetToolTip(Me.SaveJournalDir, "Save your Journal Location details to the ini file" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Location is reported in the L" &
        "og")
        Me.SaveJournalDir.UseVisualStyleBackColor = True
        '
        'RockRatsActivity
        '
        Me.RockRatsActivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RockRatsActivity.FormattingEnabled = True
        Me.RockRatsActivity.Location = New System.Drawing.Point(58, 92)
        Me.RockRatsActivity.Name = "RockRatsActivity"
        Me.RockRatsActivity.Size = New System.Drawing.Size(175, 21)
        Me.RockRatsActivity.TabIndex = 27
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(80, 72)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(155, 13)
        Me.Label20.TabIndex = 28
        Me.Label20.Text = "RockRats Site - Activity Stream"
        '
        'ChatTab
        '
        Me.ChatTab.Controls.Add(Me.chatOut)
        Me.ChatTab.Location = New System.Drawing.Point(4, 22)
        Me.ChatTab.Name = "ChatTab"
        Me.ChatTab.Size = New System.Drawing.Size(622, 371)
        Me.ChatTab.TabIndex = 5
        Me.ChatTab.Text = "Chat"
        Me.ChatTab.UseVisualStyleBackColor = True
        '
        'chatOut
        '
        Me.chatOut.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chatOut.Location = New System.Drawing.Point(0, 0)
        Me.chatOut.Name = "chatOut"
        Me.chatOut.ReadOnly = True
        Me.chatOut.Size = New System.Drawing.Size(622, 371)
        Me.chatOut.TabIndex = 0
        Me.chatOut.Text = ""
        '
        'SoftDataTab
        '
        Me.SoftDataTab.Controls.Add(Me.onTop)
        Me.SoftDataTab.Controls.Add(Me.resizeValue)
        Me.SoftDataTab.Controls.Add(Me.resizeSlider)
        Me.SoftDataTab.Controls.Add(Me.BlackAndWhile)
        Me.SoftDataTab.Controls.Add(Me.SoftDataGrid)
        Me.SoftDataTab.Controls.Add(Me.infTotalVal)
        Me.SoftDataTab.Controls.Add(Me.ocrWorking)
        Me.SoftDataTab.Controls.Add(Me.PasteEDScreen)
        Me.SoftDataTab.Controls.Add(Me.infTotal)
        Me.SoftDataTab.Controls.Add(Me.Label10)
        Me.SoftDataTab.Controls.Add(Me.Label8)
        Me.SoftDataTab.Controls.Add(Me.Label7)
        Me.SoftDataTab.Controls.Add(Me.selSystem)
        Me.SoftDataTab.Controls.Add(Me.UpdSoftData)
        Me.SoftDataTab.Controls.Add(Me.CaptureEDScreen)
        Me.SoftDataTab.Controls.Add(Me.EDCapture)
        Me.SoftDataTab.Location = New System.Drawing.Point(4, 22)
        Me.SoftDataTab.Name = "SoftDataTab"
        Me.SoftDataTab.Size = New System.Drawing.Size(622, 371)
        Me.SoftDataTab.TabIndex = 3
        Me.SoftDataTab.Text = "Soft Data"
        Me.SoftDataTab.UseVisualStyleBackColor = True
        '
        'EDCapture
        '
        Me.EDCapture.BackColor = System.Drawing.Color.Transparent
        Me.EDCapture.Location = New System.Drawing.Point(8, 67)
        Me.EDCapture.Name = "EDCapture"
        Me.EDCapture.Size = New System.Drawing.Size(187, 132)
        Me.EDCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EDCapture.TabIndex = 0
        Me.EDCapture.TabStop = False
        Me.ToolTip1.SetToolTip(Me.EDCapture, "Click on the Image to see full view")
        '
        'CaptureEDScreen
        '
        Me.CaptureEDScreen.Enabled = False
        Me.CaptureEDScreen.Location = New System.Drawing.Point(23, 295)
        Me.CaptureEDScreen.Name = "CaptureEDScreen"
        Me.CaptureEDScreen.Size = New System.Drawing.Size(77, 23)
        Me.CaptureEDScreen.TabIndex = 23
        Me.CaptureEDScreen.Text = "Capture"
        Me.ToolTip1.SetToolTip(Me.CaptureEDScreen, "Capture the Image from your Main Screen")
        Me.CaptureEDScreen.UseVisualStyleBackColor = True
        '
        'UpdSoftData
        '
        Me.UpdSoftData.Enabled = False
        Me.UpdSoftData.Location = New System.Drawing.Point(395, 295)
        Me.UpdSoftData.Name = "UpdSoftData"
        Me.UpdSoftData.Size = New System.Drawing.Size(77, 23)
        Me.UpdSoftData.TabIndex = 25
        Me.UpdSoftData.Text = "Update"
        Me.ToolTip1.SetToolTip(Me.UpdSoftData, "Update RockRats Soft Data" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Only available when Influence Total is 100%")
        Me.UpdSoftData.UseVisualStyleBackColor = True
        '
        'selSystem
        '
        Me.selSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.selSystem.FormattingEnabled = True
        Me.selSystem.Location = New System.Drawing.Point(8, 40)
        Me.selSystem.Name = "selSystem"
        Me.selSystem.Size = New System.Drawing.Size(187, 21)
        Me.selSystem.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.selSystem, "Select the system for Soft Data update")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(59, 20)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 13)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "1. Select System"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(36, 323)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(128, 13)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "2. Capture (Multiple times)"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(406, 323)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(54, 13)
        Me.Label10.TabIndex = 31
        Me.Label10.Text = "3. Update"
        '
        'infTotal
        '
        Me.infTotal.AutoSize = True
        Me.infTotal.ForeColor = System.Drawing.Color.DarkRed
        Me.infTotal.Location = New System.Drawing.Point(525, 295)
        Me.infTotal.Name = "infTotal"
        Me.infTotal.Size = New System.Drawing.Size(78, 13)
        Me.infTotal.TabIndex = 32
        Me.infTotal.Text = "Influence Total"
        Me.ToolTip1.SetToolTip(Me.infTotal, "Influence Total has to be 100% before update is enabled")
        '
        'PasteEDScreen
        '
        Me.PasteEDScreen.Enabled = False
        Me.PasteEDScreen.Location = New System.Drawing.Point(106, 295)
        Me.PasteEDScreen.Name = "PasteEDScreen"
        Me.PasteEDScreen.Size = New System.Drawing.Size(75, 23)
        Me.PasteEDScreen.TabIndex = 33
        Me.PasteEDScreen.Text = "Paste"
        Me.ToolTip1.SetToolTip(Me.PasteEDScreen, "Paste an Image from your Clipboard")
        Me.PasteEDScreen.UseVisualStyleBackColor = True
        '
        'ocrWorking
        '
        Me.ocrWorking.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ocrWorking.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ocrWorking.Controls.Add(Me.statusLabel)
        Me.ocrWorking.Location = New System.Drawing.Point(8, 81)
        Me.ocrWorking.Name = "ocrWorking"
        Me.ocrWorking.Size = New System.Drawing.Size(187, 100)
        Me.ocrWorking.TabIndex = 34
        Me.ocrWorking.Visible = False
        '
        'statusLabel
        '
        Me.statusLabel.AutoSize = True
        Me.statusLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.statusLabel.ForeColor = System.Drawing.Color.DarkGreen
        Me.statusLabel.Location = New System.Drawing.Point(37, 38)
        Me.statusLabel.Name = "statusLabel"
        Me.statusLabel.Size = New System.Drawing.Size(79, 20)
        Me.statusLabel.TabIndex = 0
        Me.statusLabel.Text = "Working..."
        '
        'infTotalVal
        '
        Me.infTotalVal.AutoSize = True
        Me.infTotalVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.infTotalVal.ForeColor = System.Drawing.Color.DarkRed
        Me.infTotalVal.Location = New System.Drawing.Point(552, 313)
        Me.infTotalVal.Name = "infTotalVal"
        Me.infTotalVal.Size = New System.Drawing.Size(16, 18)
        Me.infTotalVal.TabIndex = 36
        Me.infTotalVal.Text = "0"
        Me.ToolTip1.SetToolTip(Me.infTotalVal, "Influence Total has to be 100% before update is enabled")
        '
        'SoftDataGrid
        '
        Me.SoftDataGrid.AllowUserToAddRows = False
        Me.SoftDataGrid.AllowUserToDeleteRows = False
        Me.SoftDataGrid.AllowUserToOrderColumns = True
        Me.SoftDataGrid.AllowUserToResizeRows = False
        Me.SoftDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SoftDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Faction, Me.Influence, Me.State, Me.Found})
        Me.SoftDataGrid.Location = New System.Drawing.Point(245, 20)
        Me.SoftDataGrid.Name = "SoftDataGrid"
        Me.SoftDataGrid.RowHeadersWidth = 4
        Me.SoftDataGrid.Size = New System.Drawing.Size(358, 270)
        Me.SoftDataGrid.TabIndex = 37
        '
        'Found
        '
        Me.Found.HeaderText = "Found"
        Me.Found.Name = "Found"
        Me.Found.Width = 50
        '
        'State
        '
        Me.State.HeaderText = "State"
        Me.State.Name = "State"
        Me.State.Width = 76
        '
        'Influence
        '
        Me.Influence.HeaderText = "Influence"
        Me.Influence.MaxInputLength = 5
        Me.Influence.Name = "Influence"
        Me.Influence.Width = 55
        '
        'Faction
        '
        Me.Faction.HeaderText = "Faction"
        Me.Faction.MaxInputLength = 250
        Me.Faction.Name = "Faction"
        Me.Faction.Width = 170
        '
        'BlackAndWhile
        '
        Me.BlackAndWhile.AutoSize = True
        Me.BlackAndWhile.Location = New System.Drawing.Point(61, 260)
        Me.BlackAndWhile.Name = "BlackAndWhile"
        Me.BlackAndWhile.Size = New System.Drawing.Size(78, 17)
        Me.BlackAndWhile.TabIndex = 39
        Me.BlackAndWhile.Text = "Grey Scale"
        Me.ToolTip1.SetToolTip(Me.BlackAndWhile, "Covert the Image to Grey Scale prior to OCR processing" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This can improve OCR accu" &
        "racy but at a minor delay")
        Me.BlackAndWhile.UseVisualStyleBackColor = True
        '
        'resizeSlider
        '
        Me.resizeSlider.BackColor = System.Drawing.SystemColors.Window
        Me.resizeSlider.LargeChange = 4
        Me.resizeSlider.Location = New System.Drawing.Point(8, 205)
        Me.resizeSlider.Maximum = 12
        Me.resizeSlider.Name = "resizeSlider"
        Me.resizeSlider.Size = New System.Drawing.Size(187, 45)
        Me.resizeSlider.TabIndex = 40
        Me.resizeSlider.TickFrequency = 2
        Me.ToolTip1.SetToolTip(Me.resizeSlider, "Resize the image prior to OCR processing" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This can improve OCR accuracy but at a " &
        "slight delay")
        Me.resizeSlider.Value = 4
        '
        'resizeValue
        '
        Me.resizeValue.AutoSize = True
        Me.resizeValue.Location = New System.Drawing.Point(49, 237)
        Me.resizeValue.Name = "resizeValue"
        Me.resizeValue.Size = New System.Drawing.Size(65, 13)
        Me.resizeValue.TabIndex = 41
        Me.resizeValue.Text = "Resize: 2.0x"
        '
        'onTop
        '
        Me.onTop.AutoSize = True
        Me.onTop.Location = New System.Drawing.Point(300, 301)
        Me.onTop.Name = "onTop"
        Me.onTop.Size = New System.Drawing.Size(60, 30)
        Me.onTop.TabIndex = 42
        Me.onTop.Text = "Always" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "on Top"
        Me.onTop.UseVisualStyleBackColor = True
        '
        'StatusTab
        '
        Me.StatusTab.Controls.Add(Me.SystemsList)
        Me.StatusTab.Controls.Add(Me.Panel3)
        Me.StatusTab.Controls.Add(Me.Label6)
        Me.StatusTab.Location = New System.Drawing.Point(4, 22)
        Me.StatusTab.Name = "StatusTab"
        Me.StatusTab.Padding = New System.Windows.Forms.Padding(3)
        Me.StatusTab.Size = New System.Drawing.Size(622, 371)
        Me.StatusTab.TabIndex = 0
        Me.StatusTab.Text = "Status"
        Me.StatusTab.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(469, 17)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(123, 16)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "RockRats Systems"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.SystemName)
        Me.Panel3.Controls.Add(Me.ShipName)
        Me.Panel3.Controls.Add(Me.CommanderName)
        Me.Panel3.Controls.Add(Me.SystemLabel)
        Me.Panel3.Controls.Add(Me.ShipLabel)
        Me.Panel3.Controls.Add(Me.CommanderLabel)
        Me.Panel3.Controls.Add(Me.ConnStatus2)
        Me.Panel3.Controls.Add(Me.tailLogs)
        Me.Panel3.Controls.Add(Me.FileStatus)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.ConnStatus1)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Location = New System.Drawing.Point(9, 12)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(399, 348)
        Me.Panel3.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 102)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(141, 20)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Connection Status"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ConnStatus1
        '
        Me.ConnStatus1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ConnStatus1.AutoSize = True
        Me.ConnStatus1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ConnStatus1.ForeColor = System.Drawing.Color.DarkCyan
        Me.ConnStatus1.Location = New System.Drawing.Point(35, 125)
        Me.ConnStatus1.Name = "ConnStatus1"
        Me.ConnStatus1.Size = New System.Drawing.Size(88, 15)
        Me.ConnStatus1.TabIndex = 21
        Me.ConnStatus1.Text = "Not Connected"
        Me.ConnStatus1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 222)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 20)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Journal Status"
        '
        'FileStatus
        '
        Me.FileStatus.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FileStatus.AutoSize = True
        Me.FileStatus.ForeColor = System.Drawing.Color.DarkCyan
        Me.FileStatus.Location = New System.Drawing.Point(36, 244)
        Me.FileStatus.Name = "FileStatus"
        Me.FileStatus.Size = New System.Drawing.Size(24, 13)
        Me.FileStatus.TabIndex = 23
        Me.FileStatus.Text = "Idle"
        Me.FileStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tailLogs
        '
        Me.tailLogs.Enabled = False
        Me.tailLogs.Location = New System.Drawing.Point(66, 282)
        Me.tailLogs.Name = "tailLogs"
        Me.tailLogs.Size = New System.Drawing.Size(270, 63)
        Me.tailLogs.TabIndex = 24
        Me.tailLogs.Text = "Run"
        Me.ToolTip1.SetToolTip(Me.tailLogs, "Process new Journal Entries")
        Me.tailLogs.UseVisualStyleBackColor = True
        '
        'ConnStatus2
        '
        Me.ConnStatus2.AutoSize = True
        Me.ConnStatus2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ConnStatus2.Location = New System.Drawing.Point(35, 151)
        Me.ConnStatus2.Name = "ConnStatus2"
        Me.ConnStatus2.Size = New System.Drawing.Size(77, 15)
        Me.ConnStatus2.TabIndex = 25
        Me.ConnStatus2.Text = "ConnStatus2"
        '
        'CommanderLabel
        '
        Me.CommanderLabel.AutoSize = True
        Me.CommanderLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommanderLabel.Location = New System.Drawing.Point(6, 5)
        Me.CommanderLabel.Name = "CommanderLabel"
        Me.CommanderLabel.Size = New System.Drawing.Size(85, 16)
        Me.CommanderLabel.TabIndex = 26
        Me.CommanderLabel.Text = "Commander:"
        '
        'ShipLabel
        '
        Me.ShipLabel.AutoSize = True
        Me.ShipLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShipLabel.Location = New System.Drawing.Point(53, 31)
        Me.ShipLabel.Name = "ShipLabel"
        Me.ShipLabel.Size = New System.Drawing.Size(38, 16)
        Me.ShipLabel.TabIndex = 27
        Me.ShipLabel.Text = "Ship:"
        '
        'SystemLabel
        '
        Me.SystemLabel.AutoSize = True
        Me.SystemLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SystemLabel.Location = New System.Drawing.Point(35, 57)
        Me.SystemLabel.Name = "SystemLabel"
        Me.SystemLabel.Size = New System.Drawing.Size(56, 16)
        Me.SystemLabel.TabIndex = 28
        Me.SystemLabel.Text = "System:"
        '
        'CommanderName
        '
        Me.CommanderName.AutoSize = True
        Me.CommanderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommanderName.Location = New System.Drawing.Point(97, 5)
        Me.CommanderName.Name = "CommanderName"
        Me.CommanderName.Size = New System.Drawing.Size(119, 16)
        Me.CommanderName.TabIndex = 29
        Me.CommanderName.Text = "CommanderName"
        '
        'ShipName
        '
        Me.ShipName.AutoSize = True
        Me.ShipName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ShipName.Location = New System.Drawing.Point(97, 31)
        Me.ShipName.Name = "ShipName"
        Me.ShipName.Size = New System.Drawing.Size(72, 16)
        Me.ShipName.TabIndex = 30
        Me.ShipName.Text = "ShipName"
        '
        'SystemName
        '
        Me.SystemName.AutoSize = True
        Me.SystemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SystemName.Location = New System.Drawing.Point(97, 57)
        Me.SystemName.Name = "SystemName"
        Me.SystemName.Size = New System.Drawing.Size(90, 16)
        Me.SystemName.TabIndex = 31
        Me.SystemName.Text = "SystemName"
        '
        'SystemsList
        '
        Me.SystemsList.FormattingEnabled = True
        Me.SystemsList.Location = New System.Drawing.Point(414, 44)
        Me.SystemsList.Name = "SystemsList"
        Me.SystemsList.Size = New System.Drawing.Size(200, 316)
        Me.SystemsList.TabIndex = 22
        '
        'Tabs
        '
        Me.Tabs.Controls.Add(Me.StatusTab)
        Me.Tabs.Controls.Add(Me.SoftDataTab)
        Me.Tabs.Controls.Add(Me.ChatTab)
        Me.Tabs.Controls.Add(Me.SettingsTab)
        Me.Tabs.Controls.Add(Me.LogTab)
        Me.Tabs.Controls.Add(Me.HelpTab)
        Me.Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Tabs.Location = New System.Drawing.Point(0, 0)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(630, 397)
        Me.Tabs.TabIndex = 5
        '
        'RockRatsClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 397)
        Me.Controls.Add(Me.Tabs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "RockRatsClient"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Rock Rats Client"
        Me.HelpTab.ResumeLayout(False)
        Me.HelpTab.PerformLayout()
        Me.LogTab.ResumeLayout(False)
        Me.SettingsTab.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ChatTab.ResumeLayout(False)
        Me.SoftDataTab.ResumeLayout(False)
        Me.SoftDataTab.PerformLayout()
        CType(Me.EDCapture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ocrWorking.ResumeLayout(False)
        Me.ocrWorking.PerformLayout()
        CType(Me.SoftDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.resizeSlider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusTab.ResumeLayout(False)
        Me.StatusTab.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Tabs.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tailTimer As Timer
    Friend WithEvents LoadTimer As Timer
    Friend WithEvents JourneyDir As DirectoryServices.DirectoryEntry
    Friend WithEvents FolderBrowser As FolderBrowserDialog
    Friend WithEvents commsTimer As Timer
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents HelpTab As TabPage
    Friend WithEvents Version As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents LogTab As TabPage
    Friend WithEvents logOut As RichTextBox
    Friend WithEvents SettingsTab As TabPage
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label20 As Label
    Friend WithEvents RockRatsActivity As ComboBox
    Friend WithEvents SaveJournalDir As Button
    Friend WithEvents BrowserForDir As Button
    Friend WithEvents JournalFolder As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TestConnection As Button
    Friend WithEvents SiteKey As TextBox
    Friend WithEvents Username As TextBox
    Friend WithEvents SaveConnDetails As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ChatTab As TabPage
    Friend WithEvents chatOut As RichTextBox
    Friend WithEvents SoftDataTab As TabPage
    Friend WithEvents onTop As CheckBox
    Friend WithEvents resizeValue As Label
    Friend WithEvents resizeSlider As TrackBar
    Friend WithEvents BlackAndWhile As CheckBox
    Friend WithEvents SoftDataGrid As DataGridView
    Friend WithEvents Faction As DataGridViewTextBoxColumn
    Friend WithEvents Influence As DataGridViewTextBoxColumn
    Friend WithEvents State As DataGridViewTextBoxColumn
    Friend WithEvents Found As DataGridViewCheckBoxColumn
    Friend WithEvents infTotalVal As Label
    Friend WithEvents ocrWorking As Panel
    Friend WithEvents statusLabel As Label
    Friend WithEvents PasteEDScreen As Button
    Friend WithEvents infTotal As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents selSystem As ComboBox
    Friend WithEvents UpdSoftData As Button
    Friend WithEvents CaptureEDScreen As Button
    Friend WithEvents EDCapture As PictureBox
    Friend WithEvents StatusTab As TabPage
    Friend WithEvents SystemsList As ListBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents SystemName As Label
    Friend WithEvents ShipName As Label
    Friend WithEvents CommanderName As Label
    Friend WithEvents SystemLabel As Label
    Friend WithEvents ShipLabel As Label
    Friend WithEvents CommanderLabel As Label
    Friend WithEvents ConnStatus2 As Label
    Friend WithEvents tailLogs As Button
    Friend WithEvents FileStatus As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ConnStatus1 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Tabs As TabControl
End Class
