
Public Structure str_TvPosterSeason
    Dim posterurl As String
    Dim posterseason As String
    Dim postertype As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        posterurl = ""
        posterseason = ""
        postertype = ""
    End Sub
End Structure
