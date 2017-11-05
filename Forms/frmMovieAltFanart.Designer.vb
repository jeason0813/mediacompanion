<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovieAltFanart
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMovieAltFanart))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnthumbbrowse = New System.Windows.Forms.Button()
        Me.btncancelgetthumburl = New System.Windows.Forms.Button()
        Me.btngetthumb = New System.Windows.Forms.Button()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_SaveSelected = New System.Windows.Forms.Button()
        Me.btn_UrlOrBrowse = New System.Windows.Forms.Button()
        Me.openFD = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.AutoScroll = True
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(2, 243)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(429, 245)
        Me.Panel1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(281, 25)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "No Local Fanart is Available"
        Me.Label2.Visible = False
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.btnthumbbrowse)
        Me.Panel3.Controls.Add(Me.btncancelgetthumburl)
        Me.Panel3.Controls.Add(Me.btngetthumb)
        Me.Panel3.Controls.Add(Me.TextBox5)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Location = New System.Drawing.Point(133, 113)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(481, 98)
        Me.Panel3.TabIndex = 92
        Me.Panel3.Visible = False
        '
        'btnthumbbrowse
        '
        Me.btnthumbbrowse.Location = New System.Drawing.Point(376, 7)
        Me.btnthumbbrowse.Name = "btnthumbbrowse"
        Me.btnthumbbrowse.Size = New System.Drawing.Size(89, 23)
        Me.btnthumbbrowse.TabIndex = 4
        Me.btnthumbbrowse.Text = "Browse"
        Me.btnthumbbrowse.UseVisualStyleBackColor = True
        '
        'btncancelgetthumburl
        '
        Me.btncancelgetthumburl.Location = New System.Drawing.Point(376, 65)
        Me.btncancelgetthumburl.Name = "btncancelgetthumburl"
        Me.btncancelgetthumburl.Size = New System.Drawing.Size(89, 23)
        Me.btncancelgetthumburl.TabIndex = 3
        Me.btncancelgetthumburl.Text = "Cancel"
        Me.btncancelgetthumburl.UseVisualStyleBackColor = True
        '
        'btngetthumb
        '
        Me.btngetthumb.Location = New System.Drawing.Point(376, 36)
        Me.btngetthumb.Name = "btngetthumb"
        Me.btngetthumb.Size = New System.Drawing.Size(89, 23)
        Me.btngetthumb.TabIndex = 2
        Me.btngetthumb.Text = "Set Thumb"
        Me.btngetthumb.UseVisualStyleBackColor = True
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
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(16, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(242, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Enter URL or Browse PC For Thumbnail"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.AutoScroll = True
        Me.Panel2.AutoScrollMargin = New System.Drawing.Size(0, 5)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Location = New System.Drawing.Point(2, 6)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(782, 231)
        Me.Panel2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(437, 243)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(336, 144)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btn_SaveSelected
        '
        Me.btn_SaveSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_SaveSelected.Location = New System.Drawing.Point(686, 455)
        Me.btn_SaveSelected.Name = "btn_SaveSelected"
        Me.btn_SaveSelected.Size = New System.Drawing.Size(89, 23)
        Me.btn_SaveSelected.TabIndex = 4
        Me.btn_SaveSelected.Text = "Save Selected"
        Me.btn_SaveSelected.UseVisualStyleBackColor = True
        '
        'btn_UrlOrBrowse
        '
        Me.btn_UrlOrBrowse.Location = New System.Drawing.Point(686, 406)
        Me.btn_UrlOrBrowse.Name = "btn_UrlOrBrowse"
        Me.btn_UrlOrBrowse.Size = New System.Drawing.Size(89, 23)
        Me.btn_UrlOrBrowse.TabIndex = 93
        Me.btn_UrlOrBrowse.Text = "URL or Browse"
        Me.btn_UrlOrBrowse.UseVisualStyleBackColor = True
        '
        'openFD
        '
        Me.openFD.FileName = "OpenFileDialog1"
        '
        'frmMovieFanart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 490)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btn_UrlOrBrowse)
        Me.Controls.Add(Me.btn_SaveSelected)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmMovieFanart"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "moviefanart"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_SaveSelected As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnthumbbrowse As System.Windows.Forms.Button
    Friend WithEvents btncancelgetthumburl As System.Windows.Forms.Button
    Friend WithEvents btngetthumb As System.Windows.Forms.Button
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btn_UrlOrBrowse As System.Windows.Forms.Button
    Friend WithEvents openFD As System.Windows.Forms.OpenFileDialog
End Class
