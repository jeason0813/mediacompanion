Imports ProtoXML
Namespace Tvdb
    Public Class EpisodeList
        Inherits ProtoXML.ProtoFlatList(Of Episode)

        Private Sub New()
            MyBase.New(Nothing, Nothing)
            Throw New NotImplementedException()
        End Sub

        Public Sub New(ByRef Parent As ProtoXML.IProtoXBase, ByVal NodeName As String)
            MyBase.New(Parent, NodeName)
        End Sub


        Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)

            Dim NewActor As New Episode
            NewActor.ParentClass = Me.ParentClass

            NewActor.ProcessNode(Element)

            Me.Add(NewActor)

        End Sub

        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New EpisodeList
        End Function
    End Class
End Namespace