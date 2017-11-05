<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovPosterCrop
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    '<System.Diagnostics.DebuggerNonUserCode()> _
    'Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    '    Try
    '        If disposing AndAlso components IsNot Nothing Then
    '            components.Dispose()
    '        End If
    '    Finally
    '        MyBase.Dispose(disposing)
    '    End Try
    'End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PicBox = New System.Windows.Forms.PictureBox()
        Me.btn_CropAccept = New System.Windows.Forms.Button()
        Me.btn_CropCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tb_cropmovtitle = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.PicBox,System.ComponentModel.ISupportInitialize).BeginInit
        Me.GroupBox1.SuspendLayout
        Me.Panel1.SuspendLayout
        Me.TableLayoutPanel1.SuspendLayout
        Me.Panel2.SuspendLayout
        Me.SuspendLayout
        '
        'PicBox
        '
        Me.PicBox.Location = New System.Drawing.Point(23, 16)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(321, 480)
        Me.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicBox.TabIndex = 0
        Me.PicBox.TabStop = false
        '
        'btn_CropAccept
        '
        Me.btn_CropAccept.Location = New System.Drawing.Point(192, 22)
        Me.btn_CropAccept.Name = "btn_CropAccept"
        Me.btn_CropAccept.Size = New System.Drawing.Size(95, 29)
        Me.btn_CropAccept.TabIndex = 1
        Me.btn_CropAccept.Text = "Accept"
        Me.btn_CropAccept.UseVisualStyleBackColor = true
        '
        'btn_CropCancel
        '
        Me.btn_CropCancel.Location = New System.Drawing.Point(28, 22)
        Me.btn_CropCancel.Name = "btn_CropCancel"
        Me.btn_CropCancel.Size = New System.Drawing.Size(95, 29)
        Me.btn_CropCancel.TabIndex = 2
        Me.btn_CropCancel.Text = "Cancel"
        Me.btn_CropCancel.UseVisualStyleBackColor = true
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Poster for : "
        '
        'tb_cropmovtitle
        '
        Me.tb_cropmovtitle.BackColor = System.Drawing.SystemColors.Menu
        Me.tb_cropmovtitle.Location = New System.Drawing.Point(69, 6)
        Me.tb_cropmovtitle.Name = "tb_cropmovtitle"
        Me.tb_cropmovtitle.Size = New System.Drawing.Size(259, 20)
        Me.tb_cropmovtitle.TabIndex = 4
        '
        'GroupBox1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.GroupBox1, 3)
        Me.GroupBox1.Controls.Add(Me.PicBox)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(60, 56)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GroupBox1.Size = New System.Drawing.Size(377, 510)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = false
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btn_CropCancel)
        Me.Panel1.Controls.Add(Me.btn_CropAccept)
        Me.Panel1.Location = New System.Drawing.Point(86, 585)
        Me.Panel1.MaximumSize = New System.Drawing.Size(320, 72)
        Me.Panel1.MinimumSize = New System.Drawing.Size(320, 72)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(320, 72)
        Me.Panel1.TabIndex = 6
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 6
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 330!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 3, 5)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 516!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(492, 668)
        Me.TableLayoutPanel1.TabIndex = 7
        '
        'Panel2
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Panel2, 2)
        Me.Panel2.Controls.Add(Me.tb_cropmovtitle)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Location = New System.Drawing.Point(60, 11)
        Me.Panel2.MaximumSize = New System.Drawing.Size(340, 32)
        Me.Panel2.MinimumSize = New System.Drawing.Size(340, 32)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(340, 32)
        Me.Panel2.TabIndex = 8
        '
        'frmMovPosterCrop
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(492, 668)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = true
        Me.MaximizeBox = false
        Me.MaximumSize = New System.Drawing.Size(1245, 700)
        Me.MinimizeBox = false
        Me.MinimumSize = New System.Drawing.Size(500, 700)
        Me.Name = "frmMovPosterCrop"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Movie Poster Cropping"
        CType(Me.PicBox,System.ComponentModel.ISupportInitialize).EndInit
        Me.GroupBox1.ResumeLayout(false)
        Me.Panel1.ResumeLayout(false)
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.Panel2.ResumeLayout(false)
        Me.Panel2.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents PicBox As System.Windows.Forms.PictureBox
    Friend WithEvents btn_CropAccept As System.Windows.Forms.Button
    Friend WithEvents btn_CropCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tb_cropmovtitle As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel1 As Panel
End Class
