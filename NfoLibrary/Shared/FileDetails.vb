Imports ProtoXML


Public Class FileInfo
    Inherits ProtoPropertyGroup

    Public Property StreamDetails As New StreamDetails(Me, "streamdetails")
    Public Sub New()
        MyBase.New(Nothing, Nothing)
        'Throw New NotImplementedException()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.ProtoXChildBase
        Return New FileInfo
    End Function
End Class
