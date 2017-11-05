
Public Structure str_MediaNFOSubtitles
    Dim language As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        language = ""
    End Sub

    Shared Widening Operator CType(ByVal Input As Nfo.SubtitleDetails) As str_MediaNFOSubtitles
        Dim Temp As New str_MediaNFOSubtitles(True)

        Temp.language = Input.Language


        Return Temp
    End Operator

    Shared Widening Operator CType(ByVal Input As str_MediaNFOSubtitles) As Nfo.SubtitleDetails
        Dim Temp As New Nfo.SubtitleDetails

        Temp.Language.Value = Input.language

        Return Temp
    End Operator

End Structure
