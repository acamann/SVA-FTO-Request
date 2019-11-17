
Partial Class SvaCalendar
    Inherits System.Web.UI.UserControl

    Private ftoDayOfMonth As New Generic.Dictionary(Of Integer, Generic.List(Of ftoRequest.FtoRequestRow))

#Region " Page Load "

    ''Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ''    If Not IsPostBack Then
    ''        HideFilter()
    ''    End If
    ''End Sub

#End Region

#Region " Hide Filter "

    ''Private Sub HideFilter()
    ''    'Hide the filter panel if the current user has no fto requests assigned
    ''    Dim fto As New FtoRequestBLL
    ''    If fto.GetFtoRequestsByApproverId(Session("employeeId")).Rows.Count = 0 Then
    ''        'TODO: remove this comment
    ''        pnlFilter.Visible = False
    ''    End If
    ''End Sub

#End Region

#Region " Filter Data "

    Public Sub Filter(ByVal isMyEmployeesOnly As Nullable(Of Boolean))

        If calFTO.SelectedDates.Count = 0 Then
            calFTO.SelectedDate = Today
        End If

        If Not pnlFilter.Visible = False And isMyEmployeesOnly.HasValue Then
            ddlFilter.ClearSelection()
            If isMyEmployeesOnly Then
                ddlFilter.Items.FindByText("Assigned To Me").Selected = True
                ShowResponseFilter()
            Else
                ddlFilter.Items.FindByText("All").Selected = True
            End If
        End If

        LoadGridAndCalendar()

    End Sub

    Protected Sub ddlFilter_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFilter.DataBound
        'When we bind the dropdown the first time,
        '  add additional options

        'Only add the assigned to me option if the user has anything assigned
        Dim fto As New FtoRequestBLL
        If fto.GetFtoRequestsByApproverId(Session("employeeId")).Rows.Count > 0 Then
            ddlFilter.Items.Insert(0, "----------------")
            ddlFilter.Items.Insert(0, New ListItem("Assigned To Me", "Me"))
        End If

        ddlFilter.Items.Insert(0, New ListItem("All", ""))
        '  and bind the grid and calendar (to "All" as the default)
        LoadGridAndCalendar()
    End Sub

    Protected Sub ddlFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFilter.SelectedIndexChanged
        LoadGridAndCalendar()
        ShowResponseFilter()
    End Sub

    Private Sub ShowResponseFilter()
        If ddlFilter.SelectedValue = "Me" Then
            'If "My Employees" is selected as the filter, show the IsApproved filter
            pnlResponseFilter.Visible = True
        Else
            pnlResponseFilter.Visible = False
        End If
    End Sub

    Protected Sub ddlIsApproved_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIsApproved.SelectedIndexChanged
        LoadGridAndCalendar()
    End Sub

#End Region

#Region " Load Grid And Calendar "

    Private Sub LoadGridAndCalendar()
        BindMonthData(calFTO.SelectedDate.Month, calFTO.SelectedDate.Year)
        DisplayRequestsForDay(calFTO.SelectedDate.Day)
        lblCurrentDay.Text = calFTO.SelectedDate.ToLongDateString
    End Sub

#End Region

