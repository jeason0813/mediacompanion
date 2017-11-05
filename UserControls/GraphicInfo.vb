'Imports System.IO
Imports Alphaleonis.Win32.Filesystem

Public Class GraphicInfo

    Public xPos As Integer
    Public yPos As Integer
    Public fanartRatio As Double
    Public grFanart As Graphics
    Public iPadding As Integer = 2

    Public Sub OverlayInfo(ByRef picbxFanart As PictureBox, ByVal sRating As String, ByVal flags As List(Of KeyValuePair(Of String, String)), Optional ByVal Series As Boolean = False)
        'OVERLAY RATING STARS
        Dim iRating As Single
        Dim bmFanart As New Bitmap(picbxFanart.Image)
        grFanart = Graphics.FromImage(bmFanart)
        Dim fanartWidth  As Integer = picbxFanart.ClientRectangle.Width
        Dim fanartHeight As Integer = picbxFanart.ClientRectangle.Height
        fanartRatio = bmFanart.Height / fanartHeight

        If Pref.DisplayRatingOverlay Then
            If Not sRating.Contains(Form1.DecimalSeparator) Then
                If Form1.DecimalSeparator = "," Then
                    sRating = sRating.Replace(".", ",")
                Else
                    sRating = sRating.Replace(",", ".")
                End If
            End If
            If Not sRating = "" Or Single.TryParse(sRating, iRating) Then
            iRating = Math.Min(iRating, 10)
            sRating = sRating.FormatRating
            Dim Brush As New SolidBrush(Color.Black)
            Dim drawFont As New Font("Segoe UI", 14, FontStyle.Bold)
            Dim bmStars As New Bitmap(picbxGraphicInfo.Image)
            Dim StarsWidth As Integer = 39 + (Convert.ToInt16((iRating * 10) * ((bmStars.Width - 39) / 100)))
            Dim rectStars As New Rectangle(0, 0, StarsWidth, bmStars.Height)
            Dim rectFanart As New Rectangle(0, 0, StarsWidth * fanartRatio, bmStars.Height * fanartRatio)
            Dim grStars As Graphics = Graphics.FromImage(bmStars)
            grStars.DrawString(sRating, drawFont, Brush, If(sRating.Length > 2, 0, 8), 2)
            grFanart.DrawImage(bmStars, rectFanart, rectStars, GraphicsUnit.Pixel)
            End If
        End If
        
        xPos = iPadding * fanartRatio
        yPos = (fanartHeight - (32 + iPadding)) * fanartRatio
        
        'OVERLAY VIDEO FLAGS
        If Pref.DisplayMediainfoOverlay AndAlso Not Series Then
            For Each item In flags
                Try
                    If Not String.IsNullOrEmpty(item.Value) Then    'Catch any empty values for selected flags
                        Dim flagName As String
                        Dim flagPath As String
                        Dim TrueAspect As Boolean = True
                        If item.Key.IndexOf("aspect") > -1 Then TrueAspect = If(item.Value= "1.78" OrElse item.Value = "1.33", True, False)

                        If item.Key = "lang" OrElse item.Key = "lang_notdefault" OrElse item.Key = "sublang" OrElse Not TrueAspect Then
                            flagPath = Path.Combine(Pref.applicationPath, "Resources\video_flags\" + item.Key + ".png")
                        ElseIf item.Key = "folderSize" OrElse item.Key = "NumVideoBits" Then
                            flagPath = Path.Combine(Pref.applicationPath, "Resources\video_flags\long_blank.png")
                        Else
                            flagName = String.Format("media_{0}_{1}{2}.png", item.Key, item.Value, If(item.Value = "MPEG-4","visual","") )
                            flagPath = Path.Combine(Pref.applicationPath, String.Format("Resources\video_flags\{0}", flagName.ToLower))
                        End If
                        
                        Dim bmflagStream As New IO.MemoryStream(My.Computer.FileSystem.ReadAllBytes(flagPath))
                        Dim bmFlag       As New Bitmap(bmflagStream)
                        Dim rectFlag     As New Rectangle(0, 0, bmFlag.Width, bmFlag.Height)
                        Dim recFanart    As New Rectangle(xPos, yPos, bmFlag.Width * fanartRatio, bmFlag.Height * fanartRatio)

                        If item.Key = "lang" OrElse item.Key = "lang_notdefault" OrElse Not TrueAspect Then
                            Dim sDisplayText As String = item.Value
                            If sDisplayText.ToLower = "error" Then Continue For
                            Dim FontSize = 19
                            Dim font as new Font("Tahoma", FontSize)    'create a font to write the values in the bitmap
                            'use the bitmap to draw
                            Dim graphic = Graphics.FromImage(bmFlag)
                            Dim gradient = 224
                            Dim brush as new SolidBrush( Color.FromArgb(gradient, gradient, gradient) )
                            Dim sf As new StringFormat()        'draw the string including the value
                            sf.LineAlignment = StringAlignment.Center
                            sf.Alignment = StringAlignment.Center
                            graphic.DrawString(sDisplayText, font, brush, rectFlag, sf)
                        End If
                        If item.Key = "sublang" Then
                            Dim sDisplayText As String = "sublang:" & vbCrLf & item.Value
                            If sDisplayText.ToLower.Contains("error") Then Continue For
                            Dim FontSize = 14
                            Dim font as new Font("Tahoma", FontSize)    'create a font to write the values in the bitmap
                            'use the bitmap to draw
                            Dim graphic = Graphics.FromImage(bmFlag)
                            Dim gradient = 245
                            Dim brush as new SolidBrush( Color.FromArgb(gradient, gradient, gradient) )
                            Dim sf As new StringFormat()        'draw the string including the value
                            sf.LineAlignment = StringAlignment.Center
                            sf.Alignment = StringAlignment.Center
                            graphic.DrawString(sDisplayText, font, brush, rectFlag, sf)
                        End If
                        If item.Key = "NumVideoBits" OrElse item.Key = "folderSize" Then
                            If item.Key = "NumVideoBits" AndAlso item.Value.ToInt < 9 Then Continue For
                            If item.Key = "folderSize" AndAlso (Not Pref.DisplayMediaInfoFolderSize OrElse item.Value.ToInt < 0) Then Continue For
                            Dim str As String = If(item.Key = "folderSize", String.Format("{0:00.0}GB",item.Value), String.Format("HDR{0}",item.Value))
                            Dim offSet   = str.Length - 2
                            Dim FontSize = 18 - ((offSet-4)*2)
                            Dim font as new Font("Tahoma", FontSize)                    'create a font to write the values in the bitmap
                            Dim origin as new PointF(0, offSet)                         'origin for the string
                            Dim graphic = Graphics.FromImage(bmFlag)                    'use the bitmap to draw
                            Dim gradient = 224
                            Dim brush as new SolidBrush( Color.FromArgb(gradient, gradient, gradient) )
                            graphic.DrawString(str, font, brush, origin)        'draw the string including the valu
                        End If
                        grFanart.DrawImage(bmFlag, recFanart, rectFlag, GraphicsUnit.Pixel)
                        xPos += (bmFlag.Width + iPadding) * fanartRatio
                    End If
                Catch ex As Exception  'If the derived filname doesn't exist, we'll just ignore it (this will happen if aspect comes back as empty string).
                End Try
            Next
            'If NumVideoBits > 8 Then DisplayMediaInfo( String.Format("HDR{0}",NumVideoBits) )
        End If
        'If Pref.DisplayMediaInfoFolderSize And folderSize > -1 Then DisplayMediaInfo( String.Format("{0:00.0}GB",folderSize) )
        picbxFanart.Image = bmFanart
    End Sub

    'Public Sub DisplayMediaInfo(str As String)
    '    Dim flagPath As String = Path.Combine(Pref.applicationPath, "Resources\video_flags\long_blank.png")
    '    Dim bmflagStream As New IO.MemoryStream(My.Computer.FileSystem.ReadAllBytes(flagPath))
    '    Dim bmFlag As Bitmap = New Bitmap(bmflagStream)
    '    Dim offSet   = str.Length - 2
    '    Dim FontSize = 18 - ((offSet-4)*2)
    '    Dim font as new Font("Tahoma", FontSize)                    'create a font to write the values in the bitmap
    '    Dim origin as new PointF(0, offSet)                         'origin for the string
    '    Dim graphic = Graphics.FromImage(bmFlag)                    'use the bitmap to draw
    '    Dim gradient = 224
    '    Dim brush as new SolidBrush( Color.FromArgb(gradient, gradient, gradient) )
    '    graphic.DrawString(str, font, brush, origin)        'draw the string including the value
    '    Dim rectFlag As New Rectangle(0, 0, bmFlag.Width, bmFlag.Height)
    '    Dim recFanart As New Rectangle(xPos, yPos, bmFlag.Width * fanartRatio, bmFlag.Height * fanartRatio)
    '    grFanart.DrawImage(bmFlag, recFanart, rectFlag, GraphicsUnit.Pixel)
    '    xPos += (bmFlag.Width + iPadding) * fanartRatio
    'End Sub

End Class
