Imports Microsoft.VisualBasic
Imports SvaApplicationTableAdapters

Public Class SvaApplicationBLL

    Private _applicationAdapter As SvaApplicationTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SvaApplicationTableAdapter
        Get
            If _applicationAdapter Is Nothing Then
                _applicationAdapter = New SvaApplicationTableAdapter()
            End If

            Return _applicationAdapter
        End Get
    End Property

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetApplications() As SvaApplication.SvaApplicationDataTable
        Return Adapter.GetApplications()
    End Function

    <System.ComponentModel.DataObjectMethodAttribute _
        (System.ComponentModel.DataObjectMethodType.Select, False)> _
    Public Function GetApplicationById(ByVal applicationId As Integer) As SvaApplication.SvaApplicationRow
        Dim apps As SvaApplication.SvaApplicationDataTable = Adapter.GetApplicationById(applicationId)
        If apps.Rows.Count > 0 Then
            Return apps(0)
        Else
            Return Nothing
        End If
    End Function

End Class
