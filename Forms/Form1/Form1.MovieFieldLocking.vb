Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions
Imports System.Text
Imports Media_Companion.WorkingWithNfoFiles
Imports System.Xml
Imports Media_Companion
Imports Media_Companion.Pref
Imports System.Linq

Partial Public Class Form1



#Region "Lock Specific"

    Private Sub tsmiLockSetClick(sender As ToolStripMenuItem, e As EventArgs) Handles tsmiLockSet.Click
        mov_LockSpecific(sender.Tag, True)
    End Sub   

#End Region




#Region "Unlock Specific"

    Private Sub tsmiUnLockSetClick(sender As ToolStripMenuItem, e As EventArgs) Handles tsmiUnLockSet.Click
        mov_LockSpecific(sender.Tag, False)
    End Sub   

#End Region



    Private Sub mov_LockSpecific(ByVal field As String, lock As Boolean)

        _lockList.Field = field
        _lockList.Lock  = lock
        _lockList.FullPathAndFilenames.Clear

        For Each row As DataGridViewRow In DataGridViewMovies.SelectedRows
            Dim fullpath As String = row.Cells("fullpathandfilename").Value.ToString
            If Not File.Exists(fullpath) Then Continue For
            _lockList.FullPathAndFilenames.Add(fullpath)
        Next

        RunBackgroundMovieScrape("LockSpecific")
    End Sub

      
    Public Sub LockSpecific
        oMovies.SetFieldLockSpecific(_lockList)
    End Sub


End Class
