Imports System.ComponentModel
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Linq
Imports System.Xml
Imports Media_Companion
Imports MC_UserControls
Imports XBMC.JsonRpc

Module Ext
    <System.Runtime.CompilerServices.Extension()> _
    Public Sub AppendChild(root As XmlElement, doc As XmlDocument, name As String, value As String)

        Dim child As XmlElement = doc.CreateElement(name)

        If String.IsNullOrEmpty(value) Then value = ""
        child.InnerText = value
        root.AppendChild(child)
    End Sub

    <System.Runtime.CompilerServices.Extension()> _
    Public Sub AppendChild(root As XmlElement, doc As XmlDocument, name As String, value As String, type As String, Optional def As Boolean = False)

        Dim child As XmlElement = doc.CreateElement(name)
        Dim attr As XmlAttribute = doc.CreateAttribute("type")
        attr.Value = type
        child.SetAttributeNode(attr)
        If def Then
            Dim attr2 As XmlAttribute = doc.CreateAttribute("default")
            attr2.Value = "true"
            child.SetAttributeNode(attr2)
        End If

        If String.IsNullOrEmpty(value) Then value = ""
        child.InnerText = value
        root.AppendChild(child)
    End Sub

    <System.Runtime.CompilerServices.Extension()> _
    Public Sub AppendChildList(root As XmlElement, doc As XmlDocument, name As String, value As String, Optional splitter As String = "/")
        If String.IsNullOrEmpty(value) Then
            root.AppendChild( doc, name, value )
            Exit Sub
        End If
        Dim splt() As String = value.Split(splitter)
        For each sp In splt
            root.AppendChild( doc, name, sp.Trim)
        Next
    End Sub

    <System.Runtime.CompilerServices.Extension()> _
    Public Sub AppendChildList(root As XmlElement, doc As XmlDocument, name As String, value As List(Of String), Optional ExcludeEmptyNode As Boolean = False)
        If value.Count < 1 Then
            If ExcludeEmptyNode Then Exit Sub
            root.AppendChild( doc, name, "" )
            Exit Sub
        End If
        For each sp In value
            root.AppendChild( doc, name, sp.Trim)
        Next
    End Sub

End Module

Public Class Movies

    Public Event AmountDownloadedChanged (ByVal iNewProgress As Long)
    Public Event FileDownloadSizeObtained(ByVal iFileSize    As Long)
    Public Event FileDownloadComplete    ()
    Public Event FileDownloadFailed      (ByVal ex As Exception)

    Private     _certificateMappings    As CertificateMappings
    Private     _actorDb                As New List(Of Databases)
    Public      _tmpActorDb             As New List(Of Databases)
    Private     _directorDb             As New List(Of DirectorDatabase)
    Public      _tmpDirectorDb          As New List(Of DirectorDatabase)
    Private     _moviesetDb             As New List(Of MovieSetInfo)
    Private     _tmdbSetMissingMovies   As New List(Of TmdbSetMissingMovie)
    Private     _tagDb                  As New List(Of TagDatabase)
    Public      _tmpTagDb               As New List(Of TagDatabase)

    Public Shared movRebuildCaches      As Boolean = False

    Public Property Bw                  As BackgroundWorker = Nothing
    Public Property MovieCache          As New List(Of ComboList)
    Public Property TmpMovieCache       As New List(Of ComboList)
    Public Property tmpMVCache          As New List(Of MVComboList)
    Public Property tmpHmVidCache       As New List(Of HmMovComboList)
    
    Public Property NewMovies           As New List(Of Movie)
    Public Property PercentDone         As Integer = 0

    Private _data_GridViewMovieCache    As New List(Of Data_GridViewMovie)
    
    Public Event XbmcMcMoviesChanged
    Public Event XbmcOnlyMoviesChanged
    
    Private _xbmcMcMovies   As Dictionary(Of String, XbmcMovieForCompare)
    Private _xbmcOnlyMovies As List      (Of         XbmcMovieForCompare)

    Property XbmcMcMovies As Dictionary(Of String, XbmcMovieForCompare)
        Get
            Return _xbmcMcMovies
        End Get
        Set
            _xbmcMcMovies = Value
            RaiseEvent XbmcMcMoviesChanged
        End Set
    End Property

    Property XbmcOnlyMovies As List(Of XbmcMovieForCompare)
        Get
            Return _xbmcOnlyMovies
        End Get
        Set
            _xbmcOnlyMovies = Value
            RaiseEvent XbmcOnlyMoviesChanged
        End Set
    End Property
    
    Property ActorsFilter_AlsoInclude           As New List(Of String)
    Property TagsFilter_AlsoInclude             As New List(Of String)
    Property DirectorsFilter_AlsoInclude        As New List(Of String)
    Property SetsFilter_AlsoInclude             As New List(Of String)

