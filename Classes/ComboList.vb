'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports XBMC.JsonRpc
Imports System.Text.RegularExpressions
Imports System.Linq

Public Class ComboList

    Property fullpathandfilename  As String = "" 
    'Property MovieSet             As String = ""
    Property filename             As String = ""
    Property foldername           As String = ""
    Property _title               As String = ""
    Property _originaltitle       As String = ""
    Property year                 As Integer= 0
    Property filedate             As String = ""
    Property id                   As String = ""
    Property tmdbid               As String = ""
    Property rating               As Double = 0
    Property top250               As String = 0
    Property genre                As String = ""
    Property countries            As String = ""
    
    Private _setName              As String = "-None-"
    Private _setId                As String = ""


    Public Property SetName As String
        Get
            Return _setName 
        End Get
        Set
            If Not Locked("set") Then
                _setName = value
            End If
        End Set
    End Property
    
    Public Property TmdbSetId As String
        Get
            Return _setId
        End Get
        Set
            If Not Locked("set") Then
                _setId = value
            End If
        End Set
    End Property
        
    Public ReadOnly Property countriesList As List(Of String)
        Get
            Dim splist() As String = countries.Split(" / ")
            Dim retlist As New List(Of String)
            For Each t In splist
                retlist.Add(t.Trim)
            Next
            Return retlist
            'Return countries.Split(", ").ToList
        End Get
    End Property

    Property studios As String

    Public ReadOnly Property studioslist As List(Of String)
        Get
            Dim splist() As String = studios.Split("/")
            Dim retlist As New List(Of String)
            For Each t In splist
                retlist.Add(t.Trim)
            Next
            Return retlist
        End Get
    End Property

    Property movietag As New List(Of String)
    Property playcount            As String = ""
    Property lastplayed           As String = ""
    Property sortorder            As String = ""
    Property outline              As String = ""
    Property tagline              As String = ""
    Property runtime              As String = ""
    Property createdate           As String = ""
    Property missingdata1         As Byte   = 0
    Property plot                 As String = ""
    Property source               As String = ""
    Property director             As String = ""
    Property credits              As String = ""
    Property Votes                As Integer= 0
    Property Resolution           As Integer= -1
    Property VideoCodec           As String = ""
    Property Audio                As New List(Of AudioDetails)
    Property Premiered            As String = ""
    Property Certificate          As String = ""
    Property XbmcMovie            As XbmcMovieForCompare
    Property FrodoPosterExists    As Boolean
    Property PreFrodoPosterExists As Boolean
    Property Container            As String = ""
    Property VideoMissing         As Boolean = False
    Property SubLang              As New List(Of SubtitleDetails)
    Property stars                As String = ""
    Property Actorlist            As New List(Of str_MovieActors)
    Property DirectorList         As New List(Of DirectorDatabase)
    Property FolderSize           As Long = -1
    Property MediaFileSize        As Long = -1
    Property _rootfolder          As String = ""
    Property _usrrated            As Integer = 0
    Property _metascore           As Integer = 0
    Property NfoPath              As String = ""
    Property NumVideoBits         As Integer = -1   '10+ = HDR

    Property oMovies As Movies

    Public ReadOnly Property IsHdr As Boolean
        Get
            Return (NumVideoBits>8)
        End Get
    End Property


    Public ReadOnly Property DisplayFolderSize As Double
        Get
            Return FormatDisplaySize(FolderSize)
        End Get
    End Property

    Public ReadOnly Property DisplayMediaFileSize As Double
        Get
            Return FormatDisplaySize(MediaFileSize)
        End Get
    End Property

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
    
    Public Property title As String
        Get
            Return _title
        End Get
        Set
            _title = Value.SafeTrim
        End Set
    End Property

    Public Property originaltitle As String
        Get
            Return _originaltitle
        End Get
        Set
            _originaltitle = Value.SafeTrim
        End Set
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

    Public ReadOnly Property ImdbInFolderName As Boolean
        Get
            Return Regex.Match(foldername,"(tt\d{7})").Success
        End Get
    End Property

    Private _intRuntime As Integer = -1
    Private _calculatedRuntime As Boolean = False

    Public ReadOnly Property IntRuntime As Integer
        Get
            If Not _calculatedRuntime Then
                _calculatedRuntime = True
                Try
                    _intRuntime = runtime.Replace(" min","")
                Catch
                    _intRuntime = 0
                End Try
            End If

            Return _intRuntime
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

    Public ReadOnly Property MissingRating As Boolean
        Get
            Return rating=0     '.ToString.Trim=""
        End Get
    End Property
    
    Public ReadOnly Property MissingPremier As Boolean
        Get
            Return Premiered = ""     '.ToString.Trim=""
        End Get
    End Property 

    Public ReadOnly Property MissingCertificate As Boolean
        Get
            Return Certificate.ToString.Trim=""
        End Get
    End Property  

    Public ReadOnly Property OutlineContainsHtml As Boolean
        Get
            Return outline.ContainsHtml
        End Get
    End Property  

    Public ReadOnly Property MissingGenre As Boolean
        Get
            Return genre.ToString.Trim=""
        End Get
    End Property  

    Public ReadOnly Property MissingOutline As Boolean
        Get
            Return outline.ToString.Trim=""
        End Get
    End Property

    Public ReadOnly Property MissingTagLine As Boolean
        Get
            Return tagline.ToString.Trim=""
        End Get
    End Property 

    Public ReadOnly Property MissingIMDBId As Boolean
        Get
            Return id = "0"
        End Get
    End Property

    Public ReadOnly Property MissingSource As Boolean
        Get
            Return source = ""
        End Get
    End Property

    Public ReadOnly Property MissingDirector As Boolean
        Get
            Return director = ""
        End Get
    End Property
    
    Public ReadOnly Property MissingCredits As Boolean
        Get
            Return credits = ""
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

    Public ReadOnly Property MissingRuntime As Boolean
        Get
            Return runtime=""
        End Get
    End Property  

    Public ReadOnly Property MissingVotes As Boolean
        Get
            Return Votes=0          '.ToString.Trim=""
        End Get
    End Property  

    Public ReadOnly Property MissingYear As Boolean
        Get
            Return year < 1901           '.ToString.Trim=""
        End Get
    End Property  

    Public ReadOnly Property MissingPlot As Boolean
        Get
            Return plot.ToString.Trim="" Or plot.ToString.Trim="scraper error"
        End Get
    End Property

    Public ReadOnly Property PlotEqualsOutline As Boolean
        Get
            Return plot.ToString.Trim = outline.trim
        End Get
    End Property

    Public ReadOnly Property MissingTmdbMovieSetInfo As Boolean
        Get
            Return InASet AndAlso MovieSet.MissingInfo
        End Get
    End Property  
    
    Public ReadOnly Property InASet As Boolean
        Get
            Return SetName <> "-None-" 
        End Get
    End Property  
    
    Public ReadOnly Property GotTmdbSetDetail As Boolean
        Get
            Return Not MovieSet.MissingInfo
        End Get
    End Property  
    
    Public ReadOnly Property GotTmdbId As Boolean
        Get
            Return tmdbid<>"" AndAlso Integer.TryParse(tmdbid,Nothing)
        End Get
    End Property  

    Public ReadOnly Property GotTmdbSetId As Boolean
        Get
            Return TmdbSetId<>"" AndAlso Integer.TryParse(TmdbSetId,Nothing)
        End Get
    End Property  
    
    Public ReadOnly Property Watched As Boolean
        Get
            Return playcount<>"0"
        End Get
    End Property  

    Public ReadOnly Property MoviePathAndFileName As String
        Get
            Return Utilities.GetFileName(fullpathandfilename,True)
        End Get
    End Property

    Public ReadOnly Property MissingStars As Boolean
        Get
            Return stars.ToString.Trim=""
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

    'ReadOnly Property FrodoPosterExists As Boolean
    '    Get
    '        Return Pref.FrodoPosterExists(fullpathandfilename)
    '    End Get
    'End Property

    ReadOnly Property ActualNfoFileName As String
        Get
            Return Path.GetFileName(fullpathandfilename)
        End Get
    End Property

    ReadOnly Property PlotEqOutline As Boolean
        Get
            Return (plot=outline)
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
    
    Private _assignedDefaultAudioTrack As Boolean = False
    Private _defaultAudioTrack As New AudioDetails

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
            _usrrated = Value
        End Set
    End Property
    
    Public Property metascore As Integer
        Get
            Return _metascore
        End Get
        Set(value As Integer)
            _metascore = Value
        End Set
    End Property

    Property UserTmdbSetAddition As Char = ""
    Property UnknownSetCount     As Char = ""
    Property LockedFields        As List(Of String) = New List(Of String)
    Property FieldsLockEnabled   As Boolean = True

    Sub New
    End Sub

    Public Sub Assign(From As ComboList)

        Me.FieldsLockEnabled = False

        Me.fullpathandfilename  = From.fullpathandfilename
        'Me.MovieSet             = From.MovieSet           
        Me.filename             = From.filename           
        Me.foldername           = From.foldername         
        Me.title                = From.title
        Me.originaltitle        = From.originaltitle    
        Me.year                 = From.year               
        Me.filedate             = From.filedate           
        Me.id                   = From.id  
        Me.tmdbid               = From.tmdbid        
        Me.rating               = From.rating             
        Me.top250               = From.top250             
        Me.genre                = From.genre  
        Me.countries            = From.countries
        Me.studios              = From.studios 
        Me.movietag             = From.movietag
        Me.playcount            = From.playcount 
        Me.lastplayed           = From.lastplayed  
        Me.sortorder            = From.sortorder          
        Me.outline              = From.outline 
        Me.tagline              = From.tagline   
        Me.runtime              = From.runtime            
        Me.createdate           = From.createdate         
        Me.missingdata1         = From.missingdata1       
        Me.plot                 = From.plot               
        Me.source               = From.source
        Me.director             = From.director
        Me.credits              = From.credits     
        Me.Votes                = From.Votes              
        Me.Resolution           = From.Resolution
        Me.VideoCodec           = From.VideoCodec
        Me.Premiered            = From.Premiered
        Me.Certificate          = From.Certificate
        Me.FrodoPosterExists    = From.FrodoPosterExists
        Me.PreFrodoPosterExists = From.PreFrodoPosterExists

        AssignAudio(From.Audio)
        Me.Container            = From.Container
        Me.VideoMissing         = From.VideoMissing
        AssignSubtitleLang(From.SubLang)

        Me.SetName              = From.SetName
        Me.TmdbSetId            = From.TmdbSetId

        Me.stars                = From.stars
        Me.Actorlist            = From.Actorlist 
        Me.DirectorList         = From.DirectorList 
        Me.FolderSize           = From.FolderSize
        Me.MediaFileSize        = From.MediaFileSize
        Me.rootfolder           = From.rootfolder
        Me.usrrated             = From.usrrated
        Me.metascore            = From.metascore
        Me.UserTmdbSetAddition  = From.UserTmdbSetAddition
        Me.UnknownSetCount      = From.UnknownSetCount
        Me.LockedFields         = From.LockedFields
        Me.NfoPath              = From.NfoPath
        Me.NumVideoBits         = From.NumVideoBits

        Me.FieldsLockEnabled = True
    End Sub

    Public Sub AssignAudio(From As List(Of AudioDetails))
        Me.Audio.Clear
        Me.Audio.AddRange(From)
        Me._assignedDefaultAudioTrack = False
    End Sub

    Public Sub AssignSubtitleLang(From As List(Of SubtitleDetails))
        Me.SubLang.Clear
        Me.SubLang.AddRange(From)
    End Sub
 
    Function Locked(field As String) As Boolean
        Return FieldsLockEnabled AndAlso LockedFields.Contains(field)
    End Function

    Function FormatDisplaySize(size As Long) As Double
        Return Math.Round(size / (1024 * 1024 * 1024), 1)
    End Function
	    
End Class
