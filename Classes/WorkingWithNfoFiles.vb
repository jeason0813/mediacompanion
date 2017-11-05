Imports System.Xml
'Imports System.IO
'Imports Alphaleonis.Win32.Filesystem
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports Media_Companion
Imports System.Text
Imports System.Linq

Public Class WorkingWithNfoFiles
    Const SetDefaults = True
    'Public Shared MyCulture As New System.Globalization.CultureInfo("en-US")

    Public Shared Function util_NfoValidate(ByVal nfopath As String, Optional ByVal homemovie As Boolean = False)
        Dim tempstring As String
        Using filechck As IO.StreamReader = File.OpenText(nfopath)
            tempstring = filechck.ReadToEnd.ToLower
        End Using
        If tempstring = Nothing Then
            Return False
        End If
        If tempstring.IndexOf("<movie") <> -1 And tempstring.IndexOf("</movie>") <> -1 And tempstring.IndexOf("<title>") <> -1 And tempstring.IndexOf("</title>") <> -1 Then
            Return True
            Exit Function
        End If
        Return False
    End Function

    ''' <summary>
    ''' Convert to UTF8 if not already in UTF8 format
    ''' </summary>
    ''' <param name="FileName">Full path and filename to xml document to convert as String</param>
    Public Shared Sub ConvertFileToUTF8IfNotAlready(FileName As String)
        If Not File.Exists(FileName) Then Exit Sub
        Dim _Detected As Encoding
        Dim s As String = ""
        Using r As IO.StreamReader = File.Opentext(FileName)
            s = r.ReadToEnd
            _Detected = r.CurrentEncoding
        End Using

        If _Detected.ToString <> "System.Text.UTF8Encoding" AndAlso _Detected.ToString <> "System.Text.SBCSCodePageEncoding" Then
            Try
                File.WriteAllText(FileName, s, Encoding.UTF8)
            Catch ex As Exception
                MsgBox("Error [" & ex.Message & "] occurred trying to convert file [" & FileName & "] from format [" & _Detected.ToString & "] to UTF-8. Recommended Action: Either delete the NFO and have MC re-create it or open it in NotePad and 'Save As... UFT-8" )
            End Try
        End If
    End Sub
    
    Public Function util_CharsConvert(ByVal line As String)
        Monitor.Enter(Me)
        Try
            line = line.Replace("&amp;", "&")
            line = line.Replace("&lt;", "<")
            line = line.Replace("&gt;", ">")
            line = line.Replace("&quot;", "Chr(34)")
            line = line.Replace("&apos;", "'")
            line = line.Replace("&#xA;", vbCrLf)
            line = line.Replace("â€˜", "'")
            Return line
        Catch
        Finally
            Monitor.Exit(Me)
        End Try
        Return "Error"
    End Function

    ''' <summary>
    ''' Save xml document common routine
    ''' </summary>
    ''' <param name="doc">XMLDocument data</param>
    ''' <param name="Filename">Full path, filname and extension to save to</param>
    ''' <returns>True if successful</returns>
    Public Shared Function SaveXMLDoc(ByVal doc As XmlDocument, ByVal Filename As String) As Boolean
        Dim aok As Boolean = False
        Try
            ' Get around Long Path for XMLTextWriter by saving to cache first
            ' then moving to final destination.
            Dim tmpxml As String = Utilities.CacheFolderPath & "tmpxml.nfo"
            If File.Exists(tmpxml) Then
                File.Delete(tmpxml, True)
            End If
            Using output As XmlTextWriter = New XmlTextWriter(tmpxml, System.Text.Encoding.UTF8)
                output.Formatting = Formatting.Indented
                output.Indentation = 4
                doc.WriteTo(output)
            End Using
            aok = True
            If aok Then
                If File.Exists(Filename) Then File.Delete(Filename)
                File.Move(tmpxml, Filename)
            End If
        Catch
        End Try
        Return aok
    End Function

    Public Shared Function AppendVideo(ByRef doc As XmlDocument, ByVal Video As VideoDetails) As XmlElement
        Dim VideoElements As XmlElement = doc.CreateElement("video")
        If Not String.IsNullOrEmpty(Video.Width.Value)             Then VideoElements.AppendChild(doc, "width"               , Video.width.Value)
        If Not String.IsNullOrEmpty(Video.Height.Value)            Then VideoElements.AppendChild(doc, "height"              , Video.height.Value)
        If Not String.IsNullOrEmpty(Video.Aspect.Value)            Then VideoElements.AppendChild(doc, "aspect"              , Video.aspect.Value)
        If Not String.IsNullOrEmpty(Video.Codec.Value)             Then VideoElements.AppendChild(doc, "codec"               , Video.codec.Value)
        If Not String.IsNullOrEmpty(Video.FormatInfo.Value)        Then VideoElements.AppendChild(doc, "format"              , Video.formatinfo.Value)
        If Not String.IsNullOrEmpty(Video.DurationInSeconds.Value) Then VideoElements.AppendChild(doc, "durationinseconds"   , Video.DurationInSeconds.Value)
        If Not String.IsNullOrEmpty(Video.Bitrate.Value)           Then VideoElements.AppendChild(doc, "bitrate"             , Video.bitrate.Value)
        If Not String.IsNullOrEmpty(Video.BitrateMode.Value)       Then VideoElements.AppendChild(doc, "bitratemode"         , Video.bitratemode.Value)
        If Not String.IsNullOrEmpty(Video.BitrateMax.Value)        Then VideoElements.AppendChild(doc, "bitratemax"          , Video.bitratemax.Value)
        If Not String.IsNullOrEmpty(Video.Container.Value)         Then VideoElements.AppendChild(doc, "container"           , Video.container.Value)
        If Not String.IsNullOrEmpty(Video.CodecId.Value)           Then VideoElements.AppendChild(doc, "codecid"             , Video.codecid.Value)
        If Not String.IsNullOrEmpty(Video.CodecInfo.Value)         Then VideoElements.AppendChild(doc, "codecidinfo"         , Video.codecinfo.Value)
        If Not String.IsNullOrEmpty(Video.ScanType.Value)          Then VideoElements.AppendChild(doc, "scantype"            , Video.scantype.Value)
        If Not Video.NumVideoBits = -1                             Then VideoElements.AppendChild(doc, "NumVideoBits"        , Video.NumVideoBits)
        Return VideoElements
    End Function

    Public Shared Function AppendAudio(ByRef doc As XmlDocument, ByVal Audio As AudioDetails) As XmlElement
        Dim AudioElement As XmlElement = doc.CreateElement("audio")
        If Not String.IsNullOrEmpty(Audio.Language.Value)     Then AudioElement.AppendChild(doc, "language"       , Audio.Language.Value)
        If Not String.IsNullOrEmpty(Audio.DefaultTrack.Value) Then AudioElement.AppendChild(doc, "DefaultTrack"   , Audio.DefaultTrack.Value)
        If Not String.IsNullOrEmpty(Audio.Codec.Value)        Then AudioElement.AppendChild(doc, "codec"          , Audio.Codec.Value)
        If Not String.IsNullOrEmpty(Audio.Channels.Value)     Then AudioElement.AppendChild(doc, "channels"       , Audio.Channels.Value)
        If Not String.IsNullOrEmpty(Audio.Bitrate.Value)      Then AudioElement.AppendChild(doc, "bitrate"        , Audio.Bitrate.Value)
        Return AudioElement
    End Function

    Public Shared Function AppendSub(ByRef doc As XmlDocument, ByVal SubTitle As SubtitleDetails) As XmlElement
        Dim SubElement As XmlElement = doc.CreateElement("subtitle")
        SubElement.AppendChild(doc, "language"      , SubTitle.language.Value)
        SubElement.AppendChild(doc, "default"       , SubTitle.Primary)
        SubElement.AppendChild(doc, "forced"        , SubTitle.Forced)
        Return SubElement
    End Function

    Public Shared Function Streamdetailsload(ByVal thisresult As XmlNode) As StreamDetails
        Dim Streamdetails As New StreamDetails
        Try
            Dim detail As XmlNode = Nothing
            For Each detail In thisresult.ChildNodes
                Select Case detail.Name
                    Case "video"
                        Dim videodetails As XmlNode = Nothing
                        For Each videodetails In detail.ChildNodes
                            Select Case videodetails.Name
                                Case "width"
                                    StreamDetails.Video.Width.Value = videodetails.InnerText
                                Case "height"
                                    StreamDetails.Video.Height.Value = videodetails.InnerText
                                Case "aspect"
                                    StreamDetails.Video.Aspect.Value = Utilities.FixIntlAspectRatio(videodetails.InnerText)
                                Case "codec"
                                    StreamDetails.Video.Codec.Value = videodetails.InnerText
                                Case "format"
                                    If StreamDetails.Video.FormatInfo.Value = "" Then
                                        StreamDetails.Video.FormatInfo.Value = videodetails.InnerText
                                    End If
                                Case "formatinfo"
                                    StreamDetails.Video.FormatInfo.Value = videodetails.InnerText
                                Case "durationinseconds"
                                    StreamDetails.Video.DurationInSeconds.Value = videodetails.InnerText
                                Case "bitrate"
                                    StreamDetails.Video.Bitrate.Value = videodetails.InnerText
                                Case "bitratemode"
                                    StreamDetails.Video.BitrateMode.Value = videodetails.InnerText
                                Case "bitratemax"
                                    StreamDetails.Video.BitrateMax.Value = videodetails.InnerText
                                Case "container"
                                    StreamDetails.Video.Container.Value = videodetails.InnerText
                                Case "codecid"
                                    StreamDetails.Video.CodecId.Value = videodetails.InnerText
                                Case "codecidinfo"
                                    StreamDetails.Video.CodecInfo.Value = videodetails.InnerText
                                Case "scantype"
                                    StreamDetails.Video.ScanType.Value = videodetails.InnerText
                                Case "NumVideoBits"
                                    StreamDetails.Video.NumVideoBits = videodetails.InnerText
                            End Select
                        Next
                    Case "audio"
                        Dim audiodetails As XmlNode = Nothing
                        Dim audio As New AudioDetails
                        For Each audiodetails In detail.ChildNodes
                            Select Case audiodetails.Name
                                Case "language"
                                    audio.Language.Value = audiodetails.InnerText
                                Case "DefaultTrack"
                                    audio.DefaultTrack.Value = audiodetails.InnerText
                                Case "codec"
                                    audio.Codec.Value = audiodetails.InnerText
                                Case "channels"
                                    audio.Channels.Value = audiodetails.InnerText
                                Case "bitrate"
                                    audio.Bitrate.Value = audiodetails.InnerText
                            End Select
                        Next
                        StreamDetails.Audio.Add(audio)
                    Case "subtitle"
                        Dim subsdetails As XmlNode = Nothing
                        Dim sublang As New SubtitleDetails
                        For Each subsdetails In detail.ChildNodes
                            Select Case subsdetails.Name
                                Case "language"
                                    sublang.Language.Value  = subsdetails.InnerText
                                Case "default"
                                    sublang.Primary         = subsdetails.InnerXml 
                                Case "forced"
                                    sublang.Forced          = subsdetails.InnerXml
                            End Select
                        Next
                        StreamDetails.Subtitles.Add(sublang)
                End Select
            Next
        Catch
        End Try
        Return Streamdetails
    End Function

    Public Shared Function SetCreateDate(ByVal videodate As String) As String
        If String.IsNullOrEmpty(videodate) Then
            Try
                Dim myDate2 As Date = System.DateTime.Now
                Return Format(myDate2, Pref.datePattern).ToString
            Catch
            End Try
        End If
        Return videodate
    End Function

    Public Shared Function FormatRunTime(ByVal runtime As String, ByVal Duration As String, Optional ByVal ismovie As Boolean = False) As String
        Dim output As String = ""
        If ismovie AndAlso Pref.MovDurationAsRuntine Then
            Dim durationInt As Integer = If(String.IsNullOrEmpty(duration), 0, duration.ToInt)
            If durationInt > 60 Then
                runtime = Math.Floor(durationInt/60).ToString
            End If
        End If
        
        If Not String.IsNullOrEmpty(runtime) Then
            Dim minutes As String = runtime.ToMin
            Try
                If Not String.IsNullOrEmpty(minutes) AndAlso Convert.ToInt32(minutes) > 0 Then
                    Do While minutes.IndexOf("0") = 0 And minutes.Length > 0
                        minutes = minutes.Substring(1, minutes.Length - 1)
                    Loop
                    If Pref.RuntimePadding Then
                        If Convert.ToInt32(minutes) < 10    Then minutes = "0" & minutes
                        If Convert.ToInt32(minutes) < 100   Then minutes = "0" & minutes
                    End If
                End If
                If Pref.intruntime = False And IsNumeric(minutes) Then
                    If minutes.ToInt = 0 AndAlso Not String.IsNullOrEmpty(Duration) Then
                        Dim seconds As Integer = Convert.ToInt32(Duration)
                        If seconds > 0 AndAlso seconds < 60 Then minutes = "1"
                    End If
                    minutes = minutes & " min"
                End If
            Catch ex As Exception
                minutes = runtime
            End Try
            output = minutes
        Else
            output = runtime
        End If
        Return output
    End Function

    Public Shared Function CommaNoComma(ByVal t As String, ByVal choice As Boolean) As String
        If Not String.IsNullOrEmpty(t) then
            If Not choice Then
                t = t.Replace(",", "")
            Else
                If Not t.Contains(",") Then
                    If t.Length > 3 Then
                        t = t.Insert(t.Length-3, ",")
                    End If
                    If t.Length > 7 Then
                        t = t.Insert(t.Length-7, ",")
                    End If
                End If
            End If
        End If
        Return t
    End Function

    '  All Tv Load/Save Routines
#Region " Tv Routines "
  
