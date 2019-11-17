Imports Microsoft.VisualBasic
Imports System.Net.Mail
'Imports Microsoft.Office.Interop.Outlook

Public Class FtoHelper

#Region " Email Request "

    Public Sub EmailRequest(ByVal requestId As Integer)
        Dim request As ftoRequest.FtoRequestRow = GetRequest(requestId)

        Dim mailClient As SmtpClient = New SmtpClient(ConfigurationManager.AppSettings("MailServer"))

        Dim requestorEmail As String = GetEmail(request.RequestorId)
        Dim approverEmail As String = GetEmail(request.ApproverId)

        ' Email Approver
        Dim emailApprover As New MailMessage("info@sva.com", approverEmail)
        emailApprover.Subject = "FTO Request - " & request.Requestor
        emailApprover.IsBodyHtml = True
        emailApprover.Body = "<h3>New FTO Request from <span style='color:#316AC5; font-weight:bold;'>" & request.Requestor & "</span></h3>" & _
                "<div>From <span style='color:#316AC5; font-weight:bold;'>" & request.FromDate & "</span> to <span style='color:#316AC5; font-weight:bold;'>" & request.ToDate & "</span></div>" & _
                "<br /><div>Please <a href='http://apps.svamain.loc/Fto2008/Default.aspx?View=Request&RequestId=" & requestId & "'>click here to respond</a> to the request.</div>"

        mailClient.Send(emailApprover)


        ' Get safe request value
        Dim requestNotes As String = String.Empty
        If Not request.IsRequestNotesNull Then requestNotes = request.RequestNotes

        ' Email Confirmation to Requestor
        Dim emailConfirmation As New MailMessage("info@sva.com", requestorEmail)
        emailConfirmation.Subject = "Your FTO Request has been sent"
        emailConfirmation.IsBodyHtml = True
        emailConfirmation.Body = "<div style='color:gray'>For your records:</div>" & _
                "<h3>The following FTO Request has been sent to <span style='color:#316AC5; font-weight:bold;'>" & request.Approver & "</span></h3>" & _
                "<div>From <span style='color:#316AC5; font-weight:bold;'>" & request.FromDate & "</span> to <span style='color:#316AC5; font-weight:bold;'>" & request.ToDate & "</span></div>" & _
                "<br /><table border='0'><tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Date of Request:</td><td style='padding:3px;'>" & request.RequestDate & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Requestor:</td><td style='padding:3px;'>" & request.Requestor & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Total Hours:</td><td style='padding:3px;'>" & Format(request.RequestHours, "f2") & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Type of Request:</td><td style='padding:3px;'>" & request.RequestType & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Request Notes:</td><td style='padding:3px;'>" & requestNotes & "</td></tr></table>" & _
                "<div style='color:gray'><br />Note: This message is not an approval of your requested time off. " & _
                "You will receive an e-mail notification when the request is approved or denied.</div>"

        mailClient.Send(emailConfirmation)

    End Sub

#End Region

#Region " Email Response "

    Public Sub EmailResponse(ByVal requestId As Integer)
        Dim request As ftoRequest.FtoRequestRow = GetRequest(requestId)

        Dim mailClient As SmtpClient = New SmtpClient(ConfigurationManager.AppSettings("MailServer"))

        Dim requestorEmail As String = GetEmail(request.RequestorId)
        Dim approverEmail As String = GetEmail(request.ApproverId)

        ' Get safe request/response values
        Dim requestNotes As String = String.Empty
        If Not request.IsRequestNotesNull Then requestNotes = request.RequestNotes
        Dim responseNotes As String = String.Empty
        If Not request.IsResponseNotesNull Then responseNotes = request.ResponseNotes

        'Get safe response values
        Dim response As String = String.Empty
        Dim frontDeskNotice As String = String.Empty
        Dim responseColor As String = String.Empty
        If request.IsApproved Then
            response = "Approved"
            responseColor = "Green"
            frontDeskNotice = "<br /><div style='color:#316AC5;'>" & _
                "A corresponding advanced Sign Out has been made for you in the Front Desk Application.</div>" ' and the Time Off has been added to your outlook calendar.</div>"
        Else
            responseColor = "Red"

            'Either denied or cancelled:
            If responseNotes.Contains("This request has been cancelled by ") Then
                response = "Cancelled"
            Else
                response = "Denied"
            End If
        End If


        Dim responseEmail As New MailMessage(approverEmail, requestorEmail)
        responseEmail.CC.Add(approverEmail)


        '1/11/07 -- Added logic for IS Consulting
        If request.IsApproved Then
            Dim employeeHelper As New SvaEmployeeBLL
            Dim employees As SvaEmployee.SvaEmployeeDataTable = employeeHelper.GetEmployeeById(request.RequestorId)
            If employees.Rows.Count > 0 Then
                'If the request is approved and the requesting employee is in the Information Systems department,
                ' copy the IS Consulting Auditors in on this e-mail
                If employees(0).Department.Equals("Information Systems") Then
                    responseEmail.CC.Add(SvaParameters.GetParameter("ConsultingAuditors"))
                End If
            End If
        End If

        responseEmail.Subject = "FTO Request: " & response
        responseEmail.IsBodyHtml = True
        responseEmail.Body = "<div style='color:gray'>For your records:</div>" & _
                "<h3>The following FTO Request has been <span style='color:" & responseColor & "; font-weight:bold;'>" & response & "</span></h3>" & _
                "<div>From <span style='color:#316AC5; font-weight:bold;'>" & request.FromDate & "</span> to <span style='color:#316AC5; font-weight:bold;'>" & request.ToDate & "</span></div>" & _
                "<br /><table border='0'><tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Date of Request:</td><td style='padding:3px;'>" & request.RequestDate & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Requestor:</td><td style='padding:3px;'>" & request.Requestor & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Total Hours:</td><td style='padding:3px;'>" & Format(request.RequestHours, "f2") & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Type of Request:</td><td style='padding:3px;'>" & request.RequestType & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Request Notes:</td><td style='padding:3px;'>" & requestNotes & "</td></tr>" & _
                "<tr><td>&nbsp;</td><td></td></tr><tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Approver:</td><td style='padding:3px;'>" & request.Approver & "</td></tr>" & _
                "<tr><td style='padding:3px;width:150px;font-weight:bold;text-align:right;'>Response Notes:</td><td style='padding:3px;'>" & responseNotes & "</td></tr></table>" & _
                "<div><br />" & frontDeskNotice & "</div>"

        mailClient.Send(responseEmail)
    End Sub

