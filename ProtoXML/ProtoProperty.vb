Imports System.Xml

Public Class ProtoProperty
    Inherits ProtoXChildBase



    Public Property NotAttached As Boolean
    Public Property SurpressAlters As Boolean
    Public Property CacheMode As CacheMode



    Private _DefaultValue As String
    Public ReadOnly Property DefaultValue As String
        Get
            Return _DefaultValue
        End Get
    End Property

    Private _value As String

    Public Property Value As String
        Get
            If _value Is Nothing Then
                Return Nothing
            End If

            Return _value
        End Get
        Set(ByVal value As String)
            If value Is Nothing Then
                If Me.Node IsNot Nothing AndAlso Me.Node.Parent IsNot Nothing Then
                    Me.Node.Remove()
                    Me.Node = Nothing
                End If
                If Me.CacheNode IsNot Nothing AndAlso Me.CacheNode.Parent IsNot Nothing Then
                    Me.CacheNode.Remove()
                    Me.CacheNode = Nothing
                End If
                Exit Property
            End If

            If Me.CacheMode = ProtoXML.CacheMode.OnlyFile Or Me.CacheMode = ProtoXML.CacheMode.Both Then
                If Me.Node Is Nothing Then
                    Me.Node = <hold><%= value %></hold>
                    Me.Node.Name = Me.NodeName

                    If ParentClass IsNot Nothing AndAlso ParentClass.Node IsNot Nothing Then
                        If Node.Parent IsNot Nothing Then
                            Node.Remove()
                        End If
                        ParentClass.Node.Add(Me.Node)
                    Else
                        NotAttached = True
                    End If
                Else
                    Me.Node.Value = CType(value, String)
                End If
            End If

            If Me.CacheMode = ProtoXML.CacheMode.OnlyCache Or Me.CacheMode = ProtoXML.CacheMode.Both Then
                If Me.CacheNode Is Nothing Then
                    Me.CacheNode = <hold><%= value %></hold>
                    Me.CacheNode.Name = Me.NodeName

                    If ParentClass IsNot Nothing AndAlso ParentClass.Node IsNot Nothing Then
                        If CacheNode.Parent IsNot Nothing Then
                            CacheNode.Remove()
                        End If
                        ParentClass.CacheNode.Add(Me.CacheNode)
                    Else
                        NotAttached = True
                    End If
                Else
                    Me.CacheNode.Value = CType(value, String)
                End If
            End If

            If Not SurpressAlters AndAlso Not _value = value Then
                MyBase.RaiseValueChanged(Me)
            End If
            _value = value
        End Set
    End Property

    Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String, Optional ByVal DefaultValue As String = Nothing, Optional ByVal SortOrder As Integer = -1, Optional ByVal ExtraNodes As String() = Nothing, Optional ByVal CacheMode As CacheMode = ProtoXML.CacheMode.OnlyFile)
        MyBase.New(Parent, NodeName)

        Me.CacheMode = CacheMode

        If DefaultValue IsNot Nothing Then
            _DefaultValue = DefaultValue
            Me.Value = DefaultValue
        End If



        Me.SortIndex = SortOrder
    End Sub


    Public Overrides Sub ProcessNode(ByRef Element As System.Xml.Linq.XElement)
        If Element.Name.ToString.ToLower <> Me.NodeName Then
            Throw New InvalidExpressionException("Wrong element passed")
        End If

        Me.Node = Element
        Me.Value = Element.Value
    End Sub

    'Public Overrides Sub ResolveAttachment(ByRef ParentClass As IProtoXBase)
    '    If ParentClass Is Nothing Then
    '        Throw New Exception("Parent Class not set")
    '    End If

    '    If ParentClass.Node Is Nothing Then
    '        'Throw New Exception("Parent Class no node")
    '    Else
    '        If Not ParentClass.Node.Nodes.Contains(Node) Then
    '            If Me.Node Is Nothing Then
    '                Me.Node = New XElement(Me.NodeName)
    '                If _value IsNot Nothing Then
    '                    Me.Node.Value = _value
    '                End If
    '            End If
    '            If Node.Parent IsNot Nothing Then
    '                Node.Remove()
    '            End If
    '            ParentClass.Node.Add(Node)
    '        End If
    '    End If
    'End Sub

    'Public Shared Narrowing Operator CType(ByVal Left As ProtoProperty) As String
    '    Return Left.Value
    'End Operator

    Public Function IndexOf(ByVal Input As String) As Integer
        If Me.Value Is Nothing Then
            Return -1
        End If

        Return Me.Value.IndexOf(Input)
    End Function

    Public Overrides Function CreateNew() As IProtoXChild
        Return New ProtoProperty(Me.ParentClass, Me.NodeName)
    End Function


    Public Function GetChild(doc As XmlDocument) As XmlElement
        Dim child = doc.CreateElement(NodeName.tolower)

        child.InnerText = Value

        Return child
    End Function

End Class



Public Enum CacheMode
    OnlyCache
    OnlyFile
    Both

End Enum