Module TaskingSupport
    Public Sub ForegroundWorkPumper(ByVal Sender As Object, ByVal E As System.EventArgs)
        If Common.Tasks.Count > 0 Then
            Try
                For Each Task In Common.Tasks
                    If Task.State = TaskState.BackgroundWorkComplete Then
                        Task.FinishWork()
                    End If
                    System.Windows.Forms.Application.DoEvents()
                Next
            Catch
                System.Windows.Forms.Application.DoEvents()
            End Try
        End If

        Form1.RefreshTaskList()
    End Sub
End Module
