=================================================================================
XScraperLib - .NET library for accessing XBMC-style metadata scrapers.
Copyright (C) 2010  John Klimek
=================================================================================
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
=================================================================================

Changelog:

v0.1.0 - 8/25/2010 - Initial Release

=================================================================================

Quick Start Guide:

There are three main classes in XScraperLib:  ScraperManager, Scraper, and ScraperQuery.

---------------------------------------------------------------------------------
Scraper Class
---------------------------------------------------------------------------------

Scraper contains information about an individual scraper such as imdb.com (identified as
metadata.imdb.com by XBMC), etc.  It will contain a list of scraper functions as well as
settings required by this scraper.  Typically you will access scrapers through the scraper
manager but it is possible to use this class directly.  Also, scrapers will automatically
look for and load any required imports for additional addons.  An exception will be thrown
if any imports cannot be found but this can be toggled off.

Examples:  (VB.NET)

Dim s As Scraper = New Scraper(".\path\to\addon.xml")
Dim x As XElement = s.Func("GetDetails") ' or s("GetDetails")
listBox.DataSource = s.Functions

You can also retrieve and save scraper settings:

Debug.WriteLine(s.GetSetting("akatitles").ValueString)
s.SaveSettings("c:\mysettings.xml")
s.LoadSettings("c:\mysettings.xml")

---------------------------------------------------------------------------------
ScraperManager Class
---------------------------------------------------------------------------------

ScraperManager searches a base directory (and subdirectories) for addon.xml files and builds
a list of scrapers.  This is most commonly used to access scrapers.

Examples:

Dim mScraperManager As ScraperManager = New ScraperManager(".\scrapersDirectory")
Dim s As Scraper = mScraperManager.GetByID("metadata.imdb.com")

---------------------------------------------------------------------------------
ScraperQuery Class
---------------------------------------------------------------------------------

ScraperQuery is the final and possibly most important class.  It requires a scraper, a function
name and optionally parameters.

Examples:

' Using the ScraperQuery static (shared) helper function:
Dim result As String = ScraperQuery.ExecuteFunction(scraperObject, "FunctionName")
Dim result As String = ScraperQuery.ExecuteFunction(scraperObject, "FunctionName", New String() { param1, param2 })

The above example will execute the given function on the scraper object using the passed-in 
parameters. Parameters are optional but if included they will be assigned to variable buffers 
starting at $$1.  This means that index 0 of the parameter array is assigned $$1 and $$0 is 
unused and invalid.  The ExecuteFunction method will return a string result of the processed function.

Complete example using the metadata.imdb.com scraper:

Dim mScraperManager As ScraperManager = New ScraperManager(".\scrapers")
Dim imdbScraper As Scraper = mScraperManager.GetByID("metadata.imdb.com")
Dim result As String = _
  ScraperQuery.ExecuteFunction(imdbScraper, "CreateSearchUrl", New String() { "Alice In Wonderland", "2010" })
  
This will execute the CreateSearchUrl function and assign "Alice In Wonderland" to variable/buffer $$1,
and assign "2010" to variable/buffer $$2.  The destination buffer specified by CreateSearchUrl is 3 so
the return result of ExecuteFunction will be the contents of buffer $$3.

While executing the function the cache manager will be used for any <url> tags that specify a caching
filename and any $INFO[] tags will be replaced with the specified setting from the scraper.  
Also, regular expressions are processed from inner to outer and then top to bottom.  After all RegEx
functions are finished all <chain> and <url> elements are recursively resolved until the entire
function has finished.

---------------------------------------------------------------------------------
Configuration
---------------------------------------------------------------------------------

XScraperLib uses the built-in .NET ConfigurationManager class to manage the couple of settings needed
to operate.

Currently there are two settings:

CacheDirectory and SettingsDirectory.  The CacheDirectory is the directory to store <url> cache items 
and the SettingsDirectory is the directory to store scraper settings.

Both of these are configured in the XScraperLib.dll.config file.  They can also be overridden in code
if needed.

---------------------------------------------------------------------------------

All classes, methods, and properties include XML documentation (for Visual Studio Intellisense)
so if you need any help please refer to that or feel free to e-mail me at jklimek@gmail.com.