#Region "Public ReadOnly Properties"

    Public ReadOnly Property CertificateMappings As CertificateMappings
        Get
            If IsNothing(_certificateMappings) Then
                _certificateMappings = New CertificateMappings 
            End If

            Return _certificateMappings
        End Get
    End Property

    Public ReadOnly Property Data_GridViewMovieCache As List(Of Data_GridViewMovie)
        Get
            Return _data_GridViewMovieCache
        End Get
    End Property

    Public ReadOnly Property CertificatesFilter As List(Of String)
        Get
            Dim q = From x In MovieCache Select field=CertificateMappings.GetMapping(x.Certificate)
                        Group By field Into Num=Count
                        Order By field
                        Select field.IfBlankMissing & " (" & Num.ToString & ")" 

            Return q.AsEnumerable.ToList
        End Get
    End Property    

    Public ReadOnly Property MoviesWithUniqueMovieTitles As List(Of ComboList)
        Get
            Dim q = From x In MovieCache 
                    Join
                       u In UniqueMovieTitles On u Equals x.Title
                    Select
                        x

            Return q.AsEnumerable.ToList
        End Get
    End Property    

    Public ReadOnly Property UniqueMovieTitles As List(Of String)
        Get
            Dim q = From x In MovieCache 
                    Select 
                        x.Title
                    Group By 
                        Title Into Num=Count
                    Select 
                        Title,Num
                    Where
                        Num = 1
                    Select
                        Title

            Return q.AsEnumerable.ToList
        End Get
    End Property    
    
    Public ReadOnly Property CountriesFilter As List(Of String)
        Get
            Dim q = From x In MovieCache Select ms=x.countries.Split("/")
             
            Dim lst = q.SelectMany(Function(m) m).ToList

            lst.RemoveAll(Function(v) v="" )
            lst.RemoveAll(Function(v) v=",")
            lst.RemoveAll(Function(v) v="/")

            Dim lst2 As New List(Of String)
            For each t In lst
                lst2.Add(t.Trim())
            Next

            Dim q2 = From x In lst2
                        Group By x Into Num=Count
                        Order By x
                        Select x & " (" & Num.ToString & ")"
                        
            Return q2.AsEnumerable.ToList
        End Get
    End Property  
    
    Public ReadOnly Property StudiosFilter As List(Of String)
        Get
            Dim q = From x In MovieCache Select ms=x.studios.Split("/")
             
            Dim lst = q.SelectMany(Function(m) m).ToList

            lst.RemoveAll(Function(v) v="" )
            lst.RemoveAll(Function(v) v=",")
            lst.RemoveAll(Function(v) v="/")

            Dim lst2 As New List(Of String)
            For each t In lst
                lst2.Add(t.Trim())
            Next
            Dim q2 = From x In lst2
                        Group By x Into Num=Count
                        Order By x
                        Select x & " (" & Num.ToString & ")"
                        
            Return q2.AsEnumerable.ToList
        End Get
    End Property  

    Public ReadOnly Property GenresFilter As List(Of String)
        Get
            Dim q = From x In MovieCache Select ms=x.genre.Split("/")
             
            Dim lst = q.SelectMany(Function(m) m).ToList

            lst.RemoveAll(Function(v) v="" )
            lst.RemoveAll(Function(v) v="/")
            lst.RemoveAll(Function(v) v="/")

            Dim lst2 As New List(Of String)
            For each t In lst
                lst2.Add(t.Trim())
            Next
            Dim q2 = From x In lst2
                        Group By x Into Num=Count
                        Order By x
                        Select x & " (" & Num.ToString & ")" 

            Return q2.AsEnumerable.ToList
        End Get
    End Property    

    Public ReadOnly Property MinVotes As Integer
        Get
            If MovieCache.Count=0 Then Return 0

            Dim q = Aggregate m In MovieCache Into Min(m.Votes)

            Return q
        End Get
    End Property    

    Public ReadOnly Property ListVotes As List(Of Integer)
        Get
            Dim q = From m In MovieCache Select m.Votes

            Return q.AsEnumerable.ToList
        End Get
    End Property   
     
    Public ReadOnly Property ListRuntimes As List(Of Integer)
        Get
            Dim q = From m In MovieCache Select m.IntRuntime

            Return q.AsEnumerable.ToList
        End Get
    End Property    
    
    Public ReadOnly Property ListFolderSizes As List(Of Double)
        Get
            Dim q = From m In MovieCache Select m.DisplayFolderSize
           
            Return q.AsEnumerable.ToList
        End Get
    End Property    

    Public ReadOnly Property MinFolderSize As Double
        Get
            If MovieCache.Count=0 Then Return 0

            Dim q = Aggregate m In MovieCache Into Min(m.DisplayFolderSize)

            Return q
        End Get
    End Property    

    Public ReadOnly Property MaxFolderSize As Double
        Get
            If MovieCache.Count=0 Then Return 0

            Dim q = Aggregate m In MovieCache Into Max(m.DisplayFolderSize)

            Return q
        End Get
    End Property    
    
    Public ReadOnly Property MaxVotes As Integer
        Get
            If MovieCache.Count=0 Then Return 0

            Dim q = Aggregate m In MovieCache Into Max(m.Votes)

            Return q
        End Get
    End Property    

    Public ReadOnly Property MinYear As Integer
        Get
            If MovieCache.Count=0 Then Return 0

            Dim q = Aggregate m In MovieCache Into Min(m.Year)

            Return q
        End Get
    End Property    

    Public ReadOnly Property MaxYear As Integer
        Get
            If MovieCache.Count=0 Then Return 0

            Dim q = Aggregate m In MovieCache Into Max(m.Year)

            Return q
        End Get
    End Property    

    Public ReadOnly Property GeneralFilters As List(Of String)
        Get
            Dim lst = New List(Of String)
            lst.Add( "All"                      )
            lst.Add( Watched                    )
            lst.Add( Unwatched                  )
            lst.Add( ScrapeError                )
            lst.Add( Duplicates                 )
            If Not Pref.DisableNotMatchingRenamePattern Then lst.Add( NotMatchingRenamePattern )
            lst.Add( MissingFromSetReleased     )
            lst.Add( MissingFromSetUnreleased   )
            lst.Add( UserSetAdditions           )
            lst.Add( MissingTmdbSetInfo         )
            lst.Add( MissingCertificate         )
            lst.Add( MissingPlot                )
            lst.Add( MissingOutline             )
            lst.Add( PlotEqualsOutline          )
            lst.Add( OutlineContainsHtml        )
            lst.Add( MissingPremier             )
            lst.Add( MissingTagline             )
            lst.Add( MissingRating              )
            lst.Add( MissingVotes               )
            lst.Add( MissingGenre               )
            lst.Add( MissingStars               )
            lst.Add( MissingDirector            )
            lst.Add( MissingCredits             )
            lst.Add( MissingStudios             )
            lst.Add( MissingCountry             )
            lst.Add( MissingRuntime             )
            lst.Add( MissingYear                )
            lst.Add( MissingFanart              )
            lst.Add( MissingPoster              )
            lst.Add( PreFrodoPosterOnly         )
            lst.Add( FrodoPosterOnly            )
            lst.Add( FrodoAndPreFrodoPosters    )
            lst.Add( MissingLocalActors         )
            lst.Add( MissingTrailer             )
            lst.Add( MissingIMDBId              )
            lst.Add( MissingSource              )
            lst.Add( MissingMovie               )
            If Pref.ShowExtraMovieFilters Then
                lst.Add( "Imdb in folder name ("     &    ImdbInFolderName & ")")
                lst.Add( "Imdb not in folder name (" & NotImdbInFolderName & ")")
                lst.Add( "Imdb not in folder name & year mismatch (" & NotImdbInFolderNameAndYearMisMatch & ")")
                lst.Add( "Plot same as Outline (" &  PlotEqOutline & ")")
            End If
            If Pref.XBMC_Link Then
                If Not IsNothing(Form1.MC_Only_Movies) Then lst.Add( MC_Only_Movies )
                If Not IsNothing(XbmcMcMovies) Then lst.Add( "Different titles (" & Xbmc_DifferentTitles.Count.ToString & ")"  )
            End If

            Return lst
        End Get
    End Property    

    Public ReadOnly Property PlotEqOutline As Integer
        Get
            Return (From x In MovieCache Where x.PlotEqOutline).Count
        End Get
    End Property  

    Public ReadOnly Property NotImdbInFolderNameAndYearMisMatch As Integer
        Get
            Return (From x In MovieCache Where Not x.ImdbInFolderName And x.year<>x.FolderNameYear).Count
        End Get
    End Property  

    Public ReadOnly Property NotImdbInFolderName As Integer
        Get
            Return (From x In MovieCache Where Not x.ImdbInFolderName).Count
        End Get
    End Property  
  
    Public ReadOnly Property ImdbInFolderName As Integer
        Get
            Return (From x In MovieCache Where x.ImdbInFolderName).Count
        End Get
    End Property    

    Public ReadOnly Property NotMatchingRenamePattern As String
        Get
            Return "Not matching rename pattern (" & (From x In MovieCache Where Not x.ActualNfoFileNameMatchesDesired).Count & ")" 
        End Get
    End Property    

    Public ReadOnly Property PreFrodoPosterOnly As String
        Get
            Return "Pre-Frodo poster only (" & PreFrodoPosterOnlyCount.ToString & ")" 
        End Get
    End Property    

    Public ReadOnly Property PreFrodoPosterOnlyCount As Integer
        Get
            Return (From x In MovieCache Where x.PreFrodoPosterExists And Not x.FrodoPosterExists).Count
        End Get
    End Property    

    Public ReadOnly Property FrodoPosterOnly As String
        Get
            Return "Frodo poster only (" & FrodoPosterOnlyCount.ToString & ")" 
        End Get
    End Property    

    Public ReadOnly Property FrodoPosterOnlyCount As Integer
        Get
            Return (From x In MovieCache Where Not x.PreFrodoPosterExists And x.FrodoPosterExists).Count
        End Get
    End Property    

    Public ReadOnly Property FrodoAndPreFrodoPosters As String
        Get
            Return "Both poster formats (" & FrodoAndPreFrodoPostersCount.ToString & ")" 
        End Get
    End Property    

    Public ReadOnly Property FrodoAndPreFrodoPostersCount As Integer
        Get
            Return (From x In MovieCache Where x.PreFrodoPosterExists And x.FrodoPosterExists).Count
        End Get
    End Property    

    Public ReadOnly Property MissingCertificate As String
        Get
            Return "Missing Certificate (" & (From x In MovieCache Where x.MissingCertificate).Count & ")" 
        End Get
    End Property 
       
    Public ReadOnly Property OutlineContainsHtml As String
        Get
            Return "Outline contains html (" & (From x In MovieCache Where x.OutlineContainsHtml).Count & ")" 
        End Get
    End Property    

    Public ReadOnly Property MissingFanart As String
        Get
            Return "Missing Fanart (" & (From x In MovieCache Where x.MissingFanart).Count & ")" 
        End Get
    End Property    

    Public ReadOnly Property MissingTrailer As String
        Get
            Return "Missing Trailer (" & (From x In MovieCache Where x.MissingTrailer).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MissingLocalActors As String
        Get
            Return "Missing Local Actors (" & (From x In MovieCache Where x.MissingLocalActors).Count & ")" 
        End Get
    End Property    

    Public ReadOnly Property MissingPoster As String
        Get
            Return "Missing Poster (" & (From x In MovieCache Where x.MissingPoster).Count & ")" 
        End Get
    End Property    
     
    Public ReadOnly Property MissingPlot As String
        Get
            Return "Missing Plot (" & (From x In MovieCache Where x.MissingPlot).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MissingRating As String
        Get
            Return "Missing Rating (" & (From x In MovieCache Where x.MissingRating).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MissingPremier As String
        Get
            Return "Missing Premier (" & (From x In MovieCache Where x.MissingPremier).Count & ")" 
        End Get
    End Property

    Public ReadOnly Property MissingGenre As String
        Get
            Return "Missing Genre (" & (From x In MovieCache Where x.MissingGenre).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MissingOutline As String
        Get
            Return "Missing Outline (" & (From x In MovieCache Where x.MissingOutline).Count & ")" 
        End Get
    End Property
    
    Public ReadOnly Property PlotEqualsOutline As String
        Get
            Return "Plot same as Outline (" & (From x In MovieCache Where x.PlotEqualsOutline).Count & ")" 
        End Get
    End Property

    Public ReadOnly Property MissingTagline As String
        Get
            Return "Missing Tagline (" & (From x In MovieCache Where x.MissingTagLine).Count & ")" 
        End Get
    End Property

    Public ReadOnly Property MissingStars As String
        Get
            Return "Missing Stars (" & (From x In MovieCache Where x.MissingStars).Count & ")" 
        End Get
    End Property

    Public ReadOnly Property MissingRuntime As String
        Get
            Return "Missing Runtime (" & (From x In MovieCache Where x.MissingRuntime).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MissingIMDBId As String
        Get
            Return "Missing IMDB (" & (From x In MovieCache Where x.MissingIMDBId).Count & ")"
        End Get
    End Property

    Public ReadOnly Property MissingSource As String
        Get
            Return "Missing Source (" & (From x In MovieCache Where x.MissingSource).Count & ")"
        End Get
    End Property

    Public ReadOnly Property MissingDirector As String
        Get
            Return "Missing Director (" & (From x In MovieCache Where x.MissingDirector).Count & ")"
        End Get
    End Property
    
    Public ReadOnly Property MissingCredits As String
        Get
            Return "Missing Writer (" & (From x In MovieCache Where x.MissingCredits).Count & ")"
        End Get
    End Property
    
    Public ReadOnly Property MissingStudios As String
        Get
            Return "Missing Studios (" & (From x In MovieCache Where x.MissingStudios).Count & ")"
        End Get
    End Property

    Public ReadOnly Property MissingCountry As String
        Get
            Return "Missing Country (" & (From x In MovieCache Where x.MissingCountry).Count & ")"
        End Get
    End Property

    Public ReadOnly Property MissingVotes As String
        Get
            Return "Missing Votes (" & (From x In MovieCache Where x.MissingVotes).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MissingYear As String
        Get
            Return "Missing Year (" & (From x In MovieCache Where x.MissingYear).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property MC_Only_Movies As String
        Get
            Return "Missing from XBMC (" & (From x In Form1.MC_Only_Movies).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property ScrapeError
        Get
            Return "Scrape Error (" & (From x In MovieCache Where x.genre.ToLower = "problem").Count & ")"
        End Get
    End Property

    Public ReadOnly Property Duplicates As String
        Get
            Dim total           As Integer = (From x In MovieCache).Count
            Dim total_distinct  As Integer = (From x In MovieCache Where x.id <> "0" Select x.id ).Distinct.Count
            Dim total_noId      As Integer = (From x In MovieCache Where x.id = "0" Select x.id).Count 

            Dim num_duplicates = total - (total_distinct + total_noId)

            Return "Duplicates (" & num_duplicates & ")"
        End Get
    End Property    

    Public ReadOnly Property Watched As String
        Get
            Return "Watched (" & (From x In MovieCache Where x.Watched).Count & ")" 
        End Get
    End Property  

    Public ReadOnly Property Unwatched As String
        Get
            Return "Unwatched (" & (From x In MovieCache Where Not x.Watched).Count & ")" 
        End Get
    End Property    

    Public ReadOnly Property ActorsFilter_Preferences As IEnumerable(Of String)
        Get
            Dim q = From x In ActorDb 
                Group By 
                    x.ActorName Into NumFilms=Count 
                Where 
                    NumFilms>= Pref.ActorsFilterMinFilms
            
            If Pref.MovieFilters_Actors_Order=0 Then 
                q = From x In q Order by x.NumFilms  Descending, x.ActorName Ascending
            Else
                q = From x In q Order by x.ActorName Ascending , x.NumFilms  Descending
            End If
            Return From x In q Select x.ActorName & " (" & x.NumFilms.ToString & ")" Take Pref.MaxActorsInFilter
        End Get
    End Property    
    
    Public ReadOnly Property ActorsFilter As List(Of String)
        Get
            Dim r = (From x In ActorsFilter_Preferences).Union(From x In ActorsFilter_Extras) 
            Return r.ToList
        End Get
    End Property    
    
    Public ReadOnly Property ActorsFilter_Extras As IEnumerable(Of String)
        Get
            Dim q = From x In ActorDb 
                Group By 
                    x.ActorName Into NumFilms=Count 
                Where 
                    ActorsFilter_AlsoInclude.Contains(IIf(IsNothing(ActorName),"N/A",ActorName))
            
            Return From x In q Select x.ActorName & " (" & x.NumFilms.ToString & ")"
        End Get
    End Property    

    Public ReadOnly Property TagsFilter_Preferences As IEnumerable(Of String)
        Get
            Dim q = From x In TagDb 
                Group By 
                    x.TagName Into NumFilms=Count 
                Where 
                    NumFilms>= Pref.MinTagsInFilter
            
            If Pref.MovFiltersTagsOrder = 0 Then 
                q = From x In q Order by x.NumFilms  Descending, x.TagName Ascending
            Else
                q = From x In q Order by x.TagName Ascending , x.NumFilms  Descending
            End If
            Return From x In q Select x.TagName & " (" & x.NumFilms.ToString & ")" Take Pref.MaxTagsInFilter
        End Get
    End Property    
    
    Public ReadOnly Property TagsFilter As List(Of String)
        Get
            Dim r = (From x In TagsFilter_Preferences).Union(From x In TagsFilter_Extras) 
            Return r.ToList
        End Get
    End Property    
    
    Public ReadOnly Property TagsFilter_Extras As IEnumerable(Of String)
        Get
            Dim q = From x In TagDB 
                Group By 
                    x.TagName Into NumFilms=Count 
                Where 
                    TagsFilter_AlsoInclude.Contains(IIf(IsNothing(TagName),"N/A",TagName))
            
            Return From x In q Select x.TagName & " (" & x.NumFilms.ToString & ")"
        End Get
    End Property    
    
    Public ReadOnly Property DirectorsFilter_Preferences As IEnumerable(Of String)
        Get
            Dim q = From x In DirectorDb 
                Group By 
                    x.ActorName Into NumFilms=Count 
                Where 
                    NumFilms>= Pref.DirectorsFilterMinFilms
                                
            If Pref.MovieFilters_Directors_Order=0 Then 
                q = From x In q Order by x.NumFilms  Descending, x.ActorName Ascending
            Else
                q = From x In q Order by x.ActorName Ascending , x.NumFilms  Descending
            End If

            Return From x In q Select x.ActorName & " (" & x.NumFilms.ToString & ")" Take Pref.MaxDirectorsInFilter
        End Get
    End Property    

    Public ReadOnly Property DirectorsFilter As List(Of String)
        Get
            Dim r = (From x In DirectorsFilter_Preferences).Union(From x In DirectorsFilter_Extras) 
            Return r.ToList
        End Get
    End Property    
    
    Public ReadOnly Property DirectorsFilter_Extras As IEnumerable(Of String)
        Get
            Dim q = From x In DirectorDb 
                Group By 
                    x.ActorName Into NumFilms=Count 
                Where 
                    DirectorsFilter_AlsoInclude.Contains(IIf(IsNothing(ActorName),"N/A",ActorName))
            
            Return From x In q Select x.ActorName & " (" & x.NumFilms.ToString & ")"
        End Get
    End Property  
    
    Public ReadOnly Property TagFilter As List(Of String)
        Get
            Dim q2 = From x In Pref.movietags
           
            Return q2.AsEnumerable.ToList
        End Get
    End Property
    
    Public ReadOnly Property ResolutionFilter As List(Of String)
        Get
            Dim q = From x In MovieCache 
                Group By x.Resolution Into NumFilms=Count 
                Order by Resolution Descending

            Dim r = (From x In q Select x.Resolution & " (" & x.NumFilms.ToString & ")").ToList

            If r.Count>0 Then r.Item(r.Count-1) = r.Item(r.Count-1).Replace("-1","Unknown")

            Return r
        End Get
    End Property    

    Public ReadOnly Property VideoCodecFilter As List(Of String)
        Get
            Dim q = From x In MovieCache 
                Group By x.VideoCodec Into NumFilms=Count 
                Order by Videocodec Descending

            Dim r = (From x In q Select x.VideoCodec & " (" & x.NumFilms.ToString & ")").ToList

            If r.Count>0 Then r.Item(r.Count-1) = r.Item(r.Count-1).Replace("-1","Unknown")

            Return r
        End Get
    End Property

    Public ReadOnly Property NumAudioTracksFilter As List(Of String)
        Get
            Dim q = From m In MovieCache 
                Group By NumTracks=m.Audio.Count Into NumFilms=Count 
                Order By NumTracks

            Dim r = (From x In q Select x.NumTracks & " (" & x.NumFilms.ToString & ")").ToList

            Return r
        End Get
    End Property

    Public ReadOnly Property UserRatedFilter As List(Of String)
        Get
            Dim q = From m In MovieCache 
                Group By NumTracks=m.usrrated Into NumFilms=Count 
                Order By NumTracks

            Dim r = (From x In q Select x.NumTracks & " (" & x.NumFilms.ToString & ")").ToList

            Return r
        End Get
    End Property 

    Public ReadOnly Property LockedFilter As List(Of String)
        Get
            Dim lst = New List(Of String)

            lst.Add( LockedFieldCount("Set") )

            Return lst
        End Get
    End Property 

    Public ReadOnly Property HdrFilter As List(Of String)
        Get
            Dim lst = New List(Of String)

            Dim q = From m In MovieCache Where m.IsHdr

            lst.Add( "HDR (" & q.Count.ToString & ")" )

            Return lst
        End Get
    End Property 
    
    Public ReadOnly Property AudioLanguagesFilter As List(Of String)
        Get
            Dim leftOuterJoinTable As IEnumerable = From m In MovieCache From a In m.Audio Select fullpathandfilename=m.fullpathandfilename, field=If(a.Language.Value="","Unknown",a.Language.Value)

            Return QryMovieCache(leftOuterJoinTable)
        End Get
    End Property    

    Public ReadOnly Property AudioDefaultLanguages As IEnumerable
        Get
            Dim result As IEnumerable = From m In MovieCache Select fullpathandfilename=m.fullpathandfilename, field=If(m.DefaultAudioTrack.Language.Value="","Unknown",m.DefaultAudioTrack.Language.Value)

            Return result
        End Get
    End Property    

    Public ReadOnly Property AudioDefaultLanguagesFilter As List(Of String)
        Get
            Return QryMovieCache(AudioDefaultLanguages)
        End Get
    End Property    
    
    Public ReadOnly Property AudioChannelsFilter As List(Of String)
        Get
            Dim leftOuterJoinTable = From m In MovieCache From a In m.Audio Select fullpathandfilename=m.fullpathandfilename, field=If(a.Channels.Value="","Unknown",a.Channels.Value)

            Return QryMovieCache(leftOuterJoinTable)
        End Get
    End Property    

    Public ReadOnly Property AudioBitratesFilter As List(Of String)
        Get
            Dim leftOuterJoinTable = From m In MovieCache From a In m.Audio Select fullpathandfilename=m.fullpathandfilename, field=If(a.Bitrate.Value="","Unknown",a.Bitrate.Value)

            Return QryMovieCache(leftOuterJoinTable)
        End Get
    End Property    

    Public ReadOnly Property AudioCodecsFilter As List(Of String)
        Get
            Dim leftOuterJoinTable = From m In MovieCache From a In m.Audio Select fullpathandfilename=m.fullpathandfilename, field=If(a.Codec.Value="","Unknown",a.Codec.Value)

            Return QryMovieCache(leftOuterJoinTable)
        End Get
    End Property    
    
    Public ReadOnly Property SetsFilter_Extras As IEnumerable(Of String)
        Get
            Dim q = From x In MovieCache_NoDups 
                Group By 
                    x.SetName Into NumFilms=Count
                Where 
                    SetsFilter_AlsoInclude.Contains(SetName)
            
            Return From x In q Select x.SetName & " (" & x.NumFilms.ToString  & " of " & GetMovieSetCollectionCount(x.SetName) & ")"
        End Get
    End Property    
    
    Public ReadOnly Property SetsFilter_Preferences As IEnumerable(Of String)
        Get
            Dim q = From x In MovieCache_NoDups 
                Group By 
                    x.SetName Into NumFilms=Count
                Where 
                    NumFilms>=Pref.SetsFilterMinFilms 

            If Pref.MovieFilters_Sets_Order=0 Then 
                q = From x In q Order by x.NumFilms Descending, x.SetName Ascending
            Else
                q = From x In q Order by x.SetName.Replace("-None-","") Ascending , x.NumFilms Descending
            End If

            Return From x In q Select x.SetName & " (" & x.NumFilms.ToString & GetMovieSetCollectionCount(x.SetName) & ")" Take Pref.MaxSetsInFilter 
        End Get
    End Property

    Public ReadOnly Property MissingMovie As String
        Get
            Return "Missing Video file (" & (From x In MovieCache Where x.VideoMissing).Count & ")" 
        End Get
    End Property
    
    Public ReadOnly Property MovieCache_NoDups As IEnumerable(Of ComboList)
        Get
		    Dim q = MovieCache.GroupBy(Function(x) x.id).Select(Function(grp) grp.First)

		    Return q.ToList()  
        End Get
    End Property 
    
    Public ReadOnly Property TmDbMovieSetIds As List(Of Integer)
        Get
            Dim q = (From m In MovieCache Where IsNumeric(m.TmdbSetId) Select Convert.ToInt32(m.TmdbSetId)).Distinct()

            Return q.AsEnumerable.ToList
        End Get
    End Property
    
    Public ReadOnly Property SetsFilter As List(Of String)
        Get
            Dim r = (From x In SetsFilter_Preferences).Union(From x In SetsFilter_Extras).ToList

            Dim lstUnknownSetCount = From x In r Where x.EndsWith(" unknown)") Select x.RemoveAfterMatch

            Return r
        End Get
    End Property

    Public ReadOnly Property MissingTmdbMovieSetInfo As String
        Get
            Return "Missing Tmdb movie set info (" & (From x In MovieCache Where x.MissingTmdbMovieSetInfo).Count & ")"
        End Get
    End Property
    
    Public ReadOnly Property UserSetAdditions As String
        Get
            Return "User set additions (" & (From x In MovieCache Where x.UserTmdbSetAddition="Y").Count & ")"
        End Get
    End Property

    Public ReadOnly Property MissingTmdbSetInfo As String
        Get
            Dim q = From x In MovieCache Where x.UnknownSetCount="Y"
            Return "Missing Tmdb set info (" & q.Count & ")"
        End Get
    End Property
    
    Public ReadOnly Property MissingFromSetReleased As String
        Get
            Return "Missing from set released (" & TmdbMissingFromSetReleased.Count & ")"
        End Get
    End Property

    Public ReadOnly Property MissingFromSetUnreleased As String
        Get
            Return "Missing from set unreleased (" & TmdbMissingFromSetUnreleased.Count & ")"
        End Get
    End Property
    
    Public ReadOnly Property TmdbMissingFromSetReleased As List(Of TmdbSetMissingMovie)
        Get
            Dim q = From x In TmdbSetMissingMovies
                        Where
                            IsDate(x.movie.release_date) AndAlso (x.movie.release_date < Date.Now)
                        Select 
                            x

            Return q.ToList
        End Get
    End Property
    
    Public ReadOnly Property TmdbMissingFromSetUnreleased As List(Of TmdbSetMissingMovie)
        Get
            Dim q = From x In TmdbSetMissingMovies
                        Where
                            Not (IsDate(x.movie.release_date) AndAlso (x.movie.release_date < Date.Now))
                        Select 
                            x
            Return q.ToList
        End Get
    End Property
    
    Public ReadOnly Property MoviesSetsIncNone As List(Of String)
        Get
            Try
                Dim q = From x In MovieCache Select ms = x.SetName Distinct 

                Return q.ToList
            Catch
                Return New List(Of String)
            End Try
        End Get
    End Property

    Public ReadOnly Property MoviesSetsExNone As List(Of String)
        Get
            Dim x = MoviesSetsIncNone

            x.Remove("")
            x.Remove("-None-")

            Return x
        End Get
    End Property

    Public ReadOnly Property MovieSetsNoSetId As List(Of String)
        Get
            Try
                Dim q = From x In MovieCache Where x.TmdbSetId = "" AndAlso x.SetName <> "-None-" Select ms = x.SetName Distinct
                Return q.tolist

            Catch ex As Exception
                Return New List(Of String)
            End Try
        End Get
    End Property

    Public ReadOnly Property MovieSetsCustom As String()
        Get
            Dim q = From x In MovieSetDB Where x.TmdbSetId.StartsWith("L") Select ms = x.MovieSetName Distinct
            Return q.ToArray
        End Get
    End Property
    
	Public ReadOnly Property UsedMovieSets As String()
		Get
			Dim resTmdb = From x In MoviesSetsExNone

			Dim resUser = From x In Pref.moviesets Where x <> "-None-" Select name = If(Pref.MovSetTitleIgnArticle, Pref.RemoveIgnoredArticles(x),x)

			Dim res = resTmdb.Union(resUser).OrderBy(Function(x) x) '.ToArray
            Dim res2 = res.Union(MovieSetsCustom).OrderBy(Function(x) x).ToArray
            
			Return res2
		End Get

	End Property
    
    Public ReadOnly Property SubTitleLangFilter As List(Of String)
        Get
            Dim leftOuterJoinTable As IEnumerable = From m In MovieCache From a In m.SubLang Select fullpathandfilename=m.fullpathandfilename, field = If(a.Language.Value = "", "Unknown", a.Language.Value)

            Return QryMovieCache(leftOuterJoinTable)
        End Get
    End Property

    Public ReadOnly Property RootFolderFilter As List(Of String)
        Get
            Dim q = From x In MovieCache
                    Group By x.rootfolder Into NumFilms = Count
                    Order By rootfolder Ascending

            Dim r = (From x In q Select x.rootfolder & " (" & x.NumFilms.ToString & ")").ToList

            Return r
        End Get
    End Property
    
    Public ReadOnly Property ActorDb As List(Of Databases)
        Get
            Return _actorDb
        End Get
    End Property

    Public ReadOnly Property DirectorDb As List(Of DirectorDatabase)
        Get
            Return _directorDb
        End Get
    End Property

    Public ReadOnly Property MovieSetDB As List(Of MovieSetInfo)
        Get
            Return _moviesetDb
        End Get
    End Property
    
    Public ReadOnly Property TmdbSetMissingMovies As List(Of TmdbSetMissingMovie)
        Get
            Return _tmdbSetMissingMovies
        End Get
    End Property
    
    Public ReadOnly Property TagDB As List(Of TagDatabase)
        Get
            Return _tagDb
        End Get
    End Property

    Public ReadOnly Property Cancelled As Boolean
        Get
            Application.DoEvents()

            If Not IsNothing(_Bw) AndAlso _Bw.WorkerSupportsCancellation AndAlso _Bw.CancellationPending Then
                ReportProgress("Cancelled!", vbCrLf & "!!! Operation cancelled by user")
                Return True
            End If
            Return False
        End Get
    End Property
