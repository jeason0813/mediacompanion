'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class WikipediaMusivVideoScraper

    Public Function musicVideoScraper(ByVal fullpathandfilename As String, Optional ByVal searchterm As String = "", Optional ByVal wikipediaURL As String = "")
        Monitor.Enter(Me)
        Dim totalinfo As String = ""
        totalinfo = "<musicvideo>" & vbCrLf
        Dim s As New Classimdb
        Dim webpage As List(Of String)
        Dim Artist As String = ""
        Dim Title As String = ""
        Dim wikiurlpassed As String = wikipediaURL 
        searchterm = getArtistAndTitle(fullpathandfilename)

        If searchterm = "" And wikipediaURL = "" Then
            Dim filenameWithoutExtension As String = Path.GetFileNameWithoutExtension(fullpathandfilename)
            Dim strarr() As String
            strarr = filenameWithoutExtension.Split("-"c)
            Artist = strarr(0).Trim
            Title = strarr(1).Trim
            searchterm = Utilities.searchurltitle(filenameWithoutExtension)
        ElseIf searchterm <> "" And wikipediaURL = "" Then
            Dim strarr() As String
            strarr = searchterm.Split("-"c)
            Artist = strarr(0).Trim
            Title = strarr(1).Trim
        ElseIf wikipediaURL <> "" Then
            Dim strarr() As String
            strarr = fullpathandfilename.Split("-"c)
            Artist = strarr(0).Trim
            Title = strarr(1).Trim
            searchterm = fullpathandfilename
        End If
        searchterm = Utilities.searchurltitle(searchterm)
        Dim searchurl As String = "http://www.google.co.uk/search?hl=en-US&as_q=" & searchterm & "%20song&as_sitesearch=http://en.wikipedia.org/"

        If wikipediaURL = "" Then
            webpage = s.loadwebpage(Pref.proxysettings, searchurl, False, 10)
            For Each line In webpage
                If line.IndexOf("<a href=""https://en.wikipedia.org/wiki/") <> -1 Then
                    wikipediaURL = ""
                    Exit For
                ElseIf line.IndexOf("<a href=""/url?q=https://en.wikipedia.org/wiki/") <> -1 Then
                    Dim startinteger As Integer = line.IndexOf("<a href=""/url?q=https://en.wikipedia.org/wiki/")
                    wikipediaURL = line.Substring(startinteger + 16, 100)
                    wikipediaURL = wikipediaURL.Replace("%253F", "%3F")
                    wikipediaURL = wikipediaURL.Substring(0, wikipediaURL.IndexOf("&"))
                    Exit For
                End If
            Next
            webpage.Clear()
        End If
        Dim fullwebpage As String = ""

        If wikipediaURL <> "" Then
            webpage = s.loadwebpage(Pref.proxysettings, wikipediaURL, False, 10)
            Dim htpage As String = ""
            For each p In webpage
                htpage &= p & vbcrlf
            Next
            Dim webPg As String = String.Join( "" , webpage.ToArray() )
            fullwebpage = webPg
            Try
                'Scrape Plot and Director
                If fullwebpage.IndexOf("<h2><span class=""mw-headline"" id=""Music_video"">") <> -1 Then
                    Dim tempstring As String = fullwebpage.Substring(fullwebpage.IndexOf("<h2><span class=""mw-headline"" id=""Music_video"">"), fullwebpage.Length - fullwebpage.IndexOf("<h2><span class=""mw-headline"" id=""Music_video"">"))
                    tempstring = tempstring.Substring(4, tempstring.Length - 4)
                    tempstring = tempstring.Substring(0, tempstring.IndexOf("<h2>"))
                    tempstring = tempstring.Substring(tempstring.IndexOf("<p>"), tempstring.LastIndexOf("</p>") - tempstring.IndexOf("<p>"))

                    'get director before stripping tags
                    Try
                        If tempstring.IndexOf("directed by") <> -1 Then
                            Dim director As String = tempstring.Substring(tempstring.ToLower.IndexOf("directed by"), tempstring.Length - tempstring.ToLower.IndexOf("directed by"))
                            director = director.Substring(0, director.IndexOf("</a>"))
                            director = director.Substring(director.LastIndexOf(">") + 1, director.Length - director.LastIndexOf(">") - 1)
                            totalinfo.AppendTag("director", director)
                        End If
                    Catch
                    End Try

                    'strip html tags from plot
                    tempstring = Regex.Replace(tempstring, "<.*?>", "")

                    'strip reference tags
                    tempstring = Regex.Replace(tempstring, "\[(.*?)\]", "")

                    'tidy up
                    tempstring = tempstring.Replace("<edit>", "")
                    tempstring = tempstring.Replace("Synopsis", vbCrLf & "Synopsis" & vbCrLf)
                    tempstring = tempstring.Replace("Background", vbCrLf & "Background" & vbCrLf)
                    totalinfo.AppendTag("plot", tempstring.DeCodeSpecialChrs)
                End If

                'get year
                '<th scope="row" style="text-align:left;">Released</th>
                If fullwebpage.IndexOf(">Released</th>") <> -1 Then
                    Try
                        Dim tempstring As String = fullwebpage.Substring(fullwebpage.IndexOf("Released</th>"), fullwebpage.Length - fullwebpage.IndexOf("Released</th>"))
                        tempstring = tempstring.Substring(tempstring.IndexOf("<td>"), tempstring.IndexOf("</td>") - tempstring.IndexOf("<td>"))
                        Dim r As Regex = New Regex("\d{4}")
                        Dim match As Match = r.Match(tempstring)
                        totalinfo.AppendTag("year", match.Value)
                    Catch
                    End Try
                End If

                'get genre
                If fullwebpage.IndexOf(">Genre</a></th>") <> -1 Then
                    Try
                        Dim tempstring As String = fullwebpage.Substring(fullwebpage.IndexOf(">Genre</a></th>"), fullwebpage.Length - fullwebpage.IndexOf(">Genre</a></th>"))
                        tempstring = tempstring.Substring(tempstring.IndexOf("<td>"), tempstring.IndexOf("</td>") - tempstring.IndexOf("<td>"))
                        tempstring = Regex.Replace(tempstring, "<.*?>", "")
                        totalinfo.AppendTag("genre", tempstring)
                    Catch
                    End Try
                End If

                'get album title
                If fullwebpage.IndexOf("from the album") <> -1 Then
                    Try
                        Dim tempstring As String = fullwebpage.Substring(fullwebpage.IndexOf("from the album"), fullwebpage.Length - fullwebpage.IndexOf("from the album"))
                        tempstring = tempstring.Replace("from the album ", "")
                        tempstring = tempstring.Substring(0, tempstring.IndexOf("</i>"))
                        tempstring = Regex.Replace(tempstring, "<.*?>", "")
                        totalinfo.AppendTag("album", tempstring)
                    Catch
                    End Try
                End If

                'get studio
                If fullwebpage.IndexOf("Label</a></th>") <> -1 Then
                    Try
                        Dim tempstring As String = fullwebpage.Substring(fullwebpage.IndexOf("Label</a></th>"), fullwebpage.Length - fullwebpage.IndexOf("Label</a></th>"))
                        tempstring = tempstring.Substring(tempstring.IndexOf("<td>"), tempstring.IndexOf("</td>") - tempstring.IndexOf("<td>"))
                        tempstring = Regex.Replace(tempstring, "<.*?>", "")
                        totalinfo.AppendTag("studio", tempstring)
                    Catch
                    End Try
                End If
                
                If fullwebpage.IndexOf(""">Single</a>") <> -1 Then
                    Try
                        Dim tempstring As String = fullwebpage.Substring(fullwebpage.IndexOf(""">Single</a>"), 150 )
                        tempstring = tempstring.Replace(""">Single</a>", "")
                        tempstring = tempstring.Substring((tempstring.IndexOf(""">")+2), (tempstring.IndexOf("</a></th>") - tempstring.IndexOf(""">"))-2)
                        tempstring = Regex.Replace(tempstring, "<.*?>", "")
                        totalinfo.AppendTag("artist", tempstring.DeCodeSpecialChrs)
                    Catch
                    totalinfo.AppendTag("artist", Artist.DeCodeSpecialChrs)
                    End Try
                End If

                'Get Title
                For f = 0 to webpage.Count-1
                    If webpage(f).IndexOf("""</th>") <> -1 Then
                        Try
                            Dim d = webpage(f).IndexOf(""">")+3
                            Dim w = If(d > 0, webpage(f).IndexOf("""</th>", d), 0)
                            If Not D <= 0 And Not W <= 0 Then
                                Dim tempstring As String = webpage(f).Substring(d, w - d)
                                tempstring = Regex.Replace(tempstring, "<.*?>", "")
                                totalinfo.AppendTag("title", tempstring.DeCodeSpecialChrs)
                            End If
                            Exit For
                        Catch
                            totalinfo.AppendTag("title", Title.DeCodeSpecialChrs)
                        End Try
                    End If
                Next

                'Poster
                For p = 0 To webpage.Count-1
                    If webpage(p).Contains("class=""image""><img") AndAlso webpage(p).Contains("data-file-width=") AndAlso Not webpage(p).Contains("Question_book-new") Then
                        Dim tempstring As String = webpage(p).Substring(webpage(p).LastIndexOf("//upload"))
                        Dim jpgimg As Boolean = tempstring.Contains(".jpg")
                        If tempstring.Contains("/thumb/") AndAlso Not tempstring.Contains("440p") Then Continue For
                        If jpgimg Then
                            Dim ImgType As String = If(tempstring.Contains(" 2x"""), ".jpg ", ".jpg")
                            tempstring = tempstring.Substring(0, tempstring.IndexOf(ImgType)+4)
                        Else
                            Dim ImgType As String = If(tempstring.Contains(" 2x"""), ".png ", ".png")
                            tempstring = tempstring.Substring(0, tempstring.IndexOf(ImgType)+4)
                        End If
                        tempstring = "http:" & tempstring
                        totalinfo.AppendTag("thumb", tempstring)
                        If Utilities.UrlIsValid(tempstring) Then Exit For
                    End If
                Next
            Catch
            End Try
        End If
        totalinfo = totalinfo & "</musicvideo>" & vbCrLf
        Return totalinfo
        Monitor.Exit(Me)
    End Function

    Private Function getArtistAndTitle(ByVal fullpathandfilename As String)
        Monitor.Enter(Me)
        Dim searchTerm As String = ""
        Dim filenameWithoutExtension As String = Path.GetFileNameWithoutExtension(fullpathandfilename)
        If filenameWithoutExtension.IndexOf(" - ") <> -1 Then
            searchTerm = filenameWithoutExtension
        Else 'assume /artist/title.ext convention
            Try
                Dim lastfolder As String = Utilities.GetLastFolder(fullpathandfilename)
                searchTerm = lastfolder & " - " & filenameWithoutExtension
            Catch
            End Try
        End If

        If searchTerm = "" Then
            searchTerm = filenameWithoutExtension
        End If

        Return searchTerm
        Monitor.Exit(Me)
    End Function
    
End Class
