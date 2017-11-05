
Public Class NewVersionCheckResult

    Property ShowNoNewVersionMsgBox As Boolean
    Property NewVersion             As String


    Sub New(ShowNoNewVersionMsgBox As Boolean, NewVersion As String)
        Me.ShowNoNewVersionMsgBox = ShowNoNewVersionMsgBox
        Me.NewVersion             = NewVersion
    End Sub

End Class


