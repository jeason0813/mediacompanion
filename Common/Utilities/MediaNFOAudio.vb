
Public Structure str_MediaNFOAudio
    Dim language As String
    Dim codec As String
    Dim channels As String
    Dim bitrate As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        language = ""
        codec = ""
        channels = ""
        bitrate = ""
    End Sub

    Shared Widening Operator CType(ByVal Input As Nfo.AudioDetails) As str_MediaNFOAudio
        Dim Temp As New str_MediaNFOAudio(True)
        Temp.codec = Input.Codec
        Temp.bitrate = Input.Bitrate
        Temp.language = Input.Language
        Temp.channels = Input.Channels

        Return Temp
    End Operator

    Shared Widening Operator CType(ByVal Input As str_MediaNFOAudio) As Nfo.AudioDetails
        Dim Temp As New Nfo.AudioDetails

        Temp.Codec.Value = Input.codec
        Temp.Bitrate.Value = Input.bitrate
        Temp.Channels.Value = Input.channels
        Temp.Language.Value = Input.language

        Return Temp
    End Operator
End Structure
