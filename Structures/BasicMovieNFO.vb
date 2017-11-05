
Public Class BasicMovieNFO
    Property title         As String = ""
    Property originaltitle As String = ""
    Property sortorder     As String = ""
    Property source        As String = ""
    Property year          As String = ""
    Property rating        As String = ""
    Property votes         As String = ""
    Property outline       As String = ""
    Property plot          As String = ""
    Property tagline       As String = ""
    Property runtime       As String = ""
    Property mpaa          As String = ""
    Property genre         As String = ""
    Property tag           As New List(Of String)
    Property credits       As String = ""
    Property director      As String = ""
    Property stars         As String = ""
    Property premiered     As String = ""
    Property studio        As String = ""
    Property trailer       As String = ""
    Property playcount     As String = ""
    Property lastplayed    As String = ""
    Property imdbid        As String = ""
    Property tmdbid        As String = ""
    Property top250        As String = ""
    Property filename      As String = ""
    Property thumb         As String = ""
    Property fanart        As String = ""
    Property country       As String = ""
    Property album         As String = ""
    Property artist        As String = ""
    Property track         As String = ""
    Property showlink      As String = ""
    Property usrrated      As String = ""
    Property metascore     As String = ""
    Property LockedFields  As New List(Of String)

    Private _setName       As String = "-None-"
    Private _setId         As String = ""
    Private _setoverview   As String = ""

    Public Property SetName As String
        Get
            Return _setName 
        End Get
        Set
            If Not Locked("set") Then
                _setName = value
            End If
        End Set
    End Property

    Public Property TmdbSetId As String
        Get
            Return _setId
        End Get
        Set
            If Not Locked("set") Then
                _setId = value
            End If
        End Set
    End Property

    Public Property SetOverview As String
        Get
            Return _setoverview
        End Get
        Set(value As String)
            If Not Locked("set") Then
                _setoverview = value
            End If
        End Set
    End Property

    Sub New
    End Sub

    Sub ClearWatched()
        playcount = "0"
    End Sub 

    Sub SetWatched()
        playcount = "1"
    End Sub 
 
    Function Locked(field As String) As Boolean
        Return LockedFields.Contains(field)
    End Function

End Class
