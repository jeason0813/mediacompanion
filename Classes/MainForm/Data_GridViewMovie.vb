'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions
Imports System.Linq

Public Class Data_GridViewMovie
    Dim _fullpathandfilename    As String = ""
    Property SetName            As String = ""
    Property TmdbSetId          As String = ""
    Dim _filename               As String = ""
    Dim _foldername             As String = ""
    Dim _title                  As String = ""
    Dim _originaltitle          As String = ""
    Dim _titleandyear           As String = ""
    Dim _year                   As String = "0"
    Dim _filedate               As String = ""
    Dim _id                     As String = ""
    Dim _rating                 As Double = 0
    Dim _top250                 As String = 0
    Dim _genre                  As String = ""
    Dim _playcount              As String = ""
    Dim _sortorder              As String = ""
    Dim _outline                As String = ""
    Dim _tagline                As String = ""
    Dim _runtime                As String = ""
    Dim _createdate             As String = ""
    Dim _missingdata1           As Byte = 0
    Dim _plot                   As String = ""
    Dim _source                 As String = ""
    Dim _director               As String = ""
    Dim _votes                  As Integer = 0
    Dim _TitleUcase             As String = ""
    Dim _IntRuntime             As Integer = 0
    Dim _stars                  As String = ""
    Dim _DisplayFileDate        As String
    Dim _DisplayCreateDate      As String
    Dim _movietag               As String = ""
    Dim _container              As String = ""
    Dim _videomissing           As Boolean
    Dim _rootfolder             As String = ""
    Dim _usrrated               As Integer = 0
    Dim _metascore              As Integer = 0
    Dim _nfopath                As String = ""
    

   
    Property VideoCodec             As String = ""
    Property Audio                  As New List(Of AudioDetails)
    Property Premiered              As String
    Property FrodoPosterExists      As Boolean
    Property PreFrodoPosterExists   As Boolean
    Property FolderSize             As Long = -1
    Property MediaFileSize          As Long = -1
    Property tmdbid                 As String = ""
    Property UserSetAddition        As Char = ""
    Property oMovies                As Movies

    Public ReadOnly Property MovieSet As MovieSetInfo 
        Get
            Dim res As MovieSetInfo = Nothing

            If GotTmdbSetId Then
                res = oMovies.FindMovieSetInfoByTmdbSetId(TmdbSetId)
            End If

            If IsNothing(res) Then
                res = oMovies.FindMovieSetInfoBySetDisplayName(SetName)
            End If

            Return res
        End Get

    End Property

    Public ReadOnly Property GotTmdbSetId As Boolean
        Get
            Return TmdbSetId<>"" AndAlso Integer.TryParse(TmdbSetId,Nothing)
        End Get
    End Property  

    Public ReadOnly Property GotTmdbId As Boolean
        Get
            Return tmdbid<>"" AndAlso Integer.TryParse(tmdbid,Nothing)
        End Get
    End Property  


    Property LockedFields As List(Of String) = New List(Of String)

    Sub New
    End Sub

    Sub New(movie As ComboList) 
        Assign(movie)
    End Sub

    Public Sub Assign(movie As ComboList)
        fullpathandfilename     = movie.fullpathandfilename
        SetName                 = movie.SetName
        TmdbSetId               = movie.TmdbSetId
        filename                = movie.filename
        foldername              = movie.foldername
        title                   = movie.title
        originaltitle           = movie.originaltitle
        year                    = movie.year
        filedate                = movie.filedate
        id                      = movie.id
        Rating                  = movie.rating
        top250                  = movie.top250
        genre                   = movie.genre
        countries               = movie.countries
        studios                 = movie.studios
        playcount               = movie.playcount
        SortOrder               = movie.sortorder
        outline                 = movie.outline
        tagline                 = movie.tagline
        runtime                 = movie.runtime
        createdate              = movie.createdate
        missingdata1            = movie.missingdata1
        plot                    = movie.plot.Trim
        source                  = movie.source
        director                = movie.director 
        credits                 = movie.credits
        Votes                   = movie.Votes
        TitleUcase              = movie.title.ToUpper
        Integer.TryParse(runtime.Replace(" min",""),IntRuntime)
        Resolution              = movie.Resolution
        VideoCodec              = movie.VideoCodec
        AssignAudio(movie.Audio)
        Premiered               = movie.Premiered
        Certificate             = movie.Certificate
        FrodoPosterExists       = movie.FrodoPosterExists
        PreFrodoPosterExists    = movie.PreFrodoPosterExists
        movietag                = String.Join(", ", movie.movietag)
        container               = movie.container
        videomissing            = movie.VideoMissing
        stars                   = movie.stars 
        AssignSubtitleLang(movie.SubLang)
        FolderSize              = movie.FolderSize
        MediaFileSize           = movie.MediaFileSize
        DefaultAudioTrack       = movie.DefaultAudioTrack
        rootfolder              = movie.rootfolder
        usrrated                = movie.usrrated
        metascore               = movie.metascore
        tmdbid                  = movie.tmdbid
        UserSetAddition         = movie.UserTmdbSetAddition
        MissingTmdbSetInfo      = movie.UnknownSetCount
        oMovies                 = movie.oMovies
        LockedFields            = movie.LockedFields
        NfoPath                 = movie.NfoPath
        NumVideoBits            = movie.NumVideoBits
    End Sub

    Public Sub AssignAudio(From As List(Of AudioDetails))
        Me.Audio.Clear
        Me.Audio.AddRange(From)
    End Sub

    Public Function Export(oMovies As Movies) As ComboList

        Dim convertedMovie As New ComboList()

        convertedMovie.FieldsLockEnabled = False

        convertedMovie.oMovies              = Me.oMovies
        convertedMovie.fullpathandfilename  = Me.fullpathandfilename
        convertedMovie.SetName              = Me.SetName
        convertedMovie.TmdbSetId            = Me.TmdbSetId
        convertedMovie.filename             = Me.filename
        convertedMovie.foldername           = Me.foldername
        convertedMovie.title                = Me.title
        convertedMovie.originaltitle        = Me.originaltitle
        convertedMovie.year                 = Me.year
        convertedMovie.filedate             = Me.filedate
        convertedMovie.id                   = Me.id
        convertedMovie.rating               = Me.Rating
        convertedMovie.top250               = Me.top250
        convertedMovie.genre                = Me.genre
        convertedMovie.countries            = Me.countries
        convertedMovie.studios              = Me.studios
        convertedMovie.playcount            = Me.playcount
        convertedMovie.sortorder            = Me.SortOrder
        convertedMovie.outline              = Me.outline
        convertedMovie.tagline              = Me.tagline
        convertedMovie.runtime              = Me.runtime
        convertedMovie.createdate           = Me.createdate
        convertedMovie.missingdata1         = Me.missingdata1
        convertedMovie.plot                 = Me.plot.Trim
        convertedMovie.source               = Me.source
        convertedMovie.director             = Me.director
        convertedMovie.credits              = Me.credits
        convertedMovie.Votes                = Me.Votes
        convertedMovie.Resolution           = Me.Resolution
        convertedMovie.VideoCodec           = Me.VideoCodec
        convertedMovie.Audio                = Me.Audio
        convertedMovie.SubLang              = Me.SubLang
        convertedMovie.Premiered            = Me.Premiered
        convertedMovie.Certificate          = Me.Certificate
        convertedMovie.stars                = Me.stars
        convertedMovie.FrodoPosterExists    = Me.FrodoPosterExists
        convertedMovie.PreFrodoPosterExists = Me.PreFrodoPosterExists
        convertedMovie.FolderSize           = Me.FolderSize
        convertedMovie.MediaFileSize        = Me.MediaFileSize
        convertedMovie.DefaultAudioTrack    = Me.DefaultAudioTrack
        convertedMovie.rootfolder           = Me.rootfolder
        convertedMovie.usrrated             = Me.usrrated
        convertedMovie.metascore            = Me.metascore
        convertedMovie.tmdbid               = Me.tmdbid
        convertedMovie.UserTmdbSetAddition  = Me.UserSetAddition
        convertedMovie.UnknownSetCount      = Me.MissingTmdbSetInfo
        convertedMovie.NfoPath              = Me.NfoPath
        convertedMovie.NumVideoBits         = Me.NumVideoBits

        convertedMovie.FieldsLockEnabled = True

        Return convertedMovie
    End Function

    Public Property fullpathandfilename
        Get
            Return _fullpathandfilename
        End Get
        Set(ByVal value)
            _fullpathandfilename = value
        End Set
    End Property

    Public Property filename
        Get
            Return _filename
        End Get
        Set(ByVal value)
            _filename = value
        End Set
    End Property

    Public Property foldername
        Get
            Return _foldername
        End Get
        Set(ByVal value)
            _foldername = value
        End Set
    End Property

    ReadOnly Property DisplayTitle As String
        Get
            Dim t As String = If(IsNothing(title),"Unknown",title)
            Return Pref.RemoveIgnoredArticles(t)
        End Get
    End Property

    ReadOnly Property DisplayTitleAndYear As String
        Get
            Return DisplayTitle & " (" & If(IsNothing(year),"0000",year) & ")"
        End Get
    End Property

    Public Property title
        Get
            Return _title
        End Get
        Set(ByVal value)
            _title = value
        End Set
    End Property

    Public Property originaltitle
        Get
            Return _originaltitle
        End Get
        Set(ByVal value)
            _originaltitle = value
        End Set
    End Property

    Public Property year As String
        Get
            Return _year
        End Get
        Set(ByVal value As String)
            _year = value
        End Set
    End Property

    Public Property filedate
        Get
            Return _filedate
        End Get
        Set(ByVal value)
            _filedate = value
        End Set
    End Property

    Public Property id
        Get
            Return _id
        End Get
        Set(ByVal value)
            _id = value
        End Set
    End Property

    Public Property Rating As Double
        Get
            Return _rating
        End Get
        Set(ByVal value As Double)
            _rating = value
        End Set
    End Property

    Public Property top250
        Get
            Return _top250
        End Get
        Set(ByVal value)
            _top250 = value
        End Set
    End Property

    Public Property countries As String=""
    Public Property credits As String

    Public ReadOnly Property countriesList As List(Of String)
        Get
            Return countries.Replace(" / ", "/").Split("/").ToList
        End Get
    End Property

    Public Property studios As String

    Public ReadOnly Property studiosList As List(Of String)
        Get
            Return studios.Replace(" / ", "/").Split("/").ToList
        End Get
    End Property
    
    Public Property genre
        Get
            Return _genre
        End Get
        Set(ByVal value)
            _genre = value
        End Set
    End Property

    Public Property playcount
        Get
            Return _playcount
        End Get
        Set(ByVal value)
            _playcount = value
        End Set
    End Property

    Public Property SortOrder As String
        Get
            Return _sortorder
        End Get
        Set(ByVal value As String)
            _sortorder = value
        End Set
    End Property

    Public Property outline
        Get
            Return _outline
        End Get
        Set(ByVal value)
            _outline = value
        End Set
    End Property

    Public Property tagline
        Get
            Return _tagline
        End Get
        Set(ByVal value)
            _tagline = value
        End Set
    End Property

    Public Property runtime
        Get
            Return _runtime
        End Get
        Set(ByVal value)
            _runtime = value
        End Set
    End Property

    Public Property stars
        Get
            Return _stars
        End Get
        Set(ByVal value)
            _stars = value
        End Set
    End Property

    Public Property createdate
        Get
            Return _createdate
        End Get
        Set(ByVal value)
            _createdate = value
        End Set
    End Property

    Public Property missingdata1 As Byte
        Get
            Return _missingdata1
        End Get
        Set(ByVal value As Byte)
            _missingdata1 = value
        End Set
    End Property

    Public Property plot
        Get
            Return _plot
        End Get
        Set(ByVal value)
            _plot = value
        End Set
    End Property

    Public Property source
        Get
            Return _source
        End Get
        Set(ByVal value)
            _source = value
        End Set
    End Property

    Public Property director
        Get
            Return _director
        End Get
        Set(ByVal value)
            _director = value
        End Set
    End Property
    
    Public Property Votes As Integer
        Get
            Return _votes
        End Get
        Set(ByVal value As Integer)
            _votes = value
        End Set
    End Property

    Public Property TitleUcase
        Get
            Return _TitleUcase
        End Get
        Set(ByVal value)
            _TitleUcase = value
        End Set
    End Property

    Public Property IntRuntime
        Get
            Return _IntRuntime
        End Get
        Set(ByVal value)
            _IntRuntime = value
        End Set
    End Property

    Public ReadOnly Property DisplayFileDate As String
        Get
            If IsNothing(_DisplayFileDate) Then
                Try
                    _DisplayFileDate = DecodeDateTime(FileDate)
                Catch ex As Exception
                    Try
                        _DisplayFileDate = FileDate.ToString
                    Catch
                        _DisplayFileDate = ""
                    End Try
                End Try
            End If

            Return _DisplayFileDate
        End Get
    End Property

    Public ReadOnly Property DisplayCreateDate As String
        Get
            If IsNothing(_DisplayCreateDate) Then
                Try
                    _DisplayCreateDate = DecodeDateTime(CreateDate)
                Catch ex As Exception
                    Try
                        _DisplayCreateDate = CreateDate.ToString
                    Catch
                        _DisplayCreateDate = ""
                    End Try
                End Try
            End If

            Return _DisplayCreateDate
        End Get
    End Property

    Public ReadOnly Property MissingFanart As Boolean
        Get
            Return _missingdata1 And 1
        End Get
    End Property

    Public ReadOnly Property MissingPoster As Boolean
        Get
            Return _missingdata1 And 2
        End Get
    End Property

    Public ReadOnly Property MissingTrailer As Boolean
        Get
            Return _missingdata1 And 4
        End Get
    End Property

    Public ReadOnly Property MissingLocalActors As Boolean
        Get
            Return _missingdata1 And 8
        End Get
    End Property

    ReadOnly Property DisplaySortOrder As String
        Get
            Dim t As String = If(IsNothing(SortOrder),"Unknown",SortOrder)
            Return Pref.RemoveIgnoredArticles(t)
        End Get
    End Property


    Public Function DecodeDateTime(s As String) As String
        If s = "" Then Return ""
        Dim YYYY As String = s.SubString( 0,4)
        Dim MM   As String = s.SubString( 4,2)
        Dim DD   As String = s.SubString( 6,2)
        Dim HH   As String = s.SubString( 8,2)
        Dim MIN  As String = s.SubString(10,2)
        Dim SS   As String = s.SubString(12,2)

        Dim x As String = Pref.DateFormat

        x = x.Replace("YYYY", YYYY)
        x = x.Replace("MM"  , MM  )
        x = x.Replace("DD"  , DD  )
        x = x.Replace("HH"  , HH  )
        x = x.Replace("MIN" , MIN )
        x = x.Replace("SS"  , SS  )

        Return x
    End Function

    Sub ClearStoredCalculatedFields
        _DisplayFileDate   = Nothing
        _DisplayCreateDate = Nothing
    End Sub


    Public ReadOnly Property MissingRating As Boolean
        Get
            Return Rating=0
        End Get
    End Property  


    Public ReadOnly Property MissingGenre As Boolean
        Get
            Return genre.ToString.Trim=""
        End Get
    End Property  

    Public ReadOnly Property MissingIMDBId As Boolean
        Get
            Return id.ToString = "0"
        End Get
    End Property

    Public ReadOnly Property MissingSource As Boolean
        Get
            If IsNothing(source) Then Return True
            If source.ToString = "" Then Return True
            Return False
        End Get
    End Property

    Public ReadOnly Property MissingDirector As Boolean
        Get
            Return director.ToString = ""
        End Get
    End Property
    
    Public ReadOnly Property MissingCredits As Boolean
        Get
            Return credits.ToString = ""
        End Get
    End Property
    
    Public ReadOnly Property MissingStudios As Boolean
        Get
            Return studios = ""
        End Get
    End Property

    Public ReadOnly Property MissingCountry As Boolean
        Get
            Return countries = ""
        End Get
    End Property


    Public ReadOnly Property MissingCertificate As Boolean
        Get
            Return Certificate.ToString.Trim=""
        End Get
    End Property  

    Public ReadOnly Property OutlineContainsHtml As Boolean
        Get
            Return outline.ToString.ContainsHtml
        End Get
    End Property  

    Public ReadOnly Property MissingOutline As Boolean
        Get
            Return outline.ToString.Trim=""
        End Get
    End Property  
    
    Public ReadOnly Property PlotEqualsOutline As Boolean
        Get
            Return plot.ToString.Trim = outline.ToString.Trim
        End Get
    End Property  

    Public ReadOnly Property MissingTagline As Boolean
        Get
            Return tagline.ToString.Trim=""
        End Get
    End Property

    Public ReadOnly Property MissingRuntime As Boolean
        Get
            Return runtime=""
        End Get
    End Property
    
    Public ReadOnly Property MissingPremier As Boolean
        Get
            Return Premiered = ""
        End Get
    End Property
     

    Public ReadOnly Property MissingStars As Boolean
        Get
            Return stars = ""
        End Get
    End Property

    Public ReadOnly Property MissingVotes As Boolean
        Get
            Return Votes = 0
        End Get
    End Property  


    Public ReadOnly Property MissingYear As Boolean
        Get
            Return year < 1901
        End Get
    End Property  


    Public ReadOnly Property MissingPlot As Boolean
        Get
            Return plot.ToString.Trim="" Or plot.ToString.Trim="scraper error"
        End Get
    End Property  


    Public ReadOnly Property IncompleteMovieSet As Boolean
        Get
            Return InASet AndAlso MovieSet.MissingInfo
        End Get
    End Property  


    Public ReadOnly Property InASet As Boolean
        Get
            'If IsNothing(MovieSet) Then Return False
            'Return MovieSet.MovieSetDisplayName <> "-None-" 
            Return SetName <> "-None-" 
        End Get
    End Property  

    Public ReadOnly Property Watched As Boolean
        Get
            Return playcount<>"0"
        End Get
    End Property  

    Public ReadOnly Property DisplayRating As String
        Get
            Return _rating.ToString("f1")
        End Get
    End Property

    'Public ReadOnly Property DisplayFolderSize As Integer
    '    Get
    '        Return CInt( FolderSize /(1024*1024*1024) )
    '    End Get
    'End Property

    Public ReadOnly Property DisplayFolderSize As Double
        Get
            Return Math.Round( FolderSize/(1024*1024*1024),1 )
        End Get
    End Property

    Public ReadOnly Property MoviePathAndFileName As String
        Get
            Return Utilities.GetFileName(fullpathandfilename,True, container)
        End Get
    End Property  


    ReadOnly Property ActualNfoFileNameMatchesDesired As Boolean
        Get
            Return (ActualNfoFileName=DesiredNfoFileName)
        End Get
    End Property


    '
    ' Returns the Nfo filename based on the user configured rename pattern & name mode (1=include first stack part name e.g CD1)
    '
    ReadOnly Property DesiredNfoFileName As String
        Get
            Dim stackName       = MoviePathAndFileName
            Dim isFirstPart     = True
            Dim stackdesignator = ""
            Dim nextStackPart   = ""
            Dim result          = UserDefinedFileName

            Utilities.isMultiPartMedia(stackName, False, isFirstPart, stackdesignator, nextStackPart)

            If isFirstPart And Pref.namemode="1" Then
                Dim i As Integer  
                result &= stackdesignator & If(Integer.TryParse(nextStackPart, i), "1".PadLeft(nextStackPart.Length, "0"), "A")
            End If

            Return result & ".nfo"
        End Get
    End Property


    ReadOnly Property ActualNfoFileName As String
        Get
            Return Path.GetFileName(fullpathandfilename)
        End Get
    End Property


    ReadOnly Property UserDefinedFileName As String
        Get
            Dim s As String = Pref.MovieRenameTemplate

            s = s.Replace("%T", title)
            s = s.Replace("%Y", year)
            s = s.Replace("%I", id)
            s = s.Replace("%P", Premiered)     
            s = s.Replace("%R", rating)
            s = s.Replace("%L", runtime)
            s = s.Replace("%S", source)
            s = Utilities.cleanFilenameIllegalChars(s)

            Return s
        End Get
    End Property


    Public Property Certificate As String

    Public ReadOnly Property ImdbInFolderName As Boolean
        Get
            Return Regex.Match(foldername,"(tt\d{7})").Success
        End Get
    End Property

    Private _FolderNameYear As Integer = -1

    Public ReadOnly Property FolderNameYear As Integer
        Get
            If _FolderNameYear=-1 Then
                Dim m = Regex.Match(foldername,"(\d{4})")
                If m.Success Then
                    _FolderNameYear = Convert.ToInt32(m.Value)
                Else
                    _FolderNameYear = -2
                End If
            End If
            Return _FolderNameYear
        End Get
    End Property

    Public Property movietag
        Get
            Return _movietag
        End Get
        Set(ByVal value)
            _movietag = value
        End Set
    End Property

    Public Property container
        Get
            Return _container
        End Get
        Set(ByVal value)
            _container = value
        End Set
    End Property

    Public Property videomissing
        Get
            Return _videomissing
        End Get
        Set(ByVal value)
            _videomissing = value
        End Set
    End Property

    Property SubLang As New List(Of SubtitleDetails)

    Public Sub AssignSubtitleLang(From As List(Of SubtitleDetails))
        Me.SubLang.Clear
        Me.SubLang.AddRange(From)
    End Sub

    Private _assignedDefaultAudioTrack As Boolean = False
    Private _defaultAudioTrack As AudioDetails

    Public Property DefaultAudioTrack As AudioDetails
        Get
            If Not _assignedDefaultAudioTrack Then
                _assignedDefaultAudioTrack = True

                If Audio.Count > 0 Then
                    _defaultAudioTrack = (From x In Audio Where x.DefaultTrack.Value="Yes").FirstOrDefault

                    If IsNothing(_defaultAudioTrack) Then
                        _defaultAudioTrack = Audio(0)
                    End If
                End If
            End If

            Return _defaultAudioTrack
        End Get

        Set
            _assignedDefaultAudioTrack = True
            _defaultAudioTrack = Value
        End Set
    End Property    

    Public ReadOnly Property Languages As List(Of String)
        Get
            Dim result = (From x In Audio Select x.Language.Value).ToList

            Return result
        End Get
    End Property

    Property Resolution As Integer = -1

    Public Property rootfolder As String
        Get
            Return _rootfolder
        End Get
        Set(value As String)
            _rootfolder = value
        End Set
    End Property

    Public Property usrrated As Integer
        Get
            Return _usrrated
        End Get
        Set(value As Integer)
            _usrrated = value 
        End Set
    End Property

    Public Property metascore As Integer
        Get
            Return _metascore
        End Get
        Set(value As Integer)
            _metascore = value 
        End Set
    End Property

    'Public ReadOnly Property MovieSetDisplayName As String
    '    Get
    '        Dim res = "-None-"
    '        Try
    '            res = movieset.MovieSetDisplayName
    '        Catch ex As Exception
    '        End Try
    '        Return res
    '    End Get
    'End Property

    Public Property MissingTmdbSetInfo As Char = ""

    Public ReadOnly Property MissingMovie As Boolean
        Get
            Return videomissing
        End Get
    End Property

    Public Property NfoPath As String
        Get
            Return _nfopath
        End Get
        Set(value As String)
            _nfopath = value
        End Set
    End Property

    Property NumVideoBits As Integer = -1   '10+ = HDR

    Public ReadOnly Property IsHdr As Boolean
        Get
            Return (NumVideoBits>8)
        End Get
    End Property

End Class
