'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Linq

Public Class frmCreateDateFix
    Private _lst As New DataTable
    Private _timeUnit As Integer = 0
    Private _timePeriodDays As Integer = 0
    Private _totalCheckBoxes As Integer = 0
    Private _totalCheckedCheckBoxes As Integer = 0
    Private _headerCheckBox As CheckBox = Nothing
    Private _isHeaderCheckBoxClicked As Boolean = False
    Private _dateFixDataGridViewBindingSource As New BindingSource

    Private Sub frmCreateDateFix_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Form1.oMovies.SaveMovieCache()
    End Sub

    Private Sub frmCreateDateFix_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub



    Private Sub frmCreateDateFix_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Set up form elements
        AddHeaderCheckBox()
        initTimePeriodList()
        _lst.Columns.Add("Title", GetType(String))
        _lst.Columns.Add("CreateDate", GetType(Date))
        _lst.Columns.Add("FileDate", GetType(Date))
        _lst.Columns.Add("NFOpath", GetType(String))

        'Add event handlers
        AddHandler _headerCheckBox.KeyUp, New KeyEventHandler(AddressOf HeaderCheckBox_KeyUp)
        AddHandler _headerCheckBox.MouseClick, New MouseEventHandler(AddressOf HeaderCheckBox_MouseClick)
        AddHandler dateFixDataGridView.CellValueChanged, New DataGridViewCellEventHandler(AddressOf dgvDateFix_CellValueChanged)
        AddHandler dateFixDataGridView.CurrentCellDirtyStateChanged, New EventHandler(AddressOf dgvDateFix_CurrentCellDirtyStateChanged)
        AddHandler dateFixDataGridView.CellPainting, New DataGridViewCellPaintingEventHandler(AddressOf dgvDateFix_CellPainting)

        'Get movie list from cache
        GetDataSource()

        'Set up the DataGridView
        dateFixDataGridView.DataSource = _dateFixDataGridViewBindingSource
        dateFixDataGridView.Columns("CreateDate").Width = 115
        dateFixDataGridView.Columns("FileDate").Width = 115
        dateFixDataGridView.Columns("Title").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dateFixDataGridView.Columns("NFOpath").Visible = False
        dateFixDataGridView.AllowUserToAddRows = False

        _totalCheckBoxes = dateFixDataGridView.RowCount
        _totalCheckedCheckBoxes = 0
    End Sub

    Private Sub GetDataSource()
        'Store the movie list as a source for this form
        _lst.Rows.Clear()
        Dim data = From c In Form1.oMovies.Data_GridViewMovieCache Select New With {Key .Title = c.DisplayTitleAndYear, _
                                                                                    Key .CreateDate = c.createdate, _
                                                                                    Key .File = c.MoviePathAndFileName, _
                                                                                    Key .NFOpath = c.fullpathandfilename}
        For Each row In data
            'Dim i As Integer
            'If File.Exists(row.File) AndAlso Integer.TryParse(row.CreateDate, i) Then   'row.Title and row.NFOpath wil add, even if empty - test NFO later
            Try
                _lst.Rows.Add(row.Title, _
                              Date.ParseExact(row.CreateDate, Pref.datePattern, Globalization.DateTimeFormatInfo.InvariantInfo), _
                              File.GetLastWriteTime(row.File), _
                              row.NFOpath)
            Catch ex As Exception
                'Originally thought testing each item would be the way to go, but Try...Catch will do, and probably faster!
            End Try
            'End If
        Next

        BindGridView()
    End Sub

    Private Sub BindGridView()
        'Update selection and bind it to DataGridView
        Dim dTable As DataTable = _lst.Clone
        For Each row In _lst.Rows
            Dim span As TimeSpan = row.Item("CreateDate") - row.Item("FileDate")
            If span.Duration.Days > (_timeUnit * _timePeriodDays) Then dTable.ImportRow(row)
        Next
        _dateFixDataGridViewBindingSource.DataSource = dTable
        _totalCheckBoxes = dateFixDataGridView.RowCount
        _totalCheckedCheckBoxes = 0
        If Not IsNothing(_headerCheckBox) Then _headerCheckBox.Checked = False
        updateStatusText()
    End Sub

    Private Sub initTimePeriodList()
        'Create combobox text and values in days
        Dim lstTime As New DataTable
        lstTime.Columns.Add("TimePeriod", GetType(String))
        lstTime.Columns.Add("InDays", GetType(Integer))
        lstTime.Columns.Add("Default", GetType(Boolean))
        lstTime.Rows.Add("days", 1, True)
        lstTime.Rows.Add("weeks", 7)
        lstTime.Rows.Add("months", 30)
        lstTime.Rows.Add("years", 365)

        'Set up combobox
        cmbxDateFix.DataSource = lstTime
        cmbxDateFix.DisplayMember = "TimePeriod"
        cmbxDateFix.ValueMember = "InDays"
        cmbxDateFix.SelectedIndex = lstTime.Rows.IndexOf(lstTime.Select("Default=true").FirstOrDefault)
    End Sub

    Private Sub updateStatusText()
        statuslblDateFix.Text = String.Format("{0} Title{1} Selected", _totalCheckedCheckBoxes, If(_totalCheckedCheckBoxes = 1, "", "s"))
    End Sub

    Private Sub dgvDateFix_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)
        If Not _isHeaderCheckBoxClicked Then
            RowCheckBoxClick(DirectCast(dateFixDataGridView(e.ColumnIndex, e.RowIndex), DataGridViewCheckBoxCell))
        End If
    End Sub

    Private Sub dgvDateFix_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs)
        If TypeOf dateFixDataGridView.CurrentCell Is DataGridViewCheckBoxCell Then
            dateFixDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub HeaderCheckBox_MouseClick(sender As Object, e As MouseEventArgs)
        HeaderCheckBoxClick(DirectCast(sender, CheckBox))
    End Sub

    Private Sub HeaderCheckBox_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Space Then
            HeaderCheckBoxClick(DirectCast(sender, CheckBox))
        End If
    End Sub

    Private Sub dgvDateFix_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs)
        If e.RowIndex = -1 AndAlso e.ColumnIndex = 0 Then
            ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex)
        End If
    End Sub

    Private Sub AddHeaderCheckBox()
        _headerCheckBox = New CheckBox()
        _headerCheckBox.Size = New Size(15, 15)

        'Add the CheckBox into the DataGridView
        Me.dateFixDataGridView.Controls.Add(_headerCheckBox)
    End Sub

    Private Sub ResetHeaderCheckBoxLocation(ColumnIndex As Integer, RowIndex As Integer)
        'Get the column header cell bounds
        Dim oRectangle As Rectangle = Me.dateFixDataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, True)
        Dim oPoint As New Point()

        oPoint.X = oRectangle.Location.X + (oRectangle.Width - _headerCheckBox.Width) \ 2 + 1
        oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - _headerCheckBox.Height) \ 2 + 1

        'Change the location of the CheckBox to make it stays on the header
        _headerCheckBox.Location = oPoint
    End Sub

    Private Sub HeaderCheckBoxClick(HCheckBox As CheckBox)
        _isHeaderCheckBoxClicked = True

        For Each Row As DataGridViewRow In dateFixDataGridView.Rows
            DirectCast(Row.Cells("chkBxSelect"), DataGridViewCheckBoxCell).Value = HCheckBox.Checked
        Next

        dateFixDataGridView.RefreshEdit()

        _totalCheckedCheckBoxes = If(HCheckBox.Checked, _totalCheckBoxes, 0)
        updateStatusText()
        _isHeaderCheckBoxClicked = False
    End Sub

    Private Sub RowCheckBoxClick(RCheckBox As DataGridViewCheckBoxCell)
        If RCheckBox IsNot Nothing Then
            'Modifiy Counter;            
            If CBool(RCheckBox.Value) AndAlso _totalCheckedCheckBoxes < _totalCheckBoxes Then
                _totalCheckedCheckBoxes += 1
            ElseIf _totalCheckedCheckBoxes > 0 Then
                _totalCheckedCheckBoxes -= 1
            End If

            'Change state of the header CheckBox.
            If _totalCheckedCheckBoxes < _totalCheckBoxes Then
                _headerCheckBox.Checked = False
            ElseIf _totalCheckedCheckBoxes = _totalCheckBoxes Then
                _headerCheckBox.Checked = True
            End If
            updateStatusText()
        End If
    End Sub

    Private Sub nudDateFix_ValueChanged(sender As Object, e As EventArgs) Handles nudDateFix.ValueChanged
        _timeUnit = nudDateFix.Value
        BindGridView()
    End Sub

    Private Sub cmbxDateFix_SelectedValueChanged(sender As Object, e As EventArgs) Handles cmbxDateFix.SelectedValueChanged
        _timePeriodDays = cmbxDateFix.SelectedItem("InDays")
        BindGridView()
    End Sub

    Private Sub btnDateFix_Click(sender As Object, e As EventArgs) Handles btnDateFix.Click
        If _totalCheckedCheckBoxes Then
            statuslblDateFix.Text = "Processing..."
            btnDateFix.Enabled = False
            Application.DoEvents()
            Dim selectedItems = From theRow In dateFixDataGridView.Rows Where theRow.Cells("chkBxSelect").Value
            For Each selectedItem As DataGridViewRow In selectedItems
                Dim m As Movie = Form1.oMovies.LoadMovie(selectedItem.Cells("NFOpath").Value)
                If Not IsNothing(m) Then
                    Dim createDate As Date = selectedItem.Cells("FileDate").Value
                    m._movieCache.createdate = createDate.ToString(Pref.datePattern)
                    m.ScrapedMovie.fileinfo.createdate = m._movieCache.createdate
                    m.UpdateMovieCache()
                    m.SaveNFO()
                End If
            Next

            'After changes made, reload from cache
            GetDataSource()
            statuslblDateFix.Text = "Done."
            btnDateFix.Enabled = True
        Else
            MessageBox.Show("Please make your selection for dates to sync", "No Titles Selected")
        End If
    End Sub

End Class
