Public Class TmdbSetMissingMovie


'    Property Mso As MovieSetInfo
    Property Movie As CollectionMovie

    Property DgvMovie As New Data_GridViewMovie


    Sub New(_Mso As MovieSetInfo, _Movie As CollectionMovie, _Movies As Movies )

        Movie               = _Movie
        DgvMovie.title      = _Movie.MovieTitle
        DgvMovie.SetName    = _Mso.MovieSetName
        DgvMovie.TmdbSetId  = _Mso.TmdbSetId
        DgvMovie.filename   = _Movie.TmdbMovieId.ToString
        DgvMovie.tmdbid     = _Movie.TmdbMovieId
        DgvMovie.year       = _Movie.ReleaseYear
        DgvMovie.oMovies    = _Movies
    End Sub




End Class
