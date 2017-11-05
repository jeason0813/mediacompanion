Imports System
Imports System.Threading
Imports System.Net
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Web
Imports System.Data
Imports System.Text.RegularExpressions
Imports Media_Companion
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.IO.Compression
Imports System.xml
Imports System.Linq


Public Class MovieRegExs
    Public Const quote                      = """"
    Public Const REGEX_HREF_PATTERN         = "<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>"
    Public Const REGEX_RELEASE_DATE         = ">Release Date:</h4>(?<date>.*?)<span"
    Public Const REGEX_STARS                = "Stars:</h4>(.*?)</div>"
    Public Const REGEX_TITLE_AND_YEAR       = "<title>(.*?)</title>"
    Public Const REGEX_TITLE                = "<title>(.*?) \("
    Public Const REGEX_YEAR                 = "\(.*?(\d{4}).*?\)" 
    Public Const REGEX_NAME                 = "itemprop=""name"">(?<name>.*?)</span>"                '"<span.*?>(?<name>.*?)</span>"
    Public Const REGEX_STUDIO               = "<h4 class=""inline"">Production.*?/h4>(.*?)</div>"
    Public Const REGEX_CREDITS              = "<h4 class=""inline"">Writers?:</h4>(.*?)</div>"
    Public Const REGEX_ORIGINAL_TITLE       = "<span class=""title-extra"" itemprop=""name"">(.*?)<i>\(original title\)</i>"
    Public Const REGEX_OUTLINE              = "itemprop=""description"">(?<outline>.*?)<div"
    Public Const REGEX_ACTORS_TABLE         = "<table class=""cast_list"">(.*?)</table>"
    Public Const REGEX_TR                   = "<tr.*?>(.*?)</tr>"
    Public Const REGEX_ACTOR_NO_IMAGE       = "<td.*?>.*?<a href=""/name/nm(?<actorid>.*?)/.*?_=ttfc_fc_cl_i(?<order>.*?)"".*?title=""(?<actorname>.*?)"".*?=""character"".*?<div>(?<actorrole>.*?)</div>"
    Public Const REGEX_ACTOR_WITH_IMAGE     = "<td.*?>.*?<a href=""/name/nm(?<actorid>.*?)/.*?_=ttfc_fc_cl_i(?<order>.*?)"".*?title=""(?<actorname>.*?)"".*?loadlate=""(?<actorthumb>.*?)"".*?=""character"".*?<div>(?<actorrole>.*?)</div>"
    Public Const REGEX_IMDB_KEYWORD         = "ttkw_kw_[0-9]*"">(?<keyword>.*?)<\/a"
    Public Const REGEX_IMDB_TRAILER         = "<h2><a href=""/video/imdb/vi(.*?)/"
    Public Const REGEX_TOP_250              = "=tt_awd""> Top Rated Movies #(.*?)</a>"    '"<strong>Top 250 Movies #(.*?)</strong></a>"
    Public Const REGEX_VOTES                = "itemprop=""ratingCount"">([\d{1,3},.?\s]*[0-9]?)</span>"
    Public Const REGEX_TAGLINE              = "<h4 class=""inline"">Tagline.*?:</h4>(.*?)<"
    Public Const REGEX_RUNTIME              = "<h4 class=""inline"">Runtime:</h4>(.*?)</div>"
    Public Const REGEX_DURATION             = "<time itemprop=""duration"" datetime="".*?"">(.*?) min</time>"
    Public Const REGEX_COUNTRYS             = "class=""inline"">Countr(.*?)</div>"
    Public Const REGEX_COUNTRY              = "itemprop='url'>(.*?)</a>"
    Public Const REGEX_RATING               = "<span itemprop=""ratingValue"">(.*?)</span>"
    Public Const REGEX_RUNTIMETECH          = "class=""label""> Runtime <\/td>(.*?)<\/tr>"
    Public Const REGEX_DURATIONTECH         = "\((.*?) min"
    Public Const REGEX_DURATIONTECH2        = "(\d*?) min"
    Public Const REGEX_ASPECTRATIO          = "class=""inline"">Aspect Ratio:</h4>(.*?)</div>"
    Public Const REGEX_ASPECTRATIOALL       = "class=""label""> Aspect Ratio </td>(.*?)</td>"
End Class


Module ModGlobals
    Dim arrLetters1
    Dim arrLetters2

    <Extension()>
    Public Function CompareString(String1 As String, String2 As String) As Double
        Dim intLength1
        Dim intLength2
        Dim x
        Dim dblResult
        
        If UCase(String1) = UCase(String2) Then
            dblResult = 1
        Else
            intLength1 = Len(String1)
            intLength2 = Len(String2)
            If intLength1 = 0 Or intLength2 = 0 Then
                dblResult = 0
            Else
                ReDim arrLetters1(intLength1 - 1)
                ReDim arrLetters2(intLength2 - 1)
                For x = LBound(arrLetters1) To UBound(arrLetters1)
                    arrLetters1(x) = Asc(UCase(Mid(String1, x + 1, 1)))
                Next
                For x = LBound(arrLetters2) To UBound(arrLetters2)
                    arrLetters2(x) = Asc(UCase(Mid(String2, x + 1, 1)))
                Next
                dblResult = SubSim(1, intLength1, 1, intLength2) / (intLength1 + intLength2) * 2
            End If
        End If
        CompareString = dblResult
    End Function

    Private Function SubSim(intStart1, intEnd1, intStart2, intEnd2) As Double
        Dim intMax As Integer = Integer.MinValue
        Try
            Dim y
            Dim z
            Dim ns1 As Integer
            Dim ns2 As Integer
            Dim i
            If (intStart1 > intEnd1) Or (intStart2 > intEnd2) Or (intStart1 <= 0) Or (intStart2 <= 0) Then
                Return 0
            End If
            For y = intStart1 To intEnd1
                For z = intStart1 To intEnd2
                    i = 0
                    Do Until arrLetters1(y - 1 + i) <> arrLetters2(z - 1 + i)
                        i = i + 1
                        If i > intMax Then
                            ns1 = y
                            ns2 = z
                            intMax = i
                        End If
                        If ((y + i) > intEnd1) Or ((z + i) > intEnd2) Then
                            Exit Do
                        End If
                    Loop
                Next
            Next
            intMax = intMax + SubSim(ns1 + intMax, intEnd1, ns2 + intMax, intEnd2)
            intMax = intMax + SubSim(intStart1, ns1 - 1, intStart2, ns2 - 1)
        Catch ex As OverflowException
            Return Nothing
        Catch ex As StackOverflowException
            Return Nothing
        End Try

        Return intMax
    End Function

    <Extension()> _
    Function CleanTitle(ByVal sString As String) As String
        Dim s As String = sString
        If s.StartsWith("""") Then s = s.Remove(0, 1)
        If s.EndsWith  ("""") Then s = s.Remove(s.Length - 1, 1)
        Return s
    End Function

    <Extension()> _
    Sub AppendValue(ByRef s As String, value As String, Optional separator As String=", ")
        value = value.Trim
        If s.IndexOf(value) > -1 Then Exit Sub
        If s="" Then
            s  = value
        Else
            s &= separator & value
        End If
    End Sub

    <Extension()> _
    Sub AppendList(ByRef s As String, lst As IEnumerable(Of String) , Optional separator As String=", ")
        For Each m In lst
            m.ExtractName
            s.AppendValue(m, separator)
        Next
    End Sub

    <Extension()> _
    Sub AppendTag(ByRef s As String, name As String, value As String)
        s &= "<" & name & ">" & value.Trim.EncodeSpecialChrs & "</" & name & ">"& vbCrLf
    End Sub

    <Extension()> _
    Sub AppendTagText(ByRef s As String, name As String, value As String)
        s &= "<" & name & ">" & DeCodeSpecialChrs(value.Trim.EncodeSpecialChrs) & "</" & name & ">"& vbCrLf
    End Sub

    <Extension()> _
    Sub ExtractName(ByRef s As String)
        If s.IndexOf("itemprop=""name"">")>-1 Then s=Net.WebUtility.HtmlDecode( Regex.Match(s,MovieRegExs.REGEX_NAME, RegexOptions.Singleline).Groups("name").Value )
    End Sub

    <Extension()> _
    Function StripTagsLeaveContent(ByRef s As String) As String
        Return Net.WebUtility.HtmlDecode( Regex.Replace(s, "<(.|\n)+?>", String.Empty) )
    End Function  

    <Extension()> _
    Function EncodeSpecialChrs(ByRef s As String) As String
        s = s.Replace("&", "&amp;")
        s = s.Replace("<", "&lt;")
        s = s.Replace(">", "&gt;")
        s = s.Replace(Chr(34), "&quot;")
        s = s.Replace(Chr(31), "")
        s = s.Replace("'", "&apos;")
        Return s
    End Function

    <Extension()> _
    Function DeCodeSpecialChrs(ByRef s As String) As String
        s = s.Replace("&amp;", "&")
        s = s.Replace("&apos;", "'")
        s = s.Replace("&quot;", """")
        Return s
    End Function

    <Extension()> _
    Function CleanSpecChars(ByRef s As String) As String
        Return Utilities.cleanSpecChars(s)
    End Function

    <Extension()> _
    Function CleanFilenameIllegalChars(ByRef s As String) As String
        Return Utilities.cleanFilenameIllegalChars(s)
    End Function

    <Extension()> _
    Function CleanActorRole(ByRef s As String) As String
        Dim a As Integer = s.IndexOf("  ")-1
        Dim Words As String() = s.Split(Chr(32)& Chr(32))
        If a >0 Then
            s = s.Substring(0, a)
        End If
        s = s.Replace(" /","")
        Return s
    End Function

    <Extension()> _
    Function StripTags(ByRef s As String) As String
	' Remove HTML tags.
	Return Regex.Replace(s, "<.*?>", "")
    End Function
 
    <Extension()> _
    Function ContainsHtml(ByRef s As String) As Boolean
	    Return Regex.Match(s, "<\s*\w.*?>").Success
    End Function
    
End Module


Public Class Classimdb
    Property Html As String=""
    Property Html2 As String = ""

    Public Function getimdbID(ByVal title As String, ByRef year As String, Optional ByVal imdbmirror As String = "")
        If imdbmirror = "" Then
            imdbmirror = "http://www.imdb.com/"
        End If
        Monitor.Enter(Me)
        Try
            Dim engine As Integer = Pref.engineno 
            Dim newimdbid As String = ""
            Dim goodyear As Boolean = False
            If IsNumeric(year) AndAlso year.Length = 4 Then goodyear = True
            
            'Dim url As String = "http://www.google.co.uk/search?hl=en&q=%3C"
            'Dim url As String = "http://www.google.co.uk/search?hl=en-US&as_q="
            Dim url As String = Pref.enginefront(engine)
            Dim titlesearch As String = Utilities.searchurltitle(title)
            If goodyear = True Then titlesearch = titlesearch & "+%28" & year & "%29"

            url = url & titlesearch & Pref.engineend(engine)
            Dim webpage As String = loadwebpage(Pref.proxysettings, url, True)

            'www.imdb.com/title/tt0402022
            '''Try one of the search engines
            If webpage.IndexOf("imdb.com/title/tt") <> -1 Then newimdbid = webpage.Substring(webpage.IndexOf("imdb.com/title/tt") + 15, 9)
            If newimdbid <> "" AndAlso newimdbid.StartsWith("tt") AndAlso newimdbid.Length = 9 Then Return newimdbid
            
            '''Next try IMDb itself
            If newimdbid = "" Then newimdbid = getimdbID_fromimdb(title, imdbmirror, year)
            If newimdbid <> "" AndAlso newimdbid.StartsWith("tt") = 0 AndAlso newimdbid.Length = 9 Then Return newimdbid

            '''Last resort try Omdbapi
            newimdbid = getimdbID_fromOmdbapi(title, year)
            If newimdbid <> "" And newimdbid.IndexOf("tt") = 0 And newimdbid.Length = 9 Then Return newimdbid

            Return ""
        Catch
            Return ""
        Finally
            Monitor.Exit(Me)
        End Try
    End Function
    
    Public Function getimdbID_fromOmdbapi(BYVal title As String, ByRef year As String) As String
        Dim GOT_IMDBID As String = ""
        Try
            title = title.Replace("  ", "+").Replace(" ", "+").Replace("&", "%26")
            Dim url As String = String.Format("{0}?s={1}&y={2}&plot=full&r=xml", Pref.Omdbapiurl, title, year)
            Dim result As String = loadwebpage(Pref.proxysettings, url, True, 5)
            If result = "error" Then Return ""
            Dim adoc As New XmlDocument
            adoc.LoadXml(result)
            If adoc("root").HasAttribute("Response") AndAlso adoc("root").Attributes("Response").Value = "False" Then Return "Error"
            If adoc("root").HasAttribute("response") AndAlso adoc("root").Attributes("response").Value = "False" Then Return "Error"
            For each thisresult As XmlNode In adoc("root")
                If Not IsNothing(thisresult.Attributes.ItemOf("imdbID")) Then
                    Dim TmpValue As String = thisresult.Attributes("imdbID").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then GOT_IMDBID = TmpValue
                End If
                If year = "" AndAlso Not IsNothing(thisresult.Attributes.ItemOf("year")) Then
                    Dim TmpValue As String = thisresult.Attributes("year").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then year = TmpValue
                End If
                If GOT_IMDBID <> "" Then Exit For
            Next
        Catch
            Return ""
        End Try
        Return GOT_IMDBID
    End Function
    
    Public Function getimdbID_fromimdb(ByVal title As String, ByVal imdbmirror As String, Optional ByVal movieyear As String = "")
        Monitor.Enter(Me)
        Try
            Dim Got_Id As String = ""

                    'Pass title to IMDB Advanced Search.

            Got_Id = getimdbID_fromimdb_advanced(title, imdbmirror, movieyear)
            If Got_Id <> "" AndAlso Got_Id.ToLower <> "error" Then Return Got_Id

                    'Adjust title Swap "&" with "and", or "and" with "&"

            Dim title2 As String = title
            If title2.Contains("&") or title2.ToLower.Contains(" and ") Then
                If title2.Contains("&") Then
                    title2 = title2.Replace("&", "and")
                ElseIf title2.ToLower.Contains(" and ") Then
                    title2 = Regex.Replace(title2, "and", "&", RegexOptions.IgnoreCase)
                End If
                Got_Id = getimdbID_fromimdb_advanced(title2, imdbmirror, movieyear)
                If Got_Id <> "" AndAlso Got_Id.ToLower <> "error" Then Return Got_Id
            End If
            
                    'Original Title, change to Proper Case

            Dim title3 As String = StrConv(title, VbStrConv.ProperCase)
            If title3 <> title Then
                Got_Id = getimdbID_fromimdb_advanced(title3, imdbmirror, movieyear)
                If Got_Id <> "" AndAlso Got_Id.ToLower <> "error" Then Return Got_Id
            End If

                    'If none of above pass movie, use IMDB's Simple search

            Got_Id = getimdbID_fromimdb_simple(title, imdbmirror, movieyear)

            Return Got_Id

        Catch ex As Exception
            Return "Error"
        Finally
            Monitor.Exit(Me)
        End Try
        
    End Function

    Public Function getimdbID_fromimdb_advanced(ByVal title As String, ByVal imdbmirror As String, Optional ByVal movieyear As String = "")
        Monitor.Enter(Me)
        Try
            Dim searchyear As String = ""
            Dim popularreturn As String = ""
            Dim exactreturn As String = ""
            Dim M As Match
            Dim titlesearch As String = Utilities.searchurltitle(title)
            If movieyear <> "" Then
                Dim yr As Integer = movieyear.ToInt
                searchyear = "release_date=" & (yr-1.ToString) & "," & (yr+1.ToString) & "&"
            End If
            titlesearch = imdbmirror & "search/title?" & searchyear & "title=" & titlesearch & "&title_type=feature,tv_movie,tv_special"
            Dim urllinecount As Integer
            Dim GOT_IMDBID As String
            Dim allok As Boolean = False
            Dim websource(4000)
            For f = 1 To 10
                Try
                    Dim wrGETURL As WebRequest
                    wrGETURL = WebRequest.Create(titlesearch)
                    wrGETURL.Proxy = Utilities.MyProxy
                    Dim objStream As IO.Stream
                    objStream = wrGETURL.GetResponse.GetResponseStream()
                    Dim objReader As New IO.StreamReader(objStream)
                    Dim sLine As String = ""
                    urllinecount = 0
                    Do While Not sLine Is Nothing
                        urllinecount += 1
                        sLine = objReader.ReadLine
                        If Not sLine Is Nothing Then websource(urllinecount) = sLine
                    Loop
                    urllinecount -= 1
                    allok = True
                    Exit For
                Catch
                End Try
            Next
            Dim webPg As String = ""
            For I = 1 to urllinecount
                webPg += websource(I).ToString & vbcrlf
            Next
            GOT_IMDBID = ""
            Dim popular(1000) As String
            Dim atpopular As Boolean = False
            Dim popularcount As Integer = 0
            Dim exact(1000) As String
            Dim atexact As Boolean = False
            Dim exactcount As Integer = 0
            Dim yearcounter As Integer = 0
            Dim year(1000) As Integer
            Dim backup As String = ""
            
            For f = 1 To urllinecount
                If backup = "" And websource(f).IndexOf("href=""/title/tt") <> -1 Then
                    If websource(f).IndexOf("&#34;") = -1 Then
                        Dim first As Integer
                        Dim length As Integer
                        first = websource(f).LastIndexOf("href=""/title/tt") + 13
                        length = 9
                        If first > 12 Then
                            backup = websource(f).Substring(first, length)
                        End If
                    End If
                End If
                If movieyear <> "" Then
                    If websource(f).IndexOf("><img src=") <> -1 Then
                        If websource(f).IndexOf("&#34;") = -1 Then
                            If websource(f).IndexOf(movieyear) <> -1 Then
                                Dim first As Integer
                                Dim length As Integer
                                first = websource(f).LastIndexOf("href=""/title/tt") + 13
                                length = 9
                                GOT_IMDBID = websource(f).Substring(first, length)
                                Exit For
                            End If
                        End If
                    End If
                    If websource(f).IndexOf(movieyear) <> -1 Then
                        yearcounter += 1
                        year(yearcounter) = f
                    End If
                End If
            Next
            If movieyear <> Nothing AndAlso GOT_IMDBID = "" Then
                If yearcounter = 1 And GOT_IMDBID = "" Then
                    Dim onlyid As String = websource(year(yearcounter)).Substring(0, websource(year(yearcounter)).IndexOf(movieyear) + 6)
                    Dim first As Integer
                    Dim length As Integer
                    first = onlyid.LastIndexOf("href=""/title/tt") + 13
                    length = 9
                    If first > 12 Then
                        GOT_IMDBID = onlyid.Substring(first, length)
                    End If
                End If
                Dim populartotal As String = ""
                Dim exacttotal As String = ""
                If GOT_IMDBID = "" Then
                    For f = 1 To urllinecount
                        If websource(f).IndexOf("Popular Titles") <> -1 Or websource(f).IndexOf("Exact Matches") <> -1 Then

                            If websource(f).IndexOf("Popular Titles") <> -1 And websource(f).IndexOf("Exact Matches") <> -1 Then
                                populartotal = websource(f).Substring(0, websource(f).IndexOf("Exact Matches") - 13)
                                exacttotal = websource(f).Replace(populartotal, "")
                                Exit For
                            ElseIf websource(f).IndexOf("Popular Titles") <> -1 And websource(f).IndexOf("Exact Matches") = -1 Then
                                populartotal = websource(f)
                                exacttotal = ""
                                Exit For
                            ElseIf websource(f).IndexOf("Popular Titles") = -1 And websource(f).IndexOf("Exact Matches") <> -1 Then
                                populartotal = ""
                                exacttotal = websource(f)
                                Exit For
                            End If
                        End If
                    Next
                    Dim temps As String
                    If GOT_IMDBID = "" And exacttotal <> "" Then
                        If exacttotal.IndexOf(movieyear) <> -1 Then
                            If CharCount(exacttotal, movieyear) = 1 Then
                                temps = exacttotal.Substring(0, exacttotal.IndexOf(movieyear) + 6)
                                Dim calc As String
                                calc = temps.Substring(temps.LastIndexOf("href=""/title/tt"), temps.Length - temps.LastIndexOf("href=""/title/tt"))
                                Dim regMatch As New Regex("tt(\d){6,8}")
                                GOT_IMDBID = regMatch.Matches(calc).Item(0).Value
                            End If
                        End If
                        If GOT_IMDBID = "" And populartotal <> "" Then
                            If populartotal.IndexOf(movieyear) <> -1 Then
                                If CharCount(populartotal, movieyear) = 1 Then
                                    temps = populartotal.Substring(0, populartotal.IndexOf(movieyear) + 6)
                                    Dim calc As String
                                    calc = temps.Substring(temps.LastIndexOf("href=""/title/tt"), temps.Length - temps.LastIndexOf("href=""/title/tt"))
                                    Dim regMatch As New Regex("tt(\d){6,8}")
                                    GOT_IMDBID = regMatch.Matches(calc).Item(0).Value
                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf GOT_IMDBID = "" AndAlso movieyear = Nothing Then
                Dim exactmatch As Boolean = False
                Dim popularmatch As Boolean = False

                For f = 1 To urllinecount
                    If websource(f).IndexOf("Titles (Exact Matches)") <> -1 Then
                        exactmatch = True
                    End If
                    If websource(f).IndexOf("Popular Titles") <> -1 Then
                        popularmatch = True
                    End If
                Next
                If popularmatch = True Then
                    popularreturn = ""
                    For f = 1 To urllinecount
                        If websource(f).IndexOf("Popular Titles") <> -1 Then
                            Dim type As String
                            type = websource(f).Substring(websource(f).IndexOf("Popular Titles"), websource(f).Length - websource(f).IndexOf("Popular Titles"))
                            If type.IndexOf("Exact Matches") <> -1      Then type = type.Substring(0, type.IndexOf("Exact Matches"))
                            If type.IndexOf("Partial Matches") <> -1    Then type = type.Substring(0, type.IndexOf("Partial Matches"))
                            If type.IndexOf("Approx Matches") <> -1     Then type = type.Substring(0, type.IndexOf("Approx Matches"))

                            Do Until type.IndexOf("</td></tr>") = -1
                                Dim pyte As String = ""
                                If type.IndexOf("</td></tr>") <> -1 Then pyte = type.Substring(0, type.IndexOf("</td></tr>"))
                                If pyte <> "" Then
                                    If pyte.IndexOf("&#34;") = -1 And pyte.IndexOf("<small>(TV") = -1 And pyte.IndexOf("(VG)") = -1 Then
                                        type = pyte
                                        Exit Do
                                    Else
                                        type = type.Replace(pyte, "")
                                    End If
                                Else
                                    If type.Length = 0 Then Exit Do
                                    type = type.Substring(5, type.Length - 5)
                                End If
                            Loop
                            M = Regex.Match(type, "(tt\d{7})")
                            If M.Success Then popularreturn = M.Value
                        End If
                    Next
                End If
                If exactmatch = True Then
                    exactreturn = ""
                    For f = 1 To urllinecount
                        If websource(f).IndexOf("Titles (Exact Matches)") <> -1 Then
                            Dim type As String
                            type = websource(f).Substring(websource(f).IndexOf("Titles (Exact Matches)"), websource(f).Length - websource(f).IndexOf("Titles (Exact Matches)"))
                            Do Until type.IndexOf("</td></tr>") = -1
                                Dim pyte As String = ""
                                If type.IndexOf("</td></tr>") <> -1         Then pyte = type.Substring(0, type.IndexOf("</td></tr>"))
                                If type.IndexOf("Partial Matches") <> -1    Then type = type.Substring(0, type.IndexOf("Partial Matches"))
                                If type.IndexOf("Approx Matches") <> -1     Then type = type.Substring(0, type.IndexOf("Approx Matches"))
                                If pyte <> "" Then
                                    If pyte.IndexOf("&#34;") = -1 And pyte.IndexOf("<small>(TV") = -1 And pyte.IndexOf("(VG)") = -1 Then
                                        type = pyte
                                        Exit Do
                                    Else
                                        type = type.Replace(pyte, "")
                                    End If
                                Else
                                    If type.Length = 0 Then Exit Do
                                    type = type.Substring(5, type.Length - 5)
                                End If
                            Loop
                            M = Regex.Match(type, "(tt\d{7})")
                            If M.Success = True Then
                                exactreturn = M.Value
                            End If
                        End If
                    Next
                End If
                If popularreturn <> "" Then GOT_IMDBID = popularreturn
                If exactreturn <> "" And popularreturn = "" Then GOT_IMDBID = exactreturn
                If exactreturn = "" And popularreturn = "" And backup <> "" Then GOT_IMDBID = backup
            End If

            If GOT_IMDBID = "" Then
                Dim matc As Match
                Dim NoResults As Match
                For f = 1 To urllinecount
                    NoResults = Regex.Match(websource(f), "No results.")
                    If NoResults.Success Then Return GOT_IMDBID
                Next
                For f = 1 To urllinecount
                    matc = Regex.Match(websource(f), "tt\d{7}")
                    If matc.Success Then
                        GOT_IMDBID = matc.Value
                        Exit For
                    End If
                Next
            End If
            Return GOT_IMDBID
        Catch
            Return "Error"
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Public Function getimdbID_fromimdb_simple(ByVal title As String, ByVal imdbmirror As String, Optional ByVal movieyear As String = "")
        Monitor.Enter(Me)
        Dim GOT_IMDBID As String = ""
        Try
            Dim titlesearch As String = Utilities.searchurltitle(title)
            titlesearch = imdbmirror & "find?q=" & titlesearch & "&s=tt&exact=true&ref_=fn_tt_ex"
            Dim urllinecount As Integer
            Dim allok As Boolean = False
            Dim websource(4000)
            For f = 1 To 10
                Try
                    Dim wrGETURL As WebRequest
                    wrGETURL = WebRequest.Create(titlesearch)
                    wrGETURL.Proxy = Utilities.MyProxy
                    Dim objStream As IO.Stream
                    objStream = wrGETURL.GetResponse.GetResponseStream()
                    Dim objReader As New IO.StreamReader(objStream)
                    Dim sLine As String = ""
                    urllinecount = 0
                    Do While Not sLine Is Nothing
                        urllinecount += 1
                        sLine = objReader.ReadLine
                        If Not sLine Is Nothing Then
                            websource(urllinecount) = sLine
                        End If
                    Loop
                    urllinecount -= 1
                    allok = True
                    Exit For
                Catch
                End Try
            Next

            GOT_IMDBID = ""
            Dim resultlist As String = ""
            Dim popular(1000) As String
            Dim atpopular As Boolean = False
            Dim popularcount As Integer = 0
            Dim exact() As String
            Dim atexact As Boolean = False
            Dim exactcount As Integer = 0
            Dim yearcounter As Integer = 0
            Dim year(1000) As Integer
            Dim backup As String = ""
            
            For f = 1 To urllinecount
                If websource(f).IndexOf("<table class=""findList"">") <> -1 Then
                    If websource(f+1).IndexOf("<tr class=""findResult") <> -1 Then
                        resultlist = websource(f+1).ToString
                        Exit For
                    End If
                End If
            Next
            If resultlist = "" Then
                Return "Error"
            Else
                exact = Regex.Split(resultlist, "<tr class=")
                exactcount = exact.GetUpperBound(0)
            End If
            If exactcount <> 0 Then
                For f = 1 to exactcount
                    If exact(f).Contains(title) AndAlso exact(f).Contains(movieyear) AndAlso Not exact(f).ToLower.Contains("tv episode") Then
                            Dim first As Integer
                            Dim length As Integer
                            first = exact(f).LastIndexOf("href=""/title/tt") + 13
                            length = 9
                            backup = exact(f).Substring(first, length)
                        Exit For
                    End If
                Next
            End If
            GOT_IMDBID = backup

        Catch
            Return "Error"
        Finally
            Monitor.Exit(Me)
        End Try
        Return GOT_IMDBID 
    End Function
    
    Public Function CharCount(ByVal OrigString As String, ByVal Chars As String, Optional ByVal CaseSensitive As Boolean = False) As Long

        Dim lLen As Long
        Dim lCharLen As Long
        Dim lAns As Long
        Dim sInput As String
        Dim sChar As String
        Dim lCtr As Long
        Dim lEndOfLoop As Long
        Dim bytCompareType As Byte

        sInput = OrigString
        If sInput = "" Then 
            Return 0
        End If
        
        lLen = Len(sInput)
        lCharLen = Len(Chars)
        lEndOfLoop = (lLen - lCharLen) + 1
        bytCompareType = IIf(CaseSensitive, vbBinaryCompare, _
           vbTextCompare)

        For lCtr = 1 To lEndOfLoop
            sChar = Mid(sInput, lCtr, lCharLen)
            If StrComp(sChar, Chars, bytCompareType) = 0 Then _
                lAns = lAns + 1
        Next

        CharCount = lAns

    End Function
    
    ReadOnly Property Genres As String
        Get
            Dim s As String=""
            Dim D = 0
            Dim W = 0
            D = Html.IndexOf("<h4 class=""inline"">Genres:</h4>")
            If Not D <= 0 Then
                W = Html.IndexOf("</div>", D)
                Dim rGenres As MatchCollection = Regex.Matches(Html.Substring(D, W - D), MovieRegExs.REGEX_HREF_PATTERN, RegexOptions.Singleline)
                Dim lst = From M As Match In rGenres Select N = M.Groups("name").ToString Where Not N.Contains("more")
                s.AppendList(lst, " / ")
            End If
            If TitleAndYear.ToLower.Contains("(tv movie ") Then s.AppendValue("TV Movie", " / ")
            Return s
        End Get
    End Property

    ReadOnly Property Directors As String
        Get
            Dim s As String=""
            Dim D = Html.IndexOf("itemprop=""director""")
            Dim W = If(D > 0, Html.IndexOf("</div>", D), 0)
            If Not D <= 0 And Not W <= 0 Then
                Dim rDir As MatchCollection = Regex.Matches(Html.Substring(D, W - D), MovieRegExs.REGEX_HREF_PATTERN)
                Dim lst = From M As Match In rDir Where Not M.Groups("name").ToString.Contains("more") _
                             Select Net.WebUtility.HtmlDecode(M.Groups("name").ToString)
                s.AppendList(lst, " / ")
            End If
            Return s
        End Get
    End Property
    
    'NB Credits = Writer
    ReadOnly Property Credits As String
        Get
            Return GetNames(MovieRegExs.REGEX_CREDITS)
        End Get
    End Property
    
    ReadOnly Property ReleaseDate As String
        Get
            Dim s = ""
            Dim RelDate As Date
            Dim sRelDate As String = Regex.Match(Regex.Match(Html, MovieRegExs.REGEX_RELEASE_DATE, RegexOptions.Singleline).Groups("date").ToString.Replace("&nbsp;"," "), "\d+\s\w+\s\d\d\d\d\s").ToString
            If Not sRelDate = "" Then
                If Date.TryParse(sRelDate, RelDate) Then
                    s = RelDate.ToString("yyyy-MM-dd")
                End If
            End If
            Return s
        End Get
    End Property

    ReadOnly Property Stars As String
        Get
            Dim s       As String = ""
            Dim context As String = Regex.Match(Html,MovieRegExs.REGEX_STARS, RegexOptions.Singleline).ToString

            Dim lst = From M As Match In Regex.Matches(context, MovieRegExs.REGEX_HREF_PATTERN, RegexOptions.Singleline) 
                        Where Not (M.Groups("name").ToString.ToLower.Contains("see full cast and crew") Or M.Groups("name").ToString.ToLower.Contains("see full cast & crew"))
                        Select Net.WebUtility.HtmlDecode(M.Groups("name").ToString)

            s.AppendList(lst)

            Return s
        End Get
    End Property
    
    ReadOnly Property Title As String
        Get
            Dim s As String = ""
            If Pref.Original_Title Then
                s=Original_Title
            End If
            s = s.Replace("""", "")
            If s="" Then  
                s=Regex.Match(TitleAndYear,MovieRegExs.REGEX_TITLE, RegexOptions.Singleline).Groups(1).Value
                s = s.Replace("&amp;", "&")
                s = s.Replace( "&quot;", """")
            End If
            Return s
        End Get
    End Property
    
    ReadOnly Property Year As String
        Get
            Return Regex.Match(TitleAndYear,MovieRegExs.REGEX_YEAR, RegexOptions.Singleline).Groups(1).Value
        End Get
    End Property
    
    'Studio = Production
    ReadOnly Property Studio As String
        Get
            Return GetNames(MovieRegExs.REGEX_STUDIO,Pref.MovieScraper_MaxStudios)
        End Get
    End Property
    
    ReadOnly Property Outline As String
        Get
            Dim s As String = Regex.Match(Html,MovieRegExs.REGEX_OUTLINE, RegexOptions.Singleline).Groups(1).Value.Trim
            If s.Contains("...     ") Then
                Dim l = s.IndexOf("...     ")
                s = s.Substring(0, (l+8))
                s = s.TrimEnd
            End If
            Dim p As String = Regex.Match(s, MovieRegExs.REGEX_HREF_PATTERN).ToString
            If p <> "" Then
                s = Regex.Replace(s, "</?a.*?>", String.Empty)
            End If
            s = StripTags(s)
            If s.ToLower.Contains("add a plot") Then Return ""
            s = s.Replace("&quot;", """")
            Return Utilities.cleanSpecChars(encodespecialchrs(s.Trim()))
        End Get
    End Property
    
    ReadOnly Property Top250 As String
        Get
            Try
                Dim s As String = Regex.Match(Html, MovieRegExs.REGEX_TOP_250, RegexOptions.Singleline).Groups(1).Value.Trim
                If s = "" Then s = "0"
                Return s
            Catch
                Return "0"
            End Try
        End Get
    End Property

    ReadOnly Property Votes As String
        Get
            Try
                Dim s As String = Regex.Match(Html, MovieRegExs.REGEX_VOTES, RegexOptions.Singleline).Groups(1).Value.Trim
                s = encodespecialchrs(s)
                s = s.Replace(".", "")
                Return s
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    
    ReadOnly Property Rating As String
        Get
            Try
                Dim s As String = Regex.Match(Html, MovieRegExs.REGEX_RATING, RegexOptions.Singleline).Groups(1).Value.Trim
                Return encodespecialchrs(s)
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    
    ReadOnly Property TagLine As String
        Get
            Try
                Dim s As String = Regex.Match(Html, MovieRegExs.REGEX_TAGLINE, RegexOptions.Singleline).Groups(1).Value.Trim
                If s.Contains("</div>") Then
                    s = s.Substring(0, s.IndexOf("</div>"))
                End If
                s = s.Replace("&quot;", """")
                Return Utilities.cleanSpecChars(encodespecialchrs(s))
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    
    ReadOnly Property Duration As String
        Get
            Try
                Dim t As String = Regex.Match(Html, MovieRegExs.REGEX_RUNTIME, RegexOptions.Singleline).Groups(1).Value
                Dim i As Integer=0

                For Each m As Match In Regex.Matches(t, MovieRegExs.REGEX_DURATION, RegexOptions.Singleline) 
                    Dim s = Regex.Match(m.Value, MovieRegExs.REGEX_DURATION, RegexOptions.Singleline).Groups(1).Value.Trim
                    Dim p As Integer = s.ToInt
                    If p > i Then i = p
                    If Pref.MovImdbFirstRunTime Then Exit For
                Next
                If i = 0 Then Return ""
                Return i.ToString & " min"
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Function RuntimefromTechSpecs(ByVal imdbid As String) As String
        Dim tmpInfo As String = ""
        Try
            Dim tempstring As String= Pref.imdbmirror & "title/" & imdbid & "/technical?ref_=tt_dt_spec"
            Dim TmpHtml As String = loadwebpage(Pref.proxysettings, tempstring, True)
            Dim t As String = Regex.Match(TmpHtml, MovieRegExs.REGEX_RUNTIMETECH, RegexOptions.Singleline).Groups(1).Value
            Dim i As Integer=0
            For Each m As Match In Regex.Matches(t, MovieRegExs.REGEX_DURATIONTECH, RegexOptions.Singleline)
                Dim s = Regex.Match(m.Value, MovieRegExs.REGEX_DURATIONTECH, RegexOptions.Singleline).Groups(1).Value.Trim
                Dim p As Integer = s.ToInt
                If p > i Then i = p
                tmpInfo = i.ToString
                If Pref.MovImdbFirstRunTime Then Exit For
            Next
            If i = 0 Then
                For each m As Match In Regex.Matches(t, MovieRegExs.REGEX_DURATIONTECH2, RegexOptions.Singleline)
                    Dim s = Regex.Match(m.Value, MovieRegExs.REGEX_DURATIONTECH2, RegexOptions.Singleline).Groups(1).Value.Trim
                    Dim p As Integer = s.ToInt
                    If p > i Then i = p
                    tmpInfo = i.ToString
                    If Pref.MovImdbFirstRunTime Then Exit For
                Next
            End If
        Catch 
        End Try
        Return tmpInfo
    End Function
    
    ReadOnly Property Countrys As String
        Get
            Try
                Dim CountryInfo As String = ""
                Dim t As String = Regex.Match(Html, MovieRegExs.REGEX_COUNTRYS, RegexOptions.Singleline).Groups(1).Value
                For Each m As Match In Regex.Matches(t, MovieRegExs.REGEX_COUNTRY, RegexOptions.Singleline) 
                    Dim s = Regex.Match(m.Value, MovieRegExs.REGEX_COUNTRY, RegexOptions.Singleline).Groups(1).Value.Trim
                    CountryInfo.AppendValue(Utilities.cleanSpecChars(encodespecialchrs(s)))
                Next
                Return CountryInfo
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property
    
    Public Function metacritic(ByVal imdbid As String) As String
        Try
            Dim url As String = String.Format("{0}?i={1}&plot=short&r=xml", Pref.Omdbapiurl, imdbid)
            Dim getresult As String = loadwebpage(Pref.proxysettings, url, True)
            If getresult = "error" Then Return ""
            Dim adoc As New XmlDocument
            adoc.LoadXml(getresult)
            If adoc("root").HasAttribute("response") AndAlso adoc("root").Attributes("response").Value = "False" Then Return ""
            If adoc("root").HasAttribute("Response") AndAlso adoc("root").Attributes("Response").Value = "False" Then Return ""
            For each thisresult As XmlNode In adoc("root")
                If Not IsNothing(thisresult.Attributes.ItemOf("metascore")) Then
                    Dim TmpValue As String = thisresult.Attributes("metascore").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then Return TmpValue
                End If
            Next
        Catch
        End Try
        Return ""
    End Function

    ''' <summary>
    ''' Get Aspect Ratio from IMDb.  Code in place to get AR from Technical page, but as so many returned, no guarantee which is correct.
    ''' Reverted 19/8/2016 to get AR from IMDB main page.
    ''' </summary>
    ''' <param name="imdbid"></param>
    ''' <returns>AR in a String</returns>
    ReadOnly Property ARImdb (ByVal imdbid As String) As String
        Get
            Dim s As String = ""
            Try
                s = Regex.Match(Html, MovieRegExs.REGEX_ASPECTRATIO, RegexOptions.Singleline).Groups(1).Value.Trim
                If s = "" Then Return ""
                If s.Contains("</div>") Then s = s.Substring(0, s.IndexOf("</div>"))
                s = s.Substring(0, s.IndexOf(":")).Trim
                Return Utilities.cleanSpecChars(encodespecialchrs(s))
            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    ReadOnly Property TitleAndYear As String
        Get
            Return Regex.Match(Html,MovieRegExs.REGEX_TITLE_AND_YEAR, RegexOptions.Singleline).ToString.Trim
        End Get
    End Property
   
    ReadOnly Property Original_Title As String
        Get
            Return Regex.Match(Html,MovieRegExs.REGEX_ORIGINAL_TITLE, RegexOptions.Singleline).Groups(1).Value.Trim
        End Get
    End Property
   
    ReadOnly Property GetFromImdb As Boolean
        Get
            Return Pref.XbmcTmdbCertFromImdb OrElse Pref.XbmcTmdbStarsFromImdb OrElse Pref.XbmcTmdbTop250FromImdb OrElse 
                Pref.XbmcTmdbVotesFromImdb OrElse Pref.XbmcTmdbMissingFromImdb OrElse Pref.XbmcTmdbAkasFromImdb OrElse
                Pref.XbmcTmdbAspectFromImdb OrElse Pref.XbmcTmdbMetascoreFromImdb
        End Get
    End Property

    Function AKAS(ByVal imdbid As String) As String
        Dim totalinfo As String = ""
        Try
            'releaseinfo#akas
            Dim tempstring As String= Pref.imdbmirror & "title/" & imdbid & "/releaseinfo#akas"
            Dim webpage As New List(Of String)
            webpage.Clear()
            webpage = loadwebpage(Pref.proxysettings, tempstring, False)
            For f = 0 To webpage.Count - 1
                If webpage(f).IndexOf("<h4 class=""li_group"">Also Known As (AKA)") <> -1 Then    '"<h5><a name=""akas"">Also Known As"
                    Dim loc As Integer = f
                    Dim ignore As Boolean = False
                    Dim original As Boolean = False
                    For g = loc To loc + 500
                        If webpage(g).IndexOf("</table>") <> -1 Then Exit For
                        Dim skip As Boolean = Not ignore
                        If webpage(g).IndexOf("<td>") <> -1 Then ignore = Not ignore
                        If skip = True Then
                            If webpage(g).IndexOf("(original title)") <> -1 Then original = True
                        End If

                        If webpage(g).IndexOf("<td>") <> -1 And skip = False Then
                            If webpage(g - 1).IndexOf("Greece") = -1 And webpage(g - 1).IndexOf("Russia") = -1 Then
                                tempstring = webpage(g)
                                tempstring = LTrim(tempstring)
                                tempstring = tempstring.Replace("<td>", "")
                                tempstring = tempstring.Replace("</td>", "")
                                tempstring = Utilities.cleanSpecChars(tempstring)
                                tempstring = encodespecialchrs(tempstring)
                                Dim TitleTag As String = "<originaltitle>" & tempstring & "</originaltitle>"
                                If original = False Then
                                    totalinfo = totalinfo & "<alternativetitle>" & tempstring & "</alternativetitle>" & vbCrLf
                                Else
                                    If totalinfo.IndexOf(TitleTag) = -1 Then
                                        totalinfo = totalinfo & "<originaltitle>" & tempstring & "</originaltitle>" & vbCrLf
                                    Else
                                        totalinfo = totalinfo & "<alternativetitle>" & tempstring & "</alternativetitle>" & vbCrLf
                                    End If
                                    original = False
                                End If
                            Else
                                g = g + 1
                            End If
                            ignore = False
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
        End Try
        Return totalinfo
    End Function

    Function GetNames(RegExPattern As String, Optional ByVal Max As Integer=-1) As String
        Dim s As String=""
        Dim context = Regex.Match(Html,RegExPattern, RegexOptions.Singleline).ToString
        If context = "" Then Return ""
        Dim name=""
        If Max=-1 Then Max=999 
        Dim i As Integer=0
        For Each m As Match In Regex.Matches(context, MovieRegExs.REGEX_NAME, RegexOptions.Singleline)
            name=Net.WebUtility.HtmlDecode(m.Groups("name").Value)
            s.AppendValue(name, " / ")
            i += 1
            If i=Max Then Exit For
        Next
        Return s
    End Function
    
    Public Function getimdbbody(Optional ByVal title As String = "", Optional ByVal year As String = "", Optional ByVal imdbid As String = "", Optional ByVal imdbcounter As Integer = 0)
        Monitor.Enter(Me)

        Dim totalinfo As String = ""
        Try
            Dim tempstring As String
            Dim actors(10000, 3)
            Dim actorcount As Integer = 0
            Dim tempint As Integer
            Dim mpaacount As Integer = -1
            Dim webpage As New List(Of String)
            Dim mpaaresults(33, 1) As String
            Dim OriginalTitle As Boolean = False
            Dim FoundTitle As Boolean = False
            mpaaresults(0, 0) = "MPAA"
            mpaaresults(1, 0) = "UK"
            mpaaresults(2, 0) = "USA"
            mpaaresults(3, 0) = "Ireland"
            mpaaresults(4, 0) = "Australia"
            mpaaresults(5, 0) = "New Zealand"
            mpaaresults(6, 0) = "Norway"
            mpaaresults(7, 0) = "Singapore"
            mpaaresults(8, 0) = "South Korea"
            mpaaresults(9, 0) = "Philippines"
            mpaaresults(10, 0) = "Brazil"
            mpaaresults(11, 0) = "Netherlands"
            mpaaresults(12, 0) = "Malaysia"
            mpaaresults(13, 0) = "Argentina"
            mpaaresults(14, 0) = "Iceland"
            mpaaresults(15, 0) = "Quebec"
            mpaaresults(16, 0) = "British Columbia"
            mpaaresults(17, 0) = "Nova Scotia"
            mpaaresults(18, 0) = "Peru"
            mpaaresults(19, 0) = "Sweden"
            mpaaresults(20, 0) = "Portugal"
            mpaaresults(21, 0) = "South Africa"
            mpaaresults(22, 0) = "Denmark"
            mpaaresults(23, 0) = "Hong Kong"
            mpaaresults(24, 0) = "Finland"
            mpaaresults(25, 0) = "India"
            mpaaresults(26, 0) = "Mexico"
            mpaaresults(27, 0) = "France"
            mpaaresults(28, 0) = "Italy"
            mpaaresults(29, 0) = "canton of Vaud"
            mpaaresults(30, 0) = "canton of Geneva"
            mpaaresults(31, 0) = "Germany"
            mpaaresults(32, 0) = "Greece"
            mpaaresults(33, 0) = "Austria"
            Dim movienfoarray As String = String.Empty
            Dim genre(20)
            Dim thumbs(500)
            Dim allok As Boolean = False
            If imdbid <> Nothing Then
                If imdbid.Length = 9 And imdbid.IndexOf("tt") <> -1 Then
                    allok = True
                End If
            End If
            totalinfo = "<movie>" & vbCrLf
            If allok = False Then
                If imdbcounter < 450 Then
                    'imdbid = getimdbID_fromOmdbapi(title, year)
                    imdbid = getimdbID(title, year)
                Else
                    imdbid = getimdbID_fromimdb(title, Pref.imdbmirror, year)
                End If
                If imdbid <> "" And imdbid.IndexOf("tt") = 0 And imdbid.Length = 9 Then
                    allok = True
                End If
            End If
            If allok = False Then Return "MIC"

            If allok = True Then
                tempstring = Pref.imdbmirror & "title/" & imdbid
                webpage.Clear()
                webpage = loadwebpage(Pref.proxysettings, tempstring, False)

                Dim webPg As String = String.Join( "" , webpage.ToArray() )
                Html = webPg
                Html2 = String.Join(vbcrlf, webpage.ToArray())

                totalinfo.AppendTag( "genre"     , Genres      )
                totalinfo.AppendTag( "director"  , Directors   )
                totalinfo.AppendTag( "credits"   , Credits     )
                totalinfo.AppendTag( "premiered" , ReleaseDate )
                totalinfo.AppendTag( "stars"     , Stars       )
                totalinfo.AppendTag( "title"     , Me.Title    )
                totalinfo.AppendTag( "year"      , If(Not Pref.MovieChangeMovie, If(year = "", Me.Year, year), Me.Year))
                totalinfo.AppendTag( "studio"    , Studio      )
                totalinfo.AppendTag( "outline"   , Outline     )
                totalinfo.AppendTag( "top250"    , Top250      )
                totalinfo.AppendTag( "votes"     , Votes       )
                totalinfo.AppendTag( "rating"    , Rating      )
                totalinfo.AppendTag( "tagline"   , TagLine     )
                Dim DurationStr As String = Duration
                If DurationStr = "" Then DurationStr = RuntimefromTechSpecs(imdbid)
                totalinfo.AppendTag( "runtime"   , DurationStr )
                totalinfo.AppendTag( "id"        , imdbid      )
                totalinfo.AppendTag( "country"   , Countrys    )
                totalinfo.AppendTag( "metacritic", metacritic(imdbid))
                If Pref.MovImdbAspectRatio Then totalinfo.AppendTag( "aspect"    , ARImdb(imdbid))
                totalinfo &= getomdbTomato(imdbid)

                For f = 0 To 33
                    If mpaaresults(f, 1) <> Nothing Then
                        Try
                            mpaaresults(f, 0) = encodespecialchrs(mpaaresults(f, 0))
                            mpaaresults(f, 1) = encodespecialchrs(mpaaresults(f, 1))
                            totalinfo = totalinfo & "<cert>" & mpaaresults(f, 0) & "|" & mpaaresults(f, 1) & "</cert>" & vbCrLf
                        Catch
                            totalinfo = totalinfo & "<cert>scraper error|scraper error</cert>"
                        End Try
                    End If
                Next

                Try
                    tempstring = Pref.imdbmirror & "title/" & imdbid & "/plotsummary"
                    Dim plots(20) As String
                    webpage.Clear()
                    webpage = loadwebpage(Pref.proxysettings, tempstring, False)
                    tempint = 0
                    Dim doo As Boolean = False
                    For Each line In webpage
                        If doo = True Then
                            plots(tempint) = line
                            doo = False
                        End If
                        If line.IndexOf("<p class=""plotSummary"">") <> -1 Then
                            tempint = tempint + 1
                            doo = True
                        End If
                    Next
                    Dim sizes(tempint) As Integer
                    Dim biggest As Integer = 1
                    For f = 1 To tempint
                        If plots(f).Length > plots(biggest).Length Then
                            biggest = f
                        End If
                    Next
                    If Pref.ImdbPrimaryPlot Then biggest = 1    'If selected only use Primary Plot.
                    If plots(biggest) <> Nothing Then
                        movienfoarray = plots(biggest).StripTagsLeaveContent
                        movienfoarray = Regex.Replace(movienfoarray, "<.*?>", "").Trim
                        movienfoarray = Utilities.cleanSpecChars(movienfoarray)
                        movienfoarray = encodespecialchrs(movienfoarray)
                        totalinfo &= "<plot>" & movienfoarray & "</plot>"
                    End If
                Catch
                    totalinfo = totalinfo & "<plot>scraper error</plot>"
                End Try
                
                'certs & mpaa
                Try
                    tempstring = Pref.imdbmirror & "title/" & imdbid & "/parentalguide#certification"
                    webpage.Clear()
                    webpage = loadwebpage(Pref.proxysettings, tempstring, False)
                    'Dim webpage2 As String = String.Join(vbcrlf, webpage.ToArray())
                    For f = 0 To webpage.Count - 1
                        'mpaa
                        If webpage(f).IndexOf(" id="& MovieRegExs.quote & "mpaa-rating" & MovieRegExs.quote &">") <> -1 Then
                            tempstring = webpage(f + 2).StripTagsLeaveContent
                            If tempstring.IndexOf("<") = -1 Then
                                For g = 0 To 33
                                    If mpaaresults(g, 0) = "MPAA" Then
                                        mpaaresults(g, 1) = tempstring.Trim
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        'cert
                        If f > 1 Then
                            For g = 0 To 33
                                Dim mpaares As String = mpaaresults(g, 0)
                                If mpaares = "USA" Then mpaares = "United States"
                                If mpaares = "UK" Then mpaares = "United Kingdom"
                                tempstring = """>" & mpaares & ":"
                                If webpage(f).IndexOf("certificates=") <> -1 And webpage(f).IndexOf(tempstring) <> -1 Then
                                    tempstring = webpage(f).Substring(webpage(f).IndexOf(tempstring), webpage(f).Length - webpage(f).IndexOf(tempstring))
                                    tempstring = tempstring.Substring(tempstring.IndexOf(">") + 1, tempstring.IndexOf("</a>") - tempstring.IndexOf(">") - 1)
                                    mpaaresults(g, 1) = tempstring
                                    Try
                                        'line below determines if cert is full or short as e.g. UK:15 becomes 15
                                        If Not Pref.scrapefullcert Then
                                            mpaaresults(g, 1) = mpaaresults(g, 1).Substring(mpaaresults(g, 1).IndexOf(":") + 1, mpaaresults(g, 1).Length - mpaaresults(g, 1).IndexOf(":") - 1)
                                        End If
                                        mpaaresults(g, 1) = encodespecialchrs(mpaaresults(g, 1))
                                    Catch
                                        mpaaresults(g, 1) = "error"
                                    End Try
                                End If
                            Next
                        End If
                    Next
                Catch
                End Try

                Try
                    'releaseinfo#akas
                    totalinfo = totalinfo & AKAS(imdbid)
                Catch ex As Exception

                End Try
            End If
            For f = 0 To 33
                If mpaaresults(f, 1) <> Nothing Then
                    Try
                        mpaaresults(f, 0) = Utilities.cleanSpecChars(mpaaresults(f, 0))
                        mpaaresults(f, 1) = Utilities.cleanSpecChars(mpaaresults(f, 1))
                        mpaaresults(f, 0) = encodespecialchrs(mpaaresults(f, 0))
                        mpaaresults(f, 1) = encodespecialchrs(mpaaresults(f, 1))
                        totalinfo = totalinfo & "<cert>" & mpaaresults(f, 0) & "|" & mpaaresults(f, 1) & "</cert>" & vbCrLf
                    Catch
                        totalinfo = totalinfo & "<cert>scraper error|scraper error</cert>"
                    End Try
                End If
            Next
            totalinfo = totalinfo & "</movie>" & vbCrLf
            Return totalinfo
        Catch ex As Exception
            totalinfo = totalinfo & "</movie>" & vbCrLf
            Return totalinfo
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Public Function gettmdbbody(Optional ByVal title As String = "", Optional ByVal year As String = "", Optional ByVal tmdbid As String = "", Optional ByVal imdbcounter As Integer = 0)
        Monitor.Enter(Me)
        Dim IMDbId As String = String.Empty
        Dim totalinfo As String = ""
        Dim Thetitle As String = ""
        Dim ParametersForScraper(10) As String
        Dim FinalScrapResult As String
        Dim Scraper As String = "metadata.themoviedb.org"
        Try
            If String.IsNullOrEmpty(tmdbid) Then
                ' 1st stage
                ParametersForScraper(0) = title
                ParametersForScraper(1) = year
                FinalScrapResult = DoScrape(Scraper, "CreateSearchUrl", ParametersForScraper, False, False)
                FinalScrapResult = FinalScrapResult.Replace("<url>", "")
                FinalScrapResult = FinalScrapResult.Replace("</url>", "")
                FinalScrapResult = FinalScrapResult.Replace(" ", "%20")
                ' 2st stage
                ParametersForScraper(0) = FinalScrapResult
                FinalScrapResult = DoScrape(Scraper, "GetSearchResults", ParametersForScraper, True)
                If FinalScrapResult.ToLower = "error" or FinalScrapResult.ToLower = "<results></results>" Then Return "error"
                Dim m_xmld As XmlDocument
                Dim m_nodelist As XmlNodeList
                Dim m_node As XmlNode
                m_xmld = New XmlDocument()
                m_xmld.LoadXml(FinalScrapResult)
                m_nodelist = m_xmld.SelectNodes("/results/entity")
                For Each m_node In m_nodelist
                    TheTitle = m_node.ChildNodes.Item(0).InnerText
                    Dim id = m_node.ChildNodes.Item(1).InnerText
                    Dim Theyear = m_node.ChildNodes.Item(2).InnerText
                    Dim url = m_node.ChildNodes.Item(3).InnerText
                    ParametersForScraper(0) = url
                    ParametersForScraper(1) = id
                    Exit For
                Next
            Else
                If tmdbid.Substring(0, 2) <> "tt" Then
                    ParametersForScraper(0) = String.Format("http://api.themoviedb.org/3/movie/{0}?api_key={2}&language={1}", tmdbid, TMDb.LanguageCodes(0), Utilities.TMDBAPI)
                    ParametersForScraper(1) = tmdbid
                ElseIf tmdbid.Substring(0, 2) = "tt"
                    Dim url = String.Format("http://api.themoviedb.org/3/find/{0}?api_key={2}&language={1}&external_source=imdb_id", tmdbid, TMDb.LanguageCodes(0), Utilities.TMDBAPI)
                    Dim request = TryCast(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)
                    request.Method = "GET"
                    request.Proxy = Utilities.MyProxy
                    Dim responseContent As String = ""
                    Try
                    Using response = TryCast(request.GetResponse(), System.Net.HttpWebResponse)
                      Using reader = New IO.StreamReader(response.GetResponseStream())
                        responseContent = reader.ReadToEnd()
                      End Using
                    End Using
                    Catch
                    End Try
                    Dim RegExPattern = "id"":(.*?),"
                    Dim m As Match = Regex.Match(responseContent, RegExPattern)
                    If m.Success then
                        tmdbid = m.Groups(1).ToString
                        ParametersForScraper(0) = String.Format("http://api.themoviedb.org/3/movie/{0}?api_key={2}&language={1}", tmdbid, TMDb.LanguageCodes(0), Utilities.TMDBAPI)
                        ParametersForScraper(1) = tmdbid
                    Else
                        Return "error"
                    End If
                Else
                    Return "error"
                End If
            End If
            ' 3st stage
            FinalScrapResult = DoScrape(Scraper, "GetDetails", ParametersForScraper, True)
            If FinalScrapResult.ToLower = "error" Then
                Return "error"
            End If
            FinalScrapResult = ReplaceCharactersinXML(FinalScrapResult)
            If GetFromImdb Then
                Try
                    Dim m As Match = Regex.Match(FinalScrapResult, "(tt\d{7})")
                    If m.Success Then
                        IMDbId = m.Value 
                    End If
                Catch

                End Try
                If Not String.IsNullOrEmpty(IMDbId) Then
                    Dim something As String = getomdbTomato(IMDbId)   '  placeholder for addition of Rotten Tomatoe Rating
                    FinalScrapResult = FinalScrapResult.Replace("</details>", imdbdatafortmdbscraper(IMDbId) & vbCrLf & "</details>")
                End If
            End If
            FinalScrapResult = FinalScrapResult.Replace("details>","movie>")
            FinalScrapResult = FinalScrapResult.Replace("</genre>" & vbcrlf & "  <genre>", " / ")
            FinalScrapResult = FinalScrapResult.Replace("</studio>" & vbcrlf & "  <studio>", " / ")
            FinalScrapResult = FinalScrapResult.Replace("</country>" & vbcrlf & "  <country>", " / ")
            FinalScrapResult = FinalScrapResult.Replace("</credits>" & vbcrlf & "  <credits>", " / ")
            FinalScrapResult = FinalScrapResult.Replace("</director>" & vbcrlf & "  <director>", " / ")
            If FinalScrapResult.IndexOf("&") <> -1 Then FinalScrapResult = FinalScrapResult.Replace("&", "&amp;") 'Added for issue#352 as XML values are not checked for illegal Chars - HueyHQ
            Return FinalScrapResult
        Catch ex As Exception
            Return "error"
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Public Function imdbdatafortmdbscraper(ByVal IMDbId As String) As String
        Dim results As String = ""
        Monitor.Enter(Me)
        Try
            Dim IMDbUrl As String = Pref.imdbmirror & "title/" & IMDbId
            Dim webpage As New List(Of String)
            Dim mpaaresults(33, 1) As String
            mpaaresults(0, 0) = "MPAA"
            mpaaresults(1, 0) = "UK"
            mpaaresults(2, 0) = "USA"
            mpaaresults(3, 0) = "Ireland"
            mpaaresults(4, 0) = "Australia"
            mpaaresults(5, 0) = "New Zealand"
            mpaaresults(6, 0) = "Norway"
            mpaaresults(7, 0) = "Singapore"
            mpaaresults(8, 0) = "South Korea"
            mpaaresults(9, 0) = "Philippines"
            mpaaresults(10, 0) = "Brazil"
            mpaaresults(11, 0) = "Netherlands"
            mpaaresults(12, 0) = "Malaysia"
            mpaaresults(13, 0) = "Argentina"
            mpaaresults(14, 0) = "Iceland"
            mpaaresults(15, 0) = "Quebec"
            mpaaresults(16, 0) = "British Columbia"
            mpaaresults(17, 0) = "Nova Scotia"
            mpaaresults(18, 0) = "Peru"
            mpaaresults(19, 0) = "Sweden"
            mpaaresults(20, 0) = "Portugal"
            mpaaresults(21, 0) = "South Africa"
            mpaaresults(22, 0) = "Denmark"
            mpaaresults(23, 0) = "Hong Kong"
            mpaaresults(24, 0) = "Finland"
            mpaaresults(25, 0) = "India"
            mpaaresults(26, 0) = "Mexico"
            mpaaresults(27, 0) = "France"
            mpaaresults(28, 0) = "Italy"
            mpaaresults(29, 0) = "canton of Vaud"
            mpaaresults(30, 0) = "canton of Geneva"
            mpaaresults(31, 0) = "Germany"
            mpaaresults(32, 0) = "Greece"
            mpaaresults(33, 0) = "Austria"
            Dim movienfoarray As String = String.Empty
            webpage.Clear()
            webpage = loadwebpage(Pref.proxysettings, IMDbUrl, False)
            Dim webPg As String = String.Join( "" , webpage.ToArray() )
            Html = webPg
            Html2 = String.Join(vbcrlf, webpage.ToArray())

            If Pref.XbmcTmdbAkasFromImdb        Then results = results & AKAS(IMDbId)
            If Pref.XbmcTmdbStarsFromImdb       Then results.AppendTagText  ( "stars"       , Stars     )
            If Pref.XbmcTmdbMissingFromImdb     Then results.AppendTagText  ( "outline"     , Outline   )
            If Pref.XbmcTmdbTop250FromImdb      Then results.AppendTag      ( "top250"      , Top250    )
            If Pref.XbmcTmdbVotesFromImdb       Then results.AppendTag      ( "votes"       , Votes     )
            If Pref.XbmcTmdbGenreFromImdb       Then results.AppendTag      ( "imdbgenre"   , Genres    )
            If Pref.XbmcTmdbAspectFromImdb      Then results.AppendTag      ( "aspect"      , ARImdb(IMDbId))
            If Pref.XbmcTmdbMetascoreFromImdb   Then results.AppendTag      ( "metacritic"  , metacritic(IMDbId))
            If Pref.XbmcTmdbCertFromImdb Then
                For f = 0 To 33
                    If mpaaresults(f, 1) <> Nothing Then
                        Try
                            mpaaresults(f, 0) = encodespecialchrs(mpaaresults(f, 0))
                            mpaaresults(f, 1) = encodespecialchrs(mpaaresults(f, 1))
                            results = results & "<cert>" & mpaaresults(f, 0) & "|" & mpaaresults(f, 1) & "</cert>" & vbCrLf
                        Catch
                            results = results & "<cert>scraper error|scraper error</cert>"
                        End Try
                    End If
                Next
                Try
                    IMDbUrl = Pref.imdbmirror & "title/" & imdbid & "/parentalguide#certification"
                    webpage.Clear()
                    webpage = loadwebpage(Pref.proxysettings, IMDbUrl, False)
                    Dim tempstring As String = ""
                    For f = 0 To webpage.Count - 1
                        'mpaa
                        If webpage(f).IndexOf(" id="& MovieRegExs.quote & "mpaa-rating" & MovieRegExs.quote &">") <> -1 Then
                            tempstring = webpage(f + 2).StripTagsLeaveContent
                            If tempstring.IndexOf("<") = -1 Then
                                For g = 0 To 33
                                    If mpaaresults(g, 0) = "MPAA" Then
                                        mpaaresults(g, 1) = tempstring.Trim
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                        'cert
                        If f > 1 Then
                            For g = 0 To 33
                                Dim mpaares As String = mpaaresults(g, 0)
                                If mpaares = "USA" Then mpaares = "United States"
                                If mpaares = "UK" Then mpaares = "United Kingdom"
                                tempstring = """>" & mpaares & ":"
                                If webpage(f).IndexOf("certificates=") <> -1 And webpage(f).IndexOf(tempstring) <> -1 Then
                                    tempstring = webpage(f).Substring(webpage(f).IndexOf(tempstring), webpage(f).Length - webpage(f).IndexOf(tempstring))
                                    tempstring = tempstring.Substring(tempstring.IndexOf(">") + 1, tempstring.IndexOf("</a>") - tempstring.IndexOf(">") - 1)
                                    mpaaresults(g, 1) = tempstring
                                    Try
                                        'line below determines if cert is full or short as e.g. UK:15 becomes 15
                                        If Not Pref.scrapefullcert Then
                                            mpaaresults(g, 1) = mpaaresults(g, 1).Substring(mpaaresults(g, 1).IndexOf(":") + 1, mpaaresults(g, 1).Length - mpaaresults(g, 1).IndexOf(":") - 1)
                                        End If
                                        mpaaresults(g, 1) = encodespecialchrs(mpaaresults(g, 1))
                                    Catch
                                        mpaaresults(g, 1) = "error"
                                    End Try
                                End If
                            Next
                        End If
                    Next
                Catch
                End Try

                For f = 0 To 33
                    If mpaaresults(f, 1) <> Nothing Then
                        Try
                            mpaaresults(f, 0) = Utilities.cleanSpecChars(mpaaresults(f, 0))
                            mpaaresults(f, 1) = Utilities.cleanSpecChars(mpaaresults(f, 1))
                            mpaaresults(f, 0) = encodespecialchrs(mpaaresults(f, 0))
                            mpaaresults(f, 1) = encodespecialchrs(mpaaresults(f, 1))
                            results = results & "<cert>" & mpaaresults(f, 0) & "|" & mpaaresults(f, 1) & "</cert>" & vbCrLf
                        Catch
                            results = results & "<cert>scraper error|scraper error</cert>"
                        End Try
                    End If
                Next
            End If
            Return results
        Catch ex As Exception
            Return ""
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Public Function getomdbTomato(IMDBId As String)  As String
        Dim TotalInfo As String = ""
        Dim urlpath As String = String.Format("{0}?i={1}&tomatoes=true&r=xml", Pref.Omdbapiurl, IMDBId)
        Dim page As String = Utilities.DownloadTextFiles(urlpath)
        If page <> "" AndAlso page.Replace("""", "").Contains("root response=True") Then 
            Dim adoc As New XmlDocument
            adoc.LoadXml(page)
            For each thisresult As XmlElement In adoc("root")
                If Not IsNothing(thisresult.Attributes.ItemOf("tomatoUserReviews")) Then
                    Dim TmpValue As String = thisresult.Attributes("tomatoUserReviews").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then TotalInfo &= "<tomatoUserReviews>" & TmpValue & "</tomatoUserReviews>" & vbcrlf
                End If
                If Not IsNothing(thisresult.Attributes.ItemOf("tomatoUserRating")) Then
                    Dim TmpValue As String = thisresult.Attributes("tomatoUserRating").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then TotalInfo &= "<tomatoUserRating>" & TmpValue & "</tomatoUserRating>" & vbcrlf
                End If
                If Not IsNothing(thisresult.Attributes.ItemOf("tomatoUserMeter")) Then
                    Dim TmpValue As String = thisresult.Attributes("tomatoUserMeter").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then TotalInfo &= "<tomatoUserMeter>" & TmpValue & "</tomatoUserMeter>" & vbcrlf
                End If
                If Not IsNothing(thisresult.Attributes.ItemOf("tomatoRating")) Then
                    Dim TmpValue As String = thisresult.Attributes("tomatoRating").Value
                    If TmpValue <> "" AndAlso TmpValue <> "N/A" Then TotalInfo &= "<tomatoRating>" & TmpValue & "</tomatoRating>" & vbcrlf
                End If
            Next
        End If
        Return TotalInfo
    End Function

    Public Function GetImdbActorsList(ByVal imdbmirror As String, Optional ByVal imdbid As String = "", Optional ByVal maxactors As Integer = 9999) As List(Of str_MovieActors)
        If maxactors = 9999 Then maxactors= Pref.maxactors
        Dim tbl As String = GetActorsTable(  loadwebpage(Pref.proxysettings, imdbmirror & "title/" & imdbid & "/fullcredits#cast", True)  )
        Dim mc As MatchCollection = Regex.Matches(tbl, MovieRegExs.REGEX_TR, RegexOptions.Singleline)
        Dim results As New List(Of str_MovieActors)
        If maxactors = 0 Then Return results 
        For Each m In mc
            Dim actor As str_MovieActors = New str_MovieActors
            If actor.AssignFromImdbTr(m.ToString) Then results.Add(actor)
            If results.Count>=maxactors Then Exit For
        Next 
        Return results
    End Function

    Public Function GetActorsTable(Html As String) As String
            Return Regex.Match(Html,MovieRegExs.REGEX_ACTORS_TABLE, RegexOptions.Singleline).Groups(1).Value
    End Function
      
    Public Function gettrailerurl(ByVal imdbid As String, ByVal imdbmirror As String) As String
        Monitor.Enter(Me)
        Dim allok As Boolean = False
        Dim first As Integer
        Dim last As Integer

        Dim tempstring As String = ""
        Try
            Dim webpage As List(Of String)
            tempstring = imdbmirror & "title/" & imdbid & "/trailers"
            webpage = loadwebpage(Pref.proxysettings, tempstring, False)
            For f = 0 To webpage.Count - 1
                If webpage(f).Contains("<h2><a href=""/video/imdb") Then
                    Dim s As String = webpage(f)
                    Dim M As Match
                    M = Regex.Match(s, MovieRegExs.REGEX_IMDB_TRAILER, RegexOptions.Singleline)
                    If M.Success Then
                        Dim t As String = M.Groups(1).Value
                        tempstring = imdbmirror & "video/imdb/vi" & t &"/"
                        allok = True
                        Exit For
                    End If
                End If
            Next
            If allok = True Then
                allok = False
                webpage.Clear()
                webpage = loadwebpage(Pref.proxysettings, tempstring, False)
                Dim htmlpg As String = ""
                For Each wp In webpage
                    htmlpg += wp & vbcrlf
                Next
                For f = 0 To webpage.Count - 1
                    If webpage(f).IndexOf("www.totaleclips.com") <> -1 Then
                        first = webpage(f).IndexOf("www.totaleclips.com")
                        last = webpage(f).IndexOf(""");")
                        webpage(f) = webpage(f).Substring(first, last - first)
                        allok = True
                        webpage(f) = webpage(f).Replace("%3A", ":")
                        webpage(f) = webpage(f).Replace("%2F", "/")
                        webpage(f) = webpage(f).Replace("%3F", "?")
                        webpage(f) = webpage(f).Replace("%3D", "=")
                        webpage(f) = webpage(f).Replace("%26", "&")
                        tempstring = webpage(f)
                        tempstring = "http://" & tempstring
                        Dim totalinfo As String = "<trailer>" & tempstring & "</trailer>" & vbCrLf
                        Return tempstring
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
        Finally
            Monitor.Exit(Me)
        End Try
        Return ""
    End Function

    Private Function encodespecialchrs(ByVal text As String)
        If text.IndexOf("&") <> -1 Then text = text.Replace("&", "&amp;")
        If text.IndexOf("<") <> -1 Then text = text.Replace("<", "&lt;")
        If text.IndexOf(">") <> -1 Then text = text.Replace(">", "&gt;")
        If text.IndexOf(Chr(34)) <> -1 Then text = text.Replace(Chr(34), "&quot;")
        If text.IndexOf("'") <> -1 Then text = text.Replace("'", "&apos;")
        Return text
    End Function

    Public Function loadwebpage(ByVal proxy As List(of String), ByVal Url As String, ByVal IntoSingleString As Boolean, Optional TimeoutInSecs As Integer=-1)

        Dim webpage As New List(Of String)
        Monitor.Enter(Me)
        Try
            Dim wrGETURL As HttpWebRequest = HttpWebRequest.Create(Url)
            wrGETURL.Proxy = Utilities.MyProxy
            If TimeoutInSecs > -1 Then wrGETURL.Timeout = TimeoutInSecs * 1000
            wrGETURL.Headers.Add("Accept-Language", TMDb.LanguageCodes(0))
            wrGETURL.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2"
            Dim objStream As IO.Stream
            objStream = wrGETURL.GetResponse.GetResponseStream()
            Dim objReader As New IO.StreamReader(objStream)
            Dim sLine As String = ""
            If Not IntoSingleString Then
                Do While Not sLine Is Nothing
                    sLine = objReader.ReadLine
                    If Not sLine Is Nothing Then webpage.Add(sLine)
                Loop
            Else
                sLine = objReader.ReadToEnd
            End If
            objReader.Close()
            objStream.Close()
            
            If Not IntoSingleString Then
                Return webpage
            Else
                Return sLine
            End If

        Catch ex As WebException
            If Not IntoSingleString Then
                If webpage.Count > 0 Then
                    Return webpage
                Else
                    webpage.Add("error")
                    Return webpage
                End If
            Else
                Return "error"
            End If
        Finally
            Monitor.Exit(Me)
        End Try
    End Function
    
    Function GetGenres( ByVal webPage As String )
        Dim genres As New List(Of String)
        Dim genre As String
        For Each m As Match In Regex.Matches( webPage, Pref.MovieImdbGenreRegEx )
            genre = m.Groups("genre").Value
            If Not genres.Contains( genre ) Then genres.Add( genre )
        Next
        Return genres
    End Function
    
    Public Function GetImdbKeyWords(ByVal keylimit As Integer, ByVal imdbmirror As String, ByVal imdbid As String) As List(Of String)
        Dim keywd As New List(Of String)
        Dim tempstring As String = ""
        Dim webpage As New List(Of String)
        Dim wbpage As String = ""
        Try
            tempstring = imdbmirror & "title/" & imdbid & "keywords?ref_=tt_stry_kw"
            webpage.Clear()
            webpage = loadwebpage(Pref.proxysettings, tempstring, False)
            Dim webPg1 As String = String.Join( vbcrlf , webpage.ToArray() )
            For f = 0 To webpage.Count - 1
                If webpage(f).IndexOf("Plot Keywords</h1") <> -1 Then
                    Dim loc As Integer = f + 6
                    For g = loc to webpage.Count -10 'loc+500
                        If webpage(g).IndexOf("</tbody></table>") <> -1 Then
                            f = webpage.Count-1
                            Exit For
                        End If
                        If webpage(g).IndexOf("<a href=""/keyword") <>-1 Then wbpage &= webpage(g).TrimStart & webpage(g+1)
                    Next
                End If
            Next
            If wbpage <> "" Then
                Dim count As Integer = 0
                Dim keyw As String
                For Each m As Match In Regex.Matches( wbpage, MovieRegExs.REGEX_IMDB_KEYWORD )
                    keyw = m.Groups("keyword").Value
                    If Not keywd.Contains( keyw ) then
                        count +=1
                        keywd.Add( keyw )
                    End if
                    If count = keylimit Then Exit For
                Next
            End If
        Catch
        End Try
        Return keywd
    End Function

    Public Function GetTmdbkeywords(ByVal ID As String, ByVal keylimit As Integer) As List(Of String)
        Dim keywd As New List(Of String)
        Try
            Dim url As String = String.Format("http://api.themoviedb.org/3/movie/{0}/keywords?api_key={2}&language={1}", Id, TMDb.LanguageCodes(0), Utilities.TMDBAPI)
            Dim request = TryCast(System.Net.WebRequest.Create(url), System.Net.HttpWebRequest)
            request.Method = "GET"
            request.Proxy = Utilities.MyProxy
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2"
            Dim responseContent As String
            Using response = TryCast(request.GetResponse(), System.Net.HttpWebResponse)
              Using reader = New IO.StreamReader(response.GetResponseStream())
                responseContent = reader.ReadToEnd()
              End Using
            End Using
            If responseContent <> "" Then
                'example: {"id":584,"keywords":[{"id":830,"name":"car race"},{"id":999,"name":"sports car"},{"id":261,"name":"los angeles "},{"id":416,"name":"miami"}]}
                Dim newkey As New List(Of String)
                Dim RegExPattern = "name"":""(?<keyword>.*?)"""
                For Each m As Match In Regex.Matches(responseContent, RegExPattern, RegexOptions.Singleline)
                    Dim kwd As String = Net.WebUtility.HtmlDecode(m.Groups("keyword").Value)
                    newkey.Add(kwd)
                Next
                Dim count As Integer = 0
                For Each k In newkey
                    count = count +1
                    keywd.Add(k)
                    If count = keylimit Then Exit For
                Next
            End If
        Catch ex As Exception
            Return keywd
        End Try
        Return keywd
    End Function

    Public Function GetImdbPlots(ByVal imdbid As String) As List(Of String)
        Dim plotresults As New List(Of String)
        Dim Tempstring As String = ""
        Dim tempint As Integer = 0
        Dim movienfoarray As String = String.Empty
        Dim webpage As New List(Of String)
        Try
            tempstring = Pref.imdbmirror & "title/" & imdbid & "/plotsummary"
            Dim plots(20) As String
            webpage.Clear()
            webpage = loadwebpage(Pref.proxysettings, tempstring, False)
            tempint = 0
            Dim doo As Boolean = False
            For Each line In webpage
                If doo = True Then
                    plots(tempint) = line
                    doo = False
                End If
                If line.IndexOf("<p class=""plotSummary"">") <> -1 Then
                    tempint = tempint + 1
                    doo = True
                End If
            Next
            For Each item In plots
                If IsNothing(item) Then Continue For
                Dim PlotString As String = item.StripTagsLeaveContent
                PlotString = Regex.Replace(PlotString, "<.*?>", "").Trim
                PlotString = Utilities.cleanSpecChars(PlotString)
                plotresults.Add(PlotString)
            Next
        Catch
            
        End Try
        Return plotresults
    End Function

    Public Function getMVbodyADB(ByVal FullPathandFilename As String, ByRef MVSearchName As String)
        Monitor.Enter(Me)
        Dim Thetitle As Boolean = False
        Dim ParametersForScraper(10) As String
        Dim FinalScrapResult As String
        Dim Scraper As String = "metadata.musicvideos.imvdb" 'theaudiodb.com"
        Dim title As String = getArtistAndTitle(FullPathandFilename)
        MVSearchName = title
        Try
            ' 1st stage
            ParametersForScraper(0) = title.Replace(" ","%20")
            FinalScrapResult = DoScrape(Scraper, "CreateSearchUrl", ParametersForScraper, False, False)
            FinalScrapResult = FinalScrapResult.Replace("<url>", "")
            FinalScrapResult = FinalScrapResult.Replace("</url>", "")
            FinalScrapResult = FinalScrapResult.Replace(" ", "%20")

            ' 2st stage
            ParametersForScraper(0) = FinalScrapResult
            FinalScrapResult = DoScrape(Scraper, "GetSearchResults", ParametersForScraper, True)
            If FinalScrapResult.ToLower = "error" or FinalScrapResult.ToLower = "<results sorted=""yes""></results>" Then Return "error"
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode
            m_xmld = New XmlDocument()
            m_xmld.LoadXml(FinalScrapResult)
            m_nodelist = m_xmld.SelectNodes("/results/entity")
            For Each m_node In m_nodelist
                TheTitle = ValidateTitleMatch(m_node.ChildNodes.Item(0).InnerText, MVSearchName)
                If Not Thetitle Then Continue For
                Dim url = m_node.ChildNodes.Item(1).InnerText
                ParametersForScraper(0) = url
                Exit For
            Next
            If Not Thetitle Then Return "error"

            ' 3st stage
            FinalScrapResult = DoScrape(Scraper, "GetDetails", ParametersForScraper, True)
            If FinalScrapResult.ToLower = "error" Then
                Return "error"
            End If
            FinalScrapResult = ReplaceCharactersinXML(FinalScrapResult)
            FinalScrapResult = FinalScrapResult.Replace("details>","musicvideo>")
            FinalScrapResult = FinalScrapResult.Replace("</director>" & vbcrlf & "  <director>", " / ")
            If FinalScrapResult.IndexOf("&") <> -1 Then FinalScrapResult = FinalScrapResult.Replace("&", "&amp;")
            If FinalScrapResult.Contains("imvdbid") Then

            Else

            End If
            Return FinalScrapResult
        Catch ex As Exception
            Return "error"
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Public Function getMVbodyIMVDB(ByVal FullPathandFilename As String, ByRef MVSearchName As String)
        Monitor.Enter(Me)
        Dim Thetitle As Boolean = False
        Dim ParametersForScraper(10) As String
        Dim FinalScrapResult As String
        Dim Scraper As String = "metadata.musicvideos.imvdb"
        Dim title As String = getArtistAndTitle(FullPathandFilename)
        

        MVSearchName = title
        Try
            ' 1st stage
            ParametersForScraper(0) = title.Replace(" ","%20")
            FinalScrapResult = DoScrape(Scraper, "CreateSearchUrl", ParametersForScraper, False, False)
            FinalScrapResult = FinalScrapResult.Replace("<url>", "")
            FinalScrapResult = FinalScrapResult.Replace("</url>", "")
            FinalScrapResult = FinalScrapResult.Replace(" ", "%20")

            ' 2st stage
            ParametersForScraper(0) = FinalScrapResult
            FinalScrapResult = DoScrape(Scraper, "GetSearchResults", ParametersForScraper, True)
            If FinalScrapResult.ToLower = "error" or FinalScrapResult.ToLower = "<results sorted=""yes""></results>" Then Return "error"
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode
            m_xmld = New XmlDocument()
            m_xmld.LoadXml(FinalScrapResult)
            m_nodelist = m_xmld.SelectNodes("/results/entity")
            For Each m_node In m_nodelist
                TheTitle = ValidateTitleMatch(m_node.ChildNodes.Item(0).InnerText, MVSearchName)
                If Not Thetitle Then Continue For
                Dim url = m_node.ChildNodes.Item(1).InnerText
                ParametersForScraper(0) = url
                Exit For
            Next
            If Not Thetitle Then Return "error"

            ' 3st stage
            FinalScrapResult = DoScrape(Scraper, "GetDetails", ParametersForScraper, True)
            If FinalScrapResult.ToLower = "error" Then
                Return "error"
            End If
            FinalScrapResult = ReplaceCharactersinXML(FinalScrapResult)
            FinalScrapResult = FinalScrapResult.Replace("details>","musicvideo>")
            FinalScrapResult = FinalScrapResult.Replace("</studio>" & vbcrlf & "  <studio>", ", ")
            FinalScrapResult = FinalScrapResult.Replace("</country>" & vbcrlf & "  <country>", ", ")
            FinalScrapResult = FinalScrapResult.Replace("</credits>" & vbcrlf & "  <credits>", ", ")
            FinalScrapResult = FinalScrapResult.Replace("</director>" & vbcrlf & "  <director>", " / ")
            If FinalScrapResult.IndexOf("&") <> -1 Then FinalScrapResult = FinalScrapResult.Replace("&", "&amp;")
            Return FinalScrapResult
        Catch ex As Exception
            Return "error"
        Finally
            Monitor.Exit(Me)
        End Try
    End Function

    Private Function getArtistAndTitle(ByVal fullpathandfilename As String)
        Monitor.Enter(Me)
        Dim searchTerm As String = ""
        Dim filenameWithoutExtension As String = CleanYear(Path.GetFileNameWithoutExtension(fullpathandfilename))
        If filenameWithoutExtension.IndexOf(" - ") <> -1 Then
            searchTerm = filenameWithoutExtension
        Else 'assume /artist/title.ext convention
            Try
                Dim pathwithoutroot As String = Pref.folderandfilename(fullpathandfilename)
                Dim p() As String = pathwithoutroot.Split("\")
                Dim lastfolder As String = Utilities.GetLastFolder(fullpathandfilename)
                If p.Count > 2 Then
                    If lastfolder = filenameWithoutExtension AndAlso p(0) <> lastfolder Then
                        searchTerm = p(0)  & " - " & filenameWithoutExtension
                    Else
                        searchTerm = lastfolder & " - " & filenameWithoutExtension
                    End If
                Else
                    searchTerm = lastfolder & " - " & filenameWithoutExtension
                End If
            Catch
            End Try
        End If

        If searchTerm = "" Then searchTerm = filenameWithoutExtension
        
        Return searchTerm
        Monitor.Exit(Me)
    End Function

    Private Function ValidateTitleMatch(ByVal FullTitle As String, ByVal mvsearch As String) As Boolean
        Monitor.Enter(Me)
        Dim Similar As Double = CompareString(FullTitle, mvsearch)
        If Similar > 0.9 Then Return True
        Dim SongTitle As String = ""
        Dim splitsong() As String = FullTitle.Split("-"c)
        SongTitle = splitsong(1).ToLower.Trim
        Dim splitsearch() As String = mvsearch.Split("-"c)
        mvsearch = splitsearch(1).ToLower.Trim
        If SongTitle.Contains(mvsearch) Then Return True
        Return False
        Monitor.Exit(Me)
    End Function

    Private Function CleanYear(ByVal filename As String) As String
        '''Remove year if any.
        Dim movieyear As String = Utilities.GetYearByFilename(filename, False)
        If movieyear <> Nothing Then
            Dim posYear As Integer = filename.IndexOf(movieyear)
            If posYear <> -1 AndAlso posYear > 0 AndAlso posYear < filename.length Then
                filename = filename.Substring(0, posYear)
            End If
        End If
        Return filename.Trim
    End Function
    
End Class
