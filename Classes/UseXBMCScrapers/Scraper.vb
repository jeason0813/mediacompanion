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

Imports System.Xml
Imports System.IO
Imports System.Linq

Public Class Scraper

#Region "Fields"
   Private mLibraryRootElement As XElement
   Private mFullTreeWithAddons As XElement

   Private mID As String
   Private mName As String
   Private mVersion As String
   Private mProviderName As String

   Private mRequiredAddons As List(Of String)

   Private mSettings As List(Of ScraperSetting)

   Private mAddonXmlFilename As String
   Private mLibraryXmlFilename As String

   Private mIsMovieScraper As Boolean
   Private mIsTVShowScraper As Boolean
   Private mIsLibraryScraper As Boolean

   Private mIsMultipleScraper As Boolean
#End Region

#Region "Properties"
   ''' <summary>
   ''' Library XML filename.  (ie. imdb.xml)
   ''' </summary>
   Public Property LibraryXmlFilename() As String
      Get
         Return mLibraryXmlFilename
      End Get
      Set(ByVal value As String)
         mLibraryXmlFilename = value
      End Set
   End Property

   ''' <summary>
   ''' ID of this scraper.  (ie. metadata.common.imdb.com)
   ''' </summary>
   Public Property ID() As String
      Get
         Return Me.mID
      End Get
      Set(ByVal value As String)
         Me.mID = value
      End Set
   End Property

   ''' <summary>
   ''' List of functions for this scraper returned as list of XElements.  This does NOT include imported functions.
   ''' </summary>
   ''' <remarks>This does not include imported functions.</remarks>
   Public ReadOnly Property Functions() As List(Of XElement)
      Get
         If (mLibraryRootElement Is Nothing) Then
            Return Nothing
         Else
            Return mLibraryRootElement.Elements.ToList()
         End If
      End Get
   End Property

   ''' <summary>
   ''' Returns contents (XElement) of specified function.  This will also search imported addons.
   ''' </summary>
   ''' <param name="functionName">Name of function to retrieve.</param>
   Default Public ReadOnly Property Func(ByVal functionName As String) As XElement
      Get
         Return Me.mFullTreeWithAddons.Elements(functionName).First()
      End Get
   End Property

   ''' <summary>
   ''' List of settings for this scraper.
   ''' </summary>
   Public ReadOnly Property Settings() As List(Of ScraperSetting)
      Get
         Return mSettings
      End Get
   End Property
#End Region

#Region "Overrides (ie. ToString)"
   Public Overrides Function ToString() As String
      If (mName = String.Empty) Then
         Return "<unknown>" + ", v" + mVersion
      Else
         Return mName + ", v" + mVersion
      End If
   End Function
#End Region

#Region "Private Methods"
   ''' <summary>
   ''' Finds all required addon imports and saves the list.
   ''' </summary>
   ''' <param name="manager">Scraper manager.</param>
   ''' <remarks></remarks>
   Private Sub FindRequirements(ByVal manager As ScraperManager)
      Dim loadedAddons As List(Of String) = New List(Of String)

      RecursiveGetRequirementsList(manager, loadedAddons)
      loadedAddons = loadedAddons.Distinct().ToList()

      ' Remove ourselves from the requirements list since we don't require ourselves!
      loadedAddons.Remove(Me.ID)

      ' Store the list of required addons
      mRequiredAddons = loadedAddons
   End Sub

   ''' <summary>
   ''' Recursively builds a list of required addon imports for a given addon.  (used by FindRequirements())
   ''' </summary>
   ''' <param name="manager">Scraper manager.</param>
   ''' <param name="loadedAddons">Generic list of strings that will be filled with a list of required addon imports.</param>
   ''' <remarks></remarks>
   Private Sub RecursiveGetRequirementsList(ByVal manager As ScraperManager, ByRef loadedAddons As List(Of String))
      ' Add ourselves to the list
      loadedAddons.Add(Me.ID)

      For Each r In mRequiredAddons
         Dim s As Scraper = manager.GetByID(r)

         If (s Is Nothing) Then
            Trace.WriteLine("ERROR:  Unable to find: " + r)
         Else
            s.RecursiveGetRequirementsList(manager, loadedAddons)
         End If
      Next
   End Sub
#End Region

#Region "Public Methods"
   ''' <summary>
   ''' Builds full XML tree containing functions from this scraper and all imported (required) scrapers.
   ''' </summary>
   ''' <param name="manager">Scraper manager object used to read other scraper functions.</param>
   ''' <param name="allowMissingRequirements">Allow required imports to be omitted if not found.</param>
   ''' <remarks></remarks>
   Public Sub BuildXmlWithRequiredAddons(ByVal manager As ScraperManager, ByVal allowMissingRequirements As Boolean)
      ' Make sure we have a valid root element before we start
      If (mLibraryRootElement Is Nothing) Then Return

      mFullTreeWithAddons = New XElement(mLibraryRootElement)

      For Each requiredAddon In mRequiredAddons
         ' Retrieve the scraper object for the required addon
         Dim scraper As Scraper = manager.GetByID(requiredAddon)

         ' Skip this required scraper if it's missing and skipping is allowed
         If ((allowMissingRequirements) And (scraper Is Nothing)) Then
            Continue For
         ElseIf (scraper Is Nothing) Then
            Throw New RequiredAddonNotFound(requiredAddon)
         End If

         ' Retrieve a list of this scrapers functions
         Dim x As List(Of XElement) = scraper.Functions

         ' If the required addon doesn't have any functions we'll skip it
         If (x Is Nothing) Then Continue For

         ' Add each required addon function to our "master" xml tree
         For Each element In x
            mFullTreeWithAddons.LastNode.AddAfterSelf(x)
         Next
      Next
   End Sub
