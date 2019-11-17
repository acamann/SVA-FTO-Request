Imports Microsoft.VisualBasic
Imports ftoRequestTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FtoRequestBLL

#Region " Adapters "

    Private _adapter As FtoRequestTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As FtoRequestTableAdapter
        Get
            If _adapter Is Nothing Then
                _adapter = New FtoRequestTableAdapter
            End If
            Return _adapter
        End Get
    End Property

    Private _queriesAdapter As QueriesTableAdapter = Nothing
    Protected ReadOnly Property QueriesAdapter() As QueriesTableAdapter
        Get
            If _queriesAdapter Is Nothing Then
                _queriesAdapter = New QueriesTableAdapter
            End If
            Return _queriesAdapter
        End Get
    End Property

    Private _requestTypeAdapter As FtoRequestTypeTableAdapter = Nothing
    Protected ReadOnly Property RequestTypeAdapter() As FtoRequestTypeTableAdapter
        Get
            If _requestTypeAdapter Is Nothing Then
                _requestTypeAdapter = New FtoRequestTypeTableAdapter
            End If
            Return _requestTypeAdapter
        End Get
    End Property

#End Region

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFtoRequestsByRequestorId(ByVal requestorId As Integer) As ftoRequest.FtoRequestDataTable
        Return Adapter.GetFtoRequestsByRequestorId(requestorId)
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetFtoRequestsByApproverId(ByVal approverId As Integer) As ftoRequest.FtoRequestDataTable
        Return Adapter.GetFtoRequestsByApproverId(approverId)
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetFtoRequestByRequestId(ByVal requestId As Integer) As ftoRequest.FtoRequestDataTable
        Return Adapter.GetFtoRequestByRequestId(requestId)
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetFtoCalendar( _
            ByVal approverId As Nullable(Of Integer), _
            ByVal requestorId As Nullable(Of Integer), _
            ByVal isApproved As Nullable(Of Boolean), _
            ByVal startDate As Nullable(Of DateTime), _
            ByVal endDate As Nullable(Of DateTime), _
            ByVal departmentId As String) _
                As ftoRequest.FtoRequestDataTable

        If String.IsNullOrEmpty(departmentId) Then departmentId = Nothing

        Return Adapter.GetFtoCalendar(approverId, requestorId, isApproved, startDate, endDate, departmentId)

    End Function


    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertFtoRequest( _
            ByVal requestorId As Integer, _
            ByVal requestDate As DateTime, _
            ByVal fromDate As DateTime, _
            ByVal toDate As DateTime, _
            ByVal requestHours As Decimal, _
            ByVal requestTypeId As Integer, _
            ByVal ftoBalance As Decimal, _
            ByVal approverId As Integer, _
            ByVal requestNotes As String, _
            ByVal isEmail As Boolean) _
                As Integer

        If String.IsNullOrEmpty(requestNotes) Then requestNotes = Nothing

        'Insert the request and receive the newly created requestId
        Dim ftoRequestId As Integer
        ftoRequestId = Adapter.InsertRequest(requestorId, requestDate, fromDate, toDate, _
            requestHours, requestTypeId, ftoBalance, approverId, requestNotes)

        'If the user chose to Email the request, send the email
        If isEmail Then
            Dim ftoHelper As New FtoHelper
            ftoHelper.EmailRequest(ftoRequestId)
        End If

        Return ftoRequestId
    End Function




    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertManually( _
            ByVal requestorId As Integer, _
            ByVal fromDate As DateTime, _
            ByVal toDate As DateTime, _
            ByVal requestHours As Decimal, _
            ByVal requestTypeId As Integer, _
            ByVal requestNotes As String, _
            ByVal isEmail As Boolean) _
                As Integer

        If String.IsNullOrEmpty(requestNotes) Then requestNotes = Nothing

        Dim responseNotes As String = "This request was added manually by " & HttpContext.Current.Session("name") & " (#" & HttpContext.Current.Session("employeeId") & ")"

        'Insert the request and receive the newly created requestId
        Dim ftoRequestId As Integer
        ftoRequestId = Adapter.InsertManually(requestorId, fromDate, toDate, _
            requestHours, requestTypeId, GetFtoBalance(requestorId), GetApproverId(requestorId), requestNotes, responseNotes)

        'Add the request to the front desk
        Dim ftoHelper As New FtoHelper
        ftoHelper.AddToFrontDesk(ftoRequestId)

        'Email the notification
        'If the user chose to Email the request, send the email
        If isEmail Then
            ftoHelper.EmailResponse(ftoRequestId)
        End If

        Return ftoRequestId
    End Function

    'Allows an employee/supervisor to cancel a pre-approved request and it changes the
    Public Function CancelRequest(ByVal requestId As Integer) As Boolean
        Dim requests As ftoRequest.FtoRequestDataTable
        Dim request As ftoRequest.FtoRequestRow

        requests = GetFtoRequestByRequestId(requestId)
        If requests.Count > 0 Then
            request = requests(0)

            Dim responseNotes As New StringBuilder
            If Not request.IsResponseNotesNull Then
                responseNotes.Append(request.ResponseNotes & " ")
            End If
            responseNotes.Append("(")
            If Not request.IsResponseDateNull Then
                responseNotes.Append("This request was originally ")
                If request.IsApproved Then
                    responseNotes.Append("approved")
                Else
                    responseNotes.Append("denied")
                End If
                responseNotes.Append(" on " & request.ResponseDate & ". ")
            End If

            responseNotes.Append("This request has been cancelled by " & HttpContext.Current.Session("Name") & ".)")

            Return RespondToFtoRequest(requestId, Now(), False, responseNotes.ToString) > 0
        Else
            Return False
        End If
    End Function


    Public Function GetConflictingRequests(ByVal requestorId As Integer, ByVal fromDate As DateTime, ByVal toDate As DateTime) As ftoRequest.FtoRequestDataTable ' Generic.List(Of ftoRequest.FtoRequestRow)
        Dim conflicts As New ftoRequest.FtoRequestDataTable  'Generic.List(Of ftoRequest.FtoRequestRow)

        Dim requests As ftoRequest.FtoRequestDataTable = GetFtoRequestsByRequestorId(requestorId)
        For Each request As ftoRequest.FtoRequestRow In requests
            If request.IsIsApprovedNull OrElse request.IsApproved Then
                If (toDate > request.FromDate And fromDate < request.ToDate) Then
                    'Request conflicts with a previously approved or pending request
                    conflicts.ImportRow(request)
                End If
            End If
        Next
        Return conflicts
    End Function


    Public Function RemoveConflicts(ByVal requestIds As Generic.List(Of Integer)) As Boolean
        Dim requests As ftoRequest.FtoRequestDataTable
        Dim request As ftoRequest.FtoRequestRow

        'for each conflicting request, deny them
        For Each requestId As Integer In requestIds
            requests = GetFtoRequestByRequestId(requestId)
            If requests.Count > 0 Then
                request = requests(0)

                Dim responseNotes As New StringBuilder
                If Not request.IsResponseNotesNull Then
                    responseNotes.Append(request.ResponseNotes & " ")
                End If
                responseNotes.Append("(")
                If Not request.IsResponseDateNull Then
                    responseNotes.Append("This request was originally approved on " & request.ResponseDate & ". ")
                End If

                responseNotes.Append("This request has been automatically denied due to a conflict with a more recent request.)")

                RespondToFtoRequest(requestId, Now(), False, responseNotes.ToString)

            End If
        Next
    End Function


    <System.ComponentModel.DataObjectMethodAttribute _
            (System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function RespondToFtoRequest( _
                ByVal ftoRequestId As Integer, _
                ByVal responseDate As DateTime, _
                ByVal isApproved As Boolean, _
                ByVal responseNotes As String) _
                    As Integer

        If String.IsNullOrEmpty(responseNotes) Then responseNotes = Nothing

        'Update the request in the database
        Adapter.Update(ftoRequestId, responseDate, isApproved, responseNotes)

        Dim ftoHelper As New FtoHelper

        If isApproved Then
            'Add to front desk
            ftoHelper.AddToFrontDesk(ftoRequestId)

        Else
            'Remove from front desk
            ftoHelper.DeleteFromFrontDesk(ftoRequestId)

        End If

        'Email the confirmation
        ftoHelper.EmailResponse(ftoRequestId)

        Return ftoRequestId

    End Function

    Public Function GetApproverId(ByVal requestorId As Integer) As Integer
        Dim approverId As Integer = CType(QueriesAdapter.ReturnApproverId(requestorId), Integer)

        Return approverId
    End Function

    Public Function GetFtoBalance(ByVal employeeId As Integer) As Double
        Dim ftoBalance As Double
        If Not Double.TryParse(QueriesAdapter.ReturnFtoBalance(employeeId), ftoBalance) Then
            Return 0.0
        End If

        Return ftoBalance
    End Function

    Public Function FrontDeskSignout( _
            ByVal employeeId As Integer, _
            ByVal leavedateTime As DateTime, _
            ByVal returnDatetime As DateTime, _
            ByVal notes As String) As Boolean

        Return QueriesAdapter.FrontDeskSignout(employeeId, leavedateTime, returnDatetime, notes) = 1
    End Function

    Public Function DeleteFrontDeskSignout( _
                ByVal employeeId As Integer, _
                ByVal leavedateTime As DateTime, _
                ByVal returnDatetime As DateTime, _
                ByVal notes As String) As Boolean

        Return QueriesAdapter.DeleteFrontDeskSignout(employeeId, leavedateTime, returnDatetime, notes) = 1
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetFtoRequestTypes() As ftoRequest.FtoRequestTypeDataTable
        Return RequestTypeAdapter.GetFtoRequestTypes()
    End Function

    Public Function UserHasAccess(ByVal employeeId As Integer, ByVal requestId As Integer) As Boolean
        If employeeId = 146 Then Return True ' Gives me ability to view every request

        Dim requests As ftoRequest.FtoRequestDataTable = GetFtoRequestByRequestId(requestId)
        If requests.Rows.Count > 0 Then
            Dim request As ftoRequest.FtoRequestRow = requests.Rows(0)
            'if the employee is the requestor or approver, return true
            If request.RequestorId.Equals(employeeId) Or request.ApproverId.Equals(employeeId) Then
                Return True
            End If
        End If
        Return False
    End Function


End Class
