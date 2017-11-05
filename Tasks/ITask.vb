Public Interface ITask
    Property Id As Guid
    ReadOnly Property FriendlyTaskName As String
    Property State As TaskState

    Property Priority As Long 'Higher means sooner

    Property Attempts As Integer

    Property Arguments As Dictionary(Of String, Object)
    Property Dependancies As List(Of ITask)
    Property Options As List(Of TaskOption)
    Property Messages As List(Of Object)

    Sub Run()

    Sub FinishWork()

    Sub Halt()

    Event Completed(ByRef Sender As ITask)
    Event UserInputRequired(ByRef Sender As ITask)
    Event [Error](ByRef Sender As ITask)
End Interface

Public Enum TaskState
    CriticalFault = -100
    Fault = -10

    NotStarted = 0
    Running = 20
    Halted = 30
    WaitingForDependancies = 40
    WaitingForUserInput = 50

    BackgroundWorkComplete = 90
    Completed = 100
End Enum

Public Class TaskOption
    Public Name As String
    Public Description As String
    Public Value As String
    Public Selected As Boolean
End Class


