<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovieAltPosterArt
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMovieAltPosterArt))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnSourceTMDB = New System.Windows.Forms.Button()
        Me.btnSourceMPDB = New System.Windows.Forms.Button()
        Me.btnSourceIMDB = New System.Windows.Forms.Button()
        Me.btnSourceIMPA = New System.Windows.Forms.Button()
        Me.btnSaveSmall = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSaveBig = New System.Windows.Forms.Button()
        Me.btnScrollPrev = New System.Windows.Forms.Button()
        Me.btnScrollNext = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnthumbbrowse = New System.Windows.Forms.Button()
        Me.btncancelgetthumburl = New System.Windows.Forms.Button()
        Me.btngetthumb = New System.Windows.Forms.Button()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnSourceManual = New System.Windows.Forms.Button()
        Me.openFD = New System.Windows.Forms.OpenFileDialog()
        Me.Panel3.SuspendLayout
        Me.SuspendLayout
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Panel1.Location = New System.Drawing.Point(2, 240)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(254, 246)
        Me.Panel1.TabIndex = 0
        '
        'btnSourceTMDB
        '
        Me.btnSourceTMDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSourceTMDB.Location = New System.Drawing.Point(278, 295)
        Me.btnSourceTMDB.Name = "btnSourceTMDB"
        Me.btnSourceTMDB.Size = New System.Drawing.Size(90, 23)
        Me.btnSourceTMDB.TabIndex = 2
        Me.btnSourceTMDB.Text = "TMdB"
        Me.ToolTip1.SetToolTip(Me.btnSourceTMDB, "Show available Posters from"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"http://themoviedb.org")
        Me.btnSourceTMDB.UseVisualStyleBackColor = true
        '
        'btnSourceMPDB
        '
        Me.btnSourceMPDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSourceMPDB.Location = New System.Drawing.Point(374, 295)
        Me.btnSourceMPDB.Name = "btnSourceMPDB"
        Me.btnSourceMPDB.Size = New System.Drawing.Size(90, 23)
        Me.btnSourceMPDB.TabIndex = 3
        Me.btnSourceMPDB.Text = "MoviePosterDB"
        Me.ToolTip1.SetToolTip(Me.btnSourceMPDB, "Show available Posters from"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"http://www.movieposterdb.com/")
        Me.btnSourceMPDB.UseVisualStyleBackColor = true
        '
        'btnSourceIMDB
        '
        Me.btnSourceIMDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSourceIMDB.Location = New System.Drawing.Point(470, 295)
        Me.btnSourceIMDB.Name = "btnSourceIMDB"
        Me.btnSourceIMDB.Size = New System.Drawing.Size(90, 23)
        Me.btnSourceIMDB.TabIndex = 4
        Me.btnSourceIMDB.Text = "IMDB"
        Me.ToolTip1.SetToolTip(Me.btnSourceIMDB, "Show available Posters from"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"http://www.imdb.com/")
        Me.btnSourceIMDB.UseVisualStyleBackColor = true
        '
        'btnSourceIMPA
        '
        Me.btnSourceIMPA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSourceIMPA.Location = New System.Drawing.Point(566, 295)
        Me.btnSourceIMPA.Name = "btnSourceIMPA"
        Me.btnSourceIMPA.Size = New System.Drawing.Size(90, 23)
        Me.btnSourceIMPA.TabIndex = 5
        Me.btnSourceIMPA.Text = "IMP Awards"
        Me.ToolTip1.SetToolTip(Me.btnSourceIMPA, "Show available Posters from"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"http://www.impawards.com/")
        Me.btnSourceIMPA.UseVisualStyleBackColor = true
        '
        'btnSaveSmall
        '
        Me.btnSaveSmall.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSaveSmall.Location = New System.Drawing.Point(713, 461)
        Me.btnSaveSmall.Name = "btnSaveSmall"
        Me.btnSaveSmall.Size = New System.Drawing.Size(71, 23)
        Me.btnSaveSmall.TabIndex = 6
        Me.btnSaveSmall.Text = "Save Small"
        Me.btnSaveSmall.UseVisualStyleBackColor = true
        Me.btnSaveSmall.Visible = false
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(333, 279)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(359, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Use the Buttons Below to see the available Movie Posters from that source"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(259, 455)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Current Local Poster"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = true
        Me.Label3.Location = New System.Drawing.Point(360, 376)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(315, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Double Click a Poster for a larger view of the full resolution image."
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = true
        Me.Label4.Location = New System.Drawing.Point(344, 360)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(348, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "For TMDb and IMP Awards the images shown are lower quality previews"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = true
        Me.Label5.Location = New System.Drawing.Point(322, 404)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(402, 39)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = resources.GetString("Label5.Text")
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = true
        Me.Label6.Location = New System.Drawing.Point(259, 471)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(39, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Label6"
        '
        'btnSaveBig
        '
        Me.btnSaveBig.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSaveBig.Location = New System.Drawing.Point(636, 461)
        Me.btnSaveBig.Name = "btnSaveBig"
        Me.btnSaveBig.Size = New System.Drawing.Size(71, 23)
        Me.btnSaveBig.TabIndex = 12
        Me.btnSaveBig.Text = "Save Big"
        Me.btnSaveBig.UseVisualStyleBackColor = true
        Me.btnSaveBig.Visible = false
        '
        'btnScrollPrev
        '
        Me.btnScrollPrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnScrollPrev.Location = New System.Drawing.Point(315, 240)
        Me.btnScrollPrev.Name = "btnScrollPrev"
        Me.btnScrollPrev.Size = New System.Drawing.Size(100, 23)
        Me.btnScrollPrev.TabIndex = 13
        Me.btnScrollPrev.Text = "Previous"
        Me.btnScrollPrev.UseVisualStyleBackColor = true
        Me.btnScrollPrev.Visible = false
        '
        'btnScrollNext
        '
        Me.btnScrollNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnScrollNext.Location = New System.Drawing.Point(613, 240)
        Me.btnScrollNext.Name = "btnScrollNext"
        Me.btnScrollNext.Size = New System.Drawing.Size(100, 23)
        Me.btnScrollNext.TabIndex = 14
        Me.btnScrollNext.Text = "Next"
        Me.btnScrollNext.UseVisualStyleBackColor = true
        Me.btnScrollNext.Visible = false
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = true
        Me.Label7.Location = New System.Drawing.Point(421, 245)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Label7"
        Me.Label7.Visible = false
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(632, 332)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(36, 20)
        Me.TextBox1.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = true
        Me.Label8.Location = New System.Drawing.Point(371, 335)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(255, 13)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Maximum Number of Images to shown on each page"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.btnthumbbrowse)
        Me.Panel3.Controls.Add(Me.btncancelgetthumburl)
        Me.Panel3.Controls.Add(Me.btngetthumb)
        Me.Panel3.Controls.Add(Me.TextBox5)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Location = New System.Drawing.Point(315, 332)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(466, 91)
        Me.Panel3.TabIndex = 91
        Me.Panel3.Visible = false
        '
        'btnthumbbrowse
        '
        Me.btnthumbbrowse.Location = New System.Drawing.Point(378, 7)
        Me.btnthumbbrowse.Name = "btnthumbbrowse"
        Me.btnthumbbrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnthumbbrowse.TabIndex = 4
        Me.btnthumbbrowse.Text = "Browse"
        Me.btnthumbbrowse.UseVisualStyleBackColor = true
        '
        'btncancelgetthumburl
        '
        Me.btncancelgetthumburl.Location = New System.Drawing.Point(378, 65)
        Me.btncancelgetthumburl.Name = "btncancelgetthumburl"
        Me.btncancelgetthumburl.Size = New System.Drawing.Size(75, 23)
        Me.btncancelgetthumburl.TabIndex = 3
        Me.btncancelgetthumburl.Text = "Cancel"
        Me.btncancelgetthumburl.UseVisualStyleBackColor = true
        '
        'btngetthumb
        '
        Me.btngetthumb.Location = New System.Drawing.Point(378, 36)
        Me.btngetthumb.Name = "btngetthumb"
        Me.btngetthumb.Size = New System.Drawing.Size(75, 23)
        Me.btngetthumb.TabIndex = 2
        Me.btngetthumb.Text = "Set Thumb"
        Me.btngetthumb.UseVisualStyleBackColor = true
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(19, 38)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(343, 20)
        Me.TextBox5.TabIndex = 1
        '
        'Label9
        '
        Me.Label9.AutoSize = true
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label9.Location = New System.Drawing.Point(16, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(242, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Enter URL or Browse PC For Thumbnail"
        '
        'btnSourceManual
        '
        Me.btnSourceManual.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.btnSourceManual.Location = New System.Drawing.Point(662, 295)
        Me.btnSourceManual.Name = "btnSourceManual"
        Me.btnSourceManual.Size = New System.Drawing.Size(90, 23)
        Me.btnSourceManual.TabIndex = 92
        Me.btnSourceManual.Text = "URL or Browse"
        Me.btnSourceManual.UseVisualStyleBackColor = true
        '
        'openFD
        '
        Me.openFD.FileName = "OpenFileDialog1"
        '
        'frmCoverArt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 497)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnScrollNext)
        Me.Controls.Add(Me.btnScrollPrev)
        Me.Controls.Add(Me.btnSaveBig)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSaveSmall)
        Me.Controls.Add(Me.btnSourceIMPA)
        Me.Controls.Add(Me.btnSourceIMDB)
        Me.Controls.Add(Me.btnSourceMPDB)
        Me.Controls.Add(Me.btnSourceTMDB)
        Me.Controls.Add(Me.btnSourceManual)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(795, 524)
        Me.Name = "frmCoverArt"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "coverart"
        Me.Panel3.ResumeLayout(false)
        Me.Panel3.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnSourceTMDB As System.Windows.Forms.Button
    Friend WithEvents btnSourceMPDB As System.Windows.Forms.Button
    Friend WithEvents btnSourceIMDB As System.Windows.Forms.Button
    Friend WithEvents btnSourceIMPA As System.Windows.Forms.Button
    Friend WithEvents btnSaveSmall As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSaveBig As System.Windows.Forms.Button
    Friend WithEvents btnScrollPrev As System.Windows.Forms.Button
    Friend WithEvents btnScrollNext As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnthumbbrowse As System.Windows.Forms.Button
    Friend WithEvents btncancelgetthumburl As System.Windows.Forms.Button
    Friend WithEvents btngetthumb As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnSourceManual As System.Windows.Forms.Button
    Friend WithEvents openFD As System.Windows.Forms.OpenFileDialog
End Class
