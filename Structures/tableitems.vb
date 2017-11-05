Public Structure str_TableItems
    Dim title As String
    Dim width As Integer
    Dim index As Integer
    Dim visible As Boolean
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        title = ""
        width = 0
        index = 0
        visible = False
    End Sub

End Structure