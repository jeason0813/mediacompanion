
Public Class Progress

    Enum Commands 
        SetIt
        Append
    End Enum

    Property Command As Commands = Commands.Append
    Property Message As String
    Property Log     As String

    Sub New( Optional message As String="", Optional log as String="", Optional command As Commands=Commands.Append )
        Me.Message = message
        Me.Log     = log
        Me.Command = command
    End Sub

End Class


