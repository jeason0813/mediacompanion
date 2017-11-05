Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports Alphaleonis.Win32.Filesystem
Imports System.Net
Imports System.Threading
Imports System.Xml
Imports Media_Companion
Imports System.Linq

Public Class TVDBScraper
    Const SetDefaults                   As Boolean = True
    Const msg_append                    As Progress.Commands = Progress.Commands.Append
    Const MSG_PREFIX                    As String = ""
    Const MSG_OK                        As String = "-OK "
    Public Const MSG_ERROR              As String = "-ERROR! "
    Dim nfoFunction                     As New WorkingWithNfoFiles
    Private _possibleTvdb               As String = String.Empty
    Private _folder                     As String
    Private _lang                       As String
    Private _isepisodes                 As String
    Private newEpisodeList              As New List(Of TvEpisode)
    Private Previoustvdbid              As String = ""
    Public NewShow                      As New TvShow
    Public Property TvBw                As BackgroundWorker = Nothing
    Public Property PercentDone         As Integer = -1
    Public Property ProgressStart       As String = String.Empty
    Public Property ListOfShows         As New List(Of TvShow)
    Public Property EpForceSearch       As Boolean = False
    Public Property SingleShow          As Boolean = False
    Public Property SingleShowid        As String = Nothing
    Public Property tvBatchList         As New str_TvShowBatchWizard(SetDefaults)
    Property tvdb                       As TVDBScraper2
    Property Actions                    As New ScrapeActions
    Property Scraped                    As Boolean = False
    Property TimingsLog                 As String = ""
    Property TimingsLogThreshold        As Integer = Pref.ScrapeTimingsLogThreshold
    Property LogScrapeTimes             As Boolean = Pref.LogScrapeTimes

    Public ReadOnly Property PossibleTVdb As String
        Get
            Return _possibleTvdb
        End Get
    End Property

    Public Delegate Sub ActionDelegate()

    Class ScrapeActions
        Property Items As New List(Of ScrapeAction)
    End Class

    Class ScrapeAction
        Property Action As ActionDelegate
        Property ActionName As String
        Property Time As New Times

        Sub New(ByVal action As ActionDelegate, actionName As String)
            Me.Action = action
            Me.ActionName = actionName
        End Sub

        Sub Run
            Time.StartTm = DateTime.Now
            _Action
            Time.EndTm = DateTime.Now
        End Sub
    End Class

    Public ReadOnly Property Cancelled As Boolean
        Get
            Application.DoEvents()

            If Not IsNothing(_TvBw) AndAlso _TvBw.WorkerSupportsCancellation AndAlso _TvBw.CancellationPending Then
                ReportProgress("Cancelled!", vbCrLf & "!!! Operation cancelled by user", msg_append)
                Return True
            End If
            Return False
        End Get
    End Property

    Sub New(Optional tvbw As BackgroundWorker = Nothing)
        _TvBw = tvbw
    End Sub

    Sub ReportProgress(Optional progressText As String = Nothing, Optional log As String = Nothing, Optional command As Progress.Commands = Progress.Commands.SetIt)
        ReportProgress(New Progress(progressText, log, command))
    End Sub
    
    Sub ReportProgress(ByVal oProgress As Progress)
        If Not IsNothing(_TvBw) AndAlso _TvBw.WorkerReportsProgress AndAlso Not (String.IsNullOrEmpty(oProgress.Log) And String.IsNullOrEmpty(oProgress.Message)) Then
            Try
                _TvBw.ReportProgress(PercentDone, oProgress)
            Catch
            End Try
        End If
    End Sub

    Sub Reportprogress(ByVal tvshow As TvShow)
        If Not IsNothing(_TvBw) AndAlso _TvBw.WorkerReportsProgress AndAlso Not IsNothing(NewShow) Then
            Try
                _TvBw.ReportProgress(9999, NewShow)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Sub Reportprogress(ByVal NewEpisode As TvEpisode)
        If Not IsNothing(_TvBw) AndAlso _TvBw.WorkerReportsProgress AndAlso Not IsNothing(NewEpisode) Then
            Try
                _TvBw.ReportProgress(999, NewEpisode)
            Catch ex As Exception

            End Try
        End If
    End Sub

#Region "Episode Actions"
    Private Sub AppendEpisodeScrapeSuccessActions()
        Actions.Items.Add(New ScrapeAction(AddressOf EPPopulateSeriesInfo   , "Apply Series info to found Episodes"     ))
        Actions.Items.Add(New ScrapeAction(AddressOf EPScrape               , "Scrape found Episodes"                   ))
        'Actions.Items.Add(New ScrapeAction(AddressOf SeriesScraper_GetBody      , "Scrape TVDb Series Main Body"            ))
        'Actions.Items.Add(New ScrapeAction(AddressOf SeriesCheckTVDbBodyScrape  , "Checking TVDb Series Main body scrape"   ))
    End Sub

    Private Sub AppendEpisodeScrapeFailedActions()
        'Actions.Items.Add(New ScrapeAction(AddressOf SeriesScraper_GetBody      , "Scrape TVDb Series Main Body"            ))
        'Actions.Items.Add(New ScrapeAction(AddressOf SeriesCheckTVDbBodyScrape  , "Checking TVDb Series Main body scrape"   ))
    End Sub

    Private Sub AppendEpisodeScrapeSpecific()
        Actions.Items.Add(New ScrapeAction(AddressOf EPSearchNew            , "Search Series for new Episodes"          ))
        Actions.Items.Add(New ScrapeAction(AddressOf EPCheckifContinue      , "Check ready to scrape and continue"      ))
    End Sub

#End Region

#Region "Series Actions"
    Private Sub AppendSeriesScrapeSuccessActions
        Actions.Items.Add(New ScrapeAction(AddressOf DoRenameTVFolders          , "Rename Folders"          ))
        Actions.Items.Add(New ScrapeAction(AddressOf GetTVActors                , "Actors scraper"          ))
        Actions.Items.Add(New ScrapeAction(AddressOf TVTidyUpAnyUnscrapedFields , "Tidy up unscraped fields"))
        Actions.Items.Add(New ScrapeAction(AddressOf SaveTVNFO                  , "Save Nfo"                ))
        Actions.Items.Add(New ScrapeAction(AddressOf DownloadTvArt              , "Download Series Art"     ))
        Actions.Items.Add(New ScrapeAction(AddressOf TvAddToCache               , "Updating caches"         ))
        Actions.Items.Add(New ScrapeAction(AddressOf TvLoadExistingEpisodes     , "Load Existing Episodes"  ))
        'Actions.Items.Add(New ScrapeAction(AddressOf DownloadTVFanart           , "Fanart download"          ))
        'Actions.Items.Add(New ScrapeAction(AddressOf DownloadTVArtFromFanartTv  , "Fanart.Tv download"       ))
        'Actions.Items.Add(New ScrapeAction(AddressOf DownloadTVExtraFanart      , "Extra Fanart download"    ))
        'Actions.Items.Add( New ScrapeAction(AddressOf AssignHdTags              , "Assign HD Tags"            ))
        'Actions.Items.Add( New ScrapeAction(AddressOf GetKeyWords               , "Get Keywords for tags"     ))
        'Actions.Items.Add( New ScrapeAction(AddressOf DoRenameFiles             , "Rename Files"              ))
    End Sub

    Sub AppendSeriesScrapeFailedActions
        Actions.Items.Add(New ScrapeAction(AddressOf TVTidyUpAnyUnscrapedFields , "Tidy up unscraped fields"    ))
        Actions.Items.Add(New ScrapeAction(AddressOf SaveTVNFO                  , "Save Nfo"                    ))
        Actions.Items.Add(New ScrapeAction(AddressOf TvAddToCache               , "Updating caches"             ))
    End Sub

    Sub AppendSeriesScraperSpecific
        Actions.Items.Add(New ScrapeAction(AddressOf SeriesScraper_GetBody      , "Scrape TVDb Series Main Body"            ))
        Actions.Items.Add(New ScrapeAction(AddressOf SeriesCheckTVDbBodyScrape  , "Checking TVDb Series Main body scrape"   ))
    End Sub
#End Region

    Sub IniTVdb
        IniTVdb(PossibleTVdb)
    End Sub

    Sub IniTVdb(tvdbid As String)
        tvdb = New TVDBScraper2(tvdbid)
    End Sub

    Sub Scrape(args As TvdbArgs) ' As String)
        _possibleTvdb = args.tvdbid
        _folder = args.folder
        _isepisodes = args.episode
        _lang = args.lang
        Scrape
    End Sub

    Sub Scrape
        If Not Scraped Then
            Scraped = True
            'General
            Actions.Items.Add(New ScrapeAction(AddressOf IniTVdb, "Initialising TMDb"))

            If _isepisodes Then
                AppendEpisodeScrapeSpecific
            Else
                AppendSeriesScraperSpecific
            End If
            RunScrapeActions
        End If
    End Sub

    Sub RunScrapeActions
        While Actions.Items.Count > 0
            Dim action = Actions.Items(0)
            Try
                action.Run

                If LogScrapeTimes And action.Time.ElapsedTimeMs > TimingsLogThreshold Then
                    TimingsLog &= vbCrLf & "[" & action.ActionName & "] took " & action.Time.ElapsedTimeMs & "ms to run"
                End If

                Actions.Items.RemoveAt(0)
                If Cancelled Then Exit Sub
            Catch ex As Exception
                If ex.Message.ToString.ToLower.Contains("offline") Then
                    ReportProgress(, "!!! Error - Running action [" & action.ActionName & "] threw [" & ex.Message.ToString & "]" & vbCrLf)
                    Actions.Items.RemoveAt(0)
                Else
                    ReportProgress(MSG_ERROR, "!!! Error - Running action [" & action.ActionName & "] threw [" & ex.Message.ToString & "]" & vbCrLf)
                    Actions.Items.Clear
                End If
            End Try
        End While
        ReportProgress(, vbCrLf & vbCrLf)
    End Sub
    
