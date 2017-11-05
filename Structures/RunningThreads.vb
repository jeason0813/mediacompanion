
Public Structure str_RunningThreads
    Dim thread1 As Boolean
    Dim thread2 As Boolean
    Dim thread3 As Boolean
    Dim thread4 As Boolean
    Dim thread5 As Boolean
    Dim thread6 As Boolean
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        thread1 = False
        thread2 = False
        thread3 = False
        thread4 = False
        thread5 = False
        thread6 = False
    End Sub
End Structure
