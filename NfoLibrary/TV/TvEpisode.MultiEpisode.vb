Partial Public Class TvEpisode
    Public Property MultiEpisodeNode As XElement

    Private _HasSecondary As Boolean
    Public Property HasSecondary As Boolean 'Are there other episodes in this file?
        Get
            Return _HasSecondary
        End Get
        Set(value As Boolean)


            If Not (value = _HasSecondary) Then
                If value Then

                    MultiEpisodeNode = <multiepisode></multiepisode>
                    Me.Node.Remove()
                    MultiEpisodeNode.Add(Me.Node)
                    Me.Doc.Add(MultiEpisodeNode)
                Else
                    Me.Node.Remove()
                    Me.Doc.Add(Me.Node)

                    MultiEpisodeNode = Nothing
                End If
            End If
            _HasSecondary = value
        End Set
    End Property

    Private _IsSecondary As Boolean
    Public ReadOnly Property IsSecondary As Boolean 'Is this episode a second episode in a multi episode file?
        Get
            Return _IsSecondary
        End Get
    End Property

    Public Sub MakeSecondaryTo(ByRef Prime As TvEpisode)
        Prime.HasSecondary = True
        Me.Node.Remove()
        Prime.MultiEpisodeNode.Add(Me.Node)

        Prime.OtherEpisodes.Add(Me)

        Me.Doc = Prime.Doc

        Me.VideoFilePath = Prime.VideoFilePath

        Me.Primary = Prime

        _IsSecondary = True
    End Sub

    Public Sub MakeIndependant(ByVal VideoFilePath As String)
        Me.Primary.OtherEpisodes.Remove(Me)
        Me.VideoFilePath = VideoFilePath
        _IsSecondary = False
        Me.Doc = New XDocument(New XDeclaration("1.0", "UTF-8", "yes"))
        Me.Node.Remove()
        Me.Doc.Add(Me.Node)
    End Sub

    Public Property Primary As TvEpisode
    Public Property OtherEpisodes As New List(Of TvEpisode)
End Class
