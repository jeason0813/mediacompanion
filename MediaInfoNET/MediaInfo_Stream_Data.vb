
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

Public Class MediaInfo_Stream_Data 'Any unknown stream type
    Inherits MediaInfo_Stream

    Public Overrides ReadOnly Property StreamType() As String
        Get
            Return "Data"
        End Get
    End Property

    Public Overrides ReadOnly Property FormatID() As String
        Get
            Dim value As String = GetProperty("Format")

            If value <> "" Then
                Select Case value
                    Case "AVC"
                        value = "mpeg4avc"

                    Case "MPEG-4 Visual", "MS-MPEG4 v1", "MS-MPEG4 v2", "MS-MPEG4 v3", "S-Mpeg 4 v2", "S-Mpeg 4 v3"
                        Return "mpeg4asp"

                    Case "RealVideo 1", "RealVideo 2", "RealVideo 3", "RealVideo 4"
                        Return "rv"

                    Case "MPEG Video"
                        Return "mpeg"

                    Case Else
                        Return value.ToLower
                End Select
            Else
                value = GetProperty("Codec ID")
                Select Case value
                    Case "rawvideo"
                        Return "raw"
                    Case "dvvideo"
                        Return "dv"
                    Case Else
                        Return value.ToLower
                End Select
            End If

            Return ""
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
            If o.Trim <> "" Then
                o = o.Trim.Remove(0, 1).Trim
            End If
            Return o
        End Get
    End Property
End Class
