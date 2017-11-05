#Region "Imports"

Imports System
Imports Sanford.StateMachineToolkit
'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports System.ComponentModel
Imports XBMC.JsonRpc
Imports log4net
Imports System.Reflection
Imports System.Linq
Imports System.Data.SQLite

#End Region

Public Class XbmcController : Inherits PassiveStateMachine(Of S, E, EventArgs)

#Region "Conditional Compiler Constants"
    #Const BatchMode       = False
    #Const GenPseudoErrors = False
#End Region

#Region "Events"

    Public Event MaxXbmcMoviesChanged
    Public Event XbmcMcMoviesChanged
    Public Event XbmcOnlyMoviesChanged

#End Region               'Events

#Region "Globals"
'    Public Shared log      As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
    Public Shared fullLog  As ILog = LogManager.GetLogger("LogFileAppenderFull" )
    Public Shared briefLog As ILog = LogManager.GetLogger("LogFileAppenderBrief")
#End Region              'Globals

#Region "Private vars"

    Private _xbmcTexturesDb             As SQLiteConnection
    Private _dtCachedUrls               As DataTable
    Private _maxXbmcMovies              As List(Of XbmcMovieForCompare)
    Private _xbmcMovies                 As Dictionary(Of String, XbmcMovieForCompare)
    Private _mcMovies                   As Dictionary(Of String, ComboList          )
    Private _xbmcMcMovies               As Dictionary(Of String, XbmcMovieForCompare)
    Private _xbmcOnlyMovies             As List      (Of         XbmcMovieForCompare)
    Private _xbmcJson                   As XbmcJson
    Private _XBMC_to_MC_MoviePaths      As Dictionary(Of String,String)          ' Translates XBMC movie paths to their MC equivilant
    Private _CanDeleteCachedImages      As Boolean
    Private _TriedCanDeleteCachedImages As Boolean
    Private TimeoutTimer                As Timers.Timer = New Timers.Timer()
    Private McMainBusyTimer             As Timers.Timer = New Timers.Timer()
    Private MaxMovies_Idle_Timer        As Timers.Timer = New Timers.Timer()
    Public  LongestEnumStateName        As Integer = LongestEnum(GetType(S))
    Public  LongestEnumEventName        As Integer = LongestEnum(GetType(E))

#If GenPseudoErrors Then
    Private ScanRemoved_Count     As Integer =  0
    Private ScanFinished_Count    As Integer =  0
    Private ScanRemoved_MissRate  As Integer =  7
    Private ScanFinished_MissRate As Integer = 13
#End If

    'Private GetNewMovieIds_IdleTimer As Timers.Timer       = New Timers.Timer()
    'Private FolderScan_IdleTimer     As Timers.Timer       = New Timers.Timer()
    'Private _Different               As List(Of ComboList) = New List(Of ComboList)
    'Private _XBMC_Only               As List(Of String)    = New List(Of String)

#End Region         'Private vars

#Region "RW Properties"

    Enum EnumLogMode
     Full
     Brief
    End Enum

    'Property LogMode              As EnumLogMode = EnumLogMode.Brief

    Property XbmcThumbnailsFolder As String = Pref.XBMC_Thumbnails_Path
    Property MoviesInFolder       As New Dictionary(Of String, Integer)
    Property BatchScanFolders     As List(Of String) = New List(Of String)
    Property MovieFolderMappings  As XBMC_MC_FolderMappings = Pref.XBMC_MC_MovieFolderMappings
    Property LastEvent            As BaseEvent = New BaseEvent()
    Property Parent               As Form1
    Property McMoviePath          As String
    Property XbMoviePath          As String
    Property ErrorCount           As Integer=0
    Property WarningCount         As Integer=0
    Property Q                    As PriorityQueue 
    Property BufferQ              As PriorityQueue
    Property Bw                   As BackgroundWorker = Nothing
    Property LastState            As S
    Property LastArgs             As TransitionEventArgs(Of S, E, EventArgs)
    Property ShutDownRequested    As Boolean = False
    Property XbmcMaxMoviesDirty   As Boolean = True

    Property XbmcMcMovies As Dictionary(Of String, XbmcMovieForCompare)
        Get
            Return _xbmcMcMovies
        End Get
        Set
            _xbmcMcMovies = Value
            RaiseEvent XbmcMcMoviesChanged
        End Set
    End Property

    Property XbmcOnlyMovies As List(Of XbmcMovieForCompare)
        Get
            Return _xbmcOnlyMovies
        End Get
        Set
            _xbmcOnlyMovies = Value
            RaiseEvent XbmcOnlyMoviesChanged
        End Set
    End Property

    Property MaxXbmcMovies As List(Of XbmcMovieForCompare)
        Get
            Return _maxXbmcMovies
        End Get
        Set
            _maxXbmcMovies = Value
            RaiseEvent MaxXbmcMoviesChanged
        End Set
    End Property

    'Property MoviesToAdd      As List(Of NfoEventArgs) = New List(Of NfoEventArgs)
    'Property NewMovieIdsToGet As List(Of NfoEventArgs) = New List(Of NfoEventArgs)
    'Property ScanAllThreshold As Integer = 1

#End Region        'RW Properties

