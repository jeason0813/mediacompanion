Imports System.ComponentModel
Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Threading

Public Class WebFileDownloader
    Public Event AmountDownloadedChanged(ByVal iNewProgress As Long)
    Public Event FileDownloadSizeObtained(ByVal iFileSize As Long)
    Public Event FileDownloadComplete()
    Public Event FileDownloadFailed(ByVal ex As Exception)

    Private mCurrentFile As String = String.Empty

    Public ReadOnly Property CurrentFile() As String
        Get
            Return mCurrentFile
        End Get
    End Property

    Public Function DownloadFile(ByVal URL As String, ByVal Location As String) As Boolean
        Try
            mCurrentFile = GetFileName(URL)
            Dim WC As New WebClient
            WC.DownloadFile(URL, Location)
            RaiseEvent FileDownloadComplete()
            Return True
        Catch ex As Exception
            RaiseEvent FileDownloadFailed(ex)
            Return False
        End Try
    End Function

    Private Function GetFileName(ByVal URL As String) As String
        Try
            Return URL.Substring(URL.LastIndexOf("/") + 1)
        Catch ex As Exception
            Return URL
        End Try
    End Function
    Public Function DownloadFileWithProgress(ByVal URL As String, ByVal Location As String, Optional bw As BackgroundWorker=Nothing) As Boolean

	    Monitor.Enter(Form1.countLock)
	    Form1.blnAbortFileDownload = False
	    Monitor.Exit(Form1.countLock)

        Dim FS As IO.FileStream = Nothing
        Try
            mCurrentFile = GetFileName(URL)
            Dim wRemote As WebRequest
            Dim bBuffer As Byte()
            ReDim bBuffer( 2*1024*1024 )
            Dim iBytesRead As Integer
            Dim iTotalBytesRead As Long
            Dim iRetries    as Integer = 0
            Dim iMaxRetries as Integer = 10
            Dim FileDownloadAborted As Boolean
            
            FS = New IO.FileStream(Location, IO.FileMode.Create, IO.FileAccess.Write)
            wRemote = WebRequest.Create(URL)
            wRemote.Proxy = Utilities.MyProxy
            wRemote.Timeout = 10000

            Dim myWebResponse As WebResponse = wRemote.GetResponse
            Dim fSize As long = myWebResponse.ContentLength
            RaiseEvent FileDownloadSizeObtained(fSize)

            Dim sChunks As IO.Stream = myWebResponse.GetResponseStream

            Do
                iBytesRead = sChunks.Read(bBuffer, 0, bBuffer.Length-10)
                FS.Write(bBuffer, 0, iBytesRead)
                iTotalBytesRead += iBytesRead
                If fSize < iTotalBytesRead Then
                    RaiseEvent AmountDownloadedChanged(fSize)
                Else
                    RaiseEvent AmountDownloadedChanged(iTotalBytesRead)
                End If


                'Nov11 - AnotherPhil - Add retry handling with resume from last good position 
                If iBytesRead = 0 then
                    sChunks.Close()

                    Dim wRequest As HttpWebRequest = WebRequest.Create( URL )
                    wRequest.Proxy = Utilities.MyProxy

                    wRequest.AddRange(iTotalBytesRead)

                    myWebResponse = wRequest.GetResponse
                    sChunks       = myWebResponse.GetResponseStream
                    iRetries      = iRetries + 1
                End If

                Monitor.Enter(Form1.countLock)
	            FileDownloadAborted = Form1.blnAbortFileDownload
		        Monitor.Exit(Form1.countLock)

            Loop While (iTotalBytesRead < fSize) and (iRetries <= iMaxRetries) and Not Cancelled(bw) And Not FileDownloadAborted
            sChunks.Close()
            FS.Close()

            If (iTotalBytesRead < fSize) or (iTotalBytesRead = 0) then
                File.Delete(Location)

                If Cancelled(bw) or FileDownloadAborted then Return True
                
                Dim ex As Exception = New Exception( "Download failed" )
                RaiseEvent FileDownloadFailed(ex)
                Return False
            End If

            RaiseEvent FileDownloadComplete()
            Return True
        Catch ex As Exception
            If Not (FS Is Nothing) Then
                FS.Close()
                FS = Nothing
            End If
            RaiseEvent FileDownloadFailed(ex)
            Return False
        End Try
    End Function
    Const KB As Integer = 1024
    Const MB As Integer = KB * KB
    Public Shared Function FormatFileSize(ByVal Size As Long) As String
        Try

            ' Return size of file in kilobytes.
            If Size < KB Then
                Return (Size.ToString("D") & " bytes")
            Else
                Select Case Size / KB
                    Case Is < 1000
                        Return (Size / KB).ToString("N") & "KB"
                    Case Is < 1000000
                        Return (Size / MB).ToString("N") & "MB"
                    Case Is < 10000000
                        Return (Size / MB / KB).ToString("N") & "GB"
                End Select
            End If
        Catch ex As Exception
            Return Size.ToString
        End Try
        Return "Error"
    End Function


    Public Function Cancelled(bw As BackgroundWorker) As Boolean

        Application.DoEvents
        If Not IsNothing(bw) AndAlso bw.CancellationPending Then
            Return True
        End If

        Return False
    End Function
End Class
