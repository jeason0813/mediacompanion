Option Explicit On

Imports System.Xml
Imports Media_Companion
Imports Media_Companion.Preferences

Public Class mov_CacheSave
    Public Sub ex(ByVal fullMovieList As List(Of ComboList))   'save memory data to cache
        Dim fullpath As String = workingProfile.moviecache
        Dim doc As New XmlDocument
        Dim thispref As XmlNode = Nothing
        Dim xmlproc As XmlDeclaration
        Dim root As XmlElement
        Dim child As XmlElement
        Dim childchild As XmlElement
        Dim count2 As Integer = 0

        If IO.File.Exists(fullpath) Then
            Dim don As Boolean = False
            Dim count As Integer = 0
            Do
                Try
                    If IO.File.Exists(fullpath) Then
                        IO.File.Delete(fullpath)
                        don = True
                    Else
                        don = True
                    End If
                Catch ex As Exception
#If SilentErrorScream Then
                    Throw ex
#End If
                Finally
                    count += 1
                End Try
            Loop Until don = True
        End If

        Form1.ProgressAndStatus1.Display()
        Form1.ProgressAndStatus1.ReportProgress(0, "Creating Movie Cache xml.....")

        xmlproc = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes")
        doc.AppendChild(xmlproc)

        root = doc.CreateElement("movie_cache")

        Form1.ProgressAndStatus1.ReportProgress(0, "Creating cache xml....")
        For Each movie In fullMovieList

            child = doc.CreateElement("movie")
            childchild = doc.CreateElement("filedate")
            childchild.InnerText = movie.filedate
            child.AppendChild(childchild)
            childchild = doc.CreateElement("createdate")
            childchild.InnerText = movie.createdate
            child.AppendChild(childchild)
            childchild = doc.CreateElement("missingdata1")
            childchild.InnerText = movie.missingdata1.ToString
            child.AppendChild(childchild)
            childchild = doc.CreateElement("filename")
            childchild.InnerText = movie.filename
            child.AppendChild(childchild)
            childchild = doc.CreateElement("foldername")
            childchild.InnerText = movie.foldername
            child.AppendChild(childchild)
            childchild = doc.CreateElement("fullpathandfilename")
            childchild.InnerText = movie.fullpathandfilename
            child.AppendChild(childchild)
            If movie.source <> Nothing And movie.source <> "" Then
                childchild = doc.CreateElement("source")
                childchild.InnerText = movie.source
                child.AppendChild(childchild)
            Else
                childchild = doc.CreateElement("source")
                childchild.InnerText = ""
                child.AppendChild(childchild)
            End If
            If movie.movieset <> Nothing Then
                If movie.movieset <> "" Or movie.movieset <> "-None-" Then
                    childchild = doc.CreateElement("set")
                    childchild.InnerText = movie.movieset
                    child.AppendChild(childchild)
                Else
                    childchild = doc.CreateElement("set")
                    childchild.InnerText = ""
                    child.AppendChild(childchild)
                End If
            Else
                childchild = doc.CreateElement("set")
                childchild.InnerText = ""
                child.AppendChild(childchild)
            End If
            childchild = doc.CreateElement("genre")
            childchild.InnerText = movie.genre
            child.AppendChild(childchild)
            childchild = doc.CreateElement("id")
            childchild.InnerText = movie.id
            child.AppendChild(childchild)
            childchild = doc.CreateElement("playcount")
            childchild.InnerText = movie.playcount
            child.AppendChild(childchild)
            childchild = doc.CreateElement("rating")
            childchild.InnerText = movie.rating
            child.AppendChild(childchild)
            childchild = doc.CreateElement("title")
            childchild.InnerText = movie.title
            child.AppendChild(childchild)
            childchild = doc.CreateElement("originaltitle")
            childchild.InnerText = movie.originaltitle
            child.AppendChild(childchild)
            If movie.sortorder = Nothing Then
                movie.sortorder = movie.title
            End If
            If movie.sortorder = "" Then
                movie.sortorder = movie.title
            End If
            childchild = doc.CreateElement("outline")
            childchild.InnerText = movie.outline
            child.AppendChild(childchild)
            childchild = doc.CreateElement("plot")
            If movie.plot.Length() > 100 Then
                childchild.InnerText = movie.plot.Substring(0, 100)     'Only write first 100 chars to cache- this plot is only used for table view - normal full plot comes from the nfo file (fullbody)
            Else
                childchild.InnerText = movie.plot
            End If

            child.AppendChild(childchild)
            childchild = doc.CreateElement("sortorder")
            childchild.InnerText = movie.sortorder
            child.AppendChild(childchild)
            childchild = doc.CreateElement("titleandyear")

            If movie.titleandyear.Length >= 5 Then
                If movie.titleandyear.ToLower.IndexOf(", the") = movie.titleandyear.Length - 5 Then
                    Dim Temp As String = movie.titleandyear.Replace(", the", String.Empty)
                    movie.titleandyear = "The " & Temp
                End If
            End If

            childchild.InnerText = movie.titleandyear
            child.AppendChild(childchild)
            childchild = doc.CreateElement("runtime")
            childchild.InnerText = movie.runtime
            child.AppendChild(childchild)
            childchild = doc.CreateElement("top250")
            childchild.InnerText = movie.top250
            child.AppendChild(childchild)
            childchild = doc.CreateElement("year")
            childchild.InnerText = movie.year
            child.AppendChild(childchild)

            If movie.votes <> Nothing And movie.votes <> "" Then
                childchild = doc.CreateElement("votes")
                childchild.InnerText = movie.votes
                child.AppendChild(childchild)
            Else
                childchild = doc.CreateElement("votes")
                childchild.InnerText = ""
                child.AppendChild(childchild)
            End If

            root.AppendChild(child)
        Next

        doc.AppendChild(root)
        For f = 1 To 100
            Dim output As New XmlTextWriter(fullpath, System.Text.Encoding.UTF8)
            Form1.ProgressAndStatus1.ReportProgress(f, "Saving cache xml....")
            output.Formatting = Formatting.Indented
            doc.WriteTo(output)
            output.Close()
            Form1.ProgressAndStatus1.Visible = False
            Return
        Next
        Form1.ProgressAndStatus1.Visible = False

    End Sub
End Class
