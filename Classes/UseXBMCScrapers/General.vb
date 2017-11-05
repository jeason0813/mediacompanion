Imports System.Net
Imports System.Threading
Imports System.Text.RegularExpressions
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Xml
Imports Media_Companion


Module General
    Const SetDefaults = True
    Dim TempEpisode As New TvEpisode
    Dim episodeInformation As New List(Of TvEpisode)
    Dim TempXMLEpisode As New TvEpisode
    Dim episodeXMLinformation As New List(Of TvEpisode)


#Region "Fields"
    Public mScraperManager As ScraperManager
    Public mLastQuery As ScraperQuery
    Public ParametersForScraper(10) As String
    Public FinalScrapResult As String
#End Region

#Region "XBMC Scraper Internal Routines"

    Private Function RetrieveUrls(ByVal URLAddress As String) As String
        Dim TMDBResponse As HttpWebResponse
        Dim responseFromServer As String = ""
        Try
            Dim TMDBRequest As HttpWebRequest = WebRequest.Create(URLAddress)
            TMDBRequest.Proxy = Utilities.MyProxy
            TMDBRequest.Accept = "application/json"
            TMDBRequest.ContentType = "application/json"
            TMDBRequest.Credentials = CredentialCache.DefaultCredentials
            TMDBRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2"
            TMDBResponse = CType(TMDBRequest.GetResponse(), HttpWebResponse)
            Using dataStream As IO.Stream = TMDBResponse.GetResponseStream()
                Using reader As New IO.StreamReader(dataStream)
                    responseFromServer = reader.ReadToEnd()
                End Using 'reader.Close()
            End Using 'dataStream.Close()
            TMDBResponse.Close()
        Catch ex As Exception
            'MessageBox.Show("ERROR:  " + ex.Message + vbCrLf + vbCrLf + "URL: " + URLAddress, "Error retrieving URL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return "ERROR" 'SK: added 
        End Try
        Return responseFromServer
    End Function

    Private Function ChooseScraper(ByVal ScraperIdentification As String) As Scraper

        If IsNothing(mScraperManager) then
		    mScraperManager = New ScraperManager(Path.Combine(My.Application.Info.DirectoryPath, "Assets\scrapers"))
        End If

        Dim TempScraper As Scraper = Nothing
        For Each c In mScraperManager.Scrapers
            If c.ID = ScraperIdentification Then
                TempScraper = c
                Exit For
            End If
        Next
        Return TempScraper
    End Function

    Public Function DoScrape(ByVal ScraperName As String, ByVal WhatFunction As String, ByVal InputParameters() As String, ByVal GetURL As Boolean, Optional ByVal convertAmps As Boolean = True, Optional ByVal InputBuffer As Integer = 1)
        Dim ChoosedScraper As Scraper
        InputBuffer -= 1
        Dim ScraperFunctiontoExecute As XElement
        If GetURL = True Then
            Dim PrimaryString As String = RetrieveUrls(InputParameters(InputBuffer))
            If PrimaryString = "ERROR" Then 'SK: added this test but calling functions don't test for "error"
                Return "Error"
                Exit Function
            End If
            InputParameters(InputBuffer) = PrimaryString 'SK: Primary string was returning as Nothing when the Throw Ex was commented out with a failed causing an exception
            If PrimaryString.Length = 0 Then 'SK: this test already existed
                Return "Error"
                Exit Function
            End If
        End If
        ChoosedScraper = ChooseScraper(ScraperName)
        ScraperFunctiontoExecute = ChoosedScraper.Func(WhatFunction)
        
        mLastQuery = New ScraperQuery(ChoosedScraper)
        Dim ScrapeResult As String = mLastQuery.Execute(ScraperFunctiontoExecute.Name.LocalName, InputParameters, convertAmps)
        If ScrapeResult.Length = 0 Then
            Return "Error"
            Exit Function
        End If
        Return ScrapeResult
    End Function

#End Region

