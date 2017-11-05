Imports System.Text.RegularExpressions
Imports Media_Companion.WorkingWithNfoFiles
Imports Media_Companion
Imports Media_Companion.Pref
Imports System.Linq
Imports System.ComponentModel

Partial Public Class Form1
    Dim newEpisodeList As New List(Of TvEpisode)
    Public languageList As New List(Of Tvdb.Language)
    Dim listOfShows As New List(Of str_PossibleShowList)
    Dim tvdbposterlist As New List(Of TvBanners)
    Dim imdbposterlist As New List(Of TvBanners)
    Dim tvdbmode As Boolean = False
    Dim usedlist As New List(Of TvBanners)
    Dim tvobjects As New List(Of String)
    Dim ShOrEploading As Boolean = False

#Region "Tv Treeview Routines"
    Public Sub tv_ViewReset()
        btn_SaveTvShowOrEpisode.Enabled = True
        Tv_TreeViewContext_RefreshShow.Enabled = False
        Tv_TreeViewContext_RefreshShow.Visible = False
        Tv_TreeViewContext_ShowMissEps.Enabled = False
        Tv_TreeViewContext_ShowMissEps.Visible = False
        Tv_TreeViewContext_DispByAiredDate.Enabled = False
        Tv_TreeViewContext_DispByAiredDate.Visible = False
        Tv_TreeViewContext_SearchNewEp.Enabled = False
        Tv_TreeViewContext_SearchNewEp.Visible = False
        Tv_TreeViewContext_FindMissArt.Enabled = False
        Tv_TreeViewContext_FindMissArt.Visible = False

        Tv_TreeViewContext_ViewNfo.Enabled = False
        ExpandSelectedShowToolStripMenuItem.Enabled = False
        CollapseSelectedShowToolStripMenuItem.Enabled = False
        ExpandAllToolStripMenuItem.Enabled = False
        CollapseAllToolStripMenuItem.Enabled = False
        Tv_TreeViewContext_ReloadFromCache.Enabled = False
        Tv_TreeViewContext_OpenFolder.Enabled = False

        tb_Sh_Ep_Title.Text = ""
        tb_ShPremiered.Text = ""
        tb_ShGenre.Text = ""
        tb_ShPlot.Text = ""
        tb_ShTvdbId.Text = ""
        tb_ShImdbId.Text = ""
        tb_ShRating.Text = ""
        tb_ShVotes.Text = ""
        tb_ShCert.Text = ""
        tb_ShRunTime.Text = ""
        tb_ShStudio.Text = ""
        cmbx_shUserRating.Text = "None"
        cbTvActorRole.Items.Clear()
        cbTvActorRole.Text = ""
        cbTvActor.Items.Clear()
        cbTvActor.Text = ""
        tvdbposterlist.Clear()
        PictureBox6.Image = Nothing
        tv_PictureBoxLeft.Image = Nothing
        tv_PictureBoxRight.Image = Nothing
        tv_PictureBoxBottom.Image = Nothing

        For i = Panel13.Controls.Count - 1 To 0 Step -1
            Panel13.Controls.RemoveAt(i)
        Next
    End Sub

    Private Sub TvTreeview_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TvTreeview.AfterSelect
        TvTreeview_AfterSelect_Do() 'moved this to seperate sub so we can also call this from other locations.
    End Sub

    Private Sub TvTreeview_AfterSelect_Do()
        Try
            'chooses which sub is run to load the relavent tv data to the screen
            'note: context menu items are set during TvTreeView_MouseUp event because we only need to update if right click is done which we check in the mouseup sub
            'mouseup sub selects the node underneath the mouse & then this runs since its an event 'AfterSelect'

            If TvTreeview.SelectedNode Is Nothing Then Exit Sub
            If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
                tv_ShowLoad(TvTreeview.SelectedNode.Tag)
            ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
                tv_SeasonSelected(TvTreeview.SelectedNode.Tag)
            ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
                tv_EpisodeSelected(TvTreeview.SelectedNode.Tag)
            Else
                MsgBox("None")
            End If

        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TvTreeview_DragDrop(sender As Object, e As DragEventArgs) Handles TvTreeview.DragDrop
        Dim files() As String
        files = e.Data.GetData(DataFormats.FileDrop)
        For f = 0 To UBound(files)
            If Directory.Exists(files(f)) Then
                If files(f).ToLower.Contains(".actors") Or files(f).ToLower.Contains("season") Then Continue For
                For each fol In Pref.tvRootFolders
                    If fol.rpath = files(f) Then Continue For
                    If files(f).Contains(fol.rpath) AndAlso Not fol.selected Then
                        Dim msg As String = "The series dropped is in a root folder that has been unselected"
                        msg &= "To avoid catastrophic failure, please re-select"
                        msg &= "root folder: " & fol.rpath
                        msg &= "and attempt again"
                        MsgBox(msg)
                        Continue For
                    End If
                Next
                Dim di As New DirectoryInfo(files(f))
                If Pref.tvFolders.Contains(files(f)) Then Continue For
                Dim skip As Boolean = False
                For Each item In droppedItems
                    If item = files(f) Then
                        skip = True
                        Exit For
                    End If
                Next
                If Not skip Then droppedItems.Add(files(f))
            End If
        Next
        If droppedItems.Count < 1 Then Exit Sub
        Dim droppedmsg As String = "You have dropped the following folders onto TV," & vbCrLf & "Are you sure you wish to add these as TvShows?" & vbCrLf
        For Each item In droppedItems
            droppedmsg &= item & vbcrlf
        Next
        Dim x = MsgBox(droppedmsg, MsgBoxStyle.YesNo)
        If x = MsgBoxResult.No Then
            droppedItems.Clear()
            Exit Sub
        End If
        For Each item In droppedItems
            Pref.tvFolders.add(item)
            newTvFolders.Add(item)
        Next
        droppedItems.Clear()
        Pref.ConfigSave()
        tv_ShowScrape()
    End Sub

    Private Sub TvTreeview_DragEnter(sender As Object, e As DragEventArgs) Handles TvTreeview.DragEnter
        Try
            e.Effect = DragDropEffects.Copy
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TvTreeviewRebuild()
        TvTreeview.BeginUpdate()
        Try
            Dim shcount As Integer = 0
            Dim epcount As Integer = 0
            TvTreeview.Nodes.Clear()              'clear the treeview of old data
            ''Dirty work around until TvShows is repalced with TvCache.Shows universally
            For Each TvShow As Media_Companion.TvShow In Cache.TvCache.Shows
                If Not String.IsNullOrEmpty(TvShow.Hidden.Value) AndAlso TvShow.Hidden.Value = True Then Continue For
                shcount += 1
                epcount += TvShow.Episodes.Count
                'TvTreeview.Nodes.Add(TvShow.ShowNode)
                Dim action As Action = Sub() TvTreeview.Nodes.Add(TvShow.ShowNode)
                TvTreeview.Invoke(action)
                TvShow.UpdateTreenode()
            Next

            If rbTvListAll.Checked Then TvTreeview.BackColor = Color.white
            If rbTvListEnded.Checked Then TvTreeview.BackColor = Color.lightpink
            If rbTvListContinuing.Checked Then TvTreeview.BackColor = Color.LightSeaGreen
            If rbTvListUnKnown.Checked Then TvTreeview.BackColor = Color.LightYellow

            TextBox_TotTVShowCount.Text = shcount.ToString
            TextBox_TotEpisodeCount.Text = epcount.ToString
            If MainFormLoadedStatus Then
                Dim action2 As Action = Sub() TvTreeview.Sort()
                TvTreeview.Invoke(action2)
            Else
                TvTreeview.Sort()
            End If
        Finally
            TvTreeview.EndUpdate()
        End Try
    End Sub

    Private Sub Tv_TreeViewContextMenuItemsEnable()        'enable/disable right click context menu items depending on if its show/season/episode
        '                                                  'called from tv_treeview mouseup event where we check for a right click
        If TvTreeview.SelectedNode Is Nothing Then Return
        Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)  'set WORKINGTVSHOW to show obj irrelavent if we have selected show/season/episode
        Dim showtitle As String = WorkingTvShow.Title.Value       'set our show title



        'now we set the items that have variable text in the context menu using the 'show' text set above
        Tv_TreeViewContext_ShowTitle.BackColor = Color.Honeydew                'SK - same color as the refresh tv show splash - comments required to see if it works or not....

        If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
            Tv_TreeViewContext_ShowTitle.Text = "'" & showtitle & "'"
            Tv_TreeViewContext_ShowTitle.Font = New Font("Arial", 10, FontStyle.Bold)
            Tv_TreeViewContext_Play_Episode.Enabled = False
            Tv_TreeViewContext_ViewNfo.Text = "View TVShow .nfo"
            Tv_TreeViewContext_RescrapeShowOrEpisode.Text = "Rescrape TVShow"
            Tv_TreeViewContext_WatchedShowOrEpisode.Text = "Mark This Show as Watched"
            Tv_TreeViewContext_UnWatchedShowOrEpisode.Text = "Mark This Show as UnWatched"

            Tv_TreeViewContext_OpenFolder.Enabled = True
            Tv_TreeViewContext_ViewNfo.Enabled = True
            Tv_TreeViewContext_RescrapeShowOrEpisode.Enabled = True
            Tv_TreeViewContext_WatchedShowOrEpisode.Enabled = True
            Tv_TreeViewContext_UnWatchedShowOrEpisode.Enabled = True
            Tv_TreeViewContext_RescrapeWizard.Enabled = True
            Tv_TreeViewContext_FindMissArt.Enabled = True
            Tv_TreeViewContext_RefreshShow.Enabled = True
            Tv_TreeViewContext_RefreshShow.Visible = True
            Tv_TreeViewContext_MissingEpThumbs.Enabled = True
            Tv_TreeViewContext_MissingEpThumbs.Visible = True
            Tv_TreeViewContext_ReloadFromCache.Enabled = True
            Tv_TreeViewContext_RenameEp.Enabled = rbTvListAll.Checked     'Only show if Treeview set to 'List All'
            Tv_TreeViewContext_ShowMissEps.Enabled = True
            Tv_TreeViewContext_DispByAiredDate.Enabled = True
            tsmiTvDelShowNfoArt.Enabled = True
            tsmiTvDelShowEpNfoArt.Enabled = True

        ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
            Tv_TreeViewContext_ShowTitle.Text = "'" & showtitle & "' - " & tv_SeasonSelectedCurrently(TvTreeview).SeasonLabel
            Tv_TreeViewContext_ShowTitle.Font = New Font("Arial", 10, FontStyle.Bold)
            Tv_TreeViewContext_Play_Episode.Enabled = False
            Tv_TreeViewContext_ViewNfo.Text = "View Season .nfo"
            Tv_TreeViewContext_RescrapeShowOrEpisode.Text = "Rescrape Season"
            Tv_TreeViewContext_WatchedShowOrEpisode.Text = "Mark This Season as Watched"
            Tv_TreeViewContext_UnWatchedShowOrEpisode.Text = "Mark This Season as UnWatched"

            Tv_TreeViewContext_OpenFolder.Enabled = True
            Tv_TreeViewContext_ViewNfo.Enabled = False
            Tv_TreeViewContext_RescrapeShowOrEpisode.Enabled = False
            Tv_TreeViewContext_WatchedShowOrEpisode.Enabled = True
            Tv_TreeViewContext_UnWatchedShowOrEpisode.Enabled = True
            Tv_TreeViewContext_RescrapeWizard.Enabled = False
            Tv_TreeViewContext_FindMissArt.Enabled = False
            Tv_TreeViewContext_RefreshShow.Enabled = False
            Tv_TreeViewContext_RefreshShow.Visible = False
            Tv_TreeViewContext_MissingEpThumbs.Enabled = True
            Tv_TreeViewContext_MissingEpThumbs.Visible = True
            Tv_TreeViewContext_ReloadFromCache.Enabled = False
            Tv_TreeViewContext_RenameEp.Enabled = rbTvListAll.Checked      'Only show if Treeview set to 'List All'
            Tv_TreeViewContext_ShowMissEps.Enabled = True
            Tv_TreeViewContext_DispByAiredDate.Enabled = True
            tsmiTvDelShowNfoArt.Enabled = False
            tsmiTvDelShowEpNfoArt.Enabled = False

        ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
            Tv_TreeViewContext_ShowTitle.Text = "'" & showtitle & "' - S" & Utilities.PadNumber(ep_SelectedCurrently(TvTreeview).Season.Value, 2) & "E" & Utilities.PadNumber(ep_SelectedCurrently(TvTreeview).Episode.Value, 2) & " '" & ep_SelectedCurrently(TvTreeview).Title.Value & "'"
            Tv_TreeViewContext_ShowTitle.Font = New Font("Arial", 10, FontStyle.Bold)
            Tv_TreeViewContext_Play_Episode.Enabled = Not DirectCast(TvTreeview.SelectedNode.Tag, Media_Companion.TvEpisode).Ismissing
            Tv_TreeViewContext_ViewNfo.Text = "View Episode .nfo"
            Tv_TreeViewContext_RescrapeShowOrEpisode.Text = "Rescrape Episode"
            Tv_TreeViewContext_WatchedShowOrEpisode.Text = "Mark Episode as Watched"
            Tv_TreeViewContext_UnWatchedShowOrEpisode.Text = "Mark Episode as UnWatched"

            Tv_TreeViewContext_OpenFolder.Enabled = True
            Tv_TreeViewContext_ViewNfo.Enabled = True
            Tv_TreeViewContext_RescrapeShowOrEpisode.Enabled = Not DirectCast(TvTreeview.SelectedNode.Tag, Media_Companion.TvEpisode).Ismissing
            Tv_TreeViewContext_WatchedShowOrEpisode.Enabled = True
            Tv_TreeViewContext_UnWatchedShowOrEpisode.Enabled = True
            Tv_TreeViewContext_RescrapeWizard.Enabled = False
            Tv_TreeViewContext_FindMissArt.Enabled = False
            Tv_TreeViewContext_RefreshShow.Enabled = False
            Tv_TreeViewContext_RefreshShow.Visible = False
            Tv_TreeViewContext_MissingEpThumbs.Enabled = False
            Tv_TreeViewContext_MissingEpThumbs.Visible = False
            Tv_TreeViewContext_ReloadFromCache.Enabled = False
            Tv_TreeViewContext_RenameEp.Enabled = Not DirectCast(TvTreeview.SelectedNode.Tag, Media_Companion.TvEpisode).Ismissing
            Tv_TreeViewContext_ShowMissEps.Enabled = True
            Tv_TreeViewContext_DispByAiredDate.Enabled = True
            tsmiTvDelShowNfoArt.Enabled = False
            tsmiTvDelShowEpNfoArt.Enabled = False

        Else
            MsgBox("None")
        End If

        'these are the four items at the bottom of the menu to control Expand/Colapse the tv_treeview (always shown)
        ExpandSelectedShowToolStripMenuItem.Enabled = True
        ExpandAllToolStripMenuItem.Enabled = True
        CollapseAllToolStripMenuItem.Enabled = True
        CollapseSelectedShowToolStripMenuItem.Enabled = True
    End Sub

