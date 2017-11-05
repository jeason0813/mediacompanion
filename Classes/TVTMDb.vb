Imports System.Linq
Imports System.Collections.Generic
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem

Public Class TVTMDb

    Public Key          = Utilities.TMDBAPI '"3f026194412846e530a208cf8a39e9cb"
    Public Const TMDB_EXC_MSG = "TMDb is unavailable!"

    Public Shared LanguagesFile               = Pref.applicationPath & "\classes\tmdb_languages.xml"
    Public Const  TMDbConfigImagesBaseUrlFile = "tmdb_config_images_base_url.txt"
    Public Const  TMDbConfigFileMaxAgeInDays  = 14

    Enum Resolution
        FullHD=1920
        HD=1280
    End Enum

    #Region "Private Properties"

    Private _languages          As List(Of String) = New List(Of String)
    Private _lookupLanguages    As List(Of String) = New List(Of String)
    Private _tmdbtranslations   As New WatTmdb.V3.TmdbTranslations
    Private _tvdbid             As String = ""
    Private _imdbid             As String = ""
    Private _tmdbid             As String = ""

    #End Region

    #Region "Read-write Properties"

    Public Property TmdbId As String
        Get
            Return _tmdbid
        End Get
        Set(value As String)
            If _tmdbid <> value Then
                _tmdbid = value
                _tmdbtranslations = GetTvTranslations()
                _fetched = False
            End If
        End Set
    End Property
    
    Public Property ImdbId As String
        Get
            Return _imdbid
        End Get
        Set(ByVal value As String)
            If _imdbid <> value Then
                _imdbid = value
                _fetched = False
            End If
        End Set
    End Property
    
    Public Property TvdbId As String
        Get
            Return _tvdbid 
        End Get
        Set(ByVal value As String)
            If _tvdbid <> value Then
                _tvdbid = value
                _fetched = False
            End If
        End Set
    End Property
    
    Public Property Languages As List(Of String)
        Get
            Return _languages
        End Get 
        Set
            _languages.Clear
            _languages.AddRange(Value)

            _lookupLanguages.Clear
            For Each language In _languages
                _lookupLanguages.Add( language.ToLower.Replace("no language","xx").Replace("language not set","?") )
            Next
        End Set
    End Property

    Public Property TvResults       As New List(Of WatTmdb.V3.TvResult)
    Public Property ValidBackDrops  As New List(Of WatTmdb.V3.Backdrop)
    Public Property ValidPosters    As New List(Of WatTmdb.V3.Poster  )
    Public Property ValidKeyWords   As WatTmdb.V3.TmdbTvKeywords
    Public Property MaxGenres       As Integer = Media_Companion.Pref.maxmoviegenre

    #End Region 'Read-write properties

    #Region "Read-only Properties"

    Private _api                    As WatTmdb.V3.Tmdb
    Private _config_images_base_url As String
    Private _tv                     As New WatTmdb.V3.TmdbTv
    Private _tvfind                 As New WatTmdb.V3.TmdbFind

    Private _tvImages               As WatTmdb.V3.TmdbTvImages
    Private _releases               As WatTmdb.V3.TmdbTvReleases
    Private _genrelist              As WatTmdb.V3.TmdbGenre 
    Private _mcPosters              As New List(Of McImage)
    Private _mcFanart               As New List(Of McImage)
    Private _thumbs                 As New List(Of String)
    Private _keywords               As New List(Of String)
    Private _cast                   As WatTmdb.V3.TmdbTvCredits
    Private _director               As String
    Private _frodoPosterThumbs      As New List(Of FrodoPosterThumb)
    Private _frodoFanartThumbs      As New FrodoFanartThumbs
    Private _fetched                As Boolean = False
    

 
    Shared Public ReadOnly Property LanguageCodes As List(Of String)
        Get
            If Media_Companion.Pref.TMDbUseCustomLanguage and Media_Companion.Pref.TMDbCustomLanguageValue<>"" then        
                Return Media_Companion.Pref.TMDbCustomLanguageValue.Split(",").ToList
            Else
                Return GetLanguageCodes(Media_Companion.Pref.TMDbSelectedLanguageName)
            End If
        End Get
    End Property
    
    Shared Public ReadOnly Property AvailableLanguages As XDocument
        Get
            Return XDocument.Load(LanguagesFile)
        End Get 
    End Property

    Public ReadOnly Property LookupLanguages As List(Of String)
        Get
            Return _lookupLanguages
        End Get 
    End Property

    Public ReadOnly Property Api As WatTmdb.V3.Tmdb
        Get
            Return _api
        End Get 
    End Property

    Public ReadOnly Property Config_images_base_url As String
        Get
            Return _config_images_base_url
        End Get 
    End Property

    Public ReadOnly Property FrodoPosterThumbs As List(Of FrodoPosterThumb)
        Get
            Fetch
            Return _frodoPosterThumbs
        End Get
    End Property

    Public ReadOnly Property FrodoFanartThumbs As FrodoFanartThumbs
        Get
            Fetch
            Return _frodoFanartThumbs
        End Get
    End Property

    Public ReadOnly Property Cast As List(Of str_MovieActors)
        Get
            Fetch
            FetchCast
            Dim alist As New List(Of str_MovieActors)
            Try
                Dim x = _cast.cast.Count
                If x < 1 Then Return alist
                For i = 0 to Pref.maxactors-1
                    If x = i Then Exit For
                    Dim newact As New str_MovieActors
                    newact.actorid      = _cast.cast(i).id
                    newact.actorname    = _cast.cast(i).name
                    newact.actorrole    = _cast.cast(i).character_name
                    newact.actorthumb   = If(_cast.cast(i).profile_path = Nothing, "", "http://image.tmdb.org/t/p/original" &_cast.cast(i).profile_path)
                    newact.order        = _cast.cast(i).sort_order
                    alist.Add(newact)
                    
                Next
            Catch
            End Try
            Return alist
        End Get
    End Property

    Public ReadOnly Property Genrelist As List(Of String)
        Get
            Fetch
            FetchGenreList
            Dim genres As New List(Of String)
            For Each g In _genrelist.genres 
                genres.Add(g.ToString)
            Next
            Return genres
        End Get
    End Property

    Public ReadOnly Property TvShow As WatTmdb.V3.TmdbTv
        Get
            Fetch
            Return _tv
        End Get
    End Property

    Public ReadOnly Property Genres As String
        Get
            Fetch

            Dim s As String = ""
            Dim i As Integer
            For Each genre In _tv.genres
                s += genre.name + ", "

                i += 1
                If i >= MaxGenres then
                    Exit For
                End If
            Next

            If s <> "" then
                Return s.Substring(0,s.Length-2)
            End If
            Return ""
        End Get 
    End Property
    
    Public ReadOnly Property Director As String
        Get
            Fetch
            FetchCast

            For Each person In _cast.crew
                If person.job.ToLower = "director" then
                    Return person.name
                End If
            Next
            Return ""
        End Get 
    End Property
    
    Public ReadOnly Property Certification As String
        Get
            Fetch
            FetchReleases
            Try
                If IsNothing(_releases.results) Then Return ""
                For Each res In _releases.results
                    If res.iso_3166_1.ToLower = LookupLanguages.Item(0) then
                        Return res.rating
                    End If
                Next

                For Each res In _releases.results
                    If res.iso_3166_1.ToLower = Pref.XbmcTmdbScraperCertCountry Then
                        Return res.rating
                    End If
                Next

                For Each res In _releases.results
                    If res.rating <> "" then
                        Return res.rating
                    End If
                Next
                Return ""
            Catch
                Return ""
            End Try
        End Get 
    End Property

    Function GetTvTranslations() As WatTmdb.V3.TmdbTranslations
        Return _api.GetTvTranslations(Tmdbid)
    End Function

    Function GetTvCast As Boolean
        _cast = _api.GetTVCredits(_tv.id, )
        Return Not IsNothing(_cast)
    End Function
    
    Private Sub FetchCast
        If IsNothing(_cast) then
            If Not (new RetryHandler(AddressOf GetTvCast)).Execute Then Throw New Exception(TMDB_EXC_MSG)
        End If
    End Sub

    Function GetGenreList As Boolean
        _genrelist = _api.GetGenreList(LookupLanguages.Item(0))
        Return Not IsNothing(_genrelist)
    End Function

    Private Sub FetchGenreList
        If IsNothing(_genrelist) Then
            If Not (New RetryHandler(AddressOf GetGenreList)).Execute Then Throw New Exception(TMDB_EXC_MSG)
        End If
    End Sub

    Function GetTvReleases As Boolean
        _releases = _api.GetTvReleases(_tv.id)
        Return Not IsNothing(_releases)
    End Function

    Private Sub FetchReleases
        If IsNothing(_releases) then
            If Not (new RetryHandler(AddressOf GetTvReleases)).Execute Then Throw New Exception(TMDB_EXC_MSG)
        End If
    End Sub
    
    Public ReadOnly Property TvImages As WatTmdb.V3.TmdbTvImages
        Get
            Fetch
            Return _tvImages
        End Get 
    End Property

    Public ReadOnly Property McPosters As List(Of McImage)
        Get
            Fetch
            Return _mcPosters
        End Get 
    End Property

    Public ReadOnly Property McFanart As List(Of McImage) 
        Get
            Fetch
            Return _mcFanart
        End Get 
    End Property

    Public ReadOnly Property Thumbs As List(Of String)
        Get
            Fetch
            Return _thumbs
        End Get 
    End Property

    Public ReadOnly Property FirstOriginalPosterUrl As String
        Get
            Fetch

            If ValidPosters.Count=0 then
                Return ""
            End If

            Return Me.HdPath + ValidPosters(0).file_path
        End Get 
    End Property

    Public ReadOnly Property HdPath As String
        Get
            Return _config_images_base_url + "original"
        End Get 
    End Property

    Public ReadOnly Property LdBackDropPath As String
        Get
            Return _config_images_base_url + "w1280"
        End Get 
    End Property

    Public ReadOnly Property LdPosterPath As String
        Get
            Return _config_images_base_url + "w500"
        End Get 
    End Property

    Public ReadOnly Property Keywords As List(Of String)
        Get
            Fetch
            Return _keywords 
        End Get
    End Property

    #End Region  'Read-only properties

    Sub new( Optional __tmdb As String=Nothing )
        _api         = New WatTmdb.V3.Tmdb(Key)
        AssignConfig_images_base_url
        Languages    = LanguageCodes
        TmdbId       = __tmdb
    End Sub

    Public Shared Sub DeleteConfigFile
        Dim fi As IO.FileInfo = New IO.FileInfo(TMDbConfigImagesBaseUrlFile)

        If fi.Exists then
            fi.Delete
        End If
    End Sub
    
    Function GetConfiguration As Boolean
        _config_images_base_url = _api.GetConfiguration().images.base_url
        Return Not IsNothing(_config_images_base_url)
    End Function

    Private Sub AssignConfig_images_base_url
        Dim fi As IO.FileInfo = New IO.FileInfo(TMDbConfigImagesBaseUrlFile)
        Dim expired As Boolean = True
        _config_images_base_url = Nothing

        Try
            If fi.Exists Then
                expired = (DateTime.Now-fi.LastWriteTime).TotalDays>TMDbConfigFileMaxAgeInDays
                If Not expired then
                    _config_images_base_url = File.ReadAllText(TMDbConfigImagesBaseUrlFile)
                    Return  
                End If
            End If
        Catch
        End Try
 
        Dim Ok As Boolean = (new RetryHandler(AddressOf GetConfiguration)).Execute
        If Ok Then
            Try
                If fi.Exists Then fi.Delete
                File.WriteAllText(TMDbConfigImagesBaseUrlFile, _config_images_base_url)
                Return
            Catch
            End Try
        End If
       
        'Fallback on expired file
        Try
            If fi.Exists Then _config_images_base_url = File.ReadAllText(TMDbConfigImagesBaseUrlFile)
            Return
        Catch
        End Try
        
        'If all else fails -> Write a default one
        Try
            _config_images_base_url = "http://d3gtl9l2a4fn1j.cloudfront.net/t/p/"
            If fi.Exists then fi.Delete
            File.WriteAllText(TMDbConfigImagesBaseUrlFile, _config_images_base_url)
        Catch
            Throw New Exception("AssignConfig_images_base_url failed")
        End Try
    End Sub
    
    Function GetTvBy As Boolean
        If TmdbId <> "" Then
            Return GetTvByTMDB
        ElseIf TvdbId <> "" Then
            Return GetTvByTVDB
        Else If ImdbId <> "" Then
            Return GetTvByIMDB
        End If
        Return False
    End Function

    Function GetTvByTMDB As Boolean
        _tv  = _api.GetTVInfo(TmdbId, _lookupLanguages.Item(0))
        '_tmdbId = _movie.id.ToString
        Return Not IsNothing(_tv)
    End Function

    Function GetTvByTVDB As Boolean
        _tvfind = _api.Find(TvdbId, "tvdb_id")
        If _tvfind.tv_results(0).id <> "" Then TmdbId = _tvfind.tv_results(0).id
        Return GetTvByTMDB()
        '_movie  = _api.GetMovieByIMDB( Imdb, _lookupLanguages.Item(0) )
        '_tmdbId = _movie.id.ToString
        'Return Not IsNothing(_tvfind)
    End Function

    Function GetTvByIMDB As Boolean
        _tvfind = _api.Find(ImdbId, "imdb_id")
        If _tvfind.tv_results(0).id <> "" Then TmdbId = _tvfind.tv_results(0).id
        Return GetTvByTMDB()
        '_imdb   = _movie.imdb_id
        'Return Not IsNothing(_tvfind)
    End Function

    Function GetTvCredits As Boolean
        _cast = _api.GetTVCredits(TmdbId)
        Return Not IsNothing(_cast)
    End Function
    
    Function GetTvImages As Boolean
        _tvImages = _api.GetTVImages(TmdbId) '  (_movie.id)
        Return Not IsNothing(_tvImages)
    End Function

    Function GetTvKeywords As Boolean
        ValidKeyWords = _api.GetTvKeywords(_tv.id)
        Return Not IsNothing(ValidKeyWords)
    End Function
    
    Private Sub Fetch
        Try
            If _tv.id = 0 And Not _fetched Then
                
                _fetched = True

                Dim rhs As List(Of RetryHandler) = New List(Of RetryHandler)

                rhs.Add(New RetryHandler(AddressOf GetTvBy      ))
                rhs.Add(New RetryHandler(AddressOf GetTvImages  ))
                rhs.Add(New RetryHandler(AddressOf GetTvKeywords))

                For Each rh In rhs
                    If Not rh.Execute Then Throw New Exception(TMDB_EXC_MSG)
                Next
                

                'If Series isn't found -> Create empty child objects
                If IsNothing(_tvImages.backdrops) Then _tvImages.backdrops = New List(Of WatTmdb.V3.Backdrop)
                If IsNothing(_tvImages.posters  ) Then _tvImages.posters   = New List(Of WatTmdb.V3.Poster  )

                FixUpMovieImages()
                AssignValidBackDrops()
                AssignValidPosters()
                AssignMcPosters()
                AssignMcThumbs()
                AssignMcFanart()
                AssignFrodoExtraPosterThumbs()
                AssignFrodoExtraFanartThumbs()
                AssignKeywords()
            End If
        Catch ex As Exception
            Throw New Exception (ex.Message)
        End Try

    End Sub
    
    Private Sub AssignFrodoExtraPosterThumbs

        For Each item In ValidPosters
            _frodoPosterThumbs.Add(New FrodoPosterThumb("poster",HdPath+item.file_path))
        Next
    End Sub

    Private Sub AssignFrodoExtraFanartThumbs
        For Each item In ValidBackDrops
            _frodoFanartThumbs.Thumbs.Add(New FrodoFanartThumb( LdBackDropPath+item.file_path ,HdPath+item.file_path))
        Next
    End Sub

    Private Sub FixUpMovieImages
        FixUnassigned_iso_639_1(_tvImages.backdrops)
        FixUnassigned_iso_639_1(_tvImages.posters)
    End Sub

    Private Sub FixUnassigned_iso_639_1( images )
        If IsNothing(images) Then Exit Sub
        For Each item In images
            If IsNothing(item.iso_639_1) then
                item.iso_639_1 = "?"
            End If
        Next
    End Sub

    Private Sub AssignValidBackDrops
        Dim q = From b In _tvImages.backdrops Where _lookupLanguages.IndexOf(b.iso_639_1.ToLower) > -1 Order By _lookupLanguages.IndexOf(b.iso_639_1.ToLower) Ascending, b.vote_average Descending    
                                                                                                                                        
        For each item In q
            ValidBackDrops.AddIfNew(item)
        Next
    End Sub
    
    Private Sub AssignValidPosters
        Dim q = From b In _tvImages.posters Where _lookupLanguages.IndexOf(b.iso_639_1.ToLower) > -1 Order By _lookupLanguages.IndexOf(b.iso_639_1.ToLower) Ascending, b.vote_average Descending    

        For each item In q
            ValidPosters.AddIfNew(item)
        Next
    End Sub
    
    Private Sub AssignMcPosters
        AssignMcPosters(ValidPosters,_mcPosters)
    End Sub
    
    Private Sub AssignMcFanart
        AssignMcFanart(ValidBackDrops,_mcFanart)
    End Sub
    
    Private Sub AssignMcPosters( tmDbImages As Object, mcImages As List(Of McImage))
        Dim tmpimages As New List(Of McImage)
        For Each item In tmDbImages
            tmpimages.Add( McImage.GetFromTmDbBackDrop(item,HdPath,LdPosterPath) )
            'mcImages.Add( McImage.GetFromTmDbBackDrop(item,HdPath,LdPosterPath) )
        Next
        If Not tmpimages.Count = 0 Then
            Dim q = From x In tmpimages Order By x.votes Descending
            mcImages.AddRange(q.ToList)
        End If
    End Sub
 

    Private Sub AssignMcFanart( tmDbImages As Object, mcImages As List(Of McImage))
        Dim tmpimages As New List(Of McImage)
        For Each item In tmDbImages
            tmpimages.Add( McImage.GetFromTmDbBackDrop(item,HdPath,LdBackDropPath) )
            'mcImages.Add( McImage.GetFromTmDbBackDrop(item,HdPath,LdBackDropPath) )
        Next
        If Not tmpimages.Count = 0 Then
            Dim q = From x In tmpimages Order By x.votes Descending
            mcImages.AddRange(q.ToList)
        End If
    End Sub
  

    Private Sub AssignMcThumbs   
        For Each item In ValidBackDrops
            _thumbs.Add( HdPath + item.file_path )
        Next
    End Sub

    Private Sub AssignKeywords
        If IsNothing(ValidKeyWords.results) Then Exit Sub
        For Each keywd In ValidKeyWords.results
            _keywords.Add(keywd.ToString)
        Next
    End Sub

    Function getyear(ifdate As String) As String
        Dim isyear As String = ""
        Try
            If IsDate(ifdate) Then
				Dim x = Convert.ToDateTime(ifdate)

				isyear = x.Year.ToString
			End If
        Catch
        End Try
        Return isyear
    End Function

    Function SelectBackDrop( width As Integer ) As WatTmdb.V3.Backdrop
        Fetch
        Dim q = From b In ValidBackDrops Where b.width = width Order By b.vote_average Descending

        If q.Count = 0 then
            q = From b In ValidBackDrops Where b.width > width Order By b.vote_average Descending
        End If

        If q.Count = 0 then
            q = From b In ValidBackDrops Order By b.width Descending, b.vote_average Descending
        End If

        If q.Count = 0 then
            return Nothing
        End If

        Return q.First
    End Function

    Function GetBackDropUrl( Optional resolution As Resolution=Resolution.FullHD ) As String
        Fetch
        Dim BackDrop As WatTmdb.V3.Backdrop = SelectBackDrop( CInt(resolution) )

        If IsNothing(BackDrop) then
            Return Nothing
        End If

        If resolution=Resolution.FullHD then
            Return HdPath         + BackDrop.file_path
        Else
            Return LdBackDropPath + BackDrop.file_path
        End if
    End Function

    Function SaveBackDrop( destination As String, Optional resolution As Resolution=Resolution.FullHD ) As Boolean
        Dim url As String=GetBackDropUrl(resolution)

        If IsNothing(url) then
            Return False
        End If

        Using wc As New System.Net.WebClient()
            wc.DownloadFile(url, destination)
        End Using
 
        Return True
    End Function

    Shared Sub LoadLanguages(ByRef cb As ComboBox)
        cb.Items.Clear
        
        Dim q = From x In AvailableLanguages.Descendants("language")
                            Select name = x.Attribute("name").Value
                            Order By name
                    
        For Each element In q
            cb.Items.Add(element)
        Next
    End Sub
    
    Shared Function GetLanguageCodes(name As String) As List(Of String)
        Dim q = From x In AvailableLanguages.Descendants("language")
                            Select value   = x.Attribute("value").Value, 
                                   attName = x.Attribute("name" ).Value
                            Where attName = name 

        Return q.Single().value.Split(",").ToList
    End Function

