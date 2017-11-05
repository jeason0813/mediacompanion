Imports System.Threading
Imports System.ComponentModel

Public Class Common
    Public Shared Tasks As New TaskCache
End Class

Public Class TaskCache
    Implements IList(Of ITask), System.ComponentModel.INotifyPropertyChanged, IListSource

    Private Tasks As New List(Of ITask)

    Public MainThread As New Thread(New ThreadStart(AddressOf RunTasks))
    Public Threads As New List(Of Threading.Thread)
    Public Property MaxThreads As Integer = 16

    Public Property Done As Boolean

    Public Sub StartTaskEngine()
        MainThread.Start()
    End Sub

    Private Sub RunTasks()
        Do Until Done
            Try 'This Try block lets me be really lazy and ignore the fact that if a new task is created it breaks the enumeration and starts the whole process
                For Each Task In Tasks
                    If Task.State = TaskState.NotStarted Then
                        If Not Threads.Count > MaxThreads Then
                            Dim NewThread As Thread = New Thread(New ThreadStart(AddressOf Task.Run))

                            Threads.Add(NewThread)

                            NewThread.Start()
                            Me.NotifyPropertyChanged("UnrunTaskCount")
                            Me.NotifyPropertyChanged("UserWaitTaskCount")
                        End If
                    End If
                Next

                Dim I As Integer = 0
                Do
                    If I > Threads.Count - 1 Then Exit Do

                    If Threads(I).ThreadState = ThreadState.Stopped Then
                        Threads.Remove(Threads(I))
                        Continue Do
                        Me.NotifyPropertyChanged("CompletedTaskCount")
                    End If
                    I += 1
                Loop
            Catch

            Finally
                Thread.Sleep(1)
            End Try

        Loop
    End Sub

    Public Sub Add(item As ITask) Implements System.Collections.Generic.ICollection(Of ITask).Add
        Tasks.Add(item)
    End Sub

    Public Sub Clear() Implements System.Collections.Generic.ICollection(Of ITask).Clear
        Tasks.Clear()
        Threads.Clear()
    End Sub

    Public Function Contains(item As ITask) As Boolean Implements System.Collections.Generic.ICollection(Of ITask).Contains
        Return Tasks.Contains(item)
    End Function

    Public Sub CopyTo(array() As ITask, arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of ITask).CopyTo
        Tasks.CopyTo(array)
    End Sub

    Public ReadOnly Property Count As Integer Implements System.Collections.Generic.ICollection(Of ITask).Count
        Get
            Return Tasks.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements System.Collections.Generic.ICollection(Of ITask).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function Remove(item As ITask) As Boolean Implements System.Collections.Generic.ICollection(Of ITask).Remove
        Return Tasks.Remove(item)
    End Function

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of ITask) Implements System.Collections.Generic.IEnumerable(Of ITask).GetEnumerator
        Return Tasks.GetEnumerator
    End Function

    Public Function IndexOf(item As ITask) As Integer Implements System.Collections.Generic.IList(Of ITask).IndexOf
        Return Tasks.IndexOf(item)
    End Function

    Public Sub Insert(index As Integer, item As ITask) Implements System.Collections.Generic.IList(Of ITask).Insert
        Tasks.Insert(index, item)
    End Sub

    Default Public Property Item(index As Integer) As ITask Implements System.Collections.Generic.IList(Of ITask).Item
        Get
            Return Tasks.Item(index)
        End Get
        Set(value As ITask)
            Tasks.Item(index) = value
        End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements System.Collections.Generic.IList(Of ITask).RemoveAt
        Tasks.RemoveAt(index)
    End Sub

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Me.GetEnumerator
    End Function

    Public ReadOnly Property UnrunTaskCount
        Get
            Dim Count = (From Task As ITask In Tasks Where Task.State = TaskState.NotStarted Or Task.State = TaskState.WaitingForDependancies).Count
            Return Count
        End Get
    End Property

    Public ReadOnly Property UserWaitTaskCount
        Get
            Dim Count = (From Task As ITask In Tasks Where Task.State = TaskState.WaitingForUserInput).Count
            Return Count
        End Get
    End Property

    Public ReadOnly Property CompletedTaskCount
        Get
            Dim Count = (From Task As ITask In Tasks Where Task.State = TaskState.Completed Or Task.State = TaskState.CriticalFault Or Task.State = TaskState.Fault).Count
            Return Count
        End Get
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public ReadOnly Property ContainsListCollection As Boolean Implements System.ComponentModel.IListSource.ContainsListCollection
        Get
            If Me.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public Function GetList() As System.Collections.IList Implements System.ComponentModel.IListSource.GetList
        Return Me.Tasks
    End Function
