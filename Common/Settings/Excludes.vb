Imports System.Xml
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq


Public Class ExcludeTag

    Property Name As String

    Public Sub New
    End Sub

    Public Sub New(name As String)
        Me.Name = name
    End Sub

    Public Sub New(node As XmlNode)
        Load(node)
    End Sub

    Public Sub Load(node As XmlNode)
        Name = node.Attributes("name").Value
    End Sub

    Public Function GetChild(doc As XmlDocument) As XmlElement
        Dim child As XmlElement = doc.CreateElement("Exclude")

        child.SetAttribute("name" , Name )

        Return child
    End Function
End Class


Public Class Excludes

    Property Type  As String = ""
    Property Items As New List(Of ExcludeTag)


    Public Sub New(type As String)
        Me.Type = type
    End Sub


    Public Sub Load(node As XmlNode)
        For Each child As XmlNode In node.ChildNodes
            Items.Add(New ExcludeTag(child))
        Next
    End Sub

    Public Sub Clear()
        Items.Clear()
    End Sub


    Public Function GetChild(doc As XmlDocument) As XmlElement

        Dim child As XmlElement = doc.CreateElement("Exclude" + Type)

        For Each item In Items
            child.AppendChild(item.GetChild(doc))
        Next

        Return child
    End Function


    Function ExtactMatch(name As String) As Boolean
        
        Dim q = From x In Items Select x Where x.Name.ToUpper = name.ToUpper

        Return (q.Count=1)
    End Function

    Function ContainsMatch(name As String) As Boolean
        
        For Each curitem In Items 
            Dim newItem As String = "\" & curitem.Name & "\"
            If name.ToUpper.Contains(newItem.ToUpper) Then Return True
        Next
        
        Return False
    End Function

    Function Match(name As String) As Boolean
        
        Dim LastFolder As String = Utilities.GetLastFolderInPath(name)

        If ExtactMatch(LastFolder) Then Return True
        
        If ContainsMatch(name) Then Return True

        Dim q = From x In Items Select x Where x.Name.GetLastChar="*" and LastFolder.ToUpper.IndexOf(x.Name.ToUpper.RemoveLastChar)=0

        Return (q.Count=1) 
    End Function

    Sub PopTextBox(tb As TextBox)
        tb.Clear

        For Each item In Items
            tb.AppendLine(item.Name)
        Next
    End Sub

    Sub PopFromTextBox(tb As TextBox)

        TidyTextBoxItems(tb)

        Items.Clear

        For Each item In tb.Lines
            Items.Add(New ExcludeTag(item))
        Next
    End Sub


    Sub TidyTextBoxItems(tb As TextBox)
        
        Dim Lines As New List(Of String)

        For Each item In tb.Lines
            item = item.Trim.ToUpper
            If item<>"" Then
                If Not Lines.Contains(item) Then
                    Lines.Add(item)
                End If
            End If
        Next

        tb.Lines = Lines.ToArray
    End Sub


    Function Changed(tb As TextBox) As Boolean

        TidyTextBoxItems(tb)

        For Each item In tb.Lines
            If Not ExtactMatch(item) Then Return True
        Next    

        For Each item In Items
            If Not tb.Lines.Contains(item.Name) Then Return True
        Next    

        Return False    
    End Function

End Class
