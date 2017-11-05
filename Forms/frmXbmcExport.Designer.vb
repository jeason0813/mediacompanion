<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmXbmcExport
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btn_FolderBrowse = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblTVCount = New System.Windows.Forms.Label()
        Me.lblMovieCount = New System.Windows.Forms.Label()
        Me.MCExportdgv = New System.Windows.Forms.DataGridView()
        Me.MCType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MCSource = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Kodipath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btn_Start = New System.Windows.Forms.Button()
        Me.btn_Cancel = New System.Windows.Forms.Button()
        Me.btn_Validate = New System.Windows.Forms.Button()
        Me.pbCheck = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout
        CType(Me.MCExportdgv,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.pbCheck,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(313, 48)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Here we can Export Media Companion's Movie and"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"TV Series (including episodes), i"& _ 
    "nto a format that"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"KBMC/Kodi can use to Import directly."
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(356, 28)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(287, 20)
        Me.TextBox1.TabIndex = 1
        '
        'btn_FolderBrowse
        '
        Me.btn_FolderBrowse.Location = New System.Drawing.Point(356, 54)
        Me.btn_FolderBrowse.Name = "btn_FolderBrowse"
        Me.btn_FolderBrowse.Size = New System.Drawing.Size(131, 23)
        Me.btn_FolderBrowse.TabIndex = 2
        Me.btn_FolderBrowse.Text = "Browse for Export Folder"
        Me.btn_FolderBrowse.UseVisualStyleBackColor = true
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(16, 406)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(621, 23)
        Me.ProgressBar1.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(13, 390)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Progress"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.lblTVCount)
        Me.GroupBox1.Controls.Add(Me.lblMovieCount)
        Me.GroupBox1.Location = New System.Drawing.Point(356, 120)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(281, 66)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = false
        Me.GroupBox1.Text = "Media Tally"
        '
        'Label9
        '
        Me.Label9.AutoSize = true
        Me.Label9.Location = New System.Drawing.Point(119, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(71, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Movies found"
        '
        'Label8
        '
        Me.Label8.AutoSize = true
        Me.Label8.Location = New System.Drawing.Point(119, 35)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Tv Series found"
        '
        'lblTVCount
        '
        Me.lblTVCount.AutoSize = true
        Me.lblTVCount.Location = New System.Drawing.Point(24, 35)
        Me.lblTVCount.Name = "lblTVCount"
        Me.lblTVCount.Size = New System.Drawing.Size(27, 13)
        Me.lblTVCount.TabIndex = 1
        Me.lblTVCount.Text = "xxxx"
        '
        'lblMovieCount
        '
        Me.lblMovieCount.AutoSize = true
        Me.lblMovieCount.Location = New System.Drawing.Point(24, 16)
        Me.lblMovieCount.Name = "lblMovieCount"
        Me.lblMovieCount.Size = New System.Drawing.Size(27, 13)
        Me.lblMovieCount.TabIndex = 0
        Me.lblMovieCount.Text = "xxxx"
        '
        'MCExportdgv
        '
        Me.MCExportdgv.AllowUserToAddRows = false
        Me.MCExportdgv.AllowUserToDeleteRows = false
        Me.MCExportdgv.AllowUserToResizeRows = false
        Me.MCExportdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.MCExportdgv.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.MCType, Me.MCSource, Me.Kodipath})
        Me.MCExportdgv.Location = New System.Drawing.Point(12, 221)
        Me.MCExportdgv.MultiSelect = false
        Me.MCExportdgv.Name = "MCExportdgv"
        Me.MCExportdgv.RowHeadersVisible = false
        Me.MCExportdgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.MCExportdgv.ShowCellToolTips = false
        Me.MCExportdgv.Size = New System.Drawing.Size(645, 166)
        Me.MCExportdgv.TabIndex = 6
        '
        'MCType
        '
        Me.MCType.HeaderText = "Type"
        Me.MCType.MaxInputLength = 50
        Me.MCType.MinimumWidth = 45
        Me.MCType.Name = "MCType"
        Me.MCType.ReadOnly = true
        Me.MCType.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.MCType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.MCType.Width = 45
        '
        'MCSource
        '
        Me.MCSource.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.MCSource.DividerWidth = 2
        Me.MCSource.HeaderText = "MC Source"
        Me.MCSource.MaxInputLength = 300
        Me.MCSource.Name = "MCSource"
        Me.MCSource.ReadOnly = true
        Me.MCSource.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.MCSource.ToolTipText = "This is the path as Set in Media Companion"
        Me.MCSource.Width = 290
        '
        'Kodipath
        '
        Me.Kodipath.HeaderText = "XBMC/Kodi Path"
        Me.Kodipath.MaxInputLength = 300
        Me.Kodipath.Name = "Kodipath"
        Me.Kodipath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Kodipath.ToolTipText = "Select the matching path as set in XBMC/Kodi"
        Me.Kodipath.Width = 290
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(19, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 18)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "WARNING !!"
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label4.Location = New System.Drawing.Point(19, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(256, 39)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Before importing into XBMC/Kodi, ensure "&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"you have already created the Source pat"& _ 
    "hs,"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"and set their content in XBMC/Kodi."
        '
        'Label5
        '
        Me.Label5.AutoSize = true
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 194)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(103, 26)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Link your Movie and"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Tv Root paths"
        '
        'Label7
        '
        Me.Label7.AutoSize = true
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label7.Location = New System.Drawing.Point(353, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(248, 16)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Select the folder to output the export files."
        '
        'btn_Start
        '
        Me.btn_Start.Location = New System.Drawing.Point(463, 192)
        Me.btn_Start.Name = "btn_Start"
        Me.btn_Start.Size = New System.Drawing.Size(83, 23)
        Me.btn_Start.TabIndex = 11
        Me.btn_Start.Text = "Start Export"
        Me.btn_Start.UseVisualStyleBackColor = true
        '
        'btn_Cancel
        '
        Me.btn_Cancel.Location = New System.Drawing.Point(554, 192)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.Size = New System.Drawing.Size(83, 23)
        Me.btn_Cancel.TabIndex = 12
        Me.btn_Cancel.Text = "Close"
        Me.btn_Cancel.UseVisualStyleBackColor = true
        '
        'btn_Validate
        '
        Me.btn_Validate.Location = New System.Drawing.Point(514, 54)
        Me.btn_Validate.Name = "btn_Validate"
        Me.btn_Validate.Size = New System.Drawing.Size(100, 23)
        Me.btn_Validate.TabIndex = 13
        Me.btn_Validate.Text = "Validate Path"
        Me.btn_Validate.UseVisualStyleBackColor = true
        '
        'pbCheck
        '
        Me.pbCheck.Image = Global.Media_Companion.My.Resources.Resources.incorrect
        Me.pbCheck.Location = New System.Drawing.Point(620, 54)
        Me.pbCheck.Name = "pbCheck"
        Me.pbCheck.Size = New System.Drawing.Size(23, 23)
        Me.pbCheck.TabIndex = 14
        Me.pbCheck.TabStop = false
        '
        'Label6
        '
        Me.Label6.AutoSize = true
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label6.Location = New System.Drawing.Point(132, 192)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(230, 26)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "NB:  MC is unable to confirm these"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"paths are valid, so ensure your correct!"
        '
        'Label10
        '
        Me.Label10.AutoSize = true
        Me.Label10.Location = New System.Drawing.Point(354, 82)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(289, 26)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Your exported data will be in a sub folder of your"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"selected export folder named:"& _ 
    "  xbmc_videodb_yyyy_MM_dd"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmXbmcExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(681, 431)
        Me.ControlBox = false
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pbCheck)
        Me.Controls.Add(Me.btn_Validate)
        Me.Controls.Add(Me.btn_Cancel)
        Me.Controls.Add(Me.btn_Start)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MCExportdgv)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.btn_FolderBrowse)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.KeyPreview = true
        Me.MinimumSize = New System.Drawing.Size(683, 345)
        Me.Name = "frmXbmcExport"
        Me.Text = "MC Export Library"
        Me.GroupBox1.ResumeLayout(false)
        Me.GroupBox1.PerformLayout
        CType(Me.MCExportdgv,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.pbCheck,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents btn_FolderBrowse As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblTVCount As System.Windows.Forms.Label
    Friend WithEvents lblMovieCount As System.Windows.Forms.Label
    Friend WithEvents MCExportdgv As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btn_Start As System.Windows.Forms.Button
    Friend WithEvents btn_Cancel As System.Windows.Forms.Button
    Friend WithEvents btn_Validate As System.Windows.Forms.Button
    Friend WithEvents pbCheck As System.Windows.Forms.PictureBox
    Friend WithEvents MCType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MCSource As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Kodipath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
