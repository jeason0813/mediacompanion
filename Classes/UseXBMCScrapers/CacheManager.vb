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

'Imports System.IO
Imports Alphaleonis.Win32.Filesystem
Imports System.Net
Imports System.Linq

Public Class CacheManager

#Region "Fields"
   Private mCacheDirectory As String
   Private mCachedFiles As List(Of String)
   Private mFilesLoaded As Dictionary(Of String, String)
   Private mCacheEnabled As Boolean = True
   Private mMemoryCacheEnabled As Boolean = True
#End Region

#Region "Properties"
   ''' <summary>
   ''' Directory to use for caching.
   ''' </summary>
   ''' <value>Name of directory to use for caching.</value>
   ''' <returns>Name of directory to use for caching.</returns>
   Public Property CacheDirectory() As String
      Get
         Return mCacheDirectory
      End Get
      Set(ByVal value As String)
         mCacheDirectory = value
         RefreshDiskCache()
         mFilesLoaded.Clear()
      End Set
   End Property

   ''' <summary>
   ''' Enables or disabled disk caching.
   ''' </summary>
   Public Property CacheEnabled() As Boolean
      Get
         Return mCacheEnabled
      End Get
      Set(ByVal value As Boolean)
         mCacheEnabled = value
      End Set
   End Property

   ''' <summary>
   ''' Enables or disabled memory caching.
   ''' </summary>
   ''' <remarks></remarks>
   Public Property MemoryCacheEnabled() As Boolean
      Get
         Return mMemoryCacheEnabled
      End Get
      Set(ByVal value As Boolean)
         mMemoryCacheEnabled = value
      End Set
   End Property
#End Region

#Region "Public Methods"
   ''' <summary>
   ''' Reloads filenames from cache directory.
   ''' </summary>
   ''' <remarks></remarks>
   Public Sub RefreshDiskCache()
      mCachedFiles.Clear()

      For Each s In Directory.GetFiles(mCacheDirectory, "*.*", IO.SearchOption.AllDirectories)
         mCachedFiles.Add(Path.GetFileName(s))
      Next
   End Sub

   ''' <summary>
   ''' Clears all files cached in memory.
   ''' </summary>
   Public Sub FlushMemoryCache()
      mFilesLoaded.Clear()
   End Sub

   ''' <summary>
   ''' Retrieves a url and caches it as the given filename (if caching is enabled).
   ''' </summary>
   ''' <param name="url">URL to retrieve.</param>
   ''' <param name="filename">Filename to save for caching.</param>
   ''' <returns>Contents of URL (or cached file).</returns>
   ''' <remarks></remarks>
   Public Function GetUrl(ByVal url As String, ByVal filename As String) As String
      Return GetUrl(url, filename, mCacheEnabled)
   End Function

   ''' <summary>
   ''' Retrieves a url and caches it as the given filename (if caching is enabled).
   ''' </summary>
   ''' <param name="url">URL to retrieve.</param>
   ''' <param name="filename">Filename to save for caching.</param>
   ''' <param name="saveToCache">Enables or disables saving to cache.</param>
   ''' <returns>Contents of URL (or cached file).</returns>
   ''' <remarks>If CacheEnabled is false, saveToCache is ignored.</remarks>
   Public Function GetUrl(ByVal url As String, ByVal filename As String, ByVal saveToCache As Boolean) As String
      Dim fullFilename As String = Path.Combine(mCacheDirectory, filename)

      ' Check to see if this file exists in the cache
      If (Not mCachedFiles.Contains(filename)) Then
         ' The file doesn't exist in the cache so we will download it
         Trace.WriteLine("Cache manager (" + filename + "), downloading: " + url)
            
            Dim contents As String
            Try
                Dim TMDBRequest As HttpWebRequest = WebRequest.Create(url)
                TMDBRequest.Proxy = Utilities.MyProxy 
                TMDBRequest.Accept = "application/json"
                TMDBRequest.ContentType = "application/json"
                TMDBRequest.Credentials = CredentialCache.DefaultCredentials
                TMDBRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2"
                Dim TMDBResponse As HttpWebResponse = CType(TMDBRequest.GetResponse(), HttpWebResponse)
                Dim dataStream As IO.Stream = TMDBResponse.GetResponseStream()
                Dim reader As New IO.StreamReader(dataStream)
                contents = reader.ReadToEnd()
                reader.Close()
                dataStream.Close()
                TMDBResponse.Close()
            Catch ex As Exception
                'MessageBox.Show("ERROR:  " + ex.Message + vbCrLf + vbCrLf + "URL: " + url, "Error retrieving URL", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return "ERROR" 'SK: added 
            End Try

         ' We will also save the file to the cache if requested
         If ((saveToCache) And (mCacheEnabled)) Then
            If (File.Exists(fullFilename)) Then
               File.Delete(fullFilename)
            End If

            Using sw As IO.StreamWriter = New IO.StreamWriter(fullFilename)
               sw.Write(contents)
            End Using

            mCachedFiles.Add(filename)
         End If

         ' Also save the file to loaded memory
         mFilesLoaded(filename) = contents
      ElseIf (Not mFilesLoaded.Keys.Contains(filename)) Then
         Trace.WriteLine("Cache manager (" + filename + "), reading from disk.")

         Using sr As New IO.StreamReader(fullFilename)
            mFilesLoaded(filename) = sr.ReadToEnd()
         End Using

      Else
         Trace.WriteLine("Cache manager (" + filename + "), reading from memory.")
      End If

      Return mFilesLoaded(filename)
   End Function
#End Region

#Region "Constructors"
   ''' <summary>
   ''' Constructs a new cache manager using the given cache directory.
   ''' </summary>
   ''' <param name="cacheDirectory">Directory to store cache files.</param>
   ''' <remarks></remarks>
   Sub New(ByVal cacheDirectory As String)
      mCacheDirectory = cacheDirectory

      ' Create the cache directory if it doesn't exist
      If (Not Directory.Exists(mCacheDirectory)) Then
         Directory.CreateDirectory(mCacheDirectory)
      End If

      ' Create list and dictionary objects
      mCachedFiles = New List(Of String)
      mFilesLoaded = New Dictionary(Of String, String)

      ' Reloads the list of files currently cached on disk
      RefreshDiskCache()
   End Sub
#End Region

#Region "Singleton Cache Manager"
   Private Shared mInstance As CacheManager

   ''' <summary>
   ''' Returns singleton instance of caching manager.
   ''' </summary>
   ''' <value></value>
   ''' <returns></returns>
   ''' <remarks></remarks>
   Public Shared ReadOnly Property Instance() As CacheManager
      Get
         If (mInstance Is Nothing) Then
            mInstance = New CacheManager(My.Settings.CacheDirectory)
         End If

         Return mInstance
      End Get
   End Property
#End Region

End Class
