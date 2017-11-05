Imports System.Linq
Imports System.Collections.Generic
'Imports System.IO
Imports System.xml

Public Class xbmcmovies
    Public fileinfo          As FileDetails
    Public fullmoviebody     As BasicMovieNFO
    Public alternativetitles As List(Of String)
    Public listactors        As List(Of str_MovieActors)
    Public frodoPosterThumbs As List(Of FrodoPosterThumb)
    Public frodoFanartThumbs As FrodoFanartThumbs
    Public listthumbs        As List(Of String)
    Public filedetails       As StreamDetails

    Sub New
        Init
    End Sub

    Public Sub Init
        fileinfo          = New FileDetails()
        fullmoviebody     = New BasicMovieNFO()
        alternativetitles = New List(Of String)
        listactors        = New List(Of str_MovieActors)
        frodoPosterThumbs = New List(Of FrodoPosterThumb)
        frodoFanartThumbs = New FrodoFanartThumbs
        listthumbs        = New List(Of String)
        filedetails       = New StreamDetails
    End Sub
End Class

Public Class xbmctvseries
    Public series As xbmctv
    Public episodes As List(Of xbmctv)

    Sub New()
        init()
    End Sub

    Public Sub init
        series = New xbmctv
        episodes = New List(Of xbmctv) 
    End Sub
End Class

Public Class xbmctv
    Public Title As String
    Public ShowTitle As String
    Public Rating As String
    Public Epbookmark As String
    Public Year As String
    Public Top250 As String
    Public Season As String
    Public Episode As String
    Public UniqueId As String
    Public DisplaySeason As String
    Public DisplayEpisode As String
    Public Votes As String
    Public Outline As String
    Public Plot As String
    Public Tagline As String
    Public Runtime As String
    Public Mpaa As String
    Public Playcount As String
    Public LastPlayed As String
    Public File As String
    Public Path As String
    Public Filenameandpath As String
    Public basepath As String
    Public EpisodeGuideUrl As String
    Public Url As String
    Public TvdbId As String
    Public Genre As List(Of String)
    Public Premiered As String
    Public Status As String
    Public Code As String
    Public Aired As String
    Public Studio As String
    Public Credits As List(Of String)
    Public Director As List(Of String)
    Public Trailer As String
    Public ListActors As List(Of str_MovieActors)
    Public position As String
    Public total As String
    Public fileinfo As StreamDetails
    Public art As List(Of xbmctvart)
    Sub new()
        init
    End Sub

    Sub init
        Title = ""
        ShowTitle = ""
        Rating = ""
        Epbookmark = ""
        Year = ""
        Top250 = ""
        Season = ""
        Episode = ""
        UniqueId = ""
        DisplaySeason = ""
        DisplayEpisode = ""
        Votes = ""
        Outline = ""
        Plot = ""
        Tagline = ""
        Runtime = ""
        Mpaa = ""
        Playcount = ""
        LastPlayed = ""
        File = ""
        Path = ""
        Filenameandpath = ""
        basepath = ""
        EpisodeGuideUrl = ""
        Url = ""
        TvdbId = ""
        Genre = New List(Of String)
        Premiered = ""
        Status = ""
        Code = ""
        Aired = ""
        Studio = ""
        Credits = New List(Of String)
        Director = New List(Of String)
        Trailer = ""
        ListActors = New List(Of str_MovieActors)
        position = ""
        total = ""
        fileinfo = New StreamDetails
        art = New List(Of xbmctvart)
    End Sub
End Class

Public Class xbmctvart
    Public series As List(Of xbmcart)
    Public seasons As List(Of xbmcart)

    Sub new()
        init()
    End Sub

    Public Sub init
        series = New List(Of xbmcart)
        seasons = New List(Of xbmcart)
    End Sub

End Class

Public Class xbmcart
    Public seasonnum As Integer
    Public banner As String
    Public poster As String
    Public fanart As String
    Public thumb As String

    Sub new()
        init()
    End Sub

    Public Sub init
        seasonnum = Nothing
        banner = ""
        poster = ""
        fanart = ""
        thumb = ""
    End Sub

    
End Class

Public Class XBMCPaths
    Public rootpath As String
    Public pathsource As String

    Sub New()
        init()
    End Sub

    Public Sub init
        rootpath = ""
        pathsource = "" 
    End Sub
End Class
