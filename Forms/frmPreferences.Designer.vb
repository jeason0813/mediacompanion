<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPreferences
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPreferences))
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.FontDialog = New System.Windows.Forms.FontDialog()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.cbLocalActorSaveNoId = New System.Windows.Forms.CheckBox()
        Me.cbLocalActorSaveAlpha = New System.Windows.Forms.CheckBox()
        Me.xbmcactorpath = New System.Windows.Forms.TextBox()
        Me.btn_localactorpathbrowse = New System.Windows.Forms.Button()
        Me.Label161 = New System.Windows.Forms.Label()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.localactorpath = New System.Windows.Forms.TextBox()
        Me.cbsaveactor = New System.Windows.Forms.CheckBox()
        Me.cmbx_MovMaxActors = New System.Windows.Forms.ComboBox()
        Me.cbShowAllAudioTracks = New System.Windows.Forms.CheckBox()
        Me.cbDisplayMediaInfoOverlay = New System.Windows.Forms.CheckBox()
        Me.cbDisplayRatingOverlay = New System.Windows.Forms.CheckBox()
        Me.gbExcludeFolders = New System.Windows.Forms.GroupBox()
        Me.tbExcludeFolders = New System.Windows.Forms.TextBox()
        Me.cb_keywordlimit = New System.Windows.Forms.ComboBox()
        Me.cbMovieBasicSave = New System.Windows.Forms.CheckBox()
        Me.cbGetMovieSetFromTMDb = New System.Windows.Forms.CheckBox()
        Me.cmbxMovieScraper_MaxStudios = New System.Windows.Forms.ComboBox()
        Me.cmbxMovScraper_MaxGenres = New System.Windows.Forms.ComboBox()
        Me.cbMovCreateFanartjpg = New System.Windows.Forms.CheckBox()
        Me.cbMoviePosterInFolder = New System.Windows.Forms.CheckBox()
        Me.cbMovieFanartInFolders = New System.Windows.Forms.CheckBox()
        Me.cbMovXtraFanart = New System.Windows.Forms.CheckBox()
        Me.cbMovXtraThumbs = New System.Windows.Forms.CheckBox()
        Me.cbMovFanartTvScrape = New System.Windows.Forms.CheckBox()
        Me.cbDlXtraFanart = New System.Windows.Forms.CheckBox()
        Me.cbMovFanartScrape = New System.Windows.Forms.CheckBox()
        Me.cbMoviePosterScrape = New System.Windows.Forms.CheckBox()
        Me.tbDateFormat = New System.Windows.Forms.TextBox()
        Me.Label179 = New System.Windows.Forms.Label()
        Me.rbRenameFullStop = New System.Windows.Forms.RadioButton()
        Me.rbRenameUnderscore = New System.Windows.Forms.RadioButton()
        Me.cbMovTitleCase = New System.Windows.Forms.CheckBox()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.cbPreferredTrailerResolution = New System.Windows.Forms.ComboBox()
        Me.cbDlTrailerDuringScrape = New System.Windows.Forms.CheckBox()
        Me.cbMovieRuntimeFallbackToFile = New System.Windows.Forms.CheckBox()
        Me.grpbxImdbCertPriority = New System.Windows.Forms.GroupBox()
        Me.cbIncludeMpaaRated = New System.Windows.Forms.CheckBox()
        Me.cbExcludeMpaaRated = New System.Windows.Forms.CheckBox()
        Me.cb_MovCertRemovePhrase = New System.Windows.Forms.CheckBox()
        Me.ScrapeFullCertCheckBox = New System.Windows.Forms.CheckBox()
        Me.Label178 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.btnMovImdbPriorityDown = New System.Windows.Forms.Button()
        Me.btnMovImdbPriorityUp = New System.Windows.Forms.Button()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.lb_IMDBCertPriority = New System.Windows.Forms.ListBox()
        Me.cbMovRootFolderCheck = New System.Windows.Forms.CheckBox()
        Me.btn_tv_RegexRename_MoveDown = New System.Windows.Forms.Button()
        Me.btn_tv_RegexRename_MoveUp = New System.Windows.Forms.Button()
        Me.btn_tv_RegexScrape_MoveDown = New System.Windows.Forms.Button()
        Me.btn_tv_RegexScrape_MoveUp = New System.Windows.Forms.Button()
        Me.cb_MovPosterTabTMDBSelect = New System.Windows.Forms.CheckBox()
        Me.cbAllowUserTags = New System.Windows.Forms.CheckBox()
        Me.cbEnableFolderSize = New System.Windows.Forms.CheckBox()
        Me.cbDisplayLocalActor = New System.Windows.Forms.CheckBox()
        Me.cbCheckForNewVersion = New System.Windows.Forms.CheckBox()
        Me.cbUseMultipleThreads = New System.Windows.Forms.CheckBox()
        Me.btnFindBrowser = New System.Windows.Forms.Button()
        Me.tbaltnfoeditor = New System.Windows.Forms.TextBox()
        Me.GroupBox36 = New System.Windows.Forms.GroupBox()
        Me.llMkvMergeGuiPath = New System.Windows.Forms.LinkLabel()
        Me.btnMkvMergeGuiPath = New System.Windows.Forms.Button()
        Me.tbMkvMergeGuiPath = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.cmbxTvMaxGenres = New System.Windows.Forms.ComboBox()
        Me.cbDisplayDefaultSubtitleLang = New System.Windows.Forms.CheckBox()
        Me.cbSetIdAsCollectionnumber = New System.Windows.Forms.CheckBox()
        Me.cbtvDisplayNextAiredToolTip = New System.Windows.Forms.CheckBox()
        Me.cbEnableMovDeleteFolderTsmi = New System.Windows.Forms.CheckBox()
        Me.cbMovAllowNonImdbIdAsId = New System.Windows.Forms.CheckBox()
        Me.rbMovSetArtScrapeFanartTv = New System.Windows.Forms.RadioButton()
        Me.cbXbmcTmdbActorFromImdb = New System.Windows.Forms.CheckBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TPGenCom = New System.Windows.Forms.TabPage()
        Me.TabControl4 = New System.Windows.Forms.TabControl()
        Me.TPGeneral = New System.Windows.Forms.TabPage()
        Me.lbExternalBrowserPath = New System.Windows.Forms.Label()
        Me.cbAutoHideStatusBar = New System.Windows.Forms.CheckBox()
        Me.cbMcCloseMCForDLNewVersion = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbMultiMonitorEnable = New System.Windows.Forms.CheckBox()
        Me.cbRenameNFOtoINFO = New System.Windows.Forms.CheckBox()
        Me.cbShowLogOnError = New System.Windows.Forms.CheckBox()
        Me.cbExternalbrowser = New System.Windows.Forms.CheckBox()
        Me.chkbx_disablecache = New System.Windows.Forms.CheckBox()
        Me.GroupBox45 = New System.Windows.Forms.GroupBox()
        Me.lblaltnfoeditorclear = New System.Windows.Forms.Label()
        Me.btnaltnfoeditor = New System.Windows.Forms.Button()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.GroupBox33 = New System.Windows.Forms.GroupBox()
        Me.btnFontSelect = New System.Windows.Forms.Button()
        Me.btnFontReset = New System.Windows.Forms.Button()
        Me.lbl_FontSample = New System.Windows.Forms.Label()
        Me.GroupBox31 = New System.Windows.Forms.GroupBox()
        Me.Label116 = New System.Windows.Forms.Label()
        Me.Label107 = New System.Windows.Forms.Label()
        Me.txtbx_minrarsize = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_MediaPlayerUser = New System.Windows.Forms.Label()
        Me.btn_MediaPlayerBrowse = New System.Windows.Forms.Button()
        Me.rb_MediaPlayerUser = New System.Windows.Forms.RadioButton()
        Me.rb_MediaPlayerWMP = New System.Windows.Forms.RadioButton()
        Me.rb_MediaPlayerDefault = New System.Windows.Forms.RadioButton()
        Me.TPCommonSettings = New System.Windows.Forms.TabPage()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.cbRunTimePadding = New System.Windows.Forms.CheckBox()
        Me.gpbxPrefIgnore = New System.Windows.Forms.GroupBox()
        Me.cb_IgnoreAn = New System.Windows.Forms.CheckBox()
        Me.cb_IgnoreThe = New System.Windows.Forms.CheckBox()
        Me.cb_IgnoreA = New System.Windows.Forms.CheckBox()
        Me.gpbxPrefOverlay = New System.Windows.Forms.GroupBox()
        Me.cbDisplayAllSubtitleLang = New System.Windows.Forms.CheckBox()
        Me.cbDisplayMediaInfoFolderSize = New System.Windows.Forms.CheckBox()
        Me.cbGenreCustomBefore = New System.Windows.Forms.CheckBox()
        Me.btnEditCustomGenreFile = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label185 = New System.Windows.Forms.Label()
        Me.AutoScrnShtDelay = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.gbImageResizing = New System.Windows.Forms.GroupBox()
        Me.Label171 = New System.Windows.Forms.Label()
        Me.Label175 = New System.Windows.Forms.Label()
        Me.Label176 = New System.Windows.Forms.Label()
        Me.comboActorResolutions = New System.Windows.Forms.ComboBox()
        Me.comboBackDropResolutions = New System.Windows.Forms.ComboBox()
        Me.comboPosterResolutions = New System.Windows.Forms.ComboBox()
        Me.grpCleanFilename = New System.Windows.Forms.GroupBox()
        Me.btnCleanFilenameRemove = New System.Windows.Forms.Button()
        Me.txtCleanFilenameAdd = New System.Windows.Forms.TextBox()
        Me.btnCleanFilenameAdd = New System.Windows.Forms.Button()
        Me.lbCleanFilename = New System.Windows.Forms.ListBox()
        Me.grpVideoSource = New System.Windows.Forms.GroupBox()
        Me.btnVideoSourceRemove = New System.Windows.Forms.Button()
        Me.txtVideoSourceAdd = New System.Windows.Forms.TextBox()
        Me.btnVideoSourceAdd = New System.Windows.Forms.Button()
        Me.lbVideoSource = New System.Windows.Forms.ListBox()
        Me.cbOverwriteArtwork = New System.Windows.Forms.CheckBox()
        Me.cbRuntimeAsNumericOnly = New System.Windows.Forms.CheckBox()
        Me.gbxXBMCversion = New System.Windows.Forms.GroupBox()
        Me.Label129 = New System.Windows.Forms.Label()
        Me.rbXBMCv_both = New System.Windows.Forms.RadioButton()
        Me.rbXBMCv_post = New System.Windows.Forms.RadioButton()
        Me.rbXBMCv_pre = New System.Windows.Forms.RadioButton()
        Me.TPActors = New System.Windows.Forms.TabPage()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.GroupBox32 = New System.Windows.Forms.GroupBox()
        Me.cb_ExcludeActorNoThumb = New System.Windows.Forms.CheckBox()
        Me.Label137 = New System.Windows.Forms.Label()
        Me.cb_actorseasy = New System.Windows.Forms.CheckBox()
        Me.TPMovPref = New System.Windows.Forms.TabPage()
        Me.tcMoviePreferences = New System.Windows.Forms.TabControl()
        Me.tpMoviePreferences_Scraper = New System.Windows.Forms.TabPage()
        Me.grpbxTMDbApi = New System.Windows.Forms.GroupBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.tbTMDbAPI = New System.Windows.Forms.TextBox()
        Me.grpbxMovieScraperSettings = New System.Windows.Forms.GroupBox()
        Me.grpbxTMDBScraperPreferences = New System.Windows.Forms.GroupBox()
        Me.cbMovActorFallbackIMDbtoTMDb = New System.Windows.Forms.CheckBox()
        Me.cmbxTMDBPreferredCertCountry = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpbxTmdbfromImdbOptions = New System.Windows.Forms.GroupBox()
        Me.cbXbmcTmdbMetascoreFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbAspectFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbGenreFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbAkasFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbCertFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbVotesFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbTop250FromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbIMDBRatings = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbStarsFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbOutlineFromImdb = New System.Windows.Forms.CheckBox()
        Me.cbXbmcTmdbRename = New System.Windows.Forms.CheckBox()
        Me.Label155 = New System.Windows.Forms.Label()
        Me.cmbxXbmcTmdbTitleLanguage = New System.Windows.Forms.ComboBox()
        Me.cbXbmcTmdbFanart = New System.Windows.Forms.CheckBox()
        Me.cmbxXbmcTmdbHDTrailer = New System.Windows.Forms.ComboBox()
        Me.Label153 = New System.Windows.Forms.Label()
        Me.cbMovieUseXBMCScraper = New System.Windows.Forms.CheckBox()
        Me.GpBx_McIMDbScraperSettings = New System.Windows.Forms.GroupBox()
        Me.cbMovActorFallbackTMDbtoIMDb = New System.Windows.Forms.CheckBox()
        Me.cbMovImdbAspectRatio = New System.Windows.Forms.CheckBox()
        Me.cbMovImdbFirstRunTime = New System.Windows.Forms.CheckBox()
        Me.cbImdbPrimaryPlot = New System.Windows.Forms.CheckBox()
        Me.cbImdbgetTMDBActor = New System.Windows.Forms.CheckBox()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.lb_IMDBMirrors = New System.Windows.Forms.ListBox()
        Me.gpbxPrefScraperImages = New System.Windows.Forms.GroupBox()
        Me.grpbxMovieTagOptions = New System.Windows.Forms.GroupBox()
        Me.cbTagRes = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tb_MovTagBlacklist = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.cb_keywordasTag = New System.Windows.Forms.CheckBox()
        Me.grpbxMovieBasicSave = New System.Windows.Forms.GroupBox()
        Me.Label162 = New System.Windows.Forms.Label()
        Me.Label109 = New System.Windows.Forms.Label()
        Me.grpbxScraperLimits = New System.Windows.Forms.GroupBox()
        Me.lblMaxStudios = New System.Windows.Forms.Label()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.grpbxScraperMisc = New System.Windows.Forms.GroupBox()
        Me.cbMovSetOverviewToNfo = New System.Windows.Forms.CheckBox()
        Me.cbIMDbOriginalTitle = New System.Windows.Forms.CheckBox()
        Me.grpbxMovPreferLang = New System.Windows.Forms.GroupBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.grpbxCustomLanguage = New System.Windows.Forms.GroupBox()
        Me.llLanguagesFile = New System.Windows.Forms.LinkLabel()
        Me.tbCustomLanguageValue = New System.Windows.Forms.TextBox()
        Me.Label174 = New System.Windows.Forms.Label()
        Me.Label177 = New System.Windows.Forms.Label()
        Me.cbUseCustomLanguage = New System.Windows.Forms.CheckBox()
        Me.cmbxTMDbSelectedLanguage = New System.Windows.Forms.ComboBox()
        Me.grpbxMovieFolderOptions = New System.Windows.Forms.GroupBox()
        Me.cbMovieAllInFolders = New System.Windows.Forms.CheckBox()
        Me.cbMovieUseFolderNames = New System.Windows.Forms.CheckBox()
        Me.tpMoviePreferences_Artwork = New System.Windows.Forms.TabPage()
        Me.grpbxMovieSetArtwork = New System.Windows.Forms.GroupBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.tbMovSetArtCentralFolder = New System.Windows.Forms.TextBox()
        Me.btnMovSetCentralFolderSelect = New System.Windows.Forms.Button()
        Me.rbMovSetArtSetFolder = New System.Windows.Forms.RadioButton()
        Me.rbMovSetFolder = New System.Windows.Forms.RadioButton()
        Me.GrpbxXtraArtwork = New System.Windows.Forms.GroupBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.cmbxMovXtraFanartQty = New System.Windows.Forms.ComboBox()
        Me.grpbxMoviesInFoldersArtwork = New System.Windows.Forms.GroupBox()
        Me.cbMovCreateFolderjpg = New System.Windows.Forms.CheckBox()
        Me.grpbxMoviePosterPriority = New System.Windows.Forms.GroupBox()
        Me.btn_MovPosterPriorityRemove = New System.Windows.Forms.Button()
        Me.btn_MovPosterPriorityReset = New System.Windows.Forms.Button()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.btnMovPosterPriorityDown = New System.Windows.Forms.Button()
        Me.btnMovPosterPriorityUp = New System.Windows.Forms.Button()
        Me.lbPosterSourcePriorities = New System.Windows.Forms.ListBox()
        Me.grpbxAutoScrapeArtwork = New System.Windows.Forms.GroupBox()
        Me.rbMovSetArtScrapeTMDb = New System.Windows.Forms.RadioButton()
        Me.cbMovCustPosterjpgNoDelete = New System.Windows.Forms.CheckBox()
        Me.cbMovCustFolderjpgNoDelete = New System.Windows.Forms.CheckBox()
        Me.cbMovFanartNaming = New System.Windows.Forms.CheckBox()
        Me.btnMovFanartTvSelect = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.grpbxExtraArtwork = New System.Windows.Forms.GroupBox()
        Me.Label189 = New System.Windows.Forms.Label()
        Me.cbMovSetArtScrape = New System.Windows.Forms.CheckBox()
        Me.tpMoviePreferences_General = New System.Windows.Forms.TabPage()
        Me.grpbxMovieFilters = New System.Windows.Forms.GroupBox()
        Me.cmbxMovFiltersTagsOrder = New System.Windows.Forms.ComboBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.nudMaxTagsInFilter = New System.Windows.Forms.NumericUpDown()
        Me.nudMinTagsInFilter = New System.Windows.Forms.NumericUpDown()
        Me.cbMovieFilters_Sets_Order = New System.Windows.Forms.ComboBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.cbMovieFilters_Directors_Order = New System.Windows.Forms.ComboBox()
        Me.nudMaxDirectorsInFilter = New System.Windows.Forms.NumericUpDown()
        Me.nudMaxSetsInFilter = New System.Windows.Forms.NumericUpDown()
        Me.nudDirectorsFilterMinFilms = New System.Windows.Forms.NumericUpDown()
        Me.nudSetsFilterMinFilms = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cbMovieFilters_Actors_Order = New System.Windows.Forms.ComboBox()
        Me.cbDisableNotMatchingRenamePattern = New System.Windows.Forms.CheckBox()
        Me.Label180 = New System.Windows.Forms.Label()
        Me.Label164 = New System.Windows.Forms.Label()
        Me.nudMaxActorsInFilter = New System.Windows.Forms.NumericUpDown()
        Me.nudActorsFilterMinFilms = New System.Windows.Forms.NumericUpDown()
        Me.Label165 = New System.Windows.Forms.Label()
        Me.grpbxMovieListOptions = New System.Windows.Forms.GroupBox()
        Me.cbMovieList_ShowColWatched = New System.Windows.Forms.CheckBox()
        Me.cbMovieList_ShowColPlot = New System.Windows.Forms.CheckBox()
        Me.cbMovieShowDateOnList = New System.Windows.Forms.CheckBox()
        Me.grpbxMovieRenameOptions = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cbMovNewFolderInRootFolder = New System.Windows.Forms.CheckBox()
        Me.cbMovSortIgnArticle = New System.Windows.Forms.CheckBox()
        Me.cbMovTitleIgnArticle = New System.Windows.Forms.CheckBox()
        Me.cbMovSetIgnArticle = New System.Windows.Forms.CheckBox()
        Me.Label197 = New System.Windows.Forms.Label()
        Me.Label196 = New System.Windows.Forms.Label()
        Me.cbMovFolderRename = New System.Windows.Forms.CheckBox()
        Me.cbRenameUnderscore = New System.Windows.Forms.CheckBox()
        Me.lblFolderRename = New System.Windows.Forms.Label()
        Me.tb_MovFolderRename = New System.Windows.Forms.TextBox()
        Me.LblFilename = New System.Windows.Forms.Label()
        Me.cbMovieManualRename = New System.Windows.Forms.CheckBox()
        Me.cbMovieRenameEnable = New System.Windows.Forms.CheckBox()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.tb_MovieRenameTemplate = New System.Windows.Forms.TextBox()
        Me.grpbxMovieOfflineTitleText = New System.Windows.Forms.GroupBox()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.tb_OfflineDVDTitle = New System.Windows.Forms.TextBox()
        Me.grpbxMovieGeneralOptions = New System.Windows.Forms.GroupBox()
        Me.cb_MovRuntimeAsDuration = New System.Windows.Forms.CheckBox()
        Me.cbShowMovieGridToolTip = New System.Windows.Forms.CheckBox()
        Me.cb_MovSetTitleIgnArticle = New System.Windows.Forms.CheckBox()
        Me.cb_SorttitleIgnoreArticles = New System.Windows.Forms.CheckBox()
        Me.cb_MovDurationAsRuntine = New System.Windows.Forms.CheckBox()
        Me.cbMissingMovie = New System.Windows.Forms.CheckBox()
        Me.cbMovThousSeparator = New System.Windows.Forms.CheckBox()
        Me.cbXtraFrodoUrls = New System.Windows.Forms.CheckBox()
        Me.cbNoAltTitle = New System.Windows.Forms.CheckBox()
        Me.PanelDisplayRuntime = New System.Windows.Forms.Panel()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.rbRuntimeFile = New System.Windows.Forms.RadioButton()
        Me.rbRuntimeScraper = New System.Windows.Forms.RadioButton()
        Me.cb_MovDisplayLog = New System.Windows.Forms.CheckBox()
        Me.cb_EnableMediaTags = New System.Windows.Forms.CheckBox()
        Me.cbMovieTrailerUrl = New System.Windows.Forms.CheckBox()
        Me.grpbxMovieNameMode = New System.Windows.Forms.GroupBox()
        Me.Label163 = New System.Windows.Forms.Label()
        Me.lblNameMode = New System.Windows.Forms.Label()
        Me.cbMoviePartsIgnorePart = New System.Windows.Forms.CheckBox()
        Me.lblNameModeEg = New System.Windows.Forms.Label()
        Me.cbMoviePartsNameMode = New System.Windows.Forms.CheckBox()
        Me.tpMoviePreferences_Advanced = New System.Windows.Forms.TabPage()
        Me.gpbxMovieExcludeYearSearch = New System.Windows.Forms.GroupBox()
        Me.cbMovieExcludeYearSearch = New System.Windows.Forms.CheckBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.gpbx_MovTimerAutoScrape = New System.Windows.Forms.GroupBox()
        Me.tbMovAutoScrapeInterval = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.cbMovEnableAutoScrape = New System.Windows.Forms.CheckBox()
        Me.grpbxMovieCustomPlaycountTag = New System.Windows.Forms.GroupBox()
        Me.cbMovNfoWatchTag = New System.Windows.Forms.CheckBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.grpbxMovieSeparateIdentifier = New System.Windows.Forms.GroupBox()
        Me.btn_MovSepReset = New System.Windows.Forms.Button()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.btn_MovSepRem = New System.Windows.Forms.Button()
        Me.btn_MovSepAdd = New System.Windows.Forms.Button()
        Me.tb_MovSeptb = New System.Windows.Forms.TextBox()
        Me.lb_MovSepLst = New System.Windows.Forms.ListBox()
        Me.grpbxMovieNfoPosterOptions = New System.Windows.Forms.GroupBox()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.imdb_chk = New System.Windows.Forms.CheckBox()
        Me.mpdb_chk = New System.Windows.Forms.CheckBox()
        Me.tmdb_chk = New System.Windows.Forms.CheckBox()
        Me.IMPA_chk = New System.Windows.Forms.CheckBox()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.TPTVPref = New System.Windows.Forms.TabPage()
        Me.TabControl6 = New System.Windows.Forms.TabControl()
        Me.TabPage30 = New System.Windows.Forms.TabPage()
        Me.GroupBox17 = New System.Windows.Forms.GroupBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.tbTvAutoScrapeInterval = New System.Windows.Forms.TextBox()
        Me.cbTvEnableAutoScrape = New System.Windows.Forms.CheckBox()
        Me.cbTvThousSeparator = New System.Windows.Forms.CheckBox()
        Me.cbTvScrShtTVDBResize = New System.Windows.Forms.CheckBox()
        Me.GroupBox43 = New System.Windows.Forms.GroupBox()
        Me.cb_TvMissingEpOffset = New System.Windows.Forms.CheckBox()
        Me.cbTvMissingSpecials = New System.Windows.Forms.CheckBox()
        Me.Label111 = New System.Windows.Forms.Label()
        Me.cbTv_fixNFOid = New System.Windows.Forms.CheckBox()
        Me.GroupBox22 = New System.Windows.Forms.GroupBox()
        Me.cbtvdbIMDbRating = New System.Windows.Forms.CheckBox()
        Me.btnTVDBGetLanguages = New System.Windows.Forms.Button()
        Me.rbTvEpSortDefault = New System.Windows.Forms.RadioButton()
        Me.rbTvEpSortDVD = New System.Windows.Forms.RadioButton()
        Me.Label124 = New System.Windows.Forms.Label()
        Me.Label123 = New System.Windows.Forms.Label()
        Me.Label138 = New System.Windows.Forms.Label()
        Me.cbTvSeriesActorstoEpisodeActors = New System.Windows.Forms.CheckBox()
        Me.cbTvAutoScreenShot = New System.Windows.Forms.CheckBox()
        Me.cbTvUse_XBMC_TVDB_Scraper = New System.Windows.Forms.CheckBox()
        Me.Label139 = New System.Windows.Forms.Label()
        Me.GroupBox20 = New System.Windows.Forms.GroupBox()
        Me.cbTvDlEpisodeThumb = New System.Windows.Forms.CheckBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbxTvXtraFanartQty = New System.Windows.Forms.ComboBox()
        Me.cbTvFanartTvFirst = New System.Windows.Forms.CheckBox()
        Me.cbTvDlFanartTvArt = New System.Windows.Forms.CheckBox()
        Me.cbSeasonFolderjpg = New System.Windows.Forms.CheckBox()
        Me.cb_TvFolderJpg = New System.Windows.Forms.CheckBox()
        Me.cbTvDlXtraFanart = New System.Windows.Forms.CheckBox()
        Me.GroupBox18 = New System.Windows.Forms.GroupBox()
        Me.posterbtn = New System.Windows.Forms.RadioButton()
        Me.bannerbtn = New System.Windows.Forms.RadioButton()
        Me.cbTvDlPosterArt = New System.Windows.Forms.CheckBox()
        Me.cbTvDlSeasonArt = New System.Windows.Forms.CheckBox()
        Me.cbTvDlFanart = New System.Windows.Forms.CheckBox()
        Me.GroupBox19 = New System.Windows.Forms.GroupBox()
        Me.RadioButton39 = New System.Windows.Forms.RadioButton()
        Me.RadioButton40 = New System.Windows.Forms.RadioButton()
        Me.RadioButton41 = New System.Windows.Forms.RadioButton()
        Me.cbTvQuickAddShow = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rb_TvRenameReplaceSpaceUnderScore = New System.Windows.Forms.RadioButton()
        Me.rb_TvRenameReplaceSpaceDot = New System.Windows.Forms.RadioButton()
        Me.cb_TvRenameReplaceSpace = New System.Windows.Forms.CheckBox()
        Me.CheckBox_tv_EpisodeRenameCase = New System.Windows.Forms.CheckBox()
        Me.CheckBox_tv_EpisodeRenameAuto = New System.Windows.Forms.CheckBox()
        Me.Label140 = New System.Windows.Forms.Label()
        Me.ComboBox_tv_EpisodeRename = New System.Windows.Forms.ComboBox()
        Me.cbTvEpEnableHDTags = New System.Windows.Forms.CheckBox()
        Me.cbTvDisableLogs = New System.Windows.Forms.CheckBox()
        Me.lbxTVDBLangs = New System.Windows.Forms.ListBox()
        Me.Label122 = New System.Windows.Forms.Label()
        Me.ComboBox8 = New System.Windows.Forms.ComboBox()
        Me.GroupBox_TVDB_Scraper_Preferences = New System.Windows.Forms.GroupBox()
        Me.cbXBMCTvdbRatingFallback = New System.Windows.Forms.CheckBox()
        Me.cbXBMCTvdbRatingImdb = New System.Windows.Forms.CheckBox()
        Me.ComboBox_TVDB_Language = New System.Windows.Forms.ComboBox()
        Me.Label154 = New System.Windows.Forms.Label()
        Me.cbXBMCTvdbPosters = New System.Windows.Forms.CheckBox()
        Me.cbXBMCTvdbFanart = New System.Windows.Forms.CheckBox()
        Me.rbXBMCTvdbAbsoluteNumber = New System.Windows.Forms.RadioButton()
        Me.rbXBMCTvdbDVDOrder = New System.Windows.Forms.RadioButton()
        Me.TabPage31 = New System.Windows.Forms.TabPage()
        Me.GroupBox_tv_RegexRename = New System.Windows.Forms.GroupBox()
        Me.btn_tv_RegexRename_Restore = New System.Windows.Forms.Button()
        Me.lb_tv_RegexRename = New System.Windows.Forms.ListBox()
        Me.btn_tv_RegexRename_Remove = New System.Windows.Forms.Button()
        Me.Label158 = New System.Windows.Forms.Label()
        Me.Label159 = New System.Windows.Forms.Label()
        Me.tb_tv_RegexRename_New = New System.Windows.Forms.TextBox()
        Me.tb_tv_RegexRename_Edit = New System.Windows.Forms.TextBox()
        Me.btn_tv_RegexRename_Add = New System.Windows.Forms.Button()
        Me.btn_tv_RegexRename_Edit = New System.Windows.Forms.Button()
        Me.GroupBox_tv_RegexScrape = New System.Windows.Forms.GroupBox()
        Me.lb_tv_RegexScrape = New System.Windows.Forms.ListBox()
        Me.btn_tv_RegexScrape_Remove = New System.Windows.Forms.Button()
        Me.Label119 = New System.Windows.Forms.Label()
        Me.Label117 = New System.Windows.Forms.Label()
        Me.tb_tv_RegexScrape_New = New System.Windows.Forms.TextBox()
        Me.tb_tv_RegexScrape_Edit = New System.Windows.Forms.TextBox()
        Me.btn_tv_RegexScrape_Add = New System.Windows.Forms.Button()
        Me.btn_tv_RegexScrape_Edit = New System.Windows.Forms.Button()
        Me.btn_tv_RegexScrape_Restore = New System.Windows.Forms.Button()
        Me.GroupBox_tv_RegexScrape_Test = New System.Windows.Forms.GroupBox()
        Me.tb_tv_RegexScrape_TestResult = New System.Windows.Forms.TextBox()
        Me.btn_tv_RegexScrape_Test = New System.Windows.Forms.Button()
        Me.tb_tv_RegexScrape_TestString = New System.Windows.Forms.TextBox()
        Me.Label118 = New System.Windows.Forms.Label()
        Me.TPHmPref = New System.Windows.Forms.TabPage()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.cb_HmFanartScrnShot = New System.Windows.Forms.CheckBox()
        Me.tb_HmFanartTime = New System.Windows.Forms.TextBox()
        Me.tb_HmPosterTime = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lbl_HmHeader = New System.Windows.Forms.Label()
        Me.TPProxy = New System.Windows.Forms.TabPage()
        Me.UcGenPref_Proxy1 = New Media_Companion.ucGenPref_Proxy()
        Me.TPXBMCLink = New System.Windows.Forms.TabPage()
        Me.UcGenPref_XbmcLink1 = New Media_Companion.ucGenPref_XbmcLink()
        Me.TPPRofCmd = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox42 = New System.Windows.Forms.GroupBox()
        Me.Label141 = New System.Windows.Forms.Label()
        Me.GroupBox15 = New System.Windows.Forms.GroupBox()
        Me.btn_ProfileSetStartup = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lb_ProfileList = New System.Windows.Forms.ListBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.btn_ProfileSetDefault = New System.Windows.Forms.Button()
        Me.tb_ProfileNew = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.btn_ProfileAdd = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btn_ProfileRemove = New System.Windows.Forms.Button()
        Me.lbl_CommandTitle = New System.Windows.Forms.Label()
        Me.lcl_CommandCommand = New System.Windows.Forms.Label()
        Me.tb_CommandTitle = New System.Windows.Forms.TextBox()
        Me.tb_CommandCommand = New System.Windows.Forms.TextBox()
        Me.btn_CommandRemove = New System.Windows.Forms.Button()
        Me.lb_CommandTitle = New System.Windows.Forms.ListBox()
        Me.lb_CommandCommand = New System.Windows.Forms.ListBox()
        Me.btn_CommandAdd = New System.Windows.Forms.Button()
        Me.btn_SettingsApplyOnly = New System.Windows.Forms.Button()
        Me.btn_SettingsClose = New System.Windows.Forms.Button()
        Me.btn_SettingsApplyClose = New System.Windows.Forms.Button()
        Me.btn_SettingsClose2 = New System.Windows.Forms.Button()
        Me.cbTvEpSaveNfoEmpty = New System.Windows.Forms.CheckBox()
        Me.GroupBox12.SuspendLayout
        Me.gbExcludeFolders.SuspendLayout
        Me.grpbxImdbCertPriority.SuspendLayout
        Me.GroupBox36.SuspendLayout
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.TabControl1.SuspendLayout
        Me.TPGenCom.SuspendLayout
        Me.TabControl4.SuspendLayout
        Me.TPGeneral.SuspendLayout
        Me.GroupBox45.SuspendLayout
        Me.GroupBox33.SuspendLayout
        Me.GroupBox31.SuspendLayout
        Me.GroupBox3.SuspendLayout
        Me.TPCommonSettings.SuspendLayout
        Me.gpbxPrefIgnore.SuspendLayout
        Me.gpbxPrefOverlay.SuspendLayout
        Me.GroupBox4.SuspendLayout
        Me.gbImageResizing.SuspendLayout
        Me.grpCleanFilename.SuspendLayout
        Me.grpVideoSource.SuspendLayout
        Me.gbxXBMCversion.SuspendLayout
        Me.TPActors.SuspendLayout
        Me.GroupBox2.SuspendLayout
        Me.GroupBox32.SuspendLayout
        Me.TPMovPref.SuspendLayout
        Me.tcMoviePreferences.SuspendLayout
        Me.tpMoviePreferences_Scraper.SuspendLayout
        Me.grpbxTMDbApi.SuspendLayout
        Me.grpbxMovieScraperSettings.SuspendLayout
        Me.grpbxTMDBScraperPreferences.SuspendLayout
        Me.grpbxTmdbfromImdbOptions.SuspendLayout
        Me.GpBx_McIMDbScraperSettings.SuspendLayout
        Me.gpbxPrefScraperImages.SuspendLayout
        Me.grpbxMovieTagOptions.SuspendLayout
        Me.grpbxMovieBasicSave.SuspendLayout
        Me.grpbxScraperLimits.SuspendLayout
        Me.grpbxScraperMisc.SuspendLayout
        Me.grpbxMovPreferLang.SuspendLayout
        Me.grpbxCustomLanguage.SuspendLayout
        Me.grpbxMovieFolderOptions.SuspendLayout
        Me.tpMoviePreferences_Artwork.SuspendLayout
        Me.grpbxMovieSetArtwork.SuspendLayout
        Me.GrpbxXtraArtwork.SuspendLayout
        Me.grpbxMoviesInFoldersArtwork.SuspendLayout
        Me.grpbxMoviePosterPriority.SuspendLayout
        Me.grpbxAutoScrapeArtwork.SuspendLayout
        Me.grpbxExtraArtwork.SuspendLayout
        Me.tpMoviePreferences_General.SuspendLayout
        Me.grpbxMovieFilters.SuspendLayout
        CType(Me.nudMaxTagsInFilter,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudMinTagsInFilter,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudMaxDirectorsInFilter,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudMaxSetsInFilter,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudDirectorsFilterMinFilms,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudSetsFilterMinFilms,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudMaxActorsInFilter,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.nudActorsFilterMinFilms,System.ComponentModel.ISupportInitialize).BeginInit
        Me.grpbxMovieListOptions.SuspendLayout
        Me.grpbxMovieRenameOptions.SuspendLayout
        Me.grpbxMovieOfflineTitleText.SuspendLayout
        Me.grpbxMovieGeneralOptions.SuspendLayout
        Me.PanelDisplayRuntime.SuspendLayout
        Me.grpbxMovieNameMode.SuspendLayout
        Me.tpMoviePreferences_Advanced.SuspendLayout
        Me.gpbxMovieExcludeYearSearch.SuspendLayout
        Me.gpbx_MovTimerAutoScrape.SuspendLayout
        Me.grpbxMovieCustomPlaycountTag.SuspendLayout
        Me.grpbxMovieSeparateIdentifier.SuspendLayout
        Me.grpbxMovieNfoPosterOptions.SuspendLayout
        Me.TPTVPref.SuspendLayout
        Me.TabControl6.SuspendLayout
        Me.TabPage30.SuspendLayout
        Me.GroupBox17.SuspendLayout
        Me.GroupBox43.SuspendLayout
        Me.GroupBox22.SuspendLayout
        Me.GroupBox20.SuspendLayout
        Me.GroupBox18.SuspendLayout
        Me.GroupBox19.SuspendLayout
        Me.GroupBox1.SuspendLayout
        Me.GroupBox_TVDB_Scraper_Preferences.SuspendLayout
        Me.TabPage31.SuspendLayout
        Me.GroupBox_tv_RegexRename.SuspendLayout
        Me.GroupBox_tv_RegexScrape.SuspendLayout
        Me.GroupBox_tv_RegexScrape_Test.SuspendLayout
        Me.TPHmPref.SuspendLayout
        Me.GroupBox5.SuspendLayout
        Me.TPProxy.SuspendLayout
        Me.TPXBMCLink.SuspendLayout
        Me.TPPRofCmd.SuspendLayout
        Me.TableLayoutPanel1.SuspendLayout
        Me.GroupBox42.SuspendLayout
        Me.GroupBox15.SuspendLayout
        Me.SuspendLayout
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.cbLocalActorSaveNoId)
        Me.GroupBox12.Controls.Add(Me.cbLocalActorSaveAlpha)
        Me.GroupBox12.Controls.Add(Me.xbmcactorpath)
        Me.GroupBox12.Controls.Add(Me.btn_localactorpathbrowse)
        Me.GroupBox12.Controls.Add(Me.Label161)
        Me.GroupBox12.Controls.Add(Me.Label104)
        Me.GroupBox12.Controls.Add(Me.Label103)
        Me.GroupBox12.Controls.Add(Me.Label101)
        Me.GroupBox12.Controls.Add(Me.Label96)
        Me.GroupBox12.Controls.Add(Me.Label97)
        Me.GroupBox12.Controls.Add(Me.localactorpath)
        Me.GroupBox12.Controls.Add(Me.cbsaveactor)
        Me.GroupBox12.Location = New System.Drawing.Point(425, 33)
        Me.GroupBox12.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox12.Size = New System.Drawing.Size(523, 497)
        Me.GroupBox12.TabIndex = 49
        Me.GroupBox12.TabStop = false
        Me.GroupBox12.Text = "Download Actor Thumbs"
        Me.ToolTip1.SetToolTip(Me.GroupBox12, "Downloads actor thumbnails to Local or Network Location")
        '
        'cbLocalActorSaveNoId
        '
        Me.cbLocalActorSaveNoId.AutoSize = true
        Me.cbLocalActorSaveNoId.Location = New System.Drawing.Point(22, 149)
        Me.cbLocalActorSaveNoId.Name = "cbLocalActorSaveNoId"
        Me.cbLocalActorSaveNoId.Size = New System.Drawing.Size(216, 19)
        Me.cbLocalActorSaveNoId.TabIndex = 12
        Me.cbLocalActorSaveNoId.Text = "Exclude actor ID as filename suffix."
        Me.cbLocalActorSaveNoId.UseVisualStyleBackColor = true
        '
        'cbLocalActorSaveAlpha
        '
        Me.cbLocalActorSaveAlpha.AutoSize = true
        Me.cbLocalActorSaveAlpha.Location = New System.Drawing.Point(8, 128)
        Me.cbLocalActorSaveAlpha.Name = "cbLocalActorSaveAlpha"
        Me.cbLocalActorSaveAlpha.Size = New System.Drawing.Size(196, 19)
        Me.cbLocalActorSaveAlpha.TabIndex = 11
        Me.cbLocalActorSaveAlpha.Text = "Save actor as Actor_Name.extn"
        Me.cbLocalActorSaveAlpha.UseVisualStyleBackColor = true
        '
        'xbmcactorpath
        '
        Me.xbmcactorpath.Location = New System.Drawing.Point(87, 468)
        Me.xbmcactorpath.Margin = New System.Windows.Forms.Padding(4)
        Me.xbmcactorpath.Name = "xbmcactorpath"
        Me.xbmcactorpath.Size = New System.Drawing.Size(324, 21)
        Me.xbmcactorpath.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.xbmcactorpath, "Enter the path for the actors folder from XBMC."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"This may be a network path.")
        '
        'btn_localactorpathbrowse
        '
        Me.btn_localactorpathbrowse.Location = New System.Drawing.Point(419, 326)
        Me.btn_localactorpathbrowse.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_localactorpathbrowse.Name = "btn_localactorpathbrowse"
        Me.btn_localactorpathbrowse.Size = New System.Drawing.Size(36, 23)
        Me.btn_localactorpathbrowse.TabIndex = 4
        Me.btn_localactorpathbrowse.Text = "..."
        Me.btn_localactorpathbrowse.UseVisualStyleBackColor = true
        '
        'Label161
        '
        Me.Label161.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label161.Location = New System.Drawing.Point(8, 291)
        Me.Label161.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label161.Name = "Label161"
        Me.Label161.Size = New System.Drawing.Size(461, 31)
        Me.Label161.TabIndex = 10
        Me.Label161.Text = "The ""Local Path"" below needs to be the path to where you want the actor thumbs sa"& _ 
    "ved, eg ""C:\MovieStuff\ActorThumbs"""
        '
        'Label104
        '
        Me.Label104.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label104.Location = New System.Drawing.Point(8, 18)
        Me.Label104.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(507, 80)
        Me.Label104.TabIndex = 8
        Me.Label104.Text = resources.GetString("Label104.Text")
        '
        'Label103
        '
        Me.Label103.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label103.Location = New System.Drawing.Point(8, 368)
        Me.Label103.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(507, 96)
        Me.Label103.TabIndex = 7
        Me.Label103.Text = resources.GetString("Label103.Text")
        '
        'Label101
        '
        Me.Label101.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label101.Location = New System.Drawing.Point(8, 176)
        Me.Label101.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label101.Name = "Label101"
        Me.Label101.Size = New System.Drawing.Size(507, 114)
        Me.Label101.TabIndex = 6
        Me.Label101.Text = resources.GetString("Label101.Text")
        '
        'Label96
        '
        Me.Label96.AutoSize = true
        Me.Label96.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label96.Location = New System.Drawing.Point(8, 471)
        Me.Label96.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(80, 15)
        Me.Label96.TabIndex = 5
        Me.Label96.Text = "XBMC Path :-"
        '
        'Label97
        '
        Me.Label97.AutoSize = true
        Me.Label97.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label97.Location = New System.Drawing.Point(8, 330)
        Me.Label97.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(75, 15)
        Me.Label97.TabIndex = 2
        Me.Label97.Text = "Local Path :-"
        '
        'localactorpath
        '
        Me.localactorpath.Location = New System.Drawing.Point(87, 328)
        Me.localactorpath.Margin = New System.Windows.Forms.Padding(4)
        Me.localactorpath.Name = "localactorpath"
        Me.localactorpath.Size = New System.Drawing.Size(324, 21)
        Me.localactorpath.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.localactorpath, "The path for Media Companion to save the file")
        '
        'cbsaveactor
        '
        Me.cbsaveactor.AutoSize = true
        Me.cbsaveactor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbsaveactor.Location = New System.Drawing.Point(8, 102)
        Me.cbsaveactor.Margin = New System.Windows.Forms.Padding(4)
        Me.cbsaveactor.Name = "cbsaveactor"
        Me.cbsaveactor.Size = New System.Drawing.Size(173, 19)
        Me.cbsaveactor.TabIndex = 0
        Me.cbsaveactor.Text = "Enable Save Actor Thumbs"
        Me.cbsaveactor.UseVisualStyleBackColor = true
        '
        'cmbx_MovMaxActors
        '
        Me.cmbx_MovMaxActors.FormattingEnabled = true
        Me.cmbx_MovMaxActors.Items.AddRange(New Object() {"All Available", "None", "5", "10", "15", "20", "25", "30", "40", "50", "70", "90", "110", "125", "150", "175", "200", "250"})
        Me.cmbx_MovMaxActors.Location = New System.Drawing.Point(175, 51)
        Me.cmbx_MovMaxActors.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbx_MovMaxActors.MaxDropDownItems = 30
        Me.cmbx_MovMaxActors.Name = "cmbx_MovMaxActors"
        Me.cmbx_MovMaxActors.Size = New System.Drawing.Size(137, 23)
        Me.cmbx_MovMaxActors.TabIndex = 64
        Me.ToolTip1.SetToolTip(Me.cmbx_MovMaxActors, "Media Companion will not scrape more than the number of actors set using this con"& _ 
        "trol")
        '
        'cbShowAllAudioTracks
        '
        Me.cbShowAllAudioTracks.AutoSize = true
        Me.cbShowAllAudioTracks.Location = New System.Drawing.Point(6, 95)
        Me.cbShowAllAudioTracks.Name = "cbShowAllAudioTracks"
        Me.cbShowAllAudioTracks.Size = New System.Drawing.Size(239, 19)
        Me.cbShowAllAudioTracks.TabIndex = 98
        Me.cbShowAllAudioTracks.Text = "Show all audio tracks in Media Overlay."
        Me.ToolTip1.SetToolTip(Me.cbShowAllAudioTracks, "Unchecked - Shows just the default audio track on the fanart image"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Checked - Sho"& _ 
        "ws all the audio tracks with the non-default ones greyed out.")
        Me.cbShowAllAudioTracks.UseVisualStyleBackColor = true
        '
        'cbDisplayMediaInfoOverlay
        '
        Me.cbDisplayMediaInfoOverlay.AutoSize = true
        Me.cbDisplayMediaInfoOverlay.Location = New System.Drawing.Point(6, 45)
        Me.cbDisplayMediaInfoOverlay.Name = "cbDisplayMediaInfoOverlay"
        Me.cbDisplayMediaInfoOverlay.Size = New System.Drawing.Size(229, 19)
        Me.cbDisplayMediaInfoOverlay.TabIndex = 96
        Me.cbDisplayMediaInfoOverlay.Text = "Display Media Info over Fanart Image"
        Me.ToolTip1.SetToolTip(Me.cbDisplayMediaInfoOverlay, "Shows Movie or Episode Media details"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Overlayed over Movie Fanart or Episode Thum"& _ 
        "b.")
        Me.cbDisplayMediaInfoOverlay.UseVisualStyleBackColor = true
        '
        'cbDisplayRatingOverlay
        '
        Me.cbDisplayRatingOverlay.AutoSize = true
        Me.cbDisplayRatingOverlay.Location = New System.Drawing.Point(6, 20)
        Me.cbDisplayRatingOverlay.Name = "cbDisplayRatingOverlay"
        Me.cbDisplayRatingOverlay.Size = New System.Drawing.Size(207, 19)
        Me.cbDisplayRatingOverlay.TabIndex = 95
        Me.cbDisplayRatingOverlay.Text = "Display Rating over Fanart Image"
        Me.ToolTip1.SetToolTip(Me.cbDisplayRatingOverlay, "Shows Movie or Episode Rating, Overlayed"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"over Movie Fanart or Episode Thumbnail "& _ 
        "Image.")
        Me.cbDisplayRatingOverlay.UseVisualStyleBackColor = true
        '
        'gbExcludeFolders
        '
        Me.gbExcludeFolders.Controls.Add(Me.tbExcludeFolders)
        Me.gbExcludeFolders.Location = New System.Drawing.Point(296, 353)
        Me.gbExcludeFolders.Name = "gbExcludeFolders"
        Me.gbExcludeFolders.Size = New System.Drawing.Size(277, 95)
        Me.gbExcludeFolders.TabIndex = 94
        Me.gbExcludeFolders.TabStop = false
        Me.gbExcludeFolders.Text = "Exclude Folders from scrape"
        Me.ToolTip1.SetToolTip(Me.gbExcludeFolders, "Trailing ""*"" pattern matching supported. E.g. ""._*"" excludes all folders beginnin"& _ 
        "g ""._""")
        '
        'tbExcludeFolders
        '
        Me.tbExcludeFolders.Location = New System.Drawing.Point(6, 22)
        Me.tbExcludeFolders.Multiline = true
        Me.tbExcludeFolders.Name = "tbExcludeFolders"
        Me.tbExcludeFolders.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbExcludeFolders.Size = New System.Drawing.Size(265, 62)
        Me.tbExcludeFolders.TabIndex = 0
        '
        'cb_keywordlimit
        '
        Me.cb_keywordlimit.FormattingEnabled = true
        Me.cb_keywordlimit.Items.AddRange(New Object() {"All Available", "None", "5", "10", "15", "20", "25", "30", "40", "50", "70", "90", "100", "125", "150", "175", "200", "250", "300", "400", "500"})
        Me.cb_keywordlimit.Location = New System.Drawing.Point(213, 47)
        Me.cb_keywordlimit.Margin = New System.Windows.Forms.Padding(4)
        Me.cb_keywordlimit.MaxDropDownItems = 30
        Me.cb_keywordlimit.Name = "cb_keywordlimit"
        Me.cb_keywordlimit.Size = New System.Drawing.Size(76, 23)
        Me.cb_keywordlimit.TabIndex = 65
        Me.ToolTip1.SetToolTip(Me.cb_keywordlimit, "Media Companion will not scrape more than the number of Keywords set using this c"& _ 
        "ontrol")
        '
        'cbMovieBasicSave
        '
        Me.cbMovieBasicSave.AutoSize = true
        Me.cbMovieBasicSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieBasicSave.Location = New System.Drawing.Point(7, 80)
        Me.cbMovieBasicSave.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieBasicSave.Name = "cbMovieBasicSave"
        Me.cbMovieBasicSave.Size = New System.Drawing.Size(304, 34)
        Me.cbMovieBasicSave.TabIndex = 46
        Me.cbMovieBasicSave.Text = "Save files as ""movie.nfo"", ""fanart.jpg"", &&"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"""movie.tbn"" if Eden, or ""poster.jpg"""& _ 
    " if Frodo onwards"
        Me.ToolTip1.SetToolTip(Me.cbMovieBasicSave, resources.GetString("cbMovieBasicSave.ToolTip"))
        Me.cbMovieBasicSave.UseVisualStyleBackColor = true
        '
        'cbGetMovieSetFromTMDb
        '
        Me.cbGetMovieSetFromTMDb.AutoSize = true
        Me.cbGetMovieSetFromTMDb.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbGetMovieSetFromTMDb.Location = New System.Drawing.Point(12, 18)
        Me.cbGetMovieSetFromTMDb.Name = "cbGetMovieSetFromTMDb"
        Me.cbGetMovieSetFromTMDb.Size = New System.Drawing.Size(223, 17)
        Me.cbGetMovieSetFromTMDb.TabIndex = 50
        Me.cbGetMovieSetFromTMDb.Text = "TMDb - Where available scrape set name"
        Me.ToolTip1.SetToolTip(Me.cbGetMovieSetFromTMDb, "Get Set names from TheMovieDb during scraping")
        Me.cbGetMovieSetFromTMDb.UseVisualStyleBackColor = true
        '
        'cmbxMovieScraper_MaxStudios
        '
        Me.cmbxMovieScraper_MaxStudios.FormattingEnabled = true
        Me.cmbxMovieScraper_MaxStudios.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cmbxMovieScraper_MaxStudios.Location = New System.Drawing.Point(173, 46)
        Me.cmbxMovieScraper_MaxStudios.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbxMovieScraper_MaxStudios.MaxDropDownItems = 30
        Me.cmbxMovieScraper_MaxStudios.Name = "cmbxMovieScraper_MaxStudios"
        Me.cmbxMovieScraper_MaxStudios.Size = New System.Drawing.Size(140, 23)
        Me.cmbxMovieScraper_MaxStudios.TabIndex = 64
        Me.ToolTip1.SetToolTip(Me.cmbxMovieScraper_MaxStudios, "Media Companion will not scrape more than"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"tne number of genres set with this con"& _ 
        "trol")
        '
        'cmbxMovScraper_MaxGenres
        '
        Me.cmbxMovScraper_MaxGenres.FormattingEnabled = true
        Me.cmbxMovScraper_MaxGenres.Items.AddRange(New Object() {"All Available", "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cmbxMovScraper_MaxGenres.Location = New System.Drawing.Point(173, 17)
        Me.cmbxMovScraper_MaxGenres.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbxMovScraper_MaxGenres.MaxDropDownItems = 30
        Me.cmbxMovScraper_MaxGenres.Name = "cmbxMovScraper_MaxGenres"
        Me.cmbxMovScraper_MaxGenres.Size = New System.Drawing.Size(140, 23)
        Me.cmbxMovScraper_MaxGenres.TabIndex = 44
        Me.ToolTip1.SetToolTip(Me.cmbxMovScraper_MaxGenres, "Media Companion will not scrape more than"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"tne number of genres set with this con"& _ 
        "trol")
        '
        'cbMovCreateFanartjpg
        '
        Me.cbMovCreateFanartjpg.AutoSize = true
        Me.cbMovCreateFanartjpg.Location = New System.Drawing.Point(6, 94)
        Me.cbMovCreateFanartjpg.Name = "cbMovCreateFanartjpg"
        Me.cbMovCreateFanartjpg.Size = New System.Drawing.Size(197, 19)
        Me.cbMovCreateFanartjpg.TabIndex = 52
        Me.cbMovCreateFanartjpg.Text = "Create fanart.jpg for each folder"
        Me.ToolTip1.SetToolTip(Me.cbMovCreateFanartjpg, "If 'Save as 'fanart.jpg', not 'moviename-fanart.jpg' ' is selected,"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"then this op"& _ 
        "tion is ignored.")
        Me.cbMovCreateFanartjpg.UseVisualStyleBackColor = true
        '
        'cbMoviePosterInFolder
        '
        Me.cbMoviePosterInFolder.AutoSize = true
        Me.cbMoviePosterInFolder.Location = New System.Drawing.Point(6, 43)
        Me.cbMoviePosterInFolder.Name = "cbMoviePosterInFolder"
        Me.cbMoviePosterInFolder.Size = New System.Drawing.Size(281, 19)
        Me.cbMoviePosterInFolder.TabIndex = 44
        Me.cbMoviePosterInFolder.Text = "Save as poster.jpg', not 'moviename-poster.jpg"
        Me.ToolTip1.SetToolTip(Me.cbMoviePosterInFolder, resources.GetString("cbMoviePosterInFolder.ToolTip"))
        Me.cbMoviePosterInFolder.UseVisualStyleBackColor = true
        '
        'cbMovieFanartInFolders
        '
        Me.cbMovieFanartInFolders.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbMovieFanartInFolders.Location = New System.Drawing.Point(6, 17)
        Me.cbMovieFanartInFolders.Name = "cbMovieFanartInFolders"
        Me.cbMovieFanartInFolders.Size = New System.Drawing.Size(281, 26)
        Me.cbMovieFanartInFolders.TabIndex = 39
        Me.cbMovieFanartInFolders.Text = "Save as 'fanart.jpg', not 'moviename-fanart.jpg'"
        Me.cbMovieFanartInFolders.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.ToolTip1.SetToolTip(Me.cbMovieFanartInFolders, resources.GetString("cbMovieFanartInFolders.ToolTip"))
        Me.cbMovieFanartInFolders.UseVisualStyleBackColor = true
        '
        'cbMovXtraFanart
        '
        Me.cbMovXtraFanart.AutoSize = true
        Me.cbMovXtraFanart.Location = New System.Drawing.Point(11, 48)
        Me.cbMovXtraFanart.Name = "cbMovXtraFanart"
        Me.cbMovXtraFanart.Size = New System.Drawing.Size(149, 19)
        Me.cbMovXtraFanart.TabIndex = 1
        Me.cbMovXtraFanart.Text = "Allow save ExtraFanart"
        Me.ToolTip1.SetToolTip(Me.cbMovXtraFanart, "Either or both options can be selected")
        Me.cbMovXtraFanart.UseVisualStyleBackColor = true
        '
        'cbMovXtraThumbs
        '
        Me.cbMovXtraThumbs.AutoSize = true
        Me.cbMovXtraThumbs.Location = New System.Drawing.Point(11, 23)
        Me.cbMovXtraThumbs.Name = "cbMovXtraThumbs"
        Me.cbMovXtraThumbs.Size = New System.Drawing.Size(291, 19)
        Me.cbMovXtraThumbs.TabIndex = 0
        Me.cbMovXtraThumbs.Text = "Allow save  ExtraThumbs......Limited to 5 images."
        Me.ToolTip1.SetToolTip(Me.cbMovXtraThumbs, "Either or both options can be selected.")
        Me.cbMovXtraThumbs.UseVisualStyleBackColor = true
        '
        'cbMovFanartTvScrape
        '
        Me.cbMovFanartTvScrape.AutoSize = true
        Me.cbMovFanartTvScrape.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovFanartTvScrape.Location = New System.Drawing.Point(5, 56)
        Me.cbMovFanartTvScrape.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovFanartTvScrape.Name = "cbMovFanartTvScrape"
        Me.cbMovFanartTvScrape.Size = New System.Drawing.Size(284, 19)
        Me.cbMovFanartTvScrape.TabIndex = 45
        Me.cbMovFanartTvScrape.Text = "Download from Fanart.TV for during autoscrape"
        Me.ToolTip1.SetToolTip(Me.cbMovFanartTvScrape, "Allow to Auto-download From Fanart.Tv, during scraping of movies."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Does not inclu"& _ 
        "de ExtraThumbs or ExtraFanart.")
        Me.cbMovFanartTvScrape.UseVisualStyleBackColor = true
        '
        'cbDlXtraFanart
        '
        Me.cbDlXtraFanart.AutoSize = true
        Me.cbDlXtraFanart.Location = New System.Drawing.Point(6, 15)
        Me.cbDlXtraFanart.Name = "cbDlXtraFanart"
        Me.cbDlXtraFanart.Size = New System.Drawing.Size(199, 19)
        Me.cbDlXtraFanart.TabIndex = 43
        Me.cbDlXtraFanart.Text = "Download Extra Fanart/Thumbs"
        Me.ToolTip1.SetToolTip(Me.cbDlXtraFanart, "This allows download of Extra Thumbs or"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Extra Fanart or both, as selected below."& _ 
        ""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"NB:  Movies must be in separate folders!!!")
        Me.cbDlXtraFanart.UseVisualStyleBackColor = true
        '
        'cbMovFanartScrape
        '
        Me.cbMovFanartScrape.AutoSize = true
        Me.cbMovFanartScrape.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovFanartScrape.Location = New System.Drawing.Point(5, 36)
        Me.cbMovFanartScrape.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovFanartScrape.Name = "cbMovFanartScrape"
        Me.cbMovFanartScrape.Size = New System.Drawing.Size(281, 19)
        Me.cbMovFanartScrape.TabIndex = 38
        Me.cbMovFanartScrape.Text = "Download Fanart for Movies during autoscrape"
        Me.ToolTip1.SetToolTip(Me.cbMovFanartScrape, "Allow to Auto-download Fanart during scraping of movies."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Does not include ExtraT"& _ 
        "humbs or ExtraFanart.")
        Me.cbMovFanartScrape.UseVisualStyleBackColor = true
        '
        'cbMoviePosterScrape
        '
        Me.cbMoviePosterScrape.AutoSize = true
        Me.cbMoviePosterScrape.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMoviePosterScrape.Location = New System.Drawing.Point(5, 15)
        Me.cbMoviePosterScrape.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMoviePosterScrape.Name = "cbMoviePosterScrape"
        Me.cbMoviePosterScrape.Size = New System.Drawing.Size(281, 19)
        Me.cbMoviePosterScrape.TabIndex = 42
        Me.cbMoviePosterScrape.Text = "Download Poster for Movies during autoscrape"
        Me.ToolTip1.SetToolTip(Me.cbMoviePosterScrape, "Allow to Auto-download Posters during scraping of movies")
        Me.cbMoviePosterScrape.UseVisualStyleBackColor = true
        '
        'tbDateFormat
        '
        Me.tbDateFormat.Location = New System.Drawing.Point(89, 13)
        Me.tbDateFormat.Name = "tbDateFormat"
        Me.tbDateFormat.Size = New System.Drawing.Size(138, 21)
        Me.tbDateFormat.TabIndex = 1
        Me.tbDateFormat.Text = "YYYY-MM-DD"
        Me.ToolTip1.SetToolTip(Me.tbDateFormat, "Valid tokens: YYYY MM DD HH MIN SS")
        '
        'Label179
        '
        Me.Label179.AutoSize = true
        Me.Label179.Location = New System.Drawing.Point(14, 16)
        Me.Label179.Name = "Label179"
        Me.Label179.Size = New System.Drawing.Size(71, 15)
        Me.Label179.TabIndex = 0
        Me.Label179.Text = "Date format"
        Me.ToolTip1.SetToolTip(Me.Label179, "Valid tokens: YYYY MM DD HH MIN SS")
        '
        'rbRenameFullStop
        '
        Me.rbRenameFullStop.AutoSize = true
        Me.rbRenameFullStop.CheckAlign = System.Drawing.ContentAlignment.TopCenter
        Me.rbRenameFullStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbRenameFullStop.Location = New System.Drawing.Point(89, 251)
        Me.rbRenameFullStop.Name = "rbRenameFullStop"
        Me.rbRenameFullStop.Size = New System.Drawing.Size(25, 32)
        Me.rbRenameFullStop.TabIndex = 85
        Me.rbRenameFullStop.TabStop = true
        Me.rbRenameFullStop.Text = """."""
        Me.rbRenameFullStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.rbRenameFullStop, "Full Stop")
        Me.rbRenameFullStop.UseVisualStyleBackColor = true
        '
        'rbRenameUnderscore
        '
        Me.rbRenameUnderscore.AutoSize = true
        Me.rbRenameUnderscore.CheckAlign = System.Drawing.ContentAlignment.TopCenter
        Me.rbRenameUnderscore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbRenameUnderscore.Location = New System.Drawing.Point(29, 251)
        Me.rbRenameUnderscore.Name = "rbRenameUnderscore"
        Me.rbRenameUnderscore.Size = New System.Drawing.Size(29, 32)
        Me.rbRenameUnderscore.TabIndex = 84
        Me.rbRenameUnderscore.TabStop = true
        Me.rbRenameUnderscore.Text = """_"""
        Me.rbRenameUnderscore.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.rbRenameUnderscore, "UnderScore")
        Me.rbRenameUnderscore.UseVisualStyleBackColor = true
        '
        'cbMovTitleCase
        '
        Me.cbMovTitleCase.AutoSize = true
        Me.cbMovTitleCase.Location = New System.Drawing.Point(7, 94)
        Me.cbMovTitleCase.Name = "cbMovTitleCase"
        Me.cbMovTitleCase.Size = New System.Drawing.Size(181, 19)
        Me.cbMovTitleCase.TabIndex = 75
        Me.cbMovTitleCase.Text = "Title Case Title and Sort Title"
        Me.ToolTip1.SetToolTip(Me.cbMovTitleCase, "Changes Movie Title and Sort Title to Title Case Format."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Example: "&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"The Lion, Th"& _ 
        "e Witch And The Wardrobe"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"instead of"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"The Lion, the Witch and the Wardrobe")
        Me.cbMovTitleCase.UseVisualStyleBackColor = true
        '
        'Label102
        '
        Me.Label102.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label102.Location = New System.Drawing.Point(41, 67)
        Me.Label102.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(221, 31)
        Me.Label102.TabIndex = 72
        Me.Label102.Text = "If selected resolution is unavailable then next resolution down is used."
        Me.ToolTip1.SetToolTip(Me.Label102, "* This option is dependent on 'Add Movie Trailer url to nfo file' being checked*")
        '
        'Label78
        '
        Me.Label78.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label78.Location = New System.Drawing.Point(23, 52)
        Me.Label78.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(170, 15)
        Me.Label78.TabIndex = 71
        Me.Label78.Text = "Preferred movie trailer resolution"
        Me.ToolTip1.SetToolTip(Me.Label78, "* This option is dependent on 'Add Movie Trailer url to nfo file' being checked*")
        '
        'cbPreferredTrailerResolution
        '
        Me.cbPreferredTrailerResolution.FormattingEnabled = true
        Me.cbPreferredTrailerResolution.Items.AddRange(New Object() {"SD", "480", "720", "1080"})
        Me.cbPreferredTrailerResolution.Location = New System.Drawing.Point(214, 32)
        Me.cbPreferredTrailerResolution.Name = "cbPreferredTrailerResolution"
        Me.cbPreferredTrailerResolution.Size = New System.Drawing.Size(74, 23)
        Me.cbPreferredTrailerResolution.TabIndex = 70
        Me.ToolTip1.SetToolTip(Me.cbPreferredTrailerResolution, "* This option is dependent on 'Add Movie Trailer url to nfo file' being checked*")
        '
        'cbDlTrailerDuringScrape
        '
        Me.cbDlTrailerDuringScrape.AutoSize = true
        Me.cbDlTrailerDuringScrape.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbDlTrailerDuringScrape.Location = New System.Drawing.Point(7, 34)
        Me.cbDlTrailerDuringScrape.Margin = New System.Windows.Forms.Padding(4)
        Me.cbDlTrailerDuringScrape.Name = "cbDlTrailerDuringScrape"
        Me.cbDlTrailerDuringScrape.Size = New System.Drawing.Size(194, 19)
        Me.cbDlTrailerDuringScrape.TabIndex = 69
        Me.cbDlTrailerDuringScrape.Text = "Download trailer during scrape"
        Me.ToolTip1.SetToolTip(Me.cbDlTrailerDuringScrape, "NB Existing trailers are not redownloaded")
        Me.cbDlTrailerDuringScrape.UseVisualStyleBackColor = true
        '
        'cbMovieRuntimeFallbackToFile
        '
        Me.cbMovieRuntimeFallbackToFile.AutoSize = true
        Me.cbMovieRuntimeFallbackToFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieRuntimeFallbackToFile.Location = New System.Drawing.Point(6, 24)
        Me.cbMovieRuntimeFallbackToFile.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieRuntimeFallbackToFile.Name = "cbMovieRuntimeFallbackToFile"
        Me.cbMovieRuntimeFallbackToFile.Size = New System.Drawing.Size(224, 19)
        Me.cbMovieRuntimeFallbackToFile.TabIndex = 70
        Me.cbMovieRuntimeFallbackToFile.Text = "If runtime not scraped, fallback to file"
        Me.ToolTip1.SetToolTip(Me.cbMovieRuntimeFallbackToFile, "Check this box to display runtime from movie file when it's not available on IMDB"& _ 
        ". Only applicable if 'Scraper' selected above")
        Me.cbMovieRuntimeFallbackToFile.UseVisualStyleBackColor = true
        '
        'grpbxImdbCertPriority
        '
        Me.grpbxImdbCertPriority.Controls.Add(Me.cbIncludeMpaaRated)
        Me.grpbxImdbCertPriority.Controls.Add(Me.cbExcludeMpaaRated)
        Me.grpbxImdbCertPriority.Controls.Add(Me.cb_MovCertRemovePhrase)
        Me.grpbxImdbCertPriority.Controls.Add(Me.ScrapeFullCertCheckBox)
        Me.grpbxImdbCertPriority.Controls.Add(Me.Label178)
        Me.grpbxImdbCertPriority.Controls.Add(Me.Label95)
        Me.grpbxImdbCertPriority.Controls.Add(Me.btnMovImdbPriorityDown)
        Me.grpbxImdbCertPriority.Controls.Add(Me.btnMovImdbPriorityUp)
        Me.grpbxImdbCertPriority.Controls.Add(Me.Label94)
        Me.grpbxImdbCertPriority.Controls.Add(Me.lb_IMDBCertPriority)
        Me.grpbxImdbCertPriority.Location = New System.Drawing.Point(343, 229)
        Me.grpbxImdbCertPriority.Margin = New System.Windows.Forms.Padding(4)
        Me.grpbxImdbCertPriority.Name = "grpbxImdbCertPriority"
        Me.grpbxImdbCertPriority.Padding = New System.Windows.Forms.Padding(4)
        Me.grpbxImdbCertPriority.Size = New System.Drawing.Size(300, 289)
        Me.grpbxImdbCertPriority.TabIndex = 85
        Me.grpbxImdbCertPriority.TabStop = false
        Me.grpbxImdbCertPriority.Text = "Select IMDB Certification Priorities"
        Me.ToolTip1.SetToolTip(Me.grpbxImdbCertPriority, "You can select a specific locations certification to replace the MPAA entry on IM"& _ 
        "DB if you wish,"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"The Rating will be scraped by the first available from the abov"& _ 
        "e list.")
        '
        'cbIncludeMpaaRated
        '
        Me.cbIncludeMpaaRated.AutoSize = true
        Me.cbIncludeMpaaRated.Location = New System.Drawing.Point(12, 263)
        Me.cbIncludeMpaaRated.Name = "cbIncludeMpaaRated"
        Me.cbIncludeMpaaRated.Size = New System.Drawing.Size(232, 19)
        Me.cbIncludeMpaaRated.TabIndex = 78
        Me.cbIncludeMpaaRated.Text = "Add ""Rated"" prefix on MPAA certificate"
        Me.cbIncludeMpaaRated.UseVisualStyleBackColor = true
        '
        'cbExcludeMpaaRated
        '
        Me.cbExcludeMpaaRated.AutoSize = true
        Me.cbExcludeMpaaRated.Location = New System.Drawing.Point(12, 243)
        Me.cbExcludeMpaaRated.Name = "cbExcludeMpaaRated"
        Me.cbExcludeMpaaRated.Size = New System.Drawing.Size(255, 19)
        Me.cbExcludeMpaaRated.TabIndex = 77
        Me.cbExcludeMpaaRated.Text = "Exclude ""Rated"" prefix on MPAA certificate"
        Me.cbExcludeMpaaRated.UseVisualStyleBackColor = true
        '
        'cb_MovCertRemovePhrase
        '
        Me.cb_MovCertRemovePhrase.AutoSize = true
        Me.cb_MovCertRemovePhrase.Location = New System.Drawing.Point(12, 224)
        Me.cb_MovCertRemovePhrase.Name = "cb_MovCertRemovePhrase"
        Me.cb_MovCertRemovePhrase.Size = New System.Drawing.Size(242, 19)
        Me.cb_MovCertRemovePhrase.TabIndex = 9
        Me.cb_MovCertRemovePhrase.Text = "Remove phrases after Rating Certificate"
        Me.ToolTip1.SetToolTip(Me.cb_MovCertRemovePhrase, "ie: remove"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"for sexual content, for violence and language...")
        Me.cb_MovCertRemovePhrase.UseVisualStyleBackColor = true
        '
        'ScrapeFullCertCheckBox
        '
        Me.ScrapeFullCertCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ScrapeFullCertCheckBox.Location = New System.Drawing.Point(12, 204)
        Me.ScrapeFullCertCheckBox.Name = "ScrapeFullCertCheckBox"
        Me.ScrapeFullCertCheckBox.Size = New System.Drawing.Size(265, 24)
        Me.ScrapeFullCertCheckBox.TabIndex = 8
        Me.ScrapeFullCertCheckBox.Text = "Scrape Full Cert i.e. UK:15 instead of just 15"
        Me.ScrapeFullCertCheckBox.UseVisualStyleBackColor = true
        '
        'Label178
        '
        Me.Label178.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label178.Location = New System.Drawing.Point(8, 47)
        Me.Label178.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label178.Name = "Label178"
        Me.Label178.Size = New System.Drawing.Size(282, 16)
        Me.Label178.TabIndex = 7
        Me.Label178.Text = "The first available certificate in the order you select below"
        '
        'Label95
        '
        Me.Label95.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label95.Location = New System.Drawing.Point(9, 16)
        Me.Label95.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(282, 31)
        Me.Label95.TabIndex = 7
        Me.Label95.Text = "Customise your preference with regards to the"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"MPAA movie rating or Cert."
        '
        'btnMovImdbPriorityDown
        '
        Me.btnMovImdbPriorityDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnMovImdbPriorityDown.Location = New System.Drawing.Point(242, 161)
        Me.btnMovImdbPriorityDown.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMovImdbPriorityDown.Name = "btnMovImdbPriorityDown"
        Me.btnMovImdbPriorityDown.Size = New System.Drawing.Size(34, 29)
        Me.btnMovImdbPriorityDown.TabIndex = 6
        Me.btnMovImdbPriorityDown.Text = "↓"
        Me.btnMovImdbPriorityDown.UseVisualStyleBackColor = true
        '
        'btnMovImdbPriorityUp
        '
        Me.btnMovImdbPriorityUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnMovImdbPriorityUp.Location = New System.Drawing.Point(242, 83)
        Me.btnMovImdbPriorityUp.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMovImdbPriorityUp.Name = "btnMovImdbPriorityUp"
        Me.btnMovImdbPriorityUp.Size = New System.Drawing.Size(34, 29)
        Me.btnMovImdbPriorityUp.TabIndex = 5
        Me.btnMovImdbPriorityUp.Text = "↑"
        Me.btnMovImdbPriorityUp.UseVisualStyleBackColor = true
        '
        'Label94
        '
        Me.Label94.AutoSize = true
        Me.Label94.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label94.Location = New System.Drawing.Point(223, 118)
        Me.Label94.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(68, 36)
        Me.Label94.TabIndex = 4
        Me.Label94.Text = "Change Priority"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"of Selected"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Certification"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label94.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lb_IMDBCertPriority
        '
        Me.lb_IMDBCertPriority.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lb_IMDBCertPriority.FormattingEnabled = true
        Me.lb_IMDBCertPriority.ItemHeight = 15
        Me.lb_IMDBCertPriority.Location = New System.Drawing.Point(12, 66)
        Me.lb_IMDBCertPriority.Margin = New System.Windows.Forms.Padding(4)
        Me.lb_IMDBCertPriority.Name = "lb_IMDBCertPriority"
        Me.lb_IMDBCertPriority.Size = New System.Drawing.Size(209, 139)
        Me.lb_IMDBCertPriority.TabIndex = 0
        '
        'cbMovRootFolderCheck
        '
        Me.cbMovRootFolderCheck.AutoSize = true
        Me.cbMovRootFolderCheck.Location = New System.Drawing.Point(8, 358)
        Me.cbMovRootFolderCheck.Name = "cbMovRootFolderCheck"
        Me.cbMovRootFolderCheck.Size = New System.Drawing.Size(158, 19)
        Me.cbMovRootFolderCheck.TabIndex = 82
        Me.cbMovRootFolderCheck.Text = "Enable root folder check"
        Me.ToolTip1.SetToolTip(Me.cbMovRootFolderCheck, "If enabled, checks if movie is in root folder, disabling option of extra artwork,"& _ 
        " specifically for ""Use FolderName for Scraping""")
        Me.cbMovRootFolderCheck.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexRename_MoveDown
        '
        Me.btn_tv_RegexRename_MoveDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexRename_MoveDown.Location = New System.Drawing.Point(401, 263)
        Me.btn_tv_RegexRename_MoveDown.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexRename_MoveDown.Name = "btn_tv_RegexRename_MoveDown"
        Me.btn_tv_RegexRename_MoveDown.Size = New System.Drawing.Size(36, 31)
        Me.btn_tv_RegexRename_MoveDown.TabIndex = 43
        Me.btn_tv_RegexRename_MoveDown.Text = "v"
        Me.ToolTip1.SetToolTip(Me.btn_tv_RegexRename_MoveDown, "Alter Selected RegEx's"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"           Priority")
        Me.btn_tv_RegexRename_MoveDown.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexRename_MoveUp
        '
        Me.btn_tv_RegexRename_MoveUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexRename_MoveUp.Location = New System.Drawing.Point(399, 24)
        Me.btn_tv_RegexRename_MoveUp.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexRename_MoveUp.Name = "btn_tv_RegexRename_MoveUp"
        Me.btn_tv_RegexRename_MoveUp.Size = New System.Drawing.Size(36, 29)
        Me.btn_tv_RegexRename_MoveUp.TabIndex = 42
        Me.btn_tv_RegexRename_MoveUp.Text = "^"
        Me.ToolTip1.SetToolTip(Me.btn_tv_RegexRename_MoveUp, "Alter Selected RegEx's"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"           Priority")
        Me.btn_tv_RegexRename_MoveUp.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexScrape_MoveDown
        '
        Me.btn_tv_RegexScrape_MoveDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_MoveDown.Location = New System.Drawing.Point(398, 127)
        Me.btn_tv_RegexScrape_MoveDown.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_MoveDown.Name = "btn_tv_RegexScrape_MoveDown"
        Me.btn_tv_RegexScrape_MoveDown.Size = New System.Drawing.Size(36, 29)
        Me.btn_tv_RegexScrape_MoveDown.TabIndex = 33
        Me.btn_tv_RegexScrape_MoveDown.Text = "v"
        Me.ToolTip1.SetToolTip(Me.btn_tv_RegexScrape_MoveDown, "Alter Selected RegEx's"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"           Priority")
        Me.btn_tv_RegexScrape_MoveDown.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexScrape_MoveUp
        '
        Me.btn_tv_RegexScrape_MoveUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_MoveUp.Location = New System.Drawing.Point(398, 21)
        Me.btn_tv_RegexScrape_MoveUp.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_MoveUp.Name = "btn_tv_RegexScrape_MoveUp"
        Me.btn_tv_RegexScrape_MoveUp.Size = New System.Drawing.Size(36, 29)
        Me.btn_tv_RegexScrape_MoveUp.TabIndex = 32
        Me.btn_tv_RegexScrape_MoveUp.Text = "^"
        Me.ToolTip1.SetToolTip(Me.btn_tv_RegexScrape_MoveUp, "Alter Selected RegEx's"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"           Priority")
        Me.btn_tv_RegexScrape_MoveUp.UseVisualStyleBackColor = true
        '
        'cb_MovPosterTabTMDBSelect
        '
        Me.cb_MovPosterTabTMDBSelect.AutoSize = true
        Me.cb_MovPosterTabTMDBSelect.Location = New System.Drawing.Point(8, 431)
        Me.cb_MovPosterTabTMDBSelect.Name = "cb_MovPosterTabTMDBSelect"
        Me.cb_MovPosterTabTMDBSelect.Size = New System.Drawing.Size(255, 19)
        Me.cb_MovPosterTabTMDBSelect.TabIndex = 95
        Me.cb_MovPosterTabTMDBSelect.Text = "Poster Tab, Preselect loading from TMDB."
        Me.ToolTip1.SetToolTip(Me.cb_MovPosterTabTMDBSelect, "If checked, on entering of Poster Tab, will"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"act as if TMDB button has been press"& _ 
        "ed by user.")
        Me.cb_MovPosterTabTMDBSelect.UseVisualStyleBackColor = true
        '
        'cbAllowUserTags
        '
        Me.cbAllowUserTags.AutoSize = true
        Me.cbAllowUserTags.Location = New System.Drawing.Point(11, 16)
        Me.cbAllowUserTags.Name = "cbAllowUserTags"
        Me.cbAllowUserTags.Size = New System.Drawing.Size(108, 19)
        Me.cbAllowUserTags.TabIndex = 70
        Me.cbAllowUserTags.Text = "Allow user tags"
        Me.ToolTip1.SetToolTip(Me.cbAllowUserTags, "When checked allows you to enter your own comma separated tags in the Tags field")
        Me.cbAllowUserTags.UseVisualStyleBackColor = true
        '
        'cbEnableFolderSize
        '
        Me.cbEnableFolderSize.AutoSize = true
        Me.cbEnableFolderSize.Location = New System.Drawing.Point(8, 477)
        Me.cbEnableFolderSize.Name = "cbEnableFolderSize"
        Me.cbEnableFolderSize.Size = New System.Drawing.Size(174, 19)
        Me.cbEnableFolderSize.TabIndex = 97
        Me.cbEnableFolderSize.Text = "Get FolderSize for Movie(s)"
        Me.ToolTip1.SetToolTip(Me.cbEnableFolderSize, "This feature can slow performance of MC"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"if there are multiple movies in Sub Fold"& _ 
        "ers.")
        Me.cbEnableFolderSize.UseVisualStyleBackColor = true
        '
        'cbDisplayLocalActor
        '
        Me.cbDisplayLocalActor.AutoSize = true
        Me.cbDisplayLocalActor.Location = New System.Drawing.Point(408, 352)
        Me.cbDisplayLocalActor.Name = "cbDisplayLocalActor"
        Me.cbDisplayLocalActor.Size = New System.Drawing.Size(198, 19)
        Me.cbDisplayLocalActor.TabIndex = 113
        Me.cbDisplayLocalActor.Text = "Display Local Actor images only"
        Me.ToolTip1.SetToolTip(Me.cbDisplayLocalActor, "If selected, MC will not attempt to download actor"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"images from Internet if image"& _ 
        " is not locally stored")
        Me.cbDisplayLocalActor.UseVisualStyleBackColor = true
        '
        'cbCheckForNewVersion
        '
        Me.cbCheckForNewVersion.AutoSize = true
        Me.cbCheckForNewVersion.Location = New System.Drawing.Point(408, 297)
        Me.cbCheckForNewVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.cbCheckForNewVersion.Name = "cbCheckForNewVersion"
        Me.cbCheckForNewVersion.Size = New System.Drawing.Size(198, 19)
        Me.cbCheckForNewVersion.TabIndex = 112
        Me.cbCheckForNewVersion.Text = "Check for new version at startup"
        Me.ToolTip1.SetToolTip(Me.cbCheckForNewVersion, "Currently only implemented in movies 'Refresh All'. "&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Enable for maximum performa"& _ 
        "nce."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Disable if you encounter a problem.")
        Me.cbCheckForNewVersion.UseVisualStyleBackColor = true
        '
        'cbUseMultipleThreads
        '
        Me.cbUseMultipleThreads.AutoSize = true
        Me.cbUseMultipleThreads.Location = New System.Drawing.Point(21, 402)
        Me.cbUseMultipleThreads.Margin = New System.Windows.Forms.Padding(4)
        Me.cbUseMultipleThreads.Name = "cbUseMultipleThreads"
        Me.cbUseMultipleThreads.Size = New System.Drawing.Size(278, 19)
        Me.cbUseMultipleThreads.TabIndex = 110
        Me.cbUseMultipleThreads.Text = "Use multiple threaded version where available"
        Me.ToolTip1.SetToolTip(Me.cbUseMultipleThreads, "Currently only implemented in movies 'Refresh All'. "&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Enable for maximum performa"& _ 
        "nce."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Disable if you encounter a problem.")
        Me.cbUseMultipleThreads.UseVisualStyleBackColor = true
        '
        'btnFindBrowser
        '
        Me.btnFindBrowser.Enabled = false
        Me.btnFindBrowser.Location = New System.Drawing.Point(58, 318)
        Me.btnFindBrowser.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFindBrowser.Name = "btnFindBrowser"
        Me.btnFindBrowser.Size = New System.Drawing.Size(112, 26)
        Me.btnFindBrowser.TabIndex = 108
        Me.btnFindBrowser.Text = "Locate browser..."
        Me.ToolTip1.SetToolTip(Me.btnFindBrowser, "Select external browser to use. ")
        Me.btnFindBrowser.UseVisualStyleBackColor = false
        '
        'tbaltnfoeditor
        '
        Me.tbaltnfoeditor.Location = New System.Drawing.Point(46, 17)
        Me.tbaltnfoeditor.Name = "tbaltnfoeditor"
        Me.tbaltnfoeditor.ReadOnly = true
        Me.tbaltnfoeditor.Size = New System.Drawing.Size(287, 21)
        Me.tbaltnfoeditor.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.tbaltnfoeditor, "Browse to Program Files\MKVToolNix\mmg.exe")
        '
        'GroupBox36
        '
        Me.GroupBox36.Controls.Add(Me.llMkvMergeGuiPath)
        Me.GroupBox36.Controls.Add(Me.btnMkvMergeGuiPath)
        Me.GroupBox36.Controls.Add(Me.tbMkvMergeGuiPath)
        Me.GroupBox36.Controls.Add(Me.Label19)
        Me.GroupBox36.Location = New System.Drawing.Point(486, 212)
        Me.GroupBox36.Name = "GroupBox36"
        Me.GroupBox36.Size = New System.Drawing.Size(364, 67)
        Me.GroupBox36.TabIndex = 104
        Me.GroupBox36.TabStop = false
        Me.GroupBox36.Text = "mkvmerge GUI"
        Me.ToolTip1.SetToolTip(Me.GroupBox36, "Use this program to save disk space by removing unwanted extra audio and subtitle"& _ 
        " tracks")
        '
        'llMkvMergeGuiPath
        '
        Me.llMkvMergeGuiPath.AutoSize = true
        Me.llMkvMergeGuiPath.Location = New System.Drawing.Point(43, 45)
        Me.llMkvMergeGuiPath.Name = "llMkvMergeGuiPath"
        Me.llMkvMergeGuiPath.Size = New System.Drawing.Size(85, 15)
        Me.llMkvMergeGuiPath.TabIndex = 3
        Me.llMkvMergeGuiPath.TabStop = true
        Me.llMkvMergeGuiPath.Text = "Download link"
        '
        'btnMkvMergeGuiPath
        '
        Me.btnMkvMergeGuiPath.Location = New System.Drawing.Point(332, 21)
        Me.btnMkvMergeGuiPath.Name = "btnMkvMergeGuiPath"
        Me.btnMkvMergeGuiPath.Size = New System.Drawing.Size(26, 23)
        Me.btnMkvMergeGuiPath.TabIndex = 2
        Me.btnMkvMergeGuiPath.Text = "..."
        Me.btnMkvMergeGuiPath.UseVisualStyleBackColor = true
        '
        'tbMkvMergeGuiPath
        '
        Me.tbMkvMergeGuiPath.Location = New System.Drawing.Point(46, 21)
        Me.tbMkvMergeGuiPath.Name = "tbMkvMergeGuiPath"
        Me.tbMkvMergeGuiPath.ReadOnly = true
        Me.tbMkvMergeGuiPath.Size = New System.Drawing.Size(287, 21)
        Me.tbMkvMergeGuiPath.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.tbMkvMergeGuiPath, "Browse to Program Files\MKVToolNix\mmg.exe")
        '
        'Label19
        '
        Me.Label19.AutoSize = true
        Me.Label19.Location = New System.Drawing.Point(10, 24)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(35, 15)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Path:"
        '
        'cmbxTvMaxGenres
        '
        Me.cmbxTvMaxGenres.FormattingEnabled = true
        Me.cmbxTvMaxGenres.Items.AddRange(New Object() {"All Available", "None", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cmbxTvMaxGenres.Location = New System.Drawing.Point(150, 303)
        Me.cmbxTvMaxGenres.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbxTvMaxGenres.MaxDropDownItems = 30
        Me.cmbxTvMaxGenres.Name = "cmbxTvMaxGenres"
        Me.cmbxTvMaxGenres.Size = New System.Drawing.Size(140, 23)
        Me.cmbxTvMaxGenres.TabIndex = 76
        Me.ToolTip1.SetToolTip(Me.cmbxTvMaxGenres, "Media Companion will not scrape more than"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"tne number of genres set with this con"& _ 
        "trol")
        '
        'cbDisplayDefaultSubtitleLang
        '
        Me.cbDisplayDefaultSubtitleLang.AutoSize = true
        Me.cbDisplayDefaultSubtitleLang.Location = New System.Drawing.Point(6, 121)
        Me.cbDisplayDefaultSubtitleLang.Name = "cbDisplayDefaultSubtitleLang"
        Me.cbDisplayDefaultSubtitleLang.Size = New System.Drawing.Size(273, 19)
        Me.cbDisplayDefaultSubtitleLang.TabIndex = 108
        Me.cbDisplayDefaultSubtitleLang.Text = "Display Default Subtitle lang in Media Overlay"
        Me.ToolTip1.SetToolTip(Me.cbDisplayDefaultSubtitleLang, "If any track is set as Default Subtitle, display Subtitle language"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"else display "& _ 
        "First subtitle language."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"If unselected, First subtitle language is shown.")
        Me.cbDisplayDefaultSubtitleLang.UseVisualStyleBackColor = true
        '
        'cbSetIdAsCollectionnumber
        '
        Me.cbSetIdAsCollectionnumber.AutoSize = true
        Me.cbSetIdAsCollectionnumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbSetIdAsCollectionnumber.Location = New System.Drawing.Point(37, 36)
        Me.cbSetIdAsCollectionnumber.Name = "cbSetIdAsCollectionnumber"
        Me.cbSetIdAsCollectionnumber.Size = New System.Drawing.Size(207, 17)
        Me.cbSetIdAsCollectionnumber.TabIndex = 52
        Me.cbSetIdAsCollectionnumber.Text = "Save setid also as <collectionnumber>"
        Me.ToolTip1.SetToolTip(Me.cbSetIdAsCollectionnumber, "Compatibility for Emby manager.")
        Me.cbSetIdAsCollectionnumber.UseVisualStyleBackColor = true
        '
        'cbtvDisplayNextAiredToolTip
        '
        Me.cbtvDisplayNextAiredToolTip.AutoSize = true
        Me.cbtvDisplayNextAiredToolTip.Location = New System.Drawing.Point(132, 377)
        Me.cbtvDisplayNextAiredToolTip.Name = "cbtvDisplayNextAiredToolTip"
        Me.cbtvDisplayNextAiredToolTip.Size = New System.Drawing.Size(205, 19)
        Me.cbtvDisplayNextAiredToolTip.TabIndex = 74
        Me.cbtvDisplayNextAiredToolTip.Text = "Display next to air episode tooltip"
        Me.ToolTip1.SetToolTip(Me.cbtvDisplayNextAiredToolTip, "Only over TV Show, and will only display if Display Missing"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Episodes option is e"& _ 
        "nabled also.")
        Me.cbtvDisplayNextAiredToolTip.UseVisualStyleBackColor = true
        '
        'cbEnableMovDeleteFolderTsmi
        '
        Me.cbEnableMovDeleteFolderTsmi.AutoSize = true
        Me.cbEnableMovDeleteFolderTsmi.Location = New System.Drawing.Point(7, 500)
        Me.cbEnableMovDeleteFolderTsmi.Name = "cbEnableMovDeleteFolderTsmi"
        Me.cbEnableMovDeleteFolderTsmi.Size = New System.Drawing.Size(266, 19)
        Me.cbEnableMovDeleteFolderTsmi.TabIndex = 83
        Me.cbEnableMovDeleteFolderTsmi.Text = "Enable 'Delete Movie Folder' context option."
        Me.ToolTip1.SetToolTip(Me.cbEnableMovDeleteFolderTsmi, "***Danger***"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Will Allow user to delete movie if in its own folder."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Will delete "& _ 
        "to Recycle Bin if on local hard drive, but"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"from NAS, or if Recycle Bin disabled"& _ 
        ", deletes permanently.")
        Me.cbEnableMovDeleteFolderTsmi.UseVisualStyleBackColor = true
        '
        'cbMovAllowNonImdbIdAsId
        '
        Me.cbMovAllowNonImdbIdAsId.AutoSize = true
        Me.cbMovAllowNonImdbIdAsId.Location = New System.Drawing.Point(21, 299)
        Me.cbMovAllowNonImdbIdAsId.Name = "cbMovAllowNonImdbIdAsId"
        Me.cbMovAllowNonImdbIdAsId.Size = New System.Drawing.Size(209, 34)
        Me.cbMovAllowNonImdbIdAsId.TabIndex = 80
        Me.cbMovAllowNonImdbIdAsId.Text = "Allow Non IMDB Id's as IMDB Id's"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"(Must start with alpha character)"
        Me.ToolTip1.SetToolTip(Me.cbMovAllowNonImdbIdAsId, "This option is explicitly for filtering of Actors/Directors"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"from foreign scraped"& _ 
        " movies.  ID's MUST start with"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Alpha character to differentiate from TMDB Id's."& _ 
        ""&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"ie: RB-1234 or IP8888")
        Me.cbMovAllowNonImdbIdAsId.UseVisualStyleBackColor = true
        '
        'rbMovSetArtScrapeFanartTv
        '
        Me.rbMovSetArtScrapeFanartTv.AutoSize = true
        Me.rbMovSetArtScrapeFanartTv.Location = New System.Drawing.Point(17, 246)
        Me.rbMovSetArtScrapeFanartTv.Name = "rbMovSetArtScrapeFanartTv"
        Me.rbMovSetArtScrapeFanartTv.Size = New System.Drawing.Size(216, 19)
        Me.rbMovSetArtScrapeFanartTv.TabIndex = 52
        Me.rbMovSetArtScrapeFanartTv.TabStop = true
        Me.rbMovSetArtScrapeFanartTv.Text = "From Fanart.TV (Fallback to TMDb)"
        Me.ToolTip1.SetToolTip(Me.rbMovSetArtScrapeFanartTv, "If no MovieSet artwork on Fanart.Tv"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"get Poster and Fanart from TMDb.")
        Me.rbMovSetArtScrapeFanartTv.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbActorFromImdb
        '
        Me.cbXbmcTmdbActorFromImdb.AutoSize = true
        Me.cbXbmcTmdbActorFromImdb.Location = New System.Drawing.Point(10, 270)
        Me.cbXbmcTmdbActorFromImdb.Name = "cbXbmcTmdbActorFromImdb"
        Me.cbXbmcTmdbActorFromImdb.Size = New System.Drawing.Size(183, 19)
        Me.cbXbmcTmdbActorFromImdb.TabIndex = 75
        Me.cbXbmcTmdbActorFromImdb.Text = "Download Actors From IMDb"
        Me.cbXbmcTmdbActorFromImdb.UseVisualStyleBackColor = true
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(589, 496)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 50)
        Me.PictureBox1.TabIndex = 12
        Me.PictureBox1.TabStop = false
        Me.PictureBox1.Visible = false
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TPGenCom)
        Me.TabControl1.Controls.Add(Me.TPMovPref)
        Me.TabControl1.Controls.Add(Me.TPTVPref)
        Me.TabControl1.Controls.Add(Me.TPHmPref)
        Me.TabControl1.Controls.Add(Me.TPProxy)
        Me.TabControl1.Controls.Add(Me.TPXBMCLink)
        Me.TabControl1.Controls.Add(Me.TPPRofCmd)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1008, 623)
        Me.TabControl1.TabIndex = 15
        '
        'TPGenCom
        '
        Me.TPGenCom.Controls.Add(Me.TabControl4)
        Me.TPGenCom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TPGenCom.Location = New System.Drawing.Point(4, 24)
        Me.TPGenCom.Name = "TPGenCom"
        Me.TPGenCom.Size = New System.Drawing.Size(1000, 595)
        Me.TPGenCom.TabIndex = 5
        Me.TPGenCom.Text = "General & Common"
        Me.TPGenCom.UseVisualStyleBackColor = true
        '
        'TabControl4
        '
        Me.TabControl4.Controls.Add(Me.TPGeneral)
        Me.TabControl4.Controls.Add(Me.TPCommonSettings)
        Me.TabControl4.Controls.Add(Me.TPActors)
        Me.TabControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl4.Location = New System.Drawing.Point(0, 0)
        Me.TabControl4.Name = "TabControl4"
        Me.TabControl4.SelectedIndex = 0
        Me.TabControl4.Size = New System.Drawing.Size(1000, 595)
        Me.TabControl4.TabIndex = 0
        '
        'TPGeneral
        '
        Me.TPGeneral.Controls.Add(Me.lbExternalBrowserPath)
        Me.TPGeneral.Controls.Add(Me.cbAutoHideStatusBar)
        Me.TPGeneral.Controls.Add(Me.cbMcCloseMCForDLNewVersion)
        Me.TPGeneral.Controls.Add(Me.Label2)
        Me.TPGeneral.Controls.Add(Me.cbMultiMonitorEnable)
        Me.TPGeneral.Controls.Add(Me.cbDisplayLocalActor)
        Me.TPGeneral.Controls.Add(Me.cbCheckForNewVersion)
        Me.TPGeneral.Controls.Add(Me.cbRenameNFOtoINFO)
        Me.TPGeneral.Controls.Add(Me.cbUseMultipleThreads)
        Me.TPGeneral.Controls.Add(Me.cbShowLogOnError)
        Me.TPGeneral.Controls.Add(Me.btnFindBrowser)
        Me.TPGeneral.Controls.Add(Me.cbExternalbrowser)
        Me.TPGeneral.Controls.Add(Me.chkbx_disablecache)
        Me.TPGeneral.Controls.Add(Me.GroupBox45)
        Me.TPGeneral.Controls.Add(Me.GroupBox36)
        Me.TPGeneral.Controls.Add(Me.GroupBox33)
        Me.TPGeneral.Controls.Add(Me.GroupBox31)
        Me.TPGeneral.Controls.Add(Me.GroupBox3)
        Me.TPGeneral.Location = New System.Drawing.Point(4, 24)
        Me.TPGeneral.Name = "TPGeneral"
        Me.TPGeneral.Size = New System.Drawing.Size(992, 567)
        Me.TPGeneral.TabIndex = 2
        Me.TPGeneral.Text = "General"
        Me.TPGeneral.UseVisualStyleBackColor = true
        '
        'lbExternalBrowserPath
        '
        Me.lbExternalBrowserPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbExternalBrowserPath.Location = New System.Drawing.Point(17, 348)
        Me.lbExternalBrowserPath.Name = "lbExternalBrowserPath"
        Me.lbExternalBrowserPath.Size = New System.Drawing.Size(370, 23)
        Me.lbExternalBrowserPath.TabIndex = 118
        Me.lbExternalBrowserPath.Text = "-None-"
        '
        'cbAutoHideStatusBar
        '
        Me.cbAutoHideStatusBar.AutoSize = true
        Me.cbAutoHideStatusBar.Location = New System.Drawing.Point(21, 451)
        Me.cbAutoHideStatusBar.Name = "cbAutoHideStatusBar"
        Me.cbAutoHideStatusBar.Size = New System.Drawing.Size(222, 19)
        Me.cbAutoHideStatusBar.TabIndex = 117
        Me.cbAutoHideStatusBar.Text = "Auto Hide Status Bar on completion."
        Me.cbAutoHideStatusBar.UseVisualStyleBackColor = true
        '
        'cbMcCloseMCForDLNewVersion
        '
        Me.cbMcCloseMCForDLNewVersion.Location = New System.Drawing.Point(417, 313)
        Me.cbMcCloseMCForDLNewVersion.Name = "cbMcCloseMCForDLNewVersion"
        Me.cbMcCloseMCForDLNewVersion.Size = New System.Drawing.Size(219, 38)
        Me.cbMcCloseMCForDLNewVersion.TabIndex = 116
        Me.cbMcCloseMCForDLNewVersion.Text = "If accept download of new version, close Media Companion."
        Me.cbMcCloseMCForDLNewVersion.UseVisualStyleBackColor = true
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label2.Location = New System.Drawing.Point(21, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(725, 22)
        Me.Label2.TabIndex = 115
        Me.Label2.Text = "Options on this Tab are General Media Companion options not specific to any Video"& _ 
    " type."
        '
        'cbMultiMonitorEnable
        '
        Me.cbMultiMonitorEnable.AutoSize = true
        Me.cbMultiMonitorEnable.Location = New System.Drawing.Point(408, 378)
        Me.cbMultiMonitorEnable.Name = "cbMultiMonitorEnable"
        Me.cbMultiMonitorEnable.Size = New System.Drawing.Size(187, 19)
        Me.cbMultiMonitorEnable.TabIndex = 114
        Me.cbMultiMonitorEnable.Text = "Enable Multi-Monitor Support"
        Me.cbMultiMonitorEnable.UseVisualStyleBackColor = true
        '
        'cbRenameNFOtoINFO
        '
        Me.cbRenameNFOtoINFO.AutoSize = true
        Me.cbRenameNFOtoINFO.Location = New System.Drawing.Point(408, 403)
        Me.cbRenameNFOtoINFO.Name = "cbRenameNFOtoINFO"
        Me.cbRenameNFOtoINFO.Size = New System.Drawing.Size(272, 19)
        Me.cbRenameNFOtoINFO.TabIndex = 111
        Me.cbRenameNFOtoINFO.Text = "Rename Non-Compliant Scene '.nfo' to '.info'"
        Me.cbRenameNFOtoINFO.UseVisualStyleBackColor = true
        '
        'cbShowLogOnError
        '
        Me.cbShowLogOnError.AutoSize = true
        Me.cbShowLogOnError.Location = New System.Drawing.Point(21, 425)
        Me.cbShowLogOnError.Margin = New System.Windows.Forms.Padding(4)
        Me.cbShowLogOnError.Name = "cbShowLogOnError"
        Me.cbShowLogOnError.Size = New System.Drawing.Size(123, 19)
        Me.cbShowLogOnError.TabIndex = 109
        Me.cbShowLogOnError.Text = "Show log on error"
        Me.cbShowLogOnError.UseVisualStyleBackColor = true
        '
        'cbExternalbrowser
        '
        Me.cbExternalbrowser.AutoSize = true
        Me.cbExternalbrowser.Location = New System.Drawing.Point(24, 297)
        Me.cbExternalbrowser.Margin = New System.Windows.Forms.Padding(4)
        Me.cbExternalbrowser.Name = "cbExternalbrowser"
        Me.cbExternalbrowser.Size = New System.Drawing.Size(325, 19)
        Me.cbExternalbrowser.TabIndex = 107
        Me.cbExternalbrowser.Text = "Use external Browser to display IMDB/TVDB webpages"
        Me.cbExternalbrowser.UseVisualStyleBackColor = true
        '
        'chkbx_disablecache
        '
        Me.chkbx_disablecache.AutoSize = true
        Me.chkbx_disablecache.Location = New System.Drawing.Point(21, 378)
        Me.chkbx_disablecache.Margin = New System.Windows.Forms.Padding(4)
        Me.chkbx_disablecache.Name = "chkbx_disablecache"
        Me.chkbx_disablecache.Size = New System.Drawing.Size(315, 19)
        Me.chkbx_disablecache.TabIndex = 106
        Me.chkbx_disablecache.Text = "Disable caching of Media DB (will slow down startup)"
        Me.chkbx_disablecache.UseVisualStyleBackColor = true
        '
        'GroupBox45
        '
        Me.GroupBox45.Controls.Add(Me.lblaltnfoeditorclear)
        Me.GroupBox45.Controls.Add(Me.btnaltnfoeditor)
        Me.GroupBox45.Controls.Add(Me.tbaltnfoeditor)
        Me.GroupBox45.Controls.Add(Me.Label20)
        Me.GroupBox45.Location = New System.Drawing.Point(486, 140)
        Me.GroupBox45.Name = "GroupBox45"
        Me.GroupBox45.Size = New System.Drawing.Size(364, 66)
        Me.GroupBox45.TabIndex = 105
        Me.GroupBox45.TabStop = false
        Me.GroupBox45.Text = "Alternative nfo viewer/editor"
        '
        'lblaltnfoeditorclear
        '
        Me.lblaltnfoeditorclear.AutoSize = true
        Me.lblaltnfoeditorclear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblaltnfoeditorclear.Location = New System.Drawing.Point(55, 41)
        Me.lblaltnfoeditorclear.Name = "lblaltnfoeditorclear"
        Me.lblaltnfoeditorclear.Size = New System.Drawing.Size(65, 17)
        Me.lblaltnfoeditorclear.TabIndex = 5
        Me.lblaltnfoeditorclear.Text = "Clear path"
        '
        'btnaltnfoeditor
        '
        Me.btnaltnfoeditor.Location = New System.Drawing.Point(332, 15)
        Me.btnaltnfoeditor.Name = "btnaltnfoeditor"
        Me.btnaltnfoeditor.Size = New System.Drawing.Size(26, 23)
        Me.btnaltnfoeditor.TabIndex = 4
        Me.btnaltnfoeditor.Text = "..."
        Me.btnaltnfoeditor.UseVisualStyleBackColor = true
        '
        'Label20
        '
        Me.Label20.AutoSize = true
        Me.Label20.Location = New System.Drawing.Point(10, 20)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(35, 15)
        Me.Label20.TabIndex = 4
        Me.Label20.Text = "Path:"
        '
        'GroupBox33
        '
        Me.GroupBox33.Controls.Add(Me.btnFontSelect)
        Me.GroupBox33.Controls.Add(Me.btnFontReset)
        Me.GroupBox33.Controls.Add(Me.lbl_FontSample)
        Me.GroupBox33.Location = New System.Drawing.Point(486, 49)
        Me.GroupBox33.Name = "GroupBox33"
        Me.GroupBox33.Size = New System.Drawing.Size(364, 85)
        Me.GroupBox33.TabIndex = 103
        Me.GroupBox33.TabStop = false
        Me.GroupBox33.Text = "Interface Font"
        '
        'btnFontSelect
        '
        Me.btnFontSelect.Location = New System.Drawing.Point(7, 21)
        Me.btnFontSelect.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFontSelect.Name = "btnFontSelect"
        Me.btnFontSelect.Size = New System.Drawing.Size(90, 26)
        Me.btnFontSelect.TabIndex = 34
        Me.btnFontSelect.Text = "Select"
        Me.btnFontSelect.UseVisualStyleBackColor = true
        '
        'btnFontReset
        '
        Me.btnFontReset.Location = New System.Drawing.Point(104, 21)
        Me.btnFontReset.Name = "btnFontReset"
        Me.btnFontReset.Size = New System.Drawing.Size(75, 26)
        Me.btnFontReset.TabIndex = 41
        Me.btnFontReset.Text = "Reset Font"
        Me.btnFontReset.UseVisualStyleBackColor = true
        '
        'lbl_FontSample
        '
        Me.lbl_FontSample.AutoSize = true
        Me.lbl_FontSample.Location = New System.Drawing.Point(10, 59)
        Me.lbl_FontSample.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbl_FontSample.Name = "lbl_FontSample"
        Me.lbl_FontSample.Size = New System.Drawing.Size(77, 15)
        Me.lbl_FontSample.TabIndex = 36
        Me.lbl_FontSample.Text = "Sample Font"
        Me.lbl_FontSample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox31
        '
        Me.GroupBox31.Controls.Add(Me.Label116)
        Me.GroupBox31.Controls.Add(Me.Label107)
        Me.GroupBox31.Controls.Add(Me.txtbx_minrarsize)
        Me.GroupBox31.Location = New System.Drawing.Point(14, 191)
        Me.GroupBox31.Name = "GroupBox31"
        Me.GroupBox31.Size = New System.Drawing.Size(456, 88)
        Me.GroupBox31.TabIndex = 102
        Me.GroupBox31.TabStop = false
        Me.GroupBox31.Text = "RAR Archives"
        '
        'Label116
        '
        Me.Label116.Location = New System.Drawing.Point(7, 17)
        Me.Label116.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(442, 33)
        Me.Label116.TabIndex = 32
        Me.Label116.Text = "Media Companion can scrape data for RAR archives, to avoid scraping non-media arc"& _ 
    "hives (eg. subtitles)."
        '
        'Label107
        '
        Me.Label107.AutoSize = true
        Me.Label107.Location = New System.Drawing.Point(81, 57)
        Me.Label107.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label107.Name = "Label107"
        Me.Label107.Size = New System.Drawing.Size(319, 15)
        Me.Label107.TabIndex = 27
        Me.Label107.Text = "File size in MB (archives smaller than this will be ignored)"
        '
        'txtbx_minrarsize
        '
        Me.txtbx_minrarsize.Location = New System.Drawing.Point(10, 54)
        Me.txtbx_minrarsize.Margin = New System.Windows.Forms.Padding(4)
        Me.txtbx_minrarsize.Name = "txtbx_minrarsize"
        Me.txtbx_minrarsize.Size = New System.Drawing.Size(63, 21)
        Me.txtbx_minrarsize.TabIndex = 26
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.lbl_MediaPlayerUser)
        Me.GroupBox3.Controls.Add(Me.btn_MediaPlayerBrowse)
        Me.GroupBox3.Controls.Add(Me.rb_MediaPlayerUser)
        Me.GroupBox3.Controls.Add(Me.rb_MediaPlayerWMP)
        Me.GroupBox3.Controls.Add(Me.rb_MediaPlayerDefault)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(14, 49)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(456, 136)
        Me.GroupBox3.TabIndex = 101
        Me.GroupBox3.TabStop = false
        Me.GroupBox3.Text = "Media Player"
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(421, 26)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Media Companion can be used to playback media files.  Use the options below to se"& _ 
    "lect"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"what program to use for playback.  Multipart media files utilises a m3u pl"& _ 
    "aylist."
        '
        'lbl_MediaPlayerUser
        '
        Me.lbl_MediaPlayerUser.AutoSize = true
        Me.lbl_MediaPlayerUser.Location = New System.Drawing.Point(6, 115)
        Me.lbl_MediaPlayerUser.Name = "lbl_MediaPlayerUser"
        Me.lbl_MediaPlayerUser.Size = New System.Drawing.Size(11, 13)
        Me.lbl_MediaPlayerUser.TabIndex = 8
        Me.lbl_MediaPlayerUser.Text = "."
        '
        'btn_MediaPlayerBrowse
        '
        Me.btn_MediaPlayerBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btn_MediaPlayerBrowse.Location = New System.Drawing.Point(168, 88)
        Me.btn_MediaPlayerBrowse.Name = "btn_MediaPlayerBrowse"
        Me.btn_MediaPlayerBrowse.Size = New System.Drawing.Size(185, 23)
        Me.btn_MediaPlayerBrowse.TabIndex = 7
        Me.btn_MediaPlayerBrowse.Text = "Browse to preferred media player"
        Me.btn_MediaPlayerBrowse.UseVisualStyleBackColor = true
        '
        'rb_MediaPlayerUser
        '
        Me.rb_MediaPlayerUser.AutoSize = true
        Me.rb_MediaPlayerUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rb_MediaPlayerUser.Location = New System.Drawing.Point(6, 91)
        Me.rb_MediaPlayerUser.Name = "rb_MediaPlayerUser"
        Me.rb_MediaPlayerUser.Size = New System.Drawing.Size(129, 17)
        Me.rb_MediaPlayerUser.TabIndex = 6
        Me.rb_MediaPlayerUser.Text = "Select Different player"
        Me.rb_MediaPlayerUser.UseVisualStyleBackColor = true
        '
        'rb_MediaPlayerWMP
        '
        Me.rb_MediaPlayerWMP.AutoSize = true
        Me.rb_MediaPlayerWMP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rb_MediaPlayerWMP.Location = New System.Drawing.Point(6, 68)
        Me.rb_MediaPlayerWMP.Name = "rb_MediaPlayerWMP"
        Me.rb_MediaPlayerWMP.Size = New System.Drawing.Size(367, 17)
        Me.rb_MediaPlayerWMP.TabIndex = 1
        Me.rb_MediaPlayerWMP.Text = "Use Windows Media Player --- Launch WMP and play the selected file(s)"
        Me.rb_MediaPlayerWMP.UseVisualStyleBackColor = true
        '
        'rb_MediaPlayerDefault
        '
        Me.rb_MediaPlayerDefault.AutoSize = true
        Me.rb_MediaPlayerDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rb_MediaPlayerDefault.Location = New System.Drawing.Point(6, 45)
        Me.rb_MediaPlayerDefault.Name = "rb_MediaPlayerDefault"
        Me.rb_MediaPlayerDefault.Size = New System.Drawing.Size(446, 17)
        Me.rb_MediaPlayerDefault.TabIndex = 0
        Me.rb_MediaPlayerDefault.Text = "Use Default Player  ---------------  This option will open the default player for"& _ 
    " .m3u playlist files"
        Me.rb_MediaPlayerDefault.UseVisualStyleBackColor = true
        '
        'TPCommonSettings
        '
        Me.TPCommonSettings.Controls.Add(Me.Label26)
        Me.TPCommonSettings.Controls.Add(Me.cbRunTimePadding)
        Me.TPCommonSettings.Controls.Add(Me.gpbxPrefIgnore)
        Me.TPCommonSettings.Controls.Add(Me.gpbxPrefOverlay)
        Me.TPCommonSettings.Controls.Add(Me.cbGenreCustomBefore)
        Me.TPCommonSettings.Controls.Add(Me.btnEditCustomGenreFile)
        Me.TPCommonSettings.Controls.Add(Me.GroupBox4)
        Me.TPCommonSettings.Controls.Add(Me.Label4)
        Me.TPCommonSettings.Controls.Add(Me.gbImageResizing)
        Me.TPCommonSettings.Controls.Add(Me.grpCleanFilename)
        Me.TPCommonSettings.Controls.Add(Me.grpVideoSource)
        Me.TPCommonSettings.Controls.Add(Me.gbExcludeFolders)
        Me.TPCommonSettings.Controls.Add(Me.cbOverwriteArtwork)
        Me.TPCommonSettings.Controls.Add(Me.cbRuntimeAsNumericOnly)
        Me.TPCommonSettings.Controls.Add(Me.gbxXBMCversion)
        Me.TPCommonSettings.Location = New System.Drawing.Point(4, 24)
        Me.TPCommonSettings.Name = "TPCommonSettings"
        Me.TPCommonSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.TPCommonSettings.Size = New System.Drawing.Size(992, 567)
        Me.TPCommonSettings.TabIndex = 0
        Me.TPCommonSettings.Text = "Common Settings"
        Me.TPCommonSettings.UseVisualStyleBackColor = true
        '
        'Label26
        '
        Me.Label26.AutoSize = true
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label26.Location = New System.Drawing.Point(34, 436)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(138, 13)
        Me.Label26.TabIndex = 113
        Me.Label26.Text = "ie: 24 min becomes 024 min"
        '
        'cbRunTimePadding
        '
        Me.cbRunTimePadding.AutoSize = true
        Me.cbRunTimePadding.Location = New System.Drawing.Point(12, 418)
        Me.cbRunTimePadding.Name = "cbRunTimePadding"
        Me.cbRunTimePadding.Size = New System.Drawing.Size(155, 19)
        Me.cbRunTimePadding.TabIndex = 112
        Me.cbRunTimePadding.Text = "Pad runtime to 3 places"
        Me.cbRunTimePadding.UseVisualStyleBackColor = true
        '
        'gpbxPrefIgnore
        '
        Me.gpbxPrefIgnore.Controls.Add(Me.cb_IgnoreAn)
        Me.gpbxPrefIgnore.Controls.Add(Me.cb_IgnoreThe)
        Me.gpbxPrefIgnore.Controls.Add(Me.cb_IgnoreA)
        Me.gpbxPrefIgnore.Location = New System.Drawing.Point(296, 175)
        Me.gpbxPrefIgnore.Name = "gpbxPrefIgnore"
        Me.gpbxPrefIgnore.Size = New System.Drawing.Size(273, 100)
        Me.gpbxPrefIgnore.TabIndex = 111
        Me.gpbxPrefIgnore.TabStop = false
        Me.gpbxPrefIgnore.Text = "Ignore Article options"
        '
        'cb_IgnoreAn
        '
        Me.cb_IgnoreAn.AutoSize = true
        Me.cb_IgnoreAn.Location = New System.Drawing.Point(7, 68)
        Me.cb_IgnoreAn.Name = "cb_IgnoreAn"
        Me.cb_IgnoreAn.Size = New System.Drawing.Size(195, 19)
        Me.cb_IgnoreAn.TabIndex = 93
        Me.cb_IgnoreAn.Text = "Ignore article ""An"" when sorting"
        Me.cb_IgnoreAn.UseVisualStyleBackColor = true
        '
        'cb_IgnoreThe
        '
        Me.cb_IgnoreThe.AutoSize = true
        Me.cb_IgnoreThe.Location = New System.Drawing.Point(7, 21)
        Me.cb_IgnoreThe.Margin = New System.Windows.Forms.Padding(4)
        Me.cb_IgnoreThe.Name = "cb_IgnoreThe"
        Me.cb_IgnoreThe.Size = New System.Drawing.Size(205, 19)
        Me.cb_IgnoreThe.TabIndex = 89
        Me.cb_IgnoreThe.Text = "Ignore article ""The "" when sorting"
        Me.cb_IgnoreThe.UseVisualStyleBackColor = true
        '
        'cb_IgnoreA
        '
        Me.cb_IgnoreA.AutoSize = true
        Me.cb_IgnoreA.Location = New System.Drawing.Point(7, 43)
        Me.cb_IgnoreA.Name = "cb_IgnoreA"
        Me.cb_IgnoreA.Size = New System.Drawing.Size(194, 19)
        Me.cb_IgnoreA.TabIndex = 91
        Me.cb_IgnoreA.Text = "Ignore article ""A ""  when sorting"
        Me.cb_IgnoreA.UseVisualStyleBackColor = true
        '
        'gpbxPrefOverlay
        '
        Me.gpbxPrefOverlay.Controls.Add(Me.cbDisplayRatingOverlay)
        Me.gpbxPrefOverlay.Controls.Add(Me.cbDisplayAllSubtitleLang)
        Me.gpbxPrefOverlay.Controls.Add(Me.cbDisplayMediaInfoFolderSize)
        Me.gpbxPrefOverlay.Controls.Add(Me.cbDisplayDefaultSubtitleLang)
        Me.gpbxPrefOverlay.Controls.Add(Me.cbDisplayMediaInfoOverlay)
        Me.gpbxPrefOverlay.Controls.Add(Me.cbShowAllAudioTracks)
        Me.gpbxPrefOverlay.Location = New System.Drawing.Point(6, 175)
        Me.gpbxPrefOverlay.Name = "gpbxPrefOverlay"
        Me.gpbxPrefOverlay.Size = New System.Drawing.Size(284, 176)
        Me.gpbxPrefOverlay.TabIndex = 110
        Me.gpbxPrefOverlay.TabStop = false
        Me.gpbxPrefOverlay.Text = "Overlay options"
        '
        'cbDisplayAllSubtitleLang
        '
        Me.cbDisplayAllSubtitleLang.AutoSize = true
        Me.cbDisplayAllSubtitleLang.Location = New System.Drawing.Point(6, 146)
        Me.cbDisplayAllSubtitleLang.Name = "cbDisplayAllSubtitleLang"
        Me.cbDisplayAllSubtitleLang.Size = New System.Drawing.Size(238, 19)
        Me.cbDisplayAllSubtitleLang.TabIndex = 109
        Me.cbDisplayAllSubtitleLang.Text = "Show all Subtitle lang in Media Overlay"
        Me.cbDisplayAllSubtitleLang.UseVisualStyleBackColor = true
        '
        'cbDisplayMediaInfoFolderSize
        '
        Me.cbDisplayMediaInfoFolderSize.AutoSize = true
        Me.cbDisplayMediaInfoFolderSize.Location = New System.Drawing.Point(6, 70)
        Me.cbDisplayMediaInfoFolderSize.Name = "cbDisplayMediaInfoFolderSize"
        Me.cbDisplayMediaInfoFolderSize.Size = New System.Drawing.Size(233, 19)
        Me.cbDisplayMediaInfoFolderSize.TabIndex = 97
        Me.cbDisplayMediaInfoFolderSize.Text = "Display Folder Size over Fanart Image"
        Me.cbDisplayMediaInfoFolderSize.UseVisualStyleBackColor = true
        '
        'cbGenreCustomBefore
        '
        Me.cbGenreCustomBefore.AutoSize = true
        Me.cbGenreCustomBefore.Location = New System.Drawing.Point(12, 452)
        Me.cbGenreCustomBefore.Name = "cbGenreCustomBefore"
        Me.cbGenreCustomBefore.Size = New System.Drawing.Size(276, 19)
        Me.cbGenreCustomBefore.TabIndex = 107
        Me.cbGenreCustomBefore.Text = "Show Custom Genre's at top of Genre Listbox."
        Me.cbGenreCustomBefore.UseVisualStyleBackColor = true
        '
        'btnEditCustomGenreFile
        '
        Me.btnEditCustomGenreFile.Location = New System.Drawing.Point(24, 477)
        Me.btnEditCustomGenreFile.Name = "btnEditCustomGenreFile"
        Me.btnEditCustomGenreFile.Size = New System.Drawing.Size(195, 23)
        Me.btnEditCustomGenreFile.TabIndex = 106
        Me.btnEditCustomGenreFile.Text = "Create/Edit custom Genre file"
        Me.btnEditCustomGenreFile.UseVisualStyleBackColor = true
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label185)
        Me.GroupBox4.Controls.Add(Me.AutoScrnShtDelay)
        Me.GroupBox4.Location = New System.Drawing.Point(296, 281)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(273, 64)
        Me.GroupBox4.TabIndex = 105
        Me.GroupBox4.TabStop = false
        Me.GroupBox4.Text = "Screenshot Delay."
        '
        'Label185
        '
        Me.Label185.AutoSize = true
        Me.Label185.Location = New System.Drawing.Point(51, 29)
        Me.Label185.Name = "Label185"
        Me.Label185.Size = New System.Drawing.Size(211, 15)
        Me.Label185.TabIndex = 104
        Me.Label185.Text = "AutoScreenShot delay (Seconds only)"
        '
        'AutoScrnShtDelay
        '
        Me.AutoScrnShtDelay.Location = New System.Drawing.Point(8, 26)
        Me.AutoScrnShtDelay.Name = "AutoScrnShtDelay"
        Me.AutoScrnShtDelay.Size = New System.Drawing.Size(37, 21)
        Me.AutoScrnShtDelay.TabIndex = 103
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(348, 26)
        Me.Label4.TabIndex = 102
        Me.Label4.Text = "Common Across All Media Scrapers"
        '
        'gbImageResizing
        '
        Me.gbImageResizing.Controls.Add(Me.Label171)
        Me.gbImageResizing.Controls.Add(Me.Label175)
        Me.gbImageResizing.Controls.Add(Me.Label176)
        Me.gbImageResizing.Controls.Add(Me.comboActorResolutions)
        Me.gbImageResizing.Controls.Add(Me.comboBackDropResolutions)
        Me.gbImageResizing.Controls.Add(Me.comboPosterResolutions)
        Me.gbImageResizing.Location = New System.Drawing.Point(296, 35)
        Me.gbImageResizing.Name = "gbImageResizing"
        Me.gbImageResizing.Size = New System.Drawing.Size(273, 131)
        Me.gbImageResizing.TabIndex = 101
        Me.gbImageResizing.TabStop = false
        Me.gbImageResizing.Text = " Image Resizing "
        '
        'Label171
        '
        Me.Label171.AutoSize = true
        Me.Label171.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label171.Location = New System.Drawing.Point(10, 90)
        Me.Label171.Name = "Label171"
        Me.Label171.Size = New System.Drawing.Size(82, 15)
        Me.Label171.TabIndex = 54
        Me.Label171.Text = "Fanart Sizing:"
        '
        'Label175
        '
        Me.Label175.AutoSize = true
        Me.Label175.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label175.Location = New System.Drawing.Point(15, 32)
        Me.Label175.Name = "Label175"
        Me.Label175.Size = New System.Drawing.Size(77, 15)
        Me.Label175.TabIndex = 53
        Me.Label175.Text = "Actor height :"
        '
        'Label176
        '
        Me.Label176.AutoSize = true
        Me.Label176.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label176.Location = New System.Drawing.Point(7, 61)
        Me.Label176.Name = "Label176"
        Me.Label176.Size = New System.Drawing.Size(85, 15)
        Me.Label176.TabIndex = 52
        Me.Label176.Text = "Poster height :"
        '
        'comboActorResolutions
        '
        Me.comboActorResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboActorResolutions.FormattingEnabled = true
        Me.comboActorResolutions.Location = New System.Drawing.Point(98, 29)
        Me.comboActorResolutions.Name = "comboActorResolutions"
        Me.comboActorResolutions.Size = New System.Drawing.Size(160, 23)
        Me.comboActorResolutions.TabIndex = 51
        '
        'comboBackDropResolutions
        '
        Me.comboBackDropResolutions.FormattingEnabled = true
        Me.comboBackDropResolutions.Location = New System.Drawing.Point(98, 87)
        Me.comboBackDropResolutions.Name = "comboBackDropResolutions"
        Me.comboBackDropResolutions.Size = New System.Drawing.Size(160, 23)
        Me.comboBackDropResolutions.TabIndex = 50
        '
        'comboPosterResolutions
        '
        Me.comboPosterResolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboPosterResolutions.FormattingEnabled = true
        Me.comboPosterResolutions.Location = New System.Drawing.Point(98, 58)
        Me.comboPosterResolutions.Name = "comboPosterResolutions"
        Me.comboPosterResolutions.Size = New System.Drawing.Size(160, 23)
        Me.comboPosterResolutions.TabIndex = 49
        '
        'grpCleanFilename
        '
        Me.grpCleanFilename.Controls.Add(Me.btnCleanFilenameRemove)
        Me.grpCleanFilename.Controls.Add(Me.txtCleanFilenameAdd)
        Me.grpCleanFilename.Controls.Add(Me.btnCleanFilenameAdd)
        Me.grpCleanFilename.Controls.Add(Me.lbCleanFilename)
        Me.grpCleanFilename.Location = New System.Drawing.Point(584, 6)
        Me.grpCleanFilename.Name = "grpCleanFilename"
        Me.grpCleanFilename.Size = New System.Drawing.Size(199, 380)
        Me.grpCleanFilename.TabIndex = 100
        Me.grpCleanFilename.TabStop = false
        Me.grpCleanFilename.Text = "Clean Filename Settings"
        '
        'btnCleanFilenameRemove
        '
        Me.btnCleanFilenameRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnCleanFilenameRemove.Location = New System.Drawing.Point(91, 318)
        Me.btnCleanFilenameRemove.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCleanFilenameRemove.Name = "btnCleanFilenameRemove"
        Me.btnCleanFilenameRemove.Size = New System.Drawing.Size(100, 21)
        Me.btnCleanFilenameRemove.TabIndex = 8
        Me.btnCleanFilenameRemove.Text = "Remove Selected"
        Me.btnCleanFilenameRemove.UseVisualStyleBackColor = true
        '
        'txtCleanFilenameAdd
        '
        Me.txtCleanFilenameAdd.Location = New System.Drawing.Point(6, 347)
        Me.txtCleanFilenameAdd.Name = "txtCleanFilenameAdd"
        Me.txtCleanFilenameAdd.Size = New System.Drawing.Size(146, 21)
        Me.txtCleanFilenameAdd.TabIndex = 7
        '
        'btnCleanFilenameAdd
        '
        Me.btnCleanFilenameAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnCleanFilenameAdd.Location = New System.Drawing.Point(155, 347)
        Me.btnCleanFilenameAdd.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCleanFilenameAdd.Name = "btnCleanFilenameAdd"
        Me.btnCleanFilenameAdd.Size = New System.Drawing.Size(36, 21)
        Me.btnCleanFilenameAdd.TabIndex = 6
        Me.btnCleanFilenameAdd.Text = "Add"
        Me.btnCleanFilenameAdd.UseVisualStyleBackColor = true
        '
        'lbCleanFilename
        '
        Me.lbCleanFilename.FormattingEnabled = true
        Me.lbCleanFilename.ItemHeight = 15
        Me.lbCleanFilename.Location = New System.Drawing.Point(6, 20)
        Me.lbCleanFilename.Name = "lbCleanFilename"
        Me.lbCleanFilename.Size = New System.Drawing.Size(185, 289)
        Me.lbCleanFilename.TabIndex = 0
        '
        'grpVideoSource
        '
        Me.grpVideoSource.Controls.Add(Me.btnVideoSourceRemove)
        Me.grpVideoSource.Controls.Add(Me.txtVideoSourceAdd)
        Me.grpVideoSource.Controls.Add(Me.btnVideoSourceAdd)
        Me.grpVideoSource.Controls.Add(Me.lbVideoSource)
        Me.grpVideoSource.Location = New System.Drawing.Point(789, 6)
        Me.grpVideoSource.Name = "grpVideoSource"
        Me.grpVideoSource.Size = New System.Drawing.Size(197, 482)
        Me.grpVideoSource.TabIndex = 99
        Me.grpVideoSource.TabStop = false
        Me.grpVideoSource.Text = "Video Source"
        '
        'btnVideoSourceRemove
        '
        Me.btnVideoSourceRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnVideoSourceRemove.Location = New System.Drawing.Point(91, 428)
        Me.btnVideoSourceRemove.Margin = New System.Windows.Forms.Padding(0)
        Me.btnVideoSourceRemove.Name = "btnVideoSourceRemove"
        Me.btnVideoSourceRemove.Size = New System.Drawing.Size(100, 21)
        Me.btnVideoSourceRemove.TabIndex = 4
        Me.btnVideoSourceRemove.Text = "Remove Selected"
        Me.btnVideoSourceRemove.UseVisualStyleBackColor = true
        '
        'txtVideoSourceAdd
        '
        Me.txtVideoSourceAdd.Location = New System.Drawing.Point(6, 453)
        Me.txtVideoSourceAdd.Name = "txtVideoSourceAdd"
        Me.txtVideoSourceAdd.Size = New System.Drawing.Size(145, 21)
        Me.txtVideoSourceAdd.TabIndex = 3
        '
        'btnVideoSourceAdd
        '
        Me.btnVideoSourceAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnVideoSourceAdd.Location = New System.Drawing.Point(155, 452)
        Me.btnVideoSourceAdd.Margin = New System.Windows.Forms.Padding(0)
        Me.btnVideoSourceAdd.Name = "btnVideoSourceAdd"
        Me.btnVideoSourceAdd.Size = New System.Drawing.Size(36, 21)
        Me.btnVideoSourceAdd.TabIndex = 2
        Me.btnVideoSourceAdd.Text = "Add"
        Me.btnVideoSourceAdd.UseVisualStyleBackColor = true
        '
        'lbVideoSource
        '
        Me.lbVideoSource.FormattingEnabled = true
        Me.lbVideoSource.ItemHeight = 15
        Me.lbVideoSource.Location = New System.Drawing.Point(6, 17)
        Me.lbVideoSource.Margin = New System.Windows.Forms.Padding(0)
        Me.lbVideoSource.Name = "lbVideoSource"
        Me.lbVideoSource.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lbVideoSource.Size = New System.Drawing.Size(185, 394)
        Me.lbVideoSource.TabIndex = 0
        '
        'cbOverwriteArtwork
        '
        Me.cbOverwriteArtwork.AutoSize = true
        Me.cbOverwriteArtwork.Location = New System.Drawing.Point(12, 367)
        Me.cbOverwriteArtwork.Name = "cbOverwriteArtwork"
        Me.cbOverwriteArtwork.Size = New System.Drawing.Size(195, 19)
        Me.cbOverwriteArtwork.TabIndex = 90
        Me.cbOverwriteArtwork.Text = "Don’t overwrite existing artwork"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        Me.cbOverwriteArtwork.UseVisualStyleBackColor = true
        '
        'cbRuntimeAsNumericOnly
        '
        Me.cbRuntimeAsNumericOnly.AutoSize = true
        Me.cbRuntimeAsNumericOnly.Location = New System.Drawing.Point(12, 393)
        Me.cbRuntimeAsNumericOnly.Margin = New System.Windows.Forms.Padding(4)
        Me.cbRuntimeAsNumericOnly.Name = "cbRuntimeAsNumericOnly"
        Me.cbRuntimeAsNumericOnly.Size = New System.Drawing.Size(235, 19)
        Me.cbRuntimeAsNumericOnly.TabIndex = 88
        Me.cbRuntimeAsNumericOnly.Text = "Save media runtime as numerical only"
        Me.cbRuntimeAsNumericOnly.UseVisualStyleBackColor = true
        '
        'gbxXBMCversion
        '
        Me.gbxXBMCversion.Controls.Add(Me.Label129)
        Me.gbxXBMCversion.Controls.Add(Me.rbXBMCv_both)
        Me.gbxXBMCversion.Controls.Add(Me.rbXBMCv_post)
        Me.gbxXBMCversion.Controls.Add(Me.rbXBMCv_pre)
        Me.gbxXBMCversion.Location = New System.Drawing.Point(12, 35)
        Me.gbxXBMCversion.Name = "gbxXBMCversion"
        Me.gbxXBMCversion.Size = New System.Drawing.Size(278, 131)
        Me.gbxXBMCversion.TabIndex = 48
        Me.gbxXBMCversion.TabStop = false
        Me.gbxXBMCversion.Text = "Artwork Version"
        '
        'Label129
        '
        Me.Label129.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label129.Location = New System.Drawing.Point(6, 17)
        Me.Label129.Name = "Label129"
        Me.Label129.Size = New System.Drawing.Size(266, 47)
        Me.Label129.TabIndex = 1
        Me.Label129.Text = "From Version Frodo, Artwork is now saved as jpg, not tbn.  Choose an option below"& _ 
    " which best defines your setup."
        '
        'rbXBMCv_both
        '
        Me.rbXBMCv_both.AutoSize = true
        Me.rbXBMCv_both.Location = New System.Drawing.Point(6, 107)
        Me.rbXBMCv_both.Name = "rbXBMCv_both"
        Me.rbXBMCv_both.Size = New System.Drawing.Size(50, 19)
        Me.rbXBMCv_both.TabIndex = 0
        Me.rbXBMCv_both.TabStop = true
        Me.rbXBMCv_both.Text = "Both"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        Me.rbXBMCv_both.UseVisualStyleBackColor = true
        '
        'rbXBMCv_post
        '
        Me.rbXBMCv_post.AutoSize = true
        Me.rbXBMCv_post.Checked = true
        Me.rbXBMCv_post.Location = New System.Drawing.Point(6, 87)
        Me.rbXBMCv_post.Name = "rbXBMCv_post"
        Me.rbXBMCv_post.Size = New System.Drawing.Size(179, 19)
        Me.rbXBMCv_post.TabIndex = 0
        Me.rbXBMCv_post.TabStop = true
        Me.rbXBMCv_post.Text = "Frodo and onwards (default)"
        Me.rbXBMCv_post.UseVisualStyleBackColor = true
        '
        'rbXBMCv_pre
        '
        Me.rbXBMCv_pre.AutoSize = true
        Me.rbXBMCv_pre.Location = New System.Drawing.Point(6, 67)
        Me.rbXBMCv_pre.Name = "rbXBMCv_pre"
        Me.rbXBMCv_pre.Size = New System.Drawing.Size(80, 19)
        Me.rbXBMCv_pre.TabIndex = 0
        Me.rbXBMCv_pre.Text = "Pre-Frodo"
        Me.rbXBMCv_pre.UseVisualStyleBackColor = true
        '
        'TPActors
        '
        Me.TPActors.Controls.Add(Me.Label21)
        Me.TPActors.Controls.Add(Me.GroupBox2)
        Me.TPActors.Controls.Add(Me.GroupBox12)
        Me.TPActors.Controls.Add(Me.GroupBox32)
        Me.TPActors.Location = New System.Drawing.Point(4, 24)
        Me.TPActors.Name = "TPActors"
        Me.TPActors.Padding = New System.Windows.Forms.Padding(3)
        Me.TPActors.Size = New System.Drawing.Size(992, 567)
        Me.TPActors.TabIndex = 1
        Me.TPActors.Text = "Actor(s)"
        Me.TPActors.UseVisualStyleBackColor = true
        '
        'Label21
        '
        Me.Label21.AutoSize = true
        Me.Label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label21.Location = New System.Drawing.Point(13, 4)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(358, 26)
        Me.Label21.TabIndex = 103
        Me.Label21.Text = "Common Actor Settings, Movie && TV."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label98)
        Me.GroupBox2.Controls.Add(Me.cmbx_MovMaxActors)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 145)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(360, 92)
        Me.GroupBox2.TabIndex = 65
        Me.GroupBox2.TabStop = false
        Me.GroupBox2.Text = "Quantity of Actor's Downloaded."
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(7, 17)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(342, 30)
        Me.Label6.TabIndex = 39
        Me.Label6.Text = "Select the Maximum numbers of Actors for Movies and TV Series."
        '
        'Label98
        '
        Me.Label98.AutoSize = true
        Me.Label98.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label98.Location = New System.Drawing.Point(9, 56)
        Me.Label98.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(161, 15)
        Me.Label98.TabIndex = 63
        Me.Label98.Text = "Maximum number of actors:"
        '
        'GroupBox32
        '
        Me.GroupBox32.Controls.Add(Me.cb_ExcludeActorNoThumb)
        Me.GroupBox32.Controls.Add(Me.Label137)
        Me.GroupBox32.Controls.Add(Me.cb_actorseasy)
        Me.GroupBox32.Location = New System.Drawing.Point(12, 33)
        Me.GroupBox32.Name = "GroupBox32"
        Me.GroupBox32.Size = New System.Drawing.Size(406, 106)
        Me.GroupBox32.TabIndex = 48
        Me.GroupBox32.TabStop = false
        Me.GroupBox32.Text = "Actor Folder"
        '
        'cb_ExcludeActorNoThumb
        '
        Me.cb_ExcludeActorNoThumb.AutoSize = true
        Me.cb_ExcludeActorNoThumb.Location = New System.Drawing.Point(21, 75)
        Me.cb_ExcludeActorNoThumb.Name = "cb_ExcludeActorNoThumb"
        Me.cb_ExcludeActorNoThumb.Size = New System.Drawing.Size(219, 19)
        Me.cb_ExcludeActorNoThumb.TabIndex = 39
        Me.cb_ExcludeActorNoThumb.Text = "Exclude Actors without Thumbnails."
        Me.cb_ExcludeActorNoThumb.UseVisualStyleBackColor = true
        '
        'Label137
        '
        Me.Label137.Location = New System.Drawing.Point(7, 17)
        Me.Label137.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(392, 30)
        Me.Label137.TabIndex = 38
        Me.Label137.Text = "Media Companion has the function to scrape actor thumbnails to a folder named '.a"& _ 
    "ctors' (located in the same directory as the movie)."
        '
        'cb_actorseasy
        '
        Me.cb_actorseasy.AutoSize = true
        Me.cb_actorseasy.Location = New System.Drawing.Point(7, 51)
        Me.cb_actorseasy.Margin = New System.Windows.Forms.Padding(4)
        Me.cb_actorseasy.Name = "cb_actorseasy"
        Me.cb_actorseasy.Size = New System.Drawing.Size(247, 19)
        Me.cb_actorseasy.TabIndex = 37
        Me.cb_actorseasy.Text = "Save Actor Thumbs to the '.Actors' Folder"
        Me.cb_actorseasy.UseVisualStyleBackColor = true
        '
        'TPMovPref
        '
        Me.TPMovPref.Controls.Add(Me.tcMoviePreferences)
        Me.TPMovPref.Location = New System.Drawing.Point(4, 24)
        Me.TPMovPref.Name = "TPMovPref"
        Me.TPMovPref.Size = New System.Drawing.Size(1000, 595)
        Me.TPMovPref.TabIndex = 7
        Me.TPMovPref.Text = "Movie Preferences"
        Me.TPMovPref.UseVisualStyleBackColor = true
        '
        'tcMoviePreferences
        '
        Me.tcMoviePreferences.Controls.Add(Me.tpMoviePreferences_Scraper)
        Me.tcMoviePreferences.Controls.Add(Me.tpMoviePreferences_Artwork)
        Me.tcMoviePreferences.Controls.Add(Me.tpMoviePreferences_General)
        Me.tcMoviePreferences.Controls.Add(Me.tpMoviePreferences_Advanced)
        Me.tcMoviePreferences.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMoviePreferences.Location = New System.Drawing.Point(0, 0)
        Me.tcMoviePreferences.Margin = New System.Windows.Forms.Padding(4)
        Me.tcMoviePreferences.Name = "tcMoviePreferences"
        Me.tcMoviePreferences.SelectedIndex = 0
        Me.tcMoviePreferences.Size = New System.Drawing.Size(1000, 595)
        Me.tcMoviePreferences.TabIndex = 51
        '
        'tpMoviePreferences_Scraper
        '
        Me.tpMoviePreferences_Scraper.AutoScroll = true
        Me.tpMoviePreferences_Scraper.AutoScrollMinSize = New System.Drawing.Size(928, 370)
        Me.tpMoviePreferences_Scraper.BackColor = System.Drawing.SystemColors.Control
        Me.tpMoviePreferences_Scraper.Controls.Add(Me.grpbxTMDbApi)
        Me.tpMoviePreferences_Scraper.Controls.Add(Me.grpbxMovieScraperSettings)
        Me.tpMoviePreferences_Scraper.Controls.Add(Me.gpbxPrefScraperImages)
        Me.tpMoviePreferences_Scraper.Controls.Add(Me.grpbxMovieFolderOptions)
        Me.tpMoviePreferences_Scraper.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tpMoviePreferences_Scraper.Location = New System.Drawing.Point(4, 24)
        Me.tpMoviePreferences_Scraper.Margin = New System.Windows.Forms.Padding(4)
        Me.tpMoviePreferences_Scraper.Name = "tpMoviePreferences_Scraper"
        Me.tpMoviePreferences_Scraper.Padding = New System.Windows.Forms.Padding(4)
        Me.tpMoviePreferences_Scraper.Size = New System.Drawing.Size(992, 567)
        Me.tpMoviePreferences_Scraper.TabIndex = 0
        Me.tpMoviePreferences_Scraper.Text = "Scraper"
        '
        'grpbxTMDbApi
        '
        Me.grpbxTMDbApi.Controls.Add(Me.Label22)
        Me.grpbxTMDbApi.Controls.Add(Me.tbTMDbAPI)
        Me.grpbxTMDbApi.Location = New System.Drawing.Point(7, 442)
        Me.grpbxTMDbApi.Name = "grpbxTMDbApi"
        Me.grpbxTMDbApi.Size = New System.Drawing.Size(319, 89)
        Me.grpbxTMDbApi.TabIndex = 79
        Me.grpbxTMDbApi.TabStop = false
        Me.grpbxTMDbApi.Text = "TMDb API"
        '
        'Label22
        '
        Me.Label22.AutoSize = true
        Me.Label22.Location = New System.Drawing.Point(23, 21)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(269, 30)
        Me.Label22.TabIndex = 1
        Me.Label22.Text = "Users can save their own TMDb API key here."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"NB: TMDB XBMC Scraper uses it's own "& _ 
    "API Key."
        '
        'tbTMDbAPI
        '
        Me.tbTMDbAPI.Location = New System.Drawing.Point(7, 62)
        Me.tbTMDbAPI.Name = "tbTMDbAPI"
        Me.tbTMDbAPI.Size = New System.Drawing.Size(293, 21)
        Me.tbTMDbAPI.TabIndex = 0
        '
        'grpbxMovieScraperSettings
        '
        Me.grpbxMovieScraperSettings.Controls.Add(Me.grpbxTMDBScraperPreferences)
        Me.grpbxMovieScraperSettings.Controls.Add(Me.cbMovieUseXBMCScraper)
        Me.grpbxMovieScraperSettings.Controls.Add(Me.GpBx_McIMDbScraperSettings)
        Me.grpbxMovieScraperSettings.Location = New System.Drawing.Point(7, 6)
        Me.grpbxMovieScraperSettings.Name = "grpbxMovieScraperSettings"
        Me.grpbxMovieScraperSettings.Size = New System.Drawing.Size(319, 358)
        Me.grpbxMovieScraperSettings.TabIndex = 72
        Me.grpbxMovieScraperSettings.TabStop = false
        Me.grpbxMovieScraperSettings.Text = "Choose Default Scraper"
        '
        'grpbxTMDBScraperPreferences
        '
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cbMovActorFallbackIMDbtoTMDb)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cmbxTMDBPreferredCertCountry)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.Label5)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.grpbxTmdbfromImdbOptions)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cbXbmcTmdbActorFromImdb)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cbXbmcTmdbRename)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.Label155)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cmbxXbmcTmdbTitleLanguage)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cbXbmcTmdbFanart)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.cmbxXbmcTmdbHDTrailer)
        Me.grpbxTMDBScraperPreferences.Controls.Add(Me.Label153)
        Me.grpbxTMDBScraperPreferences.Location = New System.Drawing.Point(10, 40)
        Me.grpbxTMDBScraperPreferences.Margin = New System.Windows.Forms.Padding(4)
        Me.grpbxTMDBScraperPreferences.Name = "grpbxTMDBScraperPreferences"
        Me.grpbxTMDBScraperPreferences.Padding = New System.Windows.Forms.Padding(4)
        Me.grpbxTMDBScraperPreferences.Size = New System.Drawing.Size(300, 310)
        Me.grpbxTMDBScraperPreferences.TabIndex = 68
        Me.grpbxTMDBScraperPreferences.TabStop = false
        Me.grpbxTMDBScraperPreferences.Text = "XBMC Scraper Preferences - TMDB"
        '
        'cbMovActorFallbackIMDbtoTMDb
        '
        Me.cbMovActorFallbackIMDbtoTMDb.AutoSize = true
        Me.cbMovActorFallbackIMDbtoTMDb.Font = New System.Drawing.Font("Microsoft Sans Serif", 7!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovActorFallbackIMDbtoTMDb.Location = New System.Drawing.Point(21, 287)
        Me.cbMovActorFallbackIMDbtoTMDb.Name = "cbMovActorFallbackIMDbtoTMDb"
        Me.cbMovActorFallbackIMDbtoTMDb.Size = New System.Drawing.Size(110, 17)
        Me.cbMovActorFallbackIMDbtoTMDb.TabIndex = 85
        Me.cbMovActorFallbackIMDbtoTMDb.Text = "(fallback to TMDb)"
        Me.cbMovActorFallbackIMDbtoTMDb.UseVisualStyleBackColor = true
        '
        'cmbxTMDBPreferredCertCountry
        '
        Me.cmbxTMDBPreferredCertCountry.FormattingEnabled = true
        Me.cmbxTMDBPreferredCertCountry.Location = New System.Drawing.Point(216, 221)
        Me.cmbxTMDBPreferredCertCountry.Name = "cmbxTMDBPreferredCertCountry"
        Me.cmbxTMDBPreferredCertCountry.Size = New System.Drawing.Size(74, 23)
        Me.cmbxTMDBPreferredCertCountry.TabIndex = 84
        '
        'Label5
        '
        Me.Label5.AutoSize = true
        Me.Label5.Location = New System.Drawing.Point(9, 225)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(159, 15)
        Me.Label5.TabIndex = 83
        Me.Label5.Text = "Preferred Country Certificate"
        '
        'grpbxTmdbfromImdbOptions
        '
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbMetascoreFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbAspectFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbGenreFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbAkasFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbCertFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbVotesFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbTop250FromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbIMDBRatings)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbStarsFromImdb)
        Me.grpbxTmdbfromImdbOptions.Controls.Add(Me.cbXbmcTmdbOutlineFromImdb)
        Me.grpbxTmdbfromImdbOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.grpbxTmdbfromImdbOptions.Location = New System.Drawing.Point(11, 36)
        Me.grpbxTmdbfromImdbOptions.Name = "grpbxTmdbfromImdbOptions"
        Me.grpbxTmdbfromImdbOptions.Size = New System.Drawing.Size(244, 126)
        Me.grpbxTmdbfromImdbOptions.TabIndex = 82
        Me.grpbxTmdbfromImdbOptions.TabStop = false
        Me.grpbxTmdbfromImdbOptions.Text = "Scrape the following from IMDB"
        '
        'cbXbmcTmdbMetascoreFromImdb
        '
        Me.cbXbmcTmdbMetascoreFromImdb.AutoSize = true
        Me.cbXbmcTmdbMetascoreFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbMetascoreFromImdb.Location = New System.Drawing.Point(116, 102)
        Me.cbXbmcTmdbMetascoreFromImdb.Name = "cbXbmcTmdbMetascoreFromImdb"
        Me.cbXbmcTmdbMetascoreFromImdb.Size = New System.Drawing.Size(84, 19)
        Me.cbXbmcTmdbMetascoreFromImdb.TabIndex = 87
        Me.cbXbmcTmdbMetascoreFromImdb.Text = "Metascore"
        Me.cbXbmcTmdbMetascoreFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbAspectFromImdb
        '
        Me.cbXbmcTmdbAspectFromImdb.AutoSize = true
        Me.cbXbmcTmdbAspectFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbAspectFromImdb.Location = New System.Drawing.Point(22, 102)
        Me.cbXbmcTmdbAspectFromImdb.Name = "cbXbmcTmdbAspectFromImdb"
        Me.cbXbmcTmdbAspectFromImdb.Size = New System.Drawing.Size(91, 19)
        Me.cbXbmcTmdbAspectFromImdb.TabIndex = 86
        Me.cbXbmcTmdbAspectFromImdb.Text = "AspectRatio"
        Me.cbXbmcTmdbAspectFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbGenreFromImdb
        '
        Me.cbXbmcTmdbGenreFromImdb.AutoSize = true
        Me.cbXbmcTmdbGenreFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbGenreFromImdb.Location = New System.Drawing.Point(117, 82)
        Me.cbXbmcTmdbGenreFromImdb.Name = "cbXbmcTmdbGenreFromImdb"
        Me.cbXbmcTmdbGenreFromImdb.Size = New System.Drawing.Size(66, 19)
        Me.cbXbmcTmdbGenreFromImdb.TabIndex = 85
        Me.cbXbmcTmdbGenreFromImdb.Text = "Genres"
        Me.cbXbmcTmdbGenreFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbAkasFromImdb
        '
        Me.cbXbmcTmdbAkasFromImdb.AutoSize = true
        Me.cbXbmcTmdbAkasFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbAkasFromImdb.Location = New System.Drawing.Point(22, 82)
        Me.cbXbmcTmdbAkasFromImdb.Name = "cbXbmcTmdbAkasFromImdb"
        Me.cbXbmcTmdbAkasFromImdb.Size = New System.Drawing.Size(78, 19)
        Me.cbXbmcTmdbAkasFromImdb.TabIndex = 82
        Me.cbXbmcTmdbAkasFromImdb.Text = "Aka Titles"
        Me.cbXbmcTmdbAkasFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbCertFromImdb
        '
        Me.cbXbmcTmdbCertFromImdb.AutoSize = true
        Me.cbXbmcTmdbCertFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbCertFromImdb.Location = New System.Drawing.Point(117, 42)
        Me.cbXbmcTmdbCertFromImdb.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXbmcTmdbCertFromImdb.Name = "cbXbmcTmdbCertFromImdb"
        Me.cbXbmcTmdbCertFromImdb.Size = New System.Drawing.Size(80, 19)
        Me.cbXbmcTmdbCertFromImdb.TabIndex = 81
        Me.cbXbmcTmdbCertFromImdb.Text = "Certificate"
        Me.cbXbmcTmdbCertFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbVotesFromImdb
        '
        Me.cbXbmcTmdbVotesFromImdb.AutoSize = true
        Me.cbXbmcTmdbVotesFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbVotesFromImdb.Location = New System.Drawing.Point(117, 62)
        Me.cbXbmcTmdbVotesFromImdb.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXbmcTmdbVotesFromImdb.Name = "cbXbmcTmdbVotesFromImdb"
        Me.cbXbmcTmdbVotesFromImdb.Size = New System.Drawing.Size(56, 19)
        Me.cbXbmcTmdbVotesFromImdb.TabIndex = 78
        Me.cbXbmcTmdbVotesFromImdb.Text = "Votes"
        Me.cbXbmcTmdbVotesFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbTop250FromImdb
        '
        Me.cbXbmcTmdbTop250FromImdb.AutoSize = true
        Me.cbXbmcTmdbTop250FromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbTop250FromImdb.Location = New System.Drawing.Point(22, 42)
        Me.cbXbmcTmdbTop250FromImdb.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXbmcTmdbTop250FromImdb.Name = "cbXbmcTmdbTop250FromImdb"
        Me.cbXbmcTmdbTop250FromImdb.Size = New System.Drawing.Size(68, 19)
        Me.cbXbmcTmdbTop250FromImdb.TabIndex = 79
        Me.cbXbmcTmdbTop250FromImdb.Text = "Top250"
        Me.cbXbmcTmdbTop250FromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbIMDBRatings
        '
        Me.cbXbmcTmdbIMDBRatings.AutoSize = true
        Me.cbXbmcTmdbIMDBRatings.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbIMDBRatings.Location = New System.Drawing.Point(22, 62)
        Me.cbXbmcTmdbIMDBRatings.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXbmcTmdbIMDBRatings.Name = "cbXbmcTmdbIMDBRatings"
        Me.cbXbmcTmdbIMDBRatings.Size = New System.Drawing.Size(62, 19)
        Me.cbXbmcTmdbIMDBRatings.TabIndex = 71
        Me.cbXbmcTmdbIMDBRatings.Text = "Rating"
        Me.cbXbmcTmdbIMDBRatings.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbStarsFromImdb
        '
        Me.cbXbmcTmdbStarsFromImdb.AutoSize = true
        Me.cbXbmcTmdbStarsFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbStarsFromImdb.Location = New System.Drawing.Point(117, 22)
        Me.cbXbmcTmdbStarsFromImdb.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXbmcTmdbStarsFromImdb.Name = "cbXbmcTmdbStarsFromImdb"
        Me.cbXbmcTmdbStarsFromImdb.Size = New System.Drawing.Size(54, 19)
        Me.cbXbmcTmdbStarsFromImdb.TabIndex = 80
        Me.cbXbmcTmdbStarsFromImdb.Text = "Stars"
        Me.cbXbmcTmdbStarsFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbOutlineFromImdb
        '
        Me.cbXbmcTmdbOutlineFromImdb.AutoSize = true
        Me.cbXbmcTmdbOutlineFromImdb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXbmcTmdbOutlineFromImdb.Location = New System.Drawing.Point(22, 22)
        Me.cbXbmcTmdbOutlineFromImdb.Name = "cbXbmcTmdbOutlineFromImdb"
        Me.cbXbmcTmdbOutlineFromImdb.Size = New System.Drawing.Size(65, 19)
        Me.cbXbmcTmdbOutlineFromImdb.TabIndex = 77
        Me.cbXbmcTmdbOutlineFromImdb.Text = "Outline"
        Me.cbXbmcTmdbOutlineFromImdb.UseVisualStyleBackColor = true
        '
        'cbXbmcTmdbRename
        '
        Me.cbXbmcTmdbRename.AutoSize = true
        Me.cbXbmcTmdbRename.Location = New System.Drawing.Point(10, 250)
        Me.cbXbmcTmdbRename.Name = "cbXbmcTmdbRename"
        Me.cbXbmcTmdbRename.Size = New System.Drawing.Size(251, 19)
        Me.cbXbmcTmdbRename.TabIndex = 73
        Me.cbXbmcTmdbRename.Text = "Enable Renaming of XBMC TMDB movie"
        Me.cbXbmcTmdbRename.UseVisualStyleBackColor = true
        '
        'Label155
        '
        Me.Label155.AutoSize = true
        Me.Label155.Location = New System.Drawing.Point(8, 173)
        Me.Label155.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label155.Name = "Label155"
        Me.Label155.Size = New System.Drawing.Size(200, 15)
        Me.Label155.TabIndex = 72
        Me.Label155.Text = "Enable trailers from HD-Trailers.net"
        '
        'cmbxXbmcTmdbTitleLanguage
        '
        Me.cmbxXbmcTmdbTitleLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxXbmcTmdbTitleLanguage.FormattingEnabled = true
        Me.cmbxXbmcTmdbTitleLanguage.Location = New System.Drawing.Point(165, 195)
        Me.cmbxXbmcTmdbTitleLanguage.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbxXbmcTmdbTitleLanguage.Name = "cmbxXbmcTmdbTitleLanguage"
        Me.cmbxXbmcTmdbTitleLanguage.Size = New System.Drawing.Size(125, 23)
        Me.cmbxXbmcTmdbTitleLanguage.Sorted = true
        Me.cmbxXbmcTmdbTitleLanguage.TabIndex = 70
        '
        'cbXbmcTmdbFanart
        '
        Me.cbXbmcTmdbFanart.AutoSize = true
        Me.cbXbmcTmdbFanart.Location = New System.Drawing.Point(11, 17)
        Me.cbXbmcTmdbFanart.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXbmcTmdbFanart.Name = "cbXbmcTmdbFanart"
        Me.cbXbmcTmdbFanart.Size = New System.Drawing.Size(99, 19)
        Me.cbXbmcTmdbFanart.TabIndex = 67
        Me.cbXbmcTmdbFanart.Text = "Enable fanart"
        Me.cbXbmcTmdbFanart.UseVisualStyleBackColor = true
        '
        'cmbxXbmcTmdbHDTrailer
        '
        Me.cmbxXbmcTmdbHDTrailer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxXbmcTmdbHDTrailer.FormattingEnabled = true
        Me.cmbxXbmcTmdbHDTrailer.Location = New System.Drawing.Point(216, 170)
        Me.cmbxXbmcTmdbHDTrailer.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbxXbmcTmdbHDTrailer.Name = "cmbxXbmcTmdbHDTrailer"
        Me.cmbxXbmcTmdbHDTrailer.Size = New System.Drawing.Size(74, 23)
        Me.cmbxXbmcTmdbHDTrailer.Sorted = true
        Me.cmbxXbmcTmdbHDTrailer.TabIndex = 6
        '
        'Label153
        '
        Me.Label153.AutoSize = true
        Me.Label153.Location = New System.Drawing.Point(9, 196)
        Me.Label153.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label153.Name = "Label153"
        Me.Label153.Size = New System.Drawing.Size(113, 15)
        Me.Label153.TabIndex = 69
        Me.Label153.Text = "Preferred language"
        '
        'cbMovieUseXBMCScraper
        '
        Me.cbMovieUseXBMCScraper.AutoSize = true
        Me.cbMovieUseXBMCScraper.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieUseXBMCScraper.Location = New System.Drawing.Point(5, 18)
        Me.cbMovieUseXBMCScraper.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieUseXBMCScraper.Name = "cbMovieUseXBMCScraper"
        Me.cbMovieUseXBMCScraper.Size = New System.Drawing.Size(170, 19)
        Me.cbMovieUseXBMCScraper.TabIndex = 65
        Me.cbMovieUseXBMCScraper.Text = "Use TMDB XBMC Scraper"
        Me.cbMovieUseXBMCScraper.UseVisualStyleBackColor = true
        '
        'GpBx_McIMDbScraperSettings
        '
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.cbMovActorFallbackTMDbtoIMDb)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.cbMovImdbAspectRatio)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.cbMovImdbFirstRunTime)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.cbImdbPrimaryPlot)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.cbImdbgetTMDBActor)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.Label90)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.Label91)
        Me.GpBx_McIMDbScraperSettings.Controls.Add(Me.lb_IMDBMirrors)
        Me.GpBx_McIMDbScraperSettings.Location = New System.Drawing.Point(10, 40)
        Me.GpBx_McIMDbScraperSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.GpBx_McIMDbScraperSettings.Name = "GpBx_McIMDbScraperSettings"
        Me.GpBx_McIMDbScraperSettings.Padding = New System.Windows.Forms.Padding(4)
        Me.GpBx_McIMDbScraperSettings.Size = New System.Drawing.Size(300, 310)
        Me.GpBx_McIMDbScraperSettings.TabIndex = 40
        Me.GpBx_McIMDbScraperSettings.TabStop = false
        Me.GpBx_McIMDbScraperSettings.Text = "MC's IMDB Scraper"
        '
        'cbMovActorFallbackTMDbtoIMDb
        '
        Me.cbMovActorFallbackTMDbtoIMDb.AutoSize = true
        Me.cbMovActorFallbackTMDbtoIMDb.Font = New System.Drawing.Font("Microsoft Sans Serif", 7!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovActorFallbackTMDbtoIMDb.Location = New System.Drawing.Point(20, 181)
        Me.cbMovActorFallbackTMDbtoIMDb.Name = "cbMovActorFallbackTMDbtoIMDb"
        Me.cbMovActorFallbackTMDbtoIMDb.Size = New System.Drawing.Size(216, 17)
        Me.cbMovActorFallbackTMDbtoIMDb.TabIndex = 8
        Me.cbMovActorFallbackTMDbtoIMDb.Text = "Fall-back to IMDB if no images available."
        Me.cbMovActorFallbackTMDbtoIMDb.UseVisualStyleBackColor = true
        '
        'cbMovImdbAspectRatio
        '
        Me.cbMovImdbAspectRatio.AutoSize = true
        Me.cbMovImdbAspectRatio.Location = New System.Drawing.Point(10, 252)
        Me.cbMovImdbAspectRatio.Name = "cbMovImdbAspectRatio"
        Me.cbMovImdbAspectRatio.Size = New System.Drawing.Size(180, 19)
        Me.cbMovImdbAspectRatio.TabIndex = 7
        Me.cbMovImdbAspectRatio.Text = "Use Aspect Ratio from IMDb"
        Me.cbMovImdbAspectRatio.UseVisualStyleBackColor = true
        '
        'cbMovImdbFirstRunTime
        '
        Me.cbMovImdbFirstRunTime.AutoSize = true
        Me.cbMovImdbFirstRunTime.Location = New System.Drawing.Point(10, 225)
        Me.cbMovImdbFirstRunTime.Name = "cbMovImdbFirstRunTime"
        Me.cbMovImdbFirstRunTime.Size = New System.Drawing.Size(241, 19)
        Me.cbMovImdbFirstRunTime.TabIndex = 6
        Me.cbMovImdbFirstRunTime.Text = "Select only first found Runtime on IMDb"
        Me.cbMovImdbFirstRunTime.UseVisualStyleBackColor = true
        '
        'cbImdbPrimaryPlot
        '
        Me.cbImdbPrimaryPlot.AutoSize = true
        Me.cbImdbPrimaryPlot.Location = New System.Drawing.Point(10, 200)
        Me.cbImdbPrimaryPlot.Name = "cbImdbPrimaryPlot"
        Me.cbImdbPrimaryPlot.Size = New System.Drawing.Size(154, 19)
        Me.cbImdbPrimaryPlot.TabIndex = 5
        Me.cbImdbPrimaryPlot.Text = "Select only Primary Plot"
        Me.cbImdbPrimaryPlot.UseVisualStyleBackColor = true
        '
        'cbImdbgetTMDBActor
        '
        Me.cbImdbgetTMDBActor.AutoSize = true
        Me.cbImdbgetTMDBActor.Location = New System.Drawing.Point(9, 163)
        Me.cbImdbgetTMDBActor.Name = "cbImdbgetTMDBActor"
        Me.cbImdbgetTMDBActor.Size = New System.Drawing.Size(257, 19)
        Me.cbImdbgetTMDBActor.TabIndex = 3
        Me.cbImdbgetTMDBActor.Text = "Scrape actors from TMDB instead of IMDB"
        Me.cbImdbgetTMDBActor.UseVisualStyleBackColor = true
        '
        'Label90
        '
        Me.Label90.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label90.Location = New System.Drawing.Point(11, 132)
        Me.Label90.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(279, 32)
        Me.Label90.TabIndex = 2
        Me.Label90.Text = "Using ""www.imdb.de"" will result in an incomplete nfo file since this mirror doesn"& _ 
    "'t contain plot, tagline, or trailer nfo."
        '
        'Label91
        '
        Me.Label91.AutoSize = true
        Me.Label91.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label91.Location = New System.Drawing.Point(6, 16)
        Me.Label91.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(190, 15)
        Me.Label91.TabIndex = 1
        Me.Label91.Text = "Select your preferred IMDB Mirror"
        '
        'lb_IMDBMirrors
        '
        Me.lb_IMDBMirrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lb_IMDBMirrors.FormattingEnabled = true
        Me.lb_IMDBMirrors.ItemHeight = 15
        Me.lb_IMDBMirrors.Items.AddRange(New Object() {"http://www.imdb.com/", "http://uk.imdb.com/", "http://us.imdb.com/", "http://akas.imdb.com/", "http://italian.imdb.com/", "http://www.imdb.de/"})
        Me.lb_IMDBMirrors.Location = New System.Drawing.Point(10, 33)
        Me.lb_IMDBMirrors.Margin = New System.Windows.Forms.Padding(4)
        Me.lb_IMDBMirrors.Name = "lb_IMDBMirrors"
        Me.lb_IMDBMirrors.Size = New System.Drawing.Size(282, 94)
        Me.lb_IMDBMirrors.TabIndex = 0
        '
        'gpbxPrefScraperImages
        '
        Me.gpbxPrefScraperImages.Controls.Add(Me.grpbxImdbCertPriority)
        Me.gpbxPrefScraperImages.Controls.Add(Me.grpbxMovieTagOptions)
        Me.gpbxPrefScraperImages.Controls.Add(Me.grpbxMovieBasicSave)
        Me.gpbxPrefScraperImages.Controls.Add(Me.grpbxScraperLimits)
        Me.gpbxPrefScraperImages.Controls.Add(Me.grpbxScraperMisc)
        Me.gpbxPrefScraperImages.Controls.Add(Me.grpbxMovPreferLang)
        Me.gpbxPrefScraperImages.Location = New System.Drawing.Point(333, 6)
        Me.gpbxPrefScraperImages.Name = "gpbxPrefScraperImages"
        Me.gpbxPrefScraperImages.Size = New System.Drawing.Size(655, 525)
        Me.gpbxPrefScraperImages.TabIndex = 77
        Me.gpbxPrefScraperImages.TabStop = false
        Me.gpbxPrefScraperImages.Text = "Scraping options"
        '
        'grpbxMovieTagOptions
        '
        Me.grpbxMovieTagOptions.Controls.Add(Me.cbTagRes)
        Me.grpbxMovieTagOptions.Controls.Add(Me.cbAllowUserTags)
        Me.grpbxMovieTagOptions.Controls.Add(Me.Label9)
        Me.grpbxMovieTagOptions.Controls.Add(Me.tb_MovTagBlacklist)
        Me.grpbxMovieTagOptions.Controls.Add(Me.Label8)
        Me.grpbxMovieTagOptions.Controls.Add(Me.Label69)
        Me.grpbxMovieTagOptions.Controls.Add(Me.cb_keywordlimit)
        Me.grpbxMovieTagOptions.Controls.Add(Me.cb_keywordasTag)
        Me.grpbxMovieTagOptions.Location = New System.Drawing.Point(343, 14)
        Me.grpbxMovieTagOptions.Name = "grpbxMovieTagOptions"
        Me.grpbxMovieTagOptions.Size = New System.Drawing.Size(300, 208)
        Me.grpbxMovieTagOptions.TabIndex = 84
        Me.grpbxMovieTagOptions.TabStop = false
        Me.grpbxMovieTagOptions.Text = "Keywords As Tags"
        '
        'cbTagRes
        '
        Me.cbTagRes.AutoSize = true
        Me.cbTagRes.Location = New System.Drawing.Point(11, 72)
        Me.cbTagRes.Name = "cbTagRes"
        Me.cbTagRes.Size = New System.Drawing.Size(172, 19)
        Me.cbTagRes.TabIndex = 71
        Me.cbTagRes.Text = "Store Resolution as 1st tag"
        Me.cbTagRes.UseVisualStyleBackColor = true
        '
        'Label9
        '
        Me.Label9.AutoSize = true
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label9.Location = New System.Drawing.Point(262, 97)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(31, 18)
        Me.Label9.TabIndex = 69
        Me.Label9.Text = "' ; '"
        '
        'tb_MovTagBlacklist
        '
        Me.tb_MovTagBlacklist.Location = New System.Drawing.Point(12, 133)
        Me.tb_MovTagBlacklist.Multiline = true
        Me.tb_MovTagBlacklist.Name = "tb_MovTagBlacklist"
        Me.tb_MovTagBlacklist.Size = New System.Drawing.Size(277, 67)
        Me.tb_MovTagBlacklist.TabIndex = 68
        '
        'Label8
        '
        Me.Label8.AutoSize = true
        Me.Label8.Location = New System.Drawing.Point(8, 97)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(251, 30)
        Me.Label8.TabIndex = 67
        Me.Label8.Text = "Keyword Blacklist - Separate with semi-colon"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"No Enter Key or New Line."
        '
        'Label69
        '
        Me.Label69.AutoSize = true
        Me.Label69.Location = New System.Drawing.Point(30, 54)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(176, 15)
        Me.Label69.TabIndex = 66
        Me.Label69.Text = "Maximum number of keywords"
        '
        'cb_keywordasTag
        '
        Me.cb_keywordasTag.AutoSize = true
        Me.cb_keywordasTag.Location = New System.Drawing.Point(11, 34)
        Me.cb_keywordasTag.Name = "cb_keywordasTag"
        Me.cb_keywordasTag.Size = New System.Drawing.Size(202, 19)
        Me.cb_keywordasTag.TabIndex = 0
        Me.cb_keywordasTag.Text = "Store Plot keywords in Tags field"
        Me.cb_keywordasTag.UseVisualStyleBackColor = true
        '
        'grpbxMovieBasicSave
        '
        Me.grpbxMovieBasicSave.Controls.Add(Me.cbMovieBasicSave)
        Me.grpbxMovieBasicSave.Controls.Add(Me.Label162)
        Me.grpbxMovieBasicSave.Controls.Add(Me.Label109)
        Me.grpbxMovieBasicSave.Location = New System.Drawing.Point(6, 380)
        Me.grpbxMovieBasicSave.Name = "grpbxMovieBasicSave"
        Me.grpbxMovieBasicSave.Size = New System.Drawing.Size(331, 131)
        Me.grpbxMovieBasicSave.TabIndex = 83
        Me.grpbxMovieBasicSave.TabStop = false
        Me.grpbxMovieBasicSave.Text = "Basic Save Mode"
        '
        'Label162
        '
        Me.Label162.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label162.Location = New System.Drawing.Point(4, 46)
        Me.Label162.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label162.Name = "Label162"
        Me.Label162.Size = New System.Drawing.Size(314, 30)
        Me.Label162.TabIndex = 45
        Me.Label162.Text = "(This is not recommended since it breaks MCs ability to see the  media file, used"& _ 
    " for playback and filedetails information.)"
        '
        'Label109
        '
        Me.Label109.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label109.Location = New System.Drawing.Point(4, 17)
        Me.Label109.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label109.Name = "Label109"
        Me.Label109.Size = New System.Drawing.Size(314, 27)
        Me.Label109.TabIndex = 44
        Me.Label109.Text = "Some people prefer to save their files as movie.nfo, movie.tbn and fanart.jpg to "& _ 
    "keep compatibility with other media managers."
        '
        'grpbxScraperLimits
        '
        Me.grpbxScraperLimits.Controls.Add(Me.cmbxMovieScraper_MaxStudios)
        Me.grpbxScraperLimits.Controls.Add(Me.lblMaxStudios)
        Me.grpbxScraperLimits.Controls.Add(Me.Label92)
        Me.grpbxScraperLimits.Controls.Add(Me.cmbxMovScraper_MaxGenres)
        Me.grpbxScraperLimits.Location = New System.Drawing.Point(6, 180)
        Me.grpbxScraperLimits.Name = "grpbxScraperLimits"
        Me.grpbxScraperLimits.Size = New System.Drawing.Size(331, 80)
        Me.grpbxScraperLimits.TabIndex = 75
        Me.grpbxScraperLimits.TabStop = false
        Me.grpbxScraperLimits.Text = "Scraper Limits"
        '
        'lblMaxStudios
        '
        Me.lblMaxStudios.AutoSize = true
        Me.lblMaxStudios.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblMaxStudios.Location = New System.Drawing.Point(6, 49)
        Me.lblMaxStudios.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMaxStudios.Name = "lblMaxStudios"
        Me.lblMaxStudios.Size = New System.Drawing.Size(167, 15)
        Me.lblMaxStudios.TabIndex = 63
        Me.lblMaxStudios.Text = "Maximum number of studios:"
        '
        'Label92
        '
        Me.Label92.AutoSize = true
        Me.Label92.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label92.Location = New System.Drawing.Point(7, 21)
        Me.Label92.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(166, 15)
        Me.Label92.TabIndex = 45
        Me.Label92.Text = "Maximum number of genres:"
        '
        'grpbxScraperMisc
        '
        Me.grpbxScraperMisc.Controls.Add(Me.cbMovSetOverviewToNfo)
        Me.grpbxScraperMisc.Controls.Add(Me.cbSetIdAsCollectionnumber)
        Me.grpbxScraperMisc.Controls.Add(Me.cbIMDbOriginalTitle)
        Me.grpbxScraperMisc.Controls.Add(Me.cbGetMovieSetFromTMDb)
        Me.grpbxScraperMisc.Location = New System.Drawing.Point(6, 266)
        Me.grpbxScraperMisc.Name = "grpbxScraperMisc"
        Me.grpbxScraperMisc.Size = New System.Drawing.Size(331, 108)
        Me.grpbxScraperMisc.TabIndex = 52
        Me.grpbxScraperMisc.TabStop = false
        Me.grpbxScraperMisc.Text = " Other options "
        '
        'cbMovSetOverviewToNfo
        '
        Me.cbMovSetOverviewToNfo.AutoSize = true
        Me.cbMovSetOverviewToNfo.Location = New System.Drawing.Point(37, 56)
        Me.cbMovSetOverviewToNfo.Name = "cbMovSetOverviewToNfo"
        Me.cbMovSetOverviewToNfo.Size = New System.Drawing.Size(262, 19)
        Me.cbMovSetOverviewToNfo.TabIndex = 53
        Me.cbMovSetOverviewToNfo.Text = "Save Set Overview to nfo (Kodi 17 onwards)"
        Me.cbMovSetOverviewToNfo.UseVisualStyleBackColor = true
        '
        'cbIMDbOriginalTitle
        '
        Me.cbIMDbOriginalTitle.AutoSize = true
        Me.cbIMDbOriginalTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbIMDbOriginalTitle.Location = New System.Drawing.Point(10, 85)
        Me.cbIMDbOriginalTitle.Name = "cbIMDbOriginalTitle"
        Me.cbIMDbOriginalTitle.Size = New System.Drawing.Size(303, 17)
        Me.cbIMDbOriginalTitle.TabIndex = 51
        Me.cbIMDbOriginalTitle.Text = "IMDB - Where available scrape 'Original title' instead of title"
        Me.cbIMDbOriginalTitle.UseVisualStyleBackColor = true
        '
        'grpbxMovPreferLang
        '
        Me.grpbxMovPreferLang.Controls.Add(Me.Label27)
        Me.grpbxMovPreferLang.Controls.Add(Me.grpbxCustomLanguage)
        Me.grpbxMovPreferLang.Controls.Add(Me.Label177)
        Me.grpbxMovPreferLang.Controls.Add(Me.cbUseCustomLanguage)
        Me.grpbxMovPreferLang.Controls.Add(Me.cmbxTMDbSelectedLanguage)
        Me.grpbxMovPreferLang.Location = New System.Drawing.Point(6, 16)
        Me.grpbxMovPreferLang.Name = "grpbxMovPreferLang"
        Me.grpbxMovPreferLang.Size = New System.Drawing.Size(331, 158)
        Me.grpbxMovPreferLang.TabIndex = 38
        Me.grpbxMovPreferLang.TabStop = false
        Me.grpbxMovPreferLang.Text = "Preferred Language"
        '
        'Label27
        '
        Me.Label27.AutoSize = true
        Me.Label27.Location = New System.Drawing.Point(6, 17)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(285, 15)
        Me.Label27.TabIndex = 23
        Me.Label27.Text = "Set language for Images and other info from TMDB"
        '
        'grpbxCustomLanguage
        '
        Me.grpbxCustomLanguage.Controls.Add(Me.llLanguagesFile)
        Me.grpbxCustomLanguage.Controls.Add(Me.tbCustomLanguageValue)
        Me.grpbxCustomLanguage.Controls.Add(Me.Label174)
        Me.grpbxCustomLanguage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.grpbxCustomLanguage.Location = New System.Drawing.Point(28, 88)
        Me.grpbxCustomLanguage.Name = "grpbxCustomLanguage"
        Me.grpbxCustomLanguage.Size = New System.Drawing.Size(291, 64)
        Me.grpbxCustomLanguage.TabIndex = 22
        Me.grpbxCustomLanguage.TabStop = false
        Me.grpbxCustomLanguage.Text = " Custom string"
        '
        'llLanguagesFile
        '
        Me.llLanguagesFile.AutoSize = true
        Me.llLanguagesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.llLanguagesFile.Location = New System.Drawing.Point(50, 45)
        Me.llLanguagesFile.Name = "llLanguagesFile"
        Me.llLanguagesFile.Size = New System.Drawing.Size(158, 12)
        Me.llLanguagesFile.TabIndex = 21
        Me.llLanguagesFile.TabStop = true
        Me.llLanguagesFile.Text = "Please read the languages file for help"
        '
        'tbCustomLanguageValue
        '
        Me.tbCustomLanguageValue.Location = New System.Drawing.Point(53, 21)
        Me.tbCustomLanguageValue.Name = "tbCustomLanguageValue"
        Me.tbCustomLanguageValue.Size = New System.Drawing.Size(232, 21)
        Me.tbCustomLanguageValue.TabIndex = 20
        Me.tbCustomLanguageValue.Tag = "M"
        '
        'Label174
        '
        Me.Label174.AutoSize = true
        Me.Label174.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label174.Location = New System.Drawing.Point(6, 24)
        Me.Label174.Name = "Label174"
        Me.Label174.Size = New System.Drawing.Size(44, 15)
        Me.Label174.TabIndex = 18
        Me.Label174.Text = "Value :"
        '
        'Label177
        '
        Me.Label177.AutoSize = true
        Me.Label177.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label177.Location = New System.Drawing.Point(6, 45)
        Me.Label177.Name = "Label177"
        Me.Label177.Size = New System.Drawing.Size(102, 15)
        Me.Label177.TabIndex = 21
        Me.Label177.Text = "Select language :"
        '
        'cbUseCustomLanguage
        '
        Me.cbUseCustomLanguage.AutoSize = true
        Me.cbUseCustomLanguage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbUseCustomLanguage.Location = New System.Drawing.Point(7, 71)
        Me.cbUseCustomLanguage.Name = "cbUseCustomLanguage"
        Me.cbUseCustomLanguage.Size = New System.Drawing.Size(137, 19)
        Me.cbUseCustomLanguage.TabIndex = 20
        Me.cbUseCustomLanguage.Text = "Or define your own..."
        Me.cbUseCustomLanguage.UseVisualStyleBackColor = true
        '
        'cmbxTMDbSelectedLanguage
        '
        Me.cmbxTMDbSelectedLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxTMDbSelectedLanguage.FormattingEnabled = true
        Me.cmbxTMDbSelectedLanguage.Location = New System.Drawing.Point(114, 42)
        Me.cmbxTMDbSelectedLanguage.Name = "cmbxTMDbSelectedLanguage"
        Me.cmbxTMDbSelectedLanguage.Size = New System.Drawing.Size(130, 23)
        Me.cmbxTMDbSelectedLanguage.TabIndex = 19
        '
        'grpbxMovieFolderOptions
        '
        Me.grpbxMovieFolderOptions.Controls.Add(Me.cbMovieAllInFolders)
        Me.grpbxMovieFolderOptions.Controls.Add(Me.cbMovieUseFolderNames)
        Me.grpbxMovieFolderOptions.Location = New System.Drawing.Point(7, 370)
        Me.grpbxMovieFolderOptions.Name = "grpbxMovieFolderOptions"
        Me.grpbxMovieFolderOptions.Size = New System.Drawing.Size(319, 70)
        Me.grpbxMovieFolderOptions.TabIndex = 78
        Me.grpbxMovieFolderOptions.TabStop = false
        Me.grpbxMovieFolderOptions.Text = "Individual Movie Folder Options"
        '
        'cbMovieAllInFolders
        '
        Me.cbMovieAllInFolders.AutoSize = true
        Me.cbMovieAllInFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieAllInFolders.Location = New System.Drawing.Point(7, 44)
        Me.cbMovieAllInFolders.Name = "cbMovieAllInFolders"
        Me.cbMovieAllInFolders.Size = New System.Drawing.Size(239, 17)
        Me.cbMovieAllInFolders.TabIndex = 52
        Me.cbMovieAllInFolders.Text = "All Movies are in Folders (allows Extrathumbs)"
        Me.cbMovieAllInFolders.UseVisualStyleBackColor = true
        '
        'cbMovieUseFolderNames
        '
        Me.cbMovieUseFolderNames.AutoSize = true
        Me.cbMovieUseFolderNames.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieUseFolderNames.Location = New System.Drawing.Point(7, 20)
        Me.cbMovieUseFolderNames.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieUseFolderNames.Name = "cbMovieUseFolderNames"
        Me.cbMovieUseFolderNames.Size = New System.Drawing.Size(272, 17)
        Me.cbMovieUseFolderNames.TabIndex = 50
        Me.cbMovieUseFolderNames.Text = "Use Folder Names for Scraping (allows Extrathumbs)"
        Me.cbMovieUseFolderNames.UseVisualStyleBackColor = true
        '
        'tpMoviePreferences_Artwork
        '
        Me.tpMoviePreferences_Artwork.Controls.Add(Me.grpbxMovieSetArtwork)
        Me.tpMoviePreferences_Artwork.Controls.Add(Me.GrpbxXtraArtwork)
        Me.tpMoviePreferences_Artwork.Controls.Add(Me.grpbxMoviePosterPriority)
        Me.tpMoviePreferences_Artwork.Controls.Add(Me.grpbxAutoScrapeArtwork)
        Me.tpMoviePreferences_Artwork.Location = New System.Drawing.Point(4, 24)
        Me.tpMoviePreferences_Artwork.Name = "tpMoviePreferences_Artwork"
        Me.tpMoviePreferences_Artwork.Size = New System.Drawing.Size(184, 46)
        Me.tpMoviePreferences_Artwork.TabIndex = 4
        Me.tpMoviePreferences_Artwork.Text = "Artwork"
        Me.tpMoviePreferences_Artwork.UseVisualStyleBackColor = true
        '
        'grpbxMovieSetArtwork
        '
        Me.grpbxMovieSetArtwork.Controls.Add(Me.Label37)
        Me.grpbxMovieSetArtwork.Controls.Add(Me.tbMovSetArtCentralFolder)
        Me.grpbxMovieSetArtwork.Controls.Add(Me.btnMovSetCentralFolderSelect)
        Me.grpbxMovieSetArtwork.Controls.Add(Me.rbMovSetArtSetFolder)
        Me.grpbxMovieSetArtwork.Controls.Add(Me.rbMovSetFolder)
        Me.grpbxMovieSetArtwork.Location = New System.Drawing.Point(348, 226)
        Me.grpbxMovieSetArtwork.Name = "grpbxMovieSetArtwork"
        Me.grpbxMovieSetArtwork.Size = New System.Drawing.Size(327, 175)
        Me.grpbxMovieSetArtwork.TabIndex = 83
        Me.grpbxMovieSetArtwork.TabStop = false
        Me.grpbxMovieSetArtwork.Text = "MovieSet Artwork"
        '
        'Label37
        '
        Me.Label37.AutoSize = true
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label37.Location = New System.Drawing.Point(16, 26)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(278, 52)
        Me.Label37.TabIndex = 5
        Me.Label37.Text = "Please set your preference location for Movie Set Artwork"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"If your Movie is in a "& _ 
    "Set, but not in a Set Folder, or a"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"central folder is not selected, then No Movi"& _ 
    "eSet artwork"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"will be downloaded."
        '
        'tbMovSetArtCentralFolder
        '
        Me.tbMovSetArtCentralFolder.Location = New System.Drawing.Point(5, 139)
        Me.tbMovSetArtCentralFolder.MaxLength = 1000
        Me.tbMovSetArtCentralFolder.Name = "tbMovSetArtCentralFolder"
        Me.tbMovSetArtCentralFolder.ReadOnly = true
        Me.tbMovSetArtCentralFolder.Size = New System.Drawing.Size(294, 21)
        Me.tbMovSetArtCentralFolder.TabIndex = 4
        Me.tbMovSetArtCentralFolder.WordWrap = false
        '
        'btnMovSetCentralFolderSelect
        '
        Me.btnMovSetCentralFolderSelect.Location = New System.Drawing.Point(163, 110)
        Me.btnMovSetCentralFolderSelect.Name = "btnMovSetCentralFolderSelect"
        Me.btnMovSetCentralFolderSelect.Size = New System.Drawing.Size(142, 23)
        Me.btnMovSetCentralFolderSelect.TabIndex = 3
        Me.btnMovSetCentralFolderSelect.Text = "Select Central Folder"
        Me.btnMovSetCentralFolderSelect.UseVisualStyleBackColor = true
        '
        'rbMovSetArtSetFolder
        '
        Me.rbMovSetArtSetFolder.AutoSize = true
        Me.rbMovSetArtSetFolder.Location = New System.Drawing.Point(11, 112)
        Me.rbMovSetArtSetFolder.Name = "rbMovSetArtSetFolder"
        Me.rbMovSetArtSetFolder.Size = New System.Drawing.Size(115, 19)
        Me.rbMovSetArtSetFolder.TabIndex = 2
        Me.rbMovSetArtSetFolder.TabStop = true
        Me.rbMovSetArtSetFolder.Text = "to Central Folder"
        Me.rbMovSetArtSetFolder.UseVisualStyleBackColor = true
        '
        'rbMovSetFolder
        '
        Me.rbMovSetFolder.AutoSize = true
        Me.rbMovSetFolder.Location = New System.Drawing.Point(11, 88)
        Me.rbMovSetFolder.Name = "rbMovSetFolder"
        Me.rbMovSetFolder.Size = New System.Drawing.Size(166, 19)
        Me.rbMovSetFolder.TabIndex = 1
        Me.rbMovSetFolder.TabStop = true
        Me.rbMovSetFolder.Text = "to Movie Collection Folder"
        Me.rbMovSetFolder.UseVisualStyleBackColor = true
        '
        'GrpbxXtraArtwork
        '
        Me.GrpbxXtraArtwork.Controls.Add(Me.Label30)
        Me.GrpbxXtraArtwork.Controls.Add(Me.cmbxMovXtraFanartQty)
        Me.GrpbxXtraArtwork.Controls.Add(Me.grpbxMoviesInFoldersArtwork)
        Me.GrpbxXtraArtwork.Controls.Add(Me.cbMovXtraFanart)
        Me.GrpbxXtraArtwork.Controls.Add(Me.cbMovXtraThumbs)
        Me.GrpbxXtraArtwork.Location = New System.Drawing.Point(348, 13)
        Me.GrpbxXtraArtwork.Name = "GrpbxXtraArtwork"
        Me.GrpbxXtraArtwork.Size = New System.Drawing.Size(327, 207)
        Me.GrpbxXtraArtwork.TabIndex = 82
        Me.GrpbxXtraArtwork.TabStop = false
        Me.GrpbxXtraArtwork.Text = "If Movies in folders, save extra Artwork"
        '
        'Label30
        '
        Me.Label30.AutoSize = true
        Me.Label30.Location = New System.Drawing.Point(154, 49)
        Me.Label30.Name = "Label30"
        Me.Label30.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label30.Size = New System.Drawing.Size(100, 15)
        Me.Label30.TabIndex = 5
        Me.Label30.Text = "..............Quantity?"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbxMovXtraFanartQty
        '
        Me.cmbxMovXtraFanartQty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxMovXtraFanartQty.FormattingEnabled = true
        Me.cmbxMovXtraFanartQty.Items.AddRange(New Object() {"5", "10", "15", "20"})
        Me.cmbxMovXtraFanartQty.Location = New System.Drawing.Point(254, 46)
        Me.cmbxMovXtraFanartQty.Name = "cmbxMovXtraFanartQty"
        Me.cmbxMovXtraFanartQty.Size = New System.Drawing.Size(64, 23)
        Me.cmbxMovXtraFanartQty.TabIndex = 4
        '
        'grpbxMoviesInFoldersArtwork
        '
        Me.grpbxMoviesInFoldersArtwork.Controls.Add(Me.cbMovCreateFanartjpg)
        Me.grpbxMoviesInFoldersArtwork.Controls.Add(Me.cbMoviePosterInFolder)
        Me.grpbxMoviesInFoldersArtwork.Controls.Add(Me.cbMovieFanartInFolders)
        Me.grpbxMoviesInFoldersArtwork.Controls.Add(Me.cbMovCreateFolderjpg)
        Me.grpbxMoviesInFoldersArtwork.Location = New System.Drawing.Point(7, 78)
        Me.grpbxMoviesInFoldersArtwork.Name = "grpbxMoviesInFoldersArtwork"
        Me.grpbxMoviesInFoldersArtwork.Size = New System.Drawing.Size(311, 119)
        Me.grpbxMoviesInFoldersArtwork.TabIndex = 3
        Me.grpbxMoviesInFoldersArtwork.TabStop = false
        Me.grpbxMoviesInFoldersArtwork.Text = "If Movies In Folders, Save artwork as:"
        '
        'cbMovCreateFolderjpg
        '
        Me.cbMovCreateFolderjpg.AutoSize = true
        Me.cbMovCreateFolderjpg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovCreateFolderjpg.Location = New System.Drawing.Point(6, 69)
        Me.cbMovCreateFolderjpg.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovCreateFolderjpg.Name = "cbMovCreateFolderjpg"
        Me.cbMovCreateFolderjpg.Size = New System.Drawing.Size(216, 19)
        Me.cbMovCreateFolderjpg.TabIndex = 51
        Me.cbMovCreateFolderjpg.Text = "Create folder.jpg file for each folder"
        Me.cbMovCreateFolderjpg.UseVisualStyleBackColor = true
        '
        'grpbxMoviePosterPriority
        '
        Me.grpbxMoviePosterPriority.Controls.Add(Me.btn_MovPosterPriorityRemove)
        Me.grpbxMoviePosterPriority.Controls.Add(Me.btn_MovPosterPriorityReset)
        Me.grpbxMoviePosterPriority.Controls.Add(Me.Label99)
        Me.grpbxMoviePosterPriority.Controls.Add(Me.Label93)
        Me.grpbxMoviePosterPriority.Controls.Add(Me.btnMovPosterPriorityDown)
        Me.grpbxMoviePosterPriority.Controls.Add(Me.btnMovPosterPriorityUp)
        Me.grpbxMoviePosterPriority.Controls.Add(Me.lbPosterSourcePriorities)
        Me.grpbxMoviePosterPriority.Location = New System.Drawing.Point(9, 365)
        Me.grpbxMoviePosterPriority.Margin = New System.Windows.Forms.Padding(4)
        Me.grpbxMoviePosterPriority.Name = "grpbxMoviePosterPriority"
        Me.grpbxMoviePosterPriority.Padding = New System.Windows.Forms.Padding(4)
        Me.grpbxMoviePosterPriority.Size = New System.Drawing.Size(327, 165)
        Me.grpbxMoviePosterPriority.TabIndex = 39
        Me.grpbxMoviePosterPriority.TabStop = false
        Me.grpbxMoviePosterPriority.Text = "Movie Scraper Poster Priority"
        '
        'btn_MovPosterPriorityRemove
        '
        Me.btn_MovPosterPriorityRemove.Location = New System.Drawing.Point(151, 130)
        Me.btn_MovPosterPriorityRemove.Name = "btn_MovPosterPriorityRemove"
        Me.btn_MovPosterPriorityRemove.Size = New System.Drawing.Size(112, 23)
        Me.btn_MovPosterPriorityRemove.TabIndex = 6
        Me.btn_MovPosterPriorityRemove.Text = "Remove from List"
        Me.btn_MovPosterPriorityRemove.UseVisualStyleBackColor = true
        '
        'btn_MovPosterPriorityReset
        '
        Me.btn_MovPosterPriorityReset.Location = New System.Drawing.Point(13, 130)
        Me.btn_MovPosterPriorityReset.Name = "btn_MovPosterPriorityReset"
        Me.btn_MovPosterPriorityReset.Size = New System.Drawing.Size(113, 23)
        Me.btn_MovPosterPriorityReset.TabIndex = 5
        Me.btn_MovPosterPriorityReset.Text = "Reset List"
        Me.btn_MovPosterPriorityReset.UseVisualStyleBackColor = true
        '
        'Label99
        '
        Me.Label99.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label99.Location = New System.Drawing.Point(2, 15)
        Me.Label99.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(293, 20)
        Me.Label99.TabIndex = 4
        Me.Label99.Text = "Prioritise the order that sources are checked below."
        '
        'Label93
        '
        Me.Label93.AutoSize = true
        Me.Label93.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label93.Location = New System.Drawing.Point(227, 58)
        Me.Label93.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(78, 36)
        Me.Label93.TabIndex = 3
        Me.Label93.Text = "Change Priority of"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Default Movie"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Thumbnail Source"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        '
        'btnMovPosterPriorityDown
        '
        Me.btnMovPosterPriorityDown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnMovPosterPriorityDown.Location = New System.Drawing.Point(229, 98)
        Me.btnMovPosterPriorityDown.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMovPosterPriorityDown.Name = "btnMovPosterPriorityDown"
        Me.btnMovPosterPriorityDown.Size = New System.Drawing.Size(34, 25)
        Me.btnMovPosterPriorityDown.TabIndex = 2
        Me.btnMovPosterPriorityDown.Text = "↓"
        Me.btnMovPosterPriorityDown.UseVisualStyleBackColor = true
        '
        'btnMovPosterPriorityUp
        '
        Me.btnMovPosterPriorityUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnMovPosterPriorityUp.Location = New System.Drawing.Point(229, 34)
        Me.btnMovPosterPriorityUp.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMovPosterPriorityUp.Name = "btnMovPosterPriorityUp"
        Me.btnMovPosterPriorityUp.Size = New System.Drawing.Size(34, 25)
        Me.btnMovPosterPriorityUp.TabIndex = 1
        Me.btnMovPosterPriorityUp.Text = "↑"
        Me.btnMovPosterPriorityUp.UseVisualStyleBackColor = true
        '
        'lbPosterSourcePriorities
        '
        Me.lbPosterSourcePriorities.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbPosterSourcePriorities.FormattingEnabled = true
        Me.lbPosterSourcePriorities.ItemHeight = 15
        Me.lbPosterSourcePriorities.Location = New System.Drawing.Point(11, 39)
        Me.lbPosterSourcePriorities.Margin = New System.Windows.Forms.Padding(4)
        Me.lbPosterSourcePriorities.Name = "lbPosterSourcePriorities"
        Me.lbPosterSourcePriorities.Size = New System.Drawing.Size(212, 79)
        Me.lbPosterSourcePriorities.TabIndex = 0
        '
        'grpbxAutoScrapeArtwork
        '
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.rbMovSetArtScrapeFanartTv)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.rbMovSetArtScrapeTMDb)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMovCustPosterjpgNoDelete)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMovCustFolderjpgNoDelete)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMovFanartNaming)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.btnMovFanartTvSelect)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.Label10)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMovFanartTvScrape)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.grpbxExtraArtwork)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMovSetArtScrape)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMovFanartScrape)
        Me.grpbxAutoScrapeArtwork.Controls.Add(Me.cbMoviePosterScrape)
        Me.grpbxAutoScrapeArtwork.Location = New System.Drawing.Point(9, 13)
        Me.grpbxAutoScrapeArtwork.Name = "grpbxAutoScrapeArtwork"
        Me.grpbxAutoScrapeArtwork.Size = New System.Drawing.Size(327, 345)
        Me.grpbxAutoScrapeArtwork.TabIndex = 60
        Me.grpbxAutoScrapeArtwork.TabStop = false
        Me.grpbxAutoScrapeArtwork.Text = "Autoscrape artwork"
        '
        'rbMovSetArtScrapeTMDb
        '
        Me.rbMovSetArtScrapeTMDb.AutoSize = true
        Me.rbMovSetArtScrapeTMDb.Location = New System.Drawing.Point(17, 224)
        Me.rbMovSetArtScrapeTMDb.Name = "rbMovSetArtScrapeTMDb"
        Me.rbMovSetArtScrapeTMDb.Size = New System.Drawing.Size(139, 19)
        Me.rbMovSetArtScrapeTMDb.TabIndex = 51
        Me.rbMovSetArtScrapeTMDb.TabStop = true
        Me.rbMovSetArtScrapeTMDb.Text = "From TMDb (default)"
        Me.rbMovSetArtScrapeTMDb.UseVisualStyleBackColor = true
        '
        'cbMovCustPosterjpgNoDelete
        '
        Me.cbMovCustPosterjpgNoDelete.Location = New System.Drawing.Point(5, 304)
        Me.cbMovCustPosterjpgNoDelete.Name = "cbMovCustPosterjpgNoDelete"
        Me.cbMovCustPosterjpgNoDelete.Size = New System.Drawing.Size(300, 35)
        Me.cbMovCustPosterjpgNoDelete.TabIndex = 50
        Me.cbMovCustPosterjpgNoDelete.Text = "I use <moviename>.jpg as Poster, use and rename as Kodi poster."
        Me.cbMovCustPosterjpgNoDelete.UseVisualStyleBackColor = true
        '
        'cbMovCustFolderjpgNoDelete
        '
        Me.cbMovCustFolderjpgNoDelete.Location = New System.Drawing.Point(5, 271)
        Me.cbMovCustFolderjpgNoDelete.Name = "cbMovCustFolderjpgNoDelete"
        Me.cbMovCustFolderjpgNoDelete.Size = New System.Drawing.Size(295, 36)
        Me.cbMovCustFolderjpgNoDelete.TabIndex = 49
        Me.cbMovCustFolderjpgNoDelete.Text = "I use custom folder.jpg images, do not delete on autoscrape."
        Me.cbMovCustFolderjpgNoDelete.UseVisualStyleBackColor = true
        '
        'cbMovFanartNaming
        '
        Me.cbMovFanartNaming.AutoSize = true
        Me.cbMovFanartNaming.Location = New System.Drawing.Point(5, 111)
        Me.cbMovFanartNaming.Name = "cbMovFanartNaming"
        Me.cbMovFanartNaming.Size = New System.Drawing.Size(293, 19)
        Me.cbMovFanartNaming.TabIndex = 48
        Me.cbMovFanartNaming.Text = "Save Fanart.TV artwork as <moviename>-artwork"
        Me.cbMovFanartNaming.UseVisualStyleBackColor = true
        '
        'btnMovFanartTvSelect
        '
        Me.btnMovFanartTvSelect.Location = New System.Drawing.Point(31, 78)
        Me.btnMovFanartTvSelect.Name = "btnMovFanartTvSelect"
        Me.btnMovFanartTvSelect.Size = New System.Drawing.Size(75, 23)
        Me.btnMovFanartTvSelect.TabIndex = 47
        Me.btnMovFanartTvSelect.Text = "Select Art"
        Me.btnMovFanartTvSelect.UseVisualStyleBackColor = true
        '
        'Label10
        '
        Me.Label10.AutoSize = true
        Me.Label10.Location = New System.Drawing.Point(112, 79)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(193, 30)
        Me.Label10.TabIndex = 46
        Me.Label10.Text = "Choose Fanart.TV art to Download"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Default is All available."
        '
        'grpbxExtraArtwork
        '
        Me.grpbxExtraArtwork.Controls.Add(Me.Label189)
        Me.grpbxExtraArtwork.Controls.Add(Me.cbDlXtraFanart)
        Me.grpbxExtraArtwork.Location = New System.Drawing.Point(4, 136)
        Me.grpbxExtraArtwork.Name = "grpbxExtraArtwork"
        Me.grpbxExtraArtwork.Size = New System.Drawing.Size(317, 61)
        Me.grpbxExtraArtwork.TabIndex = 44
        Me.grpbxExtraArtwork.TabStop = false
        Me.grpbxExtraArtwork.Text = "Extra Artwork"
        '
        'Label189
        '
        Me.Label189.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label189.Location = New System.Drawing.Point(3, 37)
        Me.Label189.Name = "Label189"
        Me.Label189.Size = New System.Drawing.Size(293, 18)
        Me.Label189.TabIndex = 4
        Me.Label189.Text = "NB: - Movies must be in folders. Select Type of art below."
        '
        'cbMovSetArtScrape
        '
        Me.cbMovSetArtScrape.AutoSize = true
        Me.cbMovSetArtScrape.Location = New System.Drawing.Point(5, 203)
        Me.cbMovSetArtScrape.Name = "cbMovSetArtScrape"
        Me.cbMovSetArtScrape.Size = New System.Drawing.Size(295, 19)
        Me.cbMovSetArtScrape.TabIndex = 0
        Me.cbMovSetArtScrape.Text = "Download MovieSet Artwork if not already present"
        Me.cbMovSetArtScrape.UseVisualStyleBackColor = true
        '
        'tpMoviePreferences_General
        '
        Me.tpMoviePreferences_General.BackColor = System.Drawing.SystemColors.Control
        Me.tpMoviePreferences_General.Controls.Add(Me.grpbxMovieFilters)
        Me.tpMoviePreferences_General.Controls.Add(Me.grpbxMovieListOptions)
        Me.tpMoviePreferences_General.Controls.Add(Me.grpbxMovieRenameOptions)
        Me.tpMoviePreferences_General.Controls.Add(Me.grpbxMovieOfflineTitleText)
        Me.tpMoviePreferences_General.Controls.Add(Me.grpbxMovieGeneralOptions)
        Me.tpMoviePreferences_General.Controls.Add(Me.grpbxMovieNameMode)
        Me.tpMoviePreferences_General.Location = New System.Drawing.Point(4, 24)
        Me.tpMoviePreferences_General.Name = "tpMoviePreferences_General"
        Me.tpMoviePreferences_General.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMoviePreferences_General.Size = New System.Drawing.Size(184, 46)
        Me.tpMoviePreferences_General.TabIndex = 2
        Me.tpMoviePreferences_General.Text = "General"
        '
        'grpbxMovieFilters
        '
        Me.grpbxMovieFilters.Controls.Add(Me.cmbxMovFiltersTagsOrder)
        Me.grpbxMovieFilters.Controls.Add(Me.Label25)
        Me.grpbxMovieFilters.Controls.Add(Me.nudMaxTagsInFilter)
        Me.grpbxMovieFilters.Controls.Add(Me.nudMinTagsInFilter)
        Me.grpbxMovieFilters.Controls.Add(Me.cbMovieFilters_Sets_Order)
        Me.grpbxMovieFilters.Controls.Add(Me.Label54)
        Me.grpbxMovieFilters.Controls.Add(Me.cbMovieFilters_Directors_Order)
        Me.grpbxMovieFilters.Controls.Add(Me.nudMaxDirectorsInFilter)
        Me.grpbxMovieFilters.Controls.Add(Me.nudMaxSetsInFilter)
        Me.grpbxMovieFilters.Controls.Add(Me.nudDirectorsFilterMinFilms)
        Me.grpbxMovieFilters.Controls.Add(Me.nudSetsFilterMinFilms)
        Me.grpbxMovieFilters.Controls.Add(Me.Label11)
        Me.grpbxMovieFilters.Controls.Add(Me.Label12)
        Me.grpbxMovieFilters.Controls.Add(Me.cbMovieFilters_Actors_Order)
        Me.grpbxMovieFilters.Controls.Add(Me.cbDisableNotMatchingRenamePattern)
        Me.grpbxMovieFilters.Controls.Add(Me.Label180)
        Me.grpbxMovieFilters.Controls.Add(Me.Label164)
        Me.grpbxMovieFilters.Controls.Add(Me.nudMaxActorsInFilter)
        Me.grpbxMovieFilters.Controls.Add(Me.nudActorsFilterMinFilms)
        Me.grpbxMovieFilters.Controls.Add(Me.Label165)
        Me.grpbxMovieFilters.Location = New System.Drawing.Point(657, 172)
        Me.grpbxMovieFilters.Margin = New System.Windows.Forms.Padding(0)
        Me.grpbxMovieFilters.Name = "grpbxMovieFilters"
        Me.grpbxMovieFilters.Size = New System.Drawing.Size(329, 227)
        Me.grpbxMovieFilters.TabIndex = 82
        Me.grpbxMovieFilters.TabStop = false
        Me.grpbxMovieFilters.Text = " Movie Filters "
        '
        'cmbxMovFiltersTagsOrder
        '
        Me.cmbxMovFiltersTagsOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxMovFiltersTagsOrder.FormattingEnabled = true
        Me.cmbxMovFiltersTagsOrder.Items.AddRange(New Object() {"Num Movies desc", "A-Z asc"})
        Me.cmbxMovFiltersTagsOrder.Location = New System.Drawing.Point(198, 147)
        Me.cmbxMovFiltersTagsOrder.Name = "cmbxMovFiltersTagsOrder"
        Me.cmbxMovFiltersTagsOrder.Size = New System.Drawing.Size(125, 23)
        Me.cmbxMovFiltersTagsOrder.TabIndex = 15
        '
        'Label25
        '
        Me.Label25.AutoSize = true
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label25.Location = New System.Drawing.Point(2, 148)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(38, 15)
        Me.Label25.TabIndex = 16
        Me.Label25.Text = "Tags"
        '
        'nudMaxTagsInFilter
        '
        Me.nudMaxTagsInFilter.Location = New System.Drawing.Point(130, 147)
        Me.nudMaxTagsInFilter.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.nudMaxTagsInFilter.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxTagsInFilter.Name = "nudMaxTagsInFilter"
        Me.nudMaxTagsInFilter.Size = New System.Drawing.Size(62, 21)
        Me.nudMaxTagsInFilter.TabIndex = 14
        Me.nudMaxTagsInFilter.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'nudMinTagsInFilter
        '
        Me.nudMinTagsInFilter.Location = New System.Drawing.Point(68, 146)
        Me.nudMinTagsInFilter.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nudMinTagsInFilter.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMinTagsInFilter.Name = "nudMinTagsInFilter"
        Me.nudMinTagsInFilter.Size = New System.Drawing.Size(49, 21)
        Me.nudMinTagsInFilter.TabIndex = 13
        Me.nudMinTagsInFilter.Value = New Decimal(New Integer() {99, 0, 0, 0})
        '
        'cbMovieFilters_Sets_Order
        '
        Me.cbMovieFilters_Sets_Order.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMovieFilters_Sets_Order.FormattingEnabled = true
        Me.cbMovieFilters_Sets_Order.Items.AddRange(New Object() {"Num Movies desc", "A-Z asc"})
        Me.cbMovieFilters_Sets_Order.Location = New System.Drawing.Point(198, 118)
        Me.cbMovieFilters_Sets_Order.Name = "cbMovieFilters_Sets_Order"
        Me.cbMovieFilters_Sets_Order.Size = New System.Drawing.Size(125, 23)
        Me.cbMovieFilters_Sets_Order.TabIndex = 5
        '
        'Label54
        '
        Me.Label54.AutoSize = true
        Me.Label54.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label54.Location = New System.Drawing.Point(2, 119)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(35, 15)
        Me.Label54.TabIndex = 12
        Me.Label54.Text = "Sets"
        '
        'cbMovieFilters_Directors_Order
        '
        Me.cbMovieFilters_Directors_Order.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMovieFilters_Directors_Order.FormattingEnabled = true
        Me.cbMovieFilters_Directors_Order.Items.AddRange(New Object() {"Num Movies desc", "A-Z asc"})
        Me.cbMovieFilters_Directors_Order.Location = New System.Drawing.Point(198, 87)
        Me.cbMovieFilters_Directors_Order.Name = "cbMovieFilters_Directors_Order"
        Me.cbMovieFilters_Directors_Order.Size = New System.Drawing.Size(125, 23)
        Me.cbMovieFilters_Directors_Order.TabIndex = 11
        '
        'nudMaxDirectorsInFilter
        '
        Me.nudMaxDirectorsInFilter.Location = New System.Drawing.Point(130, 87)
        Me.nudMaxDirectorsInFilter.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMaxDirectorsInFilter.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxDirectorsInFilter.Name = "nudMaxDirectorsInFilter"
        Me.nudMaxDirectorsInFilter.Size = New System.Drawing.Size(62, 21)
        Me.nudMaxDirectorsInFilter.TabIndex = 10
        Me.nudMaxDirectorsInFilter.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'nudMaxSetsInFilter
        '
        Me.nudMaxSetsInFilter.Location = New System.Drawing.Point(130, 118)
        Me.nudMaxSetsInFilter.Maximum = New Decimal(New Integer() {5000, 0, 0, 0})
        Me.nudMaxSetsInFilter.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxSetsInFilter.Name = "nudMaxSetsInFilter"
        Me.nudMaxSetsInFilter.Size = New System.Drawing.Size(62, 21)
        Me.nudMaxSetsInFilter.TabIndex = 3
        Me.nudMaxSetsInFilter.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'nudDirectorsFilterMinFilms
        '
        Me.nudDirectorsFilterMinFilms.Location = New System.Drawing.Point(68, 86)
        Me.nudDirectorsFilterMinFilms.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nudDirectorsFilterMinFilms.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDirectorsFilterMinFilms.Name = "nudDirectorsFilterMinFilms"
        Me.nudDirectorsFilterMinFilms.Size = New System.Drawing.Size(49, 21)
        Me.nudDirectorsFilterMinFilms.TabIndex = 9
        Me.nudDirectorsFilterMinFilms.Value = New Decimal(New Integer() {99, 0, 0, 0})
        '
        'nudSetsFilterMinFilms
        '
        Me.nudSetsFilterMinFilms.Location = New System.Drawing.Point(68, 117)
        Me.nudSetsFilterMinFilms.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nudSetsFilterMinFilms.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudSetsFilterMinFilms.Name = "nudSetsFilterMinFilms"
        Me.nudSetsFilterMinFilms.Size = New System.Drawing.Size(49, 21)
        Me.nudSetsFilterMinFilms.TabIndex = 1
        Me.nudSetsFilterMinFilms.Value = New Decimal(New Integer() {99, 0, 0, 0})
        '
        'Label11
        '
        Me.Label11.AutoSize = true
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label11.Location = New System.Drawing.Point(2, 88)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(65, 15)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Directors"
        '
        'Label12
        '
        Me.Label12.AutoSize = true
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label12.Location = New System.Drawing.Point(2, 58)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 15)
        Me.Label12.TabIndex = 7
        Me.Label12.Text = "Actors"
        '
        'cbMovieFilters_Actors_Order
        '
        Me.cbMovieFilters_Actors_Order.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMovieFilters_Actors_Order.FormattingEnabled = true
        Me.cbMovieFilters_Actors_Order.Items.AddRange(New Object() {"Num Movies desc", "A-Z asc"})
        Me.cbMovieFilters_Actors_Order.Location = New System.Drawing.Point(198, 55)
        Me.cbMovieFilters_Actors_Order.Name = "cbMovieFilters_Actors_Order"
        Me.cbMovieFilters_Actors_Order.Size = New System.Drawing.Size(125, 23)
        Me.cbMovieFilters_Actors_Order.TabIndex = 5
        '
        'cbDisableNotMatchingRenamePattern
        '
        Me.cbDisableNotMatchingRenamePattern.Location = New System.Drawing.Point(7, 182)
        Me.cbDisableNotMatchingRenamePattern.Name = "cbDisableNotMatchingRenamePattern"
        Me.cbDisableNotMatchingRenamePattern.Size = New System.Drawing.Size(297, 38)
        Me.cbDisableNotMatchingRenamePattern.TabIndex = 6
        Me.cbDisableNotMatchingRenamePattern.Text = "Disable 'Not matching rename pattern' (check for better performance)"
        Me.cbDisableNotMatchingRenamePattern.UseVisualStyleBackColor = true
        '
        'Label180
        '
        Me.Label180.AutoSize = true
        Me.Label180.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label180.Location = New System.Drawing.Point(225, 32)
        Me.Label180.Name = "Label180"
        Me.Label180.Size = New System.Drawing.Size(43, 15)
        Me.Label180.TabIndex = 4
        Me.Label180.Text = "Order"
        '
        'Label164
        '
        Me.Label164.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label164.Location = New System.Drawing.Point(64, 20)
        Me.Label164.Margin = New System.Windows.Forms.Padding(1, 0, 1, 0)
        Me.Label164.Name = "Label164"
        Me.Label164.Size = New System.Drawing.Size(53, 33)
        Me.Label164.TabIndex = 0
        Me.Label164.Text = "Min Movies"
        '
        'nudMaxActorsInFilter
        '
        Me.nudMaxActorsInFilter.Location = New System.Drawing.Point(130, 57)
        Me.nudMaxActorsInFilter.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMaxActorsInFilter.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxActorsInFilter.Name = "nudMaxActorsInFilter"
        Me.nudMaxActorsInFilter.Size = New System.Drawing.Size(62, 21)
        Me.nudMaxActorsInFilter.TabIndex = 3
        Me.nudMaxActorsInFilter.Value = New Decimal(New Integer() {5000, 0, 0, 0})
        '
        'nudActorsFilterMinFilms
        '
        Me.nudActorsFilterMinFilms.Location = New System.Drawing.Point(68, 56)
        Me.nudActorsFilterMinFilms.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nudActorsFilterMinFilms.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudActorsFilterMinFilms.Name = "nudActorsFilterMinFilms"
        Me.nudActorsFilterMinFilms.Size = New System.Drawing.Size(49, 21)
        Me.nudActorsFilterMinFilms.TabIndex = 1
        Me.nudActorsFilterMinFilms.Value = New Decimal(New Integer() {99, 0, 0, 0})
        '
        'Label165
        '
        Me.Label165.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label165.Location = New System.Drawing.Point(137, 20)
        Me.Label165.Name = "Label165"
        Me.Label165.Size = New System.Drawing.Size(38, 33)
        Me.Label165.TabIndex = 2
        Me.Label165.Text = "Max list"
        '
        'grpbxMovieListOptions
        '
        Me.grpbxMovieListOptions.Controls.Add(Me.cbMovieList_ShowColWatched)
        Me.grpbxMovieListOptions.Controls.Add(Me.cbMovieList_ShowColPlot)
        Me.grpbxMovieListOptions.Controls.Add(Me.tbDateFormat)
        Me.grpbxMovieListOptions.Controls.Add(Me.Label179)
        Me.grpbxMovieListOptions.Controls.Add(Me.cbMovieShowDateOnList)
        Me.grpbxMovieListOptions.Location = New System.Drawing.Point(311, 405)
        Me.grpbxMovieListOptions.Name = "grpbxMovieListOptions"
        Me.grpbxMovieListOptions.Size = New System.Drawing.Size(340, 112)
        Me.grpbxMovieListOptions.TabIndex = 80
        Me.grpbxMovieListOptions.TabStop = false
        Me.grpbxMovieListOptions.Text = " Movie List "
        '
        'cbMovieList_ShowColWatched
        '
        Me.cbMovieList_ShowColWatched.AutoSize = true
        Me.cbMovieList_ShowColWatched.Location = New System.Drawing.Point(7, 55)
        Me.cbMovieList_ShowColWatched.Name = "cbMovieList_ShowColWatched"
        Me.cbMovieList_ShowColWatched.Size = New System.Drawing.Size(152, 19)
        Me.cbMovieList_ShowColWatched.TabIndex = 3
        Me.cbMovieList_ShowColWatched.Text = "Show column Watched"
        Me.cbMovieList_ShowColWatched.UseVisualStyleBackColor = true
        '
        'cbMovieList_ShowColPlot
        '
        Me.cbMovieList_ShowColPlot.AutoSize = true
        Me.cbMovieList_ShowColPlot.Location = New System.Drawing.Point(7, 36)
        Me.cbMovieList_ShowColPlot.Name = "cbMovieList_ShowColPlot"
        Me.cbMovieList_ShowColPlot.Size = New System.Drawing.Size(125, 19)
        Me.cbMovieList_ShowColPlot.TabIndex = 2
        Me.cbMovieList_ShowColPlot.Text = "Show column Plot"
        Me.cbMovieList_ShowColPlot.UseVisualStyleBackColor = true
        '
        'cbMovieShowDateOnList
        '
        Me.cbMovieShowDateOnList.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbMovieShowDateOnList.Enabled = false
        Me.cbMovieShowDateOnList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieShowDateOnList.Location = New System.Drawing.Point(7, 76)
        Me.cbMovieShowDateOnList.Name = "cbMovieShowDateOnList"
        Me.cbMovieShowDateOnList.Size = New System.Drawing.Size(275, 20)
        Me.cbMovieShowDateOnList.TabIndex = 72
        Me.cbMovieShowDateOnList.Text = "Show Date for 'Modified' && 'Date Added'"
        Me.cbMovieShowDateOnList.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbMovieShowDateOnList.UseVisualStyleBackColor = true
        '
        'grpbxMovieRenameOptions
        '
        Me.grpbxMovieRenameOptions.Controls.Add(Me.Label13)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.rbRenameFullStop)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.rbRenameUnderscore)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovNewFolderInRootFolder)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovSortIgnArticle)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovTitleIgnArticle)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovSetIgnArticle)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.Label197)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.Label196)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovFolderRename)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbRenameUnderscore)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.lblFolderRename)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.tb_MovFolderRename)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.LblFilename)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovieManualRename)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.cbMovieRenameEnable)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.Label100)
        Me.grpbxMovieRenameOptions.Controls.Add(Me.tb_MovieRenameTemplate)
        Me.grpbxMovieRenameOptions.Location = New System.Drawing.Point(311, 6)
        Me.grpbxMovieRenameOptions.Name = "grpbxMovieRenameOptions"
        Me.grpbxMovieRenameOptions.Size = New System.Drawing.Size(340, 393)
        Me.grpbxMovieRenameOptions.TabIndex = 74
        Me.grpbxMovieRenameOptions.TabStop = false
        Me.grpbxMovieRenameOptions.Text = "Rename Movie Settings"
        '
        'Label13
        '
        Me.Label13.AutoSize = true
        Me.Label13.Location = New System.Drawing.Point(63, 251)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(20, 15)
        Me.Label13.TabIndex = 86
        Me.Label13.Text = "Or"
        '
        'cbMovNewFolderInRootFolder
        '
        Me.cbMovNewFolderInRootFolder.AutoSize = true
        Me.cbMovNewFolderInRootFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovNewFolderInRootFolder.Location = New System.Drawing.Point(7, 139)
        Me.cbMovNewFolderInRootFolder.Name = "cbMovNewFolderInRootFolder"
        Me.cbMovNewFolderInRootFolder.Size = New System.Drawing.Size(182, 30)
        Me.cbMovNewFolderInRootFolder.TabIndex = 83
        Me.cbMovNewFolderInRootFolder.Text = "Create Folder under Root folder if"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"  not in a Movie Set"
        Me.cbMovNewFolderInRootFolder.UseVisualStyleBackColor = true
        '
        'cbMovSortIgnArticle
        '
        Me.cbMovSortIgnArticle.AutoSize = true
        Me.cbMovSortIgnArticle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovSortIgnArticle.Location = New System.Drawing.Point(7, 209)
        Me.cbMovSortIgnArticle.Name = "cbMovSortIgnArticle"
        Me.cbMovSortIgnArticle.Size = New System.Drawing.Size(178, 17)
        Me.cbMovSortIgnArticle.TabIndex = 82
        Me.cbMovSortIgnArticle.Text = "Ignore Article to end of Sort Title"
        Me.cbMovSortIgnArticle.UseVisualStyleBackColor = true
        '
        'cbMovTitleIgnArticle
        '
        Me.cbMovTitleIgnArticle.AutoSize = true
        Me.cbMovTitleIgnArticle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovTitleIgnArticle.Location = New System.Drawing.Point(7, 166)
        Me.cbMovTitleIgnArticle.Name = "cbMovTitleIgnArticle"
        Me.cbMovTitleIgnArticle.Size = New System.Drawing.Size(188, 17)
        Me.cbMovTitleIgnArticle.TabIndex = 81
        Me.cbMovTitleIgnArticle.Text = "Ignore Article to end of Movie Title"
        Me.cbMovTitleIgnArticle.UseVisualStyleBackColor = true
        '
        'cbMovSetIgnArticle
        '
        Me.cbMovSetIgnArticle.AutoSize = true
        Me.cbMovSetIgnArticle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovSetIgnArticle.Location = New System.Drawing.Point(7, 186)
        Me.cbMovSetIgnArticle.Name = "cbMovSetIgnArticle"
        Me.cbMovSetIgnArticle.Size = New System.Drawing.Size(184, 17)
        Me.cbMovSetIgnArticle.TabIndex = 80
        Me.cbMovSetIgnArticle.Text = "Ignore Article to end of Movie Set"
        Me.cbMovSetIgnArticle.UseVisualStyleBackColor = true
        '
        'Label197
        '
        Me.Label197.AutoSize = true
        Me.Label197.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline),System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label197.Location = New System.Drawing.Point(238, 15)
        Me.Label197.Name = "Label197"
        Me.Label197.Size = New System.Drawing.Size(67, 15)
        Me.Label197.TabIndex = 79
        Me.Label197.Text = "LEGEND:"
        '
        'Label196
        '
        Me.Label196.AutoSize = true
        Me.Label196.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label196.Font = New System.Drawing.Font("Microsoft Sans Serif", 7!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label196.Location = New System.Drawing.Point(7, 306)
        Me.Label196.Name = "Label196"
        Me.Label196.Size = New System.Drawing.Size(324, 80)
        Me.Label196.TabIndex = 78
        Me.Label196.Text = resources.GetString("Label196.Text")
        '
        'cbMovFolderRename
        '
        Me.cbMovFolderRename.AutoSize = true
        Me.cbMovFolderRename.Location = New System.Drawing.Point(7, 16)
        Me.cbMovFolderRename.Name = "cbMovFolderRename"
        Me.cbMovFolderRename.Size = New System.Drawing.Size(179, 19)
        Me.cbMovFolderRename.TabIndex = 77
        Me.cbMovFolderRename.Text = "Folder(s) during AutoScrape"
        Me.cbMovFolderRename.UseVisualStyleBackColor = true
        '
        'cbRenameUnderscore
        '
        Me.cbRenameUnderscore.AutoSize = true
        Me.cbRenameUnderscore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbRenameUnderscore.Location = New System.Drawing.Point(7, 232)
        Me.cbRenameUnderscore.Name = "cbRenameUnderscore"
        Me.cbRenameUnderscore.Size = New System.Drawing.Size(144, 17)
        Me.cbRenameUnderscore.TabIndex = 76
        Me.cbRenameUnderscore.Text = "Exchange Spaces with..."
        Me.cbRenameUnderscore.UseVisualStyleBackColor = true
        '
        'lblFolderRename
        '
        Me.lblFolderRename.AutoSize = true
        Me.lblFolderRename.Location = New System.Drawing.Point(6, 34)
        Me.lblFolderRename.Name = "lblFolderRename"
        Me.lblFolderRename.Size = New System.Drawing.Size(79, 15)
        Me.lblFolderRename.TabIndex = 75
        Me.lblFolderRename.Text = "Folder Name"
        '
        'tb_MovFolderRename
        '
        Me.tb_MovFolderRename.Location = New System.Drawing.Point(7, 49)
        Me.tb_MovFolderRename.Name = "tb_MovFolderRename"
        Me.tb_MovFolderRename.Size = New System.Drawing.Size(189, 21)
        Me.tb_MovFolderRename.TabIndex = 74
        Me.tb_MovFolderRename.Text = "%N\%T (%Y)"
        '
        'LblFilename
        '
        Me.LblFilename.AutoSize = true
        Me.LblFilename.Location = New System.Drawing.Point(4, 95)
        Me.LblFilename.Name = "LblFilename"
        Me.LblFilename.Size = New System.Drawing.Size(67, 15)
        Me.LblFilename.TabIndex = 73
        Me.LblFilename.Text = "File Name:"
        '
        'cbMovieManualRename
        '
        Me.cbMovieManualRename.AutoSize = true
        Me.cbMovieManualRename.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieManualRename.Location = New System.Drawing.Point(7, 281)
        Me.cbMovieManualRename.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieManualRename.Name = "cbMovieManualRename"
        Me.cbMovieManualRename.Size = New System.Drawing.Size(207, 19)
        Me.cbMovieManualRename.TabIndex = 72
        Me.cbMovieManualRename.Text = "Enable Manual Movie Renaming"
        Me.cbMovieManualRename.UseVisualStyleBackColor = true
        '
        'cbMovieRenameEnable
        '
        Me.cbMovieRenameEnable.AutoSize = true
        Me.cbMovieRenameEnable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieRenameEnable.Location = New System.Drawing.Point(7, 77)
        Me.cbMovieRenameEnable.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieRenameEnable.Name = "cbMovieRenameEnable"
        Me.cbMovieRenameEnable.Size = New System.Drawing.Size(163, 19)
        Me.cbMovieRenameEnable.TabIndex = 71
        Me.cbMovieRenameEnable.Text = "Movie during AutoScrape"
        Me.cbMovieRenameEnable.UseVisualStyleBackColor = true
        '
        'Label100
        '
        Me.Label100.AutoSize = true
        Me.Label100.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label100.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label100.Location = New System.Drawing.Point(202, 30)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(125, 236)
        Me.Label100.TabIndex = 70
        Me.Label100.Text = resources.GetString("Label100.Text")
        '
        'tb_MovieRenameTemplate
        '
        Me.tb_MovieRenameTemplate.Location = New System.Drawing.Point(7, 112)
        Me.tb_MovieRenameTemplate.Name = "tb_MovieRenameTemplate"
        Me.tb_MovieRenameTemplate.Size = New System.Drawing.Size(189, 21)
        Me.tb_MovieRenameTemplate.TabIndex = 69
        Me.tb_MovieRenameTemplate.Text = "%T (%Y)"
        '
        'grpbxMovieOfflineTitleText
        '
        Me.grpbxMovieOfflineTitleText.Controls.Add(Me.Label77)
        Me.grpbxMovieOfflineTitleText.Controls.Add(Me.tb_OfflineDVDTitle)
        Me.grpbxMovieOfflineTitleText.Location = New System.Drawing.Point(657, 427)
        Me.grpbxMovieOfflineTitleText.Name = "grpbxMovieOfflineTitleText"
        Me.grpbxMovieOfflineTitleText.Size = New System.Drawing.Size(329, 61)
        Me.grpbxMovieOfflineTitleText.TabIndex = 70
        Me.grpbxMovieOfflineTitleText.TabStop = false
        Me.grpbxMovieOfflineTitleText.Text = "Offline DVD Title Text"
        '
        'Label77
        '
        Me.Label77.AutoSize = true
        Me.Label77.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label77.Location = New System.Drawing.Point(7, 44)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(173, 13)
        Me.Label77.TabIndex = 70
        Me.Label77.Text = "Use Parameters  -  %T - Movie Title"
        '
        'tb_OfflineDVDTitle
        '
        Me.tb_OfflineDVDTitle.Location = New System.Drawing.Point(7, 20)
        Me.tb_OfflineDVDTitle.Name = "tb_OfflineDVDTitle"
        Me.tb_OfflineDVDTitle.Size = New System.Drawing.Size(286, 21)
        Me.tb_OfflineDVDTitle.TabIndex = 69
        Me.tb_OfflineDVDTitle.Text = "Please Insert '%T' Media"
        '
        'grpbxMovieGeneralOptions
        '
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbEnableMovDeleteFolderTsmi)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_MovRuntimeAsDuration)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbEnableFolderSize)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbShowMovieGridToolTip)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_MovPosterTabTMDBSelect)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_MovSetTitleIgnArticle)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_SorttitleIgnoreArticles)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbMovRootFolderCheck)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_MovDurationAsRuntine)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbMissingMovie)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbMovThousSeparator)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbMovTitleCase)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbXtraFrodoUrls)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbNoAltTitle)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.Label102)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.Label78)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbPreferredTrailerResolution)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbDlTrailerDuringScrape)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.PanelDisplayRuntime)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_MovDisplayLog)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cb_EnableMediaTags)
        Me.grpbxMovieGeneralOptions.Controls.Add(Me.cbMovieTrailerUrl)
        Me.grpbxMovieGeneralOptions.Location = New System.Drawing.Point(6, 6)
        Me.grpbxMovieGeneralOptions.Name = "grpbxMovieGeneralOptions"
        Me.grpbxMovieGeneralOptions.Size = New System.Drawing.Size(299, 525)
        Me.grpbxMovieGeneralOptions.TabIndex = 73
        Me.grpbxMovieGeneralOptions.TabStop = false
        Me.grpbxMovieGeneralOptions.Text = "General Options"
        '
        'cb_MovRuntimeAsDuration
        '
        Me.cb_MovRuntimeAsDuration.AutoSize = true
        Me.cb_MovRuntimeAsDuration.Location = New System.Drawing.Point(17, 249)
        Me.cb_MovRuntimeAsDuration.Name = "cb_MovRuntimeAsDuration"
        Me.cb_MovRuntimeAsDuration.Size = New System.Drawing.Size(241, 19)
        Me.cb_MovRuntimeAsDuration.TabIndex = 83
        Me.cb_MovRuntimeAsDuration.Text = "or Save Runtime as DurationInSeconds"
        Me.cb_MovRuntimeAsDuration.UseVisualStyleBackColor = true
        '
        'cbShowMovieGridToolTip
        '
        Me.cbShowMovieGridToolTip.AutoSize = true
        Me.cbShowMovieGridToolTip.Location = New System.Drawing.Point(8, 454)
        Me.cbShowMovieGridToolTip.Margin = New System.Windows.Forms.Padding(4)
        Me.cbShowMovieGridToolTip.Name = "cbShowMovieGridToolTip"
        Me.cbShowMovieGridToolTip.Size = New System.Drawing.Size(162, 19)
        Me.cbShowMovieGridToolTip.TabIndex = 96
        Me.cbShowMovieGridToolTip.Text = "Show movie table tool tip"
        Me.cbShowMovieGridToolTip.UseVisualStyleBackColor = true
        '
        'cb_MovSetTitleIgnArticle
        '
        Me.cb_MovSetTitleIgnArticle.AutoSize = true
        Me.cb_MovSetTitleIgnArticle.Location = New System.Drawing.Point(8, 406)
        Me.cb_MovSetTitleIgnArticle.Name = "cb_MovSetTitleIgnArticle"
        Me.cb_MovSetTitleIgnArticle.Size = New System.Drawing.Size(236, 19)
        Me.cb_MovSetTitleIgnArticle.TabIndex = 94
        Me.cb_MovSetTitleIgnArticle.Text = "Enable Ignore Article for MovieSet Title"
        Me.cb_MovSetTitleIgnArticle.UseVisualStyleBackColor = true
        '
        'cb_SorttitleIgnoreArticles
        '
        Me.cb_SorttitleIgnoreArticles.AutoSize = true
        Me.cb_SorttitleIgnoreArticles.Location = New System.Drawing.Point(8, 381)
        Me.cb_SorttitleIgnoreArticles.Name = "cb_SorttitleIgnoreArticles"
        Me.cb_SorttitleIgnoreArticles.Size = New System.Drawing.Size(278, 19)
        Me.cb_SorttitleIgnoreArticles.TabIndex = 93
        Me.cb_SorttitleIgnoreArticles.Text = "On Scrape, Ignored articles to end of Sort Title."
        Me.cb_SorttitleIgnoreArticles.UseVisualStyleBackColor = true
        '
        'cb_MovDurationAsRuntine
        '
        Me.cb_MovDurationAsRuntine.AutoSize = true
        Me.cb_MovDurationAsRuntine.Location = New System.Drawing.Point(17, 228)
        Me.cb_MovDurationAsRuntine.Name = "cb_MovDurationAsRuntine"
        Me.cb_MovDurationAsRuntine.Size = New System.Drawing.Size(210, 19)
        Me.cb_MovDurationAsRuntine.TabIndex = 78
        Me.cb_MovDurationAsRuntine.Text = "Save Media Duration As Runtime."
        Me.cb_MovDurationAsRuntine.UseVisualStyleBackColor = true
        '
        'cbMissingMovie
        '
        Me.cbMissingMovie.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbMissingMovie.Location = New System.Drawing.Point(7, 325)
        Me.cbMissingMovie.Name = "cbMissingMovie"
        Me.cbMissingMovie.Size = New System.Drawing.Size(275, 36)
        Me.cbMissingMovie.TabIndex = 81
        Me.cbMissingMovie.Text = "Include Missing video Files when rebuilding cache."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Valid nfo's but missing video"& _ 
    ", will highlighted in Red."
        Me.cbMissingMovie.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbMissingMovie.UseVisualStyleBackColor = true
        '
        'cbMovThousSeparator
        '
        Me.cbMovThousSeparator.AutoSize = true
        Me.cbMovThousSeparator.Location = New System.Drawing.Point(7, 178)
        Me.cbMovThousSeparator.Name = "cbMovThousSeparator"
        Me.cbMovThousSeparator.Size = New System.Drawing.Size(235, 19)
        Me.cbMovThousSeparator.TabIndex = 77
        Me.cbMovThousSeparator.Text = "Set , as separator in Votes saved to nfo"
        Me.cbMovThousSeparator.UseVisualStyleBackColor = true
        '
        'cbXtraFrodoUrls
        '
        Me.cbXtraFrodoUrls.AutoSize = true
        Me.cbXtraFrodoUrls.Location = New System.Drawing.Point(7, 135)
        Me.cbXtraFrodoUrls.Name = "cbXtraFrodoUrls"
        Me.cbXtraFrodoUrls.Size = New System.Drawing.Size(274, 19)
        Me.cbXtraFrodoUrls.TabIndex = 74
        Me.cbXtraFrodoUrls.Text = "Disable extra Frodo Poster and Thumb URL's"
        Me.cbXtraFrodoUrls.UseVisualStyleBackColor = true
        '
        'cbNoAltTitle
        '
        Me.cbNoAltTitle.AutoSize = true
        Me.cbNoAltTitle.Location = New System.Drawing.Point(7, 114)
        Me.cbNoAltTitle.Name = "cbNoAltTitle"
        Me.cbNoAltTitle.Size = New System.Drawing.Size(235, 19)
        Me.cbNoAltTitle.TabIndex = 73
        Me.cbNoAltTitle.Text = "Exclude adding Alternative Titles to nfo"
        Me.cbNoAltTitle.UseVisualStyleBackColor = true
        '
        'PanelDisplayRuntime
        '
        Me.PanelDisplayRuntime.Controls.Add(Me.cbMovieRuntimeFallbackToFile)
        Me.PanelDisplayRuntime.Controls.Add(Me.Label70)
        Me.PanelDisplayRuntime.Controls.Add(Me.rbRuntimeFile)
        Me.PanelDisplayRuntime.Controls.Add(Me.rbRuntimeScraper)
        Me.PanelDisplayRuntime.Enabled = false
        Me.PanelDisplayRuntime.Location = New System.Drawing.Point(6, 270)
        Me.PanelDisplayRuntime.Name = "PanelDisplayRuntime"
        Me.PanelDisplayRuntime.Size = New System.Drawing.Size(268, 49)
        Me.PanelDisplayRuntime.TabIndex = 68
        '
        'Label70
        '
        Me.Label70.AutoSize = true
        Me.Label70.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label70.Location = New System.Drawing.Point(3, 4)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(128, 15)
        Me.Label70.TabIndex = 65
        Me.Label70.Text = "Display Runtime from:"
        '
        'rbRuntimeFile
        '
        Me.rbRuntimeFile.AutoSize = true
        Me.rbRuntimeFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbRuntimeFile.Location = New System.Drawing.Point(207, 2)
        Me.rbRuntimeFile.Name = "rbRuntimeFile"
        Me.rbRuntimeFile.Size = New System.Drawing.Size(45, 19)
        Me.rbRuntimeFile.TabIndex = 67
        Me.rbRuntimeFile.Text = "File"
        Me.rbRuntimeFile.UseVisualStyleBackColor = true
        '
        'rbRuntimeScraper
        '
        Me.rbRuntimeScraper.AutoSize = true
        Me.rbRuntimeScraper.Checked = true
        Me.rbRuntimeScraper.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbRuntimeScraper.Location = New System.Drawing.Point(133, 2)
        Me.rbRuntimeScraper.Name = "rbRuntimeScraper"
        Me.rbRuntimeScraper.Size = New System.Drawing.Size(68, 19)
        Me.rbRuntimeScraper.TabIndex = 66
        Me.rbRuntimeScraper.TabStop = true
        Me.rbRuntimeScraper.Text = "Scraper"
        Me.rbRuntimeScraper.UseVisualStyleBackColor = true
        '
        'cb_MovDisplayLog
        '
        Me.cb_MovDisplayLog.AutoSize = true
        Me.cb_MovDisplayLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cb_MovDisplayLog.Location = New System.Drawing.Point(7, 156)
        Me.cb_MovDisplayLog.Margin = New System.Windows.Forms.Padding(4)
        Me.cb_MovDisplayLog.Name = "cb_MovDisplayLog"
        Me.cb_MovDisplayLog.Size = New System.Drawing.Size(205, 19)
        Me.cb_MovDisplayLog.TabIndex = 63
        Me.cb_MovDisplayLog.Text = "Display log after scraping Movies"
        Me.cb_MovDisplayLog.UseVisualStyleBackColor = true
        '
        'cb_EnableMediaTags
        '
        Me.cb_EnableMediaTags.AutoSize = true
        Me.cb_EnableMediaTags.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cb_EnableMediaTags.Location = New System.Drawing.Point(7, 204)
        Me.cb_EnableMediaTags.Margin = New System.Windows.Forms.Padding(4)
        Me.cb_EnableMediaTags.Name = "cb_EnableMediaTags"
        Me.cb_EnableMediaTags.Size = New System.Drawing.Size(251, 19)
        Me.cb_EnableMediaTags.TabIndex = 64
        Me.cb_EnableMediaTags.Text = "Enable MC to save Media Tags to nfo file."
        Me.cb_EnableMediaTags.UseVisualStyleBackColor = true
        '
        'cbMovieTrailerUrl
        '
        Me.cbMovieTrailerUrl.AutoSize = true
        Me.cbMovieTrailerUrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMovieTrailerUrl.Location = New System.Drawing.Point(7, 16)
        Me.cbMovieTrailerUrl.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMovieTrailerUrl.Name = "cbMovieTrailerUrl"
        Me.cbMovieTrailerUrl.Size = New System.Drawing.Size(186, 19)
        Me.cbMovieTrailerUrl.TabIndex = 49
        Me.cbMovieTrailerUrl.Text = "Add movie trailer url to nfo file"
        Me.cbMovieTrailerUrl.UseVisualStyleBackColor = true
        '
        'grpbxMovieNameMode
        '
        Me.grpbxMovieNameMode.Controls.Add(Me.Label163)
        Me.grpbxMovieNameMode.Controls.Add(Me.lblNameMode)
        Me.grpbxMovieNameMode.Controls.Add(Me.cbMoviePartsIgnorePart)
        Me.grpbxMovieNameMode.Controls.Add(Me.lblNameModeEg)
        Me.grpbxMovieNameMode.Controls.Add(Me.cbMoviePartsNameMode)
        Me.grpbxMovieNameMode.Location = New System.Drawing.Point(657, 6)
        Me.grpbxMovieNameMode.Name = "grpbxMovieNameMode"
        Me.grpbxMovieNameMode.Size = New System.Drawing.Size(329, 152)
        Me.grpbxMovieNameMode.TabIndex = 78
        Me.grpbxMovieNameMode.TabStop = false
        Me.grpbxMovieNameMode.Text = "Name Mode"
        '
        'Label163
        '
        Me.Label163.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label163.Location = New System.Drawing.Point(102, 119)
        Me.Label163.Name = "Label163"
        Me.Label163.Size = New System.Drawing.Size(187, 33)
        Me.Label163.TabIndex = 82
        Me.Label163.Text = "Disable stacking function for movies containing 'Pt' or 'Part' in the filename."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)& _ 
    ""
        '
        'lblNameMode
        '
        Me.lblNameMode.AutoSize = true
        Me.lblNameMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblNameMode.Location = New System.Drawing.Point(48, 52)
        Me.lblNameMode.Name = "lblNameMode"
        Me.lblNameMode.Size = New System.Drawing.Size(8, 12)
        Me.lblNameMode.TabIndex = 5
        Me.lblNameMode.Text = "."
        '
        'cbMoviePartsIgnorePart
        '
        Me.cbMoviePartsIgnorePart.AutoSize = true
        Me.cbMoviePartsIgnorePart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMoviePartsIgnorePart.Location = New System.Drawing.Point(7, 119)
        Me.cbMoviePartsIgnorePart.Name = "cbMoviePartsIgnorePart"
        Me.cbMoviePartsIgnorePart.Size = New System.Drawing.Size(92, 19)
        Me.cbMoviePartsIgnorePart.TabIndex = 81
        Me.cbMoviePartsIgnorePart.Text = "Ignore 'Part'"
        Me.cbMoviePartsIgnorePart.UseVisualStyleBackColor = true
        '
        'lblNameModeEg
        '
        Me.lblNameModeEg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblNameModeEg.Location = New System.Drawing.Point(22, 49)
        Me.lblNameModeEg.Margin = New System.Windows.Forms.Padding(0)
        Me.lblNameModeEg.Name = "lblNameModeEg"
        Me.lblNameModeEg.Size = New System.Drawing.Size(25, 20)
        Me.lblNameModeEg.TabIndex = 4
        Me.lblNameModeEg.Text = "eg."
        '
        'cbMoviePartsNameMode
        '
        Me.cbMoviePartsNameMode.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbMoviePartsNameMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbMoviePartsNameMode.Location = New System.Drawing.Point(7, 16)
        Me.cbMoviePartsNameMode.Name = "cbMoviePartsNameMode"
        Me.cbMoviePartsNameMode.Size = New System.Drawing.Size(257, 34)
        Me.cbMoviePartsNameMode.TabIndex = 3
        Me.cbMoviePartsNameMode.Text = "Determines how anciliary files for multi-part movies are labelled."
        Me.cbMoviePartsNameMode.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.cbMoviePartsNameMode.UseVisualStyleBackColor = true
        '
        'tpMoviePreferences_Advanced
        '
        Me.tpMoviePreferences_Advanced.AutoScroll = true
        Me.tpMoviePreferences_Advanced.AutoScrollMinSize = New System.Drawing.Size(928, 370)
        Me.tpMoviePreferences_Advanced.BackColor = System.Drawing.SystemColors.Control
        Me.tpMoviePreferences_Advanced.Controls.Add(Me.cbMovAllowNonImdbIdAsId)
        Me.tpMoviePreferences_Advanced.Controls.Add(Me.gpbxMovieExcludeYearSearch)
        Me.tpMoviePreferences_Advanced.Controls.Add(Me.gpbx_MovTimerAutoScrape)
        Me.tpMoviePreferences_Advanced.Controls.Add(Me.grpbxMovieCustomPlaycountTag)
        Me.tpMoviePreferences_Advanced.Controls.Add(Me.grpbxMovieSeparateIdentifier)
        Me.tpMoviePreferences_Advanced.Controls.Add(Me.grpbxMovieNfoPosterOptions)
        Me.tpMoviePreferences_Advanced.Location = New System.Drawing.Point(4, 24)
        Me.tpMoviePreferences_Advanced.Margin = New System.Windows.Forms.Padding(4)
        Me.tpMoviePreferences_Advanced.Name = "tpMoviePreferences_Advanced"
        Me.tpMoviePreferences_Advanced.Padding = New System.Windows.Forms.Padding(4)
        Me.tpMoviePreferences_Advanced.Size = New System.Drawing.Size(184, 46)
        Me.tpMoviePreferences_Advanced.TabIndex = 1
        Me.tpMoviePreferences_Advanced.Text = "Advanced"
        '
        'gpbxMovieExcludeYearSearch
        '
        Me.gpbxMovieExcludeYearSearch.Controls.Add(Me.cbMovieExcludeYearSearch)
        Me.gpbxMovieExcludeYearSearch.Controls.Add(Me.Label31)
        Me.gpbxMovieExcludeYearSearch.Location = New System.Drawing.Point(10, 187)
        Me.gpbxMovieExcludeYearSearch.Name = "gpbxMovieExcludeYearSearch"
        Me.gpbxMovieExcludeYearSearch.Size = New System.Drawing.Size(389, 106)
        Me.gpbxMovieExcludeYearSearch.TabIndex = 79
        Me.gpbxMovieExcludeYearSearch.TabStop = false
        Me.gpbxMovieExcludeYearSearch.Text = "Exclude Year of Movie from Title search."
        '
        'cbMovieExcludeYearSearch
        '
        Me.cbMovieExcludeYearSearch.AutoSize = true
        Me.cbMovieExcludeYearSearch.Location = New System.Drawing.Point(11, 68)
        Me.cbMovieExcludeYearSearch.Name = "cbMovieExcludeYearSearch"
        Me.cbMovieExcludeYearSearch.Size = New System.Drawing.Size(209, 19)
        Me.cbMovieExcludeYearSearch.TabIndex = 1
        Me.cbMovieExcludeYearSearch.Text = "Exclude year from Scrape search."
        Me.cbMovieExcludeYearSearch.UseVisualStyleBackColor = true
        '
        'Label31
        '
        Me.Label31.AutoSize = true
        Me.Label31.Location = New System.Drawing.Point(10, 19)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(373, 45)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = "Selecting this option will exclude searching by Movie Title and Year."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Caution:  "& _ 
    "Successful search results will be reduced."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Only use if you have no other choice"& _ 
    "!"
        '
        'gpbx_MovTimerAutoScrape
        '
        Me.gpbx_MovTimerAutoScrape.Controls.Add(Me.tbMovAutoScrapeInterval)
        Me.gpbx_MovTimerAutoScrape.Controls.Add(Me.Label29)
        Me.gpbx_MovTimerAutoScrape.Controls.Add(Me.cbMovEnableAutoScrape)
        Me.gpbx_MovTimerAutoScrape.Location = New System.Drawing.Point(10, 7)
        Me.gpbx_MovTimerAutoScrape.Name = "gpbx_MovTimerAutoScrape"
        Me.gpbx_MovTimerAutoScrape.Size = New System.Drawing.Size(389, 83)
        Me.gpbx_MovTimerAutoScrape.TabIndex = 78
        Me.gpbx_MovTimerAutoScrape.TabStop = false
        Me.gpbx_MovTimerAutoScrape.Text = "AutoScrape Timer"
        '
        'tbMovAutoScrapeInterval
        '
        Me.tbMovAutoScrapeInterval.Location = New System.Drawing.Point(33, 45)
        Me.tbMovAutoScrapeInterval.Name = "tbMovAutoScrapeInterval"
        Me.tbMovAutoScrapeInterval.Size = New System.Drawing.Size(56, 21)
        Me.tbMovAutoScrapeInterval.TabIndex = 2
        '
        'Label29
        '
        Me.Label29.AutoSize = true
        Me.Label29.Location = New System.Drawing.Point(95, 48)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(51, 15)
        Me.Label29.TabIndex = 1
        Me.Label29.Text = "Minutes"
        '
        'cbMovEnableAutoScrape
        '
        Me.cbMovEnableAutoScrape.AutoSize = true
        Me.cbMovEnableAutoScrape.Location = New System.Drawing.Point(13, 20)
        Me.cbMovEnableAutoScrape.Name = "cbMovEnableAutoScrape"
        Me.cbMovEnableAutoScrape.Size = New System.Drawing.Size(168, 19)
        Me.cbMovEnableAutoScrape.TabIndex = 0
        Me.cbMovEnableAutoScrape.Text = "AutoScrape Movies every :"
        Me.cbMovEnableAutoScrape.UseVisualStyleBackColor = true
        '
        'grpbxMovieCustomPlaycountTag
        '
        Me.grpbxMovieCustomPlaycountTag.Controls.Add(Me.cbMovNfoWatchTag)
        Me.grpbxMovieCustomPlaycountTag.Controls.Add(Me.Label23)
        Me.grpbxMovieCustomPlaycountTag.Location = New System.Drawing.Point(10, 96)
        Me.grpbxMovieCustomPlaycountTag.Name = "grpbxMovieCustomPlaycountTag"
        Me.grpbxMovieCustomPlaycountTag.Size = New System.Drawing.Size(389, 76)
        Me.grpbxMovieCustomPlaycountTag.TabIndex = 77
        Me.grpbxMovieCustomPlaycountTag.TabStop = false
        Me.grpbxMovieCustomPlaycountTag.Text = "Custom playcount tag"
        '
        'cbMovNfoWatchTag
        '
        Me.cbMovNfoWatchTag.AutoSize = true
        Me.cbMovNfoWatchTag.Location = New System.Drawing.Point(11, 35)
        Me.cbMovNfoWatchTag.Name = "cbMovNfoWatchTag"
        Me.cbMovNfoWatchTag.Size = New System.Drawing.Size(222, 19)
        Me.cbMovNfoWatchTag.TabIndex = 1
        Me.cbMovNfoWatchTag.Text = "Enable <watched> tag saving to nfo."
        Me.cbMovNfoWatchTag.UseVisualStyleBackColor = true
        '
        'Label23
        '
        Me.Label23.AutoSize = true
        Me.Label23.Location = New System.Drawing.Point(8, 17)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(320, 15)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "Save tag <watched> in movie nfo, mirroring playcount tag."
        '
        'grpbxMovieSeparateIdentifier
        '
        Me.grpbxMovieSeparateIdentifier.Controls.Add(Me.btn_MovSepReset)
        Me.grpbxMovieSeparateIdentifier.Controls.Add(Me.Label198)
        Me.grpbxMovieSeparateIdentifier.Controls.Add(Me.btn_MovSepRem)
        Me.grpbxMovieSeparateIdentifier.Controls.Add(Me.btn_MovSepAdd)
        Me.grpbxMovieSeparateIdentifier.Controls.Add(Me.tb_MovSeptb)
        Me.grpbxMovieSeparateIdentifier.Controls.Add(Me.lb_MovSepLst)
        Me.grpbxMovieSeparateIdentifier.Location = New System.Drawing.Point(427, 7)
        Me.grpbxMovieSeparateIdentifier.Name = "grpbxMovieSeparateIdentifier"
        Me.grpbxMovieSeparateIdentifier.Size = New System.Drawing.Size(387, 354)
        Me.grpbxMovieSeparateIdentifier.TabIndex = 76
        Me.grpbxMovieSeparateIdentifier.TabStop = false
        Me.grpbxMovieSeparateIdentifier.Text = "Separate Movie Identifyer"
        '
        'btn_MovSepReset
        '
        Me.btn_MovSepReset.Location = New System.Drawing.Point(215, 236)
        Me.btn_MovSepReset.Name = "btn_MovSepReset"
        Me.btn_MovSepReset.Size = New System.Drawing.Size(75, 23)
        Me.btn_MovSepReset.TabIndex = 5
        Me.btn_MovSepReset.Text = "Reset List"
        Me.btn_MovSepReset.UseVisualStyleBackColor = true
        '
        'Label198
        '
        Me.Label198.Location = New System.Drawing.Point(203, 20)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(178, 226)
        Me.Label198.TabIndex = 4
        Me.Label198.Text = resources.GetString("Label198.Text")
        '
        'btn_MovSepRem
        '
        Me.btn_MovSepRem.Location = New System.Drawing.Point(215, 286)
        Me.btn_MovSepRem.Name = "btn_MovSepRem"
        Me.btn_MovSepRem.Size = New System.Drawing.Size(75, 23)
        Me.btn_MovSepRem.TabIndex = 3
        Me.btn_MovSepRem.Text = "Remove"
        Me.btn_MovSepRem.UseVisualStyleBackColor = true
        '
        'btn_MovSepAdd
        '
        Me.btn_MovSepAdd.Location = New System.Drawing.Point(215, 320)
        Me.btn_MovSepAdd.Name = "btn_MovSepAdd"
        Me.btn_MovSepAdd.Size = New System.Drawing.Size(75, 23)
        Me.btn_MovSepAdd.TabIndex = 2
        Me.btn_MovSepAdd.Text = "Add"
        Me.btn_MovSepAdd.UseVisualStyleBackColor = true
        '
        'tb_MovSeptb
        '
        Me.tb_MovSeptb.Location = New System.Drawing.Point(6, 321)
        Me.tb_MovSeptb.Name = "tb_MovSeptb"
        Me.tb_MovSeptb.Size = New System.Drawing.Size(191, 21)
        Me.tb_MovSeptb.TabIndex = 1
        '
        'lb_MovSepLst
        '
        Me.lb_MovSepLst.FormattingEnabled = true
        Me.lb_MovSepLst.ItemHeight = 15
        Me.lb_MovSepLst.Location = New System.Drawing.Point(6, 20)
        Me.lb_MovSepLst.Name = "lb_MovSepLst"
        Me.lb_MovSepLst.Size = New System.Drawing.Size(191, 289)
        Me.lb_MovSepLst.TabIndex = 0
        '
        'grpbxMovieNfoPosterOptions
        '
        Me.grpbxMovieNfoPosterOptions.Controls.Add(Me.Label88)
        Me.grpbxMovieNfoPosterOptions.Controls.Add(Me.imdb_chk)
        Me.grpbxMovieNfoPosterOptions.Controls.Add(Me.mpdb_chk)
        Me.grpbxMovieNfoPosterOptions.Controls.Add(Me.tmdb_chk)
        Me.grpbxMovieNfoPosterOptions.Controls.Add(Me.IMPA_chk)
        Me.grpbxMovieNfoPosterOptions.Controls.Add(Me.Label89)
        Me.grpbxMovieNfoPosterOptions.Location = New System.Drawing.Point(427, 383)
        Me.grpbxMovieNfoPosterOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.grpbxMovieNfoPosterOptions.Name = "grpbxMovieNfoPosterOptions"
        Me.grpbxMovieNfoPosterOptions.Padding = New System.Windows.Forms.Padding(4)
        Me.grpbxMovieNfoPosterOptions.Size = New System.Drawing.Size(463, 115)
        Me.grpbxMovieNfoPosterOptions.TabIndex = 49
        Me.grpbxMovieNfoPosterOptions.TabStop = false
        Me.grpbxMovieNfoPosterOptions.Text = "nfo Poster Options"
        '
        'Label88
        '
        Me.Label88.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label88.Location = New System.Drawing.Point(8, 78)
        Me.Label88.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(447, 28)
        Me.Label88.TabIndex = 5
        Me.Label88.Text = "Urls added to the nfo file can be browsed from within XBMC. Each option added wil"& _ 
    "l slow down the scrape and rescrape functions."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)
        '
        'imdb_chk
        '
        Me.imdb_chk.AutoSize = true
        Me.imdb_chk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.imdb_chk.Location = New System.Drawing.Point(84, 55)
        Me.imdb_chk.Margin = New System.Windows.Forms.Padding(4)
        Me.imdb_chk.Name = "imdb_chk"
        Me.imdb_chk.Size = New System.Drawing.Size(57, 19)
        Me.imdb_chk.TabIndex = 4
        Me.imdb_chk.Text = "IMDB"
        Me.imdb_chk.UseVisualStyleBackColor = true
        '
        'mpdb_chk
        '
        Me.mpdb_chk.AutoSize = true
        Me.mpdb_chk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.mpdb_chk.Location = New System.Drawing.Point(84, 36)
        Me.mpdb_chk.Margin = New System.Windows.Forms.Padding(4)
        Me.mpdb_chk.Name = "mpdb_chk"
        Me.mpdb_chk.Size = New System.Drawing.Size(62, 19)
        Me.mpdb_chk.TabIndex = 3
        Me.mpdb_chk.Text = "MPDB"
        Me.mpdb_chk.UseVisualStyleBackColor = true
        '
        'tmdb_chk
        '
        Me.tmdb_chk.AutoSize = true
        Me.tmdb_chk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tmdb_chk.Location = New System.Drawing.Point(11, 55)
        Me.tmdb_chk.Margin = New System.Windows.Forms.Padding(4)
        Me.tmdb_chk.Name = "tmdb_chk"
        Me.tmdb_chk.Size = New System.Drawing.Size(61, 19)
        Me.tmdb_chk.TabIndex = 2
        Me.tmdb_chk.Text = "TMDB"
        Me.tmdb_chk.UseVisualStyleBackColor = true
        '
        'IMPA_chk
        '
        Me.IMPA_chk.AutoSize = true
        Me.IMPA_chk.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.IMPA_chk.Location = New System.Drawing.Point(11, 36)
        Me.IMPA_chk.Margin = New System.Windows.Forms.Padding(4)
        Me.IMPA_chk.Name = "IMPA_chk"
        Me.IMPA_chk.Size = New System.Drawing.Size(55, 19)
        Me.IMPA_chk.TabIndex = 1
        Me.IMPA_chk.Text = "IMPA"
        Me.IMPA_chk.UseVisualStyleBackColor = true
        '
        'Label89
        '
        Me.Label89.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label89.Location = New System.Drawing.Point(8, 18)
        Me.Label89.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(447, 13)
        Me.Label89.TabIndex = 0
        Me.Label89.Text = "During autoscrape and rescrape, add poster urls to the nfo file from the followin"& _ 
    "g sources:"
        '
        'TPTVPref
        '
        Me.TPTVPref.Controls.Add(Me.TabControl6)
        Me.TPTVPref.Location = New System.Drawing.Point(4, 24)
        Me.TPTVPref.Name = "TPTVPref"
        Me.TPTVPref.Size = New System.Drawing.Size(1000, 595)
        Me.TPTVPref.TabIndex = 8
        Me.TPTVPref.Text = "TV Preferences"
        Me.TPTVPref.UseVisualStyleBackColor = true
        '
        'TabControl6
        '
        Me.TabControl6.Controls.Add(Me.TabPage30)
        Me.TabControl6.Controls.Add(Me.TabPage31)
        Me.TabControl6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl6.Location = New System.Drawing.Point(0, 0)
        Me.TabControl6.Margin = New System.Windows.Forms.Padding(4)
        Me.TabControl6.Name = "TabControl6"
        Me.TabControl6.SelectedIndex = 0
        Me.TabControl6.Size = New System.Drawing.Size(1000, 595)
        Me.TabControl6.TabIndex = 16
        '
        'TabPage30
        '
        Me.TabPage30.Controls.Add(Me.GroupBox17)
        Me.TabPage30.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TabPage30.Location = New System.Drawing.Point(4, 24)
        Me.TabPage30.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage30.Name = "TabPage30"
        Me.TabPage30.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage30.Size = New System.Drawing.Size(992, 567)
        Me.TabPage30.TabIndex = 0
        Me.TabPage30.Text = "General / Scraper"
        Me.TabPage30.UseVisualStyleBackColor = true
        '
        'GroupBox17
        '
        Me.GroupBox17.Controls.Add(Me.Label32)
        Me.GroupBox17.Controls.Add(Me.Label28)
        Me.GroupBox17.Controls.Add(Me.tbTvAutoScrapeInterval)
        Me.GroupBox17.Controls.Add(Me.cbTvEnableAutoScrape)
        Me.GroupBox17.Controls.Add(Me.cbTvThousSeparator)
        Me.GroupBox17.Controls.Add(Me.cbtvDisplayNextAiredToolTip)
        Me.GroupBox17.Controls.Add(Me.cbTvScrShtTVDBResize)
        Me.GroupBox17.Controls.Add(Me.GroupBox43)
        Me.GroupBox17.Controls.Add(Me.Label111)
        Me.GroupBox17.Controls.Add(Me.cbTv_fixNFOid)
        Me.GroupBox17.Controls.Add(Me.GroupBox22)
        Me.GroupBox17.Controls.Add(Me.cbTvAutoScreenShot)
        Me.GroupBox17.Controls.Add(Me.cbTvUse_XBMC_TVDB_Scraper)
        Me.GroupBox17.Controls.Add(Me.Label139)
        Me.GroupBox17.Controls.Add(Me.GroupBox20)
        Me.GroupBox17.Controls.Add(Me.cbTvQuickAddShow)
        Me.GroupBox17.Controls.Add(Me.GroupBox1)
        Me.GroupBox17.Controls.Add(Me.cbTvEpEnableHDTags)
        Me.GroupBox17.Controls.Add(Me.cbTvDisableLogs)
        Me.GroupBox17.Controls.Add(Me.lbxTVDBLangs)
        Me.GroupBox17.Controls.Add(Me.Label122)
        Me.GroupBox17.Controls.Add(Me.ComboBox8)
        Me.GroupBox17.Controls.Add(Me.GroupBox_TVDB_Scraper_Preferences)
        Me.GroupBox17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox17.Location = New System.Drawing.Point(4, 4)
        Me.GroupBox17.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox17.Name = "GroupBox17"
        Me.GroupBox17.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox17.Size = New System.Drawing.Size(984, 559)
        Me.GroupBox17.TabIndex = 22
        Me.GroupBox17.TabStop = false
        Me.GroupBox17.Text = "Default TV Scraper Settings"
        '
        'Label32
        '
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 7!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label32.Location = New System.Drawing.Point(444, 496)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(179, 30)
        Me.Label32.TabIndex = 79
        Me.Label32.Text = "For 16:9 means saving at 400x225"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"For 4:3 means saving at 400x300"
        '
        'Label28
        '
        Me.Label28.AutoSize = true
        Me.Label28.Location = New System.Drawing.Point(214, 459)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(51, 15)
        Me.Label28.TabIndex = 78
        Me.Label28.Text = "Minutes"
        '
        'tbTvAutoScrapeInterval
        '
        Me.tbTvAutoScrapeInterval.Location = New System.Drawing.Point(161, 456)
        Me.tbTvAutoScrapeInterval.Name = "tbTvAutoScrapeInterval"
        Me.tbTvAutoScrapeInterval.Size = New System.Drawing.Size(47, 21)
        Me.tbTvAutoScrapeInterval.TabIndex = 77
        '
        'cbTvEnableAutoScrape
        '
        Me.cbTvEnableAutoScrape.AutoSize = true
        Me.cbTvEnableAutoScrape.Location = New System.Drawing.Point(132, 431)
        Me.cbTvEnableAutoScrape.Name = "cbTvEnableAutoScrape"
        Me.cbTvEnableAutoScrape.Size = New System.Drawing.Size(194, 19)
        Me.cbTvEnableAutoScrape.TabIndex = 76
        Me.cbTvEnableAutoScrape.Text = "AutoScrape Tv episodes every :"
        Me.cbTvEnableAutoScrape.UseVisualStyleBackColor = true
        '
        'cbTvThousSeparator
        '
        Me.cbTvThousSeparator.AutoSize = true
        Me.cbTvThousSeparator.Location = New System.Drawing.Point(132, 402)
        Me.cbTvThousSeparator.Name = "cbTvThousSeparator"
        Me.cbTvThousSeparator.Size = New System.Drawing.Size(235, 19)
        Me.cbTvThousSeparator.TabIndex = 75
        Me.cbTvThousSeparator.Text = "Set , as separator in Votes saved to nfo"
        Me.cbTvThousSeparator.UseVisualStyleBackColor = true
        '
        'cbTvScrShtTVDBResize
        '
        Me.cbTvScrShtTVDBResize.AutoSize = true
        Me.cbTvScrShtTVDBResize.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbTvScrShtTVDBResize.Location = New System.Drawing.Point(411, 477)
        Me.cbTvScrShtTVDBResize.Name = "cbTvScrShtTVDBResize"
        Me.cbTvScrShtTVDBResize.Size = New System.Drawing.Size(235, 19)
        Me.cbTvScrShtTVDBResize.TabIndex = 73
        Me.cbTvScrShtTVDBResize.Text = "Save Screenshot in TVDB friendly size."
        Me.cbTvScrShtTVDBResize.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cbTvScrShtTVDBResize.UseVisualStyleBackColor = true
        '
        'GroupBox43
        '
        Me.GroupBox43.Controls.Add(Me.cb_TvMissingEpOffset)
        Me.GroupBox43.Controls.Add(Me.cbTvMissingSpecials)
        Me.GroupBox43.Location = New System.Drawing.Point(712, 391)
        Me.GroupBox43.Name = "GroupBox43"
        Me.GroupBox43.Size = New System.Drawing.Size(264, 72)
        Me.GroupBox43.TabIndex = 72
        Me.GroupBox43.TabStop = false
        Me.GroupBox43.Text = "Missing Episode Options"
        '
        'cb_TvMissingEpOffset
        '
        Me.cb_TvMissingEpOffset.AutoSize = true
        Me.cb_TvMissingEpOffset.Location = New System.Drawing.Point(14, 45)
        Me.cb_TvMissingEpOffset.Name = "cb_TvMissingEpOffset"
        Me.cb_TvMissingEpOffset.Size = New System.Drawing.Size(181, 19)
        Me.cb_TvMissingEpOffset.TabIndex = 1
        Me.cb_TvMissingEpOffset.Text = "Delay missing aired by 1 day"
        Me.cb_TvMissingEpOffset.UseVisualStyleBackColor = true
        '
        'cbTvMissingSpecials
        '
        Me.cbTvMissingSpecials.AutoSize = true
        Me.cbTvMissingSpecials.Location = New System.Drawing.Point(14, 20)
        Me.cbTvMissingSpecials.Name = "cbTvMissingSpecials"
        Me.cbTvMissingSpecials.Size = New System.Drawing.Size(172, 19)
        Me.cbTvMissingSpecials.TabIndex = 0
        Me.cbTvMissingSpecials.Text = "Ignore specials (Season 0)"
        Me.cbTvMissingSpecials.UseVisualStyleBackColor = true
        '
        'Label111
        '
        Me.Label111.Location = New System.Drawing.Point(717, 482)
        Me.Label111.Name = "Label111"
        Me.Label111.Size = New System.Drawing.Size(223, 32)
        Me.Label111.TabIndex = 69
        Me.Label111.Text = "Enable this  to correct the <ID> field in tvshow.nfo (non-persistant)."
        '
        'cbTv_fixNFOid
        '
        Me.cbTv_fixNFOid.AutoSize = true
        Me.cbTv_fixNFOid.Location = New System.Drawing.Point(720, 517)
        Me.cbTv_fixNFOid.Name = "cbTv_fixNFOid"
        Me.cbTv_fixNFOid.Size = New System.Drawing.Size(198, 19)
        Me.cbTv_fixNFOid.TabIndex = 68
        Me.cbTv_fixNFOid.Text = "Fix NFO id during cache refresh"
        Me.cbTv_fixNFOid.UseVisualStyleBackColor = true
        '
        'GroupBox22
        '
        Me.GroupBox22.Controls.Add(Me.cbtvdbIMDbRating)
        Me.GroupBox22.Controls.Add(Me.btnTVDBGetLanguages)
        Me.GroupBox22.Controls.Add(Me.rbTvEpSortDefault)
        Me.GroupBox22.Controls.Add(Me.rbTvEpSortDVD)
        Me.GroupBox22.Controls.Add(Me.Label124)
        Me.GroupBox22.Controls.Add(Me.Label123)
        Me.GroupBox22.Controls.Add(Me.Label138)
        Me.GroupBox22.Controls.Add(Me.cbTvSeriesActorstoEpisodeActors)
        Me.GroupBox22.Location = New System.Drawing.Point(124, 20)
        Me.GroupBox22.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox22.Name = "GroupBox22"
        Me.GroupBox22.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox22.Size = New System.Drawing.Size(270, 275)
        Me.GroupBox22.TabIndex = 66
        Me.GroupBox22.TabStop = false
        '
        'cbtvdbIMDbRating
        '
        Me.cbtvdbIMDbRating.Location = New System.Drawing.Point(12, 128)
        Me.cbtvdbIMDbRating.Name = "cbtvdbIMDbRating"
        Me.cbtvdbIMDbRating.Size = New System.Drawing.Size(228, 39)
        Me.cbtvdbIMDbRating.TabIndex = 34
        Me.cbtvdbIMDbRating.Text = "Series and Episode Rating and votes from IMDb (fallback to TVDb)"
        Me.cbtvdbIMDbRating.UseVisualStyleBackColor = true
        '
        'btnTVDBGetLanguages
        '
        Me.btnTVDBGetLanguages.Location = New System.Drawing.Point(8, 15)
        Me.btnTVDBGetLanguages.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTVDBGetLanguages.Name = "btnTVDBGetLanguages"
        Me.btnTVDBGetLanguages.Size = New System.Drawing.Size(208, 29)
        Me.btnTVDBGetLanguages.TabIndex = 1
        Me.btnTVDBGetLanguages.Text = "Get Languages From TVDB"
        Me.btnTVDBGetLanguages.UseVisualStyleBackColor = true
        '
        'rbTvEpSortDefault
        '
        Me.rbTvEpSortDefault.AutoSize = true
        Me.rbTvEpSortDefault.Checked = true
        Me.rbTvEpSortDefault.Location = New System.Drawing.Point(18, 91)
        Me.rbTvEpSortDefault.Margin = New System.Windows.Forms.Padding(4)
        Me.rbTvEpSortDefault.Name = "rbTvEpSortDefault"
        Me.rbTvEpSortDefault.Size = New System.Drawing.Size(89, 19)
        Me.rbTvEpSortDefault.TabIndex = 2
        Me.rbTvEpSortDefault.TabStop = true
        Me.rbTvEpSortDefault.Text = "Use Default"
        Me.rbTvEpSortDefault.UseVisualStyleBackColor = true
        '
        'rbTvEpSortDVD
        '
        Me.rbTvEpSortDVD.AutoSize = true
        Me.rbTvEpSortDVD.Location = New System.Drawing.Point(18, 111)
        Me.rbTvEpSortDVD.Margin = New System.Windows.Forms.Padding(4)
        Me.rbTvEpSortDVD.Name = "rbTvEpSortDVD"
        Me.rbTvEpSortDVD.Size = New System.Drawing.Size(92, 19)
        Me.rbTvEpSortDVD.TabIndex = 3
        Me.rbTvEpSortDVD.Text = "DVD Sorting"
        Me.rbTvEpSortDVD.UseVisualStyleBackColor = true
        '
        'Label124
        '
        Me.Label124.AutoSize = true
        Me.Label124.Location = New System.Drawing.Point(14, 72)
        Me.Label124.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(111, 15)
        Me.Label124.TabIndex = 4
        Me.Label124.Text = "Episode Sort Order"
        '
        'Label123
        '
        Me.Label123.AutoSize = true
        Me.Label123.Location = New System.Drawing.Point(15, 52)
        Me.Label123.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label123.Name = "Label123"
        Me.Label123.Size = New System.Drawing.Size(194, 15)
        Me.Label123.TabIndex = 5
        Me.Label123.Text = "If not selected English will be used"
        '
        'Label138
        '
        Me.Label138.AutoSize = true
        Me.Label138.Location = New System.Drawing.Point(5, 168)
        Me.Label138.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label138.Name = "Label138"
        Me.Label138.Size = New System.Drawing.Size(261, 60)
        Me.Label138.TabIndex = 32
        Me.Label138.Text = "This preference gives you the option to copy an"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"actors thumb, if available, to t"& _ 
    "he episodes actor"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"folder, allowing the higher quality TVDB actor"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"thumb to be u"& _ 
    "sed for the episode."
        '
        'cbTvSeriesActorstoEpisodeActors
        '
        Me.cbTvSeriesActorstoEpisodeActors.AutoSize = true
        Me.cbTvSeriesActorstoEpisodeActors.Location = New System.Drawing.Point(12, 235)
        Me.cbTvSeriesActorstoEpisodeActors.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvSeriesActorstoEpisodeActors.Name = "cbTvSeriesActorstoEpisodeActors"
        Me.cbTvSeriesActorstoEpisodeActors.Size = New System.Drawing.Size(224, 34)
        Me.cbTvSeriesActorstoEpisodeActors.TabIndex = 33
        Me.cbTvSeriesActorstoEpisodeActors.Text = "Enable TV Show Actor Thumbs to be"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"copied for Episode Actor Thumbs "
        Me.cbTvSeriesActorstoEpisodeActors.UseVisualStyleBackColor = true
        '
        'cbTvAutoScreenShot
        '
        Me.cbTvAutoScreenShot.AutoSize = true
        Me.cbTvAutoScreenShot.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvAutoScreenShot.Location = New System.Drawing.Point(411, 457)
        Me.cbTvAutoScreenShot.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvAutoScreenShot.Name = "cbTvAutoScreenShot"
        Me.cbTvAutoScreenShot.Size = New System.Drawing.Size(296, 19)
        Me.cbTvAutoScreenShot.TabIndex = 37
        Me.cbTvAutoScreenShot.Text = "Auto create screenshot if TVDB does not have one"
        Me.cbTvAutoScreenShot.UseVisualStyleBackColor = true
        '
        'cbTvUse_XBMC_TVDB_Scraper
        '
        Me.cbTvUse_XBMC_TVDB_Scraper.AutoSize = true
        Me.cbTvUse_XBMC_TVDB_Scraper.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvUse_XBMC_TVDB_Scraper.Location = New System.Drawing.Point(132, 304)
        Me.cbTvUse_XBMC_TVDB_Scraper.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvUse_XBMC_TVDB_Scraper.Name = "cbTvUse_XBMC_TVDB_Scraper"
        Me.cbTvUse_XBMC_TVDB_Scraper.Size = New System.Drawing.Size(195, 19)
        Me.cbTvUse_XBMC_TVDB_Scraper.TabIndex = 65
        Me.cbTvUse_XBMC_TVDB_Scraper.Text = "Use XBMC ""TheTVDB"" Scraper"
        Me.cbTvUse_XBMC_TVDB_Scraper.UseVisualStyleBackColor = true
        '
        'Label139
        '
        Me.Label139.Font = New System.Drawing.Font("Microsoft Sans Serif", 7!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label139.Location = New System.Drawing.Point(401, 410)
        Me.Label139.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label139.Name = "Label139"
        Me.Label139.Size = New System.Drawing.Size(306, 43)
        Me.Label139.TabIndex = 36
        Me.Label139.Text = "Selecting 'Quick Add' will speed up the scraping of New Series, by not downloadin"& _ 
    "g Artwork.  All Series art can be scraped by using the TV context menu item 'Dow"& _ 
    "nload missing art'."
        '
        'GroupBox20
        '
        Me.GroupBox20.Controls.Add(Me.cbTvEpSaveNfoEmpty)
        Me.GroupBox20.Controls.Add(Me.cbTvDlEpisodeThumb)
        Me.GroupBox20.Controls.Add(Me.Label24)
        Me.GroupBox20.Controls.Add(Me.cmbxTvMaxGenres)
        Me.GroupBox20.Controls.Add(Me.Label7)
        Me.GroupBox20.Controls.Add(Me.cmbxTvXtraFanartQty)
        Me.GroupBox20.Controls.Add(Me.cbTvFanartTvFirst)
        Me.GroupBox20.Controls.Add(Me.cbTvDlFanartTvArt)
        Me.GroupBox20.Controls.Add(Me.cbSeasonFolderjpg)
        Me.GroupBox20.Controls.Add(Me.cb_TvFolderJpg)
        Me.GroupBox20.Controls.Add(Me.cbTvDlXtraFanart)
        Me.GroupBox20.Controls.Add(Me.GroupBox18)
        Me.GroupBox20.Controls.Add(Me.cbTvDlPosterArt)
        Me.GroupBox20.Controls.Add(Me.cbTvDlSeasonArt)
        Me.GroupBox20.Controls.Add(Me.cbTvDlFanart)
        Me.GroupBox20.Controls.Add(Me.GroupBox19)
        Me.GroupBox20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.GroupBox20.Location = New System.Drawing.Point(402, 20)
        Me.GroupBox20.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox20.Name = "GroupBox20"
        Me.GroupBox20.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox20.Size = New System.Drawing.Size(306, 363)
        Me.GroupBox20.TabIndex = 35
        Me.GroupBox20.TabStop = false
        Me.GroupBox20.Text = "TV Show Selector / Auto Scraper Default settings"
        '
        'cbTvDlEpisodeThumb
        '
        Me.cbTvDlEpisodeThumb.AutoSize = true
        Me.cbTvDlEpisodeThumb.Location = New System.Drawing.Point(10, 79)
        Me.cbTvDlEpisodeThumb.Name = "cbTvDlEpisodeThumb"
        Me.cbTvDlEpisodeThumb.Size = New System.Drawing.Size(192, 19)
        Me.cbTvDlEpisodeThumb.TabIndex = 78
        Me.cbTvDlEpisodeThumb.Text = "Download Episode Thumbnail"
        Me.cbTvDlEpisodeThumb.UseVisualStyleBackColor = true
        '
        'Label24
        '
        Me.Label24.AutoSize = true
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label24.Location = New System.Drawing.Point(8, 306)
        Me.Label24.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(134, 15)
        Me.Label24.TabIndex = 77
        Me.Label24.Text = "Max number of genres:"
        '
        'Label7
        '
        Me.Label7.AutoSize = true
        Me.Label7.Location = New System.Drawing.Point(157, 103)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(82, 15)
        Me.Label7.TabIndex = 75
        Me.Label7.Text = "........Quantity?"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cmbxTvXtraFanartQty
        '
        Me.cmbxTvXtraFanartQty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxTvXtraFanartQty.FormattingEnabled = true
        Me.cmbxTvXtraFanartQty.Items.AddRange(New Object() {"5", "10", "15", "20"})
        Me.cmbxTvXtraFanartQty.Location = New System.Drawing.Point(244, 95)
        Me.cmbxTvXtraFanartQty.Name = "cmbxTvXtraFanartQty"
        Me.cmbxTvXtraFanartQty.Size = New System.Drawing.Size(55, 23)
        Me.cmbxTvXtraFanartQty.TabIndex = 74
        '
        'cbTvFanartTvFirst
        '
        Me.cbTvFanartTvFirst.AutoSize = true
        Me.cbTvFanartTvFirst.Location = New System.Drawing.Point(171, 121)
        Me.cbTvFanartTvFirst.Name = "cbTvFanartTvFirst"
        Me.cbTvFanartTvFirst.Size = New System.Drawing.Size(110, 34)
        Me.cbTvFanartTvFirst.TabIndex = 73
        Me.cbTvFanartTvFirst.Text = "Fanart.tv before"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"TVDB Artwork"
        Me.cbTvFanartTvFirst.UseVisualStyleBackColor = true
        '
        'cbTvDlFanartTvArt
        '
        Me.cbTvDlFanartTvArt.AutoSize = true
        Me.cbTvDlFanartTvArt.Location = New System.Drawing.Point(10, 128)
        Me.cbTvDlFanartTvArt.Name = "cbTvDlFanartTvArt"
        Me.cbTvDlFanartTvArt.Size = New System.Drawing.Size(155, 19)
        Me.cbTvDlFanartTvArt.TabIndex = 72
        Me.cbTvDlFanartTvArt.Text = "Download Fanart.Tv Art."
        Me.cbTvDlFanartTvArt.UseVisualStyleBackColor = true
        '
        'cbSeasonFolderjpg
        '
        Me.cbSeasonFolderjpg.AutoSize = true
        Me.cbSeasonFolderjpg.Location = New System.Drawing.Point(10, 269)
        Me.cbSeasonFolderjpg.Name = "cbSeasonFolderjpg"
        Me.cbSeasonFolderjpg.Size = New System.Drawing.Size(237, 34)
        Me.cbSeasonFolderjpg.TabIndex = 71
        Me.cbSeasonFolderjpg.Text = "Save Season Poster as ""folder.jpg"" into"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Season Folder if present."
        Me.cbSeasonFolderjpg.UseVisualStyleBackColor = true
        '
        'cb_TvFolderJpg
        '
        Me.cb_TvFolderJpg.AutoSize = true
        Me.cb_TvFolderJpg.Location = New System.Drawing.Point(10, 250)
        Me.cb_TvFolderJpg.Name = "cb_TvFolderJpg"
        Me.cb_TvFolderJpg.Size = New System.Drawing.Size(221, 19)
        Me.cb_TvFolderJpg.TabIndex = 70
        Me.cb_TvFolderJpg.Text = "Save Show Poster also as folder.jpg"
        Me.cb_TvFolderJpg.UseVisualStyleBackColor = true
        '
        'cbTvDlXtraFanart
        '
        Me.cbTvDlXtraFanart.AutoSize = true
        Me.cbTvDlXtraFanart.Location = New System.Drawing.Point(10, 100)
        Me.cbTvDlXtraFanart.Name = "cbTvDlXtraFanart"
        Me.cbTvDlXtraFanart.Size = New System.Drawing.Size(148, 19)
        Me.cbTvDlXtraFanart.TabIndex = 69
        Me.cbTvDlXtraFanart.Text = "Download ExtraFanart"
        Me.cbTvDlXtraFanart.UseVisualStyleBackColor = true
        '
        'GroupBox18
        '
        Me.GroupBox18.Controls.Add(Me.posterbtn)
        Me.GroupBox18.Controls.Add(Me.bannerbtn)
        Me.GroupBox18.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.GroupBox18.Location = New System.Drawing.Point(10, 202)
        Me.GroupBox18.Name = "GroupBox18"
        Me.GroupBox18.Size = New System.Drawing.Size(290, 41)
        Me.GroupBox18.TabIndex = 68
        Me.GroupBox18.TabStop = false
        Me.GroupBox18.Text = "Default TV Show Thumbnails"
        '
        'posterbtn
        '
        Me.posterbtn.AutoSize = true
        Me.posterbtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.posterbtn.Location = New System.Drawing.Point(7, 16)
        Me.posterbtn.Margin = New System.Windows.Forms.Padding(4)
        Me.posterbtn.Name = "posterbtn"
        Me.posterbtn.Size = New System.Drawing.Size(60, 19)
        Me.posterbtn.TabIndex = 1
        Me.posterbtn.TabStop = true
        Me.posterbtn.Text = "Poster"
        Me.posterbtn.UseVisualStyleBackColor = true
        '
        'bannerbtn
        '
        Me.bannerbtn.AutoSize = true
        Me.bannerbtn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.bannerbtn.Location = New System.Drawing.Point(78, 16)
        Me.bannerbtn.Margin = New System.Windows.Forms.Padding(4)
        Me.bannerbtn.Name = "bannerbtn"
        Me.bannerbtn.Size = New System.Drawing.Size(65, 19)
        Me.bannerbtn.TabIndex = 2
        Me.bannerbtn.Text = "Banner"
        Me.bannerbtn.UseVisualStyleBackColor = true
        '
        'cbTvDlPosterArt
        '
        Me.cbTvDlPosterArt.AutoSize = true
        Me.cbTvDlPosterArt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvDlPosterArt.Location = New System.Drawing.Point(10, 20)
        Me.cbTvDlPosterArt.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvDlPosterArt.Name = "cbTvDlPosterArt"
        Me.cbTvDlPosterArt.Size = New System.Drawing.Size(212, 19)
        Me.cbTvDlPosterArt.TabIndex = 9
        Me.cbTvDlPosterArt.Text = "Download TV Show poster/banner"
        Me.cbTvDlPosterArt.UseVisualStyleBackColor = true
        '
        'cbTvDlSeasonArt
        '
        Me.cbTvDlSeasonArt.AutoSize = true
        Me.cbTvDlSeasonArt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvDlSeasonArt.Location = New System.Drawing.Point(10, 58)
        Me.cbTvDlSeasonArt.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvDlSeasonArt.Name = "cbTvDlSeasonArt"
        Me.cbTvDlSeasonArt.Size = New System.Drawing.Size(175, 19)
        Me.cbTvDlSeasonArt.TabIndex = 8
        Me.cbTvDlSeasonArt.Text = "Download Season Thumbs"
        Me.cbTvDlSeasonArt.UseVisualStyleBackColor = true
        '
        'cbTvDlFanart
        '
        Me.cbTvDlFanart.AutoSize = true
        Me.cbTvDlFanart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvDlFanart.Location = New System.Drawing.Point(10, 38)
        Me.cbTvDlFanart.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvDlFanart.Name = "cbTvDlFanart"
        Me.cbTvDlFanart.Size = New System.Drawing.Size(171, 19)
        Me.cbTvDlFanart.TabIndex = 10
        Me.cbTvDlFanart.Text = "Download TV Show Fanart"
        Me.cbTvDlFanart.UseVisualStyleBackColor = true
        '
        'GroupBox19
        '
        Me.GroupBox19.Controls.Add(Me.RadioButton39)
        Me.GroupBox19.Controls.Add(Me.RadioButton40)
        Me.GroupBox19.Controls.Add(Me.RadioButton41)
        Me.GroupBox19.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.GroupBox19.Location = New System.Drawing.Point(10, 154)
        Me.GroupBox19.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox19.Name = "GroupBox19"
        Me.GroupBox19.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox19.Size = New System.Drawing.Size(290, 41)
        Me.GroupBox19.TabIndex = 12
        Me.GroupBox19.TabStop = false
        Me.GroupBox19.Text = "Download season-all.tbn"
        '
        'RadioButton39
        '
        Me.RadioButton39.AutoSize = true
        Me.RadioButton39.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.RadioButton39.Location = New System.Drawing.Point(156, 15)
        Me.RadioButton39.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButton39.Name = "RadioButton39"
        Me.RadioButton39.Size = New System.Drawing.Size(65, 19)
        Me.RadioButton39.TabIndex = 2
        Me.RadioButton39.TabStop = true
        Me.RadioButton39.Text = "Banner"
        Me.RadioButton39.UseVisualStyleBackColor = true
        '
        'RadioButton40
        '
        Me.RadioButton40.AutoSize = true
        Me.RadioButton40.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.RadioButton40.Location = New System.Drawing.Point(79, 15)
        Me.RadioButton40.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButton40.Name = "RadioButton40"
        Me.RadioButton40.Size = New System.Drawing.Size(60, 19)
        Me.RadioButton40.TabIndex = 1
        Me.RadioButton40.TabStop = true
        Me.RadioButton40.Text = "Poster"
        Me.RadioButton40.UseVisualStyleBackColor = true
        '
        'RadioButton41
        '
        Me.RadioButton41.AutoSize = true
        Me.RadioButton41.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.RadioButton41.Location = New System.Drawing.Point(8, 15)
        Me.RadioButton41.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButton41.Name = "RadioButton41"
        Me.RadioButton41.Size = New System.Drawing.Size(55, 19)
        Me.RadioButton41.TabIndex = 0
        Me.RadioButton41.TabStop = true
        Me.RadioButton41.Text = "None"
        Me.RadioButton41.UseVisualStyleBackColor = true
        '
        'cbTvQuickAddShow
        '
        Me.cbTvQuickAddShow.AutoSize = true
        Me.cbTvQuickAddShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvQuickAddShow.Location = New System.Drawing.Point(412, 391)
        Me.cbTvQuickAddShow.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvQuickAddShow.Name = "cbTvQuickAddShow"
        Me.cbTvQuickAddShow.Size = New System.Drawing.Size(181, 19)
        Me.cbTvQuickAddShow.TabIndex = 34
        Me.cbTvQuickAddShow.Text = "Quick add for new TV Shows"
        Me.cbTvQuickAddShow.UseVisualStyleBackColor = true
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rb_TvRenameReplaceSpaceUnderScore)
        Me.GroupBox1.Controls.Add(Me.rb_TvRenameReplaceSpaceDot)
        Me.GroupBox1.Controls.Add(Me.cb_TvRenameReplaceSpace)
        Me.GroupBox1.Controls.Add(Me.CheckBox_tv_EpisodeRenameCase)
        Me.GroupBox1.Controls.Add(Me.CheckBox_tv_EpisodeRenameAuto)
        Me.GroupBox1.Controls.Add(Me.Label140)
        Me.GroupBox1.Controls.Add(Me.ComboBox_tv_EpisodeRename)
        Me.GroupBox1.Location = New System.Drawing.Point(712, 134)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(264, 250)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = false
        Me.GroupBox1.Text = "TV Episode Renaming Settings"
        '
        'rb_TvRenameReplaceSpaceUnderScore
        '
        Me.rb_TvRenameReplaceSpaceUnderScore.AutoSize = true
        Me.rb_TvRenameReplaceSpaceUnderScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rb_TvRenameReplaceSpaceUnderScore.Location = New System.Drawing.Point(154, 224)
        Me.rb_TvRenameReplaceSpaceUnderScore.Name = "rb_TvRenameReplaceSpaceUnderScore"
        Me.rb_TvRenameReplaceSpaceUnderScore.Size = New System.Drawing.Size(43, 19)
        Me.rb_TvRenameReplaceSpaceUnderScore.TabIndex = 6
        Me.rb_TvRenameReplaceSpaceUnderScore.TabStop = true
        Me.rb_TvRenameReplaceSpaceUnderScore.Text = """_"""
        Me.rb_TvRenameReplaceSpaceUnderScore.UseVisualStyleBackColor = true
        '
        'rb_TvRenameReplaceSpaceDot
        '
        Me.rb_TvRenameReplaceSpaceDot.AutoSize = true
        Me.rb_TvRenameReplaceSpaceDot.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rb_TvRenameReplaceSpaceDot.Location = New System.Drawing.Point(58, 224)
        Me.rb_TvRenameReplaceSpaceDot.Name = "rb_TvRenameReplaceSpaceDot"
        Me.rb_TvRenameReplaceSpaceDot.Size = New System.Drawing.Size(39, 19)
        Me.rb_TvRenameReplaceSpaceDot.TabIndex = 5
        Me.rb_TvRenameReplaceSpaceDot.TabStop = true
        Me.rb_TvRenameReplaceSpaceDot.Text = """."""
        Me.rb_TvRenameReplaceSpaceDot.UseVisualStyleBackColor = true
        '
        'cb_TvRenameReplaceSpace
        '
        Me.cb_TvRenameReplaceSpace.AutoSize = true
        Me.cb_TvRenameReplaceSpace.Location = New System.Drawing.Point(14, 203)
        Me.cb_TvRenameReplaceSpace.Name = "cb_TvRenameReplaceSpace"
        Me.cb_TvRenameReplaceSpace.Size = New System.Drawing.Size(144, 19)
        Me.cb_TvRenameReplaceSpace.TabIndex = 4
        Me.cb_TvRenameReplaceSpace.Text = "Replace Spaces with:"
        Me.cb_TvRenameReplaceSpace.UseVisualStyleBackColor = true
        '
        'CheckBox_tv_EpisodeRenameCase
        '
        Me.CheckBox_tv_EpisodeRenameCase.AutoSize = true
        Me.CheckBox_tv_EpisodeRenameCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.CheckBox_tv_EpisodeRenameCase.Location = New System.Drawing.Point(14, 147)
        Me.CheckBox_tv_EpisodeRenameCase.Margin = New System.Windows.Forms.Padding(4)
        Me.CheckBox_tv_EpisodeRenameCase.Name = "CheckBox_tv_EpisodeRenameCase"
        Me.CheckBox_tv_EpisodeRenameCase.Size = New System.Drawing.Size(170, 49)
        Me.CheckBox_tv_EpisodeRenameCase.TabIndex = 3
        Me.CheckBox_tv_EpisodeRenameCase.Text = "Use lowercase"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"s01e01 instead of S01E01"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"1x01 instead of 1X01"
        Me.CheckBox_tv_EpisodeRenameCase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.CheckBox_tv_EpisodeRenameCase.UseVisualStyleBackColor = true
        '
        'CheckBox_tv_EpisodeRenameAuto
        '
        Me.CheckBox_tv_EpisodeRenameAuto.AutoSize = true
        Me.CheckBox_tv_EpisodeRenameAuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.CheckBox_tv_EpisodeRenameAuto.Location = New System.Drawing.Point(14, 123)
        Me.CheckBox_tv_EpisodeRenameAuto.Margin = New System.Windows.Forms.Padding(4)
        Me.CheckBox_tv_EpisodeRenameAuto.Name = "CheckBox_tv_EpisodeRenameAuto"
        Me.CheckBox_tv_EpisodeRenameAuto.Size = New System.Drawing.Size(185, 19)
        Me.CheckBox_tv_EpisodeRenameAuto.TabIndex = 2
        Me.CheckBox_tv_EpisodeRenameAuto.Text = "Enable auto episode rename"
        Me.CheckBox_tv_EpisodeRenameAuto.UseVisualStyleBackColor = true
        '
        'Label140
        '
        Me.Label140.AutoSize = true
        Me.Label140.Location = New System.Drawing.Point(11, 59)
        Me.Label140.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label140.Name = "Label140"
        Me.Label140.Size = New System.Drawing.Size(235, 60)
        Me.Label140.TabIndex = 1
        Me.Label140.Text = "Rename episodes during scraping."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Only use this option if you are sure that the"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)& _ 
    "file is named according to supported"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"MC naming conventions."
        '
        'ComboBox_tv_EpisodeRename
        '
        Me.ComboBox_tv_EpisodeRename.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ComboBox_tv_EpisodeRename.FormattingEnabled = true
        Me.ComboBox_tv_EpisodeRename.Location = New System.Drawing.Point(14, 22)
        Me.ComboBox_tv_EpisodeRename.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBox_tv_EpisodeRename.Name = "ComboBox_tv_EpisodeRename"
        Me.ComboBox_tv_EpisodeRename.Size = New System.Drawing.Size(240, 23)
        Me.ComboBox_tv_EpisodeRename.TabIndex = 0
        '
        'cbTvEpEnableHDTags
        '
        Me.cbTvEpEnableHDTags.AutoSize = true
        Me.cbTvEpEnableHDTags.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvEpEnableHDTags.Location = New System.Drawing.Point(132, 327)
        Me.cbTvEpEnableHDTags.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvEpEnableHDTags.Name = "cbTvEpEnableHDTags"
        Me.cbTvEpEnableHDTags.Size = New System.Drawing.Size(229, 19)
        Me.cbTvEpEnableHDTags.TabIndex = 14
        Me.cbTvEpEnableHDTags.Text = "Save Media Tags to episode.nfo files."
        Me.cbTvEpEnableHDTags.UseVisualStyleBackColor = true
        '
        'cbTvDisableLogs
        '
        Me.cbTvDisableLogs.AutoSize = true
        Me.cbTvDisableLogs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbTvDisableLogs.Location = New System.Drawing.Point(132, 351)
        Me.cbTvDisableLogs.Margin = New System.Windows.Forms.Padding(4)
        Me.cbTvDisableLogs.Name = "cbTvDisableLogs"
        Me.cbTvDisableLogs.Size = New System.Drawing.Size(216, 19)
        Me.cbTvDisableLogs.TabIndex = 13
        Me.cbTvDisableLogs.Text = "Display log after scraping episodes"
        Me.cbTvDisableLogs.UseVisualStyleBackColor = true
        '
        'lbxTVDBLangs
        '
        Me.lbxTVDBLangs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbxTVDBLangs.FormattingEnabled = true
        Me.lbxTVDBLangs.ItemHeight = 15
        Me.lbxTVDBLangs.Location = New System.Drawing.Point(12, 26)
        Me.lbxTVDBLangs.Margin = New System.Windows.Forms.Padding(4)
        Me.lbxTVDBLangs.Name = "lbxTVDBLangs"
        Me.lbxTVDBLangs.Size = New System.Drawing.Size(103, 424)
        Me.lbxTVDBLangs.TabIndex = 11
        '
        'Label122
        '
        Me.Label122.AutoSize = true
        Me.Label122.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label122.Location = New System.Drawing.Point(709, 26)
        Me.Label122.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label122.Name = "Label122"
        Me.Label122.Size = New System.Drawing.Size(262, 60)
        Me.Label122.TabIndex = 6
        Me.Label122.Text = "Use the Selection Box Below to choose where"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"to get tv actor nfo from."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Only use "& _ 
    "IMDB if the Series order is identical"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"to thetvdb.com or wrong actors will be ob"& _ 
    "tained"
        '
        'ComboBox8
        '
        Me.ComboBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ComboBox8.FormattingEnabled = true
        Me.ComboBox8.Items.AddRange(New Object() {"All Actors from TVDB", "All Actors from IMDB", "TVshow Actor from IMDB, Episode from TVDB", "TVshow Actor from TVDB, Episode from IMDB"})
        Me.ComboBox8.Location = New System.Drawing.Point(712, 97)
        Me.ComboBox8.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBox8.Name = "ComboBox8"
        Me.ComboBox8.Size = New System.Drawing.Size(264, 23)
        Me.ComboBox8.TabIndex = 0
        '
        'GroupBox_TVDB_Scraper_Preferences
        '
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.cbXBMCTvdbRatingFallback)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.cbXBMCTvdbRatingImdb)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.ComboBox_TVDB_Language)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.Label154)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.cbXBMCTvdbPosters)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.cbXBMCTvdbFanart)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.rbXBMCTvdbAbsoluteNumber)
        Me.GroupBox_TVDB_Scraper_Preferences.Controls.Add(Me.rbXBMCTvdbDVDOrder)
        Me.GroupBox_TVDB_Scraper_Preferences.Location = New System.Drawing.Point(124, 20)
        Me.GroupBox_TVDB_Scraper_Preferences.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox_TVDB_Scraper_Preferences.Name = "GroupBox_TVDB_Scraper_Preferences"
        Me.GroupBox_TVDB_Scraper_Preferences.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox_TVDB_Scraper_Preferences.Size = New System.Drawing.Size(270, 275)
        Me.GroupBox_TVDB_Scraper_Preferences.TabIndex = 67
        Me.GroupBox_TVDB_Scraper_Preferences.TabStop = false
        Me.GroupBox_TVDB_Scraper_Preferences.Text = "XBMC Scraper TheTVDB Preferences"
        Me.GroupBox_TVDB_Scraper_Preferences.Visible = false
        '
        'cbXBMCTvdbRatingFallback
        '
        Me.cbXBMCTvdbRatingFallback.AutoSize = true
        Me.cbXBMCTvdbRatingFallback.Location = New System.Drawing.Point(12, 209)
        Me.cbXBMCTvdbRatingFallback.Name = "cbXBMCTvdbRatingFallback"
        Me.cbXBMCTvdbRatingFallback.Size = New System.Drawing.Size(243, 19)
        Me.cbXBMCTvdbRatingFallback.TabIndex = 7
        Me.cbXBMCTvdbRatingFallback.Text = "Ratings fall back to TVDB if not on IMDB"
        Me.cbXBMCTvdbRatingFallback.UseVisualStyleBackColor = true
        '
        'cbXBMCTvdbRatingImdb
        '
        Me.cbXBMCTvdbRatingImdb.AutoSize = true
        Me.cbXBMCTvdbRatingImdb.Location = New System.Drawing.Point(12, 184)
        Me.cbXBMCTvdbRatingImdb.Name = "cbXBMCTvdbRatingImdb"
        Me.cbXBMCTvdbRatingImdb.Size = New System.Drawing.Size(200, 19)
        Me.cbXBMCTvdbRatingImdb.TabIndex = 6
        Me.cbXBMCTvdbRatingImdb.Text = "Get Episode Ratings from IMDB"
        Me.cbXBMCTvdbRatingImdb.UseVisualStyleBackColor = true
        '
        'ComboBox_TVDB_Language
        '
        Me.ComboBox_TVDB_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_TVDB_Language.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.ComboBox_TVDB_Language.FormattingEnabled = true
        Me.ComboBox_TVDB_Language.Location = New System.Drawing.Point(85, 144)
        Me.ComboBox_TVDB_Language.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBox_TVDB_Language.Name = "ComboBox_TVDB_Language"
        Me.ComboBox_TVDB_Language.Size = New System.Drawing.Size(150, 23)
        Me.ComboBox_TVDB_Language.Sorted = true
        Me.ComboBox_TVDB_Language.TabIndex = 5
        '
        'Label154
        '
        Me.Label154.AutoSize = true
        Me.Label154.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label154.Location = New System.Drawing.Point(9, 148)
        Me.Label154.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label154.Name = "Label154"
        Me.Label154.Size = New System.Drawing.Size(63, 15)
        Me.Label154.TabIndex = 4
        Me.Label154.Text = "Language"
        '
        'cbXBMCTvdbPosters
        '
        Me.cbXBMCTvdbPosters.AutoSize = true
        Me.cbXBMCTvdbPosters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXBMCTvdbPosters.Location = New System.Drawing.Point(12, 114)
        Me.cbXBMCTvdbPosters.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXBMCTvdbPosters.Name = "cbXBMCTvdbPosters"
        Me.cbXBMCTvdbPosters.Size = New System.Drawing.Size(103, 19)
        Me.cbXBMCTvdbPosters.TabIndex = 3
        Me.cbXBMCTvdbPosters.Text = "Prefer Posters"
        Me.cbXBMCTvdbPosters.UseVisualStyleBackColor = true
        '
        'cbXBMCTvdbFanart
        '
        Me.cbXBMCTvdbFanart.AutoSize = true
        Me.cbXBMCTvdbFanart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.cbXBMCTvdbFanart.Location = New System.Drawing.Point(12, 84)
        Me.cbXBMCTvdbFanart.Margin = New System.Windows.Forms.Padding(4)
        Me.cbXBMCTvdbFanart.Name = "cbXBMCTvdbFanart"
        Me.cbXBMCTvdbFanart.Size = New System.Drawing.Size(103, 19)
        Me.cbXBMCTvdbFanart.TabIndex = 2
        Me.cbXBMCTvdbFanart.Text = "Enable Fanart"
        Me.cbXBMCTvdbFanart.UseVisualStyleBackColor = true
        '
        'rbXBMCTvdbAbsoluteNumber
        '
        Me.rbXBMCTvdbAbsoluteNumber.AutoSize = true
        Me.rbXBMCTvdbAbsoluteNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbXBMCTvdbAbsoluteNumber.Location = New System.Drawing.Point(12, 58)
        Me.rbXBMCTvdbAbsoluteNumber.Margin = New System.Windows.Forms.Padding(4)
        Me.rbXBMCTvdbAbsoluteNumber.Name = "rbXBMCTvdbAbsoluteNumber"
        Me.rbXBMCTvdbAbsoluteNumber.Size = New System.Drawing.Size(148, 19)
        Me.rbXBMCTvdbAbsoluteNumber.TabIndex = 1
        Me.rbXBMCTvdbAbsoluteNumber.TabStop = true
        Me.rbXBMCTvdbAbsoluteNumber.Text = "Use Absolute Ordering"
        Me.rbXBMCTvdbAbsoluteNumber.UseVisualStyleBackColor = true
        '
        'rbXBMCTvdbDVDOrder
        '
        Me.rbXBMCTvdbDVDOrder.AutoSize = true
        Me.rbXBMCTvdbDVDOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.rbXBMCTvdbDVDOrder.Location = New System.Drawing.Point(12, 29)
        Me.rbXBMCTvdbDVDOrder.Margin = New System.Windows.Forms.Padding(4)
        Me.rbXBMCTvdbDVDOrder.Name = "rbXBMCTvdbDVDOrder"
        Me.rbXBMCTvdbDVDOrder.Size = New System.Drawing.Size(157, 19)
        Me.rbXBMCTvdbDVDOrder.TabIndex = 0
        Me.rbXBMCTvdbDVDOrder.TabStop = true
        Me.rbXBMCTvdbDVDOrder.Text = "Use DVD Order (default)"
        Me.rbXBMCTvdbDVDOrder.UseVisualStyleBackColor = true
        '
        'TabPage31
        '
        Me.TabPage31.Controls.Add(Me.GroupBox_tv_RegexRename)
        Me.TabPage31.Controls.Add(Me.GroupBox_tv_RegexScrape)
        Me.TabPage31.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TabPage31.Location = New System.Drawing.Point(4, 24)
        Me.TabPage31.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage31.Name = "TabPage31"
        Me.TabPage31.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage31.Size = New System.Drawing.Size(184, 46)
        Me.TabPage31.TabIndex = 1
        Me.TabPage31.Text = "Regex"
        Me.TabPage31.UseVisualStyleBackColor = true
        '
        'GroupBox_tv_RegexRename
        '
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.btn_tv_RegexRename_Restore)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.lb_tv_RegexRename)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.btn_tv_RegexRename_Remove)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.Label158)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.btn_tv_RegexRename_MoveDown)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.Label159)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.btn_tv_RegexRename_MoveUp)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.tb_tv_RegexRename_New)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.tb_tv_RegexRename_Edit)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.btn_tv_RegexRename_Add)
        Me.GroupBox_tv_RegexRename.Controls.Add(Me.btn_tv_RegexRename_Edit)
        Me.GroupBox_tv_RegexRename.Location = New System.Drawing.Point(514, 7)
        Me.GroupBox_tv_RegexRename.Name = "GroupBox_tv_RegexRename"
        Me.GroupBox_tv_RegexRename.Size = New System.Drawing.Size(450, 506)
        Me.GroupBox_tv_RegexRename.TabIndex = 36
        Me.GroupBox_tv_RegexRename.TabStop = false
        Me.GroupBox_tv_RegexRename.Text = "Rename"
        '
        'btn_tv_RegexRename_Restore
        '
        Me.btn_tv_RegexRename_Restore.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexRename_Restore.Location = New System.Drawing.Point(266, 452)
        Me.btn_tv_RegexRename_Restore.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexRename_Restore.Name = "btn_tv_RegexRename_Restore"
        Me.btn_tv_RegexRename_Restore.Size = New System.Drawing.Size(168, 31)
        Me.btn_tv_RegexRename_Restore.TabIndex = 44
        Me.btn_tv_RegexRename_Restore.Text = "Restore Defaults"
        Me.btn_tv_RegexRename_Restore.UseVisualStyleBackColor = true
        '
        'lb_tv_RegexRename
        '
        Me.lb_tv_RegexRename.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lb_tv_RegexRename.FormattingEnabled = true
        Me.lb_tv_RegexRename.ItemHeight = 15
        Me.lb_tv_RegexRename.Location = New System.Drawing.Point(19, 24)
        Me.lb_tv_RegexRename.Margin = New System.Windows.Forms.Padding(4)
        Me.lb_tv_RegexRename.MaximumSize = New System.Drawing.Size(2000, 275)
        Me.lb_tv_RegexRename.MinimumSize = New System.Drawing.Size(4, 79)
        Me.lb_tv_RegexRename.Name = "lb_tv_RegexRename"
        Me.lb_tv_RegexRename.Size = New System.Drawing.Size(374, 259)
        Me.lb_tv_RegexRename.TabIndex = 35
        '
        'btn_tv_RegexRename_Remove
        '
        Me.btn_tv_RegexRename_Remove.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexRename_Remove.Location = New System.Drawing.Point(269, 316)
        Me.btn_tv_RegexRename_Remove.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexRename_Remove.Name = "btn_tv_RegexRename_Remove"
        Me.btn_tv_RegexRename_Remove.Size = New System.Drawing.Size(168, 31)
        Me.btn_tv_RegexRename_Remove.TabIndex = 34
        Me.btn_tv_RegexRename_Remove.Text = "Remove Selected"
        Me.btn_tv_RegexRename_Remove.UseVisualStyleBackColor = true
        '
        'Label158
        '
        Me.Label158.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label158.AutoSize = true
        Me.Label158.Location = New System.Drawing.Point(16, 379)
        Me.Label158.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label158.Name = "Label158"
        Me.Label158.Size = New System.Drawing.Size(71, 15)
        Me.Label158.TabIndex = 37
        Me.Label158.Text = "New Regex"
        '
        'Label159
        '
        Me.Label159.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label159.AutoSize = true
        Me.Label159.Location = New System.Drawing.Point(16, 422)
        Me.Label159.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label159.Name = "Label159"
        Me.Label159.Size = New System.Drawing.Size(67, 15)
        Me.Label159.TabIndex = 39
        Me.Label159.Text = "Edit Regex"
        '
        'tb_tv_RegexRename_New
        '
        Me.tb_tv_RegexRename_New.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tb_tv_RegexRename_New.Location = New System.Drawing.Point(98, 375)
        Me.tb_tv_RegexRename_New.Margin = New System.Windows.Forms.Padding(4)
        Me.tb_tv_RegexRename_New.Name = "tb_tv_RegexRename_New"
        Me.tb_tv_RegexRename_New.Size = New System.Drawing.Size(263, 21)
        Me.tb_tv_RegexRename_New.TabIndex = 36
        '
        'tb_tv_RegexRename_Edit
        '
        Me.tb_tv_RegexRename_Edit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tb_tv_RegexRename_Edit.Location = New System.Drawing.Point(98, 415)
        Me.tb_tv_RegexRename_Edit.Margin = New System.Windows.Forms.Padding(4)
        Me.tb_tv_RegexRename_Edit.Name = "tb_tv_RegexRename_Edit"
        Me.tb_tv_RegexRename_Edit.Size = New System.Drawing.Size(263, 21)
        Me.tb_tv_RegexRename_Edit.TabIndex = 40
        '
        'btn_tv_RegexRename_Add
        '
        Me.btn_tv_RegexRename_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexRename_Add.Location = New System.Drawing.Point(369, 373)
        Me.btn_tv_RegexRename_Add.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexRename_Add.Name = "btn_tv_RegexRename_Add"
        Me.btn_tv_RegexRename_Add.Size = New System.Drawing.Size(66, 31)
        Me.btn_tv_RegexRename_Add.TabIndex = 38
        Me.btn_tv_RegexRename_Add.Text = "Add"
        Me.btn_tv_RegexRename_Add.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexRename_Edit
        '
        Me.btn_tv_RegexRename_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexRename_Edit.Location = New System.Drawing.Point(369, 415)
        Me.btn_tv_RegexRename_Edit.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexRename_Edit.Name = "btn_tv_RegexRename_Edit"
        Me.btn_tv_RegexRename_Edit.Size = New System.Drawing.Size(66, 31)
        Me.btn_tv_RegexRename_Edit.TabIndex = 41
        Me.btn_tv_RegexRename_Edit.Text = "Edit"
        Me.btn_tv_RegexRename_Edit.UseVisualStyleBackColor = true
        '
        'GroupBox_tv_RegexScrape
        '
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.lb_tv_RegexScrape)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.btn_tv_RegexScrape_Remove)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.Label119)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.btn_tv_RegexScrape_MoveDown)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.Label117)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.btn_tv_RegexScrape_MoveUp)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.tb_tv_RegexScrape_New)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.tb_tv_RegexScrape_Edit)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.btn_tv_RegexScrape_Add)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.btn_tv_RegexScrape_Edit)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.btn_tv_RegexScrape_Restore)
        Me.GroupBox_tv_RegexScrape.Controls.Add(Me.GroupBox_tv_RegexScrape_Test)
        Me.GroupBox_tv_RegexScrape.Location = New System.Drawing.Point(7, 7)
        Me.GroupBox_tv_RegexScrape.Name = "GroupBox_tv_RegexScrape"
        Me.GroupBox_tv_RegexScrape.Size = New System.Drawing.Size(450, 506)
        Me.GroupBox_tv_RegexScrape.TabIndex = 34
        Me.GroupBox_tv_RegexScrape.TabStop = false
        Me.GroupBox_tv_RegexScrape.Text = "Scrape"
        '
        'lb_tv_RegexScrape
        '
        Me.lb_tv_RegexScrape.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lb_tv_RegexScrape.FormattingEnabled = true
        Me.lb_tv_RegexScrape.ItemHeight = 15
        Me.lb_tv_RegexScrape.Location = New System.Drawing.Point(16, 21)
        Me.lb_tv_RegexScrape.Margin = New System.Windows.Forms.Padding(4)
        Me.lb_tv_RegexScrape.Name = "lb_tv_RegexScrape"
        Me.lb_tv_RegexScrape.Size = New System.Drawing.Size(374, 139)
        Me.lb_tv_RegexScrape.TabIndex = 23
        '
        'btn_tv_RegexScrape_Remove
        '
        Me.btn_tv_RegexScrape_Remove.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_Remove.Location = New System.Drawing.Point(265, 171)
        Me.btn_tv_RegexScrape_Remove.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_Remove.Name = "btn_tv_RegexScrape_Remove"
        Me.btn_tv_RegexScrape_Remove.Size = New System.Drawing.Size(168, 29)
        Me.btn_tv_RegexScrape_Remove.TabIndex = 15
        Me.btn_tv_RegexScrape_Remove.Text = "Remove Selected"
        Me.btn_tv_RegexScrape_Remove.UseVisualStyleBackColor = true
        '
        'Label119
        '
        Me.Label119.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label119.AutoSize = true
        Me.Label119.Location = New System.Drawing.Point(15, 215)
        Me.Label119.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label119.Name = "Label119"
        Me.Label119.Size = New System.Drawing.Size(71, 15)
        Me.Label119.TabIndex = 25
        Me.Label119.Text = "New Regex"
        '
        'Label117
        '
        Me.Label117.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label117.AutoSize = true
        Me.Label117.Location = New System.Drawing.Point(15, 251)
        Me.Label117.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(67, 15)
        Me.Label117.TabIndex = 28
        Me.Label117.Text = "Edit Regex"
        '
        'tb_tv_RegexScrape_New
        '
        Me.tb_tv_RegexScrape_New.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tb_tv_RegexScrape_New.Location = New System.Drawing.Point(97, 212)
        Me.tb_tv_RegexScrape_New.Margin = New System.Windows.Forms.Padding(4)
        Me.tb_tv_RegexScrape_New.Name = "tb_tv_RegexScrape_New"
        Me.tb_tv_RegexScrape_New.Size = New System.Drawing.Size(262, 21)
        Me.tb_tv_RegexScrape_New.TabIndex = 24
        '
        'tb_tv_RegexScrape_Edit
        '
        Me.tb_tv_RegexScrape_Edit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tb_tv_RegexScrape_Edit.Location = New System.Drawing.Point(97, 248)
        Me.tb_tv_RegexScrape_Edit.Margin = New System.Windows.Forms.Padding(4)
        Me.tb_tv_RegexScrape_Edit.Name = "tb_tv_RegexScrape_Edit"
        Me.tb_tv_RegexScrape_Edit.Size = New System.Drawing.Size(262, 21)
        Me.tb_tv_RegexScrape_Edit.TabIndex = 29
        '
        'btn_tv_RegexScrape_Add
        '
        Me.btn_tv_RegexScrape_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_Add.Location = New System.Drawing.Point(367, 208)
        Me.btn_tv_RegexScrape_Add.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_Add.Name = "btn_tv_RegexScrape_Add"
        Me.btn_tv_RegexScrape_Add.Size = New System.Drawing.Size(66, 29)
        Me.btn_tv_RegexScrape_Add.TabIndex = 26
        Me.btn_tv_RegexScrape_Add.Text = "Add"
        Me.btn_tv_RegexScrape_Add.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexScrape_Edit
        '
        Me.btn_tv_RegexScrape_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_Edit.Location = New System.Drawing.Point(367, 244)
        Me.btn_tv_RegexScrape_Edit.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_Edit.Name = "btn_tv_RegexScrape_Edit"
        Me.btn_tv_RegexScrape_Edit.Size = New System.Drawing.Size(66, 29)
        Me.btn_tv_RegexScrape_Edit.TabIndex = 30
        Me.btn_tv_RegexScrape_Edit.Text = "Edit"
        Me.btn_tv_RegexScrape_Edit.UseVisualStyleBackColor = true
        '
        'btn_tv_RegexScrape_Restore
        '
        Me.btn_tv_RegexScrape_Restore.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_Restore.Location = New System.Drawing.Point(266, 467)
        Me.btn_tv_RegexScrape_Restore.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_Restore.Name = "btn_tv_RegexScrape_Restore"
        Me.btn_tv_RegexScrape_Restore.Size = New System.Drawing.Size(168, 29)
        Me.btn_tv_RegexScrape_Restore.TabIndex = 31
        Me.btn_tv_RegexScrape_Restore.Text = "Restore Defaults"
        Me.btn_tv_RegexScrape_Restore.UseVisualStyleBackColor = true
        '
        'GroupBox_tv_RegexScrape_Test
        '
        Me.GroupBox_tv_RegexScrape_Test.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.GroupBox_tv_RegexScrape_Test.Controls.Add(Me.tb_tv_RegexScrape_TestResult)
        Me.GroupBox_tv_RegexScrape_Test.Controls.Add(Me.btn_tv_RegexScrape_Test)
        Me.GroupBox_tv_RegexScrape_Test.Controls.Add(Me.tb_tv_RegexScrape_TestString)
        Me.GroupBox_tv_RegexScrape_Test.Controls.Add(Me.Label118)
        Me.GroupBox_tv_RegexScrape_Test.Location = New System.Drawing.Point(16, 283)
        Me.GroupBox_tv_RegexScrape_Test.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox_tv_RegexScrape_Test.Name = "GroupBox_tv_RegexScrape_Test"
        Me.GroupBox_tv_RegexScrape_Test.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox_tv_RegexScrape_Test.Size = New System.Drawing.Size(418, 176)
        Me.GroupBox_tv_RegexScrape_Test.TabIndex = 27
        Me.GroupBox_tv_RegexScrape_Test.TabStop = false
        Me.GroupBox_tv_RegexScrape_Test.Text = "Test Selected Regex"
        '
        'tb_tv_RegexScrape_TestResult
        '
        Me.tb_tv_RegexScrape_TestResult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tb_tv_RegexScrape_TestResult.Location = New System.Drawing.Point(8, 59)
        Me.tb_tv_RegexScrape_TestResult.Margin = New System.Windows.Forms.Padding(4)
        Me.tb_tv_RegexScrape_TestResult.Multiline = true
        Me.tb_tv_RegexScrape_TestResult.Name = "tb_tv_RegexScrape_TestResult"
        Me.tb_tv_RegexScrape_TestResult.Size = New System.Drawing.Size(402, 104)
        Me.tb_tv_RegexScrape_TestResult.TabIndex = 3
        '
        'btn_tv_RegexScrape_Test
        '
        Me.btn_tv_RegexScrape_Test.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btn_tv_RegexScrape_Test.Location = New System.Drawing.Point(334, 17)
        Me.btn_tv_RegexScrape_Test.Margin = New System.Windows.Forms.Padding(4)
        Me.btn_tv_RegexScrape_Test.Name = "btn_tv_RegexScrape_Test"
        Me.btn_tv_RegexScrape_Test.Size = New System.Drawing.Size(66, 29)
        Me.btn_tv_RegexScrape_Test.TabIndex = 2
        Me.btn_tv_RegexScrape_Test.Text = "Test"
        Me.btn_tv_RegexScrape_Test.UseVisualStyleBackColor = true
        '
        'tb_tv_RegexScrape_TestString
        '
        Me.tb_tv_RegexScrape_TestString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tb_tv_RegexScrape_TestString.Location = New System.Drawing.Point(81, 21)
        Me.tb_tv_RegexScrape_TestString.Margin = New System.Windows.Forms.Padding(4)
        Me.tb_tv_RegexScrape_TestString.Name = "tb_tv_RegexScrape_TestString"
        Me.tb_tv_RegexScrape_TestString.Size = New System.Drawing.Size(243, 21)
        Me.tb_tv_RegexScrape_TestString.TabIndex = 1
        '
        'Label118
        '
        Me.Label118.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.Label118.AutoSize = true
        Me.Label118.Location = New System.Drawing.Point(5, 25)
        Me.Label118.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label118.Name = "Label118"
        Me.Label118.Size = New System.Drawing.Size(65, 15)
        Me.Label118.TabIndex = 0
        Me.Label118.Text = "Test String"
        '
        'TPHmPref
        '
        Me.TPHmPref.Controls.Add(Me.GroupBox5)
        Me.TPHmPref.Controls.Add(Me.lbl_HmHeader)
        Me.TPHmPref.Location = New System.Drawing.Point(4, 24)
        Me.TPHmPref.Name = "TPHmPref"
        Me.TPHmPref.Size = New System.Drawing.Size(1000, 595)
        Me.TPHmPref.TabIndex = 11
        Me.TPHmPref.Text = "HomeMovie Pref's"
        Me.TPHmPref.UseVisualStyleBackColor = true
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.cb_HmFanartScrnShot)
        Me.GroupBox5.Controls.Add(Me.tb_HmFanartTime)
        Me.GroupBox5.Controls.Add(Me.tb_HmPosterTime)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Location = New System.Drawing.Point(32, 85)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(358, 123)
        Me.GroupBox5.TabIndex = 5
        Me.GroupBox5.TabStop = false
        Me.GroupBox5.Text = "ScreenShot Options."
        '
        'cb_HmFanartScrnShot
        '
        Me.cb_HmFanartScrnShot.AutoSize = true
        Me.cb_HmFanartScrnShot.Location = New System.Drawing.Point(15, 20)
        Me.cb_HmFanartScrnShot.Name = "cb_HmFanartScrnShot"
        Me.cb_HmFanartScrnShot.Size = New System.Drawing.Size(324, 19)
        Me.cb_HmFanartScrnShot.TabIndex = 4
        Me.cb_HmFanartScrnShot.Text = "Enable AutoScreenShot of Fanart (During Scrape Only)"
        Me.cb_HmFanartScrnShot.UseVisualStyleBackColor = true
        '
        'tb_HmFanartTime
        '
        Me.tb_HmFanartTime.Location = New System.Drawing.Point(15, 49)
        Me.tb_HmFanartTime.Name = "tb_HmFanartTime"
        Me.tb_HmFanartTime.Size = New System.Drawing.Size(100, 21)
        Me.tb_HmFanartTime.TabIndex = 0
        '
        'tb_HmPosterTime
        '
        Me.tb_HmPosterTime.Location = New System.Drawing.Point(15, 84)
        Me.tb_HmPosterTime.Name = "tb_HmPosterTime"
        Me.tb_HmPosterTime.Size = New System.Drawing.Size(100, 21)
        Me.tb_HmPosterTime.TabIndex = 1
        '
        'Label15
        '
        Me.Label15.AutoSize = true
        Me.Label15.Location = New System.Drawing.Point(121, 87)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(196, 15)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "ScreenShot delay for Poster Image"
        '
        'Label14
        '
        Me.Label14.AutoSize = true
        Me.Label14.Location = New System.Drawing.Point(121, 52)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(196, 15)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "ScreenShot delay for Fanart Image"
        '
        'lbl_HmHeader
        '
        Me.lbl_HmHeader.AutoSize = true
        Me.lbl_HmHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbl_HmHeader.Location = New System.Drawing.Point(28, 21)
        Me.lbl_HmHeader.Name = "lbl_HmHeader"
        Me.lbl_HmHeader.Size = New System.Drawing.Size(368, 24)
        Me.lbl_HmHeader.TabIndex = 4
        Me.lbl_HmHeader.Text = "Only a few preferences here currently."
        '
        'TPProxy
        '
        Me.TPProxy.Controls.Add(Me.UcGenPref_Proxy1)
        Me.TPProxy.Location = New System.Drawing.Point(4, 24)
        Me.TPProxy.Name = "TPProxy"
        Me.TPProxy.Size = New System.Drawing.Size(1000, 595)
        Me.TPProxy.TabIndex = 10
        Me.TPProxy.Text = "Proxy"
        Me.TPProxy.UseVisualStyleBackColor = true
        '
        'UcGenPref_Proxy1
        '
        Me.UcGenPref_Proxy1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcGenPref_Proxy1.Location = New System.Drawing.Point(0, 0)
        Me.UcGenPref_Proxy1.Name = "UcGenPref_Proxy1"
        Me.UcGenPref_Proxy1.Size = New System.Drawing.Size(1000, 595)
        Me.UcGenPref_Proxy1.TabIndex = 0
        '
        'TPXBMCLink
        '
        Me.TPXBMCLink.Controls.Add(Me.UcGenPref_XbmcLink1)
        Me.TPXBMCLink.Location = New System.Drawing.Point(4, 24)
        Me.TPXBMCLink.Name = "TPXBMCLink"
        Me.TPXBMCLink.Size = New System.Drawing.Size(1000, 595)
        Me.TPXBMCLink.TabIndex = 9
        Me.TPXBMCLink.Text = "XBMC Link"
        Me.TPXBMCLink.UseVisualStyleBackColor = true
        '
        'UcGenPref_XbmcLink1
        '
        Me.UcGenPref_XbmcLink1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcGenPref_XbmcLink1.Location = New System.Drawing.Point(0, 0)
        Me.UcGenPref_XbmcLink1.Name = "UcGenPref_XbmcLink1"
        Me.UcGenPref_XbmcLink1.Size = New System.Drawing.Size(1000, 595)
        Me.UcGenPref_XbmcLink1.TabIndex = 0
        '
        'TPPRofCmd
        '
        Me.TPPRofCmd.Controls.Add(Me.TableLayoutPanel1)
        Me.TPPRofCmd.Location = New System.Drawing.Point(4, 24)
        Me.TPPRofCmd.Name = "TPPRofCmd"
        Me.TPPRofCmd.Size = New System.Drawing.Size(1000, 595)
        Me.TPPRofCmd.TabIndex = 6
        Me.TPPRofCmd.Text = "Profiles && Commands"
        Me.TPPRofCmd.UseVisualStyleBackColor = true
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 8
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 328!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 275!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22!))
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox42, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox15, 6, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lbl_CommandTitle, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lcl_CommandCommand, 3, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.tb_CommandTitle, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.tb_CommandCommand, 3, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.btn_CommandRemove, 5, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.lb_CommandTitle, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.lb_CommandCommand, 3, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.btn_CommandAdd, 5, 5)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 11
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 176!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1000, 595)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'GroupBox42
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.GroupBox42, 3)
        Me.GroupBox42.Controls.Add(Me.Label141)
        Me.GroupBox42.Location = New System.Drawing.Point(11, 11)
        Me.GroupBox42.Name = "GroupBox42"
        Me.GroupBox42.Size = New System.Drawing.Size(445, 113)
        Me.GroupBox42.TabIndex = 13
        Me.GroupBox42.TabStop = false
        '
        'Label141
        '
        Me.Label141.AutoSize = true
        Me.Label141.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label141.Location = New System.Drawing.Point(7, 17)
        Me.Label141.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label141.Name = "Label141"
        Me.Label141.Size = New System.Drawing.Size(417, 90)
        Me.Label141.TabIndex = 0
        Me.Label141.Text = resources.GetString("Label141.Text")
        '
        'GroupBox15
        '
        Me.GroupBox15.BackColor = System.Drawing.Color.FromArgb(CType(CType(224,Byte),Integer), CType(CType(224,Byte),Integer), CType(CType(224,Byte),Integer))
        Me.GroupBox15.Controls.Add(Me.btn_ProfileSetStartup)
        Me.GroupBox15.Controls.Add(Me.Label3)
        Me.GroupBox15.Controls.Add(Me.lb_ProfileList)
        Me.GroupBox15.Controls.Add(Me.Label18)
        Me.GroupBox15.Controls.Add(Me.btn_ProfileSetDefault)
        Me.GroupBox15.Controls.Add(Me.tb_ProfileNew)
        Me.GroupBox15.Controls.Add(Me.Label17)
        Me.GroupBox15.Controls.Add(Me.btn_ProfileAdd)
        Me.GroupBox15.Controls.Add(Me.Label16)
        Me.GroupBox15.Controls.Add(Me.btn_ProfileRemove)
        Me.GroupBox15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.GroupBox15.Location = New System.Drawing.Point(706, 11)
        Me.GroupBox15.Name = "GroupBox15"
        Me.TableLayoutPanel1.SetRowSpan(Me.GroupBox15, 10)
        Me.GroupBox15.Size = New System.Drawing.Size(268, 491)
        Me.GroupBox15.TabIndex = 12
        Me.GroupBox15.TabStop = false
        Me.GroupBox15.Text = "Profile Manager"
        '
        'btn_ProfileSetStartup
        '
        Me.btn_ProfileSetStartup.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btn_ProfileSetStartup.Location = New System.Drawing.Point(9, 459)
        Me.btn_ProfileSetStartup.Name = "btn_ProfileSetStartup"
        Me.btn_ProfileSetStartup.Size = New System.Drawing.Size(247, 23)
        Me.btn_ProfileSetStartup.TabIndex = 11
        Me.btn_ProfileSetStartup.Text = "Set Startup"
        Me.btn_ProfileSetStartup.UseVisualStyleBackColor = true
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 441)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(191, 15)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Current Startup Profile Is :- Default"
        '
        'lb_ProfileList
        '
        Me.lb_ProfileList.FormattingEnabled = true
        Me.lb_ProfileList.Location = New System.Drawing.Point(9, 121)
        Me.lb_ProfileList.Name = "lb_ProfileList"
        Me.lb_ProfileList.Size = New System.Drawing.Size(247, 225)
        Me.lb_ProfileList.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.AutoSize = true
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label18.Location = New System.Drawing.Point(6, 387)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(191, 15)
        Me.Label18.TabIndex = 9
        Me.Label18.Text = "Current Default Profile Is :- Default"
        '
        'btn_ProfileSetDefault
        '
        Me.btn_ProfileSetDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btn_ProfileSetDefault.Location = New System.Drawing.Point(9, 405)
        Me.btn_ProfileSetDefault.Name = "btn_ProfileSetDefault"
        Me.btn_ProfileSetDefault.Size = New System.Drawing.Size(247, 23)
        Me.btn_ProfileSetDefault.TabIndex = 8
        Me.btn_ProfileSetDefault.Text = "Set Default"
        Me.btn_ProfileSetDefault.UseVisualStyleBackColor = true
        '
        'tb_ProfileNew
        '
        Me.tb_ProfileNew.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.tb_ProfileNew.Location = New System.Drawing.Point(9, 67)
        Me.tb_ProfileNew.Name = "tb_ProfileNew"
        Me.tb_ProfileNew.Size = New System.Drawing.Size(247, 22)
        Me.tb_ProfileNew.TabIndex = 3
        '
        'Label17
        '
        Me.Label17.AutoSize = true
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label17.Location = New System.Drawing.Point(6, 16)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(250, 26)
        Me.Label17.TabIndex = 7
        Me.Label17.Text = "The Default Profile can't be deleted, it is used as a"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"template. You can set any "& _ 
    "created profile to default."
        '
        'btn_ProfileAdd
        '
        Me.btn_ProfileAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btn_ProfileAdd.Location = New System.Drawing.Point(9, 95)
        Me.btn_ProfileAdd.Name = "btn_ProfileAdd"
        Me.btn_ProfileAdd.Size = New System.Drawing.Size(247, 23)
        Me.btn_ProfileAdd.TabIndex = 4
        Me.btn_ProfileAdd.Text = "Add Profile"
        Me.btn_ProfileAdd.UseVisualStyleBackColor = true
        '
        'Label16
        '
        Me.Label16.AutoSize = true
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label16.Location = New System.Drawing.Point(6, 48)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(150, 16)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Enter New Profile Name"
        '
        'btn_ProfileRemove
        '
        Me.btn_ProfileRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btn_ProfileRemove.Location = New System.Drawing.Point(9, 352)
        Me.btn_ProfileRemove.Name = "btn_ProfileRemove"
        Me.btn_ProfileRemove.Size = New System.Drawing.Size(247, 23)
        Me.btn_ProfileRemove.TabIndex = 5
        Me.btn_ProfileRemove.Text = "Remove Selected Profile"
        Me.btn_ProfileRemove.UseVisualStyleBackColor = true
        '
        'lbl_CommandTitle
        '
        Me.lbl_CommandTitle.AutoSize = true
        Me.lbl_CommandTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 10!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbl_CommandTitle.Location = New System.Drawing.Point(13, 171)
        Me.lbl_CommandTitle.Margin = New System.Windows.Forms.Padding(5, 7, 3, 0)
        Me.lbl_CommandTitle.Name = "lbl_CommandTitle"
        Me.lbl_CommandTitle.Size = New System.Drawing.Size(40, 17)
        Me.lbl_CommandTitle.TabIndex = 14
        Me.lbl_CommandTitle.Text = "Title"
        '
        'lcl_CommandCommand
        '
        Me.lcl_CommandCommand.AutoSize = true
        Me.lcl_CommandCommand.Font = New System.Drawing.Font("Microsoft Sans Serif", 10!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lcl_CommandCommand.Location = New System.Drawing.Point(177, 171)
        Me.lcl_CommandCommand.Margin = New System.Windows.Forms.Padding(5, 7, 3, 0)
        Me.lcl_CommandCommand.Name = "lcl_CommandCommand"
        Me.lcl_CommandCommand.Size = New System.Drawing.Size(78, 17)
        Me.lcl_CommandCommand.TabIndex = 15
        Me.lcl_CommandCommand.Text = "Command"
        '
        'tb_CommandTitle
        '
        Me.tb_CommandTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tb_CommandTitle.Location = New System.Drawing.Point(11, 203)
        Me.tb_CommandTitle.Name = "tb_CommandTitle"
        Me.tb_CommandTitle.Size = New System.Drawing.Size(150, 21)
        Me.tb_CommandTitle.TabIndex = 16
        '
        'tb_CommandCommand
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.tb_CommandCommand, 2)
        Me.tb_CommandCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tb_CommandCommand.Location = New System.Drawing.Point(175, 203)
        Me.tb_CommandCommand.Name = "tb_CommandCommand"
        Me.tb_CommandCommand.Size = New System.Drawing.Size(335, 21)
        Me.tb_CommandCommand.TabIndex = 17
        '
        'btn_CommandRemove
        '
        Me.btn_CommandRemove.Location = New System.Drawing.Point(528, 422)
        Me.btn_CommandRemove.Margin = New System.Windows.Forms.Padding(15, 3, 3, 3)
        Me.btn_CommandRemove.Name = "btn_CommandRemove"
        Me.btn_CommandRemove.Size = New System.Drawing.Size(122, 29)
        Me.btn_CommandRemove.TabIndex = 19
        Me.btn_CommandRemove.Text = "Remove Selected"
        Me.btn_CommandRemove.UseVisualStyleBackColor = true
        '
        'lb_CommandTitle
        '
        Me.lb_CommandTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lb_CommandTitle.FormattingEnabled = true
        Me.lb_CommandTitle.ItemHeight = 15
        Me.lb_CommandTitle.Location = New System.Drawing.Point(11, 238)
        Me.lb_CommandTitle.Name = "lb_CommandTitle"
        Me.lb_CommandTitle.Size = New System.Drawing.Size(150, 170)
        Me.lb_CommandTitle.TabIndex = 20
        '
        'lb_CommandCommand
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.lb_CommandCommand, 3)
        Me.lb_CommandCommand.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lb_CommandCommand.FormattingEnabled = true
        Me.lb_CommandCommand.ItemHeight = 15
        Me.lb_CommandCommand.Location = New System.Drawing.Point(175, 238)
        Me.lb_CommandCommand.Name = "lb_CommandCommand"
        Me.lb_CommandCommand.Size = New System.Drawing.Size(525, 170)
        Me.lb_CommandCommand.TabIndex = 21
        '
        'btn_CommandAdd
        '
        Me.btn_CommandAdd.Location = New System.Drawing.Point(528, 203)
        Me.btn_CommandAdd.Margin = New System.Windows.Forms.Padding(15, 3, 3, 3)
        Me.btn_CommandAdd.Name = "btn_CommandAdd"
        Me.btn_CommandAdd.Size = New System.Drawing.Size(75, 21)
        Me.btn_CommandAdd.TabIndex = 18
        Me.btn_CommandAdd.Text = "Add"
        Me.btn_CommandAdd.UseVisualStyleBackColor = true
        '
        'btn_SettingsApplyOnly
        '
        Me.btn_SettingsApplyOnly.Location = New System.Drawing.Point(140, 585)
        Me.btn_SettingsApplyOnly.Name = "btn_SettingsApplyOnly"
        Me.btn_SettingsApplyOnly.Size = New System.Drawing.Size(109, 23)
        Me.btn_SettingsApplyOnly.TabIndex = 22
        Me.btn_SettingsApplyOnly.Text = "Apply"
        Me.btn_SettingsApplyOnly.UseVisualStyleBackColor = true
        '
        'btn_SettingsClose
        '
        Me.btn_SettingsClose.Location = New System.Drawing.Point(260, 585)
        Me.btn_SettingsClose.Name = "btn_SettingsClose"
        Me.btn_SettingsClose.Size = New System.Drawing.Size(109, 23)
        Me.btn_SettingsClose.TabIndex = 21
        Me.btn_SettingsClose.Text = "Close"
        Me.btn_SettingsClose.UseVisualStyleBackColor = true
        '
        'btn_SettingsApplyClose
        '
        Me.btn_SettingsApplyClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btn_SettingsApplyClose.Location = New System.Drawing.Point(20, 585)
        Me.btn_SettingsApplyClose.Name = "btn_SettingsApplyClose"
        Me.btn_SettingsApplyClose.Size = New System.Drawing.Size(109, 23)
        Me.btn_SettingsApplyClose.TabIndex = 20
        Me.btn_SettingsApplyClose.Text = "Apply  &&  Close"
        Me.btn_SettingsApplyClose.UseVisualStyleBackColor = true
        '
        'btn_SettingsClose2
        '
        Me.btn_SettingsClose2.Location = New System.Drawing.Point(590, 585)
        Me.btn_SettingsClose2.Name = "btn_SettingsClose2"
        Me.btn_SettingsClose2.Size = New System.Drawing.Size(109, 23)
        Me.btn_SettingsClose2.TabIndex = 23
        Me.btn_SettingsClose2.Text = "Close"
        Me.btn_SettingsClose2.UseVisualStyleBackColor = true
        '
        'cbTvEpSaveNfoEmpty
        '
        Me.cbTvEpSaveNfoEmpty.AutoSize = true
        Me.cbTvEpSaveNfoEmpty.Location = New System.Drawing.Point(10, 331)
        Me.cbTvEpSaveNfoEmpty.Name = "cbTvEpSaveNfoEmpty"
        Me.cbTvEpSaveNfoEmpty.Size = New System.Drawing.Size(215, 19)
        Me.cbTvEpSaveNfoEmpty.TabIndex = 79
        Me.cbTvEpSaveNfoEmpty.Text = "Save basic nfo if episode not found"
        Me.ToolTip1.SetToolTip(Me.cbTvEpSaveNfoEmpty, resources.GetString("cbTvEpSaveNfoEmpty.ToolTip"))
        Me.cbTvEpSaveNfoEmpty.UseVisualStyleBackColor = true
        '
        'frmPreferences
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 623)
        Me.Controls.Add(Me.btn_SettingsApplyOnly)
        Me.Controls.Add(Me.btn_SettingsClose)
        Me.Controls.Add(Me.btn_SettingsClose2)
        Me.Controls.Add(Me.btn_SettingsApplyClose)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.KeyPreview = true
        Me.MaximizeBox = false
        Me.MaximumSize = New System.Drawing.Size(1050, 660)
        Me.MinimizeBox = false
        Me.MinimumSize = New System.Drawing.Size(1016, 650)
        Me.Name = "frmPreferences"
        Me.Text = "Media Companion Preferences"
        Me.GroupBox12.ResumeLayout(false)
        Me.GroupBox12.PerformLayout
        Me.gbExcludeFolders.ResumeLayout(false)
        Me.gbExcludeFolders.PerformLayout
        Me.grpbxImdbCertPriority.ResumeLayout(false)
        Me.grpbxImdbCertPriority.PerformLayout
        Me.GroupBox36.ResumeLayout(false)
        Me.GroupBox36.PerformLayout
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).EndInit
        Me.TabControl1.ResumeLayout(false)
        Me.TPGenCom.ResumeLayout(false)
        Me.TabControl4.ResumeLayout(false)
        Me.TPGeneral.ResumeLayout(false)
        Me.TPGeneral.PerformLayout
        Me.GroupBox45.ResumeLayout(false)
        Me.GroupBox45.PerformLayout
        Me.GroupBox33.ResumeLayout(false)
        Me.GroupBox33.PerformLayout
        Me.GroupBox31.ResumeLayout(false)
        Me.GroupBox31.PerformLayout
        Me.GroupBox3.ResumeLayout(false)
        Me.GroupBox3.PerformLayout
        Me.TPCommonSettings.ResumeLayout(false)
        Me.TPCommonSettings.PerformLayout
        Me.gpbxPrefIgnore.ResumeLayout(false)
        Me.gpbxPrefIgnore.PerformLayout
        Me.gpbxPrefOverlay.ResumeLayout(false)
        Me.gpbxPrefOverlay.PerformLayout
        Me.GroupBox4.ResumeLayout(false)
        Me.GroupBox4.PerformLayout
        Me.gbImageResizing.ResumeLayout(false)
        Me.gbImageResizing.PerformLayout
        Me.grpCleanFilename.ResumeLayout(false)
        Me.grpCleanFilename.PerformLayout
        Me.grpVideoSource.ResumeLayout(false)
        Me.grpVideoSource.PerformLayout
        Me.gbxXBMCversion.ResumeLayout(false)
        Me.gbxXBMCversion.PerformLayout
        Me.TPActors.ResumeLayout(false)
        Me.TPActors.PerformLayout
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.GroupBox32.ResumeLayout(false)
        Me.GroupBox32.PerformLayout
        Me.TPMovPref.ResumeLayout(false)
        Me.tcMoviePreferences.ResumeLayout(false)
        Me.tpMoviePreferences_Scraper.ResumeLayout(false)
        Me.grpbxTMDbApi.ResumeLayout(false)
        Me.grpbxTMDbApi.PerformLayout
        Me.grpbxMovieScraperSettings.ResumeLayout(false)
        Me.grpbxMovieScraperSettings.PerformLayout
        Me.grpbxTMDBScraperPreferences.ResumeLayout(false)
        Me.grpbxTMDBScraperPreferences.PerformLayout
        Me.grpbxTmdbfromImdbOptions.ResumeLayout(false)
        Me.grpbxTmdbfromImdbOptions.PerformLayout
        Me.GpBx_McIMDbScraperSettings.ResumeLayout(false)
        Me.GpBx_McIMDbScraperSettings.PerformLayout
        Me.gpbxPrefScraperImages.ResumeLayout(false)
        Me.grpbxMovieTagOptions.ResumeLayout(false)
        Me.grpbxMovieTagOptions.PerformLayout
        Me.grpbxMovieBasicSave.ResumeLayout(false)
        Me.grpbxMovieBasicSave.PerformLayout
        Me.grpbxScraperLimits.ResumeLayout(false)
        Me.grpbxScraperLimits.PerformLayout
        Me.grpbxScraperMisc.ResumeLayout(false)
        Me.grpbxScraperMisc.PerformLayout
        Me.grpbxMovPreferLang.ResumeLayout(false)
        Me.grpbxMovPreferLang.PerformLayout
        Me.grpbxCustomLanguage.ResumeLayout(false)
        Me.grpbxCustomLanguage.PerformLayout
        Me.grpbxMovieFolderOptions.ResumeLayout(false)
        Me.grpbxMovieFolderOptions.PerformLayout
        Me.tpMoviePreferences_Artwork.ResumeLayout(false)
        Me.grpbxMovieSetArtwork.ResumeLayout(false)
        Me.grpbxMovieSetArtwork.PerformLayout
        Me.GrpbxXtraArtwork.ResumeLayout(false)
        Me.GrpbxXtraArtwork.PerformLayout
        Me.grpbxMoviesInFoldersArtwork.ResumeLayout(false)
        Me.grpbxMoviesInFoldersArtwork.PerformLayout
        Me.grpbxMoviePosterPriority.ResumeLayout(false)
        Me.grpbxMoviePosterPriority.PerformLayout
        Me.grpbxAutoScrapeArtwork.ResumeLayout(false)
        Me.grpbxAutoScrapeArtwork.PerformLayout
        Me.grpbxExtraArtwork.ResumeLayout(false)
        Me.grpbxExtraArtwork.PerformLayout
        Me.tpMoviePreferences_General.ResumeLayout(false)
        Me.grpbxMovieFilters.ResumeLayout(false)
        Me.grpbxMovieFilters.PerformLayout
        CType(Me.nudMaxTagsInFilter,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudMinTagsInFilter,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudMaxDirectorsInFilter,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudMaxSetsInFilter,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudDirectorsFilterMinFilms,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudSetsFilterMinFilms,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudMaxActorsInFilter,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.nudActorsFilterMinFilms,System.ComponentModel.ISupportInitialize).EndInit
        Me.grpbxMovieListOptions.ResumeLayout(false)
        Me.grpbxMovieListOptions.PerformLayout
        Me.grpbxMovieRenameOptions.ResumeLayout(false)
        Me.grpbxMovieRenameOptions.PerformLayout
        Me.grpbxMovieOfflineTitleText.ResumeLayout(false)
        Me.grpbxMovieOfflineTitleText.PerformLayout
        Me.grpbxMovieGeneralOptions.ResumeLayout(false)
        Me.grpbxMovieGeneralOptions.PerformLayout
        Me.PanelDisplayRuntime.ResumeLayout(false)
        Me.PanelDisplayRuntime.PerformLayout
        Me.grpbxMovieNameMode.ResumeLayout(false)
        Me.grpbxMovieNameMode.PerformLayout
        Me.tpMoviePreferences_Advanced.ResumeLayout(false)
        Me.tpMoviePreferences_Advanced.PerformLayout
        Me.gpbxMovieExcludeYearSearch.ResumeLayout(false)
        Me.gpbxMovieExcludeYearSearch.PerformLayout
        Me.gpbx_MovTimerAutoScrape.ResumeLayout(false)
        Me.gpbx_MovTimerAutoScrape.PerformLayout
        Me.grpbxMovieCustomPlaycountTag.ResumeLayout(false)
        Me.grpbxMovieCustomPlaycountTag.PerformLayout
        Me.grpbxMovieSeparateIdentifier.ResumeLayout(false)
        Me.grpbxMovieSeparateIdentifier.PerformLayout
        Me.grpbxMovieNfoPosterOptions.ResumeLayout(false)
        Me.grpbxMovieNfoPosterOptions.PerformLayout
        Me.TPTVPref.ResumeLayout(false)
        Me.TabControl6.ResumeLayout(false)
        Me.TabPage30.ResumeLayout(false)
        Me.GroupBox17.ResumeLayout(false)
        Me.GroupBox17.PerformLayout
        Me.GroupBox43.ResumeLayout(false)
        Me.GroupBox43.PerformLayout
        Me.GroupBox22.ResumeLayout(false)
        Me.GroupBox22.PerformLayout
        Me.GroupBox20.ResumeLayout(false)
        Me.GroupBox20.PerformLayout
        Me.GroupBox18.ResumeLayout(false)
        Me.GroupBox18.PerformLayout
        Me.GroupBox19.ResumeLayout(false)
        Me.GroupBox19.PerformLayout
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        Me.GroupBox_TVDB_Scraper_Preferences.ResumeLayout(false)
        Me.GroupBox_TVDB_Scraper_Preferences.PerformLayout
        Me.TabPage31.ResumeLayout(false)
        Me.GroupBox_tv_RegexRename.ResumeLayout(false)
        Me.GroupBox_tv_RegexRename.PerformLayout
        Me.GroupBox_tv_RegexScrape.ResumeLayout(false)
        Me.GroupBox_tv_RegexScrape.PerformLayout
        Me.GroupBox_tv_RegexScrape_Test.ResumeLayout(false)
        Me.GroupBox_tv_RegexScrape_Test.PerformLayout
        Me.TPHmPref.ResumeLayout(false)
        Me.TPHmPref.PerformLayout
        Me.GroupBox5.ResumeLayout(false)
        Me.GroupBox5.PerformLayout
        Me.TPProxy.ResumeLayout(false)
        Me.TPXBMCLink.ResumeLayout(false)
        Me.TPPRofCmd.ResumeLayout(false)
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.TableLayoutPanel1.PerformLayout
        Me.GroupBox42.ResumeLayout(false)
        Me.GroupBox42.PerformLayout
        Me.GroupBox15.ResumeLayout(false)
        Me.GroupBox15.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents FontDialog As System.Windows.Forms.FontDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox15 As System.Windows.Forms.GroupBox
    Friend WithEvents btn_ProfileSetStartup As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lb_ProfileList As System.Windows.Forms.ListBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btn_ProfileSetDefault As System.Windows.Forms.Button
    Friend WithEvents tb_ProfileNew As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents btn_ProfileAdd As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btn_ProfileRemove As System.Windows.Forms.Button
    Friend WithEvents GroupBox42 As System.Windows.Forms.GroupBox
    Friend WithEvents Label141 As System.Windows.Forms.Label
    Friend WithEvents lbl_CommandTitle As System.Windows.Forms.Label
    Friend WithEvents lcl_CommandCommand As System.Windows.Forms.Label
    Friend WithEvents tb_CommandTitle As System.Windows.Forms.TextBox
    Friend WithEvents tb_CommandCommand As System.Windows.Forms.TextBox
    Friend WithEvents btn_CommandAdd As System.Windows.Forms.Button
    Friend WithEvents btn_CommandRemove As System.Windows.Forms.Button
    Friend WithEvents lb_CommandTitle As System.Windows.Forms.ListBox
    Friend WithEvents lb_CommandCommand As System.Windows.Forms.ListBox
    Friend WithEvents btn_SettingsApplyOnly As System.Windows.Forms.Button
    Friend WithEvents btn_SettingsClose As System.Windows.Forms.Button
    Friend WithEvents btn_SettingsApplyClose As System.Windows.Forms.Button
    Friend WithEvents TPGenCom As TabPage
    Friend WithEvents TabControl4 As TabControl
    Friend WithEvents TPCommonSettings As TabPage
    Friend WithEvents TPActors As TabPage
    Friend WithEvents TPMovPref As TabPage
    Friend WithEvents TPTVPref As TabPage
    Friend WithEvents TPPRofCmd As TabPage
    Friend WithEvents TPProxy As TabPage
    Friend WithEvents UcGenPref_Proxy1 As ucGenPref_Proxy
    Friend WithEvents TPXBMCLink As TabPage
    Friend WithEvents UcGenPref_XbmcLink1 As ucGenPref_XbmcLink
    Friend WithEvents GroupBox32 As GroupBox
    Friend WithEvents Label137 As Label
    Friend WithEvents cb_actorseasy As CheckBox
    Friend WithEvents GroupBox12 As GroupBox
    Friend WithEvents cbLocalActorSaveAlpha As CheckBox
    Friend WithEvents xbmcactorpath As TextBox
    Friend WithEvents btn_localactorpathbrowse As Button
    Friend WithEvents Label161 As Label
    Friend WithEvents Label104 As Label
    Friend WithEvents Label103 As Label
    Friend WithEvents Label101 As Label
    Friend WithEvents Label96 As Label
    Friend WithEvents Label97 As Label
    Friend WithEvents localactorpath As TextBox
    Friend WithEvents cbsaveactor As CheckBox
    Friend WithEvents Label98 As Label
    Friend WithEvents cmbx_MovMaxActors As ComboBox
    Friend WithEvents cbShowAllAudioTracks As CheckBox
    Friend WithEvents cbDisplayMediaInfoOverlay As CheckBox
    Friend WithEvents cbDisplayMediaInfoFolderSize As CheckBox
    Friend WithEvents cbDisplayRatingOverlay As CheckBox
    Friend WithEvents gbExcludeFolders As GroupBox
    Friend WithEvents tbExcludeFolders As TextBox
    Friend WithEvents cb_IgnoreAn As CheckBox
    Friend WithEvents cb_IgnoreA As CheckBox
    Friend WithEvents cbOverwriteArtwork As CheckBox
    Friend WithEvents cb_IgnoreThe As CheckBox
    Friend WithEvents cbRuntimeAsNumericOnly As CheckBox
    Friend WithEvents gbxXBMCversion As GroupBox
    Friend WithEvents Label129 As Label
    Friend WithEvents rbXBMCv_both As RadioButton
    Friend WithEvents rbXBMCv_post As RadioButton
    Friend WithEvents rbXBMCv_pre As RadioButton
    Friend WithEvents grpVideoSource As GroupBox
    Friend WithEvents btnVideoSourceRemove As Button
    Friend WithEvents txtVideoSourceAdd As TextBox
    Friend WithEvents btnVideoSourceAdd As Button
    Friend WithEvents lbVideoSource As ListBox
    Friend WithEvents grpCleanFilename As GroupBox
    Friend WithEvents btnCleanFilenameRemove As Button
    Friend WithEvents txtCleanFilenameAdd As TextBox
    Friend WithEvents btnCleanFilenameAdd As Button
    Friend WithEvents lbCleanFilename As ListBox
    Friend WithEvents gbImageResizing As GroupBox
    Friend WithEvents Label171 As Label
    Friend WithEvents Label175 As Label
    Friend WithEvents Label176 As Label
    Friend WithEvents comboActorResolutions As ComboBox
    Friend WithEvents comboBackDropResolutions As ComboBox
    Friend WithEvents comboPosterResolutions As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label185 As Label
    Friend WithEvents AutoScrnShtDelay As TextBox
    Friend WithEvents btn_SettingsClose2 As Button
    Friend WithEvents tcMoviePreferences As TabControl
    Friend WithEvents tpMoviePreferences_Scraper As TabPage
    Friend WithEvents grpbxMovieScraperSettings As GroupBox
    Friend WithEvents cbMovieUseXBMCScraper As CheckBox
    Friend WithEvents grpbxTMDBScraperPreferences As GroupBox
    Friend WithEvents cmbxTMDBPreferredCertCountry As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents grpbxTmdbfromImdbOptions As GroupBox
    Friend WithEvents cbXbmcTmdbAkasFromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbCertFromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbVotesFromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbTop250FromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbIMDBRatings As CheckBox
    Friend WithEvents cbXbmcTmdbStarsFromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbOutlineFromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbActorFromImdb As CheckBox
    Friend WithEvents cbXbmcTmdbRename As CheckBox
    Friend WithEvents Label155 As Label
    Friend WithEvents cmbxXbmcTmdbTitleLanguage As ComboBox
    Friend WithEvents cbXbmcTmdbFanart As CheckBox
    Friend WithEvents cmbxXbmcTmdbHDTrailer As ComboBox
    Friend WithEvents Label153 As Label
    Friend WithEvents GpBx_McIMDbScraperSettings As GroupBox
    Friend WithEvents cbImdbPrimaryPlot As CheckBox
    Friend WithEvents cbImdbgetTMDBActor As CheckBox
    Friend WithEvents Label90 As Label
    Friend WithEvents Label91 As Label
    Friend WithEvents lb_IMDBMirrors As ListBox
    Friend WithEvents gpbxPrefScraperImages As GroupBox
    Friend WithEvents grpbxImdbCertPriority As GroupBox
    Friend WithEvents ScrapeFullCertCheckBox As CheckBox
    Friend WithEvents Label178 As Label
    Friend WithEvents Label95 As Label
    Friend WithEvents btnMovImdbPriorityDown As Button
    Friend WithEvents btnMovImdbPriorityUp As Button
    Friend WithEvents Label94 As Label
    Friend WithEvents lb_IMDBCertPriority As ListBox
    Friend WithEvents grpbxMovieTagOptions As GroupBox
    Friend WithEvents Label69 As Label
    Friend WithEvents cb_keywordlimit As ComboBox
    Friend WithEvents cb_keywordasTag As CheckBox
    Friend WithEvents grpbxMovieBasicSave As GroupBox
    Friend WithEvents cbMovieBasicSave As CheckBox
    Friend WithEvents Label162 As Label
    Friend WithEvents Label109 As Label
    Friend WithEvents grpbxScraperLimits As GroupBox
    Friend WithEvents cmbxMovieScraper_MaxStudios As ComboBox
    Friend WithEvents lblMaxStudios As Label
    Friend WithEvents Label92 As Label
    Friend WithEvents cmbxMovScraper_MaxGenres As ComboBox
    Friend WithEvents grpbxScraperMisc As GroupBox
    Friend WithEvents cbIMDbOriginalTitle As CheckBox
    Friend WithEvents cbGetMovieSetFromTMDb As CheckBox
    Friend WithEvents grpbxMovPreferLang As GroupBox
    Friend WithEvents Label27 As Label
    Friend WithEvents grpbxCustomLanguage As GroupBox
    Friend WithEvents llLanguagesFile As LinkLabel
    Friend WithEvents tbCustomLanguageValue As TextBox
    Friend WithEvents Label174 As Label
    Friend WithEvents Label177 As Label
    Friend WithEvents cbUseCustomLanguage As CheckBox
    Friend WithEvents cmbxTMDbSelectedLanguage As ComboBox
    Friend WithEvents grpbxMovieFolderOptions As GroupBox
    Friend WithEvents cbMovieAllInFolders As CheckBox
    Friend WithEvents cbMovieUseFolderNames As CheckBox
    Friend WithEvents tpMoviePreferences_Artwork As TabPage
    Friend WithEvents grpbxMovieSetArtwork As GroupBox
    Friend WithEvents Label37 As Label
    Friend WithEvents tbMovSetArtCentralFolder As TextBox
    Friend WithEvents btnMovSetCentralFolderSelect As Button
    Friend WithEvents rbMovSetArtSetFolder As RadioButton
    Friend WithEvents rbMovSetFolder As RadioButton
    Friend WithEvents GrpbxXtraArtwork As GroupBox
    Friend WithEvents Label30 As Label
    Friend WithEvents cmbxMovXtraFanartQty As ComboBox
    Friend WithEvents grpbxMoviesInFoldersArtwork As GroupBox
    Friend WithEvents cbMovCreateFanartjpg As CheckBox
    Friend WithEvents cbMoviePosterInFolder As CheckBox
    Friend WithEvents cbMovieFanartInFolders As CheckBox
    Friend WithEvents cbMovCreateFolderjpg As CheckBox
    Friend WithEvents cbMovXtraFanart As CheckBox
    Friend WithEvents cbMovXtraThumbs As CheckBox
    Friend WithEvents grpbxMoviePosterPriority As GroupBox
    Friend WithEvents btn_MovPosterPriorityRemove As Button
    Friend WithEvents btn_MovPosterPriorityReset As Button
    Friend WithEvents Label99 As Label
    Friend WithEvents Label93 As Label
    Friend WithEvents btnMovPosterPriorityDown As Button
    Friend WithEvents btnMovPosterPriorityUp As Button
    Friend WithEvents lbPosterSourcePriorities As ListBox
    Friend WithEvents grpbxAutoScrapeArtwork As GroupBox
    Friend WithEvents cbMovFanartNaming As CheckBox
    Friend WithEvents btnMovFanartTvSelect As Button
    Friend WithEvents Label10 As Label
    Friend WithEvents cbMovFanartTvScrape As CheckBox
    Friend WithEvents grpbxExtraArtwork As GroupBox
    Friend WithEvents Label189 As Label
    Friend WithEvents cbDlXtraFanart As CheckBox
    Friend WithEvents cbMovSetArtScrape As CheckBox
    Friend WithEvents cbMovFanartScrape As CheckBox
    Friend WithEvents cbMoviePosterScrape As CheckBox
    Friend WithEvents tpMoviePreferences_General As TabPage
    Friend WithEvents grpbxMovieFilters As GroupBox
    Friend WithEvents cbMovieFilters_Sets_Order As ComboBox
    Friend WithEvents Label54 As Label
    Friend WithEvents cbMovieFilters_Directors_Order As ComboBox
    Friend WithEvents nudMaxDirectorsInFilter As NumericUpDown
    Friend WithEvents nudMaxSetsInFilter As NumericUpDown
    Friend WithEvents nudDirectorsFilterMinFilms As NumericUpDown
    Friend WithEvents nudSetsFilterMinFilms As NumericUpDown
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents cbMovieFilters_Actors_Order As ComboBox
    Friend WithEvents cbDisableNotMatchingRenamePattern As CheckBox
    Friend WithEvents Label180 As Label
    Friend WithEvents Label164 As Label
    Friend WithEvents nudMaxActorsInFilter As NumericUpDown
    Friend WithEvents nudActorsFilterMinFilms As NumericUpDown
    Friend WithEvents Label165 As Label
    Friend WithEvents cbMissingMovie As CheckBox
    Friend WithEvents grpbxMovieListOptions As GroupBox
    Friend WithEvents cbMovieList_ShowColWatched As CheckBox
    Friend WithEvents cbMovieList_ShowColPlot As CheckBox
    Friend WithEvents tbDateFormat As TextBox
    Friend WithEvents Label179 As Label
    Friend WithEvents cbMovieShowDateOnList As CheckBox
    Friend WithEvents grpbxMovieRenameOptions As GroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents rbRenameFullStop As RadioButton
    Friend WithEvents rbRenameUnderscore As RadioButton
    Friend WithEvents cbMovNewFolderInRootFolder As CheckBox
    Friend WithEvents cbMovSortIgnArticle As CheckBox
    Friend WithEvents cbMovTitleIgnArticle As CheckBox
    Friend WithEvents cbMovSetIgnArticle As CheckBox
    Friend WithEvents Label197 As Label
    Friend WithEvents Label196 As Label
    Friend WithEvents cbMovFolderRename As CheckBox
    Friend WithEvents cbRenameUnderscore As CheckBox
    Friend WithEvents lblFolderRename As Label
    Friend WithEvents tb_MovFolderRename As TextBox
    Friend WithEvents LblFilename As Label
    Friend WithEvents cbMovieManualRename As CheckBox
    Friend WithEvents cbMovieRenameEnable As CheckBox
    Friend WithEvents Label100 As Label
    Friend WithEvents tb_MovieRenameTemplate As TextBox
    Friend WithEvents grpbxMovieOfflineTitleText As GroupBox
    Friend WithEvents Label77 As Label
    Friend WithEvents tb_OfflineDVDTitle As TextBox
    Friend WithEvents grpbxMovieGeneralOptions As GroupBox
    Friend WithEvents cb_MovDurationAsRuntine As CheckBox
    Friend WithEvents cbMovThousSeparator As CheckBox
    Friend WithEvents cbMovTitleCase As CheckBox
    Friend WithEvents cbXtraFrodoUrls As CheckBox
    Friend WithEvents cbNoAltTitle As CheckBox
    Friend WithEvents Label102 As Label
    Friend WithEvents Label78 As Label
    Friend WithEvents cbPreferredTrailerResolution As ComboBox
    Friend WithEvents cbDlTrailerDuringScrape As CheckBox
    Friend WithEvents PanelDisplayRuntime As Panel
    Friend WithEvents cbMovieRuntimeFallbackToFile As CheckBox
    Friend WithEvents Label70 As Label
    Friend WithEvents rbRuntimeFile As RadioButton
    Friend WithEvents rbRuntimeScraper As RadioButton
    Friend WithEvents cb_MovDisplayLog As CheckBox
    Friend WithEvents cb_EnableMediaTags As CheckBox
    Friend WithEvents cbMovieTrailerUrl As CheckBox
    Friend WithEvents grpbxMovieNameMode As GroupBox
    Friend WithEvents Label163 As Label
    Friend WithEvents lblNameMode As Label
    Friend WithEvents cbMoviePartsIgnorePart As CheckBox
    Friend WithEvents lblNameModeEg As Label
    Friend WithEvents cbMoviePartsNameMode As CheckBox
    Friend WithEvents tpMoviePreferences_Advanced As TabPage
    Friend WithEvents grpbxMovieSeparateIdentifier As GroupBox
    Friend WithEvents btn_MovSepReset As Button
    Friend WithEvents Label198 As Label
    Friend WithEvents btn_MovSepRem As Button
    Friend WithEvents btn_MovSepAdd As Button
    Friend WithEvents tb_MovSeptb As TextBox
    Friend WithEvents lb_MovSepLst As ListBox
    Friend WithEvents grpbxMovieNfoPosterOptions As GroupBox
    Friend WithEvents Label88 As Label
    Friend WithEvents imdb_chk As CheckBox
    Friend WithEvents mpdb_chk As CheckBox
    Friend WithEvents tmdb_chk As CheckBox
    Friend WithEvents IMPA_chk As CheckBox
    Friend WithEvents Label89 As Label
    Friend WithEvents cbMovRootFolderCheck As CheckBox
    Friend WithEvents TabControl6 As TabControl
    Friend WithEvents TabPage30 As TabPage
    Friend WithEvents GroupBox17 As GroupBox
    Friend WithEvents cbTvScrShtTVDBResize As CheckBox
    Friend WithEvents GroupBox43 As GroupBox
    Friend WithEvents cb_TvMissingEpOffset As CheckBox
    Friend WithEvents cbTvMissingSpecials As CheckBox
    Friend WithEvents GroupBox_TVDB_Scraper_Preferences As GroupBox
    Friend WithEvents cbXBMCTvdbRatingFallback As CheckBox
    Friend WithEvents cbXBMCTvdbRatingImdb As CheckBox
    Friend WithEvents ComboBox_TVDB_Language As ComboBox
    Friend WithEvents Label154 As Label
    Friend WithEvents cbXBMCTvdbPosters As CheckBox
    Friend WithEvents cbXBMCTvdbFanart As CheckBox
    Friend WithEvents rbXBMCTvdbAbsoluteNumber As RadioButton
    Friend WithEvents rbXBMCTvdbDVDOrder As RadioButton
    Friend WithEvents Label111 As Label
    Friend WithEvents cbTv_fixNFOid As CheckBox
    Friend WithEvents GroupBox22 As GroupBox
    Friend WithEvents btnTVDBGetLanguages As Button
    Friend WithEvents rbTvEpSortDefault As RadioButton
    Friend WithEvents rbTvEpSortDVD As RadioButton
    Friend WithEvents Label124 As Label
    Friend WithEvents Label123 As Label
    Friend WithEvents Label138 As Label
    Friend WithEvents cbTvSeriesActorstoEpisodeActors As CheckBox
    Friend WithEvents cbTvAutoScreenShot As CheckBox
    Friend WithEvents cbTvUse_XBMC_TVDB_Scraper As CheckBox
    Friend WithEvents Label139 As Label
    Friend WithEvents GroupBox20 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbxTvXtraFanartQty As ComboBox
    Friend WithEvents cbTvFanartTvFirst As CheckBox
    Friend WithEvents cbTvDlFanartTvArt As CheckBox
    Friend WithEvents cbSeasonFolderjpg As CheckBox
    Friend WithEvents cb_TvFolderJpg As CheckBox
    Friend WithEvents cbTvDlXtraFanart As CheckBox
    Friend WithEvents GroupBox18 As GroupBox
    Friend WithEvents posterbtn As RadioButton
    Friend WithEvents bannerbtn As RadioButton
    Friend WithEvents cbTvDlPosterArt As CheckBox
    Friend WithEvents cbTvDlSeasonArt As CheckBox
    Friend WithEvents cbTvDlFanart As CheckBox
    Friend WithEvents GroupBox19 As GroupBox
    Friend WithEvents RadioButton39 As RadioButton
    Friend WithEvents RadioButton40 As RadioButton
    Friend WithEvents RadioButton41 As RadioButton
    Friend WithEvents cbTvQuickAddShow As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CheckBox_tv_EpisodeRenameCase As CheckBox
    Friend WithEvents CheckBox_tv_EpisodeRenameAuto As CheckBox
    Friend WithEvents Label140 As Label
    Friend WithEvents ComboBox_tv_EpisodeRename As ComboBox
    Friend WithEvents cbTvEpEnableHDTags As CheckBox
    Friend WithEvents cbTvDisableLogs As CheckBox
    Friend WithEvents lbxTVDBLangs As ListBox
    Friend WithEvents Label122 As Label
    Friend WithEvents ComboBox8 As ComboBox
    Friend WithEvents TabPage31 As TabPage
    Friend WithEvents GroupBox_tv_RegexRename As GroupBox
    Friend WithEvents btn_tv_RegexRename_Restore As Button
    Friend WithEvents lb_tv_RegexRename As ListBox
    Friend WithEvents btn_tv_RegexRename_Remove As Button
    Friend WithEvents Label158 As Label
    Friend WithEvents btn_tv_RegexRename_MoveDown As Button
    Friend WithEvents Label159 As Label
    Friend WithEvents btn_tv_RegexRename_MoveUp As Button
    Friend WithEvents tb_tv_RegexRename_New As TextBox
    Friend WithEvents tb_tv_RegexRename_Edit As TextBox
    Friend WithEvents btn_tv_RegexRename_Add As Button
    Friend WithEvents btn_tv_RegexRename_Edit As Button
    Friend WithEvents GroupBox_tv_RegexScrape As GroupBox
    Friend WithEvents lb_tv_RegexScrape As ListBox
    Friend WithEvents btn_tv_RegexScrape_Remove As Button
    Friend WithEvents Label119 As Label
    Friend WithEvents btn_tv_RegexScrape_MoveDown As Button
    Friend WithEvents Label117 As Label
    Friend WithEvents btn_tv_RegexScrape_MoveUp As Button
    Friend WithEvents tb_tv_RegexScrape_New As TextBox
    Friend WithEvents tb_tv_RegexScrape_Edit As TextBox
    Friend WithEvents btn_tv_RegexScrape_Add As Button
    Friend WithEvents btn_tv_RegexScrape_Edit As Button
    Friend WithEvents btn_tv_RegexScrape_Restore As Button
    Friend WithEvents GroupBox_tv_RegexScrape_Test As GroupBox
    Friend WithEvents tb_tv_RegexScrape_TestResult As TextBox
    Friend WithEvents btn_tv_RegexScrape_Test As Button
    Friend WithEvents tb_tv_RegexScrape_TestString As TextBox
    Friend WithEvents Label118 As Label
    Friend WithEvents cbMovImdbFirstRunTime As CheckBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents cb_SorttitleIgnoreArticles As CheckBox
    Friend WithEvents cb_MovSetTitleIgnArticle As CheckBox
    Friend WithEvents cb_MovPosterTabTMDBSelect As CheckBox
    Friend WithEvents cb_TvRenameReplaceSpace As CheckBox
    Friend WithEvents rb_TvRenameReplaceSpaceUnderScore As RadioButton
    Friend WithEvents rb_TvRenameReplaceSpaceDot As RadioButton
    Friend WithEvents Label9 As Label
    Friend WithEvents tb_MovTagBlacklist As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cbShowMovieGridToolTip As CheckBox
    Friend WithEvents cb_MovCertRemovePhrase As CheckBox
    Friend WithEvents cbAllowUserTags As System.Windows.Forms.CheckBox
    Friend WithEvents cbExcludeMpaaRated As CheckBox
    Friend WithEvents cbIncludeMpaaRated As CheckBox
    Friend WithEvents cbEnableFolderSize As CheckBox
    Friend WithEvents btnEditCustomGenreFile As Button
    Friend WithEvents TPHmPref As TabPage
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents tb_HmFanartTime As TextBox
    Friend WithEvents tb_HmPosterTime As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents lbl_HmHeader As Label
    Friend WithEvents cb_HmFanartScrnShot As CheckBox
    Friend WithEvents cbGenreCustomBefore As CheckBox
    Friend WithEvents cbXbmcTmdbGenreFromImdb As CheckBox
    Friend WithEvents TPGeneral As TabPage
    Friend WithEvents cbMcCloseMCForDLNewVersion As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cbMultiMonitorEnable As CheckBox
    Friend WithEvents cbDisplayLocalActor As CheckBox
    Friend WithEvents cbCheckForNewVersion As CheckBox
    Friend WithEvents cbRenameNFOtoINFO As CheckBox
    Friend WithEvents cbUseMultipleThreads As CheckBox
    Friend WithEvents cbShowLogOnError As CheckBox
    Friend WithEvents btnFindBrowser As Button
    Friend WithEvents cbExternalbrowser As CheckBox
    Friend WithEvents chkbx_disablecache As CheckBox
    Friend WithEvents GroupBox45 As GroupBox
    Friend WithEvents lblaltnfoeditorclear As Label
    Friend WithEvents btnaltnfoeditor As Button
    Friend WithEvents tbaltnfoeditor As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents GroupBox36 As GroupBox
    Friend WithEvents llMkvMergeGuiPath As LinkLabel
    Friend WithEvents btnMkvMergeGuiPath As Button
    Friend WithEvents tbMkvMergeGuiPath As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents GroupBox33 As GroupBox
    Friend WithEvents btnFontSelect As Button
    Friend WithEvents btnFontReset As Button
    Friend WithEvents lbl_FontSample As Label
    Friend WithEvents GroupBox31 As GroupBox
    Friend WithEvents Label116 As Label
    Friend WithEvents Label107 As Label
    Friend WithEvents txtbx_minrarsize As TextBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lbl_MediaPlayerUser As Label
    Friend WithEvents btn_MediaPlayerBrowse As Button
    Friend WithEvents rb_MediaPlayerUser As RadioButton
    Friend WithEvents rb_MediaPlayerWMP As RadioButton
    Friend WithEvents rb_MediaPlayerDefault As RadioButton
    Friend WithEvents Label21 As Label
    Friend WithEvents cb_MovRuntimeAsDuration As CheckBox
    Friend WithEvents grpbxTMDbApi As GroupBox
    Friend WithEvents Label22 As Label
    Friend WithEvents tbTMDbAPI As TextBox
    Friend WithEvents grpbxMovieCustomPlaycountTag As GroupBox
    Friend WithEvents cbMovNfoWatchTag As CheckBox
    Friend WithEvents Label23 As Label
    Friend WithEvents cbMovCustFolderjpgNoDelete As CheckBox
    Friend WithEvents cbAutoHideStatusBar As CheckBox
    Friend WithEvents cbMovImdbAspectRatio As CheckBox
    Friend WithEvents cbtvdbIMDbRating As CheckBox
    Friend WithEvents Label24 As Label
    Friend WithEvents cmbxTvMaxGenres As ComboBox
    Friend WithEvents cbTagRes As CheckBox
    Friend WithEvents cbMovCustPosterjpgNoDelete As CheckBox
    Friend WithEvents cbDisplayAllSubtitleLang As CheckBox
    Friend WithEvents cbDisplayDefaultSubtitleLang As CheckBox
    Friend WithEvents cbXbmcTmdbAspectFromImdb As CheckBox
    Friend WithEvents cmbxMovFiltersTagsOrder As ComboBox
    Friend WithEvents Label25 As Label
    Friend WithEvents nudMaxTagsInFilter As NumericUpDown
    Friend WithEvents nudMinTagsInFilter As NumericUpDown
    Friend WithEvents cbXbmcTmdbMetascoreFromImdb As CheckBox
    Friend WithEvents gpbxPrefIgnore As GroupBox
    Friend WithEvents gpbxPrefOverlay As GroupBox
    Friend WithEvents cbSetIdAsCollectionnumber As CheckBox
    Friend WithEvents lbExternalBrowserPath As Label
    Friend WithEvents cbtvDisplayNextAiredToolTip As CheckBox
    Friend WithEvents cbLocalActorSaveNoId As CheckBox
    Friend WithEvents cbMovActorFallbackIMDbtoTMDb As CheckBox
    Friend WithEvents cbMovActorFallbackTMDbtoIMDb As CheckBox
    Friend WithEvents Label26 As Label
    Friend WithEvents cbRunTimePadding As CheckBox
    Friend WithEvents cbTvThousSeparator As CheckBox
    Friend WithEvents cbEnableMovDeleteFolderTsmi As CheckBox
    Friend WithEvents cbMovSetOverviewToNfo As CheckBox
    Friend WithEvents Label28 As Label
    Friend WithEvents tbTvAutoScrapeInterval As TextBox
    Friend WithEvents cbTvEnableAutoScrape As CheckBox
    Friend WithEvents gpbx_MovTimerAutoScrape As GroupBox
    Friend WithEvents tbMovAutoScrapeInterval As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents cbMovEnableAutoScrape As CheckBox
    Friend WithEvents cbTvDlEpisodeThumb As CheckBox
    Friend WithEvents gpbxMovieExcludeYearSearch As GroupBox
    Friend WithEvents cbMovieExcludeYearSearch As CheckBox
    Friend WithEvents Label31 As Label
    Friend WithEvents cbMovAllowNonImdbIdAsId As CheckBox
    Friend WithEvents cb_ExcludeActorNoThumb As CheckBox
    Friend WithEvents rbMovSetArtScrapeFanartTv As RadioButton
    Friend WithEvents rbMovSetArtScrapeTMDb As RadioButton
    Friend WithEvents Label32 As Label
    Friend WithEvents cbTvEpSaveNfoEmpty As CheckBox
End Class
