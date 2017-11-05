Imports System.Reflection
Imports Alphaleonis.Win32.Filesystem

Public Class ucGenPref_XbmcLink

#Region "Properties"
    Private MovieFolderMappings As XBMC_MC_FolderMappings = New XBMC_MC_FolderMappings

    ReadOnly Property Changed As Boolean
        Get
            Return  cbXBMC_Active               .Checked <> Pref.XBMC_Active               OrElse
                    MovieFolderMappings.Changed(Pref.XBMC_MC_MovieFolderMappings)          OrElse
                    tbXBMC_Address              .Text    <> Pref.XBMC_Address              OrElse
                    tbXBMC_Port                 .Text    <> Pref.XBMC_Port                 OrElse
                    tbXBMC_Username             .Text    <> Pref.XBMC_Username             OrElse
                    tbXBMC_Password             .Text    <> Pref.XBMC_Password             OrElse
                    cbXBMC_Delete_Cached_Images .Checked <> Pref.XBMC_Delete_Cached_Images OrElse
                    tbXBMC_UserdataFolder       .Text    <> Pref.XBMC_UserdataFolder       OrElse
                    tbXBMC_TexturesDb           .Text    <> Pref.XBMC_TexturesDb           OrElse
                    tbXBMC_ThumbnailsFolder     .Text    <> Pref.XBMC_ThumbnailsFolder
        End Get
    End Property
#End Region         'Properties

#Region "Event Handlers"
    Private Sub llXBMC_MovieFolderMappings_LinkClicked( sender As Object,  e As LinkLabelLinkClickedEventArgs) Handles llXBMC_MovieFolderMappings.LinkClicked 
        Dim frm As new frmConfigureXBMC_MC_Folders

        frm.Init(MovieFolderMappings)
        frm.ShowDialog

        SetEnabledStates
    End Sub

    Private Sub btnSelect_XBMC_UserdataFolder_Click( sender As Object,  e As EventArgs) Handles btnSelect_XBMC_UserdataFolder.Click 
        Dim ofd As New FolderBrowserDialog

        ofd.RootFolder          = Environment.SpecialFolder.Desktop
        ofd.Description         = "Locate 'XBMC\userdata' folder"
        ofd.ShowNewFolderButton = False

        If Directory.Exists(tbXBMC_UserdataFolder.Text) Then
            ofd.SelectedPath = tbXBMC_UserdataFolder.Text
        Else
            ofd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

            If Directory.Exists(Path.Combine(ofd.SelectedPath,"XBMC\userdata")) Then
                ofd.SelectedPath = Path.Combine(ofd.SelectedPath,"XBMC\userdata")
            End If
        End If

        'Dim type As Type = ofd.[GetType]

        'Dim fieldInfo As Reflection.FieldInfo = type.GetField("rootFolder", BindingFlags.NonPublic Or BindingFlags.Instance)

        'fieldInfo.SetValue(ofd,DirectCast(18, Environment.SpecialFolder))

        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then 
            tbXBMC_UserdataFolder.Text = ofd.SelectedPath
        End If

    End Sub

    Private Sub btnGeneralPrefsSaveChanges_Click( sender As Object,  e As EventArgs) Handles btnGeneralPrefsSaveChanges.Click

        UpdatePreferences

        Pref.ConfigSave()
        SetEnabledStates
    End Sub

    Private Sub AnyFieldChanged(sender As Object,  e As EventArgs) Handles  cbXBMC_Active    .CheckedChanged, tbXBMC_Address         .TextChanged, tbXBMC_Port                .TextChanged   , _
                                                                            tbXBMC_Username  .TextChanged   , tbXBMC_Password        .TextChanged, tbXBMC_UserdataFolder      .TextChanged   , _
                                                                            tbXBMC_TexturesDb.TextChanged   , tbXBMC_ThumbnailsFolder.TextChanged, cbXBMC_Delete_Cached_Images.CheckedChanged
        SetEnabledStates
    End Sub
 

    Private Sub btnValidate_Click( sender As Object,  e As EventArgs) Handles btnValidate.Click
        ValidateSettings
    End Sub

    Private Sub btnUndo_Click( sender As Object,  e As EventArgs) Handles btnUndo.Click
        AssignFormFields
        SetEnabledStates
    End Sub

#End Region     'Event Handlers

