Imports ProtoXML

Namespace Tvdb
    Public Class BannerList
        Inherits ProtoFlatList(Of Banner)

        Public ReadOnly Property SeasonMax As Integer
            Get
                Dim Max As Integer = Integer.MinValue
                For Each Item As Banner In Me.List
                    If Item.Season.Value IsNot Nothing Then
                        If Item.Season.Value > Max Then
                            Max = Item.Season.Value
                        End If
                    End If
                Next
                Return Max
            End Get
        End Property

        Private Sub New()
            MyBase.New(Nothing, Nothing)
            Throw New NotImplementedException()
        End Sub

        Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
            MyBase.New(Parent, NodeName)
        End Sub


        Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)

            Dim NewActor As New Banner()
            NewActor.ParentClass = Me.ParentClass
            NewActor.ProcessNode(Element)

            Me.Add(NewActor)

        End Sub

        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New BannerList
        End Function
    End Class
End Namespace