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

Imports System.IO
Imports System.Reflection

Public Class ScraperManager

#Region "Fields"
   Private mScrapers As List(Of Scraper)

   Private mBaseDirectory As String
   Private mScraperSettingsDirectory As String

    Private mAllowMissingRequirements As Boolean = True
   Private mLazyLoadRequirements As Boolean = False
#End Region

#Region "Properties"
   ''' <summary>
   ''' List of known scrapers loaded in this manager.
   ''' </summary>
   Public ReadOnly Property Scrapers() As List(Of Scraper)
      Get
         Return mScrapers
      End Get
   End Property

   ''' <summary>
   ''' Scraper located as specified index in array.
   ''' </summary>
   ''' <param name="i">Index of scraper to return.</param>
   Default Public ReadOnly Property Scraper(ByVal i As Integer) As Scraper
      Get
         Return mScrapers(i)
      End Get
   End Property

   ''' <summary>
   ''' Prevents an exception from being thrown if a required addon is missing during scraper loading.
   ''' </summary>
   Public Property AllowMissingRequirements() As Boolean
      Get
         Return mAllowMissingRequirements
      End Get
      Set(ByVal value As Boolean)
         mAllowMissingRequirements = value
      End Set
   End Property

   ''' <summary>
   ''' Delays building of full XML tree for each addon until addon is used for the first time.
   ''' </summary>
   Public Property LazyLoadRequirements() As Boolean
      Get
         Return mLazyLoadRequirements
      End Get
      Set(ByVal value As Boolean)
         mLazyLoadRequirements = value
      End Set
   End Property

   ''' <summary>
   ''' Directory to save/load scraper settings from.
   ''' </summary>
   Public Property ScraperSettingsDirectory() As String
      Get
         Return mScraperSettingsDirectory
      End Get
      Set(ByVal value As String)
         mScraperSettingsDirectory = value
      End Set
   End Property
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Creates an empty scraper manager.
   ''' </summary>
   ''' <remarks></remarks>
   Sub New()
      mScrapers = New List(Of Scraper)
      mScraperSettingsDirectory = Path.Combine(My.Application.Info.DirectoryPath, "settings")

      If (Not Directory.Exists(mScraperSettingsDirectory)) Then
         Directory.CreateDirectory(mScraperSettingsDirectory)
      End If
   End Sub

   ''' <summary>
   ''' Creates a scraper manager and loads scraper from specified base directory.
   ''' </summary>
   ''' <param name="baseDirectory">Base (parent) directory of scrapers.</param>
   ''' <remarks></remarks>
   Sub New(ByVal baseDirectory As String)
      Me.New()

      LoadScrapers(baseDirectory)
   End Sub
#End Region

#Region "Methods"
   ''' <summary>
   ''' Loads all scrapers by searching for addon.xml inside provided base directory.
   ''' </summary>
   ''' <param name="baseDirectory">Parent (base) directory for addons.</param>
   Public Sub LoadScrapers(ByVal baseDirectory As String)
      mBaseDirectory = baseDirectory

      mScrapers.Clear()

      ' =====================================================================================
      '  Search and construct/build all scrapers by searching for addon.xml
      ' =====================================================================================
      For Each s In Directory.GetFiles(mBaseDirectory, "addon.xml", SearchOption.AllDirectories)
         mScrapers.Add(New Scraper(s))
      Next

      ' =====================================================================================
      '  Load configuration settings for each scraper if setting exist
      ' =====================================================================================
      For Each sFile In Directory.GetFiles(My.Settings.SettingsDirectory, "*.xml", SearchOption.TopDirectoryOnly)
         Dim s As Scraper = Me.GetByID(Path.GetFileNameWithoutExtension(sFile))

         If (s IsNot Nothing) Then
            s.LoadSettings(sFile)
         End If
      Next

      ' =====================================================================================
      '  After all scraper objects are built we can now fullfill requirements for each addon
      ' =====================================================================================
      'If (Not mLazyLoadRequirements) Then
      For Each s In mScrapers
         s.BuildXmlWithRequiredAddons(Me, mAllowMissingRequirements)
      Next
      'End If
   End Sub

   ''' <summary>
   ''' Retrieves a scraper object using it's ID
   ''' </summary>
   ''' <param name="scraperID">ID of the scraper</param>
   ''' <returns>Scraper object</returns>
   Public Function GetByID(ByVal scraperID As String) As Scraper
      For Each s In mScrapers
         If (s.ID.ToUpper() = scraperID.ToUpper) Then
            Return s
         End If
      Next

      Return Nothing
   End Function
#End Region

End Class
