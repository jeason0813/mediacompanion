Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmConfigureMovieFilters

    Property FilterSpace     As Integer = 30
    Property FilterContainer As Panel
    Dim chked As Boolean = True

    Public Sub Init(container As Panel)
        FilterContainer = container
        PopCheckListBox
    End Sub


    Sub PopCheckListBox
        clbMovieFilters.Items.Clear

        Dim lbl As Label
        Dim i   As Integer=0
        Dim query = From c As Control In FilterContainer.Controls Where c.Name.IndexOf("cbFilter")=0 Order by Convert.ToInt16(c.Tag.ToString)

        For Each c As Control In query

            lbl = FilterContainer.Controls("lbl"+ c.Name.SubString(2,c.Name.Length-2))

            clbMovieFilters.Items.Add(lbl.Text)
            clbMovieFilters.SetItemChecked(i,c.Visible)
            i += 1
        Next
    End Sub

    Private Sub btnchkunchk_Click( sender As Object,  e As EventArgs) Handles Label1.Click
        Try
            chked = Not chked
            For i=clbMovieFilters.Items.Count-1 To 0 Step -1
                If clbMovieFilters.Items(i) <> "General" Then
                    clbMovieFilters.SetItemChecked(i, chked)
                End If
            Next
        Catch

        End Try
    End Sub

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click
        ConfigureFilters
    End Sub


    Private Sub ConfigureFilters
        Dim cb      As Control
        Dim lbl     As Label
        Dim lblMode As Label
        Dim show    As Boolean
        Dim item    As String

        For i=clbMovieFilters.Items.Count-1 To 0 Step -1

            item        = clbMovieFilters.Items(i)

            lbl         = (From c As Control In FilterContainer.Controls Where c.Text=item)(0)

            cb          = FilterContainer.Controls("cb" + lbl.Name.SubString(3,lbl.Name.Length-3) )

            lblMode     = FilterContainer.Controls(lbl.Name+"Mode")

            show        = clbMovieFilters.GetItemChecked(i)

            cb .Tag     = i
            cb .Visible = show
            lbl.Visible = show

            If Not IsNothing(lblMode) Then
                lblMode.Visible = show
            End If

        Next
    End Sub



#Region "Drag'n Drop items"
    Private Swap_Drag_Index As Integer

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Clear DragDrop index
        Swap_Drag_Index = -1
    End Sub


    Private Sub clbMovieFilters_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles clbMovieFilters.MouseDown
        Dim tempIndex As Integer

        ' Start Swap operation if Left mouse button is down
        If e.Button = Windows.Forms.MouseButtons.Left Then

            ' Get the index of the item the mouse is hovering above
            tempIndex = clbMovieFilters.IndexFromPoint(e.X, e.Y)
            If (tempIndex <> ListBox.NoMatches) Then
                ' Index exists in the list
                Swap_Drag_Index = tempIndex
            End If

        End If
    End Sub


    Private Sub clbMovieFilters_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles clbMovieFilters.MouseUp
        ' Clear Swap operation index
        Swap_Drag_Index = -1
    End Sub


    Private Sub clbMovieFilters_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles clbMovieFilters.MouseMove
        Dim tempIndex As Integer
        Dim item As Object

        If Swap_Drag_Index <> ListBox.NoMatches Then
            ' Check position of mouse
            tempIndex = clbMovieFilters.IndexFromPoint(e.X, e.Y)

            If tempIndex <> ListBox.NoMatches And tempIndex <> Swap_Drag_Index Then
                ' Store the Dragged Item
                item = clbMovieFilters.Items.Item(Swap_Drag_Index)
                ' Remove the Dragged Item
                clbMovieFilters.Items.RemoveAt(Swap_Drag_Index)
                ' Re-Insert the Dragged Item
                clbMovieFilters.Items.Insert(tempIndex, item)
                ' Reset Drag Index
                Swap_Drag_Index = tempIndex
                ' Select Dragged Index
                clbMovieFilters.SelectedIndex = Swap_Drag_Index
            End If
        End If
    End Sub
#End Region 'Drag'n Drop items

End Class
