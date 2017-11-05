Imports Alphaleonis.Win32.Filesystem

Public Class clsCompareFileInfo
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim File1 As FileInfo
        Dim File2 As FileInfo

        File1 = DirectCast(x, FileInfo)
        File2 = DirectCast(y, FileInfo)

        Compare = String.Compare(File1.FullName, File2.FullName)
    End Function
End Class