#End Region

    Function LockedFieldCount(field As String) As String

         Dim q = From m In MovieCache Where m.LockedFields.Contains(field.ToLower) 

         Return field & " (" & q.Count.ToString & ")"
    End Function  
    
    Private Function QryMovieCache(leftOuterJoinTable As IEnumerable) As List(Of String)
        Dim q = From m In MovieCache
                    Group Join 
                        a In leftOuterJoinTable On a.fullpathandfilename Equals m.fullpathandfilename
                    Into
                        ResultList = Group
                    From
                        result In ResultList.DefaultIfEmpty
                    Select 
                        field=If( result Is Nothing, "Unassigned", result.field )
                    Group By 
                        field Into Num=Count
                    Order By 
                        field
                    Select 
                        aString = field & " (" & Num.ToString & ")"

        Return q.Cast(Of String)().ToList
    End Function
    
    Function GetMovieSetCollectionCount(SetName As String) As String

        If SetName="-None-" Then Return ""

        Dim movieSet = FindMovieSetInfoBySetDisplayName(SetName)
 
        Dim x = FindUserTmdbSetAdditions(SetName)
        Dim userAdditions = ""
        If x.Count>0 Then userAdditions = " *" & x.Count.ToString & " user*"

        If IsNothing(movieSet) OrElse movieSet.MissingInfo Then
            Return " of unknown"
        Else
            Dim r = From m In movieSet.Collection Where IsDate(m.release_date) AndAlso (m.release_date < Date.Now) Select m
            Return " of " & (r.Count+x.Count) & userAdditions
        End If
    End Function
    
    Function FindMovieSetInfoBySetDisplayName(SetName As String) As MovieSetInfo
        
        Dim res = (From x In MovieSetDB Where x.MovieSetDisplayName = SetName Select x).FirstOrDefault

        If IsNothing(res) Then
            Return New MovieSetInfo
        Else
            Return res
        End If

    End Function

    Function FindMovieSetInfoBySetName(SetName As String) As MovieSetInfo
        
        Dim res = (From x In MovieSetDB Where x.MovieSetName = SetName Select x).FirstOrDefault

        If IsNothing(res) Then
            Return New MovieSetInfo
        Else
            Return res
        End If

    End Function
    
    Function FindMovieSetInfoByTmdbSetId(TmdbSetId As String) As MovieSetInfo

        Return (From x In MovieSetDB Where x.TmdbSetId=TmdbSetId).FirstOrDefault

    End Function
    
    Function FindUserTmdbSetAdditions(SetName As String) As IEnumerable(Of MovieSetInfo)
        Return (From x In MovieCache Where x.SetName = SetName and x.UserTmdbSetAddition="Y" Select x.MovieSet)
    End Function
    
    
    Sub ActorsFilter_AddIfMissing(value As String)
        If Not ActorsFilter.Any(Function(x) x.StartsWith(value)) Then
            ActorsFilter_AlsoInclude.Add(value)
        End If
    End Sub

    Sub DirectorsFilter_AddIfMissing(value As String)
        If Not DirectorsFilter.Any(Function(x) x.StartsWith(value)) Then
            DirectorsFilter_AlsoInclude.Add(value)
        End If
    End Sub

    Sub SetsFilter_AddIfMissing(name As String)

        If Not SetsFilter.Any(Function(x) x.StartsWith(name)) Then
            SetsFilter_AlsoInclude.Add(name)
        End If
    End Sub

    Sub TagsFilter_AddIfMissing(value As String)
        If Not TagsFilter.Any(Function(x) x.StartsWith(value)) Then
            TagsFilter_AlsoInclude.Add(value)
        End If
    End Sub

    'Movies missing from a Set
    '-------------------------
    'From list Of movies
    '	Select Case Distinct Set ids
    'For Each set In list Of Set ids
    '	For Each movie
    '		Got Y/N
    '		N -> Add to Results
    Public Sub UpdateTmdbSetMissingMovies
        _tmdbSetMissingMovies.Clear

        For Each mset In MovieSetDB
            For Each movie In mset.Collection
                    
            '    If IsDate(movie.release_date) AndAlso (movie.release_date < Date.Now) Then

                    Dim q = From x In MovieCache Where x.tmdbid = movie.TmdbMovieId 

                    If q.Count = 0 Then
                        _tmdbSetMissingMovies.Add(New TmdbSetMissingMovie(mset, movie, Me))
                    End If
           '     End If
            Next
        Next
    End Sub

    Public Sub UpdateSetsWithOverview
        For each m In MovieCache
            If m.TmdbSetId = "" Then Continue For
            Dim q = From x In MovieSetDB Where x.TmdbSetId = m.TmdbSetId
            If Not q.Count = 0 Then
                If m.MovieSet.MovieSetPlot = "" AndAlso q(0).MovieSetPlot <> "" Then m.MovieSet.MovieSetPlot = q(0).MovieSetPlot
            End If
        Next
    End Sub
    
    Public Sub AssignUnknownUserTmdbSetAdditions

        Dim lst = From x In MovieCache Where x.UserTmdbSetAddition=""

        For Each movie In lst
            movie.UserTmdbSetAddition = "N"

            If movie.SetName <> "-None-" AndAlso Not movie.MovieSet.MissingInfo Then
                Try
                    Dim q2 = From x In movie.MovieSet.Collection Where x.TmdbMovieId = movie.tmdbid

                    If q2.Count = 0 Then
                        movie.UserTmdbSetAddition = "Y"
                    End If
                Catch e As Exception
                    dim yy = e
                End Try
            End If
        Next
    End Sub
    
    Public Sub RebuildUnknownSetCount
        Dim lst = From x In MovieCache 'Where x.UnknownSetCount="" Select x

        For Each movie In lst
            movie.UnknownSetCount = "N"

            If movie.SetName = "-None-" Then
                Continue For
            End If

            If movie.MovieSet.MissingInfo Then
                movie.UnknownSetCount = "Y"
            End If
        Next
    End Sub
    
    Sub AddUpdateMovieSetInCache(movieSetInfo As MovieSetInfo, Optional ByVal Update As Boolean = False, Optional ByVal custom As Boolean = False)

        If IsNothing(movieSetInfo) Then Return

        Dim c As MovieSetInfo = Nothing
        Try
            c = FindMovieSetInfoByTmdbSetId(movieSetInfo.TmdbSetId)
        Catch ex As Exception
        End Try

        If IsNothing(c) AndAlso custom Then
            Try
                c = FindMovieSetInfoBySetName(movieSetInfo.MovieSetName)
                If movieSetInfo.TmdbSetId <> "" AndAlso c.TmdbSetId.StartsWith("L") Then c.Dirty = True
            Catch ex As Exception
            End Try
        End If

        If IsNothing(c) Then
            MoviesetDb.Add(movieSetInfo)
            Return
        End If
        If Not IsNothing(c) Then
            If c.Dirty OrElse Update Then
                MovieSetDB.Remove(c)
                MovieSetDB.Add(movieSetInfo)
            End If
        End If

        c.Assign(movieSetInfo)
    End Sub

    Sub RemoveMovieSetInCache(s As String)
        If String.IsNullOrEmpty(s) Then Return
        Dim c As MovieSetInfo = Nothing
        Try
            c = FindMovieSetInfoBySetName(s)
            If c.MovieSetName <> "" Then MovieSetDB.Remove(c)
        Catch ex As Exception

        End Try
        'MovieSetDB.Remove(movieSetInfo)
    End Sub

    Sub UpdateMovieCacheSetName(MovieSet As MovieSetInfo)

        Dim res = (From x In MovieCache Where x.TmdbSetId=MovieSet.TmdbSetId)

        For Each m In res
            
			Dim movie As Movie = LoadMovie(m.fullpathandfilename)

            movie.ScrapedMovie.fullmoviebody.SetName        = m.SetName
            movie.ScrapedMovie.fullmoviebody.TmdbSetId      = m.TmdbSetId
            
            movie.AssignMovieToCache
            movie.UpdateMovieCache
            movie.SaveNFO
            
        Next
    End Sub
    
    Public Function GetMovieSetIdFromName(mSetName As String) As String
        For Each mset In MovieSetDB
            If mset.MovieSetName = mSetName Then
                Return mset.TmdbSetId
            End If
        Next
        Return ""
    End Function

    Public Function GetMovieSetOverviewFromName(mSetName As String) As String
        For Each mset In MovieSetDB
            If mset.MovieSetName = mSetName Then
                Return mset.MovieSetPlot
            End If
        Next
        Return ""
    End Function
    
    ''' <summary>
    ''' Make sure no Movie Set has an empty ID. If we can't find an ID on tmdb, we still need to distinguish the set
    ''' from other sets. Comparing movie names isn't the proper way, as users in Media Companion can choose to rename
    ''' their movies manually not corresponding to online names so we rather check for an ID (online or local)
    ''' 
    ''' </summary>
    ''' <param name="moviesetID"></param>
    ''' <returns></returns>
    Function setMovieSetID(moviesetID As String, setDb As List(Of MovieSetInfo)) As String
        If moviesetID = String.Empty Then   ' if no TmdbID is found set it to a local ID to avoid empty ID's
            Dim moviesetTemp As MovieSetInfo
            Dim moviesetTempHighestID = 0
            For Each moviesetTemp In setDb
                If moviesetTemp.TmdbSetId.Chars(0) = "L" Then
                    Dim moviesetTempID = moviesetTemp.TmdbSetId.Substring(1).ToInt()
                    If moviesetTempID + 1 > moviesetTempHighestID Then
                        moviesetTempHighestID = moviesetTempID + 1
                    End If
                End If
            Next
            moviesetID = "L" + moviesetTempHighestID.ToString().PadLeft(5 - moviesetTempHighestID.ToString().Length, "0")
        End If
        Return moviesetID
    End Function
    
    Public Sub UpdateMovieSetDisplayNames

        For Each m In MovieCache 
            m.MovieSet.UpdateMovieSetDisplayName
        Next 

    End Sub

    Public Sub UpdateUserMovieSetName

    End Sub

    Public Sub Rebuild_Data_GridViewMovieCache()

