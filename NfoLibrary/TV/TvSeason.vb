Imports Media_Companion

Public Class TvSeason
    Inherits ProtoXML.ProtoFile

    Public Sub New()
        MyBase.New("season")
    End Sub

    Private _ShowObj As TvShow
    Public Property ShowObj As TvShow
        Get
            Return _ShowObj
        End Get
        Set(value As TvShow)
            _ShowObj = value
            Me.Poster.FolderPath = _ShowObj.FolderPath
            Me.Banner.FolderPath = _ShowObj.FolderPath 
        End Set
    End Property

    Public MaxEpisodeCount As Integer

    Private _SeasonNumber As Integer
    Public Shadows Property SeasonNumber As Integer
        Get
            'If Not Me.SeasonNode.Text = "Season " & Utilities.PadNumber(_SeasonNumber, 2) Then
                'Me.SeasonNode.Text = "Season " & Utilities.PadNumber(_SeasonNumber, 2)
            'End If
            Return _SeasonNumber
        End Get
        Set(ByVal value As Integer)

            Select Case value
                Case -1                  'case -1 is unknown season
                    Me.Poster.FileName = "season-specials" & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")
                    Me.Banner.FileName = "season-specials-banner.jpg"
                Case 0                   'case 0 is specials
                    Me.Poster.FileName = "season-specials" & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")
                    Me.Banner.FileName = "season-specials-banner.jpg"
                Case Else                'all other are normal seasons 
                    Me.Poster.FileName = "season" & Media_Companion.Utilities.PadNumber(value, 2) & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")
                    Me.Banner.FileName = "season" & Media_Companion.Utilities.PadNumber(value, 2) & "-banner.jpg"
            End Select
            
            _SeasonNumber = value

            Me.SeasonNode.Text = "Season " & Utilities.PadNumber(_SeasonNumber, 2)
        End Set
    End Property
    Private _SeasonLabel As String
    Public Shadows Property SeasonLabel As String
        Get
            Me.SeasonNode.Text = _SeasonLabel
            Return _SeasonLabel
        End Get
        Set(ByVal value As String)
            _SeasonLabel = value
            Me.SeasonNode.Text = value
        End Set
    End Property

    Public Property ShowId As New ProtoXML.ProtoProperty(Me, "ShowId")
    Public Property Poster As New ProtoXML.ProtoImage(Me, "poster", Utilities.DefaultPosterPath) With {.FileName = "seasonX" & If(Pref.FrodoEnabled, "-poster.jpg", ".tbn")}
    Public Property Banner As New ProtoXML.ProtoImage(Me, "banner", Utilities.DefaultBannerPath) With {.FileName = "seasonX" & "-banner.jpg"}

    Public Property Episodes As New List(Of TvEpisode)
    Public Property Playcount As New ProtoXML.ProtoProperty(Me, "playcount")

    Private _Visible As Boolean
    Public Property Visible As Boolean
        Get
            If _Visible Then
                If Not Me.ShowObj.ShowNode.Nodes.Contains(Me.SeasonNode) Then
                    Me.ShowObj.ShowNode.Nodes.Add(Me.SeasonNode)
                End If
            Else
                Me.SeasonNode.Remove()
            End If

            Return _Visible
        End Get
        Set(ByVal value As Boolean)
            _Visible = value
            If _Visible Then
                If Not Me.ShowObj.ShowNode.Nodes.Contains(Me.SeasonNode) Then
                    Me.ShowObj.ShowNode.Nodes.Add(Me.SeasonNode)
                End If
            Else
                Me.SeasonNode.Remove()
            End If

        End Set
    End Property

    Public ReadOnly Property VisibleEpisodeCount As Integer
        Get
            Dim Count As Integer = 0
            For Each Ep As TvEpisode In Episodes
                If Ep.Visible Then
                    Count += 1
                End If
            Next
            Return Count
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Me.SeasonLabel
    End Function

End Class
