
Imports System.Runtime.CompilerServices
Imports Media_Companion
Imports Media_Companion.Tasks

Public Module ProtoImageExtentions

    <Extension()>
    Public Sub DownloadFile(ByVal pImage As ProtoXML.ProtoImage)
        Common.Tasks.Add(New DownloadFileTask(pImage.Url, pImage.Path))
    End Sub



End Module
