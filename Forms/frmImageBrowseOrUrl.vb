Public Class frmImageBrowseOrUrl

    Public Property Cancelled As Boolean
    Public Property Tbstring As String = ""

    Private Sub LoadfrmImageBrowseOrUrl(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            'Me.Cursor = Cursors.WaitCursor
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        tb_PathorUrl.Focus()
    End Sub

    Private Sub btn_Browse_Click( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles btn_Browse.Click
        Try
            'Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently()
            'browse
            'Form1.openFD.InitialDirectory = WorkingTvShow.NfoFilePath.Replace(Path.GetFileName(WorkingTvShow.NfoFilePath), "")
            Dim newFD As New OpenFileDialog ()
            newFD.Title = "Select a jpeg image File"
            newFD.FileName = ""
            newFD.Filter = "Media Companion Image Files|*.jpg;*.tbn;*.png;*.bmp|All Files|*.*"
            newFD.FilterIndex = 0
            If newFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Tbstring = newFD.FileName.ToString
                tb_PathorUrl.Text = Tbstring 
            End if
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btn_paste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_paste.Click
        'Get the data stored in the clipboard
        Dim iData As IDataObject = Clipboard.GetDataObject()
        'Check to see if the data is in a text format
        If iData.GetDataPresent(DataFormats.Text) Then
            'If it's text, then paste it into the textbox
            tb_PathorUrl.SelectedText = CType(iData.GetData(DataFormats.Text), String)
        Else
            'If it's not text, print a warning message
            MsgBox("Data in the clipboard is not availble for entry into a textbox")
        End If
    End Sub

    Private Sub btn_SetThumb_Click( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles btn_SetThumb.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btn_Cancel_Click( ByVal sender As System.Object,  ByVal e As System.EventArgs) Handles btn_Cancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmMessageBox_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If  e.KeyCode=Keys.Escape Then
            btn_Cancel.PerformClick()
        End If
    End Sub

End Class