Imports System.Threading
Imports System.Net
'Imports System.IO
'Imports System.Data
'Imports System.Text.RegularExpressions



Public Class Class1
    Public MCProxy As WebProxy
    
    Public Function getimdbposters(ByVal imdbid As String)
        Dim posters(10000, 1) As String
        Dim postercount As Integer = 0
        Dim fanarturl As String
        Dim fanartlinecount As Integer = 0
        Dim allok As Boolean = True
        Dim apple2(10000)

        fanarturl = "http://www.imdb.com/title/" & imdbid & "/mediaindex?refine=poster&ref_=ttmi_ref_pos"

        Dim wrGETURL2 As WebRequest = WebRequest.Create(fanarturl)
        wrGETURL2.Proxy = MCProxy
        'Dim myProxy2 As New WebProxy("myproxy", 80)
        'myProxy2.BypassProxyOnLocal = True
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

        Dim totalpages As Integer
        Dim tempint As Integer
        Dim reached As Boolean = False
        Try
        For f = 1 To fanartlinecount
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
        Catch ex As Exception 
        End Try
        
        Dim finalposters(postercount, 1) As String
        Dim counter As String = 0
        For f = postercount-1 To 0 Step -1
            Try
                If posters(f, 0).ToLower.IndexOf("http") <> -1 Then
                    finalposters(counter, 0) = posters(f, 0) & "._V1_UY720_AL_.jpg" '".jpg" '"_V1._SX1000_SY1000_.jpg"
                    finalposters(counter, 1) = posters(f, 1) & "._V1_UY1080_AL_.jpg" '".jpg" '"_V1._SX1000_SY1000_.jpg"
                    counter += 1
                End If
            Catch
            End Try
        Next
        Return finalposters
    End Function


    Private Function getimdbID(ByVal title As String, Optional ByVal year As String = "")
        Dim newimdbid As String = ""
        Dim allok As Boolean = False
        Dim goodyear As Boolean = False
        If IsNumeric(year) Then
            If year.Length = 4 Then
                goodyear = True
            End If
        End If

        Dim url As String = "http://www.google.co.uk/search?hl=en&q=%3C"
        Dim titlesearch As String = title
        titlesearch = titlesearch.Replace(" ", "+")
        titlesearch = titlesearch.Replace("·", "%C2%B7")
        titlesearch = titlesearch.Replace("Æ", "%C3%86")
        'titlesearch = titlesearch.Replace("", "")
        
        If goodyear = True Then
            titlesearch = titlesearch & "+" & year
        End If
        url = url & titlesearch & "%3E+site%3Aimdb.com&meta="

        Dim webpage As String = loadwebpage(url, True)


        'www.imdb.com/title/tt0402022
        If webpage.IndexOf("www.imdb.com/title/tt") <> -1 Then
            newimdbid = webpage.Substring(webpage.IndexOf("www.imdb.com/title/tt") + 19, 9)
        End If

        If newimdbid <> "" And newimdbid.IndexOf("tt") = 0 And newimdbid.Length = 9 Then
            allok = True
        End If

        If allok = False Then
            'try other method
        End If
        
        Return newimdbid
    End Function
    Private Function loadwebpage(ByVal url As String, ByVal method As Boolean)

        Dim webpage As New List(Of String)
        Try
            Dim wrGETURL As WebRequest = WebRequest.Create(url)
            wrGETURL.Proxy = MCProxy 
            'Dim myProxy As New WebProxy("myproxy", 80)
            'myProxy.BypassProxyOnLocal = True
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

    Public Function getimdbthumbs(Optional ByVal title As String = "", Optional ByVal year As String = "", Optional ByVal imdb As String = "")
        Dim imdbid As String = ""
        Dim count As Integer = 0
        Dim posterurls(1000) As String
        Monitor.Enter(Me)
        Dim thumbs As String = ""

        Try
            If imdb = "" Then
                If title = "" Then
                    Return "Error: Must have title or IMDBID"
                    Exit Function
                ElseIf title <> "" Then
                    If year <> "" Then
                        imdbid = getimdbID(title, year)
                    Else
                        imdbid = getimdbID(title)
                    End If
                End If
            Else
                imdbid = imdb
            End If

            Dim fanarturl As String
            Dim fanartlinecount As Integer = 0
            Dim allok As Boolean = True
            Dim apple2(10000)

            fanarturl = "http://www.imdb.com/title/" & imdbid & "/mediaindex"

            Dim wrGETURL2 As WebRequest = WebRequest.Create(fanarturl)
            wrGETURL2.Proxy = MCProxy 
            'Dim myProxy2 As New WebProxy("myproxy", 80)
            'myProxy2.BypassProxyOnLocal = True
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

            Dim totalpages As Integer
            Dim tempint As Integer
            Dim reached As Boolean = False
            For f = 1 To fanartlinecount
                If apple2(f).IndexOf("<a href=""?page=") <> -1 Then
                    apple2(f) = apple2(f).Replace("<a href=""?page=", "")
                    apple2(f) = apple2(f).Substring(0, 1)
                    tempint = Convert.ToString(apple2(f))
                    If tempint > totalpages Then totalpages = tempint
                End If
                If apple2(f).IndexOf("<div class=""thumb_list""") <> -1 Then
                    reached = True
                End If
                If reached = True Then
                    If apple2(f).IndexOf("</div>") <> -1 Then
                        reached = False
                        Exit For
                    End If
                    If apple2(f).IndexOf("src=""http://") <> -1 Then
                        apple2(f) = apple2(f).Substring(apple2(f).IndexOf("src=""") - 1, apple2(f).Length - apple2(f).IndexOf("src=""") - 1)
                        apple2(f).TrimStart()
                        apple2(f) = apple2(f).Replace("src=""", "")
                        count = count + 1
                        posterurls(count) = apple2(f).Substring(1, apple2(f).IndexOf("._V1._"))
                    End If
                End If
            Next
            For g = 2 To totalpages
                fanarturl = "http://www.imdb.com/title/" & imdbid & "/mediaindex?page=" & g.ToString
                ReDim apple2(10000)
                Dim wrGETURL As WebRequest = WebRequest.Create(fanarturl)
                wrGETURL.Proxy = MCProxy 
                'Dim myProxy As New WebProxy("myproxy", 80)
                'myProxy.BypassProxyOnLocal = True
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
                    If apple2(f).IndexOf("<div class=""thumb_list""") <> -1 Then
                        reached = True
                    End If
                    If reached = True Then
                        If apple2(f).IndexOf("</div>") <> -1 Then
                            reached = False
                            Exit For
                        End If
                        If apple2(f).IndexOf("src=""http://") <> -1 Then
                            apple2(f) = apple2(f).Substring(apple2(f).IndexOf("src=""") - 1, apple2(f).Length - apple2(f).IndexOf("src=""") - 1)
                            apple2(f).TrimStart()
                            apple2(f) = apple2(f).Replace("src=""", "")
                            count = count + 1
                            posterurls(count) = apple2(f).Substring(1, apple2(f).IndexOf("._V1._"))
                        End If
                    End If
                Next
            Next
            
            For f = 1 To count
                posterurls(f) = "<thumb>" & posterurls(f) & "_V1._SX1000_SY1000_.jpg" & "</thumb>"
                'posterurls(f) = encodespecialchrs(posterurls(f))
            Next
            
            For f = 1 To count
                thumbs = thumbs & posterurls(f)
            Next
            
            Return thumbs
        Catch
            Return thumbs
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Private Function encodespecialchrs(ByVal text As String)
        If text.IndexOf("&") <> -1 Then text = text.Replace("&", "&amp;")
        If text.IndexOf("<") <> -1 Then text = text.Replace("", "&lt;")
        If text.IndexOf(">") <> -1 Then text = text.Replace("", "&gt;")
        If text.IndexOf(Chr(34)) <> -1 Then text = text.Replace(Chr(34), "&quot;")
        If text.IndexOf("'") <> -1 Then text = text.Replace("'", "&apos;")
        Return text
    End Function
End Class
