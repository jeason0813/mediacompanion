Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Xml

Public Class Class1

    Public Function gettmdbposters_newapi(ByVal imdbid As String)
        Monitor.Enter(Me)

        Try
            Dim tmdbxml As String
            Dim fanarturl2 As String
            fanarturl2 = "http://api.themoviedb.org/2.1/Movie.getImages/en/xml/3f026194412846e530a208cf8a39e9cb/" & imdbid

            Dim wrGETURL2 As WebRequest
            wrGETURL2 = WebRequest.Create(fanarturl2)
            Dim myProxy2 As New WebProxy("myproxy", 80)
            myProxy2.BypassProxyOnLocal = True
            Dim objStream2 As Stream
            objStream2 = wrGETURL2.GetResponse.GetResponseStream()
            Dim objReader2 As New StreamReader(objStream2)
            Dim sLine2 As String = ""

            tmdbxml = objReader2.ReadToEnd
            objReader2.Close()

            Dim thumbstring As String = "<tmdb_posterlist>" & vbCrLf

            Dim bannerslist As New XmlDocument

            bannerslist.LoadXml(tmdbxml)
            Dim thisresult As XmlNode = Nothing
            For Each item In bannerslist("OpenSearchDescription")
                Select Case item.name
                    Case "movies"
                        For Each result In item
                            Select Case result.name
                                Case "movie"
                                    For Each element In result
                                        Select Case element.name
                                            Case "images"
                                                Dim mode As String = ""
                                                For Each lot In element
                                                    If lot.name = "poster" Then
                                                        mode = "poster"
                                                    ElseIf lot.name = "backdrop" Then
                                                        mode = "fanart"
                                                    End If
                                                    thumbstring = thumbstring & "<" & mode & ">" & vbCrLf
                                                    For Each node In lot.childnodes
                                                        Dim size As String = ""
                                                        Dim height As String = ""
                                                        Dim width As String = ""
                                                        Dim url As String = ""
                                                        thumbstring = thumbstring & "<pic>" & vbCrLf
                                                        For f = 0 To 3
                                                            Select Case node.attributes(f).name
                                                                Case "url"
                                                                    url = node.attributes(f).value
                                                                Case "size"
                                                                    size = node.attributes(f).value
                                                                Case "width"
                                                                    width = node.attributes(f).value
                                                                Case "height"
                                                                    height = node.attributes(f).value
                                                            End Select
                                                        Next
                                                        thumbstring = thumbstring & "<type>" & size & "</type>" & vbCrLf
                                                        thumbstring = thumbstring & "<url>" & url & "</url>" & vbCrLf
                                                        thumbstring = thumbstring & "<width>" & width & "</width>" & vbCrLf
                                                        thumbstring = thumbstring & "<height>" & height & "</height>" & vbCrLf
                                                        thumbstring = thumbstring & "</pic>" & vbCrLf
                                                    Next
                                                    thumbstring = thumbstring & "</" & mode & ">" & vbCrLf
                                                Next
                                        End Select
                                    Next
                            End Select
                        Next
                End Select
            Next

            thumbstring = thumbstring & "</tmdb_posterlist>"
            Return thumbstring

        Catch ex As Exception
            Return "Error"
        Finally
            Monitor.Exit(Me)
        End Try

    End Function


    Public Function gettmdbposters(ByVal imdbid As String)
        Monitor.Enter(Me)
        Dim fanarturl As String
        Dim tempsimdbid As String
        Dim count As Integer
        Dim posterurls(1000, 1)

        Try

            fanarturl = "http://api.themoviedb.org/2.0/Movie.imdbLookup?imdb_id=" & imdbid & "&api_key=3f026194412846e530a208cf8a39e9cb"
            Dim apple2(2000) As String
            Dim fanartlinecount As Integer = 0

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
                If sLine.IndexOf("<id>") <> -1 Then
                    tempsimdbid = sLine
                    tempsimdbid = tempsimdbid.Replace("<id>", "")
                    tempsimdbid = tempsimdbid.Replace("</id>", "")
                    tempsimdbid = tempsimdbid.Replace("  ", "")
                    tempsimdbid = tempsimdbid.Trim
                    Exit Do
                End If
            Loop
            objReader.Close()



            ReDim apple2(2000)
            fanartlinecount = 0
            Dim fanarturl2 As String
            fanarturl2 = "http://api.themoviedb.org/2.0/Movie.getInfo?id=" & tempsimdbid & "&api_key=3f026194412846e530a208cf8a39e9cb"


            Dim wrGETURL2 As WebRequest
            wrGETURL2 = WebRequest.Create(fanarturl2)
            Dim myProxy2 As New WebProxy("myproxy", 80)
            myProxy2.BypassProxyOnLocal = True
            Dim objStream2 As Stream
            objStream2 = wrGETURL2.GetResponse.GetResponseStream()
            Dim objReader2 As New StreamReader(objStream2)
            Dim sLine2 As String = ""
            fanartlinecount = 0

            Do While Not sLine2 Is Nothing
                fanartlinecount += 1
                sLine2 = objReader2.ReadLine
                apple2(fanartlinecount) = sLine2
            Loop
            fanartlinecount -= 1

            objReader2.Close()

            For f = 1 To fanartlinecount
                If apple2(f).IndexOf("<poster size=""original"">") <> -1 Then
                    count += 1
                    posterurls(count, 0) = apple2(f)
                    If apple2(f + 1).IndexOf("<poster size=""mid"">") <> -1 Then
                        posterurls(count, 1) = apple2(f + 1)
                    ElseIf posterurls(count, 1) = Nothing And apple2(f + 2).IndexOf("<poster size=""mid"">") <> -1 Then
                        posterurls(count, 1) = apple2(f + 2)
                    ElseIf posterurls(count, 1) = Nothing And apple2(f - 1).IndexOf("<poster size=""mid"">") <> -1 Then
                        posterurls(count, 1) = apple2(f - 1)
                    ElseIf posterurls(count, 1) = Nothing And apple2(f - 2).IndexOf("<poster size=""mid"">") <> -1 Then
                        posterurls(count, 1) = apple2(f - 2)
                    End If
                    posterurls(count, 0) = posterurls(count, 0).Replace("<poster size=""original"">", "")
                    posterurls(count, 0) = posterurls(count, 0).Replace("</poster>", "")
                    posterurls(count, 1) = posterurls(count, 1).Replace("<poster size=""mid"">", "")
                    posterurls(count, 1) = posterurls(count, 1).Replace("</poster>", "")
                End If
            Next
            Dim thumbs As String = ""
            For f = 1 To count
                thumbs = thumbs & "<thumb>" & posterurls(f, 0) & "</thumb>"
            Next


            Return thumbs
        Catch
            Return "Error"
        Finally
            Monitor.Exit(Me)
        End Try
    End Function
End Class
