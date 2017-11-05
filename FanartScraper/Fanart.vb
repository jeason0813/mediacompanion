Imports ProtoXML

Public Class Fanart
    Inherits ProtoFile

    Public Sub New()
        MyBase.New("fanart")
    End Sub

    Public Property ClearLogos As New ProtoGroupList(Of ProtoImage)(Me, "clearlogos", "clearlogo", New ProtoImage(Nothing, "clearlogo", Nothing) With {.FileName = "logo.png"})
    Public Property ClearArts As New ProtoGroupList(Of ProtoImage)(Me, "cleararts", "clearart", New ProtoImage(Nothing, "clearart", Nothing) With {.FileName = "clearart.png"})
    Public Property TvThumbs As New ProtoGroupList(Of ProtoImage)(Me, "tvthumbs", "tbthumb", New ProtoImage(Nothing, "tvthumb", Nothing) With {.FileName = "tvthumb.png"})
    Public Property SeasonThumbs As New ProtoGroupList(Of ProtoImage)(Me, "seasonthumbs", "seasonthumb", New ProtoImage(Nothing, "seasonthumb", Nothing) With {.FileName = "seasonX.png"})

End Class
