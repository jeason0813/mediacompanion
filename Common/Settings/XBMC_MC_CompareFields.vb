Imports System.Xml
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq
'Imports System.IO


Public Class XBMC_MC_CompareField


    Property Field   As String
    Property Enabled As Boolean


    Public Sub New
    End Sub

    Public Sub New(node As XmlNode)
        Load(node)
    End Sub

    Public Sub New(ByVal field As String, Optional ByVal enabled As Boolean=False)
        Me.Field   = field
        Me.Enabled = enabled
    End Sub

    Public Sub Load(node As XmlNode)
        Field   = node.Attributes("Field"  ).Value
        Enabled = node.Attributes("Enabled").Value
    End Sub

    Public Function GetChild(doc As XmlDocument) As XmlElement
        Dim child As XmlElement = doc.CreateElement("XBMC_MC_CompareField")

        child.SetAttribute("Field"   , Field  )
        child.SetAttribute("Enabled" , Enabled)

        Return child
    End Function
End Class


Public Class XBMC_MC_CompareFields

    Property Type  As String
    Property Items As New List(Of XBMC_MC_CompareField)

    Property     CachedFields As String() = { "Set", "Title", "Year", "Rating", "Votes", "Outline"    , "Plot" }
    Property XbmcCachedFields As String() = { "set", "title", "year", "rating", "votes", "plotoutline", "plot" }


    Public Sub New
        For Each item In CachedFields
            Items.Add(New XBMC_MC_CompareField(item,True))
        Next
    End Sub

    Public Sub New(type As String)
        Me.New
        Me.Type = type
    End Sub

    Public Sub New(node As XmlNode,type As String)
        Me.New(type)
        Load(node)
    End Sub

    Public Sub New(From As XBMC_MC_CompareFields)
        Me.Assign(From)
    End Sub

    Public Sub Load(node As XmlNode)
       For Each child As XmlNode In node.ChildNodes

            Dim o = New XBMC_MC_CompareField(child)

            Dim item = Items.Find(Function(x) x.Field=o.Field)

            If Not IsNothing(item) Then
                item.Enabled = o.Enabled
            End If
        Next
    End Sub

    Public Function GetChild(doc As XmlDocument) As XmlElement

        Dim child As XmlElement = doc.CreateElement("XBMC_MC_" + Me.Type +"CompareField")

        For Each item In Items
            child.AppendChild(item.GetChild(doc))
        Next

        Return child
    End Function

    Public Function GetItem(key As String) As XBMC_MC_CompareField
        For Each item In Items
            If item.Field.ToUpper=key.ToUpper Then Return item
        Next
        Return Nothing
    End Function
 
    Public Sub Assign(From As XBMC_MC_CompareFields)
        Me.Items.Clear
        Me.Type = From.Type
        
        For Each Item In From.Items
            Dim x As XBMC_MC_CompareField = New XBMC_MC_CompareField(Item.Field,Item.Enabled)
            Me.Items.Add(x)
        Next
    End Sub
    
    Public Function Changed(From As XBMC_MC_CompareFields) As Boolean

        If Me.Type <> From.Type Then Return True
        
        If Me.Items.Count <> From.Items.Count Then Return True
 
        Dim i=0

        For Each Item In From.Items
            If Me.Items(i).Field<>Item.Field OrElse Me.Items(i).Enabled<>Item.Enabled Then Return True
            i+=1
        Next

        Return False
    End Function

    Public Function Get_Xbmc_Fields As String()

        Dim lst As New List(Of String)

        Dim q = (From x In Items Where x.Enabled=True).ToList

        For Each item In q
            Dim i = Array.IndexOf(CachedFields,item.Field)

            If i>-1 Then
                lst.Add(XbmcCachedFields(i))
            End If
        Next

        Return lst.ToArray

    End Function

End Class
