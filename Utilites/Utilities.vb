Imports System.Threading
Imports System.Net
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Drawing
Imports System.Text
Imports System.Security.Cryptography
Imports Microsoft.Win32


Public Class Utilities
    Public Shared VideoExtensions As String() = {".avi", ".mkv", ".xvid", ".divx", ".mpg", ".mpeg", ".mov",
                                                 ".rm", ".3gp", ".m4v", ".wmv", ".asf", ".mp4", ".nrg", ".iso",
                                                 ".rmvb", ".ogm", ".bin", ".ts", ".vob", ".m2ts", ".rar", ".flv",
                                                 ".dvr-ms", ".img", ".strm", ".ssif", ".mk3d", ".webm", ".bdmv", 
                                                 ".disc", ".m3u", ".m3u8", ".vro", "vr_mangr.ifo", "video_ts.ifo" }  'video_ts.ifo must be last in list
                                                        'added m3u and m3u8 on request.  Not sure this is actually of any use or will cause issues.
    'file ext for trailers
    Public Shared TrailerExtensions As String() = {".avi", ".mkv", ".xvid", ".divx", ".mpg", ".mpeg", ".mov",
                                                 ".rm", ".3gp", ".m4v", ".wmv", ".asf", ".mp4", ".rmvb", ".ogm",
                                                 ".ts", ".m2ts", ".flv", ".webm", ".vob" }

    'movie Fanart.Tv file list
    Public Shared ReadOnly fanarttvfiles As String() = {"clearart.png", "logo.png", "disc.png", "banner.jpg",
                                                        "landscape.jpg", "fanart.jpg", "poster.jpg"}

    'files that support main movie file, ie. art, nfo.
    Public Shared ReadOnly acceptedAnciliaryExts() As String = {".nfo", ".tbn", "-fanart.jpg", "-poster.jpg", "-banner.jpg",
                                                                "-clearart.png", "-logo.png", "-banner.jpg", "-landscape.jpg", "-disc.png",
                                                                "-folder.jpg"}

    'filename entries depicting trailers
    Public Shared ReadOnly acceptedtrailernaming() As String = {"-trailer", "-theme"}

    'subtitle extensions for check of multi-part subtitle files
    Public Shared ReadOnly acceptedsubextn() As String = {".sub", ".srt", ".smi", ".idx", ".ass", ".ssa"}

    'common separators in filenames ie. dash, underscore, fullstop, and space
    Public Shared ReadOnly cleanSeparators As String = "-_. "

    'common separators in filenames ie. dash, underscore, fullstop, and space
    Public Shared ReadOnly separators() As String = {"", "-", "_", ".", " "}

    'keywords commonly used to indicate stacked files
    Public Shared ReadOnly cleanMultipart() As String = {"part", "pt", "cd", "dvd", "disk", "disc"}

    'keywords that are commonly cleaned from filenames
    Public Shared ReadOnly cleanTagsList() As String = {"ac3", "dts", "divx", "xvid", "x264", "dvdrip", "bluray", "dvdscr",
                                                "screener", "fullscreen", "widescreen", "telesync", "telecine",
                                                "480", "576", "720", "1024", "1080"}

    'keywords that are more than a single word
    Public Shared ReadOnly cleanTagsList_MultiWord() As String = {"special edition", "directors cut", "dir cut", "director's cut"}

    'must have a separator character prefix so they do not get confused with standard text.
    Public Shared ReadOnly cleanTagsList_SepPrefix() As String = {"scr", "ts", "fs", "ws", "r5"}

    Shared Property LastRootPath As String = ""


    Private Declare Function GetDiskFreeSpaceEx _
