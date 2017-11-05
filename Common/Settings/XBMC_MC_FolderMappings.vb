Imports System.Xml
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Runtime.CompilerServices

Public Module LinkExt

    <Extension()> _
    Function FormatXbmcPath(ByVal sString As String) As String
        Dim s As String = sString

        If Pref.XBMC_Link_Use_Forward_Slash Then
            s = s.Replace("\","/")


            Dim firstSlash      As Integer = s.IndexOf( "/" )
            Dim protoSlash      As Integer = s.IndexOf(":/" )
            Dim protoSlashSlash As Integer = s.IndexOf("://")

            If (firstSlash=protoSlash+1) And protoSlashSlash=-1 Then
                s = s.ReplaceFirst(":/","://")
            End If

            'If s.IndexOf("smb:/")=0 And s.IndexOf("smb://")=-1 Then
            '    s = s.Replace("smb:/","smb://")
            'End If

        End If

        Return s
    End Function


    <Extension()> _
    Function ReplaceFirst(text As String, search As String, replace As String) As String

        Dim pos As Integer = text.IndexOf(search)

        If pos<0 Then Return text

        Return text.Substring(0, pos) + replace + text.Substring(pos + search.Length)

    End Function

End Module

Public Class XBMC_MC_FolderMapping

 '   XBMC_MC_FolderMappings As New Dictionary(Of String, String)
    Property MC   As String
    Property XBMC As String


    Public Sub New
    End Sub

    Public Sub New(node As XmlNode)
        Load(node)
    End Sub

    Public Sub New(ByVal MC_Folder As String, Optional ByVal XBMC_Folder As String="")
        MC   = MC_Folder
        XBMC = XBMC_Folder
    End Sub

    Public Sub Load(node As XmlNode)
        MC   = node.Attributes("MC"  ).Value
        XBMC = node.Attributes("XBMC").Value
    End Sub

    Public Function GetChild(doc As XmlDocument) As XmlElement
        Dim child As XmlElement = doc.CreateElement("XBMC_MC_FolderMapping")

        child.SetAttribute("MC"   , MC  )
        child.SetAttribute("XBMC" , XBMC)

        Return child
    End Function
End Class


Public Class XBMC_MC_FolderMappings

    Property Type  As String
    Property Items As New List(Of XBMC_MC_FolderMapping)

    Public Sub New
    End Sub

    Public Sub New(type As String)
        Me.Type = type
    End Sub

    Public Sub New(node As XmlNode,type As String)
        Me.New(type)
        Load(node)
    End Sub

    Public Sub New(From As XBMC_MC_FolderMappings)
        Me.Assign(From)
    End Sub

    Public Sub Load(node As XmlNode)
        Items.Clear
        For Each child As XmlNode In node.ChildNodes
            Items.Add(New XBMC_MC_FolderMapping(child))
        Next
    End Sub

    Public Function GetChild(doc As XmlDocument) As XmlElement

        Dim child As XmlElement = doc.CreateElement("XBMC_MC_" + Me.Type +"FolderMappings")

        For Each item In Items
            child.AppendChild(item.GetChild(doc))
        Next

        Return child
    End Function

    Public Function GetItem(key As String) As XBMC_MC_FolderMapping
        For Each item In Items
            If item.MC.ToUpper=key.ToUpper Then Return item
        Next
        Return Nothing
    End Function

    Public Function GetXBMC_MoviePath(McMoviePath As String) As String

        For Each FolderMapping In Items
            If McMoviePath.ToUpper.StartsWith(FolderMapping.MC.ToUpper) Then

                Dim file As String = Right(McMoviePath,McMoviePath.Length-FolderMapping.MC.Length)

                If file.StartsWith(Path.DirectorySeparatorChar) Then
                    file = file.Remove(0,1)
                End If

                Dim result As String = Path.Combine(FolderMapping.XBMC,file).FormatXbmcPath

                Return result
            End If
        Next

        Return Nothing
      End Function


    Public Function GetMC_MoviePath(XbMoviePath As String) As String

        Dim match As String = ""
        
        Dim MoviePath As String = XbMoviePath
        
        If MoviePath.IndexOf("stack://")=0 Then
            Dim LenStackPlusOne As Integer = Len("stack://")+1
            MoviePath = Mid(MoviePath,LenStackPlusOne,MoviePath.IndexOf(",")-LenStackPlusOne)
        End If


        For Each FolderMapping In Items
            If MoviePath.ToUpper.StartsWith(FolderMapping.XBMC.ToUpper) Then

                Dim file As String = Right(MoviePath,MoviePath.Length-FolderMapping.XBMC.Length)

                If file.StartsWith(Path.DirectorySeparatorChar) Then
                    file = file.Remove(0,1)
                End If

                Return Path.Combine(FolderMapping.MC,file ).ToUpper
            End If
        Next

        'Missing folder mapping -> Assume same
        Return XbMoviePath.ToUpper
      End Function

    Public Function GetMC_MovieFolder(McMoviePath As String) As String

        For Each FolderMapping In Items
            If McMoviePath.ToUpper.StartsWith(FolderMapping.MC.ToUpper) Then
                Return FolderMapping.MC.ToUpper
            End If
        Next

        Return Nothing
      End Function

    ReadOnly Property MC_Folders As List(Of String)
        Get
            Dim MovFoldList As New List(Of String)
            For Each rtpath In Pref.movieFolders 
                If rtpath.selected Then
                    MovFoldList.Add(rtpath.rpath)
                End If
            Next
            MovFoldList.AddRange(Pref.offlinefolders)
            Return IIf(Me.Type="Movie",MovFoldList,Pref.tvFolders) '(Me.Type="Movie",Pref.movieFolders,Pref.tvFolders)
        End Get
    End Property

    ReadOnly Property MappedMC_Folders As List(Of String)
        Get
            Return (From X In Items Select X.MC).ToList
        End Get
    End Property

    Public Sub IniFolders
            RemoveInvalidFolders
            AddMissingFolders
    End Sub

    Public Sub RemoveInvalidFolders()
        'Dim citems As Integer = Items.Count
        For Each item In Items.ToList
            If Not MC_Folders.Contains(item.MC) Then
                Items.Remove(item)
                'citems = citems - 1
                'If citems = 0 Then Exit For
            End If
        Next
    End Sub

    Public Sub AddMissingFolders
        For Each item In MC_Folders
            If Not MappedMC_Folders.Contains(item) Then
                Items.Add(New XBMC_MC_FolderMapping(item))
            End If
        Next
    End Sub
    
    Public Sub SetSame
        For Each item In Items
            item.XBMC = item.MC
        Next
    End Sub

    Public Function Initialised As Boolean

         If Items.Count=0 Then Return False

        For Each item In Items
            If IsNothing(item.XBMC) Then Return False
            If item.XBMC.Trim="" Then Return False
        Next   

        Return True    
    End Function


    Public Sub ClearXBMC
        For Each item In Items
            item.XBMC = ""
        Next
    End Sub

    Public Sub Assign(From As XBMC_MC_FolderMappings)
        Me.Items.Clear
        Me.Type = From.Type
        
        For Each Item In From.Items
            Dim x As XBMC_MC_FolderMapping = New XBMC_MC_FolderMapping(Item.MC,Item.XBMC)
            Me.Items.Add(x)
        Next
    End Sub
    
    Public Function Changed(From As XBMC_MC_FolderMappings) As Boolean

        If Me.Type <> From.Type Then Return True
        
        If Me.Items.Count <> From.Items.Count Then Return True
 
        Dim i=0

        For Each Item In From.Items
            If Me.Items(i).MC<>Item.MC OrElse Me.Items(i).XBMC<>Item.XBMC Then Return True
            i+=1
        Next

        Return False
    End Function
    
End Class
