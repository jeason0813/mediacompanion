Imports MC_UserControls

Public Class FilteredItems

    Property Include As New List(Of String)
    Property Exclude As New List(Of String)

    Sub New (ccb As TriStateCheckedComboBox,Optional Find As String=Nothing, Optional Replace As String=Nothing)

        Dim i As Integer = 0

        For Each item As CCBoxItem In ccb.Items
            Dim value As String = item.Name.RemoveAfterMatch

            If Not IsNothing(Find) Then
                value = If(value=Find,Replace,value)
            End If

            Select ccb.GetItemCheckState(i)
                Case CheckState.Checked   : Include.Add(value)
                Case CheckState.Unchecked : Exclude.Add(value)
            End Select
            i += 1
        Next
    End Sub

    Sub FindAndReplace(Find As String, Replace As String)

        FindAndReplace(Include,Find,Replace)
        FindAndReplace(Exclude,Find,Replace)

    End Sub


    Sub FindAndReplace(lst As List(Of String), Find As String, Replace As String)

        For i=0 to lst.Count-1
            If lst(i)=Find Then
                lst(i)=Replace
            End If
        Next

    End Sub


    Sub SelectFirstMatch(ByVal cb As Windows.Forms.ComboBox)
        For Each item As String In Include
            If cb.Items.IndexOf(item)>-1 Then 
                cb.SelectedItem = item
            End If
        Next
    End Sub
End Class
