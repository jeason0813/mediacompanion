<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateDateFix
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreateDateFix))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudDateFix = New System.Windows.Forms.NumericUpDown()
        Me.cmbxDateFix = New System.Windows.Forms.ComboBox()
        Me.dateFixDataGridView = New System.Windows.Forms.DataGridView()
        Me.chkbxSelect = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.statusDateFix = New System.Windows.Forms.StatusStrip()
        Me.statuslblDateFix = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnDateFix = New System.Windows.Forms.Button()
        CType(Me.nudDateFix, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dateFixDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.statusDateFix.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(215, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Show all Titles where Create Date differs by:"
        '
        'nudDateFix
        '
        Me.nudDateFix.Location = New System.Drawing.Point(224, 7)
        Me.nudDateFix.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nudDateFix.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDateFix.Name = "nudDateFix"
        Me.nudDateFix.Size = New System.Drawing.Size(38, 20)
        Me.nudDateFix.TabIndex = 1
        Me.nudDateFix.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cmbxDateFix
        '
        Me.cmbxDateFix.FormattingEnabled = True
        Me.cmbxDateFix.Location = New System.Drawing.Point(265, 6)
        Me.cmbxDateFix.Name = "cmbxDateFix"
        Me.cmbxDateFix.Size = New System.Drawing.Size(63, 21)
        Me.cmbxDateFix.TabIndex = 2
        '
        'dateFixDataGridView
        '
        Me.dateFixDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dateFixDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dateFixDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.chkbxSelect})
        Me.dateFixDataGridView.Location = New System.Drawing.Point(15, 33)
        Me.dateFixDataGridView.Name = "dateFixDataGridView"
        Me.dateFixDataGridView.RowHeadersVisible = False
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.dateFixDataGridView.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dateFixDataGridView.Size = New System.Drawing.Size(557, 298)
        Me.dateFixDataGridView.TabIndex = 3
        '
        'chkbxSelect
        '
        Me.chkbxSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.chkbxSelect.HeaderText = " "
        Me.chkbxSelect.Name = "chkbxSelect"
        Me.chkbxSelect.Width = 20
        '
        'statusDateFix
        '
        Me.statusDateFix.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.statuslblDateFix})
        Me.statusDateFix.Location = New System.Drawing.Point(0, 344)
        Me.statusDateFix.Name = "statusDateFix"
        Me.statusDateFix.Size = New System.Drawing.Size(592, 22)
        Me.statusDateFix.TabIndex = 5
        Me.statusDateFix.Text = "StatusStrip1"
        '
        'statuslblDateFix
        '
        Me.statuslblDateFix.Name = "statuslblDateFix"
        Me.statuslblDateFix.Size = New System.Drawing.Size(10, 17)
        Me.statuslblDateFix.Text = " "
        '
        'btnDateFix
        '
        Me.btnDateFix.Location = New System.Drawing.Point(497, 4)
        Me.btnDateFix.Name = "btnDateFix"
        Me.btnDateFix.Size = New System.Drawing.Size(75, 23)
        Me.btnDateFix.TabIndex = 6
        Me.btnDateFix.Text = "Update"
        Me.btnDateFix.UseVisualStyleBackColor = True
        '
        'frmCreateDateFix
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 366)
        Me.Controls.Add(Me.btnDateFix)
        Me.Controls.Add(Me.statusDateFix)
        Me.Controls.Add(Me.dateFixDataGridView)
        Me.Controls.Add(Me.cmbxDateFix)
        Me.Controls.Add(Me.nudDateFix)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "frmCreateDateFix"
        Me.Text = "Create Date Fix"
        CType(Me.nudDateFix, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dateFixDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.statusDateFix.ResumeLayout(False)
        Me.statusDateFix.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudDateFix As System.Windows.Forms.NumericUpDown
    Friend WithEvents cmbxDateFix As System.Windows.Forms.ComboBox
    Friend WithEvents dateFixDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents statusDateFix As System.Windows.Forms.StatusStrip
    Friend WithEvents chkbxSelect As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents statuslblDateFix As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnDateFix As System.Windows.Forms.Button
End Class
