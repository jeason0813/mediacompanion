Imports System.Text.RegularExpressions
Imports Alphaleonis.Win32.Filesystem

Public Structure str_MovieActors

    Public actorname As String
    Public actorrole As String
    Public actorthumb As String
    Public actorid As String
    Public order As String

    Sub New(SetDefaults As Boolean) 'When called with new keyword & boolean constant SetDefault (either T or F), initialises all values to defaults to avoid having some variables left as 'nothing'
        actorname = ""
        actorrole = ""
        actorthumb = ""
        actorid = ""
        order = ""
    End Sub

    Shared Widening Operator CType(ByVal Input As Media_Companion.Actor) As str_MovieActors
        Dim Temp As New str_MovieActors(True)
        Temp.actorid = Input.ID.Value
        Temp.actorname = Input.Name.Value
        Temp.actorrole = Input.Role.Value
        Temp.actorthumb = Input.Thumb.Value
        Temp.order = Input.SortOrder.Value
        Return Temp
    End Operator

    Shared Widening Operator CType(ByVal Input As str_MovieActors) As Media_Companion.Actor
        Dim Temp As New Media_Companion.Actor
        Temp.ID.Value = Input.actorid
        Temp.Name.Value = Input.actorname
        Temp.Role.Value = Input.actorrole
        Temp.Thumb.Value = Input.actorthumb
        Temp.SortOrder.Value = Input.order
        Return Temp
    End Operator
    
    Function GetActorFileName(ActorPath As String) As String
        Return Path.Combine(ActorPath, actorname.Replace(" ", "_") & ".jpg")
    End Function

    Public Function SaveActor(ActorPath As String) As Boolean
        Dim aok As Boolean = True
        If Not String.IsNullOrEmpty(actorthumb) Then
            Dim filename As String = GetActorFileName(ActorPath)
            Dim newlydownloaded As Boolean = False
            'Allow to save to .actors folder
            If Pref.actorseasy Then
                Dim hg As New DirectoryInfo(ActorPath)
                If Not hg.Exists Then Directory.CreateDirectory(ActorPath)
                If Not File.Exists(filename) OrElse Pref.overwritethumbs Then
                    If Movie.SaveActorImageToCacheAndPath(actorthumb, filename) Then
                        newlydownloaded = True
                        ActorSave(filename)
                    Else
                        aok = False
                    End If
                Else 
                    aok = True
                End If
            End If

            'Allow also to save to local path/network path
            If Pref.actorsave AndAlso actorid <> "" Then
                If Not String.IsNullOrEmpty(Pref.actorsavepath) Then
                    Dim tempstring As String = Pref.actorsavepath
                    Dim workingpath As String = ""
                    If Pref.actorsavealpha Then
                        Dim actorfilename As String = actorname.Replace(" ", "_") & If(Pref.LocalActorSaveNoId, "", "_" & actorid) & ".jpg"
                        tempstring = tempstring & "\" & actorfilename.Substring(0,1) & "\"
                        workingpath = tempstring & actorfilename 
                    Else
                        tempstring = tempstring & "\" & actorid.Substring(actorid.Length - 2, 2) & "\"
                        workingpath = tempstring & actorid & ".jpg"
                    End If
                    Utilities.EnsureFolderExists(tempstring)
                    If DownloadCache.SaveImageToCacheAndPath(actorthumb, workingpath, If(newlydownloaded, False, Pref.overwritethumbs), , Movie.GetHeightResolution(Pref.ActorResolutionSI)) Then
                        ActorSave(workingpath)
                    Else
                        aok = False
                    End If
                    If aok AndAlso Not String.IsNullOrEmpty(Pref.actornetworkpath) Then
                        If Pref.actornetworkpath.IndexOf("/") <> -1 Then
                            actorthumb = workingpath.Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("\", "/") 'Pref.actornetworkpath & "/" & actorid.Substring(actorid.Length - 2, 2) & "/" & actorid & ".jpg"
                        Else
                            actorthumb = workingpath.Replace(Pref.actorsavepath, Pref.actornetworkpath).Replace("/", "\") 'Pref.actornetworkpath & "\" & actorid.Substring(actorid.Length - 2, 2) & "\" & actorid & ".jpg"
                        End If
                    End If
                End If
            End If
        End If
        Return aok
    End Function

    Sub ActorSave(ByRef workingpath As String)
        If Pref.EdenEnabled And Not Pref.FrodoEnabled Then
            Utilities.SafeCopyFile(workingpath, workingpath.Replace(".jpg", ".tbn"), Pref.overwritethumbs)
            Utilities.SafeDeleteFile(workingpath)
            workingpath = workingpath.Replace(".jpg", ".tbn")
        ElseIf Pref.EdenEnabled And Pref.FrodoEnabled Then
            Utilities.SafeCopyFile(workingpath, workingpath.Replace(".jpg", ".tbn"), Pref.overwritethumbs)
        End If
    End Sub


    'Input  : http://ia.media-imdb.com/images/M/MV5BMTY3Njc5ODc4OV5BMl5BanBnXkFtZTYwNjY5MTU0._V1_SX32_CR0,0,32,44_.jpg
    'Output : http://ia.media-imdb.com/images/M/MV5BMTY3Njc5ODc4OV5BMl5BanBnXkFtZTYwNjY5MTU0._V1._SY400_SX300_.jpg

    Public Function GetBigThumb(smallThumb As String) As String

        If smallThumb="" Then Return ""
        Return smallThumb.Substring(0, smallThumb.IndexOf("._V1_")) & "._V1._SY400_SX300_.jpg"
    End Function

    Public Function AssignFromImdbTr(Tr As String) As Boolean
        Dim m As Match
        If Tr.IndexOf("loadlate")=-1 Then
            m = Regex.Match(Tr, MovieRegExs.REGEX_ACTOR_NO_IMAGE, RegexOptions.Singleline)
        Else
            m = Regex.Match(Tr, MovieRegExs.REGEX_ACTOR_WITH_IMAGE, RegexOptions.Singleline)
        End If

        Try
            actorname  = m.Groups("actorname").ToString.CleanSpecChars.CleanFilenameIllegalChars
        Catch
        End Try

        Try
            actorrole  = m.Groups("actorrole").ToString.StripTagsLeaveContent.CleanSpecChars.Trim.CleanActorRole
        Catch
        End Try

        Try 
            actorthumb = GetBigThumb(m.Groups("actorthumb").ToString).EncodeSpecialChrs
        Catch
        End Try

        Try
            actorid    = m.Groups("actorid").ToString
        Catch
        End Try
            order       = m.Groups("order").ToString'"0"
        Return actorname<>""
    End Function

End Structure
