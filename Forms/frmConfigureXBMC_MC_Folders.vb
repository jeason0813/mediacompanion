Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmConfigureXBMC_MC_Folders
   
    Property        FolderMappings As XBMC_MC_FolderMappings = New XBMC_MC_FolderMappings
    Private  _passedFolderMappings As XBMC_MC_FolderMappings

    Private  BS As BindingSource = New BindingSource


    Public Sub Init(folderMappings As XBMC_MC_FolderMappings)

        Me._passedFolderMappings = folderMappings
        Me.FolderMappings.Assign(folderMappings)
        Me.FolderMappings.IniFolders

        BindSource
        FormatTable
    End Sub


    Public Sub BindSource

        Me.BS.DataSource = Me.FolderMappings.Items.ToList
        dgv.DataSource = Me.BS

        Dim field = dgv.Columns(0)

        field.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        field.ReadOnly     = True

        field = dgv.Columns(1)
        field.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    Private Sub FormatTable
        Dim header_style As New DataGridViewCellStyle

        header_style.ForeColor = Color.White
        header_style.BackColor = Color.DarkGreen
        header_style.Font      = new Font(dgv.Font, FontStyle.Bold)

        For Each col As DataGridViewcolumn in dgv.Columns
            col.HeaderCell.Style = header_style
        Next

        dgv.EnableHeadersVisualStyles = False
        dgv.AllowUserToResizeRows     = False
    End Sub

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click
        _passedFolderMappings.Assign(Me.FolderMappings)
    End Sub

    Private Sub btnSame_Click( sender As Object,  e As EventArgs) Handles btnSame.Click
        Me.FolderMappings.SetSame
        BindSource
    End Sub

    Private Sub btnClear_Click( sender As Object,  e As EventArgs) Handles btnClear.Click
        Me.FolderMappings.ClearXBMC
        BindSource
    End Sub
End Class
