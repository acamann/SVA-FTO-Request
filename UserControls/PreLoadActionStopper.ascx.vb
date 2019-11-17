
Partial Class UserControls_PreLoadActionStopper
    Inherits System.Web.UI.UserControl

    'Protected Overloads Overrides Sub OnLoad(ByVal e As EventArgs)
    '    Page.ClientScript.RegisterHiddenField("PageLoaded", "0")
    '    Dim scriptCommand As String = "document.getElementById('PageLoaded').value = '1';"
    '    Dim preSubmitCommand As String = "if(document.getElementById('PageLoaded').value=='1')" & Chr(13) & "" & Chr(10) & " {" & Chr(13) & "" & Chr(10) & " return true;" & Chr(13) & "" & Chr(10) & " }" & Chr(13) & "" & Chr(10) & " alert('Please allow the page to fully load before attempting an action.');" & Chr(13) & "" & Chr(10) & " return false;"

    '    Page.ClientScript.RegisterStartupScript(Me.[GetType](), "onLoad", scriptCommand, True)

    '    Page.ClientScript.RegisterOnSubmitStatement(Me.[GetType](), "OnSubmit", preSubmitCommand)
    '    MyBase.OnLoad(e)
    'End Sub

End Class
