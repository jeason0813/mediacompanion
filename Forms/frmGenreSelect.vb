Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmGenreSelect
    Property FilterSpace     As Integer = 30
    Public Property multicount As Integer = 0
    Property chkbxlst As New List(Of String)
    Dim _selectgenres As New List (Of str_genre)

    Public Property SelectedGenres As List(Of str_genre)
        Get
            Return _selectgenres 
        End Get
        Set
            _selectgenres.Clear
            If Value.Count > 0 Then
                _selectgenres = Value
            End If
        End Set
    End Property

    Public Sub Init()
        PopCheckListBox
    End Sub

    Sub PopCheckListBox
        clbGenreSelect.Items.Clear
        Dim chkd As Boolean
        For Each genre In Form1.Genrelist
            chkd = False
            chkbxlst.Add(genre)
            Dim lbl As New Label
            lbl.text = genre
            clbGenreSelect.Items.Add(lbl.Text)
        Next
        chkstate
    End Sub

    Sub chkstate
        For each g In _selectgenres
            Dim indx As Integer = Nothing
            indx = clbGenreSelect.Items.IndexOf(g.genre)
            Dim isstate As checkstate
            If g.count = 0 Then
                isstate = CheckState.Unchecked
            ElseIf g.count = multicount  Then
                isstate = CheckState.Checked
            Else
                isstate = CheckState.Indeterminate 
            End If
            clbGenreSelect.SetItemCheckState(indx, isstate)
            clbGenreSelect.Refresh()
        Next
    End Sub

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click
        RetrieveSelected
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click( sender As Object,  e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel 
    End Sub

    Private Sub RetrieveSelected
        Dim listof As New List(Of str_genre)
        listof.Clear()
        For i = 0 to clbGenreSelect.Items.Count-1
            Dim toAdd As Boolean = False
            Dim show As checkstate
            Dim g As New str_genre 
            g.genre = clbGenreSelect.Items(i)
            For each item In _selectgenres
                If item.genre.ToLower = g.genre.ToLower Then
                    toAdd = True
                    Exit For
                End If
            Next
            show = clbGenreSelect.GetItemCheckState(i)
            If show = CheckState.Indeterminate Then
                g.count = 1
            ElseIf show = CheckState.Checked Then
                g.count = 2
            End If
            If ToAdd OrElse g.count > 0 Then listof.Add(g)
        Next
        SelectedGenres = listof
    End Sub

End Class
