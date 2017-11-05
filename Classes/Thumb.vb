Public Class FrodoPosterThumb

    Property Aspect As String
    Property Url    As String

    Sub New
    End Sub

    Sub New(aspect As String, url As String)
        Me.Aspect = aspect
        Me.Url    = url
    End Sub

End Class
