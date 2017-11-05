Imports ProtoXML

Public Class AudioDetails
    Inherits ProtoPropertyGroup



    Public Property Codec As New ProtoProperty(Me, "codec")
    Public Property Language As New ProtoProperty(Me, "language")
    Public Property Channels As New ProtoProperty(Me, "channels")
    Public Property Bitrate As New ProtoProperty(Me, "bitrate")

    Public Sub New()
        MyBase.New(Nothing, Nothing)
        'Throw New NotImplementedException()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.ProtoXChildBase
        Return New AudioDetails
    End Function
End Class
