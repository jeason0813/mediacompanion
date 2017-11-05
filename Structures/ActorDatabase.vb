
Public Structure str_ActorDatabase
    Dim actorname As String
    Dim movieid As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        actorname = ""
        movieid = ""
    End Sub
End Structure