#Region "ReadOnly Properties"

    Public ReadOnly Property XbmcTexturesDb As SQLiteConnection
        Get
            If IsNothing(_xbmcTexturesDb) Then
                _xbmcTexturesDb = New SQLiteConnection(Pref.XBMC_TexturesDb_ConnectionStr)
            End If

            Return _xbmcTexturesDb
        End Get
    End Property

    Public ReadOnly Property XbmcJson As XbmcJson
        Get
            If IsNothing(_xbmcJson) Then
                _xbmcJson = New XbmcJson
            End If

            Return _xbmcJson
        End Get
    End Property


    ReadOnly Property MC_Only_Movies As List(Of ComboList)
        Get
            Dim data As List(Of ComboList) = New List(Of ComboList)

            data.AddRange(Parent.oMovies.MovieCache)

            Dim q2 = (From
                        M In data
                     Where
                        Not XBMC_to_MC_MoviePaths.ContainsValue(M.MoviePathAndFileName.ToUpper)).ToList

            Return q2
        End Get
    End Property

    Public ReadOnly Property XBMC_to_MC_MoviePaths As Dictionary(Of String,String)
        Get
            Return _XBMC_to_MC_MoviePaths
        End Get
    End Property    

    ReadOnly Property MC_Only_MovieFolders As List(Of String)
        Get
            Dim q2 = From
                        M In MC_Only_Movies
                    Select
                        MovieFolderMappings.GetMC_MovieFolder(M.MoviePathAndFileName)
                    Distinct

            Return q2.ToList
        End Get
    End Property

    Public ReadOnly Property CanDeleteCachedImages As Boolean
        Get
            If Not _TriedCanDeleteCachedImages Then
                _TriedCanDeleteCachedImages = True
                _CanDeleteCachedImages = CanConnect And Directory.Exists(XbmcThumbnailsFolder)
            End If
            Return _CanDeleteCachedImages
        End Get
    End Property

    ReadOnly Property NumberOfMcNfosInFolder As Integer
        Get
            Dim count As Integer=0

            For Each fs_info As FileInfo In diMovieFolder.GetFiles("*.NFO")

                If Movie.IsMCNfoFile( fs_info.FullName ) then
                    count +=1
                End If
            Next

            Return count
        End Get
    End Property

    ReadOnly Property diMovieFolder As DirectoryInfo
        Get
            Return New DirectoryInfo(Path.GetDirectoryName(McMoviePath))
        End Get
    End Property

    'Public ReadOnly Property PercentageToUpdate As Integer
    '    Get
    '        Return Math.Round((MoviesToAdd.Count * 100) / XbmcJson.XbmcMovies.Count)

    '    End Get
    'End Property

    'Public ReadOnly Property PercentageNewMovieIdsToGet As Integer
    '    Get
    '        Return Math.Round((NewMovieIdsToGet.Count * 100) / XbmcJson.XbmcMovies.Count)

    '    End Get
    'End Property

    'ReadOnly Property MC_Only_Movies As List(Of String)
    '    Get
    '        Dim c As List(Of String)= (From
    '                    M In Parent.oMovies.MovieCache
    '                Select
    '                    M.MoviePathAndFileName).ToList

    '        Dim q2 As List(Of String) = (From
    '                    t In c
    '                Select
    '                    t
    '                Where
    '                    Not XBMC_to_MC_MoviePaths.Contains(t.ToUpper)).ToList
    '        Return q2
    '    End Get
    'End Property

#End Region  'ReadOnly Properties

#Region "States & Events"
    Public Enum S
        Any
        NotConnected
        Wf_XBMC_Movies_PreMap
        Wf_XBMC_Movies
        Wf_ReconnectOrNot
        Wf_XBMC_ConnectResult
        Wf_XBMC_Video_Removed
        Wf_XBMC_Video_Updated
        Wf_XBMC_Video_Id
        Wf_XBMC_Video_ScanFinished
        Wf_XBMC_All_Video_Details
        Ready
    End Enum

    Public Enum E

        ConnectReq
        TimeOut
        McMainBusyTimer_TimeOut
        MaxMovies_Idle_TimeOut
        Success
        Failure
        Yes
        No
        NoMoreScanFolderReqs
  '      WatchDogTimeOut
  '      GetNewMovieIds
        ScanFolder
        TurnOff
        FetchVideoInfo
        ResetErrorCount


        MC_Movie_Updated
     '   MC_Movie_New
        MC_Movie_Removed
        MC_ScanForNewMovies
        MC_ShutDownReq
 '       MC_FetchAllMovieDetails
        MC_Only_Movies
  '      MC_MaxMovieDetails

        MC_XbmcMcMovies
        MC_XbmcOnlyMovies

        MC_XbmcQuit
        MC_PurgeQ_Req

        XBMC_Video_Removed
        XBMC_Video_Updated
        XBMC_Video_ScanFinished
        XBMC_System_Quit
        XBMC_Unknown_Event

        Exception
        JSON_Error
        JSON_Abort
    End Enum
#End Region      'States & Events

#Region "Constructor"
    Sub New(Parent As Form1, Optional bw As BackgroundWorker = Nothing) '(Of Integer, Object))
        Me.Parent  = Parent
        Me.Q       = Form1.XbmcControllerQ
        Me.BufferQ = Form1.XbmcControllerBufferQ
        Me.Bw = bw

        Utilities.SafeDeleteFile( Path.Combine(My.Application.Info.DirectoryPath,Form1.XBMC_Controller_full_log_file ) )
        Utilities.SafeDeleteFile( Path.Combine(My.Application.Info.DirectoryPath,Form1.XBMC_Controller_brief_log_file) )
        log4net.Config.XmlConfigurator.Configure

 '       Ini_Timer(GetNewMovieIds_IdleTimer)
 '       Ini_Timer(FolderScan_IdleTimer)

  '      AddHandler GetNewMovieIds_IdleTimer.Elapsed         , AddressOf GetNewMovieIds_IdleTimer_Elapsed
  '      AddHandler FolderScan_IdleTimer    .Elapsed         , AddressOf FolderScan_IdleTimer_Elapsed


        Ini_Timer(TimeoutTimer)
        AddHandler TimeoutTimer.Elapsed, AddressOf Timer1sec_Elapsed

        Ini_Timer(McMainBusyTimer,3000)
        AddHandler McMainBusyTimer.Elapsed, AddressOf McMainBusyTimer_Elapsed

        

  '      Ini_Timer(MaxMovies_Idle_Timer)
  '      AddHandler MaxMovies_Idle_Timer.Elapsed, AddressOf MaxMovies_Idle_Timer_Elapsed

        'Ini_Timer(WatchDogTimer,10000)
        'AddHandler WatchDogTimer.Elapsed, AddressOf WatchDogTimer_Elapsed


