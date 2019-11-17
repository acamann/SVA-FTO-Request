
Partial Class ManualEntry_Default
    Inherits System.Web.UI.Page

    'Check to make sure that the From date comes before the To date 
    '  (if both are entered, otherwise Required Validator will catch it)
    Public Sub cvDates_Validate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim bdtFrom As UserControls_BindDateTime = CType(fvFtoRequest.FindControl("bdtFrom"), UserControls_BindDateTime)
        Dim bdtTo As UserControls_BindDateTime = CType(fvFtoRequest.FindControl("bdtTo"), UserControls_BindDateTime)

        Dim fromDate As Nullable(Of DateTime) = bdtFrom.SelectedValue
        Dim toDate As Nullable(Of DateTime) = bdtTo.SelectedValue

        If fromDate.HasValue And toDate.HasValue Then
            If DateTime.Compare(fromDate.Value, toDate.Value) > 0 Then
                args.IsValid = False
                Exit Sub
            End If
        End If

        args.IsValid = True
    End Sub

    Protected Sub odsFtoForm_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFtoForm.Inserted
        'Display a success/failure message based on the manual insertion
        Dim requestId As Integer
        If Not Integer.TryParse(e.ReturnValue, requestId) Then
            'If it failed, send error message and show failure screen
            SvaErrorMethods.SubmitBug(Now, ConfigurationManager.AppSettings("ApplicationId"), Session("EmployeeId"), "There was a problem inserting the FTO Request manually.")
            pageFailure.Selected = True
        Else
            pageSuccess.Selected = True
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'If the user doesn't have access to this Manual Entry Page (PartId = 10049), redirect
        If Not SvaSecurityMethods.UserHasAccess(10049) Then
            Response.Redirect("~/ErrorPages/InsufficientAccess.aspx")
        End If
    End Sub

End Class
