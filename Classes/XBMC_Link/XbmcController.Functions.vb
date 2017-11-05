Imports Sanford.StateMachineToolkit
Imports XBMC.JsonRpc
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Data.SQLite
Imports System.Linq

Public Partial Class XbmcController

#Region "Info, Warning & Error Reporting"

    Const Error_Prefix    = "**ERROR** "
    Const Warning_Prefix  = "**WARNING** "

    Sub LogInfo(ByVal Action As String)
        ReportProgress(EnumLogMode.Brief, Action, LastArgs)
        ReportProgress(EnumLogMode.Full , Action, LastArgs)
    End Sub

    Sub LogInfo(LogMode As EnumLogMode, ByVal Action As String)
        ReportProgress(LogMode, Action, LastArgs)
    End Sub

    Sub LogInfo(LogMode As EnumLogMode, ByVal Action As String, evt As TransitionEventArgs(Of S, E, EventArgs))
        ReportProgress(LogMode, Action, evt)
    End Sub

    Sub LogWarning(Action As String, jeea As XbmcJsonRpcLogErrorEventArgs, tea As TransitionEventArgs(Of S, E, EventArgs))
        LogWarning(EnumLogMode.Full , Action, BuildJsonErrorMsg(jeea) ,tea)
        LogWarning(EnumLogMode.Brief, Action, BuildJsonErrorMsg(jeea) ,tea)
    End Sub

    Sub LogWarning(LogMode As EnumLogMode, Action As String, jeea As XbmcJsonRpcLogErrorEventArgs, tea As TransitionEventArgs(Of S, E, EventArgs))
        LogWarning(LogMode, Action, BuildJsonErrorMsg(jeea) ,tea)
    End Sub

    Sub LogWarning(Action As String, WarningMsg As String, tea As TransitionEventArgs(Of S, E, EventArgs) )
        ReportProgress(EnumLogMode.Full , Action, tea, "W", WarningMsg)
        ReportProgress(EnumLogMode.Brief, Action, tea, "W", WarningMsg)
    End Sub

    Sub LogWarning(LogMode As EnumLogMode, Action As String, WarningMsg As String, tea As TransitionEventArgs(Of S, E, EventArgs) )
        ReportProgress(LogMode, Action, tea, "W", WarningMsg)
    End Sub

    Sub LogError(Action As String, jeea As XbmcJsonRpcLogErrorEventArgs, tea As TransitionEventArgs(Of S, E, EventArgs))
        LogError(EnumLogMode.Brief, Action, BuildJsonErrorMsg(jeea) ,tea)
        LogError(EnumLogMode.Full , Action, BuildJsonErrorMsg(jeea) ,tea)
    End Sub

    Sub LogError(LogMode As EnumLogMode, Action As String, jeea As XbmcJsonRpcLogErrorEventArgs, tea As TransitionEventArgs(Of S, E, EventArgs))
        LogError(LogMode, Action, BuildJsonErrorMsg(jeea) ,tea)
    End Sub

    Sub LogError(Action As String, ErrorMsg As String, tea As TransitionEventArgs(Of S, E, EventArgs) )
        LogError(EnumLogMode.Brief, Action, ErrorMsg ,tea)
        LogError(EnumLogMode.Full , Action, ErrorMsg ,tea)
    End Sub


    Sub LogError(LogMode As EnumLogMode, Action As String, ErrorMsg As String, tea As TransitionEventArgs(Of S, E, EventArgs) )
        ReportProgress(LogMode, Action, tea, "E", ErrorMsg)
    End Sub

    Function BuildJsonErrorMsg(jeea As XbmcJsonRpcLogErrorEventArgs) As String

        If IsNothing(jeea) Then
            Return "No error message received"
        End If

        Dim msg        As String = ""
        Dim exMsg      As String = ""
        Dim innerExMsg As String = ""

        If Not IsNothing(jeea.Message) Then msg = "Msg : [" & jeea.Message & "]"

        Try
            exMsg = " Ex msg : [" & jeea.Exception.Message & "]"
        Catch
        End Try

        Try
            innerExMsg = " Inner ex msg : [" & jeea.Exception.InnerException.Message & "]"
        Catch
        End Try

        Return msg + exMsg + innerExMsg
    End Function

    Sub ReportProgress(LogMode As EnumLogMode, ByVal Action As String, evt As TransitionEventArgs(Of S, E, EventArgs), Optional Severity As String="I", Optional ErrorMsg As String=Nothing)

        Dim logMsg As String = ""

        If LogMode= EnumLogMode.Full Then 
            If Severity="I" Then logMsg=                                               Action
            If Severity="W" Then logMsg= Warning_Prefix + ErrorMsg + " reported in " + Action
            If Severity="E" Then logMsg=   Error_Prefix + ErrorMsg + " reported in " + Action
        Else
            'State - Event - Action                                                               UnBuffering :
            logMsg = "State : [" + evt.SourceStateID.ToString.PadRight(LongestEnumStateName) + "] Event       : [" + evt.EventID.ToString.PadRight(LongestEnumEventName) + "] Action : [" + Action + "]"
        End If

        AppendLog(LogMode,logMsg)

        If LogMode=EnumLogMode.Brief Then Return

        If Severity="W" Then WarningCount+=1
        If Severity="E" Then ErrorCount  +=1

        Dim formattedErrorMsg As String = ErrorMsg

        If formattedErrorMsg<>"" Then formattedErrorMsg = GetTimeStamp + " " + formattedErrorMsg

        Dim p As New XBMC_Controller_Progress(LastState,evt.EventID,evt.EventArgs,Action,evt.SourceStateID,Severity,formattedErrorMsg,ErrorCount,WarningCount)

        ReportProgress(p)
    End Sub

    Sub ReportProgress(EventId As XbmcController.E,  Optional Args As EventArgs=Nothing)

        Dim p As New XBMC_Controller_Progress

        p.Evt  = EventId
        p.Args = Args

        ReportProgress(p)
    End Sub

    Sub ReportProgress(ByVal oProgress As XBMC_Controller_Progress)
        Try
            Bw.ReportProgress(0, oProgress)
        Catch
        End Try
    End Sub

    Sub AppendLog(msg As String)
        AppendLog(EnumLogMode.Full , msg)
        AppendLog(EnumLogMode.Brief, msg)
    End Sub

    Sub AppendLog(LogMode As EnumLogMode, msg As String)

        Dim logMsg As String = " Q: " + Q.Count.ToString.PadLeft(2) + " " + " Buf Q: " + BufferQ.Count.ToString.PadLeft(4) + " - " + msg

        Try
            If LogMode= EnumLogMode.Full Then
                fullLog.Debug(logMsg)
            Else
                briefLog.Debug(logMsg)
            End If
        Catch
        End Try
    End Sub

