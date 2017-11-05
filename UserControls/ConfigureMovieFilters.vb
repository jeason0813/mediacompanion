Imports System.Linq
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class ConfigureMovieFilters

    Class MovieFilter
        Public Show       As Boolean
        Public FilterName As String

        Sub New( Show As Boolean, FilterName As String )
            Me.Show       = Show
            Me.FilterName = FilterName
        End Sub
    End Class

    Property FilterSpace     As Integer = 30
    Property FilterContainer As Panel

    Private MoviesFilters    As New List(Of MovieFilter)


    Public Sub Display(container As Panel)
        FilterContainer = container
        PopDataSource
        lbMovieFilters.DataSource = MoviesFilters
    End Sub


    Sub PopDataSource
        MoviesFilters.Clear

        Dim lbl As Label

        Dim query = From c As Control In FilterContainer.Controls Where c.Name.IndexOf("cbFilter")=0

        For Each cb As ComboBox In query

            lbl = FilterContainer.Controls("lbl"+ cb.Name.SubString(2,cb.Name.Length-2))
              
            MoviesFilters.Add( new MovieFilter(cb.Visible, lbl.Text ) )
        Next
    End Sub



    Private Sub btnDone_Click( sender As Object,  e As EventArgs) Handles btnApply.Click
        Dim cb    As ComboBox
        Dim lbl   As Label
        Dim index As Integer=0

        For Each row In MoviesFilters

            cb  = FilterContainer.Controls("cb" + row.FilterName )
            lbl = FilterContainer.Controls("lbl"+ row.FilterName )

            cb .Visible = row.Show
            lbl.Visible = row.Show

            If row.Show Then 
                cb .Height = index*FilterSpace
                lbl.Height = index*FilterSpace
            End If
        Next
        
    End Sub

End Class
