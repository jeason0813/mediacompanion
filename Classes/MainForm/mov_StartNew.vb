Imports Media_Companion.Preferences
Imports Media_Companion.Movies
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Net
Imports System.IO


Public Class mov_StartNew
    Public Sub ex(ByVal scraperLog As String)
        Dim dft As New List(Of String)
        Dim moviepattern As String
        Dim tempint As Integer = 0
        Dim tempstring As String
        Dim errorcounter As Integer = 0
        Dim trailer As String = ""
        Dim newmoviecount As Integer = 0
        Dim dirinfo As String = String.Empty
        newMovieList.Clear()
        Dim newmoviefolders As New List(Of String)
        Dim progress As Integer = 0
        progress = 0
        Dim progresstext As String
        scraperLog = ""
        Dim dirpath As String = String.Empty

        scraperLog &= "MC " & Trim(System.Reflection.Assembly.GetExecutingAssembly.FullName.Split(",")(1)) & vbCrLf

        If Preferences.usefoldernames = True Then
            scraperLog &= "Using FOLDERNAMES to determine Movie Title...." & vbCrLf
        Else
            scraperLog &= "Using FILENAMES to determine Movie Title...." & vbCrLf
        End If


        If Preferences.movies_useXBMC_Scraper = True Then
            scraperLog &= "Using XBMC Scraper...." & vbCrLf
            Form1.mov_XBMCScrapingInitialization()
        Else
            scraperLog &= "Using MC IMDB Scraper" & vbCrLf

            If Form1.ProgressAndStatus1.cancel = True Then
                scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                Return
            End If

            Dim ProgressBase As String = ""
            ProgressBase = "Using MC IMDB scraper/"

            If Preferences.usefoldernames = True Then
                ProgressBase &= "FOLDERNAMES"

            Else
                ProgressBase &= "FILENAMES"

            End If

            'BckWrkScnMovies.ReportProgress(progress, ProgressBase)


            scraperLog &= "Starting Folder Scan" & vbCrLf & vbCrLf

            For Each moviefolder In movieFolders
                Dim hg As New IO.DirectoryInfo(moviefolder)
                If hg.Exists Then
                    scraperLog &= "Found Movie Folder: " & hg.FullName.ToString & vbCrLf
                    newmoviefolders.Add(moviefolder)
                    scraperLog &= "Checking for subfolders" & vbCrLf
                    Dim newlist As List(Of String)
                    Try
                        newlist = Utilities.EnumerateFolders(moviefolder, 6)
                        For Each subfolder In newlist
                            scraperLog = scraperLog & "Subfolder added :- " & subfolder.ToString & vbCrLf
                            newmoviefolders.Add(subfolder)
                        Next
                    Catch ex As Exception
#If SilentErrorScream Then
                        Throw ex
#End If
                    End Try
                End If
            Next
            scraperLog &= vbCrLf
            For Each moviefolder In Preferences.offlinefolders
                Dim hg As New IO.DirectoryInfo(moviefolder)
                If hg.Exists Then
                    scraperLog = scraperLog & "Found Offline Folder: " & hg.FullName.ToString & vbCrLf
                    'newmoviefolders.Add(moviefolder)
                    scraperLog = scraperLog & "Checking for subfolders" & vbCrLf
                    Dim newlist As List(Of String)
                    Try
                        newlist = Utilities.EnumerateFolders(moviefolder, 0)
                        For Each subfolder In newlist
                            'If subfolder.IndexOf(".actors") = -1 Then
                            scraperLog = scraperLog & "Subfolder added :- " & subfolder.ToString & vbCrLf
                            Dim temge22 As String = Utilities.GetLastFolder(subfolder & "\whatever") & ".avi"
                            Dim sTempFileName22 As String = IO.Path.Combine(subfolder, temge22)
                            Dim newtemp1 As String = sTempFileName22.Replace(IO.Path.GetExtension(sTempFileName22), ".nfo")
                            If Not IO.File.Exists(newtemp1) Then
                                If Not IO.File.Exists(IO.Path.Combine(subfolder, "tempoffline.ttt")) Then
                                    Dim sTempFileName As String = IO.Path.Combine(subfolder, "tempoffline.ttt")
                                    Dim fsTemp As New System.IO.FileStream(sTempFileName, IO.FileMode.Create)
                                    fsTemp.Close()
                                End If
                                If Not IO.File.Exists(sTempFileName22) Then
                                    Dim temge As String = Utilities.GetLastFolder(subfolder & "\whatever") & ".avi"
                                    Dim sTempFileName2 As String = IO.Path.Combine(subfolder, temge)
                                    Dim fsTemp2 As New System.IO.FileStream(sTempFileName2, IO.FileMode.Create)
                                    fsTemp2.Close()
                                End If
                                newmoviefolders.Add(subfolder)
                            End If
                            'End If
                        Next
                    Catch ex As Exception
#If SilentErrorScream Then
                        Throw ex
