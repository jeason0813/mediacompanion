Public Class UnknownEventArgs : Inherits BaseEventArgs

    Public Property EventName As String

    Sub New(Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
        MyBase.New(priority)
    End Sub

    Sub New(ByVal EventName As String,Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
        Me.New(priority)
        Me.EventName = EventName
    End Sub

    Overrides Function ToString As String
        Return Me.EventName
    End Function

End Class
