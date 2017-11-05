
Public Structure str_ListOfCommands
    Dim title As String
    Dim command As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        title = ""
        command = ""
    End Sub
End Structure