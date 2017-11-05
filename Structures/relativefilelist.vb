Public Structure str_RelativeFileList
    Dim mc As String
    Dim xbmc As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        mc = ""
        xbmc = ""
    End Sub
End Structure