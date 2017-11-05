Imports ProtoXML
Namespace Tvdb
    Public Class Banner
        Inherits ProtoPropertyGroup


        Public Property Id As New ProtoProperty(Me, "id")
        Public Property BannerPath As New ProtoProperty(Me, "BannerPath")
        Public Property BannerType As New ProtoProperty(Me, "BannerType")
        Public Property BannerType2 As New ProtoProperty(Me, "BannerType2")
        Public Property Colors As New ProtoProperty(Me, "Colors")
        Public Property Language As New ProtoProperty(Me, "Language")
        Public Property Rating As New ProtoProperty(Me, "Rating")
        Public Property RatingCount As New ProtoProperty(Me, "RatingCount")
        Public Property SeriesName As New ProtoProperty(Me, "SeriesName")
        Public Property ThumbnailPath As New ProtoProperty(Me, "ThumbnailPath")
        Public Property VignettePath As New ProtoProperty(Me, "VignettePath")
        Public Property Season As New ProtoProperty(Me, "Season")

        Public ReadOnly Property Url As String
            Get
                Return "http://www.thetvdb.com/banners/" & BannerPath.Value
            End Get
        End Property

        Private _Type
        Public Property Type As ArtType
            Get
                Dim Parts As String()
                If Me.BannerType.Value = "fanart" Then
                    Parts = Me.BannerType2.Value.Split("x")
                    Me.Width = Parts(0)
                    Me.Height = Parts(1)

                    Return ArtType.Fanart
                End If
                If Me.BannerType.Value = "poster" Then
                    Parts = Me.BannerType2.Value.Split("x")
                    Me.Width = Parts(0)
                    Me.Height = Parts(1)

                    Return ArtType.Poster
                End If

                If Me.BannerType.Value = "season" Then
                    If Me.BannerType2.Value = "season" Then
                        Return ArtType.SeasonPoster
                    ElseIf Me.BannerType2.Value = "seasonwide" Then
                        Return ArtType.SeasonBanner
                    ElseIf Me.BannerType2.Value = "graphical" Then
                        Return ArtType.SeasonBanner
                    ElseIf Me.BannerType2.Value = "blank" Then
                        Return ArtType.Blank
                    End If
                End If


                Return _Type
            End Get
            Set(ByVal value As ArtType)
                _Type = value
            End Set
        End Property

        Public Property Width As Integer
        Public Property Height As Integer

        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New Banner()
        End Function
    End Class

    Public Enum ArtType
        Fanart
        Banner
        Poster
        SeasonPoster
        SeasonBanner
        Blank
    End Enum
End Namespace