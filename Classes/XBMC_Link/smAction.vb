
Imports Sanford.StateMachineToolkit

Public Class smAction
    Public Delegate Sub ActionDelegate(sender As Object, args As TransitionEventArgs(Of XbmcController.S, XbmcController.E, EventArgs))

    Property Action     As ActionDelegate
    Property ActionName As String
    Property Time       As New Times

    Sub New(ByVal action As ActionDelegate, actionName As String)
        Me.Action     = action
        Me.ActionName = actionName
    End Sub

    Sub Run(sender As Object, args As TransitionEventArgs(Of XbmcController.S, XbmcController.E, EventArgs))
        Time.StartTm = DateTime.Now
        _action(sender, args)
        Time.EndTm   = DateTime.Now
    End Sub

End Class