#End If
                    End Try
                End If
            Next
            scraperLog &= vbCrLf & "MC Found these Movies..." & vbCrLf
            Application.DoEvents()
            Dim mediacounter As Integer = newMovieList.Count
            For g = 0 To newmoviefolders.Count - 1
                If Form1.ProgressAndStatus1.cancel = True Then
                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                    Return
                End If
                Try
                    progress = ((100 / newmoviefolders.Count) * g) * 10
                    progresstext = "Scanning folder " & g + 1 & " of " & newmoviefolders.Count
                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                    If Form1.ProgressAndStatus1.cancel = True Then
                        scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                        Return
                    End If
                    For Each ext In Utilities.VideoExtensions
                        If Form1.ProgressAndStatus1.cancel = True Then
                            scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                            Return
                        End If
                        moviepattern = If((ext = "VIDEO_TS.IFO"), ext, "*" & ext)  'this bit adds the * for the extension search in mov_ListFiles2 if its not the string VIDEO_TS.IFO 

                        dirpath = newmoviefolders(g)
                        Dim dir_info As New System.IO.DirectoryInfo(dirpath)
                        Movies.listMovieFiles(dir_info, moviepattern, scraperLog)         'titlename is logged in here
                    Next

                    tempint = newMovieList.Count - mediacounter

                    scraperLog &= String.Format("{0} New movies found in directory:- {1}", tempint.ToString, dirpath) & vbCrLf
                    mediacounter = newMovieList.Count
                Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
                End Try

            Next g
            scraperLog &= String.Format("{0}{0}!!! MC found {1} NEW Movie{2}{0}", vbCrLf, newMovieList.Count, If(newMovieList.Count = 1, "", "s"))

            newmoviecount = newMovieList.Count.ToString
            scraperLog = scraperLog & vbCrLf & vbCrLf & "Starting Main Scraper Process" & vbCrLf & vbCrLf

            For f = 0 To newMovieList.Count - 1
                If Form1.ProgressAndStatus1.cancel = True Then
                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                    Return
                End If
                Dim stage As Integer = 0
                Dim bodyok As Boolean = True
                'stage 0 = starting scraper
                'Try
                Dim title As String = ""
                Dim nfopath As String = ""
                Dim movieyear As String = String.Empty
                Dim fanartpath As String = ""
                Dim posterpath As String = ""
                Dim thumbstring As New XmlDocument
                progress = ((100 / newmoviecount) * (f + 1) * 10)
                progresstext = ProgressBase & String.Concat(" Scraping " & f + 1 & " of " & newmoviecount)
                Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                If newMovieList(f).title = Nothing Then
                    scraperLog = scraperLog & "!!! No Filename found for" & newMovieList(f).nfopathandfilename & vbCrLf
                End If
                If newMovieList(f).title <> Nothing Then
                    title = Utilities.CleanFileName(newMovieList(f).title, False)
                    scraperLog = scraperLog & "Scraping Title:- " & newMovieList(f).title & vbCrLf
                    scraperLog = scraperLog & "         (cleaned):- " & title & vbCrLf
                    progresstext &= " - " & newMovieList(f).title
                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                    Application.DoEvents()
                    nfopath = newMovieList(f).nfopathandfilename
                    If Preferences.basicsavemode = True Then
                        nfopath = newMovieList(f).nfopathandfilename.Replace(IO.Path.GetFileName(newMovieList(f).nfopathandfilename), "movie.nfo")
                    End If

                    posterpath = Preferences.GetPosterPath(nfopath)
                    fanartpath = Preferences.GetFanartPath(nfopath)

                    Dim extrapossibleID As String = Movies.getExtraIdFromNFO(nfopath, scraperLog)
                    If extrapossibleID IsNot Nothing Then
                        progresstext &= " - " & extrapossibleID
                        Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                    Else
                        scraperLog &= "Checking filename for:" & vbCrLf
                        scraperLog &= "    IMDB ID - "
                        Dim mat As Match = Nothing
                        Dim T As String = newMovieList(f).nfopathandfilename
                        mat = Regex.Match(T, "(tt\d{7})")
                        If mat.Success = True Then
                            scraperLog &= mat.Value
                            progresstext &= " - " & mat.Value
                            Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                            extrapossibleID = mat.Value
                        Else
                            scraperLog &= "None" & vbCrLf
                            scraperLog &= "    Movie Year - "
                            Dim M As Match
                            M = Regex.Match(newMovieList(f).nfopathandfilename, "[\(\[]([\d]{4})[\)\]]")
                            If M.Success = True Then
                                movieyear = M.Groups(1).Value
                                scraperLog &= movieyear
                            Else
                                scraperLog &= "None"
                            End If
                        End If
                        scraperLog &= vbCrLf
                    End If
                    progresstext &= String.Format(" - using '{0}{1}'", title, If(String.IsNullOrEmpty(movieyear), "", " " & movieyear))
                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)

                    Dim newmovie As New FullMovieDetails
                    Dim scraperfunction As New Classimdb
                    Dim body As String
                    Dim actorlist As String
                    Dim certificates As New List(Of String)

                    'Get movie body
                    stage = 1

                    If Form1.ProgressAndStatus1.cancel = True Then
                        scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                        Return
                    End If

                    Form1.imdbCounter += 1
                    progresstext &= " * Now Scraping..."
                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)

                    body = scraperfunction.getimdbbody(title, movieyear, extrapossibleID, Preferences.imdbmirror, Form1.imdbCounter)
                    If Form1.ProgressAndStatus1.cancel = True Then
                        scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                        Return
                    End If
                    Dim thisresult As XmlNode = Nothing
                    If body = "MIC" Then
                        progresstext &= "!!! - ERROR!, please add Movie Manually!"
                        Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                        scraperLog = scraperLog & "!!! Unable to scrape body with refs """ & title & """, """ & movieyear & """, """ & extrapossibleID & """, """ & Preferences.imdbmirror & """" & vbCrLf
                        If Form1.imdbCounter < 50 Then
                            scraperLog = scraperLog & "Searching using Google" & vbCrLf
                        Else
                            scraperLog = scraperLog & "!!! Google session limit reached, Searching using IMDB" & vbCrLf
                        End If
                        scraperLog = scraperLog & "To add the movie manually, go to the movie edit page and select ""Change Movie"" to manually select the correct movie" & vbCrLf
                        newmovie.fullmoviebody.genre = "problem"
                        If newmovie.fullmoviebody.title = Nothing Then
                            newmovie.fullmoviebody.title = "Unknown Title"
                        End If
                        If newmovie.fullmoviebody.title = "" Then
                            newmovie.fullmoviebody.title = "Unknown Title"
                        End If
                        If newmovie.fullmoviebody.year = Nothing Then
                            newmovie.fullmoviebody.year = "0000"
                        End If
                        If newmovie.fullmoviebody.rating = Nothing Then
                            newmovie.fullmoviebody.rating = "0"
                        End If
                        If newmovie.fullmoviebody.top250 = Nothing Then
                            newmovie.fullmoviebody.top250 = "0"
                        End If
                        If newmovie.fullmoviebody.playcount = Nothing Then
                            newmovie.fullmoviebody.playcount = "0"
                        End If
                        If newmovie.fullmoviebody.title = "Unknown Title" Then
                            newmovie.fullmoviebody.plot = "This Movie has could not be identified by Media Companion, to add the movie manually, go to the movie edit page and select ""Change Movie"" to manually select the correct movie"
                            If title <> Nothing Then
                                If title = "" Then
                                    title = "Unknown Title"
                                End If
                            Else
                                title = "Unknown Title"
                            End If
                            newmovie.fullmoviebody.title = title

                        End If
                        If newmovie.fullmoviebody.title = "Unknown Title" Then
                            newmovie.fullmoviebody.genre = "Problem"
                        End If
                        If Form1.ProgressAndStatus1.cancel = True Then
                            scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                            Return
                        End If
                        Dim myDate2 As Date = System.DateTime.Now
                        Try
                            newmovie.fileinfo.createdate = Format(myDate2, datePattern).ToString
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try
                        Form1.nfoFunction.mov_NfoSave(nfopath, newmovie, True)

                        Dim movietoadd As New ComboList(SetDefaults)
                        movietoadd.fullpathandfilename = nfopath
                        movietoadd.filename = IO.Path.GetFileName(newMovieList(f).nfopathandfilename)
                        movietoadd.foldername = Utilities.GetLastFolder(newMovieList(f).nfopathandfilename)
                        movietoadd.title = newmovie.fullmoviebody.title
                        If newmovie.fullmoviebody.title <> Nothing Then
                            If newmovie.fullmoviebody.year <> Nothing Then
                                movietoadd.titleandyear = newmovie.fullmoviebody.title & " (" & newmovie.fullmoviebody.year & ")"
                            Else
                                movietoadd.titleandyear = newmovie.fullmoviebody.title & " (0000)"
                            End If
                        Else
                            movietoadd.titleandyear = "Unknown (0000)"
                        End If

                        movietoadd.year = newmovie.fullmoviebody.year

                        Dim filecreation As New IO.FileInfo(newMovieList(f).nfopathandfilename)
                        Dim myDate As Date = filecreation.LastWriteTime
                        Try
                            movietoadd.filedate = Format(myDate, datePattern).ToString
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try
                        myDate2 = System.DateTime.Now
                        Try
                            movietoadd.createdate = Format(myDate2, datePattern).ToString
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try
                        movietoadd.sortorder = newmovie.fullmoviebody.title
                        movietoadd.originaltitle = newmovie.fullmoviebody.title
                        movietoadd.outline = newmovie.fullmoviebody.outline
                        movietoadd.plot = newmovie.fullmoviebody.plot
                        movietoadd.id = newmovie.fullmoviebody.imdbid
                        movietoadd.rating = newmovie.fullmoviebody.rating
                        movietoadd.top250 = newmovie.fullmoviebody.top250
                        movietoadd.genre = newmovie.fullmoviebody.genre
                        movietoadd.playcount = newmovie.fullmoviebody.playcount
                        movietoadd.missingdata1 = 3
                        movietoadd.runtime = "0"
                        Form1.fullMovieList.Add(movietoadd)
                        If Form1.ProgressAndStatus1.cancel = True Then
                            scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                            Return
                        End If

                    Else
                        Try
                            progresstext &= " - OK!"           'movie scraped OK
                            Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                            scraperLog = scraperLog & "Movie Body Scraped OK" & vbCrLf
                            thumbstring.LoadXml(body)
                            For Each thisresult In thumbstring("movie")
                                Select Case thisresult.Name
                                    Case "title"
                                        If Preferences.keepfoldername = False Then
                                            newmovie.fullmoviebody.title = thisresult.InnerText
                                        Else
                                            If Preferences.usefoldernames = False Then
                                                tempstring = IO.Path.GetFileName(newMovieList(f).nfopathandfilename)
                                                newmovie.fullmoviebody.title = Utilities.CleanFileName(tempstring, False)
                                            Else
                                                newmovie.fullmoviebody.title = Utilities.CleanFileName(Utilities.GetLastFolder(newMovieList(f).nfopathandfilename), False)
                                            End If
                                        End If
                                    Case "originaltitle"
                                        newmovie.fullmoviebody.originaltitle = thisresult.InnerText
                                    Case "alternativetitle"
                                        newmovie.alternativetitles.Add(thisresult.InnerText)
                                    Case "country"
                                        newmovie.fullmoviebody.country = thisresult.InnerText
                                    Case "credits"
                                        newmovie.fullmoviebody.credits = thisresult.InnerText
                                    Case "director"
                                        newmovie.fullmoviebody.director = thisresult.InnerText
                                    Case "stars"
                                        newmovie.fullmoviebody.stars = thisresult.InnerText.ToString.Replace(", See full cast and crew", "")
                                    Case "genre"
                                        Dim strarr() As String
                                        strarr = thisresult.InnerText.Split("/")
                                        For count = 0 To strarr.Length - 1
                                            strarr(count) = strarr(count).Replace(" ", "")
                                        Next
                                        If strarr.Length <= Preferences.maxmoviegenre Then
                                            newmovie.fullmoviebody.genre = thisresult.InnerText
                                        Else
                                            For g = 0 To Preferences.maxmoviegenre - 1
                                                If g = 0 Then
                                                    newmovie.fullmoviebody.genre = strarr(g)
                                                Else
                                                    newmovie.fullmoviebody.genre += " / " & strarr(g)
                                                End If
                                            Next
                                        End If
                                    Case "mpaa"
                                        newmovie.fullmoviebody.mpaa = thisresult.InnerText
                                    Case "outline"
                                        newmovie.fullmoviebody.outline = thisresult.InnerText
                                    Case "plot"
                                        newmovie.fullmoviebody.plot = thisresult.InnerText
                                    Case "premiered"
                                        newmovie.fullmoviebody.premiered = thisresult.InnerText
                                    Case "rating"
                                        newmovie.fullmoviebody.rating = thisresult.InnerText
                                    Case "runtime"
                                        newmovie.fullmoviebody.runtime = thisresult.InnerText
                                        If newmovie.fullmoviebody.runtime.IndexOf(":") <> -1 Then
                                            Try
                                                newmovie.fullmoviebody.runtime = newmovie.fullmoviebody.runtime.Substring(newmovie.fullmoviebody.runtime.IndexOf(":") + 1, newmovie.fullmoviebody.runtime.Length - newmovie.fullmoviebody.runtime.IndexOf(":") - 1)
                                            Catch ex As Exception
#If SilentErrorScream Then
                                                Throw ex
#End If
                                            End Try
                                        End If
                                    Case "studio"
                                        newmovie.fullmoviebody.studio = thisresult.InnerText
                                    Case "tagline"
                                        newmovie.fullmoviebody.tagline = thisresult.InnerText
                                    Case "top250"
                                        newmovie.fullmoviebody.top250 = thisresult.InnerText
                                    Case "votes"
                                        newmovie.fullmoviebody.votes = thisresult.InnerText
                                    Case "year"
                                        newmovie.fullmoviebody.year = thisresult.InnerText
                                    Case "id"
                                        newmovie.fullmoviebody.imdbid = thisresult.InnerText
                                    Case "cert"
                                        certificates.Add(thisresult.InnerText)
                                End Select
                            Next
                            ' If plot is empty, use outline
                            If newmovie.fullmoviebody.plot = "" Then newmovie.fullmoviebody.plot = newmovie.fullmoviebody.outline

                        Catch ex As Exception
                            scraperLog = scraperLog & "!!! Error with " & newMovieList(f).nfopathandfilename & vbCrLf
                            scraperLog = scraperLog & "!!! An error was encountered at stage 1, Downloading Movie Body" & vbCrLf
                            scraperLog = scraperLog & ex.Message.ToString & vbCrLf & vbCrLf
                            errorcounter += 1
                            If Preferences.usefoldernames = False Then
                                tempstring = IO.Path.GetFileName(newMovieList(f).nfopathandfilename)
                                newmovie.fullmoviebody.title = Utilities.CleanFileName(tempstring, False)

                            Else
                                newmovie.fullmoviebody.title = Utilities.CleanFileName(Utilities.GetLastFolder(newMovieList(f).nfopathandfilename), False)

                            End If
                        End Try
                        If newmovie.fullmoviebody.playcount = Nothing Then newmovie.fullmoviebody.playcount = "0"
                        If newmovie.fullmoviebody.top250 = Nothing Then newmovie.fullmoviebody.top250 = "0"

                        Dim done As Boolean = False
                        For g = 0 To UBound(Preferences.certificatepriority)
                            For Each cert In certificates
                                If cert.IndexOf(Preferences.certificatepriority(g)) <> -1 Then
                                    newmovie.fullmoviebody.mpaa = cert.Substring(cert.IndexOf("|") + 1, cert.Length - cert.IndexOf("|") - 1)
                                    done = True
                                    Exit For
                                End If
                            Next
                            If done = True Then Exit For
                        Next
                        If Preferences.keepfoldername = True Then
                            If Preferences.usefoldernames = False Then
                                tempstring = IO.Path.GetFileName(newMovieList(f).nfopathandfilename)
                                newmovie.fullmoviebody.title = Utilities.CleanFileName(tempstring)

                            Else
                                newmovie.fullmoviebody.title = Utilities.CleanFileName(Utilities.GetLastFolder(newMovieList(f).nfopathandfilename))

                            End If
                        End If

                        '******************************** MOVIE FILE RENAME SECTION *************************************
                        If Preferences.MovieRenameEnable = True AndAlso Preferences.usefoldernames = False AndAlso newMovieList(f).nfopathandfilename.ToLower.Contains("video_ts") = False AndAlso Preferences.basicsavemode = False Then
                            Dim movieFileDetails As str_NewMovie = newMovieList(f)
                            scraperLog &= Movies.fileRename(newmovie.fullmoviebody, movieFileDetails)
                            newMovieList.RemoveAt(f)                        'remove old record
                            newMovieList.Insert(f, movieFileDetails)        'reinsert
                            nfopath = movieFileDetails.nfopathandfilename   'adjust nfopath variables
                            posterpath = Preferences.GetPosterPath(nfopath)
                            fanartpath = Preferences.GetFanartPath(nfopath)
                        End If
                        '******************************** END MOVIE FILE RENAME SECTION *************************************

                        scraperLog = scraperLog & "Output filename:- " & nfopath & vbCrLf
                        scraperLog = scraperLog & "Poster Path:- " & posterpath & vbCrLf
                        scraperLog = scraperLog & "Fanart Path:- " & fanartpath & vbCrLf & vbCrLf

                        stage = 2
                        'stage 2 = get movie actors
                        progresstext &= " * Actors"
                        Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                        actorlist = scraperfunction.getimdbactors(Preferences.imdbmirror, newmovie.fullmoviebody.imdbid)
                        Try
                            thumbstring.LoadXml(actorlist)
                            thisresult = Nothing
                            Dim actorcount As Integer = 0
                            For Each thisresult In thumbstring("actorlist")
                                Select Case thisresult.Name
                                    Case "actor"
                                        If actorcount > Preferences.maxactors Then
                                            Exit For
                                        End If
                                        actorcount += 1
                                        Dim newactor As New str_MovieActors
                                        Dim detail As XmlNode = Nothing
                                        For Each detail In thisresult.ChildNodes
                                            Select Case detail.Name
                                                Case "name"
                                                    newactor.actorname = detail.InnerText
                                                Case "role"
                                                    newactor.actorrole = detail.InnerText
                                                Case "thumb"
                                                    newactor.actorthumb = detail.InnerText
                                                Case "actorid"
                                                    If newactor.actorthumb <> Nothing Then
                                                        If detail.InnerText <> "" And Preferences.actorseasy = True Then
                                                            Dim workingpath As String = newMovieList(f).nfopathandfilename.Replace(IO.Path.GetFileName(newMovieList(f).nfopathandfilename), "")
                                                            workingpath = workingpath & ".actors\"
                                                            Dim hg As New IO.DirectoryInfo(workingpath)
                                                            Dim destsorted As Boolean = False
                                                            If Not hg.Exists Then
                                                                Try
                                                                    IO.Directory.CreateDirectory(workingpath)
                                                                    destsorted = True
                                                                Catch ex As Exception
#If SilentErrorScream Then
                                                                    Throw ex
#End If
                                                                End Try
                                                            Else
                                                                destsorted = True
                                                            End If
                                                            If destsorted = True Then
                                                                Dim filename As String = newactor.actorname.Replace(" ", "_")
                                                                filename = filename & ".tbn"
                                                                filename = IO.Path.Combine(workingpath, filename)
                                                                If Not IO.File.Exists(filename) Then
                                                                    Utilities.DownloadImage(newactor.actorthumb, filename)
                                                                    'Try
                                                                    '    Dim buffer(4000000) As Byte
                                                                    '    Dim size As Integer = 0
                                                                    '    Dim bytesRead As Integer = 0
                                                                    '    Dim thumburl As String = newactor.actorthumb
                                                                    '    Dim req As HttpWebRequest = WebRequest.Create(thumburl)
                                                                    '    Dim res As HttpWebResponse = req.GetResponse()
                                                                    '    Dim contents As Stream = res.GetResponseStream()
                                                                    '    Dim bytesToRead As Integer = CInt(buffer.Length)
                                                                    '    While bytesToRead > 0
                                                                    '        size = contents.Read(buffer, bytesRead, bytesToRead)
                                                                    '        If size = 0 Then Exit While
                                                                    '        bytesToRead -= size
                                                                    '        bytesRead += size
                                                                    '    End While

                                                                    '    Dim fstrm As New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write)
                                                                    '    fstrm.Write(buffer, 0, bytesRead)
                                                                    '    contents.Close()
                                                                    '    fstrm.Close()
                                                                    'Catch
                                                                    'End Try
                                                                End If
                                                            End If
                                                        End If
                                                        If Preferences.actorsave = True And detail.InnerText <> "" And Preferences.actorseasy = False Then
                                                            Dim workingpath As String = ""
                                                            Dim networkpath As String = Preferences.actorsavepath
                                                            Try
                                                                tempstring = networkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2)
                                                                Dim hg As New IO.DirectoryInfo(tempstring)
                                                                If Not hg.Exists Then
                                                                    IO.Directory.CreateDirectory(tempstring)
                                                                End If
                                                                workingpath = networkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "\" & detail.InnerText & ".jpg"
                                                                If Not IO.File.Exists(workingpath) Then
                                                                    Utilities.DownloadImage(newactor.actorthumb, workingpath)
                                                                    'Dim buffer(4000000) As Byte
                                                                    'Dim size As Integer = 0
                                                                    'Dim bytesRead As Integer = 0
                                                                    'Dim thumburl As String = newactor.actorthumb
                                                                    'Dim req As HttpWebRequest = WebRequest.Create(thumburl)
                                                                    'Dim res As HttpWebResponse = req.GetResponse()
                                                                    'Dim contents As Stream = res.GetResponseStream()
                                                                    'Dim bytesToRead As Integer = CInt(buffer.Length)
                                                                    'While bytesToRead > 0
                                                                    '    size = contents.Read(buffer, bytesRead, bytesToRead)
                                                                    '    If size = 0 Then Exit While
                                                                    '    bytesToRead -= size
                                                                    '    bytesRead += size
                                                                    'End While

                                                                    'Dim fstrm As New FileStream(workingpath, FileMode.OpenOrCreate, FileAccess.Write)
                                                                    'fstrm.Write(buffer, 0, bytesRead)
                                                                    'contents.Close()
                                                                    'fstrm.Close()
                                                                End If
                                                                newactor.actorthumb = IO.Path.Combine(Preferences.actornetworkpath, detail.InnerText.Substring(detail.InnerText.Length - 2, 2))
                                                                If Preferences.actornetworkpath.IndexOf("/") <> -1 Then
                                                                    newactor.actorthumb = Preferences.actornetworkpath & "/" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "/" & detail.InnerText & ".jpg"
                                                                Else
                                                                    newactor.actorthumb = Preferences.actornetworkpath & "\" & detail.InnerText.Substring(detail.InnerText.Length - 2, 2) & "\" & detail.InnerText & ".jpg"
                                                                End If
                                                            Catch
                                                            End Try
                                                        End If
                                                    End If
                                            End Select
                                        Next
                                        newmovie.listactors.Add(newactor)

                                End Select
                            Next
                            progresstext &= " - OK!"                                    'actors scraped OK
                            Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                            scraperLog = scraperLog & "Actors scraped OK" & vbCrLf
                            While newmovie.listactors.Count > Preferences.maxactors
                                newmovie.listactors.RemoveAt(newmovie.listactors.Count - 1)
                            End While
                            For Each actor In newmovie.listactors
                                Dim actornew As New str_ActorDatabase(SetDefaults)
                                actornew.actorname = actor.actorname
                                actornew.movieid = newmovie.fullmoviebody.imdbid
                                Form1.actorDB.Add(actornew)
                            Next
                        Catch ex As Exception
                            scraperLog = scraperLog & "!!! Error with " & newMovieList(f).nfopathandfilename & vbCrLf
                            scraperLog = scraperLog & "!!! An error was encountered at stage 2, Downloading Actors" & vbCrLf
                            scraperLog = scraperLog & ex.Message.ToString & vbCrLf & vbCrLf
                            errorcounter += 1
                            newmovie.listactors.Clear()
                        End Try


                        stage = 3
                        'stage 3 = get movie trailer

                        If Form1.ProgressAndStatus1.cancel = True Then
                            scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                            Return
                        End If

                        Try
                            If Preferences.gettrailer = True Then
                                progresstext &= " * Trailer"
                                Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)

                                trailer = ""

                                If Preferences.moviePreferredTrailerResolution.ToUpper() <> "SD" Then
                                    trailer = MC_Scraper_Get_HD_Trailer_URL(Preferences.moviePreferredTrailerResolution, newmovie.fullmoviebody.title)
                                End If

                                If trailer = "" Then
                                    trailer = scraperfunction.gettrailerurl(newmovie.fullmoviebody.imdbid, Preferences.imdbmirror)
                                End If


                                '                                   If trailer <> Nothing Then
                                If trailer <> String.Empty And trailer <> "Error" Then
                                    newmovie.fullmoviebody.trailer = trailer
                                    progresstext &= " - OK"
                                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                    scraperLog = scraperLog & "Trailer URL Scraped OK" & vbCrLf
                                Else
                                    progresstext &= " - Failed"
                                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                    scraperLog = scraperLog & "!!! Trailer URL Scrape failed" & vbCrLf
                                End If
                            End If
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try
                        stage = 4
                        'stage 4 = get movie thumblist(for nfo)
                        If Form1.ProgressAndStatus1.cancel = True Then
                            scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                            Return
                        End If

                        If Preferences.nfoposterscraper <> 0 Then
                            progresstext &= " * Thumb"
                            Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                            Dim thumbs As String = ""
                            If Preferences.nfoposterscraper = 1 Or Preferences.nfoposterscraper = 3 Or Preferences.nfoposterscraper = 5 Or Preferences.nfoposterscraper = 7 Or Preferences.nfoposterscraper = 9 Or Preferences.nfoposterscraper = 11 Or Preferences.nfoposterscraper = 13 Or Preferences.nfoposterscraper = 15 Then
                                Dim newobject3 As New IMPA.getimpaposters
                                Dim teststring As New XmlDocument
                                Dim testthumbs As String
                                Try
                                    testthumbs = newobject3.getimpathumbs(newmovie.fullmoviebody.title, newmovie.fullmoviebody.year)
                                    Dim testthumbs2 As String = "<totalthumbs>" & testthumbs & "</totalthumbs>"
                                    teststring.LoadXml(testthumbs2)
                                    thumbs = thumbs & testthumbs.ToString
                                Catch ex As Exception
#If SilentErrorScream Then
                                    Throw ex
#End If
                                    Thread.Sleep(1)
                                End Try
                            End If

                            If Preferences.nfoposterscraper = 2 Or Preferences.nfoposterscraper = 3 Or Preferences.nfoposterscraper = 6 Or Preferences.nfoposterscraper = 7 Or Preferences.nfoposterscraper = 10 Or Preferences.nfoposterscraper = 11 Or Preferences.nfoposterscraper = 14 Or Preferences.nfoposterscraper = 15 Then
                                Dim newobject2 As New tmdb_posters.Class1
                                Dim teststring As New XmlDocument
                                Dim testthumbs As String
                                Try
                                    testthumbs = newobject2.gettmdbposters_newapi(newmovie.fullmoviebody.imdbid)
                                    Dim bannerslist As New XmlDocument
                                    bannerslist.LoadXml(testthumbs)
                                    Dim templist As String = ""
                                    For Each item In bannerslist("tmdb_posterlist")
                                        Select Case item.name
                                            Case "poster"
                                                For Each img In item
                                                    If img.childnodes(0).innertext = "original" Then
                                                        templist = templist & "<thumbs>" & img.childnodes(1).innertext & "</thumbs>"
                                                    End If
                                                Next
                                        End Select
                                    Next
                                    thumbs = thumbs & templist.ToString
                                Catch ex As Exception
#If SilentErrorScream Then
                                    Throw ex
#End If
                                    Thread.Sleep(1)
                                End Try
                            End If

                            If Preferences.nfoposterscraper = 4 Or Preferences.nfoposterscraper = 5 Or Preferences.nfoposterscraper = 6 Or Preferences.nfoposterscraper = 7 Or Preferences.nfoposterscraper = 12 Or Preferences.nfoposterscraper = 13 Or Preferences.nfoposterscraper = 14 Or Preferences.nfoposterscraper = 15 Then
                                Dim newobject As New class_mpdb_thumbs.Class1
                                Dim teststring As New XmlDocument
                                Dim testthumbs As String
                                Try
                                    testthumbs = newobject.get_mpdb_thumbs(newmovie.fullmoviebody.imdbid)
                                    Dim testthumbs2 As String = "<totalthumbs>" & testthumbs & "</totalthumbs>"
                                    teststring.LoadXml(testthumbs2)
                                    thumbs = thumbs & testthumbs.ToString
                                Catch ex As Exception
#If SilentErrorScream Then
                                    Throw ex
#End If
                                    Thread.Sleep(1)
                                End Try
                            End If

                            If Preferences.nfoposterscraper = 8 Or Preferences.nfoposterscraper = 9 Or Preferences.nfoposterscraper = 10 Or Preferences.nfoposterscraper = 11 Or Preferences.nfoposterscraper = 12 Or Preferences.nfoposterscraper = 13 Or Preferences.nfoposterscraper = 14 Or Preferences.nfoposterscraper = 15 Then
                                Dim thumbscraper As New imdb_thumbs.Class1
                                Dim teststring As New XmlDocument
                                Try
                                    Dim testthumbs As String
                                    testthumbs = thumbscraper.getimdbthumbs(newmovie.fullmoviebody.title, newmovie.fullmoviebody.year, newmovie.fullmoviebody.imdbid)
                                    Dim testthumbs2 As String = "<totalthumbs>" & testthumbs & "</totalthumbs>"
                                    teststring.LoadXml(testthumbs2)
                                    thumbs = thumbs & testthumbs.ToString
                                Catch ex As Exception
#If SilentErrorScream Then
                                    Throw ex
#End If
                                    Thread.Sleep(1)
                                End Try
                            End If




                            thumbs = "<thumblist>" & thumbs & "</thumblist>"

                            Try
                                thumbstring.LoadXml(thumbs)



                                For Each thisresult In thumbstring("thumblist")
                                    Select Case thisresult.Name
                                        Case "thumb"
                                            newmovie.listthumbs.Add(thisresult.InnerText)
                                    End Select
                                Next
                                progresstext &= " - OK"
                                Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                scraperLog = scraperLog & "Poster URLs Scraped OK" & vbCrLf
                            Catch ex As Exception
                                scraperLog = scraperLog & "!!! Error with " & newMovieList(f).nfopathandfilename & vbCrLf
                                scraperLog = scraperLog & "!!! An error was encountered at stage 4, Downloading poster list for nfo file" & vbCrLf
                                scraperLog = scraperLog & ex.Message.ToString & vbCrLf & vbCrLf
                                errorcounter += 1
                                newmovie.listthumbs.Clear()
                            End Try
                        End If
                        stage = 5
                        'stage 5 = get hd tags
                        Try
                            Dim tempsa As String = IO.Path.GetFileName(newMovieList(f).mediapathandfilename)
                            Dim tempsb As String = newMovieList(f).mediapathandfilename.Replace(IO.Path.GetFileName(newMovieList(f).mediapathandfilename), "")
                            tempsb = IO.Path.Combine(tempsb, "tempoffline.ttt")
                            If Not IO.File.Exists(tempsb) Then

                                newmovie.filedetails = Preferences.Get_HdTags(newMovieList(f).mediapathandfilename)

                                If newmovie.filedetails.filedetails_video.DurationInSeconds.Value <> Nothing And ((Preferences.movieRuntimeDisplay = "file") Or (Preferences.movieRuntimeFallbackToFile And newmovie.fullmoviebody.runtime = "")) Then
                                    Try
                                        progresstext &= " - HD tags"
                                        Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                        newmovie.fullmoviebody.runtime = Utilities.cleanruntime(newmovie.filedetails.filedetails_video.DurationInSeconds.Value) & " min"
                                        progresstext &= " - OK"
                                        Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                        scraperLog = scraperLog & "HD Tags Added OK" & vbCrLf
                                    Catch ex As Exception
                                        scraperLog = scraperLog & "!!! Error getting HD Tags:- " & ex.Message.ToString & vbCrLf
                                    End Try
                                End If
                            End If
                        Catch ex As Exception
                            scraperLog = scraperLog & "!!! Error getting HD Tags:- " & ex.Message.ToString & vbCrLf
                        End Try

                        If newmovie.fullmoviebody.title = Nothing Then
                            newmovie.fullmoviebody.title = "Unknown Title"
                        End If
                        If newmovie.fullmoviebody.title = "" Then
                            newmovie.fullmoviebody.title = "Unknown Title"
                        End If

                        If newmovie.fullmoviebody.year = Nothing Then
                            newmovie.fullmoviebody.year = "0000"
                        End If
                        If newmovie.fullmoviebody.rating = Nothing Then
                            newmovie.fullmoviebody.rating = "0"
                        End If
                        If newmovie.fullmoviebody.top250 = Nothing Then
                            newmovie.fullmoviebody.top250 = "0"
                        End If
                        If newmovie.fullmoviebody.playcount = Nothing Then
                            newmovie.fullmoviebody.playcount = "0"
                        End If
                        If newmovie.fullmoviebody.title = "Unknown Title" Then
                            newmovie.fullmoviebody.plot = "This Movie has could not be identified by Media Companion, to add the movie manually, go to the movie edit page and select ""Change Movie"" to manually select the correct movie"
                            If title <> Nothing Then
                                If title = "" Then
                                    title = "Unknown Title"
                                End If
                            Else
                                title = "Unknown Title"
                            End If
                            newmovie.fullmoviebody.title = title

                        End If
                        If newmovie.fullmoviebody.title = "Unknown Title" Then
                            newmovie.fullmoviebody.genre = "Problem"
                        End If
                        Dim myDate2 As Date = System.DateTime.Now
                        Try
                            newmovie.fileinfo.createdate = Format(myDate2, datePattern).ToString
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try
                        Form1.nfoFunction.mov_NfoSave(nfopath, newmovie, True)


                        If Preferences.DownloadTrailerDuringScrape Then
                            Form1.DownloadTrailer(Form1.GetTrailerPath(nfopath), trailer)
                        End If

                        Dim movietoadd As New ComboList(SetDefaults)
                        movietoadd.fullpathandfilename = nfopath
                        movietoadd.filename = IO.Path.GetFileName(newMovieList(f).nfopathandfilename)
                        movietoadd.foldername = Utilities.GetLastFolder(newMovieList(f).nfopathandfilename)
                        movietoadd.title = newmovie.fullmoviebody.title
                        movietoadd.originaltitle = newmovie.fullmoviebody.originaltitle
                        movietoadd.sortorder = newmovie.fullmoviebody.sortorder
                        movietoadd.runtime = newmovie.fullmoviebody.runtime
                        movietoadd.votes = newmovie.fullmoviebody.votes

                        If newmovie.fullmoviebody.title <> Nothing Then
                            If newmovie.fullmoviebody.year <> Nothing Then
                                If newmovie.fullmoviebody.title.ToLower.IndexOf("the") = 0 Then
                                    movietoadd.titleandyear = newmovie.fullmoviebody.title.Substring(4, newmovie.fullmoviebody.title.Length - 4) & ", The (" & newmovie.fullmoviebody.year & ")"
                                Else
                                    movietoadd.titleandyear = newmovie.fullmoviebody.title & " (" & newmovie.fullmoviebody.year & ")"
                                End If
                            Else
                                movietoadd.titleandyear = newmovie.fullmoviebody.title & " (0000)"
                            End If
                        Else
                            movietoadd.titleandyear = "Unknown (0000)"
                        End If
                        movietoadd.outline = newmovie.fullmoviebody.outline
                        movietoadd.plot = newmovie.fullmoviebody.plot
                        movietoadd.year = newmovie.fullmoviebody.year



                        Dim filecreation As New IO.FileInfo(newMovieList(f).nfopathandfilename)
                        Dim myDate As Date = filecreation.LastWriteTime
                        Try
                            movietoadd.filedate = Format(myDate, datePattern).ToString
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try
                        myDate2 = System.DateTime.Now
                        Try
                            movietoadd.createdate = Format(myDate2, datePattern).ToString
                        Catch ex As Exception
#If SilentErrorScream Then
                            Throw ex
#End If
                        End Try



                        movietoadd.id = newmovie.fullmoviebody.imdbid
                        movietoadd.rating = newmovie.fullmoviebody.rating
                        movietoadd.top250 = newmovie.fullmoviebody.top250
                        movietoadd.genre = newmovie.fullmoviebody.genre
                        movietoadd.playcount = newmovie.fullmoviebody.playcount

                        If Form1.ProgressAndStatus1.cancel = True Then
                            scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                            Return
                        End If

                        stage = 6
                        'stage 6 = download movieposter
                        Dim moviethumburl As String = ""
                        If Preferences.scrapemovieposters = True And Preferences.overwritethumbs = True Or IO.File.Exists(Preferences.GetPosterPath(newMovieList(f).nfopathandfilename)) = False Then
                            Try
                                If Form1.ProgressAndStatus1.cancel = True Then
                                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                                    Return
                                End If

                                progresstext &= " * Poster"
                                Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                Select Case Preferences.moviethumbpriority(0)
                                    Case "Internet Movie Poster Awards"
                                        moviethumburl = Form1.scraperFunction2.impathumb(newmovie.fullmoviebody.title, newmovie.fullmoviebody.year)
                                    Case "IMDB"
                                        moviethumburl = Form1.scraperFunction2.imdbthumb(newmovie.fullmoviebody.imdbid)
                                    Case "Movie Poster DB"
                                        moviethumburl = Form1.scraperFunction2.mpdbthumb(newmovie.fullmoviebody.imdbid)
                                    Case "themoviedb.org"
                                        moviethumburl = Form1.scraperFunction2.tmdbthumb(newmovie.fullmoviebody.imdbid)
                                End Select
                            Catch
                                moviethumburl = "na"
                            End Try
                            Try
                                If Form1.ProgressAndStatus1.cancel = True Then
                                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                                    Return
                                End If
                                If moviethumburl.Length < 10 Then
                                    Select Case Preferences.moviethumbpriority(1)
                                        Case "Internet Movie Poster Awards"
                                            moviethumburl = Form1.scraperFunction2.impathumb(newmovie.fullmoviebody.title, newmovie.fullmoviebody.year)
                                        Case "IMDB"
                                            moviethumburl = Form1.scraperFunction2.imdbthumb(newmovie.fullmoviebody.imdbid)
                                        Case "Movie Poster DB"
                                            moviethumburl = Form1.scraperFunction2.mpdbthumb(newmovie.fullmoviebody.imdbid)
                                        Case "themoviedb.org"
                                            moviethumburl = Form1.scraperFunction2.tmdbthumb(newmovie.fullmoviebody.imdbid)
                                    End Select
                                End If
                            Catch
                                moviethumburl = "na"
                            End Try
                            Try
                                If Form1.ProgressAndStatus1.cancel = True Then
                                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                                    Return
                                End If
                                If Form1.ProgressAndStatus1.cancel = True Then
                                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                                    Return
                                End If
                                If moviethumburl.Length < 10 Then
                                    Select Case Preferences.moviethumbpriority(2)
                                        Case "Internet Movie Poster Awards"
                                            moviethumburl = Form1.scraperFunction2.impathumb(newmovie.fullmoviebody.title, newmovie.fullmoviebody.year)
                                        Case "IMDB"
                                            moviethumburl = Form1.scraperFunction2.imdbthumb(newmovie.fullmoviebody.imdbid)
                                        Case "Movie Poster DB"
                                            moviethumburl = Form1.scraperFunction2.mpdbthumb(newmovie.fullmoviebody.imdbid)
                                        Case "themoviedb.org"
                                            moviethumburl = Form1.scraperFunction2.tmdbthumb(newmovie.fullmoviebody.imdbid)
                                    End Select
                                End If
                            Catch
                                moviethumburl = "na"
                            End Try
                            Try
                                If Form1.ProgressAndStatus1.cancel = True Then
                                    scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                                    Return
                                End If
                                If moviethumburl.Length < 10 Then
                                    Select Case Preferences.moviethumbpriority(3)
                                        Case "Internet Movie Poster Awards"
                                            moviethumburl = Form1.scraperFunction2.impathumb(newmovie.fullmoviebody.title, newmovie.fullmoviebody.year)
                                        Case "IMDB"
                                            moviethumburl = Form1.scraperFunction2.imdbthumb(newmovie.fullmoviebody.imdbid)
                                        Case "Movie Poster DB"
                                            moviethumburl = Form1.scraperFunction2.mpdbthumb(newmovie.fullmoviebody.imdbid)
                                        Case "themoviedb.org"
                                            moviethumburl = Form1.scraperFunction2.tmdbthumb(newmovie.fullmoviebody.imdbid)
                                    End Select
                                End If
                            Catch
                                moviethumburl = "na"
                            End Try
                            Try
                                If moviethumburl.Length >= 10 Then
                                    Dim newmoviethumbpath As String = Preferences.GetPosterPath(newMovieList(f).nfopathandfilename)
                                    Try
                                        Utilities.DownloadImage(moviethumburl, posterpath)

                                        progresstext &= " - OK"
                                        Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)

                                        scraperLog = scraperLog & "Poster scraped and saved OK" & vbCrLf

                                        Dim temppath As String = newmoviethumbpath.Replace(System.IO.Path.GetFileName(newmoviethumbpath), "folder.jpg")
                                        If Preferences.createfolderjpg = True Then
                                            If Preferences.overwritethumbs = True Or System.IO.File.Exists(temppath) = False Then
                                                Utilities.DownloadImage(moviethumburl, temppath)
                                                scraperLog = scraperLog & "Poster also saved as ""folder.jpg"" OK" & vbCrLf
                                            Else
                                                scraperLog = scraperLog & "!!! folder.jpg Not Saved to :- " & temppath & ", file already exists" & vbCrLf
                                            End If
                                        End If
                                    Catch ex As Exception
                                        scraperLog = scraperLog & "!!! Problem Saving Thumbnail" & vbCrLf
                                        scraperLog = scraperLog & "!!! Error Returned :- " & ex.ToString & vbCrLf & vbCrLf
                                    End Try
                                End If
                            Catch ex As Exception
#If SilentErrorScream Then
                                Throw ex
#End If
                            End Try
                        End If




                        stage = 7
                        'stage 7 = download fanart
                        If Preferences.overwritethumbs = True Or Preferences.overwritethumbs = False And IO.File.Exists(Preferences.GetFanartPath(newMovieList(f).nfopathandfilename)) = False Then
                            If Preferences.savefanart = False Then
                                'scraperlog = scraperlog & "Fanart Not Downloaded - Disabled in preferences, use browser to find and add Fanart" & vbCrLf
                            Else
                                Try
                                    If Form1.ProgressAndStatus1.cancel = True Then
                                        scraperLog = scraperLog & vbCrLf & "Operation cancelled by user"
                                        Return
                                    End If
                                    progresstext &= " * Fanart"
                                    Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)

                                    moviethumburl = ""
                                    If Not System.IO.File.Exists(fanartpath) Or Preferences.overwritethumbs = True Then
                                        Dim temp As String = newmovie.fullmoviebody.imdbid

                                        Dim fanarturl As String = "http://api.themoviedb.org/2.1/Movie.imdbLookup/en/xml/3f026194412846e530a208cf8a39e9cb/" & temp
                                        Dim apple2(4000) As String
                                        Dim fanartlinecount As Integer = 0
                                        Try
                                            Dim wrGETURL As WebRequest
                                            wrGETURL = WebRequest.Create(fanarturl)
                                            Dim myProxy As New WebProxy("myproxy", 80)
                                            myProxy.BypassProxyOnLocal = True
                                            Dim objStream As Stream
                                            objStream = wrGETURL.GetResponse.GetResponseStream()
                                            Dim objReader As New StreamReader(objStream)
                                            Dim sLine As String = ""
                                            fanartlinecount = 0

                                            Do While Not sLine Is Nothing
                                                fanartlinecount += 1
                                                sLine = objReader.ReadLine
                                                apple2(fanartlinecount) = sLine
                                            Loop

                                            fanartlinecount -= 1
                                            Dim fanartfound As Boolean = False
                                            For g = 1 To fanartlinecount
                                                ' vou mudar para ser compativel com api 2.1'                                           If apple2(g).IndexOf("<backdrop size=""original"">") <> -1 Then
                                                If apple2(g).IndexOf("<image type=""backdrop""") <> -1 Then
                                                    If apple2(g).IndexOf("size=""original""") <> -1 Then
                                                        Dim StartofURL As Integer = apple2(g).IndexOf("url=""") + 5
                                                        Dim EndofURL As Integer = apple2(g).IndexOf("size=""original""") - 2
                                                        apple2(g) = apple2(g).Substring(StartofURL, (EndofURL - StartofURL))
                                                        apple2(g) = apple2(g).Trim
                                                        If apple2(g).ToLower.IndexOf("http") <> -1 And apple2(g).ToLower.IndexOf(".jpg") <> -1 Or apple2(g).IndexOf(".jpeg") <> -1 Or apple2(g).IndexOf(".png") <> -1 Then
                                                            moviethumburl = apple2(g)
                                                            fanartfound = True
                                                            Exit For
                                                        End If
                                                    End If
                                                    '                                                        Exit For
                                                End If
                                            Next
                                            If fanartfound = False Then moviethumburl = ""
                                        Catch ex As Exception
