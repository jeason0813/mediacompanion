Public Structure str_PossibleShowList
    Dim showtitle   As String
    Dim showid      As String
    Dim showbanner  As String
    Dim langcode    As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        showtitle   = ""
        showid      = ""
        showbanner  = ""
        langcode    = ""
    End Sub
End Structure
