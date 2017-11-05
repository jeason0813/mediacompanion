
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

Public Class MediaInfo_Stream_Image
    Inherits MediaInfo_Stream

    Public Overrides ReadOnly Property StreamType() As String
        Get
            Return "Image"
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

    Public Overrides ReadOnly Property Description() As String
        Get
            Dim o As String = ""
            If Format <> "" Then
                o += ", " + Format
            ElseIf CodecID <> "" Then
                o += ", " + CodecID
            End If
            If PixelFormat <> "" Then
                o += ", " + PixelFormat
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