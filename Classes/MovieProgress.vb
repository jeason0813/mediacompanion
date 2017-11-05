
Public Class MovieProgress

    Enum MsgType 
        GotFoldersCount
        DoneSome
    End Enum

    Property ProgressEvent As MsgType
    Property Data          As Object

    Sub New
    End Sub

    Sub New(ProgressEvent As MsgType, Data As Object)
        Me.ProgressEvent = ProgressEvent
        Me.Data          = Data
    End Sub

    Function Clone
        Return MemberwiseClone
    End Function

End Class


