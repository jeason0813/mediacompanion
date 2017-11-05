Imports System.IO
Imports System.Net
Imports System.Threading

Public Class Class1

    Public Function get_mpdb_thumbs(ByVal imdbid As String)

        Monitor.Enter(Me)
        Dim count As Integer = 0
        Dim posterurls(500, 1) As String
        Try
            Dim fanarturl As String
            Dim fanartlinecount As Integer = 0
            Dim allok As Boolean = True
            Dim apple2(10000)
            Dim first As Integer
            Dim last As Integer

            fanarturl = "http://www.movieposterdb.com/movie/"
            Dim temp As String = imdbid
            fanarturl = fanarturl & temp.Replace("tt", "")

            Dim wrGETURL2 As WebRequest
            wrGETURL2 = WebRequest.Create(fanarturl)
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

            For f = 1 To fanartlinecount
                If apple2(f).IndexOf("<title>MoviePosterDB.com - Internet Movie Poster DataBase</title>") <> -1 Then
                    allok = False
                End If
            Next

            If allok = True Then
                For f = 1 To fanartlinecount
                    If apple2(f).IndexOf("<img src=""http://www.movieposterdb.com/posters/") <> -1 Then
                        count = count + 1
                        first = apple2(f).IndexOf("http")
                        last = apple2(f).IndexOf("jpg")
                        posterurls(count, 0) = apple2(f).Substring(first, (last + 3) - first)
                        If posterurls(count, 0).IndexOf("t_") <> -1 Then
                            posterurls(count, 0) = posterurls(count, 0).Replace("t_", "l_")
                        End If
                        If posterurls(count, 0).IndexOf("s_") <> -1 Then
                            posterurls(count, 0) = posterurls(count, 0).Replace("s_", "l_")
                        End If
                    End If
                Next

                Dim group(1000) As String
                Dim groupcount As Integer = 0
                For f = 1 To fanartlinecount
                    If apple2(f) <> Nothing Then
                        If apple2(f).IndexOf("http://www.movieposterdb.com/group/") <> -1 Then
                            If apple2(f).IndexOf("http") <> -1 And apple2(f).IndexOf(""">") <> -1 Then
                                groupcount = groupcount + 1
                                first = apple2(f).IndexOf("http")
                                last = apple2(f).IndexOf(""">")
                                group(groupcount) = apple2(f).Substring(first, last - first)
                            End If
                        End If
                    End If
                Next

                If groupcount > 0 Then
                    For g = 1 To groupcount
                        fanarturl = group(g)

                        Dim wrGETURL3 As WebRequest
                        wrGETURL3 = WebRequest.Create(fanarturl)
                        Dim myProxy3 As New WebProxy("myproxy", 80)
                        myProxy3.BypassProxyOnLocal = True
                        ReDim apple2(10000)
                        Dim objStream3 As Stream
                        objStream3 = wrGETURL3.GetResponse.GetResponseStream()
                        Dim objReader3 As New StreamReader(objStream3)
                        Dim sLine3 As String = ""
                        fanartlinecount = 0
                        fanartlinecount = 0
                        Do While Not sLine3 Is Nothing
                            fanartlinecount += 1
                            sLine3 = objReader3.ReadLine
                            apple2(fanartlinecount) = sLine3
                        Loop
                        fanartlinecount -= 1

                        For f = 1 To fanartlinecount
                            If apple2(f) <> Nothing Then
                                If apple2(f).IndexOf("<img src=""http://www.movieposterdb.com/posters/") <> -1 Then
                                    count = count + 1
                                    first = apple2(f).IndexOf("http")
                                    last = apple2(f).IndexOf("jpg")
                                    posterurls(count, 0) = apple2(f).Substring(first, (last + 3) - first)
                                    If posterurls(count, 0).IndexOf("t_") <> -1 Then
                                        posterurls(count, 0) = posterurls(count, 0).Replace("t_", "l_")
                                    End If
                                    If posterurls(count, 0).IndexOf("s_") <> -1 Then
                                        posterurls(count, 0) = posterurls(count, 0).Replace("s_", "l_")
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If
            Else

            End If


            Dim thumblist As String = ""
            For f = 1 To count
                thumblist = thumblist & "<thumb>" & posterurls(f, 0) & "</thumb>"
            Next


            Return thumblist
        Catch ex As Exception
            Return "Error"
        Finally
            Monitor.Exit(Me)
        End Try



    End Function
End Class
