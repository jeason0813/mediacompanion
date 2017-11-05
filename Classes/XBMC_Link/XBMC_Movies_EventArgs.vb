Imports XBMC.JsonRpc

Public Class XBMC_MaxMovies_EventArgs : Inherits EventArgs

    Public Property XbmcMovies As List(Of MaxXbmcMovie)

    Sub New(ByVal XbmcMovies As List(Of MaxXbmcMovie))
        Me.XbmcMovies = XbmcMovies
    End Sub

End Class


Public Class ComboList_EventArgs : Inherits EventArgs

    Public Property XbmcMovies As List(Of ComboList)

    Sub New(ByVal XbmcMovies As List(Of ComboList))
        Me.XbmcMovies = XbmcMovies
    End Sub

End Class


Public Class XBMC_MC_Movies_EventArgs : Inherits EventArgs

    Public Property XbmcMcMovies As Dictionary(Of String, XbmcMovieForCompare)

    Sub New(ByVal XbmcMcMovies As Dictionary(Of String, XbmcMovieForCompare))
        Me.XbmcMcMovies = XbmcMcMovies
    End Sub

End Class



Public Class XBMC_Only_Movies_EventArgs : Inherits EventArgs

    Public Property XbmcOnlyMovies As List(Of XbmcMovieForCompare)

    Sub New(ByVal XbmcOnlyMovies As List(Of XbmcMovieForCompare))
        Me.XbmcOnlyMovies = XbmcOnlyMovies
    End Sub

End Class