#Region " Render Calendar Day "

    Protected Sub calFTO_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles calFTO.DayRender
        Dim dayNum As Integer = CType(e.Day.DayNumberText, Integer)
        Dim dayCount As Integer = 0
        If ftoDayOfMonth.ContainsKey(dayNum) Then dayCount = ftoDayOfMonth(dayNum).Count
        e.Cell.Controls.Clear()

        If Not e.Day.IsOtherMonth Then

            If Not e.Day.IsWeekend Then

                Dim link As String = e.SelectUrl
                If e.Day.IsSelected Then
                    link = "#"
                End If
                e.Cell.Controls.Add(New LiteralControl("<a href=""" & link & """ class='day'>"))
                e.Cell.Controls.Add(New LiteralControl("<div class='dayNum'>"))
                e.Cell.Controls.Add(New LiteralControl(dayNum))
                e.Cell.Controls.Add(New LiteralControl("</div>"))

                If dayCount > 0 Then
                    e.Cell.Controls.Add(New LiteralControl("<div class='dayContent'>("))
                    e.Cell.Controls.Add(New LiteralControl(dayCount))
                    e.Cell.Controls.Add(New LiteralControl(")</div>"))
                End If

                e.Cell.Controls.Add(New LiteralControl("</a>"))

            Else

                e.Cell.Controls.Add(New LiteralControl("<div class='day'><div class='dayNum'>" & dayNum & "</div></div>"))

            End If

        Else

            e.Cell.Controls.Add(New LiteralControl("<div class='isOtherMonth'></div>"))

        End If

    End Sub

#End Region

#Region " Bind Month Data "

    Private Sub BindMonthData(ByVal month As Integer, ByVal year As Integer)

        ftoDayOfMonth.Clear()

        'Hide the "Is Approved" column unless we are looking at 'all' of 'my employees' 
        Dim col As Telerik.WebControls.GridColumn = gvRequests.Columns.FindByUniqueNameSafe("IsApproved")
        If Not col Is Nothing Then col.Visible = False

        'find beginning and end of month
        Dim startDate As New Date(year, month, 1, 0, 0, 1)
        Dim endDate As New Date(year, month, Date.DaysInMonth(year, month), 23, 59, 59)

        'Load data for the month
        Dim ftoReqBll As New FtoRequestBLL
        Dim ftoMonth As ftoRequest.FtoRequestDataTable

        'Defaults to see all approved requests
        Dim approverId As Nullable(Of Integer) = Nothing
        Dim departmentId As String = Nothing
        Dim isApproved As Nullable(Of Boolean) = True

        If Not String.IsNullOrEmpty(ddlFilter.SelectedValue) Then
            If ddlFilter.SelectedValue = "Me" Then
                'If "Assigned to me" is selected, show it
                approverId = CType(Session("employeeId"), Integer)
                If String.IsNullOrEmpty(ddlIsApproved.SelectedValue) Then
                    isApproved = Nothing
                    If Not col Is Nothing Then col.Visible = True
                Else
                    isApproved = ddlIsApproved.SelectedValue
                End If
            Else
                'if any other value is selected (a department), show that department
                departmentId = ddlFilter.SelectedValue
            End If

        End If

        ftoMonth = ftoReqBll.GetFtoCalendar(approverId, Nothing, isApproved, startDate, endDate, departmentId)

            For Each request As ftoRequest.FtoRequestRow In ftoMonth

                'Find the first day of this request (within the month)
                Dim firstDay As Integer
                If request.FromDate.Month < month Then
                    'If this request started before this month began
                    firstDay = 1
                Else
                    firstDay = request.FromDate.Day
                End If

                'Find the last day of this request (within the month)
                Dim lastDay As Integer
                If request.ToDate.Month > month Then
                    'If this request ends after this month ends
                    lastDay = Date.DaysInMonth(year, month)
                Else
                    lastDay = request.ToDate.Day
                End If

                For dayNum As Integer = firstDay To lastDay
                    If Not ftoDayOfMonth.ContainsKey(dayNum) Then
                        ftoDayOfMonth.Add(dayNum, New Generic.List(Of ftoRequest.FtoRequestRow))
                    End If
                    ftoDayOfMonth(dayNum).Add(request)
                Next

            Next

    End Sub

#End Region

#Region " Display Requests "

    Private Sub DisplayRequestsForDay(ByVal dayNum As Integer)
        If ftoDayOfMonth.ContainsKey(dayNum) Then
            gvRequests.DataSource = GetData(ftoDayOfMonth(dayNum))
        Else
            gvRequests.DataSource = New ftoRequest.FtoRequestDataTable()
        End If
        gvRequests.DataBind()
    End Sub

    Protected Function GetIsApprovedImage(ByVal isApproved As Object) As String
        If isApproved Is DBNull.Value Then
            Return "http://apps.svamain.loc/common/images/icons/silkicons/bullet_white.png"
        Else
            If CType(isApproved, Boolean) Then
                Return "http://apps.svamain.loc/common/images/icons/silkicons/bullet_add.png"
            Else
                Return "http://apps.svamain.loc/common/images/icons/silkicons/bullet_delete.png"
            End If
        End If
    End Function

#End Region

#Region " Turn list of FtoRequests into DataTable "

    Private Function GetData(ByVal requests As Generic.List(Of ftoRequest.FtoRequestRow)) As Data.DataTable
        Dim reqTable As New Data.DataTable
        reqTable.Columns.Add("RequestId")
        reqTable.Columns.Add("Requestor")
        reqTable.Columns.Add("RequestorId")
        reqTable.Columns.Add("ApproverId")
        reqTable.Columns.Add("From")
        reqTable.Columns.Add("To")
        reqTable.Columns.Add("IsApproved")
        For Each request As ftoRequest.FtoRequestRow In requests
            Dim reqRow As Data.DataRow = reqTable.NewRow()
            reqRow("RequestId") = request.FtoRequestId
            reqRow("Requestor") = request.Requestor
            reqRow("RequestorId") = request.RequestorId
            reqRow("ApproverId") = request.ApproverId

            If request.IsIsApprovedNull Then
                reqRow("IsApproved") = Nothing
            Else
                reqRow("IsApproved") = request.IsApproved
            End If

            If request.FromDate.ToShortDateString = calFTO.SelectedDate.ToShortDateString Then
                reqRow("From") = request.FromDate.ToShortTimeString
            Else
                'reqRow("From") = request.FromDate.Month & "/" & request.FromDate.Day
                reqRow("From") = "8:00 AM"
            End If

            If request.ToDate.ToShortDateString = calFTO.SelectedDate.ToShortDateString Then
                reqRow("To") = request.ToDate.ToShortTimeString
            Else
                'reqRow("To") = request.ToDate.Month & "/" & request.ToDate.Day
                reqRow("To") = "5:00 PM"
            End If

            reqTable.Rows.Add(reqRow)
        Next
        Return reqTable
    End Function

#End Region

#Region " Event Handlers "

    Protected Sub calFTO_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calFTO.SelectionChanged
        LoadGridAndCalendar()
    End Sub

    Protected Sub calFTO_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles calFTO.VisibleMonthChanged
        calFTO.SelectedDate = e.NewDate
        LoadGridAndCalendar()
    End Sub

#End Region

End Class

