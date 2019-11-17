Imports Microsoft.VisualBasic
Imports SvaEmployeeTableAdapters

Public Class SvaEmployeeBLL

    Private _employeeAdapter As SvaEmployeeTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SvaEmployeeTableAdapter
        Get
            If _employeeAdapter Is Nothing Then
                _employeeAdapter = New SvaEmployeeTableAdapter()
            End If

            Return _employeeAdapter
        End Get
    End Property

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetEmployees() As SvaEmployee.SvaEmployeeDataTable
        Return Adapter.GetEmployees
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetEmployeeByUserName(ByVal userName As String) As SvaEmployee.SvaEmployeeDataTable
        Return Adapter.GetEmployeeByUserName(userName)
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetEmployeeById(ByVal employeeId As Integer) As SvaEmployee.SvaEmployeeDataTable
        Return Adapter.GetEmployeeById(employeeId)
    End Function

End Class
