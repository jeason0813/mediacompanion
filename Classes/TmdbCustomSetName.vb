
Public Class TmdbCustomSetName
    Public MovieSetId       As String = ""
    Public UserMovieSetName As String = ""
    Public MovieSetName     As String = ""

 
    Sub New (_MovieSetId As String, _UserMovieSetName As String, _MovieSetName As String)
        MovieSetId       = _MovieSetId       
        UserMovieSetName = _UserMovieSetName 
        MovieSetName     = _MovieSetName     
    End Sub

End Class
