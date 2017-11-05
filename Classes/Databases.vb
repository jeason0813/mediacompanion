
Public Class Databases

    Property ActorName As String = ""
    Property MovieId   As String = ""

    Sub New

    End Sub

    Sub New( _actorname As String, _movieid As String)
        ActorName = _actorname
        MovieId   = _movieid
    End Sub

End Class

Public Class DirectorDatabase

    Property ActorName As String = ""
    Property MovieId   As String = ""

    Sub New

    End Sub

    Sub New( _actorname As String, _movieid As String)
        ActorName = _actorname
        MovieId   = _movieid
    End Sub

End Class

Public Class MovieSetDatabase

    Public tmdbid   As String
    Public title    As String
    Public year     As String
    Public present  As Boolean
    
    Sub New()

    End Sub

    Sub New(Optional _tmdbid As String = "", Optional _title As String = "", Optional _year As String = "", Optional _present As Boolean = False)
        tmdbid      = _tmdbid
        title       = _title
        year        = _year
        present     = _present
    End Sub
End Class

Public Class TagDatabase

    Property TagName    As String = ""
    Property MovieId    As String = ""

    Sub New()

    End Sub

    Sub New( _tagname As String, _movieid As String)
        TagName     = _tagname
        MovieId     = _movieid
    End Sub

End Class
