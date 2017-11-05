Imports ProtoXML

Public Class VideoDetails
    Inherits ProtoPropertyGroup



    Public Property Codec As New ProtoProperty(Me, "codec")
    Public Property Aspect As New ProtoProperty(Me, "aspect")
    Public Property Width As New ProtoProperty(Me, "width")
    Public Property Height As New ProtoProperty(Me, "height")
    Public Property DurationInSeconds As New ProtoProperty(Me, "durationinseconds")
    Public Property FormatInfo As New ProtoProperty(Me, "formatinfo")
    Public Property Bitrate As New ProtoProperty(Me, "bitrate")
    Public Property BitrateMode As New ProtoProperty(Me, "bitratemode")
    Public Property BitrateMax As New ProtoProperty(Me, "bitratemax")
    Public Property Container As New ProtoProperty(Me, "Container")
    Public Property CodecId As New ProtoProperty(Me, "codecid")
    Public Property CodecInfo As New ProtoProperty(Me, "codecinfo")
    Public Property ScanType As New ProtoProperty(Me, "scantype")

    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.ProtoXChildBase
        Return New VideoDetails
    End Function
End Class