#End Region

#Region "Settings"
   ''' <summary>
   ''' Returns specified setting.
   ''' </summary>
   ''' <param name="id">ID of setting to return.</param>
   Public Function GetSetting(ByVal id As String) As ScraperSetting
      For Each s In mSettings
         If s.ID.ToUpper() = id.ToUpper() Then
            Return s
         End If
      Next

      Return Nothing
   End Function

   ''' <summary>
   ''' Saves settings for this scraper as XML to specified filename.
   ''' </summary>
   ''' <param name="filename">Filename to save settings in.</param>
   ''' <remarks></remarks>
   Public Sub SaveSettings(ByVal filename As String)
      Dim e As XElement = New XElement("settings", New XAttribute("scraper", Me.ID))

      For Each s In Me.Settings
         Dim newElement As XElement = New XElement("setting", New Object() { _
                                          New XAttribute("id", s.ID), _
                                          New XAttribute("type", s.SettingType), _
                                          New XAttribute("value", s.ValueString)})
         e.Add(newElement)
      Next

      e.Save(filename)
   End Sub

   ''' <summary>
   ''' Loads settings for this scraper using XML from specified filename.
   ''' </summary>
   ''' <param name="filename">Filename to load settings from.</param>
   ''' <remarks></remarks>
   Public Sub LoadSettings(ByVal filename As String)
      Dim e As XElement = XElement.Load(filename)

      For Each s In e.<setting>
         Select Case s.@type
            Case "bool"
               If (s.@value = "false") Then
                  CType(Me.GetSetting(s.@id), ScraperSettingBool).Value = False
               Else
                  CType(Me.GetSetting(s.@id), ScraperSettingBool).Value = True
               End If
            Case "labelenum"
               CType(Me.GetSetting(s.@id), ScraperSettingEnum).Value = s.@value
         End Select
      Next
   End Sub
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates an instace of a scraper with the specified addon.xml filename.
   ''' </summary>
   ''' <param name="addonXmlFilename">Filename containg scraper configuration.  (ie. addon.xml)</param>
   Sub New(ByVal addonXmlFilename As String)
      mAddonXmlFilename = addonXmlFilename

      Dim mAddonRootElement As XElement = XElement.Load(addonXmlFilename)

      ' =====================================================================================
      '  Stores a list of properties about this addon
      ' =====================================================================================
      mID = mAddonRootElement.@id
      mName = mAddonRootElement.@name
      mVersion = mAddonRootElement.@version
      mProviderName = mAddonRootElement.Attribute("provider-name").Value

      ' =====================================================================================
      '  Builds a list of required addon imports
      ' =====================================================================================
      mRequiredAddons = New List(Of String)

      Dim requiredImports As IEnumerable(Of XElement) = From e In mAddonRootElement.<requires>.<import> Select e

      For Each e In requiredImports
         mRequiredAddons.Add(e.@addon)
      Next

      ' =====================================================================================
      '  Determines the type of scraper  (this can be multiple types?)
      ' =====================================================================================
      Dim scraperTypes As IEnumerable(Of XElement) = From e In mAddonRootElement.<extension> Where e.@point Like "*xbmc.metadata.scraper*" Select e

      For Each t In scraperTypes
         Select Case t.@point
            Case "xbmc.metadata.scraper.tvshows"
               mIsTVShowScraper = True
            Case "xbmc.metadata.scraper.movies"
               mIsMovieScraper = True
            Case "xbmc.metadata.scraper.library"
               mIsLibraryScraper = True
         End Select

         mLibraryXmlFilename = Path.Combine(Path.GetDirectoryName(addonXmlFilename), t.@library)
      Next

      ' =====================================================================================
      '  Load the actual scraper functions
      ' =====================================================================================
      If (mLibraryXmlFilename <> String.Empty) Then
         mLibraryRootElement = XElement.Load(mLibraryXmlFilename)
      End If

      ' =====================================================================================
      '  Load the scraper settings (if any)
      ' =====================================================================================
      mSettings = New List(Of ScraperSetting)

      For Each s In Directory.GetFiles(Path.GetDirectoryName(mAddonXmlFilename), "settings.xml", SearchOption.AllDirectories)
         Dim settings As XElement = XElement.Load(s)

         For Each e In settings.<setting>
            If (e.@type = "bool") Then
               mSettings.Add(New ScraperSettingBool(e))
            ElseIf (e.@type = "labelenum") Or  (e.@type = "select") Then
               mSettings.Add(New ScraperSettingEnum(e))
            ElseIf (e.@type = "text") Then
                mSettings.Add(New ScraperSettingText(e))
            End If
         Next
      Next

   End Sub
#End Region

End Class
