Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Imports System.Data
Imports SecurityTableAdapters

Public Class SvaSecurityMethods

    Public Shared Function SetUpSessionSecurity() As Boolean
        If IsNothing(HttpContext.Current.Session("employeeId")) Then
            SvaSessionMethods.SetUpSessionVariables()
        End If

        Dim empId As Integer = HttpContext.Current.Session("employeeId")
        Dim applicationId As Integer = ConfigurationManager.AppSettings("ApplicationId")

        HttpContext.Current.Session("userParts") = GetUserParts(applicationId, empId)
    End Function

    Private Shared Function GetUserParts(ByVal applicationId As Integer, ByVal userId As Integer) As ArrayList
        Dim userPartsList As New ArrayList
        Dim userParts As New Security.SecurityPartDataTable
        userParts = PartAdapter.GetByUserId(applicationId, userId)
        For Each securityPart As Security.SecurityPartRow In userParts
            userPartsList.Add(securityPart.PartId.ToString)
        Next
        Return userPartsList

    End Function

    'Returns true if the current user has access to the Security Part with the passed ID
    Public Shared Function UserHasAccess(ByVal securityPartId As Integer) As Boolean
        If Not HttpContext.Current.Session("userParts") Is Nothing Then
            If CType(HttpContext.Current.Session("userParts"), ArrayList).Contains(securityPartId.ToString) Then
                Return True
            End If
        End If
        Return False
    End Function

    'for filling a drop down list with members of a security group
    Public Shared Function GetGroupMembers(ByVal groupId As Integer) As Security.SecurityUserDataTable
        Return UserAdapter.GetUsersByGroup(groupId)
    End Function

    Public Shared Function AddUserToGroup(ByVal empId As Integer, ByVal groupId As Integer) As Boolean
        Return UserAdapter.AddEmployeeToGroup(groupId, empId) = 1
    End Function

    Public Shared Function RemoveUserFromGroup(ByVal empId As Integer, ByVal groupId As Integer) As Boolean
        Return UserAdapter.DeleteEmployeeFromGroup(groupId, empId) = 1
    End Function

#Region " Table Adapters "

    Private Shared _partAdapter As SecurityPartTableAdapter = Nothing
    Protected Shared ReadOnly Property PartAdapter() As SecurityPartTableAdapter
        Get
            If _partAdapter Is Nothing Then
                _partAdapter = New SecurityPartTableAdapter()
            End If

            Return _partAdapter
        End Get
    End Property

    Private Shared _userAdapter As SecurityUserTableAdapter = Nothing
    Protected Shared ReadOnly Property UserAdapter() As SecurityUserTableAdapter
        Get
            If _userAdapter Is Nothing Then
                _userAdapter = New SecurityUserTableAdapter()
            End If

            Return _userAdapter
        End Get
    End Property

#End Region

End Class