#End Region

    Sub tv_Rescrape_Show(ByVal WorkingTvShow)
        If MessageBox.Show("Rescraping the TV Show will Overwrite all the current details" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then Exit Sub
        
        Application.DoEvents()
        Dim selectedLang As String = If(WorkingTvShow.Language.Value = "", "en", WorkingTvShow.Language.Value)

        '#discontinue
        If Pref.tvshow_useXBMC_Scraper = True Then
            Dim messbox As frmMessageBox = New frmMessageBox("The Selected TV Show is being Rescraped", "", "Please Wait")
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            messbox.Refresh()
            Dim TVShowNFOContent As String = XBMCScrape_TVShow_General_Info("metadata.tvdb.com", WorkingTvShow.TvdbId.Value, selectedLang, WorkingTvShow.NfoFilePath)
            If TVShowNFOContent <> "error" Then CreateMovieNfo(WorkingTvShow.NfoFilePath, TVShowNFOContent)
            Dim newshow As TvShow = nfoFunction.tvshow_NfoLoad(WorkingTvShow.NfoFilePath)
            newshow.ListActors.Clear()
            If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Or NewShow.ImdbId.Value = Nothing Then
                TvGetActorTvdb(NewShow)
            ElseIf (Pref.TvdbActorScrape = 1 Or Pref.TvdbActorScrape = 2) And NewShow.ImdbId.Value <> Nothing Then
                TvGetActorImdb(NewShow)
            End If
            If Pref.tvdbIMDbRating Then
                Dim rating As String = ""
                Dim votes As String = ""
                If ep_getIMDbRating(newshow.ImdbId.Value, rating, votes) Then
                    newshow.Rating.Value = rating
                    newshow.Votes.Value = votes
                End If
            End If
            nfoFunction.tvshow_NfoSave(newshow, True)
            messbox.Close()
            Call tv_ShowLoad(WorkingTvShow)
        Else
            TabControl3.SelectedIndex = 0
            RunBackgroundTVScrape("TVSeriesRescrape")
        End If
        
        TabControl3.SelectedIndex = 0
    End Sub

    Public Sub tv_ShowFind(ByVal rootfolders As List(Of str_RootPaths), Optional ByVal skiptvfolderschk As Boolean = False)
        Dim Folders As List(Of String)
        newTvFolders.Clear()
        For Each folder In rootfolders
            If Not folder.selected Then Continue For
            Folders = Utilities.EnumerateFolders(folder.rpath, 0)
            For Each strfolder2 As String In Folders
                If skiptvfolderschk Then
                    If Not lb_tvSeriesFolders.Items.Contains(strfolder2) AndAlso Utilities.ValidMovieDir(strfolder2) Then newTvFolders.Add(strfolder2)
                ElseIf Not Pref.tvFolders.Contains(strfolder2) AndAlso Utilities.ValidMovieDir(strfolder2) Then
                    If Not lb_tvSeriesFolders.Items.Contains(strfolder2) Then newTvFolders.Add(strfolder2)
                End If
            Next
        Next
    End Sub

    Public Sub tv_ShowScrape()
        If Not BckWrkTv.IsBusy Then
            RunBackgroundTVScrape("TVSeriesSearchForNew")
        End If
    End Sub


    Private Sub tv_ShowLoad(ByVal Show As Media_Companion.TvShow)
        ShOrEploading = True
        Show.ListActors.Clear()
        Show.Load()
        Show = nfoFunction.tvshow_NfoLoad(Show.NfoFilePath)

        'Fix episodeguide tag
        Dim lang As String = Show.EpisodeGuideUrl.Value
        If String.IsNullOrEmpty(lang) Then
            lang = "en"
        Else
            lang = lang.Substring((lang.LastIndexOf("/") + 1)).Replace(".zip", "")
        End If

        If Not Show.TvdbId.Value = "" Then
            Show.EpisodeGuideUrl.Value = ""
            Show.Url.Value = URLs.EpisodeGuide(Show.TvdbId.Value, lang) 'Show.Language.Value)
            Show.Url.Node.SetAttributeValue("cache", Show.TvdbId.Value)
        End If
        'end fix

        Dim hg As New DirectoryInfo(Show.FolderPath)
        If Not hg.Exists Then
            tb_ShPlot.Text = "Unable to find folder: " & Show.FolderPath
            tb_Sh_Ep_Title.Text = "Unable to find folder: " & Show.FolderPath
        Else
            If TabControl3.TabPages(1).Text = "Screenshot" Then
                TabControl3.TabPages.RemoveAt(1)
            End If

            lbxTvShSelectResults.Items.Clear()
            tbTvShSelectSearchTitle.Text = ""
            Dim todo As Boolean = False

            If Show.State = Media_Companion.ShowState.Locked Then
                btn_TvShState.Text = "Locked"
                btn_TvShState.BackColor = Color.Red
            ElseIf Show.State = Media_Companion.ShowState.Open Then
                btn_TvShState.Text = "Open"
                btn_TvShState.BackColor = Color.LawnGreen
            ElseIf Show.State = Media_Companion.ShowState.Unverified Then
                btn_TvShState.Text = "Un-Verified"
                btn_TvShState.BackColor = Color.Yellow
            Else
                btn_TvShState.Text = "Error"
                btn_TvShState.BackColor = Color.Gray
            End If
            btn_TvShState.Tag = Show

            If Show.Status.Value = "Ended" Then
                bnt_TvSeriesStatus.Text = "Ended"
                bnt_TvSeriesStatus.BackColor = Color.LightPink
            ElseIf Show.Status.Value = "Continuing" Then
                bnt_TvSeriesStatus.Text = "Continuing"
                bnt_TvSeriesStatus.BackColor = Color.LightSeaGreen
            Else
                bnt_TvSeriesStatus.Text = "Unknown"
                bnt_TvSeriesStatus.BackColor = Color.LightYellow
            End If
            Dim tvpbright As String = Utilities.DefaultTvPosterPath
            Dim tvpbbottom As String = Utilities.DefaultTvBannerPath

            If Pref.EdenEnabled AndAlso Not Pref.FrodoEnabled Then
                If Pref.postertype = "banner" Then
                    If Utilities.IsBanner(Show.FolderPath & "folder.jpg") Then
                        tvpbbottom = Show.FolderPath & "folder.jpg"
                    Else
                        tvpbright = Show.ImagePoster.path
                    End If
                Else
                    tvpbright = Show.ImagePoster.path
                End If
            End If
            If Pref.FrodoEnabled Then
                Show.ImagePoster.FileName = "poster.jpg"
                Show.ImageBanner.FileName = "banner.jpg"
                tvpbbottom = Show.ImageBanner.Path
                tvpbright = Show.ImagePoster.path
            End If
            util_ImageLoad(tv_PictureBoxRight, tvpbright, Utilities.DefaultTvPosterPath)
            util_ImageLoad(tv_PictureBoxBottom, tvpbbottom, Utilities.DefaultTvBannerPath)
            util_ImageLoad(tv_PictureBoxLeft, Show.ImageFanart.Path, Utilities.DefaultTvFanartPath)

            Panel_EpisodeInfo   .Visible = False
            Panel_EpisodeActors .Visible = False
            lbl_sorttitle       .Visible = True
            TextBox_Sorttitle   .Visible = True

            tb_Sh_Ep_Title.BackColor = Color.White
            If Show.Title.Value <> Nothing Then tb_Sh_Ep_Title.Text = Show.Title.Value

            ' changed indication of an issue, setting the title means that the title is saved to the nfo if the user exits. Yellow is the same colour as the unverified Button
            If Show.State = ShowState.Unverified Then tb_Sh_Ep_Title.BackColor = Color.Yellow
            If Show.State = ShowState.Error Then tb_Sh_Ep_Title.BackColor = Color.Red

            tb_ShPremiered      .Text = Utilities.ReplaceNothing(Show.Premiered.Value)
            tb_ShGenre          .Text = Utilities.ReplaceNothing(Show.Genre.Value)
            tb_ShTvdbId         .Text = Utilities.ReplaceNothing(Show.TvdbId.Value)
            tb_ShImdbId         .Text = Utilities.ReplaceNothing(Show.ImdbId.Value)
            tb_ShRating         .Text = Utilities.ReplaceNothing(Show.Rating.Value)
            tb_ShVotes          .Text = Utilities.ReplaceNothing(Show.Votes.Value)
            tb_ShCert           .Text = Utilities.ReplaceNothing(Show.Mpaa.Value)
            tb_ShRunTime        .Text = Utilities.ReplaceNothing(Show.Runtime.Value)
            tb_ShStudio         .Text = Utilities.ReplaceNothing(Show.Studio.Value)
            tb_ShPlot           .Text = Utilities.ReplaceNothing(Show.Plot.Value)
            cmbx_shUserRating   .Text = If(Show.UserRating.Value = "0", "None", Show.UserRating.Value)
            TextBox_Sorttitle   .Text = Utilities.ReplaceNothing(If(String.IsNullOrEmpty(Show.SortTitle.Value), Show.Title.Value, Show.SortTitle.Value))

            If String.IsNullOrEmpty(Show.SortOrder.Value) Then Show.SortOrder.Value = Pref.sortorder
            If Show.SortOrder.Value = "dvd" Then
                btn_TvShSortOrder.Text = "DVD"
            ElseIf Show.SortOrder.Value = "default" Then
                btn_TvShSortOrder.Text = "Default"
            End If
            '0	-	all from tvdb
            '1	-	all from imdb
            '2	-	tv imdb, eps tvdb
            '3	-	tv TVDB, eps IMDB

            If String.IsNullOrEmpty(Show.EpisodeActorSource.Value) Then
                Show.EpisodeActorSource.Value = If((Pref.TvdbActorScrape = "0" Or Pref.TvdbActorScrape = "2"), "tvdb", "imdb")
                'If Pref.TvdbActorScrape = "0" Or Pref.TvdbActorScrape = "2" Then
                '    Show.EpisodeActorSource.Value = "tvdb"
                'Else
                '    Show.EpisodeActorSource.Value = "imdb"
                'End If
            End If

            Button46.Text = Show.EpisodeActorSource.Value.ToUpper
            If String.IsNullOrEmpty(Show.TvShowActorSource.Value) Then
                Show.TvShowActorSource.Value = If((Pref.TvdbActorScrape = "0" Or Pref.TvdbActorScrape = "3"), "tvdb", "imdb")
                'If Pref.TvdbActorScrape = "0" Or Pref.TvdbActorScrape = "3" Then
                '    Show.TvShowActorSource.Value = "tvdb"
                'Else
                '    Show.TvShowActorSource.Value = "imdb"
                'End If
            End If

            TvPanel7Update(Show.FolderPath)
            Button45.Text = Show.TvShowActorSource.Value.ToUpper
            Call tv_ActorsLoad(Show.ListActors)
        End If
        If Not Show.Status.Value = "missing" Then
            Dim flags As New List(Of KeyValuePair(Of String, String))
            movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, tb_ShRating.Text, flags, True)
        End If

        Panel_EpisodeInfo.Visible = False
        Panel_EpisodeActors.Visible = False
        ShOrEploading = False
    End Sub

    Private Sub tb_ShGenre_MouseDown(sender As Object, e As MouseEventArgs) Handles tb_ShGenre.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Try
                Dim thisshow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
                Dim item() As String = thisshow.Genre.Value.Split("/")
                Dim genre As String = ""
                Dim listof As New List(Of str_genre)
                listof.Clear()
                For Each i In item
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
                    thisshow.Genre.Value = genre
                    tb_ShGenre.Text = genre
                    nfoFunction.tvshow_NfoSave(thisshow, True)
                End If
            Catch
            End Try
        End If
    End Sub

    Public Sub tv_ActorsLoad(ByVal listActors As Media_Companion.ActorList)
        cbTvActor.Items.Clear()
        cbTvActorRole.Items.Clear()
        For Each actor In ListActors
            If actor.actorname <> Nothing AndAlso Not cbTvActor.Items.Contains(actor.actorname) Then
                cbTvActor.Items.Add(actor.actorname)
                Dim role As String = actor.actorrole
                If String.IsNullOrEmpty(role) Or role = Nothing Then role = actor.actorname
                cbTvActorRole.Items.Add(role)
            End If
        Next

        If cbTvActor.Items.Count = 0 Then
            actorflag = true
            cbTvActorRole.Items.Add("")
            cbTvActorRole.SelectedIndex = 0
            Call tv_ActorDisplay(True)
        Else
            cbTvActor.SelectedIndex = 0
        End If
    End Sub

    Public Sub tv_ActorDisplay(Optional ByVal useDefault As Boolean = False)
        Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
        If WorkingTvShow Is Nothing Then Exit Sub
        Dim imgLocation As String = Utilities.DefaultActorPath
        Dim eden As Boolean = Pref.EdenEnabled
        Dim frodo As Boolean = Pref.FrodoEnabled
        PictureBox6.Image = Nothing
        If useDefault Then
            imgLocation = Utilities.DefaultActorPath
        Else
            For Each actor In WorkingTvShow.ListActors
                If actor.actorname = cbTvActor.Text Then
                    Dim temppath As String = Pref.GetActorPath(WorkingTvShow.NfoFilePath, actor.actorname, actor.ID.Value)
                    If File.Exists(temppath) Then
                        imgLocation = temppath
                    ElseIf (Not Pref.LocalActorImage) AndAlso actor.actorthumb <> Nothing AndAlso (actor.actorthumb.IndexOf("http") <> -1 OrElse File.Exists(actor.actorthumb)) Then
                        imgLocation = actor.actorthumb
                    End If
                    Exit For
                End If
            Next
        End If
        Try
            util_ImageLoad(PictureBox6, imgLocation, Utilities.DefaultActorPath)
            PictureBox6.SizeMode = PictureBoxSizeMode.Zoom
        Catch
        End Try
    End Sub

    Public Sub tv_ActorRoleDisplay(Optional ByVal useDefault As Boolean = False)
        Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
        If WorkingTvShow Is Nothing Then Exit Sub
        Dim imgLocation As String = Utilities.DefaultActorPath
        Dim eden As Boolean = Pref.EdenEnabled
        Dim frodo As Boolean = Pref.FrodoEnabled
        PictureBox6.Image = Nothing
        If useDefault Then
            imgLocation = Utilities.DefaultActorPath
        Else
            For Each actor In WorkingTvShow.ListActors
                If actor.actorrole = cbTvActorRole.Text Then
                    Dim temppath As String = Pref.GetActorPath(WorkingTvShow.NfoFilePath, actor.actorname, actor.ID.Value)
                    If File.Exists(temppath) Then
                        imgLocation = temppath
                    ElseIf (Not Pref.LocalActorImage) AndAlso actor.actorthumb <> Nothing AndAlso (actor.actorthumb.IndexOf("http") <> -1 OrElse File.Exists(actor.actorthumb)) Then
                        imgLocation = actor.actorthumb
                    End If
                    Exit For
                End If
            Next
        End If
        Try
            util_ImageLoad(PictureBox6, imgLocation, Utilities.DefaultActorPath)
            PictureBox6.SizeMode = PictureBoxSizeMode.Zoom
        Catch
        End Try
    End Sub

    Public Function TvCheckforExtraArt(ByVal Showpath As String) As Boolean
        Dim confirmedpresent As Boolean = False
        If Directory.Exists(Showpath) Then
            If File.Exists(Showpath & "clearart.png") Then tvFanlistbox.Items.Add("ClearArt") : confirmedpresent = True
            If File.Exists(Showpath & "logo.png") Then tvFanlistbox.Items.Add("Logo") : confirmedpresent = True
            If File.Exists(Showpath & "landscape.jpg") Then tvFanlistbox.Items.Add("Landscape") : confirmedpresent = True
            If File.Exists(Showpath & "character.png") Then tvFanlistbox.Items.Add("Character") : confirmedpresent = True
        End If
        Return confirmedpresent
    End Function

    Public Sub TvPanel7Update(ByVal TvShPath As String)
        tvFanlistbox.Items.Clear()
        Panel_TvShowExtraArtwork.Visible = TvCheckforExtraArt(TvShPath)
    End Sub

    Private Sub tvFanlistbox_Mouse(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvFanlistbox.MouseDown
        Dim imagepath As String = Nothing
        Dim item As String = Nothing
        If e.button = Windows.Forms.MouseButtons.Right Then
            Dim index As Integer = tvFanlistbox.IndexFromPoint(New Point(e.X, e.Y))
            If index >= 0 Then tvFanlistbox.SelectedItem = tvFanlistbox.Items(index)
        End If
        If IsNothing(tvFanlistbox.SelectedItem) Then Exit Sub
        item = tvFanlistbox.SelectedItem.ToString.ToLower
        If Not String.IsNullOrEmpty(item) Then
            imagepath = tv_ShowSelectedCurrently(TvTreeview).FolderPath
            Dim suffix As String = If((item = "clearart" or item = "logo" or item = "character"), ".png", ".jpg")
            imagepath &= item & suffix
        End If
        If e.Button = Windows.Forms.MouseButtons.Left Then
            pbtvfanarttv.Visible = True
            If Not IsNothing(imagepath) Then util_ImageLoad(pbtvfanarttv, imagepath, "")
        ElseIf e.button = Windows.Forms.MouseButtons.Right Then
            Dim tempint = MessageBox.show("Do you wish to delete this image from" & vbCrLf & "this Tv Show?", "Fanart.Tv Artwork Delete", MessageBoxButtons.YesNoCancel)
            If tempint = Windows.Forms.DialogResult.No or tempint = DialogResult.Cancel Then Exit Sub
            If tempint = Windows.Forms.DialogResult.Yes Then
                Utilities.SafeDeleteFile(imagepath)
                TvPanel7Update(tv_ShowSelectedCurrently(TvTreeview).FolderPath)
            End If
        End If
    End Sub

    Private Sub tvFanlistbox_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tvFanlistbox.MouseLeave
        tvFanlistbox.ClearSelected()
        pbtvfanarttv.Image = Nothing
        pbtvfanarttv.Visible = False
    End Sub

    Public Sub tv_SeasonSelected(ByRef SelectedSeason As Media_Companion.TvSeason)
        SelectedSeason.ShowObj.ListActors.Clear()
        SelectedSeason.ShowObj.Load()
        Panel_TvShowExtraArtwork.Visible = False
        Dim Show As Media_Companion.TvShow
        If SelectedSeason.SeasonNode.Parent.Tag IsNot Nothing Then
            Show = SelectedSeason.SeasonNode.Parent.Tag
        Else
            MsgBox("Show tag not set")
            Exit Sub
        End If
        tb_Sh_Ep_Title.BackColor = Color.White
        If Show.Title.Value <> Nothing Then tb_Sh_Ep_Title.Text = Utilities.ReplaceNothing(Show.Title.Value) & " - " & Utilities.ReplaceNothing(SelectedSeason.SeasonNode.Text)
        If TabControl3.TabPages(1).Text = "Screenshot" Then TabControl3.TabPages.RemoveAt(1)

        ' changed indication of an issue, setting the title means that the title is saved to the nfo if the user exits. Yellow is the same colour as the unverified Button
        If Show.State = ShowState.Unverified Then tb_Sh_Ep_Title.BackColor = Color.Yellow
        If Show.State = ShowState.Error Then tb_Sh_Ep_Title.BackColor = Color.Red

        tb_ShPremiered      .Text = Utilities.ReplaceNothing(Show.Premiered.Value)
        tb_ShGenre          .Text = Utilities.ReplaceNothing(Show.Genre.Value)
        tb_ShTvdbId         .Text = Utilities.ReplaceNothing(Show.TvdbId.Value)
        tb_ShImdbId         .Text = Utilities.ReplaceNothing(Show.ImdbId.Value)
        tb_ShRating         .Text = Utilities.ReplaceNothing(Show.Rating.Value)
        tb_ShVotes          .Text = Utilities.ReplaceNothing(Show.Votes.Value)
        tb_ShCert           .Text = Utilities.ReplaceNothing(Show.Mpaa.Value)
        tb_ShRunTime        .Text = Utilities.ReplaceNothing(Show.Runtime.Value)
        tb_ShStudio         .Text = Utilities.ReplaceNothing(Show.Studio.Value)
        tb_ShPlot           .Text = Utilities.ReplaceNothing(Show.Plot.Value)
        cmbx_shUserRating   .Text = If(Show.UserRating.Value = "0", "None", Show.UserRating.Value)
        Panel_EpisodeInfo   .Visible = False
        Panel_EpisodeActors .Visible = False
        lbl_sorttitle       .Visible = True
        TextBox_Sorttitle   .Visible = True
        TextBox_Sorttitle   .Text = Utilities.ReplaceNothing(If(String.IsNullOrEmpty(Show.SortTitle.Value), Show.Title.Value, Show.SortTitle.Value))
        ExpandSelectedShowToolStripMenuItem     .Enabled = True
        ExpandAllToolStripMenuItem              .Enabled = True
        CollapseAllToolStripMenuItem            .Enabled = True
        CollapseSelectedShowToolStripMenuItem   .Enabled = True
        Tv_TreeViewContext_RenameEp             .Enabled = True
        Tv_TreeViewContext_RenameEp             .Visible = True

        'MsgBox("Season")
        Dim season          As String   = SelectedSeason.SeasonLabel
        Dim trueseason      As Integer  = SelectedSeason.SeasonNumber
        Dim PaddedSeason    As String   = Utilities.PadNumber(SelectedSeason.SeasonNumber, 2)
        Dim tvpbright       As String   = Utilities.DefaultTvPosterPath
        Dim tvpbbottom      As String   = Utilities.DefaultTvBannerPath
        If trueseason = -1 Then
            If SelectedSeason.Poster.Image IsNot Nothing Then
                tvpbright = SelectedSeason.Poster.Path
            Else
                If Pref.postertype = "banner" Then
                    tvpbright = Show.ImagePoster.Path
                Else
                    tvpbbottom = Show.ImageBanner.Path
                End If
            End If
        ElseIf trueseason = 0 Then          'Specials
            If Pref.EdenEnabled AndAlso Not Pref.FrodoEnabled Then
                tvpbright = Show.FolderPath & "season-specials.tbn"
                If Not File.Exists(tvpbright) Then tvpbright = Show.FolderPath & "folder.jpg"
            End If
            If Pref.FrodoEnabled Then
                tvpbright = Show.FolderPath & "season-specials-poster.jpg"
                If Not File.Exists(tvpbright) Then tvpbright = Show.FolderPath & "folder.jpg"
                tvpbbottom = Show.FolderPath & "season-specials-banner.jpg"
                If Not File.Exists(tvpbbottom) Then tvpbbottom = Show.FolderPath & "banner.jpg"
            End If
        Else                                'Season01 & up
            tvpbright = SelectedSeason.Poster.Path
            If Pref.FrodoEnabled Then tvpbbottom = SelectedSeason.Banner.Path
        End If

        util_ImageLoad(tv_PictureBoxRight, tvpbright, Utilities.DefaultTvPosterPath)
        util_ImageLoad(tv_PictureBoxBottom, tvpbbottom, Utilities.DefaultTvBannerPath)

        If Show.NfoFilePath <> Nothing Then util_ImageLoad(tv_PictureBoxLeft, Show.FolderPath & "fanart.jpg", Utilities.DefaultTvFanartPath)

        Call tv_ActorsLoad(Show.ListActors)
        Show.UpdateTreenode()
    End Sub

    Public Sub tv_EpisodeSelected(ByRef SelectedEpisode As Media_Companion.TvEpisode, Optional ByVal Force As Boolean = False)
        If TabControl3.TabPages(1).Text <> "Screenshot" Then
            If screenshotTab IsNot Nothing Then
                TabControl3.TabPages.Insert(1, screenshotTab)
                TabControl3.Refresh()
            End If
        End If
        Panel_EpisodeInfo.Visible = True
        Panel_EpisodeActors.Visible = True   'set ep actor panel visible, we'll hide later if no actor's in episode.
        cmbxEpActor.Items.Clear()
        tbEpRole.Text = ""

        Dim Show As TvShow = tv_ShowSelectedCurrently(TvTreeview)
        Dim season As Integer = SelectedEpisode.Season.Value
        Dim episode As Integer = SelectedEpisode.Episode.Value
        Dim SeasonObj As New Media_Companion.TvSeason
        If SelectedEpisode.EpisodeNode.Parent IsNot Nothing Then
            SeasonObj = SelectedEpisode.EpisodeNode.Parent.Tag
            If season = -1 Then season = SeasonObj.SeasonLabel
        End If

        Call ep_Load(SeasonObj, SelectedEpisode, Force)

        lbl_sorttitle.Visible = False
        TextBox_Sorttitle.Visible = False
        Tv_TreeViewContext_ViewNfo.Enabled = True
        ExpandSelectedShowToolStripMenuItem.Enabled = True
        ExpandAllToolStripMenuItem.Enabled = True
        CollapseAllToolStripMenuItem.Enabled = True
        CollapseSelectedShowToolStripMenuItem.Enabled = True
        Tv_TreeViewContext_ReloadFromCache.Enabled = True
        Tv_TreeViewContext_OpenFolder.Enabled = True

        If SelectedEpisode.ListActors.Count < 1 Then    'If episode actors, don't load show's actors. (save some time)
            If SeasonObj.ShowObj.ListActors.Count = 0 Then Show.Load()
            Call tv_ActorsLoad(Show.ListActors)
        End If
    End Sub

    Private Function TestForMultiepisode(ByVal path As String)
        Dim multiepisode As Boolean = False
        Try
            Dim firstline As String = ""
            If File.Exists(path) Then
                Dim listText As New List(Of String)
                Dim objLine As String = ""
                Using objReader As IO.StreamReader = File.OpenText(path)
                    Do
                        objLine = objReader.ReadLine()
                        If objLine.IndexOf("<multiepisodenfo>") <> -1 Then
                            multiepisode = True
                            Exit Do
                        ElseIf objLine.IndexOf("<episodedetails>") <> -1 Then
                            multiepisode = False
                            Exit Do
                        End If
                    Loop Until objLine Is Nothing
                End Using
            End If
        Catch
        End Try
        Return multiepisode
    End Function

    Private Sub ep_Load(ByRef Season As Media_Companion.TvSeason, ByRef Episode As Media_Companion.TvEpisode, Optional ByVal epupdate As Boolean = False)
        Panel_TvShowExtraArtwork.Visible = False
        ShOrEploading = True

        Episode.ListActors.Clear()
        Dim episodelist As New List(Of TvEpisode)
        episodelist = WorkingWithNfoFiles.ep_NfoLoad(Episode.NfoFilePath)
        'test for multiepisodenfo
        If episodelist.Count = 1 Then
            Episode.AbsorbTvEpisode(episodelist(0))
        Else
            For Each Ep In episodelist
                If Ep.Season.Value = Episode.Season.Value AndAlso Ep.Episode.Value = Episode.Episode.Value Then
                    Episode.AbsorbTvEpisode(Ep)   'update treenode
                    Exit For
                End If
            Next
        End If

        'Dim tempstring As String = ""
        'lb_EpDetails.Items.Clear()

        cmbxEpActor.Items.Clear()
        tb_EpFilename.Text = Utilities.ReplaceNothing(Episode.NfoFilePath)
        tb_EpImdbId.Text = Episode.ImdbId.Value
        cmbx_EpUsrRating.Text = If(Episode.UserRating.Value = "0", "None", Episode.UserRating.Value)
        If Not File.Exists(Episode.NfoFilePath) Then
            tb_Sh_Ep_Title.Text = "Unable to find episode: " & Episode.NfoFilePath
            tbEpRole.Text = ""
            Panel_EpisodeInfo.Visible = True
            Panel_EpisodeActors.Visible = True
            cmbxEpActor.Items.Clear()
            Episode.EpisodeNode.BackColor = Color.Red
            Exit Sub
        Else
            Episode.EpisodeNode.BackColor = Color.Transparent   'i.e. back to normal
        End If

        tb_Sh_Ep_Title.Text = "'" & Utilities.ReplaceNothing(Episode.Title.Value, "?") & "'"
        tb_EpRating.Text = Utilities.ReplaceNothing(Episode.Rating.Value)
        tb_EpVotes.Text = Utilities.ReplaceNothing(Episode.Votes.Value)
        tb_EpPlot.Text = Utilities.ReplaceNothing(Episode.Plot.Value)
        tb_EpDirector.Text = Utilities.ReplaceNothing(Episode.Director.Value)
        tb_EpCredits.Text = Utilities.ReplaceNothing(Episode.Credits.Value)
        tb_EpAired.Text = Utilities.ReplaceNothing(Episode.Aired.Value)
        For f = 0 To cbTvSource.Items.Count - 1
            If cbTvSource.Items(f) = Episode.Source.value Then
                cbTvSource.SelectedIndex = f
                Exit For
            End If
        Next
        If Episode.Season.Value = "0" Then
            lbl_EpAirBefore.Visible = True
            lbl_EpAirSeason.Visible = True
            lbl_EPAirEpisode.Visible = True
            tb_EpAirEpisode.Visible = True
            tb_EpAirSeason.Visible = True
            tb_EpAirSeason.Text = Episode.DisplaySeason.Value
            tb_EpAirEpisode.Text = Episode.DisplayEpisode.Value
        Else
            lbl_EpAirBefore.Visible = False
            lbl_EpAirSeason.Visible = False
            lbl_EPAirEpisode.Visible = False
            tb_EpAirEpisode.Visible = False
            tb_EpAirSeason.Visible = False
            tb_EpAirEpisode.Text = ""
            tb_EpAirSeason.Text = ""
        End If

        util_EpisodeSetWatched(Episode.PlayCount.Value)

        Dim aActor As Boolean = False
        For Each actor In Episode.ListActors
            If Not String.IsNullOrEmpty(actor.actorname) Then
                cmbxEpActor.Items.Add(Utilities.ReplaceNothing(actor.actorname))
                aActor = True
            End If
        Next
        If aActor Then
            cmbxEpActor.SelectedIndex = 0
        Else
            cmbxEpActor.Items.Clear()
            cmbxEpActor.Items.Add("")
            cmbxEpActor.SelectedIndex = 0
            Panel_EpisodeActors.Visible = False
        End If

        If (Episode IsNot Nothing AndAlso Episode.Thumbnail IsNot Nothing) Then
            Dim eptvleft As String = Episode.Thumbnail.Path
            If Pref.FrodoEnabled Then eptvleft = Episode.Thumbnail.Path.Replace(".tbn", "-thumb.jpg")
            util_ImageLoad(tv_PictureBoxLeft, eptvleft, Utilities.DefaultTvFanartPath)
        End If
        If (Season IsNot Nothing AndAlso Season.Poster IsNot Nothing) Then
            util_ImageLoad(tv_PictureBoxRight, Season.Poster.Path, Utilities.DefaultTvPosterPath)
            If Pref.FrodoEnabled Then
                util_ImageLoad(tv_PictureBoxBottom, Season.Banner.Path, Utilities.DefaultTvBannerPath)
            End If
        End If

        Dim video_flags = GetEpMediaFlags()
        movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, tb_EpRating.Text, video_flags)
        Panel_EpisodeInfo.Visible = True
        ShOrEploading = False
    End Sub
    
    Private Function ep_getIMDbRating(ByVal epid As String, ByRef rating As String, ByRef votes As String) As Boolean
        Dim aok As Boolean = True
        If String.IsNullOrEmpty(epid) Then Return False
        Dim url As String = String.Format("http://akas.imdb.com/title/{0}/", epid)
        Dim IMDbpage As String = Utilities.DownloadTextFiles(url, True)
        Dim m As Match
        m = Regex.Match(IMDbpage, "<span itemprop=""ratingValue"">(.*?)</span>")
        If m.Success Then rating = m.Groups(1).Value
        Dim n As Match
        n = Regex.Match(IMDbpage, "itemprop=""ratingCount"">(.*?)</span")
        If n.Success Then votes = n.Groups(1).Value
        Return rating <> ""
    End Function

    'Public Function ep_Get(ByVal tvdbid As String, ByVal sortorder As String, ByRef seasonno As String, ByRef episodeno As String, ByVal language As String, ByVal aired As String)
    '    Dim episodestring As String = ""
    '    Dim episodeurl As String = ""
    '    Dim episodeurl2 As String = ""
    '    Dim xmlfile As String = ""

    '    If language.ToLower.IndexOf(".xml") = -1 Then
    '        language = language & ".xml"
    '    End If
    '    episodeurl2 = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seasonno & "/" & episodeno & "/" & language
    '    If aired = Nothing Then
    '        episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & seasonno & "/" & episodeno & "/" & language
    '    Else
    '        episodeurl = String.Format("http://thetvdb.com/api/GetEpisodeByAirDate.php?apikey=6E82FED600783400&seriesid={0}&airdate={1}&language={2}", tvdbid, aired, language)
    '    End If
    '    'First try seriesxml data
    '    'check if present, download if not
    '    Dim gotseriesxml As Boolean = False
    '    Dim url As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/all/" & language
    '    Dim xmlfile2 As String = Utilities.SeriesXmlPath & tvdbid & ".xml"
    '    Dim SeriesInfo As New Tvdb.ShowData
    '    If Not File.Exists(Utilities.SeriesXmlPath & tvdbid & ".xml") Then
    '        gotseriesxml = DownloadCache.Savexmltopath(url, Utilities.SeriesXmlPath, tvdbid & ".xml", True)
    '    Else
    '        'Check series xml isn't older than Five days.  If so, re-download it.
    '        Dim dtCreationDate As DateTime = File.GetLastWriteTime(xmlfile2)
    '        Dim datenow As DateTime = Date.Now()
    '        Dim dif As Long = DateDiff(DateInterval.Day, dtCreationDate, datenow)
    '        If dif > If(aired <> Nothing, 1, 5) Then
    '            gotseriesxml = DownloadCache.Savexmltopath(url, Utilities.SeriesXmlPath, tvdbid & ".xml", True)
    '        Else
    '            gotseriesxml = True
    '        End If
    '    End If

    '    If Not gotseriesxml then
    '        xmlfile = Utilities.DownloadTextFiles(episodeurl)
    '        If xmlfile.Contains("No Results from SP") AndAlso (seasonno <> "-1" And episodeno <> "-1") Then xmlfile = Utilities.DownloadTextFiles(episodeurl2)
    '    Else
    '        SeriesInfo.Load(xmlfile2)
    '        Dim gotEpxml As Boolean = False
    '        'check episode is present in seriesxml file, else, re-download it (update to latest)
    '        If aired <> Nothing Then
    '            For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
    '                If NewEpisode.FirstAired.Value = aired Then
    '                    xmlfile = NewEpisode.Node.ToString
    '                    xmlfile = "<Data>" & xmlfile & "</Data>"
    '                    gotEpxml = True
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '        If Not gotEpxml AndAlso (seasonno <> "-1" And episodeno <> "-1") Then
    '            If sortorder = "default" Then
    '                For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
    '                    If NewEpisode.EpisodeNumber.Value = episodeno AndAlso NewEpisode.SeasonNumber.Value = seasonno Then
    '                        xmlfile = NewEpisode.Node.ToString
    '                        xmlfile = "<Data>" & xmlfile & "</Data>"
    '                        gotEpxml = True
    '                        Exit For
    '                    End If
    '                Next
    '            ElseIf sortorder = "dvd" Then
    '                For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
    '                    If NewEpisode.DvdEpisodeNumber.value = (episodeno & ".0") AndAlso NewEpisode.DvdSeason.Value = seasonno Then
    '                        xmlfile = NewEpisode.Node.ToString
    '                        xmlfile = "<Data>" & xmlfile & "</Data>"
    '                        gotEpxml = True
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        End If
    '        ' Finally, if not in seriesxml file, go old-school
    '        If Not gotEpxml Then
    '            xmlfile = Utilities.DownloadTextFiles(episodeurl)
    '            If xmlfile.Contains("No Results from SP") AndAlso (seasonno <> "-1" And episodeno <> "-1") Then xmlfile = Utilities.DownloadTextFiles(episodeurl2)
    '        End If
    '    End If

    '    If xmlfile.Contains("Could not connect") OrElse xmlfile.Contains("No Results from SP") Then Return xmlfile               ' Added check if TVDB is unavailable.
    '    Dim xmlOK As Boolean = Utilities.CheckForXMLIllegalChars(xmlfile)
    '    If xmlOK Then
    '        episodestring = "<episodedetails>"
    '        episodestring = episodestring & "<url>" & If(IsNothing(aired), episodeurl, episodeurl2) & "</url>"
    '        Dim mirrorslist As New XmlDocument
    '        mirrorslist.LoadXml(xmlfile)
    '        For Each thisresult As XmlNode In mirrorslist("Data")
    '            Select Case thisresult.Name
    '                Case "Episode"
    '                    Dim mirrorselection As XmlNode = Nothing
    '                    For Each mirrorselection In thisresult.ChildNodes
    '                        Select Case mirrorselection.Name
    '                            Case "EpisodeName"
    '                                episodestring = episodestring & "<title>" & mirrorselection.InnerXml & "</title>"
    '                            Case "FirstAired"
    '                                episodestring = episodestring & "<premiered>" & mirrorselection.InnerXml & "</premiered>"
    '                            Case "GuestStars"
    '                                Dim gueststars() As String = mirrorselection.InnerXml.Split("|")
    '                                For Each guest In gueststars
    '                                    If Not String.IsNullOrEmpty(guest) Then episodestring = episodestring & "<actor><name>" & guest & "</name></actor>"
    '                                Next
    '                            Case "Director"
    '                                Dim tempstring As String = mirrorselection.InnerXml
    '                                tempstring = tempstring.Trim("|")
    '                                episodestring = episodestring & "<director>" & tempstring & "</director>"
    '                            Case "Writer"
    '                                Dim tempstring As String = mirrorselection.InnerXml
    '                                tempstring = tempstring.Trim("|")
    '                                episodestring = episodestring & "<credits>" & tempstring & "</credits>"
    '                            Case "Overview"
    '                                episodestring = episodestring & "<plot>" & mirrorselection.InnerXml & "</plot>"
    '                            Case "Rating"
    '                                episodestring = episodestring & "<rating>" & mirrorselection.InnerXml & "</rating>"
    '                            Case "RatingCount"
    '                                episodestring = episodestring & "<ratingcount>" & mirrorselection.InnerXml & "</ratingcount>"
    '                            Case "IMDB_ID"
    '                                episodestring = episodestring & "<imdbid>" & mirrorselection.InnerXml & "</imdbid>"
    '                            Case "id"
    '                                episodestring = episodestring & "<uniqueid>" & mirrorselection.InnerXml & "</uniqueid>"
    '                            Case "seriesid"
    '                                episodestring = episodestring & "<showid>" & mirrorselection.InnerXml & "</showid>"
    '                            Case "filename"
    '                                episodestring = episodestring & "<thumb>http://www.thetvdb.com/banners/" & mirrorselection.InnerXml & "</thumb>"
    '                            Case "airsbefore_episode"
    '                                episodestring = episodestring & "<displayepisode>" & mirrorselection.InnerXml & "</displayepisode>"
    '                            Case "airsbefore_season"
    '                                episodestring = episodestring & "<displayseason>" & mirrorselection.InnerXml & "</displayseason>"
    '                            Case "SeasonNumber"
    '                                If sortorder = "default" Then seasonno = mirrorselection.InnerText
    '                            Case "EpisodeNumber"
    '                                If sortorder = "default" Then episodeno = mirrorselection.InnerText
    '                            Case "DVD_season"
    '                                If sortorder = "dvd" Then seasonno = mirrorselection.InnerText
    '                            Case "DVD_episodenumber"
    '                                If sortorder = "dvd" Then episodeno = mirrorselection.InnerText.Substring(0, mirrorselection.InnerText.IndexOf("."))
    '                                episodestring = episodestring & "<dvd_episodenumber>" & mirrorselection.InnerXml & "</dvd_episodenumber>"
    '                        End Select
    '                    Next
    '            End Select
    '        Next
    '        episodestring = episodestring & "</episodedetails>"
    '    Else
    '        If CheckBoxDebugShowTVDBReturnedXML.Checked = True Then MsgBox(xmlfile, MsgBoxStyle.OkOnly, "FORM1 getepisode - TVDB returned.....")
    '        episodestring = "Error"
    '    End If
    '    Return episodestring
    'End Function

    'Private Function ep_add(ByVal alleps As List(Of TvEpisode), ByVal path As String, ByVal show As String, ByVal scrapedok As Boolean)
    '    tvScraperLog = tvScraperLog & "!!! Saving episode" & vbCrLf
    '    WorkingWithNfoFiles.ep_NfoSave(alleps, path)
    '    If Pref.TvDlEpisodeThumb OrElse Pref.autoepisodescreenshot Then
    '        tvScraperLog &= tv_EpisodeFanartGet(alleps(0), Pref.TvDlEpisodeThumb, Pref.autoepisodescreenshot) & vbcrlf
    '    Else
    '        tvScraperLog &= "!!! Skipped download of episode thumb" & vbCrLf
    '    End If

    '    If Not scrapedok AndAlso Pref.TvEpSaveNfoEmpty Then Return path

    '    If Pref.autorenameepisodes = True Then
    '        Dim eps As New List(Of String)
    '        For Each ep In alleps
    '            eps.Add(ep.Episode.Value)
    '        Next
    '        Dim tempspath As String = TVShows.episodeRename(path, alleps(0).Season.Value, eps, show, alleps(0).Title.Value, Pref.TvRenameReplaceSpace, Pref.TvRenameReplaceSpaceDot)

    '        If tempspath <> "false" Then path = tempspath
    '    End If
    '    Return path
    'End Function

    'Private Function GetEpRating(ByRef tvep As TvEpisode, ByVal Rating As String, ByVal Votes As String) As Boolean
    '    Dim ratingdone As Boolean = False
    '    If Pref.tvdbIMDbRating Then
    '        Dim aok As Boolean = False

    '        '''If no Ep IMDB Id, try getting from TMDb first.
    '        If String.IsNullOrEmpty(tvep.ImdbId.Value) Then aok = gettmdbepid(tvep)

    '        '''Check if not still empty and if is IMDB Id
    '        If Not String.IsNullOrEmpty(tvep.ImdbId.Value) AndAlso tvep.ImdbId.Value.StartsWith("tt") Then aok = True

    '        '''Try IMDb Direct if we have the IMDB Id
    '        If aok Then ratingdone = ep_getIMDbRating(tvep.ImdbId.Value, tvep.Rating.Value, tvep.Votes.Value)

    '        '' Disable Omdbapi to get episode ratings till we get a API key, or site becomes free again.
    '        ''If no success, try Omdbapi (Omdbapi is much slower)
    '        'If Not ratingdone Then ratingdone = epGetImdbRatingOmdbapi(tvep)
    '    End If

    '    ''' Fallback to TVDb if nothing from IMDb
    '    If Not ratingdone Then
    '        tvep.Rating.Value = Rating
    '        tvep.Votes.Value = Votes
    '        ratingdone = True
    '    End If
    '    Return ratingdone
    'End Function

    'Private Function epGetImdbRatingOmdbapi(ByRef ep As TvEpisode) As Boolean
    '    Dim GotEpImdbId As Boolean = False
    '    If (String.IsNullOrEmpty(ep.Showimdbid.Value) OrElse ep.Showimdbid.Value = "0") AndAlso String.IsNullOrEmpty(ep.ImdbId.Value) Then Return False
    '    If Not ep.Showimdbid.Value.StartsWith("tt") AndAlso (String.IsNullOrEmpty(ep.Season.Value) AndAlso String.IsNullOrEmpty(ep.Episode.Value)) Then Return False
    '    Dim url As String = Nothing

    '    If Not String.IsNullOrEmpty(ep.ImdbId.Value) AndAlso ep.ImdbId.Value.StartsWith("tt") Then GotEpImdbId = True

    '    If Not GotEpImdbId Then url = String.Format("{0}?i={1}&Season={2}&r=xml", Pref.Omdbapiurl, ep.Showimdbid.Value, ep.Season.Value)

    '    Dim imdb As New Classimdb
    '    If Not GotEpImdbId Then
    '        Dim result As String = imdb.loadwebpage(Pref.proxysettings, url, True, 5)
    '        If result.Contains("error") Then Return False
    '        Dim adoc As New XmlDocument
    '        adoc.LoadXml(result)
    '        If adoc("root").HasAttribute("response") AndAlso adoc("root").Attributes("response").Value = "False" Then Return False
    '        If adoc("root").HasAttribute("Response") AndAlso adoc("root").Attributes("Response").Value = "False" Then Return False
    '        For each thisresult As XmlNode In adoc("root")
    '            If Not IsNothing(thisresult.Attributes.ItemOf("Episode")) Then
    '                Dim TmpValue As String = thisresult.Attributes("Episode").Value
    '                If TmpValue <> "" AndAlso TmpValue = ep.Episode.Value Then
    '                    Dim epimdbid As String = thisresult.Attributes("imdbID").Value
    '                    ep.ImdbId.Value = epimdbid
    '                    Exit For
    '                End If
    '            End If
    '        Next
    '        If Not ep.ImdbId.Value.StartsWith("tt") Then Return False
    '    End If

    '    url = String.Format("{0}?i={1}&r=xml", Pref.Omdbapiurl, ep.ImdbId.Value)
    '    Dim result2 As String = imdb.loadwebpage(Pref.proxysettings, url, True, 5)
    '    If result2 = "error" Then Return False
    '    Dim bdoc As New XmlDocument
    '    bdoc.LoadXml(result2)
    '    If bdoc("root").HasAttribute("response") AndAlso bdoc("root").Attributes("response").Value = "False" Then Return False
    '    If bdoc("root").HasAttribute("Response") AndAlso bdoc("root").Attributes("Response").Value = "False" Then Return False

    '    For each thisresult As XmlNode In bdoc("root")
    '        If Not IsNothing(thisresult.Attributes.ItemOf("imdbRating")) Then
    '            Dim ratingVal As String = thisresult.Attributes("imdbRating").Value
    '            If ratingVal.ToLower = "n/a" Then Return False
    '            ep.Rating.Value = ratingVal
    '        End If
    '        If Not IsNothing(thisresult.Attributes.ItemOf("imdbVotes")) Then
    '            Dim voteval As String = thisresult.Attributes("imdbVotes").Value
    '            If Not voteval.ToLower = "n/a" Then ep.Votes.Value = voteval
    '        End If
    '    Next

    '    Return True
    'End Function

    'Private Function gettmdbepid(ByRef ep As TvEpisode) As Boolean
    '    Dim url As String = String.Format("https://api.themoviedb.org/3/find/{0}?api_key={1}&language=en-US&external_source=tvdb_id", ep.ShowId.Value, Utilities.TMDBAPI)
    '    Dim imdb As New Classimdb
    '    Try
    '        Dim reply As String = imdb.loadwebpage(Pref.proxysettings, url, True)
    '        If reply <> "error" Then
    '            Dim m As Match = Regex.Match(reply, """id"":(.*?),""")
    '            If Not m.Success Then Return False
    '            Dim pie As String = m.Groups(1).Value.Trim
    '            url = String.Format("https://api.themoviedb.org/3/tv/{0}/season/{1}/episode/{2}/external_ids?api_key={3}&language=en-US", pie, ep.Season.Value, ep.Episode.Value, Utilities.TMDBAPI)
    '            Dim reply2 As String = imdb.loadwebpage(Pref.proxysettings, url, True)
    '            If reply2 = "" Then Return False
    '            Dim n As Match = Regex.Match(reply2, """imdb_id"":""(.*?)"",""")
    '            If n.Success Then
    '                Dim epimdbid As String = n.Groups(1).Value.Trim
    '                If epimdbid.StartsWith("tt") AndAlso epimdbid.Length = 9 Then
    '                    ep.ImdbId.Value = epimdbid
    '                    Return True
    '                End If
    '            End If
    '        End If
    '    Catch
    '    End Try
    '    Return False
    'End Function

    Public Sub ep_VideoSourcePopulate()
        Try
            cbTvSource.Items.Clear()
            cbTvSource.Items.Add("")
            For Each mset In Pref.releaseformat
                cbTvSource.Items.Add(mset)
            Next

            cbTvSource.SelectedIndex = 0
        Catch ex As Exception
#If SilentErrorScream Then
            Throw ex
