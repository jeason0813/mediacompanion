
Public Class frmCopyProgress
    Dim donesize As Long = 0
    Dim percentage As Integer = 0
    Public Property ListOfFilesToMove As New List(Of String)
    Public Property totalfilesize As Long
    
    Private Declare Function GetDiskFreeSpaceEx _
    Lib "kernel32" _
    Alias "GetDiskFreeSpaceExA" _
    (ByVal lpDirectoryName As String, _
    ByRef lpFreeBytesAvailableToCaller As Long, _
    ByRef lpTotalNumberOfBytes As Long, _
    ByRef lpTotalNumberOfFreeBytes As Long) As Long
    
    Private Sub Exportmovies()
        With FolderBrowserDialog1
            .ShowNewFolderButton = True
            .Description = "Select destination for file copy"
        End With
        Dim drive As String = ""
        Dim savepath As String = ""
        If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            savepath = FolderBrowserDialog1.SelectedPath
            drive = Path.GetPathRoot(savepath)
            Me.Visible = True
            Me.Show()
            Me.Refresh()
            Dim PossibleFolders As Boolean = Pref.usefoldernames OrElse Pref.allfolders
            Dim drivespace As Long
            drivespace = Utilities.GetFreeSpace(drive)
            Application.DoEvents()
            Me.Refresh()
            Dim percentages As New List(Of Integer)

            If drivespace > totalfilesize Then
                ProgressBar2.Maximum = 100
                ProgressBar2.Minimum = 0
                ProgressBar2.Step = 1
                ProgressBar2.Value = 0
                ProgressBar1.Maximum = 100
                ProgressBar1.Minimum = 0
                ProgressBar1.Step = 1
                ProgressBar1.Value = 0
                ProgressBar1.Style = ProgressBarStyle.Blocks
                ProgressBar2.Style = ProgressBarStyle.Blocks
                For Each item In ListOfFilesToMove
                    Label3.Text = item
                    Try
                        Application.DoEvents()
                        Me.Refresh()

                        If PossibleFolders AndAlso item.EndsWith("\") Then
                            CopyDirectory(item, savepath & "\" & Utilities.GetLastFolderInPath(item))
                        Else
                            Copyfile(item, savepath)
                        End If
                        
                        If closing = True Then Exit For
                        ProgressBar1.Value = 0

                        Application.DoEvents()
                        Me.Refresh()
                    Catch ex As Exception
                    End Try
                Next
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub

    Public Sub CopyDirectory(ByVal sourcePath As String, ByVal destinationPath As String)
        Dim sourceDirectoryInfo As New System.IO.DirectoryInfo(sourcePath)

        ' If the destination folder don't exist then create it
        If Not IO.Directory.Exists(destinationPath) Then IO.Directory.CreateDirectory(destinationPath)

        Dim fileSystemInfo As IO.FileSystemInfo
        For Each fileSystemInfo In sourceDirectoryInfo.GetFileSystemInfos
            Dim destinationFileName As String = IO.Path.Combine(destinationPath, fileSystemInfo.Name)

            ' Now check whether its a file or a folder and take action accordingly
            If TypeOf fileSystemInfo Is IO.FileInfo Then
                If closing Then Exit Sub
                Copyfile(fileSystemInfo.FullName, destinationPath)
            Else
                ' Recursively call the method to copy all the neste folders
                CopyDirectory(fileSystemInfo.FullName, destinationFileName)
            End If
        Next
    End Sub

    Private Sub Copyfile(ByVal item As String, ByVal savepath As String)
        Try
            If donesize <> 0 Then ProgressBar2.Value = donesize * 100 / totalfilesize
            Application.DoEvents()
            Me.Refresh()
            Dim filename As String = Path.GetFileName(item)
            Dim dest As String = Path.Combine(savepath, filename)
            Dim mediafile As String = filename
            Dim fi As New FileInfo(mediafile)
            Dim sr As New IO.FileStream(item, IO.FileMode.Open) 'source file
            Dim sw As New IO.FileStream(dest, IO.FileMode.Create) 'target file, defaults overwrite
            Dim len As Long = sr.Length - 1
            donesize += len
            For i As Long = 0 To len
                Try
                    sw.WriteByte(sr.ReadByte)
                    If i Mod 1000000 = 0 Then 'only update UI every 1 Kb copied
                        ProgressBar1.Value = i * 100 / len
                        ProgressBar1.Refresh()
                        If i Mod 10000000 = 0 Then
                            Application.DoEvents()
                        End If
                    End If
                    If closing = True Then
                        sw.Close()
                        File.Delete(dest)
                        Exit For
                    End If
                Catch
                End Try
            Next

            If closing = False Then
                sw.Close()
            End If

            Application.DoEvents()
            Me.Refresh()
        Catch ex As Exception
        End Try
    End Sub

    Shadows closing As Boolean = False

    Private Sub frmCopyProgress_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Button1.PerformClick()
    End Sub

    Private Sub copyprogress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Visible = False
            Me.Refresh()
            Application.DoEvents()
            Call Exportmovies()
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim tempint As Integer = MessageBox.Show("The file export is still in progress" & vbCrLf & "Are you sure you wish to cancel?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If tempint = DialogResult.Yes Then
                closing = True
            End If
        Catch ex As Exception
            ExceptionHandler.LogError(ex)
        End Try
    End Sub

End Class