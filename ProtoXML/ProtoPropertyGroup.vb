Public Class ProtoPropertyGroup
    Inherits ProtoXChildBase


    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
        MyBase.New(Parent, NodeName)

        If Parent Is Nothing Then Exit Sub

        Me.Node = New XElement(NodeName)
        Me.ParentClass.Node.Add(Me.Node)
    End Sub



    Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)
        'If Not Element.Name = Node.Name Then
        '    Throw New Exception("Wrong element sent")
        'End If
        'If Me.Orphan Then
        '    Me.Node = Element
        'End If

        'If Me.ParentClass IsNot Nothing Then
        '    If Me.Node.Parent IsNot Nothing Then
        '        If Not Me.Node.Parent.Nodes.Contains(Element) Then
        '            If Element.Parent IsNot Nothing Then
        '                Element.Remove()
        '            End If
        '            ParentClass.Node.Add(Element)

        '            If Me.Node.Parent IsNot Nothing Then
        '                Me.Node.Remove()
        '            End If
        '        End If
        '    End If
        'End If
        Me.Node = Element

        Dim ChildProperty As IProtoXChild
        For Each Child As XElement In Me.Node.Nodes
            If Me.ChildrenLookup.ContainsKey(Child.Name.ToString.ToLower) Then
                ChildProperty = Me.ChildrenLookup.Item(Child.Name.ToString.ToLower)

                ChildProperty.ProcessNode(Child)
            End If
        Next
    End Sub

    Public Overrides Function CreateNew() As IProtoXChild
        Return Nothing
    End Function
End Class