'       AddHandler XbmcJson.xbmc.Library.Video.Updated, AddressOf XBMC_Video_Updated
        AddHandler Me.XbmcJson.xbmc.Library.Video.Removed      , AddressOf XBMC_Video_Removed
        AddHandler Me.XbmcJson.xbmc.Library.Video.ScanFinished , AddressOf XBMC_Video_ScanFinished
        AddHandler Me.XbmcJson.xbmc.System.Quit                , AddressOf XBMC_System_Quit
        AddHandler Me.XbmcJson.xbmc.Log                        , AddressOf XBMC_System_Log
        AddHandler Me.XbmcJson.xbmc.LogError                   , AddressOf XBMC_System_LogError
        AddHandler Me.XbmcJson.xbmc.Aborted                    , AddressOf XBMC_System_Aborted

        AddHandler Me.TransitionDeclined                       , AddressOf UnexpectedEvent
        AddHandler Me.TransitionCompleted                      , AddressOf HandleTransitionCompleted
        AddHandler Me.BeginDispatch                            , AddressOf BegnDispatch 
        AddHandler Me.ExceptionThrown                          , AddressOf ExceptionThrownHandler

        AddHandler Me.XbmcJson.XbmcMovies_OnChange             , AddressOf UpdateXBMC_to_MC_MoviePaths
        AddHandler Me.XbmcJson.XbmcMovies_OnChange             , AddressOf AutoSetDirectorySlash

        AddHandler Me.XbmcJson.MovieRemoved                    , AddressOf SetXbmcMaxMoviesDirty
        AddHandler Me.MaxXbmcMoviesChanged                     , AddressOf Handle_MaxXbmcMoviesChanged
        AddHandler Me.XbmcMcMoviesChanged                      , AddressOf Handle_XbmcMcMoviesChanged
        AddHandler Me.XbmcOnlyMoviesChanged                    , AddressOf Handle_XbmcOnlyMoviesChanged


        '  Me. (StateId.Connected).EntryHandler += EnterOff
        ' this[StateID.Off].ExitHandler += ExitOff


        SetupSubstates(S.Any, HistoryType.Shallow,  S.NotConnected,
                                                    S.Wf_XBMC_Movies_PreMap,
                                                    S.Wf_XBMC_Movies,
                                                    S.Wf_XBMC_ConnectResult,
                                                    S.Wf_XBMC_Video_Removed,
                                                    S.Wf_XBMC_Video_Updated,
                                                    S.Wf_XBMC_Video_Id,
                                                    S.Wf_XBMC_Video_ScanFinished,
                                                    S.Wf_XBMC_All_Video_Details,
                                                    S.Ready)


        AddTransition( S.Any                        , E.MC_ShutDownReq          , S.Any                        , AddressOf ShutDown             )
  '     AddTransition( S.Any                        , E.WatchDogTimeOut         , S.Wf_XBMC_ConnectResult      , AddressOf Connect              )
        AddTransition( S.Any                        , E.ResetErrorCount      , S.Any                        , AddressOf ResetErrorCount      )
        AddTransition( S.Any                        , E.XBMC_System_Quit        , S.NotConnected               , AddressOf Raise_XbmcQuit       )  
        AddTransition( S.Any                        , E.JSON_Abort              , S.Wf_XBMC_ConnectResult      , AddressOf Connect              )  

        AddTransition( S.NotConnected               , E.ConnectReq              , S.Wf_XBMC_ConnectResult      , AddressOf Connect              )
        AddTransition( S.NotConnected               , E.TimeOut                 , S.Wf_ReconnectOrNot          , AddressOf ReconnectOrNot       )

        AddTransition( S.Wf_ReconnectOrNot          , E.Yes                     , S.Wf_XBMC_ConnectResult      , AddressOf Connect              )
        AddTransition( S.Wf_ReconnectOrNot          , E.No                      , S.NotConnected               , AddressOf Ready                )

        AddTransition( S.Wf_XBMC_ConnectResult      , E.Failure                 , S.NotConnected               , AddressOf Start1SecTimer       )      
        AddTransition( S.Wf_XBMC_ConnectResult      , E.Success                 , S.Wf_XBMC_Movies             , AddressOf FetchMoviesInfo      )
        AddTransition( S.Wf_XBMC_ConnectResult      , E.JSON_Error              , S.Wf_XBMC_ConnectResult      , AddressOf Ready                )


'       AddTransition( S.Wf_XBMC_Movies_PreMap      , E.Failure                 , S.Wf_XBMC_Movies_PreMap      , AddressOf FetchMoviesInfo      )
'       AddTransition( S.Wf_XBMC_Movies_PreMap      , E.TimeOut                 , S.Wf_XBMC_Movies_PreMap      , AddressOf FetchMoviesInfo      )
''      AddTransition( S.Wf_XBMC_Movies_PreMap      , E.Success                 , S.Ready                      , AddressOf FetchMaxMovies       )
'       AddTransition( S.Wf_XBMC_Movies_PreMap      , E.Success                 , S.Ready                      , AddressOf AutoMapMovieFolders _

        AddTransition( S.Wf_XBMC_Movies             , E.Failure                 , S.Wf_XBMC_ConnectResult      , AddressOf ConnectAndRetry      )  
        AddTransition( S.Wf_XBMC_Movies             , E.TimeOut                 , S.Wf_XBMC_Movies             , AddressOf FetchMoviesInfo      )
        AddTransition( S.Wf_XBMC_Movies             , E.Success                 , S.Ready                      , AddressOf StopToTimer_SendMcOnlyMovies )
        AddTransition( S.Ready                      , E.McMainBusyTimer_TimeOut , S.Ready                      , AddressOf SendMcOnlyMovies     )


                                                                                                                                              
        AddTransition( S.Ready                      , E.MC_Movie_Updated        , S.Wf_XBMC_Video_Removed      , AddressOf RemoveVideoThenAdd   )
        AddTransition( S.Ready                      , E.XBMC_Video_Removed      , S.Ready                      , AddressOf IgnoreNonMcInitiatedEvent )

        AddTransition( S.Wf_XBMC_Video_Removed      , E.TimeOut                 , S.Ready                      , AddressOf Retry                )
        AddTransition( S.Wf_XBMC_Video_Removed      , E.Exception               , S.Wf_XBMC_ConnectResult      , AddressOf ConnectAndRetry      )  
        AddTransition( S.Wf_XBMC_Video_Removed      , E.XBMC_Video_Removed      , S.Ready                      , AddressOf DeleteCachedImages   _
                                                                                                               , AddressOf Ready                )


  '      AddTransition( S.Ready                      , E.MC_Movie_New            , S.Ready                      , AddressOf AddFolderToScan      _
  '                                                                                                             , AddressOf Ready                )

        AddTransition( S.Ready                      , E.MC_Movie_Removed        , S.Wf_XBMC_Video_Removed      , AddressOf RemoveVideo          )
        AddTransition( S.Ready                      , E.MaxMovies_Idle_TimeOut  , S.Ready                      , AddressOf FetchMaxMovies       _
                                                                                                               , AddressOf Ready                )
                                                                                                                                       
