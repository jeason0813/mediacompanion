Public Class ProtoGroupList(Of T As IProtoXChild)
    Inherits ProtoXChildBase
    Implements IList(Of T)


    Public Property ListItemNodeName As String

    Private Blank As ProtoXChildBase

    Public Sub New(ByRef ParentClass As IProtoXBase, ByVal GroupNodeName As String, ByRef ListItemNodeName As String, ByVal Blank As IProtoXChild)
        MyBase.New(ParentClass, GroupNodeName)

        Me.NodeName = GroupNodeName
        Me.Node = New XElement(GroupNodeName)
        Me.ListItemNodeName = ListItemNodeName
        Me.ParentClass = ParentClass
        Blank.ParentClass = Me
        Me.Blank = Blank
    End Sub

    Protected Sub ProcessAdd(ByVal Item As T)
        Item.ParentClass = Me
        If Not Me.Node.Nodes.Contains(Item.Node) Then
            If Item.Node IsNot Nothing Then
                If Item.Node.Parent IsNot Nothing Then
                    Item.Node.Remove()
                End If
            End If
            Me.Node.Add(Item.Node)
        End If
        Item.ResolveAttachment(Item)

        List.Add(Item)
    End Sub

    Public List As New List(Of T)

    Sub Add(ByVal item As T) Implements ICollection(Of T).Add
        item.ParentClass = Me
        If Not Me.Node.Nodes.Contains(item.Node) Then
            If item.Node IsNot Nothing Then
                If item.Node.Parent IsNot Nothing Then
                    item.Node.Remove()
                End If
            End If
            Me.Node.Add(item.Node)
        End If
        item.ResolveAttachment(item)

        List.Add(item)
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        For Each Node As XElement In (From X As XElement In ParentClass.Node.Nodes Where X.Name = Me.NodeName)
            Node.Remove()
        Next

        List.Clear()
    End Sub

    Public Function Contains(ByVal item As T) As Boolean Implements ICollection(Of T).Contains
        Return List.Contains(item)
    End Function

    Public Sub CopyTo(ByVal array() As T, ByVal arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        List.CopyTo(array)
    End Sub

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return List.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function Remove(ByVal item As T) As Boolean Implements ICollection(Of T).Remove
        If List.Contains(item) Then
            item.Node.Remove()
            List.Remove(item)

            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return List.GetEnumerator
    End Function

    Public Function IndexOf(ByVal item As T) As Integer Implements IList(Of T).IndexOf
        If item Is Nothing Then Return Nothing

        Return List.IndexOf(item)
    End Function

    Public Sub Insert(ByVal index As Integer, ByVal item As T) Implements IList(Of T).Insert
        List.Item(index).Node.AddBeforeSelf(item.Node)
        List.Insert(index, item)
    End Sub

    Default Public Property Item(ByVal index As Integer) As T Implements IList(Of T).Item
        Get
            If List.Count > index Then
                Return List.Item(index)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As T)
            List.Item(index) = value
        End Set
    End Property

    Public Sub RemoveAt(ByVal index As Integer) Implements IList(Of T).RemoveAt
        List.Item(index).Node.Remove()
        List.RemoveAt(index)
    End Sub

    Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Return List.GetEnumerator()
    End Function

    Public Overrides Function CreateNew() As IProtoXChild
        Return New ProtoGroupList(Of T)(Me.ParentClass, Me.NodeName, Me.ListItemNodeName, Me.Blank)
    End Function

    Public Overrides Sub ProcessNode(ByRef Element As XElement)
        Me.Node = Element

        Dim ChildProperty As IProtoXChild
        For Each Child As XElement In Me.Node.Nodes

            If Child.Name.ToString.ToLower = Me.ListItemNodeName.ToLower Then
                Dim NewItem As IProtoXChild = Me.Blank.CreateNew()
                NewItem.ParentClass = Me
                NewItem.ProcessNode(Child)
                List.Add(NewItem)
            ElseIf Me.ChildrenLookup.ContainsKey(Child.Name.ToString.ToLower) Then
                ChildProperty = Me.ChildrenLookup.Item(Child.Name.ToString.ToLower)

                ChildProperty.ProcessNode(Child)
            End If
        Next
    End Sub

End Class


