Imports System.Net.Mail

Partial Class _Default
    Inherits System.Web.UI.Page

#Region " Common Data Display Methods "

    Protected Function GetResponse(ByVal isApproved As Object, ByVal responseNotes As Object) As String
        'Turn the response Bit/Null value into a human readable string

        If Not responseNotes Is DBNull.Value Then
            If responseNotes.ToString.Contains("This request has been cancelled by ") Then
                Return "Cancelled"
            End If
        End If

        If isApproved Is DBNull.Value Then
            Return "Pending..."
        Else
            If CType(isApproved, Boolean) Then
                Return "Approved"
            Else
                Return "Denied"
            End If
        End If

    End Function

    Protected Function ColorResponse(ByVal isApproved As Object) As Drawing.Color
        'Color the response Bit/null value based on the response
        If isApproved Is DBNull.Value Then
            Return Drawing.Color.Gray
        Else
            If CType(isApproved, Boolean) Then
                Return Drawing.Color.Green
            Else
                Return Drawing.Color.Red
            End If
        End If
    End Function

    Protected Function ShortenString(ByVal originalString As Object, ByVal length As Integer) As String
        'Shorten a string object to the desired length
        If originalString Is DBNull.Value Then
            Return String.Empty
        Else
            Dim str As String = CType(originalString, String)
            If str.Length > length Then
                Return str.Substring(0, length) & "..."
            Else
                Return str
            End If
        End If
    End Function

#End Region

#Region " Select Request "

    Protected Sub rgHistory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rgHistory.SelectedIndexChanged

        'When a request is selected in the history grid, show the request and hide the grid
        rgHistory.Visible = False
        ViewRequest(rgHistory.SelectedValue)

    End Sub

    Protected Sub rgApprove_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rgApprove.SelectedIndexChanged

        'When a request is selected in the approval grid, show the request and hide the grid
        rgApprove.Visible = False
        ViewRequest(rgApprove.SelectedValue)

    End Sub

#End Region

#Region " Calendar Render "

    Protected Sub calFtoRange_DayRender(ByVal sender As Object, ByVal e As WebControls.DayRenderEventArgs)
        'Colors in the days of the calendar in the Request Form to reflect the request

        Dim fromDate As DateTime = CType(CType(fvFtoRequest.FindControl("lblFromDate"), Label).Text, DateTime)
        Dim toDate As DateTime = CType(CType(fvFtoRequest.FindControl("lblToDate"), Label).Text, DateTime)
        If e.Day.Date >= fromDate.Date And e.Day.Date <= toDate.Date Then
            e.Cell.BackColor = Drawing.ColorTranslator.FromHtml("#6788BE")
            e.Cell.ForeColor = Drawing.Color.White
            e.Cell.BorderWidth = 2
            e.Cell.BorderStyle = BorderStyle.Solid
        End If
    End Sub

#End Region

#Region " Navigation "

    Protected Sub rtsFto_TabClick(ByVal sender As Object, ByVal e As Telerik.WebControls.TabStripEventArgs) Handles rtsFto.TabClick
        SetUpDisplay()
    End Sub

    Private Sub SetUpDisplay()
        If Not rtsFto.SelectedTab Is Nothing Then
            Select Case rtsFto.SelectedTab.Text

                Case "New Request"
                    fvFtoRequest.Visible = True
                    fvFtoRequest.ChangeMode(FormViewMode.Insert)

                Case "My Request History"
                    fvFtoRequest.Visible = False
                    rgHistory.Visible = True
                    rgHistory.SelectedIndexes.Clear()

                Case "Approve Requests"
                    fvFtoRequest.Visible = False
                    rgApprove.Visible = True
                    rgApprove.SelectedIndexes.Clear()

                Case "FTO Calendar"
                    fvFtoRequest.Visible = False
                    ShowCalendar(Nothing, True)
                    
            End Select
        End If
    End Sub

#End Region

#Region " Page Load / QueryString "

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim IPAddress As String = Request.UserHostAddress.ToString
        'If IPAddress = "10.20.2.64" Then
        RadAjaxManager2.EnableAJAX = False
        'End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'show the Manual entry link at the bottom to anyone who has access to it
        If SvaSecurityMethods.UserHasAccess(10049) Then
            pnlManualEntry.Visible = True
        End If

        If Not IsPostBack Then
            HandleQueryString()
            HideApproveTab()
        End If

    End Sub

    Private Sub HandleQueryString()
        Select Case Request.QueryString("View")

            Case "Calendar"
                ShowCalendar(False, False)

            Case "Request"
                If Not Request.QueryString("RequestId") Is Nothing Then
                    Dim requestId As Integer = CType(Request.QueryString("RequestId"), Integer)
                    ViewRequest(requestId)
                End If

            Case Nothing

        End Select

        'SetUpDisplay()
    End Sub

#End Region

