Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Data


Public Module ModuleExtensions

    <Extension()> _
    Public Function ToInt(ByVal s As String) As Integer
        Dim num as Integer

        If Integer.TryParse(s.Replace(",",""), num) Then Return num
        
        Return 0
    End Function

  
    <Extension()> _
    Public Function ToRating(ByVal s As String) As Double
        Dim num As Double
        Dim x   As String = s
                            
        x = x.Replace("/10", "" )
        x = x.Replace(" "  , "" )
        x = x.Replace(","  , ".")

        If Double.TryParse(x, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), num) Then 
            If num>=0 and num<=10 Then Return num
        End If
        
        Return 0
    End Function


    <Extension()> _
    Public Function FormatRating(ByVal s As String) As String
        Dim numRating As Single

        If s = "" Or Not Single.TryParse(s, numRating) Then
            Return s
        End If

        numRating = Math.Min(numRating,10)

        Return numRating.ToString("f1")
    End Function


    <Extension()> _
    Public Function RemoveAfterMatch(ByVal s As String, Optional match As String=" (") As String
        If String.IsNullOrEmpty(s) Then Return s
        Dim i As Integer = s.LastIndexOf(match)

        If i=-1 Then Return s
        
        Return s.Substring(0,i)
    End Function


    <Extension()> _
    Public Function RemoveLastChar(ByVal s As String) As String
        Return s.Remove(s.Length-1)
    End Function

    <Extension()> _
    Public Function GetLastChar(ByVal s As String) As String
        Return s(s.Length-1)
    End Function

    
    <Extension()> _
    Public Function SafeTrim(ByVal s As String) As String

        If IsNothing(s) Then Return s

        Return s.Trim
    End Function

    <Extension()> _
    Public Sub AppendLine(tb As TextBox, value As String)
        If tb.Text.Length=0 Then
            tb.Text = value
        Else
            tb.AppendText( Environment.NewLine + value )
        End If
    End Sub

    <Extension()> _
    Public Function RemoveWhitespace(input As String) As String
        If IsNothing(input) Then Return ""
        Return New String(input.ToCharArray().Where(Function(c) Not [Char].IsWhiteSpace(c)).ToArray())
    End Function

    Public Const Missing = "Missing"

    <Extension()> _
    Public Function IfBlankMissing(value As String) As String
        Return If(value="",Missing,value)
    End Function

    <Extension()> _
    Public Sub AddIfNew(Of T)(source As List(Of T), item As Object)
        If Not source.Contains(item) then
            source.Add(item)
        End if        
    End Sub

    <Extension()> _
    Public Function ToVotes(input As String, thou As Boolean) As String
        If Not String.IsNullOrEmpty(input) then
            If Not thou Then
                input = input.Replace(",", "")
            Else
                If Not input.Contains(",") Then
                    If input.Length > 3 Then
                        input = input.Insert(input.Length-3, ",")
                    End If
                    If input.Length > 7 Then
                        input = input.Insert(input.Length-7, ",")
                    End If
                End If
            End If
        End If
        Return input
    End Function

    <Extension()> _
    Public Function ToMin(input As String) As String
        If Not String.IsNullOrEmpty(input) Then
            'Dim minutes As String = mov.fullmoviebody.runtime
            input = input.Replace("minutes", "")
            input = input.Replace("mins", "")
            input = input.Replace("min", "")
            input = input.Replace(" ", "")
        Else
            input = "0"
        End If
        Return input
    End Function

    <Extension()> _
    Public Function Pad(input As String) As String
        If Not String.IsNullOrEmpty(input) Then
            input = input & "0000"
        End If
        Return input
    End Function


    <Extension()> _
    Public Function CopyToDataTable(Of T)(ByVal source As IEnumerable(Of T)) As DataTable
        Return New ObjectShredder(Of T)().Shred(source, Nothing, Nothing)
    End Function

    <Extension()> _
    Public Function CopyToDataTable(Of T)(ByVal source As IEnumerable(Of T), ByVal table As DataTable, ByVal options As LoadOption?) As DataTable
        Return New ObjectShredder(Of T)().Shred(source, table, options)
    End Function

End Module

