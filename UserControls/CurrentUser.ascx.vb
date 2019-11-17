
<System.ComponentModel.DefaultBindingProperty("SelectedValue")> _
Partial Class UserControls_CurrentUser
    Inherits System.Web.UI.UserControl

    <ComponentModel.Bindable(True)> _
    Public ReadOnly Property SelectedValue() As Nullable(Of Integer)
        Get
            If ViewState("emp") Is Nothing Then
                Return Nothing
            Else
                Return CType(ViewState("emp"), Integer)
            End If
        End Get
    End Property

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding
        lblEmployeeName.Text = Session("name")
        ViewState("emp") = Session("employeeId")
    End Sub

End Class
