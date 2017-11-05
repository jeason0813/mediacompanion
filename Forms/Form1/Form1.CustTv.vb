Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions
Imports System.Text
Imports Media_Companion.WorkingWithNfoFiles
Imports System.Xml
Imports Media_Companion
Imports Media_Companion.Pref
Imports System.Linq

Partial Public Class Form1
    Dim CTvLoaded As Boolean = False

    Sub CustTvSearch()

    End Sub

    Sub CustTvRefresh()
        Custtv_CacheRefresh()
    End Sub

    Sub CustTvSave()

    End Sub
    
    Sub CustTvArtSetup()

    End Sub

#Region "Code Sections"


#Region "Folder Code"

    Sub CustTvFoldersSetup(Optional ByVal Refresh As Boolean = False)
        If (lbCFolders.Items.Count = 0 Or Refresh) AndAlso Pref.custtvFolders.Count > 0 Then
            lbCFolders.Items.Clear()
            For each fo In Pref.custtvFolders
                lbCFolders.Items.Add(fo)
            Next
        End If
    End Sub

#End Region

#End Region

#Region "CustomTv Controls"

#Region "Custom Browser Tab"

    Private Sub btnCSearch_Click(sender As Object, e As EventArgs) Handles btnCSearch.Click
        CustTvSearch()
    End Sub

    Private Sub btnCRefresh_Click(sender As Object, e As EventArgs) Handles btnCRefresh.Click
        CustTvRefresh()
    End Sub

    Private Sub pbSave_Click(sender As Object, e As EventArgs) Handles pbSave.Click
        CustTvSave()
    End Sub

    Private Sub CustPB_Zoom(sender As Object, e As EventArgs) Handles pb_Cust_Fanart.DoubleClick, pb_Cust_Poster.DoubleClick, pb_Cust_Banner.DoubleClick
        Try
            Dim picBox As PictureBox = sender
            Dim imageLocation As String = picBox.tag
            If imageLocation <> Nothing Then
                If File.Exists(imageLocation) Then
                    Me.ControlBox = False
                    MenuStrip1.Enabled = False
                    Call util_ZoomImage(imageLocation)
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

#End Region

#Region "Custom Tv Artwork"



#End Region

