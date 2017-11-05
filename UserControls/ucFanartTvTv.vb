Imports System.Linq
Imports Alphaleonis.Win32.Filesystem

Public Class ucFanartTvTv
    Dim WithEvents artposterpicboxes As PictureBox
    Dim WithEvents artcheckboxes As RadioButton
    Dim WithEvents tvposterlabels As Label
    Dim WithEvents tvreslabel As Label
    Dim nodata As Boolean = False
    Public Dim Form1MainFormLoadedStatus As Boolean = False
    Dim isroot As Boolean
    Dim artheight As Integer = 37
    Dim artwidth As Integer = 200
    Dim artType As String = ""
    Dim selectedimageurl As String = Nothing
    Dim FanarttvTvlist As New FanartTvTvList 
    Dim exmsg As String = Nothing
    Public messbox As New frmMessageBox("blank", "", "")
    Dim usedlist As New List(Of str_fanarttvart)
    Public WorkingShow As New TvShow
    Dim MovfieldNames = GetType(FanarttvMovielist).GetFields().[Select](Function(field) field.Name).ToList()
    Public tvFriendlyname() As String = {"HiDef ClearArt", "HiDef Tv Logo",  "Clear Art", "Clear Logo",  "Tv Poster", "Tv Thumb", 
                                         "Tv Banner", "Season Poster", "Season Banner", "Season Thumb", "Show Background", "Character Art"}


    Public Sub ucFanartTv_Refresh(ByVal ThisTvShow As TvShow)
        nodata = False
        If WorkingShow.Title.Value <> ThisTvShow.Title.Value Then
            WorkingShow = ThisTvShow
        Else
            Exit Sub
        End If
        Dim ID As String = ""
        pbexists.Image = Nothing
        lblftvgroups.Items.clear
        PanelClear()
        Me.lblTitle.Text = " Tv Show :-  " & WorkingShow.Title.value
        ID = WorkingShow.TvdbId.Value
        If String.IsNullOrEmpty(ID) Then
            Call noID
        End If
        If nodata Then Exit Sub
        GetFanartTvArt(ID)
        If Not ConfirmIfResults() Then Exit Sub
        artheightload()
    End Sub

    Public Sub GetFanartTvArt(ByVal ID As String)
        Try
            messbox = New frmMessageBox("Please wait,", "", "Gathering image data")
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            Me.Refresh()
            messbox.Refresh()
            Dim newobject As New FanartTv
            newobject.ID = ID
            newobject.src = "tv"
            FanarttvTvlist = newobject.FanarttvTvresults
        Catch ex As Exception
            exmsg = ex.Message 
        Finally
            messbox.Close()
        End Try
    End Sub

    Public Function ConfirmIfResults() As Boolean
        If Not exmsg = Nothing Then
            MsgBox(exmsg)
            exmsg = Nothing
            Return False
        End If
        If Not FanarttvTvlist.dataloaded Then
            MsgBox("Sorry, there are no results from Fanart.Tv" & vbCrLf & "for Series:  " & WorkingShow.title.Value)
            Return False
        End If
        Return True
    End Function

    Public Sub noID()
        MsgBox(" Selected Show contains no" & vbCrLf & "     IMDB or TVDB ID" & vbCrLf & "Unable to get Fanart.TV Data") 
        nodata = True
    End Sub
        
    Sub artheightload()
        Dim Tabsgl As String = vbTab
        Dim Tabdbl As String = vbTab & vbTab
        lblftvgroups.Items.Clear()
        lblftvgroups.Items.Add(tvFriendlyname(0)  & ":" & Tabsgl & "( " & FanarttvTvlist.hdclearart.count       & " )")
        lblftvgroups.Items.Add(tvFriendlyname(1)  & ":" & Tabsgl & "( " & FanarttvTvlist.hdtvlogo.count         & " )")
        lblftvgroups.Items.Add(tvFriendlyname(2)  & ":" & Tabdbl & "( " & FanarttvTvlist.clearart.count         & " )")
        lblftvgroups.Items.Add(tvFriendlyname(3)  & ":" & Tabsgl & "( " & FanarttvTvlist.clearlogo.count        & " )")
        lblftvgroups.Items.Add(tvFriendlyname(4)  & ":" & Tabsgl & "( " & FanarttvTvlist.tvposter.count         & " )")
        lblftvgroups.Items.Add(tvFriendlyname(5)  & ":" & Tabsgl & "( " & FanarttvTvlist.tvthumb.count          & " )")
        lblftvgroups.Items.Add(tvFriendlyname(6)  & ":" & Tabsgl & "( " & FanarttvTvlist.tvbanner.count         & " )")
        lblftvgroups.Items.Add(tvFriendlyname(7)  & ":" & Tabsgl & "( " & FanarttvTvlist.seasonposter.count     & " )")
        lblftvgroups.Items.Add(tvFriendlyname(8)  & ":" & Tabsgl & "( " & FanarttvTvlist.seasonbanner.count     & " )")
        lblftvgroups.Items.Add(tvFriendlyname(9)  & ":" & Tabsgl & "( " & FanarttvTvlist.seasonthumb.count      & " )")
        lblftvgroups.Items.Add(tvFriendlyname(10) & ":" & Tabsgl & "( " & FanarttvTvlist.showbackground.count   & " )")
        lblftvgroups.Items.Add(tvFriendlyname(11) & ":" & Tabsgl & "( " & FanarttvTvlist.characterart.count     & " )")
    End Sub

    Private Sub lblftvgroups_click(ByVal Sender As Object, e As EventArgs) Handles lblftvgroups.MouseDown 
        Dim indx As Integer = lblftvgroups.SelectedIndex
        Select Case indx
            Case "0"
                usedlist = FanarttvTvlist.hdclearart
                artheight = 112
                artType = "clearart.png"
            Case "1"
                usedlist = FanarttvTvlist.hdtvlogo
                artheight = 77
                artType = "logo.png"
            Case "2"
                usedlist = FanarttvTvlist.clearart  
                artheight = 112
                artType = "clearart.png"
            Case "3"
                usedlist = FanarttvTvlist.clearlogo
                artheight = 77
                artType = "logo.png"
            Case "4"
                usedlist = FanarttvTvlist.tvposter 
                artheight = 285
                artType = "poster.jpg"
            Case "5"
                usedlist = FanarttvTvlist.tvthumb 
                artheight = 112
                artType = "landscape.jpg"
            Case "6"
                usedlist = FanarttvTvlist.tvbanner 
                artheight = 37
                artType = "banner.jpg"
            Case "7"
                usedlist = FanarttvTvlist.seasonposter 
                artheight = 285
                artType = "season-poster.jpg"
            Case "8"
                usedlist = FanarttvTvlist.seasonbanner  
                artheight = 112
                artType = "season-banner.jpg"
            Case "9"
                usedlist = FanarttvTvlist.seasonthumb 
                artheight = 112
                artType = "season-landscape.jpg"
            Case "10"
                usedlist = FanarttvTvlist.showbackground  
                artheight = 112
                artType = "fanart.jpg"
            Case "11"
                usedlist = FanarttvTvlist.characterart  
                artheight = 200
                artType = "character.png"
        End Select
        If artType.Contains("season-") Then
            Label1.Text = "Exiting Season art not displayable"
        Else
            Label1.Text = "Existing Artwork"
        End If
        PanelPopulate()
    End Sub

    Private Sub PanelPopulate()
        Me.Panel1.Show()
        Call PanelSelectionDisplay()
    End Sub

    Private Sub PanelClear()
        For i = Panel1.Controls.Count - 1 To 0 Step -1
            Panel1.Controls.RemoveAt(i)
        Next
    End Sub

    Private Sub PanelSelectionDisplay()
        ''TV Image Preview sizes as follows:
        ''200  x  37     TV Banner
        ''200  x  77     HD Logo & Tv logo
        ''200  x  112    ShowBackground, HDClearArt & Clearart, Seasonthumb & Tv Thumb (landscape)
        ''200  x  200    CharacterArt
        ''200  x  285    Tv Poster & Season Poster

        PanelClear()
        If usedlist.Count = 0 Then
            lblnoart.Visible = True
            Exit Sub
        Else
            lblnoart.Visible = false
        End If
        DisplayExistingArt()
        Panel1.VerticalScroll.Visible = True 
        Dim picratio As Decimal = 1.25
        Dim locHeight = 5
        Dim colwidth As Integer = 20
        Dim colcount As Integer = 0
        Dim panelw As Integer = Panel1.Width
        Dim pbwidth As Integer = Math.Ceiling(artwidth * picratio)
        Dim pbheight As Integer = Math.Ceiling(artheight  * picratio)
        Dim imgchkbx As Integer = Math.Floor(pbwidth / 2)
        Dim colmax As Integer = Math.Floor(panelw/pbwidth)
        Dim xspace As Integer = Math.Floor((panelw - (pbwidth * colmax)) / (colmax+1))
        Dim xstart As Integer = xspace - 10
        Dim xlocation As Integer = xstart
        Dim ylocOffset = (locHeight + pbheight + 36)
        Dim itemcounter As Integer = 0
        Dim MovFanartPicBox As New List(Of FanartPicBox)
        For each item In usedlist
            Dim thispicbox As New FanartPicBox
            artposterpicboxes() = New PictureBox()
            With artposterpicboxes
                .Location = New Point(xlocation, locHeight)
                .Width = pbwidth 
                .Height = pbheight
                .BackColor = Color.Transparent 
                .SizeMode = PictureBoxSizeMode.Zoom
                .Tag = item.url                          'Full Image url
                .Visible = True
                .BorderStyle = BorderStyle.Fixed3D
                .Name = "poster" & itemcounter.ToString
                AddHandler artposterpicboxes.DoubleClick, AddressOf PosterDoubleClick
            End With
            thispicbox.pbox = artposterpicboxes
            thispicbox.imagepath = item.urlpreview
            MovFanartPicBox.Add(thispicbox)
            Application.DoEvents()
            
            artcheckboxes() = New RadioButton()
            With artcheckboxes
                .Location = New Point(xlocation + imgchkbx, locHeight + pbheight + 4)
                .Name = "imgcheckbox" & itemcounter.ToString
                .SendToBack()
                .Text = " "
                .Tag = item.url
                AddHandler artcheckboxes.CheckedChanged, AddressOf artPosterRadioChanged
            End With
            itemcounter += 1
            Me.Panel1.Controls.Add(artposterpicboxes())
            Me.Panel1.Controls.Add(artcheckboxes())
            Me.Refresh()
            Application.DoEvents()
            xlocation += xspace + pbwidth 
            colcount += 1
            If colcount = colmax Then
                xlocation = xstart
                colcount = 0
                locHeight += ylocOffset 
            End If
        Next
        Me.Panel1.Refresh()
        Me.Refresh()
        If MovFanartPicBox.Count > 0 Then
            messbox.Close()
            If Not Form1.ImgBw.IsBusy Then
                Form1.Imageloading = True
                Form1.ToolStripStatusLabel2.Text = "Starting Download of Images..."
                Form1.ToolStripStatusLabel2.Visible = True
                Form1.ImgBw.RunWorkerAsync({MovFanartPicBox, 0, MovFanartPicBox.Count, Me.Panel1})
            End If
        End If
        Me.Panel1.Refresh()
        Me.Refresh()
        Application.DoEvents()
        Me.Refresh()
        Button1.Visible = False
        EnableFanartScrolling()
        selectedimageurl = Nothing
    End Sub

    Private Sub artPosterRadioChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tempstring As String = sender.name
        Dim tempint As Integer = 0
        Dim tempstring2 As String = tempstring
        Dim allok As Boolean = False
        tempstring = tempstring.Replace("imgcheckbox", "")
        tempint = Convert.ToDecimal(tempstring)
        For each button as control in Panel1.controls
            If button.name.indexof("imgcheckbox") <> -1 Then
                Dim b1 As radiobutton = CType(button, radiobutton)
                If b1.Checked = True Then
                    allok = True
                    selectedimageurl = b1.tag
                    Exit For
                End if
            End if
        Next
        Button1.Visible = allok
    End Sub

    Private Sub PosterDoubleClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim tempstring As String = sender.name.replace("poster", "imgcheckbox")
        For Each Control In Panel1.Controls
            If Control.name = tempstring Then
                Dim rb As RadioButton = Control
                rb.Checked = True
            End If
        Next
        Dim messbox As New frmMessageBox("Please wait,", "", "Downloading Full Res Image")
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        messbox.Show()
        Me.Refresh()
        messbox.Refresh()
        Dim cachefile As String = Utilities.Download2Cache(sender.Tag.ToString)
        Form1.util_ZoomImage(cachefile)
        messbox.Close()
    End Sub

    Private Sub pbexists_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles pbexists.DoubleClick 
        If Not IsNothing(pbexists.Image) Then
            Dim loadpath As String = WorkingShow.FolderPath & artType
            Form1.util_ZoomImage(loadpath)
        End If
    End Sub

    Private Sub pbexists_MouseDown(sender As Object, e As MouseEventArgs) Handles pbexists.MouseDown
        If e.button = Windows.Forms.MouseButtons.Right Then
            If IsNothing(pbexists.Image) Then Exit Sub
            Dim tempint = MessageBox.show("Do you wish to delete this image from" & vbCrLf & "this Movie?", "Fanart.Tv Artwork Delete", MessageBoxButtons.YesNoCancel)
            If tempint = Windows.Forms.DialogResult.No or tempint = DialogResult.Cancel Then Exit Sub
            If tempint = Windows.Forms.DialogResult.Yes Then
                Dim loadpath As String = WorkingShow.FolderPath & artType
                pbexists.Image = Nothing
                Utilities.SafeDeleteFile(loadpath)
                Form1.TvPanel7Update(WorkingShow.FolderPath)
            End If
        End If
    End Sub

    Sub EnableFanartScrolling()
        Try
            Dim rb As RadioButton = Panel1.Controls("imgcheckbox0")

            rb.Select()                       'Causes RadioButtons checked state to toggle
            rb.Checked =  False  'Not rb.Checked     'Undo unwanted checked state toggling
        Catch
        End Try
    End Sub

    Private Sub DisplayExistingArt()
        Dim LoadPath As String = Nothing
        LoadPath = WorkingShow.FolderPath
        LoadPath &= artType
        If File.Exists(LoadPath) Then
            Form1.util_ImageLoad(pbexists, LoadPath, "")
        Else
            pbexists.Image = Nothing
        End If
    End Sub

    Private Sub Button1_Click( sender As Object,  e As EventArgs) Handles Button1.Click
        If nodata Then Exit Sub
        If Form1.ImgBw.IsBusy Then
            Form1.ImgBw.CancelAsync()
            Do Until Not Form1.imgbw.IsBusy
                Application.DoEvents()
            Loop
        End If
        If Not IsNothing(selectedimageurl) Then
            Dim savepaths As New List(Of String)
            Dim savepath As String = ""
            messbox = New frmMessageBox("Please wait,", "Downloading new image", "and saving")
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            Me.Refresh()
            messbox.Refresh()
            
            If artType.Contains("season") Then
                Dim seasonno As String = ""
                For Each item In usedlist
                    If selectedimageurl = item.url Then
                        seasonno = item.season
                        Exit For
                    End If
                Next
                If seasonno <> "" Then
                    Dim epseason As String = seasonno
                    If seasonno.Length = 1 Then seasonno = "0" & seasonno
                    If seasonno = "00" Then seasonno = "-specials"
                    savepath = WorkingShow.FolderPath & artType.Replace("season", "season" & seasonno)
                    If Pref.FrodoEnabled Then savepaths.Add(savepath)
                    If Pref.EdenEnabled AndAlso artType.contains("-poster.jpg") Then
                        savepath = savepath.Replace("-poster.jpg", ".tbn")
                        savepaths.Add(savepath)
                    End If
                    If Pref.seasonfolderjpg AndAlso WorkingShow.Episodes.Count > 0 Then
                        For Each ep In WorkingShow.Episodes
                            Dim TrueSeasonFolder As String = Nothing
                            Dim folder As Boolean = False
                            If ep.Season.Value = epseason Then
                                If ep.FolderPath <> WorkingShow.FolderPath Then
                                    TrueSeasonFolder = ep.FolderPath & "folder.jpg"
                                    If Not savepaths.Contains(TrueSeasonFolder) Then
                                        savepaths.Add(TrueSeasonFolder)
                                        folder = True
                                    End If
                                End If
                            End If
                            If folder Then Exit For
                        Next
                    End If
                End If
            Else
                If artType = "poster.jpg" Or artType = "fanart.jpg" or artType = "banner.jpg" Then
                    savepaths.Add(WorkingShow.FolderPath & artType)
                    If Pref.FrodoEnabled Then
                        savepaths.Add(WorkingShow.FolderPath & "season-all-" & artType)
                    End If
                    If Pref.EdenEnabled Then
                        If artType = "poster.jpg" Then
                            savepaths.Add(WorkingShow.FolderPath & "folder.jpg")
                            savepaths.Add(WorkingShow.FolderPath & "season-all.tbn")
                        End If
                    End If
                    If Pref.tvfolderjpg AndAlso artType = "poster.jpg" Then
                        savepaths.Add(WorkingShow.FolderPath & "folder.jpg")
                    End If
                End If
                savepaths.Add(WorkingShow.FolderPath & artType)
            End If
            Try
                Dim success As Boolean = False
                If savepaths.Count > 0 Then DownloadCache.SaveImageToCacheAndPaths(selectedimageurl, savepaths, False)
                DisplayExistingArt()
            Catch
            End Try
            messbox.Close()
            Form1.TvPanel7Update(WorkingShow.FolderPath)
        End If
    End Sub

    Private Sub panel_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel1.MouseWheel
        Try
            Dim mouseDelta As Integer = e.Delta / 120
            panel1.AutoScrollPosition = New Point(0, panel1.VerticalScroll.Value - (mouseDelta * 30))
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub panel_resize() Handles MyBase.resize
        If Not Form1MainFormLoadedStatus OrElse Form1.Imageloading Then Exit Sub
        PanelSelectionDisplay()
    End Sub

    
End Class
