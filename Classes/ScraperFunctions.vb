Imports System.Net
'Imports System.IO
'Imports Alphaleonis.Win32.Filesystem
Imports System.Threading
Imports System.Xml
Public Class ScraperFunctions
    
    Dim tvdburl As String
    Dim tvfblinecount As Integer
    Dim tvdbwebsource(4000) As String

    Public Function getimdbactors(ByVal imdbid As String, ByVal mirror As String)
        Dim tactors(1000, 2)
        Dim tactorcount As Integer = 0
        tvdburl = mirror & "title/" & imdbid & "/fullcredits#cast"
        loadwebpage(tvdburl)
        Try
            If tvdbwebsource(1) <> "404" Then
                For f = 1 To tvfblinecount
                    If tvdbwebsource(f).IndexOf("Episode  Cast</a>") <> -1 Then
                        tvdbwebsource(f) = tvdbwebsource(f).Substring(tvdbwebsource(f).IndexOf("Cast</a>"), tvdbwebsource(f).Length - tvdbwebsource(f).IndexOf("Cast</a>"))
                        If tvdbwebsource(f).IndexOf("<tr><td align=""center"" colspan=""4""><small>rest of cast listed alphabetically:</small></td></tr>") <> -1 Then
                            tvdbwebsource(f) = tvdbwebsource(f).Replace("</td></tr> <tr><td align=""center"" colspan=""4""><small>rest of cast listed alphabetically:</small></td></tr>", "</td></tr><tr class")
                        End If
                        Dim tvtempstring As String
                        Dim tvtempint As Integer = 0
                        tvtempstring = tvdbwebsource(f)
                        Do Until tvtempstring.IndexOf("</td></tr><tr class") = -1
                            tvtempint += 1
                            tactors(tvtempint, 0) = tvtempstring.Substring(0, tvtempstring.IndexOf("</td></tr><tr class") + 19)
                            tvtempstring = tvtempstring.Replace(tactors(tvtempint, 0), "")
                        Loop
                        tvtempint += 1
                        tactors(tvtempint, 0) = tvtempstring
                        tactorcount = tvtempint
                        For g = 1 To tactorcount
                            If tactors(g, 0).IndexOf("http://resume.imdb.com") <> -1 Then tactors(g, 0) = tactors(g, 0).Replace("http://resume.imdb.com", "")
                            If tactors(g, 0).IndexOf("http://i.media-imdb.com/images/tn15/addtiny.gif") <> -1 Then tactors(g, 0) = tactors(g, 0).Replace("http://i.media-imdb.com/images/tn15/addtiny.gif", "")
                            If tactors(g, 0).IndexOf("</td></tr></table>") <> -1 Then
                                tvtempint = tactors(g, 0).IndexOf("</td></tr></table>")
                                tvtempstring = tactors(g, 0).Substring(tvtempint, tactors(g, 0).Length - tvtempint)
                                tactors(g, 0) = tactors(g, 0).Replace(tvtempstring, "</td></tr><tr class")
                            End If
                            If tactors(g, 0).indexof("http") <> -1 Then
                                Dim tvfirst As Integer
                                Dim tvlast As Integer
                                tvfirst = tactors(g, 0).IndexOf("http")
                                tvlast = tactors(g, 0).IndexOf("._V1._")
                                tactors(g, 2) = tactors(g, 0).Substring(tvfirst, tvlast - tvfirst + 6)
                                tactors(g, 2) = tactors(g, 2).Replace("._V1._", "._V1._SY200_SX300_.jpg")
                            End If

                            If tactors(g, 0).IndexOf("a href=""/character") <> -1 Then
                                tactors(g, 1) = tactors(g, 0).Substring(tactors(g, 0).IndexOf("a href=""/character") + 19, tactors(g, 0).IndexOf("</td></tr><tr class") - tactors(g, 0).IndexOf("a href=""/character") - 19)
                                If tactors(g, 1).IndexOf("</a>") <> -1 Then
                                    tactors(g, 1) = tactors(g, 1).Substring(12, tactors(g, 1).IndexOf("</a>") - 12)
                                ElseIf tactors(g, 1).IndexOf("</a>") = -1 Then
                                    tactors(g, 1) = tactors(g, 1).Substring(12, tactors(g, 1).Length - 12)
                                End If
                                tvtempstring = tactors(g, 0).Substring(tactors(g, 0).IndexOf("a href=""/character"), tactors(g, 0).Length - tactors(g, 0).IndexOf("a href=""/character"))
                                tactors(g, 0) = tactors(g, 0).Replace(tvtempstring, "")
                                Dim tvfirst As Integer
                                Dim tvlast As Integer
                                tvfirst = tactors(g, 0).IndexOf("/"">")
                                tvlast = tactors(g, 0).IndexOf("</a></td>")
                                tactors(g, 0) = tactors(g, 0).Substring(tvfirst + 3, (tvlast) - (tvfirst + 3))
                            ElseIf tactors(g, 0).IndexOf("a href=""/character") = -1 Then
                                Dim tvfirst As Integer
                                Dim tvlast As Integer
                                tvfirst = tactors(g, 0).IndexOf("<td class=""char"">")
                                tvlast = tactors(g, 0).IndexOf("</td></tr><tr class")
                                tactors(g, 1) = tactors(g, 0).Substring(tvfirst + 17, tvlast - tvfirst - 17)
                                tvfirst = tactors(g, 0).IndexOf("/"">")
                                tvlast = tactors(g, 0).IndexOf("</a></td>")
                                tactors(g, 0) = tactors(g, 0).Substring(tvfirst + 3, (tvlast) - (tvfirst + 3))
                            End If
                        Next
                    End If
                Next
            Else
                tactors(0, 0) = "404"
            End If
        Catch ex As Exception
            tactors(0, 0) = "404"
        End Try
        
        Return tactors
    End Function

    Public Function loadwebpage(ByVal tvdburl As String)
        Monitor.Enter(Me)
        Try
            Try
                ReDim tvdbwebsource(10000)
                tvfblinecount = 0
                Dim wrGETURL As WebRequest
                wrGETURL = WebRequest.Create(tvdburl)
                wrGETURL.Proxy = Utilities.MyProxy
                Dim objStream As IO.stream
                objStream = wrGETURL.GetResponse.GetResponseStream()
                Dim objReader As New IO.streamReader(objStream)
                Dim tvdbsLine As String = ""
                tvfblinecount = 0

                Do While Not tvdbsLine Is Nothing
                    tvfblinecount += 1
                    tvdbsLine = objReader.ReadLine
                    If Not tvdbsLine Is Nothing Then
                        tvdbwebsource(tvfblinecount) = tvdbsLine
                    End If
                Loop
                objReader.Close()
                tvfblinecount -= 1
            Catch
                tvfblinecount = 1
                tvdbwebsource(1) = "404"
            Finally
                Monitor.Exit(Me)
            End Try
            Return tvdbwebsource
        Catch
        Finally
            Monitor.Exit(Me)
        End Try
        Return "Error"
    End Function
    
    Public Function mpdbthumb(ByVal posterimdbid As String)
        Monitor.Enter(Me)
        Try
            Dim first As String
            Dim last As String

            Dim allok As Boolean = False
            Dim thumburl As String = "na"
            Dim temp As String = posterimdbid
            temp = temp.Replace("tt", "")
            Dim fanarturl As String = "http://www.movieposterdb.com/movie/" & temp & "/"
            Dim apple2(4000) As String
            Dim fanartlinecount As Integer = 0
            Dim wrGETURL As WebRequest
            wrGETURL = WebRequest.Create(fanarturl)
            wrGETURL.Proxy = Utilities.MyProxy
            Dim objStream As IO.stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.streamReader(objStream)
            Dim sLine As String = ""
            fanartlinecount = 0

            Do While Not sLine Is Nothing
                fanartlinecount += 1
                sLine = objReader.ReadLine
                apple2(fanartlinecount) = sLine
            Loop
            fanartlinecount -= 1
            
            For f = 2 To fanartlinecount
                If apple2(f).IndexOf("<title>") <> -1 Then
                    If apple2(f).IndexOf("<title>MoviePosterDB.com - Internet Movie Poster DataBase</title>") <> -1 Then
                        allok = False
                        Exit For
                    Else
                        allok = True
                        Exit For
                    End If
                End If
            Next
            
            If allok = True Then
                allok = False
                For f = 2 To fanartlinecount
                    If apple2(f).IndexOf("<img  src=""http://www.movieposterdb.com/posters/") <> -1 Then
                        first = apple2(f).IndexOf("http")
                        last = apple2(f).IndexOf("jpg")
                        thumburl = apple2(f).Substring(first, (last + 3) - first)
                        If thumburl.IndexOf("t_") <> -1 Then
                            thumburl = thumburl.Replace("t_", "l_")
                            Exit For
                        End If
                        If thumburl.IndexOf("s_") <> -1 Then
                            thumburl = thumburl.Replace("s_", "l_")
                            Exit For
                        End If
                    End If
                Next
            End If

            If thumburl.IndexOf("http") = 0 And thumburl.IndexOf(".jpg") = thumburl.Length - 4 Then
                allok = True
            Else
                thumburl = "na"
            End If
            Return thumburl
        Catch
        Finally
            Monitor.Exit(Me)
        End Try
        Return "Error"
    End Function

    Public Function impathumb(ByVal title As String, ByVal year As String)
        Monitor.Enter(Me)
        Try
            Dim tempstring As String
            Dim tempint As Integer
            year = year.Replace("<year>", "")
            year = year.Replace("</year>", "")
            year = year.Replace("    ", "")
            Dim allok As Boolean = False
            Dim thumburl As String = "na"
            Dim temp As String = title

            Dim fanarturl As String = "http://www.google.com/custom?hl=en&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="

            temp = temp.ToLower
            temp = Utilities.searchurltitle(temp)
            fanarturl = fanarturl & temp & "+" & year
            fanarturl = fanarturl & "&sitesearch=www.impawards.com"
            
            Dim apple2(4000) As String
            Dim fanartlinecount As Integer = 0
            Dim wrGETURL2 As WebRequest
            wrGETURL2 = WebRequest.Create(fanarturl)
            wrGETURL2.Proxy = Utilities.MyProxy
            Dim objStream2 As IO.stream
            objStream2 = wrGETURL2.GetResponse.GetResponseStream()
            Dim objReader2 As New IO.streamReader(objStream2)
            Dim sLine2 As String = ""
            fanartlinecount = 0

            Do While Not sLine2 Is Nothing
                fanartlinecount += 1
                sLine2 = objReader2.ReadLine
                apple2(fanartlinecount) = sLine2
            Loop

            fanartlinecount -= 1
            Dim xtralge As Boolean = False
            For f = 1 To fanartlinecount
                If apple2(f).IndexOf("http://www.impawards.com/") <> -1 Then
                    Dim first As Integer = apple2(f).IndexOf("http://www.impawards.com/")
                    apple2(f) = apple2(f).Substring(first, apple2(f).Length - first)
                    fanarturl = apple2(f).Substring(0, apple2(f).IndexOf("html") + 4)
                    tempstring = fanarturl
                    tempstring = tempstring.Replace("http://", "")
                    tempint = tempstring.LastIndexOf("/")
                    If tempint - 5 = tempstring.IndexOf("/") Then
                        allok = True
                    Else
                        fanarturl = "http://www.google.com/custom?hl=en&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="
                        fanarturl = fanarturl & temp
                        fanarturl = fanarturl & "&sitesearch=www.impawards.com"
                        ReDim apple2(4000)
                        fanartlinecount = 0
                        Dim wrGETURL4 As WebRequest
                        wrGETURL4 = WebRequest.Create(fanarturl)
                        wrGETURL4.Proxy = Utilities.MyProxy
                        Dim objStream4 As IO.stream
                        objStream4 = wrGETURL4.GetResponse.GetResponseStream()
                        Dim objReader4 As New IO.streamReader(objStream4)
                        Dim sLine4 As String = ""
                        fanartlinecount = 0

                        Do While Not sLine4 Is Nothing
                            fanartlinecount += 1
                            sLine4 = objReader4.ReadLine
                            apple2(fanartlinecount) = sLine4
                        Loop
                        fanartlinecount -= 1
                        xtralge = False
                        For g = 1 To fanartlinecount
                            If apple2(f).IndexOf("http://www.impawards.com/") <> -1 Then
                                first = apple2(f).IndexOf("http://www.impawards.com/")
                                apple2(f) = apple2(f).Substring(first, apple2(f).Length - first)
                                fanarturl = apple2(f).Substring(0, apple2(f).IndexOf("html") + 4)
                                tempstring = fanarturl
                                tempstring = tempstring.Replace("http://", "")
                                tempint = tempstring.LastIndexOf("/")
                                If tempint - 5 = tempstring.IndexOf("/") Then
                                    allok = True
                                Else
                                    allok = False
                                End If
                            End If
                        Next

                    End If
                    Exit For
                End If
                If apple2(f).IndexOf("xlg.html") <> -1 Then xtralge = True
            Next

            ReDim apple2(4000)
            fanartlinecount = 0
            Dim wrGETURL3 As WebRequest

            wrGETURL3 = WebRequest.Create(fanarturl)
            wrGETURL3.Proxy = Utilities.MyProxy
            Dim objStream3 As IO.stream
            objStream3 = wrGETURL3.GetResponse.GetResponseStream()
            Dim objReader3 As New IO.streamReader(objStream3)
            Dim sLine3 As String = ""

            Do While Not sLine3 Is Nothing
                fanartlinecount += 1
                sLine3 = objReader3.ReadLine
                apple2(fanartlinecount) = sLine3
            Loop
            fanartlinecount -= 1
            For f = 1 To fanartlinecount
                If apple2(f).IndexOf("xlg.html") <> -1 Then
                    xtralge = True
                    Exit For
                End If
            Next
            
            If allok = True Then
                Dim tempstring3 As String
                Dim tempstring4 As String
                tempstring3 = fanarturl.Substring(0, fanarturl.LastIndexOf("/") + 1)
                tempstring4 = fanarturl.Substring(fanarturl.LastIndexOf("/") + 1, fanarturl.Length - fanarturl.LastIndexOf("/") - 1)
                fanarturl = tempstring3 & "posters/" & tempstring4
                If xtralge = False Then
                    thumburl = fanarturl.Replace(".html", ".jpg")
                ElseIf xtralge = True Then
                    thumburl = fanarturl.Replace(".html", "_xlg.jpg")
                End If
            Else
                thumburl = "na"
            End If
            If thumburl.IndexOf("art_machine.jpg") <> -1 Then thumburl = "na"
            Return thumburl
        Catch
        Finally
            Monitor.Exit(Me)
        End Try
        Return Nothing
    End Function

    Public Function imdbthumb(ByVal posterimdbid As String)
        Monitor.Enter(Me)
        Dim thumburl As String = "na"
        Try
            Dim pass = 2
            Do Until pass = 0 OrElse thumburl <> "na"
                pass = pass -1
                Dim posters(10000, 1) As String
                Dim postercount As Integer = 0
                Dim fanarturl As String
                Dim fanartlinecount As Integer = 0
                Dim allok As Boolean = True
                Dim apple2(10000)

                fanarturl = "http://www.imdb.com/title/" & posterimdbid & "/mediaindex?refine=poster&ref_=ttmi_ref_pos"
                If pass = 0 Then
                    fanarturl = "http://www.imdb.com/title/" & posterimdbid & "/mediaindex?ref_=ttmi_ref_mi"
                End If
                Dim wrGETURL2 As WebRequest
                wrGETURL2 = WebRequest.Create(fanarturl)
                wrGETURL2.Proxy = Utilities.MyProxy
                Dim objStream2 As IO.stream
                objStream2 = wrGETURL2.GetResponse.GetResponseStream()
                Dim objReader2 As New IO.streamReader(objStream2)
                Dim sLine2 As String = ""
                fanartlinecount = 0

                Do While Not sLine2 Is Nothing
                    fanartlinecount += 1
                    sLine2 = objReader2.ReadLine
                    apple2(fanartlinecount) = sLine2
                Loop
                fanartlinecount -= 1

                Dim totalpages As Integer
                Dim tempint As Integer
                Dim reached As Boolean = False
                Try
                    For f = 1 To fanartlinecount
                        If apple2(f) = "" Then Continue For
                        If apple2(f).IndexOf("<a href=""?page=") <> -1 Then
                            apple2(f) = apple2(f).Replace("<a href=""?page=", "")
                            apple2(f) = apple2(f).Substring(0, 1)
                            tempint = Convert.ToString(apple2(f))
                            If tempint > totalpages Then totalpages = tempint
                        End If
                        If apple2(f).IndexOf("<div class=""media_index_thumb_list""") <> -1 Then
                            reached = True
                        End If
                        If reached = True Then
                            If apple2(f).IndexOf("</div>") <> -1 Then
                                reached = False
                                Exit For
                            End If
                            If apple2(f).IndexOf("src=""http://") <> -1 And apple2(f).IndexOf("._V1_") <> -1 Then
                                apple2(f) = apple2(f).Substring(apple2(f).IndexOf("src="""), apple2(f).Length - apple2(f).IndexOf("src="""))
                                apple2(f).TrimStart()
                                apple2(f) = apple2(f).Replace("src=""", "")
                                posters(postercount, 0) = apple2(f).Substring(0, apple2(f).IndexOf("._V1_"))
                                posters(postercount, 1) = posters(postercount, 0)
                                postercount += 1
                            End If
                        End If
                    Next
                    If postercount <> 0 Then
                        thumburl = (posters(postercount -1, 0) & ".jpg")
                    End If
                Catch ex As Exception 
                End Try
                If thumburl.IndexOf("http") = -1 Or thumburl.IndexOf(".jpg") = -1 Then thumburl = "na"
            Loop
            Return thumburl
        Catch
            thumburl = "na"
        Finally
            Monitor.Exit(Me)
        End Try
        Return "Error"
    End Function

End Class
