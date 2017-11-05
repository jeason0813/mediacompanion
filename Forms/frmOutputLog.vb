'Imports System.IO
Imports System.Text

Public Class frmoutputlog
    Public output As String = ""
    Public FullViewOverride As Boolean = False
    Private Sub frmoutputlog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Commented out by AnotherPhil as already done [after search for new movies] and if left causes endless processing...
        'Try
        '    ShowLog() 'call the subroutine to actually show the log when the form is created.
        'Catch ex As Exception
        '    ExceptionHandler.LogError(ex)
        'End Try
        If Pref.MultiMonitoEnabled Then
            Me.Bounds = Screen.AllScreens(Form1.CurrentScreen).Bounds
            Me.Width = 861
            Me.Height = 580
        End If
    End Sub

    Public Sub New(ByVal displaystring As String, Optional ByVal forceoverride As Boolean = False, Optional ByVal FullView As Boolean = False)

        InitializeComponent()
        Try
            If forceoverride = False Then
                Me.Close()
            End If
            output = displaystring
            FullViewOverride = fullview


        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub


    Private Sub btn_savelog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_savelog.Click
        Try 'this is the save button, it will save the displayed text (full or brief)
            Dim strFileName As String

            With SaveFileDialog1
                .DefaultExt = "txt"
                .Filter = "Text Documents (*.txt)|*.txt|All Files(*.*)|*.*"
                .FilterIndex = 1
                .OverwritePrompt = True
                .Title = "Save Scraper Log Dialogue"
            End With

            If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                Try
                    strFileName = SaveFileDialog1.FileName
                    Dim file As IO.StreamWriter
                    file = New IO.StreamWriter(strFileName, False)
                    file.WriteLine(Now())
                    file.WriteLine()
                    file.Write(TextBox1.Text)
                    file.WriteLine()
                    file.Close()
                    MsgBox(strFileName & " Saved")
                Catch ex As Exception
                    MsgBox("Error" & vbCrLf & vbCrLf & ex.Message.ToString)
                End Try
            End If


        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub TextBox1_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.GotFocus
        Try
            TextBox1.Select(TextBox1.Text.Length, 0) 'this removes the whole text being selected when the form is created
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub ComboBoxLogViewType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBoxLogViewType.SelectedIndexChanged
        ShowLog()
    End Sub
    Private Sub ShowLog()

        TextBox1.Text = ""

        Pref.logview = ComboBoxLogViewType.SelectedIndex

        Dim builder As New StringBuilder

        For Each line In output.Split(vbCrLf) 

            line = Strings.Replace(line, Chr(10), "")

            If IsNothing(line) Then Continue For

            If line="!!!" Then
                builder.AppendLine

            ElseIf line.Contains("!!! ") Then 
                builder.Append(Strings.Right(line, Strings.Len(line) - 4)).AppendLine

            ElseIf (Pref.logview=0 OrElse FullViewOverride) Then          '0 = Full log view -> Append details
                builder.Append(line).AppendLine
            End If
        Next

        TextBox1.Text = builder.ToString
    End Sub

    Private Sub frmoutputlog_Shown( sender As System.Object,  e As System.EventArgs) Handles MyBase.Shown
        ComboBoxLogViewType.SelectedIndex = Pref.logview 'set the combobox entry as per the preferences
    End Sub

    Private Sub frmoutputlog_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown 

        If               e.KeyCode = Keys.Escape Then Me.Close() 
    End Sub

End Class