#If SilentErrorScream Then
                                            Throw ex
#End If
                                        End Try

                                        If moviethumburl <> "" Then
                                            progresstext &= " - OK!"
                                            Form1.ProgressAndStatus1.ReportProgress(progress, progresstext)
                                            'scraperlog = scraperlog & "Fanart URL is " & fanarturl & vbCrLf

                                            'need to resize thumbs
                                            Utilities.DownloadImage(moviethumburl, fanartpath, True, Preferences.resizefanart)

                                            '                                                Dim buffer(8000000) As Byte
                                            '                                                Dim size As Integer = 0
                                            '                                                Dim bytesRead As Integer = 0

                                            '                                                Dim thumburl As String = moviethumburl
                                            '                                                Dim req As HttpWebRequest = WebRequest.Create(thumburl)
                                            '                                                Dim res As HttpWebResponse = req.GetResponse()
                                            '                                                Dim contents As Stream = res.GetResponseStream()
                                            '                                                Dim bytesToRead As Integer = CInt(buffer.Length)
                                            '                                                Dim bmp As New Bitmap(contents)



                                            '                                                While bytesToRead > 0
                                            '                                                    size = contents.Read(buffer, bytesRead, bytesToRead)
                                            '                                                    If size = 0 Then Exit While
                                            '                                                    bytesToRead -= size
                                            '                                                    bytesRead += size
                                            '                                                End While

                                            '                                                Dim msgFanart As String = String.Empty
                                            '                                                If Preferences.resizefanart = 1 Then
                                            '                                                    bmp.Save(fanartpath, Imaging.ImageFormat.Jpeg)
                                            '                                                    msgFanart = " - not resized"
                                            '                                                ElseIf Preferences.resizefanart = 2 Then
                                            '                                                    If bmp.Width > 1280 Or bmp.Height > 720 Then
                                            '                                                        Dim bm_source As New Bitmap(bmp)
                                            '                                                        Dim bm_dest As New Bitmap(1280, 720)
                                            '                                                        Dim gr As Graphics = Graphics.FromImage(bm_dest)
                                            '                                                        gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
                                            '                                                        gr.DrawImage(bm_source, 0, 0, 1280 - 1, 720 - 1)
                                            '                                                        bm_dest.Save(fanartpath, Imaging.ImageFormat.Jpeg)
                                            '                                                        msgFanart = " - resized to 1280x720"
                                            '                                                    Else
                                            '                                                        msgFanart = " - not resized, already =< required size"
                                            '                                                        bmp.Save(fanartpath, Imaging.ImageFormat.Jpeg)
                                            '                                                    End If
                                            '                                                ElseIf Preferences.resizefanart = 3 Then
                                            '                                                    If bmp.Width > 960 Or bmp.Height > 540 Then
                                            '                                                        Dim bm_source As New Bitmap(bmp)
                                            '                                                        Dim bm_dest As New Bitmap(960, 540)
                                            '                                                        Dim gr As Graphics = Graphics.FromImage(bm_dest)
                                            '                                                        gr.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
                                            '                                                        gr.DrawImage(bm_source, 0, 0, 960 - 1, 540 - 1)
                                            '                                                        bm_dest.Save(fanartpath, Imaging.ImageFormat.Jpeg)
                                            '                                                        msgFanart = " - resized to 960x540"
                                            '                                                    Else
                                            '                                                        msgFanart = " - not resized, already =< required size"
                                            '                                                        bmp.Save(fanartpath, Imaging.ImageFormat.Jpeg)
                                            '                                                    End If

                                            '                                                End If
                                            '                                                scraperLog = scraperLog & "Fanart scraped OK" & msgFanart & vbCrLf

                                            '                                            Catch ex As Exception
                                            '                                                Try
                                            '                                                    scraperLog = scraperLog & "!!! Fanart Not Saved to :- " & fanartpath & vbCrLf
                                            '                                                    scraperLog = scraperLog & "!!! Error received :- " & ex.ToString & vbCrLf & vbCrLf
                                            '                                                Catch ex2 As Exception
                                            '#If SilentErrorScream Then
                                            '                                                    Throw ex2
                                            '#End If
                                            '                                                End Try
                                            '                                            End Try

                                        Else
                                            'scraperlog = scraperlog & "No Fanart is Available For This Movie" & moviethumbpath & vbCrLf
                                        End If
                                    Else
                                        'scraperlog = scraperlog & "Fanart Not Saved to :- " & moviethumbpath & ", file already exists" & vbCrLf
                                    End If

                                Catch ex As Exception
