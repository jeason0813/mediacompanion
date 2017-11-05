Imports Alphaleonis.Win32.Filesystem

Public Class ProtoImage
    Inherits ProtoXChildBase


    'Private _FolderPath As String
    'Public Property FolderPath As String
    '    Get
    '        If _FolderPath Is Nothing Then
    '            Dim Done As Boolean
    '            Do Until Done
    '                If TypeOf ParentClass Is ProtoFile Then
    '                    If ParentClass.NodeName = "tvshow" Then
    '                        Dim TempFile As ProtoFile = ParentClass
    '                        _FolderPath = TempFile.FolderPath
    '                    End If
    '                End If
    '            Loop
    '        End If
    '        Return _FolderPath
    '    End Get
    '    Set(ByVal value As String)
    '        _FolderPath = value
    '    End Set
    'End Property

    Private _FileName As String
    Public Property FileName As String
        Get
            Return _FileName
        End Get
        Set(value As String)
            _FileName = value
        End Set
    End Property


    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String, ByVal DefaultPath As String, Optional ByVal SortOrder As Integer = -1, Optional ByVal ExtraNodes As String() = Nothing)
        MyBase.New(Parent, NodeName)

        If TypeOf ParentClass Is ProtoFile Then
            Dim TempFile As ProtoFile = ParentClass
            Me.FolderPath = TempFile.FolderPath
        End If

        Me.Node = New XElement(Me.NodeName)

        Me.ParentClass = ParentClass
        Me.DefaultPath = DefaultPath

        Me.SortIndex = SortOrder
    End Sub



    Public PathAttribute As XAttribute
    Public Property Path As String
        Get
            'If _PathOveride IsNot Nothing Then Return _PathOveride

            If PathAttribute IsNot Nothing Then
                Return PathAttribute.Value
            End If

            Dim TempPath As String
            If Me.FolderPath Is Nothing OrElse Me.FileName Is Nothing Then Return Nothing

            TempPath = Alphaleonis.Win32.Filesystem.Path.Combine(Me.FolderPath, Me.FileName)

            'If File.Exists(TempPath) Then
            Return TempPath
            'End If

            'Return Nothing
        End Get
        Set(ByVal value As String)
            '_PathOveride = value
            Me.FileName = Alphaleonis.Win32.Filesystem.Path.GetFileName(value)
            Me.FolderPath = value.Replace(Me.FileName, "")
            If PathAttribute IsNot Nothing Then
                PathAttribute.Value = value
            Else
                PathAttribute = New XAttribute("path", value)
                If Me.Node IsNot Nothing Then
                    Me.Node.Add(PathAttribute)
                Else
                    Me.Node = New XElement(Me.NodeName)
                    Me.Node.Add(PathAttribute)
                End If
            End If
            'If File.Exists(value) Then
            '    _Image = Drawing.Bitmap.FromFile(value)
            'End If
        End Set
    End Property

    Public Property DefaultPath As String

    Private _Image As Drawing.Image
    Public Property Image As Drawing.Image
        Get
            If _Image Is Nothing Then
                If File.Exists(Me.Path) Then
                    _Image = Drawing.Bitmap.FromFile(Me.Path)
                Else
                    _Image = Drawing.Bitmap.FromFile(Me.DefaultPath)
                End If
            End If
            Return _Image
        End Get
        Set(ByVal value As Drawing.Image)
            _Image = value
            Try
                _Image.Save(Path)
            Catch
            End Try
        End Set
    End Property

  

    Public ReadOnly Property Exists As Boolean
        Get
            Return File.Exists(Alphaleonis.Win32.Filesystem.Path.Combine(Me.FolderPath, Me.FileName))
        End Get
    End Property

    Public UrlAttribute As XAttribute
    Public Property Url As String
        Get
            If UrlAttribute IsNot Nothing Then
                Return UrlAttribute.Value
            Else
                Return Nothing
            End If
        End Get
        Set(value As String)
            If UrlAttribute IsNot Nothing Then
                UrlAttribute.Value = value
            Else
                UrlAttribute = New XAttribute("url", value)
                Me.Node.Add(UrlAttribute)
            End If
        End Set
    End Property


    Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)
        If Element.Attribute("url") IsNot Nothing Then
            Me.Url = Element.Attribute("url")
        End If

        If Element.Attributes.Contains(PathAttribute) Then
            Me.Path = Element.Attribute("path")
        End If

    End Sub

    Public Overrides Function CreateNew() As IProtoXChild
        Return New ProtoImage(Me, Me.NodeName, Me.DefaultPath, Me.SortIndex) With {.FileName = Me.FileName}
    End Function
End Class
