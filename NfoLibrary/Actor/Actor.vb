Imports ProtoXML
Imports Alphaleonis.Win32.Filesystem
Imports Media_Companion
Imports Media_Companion.Tasks

Public Class Actor
    Inherits ProtoPropertyGroup

    Public Property Name As New ProtoProperty(Me, "name")
    Public Property Role As New ProtoProperty(Me, "role")
    Public Property Thumb As New ProtoProperty(Me, "thumb")
    Public Property Id As New ProtoProperty(Me, "actorid")
    Public Property SortOrder As New ProtoProperty(Me, "order")
    Public Property Source As New ProtoProperty(Me, "source")

    Public Property actorname As String
        Get
            Return Me.Name.Value
        End Get
        Set(ByVal value As String)
            Me.Name.Value = value
        End Set
    End Property

    Public Property actorrole As String
        Get
            Return Me.Role.Value
        End Get
        Set(ByVal value As String)
            Me.Role.Value = value
        End Set
    End Property

    Public Property actorthumb As String
        Get
            Return Me.Thumb.Value
        End Get
        Set(ByVal value As String)
            Me.Thumb.Value = value
        End Set
    End Property

    Public Property actorid As String
        Get
            Return Me.ID.Value
        End Get
        Set(ByVal value As String)
            Me.ID.Value = value
        End Set
    End Property

    Public Property order As String
        Get
            Return Me.SortOrder.Value
        End Get
        Set(ByVal value As String)
            Me.SortOrder.Value = value
        End Set
    End Property

    Public Sub New()
        MyBase.New(Nothing, Nothing)

        Me.Node = New XElement("actor")
        'Throw New NotImplementedException()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)
    End Sub


    Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
        Return New Actor
    End Function

    Public Sub DownloadThumb()
        Common.Tasks.Add(New DownloadFileTask(Me.Thumb.Value, Path.Combine(Me.FolderPath, ".actor\" & Me.actorname.Replace(" ", "_"))))
    End Sub
End Class
