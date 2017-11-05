Imports Media_Companion
Imports System.Text.RegularExpressions

Imports System.Xml
Imports System.Net
'Imports System.IO
Namespace Tasks
    Public Class ScrapeEpisodeTask
        Inherits TaskBase


        Public Property Show As TvShow
            Get
                Return GetArgumentSafe("show")
            End Get
            Set(value As TvShow)
                SetArgumentSafe("show", value)
            End Set
        End Property

        Public Property Episode As TvEpisode
            Get
                Return GetArgumentSafe("episode")
            End Get
            Set(value As TvEpisode)
                SetArgumentSafe("episode", value)
            End Set
        End Property

        Public Property VideoPath As String
            Get
                Return GetArgumentSafe("videopath")
            End Get
            Set(value As String)
                SetArgumentSafe("videopath", value)
            End Set
        End Property

        Public Overrides Sub Run()
            MyBase.Run()

            If VideoPath Is Nothing Then
                Messages.Add("No video file set")
                Me.RaiseError()
                Exit Sub
            End If

            Me.Scrape()


            Me.State = TaskState.BackgroundWorkComplete
        End Sub

        Public Property RegexList As List(Of String)
            Get
                Dim Temp As List(Of String) = GetArgumentSafe("regexlist")

                If Temp Is Nothing Then
                    Return Pref.tv_RegexScraper
                End If

                Return Temp
            End Get
            Set(value As List(Of String))
                SetArgumentSafe("regexlist", value)
            End Set
        End Property

        Public Sub Scrape()
            Dim episode As New TvEpisode

            Dim InferedSeason As String = Nothing
            Dim InferedEpisode As String = Nothing


            For Each Regexs In Me.RegexList
                Dim M As Match
                M = Regex.Match(Me.VideoPath, Regexs)
                If M.Success = True Then
                    Try
                        InferedSeason = CInt(M.Groups(1).Value.ToString)
                        InferedEpisode = CInt(M.Groups(2).Value.ToString)

                        Exit For
                    Catch
                        InferedSeason = "-1"
                        InferedEpisode = "-1"
                    End Try
                End If
            Next
            If InferedSeason Is Nothing Then InferedSeason = "-1"
            If InferedEpisode Is Nothing Then InferedEpisode = "-1"

            Me.Episode = Me.Show.GetEpisode(InferedSeason, InferedEpisode)
            If Me.Episode Is Nothing Then
                Me.Episode = New TvEpisode()
                Me.Episode.VideoFilePath = Me.VideoPath
            End If

            Me.Episode.Season.Value = InferedSeason
            Me.Episode.Episode.Value = InferedEpisode

            Me.Episode.GetFileDetails()

            '        Dim removal As String = ""

            '        'check for multiepisode files
            '        Dim M2 As Match
            '        Dim epcount As Integer = 0
            '        Dim multiepisode As Boolean = False
            '        Dim allepisodes(100) As Integer
            '        S = eps.Thumbnail.FileName
            '        eps.Thumbnail.FileName = ""
            '        Do
            '            'S = temppath '.ToLower
            '            '<tvregex>[Ss]([\d]{1,2}).?[Ee]([\d]{3})</tvregex>
            '            M2 = Regex.Match(S, "(([EeXx])([\d]{1,4}))")
            '            If M2.Success = True Then
            '                Dim skip As Boolean = False
            '                For Each epso In episodearray
            '                    If epso.Episode.Value = M2.Groups(3).Value Then skip = True
            '                Next
            '                If skip = False Then
            '                    Dim multieps As New TvEpisode
            '                    multieps.Season.Value = eps.Season.Value
            '                    multieps.Episode.Value = M2.Groups(3).Value
            '                    multieps.VideoFilePath = eps.VideoFilePath
            '                    multieps.MediaExtension = eps.MediaExtension
            '                    episodearray.Add(multieps)
            '                    allepisodes(epcount) = Convert.ToDecimal(M2.Groups(3).Value)
            '                End If
            '                Try
            '                    S = S.Substring(M2.Groups(3).Index + M2.Groups(3).Value.Length, S.Length - (M2.Groups(3).Index + M2.Groups(3).Value.Length))
            '                Catch ex As Exception
            '#If SilentErrorScream Then
            '                                            Throw ex
            '#End If
            '                End Try
            '            End If
            '            If bckgroundscanepisodes.CancellationPending Then
            '                Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
            '                Exit Sub
            '            End If
            '        Loop Until M2.Success = False
            Dim language As String = ""
            Dim sortorder As String = ""
            Dim tvdbid As String = ""
            Dim imdbid As String = ""
            Dim actorsource As String = ""
            Dim realshowpath As String = ""

            'savepath = episodearray(0).VideoFilePath
            'Dim EpisodeName As String = ""
            'For Each Shows In Cache.TvCache.Shows
            '    'If bckgroundscanepisodes.CancellationPending Then
            '    '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
            '    '    Exit Sub
            '    'End If
            '    If episodearray(0).VideoFilePath.IndexOf(Shows.NfoFilePath.Replace("tvshow.nfo", "")) <> -1 Then
            '        language = Shows.Language.Value
            '        sortorder = Shows.SortOrder.Value
            '        tvdbid = Shows.TvdbId
            '        tempTVDBiD = Shows.TvdbId
            '        imdbid = Shows.ImdbId.Value
            '        showtitle = Shows.Title.Value
            '        EpisodeName = Shows.Title.Value
            '        realshowpath = Shows.NfoFilePath
            '        actorsource = Shows.EpisodeActorSource.Value
            '    End If
            'Next
            'If episodearray.Count > 1 Then
            '    Pref.tvScraperLog &= "Multipart episode found: " & vbCrLf
            '    Pref.tvScraperLog &= "Season: " & episodearray(0).Season.Value & " Episodes, "
            '    For Each ep In episodearray
            '        Pref.tvScraperLog &= ep.Episode.Value & ", "
            '    Next
            '    Pref.tvScraperLog &= vbCrLf
            'End If
            'Preferences.tvScraperLog &= "Looking up scraper options from tvshow.nfo" & vbCrLf


            'If bckgroundscanepisodes.CancellationPending Then
            '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
            '    Exit Sub
            'End If
            'If Me.Episode.Season.Value.Length > 0 Or Me.Episode.Season.Value.IndexOf("0") = 0 Then
            '    Do Until Me.Episode.Season.Value.IndexOf("0") <> 0 Or Me.Episode.Season.Value.Length = 1
            '        Me.Episode.Season.Value = Me.Episode.Season.Value.Substring(1, Me.Episode.Season.Value.Length - 1)
            '    Loop
            '    If Me.Episode.Episode.Value = "00" Then
            '        Me.Episode.Episode.Value = "0"
            '    End If
            '    If Me.Episode.Episode.Value <> "0" Then
            '        Do Until Me.Episode.Episode.Value.IndexOf("0") <> 0
            '            Me.Episode.Episode.Value = Me.Episode.Episode.Value.Substring(1, Me.Episode.Episode.Value.Length - 1)
            '        Loop
            '    End If
            'End If
            'Dim episodescraper As New TVDB.tvdbscraper 'commented because of removed TVDB.dll
            Dim episodescraper As New TVDBScrapper
            If sortorder = "" Then sortorder = "default"
            Dim tempsortorder As String = sortorder
            If language = "" Then language = "en"
            If actorsource = "" Then actorsource = "tvdb"

            Dim tempstring As String
            Dim progresstext As String = ""
            tvdbid = Me.Show.TvdbId.Value
            Pref.tvScraperLog &= "Using Settings: TVdbID: " & tvdbid & " SortOrder: " & sortorder & " Language: " & language & " Actor Source: " & actorsource & vbCrLf
            If tvdbid <> "" Then
                progresstext &= " - Scraping..."
                'bckgroundscanepisodes.ReportProgress(progress, progresstext)
                Dim episodeurl As String = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & Me.Episode.Season.Value & "/" & Me.Episode.Episode.Value & "/" & language & ".xml"
                'Preferences.tvScraperLog &= "Trying Episode URL: " & episodeurl & vbCrLf
                If Not Utilities.UrlIsValid(episodeurl) Then
                    If sortorder.ToLower = "dvd" Then
                        tempsortorder = "default"
                        Pref.tvScraperLog &= "WARNING: This episode could not be found on TVDB using DVD sort order" & vbCrLf
                        Pref.tvScraperLog &= "Attempting to find using default sort order" & vbCrLf
                        episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/default/" & Me.Episode.Season.Value & "/" & Me.Episode.Episode.Value & "/" & language & ".xml"
                        Pref.tvScraperLog &= "Now Trying Episode URL: " & episodeurl & vbCrLf
                    End If
                End If


                '#####################################################################
                If Utilities.UrlIsValid(episodeurl) Then
                    Dim TvdbEpisodeData As String = Utilities.DownloadTextFiles(episodeurl)
                    Dim TvdbEpisode As New Tvdb.ShowData
                    TvdbEpisode.LoadXml(TvdbEpisodeData)

                    If TvdbEpisode.Episodes.Item(0) IsNot Nothing Then
                        Me.Episode.AbsorbTvdbEpisode(TvdbEpisode.Episodes.Item(0))
                        Me.Episode.Thumbnail.DownloadFile()
                    End If

                    '########################################################################

                    'If Utilities.UrlIsValid(episodeurl) Then
                    '    'If Pref.tvshow_useXBMC_Scraper = True Then
                    '    '    Dim FinalResult As String = ""
                    '    '    episodearray = XBMCScrape_TVShow_EpisodeDetails(tvdbid, tempsortorder, episodearray, language)
                    '    '    If episodearray.Count >= 1 Then
                    '    '        For x As Integer = 0 To episodearray.Count - 1
                    '    '            Pref.tvScraperLog &= "Scraping body of episode: " & episodearray(x).Episode.Value & " - OK" & vbCrLf
                    '    '        Next
                    '    '        scrapedok = True
                    '    '    Else
                    '    '        Pref.tvScraperLog &= "WARNING: Could not locate this episode on TVDB, or TVDB may be unavailable" & vbCrLf
                    '    '        scrapedok = False
                    '    '    End If
                    '    '    Exit For
                    '    'End If

                    '    'Dim tempepisode As String = episodescraper.getepisode(tvdbid, tempsortorder, singleepisode.Season.value, singleepisode.episodeno, language)
                    '    Dim tempepisode As String = ep_Get(tvdbid, tempsortorder, Me.Episode.Season.Value, Me.Episode.Episode.Value, language)
                    '    Dim ScrapedOk As Boolean = True

                    '    '                            Exit For
                    '    If tempepisode = Nothing Then
                    '        ScrapedOk = False
                    '        Pref.tvScraperLog &= "WARNING: This episode could not be found on TVDB" & vbCrLf
                    '    End If
                    '    If ScrapedOk = True Then
                    '        progresstext &= " OK."
                    '        'bckgroundscanepisodes.ReportProgress(progress, progresstext)
                    '        Dim scrapedepisode As New XmlDocument

                    '        Pref.tvScraperLog &= "Scraping body of episode: " & Me.Episode.Episode.Value & vbCrLf

                    '        scrapedepisode.LoadXml(tempepisode)

                    '        For Each thisresult As XmlNode In scrapedepisode("episodedetails")
                    '            Select Case thisresult.Name
                    '                Case "title"
                    '                    Me.Episode.Title.Value = thisresult.InnerText
                    '                Case "premiered"
                    '                    Me.Episode.Aired.Value = thisresult.InnerText
                    '                Case "plot"
                    '                    Me.Episode.Plot.Value = thisresult.InnerText
                    '                Case "director"
                    '                    Dim newstring As String
                    '                    newstring = thisresult.InnerText
                    '                    newstring = newstring.TrimEnd("|")
                    '                    newstring = newstring.TrimStart("|")
                    '                    newstring = newstring.Replace("|", " / ")
                    '                    Me.Episode.Director.Value = newstring
                    '                Case "credits"
                    '                    Dim newstring As String
                    '                    newstring = thisresult.InnerText
                    '                    newstring = newstring.TrimEnd("|")
                    '                    newstring = newstring.TrimStart("|")
                    '                    newstring = newstring.Replace("|", " / ")
                    '                    Me.Episode.Credits.Value = newstring
                    '                Case "rating"
                    '                    Me.Episode.Rating.Value = thisresult.InnerText
                    '                Case "thumb"
                    '                    Me.Episode.Thumbnail.Url = thisresult.InnerText
                    '                Case "actor"
                    '                    For Each actorl As XmlNode In thisresult.ChildNodes
                    '                        Select Case actorl.Name
                    '                            Case "name"
                    '                                Dim newactor As New Actor
                    '                                newactor.actorname = actorl.InnerText

                    '                                Me.Episode.ListActors.Add(newactor)
                    '                        End Select
                    '                    Next
                    '            End Select
                    '        Next
                    '        Me.Episode.PlayCount.Value = "0"

                    'progresstext &= " " & singleepisode.Title.Value
                    'bckgroundscanepisodes.ReportProgress(progress, progresstext)
                    If True Then
                        If actorsource = "imdb" Then
                            'Preferences.tvScraperLog &= "Scraping actors from IMDB" & vbCrLf
                            'progresstext &= " Actors..."
                            'bckgroundscanepisodes.ReportProgress(progress, progresstext)
                            Dim url As String
                            url = "http://www.imdb.com/title/" & imdbid & "/episodes"
                            Dim tvfblinecount As Integer = 0
                            Dim tvdbwebsource(10000)
                            tvfblinecount = 0
                            'If bckgroundscanepisodes.CancellationPending Then
                            '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
                            '    Exit Sub
                            'End If

                            Dim wrGETURL As WebRequest
                            wrGETURL = WebRequest.Create(url)
                            wrGETURL.Proxy = Utilities.MyProxy
                            'Dim myProxy As New WebProxy("myproxy", 80)
                            'myProxy.BypassProxyOnLocal = True
                            Dim objStream As IO.Stream
                            objStream = wrGETURL.GetResponse.GetResponseStream()
                            Dim objReader As New IO.StreamReader(objStream)
                            Dim tvdbsLine As String = ""
                            tvfblinecount = 0

                            Do While Not tvdbsLine = ""   'is Nothing Changed by SK 18/5/2011     to avoid System.NullReferenceException was with new install & 1 new tvshow scraped & tried to serach for new episodes with 1 in the folder
                                tvfblinecount += 1
                                tvdbsLine = objReader.ReadLine
                                If Not tvdbsLine = "" Then   '   Is Nothing Then Changed by SK 18/5/2011
                                    tvdbwebsource(tvfblinecount) = tvdbsLine
                                End If
                                'If bckgroundscanepisodes.CancellationPending Then
                                '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
                                '    Exit Sub
                                'End If
                            Loop
                            objReader.Close()
                            objStream.Close()
                            tvfblinecount -= 1





                            'If tvfblinecount <> 0 Then
                            '    Dim tvtempstring As String
                            '    tvtempstring = "Season " & Me.Episode.Season.Value & ", Episode " & Me.Episode.Episode.Value & ":"
                            '    For g = 1 To tvfblinecount
                            '        If tvdbwebsource(g).indexof(tvtempstring) <> -1 Then
                            '            Dim tvtempint As Integer
                            '            tvtempint = tvdbwebsource(g).indexof("<a href=""/title/")
                            '            If tvtempint <> -1 Then
                            '                tvtempstring = tvdbwebsource(g).substring(tvtempint + 16, 9)
                            '                '            Dim scraperfunction As New imdb.Classimdbscraper ' add to comment this one because of changes i made to the Class "Scraper" (ClassimdbScraper)
                            '                Dim scraperfunction As New Classimdb
                            '                Dim actorlist As String = ""
                            '                actorlist = scraperfunction.getimdbactors(Preferences.imdbmirror, tvtempstring, , Pref.maxactors)
                            '                Dim tempactorlist As New List(Of str_MovieActors)
                            '                Dim thumbstring As New XmlDocument


                            '                thumbstring.LoadXml(actorlist)

                            '                Dim countactors As Integer = 0
                            '                For Each thisresult As XmlNode In thumbstring("actorlist")
                            '                    'If bckgroundscanepisodes.CancellationPending Then
                            '                    '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
                            '                    '    Exit Sub
                            '                    'End If
                            '                    Select Case thisresult.Name
                            '                        Case "actor"
                            '                            If countactors >= Pref.maxactors Then
                            '                                Exit For
                            '                            End If
                            '                            countactors += 1
                            '                            Dim newactor As New str_MovieActors(SetDefaults)

                            '                            For Each detail As XmlNode In thisresult.ChildNodes
                            '                                Select Case detail.Name
                            '                                    Case "name"
                            '                                        newactor.actorname = detail.InnerText
                            '                                    Case "role"
                            '                                        newactor.actorrole = detail.InnerText
                            '                                    Case "thumb"
                            '                                        newactor.actorthumb = detail.InnerText
                            '                                    Case "actorid"
                            '                                        If newactor.actorthumb <> Nothing Then
                            '                                            If Pref.actorseasy = True And detail.InnerText <> "" Then
                            '                                                Dim workingpath As String = episodearray(0).VideoFilePath.Replace(Path.GetFileName(episodearray(0).VideoFilePath), "")
                            '                                                workingpath = workingpath & ".actors\"
                            '                                                Dim hg As New DirectoryInfo(workingpath)
                            '                                                Dim destsorted As Boolean = False
                            '                                                If Not hg.Exists Then

                            '                                                    Directory.CreateDirectory(workingpath)
                            '                                                    destsorted = True

                            '                                                Else
                            '                                                    destsorted = True
                            '                                                End If
                            '                                                If destsorted = True Then
                            '                                                    Dim filename As String = newactor.actorname.Replace(" ", "_")
                            '                                                    filename = filename & ".tbn"
                            '                                                    Dim tvshowactorpath As String = realshowpath
                            '                                                    tvshowactorpath = tvshowactorpath.Replace(Path.GetFileName(tvshowactorpath), "")
                            '                                                    tvshowactorpath = Path.Combine(tvshowactorpath, ".actors\")
                            '                                                    tvshowactorpath = Path.Combine(tvshowactorpath, filename)
                            '                                                    filename = Path.Combine(workingpath, filename)
                            '                                                    If File.Exists(tvshowactorpath) Then

                            '                                                        File.Copy(tvshowactorpath, filename, True)

                            '                                                    End If
                            '                                                    If Not File.Exists(filename) Then
                            '                                                        Dim buffer(4000000) As Byte
                            '                                                        Dim size As Integer = 0
                            '                                                        Dim bytesRead As Integer = 0
                            '                                                        Dim thumburl As String = newactor.actorthumb
                            '                                                        Dim req As HttpWebRequest = WebRequest.Create(thumburl)
                            '                                                        Dim res As HttpWebResponse = req.GetResponse()
                            '                                                        Dim contents As Stream = res.GetResponseStream()
                            '                                                        Dim bytesToRead As Integer = CInt(buffer.Length)
                            '                                                        While bytesToRead > 0
                            '                                                            size = contents.Read(buffer, bytesRead, bytesToRead)
                            '                                                            If size = 0 Then Exit While
                            '                                                            bytesToRead -= size
                            '                                                            bytesRead += size
                            '                                                        End While

                            '                                                        Dim fstrm As New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write)
                            '                                                        fstrm.Write(buffer, 0, bytesRead)
                            '                                                        contents.Close()
                            '                                                        fstrm.Close()
                            '                                                    End If
                            '                                                End If
                            '                                            End If
                            '                                            If Pref.actorsave = True And detail.InnerText <> "" And Pref.actorseasy = False Then
                            '                                                Dim workingpath As String = ""
                            '                                                Dim networkpath As String = Pref.actorsavepath

                            '                                                tempstring = networkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2)
                            '                                                Dim hg As New DirectoryInfo(tempstring)
                            '                                                If Not hg.Exists Then
                            '                                                    Directory.CreateDirectory(tempstring)
                            '                                                End If
                            '                                                workingpath = networkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "\" & detail.InnerText & ".jpg"
                            '                                                If Not File.Exists(workingpath) Then
                            '                                                    Dim buffer(4000000) As Byte
                            '                                                    Dim size As Integer = 0
                            '                                                    Dim bytesRead As Integer = 0
                            '                                                    Dim thumburl As String = newactor.actorthumb
                            '                                                    Dim req As HttpWebRequest = WebRequest.Create(thumburl)
                            '                                                    Dim res As HttpWebResponse = req.GetResponse()
                            '                                                    Dim contents As Stream = res.GetResponseStream()
                            '                                                    Dim bytesToRead As Integer = CInt(buffer.Length)
                            '                                                    While bytesToRead > 0
                            '                                                        size = contents.Read(buffer, bytesRead, bytesToRead)
                            '                                                        If size = 0 Then Exit While
                            '                                                        bytesToRead -= size
                            '                                                        bytesRead += size
                            '                                                    End While
                            '                                                    Dim fstrm As New FileStream(workingpath, FileMode.OpenOrCreate, FileAccess.Write)
                            '                                                    fstrm.Write(buffer, 0, bytesRead)
                            '                                                    contents.Close()
                            '                                                    fstrm.Close()
                            '                                                End If
                            '                                                newactor.actorthumb = Path.Combine(Preferences.actornetworkpath, detail.InnerText.Substring(detail.InnerText.Length - 2, 2))
                            '                                                If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                            '                                                    newactor.actorthumb = Pref.actornetworkpath & "/" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "/" & detail.InnerText & ".jpg"
                            '                                                Else
                            '                                                    newactor.actorthumb = Pref.actornetworkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "\" & detail.InnerText & ".jpg"
                            '                                                End If

                            '                                            End If
                            '                                        End If
                            '                                End Select
                            '                                'If bckgroundscanepisodes.CancellationPending Then
                            '                                '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
                            '                                '    Exit Sub
                            '                                'End If
                            '                            Next
                            '                            tempactorlist.Add(newactor)
                            '                    End Select
                            '                    'If bckgroundscanepisodes.CancellationPending Then
                            '                    '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
                            '                    '    Exit Sub
                            '                    'End If
                            '                Next





                            '                If tempactorlist.Count > 0 Then
                            '                    'Preferences.tvScraperLog &= "Actors scraped from IMDB OK" & vbCrLf
                            '                    'progresstext &= " OK."
                            '                    'bckgroundscanepisodes.ReportProgress(progress, progresstext)
                            '                    While tempactorlist.Count > Pref.maxactors
                            '                        tempactorlist.RemoveAt(tempactorlist.Count - 1)
                            '                    End While
                            '                    Me.Episode.ListActors.Clear()
                            '                    For Each actor In tempactorlist
                            '                        singleepisode.ListActors.Add(actor)
                            '                    Next
                            '                    tempactorlist.Clear()
                            '                Else
                            '                    Pref.tvScraperLog &= "WARNING: Actors not scraped from IMDB, reverting to TVDB actorlist" & vbCrLf
                            '                End If

                            '                Exit For
                            '            End If
                            '        End If
                            '        'If bckgroundscanepisodes.CancellationPending Then
                            '        '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
                            '        '    Exit Sub
                            '        'End If
                            '    Next

                            'Else
                            '    'tvscraperlog = tvscraperlog & "Unable To Get Actors From IMDB, Defaulting To TVDB" & vbCrLf
                            'End If
                        End If

                    End If


                    If Pref.enablehdtags = True Then
                        'progresstext &= " HD Tags..."
                        'bckgroundscanepisodes.ReportProgress(progress, progresstext)

                        If Not Me.Episode.StreamDetails.Video.DurationInSeconds.Value Is Nothing Then

                            '1h 24mn 48s 546ms
                            Dim tempint As Long
                            Dim hours As Integer
                            Dim minutes As Integer
                            tempstring = Me.Episode.StreamDetails.Video.DurationInSeconds.Value
                            tempint = tempstring.IndexOf("h")
                            If tempint <> -1 Then
                                hours = Convert.ToInt32(tempstring.Substring(0, tempint))
                                tempstring = tempstring.Substring(tempint + 1, tempstring.Length - (tempint + 1))
                                tempstring = Trim(tempstring)
                            End If
                            tempint = tempstring.IndexOf("mn")
                            If tempint <> -1 Then
                                minutes = Convert.ToInt32(tempstring.Substring(0, tempint))
                            End If
                            If hours <> 0 Then
                                minutes += hours * 60
                            End If
                            minutes = minutes + hours
                            Me.Episode.Runtime.Value = minutes.ToString & " min"
                            progresstext &= " OK."
                            'bckgroundscanepisodes.ReportProgress(progress, progresstext)
                        End If

                    End If
                Else
                    Pref.tvScraperLog &= "WARNING: Could not locate this episode on TVDB, or TVDB may be unavailable" & vbCrLf
                End If
            Else
                Pref.tvScraperLog &= "WARNING: No TVDB ID is available for this show, please scrape the show using the ""TV Show Selector"" TAB" & vbCrLf
            End If










            'If savepath <> "" And scrapedok = True Then
            '    If bckgroundscanepisodes.CancellationPending Then
            '        Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
            '        Exit Sub
            '    End If
            '    Dim newnamepath As String = ""



            '    '' Commented out the lines below so that the episodes are renamed irrelavent of scraper (MC or XBMC scraper) - not sure of their purpose. 
            '    'If Pref.tvshow_useXBMC_Scraper = True Then
            '    '    newnamepath = savepath
            '    'Else
            '    newnamepath = ep_add(episodearray, savepath, showtitle)
            '    ''9999999                                                               'This was already commented out, it must be a note of some sort.
            '    For Each ep In episodearray
            '        ep.VideoFilePath = newnamepath
            '    Next
            '    'End If
            '    'bckgroundscanepisodes.ReportProgress(9999999, episodearray)
            '    'If bckgroundscanepisodes.CancellationPending Then
            '    '    Pref.tvScraperLog &= vbCrLf & "Operation Cancelled by user" & vbCrLf
            '    '    Exit Sub
            '    'End If
            '    For Each Shows In Cache.TvCache.Shows
            '        If episodearray(0).VideoFilePath.IndexOf(Shows.NfoFilePath.Replace("\tvshow.nfo", "")) <> -1 Then
            '            'workingtvshow = nfofunction.loadfulltnshownfo(Shows.fullpath)
            '            For Each ept In episodearray
            '                For j = Shows.MissingEpisodes.Count - 1 To 0 Step -1
            '                    If Shows.MissingEpisodes(j).Title = ept.Title Then
            '                        Shows.MissingEpisodes.RemoveAt(j)
            '                        Exit For
            '                    End If
            '                Next
            '            Next
            '            For Each ep In episodearray
            '                Dim newwp As New TvEpisode
            '                newwp.Episode.Value = ep.Episode.Value
            '                newwp.VideoFilePath = newnamepath
            '                newwp.PlayCount.Value = "0"
            '                newwp.Rating.Value = ep.Rating.Value
            '                newwp.Season.Value = ep.Season.Value
            '                newwp.Title = ep.Title
            '                newwp.ShowObj = Shows
            '                bckgroundscanepisodes.ReportProgress(1, newwp)
            '            Next
            '        End If
            '    Next
            'End If
            'Preferences.tvScraperLog &= vbCrLf
            '        Next

            'bckgroundscanepisodes.ReportProgress(0, progresstext)
        End Sub

        Public Function ep_Get(ByVal tvdbid As String, ByVal sortorder As String, ByVal seriesno As String, ByVal episodeno As String, ByVal language As String)
            Dim ErrorCounter As Integer = 0
            Dim episodestring As String = ""
            Dim episodeurl2 As String = ""
            Dim xmlfile As String
            While ErrorCounter <= 10
                Try

                    episodestring = ""
                    episodeurl2 = ""
                    If language.ToLower.IndexOf(".xml") = -1 Then
                        language = language & ".xml"
                    End If
                    episodeurl2 = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seriesno & "/" & episodeno & "/" & language
                    Dim myProxy As New WebProxy("myproxy", 80)
                    myProxy.BypassProxyOnLocal = True

                    xmlfile = Utilities.DownloadTextFiles(episodeurl2)
                    'If CheckBoxDebugShowTVDBReturnedXML.Checked = True Then MsgBox(xmlfile, MsgBoxStyle.OkOnly, "FORM1 getepisode - TVDB returned.....")

                    Dim episode As New XmlDocument

                    episode.LoadXml(xmlfile)

                    episodestring = "<episodedetails>"
                    episodestring = episodestring & "<url>" & episodeurl2 & "</url>"
                    Dim mirrorslist As New XmlDocument
                    'Try
                    mirrorslist.LoadXml(xmlfile)
                    For Each thisresult As XmlNode In mirrorslist("Data")

                        Select Case thisresult.Name
                            Case "Episode"
                                For Each mirrorselection As XmlNode In thisresult.ChildNodes
                                    Select Case mirrorselection.Name
                                        Case "EpisodeName"
                                            episodestring = episodestring & "<title>" & mirrorselection.InnerXml & "</title>"
                                        Case "FirstAired"
                                            episodestring = episodestring & "<premiered>" & mirrorselection.InnerXml & "</premiered>"
                                        Case "GuestStars"
                                            Dim tempstring As String = mirrorselection.InnerXml
                                            Try
                                                tempstring = tempstring.TrimStart("|")
                                                tempstring = tempstring.TrimEnd("|")
                                                Dim tvtempstring2 As String
                                                Dim tvtempint As Integer
                                                Dim a() As String
                                                Dim j As Integer
                                                tvtempstring2 = ""
                                                a = tempstring.Split("|")
                                                tvtempint = a.GetUpperBound(0)
                                                tvtempstring2 = a(0)
                                                If tvtempint >= 0 Then
                                                    For j = 0 To tvtempint
                                                        Try
                                                            episodestring = episodestring & "<actor>" & "<name>" & a(j) & "</name></actor>"
                                                        Catch ex As Exception