#End Region 

#Region "Timer funcs"

    Sub StartTimeoutTimer(Interval As Integer)
        LogInfo("Timeout Timer started")
        StartTimer(TimeoutTimer,Interval)
    End Sub 

    Sub StopTimeoutTimer
        If TimeoutTimer.Enabled Then
            LogInfo("Timeout Timer stopped")
            TimeoutTimer.Stop
        End If
    End Sub 

    Sub Start_McMainBusyTimer_Timer
        LogInfo("Mc Main Busy Timer started")
        StartTimer(McMainBusyTimer,3000) 
    End Sub 

    Sub StartMaxMovies_Idle_Timer
        LogInfo("MaxMovies Timer started")
        StartTimer(MaxMovies_Idle_Timer,1000)    '30000
    End Sub 

    Sub Ini_Timer(t As Timers.Timer,Optional Interval As Integer=1000)
        t.Stop
        t.Interval = Interval
        t.AutoReset = False
    End Sub

    Sub StartTimer(t As Timers.Timer,Interval As Integer)
        t.Stop
        t.Interval = Interval
        t.Start
    End Sub 

    Sub StopAllTimers
        TimeoutTimer        .Stop 
        MaxMovies_Idle_Timer.Stop
        McMainBusyTimer     .Stop
    End Sub


#End Region

