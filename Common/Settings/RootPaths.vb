
Imports Media_Companion.Pref

Public Class str_RootPaths
	Property rpath As String
	Property selected As Boolean
	Property IncludeInSearchForNew As Boolean

	ReadOnly Property ConfigStr As String
        Get
            Return rpath & "|" & selected & "|" & IncludeInSearchForNew
        End Get
	End Property

	Public Sub New
		rpath = ""
		selected = True
		IncludeInSearchForNew = True
	End Sub

	Public Sub New(str As String) 
		Me.New
		Load(str)
	End Sub

	Public Sub Load(str As String)

		Dim t() As String = decxmlchars(str).Split("|")

		If t.Count > 0 Then rpath = t(0)
		If t.Count > 1 Then selected = t(1)
		If t.Count > 2 Then IncludeInSearchForNew = t(2)
	End Sub

End Class