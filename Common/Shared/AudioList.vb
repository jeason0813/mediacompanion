Imports ProtoXML

Public Class AudioList
    Inherits ProtoFlatList(Of AudioDetails)

    Private _node_TvShow As XElement


    Private Sub New()
        MyBase.New(Nothing, Nothing)
        Throw New NotImplementedException()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub


    Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)

        Dim NewAudio As New AudioDetails()

        NewAudio.ProcessNode(Element)

        Me.Add(NewAudio)

    End Sub

    Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
        Return New AudioList
    End Function
End Class
