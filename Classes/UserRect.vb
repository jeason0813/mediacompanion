
Imports System.Collections.Generic
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports Alphaleonis.Win32.Filesystem

Public Class UserRect
    Implements IDisposable
	Private mPictureBox As PictureBox
	Public rect As Rectangle
	Public allowDeformingDuringMovement As Boolean = False
    Public dodraw As Boolean = False
	Private mIsClick As Boolean = False
	Private mMove As Boolean = False
	Private oldX As Integer
	Private oldY As Integer
	Private sizeNodeRect As Integer = 5
	Private mBmp As Bitmap = Nothing
	Private nodeSelected As PosSizableRect = PosSizableRect.None
	Private angle As Integer = 30

	Private Enum PosSizableRect
		UpMiddle
		LeftMiddle
		LeftBottom
		LeftUp
		RightUp
		RightMiddle
		RightBottom
		BottomMiddle
		None

	End Enum

	Public Sub New(r As Rectangle)
		rect = r
		mIsClick = False
	End Sub

	Public Sub Draw(g As Graphics)
		g.DrawRectangle(New Pen(Color.Red), rect)

		For Each pos As PosSizableRect In [Enum].GetValues(GetType(PosSizableRect))
			g.DrawRectangle(New Pen(Color.Red), GetRect(pos))
		Next
	End Sub

	Public Sub SetBitmapFile(filename As String)
        Dim ms As IO.MemoryStream = New IO.MemoryStream()
        Using r As IO.Filestream = File.Open(filename, IO.FileMode.Open)
            r.CopyTo(ms)
        End Using
		Me.mBmp = New Bitmap(ms)
        ms.Dispose()
        frmMovPosterCrop.PicBox.image = Me.mBmp 
	End Sub

	Public Sub SetBitmap(bmp As Bitmap)
		Me.mBmp = bmp
	End Sub

	Public Sub SetPictureBox(p As PictureBox)  
		Me.mPictureBox = p
		AddHandler mPictureBox.MouseDown, AddressOf mPictureBox_MouseDown
		AddHandler mPictureBox.MouseUp, AddressOf mPictureBox_MouseUp
		AddHandler mPictureBox.MouseMove, AddressOf mPictureBox_MouseMove
		AddHandler mPictureBox.Paint, AddressOf mPictureBox_Paint
	End Sub
    
    Public Function GetCropped(ByVal zm As Double, ByVal origimg As Bitmap) As Bitmap
        
        'Dim zm As Double = frmMovPosterCrop.zm
        Dim sx As Integer = rect.X * zm
        Dim sy As Integer = rect.Y * zm
        Dim sw As Integer = rect.Width * zm
        Dim sh As Integer = rect.Height * zm
        
        Dim rect1 As Rectangle = New Rectangle(sx, sy, sw, sh)
        Dim OriginalImage As Bitmap = origimg  'New Bitmap(frmMovPosterCrop.img) ', mPictureBox.Width, mPictureBox.Height)
        Dim _img As New Bitmap(sw, sh) ' for cropinf image
        Dim g As Graphics = Graphics.FromImage(_img) ' create graphics

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

        'set image attributes
        g.DrawImage(OriginalImage, 0, 0, rect1, GraphicsUnit.Pixel)
        Return _img
    End Function

	Private Sub mPictureBox_Paint(sender As Object, e As PaintEventArgs)

		Try
            If dodraw Then
			    Draw(e.Graphics)
            End If
		Catch exp As Exception
			System.Console.WriteLine(exp.Message)
		End Try

	End Sub

	Private Sub mPictureBox_MouseDown(sender As Object, e As MouseEventArgs)
		mIsClick = True

		nodeSelected = PosSizableRect.None
		nodeSelected = GetNodeSelectable(e.Location)

		If rect.Contains(New Point(e.X, e.Y)) Then
			mMove = True
		End If
		oldX = e.X
		oldY = e.Y
	End Sub

	Private Sub mPictureBox_MouseUp(sender As Object, e As MouseEventArgs)
		mIsClick = False
		mMove = False
	End Sub

	Private Sub mPictureBox_MouseMove(sender As Object, e As MouseEventArgs)
		ChangeCursor(e.Location)
		If mIsClick = False Then
			Return
		End If

		Dim backupRect As Rectangle = rect

		Select Case nodeSelected
			Case PosSizableRect.LeftUp
				rect.X += e.X - oldX
				rect.Width -= e.X - oldX
				rect.Y += e.Y - oldY
				rect.Height -= e.Y - oldY
				Exit Select
			Case PosSizableRect.LeftMiddle
				rect.X += e.X - oldX
				rect.Width -= e.X - oldX
				Exit Select
			Case PosSizableRect.LeftBottom
				rect.Width -= e.X - oldX
				rect.X += e.X - oldX
				rect.Height += e.Y - oldY
				Exit Select
			Case PosSizableRect.BottomMiddle
				rect.Height += e.Y - oldY
				Exit Select
			Case PosSizableRect.RightUp
				rect.Width += e.X - oldX
				rect.Y += e.Y - oldY
				rect.Height -= e.Y - oldY
				Exit Select
			Case PosSizableRect.RightBottom
				rect.Width += e.X - oldX
				rect.Height += e.Y - oldY
				Exit Select
			Case PosSizableRect.RightMiddle
				rect.Width += e.X - oldX
				Exit Select

			Case PosSizableRect.UpMiddle
				rect.Y += e.Y - oldY
				rect.Height -= e.Y - oldY
				Exit Select
			Case Else

				If mMove Then
					rect.X = rect.X + e.X - oldX
					rect.Y = rect.Y + e.Y - oldY
				End If
				Exit Select
		End Select
		oldX = e.X
		oldY = e.Y

		If rect.Width < 5 OrElse rect.Height < 5 Then
			rect = backupRect
		End If

		TestIfRectInsideArea()

		mPictureBox.Invalidate()
	End Sub

	Private Sub TestIfRectInsideArea()
		' Test if rectangle still inside the area.
		If rect.X < 0 Then
			rect.X = 0
		End If
		If rect.Y < 0 Then
			rect.Y = 0
		End If
		If rect.Width <= 0 Then
			rect.Width = 1
		End If
		If rect.Height <= 0 Then
			rect.Height = 1
		End If

		If rect.X + rect.Width > mPictureBox.Width Then
			rect.Width = mPictureBox.Width - rect.X - 1
			' -1 to be still show 
			If allowDeformingDuringMovement = False Then
				mIsClick = False
			End If
		End If
		If rect.Y + rect.Height > mPictureBox.Height Then
			rect.Height = mPictureBox.Height - rect.Y - 1
			' -1 to be still show 
			If allowDeformingDuringMovement = False Then
				mIsClick = False
			End If
		End If
	End Sub

	Private Function CreateRectSizableNode(x As Integer, y As Integer) As Rectangle
		Return New Rectangle(x - sizeNodeRect / 2, y - sizeNodeRect / 2, sizeNodeRect, sizeNodeRect)
	End Function

	Private Function GetRect(p As PosSizableRect) As Rectangle
		Select Case p
			Case PosSizableRect.LeftUp
				Return CreateRectSizableNode(rect.X, rect.Y)

			Case PosSizableRect.LeftMiddle
				Return CreateRectSizableNode(rect.X, rect.Y + +rect.Height / 2)

			Case PosSizableRect.LeftBottom
				Return CreateRectSizableNode(rect.X, rect.Y + rect.Height)

			Case PosSizableRect.BottomMiddle
				Return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y + rect.Height)

			Case PosSizableRect.RightUp
				Return CreateRectSizableNode(rect.X + rect.Width, rect.Y)

			Case PosSizableRect.RightBottom
				Return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height)

			Case PosSizableRect.RightMiddle
				Return CreateRectSizableNode(rect.X + rect.Width, rect.Y + rect.Height / 2)

			Case PosSizableRect.UpMiddle
				Return CreateRectSizableNode(rect.X + rect.Width / 2, rect.Y)
			Case Else
				Return New Rectangle()
		End Select
	End Function

	Private Function GetNodeSelectable(p As Point) As PosSizableRect
		For Each r As PosSizableRect In [Enum].GetValues(GetType(PosSizableRect))
			If GetRect(r).Contains(p) Then
				Return r
			End If
		Next
		Return PosSizableRect.None
	End Function

	Private Sub ChangeCursor(p As Point)
		mPictureBox.Cursor = GetCursor(GetNodeSelectable(p))
	End Sub

	Private Function GetCursor(p As PosSizableRect) As Cursor
		Select Case p
			Case PosSizableRect.LeftUp
				Return Cursors.SizeNWSE

			Case PosSizableRect.LeftMiddle
				Return Cursors.SizeWE

			Case PosSizableRect.LeftBottom
				Return Cursors.SizeNESW

			Case PosSizableRect.BottomMiddle
				Return Cursors.SizeNS

			Case PosSizableRect.RightUp
				Return Cursors.SizeNESW

			Case PosSizableRect.RightBottom
				Return Cursors.SizeNWSE

			Case PosSizableRect.RightMiddle
				Return Cursors.SizeWE

			Case PosSizableRect.UpMiddle
				Return Cursors.SizeNS
			Case Else
				Return Cursors.[Default]
		End Select
	End Function

    Protected Overridable Overloads Sub Dispose(disposing As Boolean)

        If mBmp IsNot Nothing Then
            mBmp.Dispose()
            mBmp = Nothing
        End If
        If mPictureBox IsNot Nothing Then
            mPictureBox.Dispose()
            mPictureBox = Nothing
        End If
    End Sub 'Dispose

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub 'Dispose

End Class
