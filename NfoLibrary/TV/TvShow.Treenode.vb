Imports System.Windows.Forms

Partial Public Class TvShow
    Public ShowNode As New TreeNode


    Public Sub UpdateTreenode()
        Me.ShowNode.Tag = Me
        Me.ShowNode.Text = Pref.RemoveIgnoredArticles(Me.Title.Value)
        Dim locked As Boolean = False
        Select Case Me.State
            Case Media_Companion.ShowState.Open
                ShowNode.ImageKey = "blank"
                ShowNode.SelectedImageKey = "blank"
            Case Media_Companion.ShowState.Locked
                ShowNode.ImageKey = "padlock"
                ShowNode.SelectedImageKey = "padlock"
                locked = True
            Case Media_Companion.ShowState.Unverified
                ShowNode.ImageKey = "qmark"
                ShowNode.SelectedImageKey = "qmark"
            Case Media_Companion.ShowState.Error
                ShowNode.ImageKey = "error"
                ShowNode.SelectedImageKey = "error"
        End Select
        
        Me.Playcount.Value = "1"
        For Each seas In Me.Seasons.Keys
            If Me.Seasons(seas).Playcount.Value = "0" Then
                Me.Playcount.Value = "0"
                Exit For
            End If
        Next
        If Me.Episodes.Count = 0 Then Me.Playcount.Value = "0"
        If Me.Playcount.Value = "1" AndAlso locked Then
            ShowNode.ImageKey = "lockwatched"
            ShowNode.SelectedImageKey = "lockwatched"
        ElseIf Me.Playcount.Value = "1" AndAlso Not locked Then
            ShowNode.ImageKey = "watched"
            ShowNode.SelectedImageKey = "watched"
        End If

        If Me.FailedLoad Then
            ShowNode.ForeColor = Drawing.Color.Red
        End If

        If Pref.displayMissingEpisodes AndAlso Me.MissingEpisodes.Count > 0 Then
            ShowNode.ImageKey = "missing.png"
            ShowNode.SelectedImageKey = "missing.png"
        End If

    End Sub

    Public Overrides Sub Load(ByVal Path As String)
        MyBase.Load(Path)
        UpdateTreenode()
    End Sub

    'Public Overloads Sub Load(ByVal SearchForEpisodesAsWell As Boolean)
    '    Me.Load()
    '    If SearchForEpisodesAsWell Then
    '        SearchForEpisodesInFolder()
    '    End If
    'End Sub

    'Public Overloads Sub Load(ByVal Path As String, ByVal SearchForEpisodesAsWell As Boolean)
    '    Me.Load(Path)
    '    If SearchForEpisodesAsWell Then
    '        SearchForEpisodesInFolder()
    '    End If
    'End Sub
End Class
