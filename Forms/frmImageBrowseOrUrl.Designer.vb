<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImageBrowseOrUrl
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
        Me.btn_Browse = New System.Windows.Forms.Button()
        Me.btn_SetThumb = New System.Windows.Forms.Button()
        Me.btn_Cancel = New System.Windows.Forms.Button()
        Me.tb_PathorUrl = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_paste = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btn_Browse
        '
        Me.btn_Browse.Location = New System.Drawing.Point(411, 13)
        Me.btn_Browse.Name = "btn_Browse"
        Me.btn_Browse.Size = New System.Drawing.Size(75, 23)
        Me.btn_Browse.TabIndex = 0
        Me.btn_Browse.Text = "Browse"
        Me.btn_Browse.UseVisualStyleBackColor = True
        '
        'btn_SetThumb
        '
        Me.btn_SetThumb.Location = New System.Drawing.Point(492, 13)
        Me.btn_SetThumb.Name = "btn_SetThumb"
        Me.btn_SetThumb.Size = New System.Drawing.Size(75, 23)
        Me.btn_SetThumb.TabIndex = 1
        Me.btn_SetThumb.Text = "Set Thumb"
        Me.btn_SetThumb.UseVisualStyleBackColor = True
        '
        'btn_Cancel
        '
        Me.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btn_Cancel.Location = New System.Drawing.Point(492, 43)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.btn_Cancel.TabIndex = 2
        Me.btn_Cancel.Text = "Cancel"
        Me.btn_Cancel.UseVisualStyleBackColor = True
        '
        'tb_PathorUrl
        '
        Me.tb_PathorUrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tb_PathorUrl.Location = New System.Drawing.Point(12, 45)
        Me.tb_PathorUrl.Name = "tb_PathorUrl"
        Me.tb_PathorUrl.Size = New System.Drawing.Size(474, 21)
        Me.tb_PathorUrl.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(230, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Enter URL or Browse PC For Thumbnail"
        '
        'btn_paste
        '
        Me.btn_paste.Location = New System.Drawing.Point(330, 13)
        Me.btn_paste.Name = "btn_paste"
        Me.btn_paste.Size = New System.Drawing.Size(75, 23)
        Me.btn_paste.TabIndex = 5
        Me.btn_paste.Text = "Paste Path"
        Me.btn_paste.UseVisualStyleBackColor = True
        '
        'frmImageBrowseOrUrl
        '
        Me.AcceptButton = Me.btn_SetThumb
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.CancelButton = Me.btn_Cancel
        Me.ClientSize = New System.Drawing.Size(594, 81)
        Me.ControlBox = False
        Me.Controls.Add(Me.btn_paste)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tb_PathorUrl)
        Me.Controls.Add(Me.btn_Cancel)
        Me.Controls.Add(Me.btn_SetThumb)
        Me.Controls.Add(Me.btn_Browse)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmImageBrowseOrUrl"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Browse for Image or Enter URL"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_Browse As System.Windows.Forms.Button
    Friend WithEvents btn_SetThumb As System.Windows.Forms.Button
    Friend WithEvents btn_Cancel As System.Windows.Forms.Button
    Friend WithEvents tb_PathorUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btn_paste As System.Windows.Forms.Button
End Class
