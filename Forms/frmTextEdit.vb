'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Text.RegularExpressions

Public Class frmTextEdit
    Dim CustomPath As String = Pref.workingProfile.Genres
    Dim TextData As String = ""
    Private Sub frmTextEdit_Load(sender As Object, e As EventArgs) Handles Me.Load
        
        If File.Exists(Custompath) Then
            LoadTextbox()
        End If
    End Sub

    Private Sub LoadTextbox()
        Dim line As String = String.Empty
        Using fs As IO.StreamReader = File.OpenText(CustomPath)
            Do While fs.Peek() >= 0
            line = fs.ReadLine
            If line <> Nothing Then
                Dim regexMatch As Match
                regexMatch = Regex.Match(line, "<([\d]{2,3})>")
                If regexMatch.Success = False Then
                    If TextData <> "" Then TextData &= vbCrLf
                    TextData &=line.Trim 
                End If
            End If
            Loop
            
        End Using
        tbText.Text = TextData
    End Sub

    Private Sub SaveCustomGenre()
        If File.Exists(CustomPath) Then File.Delete(CustomPath)
        Using fs As IO.StreamWriter = File.CreateText(CustomPath)
            Try
                fs.Write(tbText.Text)
            Catch ex As Exception
                'fs.Close()
            End Try
        End Using
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Dim newentry As Boolean = False
        If tbText.Text <> "" Then
            Dim Lines() as String = tbText.Text.Split(vbCrLf)
            For each line In lines
                If TextData.Contains(line) Then Continue For
                newentry = True
                Exit For
            Next
        End If
        If newentry Then
            Dim t As MsgBoxResult = MsgBox("Changes may have been made in list" & vbCrLf & "Do you wish to save?", MsgBoxStyle.YesNo)
            If t = MsgBoxResult.Yes Then SaveCustomGenre()
        End If
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveCustomGenre()
        Me.Close()
    End Sub
End Class