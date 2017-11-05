Imports Media_Companion

Partial Public Class TvShow
    Public Property TvdbData As Media_Companion.Tvdb.ShowData


    Public Property FanartData As New FanartScraper.Fanart
    Public Sub DownloadFanartData()
        Dim Data As String = Utilities.DownloadTextFiles("http://fanart.tv/api/fanart.php?id=" & Me.TvdbId.Value)

        FanartData.LoadXml(Data)
        FanartData.FolderPath = Me.FolderPath

        If FanartData.ClearArts.Count > 0 Then
            FanartData.ClearArts.Item(0).DownloadFile()
        End If

        If FanartData.ClearLogos.Count > 0 Then
            FanartData.ClearLogos.Item(0).DownloadFile()
        End If

        If FanartData.TvThumbs.Count > 0 Then
            FanartData.TvThumbs.Item(0).DownloadFile()
        End If
    End Sub

    Public Sub Scrape(Optional ByVal IgnoreCache As Boolean = False)
        Dim ScrapeTask As Tasks.ScrapeShowTask = New Tasks.ScrapeShowTask(Me)

        Common.Tasks.Add(ScrapeTask)
    End Sub

End Class
