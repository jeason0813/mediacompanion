'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Xml

Public Class Profiles

    Property StartupProfile     As String
    Property DefaultProfile     As String
    Property WorkingProfileName As String
    Property ProfileList        As New List(Of ListOfProfiles)


    Public Sub Load
        ProfileList.Clear()
        Dim applicationpath As String = Pref.applicationPath 
        Dim profilepath As String = Path.Combine(applicationpath, "settings")

        profilepath = Path.Combine(profilepath, "profile.xml")

        If File.Exists(profilepath) Then
            Dim xmlDoc As New XmlDocument
            xmlDoc.Load(profilepath)
            If xmlDoc.DocumentElement.Name = "profile" Then
                For Each thisresult As XmlNode In xmlDoc("profile")
                    Select Case thisresult.Name
                        Case "default"
                            DefaultProfile = thisresult.InnerText
                        Case "startup"
                            StartupProfile = thisresult.InnerText
                        Case "profiledetails"
                            Dim currentprofile As New ListOfProfiles
                            For Each result As XmlNode In thisresult.ChildNodes
                                Dim t As Integer = result.InnerText.ToString.ToLower.IndexOf("\s")
                                Select Case result.name
                                    Case "actorcache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.ActorCache = applicationPath & s
                                    Case "directorcache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.DirectorCache = applicationPath & s
                                    Case "config"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.Config = applicationPath & s
                                    Case "moviecache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.MovieCache = applicationPath & s
                                    Case "profilename"
                                        currentprofile.ProfileName = result.InnerText
                                    Case "regex"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.RegExList = applicationPath & s
                                    Case "genres"
                                        Dim s As String = ""
                                        If result.InnerText = "" Then 
                                            s = "\settings\genres.txt"  'incase missing from existing profile.xml
                                        Else
                                            s = result.innertext.ToString.Substring(t)
                                        End If
                                        currentprofile.Genres = applicationPath & s
                                    Case "tvcache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.TvCache = applicationPath & s
                                    Case "musicvideocache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.MusicVideoCache = applicationPath & s
                                    Case "moviesetcache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.MovieSetCache = applicationPath & s
                                    Case "customtvcache"
                                        Dim s As String = result.InnerText.ToString.Substring(t)
                                        currentprofile.CustomTvCache = applicationpath & s
                                End Select
                            Next
                            ProfileList.Add(currentprofile)
                    End Select
                Next
            End If
        End If
    End Sub



End Class
