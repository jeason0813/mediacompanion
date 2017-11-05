Imports System.Linq
Imports Alphaleonis.Win32.Filesystem
Imports System.Drawing
Imports Media_Companion


Public Class ucFanartTv
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
    Dim exmsg As String = Nothing
    Dim FanarttvMovielist As New FanartTvMovieList
    Public messbox As New frmMessageBox("blank", "", "")
    Dim usedlist As New List(Of str_fanarttvart)
    Public workingMovDetails As New FullMovieDetails
    Public MovSetToggle As Boolean = False
    Dim MovfieldNames = GetType(FanarttvMovielist).GetFields().[Select](Function(field) field.Name).ToList()
    Public movFriendlyname() As String = {"HiDef ClearArt", "HiDef Logo", "Movie Art", "Movie Logo", "Movie Poster", "Movie Fanart", 
                                          "Movie Disc", "Movie Banner", "Landscape"}


    Public Sub ucFanartTv_Refresh(ByVal moviedetails As FullMovieDetails)
        nodata = False
        isroot = Pref.GetRootFolderCheck(moviedetails.fileinfo.fullpathandfilename)
        If workingMovDetails.fullmoviebody.title <> moviedetails.fullmoviebody.title Then
            workingMovDetails = Form1.workingMovieDetails
            MovSetToggle = False
            lblftvArtMode.Text = "Displaying Movie Artwork"
            btnMovArtToggle.BackColor = System.Drawing.Color.Lime
            btnMovArtToggle.Text = "Show Movie Set Artwork"
        Else
            Exit Sub
        End If
        Dim ID As String = ""
        pbexists.Image = Nothing
        lblftvgroups.Items.clear
        PanelClear()
        Me.lblTitle.Text = workingMovDetails.fullmoviebody.title
        If workingMovDetails.fullmoviebody.tmdbid <> "" Then
            ID = workingMovDetails.fullmoviebody.tmdbid
        ElseIf workingMovDetails.fullmoviebody.imdbid.Contains("tt") Then
            ID = workingMovDetails.fullmoviebody.imdbid
        Else
            Call noID
        End If
        If isroot Then RootFolderMovie()
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
            newobject.src = "movie"
            FanarttvMovielist = newobject.FanarttvMovieresults

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
        If Not FanarttvMovielist.dataloaded Then
            If Not MovSetToggle Then
                MsgBox("Sorry, there are no results from Fanart.Tv" & vbCrLf & "for movie:  " & workingMovDetails.fullmoviebody.title)
            Else
                MsgBox("Sorry, there are no results from Fanart.Tv" & vbCrLf & "for MovieSet:  " & workingMovDetails.fullmoviebody.SetName)
            End If
            Return False
        End If
        Return True
    End Function

    Public Sub noID()
        MsgBox(" Selected Movie contains no" & vbCrLf & "     IMDB or TMDB ID" & vbCrLf & "Unable to get Fanart TV Data") 
        nodata = True
    End Sub

    Private Sub RootFolderMovie()
        MsgBox("  Selected Movie is in the Root Folder!" & vbCrLf & "Unable to save artwork from" & vbCrLf & _
               "Fanart.Tv, to the root folder" & vbCrLf & "Suggest rename movie to folder to be" & vbCrLf & _
               "compatible with ""Artwork Downloader""")
        nodata = True
    End Sub
    
    Sub artheightload()
        Dim Tabsgl As String = vbTab
        Dim Tabdbl As String = vbTab & vbTab
        lblftvgroups.Items.Clear()
        lblftvgroups.Items.Add(movFriendlyname(0) & ":" & Tabsgl & "( " & FanarttvMovielist.hdmovieclearart.count   & " )")
        lblftvgroups.Items.Add(movFriendlyname(1) & ":" & Tabsgl & "( " & FanarttvMovielist.hdmovielogo.count       & " )")
        lblftvgroups.Items.Add(movFriendlyname(2) & ":" & Tabsgl & "( " & FanarttvMovielist.movieart.count          & " )")
        lblftvgroups.Items.Add(movFriendlyname(3) & ":" & Tabsgl & "( " & FanarttvMovielist.movielogo.count         & " )")
        lblftvgroups.Items.Add(movFriendlyname(4) & ":" & Tabsgl & "( " & FanarttvMovielist.movieposter.count       & " )")
        lblftvgroups.Items.Add(movFriendlyname(5) & ":" & Tabsgl & "( " & FanarttvMovielist.moviebackground.count   & " )")
        lblftvgroups.Items.Add(movFriendlyname(6) & ":" & Tabsgl & "( " & FanarttvMovielist.moviedisc.count         & " )")
        lblftvgroups.Items.Add(movFriendlyname(7) & ":" & Tabsgl & "( " & FanarttvMovielist.moviebanner.count       & " )")
        lblftvgroups.Items.Add(movFriendlyname(8) & ":" & Tabsgl & "( " & FanarttvMovielist.moviethumb.count        & " )")
    End Sub

    Private Sub lblftvgroups_click(ByVal Sender As Object, e As EventArgs) Handles lblftvgroups.MouseDown 
        Dim indx As Integer = lblftvgroups.SelectedIndex
        Select Case indx
            Case "0"
                usedlist = FanarttvMovielist.hdmovieclearart
                artheight = 112
                artType = "clearart.png"
            Case "1"
                usedlist = FanarttvMovielist.hdmovielogo
                artheight = 77
                artType = "logo.png"
            Case "2"
                usedlist = FanarttvMovielist.movieart
                artheight = 112
                artType = "clearart.png"
            Case "3"
                usedlist = FanarttvMovielist.movielogo
                artheight = 77
                artType = "logo.png"
            Case "4"
                usedlist = FanarttvMovielist.movieposter
                artheight = 285
                artType = "poster.jpg"
            Case "5"
                usedlist = FanarttvMovielist.moviebackground
                artheight = 112
                artType = "fanart.jpg"
            Case "6"
                usedlist = FanarttvMovielist.moviedisc
                artheight = 200
                artType = "disc.png"
            Case "7"
                usedlist = FanarttvMovielist.moviebanner 
                artheight = 37
                artType = "banner.jpg"
            Case "8"
                usedlist = FanarttvMovielist.moviethumb
                artheight = 112
                artType = "landscape.jpg"
        End Select
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
        ''Movie Image Preview sizes as follows:
        ''200  x  37     Banner
        ''200  x  77     HDLogo & movielogo
        ''200  x  112    Background, HDClearArt & Clearart, and moviethumb
        ''200  x  200    moviedisc
        ''200  x  285    movieposter

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
        Application.DoEvents()
        Me.Panel1.Refresh()
        Me.Refresh()
        Button1.Visible = False
        EnableFanartScrolling()
        selectedimageurl = Nothing
    End Sub

    Private Sub artPosterRadioChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tempstring As String = sender.name
        Dim tempint As Integer = 0
        Dim tempstring2 As String = tempstring.ToInt
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
        LoadPath = Path.GetDirectoryName(workingMovDetails.fileinfo.fullpathandfilename) & "\"
        If MovSetToggle Then
            LoadPath = workingMovDetails.fileinfo.movsetposterpath.Replace("poster.jpg", "")
        ElseIf Pref.MovFanartNaming Then 
            LoadPath = workingMovDetails.fileinfo.fullpathandfilename.Replace(".nfo", "-")
        End If
        LoadPath &= artType
        If File.Exists(LoadPath) Then
            Form1.util_ImageLoad(pbexists, LoadPath, "")
        Else
            pbexists.Image = Nothing
        End If
    End Sub

    Private Sub pbexists_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles pbexists.DoubleClick 
        If Not IsNothing(pbexists.Image) Then
            Form1.util_ZoomImage(pbexists.Tag.ToString)
        End If
    End Sub

    Private Sub pbexists_MouseDown(sender As Object, e As MouseEventArgs) Handles pbexists.MouseDown
        If e.button = Windows.Forms.MouseButtons.Right Then
            If IsNothing(pbexists.Image) Then Exit Sub
            Dim tempint = MessageBox.show("Do you wish to delete this image from" & vbCrLf & "this Movie?", "Fanart.Tv Artwork Delete", MessageBoxButtons.YesNoCancel)
            If tempint = Windows.Forms.DialogResult.No or tempint = DialogResult.Cancel Then Exit Sub
            If tempint = Windows.Forms.DialogResult.Yes Then
                Dim loadpath As String = Path.GetDirectoryName(workingMovDetails.fileinfo.fullpathandfilename) & "\" & artType
                pbexists.Image = Nothing
                Utilities.SafeDeleteFile(loadpath)
                Form1.MovPanel6Update()
            End If
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
            messbox = New frmMessageBox("Please wait,", "Downloading new image", "and saving")
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            Me.Refresh()
            messbox.Refresh()
            Try
                Dim savepath As String = Path.GetDirectoryName(workingMovDetails.fileinfo.fullpathandfilename) & "\" & artType
                If MovSetToggle Then
                    savepath = workingMovDetails.fileinfo.movsetposterpath.Replace("poster.jpg", artType)
                Else
                    If Pref.MovFanartNaming Then savepath = workingMovDetails.fileinfo.fullpathandfilename.Replace(".nfo", "-" & artType)
                End If
                Dim success As Boolean = Utilities.DownloadImage(selectedimageurl, savepath)
                If Pref.posterjpg AndAlso Pref.createfolderjpg AndAlso artType.Contains("poster.jpg") Then
                    savepath = savepath.Replace(artType, "folder.jpg")
                    Dim asuccess As Boolean = Utilities.DownloadImage(selectedimageurl, savepath)
                End If
                DisplayExistingArt()
            Catch
            End Try
            Form1.MovPanel6Update()
            messbox.Close()
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

    Public Sub resetloadedart(Optional ByVal ID As String = "", Optional ByVal Title As String= "")
        pbexists.Image = Nothing
        lblftvgroups.Items.clear
        PanelClear()
        If ID <> "" Then
            Me.lblTitle.Text = Title
        Else
            Me.lblTitle.Text = workingMovDetails.fullmoviebody.title
            If workingMovDetails.fullmoviebody.imdbid.Contains("tt") Then
                ID = workingMovDetails.fullmoviebody.imdbid
            ElseIf workingMovDetails.fullmoviebody.tmdbid <> "" Then
                ID = workingMovDetails.fullmoviebody.tmdbid
            Else
                Call noID
            End If
        End If
        If nodata Then Exit Sub
        GetFanartTvArt(ID)
        If Not ConfirmIfResults() Then Exit Sub
        artheightload()
    End Sub

    Private Sub btnMovArtToggle_Click( sender As Object,  e As EventArgs) Handles btnMovArtToggle.Click
        If workingMovDetails.fullmoviebody.TmdbSetId = "" Then Exit Sub
        If MovSetToggle Then
            lblftvArtMode.Text = "Displaying Movie Artwork"
            btnMovArtToggle.BackColor = System.Drawing.Color.Lime
            btnMovArtToggle.Text = "Show Movie Set Artwork"
        Else
            lblftvArtMode.Text = "Showing MovieSet Artwork"
            btnMovArtToggle.BackColor = System.Drawing.Color.Aqua
            btnMovArtToggle.Text = "Show Movie Artwork"
        End If
        MovSetToggle = Not MovSetToggle
        If MovSetToggle Then
            resetloadedart(workingMovDetails.fullmoviebody.TmdbSetId, workingMovDetails.fullmoviebody.SetName)
        Else
            resetloadedart()
        End If
    End Sub

End Class
