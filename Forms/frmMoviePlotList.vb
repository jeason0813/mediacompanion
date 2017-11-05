Imports System.IO
Imports System.Text

Public Class frmmovieplotlist
    Public ListOfPlots As New List(Of String)

    Private Sub frmplotlist(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Pref.MultiMonitoEnabled Then
            Me.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
            Me.Width = 861
            Me.Height = 580
        End If
    End Sub

    Public Sub New(ByVal displayplots As List(Of String))

        InitializeComponent()
        Try
            ListOfPlots.AddRange(displayplots)
            ShowLog()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TextBox1_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.GotFocus
        Try
            TextBox1.Select(TextBox1.Text.Length, 0) 'this removes the whole text being selected when the form is created
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub ShowLog()
        Dim count As Integer = ListOfPlots.Count
        For x = 1 to ListOfPlots.Count
            cmbxplotnumber.Items.Add(x.ToString)
        Next
        Label1.Text = count.ToString & "  Plot(s) to select from.  Click OK to save to Movie."
        cmbxplotnumber.SelectedIndex = 0
        cmbxplotnumber.Select()
    End Sub

    Private Sub cmbxplotnumber_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbxplotnumber.SelectedIndexChanged
        TextBox1.Text = ListOfPlots(cmbxplotnumber.SelectedIndex)
        cmbxplotnumber.Focus()
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub frmoutputlog_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown 

        If               e.KeyCode = Keys.Escape Then Me.Close() 
    End Sub

End Class