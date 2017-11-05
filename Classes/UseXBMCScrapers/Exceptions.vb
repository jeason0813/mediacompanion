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

Public Class RequiredAddonNotFound
   Inherits System.Exception

   Private mScraperID As String

   Public ReadOnly Property ScraperID() As String
      Get
         Return mScraperID
      End Get
   End Property

   Sub New(ByVal scraperID As String)
      MyBase.New("Required addon not found: " + scraperID)

      mScraperID = scraperID
   End Sub
End Class

Public Class FunctionNotFound
   Inherits System.Exception

   Private mFunctionName As String

   Public ReadOnly Property FunctionName() As String
      Get
         Return mFunctionName
      End Get
   End Property

   Sub New(ByVal functionName As String)
      MyBase.New("Function not found: " + functionName)

      mFunctionName = functionName
   End Sub
End Class

Public Class FullXmlTreeNotBuilt
   Inherits System.Exception

   Sub New()
      MyBase.New("Full XML tree is not built.  You must call BuildXmlWithRequiredAddons().")
   End Sub
End Class

Public Class OuterChainElementIncorrect
   Inherits System.Exception

   Private mOuterChainElement As String

   Public ReadOnly Property OuterChainElement() As String
      Get
         Return mOuterChainElement
      End Get
   End Property

   Sub New(ByVal outerChainElement As String)
      MyBase.New("Chain functions are expected to return a <details> tag but the outer tag was: " + outerChainElement)

      mOuterChainElement = outerChainElement
   End Sub
End Class

Public Class InvalidScraperSettingValue
   Inherits System.Exception

   Private mScraperSetting As ScraperSettingEnum
   Private mScraperSettingValue As String

   Public ReadOnly Property ScraperSettingValue() As String
      Get
         Return mScraperSettingValue
      End Get
   End Property

   Public ReadOnly Property ScraperSetting() As ScraperSettingEnum
      Get
         Return mScraperSetting
      End Get
   End Property

   Sub New(ByVal scraperSetting As ScraperSettingEnum, ByVal scraperSettingValue As String)
      MyBase.New("Value (" + scraperSettingValue + ") is not valid for scraper setting: " + scraperSetting.ID)

      mScraperSetting = scraperSetting
      mScraperSettingValue = scraperSettingValue
   End Sub
End Class
