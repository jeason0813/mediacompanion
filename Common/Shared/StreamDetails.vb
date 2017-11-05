Imports ProtoXML

Public Class StreamDetails
    Inherits ProtoPropertyGroup


    Public Property Video As New VideoDetails '(Me, "video")
    Public Property Audio As New List(Of AudioDetails   ) 'As New AudioList(Me, "audio")
    Public Property Subtitles As New List(Of SubtitleDetails) 'As New SubtitleList(Me, "subtitle")

    Private _assignedDefaultAudioTrack As Boolean = False
    Private _defaultAudioTrack As AudioDetails
    Private _assignedDefaultSubTrack As Boolean = False
    Private _defaultSubTrack As SubtitleDetails

    Public Property DefaultAudioTrack As AudioDetails
        Get
            If Not _assignedDefaultAudioTrack Then
                _assignedDefaultAudioTrack = True
                If Audio.Count > 0 Then
                    _defaultAudioTrack = (From x In Audio Where x.DefaultTrack.Value="Yes").FirstOrDefault
                    If IsNothing(_defaultAudioTrack) Then
                        _defaultAudioTrack = Audio(0)
                    End If
                End If
            End If
            Return _defaultAudioTrack
        End Get
        Set
            _assignedDefaultAudioTrack = True
            _defaultAudioTrack = Value
        End Set
    End Property   

    Public Property DefaultSubTrack As SubtitleDetails
        Get
            If Not _assignedDefaultSubTrack Then
                _assignedDefaultSubTrack = True
                If Subtitles.Count > 0 Then
                    _defaultSubTrack = (From x In Subtitles Where x.Primary = True).FirstOrDefault
                    If IsNothing(_defaultSubTrack) Then
                        _defaultSubTrack = Subtitles(0)
                    End If
                End If
            End If
            Return _defaultSubTrack
        End Get
        Set
            _assignedDefaultSubTrack = True
            _defaultSubTrack = Value
        End Set
    End Property

    Public Sub New()
        MyBase.New(Nothing, Nothing)
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub

    Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
        Return New StreamDetails
    End Function
    
End Class
