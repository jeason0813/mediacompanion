<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class tv_batch_wizard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(tv_batch_wizard))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbshStatus = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cbshBannerMain = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbshDelArtwork = New System.Windows.Forms.CheckBox()
        Me.cbshPosters = New System.Windows.Forms.CheckBox()
        Me.cbshFanartTv = New System.Windows.Forms.CheckBox()
        Me.cbshSeason = New System.Windows.Forms.CheckBox()
        Me.cbshXtraFanart = New System.Windows.Forms.CheckBox()
        Me.cbshFanart = New System.Windows.Forms.CheckBox()
        Me.cbshActor = New System.Windows.Forms.CheckBox()
        Me.cbshStudio = New System.Windows.Forms.CheckBox()
        Me.cbshGenre = New System.Windows.Forms.CheckBox()
        Me.cbshMpaa = New System.Windows.Forms.CheckBox()
        Me.cbshRuntime = New System.Windows.Forms.CheckBox()
        Me.cbshPlot = New System.Windows.Forms.CheckBox()
        Me.cbshRating = New System.Windows.Forms.CheckBox()
        Me.cbshYear = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cbepIMDBId = New System.Windows.Forms.CheckBox()
        Me.cbepTitle = New System.Windows.Forms.CheckBox()
        Me.cbepStreamDetails = New System.Windows.Forms.CheckBox()
        Me.cbepCreateScreenshot = New System.Windows.Forms.CheckBox()
        Me.cbepdlThumbnail = New System.Windows.Forms.CheckBox()
        Me.cbepRuntime = New System.Windows.Forms.CheckBox()
        Me.cbepActor = New System.Windows.Forms.CheckBox()
        Me.cbepCredits = New System.Windows.Forms.CheckBox()
        Me.cbepDirector = New System.Windows.Forms.CheckBox()
        Me.cbepRating = New System.Windows.Forms.CheckBox()
        Me.cbepAired = New System.Windows.Forms.CheckBox()
        Me.cbepPlot = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.cbShSeriesXML = New System.Windows.Forms.CheckBox()
        Me.cbRewriteAllNfo = New System.Windows.Forms.CheckBox()
        Me.btn_TvBatchCancel = New System.Windows.Forms.Button()
        Me.btnTvBatchStart = New System.Windows.Forms.Button()
        Me.cbincludeLocked = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SplitContainer1.Panel1.SuspendLayout
        Me.SplitContainer1.Panel2.SuspendLayout
        Me.SplitContainer1.SuspendLayout
        Me.GroupBox1.SuspendLayout
        Me.GroupBox3.SuspendLayout
        Me.GroupBox2.SuspendLayout
        CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SplitContainer2.Panel1.SuspendLayout
        Me.SplitContainer2.Panel2.SuspendLayout
        Me.SplitContainer2.SuspendLayout
        Me.SuspendLayout
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.IsSplitterFixed = true
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Size = New System.Drawing.Size(414, 413)
        Me.SplitContainer1.SplitterDistance = 239
        Me.SplitContainer1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbshStatus)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.cbshActor)
        Me.GroupBox1.Controls.Add(Me.cbshStudio)
        Me.GroupBox1.Controls.Add(Me.cbshGenre)
        Me.GroupBox1.Controls.Add(Me.cbshMpaa)
        Me.GroupBox1.Controls.Add(Me.cbshRuntime)
        Me.GroupBox1.Controls.Add(Me.cbshPlot)
        Me.GroupBox1.Controls.Add(Me.cbshRating)
        Me.GroupBox1.Controls.Add(Me.cbshYear)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 19)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(398, 212)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = false
        Me.GroupBox1.Text = "Select Tags to Rescrape"
        '
        'cbshStatus
        '
        Me.cbshStatus.AutoSize = true
        Me.cbshStatus.Location = New System.Drawing.Point(7, 66)
        Me.cbshStatus.Name = "cbshStatus"
        Me.cbshStatus.Size = New System.Drawing.Size(88, 17)
        Me.cbshStatus.TabIndex = 14
        Me.cbshStatus.Text = "Series Status"
        Me.cbshStatus.UseVisualStyleBackColor = true
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cbshBannerMain)
        Me.GroupBox3.Controls.Add(Me.Label3)
        Me.GroupBox3.Controls.Add(Me.cbshDelArtwork)
        Me.GroupBox3.Controls.Add(Me.cbshPosters)
        Me.GroupBox3.Controls.Add(Me.cbshFanartTv)
        Me.GroupBox3.Controls.Add(Me.cbshSeason)
        Me.GroupBox3.Controls.Add(Me.cbshXtraFanart)
        Me.GroupBox3.Controls.Add(Me.cbshFanart)
        Me.GroupBox3.Location = New System.Drawing.Point(7, 89)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(389, 117)
        Me.GroupBox3.TabIndex = 13
        Me.GroupBox3.TabStop = false
        Me.GroupBox3.Text = "Download Missing artwork..."
        '
        'cbshBannerMain
        '
        Me.cbshBannerMain.AutoSize = true
        Me.cbshBannerMain.Enabled = false
        Me.cbshBannerMain.Location = New System.Drawing.Point(7, 94)
        Me.cbshBannerMain.Name = "cbshBannerMain"
        Me.cbshBannerMain.Size = New System.Drawing.Size(302, 17)
        Me.cbshBannerMain.TabIndex = 15
        Me.cbshBannerMain.Text = "Save Main Banner as Season Banner if none to Download"
        Me.cbshBannerMain.UseVisualStyleBackColor = true
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(215, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(170, 54)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Some artwork automatically"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"selected if Delete artwork enabled"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"else you'll have "& _ 
    "no artwork for"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"these Tv Series."
        Me.Label3.Visible = false
        '
        'cbshDelArtwork
        '
        Me.cbshDelArtwork.AutoSize = true
        Me.cbshDelArtwork.Location = New System.Drawing.Point(7, 71)
        Me.cbshDelArtwork.Name = "cbshDelArtwork"
        Me.cbshDelArtwork.Size = New System.Drawing.Size(178, 17)
        Me.cbshDelArtwork.TabIndex = 13
        Me.cbshDelArtwork.Text = "Delete All Existing Show artwork"
        Me.cbshDelArtwork.UseVisualStyleBackColor = true
        '
        'cbshPosters
        '
        Me.cbshPosters.AutoSize = true
        Me.cbshPosters.Location = New System.Drawing.Point(7, 19)
        Me.cbshPosters.Name = "cbshPosters"
        Me.cbshPosters.Size = New System.Drawing.Size(66, 17)
        Me.cbshPosters.TabIndex = 8
        Me.cbshPosters.Text = "Poster/s"
        Me.cbshPosters.UseVisualStyleBackColor = true
        '
        'cbshFanartTv
        '
        Me.cbshFanartTv.AutoSize = true
        Me.cbshFanartTv.Location = New System.Drawing.Point(97, 42)
        Me.cbshFanartTv.Name = "cbshFanartTv"
        Me.cbshFanartTv.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cbshFanartTv.Size = New System.Drawing.Size(111, 17)
        Me.cbshFanartTv.TabIndex = 12
        Me.cbshFanartTv.Text = "Art from Fanart.Tv"
        Me.cbshFanartTv.UseVisualStyleBackColor = true
        '
        'cbshSeason
        '
        Me.cbshSeason.AutoSize = true
        Me.cbshSeason.Location = New System.Drawing.Point(98, 19)
        Me.cbshSeason.Name = "cbshSeason"
        Me.cbshSeason.Size = New System.Drawing.Size(77, 17)
        Me.cbshSeason.TabIndex = 10
        Me.cbshSeason.Text = "Season art"
        Me.cbshSeason.UseVisualStyleBackColor = true
        '
        'cbshXtraFanart
        '
        Me.cbshXtraFanart.AutoSize = true
        Me.cbshXtraFanart.Location = New System.Drawing.Point(6, 42)
        Me.cbshXtraFanart.Name = "cbshXtraFanart"
        Me.cbshXtraFanart.Size = New System.Drawing.Size(83, 17)
        Me.cbshXtraFanart.TabIndex = 11
        Me.cbshXtraFanart.Text = "Extra Fanart"
        Me.cbshXtraFanart.UseVisualStyleBackColor = true
        '
        'cbshFanart
        '
        Me.cbshFanart.AutoSize = true
        Me.cbshFanart.Location = New System.Drawing.Point(191, 19)
        Me.cbshFanart.Name = "cbshFanart"
        Me.cbshFanart.Size = New System.Drawing.Size(56, 17)
        Me.cbshFanart.TabIndex = 9
        Me.cbshFanart.Text = "Fanart"
        Me.cbshFanart.UseVisualStyleBackColor = true
        '
        'cbshActor
        '
        Me.cbshActor.AutoSize = true
        Me.cbshActor.Location = New System.Drawing.Point(198, 44)
        Me.cbshActor.Name = "cbshActor"
        Me.cbshActor.Size = New System.Drawing.Size(56, 17)
        Me.cbshActor.TabIndex = 7
        Me.cbshActor.Text = "Actors"
        Me.cbshActor.UseVisualStyleBackColor = true
        '
        'cbshStudio
        '
        Me.cbshStudio.AutoSize = true
        Me.cbshStudio.Location = New System.Drawing.Point(198, 21)
        Me.cbshStudio.Name = "cbshStudio"
        Me.cbshStudio.Size = New System.Drawing.Size(56, 17)
        Me.cbshStudio.TabIndex = 6
        Me.cbshStudio.Text = "Studio"
        Me.cbshStudio.UseVisualStyleBackColor = true
        '
        'cbshGenre
        '
        Me.cbshGenre.AutoSize = true
        Me.cbshGenre.Location = New System.Drawing.Point(284, 20)
        Me.cbshGenre.Name = "cbshGenre"
        Me.cbshGenre.Size = New System.Drawing.Size(55, 17)
        Me.cbshGenre.TabIndex = 5
        Me.cbshGenre.Text = "Genre"
        Me.cbshGenre.UseVisualStyleBackColor = true
        '
        'cbshMpaa
        '
        Me.cbshMpaa.AutoSize = true
        Me.cbshMpaa.Location = New System.Drawing.Point(105, 44)
        Me.cbshMpaa.Name = "cbshMpaa"
        Me.cbshMpaa.Size = New System.Drawing.Size(56, 17)
        Me.cbshMpaa.TabIndex = 4
        Me.cbshMpaa.Text = "MPAA"
        Me.cbshMpaa.UseVisualStyleBackColor = true
        '
        'cbshRuntime
        '
        Me.cbshRuntime.AutoSize = true
        Me.cbshRuntime.Location = New System.Drawing.Point(105, 21)
        Me.cbshRuntime.Name = "cbshRuntime"
        Me.cbshRuntime.Size = New System.Drawing.Size(65, 17)
        Me.cbshRuntime.TabIndex = 3
        Me.cbshRuntime.Text = "Runtime"
        Me.cbshRuntime.UseVisualStyleBackColor = true
        '
        'cbshPlot
        '
        Me.cbshPlot.AutoSize = true
        Me.cbshPlot.Location = New System.Drawing.Point(284, 44)
        Me.cbshPlot.Name = "cbshPlot"
        Me.cbshPlot.Size = New System.Drawing.Size(44, 17)
        Me.cbshPlot.TabIndex = 2
        Me.cbshPlot.Text = "Plot"
        Me.cbshPlot.UseVisualStyleBackColor = true
        '
        'cbshRating
        '
        Me.cbshRating.AutoSize = true
        Me.cbshRating.Location = New System.Drawing.Point(7, 44)
        Me.cbshRating.Name = "cbshRating"
        Me.cbshRating.Size = New System.Drawing.Size(57, 17)
        Me.cbshRating.TabIndex = 1
        Me.cbshRating.Text = "Rating"
        Me.cbshRating.UseVisualStyleBackColor = true
        '
        'cbshYear
        '
        Me.cbshYear.AutoSize = true
        Me.cbshYear.Location = New System.Drawing.Point(7, 20)
        Me.cbshYear.Name = "cbshYear"
        Me.cbshYear.Size = New System.Drawing.Size(73, 17)
        Me.cbshYear.TabIndex = 0
        Me.cbshYear.Text = "Premiered"
        Me.cbshYear.UseVisualStyleBackColor = true
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(3, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "TV Show batch Wizard"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cbepIMDBId)
        Me.GroupBox2.Controls.Add(Me.cbepTitle)
        Me.GroupBox2.Controls.Add(Me.cbepStreamDetails)
        Me.GroupBox2.Controls.Add(Me.cbepCreateScreenshot)
        Me.GroupBox2.Controls.Add(Me.cbepdlThumbnail)
        Me.GroupBox2.Controls.Add(Me.cbepRuntime)
        Me.GroupBox2.Controls.Add(Me.cbepActor)
        Me.GroupBox2.Controls.Add(Me.cbepCredits)
        Me.GroupBox2.Controls.Add(Me.cbepDirector)
        Me.GroupBox2.Controls.Add(Me.cbepRating)
        Me.GroupBox2.Controls.Add(Me.cbepAired)
        Me.GroupBox2.Controls.Add(Me.cbepPlot)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 26)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(400, 137)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = false
        Me.GroupBox2.Text = "Select Tags to Rescrape"
        '
        'cbepIMDBId
        '
        Me.cbepIMDBId.AutoSize = true
        Me.cbepIMDBId.Location = New System.Drawing.Point(104, 68)
        Me.cbepIMDBId.Name = "cbepIMDBId"
        Me.cbepIMDBId.Size = New System.Drawing.Size(110, 17)
        Me.cbepIMDBId.TabIndex = 11
        Me.cbepIMDBId.Text = "Update IMDB Id's"
        Me.cbepIMDBId.UseVisualStyleBackColor = true
        '
        'cbepTitle
        '
        Me.cbepTitle.AutoSize = true
        Me.cbepTitle.Location = New System.Drawing.Point(6, 68)
        Me.cbepTitle.Name = "cbepTitle"
        Me.cbepTitle.Size = New System.Drawing.Size(87, 17)
        Me.cbepTitle.TabIndex = 10
        Me.cbepTitle.Text = "Episode Title"
        Me.cbepTitle.UseVisualStyleBackColor = true
        '
        'cbepStreamDetails
        '
        Me.cbepStreamDetails.AutoSize = true
        Me.cbepStreamDetails.Location = New System.Drawing.Point(283, 44)
        Me.cbepStreamDetails.Name = "cbepStreamDetails"
        Me.cbepStreamDetails.Size = New System.Drawing.Size(82, 17)
        Me.cbepStreamDetails.TabIndex = 9
        Me.cbepStreamDetails.Text = "Media Tags"
        Me.cbepStreamDetails.UseVisualStyleBackColor = true
        '
        'cbepCreateScreenshot
        '
        Me.cbepCreateScreenshot.AutoSize = true
        Me.cbepCreateScreenshot.Location = New System.Drawing.Point(6, 114)
        Me.cbepCreateScreenshot.Name = "cbepCreateScreenshot"
        Me.cbepCreateScreenshot.Size = New System.Drawing.Size(209, 17)
        Me.cbepCreateScreenshot.TabIndex = 8
        Me.cbepCreateScreenshot.Text = "Create Screenshot (If no Thumb exists)"
        Me.ToolTip1.SetToolTip(Me.cbepCreateScreenshot, resources.GetString("cbepCreateScreenshot.ToolTip"))
        Me.cbepCreateScreenshot.UseVisualStyleBackColor = true
        '
        'cbepdlThumbnail
        '
        Me.cbepdlThumbnail.AutoSize = true
        Me.cbepdlThumbnail.Location = New System.Drawing.Point(6, 91)
        Me.cbepdlThumbnail.Name = "cbepdlThumbnail"
        Me.cbepdlThumbnail.Size = New System.Drawing.Size(257, 17)
        Me.cbepdlThumbnail.TabIndex = 7
        Me.cbepdlThumbnail.Text = "Attempt to download missing Episode thumbnails."
        Me.cbepdlThumbnail.UseVisualStyleBackColor = true
        '
        'cbepRuntime
        '
        Me.cbepRuntime.AutoSize = true
        Me.cbepRuntime.Location = New System.Drawing.Point(283, 20)
        Me.cbepRuntime.Name = "cbepRuntime"
        Me.cbepRuntime.Size = New System.Drawing.Size(65, 17)
        Me.cbepRuntime.TabIndex = 6
        Me.cbepRuntime.Text = "Runtime"
        Me.cbepRuntime.UseVisualStyleBackColor = true
        '
        'cbepActor
        '
        Me.cbepActor.AutoSize = true
        Me.cbepActor.Location = New System.Drawing.Point(197, 44)
        Me.cbepActor.Name = "cbepActor"
        Me.cbepActor.Size = New System.Drawing.Size(56, 17)
        Me.cbepActor.TabIndex = 5
        Me.cbepActor.Text = "Actors"
        Me.cbepActor.UseVisualStyleBackColor = true
        '
        'cbepCredits
        '
        Me.cbepCredits.AutoSize = true
        Me.cbepCredits.Location = New System.Drawing.Point(104, 44)
        Me.cbepCredits.Name = "cbepCredits"
        Me.cbepCredits.Size = New System.Drawing.Size(58, 17)
        Me.cbepCredits.TabIndex = 4
        Me.cbepCredits.Text = "Credits"
        Me.cbepCredits.UseVisualStyleBackColor = true
        '
        'cbepDirector
        '
        Me.cbepDirector.AutoSize = true
        Me.cbepDirector.Location = New System.Drawing.Point(104, 20)
        Me.cbepDirector.Name = "cbepDirector"
        Me.cbepDirector.Size = New System.Drawing.Size(63, 17)
        Me.cbepDirector.TabIndex = 3
        Me.cbepDirector.Text = "Director"
        Me.cbepDirector.UseVisualStyleBackColor = true
        '
        'cbepRating
        '
        Me.cbepRating.AutoSize = true
        Me.cbepRating.Location = New System.Drawing.Point(197, 20)
        Me.cbepRating.Name = "cbepRating"
        Me.cbepRating.Size = New System.Drawing.Size(57, 17)
        Me.cbepRating.TabIndex = 2
        Me.cbepRating.Text = "Rating"
        Me.cbepRating.UseVisualStyleBackColor = true
        '
        'cbepAired
        '
        Me.cbepAired.AutoSize = true
        Me.cbepAired.Location = New System.Drawing.Point(6, 44)
        Me.cbepAired.Name = "cbepAired"
        Me.cbepAired.Size = New System.Drawing.Size(64, 17)
        Me.cbepAired.TabIndex = 1
        Me.cbepAired.Text = "Air Date"
        Me.cbepAired.UseVisualStyleBackColor = true
        '
        'cbepPlot
        '
        Me.cbepPlot.AutoSize = true
        Me.cbepPlot.Location = New System.Drawing.Point(6, 20)
        Me.cbepPlot.Name = "cbepPlot"
        Me.cbepPlot.Size = New System.Drawing.Size(44, 17)
        Me.cbepPlot.TabIndex = 0
        Me.cbepPlot.Text = "Plot"
        Me.cbepPlot.UseVisualStyleBackColor = true
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(3, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Episode Batch Wizard"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = true
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.SplitContainer1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.cbShSeriesXML)
        Me.SplitContainer2.Panel2.Controls.Add(Me.cbRewriteAllNfo)
        Me.SplitContainer2.Panel2.Controls.Add(Me.btn_TvBatchCancel)
        Me.SplitContainer2.Panel2.Controls.Add(Me.btnTvBatchStart)
        Me.SplitContainer2.Panel2.Controls.Add(Me.cbincludeLocked)
        Me.SplitContainer2.Size = New System.Drawing.Size(414, 486)
        Me.SplitContainer2.SplitterDistance = 413
        Me.SplitContainer2.TabIndex = 1
        '
        'cbShSeriesXML
        '
        Me.cbShSeriesXML.AutoSize = true
        Me.cbShSeriesXML.Location = New System.Drawing.Point(18, 41)
        Me.cbShSeriesXML.Name = "cbShSeriesXML"
        Me.cbShSeriesXML.Size = New System.Drawing.Size(118, 17)
        Me.cbShSeriesXML.TabIndex = 15
        Me.cbShSeriesXML.Text = "Update Series XML"
        Me.cbShSeriesXML.UseVisualStyleBackColor = true
        '
        'cbRewriteAllNfo
        '
        Me.cbRewriteAllNfo.AutoSize = true
        Me.cbRewriteAllNfo.Location = New System.Drawing.Point(18, 25)
        Me.cbRewriteAllNfo.Name = "cbRewriteAllNfo"
        Me.cbRewriteAllNfo.Size = New System.Drawing.Size(108, 17)
        Me.cbRewriteAllNfo.TabIndex = 3
        Me.cbRewriteAllNfo.Text = "Rewrite All NFO's"
        Me.cbRewriteAllNfo.UseVisualStyleBackColor = true
        '
        'btn_TvBatchCancel
        '
        Me.btn_TvBatchCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_TvBatchCancel.Location = New System.Drawing.Point(210, 14)
        Me.btn_TvBatchCancel.Name = "btn_TvBatchCancel"
        Me.btn_TvBatchCancel.Size = New System.Drawing.Size(80, 36)
        Me.btn_TvBatchCancel.TabIndex = 2
        Me.btn_TvBatchCancel.Text = "Cancel"
        Me.btn_TvBatchCancel.UseVisualStyleBackColor = true
        '
        'btnTvBatchStart
        '
        Me.btnTvBatchStart.Location = New System.Drawing.Point(312, 14)
        Me.btnTvBatchStart.Name = "btnTvBatchStart"
        Me.btnTvBatchStart.Size = New System.Drawing.Size(83, 36)
        Me.btnTvBatchStart.TabIndex = 1
        Me.btnTvBatchStart.Text = "Start Update"
        Me.btnTvBatchStart.UseVisualStyleBackColor = true
        '
        'cbincludeLocked
        '
        Me.cbincludeLocked.AutoSize = true
        Me.cbincludeLocked.Location = New System.Drawing.Point(18, 8)
        Me.cbincludeLocked.Name = "cbincludeLocked"
        Me.cbincludeLocked.Size = New System.Drawing.Size(129, 17)
        Me.cbincludeLocked.TabIndex = 0
        Me.cbincludeLocked.Text = "Include locked shows"
        Me.cbincludeLocked.UseVisualStyleBackColor = true
        '
        'tv_batch_wizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btn_TvBatchCancel
        Me.ClientSize = New System.Drawing.Size(414, 486)
        Me.ControlBox = false
        Me.Controls.Add(Me.SplitContainer2)
        Me.KeyPreview = true
        Me.MaximumSize = New System.Drawing.Size(424, 520)
        Me.MinimumSize = New System.Drawing.Size(424, 520)
        Me.Name = "tv_batch_wizard"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "TV Batch Wizard"
        Me.SplitContainer1.Panel1.ResumeLayout(false)
        Me.SplitContainer1.Panel1.PerformLayout
        Me.SplitContainer1.Panel2.ResumeLayout(false)
        Me.SplitContainer1.Panel2.PerformLayout
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer1.ResumeLayout(false)
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.GroupBox3.ResumeLayout(false)
        Me.GroupBox3.PerformLayout
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.SplitContainer2.Panel1.ResumeLayout(false)
        Me.SplitContainer2.Panel2.ResumeLayout(false)
        Me.SplitContainer2.Panel2.PerformLayout
        CType(Me.SplitContainer2,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer2.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbshPosters As System.Windows.Forms.CheckBox
    Friend WithEvents cbshActor As System.Windows.Forms.CheckBox
    Friend WithEvents cbshStudio As System.Windows.Forms.CheckBox
    Friend WithEvents cbshGenre As System.Windows.Forms.CheckBox
    Friend WithEvents cbshMpaa As System.Windows.Forms.CheckBox
    Friend WithEvents cbshRuntime As System.Windows.Forms.CheckBox
    Friend WithEvents cbshPlot As System.Windows.Forms.CheckBox
    Friend WithEvents cbshRating As System.Windows.Forms.CheckBox
    Friend WithEvents cbshYear As System.Windows.Forms.CheckBox
    Friend WithEvents cbshFanart As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cbepCreateScreenshot As System.Windows.Forms.CheckBox
    Friend WithEvents cbepdlThumbnail As System.Windows.Forms.CheckBox
    Friend WithEvents cbepRuntime As System.Windows.Forms.CheckBox
    Friend WithEvents cbepActor As System.Windows.Forms.CheckBox
    Friend WithEvents cbepCredits As System.Windows.Forms.CheckBox
    Friend WithEvents cbepDirector As System.Windows.Forms.CheckBox
    Friend WithEvents cbepRating As System.Windows.Forms.CheckBox
    Friend WithEvents cbepAired As System.Windows.Forms.CheckBox
    Friend WithEvents cbepPlot As System.Windows.Forms.CheckBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents cbincludeLocked As System.Windows.Forms.CheckBox
    Friend WithEvents btnTvBatchStart As System.Windows.Forms.Button
    Friend WithEvents btn_TvBatchCancel As System.Windows.Forms.Button
    Friend WithEvents cbepStreamDetails As System.Windows.Forms.CheckBox
    Friend WithEvents cbRewriteAllNfo As System.Windows.Forms.CheckBox
    Friend WithEvents cbshSeason As System.Windows.Forms.CheckBox
    Friend WithEvents cbshXtraFanart As System.Windows.Forms.CheckBox
    Friend WithEvents cbshFanartTv As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cbepTitle As System.Windows.Forms.CheckBox
    Friend WithEvents cbshDelArtwork As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbshStatus As CheckBox
    Friend WithEvents cbshBannerMain As CheckBox
    Friend WithEvents cbShSeriesXML As CheckBox
    Friend WithEvents cbepIMDBId As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