#Region "Misc.Movies Routines"

    Public Function ReplaceCharactersinXML(ByVal Entrada As String) As String
        Dim StringOriginaltoXML As New XmlDocument
        Dim InicialTrailerPosition As Integer
        Dim FinalTrailerPosition As Integer
        Dim TempString As String = ""
        If Entrada.IndexOf("<trailer>") <> -1 Then
            InicialTrailerPosition = Entrada.IndexOf("<trailer>")
            FinalTrailerPosition = Entrada.LastIndexOf("</trailer>")
            TempString = Entrada.Substring(InicialTrailerPosition, (FinalTrailerPosition + 10 - InicialTrailerPosition))
            Entrada = Entrada.Remove(InicialTrailerPosition, (FinalTrailerPosition + 10 - InicialTrailerPosition))
            Entrada = ReplaceSpecialCharacters(Entrada)
            Entrada = Entrada.Insert(Entrada.LastIndexOf("</details>") - 1, TempString & Chr(13))
            Return Entrada
        Else
            Return ReplaceSpecialCharacters(Entrada)
        End If
    End Function

    Public Function ReplaceSpecialCharacters(ByVal filterstring As String) As String
        Try
            If filterstring.IndexOf("&#919;") <> -1 Then filterstring = filterstring.Replace("&#919;", "Η")
            If filterstring.IndexOf("&#918;") <> -1 Then filterstring = filterstring.Replace("&#918;", "Ζ")
            If filterstring.IndexOf("&#917;") <> -1 Then filterstring = filterstring.Replace("&#917;", "Ε")
            If filterstring.IndexOf("&#916;") <> -1 Then filterstring = filterstring.Replace("&#916;", "Δ")
            If filterstring.IndexOf("&#915;") <> -1 Then filterstring = filterstring.Replace("&#915;", "Γ")
            If filterstring.IndexOf("&#914;") <> -1 Then filterstring = filterstring.Replace("&#914;", "Β")
            If filterstring.IndexOf("&#913;") <> -1 Then filterstring = filterstring.Replace("&#913;", "Α")
            If filterstring.IndexOf("&#732;") <> -1 Then filterstring = filterstring.Replace("&#732;", "˜")
            If filterstring.IndexOf("&#710;") <> -1 Then filterstring = filterstring.Replace("&#710;", "ˆ")
            If filterstring.IndexOf("&#402;") <> -1 Then filterstring = filterstring.Replace("&#402;", "ƒ")
            If filterstring.IndexOf("&#376;") <> -1 Then filterstring = filterstring.Replace("&#376;", "Ÿ")
            If filterstring.IndexOf("&#353;") <> -1 Then filterstring = filterstring.Replace("&#353;", "š")
            If filterstring.IndexOf("&#352;") <> -1 Then filterstring = filterstring.Replace("&#352;", "Š")
            If filterstring.IndexOf("&#339;") <> -1 Then filterstring = filterstring.Replace("&#339;", "œ")
            If filterstring.IndexOf("&#338;") <> -1 Then filterstring = filterstring.Replace("&#338;", "Œ")
            If filterstring.IndexOf("&#937;") <> -1 Then filterstring = filterstring.Replace("&#937;", "Ω")
            If filterstring.IndexOf("&#936;") <> -1 Then filterstring = filterstring.Replace("&#936;", "Ψ")
            If filterstring.IndexOf("&#935;") <> -1 Then filterstring = filterstring.Replace("&#935;", "Χ")
            If filterstring.IndexOf("&#934;") <> -1 Then filterstring = filterstring.Replace("&#934;", "Φ")
            If filterstring.IndexOf("&#933;") <> -1 Then filterstring = filterstring.Replace("&#933;", "Υ")
            If filterstring.IndexOf("&#932;") <> -1 Then filterstring = filterstring.Replace("&#932;", "Τ")
            If filterstring.IndexOf("&#931;") <> -1 Then filterstring = filterstring.Replace("&#931;", "Σ")
            If filterstring.IndexOf("&#929;") <> -1 Then filterstring = filterstring.Replace("&#929;", "Ρ")
            If filterstring.IndexOf("&#928;") <> -1 Then filterstring = filterstring.Replace("&#928;", "Π")
            If filterstring.IndexOf("&#927;") <> -1 Then filterstring = filterstring.Replace("&#927;", "Ο")
            If filterstring.IndexOf("&#926;") <> -1 Then filterstring = filterstring.Replace("&#926;", "Ξ")
            If filterstring.IndexOf("&#924;") <> -1 Then filterstring = filterstring.Replace("&#924;", "Μ")
            If filterstring.IndexOf("&#923;") <> -1 Then filterstring = filterstring.Replace("&#923;", "Λ")
            If filterstring.IndexOf("&#922;") <> -1 Then filterstring = filterstring.Replace("&#922;", "Κ")
            If filterstring.IndexOf("&#921;") <> -1 Then filterstring = filterstring.Replace("&#921;", "Ι")
            If filterstring.IndexOf("&#920;") <> -1 Then filterstring = filterstring.Replace("&#920;", "Θ")
            If filterstring.IndexOf("&#955;") <> -1 Then filterstring = filterstring.Replace("&#955;", "λ")
            If filterstring.IndexOf("&#954;") <> -1 Then filterstring = filterstring.Replace("&#954;", "κ")
            If filterstring.IndexOf("&#953;") <> -1 Then filterstring = filterstring.Replace("&#953;", "ι")
            If filterstring.IndexOf("&#952;") <> -1 Then filterstring = filterstring.Replace("&#952;", "θ")
            If filterstring.IndexOf("&#951;") <> -1 Then filterstring = filterstring.Replace("&#951;", "η")
            If filterstring.IndexOf("&#950;") <> -1 Then filterstring = filterstring.Replace("&#950;", "ζ")
            If filterstring.IndexOf("&#949;") <> -1 Then filterstring = filterstring.Replace("&#949;", "ε")
            If filterstring.IndexOf("&#948;") <> -1 Then filterstring = filterstring.Replace("&#948;", "δ")
            If filterstring.IndexOf("&#947;") <> -1 Then filterstring = filterstring.Replace("&#947;", "γ")
            If filterstring.IndexOf("&#946;") <> -1 Then filterstring = filterstring.Replace("&#946;", "β")
            If filterstring.IndexOf("&#945;") <> -1 Then filterstring = filterstring.Replace("&#945;", "α")
            If filterstring.IndexOf("&#969;") <> -1 Then filterstring = filterstring.Replace("&#969;", "ω")
            If filterstring.IndexOf("&#968;") <> -1 Then filterstring = filterstring.Replace("&#968;", "ψ")
            If filterstring.IndexOf("&#967;") <> -1 Then filterstring = filterstring.Replace("&#967;", "χ")
            If filterstring.IndexOf("&#966;") <> -1 Then filterstring = filterstring.Replace("&#966;", "φ")
            If filterstring.IndexOf("&#965;") <> -1 Then filterstring = filterstring.Replace("&#965;", "υ")
            If filterstring.IndexOf("&#964;") <> -1 Then filterstring = filterstring.Replace("&#964;", "τ")
            If filterstring.IndexOf("&#963;") <> -1 Then filterstring = filterstring.Replace("&#963;", "σ")
            If filterstring.IndexOf("&#962;") <> -1 Then filterstring = filterstring.Replace("&#962;", "ς")
            If filterstring.IndexOf("&#961;") <> -1 Then filterstring = filterstring.Replace("&#961;", "ρ")
            If filterstring.IndexOf("&#960;") <> -1 Then filterstring = filterstring.Replace("&#960;", "π")
            If filterstring.IndexOf("&#959;") <> -1 Then filterstring = filterstring.Replace("&#959;", "ο")
            If filterstring.IndexOf("&#958;") <> -1 Then filterstring = filterstring.Replace("&#958;", "ξ")
            If filterstring.IndexOf("&#957;") <> -1 Then filterstring = filterstring.Replace("&#957;", "ν")
            If filterstring.IndexOf("&#956;") <> -1 Then filterstring = filterstring.Replace("&#956;", "μ")
            If filterstring.IndexOf("&#8240;") <> -1 Then filterstring = filterstring.Replace("&#8240;", "‰")
            If filterstring.IndexOf("&#8230;") <> -1 Then filterstring = filterstring.Replace("&#8230;", "…")
            If filterstring.IndexOf("&#8226;") <> -1 Then filterstring = filterstring.Replace("&#8226;", "•")
            If filterstring.IndexOf("&#8225;") <> -1 Then filterstring = filterstring.Replace("&#8225;", "‡")
            If filterstring.IndexOf("&#8224;") <> -1 Then filterstring = filterstring.Replace("&#8224;", "†")
            If filterstring.IndexOf("&#8222;") <> -1 Then filterstring = filterstring.Replace("&#8222;", "„")
            If filterstring.IndexOf("&#8218;") <> -1 Then filterstring = filterstring.Replace("&#8218;", "‚")
            If filterstring.IndexOf("&#8217;") <> -1 Then filterstring = filterstring.Replace("&#8217;", "’")
            If filterstring.IndexOf("&#8216;") <> -1 Then filterstring = filterstring.Replace("&#8216;", "‘")
            If filterstring.IndexOf("&#8212;") <> -1 Then filterstring = filterstring.Replace("&#8212;", "—")
            If filterstring.IndexOf("&#8211;") <> -1 Then filterstring = filterstring.Replace("&#8211;", "–")
            If filterstring.IndexOf("&#8805;") <> -1 Then filterstring = filterstring.Replace("&#8805;", "≥")
            If filterstring.IndexOf("&#8804;") <> -1 Then filterstring = filterstring.Replace("&#8804;", "≤")
            If filterstring.IndexOf("&#8801;") <> -1 Then filterstring = filterstring.Replace("&#8801;", "≡")
            If filterstring.IndexOf("&#8800;") <> -1 Then filterstring = filterstring.Replace("&#8800;", "≠")
            If filterstring.IndexOf("&#8776;") <> -1 Then filterstring = filterstring.Replace("&#8776;", "≈")
            If filterstring.IndexOf("&#8747;") <> -1 Then filterstring = filterstring.Replace("&#8747;", "∫")
            If filterstring.IndexOf("&#8745;") <> -1 Then filterstring = filterstring.Replace("&#8745;", "∩")
            If filterstring.IndexOf("&#8734;") <> -1 Then filterstring = filterstring.Replace("&#8734;", "∞")
            If filterstring.IndexOf("&#8730;") <> -1 Then filterstring = filterstring.Replace("&#8730;", "√")
            If filterstring.IndexOf("&#8721;") <> -1 Then filterstring = filterstring.Replace("&#8721;", "∑")
            If filterstring.IndexOf("&#8719;") <> -1 Then filterstring = filterstring.Replace("&#8719;", "∏")
            If filterstring.IndexOf("&#8706;") <> -1 Then filterstring = filterstring.Replace("&#8706;", "∂")
            If filterstring.IndexOf("&#8596;") <> -1 Then filterstring = filterstring.Replace("&#8596;", "↔")
            If filterstring.IndexOf("&#8595;") <> -1 Then filterstring = filterstring.Replace("&#8595;", "↓")
            If filterstring.IndexOf("&#8594;") <> -1 Then filterstring = filterstring.Replace("&#8594;", "→")
            If filterstring.IndexOf("&#8593;") <> -1 Then filterstring = filterstring.Replace("&#8593;", "↑")
            If filterstring.IndexOf("&#8592;") <> -1 Then filterstring = filterstring.Replace("&#8592;", "←")
            If filterstring.IndexOf("&#8482;") <> -1 Then filterstring = filterstring.Replace("&#8482;", "™")
            If filterstring.IndexOf("&#8364;") <> -1 Then filterstring = filterstring.Replace("&#8364;", "€")
            If filterstring.IndexOf("&#8260;") <> -1 Then filterstring = filterstring.Replace("&#8260;", "⁄")
            If filterstring.IndexOf("&#8254;") <> -1 Then filterstring = filterstring.Replace("&#8254;", "‾")
            If filterstring.IndexOf("&#8250;") <> -1 Then filterstring = filterstring.Replace("&#8250;", "›")
            If filterstring.IndexOf("&#8249;") <> -1 Then filterstring = filterstring.Replace("&#8249;", "‹")
            If filterstring.IndexOf("&#161;") <> -1 Then filterstring = filterstring.Replace("&#161;", Chr(161))
            If filterstring.IndexOf("&#162;") <> -1 Then filterstring = filterstring.Replace("&#162;", Chr(162))
            If filterstring.IndexOf("&#163;") <> -1 Then filterstring = filterstring.Replace("&#163;", Chr(163))
            If filterstring.IndexOf("&#164;") <> -1 Then filterstring = filterstring.Replace("&#164;", Chr(164))
            If filterstring.IndexOf("&#165;") <> -1 Then filterstring = filterstring.Replace("&#165;", Chr(165))
            If filterstring.IndexOf("&#166;") <> -1 Then filterstring = filterstring.Replace("&#166;", Chr(166))
            If filterstring.IndexOf("&#167;") <> -1 Then filterstring = filterstring.Replace("&#167;", Chr(167))
            If filterstring.IndexOf("&#168;") <> -1 Then filterstring = filterstring.Replace("&#168;", Chr(168))
            If filterstring.IndexOf("&#170;") <> -1 Then filterstring = filterstring.Replace("&#170;", Chr(170))
            If filterstring.IndexOf("&#171;") <> -1 Then filterstring = filterstring.Replace("&#171;", Chr(171))
            If filterstring.IndexOf("&#172;") <> -1 Then filterstring = filterstring.Replace("&#172;", Chr(172))
            If filterstring.IndexOf("&#173;") <> -1 Then filterstring = filterstring.Replace("&#173;", Chr(173))
            If filterstring.IndexOf("&#174;") <> -1 Then filterstring = filterstring.Replace("&#174;", Chr(174))
            If filterstring.IndexOf("&#175;") <> -1 Then filterstring = filterstring.Replace("&#175;", Chr(175))
            If filterstring.IndexOf("&#176;") <> -1 Then filterstring = filterstring.Replace("&#176;", Chr(176))
            If filterstring.IndexOf("&#177;") <> -1 Then filterstring = filterstring.Replace("&#177;", Chr(177))
            If filterstring.IndexOf("&#178;") <> -1 Then filterstring = filterstring.Replace("&#178;", Chr(178))
            If filterstring.IndexOf("&#179;") <> -1 Then filterstring = filterstring.Replace("&#179;", Chr(179))
            If filterstring.IndexOf("&#198;") <> -1 Then filterstring = filterstring.Replace("&#198;", Chr(198))
            If filterstring.IndexOf("&amp;") <> -1 Then filterstring = filterstring.Replace("&amp;", "&")
            If filterstring.IndexOf("&quot;") <> -1 Then filterstring = filterstring.Replace("&quot;", Chr(34))
            If filterstring.IndexOf("&lt;") <> -1 Then filterstring = filterstring.Replace("&lt;", "<")
            If filterstring.IndexOf("&gt;") <> -1 Then filterstring = filterstring.Replace("&gt;", ">")
            If filterstring.IndexOf("&#138;") <> -1 Then filterstring = filterstring.Replace("&#138;", Chr(138))
            If filterstring.IndexOf("&#140;") <> -1 Then filterstring = filterstring.Replace("&#140;", Chr(140))
            If filterstring.IndexOf("&#142;") <> -1 Then filterstring = filterstring.Replace("&#142;", Chr(142))
            If filterstring.IndexOf("&#153;") <> -1 Then filterstring = filterstring.Replace("&#153;", Chr(153))
            If filterstring.IndexOf("&#154;") <> -1 Then filterstring = filterstring.Replace("&#154;", Chr(154))
            If filterstring.IndexOf("&#156;") <> -1 Then filterstring = filterstring.Replace("&#156;", Chr(156))
            If filterstring.IndexOf("&#158;") <> -1 Then filterstring = filterstring.Replace("&#158;", Chr(158))
            If filterstring.IndexOf("&#159;") <> -1 Then filterstring = filterstring.Replace("&#159;", Chr(159))
            If filterstring.IndexOf("&#160;") <> -1 Then filterstring = filterstring.Replace("&#160;", Chr(160))
            If filterstring.IndexOf("&#169;") <> -1 Then filterstring = filterstring.Replace("&#169;", Chr(169))
            If filterstring.IndexOf("&#178;") <> -1 Then filterstring = filterstring.Replace("&#178;", Chr(178))
            If filterstring.IndexOf("&#179;") <> -1 Then filterstring = filterstring.Replace("&#179;", Chr(179))
            If filterstring.IndexOf("&#181;") <> -1 Then filterstring = filterstring.Replace("&#181;", Chr(181))
            If filterstring.IndexOf("&#183;") <> -1 Then filterstring = filterstring.Replace("&#183;", Chr(183))
            If filterstring.IndexOf("&#188;") <> -1 Then filterstring = filterstring.Replace("&#188;", Chr(188))
            If filterstring.IndexOf("&#189;") <> -1 Then filterstring = filterstring.Replace("&#189;", Chr(189))
            If filterstring.IndexOf("&#190;") <> -1 Then filterstring = filterstring.Replace("&#190;", Chr(190))
            If filterstring.IndexOf("&#192;") <> -1 Then filterstring = filterstring.Replace("&#192;", Chr(192))
            If filterstring.IndexOf("&#193;") <> -1 Then filterstring = filterstring.Replace("&#193;", Chr(193))
            If filterstring.IndexOf("&#194;") <> -1 Then filterstring = filterstring.Replace("&#194;", Chr(194))
            If filterstring.IndexOf("&#195;") <> -1 Then filterstring = filterstring.Replace("&#195;", Chr(195))
            If filterstring.IndexOf("&#196;") <> -1 Then filterstring = filterstring.Replace("&#196;", Chr(196))
            If filterstring.IndexOf("&#197;") <> -1 Then filterstring = filterstring.Replace("&#197;", Chr(197))
            If filterstring.IndexOf("&#199;") <> -1 Then filterstring = filterstring.Replace("&#199;", Chr(199))
            If filterstring.IndexOf("&#200;") <> -1 Then filterstring = filterstring.Replace("&#200;", Chr(200))
            If filterstring.IndexOf("&#201;") <> -1 Then filterstring = filterstring.Replace("&#201;", Chr(201))
            If filterstring.IndexOf("&#203;") <> -1 Then filterstring = filterstring.Replace("&#203;", Chr(203))
            If filterstring.IndexOf("&#204;") <> -1 Then filterstring = filterstring.Replace("&#204;", Chr(204))
            If filterstring.IndexOf("&#205;") <> -1 Then filterstring = filterstring.Replace("&#205;", Chr(205))
            If filterstring.IndexOf("&#206;") <> -1 Then filterstring = filterstring.Replace("&#206;", Chr(206))
            If filterstring.IndexOf("&#207;") <> -1 Then filterstring = filterstring.Replace("&#207;", Chr(207))
            If filterstring.IndexOf("&#208;") <> -1 Then filterstring = filterstring.Replace("&#208;", Chr(208))
            If filterstring.IndexOf("&#209;") <> -1 Then filterstring = filterstring.Replace("&#209;", Chr(209))
            If filterstring.IndexOf("&#210;") <> -1 Then filterstring = filterstring.Replace("&#210;", Chr(210))
            If filterstring.IndexOf("&#211;") <> -1 Then filterstring = filterstring.Replace("&#211;", Chr(211))
            If filterstring.IndexOf("&#212;") <> -1 Then filterstring = filterstring.Replace("&#212;", Chr(212))
            If filterstring.IndexOf("&#213;") <> -1 Then filterstring = filterstring.Replace("&#213;", Chr(213))
            If filterstring.IndexOf("&#214;") <> -1 Then filterstring = filterstring.Replace("&#214;", Chr(214))
            If filterstring.IndexOf("&#215;") <> -1 Then filterstring = filterstring.Replace("&#215;", Chr(215))
            If filterstring.IndexOf("&#216;") <> -1 Then filterstring = filterstring.Replace("&#216;", Chr(216))
            If filterstring.IndexOf("&#217;") <> -1 Then filterstring = filterstring.Replace("&#217;", Chr(217))
            If filterstring.IndexOf("&#218;") <> -1 Then filterstring = filterstring.Replace("&#218;", Chr(218))
            If filterstring.IndexOf("&#219;") <> -1 Then filterstring = filterstring.Replace("&#219;", Chr(219))
            If filterstring.IndexOf("&#220;") <> -1 Then filterstring = filterstring.Replace("&#220;", Chr(220))
            If filterstring.IndexOf("&#221;") <> -1 Then filterstring = filterstring.Replace("&#221;", Chr(221))
            If filterstring.IndexOf("&#222;") <> -1 Then filterstring = filterstring.Replace("&#222;", Chr(222))
            If filterstring.IndexOf("&#223;") <> -1 Then filterstring = filterstring.Replace("&#223;", Chr(223))
            If filterstring.IndexOf("&#224;") <> -1 Then filterstring = filterstring.Replace("&#224;", Chr(224))
            If filterstring.IndexOf("&#225;") <> -1 Then filterstring = filterstring.Replace("&#225;", Chr(225))
            If filterstring.IndexOf("&#226;") <> -1 Then filterstring = filterstring.Replace("&#226;", Chr(226))
            If filterstring.IndexOf("&#227;") <> -1 Then filterstring = filterstring.Replace("&#227;", Chr(227))
            If filterstring.IndexOf("&#228;") <> -1 Then filterstring = filterstring.Replace("&#228;", Chr(228))
            If filterstring.IndexOf("&#229;") <> -1 Then filterstring = filterstring.Replace("&#229;", Chr(229))
            If filterstring.IndexOf("&#230;") <> -1 Then filterstring = filterstring.Replace("&#230;", Chr(230))
            If filterstring.IndexOf("&#231;") <> -1 Then filterstring = filterstring.Replace("&#231;", Chr(231))
            If filterstring.IndexOf("&#232;") <> -1 Then filterstring = filterstring.Replace("&#232;", Chr(232))
            If filterstring.IndexOf("&#233;") <> -1 Then filterstring = filterstring.Replace("&#233;", Chr(233))
            If filterstring.IndexOf("&#234;") <> -1 Then filterstring = filterstring.Replace("&#234;", Chr(234))
            If filterstring.IndexOf("&#235;") <> -1 Then filterstring = filterstring.Replace("&#235;", Chr(235))
            If filterstring.IndexOf("&#236;") <> -1 Then filterstring = filterstring.Replace("&#236;", Chr(236))
            If filterstring.IndexOf("&#237;") <> -1 Then filterstring = filterstring.Replace("&#237;", Chr(237))
            If filterstring.IndexOf("&#238;") <> -1 Then filterstring = filterstring.Replace("&#238;", Chr(238))
            If filterstring.IndexOf("&#239;") <> -1 Then filterstring = filterstring.Replace("&#239;", Chr(239))
            If filterstring.IndexOf("&#240;") <> -1 Then filterstring = filterstring.Replace("&#240;", Chr(240))
            If filterstring.IndexOf("&#241;") <> -1 Then filterstring = filterstring.Replace("&#241;", Chr(241))
            If filterstring.IndexOf("&#242;") <> -1 Then filterstring = filterstring.Replace("&#242;", Chr(242))
            If filterstring.IndexOf("&#243;") <> -1 Then filterstring = filterstring.Replace("&#243;", Chr(243))
            If filterstring.IndexOf("&#244;") <> -1 Then filterstring = filterstring.Replace("&#244;", Chr(244))
            If filterstring.IndexOf("&#245;") <> -1 Then filterstring = filterstring.Replace("&#245;", Chr(245))
            If filterstring.IndexOf("&#246;") <> -1 Then filterstring = filterstring.Replace("&#246;", Chr(246))
            If filterstring.IndexOf("&#247;") <> -1 Then filterstring = filterstring.Replace("&#247;", Chr(247))
            If filterstring.IndexOf("&#248;") <> -1 Then filterstring = filterstring.Replace("&#248;", Chr(248))
            If filterstring.IndexOf("&#249;") <> -1 Then filterstring = filterstring.Replace("&#249;", Chr(249))
            If filterstring.IndexOf("&#250;") <> -1 Then filterstring = filterstring.Replace("&#250;", Chr(250))
            If filterstring.IndexOf("&#251;") <> -1 Then filterstring = filterstring.Replace("&#251;", Chr(251))
            If filterstring.IndexOf("&#252;") <> -1 Then filterstring = filterstring.Replace("&#252;", Chr(252))
            If filterstring.IndexOf("&#253;") <> -1 Then filterstring = filterstring.Replace("&#253;", Chr(253))
            If filterstring.IndexOf("&#254;") <> -1 Then filterstring = filterstring.Replace("&#254;", Chr(254))
            If filterstring.IndexOf("&#255;") <> -1 Then filterstring = filterstring.Replace("&#255;", Chr(255))
            If filterstring.IndexOf("&#38;") <> -1 Then filterstring = filterstring.Replace("&#38;", Chr(38))
            If filterstring.IndexOf("&#34;") <> -1 Then filterstring = filterstring.Replace("&#34;", Chr(34))
            If filterstring.IndexOf("&#x20;") <> -1 Then filterstring = filterstring.Replace("&#x27;", " ")
            If filterstring.IndexOf("&#x21;") <> -1 Then filterstring = filterstring.Replace("&#x21;", "!")
            If filterstring.IndexOf("&#x22;") <> -1 Then filterstring = filterstring.Replace("&#x22;", Chr(34))
            If filterstring.IndexOf("&#x23;") <> -1 Then filterstring = filterstring.Replace("&#x23;", "#")
            If filterstring.IndexOf("&#x24;") <> -1 Then filterstring = filterstring.Replace("&#x24;", "$")
            If filterstring.IndexOf("&#x25;") <> -1 Then filterstring = filterstring.Replace("&#x25;", "%")
            If filterstring.IndexOf("&#x26;") <> -1 Then filterstring = filterstring.Replace("&#x26;", "&")
            If filterstring.IndexOf("&#x27;") <> -1 Then filterstring = filterstring.Replace("&#x27;", "'")
            If filterstring.IndexOf("&#x28;") <> -1 Then filterstring = filterstring.Replace("&#x28;", "(")
            If filterstring.IndexOf("&#x29;") <> -1 Then filterstring = filterstring.Replace("&#x29;", ")")
            If filterstring.IndexOf("&#x2A;") <> -1 Then filterstring = filterstring.Replace("&#x2A;", "*")
            If filterstring.IndexOf("&#x2B;") <> -1 Then filterstring = filterstring.Replace("&#x2B;", "+")
            If filterstring.IndexOf("&#x2C;") <> -1 Then filterstring = filterstring.Replace("&#x2C;", ",")
            If filterstring.IndexOf("&#x2D;") <> -1 Then filterstring = filterstring.Replace("&#x2D;", "-")
            If filterstring.IndexOf("&#x2E;") <> -1 Then filterstring = filterstring.Replace("&#x2E;", ".")
            If filterstring.IndexOf("&#x2F;") <> -1 Then filterstring = filterstring.Replace("&#x2F;", "/")


            If filterstring.IndexOf("&#x3A;") <> -1 Then filterstring = filterstring.Replace("&#x3A;", ":")
            If filterstring.IndexOf("&#x3B;") <> -1 Then filterstring = filterstring.Replace("&#x3B;", ";")
            If filterstring.IndexOf("&#x3C;") <> -1 Then filterstring = filterstring.Replace("&#x3C;", "<")
            If filterstring.IndexOf("&#x3D;") <> -1 Then filterstring = filterstring.Replace("&#x3D;", "=")
            If filterstring.IndexOf("&#x3E;") <> -1 Then filterstring = filterstring.Replace("&#x3E;", ">")
            If filterstring.IndexOf("&#x3F;") <> -1 Then filterstring = filterstring.Replace("&#x3F;", "?")


            If filterstring.IndexOf("&#x40;") <> -1 Then filterstring = filterstring.Replace("&#x40;", "@")
            If filterstring.IndexOf("&#x5B;") <> -1 Then filterstring = filterstring.Replace("&#x5B;", "[")
            If filterstring.IndexOf("&#x5C;") <> -1 Then filterstring = filterstring.Replace("&#x5C;", "\")
            If filterstring.IndexOf("&#x5D;") <> -1 Then filterstring = filterstring.Replace("&#x5D;", "]")
            If filterstring.IndexOf("&#x5E;") <> -1 Then filterstring = filterstring.Replace("&#x5E;", "^")
            If filterstring.IndexOf("&#x5F;") <> -1 Then filterstring = filterstring.Replace("&#x5F;", "_")
            If filterstring.IndexOf("&#x60;") <> -1 Then filterstring = filterstring.Replace("&#x60;", "`")
            If filterstring.IndexOf("&#x7B;") <> -1 Then filterstring = filterstring.Replace("&#x7B;", "{")
            If filterstring.IndexOf("&#x7C;") <> -1 Then filterstring = filterstring.Replace("&#x7C;", "|")
            If filterstring.IndexOf("&#x7D;") <> -1 Then filterstring = filterstring.Replace("&#x7D;", "}")
            If filterstring.IndexOf("&#x7E;") <> -1 Then filterstring = filterstring.Replace("&#x7E;", "~")


            If filterstring.IndexOf("&#x80;") <> -1 Then filterstring = filterstring.Replace("&#x80;", "€")
            If filterstring.IndexOf("&#x82;") <> -1 Then filterstring = filterstring.Replace("&#x82;", "‚")
            If filterstring.IndexOf("&#x83;") <> -1 Then filterstring = filterstring.Replace("&#x83;", "ƒ")
            If filterstring.IndexOf("&#x84;") <> -1 Then filterstring = filterstring.Replace("&#x84;", "„")
            If filterstring.IndexOf("&#x85;") <> -1 Then filterstring = filterstring.Replace("&#x85;", "…")
            If filterstring.IndexOf("&#x86;") <> -1 Then filterstring = filterstring.Replace("&#x86;", "†")
            If filterstring.IndexOf("&#x87;") <> -1 Then filterstring = filterstring.Replace("&#x87;", "‡")
            If filterstring.IndexOf("&#x88;") <> -1 Then filterstring = filterstring.Replace("&#x88;", "ˆ")
            If filterstring.IndexOf("&#x89;") <> -1 Then filterstring = filterstring.Replace("&#x89;", "‰")
            If filterstring.IndexOf("&#x8A;") <> -1 Then filterstring = filterstring.Replace("&#x8A;", "Š")
            If filterstring.IndexOf("&#x8B;") <> -1 Then filterstring = filterstring.Replace("&#x8B;", "‹")
            If filterstring.IndexOf("&#x8C;") <> -1 Then filterstring = filterstring.Replace("&#x8C;", "Œ")
            If filterstring.IndexOf("&#x8E;") <> -1 Then filterstring = filterstring.Replace("&#x8E;", "Ž")


            If filterstring.IndexOf("&#x91;") <> -1 Then filterstring = filterstring.Replace("&#x91;", "‘")
            If filterstring.IndexOf("&#x92;") <> -1 Then filterstring = filterstring.Replace("&#x92;", "’")
            If filterstring.IndexOf("&#x93;") <> -1 Then filterstring = filterstring.Replace("&#x93;", """")
            If filterstring.IndexOf("&#x94;") <> -1 Then filterstring = filterstring.Replace("&#x94;", """")
            If filterstring.IndexOf("&#x95;") <> -1 Then filterstring = filterstring.Replace("&#x95;", "•")
            If filterstring.IndexOf("&#x96;") <> -1 Then filterstring = filterstring.Replace("&#x96;", "–")
            If filterstring.IndexOf("&#x97;") <> -1 Then filterstring = filterstring.Replace("&#x97;", "—")
            If filterstring.IndexOf("&#x98;") <> -1 Then filterstring = filterstring.Replace("&#x98;", "˜")
            If filterstring.IndexOf("&#x99;") <> -1 Then filterstring = filterstring.Replace("&#x99;", "™")
            If filterstring.IndexOf("&#x9A;") <> -1 Then filterstring = filterstring.Replace("&#x9A;", "š")
            If filterstring.IndexOf("&#x9B;") <> -1 Then filterstring = filterstring.Replace("&#x9B;", "›")
            If filterstring.IndexOf("&#x9C;") <> -1 Then filterstring = filterstring.Replace("&#x9C;", "œ")
            If filterstring.IndexOf("&#x9E;") <> -1 Then filterstring = filterstring.Replace("&#x9E;", "ž")
            If filterstring.IndexOf("&#x9F;") <> -1 Then filterstring = filterstring.Replace("&#x9F;", "Ÿ")


            If filterstring.IndexOf("&#xA0;") <> -1 Then filterstring = filterstring.Replace("&#xA0;", " ")
            If filterstring.IndexOf("&#xA1;") <> -1 Then filterstring = filterstring.Replace("&#xA1;", "¡")
            If filterstring.IndexOf("&#xA2;") <> -1 Then filterstring = filterstring.Replace("&#xA2;", "¢")
            If filterstring.IndexOf("&#xA3;") <> -1 Then filterstring = filterstring.Replace("&#xA3;", "£")
            If filterstring.IndexOf("&#xA4;") <> -1 Then filterstring = filterstring.Replace("&#xA4;", "¤")
            If filterstring.IndexOf("&#xA5;") <> -1 Then filterstring = filterstring.Replace("&#xA5;", "¥")
            If filterstring.IndexOf("&#xA6;") <> -1 Then filterstring = filterstring.Replace("&#xA6;", "¦")
            If filterstring.IndexOf("&#xA7;") <> -1 Then filterstring = filterstring.Replace("&#xA7;", "§")
            If filterstring.IndexOf("&#xA8;") <> -1 Then filterstring = filterstring.Replace("&#xA8;", "¨")
            If filterstring.IndexOf("&#xA9;") <> -1 Then filterstring = filterstring.Replace("&#xA9;", "©")
            If filterstring.IndexOf("&#xAA;") <> -1 Then filterstring = filterstring.Replace("&#xAA;", "ª")
            If filterstring.IndexOf("&#xAB;") <> -1 Then filterstring = filterstring.Replace("&#xAB;", "«")
            If filterstring.IndexOf("&#xAC;") <> -1 Then filterstring = filterstring.Replace("&#xAC;", "¬")
            If filterstring.IndexOf("&#xAD;") <> -1 Then filterstring = filterstring.Replace("&#xAD;", " ")
            If filterstring.IndexOf("&#xAE;") <> -1 Then filterstring = filterstring.Replace("&#xAE;", "®")
            If filterstring.IndexOf("&#xAF;") <> -1 Then filterstring = filterstring.Replace("&#xAF;", "¯")


            If filterstring.IndexOf("&#xB0;") <> -1 Then filterstring = filterstring.Replace("&#xB0;", "°")
            If filterstring.IndexOf("&#xB1;") <> -1 Then filterstring = filterstring.Replace("&#xB1;", "±")
            If filterstring.IndexOf("&#xB2;") <> -1 Then filterstring = filterstring.Replace("&#xB2;", "²")
            If filterstring.IndexOf("&#xB3;") <> -1 Then filterstring = filterstring.Replace("&#xB3;", "³")
            If filterstring.IndexOf("&#xB4;") <> -1 Then filterstring = filterstring.Replace("&#xB4;", "´")
            If filterstring.IndexOf("&#xB5;") <> -1 Then filterstring = filterstring.Replace("&#xB5;", "µ")
            If filterstring.IndexOf("&#xB6;") <> -1 Then filterstring = filterstring.Replace("&#xB6;", "¶")
            If filterstring.IndexOf("&#xB7;") <> -1 Then filterstring = filterstring.Replace("&#xB7;", "·")
            If filterstring.IndexOf("&#xB8;") <> -1 Then filterstring = filterstring.Replace("&#xB8;", "¸")
            If filterstring.IndexOf("&#xB9;") <> -1 Then filterstring = filterstring.Replace("&#xB9;", "¹")
            If filterstring.IndexOf("&#xBA;") <> -1 Then filterstring = filterstring.Replace("&#xBA;", "º")
            If filterstring.IndexOf("&#xBB;") <> -1 Then filterstring = filterstring.Replace("&#xBB;", "»")
            If filterstring.IndexOf("&#xBC;") <> -1 Then filterstring = filterstring.Replace("&#xBC;", "¼")
            If filterstring.IndexOf("&#xBD;") <> -1 Then filterstring = filterstring.Replace("&#xBD;", "½")
            If filterstring.IndexOf("&#xBE;") <> -1 Then filterstring = filterstring.Replace("&#xBE;", "¾")
            If filterstring.IndexOf("&#xBF;") <> -1 Then filterstring = filterstring.Replace("&#xBF;", "¿")


            If filterstring.IndexOf("&#xC0;") <> -1 Then filterstring = filterstring.Replace("&#xC0;", "À")
            If filterstring.IndexOf("&#xC1;") <> -1 Then filterstring = filterstring.Replace("&#xC1;", "Á")
            If filterstring.IndexOf("&#xC2;") <> -1 Then filterstring = filterstring.Replace("&#xC2;", "Â")
            If filterstring.IndexOf("&#xC3;") <> -1 Then filterstring = filterstring.Replace("&#xC3;", "Ã")
            If filterstring.IndexOf("&#xC4;") <> -1 Then filterstring = filterstring.Replace("&#xC4;", "Ä")
            If filterstring.IndexOf("&#xC5;") <> -1 Then filterstring = filterstring.Replace("&#xC5;", "Å")
            If filterstring.IndexOf("&#xC6;") <> -1 Then filterstring = filterstring.Replace("&#xC6;", "Æ")
            If filterstring.IndexOf("&#xC7;") <> -1 Then filterstring = filterstring.Replace("&#xC7;", "Ç")
            If filterstring.IndexOf("&#xC8;") <> -1 Then filterstring = filterstring.Replace("&#xC8;", "È")
            If filterstring.IndexOf("&#xC9;") <> -1 Then filterstring = filterstring.Replace("&#xC9;", "É")
            If filterstring.IndexOf("&#xCA;") <> -1 Then filterstring = filterstring.Replace("&#xCA;", "Ê")
            If filterstring.IndexOf("&#xCB;") <> -1 Then filterstring = filterstring.Replace("&#xCB;", "Ë")
            If filterstring.IndexOf("&#xCC;") <> -1 Then filterstring = filterstring.Replace("&#xCC;", "Ì")
            If filterstring.IndexOf("&#xCD;") <> -1 Then filterstring = filterstring.Replace("&#xCD;", "Í")
            If filterstring.IndexOf("&#xCE;") <> -1 Then filterstring = filterstring.Replace("&#xCE;", "Î")
            If filterstring.IndexOf("&#xCF;") <> -1 Then filterstring = filterstring.Replace("&#xCF;", "Ï")


            If filterstring.IndexOf("&#xD0;") <> -1 Then filterstring = filterstring.Replace("&#xD0;", "Ð")
            If filterstring.IndexOf("&#xD1;") <> -1 Then filterstring = filterstring.Replace("&#xD1;", "Ñ")
            If filterstring.IndexOf("&#xD2;") <> -1 Then filterstring = filterstring.Replace("&#xD2;", "Ò")
            If filterstring.IndexOf("&#xD3;") <> -1 Then filterstring = filterstring.Replace("&#xD3;", "Ó")
            If filterstring.IndexOf("&#xD4;") <> -1 Then filterstring = filterstring.Replace("&#xD4;", "Ô")
            If filterstring.IndexOf("&#xD5;") <> -1 Then filterstring = filterstring.Replace("&#xD5;", "Õ")
            If filterstring.IndexOf("&#xD6;") <> -1 Then filterstring = filterstring.Replace("&#xD6;", "Ö")
            If filterstring.IndexOf("&#xD7;") <> -1 Then filterstring = filterstring.Replace("&#xD7;", "×")
            If filterstring.IndexOf("&#xD8;") <> -1 Then filterstring = filterstring.Replace("&#xD8;", "Ø")
            If filterstring.IndexOf("&#xD9;") <> -1 Then filterstring = filterstring.Replace("&#xD9;", "Ù")
            If filterstring.IndexOf("&#xDA;") <> -1 Then filterstring = filterstring.Replace("&#xDA;", "Ú")
            If filterstring.IndexOf("&#xDB;") <> -1 Then filterstring = filterstring.Replace("&#xDB;", "Û")
            If filterstring.IndexOf("&#xDC;") <> -1 Then filterstring = filterstring.Replace("&#xDC;", "Ü")
            If filterstring.IndexOf("&#xDD;") <> -1 Then filterstring = filterstring.Replace("&#xDD;", "Ý")
            If filterstring.IndexOf("&#xDE;") <> -1 Then filterstring = filterstring.Replace("&#xDE;", "Þ")
            If filterstring.IndexOf("&#xDF;") <> -1 Then filterstring = filterstring.Replace("&#xDF;", "ß")


            If filterstring.IndexOf("&#xE0;") <> -1 Then filterstring = filterstring.Replace("&#xE0;", "à")
            If filterstring.IndexOf("&#xE1;") <> -1 Then filterstring = filterstring.Replace("&#xE1;", "á")
            If filterstring.IndexOf("&#xE2;") <> -1 Then filterstring = filterstring.Replace("&#xE2;", "â")
            If filterstring.IndexOf("&#xE3;") <> -1 Then filterstring = filterstring.Replace("&#xE3;", "ã")
            If filterstring.IndexOf("&#xE4;") <> -1 Then filterstring = filterstring.Replace("&#xE4;", "ä")
            If filterstring.IndexOf("&#xE5;") <> -1 Then filterstring = filterstring.Replace("&#xE5;", "å")
            If filterstring.IndexOf("&#xE6;") <> -1 Then filterstring = filterstring.Replace("&#xE6;", "æ")
            If filterstring.IndexOf("&#xE7;") <> -1 Then filterstring = filterstring.Replace("&#xE7;", "ç")
            If filterstring.IndexOf("&#xE8;") <> -1 Then filterstring = filterstring.Replace("&#xE8;", "è")
            If filterstring.IndexOf("&#xE9;") <> -1 Then filterstring = filterstring.Replace("&#xE9;", "é")
            If filterstring.IndexOf("&#xEA;") <> -1 Then filterstring = filterstring.Replace("&#xEA;", "ê")
            If filterstring.IndexOf("&#xEB;") <> -1 Then filterstring = filterstring.Replace("&#xEB;", "ë")
            If filterstring.IndexOf("&#xEC;") <> -1 Then filterstring = filterstring.Replace("&#xEC;", "ì")
            If filterstring.IndexOf("&#xED;") <> -1 Then filterstring = filterstring.Replace("&#xED;", "í")
            If filterstring.IndexOf("&#xEE;") <> -1 Then filterstring = filterstring.Replace("&#xEE;", "î")
            If filterstring.IndexOf("&#xEF;") <> -1 Then filterstring = filterstring.Replace("&#xEF;", "ï")


            If filterstring.IndexOf("&#xF0;") <> -1 Then filterstring = filterstring.Replace("&#xF0;", "ð")
            If filterstring.IndexOf("&#xF1;") <> -1 Then filterstring = filterstring.Replace("&#xF1;", "ñ")
            If filterstring.IndexOf("&#xF2;") <> -1 Then filterstring = filterstring.Replace("&#xF2;", "ò")
            If filterstring.IndexOf("&#xF3;") <> -1 Then filterstring = filterstring.Replace("&#xF3;", "ó")
            If filterstring.IndexOf("&#xF4;") <> -1 Then filterstring = filterstring.Replace("&#xF4;", "ô")
            If filterstring.IndexOf("&#xF5;") <> -1 Then filterstring = filterstring.Replace("&#xF5;", "õ")
            If filterstring.IndexOf("&#xF6;") <> -1 Then filterstring = filterstring.Replace("&#xF6;", "ö")
            If filterstring.IndexOf("&#xF7;") <> -1 Then filterstring = filterstring.Replace("&#xF7;", "÷")
            If filterstring.IndexOf("&#xF8;") <> -1 Then filterstring = filterstring.Replace("&#xF8;", "ø")
            If filterstring.IndexOf("&#xF9;") <> -1 Then filterstring = filterstring.Replace("&#xF9;", "ù")
            If filterstring.IndexOf("&#xFA;") <> -1 Then filterstring = filterstring.Replace("&#xFA;", "ú")
            If filterstring.IndexOf("&#xFB;") <> -1 Then filterstring = filterstring.Replace("&#xFB;", "û")
            If filterstring.IndexOf("&#xFC;") <> -1 Then filterstring = filterstring.Replace("&#xFC;", "ü")
            If filterstring.IndexOf("&#xFD;") <> -1 Then filterstring = filterstring.Replace("&#xFD;", "ý")
            If filterstring.IndexOf("&#xFE;") <> -1 Then filterstring = filterstring.Replace("&#xFE;", "þ")
            If filterstring.IndexOf("&#xFF;") <> -1 Then filterstring = filterstring.Replace("&#xFF;", "ÿ")
            If filterstring.IndexOf("&oacute;") <> -1 Then filterstring = filterstring.Replace("&oacute;", "ó")
            If filterstring.IndexOf("&eacute;") <> -1 Then filterstring = filterstring.Replace("&eacute;", "é")
            '

            Return filterstring
        Catch
        Finally
        End Try
        Return "Error"
    End Function

    'Public Function InsertFileInformationTags(ByVal Entrada As String, ByVal Filename As String) As String
    '    Dim WorkingFileDetails As StreamDetails = Pref.Get_HdTags(Filename)
    '    Dim FileInfoString As String = "<movie>" & vbLf & "<fileinfo>" & vbLf & "<streamdetails>" & vbLf & "<video>" & vbLf

    '    If WorkingFileDetails.Video.Width <> Nothing Then FileInfoString &= "<width>" & WorkingFileDetails.Video.Width.Value & "</width>" & vbLf
    '    If WorkingFileDetails.Video.Height <> Nothing Then FileInfoString &= "<height>" & WorkingFileDetails.Video.Height.Value & "</height>" & vbLf
    '    If WorkingFileDetails.Video.Aspect <> Nothing Then FileInfoString &= "<aspect>" & WorkingFileDetails.Video.Aspect.Value & "</aspect>" & vbLf
    '    If WorkingFileDetails.Video.Codec <> Nothing Then FileInfoString &= "<codec>" & WorkingFileDetails.Video.Codec.Value & "</codec>" & vbLf
    '    If WorkingFileDetails.Video.FormatInfo <> Nothing Then FileInfoString &= "<format>" & WorkingFileDetails.Video.FormatInfo.Value & "</format>" & vbLf
    '    If WorkingFileDetails.Video.DurationInSeconds.Value <> Nothing Then FileInfoString &= "<durationinseconds>" & WorkingFileDetails.Video.DurationInSeconds.Value & "</durationinseconds>" & vbLf  'Utilities.cleanruntime(WorkingFileDetails.Video.DurationInSeconds.Value)
    '    If WorkingFileDetails.Video.Bitrate <> Nothing Then FileInfoString &= "<bitrate>" & WorkingFileDetails.Video.Bitrate.Value & "</bitrate>" & vbLf
    '    If WorkingFileDetails.Video.BitrateMode <> Nothing Then FileInfoString &= "<bitratemode>" & WorkingFileDetails.Video.BitrateMode.Value & "</bitratemode>" & vbLf
    '    If WorkingFileDetails.Video.BitrateMax <> Nothing Then FileInfoString &= "<bitratemax>" & WorkingFileDetails.Video.BitrateMax.Value & "</bitratemax>" & vbLf
    '    If WorkingFileDetails.Video.Container <> Nothing Then FileInfoString &= "<container>" & WorkingFileDetails.Video.Container.Value & "</container>" & vbLf
    '    If WorkingFileDetails.Video.CodecId <> Nothing Then FileInfoString &= "<codecid>" & WorkingFileDetails.Video.CodecId.Value & "</codecid>" & vbLf
    '    If WorkingFileDetails.Video.CodecInfo <> Nothing Then FileInfoString &= "<codecinfo>" & WorkingFileDetails.Video.CodecInfo.Value & "</codecinfo>" & vbLf
    '    If WorkingFileDetails.Video.ScanType <> Nothing Then FileInfoString &= "<scantype>" & WorkingFileDetails.Video.ScanType.Value & "</scantype>" & vbLf
    '    FileInfoString &= "</video>" & vbLf
    '    If WorkingFileDetails.Audio.Count > 0 Then
    '        For Each item In WorkingFileDetails.Audio
    '            FileInfoString &= "<audio>" & vbLf
    '            If item.Language <> Nothing Then FileInfoString &= "<language>" & item.Language.Value & "</language>" & vbLf
    '            If item.Codec <> Nothing Then FileInfoString &= "<codec>" & item.Codec.Value & "</codec>" & vbLf
    '            If item.Channels <> Nothing Then FileInfoString &= "<channels>" & item.Channels.Value & "</channels>" & vbLf
    '            If item.Bitrate <> Nothing Then FileInfoString &= "<bitrate>" & item.Bitrate.Value & "</bitrate>" & vbLf
    '            FileInfoString &= "</audio>" & vbLf
    '        Next
    '        If WorkingFileDetails.Subtitles.Count > 0 Then
    '            FileInfoString &= "<subtitle>" & vbLf
    '            For Each item In WorkingFileDetails.Subtitles
    '                If item.Language <> Nothing Then FileInfoString &= "<language>" & item.Language.Value & "</language>" & vbLf
    '            Next
    '            FileInfoString &= "</subtitle>" & vbLf
    '        End If
    '        FileInfoString &= "</streamdetails>" & vbLf & "</fileinfo>" & vbLf
    '    Else
    '        FileInfoString &= "</streamdetails>" & vbLf & "</fileinfo>" & vbLf
    '    End If
    '    FileInfoString &= "<playcount>0</playcount>" & vbLf
    '    FileInfoString &= "<createdate>" & Format(System.DateTime.Now, Pref.datePattern).ToString & "</createdate>" & vbLf
    '    Dim TempString As String = Entrada.Remove(0, Entrada.IndexOf("<id>"))
    '    TempString = FileInfoString & TempString
    '    TempString = TempString.Remove(TempString.LastIndexOf("</details>"), 10)
    '    TempString &= "</movie>"
    '    Return TempString
    'End Function
    
    Private Function AppendData(ByVal tag As String, ByVal data As String) As String
        If String.IsNullOrEmpty(data) Then Return ""
        Return tag & data & tag.Insert(1,"/") & vbCrLf
    End Function
#End Region

#Region "Misc.TVShows Routines"
    
    Public Function Clean_AddTVShowExtraFields(ByVal Entrada As String, ByVal Language As String, ByVal IMDB_ID As String) As String
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode
        Dim TvDBId As String = ""
        Dim Year As String = ""
        Dim count As Integer = 0
        Dim ExtraFields As Boolean = False
        Dim FinalString As String = "<tvshow>"

        m_xmld = New XmlDocument()
        m_xmld.LoadXml(Entrada)
        m_nodelist = m_xmld.SelectNodes("/details")
        For Each m_node In m_nodelist
            If m_node.HasChildNodes Then
                For Each node1 As XmlNode In m_node
                    If (node1.InnerText <> "") Then
                        If node1.Name.ToLower = "actor" Then
                            For Each node2 As XmlNode In node1
                                Select Case node2.Name.ToLower
                                    Case "name"
                                        If node2.InnerText <> "" Then FinalString &= node1.OuterXml.ToString
                                        Exit For
                                End Select
                            Next
                        Else
                            Select Case node1.Name.ToLower
                                Case "id"
                                    FinalString &= "<imdbid>" & IMDB_ID & "</imdbid>"
                                    'node1.InnerText = IMDB_ID
                                Case "premiered"
                                    FinalString &= "<year>" & node1.InnerText.Substring(0, 4) & "</year>"
                                Case "genre"
                                    If ExtraFields = False Then
                                        Dim sortorder As String = String.Empty
                                        If Form1.rbTvShSelectOrderDVD.Checked Then sortorder = "dvd"
                                        If Form1.rbTvShSelectOrderDefault.Checked Then sortorder = "default"
                                        FinalString &= "<top250>0</top250><season>-1</season><episode>-1</episode><displayseason>-1</displayseason><displayepisode>-1</displayepisode>" _
                                            & "<votes></votes><outline></outline><tagline></tagline><episodeactorsource>tvdb</episodeactorsource><tvshowactorsource>tvdb</tvshowactorsource>"
                                        FinalString &= "<sortorder>" & sortorder & "</sortorder><playcount>0</playcount><lastplayed></lastplayed><status></status>" _
                                            & "<code></code><language>" & Language & "</language><locked>0</locked><trailer></trailer>"
                                        ExtraFields = True
                                    End If
                                    count += 1
                                    If count > Pref.TvMaxGenres Then Continue For
                            End Select
                            FinalString &= node1.OuterXml.ToString
                        End If
                    End If
                Next
            End If
        Next
        FinalString &= "</tvshow>"
        Return FinalString
    End Function

    Public Function TVShowPosterandFanartDownload(ByVal FullString As String, ByVal ArtforDownload() As String, ByVal Path As String) As Boolean
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        m_xmld = New XmlDocument()
        m_xmld.LoadXml(FullString)
        m_nodelist = m_xmld.SelectNodes("/tvshow")
        Dim NodeChild As XmlNode
        Dim MoviePosterURL As String = ""
        Dim MovieFanartURL As String = ""
        Dim SeasonPosters(0) As String
        Dim Seasonall As String = Nothing

        If Pref.tvdlseasonthumbs = True Then
            Try
                For Each m_node In m_nodelist
                    For Each NodeChild In m_node.ChildNodes
                        If (NodeChild.Name.ToLower = "thumb") AndAlso NodeChild.Attributes.Count > 0 AndAlso (NodeChild.Attributes("type").Value = "season") Then
                            If SeasonPosters.Length < CInt(NodeChild.Attributes("season").Value) Then
                                ReDim Preserve SeasonPosters(NodeChild.Attributes("season").Value + 1)
                            End If
                            If CInt(NodeChild.Attributes("season").Value) >= 0 Then
                                If SeasonPosters(NodeChild.Attributes("season").Value) = Nothing Then
                                    SeasonPosters(NodeChild.Attributes("season").Value) = NodeChild.InnerText
                                End If
                            Else
                                If Seasonall = Nothing Then Seasonall = NodeChild.InnerText
                            End If
                        End If
                    Next
                Next
            Catch
            End Try
        End If
        Dim myWebClient As New System.Net.WebClient()
        If Pref.tvdlposter = True Then
            If Pref.postertype = "banner" Then
                Dim ImageFilename As String = Path & "\folder.jpg"
                If ArtforDownload(0) <> Nothing Then myWebClient.DownloadFile("http://thetvdb.com/banners/" & ArtforDownload(0), ImageFilename)
            ElseIf Pref.postertype = "poster" Then
                Dim ImageFilename As String = Path & "\folder.jpg"
                If ArtforDownload(1) <> Nothing Then myWebClient.DownloadFile("http://thetvdb.com/banners/" & ArtforDownload(1), ImageFilename)
            End If
        End If
        If Pref.tvdlfanart = True Then
            Dim ImageFilename As String = Path & "\fanart.jpg"
            If ArtforDownload(2) <> Nothing Then Movie.SaveFanartImageToCacheAndPath("http://thetvdb.com/banners/" & ArtforDownload(2), ImageFilename)
        End If

        If Pref.tvdlseasonthumbs = True Then
            For n As Integer = 0 To SeasonPosters.Length - 1
                Dim SeasonTemp As String = ""
                If n <= 9 Then
                    SeasonTemp = "0" & n.ToString
                Else
                    SeasonTemp = n.ToString
                End If
                Dim ImageFilename As String = Path & "\season" & SeasonTemp & ".tbn"
                If SeasonPosters(n) <> Nothing Then
                    myWebClient.DownloadFile(SeasonPosters(n), ImageFilename)
                    If n = 0 Then
                        File.Copy(ImageFilename, Path & "\season-specials.tbn", True)
                    End If
                End If
            Next
            Dim ImageFilename1 As String = Path & "\season-all.tbn"
            If Seasonall <> Nothing Then myWebClient.DownloadFile(Seasonall, ImageFilename1)
        End If
        Return True
    End Function

    Public Function InsertFileEpisodeInformationTags(ByVal Entrada() As String, ByVal Filename As String) As String
        Dim WorkingFileDetails As New StreamDetails
        Try
            Dim wkingdetails As StreamDetails = Pref.Get_HdTags(Filename)
            If Not IsNothing(wkingdetails) Then WorkingFileDetails = wkingdetails
        Catch
            Dim Something As String = Nothing
        End Try
        
        Dim FileInfoString As String = ""
        Dim TempString As String = ""
        Try
            For n As Integer = 0 To Entrada.Length - 1
                FileInfoString &= "<episodedetails>" & vbLf & "<fileinfo>" & vbLf & "<streamdetails>" & vbLf & "<video>" & vbLf

                FileInfoString &= AppendData("<width>"              , WorkingFileDetails.Video.Width.Value)
                FileInfoString &= AppendData("<height>"             , WorkingFileDetails.Video.Height.Value)
                FileInfoString &= AppendData("<aspect>"             , WorkingFileDetails.Video.Aspect.Value)
                FileInfoString &= AppendData("<codec>"              , WorkingFileDetails.Video.Codec.Value)
                FileInfoString &= AppendData("<format>"             , WorkingFileDetails.Video.FormatInfo.Value)
                FileInfoString &= AppendData("<durationinseconds>"  , WorkingFileDetails.Video.DurationInSeconds.Value)
                FileInfoString &= AppendData("<bitrate>"            , WorkingFileDetails.Video.Bitrate.Value)
                FileInfoString &= AppendData("<bitratemode>"        , WorkingFileDetails.Video.BitrateMode.Value)
                FileInfoString &= AppendData("<bitratemax>"         , WorkingFileDetails.Video.BitrateMax.Value)
                FileInfoString &= AppendData("<container>"          , WorkingFileDetails.Video.Container.Value)
                FileInfoString &= AppendData("<codecid>"            , WorkingFileDetails.Video.CodecId.Value)
                FileInfoString &= AppendData("<codecinfo>"          , WorkingFileDetails.Video.CodecInfo.Value)
                FileInfoString &= AppendData("<scantype>"           , WorkingFileDetails.Video.ScanType.Value)
                FileInfoString &= AppendData("<NumVideoBits>"       , WorkingFileDetails.Video.NumVideoBits)
                FileInfoString &= "</video>" & vbLf
                If WorkingFileDetails.Audio.Count > 0 Then
                    For Each item In WorkingFileDetails.Audio
                        FileInfoString &= "<audio>" & vbLf
                        FileInfoString &= AppendData("<language>"   , item.Language.Value)
                        FileInfoString &= AppendData("<codec>"      , item.Codec.Value)
                        FileInfoString &= AppendData("<channels>"   , item.Channels.Value)
                        FileInfoString &= AppendData("<bitrate>"    , item.Bitrate.Value)
                        FileInfoString &= "</audio>" & vbLf
                    Next
                    If WorkingFileDetails.Subtitles.Count > 0 Then
                        FileInfoString &= "<subtitle>" & vbLf
                        For Each item In WorkingFileDetails.Subtitles
                            FileInfoString &= AppendData("<language>"   , item.Language.Value)
                        Next
                        FileInfoString &= "</subtitle>" & vbLf
                    End If
                End If
                FileInfoString &= "</streamdetails>" & vbLf & "</fileinfo>" & vbLf
                FileInfoString &= AppendData("<createdate>" , Format(System.DateTime.Now, Pref.datePattern).ToString)
                TempString = ""
                TempString = Entrada(n).Substring(0, Entrada(n).IndexOf("</details>"))
                TempString = TempString.Replace("<details>", "")
                TempString = FileInfoString & TempString & "</episodedetails>"
                If TempString.Contains("<votes>") Then TempString = TempString.Replace("votes>", "ratingcount>")
                If Entrada.Length > 1 Then TempString &= vbLf
                FileInfoString = TempString
            Next
        Catch
            Return ""
        End Try
        If Entrada.Length > 1 Then TempString = "<multiepisodenfo>" & vbLf & TempString & vbLf & "</multiepisodenfo>"
        Return TempString
    End Function

    Public Function ProcessEpisodeFile(ByVal Entrada As String, ByVal HowManyEpisodes As Integer) As List(Of TvEpisode)
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        episodeXMLinformation.Clear()
        m_xmld = New XmlDocument()
        m_xmld.LoadXml(Entrada)
        If HowManyEpisodes > 1 Then
            m_nodelist = m_xmld.SelectNodes("/multiepisodenfo/episodedetails")
        Else
            m_nodelist = m_xmld.SelectNodes("/episodedetails")
        End If
        Dim NodeChild As XmlNode
        Dim Nodechild1 As XmlNode
        Dim Counter As Integer = 0

        For Each m_node In m_nodelist
            Dim TempXMLEpisode1 As New TvEpisode
            TempXMLEpisode1.Aired.Value = String.Empty
            TempXMLEpisode1.Credits.Value = String.Empty
            TempXMLEpisode1.Director.Value = String.Empty
            TempXMLEpisode1.Episode.Value = String.Empty
            TempXMLEpisode1.Genre.Value = String.Empty
            TempXMLEpisode1.Plot.Value = String.Empty
            TempXMLEpisode1.Rating.Value = String.Empty
            TempXMLEpisode1.Votes.Value = String.Empty
            TempXMLEpisode1.Season.Value = String.Empty
            TempXMLEpisode1.Thumbnail.FileName = String.Empty
            TempXMLEpisode1.Thumbnail.Url = String.empty
            TempXMLEpisode1.Title.Value = String.Empty
            TempXMLEpisode1.ShowId.Value = String.Empty 
            TempXMLEpisode1.UniqueId.Value = String.Empty 
            TempXMLEpisode1.ListActors.Clear()
            TempXMLEpisode1.StreamDetails.Audio.Clear()
            For Each NodeChild In m_node.ChildNodes
                Select Case NodeChild.Name.ToLower
                    Case "aired"
                        TempXMLEpisode1.Aired.Value = NodeChild.InnerText
                    Case "credits"
                        If TempXMLEpisode1.Credits.Value Is String.Empty Then
                            TempXMLEpisode1.Credits.Value = NodeChild.InnerText
                        Else
                            TempXMLEpisode1.Credits.Value &= " / " & NodeChild.InnerText
                        End If
                    Case "director"
                        TempXMLEpisode1.Director.Value = NodeChild.InnerText
                    Case "genre"
                        If Not Counter = Pref.TvMaxGenres Then
                            If TempXMLEpisode1.Genre.Value Is String.Empty Then
                                TempXMLEpisode1.Genre.Value = NodeChild.InnerText
                                Counter += 1
                            Else
                                TempXMLEpisode1.Genre.Value &= " / " & NodeChild.InnerText
                                Counter += 1
                            End If
                        End If
                    Case "plot"
                        TempXMLEpisode1.Plot.Value = NodeChild.InnerText
                    Case "rating"
                        TempXMLEpisode1.Rating.Value = NodeChild.InnerText
                    Case "ratingcount"  'from TVDb
                        TempXMLEpisode1.Votes.Value = NodeChild.InnerText
                    'Case "votes"        'from IMDb
                        'TempXMLEpisode1.Votes.Value = NodeChild.InnerText
                    Case "thumb"
                        TempXMLEpisode1.Thumbnail.FileName = NodeChild.InnerText
                        TempXMLEpisode1.Thumbnail.Url = NodeChild.InnerText 
                    Case "title"
                        TempXMLEpisode1.Title.Value = NodeChild.InnerText.TrimEnd
                    Case "season"
                        TempXMLEpisode1.Season.Value = NodeChild.InnerText
                    Case "episode"
                        TempXMLEpisode1.Episode.Value = NodeChild.InnerText
                    Case "uniqueid"
                        TempXMLEpisode1.UniqueId.Value = NodeChild.InnerText 
                    Case "seriesid"
                        TempXMLEpisode1.ShowId.Value = NodeChild.InnerText
                    Case "actor"
                        Dim newActor As New Media_Companion.Actor
                        For Each Nodechild1 In NodeChild.ChildNodes
                            Select Case Nodechild1.Name
                                Case "name"
                                    newActor.Name.Value = Nodechild1.InnerText
                                Case "role"
                                    newActor.Role.Value = Nodechild1.InnerText
                                Case "thumb"
                                    newActor.Thumb.Value = Nodechild1.InnerText
                                Case "actorid"
                            End Select
                        Next
                        If newActor.Name.Value <> Nothing And newActor.Name.Value.Length > 1 Then
                            TempXMLEpisode1.ListActors.Add(newActor)
                        End If
                    Case "fileinfo"
                        Dim detail2 As XmlNode = Nothing
                        For Each detail2 In Nodechild.ChildNodes
                            Select Case detail2.Name
                                Case "streamdetails"
                                    Dim detail As XmlNode = Nothing
                                    For Each detail In detail2.ChildNodes
                                        Select Case detail.Name
                                            Case "video"
                                                Dim videodetails As XmlNode = Nothing
                                                For Each videodetails In detail.ChildNodes
                                                    Select Case videodetails.Name
                                                        Case "width"
                                                            TempXMLEpisode1.StreamDetails.Video.Width.Value = videodetails.InnerText
                                                        Case "height"
                                                            TempXMLEpisode1.StreamDetails.Video.Height.Value = videodetails.InnerText
                                                        Case "aspect"
                                                            TempXMLEpisode1.StreamDetails.Video.Aspect.Value = videodetails.InnerText
                                                        Case "codec"
                                                            TempXMLEpisode1.StreamDetails.Video.Codec.Value = videodetails.InnerText
                                                        Case "formatinfo"
                                                            TempXMLEpisode1.StreamDetails.Video.FormatInfo.Value = videodetails.InnerText
                                                        Case "durationinseconds"
                                                            TempXMLEpisode1.StreamDetails.Video.DurationInSeconds.Value = videodetails.InnerText
                                                        Case "bitrate"
                                                            TempXMLEpisode1.StreamDetails.Video.Bitrate.Value = videodetails.InnerText
                                                        Case "bitratemode"
                                                            TempXMLEpisode1.StreamDetails.Video.BitrateMode.Value = videodetails.InnerText
                                                        Case "bitratemax"
                                                            TempXMLEpisode1.StreamDetails.Video.BitrateMax.Value = videodetails.InnerText
                                                        Case "container"
                                                            TempXMLEpisode1.StreamDetails.Video.Container.Value = videodetails.InnerText
                                                        Case "codecid"
                                                            TempXMLEpisode1.StreamDetails.Video.CodecId.Value = videodetails.InnerText
                                                        Case "codecidinfo"
                                                            TempXMLEpisode1.StreamDetails.Video.CodecInfo.Value = videodetails.InnerText
                                                        Case "scantype"
                                                            TempXMLEpisode1.StreamDetails.Video.ScanType.Value = videodetails.InnerText
                                                        Case "format"
                                                            TempXMLEpisode1.StreamDetails.Video.FormatInfo.Value = videodetails.InnerText 
                                                    End Select
                                                Next
                                            Case "audio"
                                                Dim audiodetails As XmlNode = Nothing
                                                Dim audio As New AudioDetails
                                                For Each audiodetails In detail.ChildNodes
                                                    Select Case audiodetails.Name
                                                        Case "language"
                                                            audio.Language.Value = audiodetails.InnerText
                                                        Case "DefaultTrack"
                                                            audio.DefaultTrack.Value = audiodetails.InnerText
                                                        Case "codec"
                                                            audio.Codec.Value = audiodetails.InnerText
                                                        Case "channels"
                                                            audio.Channels.Value = audiodetails.InnerText
                                                        Case "bitrate"
                                                            audio.Bitrate.Value = audiodetails.InnerText
                                                    End Select
                                                Next
                                                TempXMLEpisode1.StreamDetails.Audio.Add(audio)
                                            Case "subtitle"
                                                Dim subsdetails As XmlNode = Nothing
                                                For Each subsdetails In detail.ChildNodes
                                                    Select Case subsdetails.Name
                                                        Case "language"
                                                            Dim sublang As New SubtitleDetails
                                                            sublang.Language.Value = subsdetails.InnerText
                                                            TempXMLEpisode1.StreamDetails.Subtitles.Add(sublang)
                                                    End Select
                                                Next
                                        End Select
                                    Next
                                    'newtvepisode.Details = newfilenfo
                            End Select
                        Next
                End Select
            Next
            If Not TempXMLEpisode1.StreamDetails.Video.DurationInSeconds.Value Is Nothing Then
                Dim tempstring = TempXMLEpisode1.StreamDetails.Video.DurationInSeconds.Value
                If Pref.intruntime Then
                    TempXMLEpisode1.Runtime.Value = Math.Round(tempstring / 60).ToString
                Else
                    TempXMLEpisode1.Runtime.Value = Math.Round(tempstring / 60).ToString & " min"
                End If
            End If
            episodeXMLinformation.Add(TempXMLEpisode1)
        Next
        Dim Teste As New List(Of TvEpisode)
        'Teste = episodeXMLinformation.ConvertAll(New Converter(Of TvEpisode, TvEpisode)(AddressOf NeededConversion))
        Return episodeXMLinformation    'Teste
    End Function
    
    Public Function XBMCScrape_TVShow_EpisodeDetails(ByVal TVDBId As String, ByVal SortOrder As String, ByVal EpisodeArray As List(Of TvEpisode), ByVal Language As String) As List(Of TvEpisode)
        episodeInformation.Clear()
        Dim EpisodeInfoContent(EpisodeArray.Count - 1) As String
        Try
            For n As Integer = 0 To EpisodeArray.Count - 1
                EpisodeArray(n).Season.Value = CInt(EpisodeArray(n).Season.Value)
                EpisodeArray(n).Episode.Value = CInt(EpisodeArray(n).Episode.Value)
                TempXMLEpisode.NfoFilePath = EpisodeArray(n).VideoFilePath.Substring(0, EpisodeArray(n).VideoFilePath.LastIndexOf(".")) & ".nfo"
                TempXMLEpisode.Episode.Value = EpisodeArray(n).Episode.Value
                TempXMLEpisode.Season.Value = EpisodeArray(n).Season.Value
                TempXMLEpisode.MediaExtension = EpisodeArray(n).MediaExtension
                TempXMLEpisode.ShowId.Value = TVDBId 
                TempXMLEpisode.PlayCount.Value = "0"
                ParametersForScraper(1) = TVDBId
                ParametersForScraper(3) = "http://www.thetvdb.com/api/1D62F2F90030C444/series/" & TVDBId & "/" & Language & ".xml"
                ParametersForScraper(4) = Nothing
                Try
                    For x As Integer = 0 To 20
                        ParametersForScraper(7) = Utilities.DownloadTextFiles("http://www.thetvdb.com/api/1D62F2F90030C444/series/" & TVDBId & "/" & SortOrder & "/" & EpisodeArray(n).Season.Value & "/" & EpisodeArray(n).Episode.Value & "/" & Language & ".xml")
                        If ParametersForScraper(7).Substring(0, 5).ToLower = "<?xml" Then
                            Exit For
                        Else
                            If x = 20 Then
                                episodeInformation.Clear()
                                Return episodeInformation
                                Exit Function
                            End If
                        End If
                    Next
                    
                    EpisodeInfoContent(n) = DoScrape("metadata.tvdb.com", "GetEpisodeDetails", ParametersForScraper, False)
                    
                    'Code below is to counter no Absolute order information from TVDB, where
                    'the Season and Episode tags would be scraped empty.
                    If EpisodeInfoContent(n).Contains("<season></season>") Then
                        EpisodeInfoContent(n) = EpisodeInfoContent(n).Replace("<season></season>"   , AppendData("<season>" , EpisodeArray(n).Season.Value  ))
                    End If
                    If EpisodeInfoContent(n).Contains("<episode></episode>") Then
                        EpisodeInfoContent(n) = EpisodeInfoContent(n).Replace("<episode></episode>" , AppendData("<episode>", EpisodeArray(n).Episode.Value ))
                    End If
                    'End Fix
                Catch
                    episodeInformation.Clear()
                    Return episodeInformation
                    Exit Function
                End Try
            Next
        Catch
            episodeInformation.Clear()
            Return episodeInformation
            Exit Function
        End Try
        Try
            FinalScrapResult = InsertFileEpisodeInformationTags(EpisodeInfoContent, EpisodeArray(0).VideoFilePath)
            episodeInformation = ProcessEpisodeFile(FinalScrapResult, EpisodeArray.Count)
            Return episodeInformation
        Catch
            episodeInformation.Clear()
            Return episodeInformation
            Exit Function
        End Try
    End Function

    Public Function XBMCScrape_TVShow_General_Info(ByVal Scraper As String, ByVal TVShowid As String, ByVal Language As String, ByVal Path As String) As String
        Try
            Dim Parameters(2) As String
            Dim ParametersForScraper(9) As String
            ParametersForScraper(0) = Utilities.DownloadTextFiles("http://www.thetvdb.com/api/1D62F2F90030C444/series/" & TVShowid & "/" & Language & ".xml")
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode
            m_xmld = New XmlDocument()
            m_xmld.LoadXml(ParametersForScraper(0))
            m_nodelist = m_xmld.SelectNodes("/Data/Series")
            Dim ArtforDownload(2) As String
            For Each m_node In m_nodelist
                For Each NodeChild In m_node.ChildNodes
                    If NodeChild.Name.ToLower = "banner" Then
                        ArtforDownload(0) = NodeChild.InnerText
                    ElseIf NodeChild.Name.ToLower = "poster" Then
                        ArtforDownload(1) = NodeChild.InnerText
                    ElseIf NodeChild.Name.ToLower = "fanart" Then
                        ArtforDownload(2) = NodeChild.InnerText
                    End If
                Next
            Next
            Dim IMDB_ID As String = ParametersForScraper(0).Substring(ParametersForScraper(0).ToLower.IndexOf("<imdb_id>") + 9, (ParametersForScraper(0).ToLower.LastIndexOf("</imdb_id>") - ParametersForScraper(0).ToLower.IndexOf("<imdb_id>") - 9))
            ParametersForScraper(1) = TVShowid
            ParametersForScraper(9) = TVShowid
            Parameters(0) = "http://www.thetvdb.com/api/1D62F2F90030C444/series/" & TVShowid & "/" & Language & ".xml"
            Parameters(1) = "http://www.thetvdb.com/api/1D62F2F90030C444/series/" & TVShowid & "/banners.xml"
            Parameters(2) = "http://www.thetvdb.com/api/1D62F2F90030C444/series/" & TVShowid & "/actors.xml"
            For n As Integer = 0 To 2
                ParametersForScraper(4) = Parameters(n)
                Parameters(n) = DoScrape(Scraper, "GetDetails", ParametersForScraper, True, True, 5)
            Next
            Parameters(0) = Parameters(0).Substring(0, Parameters(0).LastIndexOf("<fanart url="))
            Parameters(1) = Parameters(1).Substring(Parameters(1).IndexOf("<fanart url="), (Parameters(1).LastIndexOf("</details>") - Parameters(1).IndexOf("<fanart url=")))
            Parameters(2) = Parameters(2).Substring(Parameters(2).IndexOf("<actor>"), (Parameters(2).LastIndexOf("<fanart url=") - Parameters(2).IndexOf("<actor>"))) & "</details>"
            Dim Temp As String = Parameters(0) & Parameters(1) & Parameters(2)
            Temp = Clean_AddTVShowExtraFields(Temp, Language, IMDB_ID)
            Path = Path.Substring(0, Path.LastIndexOf("\"))
            Dim Downloads As Boolean = TVShowPosterandFanartDownload(Temp, ArtforDownload, Path)
            Return Temp
        Catch
            Return "error"
        End Try
    End Function

#End Region


#Region "nfoFileFunctions"

    Public Function CreateMovieNfo(ByVal Filename As String, ByVal FileContent As String, Optional ByVal scraper As String = "", Optional ByRef nfoFileandPath As String = "") As Boolean
        Try
            Dim ExtensionPosition As Integer = Filename.LastIndexOf(".")
            nfoFileandPath = Filename.Remove(ExtensionPosition, (Filename.Length - ExtensionPosition)) & ".nfo"
            Dim doc As New XmlDocument
            doc.LoadXml(FileContent)
            Dim xmlproc As XmlDeclaration
            xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
            Dim root As XmlElement = doc.DocumentElement
            doc.InsertBefore(xmlproc, root)
            WorkingWithNfoFiles.SaveXMLDoc(doc, nfoFileandPath)
            
            If Filename.ToLower.Contains("tvshow.nfo") Then Return True

            ' load nfo file to clean
            Dim fmd As FullMovieDetails = WorkingWithNfoFiles.mov_NfoLoadFull(nfoFileandPath)

            If fmd.fullmoviebody.SetName = "" Then fmd.fullmoviebody.SetName = "-None-"
            If fmd.fullmoviebody.top250  = "" Then fmd.fullmoviebody.top250  ="0"
            If fmd.fullmoviebody.outline = "" Then fmd.fullmoviebody.outline = fmd.fullmoviebody.plot ' Outline lonely 
            fmd.fullmoviebody.outline = fmd.fullmoviebody.outline.Replace("\", "")
            fmd.fullmoviebody.plot = fmd.fullmoviebody.plot.Replace("\", "")
                        
            'if scraped by XBMC TMDB, one poster is allocated.  Move this to Frodoposter.
            If scraper <> "" Then
                If fmd.listthumbs.Count = 1 Then fmd.frodoPosterThumbs.Add(New FrodoPosterThumb("poster",fmd.listthumbs(0)))
                fmd.listthumbs.Clear()
            End If

            ' save to make sure additional features like saving actor thumbnails takes place
            'WorkingWithNfoFiles.mov_NfoSave(nfoFileandPath, fmd, True)
            Movie.SaveNFO(nfoFileandPath, fmd)
            Return True
        Catch
            Return False
        End Try
    End Function

#End Region
    

    'Date  : Nov11 
    'By    : AnotherPhil 
    'Blurb : This uses a revised hdtrailers.xml copied from metadata.common.hdtrailers.net
    '        Main changes
    '           - Scraping from http://www.hd-trailers.net/movie/movie-name instead of http://www.hd-trailers.net/blog/?s=&quot;movie-name_(Theatrical_Trailer)
    '           - Adds RegExs for the following sources:
    '               - http://videos.hd-trailers.net
    '               - http://pdl.stream
    '           - Fixed RegExs for:
    '               - http://playlist.yahoo.com (working versions copied from here: http://code.google.com/p/xbmchuscraper/source/browse/trunk/HDTrailersForEMM/hdtrailers.xml?spec=svn53&r=53) 
    '
    Public Function MC_Scraper_Get_HD_Trailer_URL( ByVal Resolution As String, ByVal MovieTitle As String ) As String

        Dim theScraper    As Scraper = ChooseScraper("metadata.mc.hdtrailers.net")
        Dim funcName      As String  = "GetHDTrailersnet" & Resolution & "p"
        Dim funcParams(0) As String  

        'Prep movie title for HD-Trailers.net
        MovieTitle = Regex.Replace( MovieTitle, "[ ,']"    , "-"   )
        MovieTitle = Regex.Replace( MovieTitle, "[?!.;:@]" , ""    )
        MovieTitle = Regex.Replace( MovieTitle, "[&]"      , "and" )
        MovieTitle = Regex.Replace( MovieTitle, "-{2,}"    , "-"   )

        funcParams(0) = MovieTitle
        Dim url As String = ""
        Try
            url = ScraperQuery.ExecuteQuery(theScraper, funcName, funcParams)
        Catch 
            url = ""
        End Try

        Dim match As Match = Regex.Match(url, "<trailer>(.*?)</trailer>")

        If match.Groups.Count = 2 Then
	        url = match.Groups(1).Value
            url = Replace( url, "&amp;", "&" )
        Else
            url = ""
        End If

        Return url
    End Function

End Module