#End Region

#Region " Get Request Info / Get Email functions "

    Private Function GetRequest(ByVal requestId As Integer) As ftoRequest.FtoRequestRow
        Dim ftoBll As New FtoRequestBLL
        Dim requests As ftoRequest.FtoRequestDataTable = ftoBll.GetFtoRequestByRequestId(requestId)
        If requests.Rows.Count > 0 Then
            Dim request As ftoRequest.FtoRequestRow = requests.Rows(0)
            Return request
        Else
            Throw New System.Exception("While e-mailing, could not find request info that matches FtoRequestId = " & requestId)
        End If
    End Function

    Private Function GetEmail(ByVal employeeId As Integer) As String
        Dim email As String = "UnknownEmployee" & employeeId & "@sva.com"

        Dim svaEmployeeBll As New SvaEmployeeBLL
        Dim employees As SvaEmployee.SvaEmployeeDataTable = svaEmployeeBll.GetEmployeeById(employeeId)
        If employees.Rows.Count > 0 Then
            Dim employee As SvaEmployee.SvaEmployeeRow = employees.Rows(0)
            If Not employee.IsEmpEmailNull Then email = employee.EmpEmail
        End If
        Return email
    End Function

#End Region

#Region " Add To Front Desk "

    Public Sub AddToFrontDesk(ByVal requestId As Integer)
        Dim request As ftoRequest.FtoRequestRow = GetRequest(requestId)

        Dim ftoBll As New FtoRequestBLL
        ftoBll.FrontDeskSignout(request.RequestorId, request.FromDate, request.ToDate, "FTO")
    End Sub

#End Region

#Region " Remove From Front Desk "

    Public Sub DeleteFromFrontDesk(ByVal requestId As Integer)
        Dim request As ftoRequest.FtoRequestRow = GetRequest(requestId)

        Dim ftoBll As New FtoRequestBLL
        ftoBll.DeleteFrontDeskSignout(request.RequestorId, request.FromDate, request.ToDate, "FTO")
    End Sub

#End Region


#Region " Add To Outlook "

    'Public Sub AddToOutlook(ByVal requestId As Integer)
    '    ''Dim request As ftoRequest.FtoRequestRow = GetRequest(requestId)

    '    ''Try

    '    ''    Dim outlook As New ApplicationClass() 'The instance of Outlook

    '    ''    'If appointment already exists - delete it
    '    ''    RemoveFromOutlook(requestId)

    '    ''    'Create an Outlook Appointment
    '    ''    Dim item As AppointmentItem = outlook.CreateItem(OlItemType.olAppointmentItem)

    '    ''    item.Start = request.FromDate
    '    ''    item.End = request.ToDate
    '    ''    item.Subject = "FTO"
    '    ''    item.Body = request.RequestNotes
    '    ''    item.BusyStatus = OlBusyStatus.olOutOfOffice
    '    ''    item.ReminderSet = False
    '    ''    item.Save()

    '    ''    item = Nothing
    '    ''    outlook = Nothing

    '    ''Catch ex As System.Exception
    '    ''    'If adding the appointment doesn't work, send an error message, but continue as if it's ok
    '    ''    SvaErrorMethods.ExceptionThrown(ex)

    '    ''End Try

    'End Sub

#End Region

#Region " Remove from Outlook "

    'Public Sub RemoveFromOutlook(ByVal requestId As Integer)
    '    ''Dim request As ftoRequest.FtoRequestRow = GetRequest(requestId)

    '    ''Try

    '    ''    Dim outlook As New ApplicationClass()
    '    ''    Dim outlookNamespace As Microsoft.Office.Interop.Outlook.NameSpace
    '    ''    outlookNamespace = outlook.GetNamespace("MAPI")
    '    ''    Dim appointment As AppointmentItem

    '    ''    'Examine the Appointments in the Calendar Folder looking for the one Appointment to Delete.
    '    ''    For Each appointment In outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderCalendar).Items

    '    ''        'If we find an appointment at the same time with FTO as the subject, delete it
    '    ''        If appointment.Subject = "FTO" _
    '    ''                And appointment.Start = request.FromDate _
    '    ''                And appointment.End = request.ToDate Then

    '    ''            appointment.Delete()
    '    ''            Exit For
    '    ''        End If

    '    ''    Next appointment

    '    ''    'Cleanup all the objects used to Delete the Appointment.
    '    ''    appointment = Nothing
    '    ''    outlookNamespace = Nothing
    '    ''    outlook = Nothing

    '    ''Catch ex As System.Exception
    '    ''    'If deleting the appointment doesn't work, send an error message, but continue as if it's ok
    '    ''    SvaErrorMethods.ExceptionThrown(ex)

    '    ''End Try

    'End Sub

#End Region

End Class
