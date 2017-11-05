Imports ProtoXML
Imports Alphaleonis.Win32.Filesystem
Imports Media_Companion
'Imports MediaInfoNET


Public Class TvEpisode
    Inherits ProtoFile
    
    Private _Visible        As Boolean
    Private _VideoFilePath  As String
    Private _PureName       As String

    Protected Property PureName As String
        Get
            Return _PureName
        End Get
        Set(ByVal value As String)
            _PureName = value
            MyBase.NfoFilePath = value & ".nfo"
            If String.IsNullOrEmpty(Me.MediaExtension) Then
                ''  disable videofilepath to speed up cache as increases loading time.
                If Not IsMissing Then
                    If String.IsNullOrEmpty(EpExtn.Value) Then
                        For Each Item As String In Utilities.VideoExtensions
                            If File.Exists(_PureName & Item) Then
                                _VideoFilePath = _PureName & Item
                                EpExtn.value = Item
                                Me.MediaExtension = Item
                                Exit For
                            End If
                        Next
                    Else
                        _VideoFilePath = _PureName & EpExtn.Value 
                    End If
                End If
            Else
                'If File.Exists(_PureName & Me.MediaExtension) Then
                _VideoFilePath = _PureName & Me.MediaExtension
                'End If
            End If

            Me.Thumbnail.Path = _PureName & If(Pref.FrodoEnabled, "-thumb.jpg", ".tbn")

            ' Me.EditAttribute("PureName", Me.PureName)
            ' Me.EditAttribute("MediaExtension", Me.MediaExtension)
        End Set
    End Property

    Public Shadows Property NfoFilePath As String
        Get
            Return MyBase.NfoFilePath
        End Get
        Set(ByVal value As String)
            Me.PureName = value.Replace(Path.GetExtension(value), "")
            MyBase.NfoFilePath = value
        End Set
    End Property
    
    Public Property VideoFilePath As String
        Get
            Return _VideoFilePath
        End Get
        Set(ByVal value As String)
            Me.MediaExtension = Path.GetExtension(value)
            Me.PureName = value.Replace(Me.MediaExtension, "")
            _VideoFilePath = value
        End Set
    End Property

    Public Property MediaExtension  As String
    Public Property Id              As New ProtoProperty(Me, "id"               , CacheMode:=CacheMode.Both)
    Public Property Title           As New ProtoProperty(Me, "title"            , CacheMode:=CacheMode.Both)
    Public Property Rating          As New ProtoProperty(Me, "rating")
    Public Property Year            As New ProtoProperty(Me, "year")
    Public Property Top250          As New ProtoProperty(Me, "top250")
    Public Property Season          As New ProtoProperty(Me, "season"           , CacheMode:=CacheMode.Both)
    Public Property Episode         As New ProtoProperty(Me, "episode"          , CacheMode:=CacheMode.Both)
    Public Property DisplaySeason   As New ProtoProperty(Me, "displayseason")
    Public Property DisplayEpisode  As New ProtoProperty(Me, "displayepisode")
    Public Property Votes           As New ProtoProperty(Me, "votes")
    Public Property Plot            As New ProtoProperty(Me, "plot")
    Public Property TagLine         As New ProtoProperty(Me, "tagline")
    Public Property Runtime         As New ProtoProperty(Me, "runtime")
    Public Property MpaaCert        As New ProtoProperty(Me, "mpaa")
    Public Property PlayCount       As New ProtoProperty(Me, "playcount"        , CacheMode:=CacheMode.Both)
    Public Property LastPlayed      As New ProtoProperty(Me, "lastplayed")
    Public Property Credits         As New ProtoProperty(Me, "credits")
    Public Property Director        As New ProtoProperty(Me, "director")
    Public Property Premiered       As New ProtoProperty(Me, "premiered")
    Public Property Status          As New ProtoProperty(Me, "status"           , CacheMode:=CacheMode.Both)
    Public Property Aired           As New ProtoProperty(Me, "aired"            , CacheMode:=CacheMode.Both)
    Public Property Studio          As New ProtoProperty(Me, "studio")
    Public Property Trailer         As New ProtoProperty(Me, "trailer")
    Public Property Genre           As New ProtoProperty(Me, "genre")
    Public Property Missing         As New ProtoProperty(Me, "missing"          , CacheMode:=CacheMode.Both)
    Public Property EpExtn          As New ProtoProperty(Me, "epextn"           , CacheMode:=CacheMode.Both)
    Public Property ImdbId          As New ProtoProperty(Me, "imdbid"           , "")
    Public Property TvdbId          As New ProtoProperty(Me, "tvdbid")
    Public Property TmdbId          As New ProtoProperty(Me, "tmdbid")
    Public Property UniqueId        As New ProtoProperty(Me, "uniqueid"         , CacheMode:=CacheMode.Both)
    Public Property ShowId          As New ProtoProperty(Me, "ShowId"           , CacheMode:=CacheMode.Both)
    Public Property ShowLang        As New ProtoProperty(Me, "ShowLang")
    Public Property Showtvdbid      As New ProtoProperty(Me, "Showtvdbid")
    Public Property Showimdbid      As New ProtoProperty(Me, "Showimdbid")
    Public Property Showtmdbid      As New ProtoProperty(Me, "Showtmdbid")
    Public Property ShowTitle       As New ProtoProperty(Me, "ShowTitle")
    Public Property actorsource     As New ProtoProperty(Me, "actorsource")
    Public Property ShowYear        As New ProtoProperty(Me, "ShowYear")
    Public Property sortorder       As New ProtoProperty(Me, "sortorder")
    Public Property EpBookmark      As New ProtoProperty(Me, "epbookmark")
    Public Property Streamdetails   As New StreamDetails(Me, "streamdetails")
    Public Property ListActors      As New ActorList    (Me, "actor")
    Public Property Source          As New ProtoProperty(Me, "videosource")
    Public Property UserRating      As New ProtoProperty(Me, "UserRating"       , "0")
    Public Property DvdEpNumber     As New ProtoProperty(Me, "dvdepnumber"      , "")
    Public Property Thumbnail       As New ProtoImage   (Me, "thumbnail"        , Utilities.DefaultFanartPath)
    Public Property SeasonObj       As TvSeason
    Private Property _showObj       As TvShow
    Public Property ShowObj As TvShow
        Get
            Return _showObj
        End Get
        Set(ByVal value As TvShow)
            _showObj = value
            ShowId.Value = value.TvdbId.Value
            TvdbId.Value = value.TvdbId.Value
            ImdbId.Value = value.ImdbId.Value
        End Set
    End Property
    
    Public Property IsMissing As Boolean
        Get
            Missing.SurpressAlters = True
            If Missing.Value Is Nothing Then
                Missing.Value = Boolean.FalseString
            End If
            'If Missing.Value.ToLower = Boolean.TrueString.ToLower Then
            '    Me.EpisodeNode.ForeColor = Drawing.Color.Blue
            'Else
            '    Me.EpisodeNode.ForeColor = Drawing.Color.Black
            'End If
            Return CBool(_Missing.Value)
        End Get
        Set(ByVal value As Boolean)
            Missing.Value = CStr(value)
            If Missing.Value.ToLower = Boolean.TrueString.ToLower Then
                Me.EpisodeNode.ForeColor = Drawing.Color.Blue
            Else
                Me.EpisodeNode.ForeColor = Drawing.Color.Black
            End If
        End Set
    End Property
    
    Public Property Visible As Boolean
        Get
            If _Visible Then
                If Not Me.SeasonObj.SeasonNode.Nodes.Contains(Me.EpisodeNode) Then
                    Me.SeasonObj.SeasonNode.Nodes.Add(Me.EpisodeNode)

                End If
            Else
                Me.SeasonObj.SeasonNode.Nodes.Remove(Me.EpisodeNode)
            End If
            Return _Visible
        End Get
        Set(ByVal value As Boolean)
            _Visible = value
            If _Visible Then
                If Not Me.SeasonObj.SeasonNode.Nodes.Contains(Me.EpisodeNode) Then
                    Me.SeasonObj.SeasonNode.Nodes.Add(Me.EpisodeNode)

                End If
            Else
                Me.SeasonObj.SeasonNode.Nodes.Remove(Me.EpisodeNode)
            End If

        End Set
    End Property

    Public Sub New()
        MyBase.New("episodedetails")
    End Sub

    Public Sub GetFileDetails()
        Dim fileStreamDetails As StreamDetails = Pref.Get_HdTags(Me.VideoFilePath)
        If Not IsNothing(fileStreamDetails) Then
            Me.StreamDetails.Video = fileStreamDetails.Video
            Me.StreamDetails.Audio.Clear()
            For Each audioStream In fileStreamDetails.Audio
                Me.StreamDetails.Audio.Add(audioStream)
            Next
            Me.StreamDetails.Subtitles.Clear()
            For Each lan In fileStreamDetails.Subtitles
                Me.StreamDetails.Subtitles.Add(lan)
            Next

            If Not Me.StreamDetails.Video.DurationInSeconds.Value Is Nothing Then
                Dim tempstring As String = ""
                tempstring = Me.StreamDetails.Video.DurationInSeconds.Value
                If tempstring <> "" Then
                    If Pref.intruntime Then
                        Me.Runtime.Value = Math.Round(tempstring / 60).ToString
                    Else
                        Me.Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
                    End If
                Else
                    Me.Runtime.Value = tempstring
                End If
            End If
        End If
        'Me.GetFileDetails(Me.VideoFilePath)
    End Sub
    
    Public Sub AbsorbTvdbEpisode(ByRef TvdbEpisode As Tvdb.Episode)
        Me.TvdbId.Value         = TvdbEpisode.Id.Value
        Me.ImdbId.Value         = TvdbEpisode.ImdbId.Value
        Me.TmdbId.Value         = TvdbEpisode.TmdbId.Value
        Me.Title.Value          = TvdbEpisode.EpisodeName.Value
        Me.UniqueId.Value       = TvdbEpisode.Id.Value
        Me.Rating.Value         = TvdbEpisode.Rating.Value
        Me.Votes.Value          = TvdbEpisode.Votes.Value
        Me.Plot.Value           = TvdbEpisode.Overview.Value
        Me.Director.Value       = cleanvalue(TvdbEpisode.Director.Value)
        Me.Credits.Value        = cleanvalue(TvdbEpisode.Credits.Value)
        Me.MpaaCert.Value       = TvdbEpisode.ProductionCode.Value
        Me.Season.Value         = TvdbEpisode.SeasonNumber.Value
        Me.Episode.Value        = TvdbEpisode.EpisodeNumber.Value
        Me.Thumbnail.Url        = TvdbEpisode.ScreenShotUrl
        Me.Thumbnail.FileName   = TvdbEpisode.ScreenShotUrl 'ThumbNail.Value
        Me.Source.Value         = TvdbEpisode.Source.Value
        Me.DisplayEpisode.Value = TvdbEpisode.AirsBeforeEpsisode.Value
        Me.DisplaySeason.Value  = TvdbEpisode.AirsBeforeEpsisode.Value
        Me.UserRating.Value     = "0"
        Me.DvdEpNumber.Value    = TvdbEpisode.DvdEpisodeNumber.Value
        Aired.Value             = TvdbEpisode.FirstAired.Value ' Phyonics - Fix for issue #208
        If TvdbEpisode.GuestStars.Value <> "" Then
            Me.ListActors.Clear()
            Dim guests() As String = TvdbEpisode.GuestStars.Value.Split("|")
            For each guest In guests
                If guest = "" Then Continue For
                Dim newactor As New Actor
                newactor.actorname = guest.trim
                Me.ListActors.Add(newactor)
            Next
        End If
        Me.UpdateTreenode()
    End Sub

    Public Sub AbsorbTvEpisode(ByRef TvEp As TvEpisode)
        Me.TvdbId.Value         = TvEp.Id.Value
        Me.ImdbId.Value         = TvEp.ImdbId.Value
        Me.TmdbId.Value         = TvEp.TmdbId.Value
        Me.Title.Value          = TvEp.Title.Value
        Me.UniqueId.Value       = TvEp.Id.Value
        Me.Rating.Value         = TvEp.Rating.Value
        Me.Votes.Value          = TvEp.Votes.Value
        Me.Plot.Value           = TvEp.Plot.Value
        Me.Director.Value       = cleanvalue(TvEp.Director.Value)
        Me.Credits.Value        = cleanvalue(TvEp.Credits.Value)
        Me.MpaaCert.Value       = TvEp.MpaaCert.Value
        Me.Season.Value         = TvEp.Season.Value
        Me.Episode.Value        = TvEp.Episode.Value
        Me.Source.Value         = TvEp.Source.Value
        Me.Aired.Value          = TvEp.Aired.Value ' Phyonics - Fix for issue #208
        Me.ListActors           = TvEp.ListActors
        Me.StreamDetails        = TvEp.StreamDetails 
        Me.EpBookmark.Value     = TvEp.EpBookmark.Value 
        Me.UserRating.Value     = TvEp.UserRating.Value
        Me.PlayCount.Value      = TvEp.PlayCount.Value
        Me.Runtime.Value        = TvEp.Runtime.Value
        Me.Source.Value         = TvEp.Source.Value 
        Me.DisplayEpisode.Value = TvEp.DisplayEpisode.Value
        Me.DisplaySeason.Value  = TvEp.DisplaySeason.Value
        Me.DvdEpNumber.Value    = TvEp.DvdEpNumber.Value
        Me.UpdateTreenode()
    End Sub

    Public Sub AbsorbTvdbEpisode(ByRef TvdbEpisode As TheTvDB.TvdbEpisode)
        Me.TvdbId.Value         = TvdbEpisode.Identity
        Me.ImdbId.Value         = TvdbEpisode.ImdbId
        Me.TmdbId.Value         = ""
        Me.Title.Value          = TvdbEpisode.EpisodeName
        Me.UniqueId.Value       = TvdbEpisode.Identity
        Me.Rating.Value         = TvdbEpisode.Rating
        Me.Votes.Value          = TvdbEpisode.Votes
        Me.Plot.Value           = TvdbEpisode.Overview
        Me.Director.Value       = cleanvalue(TvdbEpisode.DirectorsDisplayString)
        Me.Credits.Value        = cleanvalue(TvdbEpisode.WritersDisplayString)
        Me.MpaaCert.Value       = TvdbEpisode.ProductionCode
        Me.Season.Value         = TvdbEpisode.SeasonNumber
        Me.Episode.Value        = TvdbEpisode.EpisodeNumber
        Me.Thumbnail.Url        = "http://www.thetvdb.com/banners/" & TvdbEpisode.Image
        'Me.Source.Value         = TvdbEpisode.Source.Value
        Me.Aired.Value          = TvdbEpisode.FirstAired ' Phyonics - Fix for issue #208
        Me.DisplayEpisode.Value = TvdbEpisode.AirsBeforeEpisode
        Me.DisplaySeason.Value  = TvdbEpisode.AirsBeforeSeason
        Me.UserRating.Value     = "0"
        Me.DvdEpNumber.Value    = TvdbEpisode.DVDEpisodeNumber
        If TvdbEpisode.GuestStars.Count > 0 Then
            Me.ListActors.Clear()
            For each guest In TvdbEpisode.GuestStars
                Dim newactor As New Actor
                newactor.actorname = guest
                Me.ListActors.Add(newactor)
            Next
        End If
        Me.UpdateTreenode()
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Title.Value
    End Function

    Public Function cleanvalue(s As String) As String
        If Not String.IsNullOrEmpty(s) Then
            s = s.TrimEnd("|")
            s = s.TrimStart("|")
            s = s.Replace("|", " / ")
        End If
        Return s
    End Function

    Public Function Cachedata() As TvEpisode
        Dim CacheEp As New TvEpisode
        CacheEp.NfoFilePath                 = Me.NfoFilePath
        CacheEp.IsMissing                   = Me.IsMissing
        CacheEp.Title.Value                 = Me.Title.Value
        CacheEp.Season.Value                = Me.Season.Value
        CacheEp.Episode.Value               = Me.Episode.Value
        CacheEp.Aired.Value                 = Me.Aired.Value
        CacheEp.ShowId.Value                = Me.ShowId.Value
        CacheEp.UniqueId.Value              = Me.UniqueId.Value
        CacheEp.EpExtn.Value                = Me.EpExtn.Value
        CacheEp.Playcount.Value             = Me.Playcount.Value
        Return CacheEp
    End Function
End Class

