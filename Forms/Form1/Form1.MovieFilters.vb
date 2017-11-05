'
' Form1 Movie filters stuff should go here
' Form1 is way too big - If you're reading this and you're v. bored, please feel free to split out From1 code 
' by context, into manageable chucks like this. 
'
Imports Media_Companion.Pref
Imports System.Linq

Partial Public Class Form1


    Friend WithEvents lblFilterLockedMode As Label
    Friend WithEvents lblFilterLocked     As Label
    Friend WithEvents cbFilterLocked      As MC_UserControls.TriStateCheckedComboBox

    Friend WithEvents lblFilterHdrMode    As Label
    Friend WithEvents lblFilterHdr        As Label
    Friend WithEvents cbFilterHdr         As MC_UserControls.TriStateCheckedComboBox


	Public ReadOnly Property MovieFiltersPanel As Panel
		Get
			Return  SpltCntr_MovBrowserList.Panel2
		End Get
	End Property	
     

	Public ReadOnly Property MovieTriStateCheckedComboBoxFilters As List(Of MC_UserControls.TriStateCheckedComboBox)
		Get
			Dim res = From X In MovieFiltersPanel.Controls Where X.GetType Is GetType(MC_UserControls.TriStateCheckedComboBox)

			Return (From X As MC_UserControls.TriStateCheckedComboBox In res).ToList
		End Get
	End Property	

	ReadOnly Property MovieFilters As List(Of Control)
		Get
			Dim res = From c As Control In MovieFiltersPanel.Controls Where c.Name.IndexOf("cbFilter") = 0 And c.GetType().Namespace = "MC_UserControls"
			Return res.ToList
		End Get
	End Property	

	ReadOnly Property MovieFilterLabels As List(Of Label)
		Get
			Dim res = From c As Control In MovieFiltersPanel.Controls Where c.Name.IndexOf("lblFilter") = 0 AndAlso Not c.Name.EndsWith("Mode") AndAlso Not (c.Name="lblFilterGeneral")
			Return (From X As Label In res).ToList
		End Get
	End Property	

	ReadOnly Property MovieFilterModeLabels As List(Of Label)
		Get
			Dim res = From c As Control In MovieFiltersPanel.Controls Where c.Name.IndexOf("lblFilter") = 0 AndAlso c.Name.EndsWith("Mode")
			Return (From X As Label In res).ToList
		End Get
	End Property	

     
	ReadOnly Property MovieFiltersPanelMaxHeight As Integer
		Get
			Return Pref.movie_filters.CalculatedFilterPanelHeight
		End Get
	End Property	
		
	
	 
    Sub IniMovieFilters
        CreateTriStateFilter("Locked", lblFilterLockedMode, lblFilterLocked, cbFilterLocked)
        CreateTriStateFilter("HDR"   , lblFilterHdrMode   , lblFilterHdr   , cbFilterHdr   )

		AttachMovieFilterEventHandlers
    End Sub
   

    Sub CreateTriStateFilter(name As String, ByRef lblFilterMode As Label, ByRef lblFilter As Label, ByRef cbFilter As MC_UserControls.TriStateCheckedComboBox)

      Dim panel = MovieFiltersPanel

		lblFilterMode = CreateFilterLabel            ("lblFilter" & name & "Mode", "M"  , panel, 129,  17)
		lblFilter     = CreateFilterLabel            ("lblFilter" & name         , name , panel,   4, 124)
      cbFilter      = CreateTriStateCheckedComboBox("cbFilter"  & name                , panel, 147     )
    End Sub


    Function CreateFilterLabel(name As String, text As String, panel As Panel, x As Integer, width As Integer)

        Dim lbl      As New Label
        Dim y        = 100
        Dim tabIndex = 100

        panel.Controls.Add(lbl)

        lbl.Anchor    = CType((Windows.Forms.AnchorStyles.Bottom Or Windows.Forms.AnchorStyles.Left), Windows.Forms.AnchorStyles)
        lbl.BackColor = Drawing.Color.Gray
        lbl.Font      = New Drawing.Font("Microsoft Sans Serif", 9.0!, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CType(0, Byte))
        lbl.ForeColor = Drawing.Color.White
        lbl.Location  = New Drawing.Point(x, y)
        lbl.Margin    = New Windows.Forms.Padding(4, 0, 4, 0)
        lbl.Name      = name
        lbl.Size      = New Drawing.Size(width, 21)
        lbl.TabIndex  = tabIndex
        lbl.Text      = text
        lbl.TextAlign = Drawing.ContentAlignment.MiddleLeft

        Return lbl
    End Function

	
    Function CreateTriStateCheckedComboBox(name As String, panel As Panel, x As Integer)

        Dim cb  As New MC_UserControls.TriStateCheckedComboBox()
        Dim y        = 100
        Dim tabIndex = 100
        Dim tag      = "100"
	
        panel.Controls.Add(cb)
           
        cb.Anchor                     = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        cb.BackColor                  = Drawing.SystemColors.Control
        cb.Font                       = New Drawing.Font("Microsoft Sans Serif", 9.0!, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Point, CType(0, Byte))
        cb.CheckOnClick               = true
        cb.DisplayWhenNothingSelected = "All"
        cb.DrawMode                   = Windows.Forms.DrawMode.OwnerDrawVariable
        cb.DropDownHeight             = 1
        cb.DropDownStyle              = Windows.Forms.ComboBoxStyle.DropDownList
        cb.FormattingEnabled          = true
        cb.IntegralHeight             = false
        cb.Location                   = New Drawing.Point(x,y)
        cb.Mode                       = MC_UserControls.TriStateCheckedComboBox.OperationMode.Follow
        cb.Name                       = name
        cb.QuickSelect                = false
        cb.Size                       = New Drawing.Size(0, 22)
        cb.TabIndex                   = 228
        cb.Tag                        = tag
        cb.ValueSeparator             = " "  
  
        Return cb
    End Function



    Private Sub UpdateFilteredList

        oMovies.UpdateTmdbSetMissingMovies()

        Dim lastSelectedRow As Integer = 0
        If DataGridViewMovies.SelectedRows.Count = 1 Then 
            lastSelectedRow = DataGridViewMovies.SelectedRows(0).Index 
        End If
        ProgState = ProgramState.UpdatingFilteredList
        Dim lastSelectedMovie = workingMovie.fullpathandfilename
        filteredList.Clear
        filteredList.AddRange(oMovies.MovieCache)
        Assign_FilterGeneral()
        UpdateMinMaxMovieFilters

        If cbFilterCountries            .Visible Then cbFilterCountries             .UpdateItems( oMovies.CountriesFilter               )
        If cbFilterStudios              .Visible Then cbFilterStudios               .UpdateItems (oMovies.StudiosFilter                 )
        If cbFilterGenre                .Visible Then cbFilterGenre                 .UpdateItems( oMovies.GenresFilter                  )
        If cbFilterCertificate          .Visible Then cbFilterCertificate           .UpdateItems( oMovies.CertificatesFilter            )
        If cbFilterSet                  .Visible Then cbFilterSet                   .UpdateItems( oMovies.SetsFilter                    )
        If cbFilterTag                  .Visible Then cbFilterTag                   .UpdateItems( oMovies.TagFilter                     )
        If cbFilterResolution           .Visible Then cbFilterResolution            .UpdateItems( oMovies.ResolutionFilter              )
        If cbFilterVideoCodec           .Visible Then cbFilterVideoCodec            .UpdateItems( oMovies.VideoCodecFilter              )
        If cbFilterAudioCodecs          .Visible Then cbFilterAudioCodecs           .UpdateItems( oMovies.AudioCodecsFilter             )
        If cbFilterAudioChannels        .Visible Then cbFilterAudioChannels         .UpdateItems( oMovies.AudioChannelsFilter           )
        If cbFilterAudioBitrates        .Visible Then cbFilterAudioBitrates         .UpdateItems( oMovies.AudioBitratesFilter           )
        If cbFilterNumAudioTracks       .Visible Then cbFilterNumAudioTracks        .UpdateItems( oMovies.NumAudioTracksFilter          )
        If cbFilterAudioLanguages       .Visible Then cbFilterAudioLanguages        .UpdateItems( oMovies.AudioLanguagesFilter          )
        If cbFilterAudioDefaultLanguages.Visible Then cbFilterAudioDefaultLanguages .UpdateItems( oMovies.AudioDefaultLanguagesFilter   )
        If cbFilterActor                .Visible Then cbFilterActor                 .UpdateItems( oMovies.ActorsFilter                  )
        If cbFilterDirector             .Visible Then cbFilterDirector              .UpdateItems( oMovies.DirectorsFilter               )
        If cbFilterTag                  .Visible Then cbFilterTag                   .UpdateItems( oMovies.TagsFilter                    )
        If cbFilterSubTitleLang         .Visible Then cbFilterSubTitleLang          .UpdateItems( oMovies.SubTitleLangFilter            )
        If cbFilterRootFolder           .Visible Then cbFilterRootFolder            .UpdateItems( oMovies.RootFolderFilter              )
        If cbFilterUserRated            .Visible Then cbFilterUserRated             .UpdateItems( oMovies.UserRatedFilter               )
        If cbFilterLocked               .Visible Then cbFilterLocked                .UpdateItems( oMovies.LockedFilter                  )
        If cbFilterHdr                  .Visible Then cbFilterHdr                   .UpdateItems( oMovies.HdrFilter                     )
                   
                 
                   
                                          
        Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
        Try
				Dim selMovie = (From x As datagridviewrow In DataGridViewMovies.Rows Where x.Cells("fullpathandfilename").Value.ToString = lastSelectedMovie).FirstOrDefault

				If Not IsNothing(selMovie) Then
					DataGridViewMovies.ClearSelection
					selMovie.Selected = True
				End If

            'If DataGridViewMovies.SelectedRows.Count = 0 Then
                'For Each row As DataGridViewRow In DataGridViewMovies.Rows
                '    row.Selected = (row.Cells("fullpathandfilename").Value.ToString = lastSelectedMovie)
                'Next
            'ElseIf DataGridViewMovies.SelectedRows.Count = 1 Then
            '    DataGridViewMovies.ClearSelection()
            '    DataGridViewMovies.Rows(lastSelectedRow).Selected = True
            'End If
        Catch
        End Try
        
        If DataGridViewMovies.SelectedRows.Count=0 And DataGridViewMovies.Rows.Count>0 Then
            DataGridViewMovies.Rows(0).Selected=True
        End If
        DisplayMovie()
        ProgState = ProgramState.Other
    End Sub

    Sub Assign_FilterGeneral
        If cbFilterGeneral.Visible Then
            Dim selected = cbFilterGeneral.Text

            cbFilterGeneral.Items.Clear
            cbFilterGeneral.Items.AddRange( oMovies.GeneralFilters.ToArray )

            If cbFilterGeneral.Text = "" Then cbFilterGeneral.Text = "All"

            If selected<>"" Then
                For Each item As String In cbFilterGeneral.Items
                    If item.RemoveAfterMatch=selected.RemoveAfterMatch Then
                        cbFilterGeneral.SelectedItem=item    
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub


	Sub ShowMovieFilter(cbFilter As Control)
		If Not cbFilter.Visible Then
			movie_filters.GetItem(cbFilter.Name).Visible = True
			Pref.movie_filters.SetMovieFiltersVisibility
			UpdateMovieFiltersPanel
		End If
	End Sub

	Sub AttachMovieFilterEventHandlers

		For Each X In MovieTriStateCheckedComboBoxFilters
			AddHandler X.OnFormatItem , AddressOf TriStateFilter_OnFormatItem
			AddHandler X.TextChanged  , AddressOf cbFilterChanged
		Next
		
		For Each X In MovieFilterLabels
			AddHandler X.Click        , AddressOf ResetFilter
		Next

		For Each X In MovieFilterModeLabels
			AddHandler X.Click        , AddressOf TriStateFilter_ChangeFilterMode
		Next


	End Sub

	'Private Sub TriStateFilter_ChangeFilterMode(ByVal sender As Object, ByVal e As EventArgs) Handles lblFilterGenreMode.Click, lblFilterSetMode.Click, lblFilterResolutionMode.Click,
	'																											  lblFilterAudioCodecsMode.Click, lblFilterCertificateMode.Click, lblFilterAudioChannelsMode.Click,
	'																											  lblFilterAudioBitratesMode.Click, lblFilterNumAudioTracksMode.Click, lblFilterAudioLanguagesMode.Click,
	'																											  lblFilterActorMode.Click, lblFilterSourceMode.Click, lblFilterTagMode.Click, lblFilterDirectorMode.Click,
	'																											  lblFilterVideoCodecMode.Click, lblFilterSubTitleLangMode.Click, lblFilterAudioDefaultLanguagesMode.Click,
	'																											  lblFilterCountriesMode.Click, lblFilterStudiosMode.Click, lblFilterRootFolderMode.Click,
	'																											  lblFilterUserRatedMode.Click, lblFilterLockedMode.Click
    Private Sub TriStateFilter_ChangeFilterMode(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = sender
        Dim filter As MC_UserControls.TriStateCheckedComboBox = GetFilterFromLabel(lbl)

        filter.QuickSelect = Not filter.QuickSelect

        lbl.Text = If(filter.QuickSelect, "S", "M")

        movie_filters.GetItem(filter.Name).QuickSelect = filter.QuickSelect
    End Sub




    'Private Function TriStateFilter_OnFormatItem(ByVal item As String) As String Handles cbFilterGenre.OnFormatItem, cbFilterCertificate.OnFormatItem,
    '                                                                                cbFilterSet.OnFormatItem, cbFilterResolution.OnFormatItem,
    '                                                                                cbFilterAudioCodecs.OnFormatItem, cbFilterAudioChannels.OnFormatItem,
    '                                                                                cbFilterAudioBitrates.OnFormatItem, cbFilterNumAudioTracks.OnFormatItem,
    '                                                                                cbFilterAudioLanguages.OnFormatItem, cbFilterActor.OnFormatItem,
    '                                                                                cbFilterSource.OnFormatItem, cbFilterTag.OnFormatItem, cbFilterTag.OnFormatItem,
    '                                                                                cbFilterDirector.OnFormatItem, cbFilterVideoCodec.OnFormatItem, cbFilterSubTitleLang.OnFormatItem,
    '                                                                                cbFilterAudioDefaultLanguages.OnFormatItem, cbFilterCountries.OnFormatItem, 
    '                                                                                cbFilterStudios.OnFormatItem, cbFilterRootFolder.OnFormatItem, cbFilterUserRated.OnFormatItem, cbFilterLocked.OnFormatItem
	 Private Function TriStateFilter_OnFormatItem(ByVal item As String) As String
        Return item.RemoveAfterMatch
    End Function


    Private Sub cbFilterChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterGeneral.SelectedValueChanged

        If TypeName(sender) = "TriStateCheckedComboBox" Then
            Dim x As MC_UserControls.TriStateCheckedComboBox = sender
            If x.opState <> 0 Then
                Return
            End If
        End If
        ApplyMovieFilters
    End Sub
     
    Private Sub cbFilterRatingChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterRating.SelectionChanged, cbFilterVotes.SelectionChanged,
                                                                                                          cbFilterRuntime.SelectionChanged, cbFilterFolderSizes.SelectionChanged,
                                                                                                          cbFilterYear.SelectionChanged
        ApplyMovieFilters
    End Sub   

    'Private Sub ResetFilter(sender As Object, e As EventArgs) Handles lblFilterSet.Click, lblFilterVotes.Click, lblFilterRating.Click,
    '                                                                    lblFilterCertificate.Click, lblFilterGenre.Click, lblFilterYear.Click,
    '                                                                    lblFilterResolution.Click, lblFilterAudioCodecs.Click, lblFilterAudioChannels.Click,
    '                                                                    lblFilterAudioBitrates.Click, lblFilterNumAudioTracks.Click, lblFilterAudioLanguages.Click,
    '                                                                    lblFilterActor.Click, lblFilterSource.Click, lblFilterTag.Click,
    '                                                                    lblFilterDirector.Click, lblFilterVideoCodec.Click, lblFilterSubTitleLang.Click,
    '                                                                    lblFilterFolderSizes.Click, lblFilterRuntime.Click, lblFilterAudioDefaultLanguages.Click,
    '                                                                    lblFilterCountries.Click, lblFilterStudios.Click, lblFilterRootFolder.Click,
    '                                                                    lblFilterUserRated.Click, lblFilterLocked.Click
    Private Sub ResetFilter(sender As Object, e As EventArgs)
        Dim filter As Object = GetFilterFromLabel(sender)

        ProgState = ProgramState.ResettingFilters
        filter.Reset()
        ProgState = ProgramState.Other

        UpdateFilteredList()
    End Sub   
   

    Private Sub cbFilterBeginSliding(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterRuntime.BeginSliding, cbFilterYear.BeginSliding, cbFilterVotes.BeginSliding, cbFilterRating.BeginSliding, cbFilterFolderSizes.BeginSliding
        MovieFiltersPanel.ContextMenuStrip = Nothing
    End Sub


    Private Sub cbFilterEndSliding(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterRuntime.EndSliding, cbFilterYear.EndSliding, cbFilterVotes.EndSliding, cbFilterRating.EndSliding, cbFilterFolderSizes.EndSliding
        MovieFiltersPanel.ContextMenuStrip = cmsConfigureMovieFilters
    End Sub


    Private Sub ApplyMovieFilters
        tsmiMov_ConvertToFrodo.Enabled = (cbFilterGeneral.Text.RemoveAfterMatch="Pre-Frodo poster only") or (cbFilterGeneral.Text.RemoveAfterMatch="Both poster formats")
        If ProgState = ProgramState.Other Then
            Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)
            DisplayMovie
        End If
    End Sub


    Sub HandleMovieFilter_SelectedValueChanged(cbFilter As ComboBox, ByRef filterValue As String, Optional replaceUnknown As Boolean = False)
        If ProgState = ProgramState.Other Then
            If cbFilter.Text = "All" Then
                filterValue = ""
            Else
                filterValue = cbFilter.Text.RemoveAfterMatch
                If replaceUnknown Then filterValue = filterValue.Replace("Unknown","-1")
            End If
            ApplyMovieFilters
        End If
    End Sub


    Private Sub ConfigureMovieFiltersToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ConfigureMovieFiltersToolStripMenuItem1.Click
        Dim frm As New frmConfigureMovieFilters

        frm.Init(MovieFiltersPanel)

        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            UpdateMovieFiltersPanel()
            Pref.ConfigSave()
            UpdateFilteredList()
        End If
    End Sub


    Sub UpdateMovieFiltersPanel()
        'ResizeBottomLHSPanel
        Pref.movie_filters.UpdateFromPanel()
        ResizeBottomLHSPanel(MovieFiltersPanelMaxHeight)
        Pref.movie_filters.PositionMovieFilters()
    End Sub


    Private Sub SplitContainer5_DoubleClick(sender As Object, e As EventArgs) Handles SpltCntr_MovBrowserList.DoubleClick

        If MovieFiltersPanel.Height = MovieFiltersPanelMaxHeight - 5 Then
            ResizeBottomLHSPanel(0)
        Else
            ResizeBottomLHSPanel(MovieFiltersPanelMaxHeight)
        End If
    End Sub


    Private Sub ResizeBottomLHSPanel(height As Integer, Optional ByVal MaxHeight As Integer = 0)
        ProgState = ProgramState.ResizingSplitterPanel

        SpltCntr_MovBrowserList.SplitterDistance = If((SpltCntr_MovBrowserList.Height - height) < 0, 0, SpltCntr_MovBrowserList.Height - height)

        DataGridViewMovies.Height = SpltCntr_MovBrowserList.SplitterDistance - 140

        If MaxHeight = 0 Then
            MovieFiltersPanel.AutoScrollMinSize = New Size(MovieFiltersPanel.AutoScrollMinSize.Width, height - 10)
        Else
            MovieFiltersPanel.AutoScrollMinSize = New Size(MovieFiltersPanel.AutoScrollMinSize.Width, MaxHeight - 10)
        End If

        ProgState = ProgramState.Other
    End Sub


    Private Sub ResizeBottomLHSPanel()
        If ProgState = ProgramState.ResizingSplitterPanel Then Return

        If Not MainFormLoadedStatus Then Return
        If Not MovieFiltersPanel.Visible Then Return

        Dim maxSize = MovieFiltersPanelMaxHeight
        Dim minSize = 2

        If SpltCntr_MovBrowserList.Height - SpltCntr_MovBrowserList.SplitterDistance > maxSize Then
            SpltCntr_MovBrowserList.SplitterDistance = SpltCntr_MovBrowserList.Height - maxSize
        End If

        If SpltCntr_MovBrowserList.Height - SpltCntr_MovBrowserList.SplitterDistance < minSize Then
            SpltCntr_MovBrowserList.SplitterDistance = SpltCntr_MovBrowserList.Height - minSize
        End If

        'Needed as workaround for splitter panel framework bug:
        Dim h = SpltCntr_MovBrowserList.SplitterDistance - 140
        If h < minSize Then h = minSize
        DataGridViewMovies.Height = h
    End Sub


    Private Function GetFilterFromLabel(ctl As Control)

        Dim name As String = ctl.Name.RemoveAfterMatch("Mode")

        Return ctl.Parent.Controls("cb" + name.Substring(3, name.Length - 3))
    End Function


    Private Sub ResetCbGeneralFilter(sender As Control, e As EventArgs) Handles lblFilterGeneral.Click
        cbFilterGeneral.SelectedIndex = 0
    End Sub


    Private Sub cbFilterGeneral_DropDown(sender As Object, e As EventArgs) Handles cbFilterGeneral.DropDown

        Dim maxWidth As Integer = cbFilterGeneral.DropDownWidth
        Dim g As Graphics = cbFilterGeneral.CreateGraphics
        Dim vertScrollBarWidth As Integer = If(cbFilterGeneral.Items.Count > cbFilterGeneral.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)
        Dim renderedWidth As Integer
        Dim lbl As Label = New Label()

        lbl.AutoSize = True
        lbl.Font = cbFilterGeneral.Font

        For Each item As String In cbFilterGeneral.Items
            lbl.Text = item
            renderedWidth = lbl.PreferredSize.Width + vertScrollBarWidth
            maxWidth = Math.Max(maxWidth, renderedWidth)
        Next

        cbFilterGeneral.DropDownWidth = maxWidth
    End Sub



	Public Sub resetallfilters()
		Try
			ResetFilters()
			Mc.clsGridViewMovie.mov_FiltersAndSortApply(Me)

			Try
				If DataGridViewMovies.SelectedRows.Count = 1 Then
					If workingMovieDetails.fileinfo.fullpathandfilename = CType(DataGridViewMovies.SelectedRows(0).DataBoundItem, Data_GridViewMovie).fullpathandfilename.ToString Then Return
				End If
			Catch
			End Try

			DisplayMovie()
		Catch ex As Exception
			ExceptionHandler.LogError(ex)
		End Try
	End Sub

	Sub ResetFilters()
		ProgState = ProgramState.ResettingFilters
		filterOverride = False
		TextBox1.Text = ""
		txt_titlesearch.Text = ""
		txt_titlesearch.BackColor = Color.White
		TextBox1.BackColor = Color.White
		rbTitleAndYear.Checked = True
		cbFilterGeneral.SelectedIndex = 0
		UpdateMinMaxMovieFilters()
		oMovies.ActorsFilter_AlsoInclude.Clear()
		oMovies.SetsFilter_AlsoInclude.Clear()
		cbFilterActor.UpdateItems(oMovies.ActorsFilter)
		cbFilterDirector.UpdateItems(oMovies.DirectorsFilter)
		cbFilterSet.UpdateItems(oMovies.SetsFilter)
		cbFilterTag.UpdateItems(oMovies.TagsFilter)

		For Each c As Object In MovieFilters
			c.Reset()
		Next
		ProgState = ProgramState.Other
	End Sub


	Public Sub UpdateMovieSetDisplayNames
		oMovies.UpdateMovieSetDisplayNames
		pop_cbMovieDisplay_MovieSet
		cbFilterSet.UpdateItems(oMovies.SetsFilter)
	End Sub

	Sub UpdateMinMaxMovieFilters()
		If cbFilterVotes.Visible Then cbFilterVotes.Values = oMovies.ListVotes
		If cbFilterRuntime.Visible Then cbFilterRuntime.Values = oMovies.ListRuntimes

		If cbFilterFolderSizes.Visible Then
			cbFilterFolderSizes.Min = oMovies.MinFolderSize
			cbFilterFolderSizes.Max = oMovies.MaxFolderSize
		End If
		If cbFilterYear.Visible Then
			cbFilterYear.Min = If(oMovies.MinYear < 1850, 1850, oMovies.MinYear)
			cbFilterYear.Max = oMovies.MaxYear
		End If
	End Sub



End Class
