<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBatchScraper
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
        Me.gpbxMainTagsToRescrape = New System.Windows.Forms.GroupBox()
        Me.cbMainImdbAspectRatio = New System.Windows.Forms.CheckBox()
        Me.cbMainRating = New System.Windows.Forms.CheckBox()
        Me.cbMainMetascore = New System.Windows.Forms.CheckBox()
        Me.cbMainTitle = New System.Windows.Forms.CheckBox()
        Me.cbMainTmdbSetName = New System.Windows.Forms.CheckBox()
        Me.cbMainYear = New System.Windows.Forms.CheckBox()
        Me.cbMainStars = New System.Windows.Forms.CheckBox()
        Me.cbMainCountry = New System.Windows.Forms.CheckBox()
        Me.cbMainTop250 = New System.Windows.Forms.CheckBox()
        Me.cbMainTrailer = New System.Windows.Forms.CheckBox()
        Me.cbMainStudio = New System.Windows.Forms.CheckBox()
        Me.cbMainPremiered = New System.Windows.Forms.CheckBox()
        Me.cbMainDirector = New System.Windows.Forms.CheckBox()
        Me.cbMainCredits = New System.Windows.Forms.CheckBox()
        Me.cbMainGenre = New System.Windows.Forms.CheckBox()
        Me.cbMainCert = New System.Windows.Forms.CheckBox()
        Me.cbMainRuntime = New System.Windows.Forms.CheckBox()
        Me.cbMainTagline = New System.Windows.Forms.CheckBox()
        Me.cbMainPlot = New System.Windows.Forms.CheckBox()
        Me.cbMainOutline = New System.Windows.Forms.CheckBox()
        Me.cbMainVotes = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.gpbxArtwork = New System.Windows.Forms.GroupBox()
        Me.cbMissingMovSetArt = New System.Windows.Forms.CheckBox()
        Me.cbFanartTv = New System.Windows.Forms.CheckBox()
        Me.cbXtraFanart = New System.Windows.Forms.CheckBox()
        Me.cbMissingPosters = New System.Windows.Forms.CheckBox()
        Me.cbMissingFanart = New System.Windows.Forms.CheckBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.gbOther = New System.Windows.Forms.GroupBox()
        Me.cbDlTrailer = New System.Windows.Forms.CheckBox()
        Me.cbTagsFromKeywords = New System.Windows.Forms.CheckBox()
        Me.cbRenameFolders = New System.Windows.Forms.CheckBox()
        Me.cbFrodo_Fanart_Thumbs = New System.Windows.Forms.CheckBox()
        Me.cbFrodo_Poster_Thumbs = New System.Windows.Forms.CheckBox()
        Me.cbRenameFiles = New System.Windows.Forms.CheckBox()
        Me.cbRescrapePosterUrls = New System.Windows.Forms.CheckBox()
        Me.cbRescrapeActors = New System.Windows.Forms.CheckBox()
        Me.cbRescrapeMediaTags = New System.Windows.Forms.CheckBox()
        Me.lblMain = New System.Windows.Forms.Label()
        Me.ttBatchUpdateWizard = New System.Windows.Forms.ToolTip(Me.components)
        Me.cb_ScrapeEmptyTags = New System.Windows.Forms.CheckBox()
        Me.cbFromTMDB = New System.Windows.Forms.CheckBox()
        Me.cbMainTmdbSetInfo = New System.Windows.Forms.CheckBox()
        Me.gpbxMainTagsToRescrape.SuspendLayout
        Me.gpbxArtwork.SuspendLayout
        Me.gbOther.SuspendLayout
        Me.SuspendLayout
        '
        'gpbxMainTagsToRescrape
        '
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainTmdbSetInfo)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainImdbAspectRatio)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainRating)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainMetascore)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainTitle)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainTmdbSetName)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainYear)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainStars)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainCountry)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainTop250)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainTrailer)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainStudio)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainPremiered)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainDirector)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainCredits)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainGenre)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainCert)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainRuntime)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainTagline)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainPlot)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainOutline)
        Me.gpbxMainTagsToRescrape.Controls.Add(Me.cbMainVotes)
        Me.gpbxMainTagsToRescrape.Location = New System.Drawing.Point(13, 38)
        Me.gpbxMainTagsToRescrape.Name = "gpbxMainTagsToRescrape"
        Me.gpbxMainTagsToRescrape.Size = New System.Drawing.Size(456, 160)
        Me.gpbxMainTagsToRescrape.TabIndex = 0
        Me.gpbxMainTagsToRescrape.TabStop = false
        Me.gpbxMainTagsToRescrape.Text = "Select Main Tags to Rescrape"
        '
        'cbMainImdbAspectRatio
        '
        Me.cbMainImdbAspectRatio.AutoSize = true
        Me.cbMainImdbAspectRatio.Location = New System.Drawing.Point(7, 134)
        Me.cbMainImdbAspectRatio.Name = "cbMainImdbAspectRatio"
        Me.cbMainImdbAspectRatio.Size = New System.Drawing.Size(89, 17)
        Me.cbMainImdbAspectRatio.TabIndex = 30
        Me.cbMainImdbAspectRatio.Text = "IMDB Aspect"
        Me.cbMainImdbAspectRatio.UseVisualStyleBackColor = true
        '
        'cbMainRating
        '
        Me.cbMainRating.AutoSize = true
        Me.cbMainRating.Location = New System.Drawing.Point(7, 19)
        Me.cbMainRating.Name = "cbMainRating"
        Me.cbMainRating.Size = New System.Drawing.Size(57, 17)
        Me.cbMainRating.TabIndex = 10
        Me.cbMainRating.Text = "Rating"
        Me.cbMainRating.UseVisualStyleBackColor = true
        '
        'cbMainMetascore
        '
        Me.cbMainMetascore.AutoSize = true
        Me.cbMainMetascore.Location = New System.Drawing.Point(336, 111)
        Me.cbMainMetascore.Name = "cbMainMetascore"
        Me.cbMainMetascore.Size = New System.Drawing.Size(76, 17)
        Me.cbMainMetascore.TabIndex = 29
        Me.cbMainMetascore.Text = "Metascore"
        Me.cbMainMetascore.UseVisualStyleBackColor = true
        '
        'cbMainTitle
        '
        Me.cbMainTitle.AutoSize = true
        Me.cbMainTitle.Location = New System.Drawing.Point(226, 110)
        Me.cbMainTitle.Name = "cbMainTitle"
        Me.cbMainTitle.Size = New System.Drawing.Size(46, 17)
        Me.cbMainTitle.TabIndex = 28
        Me.cbMainTitle.Text = "Title"
        Me.cbMainTitle.UseVisualStyleBackColor = true
        '
        'cbMainTmdbSetName
        '
        Me.cbMainTmdbSetName.AutoSize = true
        Me.cbMainTmdbSetName.Location = New System.Drawing.Point(116, 134)
        Me.cbMainTmdbSetName.Name = "cbMainTmdbSetName"
        Me.cbMainTmdbSetName.Size = New System.Drawing.Size(99, 17)
        Me.cbMainTmdbSetName.TabIndex = 27
        Me.cbMainTmdbSetName.Text = "Tmdb set name"
        Me.cbMainTmdbSetName.UseVisualStyleBackColor = true
        '
        'cbMainYear
        '
        Me.cbMainYear.AutoSize = true
        Me.cbMainYear.Location = New System.Drawing.Point(7, 111)
        Me.cbMainYear.Name = "cbMainYear"
        Me.cbMainYear.Size = New System.Drawing.Size(48, 17)
        Me.cbMainYear.TabIndex = 26
        Me.cbMainYear.Text = "Year"
        Me.cbMainYear.UseVisualStyleBackColor = true
        '
        'cbMainStars
        '
        Me.cbMainStars.AutoSize = true
        Me.cbMainStars.Location = New System.Drawing.Point(336, 88)
        Me.cbMainStars.Name = "cbMainStars"
        Me.cbMainStars.Size = New System.Drawing.Size(50, 17)
        Me.cbMainStars.TabIndex = 25
        Me.cbMainStars.Text = "Stars"
        Me.cbMainStars.UseVisualStyleBackColor = true
        '
        'cbMainCountry
        '
        Me.cbMainCountry.AutoSize = true
        Me.cbMainCountry.Location = New System.Drawing.Point(336, 65)
        Me.cbMainCountry.Name = "cbMainCountry"
        Me.cbMainCountry.Size = New System.Drawing.Size(62, 17)
        Me.cbMainCountry.TabIndex = 21
        Me.cbMainCountry.Text = "Country"
        Me.cbMainCountry.UseVisualStyleBackColor = true
        '
        'cbMainTop250
        '
        Me.cbMainTop250.AutoSize = true
        Me.cbMainTop250.Location = New System.Drawing.Point(116, 19)
        Me.cbMainTop250.Name = "cbMainTop250"
        Me.cbMainTop250.Size = New System.Drawing.Size(63, 17)
        Me.cbMainTop250.TabIndex = 11
        Me.cbMainTop250.Text = "Top250"
        Me.cbMainTop250.UseVisualStyleBackColor = true
        '
        'cbMainTrailer
        '
        Me.cbMainTrailer.AutoSize = true
        Me.cbMainTrailer.Location = New System.Drawing.Point(226, 42)
        Me.cbMainTrailer.Name = "cbMainTrailer"
        Me.cbMainTrailer.Size = New System.Drawing.Size(55, 17)
        Me.cbMainTrailer.TabIndex = 16
        Me.cbMainTrailer.Text = "Trailer"
        Me.cbMainTrailer.UseVisualStyleBackColor = true
        '
        'cbMainStudio
        '
        Me.cbMainStudio.AutoSize = true
        Me.cbMainStudio.Location = New System.Drawing.Point(336, 42)
        Me.cbMainStudio.Name = "cbMainStudio"
        Me.cbMainStudio.Size = New System.Drawing.Size(56, 17)
        Me.cbMainStudio.TabIndex = 17
        Me.cbMainStudio.Text = "Studio"
        Me.cbMainStudio.UseVisualStyleBackColor = true
        '
        'cbMainPremiered
        '
        Me.cbMainPremiered.AutoSize = true
        Me.cbMainPremiered.Location = New System.Drawing.Point(336, 19)
        Me.cbMainPremiered.Name = "cbMainPremiered"
        Me.cbMainPremiered.Size = New System.Drawing.Size(73, 17)
        Me.cbMainPremiered.TabIndex = 13
        Me.cbMainPremiered.Text = "Premiered"
        Me.cbMainPremiered.UseVisualStyleBackColor = true
        '
        'cbMainDirector
        '
        Me.cbMainDirector.AutoSize = true
        Me.cbMainDirector.Location = New System.Drawing.Point(226, 65)
        Me.cbMainDirector.Name = "cbMainDirector"
        Me.cbMainDirector.Size = New System.Drawing.Size(63, 17)
        Me.cbMainDirector.TabIndex = 20
        Me.cbMainDirector.Text = "Director"
        Me.cbMainDirector.UseVisualStyleBackColor = true
        '
        'cbMainCredits
        '
        Me.cbMainCredits.AutoSize = true
        Me.cbMainCredits.Location = New System.Drawing.Point(226, 88)
        Me.cbMainCredits.Name = "cbMainCredits"
        Me.cbMainCredits.Size = New System.Drawing.Size(58, 17)
        Me.cbMainCredits.TabIndex = 24
        Me.cbMainCredits.Text = "Credits"
        Me.cbMainCredits.UseVisualStyleBackColor = true
        '
        'cbMainGenre
        '
        Me.cbMainGenre.AutoSize = true
        Me.cbMainGenre.Location = New System.Drawing.Point(7, 88)
        Me.cbMainGenre.Name = "cbMainGenre"
        Me.cbMainGenre.Size = New System.Drawing.Size(55, 17)
        Me.cbMainGenre.TabIndex = 22
        Me.cbMainGenre.Text = "Genre"
        Me.cbMainGenre.UseVisualStyleBackColor = true
        '
        'cbMainCert
        '
        Me.cbMainCert.AutoSize = true
        Me.cbMainCert.Location = New System.Drawing.Point(7, 65)
        Me.cbMainCert.Name = "cbMainCert"
        Me.cbMainCert.Size = New System.Drawing.Size(80, 17)
        Me.cbMainCert.TabIndex = 18
        Me.cbMainCert.Text = "MPAA/Cert"
        Me.cbMainCert.UseVisualStyleBackColor = true
        '
        'cbMainRuntime
        '
        Me.cbMainRuntime.AutoSize = true
        Me.cbMainRuntime.Location = New System.Drawing.Point(226, 19)
        Me.cbMainRuntime.Name = "cbMainRuntime"
        Me.cbMainRuntime.Size = New System.Drawing.Size(65, 17)
        Me.cbMainRuntime.TabIndex = 12
        Me.cbMainRuntime.Text = "Runtime"
        Me.cbMainRuntime.UseVisualStyleBackColor = true
        '
        'cbMainTagline
        '
        Me.cbMainTagline.AutoSize = true
        Me.cbMainTagline.Location = New System.Drawing.Point(116, 88)
        Me.cbMainTagline.Name = "cbMainTagline"
        Me.cbMainTagline.Size = New System.Drawing.Size(61, 17)
        Me.cbMainTagline.TabIndex = 23
        Me.cbMainTagline.Text = "Tagline"
        Me.cbMainTagline.UseVisualStyleBackColor = true
        '
        'cbMainPlot
        '
        Me.cbMainPlot.AutoSize = true
        Me.cbMainPlot.Location = New System.Drawing.Point(116, 65)
        Me.cbMainPlot.Name = "cbMainPlot"
        Me.cbMainPlot.Size = New System.Drawing.Size(44, 17)
        Me.cbMainPlot.TabIndex = 19
        Me.cbMainPlot.Text = "Plot"
        Me.cbMainPlot.UseVisualStyleBackColor = true
        '
        'cbMainOutline
        '
        Me.cbMainOutline.AutoSize = true
        Me.cbMainOutline.Location = New System.Drawing.Point(116, 42)
        Me.cbMainOutline.Name = "cbMainOutline"
        Me.cbMainOutline.Size = New System.Drawing.Size(59, 17)
        Me.cbMainOutline.TabIndex = 15
        Me.cbMainOutline.Text = "Outline"
        Me.cbMainOutline.UseVisualStyleBackColor = true
        '
        'cbMainVotes
        '
        Me.cbMainVotes.AutoSize = true
        Me.cbMainVotes.Location = New System.Drawing.Point(7, 42)
        Me.cbMainVotes.Name = "cbMainVotes"
        Me.cbMainVotes.Size = New System.Drawing.Size(53, 17)
        Me.cbMainVotes.TabIndex = 14
        Me.cbMainVotes.Text = "Votes"
        Me.cbMainVotes.UseVisualStyleBackColor = true
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(13, 480)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 50
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = true
        '
        'gpbxArtwork
        '
        Me.gpbxArtwork.Controls.Add(Me.cbMissingMovSetArt)
        Me.gpbxArtwork.Controls.Add(Me.cbFanartTv)
        Me.gpbxArtwork.Controls.Add(Me.cbXtraFanart)
        Me.gpbxArtwork.Controls.Add(Me.cbMissingPosters)
        Me.gpbxArtwork.Controls.Add(Me.cbMissingFanart)
        Me.gpbxArtwork.Location = New System.Drawing.Point(12, 318)
        Me.gpbxArtwork.Name = "gpbxArtwork"
        Me.gpbxArtwork.Size = New System.Drawing.Size(457, 132)
        Me.gpbxArtwork.TabIndex = 0
        Me.gpbxArtwork.TabStop = false
        Me.gpbxArtwork.Text = "Fanart && Posters"
        '
        'cbMissingMovSetArt
        '
        Me.cbMissingMovSetArt.AutoSize = true
        Me.cbMissingMovSetArt.Location = New System.Drawing.Point(7, 109)
        Me.cbMissingMovSetArt.Name = "cbMissingMovSetArt"
        Me.cbMissingMovSetArt.Size = New System.Drawing.Size(198, 17)
        Me.cbMissingMovSetArt.TabIndex = 45
        Me.cbMissingMovSetArt.Text = "Download missing MovieSet Artwork"
        Me.cbMissingMovSetArt.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.cbMissingMovSetArt.UseVisualStyleBackColor = true
        '
        'cbFanartTv
        '
        Me.cbFanartTv.AutoSize = true
        Me.cbFanartTv.Location = New System.Drawing.Point(7, 86)
        Me.cbFanartTv.Name = "cbFanartTv"
        Me.cbFanartTv.Size = New System.Drawing.Size(354, 17)
        Me.cbFanartTv.TabIndex = 44
        Me.cbFanartTv.Text = "Attempt to Download artwork from Fanart.Tv if Moviews are in Folders"
        Me.cbFanartTv.UseVisualStyleBackColor = true
        '
        'cbXtraFanart
        '
        Me.cbXtraFanart.AutoSize = true
        Me.cbXtraFanart.Location = New System.Drawing.Point(7, 65)
        Me.cbXtraFanart.Name = "cbXtraFanart"
        Me.cbXtraFanart.Size = New System.Drawing.Size(342, 17)
        Me.cbXtraFanart.TabIndex = 43
        Me.cbXtraFanart.Text = "Attempt to Download Extra Thumbs/Fanart if Movies are in Folders."
        Me.cbXtraFanart.UseVisualStyleBackColor = true
        '
        'cbMissingPosters
        '
        Me.cbMissingPosters.AutoSize = true
        Me.cbMissingPosters.Location = New System.Drawing.Point(7, 43)
        Me.cbMissingPosters.Name = "cbMissingPosters"
        Me.cbMissingPosters.Size = New System.Drawing.Size(392, 17)
        Me.cbMissingPosters.TabIndex = 42
        Me.cbMissingPosters.Text = "Attempt To Locate && Download Posters For Movies That Are Missing A Poster"
        Me.cbMissingPosters.UseVisualStyleBackColor = true
        '
        'cbMissingFanart
        '
        Me.cbMissingFanart.AutoSize = true
        Me.cbMissingFanart.Location = New System.Drawing.Point(7, 20)
        Me.cbMissingFanart.Name = "cbMissingFanart"
        Me.cbMissingFanart.Size = New System.Drawing.Size(403, 17)
        Me.cbMissingFanart.TabIndex = 41
        Me.cbMissingFanart.Text = "Attempt To Locate && Download Fanart For Movies That Are Missing A Backdrop"
        Me.cbMissingFanart.UseVisualStyleBackColor = true
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(393, 480)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 60
        Me.btnStart.Text = "Start Update"
        Me.btnStart.UseVisualStyleBackColor = true
        '
        'gbOther
        '
        Me.gbOther.Controls.Add(Me.cbDlTrailer)
        Me.gbOther.Controls.Add(Me.cbTagsFromKeywords)
        Me.gbOther.Controls.Add(Me.cbRenameFolders)
        Me.gbOther.Controls.Add(Me.cbFrodo_Fanart_Thumbs)
        Me.gbOther.Controls.Add(Me.cbFrodo_Poster_Thumbs)
        Me.gbOther.Controls.Add(Me.cbRenameFiles)
        Me.gbOther.Controls.Add(Me.cbRescrapePosterUrls)
        Me.gbOther.Controls.Add(Me.cbRescrapeActors)
        Me.gbOther.Controls.Add(Me.cbRescrapeMediaTags)
        Me.gbOther.Location = New System.Drawing.Point(13, 227)
        Me.gbOther.Name = "gbOther"
        Me.gbOther.Size = New System.Drawing.Size(456, 85)
        Me.gbOther.TabIndex = 0
        Me.gbOther.TabStop = false
        Me.gbOther.Text = "Other"
        '
        'cbDlTrailer
        '
        Me.cbDlTrailer.AutoSize = true
        Me.cbDlTrailer.Location = New System.Drawing.Point(291, 64)
        Me.cbDlTrailer.Name = "cbDlTrailer"
        Me.cbDlTrailer.Size = New System.Drawing.Size(159, 17)
        Me.cbDlTrailer.TabIndex = 39
        Me.cbDlTrailer.Text = "Download Trailer if Url exists"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cbDlTrailer, "Will only download trailer if"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Valid URL is already present")
        Me.cbDlTrailer.UseVisualStyleBackColor = true
        '
        'cbTagsFromKeywords
        '
        Me.cbTagsFromKeywords.AutoSize = true
        Me.cbTagsFromKeywords.Location = New System.Drawing.Point(123, 64)
        Me.cbTagsFromKeywords.Name = "cbTagsFromKeywords"
        Me.cbTagsFromKeywords.Size = New System.Drawing.Size(158, 17)
        Me.cbTagsFromKeywords.TabIndex = 38
        Me.cbTagsFromKeywords.Text = "Scrape Tags from keywords"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cbTagsFromKeywords, "Will scrape keywords from current selected scraper"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"and populate them into movie "& _ 
        "Tags.")
        Me.cbTagsFromKeywords.UseVisualStyleBackColor = true
        '
        'cbRenameFolders
        '
        Me.cbRenameFolders.AutoSize = true
        Me.cbRenameFolders.Location = New System.Drawing.Point(7, 64)
        Me.cbRenameFolders.Name = "cbRenameFolders"
        Me.cbRenameFolders.Size = New System.Drawing.Size(103, 17)
        Me.cbRenameFolders.TabIndex = 37
        Me.cbRenameFolders.Text = "Rename Folders"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cbRenameFolders, "Renaming of Movie Folders as per 'Folder Renaming Pattern'"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Option to rename fold"& _ 
        "ers during Autoscrape does not need to"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"be enabled, but Folder name pattern MUST"& _ 
        " be valid.")
        Me.cbRenameFolders.UseVisualStyleBackColor = true
        '
        'cbFrodo_Fanart_Thumbs
        '
        Me.cbFrodo_Fanart_Thumbs.AutoSize = true
        Me.cbFrodo_Fanart_Thumbs.Location = New System.Drawing.Point(291, 41)
        Me.cbFrodo_Fanart_Thumbs.Name = "cbFrodo_Fanart_Thumbs"
        Me.cbFrodo_Fanart_Thumbs.Size = New System.Drawing.Size(127, 17)
        Me.cbFrodo_Fanart_Thumbs.TabIndex = 36
        Me.cbFrodo_Fanart_Thumbs.Text = "Frodo Fanart Thumbs"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cbFrodo_Fanart_Thumbs, "Frodo support must be enabled before this option can be checked")
        Me.cbFrodo_Fanart_Thumbs.UseVisualStyleBackColor = true
        '
        'cbFrodo_Poster_Thumbs
        '
        Me.cbFrodo_Poster_Thumbs.AutoSize = true
        Me.cbFrodo_Poster_Thumbs.Location = New System.Drawing.Point(123, 41)
        Me.cbFrodo_Poster_Thumbs.Name = "cbFrodo_Poster_Thumbs"
        Me.cbFrodo_Poster_Thumbs.Size = New System.Drawing.Size(127, 17)
        Me.cbFrodo_Poster_Thumbs.TabIndex = 35
        Me.cbFrodo_Poster_Thumbs.Text = "Frodo Poster Thumbs"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cbFrodo_Poster_Thumbs, "Frodo support must be enabled before this option can be checked")
        Me.cbFrodo_Poster_Thumbs.UseVisualStyleBackColor = true
        '
        'cbRenameFiles
        '
        Me.cbRenameFiles.AutoSize = true
        Me.cbRenameFiles.Location = New System.Drawing.Point(7, 41)
        Me.cbRenameFiles.Name = "cbRenameFiles"
        Me.cbRenameFiles.Size = New System.Drawing.Size(90, 17)
        Me.cbRenameFiles.TabIndex = 34
        Me.cbRenameFiles.Text = "Rename Files"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cbRenameFiles, "Renames movie files to match the selected 'Name Mode' and your 'Movie Renaming' p"& _ 
        "attern (Must be enabled). "&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Conflicting options 'Save files as 'movie.nfo...' (A"& _ 
        "dvanced tab) must be disabled.")
        Me.cbRenameFiles.UseVisualStyleBackColor = true
        '
        'cbRescrapePosterUrls
        '
        Me.cbRescrapePosterUrls.AutoSize = true
        Me.cbRescrapePosterUrls.Location = New System.Drawing.Point(291, 19)
        Me.cbRescrapePosterUrls.Name = "cbRescrapePosterUrls"
        Me.cbRescrapePosterUrls.Size = New System.Drawing.Size(141, 17)
        Me.cbRescrapePosterUrls.TabIndex = 33
        Me.cbRescrapePosterUrls.Text = "Rescrape nfo poster urls"
        Me.cbRescrapePosterUrls.UseVisualStyleBackColor = true
        '
        'cbRescrapeActors
        '
        Me.cbRescrapeActors.AutoSize = true
        Me.cbRescrapeActors.Location = New System.Drawing.Point(7, 19)
        Me.cbRescrapeActors.Name = "cbRescrapeActors"
        Me.cbRescrapeActors.Size = New System.Drawing.Size(105, 17)
        Me.cbRescrapeActors.TabIndex = 31
        Me.cbRescrapeActors.Text = "Rescrape Actors"
        Me.cbRescrapeActors.UseVisualStyleBackColor = true
        '
        'cbRescrapeMediaTags
        '
        Me.cbRescrapeMediaTags.AutoSize = true
        Me.cbRescrapeMediaTags.Location = New System.Drawing.Point(123, 19)
        Me.cbRescrapeMediaTags.Name = "cbRescrapeMediaTags"
        Me.cbRescrapeMediaTags.Size = New System.Drawing.Size(131, 17)
        Me.cbRescrapeMediaTags.TabIndex = 32
        Me.cbRescrapeMediaTags.Text = "Rescrape Media Tags"
        Me.cbRescrapeMediaTags.UseVisualStyleBackColor = true
        '
        'lblMain
        '
        Me.lblMain.AutoSize = true
        Me.lblMain.Location = New System.Drawing.Point(12, 9)
        Me.lblMain.Name = "lblMain"
        Me.lblMain.Size = New System.Drawing.Size(440, 26)
        Me.lblMain.TabIndex = 0
        Me.lblMain.Text = "This form can be used to rescrape all movies, you can select which tags are updat"& _ 
    "ed below,"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"all other tags will remain unchanged."
        '
        'ttBatchUpdateWizard
        '
        Me.ttBatchUpdateWizard.AutoPopDelay = 10000
        Me.ttBatchUpdateWizard.InitialDelay = 500
        Me.ttBatchUpdateWizard.ReshowDelay = 100
        Me.ttBatchUpdateWizard.ShowAlways = true
        '
        'cb_ScrapeEmptyTags
        '
        Me.cb_ScrapeEmptyTags.AutoSize = true
        Me.cb_ScrapeEmptyTags.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cb_ScrapeEmptyTags.Location = New System.Drawing.Point(45, 204)
        Me.cb_ScrapeEmptyTags.Name = "cb_ScrapeEmptyTags"
        Me.cb_ScrapeEmptyTags.Size = New System.Drawing.Size(367, 17)
        Me.cb_ScrapeEmptyTags.TabIndex = 29
        Me.cb_ScrapeEmptyTags.Text = "Scrape only empty tags (Excludes Tmdb set info and Trailer)"
        Me.ttBatchUpdateWizard.SetToolTip(Me.cb_ScrapeEmptyTags, "Selecting will re-scrape all selected movies, but ONLY"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"filling fields that are e"& _ 
        "mpty."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"This excludes Tmdb set info.  Select this separately if"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"required.")
        Me.cb_ScrapeEmptyTags.UseVisualStyleBackColor = true
        '
        'cbFromTMDB
        '
        Me.cbFromTMDB.AutoSize = true
        Me.cbFromTMDB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbFromTMDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbFromTMDB.Location = New System.Drawing.Point(44, 456)
        Me.cbFromTMDB.Name = "cbFromTMDB"
        Me.cbFromTMDB.Size = New System.Drawing.Size(427, 17)
        Me.cbFromTMDB.TabIndex = 61
        Me.cbFromTMDB.Text = "Scrape main tags from TMDB (excluding those tags exclusive to IMDB)"
        Me.cbFromTMDB.UseVisualStyleBackColor = true
        '
        'cbMainTmdbSetInfo
        '
        Me.cbMainTmdbSetInfo.AutoSize = true
        Me.cbMainTmdbSetInfo.Location = New System.Drawing.Point(226, 134)
        Me.cbMainTmdbSetInfo.Name = "cbMainTmdbSetInfo"
        Me.cbMainTmdbSetInfo.Size = New System.Drawing.Size(90, 17)
        Me.cbMainTmdbSetInfo.TabIndex = 31
        Me.cbMainTmdbSetInfo.Text = "Tmdb set info"
        Me.cbMainTmdbSetInfo.UseVisualStyleBackColor = true
        '
        'frmBatchScraper
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(483, 511)
        Me.ControlBox = false
        Me.Controls.Add(Me.cbFromTMDB)
        Me.Controls.Add(Me.cb_ScrapeEmptyTags)
        Me.Controls.Add(Me.gpbxMainTagsToRescrape)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.gpbxArtwork)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.gbOther)
        Me.Controls.Add(Me.lblMain)
        Me.KeyPreview = true
        Me.MaximumSize = New System.Drawing.Size(493, 545)
        Me.MinimizeBox = false
        Me.MinimumSize = New System.Drawing.Size(493, 545)
        Me.Name = "frmBatchScraper"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Batch Update Wizard"
        Me.gpbxMainTagsToRescrape.ResumeLayout(false)
        Me.gpbxMainTagsToRescrape.PerformLayout
        Me.gpbxArtwork.ResumeLayout(false)
        Me.gpbxArtwork.PerformLayout
        Me.gbOther.ResumeLayout(false)
        Me.gbOther.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents gpbxMainTagsToRescrape As System.Windows.Forms.GroupBox
    Friend WithEvents cbMainTop250 As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainStudio As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainPremiered As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainDirector As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainCredits As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainGenre As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainCert As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainRuntime As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainTagline As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainPlot As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainOutline As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainVotes As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainRating As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gpbxArtwork As System.Windows.Forms.GroupBox
    Friend WithEvents cbMissingPosters As System.Windows.Forms.CheckBox
    Friend WithEvents cbMissingFanart As System.Windows.Forms.CheckBox
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents gbOther As System.Windows.Forms.GroupBox
    Friend WithEvents cbRescrapeMediaTags As System.Windows.Forms.CheckBox
    Friend WithEvents cbRescrapeActors As System.Windows.Forms.CheckBox
    Friend WithEvents lblMain As System.Windows.Forms.Label
    Friend WithEvents cbRescrapePosterUrls As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainCountry As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainStars As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainYear As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainTmdbSetName As System.Windows.Forms.CheckBox
    Friend WithEvents cbRenameFiles As System.Windows.Forms.CheckBox
    Friend WithEvents ttBatchUpdateWizard As System.Windows.Forms.ToolTip
    Friend WithEvents cbFrodo_Fanart_Thumbs As System.Windows.Forms.CheckBox
    Friend WithEvents cbFrodo_Poster_Thumbs As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainTitle As System.Windows.Forms.CheckBox
    Friend WithEvents cbXtraFanart As System.Windows.Forms.CheckBox
    Friend WithEvents cbRenameFolders As System.Windows.Forms.CheckBox
    Friend WithEvents cb_ScrapeEmptyTags As System.Windows.Forms.CheckBox
    Friend WithEvents cbTagsFromKeywords As System.Windows.Forms.CheckBox
    Friend WithEvents cbDlTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents cbFromTMDB As System.Windows.Forms.CheckBox
    Friend WithEvents cbFanartTv As System.Windows.Forms.CheckBox
    Friend WithEvents cbMissingMovSetArt As System.Windows.Forms.CheckBox
    Friend WithEvents cbMainMetascore As CheckBox
    Friend WithEvents cbMainImdbAspectRatio As CheckBox
    Friend WithEvents cbMainTmdbSetInfo As CheckBox
End Class