#Region "Episode routines"  
    ''' <summary>
    ''' Load Episode nfo as xml document
    ''' </summary>
    ''' <param name="loadpath">Full path and filename to episode nfo</param>
    ''' <returns>List of TvEpisodes</returns>
    Public Shared Function ep_NfoLoad(ByVal loadpath As String)
        Dim episodelist As New List(Of TvEpisode)
        Dim fixmulti As Boolean = False
        Dim newtvshow As New TvEpisode
        If Not File.Exists(loadpath) Then
            Return episodelist '"Error"
        Else
            Dim tvshow As New XmlDocument
            Try
                Using tmpstrm As IO.StreamReader = File.OpenText(loadpath)
                    tvshow.Load(tmpstrm)
                End Using
            Catch ex As Exception
                Try
                    newtvshow.Title.Value = Path.GetFileName(loadpath)
                Catch
                    newtvshow.Title.Value = loadpath
                End Try
                newtvshow.ImdbId.Value = "xml error"
                newtvshow.TvdbId.Value = ""
                epnfofinal(newtvshow, loadpath)
                episodelist.Add(newtvshow)
                Return episodelist
                Exit Function
            End Try
            
            Dim tempid As String = ""
            If tvshow.DocumentElement.Name = "episodedetails" Then
                Dim newtvepisode As New TvEpisode
                For Each thisresult As XmlNode In tvshow("episodedetails")
                    Try
                        epTagApply(newtvepisode, thisresult)    'Common Routine to apply values to episode fields
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                Next
                epnfofinal(newtvepisode, loadpath)      'Common Routine to tidy-up episode season and episode numbers
                episodelist.Add(newtvepisode)
            ElseIf tvshow.DocumentElement.Name = "multiepisodenfo" Or tvshow.DocumentElement.Name = "xbmcmultiepisode" Then
                Dim temp As String = tvshow.DocumentElement.Name
                For Each thisresult As XmlNode In tvshow(temp)
                    Dim sp As String = thisresult.InnerXml.ToString
                    If sp.Contains("<streamdetails><fileinfo>") Then
                        fixmulti = True
                        Try
                            sp = sp.Replace("<streamdetails><fileinfo>", "<fileinfo><streamdetails>")
                            sp = sp.Replace("</fileinfo></streamdetails>", "</streamdetails></fileinfo>")
                            thisresult.InnerXml = sp
                        Catch
                        End Try
                    End If
                    Select Case thisresult.Name
                        Case "episodedetails"
                            Dim newepisodenfo As XmlNode = Nothing
                            Dim anotherepisode As New TvEpisode
                            Dim filedetails As String = ""
                            Dim audio As String = ""

                            Dim tempint As Integer = thisresult.ChildNodes.Count - 1
                            For f = 0 To tempint
                                Try
                                    epTagApply(anotherepisode, thisresult.ChildNodes(f))    'Common Routine to apply values to episode fields
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try
                            Next f
                            Try
                                epnfofinal(anotherepisode, loadpath)    'Common Routine to tidy-up episode season and episode numbers
                                episodelist.Add(anotherepisode)
                            Catch ex As Exception
                                MsgBox(ex.ToString)
                            End Try
                    End Select
                Next
                If fixmulti Then
                    ep_NfoSave(episodelist, loadpath)
                End If
            End If

            Return episodelist
        End If
    End Function
    
    ''' <summary>
    ''' Save episode(s) xml data as nfo
    ''' </summary>
    ''' <param name="listofepisodes">List of episodes object</param>
    ''' <param name="path">Full path and filename of nfo to save</param>
    Public Shared Sub ep_NfoSave(ByVal listofepisodes As List(Of TvEpisode), ByVal path As String)
        Dim doc As New XmlDocument
        Dim root As XmlElement
        Dim xmlEpisode As XmlElement
        Dim xmlStreamDetails As XmlElement
        Dim xmlFileInfo As XmlElement
        Dim xmlActor As XmlElement

        Dim xmlproc As XmlDeclaration
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)

        root = doc.CreateElement("multiepisodenfo")    'root starts out as multiepisode, but tested for single episode after initial population.
        For Each ep In listofepisodes

            xmlEpisode = doc.CreateElement("episodedetails")
            If Pref.enabletvhdtags = True Then
                xmlFileInfo = doc.CreateElement("fileinfo")
                xmlStreamDetails = doc.CreateElement("streamdetails")
                xmlStreamDetails.AppendChild(AppendVideo(doc, ep.streamdetails.Video))

                If ep.StreamDetails.Audio.Count > 0 Then
                    For Each aud In ep.StreamDetails.Audio
                        xmlStreamDetails.AppendChild(AppendAudio(doc, aud))
                    Next
                End If

                If ep.StreamDetails.Subtitles.Count > 0 Then
                    For Each subt In ep.StreamDetails.Subtitles
                        If Not String.IsNullOrEmpty(subt.Language.Value) Then
                            xmlStreamDetails.AppendChild(AppendSub(doc, subt))
                        End If
                        
                    Next
                End If
                xmlFileInfo.AppendChild(xmlStreamDetails)
                xmlEpisode.AppendChild(xmlFileInfo)
            End If

            xmlEpisode.AppendChild(doc, "title"         , ep.Title.value        )
            xmlEpisode.AppendChild(doc, "season"        , ep.Season.Value       )
            xmlEpisode.AppendChild(doc, "episode"       , ep.Episode.Value      )
            xmlEpisode.AppendChild(doc, "aired"         , ep.Aired.Value        )
            xmlEpisode.AppendChild(doc, "plot"          , ep.plot.Value         )
            xmlEpisode.AppendChild(doc, "playcount"     , ep.playcount.Value    )
            xmlEpisode.AppendChildList(doc, "director"  , ep.director.Value, ",")
            xmlEpisode.AppendChildList(doc, "credits"   , ep.credits.Value,  ",")
            xmlEpisode.AppendChild(doc, "rating"        , ep.rating.Value       )
            xmlEpisode.AppendChild(doc, "votes"         , CommaNoComma(ep.votes.Value, Pref.TvThousSeparator))
            If Not String.IsNullOrEmpty(ep.ImdbId.Value) Then
                xmlEpisode.AppendChild(doc, "uniqueid"  , ep.ImdbId.Value       , "imdb")
            End If
            If Not String.IsNullOrEmpty(ep.TmdbId.Value) Then
                xmlEpisode.AppendChild(doc, "uniqueid"  , ep.TmdbId.Value       , "tmdb")
            End If
            xmlEpisode.AppendChild(doc, "uniqueid"      , ep.uniqueid.Value     , "tvdb", True)
            xmlEpisode.AppendChild(doc, "runtime"       , ep.runtime.Value      )
            xmlEpisode.AppendChild(doc, "showid"        , ep.showid.Value       )
            xmlEpisode.AppendChild(doc, "imdbid"        , ep.imdbid.Value       )
            xmlEpisode.AppendChild(doc, "tmdbid"        , ep.TmdbId.Value       )
            xmlEpisode.AppendChild(doc, "displayseason" , ep.DisplaySeason.Value)
            xmlEpisode.AppendChild(doc, "displayepisode", ep.DisplayEpisode.Value)
            xmlEpisode.AppendChild(doc, "runtime"       , ep.Runtime.Value      )
            xmlEpisode.AppendChild(doc, "epbookmark"    , ep.EpBookmark.Value   )
            xmlEpisode.AppendChild(doc, "videosource"   , ep.Source.Value       )
            xmlEpisode.AppendChild(doc, "userrating"    , ep.UserRating.Value   )
            xmlEpisode.AppendChild(doc, "dvdepnumber"   , ep.DvdEpNumber.Value  )
            
            Dim actorstosave As Integer = ep.ListActors.Count
            If actorstosave > Pref.maxactors Then actorstosave = Pref.maxactors
            For f = 0 To actorstosave - 1
                xmlActor = doc.CreateElement("actor")
                xmlActor.AppendChild(doc, "name", ep.ListActors(f).actorname)
                xmlActor.AppendChild(doc, "role", ep.ListActors(f).actorrole)
                If Not String.IsNullOrEmpty(ep.ListActors(f).actorthumb) Then
                    xmlActor.AppendChild(doc, "thumb", ep.ListActors(f).actorthumb)
                End If
                xmlEpisode.AppendChild(xmlActor)
            Next
            If listofepisodes.Count = 1 Then    'file is a single episode
                root = xmlEpisode               'root now equals 'episodedetails'
                Exit For                        'now append to XML doc as a single episode
            End If
            root.AppendChild(xmlEpisode)        'otherwise, each episode is appended to the 'multiepisode' element
        Next

        doc.AppendChild(root)
        Try
            SaveXMLDoc(doc, path)
        Catch
        End Try

    End Sub

    
    ''' <summary>
    ''' To apply values to tags for Tv Episodes in one place
    ''' </summary>
    ''' <param name="tvep">Reference to tvepisode object</param>
    ''' <param name="xmlvalue">XxmNode to pass Name and Value</param>
    Public Shared Sub epTagApply(ByRef tvep As TvEpisode, byVal xmlvalue As XMLNode)
        Select Case xmlvalue.Name
            Case "title"
                tvep.Title.Value = xmlvalue.InnerText
            Case "season"
                tvep.Season.Value = xmlvalue.InnerText
            Case "episode"
                tvep.Episode.Value = xmlvalue.InnerText
            Case "tvdbid"
                tvep.TvdbId.Value = xmlvalue.InnerText
            Case "rating"
                Dim rating As String = ""
                rating = xmlvalue.InnerText
                If rating.IndexOf("/10") <> -1  Then rating.Replace("/10", "")
                If rating.IndexOf(" ") <> -1    Then rating.Replace(" ", "")
                If rating.IndexOf(".") <> -1 OrElse rating.IndexOf(",") <> -1Then
                    rating = rating.Substring(0,3)
                End If
                tvep.Rating.Value = rating
            Case "ratings"
                Dim what As XmlNode = Nothing
                For Each what In xmlvalue.ChildNodes
                    Select Case what.name
                        Case "rating"
                            If what.Attributes("name").Value = "default" Then
                                Dim what2 As XmlNode = Nothing
                                For each what2 In what.ChildNodes
                                    Select Case what2.Name
                                        Case "value"
                                            tvep.Rating.Value = what2.InnerText.ToRating.ToString
                                        Case "votes"
                                            Dim vote As String = what2.InnerText
                                            If Not String.IsNullOrEmpty(vote) Then vote = vote.Replace(",", "")
                                            tvep.Votes.Value = vote
                                    End Select
                                Next
                            End If
                    End Select
                Next
            Case "votes"
                tvep.Votes.Value = xmlvalue.InnerText
            Case "playcount"
                tvep.PlayCount.Value = xmlvalue.InnerText
            Case "aired"
                tvep.Aired.Value = xmlvalue.InnerText
            Case "plot"
                tvep.Plot.Value = xmlvalue.InnerText
            Case "director"
                If tvep.Director.Value = "" Then
                    tvep.Director.Value = xmlvalue.InnerText
                Else 
                    tvep.Director.Value += ", " & xmlvalue.InnerText
                End If
            Case "credits"
                If tvep.Credits.Value = "" Then
                    tvep.Credits.Value = xmlvalue.InnerText
                Else
                    tvep.Credits.Value += ", " & xmlvalue.InnerText
                End If
            Case "displayseason"
                tvep.DisplaySeason.Value = xmlvalue.InnerText
            Case "displayepisode"
                tvep.DisplayEpisode.Value = xmlvalue.InnerText 
            Case "videosource"
                tvep.Source.Value = xmlvalue.InnerText 
            Case "showid"
                tvep.ShowId.Value = xmlvalue.InnerText 
            Case "uniqueid"
                Dim testAttribute as XmlAttribute = CType(xmlvalue.Attributes.GetNamedItem("type"),  XmlAttribute)
                If testAttribute IsNot nothing then
                    Select Case testAttribute.Value
                        Case "imdb"     : tvep.ImdbId.Value     = xmlvalue.InnerText
                        Case "tmdb"     : tvep.TmdbId.Value     = xmlvalue.InnerText
                        Case "tvdb"     : tvep.UniqueId.Value   = xmlvalue.InnerText
                        Case "unknown"  : tvep.UniqueId.Value   = xmlvalue.InnerText
                    End Select
                    'If testAttribute.Value = "imdb" Then tvep.ImdbId.Value = xmlvalue.InnerText
                    'If testAttribute.Value = "tmdb" Then tvep.TmdbId.Value = xmlvalue.InnerText
                    'If testAttribute.Value = "tvdb" Then tvep.UniqueId.Value = xmlvalue.InnerText
                    'If testAttribute.Value = "unknown" Then tvep.UniqueId.Value = xmlvalue.InnerText
                Else
                    tvep.UniqueId.Value = xmlvalue.InnerText
                End If
                'tvep.UniqueId.Value = xmlvalue.InnerText
            Case "imdbid"
                If Not xmlvalue.InnerText = "" AndAlso xmlvalue.InnerText.StartsWith("tt") Then
                    tvep.ImdbId.Value   = xmlvalue.InnerText
                End If
            Case "epbookmark"
                tvep.EpBookmark.Value = xmlvalue.InnerText
            Case "userrating"
                tvep.UserRating.Value = xmlvalue.InnerText
            Case "dvdepnumber"
                tvep.DvdEpNumber.Value = xmlvalue.InnerText
            Case "runtime"
                tvep.Runtime.Value = xmlvalue.InnerText
            Case "actor"
                Dim actordetail As XmlNode = Nothing
                Dim newactor As New str_MovieActors(SetDefaults)
                For Each actordetail In xmlvalue.ChildNodes
                    Select Case actordetail.Name
                        Case "name"
                            newactor.actorname = actordetail.InnerText
                        Case "role"
                            newactor.actorrole = actordetail.InnerText
                        Case "order"
                            newactor.order = actordetail.InnerText 
                        Case "thumb"
                            newactor.actorthumb = actordetail.InnerText
                    End Select
                Next
                tvep.ListActors.Add(newactor)
            Case "fileinfo"
                Dim detail2 As XmlNode = Nothing
                For Each detail2 In xmlvalue.ChildNodes
                    Select Case detail2.Name
                        Case "streamdetails"
                            tvep.Streamdetails = Streamdetailsload(detail2)
                    End Select
                Next 
        End Select
    End Sub

    ''' <summary>
    ''' Complete episode number and season number
    ''' as well as apply nfo path to episode object
    ''' </summary>
    ''' <param name="tvep">Reference to tvepisode object</param>
    ''' <param name="loadpath">nfo load path</param>
    Public Shared Sub epnfofinal(ByRef tvep As TvEpisode, ByVal loadpath As String)
        tvep.NfoFilePath = loadpath
        If tvep.Episode.Value = Nothing Or tvep.Episode.Value = Nothing Then
            For Each regexp In Pref.tv_RegexScraper

                Dim M As Match
                M = Regex.Match(tvep.NfoFilePath, regexp)
                If M.Success = True Then
                    Try
                        tvep.Season.Value = M.Groups(1).Value.ToString
                        tvep.Episode.Value = M.Groups(2).Value.ToString
                        Exit For
                    Catch
                        tvep.Season.Value = "-1"
                        tvep.Season.Value = "-1"
                    End Try
                End If
            Next
        End If
        If tvep.Episode.Value = Nothing Then
            tvep.Episode.Value = "-1"
        End If
        If tvep.Season.Value = Nothing Then
            tvep.Season.Value = "-1"
        End If
        If tvep.TvdbId = Nothing Then tvep.TvdbId.Value = ""
        'If tvep.status = Nothing Then tvep.status = ""
        If tvep.Rating = Nothing Then tvep.Rating.Value = ""
        If tvep.Votes = Nothing Then tvep.Votes.Value = ""
        If String.IsNullOrEmpty(tvep.PlayCount.Value) Then tvep.PlayCount.Value = "0"
    End Sub

#End Region

#Region "Series routines"
    
    Public Function tv_NfoLoadCheck(ByVal Path As String) As Boolean
        'Check if XBMC nfo and correct some entries before loading into Media Companion.
        Dim aok As Boolean = True
        Try
            Dim thistvshow As New XmlDocument
            Using tmpstrm As IO.StreamReader = File.OpenText(Path)
                thistvshow.Load(tmpstrm)
            End Using
            Dim nodeToFind As XmlNode
		    Dim root As XmlElement = thistvshow.DocumentElement

		    ' Selects all the title elements that have an attribute named lang
		    nodeToFind = root.SelectSingleNode("epbookmark")
            If nodeToFind IsNot Nothing Then aok = False
            If Not aok Then 
                Dim tempshow As New TvShow
                tempshow = tvshow_NfoLoad(Path)
                tvshow_NfoSave(tempshow, True, "0")
                aok = True
            End If
        Catch ex As Exception
            aok = False
        End Try
        Return aok
    End Function
    
    ''' <summary>
    ''' Save TV Series nfo as xml document
    ''' </summary>
    ''' <param name="tvshowtosave">Tv Show object</param>
    ''' <param name="overwrite">Set True to overwrite nfo file.</param>
    ''' <param name="lock">Series status</param>
    Public Sub tvshow_NfoSave(ByVal tvshowtosave As TvShow, Optional ByVal overwrite As Boolean = True, Optional ByVal lock As String = "")

        Monitor.Enter(Me)
        Dim stage As Integer = 1
        Try
            Dim filenameandpath As String = tvshowtosave.NfoFilePath

            If tvshowtosave Is Nothing Then Exit Sub
            If File.Exists(filenameandpath) AndAlso Not overwrite Then Exit Sub
            Dim doc As New XmlDocument
            Dim root As XmlElement
            Dim child As XmlElement
            root = doc.CreateElement("tvshow")
            stage = 2
            Dim thispref As XmlNode = Nothing
            Dim xmlproc As XmlDeclaration

            xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
            doc.AppendChild(xmlproc)
            stage = 3
            root.AppendChild(doc, "id"                  , tvshowtosave.TvdbId.Value)
            root.AppendChild(doc, "state"               , If(lock = "", tvshowtosave.State, Media_Companion.ShowState.Open))
            root.AppendChild(doc, "title"               , tvshowtosave.Title.Value)
            root.AppendChild(doc, "showtitle"           , tvshowtosave.Title.Value)
            root.AppendChild(doc, "mpaa"                , tvshowtosave.Mpaa.Value)
            root.AppendChild(doc, "plot"                , tvshowtosave.Plot.Value)
            root.AppendChild(doc, "imdbid"              , If(tvshowtosave.ImdbId.Value = "0", "", tvshowtosave.ImdbId.Value))
            root.AppendChild(doc, "status"              , tvshowtosave.Status.Value)
            root.AppendChild(doc, "runtime"             , FormatRunTime(tvshowtosave.Runtime.Value, "", False))
            root.AppendChild(doc, "rating"              , tvshowtosave.Rating.Value)
            root.AppendChild(doc, "userrating"          , tvshowtosave.UserRating.Value)
            root.AppendChild(doc, "votes"               , CommaNoComma(tvshowtosave.Votes.Value, Pref.TvThousSeparator))
            root.AppendChild(doc, "year"                , tvshowtosave.Year.Value)
            root.AppendChild(doc, "premiered"           , tvshowtosave.Premiered.Value)
            root.AppendChild(doc, "studio"              , tvshowtosave.Studio.Value)
            stage = 15
            root.AppendChildList(doc, "genre"           , tvshowtosave.Genre.Value, "/")

            stage = 16
            child = doc.CreateElement("episodeguide")
            Dim childchild As XmlElement
            childchild = doc.CreateElement("url")
            Dim tempppp As String = tvshowtosave.tvdbid.value & ".xml"
            Dim Attr As XmlAttribute
            Attr = doc.CreateAttribute("cache")
            Attr.Value = tempppp
            childchild.Attributes.Append(Attr)
            If Not IsNothing(tvshowtosave.episodeguideurl) Then
                '"http://www.thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/all/" & language & ".zip"
                If tvshowtosave.tvdbid.Value <> Nothing Then
                    If IsNumeric(tvshowtosave.tvdbid.Value) Then
                        If tvshowtosave.language.Value <> Nothing Then
                            If tvshowtosave.language.Value <> "" Then
                                childchild.InnerText = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvshowtosave.tvdbid.Value & "/all/" & tvshowtosave.language.Value & ".zip"
                                child.AppendChild(childchild)
                            Else
                                childchild.InnerText = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvshowtosave.tvdbid.Value & "/all/en.zip"
                                child.AppendChild(childchild)
                            End If
                        Else
                            childchild.InnerText = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvshowtosave.tvdbid.Value & "/all/en.zip"
                            child.AppendChild(childchild)
                        End If
                    End If
                End If
                root.AppendChild(child)
            End If

            stage = 17
            root.AppendChild(doc, "language"            , tvshowtosave.Language.Value)
            root.AppendChildList(doc, "thumb"           , tvshowtosave.posters, True)
            root.AppendChildlist(doc, "fanart"          , tvshowtosave.fanart , True)

            stage = 20
            For each act As Actor In tvshowtosave.ListActors
                child = doc.CreateElement("actor")
                child.AppendChild(doc       , "actorid" , act.actorid)
                child.AppendChild(doc       , "name"    , act.actorname)
                child.AppendChildList(doc   , "role"    , act.actorrole)
                child.AppendChild(doc       , "thumb"   , act.actorthumb)
                child.AppendChild(doc       , "order"   , act.order)
                root.AppendChild(child)
            Next
            
            stage = 21
            root.AppendChild(doc, "episodeactorsource"  , tvshowtosave.episodeactorsource.Value)
            root.AppendChild(doc, "tvshowactorsource"   , tvshowtosave.tvshowactorsource.Value)
            root.AppendChild(doc, "sortorder"           , tvshowtosave.sortorder.Value)
            root.AppendChild(doc, "sorttitle"           , tvshowtosave.SortTitle.Value)
            stage = 23
            
            doc.AppendChild(root)

            stage = 34
            SaveXMLDoc(doc, filenameandpath)
        Catch ex As Exception
            MsgBox("Error Encountered at stage " & stage.ToString & vbCrLf & vbCrLf & ex.ToString)
        Finally
            Monitor.Exit(Me)
        End Try
    End Sub

    ''' <summary>
    ''' Load TV Series nfo from xml document
    ''' </summary>
    ''' <param name="path">Nfo path and filename</param>
    ''' <returns></returns>
    Public Function tvshow_NfoLoad(ByVal path As String) As TvShow
        Try
            Dim newtvshow As New TvShow
            Dim tvshow As New XmlDocument
            If Not File.Exists(path) Then Return blanktvshow(path)
            Try
                Using tmpstrm As IO.StreamReader = File.OpenText(path)
                    tvshow.Load(tmpstrm)
                End Using
            Catch ex As Exception
                Return blanktvshow(path)
                Exit Function
            End Try
            newtvshow.State = Media_Companion.ShowState.Unverified
            Dim tempid As String = ""
            For Each thisresult As XmlNode In tvshow("tvshow")
                Try
                    Select Case thisresult.Name
                        Case "id"
                            newtvshow.TvdbId.Value = thisresult.InnerText
                        Case "state"
                            newtvshow.State = thisresult.InnerText
                        Case "title"
                            Dim tempstring As String = ""
                            tempstring = thisresult.InnerText
                            ' Ignore Article here??
                            newtvshow.Title.Value = tempstring
                        Case "episodeguide"
                            tempid = thisresult.InnerText
                        Case "imdbid"
                            newtvshow.ImdbId.Value = thisresult.InnerText
                        Case "status"
                            newtvshow.Status.Value = thisresult.InnerText 
                        Case "mpaa"
                            newtvshow.Mpaa.Value = thisresult.InnerText
                        Case "plot"
                            newtvshow.Plot.Value = thisresult.InnerText
                        Case "runtime"
                            newtvshow.Runtime.Value = thisresult.InnerText
                        Case "rating"
                            Dim tmpstr As String = thisresult.InnerText
                            If tmpstr.IndexOf("/10") <> -1 Then tmpstr.Replace("/10", "")
                            If tmpstr.IndexOf(" ") <> -1 Then tmpstr.Replace(" ", "")
                            newtvshow.Rating.Value = tmpstr
                        Case "ratings"
                            Dim what As XmlNode = Nothing
                            For Each what In thisresult.ChildNodes
                                Select Case what.name
                                    Case "rating"
                                        If what.Attributes("name").Value = "default" Then
                                            Dim what2 As XmlNode = Nothing
                                            For each what2 In what.ChildNodes
                                                Select Case what2.Name
                                                    Case "value"
                                                        newtvshow.Rating.Value = what2.InnerText.ToRating.ToString
                                                    Case "votes"
                                                        Dim vote As String = what2.InnerText
                                                        If Not String.IsNullOrEmpty(vote) Then vote = vote.Replace(",", "")
                                                        newtvshow.Votes.Value = vote
                                                End Select
                                            Next
                                        End If
                                End Select
                            Next
                        Case "userrating"
                            newtvshow.UserRating.Value = thisresult.InnerText
                        Case "votes"
                            newtvshow.Votes.Value = thisresult.InnerText
                        Case "year"
                            newtvshow.Year.Value = thisresult.InnerText
                        Case "premiered"
                            newtvshow.Premiered.Value = thisresult.InnerText
                        Case "studio"
                            newtvshow.Studio.Value = thisresult.InnerText
                        Case "genre"
                            If newtvshow.genre.Value = "" Then
                                newtvshow.genre.Value = thisresult.InnerText
                            Else
                                newtvshow.genre.Value += " / " & thisresult.InnerText
                            End If
                        Case "language"
                            newtvshow.Language.Value = thisresult.InnerText
                        Case "episodeactorsource"
                            newtvshow.EpisodeActorSource.Value = thisresult.InnerText
                        Case "tvshowactorsource"
                            newtvshow.TvShowActorSource.Value = thisresult.InnerText
                        Case "sortorder"
                            newtvshow.SortOrder.Value = thisresult.InnerText
                        Case "sorttitle"
                            newtvshow.SortTitle.Value = thisresult.InnerText 
                        Case "actor"
                            Dim newactor As New str_MovieActors(SetDefaults)
                            Dim detail As XmlNode = Nothing
                            For Each detail In thisresult.ChildNodes
                                Select Case detail.Name
                                    Case "actorid"
                                        newactor.actorid = detail.InnerText
                                    Case "name"
                                        newactor.actorname = detail.InnerText
                                    Case "role"
                                        newactor.actorrole = detail.InnerText
                                    Case "thumb"
                                        newactor.actorthumb = detail.InnerText
                                    Case "order"
                                        newactor.order = detail.InnerText 
                                End Select
                            Next
                            newtvshow.listactors.Add(newactor)
                    End Select
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
            Next
            
            newtvshow.NfoFilePath = path
            
            If newtvshow.tvdbid         = Nothing Then newtvshow.TvdbId.Value = ""
            If newtvshow.ImdbId.Value   = Nothing Then newtvshow.ImdbId.Value = ""
            If newtvshow.Mpaa.Value     = Nothing Then newtvshow.Mpaa.Value = "na"
            If newtvshow.Studio.Value   = Nothing Then newtvshow.Studio.Value = "-"
            If newtvshow.Runtime.Value  = Nothing Then newtvshow.Runtime.Value = "0"
            If newtvshow.Year.Value <> "" AndAlso newtvshow.Year.Value.ToInt <> 0 AndAlso newtvshow.Year.Value <> "0000" AndAlso Not IsNothing(newtvshow.Premiered.Value) Then
                If newtvshow.Premiered.Value.Length = 10 Then
                    Dim tmp As String = newtvshow.Premiered.Value.Substring(0,4)
                    newtvshow.Year.Value = tmp
                End If
            ElseIf String.IsNullOrEmpty(newtvshow.Year.Value) Then
                newtvshow.Year.Value = "0000"
            End If
            ' catch bug where MC was setting sort order to Default, not to lowercase 'default' value.
            If newtvshow.SortOrder.Value <> "" Then newtvshow.SortOrder.Value = newtvshow.SortOrder.Value.ToLower

            If newtvshow.TvShowActorSource.Value = Nothing Then
                If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Then
                    newtvshow.TvShowActorSource.Value = "tvdb"
                Else
                    newtvshow.TvShowActorSource.Value = "imdb"
                End If
            End If
            If newtvshow.EpisodeActorSource.Value = Nothing Then
                If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 2 Then
                    newtvshow.EpisodeActorSource.Value = "tvdb"
                Else
                    newtvshow.EpisodeActorSource.Value = "imdb"
                End If
            End If
            If tempid <> "" Then
                Dim i As Integer = tempid.LastIndexOf("/")+1
                tempid = tempid.Substring(i, (tempid.length-i))
                tempid = tempid.Replace(".zip", "")
                newtvshow.Language.Value = tempid
            End If
            If newtvshow.SortOrder.Value = Nothing Then newtvshow.SortOrder.Value = Pref.sortorder 

            Return newtvshow
        Catch
        End Try
        Return Nothing
    End Function

    Public Function blanktvshow(ByVal path As String) As TvShow
        blanktvshow = New TvShow
        blanktvshow.Title.Value = Utilities.GetLastFolder(path)
        'blanktvshow.Year.Value = blanktvshow.Title.Value & " (0000)"
        blanktvshow.NfoFilePath = path
        blanktvshow.Year.Value = "0000"
        blanktvshow.TvdbId.Value = ""
        blanktvshow.Status.Value = "missing"
        blanktvshow.State = Media_Companion.ShowState.Locked
        Return blanktvshow
    End Function

    Public Sub SeriesXMLSave(ByVal Series As TheTvDB.TvdbSeries, ByVal filenameandpath As String, Optional ByVal overwrite As Boolean = True)
        Monitor.Enter(Me)
        Try
            If File.Exists(filenameandpath) AndAlso Not overwrite Then Exit Sub
            Dim doc As New XmlDocument
            Dim xmlproc As XmlDeclaration
            xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
            doc.AppendChild(xmlproc)
            Dim root As XmlElement
            Dim child As XmlElement
            root = doc.CreateElement("Data")
            child = doc.CreateElement("Series")
            child.AppendChild(doc,  "id"                , Series.Identity)
            'child.AppendChild(doc,  "Actors"            , Series.Actors)
            child.AppendChild(doc,  "Airs_DayOfWeek"    , Series.AirsDayOfWeek)
            child.AppendChild(doc,  "Airs_Time"         , Series.AirsTime)
            child.AppendChild(doc,  "ContentRating"     , Series.ContentRating)
            child.AppendChild(doc,  "FirstAired"        , Series.FirstAired)
            child.AppendChild(doc,  "Genre"             , Series.GenresDisplayString)
            child.AppendChild(doc,  "IMDB_ID"           , Series.ImdbID)
            child.AppendChild(doc,  "Language"          , Series.Language)
            child.AppendChild(doc,  "Network"           , Series.Network)
            child.AppendChild(doc,  "NetworkID"         , Series.NetworkId)
            child.AppendChild(doc,  "Overview"          , Series.Overview)
            child.AppendChild(doc,  "Rating"            , Series.Rating)
            child.AppendChild(doc,  "RatingCount"       , Series.Votes)
            child.AppendChild(doc,  "Runtime"           , Series.Runtime)
            child.AppendChild(doc,  "SeriesID"          , Series.SeriesId)
            child.AppendChild(doc,  "SeriesName"        , Series.SeriesName)
            child.AppendChild(doc,  "Status"            , Series.Status)
            child.AppendChild(doc,  "added"             , Series.AddedDate)
            child.AppendChild(doc,  "addedby"           , Series.AddedBy)
            child.AppendChild(doc,  "banner"            , Series.Banner)
            child.AppendChild(doc,  "fanart"            , "")
            child.AppendChild(doc,  "lastupdated"       , Series.LastUpdated)
            child.AppendChild(doc,  "poster"            , "")
            child.AppendChild(doc,  "zap2it_id"         , Series.Zap2ItID)
            root.AppendChild(child)
            For each ep In Series.Episodes
                child = doc.CreateElement("Episode")
                child.AppendChild(doc,  "id"                    , ep.Identity)
                child.AppendChild(doc,  "DVD_chapter"           , ep.DVDChapter)
                child.AppendChild(doc,  "DVD_discid"            , ep.DVDDiscID)
                child.AppendChild(doc,  "DVD_episodenumber"     , ep.DVDEpisodeNumber)
                child.AppendChild(doc,  "DVD_season"            , ep.DVDSeasonNumber)
                child.AppendChild(doc,  "Director"              , ep.DirectorsDisplayString)
                child.AppendChild(doc,  "EpisodeName"           , ep.EpisodeName)
                child.AppendChild(doc,  "EpisodeNumber"         , ep.EpisodeNumber)
                child.AppendChild(doc,  "FirstAired"            , ep.FirstAired)
                child.AppendChild(doc,  "GuestStart"            , ep.GuestStarsDisplayString)
                child.AppendChild(doc,  "IMDB_ID"               , ep.ImdbId)
                child.AppendChild(doc,  "Language"              , ep.Language.episodeName)
                child.AppendChild(doc,  "Overview"              , ep.Overview)
                child.AppendChild(doc,  "ProductionCode"        , ep.ProductionCode)
                child.AppendChild(doc,  "Rating"                , ep.Rating)
                child.AppendChild(doc,  "RatingCount"           , ep.Votes)
                child.AppendChild(doc,  "SeasonNumber"          , ep.SeasonNumber)
                child.AppendChild(doc,  "Writer"                , ep.WritersDisplayString)
                child.AppendChild(doc,  "absolute_number"       , ep.AbsoluteNumber)
                child.AppendChild(doc,  "airsafter_season"      , ep.AirsAfterSeason)
                child.AppendChild(doc,  "airsbefore_episode"    , ep.AirsBeforeEpisode)
                child.AppendChild(doc,  "airsbefore_season"     , ep.AirsBeforeSeason)
                child.AppendChild(doc,  "filename"              , ep.Image)
                child.AppendChild(doc,  "is_movie"              , 0)
                child.AppendChild(doc,  "lastupdated"           , ep.LastUpdated)
                child.AppendChild(doc,  "thumb_added"           , ep.ThumbAdded)
                child.AppendChild(doc,  "thumb_height"          , ep.ThumbHeight)
                child.AppendChild(doc,  "thumb_width"           , ep.ThumbWidth)
                root.AppendChild(child)
            Next
            doc.AppendChild(root)
            SaveXMLDoc(doc, filenameandpath)

        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Get full series and episode info from series.xml
    ''' </summary>
    ''' <param name="filenameandpath">Full path and filename.  Returns "error" if file not found</param>
    ''' <returns>TvdbSeries orelse Nothing if file not found</returns>
    Public Function SeriesXMLLoad(ByRef filenameandpath As String) As TheTvDB.TvdbSeries
        If Not File.Exists(filenameandpath) Then 
            filenameandpath = "error"
            Return Nothing
        End If
        Dim newSeries As New TheTvDB.TvdbSeries
        Dim tvshow As New XmlDocument
        Try
            Using tmpstrm As IO.StreamReader = File.OpenText(filenameandpath)
                tvshow.Load(tmpstrm)
            End Using
        Catch ex As Exception
            filenameandpath = "error"
            Return Nothing
        End Try
        For Each thisresult As XmlNode In tvshow("Data")
            Select Case thisresult.Name
                Case "Series"
                    Dim result As XmlNode
					For Each result In thisresult.childnodes
                        Select Case result.Name
                            Case "id"               : newSeries.Identity            = result.InnerText
                            Case "Airs_DayOfWeek"   : newSeries.AirsDayOfWeek       = result.InnerText
                            Case "Airs_Time"        : newSeries.AirsTime            = result.InnerText
                            Case "Airs_Time"        : newSeries.AirsTime			= result.InnerText
                            Case "ContentRating"    : newSeries.ContentRating		= result.InnerText
                            Case "FirstAired"       : newSeries.FirstAired			= result.InnerText
                            Case "Genre"            : newSeries.GenresDisplayString = result.InnerText
                            Case "IMDB_ID"          : newSeries.ImdbID				= result.InnerText
                            Case "Language"         : newSeries.Language			= result.InnerText
                            Case "Network"          : newSeries.Network				= result.InnerText
                            Case "NetworkID"        : newSeries.NetworkId			= result.InnerText
                            Case "Overview"         : newSeries.Overview			= result.InnerText
                            Case "Rating"           : newSeries.Rating				= result.InnerText
                            Case "RatingCount"      : newSeries.Votes				= result.InnerText
                            Case "Runtime"          : newSeries.Runtime             = result.InnerText	
                            Case "SeriesID"         : newSeries.SeriesId			= result.InnerText
                            Case "SeriesName"       : newSeries.SeriesName			= result.InnerText	
                            Case "Status"           : newSeries.Status				= result.InnerText
                            Case "added"            : newSeries.AddedDate			= result.InnerText
                            Case "addedby"          : newSeries.AddedBy				= result.InnerText
                            Case "banner"           : newSeries.Banner				= result.InnerText
                            Case "lastupdated"      : newSeries.LastUpdated			= result.InnerText
                            Case "zap2it_id"        : newSeries.Zap2ItID			= result.InnerText
                        End Select
					Next
                Case "Episode"
                    Dim newEpisode As New TheTvDB.TvdbEpisode
                    For Each result In thisresult.childnodes
                        Select Case result.Name
                            Case "id"                    : newEpisode.Identity                  = result.InnerText
                            Case "DVD_chapter"           : newEpisode.DVDChapter                = result.InnerText
                            Case "DVD_discid"            : newEpisode.DVDDiscID                 = result.InnerText
                            Case "DVD_episodenumber"     : newEpisode.DVDEpisode                = result.InnerText
                            Case "DVD_season"            : newEpisode.DVDSeason                 = result.InnerText
                            Case "Director"              : newEpisode.DirectorsDisplayString    = result.InnerText
                            Case "EpisodeName"           : newEpisode.EpisodeName               = result.InnerText
                            Case "EpisodeNumber"         : newEpisode.EpisodeNumber             = result.InnerText
                            Case "FirstAired"            : newEpisode.FirstAired                = result.InnerText
                            Case "GuestStart"            : newEpisode.GuestStarsDisplayString   = result.InnerText
                            Case "IMDB_ID"               : newEpisode.ImdbId                    = result.InnerText
                            Case "Language"              : newEpisode.Language.episodeName      = result.InnerText
                            Case "Overview"              : newEpisode.Overview                  = result.InnerText
                            Case "ProductionCode"        : newEpisode.ProductionCode            = result.InnerText
                            Case "Rating"                : newEpisode.Rating                    = result.InnerText
                            Case "RatingCount"           : newEpisode.Votes                     = result.InnerText
                            Case "SeasonNumber"          : newEpisode.SeasonNumber              = result.InnerText
                            Case "Writer"                : newEpisode.WritersDisplayString      = result.InnerText
                            Case "absolute_number"       : newEpisode.AbsoluteNumber            = result.InnerText
                            Case "airsafter_season"      : newEpisode.AirsAfterSeason           = result.InnerText
                            Case "airsbefore_episode"    : newEpisode.AirsBeforeEpisode         = result.InnerText
                            Case "airsbefore_season"     : newEpisode.AirsBeforeSeason          = result.InnerText
                            Case "filename"              : newEpisode.Image                     = result.InnerText
                            Case "lastupdated"           : newEpisode.LastUpdated               = result.InnerText
                            Case "thumb_added"           : newEpisode.ThumbAdded                = result.InnerText
                            Case "thumb_height"          : newEpisode.ThumbHeight               = result.InnerText
                            Case "thumb_width"           : newEpisode.ThumbWidth                = result.InnerText 
                        End Select
                    Next
                    newSeries.Episodes.Add(newEpisode)
            End Select
        Next
        Return newseries
    End Function
    
#End Region

#End Region

#Region " Obsolete "
    
    'Public Function tv_NfoLoadFull(ByVal path As String) As TvShow
    '    Dim newtvshow As New TvShow
    '    If Not File.Exists(path) Then
    '        newtvshow.Title.Value = Utilities.GetLastFolder(path)
    '        newtvshow.Plot.Value = "problem loading tvshow.nfo, file does not exist." & vbCrLf & "Use the TV Show Selector Tab to create one"
    '        newtvshow.Status.Value = "file does not exist"
    '        newtvshow.NfoFilePath = path
    '        newtvshow.Year.Value = "0000"
    '        newtvshow.TvdbId.Value = ""
    '        newtvshow.State = Media_Companion.ShowState.Locked
    '        Return newtvshow
    '        Exit Function
    '    Else
    '        newtvshow.NfoFilePath = path
    '        newtvshow.Load()
    '        'Fix episodeguide tag
    '        Dim lang As String = newtvshow.EpisodeGuideUrl.Value
    '        If String.IsNullOrEmpty(lang) Then
    '            lang = "en"
    '        Else
    '            lang = lang.Substring((lang.LastIndexOf("/") + 1)).Replace(".zip", "")
    '        End If

    '        If Not newtvshow.TvdbId.Value = "" Then
    '        newtvshow.EpisodeGuideUrl.Value = ""
    '            newtvshow.Url.Value = URLs.EpisodeGuide(newtvshow.TvdbId.Value, lang)
    '            newtvshow.Url.Node.SetAttributeValue("cache", newtvshow.TvdbId.Value)
    '        End If
    '        'end fix
    '        If IsNothing(newtvshow.Year.Value) Then
    '            If newtvshow.Premiered.Value.Length = 10 Then
    '                newtvshow.Year.Value = newtvshow.Premiered.Value.Substring(0,4)
    '            End If
    '        ElseIf newtvshow.Year.Value.ToInt = 0 AndAlso newtvshow.Premiered.Value.Length = 10 Then
    '            newtvshow.Year.Value = newtvshow.Premiered.Value.Substring(0,4)
    '        End If
    '    End If
    '    For Each season As TvSeason In newtvshow.Seasons.Values
    '        For Each episode In season.Episodes
    '            episode.ShowId = newtvshow.TvdbId
    '        Next
    '    Next
    '    Return newtvshow

    'End Function

    'Public Function tv_NfoLoad(ByVal path As String) As TvShow
    '    Dim newtvshow As New TvShow
    '    If Not File.Exists(path) Then
    '        newtvshow.Title.Value = Utilities.GetLastFolder(path)
    '        'newtvshow.Year.Value = newtvshow.Title.Value & " (0000)"
    '        newtvshow.NfoFilePath = path
    '        newtvshow.Year.Value = "0000"
    '        newtvshow.TvdbId.Value = ""
    '        newtvshow.Status.Value = "missing"
    '        newtvshow.State = Media_Companion.ShowState.Locked
    '        Return newtvshow
    '        Exit Function
    '    Else
    '        newtvshow.NfoFilePath = path
    '        newtvshow.Load()
    '        If newtvshow.Year.Value.ToInt = 0 AndAlso newtvshow.Premiered.Value.Length = 10 Then
    '            newtvshow.Year.Value = newtvshow.Premiered.Value.Substring(0,4)
    '        End If
    '    End If
    '    Return newtvshow
    'End Function

    'Public Sub tv_NfoSave(ByVal Path As String, ByRef Show As TvShow, Optional ByVal overwrite As Boolean = True, Optional ByVal forceunlocked As String = "")
    '    If File.Exists(Path) And Not overwrite Then Exit Sub

    '    Show.Save(Path)
    'End Sub

#End Region

    '  All Movie Load/Save Routines
#Region " Movie Routines "    

    ''' <summary>
    ''' Load Movie nfo as xml document to Combolist
    ''' Only used for movie Edit Form (Alt Movie Edit)
    ''' </summary>
    ''' <param name="loadpath">full path and filename of nfo</param>
    ''' <param name="mode">Set as "movielist"</param>
    ''' <param name="_oMovies">oMovies object</param>
    ''' <returns>Combolist</returns>
    Public Function mov_NfoLoadBasic(ByVal loadpath As String, ByVal mode As String,_oMovies As Movies) As ComboList
        Dim newmovie As New ComboList
        Try
            If Not File.Exists(loadpath) Then
                newmovie.title = "Error"
                Return newmovie
            End If
            newmovie.oMovies = _oMovies
            If mode = "movielist" Then
                Dim movie As New XmlDocument
                Try
                    Using tmpstrm As IO.StreamReader = File.OpenText(loadpath)
                        movie.Load(tmpstrm)
                    End Using
                Catch ex As Exception
                    If Not util_NfoValidate(loadpath) Then
                        newmovie.title = "Error"
                        Return newmovie
                    End If

                    newmovie.createdate = "999999999999"
                    Dim filecreation2 As New FileInfo(loadpath)
                    Dim myDate2 As Date = filecreation2.LastWriteTime
                    Try
                        newmovie.filedate = Format(myDate2, Pref.datePattern).ToString
                    Catch
                    End Try
                    newmovie.filename = Path.GetFileName(loadpath)
                    newmovie.foldername = Utilities.GetLastFolder(loadpath)
                    newmovie.fullpathandfilename = loadpath
                    newmovie.genre = "problem / xml error"
                    newmovie.movietag.Clear()
                    newmovie.id = ""
                    newmovie.tmdbid = ""
                    newmovie.missingdata1 = 0
                    newmovie.SetName = ""
                    newmovie.TmdbSetId = ""
                    newmovie.source = ""
                    newmovie.director = ""
                    newmovie.credits = ""
                    newmovie.originaltitle = newmovie.title
                    newmovie.outline = ""
                    newmovie.tagline = ""
                    newmovie.playcount = "0"
                    newmovie.lastplayed = ""
                    newmovie.plot = ""
                    newmovie.rating = ""
                    newmovie.runtime = "0"
                    newmovie.sortorder = ""
                    newmovie.title = Path.GetFileName(loadpath)
                    newmovie.top250 = "0"
                    newmovie.year = "1850"
                    newmovie.stars = ""
                    newmovie.usrrated = 0
                    newmovie.NumVideoBits = -1
                    Return newmovie
                End Try
                
                For Each thisresult As XmlNode In movie("movie")
                    Try
                        Select Case thisresult.Name
                            Case "title"
                                newmovie.title = thisresult.InnerText
                            Case "originaltitle"
                                newmovie.originaltitle = thisresult.InnerText
                            Case "set"
                                newmovie.SetName = thisresult.InnerText
                            Case "setid"
                                newmovie.TmdbSetId = thisresult.InnerText 
                            Case "source"
                                newmovie.source = thisresult.InnerText
                            Case "diretor"
                                If newmovie.director = "" Then
                                    newmovie.director = thisresult.InnerText
                                Else
                                    newmovie.director += " / " & thisresult.InnerText
                                End If
                            Case "credits"
                                If newmovie.credits = "" Then
                                    newmovie.credits = thisresult.InnerText 
                                Else
                                    newmovie.credits = newmovie.credits & " / " & thisresult.InnerText 
                                End If
                            Case "year"
                                Dim ayear As Integer = thisresult.InnerText.ToInt
                                If ayear = 0 Then ayear = 1850
                                newmovie.year = ayear
                            Case "outline"
                                newmovie.outline = thisresult.InnerText
                            Case "plot"
                                newmovie.plot = thisresult.InnerText
                            Case "tagline"
                                newmovie.tagline = thisresult.InnerText 
                            Case "genre"
                                If newmovie.genre = "" Then                     'genres in nfo's are individual elements - in MC cache they are one string seperated by " / "
                                    newmovie.genre = thisresult.InnerText
                                Else
                                    newmovie.genre = newmovie.genre & " / " & thisresult.InnerText
                                End If
                            Case "countries"
                                If newmovie.countries = "" Then
                                    newmovie.countries = thisresult.InnerText
                                Else
                                    newmovie.countries = newmovie.countries & " / " & thisresult.InnerText
                                End If
                            Case "studios"
                                If newmovie.studios = "" Then
                                    newmovie.studios = thisresult.InnerText 
                                Else
                                    newmovie.studios = newmovie.studios & " / " & thisresult.InnerText 
                                End If
                            Case "tag"
                                newmovie.movietag.Add(thisresult.InnerText)
                            Case "id"
                                If thisresult.Attributes.Count = 0 Then newmovie.id = thisresult.InnerText 'ignore any id nodes with attributes
                            Case "stars"
                                newmovie.stars = thisresult.InnerText 
                            Case "tmdbid"
                                newmovie.tmdbid = thisresult.InnerText 
                            Case "playcount"
                                newmovie.playcount = thisresult.InnerText
                            Case "lastplayed"
                                newmovie.lastplayed = thisresult.InnerText
                            Case "rating"
                                newmovie.rating = thisresult.InnerText.ToRating
                            Case "userrating"
                                newmovie.usrrated = thisresult.InnerText.ToInt 
                            Case " metascore"
                                newmovie.metascore = thisresult.InnerText
                            Case "top250"
                                newmovie.top250 = thisresult.InnerText
                            Case "sortorder"
                                newmovie.sortorder = thisresult.InnerText
                            Case "sorttitle"
                                newmovie.sortorder = thisresult.InnerText
                            Case "runtime"
                                newmovie.runtime = thisresult.InnerText
                                If IsNumeric(newmovie.runtime) Then
                                    newmovie.runtime = newmovie.runtime & " min"
                                End If
                            Case "createdate"
                                newmovie.createdate = thisresult.InnerText
                            Case "votes"
                                Dim vote As String = thisresult.InnerText
                                If Not String.IsNullOrEmpty(vote) Then
                                    vote = vote.Replace(",", "")
                                    newmovie.Votes = vote.toint
                                End If
                            Case "mpaa"    'aka Certificate
                                newmovie.Certificate = thisresult.InnerText
                            Case "fileinfo"
                                Dim gotWidth As Boolean
                                Dim gotHeight As Boolean
                                For Each res In thisresult.ChildNodes
                                    If res.name = "streamdetails" Then
                                        Dim newfilenfo As New StreamDetails
                                        For Each detail In res.ChildNodes
                                            Select Case detail.Name
                                                Case "video"
                                                    For Each videodetails As XmlNode In detail.ChildNodes
                                                        Select Case videodetails.Name
                                                            Case "width"
                                                                newfilenfo.Video.Width.Value = videodetails.InnerText
                                                                gotWidth = True
                                                            Case "height"
                                                                newfilenfo.Video.Height.Value = videodetails.InnerText
                                                                gotHeight = True
                                                            Case "codec"
                                                                newmovie.VideoCodec = videodetails.InnerText
                                                            Case "NumVideoBits"
                                                                newmovie.NumVideoBits = videodetails.InnerText.ToInt
                                                        End Select
                                                        If gotWidth And gotHeight Then
                                                            newmovie.Resolution = newfilenfo.Video.VideoResolution
                                                            Exit For
                                                        End If
                                                    Next
                                                Case "audio"
                                                        Dim audio As New AudioDetails
                                                        For Each audiodetails As XmlNode In detail.ChildNodes

                                                            Select Case audiodetails.Name
                                                                Case "language"
                                                                    audio.Language.Value = audiodetails.InnerText
                                                                Case "codec"
                                                                    audio.Codec.Value = audiodetails.InnerText
                                                                Case "channels"
                                                                    audio.Channels.Value = audiodetails.InnerText
                                                                Case "bitrate"
                                                                    audio.Bitrate.Value = audiodetails.InnerText
                                                            End Select
                                                        Next
                                                        newmovie.Audio.Add(audio)
                                            End Select
                                        Next
                                        If gotWidth And gotHeight Then
                                            Exit For
                                        End If
                                    End If
                                Next
    '                       Case "Resolution" : newmovie.Resolution = thisresult.InnerText          'Add later?...to replace above...
                        End Select
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                Next

                'Now we need to make sure no varibles are still set to NOTHING before returning....
                Dim filecreation As New FileInfo(loadpath)
                Dim myDate As Date = filecreation.LastWriteTime


                If newmovie.title = Nothing Then newmovie.title = "ERR - This Movie Has No TITLE!"
                If newmovie.createdate = "" Or newmovie.createdate = Nothing Then newmovie.createdate = "18000101000000"
                Try
                    newmovie.filedate = Format(myDate, Pref.datePattern)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                newmovie.filename               = Path.GetFileName(loadpath)
                newmovie.foldername             = Utilities.GetLastFolder(loadpath)
                newmovie.fullpathandfilename    = loadpath
                newmovie.rootfolder             = Pref.GetRootFolder(loadpath) & "\"
                If newmovie.genre               = Nothing Then newmovie.genre = ""
                If newmovie.id                  = Nothing Then newmovie.id = ""
                If (newmovie.tmdbid = Nothing Or newmovie.tmdbid = "0") Then newmovie.tmdbid = ""
                If newmovie.missingdata1        = Nothing Then newmovie.missingdata1 = 0
                If newmovie.source              = Nothing Then newmovie.source = ""
                If newmovie.director            = Nothing Then newmovie.director = ""
                If newmovie.credits             = Nothing Then newmovie.credits = ""
                If newmovie.SetName             = "" Then newmovie.SetName = "-None-"
                If newmovie.originaltitle       = "" Or newmovie.originaltitle = Nothing Then newmovie.originaltitle = newmovie.title
                If newmovie.playcount           = Nothing Then newmovie.playcount = "0"
                If newmovie.lastplayed          = Nothing Then newmovie.lastplayed = ""
                If newmovie.plot                = Nothing Then newmovie.plot = ""
                If newmovie.rating              = Nothing Then newmovie.rating = 0
                If newmovie.runtime             = Nothing Then newmovie.runtime = ""
                If newmovie.usrrated            = Nothing Then newmovie.usrrated = 0
                If newmovie.sortorder           = Nothing Or newmovie.sortorder = "" Then newmovie.sortorder = newmovie.title
                If newmovie.top250              = Nothing Then newmovie.top250 = "0"
                If newmovie.year                = Nothing Then newmovie.year = "0001"
                
                movie = Nothing
            End If
            Return newmovie


        Catch
        End Try

        newmovie.title = "Error"
        Return newmovie

    End Function

    ''' <summary>
    ''' Load Movie xml document from nfo to FullMovieDetails
    ''' </summary>
    ''' <param name="loadpath">Full path and filename of nfo</param>
    ''' <returns>Complete FullMovieDetails</returns>
    Public Shared Function mov_NfoLoadFull(ByVal loadpath As String) As FullMovieDetails

        ConvertFileToUTF8IfNotAlready(loadpath)

        Try
            Dim newmovie As New FullMovieDetails
            newmovie.fullmoviebody.genre = ""
            Dim newfilenfo As New StreamDetails
            newmovie.filedetails = newfilenfo
            Dim thumbstring As String = String.Empty
            Dim watched As Boolean = False
            If Not File.Exists(loadpath) Then
            Else
                Dim movie As New XmlDocument
                Try
                    Using tmpstrm As IO.StreamReader = File.OpenText(loadpath)
                        movie.Load(tmpstrm)
                    End Using
                Catch ex As Exception
                    If Not util_NfoValidate(loadpath) Then
                        File.Move(loadpath, loadpath.Replace(".nfo",".info"))
                        newmovie.fullmoviebody.title = "Error"
                        Return newmovie
                    End If
                    Dim errorstring As String
                    errorstring = ex.Message.ToString & vbCrLf & vbCrLf
                    errorstring += ex.StackTrace.ToString
                    newmovie.fullmoviebody.title        = "Unknown" 'Utilities.CleanFileName(Path.GetFileName(workingMovie.fullpathandfilename))
                    newmovie.fullmoviebody.year         = "1850"
                    newmovie.fullmoviebody.top250       = "0"
                    newmovie.fullmoviebody.credits      = ""
                    newmovie.fullmoviebody.director     = ""
                    newmovie.fullmoviebody.stars        = ""
                    newmovie.fullmoviebody.filename     = ""
                    newmovie.fullmoviebody.genre        = ""
                    newmovie.fullmoviebody.tag.Clear()    ' = ""
                    newmovie.fullmoviebody.imdbid       = ""
                    newmovie.fullmoviebody.tmdbid       = ""
                    newmovie.fullmoviebody.mpaa         = ""
                    newmovie.fullmoviebody.outline      = "This nfo file could Not be loaded"
                    newmovie.fullmoviebody.playcount    = "0"
                    newmovie.fullmoviebody.lastplayed   = ""
                    newmovie.fullmoviebody.plot         = errorstring
                    newmovie.fullmoviebody.premiered    = ""
                    newmovie.fullmoviebody.rating       = ""
                    newmovie.fullmoviebody.usrrated     = "0"
                    newmovie.fullmoviebody.runtime      = ""
                    newmovie.fullmoviebody.studio       = ""
                    newmovie.fullmoviebody.tagline      = "Rescraping the movie might fix the problem"
                    newmovie.fullmoviebody.trailer      = ""
                    newmovie.fullmoviebody.votes        = ""
                    newmovie.fullmoviebody.sortorder    = ""
                    newmovie.fullmoviebody.country      = ""
                    newmovie.fileinfo.createdate        = ""
                    Return newmovie
                End Try
                Dim Flag4resave As Boolean = False

                For Each thisresult As XmlNode In movie("movie")
                    Select Case thisresult.Name
                        Case "alternativetitle"
                            newmovie.alternativetitles.Add(thisresult.InnerText)
                        Case "set"
                            If thisresult.HasChildNodes AndAlso thisresult.ChildNodes(0).Name <> "#text" Then
                                For i = 0 To thisresult.ChildNodes.Count - 1
                                    If thisresult.ChildNodes(i).Name = "name"       Then newmovie.fullmoviebody.SetName     = thisresult.ChildNodes(i).InnerText
                                    If thisresult.ChildNodes(i).Name = "overview"   Then newmovie.fullmoviebody.SetOverview = thisresult.ChildNodes(i).InnerText
                                Next i
                            Else
                                newmovie.fullmoviebody.SetName = thisresult.InnerText
                            End If
                            
                        Case "setid"
                            newmovie.fullmoviebody.TmdbSetId = thisresult.InnerText
                        Case "collectionnumber"
                            'prefer Set Id from <setid>, so don't overwrite if coming from <collectionnumber>
                            If newmovie.fullmoviebody.TmdbSetId = "" Then
                                newmovie.fullmoviebody.TmdbSetId = thisresult.InnerText
                            End If
                        Case "videosource"
                            newmovie.fullmoviebody.source = thisresult.InnerText
                        Case "sortorder"
                            newmovie.fullmoviebody.sortorder = thisresult.InnerText
                        Case "sorttitle"
                            newmovie.fullmoviebody.sortorder = thisresult.InnerText
                        Case "votes"
                            Dim vote As String = thisresult.InnerText
                            If Not String.IsNullOrEmpty(vote) Then vote = vote.Replace(",", "")
                            newmovie.fullmoviebody.votes = vote
                        Case "country"
                            If newmovie.fullmoviebody.country = "" Then
                                newmovie.fullmoviebody.country = thisresult.InnerText
                            Else
                                newmovie.fullmoviebody.country &= " / " & thisresult.InnerText
                            End If
                        Case "outline"
                            newmovie.fullmoviebody.outline = thisresult.InnerText
                        Case "plot"
                            newmovie.fullmoviebody.plot = thisresult.InnerText
                        Case "tagline"
                            newmovie.fullmoviebody.tagline = thisresult.InnerText
                        Case "runtime"
                            newmovie.fullmoviebody.runtime = thisresult.InnerText
                            If IsNumeric(newmovie.fullmoviebody.runtime) Then
                                newmovie.fullmoviebody.runtime = newmovie.fullmoviebody.runtime & " min"
                            End If
                        Case "mpaa"
                            newmovie.fullmoviebody.mpaa = thisresult.InnerText
                        Case "credits"
                            If newmovie.fullmoviebody.credits = "" Then
                                Dim resultstr As String = thisresult.InnerText
                                If resultstr.Contains(", ") AndAlso Not resultstr.Contains(" / ") Then resultstr = resultstr.Replace(", ", " / ")
                                newmovie.fullmoviebody.credits = resultstr
                            Else
                                newmovie.fullmoviebody.credits = newmovie.fullmoviebody.credits & " / " & thisresult.InnerText
                            End If
                        Case "director"
                            If newmovie.fullmoviebody.director = "" Then
                                newmovie.fullmoviebody.director = thisresult.InnerText
                            Else
                                newmovie.fullmoviebody.director = newmovie.fullmoviebody.director & " / " & thisresult.InnerText
                            End If
                        Case "stars"
                            newmovie.fullmoviebody.stars = thisresult.InnerText.Replace(", ", " / ")

                        Case "thumb"
                            If thisresult.InnerText.IndexOf("&lt;thumbs&gt;") <> -1 Then
                                thumbstring = thisresult.InnerText
                            Else
                                'Frodo - aspect="poster"
                                If Not IsNothing(thisresult.Attributes.ItemOf("aspect")) Then
                                    newmovie.frodoPosterThumbs.Add(New FrodoPosterThumb(thisresult.Attributes("aspect").Value, thisresult.InnerText))
                                Else
                                    newmovie.listthumbs.Add(thisresult.InnerText)
                                End If
                            End If

                        'Frodo <fanart url="">
                        Case "fanart"
                            newmovie.frodoFanartThumbs.Load(thisresult)

                        Case "premiered"
                            newmovie.fullmoviebody.premiered = thisresult.InnerText
                        Case "studio"
                            If newmovie.fullmoviebody.studio = "" Then
                                Dim resultstr As String = thisresult.InnerText
                                If resultstr.Contains(", ") AndAlso Not resultstr.Contains(" / ") Then resultstr = resultstr.Replace(", ", " / ")
                                newmovie.fullmoviebody.studio = resultstr
                            Else
                                newmovie.fullmoviebody.studio &= " / " & thisresult.InnerText
                            End If
                        Case "trailer"
                            newmovie.fullmoviebody.trailer = thisresult.InnerText
                        Case "title"
                            newmovie.alternativetitles.Add(thisresult.InnerText)
                            newmovie.fullmoviebody.title = thisresult.InnerText
                        Case "originaltitle"
                            newmovie.fullmoviebody.originaltitle = thisresult.InnerText
                        Case "year"
                            Dim ayear As String = thisresult.InnerText 
                            If ayear.ToInt = 0 Then ayear = "1850"
                            newmovie.fullmoviebody.year = ayear
                        Case "genre"
                            If newmovie.fullmoviebody.genre = "" Then
                                newmovie.fullmoviebody.genre = thisresult.InnerText
                            Else
                                newmovie.fullmoviebody.genre = newmovie.fullmoviebody.genre & " / " & thisresult.InnerText
                            End If
                        Case "tag", "tags"
                            If thisresult.Name = "tags" Then Flag4resave = True
                            If Not thisresult.InnerText = "" Then newmovie.fullmoviebody.tag.Add(thisresult.InnerText)
                        Case "id"
                            Dim myresult As String = thisresult.InnerText
                            Dim testAttribute as XmlAttribute = CType(thisresult.Attributes.GetNamedItem("moviedb"),  XmlAttribute)
                            If testAttribute isnot nothing then
                                If testAttribute.InnerText = "imdb" Then newmovie.fullmoviebody.imdbid = thisresult.InnerText
                                If testAttribute.InnerText = "tmdb" Then newmovie.fullmoviebody.tmdbid = thisresult.InnerText
                                If testAttribute.InnerText = "themoviedb" Then newmovie.fullmoviebody.tmdbid = thisresult.InnerText
                            Else
                                If myresult.StartsWith("tt") Then
                                    newmovie.fullmoviebody.imdbid = myresult
                                ElseIf Pref.MovAllowNonImdbIdAsId AndAlso Regex.IsMatch(myresult, "^[A-Z|a-z]") Then
                                    newmovie.fullmoviebody.imdbid = myresult
                                Else
                                    newmovie.fullmoviebody.tmdbid = myresult
                                    myresult = nothing
                                End If
                                'Else
                                '    newmovie.fullmoviebody.tmdbid = myresult
                                '    myresult = nothing
                                'End If
                                Dim testAttribute2 as XmlAttribute = CType(thisresult.Attributes.GetNamedItem("TMDB"),  XmlAttribute)
                                If testAttribute2 IsNot Nothing Then
                                    newmovie.fullmoviebody.tmdbid = testAttribute2.InnerText
                                End If
                            End If
                        Case "tmdbid"
                            newmovie.fullmoviebody.tmdbid = thisresult.InnerText 
                        Case "playcount"
                            newmovie.fullmoviebody.playcount = thisresult.InnerText
                        Case "watched"
                            watched = If(thisresult.InnerXml = "", False, thisresult.InnerXml)
                        Case "lastplayed"
                            newmovie.fullmoviebody.lastplayed = thisresult.InnerText
                        ''' to compensate for Kodi 17 exported nfo's
                        Case "ratings"
                            Dim what As XmlNode = Nothing
                            For Each what In thisresult.ChildNodes
                                Select Case what.name
                                    Case "rating"
                                        If what.Attributes("name").Value = "default" Then
                                            Dim what2 As XmlNode = Nothing
                                            For each what2 In what.ChildNodes
                                                Select Case what2.Name
                                                    Case "value"
                                                        newmovie.fullmoviebody.rating = what2.InnerText.ToRating.ToString
                                                    Case "votes"
                                                        Dim vote As String = what2.InnerText
                                                        If Not String.IsNullOrEmpty(vote) Then vote = vote.Replace(",", "")
                                                        newmovie.fullmoviebody.votes = vote
                                                End Select
                                            Next
                                        End If
                                End Select
                            Next
                        Case "rating"
                            Dim y As String = thisresult.InnerText
                            newmovie.fullmoviebody.rating = thisresult.InnerText.ToRating.ToString
                        Case "userrating"
                            newmovie.fullmoviebody.usrrated = thisresult.InnerText
                        Case "metascore"
                            newmovie.fullmoviebody.metascore = thisresult.InnerText
                        Case "top250"
                            newmovie.fullmoviebody.top250 = thisresult.InnerText
                        Case "createdate"
                            newmovie.fileinfo.createdate = thisresult.InnerText
                        Case "showlink"
                            newmovie.fullmoviebody.showlink = thisresult.InnerText 
                        Case "LockedFields"
                            newmovie.fullmoviebody.LockedFields = thisresult.InnerText.Split(",").ToList()
                        Case "actor"
                            Dim newactor As New str_MovieActors(SetDefaults)
                            Dim detail As XmlNode = Nothing
                            For Each detail In thisresult.ChildNodes
                                Select Case detail.Name
                                    Case "id"
                                        newactor.actorid = detail.InnerText
                                    Case "name"
                                        newactor.actorname = detail.InnerText
                                    Case "role"
                                        newactor.actorrole = detail.InnerText
                                    Case "thumb"
                                        newactor.actorthumb = detail.InnerText
                                    Case "order"
                                        newactor.order = detail.InnerText
                                End Select
                            Next
                            newmovie.listactors.Add(newactor)
                        Case "fileinfo"
                            Dim what As XmlNode = Nothing
                            For Each res In thisresult.ChildNodes
                                Select Case res.name
                                    Case "streamdetails"
                                        newmovie.filedetails = Streamdetailsload(res)
                                End Select
                            Next
                    End Select
                Next
                If thumbstring <> Nothing Then
                    Do Until thumbstring.ToLower.IndexOf("http") = -1
                        Try
                            Dim tempstring As String
                            tempstring = thumbstring.ToLower.Substring(thumbstring.ToLower.LastIndexOf("http"), (thumbstring.ToLower.LastIndexOf(".jpg") + 4) - thumbstring.ToLower.LastIndexOf("http"))
                            thumbstring = thumbstring.ToLower.Replace(tempstring, "")
                            newmovie.listthumbs.Add(tempstring)
                        Catch
                            Exit Do
                        End Try
                    Loop
                End If
                If String.IsNullOrEmpty(newmovie.fullmoviebody.imdbid) Then newmovie.fullmoviebody.imdbid = "0"
                If watched Then newmovie.fullmoviebody.playcount = "1"
                newmovie.fileinfo.fullpathandfilename   = loadpath
                newmovie.fileinfo.filename              = Path.GetFileName(loadpath)
                newmovie.fileinfo.foldername            = Utilities.GetLastFolder(loadpath)
                If Path.GetFileName(loadpath).ToLower = "video_ts.nfo" Or Path.GetFileName(loadpath).ToLower = "index.nfo" Then
                    newmovie.fileinfo.videotspath       = Utilities.RootVideoTsFolder(loadpath)
                Else
                    newmovie.fileinfo.videotspath = ""
                End If
                newmovie.fileinfo.rootfolder            = Pref.GetRootFolder(loadpath) & "\"
                newmovie.fileinfo.posterpath            = Pref.GetPosterPath(loadpath, newmovie.fileinfo.filename)
                newmovie.fileinfo.trailerpath           = ""
                newmovie.fileinfo.path                  = Path.GetDirectoryName(loadpath) & "\"
                newmovie.fileinfo.basepath              = Pref.GetMovBasePath(newmovie.fileinfo.path)
                newmovie.fileinfo.fanartpath            = Pref.GetFanartPath(loadpath, newmovie.fileinfo.filename)
                newmovie.fileinfo.movsetfanartpath      = Pref.GetMovSetFanartPath(loadpath, newmovie.fullmoviebody.SetName)
                newmovie.fileinfo.movsetposterpath      = Pref.GetMovSetPosterPath(loadpath, newmovie.fullmoviebody.SetName)

                If Not String.IsNullOrEmpty(newmovie.filedetails.Video.Container.Value) Then
                    Dim container As String             = newmovie.filedetails.Video.Container.Value
                    newmovie.fileinfo.filenameandpath   = loadpath.Replace(".nfo", container)
                End If
                
                If newmovie.fullmoviebody.SetName       = "" Then newmovie.fullmoviebody.SetName      = "-None-"
                If newmovie.fullmoviebody.usrrated      = "" Then newmovie.fullmoviebody.usrrated = "0"
                movie = Nothing

                If Flag4resave Then mov_NfoSave(loadpath, newmovie, True)    ' Resave if field updated or to correct an nfo error.

                Return newmovie
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            'Monitor.Exit(Me)
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Save Movie xml document as nfo from FullMovieDetails.
    ''' </summary>
    ''' <param name="filenameandpath">Full path and filename of nfo to save</param>
    ''' <param name="movietosave">FullMovieDetails Object</param>
    ''' <param name="overwrite">All overwrite of existing nfo - Default = True</param>
    Public Shared Sub mov_NfoSave(ByVal filenameandpath As String, ByVal movietosave As FullMovieDetails, Optional ByVal overwrite As Boolean = True)
        'Monitor.Enter(Me)
        Dim stage As Integer = 1
        Dim doc As New XmlDocument
        Try
            If movietosave Is Nothing Then Exit Sub
            If Not File.Exists(filenameandpath) Or overwrite = True Then
                'Try
                
                'Dim thumbnailstring As String = "" Test code?
                stage = 2
                Dim thispref As XmlNode = Nothing
                Dim xmlproc As XmlDeclaration
                xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
                doc.AppendChild(xmlproc)
                Dim root As XmlElement = Nothing
                Dim child As XmlElement = Nothing
                Dim actorchild As XmlElement = Nothing
                Dim filedetailschild As XmlElement = Nothing
                Dim setchild As XmlElement = Nothing
                Dim anotherchild As XmlElement = Nothing

                root = doc.CreateElement("movie")
                stage = 3
                
                child = doc.CreateElement("fileinfo")
                anotherchild = doc.CreateElement("streamdetails")
                filedetailschild = doc.CreateElement("video")
                If Pref.enablehdtags = True Then
                    stage = 4
                    If Pref.MovRuntimeAsDuration AndAlso movietosave.fullmoviebody.runtime <> "" Then
                        Dim tempduration As Integer = movietosave.fullmoviebody.runtime.Replace("min","").ToInt
                        If tempduration > 1 Then tempduration = tempduration * 60
                        movietosave.filedetails.Video.DurationInSeconds.Value = tempduration
                    End If
                    stage = 6
                    anotherchild.AppendChild(AppendVideo(doc, movietosave.filedetails.Video))

                    stage = 7
                    For Each item In movietosave.filedetails.Audio
                        anotherchild.AppendChild(AppendAudio(doc, item))
                    Next
                    stage = 8
                    For Each entry In movietosave.filedetails.Subtitles
                        If Not String.IsNullOrEmpty(entry.Language.Value) Then
                            anotherchild.AppendChild(AppendSub(doc, entry))
                        End If
                    Next
                    'stage = 7
                Else
                    stage = 9
                    Dim container As String = movietosave.filedetails.Video.Container.Value
                    filedetailschild.AppendChild(doc, "container", If(String.IsNullOrEmpty(container), "", container))
                    anotherchild.AppendChild(filedetailschild)
                End If

                stage = 10
                child.AppendChild(anotherchild)
                root.AppendChild(child)

                stage = 12
                root.AppendChild(doc, "title"           , movietosave.fullmoviebody.title)
                root.AppendChild(doc, "originaltitle"   , If(String.IsNullOrEmpty(movietosave.fullmoviebody.originaltitle), movietosave.fullmoviebody.title, movietosave.fullmoviebody.originaltitle))

                stage = 14
                If Not Pref.NoAltTitle AndAlso movietosave.alternativetitles.Count > 0 Then
                    For Each title In movietosave.alternativetitles
                        If title <> movietosave.fullmoviebody.title Then root.AppendChild(doc, "alternativetitle", title)
                    Next
                End If
                stage = 25
                If Pref.MovSetOverviewToNfo AndAlso movietosave.fullmoviebody.SetName.ToLower <> "-none-" Then
                    setchild = doc.CreateElement("set")
                    setchild.AppendChild(doc, "name"    , movietosave.fullmoviebody.SetName)
                    setchild.AppendChild(doc, "overview", movietosave.fullmoviebody.SetOverview)
                    root.AppendChild(setchild)
                Else
                    If movietosave.fullmoviebody.SetName <> "-None-" Then root.AppendChild(doc, "set", movietosave.fullmoviebody.SetName)
                End If
                If movietosave.fullmoviebody.TmdbSetId <> "" Then
                    root.AppendChild(doc, "setid"       , movietosave.fullmoviebody.TmdbSetId)
                    If Pref.SetIdAsCollectionnumber Then root.AppendChild(doc, "collectionnumber", movietosave.fullmoviebody.TmdbSetId)
                End If

    '            If movietosave.fullmoviebody.SetName <> "-None-" Then root.AppendChild(doc, "set", movietosave.fullmoviebody.SetName)
				'If movietosave.fullmoviebody.TmdbSetId <> "" Then
    '                root.AppendChild(doc, "setid"       , movietosave.fullmoviebody.TmdbSetId)
    '                If Pref.SetIdAsCollectionnumber Then root.AppendChild(doc, "collectionnumber", movietosave.fullmoviebody.TmdbSetId)
    '            End If

                stage = 26
                If String.IsNullOrEmpty(movietosave.fullmoviebody.sortorder) Then movietosave.fullmoviebody.sortorder = movietosave.fullmoviebody.title
                root.AppendChild(doc, "sorttitle"       , movietosave.fullmoviebody.sortorder)
                root.AppendChild(doc, "year"            , movietosave.fullmoviebody.year)
                root.AppendChild(doc, "premiered"       , movietosave.fullmoviebody.premiered)
                root.AppendChild(doc, "rating"          , movietosave.fullmoviebody.rating.ToRating.ToString("0.0", Form1.MyCulture))
                root.AppendChild(doc, "userrating"      , If(movietosave.fullmoviebody.usrrated = "", "0", movietosave.fullmoviebody.usrrated))
                root.AppendChild(doc, "metascore"       , If(movietosave.fullmoviebody.metascore = "", "0", movietosave.fullmoviebody.metascore))

                stage = 29
                If movietosave.fullmoviebody.LockedFields.Count>0 Then
                    child = doc.CreateElement("LockedFields")
                    child.InnerText = String.Join(",", movietosave.fullmoviebody.LockedFields.ToArray())
                    root.AppendChild(child)
                End If
                
                root.AppendChild(doc, "votes"           , CommaNoComma(movietosave.fullmoviebody.votes, Pref.MovThousSeparator))
                root.AppendChild(doc, "top250"          , movietosave.fullmoviebody.top250)
                root.AppendChild(doc, "outline"         , movietosave.fullmoviebody.outline)
                root.AppendChild(doc, "plot"            , movietosave.fullmoviebody.plot)
                root.AppendChild(doc, "tagline"         , movietosave.fullmoviebody.tagline)
                root.AppendChildList(doc, "country"     , movietosave.fullmoviebody.country)

                stage = 34
                Try
                    If Pref.XtraFrodoUrls AndAlso Pref.FrodoEnabled Then
                        For Each item In movietosave.frodoPosterThumbs
                            child = doc.CreateElement("thumb")
                            child.SetAttribute("aspect", item.Aspect)
                            child.InnerText = item.Url
                            root.AppendChild(child)
                        Next
                        root.AppendChild(movietosave.frodoFanartThumbs.GetChild(doc))
                        For Each thumbnail In movietosave.listthumbs
                            Try
                                root.AppendChildList(doc, "thumb", thumbnail)
                            Catch
                            End Try
                        Next
                    End If
                Catch
                End Try
                
                stage = 35
                root.AppendChild(doc, "runtime"         , FormatRunTime(movietosave.fullmoviebody.runtime, movietosave.filedetails.Video.DurationInSeconds.Value, True))
                stage = 36
                root.AppendChild(doc, "mpaa"            , movietosave.fullmoviebody.mpaa)
                root.AppendChildList(doc, "genre"       , movietosave.fullmoviebody.genre, "/")
                root.AppendChildList(doc, "tag"         , movietosave.fullmoviebody.tag)

                stage = 39
                root.AppendChildList(doc, "credits"     , movietosave.fullmoviebody.credits)
                root.AppendChildList(doc, "director"    , movietosave.fullmoviebody.director)
                root.AppendChildList(doc, "studio"      , movietosave.fullmoviebody.studio)
                root.AppendChild(doc, "trailer"         , movietosave.fullmoviebody.trailer)
                root.AppendChild(doc, "playcount"       , movietosave.fullmoviebody.playcount)
                If Pref.MovNfoWatchTag Then root.AppendChild(doc, "watched", If(movietosave.fullmoviebody.playcount = "0", False, True))
                root.AppendChild(doc, "lastplayed"      , movietosave.fullmoviebody.lastplayed)

                stage = 45
                If Not String.IsNullOrEmpty(movietosave.fullmoviebody.imdbid) Then root.AppendChild(doc, "id", movietosave.fullmoviebody.imdbid)
                stage = 46
                If Not String.IsNullOrEmpty(movietosave.fullmoviebody.tmdbid) Then root.AppendChild(doc, "tmdbid", movietosave.fullmoviebody.tmdbid)
                stage = 47
                If Not String.IsNullOrEmpty(movietosave.fullmoviebody.source) Then root.AppendChild(doc, "videosource", movietosave.fullmoviebody.source)

                stage = 48
                root.AppendChild(doc, "showlink", movietosave.fullmoviebody.showlink)
                root.AppendChild(doc, "createdate", SetCreateDate(movietosave.fileinfo.createdate))
                root.AppendChild(doc, "stars", movietosave.fullmoviebody.stars)
                stage = 51
                Dim actorstosave As Integer = movietosave.listactors.Count
                If actorstosave > Pref.maxactors Then actorstosave = Pref.maxactors
                For f = 0 To actorstosave - 1
                    child = doc.CreateElement("actor")
                    child.AppendChild(doc, "id"     , movietosave.listactors(f).actorid      )
                    child.AppendChild(doc, "name"   , movietosave.listactors(f).actorname    )
                    child.AppendChild(doc, "role"   , movietosave.listactors(f).actorrole    )
                    child.AppendChild(doc, "thumb"  , movietosave.listactors(f).actorthumb   )
                    child.AppendChild(doc, "order"  , movietosave.listactors(f).order        )
                    root.AppendChild(child)
                Next

                doc.AppendChild(root)
                stage = 52
                Try
                    stage = 53
                    SaveXMLDoc(doc, filenameandpath)
                    stage = 54
                Catch
                End Try
            Else
                MsgBox("File already exists")
            End If

        Catch ex As Exception
            MsgBox("Error Encountered at stage " & stage.ToString & vbCrLf & vbCrLf & ex.ToString)
        Finally
            'Monitor.Exit(Me)
        End Try
        doc = Nothing
    End Sub
#End Region
    
    '  All HomeMovie Load/Save Routines
#Region " Home Movie Routines"

    ''' <summary>
    ''' Load HomeMovie nfo as xml document to FullMovieDetails
    ''' </summary>
    ''' <param name="filepath">Full path and filename of nfo</param>
    ''' <returns>FullMovieDetails</returns>
    Public Shared Function nfoLoadHomeMovie(ByVal filepath As String)
        Try
            Dim newHomeMovie As New FullMovieDetails
            newHomeMovie.fileinfo.fullpathandfilename = filepath
            If Not File.Exists(filepath) Then
                Return "Error"
            Else
                Dim movie As New XmlDocument
                Try
                    Using tmpstrm As IO.StreamReader = File.OpenText(filepath)
                        movie.Load(tmpstrm)
                    End Using
                Catch ex As Exception
                    If Not util_NfoValidate(filepath, True) Then
                        newHomeMovie.fullmoviebody.title = "ERROR"
                        Return "ERROR"
                    End If
                    Return (newHomeMovie)
                End Try
                
                For Each thisresult As XmlNode In movie("movie")
                    Try
                        Select Case thisresult.Name
                            Case "title"
                                Dim tempstring As String = ""
                                tempstring = thisresult.InnerText
                                newHomeMovie.fullmoviebody.title = Pref.RemoveIgnoredArticles(tempstring)
                            Case "set"          : newHomeMovie.fullmoviebody.SetName    = thisresult.InnerText
                            Case "stars"        : newHomeMovie.fullmoviebody.stars      = thisresult.InnerText
                            Case "year"         : newHomeMovie.fullmoviebody.year       = thisresult.InnerText
                            Case "plot"         : newHomeMovie.fullmoviebody.plot       = thisresult.InnerText
                            Case "playcount"    : newHomeMovie.fullmoviebody.playcount  = thisresult.InnerText
                            Case "sorttitle"    : newHomeMovie.fullmoviebody.sortorder  = thisresult.InnerText
                            Case "runtime"      : newHomeMovie.fullmoviebody.runtime    = thisresult.InnerText
                                If IsNumeric(newHomeMovie.fullmoviebody.runtime) Then
                                    newHomeMovie.fullmoviebody.runtime = newHomeMovie.fullmoviebody.runtime & " min"
                                End If
                            Case "fileinfo"
                                Dim what As XmlNode = Nothing
                                For Each res In thisresult.ChildNodes
                                    Select Case res.name
                                        Case "streamdetails"
                                            newHomeMovie.filedetails = Streamdetailsload(res)
                                    End Select
                                Next
                            Case "genre"
                                If newHomeMovie.fullmoviebody.genre = "" Then
                                    newHomeMovie.fullmoviebody.genre = thisresult.InnerText
                                Else
                                    newHomeMovie.fullmoviebody.genre = newHomeMovie.fullmoviebody.genre & " / " & thisresult.InnerText
                                End If
                            Case "tag"
                                newHomeMovie.fullmoviebody.tag.Add(thisresult.InnerText)
                        End Select
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                Next

                'Now we need to make sure no varibles are still set to NOTHING before returning....

                If newHomeMovie.fullmoviebody.title         = Nothing Then newHomeMovie.fullmoviebody.title = "ERR - This Movie Has No TITLE!"
                newHomeMovie.fullmoviebody.filename         = Path.GetFileName(filepath)
                If newHomeMovie.fullmoviebody.playcount     = Nothing Then newHomeMovie.fullmoviebody.playcount = "0"
                If newHomeMovie.fullmoviebody.plot          = Nothing Then newHomeMovie.fullmoviebody.plot = ""
                If newHomeMovie.fullmoviebody.runtime       = Nothing Then newHomeMovie.fullmoviebody.runtime = ""
                If newHomeMovie.fullmoviebody.sortorder     = Nothing Or newHomeMovie.fullmoviebody.sortorder = "" Then newHomeMovie.fullmoviebody.sortorder = newHomeMovie.fullmoviebody.title
                If newHomeMovie.fullmoviebody.year          = Nothing Then newHomeMovie.fullmoviebody.year = "1901"
                newHomeMovie.fileinfo.fullpathandfilename   = filepath
                newHomeMovie.fileinfo.filename              = Path.GetFileName(filepath)
                newHomeMovie.fileinfo.foldername            = Utilities.GetLastFolder(filepath)
                newHomeMovie.fileinfo.posterpath            = Pref.GetPosterPath(filepath, newHomeMovie.fileinfo.filename)
                newHomeMovie.fileinfo.trailerpath           = ""
                newHomeMovie.fileinfo.rootfolder            = Pref.GetRootFolder(filepath) & "\"
                newHomeMovie.fileinfo.path                  = Path.GetDirectoryName(filepath) & "\"
                newHomeMovie.fileinfo.basepath              = Pref.GetMovBasePath(newHomeMovie.fileinfo.path)
                newHomeMovie.fileinfo.fanartpath            = Pref.GetFanartPath(filepath, newHomeMovie.fileinfo.filename)
                If Path.GetFileName(filepath).ToLower = "video_ts.nfo" Or Path.GetFileName(filepath).ToLower = "index.nfo" Or Path.GetFileName(filepath).ToLower = "vr_mangr.nfo" Then
                    newHomeMovie.fileinfo.videotspath       = Utilities.RootVideoTsFolder(filepath)
                Else
                    newHomeMovie.fileinfo.videotspath = ""
                End If
                If newHomeMovie.filedetails.Video.Container.Value <> "" Then
                    Dim container As String             = newHomeMovie.filedetails.Video.Container.Value
                    If container.tolower = ".vro" Then
                        newHomeMovie.fileinfo.filenameandpath   = filepath.Replace("VR_MANGR.nfo", "VR_MOVIE.VRO")
                    ElseIf container.tolower = ".ifo"
                        newHomeMovie.fileinfo.filenameandpath   = filepath.Replace("VIDEO_TS.nfo", "VTS_01_1.VOB")
                    Else
                        newHomeMovie.fileinfo.filenameandpath   = filepath.Replace(".nfo", container)
                    End If
                Else
                    newHomeMovie.fileinfo.filenameandpath   = Utilities.GetFileName(filepath, True)
                End If
                
                movie = Nothing
                Return newHomeMovie
            End If
        Catch
        End Try
        Return "Error"

    End Function

    ''' <summary>
    ''' Save HomeMovie xml document as nfo from FullMovieDetails.
    ''' </summary>
    ''' <param name="filenameandpath">Full path and filename of nfo to save</param>
    ''' <param name="homemovietosave">FullMovieDetails Object</param>
    ''' <param name="overwrite">All overwrite of existing nfo - Default = True</param>
    Public Shared Sub nfoSaveHomeMovie(ByVal filenameandpath As String, ByVal homemovietosave As FullMovieDetails, Optional ByVal overwrite As Boolean = True)

        If homemovietosave Is Nothing Then Exit Sub
        If Not File.Exists(filenameandpath) Or overwrite = True Then
            Dim doc As New XmlDocument
            Dim xmlproc As XmlDeclaration
            xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
            doc.AppendChild(xmlproc)
            Dim root As XmlElement = Nothing
            Dim child As XmlElement = Nothing
            Dim anotherchild As XmlElement = Nothing

            root = doc.CreateElement("movie")

            If Pref.enablehdtags = True Then
                child = doc.CreateElement("fileinfo")
                anotherchild = doc.CreateElement("streamdetails")
                anotherchild.AppendChild(AppendVideo(doc, homemovietosave.filedetails.Video))
                
                For Each item In homemovietosave.filedetails.Audio
                    anotherchild.AppendChild(AppendAudio(doc, item))
                Next
                
                For Each entry In homemovietosave.filedetails.Subtitles
                    If Not String.IsNullOrEmpty(entry.Language.Value) Then
                        anotherchild.AppendChild(AppendSub(doc, entry))
                    End If
                Next
                child.AppendChild(anotherchild)
                root.AppendChild(child)
            End If
            If String.IsNullOrEmpty(homemovietosave.fullmoviebody.sortorder) Then homemovietosave.fullmoviebody.sortorder = homemovietosave.fullmoviebody.title
            root.AppendChild(doc        , "title"       , homemovietosave.fullmoviebody.title)
            root.AppendChild(doc        , "set"         , homemovietosave.fullmoviebody.SetName)
            root.AppendChild(doc        , "sorttitle"   , homemovietosave.fullmoviebody.sortorder)
            root.AppendChild(doc        , "year"        , homemovietosave.fullmoviebody.year)
            root.AppendChild(doc        , "plot"        , homemovietosave.fullmoviebody.plot)
            root.AppendChild(doc        , "runtime"     , FormatRunTime(homemovietosave.fullmoviebody.runtime, homemovietosave.filedetails.Video.DurationInSeconds.Value, False))
            root.AppendChild(doc        , "playcount"   , homemovietosave.fullmoviebody.playcount)
            root.AppendChild(doc        , "createdate"  , SetCreateDate(homemovietosave.fileinfo.createdate))
            root.AppendChild(doc        , "stars"       , homemovietosave.fullmoviebody.stars)
            root.AppendChildList(doc    , "genre"       , homemovietosave.fullmoviebody.genre, "/")
            root.AppendChildList(doc    , "tag"         , homemovietosave.fullmoviebody.tag)
            doc.AppendChild(root)
            SaveXMLDoc(doc, filenameandpath)
        Else
            MsgBox("File already exists")
        End If
    End Sub
    
