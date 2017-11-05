Imports ProtoXML
Namespace Tvdb
    Public Class ShowData
        Inherits ProtoFile

        Public Sub New()
            MyBase.New("Data")
        End Sub

        Public Property Series As New SeriesList(Me, "Series")
        Public Property Episodes As New EpisodeList(Me, "Episode")
    End Class
End Namespace