'       UpdateUserTmdbSetAdditions()        

        _data_GridViewMovieCache.Clear()

        For Each item In MovieCache
            _data_GridViewMovieCache.Add(New Data_GridViewMovie(item))
        Next
    End Sub

    Sub New(Optional bw As BackgroundWorker = Nothing)
        _Bw = bw

        AddHandler Me.XbmcMcMoviesChanged, AddressOf Handle_XbmcMcMoviesChanged
        AddHandler Me.XbmcOnlyMoviesChanged, AddressOf Handle_XbmcOnlyMoviesChanged

    End Sub

    Sub newMovie_AmountDownloadedChanged(ByVal iNewProgress As Long)
        RaiseEvent AmountDownloadedChanged(iNewProgress)
    End Sub

    Sub newMovie_FileDownloadSizeObtained(ByVal iFileSize As Long)
        RaiseEvent FileDownloadSizeObtained(iFileSize)
    End Sub

    Sub newMovie_FileDownloadComplete()
        RaiseEvent FileDownloadComplete()
    End Sub

    Sub newMovie_FileDownloadFailed(ByVal ex As Exception)
        RaiseEvent FileDownloadFailed(ex)
    End Sub

    Sub ReportProgress(Optional progressText As String = Nothing, Optional log As String = Nothing, Optional command As Progress.Commands = Progress.Commands.SetIt)
        ReportProgress(New Progress(progressText, log, command))
    End Sub

    Sub ReportProgress(ByVal oProgress As Progress)
        If Not IsNothing(_Bw) AndAlso _Bw.WorkerReportsProgress AndAlso Not (String.IsNullOrEmpty(oProgress.Log) And String.IsNullOrEmpty(oProgress.Message)) Then
            Try
                _Bw.ReportProgress(PercentDone, oProgress)
            Catch
            End Try
        End If
    End Sub

    Public Function FindCachedMovie(fullpathandfilename As String) As ComboList
        Dim q = From m In _MovieCache Where m.fullpathandfilename = fullpathandfilename
        If q.Count = 0 Then Return Nothing
        Return q.Single
    End Function
    
    Public Function FindData_GridViewCachedMovie(fullpathandfilename As String) As Data_GridViewMovie
        Dim q = From m In _data_GridViewMovieCache Where m.fullpathandfilename = fullpathandfilename
        Return q.Single
    End Function

    Public Function LoadMovie(fullpathandfilename As String, Optional ByVal Cacheupdate As Boolean = True) As Movie
        Dim movie = New Movie(Me, fullpathandfilename)
        If IsNothing(movie) Then Return Nothing
        movie.LoadNFO(Cacheupdate)
        Return movie
    End Function
    
    Public Sub FindNewMovies(Optional scrape = True)
        NewMovies.Clear()
        PercentDone = 0

        Dim folders As New List(Of String)

        AddOnlineFolders(folders, Pref.movieFolders)
        AddOfflineFolders(folders)

        Dim i = 0
        For Each folder In folders
            i += 1
            PercentDone = CalcPercentDone(i, folders.Count)
            ReportProgress("Scanning folder " & i & " of " & folders.Count)

            AddNewMovies(folder)

            If Cancelled Then
                NewMovies.Clear()
                Exit Sub
            End If
        Next

        If scrape Then ScrapeNewMovies()
    End Sub

    Private Sub AddMovieEventHandlers(oMovie As Movie)
        AddHandler oMovie.ProgressLogChanged, AddressOf ReportProgress
        AddHandler oMovie.AmountDownloadedChanged, AddressOf newMovie_AmountDownloadedChanged
        AddHandler oMovie.FileDownloadSizeObtained, AddressOf newMovie_FileDownloadSizeObtained
        AddHandler oMovie.FileDownloadComplete, AddressOf newMovie_FileDownloadComplete
        AddHandler oMovie.FileDownloadFailed, AddressOf newMovie_FileDownloadFailed
    End Sub

    Private Sub RemoveMovieEventHandlers(oMovie As Movie)
        RemoveHandler oMovie.ProgressLogChanged, AddressOf ReportProgress
        RemoveHandler oMovie.AmountDownloadedChanged, AddressOf newMovie_AmountDownloadedChanged
        RemoveHandler oMovie.FileDownloadSizeObtained, AddressOf newMovie_FileDownloadSizeObtained
        RemoveHandler oMovie.FileDownloadComplete, AddressOf newMovie_FileDownloadComplete
        RemoveHandler oMovie.FileDownloadFailed, AddressOf newMovie_FileDownloadFailed
    End Sub

    Public Sub AddOnlineFolders(folders As List(Of String), searchfolders As List(Of str_RootPaths))
        For Each moviefolder In searchfolders 'Pref.movieFolders
            If Not moviefolder.selected Then Continue For
            If Not moviefolder.IncludeInSearchForNew Then Continue For

            Dim dirInfo As New DirectoryInfo(moviefolder.rpath)

            If dirInfo.Exists Then
                folders.Add(moviefolder.rpath)
                ReportProgress("Searching movie Folder: " & dirInfo.FullName.ToString & vbCrLf)
                Try
                    For Each subfolder In Utilities.EnumerateFolders(moviefolder.rpath)       'Max levels restriction of 6 deep removed
                        folders.Add(subfolder)
                    Next
                Catch ex As Exception
                    ExceptionHandler.LogError(ex, "LastRootPath: [" & Utilities.LastRootPath & "]")
                End Try
            End If

            If Cancelled Then Exit Sub
        Next
    End Sub

    Public Sub AddOnlineFolders(folders As List(Of String), searchfolders As List(Of String))
        For Each moviefolder In searchfolders 'Pref.movieFolders
            Dim dirInfo As New DirectoryInfo(moviefolder)

            If dirInfo.Exists Then
                folders.Add(moviefolder)
                ReportProgress("Searching movie Folder: " & dirInfo.FullName.ToString & vbCrLf)
                Try
                    For Each subfolder In Utilities.EnumerateFolders(moviefolder)       'Max levels restriction of 6 deep removed
                        folders.Add(subfolder)
                    Next
                Catch ex As Exception
                    ExceptionHandler.LogError(ex, "LastRootPath: [" & Utilities.LastRootPath & "]")
                End Try
            End If

            If Cancelled Then Exit Sub
        Next
    End Sub

    Public Sub AddOfflineFolders(folders As List(Of String))
        For Each moviefolder In Pref.offlinefolders

            Dim dirInfo As New DirectoryInfo(moviefolder)

            If dirInfo.Exists Then
                ReportProgress(, "Found Offline Folder: " & dirInfo.FullName.ToString & vbCrLf & "Checking for subfolders" & vbCrLf)

                For Each subfolder In Utilities.EnumerateFolders(moviefolder, 0)

                    Dim DummyFileName As String = Utilities.GetLastFolder(subfolder & "\whatever") & ".avi"
                    Dim DummyFullName As String = Path.Combine(subfolder, DummyFileName)
                    Dim NfoFullName As String = DummyFullName.Replace(Path.GetExtension(DummyFullName), ".nfo")

                    If Not File.Exists(NfoFullName) Then

                        'Create temporary "tempoffline.ttt" file
                        If Not File.Exists(Path.Combine(subfolder, "tempoffline.ttt")) Then
                            Dim sTempFileName As String = Path.Combine(subfolder, "tempoffline.ttt")
                            Dim fsTemp As IO.FileStream = File.Open(sTempFileName, IO.FileMode.Create)
                            fsTemp.Close()
                        End If

                        'Create temporary "whatever.avi"' file
                        If Not File.Exists(DummyFullName) Then
                            Dim fsTemp2 As IO.FileStream = File.Open(DummyFullName, IO.FileMode.Create)
                            fsTemp2.Close()
                        End If

                        folders.Add(subfolder)
                    End If
                Next

            End If
        Next
    End Sub

    Public Sub AddNewMovies(DirPath As String)   'Search for valid video file

        If Pref.ExcludeFolders.Match(DirPath) Then
            ReportProgress(, "Skipping excluded folder [" & DirPath & "] from scrape." & vbCrLf)
            Return
        End If

        Dim dirInfo As New DirectoryInfo(DirPath)
        Dim found As Integer = 0

        For Each ext In Utilities.VideoExtensions
            ext = If((ext = "video_ts.ifo" OrElse ext = "vr_mangr.ifo"), ext, "*" & ext)
            Try
                For Each Filefound As FileInfo In dirInfo.GetFiles(ext)
                    If Not ValidateFile(Filefound) Then
                        Continue For
                    End If
                    found += 1
                    NewMovies.Add(New Movie(Filefound.FullName, Me))
                Next
            Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
            End Try
        Next
        If found > 0 Then
            ReportProgress(, String.Format("{0} new movie{1} found in [{2}]", found, If(found = 1, "", "s"), DirPath) & vbCrLf)
            Pref.DoneAMov = True
        End If
    End Sub

    Public Sub ScrapeFiles(files As List(Of String))
        AddNewMovies(files)
        ScrapeNewMovies()
        files.Clear()
    End Sub

    Public Sub AddNewMovies(files As List(Of String))
        NewMovies.Clear()
        PercentDone = 0

        Dim i = 0
        Dim found = 0
        Dim msg = ""
        For Each file In files
            Dim fileInfo = New FileInfo(file)
            i += 1
            PercentDone = CalcPercentDone(i, files.Count)
            msg = "!!! Validating file " & fileInfo.Name & "(" & i & " of " & files.Count & ")"
            ReportProgress(msg, msg & vbCrLf)
            If Not ValidateFile(fileInfo) Then
                Continue For
            End If
            found += 1
            NewMovies.Add(New Movie(fileInfo.FullName, Me))
            If Cancelled Then Exit Sub
        Next

    End Sub

    Sub ScrapeNewMovies()
        If NewMovies.Count > 0 Then
            ReportProgress(, vbCrLf & vbCrLf & "!!! A total of " & NewMovies.Count & " new movie" & If(NewMovies.Count = 1, "", "s") & " found -> Starting Main Scraper Process..." & vbCrLf & vbCrLf)
        Else
            ReportProgress(vbCrLf & vbCrLf & "No new movies found" & vbCrLf & vbCrLf, vbCrLf & vbCrLf & "!!! No new movies found" & vbCrLf & vbCrLf)
        End If

        Dim i = 0
        For Each newMovie In NewMovies
            i += 1
            PercentDone = CalcPercentDone(i, NewMovies.Count)
            ReportProgress("Scraping " & i & " of " & NewMovies.Count)
            If Pref.MusicVidScrape AndAlso Pref.MVConcertFolders.Count > 0 Then
                Pref.MusicVidConcertScrape = False
                For Each concertpath In Pref.MVConcertFolders
                    If Not concertpath.selected Then Continue For
                    If newMovie.NfoPath.ToLower.Contains(concertpath.rpath.ToLower & If(concertpath.rpath.Contains("\"), "\", "/")) Then
                        Pref.MusicVidConcertScrape = True
                        Exit For
                    End If
                Next
            End If
            ScrapeMovie(newMovie)
            Pref.MusicVidConcertScrape = False
            Pref.googlecount += 1
            Pref.engineno += 1
            If Pref.engineno = Pref.enginefront.Count Then Pref.engineno = 0
            If newMovie.TimingsLog <> "" Then
                ReportProgress(, vbCrLf & "Timings" & vbCrLf & "=======" & newMovie.TimingsLog & vbCrLf & vbCrLf)
            End If
            If Cancelled Then Exit Sub
        Next
        Pref.googlecount = 0
        ReportProgress(, "!!! " & vbCrLf & "!!! Finished")
    End Sub

    Sub ScrapeMovie(movie As Movie)
        AddMovieEventHandlers(movie)
        movie.Scraped = False
        movie.Scrape()
        RemoveMovieEventHandlers(movie)
    End Sub

    Sub ChangeMovie(NfoPathAndFilename As String, ChangeMovieId As String, MovieSearchEngine As String)
        Pref.MovieChangeMovie = True
        Dim movie = New Movie(Me, NfoPathAndFilename)

        movie.DeleteScrapedFiles(True)

        If Not Pref.MusicVidScrape Then
            movie.ScrapedMovie.Init()
        Else
            Pref.MusicVidConcertScrape = False
            For Each concertpath In Pref.MVConcertFolders
                If Not concertpath.selected Then Continue For
                If movie.NfoPath.ToLower.Contains(concertpath.rpath.ToLower & If(concertpath.rpath.Contains("\"), "\", "/")) Then
                    Pref.MusicVidConcertScrape = True
                    Exit For
                End If
            Next
        End If

        AddMovieEventHandlers(movie)
        movie.Scraped = False
        movie.MovieSearchEngine = MovieSearchEngine
        movie.Scrape(ChangeMovieId)
        RemoveMovieEventHandlers(movie)
    End Sub

    Sub RescrapeSpecificMovie(fullpathandfilename As String, rl As RescrapeList)

        Dim movie = New Movie(Me, fullpathandfilename)

        AddMovieEventHandlers(movie)
        movie.Scraped = False
        movie.RescrapeSpecific(rl)
        RemoveMovieEventHandlers(movie)
    End Sub
    
    Sub RescrapeAll(NfoFilenames As List(Of String))
        Dim i = 0
        ReportProgress(, "!!! Rescraping all data for:" & vbCrLf & vbCrLf)
        For Each NfoFilename In NfoFilenames
            i += 1
            PercentDone = CalcPercentDone(i, NfoFilenames.Count)
            ReportProgress("Rescraping " & i & " of " & NfoFilenames.Count & " ")
            RescrapeMovie(NfoFilename)
            If Cancelled Then Exit For
        Next
        SaveCaches()
        ReportProgress(, "!!! " & vbCrLf & "!!! Finished")
    End Sub

    Sub RescrapeSpecific(_rescrapeList As RescrapeSpecificParams)
        Dim rl As New RescrapeList(_rescrapeList.Field)
        Dim i = 0
        For Each FullPathAndFilename In _rescrapeList.FullPathAndFilenames
            i += 1
            PercentDone = CalcPercentDone(i, _rescrapeList.FullPathAndFilenames.Count)
            ReportProgress("Rescraping '" & Utilities.TitleCase(_rescrapeList.Field.Replace("_", " ")) & "' " & i & " of " & _rescrapeList.FullPathAndFilenames.Count & " ")
            RescrapeSpecificMovie(FullPathAndFilename, rl)
            If Cancelled Then Exit For
        Next
        SaveCaches()
    End Sub
    
    Sub SetFieldLockSpecific(_lockList As LockSpecificParams)

        Dim i = 0
        Dim action As String = "Locking"

        If Not _lockList.Lock Then
            action = "Unlocking"
        End If

        For Each FullPathAndFilename In _lockList.FullPathAndFilenames
            i += 1
            PercentDone = CalcPercentDone(i, _lockList.FullPathAndFilenames.Count)
            ReportProgress( action & " '" & Utilities.TitleCase(_lockList.Field.Replace("_", " ")) & "' " & i & " of " & _lockList.FullPathAndFilenames.Count & " ")
  
            Dim movie = New Movie(Me, fullpathandfilename)

            movie.SetFieldLockSpecific(_lockList.Field, _lockList.Lock)

            If Cancelled Then Exit For
        Next
        SaveCaches()
    End Sub
    
    Sub BatchRescrapeSpecific(NfoFilenames As List(Of String), rl As RescrapeList)
        Dim i = 0
        Dim NfoFileList As New List(Of String)

        For Each item In NfoFilenames
            If File.Exists(item) Then
                NfoFileList.Add(item)
            Else
                ReportProgress("Could Not find " & item & vbCrLf & "Please Refresh All Movies before running Batch Rescraper Wizard")
            End If
        Next

        If NfoFilenames.Count <> NfoFileList.Count Then NfoFilenames = NfoFileList
        For Each item In NfoFilenames
            i += 1
            PercentDone = CalcPercentDone(i, NfoFilenames.Count)
            Dim movie = New Movie(Me, item)
            AddMovieEventHandlers(movie)
            ReportProgress("Batch Rescraping " & i & " of " & NfoFilenames.Count & " [" & movie.Title & "] ")
            movie.Scraped = False
            movie.RescrapeSpecific(rl)
            RemoveMovieEventHandlers(movie)
            If Cancelled Then Exit For
        Next
        SaveCaches()
    End Sub

    Sub RescrapeMovie(NfoFilename As String, Optional ByVal tmdbid As String = "")
        If Not File.Exists(NfoFilename) Then
            ReportProgress("NFO not found : [" & NfoFilename & "]  ")
            Return
        End If

        Dim movie = New Movie(Me, NfoFilename)

        movie.Rescrape = True

        Dim imdbid As String = movie.PossibleImdb
        If Pref.movies_useXBMC_Scraper AndAlso tmdbid <> "" Then
            imdbid = tmdbid
        End If
        movie.DeleteScrapedFiles(False)

        movie.ScrapedMovie.Init()

        AddMovieEventHandlers(movie)
        movie.Scraped = False
        movie.Scrape(imdbid)
        RemoveMovieEventHandlers(movie)
    End Sub

    Function CapsFirstLetter(words As String)
        Return Form1.MyCulture.TextInfo.ToTitleCase(words)
    End Function

    Function CalcPercentDone(onNumber As Integer, total As Integer) As Integer
        Try
            If total = 0 Then total = onNumber
            Return Math.Min((100 / total) * onNumber, 100)
        Catch
            Return 1
        End Try
    End Function

    Public Function ValidateFile(fileInFo As FileInfo)
        If AlreadyAdded(fileInFo.FullName) Then
            ReportProgress(, " - Already Added!")
            Return False
        End If
        Dim log = ""
        Dim valid = Movie.IsValidMovieFile(fileInFo, log)
        ReportProgress(log)
        Return valid
    End Function

    Public Function AlreadyAdded(fullName As String) As Boolean
        Dim q = From m In NewMovies Where m.NfoPathAndFilename.ToLower = fullName.ToLower
        Return (q.Count > 0)
    End Function

    Private Function validateMusicVideoNfo(ByVal fullPathandFilename As String)
        Dim tempstring As String
        Using filechck As IO.StreamReader = File.OpenText(fullPathandFilename)
            tempstring = filechck.ReadToEnd.ToLower
        End Using
        If tempstring = Nothing Then Return False
        If tempstring.IndexOf("<musicvideo>") <> -1 And tempstring.IndexOf("</musicvideo>") <> -1 And tempstring.IndexOf("<title>") <> -1 And tempstring.IndexOf("</title>") <> -1 Then
            Return True
            Exit Function
        End If
        Return False
    End Function

    Public Sub LoadCaches()
        LoadMovieCache()
        LoadPeopleCaches()
        LoadMovieSetCache()
        RebuildUnknownSetCount()
        LoadTagCache()
        UpdateTmdbSetMissingMovies()
        UpdateSetsWithOverview()
        'UpdateUserTmdbSetAdditions()
        Rebuild_Data_GridViewMovieCache()
    End Sub

    Public Sub SaveCaches
        SaveMovieCache
        SaveMovieSetCache
        SaveActorCache
        SaveDirectorCache
'       UpdateUserTmdbSetAdditions()
    End Sub

    Public Sub LoadMovieCache

        MovieCache.Clear

        Dim movielist  As New XmlDocument
        Dim objReader  As New IO.StreamReader(Pref.workingProfile.MovieCache)
        Dim tempstring As String = objReader.ReadToEnd
        objReader.Close
        objReader = Nothing

        movielist.LoadXml(tempstring)

        Try
            For Each thisresult As XmlNode In movielist("movie_cache")
                Select Case thisresult.Name
                    Case "movie"
                        Dim newmovie As New ComboList
                        newmovie.oMovies = Me
                        
                        For Each detail As XmlNode In thisresult.ChildNodes
                            Select Case detail.Name
                                Case "missingdata1"         : newmovie.missingdata1 = Convert.ToByte(detail.InnerText)
                                Case "source"               : newmovie.source = detail.InnerText
                                Case "director"             : newmovie.director = detail.InnerText
                                Case "credits"              : newmovie.credits = detail.InnerText
                                Case "set"                  : newmovie.SetName = detail.InnerText
                                Case "setid"                : newmovie.TmdbSetId = detail.InnerText
                                Case "sortorder"            : newmovie.sortorder = detail.InnerText
                                Case "filedate"
                                    If detail.InnerText.Length <> 14 Then 'i.e. invalid date
                                        newmovie.filedate = "18500101000000" '01/01/1850 00:00:00
                                    Else
                                        newmovie.filedate = detail.InnerText
                                    End If
                                Case "createdate"
                                    If detail.InnerText.Length <> 14 Then 'i.e. invalid date
                                        newmovie.createdate = "18500101000000" '01/01/1850 00:00:00
                                    Else
                                        newmovie.createdate = detail.InnerText
                                    End If
                                Case "tag"                  : newmovie.movietag.Add(detail.innertext)
                                Case "filename"             : newmovie.filename = detail.InnerText
                                Case "foldername"           : newmovie.foldername = detail.InnerText
                                Case "fullpathandfilename"  : newmovie.fullpathandfilename = detail.InnerText
                                Case "genre"                : newmovie.genre = detail.InnerText & newmovie.genre
                                Case "id"                   : newmovie.id = detail.InnerText
                                Case "tmdbid"               : newmovie.tmdbid = detail.InnerText
                                Case "playcount"            : newmovie.playcount = detail.InnerText
                                Case "rating"               : newmovie.rating = detail.InnerText.ToString.ToRating
                                Case "usrrated"             : newmovie.usrrated = detail.InnerText.toInt
                                Case "metascore"            : newmovie.metascore = detail.InnerText.toInt
                                Case "title"                : newmovie.title = detail.InnerText
                                Case "originaltitle"        : newmovie.originaltitle = detail.InnerText
                                Case "top250"               : newmovie.top250 = detail.InnerText
                                Case "year"                 : newmovie.year = detail.InnerText
                                Case "outline"              : newmovie.outline = detail.InnerText
                                Case "plot"                 : newmovie.plot = detail.InnerText
                                Case "tagline"              : newmovie.tagline = detail.InnerText
                                Case "runtime"              : newmovie.runtime = detail.InnerText
                                Case "stars"                : newmovie.stars = detail.InnerText 
                                Case "votes"
                                    Try
                                        newmovie.Votes = detail.InnerText
                                    Catch
                                        newmovie.Votes = 0
                                    End Try
                                Case "countries"
                                    Dim TmpStr As String = detail.InnerText
                                    newmovie.countries = TmpStr.Replace(", ", " / ")
                                Case "studios"
                                    Dim TmpStr As String = detail.InnerText
                                    newmovie.studios = TmpStr.Replace(", ", " / ")
                                Case "Resolution"           : newmovie.Resolution = detail.InnerText
                                Case "VideoCodec"           : newmovie.VideoCodec = detail.InnerText
                                Case "Container"            : newmovie.Container = detail.InnerText
                                Case "audio"
                                    Dim audio As New AudioDetails
                                    For Each audiodetails As XmlNode In detail.ChildNodes
                                        Select Case audiodetails.Name
                                            Case "language"
                                                audio.Language.Value = audiodetails.InnerText
                                            Case "codec"
                                                audio.Codec.Value = audiodetails.InnerText
                                            Case "channels"
                                                audio.Channels.Value = audiodetails.InnerText
                                            Case "bitrate"
                                                audio.Bitrate.Value = audiodetails.InnerText
                                        End Select
                                    Next
                                    newmovie.Audio.Add(audio)
                                Case "Premiered"            : newmovie.Premiered = detail.InnerText
                                Case "Certificate"          : newmovie.Certificate = detail.InnerText
                                Case "FrodoPosterExists"    : newmovie.FrodoPosterExists = detail.InnerText
                                Case "PreFrodoPosterExists" : newmovie.PreFrodoPosterExists = detail.InnerText
                                Case "subtitlelang"
                                    Dim subtitle As New SubtitleDetails
                                    subtitle.Language.Value = detail.InnerText
                                    newmovie.SubLang.Add(subtitle)
                                Case "FolderSize" : 
                                    Try
                                        newmovie.FolderSize = detail.InnerText
                                    Catch
                                        newmovie.FolderSize = -1
                                    End Try 

                                Case "MediaFileSize" : 
                                    Try
                                        newmovie.MediaFileSize = detail.InnerText
                                    Catch
                                        newmovie.MediaFileSize = -1
                                    End Try 
												        												                               
                                Case "RootFolder"           : newmovie.rootfolder          = detail.InnerText
                                Case "UserTmdbSetAddition"  : newmovie.UserTmdbSetAddition = detail.InnerText
                                Case "UnknownSetCount"      : newmovie.UnknownSetCount     = detail.InnerText
                                Case "LockedFields"         : newmovie.LockedFields        = detail.InnerText.Split(",").ToList()
                                Case "VideoMissing"         : newmovie.VideoMissing        = detail.InnerXml

                                Case "NumVideoBits" : 
                                    Try
                                        newmovie.NumVideoBits = detail.InnerText
                                    Catch
                                        newmovie.NumVideoBits = -1
                                    End Try 

       

                            End Select
                        Next

                        If newmovie.SetName = "" Then
                            newmovie.SetName = "-None-"
                        End If

                        MovieCache.Add(newmovie)
                End Select
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        
    End Sub
    
    Public Sub SaveMovieCache
        Dim cacheFile As String = Pref.workingProfile.MovieCache
        If File.Exists(cacheFile) Then
            File.Delete(cacheFile)
        End If
        Dim doc      As New XmlDocument
        Dim xmlproc  As XmlDeclaration
        Dim root  As XmlElement
        Dim child As XmlElement
        'Dim childchild As XmlElement
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        
        root = doc.CreateElement("movie_cache")
        For Each movie In MovieCache
            ''' fix couple of fields
            'movie.credits = movie.credits.Replace(", ", " / ")
            'movie.stars = movie.stars.Replace(", ", " / ")
            child = doc.CreateElement("movie")
            child.AppendChild(doc,  "filedate",             movie.filedate)
            child.AppendChild(doc,  "createdate",           movie.createdate)
            child.AppendChild(doc,  "missingdata1",         movie.missingdata1.ToString)
            child.AppendChild(doc,  "filename",             movie.filename)
            child.AppendChild(doc,  "foldername",           movie.foldername)
            child.AppendChild(doc,  "fullpathandfilename",  movie.fullpathandfilename)
            child.AppendChild(doc,  "source",               If(movie.source = Nothing, "", movie.source))
            child.AppendChild(doc,  "director",             movie.director)
            child.AppendChild(doc,  "credits",              movie.credits)
            child.AppendChild(doc,  "set",                  If(movie.InASet, movie.SetName, ""))
            child.AppendChild(doc,  "setid",                If(movie.InASet, movie.TmdbSetId, ""))
            child.AppendChild(doc,  "genre",                movie.genre)
            child.AppendChild(doc,  "countries",            movie.countries)
            child.AppendChild(doc,  "studios",              movie.studios)
            child.AppendChildList(doc,  "tag",              movie.movietag)
            child.AppendChild(doc,  "id",                   movie.id)
            child.AppendChild(doc,  "tmdbid",               movie.tmdbid)
            child.AppendChild(doc,  "playcount",            movie.playcount)
            child.AppendChild(doc,  "rating",               movie.rating)
            child.AppendChild(doc,  "title",                movie.title)
            child.AppendChild(doc,  "originaltitle",        movie.originaltitle)
            child.AppendChild(doc,  "outline",              If(String.IsNullOrEmpty(movie.sortorder), movie.title, movie.outline))
            child.AppendChild(doc,  "plot",                 Left(movie.plot, 100))  'Only write first 100 chars to cache- this plot is only used for table view - normal full plot comes from the nfo file (fullbody)
            child.AppendChild(doc,  "tagline",              movie.tagline)
            child.AppendChild(doc,  "sortorder",            movie.sortorder)
            child.AppendChild(doc,  "stars",                movie.stars)
            child.AppendChild(doc,  "runtime",              movie.runtime)
            child.AppendChild(doc,  "top250",               movie.top250)
            child.AppendChild(doc,  "year",                 movie.year)
            child.AppendChild(doc,  "votes",                movie.Votes)
            child.AppendChild(doc,  "usrrated",             movie.usrrated)
            child.AppendChild(doc,  "metascore",            movie.metascore)
            child.AppendChild(doc,  "Resolution",           movie.Resolution)
            child.AppendChild(doc,  "VideoCodec",           movie.VideoCodec)
            child.AppendChild(doc,  "Container",            movie.Container)

            For Each item In movie.Audio
                child.AppendChild(item.GetChild(doc))
            Next

            For Each item In movie.SubLang 
                child.AppendChild(item.GetChild(doc))
            Next

            child.AppendChild(doc, "Premiered", movie.Premiered)
            child.AppendChild(doc, "Certificate", movie.Certificate)
            child.AppendChild(doc, "FrodoPosterExists", movie.FrodoPosterExists)
            child.AppendChild(doc, "PreFrodoPosterExists", movie.PreFrodoPosterExists)
            child.AppendChild(doc, "FolderSize", movie.FolderSize)
            child.AppendChild(doc, "MediaFileSize", movie.MediaFileSize)
            child.AppendChild(doc, "RootFolder", movie.rootfolder)
            child.AppendChild(doc, "UserTmdbSetAddition", movie.UserTmdbSetAddition)
            child.AppendChild(doc, "UnknownSetCount", movie.UnknownSetCount)

            If movie.LockedFields.Count>0 Then
                child.AppendChild(doc, "LockedFields", String.Join(",", movie.LockedFields.ToArray()))
            End If
            child.AppendChild(doc, "VideoMissing", movie.VideoMissing)
            child.AppendChild(doc, "NumVideoBits", movie.NumVideoBits)
            root.AppendChild(child)
            
        Next

        doc.AppendChild(root)
        WorkingWithNfoFiles.SaveXMLDoc(doc, cacheFile)
    End Sub

    Public Sub MVCacheLoadFromNfo()
        tmpMVCache.Clear()
        Dim t As New List(Of String)
        For each item In Pref.MVidFolders
            If item.selected Then
                t.Add(item.rpath)
            End If
        Next
        For each item In Pref.MVConcertFolders
            If item.selected Then
                t.Add(item.rpath)
            End If
        Next
        MV_NfoLoad(t)
    End Sub

    Public Sub LoadMovieCacheFromNfos
        TmpMovieCache.Clear
        
        Dim t As New List(Of String)
        For Each rtpath In Pref.movieFolders 
            If rtpath.selected Then
                t.Add(rtpath.rpath)
            End If
        Next
        
        t.AddRange(Pref.offlinefolders)

        ReportProgress("Searching movie folders...")
        mov_NfoLoad(t)

        If Cancelled Then Exit Sub

        If Not Pref.usefoldernames Then
            For Each movie In TmpMovieCache
                If movie.filename <> Nothing Then movie.filename = movie.filename.Replace(".nfo", "")
            Next
        End If
        
        MovieCache.Clear
        MovieCache.AddRange(TmpMovieCache)
        AssignUnknownUserTmdbSetAdditions()
        RebuildUnknownSetCount()
        Rebuild_Data_GridViewMovieCache()

        If Pref.XbmcLinkReady Then
            Dim evt As BaseEvent = New BaseEvent(XbmcController.E.MC_ScanForNewMovies,PriorityQueue.Priorities.low)
            Form1.XbmcControllerQ.Write(evt)
        End If
    End Sub

    Property TotalNumberOfFolders As Integer
    Property NumberOfFoldersDone  As Integer
    Property BWs As New List(Of BackgroundWorker)
    Property NumActiveThreads     As Integer

    Public Sub MT_LoadMovieCacheFromNfos

        TmpMovieCache.Clear
        TotalNumberOfFolders=0
        NumberOfFoldersDone =0

        BWs.Clear

        Dim RootMovieFolders As New List(Of String)

        For Each rtpath In Pref.movieFolders 
                If rtpath.selected Then
                    RootMovieFolders.Add(rtpath.rpath)
                End If
            Next
        RootMovieFolders.AddRange(Pref.offlinefolders)

        ReportProgress("Searching movie folders...")

        For each item In RootMovieFolders

            Dim bw As BackgroundWorker = New BackgroundWorker

            bw.WorkerReportsProgress      = True
            bw.WorkerSupportsCancellation = True

            AddHandler bw.DoWork            , AddressOf bw_DoWork
            AddHandler bw.ProgressChanged   , AddressOf bw_ProgressChanged
            AddHandler bw.RunWorkerCompleted, AddressOf bw_RunWorkerCompleted

            BWs.Add(bw)
            NumActiveThreads += 1

            bw.RunWorkerAsync(item)
        Next

        Dim Cancelling As Boolean = False
        Dim Busy       As Boolean = True

        While Busy
            Threading.Thread.Sleep(100)
            If Cancelled And Not Cancelling Then 
                Cancelling = True
                For each item As BackgroundWorker in BWs
                    Try
                        item.CancelAsync
                    Catch
                    End Try
                Next
            End If
            Busy = False
            For Each item As BackgroundWorker in BWs
                Try
                    Busy = Busy Or item.IsBusy
                    If Busy Then Exit For
                Catch
                End Try
            Next
        End While
      
        If Cancelled Then Exit Sub

        MovieCache.Clear
        MovieCache.AddRange(TmpMovieCache)
        RebuildUnknownSetCount()
        AssignUnknownUserTmdbSetAdditions()

        Rebuild_Data_GridViewMovieCache

        If Pref.XbmcLinkReady Then
            Dim evt As BaseEvent = New BaseEvent(XbmcController.E.MC_ScanForNewMovies,PriorityQueue.Priorities.low)
            Form1.XbmcControllerQ.Write(evt)
        End If
        BWs.Clear()
    End Sub

    Sub bw_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) 

        Dim bw     As BackgroundWorker = CType(sender, BackgroundWorker)
        Dim folder As String = DirectCast(e.Argument, String)
        Dim Cache  As New List(Of ComboList)

        MT_mov_NfoLoad(bw,folder,Cache)

        e.Result = Cache
    End Sub

    Private Sub bw_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)

        Dim mp As MovieProgress = CType(e.UserState, MovieProgress)

        Select mp.ProgressEvent

            Case MovieProgress.MsgType.GotFoldersCount : TotalNumberOfFolders += mp.Data
                                                         ReportProgress("Total number of folders : [" & TotalNumberOfFolders & "]")

            Case MovieProgress.MsgType.DoneSome        : NumberOfFoldersDone += mp.Data
                                                         PercentDone = CalcPercentDone(NumberOfFoldersDone, TotalNumberOfFolders)
                                                         ReportProgress("Active threads : [" & NumActiveThreads & "] - Scanning folder " & NumberOfFoldersDone & " of " & TotalNumberOfFolders)

        End Select
    End Sub

    Private Sub bw_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        Threading.Monitor.Enter(Me)
        NumActiveThreads -= 1

        If IsNothing(e.Result) Then Exit Sub

        Dim Cache As List(Of ComboList) = CType(e.Result, List(Of ComboList))
 
        TmpMovieCache.AddRange(Cache) 
        Threading.Monitor.Exit(Me)
    End Sub

    Private Sub MT_mov_NfoLoad(bw As BackgroundWorker, folder As String,Cache As List(Of ComboList))

        Const pattern = "*.nfo"
        
        Dim moviePaths As New List(Of String)

        If Not (New DirectoryInfo(folder)).Exists Then Exit Sub

        moviePaths.Add(folder)

        'Add sub-folders
        Try
            For Each subfolder In Utilities.EnumerateFolders(moviePaths(0))
                If Pref.ExcludeFolders.Match(subfolder) Then Continue For
                moviePaths.Add(subfolder)
            Next
        Catch ex As Exception
            ExceptionHandler.LogError(ex,"LastRootPath: [" & Utilities.LastRootPath & "]")
        End Try


        Dim progress As MovieProgress = New MovieProgress

        progress.ProgressEvent = MovieProgress.MsgType.GotFoldersCount
        progress.Data          = moviePaths.Count

        bw.ReportProgress( -1, progress.Clone )

        Dim   i           As Integer = 0
        Const ReportEvery As Integer = 1

        progress.ProgressEvent = MovieProgress.MsgType.DoneSome
        progress.Data          = ReportEvery

        For Each Path In moviePaths
            i += 1
            If i Mod ReportEvery=0 Then bw.ReportProgress( -1, progress.Clone )

            MT_mov_ListFiles( pattern, New DirectoryInfo(Path), Cache )
            If Cancelled Then Exit Sub
        Next

        progress.Data = i Mod ReportEvery

        bw.ReportProgress( -1, progress.Clone )

        If Not Pref.usefoldernames Then
            For Each movie In Cache
                If movie.filename <> Nothing Then movie.filename = movie.filename.Replace(".nfo", "")
            Next
        End If
    End Sub

    Private Sub MT_mov_ListFiles(ByVal pattern As String, ByVal dirInfo As DirectoryInfo, Cache As List(Of ComboList))
        Dim incmissing As Boolean = Pref.incmissingmovies 
        For Each oFileInfo In dirInfo.GetFiles(pattern)

            Application.DoEvents

            If Cancelled Then Exit Sub

            If Not File.Exists(oFileInfo.FullName) Then Continue For

            Dim movie = New Movie(Me,oFileInfo.FullName)
        
            If Not incmissing AndAlso movie.mediapathandfilename = "none" Then Continue For
       
            movie.LoadNFO(False)
            
            Cache.Add(movie.Cache)
        Next
    End Sub

    Private Sub mov_NfoLoad(ByVal folderlist As List(Of String))
        Dim tempint As Integer
        Dim dirinfo As String = String.Empty
        Const pattern = "*.nfo"
        Dim moviePaths As New List(Of String)

        For Each moviefolder In folderlist
            If (New DirectoryInfo(moviefolder)).Exists Then
                moviePaths.Add(moviefolder)
            End If
        Next

        tempint = moviePaths.Count

        'Add sub-folders
        For f = 0 To tempint - 1
            Try
                For Each subfolder In Utilities.EnumerateFolders(moviePaths(f))
                    If Pref.ExcludeFolders.Match(subfolder) Then Continue For
                    moviePaths.Add(subfolder)
                Next
            Catch ex As Exception
                ExceptionHandler.LogError(ex,"LastRootPath: [" & Utilities.LastRootPath & "]")
            End Try
        Next

        Dim i = 0
        For Each Path In moviePaths
            i += 1
            PercentDone = CalcPercentDone(i, moviePaths.Count)
            ReportProgress("Scanning folder " & i & " of " & moviePaths.Count)

            If Not Pref.HomeVidScrape Then
                mov_ListFiles(pattern, New DirectoryInfo(Path))
            Else
                HV_ListFiles(pattern, New DirectoryInfo(Path))
            End If
            If Cancelled Then Exit Sub
        Next
    End Sub

    Private Sub mov_ListFiles(ByVal pattern As String, ByVal dirInfo As DirectoryInfo)
        Dim incmissing As Boolean = Pref.incmissingmovies 
        If IsNothing(dirInfo) Then Exit Sub
        For Each oFileInfo In dirInfo.GetFiles(pattern)
            Application.DoEvents
            If Cancelled Then Exit Sub
            If Not File.Exists(oFileInfo.FullName) Then Continue For
            Try
                Dim movie = New Movie(Me,oFileInfo.FullName)

                If Not incmissing AndAlso movie.mediapathandfilename = "none" Then Continue For
                If Not Utilities.NfoValidate(oFileInfo.FullName) Then Continue For
                movie.LoadNFO(False)
                
                TmpMovieCache.Add(movie.Cache)
            Catch
                MsgBox("problem with : " & oFileInfo.FullName & " - Skipped" & vbCrLf & "Please check this file manually")
            End Try
        Next
    End Sub

    Sub MV_NfoLoad(ByVal folderlist As List(Of String))
        Dim tempint As Integer
        Dim dirinfo As String = String.Empty
        Const pattern = "*.nfo"
        Dim moviePaths As New List(Of String)

        For Each moviefolder In folderlist
            If (New DirectoryInfo(moviefolder)).Exists Then
                moviePaths.Add(moviefolder)
            End If
        Next
        tempint = moviePaths.Count

        'Add sub-folders
        For f = 0 To tempint - 1
            Try
                For Each subfolder In Utilities.EnumerateFolders(moviePaths(f))
                    moviePaths.Add(subfolder)
                Next
            Catch ex As Exception
                ExceptionHandler.LogError(ex,"LastRootPath: [" & Utilities.LastRootPath & "]")
            End Try
        Next
        Dim i = 0
        For Each Path In moviePaths
            i += 1
            PercentDone = CalcPercentDone(i, moviePaths.Count)
            ReportProgress("Scanning folder " & i & " of " & moviePaths.Count)

            MV_ListFiles(pattern, New DirectoryInfo(Path))
            If Cancelled Then Exit Sub
        Next
        ucMusicVideo.MVCache.Clear
        ucMusicVideo.MVCache.AddRange(tmpMVCache)
    End Sub

    Private Sub MV_ListFiles(ByVal pattern As String, ByVal dirInfo As DirectoryInfo)
        If IsNothing(dirInfo) Then Exit Sub
         
        For Each oFileInfo In dirInfo.GetFiles(pattern)
            Dim tmp As New MVComboList
            Application.DoEvents
            If Cancelled Then Exit Sub
            If Not File.Exists(oFileInfo.FullName) Then Continue For
            Try
                If Not validateMusicVideoNfo(oFileInfo.FullName) Then Continue For
                Dim mvideo As New FullMovieDetails
                mvideo = WorkingWithNfoFiles.MVloadNfo(oFileInfo.FullName)
                tmp.Assign(mvideo)
                tmpMVCache.Add(tmp)
            Catch
                MsgBox("problem with : " & oFileInfo.FullName & " - Skipped" & vbCrLf & "Please check this file manually")
            End Try
        Next

    End Sub

    Private Sub HV_ListFiles(ByVal pattern As String, ByVal dirInfo As DirectoryInfo)
        If IsNothing(dirInfo) Then Exit Sub
         
        For Each oFileInfo In dirInfo.GetFiles(pattern)
            Dim tmp As New HmMovComboList
            Application.DoEvents
            If Cancelled Then Exit Sub
            If Not File.Exists(oFileInfo.FullName) Then Continue For
            Try
                If Not Utilities.NfoValidate(oFileInfo.FullName) Then Continue For
                Dim hvideo As New FullMovieDetails
                hvideo = WorkingWithNfoFiles.nfoLoadHomeMovie(oFileInfo.FullName)
                tmp.Assign(hvideo)
                tmpHmVidCache.Add(tmp)
            Catch
                MsgBox("problem with : " & oFileInfo.FullName & " - Skipped" & vbCrLf & "Please check this file manually")
            End Try
        Next

    End Sub

    Function DeleteScrapedFiles(nfoPathAndFilename As String, Optional ByVal DeleteArtwork As Boolean = True) As Boolean
        Try
            Dim aMovie = New Movie(Me, nfoPathAndFilename)
            aMovie.DeleteScrapedFiles(True, DeleteArtwork)
            Dim isRoot As Boolean = Pref.GetRootFolderCheck(nfoPathAndFilename)
            If Not isRoot AndAlso DeleteArtwork Then
                aMovie.DeleteExtraFiles()
                aMovie.DeleteFanarTvFiles()
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Sub LoadPeopleCaches()
        LoadActorCache()
        LoadDirectorCache()
    End Sub

    Sub LoadMovieSetCache()
        LoadMovieSetCache(_moviesetDb, "movieset", Pref.workingProfile.moviesetcache)
    End Sub

    Sub LoadActorCache()
        LoadPersonCache(_actorDb,"actor",Pref.workingProfile.actorcache)
    End Sub

    Sub LoadDirectorCache()
        LoadPersonCache(_directorDb,"director",Pref.workingProfile.DirectorCache)
    End Sub

    Sub LoadPersonCache(peopleDb As List(Of Databases),typeName As String,  fileName As String)
        peopleDb.Clear()
        If Not File.Exists(fileName) Then Exit Sub
        Dim peopleList As New XmlDocument
        peopleList.Load(fileName)
        For Each thisresult As XmlNode In peopleList(typeName & "_cache")
            Select Case thisresult.Name
                Case typeName
                    Dim name = ""
                    Dim movieId = ""
                    Dim detail As XmlNode = Nothing
                    For Each detail In thisresult.ChildNodes
                        Select Case detail.Name
                            Case "name"
                                name = detail.InnerText
                            Case "id"
                                movieId = detail.InnerText
                        End Select
                    Next
                    peopleDb.Add(New Databases(name, movieId))
            End Select
        Next
    End Sub

    Sub LoadPersonCache(peopleDb As List(Of DirectorDatabase),typeName As String,  fileName As String)
        peopleDb.Clear()
        If Not File.Exists(fileName) Then Exit Sub
        Dim peopleList As New XmlDocument
        peopleList.Load(fileName)
        For Each thisresult As XmlNode In peopleList(typeName & "_cache")
            Select Case thisresult.Name
                Case typeName
                    Dim name = ""
                    Dim movieId = ""
                    Dim detail As XmlNode = Nothing
                    For Each detail In thisresult.ChildNodes
                        Select Case detail.Name
                            Case "name"
                                name = detail.InnerText
                            Case "id"
                                movieId = detail.InnerText
                        End Select
                    Next
                    peopleDb.Add(New DirectorDatabase(name, movieId))
            End Select
        Next
    End Sub

    Sub LoadMovieSetCache(setDb As List(Of MovieSetInfo), typeName As String, fileName As String)
        setDb.Clear()
        If Not File.Exists(fileName) Then Exit Sub
        Dim moviesetcache As New XmlDocument
        moviesetcache.Load(fileName)
        For Each thisresult As XmlNode In moviesetcache(typeName & "_cache")
            Select Case thisresult.Name
                Case typeName
                    Dim movieset = ""
                    Dim moviesetId = ""
                    Dim moviesetplot = ""
                    Dim LastUpdatedTs As Date = Date.MinValue
                    Dim UserMovieSetName = ""
                    Dim Dirty As Boolean = False
                    Dim detail As XmlNode = Nothing
                    Dim movac As New List(Of CollectionMovie)
                    For Each detail In thisresult.ChildNodes

                        Select Case detail.Name

                            Case "moviesetname"
                                movieset = detail.InnerText
                            Case "id"
                                moviesetId = setMovieSetID(detail.InnerText, setDb)
                            Case "plot"
                                moviesetplot = detail.InnerText
                            Case "LastUpdatedTs"
                                Dim tmpdate As String = detail.InnerText
                                If IsNumeric(tmpdate) Then
                                    LastUpdatedTs = DateTime.ParseExact(tmpdate, Pref.datePattern, Nothing)
                                Else
                                    tmpdate = tmpdate.Replace("a.m.", "AM").Replace("p.m.", "PM")
                                    LastUpdatedTs = tmpdate
                                End If
                            Case "UserMovieSetName"
                                UserMovieSetName = detail.InnerText
                            Case "DirtyData"
                                Dirty = detail.InnerXml
                            Case "collection"
                                Dim ac As New CollectionMovie
                                Dim detail2 As XmlNode = Nothing
                                For Each detail2 In detail.ChildNodes
                                    Select Case detail2.Name
                                        Case "movietitle"       : ac.MovieTitle     = detail2.InnerText
                                        Case "movieid"          : ac.TmdbMovieId    = detail2.InnerText
                                        Case "backdrop_path"    : ac.backdrop_path  = detail2.InnerText
                                        Case "poster_path"      : ac.poster_path    = detail2.InnerText
                                        Case "release_date"     : ac.release_date   = detail2.InnerText
                                    End Select
                                Next
                                movac.Add(ac)
                        End Select
                    Next
                    setDb.Add(New MovieSetInfo(movieset, moviesetId, moviesetplot, movac, LastUpdatedTs, UserMovieSetName, _dirty:= Dirty))
            End Select
        Next
    End Sub

    Sub LoadTagCache()
        _tagdb.Clear()
        If Not File.Exists(Utilities.applicationPath & "\settings\tagcache.xml") Then Exit Sub
        Dim peopleList As New XmlDocument
        peopleList.Load(Utilities.applicationPath & "\settings\tagcache.xml")
        For Each thisresult As XmlNode In peopleList("tag_cache")
            Select Case thisresult.Name
                Case "Tag"
                    Dim TagTitle = ""
                    Dim movieId = ""
                    Dim detail As XmlNode = Nothing
                    For Each detail In thisresult.ChildNodes
                        Select Case detail.Name
                            Case "TagTitle"
                                TagTitle = detail.InnerText
                            Case "id"
                                movieId = detail.InnerText
                        End Select
                    Next
                    _tagDb.Add(New TagDatabase(TagTitle, movieId))
            End Select
        Next
    End Sub
    
    Sub SaveActorCache()
        SavePersonCache(ActorDb,"actor",Pref.workingProfile.actorcache)
    End Sub

    Sub SaveDirectorCache()
        SavePersonCache(DirectorDb,"director",Pref.workingProfile.directorCache)
    End Sub

    Sub SaveMovieSetCache()
        SaveMovieSetCache(MovieSetDB, "movieset", Pref.workingProfile.moviesetcache)
    End Sub

    Sub SavePersonCache(peopleDb As List(Of Databases), typeName As String, fileName As String)
        'Threading.Monitor.Enter(Me)
        Dim doc As New XmlDocument

        Dim thispref As XmlNode = Nothing
        Dim xmlproc  As XmlDeclaration

        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)

        Dim root  As XmlElement
        Dim child As XmlElement

        root = doc.CreateElement(typeName & "_cache")
        
        For Each actor In peopleDb
            child = doc.CreateElement(typeName)
            child.AppendChild(doc, "name"   , actor.ActorName)
            child.AppendChild(doc, "id"     , actor.MovieId)
            root.AppendChild(child)
        Next
        doc.AppendChild(root)
        WorkingWithNfoFiles.SaveXMLDoc(doc, fileName)
        'Threading.Monitor.Exit(Me)
    End Sub

    Sub SavePersonCache(peopleDb As List(Of DirectorDatabase), typeName As String, fileName As String)
        Dim doc As New XmlDocument
        Dim thispref As XmlNode = Nothing
        Dim xmlproc  As XmlDeclaration
        Dim root  As XmlElement
        Dim child As XmlElement

        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        
        root = doc.CreateElement(typeName & "_cache")
        
        For Each actor In peopleDb
            child = doc.CreateElement(typeName)
            child.AppendChild(doc, "name"   , actor.ActorName)
            child.AppendChild(doc, "id"     , actor.MovieId)
            root.AppendChild(child)
        Next
        doc.AppendChild(root)
        WorkingWithNfoFiles.SaveXMLDoc(doc, fileName)
    End Sub

    Sub SaveMovieSetCache(setDb As List(Of MovieSetInfo), typeName As String, fileName As String)
        Dim doc As New XmlDocument
        Dim thispref As XmlNode = Nothing
        Dim xmlproc  As XmlDeclaration
        Dim root  As XmlElement
        Dim child As XmlElement
        Dim childchild As XmlElement

        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        
        root = doc.CreateElement(typeName & "_cache")
        
        For Each movieset In setDb
            If movieset.MovieSetName.ToLower = "-none-" Then Continue For
            child = doc.CreateElement(typeName)
            Dim attr As XmlAttribute = doc.CreateAttribute("title")  ' just to pretty it up in notepad++ when colapsing <movieset> node.
            attr.Value = movieset.MovieSetName
            child.SetAttributeNode(attr)
            
            child.AppendChild(doc   , "moviesetname"        , movieset.MovieSetName)
            child.AppendChild(doc   , "id"                  , If(movieset.TmdbSetId.StartsWith("L"), "", movieset.TmdbSetId))
            child.AppendChild(doc   , "plot"                , movieset.MovieSetPlot)
            child.AppendChild(doc   , "LastUpdatedTs"       , Format(movieset.LastUpdatedTs, Pref.datePattern).ToString) 'movieset.LastUpdatedTs.ToString)
            child.AppendChild(doc   , "DirtyData"           , movieset.Dirty)
            child.AppendChild(doc   , "UserMovieSetName"    , movieset.UserMovieSetName)

            If Not IsNothing(movieset.Collection) Then
                For each item In movieset.Collection
                    childchild = doc.CreateElement("collection")
                    childchild.AppendChild(doc   , "movietitle"          , item.MovieTitle)
                    childchild.AppendChild(doc   , "movieid"             , item.TmdbMovieId)
                    childchild.AppendChild(doc   , "backdrop_path"       , item.backdrop_path)
                    childchild.AppendChild(doc   , "poster_path"         , item.poster_path)
                    childchild.AppendChild(doc   , "release_date"        , item.release_date)
                    child.AppendChild(childchild)
                Next
            End If
            root.AppendChild(child)
        Next
        doc.AppendChild(root)

        WorkingWithNfoFiles.SaveXMLDoc(doc, fileName)
    End Sub

    Sub SaveTagCache()
        Dim doc As New XmlDocument
        Dim thispref    As XmlNode = Nothing
        Dim xmlproc     As XmlDeclaration
        Dim root        As XmlElement
        Dim child       As XmlElement
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        
        root = doc.CreateElement("tag_cache")
        
        For Each tagtosave In TagDb
            child = doc.CreateElement("Tag")
            child.AppendChild(doc   , "TagTitle"    , tagtosave.TagName.Trim)
            child.AppendChild(doc   , "id"          , tagtosave.MovieId)
            root.AppendChild(child)
        Next
        doc.AppendChild(root)

        WorkingWithNfoFiles.SaveXMLDoc(doc, Utilities.applicationPath & "\settings\tagcache.xml")
    End Sub

    Public Sub RebuildCaches
        'If Pref.UseMultipleThreads Then
        '    movRebuildCaches = False
        'Else
        '    movRebuildCaches = True
        'End If
        movRebuildCaches = Not Pref.UseMultipleThreads 
        RebuildMovieCache
        If Cancelled Then Exit Sub
        'If Not movRebuildCaches Then RebuildMoviePeopleCaches
        RebuildMoviePeopleCaches
        movRebuildCaches = False
    End Sub

    Public Sub RebuildMovieCache()
        If Pref.UseMultipleThreads Then
            MT_LoadMovieCacheFromNfos
        Else
            LoadMovieCacheFromNfos
        End If
        If Cancelled Then Exit Sub
        SaveMovieCache
    End Sub
    
    Public Sub RebuildMoviePeopleCaches()
        
        Dim MovSetDbTmp As New List(Of MovieSetInfo)

        MovSetDbTmp     .AddRange(_moviesetDb)
        _actorDB        .Clear()
        _directorDb     .Clear()
        _tagDb          .Clear()
        _tmpActorDb     .Clear()
        _tmpDirectorDb  .Clear()
        Dim i = 0

        For Each movie In MovieCache
            i += 1
            PercentDone = CalcPercentDone(i, MovieCache.Count)
            ReportProgress("Rebuilding caches " & i & " of " & MovieCache.Count)
            
            For Each act In movie.Actorlist
                _actorDb.Add(New Databases(act.actorname, movie.id))
            Next
            If movie.TmdbSetId <> "" Then
                Dim c As MovieSetInfo = Nothing
                Try
                    c = FindMovieSetInfoByTmdbSetId(movie.TmdbSetId)
                Catch
                End Try
                If IsNothing(c) Then
                    Dim d As MovieSetInfo = New MovieSetInfo(movie.SetName, movie.TmdbSetId, "", New List(Of CollectionMovie), Date.Now(), _dirty:= True)
                    Dim e As CollectionMovie = New CollectionMovie(movie.title, movie.tmdbid, _release_date:=movie.Premiered)
                    d.Collection.Add(e)
                    AddUpdateMovieSetInCache(d)
                Else
                    If c.Dirty Then
                        Dim add As Boolean = False
                        If Not movie.tmdbid = String.Empty Then  'Check if in collection by TMDBId
                            Try
                                Dim q = From x In c.Collection Where x.TmdbMovieId = movie.tmdbid
                                If q.Count = 0 Then add = True
                            Catch
                            End Try
                        Else                                      'Else check if in collection by movie title
                            Try
                                Dim q = From x In c.Collection Where x.MovieTitle = movie.title
                                If q.Count = 0 Then add = True
                            Catch
                            End Try
                        End If
                        If add Then
                            Dim e As CollectionMovie = New CollectionMovie(movie.title, movie.tmdbid, _release_date:=movie.Premiered)
                            c.Collection.Add(e)
                            AddUpdateMovieSetInCache(c, True)
                        End If
                    End If
                    If c.MovieSetName <> movie.SetName Then
                        c.UserMovieSetName = movie.SetName
                        AddUpdateMovieSetInCache(c, True)
                    End If
                End If
            ElseIf movie.SetName <> "-None-" AndAlso movie.TmdbSetId = ""
                Dim c As MovieSetInfo = Nothing
                c = FindMovieSetInfoBySetName(movie.SetName)
                If c.MovieSetName = "" Then
                    Dim d As MovieSetInfo = New MovieSetInfo(movie.SetName, setMovieSetID("", MovieSetDB), "", New List(Of CollectionMovie), Date.Now(), _dirty:= False)
                    AddUpdateMovieSetInCache(d)
                End If
            End If
            Dim directors() As String = movie.director.Split("/")
            For Each d In directors
                _directorDb.Add(New DirectorDatabase(d.Trim, movie.id))
            Next

            For each t In movie.movietag
                _tagDb.Add(New TagDatabase(t, movie.id))
                'If Not _tagdb.Contains(t) Then _tagDb.Add(t)
            Next

            If Cancelled Then Exit Sub
        Next

        If Cancelled Then Exit Sub
        
        SaveActorCache()
        SaveDirectorCache()
        SaveMovieSetCache()
        SaveTagCache()
    End Sub
    
    Function Mov_DeleteMovieFolder(fullpathandfilename) As Boolean
        Dim aok As Boolean = True
		If Pref.usefoldernames Then

			Dim str = Path.GetDirectoryName(fullpathandfilename)

            Try
                FileIO.FileSystem.DeleteDirectory(str, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin)
            Catch ex As OperationCanceledException
                aok = False
            Catch ex As IO.DirectoryNotFoundException
                aok = False
            End Try
			
			If aok Then RemoveMovieFromCache(fullpathandfilename)
		End If
        Return aok

    End Function
	
    Sub RemoveMovieFromCache(fullpathandfilename)

        If fullpathandfilename = "" Then Exit Sub
        MovieCache             .RemoveAll(Function(c) c.fullpathandfilename = fullpathandfilename)
        Data_GridViewMovieCache.RemoveAll(Function(c) c.fullpathandfilename = fullpathandfilename)

        If Pref.XbmcLinkReady Then
            Dim media As String = Utilities.GetFileName(fullpathandfilename,True)

            If media <> "none" Then
                Dim evt As New BaseEvent

                evt.E    = XbmcController.E.MC_Movie_Removed
                evt.Args = New VideoPathEventArgs(media, PriorityQueue.Priorities.medium)

                Form1.XbmcControllerQ.Write(evt)
            End If
        End If
    End Sub

    Public Shared Sub SpinUpDrives
        For each item In Pref.movieFolders
            If Not item.selected Then Continue For
            Dim bw As BackgroundWorker = New BackgroundWorker

            AddHandler bw.DoWork, AddressOf bw_SpinupDrive

            bw.RunWorkerAsync(item.rpath)
        Next
    End Sub

    Shared Sub bw_SpinupDrive(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) 
        Try
            Dim filet As String = Path.Combine( DirectCast(e.Argument,String) , "delme.tmp" )

            File.WriteAllText(filet, "Anything")
            
            Utilities.SafeDeleteFile(filet)
        Catch
        End Try
    End Sub


#Region "  Music Video Routines"
        
    Public Sub FindNewMusicVideos(Optional scrape=True)
        NewMovies.Clear
        PercentDone = 0

        Dim folders As New List(Of String)

        AddOnlineFolders ( folders , Pref.MVidFolders       )
        AddOnlineFolders ( folders , Pref.MVConcertFolders  )
        'AddOfflineFolders( folders )

        Dim i = 0
        For Each folder In folders
            i += 1
            PercentDone = CalcPercentDone(i,folders.Count)
            ReportProgress("Scanning folder " & i & " of " & folders.Count)

            AddNewMovies(folder)
            
            If Cancelled then 
                NewMovies.Clear
                Exit Sub
            End If
        Next

        If scrape then ScrapeNewMovies
    End Sub

    Public Sub RebuildMVCache()
        MVCacheLoadFromNfo()
        'If Cancelled Then Exit Sub
       ' SaveMovieCache
    End Sub
#End Region

#Region " Home Movie Routines"

    Public Sub FindNewHomeVideos(Optional scrape = True)
        NewMovies.Clear
        PercentDone = 0

        Dim folders As New List(Of String)

        AddOnlineFolders ( folders , Pref.homemoviefolders)

        Dim i = 0
        For Each folder In folders
            i += 1
            PercentDone = CalcPercentDone(i,folders.Count)
            ReportProgress("Scanning folder " & i & " of " & folders.Count)

            AddNewMovies(folder)
            
            If Cancelled then 
                NewMovies.Clear
                Exit Sub
            End If
        Next

        If scrape then ScrapeNewMovies
    End Sub

    Public Sub RebuildHomeVideoCache()
        tmpHmVidCache.Clear()
        Dim t As New List(Of String)
        For Each rtpath In Pref.homemoviefolders 
            If rtpath.selected Then
                t.Add(rtpath.rpath)
            End If
        Next

        ReportProgress("Searching Home Movie folders...")
        mov_NfoLoad(t)
        Form1.HMCache.Clear()
        Form1.HMCache.AddRange(tmpHmVidCache)
    End Sub
#End Region

#Region "Filters"

    Function ApplyCountiesFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb,ModuleExtensions.Missing,"")
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x) x.countriesList.Intersect(fi.Include).Any() )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not x.countriesList.Intersect(fi.Exclude).Any() )
        End If

        Return recs
    End Function

    Function ApplyStudiosFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb,ModuleExtensions.Missing,"")
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x) x.studiosList.Intersect(fi.Include).Any() )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not x.studiosList.Intersect(fi.Exclude).Any() )
        End If

        Return recs
    End Function

    Function ApplyGenresFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim i As Integer = 0

        For Each item As CCBoxItem In ccb.Items

            Dim value As String = item.Name.RemoveAfterMatch

            Select ccb.GetItemCheckState(i)
                Case CheckState.Checked   : recs = (From m In recs Where     m.genre.Contains(value)).ToList
                Case CheckState.Unchecked : recs = (From m In recs Where Not m.genre.Contains(value)).ToList
            End Select

            i += 1
        Next

        Return recs
    End Function
    
    Function ApplyCertificatesFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)

        Dim fi As New FilteredItems(ccb, "Missing", "")
       
        If fi.Include.Count>0 Then
            recs = recs.Where(  Function(x)     fi.Include.Contains( CertificateMappings.GetMapping(x.Certificate) )  )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where(  Function(x) Not fi.Exclude.Contains( CertificateMappings.GetMapping(x.Certificate) )  )
        End If

        Return recs
    End Function

    Function ApplySetsFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb)

        If fi.Include.Count > 0 Then
            recs = recs.Where(Function(x) fi.Include.Contains(x.SetName))
        End If
        If fi.Exclude.Count > 0 Then
            recs = recs.Where(Function(x) Not fi.Exclude.Contains(x.SetName))
        End If

        Return recs
    End Function
    
    Function ApplyResolutionsFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb,"Unknown","-1")
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x)     fi.Include.Contains(x.Resolution) )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not fi.Exclude.Contains(x.Resolution) )
        End If

        Return recs
    End Function

    Function ApplyVideoCodecFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb,"Unknown","-1")
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x)     fi.Include.Contains(x.VideoCodec) )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not fi.Exclude.Contains(x.VideoCodec) )
        End If

        Return recs
    End Function

    Function ApplyAudioCodecsFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)

        Dim fi As New FilteredItems(ccb)

        Dim leftOuterJoinTable = From m In recs From a In m.Audio Select m.fullpathandfilename, field=If(a.Codec.Value="","Unknown",a.Codec.Value)
        
        Return Filter(recs, leftOuterJoinTable, fi)
             
    End Function

    Function ApplyAudioChannelsFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )

        Dim fi As New FilteredItems(ccb)

        Dim leftOuterJoinTable = From m In recs From a In m.Audio Select m.fullpathandfilename, field=If(a.Channels.Value="","Unknown",a.Channels.Value)

        Return Filter(recs,leftOuterJoinTable, fi)
    End Function

    Function ApplyAudioBitratesFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )

        Dim fi As New FilteredItems(ccb)

        Dim leftOuterJoinTable = From m In recs From a In m.Audio Select m.fullpathandfilename, field=If(a.Bitrate.Value="","Unknown",a.Bitrate.Value)

        Return Filter(recs,leftOuterJoinTable, fi)
    End Function

    Function ApplyNumAudioTracksFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )

        Dim fi As New FilteredItems(ccb)

        Dim leftOuterJoinTable = From m In recs Select m.fullpathandfilename, field=If(m.Audio.Count.ToString="","Unknown",m.Audio.Count.ToString)

        Return Filter(recs,leftOuterJoinTable, fi)
    End Function

    Function ApplyAudioLanguagesFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )

        'Dim i As Integer = 0

        'For Each item As CCBoxItem In ccb.Items

        '    Dim value As String = item.Name.RemoveAfterMatch

        '    Select ccb.GetItemCheckState(i)
        '        Case CheckState.Checked   : recs = recs.Where ( Function(x)     x.Audio.Exists( Function(a) If(a.Language.Value="","Unknown",a.Language.Value)=value ))
        '        Case CheckState.Unchecked : recs = recs.Where ( Function(x) Not x.Audio.Exists( Function(a) If(a.Language.Value="","Unknown",a.Language.Value)=value ))
        '    End Select

        '    i += 1
        'Next

        Dim fi As New FilteredItems(ccb,"Unknown","")
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x) x.Languages.Intersect(fi.Include).Any() )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not x.Languages.Intersect(fi.Exclude).Any() )
        End If

        Return recs
    End Function
    
    Function ApplyAudioDefaultLanguagesFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )

        Dim fi As New FilteredItems(ccb,"Unknown","")
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x)     fi.Include.Contains(x.DefaultAudioTrack.Language.Value) )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not fi.Exclude.Contains(x.DefaultAudioTrack.Language.Value) )
        End If

        Return recs
    End Function
    
    Function ApplyActorsFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )
        Return ApplyPeopleFilter(ActorDb, recs, ccb)
    End Function

    Function ApplyDirectorsFilter( recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox )
        Return ApplyPeopleFilter(DirectorDb, recs, ccb)
    End Function

    Function ApplyPeopleFilter(PeopleDb As List(Of Databases), recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)

        Dim fi As New FilteredItems(ccb)

        If fi.Include.Count>0 Then 
            Dim MovieIds = (From a In PeopleDb Where fi.Include.Contains(a.ActorName) Select a.MovieId).ToList
                             
            recs = recs.Where( Function(x)     MovieIds.Contains(x.id) )
        End If

        If fi.Exclude.Count>0 Then 
            Dim MovieIds = (From a In PeopleDb Where fi.Exclude.Contains(a.ActorName) Select a.MovieId).ToList
                             
            recs = recs.Where( Function(x) Not MovieIds.Contains(x.id) )
        End If

        Return recs
    End Function

    Function ApplyPeopleFilter(PeopleDb As List(Of DirectorDatabase), recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)

        Dim fi As New FilteredItems(ccb)

        If fi.Include.Count>0 Then 
            Dim MovieIds = (From a In PeopleDb Where fi.Include.Contains(a.ActorName) Select a.MovieId).ToList
                             
            recs = recs.Where( Function(x)     MovieIds.Contains(x.id) )
        End If

        If fi.Exclude.Count>0 Then 
            Dim MovieIds = (From a In PeopleDb Where fi.Exclude.Contains(a.ActorName) Select a.MovieId).ToList
                             
            recs = recs.Where( Function(x) Not MovieIds.Contains(x.id) )
        End If

        Return recs
    End Function

    Function ApplyTagsFilter(ByVal recs As IEnumerable(Of Data_GridViewMovie), ByVal ccb As TriStateCheckedComboBox)
        Dim i As Integer = 0

        For Each item As CCBoxItem In ccb.Items

            Dim value As String = item.Name.RemoveAfterMatch

            Select Case ccb.GetItemCheckState(i)
                Case CheckState.Checked : recs = (From m In recs Where m.movietag.Contains(value)).ToList
                Case CheckState.Unchecked : recs = (From m In recs Where Not m.movietag.Contains(value)).ToList
            End Select

            i += 1
        Next

        Return recs
    End Function

    Function ApplySourcesFilter(ByVal recs As IEnumerable(Of Data_GridViewMovie), ByVal ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb)

        If fi.Include.Count > 0 Then
            recs = recs.Where(Function(x) fi.Include.Contains(x.source))
        End If

        If fi.Exclude.Count > 0 Then
            recs = recs.Where(Function(x) Not fi.Exclude.Contains(x.source))
        End If

        Return recs
    End Function

    Function ApplySubtitleLangFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim i As Integer = 0
        For Each item As CCBoxItem In ccb.Items
            Dim value As String = item.Name.RemoveAfterMatch
            Select ccb.GetItemCheckState(i)
                Case CheckState.Checked   : recs = recs.Where ( Function(x)     x.SubLang.Exists( Function(a) If(a.Language.Value="","Unknown",a.Language.Value)=value ))
                Case CheckState.Unchecked : recs = recs.Where ( Function(x) Not x.SubLang.Exists( Function(a) If(a.Language.Value="","Unknown",a.Language.Value)=value ))
            End Select
            i += 1
        Next
        Return recs
    End Function

    Function ApplyRootFolderFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb)
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x)     fi.Include.Contains(x.rootfolder) )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not fi.Exclude.Contains(x.rootfolder) )
        End If

        Return recs
    End Function
    
    Function ApplyUserRatedFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)
        Dim fi As New FilteredItems(ccb)
       
        If fi.Include.Count>0 Then
            recs = recs.Where( Function(x)     fi.Include.Contains(x.usrrated) )
        End If
        If fi.Exclude.Count>0 Then
            recs = recs.Where( Function(x) Not fi.Exclude.Contains(x.usrrated) )
        End If

        Return recs
    End Function

    Function ApplyLockedFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)

        Dim i As Integer = 0

        For Each item As CCBoxItem In ccb.Items

            Dim fieldName = item.Name.RemoveAfterMatch.ToLower

            Select ccb.GetItemCheckState(i)
                Case CheckState.Checked   : recs = recs.Where ( Function(x)     x.LockedFields.Contains(fieldName) )
                Case CheckState.Unchecked : recs = recs.Where ( Function(x) Not x.LockedFields.Contains(fieldName) )
            End Select
            i += 1
        Next
        Return recs

    End Function
 
    Function ApplyHdrFilter(recs As IEnumerable(Of Data_GridViewMovie), ccb As TriStateCheckedComboBox)

        Dim i As Integer = 0

        For Each item As CCBoxItem In ccb.Items

            Dim fieldName = item.Name.RemoveAfterMatch.ToLower

            Select ccb.GetItemCheckState(i)
                Case CheckState.Checked   : recs = recs.Where ( Function(x)     x.IsHdr )
                Case CheckState.Unchecked : recs = recs.Where ( Function(x) Not x.IsHdr )
            End Select
            i += 1
        Next
        Return recs

    End Function
   
    
     
    Function Filter(recs As IEnumerable(Of Data_GridViewMovie), leftOuterJoinTable As IEnumerable, fi As FilteredItems)

        If fi.Include.Count>0 Then
            recs = From m In recs
                    Group Join 
                        a In leftOuterJoinTable On a.fullpathandfilename Equals m.fullpathandfilename
                    Into
                        ResultList = Group
                    From
                        result In ResultList.DefaultIfEmpty
                    Where
                        fi.Include.Contains(If( result Is Nothing, "Unassigned", result.field ))
                    Select 
                        m
                    Distinct
        End If

        If fi.Exclude.Count>0 Then
            recs = From m In recs
                    Group Join 
                        a In leftOuterJoinTable On a.fullpathandfilename Equals m.fullpathandfilename
                    Into
                        ResultList = Group
                    From
                        result In ResultList.DefaultIfEmpty
                    Where
                        Not fi.Exclude.Contains(If( result Is Nothing, "Unassigned", result.field ))
                    Select 
                        m
                    Distinct
        End If

        Return recs
    End Function

    Property Xbmc_DifferentTitles As List(Of String)

    Sub Handle_XbmcMcMoviesChanged
        For Each m In MovieCache
            Try
                m.XbmcMovie = XbmcMcMovies(m.MoviePathAndFileName.ToUpper)
            Catch
            End Try
        Next
        Xbmc_DifferentTitles = (From x In MovieCache Where Not IsNothing(x.XbmcMovie) AndAlso Not x.XbmcMovie.title=x.title Select x.MoviePathAndFileName).ToList
    End Sub

    Sub Handle_XbmcOnlyMoviesChanged
        
    End Sub

#End Region

End Class
