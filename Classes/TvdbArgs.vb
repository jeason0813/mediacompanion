Public Class TvdbArgs
    Property tvdbid     As String
    Property folder      As String
    Property episode    As Boolean
    Property lang       As String

    Sub New(Optional tvdbid As String = "", Optional folder As String = "", Optional isep As Boolean = True, Optional lang As String = "")
        Me.tvdbid   = tvdbid
        Me.folder   = folder
        Me.episode  = isep
        Me.lang     = lang
    End Sub
End Class
