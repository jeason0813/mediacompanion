
Public Structure str_ComboList
    Dim fullpathandfilename As String
    Dim movieset As String
    Dim filename As String
    Dim foldername As String
    Dim title As String
    Dim originaltitle As String
    Dim titleandyear As String
    Dim year As String
    Dim filedate As String
    Dim id As String
    Dim rating As String
    Dim top250 As String
    Dim genre As String
    Dim playcount As String
    Dim sortorder As String
    Dim outline As String
    Dim runtime As String
    Dim createdate As String
    Dim missingdata1 As Byte
    Dim plot As String
    Dim source As String
    Dim votes As String

    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        fullpathandfilename = ""
        movieset = ""
        filename = ""
        foldername = ""
        title = ""
        originaltitle = ""
        titleandyear = ""
        year = ""
        filedate = ""
        id = ""
        rating = ""
        top250 = ""
        genre = ""
        playcount = ""
        sortorder = ""
        outline = ""
        runtime = ""
        createdate = ""
        missingdata1 = 0
        plot = ""
        source = ""
        votes = 0
    End Sub

End Structure
