
Public Class McImage
    Public Property hdUrl       As String   = ""
    Public Property ldUrl       As String   = ""
    Public Property hdwidth     As String   = ""
    Public Property hdheight    As String   = ""
    Public Property ldwidth     As String   = ""
    Public Property ldheight    As String   = ""
    Public Property votes       As Integer  = 0
    Public Property type        As String = ""
    Public Property Season      As String = ""
    Public Property Id          As String = ""

    Shared Function GetFromTmDbBackDrop( backdrop As Object, Optional hdUrlPrefix As String="", Optional ldUrlPrefix As String="" )

        Dim result = New McImage

        result.hdUrl    = hdUrlPrefix + backdrop.file_path
        result.hdwidth  = backdrop.width .ToString
        result.hdheight = backdrop.height.ToString

        result.ldUrl    = ldUrlPrefix + backdrop.file_path
        result.ldwidth  = "1280"
        result.ldheight = "720"
        Try
            result.votes = backdrop.vote_count
        Catch
            result.votes = 0
        End Try
        Return result

    End Function

    Shared Function GetFromTvDbBackDrop( backdrop As TheTvDB.TvdbBanner, UrlPrefix As String)

        Dim result = New McImage

        result.hdUrl    = UrlPrefix + backdrop.FileName
        If backdrop.Resolution.Contains("x") Then
            Dim resolution() As String = backdrop.Resolution.Split("x")
            result.hdwidth  = resolution(0)
            result.hdheight = resolution(1)
        End If
        result.ldUrl    = UrlPrefix + backdrop.ThumbNail
        result.ldwidth  = "1280"
        result.ldheight = "720"
        result.type     = backdrop.KeyType
        result.Id       = backdrop.Identity
        If IsNumeric(backdrop.SubKey) Then
            result.Season   = backdrop.SubKey
        End If
        
        Try
            result.votes = backdrop.RatingsInfo.Average
        Catch
            result.votes = 0
        End Try
        Return result

    End Function
End Class

Public Class FanartPicBox
    Public Property pbox As PictureBox = New Picturebox
    Public Property imagepath As String = ""
End Class
