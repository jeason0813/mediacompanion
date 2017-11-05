
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

    Public Class MediaInfo_Stream_Chapters
        Inherits MediaInfo_Stream

        Public Overrides ReadOnly Property StreamType() As String
            Get
                Return "Chapters"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Dim o As String = ""
                If Format <> "" Then
                    o += " | " + Format
                End If
                If o.Trim <> "" Then
                    o = o.Trim.Remove(0, 1).Trim
                End If
                Return o
            End Get
        End Property

    End Class