#If SilentErrorScream Then
                                    Throw ex
#End If
                                End Try
                            End If

                        End If
                        Dim tempst As String = movietoadd.fullpathandfilename
                        tempst = tempst.Replace(IO.Path.GetFileName(tempst), "tempoffline.ttt")
                        If IO.File.Exists(tempst) Then
                            IO.File.Delete(tempst)
                            Call Form1.mov_OfflineDvdProcess(movietoadd.fullpathandfilename, movietoadd.title, Utilities.GetFileName(movietoadd.fullpathandfilename))
                        End If
                        Dim completebyte1 As Byte = 0
                        Dim fanartexists As Boolean = IO.File.Exists(Preferences.GetFanartPath(movietoadd.fullpathandfilename))
                        Dim posterexists As Boolean = IO.File.Exists(Preferences.GetPosterPath(movietoadd.fullpathandfilename))
                        If fanartexists = False Then
                            completebyte1 += 1
                        End If
                        If posterexists = False Then
                            completebyte1 += 2
                        End If
                        movietoadd.missingdata1 = completebyte1
                        Form1.fullMovieList.Add(movietoadd)

                        'Add the new movie in filteredListObj
                        Form1.Data_GridViewMovie = New Data_GridViewMovie()
                        Form1.Data_GridViewMovie.fullpathandfilename = movietoadd.fullpathandfilename
                        Form1.Data_GridViewMovie.movieset = movietoadd.movieset
                        Form1.Data_GridViewMovie.filename = movietoadd.filename
                        Form1.Data_GridViewMovie.foldername = movietoadd.foldername
                        Form1.Data_GridViewMovie.title = movietoadd.title
                        Form1.Data_GridViewMovie.originaltitle = movietoadd.originaltitle
                        Form1.Data_GridViewMovie.titleandyear = movietoadd.titleandyear
                        Form1.Data_GridViewMovie.year = movietoadd.year
                        Form1.Data_GridViewMovie.filedate = movietoadd.filedate
                        Form1.Data_GridViewMovie.id = movietoadd.id
                        Form1.Data_GridViewMovie.rating = movietoadd.rating
                        Form1.Data_GridViewMovie.top250 = movietoadd.top250
                        Form1.Data_GridViewMovie.genre = movietoadd.genre
                        Form1.Data_GridViewMovie.playcount = movietoadd.playcount
                        Form1.Data_GridViewMovie.sortorder = movietoadd.sortorder
                        Form1.Data_GridViewMovie.outline = movietoadd.outline
                        Form1.Data_GridViewMovie.runtime = movietoadd.runtime
                        Form1.Data_GridViewMovie.createdate = movietoadd.createdate
                        Form1.Data_GridViewMovie.missingdata1 = movietoadd.missingdata1
                        Form1.Data_GridViewMovie.plot = movietoadd.plot
                        Form1.Data_GridViewMovie.source = movietoadd.source
                        Form1.Data_GridViewMovie.votes = movietoadd.votes
                        Form1.Data_GridViewMovie.TitleUcase = movietoadd.title.ToUpper
                        Form1.filteredListObj.Add(Form1.Data_GridViewMovie)

                    End If

                    scraperLog = scraperLog & "Movie added to list" & vbCrLf
                    progress = 999999
                    ' progresstext = String.Concat("Scraping Movie " & f + 1 & " of " & newmoviecount)
                    'BckWrkScnMovies.ReportProgress(progress, progresstext)
                End If


                'Repopulate the grid
                Form1.DataGridViewBindingSource.DataSource = Form1.filteredListObj
                Form1.DataGridViewMovies.DataSource = Form1.DataGridViewBindingSource
                scraperLog = scraperLog & vbCrLf & vbCrLf & vbCrLf

            Next

        End If
        scraperLog &= vbCrLf & "!!! Search for New Movies Complete." & vbCrLf

        If Preferences.disablelogfiles = False Then
            Dim MyFormObject As New frmoutputlog(scraperLog, True)
            Try
                MyFormObject.ShowDialog()
            Catch ex As Exception

            End Try
        End If

        Call Mc.clsGridViewMovie.mov_FiltersAndSortApply()

    End Sub
End Class
