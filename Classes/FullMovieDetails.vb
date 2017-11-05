Imports Media_Companion

Public Class FullMovieDetails
    Public fileinfo          As FileDetails                 = New FileDetails()
    Public fullmoviebody     As BasicMovieNFO               = New BasicMovieNFO()
    Public alternativetitles As List(Of String)             = New List(Of String)
    Public listactors        As List(Of str_MovieActors)    = New List(Of str_MovieActors)
    Public frodoPosterThumbs As List(Of FrodoPosterThumb)   = New List(Of FrodoPosterThumb)
    Public frodoFanartThumbs As FrodoFanartThumbs           = New FrodoFanartThumbs
    Public listthumbs        As List(Of String)             = New List(Of String)
    Public filedetails       As StreamDetails               = New StreamDetails

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
