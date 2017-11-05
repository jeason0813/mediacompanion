<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmXBMC_Progress
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmXBMC_Progress))
        Me.panelXBMC = New System.Windows.Forms.Panel()
        Me.lblWarningCount = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnPurgeQ = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.lblErrorCount = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblQueueCount = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.panelXBMC.SuspendLayout
        Me.SuspendLayout
        '
        'panelXBMC
        '
        Me.panelXBMC.BackColor = System.Drawing.Color.DimGray
        Me.panelXBMC.Controls.Add(Me.lblWarningCount)
        Me.panelXBMC.Controls.Add(Me.Label2)
        Me.panelXBMC.Controls.Add(Me.btnPurgeQ)
        Me.panelXBMC.Controls.Add(Me.LinkLabel1)
        Me.panelXBMC.Controls.Add(Me.lblErrorCount)
        Me.panelXBMC.Controls.Add(Me.Label3)
        Me.panelXBMC.Controls.Add(Me.lblQueueCount)
        Me.panelXBMC.Controls.Add(Me.Label1)
        Me.panelXBMC.Controls.Add(Me.ProgressBar1)
        Me.panelXBMC.Controls.Add(Me.lblProgress)
        Me.panelXBMC.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelXBMC.Location = New System.Drawing.Point(0, 0)
        Me.panelXBMC.Name = "panelXBMC"
        Me.panelXBMC.Size = New System.Drawing.Size(362, 76)
        Me.panelXBMC.TabIndex = 1
        '
        'lblWarningCount
        '
        Me.lblWarningCount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblWarningCount.AutoEllipsis = true
        Me.lblWarningCount.AutoSize = true
        Me.lblWarningCount.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblWarningCount.ForeColor = System.Drawing.Color.White
        Me.lblWarningCount.Location = New System.Drawing.Point(78, 52)
        Me.lblWarningCount.Name = "lblWarningCount"
        Me.lblWarningCount.Size = New System.Drawing.Size(35, 15)
        Me.lblWarningCount.TabIndex = 9
        Me.lblWarningCount.Text = "0000"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoEllipsis = true
        Me.Label2.AutoSize = true
        Me.Label2.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(7, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 15)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Warnings:"
        '
        'btnPurgeQ
        '
        Me.btnPurgeQ.AutoSize = true
        Me.btnPurgeQ.Location = New System.Drawing.Point(292, 43)
        Me.btnPurgeQ.Name = "btnPurgeQ"
        Me.btnPurgeQ.Size = New System.Drawing.Size(62, 23)
        Me.btnPurgeQ.TabIndex = 7
        Me.btnPurgeQ.Text = "Purge Q"
        Me.btnPurgeQ.UseVisualStyleBackColor = true
        Me.btnPurgeQ.Visible = false
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = true
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DeepSkyBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(121, 53)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(52, 13)
        Me.LinkLabel1.TabIndex = 6
        Me.LinkLabel1.TabStop = true
        Me.LinkLabel1.Text = "View logs"
        '
        'lblErrorCount
        '
        Me.lblErrorCount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblErrorCount.AutoEllipsis = true
        Me.lblErrorCount.AutoSize = true
        Me.lblErrorCount.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblErrorCount.ForeColor = System.Drawing.Color.White
        Me.lblErrorCount.Location = New System.Drawing.Point(78, 37)
        Me.lblErrorCount.Name = "lblErrorCount"
        Me.lblErrorCount.Size = New System.Drawing.Size(35, 15)
        Me.lblErrorCount.TabIndex = 5
        Me.lblErrorCount.Text = "0000"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoEllipsis = true
        Me.Label3.AutoSize = true
        Me.Label3.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(7, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 15)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Errors:"
        '
        'lblQueueCount
        '
        Me.lblQueueCount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblQueueCount.AutoEllipsis = true
        Me.lblQueueCount.AutoSize = true
        Me.lblQueueCount.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblQueueCount.ForeColor = System.Drawing.Color.White
        Me.lblQueueCount.Location = New System.Drawing.Point(78, 22)
        Me.lblQueueCount.Name = "lblQueueCount"
        Me.lblQueueCount.Size = New System.Drawing.Size(35, 15)
        Me.lblQueueCount.TabIndex = 3
        Me.lblQueueCount.Text = "0000"
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoEllipsis = true
        Me.Label1.AutoSize = true
        Me.Label1.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(7, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Jobs:"
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(124, 24)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(230, 13)
        Me.ProgressBar1.TabIndex = 1
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lblProgress.AutoEllipsis = true
        Me.lblProgress.Font = New System.Drawing.Font("MS Reference Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblProgress.ForeColor = System.Drawing.Color.White
        Me.lblProgress.Location = New System.Drawing.Point(7, 4)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(348, 19)
        Me.lblProgress.TabIndex = 0
        Me.lblProgress.Text = "Some Progress Some Progress Some Progress "
        '
        'frmXBMC_Progress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(362, 76)
        Me.Controls.Add(Me.panelXBMC)
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.MaximizeBox = false
        Me.Name = "frmXBMC_Progress"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XBMC  Link"
        Me.TopMost = true
        Me.panelXBMC.ResumeLayout(false)
        Me.panelXBMC.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents panelXBMC As System.Windows.Forms.Panel
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents lblQueueCount As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblErrorCount As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnPurgeQ As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblWarningCount As System.Windows.Forms.Label
End Class
