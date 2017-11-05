Imports System.ComponentModel

Public Class NotifyingList(Of T)
    Implements IList(Of T), INotifyPropertyChanged

    Dim List As New List(Of T)

    Public Property PropertyName As String

    Public Sub New(ByVal PropertyName As String)
        Me.PropertyName = PropertyName
    End Sub

    Public Sub Add(item As T) Implements System.Collections.Generic.ICollection(Of T).Add
        List.Add(item)
        NotifyPropertyChanged(Me.PropertyName)
    End Sub

    Public Sub Clear() Implements System.Collections.Generic.ICollection(Of T).Clear
        List.Clear()
        NotifyPropertyChanged(Me.PropertyName)
    End Sub

    Public Function Contains(item As T) As Boolean Implements System.Collections.Generic.ICollection(Of T).Contains
        Return List.Contains(item)
    End Function

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of T).CopyTo
        List.CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count As Integer Implements System.Collections.Generic.ICollection(Of T).Count
        Get
            Return List.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements System.Collections.Generic.ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function Remove(item As T) As Boolean Implements System.Collections.Generic.ICollection(Of T).Remove
        Dim RemoveTest As Boolean = List.Remove(item)
        If RemoveTest Then
            NotifyPropertyChanged(Me.PropertyName)
        End If
        Return RemoveTest
    End Function

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of T) Implements System.Collections.Generic.IEnumerable(Of T).GetEnumerator
        Return List.GetEnumerator
    End Function

    Public Function IndexOf(item As T) As Integer Implements System.Collections.Generic.IList(Of T).IndexOf
        Return List.IndexOf(item)
    End Function

    Public Sub Insert(index As Integer, item As T) Implements System.Collections.Generic.IList(Of T).Insert
        List.Insert(index, item)

        NotifyPropertyChanged(Me.PropertyName)
    End Sub

    Default Public Property Item(index As Integer) As T Implements System.Collections.Generic.IList(Of T).Item
        Get
            Return List.Item(index)
        End Get
        Set(value As T)
            List(index) = value
            NotifyPropertyChanged(Me.PropertyName)
        End Set
    End Property

    Public Sub RemoveAt(index As Integer) Implements System.Collections.Generic.IList(Of T).RemoveAt
        List.RemoveAt(index)
        NotifyPropertyChanged(Me.PropertyName)
    End Sub

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return List.GetEnumerator()
    End Function

    Public Function GetSortedShow() As List(Of T)
        List.Sort(New TvShowComparer)
        Return List
    End Function

    Public Function GetSortedEpisodes() As List(Of T)
        List.Sort(New TVSeasonEpisodeComparer)
        Return List
    End Function

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

End Class

Public Class TvShowComparer
    Implements IComparer(Of TvShow)

    Public Function Compare1(show1 As TvShow, show2 As TvShow) _
                        As Integer Implements System.Collections.Generic.IComparer(Of TvShow).Compare
        Return show1.TitleAndYear.CompareTo(show2.TitleAndYear)
    End Function
End Class

Public Class TVSeasonEpisodeComparer
    Implements IComparer(Of String)
    Private Shared Function ParseSeasonEpisode(ByVal item As String, ByVal type As Boolean) As Integer
        Dim hyphenIndex As Integer = item.IndexOf("-")
        ' Normally do some error checking in case hyphenIndex==-1
        Dim firstPart As String
        If type = True Then
            firstPart = item.Substring(0, hyphenIndex)
        Else
            firstPart = item.Substring(hyphenIndex + 1)
        End If
        Return Integer.Parse(firstPart)
    End Function

    Public Function Compare(ByVal first As String, ByVal second As String) _
                As Integer Implements IComparer(Of String).Compare
        ' In real code you would probably add nullity checks
        Dim firstSeason As Integer = ParseSeasonEpisode(first, True)
        Dim secondSeason As Integer = ParseSeasonEpisode(second, True)
        Dim sameSeason As Integer = firstSeason.CompareTo(secondSeason)
        If sameSeason = 0 Then
            Dim firstEpisode As Integer = ParseSeasonEpisode(first, False)
            Dim secondEpisode As Integer = ParseSeasonEpisode(second, False)
            sameSeason = firstEpisode.CompareTo(secondEpisode)
        End If
        Return sameSeason
    End Function
End Class
