Imports ProtoXML
Imports System.Xml

Public Class AudioDetails
    Inherits ProtoPropertyGroup


    Public Property Codec        As New ProtoProperty(Me, "codec"       , "")
    Public Property Language     As New ProtoProperty(Me, "language"    , "")
    Public Property DefaultTrack As New ProtoProperty(Me, "DefaultTrack", "")
    Public Property Channels     As New ProtoProperty(Me, "channels"    , "")
    Public Property Bitrate      As New ProtoProperty(Me, "bitrate"     , "")

    Public Sub New()
        MyBase.New(Nothing, Nothing)
        'Throw New NotImplementedException()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
        Return New AudioDetails
    End Function

    Public Sub Assign(from As AudioDetails)
        me.Codec        = from.Codec
        me.Language     = from.Language
        me.DefaultTrack = from.DefaultTrack
        me.Channels     = from.Channels
        me.Bitrate      = from.Bitrate 
    End Sub

    Public Function GetChild(doc As XmlDocument) As XmlElement

        Dim child = doc.CreateElement("audio")

        child.AppendChild(Language    .GetChild(doc))
        child.AppendChild(DefaultTrack.GetChild(doc))
        child.AppendChild(Codec       .GetChild(doc))
        child.AppendChild(Channels    .GetChild(doc))
        child.AppendChild(Bitrate     .GetChild(doc))

        Return child
    End Function
End Class

