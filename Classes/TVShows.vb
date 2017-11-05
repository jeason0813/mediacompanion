Imports Alphaleonis.Win32.Filesystem

Public Class SeasonEpisodeComparer
    Implements IComparer(Of String)
    Private Shared Function ParseSeasonEpisode(ByVal item As String, ByVal type As Boolean) As Integer
        Dim hyphenIndex As Integer = item.IndexOf("-")
        ' Normally do some error checking in case hyphenIndex==-1
        Dim firstPart As String
        If type = True Then
            firstPart = item.Substring(0, hyphenIndex)
        Else
            firstPart = item.Substring(hyphenIndex + 1)
        End If
        Return Integer.Parse(firstPart)
    End Function

    Public Function Compare(ByVal first As String, ByVal second As String) As Integer Implements IComparer(Of String).Compare
        ' In real code you would probably add nullity checks
        Dim firstSeason As Integer = ParseSeasonEpisode(first, True)
        Dim secondSeason As Integer = ParseSeasonEpisode(second, True)
        Dim sameSeason As Integer = firstSeason.CompareTo(secondSeason)
        If sameSeason = 0 Then
            Dim firstEpisode As Integer = ParseSeasonEpisode(first, False)
            Dim secondEpisode As Integer = ParseSeasonEpisode(second, False)
            sameSeason = firstEpisode.CompareTo(secondEpisode)
        End If
        Return sameSeason
    End Function
End Class

Public Class TvSeriesData
    Public Property SeriesId    As String
    Public Property SeriesLan   As String

    Sub New(Optional ByVal _seriesId As String = "", Optional ByVal _lang As String = "")
        SeriesId    = _seriesId
        SeriesLan   = _lang
    End Sub
End Class
Public Class TVShows

    Public Shared Function episodeRename(ByVal eppath As String, ByVal seasonno As String, ByVal episodeno As List(Of String), ByVal showtitle As String, ByVal episodetitle As String, ByVal EpSpaces As Boolean, ByVal IsDot As Boolean)
        showtitle = Pref.RemoveIgnoredArticles(showtitle)
        Dim returnpath As String = "false"

        Dim medianame As String = eppath.Replace(Path.GetExtension(eppath), "")
        For Each ext In Utilities.VideoExtensions
            If ext = "VIDEO_TS.IFO" Then Continue For
            Dim actualname As String = medianame & ext
            If File.Exists(actualname) Then
                Dim newfilename As String
                newfilename = ""
                If seasonno.Length = 1 Then
                    seasonno = "0" & seasonno
                End If
                For g = 0 To episodeno.Count - 1
                    If episodeno(g).Length = 1 Then
                        episodeno(g) = "0" & episodeno(g)
                    End If
                Next
                newfilename = Renamer.setTVFilename(showtitle, episodetitle, episodeno, seasonno)
                newfilename = newfilename.Replace("?", "")
                newfilename = newfilename.Replace("/", "")
                newfilename = newfilename.Replace("\", "")
                newfilename = newfilename.Replace("<", "")
                newfilename = newfilename.Replace(">", "")
                newfilename = newfilename.Replace(":", "")
                newfilename = newfilename.Replace("""", "")
                newfilename = newfilename.Replace("*", "")
                newfilename = newfilename.Replace("|", "-")
                If EpSpaces Then
                    If IsDot Then
                        newfilename = newfilename.Replace(" ", ".")
                    Else
                        newfilename = newfilename.Replace(" ", "_")
                    End If
                End If
                Dim listtorename As New List(Of String)
                listtorename.Clear()
                Dim done As String = ""
                Dim temppath As String = eppath

                listtorename.Add(actualname)

                Dim di As DirectoryInfo = New DirectoryInfo(eppath.Replace(Path.GetFileName(eppath), ""))
                Dim filenama As String = Path.GetFileNameWithoutExtension(eppath)
                Dim fils As FileInfo() = di.GetFiles(filenama & ".*")
                For Each fiNext In fils
                    Dim extn As String = Utilities.GetExtension(fiNext.FullName)
                    Dim tmpname As String = fiNext.FullName.Replace(extn, extn.ToLower)
                    If Not listtorename.Contains(fiNext.FullName) AndAlso Not listtorename.Contains(tmpname) Then
                        listtorename.Add(fiNext.FullName)
                    End If
                Next

                temppath = temppath.Replace(Path.GetExtension(temppath), ".nfo")
                If File.Exists(temppath) AndAlso Not listtorename.Contains(temppath) Then listtorename.Add(temppath)

                temppath = temppath.Replace(Path.GetExtension(temppath), ".rar")
                If File.Exists(temppath) AndAlso Not listtorename.Contains(temppath) Then listtorename.Add(temppath)

                temppath = temppath.Replace(Path.GetExtension(temppath), ".jpg")
                If File.Exists(temppath) AndAlso Not listtorename.Contains(temppath) Then listtorename.Add(temppath)

                temppath = temppath.Replace(Path.GetExtension(temppath), "-thumb.jpg")
                If File.Exists(temppath) AndAlso Not listtorename.Contains(temppath) Then listtorename.Add(temppath)

                Dim StillOk As Boolean = True   'first we test every file we are going to rename, if they all can be renamed we then rename
                Dim RenameFailedFile As String = ""
                For Each ITEMS In listtorename
                    Dim newname As String = ITEMS.Replace(filenama, newfilename)
                    Dim fi As New FileInfo(ITEMS)
                    If File.Exists(newname) AndAlso Not String.Compare(ITEMS, newname, False) Then ' if newname exists, but there is a mismatch of casings, still OK to rename.
                        RenameFailedFile = newname
                        StillOk = False
                    End If
                Next

                If StillOk = True Then
                    Dim FirstCount As Boolean = True
                    For Each ITEMS In listtorename
                        Dim newname As String = ITEMS.Replace(filenama, newfilename)
                        Dim TempName As String = ITEMS.Replace(filenama, "TempFile")
                        done = newname.Replace(Path.GetExtension(newname), ".nfo")
                        If done.Contains("-thumb.") Then done = done.Replace("-thumb", "")
                        Try
                            Dim fi As New FileInfo(ITEMS)
                            'If current and new filesname are the same, but there are mis-matched casing, rename to temp file first
                            If ITEMS.ToLower = newname.ToLower AndAlso String.Compare(ITEMS, newname, False) Then fi.MoveTo(TempName)

                            fi.MoveTo(newname)
                            If FirstCount = True Then  'we only want to show the renamed mediafile in the brief view
                                Pref.tvScraperLog &= "!!! Renamed to: " & newname & vbCrLf
                                FirstCount = False
                            Else
                                Pref.tvScraperLog &= "                " & newname & vbCrLf
                            End If

                        Catch ex As Exception
                            done = eppath
                            Pref.tvScraperLog &= "!!! *** Rename FAILED for : " & newname & vbCrLf
                            Pref.tvScraperLog &= "!!! *** Reported Message  : " & ex.Message & vbCrLf
                            Pref.tvScraperLog &= "!!! *** Resolve the indicated issue, & then try to rename the files again." & vbCrLf
                            Exit For 'we need to stop renaming the rest of the files if we get here. We usually get here if the episode
                            '        ' cannot be renamed because another process has locked the file. i.e. another app has the file open.  
                        End Try

                    Next
                    returnpath = done
                Else
                    Pref.tvScraperLog &= "!!! Rename aborted - file already exists : " & RenameFailedFile & vbCrLf
                End If
                Pref.tvScraperLog &= "!!! " & vbCrLf
            End If
        Next
        Return returnpath
    End Function
End Class
