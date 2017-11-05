Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports Media_Companion
Imports System.Text.RegularExpressions
Imports Media_Companion.Tvdb
Imports System.Drawing
Imports System.Xml
Imports System.Windows.Forms


Namespace Tasks
    Public Class ScrapeShowTask
        Inherits TaskBase

        Public Overrides ReadOnly Property FriendlyTaskName As String
            Get
                If Me.Arguments.ContainsKey("show") Then
                    If Me.Arguments("show") IsNot Nothing Then
                        Dim TempShow As TvShow = Me.Arguments("show")
                        Return Me.State & " - Scraping " & TempShow.Title.Value
                    End If
                End If

                Return Me.State & " - Scraping some show"
            End Get
        End Property


        Public Sub New(ByRef Show As TvShow)
            Me.Id = Guid.NewGuid

            If Show Is Nothing Then
                Me.Messages.Add(New ArgumentException("No show selected"))
                Me.State = TaskState.CriticalFault
                RaiseError() 'Can event handlers register before a constructor is completed?
            Else
                Me.Show = Show
            End If


        End Sub



        Public Overrides Sub Run()
            MyBase.Run()
            If Me.State = TaskState.Completed Then Exit Sub

            For Each Item In Dependancies
                If Not Item.State = TaskState.Completed Then
                    Me.State = TaskState.WaitingForDependancies
                    Exit Sub
                End If
            Next


            Me.Show.Seasons.Clear()
            Me.Show.clearActors()

            Me.Scrape()


            Me.RaiseCompleted()
            Me.State = TaskState.Completed
        End Sub

        Public Property Show As TvShow
            Get
                If Me.Arguments.ContainsKey("show") Then
                    Return Me.Arguments("show")
                Else
                    Me.Messages.Add(New ArgumentException("No show selected"))
                    Return Nothing
                End If
            End Get
            Set(value As TvShow)
                If Me.Arguments.ContainsKey("show") Then
                    Me.Arguments("show") = value
                Else
                    Me.Arguments.Add("show", value)
                End If
            End Set
        End Property

        Public Property Forced As Boolean
            Get
                Return True
                If Me.Arguments.ContainsKey("forced") Then
                    Return Me.Arguments("forced")
                Else
                    Return False
                End If
            End Get
            Set(value As Boolean)
                If Me.Arguments.ContainsKey("forced") Then
                    Me.Arguments("forced") = value
                Else
                    Me.Arguments.Add("forced", value)
                End If
            End Set
        End Property

        Private Sub Scrape()
            ' Should be identical to Form1.Tv.vb:bckgrnd_tvshowscraper_DoWork()

            If String.IsNullOrEmpty(Me.Show.NfoFilePath) Then
                Me.Show.NfoFilePath = Path.Combine(Me.Show.FolderPath, "tvshow.nfo")
            End If



            If Me.Show.FileContainsReadableXml And Not Me.Forced Then
                Me.Show.Load()
                Exit Sub
            End If

            If Me.Show.TvdbId.Value IsNot Nothing Then
                'Resolve show name from folder
                Dim FolderName As String = Utilities.GetLastFolder(Me.Show.FolderPath)
                Dim M As Match
                M = Regex.Match(FolderName, "\s*[\(\{\[](?<date>[\d]{4})[\)\}\]]")
                If M.Success = True Then
                    FolderName = String.Format("{0} ({1})", FolderName.Substring(0, M.Index), M.Groups("date").Value)
                End If
                Me.Show.Title.Value = FolderName

                Me.Show.GetPossibleShows()
                'Dim tvshowid As String
                If Me.Show.PossibleShowList IsNot Nothing Then
                    If Me.Show.PossibleShowList.Count = 0 Then
                        Me.Show.State = Media_Companion.ShowState.Unverified
                        Messages.Add("No matching shows found")

                        Me.RaiseError()
                        Exit Sub
                    ElseIf Me.Show.PossibleShowList.Count = 1 Then
                        Me.Show.State = Media_Companion.ShowState.Open

                        Me.Show.TvdbId.Value = Me.Show.PossibleShowList.Item(0).Id.Value
                        Me.Show.Title.Value = Me.Show.PossibleShowList.Item(0).SeriesName.Value
                        Me.Show.Language.Value = Me.Show.PossibleShowList.Item(0).Language.Value

                        Messages.Add("One possible match found and assumed correct")
                    Else
                        Me.Show.State = Media_Companion.ShowState.Unverified

                        Me.Show.TvdbId.Value = Me.Show.PossibleShowList.Item(0).Id.Value

                        Messages.Add("Multiple possible matches found, first returned used, but show remains unverified")
                        Options.Clear()
                        For Each Item In Show.PossibleShowList
                            Options.Add(New TaskOption With {.Name = "Show", .Description = Item.SeriesName.Value, .Value = Item.Id.Value})
                        Next

                    End If
                Else
                    Me.Show.State = Media_Companion.ShowState.Error
                    Messages.Add("Error downloading possible shows")
                    Me.RaiseError()
                    Exit Sub
                End If
            End If


            Dim Files As List(Of String) = Utilities.EnumerateFiles(Me.Show.FolderPath, 4)

            For Each Item In Files
                Dim ScrapeEpTask As New ScrapeEpisodeTask()
                ScrapeEpTask.Show = Me.Show
                ScrapeEpTask.VideoPath = Item
                If Utilities.VideoExtensions.Contains(Path.GetExtension(Item)) Then
                    Common.Tasks.Add(ScrapeEpTask)
                End If
            Next

            If Me.Show.TvdbId.Value IsNot Nothing Then
                'tvshow found
                Me.Show.NfoFilePath = Path.Combine(Me.Show.FolderPath, "tvshow.nfo")

                Dim tvdbstuff As New TVDBScrapper

                Dim posterurl As String = ""
                Dim tempstring As String = ""
                Dim TempLanguage As String = Pref.TvdbLanguageCode
                If String.IsNullOrEmpty(TempLanguage) Then TempLanguage = "en"

                Me.Show.TvdbData = tvdbstuff.GetShow(Me.Show.TvdbId.Value, TempLanguage, True)

                Me.Show.AbsorbTvdbSeries(Me.Show.TvdbData.Series(0))

                Dim DownloadTvdbActors As Boolean = (Pref.TvdbActorScrape = 0) OrElse (Pref.TvdbActorScrape = 3) OrElse Me.Show.ImdbId.Value Is Nothing
                Dim DownloadImdbActors As Boolean = (Pref.TvdbActorScrape = 1) OrElse (Pref.TvdbActorScrape = 3) AndAlso Me.Show.ImdbId.Value IsNot Nothing
                Dim DownloadActors As Boolean = Pref.actorseasy

                Dim TvdbActors As Tvdb.Actors = tvdbstuff.GetActors(Me.Show.TvdbId.Value, TempLanguage)
                For Each Act As Tvdb.Actor In TvdbActors.Items
                    If Me.Show.ListActors.Count >= Pref.maxactors Then
                        Exit For
                    End If

                    Dim NewAct As New Media_Companion.Actor
                    NewAct.actorid = Act.Id.Value
                    NewAct.actorname = Act.Name.Value
                    NewAct.actorrole = Act.Role.Value
                    NewAct.actorthumb = Act.Image.Value

                    If DownloadTvdbActors Then
                        Dim id As String = ""

                        If Not String.IsNullOrEmpty(NewAct.actorthumb) Then
                            If NewAct.actorthumb <> "" And Pref.actorseasy = True Then
                                If Me.Show.TvShowActorSource.Value <> "imdb" Or Me.Show.ImdbId = Nothing Then
                                    Dim workingpath As String = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "")
                                    workingpath = workingpath & ".actors\"

                                    Utilities.EnsureFolderExists(workingpath)
                                    '**Commented out the following as fairly certain Utilities.EnsureFolderExists() replaces this - Huey
                                    'Dim hg As New DirectoryInfo(workingpath)
                                    'Dim destsorted As Boolean = False
                                    'If Not hg.Exists Then

                                    '    IDirectory.CreateDirectory(workingpath)
                                    '    destsorted = True

                                    'Else
                                    '    destsorted = True
                                    'End If
                                    'If destsorted = True Then
                                    Dim filename As String = Utilities.cleanFilenameIllegalChars(NewAct.actorname)
                                    filename = filename.Replace(" ", "_")
                                    filename = filename & ".tbn"
                                    filename = Path.Combine(workingpath, filename)

                                    Utilities.DownloadFile("http://thetvdb.com/banners/_cache/" & NewAct.actorthumb, filename)
                                    'End If
                                End If
                            End If
                            If Pref.actorsave = True And id <> "" And Pref.actorseasy = False Then
                                Dim workingpath As String = ""
                                Dim networkpath As String = Pref.actorsavepath

                                tempstring = networkpath & "\" & id.Substring(id.Length - 2, 2)
                                Dim hg As New DirectoryInfo(tempstring)
                                If Not hg.Exists Then
                                    Directory.CreateDirectory(tempstring)
                                End If
                                workingpath = networkpath & "\" & id.Substring(id.Length - 2, 2) & "\tv" & id & ".jpg"
                                If Not File.Exists(workingpath) Then
                                    Utilities.DownloadFile("http://thetvdb.com/banners/_cache/" & NewAct.actorthumb, workingpath)
                                End If
                                NewAct.actorthumb = Path.Combine(Pref.actornetworkpath, id.Substring(id.Length - 2, 2))
                                If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                                    NewAct.actorthumb = Path.Combine(Pref.actornetworkpath, id.Substring(id.Length - 2, 2) & "/tv" & id & ".jpg")
                                Else
                                    NewAct.actorthumb = Path.Combine(Pref.actornetworkpath, id.Substring(id.Length - 2, 2) & "\tv" & id & ".jpg")
                                End If


                            End If
                        End If
                        Dim exists As Boolean = False
                        For Each actors In Me.Show.ListActors
                            If actors.actorname = NewAct.actorname And actors.actorrole = NewAct.actorrole Then
                                exists = True
                                Exit For
                            End If
                        Next
                        If exists = False Then
                            Me.Show.ListActors.Add(NewAct)
                        End If
                    End If
                Next






                If Pref.TvdbActorScrape = 1 Or Pref.TvdbActorScrape = 2 And Me.Show.ImdbId <> Nothing Then
                    Dim imdbscraper As New ImdbScrapper.ImdbScrapper
                    Dim actorlist As String
                    Dim actorstring As New XmlDocument
                    actorlist = imdbscraper.getimdbactors(Pref.imdbmirror, Me.Show.ImdbId.Value)

                    actorstring.LoadXml(actorlist)

                    For Each thisresult As XmlNode In actorstring("actorlist")
                        Select Case thisresult.Name
                            Case "actor"
                                Dim newactor As New Actor
                                Dim detail As XmlNode = Nothing
                                For Each detail In thisresult.ChildNodes
                                    Select Case detail.Name
                                        Case "name"
                                            newactor.actorname = detail.InnerText
                                        Case "role"
                                            newactor.actorrole = detail.InnerText
                                        Case "thumb"
                                            newactor.actorthumb = detail.InnerText
                                        Case "actorid"
                                            If newactor.actorthumb <> Nothing Then
                                                If Pref.actorsave = True And detail.InnerText <> "" Then
                                                    Dim workingpath As String = ""
                                                    Dim networkpath As String = Pref.actorsavepath

                                                    tempstring = networkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2)
                                                    Dim hg As New DirectoryInfo(tempstring)
                                                    If Not hg.Exists Then
                                                        Directory.CreateDirectory(tempstring)
                                                    End If
                                                    workingpath = networkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "\" & detail.InnerText & ".jpg"
                                                    If Not File.Exists(workingpath) Then
                                                        Utilities.DownloadFile(newactor.actorthumb, workingpath)
                                                    End If
                                                    newactor.actorthumb = Path.Combine(Pref.actornetworkpath, detail.InnerText.Substring(detail.InnerText.Length - 2, 2))
                                                    If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                                                        newactor.actorthumb = Pref.actornetworkpath & "/" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "/" & detail.InnerText & ".jpg"
                                                    Else
                                                        newactor.actorthumb = Pref.actornetworkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "\" & detail.InnerText & ".jpg"
                                                    End If

                                                End If
                                            End If
                                    End Select
                                Next
                                Me.Show.ListActors.Add(newactor)
                        End Select
                    Next
                    While Me.Show.ListActors.Count > Pref.maxactors
                        Me.Show.ListActors.RemoveAt(Me.Show.ListActors.Count - 1)
                    End While

                End If

                Dim ArtList As Tvdb.Banners = tvdbstuff.GetPosterList(Me.Show.TvdbId.Value, True)
                If Pref.tvdlfanart = True OrElse Pref.tvdlposter = True OrElse Pref.seasonall <> "none" Then
                    If Pref.tvdlseasonthumbs = True Then
                        For f = 0 To ArtList.Items.SeasonMax
                            Dim seasonposter As String = ""
                            For Each Image In ArtList.Items
                                If Image.Season.Value = f.ToString And Image.Language.Value = TempLanguage Then
                                    seasonposter = Image.Url
                                    Exit For
                                End If
                            Next
                            If seasonposter = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Season.Value = f.ToString And Image.Language.Value = "en" Then
                                        seasonposter = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If seasonposter = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Season.Value = f.ToString Then
                                        seasonposter = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If seasonposter <> "" Then
                                If f < 10 Then
                                    tempstring = "0" & f.ToString
                                Else
                                    tempstring = f.ToString
                                End If
                                Dim seasonpath As String = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "season" & tempstring & ".tbn")
                                If tempstring = "00" Then
                                    seasonpath = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "season-specials.tbn")
                                End If
                                If Not File.Exists(seasonpath) Then

                                    Utilities.DownloadFile(seasonposter, seasonpath)

                                End If
                            End If
                        Next
                    End If

                    If Pref.tvdlfanart = True Then
                        Dim fanartposter As String
                        fanartposter = ""
                        'If CheckBox7.CheckState = CheckState.Checked Then
                        For Each Image In ArtList.Items
                            If Image.Language.Value = TempLanguage And Image.Type = Tvdb.ArtType.Fanart Then
                                fanartposter = Image.Url
                                Exit For
                            End If
                        Next
                        'End If
                        If fanartposter = "" Then
                            For Each Image In ArtList.Items
                                If Image.Language.Value = "en" And Image.Type = Tvdb.ArtType.Fanart Then
                                    fanartposter = Image.Url
                                    Exit For
                                End If
                            Next
                        End If
                        If fanartposter = "" Then
                            For Each Image In ArtList.Items
                                If Image.Type = Tvdb.ArtType.Fanart Then
                                    fanartposter = Image.Url
                                    Exit For
                                End If
                            Next
                        End If
                        If fanartposter <> "" Then

                            Dim seasonpath = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "fanart.jpg")

                            Dim point = GetBackDropResolution(Pref.BackDropResolutionSI)

                            Try
                                DownloadCache.SaveImageToCacheAndPath(fanartposter, seasonpath, Pref.overwritethumbs, point.X, point.Y)
                            Catch ex As Exception
                                Messages.Add("Error [" & ex.Message & "] downloading Fanart")
                                Me.RaiseError()
                            End Try
                        End If
                    End If


                    Dim seasonallpath As String = ""
                    If Pref.tvdlposter = True Then
                        Dim posterurlpath As String = ""

                        If Pref.postertype = "poster" Then 'poster
                            For Each Image In ArtList.Items
                                If Image.Language.Value = TempLanguage And Image.Type = Tvdb.ArtType.Poster Then
                                    posterurl = Image.Url
                                    Exit For
                                End If
                            Next
                            If posterurlpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Language.Value = "en" And Image.Type = Tvdb.ArtType.Poster Then
                                        posterurlpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If posterurlpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Type = Tvdb.ArtType.Poster Then
                                        posterurlpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If posterurlpath <> "" And Pref.seasonall <> "none" Then
                                seasonallpath = posterurlpath
                            End If
                        ElseIf Pref.postertype = "banner" Then 'banner
                            For Each Image In ArtList.Items
                                If Image.Language.Value = TempLanguage And Image.Type = Tvdb.ArtType.Banner Then
                                    posterurl = Image.Url
                                    Exit For
                                End If
                            Next
                            If posterurlpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Language.Value = "en" And Image.Type = Tvdb.ArtType.Banner Then
                                        posterurlpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If posterurlpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Type = Tvdb.ArtType.Banner Then
                                        posterurlpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            'If posterurlpath <> "" And RadioButton16.Checked = True Then
                            seasonallpath = posterurlpath
                            'End If
                        End If

                        If posterurlpath <> "" Then

                            Dim seasonpath As String = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "folder.jpg")
                            If Not File.Exists(seasonpath) Then

                                Utilities.DownloadFile(posterurlpath, seasonpath)

                            End If
                        End If
                    End If

                    If Pref.seasonall <> "none" And seasonallpath = "" Then
                        If Pref.seasonall = "poster" Then 'poster
                            For Each Image In ArtList.Items
                                If Image.Language.Value = TempLanguage And Image.Type = Tvdb.ArtType.Poster Then
                                    seasonallpath = Image.Url
                                    Exit For
                                End If
                            Next
                            If seasonallpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Language.Value = "en" And Image.Type = Tvdb.ArtType.Poster Then
                                        seasonallpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If seasonallpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Type = Tvdb.ArtType.Poster Then
                                        seasonallpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                        ElseIf Pref.seasonall = "wide" = True Then 'banner
                            For Each Image In ArtList.Items
                                If Image.Language.Value = TempLanguage And Image.Type = Tvdb.ArtType.Banner Then
                                    seasonallpath = Image.Url
                                    Exit For
                                End If
                            Next
                            If seasonallpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Language.Value = "en" And Image.Type = Tvdb.ArtType.Banner Then
                                        seasonallpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                            If seasonallpath = "" Then
                                For Each Image In ArtList.Items
                                    If Image.Type = Tvdb.ArtType.Banner Then
                                        seasonallpath = Image.Url
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                        If seasonallpath <> "" Then

                            Dim seasonpath As String = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "season-all.tbn")
                            If Not File.Exists(seasonpath) Then 'Or CheckBox6.CheckState = CheckState.Checked Then

                                Utilities.DownloadFile(seasonallpath, seasonallpath)

                            End If
                        End If
                    ElseIf Pref.seasonall <> "none" And seasonallpath <> "" Then
                        Dim seasonpath As String = Me.Show.NfoFilePath.Replace(Path.GetFileName(Me.Show.NfoFilePath), "season-all.tbn")
                        If Not File.Exists(seasonpath) Then
                            Utilities.DownloadFile(seasonallpath, seasonpath)
                        End If
                    End If
                End If

                For Each url In ArtList.Items
                    If url.Type = Tvdb.ArtType.Fanart Then
                        Me.Show.posters.Add(url.Url)
                    Else
                        Me.Show.fanart.Add(url.Url)
                    End If
                Next

                'newtvshow.language = Pref.tvdblanguagecode
                If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 2 Then
                    Me.Show.EpisodeActorSource.Value = "tvdb"
                Else
                    Me.Show.EpisodeActorSource.Value = "imdb"
                End If
                If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Then
                    Me.Show.TvShowActorSource.Value = "tvdb"
                Else
                    Me.Show.TvShowActorSource.Value = "imdb"
                End If

                If tempstring = "0" Or tempstring = "2" Then
                    Me.Show.EpisodeActorSource.Value = "tvdb"
                Else
                    Me.Show.EpisodeActorSource.Value = "imdb"
                End If

                Me.Show.SortOrder.Value = Pref.sortorder

                'nfoFunction.savetvshownfo(newtvshow.path, newtvshow, True)

            End If
        End Sub

        Public Overrides Sub FinishWork()
            Me.Show.Save()
            Cache.TvCache.Shows.Add(Me.Show)
            Me.Show.SearchForEpisodesInFolder()

            Me.Show.UpdateTreenode()

            For Each Season As TvSeason In Me.Show.Seasons.Values
                Season.UpdateTreenode()

                For Each Episode As TvEpisode In Season.Episodes
                    Episode.UpdateTreenode()
                Next
            Next

            Me.Show.ShowNode.TreeView.Sort()

            Me.State = TaskState.Completed
        End Sub


        Shared Function GetBackDropResolution(selectedIndex As Integer) As Point

            'Don't resize selected
            If selectedIndex = 0 then Return New Point(0, 0)

            Dim x = XDocument.Load("Resolutions.xml")

            Dim row = x.Descendants("Resolution").ElementAt(selectedIndex - 1)

            Dim width = Convert.ToInt32(row.Attribute("width").Value)
            Dim height = Convert.ToInt32(row.Attribute("height").Value)

            Return New Point(width, height)

        End Function

    End Class


End Namespace