<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTestForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTestForm))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDone = New System.Windows.Forms.Button()
    '    Me.TmdbSetManager1 = New Media_Companion.TmdbSetManager()
        Me.Panel1.SuspendLayout
        Me.SuspendLayout
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LemonChiffon
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnDone)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 301)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(645, 34)
        Me.Panel1.TabIndex = 8
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnCancel.BackColor = System.Drawing.Color.Gainsboro
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnCancel.Location = New System.Drawing.Point(524, 6)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(49, 23)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = false
        '
        'btnDone
        '
        Me.btnDone.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnDone.BackColor = System.Drawing.Color.Gainsboro
        Me.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnDone.Location = New System.Drawing.Point(588, 6)
        Me.btnDone.Margin = New System.Windows.Forms.Padding(0)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(49, 23)
        Me.btnDone.TabIndex = 7
        Me.btnDone.Text = "Done"
        Me.btnDone.UseVisualStyleBackColor = false
        '
        'TmdbSetManager1
        '
        'Me.TmdbSetManager1.Dock = System.Windows.Forms.DockStyle.Fill
        'Me.TmdbSetManager1.Location = New System.Drawing.Point(0, 0)
        'Me.TmdbSetManager1.MoviesLst = Nothing
        'Me.TmdbSetManager1.Name = "TmdbSetManager1"
        'Me.TmdbSetManager1.Size = New System.Drawing.Size(645, 335)
        'Me.TmdbSetManager1.TabIndex = 0
        '
        'frmTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(645, 335)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TmdbSetManager1)
        Me.Name = "frmTestForm"
        Me.Text = "frmTestForm"
        Me.Panel1.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub

'	Friend WithEvents TmdbSetManager1 As TmdbSetManager
 Friend WithEvents Panel1 As System.Windows.Forms.Panel
 Friend WithEvents btnCancel As System.Windows.Forms.Button
 Friend WithEvents btnDone As System.Windows.Forms.Button
End Class
