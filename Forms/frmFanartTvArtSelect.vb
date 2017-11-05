Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmFanartTvArtSelect
    Property FilterSpace     As Integer = 6
    Property chkbxlst As New List(Of Boolean)
    Dim i As Integer = 0
    'Dim _selectgenres As New List (Of String)
    Public Property IsChanged As Boolean = False
    
    Public Sub Init()
        PopCheckListBox
    End Sub

    Sub PopCheckListBox
        clbFanartTvArtSelect.Items.Clear
        'Dim i   As Integer=0
        Dim lbl As New Label
        lbl.Text = "Clear Art"
        clbFanartTvArtSelect.Items.Add(lbl.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlClearArt)
        chkbxlst.Add(Pref.MovFanartTvDlClearArt)
        i +=1

        Dim lbl2 As New Label
        lbl2.Text = "Clear Logo"
        clbFanartTvArtSelect.Items.Add(lbl2.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlClearLogo)
        chkbxlst.Add(Pref.MovFanartTvDlClearLogo)
        i +=1

        Dim lbl3 As New Label
        lbl3.Text = "Poster"
        clbFanartTvArtSelect.Items.Add(lbl3.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlPoster)
        chkbxlst.Add(Pref.MovFanartTvDlPoster)
        i +=1

        Dim lbl4 As New Label
        lbl4.Text = "Fanart"
        clbFanartTvArtSelect.Items.Add(lbl4.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlFanart)
        chkbxlst.Add(Pref.MovFanartTvDlFanart)
        i +=1

        Dim lbl5 As New Label
        lbl5.Text = "Disc"
        clbFanartTvArtSelect.Items.Add(lbl5.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlDisc)
        chkbxlst.Add(Pref.MovFanartTvDlDisc)
        i +=1

        Dim lbl6 As New Label
        lbl6.Text = "Banner"
        clbFanartTvArtSelect.Items.Add(lbl6.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlBanner)
        chkbxlst.Add(Pref.MovFanartTvDlBanner)
        i +=1

        Dim lbl7 As New Label
        lbl7.Text = "Landscape"
        clbFanartTvArtSelect.Items.Add(lbl7.Text)
        clbFanartTvArtSelect.SetItemChecked(i, Pref.MovFanartTvDlLandscape)
        chkbxlst.Add(Pref.MovFanartTvDlLandscape)

    End Sub

    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnDone.Click
        RetrieveSelected
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click( sender As Object,  e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel 
    End Sub

    Private Sub RetrieveSelected
        Dim show As Boolean
        Dim item As String
        For h = 0 to clbFanartTvArtSelect.Items.Count-1
            item = clbFanartTvArtSelect.Items(h)
            show = clbFanartTvArtSelect.GetItemChecked(h)
            Select Case item
                Case "Clear Art"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlClearArt = show
                    End If
                Case "Clear Logo"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlClearLogo = show
                    End If
                Case "Poster"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlPoster = show
                    End If
                Case "Fanart"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlFanart = show
                    End If
                Case "Disc"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlDisc = show
                    End If
                Case "Banner"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlBanner = show
                    End If
                Case "Landscape"
                    If chkbxlst.Item(h) <> show Then
                        IsChanged = True
                        Pref.MovFanartTvDlLandscape = show
                    End If
            End Select
        Next
    End Sub
End Class
