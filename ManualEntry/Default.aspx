<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" 
    Inherits="ManualEntry_Default" %>

<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<%@ Register Src="../UserControls/BindDateTime.ascx" TagName="BindDateTime" TagPrefix="sva" %>

<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="rad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link rel="stylesheet" href="../App_Themes/SvaCalendar/SvaCalendar.css" type="text/css" />
    <link rel="stylesheet" href="../App_Themes/DivGrid/DivGrid.css" type="text/css" />
    <link rel="stylesheet" href="../App_Themes/ErrorPages/ErrorPages.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Printable/Printable.css" media="print" /> 
    <title>FTO Request - Manual Entry</title>
    <style type="text/css">

        .ftoCalPreview {
            font-weight:normal;
        }
        
        .ftoDate {
            font-weight:bold;
            color:#6788BE;
        }

        .control {
            font-family:Tahoma;
            font-size:11px;
        }
        
        /* transparent png fix for IE5.5 & IE6 */
        img {
            behavior: url("Transparency/pngbehavior.htc");
        }

    </style>
</head>
<body style="padding:0px; margin:0px; max-width:600px; min-width:500px;font-size:11px; font-family:Tahoma;">
    <form id="form1" runat="server" style="padding:0px; margin:0px;">
    <div style="padding:0px; margin:0px;">
    
    <h3 style="padding:5px;">Manual Entry</h3>
    
    <radTS:RadMultiPage ID="rmpManual" runat="server" SelectedIndex="0">
        <radTS:PageView ID="pageInsert" runat="server">
            <asp:FormView style="width:100%; border:none;"
                    id="fvFtoRequest" runat="server" 
                    datasourceid="odsFtoForm" DefaultMode="Insert">
                <InsertItemTemplate>
                
                    <div class="row">
                        <div class="left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlRequestor"
                                runat="server" ErrorMessage="***" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            
                            Employee:
                        </div>
                        <div class="right">
                            <asp:DropDownList ID="ddlRequestor" runat="server"
                                    DataSourceID="odsEmployees" DataTextField="EmpName"
                                    DataValueField="EmpId" CssClass="control"
                                    AppendDataBoundItems="true"
                                    SelectedValue='<%# Bind("RequestorId") %>'>
                                <asp:ListItem Text="Choose..." Value=""></asp:ListItem>
                            </asp:DropDownList> 
                            <asp:ObjectDataSource ID="odsEmployees" runat="server" 
                                TypeName="SVAEmployeeBLL" SelectMethod="GetEmployees">
                            </asp:ObjectDataSource>                       
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="left">
                            <asp:RequiredFieldValidator ID="rfvFromDate" ControlToValidate="bdtFrom"
                                runat="server" ErrorMessage="***" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            
                            From:
                        </div>
                        <div class="right">
                            
                            <sva:BindDateTime ID="bdtFrom" runat="server" 
                                SelectedValue='<%# Bind("FromDate") %>' defaultMode="FromDate"  />
                            
                        </div>
                        
                    </div>
                    
                    <div class="row">
                        <div class="left">
                           <asp:RequiredFieldValidator ID="rfvToDate" ControlToValidate="bdtTo"
                                runat="server" ErrorMessage="***" Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            
                            To:
                        </div>
                        <div class="right">

                            <sva:BindDateTime ID="bdtTo" runat="server" 
                                SelectedValue='<%# Bind("ToDate") %>' defaultMode="ToDate"  />
                            
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="left">
                        </div>
                        <div class="right">
                            <asp:CustomValidator ID="cvDates" runat="server" 
                                ControlToValidate="bdtFrom" Display="Dynamic"
                                OnServerValidate="cvDates_Validate"
                                ErrorMessage="No time travel - the 'To' date must come after the 'From' date.">
                            </asp:CustomValidator>
                        </div>
                    </div>
                                    
                    <div class="row">
                        <div class="left"></div>
                        <div class="right">
                            <asp:CheckBox ID="chkEmail" runat="server" Checked='<%#Bind("IsEmail")%>' /> Send an Email Notification to the employee taking time off and his/her supervisor.
                        </div> 
                    </div>
                    
                    <br />
                    
                    <div class="row">
                        <div class="left"></div>
                        <div class="right">
                            <asp:Button ID="btnInsert" runat="server"
                                Text="Insert" CssClass="control"
                                CommandName="Insert" CausesValidation="True"  />
                            &nbsp;(This time-off will be added to the FTO Calendar and the Front Desk)
                        </div> 
                    </div>
                    <br  />
                    
                </InsertItemTemplate>
            </asp:FormView>    
        </radTS:PageView>
        <radTS:PageView ID="pageSuccess" runat="server">
            The request was successfully entered.
            <br />
            <br />
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ManualEntry/Default.aspx">Insert Another Request</asp:HyperLink>
            
        </radTS:PageView>
        <radTS:PageView ID="pageFailure" runat="server">
            There was a problem submitting this request.  
            A notification has been sent to the application administrator.
            <br />
            <br />
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ManualEntry/Default.aspx">Try Again</asp:HyperLink>
            
        </radTS:PageView>
    </radTS:RadMultiPage>
    
        <br /><br />
    
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Default.aspx">
            <img src="../App_Themes/Gray/Icons/door_open.png" style="border:none;width:16px;height:16px;" alt="Back" />
            Back to FTO Request Site
        </asp:HyperLink>
    
    
    
        <asp:ObjectDataSource id="odsFtoForm" runat="server" TypeName="FtoRequestBLL" 
                SelectMethod="GetFtoRequestByRequestId" 
                InsertMethod="InsertManually">
            <SelectParameters>
                <asp:Parameter Name="requestId" Type="Int32"  />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="requestorId" Type="Int32"  />
                <asp:Parameter Name="fromDate" Type="DateTime"  />
                <asp:Parameter Name="toDate" Type="DateTime"  />
                <asp:Parameter Name="requestHours" Type="Decimal" DefaultValue="0"  />
                <asp:Parameter Name="requestTypeId" Type="Int32" DefaultValue="6"  />
                <asp:Parameter Name="requestNotes" Type="String"  />
                <asp:Parameter Name="isEmail" Type="boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        
        </div>
    </form>
</body>
</html>
