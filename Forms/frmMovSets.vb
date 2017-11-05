Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Media_Companion

Public Class frmMovSets
    Public Collection As List(Of MovieSetDatabase)
    Public CollectionTitle As String = ""

    Public Sub Init()
        lbl_CollectionTitle.Text = CollectionTitle
        dgvLoad()
    End Sub

    Public Sub dgvLoad()
        dgvmovset.Rows.Clear()
        For each item In Collection
            Dim row As DataGridViewRow = DirectCast(dgvmovset.RowTemplate.Clone(), DataGridViewRow)
            row.CreateCells(dgvmovset, If(item.present, Global.Media_Companion.My.Resources.Resources.correct, Global.Media_Companion.My.Resources.Resources.incorrect), item.title)
            dgvmovset.Rows.Add(row)
        Next
    End Sub

    Private Sub frmMovSets_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub frmMovSets_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        dgvmovset.Rows.Clear()
        dgvmovset.Dispose()
    End Sub

    Private Sub frmMovSets_Load(sender As Object, e As EventArgs) Handles Me.Load
        dgvmovset.ClearSelection()
    End Sub
End Class