<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TooltipGridViewMovies
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
        Me.TextBoxMovie = New System.Windows.Forms.TextBox()
        Me.LabelMovieName = New System.Windows.Forms.Label()
        Me.LabelMovieYear = New System.Windows.Forms.Label()
        Me.LabelRatingRuntime = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Tan
        Me.Panel1.Controls.Add(Me.LabelRatingRuntime)
        Me.Panel1.Controls.Add(Me.LabelMovieYear)
        Me.Panel1.Controls.Add(Me.LabelMovieName)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(248, 51)
        Me.Panel1.TabIndex = 0
        '
        'TextBoxMovie
        '
        Me.TextBoxMovie.BackColor = System.Drawing.Color.BlanchedAlmond
        Me.TextBoxMovie.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxMovie.Location = New System.Drawing.Point(0, 51)
        Me.TextBoxMovie.Multiline = True
        Me.TextBoxMovie.Name = "TextBoxMovie"
        Me.TextBoxMovie.Size = New System.Drawing.Size(248, 247)
        Me.TextBoxMovie.TabIndex = 1
        '
        'LabelMovieName
        '
        Me.LabelMovieName.AutoSize = True
        Me.LabelMovieName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMovieName.Location = New System.Drawing.Point(3, 0)
        Me.LabelMovieName.Name = "LabelMovieName"
        Me.LabelMovieName.Size = New System.Drawing.Size(115, 17)
        Me.LabelMovieName.TabIndex = 0
        Me.LabelMovieName.Text = "LabelMovieName"
        '
        'LabelMovieYear
        '
        Me.LabelMovieYear.AutoSize = True
        Me.LabelMovieYear.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMovieYear.Location = New System.Drawing.Point(3, 17)
        Me.LabelMovieYear.Name = "LabelMovieYear"
        Me.LabelMovieYear.Size = New System.Drawing.Size(86, 13)
        Me.LabelMovieYear.TabIndex = 1
        Me.LabelMovieYear.Text = "LabelMovieYear"
        '
        'LabelRatingRuntime
        '
        Me.LabelRatingRuntime.AutoSize = True
        Me.LabelRatingRuntime.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelRatingRuntime.Location = New System.Drawing.Point(3, 30)
        Me.LabelRatingRuntime.Name = "LabelRatingRuntime"
        Me.LabelRatingRuntime.Size = New System.Drawing.Size(111, 13)
        Me.LabelRatingRuntime.TabIndex = 2
        Me.LabelRatingRuntime.Text = "LabelRatingRuntime"
        '
        'TooltipGridViewMovies
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.TextBoxMovie)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "TooltipGridViewMovies"
        Me.Size = New System.Drawing.Size(248, 298)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextBoxMovie As System.Windows.Forms.TextBox
    Friend WithEvents LabelMovieYear As System.Windows.Forms.Label
    Friend WithEvents LabelMovieName As System.Windows.Forms.Label
    Friend WithEvents LabelRatingRuntime As System.Windows.Forms.Label

End Class
