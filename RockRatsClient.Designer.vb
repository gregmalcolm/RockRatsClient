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
        Me.JourneyDir = New System.DirectoryServices.DirectoryEntry()
        Me.FolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.CommsTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BrowserForDir = New System.Windows.Forms.Button()
        Me.JournalFolder = New System.Windows.Forms.TextBox()
        Me.CaptureEDScreen = New System.Windows.Forms.Button()
        Me.UpdateBgsData = New System.Windows.Forms.Button()
        Me.SelectedSystem = New System.Windows.Forms.ComboBox()
        Me.InfTotal = New System.Windows.Forms.Label()
        Me.InfTotalVal = New System.Windows.Forms.Label()
        Me.ViewWebTracker = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.RemoveButton = New System.Windows.Forms.Button()
        Me.LogOcrCheckbox = New System.Windows.Forms.CheckBox()
        Me.ScanMarginLeft = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.EDCapture = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LogTab = New System.Windows.Forms.TabPage()
        Me.LogTextBox = New System.Windows.Forms.RichTextBox()
        Me.SettingsTab = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CommanderName = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.SoftDataTab = New System.Windows.Forms.TabPage()
        Me.StatusBox = New System.Windows.Forms.TextBox()
        Me.SystemNameBox = New System.Windows.Forms.TextBox()
        Me.SystemLabel = New System.Windows.Forms.Label()
        Me.SoftDataGrid = New System.Windows.Forms.DataGridView()
        Me.Faction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Influence = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.State = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrevInfluence = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.InfluenceDiff = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrevState = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Found = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.LoadingLabel = New System.Windows.Forms.Label()
        Me.AlwaysOnTopCheckbox = New System.Windows.Forms.CheckBox()
        Me.Tabs = New System.Windows.Forms.TabControl()
        CType(Me.EDCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LogTab.SuspendLayout()
        Me.SettingsTab.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SoftDataTab.SuspendLayout()
        CType(Me.SoftDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tabs.SuspendLayout()
        Me.SuspendLayout()
        '
        'CommsTimer
        '
        Me.CommsTimer.Interval = 3000
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 200
        Me.ToolTip1.InitialDelay = 400
        Me.ToolTip1.ReshowDelay = 100
        '
        'BrowserForDir
        '
        Me.BrowserForDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BrowserForDir.Location = New System.Drawing.Point(507, 16)
        Me.BrowserForDir.Name = "BrowserForDir"
        Me.BrowserForDir.Size = New System.Drawing.Size(36, 28)
        Me.BrowserForDir.TabIndex = 24
        Me.BrowserForDir.Text = "..."
        Me.ToolTip1.SetToolTip(Me.BrowserForDir, "Select the location of your ED Journal")
        Me.BrowserForDir.UseVisualStyleBackColor = True
        '
        'JournalFolder
        '
        Me.JournalFolder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.JournalFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.JournalFolder.Location = New System.Drawing.Point(110, 16)
        Me.JournalFolder.Multiline = True
        Me.JournalFolder.Name = "JournalFolder"
        Me.JournalFolder.Size = New System.Drawing.Size(394, 28)
        Me.JournalFolder.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.JournalFolder, "Set the location of your ED Journal")
        Me.JournalFolder.WordWrap = False
        '
        'CaptureEDScreen
        '
        Me.CaptureEDScreen.Location = New System.Drawing.Point(472, 255)
        Me.CaptureEDScreen.Name = "CaptureEDScreen"
        Me.CaptureEDScreen.Size = New System.Drawing.Size(77, 72)
        Me.CaptureEDScreen.TabIndex = 23
        Me.CaptureEDScreen.Text = "OCR System Factions"
        Me.ToolTip1.SetToolTip(Me.CaptureEDScreen, "Capture the Image from your Main Screen")
        Me.CaptureEDScreen.UseVisualStyleBackColor = True
        '
        'UpdateBgsData
        '
        Me.UpdateBgsData.Location = New System.Drawing.Point(472, 380)
        Me.UpdateBgsData.Name = "UpdateBgsData"
        Me.UpdateBgsData.Size = New System.Drawing.Size(77, 47)
        Me.UpdateBgsData.TabIndex = 25
        Me.UpdateBgsData.Text = "Update Server"
        Me.ToolTip1.SetToolTip(Me.UpdateBgsData, "Update RockRats Soft Data" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Only available when Influence Total is 100%")
        Me.UpdateBgsData.UseVisualStyleBackColor = True
        '
        'SelectedSystem
        '
        Me.SelectedSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SelectedSystem.FormattingEnabled = True
        Me.SelectedSystem.Items.AddRange(New Object() {"Loading..."})
        Me.SelectedSystem.Location = New System.Drawing.Point(48, 3)
        Me.SelectedSystem.Name = "SelectedSystem"
        Me.SelectedSystem.Size = New System.Drawing.Size(163, 21)
        Me.SelectedSystem.TabIndex = 28
        Me.SelectedSystem.TabStop = False
        Me.ToolTip1.SetToolTip(Me.SelectedSystem, "Select the system for Soft Data update")
        Me.SelectedSystem.Visible = False
        '
        'InfTotal
        '
        Me.InfTotal.AutoSize = True
        Me.InfTotal.ForeColor = System.Drawing.Color.DarkRed
        Me.InfTotal.Location = New System.Drawing.Point(471, 330)
        Me.InfTotal.Name = "InfTotal"
        Me.InfTotal.Size = New System.Drawing.Size(78, 13)
        Me.InfTotal.TabIndex = 32
        Me.InfTotal.Text = "Influence Total"
        Me.ToolTip1.SetToolTip(Me.InfTotal, "Influence Total has to be 100% before update is enabled")
        '
        'InfTotalVal
        '
        Me.InfTotalVal.AutoSize = True
        Me.InfTotalVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InfTotalVal.ForeColor = System.Drawing.Color.DarkRed
        Me.InfTotalVal.Location = New System.Drawing.Point(498, 344)
        Me.InfTotalVal.Name = "InfTotalVal"
        Me.InfTotalVal.Size = New System.Drawing.Size(16, 18)
        Me.InfTotalVal.TabIndex = 36
        Me.InfTotalVal.Text = "0"
        Me.ToolTip1.SetToolTip(Me.InfTotalVal, "Influence Total has to be 100% before update is enabled")
        '
        'ViewWebTracker
        '
        Me.ViewWebTracker.Location = New System.Drawing.Point(464, 33)
        Me.ViewWebTracker.Name = "ViewWebTracker"
        Me.ViewWebTracker.Size = New System.Drawing.Size(89, 20)
        Me.ViewWebTracker.TabIndex = 45
        Me.ViewWebTracker.Text = "Web Tracker"
        Me.ToolTip1.SetToolTip(Me.ViewWebTracker, "Update RockRats Soft Data" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Only available when Influence Total is 100%")
        Me.ViewWebTracker.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.Enabled = False
        Me.AddButton.Location = New System.Drawing.Point(154, 33)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(48, 20)
        Me.AddButton.TabIndex = 47
        Me.AddButton.Text = "Add"
        Me.ToolTip1.SetToolTip(Me.AddButton, "Add the entered system name to the list of official Rock Rat Systems")
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'RemoveButton
        '
        Me.RemoveButton.Enabled = False
        Me.RemoveButton.Location = New System.Drawing.Point(208, 33)
        Me.RemoveButton.Name = "RemoveButton"
        Me.RemoveButton.Size = New System.Drawing.Size(56, 20)
        Me.RemoveButton.TabIndex = 48
        Me.RemoveButton.Text = "Remove"
        Me.ToolTip1.SetToolTip(Me.RemoveButton, "Add the entered system name to the list of official Rock Rat Systems")
        Me.RemoveButton.UseVisualStyleBackColor = True
        '
        'LogOcrCheckbox
        '
        Me.LogOcrCheckbox.AutoSize = True
        Me.LogOcrCheckbox.Location = New System.Drawing.Point(9, 39)
        Me.LogOcrCheckbox.Name = "LogOcrCheckbox"
        Me.LogOcrCheckbox.Size = New System.Drawing.Size(146, 17)
        Me.LogOcrCheckbox.TabIndex = 51
        Me.LogOcrCheckbox.Text = "Log OCR text translations"
        Me.ToolTip1.SetToolTip(Me.LogOcrCheckbox, "Covert the Image to Grey Scale prior to OCR processing" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This can improve OCR accu" &
        "racy but at a minor delay")
        Me.LogOcrCheckbox.UseVisualStyleBackColor = True
        '
        'ScanMarginLeft
        '
        Me.ScanMarginLeft.Location = New System.Drawing.Point(140, 13)
        Me.ScanMarginLeft.Name = "ScanMarginLeft"
        Me.ScanMarginLeft.Size = New System.Drawing.Size(41, 20)
        Me.ScanMarginLeft.TabIndex = 56
        Me.ToolTip1.SetToolTip(Me.ScanMarginLeft, "Set how much of the screen to look at when capturing text. This will stop other t" &
        "ext like this window contaminating the results")
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 16)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(131, 13)
        Me.Label14.TabIndex = 57
        Me.Label14.Text = "Area of left screen to scan"
        Me.ToolTip1.SetToolTip(Me.Label14, "Set how much of the screen to look at when capturing text. This will stop other t" &
        "ext like this window contaminating the results")
        '
        'EDCapture
        '
        Me.EDCapture.BackColor = System.Drawing.Color.Transparent
        Me.EDCapture.Location = New System.Drawing.Point(6, 85)
        Me.EDCapture.Name = "EDCapture"
        Me.EDCapture.Size = New System.Drawing.Size(175, 247)
        Me.EDCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EDCapture.TabIndex = 54
        Me.EDCapture.TabStop = False
        Me.ToolTip1.SetToolTip(Me.EDCapture, "Click on the Image to see full view")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(251, 13)
        Me.Label1.TabIndex = 60
        Me.Label1.Text = "Last OCR Image (Check the Factions text is visible):"
        Me.ToolTip1.SetToolTip(Me.Label1, "Set how much of the screen to look at when capturing text. This will stop other t" &
        "ext like this window contaminating the results")
        '
        'LogTab
        '
        Me.LogTab.Controls.Add(Me.LogTextBox)
        Me.LogTab.Location = New System.Drawing.Point(4, 22)
        Me.LogTab.Name = "LogTab"
        Me.LogTab.Padding = New System.Windows.Forms.Padding(3)
        Me.LogTab.Size = New System.Drawing.Size(558, 446)
        Me.LogTab.TabIndex = 1
        Me.LogTab.Text = "Log"
        Me.LogTab.UseVisualStyleBackColor = True
        '
        'LogTextBox
        '
        Me.LogTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LogTextBox.Location = New System.Drawing.Point(3, 3)
        Me.LogTextBox.Name = "LogTextBox"
        Me.LogTextBox.ReadOnly = True
        Me.LogTextBox.Size = New System.Drawing.Size(552, 440)
        Me.LogTextBox.TabIndex = 0
        Me.LogTextBox.Text = ""
        '
        'SettingsTab
        '
        Me.SettingsTab.Controls.Add(Me.GroupBox2)
        Me.SettingsTab.Controls.Add(Me.GroupBox1)
        Me.SettingsTab.Location = New System.Drawing.Point(4, 22)
        Me.SettingsTab.Name = "SettingsTab"
        Me.SettingsTab.Size = New System.Drawing.Size(558, 446)
        Me.SettingsTab.TabIndex = 2
        Me.SettingsTab.Text = "Settings"
        Me.SettingsTab.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.CommanderName)
        Me.GroupBox2.Controls.Add(Me.JournalFolder)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.BrowserForDir)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(548, 90)
        Me.GroupBox2.TabIndex = 61
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Journal Scraping"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label2.Location = New System.Drawing.Point(10, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Commander Name"
        '
        'CommanderName
        '
        Me.CommanderName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CommanderName.Location = New System.Drawing.Point(111, 57)
        Me.CommanderName.Name = "CommanderName"
        Me.CommanderName.Size = New System.Drawing.Size(270, 21)
        Me.CommanderName.TabIndex = 32
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label9.Location = New System.Drawing.Point(42, 24)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Logs Folder"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.EDCapture)
        Me.GroupBox1.Controls.Add(Me.ScanMarginLeft)
        Me.GroupBox1.Controls.Add(Me.LogOcrCheckbox)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 105)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(276, 338)
        Me.GroupBox1.TabIndex = 59
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "OCR Calibration"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(183, 15)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(15, 13)
        Me.Label17.TabIndex = 59
        Me.Label17.Text = "%"
        '
        'SoftDataTab
        '
        Me.SoftDataTab.Controls.Add(Me.ViewWebTracker)
        Me.SoftDataTab.Controls.Add(Me.StatusBox)
        Me.SoftDataTab.Controls.Add(Me.SystemNameBox)
        Me.SoftDataTab.Controls.Add(Me.RemoveButton)
        Me.SoftDataTab.Controls.Add(Me.AddButton)
        Me.SoftDataTab.Controls.Add(Me.SystemLabel)
        Me.SoftDataTab.Controls.Add(Me.SoftDataGrid)
        Me.SoftDataTab.Controls.Add(Me.InfTotalVal)
        Me.SoftDataTab.Controls.Add(Me.InfTotal)
        Me.SoftDataTab.Controls.Add(Me.SelectedSystem)
        Me.SoftDataTab.Controls.Add(Me.UpdateBgsData)
        Me.SoftDataTab.Controls.Add(Me.CaptureEDScreen)
        Me.SoftDataTab.Controls.Add(Me.LoadingLabel)
        Me.SoftDataTab.Controls.Add(Me.AlwaysOnTopCheckbox)
        Me.SoftDataTab.Location = New System.Drawing.Point(4, 22)
        Me.SoftDataTab.Name = "SoftDataTab"
        Me.SoftDataTab.Size = New System.Drawing.Size(558, 446)
        Me.SoftDataTab.TabIndex = 3
        Me.SoftDataTab.Text = "OCR"
        Me.SoftDataTab.UseVisualStyleBackColor = True
        '
        'StatusBox
        '
        Me.StatusBox.Location = New System.Drawing.Point(10, 255)
        Me.StatusBox.Multiline = True
        Me.StatusBox.Name = "StatusBox"
        Me.StatusBox.ReadOnly = True
        Me.StatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.StatusBox.Size = New System.Drawing.Size(456, 182)
        Me.StatusBox.TabIndex = 44
        '
        'SystemNameBox
        '
        Me.SystemNameBox.Location = New System.Drawing.Point(10, 33)
        Me.SystemNameBox.Name = "SystemNameBox"
        Me.SystemNameBox.Size = New System.Drawing.Size(138, 20)
        Me.SystemNameBox.TabIndex = 43
        '
        'SystemLabel
        '
        Me.SystemLabel.AutoSize = True
        Me.SystemLabel.Location = New System.Drawing.Point(3, 8)
        Me.SystemLabel.Name = "SystemLabel"
        Me.SystemLabel.Size = New System.Drawing.Size(44, 13)
        Me.SystemLabel.TabIndex = 29
        Me.SystemLabel.Text = "System:"
        '
        'SoftDataGrid
        '
        Me.SoftDataGrid.AllowUserToOrderColumns = True
        Me.SoftDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SoftDataGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Faction, Me.Influence, Me.State, Me.PrevInfluence, Me.InfluenceDiff, Me.PrevState, Me.Found})
        Me.SoftDataGrid.Location = New System.Drawing.Point(10, 59)
        Me.SoftDataGrid.MultiSelect = False
        Me.SoftDataGrid.Name = "SoftDataGrid"
        Me.SoftDataGrid.RowHeadersWidth = 4
        Me.SoftDataGrid.ShowCellErrors = False
        Me.SoftDataGrid.Size = New System.Drawing.Size(543, 190)
        Me.SoftDataGrid.TabIndex = 37
        Me.SoftDataGrid.Visible = False
        '
        'Faction
        '
        Me.Faction.HeaderText = "Faction"
        Me.Faction.MaxInputLength = 250
        Me.Faction.Name = "Faction"
        Me.Faction.ToolTipText = "Faction Name"
        Me.Faction.Width = 170
        '
        'Influence
        '
        Me.Influence.HeaderText = "Influence"
        Me.Influence.MaxInputLength = 5
        Me.Influence.Name = "Influence"
        Me.Influence.ToolTipText = "Faction Influence %"
        Me.Influence.Width = 55
        '
        'State
        '
        Me.State.HeaderText = "State"
        Me.State.Name = "State"
        Me.State.ToolTipText = "Faction State (if any)"
        Me.State.Width = 76
        '
        'PrevInfluence
        '
        Me.PrevInfluence.HeaderText = "Prev Influence"
        Me.PrevInfluence.Name = "PrevInfluence"
        Me.PrevInfluence.ReadOnly = True
        Me.PrevInfluence.ToolTipText = "Influence from previous collection"
        Me.PrevInfluence.Width = 55
        '
        'InfluenceDiff
        '
        Me.InfluenceDiff.HeaderText = "Influence Diff"
        Me.InfluenceDiff.Name = "InfluenceDiff"
        Me.InfluenceDiff.ReadOnly = True
        Me.InfluenceDiff.ToolTipText = "Influence difference after collection"
        Me.InfluenceDiff.Width = 55
        '
        'PrevState
        '
        Me.PrevState.HeaderText = "PrevState"
        Me.PrevState.Name = "PrevState"
        Me.PrevState.ToolTipText = "State from previous collection"
        Me.PrevState.Width = 76
        '
        'Found
        '
        Me.Found.HeaderText = "Found"
        Me.Found.Name = "Found"
        Me.Found.Width = 50
        '
        'LoadingLabel
        '
        Me.LoadingLabel.AutoSize = True
        Me.LoadingLabel.Location = New System.Drawing.Point(51, 8)
        Me.LoadingLabel.Name = "LoadingLabel"
        Me.LoadingLabel.Size = New System.Drawing.Size(54, 13)
        Me.LoadingLabel.TabIndex = 49
        Me.LoadingLabel.Text = "Loading..."
        '
        'AlwaysOnTopCheckbox
        '
        Me.AlwaysOnTopCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AlwaysOnTopCheckbox.Location = New System.Drawing.Point(419, -1)
        Me.AlwaysOnTopCheckbox.Name = "AlwaysOnTopCheckbox"
        Me.AlwaysOnTopCheckbox.Size = New System.Drawing.Size(134, 23)
        Me.AlwaysOnTopCheckbox.TabIndex = 42
        Me.AlwaysOnTopCheckbox.Text = "Always on Top"
        Me.AlwaysOnTopCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AlwaysOnTopCheckbox.UseVisualStyleBackColor = True
        '
        'Tabs
        '
        Me.Tabs.Controls.Add(Me.SoftDataTab)
        Me.Tabs.Controls.Add(Me.SettingsTab)
        Me.Tabs.Controls.Add(Me.LogTab)
        Me.Tabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Tabs.Location = New System.Drawing.Point(0, 0)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(566, 472)
        Me.Tabs.TabIndex = 5
        '
        'RockRatsClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(566, 472)
        Me.Controls.Add(Me.Tabs)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "RockRatsClient"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rock Rats Client"
        CType(Me.EDCapture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LogTab.ResumeLayout(False)
        Me.SettingsTab.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.SoftDataTab.ResumeLayout(False)
        Me.SoftDataTab.PerformLayout()
        CType(Me.SoftDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tabs.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LoadTimer As Timer
    Friend WithEvents JourneyDir As DirectoryServices.DirectoryEntry
    Friend WithEvents FolderBrowser As FolderBrowserDialog
    Friend WithEvents CommsTimer As Timer
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents LogTab As TabPage
    Friend WithEvents LogTextBox As RichTextBox
    Friend WithEvents SettingsTab As TabPage
    Friend WithEvents CommanderName As TextBox
    Friend WithEvents JournalFolder As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents BrowserForDir As Button
    Friend WithEvents SoftDataTab As TabPage
    Friend WithEvents StatusBox As TextBox
    Friend WithEvents SystemNameBox As TextBox
    Friend WithEvents RemoveButton As Button
    Friend WithEvents AddButton As Button
    Friend WithEvents ViewWebTracker As Button
    Friend WithEvents SystemLabel As Label
    Friend WithEvents SoftDataGrid As DataGridView
    Friend WithEvents Faction As DataGridViewTextBoxColumn
    Friend WithEvents Influence As DataGridViewTextBoxColumn
    Friend WithEvents State As DataGridViewTextBoxColumn
    Friend WithEvents PrevInfluence As DataGridViewTextBoxColumn
    Friend WithEvents InfluenceDiff As DataGridViewTextBoxColumn
    Friend WithEvents PrevState As DataGridViewTextBoxColumn
    Friend WithEvents Found As DataGridViewCheckBoxColumn
    Friend WithEvents InfTotalVal As Label
    Friend WithEvents InfTotal As Label
    Friend WithEvents SelectedSystem As ComboBox
    Friend WithEvents UpdateBgsData As Button
    Friend WithEvents CaptureEDScreen As Button
    Friend WithEvents LoadingLabel As Label
    Friend WithEvents Tabs As TabControl
    Friend WithEvents AlwaysOnTopCheckbox As CheckBox
    Friend WithEvents ScanMarginLeft As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents EDCapture As PictureBox
    Friend WithEvents LogOcrCheckbox As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label2 As Label
End Class
