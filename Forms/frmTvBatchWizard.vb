Public Class tv_batch_wizard

    Private Sub btnTvBatchStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTvBatchStart.Click
        
        Form1.tvBatchList.activate = True

        'Series options
        If cbshYear.Checked OrElse cbshRating.Checked OrElse cbshPlot.Checked OrElse cbshRuntime.Checked OrElse cbshMpaa.Checked OrElse cbshGenre.Checked OrElse cbshStudio.Checked OrElse cbshStatus.Checked Then
            Form1.tvBatchList.doShows = True
            Form1.tvBatchList.doShowBody = True
        End If
        If cbshActor.Checked Then
            Form1.tvBatchList.doShows = True
            Form1.tvBatchList.doShowActors = True
        End If
        If cbshPosters.Checked OrElse cbshFanart.Checked OrElse cbshXtraFanart.Checked OrElse cbshFanartTv.Checked OrElse cbshSeason.Checked Then
            Form1.tvBatchList.doShows = True
            Form1.tvBatchList.doShowArt = True
        End If

        'Episode options.
        If cbepPlot.Checked OrElse cbepAired.Checked OrElse cbepRating.Checked OrElse cbepDirector.Checked OrElse cbepCredits.Checked OrElse cbepIMDBId.Checked OrElse cbepTitle.Checked Then
            Form1.tvBatchList.doEpisodes = True
            Form1.tvBatchList.doEpisodeBody = True
        End If
        If cbepActor.Checked Then
            Form1.tvBatchList.doEpisodes = True
            Form1.tvBatchList.doEpisodeActors = True
        End If
        If cbepRuntime.Checked OrElse cbepStreamDetails.Checked Then
            Form1.tvBatchList.doEpisodes = True
            Form1.tvBatchList.doEpisodeMediaTags = True
        End If
        If cbepdlThumbnail.Checked OrElse cbepCreateScreenshot.Checked Then
            Form1.tvBatchList.doEpisodes = True
            Form1.tvBatchList.doEpisodeArt = True
        End If

        Me.Close()
    End Sub

    Private Sub btn_TvBatchCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TvBatchCancel.Click
        Me.Close()
    End Sub

    
    'tv settings
    Private Sub cbshYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshYear.CheckedChanged
        Form1.tvBatchList.shYear = cbshYear.Checked 
    End Sub     'year
    Private Sub cbshRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshRating.CheckedChanged
        Form1.tvBatchList.shRating = cbshRating.Checked 
    End Sub     'rating
    Private Sub cbshRuntime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshRuntime.CheckedChanged
        Form1.tvBatchList.shRuntime = cbshRuntime.Checked 
    End Sub     'runtime
    Private Sub cbshMpaa_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshMpaa.CheckedChanged
        Form1.tvBatchList.shMpaa = cbshMpaa.Checked 
    End Sub     'mpaa
    Private Sub cbshStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshStudio.CheckedChanged
        Form1.tvBatchList.shStudio = cbshStudio.Checked 
    End Sub     'studio
    Private Sub cbshActor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshActor.CheckedChanged
        Form1.tvBatchList.shActor = cbshActor.Checked 
    End Sub     'actors
    Private Sub cbshStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshStatus.CheckedChanged
        Form1.tvBatchList.shStatus = cbshStatus.Checked
    End Sub     'Status
    Private Sub cbshGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshGenre.CheckedChanged
        Form1.tvBatchList.shGenre = cbshGenre.checked
    End Sub     'genre
    Private Sub cbshPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshPlot.CheckedChanged
        Form1.tvBatchList.shPlot = cbshPlot.Checked 
    End Sub     'plot

    'Tv Show Art
    Private Sub cbshPosters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshPosters.CheckedChanged
        Dim IsChecked As Boolean = cbshPosters.checked
        Form1.tvBatchList.shPosters = IsChecked
        If Not IsChecked Then cbshBannerMain.Checked = CheckState.Unchecked
        cbshBannerMain.Enabled = IsChecked
    End Sub     'missing posters
    Private Sub cbshSeason_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshSeason.CheckedChanged
        Form1.tvBatchList.shSeason = cbshSeason.Checked 
    End Sub     'missing season art
    Private Sub cbshBannerMain_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshBannerMain.CheckedChanged
        Form1.tvBatchList.shBannerMain = cbshBannerMain.Checked
    End Sub     'If season banner is not available, save series banner as season banner.
    Private Sub cbshFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbshFanart.CheckedChanged
        Form1.tvBatchList.shFanart = cbshFanart.Checked 
    End Sub     'missing fanart
    Private Sub cbshXtraFanart_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbshXtraFanart.CheckedChanged
        Form1.tvBatchList.shXtraFanart = cbshXtraFanart.Checked 
    End Sub     'missing Extra Fanart
    Private Sub cbshFanartTv_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbshFanartTv.CheckedChanged
        Form1.tvBatchList.shFanartTvArt = cbshFanartTv.Checked 
    End Sub     'missing Fanart.Tv Art
    Private Sub cbshDelArtwork_CheckedChanged( sender As Object,  e As EventArgs) Handles cbshDelArtwork.CheckedChanged
        If cbshDelArtwork.Checked = True Then
            cbshPosters.Checked = True
            cbshSeason.Checked = True
            cbshFanart.Checked = True
        End If
        Form1.tvBatchList.shDelArtwork = cbshDelArtwork.Checked
        Label3.Visible = cbshDelArtwork.Checked
    End Sub     'Delete Series Artwork

    'episode settings
    Private Sub cbepPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepPlot.CheckedChanged
        
        Form1.tvBatchList.epPlot = cbepPlot.Checked 
    End Sub             'plot
    Private Sub cbepAired_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepAired.CheckedChanged
    
        Form1.tvBatchList.epAired = cbepAired.Checked
    End Sub           'air date
    Private Sub cbepDirector_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepDirector.CheckedChanged
        
        Form1.tvBatchList.epDirector = cbepDirector.Checked 
    End Sub     'director
    Private Sub cbepCredits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepCredits.CheckedChanged
        
        Form1.tvBatchList.epCredits = cbepCredits.checked
    End Sub       'credits
    Private Sub cbepRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepRating.CheckedChanged
        Form1.tvBatchList.epRating = cbepRating.checked
    End Sub         'rating
    Private Sub cbepActor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepActor.CheckedChanged
        Form1.tvBatchList.epActor = cbepActor.Checked
    End Sub           'actors
    Private Sub cbepIMDBId_CheckedChanged(sender As Object, e As EventArgs) Handles cbepIMDBId.CheckedChanged
        Form1.tvBatchList.epIMDBId = cbepIMDBId.Checked
    End Sub                                   'Force update episode IMDB Id's.
    Private Sub cbepRuntime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepRuntime.CheckedChanged
        Form1.tvBatchList.epRuntime = cbepRuntime.Checked 
    End Sub       'runtime
    Private Sub cbepTitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepTitle.CheckedChanged
        Form1.tvBatchList.epTitle = cbepTitle.Checked 
    End Sub           'Episode Title
    Private Sub cbepStreamDetails_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepStreamDetails.CheckedChanged
        Form1.tvBatchList.epStreamDetails = cbepStreamDetails.Checked 
    End Sub     'media tags
    Private Sub cbepdlThumbnail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepdlThumbnail.CheckedChanged
        Form1.tvBatchList.epdlThumbnail = cbepdlThumbnail.Checked
        'If cbepdlThumbnail.Checked Then
        '    Form1.tvBatchList.epdlThumbnail = True
        '    'cbepCreateScreenshot.Enabled    = True
        'Else
        '    Form1.tvBatchList.epdlThumbnail = False
        '    'cbepCreateScreenshot.Enabled    = False
        '    'cbepCreateScreenshot.CheckState = CheckState.Unchecked
        '    'Form1.tvBatchList.epCreateScreenshot = False
        'End If
    End Sub         'download screenshot
    Private Sub cbepCreateScreenshot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbepCreateScreenshot.CheckedChanged
        Form1.tvBatchList.epCreateScreenshot = cbepCreateScreenshot.Checked 
    End Sub     'create screenshot
    Private Sub cbincludeLocked_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbincludeLocked.CheckedChanged
        Form1.tvBatchList.includeLocked = cbincludeLocked.checked
    End Sub         'Include locked Series
    Private Sub cbRewriteAllNfo_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbRewriteAllNfo.CheckedChanged
        If cbRewriteAllNfo.Checked = True Then
            Dim infotext As String = ""
            If Form1.tvBatchList.includeLocked = True Then
                infotext = "all of your"
            Else
                infotext = "your unlocked only"
            End If
            If MsgBox("You have selected " & infotext & " TVShows nfo's" & vbCrLf & "Do you want to rewite all of these nfo's?" & vbCrLf & "MC will read in the current nfo & write back only the data it uses in order to clean the nfo.", MsgBoxStyle.OkCancel, "Question?") = MsgBoxResult.Ok Then
                Form1.tvBatchList.RewriteAllNFOs = True
                GroupBox1.Enabled = False
                GroupBox2.Enabled = False
                cbShSeriesXML.Checked = CheckState.unchecked
            Else
                cbRewriteAllNfo.Checked = False
            End If
        Else
            Form1.tvBatchList.RewriteAllNFOs = False
            GroupBox1.Enabled = True
            GroupBox2.Enabled = True
        End If
    End Sub             'ReWrite Nfo's
    Private Sub cbShSeriesXML_CheckedChanged(sender As Object, e As EventArgs) Handles cbShSeriesXML.CheckedChanged
        If cbShSeriesXML.Checked = True Then
            GroupBox1.Enabled = False
            GroupBox2.Enabled = False
            Form1.tvBatchList.shSeries = True
            cbRewriteAllNfo.Checked = CheckState.unchecked
        Else
            GroupBox1.Enabled = True
            GroupBox2.Enabled = True
            Form1.tvBatchList.shSeries = False
        End If
    End Sub                               'Update Series XML files

    Private Sub tv_batch_wizard_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        cbepStreamDetails.Enabled = Pref.enabletvhdtags
        Dim TitleText As String = "Tv Batch Wizard - "
        If Form1.singleshow Then
            Dim workingshow As TvShow = Form1.tv_ShowSelectedCurrently(Form1.TvTreeview)
            TitleText += workingshow.Title.Value
        Else
            TitleText += "All Shows"
        End If
        Me.Text = TitleText
    End Sub

    Private Sub tv_batch_wizard_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then btn_TvBatchCancel.PerformClick 
    End Sub
    
End Class