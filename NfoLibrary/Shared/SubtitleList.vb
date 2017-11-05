Imports ProtoXML

Public Class SubtitleList
    Inherits ProtoFlatList(Of SubtitleDetails)

    Private _node_TvShow As XElement


    Private Sub New()
        MyBase.New(Nothing, Nothing)
        Throw New NotImplementedException()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub


    Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)

        Dim NewSubtitle As New SubtitleDetails()
        NewSubtitle.ParentClass = Me.ParentClass
        NewSubtitle.ProcessNode(Element)

        Me.Add(NewSubtitle)

    End Sub
End Class
