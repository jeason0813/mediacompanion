Imports ProtoXML

Public Class VideoDetails
    Inherits ProtoPropertyGroup

    Public Property Codec As New ProtoProperty(Me, "codec", "")
    Public Property Aspect As New ProtoProperty(Me, "aspect", "")
    Public Property Width As New ProtoProperty(Me, "width", "")
    Public Property Height As New ProtoProperty(Me, "height", "")
    Public Property DurationInSeconds As New ProtoProperty(Me, "durationinseconds", "1")
    Public Property FormatInfo As New ProtoProperty(Me, "formatinfo", "")
    Public Property Bitrate As New ProtoProperty(Me, "bitrate", "")
    Public Property BitrateMode As New ProtoProperty(Me, "bitratemode", "")
    Public Property BitrateMax As New ProtoProperty(Me, "bitratemax", "")
    Public Property Container As New ProtoProperty(Me, "Container", "")
    Public Property CodecId As New ProtoProperty(Me, "codecid", "")
    Public Property CodecInfo As New ProtoProperty(Me, "codecinfo", "")
    Public Property ScanType As New ProtoProperty(Me, "scantype", "")
    Property NumVideoBits As Integer = -1

    Public ReadOnly Property VideoResolution As Integer
        Get
            Try
                If String.IsNullOrEmpty(Width.Value) or String.IsNullOrEmpty(Height.Value) Then Return -1
                Dim w As Integer = Convert.ToInt32(Width .Value)
                Dim h As Integer = Convert.ToInt32(Height.Value)

                If w =   0 Or  h =  0  Then Return -1
                If w<= 720 AndAlso h<=480  Then Return 480
                If w<= 768 AndAlso h<=576  Then Return 576
                If w<= 960 AndAlso h<=544  Then Return 540
                If w<=1600 AndAlso h<=720  Then Return 720  'If w<=1280 And h<=720  Then Return 720
                If w<=1920 AndAlso h<=1080 Then Return 1080
                Return 2160
            Catch
                Return -1
            End Try
        End Get
    End Property

    'Threw "Expanded Properties cannot be initialized" error
    'Public ReadOnly Property possibleResolutions As String() = {"480", "540", "576", "720", "1080"}
   
    Private _possibleResolutions As String() = {"480", "540", "576", "720", "1080"}

    Public ReadOnly Property possibleResolutions As String()
        Get
            Return _possibleResolutions 
        End Get

    End Property
 
    
    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
        Return New VideoDetails
    End Function
End Class
