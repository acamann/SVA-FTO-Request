' PopOutCard.vb
Option Strict On
Imports System
Imports System.ComponentModel
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace SVA.CustomControls

    'handles formatting for the popoutcard
    Public Class Formatter
        Private _backgroundColor As String      'the background color of the popoutcard ("red", "#000000")
        Private _borderStyle As String          'the borderstyle of the popoutcard ("solid", "none")
        Private _borderColor As String          'the bordercolor of the popoutcard ("red", "#000000")
        Private _padding As String              'the padding amount inside of the popoutcard ("0px", "5em")

        Public Sub New(ByVal BorderStyle As String, ByVal BorderColor As String, ByVal Padding As String, ByVal BackgroundColor As String)
            MyBase.New()
            Me._backgroundColor = BackgroundColor
            Me._borderStyle = BorderStyle
            Me._borderColor = BorderColor
            Me._padding = Padding
        End Sub

        Public Property BorderColor() As String
            Get
                Return _borderColor
            End Get
            Set(ByVal value As String)
                _borderColor = value
            End Set
        End Property

        Public Property BackgroundColor() As String
            Get
                Return _backgroundColor
            End Get
            Set(ByVal value As String)
                _backgroundColor = value
            End Set
        End Property

        Public Property BorderStyle() As String
            Get
                Return _borderStyle
            End Get
            Set(ByVal value As String)
                _borderStyle = value
            End Set
        End Property

        Public Property Padding() As String
            Get
                Return _padding
            End Get
            Set(ByVal value As String)
                _padding = value
            End Set
        End Property
    End Class



    < _
    AspNetHostingPermission(SecurityAction.Demand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    AspNetHostingPermission(SecurityAction.InheritanceDemand, _
        Level:=AspNetHostingPermissionLevel.Minimal), _
    DefaultProperty("Text"), _
    ToolboxData( _
        "<{0}:PopOutCard runat=""server""> </{0}:PopOutCard>") _
    > _
    Public Class PopOutCard : Inherits WebControl : Implements INamingContainer

        Private _closeImageUrl As String = Nothing  'the url of the image used to 'close' the popoutcard
        Private _showImageUrl As String = Nothing   'the url of the image used to 'show' the popout card
        Private _showImageAlt As String = Nothing
        Private _onShowClick As String = Nothing
        Private _format As Formatter = New Formatter("solid", "#716F64", "5px", "White")

        Protected Overrides Sub OnDataBinding(ByVal e As EventArgs)
            EnsureChildControls()
            MyBase.OnDataBinding(e)
        End Sub

        < _
        Category("Appearance"), _
        DefaultValue(""), _
        Description("The image for the close button."), _
        Localizable(True) _
        > _
        Public Property CloseImageURL() As String
            Get
                Return _closeImageUrl
            End Get
            Set(ByVal value As String)
                _closeImageUrl = value
            End Set
        End Property


        < _
        Category("Appearance"), _
        DefaultValue(""), _
        Description("The image for the show panel button."), _
        Localizable(True) _
        > _
        Public Property ShowImageUrl() As String
            Get
                Return _showImageUrl
            End Get
            Set(ByVal value As String)
                _showImageUrl = value
            End Set
        End Property


        < _
        Category("Appearance"), _
        DefaultValue(""), _
        Description("The alternate text for the show panel button."), _
        Localizable(True) _
        > _
        Public Property ShowImageAlt() As String
            Get
                Return _showImageAlt
            End Get
            Set(ByVal value As String)
                _showImageAlt = value
            End Set
        End Property

        < _
        Category("Behavior"), _
        DefaultValue(""), _
        Description("The PUBLIC method to call when the show button is pressed."), _
        Localizable(True) _
        > _
        Public Property OnShowClick() As String
            Get
                Return _onShowClick
            End Get
            Set(ByVal value As String)
                _onShowClick = value
            End Set
        End Property


        Protected Overrides Sub CreateChildControls()

            ' Clear Controls
            Me.Controls.Clear()

            ' Add Show Button
            Dim btnShow As New ImageButton
            btnShow.AlternateText = _showImageAlt
            btnShow.ImageUrl = _showImageUrl
            btnShow.ID = "btnShowPanel"
            btnShow.CausesValidation = False
            AddHandler btnShow.Click, AddressOf btnShow_Click
            Me.Controls.Add(btnShow)

            ' Create containing Panel
            Dim popOutPanel As New Panel
            popOutPanel.Style.Add("position", "absolute")
            popOutPanel.Style.Add("border-color", _format.BorderColor)
            popOutPanel.Style.Add("border-style", _format.BorderStyle)
            popOutPanel.Style.Add("background-color", _format.BackgroundColor)
            popOutPanel.Style.Add("padding", _format.Padding)
            popOutPanel.Visible = False
            popOutPanel.ID = "popOutPanel"

            'open closeButton div
            popOutPanel.Controls.Add(New LiteralControl("<div style='float:left;width:auto;'>"))

            ' Add Close Button
            Dim btnClose As New ImageButton
            btnClose.AlternateText = "Close"
            btnClose.ImageUrl = _closeImageUrl
            btnClose.ID = "btnHidePanel"
            btnClose.CausesValidation = False
            AddHandler btnClose.Click, AddressOf btnClose_Click
            popOutPanel.Controls.Add(btnClose)

            'close closeButton div
            popOutPanel.Controls.Add(New LiteralControl("</div>"))

            'open cardDetail div
            popOutPanel.Controls.Add(New LiteralControl("<div style='float:left;height:1%;padding-left:5px;'>"))

            ' Add CardDetail
            If Not Content Is Nothing Then
                Dim popOut As New PopOutContent
                popOut.ID = "popOutContent"
                Content.InstantiateIn(popOut)
                popOutPanel.Controls.Add(popOut)
            End If

            'close cardDetail div
            popOutPanel.Controls.Add(New LiteralControl("</div>"))

            'Add containing Panel
            Me.Controls.Add(popOutPanel)

        End Sub


        Private _content As ITemplate

        <TemplateContainer(GetType(PopOutContent)), _
         PersistenceMode(PersistenceMode.InnerProperty), _
         TemplateInstance(TemplateInstance.Single)> _
        Public Property Content() As ITemplate
            Get
                Return _content
            End Get
            Set(ByVal value As ITemplate)
                _content = value
            End Set
        End Property

        Private Sub btnClose_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
            HidePanel()
        End Sub

        Private Sub btnShow_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
            If Not OnShowClick Is Nothing Then
                If Not Page.GetType.GetMethod(OnShowClick) Is Nothing Then
                    Page.GetType.GetMethod(OnShowClick).Invoke(Page, Nothing)
                End If
            End If

            ShowPanel()
        End Sub

        Public Sub ShowPanel()
            Me.FindControl("popOutPanel").Visible = True
        End Sub

        Public Sub HidePanel()
            Me.FindControl("popOutPanel").Visible = False
        End Sub

    End Class

    Public Class PopOutContent : Inherits Control : Implements INamingContainer

    End Class

End Namespace
