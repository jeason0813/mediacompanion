
Public Class RetryHandler

    Public Delegate Function ActionDelegate As Boolean

    Private _tries As Integer = 0
    Private _ok    As Boolean = False

    Property MaxRetries      As Integer = 3
    Property InterTryDelayMs As Integer = 500

    Property Action As ActionDelegate

    ReadOnly Property Tries As Integer
        Get
            Return _tries
        End Get
    End Property

    ReadOnly Property Ok As Boolean
        Get
            Return _ok
        End Get
    End Property


    Sub New( ByVal action As ActionDelegate )
        Me.Action = action
    End Sub


    Function Execute As Boolean
        _tries = 0
        _ok    = False

        While _tries<MaxRetries And Not ok
            Try
               _ok = Action.Invoke
                _tries &=1
            Catch
                Threading.Thread.Sleep(InterTryDelayMs)
                _tries &= 1
            End Try
        End While

        Return _ok
    End Function

End Class
