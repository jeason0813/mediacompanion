
Public Structure str_MediaNFOVideo
    Dim width As String
    Dim height As String
    Dim aspect As String
    Dim codec As String
    Dim formatinfo As String
    Dim duration As String
    Dim bitrate As String
    Dim bitratemode As String
    Dim bitratemax As String
    Dim container As String
    Dim codecid As String
    Dim codecinfo As String
    Dim scantype As String
    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        width = ""
        height = ""
        aspect = ""
        codec = ""
        formatinfo = ""
        duration = ""
        bitrate = ""
        bitratemode = ""
        bitratemax = ""
        container = ""
        codecid = ""
        codecinfo = ""
        scantype = ""
    End Sub

    Shared Widening Operator CType(ByVal Input As Nfo.VideoDetails) As str_MediaNFOVideo
        Dim Temp As New str_MediaNFOVideo(True)

        Temp.width = Input.Width
        Temp.height = Input.Height
        Temp.aspect = Input.Aspect
        Temp.codec = Input.Codec
        Temp.formatinfo = Input.FormatInfo
        Temp.duration = Input.DurationInSeconds
        Temp.bitrate = Input.Bitrate
        Temp.bitratemode = Input.Bitrate
        Temp.bitratemax = Input.BitrateMax
        Temp.container = Input.Container
        Temp.codecid = Input.CodecId
        Temp.codecinfo = Input.CodecInfo
        Temp.scantype = Input.ScanType

        Return Temp
    End Operator

    Shared Widening Operator CType(ByVal Input As str_MediaNFOVideo) As Nfo.VideoDetails
        Dim Temp As New Nfo.VideoDetails

        Temp.Width.Value = Input.width
        Temp.Height.Value = Input.height
        Temp.Aspect.Value = Input.aspect
        Temp.Codec.Value = Input.codec
        Temp.FormatInfo.Value = Input.formatinfo
        Temp.DurationInSeconds.Value = Input.duration
        Temp.Bitrate.Value = Input.bitrate
        Temp.BitrateMode.Value = Input.bitratemode
        Temp.BitrateMax.Value = Input.bitratemax
        Temp.Container.Value = Input.container
        Temp.CodecId.Value = Input.codecid
        Temp.CodecInfo.Value = Input.codecinfo
        Temp.ScanType.Value = Input.scantype

        Return Temp
    End Operator

End Structure
