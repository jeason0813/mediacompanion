'Imports System.IO
Imports Alphaleonis.Win32.Filesystem

Public Class frmXBMC_Progress

    Property Errors   As String = ""
    Property Warnings As String = ""


    Private Sub panelXBMC_Click( sender As Object,  e As EventArgs) Handles LinkLabel1.Click
        Pref.OpenFileInAppPath(Form1.XBMC_Controller_full_log_file )
        Pref.OpenFileInAppPath(Form1.XBMC_Controller_brief_log_file)
    End Sub



    Private Sub lblQueueCount_MouseHover( sender As Object,  e As EventArgs) Handles lblQueueCount.MouseHover

        Try
            Dim s As String = ""

            If Convert.ToInt32(lblQueueCount.Text)=0 Then 
                s = "No queued events to show"
            Else
                s  = Form1.XbmcControllerQ      .ToString
                s += Form1.XbmcControllerBufferQ.ToString
            End If

            ToolTip1.SetToolTip(lblQueueCount, s) 
        Catch
        End Try
    End Sub

    Private Sub frmXBMC_Progress_FormClosing( sender As Object,  e As FormClosingEventArgs) Handles MyBase.FormClosing
        WindowState = FormWindowState.Minimized
        e.Cancel    = True
    End Sub

    Private Sub frmXBMC_Progress_Shown( sender As Object,  e As EventArgs) Handles MyBase.Shown
        MaximizeBox = False
    End Sub

    Private Sub btnPurgeQ_Click( sender As Object,  e As EventArgs) Handles btnPurgeQ.Click
        Form1.XbmcControllerQ.Write(XbmcController.E.MC_PurgeQ_Req)     
    End Sub


    Public Sub UpdateDetails(oProgress As XBMC_Controller_Progress)

        Dim total As Integer = Form1.Link_TotalQCount

        progressBar1.Maximum = Math.Max(progressBar1.Maximum, total)

        progressBar1.Value   = progressBar1.Maximum - total

        lblProgress    .Text = Replace(oProgress.Action,"&","&&")
        lblQueueCount  .Text = total
        lblErrorCount  .Text = oProgress.ErrorCount
        lblWarningCount.Text = oProgress.WarningCount

        If oProgress.Severity="W" Then Warnings += oProgress.ErrorMsg + Environment.NewLine
        If oProgress.Severity="E" Then Errors   += oProgress.ErrorMsg + Environment.NewLine
    End Sub


    Public Sub Reset
        ProgressBar1.Maximum = 1
        Errors  =""
        Warnings=""
    End Sub



    Private Sub lblErrorCount_MouseHover( sender As Object,  e As EventArgs) Handles lblErrorCount.MouseHover
        Try
            ToolTip1.SetToolTip(lblErrorCount, IIf(Errors="","No errors to show",Errors) )
        Catch
        End Try
    End Sub

    Private Sub lblWarningCount_MouseHover( sender As Object,  e As EventArgs) Handles lblWarningCount.MouseHover
        Try
            ToolTip1.SetToolTip(lblWarningCount, IIf(Warnings="","No warnings to show",Warnings) )
        Catch
        End Try
    End Sub

End Class