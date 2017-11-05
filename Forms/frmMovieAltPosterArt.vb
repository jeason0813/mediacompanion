Imports System.Xml
Imports Alphaleonis.Win32.Filesystem


Public Class frmMovieAltPosterArt
    Dim bigpanel As Panel
    Dim WithEvents picboxes As PictureBox
    Dim WithEvents checkboxes As RadioButton
    Dim WithEvents labels As Label
    Dim WithEvents reslabel As Label
    Dim resolutionlbl As Label
    Dim panel2 As New Panel
    Dim posterpath As String
    Dim WithEvents mainposter As New PictureBox
    Dim WithEvents bigpicbox As PictureBox
    Dim count As Integer = 0
    Dim title As String = Form1.workingMovieDetails.fullmoviebody.title
    Dim itemnumber As Integer
    Dim rememberint As Integer
    Dim maxthumbs As Integer = 6 'Pref.maximumthumbs
    Dim posterArray As New List(Of McImage)
    Dim pagecount As Integer = 0
    Dim currentpage As Integer = 1
    Dim movieyear As String
    Dim folderjpgpath As String
    Dim imdbid As String = Form1.workingMovieDetails.fullmoviebody.imdbid
    Dim tmdbid As String = Form1.workingMovieDetails.fullmoviebody.tmdbid
    Dim movietitle As String = Form1.workingMovieDetails.fullmoviebody.title
    Dim fullpathandfilename As String = Form1.workingMovieDetails.fileinfo.fullpathandfilename
    Dim videotspath As String = Form1.workingMovieDetails.fileinfo.videotspath
    Dim applicationPath As String = Pref.applicationPath

    Private Sub frmCoverArt_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub coverart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            posterpath = Form1.workingMovieDetails.fileinfo.posterpath
            folderjpgpath = posterpath.Replace(Path.GetFileName(posterpath), "folder.jpg")
            If Form1.workingMovieDetails.fullmoviebody.year <> Nothing Then movieyear = Form1.workingMovieDetails.fullmoviebody.year
            TextBox1.Text = 6 'Pref.maximumthumbs.ToString
            btnSourceMPDB.Enabled = False
            
            Dim aok As Boolean = False
            If File.Exists(posterpath) = True Then
                Dim tempstring As String
                mainposter = New PictureBox
                Try
                    Dim ms As IO.MemoryStream = New IO.MemoryStream()
                    Using r As IO.Filestream = File.Open(posterpath, IO.FileMode.Open)
                        r.CopyTo(ms)
                    End Using
                    Dim OriginalImage As New Bitmap(ms)
                    Dim Image2 As New Bitmap(OriginalImage)
                    ms.Dispose()
                    OriginalImage.Dispose()

                    With mainposter
                        .Location = New Point(0, 0)
                        .Width = 250
                        .Height = 240
                        .SizeMode = PictureBoxSizeMode.Zoom
                        .Image = Image2
                        .Visible = True
                        .BorderStyle = BorderStyle.Fixed3D
                    End With
                    Me.Panel1.Controls.Add(mainposter)
                    tempstring = mainposter.Image.Width.ToString & " x " & mainposter.Image.Height.ToString
                    Label6.Text = tempstring
                    aok = True
                Catch
                End Try
            End If
            If Not aok Then
                mainposter = New PictureBox
                With mainposter
                    .Location = New Point(0, 0)
                    .Width = 250
                    .Height = 240
                    .SizeMode = PictureBoxSizeMode.Zoom
                    .Visible = False
                    .BorderStyle = BorderStyle.Fixed3D
                End With
                Me.Panel1.Controls.Add(mainposter)
                Dim mainlabel As Label
                mainlabel = New Label
                With mainlabel
                    .Location = New Point(0, 100)
                    .Width = 423
                    .Height = 100
                    .Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
                    .Text = "No Local Poster Is Available For This Movie"
                    .BringToFront()
                End With
                Me.Panel1.Controls.Add(mainlabel)
                Label6.Visible = False
            End If

            panel2 = New Panel
            With panel2
                .Width = 782
                .Height = 237
                .Location = New Point(2, 2)
                .AutoScroll = True
            End With
            Me.Controls.Add(panel2)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub radiochanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim tempstring As String = sender.name
            Dim tempint As Integer
            Dim tempstring2 As String = tempstring
            Dim allok As Boolean = False
            tempstring = tempstring.Replace("checkbox", "")
            tempint = Convert.ToDecimal(tempstring)
            For Each button As Control In Me.panel2.Controls
                If button.Name.IndexOf("checkbox") <> -1 Then
                    Dim b1 As RadioButton = CType(button, RadioButton)
                    If b1.Checked = True Then
                        allok = True
                        Exit For
                    End If
                End If
            Next
            If allok = True Then
                btnSaveSmall.Visible = True
                btnSaveBig.Visible = True
            Else
                btnSaveSmall.Visible = False
                btnSaveBig.Visible = False
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub zoomimage(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim tempstring As String = sender.name
            Dim tempint As Integer
            tempstring = tempstring.Replace("picture", "")
            tempint = (tempstring.ToInt) + ((currentpage - 1) * maxthumbs)
            rememberint = tempint

            bigpanel = New Panel
            With bigpanel
                .Width = Me.Width
                .Height = Me.Height
                .BringToFront()
                .Dock = DockStyle.Fill
            End With

            bigpicbox = New PictureBox()
            With bigpicbox
                .Location = New Point(0, 0)
                .Width = bigpanel.Width
                .Height = bigpanel.Height
                .SizeMode = PictureBoxSizeMode.Zoom
                .ImageLocation = posterArray(tempint).hdurl
                .Visible = True
                .BorderStyle = BorderStyle.Fixed3D
                AddHandler bigpicbox.DoubleClick, AddressOf closeimage
                .Dock = DockStyle.Fill
                .SendToBack()
            End With

            Me.Controls.Add(bigpanel)
            bigpanel.BringToFront()
            Me.bigpanel.Controls.Add(bigpicbox)
            Me.Refresh()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub imageres(ByVal sender As Object, ByVal e As EventArgs)
        Try
            reslabel = New Label
            Dim tempstring As String
            'tempstring = sender.image.width.ToString
            tempstring = sender.image.width.ToString & " x " & sender.image.height.ToString
            'tempstring = tempstring & sender.image.height.ToString
            Dim locx As Integer = sender.location.x
            Dim locy As Integer = sender.location.y
            locy = locy + sender.height
            With reslabel
                .Location = New Point(locx + 30, locy)
                .Text = tempstring
                .BringToFront()
            End With
            Me.panel2.Controls.Add(reslabel)
            Me.Refresh()
            Application.DoEvents()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub closeimage()
        Try
            Me.Controls.Remove(bigpanel)
            bigpanel = Nothing
            Me.Controls.Remove(bigpicbox)
            bigpicbox = Nothing
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnSourceTMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSourceTMDB.Click
        Try
            Call initialise()
            count = 0
            Dim aok As Boolean = True
            Try
            Dim tmdb As New TMDb '(tmdbid)
            tmdb.Imdb = If(imdbid.Contains("tt"), imdbid, "")
            tmdb.TmdbId = tmdbid
            For Each item In tmdb.McPosters
                posterArray.Add(item)
                count += 1
            Next
            Catch
                aok = False
            End Try
            Call displayselection(aok)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnSourceMPDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSourceMPDB.Click
        'Try
        '    If Not imdbid.Contains("tt") Then
        '        MsgBox("No IMDB ID" & vbCrLf & "Searching Movie Poster DB halted")
        '        Exit Sub
        '    End If
        '    Dim messbox = New frmMessageBox("Please wait,", "", "Scraping Movie Poster List")
        '    System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        '    messbox.Show()
        '    Me.Refresh()
        '    messbox.Refresh()
        '    Call initialise()
        '    Dim newobject As New class_mpdb_thumbs.Class1
        '    newobject.MCProxy = Utilities.MyProxy
        '    Dim testthumbs As String = String.Empty
        '    Try
        '        testthumbs = newobject.get_mpdb_thumbs(imdbid)
        '        testthumbs = "<totalthumbs>" & testthumbs & "</totalthumbs>"
        '    Catch ex As Exception
        '        'Thread.Sleep(1)
        '    End Try
        '    Dim thumbstring As New XmlDocument
        '    Dim thisresult As XMLNode
        '    Try
        '        thumbstring.LoadXml(testthumbs)
        '        count = 0
        '        For Each thisresult In thumbstring("totalthumbs")
        '            Select Case thisresult.Name
        '                Case "thumb"
        '                    Dim MCImg As New McImage
        '                    MCImg.hdUrl = thisresult.InnerText
        '                    MCImg.ldUrl = thisresult.InnerText
        '                    posterArray.Add(MCImg)
        '                    count += 1
        '            End Select
        '        Next
        '    Catch ex As Exception
        '    End Try
        '    messbox.Close()
        '    Call displayselection()
        'Catch ex As Exception
        '    ExceptionHandler.LogError(ex)
        'End Try
    End Sub

    Private Sub btnSourceIMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSourceIMDB.Click
        Try
            Call initialise()
            count = 0
            Dim aok As Boolean = True
            Try
            Dim newobject2 As New imdb_thumbs.Class1
            newobject2.MCProxy = Utilities.MyProxy
            Dim posterurls(,) As String
            posterurls = newobject2.getimdbposters(imdbid)
            count = UBound(posterurls)
            For I = 0 To count
                Dim MCImg As New McImage
                MCImg.hdUrl = posterurls(I,0)
                MCImg.ldUrl = posterurls(I,1)
                posterArray.Add(MCImg)
            Next
            Catch
                aok = False
            End Try
            Call displayselection(aok)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnSourceIMPA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSourceIMPA.Click
        Dim messbox = New frmMessageBox("Please wait,", "", "Scraping Movie Poster List")
        Try
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
            messbox.Show()
            Me.Refresh()
            messbox.Refresh()
            Call initialise()
            count = 0
            Dim aok As Boolean = True
            Try
            Dim newobject2 As New IMPA.getimpaposters
            newobject2.MCProxy = Utilities.MyProxy
                Dim title As String = Form1.CleanMovieTitle(movietitle)
                Dim posterurls(,) As String
                posterurls = newobject2.getimpaafulllist(title, movieyear)
                count = UBound(posterurls)
                For I = 0 To count
                    Dim MCImg As New McImage
                    MCImg.hdUrl = posterurls(I,0)
                    MCImg.ldUrl = posterurls(I,1)
                    posterArray.Add(MCImg)
                Next
            Catch
                aok = False
            End Try
            messbox.Close()
            messbox = Nothing
            Call displayselection(aok)
        Catch ex As Exception
            If Not IsNothing(messbox) Then messbox.close()
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub bigpicbox_LoadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles bigpicbox.LoadCompleted
        Try
            Dim bigpanellabel As Label
            bigpanellabel = New Label
            With bigpanellabel
                .Location = New Point(20, 200)
                .Width = 150
                .Height = 50
                .Visible = True
                .Text = "Double Click Image To" & vbCrLf & "Return To Browser"
            End With

            Me.bigpanel.Controls.Add(bigpanellabel)
            bigpanellabel.BringToFront()
            Application.DoEvents()

            If Not bigpicbox.Image Is Nothing And bigpicbox.Image.Width > 20 Then
                Dim sizey As Integer = bigpicbox.Image.Height
                Dim sizex As Integer = bigpicbox.Image.Width
                Dim tempstring As String
                tempstring = "Full Image Resolution :- " & sizex.ToString & " x " & sizey.ToString
                resolutionlbl = New Label
                With resolutionlbl
                    .Location = New Point(311, 450)
                    .Width = 180
                    .Text = tempstring
                    .BackColor = Color.Transparent
                End With

                Me.bigpanel.Controls.Add(resolutionlbl)
                resolutionlbl.BringToFront()
                Me.Refresh()
                Application.DoEvents()
                Dim tempstring2 As String = resolutionlbl.Text
            Else
                bigpicbox.ImageLocation = posterArray.Item(rememberint + 1).ldurl
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub mainposter_BackgroundImageChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mainposter.BackgroundImageChanged
        Try
            If Not mainposter.Image Is Nothing And mainposter.Image.Width > 20 Then
                Dim tempstring As String
                Label6.Visible = True
                tempstring = mainposter.Image.Width.ToString & " x " & mainposter.Image.Height.ToString
                Label6.Text = tempstring
            Else
                Label6.Visible = False
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        Try
            If Char.IsNumber(e.KeyChar) = False And e.KeyChar <> Chr(8) Then
                If TextBox1.Text <> "" Then
                    If Convert.ToDecimal(TextBox1.Text) >= 1 Then
                        e.Handled = True
                        maxthumbs = Convert.ToDecimal(TextBox1.Text)
                        'Pref.maximumthumbs = maxthumbs
                    Else
                        MsgBox("Please Enter A Number More Than 0")
                    End If
                Else
                    MsgBox("Please Enter A Number More Than 0")
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnScrollNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollNext.Click
        Try
            btnSaveSmall.Visible = False
            panelclear()
            currentpage += 1
            btnScrollPrev.Enabled = True
            btnScrollNext.Enabled = Not (currentpage = pagecount)
            displayselection()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnScrollPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScrollPrev.Click
        Try
            btnSaveSmall.Visible = False
            panelclear()
            currentpage -= 1
            btnScrollPrev.Enabled = True
            If currentpage <= 1 Then
                btnScrollPrev.Enabled = False
            Else
                btnScrollPrev.Enabled = True
            End If
            If currentpage = pagecount Then
                btnScrollNext.Enabled = False
            Else
                btnScrollNext.Enabled = True
            End If
            displayselection()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub

    Private Sub displayselection(Optional ByVal aok As Boolean = True)
        Dim names As New List(Of String)

        If aok AndAlso count > 0 Then
            If count > maxthumbs Then
                pagecount = 0
                Dim tempmaxthumbs As Integer = count

                Do Until tempmaxthumbs < 1
                    pagecount += 1
                    tempmaxthumbs -= maxthumbs
                Loop
            End If

            If count > maxthumbs Then
                Dim thispagecount As Integer = ((currentpage*maxthumbs) - 1)
                If thispagecount >= count Then thispagecount = (count -1)
                For f = ((currentpage-1)*maxthumbs) To thispagecount '(currentpage*maxthumbs) - 1
                    names.Add(posterArray.Item(f).ldurl)
                Next
            Else
                For f = 0 To count - 1
                    names.Add(posterArray.Item(f).ldurl)
                Next
            End If

            Label7.Visible = True
            If pagecount > 1 Then
                btnScrollPrev.Visible = currentpage > 1
                btnScrollNext.Visible = True
                If count >= maxthumbs Then
                    Label7.Text = "Displaying " & ((currentpage*maxthumbs)-(maxthumbs-1)) & " to " & (currentpage*maxthumbs).ToString & " of " & count.ToString & " Images"
                Else
                    Label7.Text = "Displaying 1 to " & count.ToString & " of " & count.ToString & " Images"
                End If
                btnScrollPrev.Enabled = currentpage > 1
                btnScrollNext.Enabled = currentpage < pagecount 'True
            Else
                btnScrollPrev.Visible = False
                btnScrollNext.Visible = False
                If count >= maxthumbs Then
                    Label7.Text = "Displaying 1 to " & maxthumbs.ToString & " of " & count.ToString & " Images"
                Else
                    Label7.Text = "Displaying 1 to " & count.ToString & " of " & count.ToString & " Images"
                End If
            End If

            Dim location As Integer = 0
            Dim itemcounter As Integer = 0
            For Each item As String In names
                picboxes() = New PictureBox()
                With picboxes
                    .Location = New Point(location, 0)
                    .Width = 123
                    .Height = 180
                    .SizeMode = PictureBoxSizeMode.Zoom
                    .ImageLocation = item
                    .Visible = True
                    .BorderStyle = BorderStyle.Fixed3D
                    .Name = "picture" & itemcounter.ToString
                    AddHandler picboxes.DoubleClick, AddressOf zoomimage
                    AddHandler picboxes.LoadCompleted, AddressOf imageres
                End With

                checkboxes() = New RadioButton()
                With checkboxes
                    .Location = New Point(location + 50, 195)
                    .Name = "checkbox" & itemcounter.ToString
                    .SendToBack()
                    .Text = " "
                    AddHandler checkboxes.CheckedChanged, AddressOf radiochanged
                End With

                itemcounter += 1
                location += 120

                Me.panel2.Controls.Add(picboxes())
                Me.panel2.Controls.Add(checkboxes())
                Me.Refresh()
                Application.DoEvents()
            Next
        Else
            Dim txtstring As String = ""
            If aok Then
                txtstring = "No Posters Were Found For This Movie"
            Else
                txtstring = "Selected site is not available"
            End If
            Dim mainlabel2 As Label
            mainlabel2 = New Label
            With mainlabel2
                .Location = New Point(0, 100)
                .Width = 700
                .Height = 100
                .Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
                .Text = txtstring '"No Posters Were Found For This Movie"
            End With
            Me.panel2.Controls.Add(mainlabel2)
        End If
    End Sub

    Private Sub panelclear()
        For i = Panel2.Controls.Count - 1 To 0 Step -1
            Panel2.Controls.RemoveAt(i)
        Next
    End Sub

    Private Sub initialise()
        If TextBox1.Text <> "" Then
            If IsNumeric(TextBox1.Text) And Convert.ToDecimal(TextBox1.Text) <> 0 Then
                maxthumbs = Convert.ToDecimal(TextBox1.Text)
                'Pref.maximumthumbs = maxthumbs
            Else
                MsgBox("Invalid Maximum Thumb Value" & vbCrLf & "Setting to default Value of 6")
                maxthumbs = 6
                TextBox1.Text = "6"
                'Pref.maximumthumbs = 6
            End If
        Else
            MsgBox("Invalid Maximum Thumb Value" & vbCrLf & "Setting to default Value of 6")
            maxthumbs = 6
            TextBox1.Text = "6"
            'Pref.maximumthumbs = 6
        End If

        btnSaveSmall.Visible = False
        btnSaveBig.Visible = False
        Me.Controls.Remove(panel2)
        panel2 = Nothing
        picboxes = Nothing
        checkboxes = Nothing
        reslabel = Nothing
        posterArray.Clear()
        currentpage = 1

        panel2 = New Panel
        With panel2
            .Width = Me.Width
            .Height = 237
            .Location = New Point(2, 2)
            .AutoScroll = True
        End With
        Me.Controls.Add(panel2)
        panel2.Refresh()
        Application.DoEvents()
        count = 0
        pagecount = 0
    End Sub

    Private Sub btnSaveSmall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSmall.Click
        Try
            Dim tempstring As String
            Dim realnumber As Integer = 0
            Dim tempint As Integer = 0
            Dim tempstring2 As String = ""
            Dim allok As Boolean = False
            For Each button As Control In Me.panel2.Controls
                If button.Name.IndexOf("checkbox") <> -1 Then
                    Dim b1 As RadioButton = CType(button, RadioButton)
                    If b1.Checked = True Then
                        tempstring = b1.Name
                        tempstring = tempstring.Replace("checkbox", "")
                        tempint = Convert.ToDecimal(tempstring)
                        realnumber = tempint + ((currentpage - 1) * maxthumbs)
                        tempstring2 = posterarray.Item(realnumber).ldurl
                        allok = True
                        Exit For
                    End If
                End If
            Next
            If allok = False Then
                MsgBox("No Fanart Is Selected")
            End If
            If allok = True Then
                For Each PictureBox2 As Control In Me.panel2.Controls
                    If PictureBox2.Name.IndexOf("picture") <> -1 And PictureBox2.Name.IndexOf(tempint) <> -1 Then
                        Dim b1 As PictureBox = CType(PictureBox2, PictureBox)
                        If Not b1.Image Is Nothing Then
                            If b1.Image.Width > 20 Then
                                Dim paths As List(Of String) = Pref.GetPosterPaths(fullpathandfilename, If(videotspath <> "", videotspath, ""))
                                For Each pth As String In Paths
                                    b1.Image.Save(pth, Imaging.ImageFormat.Jpeg)
                                    posterpath = pth
                                Next
                                Form1.util_ImageLoad(Form2.moviethumb, posterpath, Utilities.DefaultPosterPath)
                                Form1.util_ImageLoad(Form1.PbMoviePoster, posterpath, Utilities.DefaultPosterPath)
                                Form1.util_ImageLoad(Me.mainposter, posterpath, Utilities.DefaultPosterPath)
                                Label6.Visible = True
                                tempstring = b1.Image.Width.ToString & " x " & b1.Image.Height.ToString
                                Label6.Text = tempstring
                                mainposter.Visible = True
                                b1.Dispose()

                                Dim path As String = Utilities.save2postercache(fullpathandfilename, posterpath, Form1.WallPicWidth, Form1.WallPicHeight)
                                Form1.updateposterwall(path, fullpathandfilename)
                                Me.Close()
                                Exit For
                            Else
                                Label6.Visible = False
                            End If
                        Else
                            Label6.Visible = False
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnSaveBig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveBig.Click
        Try
            Dim tempstring As String
            Dim tempint As Integer = 0
            Dim realnumber As Integer = 0
            Dim tempstring2 As String = ""
            Dim allok As Boolean = False
            For Each button As Control In Me.panel2.Controls
                If button.Name.IndexOf("checkbox") <> -1 Then
                    Dim b1 As RadioButton = CType(button, RadioButton)
                    If b1.Checked = True Then
                        'tempstring = b1.Name
                        tempstring = b1.Name.Replace("checkbox", "")
                        'tempint = Convert.ToDecimal(tempstring)
                        realnumber = (tempstring.ToInt) + ((currentpage - 1) * maxthumbs)
                        tempstring2 = posterArray.Item(realnumber).hdurl
                        allok = True
                        Exit For
                    End If
                End If
            Next
            If allok = False Then
                MsgBox("No Fanart Is Selected")
            End If
            If allok = True Then
                If Not String.IsNullOrEmpty(tempstring2) Then
                    Dim paths As List(Of String) = Pref.GetPosterPaths(fullpathandfilename, If(videotspath <> "", videotspath, ""))
                    DownloadCache.SaveImageToCacheAndPaths(tempstring2, paths, True)
                    Form1.util_ImageLoad(Form2.moviethumb, paths(0), Utilities.DefaultPosterPath)
                    Form1.util_ImageLoad(Form1.PbMoviePoster, paths(0), Utilities.DefaultPosterPath)
                    Form1.util_ImageLoad(Me.mainposter, paths(0), Utilities.DefaultPosterPath)
                    Label6.Visible = True
                    tempstring = Me.mainposter.Image.Width.ToString & " x " & Me.mainposter.Image.Height.ToString
                    Label6.Text = tempstring
                    mainposter.Visible = True
                    Dim path As String = Utilities.save2postercache(fullpathandfilename, paths(0), Form1.WallPicWidth, Form1.WallPicHeight)
                    Form1.updateposterwall(path, fullpathandfilename)
                    Me.Close()
                Else
                    Label6.Visible = False
                End If
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub coverart_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            '77%
            panel2.Width = Me.Width
            mainposter.Width = Panel1.Width
            mainposter.Height = Panel1.Height
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnthumbbrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnthumbbrowse.Click
        Try
            openFD.InitialDirectory = Form1.workingMovieDetails.fileinfo.fullpathandfilename.Replace(Path.GetFileName(Form1.workingMovieDetails.fileinfo.fullpathandfilename), "")
            openFD.Title = "Select a jpeg image file File"
            openFD.FileName = ""
            openFD.Filter = "Media Companion Image Files|*.jpg;*.tbn;*.png;*.bmp|All Files|*.*"
            openFD.FilterIndex = 0
            openFD.ShowDialog()
            TextBox5.Text = openFD.FileName
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btngetthumb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngetthumb.Click
        Try
            Dim MyWebClient As New System.Net.WebClient
            Try
                Dim ImageInBytes() As Byte = MyWebClient.DownloadData(TextBox5.Text)
                Dim ImageStream As New IO.MemoryStream(ImageInBytes)

                mainposter.Image = New System.Drawing.Bitmap(ImageStream)
                mainposter.Image.Save(posterpath)
                Form2.moviethumb.Image = mainposter.Image
                Form1.PbMoviePoster.Image = mainposter.Image
            Catch ex As Exception
                MsgBox("Unable To Download Image")
            End Try
            Panel3.Visible = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub

    Private Sub btncancelgetthumburl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncancelgetthumburl.Click
        Try
            Panel3.Visible = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub

    Private Sub btnSourceManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSourceManual.Click
        Try
            Panel3.Visible = True
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

End Class