#If BatchMode Then
        AddTransition( S.Ready                      , E.ScanFolder              , S.Ready                      , AddressOf AddFolderToBatchScan )
        AddTransition( S.Ready                      , E.NoMoreScanFolderReqs    , S.Wf_XBMC_Video_ScanFinished , AddressOf SendBatchScan        )
#Else
        AddTransition( S.Ready                      , E.ScanFolder              , S.Wf_XBMC_Video_ScanFinished , AddressOf ScanFolder           )
#End If



'       AddTransition( S.Wf_XBMC_Video_ScanFinished , E.TimeOut                 , S.Wf_XBMC_Video_ScanFinished , AddressOf AddFetchVideoInfo    ) '...ScanFolder...    )
        AddTransition( S.Wf_XBMC_Video_ScanFinished , E.TimeOut                 , S.Ready                      , AddressOf Retry                ) 
        AddTransition( S.Wf_XBMC_Video_ScanFinished , E.XBMC_Video_ScanFinished , S.Ready                      , AddressOf AddFetchVideoInfo    )
        AddTransition( S.Wf_XBMC_Video_ScanFinished , E.Exception               , S.Wf_XBMC_ConnectResult      , AddressOf ConnectAndRetry      ) 
        AddTransition( S.Wf_XBMC_Video_ScanFinished , E.Success                 , S.Wf_XBMC_ConnectResult      , AddressOf StopTimeoutTimer     ) 
       
        AddTransition( S.Ready                      , E.FetchVideoInfo          , S.Wf_XBMC_Movies             , AddressOf FetchMoviesInfo      )
        AddTransition( S.Ready                      , E.XBMC_Video_ScanFinished , S.Ready                      , AddressOf Ignore               )
                                                                                                                                              
 '      AddTransition( S.Ready                      , E.MC_FetchAllMovieDetails , S.Ready                      , AddressOf FetchMaxMovies       )
        AddTransition( S.Ready                      , E.MC_ScanForNewMovies     , S.Ready                      , AddressOf ScanNewMovies        )            
                                                                                                                               
                                                                                                                                             
       'AddTransition( S.Ready                      , E.MC_Movie_New            , S.Wf_XBMC_Video_ScanFinished , AddressOf AddMovie             )
       'AddTransition( S.Wf_XBMC_Video_Removed      , E.XBMC_Video_Removed      , S.Wf_XBMC_Video_ScanFinished , AddressOf AddMovie             )
       'AddTransition( S.Wf_XBMC_Video_Removed      , E.MC_Movie_Updated        , S.Wf_XBMC_Video_Removed      , AddressOf AddMovie             )
       'AddTransition( S.Ready                      , E.GetNewMovieIds          , S.Ready                      , AddressOf GetNewMovieIds       )
                                                                                                                                              
        Initialize(S.NotConnected)
        'If Pref.XbmcLinkReady Then Q.Write(E.ConnectReq, PriorityQueue.Priorities.medium)   
    End Sub                                                                                                                                  
#End Region          'Constructor

#Region "Main"
    Sub Go()
        While Not ShutDownRequested

            If Q.Count = 0 And Me.CurrentStateID = S.Ready And BufferQ.Count > 0 Then
                Dim Evt As BaseEvent = BufferQ.Read() 


                AppendLog(EnumLogMode.Full ,"Unbuffering Event : [" & Evt.Info & "]")

                AppendLog(EnumLogMode.Brief,"State : [" + CurrentStateID.ToString.PadRight(LongestEnumStateName) + "] UnBuffering : [" + Evt.E.ToString.PadRight(LongestEnumEventName) + "] Args   : [" + Evt.Args.ToString + "]")

                ProcessEvent(Evt)
            Else
                'If Q.Count = 0 And Me.CurrentStateID = S.Ready And BufferQ.Count = 0 And XbmcMaxMoviesDirty And Not MaxMovies_Idle_Timer.Enabled Then
                '    StartMaxMovies_Idle_Timer
                'Else
                '    MaxMovies_Idle_Timer.Stop
                'End If
                'ProcessEvent(Q.Dequeue)
                ProcessEvent(Q.Read())
            End If

        End While
    End Sub

    Sub ProcessEvent(Evt As BaseEvent)

        If Evt.E = E.MC_PurgeQ_Req Then
            BufferQ.Delete(E.MC_Movie_Updated)
            BufferQ.Delete(E.MC_ScanForNewMovies)
            BufferQ.Delete(E.ScanFolder)
        End If

        If Evt.E.ToString.IndexOf("MC_") = 0 And CurrentStateID <> S.Ready Then

            If BufferQ.Exists(Evt) Then
                AppendLog(EnumLogMode.Full,"Discarding duplicate MC request : [" + Evt.CompareAs + "]")
                AppendLog(EnumLogMode.Brief,"State : [" + CurrentStateID.ToString.PadRight(LongestEnumStateName) + "] Event : [" + Evt.CompareAs.PadRight(LongestEnumEventName) + "] Action : [Discarding duplicate MC request]")
            Else
                AppendLog(EnumLogMode.Full,"Buffering MC request : [" + Evt.Info + "] while in State : [" + CurrentStateID.ToString + "]")
                AppendLog(EnumLogMode.Brief,"State : [" + CurrentStateID.ToString.PadRight(LongestEnumStateName) + "] Buffering   : [" + Evt.E.ToString.PadRight(LongestEnumEventName) + "] Args   : [" + Evt.Args.ToString + "]")

                BufferQ.Write( New BaseEvent(Evt.E, Evt.Args) )
            End If

            If CurrentStateID = S.NotConnected Then
                Q.Write(E.ConnectReq, PriorityQueue.Priorities.medium)
            End If
                
            Return
        End If

        LastState = Me.CurrentStateID

        AppendLog(EnumLogMode.Full,"Dispatching Event : [" & Evt.Info & "]")

        If Evt.E = E.ScanFolder or Evt.E=E.MC_Movie_Updated or Evt.E=E.MC_Movie_Removed Then
            LastEvent.Assign(Evt)
        End If

        Dispatch(Evt.E, Evt.Args)
    End Sub
#End Region                 'Main

