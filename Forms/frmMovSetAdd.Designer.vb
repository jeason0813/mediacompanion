<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovSetAdd
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMovSetAdd))
        Me.tlpMovSetAdd = New System.Windows.Forms.TableLayoutPanel()
        Me.tbMovSetAdd = New System.Windows.Forms.TextBox()
        Me.lblMovSetAdd = New System.Windows.Forms.Label()
        Me.btnMovSetAdd = New System.Windows.Forms.Button()
        Me.tlpMovSetAdd.SuspendLayout
        Me.SuspendLayout
        '
        'tlpMovSetAdd
        '
        Me.tlpMovSetAdd.ColumnCount = 3
        Me.tlpMovSetAdd.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33!))
        Me.tlpMovSetAdd.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34!))
        Me.tlpMovSetAdd.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33!))
        Me.tlpMovSetAdd.Controls.Add(Me.tbMovSetAdd, 0, 1)
        Me.tlpMovSetAdd.Controls.Add(Me.lblMovSetAdd, 0, 0)
        Me.tlpMovSetAdd.Controls.Add(Me.btnMovSetAdd, 1, 2)
        Me.tlpMovSetAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMovSetAdd.Location = New System.Drawing.Point(0, 0)
        Me.tlpMovSetAdd.Name = "tlpMovSetAdd"
        Me.tlpMovSetAdd.RowCount = 3
        Me.tlpMovSetAdd.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
        Me.tlpMovSetAdd.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tlpMovSetAdd.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30!))
        Me.tlpMovSetAdd.Size = New System.Drawing.Size(384, 81)
        Me.tlpMovSetAdd.TabIndex = 1
        '
        'tbMovSetAdd
        '
        Me.tlpMovSetAdd.SetColumnSpan(Me.tbMovSetAdd, 3)
        Me.tbMovSetAdd.Location = New System.Drawing.Point(3, 23)
        Me.tbMovSetAdd.Name = "tbMovSetAdd"
        Me.tbMovSetAdd.Size = New System.Drawing.Size(378, 20)
        Me.tbMovSetAdd.TabIndex = 0
        '
        'lblMovSetAdd
        '
        Me.lblMovSetAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left),System.Windows.Forms.AnchorStyles)
        Me.lblMovSetAdd.AutoSize = true
        Me.tlpMovSetAdd.SetColumnSpan(Me.lblMovSetAdd, 3)
        Me.lblMovSetAdd.Location = New System.Drawing.Point(3, 7)
        Me.lblMovSetAdd.Name = "lblMovSetAdd"
        Me.lblMovSetAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMovSetAdd.Size = New System.Drawing.Size(152, 13)
        Me.lblMovSetAdd.TabIndex = 1
        Me.lblMovSetAdd.Text = "Enter Name of new Movie Set:"
        '
        'btnMovSetAdd
        '
        Me.btnMovSetAdd.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnMovSetAdd.Location = New System.Drawing.Point(153, 54)
        Me.btnMovSetAdd.Name = "btnMovSetAdd"
        Me.btnMovSetAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnMovSetAdd.TabIndex = 2
        Me.btnMovSetAdd.Text = "Add"
        Me.btnMovSetAdd.UseMnemonic = false
        Me.btnMovSetAdd.UseVisualStyleBackColor = true
        '
        'frmMovSetAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 81)
        Me.Controls.Add(Me.tlpMovSetAdd)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.KeyPreview = true
        Me.MaximizeBox = false
        Me.MaximumSize = New System.Drawing.Size(394, 115)
        Me.MinimizeBox = false
        Me.MinimumSize = New System.Drawing.Size(394, 115)
        Me.Name = "frmMovSetAdd"
        Me.ShowIcon = false
        Me.ShowInTaskbar = false
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Movie Set"
        Me.tlpMovSetAdd.ResumeLayout(false)
        Me.tlpMovSetAdd.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents tlpMovSetAdd As TableLayoutPanel
    Friend WithEvents tbMovSetAdd As TextBox
    Friend WithEvents lblMovSetAdd As Label
    Friend WithEvents btnMovSetAdd As Button
End Class
