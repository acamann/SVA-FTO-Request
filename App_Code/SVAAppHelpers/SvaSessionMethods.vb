Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Imports System.Data

Public Class SvaSessionMethods

    ' Returns true if session variables are populated, false if for some reason they aren't
    Public Shared Function SetUpSessionVariables() As Boolean
        If HttpContext.Current.Session("employeeId") Is Nothing Then
            ' Determine the current user's userName
            Dim userName As String
            userName = Trim(HttpContext.Current.Request.ServerVariables("LOGON_USER")) 'their username with domain
            userName = Mid(userName, InStr(1, userName, "\") + 1, Len(userName)) 'shave the domain name

            Dim reader As SqlDataReader
            reader = SqlHelper.ExecuteReader( _
                ConfigurationManager.ConnectionStrings("cnSVAApplicationFramework").ConnectionString, _
                CommandType.StoredProcedure, _
                "GetEmployeeByUsername", _
                New SqlParameter("@UserName", userName))

            If reader.HasRows Then
                'Set up Session variables for the first time
                reader.Read()
                HttpContext.Current.Session("employeeId") = reader("EmpId").ToString
                HttpContext.Current.Session("name") = reader("EmpName").ToString
                HttpContext.Current.Session("email") = reader("EmpEmail").ToString
                HttpContext.Current.Session("department") = reader("Department").ToString
                HttpContext.Current.Session("office") = reader("Office").ToString
                Return True
            End If
        Else
            'Session variables are already set up
            Return True
        End If
        Return False
    End Function

End Class
