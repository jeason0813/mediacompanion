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

Public MustInherit Class ScraperSetting

#Region "Fields"
   Private mID As String
#End Region

#Region "Properties"
   ''' <summary>
   ''' Identifier (name) of setting.
   ''' </summary>
   Public Property ID() As String
      Get
         Return mID
      End Get
      Set(ByVal value As String)
         Me.mID = value
      End Set
   End Property
#End Region

#Region "MustOverride Properties"
   ''' <summary>
   ''' Value of setting converted to a string.  (ie. boolean true will be converted to string "true")
   ''' </summary>
   Public MustOverride ReadOnly Property ValueString() As String

   ''' <summary>
   ''' XBMC-style description for type of setting.  (ie. bool, labelenum, etc)
   ''' </summary>
   Public MustOverride ReadOnly Property SettingType() As String
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates a new scraper setting with the given element.  This constructor is only called by subclasses.
   ''' </summary>
   ''' <param name="e">XElement to be used for setting.</param>
   Sub New(ByVal e As XElement)
      Me.mID = e.@id
   End Sub
#End Region

End Class

Public Class ScraperSettingBool
   Inherits ScraperSetting

#Region "Fields"
   Private mDefault As Boolean
   Private mCurrentValue As Boolean
#End Region

#Region "Properties"
   ''' <summary>
   ''' Boolean value converted to string.  (ie. True is converted to "true")
   ''' </summary>
   Public Overrides ReadOnly Property ValueString() As String
      Get
         If (mCurrentValue) Then
            Return "true"
         Else
            Return "false"
         End If
      End Get
   End Property

   ''' <summary>
   ''' XBMC-style description for type of setting.  This class returns "bool".
   ''' </summary>
   Public Overrides ReadOnly Property SettingType() As String
      Get
         Return "bool"
      End Get
   End Property

   ''' <summary>
   ''' Current boolean value of setting.
   ''' </summary>
   Public Property Value() As Boolean
      Get
         Return mCurrentValue
      End Get
      Set(ByVal value As Boolean)
         mCurrentValue = value
      End Set
   End Property
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates a new scraper setting with the given element.
   ''' </summary>
   ''' <param name="e">XElement to be used for setting.</param>
   Sub New(ByVal e As XElement)
      MyBase.New(e)

      If ((e.@default <> String.Empty) AndAlso (e.@default.ToUpper() = "TRUE")) Then
         mDefault = True
      Else
         mDefault = False
      End If

      mCurrentValue = mDefault
   End Sub
#End Region

End Class

Public Class ScraperSettingEnum
   Inherits ScraperSetting

#Region "Fields"
   Private mDefault As String
   Private mValues As List(Of String)
   Private mCurrentValue As String
#End Region

#Region "Properties"
   ''' <summary>
   ''' Enumeration value converted to string.  (full string of current value is returned)
   ''' </summary>
   Public Overrides ReadOnly Property ValueString() As String
      Get
         Return Value
      End Get
   End Property

   ''' <summary>
   ''' XBMC-style description for type of setting.  This class returns "bool".
   ''' </summary>
   Public Overrides ReadOnly Property SettingType() As String
      Get
         Return "labelenum"
      End Get
   End Property

   ''' <summary>
   ''' Current enumeration value.  (full string of current value is returned)
   ''' </summary>
   Public Property Value() As String
      Get
         Return mCurrentValue
      End Get
      Set(ByVal value As String)
         If (Not mValues.Contains(value)) Then
            Throw New InvalidScraperSettingValue(Me, value)
         End If

         mCurrentValue = value
      End Set
   End Property
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates a new scraper setting with the given element.
   ''' </summary>
   ''' <param name="e">XElement to be used for setting.</param>
   Sub New(ByVal e As XElement)
      MyBase.New(e)

      mValues = New List(Of String)

      For Each s In Strings.Split(e.@values, "|")
         mValues.Add(s.Trim())
      Next

      If (e.@default <> String.Empty) Then
         Value = e.@default
      End If
   End Sub
#End Region

End Class

Public Class ScraperSettingText
   Inherits ScraperSetting

#Region "Fields"
   Private mDefault As String
   Private mValues As List(Of String)
   Private mCurrentValue As String
#End Region

#Region "Properties"
   ''' <summary>
   ''' Enumeration value converted to string.  (full string of current value is returned)
   ''' </summary>
   Public Overrides ReadOnly Property ValueString() As String
      Get
         Return Value
      End Get
   End Property

   ''' <summary>
   ''' XBMC-style description for type of setting.  This class returns "bool".
   ''' </summary>
   Public Overrides ReadOnly Property SettingType() As String
      Get
         Return "labelenum"
      End Get
   End Property

   ''' <summary>
   ''' Current enumeration value.  (full string of current value is returned)
   ''' </summary>
   Public Property Value() As String
      Get
         Return mCurrentValue
      End Get
      Set(ByVal value As String)

         mCurrentValue = value
      End Set
   End Property
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates a new scraper setting with the given element.
   ''' </summary>
   ''' <param name="e">XElement to be used for setting.</param>
   Sub New(ByVal e As XElement)
      MyBase.New(e)

      mValues = New List(Of String)
      mValues.Add(e.@default)
      Value = e.@default

   End Sub
#End Region

End Class
