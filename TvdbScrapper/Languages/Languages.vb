Imports ProtoXML

Namespace Tvdb
    Public Class Languages
        Inherits ProtoFile

        Public Sub New()
            MyBase.New("Languages")
        End Sub

        Public Property Languages As New LanguageList(Me, "Language")

    End Class
End Namespace