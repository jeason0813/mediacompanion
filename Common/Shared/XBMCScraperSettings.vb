Imports System.IO
Imports System.xml

Public Class XBMCScraperSettings

    Public Shared Sub Save_XBMC_TMDB_Scraper_Config(ByVal KeyToBeChanged As String, ByVal ChangeValue As String)
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        m_xmld = New XmlDocument()
        m_xmld.Load(Path.Combine(Utilities.applicationPath, "assets\scrapers\metadata.themoviedb.org\resources\settings.xml"))
        m_nodelist = m_xmld.SelectNodes("/settings")
        Dim NodeChild As XmlNode
        Dim MoviePosterURL As String = ""
        Dim MovieFanartURL As String = ""
        Dim SeasonPosters(0) As String
        Dim Seasonall As String = Nothing

        Try
            For Each m_node In m_nodelist
                For Each NodeChild In m_node.ChildNodes
                    If (NodeChild.Name.ToLower = "setting") Then
                        If NodeChild.Attributes.Count > 0 Then
                            Try
                                If KeyToBeChanged.ToLower = NodeChild.Attributes("id").Value.ToLower Then
                                    Select Case KeyToBeChanged
                                        Case "ratings"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "fanart"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "trailerq"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "language"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "tmdbcertcountry"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                    End Select
                                End If
                            Catch
                                'empty node
                            End Try
                        End If
                    End If
                Next
            Next
        Catch
        End Try
        m_xmld.Save(Path.Combine(Utilities.applicationPath, "assets\scrapers\metadata.themoviedb.org\resources\settings.xml"))
    End Sub

    Public Shared Sub Save_XBMC_TVDB_Scraper_Config(ByVal KeyToBeChanged As String, ByVal ChangeValue As String)
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        m_xmld = New XmlDocument()
        m_xmld.Load(Path.Combine(Utilities.applicationPath, "assets\scrapers\metadata.tvdb.com\resources\settings.xml"))
        m_nodelist = m_xmld.SelectNodes("/settings")
        Dim NodeChild As XmlNode
        Dim MoviePosterURL As String = ""
        Dim MovieFanartURL As String = ""
        Dim SeasonPosters(0) As String
        Dim Seasonall As String = Nothing

        Try
            For Each m_node In m_nodelist
                For Each NodeChild In m_node.ChildNodes
                    If (NodeChild.Name.ToLower = "setting") Then
                        If NodeChild.Attributes.Count > 0 Then
                            If NodeChild.Attributes.ItemOf("type").Value = "sep" Then Continue For
                            Try
                                If KeyToBeChanged.ToLower = NodeChild.Attributes("id").Value.ToLower Then

                                    Select Case KeyToBeChanged 'NodeChild.Attributes("id").Value.ToLower
                                        Case "dvdorder"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "absolutenumber"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "fanart"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "posters"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "language"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "ratings"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                        Case "fallback"
                                            NodeChild.Attributes("default").Value = ChangeValue
                                    End Select
                                End If
                            Catch
                                'empty node
                            End Try
                        End If
                    End If
                Next
            Next
        Catch
        End Try
        m_xmld.Save(Path.Combine(Utilities.applicationPath, "assets\scrapers\metadata.tvdb.com\resources\settings.xml"))
    End Sub

    Public Shared Sub Read_XBMC_TVDB_Scraper_Config()
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        m_xmld = New XmlDocument()
        m_xmld.Load(Path.Combine(Utilities.applicationPath, "assets\scrapers\metadata.tvdb.com\resources\settings.xml"))
        m_nodelist = m_xmld.SelectNodes("/settings")
        Dim NodeChild As XmlNode
        Dim MoviePosterURL As String = ""
        Dim MovieFanartURL As String = ""
        Dim SeasonPosters(0) As String
        Dim Seasonall As String = Nothing
        Pref.XBMCTVDbLanguageLB.Clear()
        
        Try
            For Each m_node In m_nodelist
                For Each NodeChild In m_node.ChildNodes
                    If (NodeChild.Name.ToLower = "setting") Then
                        If NodeChild.Attributes.Count > 0 Then
                            If NodeChild.Attributes.ItemOf("type").Value = "sep" Then Continue For
                            Try
                                Select Case NodeChild.Attributes("id").Value.ToLower
                                    Case "dvdorder"
                                        Dim Test As Boolean = NodeChild.Attributes("default").Value
                                        Pref.XBMCTVDbDvdOrder = Test
                                    Case "absolutenumber"
                                        Dim Test As Boolean = NodeChild.Attributes("default").Value
                                        Pref.XBMCTVDbAbsoluteNumber = Test
                                    Case "fanart"
                                        Dim Test As Boolean = NodeChild.Attributes("default").Value
                                        Pref.XBMCTVDbFanart = Test
                                    Case "posters"
                                        Dim Test As Boolean = NodeChild.Attributes("default").Value
                                        Pref.XBMCTVDbPoster = Test
                                    Case "language"
                                        Dim Test As String = NodeChild.Attributes("default").Value
                                        Dim AllValues As String = NodeChild.Attributes("values").Value
                                        Dim GetOut As Boolean = False
                                        Do
                                            Dim Position As Integer = AllValues.LastIndexOf("|")
                                            Dim TempValue As String = ""
                                            If Position = -1 Then
                                                TempValue = Trim(AllValues.Substring(0, AllValues.Length))
                                                GetOut = True
                                            Else
                                                TempValue = Trim(AllValues.Substring(Position + 1, (AllValues.Length - Position - 1)))
                                                AllValues = AllValues.Remove(Position, (AllValues.Length - Position))
                                            End If
                                            Pref.XBMCTVDbLanguageLB.Add(TempValue)
                                        Loop Until GetOut = True
                                        Pref.XBMCTVDbLanguage = Test
                                    Case "ratings"
                                        Dim Test As String = NodeChild.Attributes("default").Value
                                        Pref.XBMCTVDbRatings = Test
                                    Case "fallback"
                                        Dim Test As Boolean = NodeChild.Attributes("default").Value
                                        Pref.XBMCTVDbfallback = Test
                                End Select
                            Catch
                                'empty node
                            End Try
                        End If
                    End If
                Next
            Next
        Catch
        End Try
    End Sub

    Public Shared Sub Read_XBMC_TMDB_Scraper_Config()
        Dim m_xmld As XmlDocument
        Dim m_nodelist As XmlNodeList
        Dim m_node As XmlNode

        m_xmld = New XmlDocument()
        m_xmld.Load(Path.Combine(Utilities.applicationPath, "assets\scrapers\metadata.themoviedb.org\resources\settings.xml"))
        m_nodelist = m_xmld.SelectNodes("/settings")
        Dim NodeChild As XmlNode
        Dim MoviePosterURL As String = ""
        Dim MovieFanartURL As String = ""
        Dim SeasonPosters(0) As String
        Dim Seasonall As String = Nothing
        Pref.XbmcTmdbScraperTrailerQLB.Clear()
        Pref.XbmcTmdbScraperLanguageLB.Clear()
        Pref.XbmcTmdbScraperCertCountryLB.Clear()
        
        Try
            For Each m_node In m_nodelist
                For Each NodeChild In m_node.ChildNodes
                    If (NodeChild.Name.ToLower = "setting") Then
                        If NodeChild.Attributes.Count > 0 Then
                            Try
                                Select Case NodeChild.Attributes("id").Value.ToLower
                                    Case "fanart"
                                        Pref.XbmcTmdbScraperFanart = NodeChild.Attributes("default").Value
                                    Case "trailerq"
                                        Dim Test As String = NodeChild.Attributes("default").Value
                                        Dim AllValues As String = NodeChild.Attributes("values").Value
                                        Dim splitvalues() As String = AllValues.Split("|")
                                        For Each thisvalue In splitvalues
                                            Pref.XbmcTmdbScraperTrailerQLB.Add(thisvalue)
                                        Next
                                        Pref.XbmcTmdbScraperTrailerQ = Test
                                    Case "language"
                                        Dim Test As String = NodeChild.Attributes("default").Value
                                        Dim AllValues As String = NodeChild.Attributes("values").Value
                                        Dim splitvalues() As String = AllValues.Split("|")
                                        For Each thisvalue In splitvalues
                                            Pref.XbmcTmdbScraperLanguageLB.Add(thisvalue)
                                        Next
                                        Pref.XbmcTmdbScraperLanguage = Test
                                    Case "ratings"
                                        Dim Test As String = NodeChild.Attributes("default").Value
                                        Pref.XbmcTmdbScraperRatings = Test
                                    Case "tmdbcertcountry"
                                        Dim Test As String = NodeChild.Attributes("default").Value
                                        Dim AllValues As String = NodeChild.Attributes("values").Value
                                        Dim splitvalues() As String = AllValues.Split("|")
                                        For Each thisvalue In splitvalues
                                            Pref.XbmcTmdbScraperCertCountryLB.Add(thisvalue)
                                        Next
                                        Pref.XbmcTmdbScraperCertCountry = Test
                                End Select
                            Catch
                                'empty node
                            End Try
                        End If
                    End If
                Next
            Next
        Catch
        End Try
    End Sub

End Class
