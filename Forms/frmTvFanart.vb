

Imports System.Net
Imports Alphaleonis.Win32.Filesystem
'Imports System.IO

Public Class frmTvFanart
    Dim totalpages As Integer = 0

    Dim bigpanel As Panel
    Dim WithEvents picboxes As PictureBox
    Dim WithEvents checkboxes As RadioButton
    Dim WithEvents labels As Label
    Dim WithEvents reslabel As Label
    Dim resolutionlbl As Label
    Dim panel2 As Panel
    Dim posterurls(1000, 1) As String
    Dim posterpath As String
    Dim WithEvents mainposter As PictureBox
    Dim WithEvents bigpicbox As PictureBox
    Dim count As Integer = 0
    Dim title As String = Form1.titletxt.Text
    Dim itemnumber As Integer
    Dim rememberint As Integer
    Dim maxthumbs As Integer = Pref.maximumthumbs
    Dim pagecount As Integer = 0
    Dim currentpage As Integer = 1
    Dim downloadthumb(3000, 1) As String
    Dim workingthumb As Integer
    Dim downloadthumbcount As Integer
    Dim url As String
    Dim websource(3000) As String
    Dim urllinecount As Integer
    Dim series As Integer


    Private Sub tvfanart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'posterpath = Form1.tvpath & "fanart.jpg"
            TextBox1.Text = maxthumbs.ToString
            Me.Refresh()
            Application.DoEvents()
            Dim exists As Boolean = File.Exists(posterpath)
            If exists = True Then

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
                        .Anchor = AnchorStyles.Bottom Or AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
                    End With
                    Me.Panel1.Controls.Add(mainposter)
                    tempstring = mainposter.Image.Width.ToString & " x " & mainposter.Image.Height.ToString
                    Label6.Text = tempstring
                Catch
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
                End Try
            Else
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
                .Width = 772
                .Height = 237
                .Location = New Point(2, 2)
                .AutoScroll = True
            End With
            Me.Controls.Add(panel2)

        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub

    Private Sub initialise()
        If TextBox1.Text <> "" Then
            If IsNumeric(TextBox1.Text) And Convert.ToDecimal(TextBox1.Text) <> 0 Then
                maxthumbs = Convert.ToDecimal(TextBox1.Text)
                Pref.maximumthumbs = maxthumbs
            Else
                MsgBox("Invalid Maximum Thumb Value" & vbCrLf & "Setting to default Value of 10")
                maxthumbs = 10
                TextBox1.Text = "10"
                Pref.maximumthumbs = 10
            End If
        Else
            MsgBox("Invalid Maximum Thumb Value" & vbCrLf & "Setting to default Value of 10")
            maxthumbs = 10
            TextBox1.Text = "10"
            Pref.maximumthumbs = 10
        End If

        Button5.Visible = False

        Me.Controls.Remove(panel2)
        panel2 = Nothing
        picboxes = Nothing
        checkboxes = Nothing
        reslabel = Nothing

        panel2 = New Panel
        With panel2
            .Width = Me.Width - 10
            .Height = 237
            .Location = New Point(2, 2)
            .AutoScroll = True
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        End With
        Me.Controls.Add(panel2)

        panel2.Refresh()
        Application.DoEvents()

        ReDim posterurls(1000, 1)
        count = 0
        pagecount = 0
    End Sub

    Private Sub displayselection()

        If TextBox1.Text <> "" Then
            If IsNumeric(TextBox1.Text) Then
                Dim tempint As Integer
                tempint = Convert.ToInt16(TextBox1.Text)
                If tempint > 5 Then
                    maxthumbs = tempint
                End If
            End If
        End If

        Dim names As New List(Of String)()

        If count > 0 Then

            If count > maxthumbs Then
                Dim tempmaxthumbs As Integer = count

                Do Until tempmaxthumbs < 1
                    pagecount += 1
                    tempmaxthumbs -= maxthumbs
                Loop
            End If


            If count > maxthumbs Then
                For f = 1 To maxthumbs
                    names.Add(downloadthumb(f, 0))
                Next
            Else
                For f = 1 To count
                    names.Add(downloadthumb(f, 0))
                Next
            End If

            Label7.Visible = True
            If pagecount > 1 Then
                Button7.Visible = True
                Button8.Visible = True
                If count >= maxthumbs Then
                    Label7.Text = "Displaying 1 to " & maxthumbs.ToString & " of " & count.ToString & " Images"
                Else
                    Label7.Text = "Displaying 1 to " & count.ToString & " of " & count.ToString & " Images"
                End If
                currentpage = 1
                Button7.Enabled = False
                Button8.Enabled = True
            Else
                Button7.Visible = False
                Button8.Visible = False
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
                    .Width = 280
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
                    .Location = New Point(location + 135, 195)
                    .Name = "checkbox" & itemcounter.ToString
                    .SendToBack()
                    .Text = " "
                    AddHandler checkboxes.CheckedChanged, AddressOf radiochanged
                End With

                itemcounter += 1
                location += 280

                Me.panel2.Controls.Add(picboxes())
                Me.panel2.Controls.Add(checkboxes())
                Me.Refresh()
                Application.DoEvents()
            Next
        Else
            Dim mainlabel2 As Label
            mainlabel2 = New Label
            With mainlabel2
                .Location = New Point(0, 100)
                .Width = 700
                .Height = 100
                .Font = New System.Drawing.Font("Arial", 15, FontStyle.Bold)
                .Text = "No Posters Were Found For This Movie"
            End With
            Me.panel2.Controls.Add(mainlabel2)
        End If
    End Sub

    Private Sub imageres(ByVal sender As Object, ByVal e As EventArgs)


        reslabel = New Label
        Dim tempstring As String

        tempstring = sender.image.width.ToString
        tempstring = tempstring & " x "
        tempstring = tempstring & sender.image.height.ToString
        Dim locx As Integer = sender.location.x
        Dim locy As Integer = sender.location.y
        locy = locy + sender.height

        With reslabel
            .Location = New Point(locx + 100, locy)
            .Text = tempstring
            .BringToFront()
        End With

        Me.panel2.Controls.Add(reslabel)


        Me.Refresh()
        Application.DoEvents()
    End Sub

    Private Sub radiochanged(ByVal sender As Object, ByVal e As EventArgs)
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
            Button5.Visible = True
        Else
            Button5.Visible = False
        End If

    End Sub

    Private Sub zoomimage(ByVal sender As Object, ByVal e As EventArgs)

        Dim tempstring As String = sender.name
        Dim tempint As Integer
        Dim tempstring2 As String = tempstring
        tempstring = tempstring.Replace("picture", "")
        tempint = Convert.ToDecimal(tempstring)
        tempint = tempint + ((currentpage - 1) * maxthumbs)
        rememberint = tempint
        Dim buffer(4000000) As Byte
        Dim size As Integer = 0
        Dim bytesRead As Integer = 0

        bigpanel = New Panel
        With bigpanel
            .Width = Me.Width
            .Height = Me.Height
            .BringToFront()
            .Dock = DockStyle.Fill
        End With

        bigpicbox = New PictureBox()

        With bigpicbox
            .SizeMode = PictureBoxSizeMode.Zoom
            .Location = New Point(0, 0)
            .Width = bigpanel.Width
            .Height = bigpanel.Height
            .SizeMode = PictureBoxSizeMode.Zoom
            '.Image = sender.image
            .ImageLocation = downloadthumb(tempint + 1, 1)
            .Visible = True
            .BorderStyle = BorderStyle.Fixed3D
            AddHandler bigpicbox.DoubleClick, AddressOf closeimage
            .Dock = DockStyle.Fill
        End With

        Dim sizex As Integer = bigpicbox.Width
        Dim sizey As Integer = bigpicbox.Height


        Me.Controls.Add(bigpanel)
        bigpanel.BringToFront()
        Me.bigpanel.Controls.Add(bigpicbox)
        Me.Refresh()

    End Sub

    Private Sub closeimage()
        Me.Controls.Remove(bigpanel)
        bigpanel = Nothing
        Me.Controls.Remove(bigpicbox)
        bigpicbox = Nothing
    End Sub

    Private Sub loadwebpage()
        urllinecount = 0
        Try
            Dim wrGETURL As WebRequest
            wrGETURL = WebRequest.Create(url)
            wrGETURL.Proxy = Utilities.MyProxy
            'Dim myProxy As New WebProxy("myproxy", 80)
            'myProxy.BypassProxyOnLocal = True
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            Dim sLine As String = ""
            urllinecount = 0

            Do While Not sLine Is Nothing
                urllinecount += 1
                sLine = objReader.ReadLine
                If Not sLine Is Nothing Then
                    websource(urllinecount) = sLine
                End If
            Loop
            objReader.Close()
            urllinecount -= 1

        Catch ex As WebException
            'MsgBox("Unable to load webpage " & url & vbCrLf & vbCrLf & ex.ToString)
        End Try

    End Sub



    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            downloadthumbcount = 0
            ReDim posterurls(1000, 1)
            count = 0
            pagecount = 0
            Call initialise()
            Call loadwebpage()
            Dim tempint As Integer
            Dim imdbsmall(2000) As String
            Dim reached As Boolean = False
            Dim counter As Integer = 0
            For f = 1 To urllinecount
                If websource(f).IndexOf("<a href=""?page=") <> -1 Then
                    websource(f) = websource(f).Replace("<a href=""?page=", "")
                    websource(f) = websource(f).Substring(0, 1)
                    tempint = Convert.ToString(websource(f))
                    If tempint > totalpages Then totalpages = tempint
                End If
                '<div class="thumb_list" 


                If websource(f).IndexOf("<div class=""thumb_list""") <> -1 Then
                    reached = True
                End If
                If reached = True Then
                    If websource(f).IndexOf("</div>") <> -1 Then
                        'reached = False
                        'Exit For
                    End If
                    If websource(f).IndexOf("src=""http://") <> -1 Then
                        websource(f) = websource(f).Substring(websource(f).IndexOf("src=""") - 1, websource(f).Length - websource(f).IndexOf("src=""") - 1)
                        websource(f).TrimStart()
                        websource(f) = websource(f).Replace("src=""", "")
                        If websource(f).IndexOf("._V1._") <> -1 Then
                            counter = counter + 1
                            imdbsmall(counter) = websource(f).Substring(1, websource(f).IndexOf("._V1._"))
                        End If
                    End If
                End If
            Next
            For g = 2 To totalpages

                Call loadwebpage()
                For f = 1 To urllinecount
                    If websource(f).IndexOf("<div class=""thumb_list""") <> -1 Then
                        reached = True
                    End If
                    If reached = True Then
                        If websource(f).IndexOf("</div>") <> -1 Then
                            'reached = False
                            'Exit For
                        End If
                        If websource(f).IndexOf("src=""http://") <> -1 Then
                            websource(f) = websource(f).Substring(websource(f).IndexOf("src=""") - 1, websource(f).Length - websource(f).IndexOf("src=""") - 1)
                            websource(f).TrimStart()
                            websource(f) = websource(f).Replace("src=""", "")
                            If websource(f).IndexOf("._V1._") <> -1 Then
                                counter = counter + 1
                                imdbsmall(counter) = websource(f).Substring(1, websource(f).IndexOf("._V1._"))
                            End If
                        End If
                    End If
                Next
            Next

            For f = 1 To counter
                imdbsmall(f) = imdbsmall(f) & "_V1._SX1000_SY1000_.jpg"

            Next

            For f = counter To 1 Step -1
                downloadthumbcount = downloadthumbcount + 1
                downloadthumb(downloadthumbcount, 0) = imdbsmall(f)
                downloadthumb(downloadthumbcount, 1) = imdbsmall(f)
            Next

            count = downloadthumbcount
            Call displayselection()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Try
            Button5.Visible = False
            Me.Controls.Remove(panel2)
            panel2 = Nothing
            picboxes = Nothing
            checkboxes = Nothing

            panel2 = New Panel
            With panel2
                .Width = 782
                .Height = 232
                .Location = New Point(2, 2)
                .AutoScroll = True
            End With
            Me.Controls.Add(panel2)

            currentpage -= 1
            Button7.Enabled = True
            If currentpage <= 1 Then
                Button7.Enabled = False
            Else
                Button7.Enabled = True
            End If
            If currentpage = pagecount Then
                Button8.Enabled = False
            Else
                Button8.Enabled = True
            End If
            Dim tempint As Integer = (currentpage * (maxthumbs) + 1) - maxthumbs
            Dim tempint2 As Integer = currentpage * maxthumbs
            If tempint2 > count Then
                tempint2 = count
            End If

            Dim names As New List(Of String)()

            For f = tempint To tempint2
                names.Add(downloadthumb(f, 0))
            Next
            Label7.Text = "Displaying " & tempint.ToString & " to " & tempint2 & " of " & count.ToString & " Images"

            Dim location As Integer = 0
            Dim itemcounter As Integer = 0
            For Each item As String In names
                picboxes() = New PictureBox()
                With picboxes
                    .Location = New Point(location, 0)
                    .Width = 280
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
                    .Location = New Point(location + 135, 195)
                    .Name = "checkbox" & itemcounter.ToString
                    .SendToBack()
                    .Text = " "
                    AddHandler checkboxes.CheckedChanged, AddressOf radiochanged
                End With

                itemcounter += 1
                location += 280

                Me.panel2.Controls.Add(picboxes())
                Me.panel2.Controls.Add(checkboxes())
                Me.Refresh()
                Application.DoEvents()
            Next
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Try
            Button5.Visible = False
            Me.Controls.Remove(panel2)
            panel2 = Nothing
            picboxes = Nothing
            checkboxes = Nothing

            panel2 = New Panel
            With panel2
                .Width = 782
                .Height = 232
                .Location = New Point(2, 2)
                .AutoScroll = True
            End With
            Me.Controls.Add(panel2)

            currentpage += 1
            Button7.Enabled = True

            If currentpage = pagecount Then
                Button8.Enabled = False
            Else
                Button8.Enabled = True
            End If


            Dim tempint As Integer = (currentpage * (maxthumbs) + 1) - maxthumbs
            Dim tempint2 As Integer = currentpage * maxthumbs

            If tempint2 > count Then
                tempint2 = count
            End If

            Dim names As New List(Of String)()

            For f = tempint To tempint2
                names.Add(downloadthumb(f, 0))
            Next
            Label7.Text = "Displaying " & tempint.ToString & " to " & tempint2 & " of " & count.ToString & " Images"

            Dim location As Integer = 0
            Dim itemcounter As Integer = 0
            For Each item As String In names


                picboxes() = New PictureBox()
                With picboxes
                    .Location = New Point(location, 0)
                    .Width = 280
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
                    .Location = New Point(location + 135, 195)
                    .Name = "checkbox" & itemcounter.ToString
                    .SendToBack()
                    .Text = " "
                    AddHandler checkboxes.CheckedChanged, AddressOf radiochanged
                End With

                itemcounter += 1
                location += 280

                Me.panel2.Controls.Add(picboxes())
                Me.panel2.Controls.Add(checkboxes())
                Me.Refresh()
                Application.DoEvents()
            Next
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub

    Private Sub bigpicbox_LoadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles bigpicbox.LoadCompleted
        Try
            Dim bigpanellabel As Label
            bigpanellabel = New Label
            With bigpanellabel
                .Location = New Point(20, 20)
                .Width = 150
                .Height = 50
                .Visible = True
                .Text = "Double Click Image To" & vbCrLf & "Return To Browser"
                '   .BringToFront()
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
                    .Location = New Point(Me.Width - 200, 20)
                    .Anchor = AnchorStyles.Right Or AnchorStyles.Top
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
                bigpicbox.ImageLocation = posterurls(rememberint + 1, 1)
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try

    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Call initialise()

            ReDim downloadthumb(2000, 1)
            downloadthumbcount = 0
            workingthumb = 0
            Dim tempstring As String
            Dim tempstring2 As String
            downloadthumbcount = 0
            Call loadwebpage()
            Dim tempstring3 As String

            tempstring3 = "<BannerPath>fanart"
            For f = 1 To urllinecount
                If websource(f).IndexOf(tempstring3) <> -1 Then
                    tempstring2 = websource(f)
                    tempstring2 = tempstring2.Replace("<BannerPath>", "")
                    tempstring2 = tempstring2.Replace("</BannerPath>", "")
                    tempstring2 = tempstring2.Replace("  ", "")
                    '_cache/posters/73739-4.jpg
                    tempstring = tempstring2
                    tempstring2 = URLs.TVdbBanners(tempstring2)
                    tempstring = URLs.TVdbBannersCache(tempstring)
                    downloadthumbcount += 1
                    downloadthumb(downloadthumbcount, 0) = tempstring
                    downloadthumb(downloadthumbcount, 1) = tempstring2
                End If
            Next

            count = downloadthumbcount
            Call displayselection()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub


    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Try
            Panel3.Visible = True
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

    Private Sub btnthumbbrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnthumbbrowse.Click
        Try
            openFD.InitialDirectory = Pref.applicationPath
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
            Dim savepath As String = String.Empty



            Dim MyWebClient As New System.Net.WebClient
            Try
                Dim ImageInBytes() As Byte = MyWebClient.DownloadData(TextBox5.Text)
                Dim ImageStream As New IO.MemoryStream(ImageInBytes)

                mainposter.Image = New System.Drawing.Bitmap(ImageStream)
                mainposter.Image.Save(savepath)

            Catch ex As Exception
                MsgBox("Unable To Download Image")
            End Try
            Panel3.Visible = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
End Class