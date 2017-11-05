
Public Structure str_TVSeries
    Dim SeriesId As String
    Dim Language As String
    Sub New(Optional ByVal _seriesId As String = "", Optional ByVal _lang As String = "")
        SeriesId = _seriesId
        Language = _lang
    End Sub
End Structure
