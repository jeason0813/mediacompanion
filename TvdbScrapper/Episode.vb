Imports ProtoXML
Imports TheTvDB
Namespace Tvdb
    Public Class Episode
        Inherits ProtoPropertyGroup

        '<id>332179</id>
        Public Property Id As New ProtoProperty(Me, "id")
        '<DVD_chapter></DVD_chapter>
        Public Property CombinedEpisodeNumber As New ProtoProperty(Me, "Combined_episodenumber")
        Public Property CombinedSeason As New ProtoProperty(Me, "Combined_season")
        Public Property DvdChapter As New ProtoProperty(Me, "DVD_chapter")
        '<DVD_discid></DVD_discid>
        Public Property DvdDiscId As New ProtoProperty(Me, "DVD_discid")
        '<DVD_episodenumber></DVD_episodenumber>
        Public Property DvdEpisodeNumber As New ProtoProperty(Me, "DVD_episodenumber")
        '<DVD_season></DVD_season>
        Public Property DvdSeason As New ProtoProperty(Me, "DVD_season")
        '<Director>|Joseph McGinty Nichol|</Director>
        Public Property Director As New ProtoProperty(Me, "Director")
        '<EpisodeName>Chuck Versus the World</EpisodeName>
        Public Property EpisodeName As New ProtoProperty(Me, "EpisodeName")
        '<EpisodeNumber>1</EpisodeNumber>
        Public Property EpisodeNumber As New ProtoProperty(Me, "EpisodeNumber")
        '<FirstAired>2007-09-24</FirstAired>
        Public Property FirstAired As New ProtoProperty(Me, "FirstAired")
        '<GuestStars>|Julia Ling|Vik Sahay|Mieko Hillman|</GuestStars>
        Public Property GuestStars As New ProtoProperty(Me, "GuestStars")
        '<Credits>Bob Geldof</Credits>
        Public Property Credits As New ProtoProperty(Me, "Credits")
        '<IMDB_ID></IMDB_ID>
        Public Property ImdbId As New ProtoProperty(Me, "IMDB_ID")
        '<TMDB_ID></TMDB_ID>
        Public Property TmdbId As New ProtoProperty(Me, "TMDB_ID")
        '<Language>English</Language>
        Public Property Language As New ProtoProperty(Me, "Language")
        '<Overview>Chuck Bartowski is an average computer geek...</Overview>
        Public Property Overview As New ProtoProperty(Me, "Overview")
        '<ProductionCode></ProductionCode>
        Public Property ProductionCode As New ProtoProperty(Me, "ProductionCode")
        '<Rating>9.0</Rating>
        Public Property Rating As New ProtoProperty(Me, "Rating")
        '<RatingCount>75</RatingCount>
        Public Property Votes As New ProtoProperty(Me, "RatingCount")
        '<SeasonNumber>1</SeasonNumber>
        Public Property SeasonNumber As New ProtoProperty(Me, "SeasonNumber")
        '<Writer>|Josh Schwartz|Chris Fedak|</Writer>
        Public Property Writer As New ProtoProperty(Me, "Writer")
        '<absolute_number></absolute_number>
        Public Property AbsoluteNumber As New ProtoProperty(Me, "absolute_number")
        '<airsafter_season></airsafter_season>
        Public Property AirsAfterSeason As New ProtoProperty(Me, "airsafter_season")
        '<airsbefore_episode></airsbefore_episode>
        Public Property AirsBeforeEpsisode As New ProtoProperty(Me, "airsbefore_episode")
        '<airsbefore_season></airsbefore_season>
        Public Property AirsBeforeSeason As New ProtoProperty(Me, "airsbefore_season")
        '<filename>episodes/80348-332179.jpg</filename>
        Public Property ThumbNail As New ProtoProperty(Me, "filename")
        '<lastupdated>1201292806</lastupdated>
        Public Property LastUpdated As New ProtoProperty(Me, "lastupdate")
        '<seasonid>27985</seasonid>
        Public Property SeasonId As New ProtoProperty(Me, "seasonid")
        '<seriesid>80348</seriesid>
        Public Property SeriesId As New ProtoProperty(Me, "seriesid")
        '<videosource>bdrip</videosource>
        Public Property Source As New ProtoProperty(Me, "videosource")
        'Media Companion confirm load success
        Public Property FailedLoad As Boolean

        Public ReadOnly Property ScreenShotUrl As String
            Get
                If Me.ThumbNail.Value.Contains("http://") Then Return Me.ThumbNail.Value
                Return "http://thetvdb.com/banners/" & Me.ThumbNail.Value
            End Get
        End Property


        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New Episode
        End Function

        'Public Sub AbsorbEpisode(ByVal NewEpisode As TvdbEpisode)
        '    Me.Id.Value                     = NewEpisode.Identity
        '    Me.CombinedEpisodeNumber.Value  = NewEpisode.DVDEpisode
        '    Me.CombinedSeason.Value         = NewEpisode.DVDSeason
        '    Me.DvdChapter.Value             = NewEpisode.DVDChapter
        '    Me.DvdDiscId.Value              = NewEpisode.DVDDiscID
        '    Me.DvdEpisodeNumber.Value       = NewEpisode.DVDEpisodeNumber
        '    Me.DvdSeason.Value              = NewEpisode.DVDSeasonNumber
        '    Me.Director.Value               = NewEpisode.DirectorsDisplayString.Replace(", ","|")
        '    Me.EpisodeName.Value            = NewEpisode.EpisodeName
        '    Me.EpisodeNumber.Value          = NewEpisode.EpisodeNumber
        '    Me.FirstAired.Value             = NewEpisode.FirstAired
        '    Me.GuestStars.Value             = NewEpisode.GuestStarsDisplayString.Replace(", ","|")
        '    Me.Credits.Value                = ""
        '    Me.ImdbId.Value                 = NewEpisode.ImdbId
        '    Me.TmdbId.Value                 = ""
        '    Me.Language.Value               = NewEpisode.Language.ToString
        '    Me.Overview.Value               = NewEpisode.Overview
        '    Me.ProductionCode.Value         = NewEpisode.ProductionCode
        '    Me.Rating.Value                 = NewEpisode.Rating
        '    Me.Votes.Value                  = NewEpisode.Votes
        '    Me.SeasonNumber.Value           = NewEpisode.SeasonNumber
        '    Me.Writer.Value                 = NewEpisode.WritersDisplayString.Replace(", ","|")
        '    Me.AbsoluteNumber.Value         = NewEpisode.AbsoluteNumber
        '    Me.AirsAfterSeason.Value        = NewEpisode.AirsAfterSeason
        '    Me.AirsBeforeEpsisode.Value     = NewEpisode.AirsBeforeEpisode
        '    Me.AirsBeforeSeason.Value       = NewEpisode.AirsBeforeSeason
        '    Me.ThumbNail.Value              = NewEpisode.Image
        '    Me.LastUpdated.Value            = NewEpisode.LastUpdated
        '    Me.SeasonId.Value               = ""
        '    Me.SeriesId.Value               = NewEpisode.SeriesId
        '    Me.Source.Value                 = ""
        'End Sub
    End Class
End Namespace