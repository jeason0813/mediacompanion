Imports System
Imports System.Drawing
Imports System.IO.Compression
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.xml
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem

Public Class DownloadCache

    Public Shared Property CacheFolder As String

    Public Shared Function GetCacheFileName(ByVal URL As String) As String
        Dim Buffer As Byte() = Utilities.ComputeHashValueToByte(URL)

        Dim Extention As String = URL.Split("?")(0)
        Extention = Path.GetExtension(Extention)

        Dim Result As String = ""
        For Each Item As Byte In Buffer
            Result &= Conversion.Hex(Item)
        Next

        Return Result & Extention
    End Function

    Public Shared Function SaveImageToCacheAndPath(ByVal URL As String, SavePath As String, Optional ByVal ForceDownload As Boolean = False, _
                                           Optional ByVal resizeWidth As Integer = 0, Optional ByVal resizeHeight As Integer = 0, _
                                           Optional ByVal Overwrite As Boolean = True) As Boolean
        Dim CacheFileName As String = ""
        If Not SaveImageToCache(URL, SavePath, ForceDownload, CacheFileName) Then Return False

        Dim CachePath = Path.Combine(CacheFolder, CacheFileName)

        If IfNotValidImage_Delete(CachePath) Then

            'Resize cache image only if need to
            If Not (resizeWidth = 0 And resizeHeight = 0) Then CopyAndDownSizeImage(CachePath, CachePath, resizeWidth, resizeHeight)

            Utilities.EnsureFolderExists(SavePath)
            File.Copy(CachePath, SavePath, Overwrite)
        Else
            Return False
        End If
        Return True
    End Function
    
    Public Shared Function SaveImageToCacheAndPaths(ByVal URL As String, Paths As List(Of String), Optional ByVal ForceDownload As Boolean = False, _
                                           Optional ByVal resizeWidth As Integer = 0, Optional ByVal resizeHeight As Integer = 0, _
                                           Optional ByVal Overwrite As Boolean = True) As Boolean
        Dim verifiedPaths As New List(Of String)
        For each path in Paths
            If Not File.exists(path) OrElse Overwrite Then verifiedPaths.add(path)
        Next
        If verifiedPaths.Count = 0 Then Return True
        Dim CacheFileName = ""
        If Not SaveImageToCache(URL, verifiedPaths(0), ForceDownload, CacheFileName) Then Return False

        Dim CachePath = Path.Combine(CacheFolder, CacheFileName)

        Try
            If IfNotValidImage_Delete(CachePath) Then

                'Resize cache image only if need to
                If Not (resizeWidth = 0 And resizeHeight = 0) Then CopyAndDownSizeImage(CachePath, CachePath, resizeWidth, resizeHeight)

                For Each path In verifiedPaths
                    Utilities.EnsureFolderExists(path)
                    If Overwrite OrElse Not File.Exists(path) Then File.Copy(CachePath, path, Overwrite)
                Next
            Else
                Return False
            End If
        Catch
            Return False
        End Try

        Return True
    End Function

    Public Shared Function IfNotValidImage_Delete(filename As String) As Boolean
        Dim ok As Boolean = True
        Try
            Dim ms As IO.MemoryStream = New IO.MemoryStream()
            Using r As IO.Filestream = File.Open(filename, IO.FileMode.Open)
                r.CopyTo(ms)
            End Using
            Dim testImage = new Drawing.Bitmap(ms)
            testImage.Dispose()
            ms.Dispose()
        Catch ex As Exception
            Try
                File.Delete(filename)
                ok = False
            Catch 
                ok = False
            End Try
            Throw 
        End Try
        Return ok
    End Function

    Public Shared Sub CopyAndDownSizeImage(ByVal src As String, ByVal dest As String, Optional ByVal resizeWidth As Integer = 0, Optional ByVal resizeHeight As Integer = 0)
        Try
            Dim img = Utilities.GetImage(src)
            Dim resized = False

            'Down-size only
            If (resizeWidth <> 0 And img.Width > resizeWidth) Or img.Height > resizeHeight And Not (resizeWidth = 0 And resizeHeight = 0) Then

                'Calc scaled height - width is passed in as zero for people pictures and posters.
                If resizeWidth = 0 Then resizeWidth = img.Width * resizeHeight / img.Height

                img = Utilities.ResizeImage(img, resizeWidth, resizeHeight)
                resized = True
            End If

            If resized Or src <> dest Then
                Utilities.SaveImage(img, dest)
            End If
        Catch
        End Try
    End Sub

    Public Shared Function DownloadFileAndCache(ByVal URL As String, Optional ByVal SavePath As String = "", _
                                          Optional ByVal ForceDownload As Boolean = False, _
                                          Optional ByVal resizeFanart As Integer = 0, _
                                          Optional ByVal retcachename As Boolean = False, _
                                          Optional ByRef strValue As String = "") As Boolean

        Dim CacheFileName As String = ""
        Dim returnCode As Boolean = SaveImageToCache(URL, SavePath, ForceDownload, CacheFileName)
        If returnCode Then
            Dim CachePath As String = Path.Combine(CacheFolder, CacheFileName)
            If String.IsNullOrEmpty(SavePath) Then
                strValue = File.ReadAllText(CachePath)
            Else
                If Not retcachename Then
                    Utilities.copyImage(CachePath, SavePath, resizeFanart)
                Else
                    strValue = CachePath 
                End If
            End If
        End If
        DownloadFileAndCache = returnCode
        Return returncode
    End Function
    
    Public Shared Function SaveImageToCache(ByVal URL As String, ByVal SavePath As String, ByVal ForceDownload As Boolean, ByRef CacheFileName As String) As Boolean
        Dim returnCode As Boolean = True
        Dim CachePath As String = ""
        Try
            Utilities.EnsureFolderExists(CacheFolder)

            If URL = "" Then Return False
            CacheFileName = GetCacheFileName(URL)
            CachePath = Path.Combine(CacheFolder, CacheFileName)

            If Not File.Exists(CachePath) OrElse ForceDownload Then
                'Check to see if URL is actually a local file
                If Not URL.Contains("://") AndAlso File.Exists(URL) Then
                    If CachePath <> URL Then
                        If Utilities.SafeDeleteFile(CachePath) Then File.Copy(URL, CachePath)
                    End If
                    Return True
                End If

                If ForceDownload AndAlso File.Exists(CachePath) Then File.Delete(CachePath)

                Try
                    Dim webReq As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
                    webReq.Proxy = Utilities.MyProxy
                    webReq.AllowAutoRedirect = True
                    If Not URL.Contains("assets.fanart.tv") Then
                        webReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                        webReq.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
                    End If

                    Using webResp As HttpWebResponse = webReq.GetResponse()
                        Using responseStreamData As IO.Stream = webResp.GetResponseStream()
                            'got a response - should probably put a Try...Catch in here for filesystem stuff, but I'll wing it for now.
                            If String.IsNullOrEmpty(SavePath) Then
                                IO.File.WriteAllText(CachePath, New IO.StreamReader(responseStreamData, Encoding.UTF8).ReadToEnd)
                            Else
                                Utilities.SafeDeleteFile(CachePath)

                                Using fileStream As IO.FileStream = IO.File.Open(CachePath, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
                                    Dim buffer(webResp.ContentLength) As Byte
                                    Dim bytesRead = responseStreamData.Read(buffer, 0, buffer.Length)
                                    While bytesRead > 0
                                        fileStream.Write(buffer, 0, bytesRead)
                                        bytesRead = responseStreamData.Read(buffer, 0, buffer.Length)
                                    End While
                                End Using
                            End If
                        End Using
                    End Using
                    If (New IO.FileInfo(CachePath)).Length = 0 Then
                        File.Delete(CachePath)
                        returnCode = False
                    End If
                Catch ex As WebException
                    If ex.Message.Contains("could not be resolved") Then Return False : Exit Try
                    Using errorResp As HttpWebResponse = DirectCast(ex.Response, HttpWebResponse)
                        Using errorRespStream As IO.Stream = errorResp.GetResponseStream()
                            Dim errorText As String = New IO.StreamReader(errorRespStream).ReadToEnd()
                            returnCode = False
                        End Using
                    End Using
                    returnCode = False
                End Try
            End If
        Catch ex As Exception
            'MsgBox(ex.Message.ToString & vbCrLf & "URL string =" & URL & vbCrLf & "cachefolder = " & CacheFolder & vbCrLf & "path = " & Path)
            returnCode = False
        End Try
        Return returnCode
    End Function

    Public Shared Function Savexmltopath(ByVal URL As String, ByVal Path As String, ByVal filename As String, ByVal ForceDownload As Boolean) As Boolean
        Dim returnCode As Boolean = True
        Dim Fullpath As String = Path & filename
        Try
            Utilities.EnsureFolderExists(Path)
            If URL = "" Then Return False
            If Not File.Exists(Fullpath) OrElse ForceDownload Then
                If ForceDownload AndAlso File.Exists(Fullpath) Then
                    File.Delete(Fullpath)
                End If
                Try
                    Dim webReq As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
                    webReq.Proxy = Utilities.MyProxy
                    webReq.AllowAutoRedirect = True
                    webReq.AutomaticDecompression = DecompressionMethods.GZip Or DecompressionMethods.Deflate
                    Using webResp As HttpWebResponse = webReq.GetResponse()
                        Using responseStreamData As IO.Stream = webResp.GetResponseStream()
                            File.WriteAllText(Fullpath, New IO.StreamReader(responseStreamData, Encoding.UTF8).ReadToEnd)
                        End Using
                    End Using
                    Dim testxml As New XmlDocument
                    testxml.Load(Fullpath)
                    
                Catch ex As WebException
                    If ex.Message.Contains("could not be resolved") Then Return False : Exit Try
                    Using errorResp As HttpWebResponse = DirectCast(ex.Response, HttpWebResponse)
                        Using errorRespStream As IO.Stream = errorResp.GetResponseStream()
                            Dim errorText As String = New IO.StreamReader(errorRespStream).ReadToEnd()
                            returnCode = False
                        End Using
                    End Using
                    returnCode = False
                Catch ex As XmlException
                    If File.Exists(Fullpath) Then File.Delete(Fullpath)
                    returnCode = False
                End Try
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString & vbCrLf & "URL string =" & URL & vbCrLf & "Filename = " & filename & vbCrLf & "path = " & Path)
            returnCode = False
        End Try
        Return returnCode
    End Function
    
End Class






