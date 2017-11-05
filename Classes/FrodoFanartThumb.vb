Imports System.Xml

'
' Required format:
'
'<fanart url="">
'    <thumb dim="" colors="" preview="http://cf2.imgobject.com/t/p/w780/lt2ZnKubSnJhAA9d1FKz1TejZ2x.jpg">http://cf2.imgobject.com/t/p/original/lt2ZnKubSnJhAA9d1FKz1TejZ2x.jpg</thumb>
'    <thumb dim="" colors="" preview="http://cf2.imgobject.com/t/p/w780/wGRW6AA3JZDEzSeuwU5AkB0Jf7x.jpg">http://cf2.imgobject.com/t/p/original/wGRW6AA3JZDEzSeuwU5AkB0Jf7x.jpg</thumb>
'</fanart>
'

Public Class FrodoFanartThumbs
    Property Thumbs As New List(Of FrodoFanartThumb)

    Public Function GetChild(doc As XmlDocument)
        Dim child = doc.CreateElement("fanart")
        child.SetAttribute("url","")
        For Each item In Thumbs
            child.AppendChild(item.GetChild(doc))
        Next
        Return child
    End Function
    
    Public Sub Load(node As XmlNode)
        For Each child As XmlNode In node.ChildNodes
            Try                 'catch put in place for nfo's not created by MC, where <fanart> node is invalid.
                If ((child.NodeType.ToString() <> "Element") And (child.Name <> "thumb")) Then
                    Continue For
                End If
                If IsNothing(child.Attributes.GetNamedItem("preview")) Then
                    child.Attributes.Append(child.OwnerDocument.CreateAttribute("preview"))
                    child.Attributes("preview").Value = ""
                End If
                Thumbs.Add(New FrodoFanartThumb(child.Attributes("preview").Value,child.InnerText))
            Catch               'this allows these nfo's to be loaded by MC.  Would be good to rebuild nfo if this occurs.  To Do...
            End Try
        Next
    End Sub

End Class


Public Class FrodoFanartThumb 
    Inherits FrodoPosterThumb

    Property preview=""

    Sub New(preview As String, url As String)
        Me.preview = preview
        Me.Url     = url
    End Sub


    '<thumb dim="" colors="" preview="http://cf2.imgobject.com/t/p/w780/vOfEyKHfE8vNWzMYloDCXELiapP.jpg">
    Public Function GetChild(doc As XmlDocument)

        Dim child = doc.CreateElement("thumb")

        child.SetAttribute("dim"    , ""     )
        child.SetAttribute("colors" , ""     )
        child.SetAttribute("preview", preview)
        child.InnerText = Url

        Return child
    End Function

End Class
