Option Explicit On

Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Runtime.InteropServices
Imports Media_Companion.Pref
Imports Media_Companion.XBMCScraperSettings
Imports System.Xml
Imports System.Reflection
Imports System.ComponentModel
Imports System.Linq
Imports XBMC.JsonRpc

#Const SilentErrorScream = False
#Const Refocus = False


Public Class Form1

	'Const HOME_PAGE = "http://mediacompanion.codeplex.com"
    Const SF_Home_Page = "https://sourceforge.net/projects/mediacompanion/"
	Const TMDB_SITE = "www.themoviedb.org/"
	Const TMDB_SET_URL = TMDB_SITE & "collection/"
	Const TMDB_MOVIE_URL = TMDB_SITE & "movie/"
	Const RELATIVE_SIZE_THRESHOLD = 0.63F
	Const MIN_MEDIA_SIZE As Integer = 10000000
    
    Public Const XBMC_Controller_full_log_file As String = "XBMC-Controller-full-log-file.txt"
	Public Const XBMC_Controller_brief_log_file As String = "XBMC-Controller-brief-log-file.txt"
	Public Const MCToolsCommands        As Integer = 5          ' increment when adding MC functions to ToolsToolStripMenuItem

	Public Dim WithEvents BckWrkScnMovies As BackgroundWorker = New BackgroundWorker
	Public Dim WithEvents BckWrkCheckNewVersion As BackgroundWorker = New BackgroundWorker
	Public Dim WithEvents BckWrkXbmcController As BackgroundWorker = New BackgroundWorker
	Public Dim WithEvents Bw            As BackgroundWorker = New BackgroundWorker
	Public Dim WithEvents ImgBw         As BackgroundWorker = New BackgroundWorker
    Public Dim WithEvents BckWrkTv      As BackgroundWorker = New BackgroundWorker
    Public Dim WithEvents BckWrkSeries  As BackgroundWorker = New BackgroundWorker
	Property BWs                        As New List(Of BackgroundWorker)
	Property NumActiveThreads           As Integer
	Shared Public XbmcControllerQ       As PriorityQueue = New PriorityQueue
	Shared Public XbmcControllerBufferQ As PriorityQueue = New PriorityQueue
	Shared Public Property MC_Only_Movies As List(Of ComboList)
	Public Shared Property MaxXbmcMovies As List(Of MaxXbmcMovie)
	Shared Public MyCulture             As New System.Globalization.CultureInfo("en-US")
    Public Shared DecimalSeparator      As String = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator
	Private Declare Function GetActiveWindow Lib "user32" Alias "GetActiveWindow" () As IntPtr

	Public Property XBMC_Controller_LogLastShownDt As Date = Now
	Private XBMC_Link_ErrorLog_Timer    As Timers.Timer = New Timers.Timer()
	Private XBMC_Link_Idle_Timer        As Timers.Timer = New Timers.Timer()
	Private XBMC_Link_Check_Timer       As Timers.Timer = New Timers.Timer()

	Declare Function AttachConsole Lib "kernel32.dll" (ByVal dwProcessId As Int32) As Boolean

	Private WithEvents Tmr              As New Windows.Forms.Timer With {.Interval = 200}
	Private fb                          As New FolderBrowserDialog
	Private Const WM_USER               As Integer = &H400
	Private Const BFFM_SETEXPANDED      As Integer = WM_USER + 106

	<DllImport("user32.dll", EntryPoint:="SendMessageW")> _
	Private Shared Function SendMessageW(ByVal hWnd As IntPtr, ByVal msg As UInteger, ByVal wParam As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As String) As IntPtr
	End Function

	<DllImport("user32.dll", EntryPoint:="FindWindowW")> _
	Private Shared Function FindWindowW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpClassName As String, <MarshalAs(UnmanagedType.LPWStr)> ByVal lpWindowName As String) As IntPtr
	End Function

	Shared ReadOnly Property Link_TotalQCount
		Get
			Return XbmcControllerQ.Count + XbmcControllerBufferQ.Count
		End Get
	End Property

	Shared ReadOnly Property MC_Only_Movies_Nfos As List(Of String)
		Get
			If IsNothing(MC_Only_Movies) Then Return New List(Of String)

			Return (From M In MC_Only_Movies Select fullpathandfilename = M.fullpathandfilename).ToList
		End Get
	End Property

	ReadOnly Shared Property NumOfScreens As Integer
		Get
			Return System.Windows.Forms.Screen.AllScreens.GetUpperBound(0)
		End Get
	End Property

	ReadOnly Shared Property CurrentScreen As Integer
		Get
			Try
				Dim display As String = System.Windows.Forms.Screen.FromControl(Form1).DeviceName
				Dim m As String = Regex.Match(display, "DISPLAY[0-9]").Value
				Return ToInt(m.Substring(m.Length - 1)) - 1
			Catch
				Return 0
			End Try
		End Get
	End Property

	Property frmXBMC_Progress           As frmXBMC_Progress = New frmXBMC_Progress

#Region "Movie scraping related objects"
	Public WithEvents oMovies           As New Movies
	Public filteredList                 As New List(Of ComboList)
	Public rescrapeList                 As New RescrapeList
	Public workingMovieDetails          As FullMovieDetails
	Public _rescrapeList                As New RescrapeSpecificParams
	Public _lockList                    As New LockSpecificParams
	Public ChangeMovieId                As String = ""
	Public droppedItems                 As New List(Of String)
	Public CtrlsToDisableMovieScrape    As IEnumerable(Of Control)

	Public Shared blnAbortFileDownload  As Boolean
	Public Shared ReadOnly countLock    = New Object
	Public ScraperErrorDetected         As Boolean

#End Region 'Movie scraping objects


	Enum ProgramState
		ResettingFilters
		UpdatingFilteredList
		ResizingSplitterPanel
		MovieControlsDisabled
		Other
        Closing
	End Enum

	Shared Public ProgState             As ProgramState = ProgramState.Other
	Public StateBefore                  As ProgramState = ProgramState.Other

	Public DataDirty                    As Boolean
	Public _yield                       As Boolean
	Public lastNfo                      As String = ""
    Private lastTvNfo                   As String = ""
	Public MainFormLoadedStatus         As Boolean = False
    Private tvfiltertrip                As Boolean = False
	Public messbox                      As New frmMessageBox("blank", "", "")
	Public startup                      As Boolean = True
	Public scraperFunction2             As New ScraperFunctions
	Public nfoFunction                  As New WorkingWithNfoFiles
	Public mediaInfoExp                 As New MediaInfoExport
	Shared Public langarray(300, 3)     As String
	Public screen                       As Screen
	Public Shared genrelist             As New List(Of String)

	Public Data_GridViewMovie           As Data_GridViewMovie
	Public DataGridViewBindingSource    As New BindingSource
    
	Public workingMovie                 As New ComboList
	Public tvBatchList                  As New str_TvShowBatchWizard(SetDefaults)

	Public moviefolderschanged          As Boolean = False
	Public tvfolderschanged             As Boolean = False
	Public customTvfolderschanged       As Boolean = False
	Public scraperLog                   As String = ""
	Public NewTagList                   As New List(Of String)
	Public MovieSearchEngine            As String = "imdb"
	Dim mov_TableColumnName             As String = ""
	Dim mov_TableRowNum                 As Integer = -1
	Dim MovFanartToggle                 As Boolean = False
	Dim MovPosterToggle                 As Boolean = False
	Private keypresstimer               As Timers.Timer = New Timers.Timer()
	Private statusstripclear            As Timers.Timer = New Timers.Timer()
    Private TvAutoScrapeTimer           As Timers.Timer = New Timers.Timer()
    Private MovAutoScrapeTimer          As Timers.Timer = New Timers.Timer()
    Private TvAutoScrapeTimerTripped    As Boolean = False
    Private MovAutoScrapeTimerTripped   As Boolean = False
	Private MovieKeyPress               As String = ""
	Public cropMode                     As String = "movieposter"
    Public tvseriestoget                As New List(Of TvSeriesData)

	Dim WithEvents bigPictureBox        As PictureBox
	Dim WithEvents fanartBoxes          As PictureBox
	Dim WithEvents fanartCheckBoxes     As RadioButton
	Dim WithEvents posterPicBoxes       As PictureBox
	Dim WithEvents posterCheckBoxes     As RadioButton
	Dim WithEvents posterLabels         As Label
	Dim WithEvents resLabel             As Label
	Dim WithEvents tvFanartBoxes        As PictureBox
	Dim WithEvents tvFanartCheckBoxes   As RadioButton
	Dim WithEvents resolutionLabels     As Label
	Dim newTvFolders                    As New List(Of String)
	Dim tvprogresstxt                   As String = ""
    Dim tooltiptv                       As New Tooltip
	Dim MovpictureList                  As New List(Of PictureBox)
	Dim tvpictureList                   As New List(Of PictureBox)
	Dim screenshotTab                   As TabPage
	Dim filterOverride                  As Boolean = False
	Dim bigPanel                        As Panel
	Public Shared profileStruct         As New Profiles
	Dim frmSplash                       As New frmSplashscreen
	Dim frmSplash2                      As New frmProgressScreen
	Public Shared multimonitor          As Boolean = False
	Dim scrapeAndQuit                   As Boolean = False
	Dim refreshAndQuit                  As Boolean = False
	Dim sandq                           As Integer = 0
	Dim mouseDelta                      As Integer = 0
	Dim resLabels                       As Label
	Dim votelabels                      As Label
	Dim fanartArray                     As New List(Of McImage)
	Dim cropString                      As String
	Dim posterArray                     As New List(Of McImage)
	Dim pageCount                       As Integer = 0
	Dim currentPage                     As Integer = 0
	Dim actorflag                       As Boolean = False
	Dim listOfTvFanarts                 As New List(Of str_FanartList)
	Dim tableSets                       As New List(Of str_TableItems)
	Dim relativeFolderList              As New List(Of str_RelativeFileList)
	Dim WithEvents tvposterpicboxes     As PictureBox
	Dim WithEvents tvpostercheckboxes   As RadioButton
	Dim WithEvents tvposterlabels       As Label
	Dim WithEvents tvreslabel           As Label
	Dim tvposterpage                    As Integer = 1
	Public Const WallPicWidth           As Integer = 165
	Public Const WallPicHeight          As Integer = 220
	Dim MovMaxWallCount                 As Integer = 0
	Dim tvMaxWallCount                  As Integer = 0
	Dim moviecount_bak                  As Integer = 0
    Dim MovieRefresh                    As Boolean = False
	Dim tvCount_bak                     As Integer = 0
	Dim lastSort                        As String = ""
	Dim lastinvert                      As String = ""
	Public displayRuntimeScraper        As Boolean = True
	Dim tv_IMDbID_detected              As Boolean = False
	Dim tv_IMDbID_warned                As Boolean = False
	Dim tv_IMDbID_detectedMsg           As String = String.Format("Media Companion has detected one or more TV Shows has an incorrect ID.{0}", vbCrLf) & _
									String.Format("To rectify, please select the following:{0}", vbCrLf) & _
									String.Format("  1. TV Preferences -> Fix NFO id during cache refresh{0}", vbCrLf) & _
									String.Format("  2. TV Shows -> Refresh Shows{0}", vbCrLf) & _
									String.Format("(This will only be reported once per session)", vbCrLf)
    Public WithEvents oTV               As New TVDBScraper
    Public tvargs                       As TvdbArgs
	Private TVSearchALL                 As Boolean = False
	Private ClickedControl              As String
	Private tvCurrentTabIndex           As Integer = 0
	Private currentTabIndex             As Integer = 0
	Private homeTabIndex                As Integer = 0
	Private CustTvIndex                 As Integer = 0
	Private currenttitle                As String
	Public singleshow                   As Boolean = False
	Public DGVMoviesColName             As String = ""
	Private CloseMC                     As Boolean = False
	Public Imageloading                 As Boolean = False
	Private MoviesFiltersResizeCalled   As Boolean = False
	Private tb_tagtxt_changed           As Boolean = False
    Private MovSetOverviewEdit          As Boolean = False
    Private tb_MovieSetOverviewChanged  As Boolean = False
    Private MovSetListAllSets           As Boolean = False
    Public tvtrial                      As Boolean = True
    
	'TODO: (Form1_Load) Need to refactor
#Region "Form1 Events"
	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ServicePointManager.UseNagleAlgorithm = False
        ServicePointManager.DefaultConnectionLimit = 20
        'Application.CurrentCulture = New Globalization.CultureInfo("de-de")
		PictureBoxAssignedMoviePoster.AllowDrop = True

		Try
			Pref.movie_filters.FilterPanel = MovieFiltersPanel
			Label73.Text = ""

			BckWrkScnMovies.WorkerReportsProgress       = True
			BckWrkScnMovies.WorkerSupportsCancellation  = True
            BckWrkTv.WorkerReportsProgress              = True
            BckWrkTv.WorkerSupportsCancellation         = True
            BckWrkSeries.WorkerReportsProgress          = True
            BckWrkSeries.WorkerSupportsCancellation     = True
			ImgBw.WorkerReportsProgress                 = True
			ImgBw.WorkerSupportsCancellation            = True
			oMovies.Bw                                  = BckWrkScnMovies
            oTV.TvBw                                    = BckWrkTv

			For I = 0 To 20
				Common.Tasks.Add(New Tasks.BlankTask())
			Next

			Pref.applicationPath = Application.StartupPath
			Utilities.applicationPath = Application.StartupPath
			Utilities.EnsureFolderExists(Utilities.PosterCachePath)
			Utilities.EnsureFolderExists(Utilities.MissingPath)
			Utilities.EnsureFolderExists(Utilities.CacheFolderPath)
			If Not Utilities.GetFrameworkVersions().IndexOf("4.0") Then
				Dim RequiredNetURL As String = "http://www.microsoft.com/download/en/details.aspx?id=17718"
				If MsgBox("The Client version is available through Windows Updates." & vbCrLf & _
							 "The Full version, while not required, is available from:" & vbCrLf & _
							 RequiredNetURL & vbCrLf & vbCrLf & _
							 "Do you wish to download the Full version?", _
							 MsgBoxStyle.YesNo, "MC Requires .Net 4.0.") = MsgBoxResult.Yes Then
					OpenUrl(RequiredNetURL)
					End
				End If
			End If

			ForegroundWorkTimer.Interval = 500
#If Not Debug Then
           AddHandler ForegroundWorkTimer.Tick, AddressOf ForegroundWorkPumper
#End If

			Dim asm As Assembly = Assembly.GetExecutingAssembly
			Dim InternalResourceNames() As String = asm.GetManifestResourceNames

			For Each Temp In InternalResourceNames
				Dim Temp1 As ManifestResourceInfo = asm.GetManifestResourceInfo(Temp)
			Next

			TvTreeview.Sort()

			For Each arg As String In Environment.GetCommandLineArgs().Skip(1)
				Select Case arg.ToLower
					Case "sq"
						scrapeAndQuit = True
						sandq = 3
					Case "st"
						scrapeAndQuit = True
						sandq = 1
					Case "sm"
						scrapeAndQuit = True
						sandq = 2
					Case "r"
						refreshAndQuit = True
					Case "?"
						AttachConsole(-1)
						Console.WriteLine("")
						Console.WriteLine("")
						Console.WriteLine("Commandline options")
						Console.WriteLine("-------------------")
						Console.WriteLine("sq - Search for & scrape new movies & tv shows")
						Console.WriteLine("st - Search for & scrape newtv shows")
						Console.WriteLine("sm - Search for & scrape new movies")
						Console.WriteLine("r  - Refresh movie & tv caches")
						Console.WriteLine("?  - Show this page")
						Console.WriteLine("")
						Environment.Exit(1)
						'Me.Close()
					Case Else
						AttachConsole(-1)
						Console.WriteLine("")
						Console.WriteLine("")
						Console.WriteLine("Unrecognised commandline option : [" & arg & "]. Type ? for help")
						Console.WriteLine("")
						Environment.Exit(1)
						'Me.Close()
				End Select
			Next

			If scrapeAndQuit Or refreshAndQuit Then
				Me.WindowState = FormWindowState.Minimized
			Else
				Dim scrn As Integer = splashscreenread()
				If multimonitor Then
					frmSplash.Bounds = screen.AllScreens(scrn).Bounds
					frmSplash.StartPosition = FormStartPosition.Manual
					Dim x As Integer = screen.AllScreens(scrn).Bounds.X
					frmSplash.Location = New Point(x + 250, 250)
					frmSplash.TopMost = True
				End If
				frmSplash.Show()
				frmSplash.Label3.Text = "Status :- Initialising Program"
				frmSplash.Label3.Refresh()
			End If
			Me.Visible = False

			Me.Refresh()
			Application.DoEvents()
			Dim tempstring As String
			tempstring = applicationPath & "\enablemultiple.set"
			If Not File.Exists(tempstring) Then
				Dim tej As Integer = 0
				Dim processes() As Process
				Dim instance As Process
				Dim process As New Process()
				processes = process.GetProcesses
				For Each instance In processes
					If instance.ProcessName = "Media Companion" Then
						tej = tej + 1
						If tej >= 2 Then
							MsgBox("Media Companion is already running")
							End                         'Close MC since another version of the program is running.
						End If
					End If
				Next
			End If
			CheckForIllegalCrossThreadCalls = False

			Pref.maximised = False
			Pref.SetUpPreferences()  'Set defaults to all userpreferences. We then load the preferences from config.xml this way any missing ones have a default already set

			GenreMasterLoad()

			tempstring = applicationPath & "\Settings\" 'read in the config.xml to set the stored preferences (if it exists)
			Dim hg As New DirectoryInfo(tempstring)
			If hg.Exists Then
				Pref.configpath = tempstring & "config.xml"
				If Not File.Exists(Pref.configpath) Then Pref.ConfigSave()
			Else
				Directory.CreateDirectory(tempstring)
				workingProfile.Config = applicationPath & "\Settings\config.xml"
				Pref.ConfigSave()
			End If

			If Not File.Exists(applicationPath & "\Settings\profile.xml") Then
				profileStruct.WorkingProfileName = "Default"
				profileStruct.DefaultProfile = "Default"
				profileStruct.StartupProfile = "Default"
				Dim currentprofile As New ListOfProfiles
				tempstring = applicationPath & "\Settings\"
				currentprofile.ActorCache = tempstring & "actorcache.xml"
				currentprofile.DirectorCache = tempstring & "directorcache.xml"
				currentprofile.Config = tempstring & "config.xml"
				currentprofile.RegExList = tempstring & "regex.xml"
				currentprofile.TvCache = tempstring & "tvcache.xml"
				currentprofile.MusicVideoCache = tempstring & "musicvideocache.xml"
				currentprofile.Genres = tempstring & "genres.txt"
				currentprofile.MovieCache = tempstring & "moviecache.xml"
				currentprofile.MovieSetCache = tempstring & "moviesetcache.xml"
				currentprofile.CustomTvCache = tempstring & "customtvcache.xml"
				currentprofile.ProfileName = "Default"
				profileStruct.ProfileList.Add(currentprofile)
				profileStruct.WorkingProfileName = "Default"
				Call util_ProfileSave()
			End If

			'hide debug xml view tabs - unhiden (i.e. added) via debug tab
			TabLevel1.TabPages.Remove(Me.TabConfigXML)
			TabLevel1.TabPages.Remove(Me.TabMovieCacheXML)
			TabLevel1.TabPages.Remove(Me.TabTVCacheXML)
			TabLevel1.TabPages.Remove(Me.TabProfile)
			TabLevel1.TabPages.Remove(Me.TabActorCache)
			TabLevel1.TabPages.Remove(Me.TabRegex)
			TabLevel1.TabPages.Remove(Me.TabCustTv)     'Hide customtv tab while Work-In-Progress

			Call util_ProfilesLoad()
			For Each prof In profileStruct.ProfileList
				If prof.ProfileName = profileStruct.StartupProfile Then
					workingProfile.ActorCache = prof.ActorCache
					workingProfile.DirectorCache = prof.DirectorCache
					workingProfile.Config = prof.Config
					workingProfile.MovieCache = prof.MovieCache
					workingProfile.ProfileName = prof.ProfileName
					workingProfile.RegExList = prof.RegExList
					workingProfile.Genres = prof.Genres
					workingProfile.TvCache = prof.TvCache
					workingProfile.CustomTvCache = prof.CustomTvCache
					workingProfile.ProfileName = prof.ProfileName
					workingProfile.MusicVideoCache = prof.MusicVideoCache
					workingProfile.MovieSetCache = prof.MovieSetCache
					For Each item In ProfilesToolStripMenuItem.DropDownItems
						If item.text = workingProfile.ProfileName Then
							With item
								item.checked = True
							End With
						Else
							item.checked = False
						End If
					Next
				End If
			Next

			'add musicvideocache to profile if it doesnt exist
			Dim counter As Int16 = 0
			tempstring = applicationPath & "\Settings\"
			For Each prof In profileStruct.ProfileList
				If counter = 0 Then
					If prof.MusicVideoCache = "" Then
						prof.MusicVideoCache = tempstring & "musicvideocache.xml"
						If prof.ProfileName = workingProfile.ProfileName Then
							workingProfile.MusicVideoCache = tempstring & "musicvideocache.xml"
						End If
					End If
					If prof.MovieSetCache = "" Then
						prof.MovieSetCache = tempstring & "moviesetcache.xml"
						If prof.ProfileName = workingProfile.ProfileName Then
							workingProfile.MovieSetCache = tempstring & "moviesetcache.xml"
						End If
					End If
					If prof.CustomTvCache = "" Then
						prof.CustomTvCache = tempstring & "customtvcache.xml"
						If prof.ProfileName = workingProfile.ProfileName Then
							workingProfile.CustomTvCache = tempstring & "customtvcache.xml"
						End If
					End If
				Else
					If prof.MusicVideoCache = "" Then
						prof.MusicVideoCache = tempstring & "musicvideocache" & counter.ToString & ".xml"
					End If
					If prof.ProfileName = workingProfile.ProfileName Then
						workingProfile.MusicVideoCache = tempstring & "musicvideocache" & counter.ToString & ".xml"
					End If
					If prof.MovieSetCache = "" Then
						prof.MovieSetCache = tempstring & "moviesetcache" & counter.ToString & ".xml"
					End If
					If prof.ProfileName = workingProfile.ProfileName Then
						workingProfile.MovieSetCache = tempstring & "moviesetcache" & counter.ToString & ".xml"
					End If
					If prof.CustomTvCache = "" Then
						prof.CustomTvCache = tempstring & "customtvcache" & counter.ToString & ".xml"
					End If
					If prof.ProfileName = workingProfile.ProfileName Then
						workingProfile.CustomTvCache = tempstring & "customtvcache" & counter.ToString & ".xml"
					End If
				End If
				counter += 1
			Next

			If workingProfile.HomeMovieCache = "" Then workingProfile.HomeMovieCache = tempstring & "homemoviecache.xml"
			'Update Main Form Window Title to show Currrent Version - displays current profile so has to be done after profile is loaded
			util_MainFormTitleUpdate()


			Dim g As New DirectoryInfo(Utilities.PosterCachePath)
			If Not g.Exists Then
				Try
					Directory.CreateDirectory(Utilities.PosterCachePath)
				Catch ex As Exception
					MsgBox(ex.Message.ToString)
					End
				End Try
			End If

			CheckForIllegalCrossThreadCalls = False

			Try
				If File.Exists(Path.Combine(applicationPath, "\error.log")) Then File.Delete(Path.Combine(applicationPath, "\error.log"))
			Catch ex As Exception
#If SilentErrorScream Then
                Throw ex
#End If
			End Try

			tempstring = applicationDatapath & "error.log"
			If File.Exists(tempstring) = True Then File.Delete(tempstring)

			IniMovieFilters

			Call util_RegexLoad()

			Call util_PrefsLoad()

			Statusstrip_Enable(False)

			'These lines fixed the associated panel so that they don't automove when the Form1 is resized
			SpltCntr_MovDisplay.FixedPanel = System.Windows.Forms.FixedPanel.Panel1 'Left Panel on Movie tab - Movie Listing 
			SpltCntr_MovBrowserList.FixedPanel = System.Windows.Forms.FixedPanel.Panel2 'Bottom Left Panel on Movie Tab - Filters
			SpltCntr_TvDisplay.FixedPanel = System.Windows.Forms.FixedPanel.Panel1 'Left Panel on TV Tab

			Movies.SpinUpDrives()

			If Not (scrapeAndQuit Or refreshAndQuit) Then
				Me.Visible = True
				Dim intX As Integer
				Dim intY As Integer
				If Pref.MultiMonitoEnabled Then
					Dim scrn As Integer = If(NumOfScreens > 0, Pref.preferredscreen, 0)
					intX = screen.AllScreens(scrn).Bounds.X ' + screen.AllScreens(scrn).Bounds.Width
					intY = screen.AllScreens(scrn).Bounds.Height
				Else
					intX = Screen.PrimaryScreen.Bounds.Width
					intY = Screen.PrimaryScreen.Bounds.Height
				End If
				SpltCntr_MovDisplay.IsSplitterFixed = True
				SpltCntr_MovPictures.IsSplitterFixed = True
				SpltCntr_TvDisplay.IsSplitterFixed = True
				SpltCntr_TvPictures.IsSplitterFixed = True
				SpltCntr_MovBrowserList.IsSplitterFixed = True

				If intX >= 0 Then  'First check if screen is Centre or to the right of main screen.
					If Pref.locx < 0 Then Pref.locx = 0
					If Pref.locy < 0 Then Pref.locy = 0
					If Pref.formheight > intY Then Pref.formheight = intY
					If Pref.formwidth > intX Then Pref.formwidth = intX
					If Pref.locx >= intX Then Pref.locx = intX - Pref.formwidth
					If Pref.locy >= intY Then Pref.locy = intY - Pref.formheight
				Else    'Else screen could be to the left of Centre/Main screen so intX is a negative number.
					'If Pref.locx < 0 Then Pref.locx = 0
					If Pref.locy < 0 Then Pref.locy = 0
					If Pref.formheight > intY Then Pref.formheight = intY
					If Pref.formwidth > Math.Abs(intX) Then Pref.formwidth = intX
					If Pref.locx <= intX Then Pref.locx = intX ' - Pref.formwidth
					If Pref.locy >= intY Then Pref.locy = intY - Pref.formheight
				End If
				If Pref.formheight <> 0 And Pref.formwidth <> 0 Then
					Me.Width = Pref.formwidth
					Me.Height = Pref.formheight
					Me.Location = New Point(Pref.locx, Pref.locy)
				End If
				If Pref.maximised Then Me.WindowState = FormWindowState.Maximized

				Dim dpi As Graphics = Me.CreateGraphics
                
				DebugSytemDPITextBox.Text = dpi.DpiX

				Me.Refresh()
				Application.DoEvents()

				Me.Refresh()
				Application.DoEvents()

				Application.DoEvents()

				screenshotTab = TabControl3.TabPages(1)

				TabControl3.TabPages.RemoveAt(1)

				If Pref.splt5 = 0 Then
					Dim tempint As Integer = SpltCntr_MovDisplay.Height
					tempint = tempint / 4
					tempint = tempint * 3
					If tempint > 275 Then
						Pref.splt5 = tempint
					Else
						Pref.splt5 = 275
					End If
				End If

				SpltCntr_MovDisplay.SplitterDistance = Pref.splt1
				SpltCntr_MovPictures.SplitterDistance = Pref.splt2
				SpltCntr_MovBrowserList.SplitterDistance = Pref.splt5
				SpltCntr_TvDisplay.SplitterDistance = Pref.splt3
				SpltCntr_TvPictures.SplitterDistance = Pref.splt4
				TabLevel1.SelectedIndex = Pref.startuptab

				If Pref.startuptab = 0 Then
					If Not MoviesFiltersResizeCalled Then
						MoviesFiltersResizeCalled = True
						Pref.movie_filters.RemoveInvalidMovieFilters
						Pref.movie_filters.SetMovieFiltersVisibility
						UpdateMovieFiltersPanel
					End If
				End If
				btn_MPDB_posters.Enabled = False        'Disable MoviePoster button on Movie Poster Tab as now not available to us.

				SpltCntr_MovDisplay.IsSplitterFixed = False
				SpltCntr_MovPictures.IsSplitterFixed = False
				SpltCntr_TvDisplay.IsSplitterFixed = False
				SpltCntr_TvPictures.IsSplitterFixed = False
				SpltCntr_MovBrowserList.IsSplitterFixed = False
			End If

			If scrapeAndQuit Or refreshAndQuit Then
				Do_ScrapeAndQuit()
				Me.Close()
			Else
				Try
					If cbMovieDisplay_MovieSet.Items.Count <> Pref.moviesets.Count Then
						cbMovieDisplay_MovieSet.Items.Clear()
						For Each mset In Pref.moviesets
							cbMovieDisplay_MovieSet.Items.Add(If(Pref.MovSetTitleIgnArticle, Pref.RemoveIgnoredArticles(mset), mset))
						Next
					End If
					If Not IsNothing(workingMovieDetails) AndAlso workingMovieDetails.fullmoviebody.SetName <> "-None-" Then
						For Each mset In Pref.moviesets
							cbMovieDisplay_MovieSet.Items.Add(If(Pref.MovSetTitleIgnArticle, Pref.RemoveIgnoredArticles(mset), mset))
						Next
						For te = 0 To cbMovieDisplay_MovieSet.Items.Count - 1
							If cbMovieDisplay_MovieSet.Items(te) = workingMovieDetails.fullmoviebody.SetName Then
								cbMovieDisplay_MovieSet.SelectedIndex = te
								Exit For
							End If
						Next
					End If

				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
				End Try
				mov_VideoSourcePopulate()
				ep_VideoSourcePopulate()
				Call util_FontSetup()
				Call langarrsetup()
				Dim mediaDropdown As New SortedList(Of String, String)
				mediaInfoExp.addTemplates(mediaDropdown)
				For Each item In mediaDropdown
					If item.Value = MediaInfoExport.mediaType.Movie Then
						ExportMovieListInfoToolStripMenuItem.DropDownItems.Add(item.Key)
					ElseIf item.Value = MediaInfoExport.mediaType.TV Then
						ExportTVShowInfoToolStripMenuItem.DropDownItems.Add(item.Key)
					End If
				Next

				Call util_CommandListLoad()
				startup = False

				frmSplash.Label3.Text = "Status :- Cleaning Cache folder."
				frmSplash.Label3.Refresh()

				frmSplash.Close()

				mov_SplitContainerAutoPosition()
				tv_ShowSelectedCurrently(TvTreeview)
				tv_SplitContainerAutoPosition()
			End If

			'Parameters to display the movie grid at startup
			Select Case Pref.moviedefaultlist
				Case 0
					rbTitleAndYear.Checked = True
				Case 1
					rbFileName.Checked = True
				Case 2
					rbFolder.Checked = True
			End Select

			Try
				cbSort.SelectedIndex = Pref.moviesortorder
			Catch
				cbSort.SelectedIndex = 0
			End Try
            cmbxHMSort.SelectedIndex = 0
			btnreverse.Checked = Pref.movieinvertorder
			If btnreverse.Checked Then
				Mc.clsGridViewMovie.GridSort = "Desc"
			Else
				Mc.clsGridViewMovie.GridSort = "Asc"
			End If

			Read_XBMC_TMDB_Scraper_Config()
			Read_XBMC_TVDB_Scraper_Config()
			MainFormLoadedStatus = True
			UcFanartTv1.Form1MainFormLoadedStatus = True
			UcFanartTvTv1.Form1MainFormLoadedStatus = True
			ReloadMovieCacheToolStripMenuItem.Visible = False
			ToolStripSeparator9.Visible = False
            HMDGV_FiltersAndSortApply()

			ResetFilters()

			UpdateFilteredList()

			If Not IsNothing(Pref.MovFiltLastSize) Then ResizeBottomLHSPanel(Pref.MovFiltLastSize, MovieFiltersPanelMaxHeight)

			Common.Tasks.StartTaskEngine()
			ForegroundWorkTimer.Start()
            
			BckWrkXbmcController.WorkerReportsProgress = True
			' BckWrkXbmcController.WorkerSupportsCancellation = true

			oMovies.Bw = BckWrkScnMovies

			AddHandler XBMC_Link_ErrorLog_Timer.Elapsed, AddressOf XBMC_Controller_Log_TO_Timer_Elapsed
			Ini_Timer(XBMC_Link_ErrorLog_Timer, 3000)

			AddHandler XBMC_Link_Idle_Timer.Elapsed, AddressOf XBMC_Link_Idle_Timer_Elapsed
			Ini_Timer(XBMC_Link_Idle_Timer, 3000)

			AddHandler XBMC_Link_Check_Timer.Elapsed, AddressOf XBMC_Link_Check_Timer_Elapsed
			Ini_Timer(XBMC_Link_Check_Timer, 2000, True)
			'XBMC_Link_Check_Timer.Start

			AddHandler keypresstimer.Elapsed, AddressOf keypresstimer_Elapsed
			Ini_Timer(keypresstimer, 1000)

			AddHandler statusstripclear.Elapsed, AddressOf statusstripclear_Elapsed
			Ini_Timer(statusstripclear, 2000)

            AddHandler TvAutoScrapeTimer.Elapsed, AddressOf TvAutoScrapeTimer_Elapsed
            Ini_Timer(TvAutoScrapeTimer, ((Pref.TvAutoScrapeInterval * 60) * 1000), True)

            AddHandler MovAutoScrapeTimer.Elapsed, AddressOf MovAutoScrapeTimer_Elapsed
            Ini_Timer(MovAutoScrapeTimer, ((Pref.MovAutoScrapeInterval * 60) * 1000), True)

            AddHandler HMkeypresstimer.Elapsed, AddressOf HMkeypresstimer_Elapsed
            Ini_Timer(HMkeypresstimer, 1000)

			AddHandler BckWrkXbmcController.ProgressChanged, AddressOf BckWrkXbmcController_ReportProgress
			AddHandler BckWrkXbmcController.DoWork, AddressOf BckWrkXbmcController_DoWork

			'BckWrkXbmcController.RunWorkerAsync(Me)

            If Pref.TvEnableAutoScrape AndAlso Not TvAutoScrapeTimer.Enabled Then
                TvAutoScrapeTimer.Start()
            End If

            If Pref.MovEnableAutoScrape AndAlso Not MovAutoScrapeTimer.Enabled Then
                MovAutoScrapeTimer.Start()
            End If

			For each pb As Control In TableLayoutPanel6.Controls
				If pb.Name.Contains("pbEpScrSht") Then
					AddHandler pb.Click, AddressOf pbepscrsht_click
				End If
			Next

			For each pb As Control In TableLayoutPanel27.Controls
				If pb.Name.Contains("pbHmScrSht") Then
					AddHandler pb.Click, AddressOf pbHmScrSht_click
				End If
			Next

		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

#If Refocus Then
    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Try
            If messbox.Visible = True Then
                messbox.Activate()
                messbox.BringToFront()
                messbox.Focus()
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub
#End If

	Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		Try
			Me.Dispose()
			Me.Finalize()
			CleanCacheFolder(, cbClearCache.Checked)  'Limit cachefolder to max 300 files.  Cleaned on startup and shutdown.
			'If cbClearCache checked then completely empty
			If cbClearMissingFolder.Checked = True Then ClearMissingFolder() ' delete missing folder if option selected.
			End
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        ' Set ProgState to Closing so we can terminate some routines that do not need to run on exiting of Media Companion
        ProgState = ProgramState.Closing

		BckWrkScnMovies_Cancel()
		While BckWrkScnMovies.IsBusy
			Application.DoEvents()
		End While
        
		Try
			oMovies.SaveCaches()

			Call Tv_CacheSave()

			Call HomeMovieCacheSave()

			Call UcMusicVideo1.MVCacheSave()
            
			'Todo: Code a better way to serialize the data

			Pref.splt1 = SpltCntr_MovDisplay.SplitterDistance
			Pref.splt2 = SpltCntr_MovPictures.SplitterDistance
			Pref.splt3 = SpltCntr_TvDisplay.SplitterDistance
			Pref.splt4 = SpltCntr_TvPictures.SplitterDistance
			Pref.splt5 = SpltCntr_MovBrowserList.SplitterDistance
			Pref.splt6 = _tv_SplitContainer.SplitterDistance
			Pref.tvbannersplit = Math.Round(_tv_SplitContainer.SplitterDistance / _tv_SplitContainer.Height, 2)
			Pref.MovFiltLastSize = SpltCntr_MovBrowserList.Height - SpltCntr_MovBrowserList.SplitterDistance
			Pref.preferredscreen = CurrentScreen


			If Me.WindowState = FormWindowState.Minimized Then
				Me.WindowState = FormWindowState.Normal
				Pref.formwidth = Me.Width
				Pref.formheight = Me.Height
				Pref.locx = Me.Location.X
				Pref.locy = Me.Location.Y
				Pref.maximised = False
			End If

			If Me.WindowState = FormWindowState.Normal Then
				Pref.formwidth = Me.Width
				Pref.formheight = Me.Height
				Pref.locx = Me.Location.X
				Pref.locy = Me.Location.Y
				Pref.maximised = False
			End If

			If Me.WindowState = FormWindowState.Maximized Then
				Me.WindowState = FormWindowState.Normal
				Pref.maximised = True
			End If

			If dgvMoviesTableView.Columns.Count > 0 Then
				Pref.tableview.Clear()
				For Each column In dgvMoviesTableView.Columns
					Dim tempstring As String = String.Format("{0}|{1}|{2}|{3}", column.name, column.width, column.displayindex, column.visible)
					Pref.tableview.Add(tempstring)
				Next
			End If

			Pref.startuptab = TabLevel1.SelectedIndex

			Pref.ConfigSave()
			SplashscreenWrite()
			Call util_ProfileSave()
		Catch ex As Exception
			MessageBox.Show(ex.ToString, "Exception")
			Environment.Exit(1)
		End Try
	End Sub

	Private Sub Form1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
		Try
			If messbox.Visible = True Then
				messbox.Activate()
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If ProgState = ProgramState.Closing Then Exit Sub
		Try
			If Me.WindowState = FormWindowState.Maximized Then
				mov_SplitContainerAutoPosition()
				tv_SplitContainerAutoPosition()
			End If
			If Not Me.WindowState = FormWindowState.Minimized Then
				tv_SplitContainerAutoPosition()
			End If
			If startup = False Then
				Pref.locx = Me.Location.X
				Pref.locy = Me.Location.Y
			End If
			If MainFormLoadedStatus = True Then
				doResizeRefresh()
			End If
		Catch ex As Exception
			Dim paramInfo As String = ""
			Try
				paramInfo = "PbMovieFanArt.Width:" & PbMovieFanArt.Width.ToString & " PbMovieFanArt.Height: " & PbMovieFanArt.Height.ToString & " Rating:" & ratingtxt.Text
			Catch ex2 As Exception
				ExceptionHandler.LogError(ex2)
			End Try
			ExceptionHandler.LogError(ex, paramInfo)
		End Try
	End Sub

	Private Sub Form1_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.ResizeEnd
		Try
			If Pref.formwidth <> Me.Width Or Pref.formheight <> Me.Height Then
				Pref.formwidth = Me.Width
				Pref.formheight = Me.Height
			End If
			mov_SplitContainerAutoPosition()
			tv_SplitContainerAutoPosition()

		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub Form1_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
		If e.KeyCode = Keys.Escape Then bckgrndcancel()
		If e.KeyCode = Keys.F5 Then doRefresh()
		If e.KeyCode = Keys.F3 Then doSearchNew()
        If e.KeyCode = Keys.F7 AndAlso TabLevel1.SelectedTab.Text = "TV Shows" Then CheckRootsForToolStripMenuItem.PerformClick()
        If e.KeyCode = Keys.F9 Then doTestTvdb
        If e.KeyCode = Keys.F2 Then
            If TabLevel1.SelectedTab.Name = TabPage2.Name AndAlso TabControl3.SelectedTab.Name = tpTvMainBrowser.Name Then
                'tvAddNewSeries()
            End If
        End If
		If e.Control And e.KeyCode = Keys.C Then AbortFileDownload()
	End Sub

	Private Sub Form1_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
		If e.KeyCode = Keys.F6 AndAlso TabLevel1.SelectedTab.Name = TabPage1.Name Then
            mov_CacheLoad
            DataGridViewMovies.ClearSelection
            If DataGridViewMovies.Rows.Count>0 Then
                DataGridViewMovies.Rows(0).Selected=True
            End If
            DisplayMovie
        End If
	End Sub

#End Region

	Sub Ini_Timer(t As Timers.Timer, Optional Interval As Integer = 1000, Optional Repeating As Boolean = False)
		t.Stop()
		t.Interval = Interval
		t.AutoReset = Repeating
	End Sub

    Sub Restart(tmr As Timers.Timer)
		tmr.Stop()
		tmr.Start()
	End Sub

#Region "XBMC Link"
	Private Sub BckWrkXbmcController_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
		Dim bw As BackgroundWorker = CType(sender, BackgroundWorker)
		Dim sm As New XbmcController(e.Argument, bw)
		sm.Go()
	End Sub

	Private Sub XBMC_Controller_Log_TO_Timer_Elapsed()
		If Not BckWrkScnMovies.IsBusy And XbmcControllerBufferQ.Count = 0 Then
			If DateDiff(DateInterval.Second, XBMC_Controller_LogLastShownDt, Now) > 30 Then

				Pref.OpenFileInAppPath(Form1.XBMC_Controller_full_log_file)
				Pref.OpenFileInAppPath(Form1.XBMC_Controller_brief_log_file)

				frmXBMC_Progress.Reset()
				Dim ce As New BaseEvent(XbmcController.E.ResetErrorCount, New BaseEventArgs())
				XbmcControllerQ.Write(ce)
			End If
			XBMC_Controller_LogLastShownDt = Now
			XBMC_Link_ErrorLog_Timer.Stop()
		End If
	End Sub

	Private Sub XBMC_Link_Idle_Timer_Elapsed()
		If Not BckWrkScnMovies.IsBusy And XbmcControllerBufferQ.Count = 0 Then
			frmXBMC_Progress.Visible = False
			XBMC_Link_ErrorLog_Timer.Stop()
		End If
	End Sub
    
	Private Sub BckWrkXbmcController_ReportProgress(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
		Dim oProgress As XBMC_Controller_Progress = CType(e.UserState, XBMC_Controller_Progress)

		If XBMC_Link_ErrorLog_Timer.Enabled Then
			Restart(XBMC_Link_ErrorLog_Timer)
		End If

		Restart(XBMC_Link_Idle_Timer)
		Restart(XBMC_Link_Check_Timer)

		frmXBMC_Progress.Visible = True
		If HandleEvents(oProgress) Then Return
		If oProgress.ErrorCount > 0 Then
			If Pref.ShowLogOnError Then
				XBMC_Link_ErrorLog_Timer.Start()
			End If
		End If

		frmXBMC_Progress.UpdateDetails(oProgress)
	End Sub

	Function HandleEvents(oProgress As XBMC_Controller_Progress) As Boolean

		Select Case oProgress.Evt

			Case XbmcController.E.MC_Only_Movies
				MC_Only_Movies = CType(oProgress.Args, ComboList_EventArgs).XbmcMovies
				Assign_FilterGeneral()
				Return True

			Case XbmcController.E.MC_XbmcMcMovies
				oMovies.XbmcMcMovies = CType(oProgress.Args, XBMC_MC_Movies_EventArgs).XbmcMcMovies
				Assign_FilterGeneral()
				Return True

			Case XbmcController.E.MC_XbmcOnlyMovies
				oMovies.XbmcOnlyMovies = CType(oProgress.Args, XBMC_Only_Movies_EventArgs).XbmcOnlyMovies
				Return True

			Case XbmcController.E.MC_XbmcQuit
				SetcbBtnLink()
				Return True

		End Select
		Return False
	End Function

	Const MaxConseqFailures As Integer = 3

	Dim ConnectSent As Boolean
	Dim XbmcLastLinkState As Boolean
	Dim ConseqFailures As Integer = 0

	Sub SetcbBtnLink(Optional sender As Object = Nothing)
		XBMC_Link_Check_Timer.Stop()

		'Only check when link is idle
		If Not BckWrkScnMovies.IsBusy And XbmcControllerBufferQ.Count = 0 Then
			Dim passed As Boolean = XBMC_TestsPassed

			'
			' Sometimes the link test fails. Don't know why yet, maybe XBMC is busy...but anyway this should reduce the number of phantom 
			' link disablings...
			' 
			If Not IsNothing(sender) Then
				If cbBtnLink.Enabled And Not passed Then
					ConseqFailures += 1
				Else
					ConseqFailures = 0
				End If
			Else
				ConseqFailures = 0
			End If

			If IsNothing(sender) Or ConseqFailures <= MaxConseqFailures Then
				cbBtnLink.Enabled = passed
				If cbBtnLink.Enabled Then
					cbBtnLink.BackColor = IIf(cbBtnLink.Checked, Color.LightGreen, Color.Transparent)
					If Pref.XBMC_Link <> cbBtnLink.Checked Then
						Pref.XBMC_Link = cbBtnLink.Checked
						If Pref.XbmcLinkReady Then
							XbmcControllerQ.Write(XbmcController.E.ConnectReq, PriorityQueue.Priorities.low)
						End If
						Pref.ConfigSave()
					End If
				Else
					cbBtnLink.Checked = False
					cbBtnLink.BackColor = Color.Transparent
				End If
				tsmiMov_SyncToXBMC.Enabled = cbBtnLink.Enabled And cbBtnLink.Checked
			End If
		End If
		XBMC_Link_Check_Timer.Start()
	End Sub

	Private Sub XBMC_Link_Check_Timer_Elapsed()
		If ProgState = ProgramState.MovieControlsDisabled Then Return
		SetcbBtnLink(XBMC_Link_Check_Timer)
	End Sub

	Private Sub keypresstimer_Elapsed()
		MovieKeyPress = ""
	End Sub

    Private Sub TvAutoScrapeTimer_Elapsed()
        Do Until TvAutoScrapeTimerTripped
            If Not tvbckrescrapewizard.IsBusy AndAlso Not bckgroundscanepisodes.IsBusy AndAlso Not Bckgrndfindmissingepisodes.IsBusy AndAlso Not BckWrkTv.IsBusy Then
                TvAutoScrapeTimerTripped = True
			    ep_SearchInvoke()
		    End If
            Application.DoEvents()
        Loop
    End Sub

    Private Sub MovAutoScrapeTimer_Elapsed()
        Do Until MovAutoScrapeTimerTripped
            If Not BckWrkScnMovies.IsBusy Then
                MovAutoScrapeTimerTripped = True
                SearchForNew()
            End If
            Application.DoEvents()
        Loop
    End Sub

	Private Sub statusstripclear_Elapsed()
		ToolStripStatusLabel2.Visible = False
		Statusstrip_Enable(False)
		ToolStripStatusLabel2.Text = "TV Show Episode Scan In Progress"
	End Sub

	Sub XbmcLink_UpdateArtwork()
		If Pref.XBMC_Delete_Cached_Images AndAlso Pref.XbmcLinkReady Then
			Dim m As Movie = oMovies.LoadMovie(workingMovieDetails.fileinfo.fullpathandfilename)
			m.SaveNFO()
		End If
	End Sub
#End Region

	Private Sub BlinkTaskBar()
		If GetActiveWindow <> Me.Handle Then
			Dim res = WindowsApi.FlashWindow(Process.GetCurrentProcess().MainWindowHandle, True, True, 5)
		End If
	End Sub

	Private Function splashscreenread() As Integer
		Dim scrn As Integer = 0
		Dim checkpath As String = Pref.applicationPath & "\Settings\screen.xml"
		If File.Exists(checkpath) Then
			Try
				Dim document As XDocument = XDocument.Load(checkpath)
				Dim sc = From t In document.Descendants("screen") Select t.Value
				Dim mten = From t In document.Descendants("MultiEnabled") Select t.Value
				multimonitor = Convert.ToBoolean(mten.First())
				scrn = sc.First().ToInt
				If scrn > NumOfScreens Then scrn = 0
			Catch
				scrn = 0
				multimonitor = False
			End Try
		End If
		Return scrn
	End Function

	Private Sub SplashscreenWrite()
		Dim doc As New XmlDocument
		Dim thispref As XmlNode = Nothing
		Dim xmlproc As XmlDeclaration
		Dim root As XmlElement = Nothing
		Dim child As XmlElement = Nothing
		xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
		doc.AppendChild(xmlproc)
		root = doc.CreateElement("root")
		root.AppendChild(doc   , "MultiEnabled"    , Pref.MultiMonitoEnabled)
        root.AppendChild(doc   , "screen"          , CurrentScreen.ToString)
		doc.AppendChild(root)
		WorkingWithNfoFiles.SaveXMLDoc(doc, Pref.applicationPath & "\Settings\screen.xml")
	End Sub

	Private Sub Batch_Rewritenfo()

		rescrapeList.ResetFields()
		rescrapeList.rebuildnfo = True

		_rescrapeList.FullPathAndFilenames.Clear()
		For Each movie As ComboList In oMovies.MovieCache
			_rescrapeList.FullPathAndFilenames.Add(movie.fullpathandfilename)
		Next
		RunBackgroundMovieScrape("BatchRescrape")

	End Sub

	Sub CleanCacheFolder(Optional ByVal All As Boolean = False, Optional ByVal Total As Boolean = False)
		Dim cachefolder As String = applicationPath & "\cache\"
		If Directory.Exists(cacheFolder) Then
			Dim Files As New DirectoryInfo(cachefolder)
			Dim FileList() = Files.GetFiles().OrderByDescending(Function(f) f.LastWriteTime).ToArray
			Dim limit As Integer = If(Total, 0, 299)
			Dim i As Integer = FileList.Count
			Try
				If i > limit Then
					Do Until i = limit
						i -= 1
						Dim filepath As String = FileList(i).FullName
						Utilities.SafeDeleteFile(filepath)
					Loop
				End If
			Catch
			End Try
		End If
		If All Then
			ClearSeriesFolder()
			ClearPosterFolder()
		End If
	End Sub

	Sub ClearSeriesFolder()
		If Directory.Exists(Utilities.SeriesXmlPath) Then
			Dim Files As New DirectoryInfo(Utilities.SeriesXmlPath)
			Dim Filelist() = Files.GetFiles()
			For Each f In Filelist
				Utilities.SafeDeleteFile(f.FullName)
			Next
		End If
	End Sub

	Sub ClearMissingFolder()
		Try
			If Directory.Exists(Utilities.MissingPath)
				Dim Files As New DirectoryInfo(Utilities.MissingPath)
				Dim Filelist() = Files.GetFiles()
				For Each f In Filelist
					Utilities.SafeDeleteFile(f.FullName)
				Next
			End If
		Catch ex As Exception
		End Try
	End Sub

	Sub ClearPosterFolder()
		If Directory.Exists(Utilities.PosterCachePath) Then
			Dim Files As New DirectoryInfo(Utilities.PosterCachePath)
			Dim Filelist() = Files.GetFiles()
			For Each f In Filelist
				Utilities.SafeDeleteFile(f.FullName)
			Next
		End If
	End Sub

	Sub util_MainFormTitleUpdate()
		'Update Main Form Window Title to show Currrent Version
		Dim sAssemblyVersion As String = Trim(System.Reflection.Assembly.GetExecutingAssembly.FullName.Split(",")(1))
		sAssemblyVersion = Microsoft.VisualBasic.Right(sAssemblyVersion, 7)       'Cuts Version=3.4.0.2 down to just 3.4.0.2
		Dim codebase As String = If(Environment.Is64BitProcess, "64Bit", "32Bit")
		If workingProfile.profilename.ToLower = "default" Then
			Me.Text = "Media Companion - V" & sAssemblyVersion & " - " & codebase
		Else
			Me.Text = "Media Companion - V" & sAssemblyVersion & " - " & codebase & " - " & workingProfile.profilename
		End If

	End Sub

	Sub mov_SplitContainerAutoPosition()
		'Set Movie Splitter Auto Position
		Dim pic1ratio As Decimal
		Dim pic2ratio As Decimal
		Try
			If Not IsNothing(PbMovieFanArt.Image) Then
				Dim pic1ImSzW = PbMovieFanArt.Image.Size.Width        'original picture sizes
				Dim pic1ImszH = PbMovieFanArt.Image.Size.Height
				Dim pic2ImSzW = PbMoviePoster.Image.Size.Width
				Dim pic2ImszH = PbMoviePoster.Image.Size.Height
				pic1ratio = pic1ImSzW / pic1ImszH
				pic2ratio = pic2ImSzW / pic2ImszH
				Dim width As Integer = SpltCntr_MovPictures.Size.Width
				' MsgBox(from & " = " & width & ":" & Int(SplitContainer2.Size.Width * (pic1ratio / (pic1ratio + pic2ratio))) - 5 & " - " & pic1ImSzW & "x" & pic1ImszH & " " & pic2ImszH & "x" & pic2ImSzW)
			Else
				pic1ratio = 2
				pic2ratio = 1
			End If
		Catch ex As Exception
			pic1ratio = 2
			pic2ratio = 1
			'MsgBox("Movie Splitter Exception")
		End Try
		SpltCntr_MovPictures.SplitterDistance = (SpltCntr_MovPictures.Size.Width - 8) * (pic1ratio / (pic1ratio + pic2ratio))
	End Sub

	Sub tv_SplitContainerAutoPosition()
		'Set TVShow Splitter Auto Position
		Dim pic3ratio As Decimal
		Dim pic4ratio As Decimal
		Dim HorizontalSplit As Decimal
		Try
			If (tv_PictureBoxLeft.Image IsNot Nothing AndAlso tv_PictureBoxRight.Image IsNot Nothing) Then
				Dim pic3ImSzW = tv_PictureBoxLeft.Image.Size.Width
				Dim pic3ImszH = tv_PictureBoxLeft.Image.Size.Height
				Dim pic4ImSzW = tv_PictureBoxRight.Image.Size.Width
				Dim pic4ImszH = tv_PictureBoxRight.Image.Size.Height
				pic3ratio = pic3ImSzW / pic3ImszH
				pic4ratio = pic4ImSzW / pic4ImszH
				HorizontalSplit = ((SpltCntr_TvPictures.Size.Width - 8) * (pic3ratio / (pic3ratio + pic4ratio)) / pic3ratio)
			Else
				pic3ratio = 2
				pic4ratio = 1
				HorizontalSplit = 235
			End If
		Catch ex As Exception
			pic3ratio = 2
			pic4ratio = 1
			HorizontalSplit = 235
		End Try
		SpltCntr_TvPictures.SplitterDistance = (SpltCntr_TvPictures.Size.Width - 8) * (pic3ratio / (pic3ratio + pic4ratio))
		Try     'Try Catch for minimize of MC when Full-screen
			If _tv_SplitContainer.Height > 100 Then
				If Pref.tvbannersplit = 0 Then
					_tv_SplitContainer.SplitterDistance = HorizontalSplit
				Else
					_tv_SplitContainer.SplitterDistance = _tv_SplitContainer.Height * Pref.tvbannersplit
				End If
			End If
		Catch
		End Try
	End Sub

	Public Sub mov_CacheLoad()
		mov_PreferencesDisplay
		oMovies.LoadCaches
		If oMovies.MovieCache.Count = 0 Then
			mov_RebuildMovieCaches
			Return
		End If
		filteredList.Clear
		filteredList.AddRange(oMovies.MovieCache)
		Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		If DataGridViewMovies.Rows.Count > 0 then
			DataGridViewMovies.Rows(0).Selected = True
		End If
		mov_FormPopulate
	End Sub

	Public Sub util_RegexLoad()
		Dim tempstring As String
		tempstring = workingProfile.regexlist
		Pref.tv_RegexScraper.Clear()
		Pref.tv_RegexRename.Clear()
		Dim path As String = tempstring
		Dim createDefaultRegexScrape As Boolean = True
		Dim createDefaultRegexRename As Boolean = True

		If File.Exists(path) Then
			Try
				Dim regexList As New XmlDocument
				regexList.Load(path)
				If regexList.DocumentElement.Name = "regexlist" Then
					For Each result As XmlElement In regexList("regexlist")
						Select Case result.Name
							Case "tvregex"                              'This is the old tag before custom renamer was introduced,
								Pref.tv_RegexScraper.Add(result.InnerText)   'so add it to the scraper regex list in case there are custom regexs.
								createDefaultRegexScrape = False        'The rename regex will not be flagged so regex.xml will be created as new format.
							Case "tvregexscrape"
								Pref.tv_RegexScraper.Add(result.InnerText)
								createDefaultRegexScrape = False
							Case "tvregexrename"
								Pref.tv_RegexRename.Add(result.InnerText)
								createDefaultRegexRename = False
						End Select
					Next
				End If
			Catch ex As Exception
				Call util_RegexSave(True, True)
#If SilentErrorScream Then
                Throw ex
#End If
			End Try
		End If
		If createDefaultRegexScrape Or createDefaultRegexRename Then
			Call util_RegexSave(createDefaultRegexScrape, createDefaultRegexRename) 'Valid regex XML doc not available, so create default one.
		End If
	End Sub

	Public Sub util_RegexSave(Optional ByVal setScraperDefault As Boolean = False, Optional ByVal setRenameDefault As Boolean = False)
		Dim doc As New XmlDocument
		Dim xmlProc As XmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
		Dim root As XmlElement
		If setScraperDefault    = True Then Pref.util_RegexSetDefaultScraper()
		If setRenameDefault     = True Then Pref.util_RegexSetDefaultRename()
		doc.AppendChild(xmlProc)
		root = doc.CreateElement("regexlist")
        root.AppendChildList(doc    , "tvregexscrape"   , Pref.tv_RegexScraper)
        root.AppendChildList(doc    , "tvregexrename"   , Pref.tv_RegexRename)
		doc.AppendChild(root)

        WorkingWithNfoFiles.SaveXMLDoc(doc, workingProfile.RegExList)
	End Sub

	Private Sub GenreMasterLoad()
		genrelist.Clear()
		genrelist = Utilities.loadGenre
	End Sub

	Private Sub util_GenreLoad()
		If Not File.Exists(workingProfile.Genres) Then Exit Sub
		Dim line As String = String.Empty
		Dim listof As New List(Of String)
		listof.Clear()
		genrelist.Sort()
		Try
			Using userConfig As IO.StreamReader = File.OpenText(workingProfile.Genres)
			    Do
				    Try
					    line = userConfig.ReadLine
					    If line = Nothing Then Continue Do
					    Dim regexMatch As Match
					    regexMatch = Regex.Match(line, "<([\d]{2,3})>")
					    If regexMatch.Success = False AndAlso (genrelist.FindIndex(Function(x) x.Equals(line.trim, StringComparison.OrdinalIgnoreCase)) = -1) Then
						    listof.Add(line.trim)
					    End If
				    Catch ex As Exception
					    MessageBox.Show(ex.Message)
				    End Try
			    Loop Until line = Nothing
			End Using
		Catch ex As Exception
			MessageBox.Show(ex.Message)
		End Try
		If listof.Count > 0 Then
			If Pref.GenreCustomBefore Then
				listof.Sort()
				genrelist.Sort()
				genrelist.InsertRange(0, listof)
			Else
				genrelist.AddRange(listof)
				genrelist.Sort()
			End If
		End If
	End Sub

	Private Sub util_PrefsLoad()
		Dim tempstring As String
		For Each prof In profileStruct.ProfileList
			If prof.profilename = workingProfile.profilename Then
				tempstring = prof.Config
				If File.Exists(tempstring) Then Pref.configpath = tempstring
				Pref.configpath = tempstring
				Pref.SetUpPreferences()
				Pref.ConfigLoad()
				If Pref.CheckForNewVersion Then
					BckWrkCheckNewVersion.RunWorkerAsync(False)
					Do Until Not BckWrkCheckNewVersion.IsBusy
						Application.DoEvents()
					Loop
					If CloseMC AndAlso Pref.CloseMCForDLNewVersion Then
						frmSplash.Close()
						Process.GetCurrentProcess.Kill()
						Application.Exit()
						Me.Close()
					Else
						CloseMC = False
					End If
				End If
				Me.util_ConfigLoad()
			End If
		Next
        MergePrefMovieSetToMovieSetDatabase()
	End Sub

	Private Sub util_ProfilesLoad()
		profileStruct.ProfileList.Clear()
		Dim profilepath As String = Path.Combine(applicationPath, "Settings")
		profilepath = Path.Combine(profilepath, "profile.xml")

		Dim notportable As Boolean = False
		Dim Propath As String = profilepath
		If File.Exists(Propath) Then
			Try
				Dim profilelist As New XmlDocument
				profilelist.Load(Propath)
				If profilelist.DocumentElement.Name = "profile" Then
					For Each thisresult As XmlNode In profilelist("profile")
						Select Case thisresult.Name
							Case "default"
								profileStruct.DefaultProfile = thisresult.InnerText
							Case "startup"
								profileStruct.StartupProfile = thisresult.InnerText
							Case "profiledetails"
								Dim currentprofile As New ListOfProfiles
								Dim result As XmlNode
								For Each result In thisresult.childnodes
									Dim t As Integer = result.InnerText.ToString.ToLower.IndexOf("\s")
									If t > 0 Then notportable = True
									Select Case result.name
										Case "actorcache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.ActorCache = applicationPath & s
										Case "directorcache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.DirectorCache = applicationPath & s
										Case "config"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.Config = applicationPath & s
										Case "moviecache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.MovieCache = applicationPath & s
										Case "profilename"
											currentprofile.ProfileName = result.InnerText
										Case "regex"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.RegExList = applicationPath & s
										Case "genres"
											Dim s As String = ""
											If result.innertext = "" Then
												s = "\Settings\genres.txt"  'incase missing from existing profile.xml
											Else
												s = result.InnerText.ToString.Substring(t)
											End If
											currentprofile.Genres = applicationPath & s
										Case "tvcache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.TvCache = applicationPath & s
										Case "musicvideocache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.MusicVideoCache = applicationPath & s
										Case "moviesetcache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.MovieSetCache = applicationPath & s
										Case "customtvcache"
											Dim s As String = result.InnerText.ToString.Substring(t)
											currentprofile.CustomTvCache = applicationPath & s
									End Select
								Next
								profileStruct.ProfileList.Add(currentprofile)
						End Select
					Next
				End If
			Catch ex As Exception
#If SilentErrorScream Then
                Throw ex
#End If

			End Try
		Else

		End If

		If notportable Then util_ProfileSave()

		If profileStruct.ProfileList.Count > 1 Then
			ProfilesToolStripMenuItem.Visible = True
			ProfilesToolStripMenuItem.Enabled = True
			ProfilesToolStripMenuItem.DropDownItems.Clear()

			For Each prof In profileStruct.ProfileList
				If prof.ProfileName <> Nothing Then
					ProfilesToolStripMenuItem.DropDownItems.Add(prof.ProfileName)
				End If
			Next
			For Each item In ProfilesToolStripMenuItem.DropDownItems
				If item.text = workingProfile.profilename Then
					With item
						item.checked = True
					End With
				Else
					item.checked = False
				End If
			Next
		End If
	End Sub

	Public Sub util_ProfileSave()
		Dim profilepath As String = Path.Combine(applicationPath, "Settings")
		profilepath = Path.Combine(profilepath, "profile.xml")
		Dim doc As New XmlDocument
		Dim thispref As XmlNode = Nothing
		Dim xmlproc As XmlDeclaration
        Dim root As XmlElement
		Dim child As XmlElement
		xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
		doc.AppendChild(xmlproc)
		
		root = doc.CreateElement("profile")
		root.AppendChild(doc    , "default"     , profileStruct.DefaultProfile)
		root.AppendChild(doc    , "startup"     , profileStruct.StartupProfile)
		
		For Each prof In profileStruct.ProfileList
			child = doc.CreateElement("profiledetails")
            child.AppendChild(doc   , "actorcache"          , prof.ActorCache.Replace(applicationPath, ""))
            child.AppendChild(doc   , "directorcache"       , prof.DirectorCache.Replace(applicationPath, ""))
            child.AppendChild(doc   , "config"              , prof.Config.Replace(applicationPath, ""))
            child.AppendChild(doc   , "moviecache"          , prof.MovieCache.Replace(applicationPath, ""))
            child.AppendChild(doc   , "profilename"         , prof.ProfileName)
            child.AppendChild(doc   , "regex"               , prof.RegExList.Replace(applicationPath, ""))
            child.AppendChild(doc   , "genres"              , prof.Genres.Replace(applicationPath, ""))
            child.AppendChild(doc   , "tvcache"             , prof.TvCache.Replace(applicationPath, ""))
            child.AppendChild(doc   , "musicvideocache"     , prof.MusicVideoCache.Replace(applicationPath, ""))
            child.AppendChild(doc   , "moviesetcache"       , prof.MovieSetCache.Replace(applicationPath, ""))
            child.AppendChild(doc   , "customtvcache"       , prof.CustomTvCache.Replace(applicationPath, ""))
			root.AppendChild(child)
		Next

		doc.AppendChild(root)

        WorkingWithNfoFiles.SaveXMLDoc(doc, profilepath)

		If profileStruct.ProfileList.Count > 1 Then
			ProfilesToolStripMenuItem.Visible = True
			ProfilesToolStripMenuItem.Enabled = True
			ProfilesToolStripMenuItem.DropDownItems.Clear()
			For Each prof In profileStruct.ProfileList
				If prof.ProfileName <> Nothing Then ProfilesToolStripMenuItem.DropDownItems.Add(prof.ProfileName)
			Next
			For Each item In ProfilesToolStripMenuItem.DropDownItems
				If item.text = workingProfile.profilename Then
					With item
						item.checked = True
					End With
				Else
					item.checked = False
				End If
			Next
		End If
	End Sub

	Private Sub util_CommandListLoad()
		For Each com In Pref.commandlist
			ToolsToolStripMenuItem.DropDownItems.Add(com.title)
		Next
	End Sub

    Private Sub MergePrefMovieSetToMovieSetDatabase()
        ''' Merging User Sets into MovieSetDatabase
        ''' So we can support Collection Plot
        ''' with User Sets.
        Dim removedSets As New List(Of String)
        For each mset In Pref.moviesets
            If mset.ToLower = "-none-" Then Continue For
            Dim q = From x In oMovies.MovieSetDB Where x.MovieSetDisplayName = mset
            If q.Count = 0 Then
                Dim NewSet As New MovieSetInfo
                NewSet.MovieSetName = mset
                NewSet.TmdbSetId = oMovies.setMovieSetID("", oMovies.MovieSetDB)
                removedSets.Add(mset)
                oMovies.AddUpdateMovieSetInCache(NewSet)
            Else
                removedSets.Add(mset)
            End If
        Next
        For each mset In removedSets
            Pref.moviesets.Remove(mset)
        Next

    End Sub

	Private Sub mov_ActorRebuild()
		oMovies.RebuildMoviePeopleCaches
	End Sub

	Private Sub MovToggleReset()
		MovFanartToggle = False
		MovPosterToggle = False
		btnMovFanartToggle.Text = "Show MovieSet Fanart"
		btnMovFanartToggle.BackColor = System.Drawing.Color.Lime
		btnMovPosterToggle.Text = "Show MovieSet Posters"
		btnMovPosterToggle.BackColor = System.Drawing.Color.Lime
		btn_IMPA_posters.Enabled = True
		btn_MPDB_posters.Enabled = True
		btn_IMDB_posters.Enabled = True
	End Sub

	Public Sub mov_FormPopulate(Optional yieldIng As Boolean = False)

		If Not IsNothing(workingMovieDetails) Then
			If workingMovie.fullpathandfilename <> workingMovieDetails.fileinfo.fullpathandfilename Then
				Try
					For i = panelAvailableMoviePosters.Controls.Count - 1 To 0 Step -1
						panelAvailableMoviePosters.Controls.RemoveAt(i)
					Next
				Catch
				End Try
				Try
					For i = Panel2.Controls.Count - 1 To 0 Step -1
						Panel2.Controls.RemoveAt(i)
					Next
					MovToggleReset()
				Catch
				End Try
				Try
					TextBox8.Text = ""
				Catch
				End Try
			End If
		End If

		If Yield(yieldIng) Then Return

		If workingMovie.fullpathandfilename <> Nothing And DataGridViewMovies.Rows.Count > 0 Then
			workingMovieDetails = WorkingWithNfoFiles.mov_NfoLoadFull(workingMovie.fullpathandfilename)

			If Yield(yieldIng) Then Return

			If IsNothing(workingMovieDetails) = False Then
				If workingMovieDetails.fullmoviebody.playcount      = Nothing Then workingMovieDetails.fullmoviebody.playcount      = "0"
				If workingMovieDetails.fullmoviebody.lastplayed     = Nothing Then workingMovieDetails.fullmoviebody.lastplayed     = ""
				If workingMovieDetails.fullmoviebody.credits        = Nothing Then workingMovieDetails.fullmoviebody.credits        = ""
				If workingMovieDetails.fullmoviebody.director       = Nothing Then workingMovieDetails.fullmoviebody.director       = ""
				If workingMovieDetails.fullmoviebody.stars          = Nothing Then workingMovieDetails.fullmoviebody.stars          = ""
				If workingMovieDetails.fullmoviebody.filename       = Nothing Then workingMovieDetails.fullmoviebody.filename       = ""
				If workingMovieDetails.fullmoviebody.genre          = Nothing Then workingMovieDetails.fullmoviebody.genre          = ""
				If workingMovieDetails.fullmoviebody.imdbid         = Nothing Then workingMovieDetails.fullmoviebody.imdbid         = ""
				If workingMovieDetails.fullmoviebody.mpaa           = Nothing Then workingMovieDetails.fullmoviebody.mpaa           = ""
				If workingMovieDetails.fullmoviebody.outline        = Nothing Then workingMovieDetails.fullmoviebody.outline        = ""
				If workingMovieDetails.fullmoviebody.playcount      = Nothing Then workingMovieDetails.fullmoviebody.playcount      = ""
				If workingMovieDetails.fullmoviebody.plot           = Nothing Then workingMovieDetails.fullmoviebody.plot           = ""
				If workingMovieDetails.fullmoviebody.premiered      = Nothing Then workingMovieDetails.fullmoviebody.premiered      = ""
				If workingMovieDetails.fullmoviebody.rating         = Nothing Then workingMovieDetails.fullmoviebody.rating         = ""
				If workingMovieDetails.fullmoviebody.runtime        = Nothing Then workingMovieDetails.fullmoviebody.runtime        = ""
				If workingMovieDetails.fullmoviebody.studio         = Nothing Then workingMovieDetails.fullmoviebody.studio         = ""
				If workingMovieDetails.fullmoviebody.tagline        = Nothing Then workingMovieDetails.fullmoviebody.tagline        = ""
				If workingMovieDetails.fullmoviebody.title          = Nothing Then workingMovieDetails.fullmoviebody.title          = ""
				If workingMovieDetails.fullmoviebody.originaltitle  = Nothing Then workingMovieDetails.fullmoviebody.originaltitle  = ""
				If workingMovieDetails.fullmoviebody.top250         = Nothing Then workingMovieDetails.fullmoviebody.top250         = ""
				If workingMovieDetails.fullmoviebody.trailer        = Nothing Then workingMovieDetails.fullmoviebody.trailer        = ""
				If workingMovieDetails.fullmoviebody.votes          = Nothing Then workingMovieDetails.fullmoviebody.votes          = ""
				If workingMovieDetails.fullmoviebody.year           = Nothing Then workingMovieDetails.fullmoviebody.year           = ""
				If workingMovieDetails.fullmoviebody.source         = Nothing Then workingMovieDetails.fullmoviebody.source         = ""

				titletxt.Items.Clear()

				titletxt.Items.Add(workingMovieDetails.fullmoviebody.title)
				For Each title In workingMovieDetails.alternativetitles
					titletxt.Items.Add(title)
				Next
				titletxt.Text           = workingMovieDetails.fullmoviebody.title
				tb_MovFanartMovieTitle.Text = workingMovieDetails.fullmoviebody.title & " (" & workingMovieDetails.fullmoviebody.year & ")"
				tbCurrentMoviePoster.Text   = workingMovieDetails.fullmoviebody.title & " (" & workingMovieDetails.fullmoviebody.year & ")"
				Me.ToolTip1.SetToolTip(Me.titletxt, "Original Title: '" & workingMovieDetails.fullmoviebody.originaltitle & "'")
				If workingMovieDetails.fullmoviebody.sortorder = "" Then workingMovieDetails.fullmoviebody.sortorder = workingMovieDetails.fullmoviebody.title
				tb_MovSortOrder .Text = workingMovieDetails.fullmoviebody.sortorder
				outlinetxt      .Text = workingMovieDetails.fullmoviebody.outline
				plottxt         .Text = workingMovieDetails.fullmoviebody.plot
				taglinetxt      .Text = workingMovieDetails.fullmoviebody.tagline
				txtStars        .Text = workingMovieDetails.fullmoviebody.stars
				genretxt        .Text = workingMovieDetails.fullmoviebody.genre
				premiertxt      .Text = workingMovieDetails.fullmoviebody.premiered
				creditstxt      .Text = workingMovieDetails.fullmoviebody.credits
				directortxt     .Text = workingMovieDetails.fullmoviebody.director
				studiotxt       .Text = workingMovieDetails.fullmoviebody.studio
				countrytxt      .Text = workingMovieDetails.fullmoviebody.country
				pathtxt         .Text = workingMovie.fullpathandfilename
				ratingtxt       .Text = workingMovieDetails.fullmoviebody.rating.FormatRating
				cbUsrRated      .Text = If(workingMovieDetails.fullmoviebody.usrrated = "0", "None", workingMovieDetails.fullmoviebody.usrrated)
				SetTagTxtField
				tagtxt          .Text = ""
				'tb_tagtxt_changed = False
				If workingMovieDetails.fullmoviebody.tag.Count <> 0 Then
					Dim first As Boolean = True
					For Each t In workingMovieDetails.fullmoviebody.tag
						If Not first Then tagtxt.Text &= ", "
						tagtxt.Text &= t
						first = False
					Next
				End If
                tb_tagtxt_changed = False
				'Catch exception thrown when votes is an empty string
				If workingMovieDetails.fullmoviebody.votes <> "" AndAlso workingMovieDetails.fullmoviebody.votes <> "scraper error" Then
					Dim votestext As String = workingMovieDetails.fullmoviebody.votes
					votestext           = votestext.RemoveWhitespace
					votestxt.Text       = Double.Parse(votestext.Replace(".", "").Replace(",","")).ToString("N0")
				Else
					votestxt.Text       = workingMovieDetails.fullmoviebody.votes
				End If
				certtxt.Text            = workingMovieDetails.fullmoviebody.mpaa
                top250txt.Text          = If(lbl_movTop250.Text = "Top 250", workingMovieDetails.fullmoviebody.top250, workingMovieDetails.fullmoviebody.metascore)
                displayRuntimeScraper   = If(Pref.movieRuntimeDisplay = "file", False, True)
				Call mov_SwitchRuntime()

				workingMovieDetails.fileinfo.fullpathandfilename    = workingMovie.fullpathandfilename
				workingMovieDetails.fileinfo.filename               = Path.GetFileName(workingMovie.fullpathandfilename)
				workingMovieDetails.fileinfo.path                   = Path.GetFullPath(workingMovie.fullpathandfilename)
				workingMovieDetails.fileinfo.foldername             = workingMovie.foldername
				workingMovieDetails.fileinfo.trailerpath            = pref.ActualTrailerPath(workingMovieDetails.fileinfo.path)
				If Yield(yieldIng) Then Return
				HandleTrailerBtn(workingMovieDetails)
				If Yield(yieldIng) Then Return
				If workingMovieDetails.fileinfo.posterpath <> Nothing Then

					If Not File.Exists(workingMovieDetails.fileinfo.posterpath) Then
						If File.Exists(workingMovieDetails.fileinfo.posterpath.Replace(Path.GetFileName(workingMovieDetails.fileinfo.fanartpath), "folder.jpg")) Then
							workingMovieDetails.fileinfo.posterpath = workingMovieDetails.fileinfo.posterpath.Replace(Path.GetFileName(workingMovieDetails.fileinfo.posterpath), "folder.jpg")
						End If
					End If

				End If
				If Yield(yieldIng) Then Return
				If workingMovieDetails.fileinfo.posterpath <> Nothing Then
					Dim workingposter As String = workingMovieDetails.fileinfo.posterpath
					util_ImageLoad(PbMoviePoster, workingposter, Utilities.DefaultPosterPath)
					If Yield(yieldIng) Then Return
					util_ImageLoad(PictureBoxAssignedMoviePoster, workingposter, Utilities.DefaultPosterPath)
					If Yield(yieldIng) Then Return
					lblCurrentLoadedPoster.Text = "Width: " & PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & PictureBoxAssignedMoviePoster.Image.Height.ToString
					lblMovPosterPages.Visible = False
				End If
				If workingMovieDetails.fileinfo.fanartpath <> Nothing Then util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)

				If Yield(yieldIng) Then Return

				If Convert.ToInt32(workingMovieDetails.fullmoviebody.playcount) > 0 Then
					btnMovWatched.Text      = "&Watched"
					btnMovWatched.BackColor = Color.LawnGreen
					btnMovWatched.Refresh()
				Else
					btnMovWatched.Text      = "Un&watched"
					btnMovWatched.BackColor = Color.Red
					btnMovWatched.Refresh()
				End If

				cbMovieDisplay_Actor.Items.Clear()
				For Each actor In workingMovieDetails.listactors
					If actor.actorname <> Nothing Then cbMovieDisplay_Actor.Items.Add(actor.actorname)
				Next

				If cbMovieDisplay_Actor.Items.Count > 0 Then
					cbMovieDisplay_Actor.SelectedIndex = 0
				Else
					util_ImageLoad(PictureBoxActor, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
				End If

				Dim fi As New FilteredItems(cbFilterActor)
				fi.SelectFirstMatch(cbMovieDisplay_Actor)

				If Yield(yieldIng) Then Return
                
				cbMovieDisplay_MovieSet.SelectedItem = Nothing

				pop_cbMovieDisplay_MovieSet
				cbMovieDisplay_MovieSet.Enabled = Not workingMovieDetails.fullmoviebody.Locked("set")

				For f = 0 To cbMovieDisplay_Source.Items.Count - 1
					If cbMovieDisplay_Source.Items(f) = workingMovieDetails.fullmoviebody.source Then
						cbMovieDisplay_Source.SelectedIndex = f
						Exit For
					End If
				Next

				btnPlayMovie.Enabled = True
				mov_SplitContainerAutoPosition

				Dim video_flags = VidMediaFlags(workingMovieDetails.filedetails, workingMovieDetails.fullmoviebody.title.ToLower.Contains("3d"), workingMovie.DisplayFolderSize)
				movieGraphicInfo.OverlayInfo(PbMovieFanArt, ratingtxt.Text, video_flags)
				MovPanel6Update()
			End If
		Else
			cbMovieDisplay_Actor.Items.Clear()
			PictureBoxActor                 .CancelAsync()
			PictureBoxActor                 .Image = Nothing
			PictureBoxActor                 .Refresh()
			btnMoviePosterSaveCroppedImage  .Enabled = False
			btnMoviePosterResetImage        .Enabled = False
			cbMoviePosterSaveLoRes          .Enabled = False
			btnPosterTabs_SaveImage         .Enabled = False
			btnMovPosterNext                .Visible = False
			btnMovPosterPrev                .Visible = False
			lblMovPosterPages               .Visible = False
			titletxt                        .Text = ""
			tb_MovFanartMovieTitle          .Text = ""
			outlinetxt                      .Text = ""
			plottxt                         .Text = ""
			taglinetxt                      .Text = ""
			txtStars                        .Text = ""
			genretxt                        .Text = ""
			premiertxt                      .Text = ""
			creditstxt                      .Text = ""
			directortxt                     .Text = ""
			studiotxt                       .Text = ""
			countrytxt                      .Text = ""
			pathtxt                         .Text = ""
			cbUsrRated                      .SelectedIndex  = -1
			ratingtxt                       .Text = ""
			runtimetxt                      .Text = ""
			votestxt                        .Text = ""
			top250txt                       .Text = ""
			certtxt                         .Text = ""
			PbMovieFanArt                   .Image = Nothing
			PictureBox2                     .Image = Nothing
			PbMoviePoster                   .Image = Nothing
			lblMovFanartWidth               .Text = ""
			lblMovFanartHeight              .Text = ""
			PictureBoxAssignedMoviePoster   .Image = Nothing
			lblCurrentLoadedPoster          .Text = ""
			tb_MovSortOrder                 .Text = ""
			tagtxt                          .Text = ""
			roletxt                         .Text = ""
			PictureBoxActor                 .Image = Nothing
			Panel6                          .Visible = False
			FanTvArtList.Items.Clear()
			btnPlayMovie                    .Enabled = False
            tb_tagtxt_changed = False
			Me.Refresh()
			Application.DoEvents()
		End If
		If ratingtxt.Text.IndexOf("/10") <> -1 Then
			ratingtxt.Text = ratingtxt.Text.Replace("/10", "")
			workingMovieDetails.fullmoviebody.rating = ratingtxt.Text
		End If

		If ratingtxt.Text.Length > 3 Then ratingtxt.Text = ratingtxt.Text.Substring(0, 3).Trim

		If Yield(yieldIng) Then Return
		GC.Collect()
	End Sub

	Private Sub Mov_PictureboxLoad()
		util_ImageLoad(PbMoviePoster, workingMovieDetails.fileinfo.posterpath, Utilities.DefaultPosterPath)
		util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultPosterPath)
	End Sub

	Public Function CheckforExtraArt() As Boolean
		Dim confirmedpresent As Boolean = False
		If File.Exists(workingMovieDetails.fileinfo.movsetposterpath) Then FanTvArtList.Items.Add("Set Poster") : confirmedpresent = True
		If File.Exists(workingMovieDetails.fileinfo.movsetfanartpath) Then FanTvArtList.Items.Add("Set Fanart") : confirmedpresent = True
		If Not Pref.GetRootFolderCheck(workingMovieDetails.fileinfo.fullpathandfilename) Then
			Dim MovPath As String = Path.GetDirectoryName(workingMovieDetails.fileinfo.fullpathandfilename) & "\"
			If Pref.MovFanartNaming Then MovPath = workingMovieDetails.fileinfo.fullpathandfilename.Replace(".nfo", "-")
			If File.Exists(MovPath & "clearart.png") Then FanTvArtList.Items.Add("ClearArt") : confirmedpresent = True
			If File.Exists(MovPath & "logo.png") Then FanTvArtList.Items.Add("Logo") : confirmedpresent = True
			If File.Exists(MovPath & "banner.jpg") Then FanTvArtList.Items.Add("Banner") : confirmedpresent = True
			If File.Exists(MovPath & "landscape.jpg") Then FanTvArtList.Items.Add("Landscape") : confirmedpresent = True
			If File.Exists(MovPath & "disc.png") Then FanTvArtList.Items.Add("Disc") : confirmedpresent = True
			If File.Exists(MovPath & "poster.jpg") AndAlso Not Pref.posterjpg AndAlso Not Pref.MovFanartNaming Then FanTvArtList.Items.Add("Poster") : confirmedpresent = True
			If File.Exists(MovPath & "fanart.jpg") AndAlso Not Pref.fanartjpg AndAlso Not Pref.MovFanartNaming Then FanTvArtList.Items.Add("Fanart") : confirmedpresent = True
			If File.Exists(MovPath & "folder.jpg") Then FanTvArtList.Items.Add("Folder") : confirmedpresent = True
		End If
		Return confirmedpresent
	End Function

	Public Sub MovPanel6Update()
		FanTvArtList.Items.Clear()
		Panel6.Visible = CheckforExtraArt()
	End Sub

	Private Sub FanTvArtList_Mouse(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FanTvArtList.MouseDown
		Dim imagepath As String = Nothing
		Dim item As String = Nothing
		Dim pbheight As Integer = 112
		If e.button = Windows.Forms.MouseButtons.Right Then
			Dim index As Integer = FanTvArtList.IndexFromPoint(New Point(e.X, e.Y))
			If index >= 0 Then FanTvArtList.SelectedItem = FanTvArtList.Items(index)
		End If
		If IsNothing(FanTvArtList.SelectedItem) Then Exit Sub
		item = FanTvArtList.SelectedItem.ToString.ToLower
		If Not String.IsNullOrEmpty(item) Then
			If item.ToLower = "set poster" Or item.ToLower = "set fanart" Then
				If item.ToLower = "set poster" Then
					imagepath = workingMovieDetails.fileinfo.movsetposterpath
				Else
					imagepath = workingMovieDetails.fileinfo.movsetfanartpath
				End If
			Else
				If Pref.MovFanartNaming Then
					imagepath = workingMovieDetails.fileinfo.fullpathandfilename.Replace(".nfo", "-")
				Else
					imagepath = Path.GetDirectoryName(workingMovieDetails.fileinfo.fullpathandfilename) & "\"
				End If

				Dim suffix As String = If((item = "clearart" or item = "logo" or item = "disc"), ".png", ".jpg")
				imagepath &= item & suffix
			End If

		End If
		If e.Button = Windows.Forms.MouseButtons.Left Then
			ftvArtPicBox.Visible = True
			If IsNothing(imagepath) Then Exit Sub
			util_ImageLoad(ftvArtPicBox, imagepath, "")
			Try
				Dim panelwidth As Integer = SpltCntr_MovDisplay.Panel2.Width
				Dim panelheight As Integer = SpltCntr_MovDisplay.Panel2.Height
				Dim pbw As Integer = Math.Ceiling(panelwidth * .413)
				Dim pbh As Integer = Math.Ceiling(PbMovieFanArt.Height * .90)
				Dim imgw As Integer = ftvArtPicBox.Image.Width
				Dim imgh As Integer = ftvArtPicBox.Image.Height
				If item.ToLower.Contains("poster") Or item.ToLower.Contains("disc") Then
					ftvArtPicBox.Height = pbh
					ftvArtPicBox.Width = Math.Ceiling(pbh / (imgh / imgw))
					ftvArtPicBox.Location = New System.Drawing.Point(Math.Ceiling((panelwidth * .4) - (ftvArtPicBox.Width / 2)), 45)
				Else
					ftvArtPicBox.Height = Math.Ceiling(pbw * (imgh / imgw))
					ftvArtPicBox.Width = pbw
					ftvArtPicBox.Location = New System.Drawing.Point(140, Math.Ceiling((panelheight * .3) - (ftvArtPicBox.Height / 2)))
				End If
			Catch
			End Try
		ElseIf e.button = Windows.Forms.MouseButtons.Right Then
			Dim tempint = MessageBox.show("Do you wish to delete this image from" & vbCrLf & "this Movie?", "Fanart.Tv Artwork Delete", MessageBoxButtons.YesNoCancel)
			If tempint = Windows.Forms.DialogResult.No or tempint = DialogResult.Cancel Then Exit Sub
			If tempint = Windows.Forms.DialogResult.Yes Then
				Utilities.SafeDeleteFile(imagepath)
				MovPanel6Update()
			End If
		End If
	End Sub

	Private Sub FanTvArtList_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FanTvArtList.MouseLeave
		FanTvArtList.ClearSelected()
		ftvArtPicBox.Image = Nothing
		ftvArtPicBox.Visible = False
	End Sub

	Private Sub HandleTrailerBtn(ByVal fmd As FullMovieDetails)

		If IsNothing(fmd) Then
			Return
		End If

		DeleteZeroLengthFile(fmd.fileinfo.trailerpath)

		ButtonTrailer.Enabled = False

		If File.Exists(fmd.fileinfo.trailerpath) Then
			ButtonTrailer.Text = "Play Trailer"
			ButtonTrailer.Enabled = True
		Else
			If fmd.fullmoviebody.trailer = ""
				ButtonTrailer.Text = "No trailer found"
			Else
				ButtonTrailer.Text = "Download Trailer"
				ButtonTrailer.Enabled = True
			End If
		End If
	End Sub

	Private Function mov_FileCheckValid(ByVal fullpathandfilename As String) As Boolean
		Dim validfile As Boolean = True
		Dim tempint2 As Integer = 0
		Dim tempstring As String
		Dim extension As String
		Dim tempname As String

		'First check, is it a video file!
		extension = Path.GetExtension(fullpathandfilename)
		Dim isvideofile As Boolean = Utilities.VideoExtensions.Contains(extension)
		'For Each extn In Utilities.VideoExtensions
		'	If extn = extension Then isvideofile = True
		'Next
		If Not isvideofile Then Return False

		'if the file is a .vob then check it is not part of a dvd folder (Stop dvdfolders vobs getting seperate nfos)
		If Path.GetExtension(fullpathandfilename) = ".vob" Then
			If File.Exists(fullpathandfilename.Replace(Path.GetFileName(fullpathandfilename), "VIDEO_TS.IFO")) Then
				validfile = False
			End If
		End If

        If Path.GetExtension(fullpathandfilename) = ".vro" Then
			If File.Exists(fullpathandfilename.Replace(Path.GetFileName(fullpathandfilename), "VR_MANGR.IFO")) Then
				validfile = False
			End If
		End If

		Dim filename2 As String = Path.GetFileName(fullpathandfilename).ToLower
		For each cleanmultipart In Utilities.cleanMultipart
			For Each separator In Utilities.separators
				For I = 2 To 5
					Dim lookfor As String = cleanmultipart & separator & I.ToString
					If filename2.IndexOf(cleanmultipart & separator & I.ToString) <> -1 Then validfile = False
				Next
			Next
		Next
		If filename2.IndexOf("-trailer") <> -1 Then validfile = False
		If filename2.IndexOf(".trailer") <> -1 Then validfile = False
		If filename2.IndexOf("_trailer") <> -1 Then validfile = False
		If filename2.IndexOf("-theme") <> -1 Then validfile = False
		If filename2.IndexOf(".theme") <> -1 Then validfile = False
		If filename2.IndexOf("_theme") <> -1 Then validfile = False
		If filename2.IndexOf("sample") <> -1 And filename2.IndexOf("people") = -1 Then validfile = False


		'check for movies ending a,b,c, etc (moviea, movieb) for multipart. movieb is multipart if moviea exists
		'extension = System.Path.GetExtension(fullpathandfilename)
		tempname = fullpathandfilename.Replace(extension, "")
		If tempname.Substring(tempname.Length - 1) = "b" Or tempname.Substring(tempname.Length - 1) = "c" Or tempname.Substring(tempname.Length - 1) = "d" Or tempname.Substring(tempname.Length - 1) = "e" Or tempname.Substring(tempname.Length - 1) = "B" Or tempname.Substring(tempname.Length - 1) = "C" Or tempname.Substring(tempname.Length - 1) = "D" Or tempname.Substring(tempname.Length - 1) = "E" Then
			tempname = fullpathandfilename.Substring(0, fullpathandfilename.Length - (1 + extension.Length)) & "a" & extension
			If File.Exists(tempname) Then validfile = False
		End If

		'now need to deal with multipart rar files
		Dim tempmovie2 As String = fullpathandfilename.Replace(".nfo", ".rar")
		Dim tempmovie As String = String.Empty
		If File.Exists(tempmovie2) = True Then
			If File.Exists(fullpathandfilename) = False Then
				Dim rarname As String = tempmovie2
				Dim SizeOfFile As Integer = FileLen(rarname)
				tempint2 = Convert.ToInt32(Pref.rarsize) * 1048576
				If SizeOfFile > tempint2 Then
					Dim mat As Match
					mat = Regex.Match(rarname, "\.part[0-9][0-9]?[0-9]?[0-9]?.rar")
					If mat.Success = True Then
						rarname = mat.Value
						If rarname.ToLower.IndexOf(".part1.rar") <> -1 Or rarname.ToLower.IndexOf(".part01.rar") <> -1 Or rarname.ToLower.IndexOf(".part001.rar") <> -1 Or rarname.ToLower.IndexOf(".part0001.rar") <> -1 Then
							Dim stackrarexists As Boolean = False
							rarname = fullpathandfilename.Replace(".nfo", ".rar")
							If rarname.ToLower.IndexOf(".part1.rar") <> -1 Then
								rarname = rarname.Replace(".part1.rar", ".nfo")
                                tempmovie = rarname
								stackrarexists = File.Exists(rarname)
							End If
							If rarname.ToLower.IndexOf(".part01.rar") <> -1 Then
								rarname = rarname.Replace(".part01.rar", ".nfo")
								tempmovie = rarname
								stackrarexists = File.Exists(rarname)
							End If
							If rarname.ToLower.IndexOf(".part001.rar") <> -1 Then
								rarname = rarname.Replace(".part001.rar", ".nfo")
								tempmovie = rarname
								stackrarexists = File.Exists(rarname)
							End If
							If rarname.ToLower.IndexOf(".part0001.rar") <> -1 Then
								rarname = rarname.Replace(".part0001.rar", ".nfo")
								tempmovie = rarname
								stackrarexists = File.Exists(rarname)
							End If
							If stackrarexists = True Then
								Dim allok As Boolean = False
								Try
									Using filechck As IO.StreamReader = File.OpenText(tempmovie)
									    Do
										    tempstring = filechck.ReadLine
										    If tempstring = Nothing Then Exit Do

										    If tempstring.IndexOf("<movie") <> -1 Then
											    allok = True
											    Exit Do
										    End If
									    Loop Until tempstring.IndexOf("</movie>") <> -1
									End Using
								Catch ex As Exception
#If SilentErrorScream Then
                                    Throw ex
#End If
								Finally
								End Try
								If allok Then validfile = False
							End If
						Else
							validfile = False
						End If
					End If
				Else
					validfile = False
				End If
			End If
		End If

		'rename fullpathandfilename to that of the nfo file
		fullpathandfilename = fullpathandfilename.Replace(fullpathandfilename.Substring(fullpathandfilename.LastIndexOf("."), fullpathandfilename.Length - fullpathandfilename.LastIndexOf(".")), ".nfo")

		'check for both variations of the filename
		Dim nfopaths(1) As String
		nfopaths(0) = fullpathandfilename
		nfopaths(1) = fullpathandfilename.Replace(Path.GetFileName(fullpathandfilename), "movie.nfo")
		'check if the file exists
		For f = 0 To 1
			If File.Exists(nfopaths(f)) Then
				'if it does check if it is a valid xbmc nfo, if it is not then move it or delete it according to prefs
				Try
					Using filechck As IO.StreamReader = File.OpenText(nfopaths(f))
					    tempstring = filechck.ReadToEnd
					End Using
					If tempstring.IndexOf("<movie") = -1 And tempstring.IndexOf("</movie>") = -1 Then
						If Pref.renamenfofiles = True Then
							Dim fi As New FileInfo(nfopaths(f))
							Dim newpath As String = nfopaths(f).Replace(nfopaths(f).Substring(nfopaths(f).LastIndexOf("."), nfopaths(f).Length - nfopaths(f).LastIndexOf(".")), ".info")
							fi.MoveTo(newpath)
						End If
					Else
						validfile = False
					End If
				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If

				Finally
				End Try
			End If
		Next
		Return validfile
	End Function

	Private Sub DeleteZeroLengthFile(ByVal fileName)
		If File.Exists(fileName) AndAlso (New FileInfo(fileName)).Length = 0 Then Utilities.SafeDeleteFile(fileName)
	End Sub

	Private Sub ReloadMovieCacheToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadMovieCacheToolStripMenuItem.Click
		mov_CacheLoad()
	End Sub

	Private Sub StartVideo(ByVal tempstring As String)
		If Pref.videomode = 4 AndAlso String.IsNullOrEmpty(Pref.selectedvideoplayer) Then
            Call util_VideoMode(1, tempstring)
        Else
            Call util_VideoMode(Pref.videomode, tempstring)
		End If
	End Sub

	Private Sub util_VideoMode(ByVal mode As Integer, ByVal tempstring As String)
		Dim action As String = ""
		Dim errors As String = ""
		If mode = 1 Then
			Try
				Dim myProc As Process = Process.Start(tempstring)
			Catch ex As Exception
				errors = ex.ToString
				action = "Dim myProc As Process = Process.Start(" & tempstring & ")"
				Call util_ErrorLog(action, errors)
			End Try
		ElseIf mode = 2 Then
			Try
				Dim thePSI As New System.Diagnostics.ProcessStartInfo("wmplayer")
				thePSI.Arguments = """" & tempstring & """"
				System.Diagnostics.Process.Start(thePSI)
			Catch ex As Exception
				errors = ex.ToString
				action = "Dim thePSI As New System.Diagnostics.ProcessStartInfo(""wmplayer"")" & vbCrLf & "thePSI.Arguments = "" & tempstring & """ & vbCrLf & "System.Diagnostics.Process.Start(thePSI)"
				Call util_ErrorLog(action, errors)
			End Try
		ElseIf mode = 4 Then
			Try
				Dim myProc As Process = Process.Start("""" & Pref.selectedvideoplayer & """", """" & tempstring & """")
			Catch ex As Exception
				errors = ex.ToString
				action = "Dim myProc As Process = Process.Start(""" & Pref.selectedvideoplayer & """," & """" & tempstring & """)"
				Call util_ErrorLog(action, errors)
			End Try
		End If
	End Sub
    
	Private Sub util_ErrorLog(ByVal action As String, Optional ByVal errors As String = "")
		Dim errpath As String = applicationPath & "\error.log"
		Try
			Dim objWriter As New IO.StreamWriter(errpath, True)
			objWriter.WriteLine(errors)
			objWriter.WriteLine(action)
			objWriter.WriteLine() '(Chr(13))
			objWriter.Close()
		Catch ex As Exception
			MsgBox("Error, cant write to " & errpath & vbCrLf & vbCrLf & ex.ToString)
		End Try

	End Sub
    
	Private Sub util_ThreadsAllExit()
		Dim busy As Boolean = False
		Try
			If bckgroundscanepisodes.IsBusy Then
				busy = True
				bckgroundscanepisodes.CancelAsync()
			End If
			If BckWrkScnMovies.IsBusy Then
				busy = True
				BckWrkScnMovies.CancelAsync()
			End If
			If BWs.Count > 0 Then
				busy = True
				Bw.CancelAsync()
			End If
			If ImgBw.IsBusy Then
				busy = True
				ImgBw.CancelAsync()
			End If
            If BckWrkTv.IsBusy Then
                busy = True
                BckWrkTv.CancelAsync()
            End If
            If BckWrkSeries.IsBusy Then
                busy = True
                BckWrkSeries.CancelAsync()
            End If
			Dim exitnowok As Boolean = False
			If busy = True Then
				messbox.TextBox1.Text = "Please Wait"
				messbox.TextBox2.Text = ""
				messbox.TextBox3.Text = "Stopping threads when it is Safe to do so"
				messbox.Refresh()
				messbox.Visible = True
			End If
			Do Until busy = False
				If Not bckgroundscanepisodes.IsBusy And Not BckWrkScnMovies.IsBusy And Not BWs.Count > 0 And Not ImgBw.IsBusy And Not BckWrkTv.IsBusy Then
					busy = False
					Exit Do
				End If
				Threading.Thread.Sleep(100)
				Application.DoEvents()
			Loop
			messbox.Visible = False
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		Finally
		End Try
	End Sub

	Public Sub util_ZoomImage(ByVal file As String)
		bigPanel = New Panel
		With bigPanel
			.Width = Me.Width
			.Height = Me.Height
			.BringToFront()
			.Dock = DockStyle.Fill
		End With

		bigPictureBox = New PictureBox()
		With bigPictureBox
			.Location = New Point(0, 0)
			.Width = bigPanel.Width
			.Height = bigPanel.Height
			.SizeMode = PictureBoxSizeMode.Zoom
			.Visible = True
			.BorderStyle = BorderStyle.Fixed3D
			AddHandler bigPictureBox.DoubleClick, AddressOf util_PicBoxClose
			.Dock = DockStyle.Fill
		End With
		util_ImageLoad(bigPictureBox, file, Utilities.DefaultPosterPath)
        
		Dim bigpanellabel As Label
		bigpanellabel = New Label
		With bigpanellabel
			.Location = New Point(20, 200)
			.Width = 170
			.Height = 75
			.Visible = True
			.Text = "Double Click Image To Return To Browser"
			.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Bold)
		End With

		Me.bigPanel.Controls.Add(bigpanellabel)
		bigpanellabel.BringToFront()
		Application.DoEvents()

		If Not bigPictureBox.Image Is Nothing And bigPictureBox.Image.Width > 20 Then
			Dim sizey As Integer = bigPictureBox.Image.Height
			Dim sizex As Integer = bigPictureBox.Image.Width
			Dim tempstring As String
			tempstring = "Full Image Resolution :- " & sizex.ToString & " x " & sizey.ToString
			Dim resolutionlbl As New Label
			With resolutionlbl
				.Location = New Point(20, 450)
				.Width = 200
				.Text = tempstring
				.BackColor = Color.Transparent
			End With

			Me.bigPanel.Controls.Add(resolutionlbl)
			resolutionlbl.BringToFront()
			Me.Refresh()
			Application.DoEvents()
			Dim tempstring2 As String = resolutionlbl.Text
		End If

		Me.Controls.Add(bigPanel)
		bigPanel.BringToFront()
		Me.bigPanel.Controls.Add(bigPictureBox)
		Me.Refresh()
	End Sub

	Private Sub util_PicBoxClose()
		Me.Controls.Remove(bigPanel)
		bigPanel = Nothing
		Me.Controls.Remove(bigPictureBox)
		bigPictureBox.Image = Nothing
		bigPictureBox = Nothing
		Me.ControlBox = True
		MenuStrip1.Enabled = True
	End Sub


	Private Sub util_FileDetailsGet()
		Try
			Dim tempstring As String = String.Empty
			Dim appPath As String = ""
			Dim exists As Boolean
			Dim movieinfo As String = String.Empty
			Dim medianfoexists As Boolean = False
			tempstring = Utilities.GetFileName(pathtxt.Text)
			If Path.GetFileName(tempstring).ToLower = "video_ts.ifo" Then
				Dim temppath As String = tempstring.Replace(Path.GetFileName(tempstring), "VTS_01_0.IFO")
				If File.Exists(temppath) Then tempstring = temppath
			End If
			If Path.GetExtension(tempstring).ToLower = ".iso" Then
				If applicationPath.IndexOf("/") <> -1 Then appPath = applicationPath & "/" & "mediainfo-rar.exe"
				If applicationPath.IndexOf("\") <> -1 Then appPath = applicationPath & "\" & "mediainfo-rar.exe"
				If Not File.Exists(appPath) Then
					MsgBox("Unable to find th file ""mediainfo-rar.exe""" & vbCrLf & vbCrLf & "Please make sure this file is available in the programs root directory")
					Exit Sub
				End If
				Try
					Dim NewProcess As New System.Diagnostics.Process()

					With NewProcess.StartInfo
						.FileName = appPath
						.Arguments = tempstring
						.RedirectStandardOutput = True
						.RedirectStandardError = True
						.RedirectStandardInput = True
						.UseShellExecute = False
						.WindowStyle = ProcessWindowStyle.Hidden
						.CreateNoWindow = False
					End With
					Dim To_Display As String = ""
					NewProcess.Start()
					movieinfo = NewProcess.StandardOutput.ReadToEnd
				Catch ex As Exception
				End Try
			Else
				If applicationPath.IndexOf("/") <> -1 Then appPath = applicationPath & "/" & "MediaInfo.dll"
				If applicationPath.IndexOf("\") <> -1 Then appPath = applicationPath & "\" & "MediaInfo.dll"
				exists = File.Exists(appPath)
				If exists = True Then medianfoexists = True
				If medianfoexists = False Then
					MsgBox("Unable to find th file ""MediaInfo.dll""" & vbCrLf & vbCrLf & "Please make sure this file is available in the programs root directory")
					Exit Sub
				End If
				Dim To_Display As String = ""
				Dim tempstring5 As String

				Dim MI As New mediainfo
				tempstring5 = MI.Option_("Info_Version", "0.7.0.0;MediaInfoDLL_Example_MSVB;0.7.0.0")
				If (tempstring5.Length() = 0) Then
					TextBox1.Text = "MediaInfo.Dll: this version of the DLL is not compatible"
					Exit Sub
				End If

				If File.Exists(tempstring) Then
					MI.Open(tempstring)
					To_Display = MI.Inform
					movieinfo = To_Display
					MI.Close()
				End If
			End If
			TextBox8.Text = movieinfo
		Catch
		End Try
	End Sub

	Private Sub mov_Rescrape()

		If outlinetxt.Text = "MC cannot find this file, either the file no longer exists, or MC cannot access the file path" Then
			MsgBox("MC cannot find this file, either the file no longer exists, or MC cannot access the file path")
			Exit Sub
		End If

		If workingMovieDetails Is Nothing Then Exit Sub

		If workingMovieDetails.fullmoviebody.title = Nothing And workingMovieDetails.fullmoviebody.imdbid = Nothing Then
			MsgBox("Can't rescrape this movie because it doesn't have any NFO File" & vbCrLf & "refresh movie database, and search for new movies", MsgBoxStyle.OkOnly, "Error")
			Exit Sub
		End If

		Dim tempint = MessageBox.Show("Rescraping the movie will Overwrite all the current details" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
		If tempint = DialogResult.No Then Exit Sub

		RunBackgroundMovieScrape("RescrapeDisplayedMovie")

	End Sub

	Private Sub mov_SaveQuick()

		If DataGridViewMovies.SelectedRows.Count = 0 Then Return

		If DataGridViewMovies.SelectedRows.Count = 1 Then

			Dim sel = workingMovieDetails.fileinfo.fullpathandfilename

			Dim movie As Movie = oMovies.LoadMovie(sel, False)
			movie.ScrapedMovie.fullmoviebody.title = titletxt.Text.Replace(" (" & workingMovieDetails.fullmoviebody.year & ")", "")
			If movie.ScrapedMovie.fullmoviebody.originaltitle = Nothing Or movie.ScrapedMovie.fullmoviebody.originaltitle = "" Then
				movie.ScrapedMovie.fullmoviebody.originaltitle = movie.ScrapedMovie.fullmoviebody.title
			End If
			movie.ScrapedMovie.fullmoviebody.director = directortxt.Text
			movie.ScrapedMovie.fullmoviebody.playcount = workingMovieDetails.fullmoviebody.playcount
			movie.ScrapedMovie.fullmoviebody.lastplayed = workingMovieDetails.fullmoviebody.lastplayed
			movie.ScrapedMovie.fullmoviebody.credits = creditstxt.Text
			movie.ScrapedMovie.fullmoviebody.studio = studiotxt.Text
			movie.ScrapedMovie.fullmoviebody.country = countrytxt.Text
			movie.ScrapedMovie.fullmoviebody.genre = genretxt.Text
			movie.ScrapedMovie.fullmoviebody.premiered = premiertxt.Text
			movie.ScrapedMovie.fullmoviebody.votes = votestxt.Text
			If lbl_movTop250.Text = "Top 250" Then
				movie.ScrapedMovie.fullmoviebody.top250 = top250txt.Text
			Else
				movie.ScrapedMovie.fullmoviebody.metascore = top250txt.Text
			End If
			movie.ScrapedMovie.fullmoviebody.rating = ratingtxt.Text
			movie.ScrapedMovie.fullmoviebody.usrrated = If(cbUsrRated.Text = "None", "0", cbUsrRated.Text)
			movie.ScrapedMovie.fullmoviebody.runtime = runtimetxt.Text
			movie.ScrapedMovie.fullmoviebody.outline = outlinetxt.Text
			movie.ScrapedMovie.fullmoviebody.plot = plottxt.Text
			movie.ScrapedMovie.fullmoviebody.tagline = taglinetxt.Text
			movie.ScrapedMovie.fullmoviebody.stars = txtStars.Text.ToString.Replace(", See full cast and crew", "")
			movie.ScrapedMovie.fullmoviebody.mpaa = certtxt.Text
			movie.ScrapedMovie.fullmoviebody.sortorder = tb_MovSortOrder.Text
			If movie.ScrapedMovie.fullmoviebody.SetName <> cbMovieDisplay_MovieSet.Items(cbMovieDisplay_MovieSet.SelectedIndex) AndAlso cbMovieDisplay_MovieSet.SelectedIndex <> -1 Then
				movie.ScrapedMovie.fullmoviebody.SetName = cbMovieDisplay_MovieSet.Items(cbMovieDisplay_MovieSet.SelectedIndex)
                Dim setid As String = oMovies.GetMovieSetIdFromName(movie.ScrapedMovie.fullmoviebody.SetName)
				If setid <> "" AndAlso Not setid.StartsWith("L") Then movie.ScrapedMovie.fullmoviebody.TmdbSetId = setid'oMovies.GetMovieSetIdFromName(movie.ScrapedMovie.fullmoviebody.SetName)
                movie.ScrapedMovie.fullmoviebody.SetOverview = oMovies.GetMovieSetOverviewFromName(movie.ScrapedMovie.fullmoviebody.SetName)
			End If
            If movie.ScrapedMovie.fullmoviebody.SetName <> "-None" AndAlso movie.ScrapedMovie.fullmoviebody.SetOverview = "" Then
                movie.ScrapedMovie.fullmoviebody.SetOverview = oMovies.GetMovieSetOverviewFromName(movie.ScrapedMovie.fullmoviebody.SetName)
            End If
			movie.ScrapedMovie.fullmoviebody.source = If(cbMovieDisplay_Source.SelectedIndex < 1, Nothing, cbMovieDisplay_Source.Items(cbMovieDisplay_Source.SelectedIndex))
			If TabControl2.SelectedTab.Name = tpMovTags.Name Then
				For Each t In NewTagList
					Dim remtag As String = t.Replace("- ", "").Replace("+ ", "")
					If t.Contains("- ") Then
						If movie.ScrapedMovie.fullmoviebody.tag.Contains(remtag) Then movie.ScrapedMovie.fullmoviebody.tag.Remove(remtag)
					ElseIf t.Contains("+ ") Then
						If Not movie.ScrapedMovie.fullmoviebody.tag.Contains(remtag) Then movie.ScrapedMovie.fullmoviebody.tag.Add(remtag)
					End If
				Next
				If movie.ScrapedMovie.fullmoviebody.tag.Count <> 0 Then
					Dim first As Boolean = True
					tagtxt.Text = ""
					For Each t In movie.ScrapedMovie.fullmoviebody.tag
						If Not first Then
							tagtxt.Text &= ", " & t
						Else
							tagtxt.Text &= t
						End If
						first = False
					Next
					tb_tagtxt_changed = False
				End If

			Else
				If Pref.AllowUserTags AndAlso tb_tagtxt_changed Then
					tb_tagtxt_changed = False
                    Dim tmplst As New List(Of String)
                    tmplst.AddRange(movie.ScrapedMovie.fullmoviebody.tag)
					movie.ScrapedMovie.fullmoviebody.tag.Clear()
					For Each wd In tagtxt.Text.Split(",")
						wd = wd.Trim
						If wd.Length = 0 Then Continue For
						movie.ScrapedMovie.fullmoviebody.tag.Add(wd)
						If Not Pref.movietags.Contains(wd) AndAlso Not tmplst.Contains(wd) Then Pref.movietags.Add(wd)
						If movie.ScrapedMovie.fullmoviebody.tag.Count >= Pref.keywordlimit Then Exit For
					Next
					ConfigSave()
				End If
			End If
			movie.SaveNFO
			movie.AssignMovieToCache
			movie.UpdateMovieCache

			DataGridViewMovies.ClearSelection
			UpdateFilteredList

			If TabControl2.SelectedTab.Name = tpMovTags.Name Then TagsPopulate()
		Else
			messbox = New frmMessageBox("Saving Selected Movies", , "     Please Wait.     ")  'Multiple movies selected
			messbox.TextBox3.Text = "Press ESC to cancel"
			messbox.TopMost = True
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			Dim Startfullpathandfilename As String = ""
			If Not ISNothing(DataGridViewMovies.CurrentRow) Then
				Dim i As Integer = DataGridViewMovies.CurrentRow.Index
                
				Startfullpathandfilename = CType(DataGridViewMovies.SelectedRows(0).DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString

				messbox.Cancelled = False
				Dim pos As Integer = 0
				Dim NfosToSave As List(Of String) = (From x As datagridviewrow In DataGridViewMovies.SelectedRows Select nfo = x.Cells("fullpathandfilename").Value.ToString).ToList
				For Each nfo As String In NfosToSave
					If Not File.Exists(nfo) Then Continue For
					Dim movie As Movie = oMovies.LoadMovie(nfo)
					If IsNothing(movie) Then Continue For
					pos += 1
					messbox.TextBox2.Text = pos.ToString + " of " + NfosToSave.Count.ToString
					If directortxt  .Text <> "" Then movie.ScrapedMovie.fullmoviebody.director = directortxt.Text
					If creditstxt   .Text <> "" Then movie.ScrapedMovie.fullmoviebody.credits = creditstxt.Text
					If genretxt     .Text <> "" Then movie.ScrapedMovie.fullmoviebody.genre = genretxt.Text
					If premiertxt   .Text <> "" Then movie.ScrapedMovie.fullmoviebody.premiered = premiertxt.Text
					If certtxt      .Text <> "" Then movie.ScrapedMovie.fullmoviebody.mpaa = certtxt.Text
					If outlinetxt   .Text <> "" Then movie.ScrapedMovie.fullmoviebody.outline = outlinetxt.Text
					If runtimetxt   .Text <> "" Then movie.ScrapedMovie.fullmoviebody.runtime = runtimetxt.Text
					If studiotxt    .Text <> "" Then movie.ScrapedMovie.fullmoviebody.studio = studiotxt.Text
					If countrytxt   .Text <> "" then movie.ScrapedMovie.fullmoviebody.country = countrytxt.Text
					If plottxt      .Text <> "" Then movie.ScrapedMovie.fullmoviebody.plot = plottxt.Text
					If taglinetxt   .Text <> "" Then movie.ScrapedMovie.fullmoviebody.tagline = taglinetxt.Text
					If txtStars     .Text <> "" Then movie.ScrapedMovie.fullmoviebody.stars = txtStars.Text.ToString.Replace(", See full cast and crew", "")
					If ratingtxt    .Text <> "" Then movie.ScrapedMovie.fullmoviebody.rating = ratingtxt.Text
					If votestxt     .Text <> "" Then movie.ScrapedMovie.fullmoviebody.votes = votestxt.Text
					If top250txt    .Text <> "" Then
						If lbl_movTop250.Text = "Top 250" Then
							movie.ScrapedMovie.fullmoviebody.top250 = top250txt.Text
						Else
							movie.ScrapedMovie.fullmoviebody.metascore = top250txt.Text
						End If
					End If
					If Not cbMovieDisplay_MovieSet.SelectedIndex < 1 Then
						movie.ScrapedMovie.fullmoviebody.SetName = cbMovieDisplay_MovieSet.Items(cbMovieDisplay_MovieSet.SelectedIndex)
                        Dim setid As String = oMovies.GetMovieSetIdFromName(movie.ScrapedMovie.fullmoviebody.SetName)
				        If setid <> "" AndAlso Not setid.StartsWith("L") Then movie.ScrapedMovie.fullmoviebody.TmdbSetId = setid
                        movie.ScrapedMovie.fullmoviebody.SetOverview = oMovies.GetMovieSetOverviewFromName(movie.ScrapedMovie.fullmoviebody.SetName)
					End If
					If cbUsrRated.SelectedIndex <> -1 Then movie.ScrapedMovie.fullmoviebody.usrrated = cbUsrRated.SelectedIndex.ToString
					movie.ScrapedMovie.fullmoviebody.source = If(cbMovieDisplay_Source.SelectedIndex < 1, Nothing, cbMovieDisplay_Source.Items(cbMovieDisplay_Source.SelectedIndex))
					If TabControl2.SelectedTab.Name = tpMovTags.Name Then
						For Each t In NewTagList
							Dim remtag As String = t.Replace("- ", "").Replace("+ ", "")
							If t.Contains("- ") Then
								If movie.ScrapedMovie.fullmoviebody.tag.Contains(remtag) Then movie.ScrapedMovie.fullmoviebody.tag.Remove(remtag)
							ElseIf t.Contains("+ ") Then
								If Not movie.ScrapedMovie.fullmoviebody.tag.Contains(remtag) Then movie.ScrapedMovie.fullmoviebody.tag.Add(remtag)
							End If
						Next
					Else
						If tb_tagtxt_changed Then
							movie.ScrapedMovie.fullmoviebody.tag.Clear()
							If tagtxt.Text <> "" AndAlso tagtxt.Text.Contains(",") Then
								Dim tags() As String = tagtxt.Text.Split(",")
								For Each strtag In tags
									If Not movie.ScrapedMovie.fullmoviebody.tag.Contains(strtag.Trim()) Then movie.ScrapedMovie.fullmoviebody.tag.Add(strtag.Trim())
								Next
							ElseIf tagtxt.Text <> "" Then
								If Not movie.ScrapedMovie.fullmoviebody.tag.Contains(tagtxt.Text.Trim()) Then movie.ScrapedMovie.fullmoviebody.tag.Add(tagtxt.Text.Trim())
							End If
						End If
					End If

					movie.SaveNFO
					movie.AssignMovieToCache
					movie.UpdateMovieCache

					Application.DoEvents()

					If messbox.Cancelled Then Exit For
				Next
				tb_tagtxt_changed = False
				DataGridViewMovies.ClearSelection
				UpdateFilteredList
				ProgState = ProgramState.Other
			Else
				messbox.Close()
				MsgBox("Must Select an Initial Movie" & vbCrLf & "Save Cancelled")
				Exit Sub
			End If
			messbox.Close()
			If TabControl2.SelectedTab.Name = tpMovTags.Name Then TabControl2.SelectedIndex = 0
		End If

	End Sub


	Private Sub mov_Edit()
		If outlinetxt.Text = "MC cannot find this file, either the file no longer exists, or MC cannot access the file path" Then
			MsgBox("MC cannot find this file, either the file no longer exists, or MC cannot access the file path")
			Exit Sub
		End If
		Dim oldmovietitle As String = workingMovieDetails.fileinfo.fullpathandfilename
		Dim newmovietitle As String = ""
		Dim MyFormObject As New Form2()
		MyFormObject.ShowDialog()
		newmovietitle = workingMovieDetails.fileinfo.fullpathandfilename
		For f = 0 To oMovies.MovieCache.Count - 1
			If oMovies.MovieCache(f).fullpathandfilename = newmovietitle Then
				Dim newfullmovie As New ComboList

				newfullmovie.oMovies = oMovies
				oMovies.MovieCache.RemoveAt(f)

				newfullmovie = nfoFunction.mov_NfoLoadBasic(workingMovieDetails.fileinfo.fullpathandfilename, "movielist", oMovies)
                'if there is a problem with the nfo being invalid we need to skip
				If workingMovie.title <> "ERROR" Then oMovies.MovieCache.Add(newfullmovie)
				Exit For
			End If
		Next

		'Todo: need to update all details after edit.

		For f = 0 To filteredList.Count - 1
			If filteredList(f).fullpathandfilename = oldmovietitle Then
				Dim newfullmovie2 As New ComboList

				newfullmovie2.oMovies = oMovies
				newfullmovie2 = filteredList(f)
				filteredList.RemoveAt(f)
				Dim filecreation2 As New FileInfo(workingMovieDetails.fileinfo.fullpathandfilename)
				Dim myDate2 As Date = filecreation2.LastWriteTime
				Try
					newfullmovie2.filedate = Format(myDate2, datePattern).ToString
				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
				End Try
				newfullmovie2.filename              = workingMovieDetails.fileinfo.filename
				newfullmovie2.foldername            = workingMovieDetails.fileinfo.foldername
				newfullmovie2.fullpathandfilename   = workingMovieDetails.fileinfo.fullpathandfilename
				newfullmovie2.genre                 = workingMovieDetails.fullmoviebody.genre
				newfullmovie2.id                    = workingMovieDetails.fullmoviebody.imdbid
				newfullmovie2.playcount             = workingMovieDetails.fullmoviebody.playcount
				newfullmovie2.lastplayed            = workingMovieDetails.fullmoviebody.lastplayed
				newfullmovie2.rating                = workingMovieDetails.fullmoviebody.rating.ToRating
				newfullmovie2.year                  = workingMovieDetails.fullmoviebody.year
				newfullmovie2.rootfolder            = workingMovieDetails.fileinfo.rootfolder
				filteredList.Add(newfullmovie2)
				Exit For
			End If
		Next
		Call mov_FormPopulate()
		Call Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
	End Sub

	Private Sub XBMCMCThreadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XBMCMCThreadToolStripMenuItem.Click
		Try
			Dim webAddress As String = "http://forum.xbmc.org/showthread.php?t=129134"
			OpenUrl(webAddress)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	'Private Sub ListMoviesWithoutFanartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	Try
	'		filterOverride = True
	'		Dim newlist As New List(Of ComboList)
	'		newlist.Clear()
	'		For Each movie In oMovies.MovieCache
	'			If Not File.Exists(Pref.GetFanartPath(movie.fullpathandfilename)) Then
	'				Dim movietoadd As New ComboList
	'				movietoadd.oMovies              = oMovies
	'				movietoadd.fullpathandfilename  = movie.fullpathandfilename
	'				movietoadd.filename             = movie.filename
	'				movietoadd.year                 = movie.year
	'				movietoadd.filedate             = movie.filedate
	'				movietoadd.foldername           = movie.foldername
	'				movietoadd.rating               = movie.rating
	'				movietoadd.top250               = movie.top250
	'				movietoadd.rootfolder           = movie.rootfolder
	'				newlist.Add(movietoadd)
	'			End If
	'		Next
	'		filteredList = newlist
	'		mov_CacheLoad()
	'		filterOverride = False
	'	Catch ex As Exception
	'		ExceptionHandler.LogError(ex)
	'	End Try
	'End Sub

	'Private Sub ListMoviesWithoutPostersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	Try
	'		filterOverride = True
	'		Dim newlist As New List(Of ComboList)
	'		newlist.Clear()
	'		For Each movie In oMovies.MovieCache
	'			If Not File.Exists(Pref.GetPosterPath(movie.fullpathandfilename)) Then
	'				Dim movietoadd As New ComboList
	'				movietoadd.fullpathandfilename = movie.fullpathandfilename
	'				movietoadd.filename = movie.filename
	'				movietoadd.year = movie.year
	'				movietoadd.filedate = movie.filedate
	'				movietoadd.foldername = movie.foldername
	'				movietoadd.rating = movie.rating
	'				movietoadd.top250 = movie.top250
	'				movietoadd.rootfolder = movie.rootfolder
	'				newlist.Add(movietoadd)
	'			End If
	'		Next
	'		filteredList = newlist
	'		mov_CacheLoad()
	'		filterOverride = False
	'	Catch ex As Exception
	'		ExceptionHandler.LogError(ex)
	'	End Try
	'End Sub

	Private Sub BatchRescraperToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BatchRescraperToolStripMenuItem.Click

		rescrapeList.ResetFields()

		Dim displaywizard As New frmBatchScraper
		displaywizard.ShowDialog()

		If rescrapeList.AnyEnabled Then
			_rescrapeList.FullPathAndFilenames.Clear()
			If DataGridViewMovies.SelectedRows.Count > 1 Then   'run batch wizard on multiple selected movies
				For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
					Dim fullpath As String = row.Cells("fullpathandfilename").Value.ToString
					If Not File.Exists(fullpath) Then Continue For
					If rescrapeList.EmptyMainTags AndAlso MovieHasEmptyMainTags(fullpath) Then Continue For
					_rescrapeList.FullPathAndFilenames.Add(fullpath)
				Next
			Else                                                 'Otherwise run batch wizard on all movies.
				For Each row As DataGridViewRow In DataGridViewMovies.Rows
					Dim m As Data_GridViewMovie = row.DataBoundItem
					Dim fullpath As String = m.fullpathandfilename.ToString
					If Not File.Exists(fullpath) Then Continue For
					If rescrapeList.EmptyMainTags AndAlso MovieHasEmptyMainTags(fullpath) Then Continue For
					_rescrapeList.FullPathAndFilenames.Add(fullpath)
				Next
			End If

			RunBackgroundMovieScrape("BatchRescrape")
		End If

	End Sub

	Public Function MovieHasEmptyMainTags(ByVal checkmovie As String) As Boolean
		If rescrapeList.AnyNonMainEnabled Then Return True
		For each movie As ComboList In oMovies.MovieCache
			If Not movie.fullpathandfilename = checkmovie Then Continue For
			If rescrapeList.rating      AndAlso movie.rating < 1        Then Return False
			If rescrapeList.votes       AndAlso movie.Votes = 0         Then Return False
			If rescrapeList.mpaa        AndAlso movie.Certificate = ""  Then Return False
			If rescrapeList.genre       AndAlso movie.genre = ""        Then Return False
			If rescrapeList.year        AndAlso movie.year < 1900       Then Return False
			If rescrapeList.top250      AndAlso movie.top250 = ""       Then Return False
			If rescrapeList.outline     AndAlso movie.outline = ""      Then Return False
			If rescrapeList.plot        AndAlso movie.plot = ""         Then Return False
			If rescrapeList.tagline     AndAlso movie.tagline = ""      Then Return False
			If rescrapeList.runtime     AndAlso movie.runtime = ""      Then Return False
			If rescrapeList.director    AndAlso movie.director = ""     Then Return False
			If rescrapeList.credits     AndAlso movie.credits = ""      Then Return False
			If rescrapeList.title       AndAlso movie.title = ""        Then Return False
			If rescrapeList.premiered   AndAlso movie.Premiered = ""    Then Return False
			If rescrapeList.stars       AndAlso movie.stars = ""        Then Return False
			If rescrapeList.studio      AndAlso movie.studios = ""      Then Return False
			If rescrapeList.country     AndAlso movie.countries = ""    Then Return False
			If rescrapeList.metascore   AndAlso movie.metascore = 0     Then Return False
			Exit For
		Next
		Return True
	End Function
    
	Private Sub mov_Play(ByVal type As String)
		If DataGridViewMovies.SelectedRows.Count < 1 Then Return

		Dim fullpathandfilename = CType(DataGridViewMovies.SelectedRows(0).DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString

		Dim playlist As New List(Of String)
		Select Case type
			Case "Movie"
				If fullpathandfilename.IndexOf("index.nfo") <> -1 Then
					fullpathandfilename = fullpathandfilename.Replace(".nfo", ".bdmv")
				Else
					fullpathandfilename = Utilities.GetFileName(fullpathandfilename)
				End If
				playlist = Utilities.GetMediaList(fullpathandfilename)
			Case "Trailer"
				Dim movie = oMovies.LoadMovie(fullpathandfilename)
				If movie.TrailerExists Then playlist.Add(movie.ActualTrailerPath)
			Case "HomeMovie"
				fullpathandfilename = Utilities.GetFileName(WorkingHomeMovie.fileinfo.fullpathandfilename)
				playlist = Utilities.GetMediaList(fullpathandfilename)
		End Select
		If playlist.Count <= 0 Then
			MsgBox("No Media File Found For This nfo")
			Exit Sub
		End If
		LaunchPlayList(playlist)
	End Sub

	Public Function CleanMovieTitle(ByVal s As String) As String
		Dim tmplist As New List(Of String)
		Dim q = From el In MovSepLst Order By el.Length Descending
		tmplist.AddRange(q.tolist)
		For each item In tmplist
			Dim t As String = " " & item
			If s.Contains(t) Then s = s.Replace(t, "")
		Next
		tmplist.Clear()
		Dim r = From el In ThreeDKeyWords Order By el.Length Descending
		tmplist.AddRange(r.ToList)
		For each item In tmplist
			Dim t As String = " " & item & " "
			If s.Contains(t) Then s = s.Replace(t, "")
			If s.Contains(t.TrimEnd) Then s = s.Replace(t.TrimEnd, "")
		Next
		Return s
	End Function

	Public Sub LaunchPlayList(ByVal plist As List(Of String))
		Statusstrip_Enable()
		ToolStripStatusLabel2.Text = ""
		ToolStripStatusLabel2.Visible = True
		Dim tempstring = applicationPath & "\Settings\temp.m3u"
		ToolStripStatusLabel2.Text = "Playing Movie...Creating m3u file:..." & tempstring
		Application.DoEvents()
		Dim aok As Boolean = True
		If File.Exists(tempstring) Then aok = Utilities.SafeDeleteFile(tempstring)
		If aok Then
            If plist.Count > 1 Then
                Dim setencoding As Encoding = If((Not String.IsNullOrEmpty(Pref.selectedvideoplayer) AndAlso Pref.videomode > 2), Encoding.Utf8, Encoding.getencoding(1252))
                Dim file As New IO.StreamWriter(tempstring, False, setencoding)
			    For Each part In plist
				    If part <> Nothing Then file.WriteLine(part)
			    Next
			    file.Close()
            Else
                tempstring = plist(0)
            End If
			ToolStripStatusLabel2.Text &= "............Launching Player."
			StartVideo(tempstring)
		Else
			ToolStripStatusLabel2.Text = "Failed to create playlist"
		End If
		statusstripclear.Start()
	End Sub

	Private Sub MovieFormInit()
		workingMovie.oMovies = oMovies
		workingMovie.filedate = Nothing
		workingMovie.filename = Nothing
		workingMovie.foldername = Nothing
		workingMovie.fullpathandfilename = Nothing
		workingMovie.genre = Nothing
		workingMovie.id = Nothing
		workingMovie.playcount = Nothing
		workingMovie.lastplayed = Nothing
		workingMovie.rating = Nothing
		workingMovie.title = Nothing
		workingMovie.top250 = Nothing
		workingMovie.year = Nothing
		workingMovie.rootfolder = Nothing
		titletxt.Text = ""
		tb_MovFanartMovieTitle.Text = ""
		outlinetxt.Text = ""
		tb_MovSortOrder.Text = ""
		plottxt.Text = ""
		taglinetxt.Text = ""
		txtStars.Text = ""
		genretxt.Text = ""
		premiertxt.Text = ""
		creditstxt.Text = ""
		directortxt.Text = ""
		studiotxt.Text = ""
		countrytxt.Text = ""
		pathtxt.Text = ""
		cbMovieDisplay_Actor.Items.Clear()
		ratingtxt.Text = ""
		runtimetxt.Text = ""
		votestxt.Text = ""
		top250txt.Text = ""
		certtxt.Text = ""
        tagtxt.Text = ""
		PbMovieFanArt.Image = Nothing
		PbMoviePoster.Image = Nothing
		roletxt.Text = ""
		PictureBoxActor.Image = Nothing
		btnPlayMovie.Enabled = False
        tb_tagtxt_changed = False
	End Sub

	Public Sub genretxt_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles genretxt.MouseDown
		If e.Button = Windows.Forms.MouseButtons.Right Then
			Dim multicount As Integer = 0
			Try
				Dim genre As String = ""
				Dim listof As New List(Of str_genre)
				Dim Multi As Boolean = DataGridViewMovies.SelectedRows.Count > 1
				If Not Multi Then
					Dim item As List(Of String) = workingMovieDetails.fullmoviebody.genre.Split("/").[Select](Function(p) p.Trim()).ToList
					If item(0) = "" Then item.RemoveAt(0)
					multicount = multicount + 1
					listof.Clear()
					For Each i In item
						Dim g As New str_genre
						g.genre = i.Trim
						g.count = 1
						listof.Add(g)
					Next
				Else
					For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
						multicount = multicount + 1
						Dim genretxt As String = row.Cells("genre").Value.ToString
						Dim spltgenre As List(Of String) = genretxt.Split("/").[Select](Function(p) p.Trim()).ToList
						If spltgenre(0) = "" Then spltgenre.RemoveAt(0)
						For Each i In spltgenre
							Dim g As New str_genre
							g.genre = i
							g.count = 1
							For each item In listof
                                If (item.genre.Equals(g.genre, StringComparison.InvariantCultureIgnoreCase)) Then
									g.count = item.count
									Exit For
								End If
							Next
							If listof.Contains(g) Then
								listof.RemoveAt(listof.IndexOf(g))
								g.count = g.count + 1
							End If
							listof.Add(g)
						Next
					Next
				End If
				Dim frm As New frmGenreSelect
				frm.multicount = multicount
				frm.SelectedGenres = listof
				frm.Init()
				If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
					listof.Clear()
					listof.AddRange(frm.SelectedGenres)
					If Not Multi Then
						genre = ""
						For each g In listof
							If g.count = 0 Then Continue For
							If genre = "" Then
								genre = g.genre
							Else
								genre += " / " & g.genre
							End If
						Next
						genretxt.Text = genre
						Call mov_SaveQuick()
					Else
						Dim NfosToSave As List(Of String) = (From x As datagridviewrow In DataGridViewMovies.SelectedRows Select nfo = x.Cells("fullpathandfilename").Value.ToString).ToList
						For Each nfo As String In NfosToSave
							If Not File.Exists(nfo) Then Continue For
							Dim movie As Movie = oMovies.LoadMovie(nfo)
							If IsNothing(movie) Then Continue For
							Dim genretxt As String = movie.ScrapedMovie.fullmoviebody.genre
							Dim spgenre2 As List(Of String) = genretxt.Split("/").[Select](Function(p) p.Trim()).ToList
							genre = ""
							For Each g In listof
								Dim ToAdd As Boolean = False
								If g.count = 0 Then Continue For
								If (g.count = 1 AndAlso spgenre2.Contains(g.genre)) OrElse g.count = 2 Then ToAdd = True
								If Not ToAdd Then Continue For
								If genre = "" Then
									genre = g.genre
								Else
									genre += " / " & g.genre
								End If
							Next
							movie.ScrapedMovie.fullmoviebody.genre = genre
							movie.AssignMovieToCache()
							movie.UpdateMovieCache()
							movie.SaveNFO()
							Application.DoEvents()
						Next
					End If
				End If
				frm.Dispose()
			Catch
			End Try
		End If
	End Sub

	Public Sub DisplayMovie(Optional yielding As Boolean = False)
		Try
			DisplayMovie(DataGridViewMovies.SelectedCells, DataGridViewMovies.SelectedRows, yielding)
		Catch
			Return
		End Try
	End Sub
    
	Public Sub DisplayMovie(ByVal selectedCells As DataGridViewSelectedCellCollection, ByVal selectedRows As DataGridViewSelectedRowCollection, yielding As Boolean)

		Try
			If selectedRows.Count = 1 Then
				Dim currNfo = CType(selectedRows(0).DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString
                
                If Not MovieRefresh AndAlso lastNfo = currNfo Then Return
                MovieRefresh = False
                lastNfo = currNfo
			Else
				lastNfo = ""
			End If
		Catch
			Return
		End Try

		If yielding Then
			_yield = True
			Application.DoEvents()
			_yield = False
		End If

		'Clear all fields of the movie
		MovieFormInit()

		If selectedRows.Count = 0 Then Exit Sub

		Dim needtoload As Boolean = False
		Dim done As Boolean = False


		Dim blnShow = (selectedRows.Count = 1)

		tsmiMov_PlayMovie.Visible = blnShow
		tsmiMov_OpenFolder.Visible = blnShow
		tsmiMov_ViewNfo.Visible = blnShow
		tsmiMov_Separator1.Visible = blnShow
		tsmiMov_Separator7.Visible = blnShow
		tsmiMov_FanartBrowserAlt.Visible = blnShow
		tsmiMov_PosterBrowserAlt.Visible = blnShow
		tsmiMov_EditMovieAlt.Visible = blnShow
		tsmiMov_ReloadFromCache.Visible = blnShow

		If selectedRows.Count = 1 Then
			If titletxt.Visible = False Then
				needtoload = True
			End If
			titletxt.Visible = True
			TextBoxMutisave.Visible = False
			btnMovRescrape.Visible = True
			SpltCntr_MovPictures.Visible = True
			Label128.Visible = False
			Label75.Visible = True
			tb_MovSortOrder.Visible = True

			Dim query = From f In oMovies.Data_GridViewMovieCache Where f.fullpathandfilename = CType(selectedRows(0).DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString

			Dim queryList As List(Of Data_GridViewMovie) = query.ToList()

			If Yield(yielding) Then Return

			If Not queryList.Count = 0 AndAlso Not File.Exists(queryList(0).MoviePathAndFileName) Then   'Detect if video file is missing
				If Mov_MissingMovie(queryList) Then Exit Sub
			End If

			If queryList.Count > 0 Then

				workingMovie.FieldsLockEnabled = False

				workingMovie.oMovies = oMovies
				workingMovie.filedate = queryList(0).filedate
				workingMovie.filename = queryList(0).filename
				workingMovie.foldername = queryList(0).foldername
				workingMovie.fullpathandfilename = queryList(0).fullpathandfilename
				workingMovie.genre = queryList(0).genre
				workingMovie.id = queryList(0).id
				workingMovie.playcount = queryList(0).playcount
				workingMovie.rating = queryList(0).Rating
				workingMovie.title = queryList(0).title
				workingMovie.top250 = queryList(0).top250
				workingMovie.year = queryList(0).year
				workingMovie.FolderSize = queryList(0).FolderSize
                workingMovie.MediaFileSize = queryList(0).MediaFileSize
				workingMovie.rootfolder = queryList(0).rootfolder
				workingMovie.SetName = queryList(0).SetName
				workingMovie.TmdbSetId = queryList(0).TmdbSetId
				workingMovie.tmdbid = queryList(0).tmdbid
                workingMovie.NumVideoBits = queryList(0).NumVideoBits

				workingMovie.FieldsLockEnabled = True

				tsmiMov_PlayTrailer.Visible = Not queryList(0).MissingTrailer

				tsmiMov_ViewMovieDbSetPage.Enabled = workingMovie.GotTmdbSetId
				tsmiMov_ViewMovieDbMoviePage.Enabled = workingMovie.GotTmdbId

				Call mov_FormPopulate(yielding)
			Else
				If needtoload = True Then Call mov_FormPopulate(yielding)
			End If
			done = True
		Else
			outlinetxt.Text = ""
			PbMovieFanArt.Image = Nothing
			PbMoviePoster.Image = Nothing
			roletxt.Text = ""
			PictureBoxActor.Image = Nothing
			SpltCntr_MovPictures.Visible = False
			titletxt.Visible = False
			Label75.Visible = False
			tb_MovSortOrder.Visible = False
			TextBoxMutisave.Visible = True
			Panel6.Visible = False
			FanTvArtList.Items.Clear()
			btnMovRescrape.Visible = False
			Label128.Visible = True
			cbMovieDisplay_MovieSet.Items.Insert(0, "")
			cbMovieDisplay_MovieSet.SelectedIndex = 0
			cbUsrRated.SelectedIndex = -1
			Dim add As Boolean = True
			Dim watched As String = ""
            
			For Each sRow As DataGridViewRow In selectedRows
				Dim old As String = watched
				For Each item In oMovies.MovieCache
					If item.fullpathandfilename = CType(sRow.DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString Then
						If watched = "" Then
							watched = item.playcount
							old = watched
						Else
							watched = item.playcount
						End If
						If watched <> old Then
							add = False
						End If
						Exit For
					End If

					If Yield(yielding) Then Return
				Next
			Next
			If add = False Then
				btnMovWatched.Text = ""
				btnMovWatched.BackColor = Color.Gray
			Else
				If watched = "1" Then
					btnMovWatched.Text = "&Watched"
					btnMovWatched.BackColor = Color.LawnGreen
					btnMovWatched.Refresh()
				Else
					btnMovWatched.Text = "Un&watched"
					btnMovWatched.BackColor = Color.Red
					btnMovWatched.Refresh()
				End If
			End If
		End If
	End Sub

#Region "DataGridViewMovies Events"
	Private Sub DataGridViewMovies_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewMovies.CellClick
		_yield = True
		Application.DoEvents()
		_yield = False
		DisplayMovie()
	End Sub

	Private Sub DataGridViewMovies_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridViewMovies.ColumnHeaderMouseClick
		Dim col_index = e.ColumnIndex
		DGVMoviesColName = DataGridViewMovies.Columns(col_index).Name
		btnreverse.Checked = Not btnreverse.Checked
		btnreverse_CheckedChanged(Nothing, Nothing)
	End Sub

	Private Sub DataGridViewMovies_DoubleClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles DataGridViewMovies.DoubleClick
		Try
			Dim info = DataGridViewMovies.HitTest(e.X, e.Y)
			If info.ColumnX = -1 Then Return

			If info.Type <> DataGridViewHitTestType.ColumnHeader Then mov_Play("Movie")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DataGridViewMovies_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridViewMovies.DragDrop

		Dim files() As String

		files = e.Data.GetData(DataFormats.FileDrop)
		For f = 0 To UBound(files)
			If File.Exists(files(f)) Then
				' This path is a file.
				Dim skip As Boolean = False
				For Each item In oMovies.MovieCache
					If item.fullpathandfilename = files(f) Then
						skip = True
						Exit For
					End If
				Next
				For Each item In droppedItems
					If item = files(f) Then
						skip = True
						Exit For
					End If
				Next
				If mov_FileCheckValid(files(f)) = True Then
					If skip = False Then droppedItems.Add(files(f))
				End If
			Else
				If Directory.Exists(files(f)) Then
					' This path is a directory.
					Dim di As New DirectoryInfo(files(f))
					Dim diar1 As FileInfo() = di.GetFiles()
					Dim dra As FileInfo

					'list the names of all files in the specified directory
					For Each dra In diar1
						Dim skip As Boolean = False
						For Each item In oMovies.MovieCache
							If item.fullpathandfilename = dra.FullName Then
								skip = True
								Exit For
							End If
						Next
						For Each item In droppedItems
							If item = dra.FullName Then
								skip = True
								Exit For
							End If
						Next
						If mov_FileCheckValid(dra.FullName) = True Then
							If skip = False Then droppedItems.Add(dra.FullName)
						End If
					Next
				End If
			End If
		Next

		If droppedItems.Count > 0 Then DoScrapeDroppedFiles()

	End Sub

	Private Sub DataGridViewMovies_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridViewMovies.DragEnter
		Try
			e.Effect = DragDropEffects.Copy
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DataGridViewMovies_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridViewMovies.KeyUp
		DisplayMovie(True)
	End Sub

	Private Sub DataGridViewMovies_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridViewMovies.Keypress
		If [Char].IsLetter(e.KeyChar) or [Char].IsDigit(e.KeyChar) Then
			Dim ekey As String = e.KeyChar.ToString.ToLower
			keypresstimer.Stop()
			MovieKeyPress &= ekey
			keypresstimer.Start()
			For i As Integer = 0 To (DataGridViewMovies.Rows.Count) - 1
				Dim rtitle As String = DataGridViewMovies.Rows(i).Cells("DisplayTitle").Value.ToString.ToLower
				If rtitle.StartsWith(MovieKeyPress) Then
					Dim icell As Integer = DataGridViewMovies.CurrentCell.ColumnIndex
					DataGridViewMovies.CurrentCell = DataGridViewMovies.Rows(i).Cells(icell)
					DisplayMovie()
					Return
				End If
			Next
		End If
	End Sub

	Private Sub DataGridViewMovies_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridViewMovies.MouseLeave
		TooltipGridViewMovies1  .Visible = False
		TimerToolTip            .Enabled = False
	End Sub

	Private Sub DataGridViewMovies_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridViewMovies.MouseHover
		TimerToolTip.Start()
	End Sub

	Private Sub DataGridViewMovies_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGridViewMovies.MouseMove
		Try
			Dim MousePos As Point = DataGridViewMovies.PointToClient(Control.MousePosition)
			Dim objHitTestInfo As DataGridView.HitTestInfo = DataGridViewMovies.HitTest(MousePos.X, MousePos.Y)
			Dim MouseRowIndex As Integer = objHitTestInfo.RowIndex

			TimerToolTip.Enabled = True

			If MouseRowIndex > -1 Then
				Dim Runtime As String = ""
				Dim RatingRuntime As String = ""
				Dim movietitle As String = DataGridViewMovies.Rows(MouseRowIndex).Cells("Title").Value.ToString
				Dim movieYear As String = DataGridViewMovies.Rows(MouseRowIndex).Cells("Year").Value.ToString
				Dim Rating As String = "Rating: " & DataGridViewMovies.Rows(MouseRowIndex).Cells("Rating").Value.ToString.FormatRating

				If DataGridViewMovies.Rows(MouseRowIndex).Cells("Runtime").Value.ToString.Length > 3 Then
					Runtime = "Runtime: " & DataGridViewMovies.Rows(MouseRowIndex).Cells("IntRuntime").Value.ToString
				End If

				RatingRuntime = Rating & "     " & Runtime

				Dim Plot As String = DataGridViewMovies.Rows(MouseRowIndex).Cells("Plot").Value.ToString

				If objHitTestInfo.RowY > -1 Then
					TooltipGridViewMovies1.Visible = Pref.ShowMovieGridToolTip

					TooltipGridViewMovies1.Textinfo(Plot)
					TooltipGridViewMovies1.TextLabelMovieYear(movieYear)
					TooltipGridViewMovies1.TextMovieName(movietitle)
					TooltipGridViewMovies1.TextLabelRatingRuntime(RatingRuntime)

					TooltipGridViewMovies1.Left = MousePos.X + 10
					TooltipGridViewMovies1.Top = MousePos.Y + TooltipGridViewMovies1.Height + 30
				End If
			End If
		Catch
		End Try
	End Sub

	Private Sub DataGridViewMovies_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGridViewMovies.MouseUp
		tsmiMov_PlayTrailer.Visible = True
		tsmiMov_ViewMovieDbSetPage.Enabled = False
		tsmiMov_ViewMovieDbMoviePage.Enabled = False

		If e.Button = MouseButtons.Right Then
			Dim multiselect As Boolean = False
			'If more than one movie is selected, check if right-click is on the selection.
			If DataGridViewMovies.SelectedRows.Count > 1 Then multiselect = True
			'Otherwise, bring up the context menu for a single movie

			If multiselect Then
				tsmiMov_MovieName.BackColor = Color.Orange
				tsmiMov_MovieName.Text = "Multisave Mode"
				tsmiMov_MovieName.Font = New Font("Arial", 10, FontStyle.Bold)
				tsmiMov_PlayTrailer.Visible = False    'multisave mode the "Play Trailer' is always hidden
			Else

				Try
					'update context menu with movie name & also if we show the 'Play Trailer' menu item

					Dim movie = CType(DataGridViewMovies.selectedRows(0).DataBoundItem, Data_GridViewMovie)

					tsmiMov_MovieName.BackColor = Color.Honeydew
					tsmiMov_MovieName.Text = "'" & movie.DisplayTitleAndYear.ToString & "'"
					tsmiMov_MovieName.Font = New Font("Arial", 10, FontStyle.Bold)
					tsmiMov_PlayTrailer.Visible = Not movie.MissingTrailer

					tsmiMov_ViewMovieDbSetPage.Enabled = workingMovie.GotTmdbSetId
					tsmiMov_ViewMovieDbMoviePage.Enabled = workingMovie.GotTmdbId
				Catch
				End Try

			End If
		End If
	End Sub

#End Region ' DataGridViewMovies  Events

	Private Function Mov_MissingMovie(ByVal qrylst As List(Of Data_GridViewMovie)) As Boolean
		Dim missingstr As String
		Dim Filepathandname As String
		Filepathandname = qrylst(0).fullpathandfilename.Replace(".nfo", "")
		missingstr = "Video file Missing" & vbCrLf & Filepathandname & vbCrLf & "Do you wish to remove from database?"
		If MsgBox(missingstr, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
			Mov_RemoveMovie()
			Return True
		End If
		Return False
	End Function

	Private Sub TextBox1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyUp
		Try
			If filterOverride = False Then
				If TextBox1.Text.Length > 0 Then
					TextBox1.BackColor = Color.Pink
				Else
					TextBox1.BackColor = Color.White
				End If
				TextBox1.Refresh()
				Call Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub TabControl2_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabControl2.MouseWheel
		Try
			If TabControl2.SelectedIndex = 1 Then
				mouseDelta = e.Delta / 120
				Try
					tpMovWall.AutoScrollPosition = New Point(0, tpMovWall.VerticalScroll.Value - (mouseDelta * 30))
				Catch ex As Exception
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub TabControl2_Selecting(ByVal sender As Object, ByVal e As CancelEventArgs) Handles TabControl2.Selecting
		Dim tab As String = TabControl2.SelectedTab.Text
		If tab.ToLower = "movie preferences" Then
			e.Cancel = True
			OpenPreferences(1)
		End If
	End Sub

	Private Sub TabControl2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl2.SelectedIndexChanged

		mov_PreferencesDisplay()
		Dim tempstring As String = ""
		Dim tab As String = TabControl2.SelectedTab.Name
		If tab <> tpMovMain.Name And tab <> tpMovFolders.Name And tab <> tpMovStubs.Name Then
			If workingMovieDetails Is Nothing And movieFolders.Count = 0 And Pref.offlinefolders.Count = 0 Then
				Me.TabControl2.SelectedIndex = currentTabIndex
				MsgBox("There are no movies in your list to work on" & vbCrLf & "Add movie folders in the Folders Tab" & vbCrLf & "Then select the ""Search For New Movies"" Tab")
				Exit Sub
			ElseIf workingMovieDetails Is Nothing And movieFolders.Count > 0 Then
				Me.TabControl2.SelectedIndex = currentTabIndex
				If oMovies.MovieCache.Count > 0 Then
					MsgBox("No Movie is selected")
					Exit Sub
				Else
					MsgBox("There are no movies in your list to work on" & vbCrLf & "If The Folders You Have Added In The" & vbCrLf & "Preferences Menu Contain Movie Files" & vbCrLf & "Then select the ""Search For New Movies"" Tab")
					Exit Sub
				End If
			End If
		Else
			currentTabIndex = Me.TabControl2.SelectedIndex
		End If
		If tab = tpMovWebBrowser.Name Then     'Movie WebBrowser tab.
			If workingMovieDetails.fullmoviebody.imdbid <> Nothing OrElse workingMovieDetails.fullmoviebody.tmdbid <> "" Then
				Dim weburl As String = ""
				Dim TMDB As String = workingMovieDetails.fullmoviebody.tmdbid
				Dim IMDB As String = workingMovieDetails.fullmoviebody.imdbid
				Dim TMDBLan As List(Of String) = Utilities.GetTmdbLanguage(Pref.TMDbSelectedLanguageName)
				If (IMDB <> "" And IMDB <> "0") AndAlso TMDB <> "" Then
					Dim tpi As Integer = tpMovWebBrowser.ImageIndex
					If tpi = 1 Then
						weburl = "https://www.themoviedb.org/movie/" & TMDB & "?language=" & TMDBLan(0) 'de"
					Else
						weburl = "http://www.imdb.com/title/" & IMDB & "/"
					End If
				ElseIf (IMDB <> "" And IMDB <> "0") AndAlso TMDB = "" Then
					weburl = "http://www.imdb.com/title/" & IMDB & "/"
				ElseIf IMDB = "0" AndAlso TMDB <> "" Then
					weburl = "https://www.themoviedb.org/movie/" & TMDB & "?language=" & TMDBLan(0) 'de"
				End If

				If Pref.externalbrowser = True Then
					Me.TabControl2.SelectedIndex = currentTabIndex
					OpenUrl(weburl)
				Else
					Try
						currentTabIndex = TabControl2.SelectedIndex
						WebBrowser2.Hide()
						WebBrowser2.Navigate("about:blank")
						Do Until WebBrowser2.ReadyState = WebBrowserReadyState.Complete
							Application.DoEvents()
							System.Threading.Thread.Sleep(100)
						Loop
						WebBrowser2.Show()
						If IsNothing(WebBrowser2.Url) OrElse WebBrowser2.Url.AbsoluteUri.ToLower.ToString <> weburl Then
							WebBrowser2.Stop()
							WebBrowser2.ScriptErrorsSuppressed = True
							WebBrowser2.Navigate(weburl)
							currentTabIndex = TabControl2.SelectedIndex
						End If
					Catch
						WebBrowser2.Stop()
						WebBrowser2.ScriptErrorsSuppressed = True
						WebBrowser2.Navigate(weburl)
					End Try
					WebBrowser2.Focus()
				End If
			Else
				MsgBox("No IMDB or TMDB ID is available for this movie")
			End If
		ElseIf tab = tpMovMain.Name Then      'Movie Main Browser tab
			currentTabIndex = TabControl2.SelectedIndex
			UpdateFilteredList()
		ElseIf tab = tpMovFileDetails.Name Then         'Movie File Details tab
			currentTabIndex = TabControl2.SelectedIndex
			If TextBox8.Text = "" Then Call util_FileDetailsGet()
		ElseIf tab = tpMovFanart.Name Then       'Movie Fanart tab
			Dim isrootfolder As Boolean = False
			If Pref.movrootfoldercheck Then
				For Each moviefolder In movieFolders
					Dim movfolder As String = workingMovieDetails.fileinfo.fullpathandfilename.Replace("\" & workingMovieDetails.fileinfo.filename, "")
					If moviefolder.rpath = movfolder Then
						isrootfolder = True 'Check movie isn't in a rootfolder, if so, disable extrathumbs option from displaying
						Exit For
					End If
				Next
			End If
			If String.IsNullOrEmpty(tb_MovFanartScrnShtTime.Text) Then tb_MovFanartScrnShtTime.Text = Pref.ScrShtDelay.ToString
			GroupBoxFanartExtrathumbs.Enabled = Not isrootfolder 'Or usefoldernames Or allfolders ' Visible 'hide or show fanart/extrathumbs depending of if we are using foldenames or not (extrathumbs needs foldernames to be used)
			UpdateMissingFanartNav()
			If Panel2.Controls.Count = 0 Then
				btn_MovFanartScrnSht.Visible = False
				tb_MovFanartScrnShtTime.Visible = False
				Label2.Visible = False
				Call mov_FanartLoad()
			End If
			If Panel2.Controls.Count = 1 AndAlso Panel2.Controls().Item(0).GetType Is GetType(Label) Then
				btn_MovFanartScrnSht.Visible = True
				tb_MovFanartScrnShtTime.Visible = True
				Label2.Visible = True
			End If
			currentTabIndex = TabControl2.SelectedIndex
			If Panel2.Controls.Count > 1 Then EnableFanartScrolling()
		ElseIf tab = tpMovPoster.Name Then   'Poster Tab
			currentTabIndex = TabControl2.SelectedIndex
			gbMoviePostersAvailable.Refresh()
			btnMovPosterToggle.Visible = workingMovieDetails.fullmoviebody.TmdbSetId <> ""
			UpdateMissingPosterNav()
			If Pref.MovPosterTabTMDBSelect Then btn_TMDb_posters.PerformClick()
		ElseIf tab = tpMovChange.Name Then         'Change Movie
			Call mov_ChangeMovieSetup(MovieSearchEngine)
			currentTabIndex = TabControl2.SelectedIndex
		ElseIf tab = tpMovWall.Name Then        'Wall Tab
			Call mov_WallSetup()
		ElseIf tab = tpMovSets.Name Then         'Movie Sets tab
			Call MovieSetstabSetup()
		ElseIf tab = tpMovTags.Name Then         'Movie Tags tab
			Call MovieTagsSetup()
		ElseIf tab = tpMovFanartTv.Name Then       'Fanart.Tv tab
			UcFanartTv1.ucFanartTv_Refresh(workingMovieDetails)
		ElseIf tab = tpMovTable.Name Then    'Table tab
			currentTabIndex = TabControl2.SelectedIndex
			Call mov_TableSetup()
		Else
			currentTabIndex = TabControl2.SelectedIndex
		End If
	End Sub

	Private Sub TabControl2_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl2.Selecting
		Try
			If Not IsNothing(messbox) AndAlso messbox.visible Then e.Cancel = True

			Dim selOption = cbFilterGeneral.Text.RemoveAfterMatch

			If selOption.IndexOf("Missing from set") = 0 Then e.Cancel = True
		Catch
		End Try
	End Sub

	Private Sub TabControl2_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TabControl2.MouseClick
		If e.Button = Windows.Forms.MouseButtons.Right Then
			For index As Integer = 0 To TabControl2.TabCount - 1 Step 1
				If TabControl2.GetTabRect(index).Contains(e.Location) Then
					If index = 9 Then
						Dim tpi As Integer = tpMovWebBrowser.ImageIndex
						If tpi = 0 Then
							tpMovWebBrowser.ImageIndex = 1
						Else
							tpMovWebBrowser.ImageIndex = 0
						End If
						tpMovWebBrowser.Refresh()
					End If
					Exit For
				End If
			Next index
		End If
	End Sub

	Private Sub mov_ChangeMovieSetup(ByVal engine As String)
		Dim tempstring As String = ""
		Dim isroot As Boolean = Pref.GetRootFolderCheck(workingMovieDetails.fileinfo.fullpathandfilename)
		If Pref.usefoldernames = False OrElse isroot Then
			tempstring = Utilities.RemoveFilenameExtension(Path.GetFileName(workingMovieDetails.fileinfo.fullpathandfilename))
		Else
			tempstring = Utilities.GetLastFolder(workingMovieDetails.fileinfo.fullpathandfilename)
		End If
		If workingMovieDetails.fileinfo.fullpathandfilename.ToLower.IndexOf("\video_ts\") <> -1 Then
			tempstring = Utilities.GetLastFolder(workingMovieDetails.fileinfo.fullpathandfilename)
		End If
		Dim url As String
		If engine = "imdb" Then
			url = Pref.imdbmirror & "find?s=tt&q=" & Utilities.CleanFileName(tempstring)
		Else
			url = "http://www.themoviedb.org/search?query=" & Utilities.CleanFileName(tempstring)
		End If
		Dim uri As New Uri(url)
		WebBrowser1.Stop()
		WebBrowser1.ScriptErrorsSuppressed = True
		WebBrowser1.Navigate(uri.AbsoluteUri)
		WebBrowser1.Refresh()
		Panel2.Visible = True
	End Sub


	Public Sub util_OpenFolder(ByVal path As String)
		Dim tempstring As String = ""
		Dim action As String = String.Empty
		Dim errors As String = String.Empty
		Try
			If path <> "" Then
				tempstring = path
				Try
					If Not tempstring.ToLower.Contains("nfo") Then
						errors = "Trying to open Folder :- " & tempstring
						action = "Command - ""Call Shell(""explorer """ & tempstring & ", AppWinStyle.NormalFocus)"""
						Call Shell("explorer " & """" & tempstring & """", AppWinStyle.NormalFocus) 'opens the selected folder showing contents, no focus on any file.
					Else
						errors = "Trying to open Folder :- " & tempstring
						action = "Command - ""Call Shell(""explorer /select,""" & tempstring & ", AppWinStyle.NormalFocus)"""
						Call Shell("explorer /select," & """" & tempstring & """", AppWinStyle.NormalFocus) 'this shows the item as selected provided as tempstring i.e. a folder or a file (.nfo)
					End If
				Catch ex As Exception
					MsgBox("Can't open folder :- " & path)
					Call util_ErrorLog(action, errors)
				End Try
			Else
				MsgBox("No Folder To Open")
			End If
		Catch
			MsgBox("No Folder To Open")
		End Try
	End Sub

	Private Sub mov_FanartLoad()
		rbMovFanart.Checked = True
		MovFanartToggle = False
		btnMovFanartToggle.Visible = workingMovieDetails.fullmoviebody.TmdbSetId <> ""
		Dim isfanartpath As String = workingMovieDetails.fileinfo.fanartpath
		Dim isvideotspath As String = If(workingMovieDetails.fileinfo.videotspath = "", "", workingMovieDetails.fileinfo.videotspath + "fanart.jpg")
		Dim movfanartpath As String = Utilities.DefaultFanartPath
		If isfanartpath <> Nothing Or isvideotspath <> "" Then
			Try
				If File.Exists(isvideotspath) Then
					movfanartpath = isvideotspath
				ElseIf File.Exists(isfanartpath) Then
					movfanartpath = isfanartpath
				End If
				util_ImageLoad(PictureBox2, movfanartpath, Utilities.DefaultFanartPath)
				If movfanartpath = "" Then
					lblMovFanartWidth.Text = ""
					lblMovFanartHeight.Text = ""
				Else
					lblMovFanartWidth.Text = PictureBox2.Image.Width
					lblMovFanartHeight.Text = PictureBox2.Image.Height
				End If
			Catch ex As Exception
			End Try
		End If
		MovFanartDisplay()
	End Sub
    
	Public Sub MovFanartDisplay(Optional ByVal TmdbSetId As String = "")

		Me.Refresh()
		fanartArray.Clear()
		messbox = New frmMessageBox("", "Retrieving Fanart...", "Please wait...")
		messbox.Show()
		messbox.Refresh()
		Dim tmdb As New TMDb
		If TmdbSetId = "" Then
			tmdb.Imdb = If(workingMovieDetails.fullmoviebody.imdbid.Contains("tt"), workingMovieDetails.fullmoviebody.imdbid, "")
			tmdb.TmdbId = workingMovieDetails.fullmoviebody.tmdbid
			Try
				fanartArray.AddRange(tmdb.McFanart)
			Catch
			End Try
		Else
			tmdb.SetId = TmdbSetId
			Try
				fanartArray.AddRange(tmdb.McSetFanart)
			Catch
			End Try
		End If
		messbox.TextBox2.Text = "Setting up display...."
		messbox.Refresh()
		Try
			If fanartArray.Count > 0 Then
				Dim MovFanartPicBox As New List(Of FanartPicBox)
				Dim location As Integer = 0
				Dim itemcounter As Integer = 0
				For Each item In fanartArray
					Dim thispicbox As New FanartPicBox
					fanartBoxes() = New PictureBox()
					With fanartBoxes
						.Location = New Point(0, location)
						If fanartArray.Count > 2 Then
							.Width = 410
							.Height = 233
						Else
							.Width = 424
							.Height = 243
						End If
						.SizeMode = PictureBoxSizeMode.Zoom
						.Visible = True
						.BorderStyle = BorderStyle.Fixed3D
						.Name = "moviefanart" & itemcounter.ToString
						AddHandler fanartBoxes.DoubleClick, AddressOf util_ZoomImage2
					End With
					thispicbox.pbox = fanartBoxes
					thispicbox.imagepath = item.ldurl
					MovFanartPicBox.Add(thispicbox)
					Application.DoEvents()
					If fanartArray.Count > 2 Then
						fanartCheckBoxes() = New RadioButton()
						With fanartCheckBoxes
							.BringToFront()
							.Location = New Point(199, location + 229)
							.Name = "moviefanartcheckbox" & itemcounter.ToString
						End With
						resLabels = New Label()
						With resLabels
							.BringToFront()
							.Location = New Point(0, location + 235)
							.Name = "label" & itemcounter.ToString
							.Text = "(" & item.hdwidth & " x " & item.hdheight & ") (" & item.ldwidth & " x " & item.ldheight & ")"
							.Width = 200
						End With
						votelabels = New Label()
						With votelabels
							.BringToFront()
							.Location = New Point(300, location + 235)
							.Name = "votelabel" & itemcounter.ToString
							.Text = "Votes:  " & item.votes.ToString
						End With
						itemcounter += 1
						location += 260
					Else
						fanartCheckBoxes() = New RadioButton()
						With fanartCheckBoxes
							.BringToFront()
							.Location = New Point(199, location + 243)
							.Name = "moviefanartcheckbox" & itemcounter.ToString
							.Text = ""
						End With
						resLabels = New Label()
						With resLabels
							.BringToFront()
							.Location = New Point(0, location + 249)
							.Name = "label" & itemcounter.ToString
							.Text = "(" & item.hdwidth & " x " & item.hdheight & ") (" & item.ldwidth & " x " & item.ldheight & ")"
							.Width = 200
						End With
						votelabels = New Label()
						With votelabels
							.BringToFront()
							.Location = New Point(300, location + 249)
							.Name = "votelabel" & itemcounter.ToString
							.Text = "Votes:  " & item.votes.ToString
							.Width = 200
						End With
						itemcounter += 1
						location += 275
					End If
					Me.Panel2.Controls.Add(fanartBoxes())
					Me.Panel2.Controls.Add(fanartCheckBoxes())
					Me.Panel2.Controls.Add(resLabels)
					Me.Panel2.Controls.Add(votelabels)
					Application.DoEvents()
				Next
				Me.Panel2.Refresh()
				Me.Refresh()
				If MovFanartPicBox.Count > 0 Then
					messbox.Close()
					If Not ImgBw.IsBusy Then
						ToolStripStatusLabel2.Text = "Starting Download of Images..."
						ToolStripStatusLabel2.Visible = True
						ImgBw.RunWorkerAsync({MovFanartPicBox, 0, MovFanartPicBox.Count, Me.Panel2})
					End If
				End If
				EnableFanartScrolling()
				Me.Panel2.Refresh()
				Me.Refresh
			Else
				Dim mainlabel2 As Label
				mainlabel2 = New Label
				With mainlabel2
					.Location = New Point(0, 100)
					.AutoSize = False
					.TextAlign = ContentAlignment.MiddleCenter
					.Width = 410
					.Height = 400
					.Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
					.Text = "No Fanart Was Found At" & Environment.NewLine & "www.themoviedb.org For This Movie"
				End With

				Me.Panel2.Controls.Add(mainlabel2)
			End If
			messbox.Close()
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			If Not IsNothing(messbox) Then messbox.Close()
		End Try
	End Sub

	Sub EnableFanartScrolling()
		Try
			Panel2.Focus()
		Catch
		End Try
	End Sub

	Private Sub util_ZoomImage2(ByVal sender As Object, ByVal e As EventArgs)

		Dim tempstring As String = sender.name
		Dim tempstring2 As String = String.Empty
		Dim tempint As Integer = 0
		If tempstring.IndexOf("poster") <> -1 Then
			tempstring = tempstring.Replace("poster", "")
			tempint = Convert.ToDecimal(tempstring)
			If tempstring2 = Nothing Then
				tempint = Convert.ToDecimal(tempstring)
				tempint = tempint + ((currentPage - 1) * 10)
				tempstring2 = posterArray(tempint).hdUrl
			End If
		End If
		If tempstring.IndexOf("moviefanart") <> -1 Then
			tempstring = tempstring.Replace("moviefanart", "")
			tempint = Convert.ToDecimal(tempstring)
			tempstring2 = fanartArray(tempint).hdUrl
		End If
		If tempstring.IndexOf("tvfanart") <> -1 Then
			tempstring = tempstring.Replace("tvfanart", "")
			tempint = Convert.ToDecimal(tempstring)
			tempstring2 = listOfTvFanarts(tempint).bigUrl
		End If
		Dim buffer(4000000) As Byte
		Dim size As Integer = 0
		Dim bytesRead As Integer = 0

		bigPanel = New Panel
		With bigPanel
			.Width = Me.Width
			.Height = Me.Height
			.BringToFront()
			.Dock = DockStyle.Fill
		End With
		Me.Controls.Add(bigPanel)
		bigPanel.BringToFront()
		bigPictureBox = New PictureBox()

		With bigPictureBox
			.Location = New Point(0, 0)
			.Width = bigPanel.Width
			.Height = bigPanel.Height
			.SizeMode = PictureBoxSizeMode.Zoom
			.WaitOnLoad = True

			.Visible = False
			.BorderStyle = BorderStyle.Fixed3D
			AddHandler bigPictureBox.DoubleClick, AddressOf util_PicBoxClose
			.Dock = DockStyle.Fill
		End With
		Try
			util_ImageLoad(bigPictureBox, tempstring2, "")
		Catch ex As Exception
		End Try
		Me.bigPanel.Controls.Add(bigPictureBox)
		Dim bigpanellabel As Label
		bigpanellabel = New Label
		With bigpanellabel
			.Location = New Point(20, 200)
			.Width = 150
			.Height = 50
			.Visible = True
			.Text = "Double Click Image To" & vbCrLf & "Return To Browser"
		End With

		Me.bigPanel.Controls.Add(bigpanellabel)
		bigpanellabel.BringToFront()
		Application.DoEvents()
		Me.Refresh()
		Try
			If bigPictureBox.Image Is Nothing Then
				tempstring2 = posterArray(tempint).ldUrl
				util_ImageLoad(bigPictureBox, tempstring2, "")
			End If
		Catch ex As Exception
		End Try
		Try
			If bigPictureBox.Image.Width < 20 Then
				tempstring2 = posterArray(tempint).ldUrl
				util_ImageLoad(bigPictureBox, tempstring2, "")
			End If
		Catch ex As Exception
		End Try
		Dim sizex As Integer = bigPictureBox.Image.Width
		Dim sizey As Integer = bigPictureBox.Image.Height
		tempstring = "Full Image Resolution :- " & sizex.ToString & " x " & sizey.ToString
		Dim resolutionlbl As New Label
		With resolutionlbl
			.Location = New Point(20, 450)
			.Width = 180
			.Text = tempstring
			.BackColor = Color.Transparent
		End With

		Me.bigPanel.Controls.Add(resolutionlbl)
		resolutionlbl.BringToFront()
		bigPictureBox.Visible = True
		bigPictureBox.Refresh()
	End Sub

	Private Sub SaveFanart(hd As Boolean, Optional clipbrd As Boolean = False)
		Try
			If ImgBw.Isbusy Then
				ImgBw.CancelAsync()
				Do Until Not ImgBw.IsBusy
					Application.DoEvents()
				Loop
			End If
			Try
				messbox.Close()
			Catch
			End Try
			messbox = New frmMessageBox("", "Downloading Fanart...", "")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

			Dim tempstring As String = String.Empty
			Dim tempint As Integer = 0
			Dim tempstring2 As String = String.Empty
			Dim allok As Boolean = False

			If clipbrd Then
				tempstring2 = PictureBox2.Tag.tostring
				allok = True
			Else
				'Find selected fanart, if any
				For Each button As Control In Me.Panel2.Controls

					If button.Name.IndexOf("checkbox") <> -1 Then
						Dim b1 As RadioButton = CType(button, RadioButton)
						If b1.Checked = True Then
							tempstring = b1.Name
							tempstring = tempstring.Replace("moviefanartcheckbox", "")
							tempint = Convert.ToDecimal(tempstring)
							If hd Then
								tempstring2 = fanartArray(tempint).hdUrl
							Else
								tempstring2 = fanartArray(tempint).ldUrl
							End If
							allok = True
							Exit For
						End If
					End If
				Next
			End If

			If Not allok Then
				MsgBox("No Fanart Is Selected")
			Else
				Try
					If Not MovFanartToggle Then
						Dim issavefanart As Boolean = Pref.savefanart
						Dim FanartOrExtraPath As String = mov_FanartORExtrathumbPath
						Dim xtra As Boolean = False
						Dim extrfanart As Boolean = False
						If rbMovThumb1.Checked Or rbMovThumb2.Checked Or rbMovThumb3.Checked Or rbMovThumb4.Checked Or rbMovThumb5.Checked Then xtra = True
						Pref.savefanart = True
						If xtra Then extrfanart = Movie.SaveFanartImageToCacheAndPath(tempstring2, FanartOrExtraPath)

						If xtra OrElse Movie.SaveFanartImageToCacheAndPath(tempstring2, FanartOrExtraPath) Then
							If Not xtra Then
								Dim paths As List(Of String) = Pref.GetfanartPaths(workingMovieDetails.fileinfo.fullpathandfilename, If(workingMovieDetails.fileinfo.videotspath <> "", workingMovieDetails.fileinfo.videotspath, ""))
								Movie.SaveFanartImageToCacheAndPaths(tempstring2, paths)
							End If
							Pref.savefanart = issavefanart
							mov_DisplayFanart()
							util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
							Dim video_flags = VidMediaFlags(workingMovieDetails.filedetails, workingMovieDetails.fullmoviebody.title.ToLower.Contains("3d"), workingMovie.DisplayFolderSize)
							movieGraphicInfo.OverlayInfo(PbMovieFanArt, ratingtxt.Text, video_flags)

							For Each paths In Pref.offlinefolders
								Dim offlinepath As String = paths & "\"
								If workingMovieDetails.fileinfo.fanartpath.IndexOf(offlinepath) <> -1 Then
									Dim mediapath As String
									mediapath = Utilities.GetFileName(workingMovieDetails.fileinfo.fullpathandfilename)
									messbox.TextBox1.Text = "Creating Offline Movie..."
									Call mov_OfflineDvdProcess(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fullmoviebody.title, mediapath)
								End If
							Next
						Else
							util_ImageLoad(PictureBox2, Utilities.DefaultFanartPath, Utilities.DefaultFanartPath)
							Pref.savefanart = issavefanart
						End If

						lblMovFanartWidth.Text = PictureBox2.Image.Width
						lblMovFanartHeight.Text = PictureBox2.Image.Height

						UpdateMissingFanart()

						XbmcLink_UpdateArtwork
					Else
						Dim MovSetFanartSavePath As String = workingMovieDetails.fileinfo.movsetfanartpath
						If MovSetFanartSavePath <> "" Then
							Movie.SaveFanartImageToCacheAndPath(tempstring2, MovSetFanartSavePath, True)
							MovPanel6Update()
							util_ImageLoad(PictureBox2, MovSetFanartSavePath, Utilities.DefaultFanartPath)
						Else
							MsgBox("!!  Problem formulating correct save location for Fanart" & vbCrLf & "                Please check your settings")
						End If
					End If

				Catch ex As WebException
					MsgBox(ex.Message)
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		Finally
			messbox.Close()
		End Try
	End Sub

	Sub UpdateMissingFanart
		oMovies.LoadMovie(workingMovieDetails.fileinfo.fullpathandfilename)

		ProgState = ProgramState.ResettingFilters
		Assign_FilterGeneral()
		ProgState = ProgramState.Other

		UpdateMissingFanartNav
	End Sub

	Sub UpdateMissingFanartNav

		'Default to selecting first row if non selected
		If DataGridViewMovies.SelectedRows.Count = 0 And DataGridViewMovies.Rows.Count > 1 Then DataGridViewMovies.Rows(0).Selected = True

		UpdateMissingFanartNextBtn
		UpdateMissingFanartPrevBtn
		UpdatelblFanartMissingCount
	End Sub

	Sub UpdatelblFanartMissingCount
		Dim i As Integer = 0
		Dim x As Integer = 0

		While i < DataGridViewMovies.Rows.Count
			Dim row As Data_GridViewMovie = DataGridViewMovies.DataSource(i)
			If row.MissingFanart Then x = x + 1
			i = i + 1
		End While

		lblFanartMissingCount.Text = x & " Missing"
		lblFanartMissingCount.Visible = x <> 0
	End Sub

	Sub UpdateMissingFanartNextBtn
		btnNextMissingFanart.Enabled = False

		If DataGridViewMovies.SelectedRows.Count = 0 Then Return

		Dim i As Integer = DataGridViewMovies.SelectedRows(0).Index + 1
		While i < DataGridViewMovies.Rows.Count
			Dim row As Data_GridViewMovie = DataGridViewMovies.DataSource(i)

			If row.MissingFanart Then
				btnNextMissingFanart.Enabled = True
				btnNextMissingFanart.Tag = i
				Return
			End If

			i = i + 1
		End While
		btnNextMissingFanart.Visible = btnNextMissingFanart.Enabled
	End Sub

	Sub UpdateMissingFanartPrevBtn
		btnPrevMissingFanart.Enabled = False

		If DataGridViewMovies.SelectedRows.Count = 0 Then Return

		Dim i As Integer = DataGridViewMovies.SelectedRows(0).Index - 1
		While i >= 0
			Dim row As Data_GridViewMovie = DataGridViewMovies.DataSource(i)

			If row.MissingFanart Then
				btnPrevMissingFanart.Enabled = True
				btnPrevMissingFanart.Tag = i
				Return
			End If

			i = i - 1
		End While
		btnPrevMissingFanart.Visible = btnPrevMissingFanart.Enabled
	End Sub

	Sub UpdateMissingPoster
		oMovies.LoadMovie(workingMovieDetails.fileinfo.fullpathandfilename)

		ProgState = ProgramState.ResettingFilters
		Assign_FilterGeneral()
		ProgState = ProgramState.Other

		UpdateMissingPosterNav
	End Sub

	Sub UpdateMissingPosterNav

		'Default to selecting first row if non selected
		If DataGridViewMovies.SelectedRows.Count = 0 And DataGridViewMovies.Rows.Count > 1 Then DataGridViewMovies.Rows(0).Selected = True

		UpdateMissingPosterNextBtn
		UpdateMissingPosterPrevBtn
		UpdatelblPosterMissingCount
	End Sub

	Sub UpdatelblPosterMissingCount
		Dim i As Integer = 0
		Dim x As Integer = 0
		While i < DataGridViewMovies.Rows.Count
			Dim row As Data_GridViewMovie = DataGridViewMovies.DataSource(i)
			If row.MissingPoster Then x = x + 1
			i = i + 1
		End While
		lblPosterMissingCount.Text = x & " Missing"
		lblPosterMissingCount.Visible = x <> 0
	End Sub

	Sub UpdateMissingPosterNextBtn
		btnNextMissingPoster.Enabled = False
		If DataGridViewMovies.SelectedRows.Count = 0 Then Return
		Dim i As Integer = DataGridViewMovies.SelectedRows(0).Index + 1
		While i < DataGridViewMovies.Rows.Count
			Dim row As Data_GridViewMovie = DataGridViewMovies.DataSource(i)
			If row.MissingPoster Then
				btnNextMissingPoster.Enabled = True
				btnNextMissingPoster.Tag = i
				Return
			End If
			i = i + 1
		End While
		btnNextMissingPoster.Visible = btnNextMissingPoster.Enabled
	End Sub

	Sub UpdateMissingPosterPrevBtn
		btnPrevMissingPoster.Enabled = False
		If DataGridViewMovies.SelectedRows.Count = 0 Then Return
		Dim i As Integer = DataGridViewMovies.SelectedRows(0).Index - 1
		While i >= 0
			Dim row As Data_GridViewMovie = DataGridViewMovies.DataSource(i)
			If row.MissingPoster Then
				btnPrevMissingPoster.Enabled = True
				btnPrevMissingPoster.Tag = i
				Return
			End If
			i = i - 1
		End While
		btnPrevMissingPoster.Visible = btnPrevMissingPoster.Enabled
	End Sub

	Private Function util_ImageCrop(ByVal SrcBmp As Bitmap, ByVal NewSize As Size, ByVal StartPoint As Point) As Bitmap
		If NewSize.Width < 1 Or NewSize.Height < 1 Then
			Return SrcBmp
			Exit Function
		End If
		Dim SrcRect As New Rectangle(StartPoint.X, StartPoint.Y, NewSize.Width, NewSize.Height)
		Dim DestRect As New Rectangle(0, 0, NewSize.Width, NewSize.Height)
		Dim DestBmp As New Bitmap(NewSize.Width, NewSize.Height, Imaging.PixelFormat.Format32bppArgb)
		Dim g As Graphics = Graphics.FromImage(DestBmp)
		g.DrawImage(SrcBmp, DestRect, SrcRect, GraphicsUnit.Pixel)
		Return DestBmp
	End Function ' Crop Image Function

	Private Sub util_ImageCropTop()
		If PictureBox2.Image Is Nothing Then Exit Sub
		Dim imagewidth As Integer
		Dim imageheight As Integer
		imagewidth = PictureBox2.Image.Width
		imageheight = PictureBox2.Image.Height
		PictureBox2.Image = util_ImageCrop(PictureBox2.Image, New Size(imagewidth, imageheight - 1), New Point(0, 1)).Clone
		PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
	End Sub

	Private Sub util_ImageCropBottom()
		If PictureBox2.Image Is Nothing Then Exit Sub
		Dim imagewidth As Integer
		Dim imageheight As Integer
		imagewidth = PictureBox2.Image.Width
		imageheight = PictureBox2.Image.Height
		PictureBox2.Image = util_ImageCrop(PictureBox2.Image, New Size(imagewidth, imageheight - 1), New Point(0, 0)).Clone
		PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
	End Sub

	Private Sub util_ImageCropLeft()
		If PictureBox2.Image Is Nothing Then Exit Sub
		Dim imagewidth As Integer
		Dim imageheight As Integer
		imagewidth = PictureBox2.Image.Width
		imageheight = PictureBox2.Image.Height
		PictureBox2.Image = util_ImageCrop(PictureBox2.Image, New Size(imagewidth - 1, imageheight), New Point(1, 0)).Clone
		PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
	End Sub

	Private Sub util_ImageCropRight()
		If PictureBox2.Image Is Nothing Then Exit Sub
		Dim imagewidth As Integer
		Dim imageheight As Integer
		'thumbedItsMade = True
		imagewidth = PictureBox2.Image.Width
		imageheight = PictureBox2.Image.Height
		PictureBox2.Image = util_ImageCrop(PictureBox2.Image, New Size(imagewidth - 1, imageheight), New Point(0, 0)).Clone
		PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
	End Sub
    
	Private Sub mov_PosterInitialise()
		pageCount = 0
		currentPage = 1
		cbMoviePosterSaveLoRes.Enabled = False
		btnPosterTabs_SaveImage.Enabled = False
        panelAvailableMoviePosters.Controls.Clear()
		If Pref.maximumthumbs < 1 Then
		Else
			Pref.maximumthumbs = 10
		End If
		btnPosterTabs_SaveImage.Enabled = False
		cbMoviePosterSaveLoRes.Enabled = False
		posterPicBoxes = Nothing
		posterCheckBoxes = Nothing
		resLabel = Nothing
		Application.DoEvents()
		posterArray.Clear()
	End Sub

	Private Sub mov_PosterSelectionDisplay()
		Dim names As New List(Of McImage)
		Me.panelAvailableMoviePosters.Visible = False
		If pageCount = 0 Then
			messbox = New frmMessageBox("", "Retrieving Posters...", "Please wait...")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
		End If
		Dim itemcounter As Integer = 0
		If posterArray.Count > 0 Then
			Dim MovPosterPicBox As New List(Of FanartPicBox)
			If pageCount = 0 Then pageCount = Math.Ceiling(posterArray.count / 10)
			If posterArray.Count > 10 Then
				Dim totalthispagecount As Integer = ((currentPage * 10) - 1)
				If totalthispagecount >= posterArray.Count Then totalthispagecount = posterArray.Count - 1
				For f = ((currentPage - 1) * 10) To totalthispagecount '((currentPage*10)-1)
					names.Add(posterArray(f))
				Next
			Else
				For f = 0 To posterArray.Count - 1
					names.Add(posterArray(f))
				Next
			End If

			If pageCount > 1 Then
				btnMovPosterNext.Visible = True
				btnMovPosterPrev.Visible = True
				If posterArray.Count >= 10 Then
					lblMovPosterPages.Text = "Displaying " & ((currentPage * 10) - 9) & " to " & (currentPage * 10) & " of " & posterArray.Count.ToString & " Images"
				Else
					lblMovPosterPages.Text = "Displaying 1 to " & posterArray.Count.ToString & " of " & posterArray.Count.ToString & " Images"
				End If
				lblMovPosterPages.Visible = True
				Me.Refresh()
				Application.DoEvents()
				btnMovPosterPrev.Enabled = currentPage > 1
				btnMovPosterNext.Enabled = currentPage < pagecount
			Else
				btnMovPosterPrev.Visible = False
				btnMovPosterNext.Visible = False
				lblMovPosterPages.Text = "Displaying 1 to " & posterArray.Count.ToString & " of " & posterArray.Count.ToString & " Images"
				lblMovPosterPages.Visible = True
				Me.Refresh()
				Application.DoEvents()
			End If
			Dim tempboolean As Boolean = True
			Dim locationX As Integer = 0
			Dim locationY As Integer = 0

			messbox.TextBox2.Text = "Setting up display...."
			messbox.Refresh()

			For Each item In names
				Dim thispicbox As New FanartPicBox
				Try
					posterPicBoxes() = New PictureBox()
					With posterPicBoxes
						.WaitOnLoad = True
						.Location = New Point(locationX, locationY)
						.Width = 126
						.Height = 189
						.SizeMode = PictureBoxSizeMode.Zoom
						.Visible = True
						.BorderStyle = BorderStyle.Fixed3D
						.Name = "poster" & itemcounter.ToString
						AddHandler posterPicBoxes.DoubleClick, AddressOf util_ZoomImage2
						AddHandler posterPicBoxes.LoadCompleted, AddressOf util_ImageRes
					End With
					thispicbox.imagepath = item.ldUrl
					thispicbox.pbox = posterPicBoxes
					MovPosterPicBox.Add(thispicbox)

					posterCheckBoxes() = New RadioButton()
					With posterCheckBoxes
						.Location = New Point(locationX + 18, locationY + 187) '179
						.Width = 79
						.Height = 32
						.Name = "postercheckbox" & itemcounter.ToString
						.SendToBack()
						.CheckAlign = ContentAlignment.TopCenter
						If item.hdheight <> "" Then
							.Text = item.hdwidth & " x " & item.hdheight
						Else
							.Text = "?"
						End If
						.TextAlign = ContentAlignment.BottomCenter
						AddHandler posterCheckBoxes.CheckedChanged, AddressOf mov_PosterRadioChanged
					End With

					itemcounter += 1

					Me.panelAvailableMoviePosters.Controls.Add(posterPicBoxes())
					Me.panelAvailableMoviePosters.Controls.Add(posterCheckBoxes())
					Me.Refresh()
					Application.DoEvents()
					If tempboolean = True Then
						locationY = (213 + 19)
					Else
						locationX += 126
						locationY = 0
					End If
					tempboolean = Not tempboolean
				Catch ex As Exception
				End Try
			Next
			Me.panelAvailableMoviePosters.Refresh()
			Me.Refresh()
			Me.panelAvailableMoviePosters.Visible = True
			If MovPosterPicBox.Count > 0 Then
				messbox.Close()
				If Not ImgBw.IsBusy Then
					ToolStripStatusLabel2.Text = "Starting Download of Images..."
					ToolStripStatusLabel2.Visible = True
					ImgBw.RunWorkerAsync({MovPosterPicBox, 0, If(posterArray.Count >= 10, 10, posterArray.Count), Me.panelAvailableMoviePosters})
				End If
			End If
		Else
			Dim mainlabel2 As Label
			mainlabel2 = New Label
			With mainlabel2
				.Location = New Point(0, 100)
				.Width = 700
				.Height = 100
				.Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
				.Text = "No Posters Were Found For This Movie"

			End With
			Me.panelAvailableMoviePosters.Controls.Add(mainlabel2)
		End If
		If itemcounter = 0 Then
			btnMovPosterNext.Visible = False
			btnMovPosterPrev.Visible = False
			Dim mainlabel2 As Label
			mainlabel2 = New Label
			With mainlabel2
				.Location = New Point(0, 100)
				.Width = 700
				.Height = 100
				.Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
				.Text = "No Posters Were Found For This Movie"
			End With
			lblMovPosterPages.Text = "0 of 0 Images"
			Me.panelAvailableMoviePosters.Controls.Add(mainlabel2)
		End If
		Me.panelAvailableMoviePosters.Visible = True
		messbox.Close()
	End Sub

	Private Sub mov_PosterSelectionDisplayPrev()
		Try
            panelAvailableMoviePosters.Controls.Clear()
			currentPage -= 1
			If currentPage = 1 Then
				btnMovPosterPrev.Enabled = False
			End If
			btnMovPosterNext.Enabled = True
			mov_PosterSelectionDisplay()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub mov_PosterSelectionDisplayNext()
		Try
            panelAvailableMoviePosters.Controls.Clear()
			currentPage += 1
			If currentPage = pageCount Then
				btnMovPosterNext.Enabled = False
			End If
			btnMovPosterPrev.Enabled = True
			mov_PosterSelectionDisplay()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub mov_PosterRadioChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim tempstring As String = sender.name
		Dim tempint As Integer = 0
		Dim tempstring2 As String = tempstring
		Dim allok As Boolean = False
		tempstring = tempstring.Replace("postercheckbox", "")
		tempint = Convert.ToDecimal(tempstring)
		For Each button As Control In Me.panelAvailableMoviePosters.Controls
			If button.Name.IndexOf("postercheckbox") <> -1 Then
				Dim b1 As RadioButton = CType(button, RadioButton)
				If b1.Checked = True Then
					allok = True
					Exit For
				End If
			End If
		Next
		If allok = True Then
			btnPosterTabs_SaveImage.Enabled = True
			cbMoviePosterSaveLoRes.Enabled = (posterArray(0).ldUrl.ToLower.IndexOf("impawards") <> -1 Or posterArray(0).ldUrl.ToLower.IndexOf("image.tmdb") <> -1)
		Else
			cbMoviePosterSaveLoRes.Enabled = False
			btnPosterTabs_SaveImage.Enabled = False
		End If
	End Sub

	Private Sub util_ImageRes(ByVal sender As Object, ByVal e As EventArgs)
		resLabel = New Label
		Dim tempstring As String
		tempstring = sender.image.width.ToString
		tempstring = tempstring & " x "
		tempstring = tempstring & sender.image.height.ToString
		Dim locx As Integer = sender.location.x
		Dim locy As Integer = sender.location.y
		locy = locy + sender.height
		With resLabel
			.Location = New Point(locx + 30, locy)
			.Text = tempstring
			.BringToFront()
		End With
		Me.panelAvailableMoviePosters.Controls.Add(resLabel)
		Me.Refresh()
		Application.DoEvents()
	End Sub

	Private Sub cmbxEpActor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbxEpActor.SelectedIndexChanged
		Try
			Dim Episode As Media_Companion.TvEpisode
			If TvTreeview.SelectedNode IsNot Nothing Then
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
					Episode = TvTreeview.SelectedNode.Tag
				Else
					Exit Sub
				End If
			Else
				Exit Sub
			End If

			pbEpActorImage.Image = Nothing
			pbEpActorImage.Visible = True
			For Each actor In Episode.ListActors
				If actor.actorname = cmbxEpActor.Text Then
					tbEpRole.Text = actor.actorrole

					Dim temppath As String = Episode.ShowObj.FolderPath
					Dim tempname As String = actor.actorname.Replace(" ", "_") & If(Pref.FrodoEnabled, ".jpg", ".tbn")
					temppath = temppath & ".actors\" & tempname
					If File.Exists(temppath) Then
						util_ImageLoad(pbEpActorImage, temppath, Utilities.DefaultActorPath)
						Exit Sub
					End If
					If actor.actorthumb <> Nothing Then
						If actor.actorthumb.IndexOf("http") <> -1 Or File.Exists(actor.actorthumb) Then
							util_ImageLoad(pbEpActorImage, actor.actorthumb, Utilities.DefaultActorPath)
						Else
							util_ImageLoad(pbEpActorImage, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
						End If
					Else
						util_ImageLoad(pbEpActorImage, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
					End If
					pbEpActorImage.SizeMode = PictureBoxSizeMode.Zoom
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub ExpandAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandAllToolStripMenuItem.Click
		Try
			Dim node As TreeNode
			For Each node In TvTreeview.Nodes
				node.ExpandAll()
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub CollapseAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseAllToolStripMenuItem.Click
		Try
			Dim node As TreeNode
			For Each node In TvTreeview.Nodes
				node.Collapse()
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ExpandSelectedShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandSelectedShowToolStripMenuItem.Click
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)

			WorkingTvShow.ShowNode.ExpandAll()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub CollapseSelectedShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseSelectedShowToolStripMenuItem.Click
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)

			WorkingTvShow.ShowNode.Collapse()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ReloadItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_ReloadFromCache.Click
		Try
			Call tv_ShowReload(True)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tv_ShowReload(Optional ByVal force As Boolean = False)
		Dim Show As Media_Companion.TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim Season As Media_Companion.TvSeason = tv_SeasonSelectedCurrently(TvTreeview)
		Dim Episode As Media_Companion.TvEpisode = ep_SelectedCurrently(TvTreeview)
	End Sub

	Private Sub TabControl3_Selecting(ByVal sender As Object, ByVal e As CancelEventArgs) Handles TabControl3.Selecting
		If TabControl3.SelectedTab.Name = tpTvPrefs.Name Then
			e.Cancel = True
			OpenPreferences(2)
		End If
	End Sub

	Private Sub TabControl3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl3.SelectedIndexChanged

		Try
			Dim Show As Media_Companion.TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim tab As String = TabControl3.SelectedTab.Name
			Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
			If (tab <> tpTvMainBrowser.Name And tab <> tpTvFolders.Name And tab <> tpTvPrefs.Name) AndAlso Show Is Nothing Then
				MsgBox("No TV Show is selected")
				Exit Sub
			End If

			Dim tempstring As String = ""
			If tab <> tpTvMainBrowser.Name And tab <> tpTvFolders.Name And tab <> tpTvPrefs.Name Then
				If Show.NfoFilePath = "" And tvFolders.Count = 0 Then
					Me.TabControl3.SelectedIndex = tvCurrentTabIndex
					MsgBox("There are no TV Shows in your list to work on" & vbCrLf & "Set the Preferences as you want them" & vbCrLf & "Using the Preferences Tab, then" & vbCrLf & "add your TV Folders using the Folders Tab" & vbCrLf & "Once the tvshow has been scraped then" & vbCrLf & "Use the tab, ""Search for new episodes""")
					If tab <> tpTvPrefs.Name Then Exit Sub
				ElseIf Show.NfoFilePath = "" And tvFolders.Count > 0 And tab <> tpTvPrefs.Name Then
					Me.TabControl3.SelectedIndex = tvCurrentTabIndex
					If Cache.TvCache.Shows.Count > 0 Then
						MsgBox("No TV Show is selected")
						Exit Sub
					Else
						MsgBox("There are no TV Shows in your list to work on")
						Exit Sub
					End If
				End If
			ElseIf tab = tpTvFolders.Name Then      'Tv Folders tab
				tvCurrentTabIndex = TabControl3.SelectedIndex
				TabControl3.SelectedIndex = tvCurrentTabIndex
				Call tv_FoldersSetup()
			Else
				tvCurrentTabIndex = 0
				Exit Sub
			End If
			If tab = tpTvSelector.Name Then         'Tv Show Change tba
				If lbxTvShSelectResults.Items.Count = 0 Then
					tvCurrentTabIndex = TabControl3.SelectedIndex
					Call tv_ShowChangedRePopulate()
				End If
			ElseIf tab = tpTvMainBrowser.Name Then  'Tv Main Browser tab
				If TvTreeview.Nodes.Count = 0 Then TvTreeview.SelectedNode = TvTreeview.TopNode
				TvTreeview.Focus()
				tvCurrentTabIndex = 0
			ElseIf tab = tpTvPosters.Name Then      'Tv Posters tab
				tvCurrentTabIndex = TabControl3.SelectedIndex
				Call tv_PosterSetup()
			ElseIf tab = tpTvTable.Name Then        'Tv TableView tab
				tvCurrentTabIndex = TabControl3.SelectedIndex
				Call tv_TableView()
			ElseIf tab = tpTvWall.Name Then         'Tv Wall View tab
				tvCurrentTabIndex = TabControl3.SelectedIndex
				Call tv_wallSetup()
			ElseIf tab = tpTvWeb.Name Then        'Tv Web Browser tab
				Dim TvdbId As Integer = 0
				If Not String.IsNullOrEmpty(Show.TvdbId.Value) AndAlso Integer.TryParse(Show.TvdbId.Value, TvdbId) Then
					Dim tpi As Integer = tpTvWeb.ImageIndex
					If tpi = 0 Then
						tempstring = "http://thetvdb.com/?tab=series&id=" & TvdbId & "&lid=7"
					Else
						tempstring = "http://www.imdb.com/title/" & Show.ImdbId.Value & "/"
					End If
					Call GoTvWebBrowser(tempstring)
				ElseIf String.IsNullOrEmpty(Show.TvdbId.Value) AndAlso String.IsNullOrEmpty(Show.ImdbId.Value) Then
					TabControl3.SelectedIndex = 0
					MsgBox("No TVDB or IMDB ID present for selected Show" & vbCrLf & "Use Tv Show Selector Tab, to select" & vbCrLf & "correct show")
				End If
			ElseIf tab = tpTvFanart.Name Then       'Tv Fanart tab
				Call tv_Fanart_Load()
				tvCurrentTabIndex = TabControl3.SelectedIndex
			ElseIf tab = tpTvFanartTv.Name Then     'Tv Fanart.Tv tab
				UcFanartTvTv1.ucFanartTv_Refresh(tv_ShowSelectedCurrently(TvTreeview))
			ElseIf tab = tpTvScreenshot.Name Then   'Tv Episode Screenshot tab
				tvCurrentTabIndex = TabControl3.SelectedIndex
				Call GoScreenshotTab(WorkingEpisode)
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub TabControl3_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TabControl3.MouseClick
		If e.Button = Windows.Forms.MouseButtons.Right Then
			For index As Integer = 0 To TabControl3.TabCount - 2 Step 1
				If TabControl3.GetTabRect(index).Contains(e.X, e.Y) AndAlso TabControl3.TabPages(index).Name.ToLower = "tptvweb" Then
					Dim tpi As Integer = tpTvWeb.ImageIndex
					If tpi = 0 Then
						tpTvWeb.ImageIndex = 1
					Else
						tpTvWeb.ImageIndex = 0
					End If
					tpTvWeb.Refresh()
					Exit For
				End If
			Next
		End If
	End Sub

	Private Sub TabControl3_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TabControl3.MouseWheel
		Try
			If Tabcontrol3.TabPages(TabControl3.SelectedIndex).Text.ToLower = "wall" Then
				mouseDelta = e.Delta / 120
				Try
					tptvwall.AutoScrollPosition = New Point(0, tptvwall.VerticalScroll.Value - (mouseDelta * 30))
				Catch ex As Exception
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub GoTvWebBrowser(ByVal tempstring As String)
		If Pref.externalbrowser = True Then
			Me.TabControl3.SelectedIndex = tvCurrentTabIndex
			OpenUrl(tempstring)
		Else
			tvCurrentTabIndex = TabControl3.SelectedIndex
			Try
				WebBrowser4.Stop()
				WebBrowser4.ScriptErrorsSuppressed = True
				WebBrowser4.Navigate(tempstring)
			Catch
				WebBrowser4.Stop()
				WebBrowser4.ScriptErrorsSuppressed = True
				WebBrowser4.Navigate(tempstring)
			End Try
			WebBrowser4.Focus()
		End If
	End Sub

	Private Sub GoScreenshotTab(ByRef WorkingEpisode As TvEpisode)
		If Pref.EdenEnabled Then
			util_ImageLoad(pbTvEpScrnShot, WorkingEpisode.VideoFilePath.Replace(Path.GetExtension(WorkingEpisode.VideoFilePath), ".tbn"), Utilities.DefaultScreenShotPath)
		End If
		If Pref.FrodoEnabled Then
			util_ImageLoad(pbTvEpScrnShot, WorkingEpisode.VideoFilePath.Replace(Path.GetExtension(WorkingEpisode.VideoFilePath), "-thumb.jpg"), Utilities.DefaultScreenShotPath)
		End If
		If TextBox35.Text = "" Then
			TextBox35.Text = Pref.ScrShtDelay
		End If
        Dim currNfo As String = WorkingEpisode.NfoFilePath
        If lastTvNfo = currNfo Then Exit Sub
        lastTvNfo = currNfo
        pbepscrsht_Clear()
        tv_EpThumbScreenShot.PerformClick()
    End Sub

	Private Sub tv_TableView()
		Dim availableshows As New List(Of TvShow)

		messbox = New frmMessageBox("Loading all tvshow.nfo")
		messbox.Show

		For Each sh As TvShow In Cache.TvCache.Shows
			Dim shload As New TvShow
			shload.NfoFilePath = sh.NfoFilePath
			shload = nfoFunction.tvshow_NfoLoad(sh.NfoFilePath) '.Load()
			availableshows.Add(shload)
		Next
		messbox.Close()
		Dim tvdbase(availableshows.Count, 8) as String
		DataGridView2.Rows.Clear()
		DataGridView2.Columns.Item(1).DefaultCellStyle.WrapMode = DataGridViewTriState.True

		Try
			Dim lst As List(Of TvShow) = (From x In availableshows Order By x.Title.Value).ToList
			For Each sh As TvShow In lst
				Dim n As Integer = DataGridView2.Rows.Add()
				DataGridView2.Rows(n).Cells(0).Value = sh.Title.Value
				DataGridView2.Rows(n).Cells(1).Value = sh.Plot.Value
				DataGridView2.Rows(n).Cells(2).Value = sh.Premiered.Value
				DataGridView2.Rows(n).Cells(3).Value = sh.Rating.Value
				DataGridView2.Rows(n).Cells(4).Value = sh.Genre.Value
				DataGridView2.Rows(n).Cells(5).Value = sh.Studio.Value
				DataGridView2.Rows(n).Cells(6).Value = sh.TvdbId.Value
				DataGridView2.Rows(n).Cells(7).Value = sh.ImdbId.Value
				DataGridView2.Rows(n).Cells(8).Value = sh.Mpaa.Value
			Next
		Catch
		End Try
	End Sub

	Public Function util_LanguageListLoad() As Boolean
		languageList.Clear()
		Application.DoEvents()
		System.Threading.Thread.Sleep(500)
		Dim XmlFile As String
		XmlFile = Utilities.DownloadTextFiles("http://thetvdb.com/api/6E82FED600783400/languages.xml")
        If XmlFile = "" Then Return False
		Dim LangList As New Tvdb.Languages()
		LangList.LoadXml(XmlFile)
		For Each Lang As Tvdb.Language In LangList.Languages
			languageList.Add(Lang)
		Next
		lbxTvShSelectLang.Items.Clear()
		For Each lan In languageList
			lbxTvShSelectLang.Items.Add(lan.Language.Value)
		Next
        Return True
	End Function

	Private Sub tv_ShowChangedRePopulate()
		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Try
			tbTvShSelectSearchTitle.Text = Utilities.GetLastFolder(WorkingTvShow.NfoFilePath)
			tb_TvShSelectSeriesPath.Enabled = True
			tb_TvShSelectSeriesPath.Text = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "")
			pbTvShSelectPreviewImg.Image = Nothing
			If languageList.Count = 0 Then
				Dim aok As Boolean = util_LanguageListLoad()
                If Not aok Then Throw New Exception("error loading languages")
			End If
			If workingTvShow.language <> Nothing Then
				For Each language In languageList
					If language.Abbreviation.Value = WorkingTvShow.Language.Value Then
						lbxTvShSelectLang.SelectedItem = language.Language.Value
						Exit For
					End If
				Next
			Else
				lbxTvShSelectLang.SelectedItem = Pref.TvdbLanguage
			End If
			Label55.Text = "Default Language for TV Shows is :- " & Pref.TvdbLanguage
			Call tv_ShowListLoad()
			Try
				If Pref.sortorder <> Nothing Then
					If Pref.sortorder = "dvd" Then
						rbTvShSelectOrderDVD.Checked = True
					Else
						rbTvShSelectOrderDefault.Checked = True
					End If
				Else
					rbTvShSelectOrderDefault.Checked = True
				End If
			Catch ex As Exception
				rbTvShSelectOrderDefault.Checked = True
			End Try

			Select Case Pref.seasonall
				Case "none"
					rbTvShSelectSAllNone.Checked = True
				Case "poster"
					rbTvShSelectSAllPoster.Checked = True
				Case "wide"
					rbTvShSelectSAllWide.Checked = True
			End Select
			'0 - Everything from TVDB
			'1 - Everything from IMDB
			'2 - TV show Nfo From IMDB, Episode nfo from TVDB
			'3 - TV show Nfo From TVDB, Episode nfo from IMDB
			If Pref.tvdbactorscrape = 0 Then
				rbTvShSelectTvActorTvdb.Checked = True
				rbTvShSelectEpActorTvdb.Checked = True
			ElseIf Pref.tvdbactorscrape = 1 Then
				rbTvShSelectTvActorImdb.Checked = True
				rbTvShSelectEpActorImdb.Checked = True
			ElseIf Pref.tvdbactorscrape = 2 Then
				rbTvShSelectTvActorImdb.Checked = True
				rbTvShSelectEpActorTvdb.Checked = True
			ElseIf Pref.tvdbactorscrape = 3 Then
				rbTvShSelectTvActorTvdb.Checked = True
				rbTvShSelectEpActorImdb.Checked = True
			End If

			If Pref.postertype = "poster" Then
				rbTvShSelectArtPoster.Checked = True
			Else
				rbTvShSelectArtBanner.Checked = True
			End If

			cbTvChgShowDLFanart         .Checked = Pref.tvdlfanart
			cbTvChgShowDLPoster         .Checked = Pref.tvdlposter
			cbTvChgShowDLSeason         .Checked = Pref.tvdlseasonthumbs
			cbTvChgShowDLFanartTvArt    .Checked = Pref.TvDlFanartTvArt

			If Pref.tvshow_useXBMC_Scraper = True Then
				gpbx_TvActorSource  .Enabled = False
				gpbx_EpActorSource  .Enabled = False
				gpbx_SortOrder      .Enabled = False
			Else
				gpbx_TvActorSource  .Enabled = True
				gpbx_EpActorSource  .Enabled = True
				gpbx_SortOrder      .Enabled = True
			End If
			cbTvChgShowOverwriteImgs.CheckState = CheckState.Checked 'set overwrite images for changing shows.
		Catch ex As Exception
			MsgBox("There seems to be a problem with the tvdb website, please try again later")
			tvCurrentTabIndex = 0
			TabControl3.SelectedIndex = 0
		End Try
	End Sub

	Private Sub util_LanguageCheck()
		Try
			If lbxTvShSelectLang.SelectedIndex < 0 Then lbxTvShSelectLang.SelectedIndex = languageList.FindIndex(Function(index As Tvdb.Language) index.Abbreviation.Value = Pref.TvdbLanguageCode)
			If lbxTvShSelectResults.SelectedIndex = -1 orElse listOfShows(lbxTvShSelectResults.SelectedIndex).showid = "none" Then Exit Sub
			Dim languagecode As String = languageList(lbxTvShSelectLang.SelectedIndex).Abbreviation.Value
			Dim url As String = "http://thetvdb.com/api/6E82FED600783400/series/" & listOfShows(lbxTvShSelectResults.SelectedIndex).showid & "/" & languagecode & ".xml"
			Dim websource(10000)
			Dim urllinecount As Integer = 0
			Try
				Dim wrGETURL As WebRequest
				wrGETURL = WebRequest.Create(url)
				wrGETURL.Proxy = Utilities.MyProxy
				Dim objStream As IO.Stream
				objStream = wrGETURL.GetResponse.GetResponseStream()
				Dim objReader As New IO.StreamReader(objStream)
				Dim sLine As String = ""
				urllinecount = 0
				Do While Not sLine Is Nothing
					urllinecount += 1
					sLine = objReader.ReadLine
					If Not sLine Is Nothing Then
						websource(urllinecount) = sLine
					End If
				Loop
				objReader.Close()
				objStream.Close()
				objReader = Nothing
				objStream = Nothing
				urllinecount -= 1
			Catch ex As Exception
			End Try
			For f = 1 To urllinecount
				If websource(f).IndexOf("<Language>") <> -1 Then
					websource(f) = websource(f).Replace("<Language>", "")
					websource(f) = websource(f).Replace("</Language>", "")
					websource(f) = websource(f).Replace("  ", "")
					If websource(f).ToLower <> languagecode Then
						Label55.BackColor = Color.Red
						Label55.Text = lbxTvShSelectResults.SelectedItem.ToString & " is not available in " & lbxTvShSelectLang.SelectedItem.ToString & ", Please try another language"
					Else
						Label55.BackColor = Color.Transparent
						Label55.Text = lbxTvShSelectResults.SelectedItem.ToString & " is available in " & lbxTvShSelectLang.SelectedItem.ToString
						Label55.Font = New Font(Label55.Font, FontStyle.Bold)
					End If
				End If
			Next
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
	End Sub

	Private Sub RefreshShowsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshShowsToolStripMenuItem.Click
		Call tv_CacheRefresh()
	End Sub

	Private Sub Tv_tsmi_CheckDuplicateEpisodes_Click(sender As Object, e As EventArgs) Handles Tv_tsmi_CheckDuplicateEpisodes.Click
		Tv_CacheCheckDuplicates()
	End Sub

	Private Sub ReloadShowCacheToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadShowCacheToolStripMenuItem.Click
		Try
			If File.Exists(workingProfile.tvcache) Then
				Call tv_CacheLoad()
			Else
				MsgBox("No Cache exists to load")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Function util_ReplaceXMLChrs(ByVal Text As String)                  'Convert textcodes to real characters
		If Text.IndexOf("â€˜") <> -1 Then Text = Text.Replace("â€˜", "'")
		If Text.IndexOf("â€™") <> -1 Then Text = Text.Replace("â€™", "'")
		If Text.IndexOf("â€™") <> -1 Then Text = Text.Replace("â€™", "'")
		If Text.IndexOf("â€" & Chr(147)) <> -1 Then Text = Text.Replace("â€" & Chr(147), "-")

		Return Text
	End Function

	Private Function ep_NfoValidate(ByVal nfopath As String)
		Dim validated As Boolean = True
		If File.Exists(nfopath) Then
			Dim tvshow As New XmlDocument
			Try
                Using tmpstrm As IO.StreamReader = File.OpenText(nfopath)
                    tvshow.Load(tmpstrm)
                End Using
				
			Catch ex As Exception
				If ex.Message.ToLower.Contains("multiple root elements") Then
					validated = chkxbmcmultinfo(nfopath)
				Else
					validated = False
				End If
			End Try
			If validated = True Then
				Try
					Dim tempstring As String
					Using filechck As IO.StreamReader = File.OpenText(nfopath)
					    tempstring = filechck.ReadToEnd.ToLower
					End Using
					If tempstring = Nothing Then
						validated = False
					End If
					Try
						Dim seasonno As String = tempstring.Substring(tempstring.IndexOf("<season>") + 8, tempstring.IndexOf("</season>") - tempstring.IndexOf("<season>") - 8)
						If Not IsNumeric(seasonno) Then
							validated = False
						End If
					Catch ex As Exception
						validated = False
					End Try
					Try
						Dim episodeno As String = tempstring.Substring(tempstring.IndexOf("<episode>") + 9, tempstring.IndexOf("</episode>") - tempstring.IndexOf("<episode>") - 9)
						If Not IsNumeric(episodeno) Then
							validated = False
						End If
					Catch ex As Exception
						validated = False
					End Try
				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
				End Try
			End If
			Return validated
		End If
		Return False
	End Function

	Private Function chkxbmcmultinfo(ByVal xmlpath As String) As Boolean
		Try
			Dim testxml() As String = File.ReadAllLines(xmlpath)
			Dim first As Boolean = True
			Dim finalxml As String = ""
			For Each line In testxml
				If line.Contains("<episodedetails>") AndAlso first Then
					finalxml &= "<multiepisodenfo>"
				End If
				finalxml &= line
				If line.Contains("</episodedetails>") Then first = False
			Next
			finalxml &= "</multiepisodenfo>"
			Dim Finaldoc As New XmlDocument
			Finaldoc.LoadXml(finalxml)
			Finaldoc.Save(xmlpath)
			Return ep_NfoValidate(xmlpath)
		Catch
			Return False
		End Try
		Return False
	End Function
    
	Private Sub OpenFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_OpenFolder.Click
		Try
			If Not TvTreeview.SelectedNode Is Nothing Then
				Dim Path As String = Nothing
				Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)  'set WORKINGTVSHOW to show obj irrelavent if we have selected show/season/episode
				Dim WorkingTvEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
				Dim WorkingTvSeason As TvSeason = tv_SeasonSelectedCurrently(TvTreeview)
				If Not IsNothing(WorkingTvEpisode) AndAlso Not WorkingTvEpisode.IsMissing Then
					Path = WorkingTvEpisode.NfoFilePath
				ElseIf Not IsNothing(WorkingTvSeason) AndAlso Not IsNothing(WorkingTvSeason.FolderPath) Then
					Path = WorkingTvSeason.FolderPath
				ElseIf Not WorkingTvShow.NfoFilePath Is Nothing And Not WorkingTvShow.NfoFilePath = "" Then
					Path = WorkingTvShow.NfoFilePath  'we send the path of the tvshow.nfo, that way in explorer it will be highlighted in the folder
				Else
					MsgBox("There is no show selected to open")
				End If
				If Not IsNothing(Path) Then
					Call util_OpenFolder(Path)
				End If
			Else
				MsgBox("There is no show selected to open")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub WebBrowser2_NewWindow(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WebBrowser2.NewWindow
		Try
			e.Cancel = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub WebBrowser4_NewWindow(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WebBrowser4.NewWindow
		Try
			Dim myelement As HtmlElement = WebBrowser4.Document.ActiveElement
			Dim target As String = myelement.GetAttribute("href")
			e.Cancel = True
			WebBrowser4.Navigate(target)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PictureBox_Zoom(ByVal sender As Object, ByVal e As System.EventArgs) Handles tv_PictureBoxBottom.DoubleClick, tv_PictureBoxRight.DoubleClick, tv_PictureBoxLeft.DoubleClick
		Try
			Dim picBox As PictureBox = sender
			Dim imageLocation As String = picBox.tag
			If imageLocation <> Nothing Then
				If File.Exists(imageLocation) Then
					Me.ControlBox = False
					MenuStrip1.Enabled = False
					Call util_ZoomImage(imageLocation)
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub
    
	Sub tv_Rescrape() 'Panel9 visibility indicates which is selected - a tvshow or an episode
		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
		If IsNothing(WorkingTvShow.TvdbId.Value) = True Then
			WorkingTvShow.TvdbId.Value = ""
		End If
		If WorkingTvShow.TvdbId.Value.IndexOf("tt").Equals(0) Then tv_IMDbID_detected = True
		If Panel_EpisodeInfo.Visible = False Then 'i.e. rescrape selected TVSHOW else rescrape selected EPISODE
			'its a tv show
			Dim selectednode As Integer = TvTreeview.SelectedNode.Index
			tv_Rescrape_Show(WorkingTvShow)
			TvTreeview.SelectedNode = TvTreeview.Nodes(selectednode)
		Else
			'its an episode
			tv_Rescrape_Episode(WorkingTvShow, WorkingEpisode)
		End If
		If Not tv_IMDbID_warned And tv_IMDbID_detected Then
			MessageBox.Show(tv_IMDbID_detectedMsg, "TV Show ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
			tv_IMDbID_warned = True
		End If
	End Sub

	Private Sub Tv_TreeViewContext_RenameEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_RenameEp.Click

		Try
			Dim renamelog As String = ""
			Dim tempint As Integer = 0
			Dim oldname As String = ""
			Dim nfofilestorename As New List(Of String)
			nfofilestorename.Clear()
			Dim donelist As New List(Of String)
			donelist.Clear()
			If TvTreeview.SelectedNode.Name.IndexOf("\missing\") = -1 Then
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
					'individual episode
					tempint = MessageBox.Show("Using this option will rename the selected episode" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					If tempint = DialogResult.No Then
						Exit Sub
					End If
					If Not nfofilestorename.Contains(TvTreeview.SelectedNode.Name) And TvTreeview.SelectedNode.Name.IndexOf("Missing: ") <> 0 Then
						nfofilestorename.Add(TvTreeview.SelectedNode.Name)
					End If
				ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
					'season
					tempint = MessageBox.Show("Using this option will rename all episode nfo's within the selected season" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					If tempint = DialogResult.No Then
						Exit Sub
					End If
					Dim childnode As TreeNode
					For Each childnode In TvTreeview.SelectedNode.Nodes
						If Not nfofilestorename.Contains(childnode.Name) And childnode.Name.IndexOf("\missing\") = -1 Then
							nfofilestorename.Add(childnode.Name)
						End If
					Next
				ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
					'full show
					tempint = MessageBox.Show("Using this option will rename all episode nfo's within the selected show" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					If tempint = DialogResult.No Then
						Exit Sub
					End If
					Dim childnode As TreeNode
					Dim childchildnode As TreeNode
					For Each childnode In TvTreeview.SelectedNode.Nodes
						For Each childchildnode In childnode.Nodes
							If Not nfofilestorename.Contains(childchildnode.Name) And childchildnode.Name.IndexOf("\missing\") = -1 Then
								nfofilestorename.Add(childchildnode.Name)
							End If
						Next
					Next
				End If
			Else
				MsgBox("This is a Missing Episode, and can not be renamed!")
				Exit Sub
			End If

			messbox = New frmMessageBox("Renaming episodes,", "", "   Please Wait")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			If nfofilestorename.Count <= 0 Then
				messbox.Close()
				Exit Sub
			End If
			renamelog += "!!! " & nfofilestorename.Count & " nfo's to rename..." & vbCrLf & vbCrLf
			For Each renamefile In nfofilestorename
                'check episode still exists as we retrieve episode info from cache
                If Not File.Exists(renamefile) Then
                    renamelog += "!!! *** " & renamefile & vbCrLf & "video file does not exist at cached location" & vbCrLf & "Please Refresh All to update cache"
                    Continue For
                End If
                
				Dim seasonno As String = ""
				Dim episodetitle As String = ""
				Dim showtitle As String = ""
				Dim episodeno As New List(Of String)
				episodeno.Clear()
				For Each tvshow In Cache.TvCache.Shows
					Dim showpath As String = tvshow.NfoFilePath.Replace(Path.GetFileName(tvshow.NfoFilePath), "")
					If renamefile.IndexOf(showpath) <> -1 Then
						showtitle = Pref.RemoveIgnoredArticles(tvshow.Title.Value)
						For Each episode In tvshow.Episodes
							If episode.NfoFilePath = renamefile Then
								If seasonno = "" Then
									seasonno = episode.Season.Value
								End If
								If episodetitle = "" Then
									episodetitle = episode.Title.Value
								End If
								episodeno.Add(episode.Episode.Value)
							End If
						Next
						Dim newfilename As String
						newfilename = ""
						If seasonno.Length = 1 Then
							seasonno = "0" & seasonno
						End If
						For f = 0 To episodeno.Count - 1
							If episodeno(f).Length = 1 Then
								episodeno(f) = "0" & episodeno(f)
							End If
						Next
						newfilename = Renamer.setTVFilename(showtitle, episodetitle, episodeno, seasonno)

						newfilename = newfilename.Replace("?", "")
						newfilename = newfilename.Replace("/", "")
						newfilename = newfilename.Replace("\", "")
						newfilename = newfilename.Replace("<", "")
						newfilename = newfilename.Replace(">", "")
						newfilename = newfilename.Replace(":", "")
						newfilename = newfilename.Replace("""", "")
						newfilename = newfilename.Replace("*", "")
						If Pref.TvRenameReplaceSpace Then
							If Pref.TvRenameReplaceSpaceDot Then
								newfilename = newfilename.Replace(" ", ".")
							Else
								newfilename = newfilename.Replace(" ", "_")
							End If
						End If
                        newfilename = newfilename.TrimEnd
						Dim listtorename As New List(Of String)
						listtorename.Clear()
						listtorename.Add(renamefile)
						For Each ext In Utilities.VideoExtensions
							If ext = "VIDEO_TS.IFO" Then Continue For
							Dim temppath2 As String = renamefile.Replace(Path.GetExtension(renamefile), ext)
							If File.Exists(temppath2) Then
								listtorename.Add(temppath2)
							End If
						Next
						Dim di As DirectoryInfo = New DirectoryInfo(renamefile.Replace(Path.GetFileName(renamefile), ""))
						Dim filenama As String = Path.GetFileNameWithoutExtension(renamefile)
						Dim fils As FileInfo() = di.GetFiles(filenama & ".*")
						For Each fiNext In fils
							If Not listtorename.Contains(fiNext.FullName) Then
								listtorename.Add(fiNext.FullName)
							End If
						Next

						Dim temppath As String = renamefile
						temppath = temppath.Replace(Path.GetExtension(temppath), ".tbn")
						If File.Exists(temppath) Then
							If Not listtorename.Contains(temppath) Then
								listtorename.Add(temppath)
							End If
						End If

						temppath = temppath.Replace(Path.GetExtension(temppath), ".rar")
						If File.Exists(temppath) Then
							If Not listtorename.Contains(temppath) Then
								listtorename.Add(temppath)
							End If
						End If

						temppath = temppath.Replace(Path.GetExtension(temppath), "-thumb.jpg")
						If File.Exists(temppath) Then
							If Not listtorename.Contains(temppath) Then
								listtorename.Add(temppath)
							End If
						End If

						Dim oldnfofile As String = ""
						Dim newnfofile As String = ""
						For Each items In listtorename
							If Path.GetExtension(items).ToLower = ".nfo" And oldnfofile = "" Then
								oldnfofile = items
								newnfofile = items.Replace(Path.GetFileName(items), newfilename) & Path.GetExtension(items)
							End If
							Dim newname As String = items.Replace(filenama, newfilename)
                            Dim TempName As String = items.Replace(filenama, "TempFile")
							Try
								Dim pathsep As String = If(items.Contains("/"), "/", "\")
								Dim origpath As String = items.Substring(0, items.LastIndexOf(pathsep) + 1)
								renamelog += "!!! " & items.Replace(origpath, "") & "  -- to --  " & newname.Replace(origpath, "")
                                Dim fi As New FileInfo(items)
								If Not File.Exists(newname) Then
									fi.MoveTo(newname)
									If items.ToLower = tb_EpFilename.Text.ToLower Then tb_EpFilename.Text = fi.FullName.ToString
									renamelog += "  ---Succeeded" & vbCrLf
								Else
                                    'If current and new filesname are the same, but there are mis-matched casing, rename to temp file first
                                    'else they are the same.
                                    If String.Compare(items, newname, False) Then
                                        fi.MoveTo(TempName)
                                        fi.MoveTo(newname)
                                        If items.ToLower = tb_EpFilename.Text.ToLower Then tb_EpFilename.Text = fi.FullName.ToString
									    renamelog += "  ---Succeeded" & vbCrLf
                                    Else
                                        renamelog += " --! Not Renamed - Same" & vbCrLf
                                    End If
								End If
							Catch ex As Exception
								renamelog += "!!! *** Not Succeeded - Please rename all files manually!" & vbCrLf & "!!! Reported Message: " & ex.Message.ToString & vbCrLf
							End Try
						Next
						renamelog += "!!! " & vbCrLf
						renamelog += "!!! Updating Tables" & vbCrLf
						Try
							For Each noder2 In TvTreeview.Nodes
								If noder2.name = oldnfofile Then
									noder2.name = newnfofile
								End If
								For Each noder3 In noder2.nodes
									If noder3.name = oldnfofile Then
										noder3.name = newnfofile
									End If
									For Each noder4 In noder3.nodes
										If noder4.name = oldnfofile Then
											noder4.name = newnfofile
										End If
									Next
								Next
							Next
							For Each episode In tvshow.Episodes
								If episode.NfoFilePath = oldnfofile Then
									episode.NfoFilePath = newnfofile
								End If
							Next
							renamelog += "!!! Tables Updated" & vbCrLf & vbCrLf
						Catch
							renamelog += "!!! Failed to update tables, use 'Refresh TV Shows' menu item to fix" & vbCrLf & vbCrLf
						End Try
					End If
				Next
			Next
			Call Tv_CacheSave()
			messbox.Close()
			If Pref.disabletvlogs Then
				Dim MyFormObject As New frmoutputlog(renamelog, True)
				Try
					MyFormObject.ShowDialog()
				Catch ex As ObjectDisposedException

				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_RescrapeWizard_Click(sender As System.Object, e As System.EventArgs) Handles Tv_TreeViewContext_RescrapeWizard.Click
		Try
			oTV.SingleShow = True
            oTV.SingleShowid = tv_ShowSelectedCurrently(TvTreeview).TvdbId.Value
            singleshow = True
			TV_BatchRescrapeWizardToolStripMenuItem.PerformClick()
			While BckWrkTv.IsBusy OrElse tvbckrescrapewizard.IsBusy
				Application.DoEvents()
			End While
            singleshow = False
			oTV.SingleShow = False
            oTV.SingleShowid = Nothing
		Catch ex As Exception
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_RescrapeMediaTags_Click(sender As System.Object, e As System.EventArgs) Handles Tv_TreeViewContext_RescrapeMediaTags.Click
		Try
			Dim tmp As Integer = Utilities.languagelibrary.count
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim tempint As Integer = 0
			Dim nfofilestorename As New List(Of String)
			nfofilestorename.Clear()
			Dim donelist As New List(Of String)
			donelist.Clear()
			If TvTreeview.SelectedNode.Name.IndexOf("\missing\") = -1 Then
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
					'individual episode
					If Not nfofilestorename.Contains(TvTreeview.SelectedNode.Name) And TvTreeview.SelectedNode.Name.IndexOf("\missing\") = -1 Then
						nfofilestorename.Add(TvTreeview.SelectedNode.Name)
					End If
				ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
					'season
					tempint = MessageBox.Show("This option will Rescrape Media tags for the selected season" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					If tempint = DialogResult.No Then
						Exit Sub
					End If
					Dim childnode As TreeNode
					For Each childnode In TvTreeview.SelectedNode.Nodes
						If Not nfofilestorename.Contains(childnode.Name) And childnode.Name.IndexOf("\missing\") = -1 Then
							nfofilestorename.Add(childnode.Name)
						End If
					Next
				ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
					'full show
					tempint = MessageBox.Show("This option will Rescrape Media tags for the selected show" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
					If tempint = DialogResult.No Then
						Exit Sub
					End If
					Dim childnode As TreeNode
					Dim childchildnode As TreeNode
					For Each childnode In TvTreeview.SelectedNode.Nodes
						For Each childchildnode In childnode.Nodes
							If Not nfofilestorename.Contains(childchildnode.Name) And childchildnode.Name.IndexOf("\missing\") = -1 Then
								nfofilestorename.Add(childchildnode.Name)
							End If
						Next
					Next
				End If
			End If

			messbox = New frmMessageBox("Getting Media Tags for episodes,", "", "   Please Wait")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			If nfofilestorename.Count <= 0 Then
				messbox.Close()
				Exit Sub
			End If

			For Each nfo In nfofilestorename
				Try
					Dim ThisEp As New List(Of TvEpisode)
					ThisEp.Clear()
					ThisEp = WorkingWithNfoFiles.ep_NfoLoad(nfo)
					For h = ThisEp.Count - 1 To 0 Step -1
						Dim fileStreamDetails As StreamDetails = Pref.Get_HdTags(Utilities.GetFileName(ThisEp(h).VideoFilePath))
						ThisEp(h).StreamDetails.Video = fileStreamDetails.Video
						ThisEp(h).StreamDetails.Audio.Clear()
						For Each audioStream In fileStreamDetails.Audio
							ThisEp(h).StreamDetails.Audio.Add(audioStream)
						Next
						ThisEp(h).StreamDetails.Subtitles.Clear()
						For each langStream In fileStreamDetails.Subtitles
							ThisEp(h).StreamDetails.Subtitles.Add(langStream)
						Next
						If ThisEp(h).StreamDetails.Video.DurationInSeconds.Value <> Nothing Then
							Try
								Dim tempstring As String
								tempstring = ThisEp(h).StreamDetails.Video.DurationInSeconds.Value
								If Pref.intruntime Then
									ThisEp(h).Runtime.Value = Math.Round(tempstring / 60).ToString
								Else
									ThisEp(h).Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
								End If
							Catch
							End Try
						End If
					Next
					WorkingWithNfoFiles.ep_NfoSave(ThisEp, ThisEp(0).NfoFilePath)
				Catch ex As Exception
					messbox.Close()
					ExceptionHandler.LogError(ex)
				End Try
			Next
			messbox.Close()
			tv_CacheRefresh(WorkingTvShow)
		Catch ex As Exception
			messbox.Close()
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_MissingEpThumbs_Click(sender As System.Object, e As System.EventArgs) Handles Tv_TreeViewContext_MissingEpThumbs.Click
		Try
			Dim tmp As Integer = Utilities.languagelibrary.count
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim tempint As Integer = 0
			Dim seasonnumber As Integer = -1
			Dim nfofilestorename As New List(Of String)
			nfofilestorename.Clear()
			Dim donelist As New List(Of String)
			donelist.Clear()
			If WorkingTvShow.Episodes.Count = 0 Then
				MsgBox("No Episodes in this Show")
				Exit Sub
			End If
			If TvTreeview.SelectedNode.Name.IndexOf("\missing\") = -1 Then
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
					Dim Thisseason As TvSeason = TvTreeview.SelectedNode.Tag
					seasonnumber = Thisseason.SeasonNumber
				End If
			End If
			messbox = New frmMessageBox("Scanning for Missing episode thumbnails,", "and downloading if available.", "   Please Wait")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()

			Dim tvdbstuff As New TVDBScraper
			Dim tvseriesdata As New Tvdb.ShowData
			Dim language As String = WorkingTvShow.Language.Value
			If language = "" Then language = "en"
			tvseriesdata = tvdbstuff.GetShow(WorkingTvShow.TvdbId.Value, language, Utilities.SeriesXmlPath)

			For Each ep As TvEpisode In WorkingTvShow.Episodes
				If ep.IsMissing Then Continue For
				If Not seasonnumber = -1 AndAlso ep.Season.Value <> seasonnumber.ToString Then Continue For
				Dim Episodedata As New Tvdb.Episode
				Dim epfound As Boolean = False
				If Not tvseriesdata.FailedLoad Then
					For Each NewEpisode As Tvdb.Episode In tvseriesdata.Episodes
						If Not String.IsNullOrEmpty(ep.UniqueId.Value) Then
							If NewEpisode.Id.Value = ep.UniqueId.Value Then epfound = True
						ElseIf NewEpisode.SeasonNumber.Value = ep.Season.Value
							If NewEpisode.EpisodeNumber.Value = ep.Episode.Value Then epfound = True
						End If
						If epfound Then
							Episodedata = NewEpisode
							Episodedata.ThumbNail.Value = "http://www.thetvdb.com/banners/" & NewEpisode.ThumbNail.value
							Exit For
						End If
					Next
				End If
				If Not epfound Then
					Dim sortorder As String = WorkingTvShow.SortOrder.Value
					If sortorder = "" Then sortorder = "default"
					Dim tvdbid As String = WorkingTvShow.TvdbId.Value
					Dim imdbid As String = WorkingTvShow.ImdbId.Value
					Dim seasonno As String = ep.Season.Value
					Dim episodeno As String = ep.Episode.Value
					Episodedata = tvdbstuff.getepisodefromxml(tvdbid, sortorder, seasonno, episodeno, language, True)
					If Episodedata.FailedLoad Then Continue For
				End If
				Dim epdata As New TvEpisode
				epdata.AbsorbTvdbEpisode(Episodedata)
				epdata.NfoFilePath = ep.NfoFilePath
				epdata.VideoFilePath = ep.VideoFilePath
				epdata.Thumbnail.Url = Episodedata.ThumbNail.Value
				tv_EpisodeFanartGet(epdata, True, False)
			Next
			messbox.Close()
		Catch ex As Exception
			messbox.Close()
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tv_PosterSetup(Optional ByVal IsOfType As String = "")
		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim WorkingSeason As TvSeason = tv_SeasonSelectedCurrently(TvTreeview)
		tvposterpage = 0
		imdbposterlist.Clear()
		tvdbposterlist.Clear()
		rbTVposter.Checked = True
		rbTVposter.Enabled = False
		rbTVbanner.Enabled = False
		ComboBox2.Items.Clear()
		tvobjects.Clear()
		TextBox31.Text = WorkingTvShow.Title.Value
		Label72.Text = ""
		tv_PosterPanelClear()
		ComboBox2.Items.Add("Main Image")
		ComboBox2.Items.Add("Season All")
		For Each tvshow In Cache.TvCache.Shows
			If tvshow.TvdbId = WorkingTvShow.TvdbId Then
				For Each Season As Media_Companion.TvSeason In tvshow.Seasons.Values
					For Each ep As Media_Companion.TvEpisode In Season.Episodes
						Dim seasonstring As String = ""
						If ep.Season.Value < 1 Then
							seasonstring = "Specials"
						Else
							seasonstring = "Season " & Utilities.PadNumber(ep.Season.Value.ToString, 2)
						End If
						If Not ComboBox2.Items.Contains(seasonstring) Then
							ComboBox2.Items.Add(seasonstring)
						End If
					Next
				Next
				Exit For
			End If
		Next
		ComboBox2.SelectedIndex = 0

		If Not WorkingSeason is Nothing then
			Dim ThisSeason As String = WorkingSeason.ToString
			If ThisSeason = "Season 00" then ThisSeason = "Specials"
			For i = 0 to ComboBox2.Items.Count
				ComboBox2.SelectedIndex = i
				If ComboBox2.text = ThisSeason Then
					If IsOfType = "banner" Then rbTVbanner.Checked = True
					Exit For
				End If
			Next
		End If
	End Sub

	Public Function BannerAndPosterViewer()
		Try
			Me.Panel16.Hide()
			Label72.Text = ""
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			rbTVposter.Enabled = True
			rbTVbanner.Enabled = True
			btnTvPosterTVDBSpecific.Enabled = True
			Dim eden As Boolean = Pref.EdenEnabled
			Dim frodo As Boolean = Pref.FrodoEnabled
			Dim edenpath As String = ""
			Dim frodopath As string = ""
			Dim tempstring As String = ComboBox2.SelectedItem
			Dim PresentImage As String = ""
			Dim defimg As String = ""
			Dim path As String = ""
			EdenImageTrue.Visible = False
			FrodoImageTrue.Visible = False
			If tempstring = "Main Image" Then
				If eden Then
					path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "folder.jpg")
					edenpath = path
				End If
				If frodo Then
					If rbTVbanner.Checked = True Then
						path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "banner.jpg")
						frodopath = path
					ElseIf rbTVposter.Checked = True Then
						path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "poster.jpg")
						frodopath = path
					End If
				End If
			ElseIf tempstring = "Specials" Then
				If eden Then
					path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season-specials.tbn")
					edenpath = path
				End If
				If frodo Then
					If rbTVbanner.Checked = True Then
						path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season-specials-banner.jpg")
						frodopath = path
					ElseIf rbTVposter.Checked = True Then
						path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season-specials-poster.jpg")
						frodopath = path
					End If
				End If
			ElseIf tempstring.IndexOf("Season") = 0 And tempstring.IndexOf("Season All") = -1 Then
				path = tempstring.Replace("Season ", "")
				path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season" & path & ".tbn")
				edenpath = path
				If frodo Then
					If rbTVbanner.Checked = True Then
						path = path.Replace(".tbn", "-banner.jpg")
						frodopath = path
					ElseIf rbTVposter.Checked = True Then
						path = path.Replace(".tbn", "-poster.jpg")
						frodopath = path
					End If
				End If
			ElseIf tempstring = "Season All" Then
				btnTvPosterTVDBSpecific.Enabled = False
				If eden Then
					path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season-all.tbn")
					edenpath = path
				End If
				If frodo Then
					If rbTVbanner.Checked = True Then
						path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season-all-banner.jpg")
						frodopath = path
					ElseIf rbTVposter.Checked = True Then
						path = WorkingTvShow.NfoFilePath.Replace("tvshow.nfo", "season-all-poster.jpg")
						frodopath = path
					End If
				End If
			End If

			If (eden and File.Exists(edenpath)) or (frodo and File.Exists(frodopath)) Then
				EdenImageTrue.Visible = False
				EdenImageTrue.Text = "Eden Image Present"
				FrodoImageTrue.Visible = False
				FrodoImageTrue.Text = "Frodo Image Present"
				ArtMode.Text = ""
				If eden Then
					EdenImageTrue.Visible = True
					ArtMode.Text = "Pre-Frodo Enabled"
				End If
				If frodo Then
					FrodoImageTrue.Visible = True
					ArtMode.Text = "Frodo Enabled"
				End If
				If frodo and eden then
					ArtMode.text = "Both Enabled"
					EdenImageTrue.Visible = True
					FrodoImageTrue.Visible = True
				End If
				If File.Exists(edenpath) then
					PresentImage = edenpath
					EdenImageTrue.Text = "Eden Image Present"
				Else
					EdenImageTrue.Text = "No Eden Image"
				End If
				If File.Exists(frodopath) then
					PresentImage = frodopath
					FrodoImageTrue.Text = "Frodo Image Present"
				Else
					FrodoImageTrue.Text = "No Frodo Image"
				End If
			Else
				If rbTVbanner.Checked = True Then
					defimg = Utilities.DefaultBannerPath
					If eden and not frodo then
						EdenImageTrue.Text = "No Eden Image"
						EdenImageTrue.Visible = True
						FrodoImageTrue.Visible = False
					ElseIf frodo and Not eden then
						FrodoImageTrue.Text = "No Frodo Image"
						FrodoImageTrue.Visible = True
						EdenImageTrue.Visible = False
					ElseIf frodo and eden then
						EdenImageTrue.Text = "No Eden Image"
						EdenImageTrue.Visible = True
						FrodoImageTrue.Text = "No Frodo Image"
						FrodoImageTrue.Visible = True
					End If
				Else
					defimg = Utilities.DefaultPosterPath
					If eden and not frodo then
						EdenImageTrue.Text = "No Eden Image"
						EdenImageTrue.Visible = True
						FrodoImageTrue.Visible = False
					ElseIf frodo and Not eden then
						FrodoImageTrue.Text = "No Frodo Image"
						FrodoImageTrue.Visible = True
						EdenImageTrue.Visible = False
					ElseIf frodo and eden then
						EdenImageTrue.Text = "No Eden Image"
						EdenImageTrue.Visible = True
						FrodoImageTrue.Text = "No Frodo Image"
						FrodoImageTrue.Visible = True
					End If
				End If
			End If
			util_ImageLoad(PictureBox12, PresentImage, If(rbTVbanner.Checked, Utilities.DefaultTvBannerPath, Utilities.DefaultTvPosterPath))
			If rbTVbanner.Checked = True Then
				Label73.Text = "Current Banner - " & PictureBox12.Image.Width.ToString & " x " & PictureBox12.Image.Height.ToString
			Else
				Label73.Text = "Current Poster - " & PictureBox12.Image.Width.ToString & " x " & PictureBox12.Image.Height.ToString
			End If

			Return 0
		Catch ex As Exception
			Return 0
			ExceptionHandler.LogError(ex)
		End Try
	End Function

	Private Sub tv_TvdbThumbsGet()

		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim showlist As New XmlDocument
		Dim tvdbstuff As New TVDBScraper
		Dim thumblist As String = tvdbstuff.GetPosterList(WorkingTvShow.TvdbId.Value, tvdbposterlist)
		If thumblist <> "ok" Then MsgBox(thumblist, MsgBoxStyle.OkOnly, "TVdb site returned.....")
	End Sub

	Private Sub tv_PosterPanelPopulate()
		Me.Panel16.Show()
		tvposterpage = 1
		If usedlist.Count <= 0 Then
			Label72.Text = "Displaying 0 of 0 Images"
			Label72.Visible = True
			btnTvPosterNext.Visible = False
			btnTvPosterPrev.Visible = False
			Call tv_PosterPanelClear()
			Exit Sub
		End If
		If Not rbTVbanner.Checked Then
			If usedlist.Count > 10 Then
				btnTvPosterNext.Visible = True
				btnTvPosterPrev.Visible = True
				If usedlist.Count >= 10 Then
					Label72.Text = "Displaying 1 to 10 of " & usedlist.Count.ToString & " Images"
				Else
					Label72.Text = "Displaying 1 to " & usedlist.Count.ToString & " of " & usedlist.Count.ToString & " Images"
				End If
				Label72.Visible = True
				Me.Refresh()
				Application.DoEvents()
				btnTvPosterPrev.Enabled = False
				btnTvPosterNext.Enabled = True
			Else
				btnTvPosterNext.Visible = False
				btnTvPosterPrev.Visible = False
				If posterArray.Count >= 10 Then
					Label72.Text = "Displaying 1 to 10 of " & usedlist.Count.ToString & " Images"
				Else
					Label72.Text = "Displaying 1 to " & usedlist.Count.ToString & " of " & usedlist.Count.ToString & " Images"
				End If
				Label72.Visible = True
				Me.Refresh()
				Application.DoEvents()
			End If
		Else
			btnTvPosterNext.Visible = False
			btnTvPosterPrev.Visible = False
			Label72.Text = "Displaying 1 to " & usedlist.Count.ToString
		End If
		Call tv_PosterSelectionDisplay()
	End Sub

	Private Sub tv_PosterPanelClear()
		For i = Panel16.Controls.Count - 1 To 0 Step -1
			Panel16.Controls.RemoveAt(i)
		Next
	End Sub

	Private Sub tv_PosterSelectionDisplay()
		tv_PosterPanelClear()
		Dim tempint As Integer = ((tvposterpage * 10) + 1) - 10
		Dim tempint2 As Integer = tvposterpage * 10

		If tempint2 > usedlist.Count Then
			tempint2 = usedlist.Count
		End If

		If Not rbTVbanner.Checked Then
			Label72.Text = "Displaying " & tempint.ToString & " to " & tempint2 & " of " & usedlist.Count.ToString & " Images"
		Else
			Label72.Text = "Displaying 1 to " & usedlist.Count.ToString
			tempint2 = usedlist.Count
		End If

		Dim locationX As Integer = 0
		Dim locationY As Integer = 0
		Dim itemcounter As Integer = 0
		Dim tempboolean As Boolean = True
		Dim MovFanartPicBox As New List(Of FanartPicBox)
		If rbTVposter.Checked = True Or rbTVbanner.Enabled = False Then
			For f = tempint - 1 To tempint2 - 1
				Dim thispicbox As New FanartPicBox
				tvposterpicboxes() = New PictureBox()
				With tvposterpicboxes
					.Location = New Point(locationX, locationY)
					.Width = 123
					.Height = 168
					.SizeMode = PictureBoxSizeMode.Zoom
					.Tag = usedlist(f).Url
					.Visible = True
					.BorderStyle = BorderStyle.Fixed3D
					.Name = "poster" & itemcounter.ToString
					AddHandler tvposterpicboxes.DoubleClick, AddressOf tv_PosterDoubleClick
					'AddHandler tvposterpicboxes.LoadCompleted, AddressOf imageres
				End With
				thispicbox.pbox = tvposterpicboxes
				thispicbox.imagepath = usedlist(f).SmallUrl
				MovFanartPicBox.Add(thispicbox)

				tvpostercheckboxes() = New RadioButton()
				With tvpostercheckboxes
					.Location = New Point(locationX + 19, locationY + 166) '179
					.Width = 79
					.Height = 32
					.Name = "postercheckbox" & itemcounter.ToString
					.SendToBack()
					.CheckAlign = ContentAlignment.TopCenter
					If usedlist(f).Resolution = "season" Then
						.Text = " "
					ElseIf usedlist(f).Resolution <> "" Then
						.Text = usedlist(f).Resolution
					Else
						.Text = "?"
					End If
					.TextAlign = ContentAlignment.BottomCenter
					.Tag = usedlist(f).Url
					AddHandler tvpostercheckboxes.CheckedChanged, AddressOf tv_PosterRadioChanged
				End With

				itemcounter += 1
				Me.Panel16.Controls.Add(tvposterpicboxes())
				Me.Panel16.Controls.Add(tvpostercheckboxes())
				Application.DoEvents()
				If tempboolean = True Then
					locationY = (192 + 19)
				Else
					locationX += 120
					locationY = 0
				End If
				tempboolean = Not tempboolean
			Next
		Else
			For f = tempint - 1 To tempint2 - 1
				Dim thispicbox As New FanartPicBox
				tvposterpicboxes() = New PictureBox()
				With tvposterpicboxes
					.Location = New Point(0, locationY)
					.Width = 600
					.Height = 114
					.SizeMode = PictureBoxSizeMode.Zoom
					.Tag = usedlist(f).Url
					.Visible = True
					.BorderStyle = BorderStyle.Fixed3D
					.Name = "poster" & itemcounter.ToString
					AddHandler tvposterpicboxes.DoubleClick, AddressOf tv_PosterDoubleClick
					'AddHandler tvposterpicboxes.LoadCompleted, AddressOf imageres
				End With
				thispicbox.pbox = tvposterpicboxes
				thispicbox.imagepath = usedlist(f).Url
				MovFanartPicBox.Add(thispicbox)

				tvpostercheckboxes() = New RadioButton()
				With tvpostercheckboxes
					.Location = New Point(290, locationY + 110)
					.Name = "postercheckbox" & itemcounter.ToString
					.SendToBack()
					.Text = " "
					.Tag = usedlist(f).Url
					AddHandler tvpostercheckboxes.CheckedChanged, AddressOf tv_PosterRadioChanged
				End With
				itemcounter += 1
				locationY += 140

				Me.Panel16.Controls.Add(tvposterpicboxes())
				Me.Panel16.Controls.Add(tvpostercheckboxes())
				Application.DoEvents()
			Next
		End If
		Me.Panel16.Refresh()
		Me.Refresh()
		If MovFanartPicBox.Count > 0 Then
			messbox.Close()
			If Not ImgBw.IsBusy Then
				ToolStripStatusLabel2.Text = "Starting Download of Images..."
				ToolStripStatusLabel2.Visible = True
				ImgBw.RunWorkerAsync({MovFanartPicBox, tempint, tempint2, Me.Panel16})
			End If
		End If
		Me.Panel16.Refresh()
		Me.Refresh()
		Application.DoEvents()
		If rbTVbanner.Checked AndAlso Me.Panel16.Controls.Count > 0 Then EnableTvBannerScrolling
		Me.Refresh()
	End Sub

	Private Sub tv_PosterRadioChanged(ByVal sender As Object, ByVal e As EventArgs)

		Dim tempstring As String = sender.name
		Dim tempint As Integer = 0
		Dim tempstring2 As String = tempstring
		Dim allok As Boolean = False
		tempstring = tempstring.Replace("postercheckbox", "")
		tempint = Convert.ToDecimal(tempstring)
		Dim hires(1)
		Dim lores(1)
		lores(0) = ""
		hires(0) = ""
		lores(1) = ""
		hires(1) = ""
		For Each cont As Control In Me.Panel16.Controls()
			If cont.Name.Replace("poster", "") = tempint.ToString Then
				Dim picbox As PictureBox = cont
				lores(0) = "Save Image (" & picbox.Image.Width & " x " & picbox.Image.Height & ")"
				lores(1) = picbox.Name
				For Each poster In usedlist
					If poster.url = sender.tag Then
						If Not IsNothing(poster.Resolution) AndAlso IsNumeric(poster.resolution.Replace("x", "")) Then
							hires(0) = "Save Image (" & poster.resolution & ")"
							hires(0) = hires(0).replace("x", " x ")
						Else
							hires(0) = "Save Image (Hi-Res)"
						End If
						hires(1) = poster.url
						Exit For
					End If
				Next
				allok = True
				Exit For
			End If
		Next

		If allok = True Then
			btnTvPosterSaveBig.Text = hires(0)
			btnTvPosterSaveBig.Visible = True
			btnTvPosterSaveBig.Tag = hires(1)
		Else
			btnTvPosterSaveBig.Visible = False
		End If
	End Sub

	Private Sub tv_PosterDoubleClick(ByVal sender As Object, ByVal e As EventArgs)
		Dim tempstring As String = sender.name.replace("poster", "postercheckbox")
		Dim hdurl As String = ""
		For Each Control In Panel16.Controls
			If Control.name = tempstring Then
				Dim rb As RadioButton = Control
				rb.Checked = True
				hdurl = rb.Tag.ToString
			End If
		Next
		messbox = New frmMessageBox("Please wait,", "", "Downloading Full Image")
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
		messbox.Show()
		Me.Refresh()
		messbox.Refresh()
		If hdurl = "" Then hdurl = sender.tag.tostring
		Dim cachefile As String = Utilities.Download2Cache(hdurl)
		Call util_ZoomImage(cachefile)
		messbox.Close()
	End Sub

	Private Sub TvPosterSave(ByVal imageUrl As String)
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		Try
			Dim witherror As Boolean = False
			Dim witherror2 As Boolean = False
			Dim postpath As String = ""
			Dim eden As Int16 = 0
			Dim frodo As Int16 = 0
			Dim imagePaths As New List(Of String)
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim workingposterpath = WorkingTvShow.FolderPath & "folder.jpg"
			If ComboBox2.Text.ToLower = "main image" Then
				If (Pref.EdenEnabled OrElse Pref.tvfolderjpg) And Not (Pref.FrodoEnabled AndAlso rbTVbanner.Checked) Then
					imagePaths.Add(workingposterpath)
					eden = 1
				End If
				If Pref.FrodoEnabled Then
					If rbTVbanner.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "banner.jpg"))
						frodo = 1
					ElseIf rbTVposter.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "poster.jpg"))
						frodo = 1
					End If
				End If
				If Pref.tvfolderjpg AndAlso rbTVposter.Checked Then
					imagePaths.Add(workingposterpath)
				End If
			ElseIf ComboBox2.Text.ToLower.IndexOf("season") <> -1 And ComboBox2.Text.ToLower.IndexOf("all") = -1 Then
				Dim temp As String = ComboBox2.Text.ToLower
				temp = temp.Replace(" ", "")
				If Pref.EdenEnabled Then
					imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), temp & ".tbn"))
					eden = 1
				End If
				If Pref.FrodoEnabled Then
					If rbTVbanner.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), temp & "-banner.jpg"))
						frodo = 1
					ElseIf rbTVposter.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), temp & "-poster.jpg"))
						frodo = 1
					End If
				End If
				If Pref.seasonfolderjpg Then
					temp = temp.Replace("season", "")
					Dim seasonno As Integer = temp.ToInt
					Dim seasonpath As String = Nothing
					For Each ep As TvEpisode In WorkingTvShow.Episodes
						If ep.Season.Value = seasonno Then
							seasonpath = ep.FolderPath.Replace(WorkingTvShow.FolderPath, "")
							Exit For
						End If
					Next
					If Not IsNothing(seasonpath) Then imagePaths.Add(WorkingTvShow.FolderPath & seasonpath & "folder.jpg")
				End If
			ElseIf ComboBox2.Text.ToLower.IndexOf("season") <> -1 And ComboBox2.Text.ToLower.IndexOf("all") <> -1 Then
				If Pref.EdenEnabled Then
					If Pref.seasonall = "poster" and rbTVposter.Checked Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-all.tbn"))
						eden = 1
					ElseIf Pref.seasonall = "wide" and rbTVbanner.Checked Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-all.tbn"))
						eden = 1
					End If
				End If
				If Pref.FrodoEnabled Then
					If rbTVbanner.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-all-banner.jpg"))
						frodo = 1
					ElseIf rbTVposter.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-all-poster.jpg"))
						frodo = 1
					End If
				End If
			ElseIf ComboBox2.Text.ToLower = "specials" Then
				If Pref.EdenEnabled Then
					imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-specials.tbn"))
					eden = 1
				End If
				If Pref.FrodoEnabled Then
					If rbTVbanner.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-specials-banner.jpg"))
						frodo = 1
					ElseIf rbTVposter.Checked = True Then
						imagePaths.Add(workingposterpath.Replace(Path.GetFileName(workingposterpath), "season-specials-poster.jpg"))
						frodo = 1
					End If
				End If
				If Pref.seasonfolderjpg AndAlso rbTVposter.Checked Then
					Dim seasonpath As String = ""
					For Each seas In WorkingTvShow.Seasons.Values
						If seas.SeasonNumber = 0 Then
							seasonpath = seas.FolderPath
							Exit For
						End If
					Next
					If seasonpath <> "" Then
						imagePaths.Add(seasonpath & "folder.jpg")
					End If
				End If
			End If
			messbox = New frmMessageBox("Please wait,", "", "Downloading Full Resolution Image")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
			Try
				If Not IsNothing(imageUrl) Then
					witherror = Not DownloadCache.SaveImageToCacheAndPaths(imageUrl, ImagePaths, True)
					If Not witherror Then
						postpath = imagePaths(0)
						If rbTVbanner.Checked Then
							util_ImageLoad(tv_PictureBoxBottom, postpath, Utilities.DefaultTvBannerPath)
						End If
						If rbTVposter.Checked Then
							util_ImageLoad(tv_PictureBoxRight, postpath, Utilities.DefaultTvPosterPath)
						End If
						util_ImageLoad(PictureBox12, postpath, Utilities.DefaultTvPosterPath)
						Label73.Text = "Current Poster - " & PictureBox12.Image.Width.ToString & " x " & PictureBox12.Image.Height.ToString
					End If
				End If
				If witherror = True Then
					MsgBox("Unable to download image")
				Else
					If eden = 1 then
						EdenImageTrue.Visible = True
						EdenImageTrue.Text = "Eden Image Present"
					End if
					If frodo = 1 then
						FrodoImageTrue.Visible = True
						FrodoImageTrue.Text = "Frodo Image Present"
					End if
				End If
			Catch ex As Exception
				MsgBox(ex.ToString)
			Finally
				messbox.Close()
			End Try
			If ComboBox2.Text.ToLower = "main image" AndAlso rbtvposter.Checked Then
				Dim popath As String = Utilities.save2postercache(WorkingTvShow.NfoFilePath, WorkingTvShow.ImagePoster.Path, WallPicWidth, WallPicHeight)
				updateTvPosterWall(popath, WorkingTvShow.NfoFilePath)
			End If
			postpath = ""
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RefreshMovieNfoFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshMovieNfoFilesToolStripMenuItem.Click
		Try
			Call Batch_Rewritenfo()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

#Region "Media Info Export"
	Dim exportMovieInfo As Boolean = False  'these are used to allow only a single execution of media export functions
	Dim exportTVInfo As Boolean = False     'when there may be mulitple drop-down events. (Found that out the hard way!)

	Private Sub ExportMovieListInfoToolStripMenuItem_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ExportMovieListInfoToolStripMenuItem.DropDownItemClicked
		If Not exportMovieInfo Then exportMovieInfo = mediaInfoExp.setTemplate(e.ClickedItem.Text)
	End Sub

	Private Sub ExportTVShowInfoToolStripMenuItem_DropDownItemClicked(sender As Object, e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ExportTVShowInfoToolStripMenuItem.DropDownItemClicked
		If Not exportTVInfo Then exportTVInfo = mediaInfoExp.setTemplate(e.ClickedItem.Text)
	End Sub

	Private Sub ExportMovieListInfoToolStripMenuItem_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExportMovieListInfoToolStripMenuItem.DropDownClosed
		If exportMovieInfo Then
			exportMovieInfo = False
			Call util_ExportMediaListInfo(MediaInfoExport.mediaType.Movie)
		End If
	End Sub

	Private Sub ExportTVShowInfoToolStripMenuItem_DropDownClosed(sender As Object, e As System.EventArgs) Handles ExportTVShowInfoToolStripMenuItem.DropDownClosed
		If exportTVInfo = True Then
			exportTVInfo = False
			Call util_ExportMediaListInfo(MediaInfoExport.mediaType.TV)
		End If
	End Sub

	Private Sub util_ExportMediaListInfo(ByVal mediaType As String)
		Dim savepath As String
		Dim extensions As New Dictionary(Of String, String)
		extensions.Add("html", "Html Documents (*.html)|*.html")
		extensions.Add("xml", "XML Data (*.xml)|*.xml")
		extensions.Add("csv", "CSV (Comma delimited) (*.csv)|*.csv")
		Dim ext As String = mediaInfoExp.getPossibleFileType
		Dim idx As Integer
		For idx = 1 To extensions.Count
			If extensions.Keys(idx - 1) = ext Then Exit For
		Next
		With SaveFileDialog1
			.DefaultExt = ext
			.Filter = String.Join("|", extensions.Values)
			.FilterIndex = idx
			.Title = "ExportMedia File"
			.OverwritePrompt = True
			.CheckPathExists = True
			If mediaInfoExp.workingTemplate.FileName <> "" Then
				.FileName = mediaInfoExp.workingTemplate.FileName
			End If
		End With
		If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
			savepath = SaveFileDialog1.FileName
			Dim mediaCollection As Object
			If mediaType = MediaInfoExport.mediaType.Movie Then
				'Dirty hack to get the media export to use the data grid source. Wasn't as straight forward as hoped,
				'and after spending many hours trying to find an elegant solution, I gave up.
				'If anyone comes across this and thinks "Huey, you twat, just do it like this", then please go right
				'ahead! - HueyHQ 15Feb13
				Dim mediaList As New List(Of ComboList)
                If DataGridViewMovies.SelectedRows.Count > 1 Then
                    For each item As DataGridViewRow In DataGridViewMovies.SelectedRows
                        Dim movnfo As String = item.Cells("fullpathandfilename").Value.ToString
                        Dim q = From x In oMovies.MovieCache Where x.fullpathandfilename = movnfo
                        mediaList.Add(q(0))
                    Next
                Else
                    For Each mediaItem As Data_GridViewMovie In DataGridViewMovies.DataSource
					    mediaList.Add(mediaItem.Export(oMovies))
				    Next
                End If
				
				mediaCollection = mediaList
			Else
				mediaCollection = Cache.TvCache.Shows
			End If
			mediaInfoExp.createDocument(savepath, mediaCollection)
		End If
	End Sub

#End Region  'Media Info Export

#Region "Episode Backgroundworker - obsolete"
	'Private Sub bckgroundscanepisodes_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs, Optional ByVal manual As Boolean = False) Handles bckgroundscanepisodes.DoWork
	'	Try
	'		Dim List As List(Of TvShow) = e.Argument(0)
	'		Dim Force As Boolean = e.Argument(1)
	'		Statusstrip_Enable()

	'		Call TV_EpisodeScraper(List, Force)

	'	Catch ex As Exception
	'		ExceptionHandler.LogError(ex)
	'	End Try
	'End Sub

	'Private Sub bckgroundscanepisodes_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bckgroundscanepisodes.ProgressChanged
	'	Try
	'		If e.ProgressPercentage = 0 Then
	'			ToolStripStatusLabel6.Text = e.UserState
	'			ToolStripStatusLabel6.Visible = True
	'		ElseIf e.ProgressPercentage = 1 Then
	'			If TypeOf e.UserState Is TvEpisode Then
	'				Dim TempEpisode As TvEpisode = CType(e.UserState, TvEpisode)

	'				TempEpisode.ShowObj.AddEpisode(TempEpisode)
	'				TempEpisode.SeasonObj.UpdateTreenode()
	'				TempEpisode.UpdateTreenode(True)
	'				'This bit updates the Epsiode Count on the fly when the progress is updated. It has to be done here to avoid thread issues. (GUI wouldn't update properly) 
	'				TextBox_TotEpisodeCount.Text = Cache.TvCache.Episodes.Count
	'			End If
	'		End If
	'	Catch ex As Exception
	'		ExceptionHandler.LogError(ex)
	'	End Try
	'End Sub

	'Private Sub bckgroundscanepisodes_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bckgroundscanepisodes.RunWorkerCompleted
	'	Try
	'		If scrapeAndQuit = True Then
	'			sandq = sandq - 1
	'			Exit Sub
	'		End If

	'		If Not (e.Error Is Nothing) Then
	'			tvScraperLog = tvScraperLog & vbCrLf
	'			tvScraperLog = tvScraperLog & "!!! Error, exiting TV episode scraper" & vbCrLf
	'			tvScraperLog = tvScraperLog & "Error:-" & vbCrLf
	'			tvScraperLog = tvScraperLog & e.Error.ToString & vbCrLf
	'		Else
	'			tvScraperLog = tvScraperLog & vbCrLf & "!!! Operation Completed" & vbCrLf
	'		End If

	'		ToolStripStatusLabel6.Text = "TV Show Scraper"
	'		ToolStripStatusLabel6.Visible = False
	'		StatusStrip1.Visible = BckWrkScnMovies.IsBusy OrElse Not Pref.AutoHideStatusBar
	'		If Not BckWrkScnMovies.IsBusy Then StatusStrip1.BackColor = Color.LightGray
	'		tsStatusLabel1.Visible = Not BckWrkScnMovies.IsBusy
	'		btnTvSearchNew.Text = "Search New"
            
	'		Tv_CacheSave()
	'		tv_CacheLoad()
	'		tv_Filter()
	'		If (Not TvAutoScrapeTimerTripped AndAlso Pref.disabletvlogs) Or ScraperErrorDetected Then
	'			Dim MyFormObject As New frmoutputlog(tvScraperLog, True)
	'			Try
	'				MyFormObject.ShowDialog()
	'			Catch ex As ObjectDisposedException
	'			End Try
	'			ScraperErrorDetected = False
	'		Else
	'			BlinkTaskBar()
	'		End If
 '           TvAutoScrapeTimerTripped = False
	'		GC.Collect()
	'	Catch ex As Exception
	'		ExceptionHandler.LogError(ex)
	'	End Try
	'End Sub
#End Region

    Private Sub ep_SearchInvoke()
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf ep_SearchInvoke))
        Else
            ep_Search()
        End If
    End Sub
	Private Sub ep_Search()
        
		Dim ShowList As New List(Of TvShow)

		If Not bckgroundscanepisodes.IsBusy And Not Bckgrndfindmissingepisodes.IsBusy And Not BckWrkTv.IsBusy Then
			btnTvSearchNew.Text = "Cancel  "
			For Each item In Cache.TvCache.Shows
				If (item.NfoFilePath.ToLower.IndexOf("tvshow.nfo") <> -1) And ((item.State = Media_Companion.ShowState.Open) Or TVSearchALL = True) Then
					ShowList.Add(item)
				End If
			Next
            oTV.ListOfShows = ShowList
            oTV.EpForceSearch = TVSearchAll
            RunBackgroundTVScrape("TvEpisodesSearchforNew")
			'bckgroundscanepisodes.RunWorkerAsync({ShowList, TVSearchALL}) 'if searching all episodes (inc locked) TVSearchALL is true
		ElseIf bckgroundscanepisodes.IsBusy Then
			MsgBox("This Episode Scraper is already running")
		ElseIf Bckgrndfindmissingepisodes.IsBusy Then
			MsgBox("The missing episode search cannot be performed" & vbCrLf & "    while the episode scraper is running")
        Else If BckWrkTv.IsBusy Then
            MsgBox("Episode search cannot be performed" & vbCrLf & "    while scraper is running")
		End If
	End Sub

	Private Sub SearchThisShowForNewEpisodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_SearchNewEp.Click
		Try
			If TvTreeview.SelectedNode Is Nothing Then Exit Sub

			Dim Season As TvSeason
			Dim Episode As TvEpisode
			Dim ShowList As New List(Of TvShow)
			Dim selectednode As Integer = TvTreeview.SelectedNode.Index
			If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
				ShowList.Add(TvTreeview.SelectedNode.Tag)
			ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then

				Season = TvTreeview.SelectedNode.Tag
				ShowList.Add(Season.ShowObj)
			ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
				Episode = TvTreeview.SelectedNode.Tag
				ShowList.Add(Episode.ShowObj)
			End If
			Dim OverrideLock As Boolean = False
			If ShowList(0).State <> 0 Then  'i.e. it is either locked or unverified
				If MsgBox("This show is either 'Locked' or 'Unverified'. Do you want to continue scan?", MsgBoxStyle.YesNo, "Question?") = MsgBoxResult.Yes Then
					OverrideLock = True
				Else
					Exit Sub
				End If
			End If

			If Not bckgroundscanepisodes.IsBusy And Not Bckgrndfindmissingepisodes.IsBusy And Not BckWrkTv.IsBusy Then
				btnTvSearchNew.Text = "Cancel  "
                oTV.ListOfShows = ShowList
                oTV.EpForceSearch = OverrideLock
                RunBackgroundTVScrape("TvEpisodesSearchforNew")
				'bckgroundscanepisodes.RunWorkerAsync({ShowList, OverrideLock})
			ElseIf bckgroundscanepisodes.IsBusy Then
				MsgBox("This Episode Scraper is already running")
			ElseIf Bckgrndfindmissingepisodes.IsBusy Then
				MsgBox("The missing episode search cannot be performed" & vbCrLf & "    while the episode scraper is running")
            Else If BckWrkTv.IsBusy Then
                MsgBox("Episode search cannot be performed" & vbCrLf & "    while scraper is running")
			End If
			Do Until Not bckgroundscanepisodes.IsBusy AndAlso Not Bckgrndfindmissingepisodes.IsBusy AndAlso Not BckWrkTv.IsBusy
				Application.DoEvents()
			Loop
			TvTreeview.SelectedNode = TvTreeview.Nodes(selectednode)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tpMovWall_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpMovWall.LostFocus
		Try
			tpMovWall.Focus()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RefreshActorDBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Try
			Call mov_ActorRebuild()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DownsizeAllFanartsToSelectedSizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownsizeAllFanartsToSelectedSizeToolStripMenuItem.Click
		DownSizeAll("Backdrops")
	End Sub

	Private Sub DownsizeAllPostersToSelectedSizeToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles DownsizeAllPostersToSelectedSizeToolStripMenuItem.Click
		DownSizeAll("Posters")
	End Sub

	Private Sub DownSizeAll(postersOrBackdrops As String)
		Dim tempint As Integer = oMovies.MovieCache.Count
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
		Dim point As Point
		Dim height = 0
		If postersOrBackdrops = "Backdrops" then
			point = Movie.GetBackDropResolution(Pref.BackDropResolutionSI)
		Else
			height = Movie.GetHeightResolution(Pref.PosterResolutionSI)
		End If
		Using messbox As frmMessageBox = New frmMessageBox("Please wait - " & postersOrBackdrops & " are being resized", "", tempint.ToString & " remaining")
			messbox.Show
			Me.Refresh
			messbox.Refresh
			Dim path = ""
			For Each m In oMovies.MovieCache
				If postersOrBackdrops = "Backdrops" then
					path = Pref.GetFanartPath(m.fullpathandfilename)
					If File.Exists(path) Then DownloadCache.CopyAndDownSizeImage(path, path, point.x, point.y)
				Else
					path = Pref.GetPosterPath(m.fullpathandfilename)
					If File.Exists(path) Then DownloadCache.CopyAndDownSizeImage(path, path, , height)
				End If
				tempint -= 1
				messbox.TextBox3.Text = tempint.ToString & " remaining"
				messbox.TextBox3.Refresh
				Application.DoEvents
			Next
		End Using
	End Sub

	Public Sub mov_RebuildMovieCaches
		mov_PreferencesDisplay
		RunBackgroundMovieScrape("RebuildCaches")
        MergePrefMovieSetToMovieSetDatabase()
	End Sub

	Private Sub CheckRootsForToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckRootsForToolStripMenuItem.Click
		Try
			tv_ShowFind(Pref.tvRootFolders, False)
			If newTvFolders.Count > 0 Then
				For Each item In newTvFolders
					Pref.tvFolders.Add(item)
				Next
				Pref.tvFolders.Sort()
				Pref.ConfigSave()
			End If
			tv_ShowScrape()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Public Function util_RegexValidate(ByVal regexs As String)
		Try
			Dim test As Match
			test = Regex.Match("", regexs)
		Catch ex As Exception
			Return False
		End Try
		Return True
	End Function

	Public Function tv_LanguageCheck(ByVal id As String, ByVal language As String)
		Try
			Dim languagecode As String = language
			Dim url As String = "http://thetvdb.com/api/6E82FED600783400/series/" & id & "/" & languagecode & ".xml"
			Dim websource(10000)
			Dim urllinecount As Integer = 0
			Try
				Dim wrGETURL As WebRequest
				wrGETURL = WebRequest.Create(url)
				wrGETURL.Proxy = Utilities.MyProxy
				Dim objStream As IO.Stream
				objStream = wrGETURL.GetResponse.GetResponseStream()
				Dim objReader As New IO.StreamReader(objStream)
				Dim sLine As String = ""
				urllinecount = 0

				Do While Not sLine Is Nothing
					urllinecount += 1
					sLine = objReader.ReadLine
					If Not sLine Is Nothing Then
						websource(urllinecount) = sLine
					End If
				Loop
				objReader.Close()
				objStream.Close()
				objReader = Nothing
				objStream = Nothing
				urllinecount -= 1

			Catch ex As Exception
			End Try
			For f = 1 To urllinecount
				If websource(f).IndexOf("<Language>") <> -1 Then
					websource(f) = websource(f).Replace("<Language>", "")
					websource(f) = websource(f).Replace("</Language>", "")
					websource(f) = websource(f).Replace("  ", "")
					If websource(f).ToLower = languagecode Then
						Return True
					Else
						Return False
					End If
				End If
			Next
		Catch ex As Exception
		End Try
		Return "Error"
	End Function

	Private Sub ProfilesToolStripMenuItem_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ProfilesToolStripMenuItem.DropDownItemClicked
		Try
			Pref.ConfigSave()
			For Each prof In profileStruct.ProfileList
				If prof.ProfileName = e.ClickedItem.Text Then
					workingProfile.actorcache = prof.actorcache
					workingProfile.DirectorCache = prof.DirectorCache
					workingProfile.config = prof.config
					workingProfile.Genres = prof.Genres
					workingProfile.moviecache = prof.moviecache
					workingProfile.MusicVideoCache = prof.MusicVideoCache
					workingProfile.profilename = prof.profilename
					workingProfile.regexlist = prof.regexlist
					workingProfile.tvcache = prof.tvcache
					workingProfile.MovieSetCache = prof.MovieSetCache
					workingProfile.CustomTvCache = prof.CustomTvCache
					Call util_ProfileSetup()
				End If
			Next
			If e.ClickedItem.Text <> workingProfile.profilename Then Exit Sub
			For Each item In ProfilesToolStripMenuItem.DropDownItems
				If item.text = workingProfile.profilename Then
					With item
						item.checked = True
					End With
				Else
					item.checked = False
				End If
			Next
            tv_Filter()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub util_ProfileSetup()
		messbox = New frmMessageBox(" Please Wait", , "Loading Profile")
		messbox.Show()
		messbox.Refresh()
		Application.DoEvents()
		Me.Enabled = False
		If File.Exists(workingProfile.config) Then
			Pref.moviesets.Clear()
			Me.util_ConfigLoad()
		Else
			Call SetUpPreferences()
		End If
		util_MainFormTitleUpdate()  'creates & shows new title to Form1, also includes current profile name
		If Not File.Exists(workingProfile.moviecache) Or Pref.startupCache = False Then
			mov_RebuildMovieCaches
		Else
			oMovies.LoadCaches
		End If
		If File.Exists(workingProfile.Genres) Then Call util_GenreLoad()

		If Not File.Exists(workingProfile.tvcache) Or Pref.startupCache = False Then
			Call tv_CacheRefresh()
		Else
			Call tv_CacheLoad()
		End If

		If Not File.Exists(workingProfile.CustomTvCache) Or Pref.startupCache = False Then
			Call Custtv_CacheRefresh()
		Else
			Call Custtv_CacheLoad()
		End If

		If File.Exists(workingProfile.MusicVideoCache) Then Call UcMusicVideo1.MVCacheLoad()

		Me.Refresh()
		Application.DoEvents()

		If Pref.splt5 = 0 Then
			Dim tempint As Integer = SpltCntr_MovDisplay.Height
			tempint = tempint / 4
			tempint = tempint * 3
			Pref.splt5 = If(tempint > 275, tempint, 275)
		End If

		If Pref.startuptab = 0 Then
			SpltCntr_MovDisplay.SplitterDistance = Pref.splt1
			SpltCntr_MovPictures.SplitterDistance = Pref.splt2
			SpltCntr_MovBrowserList.SplitterDistance = Pref.splt5
			TabLevel1.SelectedIndex = 1
			SpltCntr_TvDisplay.SplitterDistance = Pref.splt3
			SpltCntr_TvPictures.SplitterDistance = Pref.splt4
			_tv_SplitContainer.SplitterDistance = Pref.splt6
			TabLevel1.SelectedIndex = 0
		Else
			SpltCntr_MovDisplay.SplitterDistance = Pref.splt1
			SpltCntr_MovPictures.SplitterDistance = Pref.splt2
			SpltCntr_MovBrowserList.SplitterDistance = Pref.splt5
			TabLevel1.SelectedIndex = 1
			SpltCntr_TvDisplay.SplitterDistance = Pref.splt3
			SpltCntr_TvPictures.SplitterDistance = Pref.splt4
			_tv_SplitContainer.SplitterDistance = Pref.splt6
		End If
		SpltCntr_MovDisplay.IsSplitterFixed = False
		SpltCntr_MovPictures.IsSplitterFixed = False
		SpltCntr_TvDisplay.IsSplitterFixed = False
		SpltCntr_TvPictures.IsSplitterFixed = False
		SpltCntr_MovBrowserList.IsSplitterFixed = False
		_tv_SplitContainer.IsSplitterFixed = False

		Try
			If cbMovieDisplay_MovieSet.Items.Count <> Pref.moviesets.Count Then
				cbMovieDisplay_MovieSet.Items.Clear()
				For Each mset In Pref.moviesets
					cbMovieDisplay_MovieSet.Items.Add(If(Pref.MovSetTitleIgnArticle, Pref.RemoveIgnoredArticles(mset), mset))
				Next
			End If
			If workingMovieDetails.fullmoviebody.SetName <> "-None-" Then
				For Each mset In Pref.moviesets
					cbMovieDisplay_MovieSet.Items.Add(If(Pref.MovSetTitleIgnArticle, Pref.RemoveIgnoredArticles(mset), mset))
				Next
				For te = 0 To cbMovieDisplay_MovieSet.Items.Count - 1
					If cbMovieDisplay_MovieSet.Items(te) = workingMovieDetails.fullmoviebody.SetName Then
						cbMovieDisplay_MovieSet.SelectedIndex = te
						Exit For
					End If
				Next
			End If
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
		mov_VideoSourcePopulate()
		ep_VideoSourcePopulate()
		Try
			TabControl2.SelectedIndex = 0
			currentTabIndex = 0
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
		Try
			TabControl3.SelectedIndex = 0
			tvCurrentTabIndex = 0
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
		Me.Enabled = True
		messbox.Close()
	End Sub

	Public Sub mov_SwitchRuntime()
		If workingMovieDetails Is Nothing Then Exit Sub
		If Pref.enablehdtags = True And workingMovieDetails.filedetails.Video.DurationInSeconds <> Nothing And Not displayRuntimeScraper Then
			runtimetxt.Text = Utilities.cleanruntime(workingMovieDetails.filedetails.Video.DurationInSeconds.Value) & " min"
			runtimetxt.Enabled = False
		Else
			runtimetxt.Text = workingMovieDetails.fullmoviebody.runtime
			runtimetxt.Enabled = True
		End If
	End Sub

	Public Sub mov_Switchtop250()
		If workingMovieDetails Is Nothing Then Exit Sub
		If lbl_movTop250.Text = "Top 250" Then
			lbl_movTop250.Text = "Metascore"
			top250txt.Text = workingMovieDetails.fullmoviebody.metascore
		Else
			lbl_movTop250.Text = "Top 250"
			top250txt.Text = workingMovieDetails.fullmoviebody.top250
		End If
	End Sub

	Private Sub SearchForNewMoviesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchForNewMoviesToolStripMenuItem.Click
		SearchForNew
	End Sub

	Public Function Yield(yielding As Boolean) As Boolean
		If yielding Then
			Application.DoEvents
			Return _yield
		End If
		Return False
	End Function

	Private Sub util_FontSetup()
		If Pref.font <> Nothing Then
			If Pref.font <> "" Then
				Try
					Dim tcc As TypeConverter = TypeDescriptor.GetConverter(GetType(System.Drawing.Font))
					Dim newFont As System.Drawing.Font = CType(tcc.ConvertFromString(Pref.font), System.Drawing.Font)
					genretxt.Font = newFont
					premiertxt.Font = newFont
					certtxt.Font = newFont
					directortxt.Font = newFont
					creditstxt.Font = newFont
					outlinetxt.Font = newFont
					runtimetxt.Font = newFont
					studiotxt.Font = newFont
					countrytxt.Font = newFont
					taglinetxt.Font = newFont
					cbMovieDisplay_Actor.Font = newFont
					roletxt.Font = newFont
					pathtxt.Font = newFont
					tb_MovSortOrder.Font = newFont
					DataGridViewMovies.Font = newFont
					plottxt.Font = newFont
					txtStars.Font = newFont
					cbMovieDisplay_MovieSet.Font = newFont
					cmbxEpActor.Font = newFont
					TvTreeview.Font = newFont
					tbEpRole.Font = newFont
					tb_EpDirector.Font = newFont
					tb_EpCredits.Font = newFont
					tb_EpPlot.Font = newFont
					tb_EpAired.Font = newFont
					tb_EpRating.Font = newFont
					tb_EpImdbId.Font = newFont
					tb_EpFilename.Font = newFont
					tb_ShPlot.Font = newFont
					cbTvActor.Font = newFont
					cbTvActorRole.Font = newFont
					tb_ShRunTime.Font = newFont
					tb_ShStudio.Font = newFont
					tb_ShPremiered.Font = newFont
					tb_ShRating.Font = newFont
					tb_ShVotes.Font = newFont
					tb_ShTvdbId.Font = newFont
					tb_ShGenre.Font = newFont
					tb_ShImdbId.Font = newFont
					tb_ShCert.Font = newFont
                    cmbx_shUserRating.Font = newFont

					ratingtxt.Font = newFont
					votestxt.Font = newFont
					top250txt.Font = newFont

					cbUsrRated.Font = newFont
					cbFilterGeneral.Font = newFont

					For Each x In MovieFilters
						x.Font = newFont
					Next
                    
					LabelCountFilter.Font = newFont
					Me.Refresh()
					Application.DoEvents()
				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
				End Try
			End If
		End If
	End Sub

	Private Sub langarrsetup()
		Dim strfilename As String
		Dim num_rows As Long
		Dim num_cols As Long
		Dim x As Integer
		Dim y As Integer
		strfilename = applicationPath & "\Assets\" & "test.csv"
		If File.Exists(strfilename) Then
			Using tmpstream As IO.StreamReader = File.OpenText(strfilename)
			    Dim strlines() As String
			    Dim strline() As String
			    strlines = tmpstream.ReadToEnd().Split(Environment.NewLine)
			    num_rows = UBound(strlines)
			    strline = strlines(0).Split(",")
			    num_cols = UBound(strline)

			    ' Copy the data into the array.
			    For x = 0 To num_rows - 1
				    strline = strlines(x).Split(",")
				    For y = 0 To num_cols
					    langarray(x, y) = strline(y)
				    Next
			    Next
			End Using
		End If
	End Sub

#Region "Movie Table"

	'tabpage events
	Private Sub tpMovTable_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpMovTable.Enter
		'MovSetsRepopulate()
		'mov_TableSetup()
	End Sub

	Private Sub tpMovTable_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpMovTable.Leave
		dgvMoviesTableView.EndEdit()
		Pref.tableview.Clear()
		For Each column In dgvMoviesTableView.Columns
			Dim tempstring As String = String.Format("{0}|{1}|{2}|{3}", column.name, column.width, column.displayindex, column.visible)
			Pref.tableview.Add(tempstring)
		Next
		If IsNothing(dgvMoviesTableView.SortedColumn) = False Then
			Pref.tablesortorder = String.Format("{0} | {1}", dgvMoviesTableView.SortedColumn.HeaderText, dgvMoviesTableView.SortOrder.ToString)
			Pref.ConfigSave()
		End If

		If DataDirty Then
			Dim tempint As Integer = MessageBox.Show("You appear to have made changes to some of your movie details." & vbCrLf & vbCrLf & "Any changes will be lost if you do not save the changes now." & "                 Do wish to save the changes?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			If tempint = DialogResult.Yes Then
				Call mov_TableChangesSave()
				MsgBox("Changes Saved")
			End If
		End If

		DataDirty = False
		btn_movTableSave.Enabled = DataDirty
	End Sub

	'Table Setup and update
	Private Sub mov_TableViewSetup()
		Pref.tableview.Clear()
		Pref.tableview.Add("title|150|0|true")
		Pref.tableview.Add("year|40|1|true")
		Pref.tableview.Add("genre|160|2|true")
		Pref.tableview.Add("rating|50|3|true")
		Pref.tableview.Add("runtime|60|4|true")
		Pref.tableview.Add("top250|89|5|false")
		Pref.tableview.Add("source|150|6|false")
		Pref.tableview.Add("playcount|120|7|true")
		Pref.tableview.Add("set|150|8|true")
		Pref.tableview.Add("certificate|100|9|false")
		Pref.tableview.Add("sorttitle|100|10|false")
		Pref.tableview.Add("outline|200|11|false")
		Pref.tableview.Add("plot|200|12|false")
		Pref.tableview.Add("stars|200|13|false")
		Pref.tableview.Add("id|82|14|false")
		Pref.tableview.Add("missingdata1|115|15|false")
		Pref.tableview.Add("fullpathandfilename|300|16|false")
		Pref.tableview.Add("createdate|104|17|false")
		Pref.tableview.Add("userrated|100|18|false")
	End Sub

	Private Sub mov_TableSetup()
		dgvMoviesTableView.Columns.Clear()
		If Pref.tablesortorder = Nothing Then Pref.tablesortorder = "Title|Ascending"
		If Pref.tablesortorder = "" Then Pref.tablesortorder = "Title|Ascending"
		If Pref.tableview.Count < 19 Then    'Counter. Increase if adding new tableview column else new columns won't be added to config.xml.
			Call mov_TableViewSetup()
		End If
		tableSets.Clear()
		For Each item In Pref.tableview
			Dim tempdata() As String
			tempdata = item.Split("|")
			Dim newcolumn As New str_TableItems(SetDefaults)
			newcolumn.title = tempdata(0)
			newcolumn.width = Convert.ToInt32(tempdata(1))
			newcolumn.index = Convert.ToInt32(tempdata(2))
			newcolumn.visible = (tempdata(3).ToLower = "true")
			tableSets.Add(newcolumn)
		Next

		dgvMoviesTableView.AutoGenerateColumns = False
		Dim doc As New XmlDocument
		Dim thispref As XmlNode = Nothing
		Dim xmlproc As XmlDeclaration
		xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
		doc.AppendChild(xmlproc)
		Dim root As XmlElement
		Dim child As XmlElement
		root = doc.CreateElement("movie_cache")
		Dim childchild As XmlElement
		For Each row As DataGridViewRow In DataGridViewMovies.Rows
			Dim movie As Data_GridViewMovie = row.DataBoundItem
			child = doc.CreateElement("movie")
			childchild = doc.CreateElement("filedate") : childchild.InnerText = movie.filedate : child.AppendChild(childchild)
			childchild = doc.CreateElement("missingdata1") : childchild.InnerText = movie.missingdata1.ToString : child.AppendChild(childchild)
			childchild = doc.CreateElement("filename") : childchild.InnerText = movie.filename : child.AppendChild(childchild)
			childchild = doc.CreateElement("foldername") : childchild.InnerText = movie.foldername : child.AppendChild(childchild)
			childchild = doc.CreateElement("fullpathandfilename") : childchild.InnerText = movie.fullpathandfilename : child.AppendChild(childchild)
			childchild = doc.CreateElement("set") : childchild.InnerText = movie.SetName : child.AppendChild(childchild)
			childchild = doc.CreateElement("source") : childchild.InnerText = movie.source : child.AppendChild(childchild)
			childchild = doc.CreateElement("genre") : childchild.InnerText = movie.genre : child.AppendChild(childchild)
			childchild = doc.CreateElement("id") : childchild.InnerText = movie.id : child.AppendChild(childchild)
			childchild = doc.CreateElement("playcount") : childchild.InnerText = movie.playcount : child.AppendChild(childchild)
			childchild = doc.CreateElement("rating") : childchild.InnerText = movie.Rating : child.AppendChild(childchild)
			childchild = doc.CreateElement("title") : childchild.InnerText = movie.title : child.AppendChild(childchild)
			childchild = doc.CreateElement("certificate") : childchild.InnerText = movie.Certificate : child.AppendChild(childchild)
			childchild = doc.CreateElement("outline") : childchild.InnerText = movie.outline : child.AppendChild(childchild)
			childchild = doc.CreateElement("plot") : childchild.InnerText = movie.plot : child.AppendChild(childchild)
			childchild = doc.CreateElement("stars") : childchild.InnerText = movie.stars : child.AppendChild(childchild)
			childchild = doc.CreateElement("sortorder") : childchild.InnerText = movie.SortOrder : child.AppendChild(childchild)
			childchild = doc.CreateElement("runtime") : childchild.InnerText = movie.runtime : child.AppendChild(childchild)
			childchild = doc.CreateElement("top250") : childchild.InnerText = movie.top250 : child.AppendChild(childchild)
			childchild = doc.CreateElement("year") : childchild.InnerText = movie.year : child.AppendChild(childchild)
			childchild = doc.CreateElement("createdate") : childchild.InnerText = movie.createdate : child.AppendChild(childchild)
			childchild = doc.CreateElement("usrrated") : childchild.InnerText = movie.usrrated : child.AppendChild(childchild)
			root.AppendChild(child)
		Next

		doc.AppendChild(root)

		For Each thisresult As XmlNode In doc("movie_cache")
			Select Case thisresult.Name
				Case "movie"
					Dim chi As XmlElement
					For Each chi In thisresult.childnodes
						If chi.Name = "runtime" Then
							chi.InnerText = chi.InnerText.Replace("minutes", "")
							chi.InnerText = chi.InnerText.Replace("mins", "")
							chi.InnerText = chi.InnerText.Replace("min", "")
							chi.InnerText = chi.InnerText.Replace(" ", "")
							If chi.InnerText.Length = 1 Then
								chi.InnerText = "00" & chi.InnerText
							End If
							If chi.InnerText.Length = 2 Then
								chi.InnerText = "0" & chi.InnerText
							End If
							chi.InnerText = chi.InnerText & " min"
						End If
						If chi.Name = "top250" Then
							If chi.InnerText = "" Then
								chi.InnerText = "000" & chi.InnerText
							End If
							If IsNumeric(chi.InnerText) Then
								If chi.InnerText.Length = 0 Then
									chi.InnerText = "000" & chi.InnerText
								End If
								If chi.InnerText.Length = 1 Then
									chi.InnerText = "00" & chi.InnerText
								End If
								If chi.InnerText.Length = 2 Then
									chi.InnerText = "0" & chi.InnerText
								End If
							End If
						End If
						If chi.Name = "playcount" Then
							If chi.InnerText = "" OrElse chi.InnerText = "0" OrElse Not IsNumeric(chi.InnerText) Then
								chi.InnerText = "UnWatched"
							Else
								chi.InnerText = "Watched"
							End If
						End If
						If chi.Name = "missingdata1" Then
							If chi.InnerText = "" Or Not IsNumeric(chi.InnerText) Then
								chi.InnerText = "0"
							Else
								chi.InnerText = Movie.GetMissingDataText(Convert.ToByte(chi.InnerText))
							End If
						End If
						If chi.Name = "createdate" Then
							If chi.InnerText = "" Or Not IsNumeric(chi.InnerText) Then
								chi.InnerText = "0"
							Else
								chi.InnerText = Utilities.DecodeDateTime(chi.InnerText, Pref.DateFormat)
							End If
						End If
					Next
			End Select
		Next

		Dim movstring As String = doc.InnerXml
		Dim XMLreader2 As IO.StringReader = New IO.StringReader(movstring)

		' Create the dataset
		Dim newDS As DataSet = New DataSet
		newDS.ReadXml(XMLreader2)
		XMLreader2.Dispose()
		dgvMoviesTableView.DataSource = Nothing
		Try
			dgvMoviesTableView.DataSource = newDS.Tables(0)
		Catch
		End Try

		dgvMoviesTableView.AllowUserToOrderColumns = True
		dgvMoviesTableView.RowHeadersVisible = True
		dgvMoviesTableView.RowHeadersWidth = 25

		Dim titlecolumn As New DataGridViewColumn()
		With titlecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Title"
			.DataPropertyName = "title"
			.Name = "title"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim yearcolumn As New DataGridViewColumn()
		With yearcolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Year"
			.DataPropertyName = "year"
			.Name = "year"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter
		End With


		Dim idcolumn As New DataGridViewColumn()
		With idcolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "IMDB ID"
			.DataPropertyName = "id"
			.Name = "id"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.ReadOnly = True
		End With

		Dim pathcolumn As New DataGridViewColumn()
		With pathcolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Full Path"
			.DataPropertyName = "fullpathandfilename"
			.Name = "fullpathandfilename"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.ReadOnly = True
		End With

		Dim genrecolumn As New DataGridViewColumn()
		With genrecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Genre"
			.DataPropertyName = "genre"
			.Name = "genre"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.ToolTipText = "Separate Genre's with a ""Space/Space"", ie: Action / Horror"
		End With

		Dim ratingcolumn As New DataGridViewColumn()
		With ratingcolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Rating"
			.DataPropertyName = "rating"
			.Name = "rating"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter
		End With

		Dim outlinecolumn As New DataGridViewColumn()
		With outlinecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Outline"
			.DataPropertyName = "outline"
			.Name = "outline"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim plotcolumn As New DataGridViewColumn()
		With plotcolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Plot"
			.DataPropertyName = "plot"
			.Name = "plot"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim starscolumn As New DataGridViewColumn()
		With starscolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Stars"
			.DataPropertyName = "stars"
			.Name = "stars"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.ReadOnly = True
		End With

		Dim sorttitlecolumn As New DataGridViewColumn()
		With sorttitlecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Sort Title"
			.DataPropertyName = "sortorder"
			.Name = "sorttitle"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim top250column As New DataGridViewColumn()
		With top250column
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Top 250"
			.DataPropertyName = "top250"
			.Name = "top250"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim watchedcolumn As New DataGridViewComboBoxColumn()
		watchedcolumn.Items.Add("UnWatched")
		watchedcolumn.Items.Add("Watched")
		With watchedcolumn
			.HeaderText = "Watched"
			.Name = "playcount"
			.DataPropertyName = "playcount"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.DefaultCellStyle.NullValue = ""
		End With

		Dim sourcecolumn As New DataGridViewComboBoxColumn()
		For Each src In Pref.releaseformat
			sourcecolumn.Items.Add(src)
		Next
		sourcecolumn.Items.Insert(0, "")
		With sourcecolumn
			.HeaderText = "Source"
			.Name = "source"
			.DataPropertyName = "source"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.DefaultCellStyle.NullValue = ""
		End With

		Dim runtimecolumn As New DataGridViewColumn()
		With runtimecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Runtime"
			.Name = "runtime"
			.DataPropertyName = "runtime"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim certificatecolumn As New DataGridViewColumn()
		With certificatecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Certificate"
			.Name = "certificate"
			.DataPropertyName = "certificate"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim setscolumn As New DataGridViewComboBoxColumn
        MovSetsRepopulate()
		For Each sets In Pref.moviesets
			setscolumn.Items.Add(sets)
		Next
        For each sets in oMovies.MovieSetDB
            If Not setscolumn.Items.Contains(sets.MovieSetDisplayName) Then
                setscolumn.Items.Add(sets.MovieSetDisplayName)
            End If
        Next

		With setscolumn
			.HeaderText = "Sets"
			.Name = "set"
			.DataPropertyName = "set"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.DefaultCellStyle.NullValue = "-None-"
		End With

		Dim artcolumn As New DataGridViewColumn()
		With artcolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Missing"
			.Name = "missingdata1"
			.DataPropertyName = "missingdata1"
			.SortMode = DataGridViewColumnSortMode.Automatic
			.ReadOnly = True
		End With

		Dim createdatecolumn As New DataGridViewColumn()
		With createdatecolumn
			Dim oCell As DataGridViewCell = New DataGridViewTextBoxCell
			.CellTemplate = oCell
			.HeaderText = "Date Added"
			.Name = "createdate"
			.DataPropertyName = "createdate"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		Dim usrratedcolumn As New DataGridViewComboBoxColumn()
		For each t In cbUsrRated.Items
			usrratedcolumn.Items.Add(t)
		Next
		usrratedcolumn.Items.Item(0) = "0"
		With usrratedcolumn
			.HeaderText = "User Rating"
			.DataPropertyName = "usrrated"
			.Name = "userrated"
			.SortMode = DataGridViewColumnSortMode.Automatic
		End With

		For f = 0 To tableview.Count - 1
			For Each col As str_TableItems In tableSets
				If col.index = f Then
					Select Case col.title
						Case "title"
							titlecolumn.Width = col.width
							titlecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, titlecolumn)
							Exit For
						Case "year"
							yearcolumn.Width = col.width
							yearcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, yearcolumn)
							Exit For
						Case "sorttitle"
							sorttitlecolumn.Width = col.width
							sorttitlecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, sorttitlecolumn)
							Exit For
						Case "genre"
							genrecolumn.Width = col.width
							genrecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, genrecolumn)
							Exit For
						Case "rating"
							ratingcolumn.Width = col.width
							ratingcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, ratingcolumn)
							Exit For
						Case "runtime"
							runtimecolumn.Width = col.width
							runtimecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, runtimecolumn)
							Exit For
						Case "top250"
							top250column.Width = col.width
							top250column.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, top250column)
							Exit For
						Case "certificate"
							certificatecolumn.Width = col.width
							certificatecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, certificatecolumn)
						Case "source"
							sourcecolumn.Width = col.width
							sourcecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, sourcecolumn)
							Exit For
						Case "playcount"
							watchedcolumn.Width = col.width
							watchedcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, watchedcolumn)
							Exit For
						Case "set"
							setscolumn.Width = col.width
							setscolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, setscolumn)
							Exit For
						Case "outline"
							outlinecolumn.Width = col.width
							outlinecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, outlinecolumn)
							Exit For
						Case "plot"
							plotcolumn.Width = col.width
							plotcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, plotcolumn)
							Exit For
						Case "stars"
							starscolumn.Width = col.width
							starscolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, starscolumn)
							Exit For
						Case "id"
							idcolumn.Width = col.width
							idcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, idcolumn)
							Exit For
						Case "missingdata1"
							artcolumn.Width = col.width
							artcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, artcolumn)
							Exit For
						Case "fullpathandfilename"
							pathcolumn.Width = col.width
							pathcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, pathcolumn)
							Exit For
						Case "createdate"
							createdatecolumn.Width = col.width
							createdatecolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, createdatecolumn)
							Exit For
						Case "userrated"
							usrratedcolumn.Width = col.width
							usrratedcolumn.Visible = col.visible
							dgvMoviesTableView.Columns.Insert(f, usrratedcolumn)
							Exit For
					End Select
				End If
			Next
		Next f

		Dim sortheader As String
		Dim sortord As String
		Dim tempdata2() As String
		tempdata2 = Pref.tablesortorder.Split("|")
		sortheader = tempdata2(0)
		sortord = tempdata2(1)

		For Each col In dgvMoviesTableView.Columns
			If col.headertext = sortheader Then
				If sortord.ToLower.IndexOf("desc") <> -1 Then
					dgvMoviesTableView.Sort(dgvMoviesTableView.Columns(col.index), ListSortDirection.Descending)
				Else
					dgvMoviesTableView.Sort(dgvMoviesTableView.Columns(col.index), ListSortDirection.Ascending)
				End If
			End If
		Next

		For Each tempRow As System.Windows.Forms.DataGridViewRow In Me.dgvMoviesTableView.Rows
			For Each tempCell As Windows.Forms.DataGridViewCell In tempRow.Cells
				If tempCell.Value = "Fanart" Or tempCell.Value = "Poster" Or tempCell.Value = "Poster & Fanart" Then tempCell.Style.BackColor = Color.Red
			Next
		Next

		Call mov_TableEditSetup()
		Try
			For f = 0 To dgvMoviesTableView.Rows.Count - 1
				If dgvMoviesTableView.Rows(f).Cells("fullpathandfilename").Value = workingMovieDetails.fileinfo.fullpathandfilename Then
					dgvMoviesTableView.ClearSelection()
					dgvMoviesTableView.FirstDisplayedScrollingRowIndex = f
					Exit For
				End If
			Next
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
	End Sub

	Private Sub mov_TableEditSetup()
		mov_TableEditDGV.Columns.Clear()
		mov_TableEditDGV.AutoGenerateColumns = False
		mov_TableEditDGV.DataSource = Nothing
		For Each dgvCol As DataGridViewColumn In dgvMoviesTableView.Columns
			Dim dgvNewCol As New DataGridViewColumn
			dgvNewCol = DirectCast(dgvCol.Clone(), DataGridViewColumn)
			dgvNewCol.CellTemplate = DirectCast(dgvCol.CellTemplate, DataGridViewCell)
			If dgvNewCol.Name = "plot" Or dgvNewCol.Name = "outline" Or dgvNewCol.Name = "stars" Or dgvNewCol.Name = "id" Or dgvNewCol.Name = "missingdata1" or dgvNewCol.Name = "fullpathandfilename" Then
				dgvNewCol.ReadOnly = True
				dgvNewCol.DefaultCellStyle.BackColor = System.Drawing.Color.gray
			End If
			mov_TableEditDGV.Columns.Add(dgvNewCol)
		Next

		'add "Unchanged"
		Dim src_add As DataGridViewComboBoxColumn = DirectCast(Me.mov_TableEditDGV.Columns("source"), DataGridViewComboBoxColumn)
		src_add.Items.Insert(0, "UnChanged")
		Dim set_add As DataGridViewComboBoxColumn = DirectCast(Me.mov_TableEditDGV.Columns("set"), DataGridViewComboBoxColumn)
		set_add.Items.Insert(0, "UnChanged")
		Dim play_add As DataGridViewComboBoxColumn = DirectCast(Me.mov_TableEditDGV.Columns("playcount"), DataGridViewComboBoxColumn)
		play_add.Items.Insert(0, "UnChanged")
		Dim usrrated_add As DataGridViewComboBoxColumn = DirectCast(Me.mov_TableEditDGV.Columns("userrated"), DataGridViewComboBoxColumn)
		usrrated_add.Items.Insert(0, "UnChanged")

		mov_TableEditDGV.RowHeadersWidth = dgvMoviesTableView.RowHeadersWidth
		mov_TableEditDGV.ClearSelection
		mov_TableEditDGV.ScrollBars = ScrollBars.None
		Me.mov_TableEditDGV.DefaultCellStyle.SelectionBackColor = Me.mov_TableEditDGV.DefaultCellStyle.BackColor
		Me.mov_TableEditDGV.DefaultCellStyle.SelectionForeColor = Me.mov_TableEditDGV.DefaultCellStyle.ForeColor

		Dim emptydata As String = FillEmptydoc().InnerXml
		Dim XMLreader3 As IO.StringReader = New IO.StringReader(emptydata)

		' Create the Empty dataset
		Dim emptydataset As DataSet = New DataSet
		emptydataset.ReadXml(XMLreader3)
		XMLreader3.Dispose()
		mov_TableEditDGV.DataSource = emptydataset.tables(0)
		mov_TableEditDGV.CurrentRow.Selected = false
	End Sub

	Private Sub DataGridView1_CellBeginEdit(sender As System.Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvMoviesTableView.CellBeginEdit

		Dim colName = (dgvMoviesTableView.Rows(e.RowIndex).Cells(e.ColumnIndex).OwningColumn.Name.ToString).ToLower()

		If colName = "set" Then
			Dim rec = (From x In oMovies.MovieCache Where x.fullpathandfilename = dgvMoviesTableView.Rows(e.RowIndex).Cells("fullpathandfilename").Value.ToString).FirstOrDefault

			If rec.Locked(colName) Then
				e.Cancel = True
			End If
		End If


	End Sub

	Private Sub mov_TableChangesSave()

		DataDirty = False

		frmSplash2.Text = "Saving Table Changes..."
		frmSplash2.Label1.Text = "Saving Movie Data....."
		frmSplash2.Label1.Visible = True
		frmSplash2.Label2.Visible = True
		frmSplash2.ProgressBar1.Visible = True
		frmSplash2.ProgressBar1.Maximum = dgvMoviesTableView.Rows.Count
		frmSplash2.Show()

		Application.DoEvents

		Dim progcount As Integer = 0
		Dim changed As Boolean
		Dim oCachedMovie As ComboList

		For Each gridrow As DataGridViewRow In dgvMoviesTableView.Rows
			changed = False
			progcount += 1
			frmSplash2.ProgressBar1.Value = progcount
			frmSplash2.Label2.Text = gridrow.Cells("Title").Value
			oCachedMovie = oMovies.FindCachedMovie(gridrow.Cells("fullpathandfilename").Value)

			Try
				If oCachedMovie.title <> gridrow.Cells("Title").Value Then changed = True
			Catch
			End Try
			Try
				If oCachedMovie.outline <> gridrow.Cells("Outline").Value Then changed = True
			Catch
			End Try
			Try
				If oCachedMovie.genre <> gridrow.Cells("genre").Value Then changed = True
			Catch
			End Try
			Try
				If oCachedMovie.rating <> gridrow.Cells("rating").Value Then changed = True
			Catch
			End Try
			Try
				Dim playstatus As String = gridrow.Cells("playcount").EditedFormattedValue
				Dim plycnt As Integer = 0
				If playstatus.tolower = "watched" Then plycnt = 1
				If oCachedMovie.playcount <> plycnt Then Changed = True
			Catch
			End Try
			Try
				If oCachedMovie.sortorder <> gridrow.Cells("sorttitle").Value Then changed = True
			Catch
			End Try
			Try
				If oCachedMovie.year <> gridrow.Cells("year").Value Then
					If IsNumeric(gridrow.Cells("year").Value) Then changed = True
				End If
			Catch
			End Try
			Try
				If Convert.ToInt32(oCachedMovie.top250) <> Convert.ToInt32(gridrow.Cells("top250").Value) Then
					If IsNumeric(gridrow.Cells("top250").Value) Then changed = True
				End If
			Catch
			End Try
			Try
				If oCachedMovie.Certificate <> gridrow.Cells("certificate").Value Then changed = True
			Catch
			End Try
			Dim runtime As String = ""
			Dim intRunTime As Integer
			Dim runTimeChanged As Boolean
			Dim newRunTime As String = ""

			Try
				runtime = gridrow.Cells("runtime").Value
				runtime = runtime.Replace("min", "")
				runtime = runtime.Trim(" ")
				If IsNumeric(runtime) Then
					intRunTime = Convert.ToInt32(runtime)
					newRunTime = intRunTime.ToString & " min"
					If oCachedMovie.runtime <> newRunTime Then changed = True : runTimeChanged = True
				End If
			Catch
			End Try
			If oCachedMovie.source <> If(IsDBNull(gridrow.Cells("source").Value), "", gridrow.Cells("source").Value) Then changed = True
			If oCachedMovie.SetName <> If(IsDBNull(gridrow.Cells("set").Value), "", gridrow.Cells("set").Value) Then changed = True
			If oCachedMovie.usrrated <> gridrow.Cells("userrated").Value Then changed = True
			If changed And File.Exists(oCachedMovie.fullpathandfilename) Then

				Dim oMovie As Movie = oMovies.LoadMovie(oCachedMovie.fullpathandfilename)
				If IsNothing(oMovie) Then Continue For

				Try
					oCachedMovie.genre = gridrow.Cells("genre").Value
				Catch
				End Try
				Try
					oCachedMovie.title = gridrow.Cells("title").Value
				Catch
				End Try
				Try
					oCachedMovie.year = gridrow.Cells("year").Value
				Catch
				End Try
				Try
					oCachedMovie.sortorder = gridrow.Cells("sorttitle").Value
				Catch
				End Try
				Try
					oCachedMovie.rating = gridrow.Cells("rating").Value
				Catch
				End Try
				Try
					oCachedMovie.source = If(IsDBNull(gridrow.Cells("source").Value), "", gridrow.Cells("source").Value)
				Catch
				End Try
				Try
					Dim NewSetName As String = If(IsDBNull(gridrow.Cells("set").Value), "", gridrow.Cells("set").Value)
					If NewSetName = "" Then
						oCachedMovie.SetName = ""
						oCachedMovie.TmdbSetId = ""
					Else
						Dim aok As Boolean = False
						For Each m In oMovies.MovieSetDB
							If m.MovieSetDisplayName = NewSetName Then
								oCachedMovie.SetName = m.MovieSetName
								oCachedMovie.TmdbSetId = m.TmdbSetId
								aok = True
							End If
						Next
						If Not aok Then
							For each m In Pref.moviesets
								If m = NewSetName Then
									oCachedMovie.SetName = m
									Exit For
								End If
							Next
						End If
					End If
				Catch
				End Try
				oCachedMovie.top250 = Convert.ToInt32(gridrow.Cells("top250").Value).ToString
				If gridrow.Cells("playcount").EditedFormattedValue = "Watched" Then
					oCachedMovie.playcount = "1"
				Else
					oCachedMovie.playcount = "0"
				End If
				Try
					oCachedMovie.Certificate = gridrow.Cells("certificate").Value
				Catch
				End Try
				Try
					oCachedMovie.usrrated = gridrow.Cells("userrated").Value
				Catch
				End Try

				If runTimeChanged Then
					oMovie.ScrapedMovie.fullmoviebody.runtime = newRunTime
				End If
				oMovie.ScrapedMovie.fullmoviebody.title     = oCachedMovie.title
				oMovie.ScrapedMovie.fullmoviebody.year      = oCachedMovie.year
				oMovie.ScrapedMovie.fullmoviebody.playcount = oCachedMovie.playcount
				oMovie.ScrapedMovie.fullmoviebody.genre     = oCachedMovie.genre
				oMovie.ScrapedMovie.fullmoviebody.rating    = oCachedMovie.rating
				oMovie.ScrapedMovie.fullmoviebody.source    = oCachedMovie.source
				oMovie.ScrapedMovie.fullmoviebody.SetName   = oCachedMovie.SetName
				oMovie.ScrapedMovie.fullmoviebody.TmdbSetId = oCachedMovie.TmdbSetId 
				oMovie.ScrapedMovie.fullmoviebody.sortorder = oCachedMovie.sortorder
				oMovie.ScrapedMovie.fullmoviebody.top250    = oCachedMovie.top250
				oMovie.ScrapedMovie.fullmoviebody.director  = oCachedMovie.director
				oMovie.ScrapedMovie.fullmoviebody.credits   = oCachedMovie.credits
				oMovie.ScrapedMovie.fullmoviebody.usrrated  = oCachedMovie.usrrated.ToString

				Dim checkmpaa = oCachedMovie.Certificate
				oMovie.ScrapedMovie.fullmoviebody.mpaa = checkmpaa
				oMovie.AssignMovieToCache
				oMovie.SaveNFO
				oMovie.UpdateMovieCache
			End If
		Next

		oMovies.SaveMovieCache
		UpdateFilteredList
		frmSplash2.Hide

		Application.DoEvents
		Me.BringToFront
	End Sub

	Private Sub mov_TableUpdate()
		''Table View is not the best location to edit Outline, Plot, IMDB Id.
		Dim changed As Boolean = False
		For Each row In dgvMoviesTableView.SelectedRows
			If mov_TableEditDGV.Columns("title").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("title").Value <> "" Then
				row.cells("title").value = mov_TableEditDGV.Rows(0).Cells("title").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("top250").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("top250").Value <> "" Then
				row.cells("top250").value = mov_TableEditDGV.Rows(0).Cells("top250").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("year").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("year").Value <> "" Then
				row.cells("year").value = mov_TableEditDGV.Rows(0).Cells("year").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("sorttitle").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("sorttitle").Value <> "" Then
				row.cells("sorttitle") = mov_TableEditDGV.Rows(0).Cells("sorttitle").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("runtime").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("runtime").Value <> "" Then
				row.cells("runtime").value = mov_TableEditDGV.Rows(0).Cells("runtime").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("rating").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("rating").Value <> "" Then
				row.cells("rating").value = mov_TableEditDGV.Rows(0).Cells("rating").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("certificate").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("certificate").Value <> "" Then
				row.cells("certificate").value = mov_TableEditDGV.Rows(0).Cells("certificate").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("genre").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("genre").Value <> "" Then
				row.cells("genre").value = mov_TableEditDGV.Rows(0).Cells("genre").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("source").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("source").Value <> "UnChanged" Then
				row.cells("source").value = mov_TableEditDGV.Rows(0).Cells("source").Value : changed = True
			End If
			If mov_TableEditDGV.Columns("playcount").Visible AndAlso mov_TableEditDGV.Rows(0).Cells("playcount").Value <> "UnChanged" Then
				If mov_TableEditDGV.Rows(0).Cells("playcount").Value = "Watched" Then
					row.cells("playcount").value = "Watched"
				Else
					row.cells("playcount").value = "UnWatched"
				End If
				changed = True
			End If
			If mov_TableEditDGV.Columns("set").Visible AndAlso (IsDBNull(mov_TableEditDGV.Rows(0).Cells("set").Value) OrElse mov_TableEditDGV.Rows(0).Cells("set").Value <> "UnChanged") Then
				row.cells("set").value = If(IsDBNull(mov_TableEditDGV.Rows(0).Cells("set").Value), Nothing, mov_TableEditDGV.Rows(0).Cells("set").Value) : changed = True
			End If
			If mov_TableEditDGV.Columns("userrated").Visible AndAlso mov_TableEditDGV.ROws(0).Cells("userrated").Value <> "UnChanged" Then
				row.cells("userrated").Value = mov_TableEditDGV.Rows(0).Cells("userrated").Value : changed = True
			End If
		Next
		DataDirty = changed
		btn_movTableSave.Enabled = changed
	End Sub

	Private Function FillEmptyDoc() As XmlDocument
		Dim doc As New XmlDocument

		Dim thispref As XmlNode = Nothing
		Dim xmlproc As XmlDeclaration
		xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
		doc.AppendChild(xmlproc)
		Dim root As XmlElement
		Dim child As XmlElement
		root = doc.CreateElement("movie_cache")
		Dim childchild As XmlElement
		child = doc.CreateElement("movie")
		childchild = doc.CreateElement("filedate") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("missingdata1") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("filename") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("foldername") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("fullpathandfilename") : childchild.InnerText = "Not Editable in TableView" : child.AppendChild(childchild)
		childchild = doc.CreateElement("set") : childchild.InnerText = "UnChanged" : child.AppendChild(childchild)
		childchild = doc.CreateElement("source") : childchild.InnerText = "UnChanged" : child.AppendChild(childchild)
		childchild = doc.CreateElement("genre") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("id") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("playcount") : childchild.InnerText = "UnChanged" : child.AppendChild(childchild)
		childchild = doc.CreateElement("rating") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("title") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("certificate") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("outline") : childchild.InnerText = "Not Editable in TableView" : child.AppendChild(childchild)
		childchild = doc.CreateElement("plot") : childchild.InnerText = "Not Editable in TableView" : child.AppendChild(childchild)
		childchild = doc.CreateElement("sortorder") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("runtime") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("top250") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("year") : childchild.InnerText = "" : child.AppendChild(childchild)
		childchild = doc.CreateElement("usrrated") : childchild.InnerText = "UnChanged" : child.AppendChild(childchild)
		root.AppendChild(child)
		doc.AppendChild(root)
		Return doc
	End Function

	'Table buttons
	Private Sub btnTableColumnsSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_movTableColumnsSelect.Click
		Try
			Dim frm As New frmConfigureTableColumns
			frm.Init()
			frm.ShowDialog()
			mov_TableSetup()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_movTableSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_movTableSave.Click
		Try
			Call mov_TableChangesSave()
			MsgBox("Changes Saved")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_movTableApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_movTableApply.Click
		Try
			Call mov_TableUpdate()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	'Table - DataGridView1 events
	Private Sub DataGridView1_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvMoviesTableView.ColumnWidthChanged
		Try
			mov_TableEditDGV.Columns(e.Column.Index).Width = e.Column.Width
			Dim offSetValue As Integer = dgvMoviesTableView.HorizontalScrollingOffset
			mov_TableEditDGV.HorizontalScrollingOffset = offSetValue
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub dataGridViews1_Scroll(sender As Object, e As ScrollEventArgs) Handles dgvMoviesTableView.Scroll
		Try
			Dim offSetValue As Integer = dgvMoviesTableView.HorizontalScrollingOffset
			mov_TableEditDGV.HorizontalScrollingOffset = offSetValue
		Catch
		End Try
		dgvMoviesTableView.Invalidate()
	End Sub

	Private Sub DataGridView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvMoviesTableView.MouseDown
		Try
			Dim ColIndexFromMouseDown = dgvMoviesTableView.HitTest(e.X, e.Y).ColumnIndex
			Dim objHitTestInfo As DataGridView.HitTestInfo = dgvMoviesTableView.HitTest(e.X, e.Y)
			Dim MouseRowIndex As Integer = objHitTestInfo.RowIndex
			mov_TableRowNum = MouseRowIndex
			If ColIndexFromMouseDown < 0 Then Exit Sub
			mov_TableColumnName = dgvMoviesTableView.Columns(ColIndexFromMouseDown).Name
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DataGridView1_ColumnDisplayIndexChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvMoviesTableView.ColumnDisplayIndexChanged
		Dim ColNewIndex = dgvMoviesTableView.Columns(mov_TableColumnName).DisplayIndex
		mov_TableEditDGV.Columns(mov_TableColumnName).DisplayIndex = ColNewIndex
	End Sub

	Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMoviesTableView.SelectionChanged
		Dim MultiRowsSelected As Boolean = dgvMoviesTableView.SelectedRows.Count > 1
		If MultiRowsSelected Then
			mov_TableEditDGV.Visible = True
			lbl_movTableEdit.Visible = True
			lbl_movTableMulti.Visible = True
			btn_movTableApply.Visible = True
		Else
			mov_TableEditDGV.Visible = False
			lbl_movTableEdit.Visible = False
			lbl_movTableMulti.Visible = False
			btn_movTableApply.Visible = False
		End If
	End Sub

	Private Sub DataGridView1_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMoviesTableView.Sorted
		Try
			For Each tempRow As System.Windows.Forms.DataGridViewRow In Me.dgvMoviesTableView.Rows
				For Each tempCell As Windows.Forms.DataGridViewCell In tempRow.Cells
					If tempCell.Value = "Fanart" Or tempCell.Value = "Poster" Or tempCell.Value = "Poster & Fanart" Then
						tempCell.Style.BackColor = Color.Red
					End If
				Next
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub mov_TableEditDGV_CellClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles mov_TableEditDGV.CellClick
		Try
			mov_TableEditDGV.CurrentRow.Selected = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DataGridView1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dgvMoviesTableView.CurrentCellDirtyStateChanged
		DataDirty = True
		btn_movTableSave.Enabled = DataDirty
	End Sub

	'Table context toolstrips
	Private Sub MarkAllSelectedAsWatchedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarkAllSelectedAsWatchedToolStripMenuItem.Click
		Try
			Dim selectedrowindex As New List(Of Integer)
			For Each row in dgvMoviesTableView.SelectedRows
				selectedrowindex.Add(row.index)
			Next
			If selectedrowindex.Count = 0 Then selectedrowindex.Add(dgvMoviesTableView.CurrentRow.Index)
			dgvMoviesTableView.ClearSelection()
			dgvMoviesTableView.CurrentCell = dgvMoviesTableView.Rows(selectedrowindex.Item(0)).Cells(0)
			For Each row In selectedrowindex
				dgvMoviesTableView.rows(row).Selected = True
				dgvMoviesTableView.rows(row).Cells("playcount").Value = "Watched"
			Next
			DataDirty = True
			btn_movTableSave.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub MarkAllSelectedAsUnWatchedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarkAllSelectedAsUnWatchedToolStripMenuItem.Click
		Try
			Dim selectedrowindex As New List(Of Integer)
			For Each row in dgvMoviesTableView.SelectedRows
				selectedrowindex.Add(row.index)
			Next
			If selectedrowindex.Count = 0 Then selectedrowindex.Add(dgvMoviesTableView.CurrentRow.Index)
			dgvMoviesTableView.ClearSelection()
			dgvMoviesTableView.CurrentCell = dgvMoviesTableView.Rows(selectedrowindex.Item(0)).Cells(0)
			For Each row In selectedrowindex
				dgvMoviesTableView.rows(row).Selected = True
				dgvMoviesTableView.rows(row).Cells("playcount").Value = "UnWatched"
			Next
			DataDirty = True
			btn_movTableSave.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub MovieTableContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles MovieTableContextMenu.Opening
		If mov_TableRowNum = -1 Then e.Cancel = True
	End Sub

	Private Sub GoToToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoToToolStripMenuItem.Click
		Try
			Dim tempstring As String = ""
			tempstring = dgvMoviesTableView.Rows(mov_TableRowNum).Cells("fullpathandfilename").Value
			For f = 0 To DataGridViewMovies.Rows.Count - 1
				If DataGridViewMovies.Rows(f).Cells("fullpathandfilename").Value.ToString = tempstring Then
					DataGridViewMovies.ClearSelection()
					DataGridViewMovies.Rows(f).Selected = True
					Application.DoEvents()
					currentTabIndex = 0
					Me.TabControl2.SelectedIndex = 0
					Exit For
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub GoToSelectedMoviePosterSelectorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoToSelectedMoviePosterSelectorToolStripMenuItem.Click
		Try
			Dim tempstring As String = ""
			tempstring = dgvMoviesTableView.Rows(mov_TableRowNum).Cells("fullpathandfilename").Value
			For f = 0 To DataGridViewMovies.Rows.Count - 1
				If DataGridViewMovies.Rows(f).Cells("fullpathandfilename").Value.ToString = tempstring Then
					DataGridViewMovies.ClearSelection()
					DataGridViewMovies.Rows(f).Selected = True
					DisplayMovie()
					For Each tabs In TabControl2.TabPages
						If tabs.text = "Posters" Then
							currentTabIndex = tabs.tabindex + 1
							Me.TabControl2.SelectedIndex = tabs.tabindex + 1
							Exit For
						End If
					Next
					Exit For
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub GoToSelectedMovieFanartSelectorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoToSelectedMovieFanartSelectorToolStripMenuItem.Click
		Try
			Dim tempstring As String = ""
			tempstring = dgvMoviesTableView.Rows(mov_TableRowNum).Cells("fullpathandfilename").Value
			For f = 0 To DataGridViewMovies.RowCount - 1
				If DataGridViewMovies.Rows(f).Cells("fullpathandfilename").Value.ToString = tempstring Then
					DataGridViewMovies.ClearSelection()
					DataGridViewMovies.Rows(f).Selected = True
					DisplayMovie()
					For Each tabs In TabControl2.TabPages
						If tabs.text = "Fanart" Then
							currentTabIndex = tabs.tabindex + 1
							Me.TabControl2.SelectedIndex = tabs.tabindex + 1
							Exit For
						End If
					Next
					Exit For
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

#End Region 'Movie Tableview code

	Private Sub SearchForNewEpisodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchForNewEpisodesToolStripMenuItem.Click
		Try
			SearchForNewEpisodesToolStripMenuItem.Owner.Hide()
			Call ep_Search()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub
	Private Sub SearchALLForNewEpisodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchALLForNewEpisodesToolStripMenuItem.Click
		Try
			TVSearchALL = True
			Call ep_Search()
			TVSearchALL = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub mov_ScrapeSpecific(ByVal field As String)

		_rescrapeList.Field = field
		_rescrapeList.FullPathAndFilenames.Clear

		For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
			Dim fullpath As String = row.Cells("fullpathandfilename").Value.ToString
			If Not File.Exists(fullpath) Then Continue For
			_rescrapeList.FullPathAndFilenames.Add(fullpath)
		Next

		RunBackgroundMovieScrape("RescrapeSpecific")
	End Sub




	Private Sub RescrapeFanartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RescrapeFanartToolStripMenuItem.Click
		Try
			'rescrape fanart
			Call mov_FanartGet()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DownloadFanartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadFanartToolStripMenuItem.Click
		Try
			'download fanart
			Call mov_FanartGet()
			UpdateMissingFanart()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub mov_FanartGet(Optional ByVal MovSet As Boolean = False)
		If IsNothing(workingMovieDetails) Then Return
		messbox = New frmMessageBox("      Please Wait,", "", "Attempting to download Fanart")
		messbox.Show() : messbox.Refresh()
		Application.DoEvents()

		Dim tmdb As New TMDb

		tmdb.Imdb = If(workingMovieDetails.fullmoviebody.imdbid.Contains("tt"), workingMovieDetails.fullmoviebody.imdbid, "")
		tmdb.TmdbId = workingMovieDetails.fullmoviebody.tmdbid
		tmdb.SetId = workingMovieDetails.fullmoviebody.TmdbSetId

		Try
			Dim FanartUrl As String = ""
			If Not MovSet Then
				FanartUrl = tmdb.GetBackDropUrl()
			Else
				FanartUrl = tmdb.McSetFanart(0).hdUrl
			End If
			Dim isvideotspath As String = If(workingMovieDetails.fileinfo.videotspath = "", "", workingMovieDetails.fileinfo.videotspath + "fanart.jpg")
			If IsNothing(FanartUrl) then
				MsgBox("No Fanart Found on TMDB")
			Else
				If Not MovSet Then
					Dim paths As List(Of String) = Pref.GetfanartPaths(workingMovieDetails.fileinfo.fullpathandfilename, If(workingMovieDetails.fileinfo.videotspath <> "", workingMovieDetails.fileinfo.videotspath, ""))
					Dim point = Movie.GetBackDropResolution(Pref.BackDropResolutionSI)
					Dim aok As Boolean = DownloadCache.SaveImageToCacheAndPaths(FanartUrl, paths, True, point.X, point.Y)
					If Not aok Then Throw New Exception("TMDB is offline")
					util_ImageLoad(PbMovieFanArt, paths(0), Utilities.DefaultFanartPath)
					util_ImageLoad(PictureBox2, paths(0), Utilities.DefaultFanartPath)
					Dim video_flags = VidMediaFlags(workingMovieDetails.filedetails, workingMovieDetails.fullmoviebody.title.ToLower.Contains("3d"), workingMovie.DisplayFolderSize)
					movieGraphicInfo.OverlayInfo(PbMovieFanArt, ratingtxt.Text, video_flags)
				Else
					Dim MovSetFanartSavePath As String = workingMovieDetails.fileinfo.movsetfanartpath
					If MovSetFanartSavePath <> "" Then
						Movie.SaveFanartImageToCacheAndPath(FanartUrl, MovSetFanartSavePath)
						MovPanel6Update()
					Else
						MsgBox("!!  Problem formulating correct save location for Fanart" & vbCrLf & "                Please check your settings")
					End If
				End If
			End If
		Catch ex As Exception
			If ex.Message = "TMDB is offline" Then
				messbox.Close()
				MsgBox("Unable to connect to TheMovieDb.org." & vbCrLf & "Please confirm site is online")
			ElseIf ex.Message.Contains("Index was out of range") Then
				messbox.Close()
				MsgBox("No Fanart available on TMDB Site")
			End If
		End Try
		If Not IsNothing(messbox) Then messbox.Close()
	End Sub

	'Rescrape Poster
	Private Sub RescrapePToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RescrapePToolStripMenuItem.Click
		Try
			'rescrape poster IMPA
			mov_PosterGet("impa")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RescrapePosterFromTMDBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RescrapePosterFromTMDBToolStripMenuItem.Click
		Try
			'rescrape poster tmdb
			mov_PosterGet("tmdb")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RescraToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RescraToolStripMenuItem.Click
		Try
			'rescrape poster mpdb
			MsgBox("MoviePoster Db is not longer available")
			'mov_PosterGet("mpdb")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PeToolStripMenuItem.Click
		Try
			'rescrape poster imdb
			mov_PosterGet("imdb")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DownloadPosterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadPosterToolStripMenuItem.Click
		Try
			'downloadposter impa
			mov_PosterGet("impa")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DownloadPosterFromTMDBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadPosterFromTMDBToolStripMenuItem.Click
		Try
			'downloadposter tmdb
			mov_PosterGet("tmdb")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DownloadPosterFromMPDBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadPosterFromMPDBToolStripMenuItem.Click
		Try
			'downloadposter mpdb
			MsgBox("MoviePoster Db is not longer available")
			'mov_PosterGet("mpdb")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DownloadPosterFromIMDBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadPosterFromIMDBToolStripMenuItem.Click
		Try
			'downloadposter imdb
			mov_PosterGet("imdb")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub mov_PosterGet(ByVal source As String, Optional ByVal MovSet As Boolean = False)
		Dim success As Boolean = False
		Try
			If workingMovieDetails Is Nothing Then Exit Sub
			messbox = New frmMessageBox("          Please Wait,", "", "Attempting to download Poster from " & source.ToUpper)
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			Dim isvideotspath As String = ""
			If workingMovieDetails.fileinfo.videotspath <> "" Then
				isvideotspath = workingMovieDetails.fileinfo.videotspath + "poster.jpg"
			End If
			Dim moviethumburl As String = ""

			Dim tmdb As New TMDb

			tmdb.Imdb = If(workingMovieDetails.fullmoviebody.imdbid.Contains("tt"), workingMovieDetails.fullmoviebody.imdbid, "")
			tmdb.TmdbId = workingMovieDetails.fullmoviebody.tmdbid
			tmdb.SetId = workingMovieDetails.fullmoviebody.TmdbSetId

			If tmdb.Imdb = "" AndAlso tmdb.TmdbId = "" Then Exit Sub

			If source = "impa" Then
				If workingMovieDetails.fullmoviebody.title <> "" And workingMovieDetails.fullmoviebody.year <> "" Then
					moviethumburl = scraperFunction2.impathumb(workingMovieDetails.fullmoviebody.title, workingMovieDetails.fullmoviebody.year)
				End If
			ElseIf source = "tmdb" Then
				If workingMovieDetails.fullmoviebody.imdbid.Contains("tt") OrElse workingMovieDetails.fullmoviebody.tmdbid <> "" Then
					Try
						If Not MovSet Then
							moviethumburl = tmdb.FirstOriginalPosterUrl
						Else
							moviethumburl = tmdb.McSetPosters(0).hdUrl
						End If
					Catch
					End Try
				End If
			ElseIf source = "mpdb" Then
				If workingMovieDetails.fullmoviebody.imdbid.Contains("tt") Then
					moviethumburl = scraperFunction2.mpdbthumb(workingMovieDetails.fullmoviebody.imdbid)
				End If
			ElseIf source = "imdb" Then
				If workingMovieDetails.fullmoviebody.imdbid.Contains("tt") Then
					moviethumburl = scraperFunction2.imdbthumb(workingMovieDetails.fullmoviebody.imdbid)
				End If
			End If

			If moviethumburl <> "" And moviethumburl <> "na" Then
				Try
					If Not MovSet Then
						Dim PostPaths As List(Of String) = Pref.GetPosterPaths(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fileinfo.videotspath)
						Dim height As Integer = 0
						height = Movie.GetHeightResolution(Pref.PosterResolutionSI)
						Dim aok As Boolean = DownloadCache.SaveImageToCacheAndPaths(moviethumburl, PostPaths, True, , height)
						If Not aok Then Throw New Exception()
						util_ImageLoad(PictureBoxAssignedMoviePoster, PostPaths(0), Utilities.DefaultPosterPath)
						util_ImageLoad(PbMoviePoster, PostPaths(0), Utilities.DefaultPosterPath)
						Dim path As String = Utilities.save2postercache(workingMovieDetails.fileinfo.fullpathandfilename, PostPaths(0), WallPicWidth, WallPicHeight)
						updateposterwall(path, workingMovieDetails.fileinfo.fullpathandfilename)
						success = True
					Else
						Dim MovSetPosterSavePath As String = workingMovieDetails.fileinfo.movsetposterpath
						If MovSetPosterSavePath <> "" Then
							Movie.SavePosterImageToCacheAndPath(moviethumburl, MovSetPosterSavePath)
							MovPanel6Update()
						Else
							messbox.Close()
							MsgBox("!!  Problem formulating correct save location for Poster" & vbCrLf & "                    Please check your settings")
						End If
					End If
				Catch ex As Exception
					MsgBox("Error [" & ex.Message & "] occurred while trying to download and save the poster")
				End Try
			Else
				MsgBox("Unable to obtain a Poster from " & source.ToUpper)
			End If
			messbox.Close()
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		Finally
			messbox.Close()
		End Try
		If success then UpdateMissingPoster()
	End Sub

	Public Sub mov_OfflineDvdProcess(ByVal nfopath As String, ByVal title As String, ByVal mediapath As String)
		Dim tempint2 As Integer = 2097152
		Dim SizeOfFile As Integer = FileLen(mediapath)
		If SizeOfFile > tempint2 Then
			Exit Sub
		End If
		Try
			Dim fanartpath As String = ""
			If File.Exists(Pref.GetFanartPath(nfopath)) Then
				fanartpath = Pref.GetFanartPath(nfopath)
			Else
				fanartpath = Utilities.DefaultOfflineArtPath
			End If
			Dim curImage As Image = Image.FromFile(fanartpath)
			Dim tempstring As String = Pref.OfflineDVDTitle.Replace("%T", title)
			Dim g As System.Drawing.Graphics
			g = Graphics.FromImage(curImage)
			Dim semiTransBrush As New SolidBrush(Color.FromArgb(80, 0, 0, 0))
			Dim drawString As String = tempstring
			Dim drawFont As New System.Drawing.Font("Arial", 40)
			Dim drawBrush As New SolidBrush(Color.White)
			Dim StringSize As New SizeF
			StringSize = g.MeasureString(drawString, drawFont)
			Dim width As Single = StringSize.Width + 5
			Dim height As Single = StringSize.Height + 5

			If height < (curImage.Height / 100) * 8 Then
				Do
					Dim newsize As Integer = drawFont.Size + 1
					drawFont = New System.Drawing.Font("Arial", newsize)
					StringSize = g.MeasureString(drawString, drawFont)
					height = StringSize.Height
				Loop Until height > (curImage.Height / 100) * 8
			End If
			If height > (curImage.Height / 100) * 8 Then
				Do
					Dim newsize As Integer = drawFont.Size - 1
					drawFont = New System.Drawing.Font("Arial", newsize)
					StringSize = g.MeasureString(drawString, drawFont)
					height = StringSize.Height
				Loop Until height < (curImage.Height / 100) * 8
			End If
			StringSize = g.MeasureString(drawString, drawFont)
			width = StringSize.Width
			height = StringSize.Height
			If width > curImage.Width - 30 Then
				Do
					Dim newsize As Integer = drawFont.Size - 1
					drawFont = New System.Drawing.Font("Arial", newsize)
					StringSize = g.MeasureString(drawString, drawFont)
					width = StringSize.Width + 20
				Loop Until width < curImage.Width - 30
			End If
			StringSize = g.MeasureString(drawString, drawFont)
			width = StringSize.Width + 5
			height = StringSize.Height + 5
			Dim x As Integer = (curImage.Width / 2) - (width / 2)
			Dim y As Integer = (curImage.Height - StringSize.Height) - ((curImage.Height / 100) * 2)
			Dim drawRect As New RectangleF(x, y, width, height)


			g.FillRectangle(semiTransBrush, New Rectangle(x, y, width, height))

			Dim drawFormat As New StringFormat
			drawFormat.Alignment = StringAlignment.Center

			g.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat)
			For f = 1 To 16
				Dim path As String
				If f < 10 Then
					path = applicationPath & "\Settings\00" & f.ToString & ".jpg"
				Else
					path = applicationPath & "\Settings\0" & f.ToString & ".jpg"
				End If
				curImage.Save(path, Drawing.Imaging.ImageFormat.Jpeg)
			Next

			Dim myProcess As Process = New Process
			myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
			myProcess.StartInfo.CreateNoWindow = False
			myProcess.StartInfo.FileName = applicationPath & "\Assets\ffmpeg.exe"
			Dim proc_arguments As String = "-r 1 -b 1800 -qmax 6 -i """ & applicationPath & "\Settings\%03d.jpg"" -vcodec msmpeg4v2 -y """ & mediapath & """"
			myProcess.StartInfo.Arguments = proc_arguments
			myProcess.Start()
			myProcess.WaitForExit()

			For f = 1 To 16
				Dim tempstring4 As String
				If f < 10 Then
					tempstring4 = applicationPath & "\Settings\00" & f.ToString & ".jpg"
				Else
					tempstring4 = applicationPath & "\Settings\0" & f.ToString & ".jpg"
				End If
				Try
					File.Delete(tempstring4)
				Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
				End Try
			Next
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
	End Sub

	Private Sub ToolsToolStripMenuItem_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolsToolStripMenuItem.DropDownItemClicked
		Try
			For Each temp In Pref.commandlist
				If temp.title = e.ClickedItem.Text Then
					Try
						Process.Start(temp.command)
					Catch ex As Exception
					End Try
					Exit For
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PreferencesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreferencesToolStripMenuItem.Click
		OpenPreferences(0)
	End Sub

	Private Sub OpenPreferences(Optional ByVal TabRequired As Integer = 0)
		Try
			Dim t As New frmPreferences
			t.SelectTab = TabRequired
			If Pref.MultiMonitoEnabled Then
				t.Bounds = screen.AllScreens(CurrentScreen).Bounds
				t.StartPosition = FormStartPosition.Manual
			End If
			t.ShowDialog()
            If Not TvAutoScrapeTimer.Interval = ((Pref.TvAutoScrapeInterval * 60) * 1000) Then Ini_Timer(TvAutoScrapeTimer, ((Pref.TvAutoScrapeInterval * 60) * 1000), True)

            If Not MovAutoScrapeTimer.Interval = ((Pref.MovAutoScrapeInterval * 60) * 1000) Then Ini_Timer(MovAutoScrapeTimer, ((Pref.MovAutoScrapeInterval * 60) * 1000), True)

            If Pref.TvEnableAutoScrape AndAlso Not TvAutoScrapeTimer.Enabled Then
                TvAutoScrapeTimer.Start()
            Else If Not Pref.TvEnableAutoScrape AndAlso TvAutoScrapeTimer.Enabled Then
                TvAutoScrapeTimer.Stop()
            End If
            If Pref.MovEnableAutoScrape AndAlso Not MovAutoScrapeTimer.Enabled Then
                MovAutoScrapeTimer.Start()
            Else If Not Pref.MovEnableAutoScrape AndAlso MovAutoScrapeTimer.Enabled Then
                MovAutoScrapeTimer.Stop()
            End If
			
            If Not CheckForRunningBackgroundworker Then
				Statusstrip_Enable(False)
			End If
            SetcbBtnLink()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub SearchForMissingEpisodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchForMissingEpisodesToolStripMenuItem.Click
		Try
			If Not Bckgrndfindmissingepisodes.IsBusy AndAlso Not bckgroundscanepisodes.IsBusy AndAlso Not BckWrkTv.IsBusy Then
				Pref.displayMissingEpisodes = SearchForMissingEpisodesToolStripMenuItem.Checked
				Pref.ConfigSave()
				If Pref.displayMissingEpisodes = False
					RefreshMissingEpisodesToolStripMenuItem.Enabled = False
					rbTvListAll.Checked = True
					rbTvMissingEpisodes     .Enabled = False
					rbTvMissingAiredEp      .Enabled = False
                    rbTvMissingNextToAir    .Enabled = False
					RefreshMissingEpisodesToolStripMenuItem.ToolTipText = Nothing
					tv_CacheRefresh
					Return
				End If
				RefreshMissingEpisodesToolStripMenuItem.Enabled = True
				RefreshMissingEpisodesToolStripMenuItem.ToolTipText = "Last Refresh: " & Pref.lastrefreshmissingdate
				rbTvMissingEpisodes     .Enabled = True
				rbTvMissingAiredEp      .Enabled = True
                rbTvMissingNextToAir    .Enabled = True
				Pref.DlMissingEpData = False
				tv_EpisodesMissingLoad(False)
			ElseIf Bckgrndfindmissingepisodes.IsBusy Then
				MsgBox("Process is already running")
			Else
				MsgBox("Missing episode search cannot be performed" & vbCrLf & "    when the episode scraper is running")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RefreshMissingEpisodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshMissingEpisodesToolStripMenuItem.Click
		Pref.DlMissingEpData = True
		Pref.lastrefreshmissingdate = DateTime.Now.ToString("yyyy-MM-dd")
		ClearMissingFolder()
		tv_EpisodesMissingLoad(True)
	End Sub

	Private Sub RefreshThisShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_RefreshShow.Click
		Try
			Dim Show As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim selectednode As Integer = TvTreeview.SelectedNode.Index
			If Show IsNot Nothing Then
				Call tv_CacheRefreshSelected(Show)
			Else
				MsgBox("No Show Selected")
			End If
			TvTreeview.SelectedNode = TvTreeview.Nodes(selectednode)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_ShowMissEps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_ShowMissEps.Click
		Try
			Dim Show As TvShow = tv_ShowSelectedCurrently(TvTreeview)

			If Not Bckgrndfindmissingepisodes.IsBusy Then
				Dim tempstring As String = ""
				For Each sho In Cache.TvCache.Shows
					If sho.NfoFilePath = TvTreeview.SelectedNode.Name Then
						tempstring = "Checking """ & sho.Title.Value & """ for missing episodes"
						Exit For
					End If
				Next
				If tempstring = "" Then tempstring = "Checking for missing episodes"
				messbox = New frmMessageBox(tempstring, "", "Please Wait")
				messbox.Show()
				messbox.Refresh()
				Application.DoEvents()
				Dim ShowList As New List(Of TvShow)
				ShowList.Add(Show)
				Bckgrndfindmissingepisodes.RunWorkerAsync(ShowList)
				messbox.Close()
			Else
				MsgBox("The missing episode thread is already running")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub LockAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LockAllToolStripMenuItem.Click
		Try
			Dim Show As Media_Companion.TvShow
			For Each Show In Cache.TvCache.Shows
				Show = nfoFunction.tvshow_NfoLoad(Show.NfoFilePath)
				Show.State = Media_Companion.ShowState.Locked
				nfoFunction.tvshow_NfoSave(Show, True)
				Tv_CacheSave()
			Next
			tv_CacheLoad()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub UnlockAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnlockAllToolStripMenuItem.Click
		Try
			Dim Show As Media_Companion.TvShow
			For Each Show In Cache.TvCache.Shows
				Show = nfoFunction.tvshow_NfoLoad(Show.NfoFilePath)
				Show.State = Media_Companion.ShowState.Open
				nfoFunction.tvshow_NfoSave(Show, True)
				Tv_CacheSave()
			Next
			tv_CacheLoad()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub TV_BatchRescrapeWizardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TV_BatchRescrapeWizardToolStripMenuItem.Click
		Try
			If Not tvbckrescrapewizard.IsBusy Then
				tvBatchList.Reset()

				Dim displaywizard As New tv_batch_wizard
				If Pref.MultiMonitoEnabled Then
					displaywizard.Bounds = screen.AllScreens(CurrentScreen).Bounds
					displaywizard.StartPosition = FormStartPosition.Manual
				End If
				displaywizard.ShowDialog()

				If tvBatchList.activate = True Then
                    oTV.tvBatchList = tvBatchList
                    RunBackgroundTVScrape("TVBatchRescrape")
					'Statusstrip_Enable()
					'ToolStripStatusLabel8.Text = "Starting TV Batch Scrape"
					'ToolStripStatusLabel8.Visible = True
					'ToolStripProgressBar7.Value = 0
					'ToolStripProgressBar7.Visible = True
					'tvbckrescrapewizard.RunWorkerAsync()
				End If
			Else
				MsgBox("The update Wizard is Already Running")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub tsmiMov_ExportMovies_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_ExportMovies.Click
		Try
			'listoffilestomove.Clear()
			If DataGridViewMovies.SelectedRows.Count > 0 Then
                Dim listoffilestomove As New List(Of String)
                Dim totalfilesize As Long = 0
				For Each sRow As DataGridViewRow In DataGridViewMovies.SelectedRows
                    Dim isfolder As Boolean = False
					Dim playlist As New List(Of String)

					Dim fullpathandfilename As String = CType(sRow.DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString

					playlist = Utilities.GetMediaList(Utilities.GetFileName(fullpathandfilename))

                    If (Pref.usefoldernames OrElse Pref.allfolders) AndAlso playlist.Count > 0 Then
                        Dim LastFolder As String = Path.GetDirectoryName(fullpathandfilename) & "\"
                        isfolder = Not Pref.GetRootFolderCheck(fullpathandfilename, True)
                        If isfolder Then
                            listoffilestomove.Add(LastFolder)
                            totalfilesize += Utilities.GetFolderSize(LastFolder, True)
                        End If
                    End If
                    
					If playlist.Count > 0 AndAlso Not isfolder Then
						For Each File In playlist
							If Not listoffilestomove.Contains(File) Then listoffilestomove.Add(File)
						Next

						listoffilestomove.Add(fullpathandfilename)
						If File.Exists(Pref.GetFanartPath(fullpathandfilename)) Then listoffilestomove.Add(Pref.GetFanartPath(fullpathandfilename))
						If File.Exists(Pref.GetPosterPath(fullpathandfilename)) Then listoffilestomove.Add(Pref.GetPosterPath(fullpathandfilename))
						Dim di As DirectoryInfo = New DirectoryInfo(fullpathandfilename.Replace(Path.GetFileName(fullpathandfilename), ""))
						Dim filenama As String = Path.GetFileNameWithoutExtension(fullpathandfilename)
						Dim fils As FileInfo() = di.GetFiles(filenama & ".*")
						For Each fiNext In fils
							If Not listoffilestomove.Contains(fiNext.FullName) Then listoffilestomove.Add(fiNext.FullName)
						Next
						Dim trailerpath As String = Pref.ActualTrailerPath(fullpathandfilename)
						Dim filenama2 As String = Path.GetFileNameWithoutExtension(trailerpath)
						Dim fils2 As FileInfo() = di.GetFiles(filenama2 & ".*")
						For Each fiNext In fils2
							If Not listoffilestomove.Contains(fiNext.FullName) Then listoffilestomove.Add(fiNext.FullName)
						Next
					End If
				Next
                
				For Each item In listoffilestomove
					totalfilesize = totalfilesize + Utilities.GetFileSize(item)
				Next
                
                Using frm1 As New frmCopyProgress
                    If Pref.MultiMonitoEnabled Then
					    frm1.Bounds = screen.AllScreens(CurrentScreen).Bounds
					    frm1.StartPosition = FormStartPosition.Manual
				    End If
                    frm1.ListOfFilesToMove = listoffilestomove
                    frm1.totalfilesize = totalfilesize
				    frm1.ShowDialog()
                End Using
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub frm_ExportTabSetup()
		If TextBox45.Text = "" Then

			Dim tempstring2 As String = workingProfile.config.Replace(Path.GetFileName(workingProfile.config), "pathsubstitution.xml")
			If File.Exists(tempstring2) Then
				relativeFolderList.Clear()
				Dim prefs As New XmlDocument
				Try
					prefs.Load(tempstring2)
				Catch ex As Exception
				End Try
				For Each thisresult As XmlNode In prefs("relativepaths")
					Select Case thisresult.Name
						Case "folder"
							Dim mc As New str_RelativeFileList(SetDefaults)
							For Each it2 As XmlNode In thisresult.ChildNodes
								Select Case it2.Name
									Case "mc"
										mc.mc = it2.InnerText
									Case "xbmc"
										mc.xbmc = it2.InnerText
										relativeFolderList.Add(mc)
								End Select
							Next
					End Select
				Next
				For Each item In relativeFolderList
					TextBox45.Text += "<folder>" & vbCrLf
					TextBox45.Text += "    <mc>" & item.mc & "</mc>" & vbCrLf
					TextBox45.Text += "    <xbmc>" & item.xbmc & "</xbmc>" & vbCrLf
					TextBox45.Text += "</folder>" & vbCrLf & vbCrLf
				Next
			End If
			If TextBox45.Text = "" Then
				For Each pat In movieFolders
					If Not pat.selected Then Continue For
					TextBox45.Text += "<folder>" & vbCrLf
					TextBox45.Text += "    <mc>" & pat.rpath & "</mc>" & vbCrLf
					TextBox45.Text += "    <xbmc>" & pat.rpath & "</xbmc>" & vbCrLf
					TextBox45.Text += "</folder>" & vbCrLf & vbCrLf
				Next
				For Each pat In Pref.offlinefolders
					TextBox45.Text += "<folder>" & vbCrLf
					TextBox45.Text += "    <mc>" & pat & "</mc>" & vbCrLf
					TextBox45.Text += "    <xbmc>" & pat & "</xbmc>" & vbCrLf
					TextBox45.Text += "</folder>" & vbCrLf & vbCrLf
				Next
				For Each pat In tvRootFolders
					TextBox45.Text += "<folder>" & vbCrLf
					TextBox45.Text += "    <mc>" & pat.rpath & "</mc>" & vbCrLf
					TextBox45.Text += "    <xbmc>" & pat.rpath & "</xbmc>" & vbCrLf
					TextBox45.Text += "</folder>" & vbCrLf & vbCrLf
				Next
			End If
		End If
	End Sub

	Private Sub Button109_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button109.Click
		Try
			relativeFolderList.Clear()
			Dim PathSubPath As String = workingProfile.config.Replace(Path.GetFileName(workingProfile.config), "pathsubstitution.xml")
			Dim temptext As String = ""
			temptext = "<relativepaths>" & TextBox45.Text & "</relativepaths>"
			Dim loaddoc As New XmlDocument
			loaddoc.LoadXml(temptext)
			For Each thisresult As XmlNode In loaddoc("relativepaths")
				Dim newfo As New str_RelativeFileList(SetDefaults)
				For Each innerresult As XmlNode In thisresult
					Select Case innerresult.Name
						Case "mc"
							newfo.mc = innerresult.InnerText
						Case "xbmc"
							newfo.xbmc = innerresult.InnerText
					End Select
				Next
				If Not String.IsNullOrEmpty(newfo.mc) AndAlso Not String.IsNullOrEmpty(newfo.xbmc) Then relativeFolderList.Add(newfo)
			Next

			If relativeFolderList.Count > 0 Then
				Dim docs As New XmlDocument
				Dim thispref As XmlNode = Nothing
				Dim xmlproc As XmlDeclaration
				xmlproc = docs.CreateXmlDeclaration("1.0", "UTF-8", "yes")
				docs.AppendChild(xmlproc)
				Dim root As XmlElement
				Dim child As XmlElement
				root = docs.CreateElement("relativepaths")
				For Each item In relativeFolderList
					child = docs.CreateElement("folder")
					child.AppendChild(docs   , "mc"      , item.mc)
					child.AppendChild(docs   , "xbmc"    , item.xbmc)
					root.AppendChild(child)
				Next
                docs.AppendChild(root)
                WorkingWithNfoFiles.SaveXMLDoc(docs, PathSubPath)
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub


	Private Sub SplitContainer1_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SpltCntr_MovDisplay.SplitterMoved
		Mc.clsGridViewMovie.SetFirstColumnWidth(DataGridViewMovies)
	End Sub

	Private Sub SplitContainer5_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SpltCntr_MovBrowserList.SplitterMoved

		ResizeBottomLHSPanel()
		Try
			DebugSplitter5PosLabel.Text = SpltCntr_MovBrowserList.SplitterDistance
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmi_MCSite_ToolStrip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmi_MCSite_ToolStrip.Click
		Try
			OpenUrl(SF_Home_Page)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub CheckBoxDebugShowXML_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxDebugShowXML.CheckedChanged
		Try
			If CheckBoxDebugShowXML.Checked = False Then
				TabLevel1.TabPages.Remove(Me.TabConfigXML)
				TabLevel1.TabPages.Remove(Me.TabMovieCacheXML)
				TabLevel1.TabPages.Remove(Me.TabTVCacheXML)
				TabLevel1.TabPages.Remove(Me.TabProfile)
				TabLevel1.TabPages.Remove(Me.TabActorCache)
				TabLevel1.TabPages.Remove(Me.TabRegex)
			Else
				TabLevel1.TabPages.Add(Me.TabConfigXML)
				TabLevel1.TabPages.Add(Me.TabMovieCacheXML)
				TabLevel1.TabPages.Add(Me.TabTVCacheXML)
				TabLevel1.TabPages.Add(Me.TabProfile)
				TabLevel1.TabPages.Add(Me.TabActorCache)
				TabLevel1.TabPages.Add(Me.TabRegex)
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Public Sub util_ConfigLoad(ByVal Optional prefs As Boolean = False)
        If MainFormLoadedStatus Then
            Pref.SetUpPreferences()
		    Pref.ConfigLoad()
        End If
		Pref.MultiMonitoEnabled = convert.ToBoolean(multimonitor)

		DataGridViewMovies.DataSource = Nothing

		SearchForMissingEpisodesToolStripMenuItem.Checked = Pref.displayMissingEpisodes
		RefreshMissingEpisodesToolStripMenuItem.Enabled = Pref.displayMissingEpisodes
		If Pref.displayMissingEpisodes Then Me.RefreshMissingEpisodesToolStripMenuItem.ToolTipText = "Last Refresh: " & Pref.lastrefreshmissingdate
		rbTvMissingEpisodes  .Enabled = Pref.displayMissingEpisodes
		rbTvMissingAiredEp   .Enabled = Pref.displayMissingEpisodes
        rbTvMissingNextToAir .Enabled = Pref.displayMissingEpisodes

        If Pref.tvrename >= Pref.tv_RegexRename.Count Then Pref.tvrename = 0
		Renamer.setRenamePref(Pref.tv_RegexRename.Item(Pref.tvrename), Pref.tv_RegexScraper)
		XBMCTMDBConfigSave()
		XBMCTVDBConfigSave()

		'----------------------------------------------------------

		mScraperManager = New ScraperManager(Path.Combine(My.Application.Info.DirectoryPath, "Assets\scrapers"))
		'----------------------------------------------------------
		Dim loadinginfo As String = ""
		If Not File.Exists(workingProfile.moviecache) Or Pref.startupCache = False Then
			loadinginfo = "Status :- Building Movie caches"
			frmSplash.Label3.Text = loadinginfo
			frmSplash.Label3.Refresh()
			mov_RebuildMovieCaches()
		Else
			loadinginfo = "Status :- Loading Movie Database"
			frmSplash.Label3.Text = loadinginfo
			frmSplash.Label3.Refresh()
			mov_CacheLoad()
		End If

		If File.Exists(workingProfile.Genres) Then
			loadinginfo = "Status :- Loading Genre List"
			frmSplash.Label3.Text = loadinginfo
			frmSplash.Label3.Refresh()
			Call util_GenreLoad()
		End If

		If File.Exists(workingProfile.homemoviecache) Then
			loadinginfo = "Status :- Loading Home Movie Database"
			frmSplash.Label3.Text = loadinginfo
			frmSplash.Label3.Refresh()
			If Not HomeMovieCacheLoad() Then
                loadinginfo = "Status :- Building Home Movie Database"
			    frmSplash.Label3.Text = loadinginfo
			    frmSplash.Label3.Refresh()
                RefreshHomeVideoCache()
			End If
		End If

		If File.Exists(workingProfile.MusicVideoCache) Then
			loadinginfo = "Status :- Loading Music Video Database"
			frmSplash.Label3.Text = loadinginfo
			frmSplash.Label3.Refresh()
			UcMusicVideo1.cmbxMVSort.SelectedIndex = 0
			Call UcMusicVideo1.MVCacheLoad()
		End If

		If Not prefs then
			If Not File.Exists(workingProfile.tvcache) Or Pref.startupCache = False Then
				loadinginfo = "Status :- Building TV Database"
				frmSplash.Label3.Text = loadinginfo
				frmSplash.Label3.Refresh()
				Call tv_CacheRefresh()
			Else
				loadinginfo = "Status :- Loading TV Database"
				frmSplash.Label3.Text = loadinginfo
				frmSplash.Label3.Refresh()
				Call tv_CacheLoad()
			End If
			Call tv_Filter()
		End If

		If Pref.homemoviefolders.Count > 0 Then
			AuthorizeCheck = True
			clbx_HMMovieFolders.Items.Clear()
			For Each folder In homemoviefolders
				clbx_HMMovieFolders.Items.Add(folder.rpath, folder.selected)
			Next
			AuthorizeCheck = False
		End If

		cbBtnLink.Checked = Pref.XBMC_Link
		SetcbBtnLink
		If Pref.XbmcLinkReady Then
			XbmcControllerQ.Write(XbmcController.E.ConnectReq, PriorityQueue.Priorities.low)
		End If
	End Sub

	Private Sub MediaCompanionHelpFileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles MediaCompanionHelpFileToolStripMenuItem.Click
		Try
			Process.Start(applicationPath & "\Media_Companion.chm")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_DispByAiredDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_DispByAiredDate.Click
		'This function displays in a Form with a fullscreen textbox, a list off all of a TvShows episodes in 'date aired' order, separated by calendar year.
		'It can be called from a TVShow, Season or Episode context menu
		'It handles the following errors - no aired date, episodes on the same aired date, episodes on same date with same series & same episode i.e. a duplicate.... 

		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim NoDateCountUp As Integer = 0
			Dim Abort As Boolean = True     'this is used to verify that we actually have episodes to process
			Dim mySortedList As New SortedList()        'this is our sorted list, we add to the list a key (aired date) & the associated data (episode name), then we sort it & then we read out the data
			Dim childNodeLevel1 As TreeNode

			Select Case TvTreeview.SelectedNode.Level
				Case Is = 0
					childNodeLevel1 = TvTreeview.SelectedNode
				Case Is = 1
					childNodeLevel1 = TvTreeview.SelectedNode.Parent
				Case Is = 2
					childNodeLevel1 = TvTreeview.SelectedNode.Parent.Parent
				Case Else
					MsgBox("Unsupported TvTreeviewlevel in Aired Date Function", MsgBoxStyle.Exclamation, "Error!")
					Exit Sub
			End Select


			'this section steps down through the tree to get from the tvshow to each episode
			For Each childNodeLevel2 As TreeNode In childNodeLevel1.Nodes
				For Each childNodeLevel3 As TreeNode In childNodeLevel2.Nodes
					Abort = False                                          'if we get here then there is at least 1 episode
					Dim episode As New TvEpisode
					episode.Load(childNodeLevel3.Name)

					Dim EpAired As String = episode.Aired.Value  'this holds the 'aired' value

					If EpAired Is Nothing Then
						EpAired = "9999-" & Utilities.PadNumber(NoDateCountUp, 5)  'if the aired date is nothing then we add it as 9999-xxxxx where x increments
						NoDateCountUp += 1
					End If

					'Convert episode to 2 digits for formatting
					Dim episode2digit As New List(Of String)
					episode2digit.Clear()
					episode2digit.Add(childNodeLevel3.Tag.Episode.Value)
					If episode2digit(0).Length = 1 Then episode2digit(0) = "0" & episode2digit(0)

					'Convert season to 2 digits for formatting
					Dim season2digit As String = childNodeLevel3.Tag.Season.Value
					If season2digit.Length = 1 Then season2digit = "0" & season2digit

					'here we add our data in the order that it is read in the tree - the sorted list will sort it for us
					'using the key value .aired (date format is yyyy-mm-dd so simple alphabetical sort is all that is required)
					'FormatTVFilename formats the show title,episode tile, season no & episode no as per the users preferences
					Dim SameDateLoop As Boolean = True
					Dim Key As String
					Key = EpAired & season2digit & episode2digit(0)         'the key index (which is the string used to sort by) is the date+season+episode - this should be unique!

					Do Until SameDateLoop = False
						If mySortedList.ContainsKey(Key) Then
							Key += "^"                          'we add an aditional ^ to the key if its still not unique.....
						Else
							SameDateLoop = False
						End If
					Loop
					mySortedList.Add(Key, EpAired & "    " & Renamer.setTVFilename(WorkingTvShow.Title.Value, childNodeLevel3.Tag.title.value, episode2digit, season2digit))
				Next
			Next

			If Not Abort Then   'i.e. we have episodes in this show.... 
				Dim textstring As String = "!!! " & WorkingTvShow.Title.Value & "  Seasons: " & WorkingTvShow.Seasons.Count & "  Episodes: " & WorkingTvShow.Episodes.Count & vbCrLf 'start our text with the show title
				textstring += "!!! " & StrDup(textstring.Length - 2, "-") & vbCrLf              'add an underline of the same length    
				Dim prevkey As String = mySortedList.GetKey(0).Substring(0, 4)                      'load with first year value first four digits of aired date
				For Line = 0 To mySortedList.Count - 1  'read the data from the sorted list
					If mySortedList.GetKey(Line).Substring(0, 4) <> prevkey Then textstring += "!!! ----------" & vbCrLf 'line break between years...
					prevkey = mySortedList.GetKey(Line).Substring(0, 4)                             'set so that we can compare with next iteration
					textstring += "!!! " & mySortedList.GetByIndex(Line) & vbCrLf ' "!!! " allows this to be shown in either brief or Full log modes
				Next

				textstring += "!!! " & vbCrLf & "!!! 9999 episodes have no valid aired date stored" & vbCrLf

				'                                                   'Show Final Listing Screen
				Dim MyFormObject As New frmoutputlog(textstring, True)                                   'create the log form & modify it to suit our needs   
				MyFormObject.TextBox1.Font = New System.Drawing.Font("Courier New", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte)) 'constant width font
				MyFormObject.btn_savelog.AutoSize = True                                                    'change button size to text will fit automatically
				MyFormObject.btn_savelog.Text = "Save Details..."                                           'change the button text
				MyFormObject.Text = "Episodes in Aired Order for " & WorkingTvShow.Title.Value          'change the form title text
				MyFormObject.ShowDialog()                                                               'show the form

			Else                    'we get here if abort still = true, i.e. no episodes
				MsgBox("There are no Epsiodes or Missing Episodes for this show.", MsgBoxStyle.OkOnly, "No Episodes")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub util_FixSeasonEpisode() 'atleast try...       if season or episode is -1, but title contains a regexable name to retreive season & episode
		Dim textstring As String = "!!! Season Episode -1 fix..." & vbCrLf
		Dim correctionsfound As Integer = 0
		Dim correctionsfixed As Integer = 0
		Dim childNodeLevel1 As TreeNode
		Dim originallabeltext = Label148.Text
		For Each childNodeLevel1 In TvTreeview.Nodes    'step thru each tvshow/season/episode in the treeview
			For Each childNodeLevel2 As TreeNode In childNodeLevel1.Nodes
				Label148.Text = childNodeLevel1.Text & " - " & childNodeLevel2.Text 'display some sort of progress using the text label associated with the fix
				Label148.Invalidate()
				Windows.Forms.Application.DoEvents()    'this refreshes the label whilst we are still in this sub
				For Each childNodeLevel3 As TreeNode In childNodeLevel2.Nodes
					Dim episode As New TvEpisode
					episode.Load(childNodeLevel3.Name)  'load the episode from the nfo using the path stored in the treeview

					If episode.Season.Value <> -1 AndAlso episode.Episode.Value <> -1 Then Continue For ' check if we have the issue

					textstring += "!!! " & childNodeLevel1.Text & " - " & childNodeLevel3.Name      'add details to the log"
					correctionsfound += 1   'increment the found issues counter
					For Each regexp In Pref.tv_RegexScraper

						Dim M As Match
						Dim sourcetext As String = ""
						If RadioButton_Fix_Filename.Checked Then
							sourcetext = childNodeLevel3.Name       'use nfo filename to retrieve season/episode
						Else
							sourcetext = episode.Title.Value        'use 'title' node in nfo to retieve season/episode
						End If
						M = Regex.Match(episode.Title.Value, regexp)
						If Not M.Success Then Continue For           'we found a valid regex match
						Try
							episode.Season.Value = M.Groups(1).Value.ToString   'set new values
							episode.Episode.Value = M.Groups(2).Value.ToString
							correctionsfixed += 1
							episode.Save(childNodeLevel3.Name)                  'save episode
							textstring += " - Corrected - S" & episode.Season.Value & "E" & episode.Episode.Value
							Exit For
						Catch
							textstring += vbCrLf & "**** exception created during nfo save **** - " & childNodeLevel3.Name
						End Try
					Next
					textstring += vbCrLf
				Next
			Next
		Next
		Label148.Text = originallabeltext           'return the label text back after we have used it to diplay progress
		Dim MyFormObject As New frmoutputlog(textstring, True)                                   'create the log form & modify it to suit our needs   
		MyFormObject.TextBox1.Font = New System.Drawing.Font("Courier New", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte)) 'constant width font
		MyFormObject.btn_savelog.AutoSize = True                                                    'change button size to text will fit automatically
		MyFormObject.btn_savelog.Text = "Save Details..."                                           'change the button text
		MyFormObject.Font = New System.Drawing.Font("Courier New", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		MyFormObject.Text = "Corrections" & vbCrLf & "Found: " & correctionsfound & vbCrLf & " Fixed: " & correctionsfixed            'change the form title text
		MyFormObject.ShowDialog()                                                               'show the form
		If MsgBox("Corrections" & vbCrLf & "Found: " & correctionsfound & vbCrLf & "Fixed: " & correctionsfixed & vbCrLf & vbCrLf & "Do you want to perform a refresh to reload the corrected nfo's?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
			tv_CacheRefresh()   'ask to do a refresh or not, user may want to try both methods before do a refresh.
		End If
	End Sub

	Private Sub tsmiTvDelShowNfoArt_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tsmiTvDelShowNfoArt.MouseDown
		Dim NoDelArt As Boolean = (e.Button = MouseButtons.Right)
		TVContextMenu.Close()
		Dim Sh As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		TvDelShowNfoArt(Sh, False, NoDelArt)
	End Sub

	Private Sub tsmiTvDelShowEpNfoArt_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tsmiTvDelShowEpNfoArt.MouseDown
		Dim NoDelArt As Boolean = (e.Button = MouseButtons.Right)
		TVContextMenu.Close()
		Dim msgstring As String = "Warning:  This will Remove all nfo's" & If(Not NoDelArt, " and artwork", "") & " for this Show and Episodes"
		msgstring &= vbcrlf & "and remove the show's folder from MC's ""List Of Separate Folders""." & vbCrLf
		msgstring &= vbCrLf & "To Rescrape this show, use ""Check Roots for New TV Shows"" or "
		msgstring &= vbCrLf & "Add this show's folder again to your ""List Of Separate Folders""." & vbCrLf
		msgstring &= vbCrLf & "Are your sure you wish to continue?"
		Dim x = MsgBox(msgstring, MsgBoxStyle.YesNoCancel, "Delete Show and Episode's nfo's" & If(Not NoDelArt, " and artwork", ""))
		If x = MsgBoxResult.No OrElse X = MsgBoxResult.Cancel Then Exit Sub
		Dim Sh As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim seas As TvSeason = tv_SeasonSelectedCurrently(TvTreeview)
		Dim ep As TvEpisode = ep_SelectedCurrently(TvTreeview)
		TvDelEpNfoAst(Sh, seas, ep, True, NoDelArt)
		TvDelShowNfoArt(Sh, True, NoDelArt)
	End Sub

	Private Sub tsmiTvDelEpNfoArt_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tsmiTvDelEpNfoArt.MouseDown
		Dim NoDelArt As Boolean = (e.Button = MouseButtons.Right)
		TVContextMenu.Close()
		Dim Sh As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim seas As TvSeason = tv_SeasonSelectedCurrently(TvTreeview)
		Dim ep As TvEpisode = ep_SelectedCurrently(TvTreeview)
		TvDelEpNfoAst(Sh, seas, ep, False, NoDelArt)
	End Sub

	Private Sub Tv_TreeViewContext_RescrapeShowOrEpisode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_RescrapeShowOrEpisode.Click
		tv_Rescrape()
	End Sub

	Private Sub Tv_TreeViewContext_WatchedShowOrEpisode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_WatchedShowOrEpisode.Click
		Tv_MarkAs_Watched_UnWatched("1")
	End Sub

	Private Sub Tv_TreeViewContext_UnWatchedShowOrEpisode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_UnWatchedShowOrEpisode.Click
		Tv_MarkAs_Watched_UnWatched("0")
	End Sub

	Private Sub Tv_TreeViewContext_Play_Episode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_Play_Episode.Click
		Try
			Dim ep As TvEpisode = ep_SelectedCurrently(TvTreeview)
			If ep.IsMissing Then Exit Sub
			Dim tempstring As String = ep.VideoFilePath
			StartVideo(tempstring)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_ViewNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_ViewNfo.Click
		Try
			If TvTreeview.SelectedNode Is Nothing Then Exit Sub
			If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
				Utilities.NfoNotepadDisplay(DirectCast(TvTreeview.SelectedNode.Tag, Media_Companion.TvShow).NfoFilePath, Pref.altnfoeditor)
			ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
				MsgBox("A Season NFO is invalid so it can't be shown")
			ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
				Utilities.NfoNotepadDisplay(DirectCast(TvTreeview.SelectedNode.Tag, Media_Companion.TvEpisode).NfoFilePath, Pref.altnfoeditor)
			Else
				MsgBox("None")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

		Debug.Print(Me.Controls.Count)
	End Sub

	Private Sub TvDelShowNfoArt(Show As TvShow, ByVal Ignore As Boolean, ByVal NoDelArt As Boolean, Optional SeriesChange As Boolean = False)
		Try
			If Not Ignore Then
				Dim msgstring As String = "Warning:  This will Remove the selected Tv Show's nfo" & If(Not NoDelArt, " and artwork", "")
				msgstring &= vbcrlf & "and remove the show's folder from MC's ""List Of Separate Folders""." & vbCrLf
				msgstring &= vbCrLf & "To Rescrape this show, use ""Check Roots for New TV Shows"" or "
				msgstring &= vbCrLf & "Add this show's folder again to your ""List Of Separate Folders""." & vbCrLf
				msgstring &= vbCrLf & "Are your sure you wish to continue?"
				Dim x As MsgBoxResult = MsgBox(msgstring, MsgBoxStyle.YesNoCancel, "Delete Show's nfo's" & If(Not NoDelArt, " and artwork", ""))
				If x = MsgBoxResult.No OrElse x = MsgBoxResult.Cancel Then Exit Sub
			End If
			If Not NoDelArt Then TvDeleteShowArt(show)
			Dim showpath As String = Show.FolderPath
			Utilities.SafeDeleteFile(showpath & "tvshow.nfo")
			showpath = showpath.Substring(0, showpath.Length - 1)
			If lb_tvSeriesFolders.items.Contains(showpath) Then lb_tvSeriesFolders.Items.Remove(showpath)
			If Pref.tvFolders.Contains(showpath) AndAlso Not SeriesChange Then Pref.tvFolders.Remove(showpath)
			TvTreeview.Nodes.Remove(show.ShowNode)
			Cache.TvCache.Remove(show)
			Tv_CacheSave()
			TvTreeviewRebuild()
			Show.UpdateTreenode()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub TvDelEpNfoAst(Show As TvShow, season As TvSeason, ep As TvEpisode, ByVal Ignore As Boolean, Optional ByVal NoDelArt As Boolean = False)
		Try
			If Not Ignore Then
				Dim msgstring As String = "Warning, This operation will delete all Episode nfo's" & If(Not NoDelArt, " and artwork", "")
				msgstring &= vbCrLf & "!! Note: will not delete missing episodes." & vbCrLf
				msgstring &= vbCrLf & "Are your sure you wish to continue?"
				Dim res As MsgBoxResult = MsgBox(msgstring, MsgBoxStyle.YesNoCancel, "Delete episode nfo(s)" & If(Not NoDelArt, " and artwork", ""))
				If res = MsgBoxResult.No OrElse res = MsgBoxResult.Cancel Then Exit Sub
			End If
			Dim TheseEpisodes As New List(Of Media_Companion.TvEpisode)
			If Not IsNothing(ep) Then
				For Each epis In season.Episodes
					If epis.NfoFilePath = ep.NfoFilePath Then TheseEpisodes.Add(epis)
				Next
			Else
				TheseEpisodes.AddRange(Show.Episodes)
			End If
			For Each episode In TheseEpisodes
				If IsNothing(season) OrElse episode.Season.Value = season.SeasonNumber.ToString Then
					If Not episode.IsMissing Then
						If episode.FolderPath <> Show.FolderPath AndAlso File.Exists(episode.FolderPath & "folder.jpg") Then
							Utilities.SafeDeleteFile(episode.FolderPath & "folder.jpg")
						End If
						Dim eppath As String = episode.NfoFilePath
						Utilities.SafeDeleteFile(eppath)
						If Not NoDelArt Then
							Utilities.SafeDeleteFile(eppath.Replace(".nfo", ".tbn"))
							Utilities.SafeDeleteFile(eppath.Replace(".nfo", "-thumb.jpg"))
						End If
					End If
					Cache.TvCache.Remove(episode)
					TvTreeview.Nodes.Remove(episode.EpisodeNode)
					If Not IsNothing(season) Then season.Episodes.Remove(episode)
				End If
			Next
			Dim listofnodes As New List(Of TreeNode)
			For Each n As TreeNode In TvTreeview.Nodes
				listofnodes.Add(n)
				For Each chn As TreeNode in n.nodes
					listofnodes.Add(chn)
				Next
			Next
			For Each n In listofnodes
				If n.FullPath.Contains(Show.Title.Value) AndAlso n.FullPath.ToLower.Contains("season") AndAlso n.GetNodeCount(False) = 0 Then
					TvTreeview.Nodes.Remove(n)
				End If
			Next
			Tv_CacheSave()
			TvTreeviewRebuild()
			Show.UpdateTreenode()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Tv_TreeViewContext_FindMissArt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tv_TreeViewContext_FindMissArt.Click
		Try
			tv_MissingArtDownload(tv_ShowSelectedCurrently(TvTreeview))
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub MovieContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MovieContextMenu.Opening
		Try
			If DataGridViewMovies.SelectedRows.Count = 0 Then
				e.Cancel = True
			End If
			tsmiRescrapeRenameFiles         .Enabled = Not Pref.usefoldernames AndAlso Not Pref.basicsavemode And Pref.MovieRenameEnable
			tsmiMov_OpenInMkvmergeGUI       .Enabled = (Pref.MkvMergeGuiPath <> "")
			tsmiRescrapeFrodo_Poster_Thumbs .Enabled = Pref.FrodoEnabled
			tsmiRescrapeFrodo_Fanart_Thumbs .Enabled = Pref.FrodoEnabled
            tsmiMov_DeleteMovieFolder       .Enabled = Pref.EnableMovDeleteFolderTsmi
            tsmiMov_DeleteMovieFolder       .Visible = Pref.EnableMovDeleteFolderTsmi
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub TVContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TVContextMenu.Opening
		Try
			If (TvTreeview.SelectedNode Is Nothing) Then e.Cancel = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub SaveSelectedFanartAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveSelectedFanartAsToolStripMenuItem.Click
		Try
			messbox = New frmMessageBox("Please wait,", "", "Downloading Fanart")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
			Me.Refresh()
			Application.DoEvents()

			Dim tempstring As String
			Dim tempint As Integer = 0
			Dim tempstring2 As String = String.Empty
			Dim allok As Boolean = False
			For Each button As Control In Me.Panel2.Controls
				If button.Name.IndexOf("checkbox") <> -1 Then
					Dim b1 As RadioButton = CType(button, RadioButton)
					If b1.Checked = True Then
						tempstring = b1.Name
						tempstring = tempstring.Replace("moviefanartcheckbox", "")
						tempint = Convert.ToDecimal(tempstring)
						tempstring2 = fanartArray(tempint).hdUrl
						allok = True
						Exit For
					End If
				End If
			Next
			If allok = False Then
				MsgBox("No Fanart Is Selected")
			Else
				Try
					Panel1.Controls.Remove(Label1)
					With SaveFileDialog1
						.AddExtension = True
						.DefaultExt = "jpg"
						.Filter = "Jpg Pictures (*.jpg)|*.jpg"
						.Title = "Save Hi-Res Fanart as"
						.OverwritePrompt = True
						.CheckPathExists = True
						.InitialDirectory = workingMovieDetails.fileinfo.path
					End With
					If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
						Movie.SaveFanartImageToCacheAndPath(tempstring2, SaveFileDialog1.FileName)
					End If
				Catch ex As WebException
					MsgBox(ex.Message)
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		Finally
			messbox.Close()
		End Try
	End Sub

	Private Sub TasksDontShowCompleted_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles TasksDontShowCompleted.CheckedChanged
		Me.TasksOnlyIncompleteTasks = TasksDontShowCompleted.Checked
	End Sub

	Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
		util_FixSeasonEpisode()
	End Sub

	Private Sub mov_DisplayFanart()
		If workingMovieDetails Is Nothing Then Exit Sub

		If workingMovieDetails.fileinfo.fanartpath <> Nothing Then
			Try
				Dim fanartpath = mov_FanartORExtrathumbPath()
				If File.Exists(fanartpath) Then
					util_ImageLoad(PictureBox2, fanartpath, Utilities.DefaultFanartPath)
					lblMovFanartWidth.Text = PictureBox2.Image.Width
					lblMovFanartHeight.Text = PictureBox2.Image.Height
				Else
					util_ImageLoad(PictureBox2, Utilities.DefaultFanartPath, Utilities.DefaultFanartPath)
					lblMovFanartWidth.Text = ""
					lblMovFanartHeight.Text = ""
				End If
			Catch ex As Exception
#If SilentErrorScream Then
                                Throw ex
#End If
			End Try
		End If
	End Sub

	Private Function mov_FanartORExtrathumbPath() As String
		Dim fanarttype As String = ""
		For Each rb As RadioButton In GroupBoxFanartExtrathumbs.Controls
			If rb.Checked Then
				fanarttype = rb.Text.ToLower
				Exit For
			End If
		Next
		Dim workingfanartpath As String = workingMovieDetails.fileinfo.fanartpath
		If fanarttype = "fanart" Then
			Return workingfanartpath
		Else
			Return Strings.Left(workingfanartpath, workingfanartpath.LastIndexOf("\")) & "\extrathumbs\" & fanarttype & ".jpg"
		End If
	End Function

	Private Sub tpMovMain_Enter(sender As Object, e As System.EventArgs) Handles tpMovMain.Enter
		mov_SplitContainerAutoPosition()
	End Sub

	Private Sub tpTvMainBrowser_Enter(sender As Object, e As System.EventArgs) Handles tpTvMainBrowser.Enter
		tv_SplitContainerAutoPosition()
	End Sub

	Private Sub plottxt_DoubleClick(sender As System.Object, e As System.EventArgs) Handles plottxt.DoubleClick
		ShowBigMovieText()
	End Sub

	Private Sub plottxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles plottxt.KeyPress
		If e.KeyChar = Convert.ToChar(1) Then
			DirectCast(sender, TextBox).SelectAll()
			e.Handled = True
		End If
	End Sub

	Private Sub outlinetxt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles outlinetxt.KeyPress
		If e.KeyChar = Convert.ToChar(1) Then
			DirectCast(sender, TextBox).SelectAll()
			e.Handled = True
		End If
	End Sub

	Private Sub ShowBigMovieText()

		Dim frm As New frmBigMovieText
		If Pref.MultiMonitoEnabled Then
			frm.Bounds = screen.AllScreens(CurrentScreen).Bounds
			frm.StartPosition = FormStartPosition.Manual
		End If
		frm.ShowDialog(
							 titletxt.Text,
							 directortxt.Text,
							 votestxt.Text,
							 ratingtxt.Text,
							 runtimetxt.Text,
							 genretxt.Text,
							 txtStars.Text,
							 certtxt.Text,
							 plottxt.Text
							 )
	End Sub
    
	Private Sub mov_RescrapeAllSelected()
		_rescrapeList.Field = Nothing
		_rescrapeList.FullPathAndFilenames.Clear()
		For Each row In DataGridViewMovies.SelectedRows
			Dim fullpath As String = row.Cells("fullpathandfilename").Value.ToString
			If Not File.Exists(fullpath) Then Continue For
			_rescrapeList.FullPathAndFilenames.Add(fullpath)
		Next
		RunBackgroundMovieScrape("RescrapeAll")
	End Sub

	Public Sub mov_VideoSourcePopulate()
		Try
			cbMovieDisplay_Source.Items.Clear()
			cbMovieDisplay_Source.Items.Add("")
			For Each mset In Pref.releaseformat
				cbMovieDisplay_Source.Items.Add(mset)
			Next
			cbFilterSource.UpdateItems(Pref.releaseformat.ToList)
			cbMovieDisplay_Source.SelectedIndex = 0
			If IsNothing(workingMovieDetails) = False Then
				If workingMovieDetails.fullmoviebody.source <> "" Then
					For te = 0 To cbMovieDisplay_Source.Items.Count - 1
						If cbMovieDisplay_Source.Items(te) = workingMovieDetails.fullmoviebody.source Then
							cbMovieDisplay_Source.SelectedIndex = te
							Exit For
						End If
					Next
				End If
			End If
		Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
		End Try
	End Sub
    
	Public Sub OpenUrl(ByVal url As String)
        Dim aok As Boolean = True
		Try
			If Pref.selectedBrowser <> "" Then
                Try
				    Process.Start(Pref.selectedBrowser, Uri.EscapeUriString(url))
                Catch
                    aok = False
                End Try
			End If
            If Pref.selectedBrowser = "" Or Not aok Then
				Try
					Process.Start(url)
				Catch ex As Exception
					MessageBox.Show("An error occurred while trying to launch the default browser - Under 'General Preferences' check 'Use external Browser...' and then locate your browser to fix this error", "", MessageBoxButtons.OK)
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ZoomActorPictureBox(pictureBox As PictureBox)
		If IsNothing(pictureBox.Tag) orElse pictureBox.Tag.ToString = Utilities.DefaultActorPath Then Exit Sub
		Me.ControlBox = False
		MenuStrip1.Enabled = False
		Try
			util_ZoomImage(pictureBox.Tag.ToString)
		Catch
			Dim wc As New WebClient()
			Dim ImageInBytes() As Byte = wc.DownloadData(pictureBox.Tag)
			Dim ImageStream As New IO.MemoryStream(ImageInBytes)
			Dim cachefile As String = Utilities.Download2Cache(pictureBox.Tag.ToString)
			util_ZoomImage(cachefile)
		End Try
	End Sub

	Private Sub Mov_RemoveMovie()
		For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
			oMovies.RemoveMovieFromCache(row.Cells("fullpathandfilename").Value.ToString)
			DataGridViewMovies.Rows.RemoveAt(row.Index)
		Next
		DataGridViewMovies.ClearSelection
		oMovies.SaveMovieCache
		UpdateFilteredList
	End Sub

	Private Sub Mov_DeleteMovieFolder()

		If Not Pref.usefoldernames Then Exit Sub

		Dim lastRow As Integer

		For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
			lastRow = row.Index
		Next

		dim selectRow = nothing

		Try
			 selectRow = DataGridViewMovies.Rows(lastRow+1)
		Catch ex As Exception
		End Try


		For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows

            'Pre delete movie folder validation:
            '
            '  Not root folder
            '  Folder size at least 10MB
            '  Media  size at least 10MB
            '  Media size at least 63% of the folder size 
            '

            If Pref.GetRootFolderCheck(row.Cells("fullpathandfilename").Value.ToString) Then Continue For

            Dim FolderSize    As Long = Convert.ToInt64(row.Cells("FolderSize"   ).Value)
            If FolderSize = -1 Then FolderSize = Utilities.GetFolderSize(row.Cells("NfoPath").Value)
            Dim MediaFileSize As Long = Convert.ToInt64(row.Cells("MediaFileSize").Value)

            If FolderSize   <MIN_MEDIA_SIZE Then Continue For 
            If MediaFileSize<MIN_MEDIA_SIZE Then Continue For 

            Dim relativeSize = (MediaFileSize + 0.0) / FolderSize

            If (relativeSize < RELATIVE_SIZE_THRESHOLD) Then Continue For 

            Dim aok As Boolean = oMovies.Mov_DeleteMovieFolder(row.Cells("fullpathandfilename").Value.ToString)

            If aok Then DataGridViewMovies.Rows.RemoveAt(row.Index)
		Next

		DataGridViewMovies.ClearSelection

		Try
			selectRow.Selected = true
			DisplayMovie()
		Catch ex As Exception
		End Try

		'oMovies.SaveMovieCache
		'UpdateFilteredList
	End Sub
	
	Private Sub Mov_DeleteNfoArtwork(Optional ByVal DelArtwork As Boolean = True)
		Dim msgstr As String = " Are you sure you wish to delete" & vbCrLf
		msgstr &= ".nfo" & If(DelArtwork, ", Fanart, Poster And Actors", " only") & " for" & vbCrLf
		msgstr &= "Selected Movie(s)?"
		If MsgBox(msgstr, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
			Dim movielist As New List(Of String)
			Pref.MovieDeleteNfoArtwork = True
			For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows

				Dim fullpathandfilename = CType(row.DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString

				movielist.Add(fullpathandfilename)
				oMovies.DeleteScrapedFiles(fullpathandfilename, DelArtwork)
			Next

			'Last remove from dataGridViewMovies and update cache.
			Mov_RemoveMovie()
			Pref.MovieDeleteNfoArtwork = False
		Else
			MsgBox(" Deletion of .nfo, artwork And Actors " & vbCrLf & "has been Cancelled")
		End If
	End Sub

	Private Sub TabLevel1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles TabLevel1.SelectedIndexChanged

		Select Case TabLevel1.SelectedTab.Text.ToLower
			Case "config.xml"   : RichTextBoxTabConfigXML   .Text = Utilities.LoadFullText(workingProfile.config)
			Case "moviecache"   : RichTextBoxTabMovieCache  .Text = Utilities.LoadFullText(workingProfile.moviecache)
			Case "tvcache"      : RichTextBoxTabTVCache     .Text = Utilities.LoadFullText(workingProfile.tvcache)
			Case "actorcache"   : RichTextBoxTabActorCache  .Text = Utilities.LoadFullText(workingProfile.actorcache)
			Case "profile"      : RichTextBoxTabProfile     .Text = Utilities.LoadFullText(applicationPath & "\Settings\profile.xml")
			Case "regex"        : RichTextBoxTabRegex       .Text = Utilities.LoadFullText(workingProfile.regexlist)
			Case "export"       : frm_ExportTabSetup()
            Case "music videos" : UcMusicVideo1.mv_FiltersAndSortApply(True)
            Case "movies"
				If Not MoviesFiltersResizeCalled Then
					MoviesFiltersResizeCalled = True
					Pref.movie_filters.SetMovieFiltersVisibility
					UpdateMovieFiltersPanel
				End If
		End Select
	End Sub

	Sub HandleMovieList_DisplayChange(DisplayField As String)
		Mc.clsGridViewMovie.GridFieldToDisplay1 = DisplayField

		If rbTitleAndYear   .Checked Then Pref.moviedefaultlist = 0
		If rbFileName       .Checked Then Pref.moviedefaultlist = 1
		If rbFolder         .Checked Then Pref.moviedefaultlist = 2

		Mc.clsGridViewMovie.GridviewMovieDesign(Me)
		If MainFormLoadedStatus Then
			DisplayMovie()
		End If
	End Sub

	Private Sub TimerToolTip_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerToolTip.Tick
		TimerToolTip.Enabled = False
		TooltipGridViewMovies1.Visible = Pref.ShowMovieGridToolTip
	End Sub


#Region "Movie scraping stuff"

	Sub RunBackgroundMovieScrape(action As String)
		If Not BckWrkScnMovies.IsBusy Then
			scraperLog = ""
			tsStatusLabel.Text = ""
			tsMultiMovieProgressBar.Value = tsMultiMovieProgressBar.Minimum
			tsMultiMovieProgressBar.Visible = Get_MultiMovieProgressBar_Visiblity(action)
			tsStatusLabel.Visible = True
			tsLabelEscCancel.Visible = True
			Statusstrip_Enable()
			ssFileDownload.Visible = False
			tsProgressBarFileDownload_Resize()
			EnableDisableByTag("M", False)       'Disable all UI options that can't be run while scraper is running   
			ScraperErrorDetected = False

			BckWrkScnMovies.RunWorkerAsync(action)
			While BckWrkScnMovies.IsBusy
				Application.DoEvents()
			End While
			If Not Pref.MusicVidScrape Then oMovies.SaveCaches
		Else
			MsgBox("The " & If(Pref.MusicVidScrape, "MusicVideo", "Movie") & " Scraper Is Already Running")
		End If
	End Sub

	Private cbBtnLink_Checked As Boolean

	Sub EnableDisableByTag(tagQualifier As String, _state As Boolean)

		If Not _state Then
			StateBefore = ProgState
			cbBtnLink_Checked = cbBtnLink.Checked
			ProgState = ProgramState.MovieControlsDisabled
		Else
			cbBtnLink.Checked = cbBtnLink_Checked
			ProgState = StateBefore
		End If
		If IsNothing(CtrlsToDisableMovieScrape) Then
			CtrlsToDisableMovieScrape = (From c As Control In GetAllMatchingControls("M")).ToList
		End If
		For Each c In CtrlsToDisableMovieScrape
			c.Enabled = _state
		Next

		'Not picked up for some unknown reason...
		MoviesToolStripMenuItem.Enabled = _state
	End Sub

	Function GetAllMatchingControls(tagQualifier As String) As List(Of Control)
		Dim allControls As New List(Of Control)
		GetAllMatchingControls(tagQualifier, Me, allControls)
		Return allControls
	End Function

	Sub GetAllMatchingControls(tagQualifier As String, parent As Control, allControls As List(Of Control))
		Dim query = From c As Control In parent.Controls
		For Each c As Control In query
			Try
				If Not IsNothing(c) AndAlso Not IsNothing(c.Tag) AndAlso TypeName(c.Tag).ToLower = "string" AndAlso c.Tag = tagQualifier Then allControls.Add(c)
			Catch ex As Exception
			End Try
			GetAllMatchingControls(tagQualifier, c, allControls)
		Next
	End Sub

#Region "MC Scraper Calls"
	Function Get_MultiMovieProgressBar_Visiblity(action As String)

		Select Case action
			Case "BatchRescrape"            : Return _rescrapeList.FullPathAndFilenames.Count > 1               ' filteredList.Count > 1
			Case "ChangeMovie"              : Return False
			Case "RescrapeAll"              : Return _rescrapeList.FullPathAndFilenames.Count > 1
			Case "RescrapeDisplayedMovie"   : Return False
			Case "RescrapeSpecific"         : Return _rescrapeList.FullPathAndFilenames.Count > 1
			Case "LockSpecific"             : Return _lockList.FullPathAndFilenames.Count > 1
			Case "ScrapeDroppedFiles"       : Return droppedItems.Count > 1
			Case "SearchForNewMovies"       : Return True
			Case "SearchForNewMusicVideo"   : Return True
			Case "RefreshMVCache"           : Return True
            Case "HomeVidScrape"            : Return True
            Case "HomeVidRebuild"           : Return True
			Case "ChangeMusicVideo"         : Return True
			Case "RebuildCaches"            : Return True
		End Select

		MsgBox("Unrecognised scrape action : [" + action + "]!", MsgBoxStyle.Exclamation, "Programming Error!")
		Return False
	End Function

	Private Sub BckWrkScnMovies_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BckWrkScnMovies.DoWork
		Try
			CallSubByName(DirectCast(e.Argument, String))
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Public Sub CallSubByName(SubName As String)
		Me.GetType.GetMethod(SubName).Invoke(Me, Nothing)
	End Sub

	Public Sub BatchRescrape()
		oMovies.BatchRescrapeSpecific(_rescrapeList.FullPathAndFilenames, rescrapeList)    'filteredList
	End Sub

	Public Sub ChangeMovie
		oMovies.ChangeMovie(workingMovieDetails.fileinfo.fullpathandfilename, ChangeMovieId, MovieSearchEngine)
	End Sub

	Public Sub RescrapeAll
		oMovies.RescrapeAll(_rescrapeList.FullPathAndFilenames)
	End Sub

	Public Sub RebuildCaches
		oMovies.RebuildCaches
	End Sub

	Public Sub RescrapeDisplayedMovie
		oMovies.RescrapeMovie(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fullmoviebody.tmdbid)
		oMovies.SaveCaches
	End Sub

	Public Sub RescrapeSpecific
		oMovies.RescrapeSpecific(_rescrapeList)
	End Sub
    
	Public Sub ScrapeDroppedFiles
		oMovies.ScrapeFiles(droppedItems)
	End Sub

	Public Sub SearchForNewMovies
		Pref.googlecount = 0
		oMovies.FindNewMovies
	End Sub

	Public Sub SearchForNewMusicVideo
		oMovies.FindNewMusicVideos()
	End Sub

	Public Sub RefreshMVCache
		oMovies.RebuildMVCache()
	End Sub

	Public Sub ChangeMusicVideo
		oMovies.ChangeMovie(ucMusicVideo.changeMVList(0), ucMusicVideo.changeMVList(3), ucMusicVideo.changeMVList(1))
	End Sub

    Public Sub HomeVidScrape()
        oMovies.FindNewHomeVideos()
    End Sub

    Public Sub HomeVidRebuild()
        oMovies.RebuildHomeVideoCache()
    End Sub

	Private Sub BckWrkScnMovies_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BckWrkScnMovies.ProgressChanged

		Dim oProgress As Progress = CType(e.UserState, Progress)
		If e.ProgressPercentage <> -1 Then tsMultiMovieProgressBar.Value = e.ProgressPercentage

		If oProgress.Command = Progress.Commands.Append Then
			Dim msgtxt As String = tsStatusLabel.Text & oProgress.Message
			If msgtxt.Length > 144 AndAlso oProgress.Message <> "-OK" Then
				msgtxt = tsStatusLabel.Text.Substring(0, (tsStatusLabel.Text.ToLower.IndexOf("actors-ok") + 9)) ' & oProgress.Message
				msgtxt &= oProgress.Message
			End If
			tsStatusLabel.Text = msgtxt '&= oProgress.Message
		Else
			tsStatusLabel.Text = oProgress.Message
		End If

		If oProgress.Message = Movie.MSG_ERROR Then ScraperErrorDetected = True

		If Not IsNothing(oProgress.Log) Then scraperLog += oProgress.Log
	End Sub

	Private Sub BckWrkScnMovies_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BckWrkScnMovies.RunWorkerCompleted

		If scrapeAndQuit = True Then
			sandq = sandq - 2
			Exit Sub
		End If
        If Not Pref.MusicVidScrape Then
            lastNfo = ""   'Force currently displayed movie details to be re-displayed 
            UpdateFilteredList()
        End If
        tsStatusLabel.Visible = False
		tsMultiMovieProgressBar.Visible = False
		tsLabelEscCancel.Visible = False
		Statusstrip_Enable(False)
		ssFileDownload.Visible = False
		EnableDisableByTag("M", True)       'Re-enable disabled UI options that couldn't be run while scraper was running
		Pref.MovieChangeMovie = False
		GC.Collect()
		Dim Displayed As Boolean = DisplayLogFile()
		Pref.MusicVidScrape = False  '  Reset to false only after scrapers complete
		Pref.MusicVidConcertScrape = False
		If Not Displayed Then BlinkTaskBar()
	End Sub

#End Region
    
	Sub FileDownload_SizeObtained(ByVal iFileSize As Long) Handles oMovies.FileDownloadSizeObtained
		Callback_ShowHideFileDownloadProgressBar(True, iFileSize)
	End Sub

	Private Sub FileDownload_AmountDownloadedChanged(ByVal iTotalBytesRead As Long) Handles oMovies.AmountDownloadedChanged
		Me.Invoke(CType(Sub() Safe_FileDownload_AmountDownloadedChanged(iTotalBytesRead), MethodInvoker))
	End Sub

	Private Sub FileDownload_FileDownloadComplete() Handles oMovies.FileDownloadComplete
		Callback_ShowHideFileDownloadProgressBar(False, -1)
	End Sub

	Private Sub FileDownload_FileDownloadFailed() Handles oMovies.FileDownloadFailed
		Callback_ShowHideFileDownloadProgressBar(False, -1)
	End Sub


	'Initiate callback from main UI thread
	Sub Callback_ShowHideFileDownloadProgressBar(ByVal bool As Boolean, iFileSize As Long)

		Me.Invoke(CType(Sub() ShowHideFileDownloadProgressBar(bool, iFileSize), MethodInvoker))
	End Sub

	Sub ShowHideFileDownloadProgressBar(ByVal bool As Boolean, iFileSize As Long)
		ssFileDownload.Visible = bool
		If bool Then tsProgressBarFileDownload.Maximum = iFileSize
	End Sub

	Sub Safe_FileDownload_AmountDownloadedChanged(ByVal iTotalBytesRead As Long)
		tsProgressBarFileDownload.Value = iTotalBytesRead
	End Sub

	Private Sub tsLabelEscCancel_Click(sender As System.Object, e As System.EventArgs)
		BckWrkScnMovies_Cancel()
	End Sub

	Sub bckgrndcancel
		If ImgBw.IsBusy Then ImgBw.CancelAsync()
		Dim CurrentTab As String = TabLevel1.SelectedTab.Name
		If CurrentTab = TabPage1.Name Then BckWrkScnMovies_Cancel
		If CurrentTab = TabPage2.Name Then BckWrkTv_Cancel
	End Sub

	Sub BckWrkScnMovies_Cancel
		If BckWrkScnMovies.IsBusy Then
			tsStatusLabel.Text = "* Cancelling... *"
			BckWrkScnMovies.CancelAsync()
		End If
		If ImgBw.IsBusy Then
			tsStatusLabel.Text = "* Cancelling... *"
			ImgBw.CancelAsync()
		End If
	End Sub
    
    Sub BckWrkTv_Cancel
        If BckWrkTv.IsBusy OrElse BckWrkSeries.IsBusy OrElse bckgroundscanepisodes.IsBusy Then
            tsStatusLabel.Text = "* Cancelling... *"
            BckWrkTv.CancelAsync()
            BckWrkSeries.CancelAsync()
		    bckgroundscanepisodes.CancelAsync()
        End If
        
    End Sub

	Sub AbortFileDownload
		tsStatusLabel.Text = "* Aborting trailer download... *"
		Monitor.Enter(countLock)
		blnAbortFileDownload = True
		Monitor.Exit(countLock)
	End Sub

	Sub doResizeRefresh
        If ProgState = ProgramState.Closing Then Exit Sub
		Try
			Dim CurrentTab As String = TabLevel1.SelectedTab.Name
			If CurrentTab = TabPage1.Name Then   'Movies
				mov_DisplayFanart()
				util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
				Dim video_flags = VidMediaFlags(workingMovieDetails.filedetails, workingMovieDetails.fullmoviebody.title.ToLower.Contains("3d"), workingMovie.DisplayFolderSize)
				movieGraphicInfo.OverlayInfo(PbMovieFanArt, ratingtxt.Text, video_flags)
			ElseIf CurrentTab = TabPage2.Name Then  'TV Shows
				TvTreeview_AfterSelect_Do()
			End If
			Dim MovMaxWallCount2 As Integer = Math.Floor((tpMovWall.Width - 50) / 150) 'Convert.ToInt32((TabPage22.Width - 100) / 150)
			Dim tvMaxWallCount2 As Integer = Math.Floor((tpTvWall.Width - 50) / 150)
			If MovMaxWallCount2 <> MovMaxWallCount Then
				MovMaxWallCount = MovMaxWallCount2
				Call mov_WallReset()
			End If
			If tvMaxWallCount2 <> tvMaxWallCount Then
				tvMaxWallCount = tvMaxWallCount2
				Call tv_WallReset()
			End If
		Catch
		End Try
	End Sub

	Sub doRefresh
		Dim CurrentTab As String = TabLevel1.SelectedTab.Name
		If CurrentTab = TabPage1.Name Then  'Movies
			Dim SubTab As String = TabControl2.SelectedTab.Name
			If SubTab = tpMovMain.Name Then mov_RebuildMovieCaches()
			If SubTab = tpMovFanart.name Then
                ImgBw.CancelAsync()
                Do Until Not ImgBw.IsBusy
                    Application.DoEvents()
                Loop
				Panel2.Controls.Clear()
                MovFanartDisplay(If(btnMovFanartToggle.Text = "Show Movie Fanart", workingMovieDetails.fullmoviebody.TmdbSetId, ""))
			End If
		End If
		If CurrentTab = TabPage2.Name Then  'TV Shows
			Dim SubTab As String = TabControl3.SelectedTab.Name
			If SubTab = tpTvMainBrowser.Name Then tv_CacheRefresh()
			If SubTab = tpTvFanart.Name Then
                ImgBw.CancelAsync()
                Do Until Not ImgBw.IsBusy
                    Application.DoEvents()
                Loop
			    tv_Fanart_Load()
			End If
		End If
        If CurrentTab = TabPage3.Name AndAlso TabControl1.SelectedTab.Name = tp_HmMainBrowser.Name Then btn_HMRefresh.PerformClick
		If CurrentTab = TabMV.Name Then ucMusicVideo1.btnRefresh.PerformClick()
	End Sub

	Sub doSearchNew
		Dim CurrentTab As String = TabLevel1.SelectedTab.Name
		If CurrentTab = TabPage1.name AndAlso TabControl2.SelectedTab.Name = tpMovMain.Name Then SearchForNew()
		If CurrentTab = TabPage2.Name AndAlso TabControl3.SelectedTab.Name = tpTvMainBrowser.Name Then ep_Search()
        If CurrentTab = TabPage3.Name AndAlso TabControl1.SelectedTab.Name = tp_HmMainBrowser.Name Then btn_HMSearch.PerformClick
		If CurrentTab = TabMV.Name Then ucMusicVideo1.btnSearchNew.PerformClick()
	End Sub

	Private Sub ssFileDownload_Resize(sender As System.Object, e As System.EventArgs) Handles ssFileDownload.Resize
		tsProgressBarFileDownload_Resize()
	End Sub

	Private Sub tsProgressBarFileDownload_Resize()
		tsProgressBarFileDownload.Width = ssFileDownload.Width - tsFileDownloadlabel.Width - 8
	End Sub

#End Region 'Movie scraping stuff

	Sub SearchForNew
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf SearchForNew))
        Else
            RunBackgroundMovieScrape("SearchForNewMovies")
        End If
		
	End Sub

	Sub Do_ScrapeAndQuit
		If refreshAndQuit Then
			mov_RebuildMovieCaches()
			tv_CacheRefresh()
			While BckWrkScnMovies.IsBusy
				Thread.Sleep(100)
				Application.DoEvents
			End While
		End If

		Do While sandq <> 0
			If sandq >= 2 Then
				SearchForNew
				Do Until sandq < 2
					Thread.Sleep(100)
					Application.DoEvents()
				Loop
			End If
			If sandq = 1 Then
				ep_Search()
				Do Until sandq < 1
					Thread.Sleep(100)
					Application.DoEvents()
				Loop
			End If
		Loop
	End Sub

	Sub DoScrapeDroppedFiles()
		RunBackgroundMovieScrape("ScrapeDroppedFiles")
	End Sub

	Private Function DisplayLogFile()
		Dim Displayed As Boolean = False
		If ScraperErrorDetected And Pref.ShowLogOnError Then
			scraperLog = "******************************************************************************" & vbCrLf &
							 "* One Or more errors were detected during scraping. See below for details.   *" & vbCrLf &
							 "* To disable seeing this, turn off General Preferences -'Show log on error'. *" & vbCrLf &
							 "******************************************************************************" & vbCrLf & vbCrLf & scraperLog
		End If

		If ((Not MovAutoScrapeTimerTripped AndAlso Not TvAutoScrapeTimerTripped AndAlso Not Pref.disablelogfiles) Or (Pref.MusicVidScrape AndAlso Pref.MVPrefShowLog) Or (ScraperErrorDetected And Pref.ShowLogOnError)) And scraperLog <> "" Then
			Displayed = True
			Dim MyFormObject As New frmoutputlog(scraperLog, True)
			Try
				MyFormObject.ShowDialog()
			Catch ex As Exception
			End Try
		End If

		ScraperErrorDetected = False
		Return Displayed
	End Function

	Sub pop_cbMovieDisplay_MovieSet
		Dim previouslySelected = cbMovieDisplay_MovieSet.SelectedItem
		cbMovieDisplay_MovieSet.Sorted = True
		cbMovieDisplay_MovieSet.Items.Clear
        
		cbMovieDisplay_MovieSet.Items.AddRange(oMovies.UsedMovieSets)

		cbMovieDisplay_MovieSet.Sorted = False
		If cbMovieDisplay_MovieSet.Items.Count = 0 Then cbMovieDisplay_MovieSet.Items.Add("-None-")
		If cbMovieDisplay_MovieSet.Items(0) <> "-None-" Then cbMovieDisplay_MovieSet.Items.Insert(0, "-None-")

		cbMovieDisplay_MovieSet.SelectedIndex = 0

		If IsNothing(workingMovieDetails) Then Exit Sub
		If previouslySelected = Nothing Then
			If workingMovieDetails.fullmoviebody.SetName <> Nothing Then
				If workingMovieDetails.fullmoviebody.SetName.IndexOf(" / ") = -1 Then
					cbMovieDisplay_MovieSet.SelectedItem = workingMovieDetails.fullmoviebody.SetName
				End If
			End If
		Else
			cbMovieDisplay_MovieSet.SelectedItem = previouslySelected
		End If
	End Sub


	Private Sub tsmiMov_PlayMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_PlayMovie.Click
		Try
			mov_Play("Movie")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub


	Private Sub tsmiMov_PlayTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_PlayTrailer.Click
		Try
			mov_Play("Trailer")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmiMov_OpenFolder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiMov_OpenFolder.Click
		Try
			If Not workingMovieDetails.fileinfo.fullpathandfilename Is Nothing Then
				Call util_OpenFolder(workingMovieDetails.fileinfo.fullpathandfilename)
			Else
				MsgBox("There is no Movie selected to open")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmiMov_ViewNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_ViewNfo.Click
		Try
			Utilities.NfoNotepadDisplay(workingMovieDetails.fileinfo.fullpathandfilename, Pref.altnfoeditor)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub tsmiMov_DeleteNfoArtwork_Click(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles tsmiMov_DeleteNfoArtwork.MouseDown
		Dim DelArt As Boolean = (e.Button <> MouseButtons.Right)
		MovieContextMenu.Close()
		Mov_DeleteNfoArtwork(DelArt)
	End Sub


	Private Sub tsmiMov_DeleteMovieFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_DeleteMovieFolder.Click
		If Pref.EnableMovDeleteFolderTsmi Then Mov_DeleteMovieFolder()
	End Sub

	
	Private Sub tsmiMov_ReloadFromCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_ReloadFromCache.Click
		Try
			Call mov_FormPopulate()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmiMov_RemoveMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_RemoveMovie.Click
		Mov_RemoveMovie()
	End Sub

	Private Sub tsmiMov_RescrapeAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_RescrapeAll.Click
		mov_RescrapeAllSelected()
	End Sub

	Private Sub tsmiMov_FanartBrowserAlt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_FanartBrowserAlt.Click
		Try
			Dim t As New frmMovieAltFanart
			If Pref.MultiMonitoEnabled Then
				Dim w As Integer = t.Width
				Dim h As Integer = t.Height
				t.Bounds = screen.AllScreens(CurrentScreen).Bounds
				t.StartPosition = FormStartPosition.Manual
				t.Width = w
				t.Height = h
			End If
			t.ShowDialog()
			Try
				If File.Exists(workingMovieDetails.fileinfo.fanartpath) Then
					util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
				Else
					PbMovieFanArt.Image = Nothing
				End If

			Catch ex As Exception
#If SilentErrorScream Then
                Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmiMov_PosterBrowserAlt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_PosterBrowserAlt.Click
		Try
			Dim t As New frmMovieAltPosterArt
			If Pref.MultiMonitoEnabled Then
				Dim w As Integer = t.Width
				Dim h As Integer = t.Height
				t.Bounds = screen.AllScreens(CurrentScreen).Bounds
				t.StartPosition = FormStartPosition.Manual
				t.Width = w
				t.Height = h
			End If
			t.ShowDialog()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmiMov_EditMovieAlt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_EditMovieAlt.Click
		Try
			Call mov_Edit()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

#Region "ToolStripmenu Movie Rescrape Specific"

	Private Sub tsmiRescrapeTrailer_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeTrailer.Click
		Call mov_ScrapeSpecific("trailer")
	End Sub                 'trailer
	Private Sub tsmiRescrapeRenameFiles_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeRenameFiles.Click
		mov_ScrapeSpecific("rename_files")
	End Sub              'rename files
	Private Sub tsmiRescrapeTitle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeTitle.Click
		Call mov_ScrapeSpecific("title")
	End Sub        'title
	Private Sub tsmiRescrapeplotClick(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeplot.Click
		Call mov_ScrapeSpecific("plot")
	End Sub        'plot
	Private Sub tsmiRescrapeTagline_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeTagline.Click
		Call mov_ScrapeSpecific("tagline")
	End Sub        'tagline
	Private Sub tsmiRescrapeDirector_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeDirector.Click
		Call mov_ScrapeSpecific("director")
	End Sub        'director
	Private Sub tsmiRescrapeCredits_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeCredits.Click
		Call mov_ScrapeSpecific("credits")
	End Sub        'Credits
	Private Sub tsmiRescrapeCert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeCert.Click
		Call mov_ScrapeSpecific("mpaa")
	End Sub        'mpaa
	Private Sub tsmiRescrapeGenre_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeGenre.Click
		Call mov_ScrapeSpecific("genre")
	End Sub        'genre
	Private Sub tsmiRescrapeOutline_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeOutline.Click
		Call mov_ScrapeSpecific("outline")
	End Sub      'outline
	Private Sub tsmiRescrapeRuntimeImdb_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeRuntimeImdb.Click
		Call mov_ScrapeSpecific("runtime")
	End Sub      'runtime
	Private Sub tsmiRescrapeRuntimeFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeRuntimeFile.Click
		Call mov_ScrapeSpecific("runtime_file")
	End Sub      'runtime file
	Private Sub tsmiRescrapeStudio_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeStudio.Click
		Call mov_ScrapeSpecific("studio")
	End Sub      'studio
	Private Sub tsmiRescrapeActors_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeActors.Click
		Call mov_ScrapeSpecific("actors")
	End Sub      'actors
	Private Sub tsmiRescrapeFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRescrapeFanart.Click
		Call mov_ScrapeSpecific("missingfanart")
	End Sub      'missingfanart
	Private Sub tsmiRescrapePoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRescrapePoster.Click
		Call mov_ScrapeSpecific("missingposters")
	End Sub      'missingposters
	Private Sub tsmiRescrapeMediaTags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRescrapeMediaTags.Click
		Call mov_ScrapeSpecific("mediatags")
	End Sub      'mediatags
	Private Sub tsmiRescrapeRating_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRescrapeRating.Click
		Call mov_ScrapeSpecific("rating")
	End Sub      'rating
	Private Sub tsmiRescrapeVotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRescrapeVotes.Click
		Call mov_ScrapeSpecific("votes")
	End Sub      'votes
	Private Sub tsmiRescrapeStars_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiRescrapeStars.Click
		Call mov_ScrapeSpecific("stars")
	End Sub      'stars
	Private Sub tsmiRescrapeYear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeYear.Click
		Call mov_ScrapeSpecific("year")
	End Sub  'year
	Private Sub tsmiRescrapeKeyWords_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiRescrapeKeyWords.Click
		Call mov_ScrapeSpecific("TagsFromKeywords")
	End Sub    'TagsFromKeywords
	Private Sub tsmiRescrapeTMDbSetName_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeTMDbSetName.Click
		Call mov_ScrapeSpecific("tmdb_set_name")
	End Sub                         'Tmdb set info
	Private Sub tsmiMov_SetWatched_Click(sender As Object, e As EventArgs) Handles tsmiMov_SetWatched.Click
		Call mov_ScrapeSpecific("SetWatched")
	End Sub                           'set watched
	Private Sub tsmiMov_ClearWatched_Click(sender As Object, e As EventArgs) Handles tsmiMov_ClearWatched.Click
		Call mov_ScrapeSpecific("ClearWatched")
	End Sub                       'clear watched
	Private Sub tsmiDlTrailer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiDlTrailer.Click
		Call mov_ScrapeSpecific("Download_Trailer")
	End Sub                  'Download Trailer
	Private Sub tsmiRescrapeCountry_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeCountry.Click
		mov_ScrapeSpecific("country")
	End Sub                               'country
	Private Sub tsmiRescrapeTop250_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeTop250.Click
		mov_ScrapeSpecific("top250")
	End Sub                                 'top250
	Private Sub tsmiRescrapeMetascore_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeMetascore.Click
		mov_ScrapeSpecific("metascore")
	End Sub                                 'metascore
	Private Sub tsmiRescrapePremiered_Click(sender As Object, e As EventArgs) Handles tsmiRescrapePremiered.Click
		mov_ScrapeSpecific("Premiered")
	End Sub                           'premiered
	Private Sub tsmiRescrapePosterUrls_Click(sender As Object, e As EventArgs) Handles tsmiRescrapePosterUrls.Click
		mov_ScrapeSpecific("PosterUrls")
	End Sub                         'poster urls
	Private Sub tsmiRescrapeFrodo_Poster_Thumbs_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeFrodo_Poster_Thumbs.Click
		mov_ScrapeSpecific("Frodo_Poster_Thumbs")
	End Sub       'Frodo poster thumbs
	Private Sub tsmiRescrapeFrodo_Fanart_Thumbs_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeFrodo_Fanart_Thumbs.Click
		mov_ScrapeSpecific("Frodo_Fanart_Thumbs")
	End Sub       'Frodo fanart thumbs
	Private Sub tsmiRescrapeFanartTv_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeFanartTv.Click
		mov_ScrapeSpecific("ArtFromFanartTv")
	End Sub       'Frodo fanart thumbs
	Private Sub tsmiRescrapeMovieSetArt_Click(sender As Object, e As EventArgs) Handles tsmiRescrapeMovieSetArt.Click
		mov_ScrapeSpecific("missingmovsetart")
	End Sub       'MovieSet Artwork
#End Region  'ToolStripmenu Movie Rescrape Specific
    
	Private Sub mov_PreferencesDisplay()
		AuthorizeCheck = True
		clbx_MovieRoots.Items.Clear()
		For Each item In movieFolders
			clbx_MovieRoots.Items.Add(item.rpath, item.selected)
		Next
		AuthorizeCheck = False
		lbx_MovOfflineFolders.Items.Clear()
		For Each item In Pref.offlinefolders
			lbx_MovOfflineFolders.Items.Add(item)
		Next
		moviefolderschanged = False
	End Sub

	Private Sub tv_FoldersSetup()
		clbx_TvRootFolders.Items.Clear()
		lb_tvSeriesFolders.Items.Clear()
		AuthorizeCheck = True
		For Each folder In tvRootFolders
			clbx_TvRootFolders.Items.Add(folder.rpath, folder.selected)
		Next
		AuthorizeCheck = False
		For Each folder In tvFolders
			lb_tvSeriesFolders.Items.Add(folder)
		Next
	End Sub


#Region "Movie Browser Tab"

	Private Sub btnMovSearchNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovSearchNew.Click
		SearchForNew()
	End Sub

	Private Sub btnMovRefreshAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovRefreshAll.Click
		mov_RebuildMovieCaches()
	End Sub

	Private Sub ToolStripMenuItemRebuildMovieCaches_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemRebuildMovieCaches.Click
		mov_RebuildMovieCaches()
	End Sub

	Private Sub cbBtnLink_Click(sender As Object, e As EventArgs) Handles cbBtnLink.Click
		SetcbBtnLink()
		LabelCountFilter.Focus()
	End Sub

	Private Sub rbFileName_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFileName.CheckedChanged
		HandleMovieList_DisplayChange("FileName")
		If Not cbSort.Text = "A - Z" Then
			cbSort.Text = "A - Z"
		Else
			cbsort_SelectedIndexChanged(cbsort, EventArgs.Empty)
		End If
	End Sub

	Private Sub rbTitleAndYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTitleAndYear.CheckedChanged
		HandleMovieList_DisplayChange("TitleAndYear")
		cbsort_SelectedIndexChanged(cbsort, EventArgs.Empty)
	End Sub

	Private Sub rbFolder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFolder.CheckedChanged
		HandleMovieList_DisplayChange("Folder")
		If Not cbSort.Text = "A - Z" Then
			cbSort.Text = "A - Z"
		Else
			cbsort_SelectedIndexChanged(cbsort, EventArgs.Empty)
		End If
	End Sub

	Private Sub cbSort_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbSort.SelectedIndexChanged
		Mc.clsGridViewMovie.GridFieldToDisplay2 = cbSort.Text
		Call Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		DisplayMovie()
		If Not MainFormLoadedStatus Then Exit Sub
		Pref.moviesortorder = cbSort.SelectedIndex
	End Sub

	Private Sub btnreverse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnreverse.CheckedChanged
		If btnreverse.Checked Then
			Mc.clsGridViewMovie.GridSort = "Desc"
		Else
			Mc.clsGridViewMovie.GridSort = "Asc"
		End If
		Pref.movieinvertorder = btnreverse.Checked
		Call Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		DisplayMovie()
	End Sub

	Private Sub btnResetFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetFilters.Click
		resetallfilters()
	End Sub

	Private Sub txt_titlesearch_KeyDown(ByVal sender As Object, ByVal e As Object) Handles txt_titlesearch.KeyDown, txt_titlesearch.ModifiedChanged
		Try
			If filterOverride = False Then
				_yield = True
				Application.DoEvents()
				_yield = False
				If txt_titlesearch.Text.Length > 0 Then
					txt_titlesearch.BackColor = Color.DarkOrange
				Else
					txt_titlesearch.BackColor = Color.White
				End If
				txt_titlesearch.Refresh()
				Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
				If _yield Then Return
				DisplayMovie(True)
				Cursor.Current = Cursors.Default
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovSave.Click
		Try
			Call mov_SaveQuick()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovRescrape.Click
		mov_Rescrape()
	End Sub

	Private Sub btnMovieDisplay_ActorFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieDisplay_ActorFilter.Click
		Try
			ProgState = ProgramState.ResettingFilters
			oMovies.ActorsFilter_AddIfMissing(cbMovieDisplay_Actor.Text)
			ShowMovieFilter(cbFilterActor)
			cbFilterActor.UpdateItems(oMovies.ActorsFilter)
			cbFilterActor.SelectItem(cbMovieDisplay_Actor.Text)
			ProgState = ProgramState.Other
			Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovieDisplay_DirectorFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieDisplay_DirectorFilter.Click
		Try
			ProgState = ProgramState.ResettingFilters
			oMovies.DirectorsFilter_AddIfMissing(directortxt.Text)
			ShowMovieFilter(cbFilterDirector)
			cbFilterDirector.UpdateItems(oMovies.DirectorsFilter)
			cbFilterDirector.SelectItem(directortxt.Text)
			ProgState = ProgramState.Other
			Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovieDisplay_CountriesFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieDisplay_CountriesFilter.Click
		Try
			ProgState = ProgramState.ResettingFilters
			ShowMovieFilter(cbFilterCountries)
			cbFilterCountries.SelectItem(countrytxt.Text.Split(" / ")(0))
			ProgState = ProgramState.Other
			Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub titletxt_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles titletxt.Enter
		Try
			Try
				If titletxt.Text.IndexOf(workingMovieDetails.fullmoviebody.year) <> -1 Then
					Dim tempstring2 As String = " (" & workingMovieDetails.fullmoviebody.year & ")"
					Dim tempstring As String = titletxt.Text.Replace(tempstring2, "")
					tempstring = tempstring.TrimEnd(" ")
					currenttitle = tempstring
					titletxt.Items.Clear()
					For Each item In workingMovieDetails.alternativetitles
						If item <> tempstring Then titletxt.Items.Add(item)
					Next
					titletxt.SelectedIndex = -1
					titletxt.Text = tempstring
				End If
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub titletxt_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles titletxt.Leave
		Try
			Try
				If titletxt.Text.IndexOf(workingMovieDetails.fullmoviebody.year) = -1 Then
					Dim tempstring As String = titletxt.Text
					titletxt.Items.Clear()
					For Each item In workingMovieDetails.alternativetitles
						If item <> currenttitle Then titletxt.Items.Add(item)
					Next
					titletxt.Text = tempstring & " (" & workingMovieDetails.fullmoviebody.year & ")"
				End If
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub titletxt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles titletxt.SelectedIndexChanged
		Try
			Try
				If titletxt.Text.IndexOf(workingMovieDetails.fullmoviebody.year) = -1 Then
					Dim tempstring As String = titletxt.Text
					titletxt.Items.Clear()
					titletxt.Items.Add(tempstring & " (" & workingMovieDetails.fullmoviebody.year & ")")
					For Each item In workingMovieDetails.alternativetitles
						If item <> tempstring Then titletxt.Items.Add(item)
					Next
					titletxt.SelectedIndex = 0
				End If
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PbMovieFanArt_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PbMovieFanArt.MouseUp, PbMovieFanArt.Click
		Try
			If e.Button = Windows.Forms.MouseButtons.Right Then
				RescrapePToolStripMenuItem.Visible = False
				RescrapePosterFromTMDBToolStripMenuItem.Visible = False
				RescraToolStripMenuItem.Visible = False
				PeToolStripMenuItem.Visible = False
				DownloadPosterToolStripMenuItem.Visible = False
				DownloadPosterFromTMDBToolStripMenuItem.Visible = False
				DownloadPosterFromMPDBToolStripMenuItem.Visible = False
				DownloadPosterFromIMDBToolStripMenuItem.Visible = False
				RescrapeFanartToolStripMenuItem.Visible = False
				DownloadFanartToolStripMenuItem.Visible = False
				Try
					If File.Exists(Pref.GetFanartPath(workingMovieDetails.fileinfo.fullpathandfilename)) Or (workingMovieDetails.fileinfo.videotspath <> "" And File.Exists(workingMovieDetails.fileinfo.videotspath + "fanart.jpg")) Then
						RescrapeFanartToolStripMenuItem.Visible = True
					Else
						DownloadFanartToolStripMenuItem.Visible = True
					End If
				Catch
					RescrapePToolStripMenuItem.Visible = False
					RescrapeFanartToolStripMenuItem.Visible = False
					DownloadFanartToolStripMenuItem.Visible = False
					DownloadPosterToolStripMenuItem.Visible = False

				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PbMovieFanArt_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PbMovieFanArt.DoubleClick
		Try
			If workingMovieDetails.fileinfo.fanartpath <> Nothing Then
				If File.Exists(workingMovieDetails.fileinfo.fanartpath) Then
					Me.ControlBox = False
					MenuStrip1.Enabled = False
					util_ZoomImage(workingMovieDetails.fileinfo.fanartpath)
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PbMoviePoster_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PbMoviePoster.DoubleClick
		Try
			If workingMovieDetails.fileinfo.posterpath <> Nothing Then
				If File.Exists(workingMovieDetails.fileinfo.posterpath) Then
					Me.ControlBox = False
					MenuStrip1.Enabled = False
					util_ZoomImage(workingMovieDetails.fileinfo.posterpath)
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PbMoviePoster_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PbMoviePoster.MouseUp
		Try
			If e.Button = Windows.Forms.MouseButtons.Right Then
				RescrapePToolStripMenuItem.Visible = False
				RescrapePosterFromTMDBToolStripMenuItem.Visible = False
				RescraToolStripMenuItem.Visible = False
				PeToolStripMenuItem.Visible = False
				DownloadPosterToolStripMenuItem.Visible = False
				DownloadPosterFromTMDBToolStripMenuItem.Visible = False
				DownloadPosterFromMPDBToolStripMenuItem.Visible = False
				DownloadPosterFromIMDBToolStripMenuItem.Visible = False
				RescrapeFanartToolStripMenuItem.Visible = False
				DownloadFanartToolStripMenuItem.Visible = False
				Try
					If File.Exists(Pref.GetPosterPath(workingMovieDetails.fileinfo.fullpathandfilename)) Then
						RescrapePToolStripMenuItem.Visible = True
						RescrapePosterFromTMDBToolStripMenuItem.Visible = True
						RescraToolStripMenuItem.Visible = True
						PeToolStripMenuItem.Visible = True
					Else
						DownloadPosterToolStripMenuItem.Visible = True
						DownloadPosterFromTMDBToolStripMenuItem.Visible = True
						DownloadPosterFromMPDBToolStripMenuItem.Visible = True
						DownloadPosterFromIMDBToolStripMenuItem.Visible = True
					End If
				Catch
					RescrapePToolStripMenuItem.Visible = False
					RescrapePosterFromTMDBToolStripMenuItem.Visible = False
					RescraToolStripMenuItem.Visible = False
					PeToolStripMenuItem.Visible = False
					DownloadPosterToolStripMenuItem.Visible = False
					DownloadPosterFromTMDBToolStripMenuItem.Visible = False
					DownloadPosterFromMPDBToolStripMenuItem.Visible = False
					DownloadPosterFromIMDBToolStripMenuItem.Visible = False
					RescrapeFanartToolStripMenuItem.Visible = False
					DownloadFanartToolStripMenuItem.Visible = False
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub cbMovieDisplay_Actor_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbMovieDisplay_Actor.MouseEnter
		Try
			cbMovieDisplay_Actor.Focus()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub cbMovieDisplay_Actor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMovieDisplay_Actor.SelectedIndexChanged
		Try
			For Each actor In workingMovieDetails.listactors
				If actor.actorname = cbMovieDisplay_Actor.SelectedItem Then
					If actor.actorrole <> "" Then roletxt.Text = actor.actorrole
					Dim temppath = GetActorPath(workingMovieDetails.fileinfo.fullpathandfilename, actor.actorname, actor.actorid)
					If Not String.IsNullOrEmpty(temppath) AndAlso File.Exists(temppath) Then
						util_ImageLoad(PictureBoxActor, temppath, Utilities.DefaultActorPath)
						Exit Sub
					End If
					If actor.actorthumb <> Nothing Then
						Dim actorthumbpath As String = Pref.GetActorThumbPath(actor.actorthumb)
						If actorthumbpath <> "none" Then
							If Not Pref.LocalActorImage AndAlso actorthumbpath.IndexOf("http") = 0 Then
								If actorthumbpath.ToLower.IndexOf("http") <> -1 OrElse File.Exists(actorthumbpath) Then
									util_ImageLoad(PictureBoxActor, actorthumbpath, Utilities.DefaultActorPath)
								End If
							Else
								util_ImageLoad(PictureBoxActor, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
							End If
						Else
							util_ImageLoad(PictureBoxActor, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
						End If
					Else
						util_ImageLoad(PictureBoxActor, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
					End If
					Exit For
				Else
					util_ImageLoad(PictureBoxActor, Utilities.DefaultActorPath, Utilities.DefaultActorPath)
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovSelectPlot_Click(sender As Object, e As EventArgs) Handles btnMovSelectPlot.Click
		Dim newplot As String = GetlistofPlots()
		If Not IsNothing(newplot) Then
			plottxt.Text = newplot
			Call mov_SaveQuick()
		End If
	End Sub

	Private Sub lbl_movRuntime_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbl_movRuntime.MouseDown
		If e.Button = Windows.Forms.MouseButtons.Left Then
			displayRuntimeScraper = Not (runtimetxt.Enabled = True)
			Call mov_SwitchRuntime()
		End If
	End Sub

	Private Sub lbl_movTop250_Click(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbl_movTop250.MouseDown
		If e.Button = Windows.Forms.MouseButtons.Left Then
			Call mov_Switchtop250
		End If
	End Sub

	Private Sub btnMovieDisplay_SetFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieDisplay_SetFilter.Click
		Try
			ProgState = ProgramState.ResettingFilters
			oMovies.SetsFilter_AddIfMissing(cbMovieDisplay_MovieSet.Text)
			ShowMovieFilter(cbFilterSet)
			cbFilterSet.UpdateItems(oMovies.SetsFilter)
			cbFilterSet.SelectItem(cbMovieDisplay_MovieSet.Text)
			ProgState = ProgramState.Other
			Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ButtonTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonTrailer.Click
		If Not File.Exists(workingMovieDetails.fileinfo.trailerpath) Then
			_rescrapeList.Field = "Download_Trailer"
			_rescrapeList.FullPathAndFilenames.Clear()
			_rescrapeList.FullPathAndFilenames.Add(workingMovieDetails.fileinfo.fullpathandfilename)
			RunBackgroundMovieScrape("RescrapeSpecific")
		Else
			Statusstrip_Enable()
			ToolStripStatusLabel2.Text = ""
			ToolStripStatusLabel2.Visible = True
			Dim tempstring = applicationPath & "\Settings\temp.m3u"
			ToolStripStatusLabel2.Text = "Playing Movie...Creating m3u file:..." & tempstring
			Dim fi = File.CreateText(tempstring)
			fi.WriteLine(workingMovieDetails.fileinfo.trailerpath)
			fi.Close()
			ToolStripStatusLabel2.Text &= "............Launching Player."
			StartVideo(tempstring)
			statusstripclear.Start()
		End If
	End Sub

	Private Sub btnPlayMovie_Click(sender As System.Object, e As System.EventArgs) Handles btnPlayMovie.Click
		mov_Play("Movie")
	End Sub

	Private Sub btnMovWatched_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovWatched.Click
		Try
			If DataGridViewMovies.SelectedRows.Count = 1 Then
				If btnMovWatched.Text = "&Watched" Then
					btnMovWatched.Text = "Un&watched"
					btnMovWatched.BackColor = Color.Red
					btnMovWatched.Refresh()
					workingMovieDetails.fullmoviebody.playcount = "0"
				Else
					btnMovWatched.Text = "&Watched"
					btnMovWatched.BackColor = Color.LawnGreen
					btnMovWatched.Refresh()
					workingMovieDetails.fullmoviebody.playcount = "1"
				End If
				Call mov_SaveQuick()
			ElseIf DataGridViewMovies.SelectedRows.Count > 1 Then
				messbox = New frmMessageBox("Saving Selected Movies", , "     Please Wait.     ")
				messbox.Show()
				messbox.Refresh()
				Dim watched As String = ""
				If btnMovWatched.Text = "&Watched" Then
					btnMovWatched.Text = "Un&watched"
					btnMovWatched.BackColor = Color.Red
					btnMovWatched.Refresh()
					watched = "0"
				ElseIf btnMovWatched.Text = "Un&watched" Then
					btnMovWatched.Text = "&Watched"
					btnMovWatched.BackColor = Color.LawnGreen
					btnMovWatched.Refresh()
					watched = "1"
				ElseIf btnMovWatched.Text = "" Then
					btnMovWatched.Text = "&Watched"
					btnMovWatched.BackColor = Color.LawnGreen
					btnMovWatched.Refresh()
					watched = "1"
				End If
				For Each sRow As DataGridViewRow In DataGridViewMovies.SelectedRows
					Dim filepath As String = sRow.Cells("fullpathandfilename").Value.ToString
					If (File.Exists(filepath)) Then
						Dim fmd As New FullMovieDetails
						fmd = WorkingWithNfoFiles.mov_NfoLoadFull(filepath)
						If IsNothing(fmd) Then Continue For
						fmd.fullmoviebody.playcount = watched
						Movie.SaveNFO(filepath, fmd)
						For f = 0 To oMovies.MovieCache.Count - 1
							If oMovies.MovieCache(f).fullpathandfilename = filepath Then
								Dim newfullmovie As New ComboList
								newfullmovie = oMovies.MovieCache(f)
								newfullmovie.playcount = watched
								oMovies.MovieCache.RemoveAt(f)
								oMovies.MovieCache.Add(newfullmovie)
								Exit For
							End If
						Next
					End If
				Next
				messbox.Close()
			End If
			Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PictureBoxActor_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxActor.DoubleClick
		ZoomActorPictureBox(PictureBoxActor)
	End Sub

	Private Sub rcmenuOption(x, y)
		' x is equal to the menu item title that was clicked
		' Create case stament for that to call the correct xmlinteraction passing in y
		Select Case x
			Case "Premiered"
				mov_ScrapeSpecific("Premiered")
			Case "Cert"
				Call mov_ScrapeSpecific("mpaa")
			Case "Plot"
				Call mov_ScrapeSpecific("plot")
			Case "Outline"
				Call mov_ScrapeSpecific("outline")
			Case "Tagline"
				Call mov_ScrapeSpecific("tagline")
			Case "Genre"
				Call mov_ScrapeSpecific("genre")
			Case "Rating"
				Call mov_ScrapeSpecific("rating")
			Case "Runtime"
				Call mov_ScrapeSpecific("runtime")
			Case "Set"
				Call mov_ScrapeSpecific("tmdb_set_name")
			Case "Actors"
				Call mov_ScrapeSpecific("actors")
			Case "Stars"
				Call mov_ScrapeSpecific("stars")
			Case "Director"
				Call mov_ScrapeSpecific("director")
			Case "Credits", "Writers"
				Call mov_ScrapeSpecific("credits")
			Case "Studio"
				Call mov_ScrapeSpecific("studio")
			Case "Country"
				Call mov_ScrapeSpecific("country")
			Case "Tag(s)"
				Call mov_ScrapeSpecific("TagsFromKeywords")
			Case "Top 250"
				Call mov_ScrapeSpecific("top250")
			Case "Metascore"
				Call mov_ScrapeSpecific("metascore")
			Case "Votes"
				Call mov_ScrapeSpecific("votes")
		End Select
	End Sub

	Private Sub rcmenu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles rcmenu.ItemClicked
		Dim contextMenu = DirectCast(sender, ContextMenuStrip)
		Dim label = DirectCast(contextMenu.SourceControl, Label)
		Dim var As String = label.Text
		Dim toolStripItem = e.ClickedItem
		Dim var2 As String = toolStripItem.Text
		rcmenu.Close()
		rcmenuOption(var, var2)
	End Sub

	Private Sub rcmenuClicked(ByVal sender As System.Object, ByVal e As CancelEventArgs) Handles rcmenu.Opening
		' Get the Label clicked from the SourceControl property of the clicked ContextMenuStrip.
		rcmenu.Items.Clear()
		Dim contextMenu = DirectCast(sender, ContextMenuStrip)
		Dim label = DirectCast(contextMenu.SourceControl, Label)
		Dim var2 As String = label.Text
		contextMenu.Items.Add("Rescrape " & var2)
	End Sub

#End Region

#Region "Movie Wall"

	Public Shared Sub updateposterwall(ByVal path As String, ByVal movie As String)
		For Each poster As PictureBox In Form1.tpMovWall.Controls
			If poster.Tag = movie Then
				util_ImageLoad(poster, path, Utilities.DefaultPosterPath)
				poster.Tag = movie
			End If
		Next
	End Sub

	Private Sub mov_WallReset()
		For i = tpMovWall.Controls.Count - 1 To 0 Step -1
			tpMovWall.Controls.RemoveAt(i)
		Next
		Dim count As Integer = 0
		Dim locx As Integer = 0
		Dim locy As Integer = 0
		Dim MovMaxWallCount As Integer = Math.Floor((tpMovWall.Width - 40) / WallPicWidth)

		While (DataGridViewMovies.SelectedRows.Count / MovMaxWallCount) > WallPicWidth
			MovMaxWallCount += 1
		End While

		Try
			For Each pic In MovpictureList
				Try
					If count = MovMaxWallCount Then
						count = 0
						locx = 0
						locy += WallPicHeight
					End If

					With pic
						Dim vscrollPos As Integer = tpMovWall.VerticalScroll.Value
						.Location = New Point(locx, locy - vscrollPos)
						.ContextMenuStrip = MovieWallContextMenu
					End With
					locx += WallPicWidth
					count += 1

					Me.tpMovWall.Controls.Add(pic)
					tpMovWall.Refresh()
					Application.DoEvents()
				Catch ex As Exception
					MsgBox(ex.ToString)
				End Try
			Next
		Catch ex As Exception
		Finally
		End Try
	End Sub

	Private Sub mov_WallSetup()
		Dim check As Boolean = True
		Dim count As Integer = 0
		Dim locx As Integer = 0
		Dim locy As Integer = 0

		If lastSort = "" Then lastSort = Mc.clsGridViewMovie.GridFieldToDisplay2
		If lastinvert = "" Then lastinvert = Mc.clsGridViewMovie.GridSort
		If lastSort <> Mc.clsGridViewMovie.GridFieldToDisplay2 OrElse lastinvert <> Mc.clsGridViewMovie.GridSort Then
			lastSort = Mc.clsGridViewMovie.GridFieldToDisplay2
			lastinvert = Mc.clsGridViewMovie.GridSort
			moviecount_bak = 0
		End If

		If moviecount_bak = DataGridViewMovies.RowCount Then Exit Sub

		MovMaxWallCount = Math.Floor((tpMovWall.Width - 40) / WallPicWidth)

		While (DataGridViewMovies.SelectedRows.Count / MovMaxWallCount) > WallPicWidth
			MovMaxWallCount += 1
		End While

		MovpictureList.Clear()
		For i = tpMovWall.Controls.Count - 1 To 0 Step -1
			If tpMovWall.Controls(i).Name = "" Then tpMovWall.Controls.RemoveAt(i)
		Next
		tpMovWall.Refresh()
		Application.DoEvents()

		For Each row As DataGridViewRow In DataGridViewMovies.Rows
			moviecount_bak += 1
			Dim m As Data_GridViewMovie = row.DataBoundItem
			bigPictureBox = New PictureBox()
			With bigPictureBox
				.Width = WallPicWidth
				.Height = WallPicHeight
				.SizeMode = PictureBoxSizeMode.StretchImage
				Dim filename As String = Utilities.GetCRC32(m.fullpathandfilename)
				Dim posterCache As String = Utilities.PosterCachePath & filename & ".jpg"
				If Not File.Exists(posterCache) And File.Exists(Pref.GetPosterPath(m.fullpathandfilename)) Then
					Try
                        Dim ms As IO.MemoryStream = New IO.MemoryStream()
                        Using r As IO.Filestream = File.Open(Pref.GetPosterPath(m.fullpathandfilename), IO.FileMode.Open)
                            r.CopyTo(ms)
                        End Using
						Dim bitmap2 As New Bitmap(ms)
						bitmap2 = Utilities.ResizeImage(bitmap2, WallPicWidth, WallPicHeight)
						Utilities.SaveImage(bitmap2, Path.Combine(posterCache))
                        ms.Dispose()
						bitmap2.Dispose()
					Catch
						'Invalid file
						File.Delete(Pref.GetPosterPath(m.fullpathandfilename))
					End Try
				End If
				If File.Exists(posterCache) Then
					Try
						.Image = Utilities.LoadImage(posterCache)
					Catch
						'Invalid file
						File.Delete(Pref.GetPosterPath(m.fullpathandfilename))
					End Try
				Else
					.Image = Utilities.LoadImage(Utilities.DefaultPosterPath)
				End If

				.Tag = m.fullpathandfilename
				Dim toolTip1 As ToolTip = New ToolTip(Me.components)
				toolTip1.AutoPopDelay = 12000
				Dim outline As String = m.outline
				Dim newoutline As List(Of String) = util_TextWrap(outline, 50)
				outline = ""
				For Each line In newoutline
					outline = outline & vbCrLf & line
				Next
				outline.TrimEnd(vbCrLf)
				toolTip1.SetToolTip(bigPictureBox, m.fullpathandfilename & vbCrLf & vbCrLf & m.DisplayTitleAndYear & vbCrLf & outline)
				toolTip1.Active = True
				toolTip1.InitialDelay = 0

				.Visible = True
				.BorderStyle = BorderStyle.None
				.WaitOnLoad = True
				.ContextMenuStrip = MovieWallContextMenu
				AddHandler bigPictureBox.MouseEnter, AddressOf util_MouseEnter
				AddHandler bigPictureBox.DoubleClick, AddressOf mov_WallClicked
				If count = MovMaxWallCount Then
					count = 0
					locx = 0
					locy += WallPicHeight
				End If
				Dim vscrollPos As Integer = tpMovWall.VerticalScroll.Value
				If mouseDelta <> 0 Then
					vscrollPos = vscrollPos - mouseDelta
					mouseDelta = 0
				End If
				.Location = New Point(locx, locy - vscrollPos)
				locx += WallPicWidth
				count += 1
			End With
			Me.tpMovWall.Controls.Add(bigPictureBox)
			MovpictureList.Add(bigPictureBox)
			Me.tpMovWall.Refresh()
			Application.DoEvents()
		Next
	End Sub

	Private Sub mov_WallClicked(ByVal sender As Object, ByVal e As EventArgs)
		Dim item As Windows.Forms.PictureBox = sender
		Dim tempstring As String = item.Tag
		For f = 0 To DataGridViewMovies.RowCount - 1
			If DataGridViewMovies.Rows(f).Cells("fullpathandfilename").Value.ToString = tempstring Then
				DataGridViewMovies.ClearSelection()
				DataGridViewMovies.Rows(f).Selected = True
				Application.DoEvents()
				currentTabIndex = 0
				Me.TabControl2.SelectedIndex = 0
				Exit For
			End If
		Next
	End Sub

	Private Function util_TextWrap(ByVal text As String, ByVal linelength As Integer)
		Dim ReturnValue As New List(Of String)
		text = Trim(text)
		Dim Words As String() = text.Split(" ")
		If Words.Length = 1 And Words(0).Length > linelength Then
			Dim lines As Integer = (Int(text.Length / linelength) + 1)
			text = text.PadRight(lines * linelength)
			For i = 0 To lines - 1
				Dim SliceStart As Integer = i * linelength
				ReturnValue.Add(text.Substring(SliceStart, linelength))
			Next
		Else
			Dim CurrentLine As New System.Text.StringBuilder
			For Each Word As String In Words
				If CurrentLine.Length + Word.Length < linelength Then
					CurrentLine.Append(Word & " ")
				Else
					If Word.Length > linelength Then
						Dim Slice As String = Word.Substring(0, linelength - CurrentLine.Length)
						CurrentLine.Append(Slice)
						ReturnValue.Add(CurrentLine.ToString)
						CurrentLine = New System.Text.StringBuilder()
						Word = Word.Substring(Slice.Length, Word.Length - Slice.Length)
						Dim RemainingSlices As Integer = Int(Word.Length / linelength) + 1
						For LineNumber = 1 To RemainingSlices
							If LineNumber = RemainingSlices Then
								CurrentLine.Append(Word & " ")
							Else
								Slice = Word.Substring(0, linelength)
								CurrentLine.Append(Slice)
								ReturnValue.Add(CurrentLine.ToString)
								CurrentLine = New System.Text.StringBuilder()
								Word = Word.Substring(Slice.Length, Word.Length - Slice.Length)
							End If
						Next
					Else
						ReturnValue.Add(CurrentLine.ToString)
						CurrentLine = New System.Text.StringBuilder(Word & " ")
					End If
				End If
			Next
			If CurrentLine.Length > 0 Then ReturnValue.Add(CurrentLine.ToString)
		End If
		Return ReturnValue
	End Function

	Private Shadows Sub util_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Try
			ClickedControl = sender.tag
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub MovieWallContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MovieWallContextMenu.Opening
		Dim tempstring As String = ClickedControl
		If tempstring <> Nothing Then
			Dim trailerpath As String = Pref.ActualTrailerPath(tempstring) 'GetTrailerPath(tempstring)
			If File.Exists(trailerpath) Then
				tsmiWallPlayTrailer.Enabled = True
			Else
				tsmiWallPlayTrailer.Enabled = False
			End If
		End If
	End Sub

	Private Sub PlayMovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayMovieToolStripMenuItem.Click
		Try
			Dim tempstring As String = ClickedControl
			If tempstring = Nothing Then
				Exit Sub
			End If
			If tempstring = "" Then
				Exit Sub
			End If
			tempstring = Utilities.GetFileName(tempstring)
			Dim playlist As New List(Of String)
			playlist = Utilities.GetMediaList(tempstring)
			If playlist.Count <= 0 Then
				MsgBox("No Media File Found For This nfo")
				Exit Sub
			End If
			LaunchPlayList(playlist)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub EditMovieToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditMovieToolStripMenuItem1.Click
		Try
			Dim tempstring As String = ClickedControl
			For f = 0 To DataGridViewMovies.RowCount - 1
				If DataGridViewMovies.Rows(f).Cells("fullpathandfilename").Value.ToString = tempstring Then
					DataGridViewMovies.ClearSelection()
					DataGridViewMovies.Rows(f).Selected = True
					DisplayMovie()
					Application.DoEvents()
					currentTabIndex = 4
					Me.TabControl2.SelectedIndex = 4
					Exit For
				End If
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub DToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DToolStripMenuItem.Click
		Try
			Dim tempstring As String = ClickedControl
			If tempstring <> Nothing Then
				Try
					Dim Temp2 As String = Pref.GetPosterPath(tempstring)
					If File.Exists(Temp2) Then
						Me.ControlBox = False
						MenuStrip1.Enabled = False
						util_ZoomImage(Temp2)
					Else
						MsgBox("Cant find file:-" & vbCrLf & Pref.GetPosterPath(tempstring))
					End If
				Catch ex As Exception
#If SilentErrorScream Then
                Throw ex
#End If
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub OpenFolderToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFolderToolStripMenuItem1.Click
		Try
			Dim tempstring As String = ClickedControl
			If tempstring <> Nothing Then
				Try
					Call util_OpenFolder(tempstring)
				Catch ex As Exception
#If SilentErrorScream Then
                Throw ex
#End If
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsmiWallPlayTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiWallPlayTrailer.Click
		Try
			Dim tempstring As String = ClickedControl
			If tempstring <> Nothing Then
				Dim trailerpath As String = Pref.ActualTrailerPath(tempstring) 'GetTrailerPath(tempstring)
				statusstrip_Enable()
				ToolStripStatusLabel2.Text = ""
				ToolStripStatusLabel2.Visible = True
				If File.Exists(trailerpath) Then
					statusstrip_Enable()
					ToolStripStatusLabel2.Visible = True
					Dim trailerstring = applicationPath & "\Settings\temp.m3u"
					ToolStripStatusLabel2.Text = "Playing Movie...Creating m3u file:..." & trailerstring
					Dim fi = File.CreateText(trailerstring)
					fi.WriteLine(trailerpath)
					fi.Close()
					ToolStripStatusLabel2.Text &= "............Launching Player."
					StartVideo(trailerstring)
				Else
					ToolStripStatusLabel2.Text = "No downloaded trailer present"
				End If
			End If
			statusstripclear.Start()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

#End Region 'Movie Wall

#Region "Movie Fanart Tab"

	Private Sub tpMovFanart_Leave(sender As Object, e As EventArgs) Handles tpMovFanart.Leave
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
	End Sub

	Private Sub PictureBox2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox2.DoubleClick
		Try
			If Not PictureBox2.Tag Is Nothing Then
				Me.ControlBox = False
				MenuStrip1.Enabled = False
				Call util_ZoomImage(PictureBox2.Tag.ToString)
			Else
				MsgBox("No Image Available To Zoom")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncroptop_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncroptop.MouseDown
		Try
			If PictureBox2.Image Is Nothing Then Exit Sub
			'thumbedItsMade = True
			btnresetimage.Enabled = True
			btnSaveCropped.Enabled = True
			cropString = "top"
			Timer2.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncropbottom_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropbottom.MouseDown
		Try
			If PictureBox2.Image Is Nothing Then Exit Sub
			'thumbedItsMade = True
			btnresetimage.Enabled = True
			btnSaveCropped.Enabled = True
			Call util_ImageCropTop()
			cropString = "bottom"
			Timer2.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncropleft_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropleft.MouseDown
		Try
			If PictureBox2.Image Is Nothing Then Exit Sub
			'thumbedItsMade = True
			btnresetimage.Enabled = True
			btnSaveCropped.Enabled = True
			Call util_ImageCropTop()
			cropString = "left"
			Timer2.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncropright_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropright.MouseDown
		Try
			If PictureBox2.Image Is Nothing Then Exit Sub
			'thumbedItsMade = True
			btnresetimage.Enabled = True
			btnSaveCropped.Enabled = True
			Call util_ImageCropTop()
			cropString = "right"
			Timer2.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncropbottom_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropbottom.MouseUp
		Try
			Timer2.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncropleft_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropleft.MouseUp
		Timer2.Enabled = False
	End Sub

	Private Sub btncropright_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropright.MouseUp
		Try
			Timer2.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btncroptop_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncroptop.MouseUp
		Try
			Timer2.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer2.Tick
		Try
			If cropString = "top" Then Call util_ImageCropTop()
			If cropString = "bottom" Then Call util_ImageCropBottom()
			If cropString = "left" Then Call util_ImageCropLeft()
			If cropString = "right" Then Call util_ImageCropRight()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnresetimage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnresetimage.Click
		Try
			'thumbedItsMade = False
			util_ImageLoad(PictureBox2, mov_FanartORExtrathumbPath(), Utilities.DefaultFanartPath)
			btnresetimage.Enabled = False
			btnSaveCropped.Enabled = False
			lblMovFanartWidth.Text = PictureBox2.Image.Width
			lblMovFanartHeight.Text = PictureBox2.Image.Height
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnSaveCropped_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveCropped.Click
		Try
			'thumbedItsMade = False
			Try
				'Dim stream As New IO.MemoryStream
				Utilities.SaveImage(PictureBox2.Image, mov_FanartORExtrathumbPath)
				lblMovFanartWidth.Text = PictureBox2.Image.Width
				lblMovFanartHeight.Text = PictureBox2.Image.Height
				If rbMovFanart.Checked Then ' i.e. this is a fanart task rather than an extrathumb task
					PbMovieFanArt.Image = PictureBox2.Image 'if we are saving the main fanart then update the art on the main form view
					For Each paths In Pref.offlinefolders
						If workingMovieDetails.fileinfo.fanartpath.IndexOf(paths) <> -1 Then
							Dim mediapath As String
							mediapath = Utilities.GetFileName(workingMovieDetails.fileinfo.fullpathandfilename)
							Call mov_OfflineDvdProcess(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fullmoviebody.title, mediapath)
						End If
					Next
				End If
				btnresetimage.Enabled = False
				btnSaveCropped.Enabled = False
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_MovFanartScrnSht_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_MovFanartScrnSht.Click
		Dim Timepoint As Integer = Nothing
		If Int16.TryParse(tb_MovFanartScrnShtTime.Text, Timepoint) Then
			Dim cachepathandfilename As String = Utilities.CreateScrnShotToCache(workingMovieDetails.fileinfo.filenameandpath, workingMovieDetails.fileinfo.fanartpath, Timepoint)
			If cachepathandfilename <> "" Then
				File.Copy(cachepathandfilename, workingMovieDetails.fileinfo.fanartpath, True)
				util_ImageLoad(PictureBox2, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
				mov_DisplayFanart()
				util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
				Dim video_flags = VidMediaFlags(workingMovieDetails.filedetails, workingMovieDetails.fullmoviebody.title.ToLower.Contains("3d"), workingMovie.DisplayFolderSize)
				movieGraphicInfo.OverlayInfo(PbMovieFanArt, ratingtxt.Text, video_flags)
			End If
		End If
	End Sub

	Private Sub tb_MovFanartScrnShtTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_MovFanartScrnShtTime.KeyPress
		If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then btn_MovFanartScrnSht.PerformClick() 'TvEpThumbScreenShot(If(IsNumeric(tb_MovFanartScrnShtTime.Text), tb_MovFanartScrnShtTime.Text.ToInt, Pref.ScrShtDelay))
		If Char.IsNumber(e.KeyChar) = False And e.KeyChar <> Chr(8) Then e.Handled = True
	End Sub

	Private Sub tb_MovFanartScrnShtTime_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_MovFanartScrnShtTime.Leave
		If tb_MovFanartScrnShtTime.Text = "" Then
			MsgBox("Please enter a numerical value >0 into the textbox")
			tb_MovFanartScrnShtTime.Focus()
		ElseIf Convert.ToInt32(tb_MovFanartScrnShtTime.Text) = 0 Then
			MsgBox("Please enter a numerical value >0 into the textbox")
			tb_MovFanartScrnShtTime.Focus()
		End If
	End Sub

	Private Sub btnMovFanartUrlorBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovFanartUrlorBrowse.Click
		Try
			Dim t As New frmImageBrowseOrUrl
			t.Location = Me.PointToScreen(New Point(btnMovFanartUrlorBrowse.Left - 460, btnMovFanartUrlorBrowse.Top + 30))
			t.ShowDialog()
			If t.DialogResult = Windows.Forms.DialogResult.Cancel Or t.tb_PathorUrl.Text = "" Then
				t.Dispose()
				Exit Sub
			End If
			Dim PathOrUrl As String = t.tb_PathorUrl.Text
			t.Dispose()
			t = Nothing
			Dim cachename As String = Utilities.Download2Cache(PathOrUrl)
			If cachename <> "" Then
				If Not MovFanartToggle Then
					Dim issavefanart As Boolean = Pref.savefanart
					Dim FanartOrExtraPath As String = mov_FanartORExtrathumbPath
					Dim xtra As Boolean = False
					Dim extrfanart As Boolean = False
					If rbMovThumb1.Checked Or rbMovThumb2.Checked Or rbMovThumb3.Checked Or rbMovThumb4.Checked Or rbMovThumb5.Checked Then xtra = True
					Pref.savefanart = True
					If xtra AndAlso Pref.movxtrathumb Then extrfanart = Movie.SaveFanartImageToCacheAndPath(cachename, FanartOrExtraPath)

					If xtra OrElse Movie.SaveFanartImageToCacheAndPath(cachename, FanartOrExtraPath) Then
						If Not xtra Then
							Dim paths As List(Of String) = Pref.GetfanartPaths(workingMovieDetails.fileinfo.fullpathandfilename, If(workingMovieDetails.fileinfo.videotspath <> "", workingMovieDetails.fileinfo.videotspath, ""))
							Movie.SaveFanartImageToCacheAndPaths(cachename, paths)
						End If
						Pref.savefanart = issavefanart
						mov_DisplayFanart()
						util_ImageLoad(PbMovieFanArt, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
						Dim video_flags = VidMediaFlags(workingMovieDetails.filedetails, workingMovieDetails.fullmoviebody.title.ToLower.Contains("3d"), workingMovie.DisplayFolderSize)
						movieGraphicInfo.OverlayInfo(PbMovieFanArt, ratingtxt.Text, video_flags)

						For Each paths In Pref.offlinefolders
							Dim offlinepath As String = paths & "\"
							If workingMovieDetails.fileinfo.fanartpath.IndexOf(offlinepath) <> -1 Then
								Dim mediapath As String
								mediapath = Utilities.GetFileName(workingMovieDetails.fileinfo.fullpathandfilename)
								messbox.TextBox1.Text = "Creating Offline Movie..."
								Call mov_OfflineDvdProcess(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fullmoviebody.title, mediapath)
							End If
						Next
					Else
						util_ImageLoad(PictureBox2, Utilities.DefaultFanartPath, Utilities.DefaultFanartPath)
						Pref.savefanart = issavefanart
					End If
					lblMovFanartWidth.Text = PictureBox2.Image.Width
					lblMovFanartHeight.Text = PictureBox2.Image.Height
					UpdateMissingFanart()
					XbmcLink_UpdateArtwork
				Else
					Dim MovSetFanartSavePath As String = workingMovieDetails.fileinfo.movsetfanartpath
					If MovSetFanartSavePath <> "" Then
						Movie.SaveFanartImageToCacheAndPath(cachename, MovSetFanartSavePath)
						MovPanel6Update()
						util_ImageLoad(PictureBox2, MovSetFanartSavePath, Utilities.DefaultFanartPath)
					Else
						MsgBox("!!  Problem formulating correct save location for Fanart" & vbCrLf & "                Please check your settings")
					End If
				End If
			End If
			UpdateMissingFanart()
			XbmcLink_UpdateArtwork()
		Catch ex As Exception
			MsgBox("Unable To Download Image")
		End Try
	End Sub

	Private Sub ButtonFanartSaveLoRes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFanartSaveLoRes.Click
		SaveFanart(False)
	End Sub

	Private Sub ButtonFanartSaveHiRes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFanartSaveHiRes.Click
		SaveFanart(True)
	End Sub

	Private Sub btnMovPasteClipboardFanart_Click(sender As Object, e As EventArgs) Handles btnMovPasteClipboardFanart.Click
		If AssignClipboardImage(PictureBox2) Then
			SaveFanart(True, True)
			lblMovFanartWidth.Text = PictureBox2.Image.Width
			lblMovFanartHeight.Text = PictureBox2.Image.Height
		End If
	End Sub

	Private Sub rbMovFanart_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMovFanart.CheckedChanged
		If rbMovFanart.Checked Then mov_DisplayFanart()
	End Sub

	Private Sub rbMovThumb1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMovThumb1.CheckedChanged
		If rbMovThumb1.Checked Then mov_DisplayFanart()
	End Sub

	Private Sub rbMovThumb2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMovThumb2.CheckedChanged
		If rbMovThumb2.Checked Then mov_DisplayFanart()
	End Sub

	Private Sub rbMovThumb3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMovThumb3.CheckedChanged
		If rbMovThumb3.Checked Then mov_DisplayFanart()
	End Sub

	Private Sub rbMovThumb4_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMovThumb4.CheckedChanged
		If rbMovThumb4.Checked Then mov_DisplayFanart()
	End Sub

	Private Sub rbMovThumb5_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbMovThumb5.CheckedChanged
		If rbMovThumb5.Checked Then mov_DisplayFanart()
	End Sub

	Private Sub BtnSearchGoogleFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSearchGoogleFanart.Click
		GoogleSearch("+screenshot")
	End Sub

	Private Sub btnPrevNextMissingFanart_Click(sender As System.Object, e As System.EventArgs) Handles btnPrevMissingFanart.Click, btnNextMissingFanart.Click
		btnPrevMissingFanart.Enabled = False
		btnNextMissingFanart.Enabled = False
		DataGridViewMovies.ClearSelection()
		DataGridViewMovies.Rows(sender.Tag).Selected = True
		DisplayMovie()
		UpdateMissingFanartNav()
		mov_FanartLoad()
	End Sub

	Private Sub btnMovFanartToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovFanartToggle.Click
		If MovFanartToggle Then
			btnMovFanartToggle.Text = "Show MovieSet Fanart"
			btnMovFanartToggle.BackColor = System.Drawing.Color.Lime
			Panel2.Controls.Clear()
			util_ImageLoad(Picturebox2, workingMovieDetails.fileinfo.fanartpath, Utilities.DefaultFanartPath)
            ImgBw.CancelAsync()
            Do Until Not ImgBw.IsBusy
                Application.DoEvents()
            Loop
			MovFanartDisplay()
		Else
			btnMovFanartToggle.Text = "Show Movie Fanart"
			btnMovFanartToggle.BackColor = System.Drawing.Color.Aqua
			Panel2.Controls.Clear()
			util_ImageLoad(Picturebox2, workingMovieDetails.fileinfo.movsetfanartpath, Utilities.DefaultFanartPath)
            ImgBw.CancelAsync()
            Do Until Not ImgBw.IsBusy
                Application.DoEvents()
            Loop
			MovFanartDisplay(workingMovieDetails.fullmoviebody.TmdbSetId)
		End If
		MovFanartToggle = Not MovFanartToggle
	End Sub

#End Region

#Region "Movie Poster Tab"

	Private Sub tpMovPoster_Leave(sender As Object, e As EventArgs) Handles tpMovPoster.Leave
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
	End Sub

	Private Sub PictureBoxAssignedMoviePoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxAssignedMoviePoster.DoubleClick
		Try
			If Not PictureBoxAssignedMoviePoster.Tag Is Nothing Then
				Me.ControlBox = False
				MenuStrip1.Enabled = False
				Call util_ZoomImage(PictureBoxAssignedMoviePoster.Tag.ToString)
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PictureBoxAssignedMoviePoster_DragEnter(ByVal sender As System.Object, ByVal e As DragEventArgs) Handles PictureBoxAssignedMoviePoster.DragEnter
		Try
			If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then
				e.Effect = DragDropEffects.Copy
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PictureBoxAssignedMoviePoster_DragDrop(ByVal sender As System.Object, ByVal e As DragEventArgs) Handles PictureBoxAssignedMoviePoster.DragDrop
		Try
			Dim Pic As String = CType(e.Data.GetData(DataFormats.FileDrop), Array).GetValue(0).ToString  '"FileDrop", False)
			Dim FInfo As FileInfo = New FileInfo(Pic)
			If FInfo.Extension.ToLower() = ".jpg" Or FInfo.Extension.ToLower() = ".tbn" Or FInfo.Extension.ToLower() = ".bmp" Or FInfo.Extension.ToLower() = ".png" Then
				util_ImageLoad(PictureBoxAssignedMoviePoster, Pic, Utilities.DefaultPosterPath)
				lblCurrentLoadedPoster.Text = "Width: " & PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & PictureBoxAssignedMoviePoster.Image.Height.ToString
				btnMoviePosterSaveCroppedImage.Enabled = True
			Else
				MessageBox.Show("Not a picture")
			End If

		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_IMDB_posters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_IMDB_posters.Click
		Try
			If Not workingMovieDetails.fullmoviebody.imdbid.Contains("tt") Then
				MsgBox("No IMDB ID" & vbCrLf & "Searching IMDB for Posters halted")
				Exit Sub
			End If
			messbox = New frmMessageBox("Please wait,", "", "Retrieving Movie Poster List")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
			Call mov_PosterInitialise()
			Dim newobject2 As New imdb_thumbs.Class1
			newobject2.MCProxy = Utilities.MyProxy
			Dim posters(,) As String = newobject2.getimdbposters(workingMovieDetails.fullmoviebody.imdbid)
			For f = 0 To UBound(posters)
				If posters(f, 0) <> Nothing Then
					If posters(f, 1) = Nothing Then posters(f, 1) = posters(f, 0)
					Dim mcPoster As New McImage
					mcPoster.hdUrl = posters(f, 1)
					mcPoster.ldUrl = posters(f, 0)
					posterArray.Add(mcPoster)
					'mcPoster.ldUrl = Nothing
					'mcPoster.hdUrl = Nothing
				End If
			Next
			messbox.Close()
			Call mov_PosterSelectionDisplay()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovPosterNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovPosterNext.Click
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		mov_PosterSelectionDisplayNext()

	End Sub

	Private Sub btnMovPosterPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovPosterPrev.Click
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		mov_PosterSelectionDisplayPrev()

	End Sub

	Private Sub btn_TMDb_posters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TMDb_posters.Click
		Try
			messbox = New frmMessageBox("Please wait,", "", "Retrieving Movie Poster List")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
			Call mov_PosterInitialise()
			Try
				Dim tmdb As New TMDb
				If Not MovPosterToggle Then
					tmdb.Imdb = If(workingMovieDetails.fullmoviebody.imdbid.Contains("tt"), workingMovieDetails.fullmoviebody.imdbid, "")
					tmdb.TmdbId = workingMovieDetails.fullmoviebody.tmdbid
					posterArray.AddRange(tmdb.McPosters)
				Else
					tmdb.SetId = workingMovieDetails.fullmoviebody.TmdbSetId
					posterArray.AddRange(tmdb.McSetPosters)
				End If

			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			Finally
				messbox.Close()
				Call mov_PosterSelectionDisplay()
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_MPDB_posters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_MPDB_posters.Click
		Try
			If Not workingMovieDetails.fullmoviebody.imdbid.Contains("tt") Then
				MsgBox("No IMDB ID" & vbCrLf & "Searching Movie Poster DB halted")
				Exit Sub
			End If
			messbox = New frmMessageBox("Please wait,", "", "Retrieving Movie Poster List")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
			Call mov_PosterInitialise()
			Dim newobject As New class_mpdb_thumbs.Class1
			newobject.MCProxy = Utilities.MyProxy
			Dim teststring As New XmlDocument
			Dim testthumbs As String = String.Empty
			Try
				testthumbs = newobject.get_mpdb_thumbs(workingMovieDetails.fullmoviebody.imdbid)
				testthumbs = "<totalthumbs>" & testthumbs & "</totalthumbs>"
				teststring.LoadXml(testthumbs)
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
				Thread.Sleep(1)
			End Try
			Dim thumbstring As New XmlDocument
			Try
				thumbstring.LoadXml(testthumbs)
				For Each thisresult As XmlNode In thumbstring("totalthumbs")
					Select Case thisresult.Name
						Case "thumb"
							Dim newposters As New McImage
							newposters.hdUrl = thisresult.InnerText
							newposters.ldUrl = thisresult.InnerText
							posterArray.Add(newposters)
							'newposters.ldUrl = Nothing
							'newposters.hdUrl = Nothing
					End Select
				Next
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
			messbox.Close()
			Call mov_PosterSelectionDisplay()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_IMPA_posters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_IMPA_posters.Click
		Try
			messbox = New frmMessageBox("Please wait,", "", "Retrieving Movie Poster List")
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			messbox.Show()
			Me.Refresh()
			messbox.Refresh()
			Call mov_PosterInitialise()
			Dim newobject2 As New IMPA.getimpaposters
			newobject2.MCProxy = Utilities.MyProxy
			Try
				Dim title As String = CleanMovieTitle(workingMovieDetails.fullmoviebody.title)
				Dim posters(,) As String = newobject2.getimpaafulllist(title, workingMovieDetails.fullmoviebody.year)
				For f = 0 To UBound(posters)
					If posters(f, 0) <> Nothing Then
						If posters(f, 1) = Nothing Then posters(f, 1) = posters(f, 0)
						Dim poster As New McImage
						poster.hdUrl = posters(f, 0)
						poster.ldUrl = posters(f, 1)
						posterArray.Add(poster)
					End If
				Next
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
			messbox.Close()
			Call mov_PosterSelectionDisplay()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnPosterTabs_SaveImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPosterTabs_SaveImage.Click
		MoviePosterSave()
	End Sub

	Private Sub btnMovPosterURLorBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovPosterURLorBrowse.Click
		Try
			Dim t As New frmImageBrowseOrUrl
			t.Location = Me.PointToScreen(New Point(gbMoviePosterSelection.Left + 10, gbMoviePosterSelection.Top + gbMoviePosterSelection.Height))
			t.ShowDialog()
			If t.DialogResult = Windows.Forms.DialogResult.Cancel Or t.tb_PathorUrl.Text = "" Then
				t.Dispose()
				Exit Sub
			End If
			Dim PathOrUrl As String = t.tb_PathorUrl.Text
			t.Dispose()
			t = Nothing
			Dim tempstring As String = ""
			Dim aok As Boolean = True
			Dim cachename As String = Utilities.Download2Cache(PathOrUrl)
			If cachename <> "" Then
				If Not MovPosterToggle Then
					Dim Paths As List(Of String) = Pref.GetPosterPaths(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fileinfo.videotspath)
					For Each pth As String In Paths
						Try
							File.Copy(cachename, pth, True)
						Catch ex As Exception
							aok = False
						End Try
					Next
					If aok Then
						util_ImageLoad(PictureBoxAssignedMoviePoster, cachename, Utilities.DefaultPosterPath)
						util_ImageLoad(PbMoviePoster, cachename, Utilities.DefaultPosterPath)
						Dim path As String = Utilities.save2postercache(workingMovieDetails.fileinfo.fullpathandfilename, cachename, WallPicWidth, WallPicHeight)
						updateposterwall(path, workingMovieDetails.fileinfo.fullpathandfilename)
					End If
				Else
					Dim MovSetPosterSavePath As String = workingMovieDetails.fileinfo.movsetposterpath
					If MovSetPosterSavePath <> "" Then
						Movie.SavePosterImageToCacheAndPath(cachename, MovSetPosterSavePath)
						MovPanel6Update()
						util_ImageLoad(PictureBoxAssignedMoviePoster, MovSetPosterSavePath, Utilities.DefaultPosterPath)
					Else
						messbox.Close()
						MsgBox("!!  Problem formulating correct save location for Poster" & vbCrLf & "                    Please check your settings")
					End If
				End If

			Else
				aok = False
			End If
			If Not aok Then MsgBox("There was a problem downloading the Image")

			UpdateMissingPoster()
			XbmcLink_UpdateArtwork()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub BtnGoogleSearchPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGoogleSearchPoster.Click
		GoogleSearch("+poster")
	End Sub

	Private Sub btnMoviePosterResetImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoviePosterResetImage.Click
		Try
			PictureBoxAssignedMoviePoster.Image = PbMoviePoster.Image
			btnMoviePosterResetImage.Enabled = False
			btnMoviePosterSaveCroppedImage.Enabled = False
			lblCurrentLoadedPoster.Text = "Width: " & PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & PictureBoxAssignedMoviePoster.Image.Height.ToString
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMoviePosterSaveCroppedImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoviePosterSaveCroppedImage.Click
		Try
			Try
				Dim posterpath As String = ""
				'Dim stream As New IO.MemoryStream
				Dim PostPaths As List(Of String) = Pref.GetPosterPaths(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fileinfo.videotspath)
				Dim bitmap3 As New Bitmap(PictureBoxAssignedMoviePoster.Image)
				For Each pth As String In PostPaths
					bitmap3.Save(pth, System.Drawing.Imaging.ImageFormat.Jpeg)
					posterpath = pth
				Next
				bitmap3.Dispose()
				GC.Collect()
				util_ImageLoad(PbMoviePoster, posterpath, Utilities.DefaultPosterPath) '.Image = bmp4
				btnMoviePosterResetImage.Enabled = False
				btnMoviePosterSaveCroppedImage.Enabled = False
				Dim path As String = Utilities.save2postercache(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fileinfo.posterpath, WallPicWidth, WallPicHeight)
				updateposterwall(path, workingMovieDetails.fileinfo.fullpathandfilename)
				XbmcLink_UpdateArtwork()
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovPasteClipboardPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovPasteClipboardPoster.Click
		If AssignClipboardImage(PictureBoxAssignedMoviePoster) Then
			MoviePosterSave(True)
			lblCurrentLoadedPoster.Text = "Width: " & PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & PictureBoxAssignedMoviePoster.Image.Height.ToString
			btnMoviePosterSaveCroppedImage.Enabled = True
		End If
	End Sub

	Private Sub btn_MovEnableCrop_Click(sender As System.Object, e As System.EventArgs) Handles btnMoviePosterEnableCrop.Click
		Try
			cropMode = "poster"
			Using t As New frmMovPosterCrop
				If Pref.MultiMonitoEnabled Then
					t.bounds = screen.allscreens(form1.currentscreen).bounds
					t.startposition = formstartposition.manual
				end if
                Dim ms As IO.MemoryStream = New IO.MemoryStream()
                Using r As IO.Filestream = File.Open(PictureBoxAssignedMoviePoster.Tag.ToString, IO.FileMode.Open)
                    r.CopyTo(ms)
                End Using
				t.img = New Bitmap(ms)
                ms.Dispose()
				t.cropmode = "poster"
				t.title = workingMovie.title
				t.Setup()
				t.ShowDialog()
				If Not IsNothing(t.newimg) Then
					btnMoviePosterSaveCroppedImage.Enabled = True
					btnMoviePosterResetImage.Enabled = True
					PictureBoxAssignedMoviePoster.Image = t.newimg
					lblCurrentLoadedPoster.Text = "Width: " & PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & PictureBoxAssignedMoviePoster.Image.Height.ToString
				End If
			End Using
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnPrevNextMissingPoster_Click(sender As System.Object, e As System.EventArgs) Handles btnPrevMissingPoster.Click, btnNextMissingPoster.Click
		btnPrevMissingPoster.Enabled = False
		btnNextMissingPoster.Enabled = False

		PictureBoxAssignedMoviePoster.Image = Nothing
		DataGridViewMovies.ClearSelection()
		DataGridViewMovies.Rows(sender.Tag).Selected = True
		DisplayMovie()
		UpdateMissingPosterNav()
	End Sub

	Private Sub btnMovPosterToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovPosterToggle.Click
		'If Not BWs.Count = 0 Then
		'    _cancelled = True
		'    Do Until Cancelled = False
		'        Thread.Sleep(100)
		'        Application.DoEvents()
		'        If BWs.Count = 0 Then _cancelled = False
		'    Loop
		'End If
		If MovPosterToggle Then
			btnMovPosterToggle.Text = "Show MovieSet Posters"
			btnMovPosterToggle.BackColor = System.Drawing.Color.Lime
			btn_IMPA_posters.Enabled = True
			btn_MPDB_posters.Enabled = True
			btn_IMDB_posters.Enabled = True
			util_ImageLoad(PictureBoxAssignedMoviePoster, workingMovieDetails.fileinfo.posterpath, Utilities.DefaultPosterPath)
			Call mov_PosterInitialise()
		Else
			btnMovPosterToggle.Text = "Show Movie Posters"
			btnMovPosterToggle.BackColor = System.Drawing.Color.Aqua
			btn_IMPA_posters.Enabled = False
			btn_MPDB_posters.Enabled = False
			btn_IMDB_posters.Enabled = False
			util_ImageLoad(PictureBoxAssignedMoviePoster, workingMovieDetails.fileinfo.movsetposterpath, Utilities.DefaultPosterPath)
			Call mov_PosterInitialise()
		End If
		MovPosterToggle = Not MovPosterToggle
	End Sub

	Private Function MoviePosterSave(Optional clipbrd As Boolean = False) As Boolean
		If ImgBw.Isbusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		Dim allok As Boolean = False
		Try
			Dim tempstring As String = ""
			Dim tempint As Integer = 0
			Dim realnumber As Integer = 0
			Dim tempstring2 As String = ""
			'Dim allok As Boolean = False
			Dim backup As String = ""
			If messbox.Visible Then messbox.Close()
			messbox = New frmMessageBox("Downloading Poster...")
			messbox.Show()
			If clipbrd Then
				tempstring2 = PictureBoxAssignedMoviePoster.Tag.ToString
				allok = True
			Else
				For Each button As Control In Me.panelAvailableMoviePosters.Controls
					If button.Name.IndexOf("postercheckbox") <> -1 Then
						Dim b1 As RadioButton = CType(button, RadioButton)
						If b1.Checked = True Then
							tempstring = b1.Name
							If tempstring.IndexOf("postercheckbox") <> -1 Then
								tempstring = tempstring.Replace("postercheckbox", "")
								tempint = Convert.ToDecimal(tempstring)
								If tempstring2 = Nothing Then
									tempint = Convert.ToDecimal(tempstring)
									tempint = tempint + ((currentPage - 1) * 10)
									If cbMoviePosterSaveLoRes.Enabled = True Then
										If cbMoviePosterSaveLoRes.CheckState = CheckState.Checked Then
											tempstring2 = posterArray(tempint).ldUrl
										Else
											tempstring2 = posterArray(tempint).hdUrl
											backup = posterArray(tempint).ldUrl
										End If
									Else
										tempstring2 = posterArray(tempint).hdUrl
									End If
									allok = True
									Exit For
								End If
							End If
						End If
					End If
				Next
			End If

			If allok = False Then
				MsgBox("No Poster Is Selected")
				Return allok
			End If
			Try
				If Not MovPosterToggle Then
					util_ImageLoad(PictureBoxAssignedMoviePoster, Utilities.DefaultPosterPath, Utilities.DefaultPosterPath)
					Dim Paths As List(Of String) = Pref.GetPosterPaths(workingMovieDetails.fileinfo.fullpathandfilename, workingMovieDetails.fileinfo.videotspath)
					Dim success As Boolean = DownloadCache.SaveImageToCacheAndPaths(tempstring2, Paths, False, , , True)

					Dim path As String = Utilities.save2postercache(workingMovieDetails.fileinfo.fullpathandfilename, Paths(0), WallPicWidth, WallPicHeight)
					updateposterwall(path, workingMovieDetails.fileinfo.fullpathandfilename)
					util_ImageLoad(PictureBoxAssignedMoviePoster, Paths(0), Utilities.DefaultPosterPath)
					util_ImageLoad(PbMoviePoster, Paths(0), Utilities.DefaultPosterPath)
					lblCurrentLoadedPoster.Text = "Width: " & PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & PictureBoxAssignedMoviePoster.Image.Height.ToString
					lblCurrentLoadedPoster.Refresh()

					XbmcLink_UpdateArtwork()
				Else
					Dim MovSetPosterSavePath As String = workingMovieDetails.fileinfo.movsetposterpath
					If MovSetPosterSavePath <> "" Then
						Movie.SavePosterImageToCacheAndPath(tempstring2, MovSetPosterSavePath)
						MovPanel6Update()
						util_ImageLoad(PictureBoxAssignedMoviePoster, MovSetPosterSavePath, Utilities.DefaultPosterPath)
					Else
						messbox.Close()
						MsgBox("!!  Problem formulating correct save location for Poster" & vbCrLf & "                    Please check your settings")
					End If
				End If
			Catch ex As Exception
				ExceptionHandler.LogError(ex)
#If SilentErrorScream Then
                Throw ex
#End If
			End Try
			UpdateMissingPoster()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
		messbox.Close()
		Return allok
	End Function

#End Region

#Region "Movie Fanart.TV Tab"

	Private Sub tpMovFanartTv_Leave(sender As Object, e As EventArgs) Handles tpMovFanartTv.Leave
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		Mov_PictureboxLoad()
	End Sub

#End Region

#Region "Movie Set Tab"

    Private Sub tpMovSets_Leave(sender As Object, e As EventArgs) Handles tpMovSets.Leave
        If MovSetOverviewEdit Then
            Dim result As MsgBoxResult = MsgBox("Edits made to overview," & vbCrLf & "do you wish to save edits?", MsgBoxStyle.OkCancel)
            If result = MsgBoxResult.Cancel Then
                tb_MovieSetOverviewReset()
                Exit Sub
            End If
            btn_MovSetOverviewSave.PerformClick()
        End If
    End Sub

    Private Sub cbMovSetListAllSets_CheckedChanged(sender As Object, e As EventArgs) Handles cbMovSetListAllSets.CheckedChanged
        If Not MainFormLoadedStatus Then Exit Sub
        MovSetListAllSets = cbMovSetListAllSets.Checked
        MovieSetstabSetup()
    End Sub

    Private Sub MovieSetstabSetup()
        MovSetDgvLoad()
		MovSetArtworkCheck()
    End Sub
    
	''' <summary>
	''' Main column with a list of all known movie sets (both themoviedb.org and manual sets)
	''' </summary>
	Private Sub MovSetDgvLoad()
		dgvMovieSets.Rows.Clear()
		Pref.moviesets.Sort()
        RemoveHandler dgvMovieSets.CellEnter, Addressof dgvMovieSets_CellEnter

        ''Get custom Sets first
        Dim Tmplist As New List(Of TmdbCustomSetName)
		For Each mset In Pref.moviesets
            If mset.ToLower = "-none-" Then Continue For
            Dim tmpmov As New TmdbCustomSetName("", "", mset)
            Tmplist.Add(tmpmov)
        Next

        '' Now add Movies from MovieSetCache.
        For each mset In oMovies.MovieSetDB
            Dim custom As Boolean = False
            If mset.TmdbSetId.StartsWith("L") Then
                Dim p = From x In oMovies.MovieCache Where x.SetName = mset.MovieSetDisplayName
                If Not p.Count = 0 Then custom = True
            End If
            Dim q = From x In oMovies.MovieCache Where x.TmdbSetId = mset.TmdbSetId
            If (q.Count = 0 AndAlso Not custom) And Not MovSetListAllSets Then Continue For
            Dim tmpmov As New TmdbCustomSetName(mset.TmdbSetId, "", mset.MovieSetDisplayName)
            Tmplist.Add(tmpmov)
        Next
        If Tmplist.Count > 1 Then Tmplist = Tmplist.OrderBy(Function(x) x.MovieSetName).ToList
        
        '' Populate MovieSets DataGridView.
        For Each mset In Tmplist
            
            ''' New MovieSetTab population
			If mset.MovieSetName <> "-None-" Then
				Dim row As DataGridViewRow = DirectCast(dgvMovieSets.RowTemplate.Clone(), DataGridViewRow)
				'' fanart and poster columns not functional yet!
				' if a set has films but not completed, then show missing
				' if a set has all movies, then show is correct (change to complete)
				' if a set is done manually don't show themoviedb ID at all (empty) but some other marking (other color?)
				If mset.MovieSetId = String.Empty Then
					row.CreateCells(dgvMovieSets, mset.MovieSetName, Global.Media_Companion.My.Resources.Resources.error24)
				ElseIf mset.MovieSetId.Chars(0) = "L" Then
					row.CreateCells(dgvMovieSets, mset.MovieSetName, Global.Media_Companion.My.Resources.Resources.correct_manual24)
				Else
					row.CreateCells(dgvMovieSets, mset.MovieSetName, Global.Media_Companion.My.Resources.Resources.correct)
				End If
				If mset.MovieSetId <> "-None-" Then row.Cells(1).Tag = mset.MovieSetId
				dgvMovieSets.Rows.Add(row)
			End If
		Next
        AddHandler dgvMovieSets.CellEnter, AddressOf dgvMovieSets_CellEnter
        If Tmplist.Count > 0 AndAlso Not IsNothing(dgvMovieSets.CurrentRow) Then
            dgvpopulate(dgvMovieSets.Rows(dgvMovieSets.CurrentRow.Index).Cells(0).Value)
        Else If IsNothing(dgvMovieSets.CurrentRow) Then
            DataGridViewSelectedMovieSet.Rows.Clear()
            tb_MovieSetOverview.Text = ""
            tb_MovieSetOverviewReset
            lbCollectionCount.Text  = ""
            tbMovieSetTitle.Text    = ""
        End If
	End Sub

	
    ''' <summary>
    ''' Region to fill the current movies in the selected collection
    ''' </summary>
    Private Sub dgvMovieSets_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMovieSets.CellEnter
        Try
            Application.DoEvents()
            If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub
            dgvpopulate(dgvMovieSets.Rows(e.RowIndex).Cells(0).Value)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub dgvpopulate(ByVal MsetName As String, Optional refresh As Boolean = False)
        Try
            tbMovSetWarn.Visible = False
            tbMovSetWarn.Text = ""
            Dim MovSet As MovieSetInfo = oMovies.FindMovieSetInfoBySetDisplayName(MsetName)
            Dim found As Boolean = False
            If refresh OrElse MsetName <> tbMovieSetTitle.Text OrElse dgvMovieSets.Rows.Count = 1 Then
                tb_MovieSetOverviewReset()
                Dim CustomCollection As Boolean = False
                Dim dirtycollection As Boolean = False
                Dim MovCollectionList As New List(Of MovieSetDatabase)

                '''Check if custom set, get collection from MovieCache.
                If MovSet.TmdbSetId = "" Then
                    CustomCollection = True
                    For Each Mov As ComboList In oMovies.MovieCache
                        If Mov.SetName = Msetname Then
                            Dim ac As New MovieSetDatabase
                            ac.title    = Mov.title
                            ac.tmdbid   = Mov.tmdbid
                            ac.year     = Mov.year.ToString
                            ac.present  = True
                            found       = True
                            MovCollectionList.Add(ac)
                        End If
                    Next
                    If MovCollectionList.Count = 0 Then
                        tbMovieSetTitle.Text = "No Data found for this collection!!"
                        DataGridViewSelectedMovieSet.Rows.Clear()
                        lbCollectionCount.Text = "Warning, possible incomplete collection Data!"
                        lbCollectionCount.BackColor = Color.Red
                        Exit Sub
                    End If
                Else    'Belongs to a Collection and is in MovieSetDb
                    For Each mset In oMovies.MovieSetDB
                        If mset.TmdbSetId = MovSet.TmdbSetId Then
                            dirtycollection = mset.Dirty
                            If mset.Collection.Count > 0 Then
                                For Each collect In mset.Collection
                                    Dim ac As New MovieSetDatabase
                                    ac.title    = collect.MovieTitle
                                    ac.tmdbid   = collect.TmdbMovieId
                                    ac.year     = collect.ReleaseYear
                                    MovCollectionList.Add(ac)
                                Next
                            End If
                            If mset.TmdbSetId.StartsWith("L") Then
                                Dim q = From x In oMovies.MovieCache Where x.SetName = mset.MovieSetDisplayName
                                If Not q.Count = 0 Then
                                    For Each collect In q.Distinct()
                                        CustomCollection = True
                                        Dim ac As New MovieSetDatabase
                                        ac.title    = collect.title
                                        ac.tmdbid   = collect.tmdbid
                                        ac.year     = collect.year.ToString
                                        MovCollectionList.Add(ac)
                                    Next
                                End If
                            End If
                            Exit For
                        End If

                    Next
            
                    For Each x In MovCollectionList
                        Dim q = From y In oMovies.MovieCache Where y.tmdbid = x.tmdbid
                        If q.Count = 0 Then Continue For
                        found = True
                        x.present = True
                    Next
                End If
                
                If Not found Then
                    Dim message As String = MovCollectionList.Count & " Movie(s) in:  " & """" & MovSet.MovieSetName & """" & vbCrLf  & "But no matching movies found in MediaCompanion." & vbCrLf
                    message &= "Recommend Rescrape Wizard to populate Movie Collection data." & vbCrLf
                    tbMovSetWarn.Visible = True
                    tbMovSetWarn.Text = message
                End If
            
                tbMovieSetTitle.Text = If(CustomCollection, MsetName, MovSet.MovieSetName)
                tb_MovieSetOverview.Text = Pref.decxmlchars(MovSet.MovieSetPlot)
                DataGridViewSelectedMovieSet.Rows.Clear()
                Dim count As Integer = 0
                If found Then
                For Each item In MovCollectionList
                    Dim row As DataGridViewRow = DirectCast(DataGridViewSelectedMovieSet.RowTemplate.Clone(), DataGridViewRow)
                    row.CreateCells(DataGridViewSelectedMovieSet, If(item.present, Global.Media_Companion.My.Resources.Resources.correct, Global.Media_Companion.My.Resources.Resources.missing24), item.title, If(item.year <> "", "("& item.year & ")", "???"))
                    DataGridViewSelectedMovieSet.Rows.Add(row)
                    If Not item.present Then count += 1
                Next
                DataGridViewSelectedMovieSet.Sort(DataGridViewSelectedMovieSet.Columns(2), System.ComponentModel.ListSortDirection.Ascending)
                End If
                If Not dirtycollection Then
                    Dim label As String = "Movies in collection:  " & MovCollectionList.Count
                    If Not count = 0 Then
                        label &= "    :- Missing " & count & " movie" & If(count > 1, "s", "")
                    Else
                        If found Then
                            If Not CustomCollection Then
                                label &= "   : - Collection is Complete!"
                            Else
                                label &= "   : - Custom Collection."
                            End If
                        Else
                            If count = 0 Then label &= "   :- All Missing!"
                        End If
                        
                    End If
                    lbCollectionCount.BackColor = Color.LightYellow
                    lbCollectionCount.Text = label
                Else
                    lbCollectionCount.Text = "Warning, possible incomplete collection Data!"
                    lbCollectionCount.BackColor = Color.Red
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvMovieSets_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dgvMovieSets.MouseDoubleClick
		Dim columnno As Integer = dgvMovieSets.SelectedCells(0).ColumnIndex
		If columnno < 2 Then Exit Sub
		If dgvMovieSets.SelectedCells(0).Tag <> Nothing Then
			util_ZoomImage(dgvMovieSets.SelectedCells(0).Tag.ToString)
		End If
	End Sub
    
	Private Sub btnMovieSetAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieSetAdd.Click
		Try
            Dim newsetname As String = ""
            Using newForm As New frmMovSetAdd
                If newForm.ShowDialog() <> DialogResult.OK Then
			        Exit Sub
                Else
                    newsetname = newForm.newset
                End If
            End Using
			'Dim newForm As New frmMovSetAdd()
            If newsetname <> ""
                Dim tmpmovset As New MovieSetInfo
                tmpmovset.MovieSetName = newsetname
                tmpmovset.TmdbSetId = oMovies.setMovieSetID("", oMovies.MovieSetDB)
                Dim q = From x In oMovies.MovieSetDB Where x.MovieSetName = tmpmovset.MovieSetName
                If q.Count = 0 Then
                    oMovies.AddUpdateMovieSetInCache(tmpmovset)
                Else
                    MsgBox("Set already exists in Database", MsgBoxStyle.Critical, "Error")
                    Exit Sub
                End If
                'Pref.moviesets.Add(newForm.newset)
                'Pref.moviesets.Sort()
            End If
            'newForm = Nothing

			MovieSetstabSetup()
			pop_cbMovieDisplay_MovieSet()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub
        
	Private Sub btnMovieSetRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieSetRemove.Click
		Try
			Dim SelectedMovieSet As String = dgvMovieSets.SelectedCells(0).Value
            Dim q = From x In oMovies.MovieSetDB Where x.MovieSetName = SelectedMovieSet
            If Not q.Count = 0 Then
                Dim tempid As String = ""
                tempid = q(0).TmdbSetId
                Dim p = From x In oMovies.MovieCache Where x.TmdbSetId = tempid
                If p.Count > 0 Then
                    MsgBox("Selected collection name is allocated to Movies, and" & vbCrLf & _
                       "            has valid TMDB Set Id present" & vbcrlf &  _
                       "          Can not be removed from this tab.")
                    Exit Sub
                End If
            End If
			If Not RemoveFromMovieSetCache(SelectedMovieSet) Then
				MsgBox("Setname selected is already allocated to a" & vbCrLf & "   movie in Media Companions cache" & vbCrLf & "      unable to remove is in use.")
				Exit Sub
			End If
			dgvMovieSets.Rows.RemoveAt(dgvMovieSets.CurrentRow.Index)
			pop_cbMovieDisplay_MovieSet()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub
    
    Private Sub btnMovieSetsRepopulate_Click(sender As System.Object, e As System.EventArgs) Handles btnMovieSetsRepopulate.Click
		MovSetsRepopulate()
		MovieSetstabSetup()
		pop_cbMovieDisplay_MovieSet()
	End Sub

	Private Sub MovSetsRepopulate()
		Pref.moviesets.Clear()
		Pref.moviesets.Add("-None-")
		Pref.moviesets.AddRange(oMovies.MovieSetsNoSetId)
        MergePrefMovieSetToMovieSetDatabase
	End Sub 'btn_MovSetOverviewEdit

    Private Sub btn_MovSetOverviewEdit_Click(sender As System.Object, e As System.EventArgs) Handles btn_MovSetOverviewEdit.Click
		tb_MovieSetOverview.ReadOnly = False
        tb_MovieSetOverview.BackColor = SystemColors.Window
        tb_MovieSetOverview.ShortcutsEnabled = True
        MovSetOverviewEdit = True
	End Sub

    Private Sub btn_MovSetOverviewSave_Click(sender As System.Object, e As System.EventArgs) Handles btn_MovSetOverviewSave.Click
        If tb_MovieSetOverviewChanged Then
		    Try
                Dim MovSet As MovieSetInfo = oMovies.FindMovieSetInfoBySetDisplayName(dgvMovieSets.Rows(dgvMovieSets.CurrentRow.Index).Cells(0).Value)
                MovSet.MovieSetPlot = tb_MovieSetOverview.Text
                oMovies.AddUpdateMovieSetInCache(MovSet, True)
                If Pref.MovSetOverviewToNfo Then
                    Dim q = From x In DataGridViewMovies.Rows Where x.cells("tmdbsetid").Value = MovSet.TmdbSetId
                    If q.Count > 0 Then
                        For each t In q
                            Dim movpath As String = t.Cells("fullpathandfilename").Value.ToString
                            Dim tmpmov As FullMovieDetails = WorkingWithNfoFiles.mov_NfoLoadFull(movpath)
                            tmpmov.fullmoviebody.SetOverview = MovSet.MovieSetPlot
                            WorkingWithNfoFiles.mov_NfoSave(movpath, tmpmov, True)
                        Next
                    End If
                End If
		    Catch ex As Exception

		    End Try
        End If
        tb_MovieSetOverviewReset()
	End Sub

    Private Sub tb_MovieSetOverview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_MovieSetOverview.KeyPress
        If e.KeyChar = Convert.ToChar(1) Then
            DirectCast(sender, TextBox).SelectAll()
            e.Handled = True
        End If
    End Sub

    Private Sub tb_MovieSetOverview_TextChanged(sender As Object, e As EventArgs) Handles tb_MovieSetOverview.TextChanged
        If MovSetOverviewEdit Then
            tb_MovieSetOverviewChanged = True
            btn_MovSetOverviewSave.Enabled = True
        End If
    End Sub

    Private Sub tb_MovieSetOverviewReset()
        MovSetOverviewEdit = False
        tb_MovieSetOverviewChanged = False 
        tb_MovieSetOverview.ReadOnly = True
        tb_MovieSetOverview.BackColor = SystemColors.Control
        tb_MovieSetOverview.ShortcutsEnabled = False
        btn_MovSetOverviewSave.Enabled = False
        Application.DoEvents()
    End Sub

    Private Sub MovSetArtworkCheck()
		For Each row As DataGridViewRow In dgvMovieSets.Rows
			Dim mset As String = row.Cells(0).Value
            Dim NoArtFound As Boolean = True
			For Each mov In oMovies.MovieCache
				If mov.SetName = mset Then
                    NoArtFound = False
					Dim movsetfanart As String = Pref.GetMovSetFanartPath(mov.fullpathandfilename, mset)
					Dim movsetposter As String = Pref.GetMovSetPosterPath(mov.fullpathandfilename, mset)
					If File.Exists(movsetfanart) Then
						row.Cells(2).Value = Global.Media_Companion.My.Resources.Resources.correct
						row.Cells(2).Tag = movsetfanart
					Else
						row.Cells(2).Value = Global.Media_Companion.My.Resources.Resources.incorrect
						row.Cells(2).Tag = Nothing
					End If
					If File.Exists(movsetposter) Then
						row.Cells(3).Value = Global.Media_Companion.My.Resources.Resources.correct
						row.Cells(3).Tag = movsetposter
					Else
						row.Cells(3).Value = Global.Media_Companion.My.Resources.Resources.incorrect
						row.Cells(3).Tag = Nothing
					End If
					Exit For
				End If
			Next
            If NoArtFound Then
                row.Cells(2).Value = Global.Media_Companion.My.Resources.Resources.missing24
				row.Cells(2).Tag = Nothing
                row.Cells(3).Value = Global.Media_Companion.My.Resources.Resources.missing24
				row.Cells(3).Tag = Nothing
            End If
		Next
	End Sub
    
    Private Sub dgvMovieSets_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvMovieSets.MouseDown
        Dim Fail As Boolean = False
        If Not e.Button = MouseButtons.Right Then Exit Sub
        Dim ColIndexFromMouseDown = dgvMovieSets.HitTest(e.X, e.Y).ColumnIndex
        If ColIndexFromMouseDown < 0 Then
            tsmiMovSetName.Text = ""
            Exit Sub
        End If
        Dim RowIndexFromMouseDown = dgvMovieSets.HitTest(e.X, e.Y).RowIndex
        If RowIndexFromMouseDown < 0 Then Exit Sub
        
        Dim MsetName As String = dgvMovieSets.Rows(RowIndexFromMouseDown).Cells(0).Value
        Dim MsetHasId As Boolean = False
        Dim SetId As String = dgvMovieSets.Rows(RowIndexFromMouseDown).Cells(1).Tag
        If Not SetId.StartsWith("L") AndAlso SetId <> "" AndAlso SetId.ToLower <> "-none-" Then MsetHasId = True

        
        If ColIndexFromMouseDown = 0 Then
            tsmiMovSetName      .Text       = MsetName
            tsmiMovSetRebuild   .Visible    = MsetHasId
            tsmiMovSetEditName  .Visible    = True
            tsmiMovSetGetFanart .Visible    = False
            tsmiMovSetGetPoster .Visible    = False
        ElseIf ColIndexFromMouseDown = 2 Then
            tsmiMovSetName      .Text       = MsetName
            tsmiMovSetRebuild   .Visible    = False
            tsmiMovSetEditName  .Visible    = False
            tsmiMovSetGetFanart .Visible    = MsetHasId
            tsmiMovSetGetPoster .Visible    = False
        ElseIf ColIndexFromMouseDown = 3 Then
            tsmiMovSetName      .Text       = MsetName
            tsmiMovSetRebuild   .Visible    = False
            tsmiMovSetEditName  .Visible    = False
            tsmiMovSetGetFanart .Visible    = False
            tsmiMovSetGetPoster .Visible    = MsetHasId
        End If
    End Sub

    Private Sub MovSetsContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MovSetsContextMenu.Opening
        If tsmiMovSetName.Text = "" Then
            e.Cancel = True
        End If
        tsmiMovSetName.BackColor = Color.Honeydew
        tsmiMovSetName.Font = New Font("Arial", 10, FontStyle.Bold)
    End Sub
    
    Private Sub tsmiMovSetEditName_Click(sender As Object, e As EventArgs) Handles tsmiMovSetEditName.Click
        Dim newForm As New frmMovSetAdd()
        newForm.Text = "Edit Collection Title"
        newForm.lblMovSetAdd.Text = "Enter new title for collection"
        If newForm.ShowDialog() <> DialogResult.OK Then
			Exit Sub
        Else
            If newForm.newset <> ""
                Dim changedSet As Boolean = False
                Dim OldSetName As String = tsmiMovSetName.Text

                ''' Check change against MovieSetDb, allocate to UserMovieSetName
                For Each p In oMovies.MovieSetDB
                    If p.MovieSetDisplayName = OldSetName Then
                        p.UserMovieSetName = newForm.newset
                        changedSet = True
                        Exit For
                    End If
                Next
                
                ''' If Not in database, probably custom Movie Set
                If Not changedSet Then
                    If Pref.moviesets.Contains(OldSetName) Then
                        Dim setindex As Integer = Pref.moviesets.IndexOf(OldSetName)
                        Pref.moviesets(setindex) = newForm.newset
                        Pref.moviesets.Sort()
                        changedSet = True
                    End If
                End If

                If changedSet Then
                    ''' Update all movies belonging to set to new Set Title.
                    For each mov In oMovies.MovieCache
                        If mov.SetName = OldSetName Then
                            mov.SetName = newForm.newset
                            Dim tmpmov As FullMovieDetails = WorkingWithNfoFiles.mov_NfoLoadFull(mov.fullpathandfilename)
                            tmpmov.fullmoviebody.SetName = newForm.newset
                            WorkingWithNfoFiles.mov_NfoSave(mov.fullpathandfilename, tmpmov, True)
                        End If
                    Next
                    oMovies.SaveMovieCache
                    oMovies.SaveMovieSetCache()
                    pop_cbMovieDisplay_MovieSet
                    UpdateFilteredList()
                    MovieSetstabSetup()
                    Exit Sub
                End If
            End If
		End If
    End Sub

    Private Sub tsmiMovSetGetFanart_Click(sender As System.Object, e As System.EventArgs) Handles tsmiMovSetGetFanart.Click
        Try
            Dim MovSet As MovieSetInfo = GetMovSetDetails()
            If MovSet.TmdbSetId = "" Then Exit Sub
            For f = 0 To DataGridViewMovies.RowCount - 1
                If DataGridViewMovies.Rows(f).Cells("movieset").Value.MovieSetName = MovSet.MovieSetName Then
                    DataGridViewMovies.ClearSelection()
                    DataGridViewMovies.Rows(f).Selected = True
                    DisplayMovie()
                    mov_FanartGet(True)
                    MovSetArtworkCheck()
                    Exit For
                End If
            Next
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub tsmiMovSetGetPoster_Click(sender As System.Object, e As System.EventArgs) Handles tsmiMovSetGetPoster.Click
        Try
            Dim MovSet As MovieSetInfo = GetMovSetDetails()
            If MovSet.TmdbSetId = "" Then Exit Sub
            For f = 0 To DataGridViewMovies.RowCount - 1
                If DataGridViewMovies.Rows(f).Cells("movieset").Value.MovieSetName = MovSet.MovieSetName Then
                    DataGridViewMovies.ClearSelection()
                    DataGridViewMovies.Rows(f).Selected = True
                    DisplayMovie()
                    mov_PosterGet("tmdb", True)
                    MovSetArtworkCheck()
                    Exit For
                End If
            Next
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub tsmiMovSetRebuild_Click(sender As Object, e As EventArgs) Handles tsmiMovSetRebuild.Click
        Dim MovSet As MovieSetInfo = GetMovSetDetails()
        If MovSet.TmdbSetId = "" OrElse MovSet.TmdbSetId = "-None-" OrElse MovSet.TmdbSetId.StartsWith("L") Then Exit Sub
        Try
            messbox = New frmMessageBox("Updating selected collection from TMDb", "", "Please Wait")
            messbox.Show()
            messbox.Refresh()
            Dim tmdb As New TMDb
            tmdb.SetId = MovSet.TmdbSetId
            Dim McMovieSetInfo As MovieSetInfo = tmdb.MovieSet
            If IsNothing(McMovieSetInfo) Then Exit Sub
            oMovies.AddUpdateMovieSetInCache(McMovieSetInfo, True)
            dgvpopulate(MovSet.MovieSetName, True)
        Catch ex As Exception

        Finally
            messbox.Close()
        End Try
        
    End Sub

    Private Function GetMovSetDetails() As MovieSetInfo
        Dim t As New MovieSetInfo
        For Each p In oMovies.MovieSetDB
            If p.MovieSetName = tsmiMovSetName.Text Then
                t = p
                Exit For
            End If
        Next
        Return t
    End Function
    
    Private Function RemoveFromMovieSetCache(ByVal s As String) As Boolean
		Dim aok As Boolean = True
        Dim q = From x In oMovies.MovieCache Where x.SetName = s
        If Not q.Count = 0 Then aok = False

        ''If set is allocated to one or more Movies, do not allow its removal.
		If Not aok Then Return aok


        ''Remove only from Pref.moviesets, not from MovieSetDB
        oMovies.RemoveMovieSetInCache(s)
        Pref.moviesets.Remove(s)
		Return aok
	End Function

#End Region 'Movie Set Routines

#Region "Movie Tag(s) Tab"

    Private Sub tpMovTags_Leave(sender As Object, e As EventArgs) Handles tpMovTags.Leave
        MovieRefresh = True
    End Sub

    Private Sub MovieTagsSetup()
		TagListBox.Items.Clear()
		''Here we clean up Pref.MovieTags, removing any that are also in the Tags database.
		Dim ToRemove As New List(Of String)
		For Each mtag In Pref.movietags
			If Not IsNothing(mtag)
				Dim q = From t In oMovies.TagDB Where t.TagName = mtag
				If q.Count = 0 Then
					TagListBox.Items.Add(mtag)
				'Else
					'ToRemove.Add(mtag)
				End If
			End If
		Next

		''If any duplicate tags, remove them from Pref.MovieTags
		'If ToRemove.Count > 0 Then
		'	For each t In ToRemove
		'		Pref.movietags.Remove(t)
		'	Next
		'End If
		TagsPopulate()
	End Sub

	Sub TagsPopulate()
		CurrentMovieTags.Items.Clear()
		If DataGridViewMovies.SelectedRows.Count > 1 Then
			lblMovTagMulti1.Visible = True
			lblMovTagMulti2.Visible = True
		Else
			lblMovTagMulti1.Visible = False
			lblMovTagMulti2.Visible = False
		End If
		For Each item As DataGridViewRow In DataGridViewMovies.SelectedRows
			Dim filepath As String = item.Cells("fullpathandfilename").Value.ToString
			Dim movie As Movie = oMovies.LoadMovie(filepath, False)
			For Each ctag In movie.ScrapedMovie.fullmoviebody.tag
				If Not IsNothing(ctag) Then
					If Not CurrentMovieTags.Items.Contains(ctag) Then CurrentMovieTags.Items.Add(ctag)
				End If
			Next
		Next
		CurrentMovieTags.Refresh()
	End Sub
    
	Private Sub btnMovTagListAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnMovTagListAdd.Click
		Try
			If txtbxMovTagEntry.Text <> "" Then
				Dim ex As Boolean = False
				For Each mtag In Pref.movietags
                    If (mtag.Equals(txtbxMovTagEntry.Text,  StringComparison.InvariantCultureIgnoreCase)) Then
						ex = True
						Exit For
					End If
				Next
				If ex = False Then
					Pref.movietags.Add(txtbxMovTagEntry.Text)
					TagListBox.Items.Add(txtbxMovTagEntry.Text)
					txtbxMovTagEntry.Clear()
				Else
					MsgBox("This Movie Tag Already Exists")
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovTagListRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnMovTagListRemove.Click
		Try
			For i = 0 To TagListBox.SelectedItems.Count - 1
				Dim tempboolean As Boolean = False
				If TagListBox.SelectedItems(i) <> Nothing And TagListBox.SelectedItems(i) <> "" Then
					For Each mtag In Pref.movietags
						If mtag = TagListBox.SelectedItems(i) Then
							Pref.movietags.Remove(mtag)
							Exit For
						End If
					Next
				End If
			Next

			TagListBox.Items.Clear()

			For Each mset In Pref.movietags
				If Not IsNothing(mset) Then TagListBox.Items.Add(mset)
			Next
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovTagListRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnMovTagListRefresh.Click
		Try
			TagListBox.Items.Clear()
			For x = 0 To oMovies.MovieCache.Count - 1
				Dim movtag As List(Of String) = oMovies.MovieCache(x).movietag
				For Each mtag In movtag
                    If mtag = "" Then Continue For
					If Not TagListBox.Items.Contains(mtag) Then TagListBox.Items.Add(mtag)
				Next
			Next
			'Pref.movietags.Clear()
			For Each mtag In Pref.movietags
				If Not TagListBox.Items.Contains(mtag) Then TagListBox.Items.Add(mtag)
			Next
            Dim selectedrows As New List(Of Integer)
            Dim selectedCell As Integer = Nothing
            If Not IsNothing(DataGridViewMovies.CurrentCell) Then
                selectedcell = DataGridViewMovies.CurrentCell.ColumnIndex
                If DataGridViewMovies.SelectedRows.Count > 1 Then
                    For each row As DataGridViewRow In DataGridViewMovies.SelectedRows
                        selectedrows.Add(row.Index)
                    Next
                End If
            End If
			UpdateFilteredList()
            If selectedrows.Count > 1 Then
                Dim first As Boolean = True
                DataGridViewMovies.ClearSelection()
                For each t In selectedrows
                    If first Then
                        DataGridViewMovies.CurrentCell = DataGridViewMovies.Rows(t).Cells(selectedcell)
                        first = False
                    Else
                        DataGridViewMovies.Rows(t).Selected = True
                    End If
                    
                Next
            End If
		Catch ex As Exception

		End Try
	End Sub

	Private Sub btnMovTagAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnMovTagAdd.Click
		Try
			If TagListBox.SelectedIndex <> -1 Then
				For Each item In TagListBox.SelectedItems
					If item = "" Then Exit For
					If Not CurrentMovieTags.Items.Contains("+ " & item) Then
						If CurrentMovieTags.Items.Contains("- " & item) Then
							Dim i As Integer = CurrentMovieTags.Items.IndexOf("- " & item)
							CurrentMovieTags.Items(i) = item
						ElseIf CurrentMovieTags.Items.Contains(item) Then
							Dim i As Integer = CurrentMovieTags.Items.IndexOf(item)
							CurrentMovieTags.Items(i) = "+ " & item
						Else
							CurrentMovieTags.Items.Add("+ " & item)
						End If
					End If
				Next
			End If
		Catch

		End Try
	End Sub

	Private Sub btnMovTagRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnMovTagRemove.Click
		Try
			If CurrentMovieTags.SelectedIndex <> -1 Then
				Dim i As Integer = CurrentMovieTags.SelectedIndex
				Dim item As String = CurrentMovieTags.Items(i)
				If item.Contains("+ ") Then
					CurrentMovieTags.Items.RemoveAt(i)
					Exit Sub
				End If
				If item.Contains("- ") Then Exit Sub
				item = "- " & item
				CurrentMovieTags.Items(i) = item
			End If
		Catch

		End Try
	End Sub

	Private Sub btnMovTagSavetoNfo_Click(sender As System.Object, e As System.EventArgs) Handles btnMovTagSavetoNfo.Click
		Try
            Dim MultiMovie As Boolean = DataGridViewMovies.SelectedRows.Count > 1
			If CurrentMovieTags.Items.Count <> -1 Then
				NewTagList.Clear()
				For Each tags In CurrentMovieTags.Items
					NewTagList.Add(tags)
				Next
				mov_SaveQuick()
			End If
		Catch ex As Exception

		End Try
	End Sub

	Private Sub txtbxMovTagEntry_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxMovTagEntry.KeyPress
		If e.KeyChar = Convert.ToChar(Keys.Enter) Then

			btnMovTagListAdd.PerformClick()

			'This tells the system not to process the key, as you've already taken care 
			'of it 
			e.Handled = True
		End If

	End Sub
#End Region 'Tag(s) Section

#Region "Movie Web Tab"

	Private Sub btnMovWebStop_Click(sender As System.Object, e As System.EventArgs) Handles btnMovWebStop.Click
		WebBrowser2.Stop()
		WebBrowser2.Focus()
	End Sub

	Private Sub btnMovWebRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnMovWebRefresh.Click
		Try
			WebBrowser2.Refresh()
		Catch
		End Try
		WebBrowser2.Focus()
	End Sub

	Private Sub btnMovWebBack_Click(sender As System.Object, e As System.EventArgs) Handles btnMovWebBack.Click
		Try
			WebBrowser2.GoBack()
		Catch
		End Try
		WebBrowser2.Focus()
	End Sub

	Private Sub btnMovWebForward_Click(sender As System.Object, e As System.EventArgs) Handles btnMovWebForward.Click
		Try
			WebBrowser2.GoForward()
		Catch
		End Try
		WebBrowser2.Focus()
	End Sub

	Private Sub btnMovWebTMDb_Click(sender As System.Object, e As System.EventArgs) Handles btnMovWebTMDb.Click
		Dim TMDB As String = workingMovieDetails.fullmoviebody.tmdbid
		If TMDB = "" Then
			MsgBox("Selected Movie does not contain a TMDb ID number")
			Exit Sub
		End If
		Dim TMDBLan As List(Of String) = Utilities.GetTmdbLanguage(Pref.TMDbSelectedLanguageName)
		Dim weburl = "https://www.themoviedb.org/movie/" & TMDB & "?language=" & TMDBLan(0)
		Try
			WebBrowser2.Hide()
			WebBrowser2.Navigate("about:blank")
			Do Until WebBrowser2.ReadyState = WebBrowserReadyState.Complete
				Application.DoEvents()
				System.Threading.Thread.Sleep(100)
			Loop
			WebBrowser2.Show()
			WebBrowser2.Stop()
			WebBrowser2.Navigate(weburl)
		Catch
		End Try
		WebBrowser2.Focus()
	End Sub

	Private Sub btnMovWebIMDb_Click(sender As System.Object, e As System.EventArgs) Handles btnMovWebIMDb.Click
		Dim IMDB As String = workingMovieDetails.fullmoviebody.imdbid
		If Not IMDB.Contains("tt") Then
			MsgBox("Selected Movie does not contain a IMDb ID number")
			Exit Sub
		End If
		Dim weburl = "http://www.imdb.com/title/" & IMDB & "/"
		Try
			WebBrowser2.Hide()
			WebBrowser2.Navigate("about:blank")
			Do Until WebBrowser2.ReadyState = WebBrowserReadyState.Complete
				Application.DoEvents()
				System.Threading.Thread.Sleep(100)
			Loop
			WebBrowser2.Show()
			WebBrowser2.Stop()
			WebBrowser2.Navigate(weburl)
		Catch
		End Try
		WebBrowser2.Focus()
	End Sub


#End Region

#Region "Movie Change Movie Tab"

	Private Sub WebBrowser1_NewWindow(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles WebBrowser1.NewWindow
		Try
			e.Cancel = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnChangeMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeMovie.Click
		Dim messagestring As String = "Changing the movie will Overwrite all the current details"
		messagestring &= vbCrLf & "If this is an offline video, please delete folder and add as New" & vbCrLf & "Do you wish to continue?"
        ChangeMovieId = ""
        Dim RegexString As String = ""
		If MovieSearchEngine = "imdb" Then RegexString = "(tt\d{7})"
        If MovieSearchEngine = "tmdb" Then RegexString = "movie/(\d*)"

        If Not RegexString = "" Then
            Dim mat As Match = Regex.Match(WebBrowser1.Url.ToString, RegexString)
            If mat.Success Then
				ChangeMovieId = mat.Groups(1).Value.ToString
			Else
				MsgBox("Please Browse to a Movie page")
				Exit Sub
			End If
            If ChangeMovieId <> "" Then
                If MessageBox.Show(messagestring, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub
		        RunBackgroundMovieScrape("ChangeMovie")
            Else
				MsgBox("Please Browse to a Movie page")
				Exit Sub
            End If
        End If
		TabControl2.SelectedIndex = 0
	End Sub

	Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
		Try
			WebBrowser1.GoBack()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_IMDBSearch_Click(sender As System.Object, e As System.EventArgs) Handles btn_IMDBSearch.Click
		MovieSearchEngine = "imdb"
		mov_ChangeMovieSetup(MovieSearchEngine)
	End Sub

	Private Sub btn_TMDBSearch_Click(sender As System.Object, e As System.EventArgs) Handles btn_TMDBSearch.Click
		MovieSearchEngine = "tmdb"
		mov_ChangeMovieSetup(MovieSearchEngine)
	End Sub

	Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
		Pref.MovieChangeKeepExistingArt = Not CheckBox2.Checked
	End Sub

#End Region

#Region "Movie Folder Tab"

	'Movie Folders

	Private Sub tpMovFolders_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpMovFolders.Leave
		Try
			If moviefolderschanged Then
				Dim save = MsgBox("You have made changes to some folders" & vbCrLf & "    Do you wish to save these changes?", MsgBoxStyle.YesNo)
				If save = DialogResult.Yes Then
					ButtonSaveAndQuickRefresh.PerformClick()
				End If
				moviefolderschanged = False
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnMovieManualPathAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnMovieManualPathAdd.Click
		Try
			If tbMovieManualPath.Text = Nothing Then
				Exit Sub
			End If
			If tbMovieManualPath.Text = "" Then
				Exit Sub
			End If
			Dim tempstring As String = tbMovieManualPath.Text
			Do While tempstring.LastIndexOf("\") = tempstring.Length - 1
				tempstring = tempstring.Substring(0, tempstring.Length - 1)
			Loop
			Do While tempstring.LastIndexOf("/") = tempstring.Length - 1
				tempstring = tempstring.Substring(0, tempstring.Length - 1)
			Loop
			Dim exists As Boolean = False
			For Each item In clbx_MovieRoots.items
                If (item.ToString.Equals(tempstring,  StringComparison.InvariantCultureIgnoreCase)) Then
					exists = True
					Exit For
				End If
			Next
			If exists = True Then
				MsgBox("        Folder Already Exists")
			Else
				Dim f As New DirectoryInfo(tempstring)
				If f.Exists Then
					AuthorizeCheck = True
					clbx_MovieRoots.Items.Add(tempstring, True)
					clbx_MovieRoots.Refresh()
					moviefolderschanged = True
					AuthorizeCheck = False
					tbMovieManualPath.Text = ""
				Else
					Dim tempint As Integer = MessageBox.Show("This folder does not appear to exist" & vbCrLf & "Are you sure you wish to add it", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					If tempint = DialogResult.Yes Then
						AuthorizeCheck = True
						clbx_MovieRoots.Items.Add(tempstring, True)
						clbx_MovieRoots.Refresh()
						moviefolderschanged = True
						AuthorizeCheck = False
						tbMovieManualPath.Text = ""
					End If
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_addmoviefolderdialogue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addmoviefolderdialogue.Click
		Try
			Dim allok As Boolean = True
			Dim thefoldernames As String
			fb.Description = "Please Select Folder to Add to DB (Subfolders will also be added)"
			fb.ShowNewFolderButton = True
			fb.RootFolder = System.Environment.SpecialFolder.Desktop
			fb.SelectedPath = Pref.lastpath
			Tmr.Start()
			If fb.ShowDialog = Windows.Forms.DialogResult.OK Then
				thefoldernames = (fb.SelectedPath)
				Pref.lastpath = thefoldernames
				If allok = True Then
					AuthorizeCheck = True
					clbx_MovieRoots.Items.Add(thefoldernames, True)
					clbx_MovieRoots.Refresh()
					moviefolderschanged = True
					AuthorizeCheck = False
				Else
					MsgBox("        Folder Already Exists")
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub clbx_MovieRoots_DragDrop(sender As Object, e As DragEventArgs) Handles clbx_MovieRoots.DragDrop
		Dim folders() As String
		droppedItems.Clear()
		folders = e.Data.GetData(DataFormats.FileDrop)
		For f = 0 To UBound(folders)
			Dim exists As Boolean = False
			For Each rtpath In Pref.movieFolders
				If rtpath.rpath = folders(f) Then
					exists = True
					Exit For
				End If
			Next
			If exists Then Continue For
			If clbx_MovieRoots.Items.Contains(folders(f)) Then Continue For
			Dim skip As Boolean = False
			For Each item In droppedItems
				If item = folders(f) Then
					skip = True
					Exit For
				End If
			Next
			If Not skip Then droppedItems.Add(folders(f))
		Next
		If droppedItems.Count < 1 Then Exit Sub
		AuthorizeCheck = True
		For Each item In droppedItems
			clbx_MovieRoots.Items.Add(item, True)
			moviefolderschanged = True
		Next
		AuthorizeCheck = False
		clbx_MovieRoots.Refresh()
	End Sub

	Private Sub clbx_MovieRoots_DragEnter(sender As Object, e As DragEventArgs) Handles clbx_MovieRoots.DragEnter
		Try
			e.Effect = DragDropEffects.Copy
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub clbx_MovieRoots_KeyPress(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles clbx_MovieRoots.KeyDown
		If e.KeyCode = Keys.Delete AndAlso clbx_MovieRoots.SelectedItem <> Nothing
			Call btn_removemoviefolder.PerformClick()
		ElseIf e.KeyCode = Keys.Space Then
			AuthorizeCheck = True
			Call clbx_movierootstoggle()
			AuthorizeCheck = False
		End If
	End Sub

	Private Sub clbx_MovieRoots_MouseDown(sender As Object, e As MouseEventArgs) Handles clbx_MovieRoots.MouseDown
		Dim loc As Point = Me.clbx_MovieRoots.PointToClient(Cursor.Position)
		For i As Integer = 0 To Me.clbx_MovieRoots.Items.Count - 1
			Dim rec As Rectangle = Me.clbx_MovieRoots.GetItemRectangle(i)
			rec.Width = 16
			'checkbox itself has a default width of about 16 pixels
			If rec.Contains(loc) Then
				AuthorizeCheck = True
				Dim newValue As Boolean = Not Me.clbx_MovieRoots.GetItemChecked(i)
				Me.clbx_MovieRoots.SetItemChecked(i, newValue)
				AuthorizeCheck = False
				Return
			End If
		Next
	End Sub

	Private Sub clbx_MovieRoots_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbx_MovieRoots.ItemCheck
		If Not AuthorizeCheck Then
			e.NewValue = e.CurrentValue
			Exit Sub
		End If
		Static Updating As Boolean
		If Updating Then Exit Sub
		moviefolderschanged = True
		Updating = False
	End Sub

	Private Sub clbx_movierootstoggle()
		Dim i = clbx_MovieRoots.SelectedIndex
		clbx_MovieRoots.SetItemCheckState(i, If(clbx_MovieRoots.GetItemCheckState(i) = CheckState.Checked, CheckState.Unchecked, CheckState.Checked))
	End Sub

	Private Sub btn_removemoviefolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_removemoviefolder.Click
		Try
			While clbx_MovieRoots.SelectedItems.Count > 0
				clbx_MovieRoots.Items.Remove(clbx_MovieRoots.SelectedItems(0))
				moviefolderschanged = True
			End While
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	'Offline Movie Folders
	Private Sub btn_OfflineMovAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OfflineMovAdd.Click
		Try
			If lbx_MovOfflineFolders.SelectedItem <> Nothing Then
				Dim tempstring As String = tb_OfflineMovName.Text
				tempstring = tempstring.Replace("?", "")
				tempstring = tempstring.Replace("/", "")
				tempstring = tempstring.Replace("\", "")
				tempstring = tempstring.Replace("<", "")
				tempstring = tempstring.Replace(">", "")
				tempstring = tempstring.Replace(":", "")
				tempstring = tempstring.Replace("""", "")
				tempstring = tempstring.Replace("*", "")
				If tempstring.Length <> 0 Then
					Try
						Dim temppath As String = Path.Combine(lbx_MovOfflineFolders.SelectedItem, tempstring)
						Dim f As New DirectoryInfo(temppath)
						If Not f.Exists Then
							Directory.CreateDirectory(temppath)
							MsgBox("Folder Created")
						Else
							MsgBox("Folder Already Exists")
						End If
					Catch ex As Exception
						MsgBox("Unable to create folder" & vbCrLf & ex.Message.ToString)
					End Try
				End If
			Else
				MsgBox("Please Select a folder from the above listbox")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_OfflineMovLoadList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OfflineMovLoadList.Click
		Try
			If lbx_MovOfflineFolders.SelectedItem <> Nothing Then
				Dim tempint As Integer = 0
				Dim textfilename As String = ""
				Dim filebrowser As New OpenFileDialog
				Dim mstrProgramFilesPath As String = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
				filebrowser.InitialDirectory = mstrProgramFilesPath
				filebrowser.Filter = "Text Files|*.txt"
				filebrowser.Title = "Select text file to load"
				If filebrowser.ShowDialog = Windows.Forms.DialogResult.OK Then
					textfilename = filebrowser.FileName
				End If
				If textfilename <> "" Then
					Using textfile As IO.StreamReader = File.OpenText(textfilename) 'New StreamReader(textfilename)
						Dim line As String
						line = textfile.ReadLine
						Do While (Not line Is Nothing)
							' Add this line to list.
							Dim tempstring As String = line
							tempstring = tempstring.Replace("?", "")
							tempstring = tempstring.Replace("/", "")
							tempstring = tempstring.Replace("\", "")
							tempstring = tempstring.Replace("<", "")
							tempstring = tempstring.Replace(">", "")
							tempstring = tempstring.Replace(":", "")
							tempstring = tempstring.Replace("""", "")
							tempstring = tempstring.Replace("*", "")
							If tempstring.Length <> 0 Then
								Try
									Dim temppath As String = Path.Combine(lbx_MovOfflineFolders.SelectedItem, tempstring)
									Dim f As New DirectoryInfo(temppath)
									If Not f.Exists Then
										tempint += 1
										Directory.CreateDirectory(temppath)
									Else

									End If
								Catch ex As Exception
#If SilentErrorScream Then
                                Throw ex
#End If
								End Try
							End If
							line = textfile.ReadLine
						Loop
					End Using
					MsgBox(tempint.ToString & " Movie Folders added")
				End If
			Else
				MsgBox("Please Select a folder from the above listbox")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub btn_OfflineMovFolderAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OfflineMovFolderAdd.Click
		Try
			'add offline movie folder browser
			Dim allok As Boolean = True
			Dim thefoldernames As String
			fb.Description = "Please Select a Root Offline DVD Folder to Add to DB"
			fb.ShowNewFolderButton = True
			fb.RootFolder = System.Environment.SpecialFolder.Desktop
			fb.SelectedPath = Pref.lastpath
			Tmr.Start()
			If fb.ShowDialog = Windows.Forms.DialogResult.OK Then
				thefoldernames = (fb.SelectedPath)
				Pref.lastpath = thefoldernames
				For Each item As Object In lbx_MovOfflineFolders.Items
					If thefoldernames.ToString = item.ToString Then allok = False
				Next

				If allok = True Then
					lbx_MovOfflineFolders.Items.Add(thefoldernames)
					lbx_MovOfflineFolders.Refresh()
				Else
					MsgBox("        Folder Already Exists")
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_OfflineMovFolderRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OfflineMovFolderRemove.Click
		Try
			'remove selected offline movie folders
			While lbx_MovOfflineFolders.SelectedItems.Count > 0
				lbx_MovOfflineFolders.Items.Remove(lbx_MovOfflineFolders.SelectedItems(0))
			End While
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ButtonSaveAndQuickRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSaveAndQuickRefresh.Click
		Dim folderstoadd As New List(Of String)
		Dim offlinefolderstoadd As New List(Of String)
		Dim folderstoremove As New List(Of String)
		Dim offlinefolderstoremove As New List(Of String)
		For Each item In clbx_MovieRoots.Items
			Dim add As Boolean = True
			For Each folder In movieFolders
				If folder.rpath = item Then add = False
			Next
			If add = True Then folderstoadd.Add(item)
		Next
		For Each item In lbx_MovOfflineFolders.Items
			Dim add As Boolean = True
			For Each folder In Pref.offlinefolders
				If folder = item Then add = False
			Next
			If add = True Then offlinefolderstoadd.Add(item)
		Next
		For Each item In movieFolders
			Dim remove As Boolean = True
			For Each folder In clbx_MovieRoots.Items
				If folder = item.rpath Then
					remove = False
					Exit For
				End If
			Next
			If remove = True Then folderstoremove.Add(item.rpath)
		Next
		For Each item In Pref.offlinefolders
			Dim remove As Boolean = True
			For Each folder In lbx_MovOfflineFolders.Items
				If folder = item Then remove = False
			Next
			If remove = True Then offlinefolderstoremove.Add(item)
		Next
		If folderstoremove.Count > 0 Or offlinefolderstoremove.Count > 0 Then
			For Each item In folderstoremove
				For f = oMovies.MovieCache.Count - 1 To 0 Step -1
					If oMovies.MovieCache(f).fullpathandfilename.IndexOf(item) <> -1 Then
						oMovies.MovieCache.RemoveAt(f)
					End If
				Next
			Next
			For Each item In offlinefolderstoremove
				For f = oMovies.MovieCache.Count - 1 To 0 Step -1
					If oMovies.MovieCache(f).fullpathandfilename.IndexOf(item) <> -1 Then
						oMovies.MovieCache.RemoveAt(f)
					End If
				Next
			Next
			For f = movieFolders.Count - 1 To 0 Step -1
				Dim remove As Boolean = False
				For Each folder In folderstoremove
					If movieFolders(f).rpath = folder Then
						remove = True
					End If
				Next
				If remove = True Then movieFolders.RemoveAt(f)
			Next
			For f = Pref.offlinefolders.Count - 1 To 0 Step -1
				Dim remove As Boolean = False
				For Each folder In offlinefolderstoremove
					If Pref.offlinefolders(f) = folder Then
						remove = True
					End If
				Next
				If remove = True Then Pref.offlinefolders.RemoveAt(f)
			Next
		End If
		If folderstoadd.Count > 0 Or offlinefolderstoadd.Count > 0 Then
			Application.DoEvents()

			For Each folder In folderstoadd
				Dim t As New str_RootPaths
				t.rpath = folder
				movieFolders.Add(t)
			Next
			For Each folder In offlinefolderstoadd
				Pref.offlinefolders.Add(folder)
				folderstoadd.Add(folder)
			Next
			Pref.ConfigSave()
		End If
		For f = 0 To clbx_MovieRoots.Items.Count - 1
			Dim rtpath As String = clbx_MovieRoots.Items(f)
			Dim chkstate As CheckState = clbx_MovieRoots.GetItemCheckState(f)
			Dim selected As Boolean = (chkstate = CheckState.Checked)
			For Each item In Pref.movieFolders
				If item.rpath = rtpath Then
					item.selected = selected
					Exit For
				End If
			Next
		Next
		Pref.ConfigSave()
		mov_RebuildMovieCaches()
		moviefolderschanged = False
		TabControl2.SelectedIndex = 0
	End Sub

#End Region   'Controls for Movie Folders tab

#Region "Tv Browser Form"

	Private Sub TvTreeview_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TvTreeview.DoubleClick
		Try
			If TvTreeview.SelectedNode Is Nothing Then Exit Sub
			Dim tempstring2 As String
			Dim tempstring As String = ""
			Dim pathandfilename As String = TvTreeview.SelectedNode.Name
			If pathandfilename.IndexOf("tvshow.nfo") <> -1 Then Exit Sub
			If pathandfilename = "" Then Exit Sub
			If pathandfilename <> Nothing Then
				statusstrip_Enable()
				ToolStripStatusLabel2.Text = ""
				ToolStripStatusLabel2.Visible = True
				If pathandfilename.ToLower.Substring(pathandfilename.Length - 4, 4) = ".nfo" Then
					pathandfilename = pathandfilename.Substring(0, pathandfilename.Length - 4)

					Dim exists As Boolean = False
					For Each ext In Utilities.VideoExtensions
						If ext = "VIDEO_TS.IFO" Then Continue For
						tempstring2 = pathandfilename & ext

						If File.Exists(tempstring2) Then
							exists = True
							tempstring = applicationPath & "\Settings\temp.m3u"
							ToolStripStatusLabel2.Text = "Playing Movie...Creating m3u file:..." & tempstring
							Using fi As IO.StreamWriter = File.CreateText(tempstring)
							    fi.WriteLine(tempstring2)
							End Using
							ToolStripStatusLabel2.Text &= "......Launching Player."
							StartVideo(tempstring)
							Exit For
						End If
					Next
					If exists = False Then
						ToolStripStatusLabel2.Text = "Could not find file: """ & pathandfilename & """ with any supported extension"
					End If
					statusstripclear.Start()
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub TvTreeview_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles TvTreeview.MouseHover
		TvTreeview.Focus()
	End Sub

    Private Sub TvTreeview_MouseMove(sender As Object, e As MouseEventArgs) Handles TvTreeview.MouseMove
        ' check to display next aired tooltip andalso display missing episodes is enabled.
        If Not (Pref.tvDisplayNextAiredToolTip AndAlso Pref.displayMissingEpisodes) Then
            Me.toolTip1.SetToolTip(Me.TvTreeview, "")
            Exit Sub
        End If

        ' Get the node at the current mouse pointer location.
        Dim theNode As TreeNode = Me.TvTreeview.GetNodeAt(e.X, e.Y)
        
        ' Set a ToolTip only if the mouse pointer is actually paused on a node.
        If (theNode IsNot Nothing) Then
	        ' Verify that the tag property is not "null", and is a TVShow node.
	        If theNode.Tag IsNot Nothing AndAlso Typeof theNode.Tag Is Media_Companion.TvShow Then
                
                Dim nexteptoair As String = ""
                Dim found As Boolean = False
                For Each Season As Media_Companion.TvSeason In theNode.Tag.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        If Not String.IsNullOrEmpty(episode.Aired.Value) Then
                            Try
                                If Convert.ToDateTime(episode.Aired.Value) > Date.Now() Then ' Is the episode in the future?
                                    Dim epseason    As String   = episode.Season.value
                                    If epseason.Length = 1 Then epseason = "0" & epseason
                                    Dim epepisode   As String   = episode.Episode.value
                                    If epepisode.Length = 1 Then epepisode = "0" & epepisode
                                    nexteptoair = "Next Air Date: " & episode.Aired.Value & vbCrLf & "S" & epseason & "E" & epepisode & " - " & episode.Title.value
                                    found = True
                                    Exit For
                                End If
                            Catch
                            End Try
                        End If
                    Next
                    If found Then Exit For
                Next
		        ' Change the ToolTip only if the pointer moved to a new node.
		        If nexteptoair <> tooltiptv.GetToolTip(Me.TvTreeview) Then
			        tooltiptv.SetToolTip(Me.TvTreeview, nexteptoair)
		        End If
	        Else
		        tooltiptv.SetToolTip(Me.TvTreeview, "")
	        End If
        Else
	        ' Pointer is not over a node so clear the ToolTip.
	        tooltiptv.SetToolTip(Me.TvTreeview, "")
        End If
    End Sub

	Private Sub TvTreeview_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TvTreeview.MouseUp
		If e.Button = MouseButtons.Right Then
			TvTreeview.SelectedNode = TvTreeview.GetNodeAt(TvTreeview.PointToClient(Cursor.Position)) '***select actual the node 
			Tv_TreeViewContextMenuItemsEnable()
		End If
	End Sub

	Private Sub rbTvDisplayFiltering_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTvDisplayUnWatched.CheckedChanged,
																														rbTvDisplayWatched.CheckedChanged,
																														rbTvMissingAiredEp.CheckedChanged,
																														rbTvMissingEpisodes.CheckedChanged,
                                                                                                                        rbTvMissingNextToAir.CheckedChanged,
																														rbTvMissingPoster.CheckedChanged,
																														rbTvListAll.CheckedChanged,
																														rbTvMissingFanart.CheckedChanged,
																														rbTvMissingThumb.CheckedChanged,
																														rbTvListContinuing.CheckedChanged,
																														rbTvListEnded.CheckedChanged,
																														rbTvListUnKnown.CheckedChanged
        If tvfiltertrip Then
            tvfiltertrip = False
            Exit Sub
        End If
		Try
			Call tv_Filter()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_EpWatched_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_EpWatched.Click
		Try
			Tv_MarkAs_Watched_UnWatched("3")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub btn_TvRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvRescrape.Click
		Try
			tv_Rescrape()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tb_EpPlot_DoubleClick(sender As System.Object, e As System.EventArgs) Handles tb_EpPlot.DoubleClick
		ShowBigTvEpisodeText()
	End Sub

	Private Sub tb_EpPlot_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_EpPlot.KeyPress
		If e.KeyChar = Convert.ToChar(1) Then
			DirectCast(sender, TextBox).SelectAll()
			e.Handled = True
		End If
	End Sub

	Private Sub tb_ShPlot_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_ShPlot.KeyPress
		If e.KeyChar = Convert.ToChar(1) Then
			DirectCast(sender, TextBox).SelectAll()
			e.Handled = True
		End If
	End Sub

	Private Sub ShowBigTvEpisodeText()

		Dim frm As New frmBigTvEpisodeText
		If Pref.MultiMonitoEnabled Then
			frm.Bounds = screen.AllScreens(CurrentScreen).Bounds
			frm.StartPosition = FormStartPosition.Manual
		End If
		frm.ShowDialog(
							 tb_Sh_Ep_Title.Text,
							 tb_EpDirector.Text,
							 tb_EpAired.Text,
							 tb_EpRating.Text,
							 tb_EpVotes.Text,
							 tb_ShRunTime.Text,
							 tb_ShGenre.Text,
							 tb_ShCert.Text,
							 tb_EpPlot.Text
							 )
	End Sub

	Private Sub tb_Sh_Ep_Title_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_Sh_Ep_Title.Enter
		If Panel_EpisodeInfo.Visible Then
			tb_Sh_Ep_Title.Text = ep_SelectedCurrently(TvTreeview).Title.Value
		Else
			tb_Sh_Ep_Title.Text = tv_ShowSelectedCurrently(TvTreeview).Title.Value
		End If
	End Sub

	Private Sub btn_SaveTvShowOrEpisode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SaveTvShowOrEpisode.Click
		SaveShowOrEpisode()
	End Sub          'save button

	Private Sub btn_TvShSortOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvShSortOrder.Click
		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Dim TVShowNFOContent As String = ""
		If btn_TvShSortOrder.Text = "Default" Then
			WorkingTvShow.SortOrder.Value = "dvd"
			btn_TvShSortOrder.Text = "DVD"
		Else
			WorkingTvShow.SortOrder.Value = "default"
			btn_TvShSortOrder.Text = "Default"
		End If
		nfoFunction.tvshow_NfoSave(WorkingTvShow, True)
		tv_ShowLoad(WorkingTvShow)
	End Sub

	Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)

			Dim TVShowNFOContent As String = ""
			If Button45.Text = "TVDB" Then
				If WorkingTvShow.ImdbId.Value <> "" Then
					WorkingTvShow.TvShowActorSource.Value = "imdb"
					Button45.Text = "IMDB"
				Else
					MsgBox("No IMDB ID allocated to this Show!")
				End If
			Else
				WorkingTvShow.TvShowActorSource.Value = "tvdb"
				Button45.Text = "TVDB"
			End If
			nfoFunction.tvshow_NfoSave(WorkingTvShow, True)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim TVShowNFOContent As String = ""
			If Button46.Text = "TVDB" Then
				WorkingTvShow.EpisodeActorSource.Value = "imdb"
				Button46.Text = "IMDB"
			Else
				WorkingTvShow.EpisodeActorSource.Value = "tvdb"
				Button46.Text = "TVDB"
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub cbTvActor_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTvActor.MouseHover
		Try
			cbTvActor.Focus()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub cbTvActor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTvActor.SelectedIndexChanged
		Try
			If Not actorflag Then
				actorflag = True
				cbTvActorRole.SelectedIndex = cbTvActor.SelectedIndex
				Call tv_ActorDisplay()
			Else
				actorflag = False
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub cbTvActorRole_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTvActorRole.MouseHover
		Try
			cbTvActorRole.Focus()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub cbTvActorRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbTvActorRole.SelectedIndexChanged
		Try
			If Not actorflag Then
				actorflag = True
				cbTvActor.SelectedIndex = cbTvActorRole.SelectedIndex
				Call tv_ActorRoleDisplay()
			Else
				actorflag = False
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub PictureBox6_DoubleClick(sender As System.Object, e As System.EventArgs) Handles PictureBox6.DoubleClick
		ZoomActorPictureBox(PictureBox6)
	End Sub

	Private Sub btn_TvShState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvShState.Click
		Try
			Dim Btn As Button = sender
			If TypeOf Btn.Tag Is Media_Companion.TvShow Then
				Dim TempShow As Media_Companion.TvShow = tv_ShowSelectedCurrently(TvTreeview) 'Btn.Tag
				Select Case TempShow.State
					Case Media_Companion.ShowState.Locked
						TempShow.State = Media_Companion.ShowState.Open
					Case Media_Companion.ShowState.Open
						TempShow.State = Media_Companion.ShowState.Locked
					Case Media_Companion.ShowState.Error
					Case Media_Companion.ShowState.Unverified
						TempShow.State = Media_Companion.ShowState.Open
						tb_Sh_Ep_Title.BackColor = Color.White
				End Select
				If TempShow.State = Media_Companion.ShowState.Locked Then
					btn_TvShState.Text = "Locked"
					btn_TvShState.BackColor = Color.Red
				ElseIf TempShow.State = Media_Companion.ShowState.Open Then
					btn_TvShState.Text = "Open"
					btn_TvShState.BackColor = Color.LawnGreen
				ElseIf TempShow.State = Media_Companion.ShowState.Unverified Then
					btn_TvShState.Text = "Un-Verified"
					btn_TvShState.BackColor = Color.Yellow
				Else
					btn_TvShState.Text = "Error"
					btn_TvShState.BackColor = Color.Gray
				End If
				TempShow.UpdateTreenode()   'update the treenode so we can see the state change
				nfoFunction.tvshow_NfoSave(TempShow, True)  'save the nfo immediately (you don't have to press save button)
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tb_airepisode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_EpAirEpisode.KeyPress
		If Char.IsNumber(e.KeyChar) = False And e.KeyChar <> Chr(8) Then
			e.Handled = True
		End If
	End Sub

	Private Sub tb_airseason_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_EpAirSeason.KeyPress
		If Char.IsNumber(e.KeyChar) = False And e.KeyChar <> Chr(8) Then
			e.Handled = True
		End If
	End Sub

	Private Sub btnTvSearchNew_Click(sender As System.Object, e As System.EventArgs) Handles btnTvSearchNew.Click
		Try
			If btnTvSearchNew.Text = "Cancel  " Then
                BckWrkTv.CancelAsync()
				bckgroundscanepisodes.CancelAsync()
				Exit Sub
			End If
			Call ep_Search()
		Catch ex As Exception

		End Try
	End Sub

	Private Sub btnTvRefreshAll_Click(sender As System.Object, e As System.EventArgs) Handles btnTvRefreshAll.Click
		Try
			Call tv_CacheRefresh()
		Catch ex As Exception

		End Try
	End Sub

    Private Sub tb_EpImdbId_MouseDown(sender As Object, e As MouseEventArgs) Handles tb_EpImdbId.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If Utilities.ValidImdbId(tb_EpImdbId.Text) Then
                Dim m As MsgBoxResult = MsgBox("This episode has a valid IMDb Id" & vbCrLf & "Are you sure you wish to change this?", MsgBoxStyle.YesNo)
                If m = MsgBoxResult.No OrElse m = MsgBoxResult.Cancel Then Exit Sub
            End If

            Dim newImdbId As String = InputBox("Enter IMDB Id for Episdoe: ", "Manual IMDBId")

            If Utilities.ValidImdbId(newImdbId) Then
                tb_EpImdbId.Text = newImdbId
                SaveShowOrEpisode(True)
            End If
        End If
    End Sub
    
    Private Sub cmbx_EpUsrRating_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbx_EpUsrRating.SelectedValueChanged, cmbx_shUserRating.SelectedValueChanged
        If ShOrEploading Then Exit Sub
        btn_SaveTvShowOrEpisode.PerformClick()
    End Sub
    

#Region "Tv PictureBoxes"
	Private Sub ReScrFanartToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ReScrFanartToolStripMenuItem.Click
		Try
			Dim Showname As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			TvGetArtwork(Showname, True, False, False, False, False)
			tv_ShowLoad(Showname)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub SelNewFanartToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SelNewFanartToolStripMenuItem.Click
		Try
			Me.tpTvFanart.Select()
			TabControl3.SelectedIndex = 1

		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RescrapeTvEpThumbToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RescrapeTvEpThumbToolStripMenuItem.Click
		Try
			TvEpThumbRescrape()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub RescrapeTvEpScreenShotToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles RescrapeTvEpScreenShotToolStripMenuItem.Click
		Try
			TvEpThumbScreenShot()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tv_PictureBoxLeft_Click(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles tv_PictureBoxLeft.MouseUp, tv_PictureBoxLeft.Click
		Try
			If e.Button = Windows.Forms.MouseButtons.Right Then
				ReScrFanartToolStripMenuItem.Visible = False
				SelNewFanartToolStripMenuItem.Visible = False
				RescrapeTvEpThumbToolStripMenuItem.Visible = False
				RescrapeTvEpScreenShotToolStripMenuItem.Visible = False
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Or TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
					ReScrFanartToolStripMenuItem.Visible = True
					SelNewFanartToolStripMenuItem.Visible = True
				ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
					RescrapeTvEpThumbToolStripMenuItem.Visible = True
					RescrapeTvEpScreenShotToolStripMenuItem.Visible = True
				Else
					Exit Sub
				End If
			End If
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tv_PictureBoxRight_Click(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles tv_PictureBoxRight.MouseUp, tv_PictureBoxRight.Click
		Try
			If e.Button = Windows.Forms.MouseButtons.Right Then
				tsm_TvScrapeBanner.Visible = False
				tsm_TvScrapePoster.Visible = False
				tsm_TvSelectBanner.Visible = False
				tsm_TvSelectPoster.Visible = False
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Or TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Or TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
					tsm_TvScrapePoster.Visible = True
					tsm_TvSelectPoster.Visible = True
				Else
					Exit Sub
				End If
			End If
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tv_PictureBoxBottom_Click(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles tv_PictureBoxBottom.MouseUp, tv_PictureBoxBottom.Click
		Try
			If e.Button = Windows.Forms.MouseButtons.Right Then
				tsm_TvScrapeBanner.Visible = False
				tsm_TvScrapePoster.Visible = False
				tsm_TvSelectBanner.Visible = False
				tsm_TvSelectPoster.Visible = False
				If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Or TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Or TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
					tsm_TvScrapeBanner.Visible = True
					tsm_TvSelectBanner.Visible = True
				Else
					Exit Sub
				End If
			End If
		Catch ex As Exception

		End Try

	End Sub

	Private Sub _tv_SplitContainer_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles _tv_SplitContainer.SplitterMoved
		If Not MainFormLoadedStatus Then Exit Sub
        If ProgState = ProgramState.Closing Then Exit Sub
		Pref.tvbannersplit = Math.Round(_tv_SplitContainer.SplitterDistance / _tv_SplitContainer.Height, 2)
	End Sub

#End Region 'Tv PictureBoxes    

#End Region  'Tv Browser Form functions

#Region "Tv Screenshot Form"
    
	Private Sub TextBox35_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox35.KeyPress
		If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
			If TextBox35.Text <> "" AndAlso Convert.ToInt32(TextBox35.Text) > 0 Then
				tv_EpThumbScreenShot.PerformClick()
			End If
		End If
		If Char.IsNumber(e.KeyChar) = False And e.KeyChar <> Chr(8) Then
			e.Handled = True
		End If
	End Sub

	Private Sub tv_EpThumbScreenShot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tv_EpThumbScreenShot.Click
		Dim Cachename As String = epGetThumbScreenShot()
		If Cachename = "" Then
			MsgBox("Unable to get screenshots from episode file")
			Exit Sub
		End If
		Dim matches() As Control
		For i = 0 To 4
			matches = Me.Controls.Find("pbEpScrSht" & i, True)
			If matches.Length > 0 Then
				Dim pb As PictureBox = DirectCast(matches(0), PictureBox)
				pb.SizeMode = PictureBoxSizeMode.StretchImage
				Dim image2load As String = Cachename.Substring(0, Cachename.Length - 5) & i.ToString & ".jpg"
				util_ImageLoad(pb, image2load, Utilities.DefaultTvFanartPath)
			End If
		Next
		If Not IsNothing(pbEpScrSht0.Tag) Then util_ImageLoad(pbTvEpScrnShot, pbEpScrSht0.Tag.ToString, Utilities.DefaultTvFanartPath)
	End Sub

    Private Sub pbepscrsht_Clear()
        Dim matches() As Control
        For i = 0 To 4
			matches = Me.Controls.Find("pbEpScrSht" & i, True)
			If matches.Length > 0 Then
				Dim pb As PictureBox = DirectCast(matches(0), PictureBox)
				pb.Image = Nothing
                pb.Tag = Nothing
			End If
		Next
    End Sub

	Private Sub TextBox35_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox35.Leave
		If TextBox35.Text = "" Then
			MsgBox("Please enter a numerical value >0 into the textbox")
			TextBox35.Focus()
		ElseIf Convert.ToInt32(TextBox35.Text) = 0 Then
			MsgBox("Please enter a numerical value >0 into the textbox")
			TextBox35.Focus()
		End If
	End Sub

	Private Sub tv_EpThumbRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tv_EpThumbRescrape.Click
		Try
			TvEpThumbRescrape()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tv_EpThumbScreenShotSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tv_EpThumbScreenShotSave.Click
		Try
			Dim aok As Boolean = False
			Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
			Dim paths As New List(Of String)
			If Pref.EdenEnabled Then paths.Add(WorkingEpisode.NfoFilePath.Replace(".nfo", ".tbn"))
			If Pref.FrodoEnabled Then paths.Add(WorkingEpisode.NfoFilePath.Replace(".nfo", "-thumb.jpg"))
			Dim cachepathandfilename As String = pbTvEpScrnShot.Tag.ToString
			aok = True
			Dim imagearr() As Integer = GetAspect(WorkingEpisode)
			If Pref.tvscrnshtTVDBResize AndAlso Not imagearr(0) = 0 Then
				DownloadCache.CopyAndDownSizeImage(cachepathandfilename, paths(0), imagearr(0), imagearr(1))
			Else
				File.Copy(cachepathandfilename, paths(0), True)
			End If

			If paths.Count > 1 Then File.Copy(paths(0), paths(1), True)

			If File.Exists(paths(0)) Then
				util_ImageLoad(pbTvEpScrnShot, paths(0), Utilities.DefaultTvFanartPath)
				util_ImageLoad(tv_PictureBoxLeft, paths(0), Utilities.DefaultTvFanartPath)
				Dim Rating As String = tb_EpRating.Text  'WorkingEpisode.Rating.Value
				If TestForMultiepisode(WorkingEpisode.NfoFilePath) Then
					Dim episodelist As New List(Of TvEpisode)
					episodelist = WorkingWithNfoFiles.ep_NfoLoad(WorkingEpisode.NfoFilePath)
					For Each Ep In episodelist
						If Ep.Season.Value = WorkingEpisode.Season.Value And Ep.Episode.Value = WorkingEpisode.Episode.Value Then
							Dim video_flags = GetMultiEpMediaFlags(Ep)
							movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, Rating, video_flags)
						End If
					Next
				Else
					Dim video_flags = GetEpMediaFlags()
					movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, Rating, video_flags)
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub
	
	Private Sub pbepscrsht_click(ByVal sender As Object, ByVal e As EventArgs)
		Dim pb As PictureBox = sender
		If IsNothing(pb.Image) OrElse IsNothing(pb.Tag) Then Exit Sub
		util_ImageLoad(pbTvEpScrnShot, pb.Tag, Utilities.DefaultTvFanartPath)
	End Sub

	Private Function epGetThumbScreenShot() As String
		Try
			Dim aok As Boolean = False
			Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
			If WorkingEpisode.IsMissing Then Return ""
			If TextBox35.Text = "" Then TextBox35.Text = Pref.ScrShtDelay
			If IsNumeric(TextBox35.Text) Then
				Dim paths As New List(Of String)
				If Pref.EdenEnabled Then paths.Add(WorkingEpisode.NfoFilePath.Replace(".nfo", ".tbn"))
				If Pref.FrodoEnabled Then paths.Add(WorkingEpisode.NfoFilePath.Replace(".nfo", "-thumb.jpg"))
				Dim tempstring2 As String = WorkingEpisode.VideoFilePath
				If File.Exists(tempstring2) Then
					Dim seconds As Integer = Pref.ScrShtDelay
					If Convert.ToInt32(TextBox35.Text) > 0 Then
						seconds = Convert.ToInt32(TextBox35.Text)
					End If
					System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
					Application.DoEvents()
					Dim cachepathandfilename As String = Utilities.CreateScrnShotToCache(tempstring2, paths(0), seconds, 5, 1)
					If cachepathandfilename <> "" Then
						Return cachepathandfilename
					End If
				End If
			End If
		Catch
		End Try
		Return ""
	End Function
#End Region

#Region "Tv Fanart Form"

	Private Sub tpTvFanart_Leave(sender As Object, e As EventArgs) Handles tpTvFanart.Leave
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
	End Sub

    Private Sub tv_Fanart_Load()
		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		Me.Panel13.Controls.Clear()
		listOfTvFanarts.Clear()
		btnTvFanartResetImage.Visible = False
		btnTvFanartSaveCropped.Visible = False
		If TvTreeview.SelectedNode.Name.ToLower.IndexOf("tvshow.nfo") <> -1 Or TvTreeview.SelectedNode.Name = "" Then
			If Not tv_PictureBoxLeft.Image Is Nothing Then
				util_ImageLoad(PictureBox10, WorkingTvShow.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)
			Else
				PictureBox10.Image = Nothing
			End If
		Else
			util_ImageLoad(PictureBox10, WorkingTvShow.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)
		End If
		Try
			Label58.Text = PictureBox10.Image.Height.ToString
			Label59.Text = PictureBox10.Image.Width.ToString
		Catch ex As Exception
		End Try
		TextBox28.Text = WorkingTvShow.Title.Value
		messbox = New frmMessageBox("Please wait,", "", "Querying TVDB for fanart list")
		System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
		messbox.Show()
		Me.Refresh()
		messbox.Refresh()
		Dim fanarturl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & WorkingTvShow.TvdbId.Value & "/banners.xml"
		Dim apple2(4000) As String
		Dim fanartlinecount As Integer = 0
		Dim tmplistOfTvFanarts As New List(Of str_FanartList)
		Try
			Dim wrGETURL As WebRequest
			wrGETURL = WebRequest.Create(fanarturl)
			wrGETURL.Proxy = Utilities.MyProxy
			Dim objStream As IO.Stream
			objStream = wrGETURL.GetResponse.GetResponseStream()
			Dim objReader As New IO.StreamReader(objStream)
			Dim sLine As String = ""
			fanartlinecount = 0
			sLine = objReader.ReadToEnd
			Dim bannerslist As New XmlDocument
			Dim bannerlist As String = "<banners>"
			bannerslist.LoadXml(sLine)
			objReader.Close()
			objStream.Close()
			objReader = Nothing
			objStream = Nothing
			For Each thisresult As XmlNode In bannerslist("Banners")
				Select Case thisresult.Name
					Case "Banner"
						Dim fanart As New str_FanartList(SetDefaults)
						For Each bannerselection As XmlNode In thisresult.ChildNodes
							Select Case bannerselection.Name
								Case "BannerPath"
									fanart.bigUrl = "http://thetvdb.com/banners/" & bannerselection.InnerXml
									fanart.smallUrl = "http://thetvdb.com/banners/_cache/" & bannerselection.InnerXml
								Case "BannerType"
									fanart.type = bannerselection.InnerXml
								Case "BannerType2"
									fanart.resolution = bannerselection.InnerXml
								Case "Rating"
									fanart.rating = bannerselection.InnerXml.ToRating
							End Select
						Next
						If fanart.type = "fanart" Then
							tmplistOfTvFanarts.Add(fanart)
							'listOfTvFanarts.Add(fanart)
						End If
				End Select
			Next
		Catch ex As WebException
			Dim webmsg As String = ex.Message
			MsgBox("TVDB appears to be down at the moment, please try again later")
		End Try
		If tmplistOfTvFanarts.Count > 1 Then
			Dim q = From x In tmplistOfTvFanarts Order By x.rating Descending
			listOfTvFanarts.AddRange(q.ToList)
		End If

		If listOfTvFanarts.Count > 0 Then
			Dim MovFanartPicBox As New List(Of FanartPicBox)
			Dim location As Integer = 0
			Dim itemcounter As Integer = 0
			For Each item In listOfTvFanarts
				Dim thispicbox As New FanartPicBox
				tvFanartBoxes() = New PictureBox()
				With tvFanartBoxes
					.Location = New Point(0, location)
					If listOfTvFanarts.Count > 2 Then
						.Width = 400
						.Height = 225
					Else
						.Width = 415
						.Height = 225
					End If
					.SizeMode = PictureBoxSizeMode.Zoom
					.Visible = True
					.BorderStyle = BorderStyle.Fixed3D
					.Name = "tvfanart" & itemcounter.ToString
					AddHandler tvFanartBoxes.DoubleClick, AddressOf util_ZoomImage2
				End With
				thispicbox.pbox = tvFanartBoxes
				thispicbox.imagepath = item.smallUrl
				MovFanartPicBox.Add(thispicbox)
				Application.DoEvents()

				tvFanartCheckBoxes() = New RadioButton()
				With tvFanartCheckBoxes
					.BringToFront()
					.Location = New Point(199, location + 225)
					.Name = "checkbox" & itemcounter.ToString
				End With

				resolutionLabels() = New Label
				With resolutionLabels
					.BringToFront()
					.Location = New Point(10, location + 225)
					.Name = item.resolution
					.Text = item.resolution
				End With
				itemcounter += 1
				location += 250
				Me.Panel13.Controls.Add(tvFanartBoxes())
				Me.Panel13.Controls.Add(tvFanartCheckBoxes())
				Me.Panel13.Controls.Add(resolutionLabels())
				Application.DoEvents()
			Next
			Me.Panel13.Refresh()
			Me.Refresh()
			If MovFanartPicBox.Count > 0 Then
				messbox.Close()
				If Not ImgBw.IsBusy Then
					ToolStripStatusLabel2.Text = "Starting Download of Images..."
					ToolStripStatusLabel2.Visible = True
					ImgBw.RunWorkerAsync({MovFanartPicBox, 0, MovFanartPicBox.Count, Me.Panel13})
				End If
			End If
			Me.Panel13.Refresh()
			Me.Refresh()
			EnableTvFanartScrolling
		Else
			Dim mainlabel2 As Label
			mainlabel2 = New Label
			With mainlabel2
				.Location = New Point(0, 100)
				.Width = 700
				.Height = 100
				.Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
				.Text = "No Fanart Was Found At TVDB For This Movie"
			End With
			Me.Panel13.Controls.Add(mainlabel2)
		End If
		System.Windows.Forms.Cursor.Current = Cursors.Default
		messbox.Close()
	End Sub

	'Set focus on the first checkbox to enable mouse wheel scrolling 
	Sub EnableTvFanartScrolling
		Try
			Dim rb As RadioButton = Panel13.Controls("checkbox0")

			rb.Select                       'Causes RadioButtons checked state to toggle
			rb.Checked = Not rb.Checked     'Undo unwanted checked state toggling
		Catch
		End Try
	End Sub

	Sub EnableTvBannerScrolling
		Try
			Panel16.Focus()
		Catch
		End Try
	End Sub

	Private Sub Tv_FanartDisplay()
		Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
		If IsNothing(WorkingTvShow) Then Exit Sub
		If TvTreeview.SelectedNode.Name.ToLower.IndexOf("tvshow.nfo") <> -1 Or TvTreeview.SelectedNode.Name = "" Then
			If Not tv_PictureBoxLeft.Image Is Nothing Then
				util_ImageLoad(PictureBox10, WorkingTvShow.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)
			Else
				PictureBox10.Image = Nothing
			End If
		Else
			util_ImageLoad(PictureBox10, WorkingTvShow.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)
		End If
	End Sub

	Private Sub tv_FanartCropTop()
		Dim imagewidth As Integer = PictureBox10.Image.Width
		Dim imageheight As Integer = PictureBox10.Image.Height
		PictureBox10.Image = util_ImageCrop(PictureBox10.Image, New Size(imagewidth, imageheight - 1), New Point(0, 1)).Clone
		PictureBox10.SizeMode = PictureBoxSizeMode.Zoom
	End Sub

	Private Sub tv_FanartCropBottom()
		Dim imagewidth As Integer = PictureBox10.Image.Width
		Dim imageheight As Integer = PictureBox10.Image.Height
		PictureBox10.Image = util_ImageCrop(PictureBox10.Image, New Size(imagewidth, imageheight - 1), New Point(0, 0)).Clone
		PictureBox10.SizeMode = PictureBoxSizeMode.Zoom
	End Sub

	Private Sub tv_FanartCropLeft()
		Dim imagewidth As Integer = PictureBox10.Image.Width
		Dim imageheight As Integer = PictureBox10.Image.Height
		PictureBox10.Image = util_ImageCrop(PictureBox10.Image, New Size(imagewidth - 1, imageheight), New Point(1, 0)).Clone
		PictureBox10.SizeMode = PictureBoxSizeMode.Zoom
	End Sub

	Private Sub tv_FanartCropRight()
		Dim imagewidth As Integer = PictureBox10.Image.Width
		Dim imageheight As Integer = PictureBox10.Image.Height
		PictureBox10.Image = util_ImageCrop(PictureBox10.Image, New Size(imagewidth - 1, imageheight), New Point(0, 0)).Clone
		PictureBox10.SizeMode = PictureBoxSizeMode.Zoom
	End Sub


	Private Sub btnTvFanartSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvFanartSave.Click
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		Dim issavefanart As Boolean = Pref.savefanart
		Pref.savefanart = True
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			lbl_movVotes.Text = "Please Wait, Trying to Download Fanart"
			Me.Refresh()
			Application.DoEvents()
			Dim miscvar As String = String.Empty
			Dim miscint As Integer = 0
			Dim miscvar2 As String = String.Empty
			Dim allok As Boolean = False
			For Each button As Control In Me.Panel13.Controls
				If button.Name.IndexOf("checkbox") <> -1 Then
					Dim b1 As RadioButton = CType(button, RadioButton)
					If b1.Checked = True Then
						miscvar = b1.Name
						miscvar = miscvar.Replace("checkbox", "")
						miscint = Convert.ToDecimal(miscvar)
						miscvar2 = listOfTvFanarts(miscint).bigUrl
						allok = True
						Exit For
					End If
				End If
			Next
			If allok = False Then
				MsgBox("No Fanart Is Selected")
			Else
				Try
					Dim savepath As String = WorkingTvShow.NfoFilePath.ToLower.Replace("tvshow.nfo", "fanart.jpg")

					If Movie.SaveFanartImageToCacheAndPath(miscvar2, savepath) Then
						Try
							util_ImageLoad(PictureBox10, savepath, Utilities.DefaultTvFanartPath)
							If TvTreeview.SelectedNode.Name.ToLower.IndexOf("tvshow.nfo") <> -1 Or TvTreeview.SelectedNode.Name = "" Then
								util_ImageLoad(tv_PictureBoxLeft, savepath, Utilities.DefaultTvFanartPath)
							End If
						Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
						End Try
					Else
						PictureBox10.Image = Nothing
					End If
					If Pref.FrodoEnabled Then
						Utilities.SafeCopyFile(savepath, savepath.Replace("fanart.jpg", "season-all-fanart.jpg"), True)
					End If
				Catch ex As WebException
					MsgBox(ex.Message)
				End Try
			End If
			Pref.savefanart = issavefanart
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
			Pref.savefanart = issavefanart
		End Try
	End Sub

	Private Sub rbTvFanart_CheckedChanged(sender As System.Object, e As System.EventArgs)
		Tv_FanartDisplay()
	End Sub

	Private Sub Button35_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button35.MouseDown
		Try
			'up
			If PictureBox10.Image Is Nothing Then Exit Sub
			btnTvFanartResetImage.Visible = True
			btnTvFanartSaveCropped.Visible = True
			cropString = "top"
			Timer4.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Button36_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button36.MouseDown
		Try
			'down
			If PictureBox10.Image Is Nothing Then Exit Sub
			btnTvFanartResetImage.Visible = True
			btnTvFanartSaveCropped.Visible = True
			cropString = "bottom"
			Timer4.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Button38_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button38.MouseDown
		Try
			If PictureBox10.Image Is Nothing Then Exit Sub
			'thumbedItsMade = True
			btnTvFanartResetImage.Visible = True
			btnTvFanartSaveCropped.Visible = True
			cropString = "left"
			Timer4.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try 'left
	End Sub

	Private Sub Button37_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button37.MouseDown
		Try
			'right
			If PictureBox10.Image Is Nothing Then Exit Sub
			btnTvFanartResetImage.Visible = True
			btnTvFanartSaveCropped.Visible = True
			cropString = "right"
			Timer4.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub Timer4_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer4.Tick
		Try
			If cropString = "top" Then Call tv_FanartCropTop()
			If cropString = "bottom" Then Call tv_FanartCropBottom()
			If cropString = "left" Then Call tv_FanartCropLeft()
			If cropString = "right" Then Call tv_FanartCropRight()
			Label58.Text = PictureBox10.Image.Height.ToString
			Label59.Text = PictureBox10.Image.Width.ToString
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub Button35_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button35.MouseUp
		Try
			Timer4.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Button36_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button36.MouseUp
		Try
			Timer4.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Button38_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button38.MouseUp
		Try
			Timer4.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub Button37_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button37.MouseUp
		Try
			Timer4.Enabled = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvFanartResetImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvFanartResetImage.Click
		Try
			Dim workingtvshow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			util_ImageLoad(PictureBox10, workingtvshow.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)
			Label58.Text = PictureBox10.Image.Height.ToString
			Label59.Text = PictureBox10.Image.Width.ToString
			btnTvFanartResetImage.Visible = False
			btnTvFanartSaveCropped.Visible = False
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvFanartSaveCropped_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvFanartSaveCropped.Click
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Try
				PictureBox10.Image.Save(WorkingTvShow.FolderPath & "fanart.jpg", System.Drawing.Imaging.ImageFormat.Jpeg)
				If TvTreeview.SelectedNode.Name.ToLower.IndexOf("tvshow.nfo") <> -1 Or TvTreeview.SelectedNode.Name = "" Then
					util_ImageLoad(tv_PictureBoxLeft, WorkingTvShow.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)
				End If
				Label58.Text = PictureBox10.Image.Height.ToString
				Label59.Text = PictureBox10.Image.Width.ToString
				btnTvFanartResetImage.Visible = False
				btnTvFanartSaveCropped.Visible = False
			Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvFanartUrl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvFanartUrl.Click
		Try
			Dim t As New frmImageBrowseOrUrl
			t.Location = Me.PointToScreen(New Point(btnTvFanartUrl.Left - 480, btnTvFanartUrl.Top + btnTvFanartUrl.Height))
			t.ShowDialog()
			If t.DialogResult = Windows.Forms.DialogResult.Cancel Or t.tb_PathorUrl.Text = "" Then
				t.Dispose()
				Exit Sub
			End If
			Dim PathOrUrl As String = t.tb_PathorUrl.Text
			t.Dispose()
			t = Nothing
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			Dim savepath As String = WorkingTvShow.NfoFilePath.ToLower.Replace("tvshow.nfo", "fanart.jpg")
			Dim eh As Boolean = Pref.savefanart
			Pref.savefanart = True
			Movie.SaveFanartImageToCacheAndPath(PathOrUrl, savepath)
			Pref.savefanart = eh
			Dim exists As Boolean = File.Exists(savepath)
			If exists = True Then
				util_ImageLoad(PictureBox10, savepath, Utilities.DefaultTvFanartPath)
				If TvTreeview.SelectedNode.Name.ToLower.IndexOf("tvshow.nfo") <> -1 Or TvTreeview.SelectedNode.Name = "" Then
					util_ImageLoad(tv_PictureBoxLeft, savepath, Utilities.DefaultTvFanartPath)
				End If
			End If
			Label59.Text = PictureBox10.Image.Width
			Label58.Text = PictureBox10.Image.Height
            
		Catch ex As Exception
			MsgBox("Unable To Download Image")
		End Try

	End Sub

#End Region

#Region "Tv Poster Form"

	Private Sub tpTvPosters_Leave(sender As Object, e As EventArgs) Handles tpTvPosters.Leave
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
	End Sub

	Private Sub btnTvPosterSaveBig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterSaveBig.Click
		Call TvPosterSave(btnTvPosterSaveBig.Tag)
	End Sub

	Private Sub btnTvPosterTVDBAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterTVDBAll.Click
		Try
			'tvdb all
			tvdbmode = True
			usedlist.Clear()
			btnTvPosterSaveBig.Visible = False
			If tvdbposterlist.Count = 0 Then
				Call tv_TvdbThumbsGet()
			End If
			For Each poster In tvdbposterlist
				If rbTVbanner.Enabled = False Then
					If poster.BannerType <> "fanart" And poster.BannerType <> "series" Then
						usedlist.Add(poster)
					End If
				Else
					If rbTVposter.Checked = False And poster.BannerType = "series" Then
						usedlist.Add(poster)
					ElseIf rbTVposter.Checked = True And poster.BannerType <> "fanart" Then
						If poster.BannerType <> "series" Then usedlist.Add(poster)
					End If

				End If
			Next
			Call tv_PosterPanelPopulate()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvPosterTVDBSpecific_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterTVDBSpecific.Click
		Try
			'tvdb specific
			tvdbmode = True
			usedlist.Clear()
			btnTvPosterSaveBig.Visible = False
			If tvdbposterlist.Count = 0 Then
				Call tv_TvdbThumbsGet()
			End If

			Dim tempseason As String = ""
			If ComboBox2.SelectedItem.indexof("Season ") <> -1 Then
				tempseason = ComboBox2.SelectedItem.replace("Season ", "")
			End If
			If tempseason.IndexOf("0") = 0 And tempseason.Length > 1 Then
				tempseason = tempseason.Substring(1, tempseason.Length - 1)
			End If
			If ComboBox2.SelectedItem.indexof("Specials") <> -1 Then
				tempseason = "0"
			End If
			If ComboBox2.SelectedItem.indexof("Main Image") <> -1 And rbTVposter.Checked = True Then
				tempseason = "poster"
			ElseIf ComboBox2.SelectedItem.indexof("Main Image") <> -1 And rbTVposter.Checked = False Then
				tempseason = "series"
			End If
			If tempseason = "poster" Or tempseason = "series" Then
				For Each poster In tvdbposterlist
					If poster.BannerType = "poster" And rbTVposter.Checked = True Then
						If poster.BannerType <> "fanart" Then usedlist.Add(poster)
					End If
					If poster.BannerType = "series" And rbTVposter.Checked = False Then
						If poster.BannerType <> "fanart" Then usedlist.Add(poster)
					End If
				Next
			Else
				For Each poster In tvdbposterlist
					If poster.Season = tempseason Then
						If rbTVbanner.Checked = True Then
							If poster.Resolution = "seasonwide" And poster.BannerType <> "fanart" And poster.BannerType <> "poster" Then
								usedlist.Add(poster)
							End If
						End If
						If rbTVposter.Checked = True Then
							If poster.Resolution <> "seasonwide" And poster.BannerType <> "fanart" And poster.BannerType <> "banner" Then
								usedlist.Add(poster)
							End If
						End If
					End If
				Next
			End If
			Call tv_PosterPanelPopulate()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvPosterNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterNext.Click  'TV Poster Page Next
		Try
			If ImgBw.IsBusy Then
				ImgBw.CancelAsync()
				Do Until Not ImgBw.IsBusy
					Application.DoEvents()
				Loop
			End If
			tvposterpage += 1
			btnTvPosterSaveBig.Visible = False
			If usedlist.Count < 10 * tvposterpage Then
				btnTvPosterNext.Enabled = False
			End If
			Call tv_PosterSelectionDisplay()
			btnTvPosterPrev.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvPosterPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterPrev.Click  'TV Poster Page Prev
		Try
			If ImgBw.IsBusy Then
				ImgBw.CancelAsync()
				Do Until Not ImgBw.IsBusy
					Application.DoEvents()
				Loop
			End If
			tvposterpage -= 1
			btnTvPosterSaveBig.Visible = False
			If tvposterpage = 1 Then
				btnTvPosterPrev.Enabled = False
			End If
			Call tv_PosterSelectionDisplay()
			btnTvPosterNext.Enabled = True
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvPosterIMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterIMDB.Click
		Try
			btnTvPosterSaveBig.Visible = False
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			If WorkingTvShow.ImdbId = Nothing Then
				MsgBox("No IMDB ID is available for this movie, cant scrape posters")
				Exit Sub
			End If
			If WorkingTvShow.ImdbId.Value = "" Then
				MsgBox("No IMDB ID is available for this movie, cant scrape posters")
				Exit Sub
			End If
			Dim ok As Boolean = False
			If WorkingTvShow.ImdbId.Value.ToLower.IndexOf("tt") = 0 Then
				Dim tempstring As String = WorkingTvShow.ImdbId.Value.ToLower.Substring(2, WorkingTvShow.ImdbId.Value.Length - 2)
				If IsNumeric(tempstring) Then
					ok = True
				End If
			End If
			If IsNumeric(WorkingTvShow.ImdbId) And WorkingTvShow.ImdbId.Value.Length = 7 Then
				WorkingTvShow.ImdbId.Value = "tt" & WorkingTvShow.ImdbId.Value
				ok = True
			End If
			If ok = False Then
				MsgBox("IMDB ID seems to be an invalid format, can't scrape posters")
				Exit Sub
			End If
			tvdbmode = False
			usedlist.Clear()
			If imdbposterlist.Count <= 0 Then
				Dim newobject2 As New imdb_thumbs.Class1
				newobject2.MCProxy = Utilities.MyProxy
				Dim posters(,) As String = newobject2.getimdbposters(WorkingTvShow.ImdbId.Value)
				For f = 0 To UBound(posters)
					If posters(f, 0) <> Nothing Then
						Dim individualposter As New TvBanners
						individualposter.SmallUrl = posters(f, 0)
						individualposter.Url = posters(f, 0)
						imdbposterlist.Add(individualposter)
					End If
				Next
			End If
			For Each po In imdbposterlist
				usedlist.Add(po)
			Next
			usedlist.Reverse()
			Call tv_PosterPanelPopulate()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvPosterUrlBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvPosterUrlBrowse.Click
		Try
			Dim t As New frmImageBrowseOrUrl
			t.Location = Me.PointToScreen(New Point(btnTvPosterUrlBrowse.Left - 480, GroupBox23.Top + GroupBox23.Height))
			t.ShowDialog()
			If t.DialogResult = Windows.Forms.DialogResult.Cancel Or t.tb_PathorUrl.Text = "" Then
				t.Dispose()
				Exit Sub
			End If
			Dim PathOrUrl As String = t.tb_PathorUrl.Text
			t.Dispose()
			t = Nothing
			Try
				Call TvPosterSave(PathOrUrl)
			Catch ex As Exception
				MsgBox(ex.ToString)
			End Try
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
		btnTvPosterSaveBig.Visible = False
		BannerAndPosterViewer()
	End Sub

	Private Sub rbTVbanner_CheckedChanged(sender As Object, e As EventArgs) Handles rbTVbanner.CheckedChanged
		btnTvPosterSaveBig.Visible = False
		BannerAndPosterViewer()
	End Sub

#End Region

#Region "TV Fanart.TV Form"

	Private Sub tvtpfanarttv_Leave(sender As Object, e As EventArgs) Handles tpTvFanartTv.Leave
		If ImgBw.IsBusy Then
			ImgBw.CancelAsync()
			Do Until Not ImgBw.IsBusy
				Application.DoEvents()
			Loop
		End If
		tv_ShowLoad(tv_ShowSelectedCurrently(TvTreeview))
	End Sub

#End Region

#Region "TV Wall Form"

	Private Sub updateTvPosterWall(ByVal path As String, ByVal series As String)
		For Each poster As PictureBox In tpTvWall.Controls
			If poster.Tag = series Then
				util_ImageLoad(poster, path, Utilities.DefaultPosterPath)
				poster.Tag = series
			End If
		Next
		For each poster As PictureBox In tvpicturelist
			If poster.Tag = series Then
				util_ImageLoad(poster, path, Utilities.DefaultPosterPath)
				poster.Tag = series
			End If
		Next
	End Sub

	Private Sub tv_WallReset()
		For i = tpTvWall.Controls.Count - 1 To 0 Step -1
			tpTvWall.Controls.RemoveAt(i)
		Next
		Dim count As Integer = 0
		Dim locx As Integer = 0
		Dim locy As Integer = 0
		Dim tvMaxWallCount As Integer = Math.Floor((tpTvWall.Width - 40) / WallPicWidth)
		While (Cache.TvCache.Shows.Count / tvMaxWallCount) > (WallPicWidth + 15)
			tvMaxWallCount += 1
		End While
		Try
			For Each pic In tvpictureList
				Try
					If count = tvMaxWallCount Then
						count = 0
						locx = 0
						locy += WallPicHeight
					End If
					With pic
						Dim vscrollPos As Integer = tpTvWall.VerticalScroll.Value
						.Location = New Point(locx, locy - vscrollPos)
						.ContextMenuStrip = TVWallContextMenu
					End With
					locx += WallPicWidth
					count += 1
					Me.tpTvWall.Controls.Add(pic)
					tpTvWall.Refresh()
					Application.DoEvents()
				Catch ex As Exception
					MsgBox(ex.ToString)
				End Try
			Next
		Catch ex As Exception
		Finally
		End Try
	End Sub

	Private Sub tv_wallSetup()
		Dim check As Boolean = True
		Dim count As Integer = 0
		Dim locx As Integer = 0
		Dim locy As Integer = 0

		If tvCount_bak = Cache.TvCache.Shows.count Then Exit Sub

		tvMaxWallCount = Math.Floor((tpTvWall.Width - 40) / WallPicWidth)

		While (Cache.TvCache.Shows.Count / tvMaxWallCount) > (WallPicWidth + 15)
			tvMaxWallCount += 1
		End While

		tvpictureList.Clear()
		For i = tpTvWall.Controls.Count - 1 To 0 Step -1
			If tpTvWall.Controls(i).Name = "" Then
				tpTvWall.Controls.RemoveAt(i)
			End If
		Next
		tpTvWall.Refresh()
		Application.DoEvents()

		For Each tvsh In Cache.TvCache.Shows

			tvCount_bak += 1

			bigPictureBox = New PictureBox()
			With bigPictureBox
				.Width = WallPicWidth
				.Height = WallPicHeight
				.SizeMode = PictureBoxSizeMode.StretchImage
				Dim filename As String = Utilities.GetCRC32(tvsh.NfoFilePath)
				Dim posterCache As String = Utilities.PosterCachePath & filename & ".jpg"
				If Not File.Exists(posterCache) And File.Exists(tvsh.ImagePoster.Path) Then
					Try
						Utilities.save2postercache(tvsh.NfoFilePath, tvsh.ImagePoster.Path, WallPicWidth, WallPicHeight)
					Catch
						'Invalid file
						File.Delete(tvsh.ImagePoster.Path)
					End Try
				End If
				If File.Exists(posterCache) Then
					Try
						.Image = Utilities.LoadImage(posterCache)
					Catch
						'Invalid file
						File.Delete(Pref.GetPosterPath(tvsh.nfofilepath))
					End Try
				Else
					.Image = Utilities.LoadImage(Utilities.DefaultPosterPath)
				End If

				.Tag = tvsh.nfofilepath
				Dim toolTip1 As ToolTip = New ToolTip(Me.components)
				toolTip1.AutoPopDelay = 12000

				Dim outline As String = tvsh.Plot.Value
				Dim newoutline As List(Of String) = util_TextWrap(outline, 55)
				outline = ""
				For Each line In newoutline
					outline = outline & vbCrLf & line
				Next
				outline.TrimEnd(vbCrLf)
				Dim epcount As String = "Episodes: " & tvsh.Episodes.Count.ToString & vbcrlf
				Dim Seasoncount As String = "Seasons: " & tvsh.Seasons.Count.ToString & vbCrLf
				Dim ttiptext As String = tvsh.Title.Value & vbCrLf & tvsh.FolderPath & vbCrLf & vbCrLf
				ttiptext = ttiptext & epcount & Seasoncount & vbCrLf & outline
				toolTip1.SetToolTip(bigPictureBox, ttiptext)
				toolTip1.Active = True
				toolTip1.InitialDelay = 0

				.Visible = True
				.BorderStyle = BorderStyle.None
				.WaitOnLoad = True
				.ContextMenuStrip = TVWallContextMenu
				AddHandler bigPictureBox.MouseEnter, AddressOf util_MouseEnter
				AddHandler bigPictureBox.DoubleClick, AddressOf tv_WallClicked
				If count = tvMaxWallCount Then
					count = 0
					locx = 0
					locy += WallPicHeight
				End If
				Dim vscrollPos As Integer = tpTvWall.VerticalScroll.Value
				If mouseDelta <> 0 Then
					vscrollPos = vscrollPos - mouseDelta
					mouseDelta = 0
				End If
				.Location = New Point(locx, locy - vscrollPos)
				locx += WallPicWidth
				count += 1
			End With
			Me.tpTvWall.Controls.Add(bigPictureBox)
			tvpictureList.Add(bigPictureBox)
			Me.tpTvWall.Refresh()
			Application.DoEvents()
		Next
	End Sub

	Private Sub tv_WallClicked(ByVal sender As Object, ByVal e As EventArgs)
		Dim item As Windows.Forms.PictureBox = sender
		Dim tempstring As String = item.Tag
		Dim child As TreeNode
		For Each child In TvTreeview.Nodes
			If TypeOf child.Tag Is Media_Companion.TvShow Then
				Dim TempShow As TvShow = child.Tag
				If TempShow.NfoFilePath = tempstring Then
					TvTreeview.SelectedNode = child
					TabControl3.SelectedIndex = 0
					Exit For
				End If
			End If
		Next
	End Sub

	Private Sub tsmiTvWallPosterChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiTvWallPosterChange.Click
		Dim tempstring As String = ClickedControl
		If tempstring <> Nothing Then
			Dim child As TreeNode
			For Each child In TvTreeview.Nodes
				If TypeOf child.Tag Is Media_Companion.TvShow Then
					Dim TempShow As TvShow = child.Tag
					If TempShow.NfoFilePath = tempstring Then
						TvTreeview.SelectedNode = child
						TabControl3.SelectedIndex = TabControl3.TabPages.IndexOf(tpTvPosters)
						Exit For
					End If
				End If
			Next
		End If
	End Sub

	Private Sub tsmiTvWallLargeView_Click(ByVal sender As Object, ByVal e As EventArgs) Handles tsmiTvWallLargeView.Click
		Dim tempstring As String = ClickedControl
		If tempstring <> Nothing Then
			Try
				Dim Temp2 As String = SeriesFromCache(tempstring).ImagePoster.path
				If File.Exists(Temp2) Then
					Me.ControlBox = False
					MenuStrip1.Enabled = False
					util_ZoomImage(Temp2)
				Else
					MsgBox("Cant find file:-" & vbCrLf & Temp2)
				End If
			Catch ex As Exception
			End Try
		End If
	End Sub

	Private Sub tsmiTvWallOpenFolder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsmiTvWallOpenFolder.Click
		Try
			Dim tempstring As String = ClickedControl
			If tempstring <> Nothing Then
				Try
					Call util_OpenFolder(tempstring)
				Catch ex As Exception
				End Try
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Function SeriesFromCache(ByVal nfopath As String) As TvShow
		Dim child As TreeNode
		For each child In TvTreeview.Nodes
			If TypeOf child.Tag Is Media_Companion.TvShow Then
				Dim TempShow As TvShow = child.Tag
				If TempShow.NfoFilePath = nfopath Then Return TempShow
			End If
		Next
		Return Nothing
	End Function

#End Region

#Region "Tv Show Selector Form"

	Private Sub tbTvShSelectSearchTitle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbTvShSelectSearchTitle.KeyDown
		Try
			If e.KeyCode = Keys.Enter Then
				Call tv_ShowListLoad()
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btnTvShSelectSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvShSelectSearch.Click
		Try
			Call tv_ShowListLoad()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub lbxTvShSelectResults_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbxTvShSelectResults.SelectedIndexChanged
		util_ImageLoad(pbTvShSelectPreviewImg, listOfShows(lbxTvShSelectResults.SelectedIndex).showbanner, Utilities.DefaultBannerPath)
	End Sub

	Private Sub lbxTvShSelectLang_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxTvShSelectLang.SelectedIndexChanged
		Call util_LanguageCheck()
	End Sub

	Private Sub rbTvShSelectArtBanner_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTvShSelectArtBanner.CheckedChanged
		If rbTvShSelectArtBanner.Checked = True Then
			Pref.postertype = "banner"
		Else
			Pref.postertype = "poster"
		End If
	End Sub

	Private Sub rbTvShSelectArtPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTvShSelectArtPoster.CheckedChanged
		If rbTvShSelectArtPoster.Checked = True Then
			Pref.postertype = "poster"
		Else
			Pref.postertype = "banner"
		End If
	End Sub

	Private Sub rbTvShSelectSAllWide_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTvShSelectSAllWide.CheckedChanged
		If rbTvShSelectSAllWide.Checked = True Then Pref.seasonall = "wide"
	End Sub

	Private Sub rbTvShSelectSAllPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTvShSelectSAllPoster.CheckedChanged
		If rbTvShSelectSAllPoster.Checked = True Then Pref.seasonall = "poster"
	End Sub

	Private Sub rbTvShSelectSAllNone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbTvShSelectSAllNone.CheckedChanged
		If rbTvShSelectSAllNone.Checked = True Then Pref.seasonall = "none"
	End Sub

	Private Sub cbTvChgShowDLSeason_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTvChgShowDLSeason.CheckedChanged
		Pref.TvChgShowDlSeasonthumbs = cbTvChgShowDLSeason.Checked
	End Sub

	Private Sub cbTvChgShowOverwriteImgs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTvChgShowOverwriteImgs.CheckedChanged
		Pref.TvChgShowOverwriteImgs = cbTvChgShowOverwriteImgs.Checked
	End Sub

	Private Sub cbTvChgShowDLFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTvChgShowDLFanart.CheckedChanged
		Pref.TvChgShowDlFanart = cbTvChgShowDLFanart.Checked
	End Sub

	Private Sub cbTvChgShowDLFanartTvArt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTvChgShowDLFanartTvArt.CheckedChanged
		Pref.TvChgShowDlFanartTvArt = cbTvChgShowDLFanartTvArt.Checked
	End Sub

	Private Sub cbTvChgShowDLPoster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTvChgShowDLPoster.CheckedChanged
		Pref.TvChgShowDlPoster = cbTvChgShowDLPoster.Checked
	End Sub

	Private Sub btnTvShSelectScrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvShSelectScrape.Click
		Try
			Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
			If listOfShows.Count = 1 And listOfShows(0).showtitle = "TVDB Search Returned Zero Results" Then
				MsgBox("No show is selected")
				Exit Sub
			End If
			Dim tempstring As String = ""
			If Label55.Text.IndexOf("is not available in") <> -1 Then
				MsgBox("Please select a language that is available for this show")
				Exit Sub
			End If
            If WorkingTvShow.Episodes.Count > 0 Then
                Dim reply As MsgBoxResult = MsgBox("Warning!!!" & vbCrLf & "Episode nfo's found" & vbCrLf & "Continuing will delete all episode nfo", MsgBoxStyle.YesNo)
                If reply = MsgBoxResult.No Then Exit Sub
                TvDelEpNfoAst(workingtvshow, Nothing, Nothing, True, Pref.TvChgShowOverwriteImgs)
            End If
			'messbox = New frmMessageBox("The Selected TV Show is being Scraped", "", "Please Wait")
			'System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
			'messbox.Show()
			'messbox.Refresh()
			Application.DoEvents()
            Dim lbxindex As Integer = lbxTvShSelectResults.SelectedIndex
            TvDelShowNfoArt(workingtvshow, True, Pref.TvChgShowOverwriteImgs, True)
			Dim LanCode As String
			If lbxTvShSelectLang.SelectedIndex = -1 Then
				LanCode = Pref.TvdbLanguageCode
			Else
				LanCode = languageList(lbxTvShSelectLang.SelectedIndex).Abbreviation.Value
			End If
            '#discontine
			If Pref.tvshow_useXBMC_Scraper = True Then
                messbox = New frmMessageBox("The Selected TV Show is being Scraped", "", "Please Wait")
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                messbox.Show()
                messbox.Refresh()
                Dim TVShowNFOContent As String = XBMCScrape_TVShow_General_Info("metadata.tvdb.com", listOfShows(lbxTvShSelectResults.SelectedIndex).showid, LanCode, WorkingTvShow.NfoFilePath)
				If TVShowNFOContent <> "error" Then CreateMovieNfo(WorkingTvShow.NfoFilePath, TVShowNFOContent)
				Dim newshow As TvShow = nfoFunction.tvshow_NfoLoad(WorkingTvShow.NfoFilePath)
				newshow.ListActors.Clear()
				If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Or NewShow.ImdbId.Value = Nothing Then
					TvGetActorTvdb(NewShow)
				ElseIf (Pref.TvdbActorScrape = 1 Or Pref.TvdbActorScrape = 2) And NewShow.ImdbId.Value <> Nothing Then
					TvGetActorImdb(NewShow)
				End If
				If Pref.tvdbIMDbRating Then
					Dim rating As String = ""
					Dim votes As String = ""
					If ep_getIMDbRating(newshow.ImdbId.Value, rating, votes) Then
						newshow.Rating.Value = rating
						newshow.Votes.Value = votes
					End If
				End If
				nfoFunction.tvshow_NfoSave(newshow, True)
				Call tv_ShowLoad(WorkingTvShow)
				TvTreeview.Refresh()
				messbox.Close()
				TabControl3.SelectedIndex = 0
			Else
				If Pref.TvChgShowOverwriteImgs Then TvDeleteShowArt(WorkingTvShow)
				newTvFolders.Add(WorkingTvShow.FolderPath.Substring(0, WorkingTvShow.FolderPath.LastIndexOf("\")))
                Dim newshow As New str_PossibleShowList
                newshow.showid      = listOfShows(lbxindex).showid
                newshow.showbanner  = listOfShows(lbxindex).showbanner
                newshow.showtitle   = listOfShows(lbxindex).showtitle
                newshow.langcode    = LanCode
                listOfShows.Clear()
                listOfShows.Add(newshow)
                TabControl3.SelectedIndex = 0
                RunBackgroundTVScrape("TVSeriesChange")
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		Finally
			Pref.TvChgShowDlFanart = False
			Pref.TvChgShowDlFanartTvArt = False
			Pref.TvChgShowDlPoster = False
			Pref.TvChgShowDlSeasonthumbs = False
			Pref.TvChgShowOverwriteImgs = False
		End Try
	End Sub

#End Region

#Region "TV TVDB/IMDB Form"

	Private Sub btn_TvTVDb_Click(sender As System.Object, e As System.EventArgs) Handles btn_TvTVDb.Click
		Dim url As String
		Dim Show As Media_Companion.TvShow = tv_ShowSelectedCurrently(TvTreeview)
		If Show.TvdbId.Value.Contains("tt") Then
			MsgBox("Invalid Tvdb ID" & vbCrLf & "Unable to load Show's TVDB page")
			Exit Sub
		End If
		If String.IsNullOrEmpty(Show.TvdbId.Value) Then
			MsgBox("Selected Show has no Tvdb ID" & vbCrLf & "Unable to load Show's TVDB page")
			Exit Sub
		End If
		Dim TvdbId As Integer = Show.TvdbId.Value
		url = "http://thetvdb.com/?tab=series&id=" & TvdbId & "&lid=7"
		Try
			WebBrowser4.Stop()
			WebBrowser4.ScriptErrorsSuppressed = True
			WebBrowser4.Navigate(url)
		Catch
			WebBrowser4.Stop()
			WebBrowser4.ScriptErrorsSuppressed = True
			WebBrowser4.Navigate(url)
		End Try
		WebBrowser4.Focus()
	End Sub

	Private Sub btn_TvIMDB_Click(sender As System.Object, e As System.EventArgs) Handles btn_TvIMDB.Click
		Dim url As String
		Dim Show As Media_Companion.TvShow = tv_ShowSelectedCurrently(TvTreeview)
		If String.IsNullOrEmpty(Show.ImdbId.Value) Then
			MsgBox("Selected Show has no IMDB ID" & vbCrLf & "Unable to load Show's IMDB page")
			Exit Sub
		End If
		url = "http://www.imdb.com/title/" & Show.ImdbId.Value & "/"
		Try
			WebBrowser4.Stop()
			WebBrowser4.ScriptErrorsSuppressed = True
			WebBrowser4.Navigate(url)
		Catch
			WebBrowser4.Stop()
			WebBrowser4.ScriptErrorsSuppressed = True
			WebBrowser4.Navigate(url)
		End Try
		WebBrowser4.Focus()
	End Sub

	Private Sub btnTvWebStop_Click(sender As System.Object, e As System.EventArgs) Handles btnTvWebStop.Click
		WebBrowser4.Stop()
		WebBrowser4.Focus()
	End Sub

	Private Sub btnTvWebRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnTvWebRefresh.Click
		Try
			WebBrowser4.Refresh()
		Catch
		End Try
		WebBrowser4.Focus()
	End Sub

	Private Sub btnTvWebBack_Click(sender As System.Object, e As System.EventArgs) Handles btnTvWebBack.Click
		Try
			WebBrowser4.GoBack()
		Catch
		End Try
		WebBrowser4.Focus()
	End Sub

	Private Sub btnTvWebForward_Click(sender As System.Object, e As System.EventArgs) Handles btnTvWebForward.Click
		Try
			WebBrowser4.GoForward()
		Catch
		End Try
		WebBrowser4.Focus()
	End Sub

#End Region

#Region "Tv Folders Form"
    
	Private Sub tpTvFolders_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tpTvFolders.Leave
		If tvfolderschanged Then
			Dim save = MsgBox("You have made changes to some folders" & vbCrLf & "    Do you wish to save these changes?", MsgBoxStyle.YesNo)
			If save = DialogResult.Yes Then
				btn_TvFoldersSave.PerformClick()
			Else
				btn_TvFoldersUndo.PerformClick()
			End If
			tvfolderschanged = False
		End If
	End Sub

	Private Sub btn_TvFoldersRootAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersRootAdd.Click
		Try
			If TextBox39.Text = Nothing Then
				Exit Sub
			End If
			If TextBox39.Text = "" Then
				Exit Sub
			End If
			Dim tempstring As String = TextBox39.Text
			Do While tempstring.LastIndexOf("\") = tempstring.Length - 1
				tempstring = tempstring.Substring(0, tempstring.Length - 1)
			Loop
			Do While tempstring.LastIndexOf("/") = tempstring.Length - 1
				tempstring = tempstring.Substring(0, tempstring.Length - 1)
			Loop
			Dim exists As Boolean = False
			For Each item In clbx_TvRootFolders.items
                If (item.ToString.Equals(tempstring,  StringComparison.InvariantCultureIgnoreCase)) Then
					exists = True
					Exit For
				End If
			Next
			If exists = True Then
				MsgBox("        Folder Already Exists")
			Else
				Dim f As New DirectoryInfo(tempstring)
				If f.Exists Then
					AuthorizeCheck = True
					clbx_TvRootFolders.Items.Add(tempstring, True)
					AuthorizeCheck = False
					TextBox39.Text = ""
					tvfolderschanged = True
				Else
					Dim tempint As Integer = MessageBox.Show("This folder does not appear to exist" & vbCrLf & "Are you sure you wish to add it", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					If tempint = DialogResult.Yes Then
						AuthorizeCheck = True
						clbx_TvRootFolders.Items.Add(tempstring, True)
						AuthorizeCheck = False
						TextBox39.Text = ""
						tvfolderschanged = True
					End If
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_TvFoldersRootBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersRootBrowse.Click
		Try
			'browse for root tv folder
			Dim allok As Boolean = True
			Dim cancelregex As Boolean = False
			Dim newtvshow As Boolean = False
			Dim strfolder As String
			Dim tempstring3 As String
			Dim tempint As Integer = 0
			Dim tempint2 As Integer = 0
			fb.Description = "Please Select Root Folder of the TV Shows You Wish To Add to DB"
			fb.ShowNewFolderButton = True
			fb.RootFolder = System.Environment.SpecialFolder.Desktop
			fb.SelectedPath = Pref.lastpath
			Tmr.Start()
			If fb.ShowDialog = Windows.Forms.DialogResult.OK Then
				strfolder = (fb.SelectedPath)
				Pref.lastpath = strfolder
				Dim hasseason As Boolean = False
				If Not clbx_TvRootFolders.Items.Contains(strfolder) Then
					For Each strfolder2 As String In My.Computer.FileSystem.GetDirectories(strfolder)
						Dim M As Match
						tempstring3 = strfolder2.ToLower.Replace(strfolder.ToLower, "")
						M = Regex.Match(tempstring3, "(series ?\d+|season ?\d+|s ?\d+|^\d{1,3}$)")
						If M.Success = True Then
							hasseason = True
							Exit For
						End If
					Next
					If hasseason = True Then
						tempint = MessageBox.Show(strfolder & " Appears to Contain Season Folders." & vbCrLf & "Are you sure this folder contains multiple" & vbCrLf & "TV Shows, each in its own folder?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						If tempint = DialogResult.Yes Then
							clbx_TvRootFolders.Items.Add(strfolder, True)
							tvfolderschanged = True
						ElseIf tempint = DialogResult.No Then
							tempint2 = MessageBox.Show("Do you wish to add this as a single TV Show Folder?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							If tempint2 = DialogResult.Yes Then
								If Not lb_tvSeriesFolders.Items.Contains(strfolder) Then
									lb_tvSeriesFolders.Items.Add(strfolder)
									tvfolderschanged = True
								Else
									MsgBox("Folder not added, Already exists")
								End If
							End If
						End If
					Else
						AuthorizeCheck = True
						clbx_TvRootFolders.Items.Add(strfolder, True)
						AuthorizeCheck = False
						tvfolderschanged = True
					End If
				Else
					MsgBox("Root already exists")
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_TvFoldersRootRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersRootRemove.Click
		Try
			While clbx_TvRootFolders.SelectedItems.Count > 0
				clbx_TvRootFolders.Items.Remove(clbx_TvRootFolders.SelectedItems(0))
				tvfolderschanged = True
			End While
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Property AuthorizeCheck() As Boolean
		Get
			Return m_AuthorizeCheck
		End Get
		Set
			m_AuthorizeCheck = Value
		End Set
	End Property

	Private m_AuthorizeCheck As Boolean


	Private Sub clbx_TvRootFolders_MouseDown(sender As Object, e As MouseEventArgs) Handles clbx_TvRootFolders.MouseDown
		If e.Button = MouseButtons.Right Then
			Dim index As Integer = clbx_TvRootFolders.IndexFromPoint(New Point(e.X, e.Y))
			If index >= 0 Then
				clbx_TvRootFolders.SelectedItem = clbx_TvRootFolders.Items(index)
			End If
		Else
			Dim loc As Point = Me.clbx_TvRootFolders.PointToClient(Cursor.Position)
			For i As Integer = 0 To Me.clbx_TvRootFolders.Items.Count - 1
				Dim rec As Rectangle = Me.clbx_TvRootFolders.GetItemRectangle(i)
				rec.Width = 16
				'checkbox itself has a default width of about 16 pixels
				If rec.Contains(loc) Then
					AuthorizeCheck = True
					Dim newValue As Boolean = Not Me.clbx_TvRootFolders.GetItemChecked(i)
					Me.clbx_TvRootFolders.SetItemChecked(i, newValue)
					AuthorizeCheck = False

					Return
				End If
			Next
		End If
	End Sub

	Private Sub clbx_TvRootFolders_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbx_TvRootFolders.ItemCheck
		If Not AuthorizeCheck Then
			e.NewValue = e.CurrentValue
		End If
		Static Updating As Boolean
		If Updating Then Exit Sub
		Updating = True

		Dim cmbBox As CheckedListBox = sender
		Dim Item As ItemCheckEventArgs = e
		Dim unchkd As Boolean = False
		If Item.NewValue = CheckState.Checked Then
			cmbBox.SetItemChecked(Item.Index, True)
		Else
			unchkd = True
			cmbBox.SetItemChecked(Item.Index, False)
		End If

		If unchkd Then
			Dim rtfolder As String = cmbBox.Items(Item.Index).ToString
			rtfolder = rtfolder & If(rtfolder.Contains("\"), "\", "/")
			For f = lb_tvSeriesFolders.Items.Count - 1 To 0 Step -1
				If lb_tvSeriesFolders.Items(f).contains(rtfolder) Then
					lb_tvSeriesFolders.Items.RemoveAt(f)
					tvfolderschanged = True
				End If
			Next
		End If

		Updating = False
	End Sub

	Private Sub bnt_TvChkFolderList_Click(sender As System.Object, e As System.EventArgs) Handles bnt_TvChkFolderList.Click
		Try
			tvfolderschanged = tv_Showremovedfromlist(lb_tvSeriesFolders, True, , True)
		Catch ex As Exception

		End Try
	End Sub

	Private Sub btn_TvFoldersAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersAdd.Click
		Try
			If TextBox40.Text = Nothing Then
				Exit Sub
			End If
			If TextBox40.Text = "" Then
				Exit Sub
			End If
			Dim tempstring As String = TextBox40.Text
			Do While tempstring.LastIndexOf("\") = tempstring.Length - 1
				tempstring = tempstring.Substring(0, tempstring.Length - 1)
			Loop
			Do While tempstring.LastIndexOf("/") = tempstring.Length - 1
				tempstring = tempstring.Substring(0, tempstring.Length - 1)
			Loop
			Dim exists As Boolean = False
			For each fol In Pref.tvRootFolders
                If (fol.rpath.Equals(tempstring,  StringComparison.InvariantCultureIgnoreCase)) Then Continue For
				If tempstring.ToLower.Contains(fol.rpath.ToLower) AndAlso Not fol.selected Then
					Dim msg As String = "The series dropped is in a root folder that has been unselected"
					msg &= "To avoid catastrophic failure, please re-select"
					msg &= "root folder: " & fol.rpath
					msg &= "and attempt again"
					MsgBox(msg)
					exists = True
					Exit For
				End If
			Next
			If exists Then Exit Sub
			exists = False
			For Each item In lb_tvSeriesFolders.Items
                If (item.ToString.Equals(tempstring,  StringComparison.InvariantCultureIgnoreCase)) Then
					exists = True
					Exit For
				End If
			Next
			If exists Then
				MsgBox("        Folder Already Exists")
			Else
				Dim f As New DirectoryInfo(tempstring)
				If f.Exists Then
					lb_tvSeriesFolders.Items.Add(tempstring)
					tvfolderschanged = True
					TextBox40.Text = ""
					newTvFolders.Add(tempstring)
				Else
					Dim tempint As Integer = MessageBox.Show("This folder does not appear to exist" & vbCrLf & "Are you sure you wish to add it", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					If tempint = DialogResult.Yes Then
						lb_tvSeriesFolders.Items.Add(tempstring)
						tvfolderschanged = True
						TextBox40.Text = ""
						newTvFolders.Add(tempstring)
					End If
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

	End Sub

	Private Sub btn_TvFoldersAddFromRoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersAddFromRoot.Click
		Try
			Dim tmplst As New List(Of str_RootPaths)
			For f = 0 To clbx_TvRootFolders.Items.Count - 1
				Dim t As New str_RootPaths
				t.rpath = clbx_TvRootFolders.Items(f).ToString
				Dim chkstate As CheckState = clbx_TvRootFolders.GetItemCheckState(f)
				t.selected = (chkstate = CheckState.Checked)
				If t.selected Then tmplst.Add(t)
			Next
			tv_ShowFind(tmplst, True)
			If newTvFolders.Count > 0 Then
				tvfolderschanged = True
				For Each item In newTvFolders
					lb_tvSeriesFolders.Items.Add(item)
				Next
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_TvFoldersBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersBrowse.Click
		Try
			Dim allok As Boolean = True
			Dim thefoldernames As String
			fb.Description = "Please Select TV Folder to Add to DB"
			fb.ShowNewFolderButton = True
			fb.RootFolder = System.Environment.SpecialFolder.Desktop
			fb.SelectedPath = Pref.lastpath
			Tmr.Start()
			If fb.ShowDialog = Windows.Forms.DialogResult.OK Then
				thefoldernames = (fb.SelectedPath)
				For Each item As Object In lb_tvSeriesFolders.Items
					If thefoldernames.ToString = item.ToString Then allok = False
				Next
				Pref.lastpath = thefoldernames
				If allok = True Then
					lb_tvSeriesFolders.Items.Add(thefoldernames)
					newTvFolders.Add(thefoldernames)
					tvfolderschanged = True
				Else
					MsgBox("        Folder Already Exists", MsgBoxStyle.OkOnly)
				End If
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_TvFoldersRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersRemove.Click
		Try
			Dim Folder As String
			Dim cachechanged As Boolean = False
			While lb_tvSeriesFolders.SelectedItems.Count > 0
				tvfolderschanged = True
				Folder = lb_tvSeriesFolders.SelectedItems(0)

				For Each Item As Media_Companion.TvShow In Cache.TvCache.Shows
					If Item.FolderPath.Trim("\") = Folder.Trim("\") Then
						TvTreeview.Nodes.Remove(Item.ShowNode)
						For Each ep As TvEpisode In Item.Episodes
							Cache.TvCache.Remove(ep)
						Next
						Cache.TvCache.Remove(Item)
						cachechanged = True
						Exit For
					End If
				Next
				lb_tvSeriesFolders.Items.Remove(lb_tvSeriesFolders.SelectedItems(0))
			End While
			If cachechanged Then Tv_CacheSave()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub btn_TvFoldersUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersUndo.Click
		newTvFolders.Clear()
		tvfolderschanged = False
	End Sub

	Private Sub btn_TvFoldersSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvFoldersSave.Click
		Try
			Dim removeTvFolders As New List(Of String)
			Pref.tvRootFolders.Clear()
			For f = 0 To clbx_TvRootFolders.Items.Count - 1
				Dim t As New str_RootPaths
				t.rpath = clbx_TvRootFolders.Items(f).ToString
				Dim chkstate As CheckState = clbx_TvRootFolders.GetItemCheckState(f)
				t.selected = (chkstate = CheckState.Checked)
				Pref.tvRootFolders.Add(t)
			Next
			newTvFolders.Clear()
			Dim tmplist As New List(Of String)
			tmplist.AddRange(Pref.tvFolders)
			Pref.tvFolders.Clear()
			For Each item In lb_tvSeriesFolders.Items
				Pref.tvFolders.Add(item)
				If Not tmplist.Contains(item) Then
					newTvFolders.Add(item)
				End If
			Next
			For Each item In tmplist
				If Not Pref.tvFolders.Contains(item) Then removeTvFolders.Add(item)
			Next
			If Not removeTvFolders.Count = 0 Then
				Dim cachechanged As Boolean = False
				For Each tvfolder In removeTvFolders
					For Each cacheItem As Media_Companion.TvShow In Cache.TvCache.Shows
						If cacheItem.FolderPath.Trim("\") = tvfolder.Trim("\") Then
							TvTreeview.Nodes.Remove(cacheItem.ShowNode)
							For Each ep As TvEpisode In cacheItem.Episodes
								Cache.TvCache.Remove(ep)
							Next
							Cache.TvCache.Remove(cacheItem)
							cachechanged = True
							Exit For
						End If
					Next
				Next
				If cachechanged Then Tv_CacheSave()
			End If
			tvfolderschanged = False
			Pref.ConfigSave()
			tv_ShowScrape()
			TabControl3.SelectedIndex = 0
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub clbx_TvRootFolders_DragDrop(sender As Object, e As DragEventArgs) Handles clbx_TvRootFolders.DragDrop
		Dim files() As String
		Dim tempstring3 As String
		Dim tempint As Integer = 0
		Dim tempint2 As Integer = 0
		droppedItems.Clear()
		files = e.Data.GetData(DataFormats.FileDrop)
		For f = 0 To UBound(files)
			If Directory.Exists(files(f)) Then
				Dim hasseason As Boolean = False
				If Not clbx_TvRootFolders.Items.Contains(files(f)) Then
					For Each strfolder2 As String In My.Computer.FileSystem.GetDirectories(files(f))
						Dim M As Match
						tempstring3 = strfolder2.ToLower.Replace(files(f).ToLower, "")
						M = Regex.Match(tempstring3, "(series ?\d+|season ?\d+|s ?\d+|^\d{1,3}$)")
						If M.Success = True Then
							hasseason = True
							Exit For
						End If
					Next
					If hasseason = True Then
						tempint = MessageBox.Show(files(f) & " Appears to Contain Season Folders." & vbCrLf & "Are you sure this folder contains multiple" & vbCrLf & "TV Shows, each in its own folder?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						If tempint = DialogResult.Yes Then
							AuthorizeCheck = True
							clbx_TvRootFolders.Items.Add(files(f), True)
							AuthorizeCheck = False
							tvfolderschanged = True
						ElseIf tempint = DialogResult.No Then
							tempint2 = MessageBox.Show("Do you wish to add this as a single TV Show Folder?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							If tempint2 = DialogResult.Yes Then
								If Not lb_tvSeriesFolders.Items.Contains(files(f)) Then
									lb_tvSeriesFolders.Items.Add(files(f))
									tvfolderschanged = True
								Else
									MsgBox("Folder not added, Already exists")
								End If
							End If
						End If
					Else
						AuthorizeCheck = True
						clbx_TvRootFolders.Items.Add(files(f), True)
						AuthorizeCheck = False
						tvfolderschanged = True
					End If
				Else
					MsgBox("Root already exists")
				End If
			End If
		Next
	End Sub

	Private Sub clbx_TvRootFolders_DragEnter(sender As Object, e As DragEventArgs) Handles clbx_TvRootFolders.DragEnter
		Try
			e.Effect = DragDropEffects.Copy
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub clbx_TvRootFolders_KeyPress(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles clbx_TvRootFolders.KeyDown
		If e.KeyCode = Keys.Delete AndAlso clbx_TvRootFolders.SelectedItem <> Nothing Then
			Call btn_TvFoldersRootRemove.PerformClick()
		ElseIf e.KeyCode = Keys.Space Then
			AuthorizeCheck = True
			Call clbx_tvrootfoldertoggle()
			AuthorizeCheck = False
		End If
	End Sub

	Private Sub clbx_tvrootfoldertoggle()
		Dim i = clbx_TvRootFolders.SelectedIndex
		clbx_TvRootFolders.SetItemCheckState(i, If(clbx_TvRootFolders.GetItemCheckState(i) = CheckState.Checked, CheckState.Unchecked, CheckState.Checked))
	End Sub

	Private Sub tsmi_tvRtAddSeries_click(sender As Object, e As EventArgs) Handles tsmi_tvRtAddSeries.Click
		Dim rtpath As String = clbx_TvRootFolders.SelectedItem.ToString
		Dim tmplst As New List(Of str_RootPaths)
		Dim t As New str_RootPaths
		t.rpath = rtpath
		t.selected = True
		tmplst.Add(t)
		tv_ShowFind(tmplst, True)
		If newTvFolders.Count > 0 Then
			tvfolderschanged = True
			For Each item In newTvFolders
				lb_tvSeriesFolders.Items.Add(item)
			Next
		End If
	End Sub

	Private Sub TvRootFolderContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TvRootFolderContextMenu.Opening
		If clbx_TvRootFolders.SelectedIndex = -1 Then
			e.Cancel = True
		Else
			If Not clbx_TvRootFolders.GetItemChecked(clbx_TvRootFolders.SelectedIndex) Then
				e.Cancel = True
			End If
		End If
	End Sub

	Private Sub lb_tvSeriesFolders_DragDrop(sender As Object, e As DragEventArgs) Handles lb_tvSeriesFolders.DragDrop
		Dim files() As String
		droppedItems.Clear()
		Dim skipdrop As Boolean
		files = e.Data.GetData(DataFormats.FileDrop)
		For f = 0 To UBound(files)
			skipdrop = False
			If Directory.Exists(files(f)) Then
				If files(f).ToLower.Contains(".actors") Or files(f).ToLower.Contains("season") Then Continue For
				For Each fol In Pref.tvRootFolders
					If fol.rpath = files(f) Then Continue For
					If files(f).Contains(fol.rpath) AndAlso Not fol.selected Then
						Dim msg As String = "The series dropped is in a root folder that has been unselected" & vbCrLf
						msg &= "To avoid catastrophic failure, please re-select" & vbCrLf
						msg &= "root folder: " & fol.rpath & vbCrLf
						msg &= "and attempt again"
						MsgBox(msg)
						skipdrop = True
						Continue For
					End If
				Next
				If skipdrop Then Continue For
				Dim di As New DirectoryInfo(files(f))
				If lb_tvSeriesFolders.Items.Contains(files(f)) Then Continue For
				Dim skip As Boolean = False
				For Each item In droppedItems
					If item = files(f) Then
						skip = True
						Exit For
					End If
				Next
				If Not skip Then droppedItems.Add(files(f))
			End If
		Next
		If droppedItems.Count < 1 Then Exit Sub
		For Each item In droppedItems
			lb_tvSeriesFolders.Items.Add(item)
			tvfolderschanged = True
		Next
	End Sub

	Private Sub lb_tvSeriesFolders_DragEnter(sender As Object, e As DragEventArgs) Handles lb_tvSeriesFolders.DragEnter
		Try
			e.Effect = DragDropEffects.Copy
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub lb_tvSeriesFolders_KeyPress(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lb_tvSeriesFolders.KeyDown
		If e.KeyCode = Keys.Delete AndAlso lb_tvSeriesFolders.SelectedItem <> Nothing Then
			Call btn_TvFoldersRemove.PerformClick()
		End If
	End Sub

#End Region

#Region "Home Movie routines"

    ''' <summary>
    ''' All Other Home Movie code is now in Form1.HomeMovie.vb
    ''' </summary>
	Private Sub TabControl1_Selecting(sender As System.Object, e As CancelEventArgs) Handles TabControl1.Selecting
		If TabControl1.SelectedTab.Text.ToLower = "homemovie preferences" Then
			e.Cancel = True
			OpenPreferences(3)
		End If
	End Sub
    
	Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        
		Dim tab As String = TabControl1.SelectedTab.Text.ToLower
		If tab = "screenshot" Then
			pbx_HmFanartSht.SizeMode = PictureBoxSizeMode.Zoom

			util_ImageLoad(pbx_HmFanartSht, WorkingHomeMovie.fileinfo.fanartpath, Utilities.DefaultFanartPath)
			If tb_HmFanartTime.Text = "" Then tb_HmFanartTime.Text = Pref.HmFanartTime.ToString
			homeTabIndex = TabControl1.SelectedIndex
		ElseIf tab = " poster " Then
			util_ImageLoad(pbx_HmPosterSht, WorkingHomeMovie.fileinfo.posterpath, Utilities.DefaultPosterPath)
			If tb_HmPosterTime.Text = "" Then tb_HmPosterTime.Text = Pref.HmPosterTime.ToString
			homeTabIndex = TabControl1.SelectedIndex
		ElseIf tab = "folders" Then
			HomeFoldersUpdate()
			homeTabIndex = TabControl1.SelectedIndex
		Else
			homeTabIndex = TabControl1.SelectedIndex
		End If
	End Sub

#End Region

	Sub BckWrkCheckNewVersion_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BckWrkCheckNewVersion.DoWork

		Dim ShowNoNewVersionMsgBox As String = DirectCast(e.Argument, Boolean)

		e.Result = New NewVersionCheckResult(ShowNoNewVersionMsgBox, CheckForNewVersion)
	End Sub

	Private Sub BckWrkCheckNewVersion_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BckWrkCheckNewVersion.RunWorkerCompleted

		Dim Results As NewVersionCheckResult = e.Result

		If IsNothing(Results.NewVersion) Then

			If Results.ShowNoNewVersionMsgBox Then
				MsgBox("You're up-to-date!", MsgBoxStyle.OkOnly, "No new version found")
			End If

			Exit Sub
		End If


		Dim answer = MsgBox("Would you like to download the new version?", MsgBoxStyle.YesNo, "New version " & Results.NewVersion & " available")

		If answer = MsgBoxResult.Yes Then
			CloseMC = True
            OpenUrl(SF_Home_Page & "files/")
			'OpenUrl(HOME_PAGE)
		End If
	End Sub

#Region "ToolStrip Menus"

	Private Sub tsmi_RenMovieOnly_click(sender As Object, e As EventArgs) Handles tsmi_RenMovieOnly.Click
		Dim ismovrenenabled As Boolean = Pref.MovieRenameEnable
		Dim isusefolder As Boolean = Pref.usefoldernames
		If Pref.MovieManualRename Then
			If isusefolder Then
				Dim tempint As Integer = MessageBox.Show("You currently have 'UseFolderName' Selected" & vbCrLf & "Are you sure you wish to Rename this Movie file", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				If tempint = DialogResult.No Then
					Exit Sub
				End If
			End If
			Pref.MovieRenameEnable = True
			Pref.usefoldernames = False
			mov_ScrapeSpecific("rename_files")
			While BckWrkScnMovies.IsBusy
				Application.DoEvents()
			End While
		Else
			MsgBox("Manual Movie Rename is not enabled", 0)
		End If
		Pref.MovieRenameEnable = ismovrenenabled
		Pref.usefoldernames = isusefolder
	End Sub

	Private Sub tsmi_RenMovFolderOnly_click(sender As Object, e As EventArgs) Handles tsmi_RenMovFolderOnly.Click
		Dim isMovFoldRenEnabled As Boolean = Pref.MovFolderRename
		If Pref.MovieManualRename Then
			Pref.MovFolderRename = True
			mov_ScrapeSpecific("rename_folders")
			While BckWrkScnMovies.IsBusy
				Application.DoEvents()
			End While
		Else
			MsgBox("Manual Movie Rename is not enabled", 0)
		End If
		Pref.MovFolderRename = isMovFoldRenEnabled
	End Sub

	Private Sub tsmi_RenMovieAndFolder_click(sender As Object, e As EventArgs) Handles tsmi_RenMovieAndFolder.Click
		Dim isMovFoldRenEnabled As Boolean = Pref.MovFolderRename
		Dim isMovRenEnabled As Boolean = Pref.MovieRenameEnable
		Dim isusefolder As Boolean = Pref.usefoldernames
		If Pref.MovieManualRename Then
			Dim renmov As Boolean = True
			If isusefolder Then
				Dim tempint As Integer = MessageBox.Show("You currently have 'UseFolderName' Selected" & vbCrLf & "Are you sure you wish to Rename this Movie file" & vbCrLf & "Folder Renaming will still commence", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				If tempint = DialogResult.No Then
					renmov = False
				End If
			End If
			Pref.MovFolderRename = True
			Pref.MovieRenameEnable = True
			Pref.usefoldernames = False
			_rescrapeList.FullPathAndFilenames.Clear()

			For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
				_rescrapeList.FullPathAndFilenames.Add(row.Cells("fullpathandfilename").Value.ToString)
			Next
			rescrapeList.ResetFields()
			rescrapeList.Rename_Folders = True
			rescrapeList.Rename_Files = renmov

			RunBackgroundMovieScrape("BatchRescrape")

			While BckWrkScnMovies.IsBusy
				Application.DoEvents()
			End While
		Else
			MsgBox("Manual Movie Rename is not enabled", 0)
		End If
		Pref.MovFolderRename = isMovFoldRenEnabled
		Pref.MovieRenameEnable = isMovRenEnabled
		Pref.usefoldernames = isusefolder
	End Sub

	Private Sub tsmiMov_OpenInMkvmergeGUI_Click(sender As Object, e As EventArgs) Handles tsmiMov_OpenInMkvmergeGUI.Click

		If DataGridViewMovies.SelectedRows.Count > 10 Then
			If MsgBox("Are you sure you want to open that many?", MsgBoxStyle.YesNo, "About to open " & DataGridViewMovies.SelectedRows.Count & " instances of Mkvmerge Gui") <> MsgBoxResult.Ok Then Exit Sub
		End If

		For Each row In DataGridViewMovies.SelectedRows
			Process.Start(Pref.MkvMergeGuiPath, """" & Utilities.GetFileName(row.Cells("fullpathandfilename").Value) & """")
		Next
	End Sub

	Private Sub tsmiCheckForNewVersion_Click(sender As System.Object, e As System.EventArgs) Handles tsmiCheckForNewVersion.Click
		BckWrkCheckNewVersion.RunWorkerAsync(True)
	End Sub

	Private Sub tsmiMov_ConvertToFrodo_Click(sender As Object, e As EventArgs) Handles tsmiMov_ConvertToFrodo.Click
		Try
			Call mov_ScrapeSpecific("Convert_To_Frodo")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsm_TvScrapePoster_Click(sender As System.Object, e As System.EventArgs) Handles tsm_TvScrapePoster.Click
		Try
			TvScrapePosterBanner("poster")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsm_TvSelectPoster_Click(sender As System.Object, e As System.EventArgs) Handles tsm_TvSelectPoster.Click
		Try
			TvSelectPosterBanner(False)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsm_TvScrapeBanner_Click(sender As System.Object, e As System.EventArgs) Handles tsm_TvScrapeBanner.Click
		Try
			TvScrapePosterBanner("series")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub tsm_TvSelectBanner_Click(sender As System.Object, e As System.EventArgs) Handles tsm_TvSelectBanner.Click
		Try
			TvSelectPosterBanner(True)
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Private Sub ReloadHtmlTemplatesToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ReloadHtmlTemplatesToolStripMenuItem.Click
		Dim mediaDropdown As New SortedList(Of String, String)
		mediaInfoExp.addTemplates(mediaDropdown)
		ExportMovieListInfoToolStripMenuItem.DropDownItems.Clear()
		ExportTVShowInfoToolStripMenuItem.DropDownItems.Clear()
		For Each item In mediaDropdown
			If item.Value = MediaInfoExport.mediaType.Movie Then
				ExportMovieListInfoToolStripMenuItem.DropDownItems.Add(item.Key)
			ElseIf item.Value = MediaInfoExport.mediaType.TV Then
				ExportTVShowInfoToolStripMenuItem.DropDownItems.Add(item.Key)
			End If
		Next
	End Sub

	Private Sub FixNFOCreateDateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FixNFOCreateDateToolStripMenuItem.Click
		Dim fixCreateDate As New frmCreateDateFix
		If Pref.MultiMonitoEnabled Then
			Dim w As Integer = fixCreateDate.Width
			Dim h As Integer = fixCreateDate.Height
			fixCreateDate.Bounds = Screen.AllScreens(CurrentScreen).Bounds
			fixCreateDate.StartPosition = FormStartPosition.Manual
			fixCreateDate.Width = w
			fixCreateDate.Height = h
		End If
		fixCreateDate.ShowDialog()
	End Sub

	Private Sub tsmicacheclean_Click(sender As Object, e As EventArgs) Handles tsmicacheclean.Click
        If Not CheckForRunningBackgroundworker Then
			messbox = New frmMessageBox("Emptying Cache & Series Folders", , "   Please Wait.   ")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			CleanCacheFolder(True, True)
			messbox.Close()
		End If
	End Sub

	Private Sub tsmiCleanCacheOnly_Click(sender As Object, e As EventArgs) Handles tsmiCleanCacheOnly.Click
        If Not CheckForRunningBackgroundworker Then
			messbox = New frmMessageBox("Emptying Cache Folders", , "   Please Wait.   ")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			CleanCacheFolder(, True)
			messbox.Close()
		End If
	End Sub

	Private Sub tsmiCleanSeriesonly_Click(sender As Object, e As EventArgs) Handles tsmiCleanSeriesonly.Click
        If Not CheckForRunningBackgroundworker Then
			messbox = New frmMessageBox("Emptying Series Folder", , "   Please Wait.   ")
			messbox.Show()
			messbox.Refresh()
			Application.DoEvents()
			ClearSeriesFolder()
			messbox.Close()
		End If
	End Sub

	Private Sub RefreshGenreListboxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshGenreListboxToolStripMenuItem.Click
		GenreMasterLoad()
		util_GenreLoad()
	End Sub

	Private Sub ExportLibraryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportLibraryToolStripMenuItem.Click
		Dim frmxport As New frmXbmcExport
		If Pref.MultiMonitoEnabled Then
			Dim w As Integer = frmxport.Width
			Dim h As Integer = frmxport.Height
			frmxport.Bounds = Screen.AllScreens(CurrentScreen).Bounds
			frmxport.StartPosition = FormStartPosition.Manual
			frmxport.Width = w
			frmxport.Height = h
		End If
		frmxport.ShowDialog()
	End Sub

#End Region 'ToolStrip Menus misc


	Public Shared Function VidMediaFlags(ByVal Vidfiledetails As StreamDetails, Optional ByVal Is3d As Boolean = False, Optional ByVal size As Integer = -1) As List(Of KeyValuePair(Of String, String))
		Dim flags As New List(Of KeyValuePair(Of String, String))
		Try
			Dim tracks = If(Pref.ShowAllAudioTracks, Vidfiledetails.Audio, From x In Vidfiledetails.Audio Where x = Vidfiledetails.DefaultAudioTrack)

			For Each track In tracks
				flags.Add(New KeyValuePair(Of String, String)("channels" + GetNotDefaultStr(track = Vidfiledetails.DefaultAudioTrack), GetNumAudioTracks(track.Channels.Value)))
				flags.Add(New KeyValuePair(Of String, String)("audio" + GetNotDefaultStr(track = Vidfiledetails.DefaultAudioTrack), track.Codec.Value))
				flags.Add(New KeyValuePair(Of String, String)("lang" + GetNotDefaultStr(track = Vidfiledetails.DefaultAudioTrack), track.Language.Value))
			Next


			flags.Add(New KeyValuePair(Of String, String)("aspect", Utilities.GetStdAspectRatio(Vidfiledetails.Video.Aspect.Value)))
			flags.Add(New KeyValuePair(Of String, String)("codec", Utilities.GetCodecCommonName(GetMasterCodec(Vidfiledetails.Video))))  '.Codec.Value.RemoveWhitespace)))
			flags.Add(New KeyValuePair(Of String, String)("resolution", If(Vidfiledetails.Video.VideoResolution < 0, "", Vidfiledetails.Video.VideoResolution.ToString)))
			flags.Add(New KeyValuePair(Of String, String)("special", If(Is3d, "3d", "")))
            flags.Add(New KeyValuePair(Of String, String)("folderSize", size.ToString))
            flags.Add(New KeyValuePair(Of String, String)("NumVideoBits", Vidfiledetails.Video.NumVideoBits.ToString))

			Dim subtitles = If(Not Pref.DisplayDefaultSubtitleLang, Nothing, If(Pref.DisplayAllSubtitleLang, Vidfiledetails.Subtitles, From x In Vidfiledetails.Subtitles Where x = Vidfiledetails.DefaultSubTrack))

			If Not IsNothing(subtitles) Then
				For Each subtitle In subtitles  
                    If subtitle.Forced Then Continue For        'Skip displaying forced language on overlay.
					flags.Add(New KeyValuePair(Of String, String)("sublang", subtitle.Language.Value))
				Next
			End If
		Catch
		End Try
		Return flags

	End Function

	Private Shared Function GetNotDefaultStr(dflt)
		If dflt Then
			Return ""
		End If

		Return "_notdefault"
	End Function

	Private Shared Function GetNumAudioTracks(AudCh)
		Return If((AudCh = "" Or AudCh = "-1"), "0", Regex.Match(AudCh, "\d+").Value)
	End Function

	Private Shared Function GetMasterCodec(strmdata As VideoDetails)
		Dim codec As String = strmdata.Codec.Value.RemoveWhitespace
		Dim format As String = strmdata.FormatInfo.Value.RemoveWhitespace
		Return If(codec = "h264" AndAlso format = "avc1", format, codec)
	End Function
    
	Private Sub tsmiMov_SyncToXBMC_Click(sender As Object, e As EventArgs) Handles tsmiMov_SyncToXBMC.Click
		Try
			Call mov_ScrapeSpecific("Xbmc_Sync")
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

#Region "Functions"

	Private Function AssignClipboardImage(picBox As PictureBox) As Boolean
		Try
			If Clipboard.GetDataObject.GetDataPresent(DataFormats.FileDrop) Then
				Dim pth As String = CType(Clipboard.GetData(DataFormats.FileDrop), Array).GetValue(0).ToString
				Dim FInfo As FileInfo = New FileInfo(pth)
				If FInfo.Extension.ToLower() = ".jpg" Or FInfo.Extension.ToLower() = ".tbn" Or FInfo.Extension.ToLower() = ".bmp" Or FInfo.Extension.ToLower() = ".png" Then
					util_ImageLoad(picBox, pth, Utilities.DefaultPosterPath)
					Return True
				Else
					MessageBox.Show("Not a picture")
				End If
			End If

			If Clipboard.GetDataObject.GetDataPresent(DataFormats.Bitmap) Then
				Dim createfilename As String = Format(System.DateTime.Now, "yyyyMMddHHmmss").ToString
				createfilename = Utilities.CacheFolderPath & createfilename & ".jpg"
				Dim oImgObj As System.Drawing.Image = Clipboard.GetDataObject.GetData(DataFormats.Bitmap, True)
				oImgObj.Save(createfilename, System.Drawing.Imaging.ImageFormat.Jpeg)
				util_ImageLoad(picBox, createfilename, Utilities.DefaultPosterPath)
				Return True
			End If
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try

		Return False
	End Function

	Public Function CheckForNewVersion() As String

		'Dim MC_Version_RegEx = "<th><span class=""rating_header"">current</span></th>.*?<td>[\s]+.*?([0-9]*\.?[0-9]+).*?[\s]+</td>"
        Dim MCSF_Version_RegEx = "title=""MC ([0-9]*\.?[0-9]+)"

		Dim s As New Classimdb
        Dim HOME_PAGE2 As String = "https://sourceforge.net/projects/mediacompanion/files/"
		'Dim html As String = s.loadwebpage(Pref.proxysettings, HOME_PAGE, True, 10).ToString
        Dim html As String = s.loadwebpage(Pref.proxysettings, SF_Home_Page & "files/", True, 10).ToString

		'Dim m = Regex.Match(html, MC_Version_RegEx, RegexOptions.Singleline)
        Dim n = Regex.Match(html, MCSF_Version_RegEx, RegexOptions.Singleline)

        'Dim displayVersion As String = m.Groups(1).Value.Trim
		Dim displayVersion As String = n.Groups(1).Value.Trim
		Dim latestVersion As String = displayVersion.Replace(".", "")
		Dim currVersion As String = Trim(System.Reflection.Assembly.GetExecutingAssembly.FullName.Split(",")(1)).Replace(".", "").Replace("Version=", "")

		If latestVersion.Length < 4 Then Return Nothing
		If latestVersion <> currVersion Then Return displayVersion

		Return Nothing
	End Function

#End Region

	Private Sub GoogleSearch(ByVal search As String)
		'Open Webpage at Google image search for movietitle & year
		Dim title As String = workingMovieDetails.fullmoviebody.title
		Dim year As String = workingMovieDetails.fullmoviebody.year

		Dim url As String = ""
		If Not IsNothing(year) AndAlso IsNumeric(year) AndAlso year.Length = 4 Then
			url = "http://images.google.com/images?q=" & title & "+" & year & search
		Else
			url = "http://images.google.com/images?q=" & title & search
		End If
		OpenUrl(url)
	End Sub

	Public Function GetlistofPlots() As String
		messbox = New frmMessageBox("Gathering plots", , "     Please Wait.     ")
		messbox.Show()
		messbox.Refresh()
		Dim ListofPlots As New List(Of String)
		Dim _imdbScraper As New Classimdb
		Dim tmdb As New TMDB
		tmdb.Imdb = workingMovieDetails.fullmoviebody.imdbid
		tmdb.TmdbId = workingMovieDetails.fullmoviebody.tmdbid
		Dim tmdbplot As String = Nothing
		Try
			tmdbplot = tmdb.Movie.overview
		Catch
			tmdbplot = Nothing
		End Try
		ListofPlots = _imdbScraper.GetImdbPlots(workingMovieDetails.fullmoviebody.imdbid)
		If Not IsNothing(tmdbplot) AndAlso tmdbplot <> "" AndAlso Not ListofPlots.Contains(tmdbplot) Then ListofPlots.Add(tmdbplot)
		messbox.Close()
		If ListofPlots.Count < 2 Then
			MsgBox("No Extra Plots found for this movie", MsgBoxStyle.Exclamation)
			Return Nothing
		End If
		Dim plotfrm As New frmmovieplotlist(ListofPlots)
		plotfrm.ShowDialog
		If plotfrm.DialogResult = Windows.Forms.DialogResult.Cancel Then
			plotfrm.Dispose()
			Return Nothing
		End If
		Dim plotselected As Integer = plotfrm.cmbxplotnumber.SelectedIndex
		plotfrm.Dispose()
		Return ListofPlots(plotselected)
	End Function
    
	Private Sub TSMI_AboutMC_Click(sender As Object, e As EventArgs) Handles TSMI_AboutMC.Click
		Dim txt As String
		txt = "Media Companion.  Designed by William Adamson in 2008"
		txt &= vbCrLf & "" & vbCrLf & "OpenSourced in December 2010"
		txt &= vbCrLf & "" & vbCrLf & "Worked on by billyad2000,  EvLSnoopY,  FreddyKrueger,  StormyKnight,  Playos,  HueyHQ,  AnotherPhil,  anand,  vbat99 and many more"
		txt &= vbCrLf & vbCrLf & If(Environment.Is64BitProcess, "64bit build", "32bit build")
		Dim abtfrm As New frmSplashscreen
		Dim scrn As Integer = splashscreenread()
		Dim MClocation As Point
		abtfrm.StartPosition = FormStartPosition.Manual
		MClocation = Me.Location
		abtfrm.Location = New Point(MClocation.X + 50, MClocation.Y + 50)

		abtfrm.Label1.Visible = False
		abtfrm.Label3.Visible = False
		abtfrm.lbl_about.Text = txt
		abtfrm.lbl_about.Visible = True
		abtfrm.allowlostfocus = True
		abtfrm.TopMost = True
		abtfrm.Show()
		Do Until abtfrm.Cancelled
			Application.DoEvents()
		Loop
		abtfrm.Close()
	End Sub

	Private Sub DataGridViewMovies_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewMovies.SelectionChanged

	End Sub

	Private Sub Tmr_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tmr.Tick
		Dim hFb As IntPtr = FindWindowW("#32770", "Browse For Folder") '#32770 is the class name of a folderbrowser dialog
		If hFb <> IntPtr.Zero Then
			If SendMessageW(hFb, BFFM_SETEXPANDED, 1, fb.SelectedPath) = IntPtr.Zero Then
				Tmr.Stop()
			End If
		End If
	End Sub

	Private Sub XBMCTMDBConfigSave()
		If Not Pref.XbmcTmdbScraperRatings = Nothing Then
			Save_XBMC_TMDB_Scraper_Config("fanart"          , Pref.XbmcTmdbScraperFanart)
			Save_XBMC_TMDB_Scraper_Config("trailerq"        , Pref.XbmcTmdbScraperTrailerQ)
			Save_XBMC_TMDB_Scraper_Config("language"        , Pref.XbmcTmdbScraperLanguage)
			Save_XBMC_TMDB_Scraper_Config("ratings"         , Pref.XbmcTmdbScraperRatings)
			Save_XBMC_TMDB_Scraper_Config("tmdbcertcountry" , Pref.XbmcTmdbScraperCertCountry)
		End If
	End Sub

	Private Sub XBMCTVDBConfigSave()
		Save_XBMC_TVDB_Scraper_Config("dvdorder"        , Pref.XBMCTVDbDvdOrder)
		Save_XBMC_TVDB_Scraper_Config("absolutenumber"  , Pref.XBMCTVDbAbsoluteNumber)
		Save_XBMC_TVDB_Scraper_Config("fanart"          , Pref.XBMCTVDbFanart)
		Save_XBMC_TVDB_Scraper_Config("posters"         , Pref.XBMCTVDbPoster)
		Save_XBMC_TVDB_Scraper_Config("language"        , Pref.XBMCTVDbLanguage)
		Save_XBMC_TVDB_Scraper_Config("ratings"         , Pref.XBMCTVDbRatings)
		Save_XBMC_TVDB_Scraper_Config("fallback"        , Pref.XBMCTVDbfallback)
	End Sub


	Sub SetTagTxtField()
		tagtxt.ReadOnly = Not Pref.AllowUserTags
		If Pref.AllowUserTags Then
			tagtxt.BackColor = Nothing
			tagtxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Else
			tagtxt.BackColor = System.Drawing.SystemColors.Control
			tagtxt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		End If
	End Sub

	Public ReadOnly Property ImgBwCancelled As Boolean
		Get
			Application.DoEvents()
			Return ImgBw.CancellationPending
		End Get
	End Property

	Sub imgbw_DoWork(ByVal snder As Object, ByVal e As DoWorkEventArgs) Handles ImgBw.DoWork
		Statusstrip_Enable()
		Dim listpicbox As New List(Of FanartPicBox)
		listpicbox.AddRange(e.Argument(0))      '{image list, Start image number, total number of images, picture panel}
		Dim count As Integer = e.Argument(1)
		Dim Total As Integer = e.Argument(2)
		BWs.Clear()
		Dim totalcount As Integer = count
		If Total = 0 Then Total = listpicbox.Count
		NumActiveThreads = 0
		For Each item In listpicbox
			Dim bw As BackgroundWorker = New BackgroundWorker
			bw.WorkerSupportsCancellation = True

			AddHandler bw.DoWork, AddressOf bw_DoWork
			AddHandler bw.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted

			BWs.Add(bw)
			NumActiveThreads += 1

			bw.RunWorkerAsync(item)
			totalcount += 1
			
			If NumActiveThreads > 2 Then
				Do Until NumActiveThreads < 1
					If ImgBwCancelled Then
                        bw.CancelAsync()
                        Do Until Not bw.IsBusy
                            Application.DoEvents()
                        Loop
						Exit Do
					End If
				Loop
			End If
            ImgBw.ReportProgress(0, "Press ""Esc"" to Cancel:   Downloading image: " & totalcount & " of " & Total)
			If ImgBwCancelled Then
                Do Until Not bw.IsBusy
                    Application.DoEvents()
                Loop
				Exit For
			End If
		Next
	End Sub

	Sub ImgBw_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles ImgBw.ProgressChanged
		ToolStripStatusLabel2.Text = e.UserState
	End Sub

	Sub ImgBw_RunWorkerComplete(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ImgBw.RunWorkerCompleted
		Imageloading = True
        tsStatusLabel.Text = ""
		ToolStripStatusLabel2.Text = "TV Show Episode Scan In Progress"
		ToolStripStatusLabel2.Visible = False
		Statusstrip_Enable(False)
		Imageloading = False
	End Sub


	Sub bw_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
		If Not ImgBwCancelled Then
			Dim item As FanartPicBox = DirectCast(e.Argument, FanartPicBox)

			e.Result = util_ImageLoad2(item.pbox, item.imagepath, "")
		End If
	End Sub

	Private Sub bw_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
		Threading.Monitor.Enter(Me)
		NumActiveThreads -= 1
		Threading.Monitor.Exit(Me)
	End Sub

	' We need to load images in this way so that they remain unlocked by the OS so we can update the fanart/poster files as needed
	Public Function util_ImageLoad2(ByVal PicBox As PictureBox, ByVal ImagePath As String, ByVal DefaultPic As String) As Boolean
		Dim PathToUse As String = DefaultPic
		Dim cachename As String = ""
		PicBox.Tag = Nothing
		If ImgBwCancelled Then Exit Function
		If Utilities.UrlIsValid(ImagePath) Then
			cachename = Utilities.Download2Cache(ImagePath)
			If cachename <> "" Then PathToUse = cachename
		ElseIf File.Exists(ImagePath) Then
			PathToUse = ImagePath
		End If
		If PathToUse = "" Then
			PicBox.Image = Nothing
			Exit Function
		End If
		If ImgBwCancelled Then Exit Function
		Try
			Using fs As IO.Filestream = IO.File.Open(PathToUse, IO.FileMode.Open, IO.FileAccess.Read), ms As IO.MemoryStream = New IO.MemoryStream()
				fs.CopyTo(ms)
				ms.Seek(0, IO.SeekOrigin.Begin)
				PicBox.Image = Image.FromStream(ms)
			End Using
			PicBox.Tag = PathToUse
		Catch
			'Image is invalid e.g. not downloaded correctly -> Delete it
			Try
				File.Delete(PathToUse)
			Catch
			End Try
			If ImgBwCancelled Then Exit Function
			Try
				Using fs As IO.FileStream = IO.File.Open(DefaultPic, IO.FileMode.Open, IO.FileAccess.Read), ms As IO.MemoryStream = New IO.MemoryStream()
					fs.CopyTo(ms)
					ms.Seek(0, IO.SeekOrigin.Begin)
					PicBox.Image = Image.FromStream(ms)
				End Using
				PicBox.ImageLocation = DefaultPic
			Catch
				Return False
			End Try
			Return True
		End Try

		Return True
	End Function

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	Private Sub Statusstrip_Enable(Optional ByVal Enable As Boolean = True)
		If Enable Then
			StatusStrip1.BackColor = Color.Honeydew
			StatusStrip1.Visible = True
			tsStatusLabel1.Visible = False
		Else
            If CheckForRunningBackgroundworker Then Exit Sub
			StatusStrip1.Visible = Not Pref.AutoHideStatusBar
			StatusStrip1.BackColor = Color.LightGray
			tsStatusLabel1.Visible = True
		End If
	End Sub

	Private Sub tagtxt_TextChanged(sender As Object, e As EventArgs) Handles tagtxt.TextChanged
		tb_tagtxt_changed = True
	End Sub
    
	Public Function cmsMissingMovies() As ContextMenuStrip
		Dim cms = New ContextMenuStrip
		Dim tsmiMissingMovie = New ToolStripMenuItem("Open TMDb Movie Info Page", My.Resources.TheMovieDB, AddressOf tsmiMissingMovie_Click)
		Dim tsmiIncompleteMovieSet = New ToolStripMenuItem("Open TMDb Set Info Page", My.Resources.TheMovieDB, AddressOf tsmiMissingSet_Click)
		cms.Items.Add(tsmiMissingMovie)
		cms.Items.Add(tsmiIncompleteMovieSet)
		Return cms
	End Function
    
	Private Sub tsmiMissingMovie_Click(ByVal sender As Object, ByVal e As EventArgs)
		Dim tmdbid As String = DataGridViewMovies.SelectedRows(0).Cells("tmdbid").Value
		OpenUrl(TMDB_MOVIE_URL & tmdbid)
	End Sub

	Private Sub tsmiMissingSet_Click(ByVal sender As Object, ByVal e As EventArgs)
		Dim msi As MovieSetInfo = DataGridViewMovies.SelectedRows(0).Cells("movieset").Value
		OpenUrl(TMDB_SET_URL & msi.TmdbSetId)
	End Sub
    
	Private Sub tsmiMov_ViewMovieDbMoviePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_ViewMovieDbMoviePage.Click
		OpenUrl(TMDB_MOVIE_URL & workingMovie.tmdbid)
	End Sub
    
	Private Sub tsmiMov_ViewMovieDbSetPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmiMov_ViewMovieDbSetPage.Click
		OpenUrl(TMDB_SET_URL & workingMovie.TmdbSetId)
	End Sub

    Public Function CheckForRunningBackgroundworker() As Boolean
        Return BckWrkScnMovies.IsBusy OrElse Bckgrndfindmissingepisodes.IsBusy OrElse bckgroundscanepisodes.IsBusy OrElse tvbckrescrapewizard.IsBusy OrElse ImgBw.IsBusy OrElse BckWrkTv.IsBusy OrElse BckWrkSeries.IsBusy
    End Function
    
    Public Sub doTestTvdb()
        'tvseriestoget = New List(Of str_TVSeries)
        'tvseriestoget.Add(New TvSeriesData("281534", "en"))
        'tvseriestoget.Add(New TvSeriesData("72546", "en"))
        'Call GetSeriesData()

        Dim tvdb As New TVDBScraper2("72546", "en")
        'Dim languages As TheTvDB.TvdbLanguagesResult = tvdb.GetTvdbLanguages
        Dim episodes As New List(Of TheTvDB.TvdbEpisode)
        Dim alldata As New TheTvDB.TvdbSeries
        alldata = tvdb.Series
        alldata = tvdb.ReturnSeriesandEpisodes
        'nfoFunction.SeriesXMLSave(alldata, "c:\Temp\Test.xml")
        'Dim Series As New Tvdb.Series
        'Series.AbsorbTvSeries(alldata)
        'Dim episodes As New List(Of Tvdb.Episode)
        'For each epi In alldata.Episodes
        '    Dim episode As New Tvdb.Episode
        '    episode.AbsorbEpisode(epi)
        '    episodes.Add(episode)
        'Next
        'Dim doc As New XmlDocument
        'Dim xmlProc As XmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        ' tvdb.LookupLang = Pref.TvdbLanguageCode
        ' tvdb.TvdbId = "78874" '"281534" '"72546" 
        'tvdb.AllEpDetails = False
        'episodes = tvdb.Episodes
        'Dim episode1 As TheTvdb.TvdbEpisode = tvdb.LoadEpisodeDetails(episodes(2).Identity, "en")
        
        tvdb.Title = "Vikings"
        
    End Sub

End Class