#Region " View Request "

    Private Sub ViewRequest(ByVal requestId As Integer)

        Dim fto As New FtoRequestBLL
        'Make sure the current user is either the requestor or approver
        If fto.UserHasAccess(Session("employeeId"), requestId) Then

            rtsFto.SelectedTab.Selected = False 'DeSelect a tab

            fvFtoRequest.Visible = True
            fvFtoRequest.ChangeMode(FormViewMode.Edit)
            odsFtoForm.SelectParameters("requestId").DefaultValue = requestId

        Else
            'Show Insufficient Access
            rtsFto.SelectedTab.Selected = False 'DeSelect a tab
            rmpFto.SelectedIndex = 4

        End If

    End Sub

#End Region

#Region " Hide Approve Tab "

    Private Sub HideApproveTab()

        'Hide the "Approve Requests" tab if the current user has no requests assigned
        Dim fto As New FtoRequestBLL
        If fto.GetFtoRequestsByApproverId(Session("employeeId")).Rows.Count = 0 Then
            rtsFto.Tabs(2).Visible = False
        End If

    End Sub

#End Region

#Region " Insert Request "

    Protected Sub fvFtoRequest_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewCommandEventArgs) Handles fvFtoRequest.ItemCommand
        If e.CommandName = "Email" Or e.CommandName = "Print" Then

            'If E-mail was clicked, set isEmail to true for handling in BLL
            odsFtoForm.InsertParameters("isEmail").DefaultValue = (e.CommandName = "Email")

            'Validate and insert the data
            fvFtoRequest.InsertItem(True)

        End If
    End Sub

    Protected Sub fvFtoRequest_ItemInserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewInsertEventArgs) Handles fvFtoRequest.ItemInserting
        'First check to see if this request conflicts with a previous request...

        Dim fromDate As DateTime = CType(e.Values("FromDate"), DateTime)
        Dim toDate As DateTime = CType(e.Values("ToDate"), DateTime)

        Dim ftoRequestBll As New FtoRequestBLL
        Dim conflicts As New ftoRequest.FtoRequestDataTable  'Generic.List(Of ftoRequest.FtoRequestRow)
        conflicts = ftoRequestBll.GetConflictingRequests(Session("employeeId"), fromDate, toDate)
        If conflicts.Count > 0 Then
            'Handle the conflict
            e.Cancel = True
            'rtsFto.SelectedTab.Selected = False 'DeSelect a tab
            rmpFto.SelectedIndex = 5
            fvFtoRequest.Visible = False
            rgConflicts.DataSource = conflicts
            rgConflicts.Rebind()
        End If

    End Sub

    ' Fired after a Request is made
    Protected Sub odsFtoForm_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFtoForm.Inserted
        If e.Exception Is Nothing Then
            ShowRequestAfterDataAccess(e.ReturnValue)
        End If
    End Sub

#End Region

#Region " Conflicting Requests "

    Protected Sub btnSubmitRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'deny all requests that conflict and then insert the request in the form

        Dim ftoRequestBll As New FtoRequestBLL

        Dim requestIds As New Generic.List(Of Integer)
        'Grab all of the RequestIds in the Conflicting Requests grid and remove them
        For Each row As Telerik.WebControls.GridDataItem In rgConflicts.Items
            requestIds.Add(CType(row.GetDataKeyValue("FtoRequestId"), Integer))
        Next

        ftoRequestBll.RemoveConflicts(requestIds)

        odsFtoForm.InsertParameters("isEmail").DefaultValue = True
        fvFtoRequest.InsertItem(True)

    End Sub

    Protected Sub btnCancelRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'return to the New Request form

        fvFtoRequest.Visible = True
        rmpFto.SelectedIndex = 0

    End Sub

#End Region

#Region " Update Request "

    ' Fired after a Request is approved or denied
    Protected Sub odsFtoForm_Updated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsFtoForm.Updated
        If e.Exception Is Nothing Then
            ShowRequestAfterDataAccess(e.ReturnValue)
        End If
    End Sub

    Private Sub ShowRequestAfterDataAccess(ByVal returnValue As Object)
        'Get the inserted request's Id
        Dim requestId As Integer
        If Not Integer.TryParse(returnValue, requestId) Then
            Throw New Exception("An error occurred while updating or e-mailing the FTO Request.")
        End If

        ' Show the request
        Response.Redirect("~/Default.aspx?View=Request&RequestId=" & requestId)
    End Sub

#End Region

#Region " Show Calendar View "

    Protected Sub lbCalendarView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ShowCalendar(True, True)
    End Sub

    Private Sub ShowCalendar(ByVal isOnlyMyEmployees As Nullable(Of Boolean), ByVal showTabStrip As Boolean)
        rtsFto.SelectedIndex = 3
        rmpFto.SelectedIndex = 3
        rtsFto.Visible = showTabStrip
        fvFtoRequest.Visible = False
        SvaCalendar1.Filter(isOnlyMyEmployees)
    End Sub

#End Region

#Region " Validate: FromDate < ToDate "

    'Check to make sure that the From date comes before the To date (if both are entered, otherwise Required Validator will catch it)
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

#End Region

#Region " Cancel Request "

    Protected Sub lbCancelRequest_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim requestId As Integer = e.CommandArgument

        Dim ftoBll As New FtoRequestBLL
        ftoBll.CancelRequest(requestId)
        fvFtoRequest.DataBind()

    End Sub

#End Region

End Class
