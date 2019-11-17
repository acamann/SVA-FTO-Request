
Public Enum ToOrFrom
    ToDate
    FromDate
End Enum

<System.ComponentModel.DefaultBindingProperty("SelectedValue"), _
        ValidationProperty("SelectedValue")> _
Partial Class UserControls_BindDateTime
    Inherits System.Web.UI.UserControl

    <ComponentModel.Bindable(True)> _
    Public ReadOnly Property SelectedValue() As Nullable(Of DateTime)
        Get
            Return GetSelected()
        End Get
    End Property

    Private _defaultDateTime As ToOrFrom
    Public Property DefaultMode() As ToOrFrom
        Get
            Return _defaultDateTime
        End Get
        Set(ByVal value As ToOrFrom)
            _defaultDateTime = value
        End Set
    End Property

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding
        SetDefault()
    End Sub

    Private Sub SetDefault()
        If DefaultMode = ToOrFrom.FromDate Then
            ddlHour.Items.FindByValue("8").Selected = True
            ddlMin.Items.FindByValue("00").Selected = True
            ddlAMPM.Items.FindByValue("AM").Selected = True
        ElseIf DefaultMode = ToOrFrom.ToDate Then
            ddlHour.Items.FindByValue("5").Selected = True
            ddlMin.Items.FindByValue("00").Selected = True
            ddlAMPM.Items.FindByValue("PM").Selected = True
        End If
    End Sub

    Private Function GetSelected() As Nullable(Of DateTime)
        If Not rdpDate.SelectedDate.HasValue Then
            Return Nothing
        Else
            Dim hour As String = ddlHour.SelectedValue
            Dim minute As String = ddlMin.SelectedValue
            Dim amPm As String = ddlAMPM.SelectedValue

            Dim myDate As String = rdpDate.SelectedDate.Value.ToShortDateString

            Dim wholeDateTime As DateTime
            If Not DateTime.TryParse(myDate & " " & hour & ":" & minute & " " & amPm, wholeDateTime) Then
                Return Nothing
            Else
                Return wholeDateTime
            End If
        End If
    End Function

End Class
