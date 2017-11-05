Imports ProtoXML

Public Class SubtitleDetails
    Inherits ProtoPropertyGroup


    Public Property Language As New ProtoProperty(Me, "language")

    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.ProtoXChildBase
        Return New SubtitleDetails
    End Function
End Class