#Region "Main Subs"
    Public Sub Pop
        ClearImages
        AssignFormFields
        SetEnabledStates

        tbDialogue.Clear
        AppendDialogue("Hints:")
        AppendDialogue("")
        AppendDialogue(" - Delete file access needed in the Thumbnails folder for fanart & poster updates to work.")
        AppendDialogue(" - Updates are quickest in Weather view")
        AppendDialogue(" - Updates are slowest in Movies view")
        AppendDialogue(" - To synchronize multiple movies, select them in the movies list and rt-click to bring up the context menu, then select 'Sync to XBMC'")
        AppendDialogue(" - Clearing the 'Delete cached images' checkbox prevents fanart & poster updates from working, as XBMC will use it's cached images in preference to any new images. Clear this if you prefer to scrape your images from within XBMC.")
        AppendDialogue(" - With Movies the poster image doesn't always appear updated. To fix this, you may get away with coming out of the movies page and going back in, failing that come out of XBMC altogether and go back in")
        AppendDialogue(" - Scroll down to the bottom of the General filter for XBMC specific filters - currently only one 'Missing from XBMC', but more coming once we're happy the bugs are out of this first implementation")
        AppendDialogue(" - For any movie listed in 'Missing from XBMC' check the following characters are not in the path or title [ü é á ½ Æ ³ · ° , !] as XBMC doesn't like them")
        AppendDialogue(" - The occasional connect error will occur, when this happens the software automatically re-establishes connection and the retries the request.")
    '    AppendDialogue(" - If you encounter other errors & wish to raise an issue, then please include the log file: 'XBMC-Controller-log-file.txt'")
        AppendDialogue(" - If you encounter other errors & wish to raise an issue, then please include the full & brief log files: 'XBMC-Controller-full-log-file.txt' & 'XBMC-Controller-brief-log-file.txt'")
        AppendDialogue("")
        AppendDialogue("Known issues:")
        AppendDialogue("") 
        AppendDialogue(" - Movies are only picked up if they are not in a root (source) folder, i.e. they need to be in one or more sub-directories off your source folder(s). Basically it's an XBMC issue\bug. A simple workaround is to move your movies into a sub-folder, then run a clean in XBMC, then, with the link enabled do a 'Refresh'")
        AppendDialogue("")
        AppendDialogue("Future enhancements:")
        AppendDialogue("")
        AppendDialogue(" - TV support")
        AppendDialogue(" - MySQL support")
        AppendDialogue("")
        AppendDialogue("Help us make it better:")
        AppendDialogue("")
        AppendDialogue("Currently only tested on local and remote Windows XBMC installations. We would be interested in hearing your experiences on non-Windows XBMC installations, whether successful or not. So hopefully we can get it working on all platforms")
    End Sub
#End Region          'Main Subs

