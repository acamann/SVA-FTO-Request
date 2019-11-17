
<System.ComponentModel.DefaultBindingProperty("SelectedValue"), ValidationProperty("SelectedValue")> _
Partial Class UserControls_ApproveRadios
    Inherits System.Web.UI.UserControl

    <ComponentModel.Bindable(True)> _
    Public ReadOnly Property SelectedValue() As Nullable(Of Boolean)
        Get
            If rbApproved.Checked Then
                Return True
            ElseIf rbDenied.Checked Then
                Return False
            Else
                Return Nothing
            End If
        End Get
    End Property

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

    End Sub

End Class
