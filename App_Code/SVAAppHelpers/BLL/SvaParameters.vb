Imports Microsoft.VisualBasic
Imports SvaApplicationTableAdapters

Public Class SvaParameters

    Public Shared Function GetParameter(ByVal key As String) As String
        Dim adapter As New QueriesTableAdapter
        Dim applicationId As Integer = Integer.Parse(ConfigurationManager.AppSettings("ApplicationId"))

        Dim parameterData As String = String.Empty

        Dim obj As Object = adapter.GetParameter(applicationId, key)
        If Not obj Is Nothing Then
            parameterData = obj.ToString
        Else
            'Submit bug so we know this parameter could not be found
            Dim svaErrorMethods As New SvaErrorMethods
            svaErrorMethods.SubmitBug(Now, applicationId, _
                HttpContext.Current.Session("employeeId"), _
                "Parameter '" & key & "' could not be found in SVAApplicationFramework for this application. (This bug was automatically generated)")
        End If

        adapter.Dispose()

        Return parameterData
    End Function
End Class
