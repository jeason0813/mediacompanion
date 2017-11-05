Imports System.ComponentModel
Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Xml
Imports System.Linq

Public Class frmXbmcExport
    Private MovieList As New List(Of FullMovieDetails)
    Private TVSeries As New List(Of xbmctvseries)
    Private Pathlist As New List(Of XBMCPaths)
    Private ExportPathList As New Dictionary(Of String, String)
    Dim xbmcexportfolder As String = "\xbmc_videodb_" & DateTime.Now.ToString("yyyy_MM_dd")
    Private OutputFolder As String = Nothing
    Private FullPathOut As String = Nothing
    Private opActors As String = Nothing
    Private opMovies As String = Nothing
    Private opTvshows As String = Nothing
    Private opMusicvideos As String = Nothing
    Private _outputfolderchecked As Boolean = False
    Private _cancelled As Boolean = False
    Private epcount As Integer = 0
    Public Property Bw  As BackgroundWorker = Nothing
    Public nfoFunction As New WorkingWithNfoFiles

    Dim loading As Boolean = True

    Property Cancelled As Boolean
        Get
            Return _cancelled
        End Get
        Set(value As Boolean)
            _cancelled = Value
        End Set
    End Property

    Property outputfolderchecked As Boolean
      Get
        Return _outputfolderchecked
      End Get
      Set(ByVal value as Boolean)
        If value <> _outputfolderchecked Then
            _outputfolderchecked = value
            pbCheck.Image = If(_outputfolderchecked, Global.Media_Companion.My.Resources.Resources.correct, Global.Media_Companion.My.Resources.Resources.incorrect)
        End If
        btn_Start.Enabled = value
      End Set
    End Property

    Private Sub frmXbmcExport_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetTally()
        UpdateMediaTally()
        ShowMCPaths()
        TextBox1.Text = Pref.ExportXBMCPath
        btn_Validate.PerformClick()
        loading = False
    End Sub

    Private Sub GetTally()
        Dim data = From c In Form1.oMovies.Data_GridViewMovieCache Select New With {Key .Title = c.DisplayTitleAndYear, Key .NFOpath = c.fullpathandfilename, Key .path = c.foldername}
        For each row in data
            Dim movie As New FullMovieDetails 
            movie.fileinfo.fullpathandfilename = row.NFOpath
            MovieList.Add(movie)
        Next
        For Each Sh In Cache.TvCache.Shows
            Dim tvshow As New xbmctvseries 
            tvshow.Series.Title = Sh.Title.Value
            tvshow.Series.Filenameandpath = Sh.NfoFilePath
            For Each ep In Sh.Episodes
                Dim epdata As New xbmctv
                epdata.Title = ep.Title.Value
                epdata.Season = ep.Season.Value
                epdata.Episode = ep.Episode.Value
                epdata.Filenameandpath = ep.NfoFilePath
                tvshow.episodes.Add(epdata)
            Next
            tvshow.series.Episode = tvshow.episodes.Count.ToString
            tvshow.series.Season = Sh.Seasons.count.ToString
            TVSeries.Add(tvshow)
        Next
    End Sub

    Private Sub ShowMCPaths()
        If MovieList.Count > 0 Then
            For Each ph In Pref.movieFolders
                If Not ph.selected Then Continue For
                Dim n As Integer = MCExportdgv.Rows.Add()
                MCExportdgv.Rows(n).Cells(0).Value = "Movie"
                MCExportdgv.Rows(n).Cells(1).Value = ph.rpath & "\"
                MCExportdgv.Rows(n).Cells(2).Value = ph.rpath & "\"
            Next
        End If
        If TVSeries.Count > 0 Then
            For Each ph In Pref.tvRootFolders
                If Not ph.selected Then Continue For
                Dim n As Integer = MCExportdgv.Rows.Add()
                MCExportdgv.Rows(n).Cells(0).Value = "TV"
                MCExportdgv.Rows(n).Cells(1).Value = ph.rpath & "\"
                MCExportdgv.Rows(n).Cells(2).Value = ph.rpath & "\"
            Next
        End If
    End Sub

    Private Sub UpdateMediaTally()
        For Each sh In TVSeries
            epcount += CType(sh.episodes.Count, Integer)
        Next
        lblMovieCount.Text = MovieList.Count.ToString
        lblTVCount.Text = TVSeries.Count.ToString & "  (" & epcount.ToString & " episodes)"
    End Sub

    Public Sub RunExport()
        btn_Start.Enabled = False
        ProgressBar1.Value = 0
        ProgressBar1.Maximum = MovieList.Count + TVSeries.Count + epcount
        ProgressBar1.BackColor = Color.red
        SetpathMapping()
        SetOutputFolders()

        ''Create Document
        Dim doc As New XmlDocument
        Dim xmlproc As XmlDeclaration
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        Dim root As XmlElement = Nothing
        root = doc.CreateElement("videodb")
        Dim version As XmlElement = Nothing
        version = doc.CreateElement("version")
        version.InnerText = "1"
        root.AppendChild(version)
        
        ''Run Movie Output
        For Each Moviefound In MovieList
            Application.DoEvents()
            If Cancelled Then Exit Sub
            Me.ProgressBar1.Value +=1
            If Not File.Exists(Moviefound.fileinfo.fullpathandfilename) Then Continue For
            Dim mov As FullMovieDetails = WorkingWithNfoFiles.mov_NfoLoadFull(Moviefound.fileinfo.fullpathandfilename)
            Dim currentmovie As New XmlDocument
            currentmovie = Transposemovie(mov)
            Dim oDoc As XMLNode = root.OwnerDocument.ImportNode(currentmovie.DocumentElement, True)
            root.AppendChild(oDoc)
            Application.DoEvents()
            If Cancelled Then Exit Sub
            TransMovArtWork(mov)
            Application.DoEvents()
            If Cancelled Then Exit Sub
            TransActorImages(mov)
            Me.ProgressBar1.Refresh()
        Next
        If MovieList.Count > 0 Then
            For Each ph In Pref.movieFolders
                If Not ph.selected Then Continue For
                Dim t As New XBMCPaths
                t.rootpath = ph.rpath
                t.pathsource = "movies"
                Pathlist.Add(t)
            Next
        End If
        doc.AppendChild(root)

        ''Run Tv Output
        For Each sh In TVSeries
            Application.DoEvents()
            If Cancelled Then Exit Sub
            Me.ProgressBar1.Value +=1
            If Not File.Exists(sh.series.Filenameandpath) Then Continue For
            Dim tvsh As TvShow = nfoFunction.tvshow_NfoLoad(sh.series.Filenameandpath)
            Dim CurrentSh As New XmlDocument
            CurrentSh = TransposetvShows(tvsh, sh.episodes.count, sh.series.Season)
            Application.DoEvents()
            If Cancelled Then Exit Sub
            TransTvActorImages(tvsh.ListActors, sh.series.Filenameandpath)
            Application.DoEvents()
            If Cancelled Then Exit Sub
            For Each ep In sh.episodes
                Application.DoEvents()
                If Cancelled Then Exit Sub
                If Not File.Exists(ep.Filenameandpath) Then Continue For
                Dim tvep As List(Of TvEpisode) = WorkingWithNfoFiles.ep_NfoLoad(ep.Filenameandpath)
                For each subep In tvep
                    Application.DoEvents()
                    If Cancelled Then Exit Sub
                    If Not ep.Episode = subep.Episode.Value Then Continue For
                    Dim CurrentEp As New XmlDocument
                    CurrentEp = TransposeTvEp(subep, tvsh)
                    Dim oDoc2 As XMLNode = CurrentSh.ImportNode(CurrentEp.DocumentElement, True)
                    CurrentSh.DocumentElement.AppendChild(oDoc2)
                Next
                Application.DoEvents()
                If Cancelled Then Exit Sub
                Me.ProgressBar1.Value +=1
                Me.ProgressBar1.Refresh()
            Next
            Dim oDoc As XMLNode = root.OwnerDocument.ImportNode(CurrentSh.DocumentElement, True)
            root.AppendChild(oDoc)
            Me.ProgressBar1.Refresh()
        Next
        If TVSeries.Count > 0 Then
            For Each ph In Pref.tvRootFolders
                If Not ph.selected Then Continue For
                Dim t As New XBMCPaths
                t.rootpath = ph.rpath
                t.pathsource = "tvshows"
                Pathlist.Add(t)
            Next
        End If

        Dim xbpaths As New XmlDocument
        xbpaths = SetPaths()
        Dim oDoc3 As XMLNode = root.OwnerDocument.ImportNode(xbpaths.DocumentElement, True)
        root.AppendChild(oDoc3)
        doc.AppendChild(root)
        Try
            Dim settings As New XmlWriterSettings()
            settings.Encoding = Encoding.UTF8 
            settings.Indent = True
            settings.IndentChars = (ControlChars.Tab)
            settings.NewLineHandling = NewLineHandling.None
            Dim writer As XmlWriter = XmlWriter.Create(FullPathOut & "\videodb.xml", settings) 
            doc.Save(writer)
            writer.Close()
        Catch
        End Try
        If ProgressBar1.Value = ProgressBar1.Maximum Then
            ProgressBar1.ForeColor = Color.Green
            ProgressBar1.BackColor = Color.Green
            btn_Cancel.Text = "Finished"
            btn_Cancel.Refresh()
        End If
        btn_Start.Enabled = True
    End Sub

    Private Sub TransMovArtWork(ByVal mov As FullMovieDetails)
        Dim filenm As String = Utilities.SpacesToCharacter(Utilities.cleanFilenameIllegalChars(mov.fullmoviebody.title, " ") & " " & mov.fullmoviebody.year, "_")
        filenm = FormatText(filenm)
        If File.Exists(mov.fileinfo.fanartpath) Then
            File.Copy(mov.fileinfo.fanartpath, opMovies & filenm & "-fanart.jpg", True)
        End If
        If File.Exists(mov.fileinfo.posterpath) Then
            File.Copy(mov.fileinfo.posterpath, opMovies & filenm & "-poster.jpg", True)
        End If
    End Sub

    Private Sub TransActorImages(ByVal mov As FullMovieDetails)
        Try
            For Each actr In mov.listactors
                If Cancelled Then Exit Sub
                Dim temppath = Pref.GetActorPath(mov.fileinfo.fullpathandfilename, actr.actorname, actr.actorid)
                If Not String.IsNullOrEmpty(temppath) AndAlso File.Exists(temppath) Then
                    Dim actfilename As String = Utilities.GetFileNameFromPath(temppath)
                    File.Copy(temppath, opActors & actfilename, True)
                End If
            Next
        Catch

        End Try
    End Sub
    
    Private Sub TransTvActorImages(ByVal TvActor As ActorList, ByVal nfopath As String)
        Try
            For Each actr In TvActor
                If Cancelled Then Exit Sub
                Dim temppath = Pref.GetActorPath(nfopath, actr.actorname, actr.actorid)
                If Not String.IsNullOrEmpty(temppath) AndAlso File.Exists(temppath) Then
                    Dim actfilename As String = Utilities.GetFileNameFromPath(temppath)
                    File.Copy(temppath, opActors & actfilename, True)
                End If
            Next
        Catch

        End Try
    End Sub
    
    Private Sub SetOutputFolders()
        FullPathOut = OutputFolder & xbmcexportfolder
        If Not Directory.Exists(FullPathOut) Then Directory.CreateDirectory(FullPathOut)
        opActors = FullPathOut & "\actors\"
        If Not Directory.Exists(opActors) Then Directory.CreateDirectory(opActors)
        opMovies = FullPathOut & "\movies\"
        If Not Directory.Exists(opMovies) Then Directory.CreateDirectory(opMovies)
        opTvshows = FullPathOut & "\tvshows\"
        If Not Directory.Exists(opTvshows) Then Directory.CreateDirectory(opTvshows)
        opMusicvideos = FullPathOut & "\musicvideos\"
        If Not Directory.Exists(opMusicvideos) Then Directory.CreateDirectory(opMusicvideos)
    End Sub

    Private Sub SetpathMapping()
        For Each row In MCExportdgv.Rows
            Dim src As String = row.cells(1).Value
            Dim xport As String = row.Cells(2).Value
            If src = xport Then Continue For                         ' no need to convert path if source and export paths are the same.
            If ExportPathList.ContainsKey(src) Then Continue For
            ExportPathList.Add(src, row.cells(2).Value)
        Next
    End Sub

    Public Function Validatefolder(ByVal outputfolder As String) As String
        If Not Directory.Exists(outputfolder) Then
            MsgBox("Directory does not exist")
            Return Nothing
        End If
        
        Try 'create new output folder
            If Not Directory.Exists(outputfolder & xbmcexportfolder) Then Directory.CreateDirectory(outputfolder & xbmcexportfolder)
        Catch
            MsgBox("Unable to create export folder")
            Return Nothing
        End Try
        'test path is writeable
        Try
            Dim fs As IO.FileStream = File.Create(outputfolder & xbmcexportfolder & "\mc_dmmy.txt")

            ' Add text to the file. 
            Dim info As Byte() = New UTF8Encoding(True).GetBytes("This is some text in the file.")
            fs.Write(info, 0, info.Length)
            fs.Close()
        Catch
            MsgBox("Unable to create file in folder")
            Return Nothing
        End Try
        Try
            File.Delete(outputfolder & xbmcexportfolder & "\mc_dmmy.txt")
            Directory.Delete(outputfolder & xbmcexportfolder, True)
        Catch 
            MsgBox("Unable to Remove file in folder" &vbCrLf & "Check this folder's permissions")
            Return Nothing 
        End Try
        Return outputfolder 
    End Function

    Private Function Transposemovie(ByVal mov As FullMovieDetails) As XmlDocument
        Dim thismovie As New XmlDocument
        Try
            Dim root As XmlElement = Nothing
            Dim child As XmlElement = Nothing
            Dim filedetailschild As XmlElement = Nothing
            Dim anotherchild As XmlElement = Nothing

            root = thismovie.CreateElement("movie")
            root.AppendChild( thismovie, "title"                    , mov.fullmoviebody.title           )
            root.AppendChild( thismovie, "originaltitle"            , mov.fullmoviebody.originaltitle       , mov.fullmoviebody.title   )
            root.AppendChild( thismovie, "sorttitle"                , mov.fullmoviebody.sortorder           , mov.fullmoviebody.title   )
            root.AppendChild( thismovie, "rating"                   , mov.fullmoviebody.rating.ToRating.ToString("0.0", Form1.MyCulture))
            root.AppendChild( thismovie, "userrating"               , mov.fullmoviebody.usrrated        )
            root.AppendChild( thismovie, "epbookmark"               , "0.000000"                        )
            root.AppendChild( thismovie, "year"                     , mov.fullmoviebody.year            )
            root.AppendChild( thismovie, "top250"                   , mov.fullmoviebody.top250          )
            root.AppendChild( thismovie, "votes"                    , mov.fullmoviebody.votes.ToVotes(Pref.MovThousSeparator)           )
            root.AppendChild( thismovie, "outline"                  , mov.fullmoviebody.outline         )
            root.AppendChild( thismovie, "plot"                     , mov.fullmoviebody.plot            )
            root.AppendChild( thismovie, "tagline"                  , mov.fullmoviebody.tagline         )
            root.AppendChild( thismovie, "runtime"                  , mov.fullmoviebody.runtime.ToMin   )
            For Each item In mov.frodoPosterThumbs
                child = thismovie.CreateElement("thumb")
                child.SetAttribute("aspect", item.Aspect)
                child.InnerText = item.Url
                root.AppendChild(child)
            Next
            If mov.frodoFanartThumbs.Thumbs.Count > 0 Then
                root.AppendChild(mov.frodoFanartThumbs.GetChild(thismovie))
            End If
            For Each thumbnail In mov.listthumbs
                child = thismovie.CreateElement("thumb")
                child.InnerText = thumbnail
                root.AppendChild(child)
            Next
            root.AppendChild( thismovie, "mpaa"                     , mov.fullmoviebody.mpaa            )
            root.AppendChild( thismovie, "showlink"                 , mov.fullmoviebody.showlink        )
            root.AppendChild( thismovie, "playcount"                , mov.fullmoviebody.playcount       )
            root.AppendChild( thismovie, "lastplayed"               , mov.fullmoviebody.lastplayed      )
            root.AppendChild( thismovie, "file"                     , ""                                )
            root.AppendChild( thismovie, "basepath"                 , TransposePath(GetBasePath(mov))   )
            root.AppendChild( thismovie, "id"                       , mov.fullmoviebody.imdbid          )
            root.AppendChildList( thismovie, "genre"                , mov.fullmoviebody.genre           )
            root.AppendChildList( thismovie, "country"              , mov.fullmoviebody.country, ","    )
            Dim MovSet As String = If(mov.fullmoviebody.SetName = "-None-", "", mov.fullmoviebody.SetName)
            root.AppendChild( thismovie, "set"                      , MovSet                            )
            root.AppendChildList( thismovie, "tag"                  , mov.fullmoviebody.tag             )
            root.AppendChildList( thismovie, "credits"              , mov.fullmoviebody.credits         )
            root.AppendChildList( thismovie, "director"             , mov.fullmoviebody.director        )
            root.AppendChild( thismovie, "premiered"                , mov.fullmoviebody.premiered       )
            root.AppendChild( thismovie, "status"                   , "")
            root.AppendChild( thismovie, "code"                     , "")
            root.AppendChild( thismovie, "aired"                    , "")
            root.AppendChildList( thismovie, "studio"               , mov.fullmoviebody.studio, ","     )
            root.AppendChild( thismovie, "trailer"                  , If(mov.fullmoviebody.trailer.Contains("http"), mov.fullmoviebody.trailer, TransposePath(mov.fullmoviebody.trailer)))

            child = thismovie.CreateElement("fileinfo")
            anotherchild = thismovie.CreateElement("streamdetails")
            filedetailschild = thismovie.CreateElement("video")
            filedetailschild.AppendChild( thismovie, "codec"        , mov.filedetails.Video.Codec.Value         )
            filedetailschild.AppendChild( thismovie, "aspect"       , mov.filedetails.Video.Aspect.Value.Pad    )
            filedetailschild.AppendChild( thismovie, "width"        , mov.filedetails.Video.Width.Value         )
            filedetailschild.AppendChild( thismovie, "height"       , mov.filedetails.Video.Height.Value        )
            Dim durat As String = If(mov.filedetails.Video.DurationInSeconds.Value = "-1", "0", mov.filedetails.Video.DurationInSeconds.Value)
            filedetailschild.AppendChild( thismovie, "durationinseconds", durat                                             )
            filedetailschild.AppendChild( thismovie, "stereomode"   , ""                                                    )
            anotherchild.AppendChild(filedetailschild)

            For Each item In mov.filedetails.Audio
                filedetailschild = thismovie.CreateElement("audio")
                filedetailschild.AppendChild( thismovie, "codec"    , item.Codec.Value      )
                filedetailschild.AppendChild( thismovie, "language" , item.Language.Value   )
                filedetailschild.AppendChild( thismovie, "channels" , item.Channels.Value   )
                anotherchild.AppendChild(filedetailschild)
            Next
            
            For Each entry In mov.filedetails.Subtitles
                filedetailschild = thismovie.CreateElement("subtitle")
                filedetailschild.AppendChild( thismovie, "language", entry.Language.Value   )
                filedetailschild.AppendChild( thismovie, "primary" , entry.Primary          )
                anotherchild.AppendChild(filedetailschild)
            Next
            
            child.AppendChild(anotherchild)
            root.AppendChild(child)

            Dim actorstosave As Integer = mov.listactors.Count
            If actorstosave > Pref.maxactors Then actorstosave = Pref.maxactors
            For f = 0 To actorstosave - 1
                child = thismovie.CreateElement("actor")
                child.AppendChild( thismovie, "name"       , mov.listactors(f).actorname   )
                child.AppendChild( thismovie, "role"       , mov.listactors(f).actorrole   )
                child.AppendChild( thismovie, "order"      , mov.listactors(f).order       )
                child.AppendChild( thismovie, "thumb"      , mov.listactors(f).actorthumb  )
                root.AppendChild(child)
            Next

            child = thismovie.CreateElement("resume")
            child.AppendChild( thismovie, "position"        , "0.000000")
            child.AppendChild( thismovie, "total"           , "0.000000")
            root.AppendChild(child)
            
            root.AppendChild( thismovie, "dateadded"            , Formatdate(mov.fileinfo.createdate)       )
            child = thismovie.CreateElement("art")
            child.AppendChild( thismovie, "fanart"              , TransposePath(mov.fileinfo.fanartpath)    )
            child.AppendChild( thismovie, "poster"              , TransposePath(mov.fileinfo.posterpath)    )
            root.AppendChild(child)

            thismovie.AppendChild(root)
        Catch ex As Exception

        End Try
        Return thismovie 
    End Function

    Private Function TransposetvShows(ByVal tvsh As TvShow, ByVal epcount As Integer, ByVal seasoncount As Integer) As XmlDocument
        Dim ThisTvShow As New XmlDocument
        Try
            Dim root As XmlElement = Nothing
            Dim child As XmlElement = Nothing
            Dim childchild As XmlElement = Nothing
            Dim Attr As XmlAttribute = Nothing
            Dim tempppp As String = ""
            root = ThisTvShow.CreateElement("tvshow")
            
            root.AppendChild(ThisTvShow, "title"            , tvsh.Title.Value              )
            root.AppendChild(ThisTvShow, "showtitle"        , tvsh.Title.Value              )
            root.AppendChild(ThisTvShow, "rating"           , tvsh.Rating.Value             )
            root.AppendChild(ThisTvShow, "userrating"       , tvsh.UserRating.Value         )
            root.AppendChild(ThisTvShow, "epbookmark"       , "0.000000"                    )
            root.AppendChild(ThisTvShow, "year"             , tvsh.Year.Value               )
            root.AppendChild(ThisTvShow, "top250"           , tvsh.Top250.Value             )
            root.AppendChild(ThisTvShow, "season"           , seasoncount                   )
            root.AppendChild(ThisTvShow, "episode"          , epcount                       )
            root.AppendChild(ThisTvShow, "uniqueid"         , ""                            )
            root.AppendChild(ThisTvShow, "displayseason"    , "-1"                          )
            root.AppendChild(ThisTvShow, "displayepisode"   , "-1"                          )
            root.AppendChild(ThisTvShow, "votes"            , tvsh.Votes.Value              )
            root.AppendChild(ThisTvShow, "plot"             , tvsh.Plot.Value               )
            root.AppendChild(ThisTvShow, "runtime"          , tvsh.Runtime.Value            )
            root.AppendChild(ThisTvShow, "mpaa"             , tvsh.Mpaa.Value               )
            root.AppendChild(ThisTvShow, "playcount"        , tvsh.Playcount.Value          )
            root.AppendChild(ThisTvShow, "file"             , ""                            )
            root.AppendChild(ThisTvShow, "path"             , TransposePath(tvsh.FolderPath))
            root.AppendChild(ThisTvShow, "filenameandpath"  , ""                            )
            root.AppendChild(ThisTvShow, "basepath"         , TransposePath(tvsh.FolderPath))
            child = ThisTvShow.CreateElement("episodeguide")
            childchild = ThisTvShow.CreateElement("url")
            tempppp = tvsh.TvdbId.value
            Attr = ThisTvShow.CreateAttribute("cache")
            Attr.Value = tempppp & "-" & tvsh.Language.value & ".xml"
            childchild.Attributes.Append(Attr)
            If Not IsNothing(tvsh.episodeguideurl) Then
                '"http://www.thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/all/" & language & ".zip"
                If tvsh.tvdbid.Value <> Nothing Then
                    If IsNumeric(tvsh.tvdbid.Value) Then
                        If tvsh.language.Value <> Nothing Then
                            If tvsh.language.Value <> "" Then
                                childchild.InnerText = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvsh.tvdbid.Value & "/all/" & tvsh.language.Value & ".zip"
                                child.AppendChild(childchild)
                            Else
                                childchild.InnerText = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvsh.tvdbid.Value & "/all/en.zip"
                                child.AppendChild(childchild)
                            End If
                        Else
                            childchild.InnerText = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvsh.tvdbid.Value & "/all/en.zip"
                            child.AppendChild(childchild)
                        End If
                    End If
                End If
            End If
            root.AppendChild(child)
            root.AppendChild(ThisTvShow, "id"           , tvsh.TvdbId.Value         )
            root.AppendChildList(ThisTvShow, "genre"    , tvsh.Genre.Value          )
            root.AppendChild(ThisTvShow, "set"      , ""                    )
            root.AppendChild(ThisTvShow, "premiered", tvsh.Premiered.Value  )
            root.AppendChild(ThisTvShow, "status"   , tvsh.Status.Value     )
            root.AppendChild(ThisTvShow, "code"     , ""                    )
            root.AppendChild(ThisTvShow, "aired"    , ""                    )
            root.AppendChild(ThisTvShow, "studio"   , tvsh.Studio.Value     )
            root.AppendChild(ThisTvShow, "trailer"  , ""                    )
            For each act As Actor In tvsh.ListActors
                child = ThisTvShow.CreateElement("actor")
                child.AppendChild(ThisTvShow, "name"    , act.actorname )
                child.AppendChild(ThisTvShow, "role"    , act.actorrole )
                child.AppendChild(ThisTvShow, "order"   , act.order     )
                child.AppendChild(ThisTvShow, "thumb"   , act.actorthumb)
                root.AppendChild(child)
            Next

            child = ThisTvShow.CreateElement("art")
            Dim titlepath As String = opTvshows & FormatText(tvsh.Title.Value) & "\"
            If Not Directory.Exists(titlepath) Then Directory.CreateDirectory(titlepath)
            If File.Exists(tvsh.ImageBanner.Path) Then
                File.Copy(tvsh.ImageBanner.Path, titlepath & tvsh.ImageBanner.FileName.ToLower, True)
                child.AppendChild(ThisTvShow, "banner"  , TransposePath(tvsh.ImageBanner.Path)      )
            End If
            If File.Exists(tvsh.ImageFanart.Path) Then
                File.Copy(tvsh.ImageFanart.Path, titlepath & tvsh.ImageFanart.FileName.ToLower, True)
                child.AppendChild(ThisTvShow, "fanart"  , TransposePath(tvsh.ImageFanart.Path)      )
            End If
            If File.Exists(tvsh.ImagePoster.Path) Then
                File.Copy(tvsh.ImagePoster.Path, titlepath & tvsh.ImagePoster.FileName.ToLower, True)
                child.AppendChild(ThisTvShow, "poster"  , TransposePath(tvsh.ImagePoster.Path)      )
            End If
            childchild = ThisTvShow.CreateElement("season")
            tempppp = "-1"
            Attr = ThisTvShow.CreateAttribute("num")
            Attr.Value = tempppp
            childchild.Attributes.Append(Attr)
            If File.Exists(tvsh.ImageAllSeasons.Path.Replace("-poster", "-banner")) Then
                File.Copy(tvsh.ImageFanart.Path, titlepath & tvsh.ImageAllSeasons.FileName.Replace("-poster", "-banner").ToLower, True)
                childchild.AppendChild(ThisTvShow, "banner", TransposePath(tvsh.ImageAllSeasons.Path.Replace("-poster", "-banner")))
            End If
            If File.Exists(tvsh.ImageAllSeasons.Path.Replace("-poster", "-fanart")) Then
                File.Copy(tvsh.ImageFanart.Path, titlepath & tvsh.ImageAllSeasons.FileName.Replace("-poster", "-fanart").ToLower, True)
                childchild.AppendChild(ThisTvShow, "fanart", TransposePath(tvsh.ImageAllSeasons.Path.Replace("-poster", "-fanart")))
            End If
            If File.Exists(tvsh.ImageAllSeasons.Path) Then
                File.Copy(tvsh.ImageFanart.Path, titlepath & tvsh.ImageAllSeasons.Filename.ToLower, True)
                childchild.AppendChild(ThisTvShow, "poster", TransposePath(tvsh.ImageAllSeasons.Path))
            End If
            child.AppendChild(childchild)
            childchild = ThisTvShow.CreateElement("season")
            tempppp = "0"
            Attr = ThisTvShow.CreateAttribute("num")
            Attr.Value = tempppp
            childchild.Attributes.Append(Attr)
            If File.Exists(tvsh.FolderPath & "season-specials-banner.jpg") Then
                File.Copy(tvsh.FolderPath & "season-specials-banner.jpg", titlepath & "season-specials-banner.jpg", True)
                childchild.AppendChild(ThisTvShow, "banner", TransposePath(tvsh.FolderPath) & "season-specials-banner.jpg")
            End If
            If File.Exists(tvsh.FolderPath & "season-specials-poster.jpg") Then
                File.Copy(tvsh.FolderPath & "season-specials-poster.jpg", titlepath & "season-specials-poster.jpg", True)
                childchild.AppendChild(ThisTvShow, "poster", TransposePath(tvsh.FolderPath) & "season-specials-poster.jpg")
            End If
            child.AppendChild(childchild)
            For i = 1 To 200
                Dim s As String = i.ToString
                If 1 < 10 Then s = "0" & s
                If File.Exists(tvsh.FolderPath & "season" & s & "-banner.jpg") OrElse File.Exists(tvsh.FolderPath & "season" & s & "-poster.jpg") Then
                    childchild = ThisTvShow.CreateElement("season")
                    Attr = ThisTvShow.CreateAttribute("num")
                    Attr.Value = i.tostring
                    childchild.Attributes.Append(Attr)
                    Dim SeasonArt As String = "season" & s & "-banner.jpg"
                    If File.Exists(tvsh.FolderPath & SeasonArt) Then
                        File.Copy(tvsh.FolderPath & SeasonArt, titlepath & SeasonArt, True)
                        childchild.AppendChild(ThisTvShow, "banner", TransposePath(tvsh.FolderPath) & SeasonArt)
                    End If
                    SeasonArt = "season" & s & "-poster.jpg"
                    If File.Exists(tvsh.FolderPath & SeasonArt) Then
                        File.Copy(tvsh.FolderPath & SeasonArt, titlepath & SeasonArt, True)
                        childchild.AppendChild(ThisTvShow, "poster", TransposePath(tvsh.FolderPath) & SeasonArt)
                    End If
                    child.AppendChild(childchild)
                End If
            Next
            root.AppendChild(child)
            ThisTvShow.AppendChild(root)
        Catch
        End Try
        Return ThisTvShow 
    End Function

    Private Function TransposeTvEp(ByVal tvep As TvEpisode, ByVal sh As tvshow) As XmlDocument
        Dim ThisTvEp As New XmlDocument
        Try
            Dim root As XmlElement = Nothing
            Dim child As XmlElement = Nothing
            Dim filedetailschild As XmlElement = Nothing
            Dim anotherchild As XmlElement = Nothing
            root = ThisTvEp.CreateElement("episodedetails")
            root.AppendChild(ThisTvEp, "title"              , tvep.Title.Value                  )
            root.AppendChild(ThisTvEp, "showtitle"          , sh.Title.Value                    )
            root.AppendChild(ThisTvEp, "rating"             , tvep.Rating.Value                 )
            root.AppendChild(ThisTvEp, "votes"              , tvep.Votes.Value                  )
            root.AppendChild(ThisTvEp, "epbookmark"         , tvep.EpBookmark.Value             )
            root.AppendChild(ThisTvEp, "userrating"         , tvep.UserRating.Value             )
            root.AppendChild(ThisTvEp, "year"               , tvep.Year.Value                   )
            root.AppendChild(ThisTvEp, "top250"             , sh.Top250.Value                   )
            root.AppendChild(ThisTvEp, "season"             , tvep.Season.Value                 )
            root.AppendChild(ThisTvEp, "episode"            , tvep.Episode.Value                )
            root.AppendChild(ThisTvEp, "uniqueid"           , tvep.UniqueId.Value               )
            root.AppendChild(ThisTvEp, "displayseason"      , tvep.DisplaySeason.Value, "-1"    )
            root.AppendChild(ThisTvEp, "displayepisode"     , tvep.DisplayEpisode.Value, "-1"   )
            root.AppendChild(ThisTvEp, "votes"              , tvep.Votes.Value                  )
            root.AppendChild(ThisTvEp, "outline"            , ""                                )
            root.AppendChild(ThisTvEp, "plot"               , tvep.Plot.Value                   )
            root.AppendChild(ThisTvEp, "tagline"            , tvep.TagLine.Value                )
            root.AppendChild(ThisTvEp, "runtime"            , tvep.Runtime.Value                )
            root.AppendChild(ThisTvEp, "mpaa"               , sh.Mpaa.Value                     )
            root.AppendChild(ThisTvEp, "playcount"          , tvep.PlayCount.Value              )
            root.AppendChild(ThisTvEp, "lastplayed"         , tvep.LastPlayed.Value             )
            root.AppendChild(ThisTvEp, "file"               , ""                                )
            root.AppendChild(ThisTvEp, "path"               , tvep.FolderPath                   )
            root.AppendChild(ThisTvEp, "filenameandpath"    , tvep.VideoFilePath                )
            root.AppendChild(ThisTvEp, "basepath"           , tvep.VideoFilePath                )
            root.AppendChild(ThisTvEp, "id"                 , tvep.TvdbId.Value                 )
            root.AppendChild(ThisTvEp, "set"                , ""                                )
            root.AppendChild(ThisTvEp, "credits"            , tvep.Credits.Value                )
            root.AppendChild(ThisTvEp, "director"           , tvep.Director.Value               )
            root.AppendChild(ThisTvEp, "premiered"          , tvep.Premiered.Value              )
            root.AppendChild(ThisTvEp, "status"             , ""                                )
            root.AppendChild(ThisTvEp, "code"               , ""                                )
            root.AppendChild(ThisTvEp, "aired"              , tvep.Aired.Value                  )
            root.AppendChild(ThisTvEp, "studio"             , sh.Studio.Value                   )
            root.AppendChild(ThisTvEp, "trailer"            , ""                                )
            
            child = ThisTvEp.CreateElement("fileinfo")
            anotherchild = ThisTvEp.CreateElement("streamdetails")
            filedetailschild = ThisTvEp.CreateElement("video")
            Dim durat As String = If(tvep.StreamDetails.Video.DurationInSeconds.Value = "-1", "0", tvep.StreamDetails.Video.DurationInSeconds.Value)
            filedetailschild.AppendChild(ThisTvEp, "codec"              , tvep.StreamDetails.Video.Codec.Value  )
            filedetailschild.AppendChild(ThisTvEp, "aspect"             , tvep.StreamDetails.Video.Aspect.Value )
            filedetailschild.AppendChild(ThisTvEp, "width"              , tvep.StreamDetails.Video.Width.Value  )
            filedetailschild.AppendChild(ThisTvEp, "height"             , tvep.StreamDetails.Video.Height.Value )
            filedetailschild.AppendChild(ThisTvEp, "durationinseconds"  , durat                                         )
            anotherchild.AppendChild(filedetailschild)

            For Each item In tvep.StreamDetails.Audio
                filedetailschild = ThisTvEp.CreateElement("audio")
                filedetailschild.AppendChild( ThisTvEp, "codec"    , item.Codec.Value      )
                filedetailschild.AppendChild( ThisTvEp, "language" , item.Language.Value   )
                filedetailschild.AppendChild( ThisTvEp, "channels" , item.Channels.Value   )
                anotherchild.AppendChild(filedetailschild)
            Next
            
            For Each entry In tvep.StreamDetails.Subtitles
                filedetailschild = ThisTvEp.CreateElement("subtitle")
                filedetailschild.AppendChild( ThisTvEp, "language", entry.Language.Value   )
                filedetailschild.AppendChild( ThisTvEp, "primary" , entry.Primary          )
                anotherchild.AppendChild(filedetailschild)
            Next
            
            child.AppendChild(anotherchild)
            root.AppendChild(child)

            Dim listofactors As New List(Of Actor)
            listofactors.AddRange(sh.ListActors)
            If tvep.ListActors.Count > 0 Then
                For each epactor In tvep.ListActors
                    Dim add As Boolean = True
                    For each shactor In listofactors
                        If shactor.actorname.ToLower = epactor.actorname.ToLower Then
                            add = False
                            Exit for
                        End If
                    Next
                    If add Then listofactors.Add(epactor)
                Next
            End If
            If listofactors.Count > 0 Then
                For each act In listofactors
                    child = ThisTvEp.CreateElement("actor")
                    child.AppendChild(ThisTvEp, "name"      , act.actorname     )
                    child.AppendChild(ThisTvEp, "actorid"   , act.actorid       )
                    child.AppendChild(ThisTvEp, "role"      , act.actorrole     )
                    child.AppendChild(ThisTvEp, "order"     , act.order, "0"    )
                    child.AppendChild(ThisTvEp, "thumb"     , act.Thumb.Value   )
                    root.AppendChild(child)
                Next
            End If

            child = ThisTvEp.CreateElement("art")
            Dim titlepath As String = opTvshows & FormatText(sh.Title.Value) & "\"
            If File.Exists(tvep.Thumbnail.Path) Then
                Dim epimg As String = "s" & If(tvep.Season.Value.Length = 1, "0", "") & tvep.Season.Value & "e" & If(tvep.Episode.Value.Length = 1, "0", "") & tvep.Episode.Value & "-thumb.jpg"
                File.Copy(tvep.Thumbnail.Path, titlepath & epimg, True)
                child.AppendChild(ThisTvEp, "thumb", TransposePath(tvep.Thumbnail.Path))
            End If
            root.AppendChild(child)
            ThisTvEp.AppendChild(root)
        Catch
        End Try
        Return ThisTvEP 
    End Function

    Private Function SetPaths() As XmlDocument
        Dim SetXBPaths As New XmlDocument
        Try
            Dim root As XmlElement = Nothing
            Dim child As XmlElement = Nothing
            Dim anotherchild As XmlElement = Nothing
            root = SetXBPaths.CreateElement("paths")
            For Each xbpath In Pathlist 
                child = SetXBPaths.CreateElement("path")
                child.AppendChild(SetXBPaths, "url"             , xbpath.rootpath & "\"     )
                Dim scanrec As String = "2147483647"
                If xbpath.pathsource = "tv" Then scanrec = "0"
                child.AppendChild(SetXBPaths, "scanrecursive"   , scanrec                   )
                child.AppendChild(SetXBPaths, "usefoldernames"  , "False"                   )
                child.AppendChild(SetXBPaths, "content"         , xbpath.pathsource         )
                child.AppendChild(SetXBPaths, "scraperpath"     , "metadata.local"          )
                root.AppendChild(child)
            Next
            SetXBPaths.AppendChild(root)
        Catch ex As Exception
        End Try
        Return SetXBPaths
    End Function

    Private Function Formatdate(ByVal thisdate As String) As String
        Try
            Return DateTime.ParseExact(thisdate, Pref.datePattern, Nothing).ToString(Pref.DateFormat2)
        Catch
            Return "2001-01-01 01:01:00"
        End Try
    End Function

    Private Function GetBasePath(ByVal thismov As FullMovieDetails) As String
        Dim ThisBasePath As String = thismov.fileinfo.fullpathandfilename
        If Path.GetFileName(ThisBasePath).ToLower = "video_ts.nfo" Or Path.GetFileName(ThisBasePath).ToLower = "index.nfo" Then
            Return Utilities.RootVideoTsFolder(ThisBasePath)
        Else
            Return thismov.fileinfo.filenameandpath 
        End If
    End Function

    Private Function TransposePath(ByVal checkpath As String) As String
        For Each item In ExportPathList
            If checkpath.Contains(item.Key) Then
                checkpath = checkpath.Replace(item.Key, item.Value)
                If item.Value.Contains("/") Then
                    checkpath = checkpath.Replace("\", "/")
                ElseIf item.Value.Contains("\") Then
                    checkpath = checkpath.Replace("/", "\")
                End If
            End If
        Next
        Return checkpath 
    End Function

    Private Function FormatText(ByVal s As String) As String
        s = s.Replace(" ", "_")
        s = s.Replace(":", "_")
        Return s
    End Function
    
    Private Sub frmXbmcExport_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If btn_Cancel.Text = "Cancel" Then
                Cancelled = True
                Application.DoEvents()
            Else
                Me.Close()
            End If
        End If
            
    End Sub

    Private Sub btn_Start_Click( sender As Object,  e As EventArgs) Handles btn_Start.Click
        btn_Cancel.Text = "Cancel"
        btn_Cancel.Refresh()
        RunExport()
        If Cancelled Then
            Cancelled = False
            btn_Cancel.Text = "Close"
            btn_Cancel.Refresh()
            btn_Start.Enabled = True
        End If
    End Sub

    Private Sub btn_Cancel_Click( sender As Object,  e As EventArgs) Handles btn_Cancel.Click
        If btn_Cancel.Text = "Cancel" Then 
            Cancelled = True
            Application.DoEvents()
            Exit Sub
        End If
        Me.Close()
    End Sub

    Private Sub btn_FolderBrowse_Click( sender As Object,  e As EventArgs) Handles btn_FolderBrowse.Click
        Dim dialog As New FolderBrowserDialog()
        dialog.RootFolder = Environment.SpecialFolder.Desktop
        dialog.ShowNewFolderButton = True
        dialog.SelectedPath = "C:\"
        dialog.Description = "Select Path to save Exported data"
        If dialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            OutputFolder = dialog.SelectedPath
        Else
            Exit Sub
        End If
        OutputFolder = Validatefolder(OutputFolder)
        outputfolderchecked = If(IsNothing(OutputFolder), False, True)
        TextBox1.Text = OutputFolder
    End Sub

    Private Sub btn_Validate_Click( sender As Object,  e As EventArgs) Handles btn_Validate.Click
        If String.IsNullOrEmpty(TextBox1.Text) Then
            outputfolderchecked = False
            Exit Sub
        End If
        OutputFolder = Validatefolder(TextBox1.Text)
        Pref.ExportXBMCPath = OutputFolder 
        outputfolderchecked = If(IsNothing(OutputFolder), False, True)
        TextBox1.Text = OutputFolder
    End Sub

    Private Sub TextBox1_TextChanged( sender As Object,  e As EventArgs) Handles TextBox1.TextChanged
        If loading Then Exit Sub
        If outputfolderchecked Then Exit Sub
    End Sub

End Class