'Class which contains all the urls so if anything changes we change it in just one single place
Public NotInheritable Class URLs

    Public Shared ReadOnly Property TMdbAPIKey() As String
        Get
            Return "f7f51775877e0bb6703520952b3c7840"
        End Get
    End Property

    'Public Shared ReadOnly Property TMdbMovieLookup(ByVal sID As String)
    '    Get
    '        Return (String.Format("http://api.themoviedb.org/2.1/Movie.imdbLookup/en/xml/{0}/{1}", TMdbAPIKey, sID))
    '    End Get
    'End Property

    'Public Shared ReadOnly Property TMdbGetInfo(ByVal sID As String)
    '    Get
    '        Return (String.Format("http://api.themoviedb.org/2.1/Movie.getInfo/en/xml/{0}/{1}", TMdbAPIKey, sID))
    '    End Get
    'End Property

    Public Shared ReadOnly Property TVdbGetSeries(ByVal sSeriesName As String)
        Get
            Return String.Format("http://www.thetvdb.com/api/GetSeries.php?seriesname={0}&language=all", sSeriesName)
        End Get
    End Property

    Public Shared ReadOnly Property TVdbBannersCache(ByVal sValue As String)
        Get
            Return String.Format("http://images.thetvdb.com/banners/_cache/{0}", sValue)
        End Get
    End Property

    Public Shared ReadOnly Property TVdbBanners(ByVal sValue As String)
        Get
            Return (String.Format("http://images.thetvdb.com/banners/{0}", sValue))
        End Get
    End Property

    Public Shared ReadOnly Property TVdbActorsXML(ByVal sActorID As String)
        Get
            Return (String.Format("http://thetvdb.com/api/6E82FED600783400/series/{0}/actors.xml", sActorID))
        End Get
    End Property

    Public Shared ReadOnly Property TVdbSeriesLanguageXML(ByVal sSeriesName As String, ByVal sLanguageCode As String)
        Get
            Return (String.Format("http://thetvdb.com/api/6E82FED600783400/series/{0}/{1}.xml", sSeriesName, sLanguageCode))
        End Get
    End Property

    Public Shared ReadOnly Property EpisodeGuide(ByVal sSeriesName As String, ByVal sLanguageCode As String)
        Get
            Return (String.Format("http://thetvdb.com/api/6E82FED600783400/series/{0}/all/{1}.zip", sSeriesName, sLanguageCode))
        End Get
    End Property

    Public Shared ReadOnly Property MoviePosterDBMovie()
        Get
            Return "http://www.movieposterdb.com/movie/"
        End Get
    End Property

    Public Shared ReadOnly Property MoviePosterDBPoster()
        Get
            Return "http://www.movieposterdb.com/posters/"
        End Get
    End Property

    Public Shared ReadOnly Property MoviePosterDBGroup()
        Get
            Return "http://www.movieposterdb.com/group/"
        End Get
    End Property

    Public Shared ReadOnly Property IMDBUrl(ByVal sIMDBID As String)
        Get
            Return (String.Format("http://www.imdb.com/title/{0}", sIMDBID))
        End Get
    End Property

    Public Shared ReadOnly Property IMDBAddPhotoGif()
        Get
            Return "http://i.media-imdb.com/images/tn15/addtiny.gif"
        End Get
    End Property

    Public Shared ReadOnly Property IMDBResume()
        Get
            Return "http://resume.imdb.com"
        End Get
    End Property

    Public Shared ReadOnly Property IMDBName(ByVal sNameID As String)
        Get
            Return (String.Format("http://www.imdb.com/name/{0}", sNameID))
        End Get
    End Property

    Public Shared ReadOnly Property IMDBMediaIndex(ByVal sIMDBId As String)
        Get
            Return (String.Format("{0}/mediaindex", IMDBUrl(sIMDBId)))
        End Get
    End Property

    Public Shared ReadOnly Property IMDBMediaIndexPoster(ByVal sIMDBId As String)
        Get
            Return (String.Format("{0}/mediaindex?refine=poster&ref_=ttmi_ref_pos", IMDBUrl(sIMDBId)))
        End Get
    End Property

    Public Shared ReadOnly Property IMDBMediaIndexPage(ByVal sIMDBId As String, ByVal iPageNo As Integer)
        Get
            Return (String.Format("{0}?page={1}", IMDBMediaIndex(sIMDBId), iPageNo.ToString()))
        End Get
    End Property

    Public Shared ReadOnly Property GoogleFanArt()
        Get
            Return "http://www.google.com/custom?hl=en&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="
        End Get
    End Property
End Class
