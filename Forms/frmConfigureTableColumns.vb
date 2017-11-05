Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmConfigureTableColumns
    Property FilterSpace     As Integer = 30
    Property tablecolumns As New List(Of String)

    Public Sub Init()
        PopCheckListBox
    End Sub

    Sub PopCheckListBox
        clbColumnsSelect.Items.Clear
        Dim i   As Integer=0
        For Each column In Pref.tableview
            tablecolumns.Add(column)
            Dim lbl As New Label
            Dim tempdata() As String
            tempdata = column.Split("|")
            lbl.text = tempdata(0)
            clbColumnsSelect.Items.Add(lbl.Text)
            clbColumnsSelect.SetItemChecked(i,Boolean.Parse(tempdata(3)))
            If tempdata(0) = "title" or tempdata(0) = "year" Then
                clbColumnsSelect.SetItemCheckState(i, CheckState.Indeterminate)
            End If 
            i += 1
        Next
    End Sub

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click
        ConfigureFilters
    End Sub

    Private Sub ConfigureFilters
        Dim show    As Boolean
        Dim item    As String
        Pref.tableview.Clear()
        For i=0 to clbColumnsSelect.Items.Count-1
            item        = clbColumnsSelect.Items(i)
            show        = clbColumnsSelect.GetItemChecked(i)
            Dim newdata As String = String.Empty
            For Each col In tablecolumns
                Dim tempdata() As String = col.Split("|")
                If tempdata(0) = item Then
                    newdata = String.Format("{0}|{1}|{2}|{3}", tempdata(0), tempdata(1), tempdata(2), show.ToString.ToLower)
                    Exit For
                End If
            Next
            Pref.tableview.Add(newdata)
        Next
    End Sub

    'Set Title and Year as not able to unselect.
    Private Sub clbColumnsSelect_MouseUp(sender As Object, e As MouseEventArgs) Handles clbColumnsSelect.MouseUp
        clbColumnsSelect.SetItemChecked(0, True)
        clbColumnsSelect.SetItemCheckState(0, CheckState.Indeterminate)
        clbColumnsSelect.SetItemChecked(1,True)
        clbColumnsSelect.SetItemCheckState(1, CheckState.Indeterminate)
    End Sub

End Class
