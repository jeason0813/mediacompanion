Imports System.Threading
'Imports System.IO
Imports System.Net
Imports System.Xml

Public Class ImdbScrapper

    Dim tvdburl As String
    Dim tvfblinecount As Integer
    Dim tvdbwebsource(3000) As String

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
                Dim myProxy As New WebProxy("myproxy", 80)
                myProxy.BypassProxyOnLocal = True
                Dim objStream As IO.Stream
                objStream = wrGETURL.GetResponse.GetResponseStream()
                Dim objReader As New IO.StreamReader(objStream)
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
                objReader = Nothing
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

    'Public Function tmdbthumb(ByVal posterimdbid As String)
    '    Monitor.Enter(Me)
    '    Try
    '        Dim newobject2 As New tmdb_posters.Class1
    '        Dim thumburl As String = String.Empty
    '        Dim xmllist As String
    '        Dim ok As Boolean = False
    '        Try
    '            xmllist = newobject2.gettmdbposters_newapi(posterimdbid)
    '            Dim bannerslist As New XmlDocument
    '            bannerslist.LoadXml(xmllist)
    '            For Each item In bannerslist("tmdb_posterlist")
    '                Select Case item.name
    '                    Case "poster"
    '                        For Each img In item
    '                            If img.childnodes(0).innertext = "original" Then
    '                                thumburl = img.childnodes(1).innertext
    '                                ok = True
    '                                Exit For
    '                            End If
    '                        Next
    '                        If ok = True Then Exit For
    '                End Select
    '            Next
    '            Return thumburl
    '        Catch ex As Exception
    '            Thread.Sleep(1)
    '        End Try


    '    Catch ex As Exception
    '    Finally
    '        Monitor.Exit(Me)
    '    End Try
    '    Return "Error"
    'End Function
    Public Function mpdbthumb(ByVal posterimdbid As String)
        Monitor.Enter(Me)
        Try
            Dim first As String
            Dim last As String

            Dim allok As Boolean = False
            Dim thumburl As String = "na"
            Dim temp As String = posterimdbid
            temp = temp.Replace("tt", "")
            Dim fanarturl As String = "http://www.movieposterdb.com/movie/" & temp


            Dim apple2(2000) As String
            Dim fanartlinecount As Integer = 0
            'Try
            Dim wrGETURL As WebRequest

            wrGETURL = WebRequest.Create(fanarturl)

            Dim myProxy As New WebProxy("myproxy", 80)
            myProxy.BypassProxyOnLocal = True
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            Dim sLine As String = ""
            fanartlinecount = 0

            Do While Not sLine Is Nothing
                fanartlinecount += 1
                sLine = objReader.ReadLine
                apple2(fanartlinecount) = sLine
            Loop
            objReader.Close()
            objStream.Close()
            objReader = Nothing
            objStream = Nothing
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
                    If apple2(f).IndexOf("<img src=""http://www.movieposterdb.com/posters/") <> -1 Then
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



            'Catch
            'End Try
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
            temp = temp.Replace(" ", "+")
            temp = temp.Replace("&", "%26")
            temp = temp.Replace("À", "%c0")
            temp = temp.Replace("Á", "%c1")
            temp = temp.Replace("Â", "%c2")
            temp = temp.Replace("Ã", "%c3")
            temp = temp.Replace("Ä", "%c4")
            temp = temp.Replace("Å", "%c5")
            temp = temp.Replace("Æ", "%c6")
            temp = temp.Replace("Ç", "%c7")
            temp = temp.Replace("È", "%c8")
            temp = temp.Replace("É", "%c9")
            temp = temp.Replace("Ê", "%ca")
            temp = temp.Replace("Ë", "%cb")
            temp = temp.Replace("Ì", "%cc")
            temp = temp.Replace("Í", "%cd")
            temp = temp.Replace("Î", "%ce")
            temp = temp.Replace("Ï", "%cf")
            temp = temp.Replace("Ð", "%d0")
            temp = temp.Replace("Ñ", "%d1")
            temp = temp.Replace("Ò", "%d2")
            temp = temp.Replace("Ó", "%d3")
            temp = temp.Replace("Ô", "%d4")
            temp = temp.Replace("Õ", "%d5")
            temp = temp.Replace("Ö", "%d6")
            temp = temp.Replace("Ø", "%d8")
            temp = temp.Replace("Ù", "%d9")
            temp = temp.Replace("Ú", "%da")
            temp = temp.Replace("Û", "%db")
            temp = temp.Replace("Ü", "%dc")
            temp = temp.Replace("Ý", "%dd")
            temp = temp.Replace("Þ", "%de")
            temp = temp.Replace("ß", "%df")
            temp = temp.Replace("à", "%e0")
            temp = temp.Replace("á", "%e1")
            temp = temp.Replace("â", "%e2")
            temp = temp.Replace("ã", "%e3")
            temp = temp.Replace("ä", "%e4")
            temp = temp.Replace("å", "%e5")
            temp = temp.Replace("æ", "%e6")
            temp = temp.Replace("ç", "%e7")
            temp = temp.Replace("è", "%e8")
            temp = temp.Replace("é", "%e9")
            temp = temp.Replace("ê", "%ea")
            temp = temp.Replace("ë", "%eb")
            temp = temp.Replace("ì", "%ec")
            temp = temp.Replace("í", "%ed")
            temp = temp.Replace("î", "%ee")
            temp = temp.Replace("ï", "%ef")
            temp = temp.Replace("ð", "%f0")
            temp = temp.Replace("ñ", "%f1")
            temp = temp.Replace("ò", "%f2")
            temp = temp.Replace("ó", "%f3")
            temp = temp.Replace("ô", "%f4")
            temp = temp.Replace("õ", "%f5")
            temp = temp.Replace("ö", "%f6")
            temp = temp.Replace("÷", "%f7")
            temp = temp.Replace("ø", "%f8")
            temp = temp.Replace("ù", "%f9")
            temp = temp.Replace("ú", "%fa")
            temp = temp.Replace("û", "%fb")
            temp = temp.Replace("ü", "%fc")
            temp = temp.Replace("ý", "%fd")
            temp = temp.Replace("þ", "%fe")
            temp = temp.Replace("ÿ", "%ff")
            temp = temp.Replace(" ", "+")
            temp = temp.Replace("&", "%26")
            fanarturl = fanarturl & temp & "+" & year
            fanarturl = fanarturl & "&sitesearch=www.impawards.com"
            'Try
            Dim apple2(2000) As String
            Dim fanartlinecount As Integer = 0
            Dim wrGETURL2 As WebRequest
            wrGETURL2 = WebRequest.Create(fanarturl)
            Dim myProxy2 As New WebProxy("myproxy", 80)
            myProxy2.BypassProxyOnLocal = True
            Dim objStream2 As IO.Stream
            objStream2 = wrGETURL2.GetResponse.GetResponseStream()
            Dim objReader2 As New IO.StreamReader(objStream2)
            Dim sLine2 As String = ""
            fanartlinecount = 0

            Do While Not sLine2 Is Nothing
                fanartlinecount += 1
                sLine2 = objReader2.ReadLine
                apple2(fanartlinecount) = sLine2
            Loop
            objReader2.Close()
            objStream2.Close()
            objReader2 = Nothing
            objStream2 = Nothing
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
                        ReDim apple2(2000)
                        fanartlinecount = 0
                        Dim wrGETURL4 As WebRequest
                        wrGETURL4 = WebRequest.Create(fanarturl)
                        Dim myProxy4 As New WebProxy("myproxy", 80)
                        myProxy4.BypassProxyOnLocal = True
                        Dim objStream4 As IO.Stream
                        objStream4 = wrGETURL4.GetResponse.GetResponseStream()
                        Dim objReader4 As New IO.StreamReader(objStream4)
                        Dim sLine4 As String = ""
                        fanartlinecount = 0

                        Do While Not sLine4 Is Nothing
                            fanartlinecount += 1
                            sLine4 = objReader4.ReadLine
                            apple2(fanartlinecount) = sLine4
                        Loop
                        objReader4.Close()
                        objStream4.Close()
                        objReader4 = Nothing
                        objStream4 = Nothing
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

            ReDim apple2(2000)
            fanartlinecount = 0
            Dim wrGETURL3 As WebRequest

            wrGETURL3 = WebRequest.Create(fanarturl)
            Dim myProxy3 As New WebProxy("myproxy", 80)
            myProxy3.BypassProxyOnLocal = True
            Dim objStream3 As IO.Stream
            objStream3 = wrGETURL3.GetResponse.GetResponseStream()
            Dim objReader3 As New IO.StreamReader(objStream3)
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


            'Catch
            '    thumburl = "na"
            'End Try

            Return thumburl
        Catch
        Finally
            Monitor.Exit(Me)
        End Try
        Return "Error"
    End Function
    Public Function imdbthumb(ByVal posterimdbid As String)

        Monitor.Enter(Me)
        Dim thumburl As String = "na"
        Try
            Dim allok As Boolean = False


            Dim temp As String = posterimdbid
            Dim fanarturl As String = "http://www.imdb.com/title/" & temp

            Dim apple2(2000) As String
            Dim fanartlinecount As Integer = 0

            Dim wrGETURL As WebRequest

            wrGETURL = WebRequest.Create(fanarturl)
            Dim myProxy As New WebProxy("myproxy", 80)
            myProxy.BypassProxyOnLocal = True
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            Dim sLine As String = ""
            fanartlinecount = 0

            Do While Not sLine Is Nothing
                fanartlinecount += 1
                sLine = objReader.ReadLine
                apple2(fanartlinecount) = sLine
            Loop

            fanartlinecount -= 1
            For f = 1 To fanartlinecount
                If apple2(f).IndexOf("<div class=""photo"">") <> -1 Then
                    thumburl = apple2(f + 1)
                    thumburl = thumburl.Substring(thumburl.IndexOf("http"), thumburl.IndexOf("._V1") - thumburl.IndexOf("http"))
                    thumburl = thumburl & "._V1._SX1500_SY1000_.jpg"
                End If
            Next

            If thumburl.IndexOf("http") = -1 Or thumburl.IndexOf(".jpg") = -1 Then thumburl = "na"

            Return thumburl
        Catch
            thumburl = "na"
        Finally

            Monitor.Exit(Me)
        End Try
        Return "Error"
    End Function
End Class
