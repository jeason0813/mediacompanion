Imports ProtoXML
Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Xml
Imports Media_Companion


Public Class TvShow
    Inherits ProtoFile

    Dim _PossibleShowList               As List(Of Tvdb.Series)
    Private Property _State             As New ProtoProperty(Me, "state", CacheMode:=CacheMode.Both)
    Private _Visible                    As Boolean

    Public Property TvdbId              As New ProtoProperty(Me, "id"                   , CacheMode:=CacheMode.Both)
    Public Property Title               As New ProtoProperty(Me, "title"                , CacheMode:=CacheMode.Both)
    Public Property SortTitle           As New ProtoProperty(Me, "sorttitle")                           'Not sure if needs be in cache , CacheMode:= CacheMode.Both)
    Public Property Rating              As New ProtoProperty(Me, "rating"   , "")
    Public Property Year                As New ProtoProperty(Me, "year")
    Public Property Top250              As New ProtoProperty(Me, "top250")                              'from XBMC created tvshow.nfo
    Public Property Season              As New ProtoProperty(Me, "season")                              'from XBMC created tvshow.nfo
    Public Property EpisodeCount        As New ProtoProperty(Me, "episode")                             'from XBMC created tvshow.nfo
    Public Property DisplaySeason       As New ProtoProperty(Me, "displayseason")                       'from XBMC created tvshow.nfo
    Public Property DisplayEpisode      As New ProtoProperty(Me, "displayepisode")                      'from XBMC created tvshow.nfo
    Public Property Votes               As New ProtoProperty(Me, "votes"    , "")                       'from XBMC created tvshow.nfo
    Public Property Plot                As New ProtoProperty(Me, "plot"                 , CacheMode:= CacheMode.Both)
    'Public Property Outline             As New ProtoProperty(Me, "outline")                             'XBMC uses same nfo structure for movie as for tvshow nfo file
    'Public Property TagLine             As New ProtoProperty(Me, "tagline")                             'XBMC uses same nfo structure for movie as for tvshow nfo file
    Public Property Runtime             As New ProtoProperty(Me, "runtime")
    Public Property Mpaa                As New ProtoProperty(Me, "mpaa")
    Public Property Playcount           As New ProtoProperty(Me, "playcount", "1"       , CacheMode:= CacheMode.Both)
    Public Property Genre               As New ProtoProperty(Me, "genre"    , "")
    Public Property Credits             As New ProtoProperty(Me, "credits")
    'Public Property [Set]               As New ProtoProperty(Me, "set")                                 'XBMC uses same nfo structure for movie as for tvshow nfo file
    Public Property Director            As New ProtoProperty(Me, "director")
    Public Property Premiered           As New ProtoProperty(Me, "premiered")
    Public Property Status              As New ProtoProperty(Me, "status"               , CacheMode:=CacheMode.Both)
    Public Property Aired               As New ProtoProperty(Me, "aired") '
    Public Property Studio              As New ProtoProperty(Me, "studio")
    'Public Property Trailer             As New ProtoProperty(Me, "trailer")                             'XBMC uses same nfo structure for movie as for tvshow nfo file
    'Public Property Artist              As New ProtoProperty(Me, "Artist")                              'Possible hold over from Music?
    Public Property EpisodeGuideUrl     As New ProtoProperty(Me, "episodeguide")
    Public Property Url                 As New ProtoProperty(EpisodeGuideUrl, "url")
    Public Property ListActors          As New ActorList(Me, "actor")

    Public Property ImageFanart         As New ProtoImage(Me, "fanart"      , Utilities.DefaultFanartPath) With {.FileName = "fanart.jpg"}
    Public Property ImagePoster         As New ProtoImage(Me, "poster"      , Utilities.DefaultPosterPath) With {.FileName = "poster.jpg"}
    Public Property ImageBanner         As New ProtoImage(Me, "banner"      , Utilities.DefaultBannerPath) With {.FileName = "banner.jpg"}
    Public Property ImageAllSeasons     As New ProtoImage(Me, "allseasons"  , Utilities.DefaultPosterPath) With {.FileName = "season" & If(Pref.FrodoEnabled, "-all-poster.jpg", "-all.tbn")}
    Public Property ImageClearArt       As New ProtoImage(Me, "clearart"    , Utilities.DefaultBannerPath) With {.FileName = "clearart.png"}
    Public Property ImageLogo           As New ProtoImage(Me, "logo"        , Utilities.DefaultBannerPath) With {.FileName = "logo.png"}

    'Media Companion Specific
    Public Property ImdbId              As New ProtoProperty(Me, "imdbid"               , CacheMode:=CacheMode.Both)        'MC uses to locate artwork on IMDB & PosterDB sites
    Public Property TmdbId              As New ProtoProperty(Me, "tmdbid"               , CacheMode:=CacheMode.Both)        'MC to use for TheMovieDb Series & episode scraping.
    Public Property SortOrder           As New ProtoProperty(Me, "sortorder"            , CacheMode:=CacheMode.Both)
    Public Property Language            As New ProtoProperty(Me, "language"             , CacheMode:=CacheMode.Both)
    Public Property TvShowActorSource   As New ProtoProperty(Me, "tvshowactorsource")
    Public Property EpisodeActorSource  As New ProtoProperty(Me, "episodeactorsource"   , CacheMode:=CacheMode.Both)
    Public Property Hidden              As New ProtoProperty(Me, "hidden"               , CacheMode:=CacheMode.OnlyCache)   'Field for filtering out series from Treeview.
    Public Property UserRating          As New ProtoProperty(Me, "userrating"           , "0")
    
    Public Shadows Property State As Media_Companion.ShowState
        Get
            Select Case _State.Value
                Case Media_Companion.ShowState.Open
                    If ShowNode.ImageKey = "missing.png" Then
                        ShowNode.ImageKey = "missing.png"
                        ShowNode.SelectedImageKey = "missing.png"
                    Else
                        ShowNode.ImageKey = "blank"
                        ShowNode.SelectedImageKey = "blank"
                    End If
                Case Media_Companion.ShowState.Locked
                    ShowNode.ImageKey = "padlock"
                    ShowNode.SelectedImageKey = "padlock"
                Case Media_Companion.ShowState.Unverified
                    ShowNode.ImageKey = "qmark"
                    ShowNode.SelectedImageKey = "qmark"
                Case Media_Companion.ShowState.Error
                    ShowNode.ImageKey = "error"
                    ShowNode.SelectedImageKey = "error"
            End Select
            Return CType(_State.Value, Media_Companion.ShowState)
        End Get
        Set(ByVal value As Media_Companion.ShowState)
            _State.Value = value
            Select Case _State.Value
                Case Media_Companion.ShowState.Open
                    ShowNode.ImageKey = "blank"
                    ShowNode.SelectedImageKey = "blank"
                Case Media_Companion.ShowState.Locked
                    ShowNode.ImageKey = "padlock"
                    ShowNode.SelectedImageKey = "padlock"
                Case Media_Companion.ShowState.Unverified
                    ShowNode.ImageKey = "qmark"
                    ShowNode.SelectedImageKey = "qmark"
                Case Media_Companion.ShowState.Error
                    ShowNode.ImageKey = "error"
                    ShowNode.SelectedImageKey = "error"
            End Select
        End Set
    End Property

    'Non-xml properties
    Public Property Seasons     As New Dictionary(Of String, TvSeason)
    Public Property Episodes    As New List(Of TvEpisode)
    Public Property posters     As New List(Of String)
    Public Property fanart      As New List(Of String)

    Public Property PossibleShowList As List(Of Tvdb.Series)
        Get
            If _PossibleShowList Is Nothing Then Me.GetPossibleShows()

            Return _PossibleShowList
        End Get
        Set(ByVal value As List(Of Tvdb.Series))
            _PossibleShowList = value
        End Set
    End Property

    Public Property Visible As Boolean
        Get
            If _Visible Then
                ShowNode.ForeColor = Drawing.Color.Black
            Else
                ShowNode.ForeColor = Drawing.Color.LightGray
            End If
            Return _Visible
        End Get
        Set(ByVal value As Boolean)
            _Visible = value
            If _Visible Then
                ShowNode.ForeColor = Drawing.Color.Black
            Else
                ShowNode.ForeColor = Drawing.Color.LightGray
            End If
        End Set
    End Property

    Public ReadOnly Property MissingEpisodes As List(Of TvEpisode)
        Get
            Return (From e In Me.Episodes Where e.IsMissing).ToList()
        End Get
    End Property

    Public ReadOnly Property TitleAndYear As String
        Get
            Return Title.Value & " " & Year.Value
        End Get
    End Property

    Public ReadOnly Property VisibleEpisodeCount As Integer
        Get
            Dim Count As Integer = 0
            For Each Ep As TvEpisode In Episodes
                If Ep.Visible Then
                    Count += 1
                End If
            Next
            Return Count
        End Get
    End Property

    Public ReadOnly Property VisibleSeasonCount As Integer
        Get
            Dim Count As Integer = 0
            For Each Ep As TvSeason In Seasons.Values
                If Ep.Visible Then
                    Count += 1
                End If
            Next
            Return Count
        End Get
    End Property


    Sub New()
        MyBase.New("tvshow")
    End Sub

    Protected Overrides Sub LoadDoc()
        MyBase.LoadDoc()
    End Sub
    
    Public Sub clearActors()
        MyBase.DeleteElement("actor")
        Me.ListActors.Clear()
    End Sub

    Public Sub GetPossibleShows()
        'Dim possibleshows As New List(Of possibleshowlist)
        Dim xmlfile As String
        If String.IsNullOrEmpty(Me.FolderPath) Then Exit Sub

        'TODO: Properly encode URL
        Dim SearchTitle As String
        Dim mirrorsurl As String
        If Title.Value Is Nothing Then
            SearchTitle = Utilities.GetLastFolder(Me.FolderPath)
        Else
            SearchTitle = Title.Value
        End If

        'TODO: remove articles (And, &, The, ext)... Tvdb search is extremly litteral with absolutly no text adjustment
        SearchTitle = SearchTitle.ToLower
        SearchTitle = SearchTitle.Replace(" & ", " ") 'Extra spacing for whole word match only
        SearchTitle = SearchTitle.Replace(" and ", " ")
        'SearchTitle = SearchTitle.Replace(".", " ")  'Replace periods in foldernames with spaces (linux OS support)
        SearchTitle = SearchTitle.Replace("&", "%26")  'Finally if title has & with no spaces between words, ie: Sex&Drugs&Rock&Roll

        mirrorsurl = "http://www.thetvdb.com/api/GetSeries.php?seriesname=" & SearchTitle & "&language=all"
        xmlfile = Utilities.DownloadTextFiles(mirrorsurl, True)

        If String.IsNullOrEmpty(xmlfile) Then Exit Sub

        'clean html tags from xml data
        xmlfile = xmlfile.Replace("<br>", "")

        Dim ReturnData As New Tvdb.ShowData

        Try
            ReturnData.LoadXml(xmlfile)
        Catch ex As Exception
            MsgBox("Error is returned xml file from TVDb" & vbCrLf & xmlfile)
        End Try

        If _PossibleShowList Is Nothing Then _PossibleShowList = New List(Of Tvdb.Series)

        Dim Found As Boolean
        For Each Item As Tvdb.Series In ReturnData.Series.List
            For Each Existing As Tvdb.Series In _PossibleShowList
                If Existing.Id = Item.Id Then Found = True
            Next
            If Not Found Then
                _PossibleShowList.Add(Item)
            End If
        Next
        If _PossibleShowList.Count = 0 Then
            _PossibleShowList = Nothing
            Me.State = ShowState.Error
            Me.FailedLoad = True
        End If
    End Sub

    Public Sub AbsorbTvdbSeries(ByVal Series As Tvdb.Series)
        Me.TvdbId.Value     = Series.Id.Value
        Me.Mpaa.Value       = Series.ContentRating.Value
        Me.Genre.Value      = Series.Genre.Value.Trim("|"c).Replace("|", " / ")
        Me.ImdbId.Value     = Series.ImdbId.Value
        Me.TmdbId.Value     = Series.TmdbId.Value
        Dim tmp As String   = Series.Overview.Value.ToString
        tmp                 = string.Join("  ", tmp.Split(Environment.NewLine.ToCharArray()))
        Me.Plot.Value       = tmp
        Me.Title.Value      = If(Not String.IsNullOrEmpty(Series.SeriesName.Value), Series.SeriesName.Value, Me.Title.Value) 'not set up in ScrapeShowTask.vb
        Me.Runtime.Value    = Series.RunTime.Value
        Me.Rating.Value     = Series.Rating.Value
        Me.Votes.Value      = Series.RatingCount.value
        Me.Premiered.Value  = Series.FirstAired.Value
        Me.Year.Value       = If(Not String.IsNullOrEmpty(Series.FirstAired.Value), Series.FirstAired.Value.Substring(0,4), "")
        Me.Studio.Value     = Series.Network.Value
        Me.Status.Value     = Series.Status.Value 
        Me.EpisodeGuideUrl.Value = ""
        Me.Url.Value        = URLs.EpisodeGuide(Series.Id.Value, Series.Language.Value)
        Me.Url.Node.SetAttributeValue("cache", Series.Id.Value)
        Me.Hidden.Value     = False.ToString
        Me.UserRating.Value = "0"

    End Sub

    Public Sub AbsorbTvdbSeries(ByVal Series As TheTvDB.TvdbSeries)
        Me.TvdbId.Value     = Series.Identity
        Me.Mpaa.Value       = Series.ContentRating
        Me.Genre.Value      = Series.GenresDisplayString.Replace(", ", " / ")
        Me.ImdbId.Value     = Series.ImdbID
        Me.Plot.Value       = Series.Overview
        Me.Title.Value      = If(Not String.IsNullOrEmpty(Series.SeriesName), Series.SeriesName, Me.Title.Value) 'not set up in ScrapeShowTask.vb
        Me.Runtime.Value    = Series.Runtime
        Me.Rating.Value     = Series.Rating
        Me.Votes.Value      = Series.Votes
        Me.Premiered.Value  = Series.FirstAired
        Me.Year.Value       = If(Not String.IsNullOrEmpty(Series.FirstAired), Series.FirstAired.Substring(0,4), "")
        Me.Studio.Value     = Series.Network
        Me.Status.Value     = Series.Status
        Me.EpisodeGuideUrl.Value = ""
        'Me.Url.Value        = URLs.EpisodeGuide(Series.Id.Value, Series.Language.Value)
        'Me.Url.Node.SetAttributeValue("cache", Series.Id.Value)
        Me.Hidden.Value     = False.ToString
        Me.UserRating.Value = "0"

    End Sub
    
    Public Sub SearchForEpisodesInFolder()
        Dim newlist As New List(Of String)
        newlist.Clear()
        newlist = Utilities.EnumerateFolders(Me.FolderPath, 6) 'TODO: Restore loging functions

        newlist.Insert(0, Me.FolderPath)

        For Each folder In newlist
            Dim dir_info As New directoryInfo(folder)

            Dim fs_infos() As FileInfo = dir_info.GetFiles("*.NFO", IO.SearchOption.TopDirectoryOnly)
            For Each fs_info As FileInfo In fs_infos
                'Application.DoEvents()
                If Path.GetFileName(fs_info.FullName.ToLower) <> "tvshow.nfo" And fs_info.ToString.Substring(0, 2) <> "._" Then
                    Dim NewEpisode As New TvEpisode
                    NewEpisode.NfoFilePath = fs_info.FullName
                    NewEpisode.Load()
                    DirectCast(NewEpisode.CacheDoc.FirstNode, System.Xml.Linq.XElement).FirstAttribute.Value = NewEpisode.NfoFilePath
                    Me.AddEpisode(NewEpisode)
                End If

            Next fs_info
        Next
    End Sub
    
    Public Sub AddEpisode(ByRef Episode As TvEpisode)
        If Not Cache.TvCache.Contains(Episode) Then
            Cache.TvCache.Add(Episode)
        End If

        If Not Me.Episodes.Contains(Episode) Then
            Me.Episodes.Add(Episode)
        Else
            Exit Sub
        End If

        Dim CurrentSeason As TvSeason
        If Episode.Season.Value IsNot Nothing Then
            If Not Me.Seasons.ContainsKey(Episode.Season.Value) Then
                CurrentSeason = New TvSeason
                Me.Seasons.Add(Episode.Season.Value, CurrentSeason)

                If Utilities.IsNumeric(Episode.Season.Value) Then
                    CurrentSeason.SeasonNumber = Episode.Season.Value
                    If CurrentSeason.SeasonNumber=0 THen
                        CurrentSeason.SeasonLabel = "Specials"
                    Else
                        CurrentSeason.SeasonLabel = "Season " & Utilities.PadNumber(Episode.Season.Value, 2)
                    End If
                    CurrentSeason.Poster.FolderPath = Me.FolderPath
                    CurrentSeason.Banner.FolderPath = Me.FolderPath
                    If Episode.Season.Value <> 0 Then
                        CurrentSeason.Poster.FileName = "season" & Utilities.PadNumber(Episode.Season.Value, 2) & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")
                        CurrentSeason.Banner.FileName = "season" & Utilities.PadNumber(Episode.Season.Value, 2) & "-banner.jpg"
                    Else
                        CurrentSeason.Poster.FileName = "season-specials" & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")
                        CurrentSeason.Banner.FileName = "season-specials-banner.jpg"
                    End If
                    CurrentSeason.FolderPath = Episode.FolderPath

                Else
                    CurrentSeason.SeasonNumber = -1
                    CurrentSeason.SeasonLabel = Episode.Season.Value
                    CurrentSeason.Poster.FolderPath = Me.FolderPath
                    CurrentSeason.Banner.FolderPath = Me.FolderPath
                    CurrentSeason.Poster.FileName = "season-all" & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")
                    CurrentSeason.Banner.FileName = "season-all-banner.jpg"
                    CurrentSeason.FolderPath = Episode.FolderPath
                End If
            Else
                CurrentSeason = Me.Seasons(Episode.Season.Value)
            End If

            Episode.SeasonObj = CurrentSeason

            CurrentSeason.ShowObj = Me
            CurrentSeason.ShowId.Value = Me.TvdbId.Value
            CurrentSeason.Episodes.Add(Episode)

            Episode.ShowObj = Me
            Episode.ShowId.Value = Me.TvdbId.Value

            Dim EpisodeNum As Integer                                                                         'code added as suggestion
            If Not Integer.TryParse(Episode.Episode.Value, EpisodeNum) Then                                   ' by Archaetect 
                Dim Match As System.Text.RegularExpressions.Match                                             ' to halt error caused if
                Dim Regex As New System.Text.RegularExpressions.Regex("[sS][0-9][0-9][eE]([0-9][0-9])[^0-9]") ' missing season or episode
                Match = Regex.Match(Episode.NfoFilePath)                                                      ' number.
                If Match.Success Then                                                                         ' This code should allow next 
                    Integer.TryParse(Match.Value, EpisodeNum)                                                 ' episode to continue scraping
                End If
            End If

            If EpisodeNum > 0 Then
                If IsNothing(CurrentSeason.MaxEpisodeCount) Then
                    CurrentSeason.MaxEpisodeCount = EpisodeNum
                ElseIf EpisodeNum > CurrentSeason.MaxEpisodeCount Then
                    CurrentSeason.MaxEpisodeCount = EpisodeNum
                End If
            Else
                CurrentSeason.MaxEpisodeCount = CurrentSeason.Episodes.Count
            End If

            'If IsNothing(CurrentSeason.MaxEpisodeCount) Then
            'CurrentSeason.MaxEpisodeCount = Integer.Parse(Episode.Episode.Value)
            'ElseIf Integer.Parse(Episode.Episode.Value) > CurrentSeason.MaxEpisodeCount Then
            'CurrentSeason.MaxEpisodeCount = Integer.Parse(Episode.Episode.Value)
            'End If
        End If
    End Sub
    
    Public Sub SearchForNewEpisodes()
        'Enumerate Files in this shows folder
        '   Does it have an nfo file?
        '       Yes? Is it already added to the tv show?
        '           Load if nececery and skip without comment
        '       No? Create an episode object for the file, add it to the show
        'Enumerate all shows and make sure they are nfoed and if not scrape
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Title.Value
    End Function

    Public Function GetEpisode(ByVal Season As Long, ByVal Episode As Long) As TvEpisode
        For Each Item As TvSeason In Me.Seasons.Values.ToList
            Dim list = Item.Episodes.ToList
            If Item.SeasonNumber = Season Then
                For Each Ep As TvEpisode In list
                    If Ep.Episode.Value = Episode Then
                        Return Ep
                    End If
                Next
            End If
        Next

        Return Nothing
    End Function

    Public Function Cachedata() As TvShow
        Dim CacheSeries As New TvShow
        CacheSeries.NfoFilePath                 = Me.NfoFilePath
        CacheSeries.State                       = Me.State
        CacheSeries.Title.Value                 = Me.Title.Value
        CacheSeries.TvdbId.Value                = Me.TvdbId.Value
        CacheSeries.Status.Value                = Me.Status.Value
        CacheSeries.Plot.Value                  = Me.Plot.Value
        CacheSeries.SortOrder.Value             = Me.SortOrder.Value
        CacheSeries.Language.Value              = Me.Language.Value
        CacheSeries.EpisodeActorSource.Value    = Me.EpisodeActorSource.Value
        CacheSeries.ImdbId.Value                = Me.ImdbId.Value
        CacheSeries.Playcount.Value             = Me.Playcount.Value
        CacheSeries.Hidden.Value                = Me.Hidden.Value
        Return cacheseries
    End Function

End Class

Public Enum ShowState
    Open
    Locked
    Unverified
    [Error]
End Enum