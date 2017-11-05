
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

Public Class MediaInfo_Stream
    Public SourceFile As MediaFile
    Public StreamIndex As Integer
    Public StreamTypeIndex As Integer
    Public Properties As Dictionary(Of String, String)
    Protected exp As Regex
    Protected exp_matches As MatchCollection

    Sub New()
        Properties = New Dictionary(Of String, String)
    End Sub

    Public Overridable ReadOnly Property StreamType() As String
        Get
            Return ""
        End Get
    End Property

    Public Function GetProperty(ByVal Name As String) As String
        If Properties.ContainsKey(Name) Then
            Return Properties(Name)
        Else
            Return ""
        End If
    End Function

    Public Overridable ReadOnly Property Bitrate() As Integer
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Bit rate", value) Then
                If value IsNot Nothing Then

                    Dim i As Integer = 0

                    exp = New Regex("([ 0-9.,]+)[Kbps]*")
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

    Public Overridable ReadOnly Property Format() As String
        Get
            Return GetProperty("Format")
        End Get
    End Property

    Public Overridable ReadOnly Property FormatID() As String
        Get
            Dim value As String = GetProperty("Format")
            Return value
        End Get
    End Property

    Public Overridable ReadOnly Property CodecID() As String
        Get
            Return GetProperty("Codec ID")
        End Get
    End Property

    Public ReadOnly Property StreamSize() As Long
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("StreamSize", value) Then
                Dim i As Long = 0
                If value.Contains(" KiB") Then
                    i = CDbl(value.Replace(" KiB", "").Replace(" ", "").Replace(",", "").Trim) * KB
                ElseIf value.Contains(" MiB") Then
                    i = CDbl(value.Replace(" MiB", "").Replace(" ", "").Replace(",", "").Trim) * MB
                ElseIf value.Contains(" GiB") Then
                    i = CDbl(value.Replace(" GiB", "").Replace(" ", "").Replace(",", "").Trim) * GB
                End If
                Return i
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property ID() As Integer
        Get
            Dim value As String = Nothing
            Dim sid As Integer = Nothing
            If Properties.TryGetValue("ID", value) Then
                value = value.Trim
                If value.Contains(" ") Then
                    value = value.Split(" ")(0)
                End If
                If Integer.TryParse(value, sid) Then
                    Return sid
                End If
            End If
            Return -1
        End Get
    End Property

    Public ReadOnly Property DurationMillis() As Long
        Get
            Dim value As String = Nothing
            If Properties.TryGetValue("Duration", value) Then
                If value IsNot Nothing Then

					If value.Contains(":") Then
						'Example: 02:15:30.004
						Return getMillisFromString(value)
					Else

						value = value.Replace(" h", "h")
						value = value.Replace(" min", "mn")
                        value = value.Replace(" s", "s")
                        value = value.Replace(" ms", "ms")


						'Example: 2h 15mn 30s 4ms
						Dim x As Long = 0
                        Dim p() As String = Split(value)
                        For Each s As String In p
                            If s.Contains("ms") Then
                                x += CLng(Trim(s.Replace("ms", "")))
                            ElseIf s.Contains("s") Then
                                x += CLng(Trim(s.Replace("s", ""))) * 1000
                            ElseIf s.Contains("mn") Then
                                x += CLng(Trim(s.Replace("mn", ""))) * MIN * 1000
                            ElseIf s.Contains("h") Then
                                x += CLng(Trim(s.Replace("h", ""))) * HR * 1000
                            End If
                        Next
                        Return x
                    End If

                End If
            End If

            Return 0
        End Get
    End Property

    Public ReadOnly Property DurationString() As String
        Get
            Dim d As Long = DurationMillis
            If d > 0 Then
                Return (TimeSpan.FromSeconds(Math.Floor(d / 1000)).ToString())
            Else
                Return "00:00:00"
            End If
        End Get
    End Property

    Public ReadOnly Property DurationStringAccurate() As String
        Get
            Dim d As Long = DurationMillis
            If d > 0 Then
                Return (TimeSpan.FromSeconds(Math.Floor(d / 1000)).ToString() + "." + (d Mod 1000).ToString("000"))
            Else
                Return "00:00:00.000"
            End If
        End Get
    End Property

    Public Overridable ReadOnly Property Description() As String
        Get
            Return ""
        End Get
    End Property

    Overrides Function toString() As String
        Return Description
    End Function

End Class