Imports System.Threading
Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.IO.Compression
Imports System.Text
Imports System.Reflection
Imports System.Drawing


Public Class Utilities
    Const SetDefaults = True
    Public Shared VideoExtensions As String() = {".avi", ".xvid", ".divx", ".img", ".mpg", ".mpeg", ".mov",
                                                 ".rm", ".3gp", ".m4v", ".wmv", ".asf", ".mp4", ".mkv", ".nrg", ".iso",
                                                 ".rmvb", ".ogm", ".bin", ".ts", ".vob", ".m2ts", ".rar", ".flv",
                                                 ".dvr-ms", "VIDEO_TS.IFO"}

    Private Declare Function GetDiskFreeSpaceEx _
Lib "kernel32" _
Alias "GetDiskFreeSpaceExA" _
(ByVal lpDirectoryName As String, _
ByRef lpFreeBytesAvailableToCaller As Long, _
ByRef lpTotalNumberOfBytes As Long, _
ByRef lpTotalNumberOfFreeBytes As Long) As Long

    Public Shared Function GetFreeSpace(ByVal Drive As String) As Long
        'returns free space in MB, formatted to two decimal places
        'e.g., msgbox("Free Space on C: "& GetFreeSpace("C:\") & "MB")

        Dim lBytesTotal, lFreeBytes, lFreeBytesAvailable As Long

        Dim iAns As Long

        iAns = GetDiskFreeSpaceEx(Drive, lFreeBytesAvailable, _
             lBytesTotal, lFreeBytes)
        If iAns > 0 Then

            Return lFreeBytes
        Else
            Throw New Exception("Invalid or unreadable drive")
        End If


    End Function

    Public Shared Function CreateScreenShot(ByVal FilePath As String, Optional ByVal Overwrite As Boolean = False) As String
        Dim thumbpathandfilename As String = FilePath.Replace(IO.Path.GetExtension(FilePath), ".tbn")

        Dim ThumbExists As Boolean = Not IO.File.Exists(thumbpathandfilename)

        If ThumbExists AndAlso Overwrite Then
            Try
                IO.File.Delete(thumbpathandfilename)
                ThumbExists = False
            Catch
                Return "nodelete"
            End Try
        Else
            Return "nooverwrite"
        End If

        Dim nfofilename As String = IO.Path.GetFileName(FilePath)
        Dim FullPath As String = IO.Path.GetFileName(FilePath)
        Dim Extention As String = IO.Path.GetExtension(FilePath)
        Dim tempfilename As String = nfofilename
        For j = 0 To VideoExtensions.Length
            tempfilename = nfofilename.Replace(Extention, VideoExtensions(j))

            Dim tempstring2 As String = FilePath.Replace(FullPath, tempfilename)
            If IO.File.Exists(tempstring2) Then
                Try
                    Dim seconds As Integer = 100
                    Dim myProcess As Process = New Process
                    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    myProcess.StartInfo.CreateNoWindow = False
                    myProcess.StartInfo.FileName = Preferences.applicationPath & "\ffmpeg.exe"
                    Dim proc_arguments As String = "-y -i """ & tempstring2 & """ -f mjpeg -ss " & seconds.ToString & " -vframes 1 -an " & """" & thumbpathandfilename & """"
                    myProcess.StartInfo.Arguments = proc_arguments
                    myProcess.Start()
                    myProcess.WaitForExit()

                    Return "done"
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        Next


        Return "failure"
    End Function

    Public Shared Function DownloadTextFiles(ByVal StartURL As String) As String
        'Dim wr As HttpWebRequest = CType(WebRequest.Create(StartURL), HttpWebRequest)
        'Dim ws As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)
        'Dim str As New StreamReader(ws.GetResponseStream())
        'Dim co As Integer = ws.Headers.Count                           'this parts was to read & display all the headers to see which one showed gzip - header 2 was the answer
        'Dim txt As String = ""
        'For myco = 0 To co - 1
        '    txt = txt & ws.Headers.Get(myco) & Environment.NewLine()
        'Next
        'Dim gzipresult As Boolean = (ws.Headers.Get(2) = "gzip")       'this tests if the header(2) contains gzip, if so the xml is gzipped!
        'MsgBox(txt, MsgBoxStyle.OkOnly, gzipresult)

        'Dim result As String

        'result = str.ReadToEnd


        'Return result
        '                                                               'Converting the above was too hard so I started fresh.....
        Dim Http As HttpWebRequest = WebRequest.Create(StartURL)
        Using WebResponse As HttpWebResponse = Http.GetResponse()

            Dim responseStream As Stream = WebResponse.GetResponseStream()
            If (WebResponse.ContentEncoding.ToLower().Contains("gzip")) Then
                responseStream = New GZipStream(responseStream, CompressionMode.Decompress)
                Preferences.tvScraperLog &= "TVDB Returned GZIP File" & vbCrLf
            ElseIf (WebResponse.ContentEncoding.ToLower().Contains("deflate")) Then
                responseStream = New DeflateStream(responseStream, CompressionMode.Decompress)
                Preferences.tvScraperLog &= "TVDB Returned DEFLATE Encoded File" & vbCrLf
            Else
                Preferences.tvScraperLog &= "TVDB Returned TEXT File" & vbCrLf
            End If
            Dim reader As StreamReader = New StreamReader(responseStream, Encoding.Default)

            Dim html As String = reader.ReadToEnd()

            responseStream.Close()

            Return html
        End Using
    End Function

    Public Shared Function GetCRC32(ByVal sFileName As String) As String
        Dim oCRC As Vbaccelerator.Components.Algorithms.CRC32 = New Vbaccelerator.Components.Algorithms.CRC32()
        Dim oEnc As System.Text.UTF7Encoding = New System.Text.UTF7Encoding()
        Return (oCRC.GetCrc32(New System.IO.MemoryStream(oEnc.GetBytes(sFileName))))
    End Function

    Public Shared Function GetLastFolder(ByVal FullPath As String) As String
        If FullPath.Contains("/") AndAlso Not FullPath.Contains("\") Then
            FullPath = FullPath.Replace("/", "\")
        End If

        FullPath = FullPath.Replace(IO.Path.GetFileName(FullPath), "")

        Dim foldername As String = ""
        Dim paths() As String
        paths = FullPath.Split("\")
        For g = UBound(paths) To 0 Step -1
            If paths(g).ToLower.IndexOf("video_ts") = -1 And paths(g) <> "" Then
                foldername = paths(g)
                Return foldername
            End If
        Next

        Return ""
    End Function


    Public Shared Function GetPosterPath(ByVal FullPath As String) As String
        Dim posterpath As String = ""
        posterpath = FullPath.Substring(0, FullPath.Length - 4)
        posterpath = posterpath & ".tbn"
        'If Not IO.File.Exists(posterpath) Then
        Dim stackname As String = GetStackName(IO.Path.GetFileName(FullPath), FullPath.Replace(IO.Path.GetFileName(FullPath), ""))
        stackname = FullPath.Replace(IO.Path.GetFileName(FullPath), stackname)
        stackname = stackname & ".tbn"
        If stackname <> "na" And IO.File.Exists(stackname) Then
            posterpath = stackname
        Else
            posterpath = posterpath.Replace(IO.Path.GetFileName(posterpath), "")
            posterpath = posterpath & "movie.tbn"
            If Not IO.File.Exists(posterpath) Then
                posterpath = ""
            End If
        End If
        '    Else
        'posterpath = fullpath.Replace("movie.nfo", "movie.tbn")
        'End If
        If posterpath = "" Then
            If FullPath.IndexOf("movie.nfo") <> -1 Then
                posterpath = FullPath.Replace("movie.nfo", "movie.tbn")
            End If
        End If
        If posterpath = "" Then
            If Preferences.posternotstacked = True Then
                posterpath = FullPath.Substring(0, FullPath.Length - 4) & ".tbn"
            Else
                posterpath = GetStackName(IO.Path.GetFileName(FullPath), posterpath.Replace(IO.Path.GetFileName(FullPath), "")) & ".tbn"
                If posterpath = "na.tbn" Then
                    posterpath = FullPath.Substring(0, FullPath.Length - 4) & ".tbn"
                Else
                    posterpath = FullPath.Replace(IO.Path.GetFileName(FullPath), posterpath)
                End If
            End If
            If Preferences.basicsavemode = True Then
                posterpath = posterpath.Replace(IO.Path.GetFileName(FullPath), "movie.tbn")
            End If
        End If
        If posterpath = "na" Then
            If IO.File.Exists(FullPath.Replace(IO.Path.GetFileName(FullPath), "folder.jpg")) Then
                posterpath = FullPath.Replace(IO.Path.GetFileName(FullPath), "folder.jpg")
            End If
        End If
        Return posterpath

    End Function

    Public Shared Function GetFanartPath(ByVal FullPath As String) As String
        Dim posterpath As String = ""
        posterpath = FullPath.Substring(0, FullPath.Length - 4)
        posterpath = posterpath & "-fanart.jpg"
        If Not IO.File.Exists(posterpath) Then
            Dim stackname As String = GetStackName(IO.Path.GetFileName(FullPath), FullPath.Replace(IO.Path.GetFileName(FullPath), ""))

            stackname = FullPath.Replace(IO.Path.GetFileName(FullPath), stackname)
            stackname = stackname & "-fanart.jpg"
            If stackname <> "na" And IO.File.Exists(stackname) Then
                posterpath = stackname
            Else
                posterpath = posterpath.Replace(IO.Path.GetFileName(posterpath), "")
                posterpath = posterpath & "fanart.jpg"
                If Not IO.File.Exists(posterpath) Then
                    posterpath = ""
                End If
            End If
            'Else
            '    posterpath = fullpath.Replace("movie.nfo", "movie.tbn")
        End If
        If posterpath = "" Then
            If FullPath.IndexOf("movie.nfo") <> -1 Then
                posterpath = FullPath.Replace("movie.nfo", "fanart.jpg")
            End If
        End If
        If posterpath = "" Then
            If Preferences.fanartnotstacked = True Then
                posterpath = FullPath.Substring(0, FullPath.Length - 4) & "-fanart.jpg"
            Else
                posterpath = GetStackName(IO.Path.GetFileName(FullPath), FullPath) & "-fanart.jpg"
                If posterpath = "na-fanart.jpg" Then
                    posterpath = FullPath.Substring(0, FullPath.Length - 4) & "-fanart.jpg"
                Else
                    posterpath = FullPath.Replace(IO.Path.GetFileName(FullPath), posterpath)
                End If
            End If
            If Preferences.basicsavemode = True Then
                posterpath = posterpath.Replace(IO.Path.GetFileName(posterpath), "fanart.jpg")
            End If
        End If

        Return posterpath
    End Function

    Public Shared Function GetStackName(ByVal filenames As String, ByVal filepath As String) As String
        Dim tempboolean As Boolean = False
        Dim truerar As Boolean = False
        Dim filename As String = filenames
        If IO.Path.GetExtension(filename).ToLower = ".rar" Then
            truerar = True
        End If
        filenames = filenames.Replace(System.IO.Path.GetExtension(filenames), "")
        filename = filename.ToLower
        Dim stackname As String = ""
        Dim workingstring As String = "na"


        If filename.IndexOf("cd1") <> -1 Then
            tempboolean = True
            workingstring = "cd1"
        End If

        If filename.IndexOf("cd.1") <> -1 Then
            tempboolean = True
            workingstring = "cd.1"
        End If


        If filename.IndexOf("cd_1") <> -1 Then
            tempboolean = True
            workingstring = "cd_1"
        End If


        If filename.IndexOf("cd 1") <> -1 Then
            tempboolean = True
            workingstring = "cd 1"
        End If


        If filename.IndexOf("cd-1") <> -1 Then
            tempboolean = True
            workingstring = "cd-1"
        End If


        If filename.IndexOf("dvd1") <> -1 Then
            tempboolean = True
            workingstring = "dvd1"
        End If


        If filename.IndexOf("dvd.1") <> -1 Then
            tempboolean = True
            workingstring = "dvd.1"
        End If


        If filename.IndexOf("dvd_1") <> -1 Then
            tempboolean = True
            workingstring = "dvd_1"
        End If


        If filename.IndexOf("dvd 1") <> -1 Then
            tempboolean = True
            workingstring = "dvd 1"
        End If


        If filename.IndexOf("dvd-1") <> -1 Then
            tempboolean = True
            workingstring = "dvd-1"
        End If


        If filename.IndexOf("part1") <> -1 Then
            tempboolean = True
            workingstring = "part1"
        End If


        If filename.IndexOf("part.1") <> -1 Then
            tempboolean = True
            workingstring = "part.1"
        End If

        If filename.IndexOf("part_1") <> -1 Then
            tempboolean = True
            workingstring = "part_1"
        End If

        If filename.IndexOf("part-1") <> -1 Then
            tempboolean = True
            workingstring = "part-1"
        End If

        If filename.IndexOf("part 1") <> -1 Then
            tempboolean = True
            workingstring = "part 1"
        End If

        If filename.IndexOf("disk1") <> -1 Then
            tempboolean = True
            workingstring = "disk1"
        End If

        If filename.IndexOf("disk.1") <> -1 Then
            tempboolean = True
            workingstring = "disk.1"
        End If

        If filename.IndexOf("disk_1") <> -1 Then
            tempboolean = True
            workingstring = "disk_1"
        End If

        If filename.IndexOf("disk 1") <> -1 Then
            tempboolean = True
            workingstring = "disk 1"
        End If

        If filename.IndexOf("disk-1") <> -1 Then
            tempboolean = True
            workingstring = "disk-1"
        End If

        If filename.IndexOf("pt 1") <> -1 Then
            tempboolean = True
            workingstring = "pt 1"
        End If

        If filename.IndexOf("pt-1") <> -1 Then
            tempboolean = True
            workingstring = "pt-1"
        End If

        If filename.IndexOf("pt1") <> -1 Then
            tempboolean = True
            workingstring = "pt1"
        End If

        If filename.IndexOf("pt_1") <> -1 Then
            tempboolean = True
            workingstring = "pt_1"
        End If

        If filename.IndexOf("pt.1") <> -1 Then
            tempboolean = True
            workingstring = "pt.1"
        End If


        Dim extension As String = System.IO.Path.GetExtension(filename)
        Dim filenameex As String

        filenameex = filename.Replace(System.IO.Path.GetExtension(filename), "")

        If filenameex.Substring(filenameex.Length - 1).ToLower = "a" Then
            Dim exists As Boolean = False
            Dim tempname As String
            For f = 0 To VideoExtensions.Length - 1
                tempname = filepath & filename.Substring(0, filename.Length - (1 + extension.Length)) & "b" & VideoExtensions(f)
                exists = System.IO.File.Exists(tempname)
                If exists = True Then
                    workingstring = "a"
                    tempboolean = True
                    Exit For
                End If
            Next
        End If

        If tempboolean = True Then
            Dim tbool As Boolean = False
            If workingstring <> "na" Then

                filename = filename.Replace(System.IO.Path.GetExtension(filename), "")

                Dim a() As String = {".", " ", "-", "_"}
                Dim multipartidentify As String
                For f = 0 To 3
                    multipartidentify = a(f) & workingstring
                    'filename = filename.Replace(System.IO.Path.GetExtension(filename), "")
                    If filename.IndexOf(multipartidentify) <> -1 Then
                        If multipartidentify.IndexOf(".") <> -1 Then
                            multipartidentify = multipartidentify.Replace(".", "\.")
                        End If
                        'filename = filename.Replace(multipartidentify, "")
                        filename = Regex.Replace(filenames, multipartidentify, "", RegexOptions.IgnoreCase)
                        tbool = True
                        Exit For
                    End If
                Next
            End If

            If workingstring = "a" And tbool = False Then
                Dim temp As String = filename
                Dim temp2 As String
                If temp.Substring(temp.Length - 1, 1) = "a" Then
                    temp = temp.Substring(0, temp.Length - 1)
                    For f = 0 To VideoExtensions.Length
                        temp2 = filepath & temp & "b" & VideoExtensions(f)
                        If System.IO.File.Exists(temp2) = True Then
                            filename = filenames.Substring(0, filename.Length - 1)
                            Exit For
                        End If
                    Next
                End If
            End If
        End If

        If truerar = True Then
            If IO.Path.GetExtension(filename).ToLower = ".rar" Then
                filename = filename.Replace(IO.Path.GetExtension(filename), "")
            End If
            If filename.ToLower.IndexOf(".part1") <> -1 Then
                filename = filename.Replace(".part1", "")
                tempboolean = True
            End If
            If filename.ToLower.IndexOf(".part01") <> -1 Then
                filename = filename.Replace(".part01", "")
                tempboolean = True
            End If
            If filename.ToLower.IndexOf(".part001") <> -1 Then
                filename = filename.Replace(".part001", "")
                tempboolean = True
            End If
            If filename.ToLower.IndexOf(".part0001") <> -1 Then
                filename = filename.Replace(".part0001", "")
                tempboolean = True
            End If
        End If

        If tempboolean = False Then filename = "na"
        Dim prefix(3)
        prefix(0) = " "
        prefix(1) = "_"
        prefix(2) = "-"
        prefix(3) = "."
        filename = RTrim(filename)
        If filename.IndexOf("_") = filename.Length - 1 Then filename = filename.Substring(0, filename.Length - 1)
        If filename.IndexOf("-") = filename.Length - 1 Then filename = filename.Substring(0, filename.Length - 1)
        If filename.IndexOf(".") = filename.Length - 1 Then filename = filename.Substring(0, filename.Length - 1)
        filename = RTrim(filename)

        Return filename

    End Function

    Public Shared Function GetFileName(ByVal path As String)
        Dim tempstring As String
        Dim tempfilename As String = path
        Dim actualpathandfilename As String = ""


        If IO.File.Exists(tempfilename.Replace(IO.Path.GetFileName(tempfilename), "VIDEO_TS.IFO")) Then
            actualpathandfilename = tempfilename.Replace(IO.Path.GetFileName(tempfilename), "VIDEO_TS.IFO")
        End If

        If actualpathandfilename = "" Then
            For f = 0 To VideoExtensions.Length
                tempfilename = tempfilename.Replace(IO.Path.GetExtension(tempfilename), VideoExtensions(f))
                If IO.File.Exists(tempfilename) Then
                    actualpathandfilename = tempfilename
                    Exit For
                End If
            Next
        End If

        If actualpathandfilename = "" Then
            If tempfilename.IndexOf("movie.nfo") <> -1 Then
                Dim possiblemovies(1000) As String
                Dim possiblemoviescount As Integer = 0
                For f = 1 To 23
                    Dim dirpath As String = tempfilename.Replace(IO.Path.GetFileName(tempfilename), "")
                    Dim dir_info As New System.IO.DirectoryInfo(dirpath)
                    Dim pattern As String = "*" & VideoExtensions(f)
                    Dim fs_infos() As System.IO.FileInfo = dir_info.GetFiles(pattern)
                    For Each fs_info As System.IO.FileInfo In fs_infos
                        'Application.DoEvents()
                        If IO.File.Exists(fs_info.FullName) Then
                            tempstring = fs_info.FullName.ToLower
                            If tempstring.IndexOf("-trailer") = -1 And tempstring.IndexOf("-sample") = -1 And tempstring.IndexOf(".trailer") = -1 And tempstring.IndexOf(".sample") = -1 Then
                                possiblemoviescount += 1
                                possiblemovies(possiblemoviescount) = fs_info.FullName
                            End If
                        End If
                    Next
                Next
                If possiblemoviescount = 1 Then
                    actualpathandfilename = possiblemovies(possiblemoviescount)
                ElseIf possiblemoviescount > 1 Then
                    Dim multistrings(6) As String
                    multistrings(1) = "cd"
                    multistrings(2) = "dvd"
                    multistrings(3) = "part"
                    multistrings(4) = "pt"
                    multistrings(5) = "disk"
                    multistrings(6) = "disc"
                    Dim types(5) As String
                    types(1) = ""
                    types(2) = "-"
                    types(3) = "_"
                    types(4) = " "
                    types(5) = "."
                    Dim workingstring As String
                    For f = 1 To 6
                        For g = 1 To 5
                            For h = 1 To possiblemoviescount
                                workingstring = multistrings(f) & types(h) & "1"
                                Dim workingtitle As String = possiblemovies(h).ToLower
                                If workingtitle.IndexOf(workingstring) <> -1 Then
                                    actualpathandfilename = possiblemovies(h)
                                End If
                            Next
                        Next
                    Next
                End If
            End If
        End If

        If actualpathandfilename = "" Then
            actualpathandfilename = "none"
        End If


        Return actualpathandfilename

        Return "Error"
    End Function

    Public Shared Function ValidMovieDir(ByVal PathToCheck As String) As Boolean
        Dim passed As Boolean = True
        Dim s As String = PathToCheck.ToLower
        Try
            If Strings.Right(s, 7) = "trailer" Then
                passed = False
            ElseIf Strings.Right(s, 8) = "(noscan)" Then
                passed = False
            ElseIf Strings.Right(s, 6) = "sample" Then
                passed = False
            ElseIf Strings.Right(s, 8) = "recycler" Then
                passed = False
            ElseIf s.Contains("$recycle.bin") Then
                passed = False
            ElseIf Strings.Right(s, 10) = "lost+found" Then
                passed = False
            ElseIf s.Contains("system volume information") Then
                passed = False
            ElseIf s.Contains("msocache") Then
                passed = False
            End If
        Catch ex As Exception
            passed = False
        End Try
        Return passed
    End Function

    Public Shared Function EnumerateFolders(ByVal RootPath As String, ByVal MaxLevels As Long) As List(Of String)
        Return EnumerateFolders(RootPath, MaxLevels, 0)
    End Function

    Private Shared Function EnumerateFolders(ByVal RootPath As String, ByVal MaxLevels As Long, ByVal Level As Long) As List(Of String)
        Dim TempReturn As New List(Of String)
        If String.IsNullOrEmpty(RootPath) Then Return Nothing
        Dim ChildList
        Try
            ChildList = Directory.GetDirectories(RootPath)
        Catch ex As UnauthorizedAccessException
            Return TempReturn
        End Try
        If Level > 0 Then
            TempReturn.Add(RootPath)
        End If

        For Each Item In ChildList
            If Level <= MaxLevels Then
                If ValidMovieDir(Item) Then
                    TempReturn.AddRange(EnumerateFolders(Item, MaxLevels, Level + 1))
                End If
            End If
        Next

        Return TempReturn
    End Function

    Public Shared Function GetYearByFilename(ByVal filename As String, Optional ByVal withextension As Boolean = True)
        Dim cleanname As String = filename
        Try
            If withextension = True Then
                Try
                    cleanname = filename.Replace(IO.Path.GetExtension(cleanname), "")
                Catch
                End Try
            End If
            Dim movieyear As String
            Dim S As String = cleanname
            Dim M As Match
            M = Regex.Match(S, "(\([\d]{4}\))")
            If M.Success = True Then
                movieyear = M.Value
            Else
                movieyear = Nothing
            End If
            If movieyear = Nothing Then
                M = Regex.Match(S, "(\[[\d]{4}\])")
                If M.Success = True Then
                    movieyear = M.Value
                Else
                    movieyear = Nothing
                End If
            End If
            Try
                movieyear = movieyear.Trim
                If movieyear.Length = 6 Then
                    movieyear = movieyear.Remove(0, 1)
                    movieyear = movieyear.Remove(4, 1)
                End If
            Catch
            End Try
            Return movieyear
        Catch
        End Try
        Return "Error"
    End Function

    Public Shared Function CleanFileName(ByVal filename As String, Optional ByVal withextension As Boolean = True)
        Dim cleanname As String = filename
        Try
            If withextension = True Then
                Try
                    cleanname = filename.Replace(IO.Path.GetExtension(cleanname), "")
                Catch
                End Try
            End If
            Dim movieyear As String
            Dim S As String = cleanname
            Dim M As Match
            M = Regex.Match(S, "(\([\d]{4}\))")
            If M.Success = True Then
                movieyear = M.Value
            Else
                movieyear = Nothing
            End If
            If movieyear = Nothing Then
                M = Regex.Match(S, "(\[[\d]{4}\])")
                If M.Success = True Then
                    movieyear = M.Value
                Else
                    movieyear = Nothing
                End If
            End If



            filename = cleanname
            Dim removal(75) As String
            removal(1) = "cd1"
            removal(2) = "cd.1"
            removal(3) = "cd_1"
            removal(4) = "cd 1"
            removal(5) = "cd-1"
            removal(6) = "dvd1"
            removal(7) = "dvd.1"
            removal(8) = "dvd_1"
            removal(9) = "dvd 1"
            removal(10) = "dvd-1"
            removal(11) = "part1"
            removal(12) = "part.1"
            removal(13) = "part_1"
            removal(14) = "part 1"
            removal(15) = "part-1"
            removal(16) = "disk1"
            removal(17) = "disk.1"
            removal(18) = "disk_1"
            removal(19) = "disk 1"
            removal(20) = "disk-1"
            removal(21) = "pt1"
            removal(22) = "pt.1"
            removal(23) = "pt_1"
            removal(24) = "pt 1"
            removal(25) = "pt-1"
            removal(26) = "ac3"
            removal(27) = "divx"
            removal(28) = "xvid"
            removal(29) = "dvdrip"
            removal(30) = "directors cut"
            removal(31) = "special edition"
            removal(32) = "screener"
            removal(33) = "telesync"
            removal(34) = "telecine"
            removal(35) = "director's cut"
            removal(36) = " r5"
            removal(37) = " scr"
            removal(38) = ".scr"
            removal(39) = "_scr"
            removal(40) = "-scr"
            removal(41) = " ts"
            removal(42) = "_ts"
            removal(43) = ".ts"
            removal(44) = "-ts"
            removal(45) = " fs"
            removal(46) = ".fs"
            removal(47) = "_fs"
            removal(48) = "-fs"
            removal(49) = " ws"
            removal(50) = ".ws"
            removal(51) = "_ws"
            removal(52) = "-ws"
            removal(53) = "-r5"
            removal(54) = "_r5"
            removal(55) = ".r5"
            removal(56) = "bluray"
            removal(57) = "720"
            removal(58) = "1024"
            removal(59) = "fullscreen"
            removal(60) = "widescreen"
            removal(61) = "dvdscr"
            removal(62) = "part01"
            removal(63) = "dvd5"
            removal(64) = "dvd9"
            removal(65) = "dvd 5"
            removal(66) = "dvd 9"
            removal(67) = "dvd-5"
            removal(68) = "dvd-9"
            removal(69) = "dvd_5"
            removal(70) = "dvd_9"
            removal(71) = "dvd.5"
            removal(72) = "dvd.9"
            removal(73) = "x264"
            removal(74) = "dts"
            removal(75) = "bluray"
            Dim currentposition As Integer = filename.Length
            Dim newposition As Integer = filename.Length
            For f = 1 To 75
                If filename.ToLower.IndexOf(removal(f)) <> -1 Then
                    newposition = filename.ToLower.IndexOf(removal(f))
                    If newposition < currentposition Then currentposition = newposition
                End If
            Next
            If movieyear <> Nothing Then
                If filename.IndexOf(movieyear) <> -1 Then
                    newposition = filename.IndexOf(movieyear)
                    If newposition < currentposition Then currentposition = newposition
                End If
            End If
            If currentposition < filename.Length And currentposition > 0 Then
                filename = filename.Substring(0, currentposition)
                If filename.Substring(filename.Length - 1, 1) = "-" Or filename.Substring(filename.Length - 1, 1) = "_" Or filename.Substring(filename.Length - 1, 1) = "." Or filename.Substring(filename.Length - 1, 1) = " " Then
                    filename = filename.Substring(0, filename.Length - 1)
                End If
            End If

            If filename <> "" Then
                cleanname = filename
            End If
            cleanname = Trim(cleanname)
            cleanname = cleanname.Replace(".", " ")         'replace '.' with ' ' in name for OS's that can't have spaces in filenames.....
            Return cleanname
        Catch ex As Exception
            cleanname = "error"
            Return cleanname
        Finally

        End Try
    End Function

    Public Shared Function Get_HdTags(ByVal filename As String)
        Try
            If IO.Path.GetFileName(filename).ToLower = "video_ts.ifo" Then
                Dim temppath As String = filename.Replace(IO.Path.GetFileName(filename), "VTS_01_0.IFO")
                If IO.File.Exists(temppath) Then
                    filename = temppath
                End If
            End If

            Dim playlist As New List(Of String)
            Dim tempstring As String
            tempstring = GetFileName(filename)
            playlist = GetMediaList(tempstring)

            If Not IO.File.Exists(filename) Then
                Return "Error"
            End If
            Dim workingfiledetails As New FullFileDetails
            Dim MI As New mediainfo
            'MI = New mediainfo
            MI.Open(filename)
            Dim curVS As Integer = 0
            Dim addVS As Boolean = False
            Dim numOfVideoStreams As Integer = MI.Count_Get(StreamKind.Visual)

            Dim tempmediainfo As String
            Dim tempmediainfo2 As String

            workingfiledetails.filedetails_video.width = MI.Get_(StreamKind.Visual, curVS, "Width")
            workingfiledetails.filedetails_video.height = MI.Get_(StreamKind.Visual, curVS, "Height")
            If workingfiledetails.filedetails_video.width <> Nothing Then
                If IsNumeric(workingfiledetails.filedetails_video.width) Then
                    If workingfiledetails.filedetails_video.height <> Nothing Then
                        If IsNumeric(workingfiledetails.filedetails_video.height) Then
                            '                            Dim tempwidth As Integer = Convert.ToInt32(workingfiledetails.filedetails_video.width)
                            '                            Dim tempheight As Integer = Convert.ToInt32(workingfiledetails.filedetails_video.height)
                            '                            Dim aspect As Decimal
                            Try
                                '                                aspect = tempwidth / tempheight  'Next three line are wrong for getting display aspect ratio
                                '                                aspect = FormatNumber(aspect, 3)
                                '                                If aspect > 0 Then workingfiledetails.filedetails_video.aspect = aspect.ToString

                                Dim Information As String = MI.Inform
                                Dim BeginString As Integer = Information.ToLower.IndexOf(":", Information.ToLower.IndexOf("display aspect ratio"))
                                Dim EndString As Integer = Information.ToLower.IndexOf("frame rate")
                                Dim SizeofString As Integer = EndString - BeginString
                                Dim DisplayAspectRatio As String = Information.Substring(BeginString, SizeofString).Trim(" ", ":", Chr(10), Chr(13))
                                'DisplayAspectRatio = DisplayAspectRatio.Substring(0, Len(DisplayAspectRatio) - 1)
                                If Len(DisplayAspectRatio) > 0 Then
                                    workingfiledetails.filedetails_video.aspect = DisplayAspectRatio
                                Else
                                    workingfiledetails.filedetails_video.aspect = "Unknown"
                                End If

                            Catch ex As Exception

                            End Try
                        End If
                    End If
                End If
            End If
            'workingfiledetails.filedetails_video.aspect = MI.Get_(StreamKind.Visual, 0, 79)


            tempmediainfo = MI.Get_(StreamKind.Visual, curVS, "Format")
            If tempmediainfo.ToLower = "avc" Then
                tempmediainfo2 = "h264"
            Else
                tempmediainfo2 = tempmediainfo
            End If

            'workingfiledetails.filedetails_video.codec = tempmediainfo2
            'workingfiledetails.filedetails_video.formatinfo = tempmediainfo
            workingfiledetails.filedetails_video.codec = MI.Get_(StreamKind.Visual, curVS, "CodecID")
            If workingfiledetails.filedetails_video.codec = "DX50" Then
                workingfiledetails.filedetails_video.codec = "DIVX"
            End If
            '_MPEG4/ISO/AVC
            If workingfiledetails.filedetails_video.codec.ToLower.IndexOf("mpeg4/iso/avc") <> -1 Then
                workingfiledetails.filedetails_video.codec = "h264"
            End If
            workingfiledetails.filedetails_video.formatinfo = MI.Get_(StreamKind.Visual, curVS, "CodecID")
            Dim fs(100) As String
            For f = 1 To 100
                fs(f) = MI.Get_(StreamKind.Visual, 0, f)
            Next

            Try
                If playlist.Count = 1 Then
                    workingfiledetails.filedetails_video.duration = MI.Get_(StreamKind.Visual, 0, 61)
                ElseIf playlist.Count > 1 Then
                    Dim totalmins As Integer = 0
                    For f = 0 To playlist.Count - 1
                        Dim M2 As mediainfo
                        M2 = New mediainfo
                        M2.Open(playlist(f))
                        Dim temptime As String = M2.Get_(StreamKind.Visual, 0, 61)
                        Dim tempint As Integer
                        If temptime <> Nothing Then
                            Try
                                '1h 24mn 48s 546ms
                                Dim hours As Integer = 0
                                Dim minutes As Integer = 0
                                Dim tempstring2 As String = temptime
                                tempint = tempstring2.IndexOf("h")
                                If tempint <> -1 Then
                                    hours = Convert.ToInt32(tempstring2.Substring(0, tempint))
                                    tempstring2 = tempstring2.Substring(tempint + 1, tempstring2.Length - (tempint + 1))
                                    tempstring2 = Trim(tempstring2)
                                End If
                                tempint = tempstring2.IndexOf("mn")
                                If tempint <> -1 Then
                                    minutes = Convert.ToInt32(tempstring2.Substring(0, tempint))
                                End If
                                If hours <> 0 Then
                                    hours = hours * 60
                                End If
                                minutes = minutes + hours
                                totalmins = totalmins + minutes
                            Catch
                            End Try
                        End If
                    Next
                    workingfiledetails.filedetails_video.duration = totalmins & " min"
                End If
            Catch
                workingfiledetails.filedetails_video.duration = MI.Get_(StreamKind.Visual, 0, 57)
            End Try
            workingfiledetails.filedetails_video.bitrate = MI.Get_(StreamKind.Visual, curVS, "BitRate/String")
            workingfiledetails.filedetails_video.bitratemode = MI.Get_(StreamKind.Visual, curVS, "BitRate_Mode/String")

            workingfiledetails.filedetails_video.bitratemax = MI.Get_(StreamKind.Visual, curVS, "BitRate_Maximum/String")

            tempmediainfo = IO.Path.GetExtension(filename) '"This is the extension of the file"
            workingfiledetails.filedetails_video.container = tempmediainfo
            'workingfiledetails.filedetails_video.codecid = MI.Get_(StreamKind.Visual, curVS, "CodecID")

            workingfiledetails.filedetails_video.codecinfo = MI.Get_(StreamKind.Visual, curVS, "CodecID/Info")
            workingfiledetails.filedetails_video.scantype = MI.Get_(StreamKind.Visual, curVS, 102)
            'Video()
            'Format                     : MPEG-4 Visual
            'Format profile             : Streaming Video@L1
            'Format(settings, BVOP)     : Yes()
            'Format(settings, QPel)     : No()
            'Format(settings, GMC)      : No(warppoints)
            'Format(settings, Matrix)   : Custom()
            'Codec(ID)                  : XVID()
            'Codec(ID / Hint)           : XviD()
            'Duration                   : 1h 33mn
            'Bit rate                   : 903 Kbps
            'Width                      : 528 pixels
            'Height                     : 272 pixels
            'Display aspect ratio       : 1.941
            'Frame rate                 : 25.000 fps
            'Resolution                 : 24 bits
            'Colorimetry                : 4:2:0
            'Scan(Type)                 : Progressive()
            'Bits/(Pixel*Frame)         : 0.252
            'Stream size                : 604 MiB (86%)
            'Writing library            : XviD 1.0.3 (UTC 2004-12-20)

            Dim numOfAudioStreams As Integer = MI.Count_Get(StreamKind.Audio)
            Dim curAS As Integer = 0
            Dim addAS As Boolean = False

            'get audio data
            If numOfAudioStreams > 0 Then
                While curAS < numOfAudioStreams
                    Dim audio As New str_MediaNFOAudio(SetDefaults)
                    audio.language = GetLangCode(MI.Get_(StreamKind.Audio, curAS, "Language/String"))
                    If MI.Get_(StreamKind.Audio, curAS, "Format") = "MPEG Audio" Then
                        audio.codec = "MP3"
                    Else
                        audio.codec = MI.Get_(StreamKind.Audio, curAS, "Format")
                    End If
                    If audio.codec = "AC-3" Then
                        audio.codec = "AC3"
                    End If
                    If audio.codec = "DTS" Then
                        audio.codec = "dca"
                    End If
                    audio.channels = MI.Get_(StreamKind.Audio, curAS, "Channel(s)")
                    audio.bitrate = MI.Get_(StreamKind.Audio, curAS, "BitRate/String")
                    workingfiledetails.filedetails_audio.Add(audio)
                    curAS += 1
                End While
            End If


            Dim numOfSubtitleStreams As Integer = MI.Count_Get(StreamKind.Text)
            Dim curSS As Integer = 0
            If numOfSubtitleStreams > 0 Then
                While curSS < numOfSubtitleStreams
                    Dim sublanguage As New str_MediaNFOSubtitles(setdefaults)
                    sublanguage.language = GetLangCode(MI.Get_(StreamKind.Text, curSS, "Language/String"))
                    workingfiledetails.filedetails_subtitles.Add(sublanguage)
                    curSS += 1
                End While
            End If

            Return workingfiledetails
        Catch ex As Exception

        Finally
        End Try
        Return "Error"
    End Function
    Public Shared Function GetLangCode(ByVal strLang As String) As String
        Try
            Select Case strLang.ToLower
                Case "english"
                    Return "eng"
                Case "german"
                    Return "deu"
                Case ""
                    Return ""
                Case "afar"
                    Return "aar"
                Case "abkhazian"
                    Return "abk"
                Case "achinese"
                    Return "ace"
                Case "acoli"
                    Return "ach"
                Case "adangme"
                    Return "ada"
                Case "adyghe", "adygei"
                    Return "ady"
                Case "afro-asiatic (other)"
                    Return "afa"
                Case "afrihili"
                    Return "afh"
                Case "afrikaans"
                    Return "afr"
                Case "ainu"
                    Return "ain"
                Case "akan"
                    Return "aka"
                Case "akkadian"
                    Return "akk"
                Case "albanian"
                    Return "alb"
                Case "aleut"
                    Return "ale"
                Case "algonquian languages"
                    Return "alg"
                Case "southern altai"
                    Return "alt"
                Case "amharic"
                    Return "amh"
                Case "english"
                    Return "ang"
                Case "angika"
                    Return "anp"
                Case "apache languages"
                    Return "apa"
                Case "arabic"
                    Return "ara"
                Case "official aramaic (700-300 bce)", "imperial aramaic (700-300 bce)"
                    Return "arc"
                Case "aragonese"
                    Return "arg"
                Case "armenian"
                    Return "arm"
                Case "mapudungun", "mapuche"
                    Return "arn"
                Case "arapaho"
                    Return "arp"
                Case "artificial (other)"
                    Return "art"
                Case "arawak"
                    Return "arw"
                Case "assamese"
                    Return "asm"
                Case "asturian", "bable", "leonese", "asturleonese"
                    Return "ast"
                Case "athapascan languages"
                    Return "ath"
                Case "australian languages"
                    Return "aus"
                Case "avaric"
                    Return "ava"
                Case "avestan"
                    Return "ave"
                Case "awadhi"
                    Return "awa"
                Case "aymara"
                    Return "aym"
                Case "azerbaijani"
                    Return "aze"
                Case "banda languages"
                    Return "bad"
                Case "bamileke languages"
                    Return "bai"
                Case "bashkir"
                    Return "bak"
                Case "baluchi"
                    Return "bal"
                Case "bambara"
                    Return "bam"
                Case "balinese"
                    Return "ban"
                Case "basque"
                    Return "baq"
                Case "basa"
                    Return "bas"
                Case "baltic (other)"
                    Return "bat"
                Case "beja", "bedawiyet"
                    Return "bej"
                Case "belarusian"
                    Return "bel"
                Case "bemba"
                    Return "bem"
                Case "bengali"
                    Return "ben"
                Case "berber (other)"
                    Return "ber"
                Case "bhojpuri"
                    Return "bho"
                Case "bihari"
                    Return "bih"
                Case "bikol"
                    Return "bik"
                Case "bini", "edo"
                    Return "bin"
                Case "bislama"
                    Return "bis"
                Case "siksika"
                    Return "bla"
                Case "bantu (other)"
                    Return "bnt"
                Case "bosnian"
                    Return "bos"
                Case "braj"
                    Return "bra"
                Case "breton"
                    Return "bre"
                Case "batak languages"
                    Return "btk"
                Case "buriat"
                    Return "bua"
                Case "buginese"
                    Return "bug"
                Case "bulgarian"
                    Return "bul"
                Case "burmese"
                    Return "bur"
                Case "blin", "bilin"
                    Return "byn"
                Case "caddo"
                    Return "cad"
                Case "central american indian (other)"
                    Return "cai"
                Case "galibi carib"
                    Return "car"
                Case "catalan", "valencian"
                    Return "cat"
                Case "caucasian (other)"
                    Return "cau"
                Case "cebuano"
                    Return "ceb"
                Case "celtic (other)"
                    Return "cel"
                Case "chamorro"
                    Return "cha"
                Case "chibcha"
                    Return "chb"
                Case "chechen"
                    Return "che"
                Case "chagatai"
                    Return "chg"
                Case "chinese"
                    Return "chi"
                Case "chuukese"
                    Return "chk"
                Case "mari"
                    Return "chm"
                Case "chinook jargon"
                    Return "chn"
                Case "choctaw"
                    Return "cho"
                Case "chipewyan", "dene suline"
                    Return "chp"
                Case "cherokee"
                    Return "chr"
                Case "church slavic", "old slavonic", "church slavonic", "old bulgarian", "old church slavonic"
                    Return "chu"
                Case "chuvash"
                    Return "chv"
                Case "cheyenne"
                    Return "chy"
                Case "chamic languages"
                    Return "cmc"
                Case "coptic"
                    Return "cop"
                Case "cornish"
                    Return "cor"
                Case "corsican"
                    Return "cos"
                Case "creoles and pidgins"
                    Return "cpe"
                Case "creoles and pidgins"
                    Return "cpf"
                Case "creoles and pidgins"
                    Return "cpp"
                Case "cree"
                    Return "cre"
                Case "crimean tatar", "crimean turkish"
                    Return "crh"
                Case "creoles and pidgins (other)"
                    Return "crp"
                Case "kashubian"
                    Return "csb"
                Case "cushitic (other)"
                    Return "cus"
                Case "czech"
                    Return "cze"
                Case "dakota"
                    Return "dak"
                Case "danish"
                    Return "dan"
                Case "dargwa"
                    Return "dar"
                Case "land dayak languages"
                    Return "day"
                Case "delaware"
                    Return "del"
                Case "slave (athapascan)"
                    Return "den"
                Case "dogrib"
                    Return "dgr"
                Case "dinka"
                    Return "din"
                Case "divehi", "dhivehi", "maldivian"
                    Return "div"
                Case "dogri"
                    Return "doi"
                Case "dravidian (other)"
                    Return "dra"
                Case "lower sorbian"
                    Return "dsb"
                Case "duala"
                    Return "dua"
                Case "dutch"
                    Return "dum"
                Case "dutch", "flemish"
                    Return "dut"
                Case "dyula"
                    Return "dyu"
                Case "dzongkha"
                    Return "dzo"
                Case "efik"
                    Return "efi"
                Case "egyptian (ancient)"
                    Return "egy"
                Case "ekajuk"
                    Return "eka"
                Case "elamite"
                    Return "elx"
                Case "english"
                    Return "eng"
                Case "english"
                    Return "enm"
                Case "esperanto"
                    Return "epo"
                Case "estonian"
                    Return "est"
                Case "ewe"
                    Return "ewe"
                Case "ewondo"
                    Return "ewo"
                Case "fang"
                    Return "fan"
                Case "faroese"
                    Return "fao"
                Case "fanti"
                    Return "fat"
                Case "fijian"
                    Return "fij"
                Case "filipino", "pilipino"
                    Return "fil"
                Case "finnish"
                    Return "fin"
                Case "finno-ugrian (other)"
                    Return "fiu"
                Case "fon"
                    Return "fon"
                Case "french"
                    Return "fre"
                Case "french"
                    Return "frm"
                Case "french"
                    Return "fro"
                Case "northern frisian"
                    Return "frr"
                Case "eastern frisian"
                    Return "frs"
                Case "western frisian"
                    Return "fry"
                Case "fulah"
                    Return "ful"
                Case "friulian"
                    Return "fur"
                Case "ga"
                    Return "gaa"
                Case "gayo"
                    Return "gay"
                Case "gbaya"
                    Return "gba"
                Case "germanic (other)"
                    Return "gem"
                Case "georgian"
                    Return "geo"
                Case "german"
                    Return "ger"
                Case "geez"
                    Return "gez"
                Case "gilbertese"
                    Return "gil"
                Case "gaelic", "scottish gaelic"
                    Return "gla"
                Case "irish"
                    Return "gle"
                Case "galician"
                    Return "glg"
                Case "manx"
                    Return "glv"
                Case "german"
                    Return "gmh"
                Case "german"
                    Return "goh"
                Case "gondi"
                    Return "gon"
                Case "gorontalo"
                    Return "gor"
                Case "gothic"
                    Return "got"
                Case "grebo"
                    Return "grb"
                Case "greek"
                    Return "grc"
                Case "greek"
                    Return "gre"
                Case "guarani"
                    Return "grn"
                Case "swiss german", "alemannic", "alsatian"
                    Return "gsw"
                Case "gujarati"
                    Return "guj"
                Case "gwich'in"
                    Return "gwi"
                Case "haida"
                    Return "hai"
                Case "haitian", "haitian creole"
                    Return "hat"
                Case "hausa"
                    Return "hau"
                Case "hawaiian"
                    Return "haw"
                Case "hebrew"
                    Return "heb"
                Case "herero"
                    Return "her"
                Case "hiligaynon"
                    Return "hil"
                Case "himachali"
                    Return "him"
                Case "hindi"
                    Return "hin"
                Case "hittite"
                    Return "hit"
                Case "hmong"
                    Return "hmn"
                Case "hiri motu"
                    Return "hmo"
                Case "croatian"
                    Return "hrv"
                Case "upper sorbian"
                    Return "hsb"
                Case "hungarian"
                    Return "hun"
                Case "hupa"
                    Return "hup"
                Case "iban"
                    Return "iba"
                Case "igbo"
                    Return "ibo"
                Case "icelandic"
                    Return "ice"
                Case "ido"
                    Return "ido"
                Case "sichuan yi", "nuosu"
                    Return "iii"
                Case "ijo languages"
                    Return "ijo"
                Case "inuktitut"
                    Return "iku"
                Case "interlingue", "occidental"
                    Return "ile"
                Case "iloko"
                    Return "ilo"
                Case "interlingua (international auxiliary language association)"
                    Return "ina"
                Case "indic (other)"
                    Return "inc"
                Case "indonesian"
                    Return "ind"
                Case "indo-european (other)"
                    Return "ine"
                Case "ingush"
                    Return "inh"
                Case "inupiaq"
                    Return "ipk"
                Case "iranian (other)"
                    Return "ira"
                Case "iroquoian languages"
                    Return "iro"
                Case "italian"
                    Return "ita"
                Case "javanese"
                    Return "jav"
                Case "lojban"
                    Return "jbo"
                Case "japanese"
                    Return "jpn"
                Case "judeo-persian"
                    Return "jpr"
                Case "judeo-arabic"
                    Return "jrb"
                Case "kara-kalpak"
                    Return "kaa"
                Case "kabyle"
                    Return "kab"
                Case "kachin", "jingpho"
                    Return "kac"
                Case "kalaallisut", "greenlandic"
                    Return "kal"
                Case "kamba"
                    Return "kam"
                Case "kannada"
                    Return "kan"
                Case "karen languages"
                    Return "kar"
                Case "kashmiri"
                    Return "kas"
                Case "kanuri"
                    Return "kau"
                Case "kawi"
                    Return "kaw"
                Case "kazakh"
                    Return "kaz"
                Case "kabardian"
                    Return "kbd"
                Case "khasi"
                    Return "kha"
                Case "khoisan (other)"
                    Return "khi"
                Case "central khmer"
                    Return "khm"
                Case "khotanese", "sakan"
                    Return "kho"
                Case "kikuyu", "gikuyu"
                    Return "kik"
                Case "kinyarwanda"
                    Return "kin"
                Case "kirghiz", "kyrgyz"
                    Return "kir"
                Case "kimbundu"
                    Return "kmb"
                Case "konkani"
                    Return "kok"
                Case "komi"
                    Return "kom"
                Case "kongo"
                    Return "kon"
                Case "korean"
                    Return "kor"
                Case "kosraean"
                    Return "kos"
                Case "kpelle"
                    Return "kpe"
                Case "karachay-balkar"
                    Return "krc"
                Case "karelian"
                    Return "krl"
                Case "kru languages"
                    Return "kro"
                Case "kurukh"
                    Return "kru"
                Case "kuanyama", "kwanyama"
                    Return "kua"
                Case "kumyk"
                    Return "kum"
                Case "kurdish"
                    Return "kur"
                Case "kutenai"
                    Return "kut"
                Case "ladino"
                    Return "lad"
                Case "lahnda"
                    Return "lah"
                Case "lamba"
                    Return "lam"
                Case "lao"
                    Return "lao"
                Case "latin"
                    Return "lat"
                Case "latvian"
                    Return "lav"
                Case "lezghian"
                    Return "lez"
                Case "limburgan", "limburger", "limburgish"
                    Return "lim"
                Case "lingala"
                    Return "lin"
                Case "lithuanian"
                    Return "lit"
                Case "mongo"
                    Return "lol"
                Case "lozi"
                    Return "loz"
                Case "luxembourgish", "letzeburgesch"
                    Return "ltz"
                Case "luba-lulua"
                    Return "lua"
                Case "luba-katanga"
                    Return "lub"
                Case "ganda"
                    Return "lug"
                Case "luiseno"
                    Return "lui"
                Case "lunda"
                    Return "lun"
                Case "luo (kenya and tanzania)"
                    Return "luo"
                Case "lushai"
                    Return "lus"
                Case "macedonian"
                    Return "mac"
                Case "madurese"
                    Return "mad"
                Case "magahi"
                    Return "mag"
                Case "marshallese"
                    Return "mah"
                Case "maithili"
                    Return "mai"
                Case "makasar"
                    Return "mak"
                Case "malayalam"
                    Return "mal"
                Case "mandingo"
                    Return "man"
                Case "maori"
                    Return "mao"
                Case "austronesian (other)"
                    Return "map"
                Case "marathi"
                    Return "mar"
                Case "masai"
                    Return "mas"
                Case "malay"
                    Return "may"
                Case "moksha"
                    Return "mdf"
                Case "mandar"
                    Return "mdr"
                Case "mende"
                    Return "men"
                Case "irish"
                    Return "mga"
                Case "mi'kmaq", "micmac"
                    Return "mic"
                Case "minangkabau"
                    Return "min"
                Case "uncoded languages"
                    Return "mis"
                Case "mon-khmer (other)"
                    Return "mkh"
                Case "malagasy"
                    Return "mlg"
                Case "maltese"
                    Return "mlt"
                Case "manchu"
                    Return ("mnc")
                Case "manipuri"
                    Return "mni"
                Case "manobo languages"
                    Return "mno"
                Case "mohawk"
                    Return "moh"
                Case "mongolian"
                    Return "mon"
                Case "mossi"
                    Return "mos"
                Case "multiple languages"
                    Return "mul"
                Case "munda languages"
                    Return "mun"
                Case "creek"
                    Return "mus"
                Case "mirandese"
                    Return "mwl"
                Case "marwari"
                    Return "mwr"
                Case "mayan languages"
                    Return "myn"
                Case "erzya"
                    Return "myv"
                Case "nahuatl languages"
                    Return "nah"
                Case "north american indian"
                    Return "nai"
                Case "neapolitan"
                    Return "nap"
                Case "nauru"
                    Return "nau"
                Case "navajo", "navaho"
                    Return "nav"
                Case "ndebele"
                    Return "nbl"
                Case "ndebele"
                    Return "nde"
                Case "ndonga"
                    Return "ndo"
                Case "low german", "low saxon", "german"
                    Return "nds"
                Case "nepali"
                    Return "nep"
                Case "nepal bhasa", "newari"
                    Return "new"
                Case "nias"
                    Return "nia"
                Case "niger-kordofanian (other)"
                    Return "nic"
                Case "niuean"
                    Return "niu"
                Case "norwegian nynorsk", "nynorsk"
                    Return "nno"
                Case "bokmål"
                    Return "nob"
                Case "nogai"
                    Return "nog"
                Case "norse"
                    Return "non"
                Case "norwegian"
                    Return "nor"
                Case "n'ko"
                    Return "nqo"
                Case "pedi", "sepedi", "northern sotho"
                    Return "nso"
                Case "nubian languages"
                    Return "nub"
                Case "classical newari", "old newari", "classical nepal bhasa"
                    Return "nwc"
                Case "chichewa", "chewa", "nyanja"
                    Return "nya"
                Case "nyamwezi"
                    Return "nym"
                Case "nyankole"
                    Return "nyn"
                Case "nyoro"
                    Return "nyo"
                Case "nzima"
                    Return "nzi"
                Case "occitan (post 1500)", "provençal"
                    Return "oci"
                Case "ojibwa"
                    Return "oji"
                Case "oriya"
                    Return "ori"
                Case "oromo"
                    Return "orm"
                Case "osage"
                    Return "osa"
                Case "ossetian", "ossetic"
                    Return "oss"
                Case "turkish"
                    Return "ota"
                Case "otomian languages"
                    Return "oto"
                Case "papuan (other)"
                    Return "paa"
                Case "pangasinan"
                    Return "pag"
                Case "pahlavi"
                    Return "pal"
                Case "pampanga", "kapampangan"
                    Return "pam"
                Case "panjabi", "punjabi"
                    Return "pan"
                Case "papiamento"
                    Return "pap"
                Case "palauan"
                    Return "pau"
                Case "persian"
                    Return "peo"
                Case "persian"
                    Return "per"
                Case "philippine (other)"
                    Return "phi"
                Case "phoenician"
                    Return "phn"
                Case "pali"
                    Return "pli"
                Case "polish"
                    Return "pol"
                Case "pohnpeian"
                    Return "pon"
                Case "portuguese"
                    Return "por"
                Case "prakrit languages"
                    Return "pra"
                Case "provençal"
                    Return "pro"
                Case "pushto", "pashto"
                    Return "pus"
                Case "reserved for local use"
                    Return "qaa-qtz"
                Case "quechua"
                    Return "que"
                Case "rajasthani"
                    Return "raj"
                Case "rapanui"
                    Return "rap"
                Case "rarotongan", "cook islands maori"
                    Return "rar"
                Case "romance (other)"
                    Return "roa"
                Case "romansh"
                    Return "roh"
                Case "romany"
                    Return "rom"
                Case "romanian", "moldavian", "moldovan"
                    Return "rum"
                Case "rundi"
                    Return "run"
                Case "aromanian", "arumanian", "macedo-romanian"
                    Return "rup"
                Case "russian"
                    Return "rus"
                Case "sandawe"
                    Return "sad"
                Case "sango"
                    Return "sag"
                Case "yakut"
                    Return "sah"
                Case "south american indian (other)"
                    Return "sai"
                Case "salishan languages"
                    Return "sal"
                Case "samaritan aramaic"
                    Return "sam"
                Case "sanskrit"
                    Return "san"
                Case "sasak"
                    Return "sas"
                Case "santali"
                    Return "sat"
                Case "sicilian"
                    Return "scn"
                Case "scots"
                    Return "sco"
                Case "selkup"
                    Return "sel"
                Case "semitic (other)"
                    Return "sem"
                Case "irish"
                    Return "sga"
                Case "sign languages"
                    Return "sgn"
                Case "shan"
                    Return "shn"
                Case "sidamo"
                    Return "sid"
                Case "sinhala", "sinhalese"
                    Return "sin"
                Case "siouan languages"
                    Return "sio"
                Case "sino-tibetan (other)"
                    Return "sit"
                Case "slavic (other)"
                    Return "sla"
                Case "slovak"
                    Return "slo"
                Case "slovenian"
                    Return "slv"
                Case "southern sami"
                    Return "sma"
                Case "northern sami"
                    Return "sme"
                Case "sami languages (other)"
                    Return "smi"
                Case "lule sami"
                    Return "smj"
                Case "inari sami"
                    Return "smn"
                Case "samoan"
                    Return "smo"
                Case "skolt sami"
                    Return "sms"
                Case "shona"
                    Return "sna"
                Case "sindhi"
                    Return "snd"
                Case "soninke"
                    Return "snk"
                Case "sogdian"
                    Return "sog"
                Case "somali"
                    Return "som"
                Case "songhai languages"
                    Return "son"
                Case "sotho"
                    Return "sot"
                Case "spanish", "castilian"
                    Return "spa"
                Case "sardinian"
                    Return "srd"
                Case "sranan tongo"
                    Return "srn"
                Case "serbian"
                    Return "srp"
                Case "serer"
                    Return "srr"
                Case "nilo-saharan (other)"
                    Return "ssa"
                Case "swati"
                    Return "ssw"
                Case "sukuma"
                    Return "suk"
                Case "sundanese"
                    Return "sun"
                Case "susu"
                    Return "sus"
                Case "sumerian"
                    Return "sux"
                Case "swahili"
                    Return "swa"
                Case "swedish"
                    Return "swe"
                Case "classical syriac"
                    Return "syc"
                Case "syriac"
                    Return "syr"
                Case "tahitian"
                    Return "tah"
                Case "tai (other)"
                    Return "tai"
                Case "tamil"
                    Return "tam"
                Case "tatar"
                    Return "tat"
                Case "telugu"
                    Return "tel"
                Case "timne"
                    Return "tem"
                Case "tereno"
                    Return "ter"
                Case "tetum"
                    Return "tet"
                Case "tajik"
                    Return "tgk"
                Case "tagalog"
                    Return "tgl"
                Case "thai"
                    Return "tha"
                Case "tibetan"
                    Return "tib"
                Case "tigre"
                    Return "tig"
                Case "tigrinya"
                    Return "tir"
                Case "tiv"
                    Return "tiv"
                Case "tokelau"
                    Return "tkl"
                Case "klingon", "tlhingan-hol"
                    Return "tlh"
                Case "tlingit"
                    Return "tli"
                Case "tamashek"
                    Return "tmh"
                Case "tonga (nyasa)"
                    Return "tog"
                Case "tonga (tonga islands)"
                    Return "ton"
                Case "tok pisin"
                    Return "tpi"
                Case "tsimshian"
                    Return "tsi"
                Case "tswana"
                    Return "tsn"
                Case "tsonga"
                    Return "tso"
                Case "turkmen"
                    Return "tuk"
                Case "tumbuka"
                    Return "tum"
                Case "tupi languages"
                    Return "tup"
                Case "turkish"
                    Return "tur"
                Case "altaic (other)"
                    Return "tut"
                Case "tuvalu"
                    Return "tvl"
                Case "twi"
                    Return "twi"
                Case "tuvinian"
                    Return "tyv"
                Case "udmurt"
                    Return "udm"
                Case "ugaritic"
                    Return "uga"
                Case "uighur", "uyghur"
                    Return "uig"
                Case "ukrainian"
                    Return "ukr"
                Case "umbundu"
                    Return "umb"
                Case "undetermined"
                    Return "und"
                Case "urdu"
                    Return "urd"
                Case "uzbek"
                    Return "uzb"
                Case "vai"
                    Return "vai"
                Case "venda"
                    Return "ven"
                Case "vietnamese"
                    Return "vie"
                Case "volapük"
                    Return "vol"
                Case "votic"
                    Return "vot"
                Case "wakashan languages"
                    Return "wak"
                Case "walamo"
                    Return "wal"
                Case "waray"
                    Return "war"
                Case "washo"
                    Return "was"
                Case "welsh"
                    Return "wel"
                Case "sorbian languages"
                    Return "wen"
                Case "walloon"
                    Return "wln"
                Case "wolof"
                    Return "wol"
                Case "kalmyk", "oirat"
                    Return "xal"
                Case "xhosa"
                    Return "xho"
                Case "yao"
                    Return "yao"
                Case "yapese"
                    Return "yap"
                Case "yiddish"
                    Return "yid"
                Case "yoruba"
                    Return "yor"
                Case "yupik languages"
                    Return "ypk"
                Case "zapotec"
                    Return "zap"
                Case "blissymbols", "blissymbolics", "bliss"
                    Return "zbl"
                Case "zenaga"
                    Return "zen"
                Case "zhuang", "chuang"
                    Return "zha"
                Case "zande languages"
                    Return "znd"
                Case "zulu"
                    Return "zul"
                Case "zuni"
                    Return "zun"
                Case "no linguistic content", "not applicable"
                    Return "zxx"
                Case "zaza", "dimili", "dimli", "kirdki", "kirmanjki", "zazaki"
                    Return "zza"
            End Select



        Catch
        Finally
        End Try
        Return "Error"
    End Function
    Public Shared Function GetActorThumbPath(Optional ByVal location As String = "")
        Dim actualpath As String = ""
        Try
            If location = Nothing Then
                Return "none"
                Exit Function
            End If
            If location = "" Then
                Return "none"
                Exit Function
            End If

            If location.IndexOf("http") <> -1 Then
                Return location
                Exit Function
            Else
                If location.IndexOf(Preferences.actornetworkpath) <> -1 Then
                    If Preferences.actornetworkpath <> Nothing And Preferences.actorsavepath <> Nothing Then
                        If Preferences.actornetworkpath <> "" And Preferences.actorsavepath <> "" Then
                            Dim filename As String = IO.Path.GetFileName(location)
                            actualpath = IO.Path.Combine(Preferences.actorsavepath, filename)
                            If Not IO.File.Exists(actualpath) Then
                                Dim extension As String = IO.Path.GetExtension(location)
                                Dim purename As String = IO.Path.GetFileName(location)
                                purename = purename.Replace(extension, "")
                                actualpath = Preferences.actorsavepath & "\" & purename.Substring(purename.Length - 2, 2) & "\" & filename
                            End If
                            If Not IO.File.Exists(actualpath) Then
                                actualpath = "none"
                            End If
                        End If
                    Else
                        actualpath = "none"
                    End If
                Else
                    actualpath = "none"
                End If
            End If
            If actualpath = "" Then actualpath = "none"
            Return actualpath
        Catch
            Return "none"
        Finally

        End Try
    End Function
    Public Shared Function GetMediaList(ByVal pathandfilename As String)

        Try
            Dim tempstring As String = pathandfilename
            Dim playlist As New List(Of String)
            If IO.File.Exists(tempstring) Then
                playlist.Add(tempstring)
            End If
            tempstring = tempstring.ToLower
            If tempstring.IndexOf("cd1") <> -1 Then
                tempstring = tempstring.Replace("cd1", "cd2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd2", "cd3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd3", "cd4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd4", "cd5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("cd_1") <> -1 Then
                tempstring = tempstring.Replace("cd_1", "cd_2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd_2", "cd_3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd_3", "cd_4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd_4", "cd_5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("cd 1") <> -1 Then
                tempstring = tempstring.Replace("cd 1", "cd 2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd 2", "cd 3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd 3", "cd 4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd 4", "cd 5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("cd.1") <> -1 Then
                tempstring = tempstring.Replace("cd.1", "cd.2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd.2", "cd.3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd.3", "cd.4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("cd.4", "cd.5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("dvd1") <> -1 Then
                tempstring = tempstring.Replace("dvd1", "dvd2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd2", "dvd3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd3", "dvd4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd4", "dvd5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("dvd_1") <> -1 Then
                tempstring = tempstring.Replace("dvd_1", "dvd_2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd_2", "dvd_3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd_3", "dvd_4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd_4", "dvd_5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("dvd 1") <> -1 Then
                tempstring = tempstring.Replace("dvd 1", "dvd 2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd 2", "dvd 3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd 3", "dvd 4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd 4", "dvd 5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("dvd.1") <> -1 Then
                tempstring = tempstring.Replace("dvd.1", "dvd.2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd.2", "dvd.3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd.3", "dvd.4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("dvd.4", "dvd.5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("part1") <> -1 Then
                tempstring = tempstring.Replace("part1", "part2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part2", "part3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part3", "part4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part4", "part5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("part_1") <> -1 Then
                tempstring = tempstring.Replace("part_1", "part_2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part_2", "part_3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part_3", "part_4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part_4", "part_5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("part 1") <> -1 Then
                tempstring = tempstring.Replace("part 1", "part 2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part 2", "part 3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part 3", "part 4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part 4", "part 5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("part.1") <> -1 Then
                tempstring = tempstring.Replace("part.1", "part.2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part.2", "part.3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part.3", "part.4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("part.4", "part.5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("disk1") <> -1 Then
                tempstring = tempstring.Replace("disk1", "disk2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk2", "disk3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk3", "disk4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk4", "disk5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("disk_1") <> -1 Then
                tempstring = tempstring.Replace("disk_1", "disk_2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk_2", "disk_3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk_3", "disk_4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk_4", "disk_5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("disk 1") <> -1 Then
                tempstring = tempstring.Replace("disk 1", "disk 2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk 2", "disk 3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk 3", "disk 4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk 4", "disk 5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("disk.1") <> -1 Then
                tempstring = tempstring.Replace("disk.1", "disk.2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk.2", "disk.3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk.3", "disk.4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("disk.4", "disk.5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("pt1") <> -1 Then
                tempstring = tempstring.Replace("pt1", "pt2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt2", "pt3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt3", "pt4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt4", "pt5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("pt_1") <> -1 Then
                tempstring = tempstring.Replace("pt_1", "pt_2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt_2", "pt_3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt_3", "pt_4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt_4", "pt_5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("pt 1") <> -1 Then
                tempstring = tempstring.Replace("pt 1", "pt 2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt 2", "pt 3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt 3", "pt 4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt 4", "pt 5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            If tempstring.IndexOf("pt.1") <> -1 Then
                tempstring = tempstring.Replace("pt.1", "pt.2")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt.2", "pt.3")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt.3", "pt.4")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
                tempstring = tempstring.Replace("pt.4", "pt.5")
                If IO.File.Exists(tempstring) Then
                    playlist.Add(tempstring)
                End If
            End If
            Return playlist
        Catch

        Finally

        End Try
        Return "Error"
    End Function
    Public Shared Function cleanruntime(ByVal runtime As String)
        Try
            Dim temptime As String = runtime
            Dim totalmins As Integer = -1
            If runtime.ToLower.IndexOf("min") <> -1 Then
                Dim tempstring As String = runtime
                Dim tempint As Integer
                Dim newmins As String
                tempint = runtime.ToLower.IndexOf("min")
                newmins = runtime.Substring(0, tempint)
                newmins = Trim(newmins)
                If Not IsNumeric(newmins) Then
                    Dim guess As String = ""
                    For f = 0 To newmins.Length - 1
                        If IsNumeric(newmins.Substring(f, 1)) Then
                            guess = guess & newmins.Substring(f, 1)
                        End If
                    Next
                    If IsNumeric(guess) Then
                        totalmins = Convert.ToInt32(guess)
                    End If
                End If
            ElseIf runtime.ToLower.IndexOf("h") <> -1 Or runtime.ToLower.IndexOf("mn") <> -1 Then
                Try
                    Dim tempint As Integer = 0
                    '1h 24mn 48s 546ms
                    Dim hours As Integer = 0
                    Dim minutes As Integer = 0
                    Dim tempstring2 As String = temptime
                    tempint = tempstring2.IndexOf("h")
                    If tempint <> -1 Then
                        hours = Convert.ToInt32(tempstring2.Substring(0, tempint))
                        tempstring2 = tempstring2.Substring(tempint + 1, tempstring2.Length - (tempint + 1))
                        tempstring2 = Trim(tempstring2)
                    End If
                    tempint = tempstring2.IndexOf("mn")
                    If tempint <> -1 Then
                        minutes = Convert.ToInt32(tempstring2.Substring(0, tempint))
                    End If
                    If hours <> 0 Then
                        hours = hours * 60
                    End If
                    minutes = minutes + hours
                    totalmins = totalmins + minutes
                Catch
                End Try
            End If

            If totalmins <> -1 Then
                runtime = totalmins.ToString
            End If



            Return runtime
        Catch
        Finally

        End Try
        Return "Error"
    End Function
    Public Shared Function FindAllFolders(ByVal SourcePaths As List(Of String)) As List(Of String)
        Dim intCounter As Integer = 0
        Dim lstStringFolders As New List(Of String)
        Dim strSubFolders As String()

        For Each SourceFolder In SourcePaths
            lstStringFolders.Add(SourceFolder)
        Next
        Do Until intCounter = lstStringFolders.Count
            Try
                strSubFolders = System.IO.Directory.GetDirectories(lstStringFolders.Item(intCounter))
                lstStringFolders.AddRange(strSubFolders)
                intCounter += 1
            Catch ex As Exception
                intCounter += 1
            End Try
        Loop
        'sorts the folders so that related folders (parent/child) are together
        lstStringFolders.Sort()
        'Dim strFolder As String
        'Dim SourcePathsCounter As Integer = SourcePaths.Count
        'Dim n As Integer = 0
        'Do Until n = SourcePathsCounter
        '    For Each Folder In lstStringFolders
        '        If Folder = SourcePaths(n) Then
        '            lstStringFolders.Remove(Folder)
        '            n += 1
        '            Exit For
        '        End If
        '    Next
        'Loop
        Return lstStringFolders
    End Function


    Public Shared Function SaveText(ByVal text As String, ByVal path As String) As Boolean

        Try
            Dim file As IO.StreamWriter = IO.File.CreateText(path)
            Try
                file.Write(text, False, Encoding.UTF8)
                file.Close()
                Return True
            Catch ex As Exception
                file.Close()
                Try
                    IO.File.Delete(path)
                Catch
                End Try
                Return False
            End Try
        Catch ex As Exception
        Finally

        End Try
        Return False
    End Function

    Public Shared Function DeleteFile(ByVal path As String) As Boolean

        Try
            If IO.File.Exists(path) Then
                IO.File.Delete(path)
            End If
            Return True
        Catch
            Return False
        Finally

        End Try
    End Function

    Public Shared Function LoadTextLines(ByVal path As String) As List(Of String)

        Dim listoflines As New List(Of String)
        Try
            If Not IO.File.Exists(path) Then
                listoflines.Add("nofile")
                Return listoflines
            Else
                Dim lines As IO.StreamReader = IO.File.OpenText(path)
                Dim line As String
                Do
                    line = lines.ReadLine
                    If Not line Is Nothing Then
                        listoflines.Add(line)
                    Else
                        Exit Do
                    End If
                Loop Until line = Nothing
                Return listoflines
            End If
        Catch
            If listoflines.Count > 0 Then
                Return listoflines
            Else
                listoflines.Add("Error")
                Return listoflines
            End If
        Finally

        End Try
    End Function

    Public Shared Function LoadFullText(ByVal path As String) As String

        Dim text As String = String.Empty
        Try
            If Not IO.File.Exists(path) Then
                text = "nofile"
                Return text
            Else
                Dim lines As IO.StreamReader = IO.File.OpenText(path)
                text = lines.ReadToEnd
                Return text
            End If
        Catch
            If text Is Nothing Then
                text = "error"
            End If
            If text.Length = 0 Then
                text = "error"
            End If
            Return text
        Finally
        End Try
    End Function

    Public Shared Function SaveXml(ByVal path As String, ByVal xmldoc As XmlDocument) As Boolean

        Try
            Dim output As New XmlTextWriter(path, System.Text.Encoding.UTF8)
            Try
                output.Formatting = Formatting.Indented
                xmldoc.WriteTo(output)
                output.Close()
                Return True
            Catch ex As Exception
                Try
                    output.Close()
                    Try
                        IO.File.Delete(path)
                    Catch
                    End Try
                Catch
                End Try
                Return False
            End Try
        Catch ex As Exception
            Return False
        Finally

        End Try
    End Function

    Public Shared Function SaveImage(ByVal image As Bitmap, ByVal path As String) As Boolean

        Try
            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg)
            Return True
        Catch ex As Exception
            Return False
        Finally

        End Try
    End Function

    Public Shared Function ResizeImage(ByVal bmp As Bitmap, ByVal width As Integer, ByVal height As Integer) As Bitmap
        Dim bm_source As New Bitmap(bmp)
        Dim bm_dest As New Bitmap(width, height)
        Dim gr As Graphics = Graphics.FromImage(bm_dest)
        gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
        gr.DrawImage(bm_source, 0, 0, width, height)
        Dim tempbitmap As Bitmap = bm_dest
        Return tempbitmap
    End Function

    Public Shared Function LoadBitmap(ByVal path As String) As Bitmap

        Try
            Dim bmp As New Bitmap(path)
            Return bmp
        Catch
            Return Nothing
        Finally

        End Try
    End Function

    Public Shared Sub DownloadFile(ByVal URL As String, ByVal Path As String)
        Try
            Dim buffer(4000000) As Byte
            Dim size As Integer = 0
            Dim bytesRead As Integer = 0
            Dim req As HttpWebRequest = WebRequest.Create(URL)
            Dim res As HttpWebResponse = req.GetResponse()
            Dim contents As Stream = res.GetResponseStream()
            Dim bytesToRead As Integer = CInt(buffer.Length)
            While bytesToRead > 0
                size = contents.Read(buffer, bytesRead, bytesToRead)
                If size = 0 Then Exit While
                bytesToRead -= size
                bytesRead += size
            End While

            Dim fstrm As New FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write)
            fstrm.Write(buffer, 0, bytesRead)
            contents.Close()
            fstrm.Close()
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Function GetResourceStream(ByVal resfile As String) As Stream
        Dim asm As Assembly = Assembly.GetExecutingAssembly
        Return asm.GetManifestResourceStream(resfile)
    End Function

    Public Shared Function EnsureFolderExists(ByVal Path As String) As Boolean
        Dim Parts As String()

        Parts = Split(Path, "\")

        If Left(Path, 2) = "\\" Then 'Network path
            Dim Drive As String = Parts(0) & "\" & Parts(1) & "\" & Parts(2) & "\" & Parts(3)

            Dim CurrentPath As String = Drive
            For I = 4 To Parts.GetUpperBound(0)
                CurrentPath = IO.Path.Combine(CurrentPath, Parts(I))

                If Not IO.Directory.Exists(CurrentPath) Then
                    IO.Directory.CreateDirectory(CurrentPath)
                End If
            Next
        Else 'Local
            Dim Drive As String = Parts(0)

            Dim CurrentPath As String = Drive
            For I = 1 To Parts.GetUpperBound(0)
                CurrentPath = IO.Path.Combine(CurrentPath, Parts(I))

                If Not IO.Directory.Exists(CurrentPath) Then
                    IO.Directory.CreateDirectory(CurrentPath)
                End If
            Next
        End If

        Return True
    End Function
End Class
