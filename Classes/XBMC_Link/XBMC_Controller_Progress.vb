
Public Class XBMC_Controller_Progress 

    Property LastState    As XbmcController.S
    Property Evt          As XbmcController.E
    Property Args         As EventArgs
    Property Action       As String
    Property CurrentState As XbmcController.S
    Property Severity     As String
    Property ErrorMsg     As String
    Property ErrorCount   As Integer=0
    Property WarningCount As Integer=0

    Sub New()
    End Sub

    Sub New ( 
            LastState    As XbmcController.S,
            Evt          As XbmcController.E,
            Args         As EventArgs,
            Action       As String,
            CurrentState As XbmcController.S,
            Severity     As String,
            ErrorMsg     As String,
            ErrorCount   As Integer,
            WarningCount As Integer
            )

        Me.LastState    = LastState   
        Me.Evt          = Evt   
        Me.Args         = Args   
        Me.Action       = Action      
        Me.CurrentState = CurrentState   
        Me.Severity     = Severity    
        Me.ErrorMsg     = ErrorMsg    
        Me.ErrorCount   = ErrorCount
        Me.WarningCount = WarningCount
    End Sub

End Class


