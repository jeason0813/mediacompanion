Imports System.Threading

Public Class frmExceptions
    Dim continueMC As Boolean = False
    Private Sub btnQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuit.Click
        For Each frmOpen As Form In My.Application.OpenForms
            frmOpen.Close() 'Close form instead of existing application so background threads are properly terminated.
        Next
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim thCopy As New Thread(AddressOf CopyText)
        thCopy.SetApartmentState(ApartmentState.STA)
        thCopy.Start()
    End Sub

    Private Sub lnkTicket_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkTicket.LinkClicked
        Dim aok As Boolean = True
        Try
            If Pref.selectedBrowser <> "" then
                Try
                    Process.Start(Pref.selectedBrowser,lnkTicket.Text)
                Catch
                    aok = False
                End Try
            End If
            If Pref.selectedBrowser = "" Or Not aok Then
                Try
                    Process.Start(lnkTicket.Text)
                Catch ex As Exception
                    MessageBox.Show( "An error occurred while trying to launch the default browser - Using the 'Locate browser' button under 'General Preferences' to select the browser should resolve this error", "", MessageBoxButtons.OK )
                End Try
            End If 
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub frmExceptions_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If continueMC = False Then
            If (e.CloseReason = CloseReason.UserClosing) Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub CopyText()
        Clipboard.SetText(Me.txtExceptionTrace.Text)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        continueMC = True
        Me.Close()
    End Sub
End Class