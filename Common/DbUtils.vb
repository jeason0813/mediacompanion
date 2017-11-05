Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SQLite

Public Class DbUtils


    Public Shared Function ExecuteReader(cmd As SQLiteCommand) As DataTable

        Dim dt As DataTable = New DataTable

        Using reader As SQLiteDataReader = cmd.ExecuteReader
			dt.Load(reader)
        End Using

        Return dt
    End Function


    Public Shared Function ExecuteReader(conn As SQLiteConnection, query As string) As DataTable

        Dim dt As DataTable = New DataTable

        Using cmd As SQLiteCommand = new SQLiteCommand(conn)

            cmd.CommandText = query

            Using reader As SQLiteDataReader = cmd.ExecuteReader
			    dt.Load(reader)
            End Using
        End Using

        Return dt
    End Function
    
	 Public Shared Sub ExecuteNonQuery(conn As SQLiteConnection, query As string)

        Using cmd As SQLiteCommand = new SQLiteCommand(query,conn)
			cmd.ExecuteNonQuery
        End Using
    End Sub


	Public Shared Function Stuff(s As string) As string
			
	'	s = s.Replace( "'"  , "''"   )
	'	s = s.Replace( "["  , "[[]"  )
	'	s = s.Replace( "^"  , "[^]"  )

		return s.Replace("'","''")
	End Function

End Class
