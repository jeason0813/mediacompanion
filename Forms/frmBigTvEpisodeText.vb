Public Class frmBigTvEpisodeText

    Public Overloads Function ShowDialog(
                                        Title    As String,   Director As String,   Aired As String,   Rating As String, Votes As String,  
                                        Duration As String,   Genre    As String,   Cert  As String,   Plot   As String
                                        ) As Windows.Forms.DialogResult
        
        With Me
            Text            = ""
            lblTitle    .Text = Title
            tbDirector  .Text = Director 
            tbAired     .Text = Aired
            tbRating    .Text = Rating
            tbVotes     .Text = Votes
            tbDuration  .Text = Duration
            tbGenre     .Text = Genre
            tbCert      .Text = Cert
            tbPlot      .Text = Plot

            tbPlot.SelectionStart  = 0
            tbPlot.SelectionLength = 0

            ActiveControl = lblDetails
        End With

        Return MyBase.ShowDialog()
    End Function


    Private Sub Panel1_DoubleClick( sender As System.Object,  e As System.EventArgs) Handles  tbGenre.DoubleClick, tbDuration.DoubleClick, tbPlot.DoubleClick , tbCloseMsg.DoubleClick , tbGenre.DoubleClick, TextBox6.DoubleClick, TextBox5.DoubleClick, TextBox4.DoubleClick,  TextBox2.DoubleClick, TextBox1.DoubleClick, tbDuration.DoubleClick, tbAired.DoubleClick, tbRating.DoubleClick, tbDetails.DoubleClick, tbCert.DoubleClick, Panel1.DoubleClick, lblTitle.DoubleClick, lblPlot.DoubleClick, lblDetails.DoubleClick, tbAired.DoubleClick, tbDirector.DoubleClick, tbRating.DoubleClick, TextBox3.DoubleClick 

        Me.Dispose()    
                                                                                              
    End Sub                                                                                                           

    Private Sub frmBigTvEpisodeText_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If               e.KeyCode = Keys.Escape Then Me.Close() 
    End Sub

End Class
