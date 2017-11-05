Public Class FolderEventArgs : Inherits BaseEventArgs

    Public Property Folder As String


    Sub New(Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
        MyBase.New(priority)
    End Sub

'    Sub New(ByVal Folder As String, Optional title As String = Nothing,Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
    Sub New(ByVal Folder As String, Optional priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low)
        Me.New(priority)
        Me.Folder = Folder
    End Sub

    Overrides Function ToString As String
        Return Me.Folder
    End Function

End Class
