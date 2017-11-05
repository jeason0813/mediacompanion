Public Class VideoPathEventArgs : Inherits BaseEventArgs

    Public Property McMoviePath   As String

    Sub New(Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
        MyBase.New(priority)
    End Sub


    Sub New(ByVal videoPath As String, Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
        Me.New(priority)
        Me.McMoviePath   = videoPath
    End Sub

    Overrides Function ToString As String
        Return Me.McMoviePath
    End Function

End Class