#Region "Obsolete code"
    'Private _setId As String
    'Public Property ValidSetBackDrops As New List(Of WatTmdb.V3.CollectionBackdrop)
    'Public Property ValidSetPosters As New List(Of WatTmdb.V3.CollectionPoster  )
    'Private _movie                  As New WatTmdb.V3.TmdbMovie
    'Private _collection             As New WatTmdb.V3.TmdbCollection
    'Private _collectionImages       As WatTmdb.V3.TmdbCollectionImages 
    'Private _trailers               As WatTmdb.V3.TmdbMovieTrailers
    'Private _mcSetPosters           As New List(Of McImage)
    'Private _mcSetFanart            As New List(Of McImage)
    'Private _mc_collection          As New List(Of MovieSetDatabase)
    'Private _alternateTitles        As WatTmdb.V3.TmdbMovieAlternateTitles
    'Private _mcAlternateTitles      As New List(Of String)
    'Private _setFetched             As Boolean = False

    'Function GetTrailerUrl(FailedUrls As List(Of String), Optional resolution As String="1080" ) As String
    '    Fetch
            
    '    Dim q = From t In _trailers.youtube Where t.size=resolution+"p" And Not FailedUrls.Contains(t.source)

    '    If q.Count = 0 then
    '        q = From t In _trailers.youtube Where Not FailedUrls.Contains(t.source) Order By t.size Descending
    '    End If

    '    If q.Count = 0 then
    '        return ""
    '    End If

    '    Return q.First.source
    'End Function

    
    'Private Sub AssignValidSetBackDrops
    '    If IsNothing(_collectionImages.backdrops) Then Exit Sub
    '    Dim q = From b In _collectionImages.backdrops Where _lookupLanguages.IndexOf(b.iso_639_1.ToLower) > -1 Order By _lookupLanguages.IndexOf(b.iso_639_1.ToLower) Ascending, b.height Descending
                                                                                                                         
    '    For each item In q
    '        ValidSetBackDrops.AddIfNew(item)
    '    Next
    'End Sub

    'Private Sub AssignValidSetPosters
    '    If IsNothing(_collectionImages.posters) Then Exit Sub
    '    Dim q = From b In _collectionImages.posters Where _lookupLanguages.IndexOf(b.iso_639_1.ToLower) > -1 Order By _lookupLanguages.IndexOf(b.iso_639_1.ToLower) Ascending, b.Height Descending

    '    For each item In q
    '        ValidSetPosters.AddIfNew(item)
    '    Next
    'End Sub

    'Private Sub AssignMcSetPosters
    '    AssignMcPosters(ValidSetPosters,_mcSetPosters)
    'End Sub

    'Private Sub AssignMcSetFanart
    '    AssignMcFanart(ValidSetBackDrops,_mcSetFanart)
    'End Sub
    
    'Private Sub AssignMcCollections
    '    If IsNothing(_collection.parts) Then Exit Sub
    '    For each item In _collection.parts
    '        Dim tmpitem As New MovieSetDatabase
    '        tmpitem.title   = item.title
    '        tmpitem.tmdbid  = item.id
    '        tmpitem.year    = getyear(item.release_date)
    '        _mc_collection.Add(tmpitem)
    '    Next
    'End Sub
    
    'Public ReadOnly Property Trailers As WatTmdb.V3.TmdbMovieTrailers
    '    Get
    '        Fetch
    '        Return _trailers
    '    End Get 
    'End Property

    'Private Sub SafeAssignSetId
    '    If Not IsNothing(_movie.belongs_to_collection) Then
    '        SetId = _movie.belongs_to_collection.id.ToString
    '    End If
    'End Sub

    'Function GetMovieCollectionImages As Boolean
    '    _collectionImages = _api.GetCollectionImages(ToInt(SetId))  
    '    Return Not IsNothing(_collectionImages)
    'End Function

    'Function GetMovieTrailers As Boolean
    '    _trailers    = _api.GetMovieTrailers(_movie.id)
    '    Return Not IsNothing(_trailers)
    'End Function

    'Function GetMoviesInCollection As Boolean
    '    _collection = _api.GetCollectionInfo(ToInt(SetId), _lookupLanguages.Item(0))
    '    Return Not IsNothing(_collection)
    'End Function

    
    'Private Sub FetchSet
    '    Try
    '        If SetId <> "" And Not _setFetched Then

    '            _setFetched = True

    '            'If urlcheck AndAlso Not Utilities.UrlIsValid("https://api.themoviedb.org") Then
    '            '    Throw New Exception(TMDB_EXC_MSG)
    '            'End If

    '            Dim rhs As List(Of RetryHandler) = New List(Of RetryHandler)

    '            rhs.Add(New RetryHandler(AddressOf GetMoviesInCollection   ))
    '            rhs.Add(New RetryHandler(AddressOf GetMovieCollectionImages))

    '            For Each rh In rhs
    '                If Not rh.Execute Then Throw New Exception(TMDB_EXC_MSG)
    '            Next

    '            FixUpCollectionImages()
    '            AssignValidSetBackDrops()
    '            AssignValidSetPosters()
    '            AssignMcSetPosters()
    '            AssignMcSetFanart()
    '            AssignMcCollections()
    '        End If
    '    Catch ex As Exception
    '        Throw New Exception (ex.Message)
    '    End Try

    'End Sub
    
    'Private Sub FixUpCollectionImages
    '    FixUnassigned_iso_639_1(_collectionImages.backdrops)
    '    FixUnassigned_iso_639_1(_collectionImages.posters)
    'End Sub
    
    'Public ReadOnly Property Movie As WatTmdb.V3.TmdbMovie
    '    Get
    '        Fetch
    '        Return _movie
    '    End Get 
    'End Property

    'Public ReadOnly Property Collection As List(Of MovieSetDatabase)
    '    Get
    '        FetchSet()
    '        Return _mc_collection
    '    End Get
    'End Property


    'Public ReadOnly Property TmdbCollection As WatTmdb.V3.TmdbCollection
    '    Get
    '        FetchSet
    '        Return _collection
    '    End Get
    'End Property


    'Public ReadOnly Property MovieSet As MovieSetInfo
    '    Get
    '        FetchSet

    '        If IsNothing(_collection) Then Return Nothing

    '        If _collection.id=0 Then Return Nothing

    '        Return New MovieSetInfo(_collection.name, _collection.id, _collection.overview, CollectionMovies, Date.Now )
    '    End Get
    'End Property


    'Public ReadOnly Property CollectionMovies As List(Of CollectionMovie)
    '    Get
    '        Dim q = From m In _collection.Parts Select New CollectionMovie( m.title, m.id, m.backdrop_path, m.poster_path, m.release_date )

    '        Return q.ToList
    '    End Get

    'End Property


    'Public ReadOnly Property releasedate As String
    '    Get
    '        Fetch
    '        Return movie.release_date 
    '    End Get
    'End Property

    'Don't think TMDb returns this info...
    'Public ReadOnly Property Stars As String
    '    Get
    '        Fetch
    '        FetchCast

    '        Dim s As String = ""
            
    '        For Each person In _cast.crew
    '            If person.job.ToLower = "star" then
    '                s += person.name + ", "
    '            End If
    '        Next

    '        If s <> "" then
    '            Return s.Substring(0,s.Length-2)
    '        End If
    '        Return ""
    '    End Get 
    'End Property
    
    'Function GetMovieCast As Boolean
    '    _cast = _api.GetMovieCast(_movie.id)
    '    Return Not IsNothing(_cast)
    'End Function

    'Function GetMovieAlternateTitles As Boolean
    '    _alternateTitles = _api.GetMovieAlternateTitles(Movie.id,LookupLanguages.Item(0))
    '    Return Not IsNothing(_alternateTitles)
    'End Function

    'Public ReadOnly Property AlternateTitles As List(Of String)
    '    Get
    '        Fetch
    '        If IsNothing(_alternateTitles) then 
    '            If Not (new RetryHandler(AddressOf GetMovieAlternateTitles)).Execute Then Throw New Exception(TMDB_EXC_MSG)
    '        End If

    '        _mcAlternateTitles.Clear

    '        For Each item In _alternateTitles.titles
    '            _mcAlternateTitles.Add(item.title)
    '        Next

    '        Return _mcAlternateTitles
    '    End Get 
    'End Property
    
    'Public ReadOnly Property McSetPosters As List(Of McImage)
    '    Get
    '        FetchSet
    '        Return _mcSetPosters
    '    End Get 
    'End Property
    
    'Public ReadOnly Property McSetFanart As List(Of McImage) 
    '    Get
    '        FetchSet
    '        Return _mcSetFanart
    '    End Get 
    'End Property

#End Region

End Class
