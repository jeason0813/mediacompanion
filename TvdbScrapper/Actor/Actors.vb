Imports ProtoXML

Namespace Tvdb
    Public Class Actors
        Inherits ProtoFile

        Public Sub New()
            MyBase.New("Actors")
        End Sub

        Public Property Items As New ActorList(Me, "Actor")
    End Class
End Namespace
