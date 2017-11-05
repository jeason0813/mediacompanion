
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

Public Class MediaInfo_Stream_Text
    Inherits MediaInfo_Stream

    Public Overrides ReadOnly Property StreamType() As String
        Get
            Return "Text"
        End Get
    End Property

    Public ReadOnly Property Language() As String
        Get
            Return GetProperty("Language")
        End Get
    End Property

    Public ReadOnly Property MPlayerID() As String
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("MPlayer -sid", value) Then
                Return value
            Else
                Return ""
            End If
        End Get
    End Property

    Public Overrides ReadOnly Property FormatID() As String
        Get
            Dim value As String = GetProperty("Format")

            If value <> "" Then
                Select Case value
                    Case Else
                        Return value.ToUpper
                End Select
            End If

            value = GetProperty("Codec ID")
            If value <> "" Then
                Return value.ToUpper
            End If

            Return ""
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Dim o As String = ""
            If Format <> "" Then
                o += ", " + Format
            End If
            If Language <> "" Then
                o += ", " + Language
            End If
            If o.Trim <> "" Then
                o = o.Trim.Remove(0, 1).Trim
            End If
            Return o
        End Get
    End Property

End Class