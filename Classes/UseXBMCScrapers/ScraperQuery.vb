' XScraperLib - .NET library for accessing XBMC-style metadata scrapers.
' Copyright (C) 2010  John Klimek

' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' any later version.

' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.

' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Option Strict On
Option Explicit On

Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Net
Imports System.Linq

Public Class ScraperQuery

#Region "Constants"
   Const MAX_VARS As Integer = 99
   Const MAX_FIELDS As Integer = 99
   Const DEFAULT_EXPRESSION As String = "(.*)"
#End Region

#Region "Fields"
   ' Important!  Buffer 0 (ie. mVars(0)) is UNUSED.
   Private mVars(MAX_VARS) As String
   Private mScraper As Scraper
   Private mLastFunctionResult As String
#End Region

#Region "Properties"
   ''' <summary>
   ''' Variables (buffers) used for scraper query.
   ''' </summary>
   ''' <value>Index of variable/buffer.</value>
   ''' <returns>Contents of specified variable/buffer.</returns>
   ''' <remarks></remarks>
   Public ReadOnly Property Variables() As String()
      Get
         Return mVars
      End Get
   End Property

   ''' <summary>
   ''' The string result of the last function executed.
   ''' </summary>
   Public ReadOnly Property LastFunctionResult() As String
      Get
         Return mLastFunctionResult
      End Get
   End Property
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates a scraper query object for the given scraper.
   ''' </summary>
   ''' <param name="scraper">Scraper to be used for query/function.</param>
   Sub New(ByVal scraper As Scraper)
      mScraper = scraper
   End Sub
#End Region

#Region "Execute"
   ''' <summary>
   ''' Executes scraper function with no parameters.
   ''' </summary>
   ''' <param name="functionName">Scraper function name.</param>
   ''' <returns>Result of scraper function.</returns>
   Public Function Execute(ByVal functionName As String) As String
      Return Execute(functionName, True)
   End Function

   ''' <summary>
   ''' Executes scraper function with no parameters.
   ''' </summary>
   ''' <param name="functionName">Scraper function name.</param>
   ''' <param name="convertAmpersands">Convert ampersands to &amp; to fix invalid XML.</param>
   ''' <returns>Result of scraper function.</returns>
   Public Function Execute(ByVal functionName As String, ByVal convertAmpersands As Boolean) As String
      Return Me.Execute(functionName, New String() {}, convertAmpersands)
   End Function

   ''' <summary>
   ''' Executes scraper function with given parameters.
   ''' </summary>
   ''' <param name="functionName">Scraper function name.</param>
   ''' <param name="params">Parameters to pass to scraper function.</param>
   ''' <returns>Result of scraper function.</returns>
   Public Function Execute(ByVal functionName As String, ByRef params() As String) As String
      Return Execute(functionName, params, True)
   End Function

   ''' <summary>
   ''' Executes scraper function with given parameters.
   ''' </summary>
   ''' <param name="functionName">Scraper function name.</param>
   ''' <param name="params">Parameters to pass to scraper function.</param>
   ''' <param name="convertAmpersands">Convert ampersands to &amp; to fix invalid XML.</param>
   ''' <returns>Result of scraper function.</returns>
   Public Function Execute(ByVal functionName As String, ByRef params() As String, ByVal convertAmpersands As Boolean) As String
      ' Retrieve the function element
      Dim functionElement As XElement = mScraper(functionName)

      If (functionElement Is Nothing) Then
         Throw New FunctionNotFound(functionName)
      End If

      Return Execute(functionElement, params, convertAmpersands)
   End Function

   ''' <summary>
   ''' Executes scraper function with given parameters.
   ''' </summary>
   ''' <param name="functionElement">Function element to be used.</param>
   ''' <param name="params">Parameters to pass to scraper function.</param>
   ''' <param name="convertAmpersands">Convert ampersands to &amp; to fix invalid XML.</param>
   ''' <returns>Result of scraper function.</returns>
   Private Function Execute(ByVal functionElement As XElement, ByRef params() As String, ByVal convertAmpersands As Boolean) As String
      Const AmpersandRegExReplace As String = "&(?!quot;|apos;|amp;|lt;|gt;#x?.*?;)"

      ' Insert (copy) our initialization variables/buffers
      InitializeBuffers(params)

      ' Perform the regular expression loop
      PerformRegEx(functionElement)

      ' Retrieve the result of our function (specified by the dest attribute)
      Dim functionResult As String = mVars(CInt(functionElement.@dest))

      Dim outputElement As XElement

      ' Convert ampersands to &amp; if requested.  This fixes invalid XML generated from certain scrapers.
        ' I've inserted this check below, for handling null result
        If functionResult Is Nothing Then
            Return ""
        End If
        outputElement = XElement.Parse(Regex.Replace(functionResult, AmpersandRegExReplace, "&amp;"))
        'If (convertAmpersands) Then
        'Else
        '    outputElement = XElement.Parse(functionResult)
        'End If
        ' Finally, process any chain and url elements and return the final result
        mLastFunctionResult = ProcessChainAndUrlElements(outputElement).ToString()
        If (Not convertAmpersands) Then mLastFunctionResult = Regex.Replace(mLastFunctionResult, "&amp;", "&")
        Return mLastFunctionResult
    End Function

   ''' <summary>
   ''' Initializes (inserts/copies) specified parameters into variable buffers.
   ''' </summary>
   ''' <param name="params">Parameter array.  Index 0 will be inserted into $$1, etc.</param>
   Private Sub InitializeBuffers(ByRef params() As String)
      If params.Length > mVars.Length - 1 Then Throw New Exception("Initialization parameter count cannot exceed MAX_VARS (" + CStr(MAX_VARS) + ")")

      ' Clear out the array
      Array.Clear(mVars, 0, mVars.Length)

      ' Copy the given parameters into our variable/buffer array
      Array.Copy(params, 0, mVars, 1, params.Length)
   End Sub
