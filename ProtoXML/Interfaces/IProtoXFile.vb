Public Interface IProtoXFile
    Inherits IProtoXBase

    Property Doc As XDocument
    Property CacheDoc As XDocument

    Property NfoFilePath As String

    ReadOnly Property FileExists As Boolean
    ReadOnly Property FileContainsReadableXml As Boolean

    Sub Save()
    Sub Save(ByVal Path As String)

    Sub Load()
    Sub Load(ByVal Path As String)

End Interface
