
Partial Class Submit
    Inherits System.Web.UI.Page

#Region " Submit Bug "

    Protected Sub btnReportBug_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportBug.Click
        ' Try to submit bug and display appropriate message
        If SubmitBug() Then
            mvBug.SetActiveView(viewSuccess)
        Else
            mvBug.SetActiveView(viewFailure)
        End If
    End Sub

    Protected Function SubmitBug() As Boolean
        Dim connectionString As String = _
            ConfigurationManager.ConnectionStrings("cnSVAApplicationFramework").ConnectionString
        Dim dateOccurred As Date = Date.Now()
        Dim applicationId As Integer = ConfigurationManager.AppSettings("ApplicationId")
        Dim details As String = txtDescription.Text

        Return SvaErrorMethods.SubmitBug(dateOccurred, _
            applicationId, Session("employeeId"), details)

    End Function

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not ConfigurationManager.AppSettings("ApplicationId") Is Nothing Then
            Dim appBll As New SvaApplicationBLL
            Dim application As SvaApplication.SvaApplicationRow
            application = appBll.GetApplicationById(ConfigurationManager.AppSettings("ApplicationId"))
            If Not application Is Nothing Then
                lblApplication.Text = application.Name
            End If
        End If
    End Sub

End Class
