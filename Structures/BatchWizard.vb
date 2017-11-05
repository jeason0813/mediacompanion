
Public Structure str_BatchWizard
    Dim title As Boolean
    Dim votes As Boolean
    Dim rating As Boolean
    Dim top250 As Boolean
    Dim runtime As Boolean
    Dim director As Boolean
    Dim stars As Boolean
    Dim year As Boolean
    Dim outline As Boolean
    Dim plot As Boolean
    Dim tagline As Boolean
    Dim genre As Boolean
    Dim studio As Boolean
    Dim premiered As Boolean
    Dim mpaa As Boolean
    Dim trailer As Boolean
    Dim credits As Boolean
    Dim posterurls As Boolean
    Dim country As Boolean
    Dim actors As Boolean
    Dim mediatags As Boolean
    Dim missingposters As Boolean
    Dim missingfanart As Boolean
    Dim activate As Boolean
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        title = False
        votes = False
        rating = False
        top250 = False
        runtime = False
        director = False
        stars = False
        year = False
        outline = False
        plot = False
        tagline = False
        genre = False
        studio = False
        premiered = False
        mpaa = False
        trailer = False
        credits = False
        posterurls = False
        country = False
        actors = False
        mediatags = False
        missingposters = False
        missingfanart = False
        activate = False
    End Sub
End Structure
