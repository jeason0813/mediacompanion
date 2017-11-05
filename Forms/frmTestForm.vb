Public Class frmTestForm

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click

        TmdbSetManager1.ApplyUpdates
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click( sender As Object,  e As EventArgs) Handles btnCancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

End Class