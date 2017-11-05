<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucMusicVideo
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucMusicVideo))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TabControlMain = New System.Windows.Forms.TabControl()
        Me.tPMainMV = New System.Windows.Forms.TabPage()
        Me.lblMultiMode = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbMVFilename = New System.Windows.Forms.RadioButton()
        Me.rbMVTitleandYear = New System.Windows.Forms.RadioButton()
        Me.rbMVArtistAndTitle = New System.Windows.Forms.RadioButton()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.btnSearchNew = New System.Windows.Forms.Button()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.PcBxPoster = New System.Windows.Forms.PictureBox()
        Me.txtFullpath = New System.Windows.Forms.TextBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPlot = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.PcBxMusicVideoScreenShot = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtArtist = New System.Windows.Forms.TextBox()
        Me.txtDirector = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtYear = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAlbum = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtStudio = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtGenre = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtRuntime = New System.Windows.Forms.TextBox()
        Me.btnMVPlay = New System.Windows.Forms.Button()
        Me.MVDgv1 = New System.Windows.Forms.DataGridView()
        Me.CM1MVBrowser = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmiMVName = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiMVPlay = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiMVOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVViewNfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiMVDelNfoArt = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiMVRescrape = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVRescrapeSpecific = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVReDirector = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVReGenre = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVRePlot = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVReStudio = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmiMVReYear = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmbxMVSort = New System.Windows.Forms.ComboBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.btn_MVSortReset = New System.Windows.Forms.Button()
        Me.tPScreenshotMV = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnMvSaveScreenShot = New System.Windows.Forms.Button()
        Me.pcBxScreenshot = New System.Windows.Forms.PictureBox()
        Me.btnScreenshotMinus = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.pbMvScrSht4 = New System.Windows.Forms.PictureBox()
        Me.pbMvScrSht0 = New System.Windows.Forms.PictureBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.pbMvScrSht3 = New System.Windows.Forms.PictureBox()
        Me.pbMvScrSht1 = New System.Windows.Forms.PictureBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.pbMvScrSht2 = New System.Windows.Forms.PictureBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btnGoogleSearch = New System.Windows.Forms.Button()
        Me.btnPasteFromClipboard = New System.Windows.Forms.Button()
        Me.btnCrop = New System.Windows.Forms.Button()
        Me.btnCropReset = New System.Windows.Forms.Button()
        Me.btnScreenshotPlus = New System.Windows.Forms.Button()
        Me.btnCreateScreenshot = New System.Windows.Forms.Button()
        Me.btnSaveCrop = New System.Windows.Forms.Button()
        Me.txtScreenshotTime = New System.Windows.Forms.TextBox()
        Me.tPPosterScrape = New System.Windows.Forms.TabPage()
        Me.btnGoogleSearchPoster = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.btnPosterPaste = New System.Windows.Forms.Button()
        Me.btnPosterSave = New System.Windows.Forms.Button()
        Me.btnPosterReset = New System.Windows.Forms.Button()
        Me.btnPosterCrop = New System.Windows.Forms.Button()
        Me.pcBxSinglePoster = New System.Windows.Forms.PictureBox()
        Me.tPMVChange = New System.Windows.Forms.TabPage()
        Me.chkBxOverWriteArt = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.btnManualScrape = New System.Windows.Forms.Button()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.tPPref = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnRemoveConcertFolder = New System.Windows.Forms.Button()
        Me.btnBrowseConcertFolders = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnAddConcertPath = New System.Windows.Forms.Button()
        Me.clbxMVConcertFolder = New System.Windows.Forms.CheckedListBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.clbxMvFolders = New System.Windows.Forms.CheckedListBox()
        Me.tbFolderPath = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.rb_MvScr3 = New System.Windows.Forms.RadioButton()
        Me.rb_MvScr2 = New System.Windows.Forms.RadioButton()
        Me.rb_MvScr1 = New System.Windows.Forms.RadioButton()
        Me.btnMVApply = New System.Windows.Forms.Button()
        Me.cb_MVPrefShowLog = New System.Windows.Forms.CheckBox()
        Me.tb_MVPrefScrnSht = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnAddFolderPath = New System.Windows.Forms.Button()
        Me.btnBrowseFolders = New System.Windows.Forms.Button()
        Me.btnRemoveFolder = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TabControlMain.SuspendLayout
        Me.tPMainMV.SuspendLayout
        Me.TableLayoutPanel1.SuspendLayout
        Me.Panel1.SuspendLayout
        CType(Me.PcBxPoster,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PcBxMusicVideoScreenShot,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.MVDgv1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.CM1MVBrowser.SuspendLayout
        Me.tPScreenshotMV.SuspendLayout
        Me.TableLayoutPanel3.SuspendLayout
        CType(Me.pcBxScreenshot,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.pbMvScrSht4,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.pbMvScrSht0,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.pbMvScrSht3,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.pbMvScrSht1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.pbMvScrSht2,System.ComponentModel.ISupportInitialize).BeginInit
        Me.tPPosterScrape.SuspendLayout
        CType(Me.pcBxSinglePoster,System.ComponentModel.ISupportInitialize).BeginInit
        Me.tPMVChange.SuspendLayout
        Me.tPPref.SuspendLayout
        Me.TableLayoutPanel2.SuspendLayout
        Me.GroupBox1.SuspendLayout
        Me.SuspendLayout
        '
        'TabControlMain
        '
        Me.TabControlMain.Controls.Add(Me.tPMainMV)
        Me.TabControlMain.Controls.Add(Me.tPScreenshotMV)
        Me.TabControlMain.Controls.Add(Me.tPPosterScrape)
        Me.TabControlMain.Controls.Add(Me.tPMVChange)
        Me.TabControlMain.Controls.Add(Me.tPPref)
        Me.TabControlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TabControlMain.Location = New System.Drawing.Point(0, 0)
        Me.TabControlMain.Name = "TabControlMain"
        Me.TabControlMain.SelectedIndex = 0
        Me.TabControlMain.Size = New System.Drawing.Size(1030, 600)
        Me.TabControlMain.TabIndex = 0
        Me.TabControlMain.TabStop = false
        '
        'tPMainMV
        '
        Me.tPMainMV.BackColor = System.Drawing.Color.LightGray
        Me.tPMainMV.Controls.Add(Me.lblMultiMode)
        Me.tPMainMV.Controls.Add(Me.TableLayoutPanel1)
        Me.tPMainMV.ForeColor = System.Drawing.Color.Black
        Me.tPMainMV.Location = New System.Drawing.Point(4, 24)
        Me.tPMainMV.Name = "tPMainMV"
        Me.tPMainMV.Padding = New System.Windows.Forms.Padding(3)
        Me.tPMainMV.Size = New System.Drawing.Size(1022, 572)
        Me.tPMainMV.TabIndex = 0
        Me.tPMainMV.Text = "Main Browser"
        '
        'lblMultiMode
        '
        Me.lblMultiMode.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblMultiMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.lblMultiMode.Location = New System.Drawing.Point(340, 75)
        Me.lblMultiMode.Margin = New System.Windows.Forms.Padding(40, 40, 4, 0)
        Me.lblMultiMode.Name = "lblMultiMode"
        Me.lblMultiMode.Size = New System.Drawing.Size(517, 163)
        Me.lblMultiMode.TabIndex = 165
        Me.lblMultiMode.Text = resources.GetString("lblMultiMode.Text")
        Me.lblMultiMode.Visible = false
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 17
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSearchNew, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.txtFilter, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.PcBxPoster, 13, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txtFullpath, 9, 15)
        Me.TableLayoutPanel1.Controls.Add(Me.btnRefresh, 5, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.btnSave, 15, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 8, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.txtPlot, 9, 17)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 8, 17)
        Me.TableLayoutPanel1.Controls.Add(Me.txtTitle, 8, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.PcBxMusicVideoScreenShot, 8, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 8, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.Label10, 8, 15)
        Me.TableLayoutPanel1.Controls.Add(Me.txtArtist, 9, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txtDirector, 9, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 8, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.txtYear, 9, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 11, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.txtAlbum, 12, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 11, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.txtStudio, 12, 11)
        Me.TableLayoutPanel1.Controls.Add(Me.Label11, 11, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.txtGenre, 12, 13)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 11, 15)
        Me.TableLayoutPanel1.Controls.Add(Me.txtRuntime, 12, 15)
        Me.TableLayoutPanel1.Controls.Add(Me.btnMVPlay, 14, 15)
        Me.TableLayoutPanel1.Controls.Add(Me.MVDgv1, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.cmbxMVSort, 2, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label23, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Label24, 5, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.btn_MVSortReset, 5, 4)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 20
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 87!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1016, 566)
        Me.TableLayoutPanel1.TabIndex = 53
        '
        'Panel1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Panel1, 7)
        Me.Panel1.Controls.Add(Me.rbMVFilename)
        Me.Panel1.Controls.Add(Me.rbMVTitleandYear)
        Me.Panel1.Controls.Add(Me.rbMVArtistAndTitle)
        Me.Panel1.Controls.Add(Me.Label31)
        Me.Panel1.Location = New System.Drawing.Point(4, 49)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.TableLayoutPanel1.SetRowSpan(Me.Panel1, 2)
        Me.Panel1.Size = New System.Drawing.Size(277, 27)
        Me.Panel1.TabIndex = 62
        '
        'rbMVFilename
        '
        Me.rbMVFilename.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbMVFilename.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbMVFilename.Image = Global.Media_Companion.My.Resources.Resources.Folder
        Me.rbMVFilename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rbMVFilename.Location = New System.Drawing.Point(198, 2)
        Me.rbMVFilename.Margin = New System.Windows.Forms.Padding(4)
        Me.rbMVFilename.Name = "rbMVFilename"
        Me.rbMVFilename.Size = New System.Drawing.Size(80, 25)
        Me.rbMVFilename.TabIndex = 2
        Me.rbMVFilename.Text = "Filename"
        Me.rbMVFilename.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbMVFilename.UseVisualStyleBackColor = true
        '
        'rbMVTitleandYear
        '
        Me.rbMVTitleandYear.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbMVTitleandYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbMVTitleandYear.Image = Global.Media_Companion.My.Resources.Resources.Page
        Me.rbMVTitleandYear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rbMVTitleandYear.Location = New System.Drawing.Point(112, 2)
        Me.rbMVTitleandYear.Margin = New System.Windows.Forms.Padding(4)
        Me.rbMVTitleandYear.Name = "rbMVTitleandYear"
        Me.rbMVTitleandYear.Size = New System.Drawing.Size(84, 25)
        Me.rbMVTitleandYear.TabIndex = 1
        Me.rbMVTitleandYear.Text = "Title && Year"
        Me.rbMVTitleandYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbMVTitleandYear.UseVisualStyleBackColor = true
        '
        'rbMVArtistAndTitle
        '
        Me.rbMVArtistAndTitle.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbMVArtistAndTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbMVArtistAndTitle.Image = Global.Media_Companion.My.Resources.Resources.Clock
        Me.rbMVArtistAndTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.rbMVArtistAndTitle.Location = New System.Drawing.Point(28, 2)
        Me.rbMVArtistAndTitle.Margin = New System.Windows.Forms.Padding(4)
        Me.rbMVArtistAndTitle.Name = "rbMVArtistAndTitle"
        Me.rbMVArtistAndTitle.Size = New System.Drawing.Size(84, 25)
        Me.rbMVArtistAndTitle.TabIndex = 0
        Me.rbMVArtistAndTitle.Text = "Artist && Title"
        Me.rbMVArtistAndTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbMVArtistAndTitle.UseVisualStyleBackColor = true
        '
        'Label31
        '
        Me.Label31.AutoSize = true
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label31.Location = New System.Drawing.Point(1, 8)
        Me.Label31.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(23, 13)
        Me.Label31.TabIndex = 62
        Me.Label31.Text = "List"
        '
        'btnSearchNew
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.btnSearchNew, 3)
        Me.btnSearchNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnSearchNew.Image = Global.Media_Companion.My.Resources.Resources.new2
        Me.btnSearchNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearchNew.Location = New System.Drawing.Point(8, 8)
        Me.btnSearchNew.Name = "btnSearchNew"
        Me.btnSearchNew.Size = New System.Drawing.Size(120, 33)
        Me.btnSearchNew.TabIndex = 25
        Me.btnSearchNew.TabStop = false
        Me.btnSearchNew.Text = "Search  "
        Me.btnSearchNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSearchNew, "Search Folders For New"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"     Music Videos and"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"         Scrape Data")
        Me.btnSearchNew.UseVisualStyleBackColor = true
        '
        'txtFilter
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtFilter, 4)
        Me.txtFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtFilter.Location = New System.Drawing.Point(8, 108)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(140, 26)
        Me.txtFilter.TabIndex = 50
        Me.txtFilter.TabStop = false
        Me.ToolTip1.SetToolTip(Me.txtFilter, "Text Filter")
        '
        'PcBxPoster
        '
        Me.PcBxPoster.BackColor = System.Drawing.Color.White
        Me.TableLayoutPanel1.SetColumnSpan(Me.PcBxPoster, 2)
        Me.PcBxPoster.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PcBxPoster.Location = New System.Drawing.Point(717, 51)
        Me.PcBxPoster.Name = "PcBxPoster"
        Me.TableLayoutPanel1.SetRowSpan(Me.PcBxPoster, 5)
        Me.PcBxPoster.Size = New System.Drawing.Size(245, 227)
        Me.PcBxPoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PcBxPoster.TabIndex = 52
        Me.PcBxPoster.TabStop = false
        '
        'txtFullpath
        '
        Me.txtFullpath.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtFullpath, 2)
        Me.txtFullpath.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtFullpath.Location = New System.Drawing.Point(381, 412)
        Me.txtFullpath.Name = "txtFullpath"
        Me.txtFullpath.ReadOnly = true
        Me.txtFullpath.Size = New System.Drawing.Size(230, 26)
        Me.txtFullpath.TabIndex = 45
        Me.txtFullpath.TabStop = false
        '
        'btnRefresh
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.btnRefresh, 2)
        Me.btnRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnRefresh.Image = Global.Media_Companion.My.Resources.Resources.RefreshAll
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefresh.Location = New System.Drawing.Point(154, 8)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(123, 33)
        Me.btnRefresh.TabIndex = 49
        Me.btnRefresh.TabStop = false
        Me.btnRefresh.Text = "Refresh  "
        Me.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRefresh, "Reload all nfo's from"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Music Video Folders")
        Me.btnRefresh.UseVisualStyleBackColor = true
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSave.Image = Global.Media_Companion.My.Resources.Resources.Save
        Me.btnSave.Location = New System.Drawing.Point(968, 8)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(34, 31)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Button2"
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save Manual Edits")
        Me.btnSave.UseVisualStyleBackColor = true
        '
        'Label5
        '
        Me.Label5.AutoSize = true
        Me.Label5.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label5.Location = New System.Drawing.Point(306, 374)
        Me.Label5.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 27)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Director:"
        '
        'txtPlot
        '
        Me.txtPlot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtPlot, 6)
        Me.txtPlot.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtPlot.Location = New System.Drawing.Point(381, 453)
        Me.txtPlot.Multiline = true
        Me.txtPlot.Name = "txtPlot"
        Me.TableLayoutPanel1.SetRowSpan(Me.txtPlot, 2)
        Me.txtPlot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPlot.Size = New System.Drawing.Size(581, 104)
        Me.txtPlot.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label3.Location = New System.Drawing.Point(335, 456)
        Me.Label3.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 49)
        Me.Label3.TabIndex = 38
        Me.Label3.Text = "Plot:"
        '
        'txtTitle
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtTitle, 6)
        Me.txtTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtTitle.Location = New System.Drawing.Point(296, 8)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(460, 31)
        Me.txtTitle.TabIndex = 1
        '
        'PcBxMusicVideoScreenShot
        '
        Me.PcBxMusicVideoScreenShot.BackColor = System.Drawing.Color.White
        Me.TableLayoutPanel1.SetColumnSpan(Me.PcBxMusicVideoScreenShot, 5)
        Me.PcBxMusicVideoScreenShot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PcBxMusicVideoScreenShot.Location = New System.Drawing.Point(296, 51)
        Me.PcBxMusicVideoScreenShot.Name = "PcBxMusicVideoScreenShot"
        Me.TableLayoutPanel1.SetRowSpan(Me.PcBxMusicVideoScreenShot, 5)
        Me.PcBxMusicVideoScreenShot.Size = New System.Drawing.Size(415, 227)
        Me.PcBxMusicVideoScreenShot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PcBxMusicVideoScreenShot.TabIndex = 26
        Me.PcBxMusicVideoScreenShot.TabStop = false
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label2.Location = New System.Drawing.Point(325, 292)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 27)
        Me.Label2.TabIndex = 37
        Me.Label2.Text = "Artist:"
        '
        'Label10
        '
        Me.Label10.AutoSize = true
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label10.Location = New System.Drawing.Point(297, 415)
        Me.Label10.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(78, 27)
        Me.Label10.TabIndex = 46
        Me.Label10.Text = "Filename:"
        '
        'txtArtist
        '
        Me.txtArtist.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtArtist, 2)
        Me.txtArtist.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtArtist.Location = New System.Drawing.Point(381, 289)
        Me.txtArtist.Name = "txtArtist"
        Me.txtArtist.Size = New System.Drawing.Size(230, 26)
        Me.txtArtist.TabIndex = 2
        '
        'txtDirector
        '
        Me.txtDirector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtDirector, 2)
        Me.txtDirector.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtDirector.Location = New System.Drawing.Point(381, 371)
        Me.txtDirector.Name = "txtDirector"
        Me.txtDirector.Size = New System.Drawing.Size(230, 26)
        Me.txtDirector.TabIndex = 6
        '
        'Label7
        '
        Me.Label7.AutoSize = true
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label7.Location = New System.Drawing.Point(328, 333)
        Me.Label7.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(47, 27)
        Me.Label7.TabIndex = 42
        Me.Label7.Text = "Year:"
        '
        'txtYear
        '
        Me.txtYear.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtYear, 2)
        Me.txtYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtYear.Location = New System.Drawing.Point(381, 330)
        Me.txtYear.Name = "txtYear"
        Me.txtYear.Size = New System.Drawing.Size(230, 26)
        Me.txtYear.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.Location = New System.Drawing.Point(633, 292)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 27)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "Album:"
        '
        'txtAlbum
        '
        Me.txtAlbum.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtAlbum, 3)
        Me.txtAlbum.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtAlbum.Location = New System.Drawing.Point(697, 289)
        Me.txtAlbum.Name = "txtAlbum"
        Me.txtAlbum.Size = New System.Drawing.Size(265, 26)
        Me.txtAlbum.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label4.Location = New System.Drawing.Point(632, 333)
        Me.Label4.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 27)
        Me.Label4.TabIndex = 39
        Me.Label4.Text = "Studio:"
        '
        'txtStudio
        '
        Me.txtStudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtStudio, 3)
        Me.txtStudio.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtStudio.Location = New System.Drawing.Point(697, 330)
        Me.txtStudio.Name = "txtStudio"
        Me.txtStudio.Size = New System.Drawing.Size(265, 26)
        Me.txtStudio.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.AutoSize = true
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label11.Location = New System.Drawing.Point(633, 374)
        Me.Label11.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(58, 27)
        Me.Label11.TabIndex = 48
        Me.Label11.Text = "Genre:"
        '
        'txtGenre
        '
        Me.txtGenre.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtGenre, 3)
        Me.txtGenre.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtGenre.Location = New System.Drawing.Point(697, 371)
        Me.txtGenre.Name = "txtGenre"
        Me.txtGenre.Size = New System.Drawing.Size(265, 26)
        Me.txtGenre.TabIndex = 7
        '
        'Label6
        '
        Me.Label6.AutoSize = true
        Me.Label6.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label6.Location = New System.Drawing.Point(618, 415)
        Me.Label6.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 27)
        Me.Label6.TabIndex = 41
        Me.Label6.Text = "Runtime:"
        '
        'txtRuntime
        '
        Me.txtRuntime.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtRuntime, 2)
        Me.txtRuntime.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtRuntime.Location = New System.Drawing.Point(697, 412)
        Me.txtRuntime.Name = "txtRuntime"
        Me.txtRuntime.ReadOnly = true
        Me.txtRuntime.Size = New System.Drawing.Size(89, 26)
        Me.txtRuntime.TabIndex = 34
        Me.txtRuntime.TabStop = false
        '
        'btnMVPlay
        '
        Me.btnMVPlay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnMVPlay.Image = Global.Media_Companion.My.Resources.Resources.Movie
        Me.btnMVPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMVPlay.Location = New System.Drawing.Point(799, 412)
        Me.btnMVPlay.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.btnMVPlay.Name = "btnMVPlay"
        Me.btnMVPlay.Size = New System.Drawing.Size(95, 27)
        Me.btnMVPlay.TabIndex = 10
        Me.btnMVPlay.Text = "Play"
        Me.btnMVPlay.UseVisualStyleBackColor = true
        '
        'MVDgv1
        '
        Me.MVDgv1.AllowUserToAddRows = false
        Me.MVDgv1.AllowUserToDeleteRows = false
        Me.MVDgv1.AllowUserToResizeRows = false
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240,Byte),Integer), CType(CType(240,Byte),Integer), CType(CType(240,Byte),Integer))
        Me.MVDgv1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.MVDgv1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.MVDgv1.BackgroundColor = System.Drawing.Color.Gray
        Me.MVDgv1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.MVDgv1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.MVDgv1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TableLayoutPanel1.SetColumnSpan(Me.MVDgv1, 6)
        Me.MVDgv1.ContextMenuStrip = Me.CM1MVBrowser
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.MediumSeaGreen
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.MVDgv1.DefaultCellStyle = DataGridViewCellStyle2
        Me.MVDgv1.GridColor = System.Drawing.Color.FromArgb(CType(CType(240,Byte),Integer), CType(CType(240,Byte),Integer), CType(CType(240,Byte),Integer))
        Me.MVDgv1.Location = New System.Drawing.Point(8, 143)
        Me.MVDgv1.Name = "MVDgv1"
        Me.MVDgv1.RowHeadersVisible = false
        Me.TableLayoutPanel1.SetRowSpan(Me.MVDgv1, 12)
        Me.MVDgv1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.MVDgv1.ShowCellErrors = false
        Me.MVDgv1.Size = New System.Drawing.Size(274, 359)
        Me.MVDgv1.StandardTab = true
        Me.MVDgv1.TabIndex = 54
        Me.MVDgv1.TabStop = false
        '
        'CM1MVBrowser
        '
        Me.CM1MVBrowser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiMVName, Me.ToolStripSeparator1, Me.tsmiMVPlay, Me.ToolStripSeparator2, Me.tsmiMVOpenFolder, Me.tsmiMVViewNfo, Me.ToolStripSeparator3, Me.tsmiMVDelNfoArt, Me.ToolStripSeparator4, Me.tsmiMVRescrape, Me.tsmiMVRescrapeSpecific})
        Me.CM1MVBrowser.Name = "CM1MVBrowser"
        Me.CM1MVBrowser.Size = New System.Drawing.Size(188, 182)
        '
        'tsmiMVName
        '
        Me.tsmiMVName.BackColor = System.Drawing.Color.Honeydew
        Me.tsmiMVName.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tsmiMVName.Name = "tsmiMVName"
        Me.tsmiMVName.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVName.Text = "MusicVid Name"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(184, 6)
        '
        'tsmiMVPlay
        '
        Me.tsmiMVPlay.Name = "tsmiMVPlay"
        Me.tsmiMVPlay.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVPlay.Text = "Play Music Video"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(184, 6)
        '
        'tsmiMVOpenFolder
        '
        Me.tsmiMVOpenFolder.Name = "tsmiMVOpenFolder"
        Me.tsmiMVOpenFolder.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVOpenFolder.Text = "Open Folder"
        '
        'tsmiMVViewNfo
        '
        Me.tsmiMVViewNfo.Name = "tsmiMVViewNfo"
        Me.tsmiMVViewNfo.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVViewNfo.Text = "View Nfo"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(184, 6)
        '
        'tsmiMVDelNfoArt
        '
        Me.tsmiMVDelNfoArt.Name = "tsmiMVDelNfoArt"
        Me.tsmiMVDelNfoArt.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVDelNfoArt.Text = "Delete Nfo and Artwork"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(184, 6)
        '
        'tsmiMVRescrape
        '
        Me.tsmiMVRescrape.Enabled = false
        Me.tsmiMVRescrape.Name = "tsmiMVRescrape"
        Me.tsmiMVRescrape.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVRescrape.Text = "ToolStripMenuItem1"
        '
        'tsmiMVRescrapeSpecific
        '
        Me.tsmiMVRescrapeSpecific.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiMVReDirector, Me.tsmiMVReGenre, Me.tsmiMVRePlot, Me.tsmiMVReStudio, Me.tsmiMVReYear})
        Me.tsmiMVRescrapeSpecific.Enabled = false
        Me.tsmiMVRescrapeSpecific.Name = "tsmiMVRescrapeSpecific"
        Me.tsmiMVRescrapeSpecific.Size = New System.Drawing.Size(187, 22)
        Me.tsmiMVRescrapeSpecific.Text = "Rescrape Specific"
        '
        'tsmiMVReDirector
        '
        Me.tsmiMVReDirector.Enabled = false
        Me.tsmiMVReDirector.Name = "tsmiMVReDirector"
        Me.tsmiMVReDirector.Size = New System.Drawing.Size(112, 22)
        Me.tsmiMVReDirector.Text = "Director"
        '
        'tsmiMVReGenre
        '
        Me.tsmiMVReGenre.Enabled = false
        Me.tsmiMVReGenre.Name = "tsmiMVReGenre"
        Me.tsmiMVReGenre.Size = New System.Drawing.Size(112, 22)
        Me.tsmiMVReGenre.Text = "Genre"
        '
        'tsmiMVRePlot
        '
        Me.tsmiMVRePlot.Enabled = false
        Me.tsmiMVRePlot.Name = "tsmiMVRePlot"
        Me.tsmiMVRePlot.Size = New System.Drawing.Size(112, 22)
        Me.tsmiMVRePlot.Text = "Plot"
        '
        'tsmiMVReStudio
        '
        Me.tsmiMVReStudio.Enabled = false
        Me.tsmiMVReStudio.Name = "tsmiMVReStudio"
        Me.tsmiMVReStudio.Size = New System.Drawing.Size(112, 22)
        Me.tsmiMVReStudio.Text = "Studio"
        '
        'tsmiMVReYear
        '
        Me.tsmiMVReYear.Enabled = false
        Me.tsmiMVReYear.Name = "tsmiMVReYear"
        Me.tsmiMVReYear.Size = New System.Drawing.Size(112, 22)
        Me.tsmiMVReYear.Text = "Year"
        '
        'cmbxMVSort
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.cmbxMVSort, 3)
        Me.cmbxMVSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxMVSort.FormattingEnabled = true
        Me.cmbxMVSort.Items.AddRange(New Object() {"A-Z", "Year", "Runtime", "DateAdded"})
        Me.cmbxMVSort.Location = New System.Drawing.Point(43, 83)
        Me.cmbxMVSort.Name = "cmbxMVSort"
        Me.cmbxMVSort.Size = New System.Drawing.Size(105, 23)
        Me.cmbxMVSort.TabIndex = 55
        Me.cmbxMVSort.TabStop = false
        '
        'Label23
        '
        Me.Label23.AutoSize = true
        Me.Label23.Location = New System.Drawing.Point(8, 86)
        Me.Label23.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(29, 15)
        Me.Label23.TabIndex = 56
        Me.Label23.Text = "Sort"
        '
        'Label24
        '
        Me.Label24.AutoSize = true
        Me.TableLayoutPanel1.SetColumnSpan(Me.Label24, 2)
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label24.Location = New System.Drawing.Point(157, 113)
        Me.Label24.Margin = New System.Windows.Forms.Padding(6, 8, 3, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(61, 18)
        Me.Label24.TabIndex = 57
        Me.Label24.Text = "Search"
        '
        'btn_MVSortReset
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.btn_MVSortReset, 2)
        Me.btn_MVSortReset.Location = New System.Drawing.Point(161, 80)
        Me.btn_MVSortReset.Margin = New System.Windows.Forms.Padding(10, 0, 3, 0)
        Me.btn_MVSortReset.Name = "btn_MVSortReset"
        Me.btn_MVSortReset.Size = New System.Drawing.Size(80, 22)
        Me.btn_MVSortReset.TabIndex = 58
        Me.btn_MVSortReset.TabStop = false
        Me.btn_MVSortReset.Text = "Reset Sort"
        Me.btn_MVSortReset.UseVisualStyleBackColor = true
        '
        'tPScreenshotMV
        '
        Me.tPScreenshotMV.BackColor = System.Drawing.Color.LightGray
        Me.tPScreenshotMV.Controls.Add(Me.TableLayoutPanel3)
        Me.tPScreenshotMV.Location = New System.Drawing.Point(4, 24)
        Me.tPScreenshotMV.Name = "tPScreenshotMV"
        Me.tPScreenshotMV.Padding = New System.Windows.Forms.Padding(3)
        Me.tPScreenshotMV.Size = New System.Drawing.Size(1022, 572)
        Me.tPScreenshotMV.TabIndex = 2
        Me.tPScreenshotMV.Text = "Screenshot"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel3.ColumnCount = 24
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 141!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3!))
        Me.TableLayoutPanel3.Controls.Add(Me.btnMvSaveScreenShot, 15, 16)
        Me.TableLayoutPanel3.Controls.Add(Me.pcBxScreenshot, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.btnScreenshotMinus, 5, 16)
        Me.TableLayoutPanel3.Controls.Add(Me.Label12, 1, 16)
        Me.TableLayoutPanel3.Controls.Add(Me.pbMvScrSht4, 15, 14)
        Me.TableLayoutPanel3.Controls.Add(Me.pbMvScrSht0, 1, 14)
        Me.TableLayoutPanel3.Controls.Add(Me.Label16, 20, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.pbMvScrSht3, 13, 14)
        Me.TableLayoutPanel3.Controls.Add(Me.pbMvScrSht1, 4, 14)
        Me.TableLayoutPanel3.Controls.Add(Me.Label14, 18, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.pbMvScrSht2, 9, 14)
        Me.TableLayoutPanel3.Controls.Add(Me.Label15, 18, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.Label17, 20, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.btnGoogleSearch, 18, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.btnPasteFromClipboard, 18, 6)
        Me.TableLayoutPanel3.Controls.Add(Me.btnCrop, 18, 8)
        Me.TableLayoutPanel3.Controls.Add(Me.btnCropReset, 18, 9)
        Me.TableLayoutPanel3.Controls.Add(Me.btnScreenshotPlus, 8, 16)
        Me.TableLayoutPanel3.Controls.Add(Me.btnCreateScreenshot, 11, 16)
        Me.TableLayoutPanel3.Controls.Add(Me.btnSaveCrop, 18, 11)
        Me.TableLayoutPanel3.Controls.Add(Me.txtScreenshotTime, 6, 16)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 18
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(1016, 566)
        Me.TableLayoutPanel3.TabIndex = 22
        '
        'btnMvSaveScreenShot
        '
        Me.btnMvSaveScreenShot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnMvSaveScreenShot, 3)
        Me.btnMvSaveScreenShot.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnMvSaveScreenShot.Location = New System.Drawing.Point(601, 511)
        Me.btnMvSaveScreenShot.Name = "btnMvSaveScreenShot"
        Me.btnMvSaveScreenShot.Size = New System.Drawing.Size(135, 49)
        Me.btnMvSaveScreenShot.TabIndex = 23
        Me.btnMvSaveScreenShot.Text = "Save Screenshot"
        Me.btnMvSaveScreenShot.UseVisualStyleBackColor = true
        '
        'pcBxScreenshot
        '
        Me.pcBxScreenshot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.pcBxScreenshot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanel3.SetColumnSpan(Me.pcBxScreenshot, 16)
        Me.pcBxScreenshot.Location = New System.Drawing.Point(6, 6)
        Me.pcBxScreenshot.Name = "pcBxScreenshot"
        Me.TableLayoutPanel3.SetRowSpan(Me.pcBxScreenshot, 12)
        Me.pcBxScreenshot.Size = New System.Drawing.Size(730, 384)
        Me.pcBxScreenshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pcBxScreenshot.TabIndex = 0
        Me.pcBxScreenshot.TabStop = false
        '
        'btnScreenshotMinus
        '
        Me.btnScreenshotMinus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnScreenshotMinus.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnScreenshotMinus.Location = New System.Drawing.Point(176, 511)
        Me.btnScreenshotMinus.Name = "btnScreenshotMinus"
        Me.btnScreenshotMinus.Size = New System.Drawing.Size(35, 49)
        Me.btnScreenshotMinus.TabIndex = 5
        Me.btnScreenshotMinus.Text = "-"
        Me.btnScreenshotMinus.UseVisualStyleBackColor = true
        '
        'Label12
        '
        Me.Label12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = true
        Me.TableLayoutPanel3.SetColumnSpan(Me.Label12, 4)
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label12.Location = New System.Drawing.Point(6, 514)
        Me.Label12.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(158, 49)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Enter Time in Seconds"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"for Capture:"
        '
        'pbMvScrSht4
        '
        Me.pbMvScrSht4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.pbMvScrSht4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanel3.SetColumnSpan(Me.pbMvScrSht4, 2)
        Me.pbMvScrSht4.Location = New System.Drawing.Point(601, 404)
        Me.pbMvScrSht4.Name = "pbMvScrSht4"
        Me.pbMvScrSht4.Size = New System.Drawing.Size(134, 90)
        Me.pbMvScrSht4.TabIndex = 21
        Me.pbMvScrSht4.TabStop = false
        '
        'pbMvScrSht0
        '
        Me.pbMvScrSht0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.pbMvScrSht0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanel3.SetColumnSpan(Me.pbMvScrSht0, 2)
        Me.pbMvScrSht0.Location = New System.Drawing.Point(6, 404)
        Me.pbMvScrSht0.Name = "pbMvScrSht0"
        Me.pbMvScrSht0.Size = New System.Drawing.Size(134, 90)
        Me.pbMvScrSht0.TabIndex = 17
        Me.pbMvScrSht0.TabStop = false
        '
        'Label16
        '
        Me.Label16.AutoSize = true
        Me.TableLayoutPanel3.SetColumnSpan(Me.Label16, 2)
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label16.Location = New System.Drawing.Point(823, 9)
        Me.Label16.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(66, 20)
        Me.Label16.TabIndex = 14
        Me.Label16.Text = "Label16"
        '
        'pbMvScrSht3
        '
        Me.pbMvScrSht3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.pbMvScrSht3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbMvScrSht3.Location = New System.Drawing.Point(454, 404)
        Me.pbMvScrSht3.Name = "pbMvScrSht3"
        Me.pbMvScrSht3.Size = New System.Drawing.Size(134, 90)
        Me.pbMvScrSht3.TabIndex = 20
        Me.pbMvScrSht3.TabStop = false
        '
        'pbMvScrSht1
        '
        Me.pbMvScrSht1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.pbMvScrSht1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanel3.SetColumnSpan(Me.pbMvScrSht1, 4)
        Me.pbMvScrSht1.Location = New System.Drawing.Point(153, 404)
        Me.pbMvScrSht1.Name = "pbMvScrSht1"
        Me.pbMvScrSht1.Size = New System.Drawing.Size(134, 90)
        Me.pbMvScrSht1.TabIndex = 18
        Me.pbMvScrSht1.TabStop = false
        '
        'Label14
        '
        Me.Label14.AutoSize = true
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label14.Location = New System.Drawing.Point(748, 9)
        Me.Label14.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 20)
        Me.Label14.TabIndex = 12
        Me.Label14.Text = "Width:"
        '
        'pbMvScrSht2
        '
        Me.pbMvScrSht2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.pbMvScrSht2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TableLayoutPanel3.SetColumnSpan(Me.pbMvScrSht2, 3)
        Me.pbMvScrSht2.Location = New System.Drawing.Point(299, 404)
        Me.pbMvScrSht2.Name = "pbMvScrSht2"
        Me.pbMvScrSht2.Size = New System.Drawing.Size(134, 90)
        Me.pbMvScrSht2.TabIndex = 19
        Me.pbMvScrSht2.TabStop = false
        '
        'Label15
        '
        Me.Label15.AutoSize = true
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label15.Location = New System.Drawing.Point(748, 51)
        Me.Label15.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(60, 20)
        Me.Label15.TabIndex = 13
        Me.Label15.Text = "Height:"
        '
        'Label17
        '
        Me.Label17.AutoSize = true
        Me.TableLayoutPanel3.SetColumnSpan(Me.Label17, 2)
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label17.Location = New System.Drawing.Point(823, 51)
        Me.Label17.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(66, 20)
        Me.Label17.TabIndex = 15
        Me.Label17.Text = "Label17"
        '
        'btnGoogleSearch
        '
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnGoogleSearch, 3)
        Me.btnGoogleSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnGoogleSearch.Location = New System.Drawing.Point(748, 90)
        Me.btnGoogleSearch.Name = "btnGoogleSearch"
        Me.btnGoogleSearch.Size = New System.Drawing.Size(126, 32)
        Me.btnGoogleSearch.TabIndex = 16
        Me.btnGoogleSearch.Text = "Google Search"
        Me.btnGoogleSearch.UseVisualStyleBackColor = true
        '
        'btnPasteFromClipboard
        '
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnPasteFromClipboard, 3)
        Me.btnPasteFromClipboard.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnPasteFromClipboard.Location = New System.Drawing.Point(748, 129)
        Me.btnPasteFromClipboard.Name = "btnPasteFromClipboard"
        Me.btnPasteFromClipboard.Size = New System.Drawing.Size(143, 49)
        Me.btnPasteFromClipboard.TabIndex = 11
        Me.btnPasteFromClipboard.Text = "Paste from Clipboard"
        Me.btnPasteFromClipboard.UseVisualStyleBackColor = true
        '
        'btnCrop
        '
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnCrop, 3)
        Me.btnCrop.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnCrop.Location = New System.Drawing.Point(748, 192)
        Me.btnCrop.Name = "btnCrop"
        Me.btnCrop.Size = New System.Drawing.Size(131, 32)
        Me.btnCrop.TabIndex = 7
        Me.btnCrop.Text = "Enable Crop"
        Me.btnCrop.UseVisualStyleBackColor = true
        '
        'btnCropReset
        '
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnCropReset, 3)
        Me.btnCropReset.Enabled = false
        Me.btnCropReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnCropReset.Location = New System.Drawing.Point(748, 232)
        Me.btnCropReset.Name = "btnCropReset"
        Me.btnCropReset.Size = New System.Drawing.Size(131, 32)
        Me.btnCropReset.TabIndex = 9
        Me.btnCropReset.Text = "Reset Image"
        Me.btnCropReset.UseVisualStyleBackColor = true
        '
        'btnScreenshotPlus
        '
        Me.btnScreenshotPlus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnScreenshotPlus, 2)
        Me.btnScreenshotPlus.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnScreenshotPlus.Location = New System.Drawing.Point(293, 511)
        Me.btnScreenshotPlus.Name = "btnScreenshotPlus"
        Me.btnScreenshotPlus.Size = New System.Drawing.Size(35, 49)
        Me.btnScreenshotPlus.TabIndex = 4
        Me.btnScreenshotPlus.Text = "+"
        Me.btnScreenshotPlus.UseVisualStyleBackColor = true
        '
        'btnCreateScreenshot
        '
        Me.btnCreateScreenshot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnCreateScreenshot, 3)
        Me.btnCreateScreenshot.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnCreateScreenshot.Location = New System.Drawing.Point(350, 511)
        Me.btnCreateScreenshot.Name = "btnCreateScreenshot"
        Me.btnCreateScreenshot.Size = New System.Drawing.Size(188, 49)
        Me.btnCreateScreenshot.TabIndex = 2
        Me.btnCreateScreenshot.Text = "Populate screenshot previews"
        Me.btnCreateScreenshot.UseVisualStyleBackColor = true
        '
        'btnSaveCrop
        '
        Me.btnSaveCrop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.SetColumnSpan(Me.btnSaveCrop, 3)
        Me.btnSaveCrop.Enabled = false
        Me.btnSaveCrop.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnSaveCrop.Location = New System.Drawing.Point(748, 280)
        Me.btnSaveCrop.Name = "btnSaveCrop"
        Me.btnSaveCrop.Size = New System.Drawing.Size(131, 34)
        Me.btnSaveCrop.TabIndex = 10
        Me.btnSaveCrop.Text = "Save Changes"
        Me.btnSaveCrop.UseVisualStyleBackColor = true
        '
        'txtScreenshotTime
        '
        Me.TableLayoutPanel3.SetColumnSpan(Me.txtScreenshotTime, 2)
        Me.txtScreenshotTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.txtScreenshotTime.Location = New System.Drawing.Point(217, 520)
        Me.txtScreenshotTime.Margin = New System.Windows.Forms.Padding(3, 12, 3, 3)
        Me.txtScreenshotTime.Name = "txtScreenshotTime"
        Me.txtScreenshotTime.Size = New System.Drawing.Size(70, 26)
        Me.txtScreenshotTime.TabIndex = 22
        Me.txtScreenshotTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tPPosterScrape
        '
        Me.tPPosterScrape.Controls.Add(Me.btnGoogleSearchPoster)
        Me.tPPosterScrape.Controls.Add(Me.Label18)
        Me.tPPosterScrape.Controls.Add(Me.Label19)
        Me.tPPosterScrape.Controls.Add(Me.Label20)
        Me.tPPosterScrape.Controls.Add(Me.Label21)
        Me.tPPosterScrape.Controls.Add(Me.btnPosterPaste)
        Me.tPPosterScrape.Controls.Add(Me.btnPosterSave)
        Me.tPPosterScrape.Controls.Add(Me.btnPosterReset)
        Me.tPPosterScrape.Controls.Add(Me.btnPosterCrop)
        Me.tPPosterScrape.Controls.Add(Me.pcBxSinglePoster)
        Me.tPPosterScrape.Location = New System.Drawing.Point(4, 24)
        Me.tPPosterScrape.Name = "tPPosterScrape"
        Me.tPPosterScrape.Size = New System.Drawing.Size(1022, 572)
        Me.tPPosterScrape.TabIndex = 4
        Me.tPPosterScrape.Text = "Poster"
        Me.tPPosterScrape.UseVisualStyleBackColor = true
        '
        'btnGoogleSearchPoster
        '
        Me.btnGoogleSearchPoster.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnGoogleSearchPoster.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnGoogleSearchPoster.Location = New System.Drawing.Point(433, 134)
        Me.btnGoogleSearchPoster.Name = "btnGoogleSearchPoster"
        Me.btnGoogleSearchPoster.Size = New System.Drawing.Size(126, 32)
        Me.btnGoogleSearchPoster.TabIndex = 30
        Me.btnGoogleSearchPoster.Text = "Google Search"
        Me.btnGoogleSearchPoster.UseVisualStyleBackColor = true
        '
        'Label18
        '
        Me.Label18.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoSize = true
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label18.Location = New System.Drawing.Point(489, 90)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(66, 20)
        Me.Label18.TabIndex = 29
        Me.Label18.Text = "Label18"
        '
        'Label19
        '
        Me.Label19.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label19.AutoSize = true
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label19.Location = New System.Drawing.Point(489, 57)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(66, 20)
        Me.Label19.TabIndex = 28
        Me.Label19.Text = "Label19"
        '
        'Label20
        '
        Me.Label20.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoSize = true
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label20.Location = New System.Drawing.Point(429, 90)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(60, 20)
        Me.Label20.TabIndex = 27
        Me.Label20.Text = "Height:"
        '
        'Label21
        '
        Me.Label21.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label21.AutoSize = true
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label21.Location = New System.Drawing.Point(429, 57)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(54, 20)
        Me.Label21.TabIndex = 26
        Me.Label21.Text = "Width:"
        '
        'btnPosterPaste
        '
        Me.btnPosterPaste.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnPosterPaste.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnPosterPaste.Location = New System.Drawing.Point(565, 134)
        Me.btnPosterPaste.Name = "btnPosterPaste"
        Me.btnPosterPaste.Size = New System.Drawing.Size(165, 32)
        Me.btnPosterPaste.TabIndex = 25
        Me.btnPosterPaste.Text = "Paste from Clipboard"
        Me.btnPosterPaste.UseVisualStyleBackColor = true
        '
        'btnPosterSave
        '
        Me.btnPosterSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnPosterSave.Enabled = false
        Me.btnPosterSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnPosterSave.Location = New System.Drawing.Point(583, 201)
        Me.btnPosterSave.Name = "btnPosterSave"
        Me.btnPosterSave.Size = New System.Drawing.Size(131, 32)
        Me.btnPosterSave.TabIndex = 24
        Me.btnPosterSave.Text = "Save Changes"
        Me.btnPosterSave.UseVisualStyleBackColor = true
        '
        'btnPosterReset
        '
        Me.btnPosterReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnPosterReset.Enabled = false
        Me.btnPosterReset.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnPosterReset.Location = New System.Drawing.Point(433, 220)
        Me.btnPosterReset.Name = "btnPosterReset"
        Me.btnPosterReset.Size = New System.Drawing.Size(131, 32)
        Me.btnPosterReset.TabIndex = 23
        Me.btnPosterReset.Text = "Reset Image"
        Me.btnPosterReset.UseVisualStyleBackColor = true
        '
        'btnPosterCrop
        '
        Me.btnPosterCrop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnPosterCrop.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnPosterCrop.Location = New System.Drawing.Point(433, 182)
        Me.btnPosterCrop.Name = "btnPosterCrop"
        Me.btnPosterCrop.Size = New System.Drawing.Size(131, 32)
        Me.btnPosterCrop.TabIndex = 22
        Me.btnPosterCrop.Text = "Enable Crop"
        Me.btnPosterCrop.UseVisualStyleBackColor = true
        '
        'pcBxSinglePoster
        '
        Me.pcBxSinglePoster.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.pcBxSinglePoster.Location = New System.Drawing.Point(3, 3)
        Me.pcBxSinglePoster.Name = "pcBxSinglePoster"
        Me.pcBxSinglePoster.Size = New System.Drawing.Size(410, 557)
        Me.pcBxSinglePoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pcBxSinglePoster.TabIndex = 0
        Me.pcBxSinglePoster.TabStop = false
        '
        'tPMVChange
        '
        Me.tPMVChange.Controls.Add(Me.chkBxOverWriteArt)
        Me.tPMVChange.Controls.Add(Me.Label13)
        Me.tPMVChange.Controls.Add(Me.btnManualScrape)
        Me.tPMVChange.Controls.Add(Me.WebBrowser1)
        Me.tPMVChange.Location = New System.Drawing.Point(4, 24)
        Me.tPMVChange.Name = "tPMVChange"
        Me.tPMVChange.Size = New System.Drawing.Size(1022, 572)
        Me.tPMVChange.TabIndex = 3
        Me.tPMVChange.Text = "Change MusicVideo/Concert"
        Me.tPMVChange.UseVisualStyleBackColor = true
        '
        'chkBxOverWriteArt
        '
        Me.chkBxOverWriteArt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.chkBxOverWriteArt.AutoSize = true
        Me.chkBxOverWriteArt.Checked = true
        Me.chkBxOverWriteArt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBxOverWriteArt.Location = New System.Drawing.Point(762, 527)
        Me.chkBxOverWriteArt.Name = "chkBxOverWriteArt"
        Me.chkBxOverWriteArt.Size = New System.Drawing.Size(190, 34)
        Me.chkBxOverWriteArt.TabIndex = 3
        Me.chkBxOverWriteArt.Text = "Un-check if you don't want MC"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"to overwrite your current art. "
        Me.chkBxOverWriteArt.UseVisualStyleBackColor = true
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = true
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label13.Location = New System.Drawing.Point(322, 534)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(367, 20)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Browse to the correct Wikipedia Page and Click Go"
        '
        'btnManualScrape
        '
        Me.btnManualScrape.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnManualScrape.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnManualScrape.Location = New System.Drawing.Point(695, 529)
        Me.btnManualScrape.Name = "btnManualScrape"
        Me.btnManualScrape.Size = New System.Drawing.Size(70, 30)
        Me.btnManualScrape.TabIndex = 1
        Me.btnManualScrape.Text = "Go"
        Me.btnManualScrape.UseVisualStyleBackColor = true
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.WebBrowser1.Location = New System.Drawing.Point(3, 3)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(964, 520)
        Me.WebBrowser1.TabIndex = 0
        '
        'tPPref
        '
        Me.tPPref.BackColor = System.Drawing.Color.LightGray
        Me.tPPref.Controls.Add(Me.TableLayoutPanel2)
        Me.tPPref.Location = New System.Drawing.Point(4, 24)
        Me.tPPref.Name = "tPPref"
        Me.tPPref.Padding = New System.Windows.Forms.Padding(3)
        Me.tPPref.Size = New System.Drawing.Size(1022, 572)
        Me.tPPref.TabIndex = 1
        Me.tPPref.Text = "Preferences"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 11
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 149!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 142!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel2.Controls.Add(Me.btnRemoveConcertFolder, 8, 10)
        Me.TableLayoutPanel2.Controls.Add(Me.btnBrowseConcertFolders, 7, 10)
        Me.TableLayoutPanel2.Controls.Add(Me.TextBox1, 7, 9)
        Me.TableLayoutPanel2.Controls.Add(Me.btnAddConcertPath, 6, 9)
        Me.TableLayoutPanel2.Controls.Add(Me.clbxMVConcertFolder, 6, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.Label26, 6, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.clbxMvFolders, 1, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.tbFolderPath, 2, 9)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox1, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.btnMVApply, 1, 11)
        Me.TableLayoutPanel2.Controls.Add(Me.cb_MVPrefShowLog, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.tb_MVPrefScrnSht, 3, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label25, 4, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label9, 1, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label8, 1, 8)
        Me.TableLayoutPanel2.Controls.Add(Me.btnAddFolderPath, 1, 9)
        Me.TableLayoutPanel2.Controls.Add(Me.btnBrowseFolders, 2, 10)
        Me.TableLayoutPanel2.Controls.Add(Me.btnRemoveFolder, 3, 10)
        Me.TableLayoutPanel2.Controls.Add(Me.Button1, 5, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label27, 6, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 13
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1016, 566)
        Me.TableLayoutPanel2.TabIndex = 8
        '
        'btnRemoveConcertFolder
        '
        Me.btnRemoveConcertFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.SetColumnSpan(Me.btnRemoveConcertFolder, 2)
        Me.btnRemoveConcertFolder.Location = New System.Drawing.Point(639, 458)
        Me.btnRemoveConcertFolder.Name = "btnRemoveConcertFolder"
        Me.btnRemoveConcertFolder.Size = New System.Drawing.Size(122, 48)
        Me.btnRemoveConcertFolder.TabIndex = 18
        Me.btnRemoveConcertFolder.Text = "Remove Selected Folder"
        Me.btnRemoveConcertFolder.UseVisualStyleBackColor = true
        '
        'btnBrowseConcertFolders
        '
        Me.btnBrowseConcertFolders.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnBrowseConcertFolders.Location = New System.Drawing.Point(497, 458)
        Me.btnBrowseConcertFolders.Name = "btnBrowseConcertFolders"
        Me.btnBrowseConcertFolders.Size = New System.Drawing.Size(122, 48)
        Me.btnBrowseConcertFolders.TabIndex = 17
        Me.btnBrowseConcertFolders.Text = "Browse for Music Video Folders"
        Me.btnBrowseConcertFolders.UseVisualStyleBackColor = true
        '
        'TextBox1
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.TextBox1, 2)
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Location = New System.Drawing.Point(497, 411)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(3, 11, 3, 3)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(316, 21)
        Me.TextBox1.TabIndex = 16
        '
        'btnAddConcertPath
        '
        Me.btnAddConcertPath.Location = New System.Drawing.Point(426, 403)
        Me.btnAddConcertPath.Name = "btnAddConcertPath"
        Me.btnAddConcertPath.Size = New System.Drawing.Size(56, 35)
        Me.btnAddConcertPath.TabIndex = 15
        Me.btnAddConcertPath.Text = "Add"
        Me.btnAddConcertPath.UseVisualStyleBackColor = true
        '
        'clbxMVConcertFolder
        '
        Me.clbxMVConcertFolder.AllowDrop = true
        Me.TableLayoutPanel2.SetColumnSpan(Me.clbxMVConcertFolder, 3)
        Me.clbxMVConcertFolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clbxMVConcertFolder.FormattingEnabled = true
        Me.clbxMVConcertFolder.Location = New System.Drawing.Point(426, 174)
        Me.clbxMVConcertFolder.Name = "clbxMVConcertFolder"
        Me.TableLayoutPanel2.SetRowSpan(Me.clbxMVConcertFolder, 3)
        Me.clbxMVConcertFolder.Size = New System.Drawing.Size(387, 203)
        Me.clbxMVConcertFolder.Sorted = true
        Me.clbxMVConcertFolder.TabIndex = 14
        '
        'Label26
        '
        Me.Label26.AutoSize = true
        Me.Label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label26, 3)
        Me.Label26.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label26.Location = New System.Drawing.Point(426, 152)
        Me.Label26.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(387, 19)
        Me.Label26.TabIndex = 13
        Me.Label26.Text = "Concert Root Video Folder(s)"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'clbxMvFolders
        '
        Me.clbxMvFolders.AllowDrop = true
        Me.TableLayoutPanel2.SetColumnSpan(Me.clbxMvFolders, 4)
        Me.clbxMvFolders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clbxMvFolders.FormattingEnabled = true
        Me.clbxMvFolders.Location = New System.Drawing.Point(12, 174)
        Me.clbxMvFolders.Name = "clbxMvFolders"
        Me.TableLayoutPanel2.SetRowSpan(Me.clbxMvFolders, 3)
        Me.clbxMvFolders.Size = New System.Drawing.Size(393, 203)
        Me.clbxMvFolders.Sorted = true
        Me.clbxMvFolders.TabIndex = 7
        '
        'tbFolderPath
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbFolderPath, 3)
        Me.tbFolderPath.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbFolderPath.Location = New System.Drawing.Point(77, 411)
        Me.tbFolderPath.Margin = New System.Windows.Forms.Padding(3, 11, 3, 3)
        Me.tbFolderPath.Name = "tbFolderPath"
        Me.tbFolderPath.Size = New System.Drawing.Size(328, 21)
        Me.tbFolderPath.TabIndex = 3
        '
        'GroupBox1
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.GroupBox1, 4)
        Me.GroupBox1.Controls.Add(Me.Label22)
        Me.GroupBox1.Controls.Add(Me.rb_MvScr3)
        Me.GroupBox1.Controls.Add(Me.rb_MvScr2)
        Me.GroupBox1.Controls.Add(Me.rb_MvScr1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(12, 19)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(393, 100)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = false
        Me.GroupBox1.Text = "Scraper Select"
        '
        'Label22
        '
        Me.Label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label22.Location = New System.Drawing.Point(105, 16)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(282, 78)
        Me.Label22.TabIndex = 3
        Me.Label22.Text = "Choose your Scraper."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Wikipedia - More information, but may not be as current."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"IMVDB - Limited Info but larger database"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"TheAudioDb - Limited Info and limited"& _ 
    " database."
        '
        'rb_MvScr3
        '
        Me.rb_MvScr3.AutoSize = true
        Me.rb_MvScr3.Location = New System.Drawing.Point(6, 77)
        Me.rb_MvScr3.Name = "rb_MvScr3"
        Me.rb_MvScr3.Size = New System.Drawing.Size(93, 19)
        Me.rb_MvScr3.TabIndex = 2
        Me.rb_MvScr3.TabStop = true
        Me.rb_MvScr3.Text = "TheAudioDb"
        Me.rb_MvScr3.UseVisualStyleBackColor = true
        '
        'rb_MvScr2
        '
        Me.rb_MvScr2.AutoSize = true
        Me.rb_MvScr2.Location = New System.Drawing.Point(6, 54)
        Me.rb_MvScr2.Name = "rb_MvScr2"
        Me.rb_MvScr2.Size = New System.Drawing.Size(62, 19)
        Me.rb_MvScr2.TabIndex = 1
        Me.rb_MvScr2.TabStop = true
        Me.rb_MvScr2.Text = "IMVDb"
        Me.rb_MvScr2.UseVisualStyleBackColor = true
        '
        'rb_MvScr1
        '
        Me.rb_MvScr1.AutoSize = true
        Me.rb_MvScr1.Location = New System.Drawing.Point(6, 31)
        Me.rb_MvScr1.Name = "rb_MvScr1"
        Me.rb_MvScr1.Size = New System.Drawing.Size(79, 19)
        Me.rb_MvScr1.TabIndex = 0
        Me.rb_MvScr1.TabStop = true
        Me.rb_MvScr1.Text = "Wikipedia"
        Me.rb_MvScr1.UseVisualStyleBackColor = true
        '
        'btnMVApply
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.btnMVApply, 2)
        Me.btnMVApply.Location = New System.Drawing.Point(12, 515)
        Me.btnMVApply.Margin = New System.Windows.Forms.Padding(3, 6, 3, 3)
        Me.btnMVApply.Name = "btnMVApply"
        Me.btnMVApply.Size = New System.Drawing.Size(114, 40)
        Me.btnMVApply.TabIndex = 9
        Me.btnMVApply.Text = "Apply Changes"
        Me.btnMVApply.UseVisualStyleBackColor = true
        '
        'cb_MVPrefShowLog
        '
        Me.cb_MVPrefShowLog.AutoSize = true
        Me.TableLayoutPanel2.SetColumnSpan(Me.cb_MVPrefShowLog, 2)
        Me.cb_MVPrefShowLog.Location = New System.Drawing.Point(17, 125)
        Me.cb_MVPrefShowLog.Margin = New System.Windows.Forms.Padding(8, 3, 3, 3)
        Me.cb_MVPrefShowLog.Name = "cb_MVPrefShowLog"
        Me.cb_MVPrefShowLog.Size = New System.Drawing.Size(132, 19)
        Me.cb_MVPrefShowLog.TabIndex = 10
        Me.cb_MVPrefShowLog.Text = "Display Scrape Log"
        Me.cb_MVPrefShowLog.UseVisualStyleBackColor = true
        '
        'tb_MVPrefScrnSht
        '
        Me.tb_MVPrefScrnSht.Location = New System.Drawing.Point(226, 125)
        Me.tb_MVPrefScrnSht.Name = "tb_MVPrefScrnSht"
        Me.tb_MVPrefScrnSht.Size = New System.Drawing.Size(46, 21)
        Me.tb_MVPrefScrnSht.TabIndex = 11
        '
        'Label25
        '
        Me.Label25.AutoSize = true
        Me.Label25.Location = New System.Drawing.Point(281, 127)
        Me.Label25.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(102, 15)
        Me.Label25.TabIndex = 12
        Me.Label25.Text = "ScreenShot Time"
        '
        'Label9
        '
        Me.Label9.AutoSize = true
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label9, 4)
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Location = New System.Drawing.Point(12, 152)
        Me.Label9.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(393, 19)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Single Music Video Root Folder(s)"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = true
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label8, 3)
        Me.Label8.Location = New System.Drawing.Point(12, 385)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(226, 15)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Manually add Path to Music Video folder"
        '
        'btnAddFolderPath
        '
        Me.btnAddFolderPath.Location = New System.Drawing.Point(12, 403)
        Me.btnAddFolderPath.Name = "btnAddFolderPath"
        Me.btnAddFolderPath.Size = New System.Drawing.Size(56, 35)
        Me.btnAddFolderPath.TabIndex = 4
        Me.btnAddFolderPath.Text = "Add"
        Me.btnAddFolderPath.UseVisualStyleBackColor = true
        '
        'btnBrowseFolders
        '
        Me.btnBrowseFolders.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.btnBrowseFolders.Location = New System.Drawing.Point(77, 458)
        Me.btnBrowseFolders.Name = "btnBrowseFolders"
        Me.btnBrowseFolders.Size = New System.Drawing.Size(122, 48)
        Me.btnBrowseFolders.TabIndex = 2
        Me.btnBrowseFolders.Text = "Browse for Music Video Folders"
        Me.btnBrowseFolders.UseVisualStyleBackColor = true
        '
        'btnRemoveFolder
        '
        Me.btnRemoveFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.SetColumnSpan(Me.btnRemoveFolder, 2)
        Me.btnRemoveFolder.Location = New System.Drawing.Point(226, 458)
        Me.btnRemoveFolder.Name = "btnRemoveFolder"
        Me.btnRemoveFolder.Size = New System.Drawing.Size(122, 48)
        Me.btnRemoveFolder.TabIndex = 5
        Me.btnRemoveFolder.Text = "Remove Selected Folder"
        Me.btnRemoveFolder.UseVisualStyleBackColor = true
        '
        'Button1
        '
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Button1.Enabled = false
        Me.Button1.Location = New System.Drawing.Point(408, 0)
        Me.Button1.Margin = New System.Windows.Forms.Padding(0)
        Me.Button1.Name = "Button1"
        Me.TableLayoutPanel2.SetRowSpan(Me.Button1, 13)
        Me.Button1.Size = New System.Drawing.Size(15, 566)
        Me.Button1.TabIndex = 19
        Me.Button1.UseVisualStyleBackColor = true
        '
        'Label27
        '
        Me.Label27.AutoSize = true
        Me.TableLayoutPanel2.SetColumnSpan(Me.Label27, 3)
        Me.Label27.Location = New System.Drawing.Point(426, 8)
        Me.Label27.Name = "Label27"
        Me.TableLayoutPanel2.SetRowSpan(Me.Label27, 2)
        Me.Label27.Size = New System.Drawing.Size(366, 105)
        Me.Label27.TabIndex = 20
        Me.Label27.Text = resources.GetString("Label27.Text")
        '
        'ucMusicVideo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TabControlMain)
        Me.Name = "ucMusicVideo"
        Me.Size = New System.Drawing.Size(1030, 600)
        Me.TabControlMain.ResumeLayout(false)
        Me.tPMainMV.ResumeLayout(false)
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.TableLayoutPanel1.PerformLayout
        Me.Panel1.ResumeLayout(false)
        Me.Panel1.PerformLayout
        CType(Me.PcBxPoster,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PcBxMusicVideoScreenShot,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.MVDgv1,System.ComponentModel.ISupportInitialize).EndInit
        Me.CM1MVBrowser.ResumeLayout(false)
        Me.tPScreenshotMV.ResumeLayout(false)
        Me.TableLayoutPanel3.ResumeLayout(false)
        Me.TableLayoutPanel3.PerformLayout
        CType(Me.pcBxScreenshot,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.pbMvScrSht4,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.pbMvScrSht0,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.pbMvScrSht3,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.pbMvScrSht1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.pbMvScrSht2,System.ComponentModel.ISupportInitialize).EndInit
        Me.tPPosterScrape.ResumeLayout(false)
        Me.tPPosterScrape.PerformLayout
        CType(Me.pcBxSinglePoster,System.ComponentModel.ISupportInitialize).EndInit
        Me.tPMVChange.ResumeLayout(false)
        Me.tPMVChange.PerformLayout
        Me.tPPref.ResumeLayout(false)
        Me.TableLayoutPanel2.ResumeLayout(false)
        Me.TableLayoutPanel2.PerformLayout
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents TabControlMain As System.Windows.Forms.TabControl
    Friend WithEvents tPMainMV As System.Windows.Forms.TabPage
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtGenre As System.Windows.Forms.TextBox
    Friend WithEvents txtFullpath As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtRuntime As System.Windows.Forms.TextBox
    Friend WithEvents txtYear As System.Windows.Forms.TextBox
    Friend WithEvents txtPlot As System.Windows.Forms.TextBox
    Friend WithEvents txtStudio As System.Windows.Forms.TextBox
    Friend WithEvents txtAlbum As System.Windows.Forms.TextBox
    Friend WithEvents txtDirector As System.Windows.Forms.TextBox
    Friend WithEvents txtArtist As System.Windows.Forms.TextBox
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents PcBxMusicVideoScreenShot As System.Windows.Forms.PictureBox
    Friend WithEvents btnSearchNew As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tPPref As System.Windows.Forms.TabPage
    Friend WithEvents btnBrowseFolders As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents tPScreenshotMV As System.Windows.Forms.TabPage
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnCreateScreenshot As System.Windows.Forms.Button
    Friend WithEvents pcBxScreenshot As System.Windows.Forms.PictureBox
    Friend WithEvents btnScreenshotMinus As System.Windows.Forms.Button
    Friend WithEvents btnScreenshotPlus As System.Windows.Forms.Button
    Friend WithEvents PcBxPoster As System.Windows.Forms.PictureBox
    Friend WithEvents btnRemoveFolder As System.Windows.Forms.Button
    Friend WithEvents btnAddFolderPath As System.Windows.Forms.Button
    Friend WithEvents tbFolderPath As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents tPMVChange As System.Windows.Forms.TabPage
    Friend WithEvents WebBrowser1 As System.Windows.Forms.WebBrowser
    Friend WithEvents btnManualScrape As System.Windows.Forms.Button
    Friend WithEvents chkBxOverWriteArt As System.Windows.Forms.CheckBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tPPosterScrape As System.Windows.Forms.TabPage
    Friend WithEvents btnCrop As System.Windows.Forms.Button
    Friend WithEvents btnSaveCrop As System.Windows.Forms.Button
    Friend WithEvents btnCropReset As System.Windows.Forms.Button
    Friend WithEvents btnPasteFromClipboard As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents btnGoogleSearch As System.Windows.Forms.Button
    Friend WithEvents pcBxSinglePoster As System.Windows.Forms.PictureBox
    Friend WithEvents btnGoogleSearchPoster As System.Windows.Forms.Button
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents btnPosterPaste As System.Windows.Forms.Button
    Friend WithEvents btnPosterSave As System.Windows.Forms.Button
    Friend WithEvents btnPosterReset As System.Windows.Forms.Button
    Friend WithEvents btnPosterCrop As System.Windows.Forms.Button
    Friend WithEvents clbxMvFolders As CheckedListBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents btnMVPlay As Button
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label22 As Label
    Friend WithEvents rb_MvScr3 As RadioButton
    Friend WithEvents rb_MvScr2 As RadioButton
    Friend WithEvents rb_MvScr1 As RadioButton
    Friend WithEvents btnMVApply As Button
    Friend WithEvents CM1MVBrowser As ContextMenuStrip
    Friend WithEvents tsmiMVName As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsmiMVPlay As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents tsmiMVOpenFolder As ToolStripMenuItem
    Friend WithEvents tsmiMVViewNfo As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents tsmiMVDelNfoArt As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents tsmiMVRescrape As ToolStripMenuItem
    Friend WithEvents tsmiMVRescrapeSpecific As ToolStripMenuItem
    Friend WithEvents tsmiMVReDirector As ToolStripMenuItem
    Friend WithEvents tsmiMVReGenre As ToolStripMenuItem
    Friend WithEvents tsmiMVRePlot As ToolStripMenuItem
    Friend WithEvents tsmiMVReStudio As ToolStripMenuItem
    Friend WithEvents tsmiMVReYear As ToolStripMenuItem
    Friend WithEvents MVDgv1 As DataGridView
    Friend WithEvents cmbxMVSort As ComboBox
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents btn_MVSortReset As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents rbMVFilename As RadioButton
    Friend WithEvents rbMVTitleandYear As RadioButton
    Friend WithEvents rbMVArtistAndTitle As RadioButton
    Friend WithEvents Label31 As Label
    Friend WithEvents tb_MVPrefScrnSht As TextBox
    Friend WithEvents cb_MVPrefShowLog As CheckBox
    Friend WithEvents Label25 As Label
    Friend WithEvents btnRemoveConcertFolder As Button
    Friend WithEvents btnBrowseConcertFolders As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents btnAddConcertPath As Button
    Friend WithEvents clbxMVConcertFolder As CheckedListBox
    Friend WithEvents Label26 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Label27 As Label
    Friend WithEvents lblMultiMode As Label
    Friend WithEvents pbMvScrSht4 As PictureBox
    Friend WithEvents pbMvScrSht3 As PictureBox
    Friend WithEvents pbMvScrSht2 As PictureBox
    Friend WithEvents pbMvScrSht1 As PictureBox
    Friend WithEvents pbMvScrSht0 As PictureBox
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents txtScreenshotTime As TextBox
    Friend WithEvents btnMvSaveScreenShot As Button
End Class
