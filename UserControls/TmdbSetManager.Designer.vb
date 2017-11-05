<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TmdbSetManager
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.tlpTmdbSetManager = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgvCustomSetNames = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblCustomSetNames = New System.Windows.Forms.Label()
        Me.TmdbSetId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MovieSetName = New System.Windows.Forms.DataGridViewTextBoxColumn()
   '     Me.UserMovieSetName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tlpTmdbSetManager.SuspendLayout
        CType(Me.dgvCustomSetNames,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'tlpTmdbSetManager
        '
        Me.tlpTmdbSetManager.BackColor = System.Drawing.Color.Khaki
        Me.tlpTmdbSetManager.ColumnCount = 4
        Me.tlpTmdbSetManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTmdbSetManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTmdbSetManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTmdbSetManager.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTmdbSetManager.Controls.Add(Me.Label1, 0, 2)
        Me.tlpTmdbSetManager.Controls.Add(Me.dgvCustomSetNames, 0, 3)
        Me.tlpTmdbSetManager.Controls.Add(Me.lblTitle, 0, 0)
        Me.tlpTmdbSetManager.Controls.Add(Me.lblCustomSetNames, 0, 1)
        Me.tlpTmdbSetManager.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpTmdbSetManager.Location = New System.Drawing.Point(0, 0)
        Me.tlpTmdbSetManager.Name = "tlpTmdbSetManager"
        Me.tlpTmdbSetManager.RowCount = 4
        Me.tlpTmdbSetManager.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpTmdbSetManager.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpTmdbSetManager.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpTmdbSetManager.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpTmdbSetManager.Size = New System.Drawing.Size(671, 383)
        Me.tlpTmdbSetManager.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = true
        Me.Label1.BackColor = System.Drawing.Color.LemonChiffon
        Me.tlpTmdbSetManager.SetColumnSpan(Me.Label1, 4)
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 69)
        Me.Label1.Name = "Label1"
        Me.Label1.Padding = New System.Windows.Forms.Padding(4)
        Me.Label1.Size = New System.Drawing.Size(665, 28)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Use this section to override Tmdb set names with your own."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dgvCustomSetNames
        '
        Me.dgvCustomSetNames.AllowUserToAddRows = false
        Me.dgvCustomSetNames.AllowUserToDeleteRows = false
        Me.dgvCustomSetNames.BackgroundColor = System.Drawing.Color.Khaki
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvCustomSetNames.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvCustomSetNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCustomSetNames.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TmdbSetId, Me.MovieSetName}) ', Me.UserMovieSetName
        Me.tlpTmdbSetManager.SetColumnSpan(Me.dgvCustomSetNames, 4)
        Me.dgvCustomSetNames.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCustomSetNames.GridColor = System.Drawing.Color.FromArgb(CType(CType(255,Byte),Integer), CType(CType(192,Byte),Integer), CType(CType(192,Byte),Integer))
        Me.dgvCustomSetNames.Location = New System.Drawing.Point(3, 100)
        Me.dgvCustomSetNames.Name = "dgvCustomSetNames"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.GreenYellow
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvCustomSetNames.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvCustomSetNames.Size = New System.Drawing.Size(665, 280)
        Me.dgvCustomSetNames.TabIndex = 5
        '
        'lblTitle
        '
        Me.lblTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblTitle.AutoSize = true
        Me.lblTitle.BackColor = System.Drawing.Color.Black
        Me.tlpTmdbSetManager.SetColumnSpan(Me.lblTitle, 4)
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 18!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(3, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(4)
        Me.lblTitle.Size = New System.Drawing.Size(665, 37)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Tmdb Set Manager"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCustomSetNames
        '
        Me.lblCustomSetNames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblCustomSetNames.AutoSize = true
        Me.lblCustomSetNames.BackColor = System.Drawing.Color.LemonChiffon
        Me.tlpTmdbSetManager.SetColumnSpan(Me.lblCustomSetNames, 4)
        Me.lblCustomSetNames.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCustomSetNames.ForeColor = System.Drawing.Color.Blue
        Me.lblCustomSetNames.Location = New System.Drawing.Point(3, 37)
        Me.lblCustomSetNames.Name = "lblCustomSetNames"
        Me.lblCustomSetNames.Padding = New System.Windows.Forms.Padding(4)
        Me.lblCustomSetNames.Size = New System.Drawing.Size(665, 32)
        Me.lblCustomSetNames.TabIndex = 1
        Me.lblCustomSetNames.Text = "Custom Set names"
        Me.lblCustomSetNames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TmdbSetId
        '
        Me.TmdbSetId.DataPropertyName = "TmdbSetId"
        Me.TmdbSetId.HeaderText = "Set Id"
        Me.TmdbSetId.Name = "TmdbSetId"
        Me.TmdbSetId.ReadOnly = true
        Me.TmdbSetId.ToolTipText = "Tmdb movie collection id"
        Me.TmdbSetId.Width = 70
        '
        'MovieSetName
        '
        Me.MovieSetName.DataPropertyName = "MovieSetName"
        Me.MovieSetName.HeaderText = "Tmdb Set Name"
        Me.MovieSetName.MinimumWidth = 150
        Me.MovieSetName.Name = "MovieSetName"
        Me.MovieSetName.ReadOnly = true
        Me.MovieSetName.Width = 200
        '
        'UserMovieSetName
        '
        'Me.UserMovieSetName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        'Me.UserMovieSetName.DataPropertyName = "UserMovieSetName"
        'Me.UserMovieSetName.HeaderText = "User Preferred Name"
        'Me.UserMovieSetName.MinimumWidth = 150
        'Me.UserMovieSetName.Name = "UserMovieSetName"
        '
        'TmdbSetManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tlpTmdbSetManager)
        Me.Name = "TmdbSetManager"
        Me.Size = New System.Drawing.Size(671, 383)
        Me.tlpTmdbSetManager.ResumeLayout(false)
        Me.tlpTmdbSetManager.PerformLayout
        CType(Me.dgvCustomSetNames,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents tlpTmdbSetManager As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblCustomSetNames As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgvCustomSetNames As System.Windows.Forms.DataGridView
    Friend WithEvents TmdbSetId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MovieSetName As System.Windows.Forms.DataGridViewTextBoxColumn
'    Friend WithEvents UserMovieSetName As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