#Region "Other Subs"
    Sub SetEnabledStates

        tbXBMC_UserdataFolder     .Enabled = cbXBMC_Delete_Cached_Images.Checked 
        tbXBMC_TexturesDb         .Enabled = cbXBMC_Delete_Cached_Images.Checked     
        tbXBMC_ThumbnailsFolder   .Enabled = cbXBMC_Delete_Cached_Images.Checked     

        btnGeneralPrefsSaveChanges.Enabled = Changed
        btnUndo                   .Enabled = btnGeneralPrefsSaveChanges.Enabled

    End Sub

    Sub AssignFormFields
        cbXBMC_Active               .Checked = Pref.XBMC_Active
        tbXBMC_Address              .Text    = Pref.XBMC_Address
        tbXBMC_Port                 .Text    = Pref.XBMC_Port
        tbXBMC_Username             .Text    = Pref.XBMC_Username
        tbXBMC_Password             .Text    = Pref.XBMC_Password
        cbXBMC_Delete_Cached_Images .Checked = Pref.XBMC_Delete_Cached_Images
        tbXBMC_UserdataFolder       .Text    = Pref.XBMC_UserdataFolder
        tbXBMC_TexturesDb           .Text    = Pref.XBMC_TexturesDb
        tbXBMC_ThumbnailsFolder     .Text    = Pref.XBMC_ThumbnailsFolder
        MovieFolderMappings.Assign(Pref.XBMC_MC_MovieFolderMappings)
    End Sub

    Sub UpdatePreferences
        Pref.XBMC_Active                = cbXBMC_Active              .Checked
        Pref.XBMC_Address              = tbXBMC_Address              .Text
        Pref.XBMC_Port                 = tbXBMC_Port                 .Text
        Pref.XBMC_Username             = tbXBMC_Username             .Text
        Pref.XBMC_Password             = tbXBMC_Password             .Text
        Pref.XBMC_Delete_Cached_Images = cbXBMC_Delete_Cached_Images .Checked
        Pref.XBMC_UserdataFolder       = tbXBMC_UserdataFolder       .Text
        Pref.XBMC_TexturesDb           = tbXBMC_TexturesDb           .Text
        Pref.XBMC_ThumbnailsFolder     = tbXBMC_ThumbnailsFolder     .Text
        Pref.XBMC_MC_MovieFolderMappings.Assign(MovieFolderMappings)
    End Sub

    Private Sub ValidateSettings

        Dim tmp_MovieFolderMappings As XBMC_MC_FolderMappings = New XBMC_MC_FolderMappings

        Dim tmp_tbXBMC_Address              As String  = Pref.XBMC_Address
        Dim tmp_tbXBMC_Port                 As String  = Pref.XBMC_Port
        Dim tmp_tbXBMC_Username             As String  = Pref.XBMC_Username
        Dim tmp_tbXBMC_Password             As String  = Pref.XBMC_Password
        Dim tmp_cbXBMC_Delete_Cached_Images As Boolean = Pref.XBMC_Delete_Cached_Images
        Dim tmp_tbXBMC_UserdataFolder       As String  = Pref.XBMC_UserdataFolder
        Dim tmp_tbXBMC_TexturesDb           As String  = Pref.XBMC_TexturesDb
        Dim tmp_tbXBMC_ThumbnailsFolder     As String  = Pref.XBMC_ThumbnailsFolder

        tmp_MovieFolderMappings.Assign(Pref.XBMC_MC_MovieFolderMappings)

        UpdatePreferences

        tbDialogue.Clear


        Dim ParentForm                                 As Form1   = Media_Companion.Form1
        Dim PreFrodoPosterOnlyCount                    As Integer = ParentForm.oMovies.PreFrodoPosterOnlyCount
        Dim MovieFoldersConfigured                     As Boolean = (Pref.XBMC_MC_MovieFolderMappings.Items.Count>0)

        Dim tstFrodoEnabled                            As Boolean = Pref.FrodoEnabled
        Dim tstPreFrodoPosterOnlyCount                 As Boolean = PreFrodoPosterOnlyCount=0
        Dim tstXBMC_CanPing                            As Boolean = Pref.XBMC_CanPing                  
        Dim tstcanConnect                              As Boolean = Pref.XBMC_CanConnect                                 
        Dim tstXBMC_UserdataFolder_Valid               As Boolean = Pref.XBMC_UserdataFolder_Valid     
        Dim tstXBMC_TexturesDbFile_Valid               As Boolean = Pref.XBMC_TexturesDbFile_Valid     
        Dim tstXBMC_TexturesDb_Conn_Valid              As Boolean = Pref.XBMC_TexturesDb_Conn_Valid    
        Dim tstXBMC_TexturesDb_Version_Valid           As Boolean = Pref.XBMC_TexturesDb_Version_Valid 
        Dim tstXBMC_ThumbnailsFolder_Valid             As Boolean = Pref.XBMC_ThumbnailsFolder_Valid   
        Dim tstMovieFoldersConfigured                  As Boolean = MovieFoldersConfigured                    
        Dim tstXBMC_MC_MovieFolderMappings_Initialised As Boolean = Pref.XBMC_MC_MovieFolderMappings.Initialised

        Dim needDb = Pref.XBMC_Delete_Cached_Images

        Dim overAll As Boolean = tstFrodoEnabled                            And
                                 tstPreFrodoPosterOnlyCount                 And
                                 tstXBMC_CanPing                            And
                                 tstcanConnect                              And
                                 tstMovieFoldersConfigured                  And
                                 tstXBMC_MC_MovieFolderMappings_Initialised 

        Dim dbTestsResult As Boolean = tstXBMC_UserdataFolder_Valid      And
                                       tstXBMC_TexturesDbFile_Valid      And
                                       tstXBMC_TexturesDb_Conn_Valid     And
                                       tstXBMC_TexturesDb_Version_Valid  And
                                       tstXBMC_ThumbnailsFolder_Valid    

        If needDb Then
            overAll = overAll And dbTestsResult
        End If

        


        ShowTest("Frodo Enabled"                                                                                        , tstFrodoEnabled                           ,11)
        ShowTest("Pre-Frodo only movies ('.tbn' posters instead of '-poster.jpg') : " & PreFrodoPosterOnlyCount.ToString, tstPreFrodoPosterOnlyCount                ,11)
        ShowTest("XBMC PC Ping"                                                                                         , tstXBMC_CanPing                           , 1)
        ShowTest("XBMC Connect"                                                                                         , tstcanConnect                             , 2)
        ShowTest("Userdata Folder"                                                                                      , tstXBMC_UserdataFolder_Valid              , 6)
        ShowTest("TexturesDb File"                                                                                      , tstXBMC_TexturesDbFile_Valid              , 7)
        ShowTest("TexturesDb Connection"                                                                                , tstXBMC_TexturesDb_Conn_Valid             , 7)
        ShowTest("TexturesDb Version (Frodo needed)"                                                                    , tstXBMC_TexturesDb_Version_Valid          , 7)
        ShowTest("Thumbnails Folder"                                                                                    , tstXBMC_ThumbnailsFolder_Valid            , 8)
        ShowTest("Movie folder(s) configured"                                                                           , tstMovieFoldersConfigured                 , 9)
        ShowTest("Movie Folder mappings set (NB Actual paths not validated as applicable to XBMC PC)"                   , tstXBMC_MC_MovieFolderMappings_Initialised, 9)
        ShowTest("Overall", overAll , 11) 

        UpdateImage(3 ,tstcanConnect)
        UpdateImage(4 ,tstcanConnect)


        If Not Pref.FrodoEnabled Or PreFrodoPosterOnlyCount>0 Or Not tstcanConnect Or Not MovieFoldersConfigured Then
            AppendDialogue("****************************************")
            AppendDialogue("")
            AppendDialogue("Things to check:")

            If Not Pref.FrodoEnabled Then
                AppendDialogue("    - MC has General Preferences-General-Artwork Version->Frodo enabled ")
            End If

            If PreFrodoPosterOnlyCount>0 Then
                AppendDialogue("    - Warning: Some of your movies only have '.tbn' poster extensions, Frodo expects '-poster.jpg'. You can fix this by:")
                AppendDialogue("         - 1. Selecting 'Pre-Frodo poster only' from the 'General' Movie Filter" )
                AppendDialogue("         - 2. Selecting all the movies in the list, then Rt-Click & select 'Convert to Frodo only'" )
            End If

            If Not tstcanConnect Then
                AppendDialogue("    - XBMC is running")
                AppendDialogue("    - System - Settings - Services - Webserver:")
                AppendDialogue("        - Allow control of XBMC via HTTP' is checked ")
                AppendDialogue("        - Port, Username and Password match")
                AppendDialogue("        - Web interface is set to 'Default'")
                AppendDialogue("    - If you are trying to connect to XBMC on a remote PC, make sure:")
                AppendDialogue("        - The PCs' IP address is static")
                AppendDialogue("        - System - Settings - Services - Remote Control has 'Allow programs on other systems to control XBMC' checked")
            End If

            If Not MovieFoldersConfigured Then
                AppendDialogue("    - You have no movie folders set up. Go to Movies-Folders and add them")
            End If

            AppendDialogue("")
            AppendDialogue("****************************************")
        End If

        If Not needDb Then
            AppendDialogue("")
            AppendDialogue("****************************************")
            AppendDialogue("")
            AppendDialogue("Database test results ignored, as database not needed.")
            AppendDialogue("")
            AppendDialogue("****************************************")
        End If

        Pref.XBMC_Address              = tmp_tbXBMC_Address         
        Pref.XBMC_Port                 = tmp_tbXBMC_Port            
        Pref.XBMC_Username             = tmp_tbXBMC_Username        
        Pref.XBMC_Password             = tmp_tbXBMC_Password        
        Pref.XBMC_Delete_Cached_Images = tmp_cbXBMC_Delete_Cached_Images
        Pref.XBMC_UserdataFolder       = tmp_tbXBMC_UserdataFolder  
        Pref.XBMC_TexturesDb           = tmp_tbXBMC_TexturesDb      
        Pref.XBMC_ThumbnailsFolder     = tmp_tbXBMC_ThumbnailsFolder
                                              
        Pref.XBMC_MC_MovieFolderMappings.Assign(tmp_MovieFolderMappings)
    End Sub

    Function ShowTest(test As String, result As Boolean, row As Integer)

        AppendTestDialogue(test,result)
        UpdateImage       (row ,result)

        Return result
    End Function
  
    Sub AppendTestDialogue(testing As String, result As Boolean)
        AppendDialogue( testing & " -> " & IIf(result,"Passed","Failed") )
    End Sub
 
    Sub ClearImages
        For row As Integer = 1 to 11
            If row = 5 Or row = 10 Then row +=1
            Try
                GetPictureBox(row).Image = Nothing
            Catch
            End Try
        Next
    End Sub

    Sub UpdateImage(row As Integer,result As Boolean)
        GetPictureBox(row).Image = IIf(result,Global.Media_Companion.My.Resources.Resources.correct,Global.Media_Companion.My.Resources.Resources.incorrect)
    End Sub
    
    Function GetPictureBox(row As Integer) As PictureBox
        Return tlpXbmcLink.GetControlFromPosition(3,row)
    End Function

    Sub AppendDialogue(msg As String)
        tbDialogue.Text += msg + Environment.NewLine
    End Sub

#End Region         'Other Subs

End Class
