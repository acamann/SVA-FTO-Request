
<System.ComponentModel.DefaultBindingProperty("SelectedValue")> _
Partial Class UserControls_CurrentDateTime
    Inherits System.Web.UI.UserControl

    <ComponentModel.Bindable(True)> _
        Public ReadOnly Property SelectedValue() As DateTime
        Get
            Return Now
        End Get
    End Property


End Class
