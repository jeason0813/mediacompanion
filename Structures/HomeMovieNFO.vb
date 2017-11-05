Public Structure HomeMovieNFO
    Dim title As String
    Dim sortorder As String
    Dim movieset As String
    Dim year As String
    Dim plot As String
    Dim runtime As String
    Dim stars As String
    Dim genre As String
    Dim playcount As String
    Dim filename As String
    Dim fanart As String
    Sub New(ByVal SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        title = ""
        sortorder = ""
        movieset = ""
        year = ""
        plot = ""
        runtime = ""
        stars = ""
        genre = ""
        playcount = ""
        filename = ""
        fanart = ""
    End Sub
End Structure
