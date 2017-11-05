<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigureMovieFilters
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblConfigureMovieFilters = New System.Windows.Forms.Label()
        Me.tbMovieFilters = New System.Windows.Forms.TextBox()
        Me.lbMovieFilters = New System.Windows.Forms.ListBox()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout
        Me.SuspendLayout
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.lblConfigureMovieFilters)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(248, 28)
        Me.Panel1.TabIndex = 0
        '
        'lblConfigureMovieFilters
        '
        Me.lblConfigureMovieFilters.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblConfigureMovieFilters.AutoSize = true
        Me.lblConfigureMovieFilters.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblConfigureMovieFilters.ForeColor = System.Drawing.Color.SeaGreen
        Me.lblConfigureMovieFilters.Location = New System.Drawing.Point(48, 6)
        Me.lblConfigureMovieFilters.Name = "lblConfigureMovieFilters"
        Me.lblConfigureMovieFilters.Size = New System.Drawing.Size(153, 17)
        Me.lblConfigureMovieFilters.TabIndex = 0
        Me.lblConfigureMovieFilters.Text = "Configure Movie Filters"
        '
        'tbMovieFilters
        '
        Me.tbMovieFilters.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.tbMovieFilters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbMovieFilters.Location = New System.Drawing.Point(0, 28)
        Me.tbMovieFilters.Multiline = true
        Me.tbMovieFilters.Name = "tbMovieFilters"
        Me.tbMovieFilters.Size = New System.Drawing.Size(248, 270)
        Me.tbMovieFilters.TabIndex = 1
        '
        'lbMovieFilters
        '
        Me.lbMovieFilters.FormattingEnabled = true
        Me.lbMovieFilters.Location = New System.Drawing.Point(8, 38)
        Me.lbMovieFilters.Name = "lbMovieFilters"
        Me.lbMovieFilters.Size = New System.Drawing.Size(231, 212)
        Me.lbMovieFilters.TabIndex = 3
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(190, 265)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(49, 23)
        Me.btnApply.TabIndex = 4
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = true
        '
        'ConfigureMovieFilters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.lbMovieFilters)
        Me.Controls.Add(Me.tbMovieFilters)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ConfigureMovieFilters"
        Me.Size = New System.Drawing.Size(248, 298)
        Me.Panel1.ResumeLayout(false)
        Me.Panel1.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents tbMovieFilters As System.Windows.Forms.TextBox
    Friend WithEvents lblConfigureMovieFilters As System.Windows.Forms.Label
    Friend WithEvents lbMovieFilters As System.Windows.Forms.ListBox
    Friend WithEvents btnApply As System.Windows.Forms.Button

End Class
