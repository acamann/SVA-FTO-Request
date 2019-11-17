
<System.ComponentModel.DefaultBindingProperty("SelectedValue")> _
Partial Class UserControls_Approver
    Inherits System.Web.UI.UserControl

    <ComponentModel.Bindable(True)> _
    Public ReadOnly Property SelectedValue() As Nullable(Of Integer)
        Get
            If ViewState("approverId") Is Nothing Then
                Return Nothing
            Else
                Return CType(ViewState("approverId"), Integer)
            End If
        End Get
    End Property

    Protected Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

        Dim ftoRequestBll As New FtoRequestBLL
        Dim approverId As Integer = ftoRequestBll.GetApproverId(Session("employeeId"))

        lblEmployeeName.Text = "Unknown (#" & approverId & ")"

        Dim svaEmployeeBll As New SvaEmployeeBLL
        Dim employee As SvaEmployee.SvaEmployeeDataTable = svaEmployeeBll.GetEmployeeById(approverId)

        If employee.Rows.Count > 0 Then
            lblEmployeeName.Text = employee(0).EmpName
        End If

        ViewState("approverId") = approverId

    End Sub

End Class
