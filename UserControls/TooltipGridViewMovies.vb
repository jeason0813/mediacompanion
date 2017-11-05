Public Class TooltipGridViewMovies


    Public Sub Initialisation()
        Me.Height = 300
        Me.Width = 250
    End Sub

    Public Sub Textinfo(ByVal text As String)
        TextBoxMovie.Text = text
    End Sub

    Public Sub TextMovieName(ByVal text As String)
        LabelMovieName.Text = text
    End Sub

    Public Sub TextLabelMovieYear(ByVal text As String)
        LabelMovieYear.Text = text
    End Sub

    Public Sub TextLabelRatingRuntime(ByVal text As String)
        LabelRatingRuntime.Text = text
    End Sub

End Class