Lib "kernel32" _
Alias "GetDiskFreeSpaceExA" _
(ByVal lpDirectoryName As String, _
ByRef lpFreeBytesAvailableToCaller As Long, _
ByRef lpTotalNumberOfBytes As Long, _
ByRef lpTotalNumberOfFreeBytes As Long) As Long

    Public Shared ReadOnly defaultculture As System.Globalization.CultureInfo = New Globalization.CultureInfo("en-us")
    'Public Shared ReadOnly trueculture As System.Globalization.CultureInfo = Thread.CurrentThread.CurrentCulture
    Public Shared Property DefaultPosterPath As String
    Public Shared Property DefaultBannerPath As String
    Public Shared Property DefaultFanartPath As String
    Public Shared Property DefaultTvPosterPath As String
    Public Shared Property DefaultTvBannerPath As String
    Public Shared Property DefaultTvFanartPath As String
    Public Shared Property DefaultPreFrodoBannerPath As String
    Public Shared Property DefaultOfflineArtPath As String
    Public Shared Property DefaultActorPath As String
    Public Shared Property DefaultScreenShotPath As String
    Public Shared Property CacheFolderPath As String
    Public Shared Property PosterCachePath As String
    Public Shared Property MissingPath As String
    Public Shared Property SeriesXmlPath As String

    Public Shared Property ignoreParts      As Boolean = False
    Public Shared Property userCleanTags    As String = "UNRATED|LIMITED|YIFY|3D|SBS"
    Public Shared Property RARsize          As Integer
    Private Shared _tmdbapi                 As String
    Private Shared _tvdbapi                 As String
    Private Shared _fanarttvapi             As String
    Private Shared _ApplicationPath         As String
    Private Shared _LanguageLibrary         As New List(Of langlib)
    Private Shared _mcproxy                 As New List(Of String)

    Public Shared Function GetFrameworkVersions() As List(Of String)
        Dim installedFrameworks As New List(Of String)
        'send key & value to test function - it will return true if it exists - installedFrameworks contains a list of all found NET versions
        If (TestKey("Software\Microsoft\.NETFramework\Policy\v1.0", "3705")) Then installedFrameworks.Add("1.0")
        If (TestKey("Software\Microsoft\NET Framework Setup\NDP\v1.1.4322", "Install")) Then installedFrameworks.Add("1.1")
        If (TestKey("Software\Microsoft\NET Framework Setup\NDP\v2.0.50727", "Install")) Then installedFrameworks.Add("2.0")
        If (TestKey("Software\Microsoft\NET Framework Setup\NDP\v3.0\Setup", "InstallSuccess")) Then installedFrameworks.Add("3.0")
        If (TestKey("Software\Microsoft\NET Framework Setup\NDP\v3.5", "Install")) Then installedFrameworks.Add("3.5")
        If (TestKey("Software\Microsoft\NET Framework Setup\NDP\v4\Client", "Install")) Then installedFrameworks.Add("4.0 Client")
        If (TestKey("Software\Microsoft\NET Framework Setup\NDP\v4\Full", "Install")) Then installedFrameworks.Add("4.0 Full")
        Return installedFrameworks
    End Function


    '''  Media Companions own TheMovieDb API Key.
    '''  Signed for under account belonging to vbat99@gmail.com
    '''  Note:  API Key (v3 auth)  ebe93edbc30735a0e702eb883fc855be
    '''  API Read Access Token (v4 auth)   eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJlYmU5M2VkYmMzMDczNWEwZTcwMmViODgzZmM4NTViZSIsInN1YiI6IjRmMjlkODI4NzYwZWUzNTlhMTAwMzNiYyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.wVGTD-EO8JDvcmh0tTC-CEMkQ4q1PwwgXTjSxjGPR_U
    '''  
    '''  API should be used for Read-Only access of TheMovieDB site.
    '''  

    Public Shared Property TMDBAPI As String
        
        Get
            Return If(String.IsNullOrEmpty(_tmdbapi), "ebe93edbc30735a0e702eb883fc855be", _tmdbapi)
        End Get
        Set(value As String)
            _tmdbapi = value
        End Set
    End Property

    Public Shared Property TVDBAPI As String
        Get
            Return If(String.IsNullOrEmpty(_tvdbapi), "A7613F5C1482A540", _tvdbapi)
        End Get
        Set(value As String)
            _tvdbapi = value
        End Set
    End Property
    
    
    '''  Media Companions own FanartTv Project API Key. 
    '''  Signed for under account belonging to vbat99@gmail.com
    '''  Note:  API Key V3   28405cbbd8cf53dd2f01739ccb0066a3
    '''  
    '''  API is for Media Companion Read-Only access of Fanart.TV.
    '''

    Public Shared Property FANARTTVAPI As String
        Get
            Return If(String.IsNullOrEmpty(_fanarttvapi), "28405cbbd8cf53dd2f01739ccb0066a3", _fanarttvapi)
        End Get
        Set(value As String)
            _fanarttvapi = value
        End Set
    End Property

    Public Shared Function TestKey(key As String, value As String)
        Dim regKey As Microsoft.Win32.RegistryKey = Registry.LocalMachine.OpenSubKey(key, False)
        If regKey Is Nothing Then 'Key Not Found
            Return False
        Else
            If regKey.GetValue(value) Is Nothing Then
                Return False  'Key Found, Value Not Found
            End If
            Return True 'Key & Value Found
        End If
    End Function

    Public Shared Property applicationPath As String
        Get
            Return _ApplicationPath
        End Get
        Set(value As String)
            _ApplicationPath            = value
            DefaultPosterPath           = Path.Combine(_ApplicationPath, "Resources\default_poster.jpg")
            DefaultBannerPath           = Path.Combine(_ApplicationPath, "Resources\default_banner.jpg")
            DefaultFanartPath           = Path.Combine(_ApplicationPath, "Resources\default_fanart.jpg")
            DefaultTvPosterPath         = Path.Combine(_ApplicationPath, "Resources\default_tvposter.jpg")
            DefaultTvBannerPath         = Path.Combine(_ApplicationPath, "Resources\default_tvbanner.jpg")
            DefaultTvFanartPath         = Path.Combine(_ApplicationPath, "Resources\default_tvfanart.jpg")
            DefaultPreFrodoBannerPath   = Path.Combine(_ApplicationPath, "Resources\prefrodo_banner.jpg")
            DefaultOfflineArtPath       = Path.Combine(_ApplicationPath, "Resources\default_offline.jpg")
            DefaultActorPath            = Path.Combine(_ApplicationPath, "Resources\default_actor.jpg")
            DefaultScreenShotPath       = Path.Combine(_ApplicationPath, "Resources\default_offline.jpg")
            CacheFolderPath             = Path.Combine(_ApplicationPath, "cache\")
            PosterCachePath             = Path.Combine(_ApplicationPath, "Settings\postercache\")
            MissingPath                 = Path.Combine(_ApplicationPath, "missing\")
            SeriesXmlPath               = Path.Combine(_ApplicationPath, "SeriesXml\")
            DownloadCache.CacheFolder   = CacheFolderPath
        End Set
    End Property

    Public Shared Property MCProxy As List(Of String)
        Get
            Return _mcproxy
        End Get
        Set(value As List(Of String))
            _mcproxy = value
        End Set
    End Property

    Public Shared ReadOnly Property MyProxy As WebProxy
        Get
            Try
                If MCProxy.Item(0).ToLower = "false" Then
                    Return Nothing
                ElseIf MCProxy.Item(0).ToLower = "system" Then
                    Dim _myProxy As Webproxy = WebRequest.GetSystemWebProxy()
                    _myProxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials()
                    _myProxy.BypassProxyOnLocal = True
                    Return _myProxy
                Else
                    Dim _myProxy As New WebProxy(MCProxy.Item(1), Convert.ToInt32(MCProxy.Item(2)))
                    _myProxy.Credentials = New NetworkCredential(MCProxy.Item(3), MCProxy.item(4))
                    _myProxy.BypassProxyOnLocal = True
                    Return _myProxy
                End If
            Catch
                Return Nothing
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property languagelibrary As List(Of langlib)
        Get
            If IsNothing(_LanguageLibrary) OrElse _LanguageLibrary.Count < 1 Then
                langlibraryload()
            End If
            Return _LanguageLibrary 
        End Get
    End Property

    Public Shared tvScraperLog As String = ""

    Private Shared Sub langlibraryload()
        Dim libraryfile As String = Path.Combine(_ApplicationPath, "Assets\LangList.csv")
        Dim libline As New langlib
        Dim lst As New List(Of String)
        lst = LoadTextLines(libraryfile)
        For Each item In lst
            Dim splt() As String = item.Split(",")
            libline.language    = splt(0)
            libline.lang2       = splt(1)
            libline.lang3       = splt(2)
            _LanguageLibrary.Add(libline)
        Next
    End Sub

    Public Shared Sub NfoNotepadDisplay(ByVal nfopath As String, Optional ByVal altnfoeditor As String = "")
        Try
            Dim npapp As String = "notepad"    'Tweaked to use Notepad++ if installed.
            Dim np As String = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\notepad++\notepad++.exe"
            If File.Exists(np) Then npapp = "notepad++"
            If altnfoeditor <> "" AndAlso File.Exists(altnfoeditor) Then npapp = altnfoeditor 
            Dim thePSI As New System.Diagnostics.ProcessStartInfo(npapp)
            thePSI.Arguments = """" & nfopath & """"
            System.Diagnostics.Process.Start(thePSI)
        Catch ex As Exception
            MsgBox("Unable to open File")
        End Try
    End Sub

    Public Shared Function TitleCase(words As String)
        Return Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words)
    End Function

    Public Shared Function UrlIsValid(ByVal url As String) As Boolean
        
        If IsNothing(url) or url = "" Then return False

        If url.IndexOf(".youtube.com") > -1 Then Return True

        Dim is_valid As Boolean = False
        If url.ToLower().StartsWith("www.") Then url = _
            "http://" & url
        If Not url.ToLower().StartsWith("http") Then Return False
        Dim web_response As HttpWebResponse = Nothing
        Try
            Dim web_request As HttpWebRequest = HttpWebRequest.Create(url)
            web_request.Proxy = Utilities.MyProxy
            web_request.Timeout = 10000
            web_response = DirectCast(web_request.GetResponse(), HttpWebResponse)
            Return True
        Catch ex As Exception
            Return False
        Finally
            If Not (web_response Is Nothing) Then _
                web_response.Close()
        End Try
    End Function
    
    Public Shared Function IsNumeric(ByVal TestString As String) As Boolean
        Dim SeasonInt As Integer
        If Integer.TryParse(TestString, SeasonInt) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function PadNumber(ByVal input As String, ByVal Length As Integer) As String
        Do Until input.Length >= Length
            input = "0" & input
        Loop
        Return input
    End Function

    Public Shared Function CreateScrnShotToCache(ByVal FullPathAndFilename As String, ByVal SavePath As String, ByVal sec As Integer, Optional ByVal count As Integer = 1, Optional ByVal gap As Integer = 2) As String
        Dim cachename As String = GetCRC32(SavePath)' & ".jpg"
        Dim Returnpath As String = ""
        For i = 0 To count-1
            Dim cachename2 As String = cachename & "-" & i.tostring & ".jpg"
            sec = sec + (i * gap)
            Dim CachePath As String = Path.Combine(CacheFolderPath, cachename2)
            If CreateScreenShot(FullPathAndFilename, CachePath, sec, True) Then
                If i = 0 Then Returnpath = cachepath
            End If
        Next
        Return Returnpath  '""
    End Function

    Public Shared Function CreateScreenShot(ByVal FullPathAndFilename As String, ByVal SavePath As String, ByVal sec As Integer, Optional ByVal Overwrite As Boolean = False) As Boolean
        If Not File.Exists(SavePath) Or Overwrite Then
            Try
                File.Delete(SavePath)
            Catch
                Return False
            End Try
            If File.Exists(FullPathAndFilename) Then
                Dim myProcess As Process = New Process
                Try
                    Dim seconds As Integer = sec
                    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    myProcess.StartInfo.CreateNoWindow = False
                    myProcess.StartInfo.FileName = Utilities.applicationPath & "\Assets\ffmpeg.exe"
                    Dim proc_arguments As String = "-ss " & seconds.ToString & " -i """ & FullPathAndFilename & """ -vframes:v 1 -an " & """" & SavePath & """"
                    'Dim proc_arguments As String = "-y -i """ & FullPathAndFilename & """ -f mjpeg -ss " & seconds.ToString & " -vframes 1 -an " & """" & SavePath & """"
                    myProcess.StartInfo.Arguments = proc_arguments
                    myProcess.Start()
                    myProcess.WaitForExit()
                    If File.Exists(SavePath) Then Return True
                Catch ex As Exception
                    Throw
                Finally
                    myProcess.Close()
                End Try
            End If
        End If
        Return False
    End Function

    Public Shared Function DownloadTextFiles(ByVal StartURL As String, Optional ByVal ForceDownload As Boolean = False) As String
        Dim data As String = ""
        Dim returnState As Boolean = DownloadCache.DownloadFileAndCache(StartURL, "", ForceDownload,,, strValue:=data)
        Return data
    End Function

    Public Shared Function GetStdAspectRatio(ByVal Ratio As String) As String

        If String.IsNullOrEmpty(Ratio) Then Return ""
        If Ratio.IndexOf(":"c) > -1 Then Ratio = Ratio.Substring(0, Ratio.IndexOf(":"))
        
        Return Ratio
    End Function

    Public Shared Function FixIntlAspectRatio(ByVal Ratio As String) As String
        If Ratio.Contains(",") Then
            Dim pos As Integer = Ratio.IndexOf(",")
            If pos > 2 Then
                If Not Ratio.Contains(".") And IsNumeric(Ratio.Substring(1,1)) Then
                    Ratio = Ratio.Replace(",", "")
                    Ratio = Ratio.Insert(1, ",")
                    If Ratio.Length > 4 Then Ratio = Ratio.Substring(0,4)
                End If
            End If
        End If
        If Ratio.Length > 4 Then Ratio = Ratio.Substring(0,4)
        Return Ratio
    End Function
    
    Public Shared Function GetCodecCommonName(ByVal codec As String) As String
        If codec.ToLower.Contains("div") or codec.ToLower.Contains("dx50")Then codec = "divx"
        Return codec
    End Function

    Public Shared Function GetCRC32(ByVal sFileName As String) As String
        Dim oCRC As New CRC32
        Dim oEnc As System.Text.UTF7Encoding = New System.Text.UTF7Encoding()
        Return (oCRC.GetCrc32(New IO.MemoryStream(oEnc.GetBytes(sFileName))))
    End Function

    Public Shared Function GetLastFolder(ByVal FullPath As String) As String

        If Right(FullPath, 1) <> Path.DirectorySeparatorChar Then
            FullPath = FullPath.Replace(Path.GetFileName(FullPath), "")
        End If

        Dim foldername As String = ""
        Dim paths() As String
        paths = FullPath.Split(Path.DirectorySeparatorChar)
        For g = UBound(paths) To 0 Step -1
            If paths(g).ToLower.IndexOf("video_ts") = -1 And paths(g).ToLower.IndexOf("bdmv") = -1 And paths(g) <> "" Then
                foldername = paths(g)
                Return foldername
            End If
        Next

        Return ""
    End Function

    Public Shared Function GetDvdLargestVobSet(ByVal Filename As String) As String
        Dim returnfilename As String = Filename
        Try
            Dim vobsetName As String = ""
            Dim vobsetcount As Integer = 0
            Dim vobPath As String = Path.GetDirectoryName(Filename)
            Dim di As New DirectoryInfo(vobPath)
            Dim aryFi As FileInfo() = di.GetFiles("vts*.vob")
            For each fi In aryFi
                Dim vset As String = "vts_" & fi.Name.Substring(4,2) & "*.vob"
                Dim grp As FileInfo() = di.GetFiles(vset)
                If grp.Count > vobsetcount Then
                    vobsetcount = grp.Count
                    vobsetName = vset
                End If
            Next
            If vobsetName <> "" Then
                Dim fileifo As FileInfo() = di.GetFiles(vobsetName.Replace("vob", "ifo"))
                If fileifo.Count = 1 Then returnfilename = fileifo(0).FullName
            End If
        Catch
        End Try
        Return returnfilename
    End Function

    Public Shared Function isMultiPartMedia(ByRef workingFileName As String, ByVal nameOnly As Boolean, Optional ByRef isFirstPart As Boolean = True, _
                                            Optional ByRef stackType As String = "", Optional ByRef nextPart As String = "") _
                                        As Boolean
        Dim returnCode As Boolean = False
        If nameOnly OrElse File.Exists(workingFileName) Then
            Dim pathOnly As String = Path.GetDirectoryName(workingFileName) & "\"
            Dim filename As String = Path.GetFileNameWithoutExtension(workingFileName)
            Dim stackName As String = filename.ToLower
            Dim extension As String = Path.GetExtension(workingFileName).ToLower
            Dim M As Match
            If extension = ".rar" AndAlso FileLen(workingFileName) > (RARsize * 1048576) Then
                'process RAR stack that contains digits in the style of ".part1" to ".part0001"
                M = Regex.Match(stackName, "\.part([0]{0,3}[0-9]+)$")
            Else
                'process a typical multi‑part, ending in digits or a single letter
                M = Regex.Match(stackName, "[" & cleanSeparators & "](" & Join(cleanMultipart, "|") & ")([" & cleanSeparators & "]?)([0-9a-z]+)$")
                'M = Regex.Match(stackName, "(" & Join(cleanMultipart, "|") & ")([" & cleanSeparators & "]?)([0-9a-z]+)$")
                If M.Success = False Then
                    'finally, process a multi‑part that may be designated by a single letter
                    M = Regex.Match(stackName, "([a-z])$")
                End If
            End If
            If M.Success = True Then
                'if there is a possible stack, confirm by testing for another part,
                '   ‑ either the first part '1' or 'a', or the next sequential part.
                If ignoreParts AndAlso (M.Groups(1).Value = "part" Or M.Groups(1).Value = "pt") Then
                    'don't modify stack name
                ElseIf M.Groups(3).Value.Length > 2 Then
                    'not a stacked file
                Else
                    Dim first As Boolean = False
                    Dim grpPartNo As Group = M.Groups(M.Groups.Count - 1)   'get the number or letter at the end of filename
                    Dim partNo As String = grpPartNo.Value
                    Dim i As Integer
                    If Integer.TryParse(partNo, i) Then
                        nextPart = (i + 1).ToString.PadLeft(grpPartNo.Length, "0")
                        If i = 1 Then
                            first = True
                            partNo = nextPart
                        Else
                            partNo = "1".PadLeft(grpPartNo.Length, "0")
                        End If
                    Else
                        Right(partNo, 1)
                        nextPart = Chr(Asc(partNo) + 1)
                        If partNo = "a" Then
                            first = True
                            partNo = nextPart
                        Else
                            partNo = "a"
                        End If
                    End If
                    Mid(workingFileName, pathOnly.Length + grpPartNo.Index + 1, grpPartNo.Length) = partNo
                    If nameOnly OrElse File.Exists(workingFileName) Then
                        returnCode = True
                        stackName = filename.Substring(0, M.Index)
                        isFirstPart = first
                        filename = Regex.Replace(stackName, "[" & cleanSeparators & "]+$", "")
                        stackType = stackName.Substring(filename.Length) & M.Value.Replace(grpPartNo.Value, "")
                    End If
                End If
            End If
            workingFileName = filename
        End If
        Return returnCode
    End Function

    Public Shared Function GetStackName(ByVal fullFileName As String) As String
        Dim stackName As String = fullFileName
        Dim isStack As Boolean = isMultiPartMedia(stackName, True)
        Return stackName
    End Function

    Public Shared Function findFileOfType(ByRef fullPath As String, ByVal fileType As String, Optional ByVal basicsave As Boolean = False, Optional ByVal fanartjpg As Boolean = False, Optional ByVal posterjpg As Boolean = False) As Boolean
        Dim pathOnly As String = Path.GetDirectoryName(fullPath) & "\"
        Dim returnCode As Boolean = False
        Dim typeOfFile As New List(Of String)
        typeOfFile.Add(pathOnly & GetStackName(fullPath) & fileType)                             'multi-part string removed
        
        If basicsave Then
            typeOfFile.Add(pathOnly & Regex.Replace("movie" & fileType, "movie-", ""))              'special case where using folder-per-movie
        End If
        If fanartjpg or fileType.Contains("fanart") Then
            typeOfFile.Add(pathOnly & "fanart.jpg")
        End If
        If posterjpg Then
            typeOfFile.Add(pathOnly & "poster.jpg")
        End If
        typeOfFile.Add(pathOnly & Path.GetFileNameWithoutExtension(fullPath) & fileType)      'match filename sans extension
        For Each filet As String In typeOfFile
            If File.Exists(filet) Then
                returnCode = True
                fullPath = filet
                'Exit For
            End If
        Next
        Return returnCode
    End Function

    Public Shared Function GetFileSize(ByVal filePath As String) As Long
        If File.Exists(filePath) Then
            Dim file As New FileInfo(filePath)
            Return file.Length
        End If
        Return 0
    End Function

    Public Shared Function GetFreeSpace(ByVal Drive As String) As Long
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

    Public Shared Function GetFolderSize(ByVal DirPath As String, Optional IncludeSubFolders as Boolean = True) As Long
      Dim size As Long          = 0
      Dim di   As DirectoryInfo = New DirectoryInfo(DirPath)
      Try
         For Each fi In di.GetFiles()
            size += fi.Length
         Next
         If IncludeSubFolders then
            For Each sub_di In di.GetDirectories()
               size += GetFolderSize(sub_di.FullName)
            Next
         End if
         Return size
      Catch
         Return -1
      End Try
    End Function

    Public Shared Function testForFileByName(ByVal targetMovieFile As String, ByVal fileType As String) As Boolean
        Dim aFileExists As Boolean = False
        Dim fileTypes As New ArrayList
        fileTypes.Add(fileType)
        fileTypes.AddRange(acceptedAnciliaryExts)
        fileTypes.AddRange(acceptedsubextn)
        For Each item As String In fileTypes 'issue - if part found mc doesn't use part for fanart & tbn so this test is not right yet
            If File.Exists(targetMovieFile & item) Then
                aFileExists = True
                Exit For
            End If
        Next
        Return aFileExists
    End Function

    Public Shared Function testForsubtitleByExtension(ByVal subFilename As String) As String
        Dim aFileExists As String = ""
        Dim fileTypes As New ArrayList
        fileTypes.AddRange(acceptedsubextn)
        For Each item As String In fileTypes 'issue - if part found mc doesn't use part for fanart & tbn so this test is not right yet
            If File.Exists(subFilename & item) Then
                aFileExists = item
                Exit For
            End If
        Next
        Return aFileExists
    End Function

    Public Shared Function ListSubtitleFilesExtensions(ByVal subFilename As String) As List(Of String)
        Dim aFileExists As New List(Of String)
        Dim fileTypes As New ArrayList
        fileTypes.AddRange(acceptedsubextn)
        For Each item As String In fileTypes
            If File.Exists(subFilename & item) Then
                aFileExists.Add(item)
            End If
        Next
        Return aFileExists
    End Function

    Public Shared Function GetbdMainStream(ByVal path) As String
        If path.ToString.Contains(".nfo") Then Return path
        Dim di As New DirectoryInfo(path.Replace("index.bdmv","STREAM\"))
        Dim fi As FileInfo() = di.GetFiles("*.m2ts")
        Dim sort =  fi.OrderByDescending(Function(f) f.Length)
        Return sort(0).FullName
    End Function

    Public Shared Function GetFileName(ByVal filepath As String, Optional strict As Boolean = True, Optional VideoExtn As String = Nothing) As String
        Dim tempstring As String
        Dim tempfilename As String = filepath
        Dim actualpathandfilename As String = ""

        If String.IsNullOrEmpty(filepath) Then Return Nothing

        If File.Exists(tempfilename.Replace(Path.GetFileName(tempfilename), "VIDEO_TS.IFO")) Then
            actualpathandfilename = tempfilename.Replace(Path.GetFileName(tempfilename), "VIDEO_TS.IFO")
        End If
        If File.Exists(tempfilename.Replace(Path.GetFileName(tempfilename), "VR_MANGR.IFO")) Then
            actualpathandfilename = tempfilename.Replace(Path.GetFileName(tempfilename), "VR_MANGR.IFO")
        End If
        If filepath.ToLower.Contains(".bdmv") Then
            Dim bdlargestfile As String = GetbdMainStream(filepath)
            actualpathandfilename = bdlargestfile
        End If

        If actualpathandfilename = "" AndAlso Not String.IsNullOrEmpty(VideoExtn) Then
            Dim tempname As String = tempfilename.Replace(GetExtension(tempfilename), VideoExtn.Trim("."))
            If File.Exists(tempname) Then actualpathandfilename = tempname
        End If

        If actualpathandfilename = "" Then
            Dim tempname As String = tempfilename.Replace(Path.GetExtension(tempfilename), "")
            For Each extn In VideoExtensions
                If File.Exists(tempname & extn) Then
                    actualpathandfilename = tempname & extn
                    Exit For
                End If
            Next
        End If

        If actualpathandfilename = "" Then
            Dim possiblemovies As New List(Of String)
            Dim filenamewithoutextension As String = Path.GetFileNameWithoutExtension(filepath)
            Dim dirpath As String = tempfilename.Replace(Path.GetFileName(tempfilename), "")
            If Not Directory.Exists(dirpath) Then Return "none"
            Dim dir_info As New DirectoryInfo(dirpath)
            Dim pattern As String = "*.*" 
            If strict and Not filepath.Contains("movie.nfo") Then pattern = filenamewithoutextension & "*.*" ' & videoextn
            Dim fs_infos() As FileInfo = dir_info.GetFiles(pattern)
            For Each vidextn In VideoExtensions
                'Dim pattern As String = "*" & videoextn
                'If strict and Not path.Contains("movie.nfo") Then pattern = filenamewithoutextension & "*" & videoextn
                Try
                    'Dim fs_infos() As FileInfo = dir_info.GetFiles("*.*")
                    For Each fs_info As FileInfo In fs_infos
                        If fs_info.Extension <> vidextn Then Continue For
                        If vidextn = ".rar" Then
                            If fs_info.length < 8388608 Then Continue For  'If Rar file size less than 8MB, ignore it as probably subtitle file.
                        End If
                        tempstring = fs_info.FullName.ToLower
                        If tempstring.IndexOf("-trailer") = -1 And tempstring.IndexOf("-sample") = -1 And tempstring.IndexOf(".trailer") = -1 And tempstring.IndexOf(".sample") = -1 Then
                            possiblemovies.Add(fs_info.FullName)
                        End If
                    Next
                Catch
                End Try
            Next
            If possiblemovies.Count = 1 Then
                actualpathandfilename = possiblemovies(0)
            ElseIf possiblemovies.Count > 1 Then
                Dim success As Boolean = False
                Dim workingstring As String
                For Each multi In cleanMultipart
                    For Each sep In separators
                        For Each possiblemov In possiblemovies
                            workingstring = multi & sep & "1"
                            Dim workingtitle As String = possiblemov.ToLower
                            If workingtitle.IndexOf(workingstring) <> -1 Then
                                actualpathandfilename = possiblemov
                                success = True
                                Exit For
                            End If
                        Next
                        If success Then Exit For
                    Next
                    If success Then Exit For
                Next
            End If
        End If

        If actualpathandfilename = "" Then
            actualpathandfilename = "none"
        End If

        Return actualpathandfilename

        'Return "Error"
    End Function

    Public Shared Function GetTvEpExtension(ByVal epnfopath As String) As String
        Dim epfilename As String = RemoveFilenameExtension(epnfopath)
        For Each extn In VideoExtensions
            If File.Exists(epfilename & extn) Then
                Return extn
            End If
        Next
        Return "error"
    End Function

    Public Shared Function ValidMovieDir(ByVal PathToCheck As String) As Boolean
        Dim passed As Boolean = True
        Dim s As String = PathToCheck.ToLower
        Try
            If Strings.Right(s, 7) = "trailer" Then
                passed = False
            ElseIf Strings.Right(s, 10) = "thumbnails" Then
                passed = False
            ElseIf Strings.Right(s, 7) = ".actors" Then
                passed = False
            ElseIf Strings.Right(s, ".appledouble".Length) = ".appledouble" Then
                passed = False
            ElseIf Strings.Right(s, 9) = ".ds_store" Then
                passed = False
            ElseIf Strings.Right(s, 3) = ".tb" Then
                passed = False
            ElseIf Strings.Right(s, 8) = "(noscan)" Then
                passed = False
            ElseIf Strings.Right(s, 6) = "sample" Then
                passed = False
            ElseIf Strings.Right(s, 8) = "recycler" Then
                passed = False
            ElseIf s.Contains("$recycle.bin") Then
                passed = False
            ElseIf s.Contains(".recycle.bin") Then
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

    Public Shared Function NfoValidate(ByVal nfopath As String, Optional ByVal homemovie As Boolean = False)
        Dim tempstring As String
        Using filechck As IO.StreamReader = File.OpenText(nfopath)
            tempstring = filechck.ReadToEnd.ToLower
        End Using
        If tempstring = Nothing Then
            Return False
        End If
        If tempstring.IndexOf("<movie") <> -1 And tempstring.IndexOf("</movie>") <> -1 And tempstring.IndexOf("<title>") <> -1 And tempstring.IndexOf("</title>") <> -1 Then
            Return True
            Exit Function
        End If
        Return False
    End Function

    Public Shared Function DecodeDateTime(s As String, df As String) As String

        Dim YYYY As String = s.SubString( 0,4)
        Dim MM   As String = s.SubString( 4,2)
        Dim DD   As String = s.SubString( 6,2)
        Dim HH   As String = s.SubString( 8,2)
        Dim MIN  As String = s.SubString(10,2)
        Dim SS   As String = s.SubString(12,2)

        Dim x As String = df

        x = x.Replace("YYYY", YYYY)
        x = x.Replace("MM"  , MM  )
        x = x.Replace("DD"  , DD  )
        x = x.Replace("HH"  , HH  )
        x = x.Replace("MIN" , MIN )
        x = x.Replace("SS"  , SS  )

        Return x
    End Function

    Public Shared Function EnumerateFolders(ByVal RootPath As String, Optional ByVal MaxLevels As Long = 999) As List(Of String)
        Return EnumerateFolders(RootPath, MaxLevels, 0)
    End Function

    Private Shared Function EnumerateFolders(ByVal RootPath As String, ByVal MaxLevels As Long, ByVal Level As Long) As List(Of String)

        LastRootPath = RootPath

        Dim TempReturn As New List(Of String)
        Try
            If String.IsNullOrEmpty(RootPath) Then Return Nothing
            Dim ChildList
            Try
                ChildList = Directory.GetDirectories(RootPath)
            Catch ex As Exception' As UnauthorizedAccessException
                Return TempReturn
            End Try
            If Level > 0 Then
                TempReturn.Add(RootPath)
            End If

            For Each Item In ChildList
                If (Item.ToString.Contains(".actors"))              Then Continue For
                If (Item.ToString.ToLower.Contains("thumbnails"))   Then Continue For
                If (Item.ToString.ToLower.Contains("extrafanart"))  Then Continue For
                If (Item.ToString.ToLower.Contains("extrathumbs"))  Then Continue For
                If (Item.ToString.ToLower.Contains(".recycle.bin")) Then Continue For
                If (Item.ToString.ToLower.Contains("bdmv"))         Then 
                    If Not File.Exists(Item.ToString.Replace("BDMV", "BDMV\index.bdmv")) Then Continue For
                End If
                If (Item.ToString.ToLower.Contains("certificate"))  Then Continue For
                If Level <= MaxLevels Then
                    If ValidMovieDir(Item) Then
                        TempReturn.AddRange(EnumerateFolders(Item, MaxLevels, Level + 1))
                    End If
                End If
            Next
        Catch
        End Try

        Return TempReturn
    End Function

    Public Shared Function EnumerateFiles(ByVal RootPath As String, ByVal MaxLevels As Long) As List(Of String)
        Return EnumerateFiles(RootPath, MaxLevels, 0)

    End Function

    Public Shared Function EnumerateFiles(ByVal RootPath As String, ByVal MaxLevels As Long, ByVal Level As Long) As List(Of String)
        Dim TempReturn As New List(Of String)
        If String.IsNullOrEmpty(RootPath) Then Return Nothing
        Dim ChildList

        Try
            TempReturn.AddRange(Directory.GetFiles(RootPath))
            ChildList = Directory.GetDirectories(RootPath)
        Catch ex As UnauthorizedAccessException
            Return TempReturn
        End Try
        If Level > 0 Then
            TempReturn.Add(RootPath)
        End If

        For Each Item In ChildList
            'If (Item.ToString.Contains(".actors")) Then Continue For
            If Level <= MaxLevels Then
                TempReturn.AddRange(EnumerateFiles(Item, MaxLevels, Level + 1))
            End If
        Next

        Return TempReturn
    End Function

    Public Shared Function GetYearByFilename(ByVal filename As String, Optional ByVal trimBrackets As Boolean = True, Optional ByVal Scraper As String = "")
        Dim movieyear As String = ""
        If filename.Length > 6 Then    'check if year IS actual Title, ie: movie  2013
            Dim M As Match = Regex.Match(filename, "[ \(\[\.]([\d]{4})[\)\] \.]")
            If M.Success = True Then
                movieyear = If(trimBrackets, M.Groups(1).Value, M.Value)
            End If
            If String.IsNullOrEmpty(movieyear) AndAlso Scraper.ToLower = "tmdb" Then
                M = Regex.Match(filename, "\d{4}")
                If M.Success = True Then movieyear = M.Value
            End If
            'If String.IsNullOrEmpty(movieyear) Then
            '    M = Regex.Match(filename, "\d{8}")
            '    If M.Success = True Then
            '        movieyear = M.Value
            '        movieyear = movieyear.Substring(0,4)
            '    End If
            'End If
        End If
        Return movieyear
    End Function

    Public Shared Function RemoveFilenameExtension(filename As String)
        Return filename.Replace(Path.GetExtension(filename), "")
    End Function

    Public Shared Function checktitle(ByVal fulltitle As String, ByVal movseplst As List(Of String)) As String
        fulltitle = fulltitle.replace(".", " ")
        Dim s As String = ""
        For Each t In movseplst
            Dim u As String = " " & t & " "
            If fulltitle.ToLower.Contains(u.ToLower) Then
                s = t
                Exit For
            End If
        Next
        
        Return s
    End Function

    Public Shared Function CleanFileName(ByVal filename As String, Optional ByVal Scraper As String = "") As String

        Dim currentposition As Integer = filename.Length
        Try
            '0: remove full stops and underscore from filename
            filename = filename.Replace(".", " ")
            filename = filename.Replace("_", " ")
            '1: check for multipart tags
            Dim M As Match = Regex.Match(filename.ToLower, "((" & Join(cleanMultipart, "|") & ")([" & cleanSeparators & "0]?)[1a]$)")
            If M.Success = True Then
                If ignoreParts AndAlso M.Value.IndexOf("p") <> -1 Then   ' "p" identifies a "part" or "pt" tag
                    'skip this shift
                Else
                    If M.Index < currentposition Then currentposition = M.Index
                End If
            End If

            '2: check dvd5 or dvd9 tags
            M = Regex.Match(filename.ToLower, "(dvd[" & cleanSeparators & "]?[59])")
            If M.Success = True Then
                If M.Index < currentposition Then currentposition = M.Index
            End If

            '3: check tags that must have a separator character before them
            M = Regex.Match(filename.ToLower, "([" & cleanSeparators & "]{1}(" & Join(cleanTagsList_SepPrefix, "|") & "))")
            If M.Success = True Then
                If M.Index < currentposition Then currentposition = M.Index
            End If

            '4: check tags that don't need to have a separator character before them
            M = Regex.Match(filename.ToLower, "([" & cleanSeparators & "]?(" & Join(cleanTagsList, "|") & "))")
            If M.Success = True Then
                If M.Index < currentposition Then currentposition = M.Index
            End If

            '5: check tags that are made up of multiple words, and insert the separator characters between them
            Dim multiWordList() As String  = cleanTagsList_MultiWord.Select(Function(str) str.Replace(" ", "[" & cleanSeparators & "]")).ToArray()
            M = Regex.Match(filename.ToLower, "(" & Join(multiWordList, "|") & ")")
            If M.Success = True Then
                If M.Index < currentposition Then currentposition = M.Index
            End If

            '6: check user tags
            If userCleanTags <> "" Then
                Dim escapedCleanTags As String = EscapeSpecialCharacters(userCleanTags)
                Dim splitcleantags As String() = escapedCleanTags.Split("|")
                For Each splittag In splitcleantags
                    M = Regex.Match(filename, "([" & cleanSeparators & "]?(" & splittag & "))")
                    If M.Success = True Then
                        If M.Index < currentposition Then currentposition = M.Index
                    End If
                Next
                
            End If

            '7: remove year from filename, don't panic tho' - MC will still scrape with the year
            Dim movieyear As String = GetYearByFilename(filename, False, Scraper)
            If movieyear <> Nothing Then
                Dim posYear As Integer = filename.IndexOf(movieyear)
                If posYear <> -1 And posYear < currentposition Then currentposition = posYear
            End If

            'Clean up filename if we have any characters left, otherwise original filename is returned
            If currentposition < filename.Length And currentposition > 0 Then
                filename = filename.Substring(0, currentposition)
                filename = Regex.Replace(filename, "[" & cleanSeparators & "]+$", "")   ' remove any trailing separator characters
            End If
            'filename = filename.Replace(".", " ")
        Catch ex As Exception
            filename = "error"
        End Try

        Return filename
    End Function

    Public Shared Function CleanReleaseFormat(ByVal title As String, ByVal relformat() As String) As String
        Dim cleanfilename As String = title
        Try
            For i = 0 to relformat.Length -1
                Dim p As Integer = title.IndexOf(relformat(i), 0, StringComparison.CurrentCultureIgnoreCase)
                If p > -1 Then
                    Dim rl As Integer = relformat(i).length
                    If p+rl <> cleanfilename.Length Then
                        Dim t As String = title.chars(p+rl)
                        If t = " " Then
                            Dim s As String = title.Substring(p, relformat(i).Length)
                            cleanfilename = title.Replace(s, "")
                        End If
                    Else
                        Dim s As String = title.Substring(p, relformat(i).Length)
                        cleanfilename = title.Replace(s, "")
                    End If
                End If
            Next
        Catch ex As Exception

        End Try

        Return cleanfilename
    End Function

    Public Shared Function GetMediaList(ByVal pathandfilename As String)
        Try
            Dim tempstring As String = pathandfilename
            Dim playlist As New List(Of String)
            If File.Exists(tempstring) Then
                playlist.Add(tempstring)
            End If
            tempstring = tempstring.ToLower
            Dim gotit As Boolean = False
            Dim partindex As String = ""
            Dim partindex2 As String = ""
            For Each part In cleanMultipart
                For Each sep In separators
                    For i = 1 to 8
                        partindex = part & sep & i.ToString
                        If tempstring.IndexOf(partindex) = -1 Then Exit For
                        partindex2 = part & sep & (i+1).ToString
                        tempstring = tempstring.Replace(partindex, partindex2)
                        If File.Exists(tempstring) Then
                            playlist.Add(tempstring)
                            gotit = True
                        End If
                    Next
                    If gotit Then Exit For
                Next
                If gotit Then Exit For
            Next
            Return playlist
        Catch
        End Try
        Return "Error"
    End Function

    Public Shared Function save2postercache(ByVal fullpathandfilename As String, ByVal posterpath As String, optional ByVal Width As Integer = 150, Optional ByVal Height As Integer = 200) As String
        Dim ms As IO.MemoryStream = New IO.MemoryStream()
        Using r As IO.Filestream = File.Open(posterpath, IO.FileMode.Open)
            r.CopyTo(ms)
        End Using
        Dim bitmap3 As New Bitmap(ms)
        Dim bitmap2 As New Bitmap(bitmap3)
        bitmap3.Dispose()
        ms.Dispose()
        ms = Nothing
        Dim bm_source As New Bitmap(bitmap2)
        Dim bm_dest As New Bitmap(Width, Height)
        Dim gr As Graphics = Graphics.FromImage(bm_dest)
        gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
        gr.DrawImage(bm_source, 0, 0, Width - 1, Height - 1)
        Dim tempbitmap As Bitmap = bm_dest
        Dim filename As String = Utilities.GetCRC32(fullpathandfilename)
        If Not Directory.Exists(Utilities.PosterCachePath) Then
            Directory.CreateDirectory(Utilities.PosterCachePath)
        End If
        Dim path = Utilities.PosterCachePath & filename & ".jpg"
        Try
            File.Delete(path)
        Catch
        End Try
        Try
            tempbitmap.Save(path, Imaging.ImageFormat.Jpeg)
        Catch
        End Try
        tempbitmap.Dispose()
        GC.Collect()
        Return path
    End Function

    Public Shared Function SpacesToCharacter( ByVal inputText as String, ByVal Character As String) As String
        Return inputText.Replace(" ", Character)
    End Function

    Public Shared Function cleanruntime(ByVal runtime As String) As String
        Try
            If IsNothing(runtime) Then Return runtime
            Dim tempstring As String = runtime
            Dim hours As Integer = 0
            Dim minutes As Integer = 0
            Dim tempruntime As Integer = 0
            If runtime.ToLower.IndexOf("min") <> -1 Then
                tempstring = runtime.Substring(0, runtime.ToLower.IndexOf("min"))
                tempstring = Trim(tempstring)
                If Not IsNumeric(tempstring) Then
                    Dim guess As String = ""
                    For f = 0 To tempstring.Length - 1
                        If IsNumeric(tempstring.Substring(f, 1)) Then
                            guess = guess & tempstring.Substring(f, 1)
                        End If
                    Next
                    If IsNumeric(guess) Then
                        minutes = Convert.ToInt32(guess)
                    End If
                End If
            ElseIf runtime.ToLower.IndexOf("h") <> -1 Or runtime.ToLower.IndexOf("mn") <> -1 Then
                Try
                    '1h 24mn 48s 546ms
                    Dim tempint As Integer = tempstring.IndexOf("h")
                    If tempint <> -1 Then
                        hours = Convert.ToInt32(tempstring.Substring(0, tempint))
                        tempstring = tempstring.Substring(tempint + 1, tempstring.Length - (tempint + 1))
                        tempstring = Trim(tempstring)
                    End If
                    tempint = tempstring.IndexOf("mn")
                    If tempint <> -1 Then
                        minutes = Convert.ToInt32(tempstring.Substring(0, tempint))
                    End If
                    minutes = minutes + (hours * 60)
                Catch
                End Try
            ElseIf IsNumeric(tempstring) Then
                Try
                    tempruntime = Convert.ToInt32(tempstring)
                    minutes = Math.Round(tempruntime / 60)
                Catch
                End Try
            End If

            Return minutes.ToString
        Catch
        Finally

        End Try
        Return "0"
    End Function

    Public Shared Function Cleanbraced(ByVal s As String) As String
        If Not String.IsNullOrEmpty(s) Then
            s = s.TrimStart("|")
            s = s.TrimEnd("|")
            s = s.Replace("|", " / ")
        End If
        Return s
    End Function
    
    Public Shared Function GetLangCode(ByVal strLang As String) As String
        Try
            Select Case strLang.ToLower
                Case "portugues"
                    Return "por"
                Case "undefined"
                    Return ""
                Case "unknown"
                    Return ""
               Case "english","english / english","english (us)"
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
                Case "afroasiatic (other)"
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
                Case "official aramaic (700300 bce)", "imperial aramaic (700300 bce)"
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
                    Return "dut"
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
                Case "finnougrian (other)"
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
                Case "indoeuropean (other)"
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
                Case "judeopersian"
                    Return "jpr"
                Case "judeoarabic"
                    Return "jrb"
                Case "karakalpak"
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
                Case "karachaybalkar"
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
                Case "lubalulua"
                    Return "lua"
                Case "lubakatanga"
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
                Case "monkhmer (other)"
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
                Case "nigerkordofanian (other)"
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
                    Return "qaaqtz"
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
                Case "aromanian", "arumanian", "macedoromanian"
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
                Case "sinotibetan (other)"
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
                Case "nilosaharan (other)"
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
                Case "klingon", "tlhinganhol"
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
    
    Public Shared Function LoadTextLines(ByVal path As String) As List(Of String)

        Dim listoflines As New List(Of String)
        Try
            If Not File.Exists(path) Then
                listoflines.Add("nofile")
                Return listoflines
            Else
                Using lines As IO.StreamReader = File.OpenText(path)
                    Dim line As String
                    Do
                        line = lines.ReadLine
                        If Not line Is Nothing Then
                            listoflines.Add(line)
                        Else
                            Exit Do
                        End If
                    Loop Until line = Nothing
                End Using
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
            If Not File.Exists(path) Then
                text = "nofile"
                Return text
            Else
                Using lines As IO.StreamReader = File.OpenText(path)
                    text = lines.ReadToEnd
                End Using
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
    
    Public Shared Function createImage(ByVal origImage As String, ByVal sizeLimit As Integer, ByVal target As String, Optional ByVal picType As String = "poster")

        Dim isPoster As Boolean = Equals(picType, "poster")
        Dim filename As String = If(isPoster, "DefaultPoster", "DefaultBanner") & If(sizeLimit <> 0, "_" & sizeLimit.ToString, "") & ".jpg"
        Dim imgPoster As String = If(isPoster, Utilities.DefaultPosterPath, Utilities.DefaultBannerPath)
        Try
            'First, check if source image is legitimate. If so, create the unique filename, otherwise the default image will be used.
            If File.Exists(origImage) Then
                Dim origBitmap As Image = Image.FromFile(origImage)
                Dim origRatio As Single = 0
                origRatio = origBitmap.Height / origBitmap.Width
                If isPoster And origRatio >= 1 Or Not isPoster And origRatio < 1 Then
                    If sizeLimit = 0 Then sizeLimit = If(isPoster, origBitmap.Height, origBitmap.Width) 'sizeLimit = 0 denotes keep original dimensions
                    filename = File.GetLastWriteTime(origImage).ToFileTimeUtc & "_" & Utilities.GetCRC32(origImage) & "_" & sizeLimit.ToString & ".jpg"
                    imgPoster = origImage
                End If
                origBitmap.Dispose()
            End If
        Catch ex As Exception
            'If the source is corrupt, alert the user and use the default image.
            MsgBox(String.Format("There was an error processing image: {0}{1}Please check source image. Using default {2}.", origImage, vbCrLf, picType))
        End Try

        Try
            'Second, if the target image already exists, don't bother creating it again.
            If Not File.Exists(Path.Combine(target, filename)) Then
                Dim ms As IO.MemoryStream = New IO.MemoryStream()
                Using r As IO.Filestream = File.Open(imgPoster, IO.FileMode.Open)
                    r.CopyTo(ms)
                End Using
                Dim srcBitmap As New Bitmap(ms)
                ms.dispose
                Dim height As Integer = srcBitmap.Height
                Dim width As Integer = srcBitmap.Width
                Dim dstBitmap As New Bitmap(srcBitmap)
                If sizeLimit <> 0 Then
                    If isPoster Then
                        height = sizeLimit
                        width = Math.Truncate(height * (srcBitmap.Width / srcBitmap.Height))
                    Else
                        width = sizeLimit
                        height = Math.Truncate(width * (srcBitmap.Height / srcBitmap.Width))
                    End If
                End If
                srcBitmap.Dispose()
                dstBitmap = Utilities.ResizeImage(dstBitmap, width, height)
                dstBitmap.Save(Path.Combine(target, filename), System.Drawing.Imaging.ImageFormat.Jpeg)
                dstBitmap.Dispose()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return filename
    End Function

    Public Shared Function SaveImage(ByVal image As Bitmap, ByVal path As String) As Boolean
        Try
            GC.Collect()
            If (File.Exists(path)) Then
                File.Delete(path)
            Else
                Utilities.EnsureFolderExists(path)
            End If
            If path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) Then
                image.Save(path,Imaging.ImageFormat.Png)
            Else
                image.Save(path, Imaging.ImageFormat.Jpeg)
            End If
            
            SaveImage = True
        Catch ex As Exception
            SaveImage = False
        'Finally
        '    image.Dispose() 'because image is passed in ByRef, it should be disposed of, but alas it is not.
        End Try
        image.Dispose()
    End Function

    Public Shared Sub ResizeImage(ByVal path As String, ByVal width As Integer, ByVal height As Integer)
        Using ms As IO.MemoryStream = New IO.MemoryStream()
            Using r As IO.Filestream = File.Open(path, IO.FileMode.Open)
                r.CopyTo(ms)
            End Using
            Using img As Bitmap = ResizeImage(New Bitmap(ms), width, height)
                SaveImage(img, path)
            End Using
        End Using
    End Sub
    
    Public Shared Function ResizeImage(ByVal bm_source As Bitmap, ByVal width As Integer, ByVal height As Integer) As Bitmap
        Dim bm_dest As New Bitmap(width, height)
        Using gr As Graphics = Graphics.FromImage(bm_dest)
            gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
            gr.DrawImage(bm_source, 0, 0, width, height)
        End Using
        Return bm_dest
    End Function

    Public Shared Function GetImage(src As String) As Bitmap

        Dim bm As New IO.MemoryStream(My.Computer.FileSystem.ReadAllBytes(src))  'As New Bitmap(src)
        Dim bm2 As New Bitmap(bm)

        bm.Dispose()

        Return bm2
    End Function

    Public Shared Function LoadImage(ByVal path As String) As Bitmap
        Try
            Dim ms As IO.MemoryStream = New IO.MemoryStream()
            Using r As IO.Filestream = File.Open(path, IO.FileMode.Open)
                r.CopyTo(ms)
                Using img As Bitmap = New Bitmap(ms)
                    ms.Dispose()
                    Return Utilities.ResizeImage(img, img.Width, img.Height)
                End Using
            End Using
        Catch
            Return Nothing

        End Try
    End Function
    
    Public Shared Sub copyImage(ByVal src As String, ByVal dest As String, Optional ByVal resizeFanart As Integer = 0)
        Try
            Dim img = Utilities.GetImage(src)
            Dim width As Integer = img.Width
            Dim height As Integer = img.Height

            Select Case resizeFanart
                Case 2
                    width = 1280
                    height = 720
                Case 3
                    width = 960
                    height = 540
            End Select

            img = ResizeImage(img, width, height)
            SaveImage(img, dest)
            img.Dispose()   'because image is passed in ByRef to SaveImage, it should be disposed of, but alas it is not.
            GC.Collect()
        Catch
        End Try
    End Sub

    Public Shared Function DownloadFile(ByVal URL As String, ByVal Path As String, Optional ByVal Force As Boolean = True) As Boolean
        Try
            If Not Force AndAlso File.Exists(Path) Then Return True   'True as file exists.
            Dim returnState As Boolean = DownloadCache.DownloadFileAndCache(URL, Path, True)
            Return returnstate
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Shared Function DownloadImage(ByVal URL As String, ByVal Path As String, Optional Overwrite As Boolean = True) As Boolean
        Try
            Dim returnState As Boolean = DownloadCache.DownloadFileAndCache(URL, Path, False)  'If image already in cache, why re-download it.
            Return returnstate
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Shared Function Download2Cache(ByVal url As String) As String     'Returns cachefilename if successful
        Try
            Dim cachefilenameandpath As String = ""
            Dim returnState As Boolean = DownloadCache.DownloadFileAndCache(url, "cache", False, 0, True, strValue:=cachefilenameandpath)
            Return cachefilenameandpath 
        Catch ex As Exception
            Return ""
        End Try
    End Function
    
    Public Shared Function EnsureFolderExists(ByVal foldPath As String) As Boolean
        Dim Parts As String() = Split(Path.GetDirectoryName(foldPath), "\")
        Dim currentPath As String = Parts(0)
        Dim addStart As Integer = 1

        If Left(foldPath, 2) = "\\" Then 'Network path
            currentPath &= "\" & Parts(1) & "\" & Parts(2) & "\" & Parts(3)
            addStart = 4
        End If

        For I = addStart To Parts.GetUpperBound(0)
            currentPath = Path.Combine(currentPath & "\", Parts(I))

            If Not Directory.Exists(currentPath) Then
                Try
                    Directory.CreateDirectory(currentPath)
                Catch
                    Return False
                End Try
            End If
        Next

        Return True
    End Function
    
    Public Shared Function GetFileNameFromPath(ByVal ispath As String) As String
        Try
            Return ispath.Substring(ispath.LastIndexOf("\") + 1)
        Catch ex As Exception
            Return ispath
        End Try
    End Function

    Public Shared Function ComputeHashValueToByte(ByVal Input As String) As Byte()
        Return ComputeHashValue(StrToByteArray(Input))
    End Function
    
    Public Shared Function ComputeHashValue(ByVal data() As Byte) As Byte()
        Dim hashAlg As SHA1 = SHA1.Create()
        Dim hashvalue() As Byte = hashAlg.ComputeHash(data)
        Return hashvalue
    End Function
    
    ''' <summary>
    ''' Get MD5Hash value from 1st 64k and last 64k of video file
    ''' for searching for correct subtitles from SubDB.
    ''' </summary>
    ''' <param name="source">path and filename</param>
    ''' <returns></returns>
    Public Shared Function GetMd5Hash(source As String) As String
        Dim datasize As Int32 = 64*1024
        Dim buffer() As Byte = New Byte(datasize-1) {}
        Dim buffer2() As Byte = New Byte(datasize-1) {}
        Using fs As New IO.FileStream(source, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
            fs.Read(buffer, 0, datasize)
            Dim endbyte As Long = fs.Length - datasize
            fs.Seek(endbyte, IO.SeekOrigin.Begin)
            fs.Read(buffer2, 0, datasize)
        End Using
        Dim buffer3() As Byte = buffer.Concat(buffer2).ToArray
        Return GetMd5Hash(buffer3)
 
    End Function

    Public Shared Function GetMd5Hash(bytes() As Byte) As String
        Dim md5 As New MD5CryptoServiceProvider()
        Return Bytes_To_HexString2(md5.ComputeHash(bytes))
    End Function

    Public Shared Function Bytes_To_HexString2(ByVal bytes_Input As Byte()) As String
        Dim strTemp As New StringBuilder(bytes_Input.Length * 2)
        For Each b As Byte In bytes_Input
            strTemp.Append(Conversion.Hex(b))
        Next
        Return strTemp.ToString()
    End Function
 
    Public Shared Function StrToByteArray(str As String) As Byte()
        Dim encoding As New System.Text.ASCIIEncoding()
        Return encoding.GetBytes(str)
    End Function

    'Public Shared Function ByteArrayToStr(dBytes As Byte()) As String
    '    Dim str As String
    '    Dim enc As New System.Text.ASCIIEncoding()
    '    str = enc.GetString(dBytes)
    '    Return str
    'End Function

    Public Shared Function EscapeSpecialCharacters(s As String) As String
        s = s.Replace("(", "\(")
        s = s.Replace(")", "\)")
        s = s.Replace("{", "\{")
        s = s.Replace("}", "\}")
        s = s.Replace("[", "\[")
        s = s.Replace("]", "\]")
        Return s
    End Function

    Public Shared Function RemoveEscapeCharacter(ByVal s As String) As String
        s = s.Replace("\"&Chr(34), Chr(34))
        s = s.Replace("\'", "'")
        Return s
    End Function

    Public Shared Function cleanSpecChars(ByVal string2clean As String) As String
        Return WebUtility.HtmlDecode(string2clean)
    End Function

    Public Shared Function ReplaceNothing(ByVal text As String, Optional ByVal replacetext As String = "") As String
        If text Is Nothing Then text = replacetext
        Return text
    End Function

    Public Shared Function cleanFilenameIllegalChars(ByVal string2clean As String, Optional ByVal changechr As String = "") As String
        Dim strIllegalChars As String = "\/:""*?<>|"
        Dim illegalChars As Char() = strIllegalChars.ToCharArray
        Dim M As Match = Regex.Match(string2clean, "[\" & strIllegalChars & "]") 'HACK ALERT! - back-slash added to regex pattern string to escape illegal back-slash character!
        If M.Success = True Then
            Dim changeTo As String = ""
            For Each c As Char In illegalChars
                Select Case c
                    Case """"
                        changeTo = "'"
                    Case ":", "|"
                        changeTo = " -"
                    Case Else
                        changeTo = changechr
                End Select
                string2clean = string2clean.Replace(c, changeTo)
            Next
        End If
        Return string2clean
    End Function

    Public Shared Function cleanFoldernameIllegalChars(ByVal string2clean As String) As String
        Dim strIllegalChars As String = "/:""*?<>|"
        Dim illegalChars As Char() = strIllegalChars.ToCharArray
        Dim M As Match = Regex.Match(string2clean, "[\" & strIllegalChars & "]") 'HACK ALERT! - back-slash added to regex pattern string to escape illegal back-slash character!
        If M.Success = True Then
            Dim changeTo As String = ""
            For Each c As Char In illegalChars
                Select Case c
                    Case """"
                        changeTo = "'"
                    Case ":", "|"
                        changeTo = " -"
                    Case Else
                        changeTo = ""
                End Select
                string2clean = string2clean.Replace(c, changeTo)
            Next
        End If
        Return string2clean
    End Function

    Public Shared Function CheckForXMLIllegalChars(ByRef xmlfile As String) As Boolean
        Dim xmlOK As Boolean = False
        Dim numCharLimit As Integer = 10    'Arbitrary limit so we don't get lost in an infinite loop
        Try
            Do
                Dim episode As New XmlDocument
                Try
                    episode.LoadXml(xmlfile)    'Load XML as normal - if all goes well, we're outta here!
                    xmlOK = True
                Catch ex As XmlException
                    xmlfile = Utilities.ReplaceXMLIllegalChars(xmlfile, ex.LineNumber, ex.LinePosition) 'Let's assume an illegal character is the problem, and convert it.
                    numCharLimit -= 1
                End Try
            Loop Until xmlOK Or numCharLimit = 0
        Catch
        End Try
        Return xmlOK
    End Function

    Public Shared Function CleanInvalidXmlChars(text As String) As String
        ' From xml spec valid chars: 
        ' #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
        ' any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
        Dim re As String = "[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-u10FFFF]"
        Return Regex.Replace(text, re, "")
    End Function

    Public Shared Function ReplaceXMLIllegalChars(ByVal xmlfile As String, ByVal linenumber As Long, ByVal charpos As Integer) As String
        Dim lines As New List(Of String)
        Using reader As New IO.StringReader(xmlfile)   'Using StringReader to take care of unknown newlines
            While reader.Peek() <> -1
                lines.Add(reader.ReadLine())
            End While
        End Using
        Dim suspectLine As String = lines(linenumber - 1)
        If suspectLine.Contains(Chr(1)) Then
            lines(linenumber -1) = suspectLine.Replace(Chr(1),"")
            Return String.Join(Environment.NewLine, lines)
        End If
        If charpos > 1 Then charpos = charpos - 2 'Not ideal but the "illegal" character may be after a "<" so we have to go back an extra character position. 
        Dim suspectChar As String = suspectLine.Substring(charpos, 2)  '(charpos - 2, 2)   - Changed to above as character may be at start of xml file. 
        lines(linenumber - 1) = suspectLine.Replace(suspectChar, System.Security.SecurityElement.Escape(suspectChar))
        Return String.Join(Environment.NewLine, lines)
    End Function
    
    Public Shared Function SafeDeleteFile(ByVal fileName As String) As Boolean
        If Not File.Exists(fileName) Then Return True
        Try
            Dim numTries As Integer = 0
            While (True)
                numTries += 1
                Try
                    
                    Using fs As IO.Stream = File.Open(fileName, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.None, 100)
                        fs.ReadByte()
                        Exit While
                    End Using
                Catch ex As Exception
                    If numTries > 10 Then Return False
                    Thread.Sleep(100)
                End Try
            End While

            File.Delete(fileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function SafeCopyFile(ByVal srcFile As String, ByVal destFile As String, Optional ByVal overwrite As Boolean = True) As Boolean
        If File.Exists(destFile) Then
            If overwrite Then
                SafeDeleteFile(destFile)
            Else
                Return False
            End If
        End If
        File.Copy(srcFile, destFile)
        Return True
    End Function

    Public Shared Function IsBanner(ByVal srcfile As String) As Boolean
        Dim state As Boolean = False
        Try
            Dim srcimg As Bitmap 
            srcimg = GetImage(srcfile)
            Dim width As Integer = srcimg.Width 
            Dim height As Integer= srcimg.Height 
            If width > (height * 3) Then 
                state = True
            End If
            srcimg.Dispose()           
        Catch ex As Exception

        End Try
        Return state
    End Function

    Public Shared Function IsDirectoryEmpty(ByVal testdir As String) As Boolean
        Dim s() As String = Directory.GetFiles(testdir)
        If s.Length = 0 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function RootVideoTsFolder(ByVal FullPath As String) As String
        If Right(FullPath, 1) <> Path.DirectorySeparatorChar Then
            FullPath = FullPath.Replace(Path.GetFileName(FullPath), "")
        End If
        Dim length As integer
        Dim foldername As String = ""
        Dim paths() As String
        paths = FullPath.Split(Path.DirectorySeparatorChar)
        For g = UBound(paths) To 0 Step -1
            If paths(g).ToLower.IndexOf("video_ts") = -1 And paths(g).ToLower.IndexOf("bdmv") = -1And paths(g) <> "" Then
                length = g
                Exit For
            End If
        Next
        FullPath=""
        For g = 0 to length
            FullPath += paths(g)+"\"
        Next
        Return FullPath
    End Function

    Public Shared Function GetLastFolderInPath(ByVal path As String) As String
        Return New DirectoryInfo(path.TrimEnd("\")).Name
    End Function

    Public Shared Function GetExtension(path As String) As String
        Dim Extn As String
        Dim NoDot() As String
        NoDot = Split(path, “.”)
        Extn = NoDot(UBound(NoDot))
        Return Extn
    End Function
    
    Public Shared Function GetTmdbLanguage(ByVal xbmctmdb As String) As List(Of String)
        Dim AvailableLanguages As XDocument = XDocument.Load(applicationPath & "\classes\tmdb_languages.xml")
        Dim q = From x In AvailableLanguages.Descendants("language")
                            Select value   = x.Attribute("value").Value, 
                                   attName = x.Attribute("name" ).Value
                            Where attName = xbmctmdb 

        Return q.Single().value.Split(",").ToList
    End Function

    Public Shared Function loadGenre() As List(Of String)
        Dim Genrelist As New List(Of String)
        Dim genrepath As String = applicationPath & "\classes\genre.txt"
        If File.Exists(genrepath) Then
            Dim line As String = String.Empty
            Try
                Using userConfig As IO.StreamReader = File.OpenText(genrepath)
                    Do
                        Try
                            line = userConfig.ReadLine
                            If line <> Nothing Then
                                Dim regexMatch As Match
                                regexMatch = Regex.Match(line, "<([\d]{2,3})>")
                                If regexMatch.Success = False Then
                                    Genrelist.Add(line.Trim)
                                End If
                            End If
                        Catch ex As Exception
                        End Try
                    Loop Until line = Nothing
                End Using
            Catch ex As Exception
            End Try
        End If
        Return Genrelist
    End Function

    Public Shared Function searchurltitle(ByVal title As String) As String
        Dim urltitle As String = title
        Try

            urltitle = urltitle.Replace(".", "+")
            urltitle = urltitle.Replace(" ", "+")
            urltitle = urltitle.Replace("_", "+")
            urltitle = urltitle.Replace("À", "%c0")
            urltitle = urltitle.Replace("Á", "%c1")
            urltitle = urltitle.Replace("Â", "%c2")
            urltitle = urltitle.Replace("Ã", "%c3")
            urltitle = urltitle.Replace("Ä", "%c4")
            urltitle = urltitle.Replace("Å", "%c5")
            urltitle = urltitle.Replace("Æ", "%c6")
            urltitle = urltitle.Replace("Ç", "%c7")
            urltitle = urltitle.Replace("È", "%c8")
            urltitle = urltitle.Replace("É", "%c9")
            urltitle = urltitle.Replace("Ê", "%ca")
            urltitle = urltitle.Replace("Ë", "%cb")
            urltitle = urltitle.Replace("Ì", "%cc")
            urltitle = urltitle.Replace("Í", "%cd")
            urltitle = urltitle.Replace("Î", "%ce")
            urltitle = urltitle.Replace("Ï", "%cf")
            urltitle = urltitle.Replace("Ð", "%d0")
            urltitle = urltitle.Replace("Ñ", "%d1")
            urltitle = urltitle.Replace("Ò", "%d2")
            urltitle = urltitle.Replace("Ó", "%d3")
            urltitle = urltitle.Replace("Ô", "%d4")
            urltitle = urltitle.Replace("Õ", "%d5")
            urltitle = urltitle.Replace("Ö", "%d6")
            urltitle = urltitle.Replace("Ø", "%d8")
            urltitle = urltitle.Replace("Ù", "%d9")
            urltitle = urltitle.Replace("Ú", "%da")
            urltitle = urltitle.Replace("Û", "%db")
            urltitle = urltitle.Replace("Ü", "%dc")
            urltitle = urltitle.Replace("Ý", "%dd")
            urltitle = urltitle.Replace("Þ", "%de")
            urltitle = urltitle.Replace("ß", "%df")
            urltitle = urltitle.Replace("à", "%e0")
            urltitle = urltitle.Replace("á", "%e1")
            urltitle = urltitle.Replace("â", "%e2")
            urltitle = urltitle.Replace("ã", "%e3")
            urltitle = urltitle.Replace("ä", "%e4")
            urltitle = urltitle.Replace("å", "%e5")
            urltitle = urltitle.Replace("æ", "%e6")
            urltitle = urltitle.Replace("ç", "%e7")
            urltitle = urltitle.Replace("è", "%e8")
            urltitle = urltitle.Replace("é", "%e9")
            urltitle = urltitle.Replace("ê", "%ea")
            urltitle = urltitle.Replace("ë", "%eb")
            urltitle = urltitle.Replace("ì", "%ec")
            urltitle = urltitle.Replace("í", "%ed")
            urltitle = urltitle.Replace("î", "%ee")
            urltitle = urltitle.Replace("ï", "%ef")
            urltitle = urltitle.Replace("ð", "%f0")
            urltitle = urltitle.Replace("ñ", "%f1")
            urltitle = urltitle.Replace("ò", "%f2")
            urltitle = urltitle.Replace("ó", "%f3")
            urltitle = urltitle.Replace("ô", "%f4")
            urltitle = urltitle.Replace("õ", "%f5")
            urltitle = urltitle.Replace("ö", "%f6")
            urltitle = urltitle.Replace("÷", "%f7")
            urltitle = urltitle.Replace("ø", "%f8")
            urltitle = urltitle.Replace("ù", "%f9")
            urltitle = urltitle.Replace("ú", "%fa")
            urltitle = urltitle.Replace("û", "%fb")
            urltitle = urltitle.Replace("ü", "%fc")
            urltitle = urltitle.Replace("ý", "%fd")
            urltitle = urltitle.Replace("þ", "%fe")
            urltitle = urltitle.Replace("ÿ", "%ff")
            urltitle = urltitle.Replace("'","%27")
            urltitle = urltitle.Replace("!", "%21")
            urltitle = urltitle.Replace("&", "%26")
            urltitle = urltitle.Replace(",", "")
            urltitle = urltitle.Replace("++", "+")
            Return urltitle
        Catch
            Return urltitle
        End Try
    End Function

    Public Shared Function ValidImdbId(ByVal id As String) As Boolean
        Dim aok As Boolean = False
        If Not String.IsNullOrEmpty(id) AndAlso id.StartsWith("tt") AndAlso id.Length = 9 Then aok = True
        Return aok
    End Function

End Class

Public Class langlib
    Public language  As String
    Public lang2     As String
    Public lang3     As String

    Sub New
        Init
    End Sub

    Public Sub Init
        language    = ""
        lang2       = ""
        lang3       = ""
    End Sub

End Class