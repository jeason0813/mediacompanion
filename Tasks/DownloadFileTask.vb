Imports System.Net
Imports System.IO
Imports Media_Companion
Namespace Tasks
    Public Class DownloadFileTask
        Inherits TaskBase



        Private _FriendlyName As String
        Public Overrides ReadOnly Property FriendlyTaskName As String
            Get
                Return Me.State & " - " & _FriendlyName
            End Get
        End Property
        Public Sub New(Optional FriendlyName As String = "Download File to Disk")
            Me.Id = Guid.NewGuid
            _FriendlyName = FriendlyName
        End Sub

        Public Sub New(ByVal URL As String, ByVal Path As String, Optional FriendlyName As String = "Download File to Disk")
            Me.Id = Guid.NewGuid
            _FriendlyName = FriendlyName

            If String.IsNullOrEmpty(URL) Then
                Me.Messages.Add(New ArgumentException("URL is not set/valid"))
                Me.RaiseError() 'Can event handlers register before a constructor is completed?
            Else
                If Me.Arguments.ContainsKey("url") Then
                    Me.Arguments("url") = URL
                Else
                    Me.Arguments.Add("url", URL)
                End If
            End If

            If String.IsNullOrEmpty(Path) Then
                Me.Messages.Add(New ArgumentException("Path is not set/valid"))
                Me.RaiseError() 'Can event handlers register before a constructor is completed?
            Else
                If Me.Arguments.ContainsKey("path") Then
                    Me.Arguments("path") = Path
                Else
                    Me.Arguments.Add("path", Path)
                End If
            End If
        End Sub

        Public Overrides Sub Run()
            If Me.State = TaskState.Completed Then Exit Sub

            For Each Item In Dependancies
                If Not Item.State = TaskState.Completed Then
                    Me.State = TaskState.WaitingForDependancies
                    Exit Sub
                End If
            Next
            Me.State = TaskState.Running
            Me.Messages.Clear()

            Dim URL As String = Arguments("url")
            Dim Path As String = Arguments("path")

            If String.IsNullOrEmpty(URL) Then
                Me.Messages.Add(New ArgumentException("URL is not set/valid"))
                Me.State = TaskState.Fault
                Me.RaiseError()
            End If

            If String.IsNullOrEmpty(Path) Then
                Me.Messages.Add(New ArgumentException("Path is not set/valid"))
                Me.State = TaskState.Fault
                Me.RaiseError()
            End If

            If File.Exists(Path) Then
                If Me.Arguments.ContainsKey("overwrite") AndAlso Me.Arguments("overwrite") = True Then
                    Try
                        File.Delete(Path)
                    Catch
                        Me.Messages.Add(New Exception("File already exists, can't be deleted"))
                        Me.State = TaskState.Fault
                        Me.RaiseError()
                    End Try
                End If
            End If

            If Me.State = TaskState.Fault Then Exit Sub

            Try

            Catch ex As Exception
                Me.Messages.Add(ex)
                Me.State = TaskState.Fault
                Me.RaiseError()
            End Try

            Me.State = TaskState.BackgroundWorkComplete
        End Sub


        Public Overrides Function ToString() As String
            Return Me.FriendlyTaskName
        End Function

        Public Overrides Sub FinishWork()
            Me.RaiseCompleted()
            Me.State = TaskState.Completed
        End Sub
    End Class
End Namespace