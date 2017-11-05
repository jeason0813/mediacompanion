Imports Microsoft.Win32
Module Module1

    Sub Main()
		Try
			Dim logName As String = "MediaCompanion"
			Dim sourceName As String = "MediaCompanion"

			If String.IsNullOrEmpty(sourceName) Then
				Console.WriteLine("{0} Source [Log]", AppDomain.CurrentDomain.FriendlyName)
			ElseIf EventLog.SourceExists(sourceName) Then
				logName = EventLog.LogNameFromSourceName(sourceName, ".")
				Console.WriteLine("The source '{0}' already exists in log '{1}'.", sourceName, logName)
			ElseIf Not String.IsNullOrEmpty(logName) AndAlso EventLog.Exists(logName) Then
				Console.WriteLine("The log '{0}' is not valid.", logName)
			Else
				Console.WriteLine("Creating '{0}' on '{1}'", sourceName, logName)
				EventLog.CreateEventSource(sourceName, logName)
			End If
            Console.WriteLine(vbcrlf & "Press any key to close")
            Console.ReadKey(True)
		Catch ex As Exception
			Console.WriteLine(ex)
            
            
		End Try
    End Sub

End Module
