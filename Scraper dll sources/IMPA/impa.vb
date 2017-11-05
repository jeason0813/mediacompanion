'Imports System.Threading
Imports System.Net
'Imports System.IO
'Imports System.Data
'Imports System.Text.RegularExpressions

Public Class getimpaposters
    Public MCProxy As WebProxy

    Public Function getimpaafulllist(ByVal title As String, Optional ByVal movieyear As String = "")
        Dim fanarturl As String
        Dim fanartlinecount As Integer = 0
        Dim allok As Boolean = True
        Dim apple2(10000)
        
        fanarturl = "http://www.google.com/custom?hl=en&client=pub-6811780361519631&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="
        'fanarturl = "http://www.impawards.com/googlesearch.html?cx=partner-pub-6811780361519631%3A48v46vdqqnk&cof=FORID%3A9&ie=ISO-8859-1&q="

        title = cleantitle(title)

        fanarturl = fanarturl & title & "+" & movieyear
        fanarturl = fanarturl & "&sitesearch=www.impawards.com"
        ReDim apple2(10000)
        Dim wrGETURL2 As WebRequest = WebRequest.Create(fanarturl)
        wrGETURL2.Proxy = MCProxy 
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
        fanartlinecount -= 1

        For f = 1 To fanartlinecount
            If apple2(f).indexof("http://www.impawards.com/") <> -1 Then
                Dim first As Integer = apple2(f).indexof("http://www.impawards.com/")
                apple2(f) = apple2(f).substring(first, apple2(f).length - first)
                fanarturl = apple2(f).substring(0, apple2(f).indexof("html") + 4)
            End If
        Next
        
        Dim tempint As Integer
        Dim tempstring As String
        tempstring = fanarturl.Replace("http://", "")
        tempint = tempstring.LastIndexOf("/")
        If tempint - 5 = tempstring.IndexOf("/") Then
            allok = True
        Else
            'fanarturl = "http://www.impawards.com/googlesearch.html?cx=partner-pub-6811780361519631%3A48v46vdqqnk&cof=FORID%3A9&ie=ISO-8859-1&q="
            fanarturl = "http://www.google.com/custom?hl=en&client=pub-6811780361519631&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="
            fanarturl = fanarturl & title
            fanarturl = fanarturl & "&sitesearch=www.impawards.com"
            ReDim apple2(2000)
            fanartlinecount = 0
            Dim wrGETURL4 As WebRequest = WebRequest.Create(fanarturl)
            wrGETURL4.Proxy = MCProxy
            
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
            fanartlinecount -= 1
            Dim first As Integer
            For g = 1 To fanartlinecount
                If apple2(g).IndexOf("http://www.impawards.com/") <> -1 Then
                    first = apple2(g).IndexOf("http://www.impawards.com/")
                    apple2(g) = apple2(g).Substring(first, apple2(g).Length - first)
                    fanarturl = apple2(g).Substring(0, apple2(g).IndexOf("html") + 4)
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
        
        Dim count As Integer = 1
        If fanarturl.IndexOf("art_machine") = -1 And allok = True Then
            ReDim apple2(10000)
            fanartlinecount = 0
            Dim wrGETURL As WebRequest = WebRequest.Create(fanarturl)
            wrGETURL.Proxy = MCProxy 
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            Dim sLine As String = ""
            fanartlinecount = -1
            Dim vertest As Boolean = False
            Do While Not sLine Is Nothing
                fanartlinecount += 1
                sLine = objReader.ReadLine
                apple2(fanartlinecount) = sLine
            Loop
            fanartlinecount -= 1
            Dim highest As Integer = 0
            Dim version As Boolean = False
            For f = 1 To fanartlinecount
                If apple2(f).indexof("_ver") <> -1 Then
                    For g = 1 To 50
                        Dim tempstring2 As String = "_ver" & g.ToString & "."
                        If apple2(f).IndexOf(tempstring2) <> -1 Then
                            If g = 1 Then
                                version = True
                            End If
                            If g > highest Then
                                highest = g
                            End If
                        End If
                    Next
                End If
            Next
            Dim tempstring3 As String
            Dim tempstring4 As String
            tempstring3 = fanarturl.Substring(0, fanarturl.LastIndexOf("/") + 1)
            tempstring4 = fanarturl.Substring(fanarturl.LastIndexOf("/") + 1, fanarturl.Length - fanarturl.LastIndexOf("/") - 1)
            fanarturl = tempstring3 & "posters/" & tempstring4
            fanarturl = fanarturl.Replace(".html", "")
            If fanarturl.IndexOf("_ver") <> -1 Then
                fanarturl = fanarturl.Substring(0, fanarturl.IndexOf("_ver"))
            End If
            fanarturl = fanarturl.Replace("_gallery", "")
            Dim posterurls(1000, 1)
            If highest > count Then count = highest
            If version = True Then
                posterurls(1, 1) = fanarturl & "_ver1.jpg"
                posterurls(1, 0) = fanarturl & "_ver1_xlg.jpg"
            Else
                posterurls(1, 1) = fanarturl & ".jpg"
                posterurls(1, 0) = fanarturl & "_xlg.jpg"
            End If

            For f = 2 To count
                posterurls(f, 1) = fanarturl & "_ver" & f.ToString & ".jpg"
                posterurls(f, 0) = fanarturl & "_ver" & f.ToString & "_xlg.jpg"
            Next

            Dim finalposters(count, 1) As String
            Dim counter As String = 0
            For f = 1 To count
                Try
                    If posterurls(f, 0).ToLower.IndexOf("http") <> -1 And posterurls(f, 0).tolower.indexof(".jpg") <> -1 Then
                        If posterurls(f, 0).tolower.indexof("http://www.google.com/") = -1 Then
                            finalposters(counter, 0) = posterurls(f, 0)
                            finalposters(counter, 1) = posterurls(f, 1)
                            counter += 1
                        End If
                    End If
                Catch
                End Try
            Next
            Return finalposters
        Else
            Return "error"
        End If
    End Function

    Public Function getimpathumbs(ByVal title As String, Optional ByVal movieyear As String = "")
        Dim url As String
        Dim fanartlinecount As Integer = 0
        Dim allok As Boolean = True
        Dim apple2(10000)
        Dim count As Integer = 0
        Dim posterurls As New List(Of String)
        url = "http://www.google.com/custom?hl=en&client=pub-6811780361519631&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="
        'fanarturl = "http://www.impawards.com/googlesearch.html?cx=partner-pub-6811780361519631%3A48v46vdqqnk&cof=FORID%3A9&ie=ISO-8859-1&q="

        title = cleantitle(title)

        If movieyear <> "" Then
            url = url & title & "+" & movieyear
        Else
            url = url & title
        End If
        url = url & "&sitesearch=www.impawards.com"
        ReDim apple2(10000)
        Dim wrGETURL2 As WebRequest = WebRequest.Create(url)
        wrGETURL2.Proxy = MCProxy 
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
        fanartlinecount -= 1

        For f = 1 To fanartlinecount
            If apple2(f).indexof("http://www.impawards.com/") <> -1 Then
                Dim first As Integer = apple2(f).indexof("http://www.impawards.com/")
                apple2(f) = apple2(f).substring(first, apple2(f).length - first)
                url = apple2(f).substring(0, apple2(f).indexof("html") + 4)
            End If
        Next
        
        Dim tempint As Integer
        Dim tempstring As String
        tempstring = url.Replace("http://", "")
        tempint = tempstring.LastIndexOf("/")
        If tempint - 5 = tempstring.IndexOf("/") Then
            allok = True
        Else
            'fanarturl = "http://www.impawards.com/googlesearch.html?cx=partner-pub-6811780361519631%3A48v46vdqqnk&cof=FORID%3A9&ie=ISO-8859-1&q="
            url = "http://www.google.com/custom?hl=en&client=pub-6811780361519631&cof=FORID%3A1%3BGL%3A1%3BLBGC%3A000000%3BBGC%3A%23000000%3BT%3A%23cccccc%3BLC%3A%2333cc33%3BVLC%3A%2333ff33%3BGALT%3A%2333CC33%3BGFNT%3A%23ffffff%3BGIMP%3A%23ffffff%3B&domains=www.impawards.com&ie=ISO-8859-1&oe=ISO-8859-1&q="
            url = url & title
            url = url & "&sitesearch=www.impawards.com"
            ReDim apple2(2000)
            fanartlinecount = 0
            Dim wrGETURL4 As WebRequest = WebRequest.Create(url)
            wrGETURL4.Proxy = MCProxy 
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
            fanartlinecount -= 1
            Dim first As Integer
            For g = 1 To fanartlinecount
                If apple2(g).IndexOf("http://www.impawards.com/") <> -1 Then
                    first = apple2(g).IndexOf("http://www.impawards.com/")
                    apple2(g) = apple2(g).Substring(first, apple2(g).Length - first)
                    url = apple2(g).Substring(0, apple2(g).IndexOf("html") + 4)
                    tempstring = url
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
        
        If url.IndexOf("art_machine") = -1 And allok = True Then
            count = 1
            ReDim apple2(10000)
            fanartlinecount = 0
            Dim wrGETURL As WebRequest = WebRequest.Create(url)
            wrGETURL.Proxy = MCProxy 
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            Dim sLine As String = ""
            fanartlinecount = -1
            Dim vertest As Boolean = False
            Do While Not sLine Is Nothing
                fanartlinecount += 1
                sLine = objReader.ReadLine
                apple2(fanartlinecount) = sLine
            Loop
            fanartlinecount -= 1
            Dim highest As Integer = 0
            Dim version As Boolean = False
            For f = 1 To fanartlinecount
                If apple2(f).indexof("_ver") <> -1 Then
                    For g = 1 To 50
                        Dim tempstring2 As String = "ver" & g.ToString & "."
                        If apple2(f).IndexOf(tempstring2) <> -1 Then
                            If g = 1 Then
                                version = True
                            End If
                            If g > highest Then
                                highest = g
                            End If
                        End If
                    Next
                End If
            Next
            Dim tempstring3 As String
            Dim tempstring4 As String
            tempstring3 = url.Substring(0, url.LastIndexOf("/") + 1)
            tempstring4 = url.Substring(url.LastIndexOf("/") + 1, url.Length - url.LastIndexOf("/") - 1)
            url = tempstring3 & "posters/" & tempstring4
            url = url.Replace(".html", "")
            If url.IndexOf("_ver") <> -1 Then
                url = url.Substring(0, url.IndexOf("_ver"))
            End If

            If highest > count Then count = highest
            If version = True Then
                posterurls.Add(url & "_ver1_xlg.jpg")
            Else
                posterurls.Add(url & "_xlg.jpg")
            End If

            For f = 2 To count
                posterurls.Add(url & "_ver" & f.ToString & ".jpg")
                posterurls.Add(url & "_ver" & f.ToString & "_xlg.jpg")
            Next
        End If
        Dim posterlist As String = ""
        If posterurls.Count > 0 Then
            For Each poster In posterurls
                If poster.IndexOf("http://www.google.com/") = -1 Then
                    posterlist = posterlist & "<thumb>" & poster & "</thumb>"
                End If
            Next
        End If

        Return posterlist
    End Function

    Private Function loadwebpage(ByVal url As String, ByVal method As Boolean)

        Dim webpage As New List(Of String)
        Try
            Dim wrGETURL As WebRequest = WebRequest.Create(url)
            wrGETURL.Proxy = MCProxy 
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream, System.Text.UTF8Encoding.UTF7)
            Dim sLine As String = ""

            If method = False Then
                Do While Not sLine Is Nothing

                    sLine = objReader.ReadLine
                    If Not sLine Is Nothing Then
                        webpage.Add(sLine)
                    End If
                Loop
            Else
                sLine = objReader.ReadToEnd
            End If
            objReader.Close()

            If method = False Then
                Return webpage
            Else
                Return sLine
            End If

        Catch ex As WebException
            'MsgBox("Unable to load webpage " & url & vbCrLf & vbCrLf & ex.ToString)
            If webpage.Count > 0 Then
                Return webpage
            Else
                webpage.Add("error")
                Return webpage
            End If
        End Try
    End Function
    
    Private Function cleantitle(ByVal title) As String
        title = title.ToLower
        title = title.Replace(" ", "+")
        title = title.Replace("&", "%26")
        title = title.Replace("À", "%c0")
        title = title.Replace("Á", "%c1")
        title = title.Replace("Â", "%c2")
        title = title.Replace("Ã", "%c3")
        title = title.Replace("Ä", "%c4")
        title = title.Replace("Å", "%c5")
        title = title.Replace("Æ", "%c6")
        title = title.Replace("Ç", "%c7")
        title = title.Replace("È", "%c8")
        title = title.Replace("É", "%c9")
        title = title.Replace("Ê", "%ca")
        title = title.Replace("Ë", "%cb")
        title = title.Replace("Ì", "%cc")
        title = title.Replace("Í", "%cd")
        title = title.Replace("Î", "%ce")
        title = title.Replace("Ï", "%cf")
        title = title.Replace("Ð", "%d0")
        title = title.Replace("Ñ", "%d1")
        title = title.Replace("Ò", "%d2")
        title = title.Replace("Ó", "%d3")
        title = title.Replace("Ô", "%d4")
        title = title.Replace("Õ", "%d5")
        title = title.Replace("Ö", "%d6")
        title = title.Replace("Ø", "%d8")
        title = title.Replace("Ù", "%d9")
        title = title.Replace("Ú", "%da")
        title = title.Replace("Û", "%db")
        title = title.Replace("Ü", "%dc")
        title = title.Replace("Ý", "%dd")
        title = title.Replace("Þ", "%de")
        title = title.Replace("ß", "%df")
        title = title.Replace("à", "%e0")
        title = title.Replace("á", "%e1")
        title = title.Replace("â", "%e2")
        title = title.Replace("ã", "%e3")
        title = title.Replace("ä", "%e4")
        title = title.Replace("å", "%e5")
        title = title.Replace("æ", "%e6")
        title = title.Replace("ç", "%e7")
        title = title.Replace("è", "%e8")
        title = title.Replace("é", "%e9")
        title = title.Replace("ê", "%ea")
        title = title.Replace("ë", "%eb")
        title = title.Replace("ì", "%ec")
        title = title.Replace("í", "%ed")
        title = title.Replace("î", "%ee")
        title = title.Replace("ï", "%ef")
        title = title.Replace("ð", "%f0")
        title = title.Replace("ñ", "%f1")
        title = title.Replace("ò", "%f2")
        title = title.Replace("ó", "%f3")
        title = title.Replace("ô", "%f4")
        title = title.Replace("õ", "%f5")
        title = title.Replace("ö", "%f6")
        title = title.Replace("÷", "%f7")
        title = title.Replace("ø", "%f8")
        title = title.Replace("ù", "%f9")
        title = title.Replace("ú", "%fa")
        title = title.Replace("û", "%fb")
        title = title.Replace("ü", "%fc")
        title = title.Replace("ý", "%fd")
        title = title.Replace("þ", "%fe")
        title = title.Replace("ÿ", "%ff")
        title = title.Replace(" ", "+")
        title = title.Replace("&", "%26")
        Return title
    End Function

End Class