#If SilentErrorScream Then
                                                        Throw ex
#End If
                                                        End Try
                                                    Next
                                                End If
                                            Catch ex As Exception
#If SilentErrorScream Then
                                            Throw ex
#End If
                                            End Try
                                        Case "Director"
                                            Dim tempstring As String = mirrorselection.InnerXml
                                            tempstring = tempstring.TrimStart("|")
                                            tempstring = tempstring.TrimEnd("|")
                                            episodestring = episodestring & "<director>" & tempstring & "</director>"
                                        Case "Writer"
                                            Dim tempstring As String = mirrorselection.InnerXml
                                            tempstring = tempstring.TrimStart("|")
                                            tempstring = tempstring.TrimEnd("|")
                                            episodestring = episodestring & "<credits>" & tempstring & "</credits>"
                                        Case "Overview"
                                            episodestring = episodestring & "<plot>" & mirrorselection.InnerXml & "</plot>"
                                        Case "Rating"
                                            episodestring = episodestring & "<rating>" & mirrorselection.InnerXml & "</rating>"
                                        Case "filename"
                                            episodestring = episodestring & "<thumb>http://www.thetvdb.com/banners/" & mirrorselection.InnerXml & "</thumb>"
                                    End Select
                                Next
                        End Select
                    Next
                    episodestring = episodestring & "</episodedetails>"
                    Return episodestring
                    'Catch ex As Exception
                    '    Return "ERROR - <url>" & episodeurl & "</url>"
                    'Finally
                    '    Monitor.Exit(Me)
                    'End Try
                Catch ex As Exception
                    If ErrorCounter <= 10 Then
                        ErrorCounter += 1
                    Else
                        episodestring = Nothing
                    End If
                End Try

            End While
            Return "Error"
        End Function

        Public Overrides Sub FinishWork()
            Me.Show.AddEpisode(Me.Episode)
            Me.Episode.SeasonObj.UpdateTreenode()
            Me.Episode.UpdateTreenode()

            Me.State = TaskState.Completed
        End Sub

        Public Overrides ReadOnly Property FriendlyTaskName As String
            Get
                If Episode IsNot Nothing AndAlso Show IsNot Nothing Then
                    Return Me.State & " - Scrapping Episode " & Me.Episode.Title.Value & " of " & Me.Show.Title.Value
                Else
                    Return Me.State & " - Scrapping Unkown Episode"
                End If
            End Get
        End Property
    End Class
End Namespace