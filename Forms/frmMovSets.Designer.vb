<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovSets
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvmovset = New System.Windows.Forms.DataGridView()
        Me.lbl_CollectionTitle = New System.Windows.Forms.Label()
        Me.tmdbid = New System.Windows.Forms.DataGridViewImageColumn()
        Me.movsettitle = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvmovset,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'dgvmovset
        '
        Me.dgvmovset.AllowUserToAddRows = false
        Me.dgvmovset.AllowUserToDeleteRows = false
        Me.dgvmovset.AllowUserToResizeColumns = false
        Me.dgvmovset.AllowUserToResizeRows = false
        Me.dgvmovset.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvmovset.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvmovset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvmovset.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.tmdbid, Me.movsettitle})
        Me.dgvmovset.Location = New System.Drawing.Point(12, 47)
        Me.dgvmovset.MaximumSize = New System.Drawing.Size(455, 232)
        Me.dgvmovset.MinimumSize = New System.Drawing.Size(455, 232)
        Me.dgvmovset.MultiSelect = false
        Me.dgvmovset.Name = "dgvmovset"
        Me.dgvmovset.ReadOnly = true
        Me.dgvmovset.RowHeadersVisible = false
        Me.dgvmovset.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.dgvmovset.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvmovset.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.dgvmovset.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvmovset.ShowCellToolTips = false
        Me.dgvmovset.ShowEditingIcon = false
        Me.dgvmovset.Size = New System.Drawing.Size(455, 232)
        Me.dgvmovset.TabIndex = 14
        '
        'lbl_CollectionTitle
        '
        Me.lbl_CollectionTitle.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.lbl_CollectionTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 14!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lbl_CollectionTitle.Location = New System.Drawing.Point(35, 9)
        Me.lbl_CollectionTitle.Name = "lbl_CollectionTitle"
        Me.lbl_CollectionTitle.Size = New System.Drawing.Size(412, 24)
        Me.lbl_CollectionTitle.TabIndex = 15
        '
        'tmdbid
        '
        Me.tmdbid.HeaderText = "Got it?"
        Me.tmdbid.MinimumWidth = 60
        Me.tmdbid.Name = "tmdbid"
        Me.tmdbid.ReadOnly = true
        Me.tmdbid.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.tmdbid.Width = 60
        '
        'movsettitle
        '
        Me.movsettitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.movsettitle.HeaderText = "Movie Title"
        Me.movsettitle.Name = "movsettitle"
        Me.movsettitle.ReadOnly = true
        Me.movsettitle.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        '
        'frmMovSets
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Desktop
        Me.ClientSize = New System.Drawing.Size(490, 290)
        Me.Controls.Add(Me.lbl_CollectionTitle)
        Me.Controls.Add(Me.dgvmovset)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.KeyPreview = true
        Me.MaximizeBox = false
        Me.MaximumSize = New System.Drawing.Size(603, 324)
        Me.MinimizeBox = false
        Me.MinimumSize = New System.Drawing.Size(500, 324)
        Me.Name = "frmMovSets"
        Me.ShowIcon = false
        Me.Text = "Displaying Movies In Current Collection."
        CType(Me.dgvmovset,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents dgvmovset As DataGridView
    Friend WithEvents lbl_CollectionTitle As Label
    Friend WithEvents tmdbid As DataGridViewImageColumn
    Friend WithEvents movsettitle As DataGridViewTextBoxColumn
End Class
