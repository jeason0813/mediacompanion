Imports System.Windows.Forms

Partial Public Class TvSeason
    Public SeasonNode As New TreeNode With {.SelectedImageKey = "blank", .ImageKey = "blank"}

    Public Sub UpdateTreenode()
        Me.SeasonNode.Tag = Me
        Me.SeasonNode.Text = Me.SeasonLabel
        SeasonNode.Tag = Me

        If Me.SeasonNode.TreeView Is Nothing Then
            If ShowObj IsNot Nothing Then
                If Pref.displayMissingEpisodes AndAlso ShowObj.MissingEpisodes.Count > 0 Then
                    Dim seasonismissingeps As Boolean = False
                    Dim thisseason As String = ""
                    For Each ShSeason In ShowObj.Seasons
                        If ShSeason.Value.SeasonLabel = Me.SeasonLabel Then thisseason = ShSeason.key : Exit For
                    Next
                    For Each missingep In ShowObj.MissingEpisodes
                        If missingep.Season.Value = thisseason Then seasonismissingeps = True : Exit For
                    Next
                    If seasonismissingeps Then
                        SeasonNode.ImageKey = "missing.png"
                        SeasonNode.SelectedImageKey = "missing.png"
                    Else
                        SeasonNode.ImageKey = "blank"
                        SeasonNode.SelectedImageKey = "blank"
                    End If
                    
                    SeasonNode.SelectedImageKey = "missing.png"
                Else
                    Me.Playcount.Value = "1"
                    For Each ep In ShowObj.Episodes 
                        If ep.Season.Value = Me.SeasonNumber.ToString AndAlso (ep.PlayCount.Value = "0" orElse ep.PlayCount.Value = Nothing) Then
                            Me.Playcount.Value = "0"
                            Exit For
                        End If
                    Next
                    If Me.Playcount.Value = "1" Then
                        SeasonNode.ImageKey = "watched"
                        SeasonNode.SelectedImageKey = "watched"
                    Else
                        SeasonNode.ImageKey = "blank"
                        SeasonNode.SelectedImageKey = "blank"
                    End If
                    
                End If
                Me.ShowObj.ShowNode.Nodes.Add(Me.SeasonNode)
            End If
        Else
            Me.Playcount.Value = "1"
            For Each ep In ShowObj.Episodes 
                If ep.Season.Value = Me.SeasonNumber.ToString AndAlso ep.PlayCount.Value = "0" Then
                    Me.Playcount.Value = "0"
                    Exit For
                End If
            Next
            If Me.Playcount.Value = "1" Then
                SeasonNode.ImageKey = "watched"
                SeasonNode.SelectedImageKey = "watched"
            Else
                SeasonNode.ImageKey = "blank"
                SeasonNode.SelectedImageKey = "blank"
            End If
        End If
    End Sub

    Public Function GetParentShow() As TvShow
        If Me.SeasonNode.Parent IsNot Nothing Then
            Return Me.SeasonNode.Parent.Tag
        End If
        Return Nothing
    End Function

End Class
