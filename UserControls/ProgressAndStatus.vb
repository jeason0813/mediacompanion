Public Class ProgressAndStatus
    Public cancel As Boolean


    Public Sub Display()
        Me.Height = 185
        Me.Width = 532
        Me.Left = (Parent.Width / 2) - (Me.Width / 2)
        Me.Top = (Parent.Height / 2) - (Me.Height / 2)
        ProgressBar1.Value = 0
        cancel = False
        Me.Visible = True
    End Sub

    Public Sub Counter(ByVal value As Integer, ByVal Maximum As Integer)
        LabelCounter.Text = value.ToString & "/" & Maximum.ToString
    End Sub

    Public Sub Status(ByVal value As String)
        TextBoxStatus.Text = value
    End Sub

    Public Sub ReportProgress(ByVal progress As Integer, ByVal progresstext As String)
        ProgressBar1.Value = progress
        TextBoxProgresstext.Text = progresstext
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        cancel = True
    End Sub
End Class
