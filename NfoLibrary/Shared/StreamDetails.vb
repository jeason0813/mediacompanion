Imports ProtoXML

Public Class StreamDetails
    Inherits ProtoPropertyGroup


    Public Property Video As New VideoDetails(Me, "video")
    Public Property Audio As New AudioList(Me, "audio")
    Public Property Subtitles As New SubtitleList(Me, "subtitle")

    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.ProtoXChildBase
        Return New StreamDetails
    End Function
End Class
