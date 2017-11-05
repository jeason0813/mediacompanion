<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GraphicInfo
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
        Me.picbxGraphicInfo = New System.Windows.Forms.PictureBox()
        CType(Me.picbxGraphicInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picbxGraphicInfo
        '
        Me.picbxGraphicInfo.Image = Global.Media_Companion.My.Resources.Resources.Stars
        Me.picbxGraphicInfo.Location = New System.Drawing.Point(0, 0)
        Me.picbxGraphicInfo.Name = "picbxGraphicInfo"
        Me.picbxGraphicInfo.Size = New System.Drawing.Size(199, 32)
        Me.picbxGraphicInfo.TabIndex = 0
        Me.picbxGraphicInfo.TabStop = False
        '
        'GraphicInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.picbxGraphicInfo)
        Me.Name = "GraphicInfo"
        Me.Size = New System.Drawing.Size(199, 32)
        CType(Me.picbxGraphicInfo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents picbxGraphicInfo As System.Windows.Forms.PictureBox

End Class
