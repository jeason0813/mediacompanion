Imports System.Windows.Forms
Imports System.Threading

Friend NotInheritable Class ExceptionHandler
    Friend Shared Sub LogError(ByVal ex As Exception, Optional paramInfo As String = "")
        If TypeOf (ex) Is UnauthorizedAccessException Then
            Dim ErrorMessage As String
            ErrorMessage = "Unable to write to file.  Please verify file permissions" + vbCrLf + vbCrLf + ex.Message
            ErrorMessage += vbCrLf + vbCrLf + "Now exiting."
            MessageBox.Show(ErrorMessage, "Unable to write Metadata", MessageBoxButtons.OK, MessageBoxIcon.Error)
            For Each frmOpen As Form In My.Application.OpenForms
                frmOpen.Close()
            Next
        Else
            Dim msg As String = ""
            If paramInfo <> "" Then msg = "Called with: [" & paramInfo & "]" & vbCrLf & vbCrLf

            msg &= ex.ToString

            Try
                Dim ofrmExcept As New frmExceptions
                ofrmExcept.Bounds = Screen.PrimaryScreen.Bounds
                ofrmExcept.txtExceptionTrace.Text = msg
                ofrmExcept.TopMost = True
                ofrmExcept.ShowDialog()
            Catch ex1 As Exception
                Dim log As New EventLog
                Dim source As String = "MediaCompanion"
                Try
                    If Not EventLog.SourceExists(source) Then EventLog.CreateEventSource(source, source)
                    log.Source = source
                    log.EnableRaisingEvents = True
                    log.WriteEntry(ex1.ToString, EventLogEntryType.Error)
                    log.WriteEntry(msg, EventLogEntryType.Error)
                Catch ex2 As Exception
                    MsgBox(ex1.ToString & vbCrLf & ex2.ToString)
                    If ex2.ToString.Contains("some or all event logs") Then
                        Dim mymsg As String = ""
                        mymsg +=          "Please find ""mcEventlog.exe"" in"
                        mymsg += vbcrlf & "Media Companion's ""Asset"" folder"
                        mymsg += vbcrlf & "and ""Run As Administrator"" to register"
                        mymsg += vbcrlf & "Media Companion to your EventLog"
                        MsgBox(mymsg)
                    End If
                End Try
            End Try
        End If
    End Sub
End Class
