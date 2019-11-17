Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail

Public Class SvaErrorMethods

    Protected connectionString As String = ConfigurationManager.ConnectionStrings("cnSVAApplicationFramework").ConnectionString

#Region " Public Shared Methods "

    Public Shared Function SubmitBug( _
        ByVal dateOccurred As DateTime, _
        ByVal applicationId As Integer, _
        ByVal employeeId As Integer, _
        ByVal details As String) _
            As Boolean

        Dim svaError As New SvaErrorMethods

        'Email the bug to the admin
        Dim emailSent As Boolean = _
            svaError.EmailError(applicationId, _
                employeeId, "User Submitted Bug", _
                details)

        'Insert the bug into the database
        Dim dbUpdated As Boolean
        If svaError.ShouldLogException(applicationId) Then
            dbUpdated = _
                svaError.InsertError( _
                    dateOccurred, applicationId, _
                    employeeId, "User Submitted Bug", _
                    details, Nothing, Nothing, Nothing, Nothing)
        End If

        Return emailSent And dbUpdated

    End Function

    Public Shared Sub ExceptionThrown(ByVal ex As Exception)
        Dim dateOccurred As DateTime = Now
        Dim applicationId As Integer = ConfigurationManager.AppSettings("ApplicationId")
        Dim employeeId As Integer = Nothing
        If Not HttpContext.Current.Session Is Nothing Then
            employeeId = HttpContext.Current.Session("employeeId")
        End If
        Dim title As String = String.Empty

        If Not IsNothing(ex.Message) Then
            If ex.Message.ToString.Length > 105 Then
                title = ex.Message.ToString.Substring(0, 100)
            Else
                title = ex.Message.ToString
            End If
        End If

        Dim svaError As New SvaErrorMethods
        Dim details As String = svaError.ExceptionDetails(ex)
        svaError.HandleException(dateOccurred, applicationId, employeeId, title, details)

    End Sub

#End Region

#Region " Get Exception Details "

    Private Function ExceptionDetails(ByVal ex As Exception) As String
        Dim details As New StringBuilder
        If Not IsNothing(ex.Message) Then
            details.Append("ERROR MESSAGE: " & ex.Message.ToString & vbCrLf & vbCrLf)
        End If
        If Not IsNothing(ex.Source) Then
            details.Append("SOURCE: " & ex.Source.ToString & vbCrLf & vbCrLf)
        End If
        If Not IsNothing(HttpContext.Current.Request.QueryString) Then
            details.Append("QUERYSTRING: " & HttpContext.Current.Request.QueryString.ToString & vbCrLf & vbCrLf)
        End If
        If Not IsNothing(HttpContext.Current.Request.UrlReferrer) Then
            details.Append("REFERRER: " & HttpContext.Current.Request.UrlReferrer.ToString & vbCrLf & vbCrLf)
        End If
        If Not IsNothing(ex.StackTrace) Then
            details.Append("TARGET SITE: " & ex.StackTrace & vbCrLf & vbCrLf)
        End If
        If Not IsNothing(HttpContext.Current.Request.FilePath) Then
            details.Append("PAGE: " & HttpContext.Current.Request.FilePath.ToString & vbCrLf & vbCrLf)
        End If
        Return details.ToString
    End Function

#End Region

#Region " Handle Exception "

    Private Function HandleException( _
            ByVal dateOccurred As DateTime, _
            ByVal applicationId As Integer, _
            ByVal employeeId As Integer, _
            ByVal title As String, _
            ByVal details As String) _
                As Boolean


        'Email the admin 
        Dim emailSent As Boolean = _
            EmailError(applicationId, _
                employeeId, title, details)

        'Insert the error into the database 
        Dim dbUpdated As Boolean
        If ShouldLogException(applicationId) Then
            dbUpdated = _
               InsertError( _
                    dateOccurred, applicationId, _
                    employeeId, title, details, _
                    Nothing, Nothing, Nothing, Nothing)
        End If

    End Function

#End Region

#Region " Private Data Function "

    Private Function InsertError( _
        ByVal dateOccurred As DateTime, _
        ByVal applicationId As Integer, _
        ByVal employeeId As Integer, _
        ByVal title As String, _
        ByVal details As String, _
        ByVal notes As String, _
        ByVal statusId As Nullable(Of Integer), _
        ByVal priorityId As Nullable(Of Integer), _
        ByVal assignedTo As Nullable(Of Integer)) _
            As Boolean

        ' Insert Error into the Database
        Return SqlHelper.ExecuteNonQuery( _
            connectionString, _
            CommandType.StoredProcedure, "dbo.AddError", _
            New SqlParameter("@DateOccurred", dateOccurred), _
            New SqlParameter("@ApplicationId", applicationId), _
            New SqlParameter("@EmpId", employeeId), _
            New SqlParameter("@Title", title), _
            New SqlParameter("@Details", details), _
            New SqlParameter("@Notes", notes), _
            New SqlParameter("@StatusId", statusId), _
            New SqlParameter("@PriorityId", priorityId), _
            New SqlParameter("@AssignedTo", assignedTo)) _
                = 1

    End Function

    Private Function ShouldLogException(ByVal applicationId As Integer) As Boolean
        'If the application doesn't call for exceptions to be emailed, return false
        Dim appBll As New SvaApplicationBLL
        Dim app As SvaApplication.SvaApplicationRow
        app = appBll.GetApplicationById(applicationId)
        If Not app Is Nothing Then
            Return app.LogExceptionsToDB
        End If
        Return True
    End Function

#End Region

#Region " Private Mail Function "

    Private Function EmailError( _
            ByVal applicationId As Integer, _
            ByVal employeeId As Integer, _
            ByVal subject As String, _
            ByVal details As String) _
                As Boolean

        ' Set up dummy application information by default
        Dim applicationName As String = "Unknown Application"
        Dim adminEmail As String = "webmaster@sva.com"

        ' Try finding Application information
        Dim appBll As New SvaApplicationBLL
        Dim app As SvaApplication.SvaApplicationRow
        app = appBll.GetApplicationById(applicationId)
        If Not app Is Nothing Then
            If Not app.EmailExceptions Then Return False

            applicationName = app.Name
            adminEmail = app.AdminEmail
        End If

        ' Set up dummy employee information by default
        Dim employeeName As String = "Unknown Employee (#" & employeeId & ")"
        Dim employeeEmail As String = "no-reply@sva.com"

        ' Try finding Employee information
        Dim empBll As New SvaEmployeeBLL
        Dim emps As SvaEmployee.SvaEmployeeDataTable = empBll.GetEmployeeById(employeeId)
        If emps.Rows.Count > 0 Then
            Dim emp As SvaEmployee.SvaEmployeeRow = emps.Rows(0)
            employeeName = emp.EmpName
            employeeEmail = emp.EmpEmail
        End If

        Dim emailName As String = applicationName & " Error"
        Dim smtpClient As New SmtpClient
        Dim message As New MailMessage
        Dim mailSender As New MailAddress(employeeEmail, emailName)

        message.To.Add(adminEmail)
        message.IsBodyHtml = False
        message.From = mailSender
        message.Subject = subject
        message.Body = message.Body & "The following bug/exception was submitted by " & employeeName & "." & vbCrLf & vbCrLf
        message.Body = message.Body & details
        smtpClient.Send(message)

        message.Dispose()
        mailSender = Nothing

        Return True
    End Function

#End Region

End Class
