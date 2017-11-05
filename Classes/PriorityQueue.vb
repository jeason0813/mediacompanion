'imports System
Imports System.Collections.Concurrent
'Imports System.Collections.Generic
'Imports System.Diagnostics.Contracts
'Imports System.Threading
'Imports System.Threading.Tasks
Imports System.Linq
    
Public Class PriorityQueue

    Public Enum Priorities
        critical
        high 
        medium
        low
        lowest
    End Enum

    Private lowest   As BlockingCollection(Of BaseEvent) = new BlockingCollection(Of BaseEvent)
    Private low      As BlockingCollection(Of BaseEvent) = new BlockingCollection(Of BaseEvent)
    Private medium   As BlockingCollection(Of BaseEvent) = new BlockingCollection(Of BaseEvent)
    Private high     As BlockingCollection(Of BaseEvent) = new BlockingCollection(Of BaseEvent)
    Private critical As BlockingCollection(Of BaseEvent) = new BlockingCollection(Of BaseEvent)

    Private queues  As BlockingCollection(Of BaseEvent)()   = new BlockingCollection(Of BaseEvent)() { critical, high, medium, low, lowest }

    Public ReadOnly Property Count As Integer
        Get
            Return lowest  .Count + 
                   low     .Count + 
                   medium  .Count + 
                   high    .Count + 
                   critical.Count
        End Get
    End Property


    Public Sub Write(ByVal e As XbmcController.E, Optional ByVal priority As PriorityQueue.Priorities = PriorityQueue.Priorities.low) 

        Write(New BaseEvent(e,priority) )
    End Sub

    Public Sub Write(evt As BaseEvent) 

        Select evt.Args.Priority
            Case Priorities.lowest   : lowest  .Add(evt)
            Case Priorities.low      : low     .Add(evt)
            Case Priorities.medium   : medium  .Add(evt)
            Case Priorities.high     : high    .Add(evt)
            Case Priorities.critical : critical.Add(evt)
        End Select
    End Sub

    Public Function Exists(evt As BaseEvent) As Boolean

        Dim q = From x In View Where x.CompareAs = evt.CompareAs

        Return q.Count>0
    End Function


    Public Function Read As BaseEvent
        Dim evt As BaseEvent = Nothing

        BlockingCollection(Of BaseEvent).TakeFromAny(queues, evt)

        Return evt
    End Function

    Public Function Peek As BaseEvent

        If critical.Count>0 Then Return critical.First
        If high    .Count>0 Then Return high    .First
        If medium  .Count>0 Then Return medium  .First
        If low     .Count>0 Then Return low     .First
        If lowest  .Count>0 Then Return low     .First

        Return Nothing
    End Function

    Public Sub Clear
        While Count>0
            Read
        End While
    End Sub

    Public Function View As List(Of BaseEvent)

        Dim v = (from q in queues
            from element in q
            select element)

        Return v.ToList

    End Function

    Public Sub Delete(e As XbmcController.E)

        For Each q In queues
            For Each item In q.ToList
                If item.E = e Then
                    q.TryTake(item)
                End If
            Next
        Next

    End Sub

    Public Sub Delete(evt As BaseEvent)

        For Each q In queues
            For Each item In q.ToList
                If item.E = evt.E And item.Args.ToString.Contains(evt.Args.ToString) Then
                    q.TryTake(item)
                End If
            Next
        Next

    End Sub

    Public Overrides Function ToString() As String

        Dim s As String = ""

        For Each item In View
            s += item.ToString + Environment.NewLine
        Next

        Return s
    End Function

End Class


