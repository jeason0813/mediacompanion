Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Linq

Public Class Form2
    Const SetDefaults = True
    Dim editsmade As Boolean = False
    Dim thumbeditsmade As Boolean = False
    Dim oldactors(9999, 2)
    Dim actorcount As Integer = 0
    Dim workingmovieedit As New FullMovieDetails
    Dim posterpath As String = ""
    Dim cropstring As String
    Dim datechanged As Boolean = False
    Dim PremierDateChanged As Boolean = False
    Dim textBoxList As List(Of TextBox)
    Dim nfoFunction As New WorkingWithNfoFiles

    
    Private Sub setupdisplay()
        actorcb.Items.Clear()
        If workingmovieedit.fullmoviebody.title         <> Nothing Then titletxt.Text = workingmovieedit.fullmoviebody.title
        If workingmovieedit.fullmoviebody.originaltitle <> Nothing Then originaltxt.Text = workingmovieedit.fullmoviebody.originaltitle
        If workingmovieedit.fullmoviebody.sortorder     <> Nothing Then sorttxt.Text = workingmovieedit.fullmoviebody.sortorder 
        If workingmovieedit.fullmoviebody.director      <> Nothing Then directortxt.Text = workingmovieedit.fullmoviebody.director
        If workingmovieedit.fullmoviebody.stars         <> Nothing Then starstxt.Text = workingmovieedit.fullmoviebody.stars
        If workingmovieedit.fullmoviebody.runtime       <> Nothing Then runtimetxt.Text = workingmovieedit.fullmoviebody.runtime
        If workingmovieedit.fullmoviebody.credits       <> Nothing Then creditstxt.Text = workingmovieedit.fullmoviebody.credits
        If workingmovieedit.fullmoviebody.mpaa          <> Nothing Then mpaatxt.Text = workingmovieedit.fullmoviebody.mpaa
        If workingmovieedit.fullmoviebody.studio        <> Nothing Then studiotxt.Text = workingmovieedit.fullmoviebody.studio
        If workingmovieedit.fullmoviebody.country       <> Nothing Then countrytxt.Text = workingmovieedit.fullmoviebody.country 
        If workingmovieedit.fullmoviebody.genre         <> Nothing Then genretxt.Text = workingmovieedit.fullmoviebody.genre
        If workingmovieedit.fullmoviebody.year          <> Nothing Then yeartxt.Text = workingmovieedit.fullmoviebody.year
        If workingmovieedit.fullmoviebody.rating        <> Nothing Then ratingtxt.Text = workingmovieedit.fullmoviebody.rating
        If workingmovieedit.fullmoviebody.imdbid        <> Nothing Then idtxt.Text = workingmovieedit.fullmoviebody.imdbid
        If workingmovieedit.fullmoviebody.votes         <> Nothing Then votestxt.Text = workingmovieedit.fullmoviebody.votes
        If workingmovieedit.fullmoviebody.outline       <> Nothing Then outlinetxt.Text = workingmovieedit.fullmoviebody.outline
        If workingmovieedit.fullmoviebody.plot          <> Nothing Then plottxt.Text = workingmovieedit.fullmoviebody.plot
        If workingmovieedit.fullmoviebody.tagline       <> Nothing Then taglinetxt.Text = workingmovieedit.fullmoviebody.tagline
        If workingmovieedit.fullmoviebody.top250        <> Nothing Then top250txt.Text = workingmovieedit.fullmoviebody.top250 
        If workingmovieedit.fullmoviebody.trailer       <> Nothing Then tb_TrailerURL.Text = workingmovieedit.fullmoviebody.trailer
        If workingmovieedit.fullmoviebody.showlink      <> Nothing Then tb_SeriesLnk.Text = workingmovieedit.fullmoviebody.showlink 
        If workingmovieedit.fullmoviebody.usrrated      <> Nothing Then cbUsrRating.Text = If(workingmovieedit.fullmoviebody.usrrated = "0", "None", workingmovieedit.fullmoviebody.usrrated)
        Try
            If workingmovieedit.fileinfo.createdate <> Nothing Then Createdatepicker.Value = DateTime.ParseExact(workingmovieedit.fileinfo.createdate, Pref.datePattern, Nothing)
        Catch ex As Exception 
            MsgBox(ex.tostring)
        End Try
        Try
            If workingmovieedit.fullmoviebody.premiered <> Nothing Then PremieredDatePicker.Value = workingmovieedit.fullmoviebody.premiered 
        Catch
        End Try
        If workingmovieedit.fileinfo.fullpathandfilename <> Nothing Then filenametxt.Text = workingmovieedit.fileinfo.fullpathandfilename
        If Convert.ToInt32(workingmovieedit.fullmoviebody.playcount ) > 0 Then
            btnWatched.Text = "Watched"
            btnWatched.BackColor = Color.LawnGreen
            btnWatched.Refresh()
        Else
            btnWatched.Text = "UnWatched"
            btnWatched.BackColor = Color.Red
            btnWatched.Refresh()
        End If
        For Each actor In workingmovieedit.listactors
            actorcb.Items.Add(actor.actorname)
        Next

        Try
            If workingmovieedit.listactors.Count > 0 Then
                actorcb.SelectedItem = workingmovieedit.listactors(0).actorname
            End If
        Catch ex As Exception
        End Try

        If actorcb.Text <> "" Then roletxt.Text = workingmovieedit.listactors(0).actorrole
        posterpath = Form1.workingMovieDetails.fileinfo.posterpath
        If posterpath <> Nothing Then
            If posterpath <> "" Then
                If posterpath.ToLower.IndexOf(".jpg") <> -1 Or posterpath.ToLower.IndexOf(".tbn") <> -1 Then
                    moviethumb.ImageLocation = posterpath
                Else
                    moviethumb.Image = Nothing
                End If
            Else
                moviethumb.Image = Nothing
            End If
        Else
            moviethumb.Image = Nothing
        End If
        editsmade = False
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            workingmovieedit = Form1.workingMovieDetails
            For Each actor In workingmovieedit.listactors
                If actor.actorname <> Nothing Then
                    actorcount += 1
                    oldactors(actorcount, 0) = actor.actorname
                    If actor.actorname <> Nothing Then oldactors(actorcount, 1) = actor.actorrole
                    If actor.actorthumb <> Nothing Then oldactors(actorcount, 2) = actor.actorthumb
                End If
            Next
            RemoveHandler Createdatepicker.ValueChanged, AddressOf Createdatepicker_ValueChanged
            RemoveHandler PremieredDatePicker.ValueChanged, AddressOf PremieredDatePicker_ValueChanged
            Createdatepicker.Format = DateTimePickerFormat.Custom
            Createdatepicker.CustomFormat = Pref.datePattern  '"yyyyMMddHHmmss"
            PremieredDatePicker.Format = DateTimePickerFormat.Custom
            PremieredDatePicker.CustomFormat = Pref.nfoDatePattern  '"yyyy-MM-dd"
            
            
            Call setupdisplay()

            textBoxList = New List(Of TextBox)
            Dim cboxlist As New List(Of ComboBox)
            'Loop through every control on the form and add every textbox to the list
            For Each c As Control In Me.Controls
                If TypeOf c Is TextBox Then
                    textBoxList.Add(DirectCast(c, TextBox))
                End If
                If TypeOf c Is ComboBox Then
                    cboxlist.Add(DirectCast(c, ComboBox))
                End If
            Next

            'Loop through your list of textboxes and add eventhandlers
            For Each txt As TextBox In textBoxList
                AddHandler txt.TextChanged, AddressOf AnyTextBox_TextChanged
            Next
            RemoveHandler roletxt.TextChanged, AddressOf AnyTextBox_TextChanged
            For each cbox As ComboBox In cboxlist
                AddHandler cbox.SelectedIndexChanged, AddressOf AnyCbox_SelectedIndexChanged
            Next
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Form2_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Call checkforedits()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub checkforedits()
        If editsmade Then
            editsmade = False
            Dim t As Integer = MsgBox("Edits were made" & vbCrLf & "Do you wish to Save changes before this form closes", MsgBoxStyle.YesNo)
            If t = DialogResult.Yes Then
                btnsavechanges.PerformClick()
            End If
        End If
    End Sub

    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Try
            Me.Close()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub actorcb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles actorcb.SelectedIndexChanged
        Try
            roletxt.Text = workingmovieedit.listactors(actorcb.SelectedIndex).actorrole
            If workingmovieedit.listactors(actorcb.SelectedIndex).actorthumb <> Nothing Then
                Try
                    PictureBox1.ImageLocation = Pref.GetActorThumbPath(workingmovieedit.listactors(actorcb.SelectedIndex).actorthumb)
                Catch ex As Exception
                    PictureBox1.Image = Nothing
                End Try
            Else
                PictureBox1.Image = Nothing
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    
    Private Sub btndeleteactor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndeleteactor.Click
        Try
            If actorcb.Items.Count <> 0 Then
                Dim tempint As Integer = actorcb.SelectedIndex
                workingmovieedit.listactors.RemoveAt(actorcb.SelectedIndex)
                actorcb.Items.RemoveAt(actorcb.SelectedIndex)
                If workingmovieedit.listactors.Count <= tempint Then
                    tempint = workingmovieedit.listactors.Count - 1
                End If
                If workingmovieedit.listactors.Count <> 0 Then
                    actorcb.SelectedIndex = tempint
                Else
                    actorcb.Text = ""
                    roletxt.Text = ""
                End If
                editsmade = True
            Else
                MsgBox("No Actors to delete")
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub ' Delete Actor Button

    Private Sub btnaddactor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnaddactor.Click
        Try
            Dim newactor As New Actor
            For Each currentactor In actorcb.items
                If tb_RoleAdd.Text = currentactor Then
                    MsgBox("Actor exists in this movie")
                    Exit sub
                End If
            Next
            newactor.ID.value = ""
            newactor.actorname = tb_ActorAdd.Text
            newactor.actorrole = tb_RoleAdd.Text
            newactor.actorthumb = ""
            workingmovieedit.listactors.Add(newactor)
            actorcb.Items.Add(newactor.Actorname)
            tb_ActorAdd.Text = ""
            tb_RoleAdd.Text = ""
            editsmade = True
            If actorcb.SelectedIndex = -1 AndAlso actorcb.Items.Count > 0 Then actorcb.SelectedIndex = 0
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub ' Add Actor Button

    Private Sub btnchangemovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnchangemovie.Click
        Try
            MsgBox("Use 'Change Movie' on main Media Companion",vbExclamation)
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnAltPosterBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAltPosterBrowser.Click
        Try
            Dim t As New frmMovieAltPosterArt()
            If Pref.MultiMonitoEnabled Then
                Dim w As Integer = t.Width
                Dim h As Integer = t.Height
                t.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
                t.StartPosition = FormStartPosition.Manual
                t.Width = w
                t.Height = h
            End If
            t.ShowDialog()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    '_______________________________________________________
    '_________________Crop Thumbnail Code___________________
    '_______________________________________________________
