
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

Public Class MediaInfo_Stream_Audio
    Inherits MediaInfo_Stream

    Public Overrides ReadOnly Property StreamType() As String
        Get
            Return "Audio"
        End Get
    End Property

    Public Overrides ReadOnly Property FormatID() As String
        Get
            Dim value As String = GetProperty("Format")
            Dim fopro As String = GetProperty("Format profile")
            Dim jointstr As String =""

            If value <> "" Then
                Select Case value.ToLower
                    Case "mpeg audio"
                        Select Case GetProperty("Format profile").ToLower()
                            Case "layer 2"
                                Return "MP2"
                            Case "layer 3"
                                Return "MP3"
                        End Select

                    Case "2048"
                        Return "SONIC"

                    Case "ac-3"
                        Return "AC3"

                    Case "wma1", "wma2", "wmav1", "wmav2"
                        Return "WMA"

                    Case "dts"
                        jointstr="dts "+fopro
                        Return jointstr

                    Case Else 'PCM, FLAC, AAC, Vorbis
                        Return value.ToUpper

                End Select
            Else
                value = GetProperty("Codec ID")

                If value.Contains("pcm") Then
                    Return "PCM"
                Else
                    Return value.ToUpper
                End If
            End If

            Return ""
        End Get
    End Property

    Public ReadOnly Property MPlayerID() As String
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("MPlayer -aid", value) Then
                Return value
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property SamplingRate() As Integer
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Sampling rate", value) Then
                If value IsNot Nothing Then

                    Dim i As Integer = 0
                    Dim r As Double = 0

                    exp = New Regex("([ 0-9.,]+)KHz*")
                    exp_matches = exp.Matches(value)

                    If exp_matches.Count > 0 Then
                        value = exp_matches.Item(0).Value
                        value = exp.Replace(value, "$1").Replace(" ", "").Replace(",", "").Trim
                        If Double.TryParse(value, r) Then
                            Return CInt(r * 1000)
                        End If
                    End If

                    exp = New Regex("([ 0-9.,]+)Hz*")
                    exp_matches = exp.Matches(value)

                    If exp_matches.Count > 0 Then
                        value = exp_matches.Item(0).Value
                        value = exp.Replace(value, "$1").Replace(" ", "").Replace(",", "").Trim
                        If Double.TryParse(value, r) Then
                            Return CInt(r)
                        End If
                    End If
                End If
            End If

            Return 0
        End Get
    End Property

    Public ReadOnly Property Channels() As Integer
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Channel(s)", value) Then
                If value IsNot Nothing Then

                    Dim i As Integer = 0
                    Dim r As Double = 0

                    exp = New Regex("([ 0-9.]+)[channels]*")
                    exp_matches = exp.Matches(value)

                    If exp_matches.Count > 0 Then
                        value = exp_matches.Item(0).Value
                        value = exp.Replace(value, "$1").Replace(" ", "").Replace(",", "").Trim
                        If Double.TryParse(value, r) Then
                            i = Math.Ceiling(r)
                            Return i
                        End If
                    End If

                End If
            End If

            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Dim o As String = ""
            If FormatID <> "" Then
                o += ", " + FormatID
            End If
            If Bitrate <> 0 Then
                o += ", " + Bitrate.ToString + " kbps"
            End If
            If Channels <> 0 Then
                o += ", " + Channels.ToString + " ch"
            End If
            If SamplingRate <> 0 Then
                o += ", " + SamplingRate.ToString + " hz"
            End If
            If o.Trim <> "" Then
                o = o.Trim.Remove(0, 1).Trim
            End If
            Return o
        End Get
    End Property
End Class