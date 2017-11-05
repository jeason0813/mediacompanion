Public Class BaseEvent

        'Public Property E        As XbmcController.E
        'Public Property Args     As EventArgs
        'Public Property Priority As PriorityQueue.Priorities 

        'Sub New()
        'End Sub

        'Sub New(ByVal e As XbmcController.E, Optional priority As PriorityQueue.Priorities=PriorityQueue.Priorities.low, Optional ByVal args As EventArgs = Nothing)
        '    Me.E        = e
        '    Me.Priority = priority
        '    Me.Args     = args
        'End Sub


        Public Property E        As XbmcController.E
        Public Property Args     As BaseEventArgs
        Public Property Ts       As DateTime
        Public Property Retries  As Integer

        Sub New()
        End Sub

        Sub New(ByVal e As XbmcController.E, Optional ByVal args As BaseEventArgs = Nothing)
            Me.E    = e
            Me.Args = args
            Me.Ts   = DateTime.Now
            Me.Retries = 0
        End Sub

        Sub New(ByVal e As XbmcController.E, ByVal priority As PriorityQueue.Priorities) 
            Me.New( e,New BaseEventArgs(priority) )
        End Sub

        Overrides Function ToString As String
            Return  GetTimeStamp + " " + Args.Priority.ToString + " Retry " + Retries.ToString + " " + E.ToString + " " + Args.ToString
        End Function

        Function Info As String
            Return  " Retry " + Retries.ToString + " " + E.ToString + " " + Args.ToString
        End Function

        Function CompareAs As String
            Return  (E.ToString + " " + Args.CompareAs).Trim
        End Function

        Function GetTimeStamp As String
            Return Format(Ts, "HH:mm:ss.fff").ToString
        End Function

        Sub Assign(ByVal Evt As BaseEvent)
            Me.E       = Evt.E
            Me.Args    = Evt.Args
            Me.Ts      = DateTime.Now
            Me.Retries = Evt.Retries
        End Sub
End Class
