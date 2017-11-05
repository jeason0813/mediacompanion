Public Class ValueDescriptionPair

    Public Value As Object
    Public description As String

    Public Sub New(ByVal newValue As Object, ByVal newDescription As String)
        Value = newValue
        description = newDescription
    End Sub

    Public Overrides Function ToString() As String
        Return description
    End Function
End Class