#Region "Custom Folder Tab"
    
    Private Sub TpCustTvFolders_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TpCustTvFolders.Leave
        Try
            If customTvfolderschanged Then
                Dim save = MsgBox("You have made changes to some folders" & vbCrLf & "    Do you wish to save these changes?", MsgBoxStyle.YesNo)
                If save = DialogResult.Yes Then
                    btnCFolderSave.PerformClick()
                Else
                    CustTvFoldersSetup(True)
                End If
                customTvfolderschanged = False
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnCFolderAdd_Click(sender As Object, e As EventArgs) Handles btnCFolderAdd.Click
        Try
            Dim allok As Boolean = True
            Dim thefoldernames As String
            fb.Description = "Select Folder for Custom TV Root Folder."
            fb.ShowNewFolderButton = True
            fb.RootFolder = System.Environment.SpecialFolder.Desktop
            fb.SelectedPath = Pref.lastpath
            Tmr.Start()
            If fb.ShowDialog = Windows.Forms.DialogResult.OK Then
                thefoldernames = (fb.SelectedPath)
                Pref.lastpath = thefoldernames
                For each fo In lbCFolders.Items
                    If fo = thefoldernames Then
                        allok = False
                        Exit For
                    End If
                Next
                If allok = True Then
                    lbCFolders.Items.Add(thefoldernames)
                    lbCFolders.Refresh()
                    customTvfolderschanged = True
                Else
                    MsgBox("        Folder Already Exists")
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnCFolderRemove_Click(sender As Object, e As EventArgs) Handles btnCFolderRemove.Click
        Try
            While lbCFolders.SelectedItems.Count > 0
                lbCFolders.Items.Remove(lbCFolders.SelectedItems(0))
            End While
            customTvfolderschanged = True
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnCFolderSave_Click(sender As Object, e As EventArgs) Handles btnCFolderSave.Click
        Pref.custtvFolders.Clear()
        For each fo In lbCFolders.Items
            Pref.custtvFolders.Add(fo)
        Next
        Pref.ConfigSave()
        customTvfolderschanged = False
        CustTvSearch()       ' Also updates cache if any custom shows exist.
        CustTvTabControl.SelectedIndex = 0
    End Sub

    Private Sub lbCFolders_KeyPress(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lbCFolders.KeyDown
        If e.KeyCode = Keys.Delete AndAlso lbCFolders.SelectedItem <> Nothing
            Call btnCFolderRemove.PerformClick()
        End If
    End Sub

    Private Sub lbCFolders_DragEnter(sender As Object, e As DragEventArgs) Handles lbCFolders.DragEnter
        Try
            e.Effect = DragDropEffects.Copy
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub lbCFolders_DragDrop(sender As Object, e As DragEventArgs) Handles lbCFolders.DragDrop
        Dim folders() As String
        droppedItems.Clear()
        folders = e.Data.GetData(DataFormats.filedrop)
        For f = 0 To UBound(folders)
            Dim exists As Boolean = False
            For Each rtpath In Pref.custtvFolders
                If rtpath = folders(f) Then
                    exists = True
                    Exit For
                End If
            Next
            If exists Then Continue For
            If lbCFolders.Items.Contains(folders(f)) Then Continue For
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
        For Each item In droppedItems
            lbCFolders.Items.Add(item)
            customTvfolderschanged = True
        Next
        lbCFolders.Refresh()
    End Sub

#End Region
    
#End Region
    
    Private Sub CustTvTabControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustTvTabControl.SelectedIndexChanged
        Try
            Dim Show As Media_Companion.TvShow = tv_ShowSelectedCurrently(CTvTreeView)
            Dim WorkingEpisode As TvEpisode = ep_SelectedCurrently(CTvTreeView)
            Dim tab As String = CustTvTabControl.SelectedTab.Name
            'If Show Is Nothing AndAlso tab <> TpCustTvFolders.Name Then
            '    MsgBox("No TV Show is selected")
            '    Exit Sub
            'End If
            
            If tab <> TpCustTvBrowser.Name AndAlso tab <> TpCustTvArt.Name And tab <> TpCustTvFolders.Name Then
                If Show.NfoFilePath = "" And custtvFolders.Count = 0 Then
                    Me.CustTvTabControl.SelectedIndex = tvCurrentTabIndex
                    MsgBox("There are no Shows in your list to work on" & vbCrLf & "Add a Custom Folder using the Folders Tab")
                ElseIf Show.NfoFilePath = "" And tvFolders.Count > 0 Then
                    Me.CustTvTabControl.SelectedIndex = CustTvIndex
                    If Cache.CustTvCache.Shows.Count > 0 Then
                        MsgBox("No Show is selected")
                        Exit Sub
                    Else
                        MsgBox("There are no Shows in your list to work on")
                        Exit Sub
                    End If
                End If
            ElseIf tab = TpCustTvArt.Name Then
                If Not Show Is Nothing Then
                    Call CustTvArtSetup()
                End If
            ElseIf tab= TpCustTvFolders.Name Then
                CustTvIndex = CustTvTabControl.SelectedIndex
                CustTvTabControl.SelectedIndex = CustTvIndex
                Call CustTvFoldersSetup()
            Else
                CustTvIndex = 0
                Exit Sub
            End If
           
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

#Region "CustTvCache Routines"

    Public Sub Custtv_CacheLoad()
        Cache.CustTvCache.TvCachePath = Pref.workingProfile.CustomTvCache
        Cache.CustTvCache.Load()
        CTvTreeView.BeginUpdate()
        Try
            CTvTreeView.Nodes.Clear()              'clear the treeview of old data
            For Each TvShow As Media_Companion.TvShow In Cache.CustTvCache.Shows
                CTvTreeView.Nodes.Add(TvShow.ShowNode)
                TvShow.UpdateTreenode()
            Next
        Finally
            CTvTreeView.EndUpdate()
        End Try
        tbCShowCount.Text = Cache.CustTvCache.Shows.Count
        tbCEpCount  .Text = Cache.CustTvCache.Episodes.Count
    End Sub

    Public Function CustTv_CacheSave() As Boolean

        Cache.CustTvCache.TvCachePath = Pref.workingProfile.CustomTvCache
        Cache.CustTvCache.Save()
        Return False
    End Function

    Private Sub CustTv_RefreshCacheSave(ByVal tvshowlist As List(Of TvShow), ByVal episodeelist As List(Of TvEpisode))
        Dim custtvcachepath As String = Pref.workingProfile.CustomTvCache
        Dim doc As New XmlDocument
        Dim root As XmlElement
        Dim child As XmlElement
        Dim xmlproc As XmlDeclaration
        xmlproc =doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)
        root =doc.CreateElement("customtvcache")
        root.SetAttribute("ver", "3.5")
        For Each item In tvshowlist
            child =doc.CreateElement("tvshow")
            child.SetAttribute("NfoPath", item.NfoFilePath)
            child.AppendChild(doc   , "state"           , item.State)
            child.AppendChild(doc   , "title"           , item.Title.Value)
            child.AppendChild(doc   , "id"              , item.TvdbId.Value)
            child.AppendChild(doc   , "status"          , item.Status.Value)
            child.AppendChild(doc   , "plot"            , item.Plot.Value)
            child.AppendChild(doc   , "language"        , item.Language.Value)
            child.AppendChild(doc   , "playcount"       , item.Playcount.Value)
            child.AppendChild(doc   , "hidden"          , item.Hidden.Value)
            root.AppendChild(child)
        Next

        For Each item In episodeelist
            child =doc.CreateElement("episodedetails")
            child.SetAttribute("NfoPath", item.NfoFilePath)
            child.AppendChild(doc   , "title"           , item.Title.Value)
            child.AppendChild(doc   , "season"          , item.Season.Value)
            child.AppendChild(doc   , "episode"         , item.Episode.Value)
            child.AppendChild(doc   , "aired"           , item.Aired.Value)
            child.AppendChild(doc   , "showid"          , item.ShowId.Value)
            child.AppendChild(doc   , "epextn"          , item.EpExtn.Value)
            child.AppendChild(doc   , "playcount"       , item.PlayCount.Value)
            root.AppendChild(child)
        Next
        doc.AppendChild(root)

        WorkingWithNfoFiles.SaveXMLDoc(doc, custtvcachepath)
    End Sub

    Private Sub Custtv_CacheRefresh(Optional ByVal TvShowSelected As TvShow = Nothing) 'refresh = clear & recreate cache from nfo's
        frmSplash2.Text = "Refresh Custom Shows..."
        frmSplash2.Label1.Text = "Searching Custom Folders....."
        frmSplash2.Label1.Visible = True
        frmSplash2.Label2.Visible = True
        frmSplash2.ProgressBar1.Visible = True
        frmSplash2.ProgressBar1.Maximum = Pref.custtvFolders.Count ' - 1
        frmSplash2.Show()
        Application.DoEvents()

        Dim fulltvshowlist As New List(Of TvShow)
        Dim fullepisodelist As New List(Of TvEpisode)

        'tv_RefreshLog("Starting TV Show Refresh" & vbCrLf & vbCrLf, , True)
        tbCShowCount.Text = ""
        tbCEpCount  .Text = ""
        Me.Enabled = False

        Dim nofolder As New List(Of String)
        Dim prgCount As Integer = 0
        Dim FolderList As New List(Of String)

        If TvShowSelected IsNot Nothing Then ' if we have provided a tv show, then add just this show to the list, else scan through all of the folders
            FolderList.Add(TvShowSelected.FolderPath) 'add the single show to our list
            Cache.CustTvCache.Remove(TvShowSelected)
            For Each episode In TvShowSelected.Episodes
                Cache.CustTvCache.Remove(episode)
            Next
            For Each showitem In Cache.CustTvCache.Shows
                fulltvshowlist.Add(showitem)
            Next
            For Each episodeitem In Cache.CustTvCache.Episodes
                fullepisodelist.Add(episodeitem)
            Next
        Else
            FolderList = Pref.custtvFolders ' add all folders to list to scan
            Cache.CustTvCache.Clear() 'Full rescan means clear all old data
            CTvTreeView.Nodes.Clear()
            'realTvPaths.Clear()
        End If

        For Each tvfolder In FolderList
            frmSplash2.Label2.Text = "(" & prgCount + 1 & "/" & Pref.custtvFolders.Count & ") " & tvfolder
            frmSplash2.ProgressBar1.Value = prgCount
            If Not Directory.Exists(tvfolder) OrElse Not File.Exists(tvfolder & "\tvshow.nfo") Then 
                nofolder.Add(tvfolder)
                Continue For
            End If
            prgCount += 1
            Application.DoEvents()
            Dim newtvshownfo As New TvShow
            newtvshownfo.NfoFilePath = Path.Combine(tvfolder, "tvshow.nfo")
            newtvshownfo.Load() 
            fulltvshowlist.Add(newtvshownfo)
            Dim episodelist As New List(Of TvEpisode)
            episodelist = loadepisodes(newtvshownfo, episodelist)
            For Each ep In episodelist
                ep.ShowId.Value = newtvshownfo.TvdbId.Value
                fullepisodelist.Add(ep)
            Next
        Next

        If nofolder.Count > 0 Then
            Dim mymsg As String
            mymsg = (nofolder.Count).ToString + " folder/s missing or no tvshow.nfo:" + vbCrLf + vbCrLf
            For Each item In nofolder
                mymsg = mymsg + item + vbCrLf
            Next
            mymsg = mymsg + vbCrLf + "Do you wish to remove these folders" + vbCrLf + "from your list of TV Folders?" + vbCrLf
            If MsgBox(mymsg, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                tv_Showremovedfromlist(lbCFolders, False, nofolder)
            End If
        End If

        frmSplash2.Label2.Visible = False

        frmSplash2.Label1.Text = "Saving Cache..."
        Windows.Forms.Application.DoEvents()
        CustTv_RefreshCacheSave(fulltvshowlist, fullepisodelist)    'save the cache file

        frmSplash2.Label1.Text = "Loading Cache..."
        Windows.Forms.Application.DoEvents()
        custtv_CacheLoad()    'reload the cache file to update the treeview
        Me.Enabled = True
        tbCShowCount.Text = Cache.CustTvCache.Shows.Count
        tbCEpCount  .Text = Cache.CustTvCache.Episodes.Count
        frmSplash2.Hide()
        'tv_Filter()
        BlinkTaskBar()
    End Sub

#End Region

    Public Function CustID() As String
        Dim datenow As DateTime = Date.Now()
        Dim idint As Integer = (((datenow.Year + datenow.Month + datenow.Day)+100)*1000) +((datenow.Hour + datenow.Minute + datenow.Second)+100)
        Return "MC" & idint.ToString
    End Function

End Class