#Region "Crop"
    Private Function CropImage(ByVal SrcBmp As Bitmap, ByVal NewSize As Size, ByVal StartPoint As Point) As Bitmap
        Dim SrcRect As New Rectangle(StartPoint.X, StartPoint.Y, NewSize.Width, NewSize.Height)
        Dim DestRect As New Rectangle(0, 0, NewSize.Width, NewSize.Height)
        Dim DestBmp As New Bitmap(NewSize.Width, NewSize.Height, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(DestBmp)
        g.DrawImage(SrcBmp, DestRect, SrcRect, GraphicsUnit.Pixel)
        Return DestBmp
    End Function ' Crop Image Function

    ' Below sets part of image to crop, top, bottom, left, right
    Private Sub croptop()
        If moviethumb.Image Is Nothing Then Exit Sub
        Dim imagewidth As Integer
        Dim imageheight As Integer
        imagewidth = moviethumb.Image.Width
        imageheight = moviethumb.Image.Height
        PictureBox2.Image = moviethumb.Image
        moviethumb.Image = CropImage(PictureBox2.Image, New Size(imagewidth, imageheight - 1), New Point(0, 1)).Clone
        moviethumb.SizeMode = PictureBoxSizeMode.Zoom
    End Sub
    Private Sub cropbottom()
        If moviethumb.Image Is Nothing Then Exit Sub
        Dim imagewidth As Integer
        Dim imageheight As Integer
        imagewidth = moviethumb.Image.Width
        imageheight = moviethumb.Image.Height
        PictureBox2.Image = moviethumb.Image
        moviethumb.Image = CropImage(PictureBox2.Image, New Size(imagewidth, imageheight - 1), New Point(0, 0)).Clone
        moviethumb.SizeMode = PictureBoxSizeMode.Zoom
    End Sub
    Private Sub cropleft()
        If moviethumb.Image Is Nothing Then Exit Sub
        Dim imagewidth As Integer
        Dim imageheight As Integer
        imagewidth = moviethumb.Image.Width
        imageheight = moviethumb.Image.Height
        PictureBox2.Image = moviethumb.Image
        moviethumb.Image = CropImage(PictureBox2.Image, New Size(imagewidth - 1, imageheight), New Point(1, 0)).Clone
        moviethumb.SizeMode = PictureBoxSizeMode.Zoom
    End Sub
    Private Sub cropright()
        If moviethumb.Image Is Nothing Then Exit Sub
        Dim imagewidth As Integer
        Dim imageheight As Integer
        thumbeditsmade = True
        imagewidth = moviethumb.Image.Width
        imageheight = moviethumb.Image.Height
        PictureBox2.Image = moviethumb.Image
        moviethumb.Image = CropImage(PictureBox2.Image, New Size(imagewidth - 1, imageheight), New Point(0, 0)).Clone
        moviethumb.SizeMode = PictureBoxSizeMode.Zoom
    End Sub

    'crop while button is down, and stop when button is up
    Private Sub btncroptop_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncroptop.MouseDown
        Try
            If moviethumb.Image Is Nothing Then Exit Sub
            thumbeditsmade = True
            btnresetimage.Enabled = True
            btnSaveCropped.Enabled = True
            Call croptop()
            cropstring = "top"
            Timer1.Enabled = True
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncroptop_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncroptop.MouseUp
        Try
            Timer1.Enabled = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncropbottom_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropbottom.MouseDown
        Try
            If moviethumb.Image Is Nothing Then Exit Sub
            thumbeditsmade = True
            btnresetimage.Enabled = True
            btnSaveCropped.Enabled = True
            Call cropbottom()
            cropstring = "bottom"
            Timer1.Enabled = True
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncropbottom_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropbottom.MouseUp
        Try
            Timer1.Enabled = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncropleft_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropleft.MouseDown
        Try
            If moviethumb.Image Is Nothing Then Exit Sub
            thumbeditsmade = True
            btnresetimage.Enabled = True
            btnSaveCropped.Enabled = True
            Call cropleft()
            cropstring = "left"
            Timer1.Enabled = True
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncropleft_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropleft.MouseUp
        Try
            Timer1.Enabled = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncropright_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropright.MouseDown
        Try
            If moviethumb.Image Is Nothing Then Exit Sub
            editsmade = True
            btnresetimage.Enabled = True
            btnSaveCropped.Enabled = True
            Call cropright()
            cropstring = "right"
            Timer1.Enabled = True
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub
    Private Sub btncropright_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btncropright.MouseUp
        Try
            Timer1.Enabled = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnresetimage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnresetimage.Click
        Try
            thumbeditsmade = False
            moviethumb.Image = Form1.PbMoviePoster.Image
            btnresetimage.Enabled = False
            btnSaveCropped.Enabled = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnsavecropped_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsavecropped.Click
        Try
            thumbeditsmade = False
            Dim tempstring As String
            tempstring = Form1.workingMovieDetails.fileinfo.posterpath
            Dim stream As New IO.MemoryStream
            moviethumb.Image.Save(tempstring, System.Drawing.Imaging.ImageFormat.Jpeg)
            Form1.PbMoviePoster.Image = moviethumb.Image
            btnresetimage.Enabled = False
            btnSaveCropped.Enabled = False
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            If cropstring = "top" Then Call croptop()
            If cropstring = "bottom" Then Call cropbottom()
            If cropstring = "left" Then Call cropleft()
            If cropstring = "right" Then Call cropright()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub ' Set auto repeat for crop

#End Region 


    Private Sub btnsavechanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsavechanges.Click
        Try
            workingmovieedit.fullmoviebody.plot         = plottxt.Text
            workingmovieedit.fullmoviebody.title        = titletxt.Text
            workingmovieedit.fullmoviebody.originaltitle = originaltxt.Text
            workingmovieedit.fullmoviebody.sortorder    = sorttxt.text
            workingmovieedit.fullmoviebody.director     = directortxt.Text
            workingmovieedit.fullmoviebody.stars        = starstxt.Text
            workingmovieedit.fullmoviebody.runtime      = runtimetxt.Text
            workingmovieedit.fullmoviebody.credits      = creditstxt.Text
            workingmovieedit.fullmoviebody.mpaa         = mpaatxt.Text
            workingmovieedit.fullmoviebody.studio       = studiotxt.Text
            workingmovieedit.fullmoviebody.genre        = genretxt.Text
            workingmovieedit.fullmoviebody.country      = countrytxt.Text 
            workingmovieedit.fullmoviebody.year         = yeartxt.Text
            workingmovieedit.fullmoviebody.rating       = ratingtxt.Text
            workingmovieedit.fullmoviebody.votes        = votestxt.Text
            workingmovieedit.fullmoviebody.outline      = outlinetxt.Text
            workingmovieedit.fullmoviebody.tagline      = taglinetxt.Text
            workingmovieedit.fullmoviebody.showlink     = tb_SeriesLnk.Text
            workingmovieedit.fullmoviebody.usrrated     = If(cbUsrRating.Text = "None", "0", cbUsrRating.Text)
            Form1.workingMovieDetails.fullmoviebody     = workingmovieedit.fullmoviebody
            Form1.workingMovieDetails.listactors        = workingmovieedit.listactors
            Form1.workingMovieDetails.listthumbs        = workingmovieedit.listthumbs
            If datechanged Then
                Dim credate As String =  Format(Createdatepicker.Value, Pref.datePattern).ToString
                If workingmovieedit.fileinfo.createdate <> credate Then
                    workingmovieedit.fileinfo.createdate = credate
                End If
            End If
            If PremierDateChanged Then
                Dim premdate As String = Format(PremieredDatePicker.Value, Pref.nfoDatePattern).ToString
                If workingmovieedit.fullmoviebody.premiered <> premdate Then
                    workingmovieedit.fullmoviebody.premiered = premdate 
                End If
            End If

            ' check valid url of trailer if changed
            If Not tb_TrailerURL.Text = workingmovieedit.fullmoviebody.trailer Then
                Dim isvalid As Boolean = Utilities.UrlIsValid(tb_TrailerURL.Text)
                If Not isvalid Then
                    Dim t As Integer = MsgBox("The entered Trailer URL is not a Valid URL." & vbCrLf & "Do you wish to keep the new URL, revert to original URL?" & vbCrLf & "Yes to Keep, No to Revert", MsgBoxStyle.YesNo)
                    If t = DialogResult.Yes Then
                        workingmovieedit.fullmoviebody.trailer = tb_TrailerURL.Text
                    End If
                End If
            End If
            Movie.SaveNFO(Form1.workingMovieDetails.fileinfo.fullpathandfilename, Form1.workingMovieDetails)
            editsmade = False
            Me.Close()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnWatched_Click( sender As Object,  e As EventArgs) Handles btnWatched.Click
        If btnWatched.Text = "Watched" Then
            btnWatched.Text = "UnWatched"
            btnWatched.BackColor = Color.Red
            btnWatched.Refresh()
            workingmovieedit.fullmoviebody.playcount = "0"
        Else
            btnWatched.Text = "Watched"
            btnWatched.BackColor = Color.LawnGreen
            btnWatched.Refresh()
            workingmovieedit.fullmoviebody.playcount = "1"
        End If
    End Sub

    Private Sub saved()

    End Sub

    Private Sub moviethumb_LoadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles moviethumb.LoadCompleted
        Try
            Dim exists As Boolean = False
            Label16.Text = moviethumb.Image.Width
            Label17.Text = moviethumb.Image.Height
            exists = File.Exists(Form1.workingMovieDetails.fileinfo.posterpath)
            If exists = True Then
                Dim lngSizeOfFile As Decimal
                lngSizeOfFile = FileLen(Form1.workingMovieDetails.fileinfo.posterpath)
                lngSizeOfFile = lngSizeOfFile / 1024
                lngSizeOfFile = Decimal.Round(lngSizeOfFile, 2)
                Label18.Text = lngSizeOfFile & "kB"
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub zoom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles zoom.Click
        Try
            If moviethumb.Height = 213 Then
                moviethumb.Location = New Point(371, 12)
                moviethumb.Height = 518
                moviethumb.Width = 362
            ElseIf moviethumb.Height = 518 Then
                moviethumb.Height = 213
                moviethumb.Width = 163
                moviethumb.Location = New Point(570, 12)
            End If
            Application.DoEvents()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnfanart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfanart.Click
        Try
            Dim t As New frmMovieAltFanart
            If Pref.MultiMonitoEnabled Then
                Dim w As Integer = t.Width
                Dim h As Integer = t.Height
                t.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
                t.StartPosition = FormStartPosition.Manual
                t.Width = w
                t.Height = h
            End If
            t.ShowDialog()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btn_BlankNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_BlankNfo.Click
        Try
            For Each txt As TextBox In textBoxList
                txt.Text = ""
            Next
            actorcb.Items.Clear()
            PremieredDatePicker.Value = Date.Now
            Createdatepicker.Value = Date.now
            filenametxt.Text = workingmovieedit.fileinfo.fullpathandfilename
            Dim newmovie As New FullMovieDetails
            Dim currentfiledetails As StreamDetails = workingmovieedit.filedetails
            workingmovieedit = newmovie
            PictureBox1.Image = Nothing
            workingmovieedit.filedetails = currentfiledetails
            If Not String.IsNullOrEmpty(workingmovieedit.filedetails.Video.DurationInSeconds.Value) Then
                runtimetxt.Text = Math.Round(workingmovieedit.filedetails.Video.DurationInSeconds.Value/60).ToString & "min" 
            Else
                runtimetxt.Text = ""
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Createdatepicker_ValueChanged( sender As System.Object,  e As System.EventArgs) Handles Createdatepicker.ValueChanged
        Dim newdate As String = Format(Createdatepicker.Value, Pref.datePattern).ToString
        If workingmovieedit.fileinfo.createdate <> newdate Then
            datechanged = True
            editsmade = True
        End If
    End Sub

    Private Sub Createdatepicker_DropDown(ByVal sender As Object, ByVal e As EventArgs) Handles Createdatepicker.DropDown
      RemoveHandler Createdatepicker.ValueChanged, AddressOf Createdatepicker_ValueChanged
    End Sub

    Private Sub Createdatepicker_CloseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Createdatepicker.CloseUp
      AddHandler Createdatepicker.ValueChanged, AddressOf Createdatepicker_ValueChanged
      Call Createdatepicker_ValueChanged(sender, EventArgs.Empty)
    End Sub

    Private Sub PremieredDatePicker_ValueChanged( sender As System.Object,  e As System.EventArgs) Handles PremieredDatePicker.ValueChanged
        Dim newdate As String = Format(PremieredDatePicker.Value, Pref.nfoDatePattern).ToString
        If workingmovieedit.fullmoviebody.premiered <> newdate Then
            PremierDateChanged = True
            editsmade = True
        End If
    End Sub

    Private Sub PremieredDatePicker_DropDown(ByVal sender As Object, ByVal e As EventArgs) Handles PremieredDatePicker.DropDown
      RemoveHandler PremieredDatePicker.ValueChanged, AddressOf PremieredDatePicker_ValueChanged
    End Sub

    Private Sub PremieredDatePicker_CloseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PremieredDatePicker.CloseUp
      AddHandler PremieredDatePicker.ValueChanged, AddressOf PremieredDatePicker_ValueChanged
      Call PremieredDatePicker_ValueChanged(sender, EventArgs.Empty)
    End Sub

    Private Sub AnyTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        editsmade = True
    End Sub
    
    Private Sub AnyCbox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        editsmade = True
    End Sub

    Private Sub btnSelectAPlot_Click( sender As Object,  e As EventArgs) Handles btnSelectAPlot.Click
        Dim newplot As String = Form1.GetlistofPlots()
        If Not IsNothing(newplot) Then
            plottxt.Text = newplot
        End If
    End Sub

    Private Sub btn_SeriesLnkSearch_Click(sender As Object, e As EventArgs) Handles btn_SeriesLnkSearch.Click
        Try
            Dim theFolderBrowser As New FolderBrowserDialog
            Dim strfolder As String = ""
            theFolderBrowser.Description = "Please Select a Series Folder, containing tvshow.nfo file"
            theFolderBrowser.ShowNewFolderButton = True
            theFolderBrowser.RootFolder = System.Environment.SpecialFolder.Desktop
            theFolderBrowser.SelectedPath = Pref.lastpath
            If theFolderBrowser.ShowDialog = Windows.Forms.DialogResult.OK Then
                strfolder = (theFolderBrowser.SelectedPath)
                If strfolder <> "" AndAlso Directory.Exists(strfolder) Then
                    Dim nfopath As String = Path.Combine(strfolder, "tvshow.nfo")
                    If File.Exists(nfopath) Then
                        Dim tvshownfo As New TvShow
                        tvshownfo.NfoFilePath = nfopath
                        tvshownfo = nfoFunction.tvshow_NfoLoad(nfopath)
                        If tvshownfo.Title.Value <> "" Then
                            tb_SeriesLnk.Text = tvshownfo.Title.Value
                            editsmade = True
                        End If
                        tvshownfo = Nothing
                    Else
                        MsgBox("Selected folder does not contain a tvshow.nfo")
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub tb_SeriesLnk_KeyDown(sender As Object, e As KeyEventArgs) Handles tb_SeriesLnk.KeyDown
        editsmade = True
    End Sub

    Private Sub btn_SeriesList_Click(sender As Object, e As EventArgs) Handles btn_SeriesList.Click
        Dim frm As New frmSeriesList
        frm.Init()
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            tb_SeriesLnk.Text = frm.Selected
            editsmade = True
        End If
        frm.Dispose()
        frm = Nothing
    End Sub
End Class