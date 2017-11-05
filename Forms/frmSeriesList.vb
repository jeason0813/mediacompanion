Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmSeriesList
    Public Property Selected As String = ""

    Public Sub Init()
        PopCheckListBox
    End Sub

    Sub PopCheckListBox
        lbx_Series.Items.Clear()
        For Each sh In Cache.TvCache.shows
            If sh.Title.Value <> "" Then lbx_Series.Items.Add(sh.Title.Value)
        Next
    End Sub

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click
        RetrieveSelected
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click( sender As Object,  e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel 
    End Sub

    Private Sub RetrieveSelected
        If lbx_Series.SelectedIndex <> -1 And lbx_Series.SelectedItem <> "" Then
            Selected = lbx_Series.SelectedItem.ToString
        End If
    End Sub

End Class
