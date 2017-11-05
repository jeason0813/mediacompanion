Imports ProtoXML

Namespace Tvdb
    Public Class Language
        Inherits ProtoPropertyGroup

        Public Property Language As New ProtoProperty(Me, "name")
        Public Property Abbreviation As New ProtoProperty(Me, "abbreviation")
        Public Property Id As New ProtoProperty(Me, "id")

        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New Language
        End Function
    End Class
End Namespace