#End Region

#Region "Utility/Private Methods"
   ''' <summary>
   ''' Replaces buffer variables ($$1, $$2, etc) with buffer contents.
   ''' </summary>
   ''' <param name="input">String containing buffer variables.</param>
   ''' <returns>Input string with with buffer variables replaced with buffer contents.</returns>
   Private Function ReplaceBuffers(ByVal input As String) As String
      ' We are using regular expressions for buffer and setting replacements due to speed
      ' (eg. they are MUCH, MUCH faster than looping through the variable array, etc)
      Const BufferReplacementRegEx As String = "\$\$(\d\d?)"
      Const SettingReplacementRegex As String = "\$INFO\[([^\]]+)\]"

      Dim result As String = input
        If IsNothing(result) Then Return ""
        ' I've inserted the Try routine below, for handling null result
        Try ' isto é meu
            For Each m As Match In Regex.Matches(result, BufferReplacementRegEx)
                If (m.Groups.Count > 1) Then
                    result = result.Replace("$$" + m.Groups(1).Value, mVars(CInt(m.Groups(1).Value)))
                End If
            Next

            For Each m As Match In Regex.Matches(result, SettingReplacementRegex)
                Try
                    If (m.Groups.Count > 1) Then
                        result = result.Replace("$INFO[" + m.Groups(1).Value + "]", mScraper.GetSetting(m.Groups(1).Value).ValueString)
                    End If
                Catch
                End Try
            Next
        Catch
            result = ""
        End Try
        Return result
    End Function

   ''' <summary>
   ''' Replaces regex captures inside given output string.  (eg. \1 will be replaced with capture group one, etc)
   ''' </summary>
   ''' <param name="match">Regular expression match result.</param>
   ''' <param name="output">Output string containing replacement strings (\1, \2, etc) to be replaced.</param>
   ''' <param name="nocleanFields">Array of fields we will NOT clean.  (eg. if array index one is true, field one will NOT be cleaned)</param>
   ''' <param name="trimFields">Array of fields to trim.  (eg. if array index one is true, we will trim field one)</param>
   ''' <param name="encodeFields">Array of fields to URL encode.  (eg. if array index one is true, we will URL encode field one)</param>
   ''' <param name="fixcharsFields">Array of fields to "fix chars".  (eg. if array index one is true, we will "fix chars" field one)</param>
   ''' <returns>Output string with capture groups replaced.</returns>
   ''' <remarks></remarks>
   Private Function ReplaceRegExCaptures(ByVal match As Match, ByVal output As String, ByRef nocleanFields() As Boolean, ByRef trimFields() As Boolean, ByVal encodeFields() As Boolean, ByVal fixcharsFields() As Boolean) As String
      For i As Integer = 1 To match.Groups.Count - 1
         Dim captureValue As String = match.Groups(i).Value

         ' Here we will clean, trim, encode, and/or "fixchars" the capture field if requested.
         If (Not nocleanFields(i)) Then captureValue = Clean(captureValue)
         If (trimFields(i)) Then captureValue = Trim(captureValue)
         If (encodeFields(i)) Then captureValue = UrlEncode(captureValue)
         If (fixcharsFields(i)) Then captureValue = FixChars(captureValue)

         output = output.Replace("\" + CStr(i), match.Groups(i).Value)
      Next

      Return output
   End Function

   ''' <summary>
   ''' Removes HTML/XML tags from given string.
   ''' </summary>
   ''' <param name="s">String to remove HTML/XML tags from.</param>
   ''' <returns>String with HTML/XML tags removed.</returns>
   Private Function RemoveTags(ByVal s As String) As String
      ' See:  HTMLUtil.cpp, CHTMLUtil::RemoveTags()
      '   Looks for < and > and removes them and anything inside.  (ie. simple tag removal)

      ' http://weblogs.asp.net/rosherove/archive/2003/05/13/6963.aspx
      ' string stripped = Regex.Replace(textBox1.Text,@"<(.|\n)*?>",string.Empty);

      Return Regex.Replace(s, "<(.|\n)*?>", String.Empty)
   End Function

   ''' <summary>
   ''' Removes tab, carriage return, and new line characters from the front and end of the line of the given string.  (repeating)
   ''' </summary>
   ''' <param name="s">String to search and replace.</param>
   ''' <returns>String with special characters remove.</returns>
   Private Function Trim(ByVal s As String) As String
      ' ^  = start of line
      ' $  = end of line
      ' [] = set of characters
      ' +  = match one or more times 

      Return Regex.Replace(s, "^[\t\n\r]+|[\t\n\r]+$", String.Empty)
   End Function

   Private Function FixChars(ByVal s As String) As String
      ' http://trac.xbmc.org/changeset/31124
      ' "converts html entities"

      'Throw New NotImplementedException("FixChars is not implemented yet.")

      'strBuffer = strDirty.substr(i+14,i2-i-14);
      'CStdStringW wbuffer;
      'g_charsetConverter.toW(strBuffer,wbuffer,GetSearchStringEncoding());
      'CStdStringW wConverted;
      'HTML::CHTMLUtil::ConvertHTMLToW(wbuffer,wConverted);
      'g_charsetConverter.fromW(wConverted,strBuffer,GetSearchStringEncoding());
      'RemoveWhiteSpace(strBuffer);
      'strDirty.erase(i,i2-i+14);
      'strDirty.Insert(i,strBuffer);
      'i += strBuffer.size();

      Return s
   End Function

   ''' <summary>
   ''' Returns a URL encoded string.
   ''' </summary>
   ''' <param name="s">String to URL encode.</param>
   ''' <returns>URL encoded string.</returns>
   Private Function UrlEncode(ByVal s As String) As String
      ' See CUtil::URLEncode()
        Return Uri.EscapeUriString(s)
   End Function

   ''' <summary>
   ''' Removes tags (RemoveTags()) and trims (Trim()) the given input string.
   ''' </summary>
   ''' <param name="s">String to remove tags and trim.</param>
   ''' <returns>Trimmed and tag removed string.</returns>
   Private Function Clean(ByVal s As String) As String
      Return Trim(RemoveTags(s))
   End Function
#End Region

#Region "Public Methods"
   ''' <summary>
   ''' Returns the entire variable/buffer list as a string (with CR+LF).  Used for debugging.
   ''' </summary>
   Public Function GetVariableListAsString() As String
      Dim result As String = String.Empty

      For i As Integer = 1 To mVars.Length - 1
         If mVars(i) <> String.Empty Then
            result += "Buffer (" + CStr(i) + "): " + mVars(i) + vbCrLf
         End If
      Next

      Return result
   End Function

   ''' <summary>
   ''' Recursively performs regular expression function starting with the given function element.
   ''' </summary>
   ''' <param name="functionElement">Function element to be used.</param>
   ''' <remarks>This will recursively perform regular expressions from inner to outer and then top to bottom.
   ''' Chain and URL functions are included as-is are not processed (resolved).</remarks>
   Private Sub PerformRegEx(ByVal functionElement As XElement)
      ' Loop through each sibling RegEx
      For Each e In functionElement.<RegExp>
         ' Does this sibling have any RegExp children?  If so, recursively call it
         If e.<RegExp>.Count > 0 Then
                PerformRegEx(e)
         End If

         ' =====================================================================================
         '  RegExp element attributes
         ' =====================================================================================
         Dim input As String

         ' If the input attribute is missing we default to the first buffer for input
         If (e.@input = String.Empty) Then
                input = ReplaceBuffers("$$1")
         Else
            input = ReplaceBuffers(e.@input)
         End If

         Dim output As String = ReplaceBuffers(e.@output)
         Dim destination As String = e.@dest
            On Error Resume Next  'isto é meu ' I've inserted the On Error, for handling null result coming from ReplaceBuffer
            Dim destinationAppend As Boolean = If(IsNothing(e.@dest), False, (e.@dest.Contains("+")))
            On Error GoTo 0 'isto é meu' I've inserted the On Error GoTo 0 for restoring error trapping again
            Dim conditional As String = e.@conditional

            ' =====================================================================================
            '  If conditional exists then let's check it before we go any further
            ' =====================================================================================
            If (conditional <> String.Empty) Then
                Dim setting As ScraperSetting = mScraper.GetSetting(conditional.Replace("!", ""))

                If (setting IsNot Nothing) AndAlso (TypeOf (setting) Is ScraperSettingBool) Then
                    Dim reverseCondition As Boolean = False

                    If (conditional.Contains("!")) Then
                        reverseCondition = True
                    End If

                    If (((Not CType(setting, ScraperSettingBool).Value) And Not reverseCondition) Or _
                             (CType(setting, ScraperSettingBool).Value And reverseCondition)) Then
                        Trace.WriteLine("Skipping RegEx because of conditional: " + conditional + " (" + setting.ValueString + ")")
                        Continue For
                    End If
                ElseIf (setting Is Nothing) Then
                    Trace.WriteLine("WARNING: Can't find setting (" + conditional + ") for conditional check.  Skipping check.")
                End If
            End If

            ' =====================================================================================
            '  Expression element attributes/value
            ' =====================================================================================
            Dim expression As String = DEFAULT_EXPRESSION
            Dim clear As Boolean = False
            Dim repeat As Boolean = False
            Dim nocleanFields(MAX_FIELDS) As Boolean   ' defaults to false for each index
            Dim trimFields(MAX_FIELDS) As Boolean      ' defaults to false for each index
            Dim encodeFields(MAX_FIELDS) As Boolean    ' defaults to false for each index
            Dim fixcharsFields(MAX_FIELDS) As Boolean  ' defaults to false for each index

            ' =====================================================================================
            ' Does the expression child exist?  If so we'll parse it's attributes and value
            ' =====================================================================================
            If (e.<expression>.Count > 0) Then
                If (e.<expression>.Value <> String.Empty) Then expression = e.<expression>.Value
                If (e.<expression>.@clear = "yes") Then clear = True
                If (e.<expression>.@repeat = "yes") Then repeat = True

                If (e.<expression>.@trim <> String.Empty) Then
                    For Each i As Integer In e.<expression>.@trim.Split(CChar(","))
                        trimFields(i) = True
                    Next
                End If

                If (e.<expression>.@noclean <> String.Empty) Then
                    For Each i As Integer In e.<expression>.@noclean.Split(CChar(","))
                        nocleanFields(i) = True
                    Next
                End If

                If (e.<expression>.@encode <> String.Empty) Then
                    For Each i As Integer In e.<expression>.@encode.Split(CChar(","))
                        encodeFields(i) = True
                    Next
                End If

                If (e.<expression>.@fixchars <> String.Empty) Then
                    For Each i As Integer In e.<expression>.@fixchars.Split(CChar(","))
                        fixcharsFields(i) = True
                    Next
                End If
            End If

            ' =====================================================================================
            '  Execute the actual regular expression, then replace captures ("fields") in output, 
            '  and finally replaces buffer variables
            ' =====================================================================================
            'Trace.WriteLine("=================================================================")
            'Trace.WriteLine("Performing Regular Expression     " + "Destination: " + destination + " (Append? " + (destinationAppend = True).ToString() + ")")
            'Trace.WriteLine("=================================================================")
            'Trace.WriteLine(e.ToString())
            'Trace.WriteLine("=================================================================" + vbCrLf)

            Dim matches As MatchCollection = Regex.Matches(input, WebUtility.HtmlDecode(expression), RegexOptions.Singleline)

            If ((matches.Count = 0) And (Not clear)) Then
                Continue For
            ElseIf ((matches.Count = 0) And (clear)) Then
                output = String.Empty
            Else
                If (repeat) Then
                    Dim finalOutput As String = String.Empty

                    For Each m As Match In matches
                        finalOutput += ReplaceBuffers(ReplaceRegExCaptures(m, output, nocleanFields, trimFields, encodeFields, fixcharsFields))
                    Next

                    output = finalOutput
                Else
                    output = ReplaceBuffers(ReplaceRegExCaptures(matches(0), output, nocleanFields, trimFields, encodeFields, fixcharsFields))
                End If
            End If

            ' =====================================================================================
            '  Place output into destination variable/buffer
            ' =====================================================================================
            If (destinationAppend) Then
                mVars(CInt(destination)) += output
            Else
                mVars(CInt(destination)) = output
            End If
        Next
   End Sub

   ''' <summary>
   ''' Process and replaces chain and url functions.
   ''' </summary>
   ''' <param name="rootElement">Root element of XML tree to look for chain and url functions.</param>
   ''' <returns>Root element with chain and url functions processed/replaced.</returns>
   Private Function ProcessChainAndUrlElements(ByVal rootElement As XElement) As XElement
      ' =====================================================================================
      '   Process each chain and/or url element (with a function) that we find
      '     NOTES:  Chain elements are processed first.
      ' =====================================================================================
      While (rootElement...<chain>.Count <> 0) Or (rootElement...<url>.Attributes("function").Count <> 0)
         Dim chainElement As XElement
         Dim functionParams As String()

         If (rootElement...<chain>.Count <> 0) Then
            chainElement = rootElement...<chain>(0)
            functionParams = New String() {chainElement.Value}
         Else
            chainElement = rootElement.Descendants("url").Attributes("function")(0).Parent

            ' Attempt to download/retrieve the url via cache manager or normal web client
            Try
               If (chainElement.@cache <> String.Empty) Then
                  functionParams = New String() {CacheManager.Instance.GetUrl(chainElement.Value, chainElement.@cache)}
               Else
                  Trace.WriteLine("Downloading: " + chainElement.Value)
                  functionParams = New String() {New WebClient().DownloadString(chainElement.Value)}
               End If
            Catch ex As WebException
               ' If an exception occurs while retrieving the URL we will remove (eg. ignore) this chain element and continue
               Trace.WriteLine("  Web Exception: " + ex.Message)
               chainElement.Remove()
               Continue While
            End Try

         End If

         Dim functionName As String = chainElement.@function

            Dim chainResultElement As XElement = Nothing
            ' I've inserted the Try routine below, for handling null result
            Try 'isto é meu
                Dim chainResult As String = ScraperQuery.ExecuteQuery(mScraper, functionName, functionParams)
                chainResultElement = XElement.Parse(chainResult)

            Catch
            End Try
            If chainResultElement IsNot Nothing Then
                ReplaceOrAddElements(rootElement, chainResultElement, chainElement)
            End If
            chainElement.Remove()
      End While

      Return rootElement
   End Function

   ''' <summary>
   ''' Parses replacement element and then replaces (or adds if not found) elements in the outputElement.
   ''' </summary>
   ''' <param name="outputElement">Output element tree containing XML elements that will be replace or added.</param>
   ''' <param name="replacementElement">Replacement element tree enclosed in details tags.</param>
   ''' <param name="chainElement">Chain element that we are processing.</param>
   ''' <remarks></remarks>
   Private Sub ReplaceOrAddElements(ByRef outputElement As XElement, ByVal replacementElement As XElement, ByVal chainElement As XElement)
      ' We are expecting a <details></details> tag so make sure it's the outermost element.
      If (replacementElement.Name.LocalName.ToUpper <> "DETAILS") Then
         Throw New OuterChainElementIncorrect(replacementElement.Name.LocalName)
      End If

      ' Tags that are allowed multiples times:  genre, credits, director, actor
      '   http://wiki.xbmc.org/index.php?title=Scrapers#.3CGetDetails.3E

      For Each e In replacementElement.Elements
         Dim tagName As String = e.Name.LocalName

         Select Case tagName
            Case "genre", "credits", "director", "actor", "country", "studio"
               If (outputElement.Elements(tagName).Count > 0) Then
                  If Not outputElement.LastNode.ToString = e.ToString  Then
                     outputElement.Elements(tagName).Last.AddAfterSelf(e)
                  End If
               Else
                  outputElement.LastNode.AddAfterSelf(e)
               End If
            Case "url"
               ' Special case (url tag)
               outputElement.LastNode.AddAfterSelf(e)
            Case Else
               ' All other tags simply get replaced (or added if they didn't exist)
               If (outputElement.Elements(tagName).Count > 0) Then
                  outputElement.Elements(tagName)(0).ReplaceAll(e)
               Else
                  outputElement.LastNode.AddAfterSelf(e)
               End If
         End Select
      Next

      ' Remove the chain element from our output so we don't reprocess it again
        'chainElement.Remove()
   End Sub
#End Region

#Region "Shared/Static Helper Functions"
   ''' <summary>
   ''' Helper function to create a scraper query and execute the specified function.
   ''' </summary>
   ''' <param name="scraper">Scraper to be used for query.</param>
   ''' <param name="functionName">Name of function to execute.</param>
   ''' <param name="params">Parameters to be passed to function.</param>
   ''' <returns>Result of function.</returns>
   Public Shared Function ExecuteQuery(ByVal scraper As Scraper, ByVal functionName As String, ByVal params() As String) As String
      Dim query As ScraperQuery = New ScraperQuery(scraper)
      Return query.Execute(functionName, params)
   End Function

   ''' <summary>
   ''' Helper function to create a scraper query and execute the specified function.
   ''' </summary>
   ''' <param name="scraper">Scraper to be used for query.</param>
   ''' <param name="functionName">Name of function to execute.</param>
   ''' <param name="params">Parameters to be passed to function.</param>
   ''' <param name="convertAmpersands">Convert ampersands to &amp; to fix invalid XML.</param>
   ''' <returns>Result of function.</returns>
   Public Shared Function ExecuteQuery(ByVal scraper As Scraper, ByVal functionName As String, ByVal params() As String, ByVal convertAmpersands As Boolean) As String
      Dim query As ScraperQuery = New ScraperQuery(scraper)
      Return query.Execute(functionName, params, convertAmpersands)
   End Function

   ''' <summary>
   ''' Helper function to create a scraper query and execute the specified function.
   ''' </summary>
   ''' <param name="scraper">Scraper to be used for query.</param>
   ''' <param name="functionName">Name of function to execute.</param>
   ''' <returns>Result of function.</returns>
   Public Shared Function ExecuteQuery(ByVal scraper As Scraper, ByVal functionName As String) As String
      Dim query As ScraperQuery = New ScraperQuery(scraper)
      Return query.Execute(functionName)
   End Function

   ''' <summary>
   ''' Helper function to create a scraper query and execute the specified function.
   ''' </summary>
   ''' <param name="scraper">Scraper to be used for query.</param>
   ''' <param name="functionName">Name of function to execute.</param>
   ''' <param name="convertAmpersands">Convert ampersands to &amp; to fix invalid XML.</param>
   ''' <returns>Result of function.</returns>
   Public Shared Function ExecuteQuery(ByVal scraper As Scraper, ByVal functionName As String, ByVal convertAmpersands As Boolean) As String
      Dim query As ScraperQuery = New ScraperQuery(scraper)
      Return query.Execute(functionName, convertAmpersands)
   End Function
#End Region

#Region "Comments/Documentation"
   ' fixchars

   ' CreateSearchUrl
   '   $$1 = full name
   '   $$2 = year (if found)

   ' GetSearchResults
   '   $$1 = url contents

   ' GetDetails
   '   $$1 = url contents
   '   $$2 = id (specific to each scraper but comes from GetSearchResults)

   '  <!-- input:	$1=html $2=search query -->
   '	<!-- returns:	results in xml format <results><movie><title>*</title><url>*</url>*#urls<extra>*</extra></movie>*</results> -->
#End Region

End Class