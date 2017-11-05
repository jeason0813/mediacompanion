
Public Class Times
    Property StartTm  As DateTime
    Property EndTm    As DateTime
    ReadOnly Property ElapsedTimeMs As Integer
        Get
            Return Convert.ToInt32( (EndTm - StartTm).TotalMilliseconds )
        End Get
    End Property
End Class