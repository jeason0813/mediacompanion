Public MustInherit Class TaskBase
    Implements ITask



    Public Property Arguments As New System.Collections.Generic.Dictionary(Of String, Object) Implements ITask.Arguments
    Public Property Dependancies As New System.Collections.Generic.List(Of ITask) Implements ITask.Dependancies
    Public Property Messages As New System.Collections.Generic.List(Of Object) Implements ITask.Messages
    Public Property Options As New System.Collections.Generic.List(Of TaskOption) Implements ITask.Options

    Public Property Id As System.Guid Implements ITask.Id
    Public Property Priority As Long Implements ITask.Priority
    Public Property Attempts As Integer Implements ITask.Attempts
    Public Property State As TaskState Implements ITask.State

    Public Event Completed(ByRef Sender As ITask) Implements ITask.Completed
    Public Event UserInputRequired(ByRef Sender As ITask) Implements ITask.UserInputRequired
    Public Event [Error](ByRef Sender As ITask) Implements ITask.Error

    Public MustOverride ReadOnly Property FriendlyTaskName As String Implements ITask.FriendlyTaskName

    Public Sub New()
        Me.Id = Guid.NewGuid
    End Sub

    Public Overridable Sub Halt() Implements ITask.Halt
        Me.State = TaskState.Fault

        'TODO: Harden this to actually stop the thread, not sure where it'll be needed so leaving it for now.
    End Sub

    Public MustOverride Sub FinishWork() Implements ITask.FinishWork

    Public Overridable Sub Run() Implements ITask.Run
        Me.State = TaskState.Running
    End Sub

    Protected Sub RaiseCompleted()
        Me.State = TaskState.Completed
        RaiseEvent Completed(Me)
    End Sub

    Protected Sub RaiseUserInputRequired()
        Me.State = TaskState.WaitingForUserInput
        RaiseEvent UserInputRequired(Me)
    End Sub

    Protected Sub RaiseError()
        Me.State = TaskState.Fault
        RaiseEvent Error(Me)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.FriendlyTaskName
    End Function

    Public Function GetArgumentSafe(ByVal Attribute As String)
        If Me.Arguments.ContainsKey(Attribute) Then
            Return Me.Arguments(Attribute)
        Else
            Return Nothing
        End If
    End Function

    Public Sub SetArgumentSafe(ByVal Attribute As String, ByRef Value As Object)
        If Me.Arguments.ContainsKey(Attribute) Then
            Me.Arguments(Attribute) = Value
        Else
            Me.Arguments.Add(Attribute, Value)
        End If
    End Sub
End Class
