Imports ProtoXML

Namespace Tvdb
    Public Class Banners
        Inherits ProtoFile

        Public Sub New()
            MyBase.New("Banners")
        End Sub

        Public Property Items As New BannerList(Me, "Banner")


    End Class
End Namespace
