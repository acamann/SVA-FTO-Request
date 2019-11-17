
<System.ComponentModel.DefaultBindingProperty("SelectedValue")> _
Partial Class UserControls_FtoBalance
    Inherits System.Web.UI.UserControl

    <ComponentModel.Bindable(True)> _
    Public ReadOnly Property SelectedValue() As Nullable(Of Double)
        Get
            If ViewState("balance") Is Nothing Then
                Return Nothing
            Else
                Return CType(ViewState("balance"), Double)
            End If
        End Get
    End Property

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding


        Dim ftoRequestBll As New FtoRequestBLL
        Dim ftoBalance As Double = ftoRequestBll.GetFtoBalance(Session("employeeId"))

        lblFtoBalance.Text = Format(ftoBalance, "f2")

        ViewState("balance") = ftoBalance

    End Sub

End Class
