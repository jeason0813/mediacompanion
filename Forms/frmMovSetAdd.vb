Imports System.ComponentModel

Public Class frmMovSetAdd
    Public newset As String = ""

    Private Sub tbMovSetAdd_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tbMovSetAdd.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            btnMovSetAdd.PerformClick()
            e.Handled = True
        End If
    End Sub

    Private Sub btnMovSetAdd_Click(sender As Object, e As EventArgs) Handles btnMovSetAdd.Click
        Try
            If tbMovSetAdd.Text <> "" Then
                Dim ex As Boolean = False
                For Each mset In Pref.moviesets
                    If mset.ToLower = tbMovSetAdd.Text.ToLower Then
                        ex = True
                        Exit For
                    End If
                Next
                If ex = False Then
                    newset = tbMovSetAdd.text
                    'Pref.moviesets.Add(tbMovSetAdd.Text)
                    'Pref.moviesets.Sort()
                    tbMovSetAdd.Clear()
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MsgBox("This Movie Set Already Exists")
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
            Me.Close()
        End Try
    End Sub

    Private Sub frmMovSetAdd_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = DialogResult.Cancel
            Me.Close()
        End If
    End Sub
End Class