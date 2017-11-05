'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text
Imports System.Text.RegularExpressions
Imports Media_Companion.WorkingWithNfoFiles

Public Class MediaInfoExport

    Public Enum mediaType
        None
        Movie
        TV
    End Enum

    Public Structure mediaInfoExportTemplate
        Dim title As String
        Dim path As String  'not used anymore as the template is stored when adding
        Dim body As String
        Dim type As mediaType
        Dim css As String
        Dim cssfile As String
        Dim TextEncoding As Encoding
        Dim FileName As String

        Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
            title = ""
            path = ""
            body = ""
            type = mediaType.None
            css = ""
            cssfile = ""
            TextEncoding = Encoding.UTF8
            FileName = ""
        End Sub
    End Structure

    Public workingTemplate As mediaInfoExportTemplate
    Dim templateList As New List(Of mediaInfoExportTemplate)
    Dim fullTemplateString As String = Nothing
    Dim nfoFunction As New WorkingWithNfoFiles
    Dim templateFolder As String = Path.Combine(Application.StartupPath, "html_templates\")
    'RegexOptions.IgnoreCase to make the regex case insensitive, and RegexOptions.Singleline causes the dot to match newlines
    Dim regexBlockOption As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Singleline

    Private Delegate Function getTagsDelegate(ByVal text As String, ByVal mediaCollection As Object, ByVal showCounter As Integer, ByVal imagepath As String, ByVal moviecount As Integer, ByVal filetype As String, ByRef index As Integer) As String

    Public Sub addTemplates(Optional ByRef mediaDropdown As SortedList(Of String, String) = Nothing)
        Dim dir_info As New DirectoryInfo(templateFolder)
        Dim fs_infos() As FileInfo = dir_info.GetFiles("*.txt", IO.SearchOption.TopDirectoryOnly)
        templateList.Clear()
        For Each info In fs_infos
            Try
                Dim fileTemplateString As String
                Using fileStream As IO.StreamReader = File.OpenText(info.FullName)
                    fileTemplateString = fileStream.ReadToEnd
                End Using
                Dim M As Match = Regex.Match(fileTemplateString, "<(menu)?title>(?<title>.*?)</(menu)?title>.*?<<(?<mcpage>mc(?<type> tv)? html page)>>(?<body>.*?)<</\k<mcpage>>>", regexBlockOption)
                If M.Success Then
                    Dim template As New mediaInfoExportTemplate(True)
                    template.title = M.Groups("title").Value.Trim.ToLower
                    template.path = info.FullName
                    template.body = M.Groups("body").Value.Trim
                    template.type = If(M.Groups("type").Value Is String.Empty, mediaType.Movie, mediaType.TV)

                    Dim css As Match = Regex.Match(fileTemplateString, "<<css>>.*?<filename>(?<cssfile>.*?)</filename>(?<cssbody>.*?)<</css>>", regexBlockOption)
                    If css.Success Then
                        template.css = css.Groups("cssbody").Value.Trim
                        template.cssfile = css.Groups("cssfile").Value
                    End If

                    Dim M2 As Match

                    M2 = Regex.Match(fileTemplateString, "<<filename>>(?<filename>.*?)<</filename>>", regexBlockOption)
                    If M2.Success Then template.FileName = M2.Groups("filename").Value.Trim

                    M2 = Regex.Match(fileTemplateString, "<<textencoding>>(?<textencoding>.*?)<</textencoding>>", regexBlockOption)
                    If M2.Success Then
                        If M2.Groups("textencoding").Value.Trim.ToUpper() = "ASCII" Then template.TextEncoding = Encoding.ASCII
                    End If
                    
                    If mediaDropdown IsNot Nothing Then mediaDropdown.Add(M.Groups("title").Value.Trim, template.type) 'title used as key to avoid duplicate titles
                    templateList.Add(template)
                End If
            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Sub createDocument(ByVal savePath As String, ByVal media As Object)
        Dim templateBody As String = workingTemplate.body
        Dim isMovies As Boolean = [Enum].Equals(workingTemplate.type, mediaType.Movie)
        Dim getTags As getTagsDelegate
        Dim mediaCollection
        If isMovies Then
            getTags = AddressOf getTagsMovies
            mediaCollection = TryCast(CObj(media), List(Of ComboList))
        Else
            getTags = AddressOf getTagsTV
            mediaCollection = TryCast(CObj(media), NotifyingList(Of TvShow)).GetSortedShow()
        End If

        Dim M As Match
        Dim counter As Integer = 1
        Dim limit As Integer = 0
        Dim mediaInsertIndex As Integer = 0
        Dim pathstring As String = ""
        Dim populatedDoc As String = ""
        Dim tempBody As String = ""
        Dim mediaList As String = ""
        Dim mediaTemplate As String = ""
        Dim mediaTagpresent As Boolean = False
        Dim filetype As String = Path.GetExtension(savePath).TrimStart("."c)
        Dim displayLineTitle As String = "Exporting Media Info"
        Dim displayLineRemaining As String = ""
        Dim callingApp As String = My.Application.Info.AssemblyName
        Dim isConsole As Boolean = Equals(callingApp, "mc_com")
        Dim padConsoleLine As Integer = 0
        Dim frmMediaInfoExport As New frmDialog1    'frmMediaInfoExport could run in a different thread?
        If isConsole Then
            Console.WriteLine(displayLineTitle & "...")
        Else
            frmMediaInfoExport.Label3.Text = displayLineTitle
            frmMediaInfoExport.Label4.Text = displayLineRemaining
            frmMediaInfoExport.Label3.Refresh()
            frmMediaInfoExport.Label4.Refresh()
            Application.DoEvents()
            frmMediaInfoExport.Show()
        End If

        'Check to see if any images are to be created, and provide reference to image save directory - non-destructive
        If Regex.IsMatch(templateBody, "<<(smallimage|createimage(:\w*?)*)>>") Then
            pathstring = String.Format("{0}{1}{2}images{1}", Path.GetDirectoryName(savePath), Path.DirectorySeparatorChar, If(isMovies, "", "tv"))
            Dim fso As New DirectoryInfo(pathstring)
            If fso.Exists = False Then Directory.CreateDirectory(pathstring)
        End If

        'Check for media item tag
        M = Regex.Match(templateBody, "<<media_item(?<limit>:\d+)?>>(?<mediaitem>.*?)<</media_item>>", regexBlockOption)
        If M.Success Then
            mediaTagpresent = True
            mediaInsertIndex = M.Index
            mediaTemplate = M.Groups("mediaitem").Value.Trim(vbCrLf)
    '       If filetype = "csv" Then mediaTemplate = mediaTemplate.Replace(vbCrLf, "")
            templateBody = templateBody.Replace(M.Value, "")
            Integer.TryParse(M.Groups("limit").Value.TrimStart(":"), limit) 'a fail means "limit" remains at default of 0 - display all media items
        End If

        'Check for body tag - this is mainly to support legacy templates that used <<body>> for the media collection template
        M = Regex.Match(templateBody, "<<body>>(?<body>.*?)<</body>>", regexBlockOption)
        If M.Success Then
            If mediaTagpresent Then
                mediaInsertIndex -= 2
                tempBody = M.Groups("body").Value
            Else
                mediaInsertIndex = M.Index + 6
                mediaTemplate = M.Groups("body").Value
                tempBody = ""
            End If
            templateBody = templateBody.Replace(M.Value, String.Format("<body>{0}</body>", tempBody))
        End If

        'Check for header tag, and if present, create HTML document with header
        M = Regex.Match(templateBody, "<<header>>(?<header>.*?)<</header>>", regexBlockOption)
        If M.Success Then
            templateBody = templateBody.Replace(M.Value, String.Format("<!DOCTYPE html>{0}<head>{1}</head>{0}", vbCrLf, M.Groups("header").Value)) & vbCrLf & "</html>"
            mediaInsertIndex += 11
        End If

        'Check for footer tag, and if present, replace with footer tags and content
        M = Regex.Match(templateBody, "<<footer>>(?<footer>.*?)<</footer>>", regexBlockOption)
        If M.Success Then templateBody = templateBody.Replace(M.Value, String.Format("<footer>{0}</footer>", M.Groups("footer").Value))

        'Populate document template
        templateBody = getTags(templateBody, mediaCollection(0), counter, pathstring, mediaCollection.Count, filetype, mediaInsertIndex)

        'Iterate over media collection using the appropriate template
        For Each mediaItem In mediaCollection
            If frmMediaInfoExport.IsDisposed OrElse (limit <> 0 And counter > limit) Then Exit For
            displayLineTitle = String.Format("Processing: {0}", If(isMovies, mediaItem.title, mediaItem.title.Value))
            displayLineRemaining = String.Format("{0} {1}{2} Remaining", mediaCollection.Count - counter, If(isMovies, "Movie", "TV Show"), If((mediaCollection.Count - (counter + 1)) > 1, "s", ""))
            If isConsole Then
                Console.Write(String.Format("{0} - {1}", displayLineRemaining, displayLineTitle.PadRight(padConsoleLine)))
                Console.SetCursorPosition(0, Console.CursorTop)
                padConsoleLine = displayLineTitle.Length + 2    'an extra char to account for decreasing Count length
            Else
                frmMediaInfoExport.Label3.Text = displayLineTitle
                frmMediaInfoExport.Label4.Text = displayLineRemaining
                frmMediaInfoExport.Label3.Refresh()
                frmMediaInfoExport.Label4.Refresh()
                Application.DoEvents()
            End If
            mediaList &= getTags(mediaTemplate, mediaItem, counter, pathstring, mediaCollection.Count, filetype, 0)
            counter += 1
        Next

        If frmMediaInfoExport.IsDisposed Then
            MsgBox("Operation Canceled")
        Else
            If mediaInsertIndex Then
                populatedDoc = templateBody.Insert(mediaInsertIndex, mediaList)
            Else
                populatedDoc = mediaList
            End If

            Try
                If workingTemplate.css IsNot String.Empty Then
                    Dim cssWriter As New IO.StreamWriter(Path.GetDirectoryName(savePath) & Path.DirectorySeparatorChar & workingTemplate.cssfile, False, workingTemplate.TextEncoding)
                    cssWriter.Write(workingTemplate.css)
                    cssWriter.Dispose()
                End If
                Dim docWriter As New IO.StreamWriter(savePath, False, workingTemplate.TextEncoding)
                docWriter.Write(populatedDoc)
                docWriter.Close()
            Catch ex As Exception
                MsgBox(ex.ToString)
            Finally
                frmMediaInfoExport.Close()
            End Try
            If isConsole Then
                Console.WriteLine(" ".PadRight(padConsoleLine + 20))
            Else
                Dim tempint As Integer = MessageBox.Show("Your list is now complete" & vbCrLf & " Do You wish to view it now?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If tempint = DialogResult.Yes Then Process.Start(savePath)
            End If
        End If
    End Sub

    Private Function getTagsMovies(ByVal templ As String, ByVal movie As ComboList, ByVal counter As Integer, ByVal thumbpath As String, ByVal moviecount As Integer, ByVal filetype As String, Optional ByRef insertIndex As Integer = 0)
        Dim templateParts As New List(Of String)
        Dim templatePopulated As New List(Of String)
        If insertIndex Then
            templateParts.Add(templ.Substring(0, insertIndex))
        End If
        templateParts.Add(templ.Substring(insertIndex))

        For Each templPart In templateParts
            Dim tokenRegExp As New Regex("<<[\w_:]+>>")
            Dim tokenCollection As MatchCollection = tokenRegExp.Matches(templPart)
            Dim token As Match
            Dim fi As FileInfo

            For Each token In tokenCollection
                Dim strNFOprop As String = ""
                Dim preEscapeNFOprop As String = ""
                Dim valToken As String = token.Value.Substring(2, token.Value.Length - 4)
                Dim tokenInstr() As String = valToken.Split(":")
                Select Case tokenInstr(0)
                    Case "smallimage", "createimage"
                        If thumbpath IsNot String.Empty AndAlso Not thumbpath.Equals("!HEADER!") Then
                            Dim origImage = Pref.GetPosterPath(movie.fullpathandfilename)
                            Try
                                If (tokenInstr(UBound(tokenInstr)) <> "nopath") Then strNFOprop &= "images/"
                                strNFOprop &= Utilities.createImage(origImage, If(tokenInstr(0) = "createimage" And tokenInstr.Length > 1, Val(tokenInstr(1)), 200), thumbpath)
                            Catch ex As Exception
                                MsgBox(ex.ToString)
                            End Try
                        End If

                    Case "moviecount"
                        strNFOprop = If(moviecount, moviecount.ToString, "0000")

                    Case "counter"
                        strNFOprop = counter.ToString

                    Case "imdb_id"
                        strNFOprop = If(movie.id <> Nothing, movie.id, "")

                    Case "imdb_num"
                        strNFOprop = If(movie.id <> Nothing, movie.id.Replace("tt", ""), "")

                    Case "file_size"
                        Try
                            strNFOprop = Utilities.GetFileSize(movie.MoviePathAndFileName).ToString
                        Catch
                            strNFOprop = "0"
                        End Try

                    Case "folder_size"
                        Try
                            fi = New FileInfo(movie.fullpathandfilename)
                            strNFOprop = Utilities.GetFolderSize(fi.DirectoryName).ToString
                        Catch
                            strNFOprop = "0"
                        End Try

                    Case "folder"
                        fi = New FileInfo(movie.fullpathandfilename)
                        strNFOprop = fi.DirectoryName

                    Case "folder_no_drive"
                        fi = New FileInfo(movie.fullpathandfilename)
                        strNFOprop = Right(fi.DirectoryName, Len(fi.DirectoryName) - 2)

                    Case "imdb_url"
                        strNFOprop = If(movie.id <> Nothing, Pref.imdbmirror & "title/" & movie.id & "/", Pref.imdbmirror)

                    Case "title"
                        strNFOprop = If(movie.title <> Nothing, movie.title, "")
                        If Not String.IsNullOrEmpty(strNFOprop) And tokenInstr.Length > 1 Then
                            Dim M As Match = Regex.Match(movie.title, "^(?<article>The )?(?<title>.*?)$")
                            If M.Success Then
                                strNFOprop = M.Groups("title").Value.Trim
                                If tokenInstr(1).StartsWith("append") Then strNFOprop.AppendValue(M.Groups("article").Value.Trim)
                                If tokenInstr(1).StartsWith("article") Then strNFOprop = M.Groups("article").Value.Trim
                            End If
                        End If

                    Case "movieset" :
                        strNFOprop = movie.SetName

                    Case "release_date" :
                        strNFOprop = movie.MovieSet.Collection.Find(function(x) x.TmdbMovieId=movie.tmdbid).release_date

                   Case "movieyear"
                        strNFOprop = If(movie.year <> Nothing, movie.year, "0000")

                    Case "movietitleandyear"
                        strNFOprop = If(movie.title <> Nothing, movie.title, "") & " (" & If(movie.year <> Nothing, movie.year, "0000") & ")"

                    Case "rating"
                        strNFOprop = If(movie.rating <> Nothing, movie.rating.ToString(Globalization.CultureInfo.InvariantCulture), "")

                    Case "userrating"
                        strNFOprop = movie.usrrated.ToString

                    Case "runtime"
                        strNFOprop = If(movie.runtime <> Nothing, movie.runtime, "")

                    Case "outline"
                        strNFOprop = If(movie.outline <> Nothing, movie.outline, "")

                    Case "fullpathandfilename"
                        strNFOprop = If(movie.fullpathandfilename <> Nothing, movie.fullpathandfilename, "")

                    Case "filename"
                        Dim tempFileAndPath As String = Utilities.GetFileName(movie.fullpathandfilename)
                        Dim idxEndParam As Integer = tokenInstr.Length - 1
                        If idxEndParam > 0 Then
                            Dim arrlstFormat As New ArrayList
                            Dim separator As String = ""
                            Dim tempFilePathOnly As String = String.Concat(Path.GetDirectoryName(tempFileAndPath), Path.DirectorySeparatorChar)
                            Dim tempRoot As String = Path.GetPathRoot(tempFileAndPath)
                            For i = 0 To idxEndParam - 1
                                Select Case tokenInstr(i + 1).ToLower
                                    Case "root"
                                        arrlstFormat.Add(tempRoot)
                                    Case "path"
                                        arrlstFormat.Add(tempFilePathOnly.Replace(tempRoot, ""))
                                    Case "file"
                                        arrlstFormat.Add(Path.GetFileNameWithoutExtension(tempFileAndPath))
                                    Case "ext"
                                        arrlstFormat.Add(Path.GetExtension(tempFileAndPath))
                                End Select
                            Next
                            Dim arrFormat As String() = CType(arrlstFormat.ToArray(GetType(String)), String())
                            strNFOprop = String.Join(separator, arrFormat)
                        Else
                            strNFOprop = tempFileAndPath
                        End If

                    ' The tokens "fullplot", "director", "stars", "writer", "moviegenre" and "releasedate" are included for backwards compatibility
                    Case "fullplot", "director", "stars", "writer", "moviegenre", "releasedate", "actors", "format", "filename", "nfo"
                        Dim newplotdetails As New FullMovieDetails
                        newplotdetails = WorkingWithNfoFiles.mov_NfoLoadFull(movie.fullpathandfilename)
                        If Not IsNothing(newplotdetails) Then
                            If tokenInstr(0) = "fullplot"       Then strNFOprop = newplotdetails.fullmoviebody.plot
                            If tokenInstr(0) = "director"       Then strNFOprop = newplotdetails.fullmoviebody.director
                            If tokenInstr(0) = "stars"          Then strNFOprop = newplotdetails.fullmoviebody.stars
                            If tokenInstr(0) = "writer"         Then strNFOprop = newplotdetails.fullmoviebody.credits
                            If tokenInstr(0) = "moviegenre"     Then strNFOprop = newplotdetails.fullmoviebody.genre
                            If tokenInstr(0) = "releasedate"    Then strNFOprop = newplotdetails.fullmoviebody.premiered
                            If tokenInstr(0) = "actors" Then
                                Dim idxEndParam As Integer = tokenInstr.Length - 1
                                Dim numActors As Integer = 9999
                                If idxEndParam > 0 Then
                                    If IsNumeric(tokenInstr(idxEndParam)) Then
                                        numActors = Int(tokenInstr(idxEndParam))
                                        idxEndParam -= 1
                                    End If
                                    Dim count As Integer = 0
                                    For Each actor In newplotdetails.listactors
                                        count += 1
                                        If count > numActors Then Exit For
                                        preEscapeNFOprop &= "<actor>"
                                        For idx = 1 To idxEndParam
                                            Select Case tokenInstr(idx).ToLower
                                                Case "name"
                                                    preEscapeNFOprop &= String.Format("<name>{0}</name>", Security.SecurityElement.Escape(actor.actorname))
                                                Case "role"
                                                    preEscapeNFOprop &= String.Format("<role>{0}</role>", Security.SecurityElement.Escape(actor.actorrole))
                                                Case "thumb"
                                                    preEscapeNFOprop &= String.Format("<thumb>{0}</thumb>", Security.SecurityElement.Escape(actor.actorthumb))
                                            End Select
                                        Next
                                        preEscapeNFOprop &= "</actor>"
                                    Next
                                Else
                                    strNFOprop = newplotdetails.fullmoviebody.stars
                                End If
                            End If
                            If tokenInstr(0) = "format" Then
                                Dim idxEndParam As Integer = tokenInstr.Length - 1
                                If idxEndParam > 0 Then
                                    Dim arrlstFormat As New ArrayList
                                    Dim separator As String = " "
                                    For i = 0 To idxEndParam - 1
                                        Select Case tokenInstr(i + 1).ToLower
                                            Case "container"
                                                Dim container As String = newplotdetails.filedetails.Video.Container.Value
                                                If container <> Nothing And container <> "" Then
                                                    If tokenInstr(i + 1).ToLower <> tokenInstr(i + 1) Then container = container.ToUpper
                                                    arrlstFormat.Add(container.TrimStart("."))
                                                End If
                                            Case "source"
                                                Dim source As String = newplotdetails.fullmoviebody.source
                                                If source <> Nothing And source <> "" Then
                                                    arrlstFormat.Add(source)
                                                End If
                                            Case "resolution"
                                                Dim width, height As Integer
                                                width = newplotdetails.filedetails.Video.Width.Value
                                                height = newplotdetails.filedetails.Video.Height.Value
                                                If width AndAlso height Then
                                                    If (width <= 720 And height <= 480) Then
                                                        arrlstFormat.Add("480")
                                                    ElseIf (width <= 768 And height <= 576) Then
                                                        arrlstFormat.Add("576")
                                                    ElseIf (width <= 960 And height <= 544) Then
                                                        arrlstFormat.Add("540")
                                                    ElseIf (width <= 1280 And height <= 720) Then
                                                        arrlstFormat.Add("720")
                                                    ElseIf (width <= 1920 And height <= 1080) Then
                                                        arrlstFormat.Add("1080")
                                                    Else
                                                        arrlstFormat.Add("2160")
                                                    End If
                                                End If
                                        End Select
                                    Next
                                    Dim arrFormat As String() = CType(arrlstFormat.ToArray(GetType(String)), String())
                                    strNFOprop = String.Join(separator, arrFormat)
                                Else
                                    strNFOprop = newplotdetails.filedetails.Video.Container.Value
                                End If
                            End If
                            If tokenInstr(0) = "nfo" Then
                                Try
                                    Select Case tokenInstr(1)
                                        Case "file"
                                            Select Case tokenInstr(2)
                                                Case "video"
                                                    Select Case tokenInstr(3)
                                                        Case "width"
                                                            strNFOprop = newplotdetails.filedetails.Video.Width.Value
                                                        Case "height"
                                                            strNFOprop = newplotdetails.filedetails.Video.Height.Value
                                                        Case "aspect"
                                                            strNFOprop = newplotdetails.filedetails.Video.Aspect.Value
                                                        Case "codec"
                                                            strNFOprop = newplotdetails.filedetails.Video.Codec.Value
                                                        Case "duration"
                                                            strNFOprop = newplotdetails.filedetails.Video.DurationInSeconds.Value
                                                        Case "container"
                                                            strNFOprop = newplotdetails.filedetails.Video.Container.Value
                                                        Case Else
                                                            strNFOprop = tokenInstr(3) & "not supported"
                                                    End Select
                                                'strNFOprop = CallByName(newplotdetails.filedetails.Video, tokenInstr(3), vbGet)
                                                Case "audio"
                                                    Dim i As Integer = 1
                                                    For Each audioStream In newplotdetails.filedetails.Audio
                                                        Select Case tokenInstr(3)
                                                            Case "language"
                                                                strNFOprop &= audioStream.Language.Value
                                                            Case "channels"
                                                                strNFOprop &= audioStream.Channels.Value
                                                            Case "bitrate"
                                                                strNFOprop &= audioStream.Bitrate.Value
                                                            Case "codec"
                                                                strNFOprop &= audioStream.Codec.Value
                                                        End Select
                                                        'strNFOprop = strNFOprop & CallByName(audioStream, tokenInstr(3), vbGet)
                                                        If (newplotdetails.filedetails.Audio.Count > 1 And i <> newplotdetails.filedetails.Audio.Count) Then
                                                            strNFOprop = strNFOprop & " / "
                                                        End If
                                                        i += 1
                                                    Next
                                                Case "subtitles"
                                                    Dim i As Integer = 1
                                                    For Each subLang In newplotdetails.filedetails.Subtitles
                                                        Select Case tokenInstr(3)
                                                            Case "language"
                                                                strNFOprop &= subLang.Language.Value
                                                        End Select
                                                        'strNFOprop = strNFOprop & CallByName(subLang, tokenInstr(3), vbGet)
                                                        If (newplotdetails.filedetails.Subtitles.Count > 1 And i <> newplotdetails.filedetails.Subtitles.Count) Then
                                                            strNFOprop = strNFOprop & " / "
                                                        End If
                                                        i += 1
                                                    Next
                                                Case "filesize"
                                                    strNFOprop = Utilities.GetFileSize(movie.MoviePathAndFileName)
                                            End Select
                                        Case "title"
                                            strNFOprop = newplotdetails.fullmoviebody.title
                                        Case "originaltitle"
                                            strNFOprop = newplotdetails.fullmoviebody.originaltitle
                                        Case "sorttitle"
                                            strNFOprop = newplotdetails.fullmoviebody.sortorder
                                        Case "year"
                                            strNFOprop = newplotdetails.fullmoviebody.year
                                        Case "set"
                                            strNFOprop = If(newplotdetails.fullmoviebody.SetName = "-None-", "", newplotdetails.fullmoviebody.SetName)
                                        Case "createdate", "premiered"
                                            Dim newDate, localDatePattern As String
                                            If tokenInstr(1) = "createdate" Then
                                                newDate = newplotdetails.fileinfo.createdate
                                                localDatePattern = Pref.datePattern
                                            Else
                                                newDate = newplotdetails.fullmoviebody.premiered
                                                localDatePattern = Pref.nfoDatePattern
                                            End If
                                            Try
                                                Dim result As Date
                                                result = DateTime.ParseExact(newDate, localDatePattern, Nothing)
                                                If tokenInstr.Length > 2 Then
                                                    strNFOprop = Format(result, tokenInstr(2)).ToString
                                                    If tokenInstr.Length > 3 Then
                                                        Dim separator As String = "!"
                                                        Select Case tokenInstr(3).ToLower
                                                            Case "space"
                                                                separator = " "
                                                            Case "colon"
                                                                separator = ":"
                                                            Case "dash"
                                                                separator = "-"
                                                            Case "slash"
                                                                separator = "/"
                                                        End Select
                                                        strNFOprop = strNFOprop.Replace("_", separator)
                                                    End If
                                                Else
                                                    strNFOprop = Format(result, Pref.datePattern).ToString
                                                End If
                                            Catch ex As Exception
                                                strNFOprop = "Error in date format"
                                            End Try
                                        Case "rating"
                                            strNFOprop = newplotdetails.fullmoviebody.rating
                                        Case "userrating"
                                            strNFOprop = newplotdetails.fullmoviebody.usrrated
                                        Case "votes"
                                            strNFOprop = newplotdetails.fullmoviebody.votes
                                        Case "top250"
                                            strNFOprop = newplotdetails.fullmoviebody.top250
                                        Case "outline"
                                            strNFOprop = newplotdetails.fullmoviebody.outline
                                        Case "plot"
                                            strNFOprop = newplotdetails.fullmoviebody.plot
                                        Case "tagline"
                                            strNFOprop = newplotdetails.fullmoviebody.tagline
                                        Case "country"
                                            strNFOprop = newplotdetails.fullmoviebody.country
                                        Case "runtime"
                                            Dim M As Match = Regex.Match(newplotdetails.fullmoviebody.runtime.Trim, "^(?<rt>\d{1,3}).*?")
                                            strNFOprop = If(M.Success, M.Groups("rt").Value, "0")
                                        Case "mpaa"
                                            strNFOprop = newplotdetails.fullmoviebody.mpaa
                                        Case "genre"
                                            strNFOprop = newplotdetails.fullmoviebody.genre
                                        Case "credits"
                                            strNFOprop = newplotdetails.fullmoviebody.credits
                                        Case "director"
                                            strNFOprop = newplotdetails.fullmoviebody.director
                                        Case "studio"
                                            strNFOprop = newplotdetails.fullmoviebody.studio
                                        Case "trailer"
                                            strNFOprop = newplotdetails.fullmoviebody.trailer
                                        Case "playcount"
                                            strNFOprop = newplotdetails.fullmoviebody.playcount
                                        Case "id"
                                            strNFOprop = newplotdetails.fullmoviebody.imdbid
                                        Case "stars"
                                            strNFOprop = newplotdetails.fullmoviebody.stars
                                        Case "source"
                                            strNFOprop = newplotdetails.fullmoviebody.source
                                        Case Else
                                            strNFOprop = "No support for " & tokenInstr(1)
                                    End Select

                                    Select Case tokenInstr(1)
                                        Case "file", "createdate", "premiered", "filename", "actors"
                                            'Do nothing
                                        Case Else
                                            If tokenInstr.Length > 2 Then
                                                Dim intCharLimit = CInt(tokenInstr(2))
                                                If strNFOprop.Length > intCharLimit Then
                                                    strNFOprop = strNFOprop.Substring(0, strNFOprop.LastIndexOf(" ", intCharLimit - 3)) & "<font class=dim>...</font>"
                                                End If
                                            End If
                                    End Select

                                Catch
                                    strNFOprop = "Error in token"
                                End Try
                            End If
                        End If

                End Select
                Try
                    If IsNothing(strNFOprop) Then strNFOprop = ""
                    Select Case filetype
                        Case "xml"
                            strNFOprop = Security.SecurityElement.Escape(strNFOprop)    'this may be applicable to HTML too? - Huey
                            strNFOprop &= preEscapeNFOprop
                        Case "csv"
                            strNFOprop = strNFOprop.Replace(",", "")
                            strNFOprop = strNFOprop.Replace(Chr(34), "'")
                        Case Else
                            strNFOprop = strNFOprop.Replace(Chr(34), "&quot;")
                            strNFOprop &= preEscapeNFOprop
                    End Select
                    templPart = templPart.Replace(token.Value, strNFOprop)
                Catch
                    templPart = templPart.Replace(token.Value, "")
                End Try
            Next
            templatePopulated.Add(templPart)
        Next
        insertIndex = templatePopulated(0).Length

        Return String.Join("", templatePopulated)
    End Function

    Private Function getTagsTV(ByVal templ As String, ByVal tvShow As Media_Companion.TvShow, ByVal showCounter As Integer, ByVal imagepath As String, ByVal showcount As Integer, ByVal filetype As String, Optional ByRef insertIndex As Integer = 0)
        Dim inclShow As Boolean = False
        Dim templateParts As New List(Of String)
        Dim templatePopulated As New List(Of String)
        If insertIndex Then
            inclShow = True
            templateParts.Add(templ.Substring(0, insertIndex))
        End If
        templateParts.Add(templ.Substring(insertIndex))

        For Each templPart In templateParts

            Dim blockShow As String = templPart
            Dim blockSeason As String = ""
            Dim blockEpisode As String = ""
            Dim strMediaDoc As String = ""
            Dim counterSeason = 0
            If templPart.IndexOf("<<season") <> -1 And templPart.IndexOf("<</season>>") <> -1 Or templPart.IndexOf("<<episode") <> -1 And templPart.IndexOf("<</episode>>") <> -1 Then
                Dim setTVshows = New SortedList(Of String, TvEpisode)(New SeasonEpisodeComparer)
                Dim keySE As String
                Dim arrSeasonPresent(0 To 0) As Boolean
                Dim firstSeason As Integer = 99999
                Dim inclSeason As Boolean = False
                Dim inclEpisode As Boolean = False
                Dim inclMissingSeason As Boolean = False
                Dim inclMissingEpisode As Boolean = Pref.displayMissingEpisodes
                If templPart.IndexOf("<<season>>") <> -1 Or templPart.IndexOf("<<season:all>>") <> -1 Or templPart.IndexOf("<<episode>>") <> -1 Or templPart.IndexOf("<<episode:all>>") <> -1 Then
                    If templPart.IndexOf("<<season") <> -1 Then inclSeason = True
                    If templPart.IndexOf("<<episode") <> -1 Then inclEpisode = True
                    For Each episode In tvShow.Episodes
                        If episode.Season.Value <> "-1" And episode.Episode.Value <> "-1" Then
                            keySE = episode.Season.Value & "-" & episode.Episode.Value
                            If Not setTVshows.ContainsKey(keySE) Then setTVshows.Add(keySE, episode)
                            If episode.Season.Value > UBound(arrSeasonPresent) Then
                                ReDim Preserve arrSeasonPresent(episode.Season.Value)
                                arrSeasonPresent(episode.Season.Value) = True
                            End If
                            If episode.Season.Value < firstSeason Then
                                firstSeason = episode.Season.Value
                                If episode.Season.Value = 0 Then arrSeasonPresent(0) = True
                            End If
                        End If
                    Next
                End If
                If templPart.IndexOf("<<season:missing>>") <> -1 Or templPart.IndexOf("<<season:all>>") <> -1 Or templPart.IndexOf("<<episode:missing>>") <> -1 Or templPart.IndexOf("<<episode:all>>") <> -1 Then
                    If tvShow.MissingEpisodes.Count > 0 Then
                        If templPart.IndexOf("<<season") <> -1 Then inclMissingSeason = True
                        If templPart.IndexOf("<<episode") <> -1 Then inclMissingEpisode = True
                        For Each episode In tvShow.MissingEpisodes
                            If episode.Season.Value <> "-1" And episode.Episode.Value <> "-1" Then
                                keySE = episode.Season.Value & "-" & episode.Episode.Value
                                If Not setTVshows.ContainsKey(keySE) Then setTVshows.Add(keySE, episode)
                                If episode.Season.Value > UBound(arrSeasonPresent) Then
                                    ReDim Preserve arrSeasonPresent(episode.Season.Value)
                                    arrSeasonPresent(episode.Season.Value) = True
                                End If
                                If episode.Season.Value < firstSeason Then
                                    firstSeason = episode.Season.Value
                                    If episode.Season.Value = 0 Then arrSeasonPresent(0) = True
                                End If
                            End If
                        Next
                    End If
                End If
                If setTVshows.Count Then
                    Dim separator As String = "<<|separator|>>"
                    If inclSeason Or inclMissingSeason Then
                        blockSeason = templPart.Substring(templPart.IndexOf("<<season"), templPart.IndexOf("<</season>>") - templPart.IndexOf("<<season") + 11)
                        blockShow = blockShow.Replace(blockSeason, separator)
                    End If
                    If inclEpisode Or inclMissingEpisode Then
                        blockEpisode = templPart.Substring(templPart.IndexOf("<<episode"), templPart.IndexOf("<</episode>>") - templPart.IndexOf("<<episode") + 12)
                        If blockSeason <> "" Then
                            blockSeason = blockSeason.Replace(blockEpisode, separator)
                        Else
                            blockShow = blockShow.Replace(blockEpisode, separator)
                        End If
                        blockEpisode = blockEpisode.Substring(blockEpisode.IndexOf(">>") + 2, blockEpisode.IndexOf("<</episode>>") - blockEpisode.IndexOf(">>") - 2)
                    End If
                    If blockSeason <> "" Then
                        blockSeason = blockSeason.Substring(blockSeason.IndexOf(">>") + 2, blockSeason.IndexOf("<</season>>") - blockSeason.IndexOf(">>") - 2)
                    End If

                    Dim strMediaDocSeason As String = ""
                    Dim strMediaDocEpisode As String = ""
                    Dim strTempEpisode As String = ""
                    Dim strMediaDocSeasonSpecials As String = ""
                    Dim currSeason As Integer = firstSeason
                    Dim counterSeasonEpisodes = 0
                    Dim counterSeasonMissingEpisodes = 0
                    Dim counterSeasonTotalEpisodes = 0
                    Dim lastEpisode As Boolean = False
                    'Build string
                    For Each episode In setTVshows
                        If Not (episode.Value.IsMissing And Not inclMissingEpisode) Then
                            strTempEpisode = If(inclEpisode Or inclMissingEpisode, getTagsTVEpisode(blockEpisode, episode.Value, showCounter, counterSeasonTotalEpisodes), "")
                            inclShow = True
                        End If
                        If setTVshows.IndexOfKey(episode.Key) = setTVshows.Count - 1 Then   'This is the last episode
                            If strTempEpisode <> "" Then
                                strMediaDocEpisode = strMediaDocEpisode & strTempEpisode
                                counterSeasonTotalEpisodes += 1
                                If episode.Value.IsMissing Then
                                    counterSeasonMissingEpisodes += 1
                                Else
                                    counterSeasonEpisodes += 1
                                End If
                            End If
                            lastEpisode = True
                        End If
                        ' If season changes or reach end of sorted list, aggregate season and episode document
                        If lastEpisode Or episode.Value.Season.Value > currSeason Then
                            If inclSeason Or inclMissingSeason Then
                                strMediaDocSeason = getTagsTVSeason(blockSeason, tvShow, showCounter, currSeason, counterSeasonEpisodes, counterSeasonMissingEpisodes,
                                                                counterSeasonTotalEpisodes, arrSeasonPresent(currSeason), imagepath)
                                strMediaDocSeason = strMediaDocSeason.Replace(separator, If(inclMissingSeason And Not inclMissingEpisode Or inclSeason And Not inclEpisode,
                                                                                    "", strMediaDocEpisode))
                                inclShow = True
                            Else
                                strMediaDocSeason = strMediaDocEpisode
                            End If
                            If currSeason = 0 Then
                                strMediaDocSeasonSpecials = strMediaDocSeason
                            Else
                                strMediaDoc = strMediaDoc & strMediaDocSeason
                            End If
                            strMediaDocSeason = ""
                            strMediaDocEpisode = ""
                            currSeason = episode.Value.Season.Value
                            counterSeason += 1
                            counterSeasonEpisodes = 0
                            counterSeasonMissingEpisodes = 0
                            counterSeasonTotalEpisodes = 0
                        End If
                        If episode.Value.IsMissing Then
                            counterSeasonMissingEpisodes += 1
                        Else
                            counterSeasonEpisodes += 1
                        End If
                        counterSeasonTotalEpisodes += 1
                        strMediaDocEpisode = strMediaDocEpisode & strTempEpisode
                    Next
                    strMediaDoc = strMediaDoc & strMediaDocSeasonSpecials
                    blockShow = blockShow.Replace(separator, strMediaDoc)
                End If

            End If
            If inclShow Then
                templatePopulated.Add(getTagsTVShow(blockShow, tvShow, showCounter, counterSeason, imagepath))
            End If
        Next
        insertIndex = If(templatePopulated.Count, templatePopulated(0).Length, 0)

        Return String.Join("", templatePopulated)
    End Function

    Private Function getTagsTVShow(ByRef text As String, ByVal tvShow As TvShow, ByVal counter As Integer, ByVal numSeasons As Integer, Optional ByVal imagepath As String = "")
        Dim fullTVShowDetails As New TvShow
        fullTVShowDetails = nfoFunction.tvshow_NfoLoad(tvShow.NfoFilePath)
        If IsNothing(fullTVShowDetails) Then Return ""

        Dim tokenCol As MatchCollection
        Dim tokenRegExp As New Regex("<<[\w_:]+>>")
        tokenCol = tokenRegExp.Matches(text)
        Dim token As Match

        For Each token In tokenCol
            Dim strNFOprop As String = ""
            Dim valToken As String = token.Value.Substring(2, token.Value.Length - 4)
            Dim tokenInstr() As String = valToken.Split(":")
            Dim addText As Boolean = False
            Dim padNumber As Boolean = False
            If tokenInstr.Length > 1 Then
                If tokenInstr(1).IndexOf("text") <> -1 Then addText = True
                If tokenInstr(1).IndexOf("pad") <> -1 Then padNumber = True
            End If
            Select Case tokenInstr(0)
                Case "createimage"
                    If imagepath <> "" And tokenInstr.Length > 1 Then
                        Dim origImage As String = tvShow.ImagePoster.Path
                        Dim imageType As String = "poster"
                        If tokenInstr.Length > 2 AndAlso tokenInstr(2) = "banner" Then
                            imageType = "banner"
                            origImage = If(Pref.FrodoEnabled, tvShow.ImageBanner.Path, tvShow.ImageAllSeasons.Path)
                        End If

                        If (tokenInstr(UBound(tokenInstr)) <> "nopath") Then strNFOprop &= "tvimages/"
                        strNFOprop &= Utilities.createImage(origImage, tokenInstr(1), imagepath, imageType)
                    End If
                Case "show_title"
                    strNFOprop = tvShow.Title.Value
                Case "show_plot"
                    strNFOprop = tvShow.Plot.Value
                Case "show_year"
                    strNFOprop = tvShow.Year.Value
                Case "show_titleandyear"
                    strNFOprop = tvShow.TitleAndYear
                Case "show_imdbid"
                    strNFOprop = tvShow.ImdbId.Value
                Case "show_imdburl"
                    strNFOprop = If(tvShow.ImdbId <> Nothing, Pref.imdbmirror & "title/" & tvShow.ImdbId.Value & "/", Pref.imdbmirror)
                Case "show_tvdbid"
                    strNFOprop = tvShow.TvdbId.Value
                Case "show_tvdburl"
                    strNFOprop = If(tvShow.TvdbId <> Nothing, "http://thetvdb.com/?tab=series&id=" & tvShow.TvdbId.Value, "http://thetvdb.com/")
                Case "show_genre"
                    strNFOprop = tvShow.Genre.Value
                Case "show_episodeactorsource"
                    strNFOprop = tvShow.EpisodeActorSource.Value
                Case "show_language"
                    strNFOprop = tvShow.Language.Value
                Case "show_locked"
                    strNFOprop = tvShow.State
                Case "show_rating"
                    strNFOprop = If(tvShow.Rating <> Nothing, tvShow.Rating.Value & If(tvShow.Rating.Value.IndexOf(".") <> -1, "", ".0"), "")
                Case "show_sortorder"
                    strNFOprop = tvShow.SortOrder.Value
                Case "show_status"
                    strNFOprop = tvShow.Status.Value
                Case "show_count"
                    strNFOprop = If(Cache.TvCache.Shows.Count, Cache.TvCache.Shows.Count.ToString, "00")
                Case "show_counter"
                    strNFOprop = counter.ToString
                Case "show_seasons"
                    strNFOprop = numSeasons.ToString
                    If addText Then strNFOprop = strNFOprop & " Season" & If(numSeasons <> 1, "s", "")
                Case "class"
                    If tokenInstr.Length > 1 Then
                        If tokenInstr(1).IndexOf("row") <> -1 Then
                            strNFOprop = " class=show_row_" & If(counter Mod 2, "odd", "even")
                        Else
                            strNFOprop = " class=" & tokenInstr(1)
                        End If
                    End If
                Case "show_nfo"
                    Try
                        Select Case tokenInstr(1)
                            Case "id"
                                strNFOprop = fullTVShowDetails.TvdbId.Value
                            Case "state"
                                strNFOprop = fullTVShowDetails.State.value__
                            Case "title"
                                strNFOprop = fullTVShowDetails.Title.Value
                            Case "showtitle"
                                strNFOprop = fullTVShowDetails.Title.Value
                            Case "mpaa"
                                strNFOprop = fullTVShowDetails.Mpaa.Value
                            Case "plot"
                                strNFOprop = fullTVShowDetails.Plot.Value
                            Case "imdbid"
                                strNFOprop = fullTVShowDetails.ImdbId.Value
                            Case "status"
                                strNFOprop = fullTVShowDetails.Status.Value
                            Case "runtime"
                                strNFOprop = fullTVShowDetails.Runtime.Value
                            Case "rating"
                                strNFOprop = fullTVShowDetails.Rating.Value
                            Case "year"
                                strNFOprop = fullTVShowDetails.Year.Value
                            Case "premiered"
                                strNFOprop = fullTVShowDetails.Premiered.Value
                            Case "studio"
                                strNFOprop = fullTVShowDetails.Studio.Value
                            Case "genre"
                                strNFOprop = fullTVShowDetails.Genre.Value
                            Case "episodeguide"
                                strNFOprop = fullTVShowDetails.EpisodeGuideUrl.Value
                            Case "language"
                                strNFOprop = fullTVShowDetails.Language.Value
                            Case "sorttitle"
                                strNFOprop = fullTVShowDetails.SortTitle.Value
                            Case "episodeactorsource"
                                strNFOprop = fullTVShowDetails.EpisodeActorSource.Value
                            Case "tvshowactorsource"
                                strNFOprop = fullTVShowDetails.TvShowActorSource.Value
                            Case "sortorder"
                                strNFOprop = fullTVShowDetails.SortOrder.Value
                            Case "actor"                                        ' No support for actor list
                                strNFOprop = "No support"
                            Case "thumb"                                        ' No support for thumbnail list
                                strNFOprop = "No support"
                            Case "fanart"                                       ' No support for fanart list
                                strNFOprop = "No support"
                            Case Else
                                strNFOprop = CallByName(fullTVShowDetails, tokenInstr(1), vbGet)
                        End Select
                        If tokenInstr(1) <> "episodeguide" And tokenInstr.Length > 2 Then
                            Dim intCharLimit = CInt(tokenInstr(2))
                            If strNFOprop.Length > intCharLimit Then
                                strNFOprop = strNFOprop.Substring(0, strNFOprop.LastIndexOf(" ", intCharLimit - 3)) & "<font class=dim>...</font>"
                            End If
                        End If
                    Catch
                        strNFOprop = "Error in token"
                    End Try
            End Select
            Try
                strNFOprop = strNFOprop.Replace(Chr(34), "&quot;")
                text = text.Replace(token.Value, strNFOprop)
            Catch
                text = text.Replace(token.Value, "")
            End Try
        Next
        Return text
    End Function

    Private Function getTagsTVSeason(ByVal text As String, ByVal tvShow As TvShow, ByVal showCounter As Integer, _
                                     ByVal currSeason As Integer, ByVal numEpisodes As Integer, ByVal numMissingEpisodes As Integer, _
                                     ByVal numTotalEpisodes As Integer, ByVal seasonPresent As Boolean, Optional ByVal imagepath As String = "")
        Dim tokenCol As MatchCollection
        Dim tokenRegExp As New Regex("<<[\w_:]+>>")
        tokenCol = tokenRegExp.Matches(text)
        Dim token As Match

        For Each token In tokenCol
            Dim strNFOprop As String = ""
            Dim valToken As String = token.Value.Substring(2, token.Value.Length - 4)
            Dim tokenInstr() As String = valToken.Split(":")
            Dim addText As Boolean = False
            Dim padNumber As Boolean = False
            Dim specials As Boolean = False
            If tokenInstr.Length > 1 Then
                If tokenInstr(1).IndexOf("text") <> -1 Then addText = True
                If tokenInstr(1).IndexOf("pad") <> -1 Then padNumber = True
                If tokenInstr(1).IndexOf("special") <> -1 Then specials = True
            End If
            Select Case tokenInstr(0)
                Case "createimage"
                    If imagepath <> "" And tokenInstr.Length > 1 Then
                        Dim imagetest As String = tvShow.Seasons(currSeason).Poster.Path

                        If (tokenInstr(UBound(tokenInstr)) <> "nopath") Then strNFOprop &= "tvimages/"
                        strNFOprop &= Utilities.createImage(tvShow.Seasons(currSeason).Poster.Path, tokenInstr(1), imagepath)
                    End If
                Case "show_counter"
                    strNFOprop = showCounter.ToString
                Case "seas_number"
                    If currSeason = 0 And specials Then
                        Dim endOfPrevTag = text.LastIndexOf(">", text.IndexOf(token.Value)) + 1
                        text = text.Remove(endOfPrevTag, text.IndexOf(token.Value) - endOfPrevTag)
                        strNFOprop = "Specials"
                    Else
                        strNFOprop = If(currSeason.ToString.Length = 1 And padNumber, "0", "") & currSeason.ToString
                        If addText Then strNFOprop = strNFOprop & " Season" & If(numTotalEpisodes <> 1, "s", "")
                    End If
                Case "seas_episodes"
                    strNFOprop = If(numTotalEpisodes.ToString.Length = 1 And padNumber, "0", "") & numTotalEpisodes.ToString
                    If addText Then strNFOprop = strNFOprop & " Episode" & If(numTotalEpisodes <> 1, "s", "")
                Case "seas_episodesof"
                    strNFOprop = numEpisodes.ToString
                    If numEpisodes = numTotalEpisodes Then
                        ' Do nothing
                    ElseIf numMissingEpisodes Then
                        strNFOprop = strNFOprop & " of " & numTotalEpisodes.ToString
                    End If
                    If addText Then strNFOprop = strNFOprop & " Episode" & If(numTotalEpisodes <> 1, "s", "")
                Case "class"
                    If tokenInstr.Length > 1 Then
                        If tokenInstr(1).IndexOf("missing") <> -1 Then
                            ' Special case where we want to know if the season contains missing episodes
                            If tokenInstr(1).IndexOf("episode") <> -1 And numMissingEpisodes Then
                                strNFOprop = " class=missingseason"
                            ElseIf Not seasonPresent Then
                                strNFOprop = " class=" & tokenInstr(1)
                            End If
                        ElseIf tokenInstr(1).IndexOf("row") <> -1 Then
                            strNFOprop = " class=seas_row_" & If(currSeason Mod 2, "odd", "even")
                        Else
                            strNFOprop = " class=" & tokenInstr(1)
                        End If
                    End If
            End Select

            Try
                strNFOprop = strNFOprop.Replace(Chr(34), "&quot;")
                text = text.Replace(token.Value, strNFOprop)
            Catch
                text = text.Replace(token.Value, "")
            End Try
        Next
        Return text
    End Function

    Private Function getTagsTVEpisode(ByVal text As String, ByVal tvEpisode As TvEpisode, ByVal showCounter As Integer, ByVal episodeCounter As Integer, Optional ByVal imagepath As String = "")
        Dim tokenCol As MatchCollection
        Dim tokenRegExp As New Regex("<<[\w_:]+>>")
        tokenCol = tokenRegExp.Matches(text)
        Dim token As Match

        For Each token In tokenCol
            Dim strNFOprop As String = ""
            Dim valToken As String = token.Value.Substring(2, token.Value.Length - 4)
            Dim tokenInstr() As String = valToken.Split(":")
            Dim addText As Boolean = False
            Dim padNumber As Boolean = False
            If tokenInstr.Length > 1 Then
                If tokenInstr(1).IndexOf("text") <> -1 Then addText = True
                If tokenInstr(1).IndexOf("pad") <> -1 Then padNumber = True
            End If
            Select Case tokenInstr(0)
                Case "show_counter"
                    strNFOprop = showCounter.ToString
                Case "ep_title"
                    strNFOprop = tvEpisode.Title.Value
                Case "ep_season"
                    strNFOprop = If(tvEpisode.Season.Value.Length = 1 And padNumber, "0", "") & tvEpisode.Season.Value
                Case "ep_number"
                    strNFOprop = If(tvEpisode.Episode.Value.Length = 1 And padNumber, "0", "") & tvEpisode.Episode.Value
                Case "ep_rating"
                    strNFOprop = If(tvEpisode.Rating.Value <> Nothing, tvEpisode.Rating.Value & If(tvEpisode.Rating.Value.IndexOf(".") <> -1, "", ".0"), "")
                Case "ep_playcount"
                    strNFOprop = tvEpisode.PlayCount.Value
                Case "ep_imdbid"
                    strNFOprop = tvEpisode.ImdbId.Value
                Case "ep_imdburl"
                    strNFOprop = If(tvEpisode.ImdbId.Value <> Nothing, Pref.imdbmirror & "title/" & tvEpisode.ImdbId.Value & "/", Pref.imdbmirror)
                Case "ep_tvdbid"
                    strNFOprop = tvEpisode.TvdbId.Value
                Case "class"
                    If tokenInstr.Length > 1 Then
                        If tokenInstr(1).IndexOf("missing") <> -1 And Not tvEpisode.IsMissing Then
                            ' Do nothing
                        ElseIf tokenInstr(1).IndexOf("row") <> -1 Then
                            strNFOprop = " class=ep_row_" & If(episodeCounter Mod 2, "odd", "even")
                        Else
                            strNFOprop = " class=" & tokenInstr(1)
                        End If
                    End If
                Case "ep_nfo"
                    Dim TVEpisodeNFO As List(Of TvEpisode) = WorkingWithNfoFiles.ep_NfoLoad(tvEpisode.NfoFilePath)   'ep_NfoLoadGeneric(tvEpisode.NfoFilePath)
                    Dim fullTVEpisodeDetails As TvEpisode = TVEpisodeNFO(0)
                    Try
                        Select Case tokenInstr(1)
                            Case "file"
                                Select Case tokenInstr(2)
                                    Case "video"
                                        Select Case tokenInstr(3)
                                            Case "width"
                                                strNFOprop = fullTVEpisodeDetails.StreamDetails.Video.Width.Value
                                            Case "height"
                                                strNFOprop = fullTVEpisodeDetails.StreamDetails.Video.Height.Value
                                            Case "aspect"
                                                strNFOprop = fullTVEpisodeDetails.StreamDetails.Video.Aspect.Value
                                            Case "codec"
                                                strNFOprop = fullTVEpisodeDetails.StreamDetails.Video.Codec.Value
                                            Case "duration"
                                                strNFOprop = fullTVEpisodeDetails.StreamDetails.Video.DurationInSeconds.Value
                                            Case "container"
                                                strNFOprop = fullTVEpisodeDetails.StreamDetails.Video.Container.Value
                                            Case Else
                                                strNFOprop = tokenInstr(3) & "not supported"
                                        End Select
                                    Case "audio"
                                        Dim i As Integer = 1
                                        For Each audioStream In fullTVEpisodeDetails.StreamDetails.Audio
                                            Select Case tokenInstr(3)
                                                Case "language"
                                                    strNFOprop &= audioStream.Language.Value
                                                Case "channels"
                                                    strNFOprop &= audioStream.Channels.Value
                                                Case "bitrate"
                                                    strNFOprop &= audioStream.Bitrate.Value
                                                Case "codec"
                                                    strNFOprop &= audioStream.Codec.Value
                                            End Select
                                            If (fullTVEpisodeDetails.StreamDetails.Audio.Count > 1 And i <> fullTVEpisodeDetails.StreamDetails.Audio.Count) Then
                                                strNFOprop = strNFOprop & " / "
                                            End If
                                            i += 1
                                        Next
                                    Case "subtitles"
                                        Dim i As Integer = 1
                                        For Each subLang In fullTVEpisodeDetails.StreamDetails.Subtitles
                                            Select Case tokenInstr(3)
                                                Case "language"
                                                    strNFOprop &= subLang.Language.Value
                                            End Select
                                            If (fullTVEpisodeDetails.StreamDetails.Subtitles.Count > 1 And i <> fullTVEpisodeDetails.StreamDetails.Subtitles.Count) Then
                                                strNFOprop = strNFOprop & " / "
                                            End If
                                            i += 1
                                        Next
                                End Select
                            Case "title"
                                strNFOprop = fullTVEpisodeDetails.Title.Value
                            Case "season"
                                strNFOprop = fullTVEpisodeDetails.Season.Value
                            Case "episode"
                                strNFOprop = fullTVEpisodeDetails.Episode.Value
                            Case "aired"
                                Dim newDate As String = fullTVEpisodeDetails.Aired.Value
                                If tokenInstr.Length > 2 Then
                                    Try
                                        Dim result As Date
                                        result = DateTime.ParseExact(newDate, Pref.nfoDatePattern, Nothing)
                                        strNFOprop = Format(result, tokenInstr(2)).ToString
                                        If tokenInstr.Length > 3 Then
                                            Dim separator As String = "!"
                                            Select Case tokenInstr(3).ToLower
                                                Case "space"
                                                    separator = " "
                                                Case "colon"
                                                    separator = ":"
                                                Case "dash"
                                                    separator = "-"
                                                Case "slash"
                                                    separator = "/"
                                            End Select
                                            strNFOprop = strNFOprop.Replace("_", separator)
                                        End If
                                    Catch ex As Exception
                                        strNFOprop = "Error in date format"
                                    End Try
                                Else
                                    strNFOprop = newDate
                                End If
                            Case "plot"
                                strNFOprop = fullTVEpisodeDetails.Plot.Value
                            Case "playcount"
                                strNFOprop = fullTVEpisodeDetails.PlayCount.Value
                            Case "director"
                                strNFOprop = fullTVEpisodeDetails.Director.Value
                            Case "credits"
                                strNFOprop = fullTVEpisodeDetails.Credits.Value
                            Case "rating"
                                strNFOprop = fullTVEpisodeDetails.Rating.Value
                            Case "displayseason"
                                strNFOprop = fullTVEpisodeDetails.DisplaySeason.Value
                            Case "displayepisode"
                                strNFOprop = fullTVEpisodeDetails.DisplayEpisode.Value
                            Case "runtime"
                                strNFOprop = fullTVEpisodeDetails.Runtime.Value
                            Case "showid"
                                strNFOprop = fullTVEpisodeDetails.Id.Value
                            Case "uniqueid"
                                strNFOprop = fullTVEpisodeDetails.UniqueId.Value
                            Case "actor"                                        ' No support for actor list
                                strNFOprop = "No support"
                            Case "thumb"                                        ' No support for thumbnail list
                                strNFOprop = "No support"
                            Case "fanart"                                       ' No support for fanart list
                                strNFOprop = "No support"
                            Case Else
                                strNFOprop = "No support"
                        End Select
                        If tokenInstr.Length > 2 AndAlso tokenInstr(1) <> "file" AndAlso tokenInstr(1) <> "aired" Then
                            Dim intCharLimit = CInt(tokenInstr(2))
                            If strNFOprop.Length > intCharLimit Then
                                strNFOprop = strNFOprop.Substring(0, strNFOprop.LastIndexOf(" ", intCharLimit - 3)) & "<font class=dim>...</font>"
                            End If
                        End If
                    Catch
                        strNFOprop = "Error in token"
                    End Try
            End Select
            Try
                strNFOprop = strNFOprop.Replace(Chr(34), "&quot;")
                text = text.Replace(token.Value, strNFOprop)
            Catch
                text = text.Replace(token.Value, "")
            End Try
        Next
        Return text
    End Function

    Public Function setTemplate(ByVal selectedTemplate As String, Optional ByRef type As Integer = mediaType.None) As Boolean
        Dim returnCode As Boolean = False
        workingTemplate = templateList.Find(Function(item As mediaInfoExportTemplate) item.title = selectedTemplate.ToLower)
        If workingTemplate.type > mediaType.None Then   'if mediaType has been set, then we assume the template is valid!
            type = workingTemplate.type
            returnCode = True
        End If
        Return returnCode
    End Function

    Public Function getPossibleFileType() As String
        Dim M As Match = Regex.Match(workingTemplate.title, "(?<type>xml|csv)")
        Return If(M.Success, M.Groups("type").Value, "html")
    End Function

End Class
