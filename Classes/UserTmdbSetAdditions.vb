Public Class UserTmdbSetAddition



    Property TmdbId As Integer

    Property Msi As MovieSetInfo


    Sub New(_TmdbId As Integer, _Msi As MovieSetInfo)
        TmdbId = _TmdbId
        Msi = _Msi
    End Sub


End Class