#End Region

    '  All Music Video Load/Save Routines
#Region " Music Video Routines "

    ''' <summary>
    ''' Save MusicVideo xml document as nfo from FullMovieDetails.
    ''' </summary>
    ''' <param name="filenameandpath">Full path and filename of nfo to save</param>
    ''' <param name="musicvidtosave">FullMovieDetails Object</param>
    Public Shared Sub MVsaveNfo(ByVal filenameandpath As String, ByVal musicvidtosave As FullMovieDetails)
        Dim doc As New XmlDocument
        Dim thumbnailstring As String = ""
        Dim xmlproc As XmlDeclaration
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        Dim root As XmlElement = Nothing
        Dim child As XmlElement = Nothing
        Dim anotherchild As XmlElement = Nothing

        root = doc.CreateElement("musicvideo")
        child = doc.CreateElement("fileinfo")
        anotherchild = doc.CreateElement("streamdetails")
        anotherchild.AppendChild(AppendVideo(doc, musicvidtosave.filedetails.Video))

        For Each item In musicvidtosave.filedetails.Audio
            anotherchild.AppendChild(AppendAudio(doc, item))
        Next

        For each item In musicvidtosave.filedetails.Subtitles
            anotherchild.AppendChild(AppendSub(doc, item))
        Next

        child.AppendChild(anotherchild)
        root.AppendChild(child)
        
        root.AppendChild(doc, "tmdbid"  , musicvidtosave.fullmoviebody.tmdbid)
        root.AppendChild(doc, "title"   , musicvidtosave.fullmoviebody.title)
        root.AppendChild(doc, "year"    , musicvidtosave.fullmoviebody.year)
        root.AppendChild(doc, "artist"  , musicvidtosave.fullmoviebody.artist)
        root.AppendChild(doc, "director", musicvidtosave.fullmoviebody.director)
        root.AppendChild(doc, "album"   , musicvidtosave.fullmoviebody.album)
        root.AppendChild(doc, "genre"   , musicvidtosave.fullmoviebody.genre)
        root.AppendChild(doc, "thumb"   , musicvidtosave.fullmoviebody.thumb)
        root.AppendChild(doc, "track"   , musicvidtosave.fullmoviebody.track)

        If musicvidtosave.fullmoviebody.runtime = "-1" AndAlso musicvidtosave.filedetails.Video.DurationInSeconds.Value.ToInt > 0 Then
            Try
                Dim seconds As Integer = Convert.ToInt32(musicvidtosave.filedetails.Video.DurationInSeconds.Value)
                Dim hms = TimeSpan.FromSeconds(seconds)
                Dim h = hms.Hours
                Dim m = hms.Minutes
                Dim runtime As Integer
                runtime = (h*60)+m
                musicvidtosave.fullmoviebody.runtime = runtime.ToString & If(Pref.intruntime, "", " min")
            Catch
            End Try
        End If
        root.AppendChild(doc, "runtime", musicvidtosave.fullmoviebody.runtime)
        root.AppendChild(doc, "plot", musicvidtosave.fullmoviebody.plot)
        root.AppendChild(doc, "studio", musicvidtosave.fullmoviebody.studio)
        root.AppendChild(doc, "createdate", SetCreateDate(musicvidtosave.fileinfo.createdate))
        doc.AppendChild(root)
        
        Try
            SaveXMLDoc(doc, filenameandpath)
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Load MusicVideo nfo as xml document to FullMovieDetails
    ''' </summary>
    ''' <param name="filepath">Full path and filename of nfo</param>
    ''' <returns>FullMovieDetails</returns>
    Public Shared Function MVloadNfo(ByVal filePath)
        Dim NewMusicVideo As New FullMovieDetails 
        NewMusicVideo.fileinfo.fullPathAndFilename = filePath
        Dim document As New XmlDocument
        Using tmpstrm As IO.StreamReader = File.OpenText(filepath)
            document.Load(tmpstrm)
        End Using
        For Each thisresult As XmlNode In document("musicvideo")
            Select Case thisresult.Name
                Case "album"        : NewMusicVideo.fullmoviebody.album     = thisresult.InnerText
                Case "tmdbid"       : NewMusicVideo.fullmoviebody.tmdbid    = thisresult.InnerText
                Case "title"        : NewMusicVideo.fullmoviebody.title     = thisresult.InnerText
                Case "year"         : NewMusicVideo.fullmoviebody.year      = thisresult.InnerText
                Case "artist"       : NewMusicVideo.fullmoviebody.artist    = thisresult.InnerText
                Case "director"     : NewMusicVideo.fullmoviebody.director  = thisresult.InnerText
                Case "genre"        : NewMusicVideo.fullmoviebody.genre     = thisresult.InnerText
                Case "thumb"        : NewMusicVideo.fullmoviebody.thumb     = thisresult.InnerText
                Case "track"        : NewMusicVideo.fullmoviebody.track     = thisresult.InnerText
                Case "runtime"      : NewMusicVideo.fullmoviebody.runtime   = thisresult.InnerText
                Case "plot"         : NewMusicVideo.fullmoviebody.plot      = thisresult.InnerText
                Case "studio"       : NewMusicVideo.fullmoviebody.studio    = thisresult.InnerText
                Case "createdate"   : NewMusicVideo.fileinfo.createdate     = thisresult.InnerText
                Case "fileinfo"
                    Dim what As XmlNode = Nothing
                    For Each res In thisresult.ChildNodes
                        Select Case res.name
                            Case "streamdetails"
                                NewMusicVideo.filedetails = Streamdetailsload(res)
                        End Select
                    Next
            End Select
        Next

        NewMusicVideo.fileinfo.fullpathandfilename  = filepath
        NewMusicVideo.fileinfo.filename             = Path.GetFileName(filepath).Replace(".nfo", NewMusicVideo.filedetails.Video.Container.Value)
        NewMusicVideo.fileinfo.foldername           = Utilities.GetLastFolder(filepath)
        NewMusicVideo.fileinfo.posterpath           = Pref.GetPosterPath(filepath, NewMusicVideo.fileinfo.filename)
        NewMusicVideo.fileinfo.trailerpath          = ""
        NewMusicVideo.fileinfo.rootfolder           = Pref.GetRootFolder(filePath) & "\"
        NewMusicVideo.fileinfo.path                 = Path.GetDirectoryName(filepath) & "\"
        NewMusicVideo.fileinfo.basepath             = Pref.GetMovBasePath(NewMusicVideo.fileinfo.path)
        NewMusicVideo.fileinfo.fanartpath           = Pref.GetFanartPath(filepath, NewMusicVideo.fileinfo.filename)
        If NewMusicVideo.filedetails.Video.Container.Value <> "" Then
            Dim container As String = NewMusicVideo.filedetails.Video.Container.Value
            NewMusicVideo.fileinfo.filenameandpath  = filepath.Replace(".nfo", container)
        Else
            NewMusicVideo.fileinfo.filenameandpath = Utilities.GetFileName(filepath, True)
        End If
        Return NewMusicVideo
    End Function

#End Region

End Class



