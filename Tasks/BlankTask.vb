Namespace Tasks
    Public Class BlankTask
        Inherits TaskBase

        Public Overrides Sub Run()
            MyBase.Run()

            Dim Random As New Random
            Dim Input As Double = Random.NextDouble
            Dim WaitTime As Integer = Input * 10000
            Threading.Thread.Sleep(WaitTime)

            Me.State = TaskState.BackgroundWorkComplete
        End Sub

        Public Overrides Sub FinishWork()
            Me.State = TaskState.Completed
        End Sub

        Public Overrides ReadOnly Property FriendlyTaskName As String
            Get
                Return "Blank Task for Testing"
            End Get
        End Property
    End Class
End Namespace