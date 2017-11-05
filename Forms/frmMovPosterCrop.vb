Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing.BitMap
Imports System.Text
Imports System.Windows.Forms

Public Class frmMovPosterCrop
    Implements IDisposable
    Private rect As UserRect
    Public img As Bitmap
    Public newimg As Bitmap
    Public cropmode As String
    Public title As String

    Public Sub New()
        InitializeComponent()
        'If cropMode = "fanart" Then
        '    img = Form1.PictureBoxAssignedMoviePoster.Image
        '    tb_cropmovtitle.Text = Form1.workingMovieDetails.fullmoviebody.title
        'ElseIf Form1.cropMode = "mvscreenshot" Then
        '    img = New Bitmap(Form1.UcMusicVideo1.pcBxScreenshot.Tag.ToString)
        '    'img = Form1.UcMusicVideo1.pcBxScreenshot.Image
        '    tb_cropmovtitle.Text = Form1.UcMusicVideo1.txtTitle.Text
        '    'Width = Math.Ceiling((Height/img.Height)*img.Width)
        'End If
        'tb_cropmovtitle.Text = title
        'SizePicBox()
        'PicBox.Image = img
        'rect = New UserRect(New Rectangle(80, 10, 250, 400))
        'rect.dodraw = True
        'rect.SetPictureBox(Me.PicBox)
    End Sub

    Private Sub btn_CropAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CropAccept.Click
        Dim zm As Double = (img.Height / PicBox.Height)
        'If Form1.cropMode = "movieposter" Then
        '    Form1.btnMoviePosterResetImage.Enabled = True
        '    Form1.btnMoviePosterSaveCroppedImage.Enabled = True
        '    Form1.PictureBoxAssignedMoviePoster.Image = rect.GetCropped(zm)
        '    Form1.lblCurrentLoadedPoster.Text = "Width: " & Form1.PictureBoxAssignedMoviePoster.Image.Width.ToString & "  Height: " & Form1.PictureBoxAssignedMoviePoster.Image.Height.ToString
        'ElseIf Form1.cropMode = "mvscreenshot" Then
        '    Form1.UcMusicVideo1.btnSaveCrop.Enabled = True
        '    Form1.UcMusicVideo1.btnCropReset.Enabled = True
        '    Form1.UcMusicVideo1.pcBxScreenshot.Image = rect.GetCropped(zm)
        'End If
        newimg = rect.GetCropped(zm, img)
        PicBox.Dispose()
        Me.Close()
    End Sub

    Private Sub btn_CropCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CropCancel.Click
        Me.Close()
    End Sub

    Private Sub SizePicBox()
        Dim newW As Integer = Math.Ceiling((PicBox.Height/img.Height) * img.Width) 'Math.Ceiling(img.Width / (img.Height / PicBox.Height))
        PicBox.Width = newW
        Width = PicBox.Width +200
        'Dim newxpos = Math.Ceiling(245 - (newW / 2))
        'PicBox.Location = New Point(newxpos, 15)
    End Sub

    Public Sub Setup()
        tb_cropmovtitle.Text = title
        SizePicBox()
        PicBox.Image = img
        Dim cropwidth As Integer = If(cropmode = "fanart", PicBox.Width, Math.Floor((PicBox.Height/9) * 6))
        rect = New UserRect(New Rectangle(0, 0, PicBox.Width, PicBox.Height)) '(80, 10, 250, 400))
        rect.dodraw = True
        rect.SetPictureBox(Me.PicBox)
    End Sub

    Private Sub frmMovPosterCrop_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then btn_CropCancel.PerformClick()
    End Sub

    Protected Overridable Overloads Sub Dispose(disposing As Boolean)
        If img IsNot Nothing Then
            img.Dispose()
            img = Nothing
        End If
        If rect IsNot Nothing Then
            rect.Dispose()
            rect = Nothing
        End If
    End Sub 'Dispose
End Class