Imports ProtoXML
Imports System.Runtime.CompilerServices
Imports TheTvDB


Namespace Tvdb


    Public Class Series
        Inherits ProtoPropertyGroup


        'Documented
        Public Property Id                  As New ProtoProperty(Me, "id")
        Public Property TvdbId              As New ProtoProperty(Me, "tvdbid")
        Public Property ActorsNames         As New ProtoProperty(Me, "actors")
        Public Property AirsDayOfWeek       As New ProtoProperty(Me, "Airs_DayOfWeek")
        Public Property AirsTime            As New ProtoProperty(Me, "Airs_Time")
        Public Property ContentRating       As New ProtoProperty(Me, "ContentRating")
        Public Property FirstAired          As New ProtoProperty(Me, "FirstAired")
        Public Property Genre               As New ProtoProperty(Me, "Genre")
        Public Property ImdbId              As New ProtoProperty(Me, "IMDB_ID")
        Public Property TmdbId              As New ProtoProperty(Me, "TMDB_ID")
        Public Property Language            As New ProtoProperty(Me, "Language")
        Public Property Network             As New ProtoProperty(Me, "Network")
        Public Property NetworkId           As New ProtoProperty(Me, "NetworkId")
        Public Property Overview            As New ProtoProperty(Me, "Overview")
        Public Property Rating              As New ProtoProperty(Me, "Rating")
        Public Property RatingCount         As New ProtoProperty(Me, "RatingCount")
        Public Property RunTime             As New ProtoProperty(Me, "Runtime")
        Public Property SeriesID            As New ProtoProperty(Me, "SeriesID")
        Public Property SeriesName          As New ProtoProperty(Me, "SeriesName")
        Public Property Status              As New ProtoProperty(Me, "Status")
        Public Property Added               As New ProtoProperty(Me, "added")
        Public Property AddedBy             As New ProtoProperty(Me, "addedBy")
        Public Property Banner              As New ProtoProperty(Me, "banner")
        Public Property Fanart              As New ProtoProperty(Me, "fanart")
        Public Property FinaleAired         As New ProtoProperty(Me, "finale_aired")
        Public Property LastUpdatedOnTvdb   As New ProtoProperty(Me, "lastupdated")
        Public Property Poster              As New ProtoProperty(Me, "poster")
        Public Property Zap2ItId            As New ProtoProperty(Me, "zap2it_id")
        
        Public Sub New()
            MyBase.New(Nothing, Nothing)

            Me.Node = New XElement("Series")
            For Each item In Me.ChildrenLookup.Values
                item.ParentClass = Me
                item.ResolveAttachment(item.ParentClass)
            Next
            'Throw New NotImplementedException()
        End Sub

        Public Sub New(ByRef Parent As IProtoXBase, ByVal NodeName As String)
            MyBase.New(Parent, NodeName)
        End Sub

        Public Overrides Function CreateNew() As ProtoXML.IProtoXChild
            Return New Series()
        End Function


        Public Property Similarity As Double

        'Public Sub AbsorbTvSeries(ByVal NewSeries As TvdbSeries)
        '    Me.Id.Value                     = Newseries.Identity
        '    Me.TvdbId.Value                 = NewSeries.Identity
        '    Me.ActorsNames.Value            = NewSeries.ActorsDisplayString.Replace(", ","|")
        '    Me.AirsDayOfWeek.Value          = NewSeries.AirsDayOfWeek
        '    Me.AirsTime.Value               = NewSeries.AirsTime
        '    Me.ContentRating.Value          = NewSeries.ContentRating
        '    Me.FirstAired.Value             = NewSeries.FirstAired
        '    Me.Genre.Value                  = NewSeries.GenresDisplayString.Replace(", ","|")
        '    Me.ImdbId.Value                 = NewSeries.ImdbID
        '    Me.TmdbId.Value                 = ""
        '    Me.Language.Value               = ""
        '    Me.Network.Value                = NewSeries.Network
        '    Me.NetworkId.Value              = NewSeries.NetworkId
        '    Me.Overview.Value               = NewSeries.Overview
        '    Me.Rating.Value                 = NewSeries.Rating
        '    Me.RatingCount.Value            = NewSeries.Votes
        '    Me.RunTime.Value                = NewSeries.RunningTimeString
        '    Me.SeriesID.Value               = NewSeries.SeriesId
        '    Me.SeriesName.Value             = NewSeries.SeriesName
        '    Me.Status.Value                 = NewSeries.Status
        '    Me.Added.Value                  = NewSeries.AddedDate
        '    Me.AddedBy.Value                = NewSeries.AddedBy
        '    Me.Banner.Value                 = NewSeries.Banner
        '    Me.Fanart.Value                 = ""
        '    Me.FinaleAired.Value            = ""
        '    Me.LastUpdatedOnTvdb.Value      = NewSeries.LastUpdated
        '    Me.Poster.Value                 = ""
        '    Me.Zap2ItId.Value               = NewSeries.Zap2ItID
        'End Sub
    End Class



    Public Module ExtentionMethods
        Dim arrLetters1
        Dim arrLetters2

        <Extension()>
        Public Function CompareString(String1 As String, String2 As String) As Double
            Dim intLength1
            Dim intLength2
            Dim x
            Dim dblResult


            If UCase(String1) = UCase(String2) Then
                dblResult = 1
            Else
                intLength1 = Len(String1)
                intLength2 = Len(String2)


                If intLength1 = 0 Or intLength2 = 0 Then
                    dblResult = 0
                Else
                    ReDim arrLetters1(intLength1 - 1)
                    ReDim arrLetters2(intLength2 - 1)

                    For x = LBound(arrLetters1) To UBound(arrLetters1)
                        arrLetters1(x) = Asc(UCase(Mid(String1, x + 1, 1)))
                    Next

                    For x = LBound(arrLetters2) To UBound(arrLetters2)
                        arrLetters2(x) = Asc(UCase(Mid(String2, x + 1, 1)))
                    Next

                    dblResult = SubSim(1, intLength1, 1, intLength2) / (intLength1 + intLength2) * 2
                End If
            End If

            CompareString = dblResult
        End Function


        Private Function SubSim(intStart1, intEnd1, intStart2, intEnd2) As Double
            Dim intMax As Integer = Integer.MinValue

            Try
                Dim y
                Dim z
                Dim ns1 As Integer
                Dim ns2 As Integer
                Dim i

                If (intStart1 > intEnd1) Or (intStart2 > intEnd2) Or (intStart1 <= 0) Or (intStart2 <= 0) Then
                    Return 0
                End If

                For y = intStart1 To intEnd1
                    For z = intStart1 To intEnd2
                        i = 0

                        Do Until arrLetters1(y - 1 + i) <> arrLetters2(z - 1 + i)
                            i = i + 1

                            If i > intMax Then
                                ns1 = y
                                ns2 = z
                                intMax = i
                            End If

                            If ((y + i) > intEnd1) Or ((z + i) > intEnd2) Then
                                Exit Do
                            End If
                        Loop
                    Next
                Next

                intMax = intMax + SubSim(ns1 + intMax, intEnd1, ns2 + intMax, intEnd2)
                intMax = intMax + SubSim(intStart1, ns1 - 1, intStart2, ns2 - 1)
            Catch ex As OverflowException
                Return Nothing
            Catch ex As StackOverflowException
                Return Nothing
            End Try

            Return intMax
        End Function

        <Extension()>
        Public Function FindBestPossibleShow(ByVal ThisList As List(Of Tvdb.Series), ByVal FolderName As String, ByVal PreferedLang As String) As Tvdb.Series

            FolderName = FolderName.Replace(".", " ") ' we remove periods to find the title, we should also do it here to compare

            For Each Item In ThisList
                'Item.Similarity = Item.SeriesName.Value.CompareString(FolderName)
                Item.Similarity = Tvdb.CompareString(Item.SeriesName.Value, FolderName)
            Next

            Dim Search = From Ser As Tvdb.Series In ThisList Order By Ser.Similarity Descending ', Ser.FirstAired Descending

            If Search.Count > 0 Then
                For Each Item As Series In Search
                    If Item.Language.Value = PreferedLang Then
                        Return Item
                    End If
                Next

                Dim Test As Tvdb.Series = Search.FirstOrDefault()
                Return Test
            End If

            'Catch All
            Return ThisList.Item(0)
        End Function
    End Module
End Namespace