#End If
        End Try
    End Sub

    Public Sub SaveShowOrEpisode(Optional ByVal ChangeImdbId As Boolean = False)
        Try
            Dim Show As Media_Companion.TvShow = Nothing
            Dim Season As Media_Companion.TvSeason = Nothing
            Dim Episode As Media_Companion.TvEpisode = Nothing
            If TvTreeview.SelectedNode IsNot Nothing Then
                If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow Then
                    Show = TvTreeview.SelectedNode.Tag
                ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then
                    Episode = TvTreeview.SelectedNode.Tag
                ElseIf TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvSeason Then
                    Exit Sub
                Else
                    Exit Sub
                End If
            Else
                Exit Sub
            End If

            Dim tempint As Integer = 0
            Dim tempstring As String = ""
            If Show IsNot Nothing Then
                Dim changed As Integer = 0
                If Utilities.ReplaceNothing(Show.TvdbId.Value) <> tb_ShTvdbId.Text Then changed += 1
                If Utilities.ReplaceNothing(Show.ImdbId.Value).ToLower <> tb_ShImdbId.Text.ToLower Then changed += 2
                If changed > 0 Then
                    If changed = 1 Then
                        tempint = MessageBox.Show("It appears that you have changed the TVDB ID" & vbCrLf & "Media Companion depends on this ID for scraping episodes And art" & vbCrLf & vbCrLf & "Are you sure you wish to continue And save this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        If tempint = DialogResult.No Then Exit Sub
                    ElseIf changed = 2 Then
                        tempint = MessageBox.Show("It appears that you have changed the IMDB ID" & vbCrLf & "Media Companion depends on this ID for scraping actors from IMDB" & vbCrLf & vbCrLf & "Are you sure you wish to continue And save this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        If tempint = DialogResult.No Then Exit Sub
                    ElseIf changed = 3 Then
                        tempint = MessageBox.Show("It appears that you have changed the IMDB ID & TVDB ID" & vbCrLf & "Media Companion depends on these IDs being correct for a number of scraping operations" & vbCrLf & vbCrLf & "Are you sure you wish to continue And save this?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        If tempint = DialogResult.No Then Exit Sub
                    End If
                End If
                'its a tvshow
                Dim TmpTitle As String = ""
                If tb_Sh_Ep_Title.Text.ToLower.IndexOf(", the") = tb_Sh_Ep_Title.Text.Length - 5 And tb_Sh_Ep_Title.Text.Length > 5 Then
                    TmpTitle = "The " & tb_Sh_Ep_Title.Text.Substring(0, tb_Sh_Ep_Title.Text.Length - 5)
                Else
                    TmpTitle = tb_Sh_Ep_Title.Text
                End If

                If TmpTitle <> Show.Title.Value Then
                    Dim TryTitle As MsgBoxResult = MsgBox(" You have changed this Show's Title " & vbCrLf & "Are you sure you want to accept this change", MsgBoxStyle.YesNo)
                    If TryTitle = MsgBoxResult.No Then
                        tb_Sh_Ep_Title.Text = Show.Title.Value
                        Exit Sub
                    End If
                    Show.Title.Value = TmpTitle
                End If
                Show.Plot       .Value = tb_ShPlot.Text
                Show.Runtime    .Value = tb_ShRunTime.Text
                Show.Premiered  .Value = tb_ShPremiered.Text
                Show.Studio     .Value = tb_ShStudio.Text
                Show.Rating     .Value = tb_ShRating.Text
                Show.ImdbId     .Value = tb_ShImdbId.Text
                Show.TvdbId     .Value = tb_ShTvdbId.Text
                Show.Mpaa       .Value = tb_ShCert.Text
                Show.Genre      .Value = tb_ShGenre.Text
                Show.UserRating .Value = If(cmbx_shUserRating.Text = "None", "0", cmbx_shUserRating.Text)
                Show.SortTitle  .Value = If(TextBox_Sorttitle.Text <> Show.Title.Value, TextBox_Sorttitle.Text, "")

                nfoFunction.tvshow_NfoSave(Show, True)   'Show.Save()
                Show.UpdateTreenode()
            Else
                Dim trueseason As String = Utilities.PadNumber(Episode.Season.Value, 2)
                Do While trueseason.Substring(0, 1) = "0" AndAlso trueseason.Length <> 1
                    trueseason = trueseason.Substring(1, trueseason.Length - 1)
                Loop
                Dim trueepisode As String = Utilities.PadNumber(Episode.Episode.Value, 2)
                Do While trueepisode.Substring(0, 1) = "0" AndAlso trueepisode.Length <> 1
                    trueepisode = trueepisode.Substring(1, trueepisode.Length - 1)
                Loop
                Dim episodelist As New List(Of TvEpisode)
                episodelist = WorkingWithNfoFiles.ep_NfoLoad(Episode.NfoFilePath)
                For Each ep In episodelist
                    If ep.Season.Value = trueseason And ep.Episode.Value = trueepisode Then
                        If tb_Sh_Ep_Title.Text.Replace("'", "").ToLower <> ep.Title.Value.ToLower Then
                            If MsgBox(" You have changed this Episode's Title " & vbCrLf & "Are you sure you want to accept this change", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                ep.Title.Value = tb_Sh_Ep_Title.Text.Replace("'", "")
                            End If
                        End If
                        ep.Plot         .Value = tb_EpPlot.Text
                        ep.Aired        .Value = tb_EpAired.Text
                        ep.Rating       .Value = tb_EpRating.Text
                        ep.UserRating   .Value = If(cmbx_EpUsrRating.Text = "None", "0", cmbx_EpUsrRating.Text)
                        ep.Credits      .Value = tb_EpCredits.Text
                        ep.Director     .Value = tb_EpDirector.Text
                        If ep.Season    .Value = "0" Then
                            ep.DisplayEpisode.Value = tb_EpAirEpisode.Text
                            ep.DisplaySeason.Value = tb_EpAirSeason.Text
                        End If
                        If ChangeImdbId Then ep.ImdbId.Value = tb_EpImdbId.Text
                        ep.Source.Value = If(cbTvSource.SelectedIndex = 0, "", cbTvSource.Items(cbTvSource.SelectedIndex))
                    End If
                Next
                WorkingWithNfoFiles.ep_NfoSave(episodelist, Episode.NfoFilePath)
                ep_Load(Episode.EpisodeNode.Parent.Tag, Episode, True)
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    ' We need to load images in this way so that they remain unlocked by the OS so we can update the fanart/poster files as needed
    Public Shared Function util_ImageLoad(ByVal PicBox As PictureBox, ByVal ImagePath As String, ByVal DefaultPic As String) As Boolean
        Dim PathToUse As String = DefaultPic
        Dim cachename As String = ""
        PicBox.Tag = Nothing
        If Utilities.UrlIsValid(ImagePath) Then
            cachename = Utilities.Download2Cache(ImagePath)
            If cachename <> "" Then PathToUse = cachename
        ElseIf File.Exists(ImagePath) Then
            PathToUse = ImagePath
        End If
        If PathToUse = "" Then
            PicBox.Image = Nothing
            Exit Function
        End If
        Try
            Using fs As IO.FileStream = File.Open(PathToUse, IO.FileMode.Open, IO.FileAccess.Read), ms As IO.MemoryStream = New IO.MemoryStream()
                fs.CopyTo(ms)
                ms.Seek(0, IO.SeekOrigin.Begin)
                If ms.Length = 0 Then Throw New Exception()
                PicBox.Image = Image.FromStream(ms)
            End Using
            PicBox.Tag = PathToUse
        Catch
            'Image is invalid e.g. not downloaded correctly -> Delete it
            Try
                File.Delete(PathToUse)
            Catch
            End Try
            Try
                Using fs As IO.FileStream = File.Open(DefaultPic, IO.FileMode.Open, IO.FileAccess.Read), ms As IO.MemoryStream = New IO.MemoryStream()
                    fs.CopyTo(ms)
                    ms.Seek(0, IO.SeekOrigin.Begin)
                    PicBox.Image = Image.FromStream(ms)
                End Using
                PicBox.ImageLocation = DefaultPic
            Catch
                Return False
            End Try
            Return True
        End Try
        Return True
    End Function


#Region "Tv Cache Routines"
    Public Sub tv_CacheLoad()
        Cache.TvCache.TvCachePath = Pref.workingProfile.TvCache
        Cache.TvCache.Load()
        TvTreeview.BeginUpdate()
        Try
            TvTreeview.Nodes.Clear()              'clear the treeview of old data
            ''Dirty work around until TvShows is repalced with TvCache.Shows universally
            For Each TvShow As Media_Companion.TvShow In Cache.TvCache.Shows
                Dim action As Action = Sub() TvTreeview.Nodes.Add(TvShow.ShowNode)
                TvTreeview.Invoke(action)
                'TvTreeview.Nodes.Add(TvShow.ShowNode)
                TvShow.UpdateTreenode()
            Next
        Finally
            TvTreeview.EndUpdate()
        End Try
        TextBox_TotTVShowCount.Text = Cache.TvCache.Shows.Count
        TextBox_TotEpisodeCount.Text = Cache.TvCache.Episodes.Count
    End Sub

    Private Sub tv_CacheRefreshSelected(ByVal Show As TvShow)
        tv_CacheRefresh(Show)
    End Sub

    Public Sub TvCacheRefresh()
        tv_CacheRefresh()
    End Sub

    Private Sub tv_CacheRefresh(Optional ByVal TvShowSelected As TvShow = Nothing) 'refresh = clear & recreate cache from nfo's
        Dim nfoclass As New WorkingWithNfoFiles
        frmSplash2.Text = "Refresh TV Shows..."
        frmSplash2.Label1.Text = "Searching TV Folders....."
        frmSplash2.Label1.Visible = True
        frmSplash2.Label2.Visible = True
        frmSplash2.ProgressBar1.Visible = True
        frmSplash2.ProgressBar1.Maximum = Pref.tvFolders.Count ' - 1
        If Pref.MultiMonitoEnabled Then
            frmSplash2.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
            frmSplash2.StartPosition = FormStartPosition.Manual
        End If
        If frmSplash.Visible Then frmSplash2.BringToFront
        frmSplash2.Show()
        Application.DoEvents()

        Dim fulltvshowlist As New List(Of TvShow)
        Dim fullepisodelist As New List(Of TvEpisode)

        TextBox_TotTVShowCount.Text = ""
        TextBox_TotEpisodeCount.Text = ""
        Me.Enabled = False

        Dim nofolder As New List(Of String)
        Dim prgCount As Integer = 0
        Dim FolderList As New List(Of String)

        If TvShowSelected IsNot Nothing Then ' if we have provided a tv show, then add just this show to the list, else scan through all of the folders
            FolderList.Add(TvShowSelected.FolderPath) 'add the single show to our list
            Cache.TvCache.Remove(TvShowSelected)
            For Each episode In TvShowSelected.Episodes
                If Pref.displayMissingEpisodes AndAlso episode.IsMissing = True Then
                    fullepisodelist.Add(episode)
                    Cache.TvCache.Remove(episode)
                Else
                    Cache.TvCache.Remove(episode)
                End If
            Next
            For Each showitem In Cache.TvCache.Shows
                fulltvshowlist.Add(showitem)
            Next
            For Each episodeitem In Cache.TvCache.Episodes
                fullepisodelist.Add(episodeitem)
            Next
        Else
            For Each ep In Cache.TvCache.Episodes
                If Pref.displayMissingEpisodes AndAlso ep.IsMissing = True Then fullepisodelist.Add(ep)
            Next
            FolderList = Pref.tvFolders ' add all folders to list to scan
            Cache.TvCache.Clear() 'Full rescan means clear all old data
            TvTreeview.Nodes.Clear()
        End If

        For Each tvfolder In FolderList

            frmSplash2.Label2.Text = "(" & prgCount + 1 & "/" & Pref.tvFolders.Count & ") " & tvfolder
            frmSplash2.ProgressBar1.Value = prgCount

            If Not Directory.Exists(tvfolder) OrElse Not File.Exists(tvfolder & "\tvshow.nfo") Then
                nofolder.Add(tvfolder)
                Continue For
            End If

            prgCount += 1
            Application.DoEvents()
            Dim newtvshownfo As New TvShow
            newtvshownfo.NfoFilePath = Path.Combine(tvfolder, "tvshow.nfo")

            newtvshownfo = nfoFunction.tvshow_NfoLoad(newtvshownfo.NfoFilePath) '.Load()
            If (IsNothing(newtvshownfo.Year.Value) OrElse newtvshownfo.Year.Value.ToInt = 0) Then
                If Not String.IsNullOrEmpty(newtvshownfo.Premiered.Value) AndAlso newtvshownfo.Premiered.Value.Length = 10 Then
                    newtvshownfo.Year.Value = newtvshownfo.Premiered.Value.Substring(0, 4)
                    nfoclass.tvshow_NfoSave(newtvshownfo, True)
                End If
            End If
            fulltvshowlist.Add(newtvshownfo.Cachedata)
            Dim episodelist As New List(Of TvEpisode)
            episodelist = loadepisodes(newtvshownfo, episodelist)
            For Each ep In episodelist
                ep.ShowId.Value = newtvshownfo.TvdbId.Value
                If Pref.displayMissingEpisodes Then
                    Dim q = From x In fullepisodelist Where x.IsMissing = True AndAlso x.UniqueId.Value = ep.UniqueId.Value
                    If Not q.Count = 0 Then
                        fullepisodelist.Remove(q(0))
                        
                    End If
                End If
                fullepisodelist.Add(ep.Cachedata)
            Next
        Next

        If nofolder.Count > 0 Then
            Dim mymsg As String
            mymsg = (nofolder.Count).ToString + " folder/s missing or no tvshow.nfo:" + vbCrLf + vbCrLf
            For Each item In nofolder
                mymsg = mymsg + item + vbCrLf
            Next
            mymsg = mymsg + vbCrLf + "Do you wish to remove these folders" + vbCrLf + "from your list of TV Folders?" + vbCrLf
            If frmSplash.Visible Then frmSplash.SendToBack()
            If MsgBox(mymsg, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                tv_Showremovedfromlist(lb_tvSeriesFolders, True, nofolder)
            End If
        End If
        frmSplash2.Label2.Visible = False
        
        frmSplash2.Label1.Text = "Saving Cache..."
        Windows.Forms.Application.DoEvents()
        Tv_RefreshCacheSave(fulltvshowlist, fullepisodelist)    'save the cache file
        
        frmSplash2.Label1.Text = "Loading Cache..."
        Windows.Forms.Application.DoEvents()
        tv_CacheLoad()    'reload the cache file to update the treeview
        Me.Enabled = True

        TextBox_TotTVShowCount.Text = Cache.TvCache.Shows.Count
        TextBox_TotEpisodeCount.Text = Cache.TvCache.Episodes.Count
        frmSplash2.Hide()
        If Not tv_IMDbID_warned And tv_IMDbID_detected Then
            MessageBox.Show(tv_IMDbID_detectedMsg, "TV Show ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            tv_IMDbID_warned = True
        End If
        tv_Filter()
        BlinkTaskBar()
    End Sub

    Private Sub Tv_RefreshCacheSave(ByVal tvshowlist As List(Of TvShow), ByVal episodeelist As List(Of TvEpisode))
        Dim tvcachepath As String = Pref.workingProfile.TvCache
        Dim doc As New XmlDocument
        Dim root As XmlElement
        Dim child As XmlElement
        Dim xmlproc As XmlDeclaration
        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        root = doc.CreateElement("tvcache")
        root.SetAttribute("ver", "3.5")
        For Each item In tvshowlist
            child = doc.CreateElement("tvshow")
            child.SetAttribute("NfoPath", item.NfoFilePath)
            child.AppendChild(doc, "state", item.State)
            child.AppendChild(doc, "title", item.Title.Value)
            child.AppendChild(doc, "id", item.TvdbId.Value)
            child.AppendChild(doc, "status", item.Status.Value)
            child.AppendChild(doc, "plot", item.Plot.Value)
            child.AppendChild(doc, "sortorder", item.SortOrder.Value)
            child.AppendChild(doc, "language", item.Language.Value)
            child.AppendChild(doc, "episodeactorsource", item.EpisodeActorSource.Value)
            child.AppendChild(doc, "imdbid", item.ImdbId.Value)
            child.AppendChild(doc, "playcount", item.Playcount.Value)
            child.AppendChild(doc, "hidden", item.Hidden.Value)
            root.AppendChild(child)
        Next

        For Each item In episodeelist
            child = doc.CreateElement("episodedetails")
            child.SetAttribute("NfoPath", item.NfoFilePath)
            child.AppendChild(doc, "missing", item.IsMissing)
            child.AppendChild(doc, "title", item.Title.Value)
            child.AppendChild(doc, "season", item.Season.Value)
            child.AppendChild(doc, "episode", item.Episode.Value)
            child.AppendChild(doc, "aired", item.Aired.Value)
            child.AppendChild(doc, "showid", item.ShowId.Value)
            child.AppendChild(doc, "uniqueid", item.UniqueId.Value)
            child.AppendChild(doc, "epextn", item.EpExtn.Value)
            child.AppendChild(doc, "playcount", item.PlayCount.Value)
            root.AppendChild(child)
        Next
        doc.AppendChild(root)
        WorkingWithNfoFiles.SaveXMLDoc(doc, tvcachepath)
    End Sub

    Private Sub Tv_CacheSave()
        Cache.TvCache.TvCachePath = Pref.workingProfile.TvCache
        Cache.TvCache.Save()
    End Sub

    Private Function loadepisodes(ByVal newtvshownfo As TvShow, ByRef episodelist As List(Of TvEpisode))
        Dim missingeppath As String = Utilities.MissingPath
        Dim newlist As New List(Of String)
        newlist.Clear()
        newlist = Utilities.EnumerateFolders(newtvshownfo.FolderPath) 'TODO: Restore loging functions
        newlist.Add(newtvshownfo.FolderPath)
        For Each folder In newlist
            If folder = "long_path" Then
                Continue For
            End If
            Dim dir_info As New DirectoryInfo(folder)
            Dim fs_infos() As FileInfo = dir_info.GetFiles("*.NFO", IO.SearchOption.TopDirectoryOnly)
            For Each fs_info As FileInfo In fs_infos
                Application.DoEvents()
                If Path.GetFileName(fs_info.FullName.ToLower) <> "tvshow.nfo" And fs_info.ToString.Substring(0, 2) <> "._" Then
                    Dim EpNfoPath As String = fs_info.FullName
                    If ep_NfoValidate(EpNfoPath) Then
                        Dim multiepisodelist As New List(Of TvEpisode)
                        Dim need2resave As Boolean = False
                        multiepisodelist = ep_NfoLoad(EpNfoPath)
                        For Each Ep In multiepisodelist
                            If Ep.ShowId.Value <> newtvshownfo.TvdbId.Value AndAlso newtvshownfo.TvdbId.Value <> "" Then need2resave = True
                            Ep.ShowObj = newtvshownfo
                            Dim missingNfoPath As String = missingeppath & newtvshownfo.TvdbId.Value & "." & Ep.Season.Value & "." & Ep.Episode.Value & ".nfo"
                            If File.Exists(missingNfoPath) Then Utilities.SafeDeleteFile(missingNfoPath)
                            episodelist.Add(Ep.Cachedata)
                        Next
                        If need2resave Then ep_NfoSave(multiepisodelist, EpNfoPath)    'If new ShowID stored, resave episode nfo.
                    End If
                End If
            Next fs_info
        Next
        Return episodelist
    End Function

    Private Sub tv_ShowListLoad()
        Try
            If tbTvShSelectSearchTitle.Text <> "" Then

                messbox = New frmMessageBox("Please wait,", "", "Getting possible TV Shows from TVDB")
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                messbox.Show()
                Me.Refresh()
                messbox.Refresh()
                Try
                    Dim showlist As New List(Of str_PossibleShowList)
                    Dim tvdbstuff As New TVDBScraper
                    Dim aok As Boolean = tvdbstuff.findshows(tbTvShSelectSearchTitle.Text, showlist)

                    listOfShows.Clear()

                    If Not aok Then
                        messbox.Close()
                        MsgBox("TVDB seems to have an error with the xml for this search")
                        Exit Sub
                    End If
                    If showlist.count > 0 Then
                        For each foundshow In showlist
                            Dim exists As Boolean = False
                            For Each item In listOfShows
                                If item.showid = foundshow.showid Then
                                    exists = True
                                    Exit For
                                End If
                            Next
                            If Not exists Then listOfShows.Add(foundshow)
                        Next
                    Else
                        Dim lan As New str_PossibleShowList(SetDefaults)
                        lan.showid = "none"
                        lan.showtitle = "TVDB Search Returned Zero Results"
                        lan.showbanner = Nothing
                        listOfShows.Add(lan)
                        
                        lan.showtitle = "Adjust the TV Shows Title & search again"
                        listOfShows.Add(lan)
                    End If
                    lbxTvShSelectResults.Items.Clear()
                    For Each item In listOfShows
                        lbxTvShSelectResults.Items.Add(item.showtitle)
                    Next

                    lbxTvShSelectResults.SelectedIndex = 0
                    If listOfShows(0).showbanner <> Nothing Then
                        Try
                            util_ImageLoad(pbTvShSelectPreviewImg, listOfShows(0).showbanner, Utilities.DefaultTvBannerPath)
                        Catch ex As Exception
                            pbTvShSelectPreviewImg.Image = Nothing
                        End Try
                    End If

                    Call util_LanguageCheck()
                    messbox.Close()
                Catch
                    If Not IsNothing(messbox) Then messbox.Close()
                    Throw New Exception()
                End Try
            Else
                MsgBox("Please Enter a Search Term")
            End If
        Catch ex As Exception
            MsgBox("There seems to be a problem with the tvdb website, please try again later")
            tvCurrentTabIndex = 0
            TabControl3.SelectedIndex = 0
        End Try
    End Sub

    Private Sub Tv_CacheCheckDuplicates()
        Dim progress As String = ""
        Dim Showfound As String = ""
        Dim Episodesfound As String = ""
        Dim Count As Integer = 0
        Dim lasttestedseason As String = ""
        Dim lasttestedepisode As String = ""
        For Each sh In Cache.TvCache.shows
            If Not String.IsNullOrEmpty(sh.TvdbId.Value) Then
                Dim unique As Integer = 0
                Dim shid As Integer = 0
                Showfound = sh.Title.Value
                For Each episo In sh.episodes
                    If String.IsNullOrEmpty(episo.ShowId.Value) Then shid += 1
                    If String.IsNullOrEmpty(episo.UniqueId.Value) Then unique += 1
                    If Not String.IsNullOrEmpty(episo.ShowId.Value) Then
                        Dim Thisseason As String = episo.Season.Value
                        Dim Thisepisode As String = episo.Episode.Value
                        If Thisseason = lasttestedseason AndAlso Thisepisode = lasttestedepisode Then Continue For
                        Dim testShow As String = episo.ShowId.Value
                        For Each testep In sh.episodes
                            If Not String.IsNullOrEmpty(testep.ShowId.Value) AndAlso testep.ShowId.Value = testShow Then
                                If testep.Season.Value = Thisseason AndAlso testep.Episode.Value = Thisepisode Then
                                    Count += 1
                                    Episodesfound &= testep.NfoFilePath & vbcrlf
                                End If
                            End If
                        Next
                        If Count > 1 Then
                            progress &= "Duplicates Found in:" & vbCrLf & vbcrlf
                            progress &= Showfound & vbCrLf & Episodesfound & vbCrLf & vbcrlf
                        End If
                        Count = 0
                        Episodesfound = ""
                        lasttestedseason = Thisseason
                        lasttestedepisode = Thisepisode
                    End If
                Next
                If shid > 0 Then progress &= "Show:- " & sh.Title.Value & vbCrLf & "contains: - """ & shid & """  episodes without valid Tvdb Id!" & vbcrlf & vbcrlf
                If unique > 0 Then progress &= "Show:- " & sh.Title.Value & vbCrLf & "contains: - """ & unique & """  episodes without valid <uniqueid> Tag!" & vbcrlf & vbcrlf
            Else
                progress &= "Show:- " & sh.Title.Value & vbCrLf & "does not have a TVDB ID.  Skipped for checking Duplicates" & vbcrlf & vbcrlf
            End If
            Showfound = ""
        Next
        If Not progress = "" Then
            Dim MyFormObject As New frmoutputlog(progress, True, True)
            MyFormObject.ShowDialog()
        Else
            MsgBox("No Duplicates found")
        End If
    End Sub

#End Region

#Region "New TV Series and Episode Scraper"
    Private Sub BckWrkTv_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BckWrkTv.DoWork
        Try
            e.Result = e.Argument
            CallTVSubByName(DirectCast(e.Argument, String))
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub BckWrkTv_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BckWrkTv.ProgressChanged
        Try
            If e.ProgressPercentage = 9999 Then
                Dim NewTvShow As TvShow = e.UserState
                Cache.TvCache.Add(NewTvShow)
                TvTreeview.Nodes.Add(NewTvShow.ShowNode)
                NewTvShow.UpdateTreenode()

                TextBox_TotTVShowCount.Text = Cache.TvCache.Shows.Count
                TextBox_TotEpisodeCount.Text = Cache.TvCache.Episodes.Count
                Exit Sub
            End If
            If e.ProgressPercentage = 999 Then
                Dim TempEpisode As TvEpisode = CType(e.UserState, TvEpisode)
                TempEpisode.ShowObj.AddEpisode(TempEpisode)
                TempEpisode.SeasonObj.UpdateTreenode()
                TempEpisode.UpdateTreenode(True)
                'This bit updates the Epsiode Count on the fly when the progress is updated. It has to be done here to avoid thread issues. (GUI wouldn't update properly) 
                TextBox_TotEpisodeCount.Text = Cache.TvCache.Episodes.Count
                Exit Sub
            End If

            Dim oProgress As Progress = CType(e.UserState, Progress)
            If e.ProgressPercentage <> -1 Then
                tsMultiMovieProgressBar.Value = e.ProgressPercentage
                If IsNothing(oProgress) Then Exit Sub
            End If

            If oProgress.Message <> Nothing Then
                If oProgress.Command = Progress.Commands.Append Then
                    Dim msgtxt As String = tsStatusLabel.Text & oProgress.Message
                    If msgtxt.Length > 144 AndAlso oProgress.Message <> "-OK" Then
                        msgtxt = tsStatusLabel.Text.Substring(0, (tsStatusLabel.Text.ToLower.IndexOf("actors-ok") + 9))
                        msgtxt &= oProgress.Message
                    End If
                    tsStatusLabel.Text = msgtxt
                Else
                    tsStatusLabel.Text = oProgress.Message
                End If
            End If

            If oProgress.Message = Movie.MSG_ERROR Then ScraperErrorDetected = True

            If Not IsNothing(oProgress.Log) Then scraperLog += oProgress.Log

        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub BckWrkTv_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BckWrkTv.RunWorkerCompleted

        tsStatusLabel.Visible = False
        tsMultiMovieProgressBar.Visible = False
        tsLabelEscCancel.Visible = False
        Statusstrip_Enable(False)
        If e.Result = "TVSeriesSearchForNew" Then
            oTV.ListOfShows.Clear()
            oTV.EpForceSearch = False
        End If
        If e.Result = "TVEpisodeRescrape" Then
            tv_EpisodeSelected(TvTreeview.SelectedNode.Tag, True)
            Tv_CacheSave()
            Exit Sub
        End If
        If e.Result = "TVBatchRescrape" Then
            oTV.tvBatchList = New str_TvShowBatchWizard(SetDefaults)
            oTV.SingleShow = False
            TvTreeview_AfterSelect_Do()
            GC.Collect()
        End If
        Tv_CacheSave()
        tv_CacheLoad()
        tv_Filter()
        btnTvSearchNew.Text = "Search New"
        Dim Displayed As Boolean = DisplayLogFile()
        If Not Displayed Then BlinkTaskBar()
        TvAutoScrapeTimerTripped = False
        GC.Collect()
        
    End Sub

    Function GetTV_MultiMovieProgressBar_Visiblity(action As String)

        Select Case action
            Case "TVSeriesSearchForNew"     : Return False
            Case "TVSeriesChange"           : Return False
            Case "TVSeriesRescrape"         : Return False
            Case "TvEpisodesSearchforNew"   : Return False
            Case "TVEpisodeRescrape"        : Return False
            Case "TVBatchRescrape"          : Return True   '_rescrapeList.FullPathAndFilenames.Count > 1               ' filteredList.Count > 1
                'Case "RescrapeAll"                 : Return _rescrapeList.FullPathAndFilenames.Count > 1
                'Case "RescrapeSpecific"            : Return _rescrapeList.FullPathAndFilenames.Count > 1
                'Case "LockSpecific"                : Return _lockList.FullPathAndFilenames.Count > 1
                'Case "RebuildCaches"               : Return True
        End Select

        MsgBox("Unrecognised scrape action : [" + action + "]!", MsgBoxStyle.Exclamation, "Programming Error!")
        Return False
    End Function

    Public Sub CallTVSubByName(SubName As String)
        Me.GetType.GetMethod(SubName).Invoke(Me, Nothing)
    End Sub

    Public Sub TVSeriesSearchForNew()
        Dim x As String = newTvFolders.Count.ToString
        If x = 0 Then Exit Sub
        Dim ismulti As Boolean = x > 1
        Dim i As Integer = 0
        oTV.ReportProgress(, "Found Folders:" & vbCrLf & String.Join(vbcrlf, newTvFolders.ToArray()) & vbCrLf & vbcrlf)
        For each tvseries In newTvFolders
            i += 1
            oTV.ProgressStart = String.Format("Scraping Show {0} of {1} : ", i.ToString, x)
            Dim args As New TvdbArgs("", tvseries, False, Pref.TvdbLanguageCode)
            TVDoScrape(args)
            Pref.tvFolders.AddIfNew(tvseries)
        Next
        newTvFolders.Clear()
    End Sub

    Public Sub TVSeriesChange()
        oTV.Reportprogress(, "Changing Series in folder: " & newTvFolders(0) & " - with Series title: " & listOfShows(0).showtitle & vbCrLf)
        oTV.ProgressStart = String.Format("Changing to Series {0} - Tvdb Id: {1}. - ", listOfShows(0).showtitle, listOfShows(0).showid)
        Dim args As TvdbArgs = New TvdbArgs(listOfShows(0).showid, newTvFolders(0), False, listOfShows(0).langcode)
        TVDoScrape(args)
    End Sub
    
    Public Sub TVSeriesRescrape()
        Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
        For Each episode In WorkingTvShow.Episodes
            If Pref.displayMissingEpisodes AndAlso episode.IsMissing = True Then
                Cache.TvCache.Remove(episode)
            Else
                Cache.TvCache.Remove(episode)
            End If
        Next
        oTV.ReportProgress(, "Rescraping Series: " & WorkingTvShow.Title.Value & vbCrLf)
        Cache.TvCache.Remove(WorkingTvShow)
        newTvFolders.Add(WorkingTvShow.FolderPath.Substring(0, WorkingTvShow.FolderPath.LastIndexOf("\")))
        Dim args As New TvdbArgs(WorkingTvShow.TvdbId.Value, newTvFolders(0), False, WorkingTvShow.Language.Value)
        oTV.ProgressStart = String.Format("Rescraping Show: {0} : ", WorkingTvShow.Title.Value)
        TVDoScrape(args)
    End Sub

    Public Sub TvEpisodesSearchforNew()
        Dim args As New TvdbArgs("", "", True, Pref.TvdbLanguageCode)
        TVDoScrape(args)
    End Sub

    Public Sub TVEpisodeRescrape()
        Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
        Cache.TvCache.Remove(WorkingEpisode)
        Dim uid As String = WorkingEpisode.UniqueId.Value
        Dim eppath As String = WorkingEpisode.NfoFilePath.Replace(".nfo", WorkingEpisode.EpExtn.Value)
        Dim eplang As String = WorkingEpisode.ShowLang.Value
        Dim args As New TvdbArgs(uid, eppath, True, eplang)
        oTV.ProgressStart = String.Format("Rescraping Episode: {0} : ", WorkingEpisode.Title.Value)
        TVDoScrape(args)
    End Sub

    Public Sub TVBatchRescrape()
        oTV.BatchRescrape
    End Sub

    Public Sub TVDoScrape(args As TvdbArgs)
        oTV.Scraped = False
        oTV.Scrape(args)
    End Sub

    Sub RunBackgroundTVScrape(action As String)
        If Not BckWrkTv.IsBusy Then
            scraperLog = ""
            tsStatusLabel.Text = ""
            tsMultiMovieProgressBar.Value = tsMultiMovieProgressBar.Minimum
            tsMultiMovieProgressBar.Visible = GetTV_MultiMovieProgressBar_Visiblity(action)
            tsStatusLabel.Text = "Initializing..."
            tsStatusLabel.Visible = True
            tsLabelEscCancel.Visible = True
            Statusstrip_Enable()
            ssFileDownload.Visible = False
            tsProgressBarFileDownload_Resize()
            Application.DoEvents()

            BckWrkTv.RunWorkerAsync(action)
            While BckWrkTv.IsBusy
                Application.DoEvents()
            End While
        Else
            MsgBox("The TV Scraper Is Already Running")
        End If
    End Sub

#End Region

    'Function CalcPercentDone(onNumber As Integer, total As Integer) As Integer
    '    Try
    '        If total = 0 Then total = onNumber
    '        Return Math.Min((100 / total) * onNumber, 100)
    '    Catch
    '        Return 1
    '    End Try
    'End Function

#Region "New TV Series XMl Background workers"

    Private Sub GetSeriesData()
        If BckWrkSeries.IsBusy AndAlso tvseriestoget.Count > 0 Then Exit Sub
        Do Until tvseriestoget.Count = 0
            BckWrkSeries.RunWorkerAsync(tvseriestoget(0))
            While BckWrkSeries.IsBusy
                Application.DoEvents()
            End While
            tvseriestoget.RemoveAt(0)
        Loop
        MsgBox("COmpleted")
    End Sub
    
    Private Sub BckWrkSeries_DoWork(sender As Object, e As DoWorkEventArgs) Handles BckWrkSeries.DoWork
        Dim tvseries As TvSeriesData = e.Argument
        If tvseries.SeriesId = "" Then Exit Sub
        Dim tvdb As New TVDBScraper2(tvseries.SeriesId, tvseries.SeriesLan)
        Dim alldata As New TheTvDB.TvdbSeries
        alldata = tvdb.ReturnSeriesandEpisodes
        nfoFunction.SeriesXMLSave(alldata, "C:\temp\" & tvseries.SeriesId & "-" & tvseries.SeriesLan & ".xml", True)
    End Sub

    Private Sub BckWrkSeries_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BckWrkSeries.ProgressChanged
       
    End Sub

    Private Sub BckWrkSeries_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BckWrkSeries.RunWorkerCompleted
        
        'tvseriestoget.RemoveAt(0)
    End Sub

#End Region

#Region "Tv Series Scraper"
    'Private Sub bckgrnd_tvshowscraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bckgrnd_tvshowscraper.DoWork
    '    Try
    '        Statusstrip_Enable()
    '        Dim nfoFunction As New WorkingWithNfoFiles
    '        Dim args As TvdbArgs = e.Argument
    '        Dim searchTVDbID As String = If(IsNothing(args), "", args.tvdbid)
    '        Dim searchLanguage As String = If(IsNothing(args), Pref.TvdbLanguageCode, args.lang)
    '        Dim haveTVDbID As Boolean = Not String.IsNullOrEmpty(searchTVDbID)
    '        Dim success As Boolean = False
    '        Dim i As Integer = 0
    '        Dim x As String = newTvFolders.Count.ToString
    '        Do While newTvFolders.Count > 0
    '            tvprogresstxt = ""
    '            i += 1
    '            Dim NewShow As New TvShow
    '            NewShow.NfoFilePath = Path.Combine(newTvFolders(0), "tvshow.nfo")
    '            NewShow.TvdbId.Value = searchTVDbID
    '            NewShow.State = Media_Companion.ShowState.Unverified
    '            tvprogresstxt &= "Scraping Show " & i.ToString & " of " & x & " : "
    '            bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)
    '            'Dim tvdb2 As New TVDBScraper2()
    '            If Not haveTVDbID And NewShow.FileContainsReadableXml Then
    '                Dim validcheck As Boolean = nfoFunction.tv_NfoLoadCheck(NewShow.NfoFilePath)
    '                If validcheck Then
    '                    NewShow = nfoFunction.tvshow_NfoLoad(NewShow.NfoFilePath) '.Load()
    '                End If
    '            Else
    '                If haveTVDbID Then
    '                    NewShow.State = Media_Companion.ShowState.Open
    '                Else

    '                    'Resolve show name from folder
    '                    Dim FolderName As String = Utilities.GetLastFolder(newTvFolders(0) & "\")
    '                    If FolderName.ToLower.Contains(excludefromshowfoldername.ToLower) Then
    '                        Dim indx As Integer = FolderName.ToLower.IndexOf(excludefromshowfoldername.ToLower)
    '                        Dim excludearticle As String = FolderName.Substring(indx - 1, excludefromshowfoldername.Length + 1)
    '                        FolderName = FolderName.Replace(excludearticle, "")
    '                    End If
    '                    FolderName = FolderName.Replace(excludefromshowfoldername, "")
    '                    Dim M As Match
    '                    M = Regex.Match(FolderName, "\s*[\(\{\[](?<date>[\d]{4})[\)\}\]]")
    '                    If M.Success = True Then
    '                        FolderName = String.Format("{0} ({1})", FolderName.Substring(0, M.Index), M.Groups("date").Value)
    '                    End If
    '                    NewShow.Title.Value = FolderName

    '                    tvprogresstxt &= "possibly - " & foldername
    '                    bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)

    '                    'tvdb2.LookupLang = searchLanguage
    '                    'tvdb2.Title = FolderName
    '                    'If tvdb2.PossibleShowList IsNot Nothing Then
    '                    '    Dim tempseries As New TheTvDB.TvdbSeries
    '                    '    tempseries = tvdb2.FindBestPossibleShow(tvdb2.PossibleShowList, FolderName, searchLanguage)
    '                    '    If tempseries.Similarity > .9 Then
    '                    '        NewShow.State = Media_Companion.ShowState.Open
    '                    '    End If
    '                    '    searchTVDbID = tempseries.Identity
    '                    '    tempseries = Nothing
    '                    'End If
    '                    If NewShow.PossibleShowList IsNot Nothing Then
    '                        Dim TempSeries As New Tvdb.Series
    '                        TempSeries = Tvdb.FindBestPossibleShow(NewShow.PossibleShowList, NewShow.Title.Value, searchLanguage)
    '                        If TempSeries.Similarity > 0.9 AndAlso TempSeries.Language.Value = searchLanguage Then
    '                            NewShow.State = Media_Companion.ShowState.Open
    '                        End If
    '                        searchTVDbID = TempSeries.Id.Value
    '                    End If
    '                End If

    '                If Not String.IsNullOrEmpty(searchTVDbID) Then
    '                    'Dim Series As New TheTvDB.TvdbSeries
    '                    'tvdb2.TvdbId = searchTVDbID
    '                    'Series = tvdb2.series
    '                    'If tvdb2.SeriesNotFound Then
    '                    '    MsgBox("Please adjust the TV Show title And try again", MsgBoxStyle.OkOnly, "'" & NewShow.Title.Value & "' - No Show Returned")
    '                    '    bckgrnd_tvshowscraper.ReportProgress(1, NewShow)
    '                    '    newTvFolders.RemoveAt(0)
    '                    '    Continue Do
    '                    'End If
    '                    Dim tvdbstuff As New TVDBScraper
    '                    Dim SeriesInfo As Tvdb.ShowData = tvdbstuff.GetShow(searchTVDbID, searchLanguage, Utilities.SeriesXmlPath)
    '                    searchTVDbID = ""
    '                    If SeriesInfo.FailedLoad Then
    '                        MsgBox("Please adjust the TV Show title And try again", _
    '                               MsgBoxStyle.OkOnly, _
    '                               String.Format("'{0}' - No Show Returned", NewShow.Title.Value))
    '                        bckgrnd_tvshowscraper.ReportProgress(1, NewShow)
    '                        newTvFolders.RemoveAt(0)
    '                        Continue Do
    '                    End If
    '                    tvprogresstxt = "Scraping Show " & i.ToString & " of " & x & " : "
    '                    'tvprogresstxt &= "Show Title: " & Series.SeriesName & " "
    '                    tvprogresstxt &= "Show Title: " & SeriesInfo.Series(0).SeriesName.Value & " "
    '                    bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)
    '                    'NewShow.AbsorbTvdbSeries(Series)
    '                    NewShow.AbsorbTvdbSeries(SeriesInfo.Series(0))
    '                    NewShow.Language.Value = searchLanguage

    '                    If Pref.tvdbIMDbRating Then
    '                        Dim ratingdone As Boolean = False
    '                        Dim rating As String = ""
    '                        Dim votes As String = ""
    '                        If ep_getIMDbRating(NewShow.ImdbId.Value, rating, votes) Then
    '                            If rating <> "" Then
    '                                ratingdone = True
    '                                NewShow.Rating.Value = rating
    '                                NewShow.Votes.Value = votes
    '                            End If
    '                        End If
    '                        If Not ratingdone Then
    '                            NewShow.Rating.Value = SeriesInfo.Series(0).Rating.Value
    '                            'NewShow.Rating.Value    = Series.Rating
    '                            NewShow.Votes.Value = SeriesInfo.Series(0).RatingCount.Value
    '                            'NewShow.Votes.Value     = Series.Votes
    '                        End If
    '                    End If

    '                    tvprogresstxt &= " - Getting Actors"
    '                    bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)

    '                    If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Or NewShow.ImdbId.Value = Nothing Then
    '                        NewShow.TvShowActorSource.Value = "tvdb"
    '                        'success = TvGetActorTvdb(tvdb2.Cast, NewShow)
    '                        success = TvGetActorTvdb(NewShow)
    '                    End If

    '                    If (Pref.TvdbActorScrape = 1 Or Pref.TvdbActorScrape = 2) And NewShow.ImdbId.Value <> Nothing Then
    '                        NewShow.TvShowActorSource.Value = "imdb"
    '                        success = TvGetActorImdb(NewShow)
    '                    End If

    '                    If success Then
    '                        tvprogresstxt &= ": -OK!"
    '                    Else
    '                        tvprogresstxt &= ": -error!!"
    '                    End If

    '                    If Pref.TvFanartTvFirst Then
    '                        If Pref.TvDlFanartTvArt OrElse Pref.TvChgShowDlFanartTvArt Then
    '                            tvprogresstxt &= " - Getting FanartTv Artwork"
    '                            bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)
    '                            TvFanartTvArt(NewShow, False)
    '                        End If
    '                        If Pref.tvdlfanart Or Pref.tvdlposter or Pref.tvdlseasonthumbs Then
    '                            tvprogresstxt &= " - Getting TVDB artwork"
    '                            bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)
    '                            success = TvGetArtwork(NewShow, True, True, True, Pref.dlTVxtrafanart, langu:=searchLanguage)
    '                            If success Then
    '                                tvprogresstxt &= ": OK!"
    '                            Else
    '                                tvprogresstxt &= ": error!!"
    '                            End If
    '                        End If
    '                    Else
    '                        If Pref.tvdlfanart Or Pref.tvdlposter or Pref.tvdlseasonthumbs Then
    '                            tvprogresstxt &= " - Getting TVDB artwork"
    '                            bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)
    '                            success = TvGetArtwork(NewShow, True, True, True, Pref.dlTVxtrafanart, langu:=searchLanguage)
    '                            If success Then
    '                                tvprogresstxt &= ": OK!"
    '                            Else
    '                                tvprogresstxt &= ": error!!"
    '                            End If
    '                        End If
    '                        If Pref.TvDlFanartTvArt OrElse Pref.TvChgShowDlFanartTvArt Then
    '                            tvprogresstxt &= " - Getting FanartTv Artwork"
    '                            bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)
    '                            TvFanartTvArt(NewShow, Pref.TvChgShowDlFanartTvArt)
    '                        End If
    '                    End If

    '                    tvprogresstxt &= " - Completed. Saving Show."
    '                    bckgrnd_tvshowscraper.ReportProgress(0, tvprogresstxt)

    '                    If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 2 Then
    '                        NewShow.EpisodeActorSource.Value = "tvdb"
    '                    Else
    '                        NewShow.EpisodeActorSource.Value = "imdb"
    '                    End If

    '                    NewShow.SortOrder.Value = Pref.sortorder

    '                    nfoFunction.tvshow_NfoSave(NewShow, True)
    '                End If
    '            End If

    '            Cache.TvCache.Add(NewShow)
    '            If Not NewShow.FailedLoad Then   'If show fails to scrape, do not add episodes to cache.
    '                Dim episodelist As New List(Of TvEpisode)
    '                episodelist = loadepisodes(NewShow, episodelist)
    '                For Each ep In episodelist
    '                    NewShow.AddEpisode(ep)
    '                Next
    '                TvCheckfolderjpgart(NewShow)
    '            Else                            'If failed, save nfo so users can change Series.
    '                'nfoFunction.tvshow_NfoSave(NewShow, True)
    '            End If
    '            If newTvFolders.Count > 0 AndAlso Not Pref.tvFolders.Contains(newTvFolders(0)) Then Pref.tvFolders.Add(newTvFolders(0))
    '            bckgrnd_tvshowscraper.ReportProgress(1, NewShow)
    '            If newTvFolders.Count > 0 Then newTvFolders.RemoveAt(0)
    '        Loop
    '    Catch ex As Exception
    '        ExceptionHandler.LogError(ex)
    '    End Try
    'End Sub

    'Private Sub bckgrnd_tvshowscraper_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bckgrnd_tvshowscraper.ProgressChanged
    '    Try
    '        If e.ProgressPercentage = 0 Then
    '            ToolStripStatusLabel5.Text = e.UserState
    '            ToolStripStatusLabel5.Visible = True
    '        Else
    '            Dim NewShow As TvShow = e.UserState
    '            ToolStripStatusLabel5.Text = "Scraping TV Shows, " & newTvFolders.Count & " remaining"
    '            ToolStripStatusLabel5.Visible = True

    '            TvTreeview.Nodes.Add(NewShow.ShowNode)
    '            NewShow.UpdateTreenode()

    '            TextBox_TotTVShowCount.Text = Cache.TvCache.Shows.Count
    '            TextBox_TotEpisodeCount.Text = Cache.TvCache.Episodes.Count
    '        End If
    '    Catch ex As Exception
    '        ExceptionHandler.LogError(ex)
    '    End Try
    'End Sub

    'Private Sub bckgrnd_tvshowscraper_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bckgrnd_tvshowscraper.RunWorkerCompleted
    '    Try
    '        ToolStripStatusLabel5.Text = "Saving data"
    '        Tv_CacheSave()
    '        tv_CacheLoad()
    '        tv_Filter()
    '        ToolStripStatusLabel5.Visible = False
    '        Statusstrip_Enable(False)
    '        BlinkTaskBar
    '        GC.Collect()
    '    Catch ex As Exception
    '        ExceptionHandler.LogError(ex)
    '    End Try
    'End Sub

#End Region

#Region "Tv Rescraper Obsolete"
    '    Private Sub tvbckrescrapewizard_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles tvbckrescrapewizard.DoWork
    '        Try
    '            Dim showprocesscount As Integer = 0
    '            Dim progresstext As String = ""
    '            Dim progress As Integer = 0
    '            Dim progcount As Integer = 0
    '            Dim shcachecount As Integer = Cache.TvCache.Shows.Count
    '            Dim done As Integer = 0
    '            If singleshow Then
    '                Dim showslist As TvShow = tv_ShowSelectedCurrently(TvTreeview)
    '                For x = Cache.TvCache.Shows.Count - 1 To 0 Step -1
    '                    If Cache.TvCache.Shows(x).Title.Value = showslist.Title.Value Then
    '                        shcachecount = x + 1
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '            For f = shcachecount - 1 To 0 Step -1
    '                If Cache.TvCache.Shows(f).State = Media_Companion.ShowState.Open Or Cache.TvCache.Shows(f).State = -1 Or tvBatchList.includeLocked = True Then
    '                    If tvBatchList.doEpisodes = True Then
    '                        showprocesscount += (Cache.TvCache.Shows(f).Episodes.Count - Cache.TvCache.Shows(f).MissingEpisodes.Count)
    '                        showprocesscount += 1
    '                        progcount += 1
    '                    Else
    '                        showprocesscount += 1
    '                        progcount += 1
    '                    End If
    '                End If
    '                If singleshow Then Exit For
    '            Next
    '            Dim showsdone As Integer = 0
    '            Dim showcounter As Integer = 0
    '            For f = shcachecount - 1 To 0 Step -1
    '                If tvBatchList.RewriteAllNFOs Then
    '                    If Cache.TvCache.Shows(f).State = 0 Or tvBatchList.includeLocked = True Then
    '                        Dim SelectedSeries As New TvShow
    '                        SelectedSeries = nfoFunction.tvshow_NfoLoad(Cache.TvCache.Shows(f).NfoFilePath)
    '                        Call FixTvActorsNfo(SelectedSeries)
    '                        Call nfoFunction.tvshow_NfoSave(SelectedSeries, True)
    '                        For g = Cache.TvCache.Shows(f).Episodes.Count - 1 To 0 Step -1
    '                            Dim epcount As Integer = Cache.TvCache.Shows(f).Episodes.Count
    '                            progresstext = "Rewriting nfo's of Show: " & Cache.TvCache.Shows(f).Title.Value & ", Episode: " & epcount - g & " of " & epcount & ", Episode: " & Cache.TvCache.Shows(f).Episodes(g).Season.Value & "x" & Cache.TvCache.Shows(f).Episodes(g).Episode.Value & " - " & Cache.TvCache.Shows(f).Episodes(g).Title.Value
    '                            If done > 0 Then
    '                                progress = (100 / showprocesscount) * done
    '                            Else
    '                                progress = 0
    '                            End If
    '                            If Cache.TvCache.Shows(f).Episodes(g).IsMissing Then
    '                                progresstext &= "Skip Missing episode"
    '                                Continue For
    '                            End If
    '                            tvbckrescrapewizard.ReportProgress(progress, progresstext)
    '                            Dim listofepisodes As New List(Of TvEpisode)
    '                            listofepisodes.Clear()
    '                            listofepisodes = WorkingWithNfoFiles.ep_NfoLoad(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)
    '                            WorkingWithNfoFiles.ep_NfoSave(listofepisodes, listofepisodes(0).NfoFilePath)
    '                        Next
    '                    End If
    '                    If singleshow Then Exit For
    '                    Continue For
    '                End If

    '                If Cache.TvCache.Shows(f).State = Media_Companion.ShowState.Open OrElse Cache.TvCache.Shows(f).State = -1 OrElse tvBatchList.includeLocked = True Then
    '                    showcounter += 1
    '                    progresstext = "Working on Show: " & showcounter.ToString & " of " & progcount
    '                    If done > 0 Then
    '                        progress = (100 / showprocesscount) * done
    '                    Else
    '                        progress = 0
    '                    End If
    '                    tvbckrescrapewizard.ReportProgress(progress, progresstext)
    '                    Dim editshow As New TvShow
    '                    editshow = nfoFunction.tv_NfoLoadFull(Cache.TvCache.Shows(f).NfoFilePath)
    '                    If tvBatchList.doShowActors Then editshow.ListActors.Clear()

    '                    Dim tvdbstuff As New TVDBScraper
    '                    Dim tvseriesdata As New Tvdb.ShowData
    '                    Dim language As String = editshow.Language.Value
    '                    If language = "" Then language = "en"
    '                    If tvBatchList.shSeries Then
    '                        Dim aok As Boolean = tvdbstuff.GetSeriesXml(editshow.TvdbId.Value, language, Utilities.SeriesXmlPath)
    '                        If aok Then
    '                            progresstext &= "Updated Series.xml data"
    '                        Else
    '                            progresstext &= "Failed to update xml data"
    '                        End If
    '                        tvbckrescrapewizard.ReportProgress(progress, progresstext)
    '                        If singleshow Then Exit For
    '                        Continue For
    '                    End If
    '                    tvseriesdata = tvdbstuff.GetShow(editshow.TvdbId.Value, language, Utilities.SeriesXmlPath)
    '                    If tvseriesdata.FailedLoad Then
    '                        progresstext &= "Failed to load xml data"
    '                        tvbckrescrapewizard.ReportProgress(progress, progresstext)
    '                        Continue For
    '                    End If
    '                    If tvBatchList.doShows = True Then
    '                        If tvBatchList.doShowBody = True Or tvBatchList.doShowActors = True Then
    '                            Try
    '                                editshow.ImdbId.Value = tvseriesdata.Series(0).ImdbId.Value
    '                                If tvBatchList.shMpaa Then editshow.Mpaa.Value = tvseriesdata.Series(0).ContentRating.Value
    '                                If tvBatchList.shYear Then
    '                                    editshow.Premiered.Value = tvseriesdata.Series(0).FirstAired.Value
    '                                    If Not String.IsNullOrEmpty(editshow.Premiered.Value) Then editshow.Year.Value = editshow.Premiered.Value.Substring(0, 4)
    '                                End If
    '                                If tvBatchList.shGenre Then
    '                                    Dim newstring As String
    '                                    newstring = tvseriesdata.Series(0).Genre.Value
    '                                    newstring = newstring.TrimEnd("|")
    '                                    newstring = newstring.TrimStart("|")
    '                                    Dim strsplt() As String = newstring.Split("|")
    '                                    Dim counter As Integer = 0
    '                                    newstring = ""
    '                                    For i = 0 To strsplt.Count - 1
    '                                        If i = Pref.TvMaxGenres Then Exit For
    '                                        If newstring = "" Then
    '                                            newstring = strsplt(i)
    '                                        Else
    '                                            newstring &= " / " & strsplt(i)
    '                                        End If
    '                                    Next
    '                                    editshow.Genre.Value = newstring
    '                                End If
    '                                If tvBatchList.shStudio Then editshow.Studio.Value = tvseriesdata.Series(0).Network.Value
    '                                If tvBatchList.shPlot Then editshow.Plot.Value = tvseriesdata.Series(0).Overview.Value
    '                                If tvBatchList.shRuntime Then editshow.Runtime.Value = tvseriesdata.Series(0).RunTime.Value
    '                                If tvBatchList.shStatus Then editshow.Status.Value = tvseriesdata.Series(0).Status.Value
    '                                Dim episodeguideurl As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & editshow.TvdbId.Value & "/all/" & language & ".zip"
    '                                editshow.EpisodeGuideUrl.Value = ""
    '                                editshow.Url.Value = episodeguideurl
    '                                editshow.Url.Node.SetAttributeValue("cache", editshow.TvdbId.Value)
    '                                editshow.Url.AttachToParentNode(editshow.EpisodeGuideUrl.Node)
    '                                If tvBatchList.shRating Then
    '                                    Dim ratingdone As Boolean = False
    '                                    If Pref.tvdbIMDbRating Then
    '                                        Dim rating As String = ""
    '                                        Dim votes As String = ""
    '                                        If ep_getIMDbRating(editshow.ImdbId.Value, rating, votes) Then
    '                                            If rating <> "" Then
    '                                                ratingdone = True
    '                                                editshow.Rating.Value = rating
    '                                                editshow.Votes.Value = votes
    '                                            End If
    '                                        End If
    '                                    End If
    '                                    If Not ratingdone Then
    '                                        editshow.Rating.Value = tvseriesdata.Series(0).Rating.Value
    '                                        editshow.Votes.Value = tvseriesdata.Series(0).RatingCount.Value
    '                                    End If
    '                                End If
    '                                If tvBatchList.doShowActors = True Then
    '                                    If editshow.TvShowActorSource.Value = Nothing Then
    '                                        If Pref.TvdbActorScrape = 0 Or Pref.TvdbActorScrape = 3 Then
    '                                            editshow.TvShowActorSource.Value = "tvdb"
    '                                        Else
    '                                            editshow.TvShowActorSource.Value = "imdb"
    '                                        End If
    '                                    End If
    '                                    If editshow.TvShowActorSource.Value = "tvdb" Then TvGetActorTvdb(editshow)
    '                                    If editshow.TvShowActorSource.Value = "imdb" Then TvGetActorImdb(editshow)
    '                                End If

    '                            Catch ex As Exception
    '                            End Try
    '                            Call nfoFunction.tvshow_NfoSave(editshow, True)

    '                        End If

    '                        'Posters, Fanart and Season art
    '                        If tvBatchList.doShowArt = True Then
    '                            If tvBatchList.shDelArtwork Then TvDeleteShowArt(Cache.TvCache.Shows(f), False)
    '                            If tvBatchList.shFanart orElse tvBatchList.shPosters OrElse tvBatchList.shSeason OrElse tvBatchList.shXtraFanart Then
    '                                TvGetArtwork(Cache.TvCache.Shows(f), tvBatchList.shFanart, tvBatchList.shPosters, tvBatchList.shSeason, tvBatchList.shXtraFanart, force:=False)
    '                            End If
    '                            If tvBatchList.shFanartTvArt Then TvFanartTvArt(Cache.TvCache.Shows(f), False) 'We're only looking for missing art from Fanart.Tv

    '                            'If selected, copy Series banner to season banner if no season banner downloaded.
    '                            If tvBatchList.shBannerMain AndAlso Pref.FrodoEnabled Then
    '                                Dim mainbanner As String = Cache.TvCache.Shows(f).ImageBanner.Path
    '                                If File.Exists(mainbanner) Then
    '                                    For each seas As TvSeason In Cache.TvCache.Shows(f).Seasons.values
    '                                        Dim SeasonNo As String = seas.SeasonNode.Text.Replace("Season ", "")
    '                                        If SeasonNo.ToLower = "specials" Then SeasonNo = "-" & SeasonNo.ToLower
    '                                        Dim SeasonBanner As String = Cache.TvCache.Shows(f).FolderPath & "season" & SeasonNo & "-banner.jpg"
    '                                        If Not File.Exists(SeasonBanner) Then
    '                                            Utilities.SafeCopyFile(mainbanner, SeasonBanner)
    '                                        End If
    '                                    Next
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                    If tvBatchList.doEpisodes = True Then
    '                        Dim i As Integer = 0
    '                        Dim TotalEpisodes As Integer = Cache.TvCache.Shows(f).Episodes.Count - Cache.TvCache.Shows(f).MissingEpisodes.count
    '                        For g = Cache.TvCache.Shows(f).Episodes.Count - 1 To 0 Step -1
    '                            If Cache.TvCache.Shows(f).Episodes(g).IsMissing Then Continue For
    '                            i += 1
    '                            progresstext = "Working on Show: " & Cache.TvCache.Shows(f).Title.Value & " Episode: " & i & " of " & TotalEpisodes & ", Episode: " & Cache.TvCache.Shows(f).Episodes(g).Season.Value & "x" & Cache.TvCache.Shows(f).Episodes(g).Episode.Value & " - " & Cache.TvCache.Shows(f).Episodes(g).Title.Value

    '                            If done > 0 Then
    '                                progress = (100 / showprocesscount) * done
    '                            Else
    '                                progress = 0
    '                            End If
    '                            tvbckrescrapewizard.ReportProgress(progress, progresstext)
    '                            Dim actorsource As String = Cache.TvCache.Shows(f).EpisodeActorSource.Value
    '                            If actorsource = "" Then actorsource = "tvdb"
    '                            Dim Episodedata As New Tvdb.Episode
    '                            Dim epfound As Boolean = False
    '                            For Each NewEpisode As Tvdb.Episode In tvseriesdata.Episodes
    '                                If NewEpisode.SeasonNumber.Value = Cache.TvCache.Shows(f).Episodes(g).Season.Value
    '                                    If NewEpisode.EpisodeNumber.Value = Cache.TvCache.Shows(f).Episodes(g).Episode.Value
    '                                        Episodedata = NewEpisode
    '                                        Episodedata.ThumbNail.Value = "http://www.thetvdb.com/banners/" & NewEpisode.ThumbNail.value
    '                                        epfound = True
    '                                        Exit For
    '                                    End If
    '                                End If
    '                            Next
    '                            If Not epfound Then
    '                                Dim sortorder As String = Cache.TvCache.Shows(f).SortOrder.Value
    '                                If sortorder = "" Then sortorder = "default"
    '                                Dim tvdbid As String = Cache.TvCache.Shows(f).TvdbId.Value
    '                                Dim imdbid As String = Cache.TvCache.Shows(f).ImdbId.Value
    '                                Dim seasonno As String = Cache.TvCache.Shows(f).Episodes(g).Season.Value
    '                                Dim episodeno As String = Cache.TvCache.Shows(f).Episodes(g).Episode.Value
    '                                Episodedata = tvdbstuff.getepisodefromxml(tvdbid, sortorder, seasonno, episodeno, language, True)
    '                                If Episodedata.FailedLoad Then
    '                                    progresstext = "tvdb was unable to process the following show episode." & vbCrLf & Cache.TvCache.Shows(f).Title.Value & " - S" & Utilities.PadNumber(Cache.TvCache.Shows(f).Episodes(g).Season.Value, 2) & "E" & Utilities.PadNumber(Cache.TvCache.Shows(f).Episodes(g).Episode.Value, 2) & " " & Cache.TvCache.Shows(f).Episodes(g).Title.Value
    '                                    tvbckrescrapewizard.ReportProgress(progress, progresstext)
    '                                    Continue For
    '                                End If
    '                            End If
    '                            If tvBatchList.doEpisodeBody Or (tvBatchList.doEpisodeActors And Cache.TvCache.Shows(f).EpisodeActorSource.Value <> "") Or (tvBatchList.doEpisodeArt) Then
    '                                Dim listofnewepisodes As New List(Of TvEpisode)
    '                                listofnewepisodes.Clear()
    '                                listofnewepisodes = WorkingWithNfoFiles.ep_NfoLoad(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)   'Generic(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)
    '                                For h = listofnewepisodes.Count - 1 To 0 Step -1
    '                                    If listofnewepisodes(h).Season.Value = Cache.TvCache.Shows(f).Episodes(g).Season.Value And listofnewepisodes(h).Episode.Value = Cache.TvCache.Shows(f).Episodes(g).Episode.Value Then
    '                                        Dim newactors As New List(Of str_MovieActors)
    '                                        newactors.Clear()

    '                                        'its an episode
    '                                        Dim episodescreenurl As String = ""
    '                                        Try
    '                                            Dim epimdbid As String = listofnewepisodes(h).ImdbId.Value
    '                                            If tvBatchList.epIMDBId OrElse (Not epimdbid.StartsWith("tt") AndAlso epimdbid.Length < 9) Then
    '                                                listofnewepisodes(h).ImdbId.Value = Episodedata.ImdbId.Value
    '                                            End If
    '                                            listofnewepisodes(h).UniqueId.Value = Episodedata.Id.Value
    '                                            listofnewepisodes(h).ShowId.Value = Episodedata.SeriesId.Value
    '                                            listofnewepisodes(h).Showimdbid.Value = Cache.TvCache.Shows(f).ImdbId.Value
    '                                            If tvBatchList.epAired Then listofnewepisodes(h).Aired.Value = Episodedata.FirstAired.Value
    '                                            If tvBatchList.epPlot Then listofnewepisodes(h).Plot.Value = Episodedata.Overview.Value
    '                                            If tvBatchList.epDirector Then listofnewepisodes(h).Director.Value = Utilities.Cleanbraced(Episodedata.Director.Value)
    '                                            If tvBatchList.epCredits Then listofnewepisodes(h).Credits.Value = Utilities.Cleanbraced(Episodedata.Writer.Value)
    '                                            If tvBatchList.epRating Then GetEpRating(listofnewepisodes(h), Episodedata.Rating.Value, Episodedata.Votes.Value)
    '                                            If tvBatchList.epTitle Then listofnewepisodes(h).Title.Value = Episodedata.EpisodeName.Value

    '                                            If tvBatchList.epActor Then
    '                                                If actorsource = "tvdb" Then
    '                                                    listofnewepisodes(h).ListActors.Clear()
    '                                                    Dim tempstr As String = Episodedata.GuestStars.Value
    '                                                    tempstr = tempstr.TrimStart("|")
    '                                                    tempstr = tempstr.TrimEnd("|")
    '                                                    Dim Tmp() As String
    '                                                    Tmp = tempstr.Split("|")
    '                                                    For Each act In Tmp
    '                                                        Dim newactor As New str_MovieActors
    '                                                        newactor.actorname = act
    '                                                        listofnewepisodes(h).ListActors.Add(newactor)
    '                                                    Next
    '                                                ElseIf actorsource = "imdb" Then
    '                                                    Dim epid As String = ""
    '                                                    If listofnewepisodes(h).ImdbId.Value <> "" Then
    '                                                        epid = listofnewepisodes(h).ImdbId.Value
    '                                                    Else
    '                                                        epid = GetEpImdbId(listofnewepisodes(h).Showimdbid.Value, listofnewepisodes(h).Season.Value, listofnewepisodes(h).Episode.Value)
    '                                                    End If
    '                                                    'Dim epid As String = GetEpImdbId(Cache.TvCache.Shows(f).ImdbId.Value, listofnewepisodes(h).Season.Value, listofnewepisodes(h).Episode.Value)
    '                                                    If epid.Contains("tt") Then
    '                                                        EpGetActorImdb(listofnewepisodes(h))
    '                                                    End If
    '                                                End If
    '                                                If Pref.copytvactorthumbs AndAlso Not IsNothing(Cache.TvCache.Shows(f)) Then
    '                                                    If listofnewepisodes(h).ListActors.Count = 0 Then
    '                                                        For each act In Cache.TvCache.Shows(f).ListActors
    '                                                            listofnewepisodes(h).ListActors.Add(act)
    '                                                        Next
    '                                                    Else
    '                                                        Dim t As Integer = 0
    '                                                        For each act In Cache.TvCache.Shows(f).ListActors
    '                                                            Dim q = From x In listofnewepisodes(h).ListActors Where x.actorname = act.actorname
    '                                                            If q.Count = 1 Then listofnewepisodes(h).ListActors.Remove(q(0))
    '                                                            t += 1
    '                                                            listofnewepisodes(h).ListActors.Add(act)
    '                                                            If t = Pref.maxactors Then Exit For
    '                                                        Next
    '                                                    End If
    '                                                End If
    '                                            End If
    '                                            If tvBatchList.doEpisodeArt AndAlso (tvBatchList.epdlThumbnail OrElse tvBatchList.epCreateScreenshot) Then
    '                                                listofnewepisodes(h).Thumbnail.FileName = Episodedata.ThumbNail.Value
    '                                                progresstext = tv_EpisodeFanartGet(listofnewepisodes(h), tvBatchList.epdlThumbnail, tvBatchList.epCreateScreenshot).Replace("!!! ", "")
    '                                            End If
    '                                        Catch ex As Exception
    '#If SilentErrorScream Then
    '                                            Throw ex
    '#End If
    '                                            'MsgBox("hekp")
    '                                        End Try
    '                                        WorkingWithNfoFiles.ep_NfoSave(listofnewepisodes, listofnewepisodes(0).NfoFilePath)
    '                                        listofnewepisodes(h).UpdateTreenode()
    '                                        Exit For
    '                                    End If
    '                                Next
    '                            End If

    '                            If tvBatchList.doEpisodeMediaTags = True Then
    '                                Dim listofnewepisodes As New List(Of TvEpisode)
    '                                listofnewepisodes.Clear()
    '                                listofnewepisodes = WorkingWithNfoFiles.ep_NfoLoad(Cache.TvCache.Shows(f).Episodes(g).NfoFilePath)
    '                                For h = listofnewepisodes.Count - 1 To 0 Step -1
    '                                    listofnewepisodes(h).GetFileDetails()
    '                                    If Not String.IsNullOrEmpty(listofnewepisodes(h).StreamDetails.Video.DurationInSeconds.Value) Then
    '                                        Try
    '                                            Dim tempstring As String
    '                                            tempstring = listofnewepisodes(h).StreamDetails.Video.DurationInSeconds.Value
    '                                            If Pref.intruntime Then
    '                                                listofnewepisodes(h).Runtime.Value = Math.Round(tempstring / 60).ToString
    '                                            Else
    '                                                listofnewepisodes(h).Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
    '                                            End If

    '                                        Catch
    '                                        End Try
    '                                        WorkingWithNfoFiles.ep_NfoSave(listofnewepisodes, listofnewepisodes(0).NfoFilePath)
    '                                    End If
    '                                Next
    '                            End If
    '                            done += 1
    '                        Next
    '                    End If
    '                    done += 1
    '                    If singleshow Then Exit For
    '                End If

    '            Next
    '            singleshow = False
    '        Catch ex As Exception
    '            ExceptionHandler.LogError(ex)
    '        End Try
    '    End Sub

    'Private Sub tvbckrescrapewizard_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles tvbckrescrapewizard.ProgressChanged
    '    Try
    '        If e.ProgressPercentage = 999999 Then
    '            ToolStripStatusLabel8.Text = e.UserState
    '        Else
    '            ToolStripStatusLabel8.Text = e.UserState
    '            ToolStripProgressBar7.Value = e.ProgressPercentage
    '            ToolStripProgressBar7.ProgressBar.Refresh()
    '            ToolStripProgressBar7.ProgressBar.PerformStep()
    '        End If
    '    Catch ex As Exception
    '        ExceptionHandler.LogError(ex)
    '    End Try
    'End Sub

    'Private Sub tvbckrescrapewizard_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles tvbckrescrapewizard.RunWorkerCompleted
    '    Try
    '        ToolStripStatusLabel8.Visible = False
    '        ToolStripProgressBar7.Visible = False
    '        Statusstrip_Enable(False)
    '        TvTreeview_AfterSelect_Do()
    '        GC.Collect()
    '    Catch ex As Exception
    '        ExceptionHandler.LogError(ex)
    '    End Try
    'End Sub

#End Region

    'Private Sub TV_EpisodeScraper(ByVal ListOfShows As List(Of TvShow), ByVal manual As Boolean)
    '    Dim stage As String = "0"
    '    Try
    '        Dim tempstring As String = ""
    '        Dim tempint As Integer
    '        Dim errorcounter As Integer = 0
    '        newEpisodeList.Clear()
    '        Dim newtvfolders As New List(Of String)
    '        Dim progress As Integer
    '        progress = 0
    '        Dim progresstext As String = String.Empty
    '        Pref.tvScraperLog = ""
    '        Dim ShowsScanned As Integer = 0
    '        Dim FoldersScanned As Integer = 0
    '        Dim ShowsLocked As Integer = 0
    '        'Dim dirpath As String = String.Empty
    '        Dim moviepattern As String = String.Empty
    '        If bckgroundscanepisodes.CancellationPending Then
    '            Pref.tvScraperLog &= vbCrLf & "!!! Operation cancelled by user"
    '            Exit Sub
    '        End If
    '        If Pref.tvshow_useXBMC_Scraper = True Then
    '            Pref.tvScraperLog &= "---Using XBMC TVDB Scraper---" & vbCrLf
    '        Else
    '            Pref.tvScraperLog &= "---Using MC TVDB Scraper---" & vbCrLf
    '        End If
    '        progresstext = String.Concat("Scanning TV Folders For New Episodes...")
    '        bckgroundscanepisodes.ReportProgress(progress, progresstext)

    '        Pref.tvScraperLog &= "Starting Folder Scan" & vbCrLf & vbCrLf

    '        Dim TvFolder As String
    '        For Each TvShow As Media_Companion.TvShow In ListOfShows
    '            TvFolder = Path.GetDirectoryName(TvShow.FolderPath)
    '            Dim Add As Boolean = True
    '            If TvShow.State <> Media_Companion.ShowState.Open AndAlso manual = False Then Add = False

    '            If Add = True Then
    '                ShowsScanned += 1
    '                progresstext = String.Concat("Stage 1 of 3 : Found " & newtvfolders.Count & " : Creating List of Folders From Roots : Searching - '" & TvFolder & "'")
    '                bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '                If bckgroundscanepisodes.CancellationPending Then
    '                    Pref.tvScraperLog &= vbCrLf & "!!! Operation cancelled by user"
    '                    Exit Sub
    '                End If
    '                tempstring = ""
    '                Dim hg As New DirectoryInfo(TvFolder)
    '                If hg.Exists Then
    '                    scraperLog = scraperLog & "Found " & hg.FullName.ToString & vbCrLf
    '                    newtvfolders.Add(TvFolder)
    '                    scraperLog = scraperLog & "Checking for subfolders" & vbCrLf
    '                    Dim ExtraFolder As List(Of String) = Utilities.EnumerateFolders(TvFolder, 3)
    '                    For Each Item As String In ExtraFolder
    '                        If Pref.ExcludeFolders.Match(Item) Then Continue For
    '                        newtvfolders.Add(Item)
    '                        FoldersScanned += 1
    '                    Next
    '                End If
    '            Else
    '                ShowsLocked += 1
    '            End If
    '        Next

    '        scraperLog = scraperLog & vbCrLf
    '        Dim mediacounter As Integer = newEpisodeList.Count
    '        newtvfolders.Sort()
    '        For g = 0 To newtvfolders.Count - 1
    '            If bckgroundscanepisodes.CancellationPending Then
    '                Pref.tvScraperLog &= vbCrLf & "!!! Operation cancelled by user"
    '                Exit Sub
    '            End If
    '            progresstext = String.Concat("Stage 2 of 3 : Found " & newEpisodeList.Count & " : Searching for New Episodes in Folders " & g + 1 & " of " & newtvfolders.Count & " - '" & newtvfolders(g) & "'")
    '            bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '            For Each f In Utilities.VideoExtensions
    '                Dim dirpath As String = newtvfolders(g)
    '                Dim dir_info As New DirectoryInfo(dirpath)
    '                tv_NewFind(dirpath, f)
    '            Next f
    '            tempint = newEpisodeList.Count - mediacounter
    '            mediacounter = newEpisodeList.Count
    '        Next g

    '        'report so far
    '        Pref.tvScraperLog &= "!!! Scanned """ & ShowsScanned.ToString & """ Shows." & vbCrLf
    '        If ShowsLocked > 0 Then Pref.tvScraperLog &= "!!! Skipped """ & ShowsLocked.ToString & " Locked Shows." & vbCrLf
    '        Pref.tvScraperLog &= "!!! Scanned """ & (ShowsScanned + FoldersScanned).ToString & """ folders (includes Show and subfolders)." & vbCrLf & vbCrLf

    '        If newEpisodeList.Count <= 0 Then
    '            Pref.tvScraperLog &= "!!! No new episodes found, exiting scraper." & vbCrLf
    '            Exit Sub
    '        Else
    '            Pref.tvScraperLog &= "!!! """ & newEpisodeList.Count.ToString & """ Episodes found." & vbCrLf & vbCrLf
    '        End If

    '        Dim S As String = ""
    '        For Each newepisode In newEpisodeList
    '            S = ""
    '            If bckgroundscanepisodes.CancellationPending Then
    '                Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                Exit Sub
    '            End If
    '            For Each Shows In Cache.TvCache.Shows
    '                If bckgroundscanepisodes.CancellationPending Then
    '                    Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                    Exit Sub
    '                End If
    '                If newepisode.FolderPath.Contains(Shows.FolderPath) Then
    '                    If Shows.ImdbId.Value Is Nothing OrElse String.IsNullOrEmpty(Shows.Premiered.Value) Then
    '                        Shows = nfoFunction.tvshow_NfoLoad(Shows.NfoFilePath) '.Load()
    '                    End If
    '                    newepisode.ShowLang.Value = Shows.Language.Value
    '                    newepisode.sortorder.Value = Shows.SortOrder.Value
    '                    newepisode.Showtvdbid.Value = Shows.TvdbId.Value
    '                    newepisode.Showimdbid.Value = Shows.ImdbId.Value
    '                    newepisode.ShowTitle.Value = Shows.Title.Value
    '                    newepisode.ShowYear.Value = Shows.Year.Value
    '                    newepisode.ShowObj = Shows
    '                    If String.IsNullOrEmpty(newepisode.ShowYear.Value) Then
    '                        If Not String.IsNullOrEmpty(Shows.Premiered.Value) Then
    '                            Dim yr As String = Shows.Premiered.Value.Substring(0, 4)
    '                            If yr.Length = 4 Then newepisode.ShowYear.Value = yr
    '                        End If
    '                    End If
    '                    newepisode.actorsource.Value = Shows.EpisodeActorSource.Value

    '                    ''' Fix for Episode getting Show's IMDb Id number, not the Episode IMDb Id number.
    '                    newepisode.ImdbId.Value = ""

    '                    Exit For
    '                End If
    '            Next
    '            Dim episode As New TvEpisode
    '            Dim airedgot As Boolean = False
    '            For Each Regexs In tv_RegexScraper
    '                S = newepisode.VideoFilePath '.ToLower
    '                stage = "1"
    '                Dim i As Integer                  'sacrificial variable to appease the TryParseosaurus Checks
    '                If Not String.IsNullOrEmpty(newepisode.ShowTitle.Value) AndAlso Integer.TryParse(newepisode.ShowTitle.Value, i) <> -1 Then S = S.Replace(newepisode.ShowTitle.Value, "")
    '                stage = "2"
    '                If Not String.IsNullOrEmpty(newepisode.ShowYear.Value) AndAlso (newepisode.ShowYear.Value.ToInt <> 0) Then
    '                    If S.Contains(newepisode.ShowYear.Value) AndAlso Not S.ToLower.Contains("s" & newepisode.ShowYear.Value) Then
    '                        S = S.Replace(newepisode.ShowYear.Value, "")
    '                    End If
    '                End If

    '                stage = "3"
    '                S = S.Replace("x265", "")
    '                S = S.Replace("x264", "")
    '                S = S.Replace("720p", "")
    '                S = S.Replace("720i", "")
    '                S = S.Replace("1080p", "")
    '                S = S.Replace("1080i", "")
    '                S = S.Replace("X265", "")
    '                S = S.Replace("X264", "")
    '                S = S.Replace("720P", "")
    '                S = S.Replace("720I", "")
    '                S = S.Replace("1080P", "")
    '                S = S.Replace("1080I", "")
    '                stage = "4"
    '                Dim N As Match
    '                stage = "5"     'Do date test first.
    '                N = Regex.Match(S, tv_EpRegexDate)
    '                If N.Success Then
    '                    If Not airedgot Then
    '                        Dim aired As String = N.Groups(0).Value.Replace(".", "-").Replace("_", "-")
    '                        newepisode.Aired.Value = aired
    '                        airedgot = True
    '                    End If
    '                    If airedgot Then S = S.Replace(N.Groups(0).Value, "")
    '                    'Exit For
    '                End If
    '                stage = "6"
    '                If Not N.Success OrElse airedgot Then
    '                    Dim M As Match
    '                    M = Regex.Match(S, Regexs)
    '                    If M.Success = True Then
    '                        Try
    '                            stage = "7"
    '                            newepisode.Season.Value = M.Groups(1).Value.ToString
    '                            newepisode.Episode.Value = M.Groups(2).Value.ToString

    '                            Try
    '                                Dim matchvalue As String = M.Value
    '                                newepisode.Thumbnail.FileName = S.Substring(S.LastIndexOf(matchvalue) + matchvalue.Length, S.Length - (S.LastIndexOf(matchvalue) + (matchvalue.Length)))
    '                            Catch ex As Exception
    '                            End Try
    '                            Exit For
    '                        Catch
    '                            newepisode.Season.Value = "-1"
    '                            newepisode.Episode.Value = "-1"
    '                        End Try
    '                    End If
    '                End If
    '            Next
    '            stage = "8"
    '            If newepisode.Season.Value = Nothing Then newepisode.Season.Value = "-1"
    '            If newepisode.Episode.Value = Nothing Then newepisode.Episode.Value = "-1"
    '            If newepisode.Season.Value <> "-1" AndAlso newepisode.Episode.Value <> "-1" Then newepisode.Aired.Value = "" 'Clear Aired value if got valid Ep and Season values.
    '        Next
    '        stage = "9"
    '        Dim savepath As String = ""
    '        Dim scrapedok As Boolean
    '        Dim epscount As Integer = 0
    '        For Each eps In newEpisodeList
    '            stage = "10"
    '            Dim showtitle As String = eps.ShowTitle.Value
    '            epscount += 1
    '            Pref.tvScraperLog &= "!!! With File : " & eps.VideoFilePath & vbCrLf
    '            If eps.Aired.Value <> Nothing Then
    '                Pref.tvScraperLog &= "!!! Detected  : Aired Date: " & eps.Aired.Value & vbCrLf
    '            Else
    '                Pref.tvScraperLog &= "!!! Detected  : Season : " & eps.Season.Value & " Episode : " & eps.Episode.Value & vbCrLf
    '            End If
    '            If eps.Season.Value = "-1" And eps.Episode.Value = "-1" AndAlso eps.Aired.Value = Nothing Then
    '                Pref.tvScraperLog &= "!!! WARNING: Can't extract Season and Episode details from this filename, file not added!" & vbCrLf
    '                Pref.tvScraperLog &= "!!!" & vbCrLf
    '                Continue For    'if we can't get season or episode then skip to next episode
    '            End If

    '            Dim episodearray As New List(Of TvEpisode)
    '            episodearray.Clear()
    '            stage = "11"
    '            episodearray.Add(eps)
    '            If bckgroundscanepisodes.CancellationPending Then
    '                Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                Exit Sub
    '            End If
    '            stage = "12"
    '            Dim WhichScraper As String = ""
    '            If Pref.tvshow_useXBMC_Scraper = True Then
    '                WhichScraper = "XBMC TVDB"
    '            Else
    '                WhichScraper = "MC TVDB"
    '            End If
    '            progresstext = String.Concat("ESC to Cancel : Stage 3 of 3 : Scraping New Episodes : Using " & WhichScraper & "Scraper : Scraping " & epscount & " of " & newEpisodeList.Count & " - '" & Path.GetFileName(eps.VideoFilePath) & "'")
    '            bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '            Dim removal As String = ""
    '            If (eps.Season.Value = "-1" Or eps.Episode.Value = "-1") AndAlso eps.Aired.Value = Nothing Then
    '                stage = "13"
    '                eps.Title.Value = Utilities.GetFileName(eps.VideoFilePath)
    '                eps.Rating.Value = "0"
    '                eps.Votes.Value = "0"
    '                eps.PlayCount.Value = "0"
    '                eps.Genre.Value = "Unknown Episode Season and/or Episode Number"
    '                eps.GetFileDetails()
    '                episodearray.Add(eps)
    '                savepath = episodearray(0).NfoFilePath
    '            Else
    '                stage = "14"
    '                Dim temppath As String = eps.NfoFilePath
    '                'check for multiepisode files
    '                Dim M2 As Match
    '                Dim epcount As Integer = 0
    '                Dim allepisodes(100) As Integer
    '                stage = "15"
    '                If Not String.IsNullOrEmpty(eps.Thumbnail.FileName) Then
    '                    S = Regex.Replace(eps.Thumbnail.FileName, "\(.*?\)", "")   'Remove anything from filename in brackets like resolution ie: (1920x1080) that may give false episode number
    '                    S = Regex.Replace(S, "\[.*?\]", "")
    '                End If
    '                stage = "16"
    '                eps.Thumbnail.FileName = ""
    '                Do
    '                    If eps.Aired.Value <> Nothing Then Exit Do
    '                    '<tvregex>[Ss]([\d]{1,2}).?[Ee]([\d]{3})</tvregex>
    '                    M2 = Regex.Match(S, "(([EeXx])([\d]{1,4}))")
    '                    If M2.Success = True Then
    '                        Dim skip As Boolean = False
    '                        For Each epso In episodearray
    '                            If epso.Episode.Value = M2.Groups(3).Value Then skip = True
    '                        Next
    '                        If skip = False Then
    '                            Dim multieps As New TvEpisode
    '                            multieps.Season.Value = eps.Season.Value
    '                            multieps.Episode.Value = M2.Groups(3).Value
    '                            multieps.VideoFilePath = eps.VideoFilePath
    '                            multieps.MediaExtension = eps.MediaExtension
    '                            multieps.ShowObj = eps.ShowObj
    '                            episodearray.Add(multieps)
    '                            allepisodes(epcount) = Convert.ToDecimal(M2.Groups(3).Value)
    '                        End If
    '                        Try
    '                            S = S.Substring(M2.Groups(3).Index + M2.Groups(3).Value.Length, S.Length - (M2.Groups(3).Index + M2.Groups(3).Value.Length))
    '                        Catch ex As Exception
    '                        End Try
    '                    End If
    '                    If bckgroundscanepisodes.CancellationPending Then
    '                        Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                        Exit Sub
    '                    End If
    '                Loop Until M2.Success = False
    '                stage = "17"
    '                Dim language As String = eps.ShowLang.Value
    '                Dim sortorder As String = eps.sortorder.Value
    '                Dim tvdbid As String = eps.Showtvdbid.Value
    '                Dim imdbid As String = eps.Showimdbid.Value
    '                Dim actorsource As String = eps.actorsource.Value
    '                stage = "18"
    '                savepath = episodearray(0).NfoFilePath
    '                stage = "19"
    '                If episodearray.Count > 1 Then
    '                    For I = 1 To episodearray.Count - 1
    '                        episodearray(I).MakeSecondaryTo(episodearray(0))
    '                    Next
    '                    Pref.tvScraperLog &= "Multipart episode found: " & vbCrLf
    '                    Pref.tvScraperLog &= "Season: " & episodearray(0).Season.Value & " Episodes, "
    '                    For Each ep In episodearray
    '                        Pref.tvScraperLog &= ep.Episode.Value & ", "
    '                        ep.Showimdbid.Value = imdbid
    '                    Next
    '                    Pref.tvScraperLog &= vbCrLf
    '                End If
    '                stage = "20"
    '                Dim Firstep As Boolean = True
    '                For Each singleepisode In episodearray
    '                    If bckgroundscanepisodes.CancellationPending Then
    '                        Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                        Exit Sub
    '                    End If
    '                    If singleepisode.Season.Value.Length > 0 Or singleepisode.Season.Value.IndexOf("0") = 0 Then
    '                        Do Until singleepisode.Season.Value.IndexOf("0") <> 0 Or singleepisode.Season.Value.Length = 1
    '                            singleepisode.Season.Value = singleepisode.Season.Value.Substring(1, singleepisode.Season.Value.Length - 1)
    '                        Loop
    '                        If singleepisode.Episode.Value = "00" Then
    '                            singleepisode.Episode.Value = "0"
    '                        End If
    '                        If singleepisode.Episode.Value <> "0" Then
    '                            Do Until singleepisode.Episode.Value.IndexOf("0") <> 0
    '                                singleepisode.Episode.Value = singleepisode.Episode.Value.Substring(1, singleepisode.Episode.Value.Length - 1)
    '                            Loop
    '                        End If
    '                    End If
    '                    stage = "21"
    '                    Dim episodescraper As New TVDBScraper
    '                    If sortorder = "" Then sortorder = "default"
    '                    Dim tempsortorder As String = sortorder
    '                    If language = "" Then language = "en"
    '                    If actorsource = "" Then actorsource = "tvdb"
    '                    Pref.tvScraperLog &= "Using Settings: TVdbID: " & tvdbid & " SortOrder: " & sortorder & " Language: " & language & " Actor Source: " & actorsource & vbCrLf
    '                    stage = "22"
    '                    If tvdbid <> "" Then
    '                        progresstext &= " - Scraping..."
    '                        bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '                        Dim episodeurl As String = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/" & sortorder & "/" & singleepisode.Season.Value & "/" & singleepisode.Episode.Value & "/" & language & ".xml"
    '                        If eps.Aired.Value <> Nothing Then
    '                            episodeurl = String.Format("http://thetvdb.com/api/GetEpisodeByAirDate.php?apikey=6E82FED600783400&seriesid={0}&airdate={1}&language={2}", tvdbid, singleepisode.Aired.Value, language & ".xml")
    '                        End If
    '                        stage = "22a"
    '                        Dim tmpaok As Boolean = False
    '                        If Not Utilities.UrlIsValid(episodeurl) Then
    '                            If sortorder.ToLower = "dvd" Then
    '                                tempsortorder = "default"
    '                                Pref.tvScraperLog &= "!!! WARNING: This episode could not be found on TVDB using DVD sort order" & vbCrLf
    '                                Pref.tvScraperLog &= "!!! Attempting to find using default sort order" & vbCrLf
    '                                episodeurl = "http://thetvdb.com/api/6E82FED600783400/series/" & tvdbid & "/default/" & singleepisode.Season.Value & "/" & singleepisode.Episode.Value & "/" & language & ".xml"
    '                                Pref.tvScraperLog &= "Now Trying Episode URL: " & episodeurl & vbCrLf
    '                            End If
    '                        Else
    '                            tmpaok = True
    '                        End If
    '                        stage = "22b"
    '                        If tmpaok OrElse Utilities.UrlIsValid(episodeurl) Then
    '                            IF singleepisode.Aired.Value = Nothing AndAlso Pref.tvshow_useXBMC_Scraper = True Then
    '                                Dim FinalResult As String = ""
    '                                stage = "12b1"
    '                                episodearray = XBMCScrape_TVShow_EpisodeDetails(tvdbid, tempsortorder, episodearray, language)
    '                                stage = "12b2"
    '                                If episodearray.Count >= 1 Then
    '                                    stage = "12b2a"
    '                                    episodearray(0).NfoFilePath = savepath
    '                                    stage = "12b2b"
    '                                    For x As Integer = 0 To episodearray.Count - 1
    '                                        episodearray(x).ShowObj = singleepisode.ShowObj
    '                                        Pref.tvScraperLog &= "Scraping body of episode: " & episodearray(x).Episode.Value & " - OK" & vbCrLf
    '                                    Next
    '                                    stage = "12b2c"
    '                                    scrapedok = True
    '                                Else
    '                                    Pref.tvScraperLog &= "!!! WARNING: Could not locate this episode on TVDB, or TVDB may be unavailable" & vbCrLf
    '                                    scrapedok = False
    '                                End If
    '                                Exit For
    '                            End If
    '                            stage = "22b3"
    '                            Dim tempepisode As String = ep_Get(tvdbid, tempsortorder, singleepisode.Season.Value, singleepisode.Episode.Value, language, singleepisode.Aired.Value)
    '                            stage = "22b4"
    '                            scrapedok = True
    '                            If tempepisode = Nothing Or tempepisode = "Error" Then
    '                                scrapedok = False
    '                                singleepisode.Title.Value = tempepisode
    '                                Pref.tvScraperLog &= "!!! WARNING: This episode: " & singleepisode.Episode.Value & " - could not be found on TVDB" & vbCrLf
    '                            ElseIf tempepisode.Contains("Could not connect") Then     'If TVDB unavailable, advise user to try again later
    '                                scrapedok = False
    '                                Pref.tvScraperLog &= "!!! Issue at TheTVDb, Episode could not be retrieve. Try again later" & vbCrLf
    '                            ElseIf tempepisode.Contains("No Results from SP") Then
    '                                scrapedok = False
    '                                Pref.tvScraperLog &= "!!! Scraping using AirDate found in Filename failed.  Check Episode Filename AiredDate is correct." & vbCrLf
    '                            End If
    '                            stage = "22b5"
    '                            If scrapedok = True Then
    '                                progresstext &= "OK."
    '                                bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '                                Dim scrapedepisode As New XmlDocument
    '                                Pref.tvScraperLog &= "Scraping body of episode: " & singleepisode.Episode.Value & vbCrLf
    '                                stage = "22b5a"
    '                                scrapedepisode.LoadXml(tempepisode)
    '                                For Each thisresult As XmlNode In scrapedepisode("episodedetails")
    '                                    Select Case thisresult.Name
    '                                        Case "title"
    '                                            stage = "22b5a1"
    '                                            singleepisode.Title.Value = thisresult.InnerText.TrimEnd
    '                                        Case "premiered"
    '                                            stage = "22b5a2"
    '                                            singleepisode.Aired.Value = thisresult.InnerText
    '                                        Case "plot"
    '                                            stage = "22b5a3"
    '                                            singleepisode.Plot.Value = thisresult.InnerText
    '                                        Case "director"
    '                                            stage = "22b5a4"
    '                                            Dim newstring As String
    '                                            newstring = thisresult.InnerText
    '                                            newstring = newstring.TrimEnd("|")
    '                                            newstring = newstring.TrimStart("|")
    '                                            newstring = newstring.Replace("|", " / ")
    '                                            singleepisode.Director.Value = newstring
    '                                        Case "credits"
    '                                            stage = "22b5a5"
    '                                            Dim newstring As String
    '                                            newstring = thisresult.InnerText
    '                                            newstring = newstring.TrimEnd("|")
    '                                            newstring = newstring.TrimStart("|")
    '                                            newstring = newstring.Replace("|", " / ")
    '                                            singleepisode.Credits.Value = newstring
    '                                        Case "rating"
    '                                            stage = "22b5a6"
    '                                            singleepisode.Rating.Value = thisresult.InnerText
    '                                        Case "ratingcount"
    '                                            stage = "22b5a6a"
    '                                            singleepisode.Votes.Value = thisresult.InnerText
    '                                        Case "uniqueid"
    '                                            stage = "22b5a7"
    '                                            singleepisode.UniqueId.Value = thisresult.InnerText
    '                                        Case "showid"
    '                                            stage = "22b5a8"
    '                                            singleepisode.ShowId.Value = thisresult.InnerText
    '                                        Case "imdbid"
    '                                            stage = "22b5a9"
    '                                            singleepisode.ImdbId.Value = thisresult.InnerText
    '                                        Case "displayseason"
    '                                            stage = "22b5a10"
    '                                            singleepisode.DisplaySeason.Value = thisresult.InnerXml
    '                                        Case "displayepisode"
    '                                            stage = "22b5a11"
    '                                            singleepisode.DisplayEpisode.Value = thisresult.InnerXml
    '                                        Case "thumb"
    '                                            stage = "22b5a11"
    '                                            singleepisode.Thumbnail.FileName = thisresult.InnerText
    '                                        Case "dvd_episodenumber"
    '                                            singleepisode.DvdEpNumber.Value = thisresult.InnerText
    '                                        Case "actor"
    '                                            stage = "22b5a12"
    '                                            For Each actorl As XmlNode In thisresult.ChildNodes
    '                                                Select Case actorl.Name
    '                                                    Case "name"
    '                                                        stage = "22b5a12a"
    '                                                        Dim newactor As New str_MovieActors(SetDefaults)
    '                                                        If actorl.InnerText <> "" Then
    '                                                            newactor.actorname = actorl.InnerText
    '                                                            stage = "22b5a12b"
    '                                                            singleepisode.ListActors.Add(newactor)
    '                                                        End If
    '                                                End Select
    '                                            Next
    '                                    End Select
    '                                Next
    '                                stage = "22b5a13"
    '                                Dim ratingdone As Boolean = False
    '                                Dim rating As String = singleepisode.Rating.Value
    '                                Dim votes As String = singleepisode.Votes.Value
    '                                If Pref.tvdbIMDbRating Then
    '                                    ratingdone = GetEpRating(singleepisode, rating, votes)
    '                                End If
    '                                If Not ratingdone Then
    '                                    singleepisode.Rating.Value = rating
    '                                    singleepisode.Votes.Value = votes
    '                                End If
    '                                stage = "22b5b"
    '                                singleepisode.PlayCount.Value = "0"
    '                                singleepisode.ShowId.Value = tvdbid
    '                                stage = "22b5c"
    '                                'check file name for Episode source
    '                                Dim searchtitle As String = singleepisode.NfoFilePath
    '                                If searchtitle <> "" Then
    '                                    For i = 0 To Pref.releaseformat.Length - 1
    '                                        If searchtitle.ToLower.Contains(Pref.releaseformat(i).ToLower) Then
    '                                            singleepisode.Source.Value = Pref.releaseformat(i)
    '                                            Exit For
    '                                        End If
    '                                    Next
    '                                End If
    '                                Pref.tvScraperLog &= "Scrape body of episode: " & singleepisode.Episode.Value & " - OK" & vbCrLf
    '                                stage = "22b5d"
    '                                progresstext &= " : Scraped Title - '" & singleepisode.Title.Value & "'"
    '                                bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '                                stage = "22b5e"
    '                                If actorsource = "imdb" And (imdbid <> "" OrElse singleepisode.ImdbId.Value <> "") Then
    '                                    Pref.tvScraperLog &= "Scraping actors from IMDB" & vbCrLf
    '                                    progresstext &= " : Actors..."
    '                                    bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '                                    stage = "22b5e1"
    '                                    Dim epid As String = ""
    '                                    If singleepisode.ImdbId.Value <> "" Then
    '                                        epid = singleepisode.ImdbId.Value
    '                                    Else
    '                                        stage = "22b5e2"
    '                                        epid = GetEpImdbId(imdbid, singleepisode.Season.Value, singleepisode.Episode.Value)
    '                                    End If
    '                                    stage = "22b5e3"
    '                                    If epid.contains("tt") Then
    '                                        stage = "22b5e3a"
    '                                        'singleepisode.ListActors.Clear()
    '                                        Dim aok As Boolean = EpGetActorImdb(singleepisode)
    '                                        If aok Then
    '                                            Pref.tvScraperLog &= "Actors scraped from IMDB OK" & vbCrLf
    '                                            progresstext &= "OK."
    '                                            bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '                                        Else
    '                                            If String.IsNullOrEmpty(singleepisode.ImdbId.Value) OrElse Not singleepisode.ImdbId.Value.Contains("tt") Then
    '                                                Pref.tvScraperLog &= "!!! WARNING: No Episode IMDB Id, Actors not able to be scraped from IMDB" & vbCrLf
    '                                            Else
    '                                                Pref.tvScraperLog &= "!!! WARNING: Actors not available to scraped from IMDB" & vbCrLf
    '                                            End If
    '                                            'Pref.tvScraperLog &= "!!! WARNING: Actors not available to scraped from IMDB" & vbCrLf
    '                                        End If
    '                                        If bckgroundscanepisodes.CancellationPending Then
    '                                            Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                                            Exit Sub
    '                                        End If
    '                                    Else
    '                                        tvScraperLog = tvScraperLog & "Unable To Get Actors From IMDB" & vbCrLf
    '                                    End If
    '                                    stage = "22b5e4"
    '                                End If
    '                                If imdbid = "" Then
    '                                    Pref.tvScraperLog &= "Failed Scraping Actors from IMDB!!!  No IMDB Id for Show:  " & showtitle & vbCrLf
    '                                End If
    '                                If Pref.copytvactorthumbs AndAlso Not IsNothing(singleepisode.ShowObj) Then
    '                                    If singleepisode.ListActors.Count = 0 Then
    '                                        For each act In singleepisode.ShowObj.ListActors
    '                                            singleepisode.ListActors.Add(act)
    '                                        Next
    '                                    Else
    '                                        Dim i As Integer = singleepisode.ListActors.Count
    '                                        For each act In singleepisode.ShowObj.ListActors
    '                                            Dim q = From x In singleepisode.ListActors Where x.actorname = act.actorname
    '                                            If q.Count = 1 Then singleepisode.ListActors.Remove(q(0))
    '                                            i += 1
    '                                            singleepisode.ListActors.Add(act)
    '                                            If i = Pref.maxactors Then Exit For
    '                                        Next
    '                                    End If
    '                                End If
    '                                stage = "22b5f"
    '                                GetEpHDTags(singleepisode, progress, progresstext)
    '                                stage = "22b5g"
    '                            End If
    '                        Else
    '                            Pref.tvScraperLog &= "!!! WARNING: Could not locate this episode on TVDB, or TVDB may be unavailable" & vbCrLf
    '                            singleepisode.Title.Value = ""
    '                            If Pref.TvEpSaveNfoEmpty Then
    '                                Pref.tvScraperLog &= "!!! Basic empty nfo created as per selected option." & vbCrLf
    '                                GetEpHDTags(singleepisode, progress, progresstext)
    '                            End If
    '                            scrapedok = False
    '                        End If
    '                    Else
    '                        Pref.tvScraperLog &= "!!! WARNING: No TVDB ID is available for this show, please scrape the show using the ""TV Show Selector"" TAB" & vbCrLf
    '                        scrapedok = False
    '                    End If
    '                    stage = "22c"
    '                    Firstep = False
    '                Next
    '                If Not scrapedok AndAlso Not Firstep Then
    '                    For i = episodearray.Count - 1 To 0 Step -1
    '                        If episodearray(i).Title.Value = "Error" Then
    '                            Pref.tvScraperLog &= "!!! WARNING: MultiEpisode No: " & episodearray(i).Episode.Value & " Not Found!  Please check file: " & episodearray(i).VideoFilePath & vbCrLf
    '                            episodearray.RemoveAt(i)
    '                            scrapedok = True
    '                            ScraperErrorDetected = True
    '                        End If
    '                    Next
    '                End If
    '                stage = "23"
    '            End If
    '            stage = "24"
    '            If savepath <> "" AndAlso (scrapedok = True OrElse Pref.TvEpSaveNfoEmpty) Then
    '                If bckgroundscanepisodes.CancellationPending Then
    '                    Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                    Exit Sub
    '                End If
    '                Dim newnamepath As String = ""
    '                stage = "24a"
    '                newnamepath = ep_add(episodearray, savepath, showtitle, scrapedok)
    '                stage = "24b"
    '                For Each ep In episodearray
    '                    ep.NfoFilePath = newnamepath
    '                Next
    '                stage = "24c"
    '                If bckgroundscanepisodes.CancellationPending Then
    '                    Pref.tvScraperLog &= vbCrLf & "!!! Operation Cancelled by user" & vbCrLf
    '                    Exit Sub
    '                End If
    '                stage = "24d"
    '                stage = "24d1"
    '                If episodearray(0).NfoFilePath.IndexOf(episodearray(0).ShowObj.NfoFilePath.Replace("\tvshow.nfo", "")) <> -1 Then
    '                    stage = "24d1a"
    '                    Dim epseason As String = episodearray(0).Season.Value
    '                    Dim Seasonxx As String = episodearray(0).ShowObj.FolderPath + "season" + (If(epseason.ToInt < 10, "0" + epseason, epseason)) + (If(Pref.FrodoEnabled, "-poster.jpg", ".tbn"))
    '                    stage = "24d1b"
    '                    If epseason = "0" Then Seasonxx = episodearray(0).ShowObj.FolderPath & "season-specials" & (If(Pref.FrodoEnabled, "-poster.jpg", ".tbn"))
    '                    stage = "24d1c"
    '                    If Not File.Exists(Seasonxx) Then
    '                        TvGetArtwork(episodearray(0).ShowObj, False, False, True, False)
    '                    End If
    '                    stage = "24d1d"
    '                    If Pref.seasonfolderjpg AndAlso episodearray(0).ShowObj.FolderPath <> episodearray(0).FolderPath AndAlso (Not File.Exists(episodearray(0).FolderPath & "folder.jpg")) Then
    '                        If File.Exists(Seasonxx) Then Utilities.SafeCopyFile(Seasonxx, (episodearray(0).FolderPath & "folder.jpg"))
    '                    End If
    '                    stage = "24d1e"
    '                    For Each ep In episodearray
    '                        bckgroundscanepisodes.ReportProgress(1, ep)
    '                    Next
    '                    stage = "24d1f"
    '                    tv_EpisodesMissingUpdate(episodearray)
    '                    stage = "24d1g"
    '                End If
    '                stage = "25"
    '            End If
    '            If Not scrapedok Then ScraperErrorDetected = True
    '            Pref.tvScraperLog &= "!!!" & vbCrLf
    '            stage = "26"
    '        Next
    '        stage = "27"
    '        bckgroundscanepisodes.ReportProgress(0, progresstext)
    '        stage = "28"
    '    Catch ex As Exception
    '        stage = "stage: " & stage
    '        ExceptionHandler.LogError(ex, stage)
    '    End Try
    'End Sub

    'Public Sub GetEpHDTags(ByRef singleepisode As TvEpisode, ByRef progress As Integer, ByRef progresstext As String)
    '    If Pref.enabletvhdtags = True Then
    '        progresstext &= " : HD Tags..."
    '        bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '        Dim fileStreamDetails As StreamDetails = Pref.Get_HdTags(Utilities.GetFileName(singleepisode.VideoFilePath))
    '        If Not IsNothing(fileStreamDetails) Then
    '            singleepisode.StreamDetails.Video = fileStreamDetails.Video
    '            For Each audioStream In fileStreamDetails.Audio
    '                singleepisode.StreamDetails.Audio.Add(audioStream)
    '            Next
    '            For Each substrm In fileStreamDetails.Subtitles
    '                singleepisode.StreamDetails.Subtitles.Add(substrm)
    '            Next
    '            If Not String.IsNullOrEmpty(singleepisode.StreamDetails.Video.DurationInSeconds.Value) Then
    '                Dim tempstring As String = singleepisode.StreamDetails.Video.DurationInSeconds.Value
    '                If Pref.intruntime Then
    '                    singleepisode.Runtime.Value = Math.Round(tempstring / 60).ToString
    '                Else
    '                    singleepisode.Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
    '                End If
    '                progresstext &= "OK."
    '                bckgroundscanepisodes.ReportProgress(progress, progresstext)
    '            End If
    '        End If
    '    End If
    'End Sub
    
    'Private Sub tv_NewFind(ByVal tvpath As String, ByVal pattern As String)
    '    Dim episode As New List(Of TvEpisode)
    '    Dim propfile As Boolean = False
    '    Dim allok As Boolean = False
    '    Dim dir_info As New DirectoryInfo(tvpath)

    '    Dim fs_infos() As String = Directory.GetFiles(tvpath, "*" & pattern, IO.SearchOption.TopDirectoryOnly)
    '    Dim counter As Integer = 1
    '    Dim counter2 As Integer = 1
    '    For Each FilePath As String In fs_infos
    '        Dim filename_video As String = FilePath
    '        Dim filename_nfo As String = filename_video.Replace(Path.GetExtension(filename_video), ".nfo")
    '        If File.Exists(filename_nfo) Then
    '            If ep_NfoValidate(filename_nfo) = False And Pref.renamenfofiles = True Then
    '                Dim movefilename As String = filename_nfo.Replace(Path.GetExtension(filename_nfo), ".info")
    '                Try
    '                    If File.Exists(movefilename) Then Utilities.SafeDeleteFile(movefilename)
    '                    File.Move(filename_nfo, movefilename)
    '                Catch ex As Exception
    '                    Utilities.SafeDeleteFile(movefilename)
    '                End Try
    '            End If
    '        End If
    '        If Not File.Exists(filename_nfo) Then
    '            Dim add As Boolean = True
    '            If pattern = ".vob" Then 'If a vob file is detected, check that it is not part of a dvd file structure
    '                Dim name As String = filename_nfo
    '                name = name.Replace(Path.GetFileName(name), "VIDEO_TS.IFO")
    '                If File.Exists(name) Then add = False
    '            End If
    '            If pattern = ".rar" Then
    '                Dim tempmovie As String = String.Empty
    '                Dim tempint2 As Integer = 0
    '                Dim tempmovie2 As String = FilePath
    '                If Path.GetExtension(tempmovie2).ToLower = ".rar" Then
    '                    If File.Exists(tempmovie2) = True Then
    '                        If File.Exists(tempmovie) = False Then
    '                            Dim rarname As String = tempmovie2
    '                            Dim SizeOfFile As Integer = FileLen(rarname)
    '                            tempint2 = Convert.ToInt32(Pref.rarsize) * 1048576
    '                            If SizeOfFile > tempint2 Then
    '                                Dim mat As Match
    '                                mat = Regex.Match(rarname, "\.part[0-9][0-9]?[0-9]?[0-9]?.rar")
    '                                If mat.Success = True Then
    '                                    rarname = mat.Value
    '                                    If rarname.ToLower.IndexOf(".part1.rar") <> -1 Or rarname.ToLower.IndexOf(".part01.rar") <> -1 Or rarname.ToLower.IndexOf(".part001.rar") <> -1 Or rarname.ToLower.IndexOf(".part0001.rar") <> -1 Then
    '                                        Dim stackrarexists As Boolean = False
    '                                        rarname = tempmovie.Replace(".nfo", ".rar")
    '                                        If rarname.ToLower.IndexOf(".part1.rar") <> -1 Then
    '                                            rarname = rarname.Replace(".part1.rar", ".nfo")
    '                                            If File.Exists(rarname) Then
    '                                                stackrarexists = True
    '                                                tempmovie = rarname
    '                                            Else
    '                                                stackrarexists = False
    '                                                tempmovie = rarname
    '                                            End If
    '                                        End If
    '                                        If rarname.ToLower.IndexOf(".part01.rar") <> -1 Then
    '                                            rarname = rarname.Replace(".part01.rar", ".nfo")
    '                                            If File.Exists(rarname) Then
    '                                                stackrarexists = True
    '                                                tempmovie = rarname
    '                                            Else
    '                                                stackrarexists = False
    '                                                tempmovie = rarname
    '                                            End If
    '                                        End If
    '                                        If rarname.ToLower.IndexOf(".part001.rar") <> -1 Then
    '                                            rarname = rarname.Replace(".part001.rar", ".nfo")
    '                                            If File.Exists(rarname) Then
    '                                                tempmovie = rarname
    '                                                stackrarexists = True
    '                                            Else
    '                                                stackrarexists = False
    '                                                tempmovie = rarname
    '                                            End If
    '                                        End If
    '                                        If rarname.ToLower.IndexOf(".part0001.rar") <> -1 Then
    '                                            rarname = rarname.Replace(".part0001.rar", ".nfo")
    '                                            If File.Exists(rarname) Then
    '                                                tempmovie = rarname
    '                                                stackrarexists = True
    '                                            Else
    '                                                stackrarexists = False
    '                                                tempmovie = rarname
    '                                            End If
    '                                        End If
    '                                    Else
    '                                        add = False
    '                                    End If
    '                                Else
    '                                    'remove = True
    '                                End If
    '                            Else
    '                                add = False
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '            Dim truefilename As String = Utilities.GetFileNameFromPath(filename_video)
    '            If truefilename.Substring(0, 2) = "._" Then add = False
    '            If add = True Then
    '                Dim newep As New TvEpisode
    '                newep.NfoFilePath = filename_nfo
    '                newep.VideoFilePath = filename_video
    '                newep.MediaExtension = Path.GetExtension(filename_video)
    '                newEpisodeList.Add(newep)
    '            End If
    '        End If
    '    Next
    '    fs_infos = Nothing
    'End Sub

    Sub tv_Rescrape_Episode(ByRef WorkingTvShow As TVShow, ByRef WorkingEpisode As TvEpisode)
        Dim tempint As Integer = 0
        Dim tempstring As String = ""
        If WorkingEpisode.IsMissing Then
            MsgBox("This is a Missing Episode, and can not be rescraped!")
            Exit Sub
        End If
        If Utilities.GetTvEpExtension(WorkingEpisode.NfoFilePath) = "error" Then
            tempint = MessageBox.Show("Video file for this episode does not exist." & vbCrLf & "Please Delete this episode's nfo, and refresh the Show for the selected episode.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        tempint = MessageBox.Show("Rescraping the Episode will Overwrite all the current details" & vbCrLf & "Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If tempint = DialogResult.No Then Exit Sub
        
        Application.DoEvents()
        RunBackgroundTVScrape("TVEpisodeRescrape")
        
    End Sub

    Public Function tv_EpisodeFanartGet(ByVal episode As TvEpisode, ByVal doDownloadThumb As Boolean, ByVal doScreenShot As Boolean) As String
        Dim result As String = "!!!  *** Unable to download Episode Thumb ***"
        Dim fpath As String = episode.NfoFilePath.Replace(".nfo", ".tbn")
        Dim paths As New List(Of String)
        If Pref.EdenEnabled AndAlso (Pref.overwritethumbs Or Not File.Exists(fpath)) Then paths.Add(fpath)
        fpath = fpath.Replace(".tbn", "-thumb.jpg")
        If Pref.FrodoEnabled AndAlso (Pref.overwritethumbs Or Not File.Exists(fpath)) Then paths.Add(fpath)
        If paths.Count > 0 Then
            Dim downloadok As Boolean = False
            If doDownloadThumb Then
                If episode.Thumbnail.FileName = Nothing Then
                    Dim tvdbstuff As New TVDBScraper
                    Dim tempepisode As Tvdb.Episode = tvdbstuff.getepisodefromxml(episode.ShowId.Value, episode.sortorder.Value, episode.Season.value, episode.Episode.Value, episode.ShowLang.Value, True)
                    If tempepisode.ThumbNail.Value <> Nothing Then episode.Thumbnail.FileName = tempepisode.ThumbNail.Value
                End If
                If episode.Thumbnail.FileName <> Nothing AndAlso episode.Thumbnail.FileName <> "http://www.thetvdb.com/banners/" Then
                    Dim url As String = episode.Thumbnail.FileName
                    If Not url.IndexOf("http") = 0 And url.IndexOf(".jpg") <> -1 Then url = episode.Thumbnail.Url
                    If url <> Nothing AndAlso url.IndexOf("http") = 0 AndAlso url.IndexOf(".jpg") <> -1 Then
                        downloadok = DownloadCache.SaveImageToCacheAndPaths(url, paths, True, , , Pref.overwritethumbs)
                    Else
                        result = "!!! No thumbnail to download"
                    End If
                    If downloadok Then result = "!!! Episode Thumb downloaded"
                Else
                    result = "!!! No thumbnail to download"
                End If
            End If
            If Not downloadok AndAlso doScreenShot Then
                Dim cachepathandfilename As String = Utilities.CreateScrnShotToCache(episode.VideoFilePath, paths(0), Pref.ScrShtDelay)
                If Not cachepathandfilename = "" Then
                    Dim imagearr() As Integer = GetAspect(episode)
                    If Pref.tvscrnshtTVDBResize AndAlso Not imagearr(0) = 0 Then
                        DownloadCache.CopyAndDownSizeImage(cachepathandfilename, paths(0), imagearr(0), imagearr(1))
                    Else
                        File.Copy(cachepathandfilename, paths(0), Pref.overwritethumbs)
                    End If
                    If paths.Count > 1 Then File.Copy(paths(0), paths(1), Pref.overwritethumbs)
                    result = "!!! No Episode thumb to download, Screenshot saved"
                Else
                    result = "!!! No Episode thumb to download, Screenshot Could not be saved!"
                End If
            End If
        ElseIf paths.Count = 0 Then
            result = "!!! Episode Thumb(s) already exist and are not set to overwrite"
        End If
        Return result
    End Function

    Public Function tv_ShowSelectedCurrently(ByRef tree As TreeView) As Media_Companion.TvShow
        If tree.SelectedNode Is Nothing Then
            If tree.Nodes.Count = 0 Then Return Nothing
            tree.SelectedNode = tree.TopNode
        End If
        If tree.SelectedNode Is Nothing Then
            tree.SelectedNode = tree.Nodes(0)
        End If

        Dim Show As Media_Companion.TvShow = Nothing
        Dim Season As Media_Companion.TvSeason = Nothing
        Dim Episode As Media_Companion.TvEpisode = Nothing

        If TypeOf tree.SelectedNode.Tag Is Media_Companion.TvShow Then
            Show = tree.SelectedNode.Tag
        ElseIf TypeOf tree.SelectedNode.Tag Is Media_Companion.TvSeason Then
            Season = tree.SelectedNode.Tag
            Show = Season.GetParentShow
        ElseIf TypeOf tree.SelectedNode.Tag Is Media_Companion.TvEpisode Then
            Episode = tree.SelectedNode.Tag
            Season = Episode.SeasonObj
            Show = Episode.ShowObj
        End If

        Return Show
    End Function

    Public Function tv_SeasonSelectedCurrently(ByRef tree As TreeView) As Media_Companion.TvSeason
        If tree.SelectedNode Is Nothing Then Return Nothing

        Dim Show As Media_Companion.TvShow = Nothing
        Dim Season As Media_Companion.TvSeason = Nothing
        Dim Episode As Media_Companion.TvEpisode = Nothing

        If TypeOf tree.SelectedNode.Tag Is Media_Companion.TvShow Then
            Show = tree.SelectedNode.Tag
        ElseIf TypeOf tree.SelectedNode.Tag Is Media_Companion.TvSeason Then
            Season = tree.SelectedNode.Tag
            Show = Season.GetParentShow
        ElseIf TypeOf tree.SelectedNode.Tag Is Media_Companion.TvEpisode Then
            Episode = tree.SelectedNode.Tag
            Season = Episode.SeasonObj
            Show = Episode.ShowObj
        End If

        Return Season
    End Function

    Public Function ep_SelectedCurrently(ByRef tree As TreeView) As Media_Companion.TvEpisode
        If tree.SelectedNode Is Nothing Then Return Nothing

        Dim Show As Media_Companion.TvShow = Nothing
        Dim Season As Media_Companion.TvSeason = Nothing
        Dim Episode As Media_Companion.TvEpisode = Nothing

        If TypeOf tree.SelectedNode.Tag Is Media_Companion.TvShow Then
            Show = tree.SelectedNode.Tag
        ElseIf TypeOf tree.SelectedNode.Tag Is Media_Companion.TvSeason Then
            Season = tree.SelectedNode.Tag
            Show = Season.GetParentShow
        ElseIf TypeOf tree.SelectedNode.Tag Is Media_Companion.TvEpisode Then
            Episode = tree.SelectedNode.Tag
            Season = Episode.SeasonObj
            Show = Episode.ShowObj
        End If

        Return Episode
    End Function

    Private Sub tv_Filter()
        tv_Filter(Nothing)
    End Sub

    Private Sub tv_Filter(ByVal overrideShowIsMissing As String)
        Dim butt As String = ""
        Dim ThisDate As Date = If(Pref.TvMissingEpOffset, Now.AddDays(-1), Now)
        Dim eden As Boolean = Pref.EdenEnabled
        Dim frodo As Boolean = Pref.FrodoEnabled
        Dim overrideIsMissing As Boolean = overrideShowIsMissing IsNot Nothing

        If rbTvListAll.Checked Then butt = "all"
        If rbTvMissingFanart.Checked Then butt = "fanart"
        If rbTvMissingPoster.Checked Then butt = "posters"
        If rbTvMissingThumb.Checked Then butt = "screenshot"
        If rbTvMissingEpisodes.Checked Then butt = "missingeps"
        If rbTvMissingAiredEp.Checked Then butt = "airedmissingeps"
        If rbTvMissingNextToAir.Checked Then butt = "nexttoair"
        If rbTvDisplayWatched.Checked Then butt = "watched"
        If rbTvDisplayUnWatched.Checked Then butt = "unwatched"
        If rbTvListContinuing.Checked Then butt = "continuing"
        If rbTvListEnded.Checked Then butt = "ended"
        If rbTvListUnKnown.Checked Then butt = "unknown"

        tvfiltertrip = True
        If startup = True Then butt = "all"
        If butt = "missingeps" Then
            If Pref.displayMissingEpisodes Then
                For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                    For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                        For Each episode As Media_Companion.TvEpisode In Season.Episodes
                            If Not episode.IsMissing Then
                                episode.Visible = False
                            Else
                                If String.IsNullOrEmpty(episode.Aired.Value) Then                   ' Change the colour to gray
                                    episode.EpisodeNode.ForeColor = Color.Gray
                                Else
                                    Try
                                        episode.EpisodeNode.ForeColor =If(Convert.ToDateTime(episode.Aired.Value) > ThisDate, Color.Red, Color.Blue)
                                        'If Convert.ToDateTime(episode.Aired.Value) > ThisDate Then       ' Is the episode in the future?
                                        '    episode.EpisodeNode.ForeColor = Color.Red               '  Yes, so change its colour to Red
                                        'Else
                                        '    episode.EpisodeNode.ForeColor = Drawing.Color.Blue
                                        'End If
                                    Catch ex As Exception
                                        episode.EpisodeNode.ForeColor = Drawing.Color.Blue          ' Set the colour to the missing colour
                                    End Try
                                End If
                                episode.Visible = True
                                episode.EpisodeNode.EnsureVisible()
                            End If
                        Next
                        Season.Visible = Season.VisibleEpisodeCount > 0
                    Next
                    item.Visible = item.VisibleSeasonCount > 0
                    item.Hidden.Value = Not item.Visible
                Next
            Else
                MsgBox("Enable Display Missing Episodes")
            End If
        ElseIf butt.Contains("watched") Then
            Dim playcount As Integer = If(butt = "watched", 1, 0)
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        'If episode.PlayCount.Value = playcount.ToString Then
                        '    episode.Visible = True
                        'Else
                        '    episode.Visible = False
                        'End If
                        episode.VideoFilePath = If(episode.PlayCount.Value = playcount.ToString, True, False)
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = item.VisibleSeasonCount > 0
                item.Hidden.Value = Not item.Visible
            Next
        'ElseIf butt.Contains("unwatched") Then
        '    Dim playcount As Integer = If(butt = "watched", 1, 0)
        '    For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
        '        For Each Season As Media_Companion.TvSeason In item.Seasons.Values
        '            For Each episode As Media_Companion.TvEpisode In Season.Episodes
        '                'If episode.PlayCount.Value = playcount.ToString Then
        '                '    episode.Visible = True
        '                'Else
        '                '    episode.Visible = False
        '                'End If
        '                episode.VideoFilePath = If(episode.PlayCount.Value = playcount.ToString, True, False)
        '            Next
        '            Season.Visible = Season.VisibleEpisodeCount > 0
        '        Next
        '        item.Visible = item.VisibleSeasonCount > 0
        '        item.Hidden.Value = Not item.Visible
        '    Next
        ElseIf butt = "airedmissingeps" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        If Not episode.IsMissing Then
                            episode.Visible = False
                        Else
                            If String.IsNullOrEmpty(episode.Aired.Value) Then
                                episode.Visible = False
                            Else
                                Try     ' Has the episode been aired yet?
                                    If Convert.ToDateTime(episode.Aired.Value) <= ThisDate Then
                                        episode.Visible = True
                                        episode.EpisodeNode.EnsureVisible()
                                    Else
                                        episode.Visible = False
                                    End If
                                Catch ex As Exception
                                    episode.Visible = False     ' We failed to convert the aired date to a date, therefore don't show the episode
                                End Try
                            End If
                        End If
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = item.VisibleSeasonCount > 0
                item.Hidden.Value = Not item.Visible
            Next
        ElseIf butt = "nexttoair" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                Dim found As Boolean = False
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        If found OrElse Not episode.IsMissing Then
                            episode.Visible = False
                        Else
                            If String.IsNullOrEmpty(episode.Aired.Value) Then
                                episode.Visible = False
                            Else
                                Try     ' Has the episode been aired yet?
                                    If Convert.ToDateTime(episode.Aired.Value) >= ThisDate Then
                                        episode.Visible = True
                                        episode.EpisodeNode.EnsureVisible()
                                        found = True
                                    Else
                                        episode.Visible = False
                                    End If
                                Catch ex As Exception
                                    episode.Visible = False     ' We failed to convert the aired date to a date, therefore don't show the episode
                                End Try
                            End If
                        End If
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = item.VisibleSeasonCount > 0
                item.Hidden.Value = Not item.Visible
            Next
        ElseIf butt = "screenshot" Then
            Dim edenart As String = ""
            Dim frodoart As String = ""
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        If episode.IsMissing Then
                            episode.Visible = False
                            Continue For
                        End If
                        edenart = episode.Thumbnail.Path
                        frodoart = episode.Thumbnail.Path.Replace(".tbn", "-thumb.jpg")
                        If (Not String.IsNullOrEmpty(episode.Thumbnail.FileName)) Then
                            If ((eden And Not frodo) AndAlso File.Exists(edenart)) Or ((frodo And Not eden) AndAlso File.Exists(frodoart)) Or ((frodo And eden) And (File.Exists(edenart) And File.Exists(frodoart))) Then
                                episode.Visible = False
                            Else
                                episode.Visible = True
                                episode.EpisodeNode.EnsureVisible()
                            End If
                        End If
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = item.VisibleSeasonCount > 0
                item.Hidden.Value = Not item.Visible
            Next
        ElseIf butt = "all" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                item.Visible = True
                item.Hidden.Value = False.ToString
                Dim containsVisibleSeason As Boolean = False
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        If episode.IsMissing AndAlso Not (Pref.displayMissingEpisodes Or (overrideIsMissing AndAlso episode.ShowObj.ToString = overrideShowIsMissing)) Then
                            episode.Visible = False
                        Else
                            Try
                                If episode.IsMissing Then
                                    If episode.Aired.Value <> "" Then
                                        If Convert.ToDateTime(episode.Aired.Value) > ThisDate Then
                                            episode.EpisodeNode.ForeColor = Color.Red           '  Yes, so change its colour to Red
                                        Else
                                            episode.EpisodeNode.ForeColor = Drawing.Color.Blue
                                        End If
                                    Else
                                        episode.EpisodeNode.ForeColor = Drawing.Color.gray
                                    End If
                                End If
                            Catch
                                episode.EpisodeNode.ForeColor = Color.Red
                            End Try
                            episode.Visible = True
                        End If
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = item.VisibleSeasonCount > 0
            Next
        ElseIf butt = "continuing" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                Dim visible As Boolean = item.Status.Value = "Continuing"
                item.Hidden.Value = Not visible
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        episode.Visible = visible
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = True
            Next
        ElseIf butt = "ended" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                Dim visible As Boolean = item.Status.Value = "Ended"
                item.Hidden.Value = Not visible
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        episode.Visible = visible
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = True
            Next
        ElseIf butt = "unknown" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                Dim visible As Boolean = item.Status.Value = ""
                item.Hidden.Value = Not visible
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        episode.Visible = visible
                    Next
                    Season.Visible = Season.VisibleEpisodeCount > 0
                Next
                item.Visible = True
            Next
        ElseIf butt = "fanart" Then
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                item.Visible = True
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    For Each episode As Media_Companion.TvEpisode In Season.Episodes
                        episode.Visible = False
                    Next
                    Season.Visible = False
                Next
                'If item.ImageFanart.Exists Then
                '    item.Visible = False
                'Else
                '    item.Visible = True
                'End If
                item.Visible = Not item.ImageFanart.Exists
                item.Hidden.Value = Not item.Visible
            Next
        ElseIf butt = "posters" Then
            Dim edenpost As String = ""
            Dim frodopost As String = ""
            Dim frodobann As String = ""
            For Each item As Media_Companion.TvShow In Cache.TvCache.Shows
                item.Visible = False
                For Each Season As Media_Companion.TvSeason In item.Seasons.Values
                    edenpost = Season.Poster.Path
                    frodopost = Season.Poster.Path.Replace(".tbn", "-poster.jpg")
                    frodobann = Season.Banner.Path
                    If ((eden And frodo) AndAlso (File.Exists(edenpost) And (File.Exists(frodopost) AndAlso File.Exists(frodobann)))) Or ((eden And Not frodo) AndAlso File.Exists(edenpost)) Or ((frodo And Not eden) AndAlso (File.Exists(frodopost) Or File.Exists(frodobann))) Then
                        Season.Visible = False
                    Else
                        Season.Visible = True
                    End If
                    For Each Episode As Media_Companion.TvEpisode In Season.Episodes
                        Episode.Visible = False
                    Next
                Next
                If frodo And Not File.Exists(item.FolderPath + "poster.jpg") Then
                    item.Visible = True
                ElseIf eden And Not frodo Then
                    item.Visible = Not item.ImagePoster.Exists
                End If
                item.Hidden.Value = Not item.Visible
            Next
        End If

        TvTreeviewRebuild()
    End Sub

#Region "Tv MissingEpisode Routines"

    Private Sub Bckgrndfindmissingepisodes_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles Bckgrndfindmissingepisodes.DoWork
        Try
            Statusstrip_Enable()
            Call tv_EpisodesMissingFind(e.Argument)
            e.Result = e.Argument
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Bckgrndfindmissingepisodes_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles Bckgrndfindmissingepisodes.ProgressChanged
        Try
            If e.ProgressPercentage = 1 Then
                If TypeOf e.UserState Is Media_Companion.TvEpisode Then
                    Dim MissingEpisode As Media_Companion.TvEpisode = e.UserState

                    MissingEpisode.ShowObj.AddEpisode(MissingEpisode)
                End If
            Else
                ToolStripStatusLabel2.Text = e.UserState
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub

    Private Sub Bckgrndfindmissingepisodes_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles Bckgrndfindmissingepisodes.RunWorkerCompleted
        Try
            Dim ShowList As List(Of TvShow) = e.Result
            Pref.DlMissingEpData = False
            ToolStripStatusLabel2.Visible = False
            ToolStripStatusLabel2.Text = "TV Show Episode Scan In Progress"
            Statusstrip_Enable(False)
            Application.DoEvents()
            TvTreeview.Sort()
            Dim showToRefresh = Nothing
            If (ShowList.Count = 1) Then
                ' ignore missing episodes checked entry for this forced node refresh
                showToRefresh = ShowList(0).ToString()
            End If
            tv_CacheLoad()
            tv_Filter(showToRefresh)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub tv_EpisodesMissingLoad(ByVal refresh As Boolean)
        Try
            If Not Bckgrndfindmissingepisodes.IsBusy AndAlso Not bckgroundscanepisodes.IsBusy AndAlso Not BckWrkTv.IsBusy Then
                If refresh Then
                    Dim nod As TreeNode
                    For Each nod In TvTreeview.Nodes
                        Dim nod2 As TreeNode
                        For Each nod2 In nod.Nodes
                            Dim nod3 As TreeNode
                            For I = nod2.Nodes.count - 1 to 0 Step -1
                                nod3 = nod2.Nodes(I)
                                Dim episo As TvEpisode
                                episo = nod3.Tag
                                If episo.IsMissing Then nod2.Nodes.RemoveAt(I)
                            Next
                        Next
                    Next
                    Dim epcount As Integer = Cache.TvCache.Episodes.Count - 1
                    For I = epcount to 0 Step -1
                        Dim episode As TvEpisode = Cache.TvCache.Episodes.Item(I)
                        If episode.IsMissing = True Then Cache.TvCache.Remove(episode)
                    Next
                    Tv_CacheSave()
                    tv_CacheLoad()
                End If
                Dim ShowList As New List(Of TvShow)
                For Each shows In Cache.TvCache.Shows
                    ShowList.Add(shows)
                Next
                ToolStripStatusLabel2.Text = "Starting search for missing episodes"
                ToolStripStatusLabel2.Visible = True
                Bckgrndfindmissingepisodes.RunWorkerAsync(ShowList)
            ElseIf Bckgrndfindmissingepisodes.IsBusy Then
                MsgBox("Process is already running")
            Else
                MsgBox("Missing episode search cannot be performed" & vbCrLf & "    when the episode scraper is running")
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub tv_EpisodesMissingFind(ByVal ShowList As List(Of TvShow))
        Utilities.EnsureFolderExists(Utilities.MissingPath)
        For Each item In ShowList
            Bckgrndfindmissingepisodes.ReportProgress(0, "Downloading episode data for: " & item.Title.Value)
            If item.State = Media_Companion.ShowState.Open Then
                Dim showid As String = item.TvdbId.Value
                If IsNumeric(showid) Then
                    'http://www.thetvdb.com/api/6E82FED600783400/series/85137/all/en.xml
                    Dim language As String = ""
                    If item.Language.Value <> "" Then
                        language = item.Language.Value
                    Else
                        language = "en"
                    End If
                    Dim sortorder As String = item.SortOrder.Value
                    Dim url As String = "http://www.thetvdb.com/api/6E82FED600783400/series/" & showid & "/all/" & language & ".xml"
                    If sortorder = "" Then
                        sortorder = "default"
                    End If
                    Dim xmlfile As String = Utilities.SeriesXmlPath & showid & ".xml"
                    Dim SeriesInfo As New Tvdb.ShowData
                    If Not File.Exists(Utilities.SeriesXmlPath & showid & ".xml") OrElse Pref.DlMissingEpData Then
                        If Not DownloadCache.Savexmltopath(url, Utilities.SeriesXmlPath, showid & ".xml", True) Then
                            MsgBox("Error retrieving data from show" & vbCrLf & "--   " & item.Title.Value.ToString & "   --", )
                            Continue For
                        End If
                    End If
                    SeriesInfo.Load(xmlfile)

                    For Each NewEpisode As Tvdb.Episode In SeriesInfo.Episodes
                        If Pref.ignoreMissingSpecials AndAlso NewEpisode.SeasonNumber.Value = "0" Then
                            Dim missingspecialnfo As String = Utilities.MissingPath & item.TvdbId.Value & "." & NewEpisode.SeasonNumber.Value & "." & NewEpisode.EpisodeNumber.Value & ".nfo"
                            If File.Exists(missingspecialnfo) Then Utilities.SafeDeleteFile(missingspecialnfo)
                            Continue For
                        End If
                        Dim Episode As TvEpisode = item.GetEpisode(NewEpisode.SeasonNumber.Value, NewEpisode.EpisodeNumber.Value)
                        If Episode Is Nothing OrElse Not File.Exists(Episode.NfoFilePath) Then
                            Dim MissingEpisode As New Media_Companion.TvEpisode
                            MissingEpisode.NfoFilePath = Utilities.MissingPath & item.TvdbId.Value & "." & NewEpisode.SeasonNumber.Value & "." & NewEpisode.EpisodeNumber.Value & ".nfo"
                            If Not File.Exists(MissingEpisode.NfoFilePath) OrElse Pref.DlMissingEpData Then
                                MissingEpisode.AbsorbTvdbEpisode(NewEpisode)
                                MissingEpisode.IsMissing = True
                                MissingEpisode.IsCache = True
                                MissingEpisode.ShowObj = item
                                MissingEpisode.Save()
                            Else
                                MissingEpisode.Load()
                            End If
                            item.AddEpisode(MissingEpisode)
                            Bckgrndfindmissingepisodes.ReportProgress(1, MissingEpisode)
                        End If
                    Next
                End If
            End If
        Next
        Tv_CacheSave()
    End Sub

    Public Function tv_EpisodesMissingUpdate(ByRef newEpList As List(Of TvEpisode)) As Boolean
        Dim Removed As Boolean = False
        Try
            For Each Ep In newEpList
                If File.Exists(Ep.NfoFilePath) Then
                    Dim missingEpNfoPath As String = Utilities.MissingPath & Ep.TvdbId.Value & "." & Ep.Season.Value & "." & Ep.Episode.Value & ".nfo"
                    If Not File.Exists(missingEpNfoPath) Then Continue For
                    File.Delete(missingEpNfoPath)
                    Dim Ep2Remove As New TvEpisode
                    For Each epis As TvEpisode In Cache.TvCache.Episodes
                        If epis.TvdbId.Value = Ep.TvdbId.Value Then
                            If epis.Season.Value = Ep.Season.Value AndAlso epis.Episode.Value = Ep.Episode.Value AndAlso epis.IsMissing = True Then
                                Ep2Remove = epis
                                Removed = True
                                Exit For
                            End If
                        End If
                    Next
                    If Removed Then Cache.TvCache.Remove(Ep2Remove)
                End If
            Next
        Catch
        End Try
        Return Removed
    End Function

#End Region

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

#Region "Tasks"
    Private Sub lstTasks_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TasksList.SelectedIndexChanged
        If TasksList.SelectedItem Is Nothing Then Exit Sub

        Dim SelectedTask As ITask

        SelectedTask = TasksList.SelectedItem

        Select Case SelectedTask.State
            Case TaskState.Completed
                TasksStateLabel.Text = "Completed"
            Case TaskState.BackgroundWorkComplete
                TasksStateLabel.Text = "Background Completed"
            Case TaskState.CriticalFault
                TasksStateLabel.Text = "Critial Fault"
            Case TaskState.Fault
                TasksStateLabel.Text = "Fault"
            Case TaskState.Halted
                TasksStateLabel.Text = "Halted"
            Case TaskState.NotStarted
                TasksStateLabel.Text = "Not Started"
            Case TaskState.WaitingForUserInput
                TasksStateLabel.Text = "Waiting For Input"
            Case TaskState.Running
                TasksStateLabel.Text = "Running"
        End Select

        TasksArgumentSelector.Items.Clear()
        TasksArgumentSelector.Text = ""
        For Each Item In SelectedTask.Arguments
            TasksArgumentSelector.Items.Add(Item)

        Next
        If TasksArgumentSelector.Items.Count > 0 Then TasksArgumentSelector.SelectedIndex = 0

        TasksDependancies.Items.Clear()
        For Each Item In SelectedTask.Dependancies
            TasksDependancies.Items.Add(Item)
        Next

        TasksMessages.Items.Clear()
        For Each Item In SelectedTask.Messages
            TasksMessages.Items.Add(Item)
        Next
    End Sub

    Private Sub lstTasks_Messages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TasksMessages.SelectedIndexChanged
        If TasksMessages.SelectedItem Is Nothing Then Exit Sub

        TasksSelectedMessage.Text = TasksMessages.SelectedItem
    End Sub

    Private Sub cmbTasks_Arguments_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TasksArgumentSelector.SelectedIndexChanged
        If TasksArgumentSelector.SelectedItem Is Nothing Then Exit Sub

        TasksArgumentDisplay.Controls.Clear()

        Dim Item As KeyValuePair(Of String, Object) = TasksArgumentSelector.SelectedItem
        If TypeOf Item.Value Is String Then
            TasksArgumentDisplay.Controls.Add(New TextBox() With {.Text = Item.Value, .Dock = DockStyle.Fill, .Multiline = True, .ScrollBars = ScrollBars.Both})
        ElseIf TypeOf Item.Value Is TvShow Then
            Dim TempShow As TvShow = Item.Value
            TasksArgumentDisplay.Controls.Add(New TextBox() With {.Text = TempShow.TvdbId.Value & " - " & TempShow.Title.Value, .Dock = DockStyle.Fill, .ScrollBars = ScrollBars.Both})
        ElseIf TypeOf Item.Value Is TvEpisode Then
            Dim TempEpisode As TvEpisode = Item.Value
            TasksArgumentDisplay.Controls.Add(New TextBox() With {.Text = TempEpisode.Id.Value & " - " & TempEpisode.Title.Value, .Dock = DockStyle.Fill, .ScrollBars = ScrollBars.Both})
        ElseIf TypeOf Item.Value Is Image Then
            TasksArgumentDisplay.Controls.Add(New PictureBox() With {.Image = Item.Value, .Dock = DockStyle.Fill})
        End If
    End Sub

    Private Sub TasksTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TasksTest.Click
        For I = 0 To 20
            Common.Tasks.Add(New Tasks.BlankTask())
        Next

        RefreshTaskList()
    End Sub

    Private Sub TasksRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TasksRefresh.Click
        RefreshTaskList()
    End Sub

    Private Sub TasksClearCompleted_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TasksClearCompleted.Click
        'Manually building a for loop
        Dim Count As Long = Common.Tasks.Count
        Dim Cursor As Long = 0

        Dim CurrentTask As ITask
        Do
            CurrentTask = Common.Tasks(Cursor)
            If CurrentTask.State = TaskState.Completed Then
                Common.Tasks.Remove(CurrentTask)
                Count -= 1
            Else
                Cursor += 1
            End If
            If Count < 1 Or Cursor >= Count Then
                Exit Do
            End If
            System.Windows.Forms.Application.DoEvents()
        Loop

        RefreshTaskList()
    End Sub

    Public TasksOnlyIncompleteTasks As Boolean = True

    Public Sub RefreshTaskList()
        For Each Item As ITask In Common.Tasks
            If Not (TasksOnlyIncompleteTasks AndAlso Item.State = TaskState.Completed) AndAlso Not TasksList.Items.Contains(Item) Then
                TasksList.Items.Add(Item)
                System.Windows.Forms.Application.DoEvents()
            End If
        Next

        Dim taskCount = TasksList.Items.Count
        Dim Cursor = 0

        Dim CurrentTask As ITask
        Do
            If taskCount < 1 Or Cursor >= taskCount Then
                Exit Do
            End If

            Try
                CurrentTask = TasksList.Items(Cursor)
                If (TasksOnlyIncompleteTasks AndAlso CurrentTask.State = TaskState.Completed) OrElse Not Common.Tasks.Contains(CurrentTask) Then
                    TasksList.Items.Remove(CurrentTask)
                    taskCount -= 1

                Else
                    Cursor += 1
                End If
            Catch
            End Try

            System.Windows.Forms.Application.DoEvents()
        Loop
    End Sub

#End Region

#Region "Tv Artwork, TV Actor & EP Thumbnail Routines"
    Private Function TvGetArtwork(ByVal currentshow As TvShow, ByVal shFanart As Boolean, ByVal shPosters As Boolean, ByVal shSeason As Boolean, ByVal shXtraFanart As Boolean, Optional ByVal langu As String = "", Optional ByVal force As Boolean = True) As Boolean
        Dim success As Boolean = False
        Try
            Dim MaxSeasonNo As Integer = 1
            Dim tvdbstuff As New TVDBScraper
            Dim currentshowpath As String = currentshow.NfoFilePath.Replace("tvshow.nfo", "")
            Dim eden As Boolean = Pref.EdenEnabled
            Dim frodo As Boolean = Pref.FrodoEnabled
            Dim doPoster As Boolean = If(Pref.tvdlposter OrElse Pref.TvChgShowDlPoster, True, False)
            Dim doFanart As Boolean = If(Pref.tvdlfanart OrElse Pref.TvChgShowDlFanart, True, False)
            Dim doSeason As Boolean = If(Pref.tvdlseasonthumbs OrElse Pref.TvChgShowDlSeasonthumbs, True, False)
            Dim artlist As New List(Of TvBanners)
            artlist.Clear()
            Dim thumblist As String = tvdbstuff.GetPosterList(currentshow.TvdbId.Value, artlist)
            Dim isposter As String = Pref.postertype
            Dim isseasonall As String = Pref.seasonall
            Dim overwriteimage As Boolean = If(Pref.overwritethumbs OrElse Pref.TvChgShowOverwriteImgs, True, False)
            If Not force then overwriteimage = False    'Over-ride overwrite if we force not to overwrite, ie: missing artwork

            Dim Langlist As New List(Of String)
            If Not langu = "" Then Langlist.Add(langu)
            Langlist.Add(currentshow.Language.Value)
            If Not Langlist.Contains("en") Then Langlist.Add("en")
            If Not Langlist.Contains(Pref.TvdbLanguageCode) Then Langlist.Add(Pref.TvdbLanguageCode)
            Langlist.Add("")

            If artlist.Count = 0 Then Exit Function
            For each art In artlist
                If Not IsNothing(art.Season) AndAlso art.Season.ToInt > MaxSeasonNo Then MaxSeasonNo = art.Season.ToInt
            Next

            'Posters, Main and Season Including Banners
            If shPosters Then
                'Main Poster
                If (isposter = "poster" Or frodo Or isseasonall = "poster") And doPoster Then 'poster
                    Dim mainposter As String = Nothing
                    For Each lang In Langlist
                        For Each Image In artlist
                            If (Image.Language = lang Or lang = "") AndAlso Image.BannerType = "poster" Then
                                mainposter = Image.Url
                                Exit For
                            End If
                        Next
                        If Not IsNothing(mainposter) Then Exit For
                    Next
                    If Not IsNothing(mainposter) Then
                        Dim imgpaths As New List(Of String)
                        If frodo Then imgpaths.Add(currentshowpath & "poster.jpg")
                        If eden AndAlso isposter = "poster" Then imgpaths.Add(currentshowpath & "folder.jpg")
                        If frodo AndAlso isseasonall <> "none" Then imgpaths.Add(currentshowpath & "season-all-poster.jpg")
                        If eden AndAlso isseasonall = "poster" Then imgpaths.Add(currentshowpath & "season-all.tbn")
                        success = DownloadCache.SaveImageToCacheAndPaths(mainposter, imgpaths, False, , , overwriteimage)
                        Dim popath As String = Utilities.save2postercache(currentshow.NfoFilePath, currentshow.ImagePoster.Path, WallPicWidth, WallPicHeight)
                        updateTvPosterWall(popath, currentshow.NfoFilePath)
                    End If
                End If

                'Main Banner
                If (isposter = "banner" Or frodo Or isseasonall = "wide") And doPoster Then 'banner
                    Dim mainbanner As String = Nothing
                    For Each lang In Langlist
                        For Each Image In artlist
                            If (Image.Language = lang Or lang = "") AndAlso Image.BannerType = "series" AndAlso Image.Season = Nothing Then
                                mainbanner = Image.Url
                                Exit For
                            End If
                        Next
                        If Not IsNothing(mainbanner) Then Exit For
                    Next
                    If Not IsNothing(mainbanner) Then
                        Dim imgpaths As New List(Of String)
                        If frodo then imgpaths.Add(currentshowpath & "banner.jpg")
                        If eden AndAlso isposter = "banner" Then imgpaths.Add(currentshowpath & "folder.jpg")
                        If frodo AndAlso isseasonall <> "none" Then imgpaths.Add(currentshowpath & "season-all-banner.jpg")
                        If eden AndAlso isseasonall = "wide" Then imgpaths.Add(currentshowpath & "season-all.tbn")
                        success = DownloadCache.SaveImageToCacheAndPaths(mainbanner, imgpaths, False, , , overwriteimage)
                    End If
                End If
            End If

            'Dim shSeason As Boolean =True
            If shSeason Then
                'SeasonXX Poster
                For f = 0 To MaxSeasonNo + 1
                    If (isposter = "poster" Or frodo) And doSeason Then 'poster
                        Dim seasonXXposter As String = Nothing
                        For Each lang In Langlist
                            For Each Image In artlist
                                If Image.Season = f.ToString AndAlso (Image.Language = lang Or lang = "") Then
                                    seasonXXposter = Image.Url
                                    Exit For
                                End If
                            Next
                            If Not IsNothing(seasonXXposter) Then Exit For
                        Next
                        If Not IsNothing(seasonXXposter) Then
                            Dim tempstring As String = ""
                            If f < 10 Then
                                tempstring = "0" & f.ToString
                            Else
                                tempstring = f.ToString
                            End If
                            If tempstring = "00" Then tempstring = "-specials"
                            Dim imgpaths As New List(Of String)
                            If frodo Then imgpaths.Add(currentshowpath & "season" & tempstring & "-poster.jpg")
                            If eden Then imgpaths.Add(currentshowpath & "season" & tempstring & ".tbn")
                            success = DownloadCache.SaveImageToCacheAndPaths(seasonXXposter, imgpaths, False, , , overwriteimage)
                        End If
                    End If

                    'SeasonXX Banner
                    If (isposter = "banner" Or frodo) And doSeason Then 'banner
                        Dim seasonXXbanner As String = Nothing
                        For Each lang In Langlist
                            For Each Image In artlist
                                If Image.Season = f.ToString AndAlso (Image.Language = lang Or lang = "") AndAlso Image.Resolution = "seasonwide" Then
                                    seasonXXbanner = Image.Url
                                    Exit For
                                End If
                            Next
                            If Not IsNothing(seasonXXbanner) Then Exit For
                        Next
                        If seasonXXbanner <> "" Then
                            Dim tempstring As String = ""
                            If f < 10 Then
                                tempstring = "0" & f.ToString
                            Else
                                tempstring = f.ToString
                            End If
                            If tempstring = "00" Then tempstring = "-specials"
                            Dim imgpaths As New List(Of String)
                            If frodo Then imgpaths.Add(currentshowpath & "season" & tempstring & "-banner.jpg")
                            If eden Then imgpaths.Add(currentshowpath & "season" & tempstring & ".tbn")
                            success = DownloadCache.SaveImageToCacheAndPaths(seasonXXbanner, imgpaths, False, , , overwriteimage)
                        End If
                    End If
                Next
                TvCheckfolderjpgart(currentshow)
            End If

            'Main Fanart
            If shFanart AndAlso doFanart Then
                Dim fanartposter As String = Nothing
                For Each lang In Langlist
                    For Each Image In artlist
                        If (Image.Language = lang Or lang = "") AndAlso Image.BannerType = "fanart" Then
                            fanartposter = Image.Url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(fanartposter) Then Exit For
                Next
                If Not IsNothing(fanartposter) Then
                    Dim imgpaths As New List(Of String)
                    imgpaths.Add(currentshowpath & "fanart.jpg")
                    If frodo And isseasonall <> "none" Then imgpaths.Add(currentshowpath & "season-all-fanart.jpg")
                    success = DownloadCache.SaveImageToCacheAndPaths(fanartposter, imgpaths, False, , , overwriteimage)
                End If
            End If

            'ExtraFanart
            If shXtraFanart Then
                Dim xfanart As String = currentshowpath & "extrafanart\"
                Dim fanartposter As New List(Of TVBanners)
                For Each lang In Langlist
                    For Each Image In artlist
                        If (Image.Language = lang Or lang = "") AndAlso Image.BannerType = "fanart" Then
                            fanartposter.Add(Image)
                        End If
                    Next
                Next
                If fanartposter.Count > 0 Then
                    Dim x As Integer = 0
                    Do Until x = Pref.TvXtraFanartQty
                        If x = fanartposter.Count Then Exit Do
                        success = Utilities.DownloadFile(fanartposter(x).url, (xfanart & fanartposter(x).id & ".jpg"))
                        x = x + 1
                    Loop
                End If
            End If
        Catch
        End Try
        Return success

    End Function

    Private Function TvGetActorTvdb(ByRef NewShow As Media_Companion.TvShow) As Boolean
        Dim tvdbstuff As New TVDBScraper
        Dim TvdbActors As List(Of str_MovieActors) = tvdbstuff.GetActors(NewShow.TvdbId.Value)
        Return TvGetActorTvdb(TvdbActors, NewShow)
    End Function

    Private Function TvGetActorTvdb(ByVal TvdbActors As List(Of str_MovieActors), ByRef NewShow As Media_Companion.TvShow) As Boolean
        Dim success As Boolean = True
        Dim workingpath As String = ""
        Dim tempstring As String = ""
        If Pref.actorseasy AndAlso Not Pref.tvshowautoquick Then
            workingpath = NewShow.NfoFilePath.Replace(Path.GetFileName(NewShow.NfoFilePath), "") & ".actors\"
            Utilities.EnsureFolderExists(workingpath)
        End If
        For Each NewAct In TvdbActors
            If Pref.ExcludeActorNoThumb AndAlso String.IsNullOrEmpty(newact.actorthumb) Then Continue For
            Dim id As String = If(NewAct.ActorId = Nothing, "", NewAct.ActorId)
            Dim results As XmlNode = Nothing
            Dim filename As String = Utilities.cleanFilenameIllegalChars(NewAct.actorname)
            filename = filename.Replace(" ", "_")
            If Not String.IsNullOrEmpty(NewAct.actorthumb) And NewAct.actorthumb <> "http://thetvdb.com/banners/" Then

                'Save to .actor folder
                If NewAct.actorthumb <> "" And Pref.actorseasy = True And Pref.tvshowautoquick = False Then
                    If NewShow.TvShowActorSource.Value <> "imdb" Or NewShow.ImdbId = Nothing Then
                        Dim ActorFilename As String = Path.Combine(workingpath, filename)
                        Dim actorpaths As New List(Of String)
                        If Pref.FrodoEnabled Then actorpaths.Add(ActorFilename & ".jpg")
                        If Pref.EdenEnabled Then actorpaths.Add(ActorFilename & ".tbn")
                        Dim cachename As String = Utilities.Download2Cache(NewAct.actorthumb)
                        If cachename <> "" Then
                            For Each p In actorpaths
                                Utilities.SafeCopyFile(cachename, p, Pref.overwritethumbs)
                            Next
                        End If
                    End If
                End If

                'Save to Local actor folder
                If Pref.actorsave = True And id <> "" Then 'Allow Local folder save, separate from .actor folder saving 
                    Dim workingpath2 As String = ""
                    Dim networkpath As String = Pref.actorsavepath
                    filename = filename & "_" & id
                    tempstring = networkpath & "\" & filename.Substring(0, 1) & "\"

                    Dim aok As Boolean = Utilities.EnsureFolderExists(tempstring)
                    workingpath2 = tempstring & filename
                    Dim actorpaths As New List(Of String)
                    If Pref.FrodoEnabled Then actorpaths.Add(workingpath2 & ".jpg")
                    If Pref.EdenEnabled Then actorpaths.Add(workingpath2 & ".tbn")
                    Dim cachename As String = Utilities.Download2Cache(NewAct.actorthumb)
                    If aok AndAlso cachename <> "" Then
                        For Each p In actorpaths
                            Utilities.SafeCopyFile(cachename, p, Pref.overwritethumbs)
                        Next
                    End If
                    NewAct.actorthumb = actorpaths(0)
                    If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                        NewAct.actorthumb = actorpaths(0).Replace(networkpath, Pref.actornetworkpath).Replace("\", "/")
                    ElseIf Pref.actornetworkpath.IndexOf("\") <> -1
                        NewAct.actorthumb = actorpaths(0).Replace(networkpath, Pref.actornetworkpath).Replace("/", "\")
                    End If
                End If
            End If
            Dim exists As Boolean = False
            NewShow.ListActors.Add(NewAct)
        Next
        Return success
    End Function

    Private Function TvGetActorImdb(ByRef NewShow As Media_Companion.TvShow) As Boolean
        Dim imdbscraper As New Classimdb
        Dim success As Boolean = False
        If String.IsNullOrEmpty(NewShow.ImdbId.Value) Then Return success
        Dim actorlist As List(Of str_MovieActors) = imdbscraper.GetImdbActorsList(Pref.imdbmirror, NewShow.ImdbId.Value, Pref.maxactors)
        Dim workingpath As String = ""
        If Pref.actorseasy And Not Pref.tvshowautoquick Then
            workingpath = NewShow.NfoFilePath.Replace(Path.GetFileName(NewShow.NfoFilePath), "")
            workingpath = workingpath & ".actors\"
            Utilities.EnsureFolderExists(workingpath)
        End If

        Dim listofactors As List(Of str_MovieActors) = IMDbActors(actorlist, success, workingpath)

        Dim i As Integer = 0
        For each listact In listofactors
            i += 1
            NewShow.ListActors.Add(listact)
            If i > Pref.maxactors Then Exit For
        Next
        Return success
    End Function

    Private Function IMDbActors(ByVal actorlist As List(Of str_MovieActors), ByRef success As Boolean, ByVal workingpath As String) As List(Of str_MovieActors)
        Dim actcount As Integer = 0
        Dim totalactors As New List(Of str_MovieActors)
        For Each thisresult As str_MovieActors In actorlist
            If Pref.ExcludeActorNoThumb AndAlso String.IsNullOrEmpty(thisresult.actorthumb) Then Continue For
            If Not String.IsNullOrEmpty(thisresult.actorthumb) AndAlso Not String.IsNullOrEmpty(thisresult.actorid) AndAlso actcount < (Pref.maxactors + 1) Then
                If Pref.actorseasy And Not Pref.tvshowautoquick Then
                    Dim actorpaths As New List(Of String)
                    Dim filename As String = Utilities.cleanFilenameIllegalChars(thisresult.actorname)
                    filename = filename.Replace(" ", "_")
                    filename = Path.Combine(workingpath, filename)
                    If Pref.FrodoEnabled Then actorpaths.Add(filename & ".jpg")
                    If Pref.EdenEnabled Then actorpaths.Add(filename & ".tbn")
                    Dim cachename As String = Utilities.Download2Cache(thisresult.actorthumb)
                    If cachename <> "" Then
                        For Each p In actorpaths
                            Utilities.SafeCopyFile(cachename, p, Pref.overwritethumbs)
                        Next
                    End If
                End If
                If Pref.actorsave = True AndAlso Pref.tvshowautoquick = False Then
                    Dim tempstring As String = Pref.actorsavepath
                    Dim workingpath2 As String = ""
                    If Pref.actorsavealpha Then
                        Dim actorfilename As String = thisresult.actorname.Replace(" ", "_") & "_" & If(Pref.LocalActorSaveNoId, "", thisresult.actorid) ' & ".tbn"
                        tempstring = tempstring & "\" & actorfilename.Substring(0, 1) & "\"
                        workingpath2 = tempstring & actorfilename
                    Else
                        tempstring = tempstring & "\" & thisresult.actorid.Substring(thisresult.actorid.Length - 2, 2) & "\"
                        workingpath2 = tempstring & thisresult.actorid ' & ".tbn"
                    End If
                    Dim actorpaths As New List(Of String)
                    If Pref.FrodoEnabled Then actorpaths.Add(workingpath2 & ".jpg")
                    If Pref.EdenEnabled Then actorpaths.Add(workingpath2 & ".tbn")
                    Utilities.EnsureFolderExists(tempstring)
                    Dim cachename As String = Utilities.Download2Cache(thisresult.actorthumb)
                    If cachename <> "" Then
                        For Each p In actorpaths
                            Utilities.SafeCopyFile(cachename, p, Pref.overwritethumbs)
                        Next
                    End If
                    If Not String.IsNullOrEmpty(Pref.actornetworkpath) Then
                        If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                            thisresult.actorthumb = actorpaths(0).Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("\", "/")
                        Else
                            thisresult.actorthumb = actorpaths(0).Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("/", "\")
                        End If
                    End If
                End If
            End If
            totalactors.Add(thisresult)
            success = True
            actcount += 1
        Next
        Return totalactors
    End Function
    
    'Private Function EpGetActorImdb(ByRef NewEpisode As TvEpisode) As Boolean
    '    Dim imdbscraper As New Classimdb
    '    Dim success As Boolean = False
    '    If String.IsNullOrEmpty(NewEpisode.ImdbId.Value) Then Return success
    '    Dim actorlist As List(Of str_MovieActors) = imdbscraper.GetImdbActorsList(Pref.imdbmirror, NewEpisode.ImdbId.Value, Pref.maxactors)
    '    Dim workingpath As String = ""
    '    If Pref.actorseasy And Not Pref.tvshowautoquick Then
    '        workingpath = NewEpisode.ShowObj.FolderPath
    '        workingpath = workingpath & ".actors\"
    '        Utilities.EnsureFolderExists(workingpath)
    '    End If

    '    Dim listofactors As List(Of str_MovieActors) = IMDbActors(actorlist, success, workingpath)
    '    If Not success Then Return success

    '    NewEpisode.ListActors.Clear()
    '    Dim i As Integer = 0
    '    For each listact In listofactors
    '        i += 1
    '        NewEpisode.ListActors.Add(listact)
    '        If i > Pref.maxactors Then Exit For
    '    Next
    '    Return success
    'End Function

    'Private Sub FixTvActorsNfo(ByRef TvSeries As TvShow)
    '    'If XBMC networkpath changed, update actor thumb path
    '    For Each tvActor In TvSeries.listactors
    '        If Pref.actorsave AndAlso tvActor.actorid <> "" Then
    '            If Not String.IsNullOrEmpty(Pref.actorsavepath) Then
    '                Dim tempstring As String = Pref.actorsavepath
    '                Dim workingpath As String = ""
    '                If Pref.actorsavealpha Then
    '                    Dim actorfilename As String = tvActor.actorname.Replace(" ", "_") & "_" & If(Pref.LocalActorSaveNoId, "", tvActor.actorid) & ".jpg"
    '                    tempstring = tempstring & "\" & actorfilename.Substring(0, 1) & "\"
    '                    workingpath = tempstring & actorfilename
    '                Else
    '                    tempstring = tempstring & "\" & tvActor.actorid.Substring(tvActor.actorid.Length - 2, 2) & "\"
    '                    workingpath = tempstring & tvActor.actorid & ".jpg"
    '                End If
    '                If Not String.IsNullOrEmpty(Pref.actornetworkpath) Then
    '                    If Pref.actornetworkpath.IndexOf("/") <> -1 Then
    '                        tvActor.actorthumb = workingpath.Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("\", "/")
    '                    Else
    '                        tvActor.actorthumb = workingpath.Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("/", "\")
    '                    End If
    '                End If
    '            End If
    '        End If
    '    Next
    'End Sub

    Sub TvDeleteShowArt(ByVal NewShow As Media_Companion.TvShow, Optional ByVal NotActors As Boolean = True)
        Try
            Dim workingpath As String = NewShow.FolderPath

            If NotActors AndAlso Directory.Exists(workingpath & ".actors") Then Directory.Delete(workingpath & ".actors", True)

            If Directory.Exists(workingpath & "extrafanart") Then Directory.Delete(workingpath & "extrafanart", True)

            Dim artextn() As String = "*.jpg:*.tbn:*.png".Split(":")
            For Each arttype In artextn
                For Each filepath In Directory.GetFiles(workingpath, arttype, IO.SearchOption.TopDirectoryOnly)
                    File.Delete(filepath)
                Next
            Next

            Dim seasonfolderpath As String = ""
            For Each ep As Media_Companion.TvEpisode In NewShow.Episodes
                seasonfolderpath = ep.FolderPath
                If File.Exists(seasonfolderpath & "folder.jpg") Then Utilities.SafeDeleteFile(seasonfolderpath & "folder.jpg")
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TvDeleteEpisodeNfoAndArt(ByVal NewShow As Media_Companion.TvShow)
        Try
            For Each ep As Media_Companion.TvEpisode In NewShow.Episodes
                Dim eppath As String = ep.NfoFilePath.Replace(".nfo", "")
                If File.Exists(eppath & "-fanart.jpg") Then Utilities.SafeDeleteFile(eppath & "-fanart.jpg")
                If File.Exists(eppath & ".tbn") Then Utilities.SafeDeleteFile(eppath & ".tbn")
                If File.Exists(ep.FolderPath & "folder.jpg") Then Utilities.SafeDeleteFile(ep.FolderPath & "folder.jpg")
                If File.Exists(ep.NfoFilePath) Then Utilities.SafeDeleteFile(ep.NfoFilePath)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Public Sub tv_MissingArtDownload(ByVal BrokenShow As TvShow)
        Dim messbox As New frmMessageBox("Attempting to download art", "", "       Please Wait")
        messbox.Show()
        messbox.Refresh()
        Application.DoEvents()
        Try
            If Pref.TvFanartTvFirst Then
                If Pref.TvDlFanartTvArt Then TvFanartTvArt(BrokenShow, False)
                TvGetArtwork(BrokenShow, True, True, True, Pref.dlTVxtrafanart, force:=False)
            Else
                TvGetArtwork(BrokenShow, True, True, True, Pref.dlTVxtrafanart, force:=False)
                If Pref.TvDlFanartTvArt Then TvFanartTvArt(BrokenShow, False)
            End If
            If Pref.tvfolderjpg OrElse Pref.seasonfolderjpg Then
                TvCheckfolderjpgart(BrokenShow)
            End If
        Catch
        End Try
        Call tv_ShowLoad(BrokenShow)
        messbox.Close()

    End Sub

    Private Sub TvCheckfolderjpgart(ByVal ThisShow As TvShow)
        Dim currentshowpath As String = ThisShow.FolderPath
        If Pref.tvfolderjpg AndAlso Not File.Exists(currentshowpath & "folder.jpg") Then
            If File.Exists(currentshowpath & "poster.jpg") AndAlso Pref.FrodoEnabled Then
                Utilities.SafeCopyFile(currentshowpath & "poster.jpg", currentshowpath & "folder.jpg", False)
            End If
        End If
        Dim I = 1
        If Pref.seasonfolderjpg Then
            For Each Seas In ThisShow.Seasons.Values
                If Seas.FolderPath <> ThisShow.FolderPath Then
                    Dim seasonfile As String = Seas.Poster.FileName   '= Seas.SeasonLabel.ToLower.Replace(" ", "")& "-poster.jpg"
                    If File.Exists(ThisShow.FolderPath & seasonfile) AndAlso Not File.Exists(Seas.FolderPath & "folder.jpg") Then
                        Utilities.SafeCopyFile(ThisShow.FolderPath & seasonfile, Seas.FolderPath & "folder.jpg", False)
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub TvEpThumbScreenShot(Optional ByVal delay As Integer = 0)
        Try
            Dim aok As Boolean = False
            Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
            If WorkingEpisode.IsMissing Then Exit Sub
            If delay = 0 Then delay = If(IsNumeric(TextBox35.Text), TextBox35.Text.ToInt, Pref.ScrShtDelay)
            If Not delay = 0 Then
                Dim paths As New List(Of String)
                If Pref.EdenEnabled Then paths.Add(WorkingEpisode.NfoFilePath.Replace(".nfo", ".tbn"))
                If Pref.FrodoEnabled Then paths.Add(WorkingEpisode.NfoFilePath.Replace(".nfo", "-thumb.jpg"))
                Dim messbox As frmMessageBox = New frmMessageBox("ffmpeg is working to capture the desired screenshot", "", "Please Wait")
                Dim tempstring2 As String = WorkingEpisode.VideoFilePath
                If File.Exists(tempstring2) Then
                    'Dim seconds As Integer = If(Convert.ToInt32(TextBox35.Text) > 0, Convert.ToInt32(TextBox35.Text), Pref.ScrShtDelay)
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    messbox.Show()
                    messbox.Refresh()
                    Application.DoEvents()
                    Dim cachepathandfilename As String = Utilities.CreateScrnShotToCache(tempstring2, paths(0), delay)
                    If cachepathandfilename <> "" Then
                        aok = True
                        Dim imagearr() As Integer = GetAspect(WorkingEpisode)
                        If Pref.tvscrnshtTVDBResize AndAlso Not imagearr(0) = 0 Then
                            DownloadCache.CopyAndDownSizeImage(cachepathandfilename, paths(0), imagearr(0), imagearr(1))
                        Else
                            File.Copy(cachepathandfilename, paths(0), True)
                        End If

                        If paths.Count > 1 Then File.Copy(paths(0), paths(1), True)

                        If File.Exists(paths(0)) Then
                            util_ImageLoad(pbTvEpScrnShot, paths(0), Utilities.DefaultTvFanartPath)
                            util_ImageLoad(tv_PictureBoxLeft, paths(0), Utilities.DefaultTvFanartPath)
                            Dim Rating As String = tb_EpRating.Text  'WorkingEpisode.Rating.Value
                            If TestForMultiepisode(WorkingEpisode.NfoFilePath) Then
                                Dim episodelist As New List(Of TvEpisode)
                                episodelist = WorkingWithNfoFiles.ep_NfoLoad(WorkingEpisode.NfoFilePath)
                                For Each Ep In episodelist
                                    If Ep.Season.Value = WorkingEpisode.Season.Value And Ep.Episode.Value = WorkingEpisode.Episode.value Then
                                        Dim video_flags = GetMultiEpMediaFlags(ep)
                                        movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, Rating, video_flags)
                                    End If
                                Next
                            Else
                                Dim video_flags = GetEpMediaFlags()
                                movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, Rating, video_flags)
                            End If
                        End If
                    End If
                End If
                messbox.Close()
                If Not aok Then MsgBox("Could not create ScreenShot")
            Else
                MsgBox("Please enter a numerical value into the textbox")
                tb_MovSortOrder.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TvEpThumbRescrape()
        Try
            Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)

            Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
            If WorkingEpisode.IsMissing Then Exit Sub
            Dim messbox As frmMessageBox = New frmMessageBox("Checking TVDB for screenshot", "", "Please Wait")
            Dim episodescraper As New TVDBScraper
            Dim id As String = WorkingTvShow.TvdbId.Value
            Dim sortorder As String = WorkingTvShow.SortOrder.Value
            Dim seasonno As String = WorkingEpisode.Season.Value
            Dim episodeno As String = WorkingEpisode.Episode.Value
            Dim language As String = WorkingTvShow.Language.Value
            Dim eden As Boolean = Pref.EdenEnabled
            Dim frodo As Boolean = Pref.FrodoEnabled
            If language = Nothing Then language = "en"
            If language = "" Then language = "en"
            If String.IsNullOrEmpty(sortorder) Then sortorder = "default"

            If String.IsNullOrEmpty(id) Then
                MsgBox("No ID is available for this show")
                Exit Sub
            End If
            If String.IsNullOrEmpty(episodeno) Then
                MsgBox("No Episode Number is available for this show")
                Exit Sub
            End If
            If String.IsNullOrEmpty(seasonno) Then
                MsgBox("No Season Number is available for this show")
                Exit Sub
            End If
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            messbox.Refresh()
            Application.DoEvents()
            Dim tempepisode As String = episodescraper.getepisode(WorkingTvShow.TvdbId.Value, sortorder, seasonno, episodeno, language, True)
            Dim thumburl As String = ""
            messbox.Close()
            Dim scrapedepisode As New XmlDocument
            scrapedepisode.LoadXml(tempepisode)
            Try
                For Each thisresult As XmlNode In scrapedepisode("episodedetails")
                    Select Case thisresult.Name
                        Case "thumb"
                            thumburl = thisresult.InnerText
                            Exit For
                    End Select
                Next
            Catch
            End Try
            If thumburl = "" Then
                Dim tvdbstuff As New TVDBScraper
                Dim tmpep As Tvdb.Episode = tvdbstuff.getepisodefromxml(WorkingTvShow.TvdbId.Value, sortorder, seasonno, episodeno, language, True)
                If tmpep.ThumbNail.Value <> Nothing Then thumburl = tmpep.ThumbNail.Value
            End If
            Try
                If thumburl <> "" And thumburl.ToLower <> "http://www.thetvdb.com/banners/" Then
                    messbox = New frmMessageBox("Screenshot found, downloading now", "", "Please Wait")
                    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    messbox.Show()
                    messbox.Refresh()
                    Application.DoEvents()
                    Dim tempstring As String = WorkingEpisode.VideoFilePath.Replace(Path.GetExtension(WorkingEpisode.VideoFilePath), ".tbn")
                    Dim cachename As String = Utilities.Download2Cache(thumburl)
                    If cachename <> "" Then
                        If eden Then Utilities.SafeCopyFile(cachename, tempstring, True)
                        If frodo Then
                            tempstring = tempstring.Replace(".tbn", "-thumb.jpg")
                            Utilities.SafeCopyFile(cachename, tempstring, True)
                        End If

                        util_ImageLoad(pbTvEpScrnShot, tempstring, Utilities.DefaultTvFanartPath)
                        util_ImageLoad(tv_PictureBoxLeft, tempstring, Utilities.DefaultTvFanartPath)

                        Dim Rating As String = tb_EpRating.Text  'WorkingEpisode.Rating.Value
                        If TestForMultiepisode(WorkingEpisode.NfoFilePath) Then
                            Dim episodelist As New List(Of TvEpisode)
                            episodelist = WorkingWithNfoFiles.ep_NfoLoad(WorkingEpisode.NfoFilePath)
                            For Each Ep In episodelist
                                If Ep.Season.Value = seasonno And Ep.Episode.Value = episodeno Then
                                    Dim video_flags = GetMultiEpMediaFlags(ep)
                                    movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, Rating, video_flags)
                                    Exit For
                                End If
                            Next
                        Else
                            Dim video_flags = GetEpMediaFlags()
                            movieGraphicInfo.OverlayInfo(tv_PictureBoxLeft, Rating, video_flags)
                        End If
                        messbox.Close()
                    Else
                        messbox.Close()
                        MsgBox("Unable To Download Image")
                    End If
                Else
                    MsgBox("No Episode Screenshot Found On TVDB")
                End If
            Catch ex As Exception
#If SilentErrorScream Then
                Throw ex
#End If
            Finally
                messbox.Close()
            End Try
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TvScrapePosterBanner(ByVal postertype As String)
        Try
            Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
            Dim messbox As frmMessageBox = New frmMessageBox("Searching TVDB for " & If(postertype = "poster", "Poster", "Banner"), "", "Please Wait")
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            messbox.Refresh()
            Application.DoEvents()
            Dim id As String = WorkingTvShow.TvdbId.Value
            Dim mainimages As Boolean = TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvShow
            Dim posterpath As String = ""
            Dim seasonno As String = ""
            Dim seasonpath As String = ""
            If Not mainimages Then
                seasonno = tv_SeasonSelectedCurrently(TvTreeview).ToString
                seasonno = seasonno.ToLower.Replace("season ", "")
                Dim tmp As Integer = seasonno.ToInt
                seasonno = tmp.ToString
                If Pref.seasonfolderjpg Then
                    For Each ep As TvEpisode In WorkingTvShow.Episodes
                        If ep.Season.Value = seasonno Then
                            seasonpath = ep.FolderPath.Replace(WorkingTvShow.FolderPath, "")
                            Exit For
                        End If
                    Next
                End If
            End If
            Dim language As String = WorkingTvShow.Language.Value
            Dim eden As Boolean = Pref.EdenEnabled
            Dim frodo As Boolean = Pref.FrodoEnabled
            If String.IsNullOrEmpty(language) Then language = "en"
            Dim tvdbstuff As New TVDBScraper
            Dim artlist As New List(Of TvBanners)
            artlist.Clear()
            Dim thumblist As String = tvdbstuff.GetPosterList(id, artlist)

            If artlist.Count = 0 Then
                messbox.Close()
                If thumblist <> "ok" Then
                    MsgBox("failed to retrieve any artwork")
                Else
                    MsgBox("No " & If(postertype = "poster", "Poster", "Banner") & " found")
                End If
                Exit Sub
            End If

            If mainimages Then
                For Each Image In artlist
                    If Image.Language = Pref.TvdbLanguageCode And Image.BannerType = postertype And Image.Season = Nothing Then
                        posterpath = Image.Url
                        Exit For
                    End If
                Next
                If posterpath = "" Then
                    For Each Image In artlist
                        If Image.Language = "en" AndAlso Image.BannerType = postertype AndAlso Image.Season = Nothing Then posterpath = Image.Url : Exit For
                    Next
                End If
                If posterpath = "" Then
                    For Each Image In artlist
                        If Image.BannerType = postertype AndAlso Image.Season = Nothing Then posterpath = Image.Url : Exit For
                    Next
                End If
            Else
                Dim istype As String = ""
                If postertype = "poster" Then
                    istype = "season"
                Else
                    istype = "seasonwide"
                End If
                For Each Image In artlist
                    If Image.Season = seasonno And Image.Language = Pref.TvdbLanguageCode And Image.Resolution = istype Then posterpath = Image.Url : Exit For
                Next
                If posterpath = "" Then
                    For Each Image In artlist
                        If Image.Season = seasonno And Image.Language = "en" And Image.Resolution = istype Then posterpath = Image.Url : Exit For
                    Next
                End If
                If posterpath = "" Then
                    For Each Image In artlist
                        If Image.Season = seasonno And Image.Resolution = istype Then posterpath = Image.Url : Exit For
                    Next
                End If
            End If
            If posterpath <> "" Then
                Dim imagepath As New List(Of String)
                If mainimages Then
                    If postertype = "poster" Then
                        If frodo Then imagepath.Add(WorkingTvShow.FolderPath & "poster.jpg")
                        If eden Then imagepath.Add(WorkingTvShow.FolderPath & "folder.jpg")
                    Else
                        If frodo Then imagepath.Add(WorkingTvShow.FolderPath & "banner.jpg")
                        If eden And Pref.postertype = "banner" Then imagepath.Add(WorkingTvShow.FolderPath & "folder.jpg")
                    End If
                Else
                    If seasonno.ToInt < 10 Then seasonno = "0" & seasonno
                    If postertype = "poster" Then
                        If seasonno <> "00" Then
                            If Pref.seasonfolderjpg AndAlso seasonpath <> "" Then imagepath.Add(WorkingTvShow.FolderPath & seasonpath & "folder.jpg")
                            If eden Then imagepath.Add(WorkingTvShow.FolderPath & "season" & seasonno & ".tbn")
                            If frodo Then imagepath.Add(WorkingTvShow.FolderPath & "season" & seasonno & "-poster.jpg")
                        Else
                            If Pref.seasonfolderjpg AndAlso seasonpath <> "" Then imagepath.Add(WorkingTvShow.FolderPath & seasonpath & "folder.jpg")
                            If eden Then imagepath.Add(WorkingTvShow.FolderPath & "season-specials" & ".tbn")
                            If frodo Then imagepath.Add(WorkingTvShow.FolderPath & "season-specials" & "-poster.jpg")
                        End If
                    Else
                        If eden And Pref.postertype = "banner" Then imagepath.Add(WorkingTvShow.FolderPath & "season" & seasonno & ".tbn")
                        If frodo Then imagepath.Add(WorkingTvShow.FolderPath & "season" & seasonno & "-banner.jpg")
                    End If
                End If
                For Each impath In imagepath
                    Utilities.DownloadFile(posterpath, impath)
                Next
                Dim last As Integer = imagepath.Count - 1
                If postertype = "poster" Then
                    util_ImageLoad(tv_PictureBoxRight, imagepath(last), Utilities.DefaultTvPosterPath)
                Else
                    util_ImageLoad(tv_PictureBoxBottom, imagepath(last), Utilities.DefaultTvBannerPath)
                End If
            Else
                messbox.Close()
                MsgBox("No " & If(postertype = "poster", "Poster", "Banner") & " found")
            End If

            messbox.Close()
        Catch ex As Exception
            messbox.Close()
        End Try
    End Sub

    Private Sub TvSelectPosterBanner(ByVal poster As Boolean)
        Dim IsOfType As String = "banner"
        Dim tpindex As Integer = 2
        If Not poster Then IsOfType = "poster"
        If TypeOf TvTreeview.SelectedNode.Tag Is Media_Companion.TvEpisode Then tpindex = 3
        RemoveHandler TabControl3.SelectedIndexChanged, AddressOf TabControl3_SelectedIndexChanged
        TabControl3.SelectTab(tpindex)
        AddHandler TabControl3.SelectedIndexChanged, AddressOf TabControl3_SelectedIndexChanged
        tv_PosterSetup(IsOfType)
    End Sub

    Private Sub TvFanartTvArt(ByVal ThisShow As TvShow, ByVal force As Boolean)
        Dim clearartLD As String = Nothing : Dim logoLD As String = Nothing : Dim clearart As String = Nothing : Dim logo As String = Nothing
        Dim poster As String = Nothing : Dim fanart As String = Nothing : Dim banner As String = Nothing : Dim landscape As String = Nothing
        Dim character As String = Nothing
        Dim currentshowpath As String = ThisShow.FolderPath
        Dim DestImg As String = ""
        Dim aok As Boolean = True
        Dim frodo As Boolean = Pref.FrodoEnabled
        Dim eden As Boolean = Pref.EdenEnabled
        Dim ID As String = ThisShow.TvdbId.Value
        Dim TvFanartlist As New FanartTvTvList
        Dim newobj As New FanartTv
        newobj.ID = ID
        newobj.src = "tv"
        Try
            TvFanartlist = newobj.FanarttvTvresults
        Catch ex As Exception
            aok = False
        End Try
        If Not aok Then Exit Sub
        Dim lang As New List(Of String)
        lang.Add(ThisShow.Language.Value)
        If Not lang.Contains(Pref.TvdbLanguageCode) Then lang.Add(Pref.TvdbLanguageCode)
        If Not lang.Contains("en") Then lang.Add("en")
        lang.Add("00")
        For Each lan In lang
            If IsNothing(clearart) Then
                For Each Art In TvFanartlist.hdclearart
                    If Art.lang = lan Then clearart = Art.url : Exit For
                Next
            End If
            If IsNothing(clearartLD) Then
                For Each Art In TvFanartlist.clearart
                    If Art.lang = lan Then clearartLD = Art.url : Exit For
                Next
            End If
            If IsNothing(logo) Then
                For Each Art In TvFanartlist.hdtvlogo
                    If Art.lang = lan Then logo = Art.url : Exit For
                Next
            End If
            If IsNothing(logoLD) Then
                For Each Art In TvFanartlist.clearlogo
                    If Art.lang = lan Then logoLD = Art.url : Exit For
                Next
            End If
            If IsNothing(poster) Then
                For Each Art In TvFanartlist.tvposter
                    If Art.lang = lan Then poster = Art.url : Exit For
                Next
            End If
            If IsNothing(fanart) Then
                For Each Art In TvFanartlist.showbackground
                    If Art.lang = lan Then fanart = Art.url : Exit For
                Next
            End If
            If IsNothing(banner) Then
                For Each Art In TvFanartlist.tvbanner
                    If Art.lang = lan Then banner = Art.url : Exit For
                Next
            End If
            If IsNothing(landscape) Then
                For Each Art In TvFanartlist.tvthumb
                    If Art.lang = lan Then landscape = Art.url : Exit For
                Next
            End If
            If IsNothing(character) Then
                For Each Art In TvFanartlist.characterart
                    If Art.lang = lan Then character = Art.url : Exit For
                Next
            End If
        Next
        If IsNothing(clearart) AndAlso Not IsNothing(clearartld) Then clearart = clearartLD
        If IsNothing(logo) AndAlso Not IsNothing(logold) Then logo = logold
        DestImg = currentshowpath & "clearart.png"
        If Not IsNothing(clearart) Then Utilities.DownloadFile(clearart, DestImg, force)
        DestImg = currentshowpath & "logo.png"
        If Not IsNothing(logo) Then Utilities.DownloadFile(logo, DestImg, force)
        DestImg = currentshowpath & "character.png"
        If Not IsNothing(character) Then Utilities.DownloadFile(character, DestImg, force)
        If Not IsNothing(poster) Then
            Dim destpaths As New List(Of String)
            If frodo Then
                destpaths.Add(currentshowpath & "poster.jpg")
                destpaths.Add(currentshowpath & "season-all-poster.jpg")
            End If
            If eden then
                destpaths.Add(currentshowpath & "poster.jpg")
                destpaths.Add(currentshowpath & "season-all.tbn")
            End If
            Dim success As Boolean = DownloadCache.SaveImageToCacheAndPaths(poster, destpaths, False, , , force)
        End If
        If Not IsNothing(fanart) Then
            Dim Destpaths As New List(Of String)
            If frodo Then
                Destpaths.Add(currentshowpath & "fanart.jpg")
                Destpaths.Add(currentshowpath & "season-all-fanart.jpg")
            End If
            If eden Then Destpaths.Add(currentshowpath & "fanart.jpg")
            Dim success As Boolean = DownloadCache.SaveImageToCacheAndPaths(fanart, Destpaths, False, , , force)
        End If
        DestImg = currentshowpath & "landscape.jpg"
        If Not IsNothing(landscape) Then Utilities.DownloadFile(landscape, DestImg, force)
        DestImg = currentshowpath & "banner.jpg"
        If Not IsNothing(banner) Then
            Utilities.DownloadFile(banner, DestImg, force)
            If frodo Then
                DestImg = currentshowpath & "season-all-banner.jpg"
                Utilities.DownloadFile(banner, DestImg, force)
            End If
        End If

        Dim firstseason As Integer = 1
        Dim lastseason As Integer = -1
        For Each item In TvFanartlist.seasonposter
            Dim itemseason As Integer = item.season.ToInt
            If itemseason > lastseason Then lastseason = itemseason
            If itemseason < firstseason Then firstseason = itemseason
        Next
        If lastseason >= firstseason Then
            For i = firstseason to lastseason
                Dim savepaths As New List(Of String)
                Dim seasonurl As String = Nothing
                For Each lan In lang
                    For Each item In TvFanartlist.seasonposter
                        If item.lang = lan AndAlso item.season = i.ToString Then
                            seasonurl = item.url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(seasonurl) Then Exit for
                Next
                If Not IsNothing(seasonurl) Then
                    Dim seasonno As String = i.ToString
                    If seasonno <> "" Then
                        If seasonno.Length = 1 Then seasonno = "0" & seasonno
                        If seasonno = "00" Then
                            seasonno = "-specials"
                        End If
                        destimg = currentshowpath & "season" & seasonno & "-poster.jpg"
                        If Pref.FrodoEnabled Then savepaths.Add(destimg)
                        If Pref.EdenEnabled Then
                            destimg = destimg.Replace("-poster.jpg", ".tbn")
                            savepaths.Add(destimg)
                        End If
                    End If
                End If
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(seasonurl, savepaths, False, , , force)
            Next
        End If

        firstseason = 1
        lastseason = -1
        For Each item In TvFanartlist.seasonbanner
            Dim itemseason As Integer = item.season.ToInt
            If itemseason > lastseason Then lastseason = itemseason
            If itemseason < firstseason Then firstseason = itemseason
        Next
        If lastseason >= firstseason AndAlso frodo Then
            For i = firstseason to lastseason
                Dim savepaths As New List(Of String)
                Dim seasonurl As String = Nothing
                For Each lan In lang
                    For Each item In TvFanartlist.seasonbanner
                        If item.lang = lan AndAlso item.season = i.ToString Then
                            seasonurl = item.url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(seasonurl) Then Exit for
                Next
                If Not IsNothing(seasonurl) Then
                    Dim seasonno As String = i.ToString
                    If seasonno <> "" Then
                        If seasonno.Length = 1 Then seasonno = "0" & seasonno
                        If seasonno = "00" Then
                            seasonno = "-specials"
                        End If
                        destimg = currentshowpath & "season" & seasonno & "-banner.jpg"
                        If Pref.FrodoEnabled Then savepaths.Add(destimg)
                    End If
                End If
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(seasonurl, savepaths, False, , , force)
            Next
        End If

        firstseason = 1
        lastseason = -1
        For Each item In TvFanartlist.seasonthumb
            Dim itemseason As Integer = item.season.ToInt
            If itemseason > lastseason Then lastseason = itemseason
            If itemseason < firstseason Then firstseason = itemseason
        Next
        If lastseason >= firstseason AndAlso frodo Then
            For i = firstseason to lastseason
                Dim savepaths As New List(Of String)
                Dim seasonurl As String = Nothing
                For Each lan In lang
                    For Each item In TvFanartlist.seasonthumb
                        If item.lang = lan AndAlso item.season = i.ToString Then
                            seasonurl = item.url
                            Exit For
                        End If
                    Next
                    If Not IsNothing(seasonurl) Then Exit for
                Next
                If Not IsNothing(seasonurl) Then
                    Dim seasonno As String = i.ToString
                    If seasonno <> "" Then
                        If seasonno.Length = 1 Then seasonno = "0" & seasonno
                        If seasonno = "00" Then
                            seasonno = "-specials"
                        End If
                        destimg = currentshowpath & "season" & seasonno & "-landscape.jpg"
                        If Pref.FrodoEnabled Then savepaths.Add(destimg)
                    End If
                End If
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(seasonurl, savepaths, False, , , force)
            Next
        End If
    End Sub

#End Region

    Public Function tv_Showremovedfromlist(ByRef lstbox As Listbox, ByVal tvcache As Boolean, Optional ByVal nofolder As List(Of String) = Nothing, Optional ByVal lstboxscan As Boolean = False) As Boolean
        Dim remfolder As New List(Of String)
        Dim notvshownfo As New List(Of String)
        Dim status As Boolean = False
        If IsNothing(nofolder) Then
            For Each item In Pref.tvFolders
                If Not Directory.Exists(item) Then
                    remfolder.Add(item)
                Else
                    Dim tvnfopath As String = item & "\tvshow.nfo"
                    If Not File.Exists(tvnfopath) Then notvshownfo.Add(item)
                End If
            Next
        End If
        If notvshownfo.Count > 0 Then
            Dim notvshmsg As String = "Folders found that did not contain a tvshow.nfo file" & vbCrLf & "Do you wish to also remove these folders?" & vbCrLf
            For Each fo In notvshownfo
                notvshmsg &= fo & vbcrlf
            Next
            Dim x = MsgBox(notvshmsg, MsgBoxStyle.YesNo)
            If x = MsgBoxResult.Yes Then
                remfolder.AddRange(notvshownfo)
            Else
                notvshownfo.Clear()
            End If
        End If
        If IsNothing(nofolder) AndAlso (IsNothing(remfolder) Or remfolder.Count <= 0) AndAlso notvshownfo.Count = 0 Then
            MsgBox("No Folders Missing or removed")
            Return False
        End If

        If IsNothing(nofolder) And Not IsNothing(remfolder) Then nofolder = remfolder
        If nofolder.Count > 0 Then
            For Each folder In nofolder
                If tvcache Then
                    If lstboxscan Then
                        For Each Item As Media_Companion.TvShow In Cache.TvCache.Shows
                            If Item.FolderPath.Trim("\") = folder Then
                                TvTreeview.Nodes.Remove(Item.ShowNode)
                                Cache.TvCache.Remove(Item)
                                Exit For
                            End If
                        Next
                        lstbox.Items.Remove(folder)
                        status = True
                    Else
                        tvFolders.Remove(folder)
                    End If
                Else
                    lstbox.Items.Remove(folder)
                    custtvFolders.Remove(folder)
                End If
            Next
        End If
        Return status
    End Function

#Region "Tv Watched/Unwatched Routines"

    Private Sub Tv_MarkAs_Watched_UnWatched(ByVal toggle As String)
        If TvTreeview.SelectedNode Is Nothing Then Exit Sub
        Dim WorkingTvShow As TvShow = tv_ShowSelectedCurrently(TvTreeview)
        Dim WorkingTvSeason As TvSeason = tv_SeasonSelectedCurrently(TvTreeview)
        Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(TvTreeview)
        If WorkingTvShow Is Nothing Then Exit Sub

        If Not IsNothing(WorkingEpisode) Then
            Dim episodelist As New List(Of TvEpisode)
            episodelist = WorkingWithNfoFiles.ep_NfoLoad(WorkingEpisode.NfoFilePath)
            For Each epis In episodelist
                If toggle = "3" Then
                    Dim tmpstr As Integer = If(String.IsNullOrEmpty(epis.PlayCount.Value), 0, epis.PlayCount.Value.ToInt)
                    If tmpstr > 0 Then
                        epis.PlayCount.Value = "0"
                    Else
                        epis.PlayCount.Value = "1"
                    End If
                    With Me.btn_EpWatched
                        .Text = If(tmpstr = 0, "Watched", "Unwatched")
                        .BackColor = If(tmpstr = 0, Color.LawnGreen, Color.Red)
                    End With
                Else
                    epis.PlayCount.Value = toggle
                End If
            Next
            WorkingWithNfoFiles.ep_NfoSave(episodelist, WorkingEpisode.NfoFilePath)
            WorkingEpisode.Load
            WorkingEpisode.UpdateTreenode()
            WorkingTvSeason.UpdateTreenode()
            WorkingTvShow.UpdateTreenode()
        ElseIf Not IsNothing(WorkingTvSeason) Then
            For Each ep In WorkingTvSeason.Episodes
                If ep.IsMissing Then Continue For
                Dim episodelist As New List(Of TvEpisode)
                episodelist = WorkingWithNfoFiles.ep_NfoLoad(ep.NfoFilePath)
                For Each epis In episodelist
                    epis.PlayCount.Value = toggle
                Next
                WorkingWithNfoFiles.ep_NfoSave(episodelist, ep.NfoFilePath)
                ep.Load
                ep.UpdateTreenode()
            Next
            WorkingTvSeason.UpdateTreenode()
            WorkingTvShow.UpdateTreenode()
        ElseIf Not IsNothing(WorkingTvShow) Then
            For Each ep In WorkingTvShow.Episodes
                If ep.IsMissing Then Continue For
                Dim episodelist As New List(Of TvEpisode)
                episodelist = WorkingWithNfoFiles.ep_NfoLoad(ep.NfoFilePath)
                For Each epis In episodelist
                    epis.PlayCount.Value = toggle
                Next
                WorkingWithNfoFiles.ep_NfoSave(episodelist, ep.NfoFilePath)
                ep.Load
                ep.UpdateTreenode()
            Next
            For Each seas In WorkingTvShow.Seasons.keys
                WorkingTvShow.Seasons(seas).UpdateTreenode()
            Next
        End If
        WorkingTvShow.UpdateTreenode()
    End Sub

    Public Shared Sub util_EpisodeSetWatched(ByRef playcount As String, Optional ByVal toggle As Boolean = False)
        Dim watched As Boolean = False
        If IsNumeric(playcount) Then
            watched = Convert.ToInt32(playcount) > 0
            If toggle Then
                watched = Not watched
                playcount = If(watched, "1", "0")
            End If
        End If
        With Form1.btn_EpWatched
            .Text = If(watched, "Watched", "Unwatched")
            .BackColor = If(watched, Color.LawnGreen, Color.Red)
        End With
    End Sub

#End Region

    'Private Function GetEpImdbId(ByVal ImdbId As String, ByVal SeasonNo As String, ByVal EpisodeNo As String) As String
    '    Dim url = "http://www.imdb.com/title/" & ImdbId & "/episodes?season=" & SeasonNo
    '    Dim webpage As New List(Of String)
    '    Dim s As New Classimdb
    '    webpage.Clear()
    '    webpage = s.loadwebpage(Pref.proxysettings, url, False, 10)
    '    Dim webPg As String = String.Join("", webpage.ToArray())
    '    Dim matchstring As String = "<strong><a href=""/title/tt"
    '    For f = 0 to webpage.Count - 1
    '        Dim m As Match = Regex.Match(webpage(f), matchstring)
    '        If m.Success AndAlso webpage(f).Contains("ttep_ep" & EpisodeNo) Then
    '            Dim tmp As String = webpage(f)
    '            Dim n As Match = Regex.Match(tmp, "(tt\d{7})")
    '            If n.Success = True Then
    '                url = n.Value
    '                Exit For
    '            End If
    '        End If
    '    Next
    '    Return url
    'End Function

    Private Function GetEpMediaFlags() As List(Of KeyValuePair(Of String, String))
        Dim thisep As TvEpisode = ep_SelectedCurrently(TvTreeview)
        Dim flags As New List(Of KeyValuePair(Of String, String))
        Try
            If thisep.StreamDetails.Audio.Count > 0 Then

                Dim tracks = If(Pref.ShowAllAudioTracks, thisep.StreamDetails.Audio, From x In thisep.StreamDetails.Audio Where x = thisep.StreamDetails.defaultAudioTrack)

                For Each track In tracks
                    flags.Add(New KeyValuePair(Of String, string)("channels" + GetNotDefaultStr(track = thisep.StreamDetails.defaultAudioTrack), GetNumAudioTracks(track.Channels.Value)))
                    flags.Add(New KeyValuePair(Of String, string)("audio" + GetNotDefaultStr(track = thisep.StreamDetails.defaultAudioTrack), track.Codec.Value))
                    flags.Add(New KeyValuePair(Of String, string)("lang" + GetNotDefaultStr(track = thisep.StreamDetails.defaultAudioTrack), track.Language.Value))
                Next
            Else
                flags.Add(New KeyValuePair(Of String, string)("channels", ""))
                flags.Add(New KeyValuePair(Of String, string)("audio", ""))
                flags.Add(New KeyValuePair(Of String, string)("lang", ""))
            End If

            flags.Add(New KeyValuePair(Of String, string)("aspect", Utilities.GetStdAspectRatio(thisep.StreamDetails.Video.Aspect.Value)))
            flags.Add(New KeyValuePair(Of String, string)("codec", Utilities.GetCodecCommonName(GetMasterCodec(thisep.StreamDetails.Video))))
            flags.Add(New KeyValuePair(Of String, string)("resolution", If(thisep.StreamDetails.Video.VideoResolution < 0, "", thisep.StreamDetails.Video.VideoResolution.ToString)))
            flags.Add(New KeyValuePair(Of String, String)("folderSize", "-1"))
            flags.Add(New KeyValuePair(Of String, String)("NumVideoBits", thisep.Streamdetails.Video.NumVideoBits))

            Dim subtitles = If(Not Pref.DisplayDefaultSubtitleLang, Nothing, If(Pref.DisplayAllSubtitleLang, thisep.StreamDetails.Subtitles, From x In thisep.StreamDetails.Subtitles Where x = thisep.StreamDetails.DefaultSubTrack))

            If Not IsNothing(subtitles) Then
                For each subtitle In subtitles
                    flags.Add(New KeyValuePair(Of String, String)("sublang", subtitle.Language.Value))
                Next
            End If
        Catch
        End Try
        Return flags

    End Function

    Private Function GetMultiEpMediaFlags(ByVal thisep As TvEpisode) As List(Of KeyValuePair(Of String, String))

        Dim flags As New List(Of KeyValuePair(Of String, String))
        Try
            If thisep.StreamDetails.Audio.Count > 0 Then
                Dim tracks = If(Pref.ShowAllAudioTracks, thisep.StreamDetails.Audio, From x In thisep.StreamDetails.Audio Where x = thisep.StreamDetails.defaultAudioTrack)

                For Each track In tracks
                    flags.Add(New KeyValuePair(Of String, string)("channels" + GetNotDefaultStr(track = thisep.StreamDetails.defaultAudioTrack), GetNumAudioTracks(track.Channels.Value)))
                    flags.Add(New KeyValuePair(Of String, string)("audio" + GetNotDefaultStr(track = thisep.StreamDetails.defaultAudioTrack), track.Codec.Value))
                    flags.Add(New KeyValuePair(Of String, string)("lang" + GetNotDefaultStr(track = thisep.StreamDetails.defaultAudioTrack), track.Language.Value))
                Next
            Else
                flags.Add(New KeyValuePair(Of String, string)("channels", ""))
                flags.Add(New KeyValuePair(Of String, string)("audio", ""))
                flags.Add(New KeyValuePair(Of String, string)("lang", ""))
            End If
            flags.Add(New KeyValuePair(Of String, string)("aspect", Utilities.GetStdAspectRatio(thisep.StreamDetails.Video.Aspect.Value)))
            flags.Add(New KeyValuePair(Of String, string)("codec", Utilities.GetCodecCommonName(GetMasterCodec(thisep.StreamDetails.Video))))  'thisep.StreamDetails.Video.Codec.Value.RemoveWhitespace))
            flags.Add(New KeyValuePair(Of String, string)("resolution", If(thisep.StreamDetails.Video.VideoResolution < 0, "", thisep.StreamDetails.Video.VideoResolution.ToString)))
            flags.Add(New KeyValuePair(Of String, String)("folderSize", "-1"))
            flags.Add(New KeyValuePair(Of String, String)("NumVideoBits", thisep.Streamdetails.Video.NumVideoBits))

            Dim subtitles = If(Not Pref.DisplayDefaultSubtitleLang, Nothing, If(Pref.DisplayAllSubtitleLang, thisep.StreamDetails.Subtitles, From x In thisep.StreamDetails.Subtitles Where x = thisep.StreamDetails.DefaultSubTrack))

            If Not IsNothing(subtitles) Then
                For each subtitle In subtitles
                    flags.Add(New KeyValuePair(Of String, String)("sublang", subtitle.Language.Value))
                Next
            End If
        Catch
        End Try
        Return flags
    End Function

    Private Function GetAspect(ep As TvEpisode)
        Dim thisarray(2) As Integer
        thisarray(0) = 400
        thisarray(1) = 225
        Try
            If ep.StreamDetails.Video.Width.Value is Nothing Then
                ep.GetFileDetails
            End If
            Dim epw As Integer = ep.StreamDetails.Video.Width.Value.ToInt
            Dim eph As Integer = ep.StreamDetails.Video.Height.Value.ToInt
            Dim ThisAsp As Double = epw / eph
            If ThisAsp < 1.37 Then  'aspect greater than Industry Standard of 1.37:1 is classed as WideScreen
                thisarray(1) = 300
            End If
        Catch
            thisarray(0) = 0
        End Try
        Return thisarray
    End Function

    Private Sub TestTvTMDB()
        Dim tvtmdb As New TVTMDb
        tvtmdb.TmdbId = "1622"
        'tvtmdb.TvdbId = "74205"
        'tvtmdb.ImdbId = "tt0185906"
        If Not IsNothing(tvtmdb.TvShow) Then
            Dim actors As List(Of str_MovieActors) = tvtmdb.cast
            Dim Something As String = Nothing
            If Not IsNothing(tvtmdb.Certification) Then
                Dim somethingelse As String = Nothing
            End If
        End If
    End Sub

End Class
