Imports System.Text.RegularExpressions
Imports Media_Companion.WorkingWithNfoFiles
Imports Media_Companion
Imports Media_Companion.Pref
Imports System.Linq
Imports System.ComponentModel

Partial Public Class Form1
    Public DataGridViewBindingSourceHM As New BindingSource
    Public Shared homemovielist As New List(Of str_BasicHomeMovie)
    Public Shared Property HMCache As New List(Of HmMovComboList)
    Public WorkingHomeMovie As New FullMovieDetails
    Public WorkingHomeVid As New HmMovComboList
    Dim LastHV As String = ""
    Dim HMGridFieldToDisplay1 As String = "TitleAndYear"
    Dim HMGridFieldToDisplay2 As String = "A-Z"
    Dim HMGridInvert As Boolean = False
    Private HMkeypresstimer As Timers.Timer = New Timers.Timer()
    Private HVKeyPress As String = ""
    Public hmfolderschanged As Boolean = False
    Private HDGV1RowSelected As Integer = -1
    Const HomeCacheVersion As String = "1.1"


#Region "Browser Tab"

    Private Sub PlayHomeMovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayHomeMovieToolStripMenuItem.Click
        mov_Play("HomeMovie")
    End Sub

    Private Sub OpenFolderToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFolderToolStripMenuItem.Click
        Try
            If Not WorkingHomeMovie.fileinfo.fullpathandfilename Is Nothing Then
                Call util_OpenFolder(WorkingHomeMovie.fileinfo.fullpathandfilename)
            Else
                MsgBox("There is no Movie selected to open")
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub OpenFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFileToolStripMenuItem.Click
        Try
            Utilities.NfoNotepadDisplay(WorkingHomeMovie.fileinfo.fullpathandfilename, Pref.altnfoeditor)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub pbx_HmFanart_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbx_HmFanart.DoubleClick
        Try
            If WorkingHomeMovie.fileinfo.fanartpath <> Nothing Then
                If File.Exists(WorkingHomeMovie.fileinfo.fanartpath) Then
                    Me.ControlBox = False
                    MenuStrip1.Enabled = False
                    util_ZoomImage(WorkingHomeMovie.fileinfo.fanartpath)
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub pbx_HmPoster_DoubleClick1(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbx_HmPoster.DoubleClick
        Try
            If WorkingHomeMovie.fileinfo.posterpath <> Nothing Then
                If File.Exists(WorkingHomeMovie.fileinfo.posterpath) Then
                    Me.ControlBox = False
                    MenuStrip1.Enabled = False
                    util_ZoomImage(WorkingHomeMovie.fileinfo.posterpath)
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub SearchForNewHomeMoviesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchForNewHomeMoviesToolStripMenuItem.Click, btn_HMSearch.Click
        Call SearchNewHomeVideo()
    End Sub

    Private Sub RebuildHomeMovieCacheToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RebuildHomeMovieCacheToolStripMenuItem.Click, btn_HMRefresh.Click
        Call RefreshHomeVideoCache()
    End Sub
    
    Private Sub btnHomeMovieSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHomeMovieSave.Click
        If HmMovTitle.Text <> "" Then
            WorkingHomeMovie.fullmoviebody.title = HmMovTitle.Text
        End If
        If HmMovSort.Text <> "" Then
            WorkingHomeMovie.fullmoviebody.sortorder = HmMovSort.Text
        End If
        WorkingHomeMovie.fullmoviebody.year     = HmMovYear.Text
        WorkingHomeMovie.fullmoviebody.plot     = HmMovPlot.Text
        WorkingHomeMovie.fullmoviebody.stars    = HmMovStars.Text
        WorkingHomeMovie.fullmoviebody.genre    = HmMovGenre.Text
        WorkingHomeMovie.fullmoviebody.tag.Clear()
        If Not IsNothing(HmMovTag.Text) Then
            Dim p() As String = HmMovTag.Text.Split(",")
            For each t In p
                WorkingHomeMovie.fullmoviebody.tag.Add(t.Trim())
            Next
        End If
        nfoSaveHomeMovie(WorkingHomeMovie.fileinfo.fullpathandfilename, WorkingHomeMovie)
    End Sub
    
    Private Sub HmMovGenre_MouseDown(sender As Object, e As MouseEventArgs) Handles HmMovGenre.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Try
                Dim item() As String = WorkingHomeMovie.fullmoviebody.genre.Split("/")
                Dim genre As String = ""
                Dim listof As New List(Of str_genre)
                listof.Clear()
                For Each i In item
                    If i = "" Then Continue For
                    Dim g As str_genre
                    g.genre = i.Trim
                    g.count = 1
                    listof.Add(g)
                Next
                Dim frm As New frmGenreSelect
                frm.multicount = 1
                frm.SelectedGenres = listof
                frm.Init()
                If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    listof.Clear()
                    listof.AddRange(frm.SelectedGenres)
                    For each g In listof
                        If g.count = 0 Then Continue For
                        If genre = "" Then
                            genre = g.genre
                        Else
                            genre += " / " & g.genre
                        End If
                    Next
                    WorkingHomeMovie.fullmoviebody.genre = genre
                    HmMovGenre.Text = genre
                    nfoSaveHomeMovie(WorkingHomeMovie.fileinfo.fullpathandfilename, WorkingHomeMovie, True)
                End If
            Catch
            End Try
        End If
    End Sub

#End Region

#Region "Home fanart"

    Private Sub btn_HmFanartShot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_HmFanartShot.Click
        HmScreenshot_Save()
    End Sub

    Private Sub tb_HmFanartTime_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tb_HmFanartTime.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            If tb_HmFanartTime.Text <> "" AndAlso Convert.ToInt32(tb_HmFanartTime.Text) > 0 Then
                HmScreenshot_Load()
            End If
        End If
        If Char.IsNumber(e.KeyChar) = False And e.KeyChar <> Chr(8) Then
            e.Handled = True
        End If
    End Sub

    Private Sub tb_HmFanartTime_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_HmFanartTime.Leave
        If tb_HmFanartTime.Text = "" Then
            MsgBox("Please enter a numerical value >0 into the textbox")
            tb_HmFanartTime.Focus()
        ElseIf Convert.ToInt32(tb_HmFanartTime.Text) = 0 Then
            MsgBox("Please enter a numerical value >0 into the textbox")
            tb_HmFanartTime.Focus()
        End If
    End Sub

    Private Sub btn_HmFanartGet_Click(sender As Object, e As EventArgs) Handles btn_HmFanartGet.Click
        HmScreenshot_Load()
    End Sub

    Private Sub HmScreenshot_Save()
        Try
            pbx_HmFanart.Image = Nothing
            Dim screenshotpath As String = WorkingHomeMovie.fileinfo.fanartpath
            If screenshotpath = "" Then screenshotpath = WorkingHomeMovie.fileinfo.fullpathandfilename.Replace(".nfo", "-fanart.jpg")
            Dim cachepathandfilename As String = pbx_HmFanartSht.Tag.ToString
            File.Copy(cachepathandfilename, screenshotpath, True)
            util_ImageLoad(pbx_HmFanart, cachepathandfilename, Utilities.DefaultTvFanartPath)
            Dim video_flags = VidMediaFlags(WorkingHomeMovie.filedetails)
            movieGraphicInfo.OverlayInfo(pbx_HmFanart, "", video_flags)
        Catch
        End Try
    End Sub

    Private Sub HmScreenshot_Load()
        Dim Cachename As String = HmGetScreenShot()
        If Cachename = "" Then
            MsgBox("Unable to get screenshots from HomeVideo file")
            Exit Sub
        End If
        Try
            Dim matches() As Control
            For i = 0 To 4
                matches = Me.Controls.Find("pbHmScrSht" & i, True)
                If matches.Length > 0 Then
                    Dim pb As PictureBox = DirectCast(matches(0), PictureBox)
                    pb.SizeMode = PictureBoxSizeMode.StretchImage
                    Dim image2load As String = Cachename.Substring(0, Cachename.Length - 5) & i.ToString & ".jpg"
                    util_ImageLoad(pb, image2load, Utilities.DefaultTvFanartPath)
                End If
            Next
            If Not IsNothing(pbHmScrSht0.Image) Then util_ImageLoad(pbx_HmFanartSht, pbHmScrSht0.Tag.ToString, Utilities.DefaultTvFanartPath)
        Catch
        End Try
    End Sub

    Private Function HmGetScreenShot() As String
        Try
            If tb_HmFanartTime.Text = "" OrElse Convert.ToInt32(tb_HmFanartTime.Text) < 1 Then tb_HmFanartTime.Text = Pref.HmFanartTime.ToString
            If IsNumeric(tb_HmFanartTime.Text) Then
                Dim path As String = WorkingHomeMovie.fileinfo.fullpathandfilename.Replace(".nfo", "-fanart.jpg")
                Dim tempstring2 As String = WorkingHomeMovie.fileinfo.filenameandpath
                If File.Exists(tempstring2) Then
                    Dim seconds = Convert.ToInt32(tb_HmFanartTime.Text)
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    Application.DoEvents()
                    Dim cachepathandfilename As String = Utilities.CreateScrnShotToCache(tempstring2, path, seconds, 5, 10)
                    If cachepathandfilename <> "" Then
                        'if vro, check width & height as can be reversed.
                        If tempstring2.ToLower.Contains(".vro") AndAlso WorkingHomeMovie.filedetails.Video.Height.Value > WorkingHomeMovie.filedetails.Video.Width.Value Then
                            Resizescreenshots(cachepathandfilename)
                        End If

                        Return cachepathandfilename
                    End If
                End If
            End If
        Catch
        End Try
        Return ""
    End Function

    Private Sub pbHmScrSht_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim pb As PictureBox = sender
        If IsNothing(pb.Image) Then Exit Sub
        Form1.util_ImageLoad(pbx_HmFanartSht, pb.Tag, Utilities.DefaultTvFanartPath)
    End Sub

    Private Sub Resizescreenshots(ByVal path As String)
        For i = 0 To 4
            Dim image2load As String = path.Substring(0, path.Length - 5) & i.ToString & ".jpg"
            If IO.File.Exists(image2load) Then Utilities.ResizeImage(image2load, WorkingHomeMovie.filedetails.Video.Height.Value, WorkingHomeMovie.filedetails.Video.Width.Value)
        Next
    End Sub
#End Region

#Region "Home poster"

    Private Sub btn_HmPosterShot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_HmPosterShot.Click
        Try
            If IsNumeric(tb_HmPosterTime.Text) Then
                Dim thumbpathandfilename As String = Path.Combine(Utilities.CacheFolderPath, WorkingHomeMovie.fileinfo.posterpath.Replace(WorkingHomeMovie.fileinfo.path, ""))
                Dim pathandfilename As String = WorkingHomeMovie.fileinfo.fullpathandfilename.Replace(".nfo", "")
                messbox = New frmMessageBox("ffmpeg is working to capture the desired screenshot", "", "Please Wait")
                Dim aok As Boolean = False
                For Each ext In Utilities.VideoExtensions
                    Dim tempstring2 As String = pathandfilename & ext
                    If File.Exists(tempstring2) Then
                        pathandfilename = tempstring2
                        aok = True
                        Exit For
                    End If
                Next
                If aok Then
                    Dim seconds As Integer = 10
                    If Convert.ToInt32(tb_HmPosterTime.Text) > 0 Then
                        seconds = Convert.ToInt32(tb_HmPosterTime.Text)
                    End If
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    messbox.Show()
                    messbox.Refresh()
                    Application.DoEvents()
                    aok = Utilities.CreateScreenShot(pathandfilename, thumbpathandfilename, seconds, True)
                    messbox.Close()
                    If aok Then
                        Dim cancelclicked As Boolean
                        Using pbx As New PictureBox
                            util_ImageLoad(pbx, thumbpathandfilename, Utilities.DefaultPosterPath)
                            Using t As New frmMovPosterCrop
                                If Pref.MultiMonitoEnabled Then
                                    t.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
                                    t.StartPosition = FormStartPosition.Manual
                                End If
                                t.img = pbx.Image
                                t.cropmode = "poster"
                                t.title = WorkingHomeMovie.fullmoviebody.title
                                t.Setup()
                                t.ShowDialog()
                                If Not IsNothing(t.newimg) Then
                                    Utilities.SaveImage(t.newimg, WorkingHomeMovie.fileinfo.posterpath)
                                Else
                                    cancelclicked = True
                                End If
                            End Using
                        End Using

                        Utilities.SafeDeleteFile(thumbpathandfilename)

                        If File.Exists(WorkingHomeMovie.fileinfo.posterpath) Then
                            Try
                                util_ImageLoad(pbx_HmPosterSht, WorkingHomeMovie.fileinfo.posterpath, Utilities.DefaultFanartPath)
                                util_ImageLoad(pbx_HmPoster, WorkingHomeMovie.fileinfo.posterpath, Utilities.DefaultFanartPath)
                            Catch
                            End Try
                        End If
                    Else
                        MsgBox("Failed to get ScreenShot")
                    End If
                End If
                If Not IsNothing(messbox) Then messbox.Close()
            Else
                MsgBox("Please enter a numerical value into the textbox")
                tb_HmPosterTime.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        Finally
            If Not IsNothing(messbox) Then messbox.Close()
        End Try

    End Sub

#End Region

#Region "Home folders"

    Private Sub tp_HmFolders_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tp_HmFolders.Leave
        Try
            If hmfolderschanged Then
                Dim save = MsgBox("You have made changes to some folders" & vbCrLf & "    Do you wish to save these changes?", MsgBoxStyle.YesNo)
                If save = DialogResult.Yes Then Call HomeMovieFoldersRefresh()
                hmfolderschanged = False
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btn_HmFolderSaveRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_HmFolderSaveRefresh.Click
        Call HomeMovieFoldersRefresh()
        hmfolderschanged = False
    End Sub

    Private Sub btnHomeFolderAdd_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHomeFolderAdd.Click
        Try
            Dim allok As Boolean = True
            Dim thefoldernames As String
            fb.Description = "Please Select Folder to Add to DB (Subfolders will also be added)"
            fb.ShowNewFolderButton = True
            fb.RootFolder = System.Environment.SpecialFolder.Desktop
            fb.SelectedPath = Pref.lastpath
            Tmr.Start()
            If fb.ShowDialog = Windows.Forms.DialogResult.OK Then
                thefoldernames = (fb.SelectedPath)
                Pref.lastpath = thefoldernames
                For Each item As Object In clbx_HMMovieFolders.Items
                    If thefoldernames.ToString = item.ToString Then allok = False
                Next

                If allok = True Then
                    AuthorizeCheck = True
                    clbx_HMMovieFolders.Items.Add(thefoldernames, True)
                    clbx_HMMovieFolders.Refresh()
                    AuthorizeCheck = False
                Else
                    MsgBox("        Folder Already Exists")
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnHomeFoldersRemove_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHomeFoldersRemove.Click
        Try
            While clbx_HMMovieFolders.SelectedItems.Count > 0
                clbx_HMMovieFolders.Items.Remove(clbx_HMMovieFolders.SelectedItems(0))
            End While
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnHomeManualPathAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnHomeManualPathAdd.Click
        Try
            If tbHomeManualPath.Text = Nothing Then
                Exit Sub
            End If
            If tbHomeManualPath.Text = "" Then
                Exit Sub
            End If
            Dim tempstring As String = tbHomeManualPath.Text
            Do While tempstring.LastIndexOf("\") = tempstring.Length - 1
                tempstring = tempstring.Substring(0, tempstring.Length - 1)
            Loop
            Do While tempstring.LastIndexOf("/") = tempstring.Length - 1
                tempstring = tempstring.Substring(0, tempstring.Length - 1)
            Loop
            Dim exists As Boolean = False
            For Each item In clbx_HMMovieFolders.Items
                If (item.ToString.Equals(tempstring, StringComparison.InvariantCultureIgnoreCase)) Then
                    exists = True
                    Exit For
                End If
            Next
            If exists = True Then
                MsgBox("        Folder Already Exists")
            Else
                Dim f As New DirectoryInfo(tempstring)
                If f.Exists Then
                    AuthorizeCheck = True
                    clbx_HMMovieFolders.Items.Add(tempstring, True)
                    clbx_HMMovieFolders.Refresh()
                    AuthorizeCheck = False
                    tbHomeManualPath.Text = ""
                Else
                    Dim tempint As Integer = MessageBox.Show("This folder does not appear to exist" & vbCrLf & "Are you sure you wish to add it", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If tempint = DialogResult.Yes Then
                        AuthorizeCheck = True
                        clbx_HMMovieFolders.Items.Add(tempstring, True)
                        clbx_HMMovieFolders.Refresh()
                        AuthorizeCheck = False
                        tbHomeManualPath.Text = ""
                    End If
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub clbx_HMMovieFolders_DragDrop(sender As Object, e As DragEventArgs) Handles clbx_HMMovieFolders.DragDrop
        Dim folders() As String
        droppedItems.Clear()
        folders = e.Data.GetData(DataFormats.FileDrop)
        For f = 0 To UBound(folders)
            Dim exists As Boolean = False
            For Each rtpath In Pref.homemoviefolders
                If rtpath.rpath = folders(f) Then
                    exists = True
                    Exit For
                End If
            Next
            If exists OrElse clbx_HMMovieFolders.Items.Contains(folders(f)) Then Continue For
            Dim skip As Boolean = False
            For Each item In droppedItems
                If item = folders(f) Then
                    skip = True
                    Exit For
                End If
            Next
            If Not skip Then droppedItems.Add(folders(f))
        Next
        If droppedItems.Count < 1 Then Exit Sub
        AuthorizeCheck = True
        For Each item In droppedItems
            clbx_HMMovieFolders.Items.Add(item, True)
            hmfolderschanged = True
        Next
        AuthorizeCheck = False
        clbx_HMMovieFolders.Refresh()
    End Sub

    Private Sub clbx_HMMovieFolders_DragEnter(sender As Object, e As DragEventArgs) Handles clbx_HMMovieFolders.DragEnter
        Try
            e.Effect = DragDropEffects.Copy
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub clbx_HMMovieFolders_KeyPress(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles clbx_HMMovieFolders.KeyDown
        If e.KeyCode = Keys.Delete AndAlso clbx_MovieRoots.SelectedItem <> Nothing Then
            Call btnHomeFoldersRemove.PerformClick()
        ElseIf e.KeyCode = Keys.Space Then
            AuthorizeCheck = True
            Call clbx_hmmoviefolderstoggle()
            AuthorizeCheck = False
        End If
    End Sub

    Private Sub clbx_HMMovieFolders_MouseDown(sender As Object, e As MouseEventArgs) Handles clbx_HMMovieFolders.MouseDown
        Dim loc As Point = Me.clbx_HMMovieFolders.PointToClient(Cursor.Position)
        For i As Integer = 0 To Me.clbx_HMMovieFolders.Items.Count - 1
            Dim rec As Rectangle = Me.clbx_HMMovieFolders.GetItemRectangle(i)
            rec.Width = 16
            'checkbox itself has a default width of about 16 pixels
            If rec.Contains(loc) Then
                AuthorizeCheck = True
                Dim newValue As Boolean = Not Me.clbx_HMMovieFolders.GetItemChecked(i)
                Me.clbx_HMMovieFolders.SetItemChecked(i, newValue)
                AuthorizeCheck = False
                Return
            End If
        Next
    End Sub

    Private Sub clbx_HMMovieFolders_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbx_HMMovieFolders.ItemCheck
        If Not AuthorizeCheck Then
            e.NewValue = e.CurrentValue
            Exit Sub
        End If
        hmfolderschanged = True
    End Sub

    Private Sub clbx_hmmoviefolderstoggle()
        Dim i = clbx_HMMovieFolders.SelectedIndex
        clbx_HMMovieFolders.SetItemCheckState(i, If(clbx_HMMovieFolders.GetItemCheckState(i) = CheckState.Checked, CheckState.Unchecked, CheckState.Checked))
    End Sub

    Private Sub HomeFoldersUpdate()
        AuthorizeCheck = True
        clbx_HMMovieFolders.Items.Clear()
        For Each item In Pref.homemoviefolders
            clbx_HMMovieFolders.Items.Add(item.rpath, item.selected)
        Next
        AuthorizeCheck = False
        hmfolderschanged = False
    End Sub
#End Region

    Private Sub SearchNewHomeVideo()
        Pref.HomeVidScrape = True
        RunBackgroundMovieScrape("HomeVidScrape")
        While BckWrkScnMovies.IsBusy
            Application.DoEvents()
        End While
        Pref.HomeVidScrape = False
        HomeMovieCacheSave()
        HMDGVload()
    End Sub

    Private Sub RefreshHomeVideoCache()
        Pref.HomeVidScrape = True
        RunBackgroundMovieScrape("HomeVidRebuild")
        While BckWrkScnMovies.IsBusy
            Application.DoEvents()
        End While
        Pref.HomeVidScrape = False
        HomeMovieCacheSave()
        HMDGVload()
    End Sub
    
    Public Sub HomeMovieCacheSave()
        Dim fullpath As String = Pref.workingProfile.HomeMovieCache
        If File.Exists(fullpath) Then
            Dim aok As Boolean = Utilities.SafeDeleteFile(fullpath)
            If Not aok Then
                MsgBox(" Error Overwriting existing HomeVideo Cache! ")
                Exit Sub
            End If
        End If

        Dim doc As New XmlDocument
        Dim xmlproc As XmlDeclaration
        Dim root As XmlElement
        Dim child As XmlElement
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        root = doc.CreateElement("homemovie_cache")
        Dim attr As XmlAttribute = doc.CreateAttribute("ver")
        attr.Value = HomeCacheVersion
        root.SetAttributeNode(attr)

        For Each item In HMCache
            child = doc.CreateElement("movie")
            child.AppendChild(doc, "fullpathandfilename"    , item.nfopathandfilename)
            child.AppendChild(doc, "filename"               , item.filename)
            child.AppendChild(doc, "foldername"             , item.foldername)
            child.AppendChild(doc, "title"                  , item.title)
            child.AppendChild(doc, "year"                   , item.year)
            child.AppendChild(doc, "filedate"               , item.filedate)
            child.AppendChild(doc, "createdate"             , item.createdate)
            child.AppendChild(doc, "genre"                  , item.genre)
            child.AppendChild(doc, "plot"                   , item.plot)
            child.AppendChild(doc, "playcount"              , item.playcount)
            child.AppendChild(doc, "runtime"                , item.runtime)
            child.AppendChild(doc, "Resolution"             , item.Resolution)
            child.AppendChild(doc, "FrodoPosterExists"      , item.FrodoPosterExists)
            child.AppendChild(doc, "PreFrodoPosterExists"   , item.PreFrodoPosterExists)
            For Each track In item.Audio
                child.AppendChild(track.GetChild(doc))
            Next
            root.AppendChild(child)
        Next
        doc.AppendChild(root)

        SaveXMLDoc(doc, fullpath)
    End Sub

    Private Function HomeMovieCacheLoad() As Boolean
        Dim aok As Boolean = True
        Dim movielist As New XmlDocument
        Dim tempstring As String = Nothing
        Using objReader As IO.StreamReader = File.OpenText(workingProfile.HomeMovieCache)
            tempstring = objReader.ReadToEnd
        End Using

        movielist.LoadXml(tempstring)
        Dim root as XmlElement = movielist.DocumentElement
        If root.HasAttribute("ver") Then
            Dim docversion As String = root.GetAttribute("ver")
            If docversion <> HomeCacheVersion Then Return False
        Else
            Return False
        End If
        For Each thisresult As XmlNode In movielist("homemovie_cache")
            Select Case thisresult.Name
                Case "movie"
                    Dim newmovie As New HmMovComboList
                    For Each detail As XmlNode In thisresult.ChildNodes
                        Select Case detail.Name
                            Case "fullpathandfilename"      : newmovie.nfopathandfilename   = detail.InnerText
                            Case "title"                    : newmovie.Title                = detail.InnerText
                            Case "year"                     : newmovie.year                 = detail.InnerText
                            Case "filename"                 : newmovie.filename             = detail.InnerText
                            Case "foldername"               : newmovie.foldername           = detail.InnerText
                            Case "genre"                    : newmovie.genre                = detail.InnerText
                            Case "plot"                     : newmovie.plot                 = detail.InnerText
                            Case "playcount"                : newmovie.playcount            = detail.InnerText
                            Case "runtime"                  : newmovie.runtime              = detail.InnerText
                            Case "Resolution"               : newmovie.Resolution           = detail.InnerText
                            Case "FrodoPosterExists"        : newmovie.FrodoPosterExists    = detail.InnerText
                            Case "PreFrodoPosterExists"     : newmovie.PreFrodoPosterExists = detail.InnerText
                            Case "audio"
                                Dim audio As New AudioDetails
                                For Each audiodetails As XmlNode In detail.ChildNodes
                                    Select Case audiodetails.Name
                                        Case "language" : audio.Language.Value  = audiodetails.InnerText
                                        Case "codec"    : audio.Codec.Value     = audiodetails.InnerText
                                        Case "channels" : audio.Channels.Value  = audiodetails.InnerText
                                        Case "bitrate"  : audio.Bitrate.Value   = audiodetails.InnerText
                                    End Select
                                Next
                            Case "createdate"
                                If detail.InnerText.Length <> 14 Then 'i.e. invalid date
                                    newmovie.CreateDate = "18500101000000" '01/01/1850 00:00:00
                                Else
                                    newmovie.CreateDate = detail.InnerText
                                End If
                            Case "filedate"
                                If detail.InnerText.Length <> 14 Then 'i.e. invalid date
                                    newmovie.filedate = "18500101000000" '01/01/1850 00:00:00
                                Else
                                    newmovie.filedate = detail.InnerText
                                End If
                        End Select
                    Next
                    HMCache.Add(newmovie)
            End Select
            
        Next
        Call HMDGVload()
        Try
            If HVDgv1.Rows.Count > 0 Then HVDgv1.Rows(0).Selected = True
            DisplayHV()
        Catch ex As Exception
        End Try
        Return aok
    End Function

    Private Sub DisplayHV()
        Try
            DisplayMV(HVDgv1.SelectedCells, HVDgv1.selectedrows)
        Catch
            Return
        End Try
    End Sub

    Private Sub DisplayMV(ByVal selectedCells As DataGridViewSelectedCellCollection, ByVal selectedRows As DataGridViewSelectedRowCollection)
        Try
            If selectedRows.Count = 1 Then
                If LastHV = selectedCells(0).Value.ToString Then Return
                LastHV = selectedCells(0).Value.ToString
            Else
                LastHV = ""
            End If
        Catch
            Return
        End Try
        If selectedRows.Count = 0 Then Exit Sub
        HVForm_Init()
        If selectedRows.Count = 1 Then
            pbx_HmFanart.Visible = True
            pbx_HmPoster.Visible = True
            Dim query = From f In HMCache Where f.nfopathandfilename = selectedCells(0).Value.ToString
            Dim queryList As List(Of HmMovComboList) = query.ToList()

            If queryList.Count > 0 Then
                WorkingHomeVid.nfopathandfilename = queryList(0).nfopathandfilename
                HMForm_Populate()
            Else
                HMForm_Populate()
            End If
        End If

    End Sub

    Private Sub HVForm_Init()
        pbx_HmFanart.Image = Nothing
        pbx_HmFanart.Visible = False
        pbx_HmPoster.Image = Nothing
        pbx_HmPoster.Visible = False
        HmMovTitle  .Text = ""
        HmMovSort   .Text = ""
        HmMovYear   .Text = ""
        HmMovPlot   .Text = ""
        HmMovStars  .Text = ""
        HmMovGenre  .Text = ""
        HmMovTag    .Text = ""
        HmMovPath   .Text = ""
    End Sub

    Private Sub HMForm_Populate()
        If Not String.IsNullOrEmpty(WorkingHomeVid.nfopathandfilename) AndAlso HVDgv1.Rows.Count > 0 Then
            WorkingHomeMovie = nfoLoadHomeMovie(WorkingHomeVid.nfopathandfilename)
            If IsNothing(WorkingHomeMovie) = False Then
                HmMovTitle  .Text = WorkingHomeMovie.fullmoviebody.title
                HmMovSort   .Text = WorkingHomeMovie.fullmoviebody.sortorder
                HmMovPlot   .Text = WorkingHomeMovie.fullmoviebody.plot
                HmMovStars  .Text = WorkingHomeMovie.fullmoviebody.stars
                HmMovYear   .Text = WorkingHomeMovie.fullmoviebody.year
                HmMovGenre  .Text = WorkingHomeMovie.fullmoviebody.genre
                HmMovPath   .Text = WorkingHomeMovie.fileinfo.fullpathandfilename
                If WorkingHomeMovie.fullmoviebody.tag.Count > 0 Then
                    HmMovTag.Text = String.Join(", ", WorkingHomeMovie.fullmoviebody.tag.ToArray)
                End If

                'Dim runtimestr As String = WorkingHomeMovie.fullmoviebody.runtime
                'If runtimestr = "" OrElse runtimestr = "-1" Then
                '    If WorkingHomeMovie.filedetails.Video.DurationInSeconds.Value <> "" AndAlso WorkingHomeMovie.filedetails.Video.DurationInSeconds.Value <> "-1" Then
                '        runtimestr = Math.Floor((WorkingHomeMovie.filedetails.Video.DurationInSeconds.Value.ToInt) / 60).ToString & " min"
                '    End If
                'Else
                '    runtimestr &= " min"
                'End If

                util_ImageLoad(pbx_HmFanart, WorkingHomeMovie.fileinfo.fanartpath, Utilities.DefaultFanartPath)
                util_ImageLoad(pbx_HmFanartSht, WorkingHomeMovie.fileinfo.fanartpath, Utilities.DefaultFanartPath)

                'Set Media overlay
                Dim video_flags = VidMediaFlags(WorkingHomeMovie.filedetails)
                movieGraphicInfo.OverlayInfo(pbx_HmFanart, "", video_flags)

                If File.Exists(WorkingHomeMovie.fileinfo.posterpath) Then
                    util_ImageLoad(pbx_HmPoster, WorkingHomeMovie.fileinfo.posterpath, Utilities.DefaultFanartPath)
                    util_ImageLoad(pbx_HmPosterSht, WorkingHomeMovie.fileinfo.posterpath, Utilities.DefaultFanartPath)
                End If
            End If
        End If
    End Sub

    Private Sub HMDGVload()
        HVDgv1.DataSource = Nothing
        HVDgv1.DataSource = HmCache
        HMDGVGridviewMovieDesign()
        HMDGV_FiltersAndSortApply()
        DisplayHV()
    End Sub

    Public Sub HMDGVGridviewMovieDesign()
        If Not MainFormLoadedStatus Then Exit Sub
        Dim dgv As DataGridView = HVDgv1
        If dgv.Columns.Count < 27 Then Return
        Cursor.Current = Cursors.WaitCursor
        While dgv.Columns(0).CellType.Name = "DataGridViewImageCell"
            dgv.Columns.Remove(dgv.Columns(0))
        End While

        Dim header_style As New DataGridViewCellStyle
        header_style.ForeColor = Color.White
        header_style.BackColor = Color.ForestGreen
        header_style.Font = new Font(dgv.Font, FontStyle.Bold)
        For Each col As DataGridViewcolumn in dgv.Columns
            col.HeaderCell.Style = header_style
        Next
        dgv.EnableHeadersVisualStyles = False
        For Each column As DataGridViewColumn In dgv.Columns
            column.Resizable = DataGridViewTriState.True
            column.ReadOnly = True
            column.SortMode = DataGridViewColumnSortMode.Automatic
            column.Visible = False
        Next

        'Highlight titles in datagridview with missing video files.
        If Pref.incmissingmovies Then
            For Each row As DataGridViewRow In dgv.Rows
                If row.Cells("videomissing").Value = True Then
                    row.DefaultCellStyle.BackColor = Color.Red
                End If
            Next
        End If
        dgv.RowHeadersVisible = False

        If HMGridFieldToDisplay1 = "TitleAndYear" Then
            HMDGVIniColumn(dgv, "Title", HMGridFieldToDisplay2 = "Year", "Title")
            HMDGVIniColumn(dgv, "TitleYear", HMGridFieldToDisplay2 <> "Year", "Title & Year")
        End If
        HMDGVIniColumn(dgv, "filename"          , HMGridFieldToDisplay1 = "FileName"    , "File name")
        HMDGVIniColumn(dgv, "year"              , HMGridFieldToDisplay2 = "Year"        , "Movie year"  , "Year"    , -20)
        HMDGVIniColumn(dgv, "runtime"           , HMGridFieldToDisplay2 = "Runtime"     , "Runtime"     ,           , -20   , DataGridViewContentAlignment.MiddleRight)
        HMDGVIniColumn(dgv, "DisplayCreateDate" , HMGridFieldToDisplay2 = "Date Added"  , "Date Added"  , "Added")
        HMDGVIniColumn(dgv, "Resolution"        , HMGridFieldToDisplay2 = "Resolution"  , "Resolution"  , "Res"     ,       , DataGridViewContentAlignment.MiddleRight)
        HmDGVSetFirstColumnWidth(dgv)
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub HMDGVIniColumn(dgv As DataGridView, name As String, visible As Boolean, Optional toolTip As String = Nothing, Optional headerText As String = Nothing, Optional widthAdjustment As Integer = 0, Optional alignment As DataGridViewContentAlignment = Nothing)

        Dim col As DataGridViewColumn = dgv.Columns(name)
        If IsNothing(toolTip) Then toolTip = Utilities.TitleCase(name)  'CapsFirstLetter(name)
        col.Visible = visible
        col.ToolTipText = toolTip
        col.HeaderText = If(IsNothing(headerText), toolTip, headerText)
        HmDGVSetColWidth(col, widthAdjustment)

        If Not IsNothing(alignment) Then
            Dim header_style As New DataGridViewCellStyle
            header_style.ForeColor = Color.White
            header_style.BackColor = Color.ForestGreen
            header_style.Font = new Font(dgv.Font, FontStyle.Bold)
            header_style.Alignment = alignment
            col.HeaderCell.Style = header_style
            col.DefaultCellStyle.Alignment = alignment
        End If
    End Sub

    Sub HmDGVSetColWidth(col As DataGridViewColumn, Optional widthAdjustment As Integer = 0)
        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells    'Set auto-size mode
        Dim initialAutoSizeWidth As Integer = col.Width                 'Save calculated width after auto-sizing
        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet        'Revert sizing mode to default
        col.Width = initialAutoSizeWidth + widthAdjustment                'Set width to calculated auto-size - adjustment needed because header has excess padding
    End Sub

    Sub HmDGVSetFirstColumnWidth(dgvHomMov As DataGridView)
        Try
            Dim firstColWidth As Integer = dgvHomMov.Width - 17

            If Not IsNothing(dgvHomMov.Columns("Watched")) AndAlso dgvHomMov.Columns("Watched").Visible then firstColWidth -= dgvHomMov.Columns("Watched").Width

            If HMGridFieldToDisplay2 = "Year"       Then firstColWidth -= dgvHomMov.Columns("Year")                 .Width
            If HMGridFieldToDisplay2 = "Runtime"    Then firstColWidth -= dgvHomMov.Columns("runtime")              .Width
            If HMGridFieldToDisplay2 = "Date Added" Then firstColWidth -= dgvHomMov.Columns("DisplayCreateDate")    .Width
            If HMGridFieldToDisplay2 = "Resolution" Then firstColWidth -= dgvHomMov.Columns("Resolution"         )  .Width

            If firstColWidth > 0 Then
                If Not IsNothing(dgvHomMov.Columns("filename")) Then dgvHomMov.Columns("filename").Width = firstColWidth
                If Not IsNothing(dgvHomMov.Columns("ArtistTitle")) Then dgvHomMov.Columns("ArtistTitle").Width = firstColWidth
                If Not IsNothing(dgvHomMov.Columns("ArtistTitleYear")) Then dgvHomMov.Columns("ArtistTitleYear").Width = firstColWidth
                If Not IsNothing(dgvHomMov.Columns("Title")) Then dgvHomMov.Columns("Title").Width = firstColWidth
                If Not IsNothing(dgvHomMov.Columns("TitleYear")) Then dgvHomMov.Columns("TitleYear").Width = firstColWidth
            End If
        Catch
        End Try
    End Sub

    Public Sub HMDGV_FiltersAndSortApply(Optional ByVal Force As Boolean = False) 'Form1 As Form1)
        If Not MainFormLoadedStatus AndAlso Not Force Then Exit Sub

        Dim b = From f In HMCache
        'If txtFilter.Text.ToUpper<>"" Then
        '    If rbMVArtistAndTitle.Checked Then
        '        b = From f In b Where f.ArtistTitle.ToUpper.Contains(txtFilter.Text.ToUpper)
        '    ElseIf rbMVTitleandYear.Checked Then
        '        b = From f In b Where f.TitleUcase.Contains(txtFilter.Text.ToUpper)
        '    ElseIf Form1.rbFileName.Checked Then
        '        b = From f In b Where f.filename.ToLower.Contains(txtFilter.Text.ToLower)
        '    End If
        'End If

        Select Case cmbxHMSort.Text
            Case "A - Z"
                If Not HMGridInvert Then
                    b = From f In b Order By f.Title Ascending
                Else
                    b = From f In b Order By f.Title Descending
                End If
            Case "Year"
                If Not HMGridInvert Then
                    b = From f In b Order By f.year Ascending
                Else
                    b = From f In b Order By f.year Descending
                End If
            Case "Runtime"
                If Not HMGridInvert Then
                    b = From f In b Order By f.IntRuntime Ascending
                Else
                    b = From f In b Order By f.IntRuntime Descending
                End If
            Case "Date Added"
                If Not HMGridInvert Then
                    b = From f In b Order By f.Createdate Descending
                Else
                    b = From f In b Order By f.Createdate Ascending
                End If
            Case "Resolution"
                If HMGridInvert Then
                    b = From f In b Order By f.Resolution Ascending, f.Title 
                Else
                    b = From f In b Order By f.Resolution Descending, f.Title 
                End If
        End Select

        Dim lst = b.ToList
        DataGridViewBindingSourceHM.DataSource = lst
        HVDgv1.DataSource = DataGridViewBindingSourceHM
        HMDGVGridviewMovieDesign()
    End Sub

    Private Sub HomeMovieFoldersRefresh()
        AuthorizeCheck = True
        Pref.homemoviefolders.Clear()
        For f = 0 To clbx_HMMovieFolders.Items.Count - 1
            Dim t As New str_RootPaths
            t.rpath = clbx_HMMovieFolders.Items(f).ToString
            Dim chkstate As CheckState = clbx_HMMovieFolders.GetItemCheckState(f)
            t.selected = (chkstate = CheckState.Checked)
            Pref.homemoviefolders.Add(t)
        Next
        Call ConfigSave()
        Call RefreshHomeVideoCache()
        AuthorizeCheck = False
        hmfolderschanged = False
        TabControl1.SelectedIndex = 0
    End Sub

#Region "MVDgv1 Handlers"

    Private Sub HVDgv1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles HVDgv1.CellClick
        DisplayHV()
    End Sub

    Private Sub HVDgv1_KeyUp(sender As Object, e As KeyEventArgs) Handles HVDgv1.KeyUp
        DisplayHV()
    End Sub

    Private Sub HVDgv1_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles HVDgv1.ColumnHeaderMouseClick
        HMGridInvert = Not HMGridInvert
        HMDGV_FiltersAndSortApply()
        DisplayHV()
    End Sub

    Private Sub HVDgv1_DoubleClick(sender As Object, e As MouseEventArgs) Handles HVDgv1.DoubleClick
        Try
            Dim info = HVDgv1.HitTest(e.X, e.Y)
            If info.ColumnX = -1 Then Return
            Try
                If IsNumeric(HVDgv1.SelectedCells(1).Value.ToString) Then Return
            Catch
                Return
            End Try
            If info.Type <> DataGridViewHitTestType.ColumnHeader Then
                HVPlay()
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub HVDgv1_MouseDown(sender As Object, e As MouseEventArgs) Handles HVDgv1.MouseDown
        If e.Button = MouseButtons.Right Then
            Try
                HDGV1RowSelected = HVDgv1.HitTest(e.X, e.Y).RowIndex
            Catch
            End Try
        End If
    End Sub

    Private Sub HVDgv1_MouseUp(sender As Object, e As MouseEventArgs) Handles HVDgv1.MouseUp
        Try
            If e.Button = MouseButtons.Right AndAlso HDGV1RowSelected <> -1 Then
                HVDgv1.ClearSelection()
                HVDgv1.rows(HDGV1RowSelected).Selected = True
                DisplayHV()
                TSMIHVConfig()
            End If
        Catch
        End Try
    End Sub

    Private Sub HVDgv1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles HVDgv1.Keypress
        If [Char].IsLetter(e.KeyChar) or [Char].IsDigit(e.KeyChar) Then
            Dim ekey As String = e.KeyChar.ToString.ToLower
            HMkeypresstimer.Stop()
            HVKeyPress &= ekey
            HMkeypresstimer.Start()
            For i As Integer = 0 To (HVDgv1.Rows.Count) - 1
                Dim rtitle As String = HVDgv1.Rows(i).Cells(HMGridFieldToDisplay1).Value.ToString.ToLower
                If rtitle.StartsWith(HVKeyPress) Then
                    Dim icell As Integer = HVDgv1.CurrentCell.ColumnIndex
                    HVDgv1.CurrentCell = HVDgv1.Rows(i).Cells(icell)
                    DisplayHV()
                    Return
                End If
            Next
        End If
    End Sub

#End Region             'MVDgv1 Handlers

    Private Sub HMkeypresstimer_Elapsed()
        HVKeyPress = ""
    End Sub

    Private Sub TSMIHVConfig()
        PlaceHolderforHomeMovieTitleToolStripMenuItem.BackColor = Color.Honeydew
        PlaceHolderforHomeMovieTitleToolStripMenuItem.Text = "'" & HVDgv1.SelectedCells(2).Value.ToString & "'"
        PlaceHolderforHomeMovieTitleToolStripMenuItem.Font = New Font("Arial", 10, FontStyle.Bold)
    End Sub

    Private Sub HomeMovieContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles HomeMovieContextMenu.Opening
        If HDGV1RowSelected = -1 Then e.Cancel = True
    End Sub
    
    Private Sub HVPlay()
        Dim tempstring As String = ""
        Dim playlist As New List(Of String)
        Dim fullpathandfilename As String = WorkingHomeMovie.fileinfo.fullpathandfilename 
        If Not String.IsNullOrEmpty(fullpathandfilename) Then
            tempstring = Utilities.GetFileName(fullpathandfilename)
            If tempstring <> "" Then playlist.Add(tempstring)
        End If
        If playlist.Count > 0 Then
            Call launchplaylist(playlist)
        End If
    End Sub

    Private Sub cmbxHMSort_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbxHMSort.SelectedIndexChanged
        HMGridFieldToDisplay2 = cmbxHMSort.Text
        If Not MainFormLoadedStatus Then Exit Sub
        HMDGV_FiltersAndSortApply()
    End Sub
End Class