Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Text
Imports Media_Companion.WorkingWithNfoFiles
Imports System.Xml
Imports Media_Companion
Imports Media_Companion.Pref
Imports System.Linq

Partial Public Class Form1
  
    Sub Form1_KeyPress(ByVal sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.T AndAlso e.Control Then


            Dim frm As New frmTestForm

            frm.TmdbSetManager1.Init
            frm.TmdbSetManager1.MoviesLst = oMovies

            If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
                UpdateFilteredList
            End If
        End If

    End Sub

End Class
