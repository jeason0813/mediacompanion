Imports System.Text.RegularExpressions

Public Class Renamer
    Const SetDefaults = True

    Private Structure str_renameTemplate
        'Episode Renaming
        Dim previous As String
        Dim prefix As String
        Dim showTitleCase As String
        Dim episodeTitleCase As String
        Dim sepShowTitle As String
        Dim sepPreSeasEp As String
        Dim sepPostSeasEp As String
        Dim sepEpisodeTitle As String
        Dim seasNoLen As Integer
        Dim seasChar As String
        Dim epChar As String
        Dim suffix As String

        'Season Renaming
        Dim Se_previous As String
        Dim Se_prefix As String
        Dim Se_showTitleCase As String
        Dim Se_seasonTitleCase As String
        Dim Se_sepShowTitle As String
        Dim Se_sepPreSeas As String
        Dim Se_sepPostSeas As String
        Dim Se_seasNoLen As Integer
        Dim Se_seasChar As String

        Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
            previous = ""
            prefix = ""
            showTitleCase = ""
            episodeTitleCase = ""
            sepShowTitle = ""
            sepPreSeasEp = ""
            sepPostSeasEp = ""
            sepEpisodeTitle = ""
            seasNoLen = 0
            seasChar = ""
            epChar = ""
            suffix = ""
            Se_previous = ""
            Se_prefix = ""
            Se_showTitleCase = ""
            Se_seasonTitleCase = ""
            Se_sepShowTitle = ""
            Se_sepPreSeas = ""
            Se_sepPostSeas = ""
            Se_seasNoLen = 0
            Se_seasChar = ""
        End Sub
    End Structure

    Shared rename As New str_renameTemplate(SetDefaults)

    Public Shared Function setRenamePref(ByVal strRenamePref As String, ByRef tvRegexScraper As List(Of String)) As Boolean
        If String.Equals(strRenamePref, rename.previous) Then Return True
        Dim strRenameWorking As String = strRenamePref.ToLower
        Dim posShow As Integer = strRenameWorking.IndexOf("show")
        Dim posShowTitle As Integer = If(posShow <> -1, strRenameWorking.IndexOf("title"), -1)
        Dim posEpisode As Integer = strRenameWorking.IndexOf("episode")
        Dim posEpisodeTitle As Integer = If(posEpisode <> -1, strRenameWorking.IndexOf("title", posEpisode), -1)
        Dim posExt As Integer = strRenameWorking.IndexOf(".ext")

        'Check for correct syntax for show and episode titles
        Dim test4ShowTitle As Match = Regex.Match(strRenameWorking, "show.?title")
        If Not test4ShowTitle.Success Then posShow = -1
        Dim test4EpTitle As Match = Regex.Match(strRenameWorking, "episode.?title")
        If Not test4EpTitle.Success Then posEpisode = -1

        Dim DoWeReturn As Boolean = True  'added result holder of test since m.sucess may still contain nothing if there are no regexp in Form1.tv_RegexScraper

        Dim M As Match = Nothing        'm.success is readonly so cannot be set to false in advance....

        For Each regexp In tvRegexScraper
            M = Regex.Match(strRenameWorking, regexp)           'm.sucess is true or false now....
            If M.Success = True Then
                DoWeReturn = False 'added result change
                Exit For
            End If
        Next

        If DoWeReturn Then Return False 'If Not M.Sucess then Return false     'If For Loop did not loop at all then M.Success still contains 'nothing' and we get an exception

        rename.previous = strRenamePref
        rename.prefix = If(posShow > 0, strRenamePref.Substring(0, posShow), If(posShow < 0 And M.Index > 0, strRenamePref.Substring(0, M.Index), ""))
        rename.sepShowTitle = If(posShow <> -1 And posShowTitle <> -1, strRenamePref.Substring(posShow + 4, posShowTitle - (posShow + 4)), "")
        rename.sepEpisodeTitle = If(posEpisode <> -1, strRenamePref.Substring(posEpisode + 7, posEpisodeTitle - (posEpisode + 7)), "")
        rename.sepPreSeasEp = If(posShowTitle <> -1, strRenamePref.Substring(posShowTitle + 5, M.Index - (posShowTitle + 5)), "")
        rename.sepPostSeasEp = If(posEpisode <> -1, strRenamePref.Substring(M.Index + M.Length, posEpisode - (M.Index + M.Length)), "")
        rename.seasNoLen = M.Groups(1).Length
        rename.seasChar = strRenameWorking.Substring(M.Index, M.Groups(1).Index - M.Index)
        rename.epChar = strRenameWorking.Substring(M.Groups(1).Index + M.Groups(1).Length, M.Groups(2).Index - (M.Groups(1).Index + M.Groups(1).Length))
        rename.suffix = If(posExt <> -1, strRenamePref.Substring(If(posEpisodeTitle <> -1, posEpisodeTitle + 5, M.Index + M.Length), posExt - If(posEpisodeTitle <> -1, posEpisodeTitle + 5, M.Index + M.Length)), "")

        applySeasonEpisodeCase()

        rename.showTitleCase = ""
        If posShow <> -1 Then
            If Char.IsLower(strRenamePref.Chars(posShow)) Then
                rename.showTitleCase = "LC"                                 'LC = lowercase
            ElseIf Char.IsUpper(strRenamePref.Chars(posShow + 1)) Then
                rename.showTitleCase = "UC"                                 'UC = uppercase
            ElseIf Char.IsLower(strRenamePref.Chars(posShowTitle)) Then
                rename.showTitleCase = "FL"                                 'FL = first letter
            Else
                rename.showTitleCase = "SC"                                 'SC = sentence case
            End If
        End If

        rename.episodeTitleCase = ""
        If posEpisode <> -1 Then
            If Char.IsLower(strRenamePref.Chars(posEpisode)) Then
                rename.episodeTitleCase = "LC"
            ElseIf Char.IsUpper(strRenamePref.Chars(posEpisode + 1)) Then
                rename.episodeTitleCase = "UC"
            ElseIf Char.IsLower(strRenamePref.Chars(posEpisodeTitle)) Then
                rename.episodeTitleCase = "FL"
            Else
                rename.episodeTitleCase = "SC"
            End If
        End If

        Return True
    End Function

    Public Shared Function setTVFilename(ByVal showtitle As String, ByVal episodetitle As String, ByVal episodeno As List(Of String), ByVal seasonno As String) As String
        If showtitle = Nothing Then showtitle = ""
        If episodetitle = Nothing Then episodetitle = ""

        Dim newfilename As String = ""

        If rename.showTitleCase <> "" Then
            Dim formatShowTitle() As String = showtitle.ToLower.Split(" ")
            Select Case rename.showTitleCase
                Case "UC"
                    For i = 0 To formatShowTitle.Length - 1
                        formatShowTitle(i) = formatShowTitle(i).ToUpper
                    Next
                Case "SC"
                    For i = 0 To formatShowTitle.Length - 1
                        formatShowTitle(i) = StrConv(formatShowTitle(i), VbStrConv.ProperCase)
                    Next
                Case "FL"
                    formatShowTitle(0) = StrConv(formatShowTitle(0), VbStrConv.ProperCase)
            End Select
            
            If rename.showTitleCase = "SC" Then
                showtitle = Utilities.TitleCase(showtitle).Replace(" ", rename.sepShowTitle)
            Else
                showtitle = String.Join(rename.sepShowTitle, formatShowTitle)
            End If
        Else
            showtitle = ""
        End If

        If rename.episodeTitleCase <> "" Then
            Dim formatEpisodeTitle() As String = episodetitle.ToLower.Split(" ")
            Select Case rename.episodeTitleCase
                Case "UC"
                    For i = 0 To formatEpisodeTitle.Length - 1
                        formatEpisodeTitle(i) = formatEpisodeTitle(i).ToUpper
                    Next
                Case "SC"
                    For i = 0 To formatEpisodeTitle.Length - 1
                        formatEpisodeTitle(i) = ApostropheCheck(formatEpisodeTitle(i))
                        formatEpisodeTitle(i) = StrConv(formatEpisodeTitle(i), VbStrConv.ProperCase)
                    Next
                Case "FL"
                    formatEpisodeTitle(0) = StrConv(formatEpisodeTitle(0), VbStrConv.ProperCase)
            End Select
            If rename.episodeTitleCase = "SC" Then
                episodetitle = Utilities.TitleCase(episodetitle).Replace(" ", rename.sepEpisodeTitle)
            Else
                episodetitle = String.Join(rename.sepEpisodeTitle, formatEpisodeTitle)
            End If
        Else
            episodetitle = ""
        End If
        episodetitle = episodetitle.TrimEnd

        If rename.seasNoLen = 1 Then
            Do While seasonno.Length > 1 And seasonno.Substring(0, 1) = "0"
                seasonno = seasonno.Substring(1, seasonno.Length - 1)
            Loop

        End If
        newfilename = rename.prefix & showtitle & rename.sepPreSeasEp & rename.seasChar & seasonno
        Dim epChar = rename.epChar
        For Each ep In episodeno
            newfilename = newfilename & epChar & ep
            If epChar = "" Then epChar = "x"
        Next
        newfilename = newfilename & rename.sepPostSeasEp & episodetitle & rename.suffix

        Return newfilename
    End Function

    Public Shared Sub applySeasonEpisodeCase()
        If Pref.eprenamelowercase Then
            rename.seasChar = rename.seasChar.ToLower()
            rename.epChar = rename.epChar.ToLower()
        Else
            rename.seasChar = rename.seasChar.ToUpper()
            rename.epChar = rename.epChar.ToUpper()
        End If
    End Sub

    Public Shared Function ApostropheCheck(ByVal s As String) As String
        If s.Contains(Chr(146)) Then
            Dim chkarr() As String = {"t", "s", "m", "ll", "ve"}
            For Each ch In chkarr
                If s.Contains(Chr(146) & ch) Then
                    s = s.Replace(Chr(146), "'")
                End If
            Next
        End If
        Return s
    End Function

    'Public Shared Function setSeasonRenamePref(ByVal strSeasonRenamePref As String) As Boolean
    '    If String.Equals(strSeasonRenamePref, rename.Se_previous) Then Return True
    '    Dim strSeasonRenameWorking As String = strSeasonRenamePref.ToLower
    '    Dim posShow As Integer = strSeasonRenameWorking.IndexOf("show")
    '    Dim posShowTitle As Integer = If(posShow <> -1, strSeasonRenameWorking.IndexOf("title"), -1)
    '    Dim posSeason As Integer = strSeasonRenameWorking.IndexOf("Season")
    '    'Dim posEpisodeTitle As Integer = If(posEpisode <> -1, strSeasonRenameWorking.IndexOf("title", posEpisode), -1)
    '    Dim posExt As Integer = strSeasonRenameWorking.IndexOf("xx")

    '    'Check for correct syntax for show and episode titles
    '    Dim test4ShowTitle As Match = Regex.Match(strSeasonRenameWorking, "show.?title")
    '    If Not test4ShowTitle.Success Then posShow = -1
    '    Dim test4EpTitle As Match = Regex.Match(strSeasonRenameWorking, "episode.?title")
    '    If Not test4EpTitle.Success Then posEpisode = -1

    '    Dim DoWeReturn As Boolean = True  'added result holder of test since m.sucess may still contain nothing if there are no regexp in Form1.tv_RegexScraper

    '    Dim M As Match = Nothing        'm.success is readonly so cannot be set to false in advance....

    '    For Each regexp In tvRegexScraper
    '        M = Regex.Match(strRenameWorking, regexp)           'm.sucess is true or false now....
    '        If M.Success = True Then
    '            DoWeReturn = False 'added result change
    '            Exit For
    '        End If
    '    Next

    '    If DoWeReturn Then Return False 'If Not M.Sucess then Return false     'If For Loop did not loop at all then M.Success still contains 'nothing' and we get an exception

    '    rename.previous = strSeasonRenamePref
    '    rename.prefix = If(posShow > 0, strSeasonRenamePref.Substring(0, posShow), If(posShow < 0 And M.Index > 0, strSeasonRenamePref.Substring(0, M.Index), ""))
    '    rename.sepShowTitle = If(posShow <> -1 And posShowTitle <> -1, strSeasonRenamePref.Substring(posShow + 4, posShowTitle - (posShow + 4)), "")
    '    rename.sepEpisodeTitle = If(posEpisode <> -1, strSeasonRenamePref.Substring(posEpisode + 7, posEpisodeTitle - (posEpisode + 7)), "")
    '    rename.sepPreSeasEp = If(posShowTitle <> -1, strSeasonRenamePref.Substring(posShowTitle + 5, M.Index - (posShowTitle + 5)), "")
    '    rename.sepPostSeasEp = If(posEpisode <> -1, strSeasonRenamePref.Substring(M.Index + M.Length, posEpisode - (M.Index + M.Length)), "")
    '    rename.seasNoLen = M.Groups(1).Length
    '    rename.seasChar = strSeasonRenameWorking.Substring(M.Index, M.Groups(1).Index - M.Index)
    '    rename.epChar = strSeasonRenameWorking.Substring(M.Groups(1).Index + M.Groups(1).Length, M.Groups(2).Index - (M.Groups(1).Index + M.Groups(1).Length))
    '    rename.suffix = If(posExt <> -1, strSeasonRenamePref.Substring(If(posEpisodeTitle <> -1, posEpisodeTitle + 5, M.Index + M.Length), posExt - If(posEpisodeTitle <> -1, posEpisodeTitle + 5, M.Index + M.Length)), "")

    '    applySeasonEpisodeCase()

    '    rename.showTitleCase = ""
    '    If posShow <> -1 Then
    '        If Char.IsLower(strRenamePref.Chars(posShow)) Then
    '            rename.showTitleCase = "LC"                                 'LC = lowercase
    '        ElseIf Char.IsUpper(strRenamePref.Chars(posShow + 1)) Then
    '            rename.showTitleCase = "UC"                                 'UC = uppercase
    '        ElseIf Char.IsLower(strRenamePref.Chars(posShowTitle)) Then
    '            rename.showTitleCase = "FL"                                 'FL = first letter
    '        Else
    '            rename.showTitleCase = "SC"                                 'SC = sentence case
    '        End If
    '    End If

    '    rename.episodeTitleCase = ""
    '    If posEpisode <> -1 Then
    '        If Char.IsLower(strRenamePref.Chars(posEpisode)) Then
    '            rename.episodeTitleCase = "LC"
    '        ElseIf Char.IsUpper(strRenamePref.Chars(posEpisode + 1)) Then
    '            rename.episodeTitleCase = "UC"
    '        ElseIf Char.IsLower(strRenamePref.Chars(posEpisodeTitle)) Then
    '            rename.episodeTitleCase = "FL"
    '        Else
    '            rename.episodeTitleCase = "SC"
    '        End If
    '    End If

    '    Return True
    'End Function

End Class
