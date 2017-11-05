Imports ProtoXML
Imports System.Xml

Public Class SubtitleDetails
    Inherits ProtoPropertyGroup


    Public Property Language    As New ProtoProperty(Me, "language" , "")
    Public Property Primary     As Boolean = False
    Public Property Forced      As Boolean = False

    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
        Return New SubtitleDetails
    End Function

    Public Function GetChild(doc As XmlDocument) As XmlElement

        Dim child = doc.CreateElement("subtitlelang")

        child.AppendChild(Language  .GetChild(doc))
        child.AppendChild(doc, "forced", Forced)

        Return child
    End Function
End Class