#Region "Series routines"
    Sub DoRenameTVFolders
        'Not assigned code yet.
    End Sub

    Sub GetTVActors
        If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Or NewShow.ImdbId.Value = Nothing Then
            NewShow.TvShowActorSource.Value = "tvdb"
            ReportProgress("TVDb Actors ", "Scraping actors from TVDb" & vbCrLf, msg_append)
            If TvGetActorTvdb(NewShow) Then
                Reportprogress(MSG_OK, MSG_OK & vbCrLf, msg_append)
            Else
                Reportprogress("-failed ", "-failed" & vbCrLf, msg_append)
            End If
        End If
        If (Pref.TvdbActorScrape = 1 Or Pref.TvdbActorScrape = 2) And NewShow.ImdbId.Value <> Nothing Then
            NewShow.TvShowActorSource.Value = "imdb"
            ReportProgress("IMDB Actors ", "Scraping actors from IMDb" & vbCrLf, msg_append)
            If TvGetActorImdb(NewShow) Then
                Reportprogress(MSG_OK, MSG_OK & vbCrLf, msg_append)
            Else
                Reportprogress("-failed ", "-failed" & vbCrLf, msg_append)
            End If
        End If
    End Sub

    Function TvGetActorTvdb(ByRef NewSeries As TvShow) As Boolean
        Dim success As Boolean = True
        Dim actors As List(Of str_MovieActors) = tvdb.Cast
        Dim actorpaths As New List(Of String)
        Dim workingpath As String = ""
        Dim tempstring As String = ""
        If Pref.actorseasy AndAlso Not Pref.tvshowautoquick Then
            workingpath = NewSeries.NfoFilePath.Replace(Path.GetFileName(NewSeries.NfoFilePath), "") & ".actors\"
            Utilities.EnsureFolderExists(workingpath)
        End If
        For Each NewAct In actors
            actorpaths.Clear()
            If Pref.ExcludeActorNoThumb AndAlso String.IsNullOrEmpty(NewAct.actorthumb) Then Continue For
            Dim id As String = If(NewAct.actorid = Nothing, "", NewAct.actorid)
            Dim results As XmlNode = Nothing
            Dim filename As String = Utilities.cleanFilenameIllegalChars(NewAct.actorname)
            filename = filename.Replace(" ", "_")
            If Not String.IsNullOrEmpty(NewAct.actorthumb) And NewAct.actorthumb <> "http://thetvdb.com/banners/" Then
                Dim actorurl As String = NewAct.actorthumb
                'Save to .actor folder
                If Pref.actorseasy = True And Pref.tvshowautoquick = False Then
                    If NewSeries.TvShowActorSource.Value <> "imdb" Or NewSeries.ImdbId = Nothing Then
                        Dim ActorFilename As String = Path.Combine(workingpath, filename)
                        If Pref.FrodoEnabled Then actorpaths.Add(ActorFilename & ".jpg")
                        If Pref.EdenEnabled Then actorpaths.Add(ActorFilename & ".tbn")
                    End If
                End If

                'Save to Local actor folder
                If Pref.actorsave = True And id <> "" Then 'Allow Local folder save, separate from .actor folder saving 
                    Dim workingpath2 As String = ""
                    Dim networkpath As String = Pref.actorsavepath
                    filename = filename & "_" & id
                    workingpath2 = networkpath & "\" & filename.Substring(0, 1) & "\" & filename
                    If Pref.FrodoEnabled Then actorpaths.Add(workingpath2 & ".jpg")
                    If Pref.EdenEnabled Then actorpaths.Add(workingpath2 & ".tbn")
                    NewAct.actorthumb = actorpaths(0)
                    If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                        NewAct.actorthumb = actorpaths(0).Replace(networkpath, Pref.actornetworkpath).Replace("\", "/")
                    ElseIf Pref.actornetworkpath.IndexOf("\") <> -1
                        NewAct.actorthumb = actorpaths(0).Replace(networkpath, Pref.actornetworkpath).Replace("/", "\")
                    End If
                    DoDownloadActorImage(actorurl, actorpaths)
                End If
            End If
            Dim exists As Boolean = False
            NewSeries.ListActors.Add(NewAct)
        Next
        Return success
    End Function

    Function TvGetActorImdb(ByRef NewSeries As TvShow) As Boolean
        Dim imdbscraper As New Classimdb
        Dim success As Boolean = False
        If String.IsNullOrEmpty(NewSeries.ImdbId.Value) Then Return success
        Dim actorlist As List(Of str_MovieActors) = imdbscraper.GetImdbActorsList(Pref.imdbmirror, NewSeries.ImdbId.Value, Pref.maxactors)
        Dim workingpath As String = ""
        If Pref.actorseasy And Not Pref.tvshowautoquick Then
            workingpath = NewSeries.NfoFilePath.Replace(Path.GetFileName(NewSeries.NfoFilePath), "")
            workingpath = workingpath & ".actors\"
            Utilities.EnsureFolderExists(workingpath)
        End If

        Dim listofactors As List(Of str_MovieActors) = IMDbActors(actorlist, success, workingpath)

        Dim i As Integer = 0
        For Each listact In listofactors
            i += 1
            NewSeries.ListActors.Add(listact)
            If i > Pref.maxactors Then Exit For
        Next
        Return success
    End Function

    Private Function IMDbActors(ByVal actorlist As List(Of str_MovieActors), ByRef success As Boolean, ByVal workingpath As String) As List(Of str_MovieActors)
        Dim actcount As Integer = 0
        Dim totalactors As New List(Of str_MovieActors)
        Dim actorpaths As New List(Of String)
        For Each thisresult As str_MovieActors In actorlist
            actorpaths.Clear()
            Dim actorimageurl As String = thisresult.actorthumb
            If Pref.ExcludeActorNoThumb AndAlso String.IsNullOrEmpty(thisresult.actorthumb) Then Continue For
            If Not String.IsNullOrEmpty(thisresult.actorthumb) AndAlso Not String.IsNullOrEmpty(thisresult.actorid) AndAlso actcount < (Pref.maxactors + 1) Then
                If Pref.actorseasy And Not Pref.tvshowautoquick Then
                    Dim filename As String = Utilities.cleanFilenameIllegalChars(thisresult.actorname)
                    filename = Path.Combine(workingpath, filename.Replace(" ", "_"))
                    If Pref.FrodoEnabled Then actorpaths.Add(filename & ".jpg")
                    If Pref.EdenEnabled Then actorpaths.Add(filename & ".tbn")
                End If
                If Pref.actorsave AndAlso Not Pref.tvshowautoquick Then
                    Dim tempstring As String = Pref.actorsavepath
                    Dim workingpath2 As String = ""
                    If Pref.actorsavealpha Then
                        Dim actorfilename As String = thisresult.actorname.Replace(" ", "_") & "_" & If(Pref.LocalActorSaveNoId, "", thisresult.actorid) ' & ".tbn"
                        tempstring = tempstring & "\" & actorfilename.Substring(0, 1) & "\"
                        workingpath2 = tempstring & actorfilename
                    Else
                        tempstring = tempstring & "\" & thisresult.actorid.Substring(thisresult.actorid.Length - 2, 2) & "\"
                        workingpath2 = tempstring & thisresult.actorid ' & ".tbn"
                    End If
                    If Pref.FrodoEnabled Then actorpaths.Add(workingpath2 & ".jpg")
                    If Pref.EdenEnabled Then actorpaths.Add(workingpath2 & ".tbn")
                    If Not String.IsNullOrEmpty(Pref.actornetworkpath) Then
                        If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                            thisresult.actorthumb = actorpaths(0).Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("\", "/")
                        Else
                            thisresult.actorthumb = actorpaths(0).Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("/", "\")
                        End If
                    End If
                    DoDownloadActorImage(actorimageurl, actorpaths)
                End If
            End If
            totalactors.Add(thisresult)
            success = True
            actcount += 1
        Next
        Return totalactors
    End Function

    Sub DoDownloadActorImage(ByVal url As String, actorpaths As List(Of String))
        If actorpaths.Count = 0 Then Exit Sub
        Dim cachename As String = Utilities.Download2Cache(url)
        If cachename <> "" Then
            For Each p In actorpaths
                Utilities.EnsureFolderExists(p)
                Utilities.SafeCopyFile(cachename, p, Pref.overwritethumbs)
            Next
        End If
    End Sub

    Sub TVTidyUpAnyUnscrapedFields
        If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 2 Then
            NewShow.EpisodeActorSource.Value = "tvdb"
        Else
            NewShow.EpisodeActorSource.Value = "imdb"
        End If
        NewShow.SortOrder.Value = Pref.sortorder
        GetSeriesRatings()
    End Sub

    Sub SaveTVNFO
        ReportProgress("Saving tvshow.nfo", "Saving tvshow.nfo ", msg_append)
        nfoFunction.tvshow_NfoSave(NewShow, True)
        ReportProgress(MSG_OK, MSG_OK & vbCrLf, msg_append)
    End Sub

    Sub GetSeriesRatings()
        If Pref.tvdbIMDbRating Then
            ReportProgress(" Ratings from IMDb", "Attempt to get ratings from IMDb ", msg_append)
            Dim ratingdone As Boolean = False
            Dim rating As String = ""
            Dim votes As String = ""
            If ep_getIMDbRating(NewShow.ImdbId.Value, rating, votes) Then
                If rating <> "" Then
                    ratingdone = True
                    NewShow.Rating.Value    = rating
                    NewShow.Votes.Value     = votes
                    ReportProgress(MSG_OK, MSG_OK & vbCrLf, msg_append)
                End If
            End If
            If Not ratingdone Then
                NewShow.Rating.Value    = tvdb.Rating
                NewShow.Votes.Value     = tvdb.Votes
                ReportProgress("-fallback to TVDb", "-fallback to TVDb" & vbCrLf, msg_append)
            End If
        End If
    End Sub

    Sub DownloadTvArt()
        Dim success As Boolean
        If Pref.TvFanartTvFirst Then
            If Pref.TvDlFanartTvArt OrElse Pref.TvChgShowDlFanartTvArt Then
                ReportProgress(" - Getting FanartTv Artwork", "Getting artwork from FanartTv", msg_append)
                TvFanartTvArt(NewShow, False)
                ReportProgress(": OK!", ": OK!", msg_append)
            End If
            If Pref.tvdlfanart Or Pref.tvdlposter Or Pref.tvdlseasonthumbs Then
                ReportProgress(" - Getting TVDB artwork", "Getting artwork from TVDb", msg_append)
                success = TvGetArtwork(NewShow.NfoFilePath, True, True, True, Pref.dlTVxtrafanart)
                If success Then
                    ReportProgress(": OK!", ": OK!", msg_append)
                Else
                    ReportProgress(": error!!", ": error!!", msg_append)
                End If
            End If
        Else
            If Pref.tvdlfanart Or Pref.tvdlposter Or Pref.tvdlseasonthumbs Then
                ReportProgress(" - Getting TVDB artwork", "Getting artwork from TVDb", msg_append)
                success = TvGetArtwork(NewShow.NfoFilePath, True, True, True, Pref.dlTVxtrafanart)
                If success Then
                    ReportProgress(": OK!", ": OK!", msg_append)
                Else
                    ReportProgress(": error!!", ": error!!", msg_append)
                End If
            End If
            If Pref.TvDlFanartTvArt OrElse Pref.TvChgShowDlFanartTvArt Then
                ReportProgress(" - Getting FanartTv Artwork", "Getting artwork from FanartTv", msg_append)
                TvFanartTvArt(NewShow, Pref.TvChgShowDlFanartTvArt)
                ReportProgress(": OK!", ": OK!", msg_append)
            End If
        End If
    End Sub

    Sub DownloadTVPoster
        'Not Assigned yet.
    End Sub

    Sub DownloadTVFanart
        'Not Assigned yet.
    End Sub

    Sub DownloadTVArtFromFanartTv
        'Not Assigned yet.
    End Sub

    Sub DownloadTVExtraFanart
        'Not Assigned yet.
    End Sub

    Sub TvAddToCache
        Reportprogress(NewShow)
    End Sub

    Sub SeriesScraper_GetBody
        NewShow = New TvShow
        Dim haveTVDbID As Boolean = Not String.IsNullOrEmpty(_possibleTvdb)
        NewShow.NfoFilePath = Path.Combine(_folder, "tvshow.nfo")
        NewShow.TvdbId.Value = _possibleTvdb
        NewShow.State = Media_Companion.ShowState.Unverified
        If Not haveTVDbID And NewShow.FileContainsReadableXml Then
            Dim validcheck As Boolean = nfoFunction.tv_NfoLoadCheck(NewShow.NfoFilePath)
            If validcheck Then
                NewShow = nfoFunction.tvshow_NfoLoad(NewShow.NfoFilePath)
                ReportProgress("Series '" & NewShow.Title.Value & "' found with existing tvshow.nfo", "Series '" & NewShow.Title.Value & "' found with existing tvshow.nfo - Added", msg_append)
                Actions.Items.RemoveAt(1)
                Actions.Items.Add(New ScrapeAction(AddressOf TvAddToCache               , "Updating caches"         ))
                Actions.Items.Add(New ScrapeAction(AddressOf TvLoadExistingEpisodes     , "Load Existing Episodes"  ))
            End If
        Else
            If haveTVDbID Then
                NewShow.State = Media_Companion.ShowState.Open
            Else
                'Resolve show name from folder
                Dim FolderName As String = Utilities.GetLastFolder(_folder & "\")
                If FolderName.ToLower.Contains(Pref.excludefromshowfoldername.ToLower) Then
                    Dim indx As Integer = FolderName.ToLower.IndexOf(Pref.excludefromshowfoldername.ToLower)
                    Dim excludearticle As String = FolderName.Substring(indx - 1, Pref.excludefromshowfoldername.Length + 1)
                    FolderName = FolderName.Replace(excludearticle, "")
                End If
                FolderName = FolderName.Replace(Pref.excludefromshowfoldername, "")
                Dim M As Match
                M = Regex.Match(FolderName, "\s*[\(\{\[](?<date>[\d]{4})[\)\}\]]")
                If M.Success = True Then
                    FolderName = String.Format("{0} ({1})", FolderName.Substring(0, M.Index), M.Groups("date").Value)
                End If
                NewShow.Title.Value = FolderName
                ReportProgress(ProgressStart & " possibly - " & FolderName, "Searching for series: " & FolderName & vbCrLf)

                tvdb.LookupLang = _lang
                tvdb.Title = FolderName
                If tvdb.PossibleShowList IsNot Nothing Then
                    Dim tempseries As New TheTvDB.TvdbSeries
                    tempseries = tvdb.FindBestPossibleShow(tvdb.PossibleShowList, FolderName, _lang)
                    If tempseries.Similarity > .9 Then
                        NewShow.State = Media_Companion.ShowState.Open
                    End If
                    _possibleTvdb = tempseries.Identity
                    tempseries = Nothing
                End If
            End If
            If Not String.IsNullOrEmpty(_possibleTvdb) Then
                Dim Series As New TheTvDB.TvdbSeries
                tvdb.TvdbId = _possibleTvdb
                Series = tvdb.Series
                If tvdb.SeriesNotFound Then
                    NewShow.FailedLoad = True
                    ReportProgress(MSG_ERROR, "Please adjust the TV Show title And try again for: " & tvdb.Title & "' - No Show Returned", Progress.Commands.SetIt)
                    Exit Sub
                End If
                ReportProgress(ProgressStart & "Show Title: " & Series.SeriesName & " ", "!!! Series Found: " & Series.SeriesName & " : Id: (" & Series.Identity & ")" & vbCrLf)
                NewShow.AbsorbTvdbSeries(Series)
                NewShow.Language.Value = _lang
            End If
        End If
    End Sub

    Sub SeriesCheckTVDbBodyScrape
        'Failed...
        If NewShow.FailedLoad Then
            ReportProgress(MSG_ERROR, "!!! Unable to scrape body with refs """ & _folder & vbCrLf & "TVDB may not be available or Series Title is invalid" & vbCrLf, msg_append)
            AppendSeriesScrapeFailedActions
        Else
            ReportProgress(MSG_OK, "!!! Series Body Scraped OK" & vbCrLf, msg_append)
            AppendSeriesScrapeSuccessActions
        End If
    End Sub

    ''' <summary>
    ''' Get TV or Episode ratings and Votes from IMDb
    ''' </summary>
    ''' <param name="IMDbId">Tv or Episode IMDB Id</param>
    ''' <param name="rating">current rating if one allocated</param>
    ''' <param name="votes">current votes if allocated</param>
    ''' <returns></returns>
    Function ep_getIMDbRating(ByVal IMDbId As String, ByRef rating As String, ByRef votes As String) As Boolean
        Dim aok As Boolean = True
        If String.IsNullOrEmpty(IMDbId) Then Return False
        Dim url As String = String.Format("http://akas.imdb.com/title/{0}/", IMDbId)
        Dim IMDbpage As String = Utilities.DownloadTextFiles(url, True)
        Dim m As Match
        m = Regex.Match(IMDbpage, "<span itemprop=""ratingValue"">(.*?)</span>")
        If m.Success Then rating = m.Groups(1).Value
        Dim n As Match
        n = Regex.Match(IMDbpage, "itemprop=""ratingCount"">(.*?)</span")
        If n.Success Then votes = n.Groups(1).Value
        Return rating <> ""
    End Function

    Sub TvLoadExistingEpisodes
        Dim episodelist As New List(Of TvEpisode)
        episodelist = loadepisodes()
        For Each ep In episodelist
            NewShow.AddEpisode(ep)
        Next
    End Sub

    ''' <summary>
    ''' Scan folders in a series folder for existing episode nfo's and return
    ''' </summary>
    ''' <returns>List (of TvEpisode)</returns>
    Function loadepisodes() As List(Of TvEpisode)
        Dim episodelist As New List(Of TvEpisode)
        Dim missingeppath As String = Utilities.MissingPath
        Dim newlist As New List(Of String)
        newlist.Clear()
        newlist = Utilities.EnumerateFolders(NewShow.FolderPath) 'TODO: Restore loging functions
        newlist.Add(NewShow.FolderPath)
        For Each folder In newlist
            If folder = "long_path" Then Continue For
            Dim dir_info As New DirectoryInfo(folder)
            Dim fs_infos() As FileInfo = dir_info.GetFiles("*.NFO", IO.SearchOption.TopDirectoryOnly)
            For Each fs_info As FileInfo In fs_infos
                Application.DoEvents()
                If Path.GetFileName(fs_info.FullName.ToLower) <> "tvshow.nfo" And fs_info.ToString.Substring(0, 2) <> "._" Then
                    Dim EpNfoPath As String = fs_info.FullName
                    If ep_NfoValidate(EpNfoPath) Then
                        Dim multiepisodelist As New List(Of TvEpisode)
                        Dim need2resave As Boolean = False
                        multiepisodelist = WorkingWithNfoFiles.ep_NfoLoad(EpNfoPath)
                        For Each Ep In multiepisodelist
                            If Ep.ShowId.Value <> PossibleTVdb Then need2resave = True
                            Ep.ShowObj = NewShow
                            Dim missingNfoPath As String = missingeppath & PossibleTVdb & "." & Ep.Season.Value & "." & Ep.Episode.Value & ".nfo"
                            If File.Exists(missingNfoPath) Then Utilities.SafeDeleteFile(missingNfoPath)
                            episodelist.Add(Ep.Cachedata)
                        Next
                        If need2resave Then WorkingWithNfoFiles.ep_NfoSave(multiepisodelist, EpNfoPath)    'If new ShowID stored, resave episode nfo.
                    End If
                End If
            Next fs_info
        Next
        Return episodelist
    End Function

    ''' <summary>
    ''' Valididate if episode nfo file is valid XBMC/Kodi nfo
    ''' </summary>
    ''' <param name="nfopath"></param>
    ''' <returns>True if XBMC/Kodi compliant nfo</returns>
    Function ep_NfoValidate(ByVal nfopath As String) As Boolean
        ep_NfoValidate = True
		If File.Exists(nfopath) Then
			Dim tvshow As New XmlDocument
			Try
                Using tmpstrm As IO.StreamReader = File.OpenText(nfopath)
                    tvshow.Load(tmpstrm)
                End Using
				
			Catch ex As Exception
				If ex.Message.ToLower.Contains("multiple root elements") Then
					ep_NfoValidate = chkxbmcmultinfo(nfopath)
				Else
					ep_NfoValidate = False
				End If
			End Try
			If ep_NfoValidate = True Then
				Try
					Dim tempstring As String
					Using filechck As IO.StreamReader = File.OpenText(nfopath)
					    tempstring = filechck.ReadToEnd.ToLower
					End Using
					If tempstring = Nothing Then ep_NfoValidate = False
					Try
						Dim seasonno As String = tempstring.Substring(tempstring.IndexOf("<season>") + 8, tempstring.IndexOf("</season>") - tempstring.IndexOf("<season>") - 8)
						If Not IsNumeric(seasonno) Then ep_NfoValidate = False
					Catch ex As Exception
						ep_NfoValidate = False
					End Try
					Try
						Dim episodeno As String = tempstring.Substring(tempstring.IndexOf("<episode>") + 9, tempstring.IndexOf("</episode>") - tempstring.IndexOf("<episode>") - 9)
						If Not IsNumeric(episodeno) Then ep_NfoValidate = False
					Catch ex As Exception
						ep_NfoValidate = False
					End Try
				Catch ex As Exception
				End Try
			End If
			Return ep_NfoValidate
		End If
		Return False
	End Function

    ''' <summary>
    ''' Check if nfo file is XBMC/Kodi multiepisode nfo and convert to MediaCompanion format.
    ''' </summary>
    ''' <param name="xmlpath">nfo full path and filename</param>
    ''' <returns>True if resaved nfo is Valid XBMC/Kodi nfo</returns>
    Function chkxbmcmultinfo(ByVal xmlpath As String) As Boolean
		Try
			Dim testxml() As String = File.ReadAllLines(xmlpath)
			Dim first As Boolean = True
			Dim finalxml As String = ""
			For Each line In testxml
				If line.Contains("<episodedetails>") AndAlso first Then finalxml &= "<multiepisodenfo>"
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

    Function TvGetArtwork(ByVal nfopath As String, ByVal shFanart As Boolean, ByVal shPosters As Boolean, ByVal shSeason As Boolean, ByVal shXtraFanart As Boolean, Optional ByVal force As Boolean = True) As Boolean
        Dim success As Boolean = False
        Try
            Dim MaxSeasonNo As Integer = 1
            Dim eden As Boolean     = Pref.EdenEnabled
            Dim frodo As Boolean    = Pref.FrodoEnabled
            Dim currentshowpath As String = nfopath.Replace("tvshow.nfo", "")
            Dim doPoster As Boolean = If(Pref.tvdlposter OrElse Pref.TvChgShowDlPoster, True, False)
            Dim doFanart As Boolean = If(Pref.tvdlfanart OrElse Pref.TvChgShowDlFanart, True, False)
            Dim doSeason As Boolean = If(Pref.tvdlseasonthumbs OrElse Pref.TvChgShowDlSeasonthumbs, True, False)
            Dim artlist As New List(Of McImage)
            artlist.Clear()
            Dim isposter As String = Pref.postertype
            Dim isseasonall As String = Pref.seasonall

            Dim overwriteimage As Boolean = If(Pref.overwritethumbs OrElse Pref.TvChgShowOverwriteImgs, True, False)
            If Not force Then overwriteimage = False    'Over-ride overwrite if we force not to overwrite, ie: missing artwork

            Dim Langlist As New List(Of String)
            If Not _lang = "" Then Langlist.Add(_lang)
            Langlist.AddIfNew("en")
            Langlist.AddifNew(Pref.TvdbLanguageCode)
            Langlist.Add("")
            If tvdb.McPosters.Count = 0 AndAlso tvdb.McFanart.Count = 0 AndAlso tvdb.McSeason.Count = 0 Then Return success

            For Each art In tvdb.McSeason
                If Not IsNothing(art.Season) AndAlso art.Season.ToInt > MaxSeasonNo Then MaxSeasonNo = art.Season.ToInt
            Next

            'Posters, Main and Season Including Banners
            If shPosters Then
                artlist.AddRange(tvdb.McPosters)
                'Main Poster
                If artlist.Count > 0 AndAlso (isposter = "poster" Or frodo Or isseasonall = "poster") AndAlso doPoster Then 'poster
                    Dim mainposter As String = Nothing
                    Dim q = From b In artlist Order By b.votes Descending
                    If Not q.count = 0 Then
                        Dim tmpimg As McImage = q(0)
                        mainposter = q(0).hdUrl
                    End If
                    If Not IsNothing(mainposter) Then
                        Dim imgpaths As New List(Of String)
                        If frodo Then imgpaths.Add(currentshowpath & "poster.jpg")
                        If eden AndAlso isposter = "poster" Then imgpaths.Add(currentshowpath & "folder.jpg")
                        If frodo AndAlso isseasonall <> "none" Then imgpaths.Add(currentshowpath & "season-all-poster.jpg")
                        If eden AndAlso isseasonall = "poster" Then imgpaths.Add(currentshowpath & "season-all.tbn")
                        success = DownloadCache.SaveImageToCacheAndPaths(mainposter, imgpaths, False, , , overwriteimage)
                    End If
                End If
                artlist.Clear()
                artlist.AddRange(tvdb.McSeries)
                'Main Banner
                If (isposter = "banner" Or frodo Or isseasonall = "wide") And doPoster Then 'banner
                    Dim mainbanner As String = Nothing
                    Dim q = From b In artlist Order By b.votes Descending
                    If Not q.count = 0 Then
                        Dim tmpimg As McImage = q(0)
                        mainbanner = tmpimg.hdUrl
                    End If
                    If Not IsNothing(mainbanner) Then
                        Dim imgpaths As New List(Of String)
                        If frodo Then imgpaths.Add(currentshowpath & "banner.jpg")
                        If eden AndAlso isposter = "banner" Then imgpaths.Add(currentshowpath & "folder.jpg")
                        If frodo AndAlso isseasonall <> "none" Then imgpaths.Add(currentshowpath & "season-all-banner.jpg")
                        If eden AndAlso isseasonall = "wide" Then imgpaths.Add(currentshowpath & "season-all.tbn")
                        success = DownloadCache.SaveImageToCacheAndPaths(mainbanner, imgpaths, False, , , overwriteimage)
                    End If
                End If
            End If

            artlist.Clear()
            artlist.AddRange(tvdb.McSeason)
            artlist.AddRange(tvdb.McSeasonWide)
            If shSeason Then
                'SeasonXX Poster
                For f = 0 To MaxSeasonNo + 1
                    Dim seasonnumber As String = f.ToString
                    If (isposter = "poster" Or frodo) And doSeason Then 'poster
                        Dim seasonXXposter As String = Nothing
                        Dim q = From b In artlist Where b.type = "season" AndAlso b.Season = seasonnumber Order By b.votes Descending
                        If Not q.count = 0 Then
                            Dim tmpimg As McImage = q(0)
                            seasonXXposter = tmpimg.hdUrl
                        End If
                        If Not IsNothing(seasonXXposter) Then
                            Dim tempstring As String = ""
                            If f < 10 Then
                                tempstring = "0" & f.ToString
                            Else
                                tempstring = f.ToString
                            End If
                            If tempstring = "00" Then tempstring = "-specials"
                            Dim imgpaths As New List(Of String)
                            If frodo Then imgpaths.Add(currentshowpath & "season" & tempstring & "-poster.jpg")
                            If eden Then imgpaths.Add(currentshowpath & "season" & tempstring & ".tbn")
                            success = DownloadCache.SaveImageToCacheAndPaths(seasonXXposter, imgpaths, False, , , overwriteimage)
                        End If
                    End If

                    'SeasonXX Banner
                    If (isposter = "banner" Or frodo) And doSeason Then 'banner
                        Dim seasonXXbanner As String = Nothing
                        Dim q = From b In artlist Where b.type = "seasonwide" AndAlso b.Season = seasonnumber Order By b.votes Descending
                        If Not q.count = 0 Then
                            Dim tmpimg As McImage = q(0)
                            seasonXXbanner = tmpimg.hdUrl
                        End If
                        If Not IsNothing(seasonXXbanner) Then
                            Dim tempstring As String = ""
                            If f < 10 Then
                                tempstring = "0" & f.ToString
                            Else
                                tempstring = f.ToString
                            End If
                            If tempstring = "00" Then tempstring = "-specials"
                            Dim imgpaths As New List(Of String)
                            If frodo Then imgpaths.Add(currentshowpath & "season" & tempstring & "-banner.jpg")
                            If eden Then imgpaths.Add(currentshowpath & "season" & tempstring & ".tbn")
                            success = DownloadCache.SaveImageToCacheAndPaths(seasonXXbanner, imgpaths, False, , , overwriteimage)
                        End If
                    End If
                Next
                TvCheckfolderjpgart()
            End If

            artlist.Clear()
            artlist.AddRange(tvdb.McFanart)
            'Main Fanart
            If shFanart AndAlso doFanart Then
                Dim fanartposter As String = Nothing
                Dim q = From b In artlist Order By b.votes Descending
                If Not q.count = 0 Then
                    Dim tmpimg As McImage = q(0)
                    fanartposter = tmpimg.hdUrl
                End If
                If Not IsNothing(fanartposter) Then
                    Dim imgpaths As New List(Of String)
                    imgpaths.Add(currentshowpath & "fanart.jpg")
                    If frodo And isseasonall <> "none" Then imgpaths.Add(currentshowpath & "season-all-fanart.jpg")
                    success = DownloadCache.SaveImageToCacheAndPaths(fanartposter, imgpaths, False, , , overwriteimage)
                End If
            End If

            'ExtraFanart
            If shXtraFanart Then
                Dim xfanart As String = currentshowpath & "extrafanart\"
                Dim fanartposter As New List(Of McImage)
                Dim q = From b In artlist Order By b.votes Descending
                If Not q.count = 0 Then
                    fanartposter.AddRange(q.ToList)
                    If fanartposter.Count > 1 Then fanartposter.RemoveAt(0) 'Remove first fanart as used for main fanart
                End If
                If fanartposter.Count > 0 Then
                    Dim x As Integer = 0
                    Do Until x = Pref.TvXtraFanartQty
                        If x = fanartposter.Count Then Exit Do
                        success = Utilities.DownloadFile(fanartposter(x).hdUrl, (xfanart & fanartposter(x).id & ".jpg"))
                        x = x + 1
                    Loop
                End If
            End If
        Catch
        End Try
        Return success

    End Function

    Sub TvCheckfolderjpgart()
        Dim currentshowpath As String = NewShow.FolderPath
        If Pref.tvfolderjpg AndAlso Not File.Exists(currentshowpath & "folder.jpg") Then
            If File.Exists(currentshowpath & "poster.jpg") AndAlso Pref.FrodoEnabled Then
                Utilities.SafeCopyFile(currentshowpath & "poster.jpg", currentshowpath & "folder.jpg", False)
            End If
        End If
        Dim I = 1
        If Pref.seasonfolderjpg Then
            For Each Seas In NewShow.Seasons.Values
                If Seas.FolderPath <> NewShow.FolderPath Then
                    Dim seasonfile As String = Seas.Poster.FileName   '= Seas.SeasonLabel.ToLower.Replace(" ", "")& "-poster.jpg"
                    If File.Exists(NewShow.FolderPath & seasonfile) AndAlso Not File.Exists(Seas.FolderPath & "folder.jpg") Then
                        Utilities.SafeCopyFile(NewShow.FolderPath & seasonfile, Seas.FolderPath & "folder.jpg", False)
                    End If
                End If
            Next
        End If
    End Sub

    Sub TvFanartTvArt(ByVal ThisShow As TvShow, ByVal force As Boolean)
        Dim clearartLD As String = Nothing  : Dim logoLD As String = Nothing    : Dim clearart As String = Nothing  : Dim logo As String = Nothing
        Dim poster As String = Nothing      : Dim fanart As String = Nothing    : Dim banner As String = Nothing    : Dim landscape As String = Nothing
        Dim character As String = Nothing
        Dim currentshowpath As String = ThisShow.FolderPath
        Dim DestImg As String = ""
        Dim aok As Boolean = True
        Dim frodo As Boolean = Pref.FrodoEnabled
        Dim eden As Boolean = Pref.EdenEnabled
        Dim ID As String = ThisShow.TvdbId.Value
        Dim TvFanartlist As New FanartTvTvList
        Dim newobj As New FanartTv
        newobj.ID = ID
        newobj.src = "tv"
        Try
            TvFanartlist = newobj.FanarttvTvresults
        Catch ex As Exception
            aok = False
        End Try
        If Not aok Then Exit Sub
        Dim lang As New List(Of String)
        If Not _lang = "" Then lang.AddIfNew(_lang)
        lang.AddIfNew(Pref.TvdbLanguageCode)
        lang.AddIfNew("en")
        lang.Add("00")
        For Each lan In lang
            If IsNothing(clearart) Then
                For Each Art In TvFanartlist.hdclearart 
                    If Art.lang = lan Then clearart = Art.url : Exit For
                Next
            End If
            If IsNothing(clearartLD) Then
                For Each Art In TvFanartlist.clearart 
                    If Art.lang = lan Then clearartLD = Art.url : Exit For
                Next
            End If
            If IsNothing(logo) Then
                For Each Art In TvFanartlist.hdtvlogo
                    If Art.lang = lan Then logo = Art.url : Exit For
                Next
            End If
            If IsNothing(logoLD) Then
                For Each Art In TvFanartlist.clearlogo
                    If Art.lang = lan Then logoLD = Art.url : Exit For
                Next
            End If
            If IsNothing(poster) Then
                For Each Art In TvFanartlist.tvposter 
                    If Art.lang = lan Then poster = Art.url : Exit For
                Next
            End If
            If IsNothing(fanart) Then
                For Each Art In TvFanartlist.showbackground  
                    If Art.lang = lan Then fanart = Art.url : Exit For
                Next
            End If
            If IsNothing(banner) Then
                For Each Art In TvFanartlist.tvbanner 
                    If Art.lang = lan Then banner = Art.url : Exit For
                Next
            End If
            If IsNothing(landscape) Then
                For Each Art In TvFanartlist.tvthumb  
                    If Art.lang = lan Then landscape = Art.url : Exit For
                Next
            End If
            If IsNothing(character) Then
                For Each Art In TvFanartlist.characterart   
                    If Art.lang = lan Then character = Art.url : Exit For
                Next
            End If
        Next
        If IsNothing(clearart) AndAlso Not IsNothing(clearartld) Then clearart = clearartLD 
        If IsNothing(logo) AndAlso Not IsNothing(logold) Then logo = logold
            DestImg = currentshowpath & "clearart.png"
        If Not IsNothing(clearart) Then Utilities.DownloadFile(clearart, DestImg, force)
            DestImg = currentshowpath & "logo.png"
        If Not IsNothing(logo) Then Utilities.DownloadFile(logo, DestImg, force)
        DestImg = currentshowpath & "character.png"
        If Not IsNothing(character) Then Utilities.DownloadFile(character, DestImg, force)
        If Not IsNothing(poster) Then
            Dim destpaths As New List(Of String)
            If frodo Then
                destpaths.Add(currentshowpath & "poster.jpg")
                destpaths.Add(currentshowpath & "season-all-poster.jpg")
            End If
            If eden then
                destpaths.Add(currentshowpath & "poster.jpg")
                destpaths.Add(currentshowpath & "season-all.tbn")
            End If
            Dim success As Boolean = DownloadCache.SaveImageToCacheAndPaths(poster, destpaths, False, , , force)
        End If
        If Not IsNothing(fanart) Then
            Dim Destpaths As New List(Of String)
            If frodo Then
                Destpaths.Add(currentshowpath & "fanart.jpg")
                Destpaths.Add(currentshowpath & "season-all-fanart.jpg")
            End If
            If eden Then Destpaths.Add(currentshowpath & "fanart.jpg")
            Dim success As Boolean = DownloadCache.SaveImageToCacheAndPaths(fanart, Destpaths, False, , , force)
        End If
        DestImg = currentshowpath & "landscape.jpg"
        If Not IsNothing(landscape) Then Utilities.DownloadFile(landscape, DestImg, force)
        DestImg = currentshowpath & "banner.jpg"
        If Not IsNothing(banner) Then
            Utilities.DownloadFile(banner, DestImg, force)
            If frodo Then
                DestImg = currentshowpath & "season-all-banner.jpg"
                Utilities.DownloadFile(banner, DestImg, force)
            End If
        End If

        Dim firstseason As Integer = 1
        Dim lastseason As Integer = -1
        For Each item In TvFanartlist.seasonposter
            Dim itemseason As Integer = item.season.ToInt
            If itemseason > lastseason Then lastseason = itemseason
            If itemseason < firstseason Then firstseason = itemseason
        Next
        If lastseason >= firstseason Then
            For i = firstseason to lastseason 
                Dim savepaths As New List(Of String)
                Dim seasonurl As String = Nothing
                For Each lan In lang
                    For Each item In TvFanartlist.seasonposter
                        If item.lang = lan AndAlso item.season = i.ToString Then
                            seasonurl = item.url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(seasonurl) Then Exit for
                Next
                If Not IsNothing(seasonurl) Then
                    Dim seasonno As String = i.ToString
                    If seasonno <> "" Then
                        If seasonno.Length = 1 Then seasonno = "0" & seasonno
                        If seasonno = "00" Then
                            seasonno = "-specials"
                        End If
                        destimg = currentshowpath &  "season" & seasonno & "-poster.jpg"
                        If Pref.FrodoEnabled Then savepaths.Add(destimg)
                        If Pref.EdenEnabled Then
                            destimg = destimg.Replace("-poster.jpg", ".tbn")
                            savepaths.Add(destimg)
                        End If
                    End If
                End If
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(seasonurl, savepaths, False, , , force)
            Next
        End If

        firstseason = 1
        lastseason = -1
        For Each item In TvFanartlist.seasonbanner 
            Dim itemseason As Integer = item.season.ToInt
            If itemseason > lastseason Then lastseason = itemseason
            If itemseason < firstseason Then firstseason = itemseason
        Next
        If lastseason >= firstseason AndAlso frodo Then
            For i = firstseason to lastseason 
                Dim savepaths As New List(Of String)
                Dim seasonurl As String = Nothing
                For Each lan In lang
                    For Each item In TvFanartlist.seasonbanner
                        If item.lang = lan AndAlso item.season = i.ToString Then
                            seasonurl = item.url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(seasonurl) Then Exit for
                Next
                If Not IsNothing(seasonurl) Then
                    Dim seasonno As String = i.ToString
                    If seasonno <> "" Then
                        If seasonno.Length = 1 Then seasonno = "0" & seasonno
                        If seasonno = "00" Then
                            seasonno = "-specials"
                        End If
                        destimg = currentshowpath &  "season" & seasonno & "-banner.jpg"
                        If Pref.FrodoEnabled Then savepaths.Add(destimg)
                    End If
                End If
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(seasonurl, savepaths, False, , , force)
            Next
        End If

        firstseason = 1
        lastseason = -1
        For Each item In TvFanartlist.seasonthumb  
            Dim itemseason As Integer = item.season.ToInt
            If itemseason > lastseason Then lastseason = itemseason
            If itemseason < firstseason Then firstseason = itemseason
        Next
        If lastseason >= firstseason AndAlso frodo Then
            For i = firstseason to lastseason 
                Dim savepaths As New List(Of String)
                Dim seasonurl As String = Nothing
                For Each lan In lang
                    For Each item In TvFanartlist.seasonthumb
                        If item.lang = lan AndAlso item.season = i.ToString Then
                            seasonurl = item.url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(seasonurl) Then Exit for
                Next
                If Not IsNothing(seasonurl) Then
                    Dim seasonno As String = i.ToString
                    If seasonno <> "" Then
                        If seasonno.Length = 1 Then seasonno = "0" & seasonno
                        If seasonno = "00" Then
                            seasonno = "-specials"
                        End If
                        destimg = currentshowpath &  "season" & seasonno & "-landscape.jpg"
                        If Pref.FrodoEnabled Then savepaths.Add(destimg)
                    End If
                End If
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(seasonurl, savepaths, False, , , force)
            Next
        End If
    End Sub
    
    Public Function findshows(ByVal title As String, ByRef showslist As List(Of str_PossibleShowList)) As Boolean
        Monitor.Enter(Me)
        Try
            IniTVdb("")
            tvdb.Title = title
            Dim SeriesList As New List(Of TheTvDB.TvdbSeries)
            SeriesList = tvdb.PossibleShowList
            For each result In SeriesList
                Dim newshow As New str_PossibleShowList(SetDefaults)
                newshow.showtitle = result.SeriesName
                newshow.showid = result.Identity
                newshow.showbanner = "http://www.thetvdb.com/banners/" & result.Banner
                showslist.Add(newshow)
            Next
            Return True
        Catch EX As Exception
            Return False
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

#End Region

#Region "Episode routines"

    Sub EPSearchNew()
        Dim progresstext    As String   = ""
        Dim ShowsScanned    As Integer  = 0
        Dim newtvfolders    As New List(Of String)
        Dim FoldersScanned  As Integer  = 0
        Dim ShowsLocked     As Integer  = 0
        newEpisodeList.Clear()

        'Check if Episode Rescrape
        If _possibleTvdb <> "" AndAlso _folder <> "" Then
            Dim filename_video  = _folder
            Dim filename_nfo    = filename_video.Replace(Path.GetExtension(filename_video), ".nfo")
            Dim truefilename As String = Utilities.GetFileNameFromPath(filename_video)
			If Not truefilename.Substring(0, 2) = "._" Then
				Dim newep As New TvEpisode
                newep.UniqueId.Value    = _possibleTvdb
				newep.NfoFilePath       = filename_nfo
				newep.VideoFilePath     = filename_video
				newep.MediaExtension    = Path.GetExtension(filename_video)
				newEpisodeList.Add(newep)
                Exit Sub
			End If
        End If

        ReportProgress(, "---Using MC TVDB api V2 Scraper---" & vbCrLf)
        For Each TvShow As Media_Companion.TvShow In ListOfShows
            Dim TvFolder As String = Path.GetDirectoryName(TvShow.FolderPath)
            Dim Add As Boolean = True
            If TvShow.State <> Media_Companion.ShowState.Open AndAlso Not EpForceSearch Then Add = False

            If Add Then
                ShowsScanned +=  1
                progresstext = String.Concat("Stage 1 of 3 : Found " & newtvfolders.Count & " : Creating List of Folders From Roots : Searching - '" & TvFolder & "'")
                Reportprogress(progresstext, "")
                If Cancelled Then Exit Sub
                Dim hg As New DirectoryInfo(TvFolder)
                If hg.Exists Then
                    Reportprogress(,"Found " & hg.FullName.ToString & vbCrLf)
                    newtvfolders.Add(TvFolder)
                    Reportprogress(,"Checking for subfolders" & vbCrLf)
                    Dim ExtraFolder As List(Of String) = Utilities.EnumerateFolders(TvFolder, 3)
                    For Each Item As String In ExtraFolder
                        If Pref.ExcludeFolders.Match(Item) Then Continue For
                        newtvfolders.Add(Item)
                        FoldersScanned += 1
                    Next
                End If
            Else
                ShowsLocked += 1
            End If
        Next

        Reportprogress(,"" & vbCrLf)
        newtvfolders.Sort()
        For g = 0 To newtvfolders.Count - 1
            If Cancelled Then Exit Sub
            For Each f In Utilities.VideoExtensions
                Dim dirpath As String = newtvfolders(g)
                Dim dir_info As New DirectoryInfo(dirpath)
                tv_NewFind(dirpath, f)
            Next f
            progresstext = String.Concat("Stage 2 of 3 : Found " & newEpisodeList.Count & " : Searching for New Episodes in Folders " & g + 1 & " of " & newtvfolders.Count & " - '" & newtvfolders(g) & "'")
            Reportprogress(progresstext, "")
        Next g
    End Sub

    Sub EPPopulateSeriesInfo()
        Dim S As String = ""
        Reportprogress(, "Pre-Populating found episodes with Series info" & vbCrLf)
        For Each newepisode In newEpisodeList
            S = ""
            For Each Shows In Cache.TvCache.Shows
                If Cancelled Then Exit Sub
                If newepisode.FolderPath.Contains(Shows.FolderPath) Then
                    If Shows.ImdbId.Value Is Nothing OrElse String.IsNullOrEmpty(Shows.Premiered.Value) Then
                        Shows = nfoFunction.tvshow_NfoLoad(Shows.NfoFilePath)
                    End If
                    newepisode.ShowLang.Value   = Shows.Language.Value
                    newepisode.sortorder.Value  = Shows.SortOrder.Value
                    newepisode.Showtvdbid.Value = Shows.TvdbId.Value
                    newepisode.Showimdbid.Value = Shows.ImdbId.Value
                    newepisode.ShowTitle.Value  = Shows.Title.Value
                    newepisode.ShowYear.Value   = Shows.Year.Value
                    newepisode.ShowObj          = Shows
                    If String.IsNullOrEmpty(newepisode.ShowYear.Value) AndAlso Not String.IsNullOrEmpty(Shows.Premiered.Value) Then
                        Dim yr As String = Shows.Premiered.Value.Substring(0,4)
                        If yr.Length = 4 Then newepisode.ShowYear.Value = yr
                    End If
                    newepisode.actorsource.Value = Shows.EpisodeActorSource.Value
                    newepisode.ImdbId.Value      = ""    ' Fix for Episode getting Show's IMDb Id number, not the Episode IMDb Id number.
                    Exit For
                End If
            Next
            Dim episode As New TvEpisode
            Dim airedgot As Boolean = False
            For Each Regexs In Pref.tv_RegexScraper
                S = newepisode.VideoFilePath
                Dim i As Integer                  'sacrificial variable to appease the TryParseosaurus Checks
                If Not String.IsNullOrEmpty(newepisode.ShowTitle.Value) AndAlso Integer.TryParse(newepisode.ShowTitle.Value, i) <> -1 Then S = S.Replace(newepisode.ShowTitle.Value, "")
                If Not String.IsNullOrEmpty(newepisode.ShowYear.Value) AndAlso (newepisode.ShowYear.Value.ToInt <> 0) Then 
                    If S.Contains(newepisode.ShowYear.Value) AndAlso Not S.ToLower.Contains("s" & newepisode.ShowYear.Value) Then S = S.Replace(newepisode.ShowYear.Value, "")
                End If
                    
                'stage = "3"
                S = S.Replace("x265"    , "")
                S = S.Replace("x264"    , "")
                S = S.Replace("720p"    , "")
                S = S.Replace("720i"    , "")
                S = S.Replace("1080p"   , "")
                S = S.Replace("1080i"   , "")
                S = S.Replace("X265"    , "")
                S = S.Replace("X264"    , "")
                S = S.Replace("720P"    , "")
                S = S.Replace("720I"    , "")
                S = S.Replace("1080P"   , "")
                S = S.Replace("1080I"   , "")
                
                Dim N As Match      'Do date test first.
                N = Regex.Match(S, Pref.tv_EpRegexDate)
                If N.Success Then
                    If Not airedgot Then
                        Dim aired As String = N.Groups(0).Value.Replace(".", "-").Replace("_", "-")
                        newepisode.Aired.Value = aired
                        airedgot = True
                    End If
                    If airedgot Then S = S.Replace(N.Groups(0).Value, "")
                End If
                
                If Not N.Success OrElse airedgot Then
                    Dim M As Match
                    M = Regex.Match(S, Regexs)
                    If M.Success = True Then
                        Try
                            newepisode.Season.Value = M.Groups(1).Value.ToString
                            newepisode.Episode.Value = M.Groups(2).Value.ToString
                            Try
                                Dim matchvalue As String = M.Value
                                newepisode.Thumbnail.FileName = S.Substring(S.LastIndexOf(matchvalue)+matchvalue.Length, S.Length - (S.LastIndexOf(matchvalue) + (matchvalue.Length)))
                            Catch ex As Exception
                            End Try
                            Exit For
                        Catch
                            newepisode.Season.Value = "-1"
                            newepisode.Episode.Value = "-1"
                        End Try
                    End If
                End If
            Next
            If newepisode.Season.Value = Nothing Then newepisode.Season.Value = "-1"
            If newepisode.Episode.Value = Nothing Then newepisode.Episode.Value = "-1"
            If newepisode.Season.Value <> "-1" AndAlso newepisode.Episode.Value <> "-1" Then newepisode.Aired.Value = "" 'Clear Aired value if got valid Ep and Season values.
        Next
    End Sub

    Sub EPCheckifContinue()
        If newEpisodeList.Count > 0 Then
            AppendEpisodeScrapeSuccessActions
        Else
            Reportprogress("No new episodes found, exiting scraper.", "!!! No new episodes found, exiting scraper." & vbCrLf)
        End If
    End Sub

    Sub EPScrape()
        Dim savepath        As String   = ""
        Dim scrapedok       As Boolean
        Dim epscount        As Integer  = 0
        Dim progresstext    As String   = ""
        For Each eps In newEpisodeList
            Dim showtitle As String = eps.ShowTitle.Value
            epscount += 1
            ReportProgress(, "!!! With File : " & eps.VideoFilePath & vbCrLf)
            If eps.Aired.Value <> Nothing Then
                Reportprogress(, "!!! Detected  : Aired Date: " & eps.Aired.Value & vbCrLf)
            Else
                Reportprogress(, "!!! Detected  : Season : " & eps.Season.Value & " Episode : " & eps.Episode.Value & vbCrLf)
            End If
            If eps.Season.Value = "-1" And eps.Episode.Value = "-1" AndAlso eps.Aired.Value = Nothing Then
                Reportprogress(, "!!! WARNING: Can't extract Season and Episode details from this filename, file not added!" & vbCrLf)
                Reportprogress(, "!!!" & vbCrLf)
                Continue For    'if we can't get season or episode then skip to next episode
            End If
                
            Dim episodearray As New List(Of TvEpisode)
            episodearray.Clear()
            episodearray.Add(eps)
            If Cancelled Then Exit Sub
            'Dim WhichScraper As String = ""
            'If Pref.tvshow_useXBMC_Scraper = True Then
            '    WhichScraper = "XBMC TVDB"
            'Else
            '    WhichScraper = "MC TVDB"
            'End If
            If eps.UniqueId.Value = Nothing Then
                progresstext = String.Concat("Stage 3 of 3 : Scraping New Episodes : Scraping " & epscount & " of " & newEpisodeList.Count & " - '" & Path.GetFileName(eps.VideoFilePath) & "'")
            Else
                progresstext = String.Concat("Rescraping episode - '" & Path.GetFileName(eps.VideoFilePath) & "'")
            End If
            'progresstext = String.Concat("Stage 3 of 3 : Scraping New Episodes : Scraping " & epscount & " of " & newEpisodeList.Count & " - '" & Path.GetFileName(eps.VideoFilePath) & "'")
            ReportProgress(progresstext)
            Dim removal As String = ""
            If (eps.Season.Value = "-1" Or eps.Episode.Value = "-1") AndAlso eps.Aired.Value = Nothing Then
                eps.Title.Value = Utilities.GetFileName(eps.VideoFilePath)
                eps.Rating.Value = "0"
                eps.Votes.Value = "0"
                eps.PlayCount.Value = "0"
                eps.Genre.Value = "Unknown Episode Season and/or Episode Number"
                eps.GetFileDetails()
                episodearray.Add(eps)
                savepath = episodearray(0).NfoFilePath
            Else
                Dim temppath As String = eps.NfoFilePath
                'check for multiepisode files
                Dim M2 As Match
                Dim epcount As Integer = 0
                Dim allepisodes(100) As Integer
                Dim S As String = ""
                If Not String.IsNullOrEmpty(eps.Thumbnail.FileName) Then
                    S = Regex.Replace(eps.Thumbnail.FileName, "\(.*?\)", "")   'Remove anything from filename in brackets like resolution ie: (1920x1080) that may give false episode number
                    S = Regex.Replace(S, "\[.*?\]", "")
                End If
                eps.Thumbnail.FileName = ""
                Do
                    If eps.Aired.Value <> Nothing Then Exit Do
                    '<tvregex>[Ss]([\d]{1,2}).?[Ee]([\d]{3})</tvregex>
                    M2 = Regex.Match(S, "(([EeXx])([\d]{1,4}))")
                    If M2.Success = True Then
                        Dim skip As Boolean = False
                        For Each epso In episodearray
                            If epso.Episode.Value = M2.Groups(3).Value Then skip = True
                        Next
                        If skip = False Then
                            Dim multieps As New TvEpisode
                            multieps.Season.Value = eps.Season.Value
                            multieps.Episode.Value = M2.Groups(3).Value
                            multieps.VideoFilePath = eps.VideoFilePath
                            multieps.MediaExtension = eps.MediaExtension
                            multieps.ShowObj = eps.ShowObj 
                            episodearray.Add(multieps)
                            allepisodes(epcount) = Convert.ToDecimal(M2.Groups(3).Value)
                        End If
                        Try
                            S = S.Substring(M2.Groups(3).Index + M2.Groups(3).Value.Length, S.Length - (M2.Groups(3).Index + M2.Groups(3).Value.Length))
                        Catch ex As Exception
                        End Try
                    End If
                    If Cancelled Then Exit Sub
                Loop Until M2.Success = False
                Dim language    As String = eps.ShowLang.Value
                Dim sortorder   As String = eps.sortorder.Value
                Dim tvdbid      As String = eps.Showtvdbid.Value
                Dim imdbid      As String = eps.Showimdbid.Value
                Dim actorsource As String = eps.actorsource.Value
                savepath = episodearray(0).NfoFilePath
                If episodearray.Count > 1 Then
                    For I = 1 To episodearray.Count - 1
                        episodearray(I).MakeSecondaryTo(episodearray(0))
                    Next
                    ReportProgress(, "Multipart episode found: " & vbCrLf)
                    ReportProgress(, "Season: " & episodearray(0).Season.Value & " Episodes, " & vbCrLf)
                    For Each ep In episodearray
                        ReportProgress(, ep.Episode.Value & ", ")
                        ep.Showimdbid.Value = imdbid
                    Next
                    ReportProgress(, "" & vbCrLf)
                End If
                Dim Firstep As Boolean = True
                For Each singleepisode In episodearray
                    If Cancelled Then Exit Sub
                    If singleepisode.Season.Value.Length > 0 Or singleepisode.Season.Value.IndexOf("0") = 0 Then
                        Do Until singleepisode.Season.Value.IndexOf("0") <> 0 Or singleepisode.Season.Value.Length = 1
                            singleepisode.Season.Value = singleepisode.Season.Value.Substring(1, singleepisode.Season.Value.Length - 1)
                        Loop
                        If singleepisode.Episode.Value = "00" Then singleepisode.Episode.Value = "0"
                        If singleepisode.Episode.Value <> "0" Then
                            Do Until singleepisode.Episode.Value.IndexOf("0") <> 0
                                singleepisode.Episode.Value = singleepisode.Episode.Value.Substring(1, singleepisode.Episode.Value.Length - 1)
                            Loop
                        End If
                    End If
                    If sortorder = "" Then sortorder = "default"
                    Dim tempsortorder As String = sortorder
                    If language = "" Then language = "en"
                    If actorsource = "" Then actorsource = "tvdb"
                    ReportProgress(, "Using Settings: TVdbID: " & tvdbid & " SortOrder: " & sortorder & " Language: " & language & " Actor Source: " & actorsource & vbCrLf)
                    If tvdbid <> "" Then
                        progresstext &= " - Scraping..."
                        ReportProgress(progresstext, "", msg_append)
                        Dim tmpaok As Boolean = True
                        If tmpaok Then
                            ReportProgress(, "Scraping body of episode: " & singleepisode.Episode.Value)
                            Dim tempepisode As String = ep_Get(singleepisode, tempsortorder, language)
                            scrapedok = True
                            If tempepisode = Nothing Or tempepisode = "Error" Then
                                scrapedok = False
                                singleepisode.Title.Value = tempepisode
                                ReportProgress(, vbCrLf & "!!! WARNING: This episode: " & singleepisode.Episode.Value & " - could not be found on TVDB" & vbCrLf)
                            ElseIf tempepisode.Contains("Could not connect") Then     'If TVDB unavailable, advise user to try again later
                                scrapedok = False
                                ReportProgress(, vbCrLf & "!!! Issue at TheTVDb, Episode could not be retrieve. Try again later" & vbCrLf)
                            ElseIf tempepisode.Contains("No Results from SP") Then
                                scrapedok = False
                                ReportProgress(, vbCrLf & "!!! Scraping using AirDate found in Filename failed.  Check Episode Filename AiredDate is correct." & vbCrLf)
                            End If
                            If scrapedok Then
                                progresstext &= "OK."
                                ReportProgress(progresstext, MSG_OK & vbCrLf)
                                Dim ratingdone As Boolean = False
                                Dim rating As String    = singleepisode.Rating.Value
                                Dim votes As String     = singleepisode.Votes.Value
                                If Pref.tvdbIMDbRating Then ratingdone = GetEpRating(singleepisode, rating, votes)
                                If Not ratingdone Then
                                    singleepisode.Rating.Value  = rating
                                    singleepisode.Votes.Value   = votes
                                End If
                                singleepisode.PlayCount.Value = "0"
                                singleepisode.ShowId.Value = tvdbid
                                
                                'check file name for Episode source
                                Dim searchtitle As String = singleepisode.NfoFilePath
                                If searchtitle <> "" Then
                                    For i = 0 To Pref.releaseformat.Length - 1
                                        If searchtitle.ToLower.Contains(Pref.releaseformat(i).ToLower) Then
                                            singleepisode.Source.Value = Pref.releaseformat(i)
                                            Exit For
                                        End If
                                    Next
                                End If
                                'ReportProgress(, "Scrape body of episode: " & singleepisode.Episode.Value & " - OK" & vbCrLf)
                                progresstext &= " : Scraped Title - '" & singleepisode.Title.Value & "'"
                                ReportProgress(progresstext)
                                If actorsource = "imdb" And (imdbid <> "" OrElse singleepisode.ImdbId.Value <> "") Then
                                    ReportProgress(, "Scraping actors from IMDB" & vbCrLf)
                                    progresstext &= " : Actors..."
                                    ReportProgress(progresstext)
                                    Dim epid As String = ""
                                    If singleepisode.ImdbId.Value <> "" Then
                                        epid = singleepisode.ImdbId.Value 
                                    Else
                                        epid = GetEpImdbId(imdbid, singleepisode.Season.Value, singleepisode.Episode.Value)
                                    End If
                                    If epid.contains("tt") Then
                                        Dim aok As Boolean = EpGetActorImdb(singleepisode)
                                        If aok Then
                                            ReportProgress(, "Actors scraped from IMDB OK" & vbCrLf)
                                            progresstext &= "OK."
                                            ReportProgress(progresstext)
                                        Else
                                            If String.IsNullOrEmpty(singleepisode.ImdbId.Value) OrElse Not singleepisode.ImdbId.Value.Contains("tt") Then
                                                ReportProgress(, "!!! WARNING: No Episode IMDB Id, Actors not able to be scraped from IMDB" & vbCrLf)
                                            Else
                                                ReportProgress(, "!!! WARNING: Actors not available to scraped from IMDB" & vbCrLf)
                                            End If
                                        End If
                                         If Cancelled Then Exit Sub
                                    Else
                                        ReportProgress(, "Unable To Get Actors From IMDB" & vbCrLf)
                                    End If
                                End If
                                If imdbid = "" Then ReportProgress(, "Failed Scraping Actors from IMDB!!!  No IMDB Id for Show:  " & showtitle & vbCrLf)
                                If Pref.copytvactorthumbs AndAlso Not IsNothing(singleepisode.ShowObj) Then
                                    If singleepisode.ListActors.Count = 0 Then
                                        For each act In singleepisode.ShowObj.ListActors
                                            singleepisode.ListActors.Add(act)
                                        Next
                                    Else
                                        Dim i As Integer = singleepisode.ListActors.Count
                                        For each act In singleepisode.ShowObj.ListActors
                                            Dim q = From x In singleepisode.ListActors Where x.actorname = act.actorname
                                            If q.Count = 1 Then singleepisode.ListActors.Remove(q(0))
                                            i += 1
                                            singleepisode.ListActors.Add(act)
                                            If i = Pref.maxactors Then Exit For
                                        Next
                                    End If
                                End If
                                GetEpHDTags(singleepisode, progresstext)
                            End If
                        Else
                            ReportProgress(, "!!! WARNING: Could not locate this episode on TVDB, or TVDB may be unavailable" & vbCrLf)
                            singleepisode.Title.Value = ""
                            If Pref.TvEpSaveNfoEmpty Then
                                ReportProgress(, "!!! Basic empty nfo created as per selected option." & vbCrLf)
                                GetEpHDTags(singleepisode, progresstext)
                            End If
                            scrapedok = False
                        End If
                    Else
                        ReportProgress(, "!!! WARNING: No TVDB ID is available for this show, please scrape the show using the ""TV Show Selector"" TAB" & vbCrLf)
                        scrapedok = False
                    End If
                    Firstep = False
                Next
                If Not scrapedok AndAlso Not Firstep Then
                    For i = episodearray.Count-1 To 0 Step -1
                        If episodearray(i).Title.Value = "Error" Then
                            Reportprogress(, "!!! WARNING: MultiEpisode No: " & episodearray(i).Episode.Value & " Not Found!  Please check file: " & episodearray(i).VideoFilePath & vbCrLf)
                            episodearray.RemoveAt(i)
                            scrapedok = True
                            Reportprogress(, MSG_ERROR)
                        End If
                    Next
                End If
            End If
            If savepath <> "" AndAlso (scrapedok = True OrElse Pref.TvEpSaveNfoEmpty) Then
                If Cancelled Then Exit Sub
                Dim newnamepath As String = ""
                newnamepath = ep_add(episodearray, savepath, showtitle, scrapedok)
                For Each ep In episodearray
                    ep.NfoFilePath = newnamepath
                Next
                If Cancelled Then Exit Sub
                If episodearray(0).NfoFilePath.IndexOf(episodearray(0).ShowObj.NfoFilePath.Replace("\tvshow.nfo", "")) <> -1 Then
                    Dim epseason As String = episodearray(0).Season.Value
                    Dim Seasonxx As String = episodearray(0).ShowObj.FolderPath + "season" + (If(epseason.ToInt < 10, "0" + epseason, epseason)) + (If(Pref.FrodoEnabled, "-poster.jpg", ".tbn"))
                    If epseason = "0" Then Seasonxx = episodearray(0).ShowObj.FolderPath & "season-specials" & (If(Pref.FrodoEnabled, "-poster.jpg", ".tbn"))
                    If Not File.Exists(Seasonxx) Then TvGetArtwork(episodearray(0).ShowObj.NfoFilePath, False, False, True, False, False)
                    If Pref.seasonfolderjpg AndAlso episodearray(0).ShowObj.FolderPath <> episodearray(0).FolderPath AndAlso (Not File.Exists(episodearray(0).FolderPath & "folder.jpg")) Then
                        If File.Exists(Seasonxx) Then Utilities.SafeCopyFile(Seasonxx, (episodearray(0).FolderPath & "folder.jpg"))
                    End If
                    For Each ep In episodearray
                        PercentDone = 999
                        ReportProgress(ep)
                    Next
                    PercentDone = -1
                    tv_EpisodesMissingUpdate(episodearray)
                End If
            End If
            If Not scrapedok Then Reportprogress(, MSG_ERROR)
        Next
        'bckgroundscanepisodes.ReportProgress(0, progresstext)
    End Sub

    Private Function ep_Get(ByRef singleepisode As TvEpisode, ByVal sortorder As String, ByVal Lang As String) As String

        'Get episode info from XML via API V1 if still active via Pref.tvdbapiv1 boolean
        If Pref.tvdbapiv1 AndAlso ep_GetXML(singleepisode, sortorder, Lang) Then Return "ok"

        'Else load episodes against series from API V2, and populate Episode information.
        Dim tvdbid      As String   = singleepisode.Showtvdbid.Value
        Dim result      As String   = "error"
        Dim episodes    As New List(Of TheTvDB.TvdbEpisode)
        If Previoustvdbid <> tvdbid Then
            Previoustvdbid = tvdbid
            tvdb = New TVDBScraper2(tvdbid, Lang)
        End If
        tvdb.LookupLang     = Lang
        tvdb.TvdbId         = tvdbid 
        tvdb.AllEpDetails   = False
        episodes            = tvdb.Episodes
        Dim seasonsno       As String = singleepisode.Season.Value
        Dim episodeno       As String = singleepisode.Episode.Value
        Dim q = From b In episodes Where b.SeasonNumber = seasonsno AndAlso b.EpisodeNumber = episodeno
        If q.Count = 1 Then
            Dim epidentity As Integer = q(0).Identity
            Dim foundepisode As TheTvDB.TvdbEpisode = tvdb.GetEpisode(epidentity)
            singleepisode.AbsorbTvdbEpisode(foundepisode)
            result = "ok"
        End If
        Return result
    End Function

    Private Function ep_GetXML(ByRef singleepisode As TvEpisode, ByVal sortorder As String, ByVal language As String) As Boolean
        Dim result As Boolean = False
        If language.ToLower.IndexOf(".xml") = -1 Then language = language & ".xml"

        'First try seriesxml data - 'check if present, download if not
        Dim gotseriesxml    As Boolean  = False
        Dim tvdbid          As String   = singleepisode.Showtvdbid.Value
        Dim aired           As String   = singleepisode.Aired.Value
        Dim seasonno        As String   = singleepisode.Season.Value
        Dim episodeno       As String   = singleepisode.Episode.Value
        Dim xmlfile         As String   = Utilities.SeriesXmlPath & tvdbid & ".xml"
        Dim ForceRenew      As Boolean  = singleepisode.UniqueId.Value <> Nothing
        Dim url As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/all/" & language
        If Not File.Exists(Utilities.SeriesXmlPath & tvdbid & ".xml") OrElse ForceRenew Then
            gotseriesxml = DownloadCache.Savexmltopath(url, Utilities.SeriesXmlPath, tvdbid & ".xml", True)
        Else
            'Check series xml isn't older than Five days.  If so, re-download it.
            Dim dtCreationDate As DateTime = File.GetLastWriteTime(xmlfile)
            Dim datenow As DateTime = Date.Now()
            Dim dif As Long = DateDiff(DateInterval.Day, dtCreationDate, datenow)
            If dif > If(aired <> Nothing, 1, 5) Then
                gotseriesxml = DownloadCache.Savexmltopath(url, Utilities.SeriesXmlPath, tvdbid & ".xml", True)
            Else
                gotseriesxml = True
            End If
        End If

        'Check and get episode if present in seriesxml file.
        If gotseriesxml Then
            Dim SeriesInfo As New Tvdb.ShowData
            SeriesInfo.Load(xmlfile)
            If aired <> Nothing Then
                For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
                    If NewEpisode.FirstAired.Value = aired Then
                        singleepisode.AbsorbTvdbEpisode(NewEpisode)
                        result = True
                        Exit For
                    End If
                Next
            End If
            If Not result AndAlso (seasonno <> "-1" And episodeno <> "-1") Then
                If sortorder = "default" Then
                    For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
                        If NewEpisode.EpisodeNumber.Value = episodeno AndAlso NewEpisode.SeasonNumber.Value = seasonno Then
                            singleepisode.AbsorbTvdbEpisode(NewEpisode)
                            result = True
                            Exit For
                        End If
                    Next
                ElseIf sortorder = "dvd" Then
                    For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
                        If NewEpisode.DvdEpisodeNumber.value = (episodeno & ".0") AndAlso NewEpisode.DvdSeason.Value = seasonno Then
                            singleepisode.AbsorbTvdbEpisode(NewEpisode)
                            result = True
                            Exit For
                        End If
                    Next
                End If
            End If
        End If
        
        Return result
    End Function

    Private Function ep_add(ByVal alleps As List(Of TvEpisode), ByVal path As String, ByVal show As String, ByVal scrapedok As Boolean)
        Reportprogress(, "!!! Saving episode" & vbCrLf)
        WorkingWithNfoFiles.ep_NfoSave(alleps, path)
        If Pref.TvDlEpisodeThumb OrElse Pref.autoepisodescreenshot Then
            Reportprogress(, tv_EpisodeFanartGet(alleps(0), Pref.TvDlEpisodeThumb, Pref.autoepisodescreenshot) & vbcrlf)
        Else
            Reportprogress(, "!!! Skipped download of episode thumb" & vbCrLf)
        End If

        If Not scrapedok AndAlso Pref.TvEpSaveNfoEmpty Then Return path

        If Pref.autorenameepisodes Then
            Dim eps As New List(Of String)
            For Each ep In alleps
                eps.Add(ep.Episode.Value)
            Next
            Dim tempspath As String = TVShows.episodeRename(path, alleps(0).Season.Value, eps, show, alleps(0).Title.Value, Pref.TvRenameReplaceSpace, Pref.TvRenameReplaceSpaceDot)

            If tempspath <> "false" Then path = tempspath
        End If
        Return path
    End Function

    Private Function tv_EpisodesMissingUpdate(ByRef newEpList As List(Of TvEpisode)) As Boolean
        Dim Removed As Boolean = False
        Try
            For Each Ep In newEpList
                If File.Exists(Ep.NfoFilePath) Then
                    Dim missingEpNfoPath As String = Utilities.MissingPath & Ep.TvdbId.Value & "." & Ep.Season.Value & "." & Ep.Episode.Value & ".nfo"
                    If Not File.Exists(missingEpNfoPath) Then Continue For
                    File.Delete(missingEpNfoPath)
                    Dim Ep2Remove As New TvEpisode
                    For Each epis As TvEpisode In Cache.TvCache.Episodes 
                        If epis.TvdbId.Value = Ep.TvdbId.Value Then
                            If epis.Season.Value = Ep.Season.Value AndAlso epis.Episode.Value = Ep.Episode.Value AndAlso epis.IsMissing = True Then
                                Ep2Remove = epis 
                                Removed = True
                                Exit For
                            End If
                        End If
                    Next
                    If Removed Then Cache.TvCache.Remove(Ep2Remove)
                End If
            Next
        Catch
        End Try
        Return Removed
    End Function

    Public Function tv_EpisodeFanartGet(ByVal episode As TvEpisode, ByVal doDownloadThumb As Boolean, ByVal doScreenShot As Boolean) As String
        Dim result As String = "!!!  *** Unable to download Episode Thumb ***"
        Dim fpath As String = episode.NfoFilePath.Replace(".nfo", ".tbn")
        Dim paths As New List(Of String)
        If Pref.EdenEnabled AndAlso (Pref.overwritethumbs Or Not File.Exists(fpath)) Then paths.Add(fpath)
        fpath = fpath.Replace(".tbn", "-thumb.jpg")
        If Pref.FrodoEnabled AndAlso (Pref.overwritethumbs Or Not File.Exists(fpath)) Then paths.Add(fpath)
        If paths.Count > 0 Then
            Dim downloadok As Boolean = False
            If doDownloadThumb Then
                If episode.Thumbnail.FileName = Nothing Then
                    Dim tempepisode As Tvdb.Episode = getepisodefromxml(episode.ShowId.Value, episode.sortorder.Value, episode.Season.value, episode.Episode.Value, episode.ShowLang.Value, True)
                    If tempepisode.ThumbNail.Value <> Nothing Then episode.Thumbnail.FileName = tempepisode.ThumbNail.Value
                End If
                If episode.Thumbnail.FileName <> Nothing AndAlso episode.Thumbnail.FileName <> "http://www.thetvdb.com/banners/" Then
                    Dim url As String = episode.Thumbnail.FileName
                    If Not url.IndexOf("http") = 0 And url.IndexOf(".jpg") <> -1 Then url = episode.Thumbnail.Url 
                    If url <> Nothing AndAlso url.IndexOf("http") = 0 AndAlso url.IndexOf(".jpg") <> -1 Then
                        downloadok = DownloadCache.SaveImageToCacheAndPaths(url, paths, True, , ,Pref.overwritethumbs)
                    Else
                        result = "!!! No thumbnail to download"
                    End If
                    If downloadok Then result = "!!! Episode Thumb downloaded"
                Else
                    result = "!!! No thumbnail to download"
                End If
            End If
            If Not downloadok AndAlso doScreenShot Then
                Dim cachepathandfilename As String = Utilities.CreateScrnShotToCache(episode.VideoFilePath, paths(0), Pref.ScrShtDelay)
                If Not cachepathandfilename = "" Then
                    Dim imagearr() As Integer = GetAspect(episode)
                    If Pref.tvscrnshtTVDBResize AndAlso Not imagearr(0) = 0 Then
                        DownloadCache.CopyAndDownSizeImage(cachepathandfilename, paths(0), imagearr(0), imagearr(1))
                    Else
                        File.Copy(cachepathandfilename, paths(0), Pref.overwritethumbs)
                    End If
                    If paths.Count > 1 Then File.Copy(paths(0), paths(1), Pref.overwritethumbs)
                    result = "!!! No Episode thumb to download, Screenshot saved"
                Else
                    result = "!!! No Episode thumb to download, Screenshot Could not be saved!"
                End If
            End If
        ElseIf paths.Count = 0 Then
            result = "!!! Episode Thumb(s) already exist and are not set to overwrite"
        End If
        Return result
    End Function

    Private Function GetAspect(ep As TvEpisode)
        Dim thisarray(2) As Integer
        thisarray(0) = 400
        thisarray(1) = 225
        Try
            If ep.StreamDetails.Video.Width.Value is Nothing Then ep.GetFileDetails
            Dim epw As Integer = ep.StreamDetails.Video.Width.Value.ToInt
            Dim eph As Integer= ep.StreamDetails.Video.Height.Value.ToInt
            Dim ThisAsp As Double = epw/eph
            If ThisAsp < 1.37 Then thisarray(1) = 300     'aspect greater than Industry Standard of 1.37:1 is classed as WideScreen
        Catch
            thisarray(0) = 0
        End Try
        Return thisarray
    End Function

    Private Sub tv_NewFind(ByVal tvpath As String, ByVal pattern As String)
		Dim episode As New List(Of TvEpisode)
		Dim propfile As Boolean = False
		Dim allok As Boolean = False
		Dim dir_info As New DirectoryInfo(tvpath)

		Dim fs_infos() As String = Directory.GetFiles(tvpath, "*" & pattern, IO.SearchOption.TopDirectoryOnly)
		Dim counter As Integer = 1
		Dim counter2 As Integer = 1
		For Each FilePath As String In fs_infos
			Dim filename_video As String = FilePath
			Dim filename_nfo As String = filename_video.Replace(Path.GetExtension(filename_video), ".nfo")
			If File.Exists(filename_nfo) Then
				If ep_NfoValidate(filename_nfo) = False And Pref.renamenfofiles = True Then
					Dim movefilename As String = filename_nfo.Replace(Path.GetExtension(filename_nfo), ".info")
					Try
						If File.Exists(movefilename) Then Utilities.SafeDeleteFile(movefilename)
						File.Move(filename_nfo, movefilename)
					Catch ex As Exception
						Utilities.SafeDeleteFile(movefilename)
					End Try
				End If
			End If
			If Not File.Exists(filename_nfo) Then
				Dim add As Boolean = True
				If pattern = ".vob" Then 'If a vob file is detected, check that it is not part of a dvd file structure
					Dim name As String = filename_nfo
					name = name.Replace(Path.GetFileName(name), "VIDEO_TS.IFO")
					If File.Exists(name) Then add = False
				End If
				If pattern = ".rar" Then
					Dim tempmovie As String = String.Empty
					Dim tempint2 As Integer = 0
					Dim tempmovie2 As String = FilePath
					If Path.GetExtension(tempmovie2).ToLower = ".rar" Then
						If File.Exists(tempmovie2) = True Then
							If File.Exists(tempmovie) = False Then
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
											rarname = tempmovie.Replace(".nfo", ".rar")
											If rarname.ToLower.IndexOf(".part1.rar") <> -1 Then
												rarname = rarname.Replace(".part1.rar", ".nfo")
												If File.Exists(rarname) Then
													stackrarexists = True
													tempmovie = rarname
												Else
													stackrarexists = False
													tempmovie = rarname
												End If
											End If
											If rarname.ToLower.IndexOf(".part01.rar") <> -1 Then
												rarname = rarname.Replace(".part01.rar", ".nfo")
												If File.Exists(rarname) Then
													stackrarexists = True
													tempmovie = rarname
												Else
													stackrarexists = False
													tempmovie = rarname
												End If
											End If
											If rarname.ToLower.IndexOf(".part001.rar") <> -1 Then
												rarname = rarname.Replace(".part001.rar", ".nfo")
												If File.Exists(rarname) Then
													tempmovie = rarname
													stackrarexists = True
												Else
													stackrarexists = False
													tempmovie = rarname
												End If
											End If
											If rarname.ToLower.IndexOf(".part0001.rar") <> -1 Then
												rarname = rarname.Replace(".part0001.rar", ".nfo")
												If File.Exists(rarname) Then
													tempmovie = rarname
													stackrarexists = True
												Else
													stackrarexists = False
													tempmovie = rarname
												End If
											End If
										Else
											add = False
										End If
									Else
										'remove = True
									End If
								Else
									add = False
								End If
							End If
						End If
					End If
				End If
				Dim truefilename As String = Utilities.GetFileNameFromPath(filename_video)
				If truefilename.Substring(0, 2) = "._" Then add = False
				If add = True Then
					Dim newep As New TvEpisode
					newep.NfoFilePath = filename_nfo
					newep.VideoFilePath = filename_video
					newep.MediaExtension = Path.GetExtension(filename_video)
					newEpisodeList.Add(newep)
				End If
			End If
		Next
		fs_infos = Nothing
	End Sub

    Private Sub GetEpHDTags(ByRef singleepisode As TvEpisode, ByRef progresstext As String)
        If Pref.enabletvhdtags = True Then
            progresstext &= " : HD Tags..."
            ReportProgress(progresstext, msg_append)
            Dim fileStreamDetails As StreamDetails = Pref.Get_HdTags(Utilities.GetFileName(singleepisode.VideoFilePath))
            If Not IsNothing(fileStreamDetails) Then
                singleepisode.StreamDetails.Video = fileStreamDetails.Video
                For Each audioStream In fileStreamDetails.Audio
                    singleepisode.StreamDetails.Audio.Add(audioStream)
                Next
                For Each substrm In fileStreamDetails.Subtitles
                    singleepisode.StreamDetails.Subtitles.Add(substrm)
                Next
                If Not String.IsNullOrEmpty(singleepisode.StreamDetails.Video.DurationInSeconds.Value) Then
                    Dim tempstring As String = singleepisode.StreamDetails.Video.DurationInSeconds.Value
                    If Pref.intruntime Then
                        singleepisode.Runtime.Value = Math.Round(tempstring / 60).ToString
                    Else
                        singleepisode.Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
                    End If
                    progresstext &= "OK."
                    ReportProgress(progresstext, msg_append)
                End If
            End If
        End If
    End Sub

    Private Function GetEpRating(ByRef tvep As TvEpisode, ByVal Rating As String, ByVal Votes As String) As Boolean
        Dim ratingdone As Boolean = False
        If Pref.tvdbIMDbRating Then
            Dim aok As Boolean = False
            
            '''If no Ep IMDB Id, try getting from TMDb first.
            If String.IsNullOrEmpty(tvep.ImdbId.Value) Then aok = gettmdbepid(tvep)

            '''Check if not still empty and if is IMDB Id
            If Not String.IsNullOrEmpty(tvep.ImdbId.Value) AndAlso tvep.ImdbId.Value.StartsWith("tt") Then aok = True

            '''Try IMDb Direct if we have the IMDB Id
            If aok Then ratingdone = ep_getIMDbRating(tvep.ImdbId.Value, tvep.Rating.Value, tvep.Votes.Value)

            '' Disable Omdbapi to get episode ratings till we get a API key, or site becomes free again.
            ''If no success, try Omdbapi (Omdbapi is much slower)
            'If Not ratingdone Then ratingdone = epGetImdbRatingOmdbapi(tvep)
        End If

        ''' Fallback to TVDb if nothing from IMDb
        If Not ratingdone Then
            tvep.Rating.Value   = Rating
            tvep.Votes.Value    = Votes
            ratingdone = True
        End If
        Return ratingdone
    End Function

    Private Function epGetImdbRatingOmdbapi(ByRef ep As TvEpisode) As Boolean
        Dim GotEpImdbId As Boolean = False
        If (String.IsNullOrEmpty(ep.Showimdbid.Value) OrElse ep.Showimdbid.Value = "0") AndAlso String.IsNullOrEmpty(ep.ImdbId.Value) Then Return False
        If Not ep.Showimdbid.Value.StartsWith("tt") AndAlso (String.IsNullOrEmpty(ep.Season.Value) AndAlso String.IsNullOrEmpty(ep.Episode.Value)) Then Return False
        Dim url As String = Nothing

        If Not String.IsNullOrEmpty(ep.ImdbId.Value) AndAlso ep.ImdbId.Value.StartsWith("tt") Then GotEpImdbId = True

        If Not GotEpImdbId Then url = String.Format("{0}?i={1}&Season={2}&r=xml", Pref.Omdbapiurl, ep.Showimdbid.Value, ep.Season.Value)

        Dim imdb As New Classimdb
        If Not GotEpImdbId Then
            Dim result As String = imdb.loadwebpage(Pref.proxysettings, url, True, 5)
            If result.Contains("error") Then Return False
            Dim adoc As New XmlDocument
            adoc.LoadXml(result)
            If adoc("root").HasAttribute("response") AndAlso adoc("root").Attributes("response").Value = "False" Then Return False
            If adoc("root").HasAttribute("Response") AndAlso adoc("root").Attributes("Response").Value = "False" Then Return False
            For each thisresult As XmlNode In adoc("root")
                If Not IsNothing(thisresult.Attributes.ItemOf("Episode")) Then
                    Dim TmpValue As String = thisresult.Attributes("Episode").Value
                    If TmpValue <> "" AndAlso TmpValue = ep.Episode.Value Then
                        Dim epimdbid As String = thisresult.Attributes("imdbID").Value
                        ep.ImdbId.Value = epimdbid
                        Exit For
                    End If
                End If
            Next
            If Not ep.ImdbId.Value.StartsWith("tt") Then Return False
        End If

        url = String.Format("{0}?i={1}&r=xml", Pref.Omdbapiurl, ep.ImdbId.Value)
        Dim result2 As String = imdb.loadwebpage(Pref.proxysettings, url, True, 5)
        If result2 = "error" Then Return False
        Dim bdoc As New XmlDocument
        bdoc.LoadXml(result2)
        If bdoc("root").HasAttribute("response") AndAlso bdoc("root").Attributes("response").Value = "False" Then Return False
        If bdoc("root").HasAttribute("Response") AndAlso bdoc("root").Attributes("Response").Value = "False" Then Return False

        For each thisresult As XmlNode In bdoc("root")
            If Not IsNothing(thisresult.Attributes.ItemOf("imdbRating")) Then
                Dim ratingVal As String = thisresult.Attributes("imdbRating").Value
                If ratingVal.ToLower = "n/a" Then Return False
                ep.Rating.Value = ratingVal
            End If
            If Not IsNothing(thisresult.Attributes.ItemOf("imdbVotes")) Then
                Dim voteval As String = thisresult.Attributes("imdbVotes").Value
                If Not voteval.ToLower = "n/a" Then ep.Votes.Value = voteval
            End If
        Next
        
        Return True
    End Function

    Private Function gettmdbepid(ByRef ep As TvEpisode) As Boolean
        Dim url As String = String.Format("https://api.themoviedb.org/3/find/{0}?api_key={1}&language=en-US&external_source=tvdb_id", ep.ShowId.Value, Utilities.TMDBAPI)
        Dim imdb As New Classimdb
        Try
            Dim reply As String = imdb.loadwebpage(Pref.proxysettings, url, True)
            If reply <> "error" Then
                Dim m As Match = Regex.Match(reply, """id"":(.*?),""")
                If Not m.Success Then Return False
                Dim pie As String = m.Groups(1).Value.Trim
                url = String.Format("https://api.themoviedb.org/3/tv/{0}/season/{1}/episode/{2}/external_ids?api_key={3}&language=en-US", pie, ep.Season.Value, ep.Episode.Value, Utilities.TMDBAPI)
                Dim reply2 As String = imdb.loadwebpage(Pref.proxysettings, url, True)
                If reply2 = "" Then Return False
                Dim n As Match = Regex.Match(reply2, """imdb_id"":""(.*?)"",""")
                If n.Success Then
                    Dim epimdbid As String = n.Groups(1).Value.Trim
                    If epimdbid.StartsWith("tt") AndAlso epimdbid.Length = 9 Then
                        ep.ImdbId.Value = epimdbid
                        Return True
                    End If
                End If
            End If
        Catch
        End Try
        Return False
    End Function

    Private Function GetEpImdbId(ByVal ImdbId As String, ByVal SeasonNo As String, ByVal EpisodeNo As String) As String
        Dim url = "http://www.imdb.com/title/" & ImdbId & "/episodes?season=" & SeasonNo
        Dim webpage As New List(Of String)
        Dim s As New Classimdb
        webpage.Clear()
        webpage = s.loadwebpage(Pref.proxysettings, url,False,10)
        Dim webPg As String = String.Join( "" , webpage.ToArray() )
        Dim matchstring As String = "<strong><a href=""/title/tt"
        For f = 0 to webpage.Count -1
            Dim m As Match = Regex.Match(webpage(f), matchstring)
            If m.Success AndAlso webpage(f).Contains("ttep_ep"&EpisodeNo) Then
                Dim tmp As String = webpage(f)
                Dim n As Match = Regex.Match(tmp, "(tt\d{7})")
                If n.Success = True Then
                    url = n.Value
                    Exit For
                End If
            End If
        Next
        Return url
    End Function

    Private Function EpGetActorImdb(ByRef NewEpisode As TvEpisode) As Boolean
        Dim imdbscraper As New Classimdb
        Dim success As Boolean = False
        If String.IsNullOrEmpty(NewEpisode.ImdbId.Value) Then Return success
        Dim actorlist As List(Of str_MovieActors) = imdbscraper.GetImdbActorsList(Pref.imdbmirror, NewEpisode.ImdbId.Value, Pref.maxactors)
        Dim workingpath As String = ""
        If Pref.actorseasy And Not Pref.tvshowautoquick Then
            workingpath = NewEpisode.ShowObj.FolderPath
            workingpath = workingpath & ".actors\"
            Utilities.EnsureFolderExists(workingpath)
        End If

        Dim listofactors As List(Of str_MovieActors) = IMDbActors(actorlist, success, workingpath)
        If Not success Then Return success

        NewEpisode.ListActors.Clear()
        Dim i As Integer = 0
        For each listact In listofactors
            i += 1
            NewEpisode.ListActors.Add(listact)
            If i > Pref.maxactors Then Exit For
        Next
        Return success
    End Function

#End Region

#Region "BatchRescrape Routines"

    Function NeedTVDb(rl As str_TvShowBatchWizard)
        Return rl.doShowActors OrElse rl.doShowBody OrElse rl.doshows
    End Function

    Public Sub BatchRescrape()
        Try
            PercentDone = 0
            Dim showprocesscount As Integer = 0
            Dim progresstext As String = ""
            Dim progcount As Integer = 0
            Dim shcachecount As Integer = Cache.TvCache.Shows.Count
            Dim done As Integer = 0
            If singleshow Then
                For x = Cache.TvCache.Shows.Count - 1 To 0 Step -1
                    If Cache.TvCache.Shows(x).TvdbId.Value = SingleShowid Then
                        shcachecount = x + 1
                        Exit For
                    End If
                Next
            End If
            For f = shcachecount - 1 To 0 Step -1
                If Cache.TvCache.Shows(f).State = Media_Companion.ShowState.Open Or Cache.TvCache.Shows(f).State = -1 Or tvBatchList.includeLocked = True Then
                    If tvBatchList.doEpisodes = True Then
                        showprocesscount += (Cache.TvCache.Shows(f).Episodes.Count - Cache.TvCache.Shows(f).MissingEpisodes.Count)
                        showprocesscount += 1
                        progcount += 1
                    Else
                        showprocesscount += 1
                        progcount += 1
                    End If
                End If
                If singleshow Then Exit For
            Next
            Dim showsdone As Integer = 0
            Dim showcounter As Integer = 0
            For f = shcachecount - 1 To 0 Step -1
                If tvBatchList.RewriteAllNFOs Then
                    If Cache.TvCache.Shows(f).State = 0 Or tvBatchList.includeLocked = True Then
                        Dim SelectedSeries As New TvShow
                        SelectedSeries = nfoFunction.tvshow_NfoLoad(Cache.TvCache.Shows(f).NfoFilePath)
                        Call FixTvActorsNfo(SelectedSeries)
                        Call nfoFunction.tvshow_NfoSave(SelectedSeries, True)
                        For g = Cache.TvCache.Shows(f).Episodes.Count - 1 To 0 Step -1
                            Dim epcount As Integer = Cache.TvCache.Shows(f).Episodes.Count
                            progresstext = "Rewriting nfo's of Show: " & Cache.TvCache.Shows(f).Title.Value & ", Episode: " & epcount - g & " of " & epcount & ", Episode: " & Cache.TvCache.Shows(f).Episodes(g).Season.Value & "x" & Cache.TvCache.Shows(f).Episodes(g).Episode.Value & " - " & Cache.TvCache.Shows(f).Episodes(g).Title.Value
                            PercentDone = If(done = showprocesscount, 100, (100 / showprocesscount) * (done+1))
                            If Cache.TvCache.Shows(f).Episodes(g).IsMissing Then
                                progresstext &= "Skip Missing episode"
                                Continue For
                            End If
                            ReportProgress(progresstext)
                            Dim listofepisodes As New List(Of TvEpisode)
                            listofepisodes.Clear()
                            listofepisodes = WorkingWithNfoFiles.ep_NfoLoad(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)
                            WorkingWithNfoFiles.ep_NfoSave(listofepisodes, listofepisodes(0).NfoFilePath)
                        Next
                    End If
                    If singleshow Then Exit For
                    Continue For
                End If

                If NeedTVDb(tvBatchList) Then IniTVdb

                If Cache.TvCache.Shows(f).State = Media_Companion.ShowState.Open OrElse Cache.TvCache.Shows(f).State = -1 OrElse tvBatchList.includeLocked = True Then
                    showcounter += 1
                    progresstext = "Working on Show: " & showcounter.ToString & " of " & progcount
                    PercentDone = If(done = showprocesscount, 100, (100 / showprocesscount) * (done+1))
                    ReportProgress(progresstext)
                    Dim editshow As New TvShow
                    editshow = nfoFunction.tvshow_NfoLoad(Cache.TvCache.Shows(f).NfoFilePath)
                    If tvBatchList.doShowActors Then editshow.ListActors.Clear()

                    'Dim tvdbstuff As New TVDBScraper
                    Dim tvseriesdata As New Tvdb.ShowData
                    Dim language As String = editshow.Language.Value
                    If language = "" Then language = "en"
                    If tvBatchList.shSeries Then
                        Dim aok As Boolean = GetSeriesXml(editshow.TvdbId.Value, language, Utilities.SeriesXmlPath)
                        If aok Then
                            progresstext &= "Updated Series.xml data"
                        Else
                            progresstext &= "Failed to update xml data"
                        End If
                        ReportProgress(progresstext)
                        If singleshow Then Exit For
                        Continue For
                    End If
                    Dim NewSeries As New TvShow
                    If NeedTVDb(tvBatchList) Then tvdb.TvdbId = editshow.TvdbId.Value
                    tvseriesdata = GetShow(editshow.TvdbId.Value, language, Utilities.SeriesXmlPath)
                    If Not tvseriesdata.FailedLoad Then
                        NewSeries.AbsorbTvdbSeries(tvseriesdata.Series(0))
                    Else
                        tvdb = New TVDBScraper2(editshow.TvdbId.Value, language)
                        Dim tmpseries As New TheTvDB.TvdbSeries
                        tmpseries = tvdb.Series
                        If Not tvdb.SeriesNotFound Then
                            NewSeries.AbsorbTvdbSeries(tmpseries)
                        Else
                            progresstext &= "Failed to load xml data"
                            ReportProgress(progresstext)
                            Continue For
                        End If
                    End If
                    If tvBatchList.doShows = True Then
                        If tvBatchList.doShowBody = True Or tvBatchList.doShowActors = True Then
                            Try
                                UpdateProperty(NewSeries.ImdbId.Value       , editshow.ImdbId.Value     , True                  )
                                UpdateProperty(NewSeries.Mpaa.Value         , editshow.Mpaa.Value       , tvBatchList.shMpaa    )
                                UpdateProperty(NewSeries.Premiered.Value    , editshow.Premiered.Value  , tvBatchList.shYear    )
                                UpdateProperty(NewSeries.Year.Value         , editshow.Year.Value       , tvBatchList.shYear    )
                                UpdateProperty(NewSeries.Genre.Value        , editshow.Genre.Value      , tvBatchList.shGenre   )
                                UpdateProperty(NewSeries.Studio.Value       , editshow.Studio.Value     , tvBatchList.shStudio  )
                                UpdateProperty(NewSeries.Plot.Value         , editshow.Plot.Value       , tvBatchList.shPlot    )
                                UpdateProperty(NewSeries.Runtime.Value      , editshow.Runtime.Value    , tvBatchList.shRuntime )
                                UpdateProperty(NewSeries.Status.Value       , editshow.Status.Value     , tvBatchList.shStatus  )
                                
                                Dim episodeguideurl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & editshow.TvdbId.Value & "/all/" & language & ".zip"
                                editshow.EpisodeGuideUrl.Value = ""
                                editshow.Url.Value = episodeguideurl
                                editshow.Url.Node.SetAttributeValue("cache", editshow.TvdbId.Value)
                                editshow.Url.AttachToParentNode(editshow.EpisodeGuideUrl.Node)
                                If tvBatchList.shRating Then
                                    If Pref.tvdbIMDbRating Then ep_getIMDbRating(NewSeries.ImdbId.Value, NewSeries.Rating.Value, NewSeries.Votes.Value)
                                    UpdateProperty(NewSeries.Rating.Value   , editshow.Rating.Value     , True)
                                    UpdateProperty(NewSeries.Votes.Value    , editshow.Votes.Value      , True)
                                End If
                                If tvBatchList.doShowActors Then
                                    If editshow.TvShowActorSource.Value = Nothing Then editshow.TvShowActorSource.Value = If((Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3), "tvdb", "imdb")
                                    If editshow.TvShowActorSource.Value = "tvdb" Then TvGetActorTvdb(editshow)
                                    If editshow.TvShowActorSource.Value = "imdb" Then TvGetActorImdb(editshow)
                                End If
                            Catch ex As Exception
                            End Try
                            Call nfoFunction.tvshow_NfoSave(editshow, True)
                            editshow.UpdateTreenode()
                        End If

                        'Posters, Fanart and Season art
                        If tvBatchList.doShowArt = True Then
                            If tvBatchList.shDelArtwork Then Form1.TvDeleteShowArt(editshow, False) 'Cache.TvCache.Shows(f), False)
                            If tvBatchList.shFanart orElse tvBatchList.shPosters OrElse tvBatchList.shSeason OrElse tvBatchList.shXtraFanart Then
                                TvGetArtwork(editshow.NfoFilePath, tvBatchList.shFanart, tvBatchList.shPosters, tvBatchList.shSeason, tvBatchList.shXtraFanart, force:=False)
                            End If
                            If tvBatchList.shFanartTvArt Then TvFanartTvArt(editshow, False) 'We're only looking for missing art from Fanart.Tv

                            'If selected, copy Series banner to season banner if no season banner downloaded.
                            If tvBatchList.shBannerMain AndAlso Pref.FrodoEnabled Then
                                Dim mainbanner As String = Cache.TvCache.Shows(f).ImageBanner.Path
                                If File.Exists(mainbanner) Then
                                    For each seas As TvSeason In Cache.TvCache.Shows(f).Seasons.values
                                        Dim SeasonNo As String = seas.SeasonNode.Text.Replace("Season ", "")
                                        If SeasonNo.ToLower = "specials" Then SeasonNo = "-" & SeasonNo.ToLower
                                        Dim SeasonBanner As String = Cache.TvCache.Shows(f).FolderPath & "season" & SeasonNo & "-banner.jpg"
                                        If Not File.Exists(SeasonBanner) Then
                                            Utilities.SafeCopyFile(mainbanner, SeasonBanner)
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                    If tvBatchList.doEpisodes = True Then
                        Dim i As Integer = 0
                        Dim TotalEpisodes As Integer = Cache.TvCache.Shows(f).Episodes.Count - Cache.TvCache.Shows(f).MissingEpisodes.count
                        For g = Cache.TvCache.Shows(f).Episodes.Count - 1 To 0 Step -1
                            If Cache.TvCache.Shows(f).Episodes(g).IsMissing Then Continue For
                            i += 1
                            progresstext = "Working on Show: " & Cache.TvCache.Shows(f).Title.Value & " Episode: " & i & " of " & TotalEpisodes & ", Episode: " & Cache.TvCache.Shows(f).Episodes(g).Season.Value & "x" & Cache.TvCache.Shows(f).Episodes(g).Episode.Value & " - " & Cache.TvCache.Shows(f).Episodes(g).Title.Value
                            PercentDone = If(done = showprocesscount, 100, (100 / showprocesscount) * (done+1))
                            ReportProgress(progresstext)
                            Dim actorsource As String = Cache.TvCache.Shows(f).EpisodeActorSource.Value
                            If actorsource = "" Then actorsource = "tvdb"
                            Dim epid As String = Cache.TvCache.Shows(f).Episodes(g).UniqueId.Value
                            Dim Episodedata As New TvEpisode
                            Dim epfound As Boolean = False
                            For Each NewEpisode As Tvdb.Episode In tvseriesdata.Episodes
                                If NewEpisode.SeasonNumber.Value = Cache.TvCache.Shows(f).Episodes(g).Season.Value
                                    If NewEpisode.EpisodeNumber.Value = Cache.TvCache.Shows(f).Episodes(g).Episode.Value
                                        Episodedata.AbsorbTvdbEpisode(NewEpisode)
                                        epfound = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If Not epfound Then
                                Dim sortorder As String = Cache.TvCache.Shows(f).SortOrder.Value
                                If sortorder = "" Then sortorder = "default"
                                Dim tvdbid As String = Cache.TvCache.Shows(f).TvdbId.Value
                                Dim imdbid As String = Cache.TvCache.Shows(f).ImdbId.Value
                                Dim seasonno As String = Cache.TvCache.Shows(f).Episodes(g).Season.Value
                                Dim episodeno As String = Cache.TvCache.Shows(f).Episodes(g).Episode.Value
                                Episodedata = getepfromxml(tvdbid, sortorder, seasonno, episodeno, language, True)
                                If Episodedata.FailedLoad AndAlso Not IsNothing(epid) Then
                                    If IsNothing(tvdb) Then tvdb = New TVDBScraper2()
                                    Dim tmpepisode As TheTvDB.TvdbEpisode = tvdb.GetEpisode(epid)
                                    If Not tvdb.EpNotFound Then
                                        Episodedata.AbsorbTvdbEpisode(tmpepisode)
                                    Else
                                        progresstext = "tvdb was unable to process the following show episode." & vbCrLf & Cache.TvCache.Shows(f).Title.Value & " - S" & Utilities.PadNumber(Cache.TvCache.Shows(f).Episodes(g).Season.Value, 2) & "E" & Utilities.PadNumber(Cache.TvCache.Shows(f).Episodes(g).Episode.Value, 2) & " " & Cache.TvCache.Shows(f).Episodes(g).Title.Value
                                        ReportProgress(progresstext)
                                        Continue For
                                    End If
                                End If
                            End If
                            If tvBatchList.doEpisodeBody Or (tvBatchList.doEpisodeActors And Cache.TvCache.Shows(f).EpisodeActorSource.Value <> "") Or (tvBatchList.doEpisodeArt) Then
                                Dim listofnewepisodes As New List(Of TvEpisode)
                                listofnewepisodes.Clear()
                                listofnewepisodes = WorkingWithNfoFiles.ep_NfoLoad(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)
                                For h = listofnewepisodes.Count - 1 To 0 Step -1
                                    If listofnewepisodes(h).Season.Value = Cache.TvCache.Shows(f).Episodes(g).Season.Value And listofnewepisodes(h).Episode.Value = Cache.TvCache.Shows(f).Episodes(g).Episode.Value Then
                                        Dim newactors As New List(Of str_MovieActors)
                                        Dim episodescreenurl As String = ""
                                        Dim epimdbid As String = listofnewepisodes(h).ImdbId.Value
                                        If tvBatchList.epIMDBId OrElse (Not epimdbid.StartsWith("tt") AndAlso epimdbid.Length < 9) Then
                                            UpdateProperty(Episodedata.ImdbId.Value,  listofnewepisodes(h).ImdbId.Value, True)
                                        End If
                                        UpdateProperty(Episodedata.UniqueId.Value, listofnewepisodes(h).UniqueId.Value, True)
                                        UpdateProperty(Episodedata.ShowId.Value, listofnewepisodes(h).ShowId.Value, True)
                                        UpdateProperty(Episodedata.ImdbId.Value, listofnewepisodes(h).Showimdbid.Value, True)
                                        UpdateProperty(Episodedata.Aired.Value, listofnewepisodes(h).Aired.Value, tvBatchList.epAired)
                                        UpdateProperty(Episodedata.Plot.Value, listofnewepisodes(h).Plot.Value, tvBatchList.epPlot)
                                        UpdateProperty(Episodedata.Director.Value, listofnewepisodes(h).Director.Value, tvBatchList.epDirector)
                                        UpdateProperty(Episodedata.Credits.Value, listofnewepisodes(h).Credits.Value, tvBatchList.epCreateScreenshot)
                                        UpdateProperty(Episodedata.Title.Value, listofnewepisodes(h).Title.Value, tvBatchList.epTitle)
                                        If tvBatchList.epRating Then GetEpRating(listofnewepisodes(h), Episodedata.Rating.Value, Episodedata.Votes.Value)
                                        If tvBatchList.epActor Then
                                            If actorsource = "tvdb" Then
                                                listofnewepisodes(h).ListActors.Clear()
                                                For each actor In Episodedata.ListActors
                                                    listofnewepisodes(h).ListActors.Add(actor)
                                                Next
                                            ElseIf actorsource = "imdb" Then
                                                If String.IsNullOrEmpty(epimdbid ) Then epimdbid = GetEpImdbId(listofnewepisodes(h).Showimdbid.Value, listofnewepisodes(h).Season.Value, listofnewepisodes(h).Episode.Value)
                                                If epimdbid.Contains("tt") Then EpGetActorImdb(listofnewepisodes(h))
                                            End If
                                            If Pref.copytvactorthumbs AndAlso Not IsNothing(Cache.TvCache.Shows(f)) Then
                                                If listofnewepisodes(h).ListActors.Count = 0 Then
                                                    For each act In Cache.TvCache.Shows(f).ListActors
                                                        listofnewepisodes(h).ListActors.Add(act)
                                                    Next
                                                Else
                                                    Dim t As Integer = 0
                                                    For each act In Cache.TvCache.Shows(f).ListActors
                                                        Dim q = From x In listofnewepisodes(h).ListActors Where x.actorname = act.actorname
                                                        If q.Count = 1 Then listofnewepisodes(h).ListActors.Remove(q(0))
                                                        t += 1
                                                        listofnewepisodes(h).ListActors.Add(act)
                                                        If t = Pref.maxactors Then Exit For
                                                    Next
                                                End If
                                            End If
                                        End If
                                        If tvBatchList.doEpisodeArt AndAlso (tvBatchList.epdlThumbnail OrElse tvBatchList.epCreateScreenshot) Then
                                            listofnewepisodes(h).Thumbnail.FileName = Episodedata.Thumbnail.FileName
                                            progresstext = tv_EpisodeFanartGet(listofnewepisodes(h), tvBatchList.epdlThumbnail, tvBatchList.epCreateScreenshot).Replace("!!! ", "")
                                        End If

                                        WorkingWithNfoFiles.ep_NfoSave(listofnewepisodes, listofnewepisodes(0).NfoFilePath)
                                        listofnewepisodes(h).UpdateTreenode()
                                        Exit For
                                    End If
                                Next
                            End If

                            If tvBatchList.doEpisodeMediaTags = True Then
                                Dim listofnewepisodes As New List(Of TvEpisode)
                                listofnewepisodes.Clear()
                                listofnewepisodes = WorkingWithNfoFiles.ep_NfoLoad(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)
                                For h = listofnewepisodes.Count - 1 To 0 Step -1
                                    listofnewepisodes(h).GetFileDetails()
                                    If Not String.IsNullOrEmpty(listofnewepisodes(h).StreamDetails.Video.DurationInSeconds.Value) Then
                                        Try
                                            Dim tempstring As String
                                            tempstring = listofnewepisodes(h).StreamDetails.Video.DurationInSeconds.Value
                                            If Pref.intruntime Then
                                                listofnewepisodes(h).Runtime.Value = Math.Round(tempstring / 60).ToString
                                            Else
                                                listofnewepisodes(h).Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
                                            End If
                                        Catch
                                        End Try
                                        WorkingWithNfoFiles.ep_NfoSave(listofnewepisodes, listofnewepisodes(0).NfoFilePath)
                                    End If
                                Next
                            End If
                            done += 1
                        Next
                    End If
                    done += 1
                    If singleshow Then Exit For
                End If
            Next
            singleshow = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub FixTvActorsNfo(ByRef TvSeries As TvShow)
        'If XBMC networkpath changed, update actor thumb path
        For Each tvActor In TvSeries.listactors
            If Pref.actorsave AndAlso tvActor.actorid <> "" Then
                If Not String.IsNullOrEmpty(Pref.actorsavepath) Then
                    Dim tempstring As String = Pref.actorsavepath
                    Dim workingpath As String = ""
                    If Pref.actorsavealpha Then
                        Dim actorfilename As String = tvActor.actorname.Replace(" ", "_") & "_" & If(Pref.LocalActorSaveNoId, "", tvActor.actorid) & ".jpg"
                        tempstring = tempstring & "\" & actorfilename.Substring(0, 1) & "\"
                        workingpath = tempstring & actorfilename
                    Else
                        tempstring = tempstring & "\" & tvActor.actorid.Substring(tvActor.actorid.Length - 2, 2) & "\"
                        workingpath = tempstring & tvActor.actorid & ".jpg"
                    End If
                    If Not String.IsNullOrEmpty(Pref.actornetworkpath) Then
                        If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                            tvActor.actorthumb = workingpath.Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("\", "/")
                        Else
                            tvActor.actorthumb = workingpath.Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("/", "\")
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    
    Sub UpdateProperty(Of T) (ByVal fromField As T, ByRef toField As T,  Optional rescrape As Boolean=True, Optional ifempty As Boolean = False )  
        If Not rescrape         Then Exit Sub
        If IsNothing(fromField) Then Exit Sub
        If ifempty AndAlso Not IsNothing(toField) Then Exit Sub
        toField = fromField
    End Sub    

#End Region
#Region "Misc Routines"

    Sub GetSeriesDataComplete(ByVal seriesid As String)
        Dim tvdbseries As New TheTvDB.TvdbSeries
        Dim tvdb2 As New TVDBScraper2(seriesid, _lang)
        tvdbseries = tvdb2.ReturnSeriesandEpisodes
    End Sub
#End Region

#Region "original routines"

    Public Function GetPosterList(ByVal TvdbId As String, ByVal ReturnPoster As Boolean) As Tvdb.Banners
        If Not ReturnPoster Then Return Nothing

        Dim mirrorsurl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & TvdbId & "/banners.xml"
        Dim XmlFile As String

        XmlFile = Utilities.DownloadTextFiles(mirrorsurl)

        Dim BannerList As New Tvdb.Banners
        BannerList.LoadXml(XmlFile)
        Return BannerList
    End Function

    Public Function getposterlist(ByVal tvdbid As String, ByRef ListOfBanners As List(Of TvBanners)) As String
        Monitor.Enter(Me)
        Try
            Dim mirrors As New List(Of String)
            Dim xmlfile As String
            Dim wrGETURL As WebRequest
            Dim mirrorsurl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/banners.xml"
            wrGETURL = WebRequest.Create(mirrorsurl)
            wrGETURL.Proxy = Utilities.MyProxy
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            xmlfile = objReader.ReadToEnd
            Dim bannerslist As New XmlDocument
            bannerslist.LoadXml(xmlfile)
            For Each thisresult As XmlNode In bannerslist("Banners")

                Select Case thisresult.Name
                    Case "Banner"
                        Dim banner As New TvBanners
                        Dim bannerselection As XmlNode = Nothing
                        For Each bannerselection In thisresult.ChildNodes
                            Select Case bannerselection.Name
                                Case "id"
                                    banner.id = bannerselection.InnerXml
                                Case "BannerPath"
                                    banner.Url = "http://www.thetvdb.com/banners/" & bannerselection.InnerXml
                                    banner.SmallUrl = "http://www.thetvdb.com/banners/_cache/" & bannerselection.InnerXml
                                Case "BannerType"
                                    banner.BannerType = bannerselection.InnerXml
                                Case "BannerType2"
                                    banner.Resolution = bannerselection.InnerXml
                                Case "Language"
                                    banner.Language = bannerselection.InnerXml
                                Case "Season"
                                    banner.Season = bannerselection.InnerXml
                                Case "Rating"
                                    banner.Rating = bannerselection.InnerText.ToRating
                                Case ""
                            End Select
                        Next
                        ListOfBanners.Add(banner)
                End Select
            Next
            Return "ok"
        Catch ex As WebException
            Return ex.ToString
        Catch EX As Exception
            Return EX.ToString
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    'Public Function getmirrors()
    '    Monitor.Enter(Me)
    '    Try
    '        Dim mirrors As New List(Of String)
    '        Dim xmlfile As String
    '        Dim mirrorsurl As String = "http://www.thetvdb.com/api/6E82FED600783400/mirrors.xml"
    '        Dim wrGETURL As WebRequest = WebRequest.Create(mirrorsurl)

    '        wrGETURL.Proxy = Utilities.MyProxy
    '        Dim objStream As IO.Stream
    '        objStream = wrGETURL.GetResponse.GetResponseStream()
    '        Dim objReader As New IO.StreamReader(objStream)
    '        xmlfile = objReader.ReadToEnd
    '        Dim mirrorslist As New XmlDocument
    '        'Try
    '        mirrorslist.LoadXml(xmlfile)
    '        For Each thisresult As XmlNode In mirrorslist("Mirrors")

    '            Select Case thisresult.Name
    '                Case "Mirror"
    '                    Dim mirrorselection As XmlNode = Nothing
    '                    For Each mirrorselection In thisresult.ChildNodes
    '                        Select Case mirrorselection.Name
    '                            Case "mirrorpath"
    '                                If mirrorselection.InnerText <> Nothing Then
    '                                    mirrors.Add(mirrorselection.InnerText)
    '                                End If
    '                        End Select
    '                    Next
    '            End Select
    '        Next
    '        Return mirrors

    '    Catch EX As Exception
    '        Return EX.ToString
    '    Finally
    '        Monitor.Exit(Me)
    '    End Try
    'End Function

    Public Function GetShow(ByVal TvdbId As String, ByVal Language As String, ByVal SeriesXmlPath As String) As Tvdb.ShowData

        Dim mirrorsurl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & TvdbId & "/" & Language & ".xml"
        Dim mirrorsurl2 As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & TvdbId & "/" & "/all/" & Language & ".xml"
        Dim success As Boolean = DownloadCache.Savexmltopath(mirrorsurl2, SeriesXmlPath, TvdbId & ".xml", True)

        Dim showlist As New Tvdb.ShowData
        If success Then     'If download series xml successful
            showlist.Load(SeriesXmlPath & TvdbId & ".xml")
        Else                'Else get just the Show's data.
            Dim xmlfile As String
            xmlfile = Utilities.DownloadTextFiles(mirrorsurl)
            If String.IsNullOrEmpty(xmlfile) Then
                showlist.FailedLoad = True
            Else
                showlist.LoadXml(xmlfile)
            End If
        End If
        If showlist.FailedLoad Then
            Dim xmlfile As String
            xmlfile = Utilities.DownloadTextFiles(mirrorsurl)
            If String.IsNullOrEmpty(xmlfile) Then
                showlist.FailedLoad = True
            Else
                showlist.FailedLoad = False
                showlist.LoadXml(xmlfile)
            End If
        End If
        If Not showlist.FailedLoad Then
            Dim strsplt As New List(Of String)
            Try
                strsplt.AddRange(showlist.Series(0).Genre.Value.Trim("|"c).Split("|"))
            Catch
            End Try
            If strsplt.Count > 0 Then
                Dim NewGenre As String = ""
                For i = 0 To strsplt.Count - 1
                    If i = Pref.TvMaxGenres Then Exit For
                    If NewGenre = "" Then
                        NewGenre = strsplt.Item(i)
                    Else
                        NewGenre &= "|" & strsplt.Item(i)
                    End If
                Next
                showlist.Series(0).Genre.Value = NewGenre
            End If
        End If
        Return showlist
    End Function

    Public Function GetSeriesXml(ByVal TvdbId As String, ByVal Lan As String, ByVal SeriesXmlPath As String) As Boolean
        Dim url As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & TvdbId & "/" & "/all/" & Lan & ".xml"
        Return DownloadCache.Savexmltopath(url, SeriesXmlPath, TvdbId & ".xml", True)
    End Function

    Public Function GetActors(ByVal TvdbId As String, Optional ByVal maxactors As Integer = 9999) As List(Of str_MovieActors) 'Tvdb.Actors
        If maxactors = 9999 Then
            maxactors = Pref.maxactors
        End If
        Dim results As New List(Of str_MovieActors)
        Dim mirrorsurl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & TvdbId & "/actors.xml"

        Dim xmlfile As String
        xmlfile = Utilities.DownloadTextFiles(mirrorsurl)
        Utilities.CheckForXMLIllegalChars(xmlfile)
        Dim showlist As New Tvdb.Actors
        'Try
        showlist.LoadXml(xmlfile)
        For Each Mc In showlist.Items
            Dim actor As str_MovieActors = New str_MovieActors
            actor.actorid = Mc.Id.Value
            actor.actorname = Utilities.cleanSpecChars(Mc.Name.Value).Trim
            Dim newstring As String
            newstring = Mc.Role.Value
            newstring = newstring.TrimEnd("|")
            newstring = newstring.TrimStart("|")
            newstring = newstring.Replace("|", ", ")
            actor.actorrole = newstring.TrimStart.TrimEnd
            If Mc.Image.Value <> "" Then
                actor.actorthumb = "http://thetvdb.com/banners/_cache/" & Mc.Image.Value
            Else
                actor.actorthumb = ""
            End If
            actor.order = Mc.SortOrder.Value
            results.Add(actor)
        Next
        Return results
    End Function

    Public Function getepisode(ByVal tvdbid As String, ByVal sortorder As String, ByVal seasonno As String, ByVal episodeno As String, ByVal language As String, Optional ByVal forcedownload As Boolean = False)
        Monitor.Enter(Me)
        Dim episodestring As String = ""
        Dim episodeurl As String = ""
        Try
            'http://thetvdb.com/api/6E82FED600783400/series/70726/default/1/1/en.xml
            Dim SeriesXmlPath As String = Utilities.SeriesXmlPath

            Dim xmlfile As String = Nothing
            If language.ToLower.IndexOf(".xml") = -1 Then
                language = language & ".xml"
            End If
            episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seasonno & "/" & episodeno & "/" & language

            'First try seriesxml data
            'check if present, download if not
            Dim gotseriesxml As Boolean = False
            Dim url As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/all/" & language
            Dim xmlfile2 As String = SeriesXmlPath & tvdbid & ".xml"
            Dim SeriesInfo As New Tvdb.ShowData
            If Not File.Exists(xmlfile2) Then
                gotseriesxml = DownloadCache.Savexmltopath(url, SeriesXmlPath, tvdbid & ".xml", True)
            Else
                'Check series xml isn't older than Five days.  If so, re-download it.
                Dim dtCreationDate As DateTime = File.GetLastWriteTime(xmlfile2)
                Dim datenow As DateTime = Date.Now()
                Dim dif As Long = DateDiff(DateInterval.Day, dtCreationDate, datenow)
                If dif > 5 Then
                    gotseriesxml = DownloadCache.Savexmltopath(url, SeriesXmlPath, tvdbid & ".xml", True)
                Else
                    gotseriesxml = True
                End If
            End If

            If Not gotseriesxml Then
                episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seasonno & "/" & episodeno & "/" & language
                xmlfile = Utilities.DownloadTextFiles(episodeurl)
            Else
                SeriesInfo.Load(xmlfile2)
                Dim gotEpxml As Boolean = False
                'check episode is present in seriesxml file, else, re-download it (update to latest)
                If sortorder = "default" Then
                    For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
                        If NewEpisode.EpisodeNumber.Value = episodeno AndAlso NewEpisode.SeasonNumber.Value = seasonno Then
                            xmlfile = NewEpisode.Node.ToString
                            xmlfile = "<Data>" & xmlfile & "</Data>"
                            gotEpxml = True
                            Exit For
                        End If
                    Next
                ElseIf sortorder = "dvd" Then
                    For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
                        If NewEpisode.DvdEpisodeNumber.Value = (episodeno & ".0") AndAlso NewEpisode.DvdSeason.Value = seasonno Then
                            xmlfile = NewEpisode.Node.ToString
                            xmlfile = "<Data>" & xmlfile & "</Data>"
                            gotEpxml = True
                            Exit For
                        End If
                    Next
                End If
                ' Finally, if not in seriesxml file, go old-school
                If Not gotEpxml Then
                    episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seasonno & "/" & episodeno & "/" & language
                    xmlfile = Utilities.DownloadTextFiles(episodeurl)
                End If
            End If

            episodestring = "<episodedetails>"
            episodestring = episodestring & "<url>" & episodeurl & "</url>"
            Dim mirrorslist As New XmlDocument

            mirrorslist.LoadXml(xmlfile)
            For Each thisresult As XmlNode In mirrorslist("Data")
                Select Case thisresult.Name
                    Case "Episode"
                        Dim mirrorselection As XmlNode = Nothing
                        For Each mirrorselection In thisresult.ChildNodes
                            Select Case mirrorselection.Name
                                Case "EpisodeName"
                                    episodestring = episodestring & "<title>" & mirrorselection.InnerXml & "</title>"
                                Case "FirstAired"
                                    episodestring = episodestring & "<premiered>" & mirrorselection.InnerXml & "</premiered>"
                                Case "GuestStars"
                                    Dim tempstring As String = mirrorselection.InnerXml
                                    Try
                                        tempstring = tempstring.TrimStart("|")
                                        tempstring = tempstring.TrimEnd("|")
                                        Dim tvtempstring2 As String
                                        Dim tvtempint As Integer
                                        Dim a() As String
                                        Dim j As Integer
                                        tvtempstring2 = ""
                                        a = tempstring.Split("|")
                                        tvtempint = a.GetUpperBound(0)
                                        tvtempstring2 = a(0)
                                        If tvtempint >= 0 Then
                                            For j = 0 To tvtempint
                                                Try
                                                    episodestring = episodestring & "<actor>" & "<name>" & a(j) & "</name></actor>"
                                                Catch
                                                End Try
                                            Next
                                        End If
                                    Catch
                                    End Try
                                Case "Director"
                                    Dim tempstring As String = mirrorselection.InnerXml
                                    tempstring = tempstring.TrimStart("|")
                                    tempstring = tempstring.TrimEnd("|")
                                    episodestring = episodestring & "<director>" & tempstring & "</director>"
                                Case "Writer"
                                    Dim tempstring As String = mirrorselection.InnerXml
                                    tempstring = tempstring.TrimStart("|")
                                    tempstring = tempstring.TrimEnd("|")
                                    episodestring = episodestring & "<credits>" & tempstring & "</credits>"
                                Case "Overview"
                                    episodestring = episodestring & "<plot>" & mirrorselection.InnerXml & "</plot>"
                                Case "Rating"
                                    episodestring = episodestring & "<rating>" & mirrorselection.InnerXml & "</rating>"
                                Case "RatingCount"
                                    episodestring = episodestring & "<ratingcount>" & mirrorselection.InnerXml & "</ratingcount>"
                                Case "id"
                                    episodestring = episodestring & "<uniqueid>" & mirrorselection.InnerXml & "</uniqueid>"
                                Case "IMDB_ID"
                                    episodestring = episodestring & "<imdbid>" & mirrorselection.InnerXml & "</imdbid>"
                                Case "filename"
                                    episodestring = episodestring & "<thumb>http://www.thetvdb.com/banners/" & mirrorselection.InnerXml & "</thumb>"
                                Case "airsbefore_episode"
                                    episodestring = episodestring & "<displayepisode>" & mirrorselection.InnerXml & "</displayepisode>"
                                Case "airsbefore_season"
                                    episodestring = episodestring & "<displayseason>" & mirrorselection.InnerXml & "</displayseason>"
                            End Select
                        Next
                End Select
            Next
            episodestring = episodestring & "</episodedetails>"
            Return episodestring
        Catch ex As Exception
            Return "ERROR - <url>" & episodeurl & "</url>"
        Finally
            Monitor.Exit(Me)
        End Try

    End Function

    Public Function getepfromxml(ByVal tvdbid As String, ByVal sortorder As String, ByVal seriesno As String, ByVal episodeno As String, ByVal language As String, Optional ByVal forcedownload As Boolean = False) As TvEpisode
        Monitor.Enter(Me)
        Dim episodeurl As String = ""
        Dim thisepisode As New TvEpisode
        Try
            'http://thetvdb.com/api/6E82FED600783400/series/70726/default/1/1/en.xml

            Dim xmlfile As String
            If language.ToLower.IndexOf(".xml") = -1 Then language = language & ".xml"
            episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seriesno & "/" & episodeno & "/" & language

            xmlfile = Utilities.DownloadTextFiles(episodeurl, forcedownload) 'this function has gzip detection in it 
            Dim xmlOK As Boolean = Utilities.CheckForXMLIllegalChars(xmlfile)
            Dim mirrorslist As New XmlDocument

            mirrorslist.LoadXml(xmlfile)
            For Each thisresult As XmlNode In mirrorslist("Data")
                Select Case thisresult.Name
                    Case "Episode"
                        Dim gueststars As String = ""
                        For Each thisresult2 As XmlNode In thisresult.ChildNodes
                            Select Case thisresult2.Name
                                Case "id"
                                    thisepisode.Id.Value = thisresult2.InnerXml
                                Case "EpisodeNumber"        : thisepisode.Episode.Value             = thisresult2.InnerXml
                                Case "EpisodeName"          : thisepisode.Title.Value               = thisresult2.InnerXml
                                Case "FirstAired"           : thisepisode.Aired.Value               = thisresult2.InnerXml
                                Case "GuestStars"           : gueststars                            = thisresult2.InnerXml.Trim("|")
                                Case "Director"             : thisepisode.Director.Value            = thisresult2.InnerXml
                                Case "Writer"               : thisepisode.Credits.Value             = thisresult2.InnerXml
                                Case "Overview"             : thisepisode.Plot.Value                = thisresult2.InnerXml
                                Case "Rating"               : thisepisode.Rating.Value              = thisresult2.InnerXml
                                'Case "DVD_Season"           : thisepisode.DvdSeason.Value           = thisresult2.InnerXml
                                Case "DVD_episodenumber"    : thisepisode.DvdEpNumber.Value         = thisresult2.InnerXml
                                Case "seriesid"             : thisepisode.Showtvdbid.Value          = thisresult2.InnerXml
                                Case "IMDB_ID"              : thisepisode.ImdbId.Value              = thisresult2.InnerXml
                                Case "Rating"               : thisepisode.Rating.Value              = thisresult2.InnerXml
                                Case "SeasonNumber"         : thisepisode.Season.Value              = thisresult2.InnerXml
                                'Case "Language"             : thisepisode.Language.Value            = thisresult2.InnerXml
                                Case "RatingCount"          : thisepisode.Votes.Value               = thisresult2.InnerXml
                                Case "filename"             : thisepisode.Thumbnail.FileName        = "http://www.thetvdb.com/banners/" & thisresult2.InnerXml
                            End Select
                        Next
                End Select
            Next
        Catch ex As Exception
            thisepisode.FailedLoad = True
        Finally
            Monitor.Exit(Me)
        End Try
        Return thisepisode
    End Function

    Public Function getepisodefromxml(ByVal tvdbid As String, ByVal sortorder As String, ByVal seriesno As String, ByVal episodeno As String, ByVal language As String, Optional ByVal forcedownload As Boolean = False) As Tvdb.Episode
        Monitor.Enter(Me)
        Dim episodeurl As String = ""
        Dim thisepisode As New Tvdb.Episode
        Try
            'http://thetvdb.com/api/6E82FED600783400/series/70726/default/1/1/en.xml

            Dim xmlfile As String
            If language.ToLower.IndexOf(".xml") = -1 Then language = language & ".xml"
            episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seriesno & "/" & episodeno & "/" & language

            xmlfile = Utilities.DownloadTextFiles(episodeurl, forcedownload) 'this function has gzip detection in it 
            Dim xmlOK As Boolean = Utilities.CheckForXMLIllegalChars(xmlfile)
            Dim mirrorslist As New XmlDocument

            mirrorslist.LoadXml(xmlfile)
            For Each thisresult As XmlNode In mirrorslist("Data")
                Select Case thisresult.Name
                    Case "Episode"
                        For Each thisresult2 As XmlNode In thisresult.ChildNodes
                            Select Case thisresult2.Name
                                Case "id"
                                    thisepisode.Id.Value = thisresult2.InnerXml
                                Case "EpisodeNumber"        : thisepisode.EpisodeNumber.Value       = thisresult2.InnerXml
                                Case "EpisodeName"          : thisepisode.EpisodeName.Value         = thisresult2.InnerXml
                                Case "FirstAired"           : thisepisode.FirstAired.Value          = thisresult2.InnerXml
                                Case "GuestStars"           : thisepisode.GuestStars.Value          = thisresult2.InnerXml.Trim("|")
                                Case "Director"             : thisepisode.Director.Value            = thisresult2.InnerXml
                                Case "Writer"               : thisepisode.Writer.Value              = thisresult2.InnerXml
                                Case "Overview"             : thisepisode.Overview.Value            = thisresult2.InnerXml
                                Case "Rating"               : thisepisode.Rating.Value              = thisresult2.InnerXml
                                Case "DVD_Season"           : thisepisode.DvdSeason.Value           = thisresult2.InnerXml
                                Case "DVD_episodenumber"    : thisepisode.DvdEpisodeNumber.Value    = thisresult2.InnerXml
                                Case "seriesid"             : thisepisode.SeriesId.Value            = thisresult2.InnerXml
                                Case "IMDB_ID"              : thisepisode.ImdbId.Value              = thisresult2.InnerXml
                                Case "Rating"               : thisepisode.Rating.Value              = thisresult2.InnerXml
                                Case "SeasonNumber"         : thisepisode.SeasonNumber.Value        = thisresult2.InnerXml
                                Case "Language"             : thisepisode.Language.Value            = thisresult2.InnerXml
                                Case "RatingCount"          : thisepisode.Votes.Value               = thisresult2.InnerXml
                                Case "filename"             : thisepisode.ThumbNail.Value           = "http://www.thetvdb.com/banners/" & thisresult2.InnerXml
                            End Select
                        Next
                End Select
            Next
        Catch ex As Exception
            thisepisode.FailedLoad = True
        Finally
            Monitor.Exit(Me)
        End Try
        Return thisepisode
    End Function
#End Region

End Class
