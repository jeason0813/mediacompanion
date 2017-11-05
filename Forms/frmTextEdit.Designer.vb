<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTextEdit
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
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tbText = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout
        '
        'BtnClose
        '
        Me.BtnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnClose.Location = New System.Drawing.Point(167, 472)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(90, 23)
        Me.BtnClose.TabIndex = 0
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = true
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(50, 472)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 23)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save && Close"
        Me.btnSave.UseVisualStyleBackColor = true
        '
        'tbText
        '
        Me.tbText.AcceptsReturn = true
        Me.tbText.Location = New System.Drawing.Point(25, 25)
        Me.tbText.Multiline = true
        Me.tbText.Name = "tbText"
        Me.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbText.Size = New System.Drawing.Size(260, 435)
        Me.tbText.TabIndex = 2
        Me.tbText.WordWrap = false
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(263, 15)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Enter each custom Genre on a new line."
        '
        'frmTextEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnClose
        Me.ClientSize = New System.Drawing.Size(308, 507)
        Me.ControlBox = false
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.tbText)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.BtnClose)
        Me.MaximumSize = New System.Drawing.Size(316, 534)
        Me.MinimumSize = New System.Drawing.Size(316, 534)
        Me.Name = "frmTextEdit"
        Me.Text = "Edit/Add Custom Genre's"
        Me.TopMost = true
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents BtnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents tbText As TextBox
    Friend WithEvents Label1 As Label
End Class
