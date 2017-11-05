
Public Class ListOfProfiles

    Property MovieCache         As String = ""
    Property TvCache            As String = ""
    Property CustomTvCache      As String = ""
    Property ActorCache         As String = ""
    Property MusicVideoCache    As String = ""
    Private _directorCache      As String = ""

    Property DirectorCache  As String
        Get
            Return IIf(_directorCache="", Pref.applicationPath & "\Settings\directorcache.xml", _directorCache)
        End Get

        Set(value As String)
            _directorCache=value
        End Set
    End Property

    Property ProfileName    As String = ""
    Property RegExList      As String = ""
    Property Genres         As String = ""
    Property Config         As String = ""
    Property HomeMovieCache As String = ""
    Property MovieSetCache  As String = ""

    Public Sub Assign(profileTo As ListOfProfiles)
        profileTo.ActorCache        = ActorCache
        profileTo.DirectorCache     = DirectorCache
        profileTo.Config            = Config
        profileTo.MovieCache        = MovieCache
        profileTo.ProfileName       = ProfileName
        profileTo.RegExList         = RegExList
        profileTo.Genres            = Genres
        profileTo.TvCache           = TvCache
        profileTo.CustomTvCache     = CustomTvCache
        profileTo.ProfileName       = ProfileName
        profileTo.MusicVideoCache   = MusicVideoCache
        profileTo.MovieSetCache     = MovieSetCache
    End Sub

End Class
