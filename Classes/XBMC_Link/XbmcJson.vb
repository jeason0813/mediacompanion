Imports System.ComponentModel
'Imports System.IO
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports Media_Companion
Imports System.Text
Imports XBMC.JsonRpc
Imports Newtonsoft.Json.Linq

Public Class XbmcJson

    Private _opened     As Boolean = False
    Private _xbmcMovies As List(Of MinXbmcMovie)

    Public Property Address  As String = Pref.XBMC_Address
    Public Property Port     As String = Pref.XBMC_Port
    Public Property Username As String = Pref.XBMC_Username
    Public Property Password As String = Pref.XBMC_Password

    Public Property xbmc     As XbmcJsonRpcConnection

    Public Event XbmcMovies_OnChange(XbmcMovies As List(Of MinXbmcMovie))
    Public Event MovieRemoved

    Public ReadOnly Property Opened As Boolean
        Get
            Return _opened
        End Get
    End Property


    Public ReadOnly Property XbmcMovies As List(Of MinXbmcMovie)
        Get
            If IsNothing(_xbmcMovies) Then
                GetXbmcMovies
            End If
            Return _xbmcMovies
        End Get
    End Property


    Sub New
        xbmc = new XbmcJsonRpcConnection(Address, Port, Username, Password)
    End Sub


    Function Open As Boolean
        Return xbmc.Open
    End Function


    'Function UpdateXbmcMovies(Optional title As String=Nothing) As Boolean
    '    Try
    '        If IsNothing(_xbmcMovies) Then
    '            _xbmcMovies = New List(Of MinXbmcMovie)
    '        End If
    '        Dim movies As List(Of MinXbmcMovie) = xbmc.Library.Video.GetMinXbmcMovies(title)

    '        XbmcMovies.RemoveAll( Function(x) Exists(movies,x.file) ) 
                                     
    '        _xbmcMovies.AddRange(movies)
    '        Return True
    '    Catch
    '        Return False
    '    End Try
    'End Function


    Function GetXbmcMovies As Boolean
        Try
            _xbmcMovies = xbmc.Library.Video.GetMinXbmcMovies()
            RaiseEvent XbmcMovies_OnChange(_xbmcMovies)
            Return True
        Catch
            Return False
        End Try
    End Function


    Sub RemoveXbmcMovie(MediaName As String)

        XbmcMovies.RemoveAll(Function(x) x.file.ToUpper.Trim.Contains(MediaName.ToUpper.Trim))
        RaiseEvent MovieRemoved
    End Sub


    Function Exists(movies As List(Of MinXbmcMovie), MediaName As String)  As Boolean

        Dim q = From m In movies Where m.file.ToUpper.Trim.Contains(MediaName.ToUpper.Trim)

        Return q.Count>0
    End Function

    Function NumMoviesInFolder(Folder As String)  As Integer

        Dim q = From m In XbmcMovies Where m.file.ToUpper.Trim.Contains(Folder.ToUpper)

        Return q.Count
    End Function


    Function GetMinXbmcMovie(MediaName As String)  As MinXbmcMovie

        Dim q = From m In XbmcMovies Where m.file.ToUpper.Trim.Contains(MediaName.ToUpper.Trim)

        If q.Count=0 Then Return Nothing

        Return q.First
    End Function

    Public ReadOnly Property MoviesWithUniqueMovieTitles As List(Of MinXbmcMovie)
        Get
            Dim q = From x In XbmcMovies 
                    Join
                       u In UniqueMovieTitles On u Equals x.Title
                    Select
                        x

            Return q.AsEnumerable.ToList
        End Get
    End Property    

    Public ReadOnly Property UniqueMovieTitles As List(Of String)
        Get
            Dim q = From x In XbmcMovies 
                    Select 
                        x.Title
                    Group By 
                        Title Into Num=Count
                    Select 
                        Title,Num
                    Where
                        Num = 1
                    Select
                        Title

            Return q.AsEnumerable.ToList
        End Get
    End Property    

End Class
