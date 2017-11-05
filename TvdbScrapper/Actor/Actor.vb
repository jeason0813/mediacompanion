Imports ProtoXML
Namespace Tvdb
    Public Class Actor
        Inherits ProtoPropertyGroup

        Public Property Id As New ProtoProperty(Me, "id")
        Public Property Image As New ProtoProperty(Me, "Image")
        Public Property Name As New ProtoProperty(Me, "Name")
        Public Property Role As New ProtoProperty(Me, "Role")
        Public Property SortOrder As New ProtoProperty(Me, "SortOrder")

        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New Actor()
        End Function
    End Class

End Namespace