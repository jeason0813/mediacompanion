Imports System.Reflection

Public Class RescrapeList
    Property title                  As Boolean   'Booleans default to false
    Property votes                  As Boolean
    Property rating                 As Boolean
    Property top250                 As Boolean
    Property runtime                As Boolean
    Property director               As Boolean
    Property stars                  As Boolean
    Property year                   As Boolean
    Property outline                As Boolean
    Property plot                   As Boolean
    Property tagline                As Boolean
    Property genre                  As Boolean
    Property studio                 As Boolean
    Property premiered              As Boolean
    Property mpaa                   As Boolean
    Property credits                As Boolean
    Property country                As Boolean
    Property trailer                As Boolean
    Property metascore              As Boolean
    Property posterurls             As Boolean
    Property actors                 As Boolean
    Property mediatags              As Boolean
    Property missingposters         As Boolean
    Property missingfanart          As Boolean
    Property missingmovsetart       As Boolean
    Property dlxtraart              As Boolean
    Property runtime_file           As Boolean
    Property tmdb_set_name          As Boolean
    Property tmdb_set_info          As Boolean
    Property Download_Trailer       As Boolean
    Property Rename_Files           As Boolean
    Property Rename_Folders         As Boolean
    Property Frodo_Poster_Thumbs    As Boolean
    Property Frodo_Fanart_Thumbs    As Boolean
    Property Xbmc_Sync              As Boolean
    Property Convert_To_Frodo       As Boolean
    Property EmptyMainTags          As Boolean
    Property TagsFromKeywords       As Boolean
    Property SetWatched             As Boolean
    Property ClearWatched           As Boolean
    Property ArtFromFanartTv        As Boolean
    Property FromTMDB               As Boolean
    Property rebuildnfo             As Boolean
    Property imdbaspect             As Boolean

    Sub New
    End Sub

    Sub New(field As String)
        Dim propInfos() As PropertyInfo = Me.GetType.GetProperties

        For Each propInfo in propInfos

            If propInfo.Name.ToLower = field.ToLower then
                propInfo.SetValue(Me,True,Nothing)
            End If
        Next
    End Sub

    Sub ResetFields
        Dim propInfos() As PropertyInfo = Me.GetType.GetProperties

        For Each propInfo in propInfos

            If Not propInfo.CanWrite Then Continue For

            propInfo.SetValue(Me,False,Nothing)
        Next
    End Sub

    ReadOnly Property AnyEnabled As Boolean
        Get
            Dim propInfos() As PropertyInfo = Me.GetType.GetProperties

            For Each propInfo in propInfos

                If Not propInfo.CanWrite Then Continue For
                If propInfo.Name = "FromTMDB" Then Continue For

                Dim value As Boolean = propInfo.GetValue(Me,Nothing)

                If value Then Return True
            Next

            Return False
        End Get
    End Property

    ReadOnly Property AnyNonMainEnabled As Boolean
        Get
            Return  trailer Or posterurls Or actors Or mediatags Or missingposters Or missingfanart Or missingmovsetart Or dlxtraart Or runtime_file Or
                tmdb_set_name Or tmdb_set_info Or Download_Trailer Or Rename_Files Or Rename_Folders Or Frodo_Poster_Thumbs Or Frodo_Fanart_Thumbs Or
                Xbmc_Sync Or Convert_To_Frodo Or TagsFromKeywords Or SetWatched Or ClearWatched Or ArtFromFanartTv
        End Get
    End Property

End Class
