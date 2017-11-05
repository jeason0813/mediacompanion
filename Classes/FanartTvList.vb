Imports Media_Companion

Public Class FanartTvMovieList
    Public hdmovieclearart     As List(Of str_fanarttvart)
    Public hdmovielogo         As List(Of str_fanarttvart)
    Public movielogo           As List(Of str_fanarttvart)
    Public movieart            As List(Of str_fanarttvart)
    Public moviebackground     As List(Of str_fanarttvart)
    Public moviedisc           As List(Of str_fanarttvart)
    Public moviebanner         As List(Of str_fanarttvart)
    Public moviethumb          As List(Of str_fanarttvart)
    Public movieposter         As List(Of str_fanarttvart)
    Public dataloaded          As Boolean

    Sub New
        Init
    End Sub

    Public Sub Init
        hdmovieclearart     = New List(Of str_fanarttvart)
        hdmovielogo         = New List(Of str_fanarttvart)
        movielogo           = New List(Of str_fanarttvart)
        movieart            = New List(Of str_fanarttvart)
        moviebackground     = New List(Of str_fanarttvart)
        moviedisc           = New List(Of str_fanarttvart)
        moviebanner         = New List(Of str_fanarttvart)
        moviethumb          = New List(Of str_fanarttvart)
        movieposter         = New List(Of str_fanarttvart)
        dataloaded          = False
    End Sub
End Class

Public Class FanartTvTvList
    Public hdtvlogo             As List(Of str_fanarttvart)
    Public hdclearart           As List(Of str_fanarttvart)
    Public clearlogo            As List(Of str_fanarttvart)
    Public clearart             As List(Of str_fanarttvart)
    Public tvposter             As List(Of str_fanarttvart)
    Public tvthumb              As List(Of str_fanarttvart)
    Public tvbanner             As List(Of str_fanarttvart)
    Public showbackground       As List(Of str_fanarttvart)
    Public seasonposter         As List(Of str_fanarttvart)
    Public seasonbanner         As List(Of str_fanarttvart)
    Public seasonthumb          As List(Of str_fanarttvart)
    Public characterart         As List(Of str_fanarttvart)
    Public dataloaded           As Boolean

    Sub New
        Init
    End Sub

    Public Sub Init
        hdtvlogo            = New List(Of str_fanarttvart)
        hdclearart          = New List(Of str_fanarttvart)
        clearlogo           = New List(Of str_fanarttvart)
        clearart            = New List(Of str_fanarttvart)
        tvposter            = New List(Of str_fanarttvart)
        tvthumb             = New List(Of str_fanarttvart)
        tvbanner            = New List(Of str_fanarttvart)
        showbackground      = New List(Of str_fanarttvart)
        seasonposter        = New List(Of str_fanarttvart)
        seasonbanner        = New List(Of str_fanarttvart)
        seasonthumb         = New List(Of str_fanarttvart)
        characterart        = New List(Of str_fanarttvart)
        dataloaded          = False
    End Sub
End Class
