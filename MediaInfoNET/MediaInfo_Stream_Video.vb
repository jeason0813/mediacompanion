
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
'Imports System.Collections
Imports System.Text.RegularExpressions

Public Class MediaInfo_Stream_Video
    Inherits MediaInfo_Stream
    
    Public Overrides ReadOnly Property StreamType() As String
        Get
            Return "Video"
        End Get
    End Property

    Public Overrides ReadOnly Property FormatID() As String
        Get
            Dim value As String = GetProperty("Format").Trim()

            If value <> "" Then
                Select Case value.ToLower
                    Case "avc", "h264"
                        Return "H264"

                    Case "mpeg-4", "mpeg-4 visual", "ms-mpeg v1", "ms-mpeg v2", "ms-mpeg v3", "s-mpeg 4 v2", "s-mpeg 4 v3"
                        Return "MPEG4"

                    Case "realvideo 1", "realvideo 2", "realvideo 3", "realvideo 4", "rv10", "rv20", "rv30", "rv40"
                        Return "RV"

                    Case "mpeg video", "mpeg2video"
                        Dim ver As String = GetProperty("Format version")
                        If ver.Contains("2") Then Return "MPEG2VIDEO"
                        Return "MPEG"

                    Case "wmv2"
                        Return "WMV"

                    Case Else
                        Return value.ToUpper
                End Select
            Else
                value = GetProperty("Codec ID")
                Select Case value.ToLower
                    Case "rawvideo"
                        Return "RAW"
                    Case "dvvideo"
                        Return "DV"
                    Case Else
                        Return value.ToUpper
                End Select
            End If

            Return ""
        End Get
    End Property

    Public ReadOnly Property MPlayerID() As String
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("MPlayer -vid", value) Then
                Return value
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property PixelFormat() As String
        Get
            Return GetProperty("Pixel format")
        End Get
    End Property

    Public ReadOnly Property Width() As Integer
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Width", value) Then
                If value IsNot Nothing Then

                    Dim i As Integer = 0

                    exp = New Regex("([ 0-9,]+)[pixels]*")
                    exp_matches = exp.Matches(value)

                    If exp_matches.Count > 0 Then
                        value = exp_matches.Item(0).Value
                        value = exp.Replace(value, "$1").Replace(" ", "").Replace(",", "").Trim
                        If Integer.TryParse(value, i) Then
                            Return i
                        End If
                    End If

                End If
            End If

            Return 0

        End Get
    End Property

    Public ReadOnly Property Height() As Integer
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Height", value) Then
                If value IsNot Nothing Then

                    Dim i As Integer = 0

                    exp = New Regex("([ 0-9,]+)[pixels]*")
                    exp_matches = exp.Matches(value)

                    If exp_matches.Count > 0 Then
                        value = exp_matches.Item(0).Value
                        value = exp.Replace(value, "$1").Replace(" ", "").Replace(",", "").Trim
                        If Integer.TryParse(value, i) Then
                            Return i
                        End If
                    End If

                End If
            End If

            Return 0

        End Get
    End Property

    Public ReadOnly Property Resolution() As Long
        Get
            Return (Width * Height)
        End Get
    End Property

    Public ReadOnly Property FrameSize() As String
        Get
            Return (Width.ToString + "x" + Height.ToString)
        End Get
    End Property

    Public ReadOnly Property FrameRate() As Double
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Frame rate", value) Then
                If value IsNot Nothing Then

                    Dim i As Double = 0

                    exp = New Regex("([ 0-9.,]+)[fps]*")
                    exp_matches = exp.Matches(value)

                    If exp_matches.Count > 0 Then
                        value = exp_matches.Item(0).Value
                        value = exp.Replace(value, "$1").Replace(" ", "").Replace(",", "").Trim
                        If Double.TryParse(value, i) Then

                            If i >= 24.97 And i <= 25.2 Then
                                i = 25
                            End If

                            Return i
                        End If
                    End If

                End If
            End If

            Return 0
        End Get
    End Property

    Public ReadOnly Property AspectRatio() As Double
        Get
            Dim i As Double = 0
            Try
            Dim value As String = Nothing
            If Properties.TryGetValue("Display aspect ratio", value) Then
                If value IsNot Nothing AndAlso value.contains(":") Then
                    Dim s() As String = value.Split(":")
                    If s(0) <> "" Then 
                        Double.TryParse(s(0), Globalization.NumberStyles.Any, Globalization.CultureInfo.CreateSpecificCulture("en-US"), i)
                    End If
                    'Return i
                End If
            End If
            Catch
            End Try
            Return i
        End Get
    End Property

    Public ReadOnly Property IsInterlaced() As Boolean
        Get
            Dim value As String = Nothing
            Dim displayAspect As Double = 1
            If Properties.TryGetValue("Scan type", value) Then
                If value = "Interlaced" Then
                    Return True
                End If
            End If

            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Dim o As String = ""
            If FormatID <> "" Then
                o += ", " + FormatID
            End If
            'If PixelFormat <> "" Then
            '    o += ", " + PixelFormat
            'End If
            If Bitrate <> 0 Then
                o += ", " + Bitrate.ToString + " kbps"
            End If
            If FrameRate <> 0 Then
                o += ", " + FrameRate.ToString + " fps"
            End If
            If FrameSize <> "" Then
                o += ", " + FrameSize
            End If
            If o.Trim <> "" Then
                o = o.Trim.Remove(0, 1).Trim
            End If
            Return o
        End Get
    End Property
End Class