#Region "Event Handlers"

    Sub UnexpectedEvent(sender As Object, evt As TransitionEventArgs(Of S, E, EventArgs))

        If evt.EventID=E.ConnectReq Then
            Return
        End If

        If  evt.EventID=e.McMainBusyTimer_TimeOut Then
            Start_McMainBusyTimer_Timer
            Return
        End If

        If evt.EventID=e.XBMC_Video_ScanFinished Then
            LogWarning("UnexpectedEvent", "State : [" + evt.SourceStateID.ToString + "] Ignoring : [" + evt.EventID.ToString + "]",evt)
            Return
        End If

        If evt.EventID.ToString.IndexOf("MC_") = 0 Or evt.EventID=E.ScanFolder Then

            AppendLog("Buffering request : [" + evt.EventID.ToString + "] while in State : [" + evt.SourceStateID.ToString + "]")
            BufferQ.Write( New BaseEvent(evt.EventID, evt.EventArgs) )

            If evt.SourceStateID = S.NotConnected Then
                Q.Write(E.ConnectReq, PriorityQueue.Priorities.medium)
            End If
            Return
        End If

'       LogError("UnexpectedEvent", "State : [" + evt.SourceStateID.ToString + "] Missing event handler for : [" + evt.EventID.ToString + "]",evt)
        LogWarning("UnexpectedEvent", "State : [" + evt.SourceStateID.ToString + "] Ignoring : [" + evt.EventID.ToString + "]",evt)
    End Sub

    Sub ExceptionThrownHandler(sender As Object, evt As TransitionErrorEventArgs(Of S, E, EventArgs))
        LogError(EnumLogMode.Full,"ExceptionThrownHandler", evt.Error.InnerException.Message, evt)
        Q.Write(e.Exception,PriorityQueue.Priorities.critical)
    End Sub

    Sub HandleTransitionCompleted(sender As Object, e As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo(EnumLogMode.Full,"Transition Completed",e)
    End Sub

    Sub BegnDispatch(sender As Object, e As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo(EnumLogMode.Full,"Transition Started",e)
        LastArgs = e
    End Sub 

    Sub Timer1sec_Elapsed(ByVal sender As Object, ByVal ev As Timers.ElapsedEventArgs)

        sender.Stop()
        AppendLog(EnumLogMode.Full,"Timer Elapsed")

        Q.Write(E.TimeOut,PriorityQueue.Priorities.high)
    End Sub

    Sub McMainBusyTimer_Elapsed(ByVal sender As Object, ByVal ev As Timers.ElapsedEventArgs)

        AppendLog(EnumLogMode.Full,"Mc Main Busy Timer Elapsed")

        Q.Write(E.McMainBusyTimer_TimeOut,PriorityQueue.Priorities.low)
    End Sub


    Sub MaxMovies_Idle_Timer_Elapsed(ByVal sender As Object, ByVal ev As Timers.ElapsedEventArgs)

        sender.Stop()
        AppendLog(EnumLogMode.Full,"Idle Timer Elapsed")

        Q.Write(E.MaxMovies_Idle_TimeOut,PriorityQueue.Priorities.low)
    End Sub

    Sub XBMC_System_Log(sender As Object, evt As XbmcJsonRpcLogEventArgs)
        AppendLog(EnumLogMode.Full,"XbmcJsonRpc : " & evt.Message)
    End Sub

    Sub XBMC_System_LogError(sender As Object, evt As XbmcJsonRpcLogErrorEventArgs)

        Try
            If evt.Message="Could not open a connection to XBMC" Then
                Q.Write(E.XBMC_System_Quit,PriorityQueue.Priorities.high)
                Return
            End If
        Catch
        End Try

        LogWarning(EnumLogMode.Full,"XbmcJsonRpc - Error received",evt,LastArgs)

        Q.Write(e.JSON_Error,PriorityQueue.Priorities.critical)
    End Sub

    Sub XBMC_System_Aborted(sender As Object, jeea As XbmcJsonRpcLogErrorEventArgs)
        LogWarning(EnumLogMode.Full,"XbmcJsonRpc - Abort received",jeea,LastArgs)
        Q.Write(e.JSON_Abort,PriorityQueue.Priorities.critical)
    End Sub

    Sub XBMC_Video_Updated(sender As Object, ea As EventArgs)
        Q.Write(E.XBMC_Video_Updated,PriorityQueue.Priorities.high)
    End Sub

    Sub XBMC_Video_Removed(sender As Object, ea As EventArgs)

#If GenPseudoErrors Then
        ScanRemoved_Count += 1
        If ScanRemoved_Count mod ScanRemoved_MissRate = 0 Then
            log.Debug("Faking missed Video_Removed event")
            Return
        End If
#End If

        Q.Write(E.XBMC_Video_Removed,PriorityQueue.Priorities.high)
    End Sub

    Sub XBMC_Video_ScanFinished(sender As Object, ea As EventArgs)

#If GenPseudoErrors Then
        ScanFinished_Count += 1
        If ScanFinished_Count mod ScanFinished_MissRate = 0 Then
            log.Debug("Faking missed ScanFinished event")
            Return
        End If
#End If

        Q.Write(E.XBMC_Video_ScanFinished,PriorityQueue.Priorities.high)
    End Sub

    Sub XBMC_System_Quit(sender As Object, ea As EventArgs)
        Q.Write(E.XBMC_System_Quit,PriorityQueue.Priorities.high)
    End Sub

    Sub XBMC_UnknownEvent(sender As Object, ea As XbmcJsonRpcUnknownEventArgs)
        Q.Write(New BaseEvent(E.XBMC_Unknown_Event, New UnknownEventArgs(ea.Event,PriorityQueue.Priorities.high)))
    End Sub

    Sub UpdateXBMC_to_MC_MoviePaths(XbmcMovies As List(Of MinXbmcMovie))

        Dim q2 = XbmcJson.XbmcMovies.ToDictionary(Function(p) p.file.ToUpper,Function(p) MovieFolderMappings.GetMC_MoviePath(p.file))

        _XBMC_to_MC_MoviePaths = q2
    End Sub  


    Sub AutoSetDirectorySlash(XbmcMovies As List(Of MinXbmcMovie))
        Dim backSlashes    As Integer = 0
        Dim forwardSlashes As Integer = 0
        Dim file           As String  = ""

        Try
            file = XbmcJson.XbmcMovies.FirstOrDefault.file.Replace("stack://","")

            backSlashes    = file.Split("\").Length -1 
            forwardSlashes = file.Split("/").Length -1 

            Pref.XBMC_Link_Use_Forward_Slash =(forwardSlashes>backSlashes)
        Catch
        End Try

        RemoveHandler Me.XbmcJson.XbmcMovies_OnChange, AddressOf AutoSetDirectorySlash
    End Sub  


    Sub Handle_MaxXbmcMoviesChanged

        XbmcMaxMoviesDirty = False
        _xbmcMovies = MaxXbmcMovies.ToDictionary(Function(x) MovieFolderMappings.GetMC_MoviePath(x.file).ToUpper)
                                                                        
        _mcMovies   = Parent.oMovies.MovieCache.ToDictionary(Function(x) x.MoviePathAndFileName.ToUpper)

        XbmcMcMovies = (From xb In _xbmcMovies
                         From mc In _mcMovies
                         Where mc.Key=xb.key   ).ToDictionary(Function(x) x.mc.Key, Function(x) x.xb.Value)

        XbmcOnlyMovies = (From xb In _xbmcMovies
                        Where Not _xbmcMcMovies.ContainsKey(xb.key)
                        Select xb.Value
                        ).ToList

        'ReportProgress("Number of XBMC only movies : [" + _xbmcOnlyMovies.count.ToString + "]",LastArgs)
    End Sub

    Sub Handle_XbmcMcMoviesChanged
        LogInfo("Passing back XBMC-MC movies")

        Dim msg = New XBMC_MC_Movies_EventArgs(XbmcMcMovies)

        ReportProgress(E.MC_XbmcMcMovies,msg)                
    End Sub

    Sub Handle_XbmcOnlyMoviesChanged
        LogInfo("Passing back XBMC only movies")

        Dim msg = New XBMC_Only_Movies_EventArgs(XbmcOnlyMovies)

        ReportProgress(E.MC_XbmcOnlyMovies,msg)      
    End Sub


#End Region       'Event handlers

#Region "Actions"

    Sub Start1SecTimer(sender As Object, e As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo("Starting 1 second timer")
        StartTimeoutTimer(1000)
    End Sub 

    Sub ShutDown(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo("Shutting down...")
        ShutDownRequested = True
    End Sub

    Sub ReconnectOrNot(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        LogInfo("Reconnect or not...")
        Q.Write(IIf(Pref.XBMC_Link, E.Yes, E.No), PriorityQueue.Priorities.high )
    End Sub

    Sub Connect(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        LogInfo("Connecting...")

        '      Send     (   IIf(XbmcJson.Open,E.Success,E.Failure) )
        '       Q.Enqueue(1, New CompleteEvent(IIf(XbmcJson.Open,E.Success,E.Failure)) )
        Q.Write(IIf(XbmcJson.Open, E.Success, E.Failure), PriorityQueue.Priorities.high )
    End Sub

    Sub ConnectAndRetry(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        LogInfo("Connect & retry...")
        StopTimeoutTimer
        Retry  (sender,args)

        XbmcJson.xbmc.Close

        Connect(sender,args)
    End Sub

    Sub AddFetchVideoInfo(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        StopTimeoutTimer
        Dim got = From i In BufferQ.View Where i.E = E.FetchVideoInfo 
      
        If got.Count=0 Then
            LogInfo("AddFetchVideoInfo")
            BufferQ.Write(E.FetchVideoInfo,PriorityQueue.Priorities.lowest)
        End If
    End Sub

    Sub FetchMoviesInfo(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        AssignLastEvent(args)

        LogInfo("Fetching movies info...")
        StartTimeoutTimer(5000)
        Q.Write(IIf(XbmcJson.GetXbmcMovies, E.Success, E.Failure), PriorityQueue.Priorities.medium )
    End Sub

    Sub FetchMaxMovies(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        LogInfo("Fetching full movie details for all movies...")

        MaxXbmcMovies = XbmcJson.xbmc.Library.Video.GetMaxXbmcMovies(Pref.XBMC_MC_CompareFields.Get_Xbmc_Fields)

  '      Dim msg = New XBMC_MaxMovies_EventArgs(MaxXbmcMovies)
  '      ReportProgress(E.MC_MaxMovieDetails,msg)
    End Sub

    
  

    Sub StopToTimer_SendMcOnlyMovies(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        StopTimeoutTimer

        SendMcOnlyMovies(sender,args)
    End Sub


    Sub SendMcOnlyMovies(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        'Don't bother if main is mid scraping or otherwise busy
        If Form1.ProgState <> Form1.ProgramState.Other Then 
            Start_McMainBusyTimer_Timer
            Return
        End If

        LogInfo("Passing back Mc only movies")

        Dim data As List(Of ComboList) = New List(Of ComboList)

        data.AddRange(MC_Only_Movies)

        Dim msg = New ComboList_EventArgs(data)

        ReportProgress(E.MC_Only_Movies,msg)
    End Sub

    Sub IgnoreNonMcInitiatedEvent(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo("Ignoring non-MC initiated event")
    End Sub

    Sub Ignore(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo("Ignoring error as already handled elsewhere")
    End Sub

    Sub Ready(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        LogInfo("Ready & waiting...")
        StopTimeoutTimer
  '      WatchDogTimer.Stop
    End Sub

    Sub RemoveVideo(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        DecodeNfoEventArgs(args)

        GetCachedUrls

        Dim Title       As String  = ""
        Dim xbmcMovieId As Integer = -1

        If IsNothing(XbMoviePath) Then
            AppendLog(Error_Prefix + "Failed to map [" + McMoviePath + "] to XBMC folders. Check your XBMC_MC_FolderMappings in Config.XML" )
            Q.Write(E.XBMC_Video_Removed,PriorityQueue.Priorities.high)
            Return
        End If

        Try
            Title = XbmcJson.GetMinXbmcMovie(XbMoviePath).title
        Catch
        End Try

        If Title<>"" Then
            LogInfo("Removing : " + Title)
            
            Try
                'xbmcMovieId = XbmcJson.GetMinXbmcMovie(XbMoviePath).movieid

                Dim movies As List(Of MinXbmcMovie) = XbmcJson.xbmc.Library.Video.GetMinXbmcMovies(title)

                If movies.Count=1 Then
                    xbmcMovieId = movies(0).movieid
                Else
                    If movies.Count>1 Then
                        xbmcMovieId = (From x In movies Where MovieFolderMappings.GetMC_MoviePath(x.file).ToUpper=McMoviePath.ToUpper Select x.movieid).First
                    End If
                End If
            Catch
            End Try
        End If

        If xbmcMovieId > -1 Then
            XbmcJson.xbmc.Library.Video.RemoveMovie(xbmcMovieId)

            StartTimeoutTimer(5000)

            XbmcJson.RemoveXbmcMovie(XbMoviePath)
        Else
            'ErrorCount = ErrorCount + 1
            LogInfo("Failed to find movieid for [" & McMoviePath & "] - Probably new to XBMC") 'This can happen if not already in XBMC
            Q.Write(E.XBMC_Video_Removed,PriorityQueue.Priorities.high)
        End If
    End Sub

    Sub RemoveVideoThenAdd(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        RemoveVideo    (sender,args)
        AddFolderToScan(sender,args)
    End Sub

    Sub ResetErrorCount(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        ErrorCount   = 0
        WarningCount = 0
    End Sub

    Sub Raise_XbmcQuit(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        XbmcJson.xbmc.Close
        LogInfo("XBMC is not longer running. Jobs Queue will be purged")
        Reset
        ReportProgress(E.MC_XbmcQuit)      
   End Sub

    Sub Retry(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        
        If LastEvent.Retries > 0 Then 
            LogInfo("Event failed")
            Return
        End If


        Dim le As BaseEvent = New BaseEvent

        le.Assign(LastEvent)
        le.Retries = le.Retries + 1

        LogInfo("Resubmitting last event : [" + le.E.ToString +"] Retry number : [" + le.Retries.ToString + "]")
        BufferQ.Write(le)
    End Sub

    Sub ScanFolder(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        Dim ea As FolderEventArgs = args.EventArgs

        Dim scanFolder = ea.Folder.FormatXbmcPath


        Dim mif As Integer = 1


        If Not MoviesInFolder.ContainsKey(scanFolder) Then
            MoviesInFolder.Add(scanFolder,1)
            'LogError("ScanFolder","MoviesInFolder was missing : [" + scanFolder + "]", args)
        End If

        Try
            mif = MoviesInFolder(scanFolder)
        Catch ex As Exception
            LogError("ScanFolder","[" & ex.Message & "] was thrown looking up : [" + scanFolder + "] in MoviesInFolder", args)
        End Try

        Dim Interval = 5000 + (mif*1000)

        MoviesInFolder(scanFolder)=0

        Dim Total = Aggregate c In MoviesInFolder Into Sum(c.Value)

        If Total = 0 Then 
            MoviesInFolder.Clear
        End If

        '
        ' Scanning named source folders doesn't work because it doesn't scan the sub-folders [XBMC bug]
        ' See: http://forum.xbmc.org/showthread.php?tid=158772
        ' MilhouseVH's comments describes this issue exactly.
        '

        'Dim numScanFolderReqs = (From i In BufferQ.View Where i.E = E.ScanFolder).Count 

        ''Dim numDiffFolders    = (From i In Q.View Where i.E = E.ScanFolder _
        ''                        Select 
        ''                            Folder=DirectCast(i.Args,FolderEventArgs).Folder 
        ''                        Group By 
        ''                            Folder Into Num=Count).Count


        'If numScanFolderReqs>10 Then
          
        '    If scanFolder.IndexOf(Path.DirectorySeparatorChar) > 0 Then

        '        numScanFolderReqs = NumMatchingFolders(Directory.GetParent(scanFolder).FullName)
                

        '        If numScanFolderReqs>10  Then

        '            Interval = Math.Min( (numScanFolderReqs*1000)+4000, 120000 )

        '            scanFolder = Directory.GetParent(scanFolder).FullName
        '            BufferQ.Delete(New BaseEvent(E.ScanFolder, New FolderEventArgs(scanFolder,PriorityQueue.Priorities.low)))
        '        End If

        '    End If

        'End If


        'If numFolders>10 Then
          
        '    scanFolder = Nothing
        '    Q      .Delete(E.ScanFolder)
        '    BufferQ.Delete(E.ScanFolder)
        '    ReportProgress("Scanning folders...",args)
        'Else
        '    ReportProgress("Scanning folder: " + scanFolder,args)
        'End If

        'BufferQ.Delete(New BaseEvent(E.ScanFolder, New FolderEventArgs(scanFolder,PriorityQueue.Priorities.low)))


        StartTimeoutTimer(Interval)

        LogInfo("Scanning folder: " + scanFolder)
        XbmcJson.xbmc.Library.Video.AddMovies(scanFolder)

        '     NewMovieIdsToGet.Add(ea)
        '     Restart_Timer(GetNewMovieIds_IdleTimer)
    End Sub

    Sub ScanNewMovies(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        Try
            For Each movie In MC_Only_Movies

                Dim evt As New BaseEvent

                evt.E    = XbmcController.E.MC_Movie_Updated
                evt.Args = New VideoPathEventArgs(movie.MoviePathAndFileName, PriorityQueue.Priorities.medium)

                Q.Write(evt)
            Next
        Catch ex As Exception
            LogError("ScanNewMovies",ex.Message,args)
        End Try
        
    End Sub

    Sub DeleteCachedImages(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))
        
        StopTimeoutTimer

        If Not Pref.XBMC_Delete_Cached_Images Then
            LogInfo("Skipping delete orphaned movie images from thumbnail folder")
            Return
        End If

        LogInfo("Deleting orphaned movie images from thumbnail folder")

        If IsNothing(_dtCachedUrls) OrElse _dtCachedUrls.Rows.Count=0 or _dtCachedUrls.Rows.Count>4 Then 
            AppendLog("Skipping cached file delete as expected 1-4 rows to be matched, but actually matched : [" + _dtCachedUrls.Rows.Count.ToString + "]")
            _dtCachedUrls = Nothing
            Return
        End If

        XbmcTexturesDb.Open

        For Each row In _dtCachedUrls.Rows
            Dim filePath As String = Path.Combine(XbmcThumbnailsFolder,row("cachedurl").ToString.Replace("/","\"))

            If File.Exists(filePath) Then
                LogInfo("Deleting : [" + filePath + "]")
                Try
                    File.Delete(filePath)
                Catch ex As Exception
                    LogError("DeleteCachedImages", "Failed to delete thumbnail : [" + filePath + "] - Delete file access needed! Error : [" & ex.Message & "]",args)
                End Try
            Else
                LogInfo("Thumbnail [" + filePath + "] not found")
            End If

            DbUtils.ExecuteNonQuery(XbmcTexturesDb, "Delete from texture where id=" + row("id").ToString)
        Next

        XbmcTexturesDb.Close

        _dtCachedUrls = Nothing
    End Sub

    Sub SetXbmcMaxMoviesDirty        
        XbmcMaxMoviesDirty = True
    End Sub

#Region "Batch mode"
#If BatchMode Then
    Sub AddFolderToBatchScan(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        LogInfo("Adding folder to batch scan...",args)

        Dim ea As FolderEventArgs = args.EventArgs

        BatchScanFolders.Add( XbmcJson.xbmc.Library.Video.GetCall_AddMovies(ea.Folder).ToString )

        If (From i In BufferQ.View Where i.E = E.ScanFolder).Count=0 Then
            Q.Write(E.NoMoreScanFolderReqs)
        End If
    End Sub

    Sub SendBatchScan(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

        LogInfo("Sending batch scan for "+ BatchScanFolders.Count.ToString + " folders...",args)

        Dim request As String=""
        Dim timeout = 4000

        For Each job In BatchScanFolders
            request += job+","
            timeout += 1000
        Next

        request = Left(request,Len(request)-1)

        StartTimeoutTimer(timeout)

        If BatchScanFolders.Count>1 Then
            request = "[" + request + "]"
        End If

        log.Debug(request)

        Try
            XbmcJson.xbmc.Library.client.SendBatchCall(request)
        Catch ex As Exception
            log.Debug(ex.Message)
        End Try
        BatchScanFolders.Clear
    End Sub

#End If
#End Region

    'Sub FetchMovieInfo(sender As Object, args As TransitionEventArgs(Of S, E, EventArgs))

    '    ReportProgress("Fetching movie info for : " + Title,args)

    '    Q.Write(IIf(XbmcJson.UpdateXbmcMovies(Title), E.Success, E.Failure), PriorityQueue.Priorities.medium )
    'End Sub

#End Region              'Actions

#Region "WIP"
    'Sub Handle_MaxXbmcMoviesChanged_Old

    '    XbmcMaxMoviesDirty = False

    '    _Different.Clear
    '    _XBMC_Only.Clear


    '    Dim result        As Boolean = False
    '    Dim count         As Integer = 0
    '    Dim mediaFileName As String

    '    For Each xbMovie In MaxXbmcMovies

    '        mediaFileName = MovieFolderMappings.GetMC_MoviePath(xbMovie.file).ToUpper

    '        Dim movieCache As ComboList = (From m In Parent.oMovies.MovieCache Where m.MoviePathAndFileName.ToUpper=mediaFileName).FirstOrDefault

    '        If IsNothing(movieCache) And Not _XBMC_Only.Contains(mediaFileName) Then
    '            _XBMC_Only.Add(mediaFileName)

    '            Dim cl As New ComboList
    '            Dim xb As New XbmcProps
    '            cl.XbmcProps = xb

    '            'For Each item In Pref.XBMC_MC_CompareFields.Get_Xbmc_Fields.ToList

    '            'Next
 
    '            Parent.oMovies.MovieCache.Add(cl)
    '        Else
    '            If IsDifferent(xbMovie,movieCache) Then

    '                If Not _Different.Contains(movieCache) Then
    '                    _Different.Add(movieCache)
    '                End If
    '            End If
    '        End If
    '    Next

    '    ReportProgress("Number of XBMC only movies : [" + _XBMC_Only.count.ToString + "]",LastArgs)
    '    ReportProgress("Number of XBMC vs MC movies that have different meta data : [" + _Different.count.ToString + "]",LastArgs)
    'End Sub

    Function IsDifferent(xbMovie As MaxXbmcMovie, mcMovie As ComboList) As Boolean

    End Function

    Function IsDifferentOld(xbMovie As MaxXbmcMovie, mcMovie As ComboList) As Boolean

        Dim oMovie          As Movie

        Try

            oMovie  = New Movie(Parent.oMovies,mcMovie.fullpathandfilename)

            oMovie.LoadNFO


            'public List<string> genre         ; //  genre

            'public List<string> director      ; //  Not in Combolist

            'public      string  lastPlayed    ; //  NA

            'public List<string> writer        ; //  Not in Combolist
            'public List<string> studio        ; //  Not in Combolist
            'public List<string> country       ; //  Not in Combolist

            'public List<string> tag           ; //  tag

            'public      string  dateadded     ; //  createdate?
            'public List<Cast  > cast          ; //  Not in Combolist
            'public List<string> showlink      ; //  NA
            'public      string  streamdetails ; //  [Resolution]
            'public      string  fanart        ; //  Not in Combolist
            'public      string  thumbnail     ; //  Not in Combolist
            'public      string  resume        ; //  NA
            'public      int     setid         ; //  NA
            'public      Art     art           ; //  Not in Combolist

            Dim fmb As BasicMovieNFO = oMovie.ScrapedMovie.fullmoviebody

            If xbMovie.title                                          <> fmb.title                                      Then Return True
            If xbMovie.year                                           <> fmb.year                                       Then Return True
            If xbMovie.rating.ToString                                <> fmb.rating                                     Then Return True
            If xbMovie.tagline                                        <> fmb.tagline                                    Then Return True
            If xbMovie.plot                                           <> fmb.plot                                       Then Return True
            If xbMovie.plotoutline                                    <> fmb.outline                                    Then Return True
            If xbMovie.originalTitle                                  <> fmb.originalTitle                              Then Return True
            If xbMovie.playCount                                      <> fmb.playCount                                  Then Return True
            If xbMovie.mpaa                                           <> fmb.mpaa                                       Then Return True
            If xbMovie.imdbnumber                                     <> fmb.imdbid                                     Then Return True
            If (xbMovie.runtime\60).ToString                          <> fmb.runtime .Replace(" min"  ,"")              Then Return True
            If xbMovie.set                                            <> fmb.SetName.Replace("-None-","")               Then Return True
            If xbMovie.top250.ToString                                <> fmb.top250                                     Then Return True
            If xbMovie.votes.ToString                                 <> fmb.votes.Replace(",","")                      Then Return True
            If xbMovie.sorttitle                                      <> fmb.sortorder                                  Then Return True
            If MovieFolderMappings.GetMC_MoviePath(xbMovie.trailer)   <> oMovie.ActualTrailerPath.ToUpper               Then Return True


            'If xbMovie. <> fmb. Then Return True
            'If xbMovie. <> oMovie. Then Return True



        Catch ex As Exception
            LogError("IsDifferent", ex.Message,LastArgs)
        End Try

'Parent.oMovies.MovieCache

        Return False
    End Function
#End Region                  'WIP

End Class


