Public Class frmBatchScraper

    Private Sub cbMainTop250_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainTop250.CheckedChanged
        Form1.rescrapeList.top250 = cbMainTop250.Checked
    End Sub

    Private Sub cbMainRuntime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainRuntime.CheckedChanged
        Form1.rescrapeList.runtime = cbMainRuntime.Checked
    End Sub

    Private Sub CcbMainDirector_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainDirector.CheckedChanged
        Form1.rescrapeList.director = cbMainDirector.Checked
    End Sub

    Private Sub cbMainOutline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainOutline.CheckedChanged
        Form1.rescrapeList.outline = cbMainOutline.Checked
    End Sub

    Private Sub cbMainCert_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainCert.CheckedChanged
        Form1.rescrapeList.mpaa = cbMainCert.Checked
    End Sub

    Private Sub cbMainPremiered_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainPremiered.CheckedChanged
        Form1.rescrapeList.premiered = cbMainPremiered.Checked
    End Sub

    Private Sub cbMainRating_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainRating.CheckedChanged
        Form1.rescrapeList.rating = cbMainRating.Checked
    End Sub

    Private Sub cbMainPlot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainPlot.CheckedChanged
        Form1.rescrapeList.plot = cbMainPlot.Checked
    End Sub

    Private Sub cbMainGenre_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainGenre.CheckedChanged
        Form1.rescrapeList.genre = cbMainGenre.Checked
    End Sub

    Private Sub cbMainStudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainStudio.CheckedChanged
        Form1.rescrapeList.studio = cbMainStudio.Checked
    End Sub

    Private Sub cbMainVotes_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainVotes.CheckedChanged
        Form1.rescrapeList.votes = cbMainVotes.Checked
    End Sub

    Private Sub cbMainTagline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainTagline.CheckedChanged
        Form1.rescrapeList.tagline = cbMainTagline.Checked
    End Sub

    Private Sub cbMainCredits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainCredits.CheckedChanged
        Form1.rescrapeList.credits = cbMainCredits.Checked
    End Sub

    Private Sub cbMainTrailer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMainTrailer.CheckedChanged
        Form1.rescrapeList.trailer = cbMainTrailer.Checked
    End Sub

    Private Sub cbRescrapeActors_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRescrapeActors.CheckedChanged
         Form1.rescrapeList.actors = cbRescrapeActors.Checked
    End Sub

    Private Sub cbRescrapeMediaTags_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRescrapeMediaTags.CheckedChanged
        Form1.rescrapeList.mediatags = cbRescrapeMediaTags.Checked
    End Sub

    Private Sub cbRescrapePosterUrls_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRescrapePosterUrls.CheckedChanged
        Form1.rescrapeList.posterurls = cbRescrapePosterUrls.Checked 
    End Sub

    Private Sub cbMissingFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMissingFanart.CheckedChanged
        If cbMissingFanart.CheckState = CheckState.Checked Then
            If Pref.savefanart Then
                Form1.rescrapeList.missingfanart = True
            Else
                Form1.rescrapeList.missingfanart = False
                cbMissingFanart.Checked = False
                MsgBox("Movie Preferences set to not download Fanart.", 48, "Movie Preferences")
            End If
        Else
            Form1.rescrapeList.missingfanart = False
        End If
    End Sub

    Private Sub cbMissingPosters_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMissingPosters.CheckedChanged
        If cbMissingPosters.CheckState = CheckState.Checked Then
            If Pref.scrapemovieposters Then
                Form1.rescrapeList.missingposters = True
            Else
                Form1.rescrapeList.missingposters = False
                cbMissingPosters.Checked = False
                MsgBox("Movie Preferences set to not download Posters.", 48, "Movie Preferences")
            End If
        Else
            Form1.rescrapeList.missingposters = False
        End If
    End Sub

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        Try
            Me.Close()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Form1.rescrapeList.ResetFields
            Me.Close()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub cbMainCountry_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbMainCountry.CheckedChanged
        Form1.rescrapeList.country = cbMainCountry.Checked
    End Sub

    Private Sub cbMainStars_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbMainStars.CheckedChanged
        Form1.rescrapeList.stars =cbMainStars.Checked
    End Sub
    Private Sub CcbMainYear_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbMainYear.CheckedChanged
        Form1.rescrapeList.year = cbMainYear.Checked
    End Sub

    Private Sub cbMainImdbAspectRatio_CheckedChanged(sender As Object, e As EventArgs) Handles cbMainImdbAspectRatio.CheckedChanged
        Form1.rescrapeList.imdbaspect = cbMainImdbAspectRatio.Checked
    End Sub

    Private Sub cbFromTMDB_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbFromTMDB.CheckedChanged
        Form1.rescrapeList.FromTMDB = cbFromTMDB.Checked
    End Sub
    
    Private Sub cbMainMetascore_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbMainMetascore.CheckedChanged
        Form1.rescrapelist.metascore = cbMainMetascore.Checked
    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        
    End Sub

    ''' <summary>
    ''' Choose if wanting to update Collection Title only
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cbMainTmdbSetName_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbMainTmdbSetName.CheckedChanged
        Form1.rescrapeList.tmdb_set_name = cbMainTmdbSetName.Checked
    End Sub
    
    ''' <summary>
    ''' Choose if wanting to update collection information completely
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cbMainTmdbSetinfo_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbMainTmdbSetinfo.CheckedChanged
        Form1.rescrapeList.tmdb_set_info = cbMainTmdbSetinfo.Checked
    End Sub

    ReadOnly Property FilesRenamable As Boolean
        Get
            If Pref.basicsavemode OrElse Not Pref.MovieManualRename Then Return False
            If Pref.usefoldernames Then
                Dim tempint As Integer = MessageBox.Show("You currently have 'UseFolderName' Selected" & vbCrLf & "Are you sure you wish to Rename this Movie file" & vbCrLf & "Folder Renaming will still commence", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If tempint = DialogResult.No Then
                    Return False
                Else 
                    Return True
                End If
            End If
            If Pref.MovieManualRename Then Return True
        End Get
    End Property

    ReadOnly Property FoldersRenamable As Boolean
        Get
            Return Pref.MovieManualRename
        End Get
    End Property

    'Disabled controls don't show tool tips (friggin' MS poo), so need to filter invalid changes 
    Private Sub cbRenameFiles_CheckedChanged( sender As Object,  e As EventArgs) Handles cbRenameFiles.CheckedChanged
        If cbRenameFiles.Checked AndAlso FilesRenamable Then
            Form1.rescrapeList.Rename_Files = cbRenameFiles.Checked
        Else
            cbRenameFiles.Checked = False
        End If
    End Sub

    Private Sub cbRenameFolders_CheckedChanged( sender As Object,  e As EventArgs) Handles cbRenameFolders.CheckedChanged
        If FoldersRenamable Then
            Form1.rescrapeList.Rename_Folders = cbRenameFolders.Checked
        Else
            cbRenameFolders.Checked = False
        End If
    End Sub

    Private Sub cbTagsFromKeywords_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbTagsFromKeywords.CheckedChanged
        Form1.rescrapeList.TagsFromKeywords = cbTagsFromKeywords.Checked
    End Sub

    'Fix MS XP tool tip won't display a second time bug (more friggin' MS poo)
    Private Sub ForceToolTipDisplay( sender As System.Object,  e As System.EventArgs) Handles cbRenameFiles.MouseEnter,cbFrodo_Poster_Thumbs.MouseEnter,cbFrodo_Fanart_Thumbs.MouseEnter,cbRenameFolders.MouseEnter
        ttBatchUpdateWizard.Active = False
        ttBatchUpdateWizard.Active = True
    End Sub


    Private Sub cbFrodo_Poster_Thumbs_CheckedChanged( sender As Object,  e As EventArgs) Handles cbFrodo_Poster_Thumbs.CheckedChanged
        If Pref.FrodoEnabled Then
            Form1.rescrapeList.Frodo_Poster_Thumbs = cbFrodo_Poster_Thumbs.Checked
        Else
            cbFrodo_Poster_Thumbs.Checked = False
        End If
    End Sub
    
    Private Sub cbFrodo_Fanart_Thumbs_CheckedChanged( sender As Object,  e As EventArgs) Handles cbFrodo_Fanart_Thumbs.CheckedChanged
        If Pref.FrodoEnabled Then
            Form1.rescrapeList.Frodo_Fanart_Thumbs = cbFrodo_Fanart_Thumbs.Checked
        Else
            cbFrodo_Fanart_Thumbs.Checked = False
        End If
    End Sub

    Private Sub cbXtraFanart_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbXtraFanart.CheckedChanged
        Form1.rescrapeList.dlxtraart = cbXtraFanart.Checked 
    End Sub

    Private Sub cbFanartTv_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbFanartTv.CheckedChanged
        Form1.rescrapeList.ArtFromFanartTv = cbFanartTv.Checked 
    End Sub
    
    Private Sub cbMissingMovSetArt_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbMissingMovSetArt.CheckedChanged
        Form1.rescrapeList.missingmovsetart = cbMissingMovSetArt.Checked 
    End Sub

    Private Sub cbDlTrailer_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cbDlTrailer.CheckedChanged
        Form1.rescrapeList.Download_Trailer = cbDlTrailer.Checked 
    End Sub

    Private Sub cbMainTitle_CheckedChanged( sender As Object,  e As EventArgs) Handles cbMainTitle.CheckedChanged
        Form1.rescrapeList.title = cbMainTitle.Checked
    End Sub

    Private Sub frmBatchScraper_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then btnCancel.PerformClick 
    End Sub

    Private Sub cb_ScrapeEmptyTags_CheckedChanged( sender As System.Object,  e As System.EventArgs) Handles cb_ScrapeEmptyTags.CheckedChanged
        Dim ChkBox As CheckBox = Nothing
        Dim Ischecked As Boolean = cb_ScrapeEmptyTags.CheckState 
        For Each xObject As Object In Me.gpbxMainTagsToRescrape.Controls
            If TypeOf xObject Is CheckBox Then
                ChkBox = xObject
                If Not ChkBox.Text.ToLower.Contains("tmdb") And Not ChkBox.Text.ToLower.Contains("trailer")Then
                ChkBox.Checked = Ischecked
                End If
            End If
        Next
        Form1.rescrapeList.EmptyMainTags = cb_ScrapeEmptyTags.checked
    End Sub

    Private Sub frmBatchScraper_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
    End Sub

    
End Class