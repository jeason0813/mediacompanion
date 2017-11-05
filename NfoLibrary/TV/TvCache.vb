Imports ProtoXML


Public Class TvCache
    Implements IList(Of ProtoFile)

    Public Items As New List(Of ProtoFile)
    Public Shows As New NotifyingList(Of TvShow)("Shows")
    Public Seasons As New NotifyingList(Of Media_Companion.TvSeason)("Seasons")
    Public Episodes As New NotifyingList(Of TvEpisode)("Episodes")
    Public Other As New List(Of ProtoFile)

    Public ReadOnly Property IsAltered As Boolean
        Get
            For Each Item As ProtoFile In Items
                If Item.IsAltered Then
                    Return True
                End If
            Next

            Return False
        End Get
    End Property



    Public Sub Add(ByVal item As ProtoFile) Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).Add
        If TypeOf item Is TvShow Then
            Shows.Add(item)
        ElseIf TypeOf item Is Media_Companion.TvSeason Then
            Seasons.Add(item)
        ElseIf TypeOf item Is TvEpisode Then
            Episodes.Add(item)
        Else
            Other.Add(item)
        End If
        Items.Add(item)
    End Sub

    Public Sub Clear() Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).Clear
        Shows.Clear()
        Seasons.Clear()
        Episodes.Clear()
        Other.Clear()
        Items.Clear()
    End Sub

    Public Function Contains(ByVal item As ProtoXML.ProtoFile) As Boolean Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).Contains
        If Items.Contains(item) Then
            Return True
        Else
            Return False
        End If
        'If TypeOf item Is TvShow Then
        '    Return Shows.Contains(item)
        'ElseIf TypeOf item Is TvSeason Then
        '    Return Seasons.Contains(item)
        'ElseIf TypeOf item Is TvEpisode Then
        '    Return Episodes.Contains(item)
        'Else
        '    Return Other.Contains(item)
        'End If
    End Function

    Private Sub CopyTo(ByVal array() As ProtoXML.ProtoFile, ByVal arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).CopyTo
        Throw New NotImplementedException
    End Sub

    Public ReadOnly Property Count As Integer Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).Count
        Get
            Return Items.Count
        End Get
    End Property

    Public ReadOnly Property CountShows
        Get
            Return Shows.Count
        End Get
    End Property

    Public ReadOnly Property CountSeasons
        Get
            Return Seasons.Count
        End Get
    End Property

    Public ReadOnly Property CountEpisodes
        Get
            Return Episodes.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function Remove(ByVal item As ProtoXML.ProtoFile) As Boolean Implements System.Collections.Generic.ICollection(Of ProtoXML.ProtoFile).Remove
        If Items.Contains(item) Then
            Items.Remove(item)
        End If

        If TypeOf item Is TvShow Then
            If Shows.Contains(item) Then
                Shows.Remove(item)
                Return True
            End If
        ElseIf TypeOf item Is Media_Companion.TvSeason Then
            If Seasons.Contains(item) Then
                Seasons.Remove(item)
                Return True
            End If
        ElseIf TypeOf item Is TvEpisode Then
            If Episodes.Contains(item) Then
                Episodes.Remove(item)
                Return True
            End If
        Else
            If Other.Contains(item) Then
                Other.Remove(item)
                Return True
            End If
        End If

        Return False
    End Function

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of ProtoXML.ProtoFile) Implements System.Collections.Generic.IEnumerable(Of ProtoXML.ProtoFile).GetEnumerator
        Return Items.GetEnumerator
    End Function

    Public Function IndexOf(ByVal item As ProtoXML.ProtoFile) As Integer Implements System.Collections.Generic.IList(Of ProtoXML.ProtoFile).IndexOf
        Return Items.IndexOf(item)
    End Function

    Private Sub Insert(ByVal index As Integer, ByVal item As ProtoXML.ProtoFile) Implements System.Collections.Generic.IList(Of ProtoXML.ProtoFile).Insert
        Throw New NotImplementedException
    End Sub

    Default Public Property Item(ByVal index As Integer) As ProtoXML.ProtoFile Implements System.Collections.Generic.IList(Of ProtoXML.ProtoFile).Item
        Get
            Return Items(index)
        End Get
        Set(ByVal value As ProtoXML.ProtoFile)
            Items(index) = value
        End Set
    End Property

    Public Sub RemoveAt(ByVal index As Integer) Implements System.Collections.Generic.IList(Of ProtoXML.ProtoFile).RemoveAt
        Dim Temp As ProtoFile
        Temp = Items(index)

        If Shows.Contains(Temp) Then
            Shows.Remove(Temp)
        ElseIf Seasons.Contains(Temp) Then
            Seasons.Remove(Temp)
        ElseIf Episodes.Contains(Temp) Then
            Episodes.Remove(Temp)
        ElseIf Other.Contains(Temp) Then
            Other.Remove(Temp)
        End If

        Items.RemoveAt(index)
    End Sub

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Items.GetEnumerator
    End Function

    Public Property TvCachePath As String

    Private Dom As XDocument
    Public Sub Save()
        If String.IsNullOrEmpty(TvCachePath) Then
            Throw New Exception("Tv cache path not set")
        End If

        Dom = New XDocument(<tvcache ver="3.5"></tvcache>)

        For Each Item As ProtoFile In Items
            Dom.Root.Add(Item.CacheNode)
        Next

        Dom.Save(TvCachePath)
    End Sub

    Private ShowsByID As New Dictionary(Of String, TvShow)
    Private ShowsByPath As New Dictionary(Of String, TvShow)
    
    Public Sub Load()
        Clear()        'empty the old cache before repopulation

        If String.IsNullOrEmpty(TvCachePath) Then
            Throw New Exception("Tv cache path not set")
        End If

        Dom = XDocument.Load(TvCachePath)

        If Dom Is Nothing OrElse Dom.Root Is Nothing Then
            Exit Sub
        End If

        For Each Node As XElement In Dom.Root.Nodes
            Select Case Node.Name
                Case "tvshow"
                    Dim NewShow As New TvShow
                    NewShow.LoadXml(Node)
                    NewShow.NfoFilePath = Node.Attribute("NfoPath")
                    DirectCast(NewShow.CacheDoc.FirstNode, XElement).FirstAttribute.Value = NewShow.NfoFilePath

                    Shows.Add(NewShow)
                    Items.Add(NewShow)
                    If NewShow.TvdbId.Value IsNot Nothing AndAlso Not ShowsByID.ContainsKey(NewShow.TvdbId.Value) Then ShowsByID.Add(NewShow.TvdbId.Value, NewShow)
                    If NewShow.FolderPath IsNot Nothing AndAlso Not ShowsByPath.ContainsKey(NewShow.FolderPath) Then ShowsByPath.Add(NewShow.FolderPath, NewShow)
                   

                Case "season"
                    Dim NewShow As New Media_Companion.TvSeason
                    NewShow.LoadXml(Node)
                    Seasons.Add(NewShow)
                    Items.Add(NewShow)
                Case "episodedetails"
                    Dim NewShow As New TvEpisode
                    NewShow.LoadXml(Node)
                    'NewShow.MediaExtension = Node.Attribute("MediaExtension") 'is not being used in TV cache as yet - HueyHQ 26Feb2013
                    NewShow.NfoFilePath = Node.Attribute("NfoPath")
                    Episodes.Add(NewShow)
                    Items.Add(NewShow)

                Case Else
                    Dim NewShow As New ProtoFile(Node.Name.ToString)
                    NewShow.LoadXml(Node)
                    NewShow.NfoFilePath = Node.Attribute("NfoPath")
                    Other.Add(NewShow)
                    Items.Add(NewShow)
            End Select

        Next

        AttachEpisodes()
        UpdateTreeNodes()
    End Sub

    Private Sub AttachEpisodes()
        For Each Show As TvShow In Shows
            Dim showID = Show.TvdbId.Value
            Dim EpisodeList = Episodes.Where(Function(ele As TvEpisode) ele.ShowId.Value = showID)
            For Each episode In EpisodeList
                Show.AddEpisode(episode)
            Next
        Next
    End Sub

    Private Sub UpdateTreeNodes()
        For Each Show As TvShow In Shows
            For Each Season As TvSeason In Show.Seasons.Values
                For Each Episode As TvEpisode In Season.Episodes
                    Episode.UpdateTreenode()
                Next
                Season.UpdateTreenode()
            Next
            Show.UpdateTreenode()
        Next
    End Sub
    
End Class