#Region "General Func"

    Function LongestEnum(ByVal Item as Type) As Integer
        Return Item.GetEnumNames.OrderByDescending((Function(s) s.Length )).FirstOrDefault.Length
    End Function


    Sub Reset
        StopAllTimers
        Q.Clear
        BufferQ.Clear
    End Sub

    Sub AssignLastEvent(args As TransitionEventArgs(Of S, E, EventArgs))
        LastEvent.Assign(New BaseEvent(args.EventID, args.EventArgs))
    End Sub

    Sub DecodeNfoEventArgs(args As TransitionEventArgs(Of S, E, EventArgs))
        Dim ea As VideoPathEventArgs = args.EventArgs

        McMoviePath = ea.McMoviePath

        XbMoviePath = MovieFolderMappings.GetXBMC_MoviePath(McMoviePath)

        If IsNothing(XbMoviePath) Then
            AppendLog(Error_Prefix + "Failed to map [" + McMoviePath + "] to XBMC folders. Check your MC <-> XBMC Movies FolderMappings in Config.XML" )
        End If
    End Sub

    Function CanConnect As Boolean
        Try
            XbmcTexturesDb.Open
            XbmcTexturesDb.Close
            Return True
        Catch ex As Exception
            LogError("CanConnect", "Failed to connect to Textures db. Fanart & Poster changes will not appear in XBMC as the existing cached images cannot be deleted. Error : ["+ex.Message+"]",Nothing)
            Return False
        End Try
    End Function

    Sub GetCachedUrls

        If Not Pref.XBMC_Delete_Cached_Images Then
            LogInfo("Skipping getting cached image urls from TexturesDb")
            Return
        End If

        If Not CanDeleteCachedImages Then Return

        XbmcTexturesDb.Open

        _dtCachedUrls = GetCachedUrlsMeat(False)

        If _dtCachedUrls.Rows.Count=0 Then
            _dtCachedUrls = GetCachedUrlsMeat(True)
        End If

        XbmcTexturesDb.Close
    End Sub

    Function GetCachedUrlsMeat(useXbmcPath As Boolean) As DataTable

        Dim oMovie As Movie = New Movie(McMoviePath,Me.Parent.oMovies)

        oMovie.LoadNFO(False)

        Dim cmd As SQLiteCommand = new SQLiteCommand(XbmcTexturesDb)

        Dim PosterPaths As List(Of String) = oMovie.ActualPosterPaths

        Dim sql As String = "Select id, cachedurl from texture where url=@FanartPath"
        Dim i As Integer=1

        For Each Item As String In PosterPaths
            sql +=  " or url=@PosterPath" + i.ToString
            i +=1
        Next

        cmd.CommandText = sql

        cmd.Parameters.Add("@FanartPath",SqlDbType.VarChar,500).Value=IIf(useXbmcPath,MovieFolderMappings.GetXBMC_MoviePath(oMovie.ActualFanartPath),oMovie.ActualFanartPath)

        i=1
        For Each Item As String In PosterPaths
            cmd.Parameters.Add("@PosterPath" + i.ToString,SqlDbType.VarChar,500).Value=IIf(useXbmcPath,MovieFolderMappings.GetXBMC_MoviePath(Item),Item)
            i +=1
        Next

        Return DbUtils.ExecuteReader(cmd)

    End Function

    Sub AddFolderToScan(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        DecodeNfoEventArgs(args)
       
        If IsNothing(XbMoviePath) Then
            AppendLog(Warning_Prefix + "Can't add new movie, as XBMC is missing a source movie folder to access this folder : [" + Path.GetDirectoryName(McMoviePath) + "]" )
            Return
        End If

        Dim XbFolder As String = Path.GetDirectoryName(XbMoviePath)


        If Not MoviesInFolder.ContainsKey(XbFolder) Then
            MoviesInFolder.Add(XbFolder,1)
        Else
            MoviesInFolder(XbFolder) = MoviesInFolder(XbFolder)+1
        End If


        Dim got = From i In BufferQ.View Where i.E = E.ScanFolder AndAlso DirectCast(i.Args,FolderEventArgs).Folder=XbFolder
              
        If got.Count=0 Then
            If NumberOfMcNfosInFolder=1 Then
                Q      .Write(New BaseEvent(E.ScanFolder, New FolderEventArgs(XbFolder,PriorityQueue.Priorities.high)))
            Else
                BufferQ.Write(New BaseEvent(E.ScanFolder, New FolderEventArgs(XbFolder,PriorityQueue.Priorities.low )))
            End If
        End If
    End Sub

    Function GetTimeStamp() As String
        Return Format(DateTime.Now, "HH:mm:ss.fff").ToString
    End Function
#End Region

#Region "Clutter"
    'Function GetMatchingScanFolderItems(x As String) 
    '    Return From i In Q.View 
    '           Where 
    '                i.E = E.ScanFolder _
    '            AndAlso
    '                DirectCast(i.Args,FolderEventArgs).Folder.Contains(x)
    'End Function

    'Function NumMatchingFolders(x As String) As Integer
    '    Return (From i In BufferQ.View Where i.E = E.ScanFolder _
    '        Select 
    '            Folder=DirectCast(i.Args,FolderEventArgs).Folder 
    '        Where
    '            Folder.Contains(x)).Count
    'End Function

    'Property Mapped
    '            Dim moviePaths = (From
    '                            m In Parent.oMovies.MoviesWithUniqueMovieTitles
    '                        Where
    '                            m.fullpathandfilename.ToUpper.Contains(wotEver.ToUpper)
    '                        Join
    '                            x In XbmcJson.MoviesWithUniqueMovieTitles
    '                        On
    '                            x.title Equals m.title And Path.GetFileName(x.file).ToUpper Equals Path.GetFileName(m.MoviePathAndFileName).ToUpper
    '                        Select
    '                            m.MoviePathAndFileName, x.file).First


    'Private Sub GetNewMovieIds_IdleTimer_Elapsed(ByVal sender As Object, ByVal ev As Timers.ElapsedEventArgs)

    '    GetNewMovieIds_IdleTimer.Stop()
    '    AppendLog("GetNewMovieIds_IdleTimer Elapsed")

    '    Q.Write(E.GetNewMovieIds,PriorityQueue.Priorities.low)

    'End Sub


    'Private Sub FolderScan_IdleTimer_Elapsed(ByVal sender As Object, ByVal ev As Timers.ElapsedEventArgs)

    '    FolderScan_IdleTimer.Stop()
    '    AppendLog("FolderScan_IdleTimer Elapsed")

    '    Q.Write(E.ScanForContentReq,PriorityQueue.Priorities.low)
    'End Sub

    'Private Sub WatchDogTimer_Elapsed(ByVal sender As Object, ByVal ev As Timers.ElapsedEventArgs)

    '    sender.Stop()
    '    AppendLog("WatchDog Timer Elapsed!")

    '    Q.Write(E.WatchDogTimeOut,PriorityQueue.Priorities.high)
    'End Sub

    'Sub GetNewMovieIds(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

    '    AppendLog("GetNewMovieIds")

    '    'If PercentageNewMovieIdsToGet >= ScanAllThreshold Then
    '    XbmcJson.XbmcMovies.Clear()
    '    XbmcJson.UpdateXbmcMovies()
    '    'Else
    '    '    For Each ea As NfoEventArgs In NewMovieIdsToGet
    '    '        XbmcJson.UpdateXbmcMovies(ea.Title)
    '    '    Next
    '    'End If

    'End Sub


    '  'From: http://forum.xbmc.org/showthread.php?pid=1428281
    '  '
    '  ' Batch commands ARE supported, BUT CAN BE EXECUTED IN ANY ORDER.
    '  '
    '  Sub AddMovie(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

    '      Dim ea As NfoEventArgs = args.EventArgs

    '      Dim folder As String = Path.GetDirectoryName(ea.Nfo)

    '      'JObject GetCall(string method, object args)

    '      Dim call_1 As JObject = XbmcJson.xbmc.Library.Video.GetCall_AddMovies    (folder)
    '      Dim call_2 As JObject = XbmcJson.xbmc.Library.Video.GetCall_MinXbmcMovies(ea.Title)

    '      'Dim main As List(Of String) = New List(Of String)

    '      'main.Add(call_1.ToString)
    '      'main.Add(call_2.ToString)

    ''     Dim json = JsonConvert.SerializeObject(main)

    '      Try
    '          Dim json = "[" + call_1.ToString + "," + call_2.ToString + "]"

    '          Dim o = XbmcJson.xbmc.Library.client.SendCall(json)

    '          'Dim m As JArray = new JArray()
    '          'm.Add(call_1)
    '          'm.Add(call_2)
    '          'Dim o = XbmcJson.xbmc.Library.client.SendCall(m)

    '          Console.Write(o.ToString + Environment.NewLine)
    '      Catch ex As Exception
    '          Console.Write(ex.ToString + Environment.NewLine)
    '      End Try
    '  End Sub


    'Sub ScanNewMovies(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

    '    Dim ea As ScanNewMoviesEventArgs = args.EventArgs

    '    Dim Interval As Integer = 5000 + ((ea.NumMovies)*1000)

    '    ReportProgress("Scanning for new movies in ALL movie folders...",args)

    '    XbmcJson.xbmc.Library.Video.AddMovies()

    '    StartTimer(Interval)
    'End Sub

    'Function GetTimeStamp() As String
    '    Return Format(DateTime.Now, "HH:mm:ss.fff").ToString
    'End Function

    'Sub RemoveThenAddMovie(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

    '    ReportProgress( "RemoveThenAddMovie" )

    '    RemoveMovie(sender, args)
    '    'AddMovie   (sender, args)

    '    'Dim ea As NfoEventArgs = args.EventArgs

    '    'MoviesToAdd.Add(ea)

    '    'If Q.Count = 0 Then

    '    '    If PercentageToUpdate >= ScanAllThreshold Then
    '    '        Q.Write(PriorityQueue.Priorities.low, New CompleteEvent(E.ScanForContent))
    '    '    Else
    '    '        For Each movie In MoviesToAdd
    '    '            Q.Write(PriorityQueue.Priorities.low, New CompleteEvent(E.Movie_New, movie)) 'New NfoEventArgs(movie.Nfo,movie.Title)) ) 
    '    '        Next
    '    '    End If
    '    '    MoviesToAdd.Clear()
    '    'End If
    'End Sub

    'Sub IniGetNewMovieIds_IdleTimer()
    '    GetNewMovieIds_IdleTimer.Stop()
    '    GetNewMovieIds_IdleTimer.Interval = 5000
    '    GetNewMovieIds_IdleTimer.AutoReset = True

    '    AddHandler GetNewMovieIds_IdleTimer.Elapsed, New System.Timers.ElapsedEventHandler(AddressOf Me.GetNewMovieIds_IdleTimer_Elapsed)
    'End Sub

    'Sub Restart_Timer(t As Timers.Timer)
    '    t.Stop()
    '    t.Start()
    'End Sub



    'Sub AutoMapMovieFolders(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

    '    ReportProgress("Mapping movie folders : ",args)

    '    MovieFolderMappings.Clear

    '    For Each folder In Pref.movieFolders
    '        Try
    '            Dim wotEver = folder

    '            Dim moviePaths = (From
    '                            m In Parent.oMovies.MoviesWithUniqueMovieTitles
    '                        Where
    '                            m.fullpathandfilename.ToUpper.Contains(wotEver.ToUpper)
    '                        Join
    '                            x In XbmcJson.MoviesWithUniqueMovieTitles
    '                        On
    '                            x.title Equals m.title And Path.GetFileName(x.file).ToUpper Equals Path.GetFileName(m.MoviePathAndFileName).ToUpper
    '                        Select
    '                            m.MoviePathAndFileName, x.file).First


    '            Dim mc = moviePaths.MoviePathAndFileName.ToUpper
    '            Dim xb = moviePaths.file.ToUpper

    '            'xb = xb.Replace(folder.ToUpper,"X:")       'Test to simulate a drive mapping


    '            If mc = xb Then
    '                MovieFolderMappings_Add(folder, folder)
    '            Else
    '                While mc.Length > folder.Length And mc.Chars(mc.Length - 1) = xb.Chars(xb.Length - 1)

    '                    mc = mc.Remove(mc.Length - 1)
    '                    xb = xb.Remove(xb.Length - 1)

    '                End While

    '                MovieFolderMappings_Add(folder, xb)
    '            End If
    '        Catch ex As Exception
    '           AppendLog(Warning_Prefix + "Check XBMC has a movie mapping equivilant to MC's : [" + folder + "] folder")
    '        End Try
    '    Next
    'End Sub

    'Sub MovieFolderMappings_Add( dirMc As String, dirXb As String )

    '    log.Debug("Mapping MC movie folder : [" + dirMc + "] to XBMC movie folder : [" + dirXb + "]")
    '    MovieFolderMappings.Add(dirMc,dirXb)
    'End Sub



#End Region

End Class
