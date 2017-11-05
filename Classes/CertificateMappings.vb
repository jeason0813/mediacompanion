Imports System.ComponentModel
Imports System.IO
'Imports Alphaleonis.Win32.Filesystem
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports Media_Companion
Imports System.Text
'Imports YouTubeFisher

Public Class CertificateMapping
    Public Property Find    As String 
    Public Property Replace As String 

   
    Sub New
    End Sub

    Sub New(find As String, replace As String)
        Me.Find    = find
        Me.Replace = replace
    End Sub
End Class

Public Class CertificateMappings

    Public UserMappingsFile    = Pref.applicationPath & "\classes\UserCertificateMappings.xml"
    Public DefaultMappingsFile = Pref.applicationPath & "\classes\DefaultCertificateMappings.xml"

    Public Property List As New List(Of CertificateMapping)

    Public ReadOnly Property XDoc As XDocument
        Get
            'Allow Users to use their own Certificate Mappings:
            If File.Exists(UserMappingsFile) Then
                Try
                    Return XDocument.Load(UserMappingsFile)
                Catch ex As Exception
                    ExceptionHandler.LogError(ex,"error in UserCertificateMappings.xml, falling back to DefaultCertificateMappings.xml")
                End Try
            End If

            Return XDocument.Load(DefaultMappingsFile)
        End Get 
    End Property


    Sub New
        List.Clear
        
        Dim q = From x In XDoc.Descendants("Certificate")
                            Select 
                                Find    = x.Attribute("find"   ).Value,
                                Replace = x.Attribute("replace").Value
                            Order By 
                                Find
                    
        For Each item In q
            List.Add( New CertificateMapping(item.find,item.replace) )
        Next
    End Sub


    Function GetMapping(certificate As String) As String
        
        Dim q = From x In List Select x Where x.find.ToUpper = certificate.ToUpper

        If q.Count=1 Then Return q(0).Replace


        q = From x In List Select x Where x.Find.GetLastChar="*" and certificate.ToUpper.IndexOf(x.Find.ToUpper.RemoveLastChar)=0

        If q.Count=1 Then Return q(0).Replace

        Return certificate
    End Function


End Class