End Class

'Private Sub cmdTasks_Refresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdTasks_Refresh.Click
'    lstTasks.Items.Clear()
'    tv_MissingArtDownload(tv_ShowSelectedCurrently)

'    For Each Item As ITask In TaskCache.Tasks
'        lstTasks.Items.Add(Item)
'    Next
'End Sub

'Private Sub lstTasks_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstTasks.SelectedIndexChanged
'    If lstTasks.SelectedItem Is Nothing Then Exit Sub

'    Dim SelectedTask As ITask

'    SelectedTask = lstTasks.SelectedItem

'    Select Case SelectedTask.State
'        Case TaskState.Completed
'            lblTask_State.Text = "Completed"
'        Case TaskState.BackgroundWorkComplete
'            lblTask_State.Text = "Background Completed"
'        Case TaskState.CriticalFault
'            lblTask_State.Text = "Critial Fault"
'        Case TaskState.Fault
'            lblTask_State.Text = "Fault"
'        Case TaskState.Halted
'            lblTask_State.Text = "Halted"
'        Case TaskState.NotStarted
'            lblTask_State.Text = "Not Started"
'        Case TaskState.WaitingForUserInput
'            lblTask_State.Text = "Waiting For Input"
'        Case TaskState.Running
'            lblTask_State.Text = "Running"
'    End Select

'    lblTask_Attempts.Text = SelectedTask.Attempts

'    cmbTasks_Arguments.Items.Clear()
'    cmbTasks_Arguments.Text = ""
'    For Each Item In SelectedTask.Arguments
'        cmbTasks_Arguments.Items.Add(Item)

'    Next
'    If cmbTasks_Arguments.Items.Count > 0 Then cmbTasks_Arguments.SelectedIndex = 0

'    lstTasks_Dependancies.Items.Clear()
'    For Each Item In SelectedTask.Dependancies
'        lstTasks_Dependancies.Items.Add(Item)
'    Next

'    lstTasks_Messages.Items.Clear()
'    For Each Item In SelectedTask.Messages
'        lstTasks_Messages.Items.Add(Item)
'    Next
'End Sub

'Private Sub lstTasks_Messages_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstTasks_Messages.SelectedIndexChanged
'    If lstTasks_Messages.SelectedItem Is Nothing Then Exit Sub

'    txtTasks_SelectedMessage.Text = lstTasks_Messages.SelectedItem
'End Sub

'Private Sub cmbTasks_Arguments_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTasks_Arguments.SelectedIndexChanged
'    If cmbTasks_Arguments.SelectedItem Is Nothing Then Exit Sub

'    pnlTasks_Arguments.Controls.Clear()

'    Dim Item As KeyValuePair(Of String, Object) = cmbTasks_Arguments.SelectedItem
'    If TypeOf Item.Value Is String Then
'        pnlTasks_Arguments.Controls.Add(New TextBox() With {.Text = Item.Value, .Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Both})
'    ElseIf TypeOf Item.Value Is TvShow Then
'        Dim TempShow As TvShow = Item.Value
'        pnlTasks_Arguments.Controls.Add(New TextBox() With {.Text = TempShow.Id.Value & " - " & TempShow.Title.Value, .Dock = DockStyle.Fill, .ScrollBars = ScrollBars.Both})
'    ElseIf TypeOf Item.Value Is TvEpisode Then
'        Dim TempEpisode As TvEpisode = Item.Value
'        pnlTasks_Arguments.Controls.Add(New TextBox() With {.Text = TempEpisode.Id.Value & " - " & TempEpisode.Title.Value, .Dock = DockStyle.Fill, .ScrollBars = ScrollBars.Both})
'    ElseIf TypeOf Item.Value Is Image Then
'        pnlTasks_Arguments.Controls.Add(New PictureBox() With {.Image = Item.Value, .Dock = DockStyle.Fill})
'    End If
'End Sub


'Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
'    Try
'        For Each Task In TaskCache.Tasks
'            If Task.State = TaskState.BackgroundWorkComplete Then
'                Task.FinishWork()
'            End If
'            Windows.Forms.Application.DoEvents()
'        Next
'    Catch

'    End Try
'End Sub

'Private Sub ContextMenuStrip1_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

'End Sub

'Private Sub TaskListUpdater_Tick(sender As System.Object, e As System.EventArgs) Handles TaskListUpdater.Tick
'    'cmdTasks_Refresh_Click(Nothing, Nothing)
'    TaskListUpdater.Enabled = False
'    lstTasks.ResetText()

'End Sub