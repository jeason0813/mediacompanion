Public Class TvBanners
    Public id           As String
    Public Url          As String
    Public SmallUrl     As String
    Public BannerType   As String
    Public Resolution   As String
    Public Language     As String
    Public Season       As String
    Public Rating       As Double = 0

    Public Shared Widening Operator CType(ByVal Input As Tvdb.Banner) As TvBanners
        Dim Output As New TvBanners

        Dim Preamble As String = "http://thetvdb.com/banners/"
        Output.id = Input.Id.Value 
        Output.Url = Preamble & Input.BannerPath.Value
        Output.SmallUrl = Preamble & "_cache/" & Input.BannerPath.Value
        Output.BannerType = Input.Type
        Output.Resolution = Input.Width & "x" & Input.Height
        Output.Language = Input.Language.Value
        Output.Season = Input.Season.Value
        Output.Rating = Input.Rating.Value.ToRating

        Return Output
    End Operator

End Class
