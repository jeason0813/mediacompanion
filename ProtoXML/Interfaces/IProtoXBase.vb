Public Interface IProtoXBase
    Property NodeName As String

    Property Node As XElement
    Property CacheNode As XElement

    Property ChildrenLookup As Dictionary(Of String, IProtoXChild)
    Sub AddChildForLoad(ByRef NewChild As IProtoXChild)

    Event ValueChanged(ByRef ProtoChild As ProtoXChildBase)
    Sub RaiseValueChanged(ByRef ProtoChild As ProtoXChildBase)
    Sub HandleChildValueChanged(ByRef ProtoChild As ProtoXChildBase)

    Property IsAltered As Boolean
    Property SurpressAlter As Boolean

    Property FolderPath As String

End Interface
