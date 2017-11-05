Imports System.Windows.Forms
Imports Media_Companion

Partial Public Class TvEpisode
    Public EpisodeNode As New TreeNode With {.SelectedImageKey = "blank", .ImageKey = "blank"}

    Public Sub UpdateTreenode(Optional ByVal scraping As Boolean = False)
        Me.EpisodeNode.Tag = Me

        Dim episode_count = 1
        If Me.SeasonObj IsNot Nothing Then
            If Me.SeasonObj.MaxEpisodeCount > 99 Then
                episode_count = 3
            ElseIf Me.SeasonObj.MaxEpisodeCount > 9 Then
                episode_count = 2
            End If
        End If

        If scraping = True Then
            For Each EpisodeNode2 As TreeNode In Me.SeasonObj.SeasonNode.Nodes
                Try
                    Dim TempEpisodeNo As String = EpisodeNode2.Text.Substring(0, EpisodeNode2.Text.IndexOf("-") - 1)
                    Dim TempEpisodeTitle As String = EpisodeNode2.Text.Substring(TempEpisodeNo.Length + 2, EpisodeNode2.Text.Length - (TempEpisodeNo.Length + 2))
                    TempEpisodeTitle = TempEpisodeTitle.Trim
                    EpisodeNode2.Text = Utilities.PadNumber(TempEpisodeNo, episode_count) & " - " & TempEpisodeTitle
                Catch ex As Exception

                End Try
            Next
        End If

        If Me.Title.Value IsNot Nothing Then
            Me.EpisodeNode.Name = Me.NfoFilePath             ' save nfo path in this node
            If Me.Episode.Value IsNot Nothing Then
                If IsNumeric(Me.Episode.Value) Then
                    Dim seasonstring As String = Me.Episode.Value.ToString
                    Me.EpisodeNode.Text = Utilities.PadNumber(Me.Episode.Value, episode_count) & " - " & Me.Title.Value
                Else
                    Me.EpisodeNode.Text = Utilities.PadNumber(Me.Episode.Value, episode_count) & " - " & Me.Title.Value
                End If
            Else
                Me.EpisodeNode.Text = Me.Title.Value
            End If
        Else
            Me.EpisodeNode.Text = Me.NfoFilePath
        End If

        If Me.IsMissing Then
            Dim ThisDate As Date = If(Media_Companion.Pref.TvMissingEpOffset, Now.AddDays(-1), Now)
            ' Phyonics - Fix for issue #208
            If String.IsNullOrEmpty(Aired.Value) Then
                ' Change the colour to gray
                EpisodeNode.ForeColor = Drawing.Color.Gray
            Else
                Try
                    ' Is the episode in the future?
                    If Convert.ToDateTime(Aired.Value) > ThisDate Then
                        ' Yes, so change its colour to Red
                        EpisodeNode.ForeColor = Drawing.Color.Red
                    Else
                        EpisodeNode.ForeColor = Drawing.Color.Blue
                    End If
                Catch ex As Exception
                    ' Set the colour to the missing colour
                    EpisodeNode.ForeColor = Drawing.Color.Blue
                End Try
            End If
        Else
            EpisodeNode.ForeColor = Drawing.Color.Black
        End If

        If Me.FailedLoad Then
            EpisodeNode.ForeColor = Drawing.Color.Red
        End If

        'If Me.IsAltered Then
        '    EpisodeNode.ImageKey = "edit"
        '    EpisodeNode.SelectedImageKey = "edit"
        If Me.PlayCount.Value <> "0" AndAlso Me.PlayCount.Value <> "" Then
            EpisodeNode.ImageKey = "watched"
            EpisodeNode.SelectedImageKey = "watched"
        Else
            EpisodeNode.ImageKey = "blank"
            EpisodeNode.SelectedImageKey = "blank"
        End If

        If Me.EpisodeNode.TreeView Is Nothing Then
            If Me.SeasonObj IsNot Nothing Then
                Me.SeasonObj.SeasonNode.Nodes.Add(Me.EpisodeNode)
            End If
        End If
    End Sub
    Public Overrides Sub Load()
        MyBase.Load()

        UpdateTreenode()
    End Sub

    Public Overrides Sub Load(ByVal Path As String)
        MyBase.Load(Path)

        UpdateTreenode()
    End Sub
End Class
