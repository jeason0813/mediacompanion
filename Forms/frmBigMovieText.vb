Public Class frmBigMovieText

    Public Overloads Function ShowDialog(
                                        Title    As String,   Director As String,   Votes    As String,   Rating   As String,   Runtime  As String,   
                                        Genre    As String,   Stars    As String,   Cert     As String,   Plot     As String
                                        ) As Windows.Forms.DialogResult
        
        With Me
            Text            = ""
            lblTitle  .Text = Title
            tbGenre   .Text = Genre
            tbDetails .Text = Rating + " (" + Votes + " votes)"
            tbDuration.Text = Runtime
            tbCert    .Text = Cert
            tbDetails2.Text = Stars 
            tbDetails3.Text = Director 
            tbPlot    .Text = Plot

            tbPlot.SelectionStart  = 0
            tbPlot.SelectionLength = 0

            ActiveControl = lblMovieDetails
        End With

        Return MyBase.ShowDialog()
    End Function


    Private Sub Panel1_DoubleClick( sender As System.Object,  e As System.EventArgs) Handles  tbPlot.DoubleClick , tbCloseMsg.DoubleClick , tbGenre.DoubleClick, TextBox6.DoubleClick, TextBox5.DoubleClick, TextBox4.DoubleClick, TextBox3.DoubleClick, TextBox2.DoubleClick, TextBox1.DoubleClick, tbDuration.DoubleClick, tbDetails3.DoubleClick, tbDetails2.DoubleClick, tbDetails.DoubleClick, tbCert.DoubleClick, Panel1.DoubleClick, lblTitle.DoubleClick, lblPlot.DoubleClick, lblMovieDetails.DoubleClick 

        Me.Dispose()    
                                                                                              
    End Sub                                                                                                           

    Private Sub frmBigMovieText_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown 

        If               e.KeyCode = Keys.Escape Then Me.Close() 
    End Sub

End Class
