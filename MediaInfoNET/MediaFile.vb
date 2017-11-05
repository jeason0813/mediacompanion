
'==============================================================================
'Copyright 2013 Tony George <teejee2008@gmail.com>

'This program is free software; you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation; either version 2 of the License, or
'(at your option) any later version.

'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License for more details.

'You should have received a copy of the GNU General Public License
'along with this program; if not, write to the Free Software
'Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
'MA 02110-1301, USA.
'==============================================================================

'Imports System.IO
Imports System.Collections
Imports System.Text.RegularExpressions

Public Class MediaFile
    Inherits MediaInfoFile
    Public Description As String = ""
    Public Info_Text As String = ""
    Public Info_HTML As String = ""

    Public MediaInfo_Text As String = ""
    Public MediaInfo_HTML As String = ""
    Public MediaInfo_Available As Boolean = False

    Public AllStreams As List(Of MediaInfo_Stream) 'List containing references for all video, audio, image, text, menu and chapter streams in file
    Public General As MediaInfo_Stream_General 'Not added to AllStreams
    Public Audio As List(Of MediaInfo_Stream_Audio)
    Public Video As List(Of MediaInfo_Stream_Video)
    Public Text As List(Of MediaInfo_Stream_Text)
    Public Image As List(Of MediaInfo_Stream_Image)
    Public Menu As List(Of MediaInfo_Stream_Menu)
    Public Chapters As List(Of MediaInfo_Stream_Chapters)
    Public Data As List(Of MediaInfo_Stream_Data)

    Sub New(ByVal SourceFile As String)
        MyBase.new(SourceFile)

        AllStreams = New List(Of MediaInfo_Stream)
        General = New MediaInfo_Stream_General
        Audio = New List(Of MediaInfo_Stream_Audio)
        Video = New List(Of MediaInfo_Stream_Video)
        Text = New List(Of MediaInfo_Stream_Text)
        Image = New List(Of MediaInfo_Stream_Image)
        Menu = New List(Of MediaInfo_Stream_Menu)
        Chapters = New List(Of MediaInfo_Stream_Chapters)
        Data = New List(Of MediaInfo_Stream_Data)

        If SourceFile = "" Then
            Exit Sub
        End If

        GetMediaInfo(True)

        If InfoAvailable = False Then
            Exit Sub
        End If

        getHTML(Info_HTML, General, AllStreams)
        getInfoText()
        getDescription()
    End Sub

    Public Sub GetMediaInfo(ByVal AppendInfo As Boolean)
        If MediaInfo_Available Then
            Exit Sub
        End If

        If My.Computer.FileSystem.FileExists(Me.File) = False Then
            Exit Sub
        End If

        Dim objMediaInfo As MediaInfo = New MediaInfo()
        objMediaInfo.Open(Me.File)

        MediaInfo_Text = Trim(objMediaInfo.Inform)

        Dim m_General As New MediaInfo_Stream_General
        Dim m_AllStreams As New List(Of MediaInfo_Stream)

        Dim streams() As String
        Dim lines() As String
        Dim temp() As String
        Dim PropertyName, PropertyValue As String

        streams = Split(MediaInfo_Text, vbNewLine + vbNewLine)

        Dim StreamIndex As Integer = -1

        For Each stream As String In streams
            Dim m As MediaInfo_Stream = Nothing
            lines = Split(stream, vbNewLine)
            Dim main_line As String = lines(0)

            If main_line <> "" Then
                If main_line.Contains("General") Then
                    m = New MediaInfo_Stream_General
                    m_General = m
                    'MediaInfo cannot read Avisynth files, so add format manually
                    If Extension = ".avs" Then
                        m.Properties.Add("Format", "Avisynth Script")
                    End If

                ElseIf main_line.Contains("Audio") Then
                    m = New MediaInfo_Stream_Audio
                    m_AllStreams.Add(m)
                    StreamIndex += 1
                    m.StreamIndex = StreamIndex

                ElseIf main_line.Contains("Video") Then
                    m = New MediaInfo_Stream_Video
                    m_AllStreams.Add(m)
                    StreamIndex += 1
                    m.StreamIndex = StreamIndex

                ElseIf main_line.Contains("Text") Then
                    m = New MediaInfo_Stream_Text
                    m_AllStreams.Add(m)
                    StreamIndex += 1
                    m.StreamIndex = StreamIndex

                ElseIf main_line.Contains("Image") Then
                    m = New MediaInfo_Stream_Image
                    m_AllStreams.Add(m)
                    StreamIndex += 1
                    m.StreamIndex = StreamIndex

                ElseIf main_line.Contains("Menu") Then
                    m = New MediaInfo_Stream_Menu
                    m_AllStreams.Add(m)
                    StreamIndex += 1
                    m.StreamIndex = StreamIndex

                ElseIf main_line.Contains("Chapters") Then
                    m = New MediaInfo_Stream_Chapters
                    m_AllStreams.Add(m)
                    StreamIndex += 1
                    m.StreamIndex = StreamIndex

                End If
            End If

            If m IsNot Nothing Then
                'get properties
                For Each line As String In lines
                    temp = Split(line, " : ")
                    If temp.Length > 1 Then
                        PropertyName = temp(0).Trim
                        PropertyValue = temp(1).Trim
                        If Not m.Properties.ContainsKey(PropertyName) Then
                            m.Properties.Add(PropertyName, PropertyValue)
                        End If
                    End If
                Next line
            End If
        Next stream

        For Each s As MediaInfo_Stream In m_AllStreams
            s.SourceFile = Me
        Next s

        'fix stream order using ID property ------------------------------------

        Dim swapped As Boolean = True
        Dim tempStream As MediaInfo_Stream

        'bubble-sort the streams using the ID number

        For passes = 1 To m_AllStreams.Count 'N passes
            If swapped = False Then Exit For
            For i = 0 To m_AllStreams.Count - 2 'N-1 comparisons
                If m_AllStreams(i).ID <> -1 And m_AllStreams(i + 1).ID <> -1 Then
                    If m_AllStreams(i).ID > m_AllStreams(i + 1).ID Then
                        tempStream = m_AllStreams(i)
                        m_AllStreams(i) = m_AllStreams(i + 1)
                        m_AllStreams(i + 1) = tempStream
                        swapped = True
                    End If
                End If
            Next
        Next

        For Each s As MediaInfo_Stream In m_AllStreams
            s.StreamIndex = AllStreams.IndexOf(s)
        Next

        setStreamTypeIndices(m_AllStreams)

        '--finished fixing stream order ---------------------------------------

        MediaInfo_Available = True

        getHTML(MediaInfo_HTML, m_General, m_AllStreams)

        If AppendInfo Then
            addProperties(m_General, m_AllStreams, True)
        End If

        objMediaInfo.Close()
    End Sub

    Private Sub addProperties(ByVal m_General As MediaInfo_Stream_General, _
                                     ByVal m_AllStreams As List(Of MediaInfo_Stream), Optional ByVal IsMediaInfo As Boolean = False)

        Dim d As Dictionary(Of String, String)
        Dim index As Integer

        If General Is Nothing Then
            General = m_General
        Else
            d = m_General.Properties
            For Each r As String In d.Keys
                If Not General.Properties.ContainsKey(r) Then
                    General.Properties.Add(r, d.Item(r))
                End If
            Next r
        End If

        index = -1
        If AllStreams.Count = 0 Then 'simply add streams one by one
            For Each m As MediaInfo_Stream In m_AllStreams
                index += 1

                AllStreams.Add(m)
                Select Case m.StreamType.ToString
                    Case "Data"
                        Data.Add(m)
                        m.StreamTypeIndex = Data.IndexOf(m)
                    Case "Audio"
                        Audio.Add(m)
                        m.StreamTypeIndex = Audio.IndexOf(m)
                    Case "Video"
                        Video.Add(m)
                        m.StreamTypeIndex = Video.IndexOf(m)
                    Case "Text"
                        Text.Add(m)
                        m.StreamTypeIndex = Text.IndexOf(m)
                    Case "Image"
                        Image.Add(m)
                        m.StreamTypeIndex = Image.IndexOf(m)
                    Case "Menu"
                        Menu.Add(m)
                        m.StreamTypeIndex = Menu.IndexOf(m)
                    Case "Chapters"
                        Chapters.Add(m)
                        m.StreamTypeIndex = Chapters.IndexOf(m)
                End Select
            Next
        Else
            'match the streams and append additional properties for each stream
            For Each m As MediaInfo_Stream In m_AllStreams
                For Each s As MediaInfo_Stream In AllStreams
                    If s.StreamType.Equals(m.StreamType) And (s.StreamTypeIndex = m.StreamTypeIndex) And ((s.FormatID = m.FormatID) Or s.FormatID = "") Then
                        d = m.Properties

                        If IsMediaInfo Then
                            'overwrite ffmpeg's CodecID and Format values with MediaInfo's values
                            If s.Properties.ContainsKey("Codec ID") And m.Properties.ContainsKey("Codec ID") Then
                                s.Properties("Codec ID") = m.Properties("Codec ID")
                            End If
                            If s.Properties.ContainsKey("Format") And m.Properties.ContainsKey("Format") Then
                                s.Properties("Format") = m.Properties("Format")
                            End If
                        End If

                        For Each r As String In d.Keys
                            If Not s.Properties.ContainsKey(r) Then
                                s.Properties.Add(r, d.Item(r))
                            End If
                        Next
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub setStreamTypeIndices(ByRef streamList As List(Of MediaInfo_Stream))
        Dim audioCount As Integer = -1
        Dim videoCount As Integer = -1
        Dim imageCount As Integer = -1
        Dim textCount As Integer = -1
        Dim chaptersCount As Integer = -1
        Dim menuCount As Integer = -1

        For Each s As MediaInfo_Stream In streamList
            s.StreamIndex = streamList.IndexOf(s)
            If TypeOf s Is MediaInfo_Stream_Video Then
                videoCount += 1
                s.StreamTypeIndex = videoCount
            ElseIf TypeOf s Is MediaInfo_Stream_Audio Then
                audioCount += 1
                s.StreamTypeIndex = audioCount
            ElseIf TypeOf s Is MediaInfo_Stream_Image Then
                imageCount += 1
                s.StreamTypeIndex = imageCount
            ElseIf TypeOf s Is MediaInfo_Stream_Text Then
                textCount += 1
                s.StreamTypeIndex = textCount
            ElseIf TypeOf s Is MediaInfo_Stream_Chapters Then
                chaptersCount += 1
                s.StreamTypeIndex = chaptersCount
            ElseIf TypeOf s Is MediaInfo_Stream_Menu Then
                menuCount += 1
                s.StreamTypeIndex = menuCount
            End If
        Next
    End Sub

    Public Sub DeleteProperties()
        General = Nothing
        AllStreams.Clear()
        Audio.Clear()
        Video.Clear()
        Text.Clear()
        Image.Clear()
        Menu.Clear()
        Chapters.Clear()
        Info_Text = ""
        Info_HTML = ""
    End Sub



    Private Sub getDescription()
        Dim o As String = ""

        If General.FormatID <> "" Then
            o += ", " & General.Extension & " File"
        End If
        If FileSize <> 0 Then
            o += ", " & FormatFileSize(FileSize)
        End If
        If StreamCount >= 0 Then
            o += ", " & StreamCount.ToString & " streams"
        End If
        If General.Bitrate <> 0 Then
            o += ", " & General.Bitrate.ToString & " kbps"
        End If
        If General.DurationString <> "" Then
            o += ", " & General.DurationString
        End If
        If o.Trim <> "" Then
            o = o.Trim.Remove(0, 1).Trim
        End If

        o += vbNewLine + vbNewLine

        For Each s As MediaInfo_Stream In AllStreams
            Select Case s.StreamType
                Case "Audio", "Video", "Text"
                    o += "#" + s.StreamIndex.ToString() + " | " + s.StreamType + " [ " + s.toString() + " ]" + vbNewLine
                Case "Menu", "Chapter"
                    'do nothing
            End Select
        Next

        o = o.Trim()

        If o.EndsWith(vbNewLine) Then
            o = o.Remove(o.Length, 1)
        End If

        Description = o
    End Sub

    Private Sub getHTML(ByRef StringHTML As String, _
                        ByRef m_General As MediaInfo_Stream_General, _
                        ByRef m_AllStreams As List(Of MediaInfo_Stream))

        Dim html As String = ""
        Dim odd As Boolean = False

        Dim d As Dictionary(Of String, String)
        d = m_General.Properties

        html += "<table width='100%'>" + vbNewLine

        html += "<thead><tr>" + vbNewLine
        html += "<th colspan=2>&nbsp;General</th>" + vbNewLine
        html += "</tr></thead>" + vbNewLine

        html += "<tbody>" + vbNewLine
        For Each r As String In d.Keys
            odd = Not odd
            If odd Then
                html += "<tr class='odd'>"
            Else
                html += "<tr>"
            End If
            html += "<td nowrap>" + r + "</td><td>" + d.Item(r).ToString + "</td>" + vbNewLine
            html += "</tr>"
        Next r
        html += "</tbody>" + vbNewLine
        html += "</table>" + vbNewLine + vbNewLine

        For Each m As MediaInfo_Stream In m_AllStreams

            If m.Properties.Count = 0 Then Continue For

            d = m.Properties
            html += "<table width='100%'>" + vbNewLine

            html += "<thead><tr>" + vbNewLine
            html += "<th colspan=2>&nbsp;" + m.StreamType + " #" + m.StreamTypeIndex.ToString + "</th>" + vbNewLine
            html += "</tr></thead>" + vbNewLine

            html += "<tbody>" + vbNewLine
            For Each r As String In d.Keys
                odd = Not odd
                If odd Then
                    html += "<tr class='odd'>"
                Else
                    html += "<tr>"
                End If
                html += "<td nowrap>" + r + "</td><td>" + d.Item(r).ToString + "</td>" + vbNewLine
                html += "</tr>"
            Next r
            html += "</tbody>" + vbNewLine
            html += "</table>" + vbNewLine + vbNewLine
        Next

        StringHTML = html
    End Sub

    Private Sub getInfoText()
        Dim s As String = ""
        Dim odd As Boolean = False

        Dim d As Dictionary(Of String, String)
        d = General.Properties

        s += "General" + vbNewLine
        For Each r As String In d.Keys
            s += r + " : " + d.Item(r).ToString + vbNewLine
        Next r
        s += vbNewLine
        For Each m As MediaInfo_Stream In AllStreams
            d = m.Properties

            s += m.StreamType + " #" + m.StreamTypeIndex.ToString + vbNewLine
            For Each r As String In d.Keys
                s += r + " : " + d.Item(r).ToString + vbNewLine
            Next r
            s += vbNewLine
        Next

        Info_Text = s
    End Sub


    Public ReadOnly Property FrameCount() As Integer
        Get
            If Video.Count > 0 Then
                If Video(0).FrameRate <> 0 And General.DurationMillis <> 0 Then
                    Return Math.Ceiling(Video(0).FrameRate * General.DurationMillis / 1000)
                End If
            End If

            Return 0
        End Get
    End Property

    Public ReadOnly Property StreamCount() As Integer
        Get
            Return (Audio.Count + Video.Count + Text.Count)
        End Get
    End Property

    Public ReadOnly Property InfoAvailable() As Boolean
        Get
            If Audio.Count > 0 Or Video.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property FileSize() As Long
        Get
            If My.Computer.FileSystem.FileExists(File) Then
                Return My.Computer.FileSystem.GetFileInfo(File).Length
            End If
            Return 0
        End Get
    End Property
End Class
