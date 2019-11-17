<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Src="UserControls/PreLoadActionStopper.ascx" TagName="PreLoadActionStopper"
    TagPrefix="uc2" %>

<%@ Register Src="UserControls/SubmitBugLink.ascx" TagName="SubmitBugLink" TagPrefix="uc1" %>

<%@ Register Src="UserControls/ApproveRadios.ascx" TagName="ApproveRadios" TagPrefix="sva" %>
<%@ Register Src="UserControls/CurrentDateTime.ascx" TagName="CurrentDateTime" TagPrefix="sva" %>
<%@ Register Src="UserControls/BindDateTime.ascx" TagName="BindDateTime" TagPrefix="sva" %>
<%@ Register Src="UserControls/Approver.ascx" TagName="Approver" TagPrefix="sva" %>
<%@ Register Src="UserControls/CurrentUser.ascx" TagName="CurrentUser" TagPrefix="sva" %>
<%@ Register Src="UserControls/FtoBalance.ascx" TagName="FtoBalance" TagPrefix="sva" %>
<%@ Register Src="Calendar/SvaCalendar.ascx" TagName="SvaCalendar" TagPrefix="sva" %>

<%@ Register Assembly="RadInput.Net2" Namespace="Telerik.WebControls" TagPrefix="radI" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>
<%@ Register Assembly="RadAjax.Net2" Namespace="Telerik.WebControls" TagPrefix="radA" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<%@ Register Assembly="RadTabStrip.Net2" Namespace="Telerik.WebControls" TagPrefix="radTS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link rel="stylesheet" href="App_Themes/SvaCalendar/SvaCalendar.css" type="text/css" />
    <link rel="stylesheet" href="App_Themes/DivGrid/DivGrid.css" type="text/css" />
    <link rel="stylesheet" href="App_Themes/ErrorPages/ErrorPages.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="App_Themes/Printable/Printable.css" media="print" /> 
    <title>Flexible Time Off (FTO) - Request Form</title>
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
        
        <radA:RadAjaxManager ID="RadAjaxManager2" runat="server">
            <AjaxSettings>
                <radA:AjaxSetting AjaxControlID="rtsFto">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="rtsFto" />
                        <radA:AjaxUpdatedControl ControlID="rmpFto" />
                        <radA:AjaxUpdatedControl ControlID="fvFtoRequest" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="rgHistory">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="rtsFto" />
                        <radA:AjaxUpdatedControl ControlID="rmpFto" />
                        <radA:AjaxUpdatedControl ControlID="fvFtoRequest" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="rgApprove">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="rtsFto" />
                        <radA:AjaxUpdatedControl ControlID="rmpFto" />
                        <radA:AjaxUpdatedControl ControlID="fvFtoRequest" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="pnlCalendar">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="pnlCalendar" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="btnSubmitRequest">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="rtsFto" />
                        <radA:AjaxUpdatedControl ControlID="rmpFto" />
                        <radA:AjaxUpdatedControl ControlID="fvFtoRequest" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="btnCancelRequest">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="rtsFto" />
                        <radA:AjaxUpdatedControl ControlID="rmpFto" />
                        <radA:AjaxUpdatedControl ControlID="fvFtoRequest" />
                    </UpdatedControls>
                </radA:AjaxSetting>
                <radA:AjaxSetting AjaxControlID="fvFtoRequest">
                    <UpdatedControls>
                        <radA:AjaxUpdatedControl ControlID="rtsFto" />
                        <radA:AjaxUpdatedControl ControlID="rmpFto" />
                        <radA:AjaxUpdatedControl ControlID="fvFtoRequest" />
                    </UpdatedControls>
                </radA:AjaxSetting>
            </AjaxSettings>
        </radA:RadAjaxManager>

        <radTS:RadTabStrip ID="rtsFto" runat="server" AutoPostBack="True" MultiPageID="rmpFto" CausesValidation="False" SelectedIndex="0" Skin="Web20">
            <Tabs>
                <radTS:Tab Text="New Request" runat="server" PageViewID="pageNewRequest"></radTS:Tab>
                <radTS:Tab Text="My Request History" PageViewID="pageHistory" runat="server"></radTS:Tab>
                <radTS:Tab Text="Approve Requests" runat="server" PageViewID="pageApprove"></radTS:Tab>
                <radTS:Tab Text="FTO Calendar" runat="server" PageViewID="pageCalendar"></radTS:Tab>
            </Tabs>
        </radTS:RadTabStrip>
        
        <asp:ObjectDataSource ID="odsFto" runat="server" SelectMethod="GetFtoRequestsByRequestorId" TypeName="FtoRequestBLL">
            <SelectParameters>
                <asp:SessionParameter Name="requestorId" SessionField="employeeId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <radTS:RadMultiPage ID="rmpFto" runat="server" RenderSelectedPageOnly="True" SelectedIndex="0">
            <radTS:PageView ID="pageNewRequest" runat="server"></radTS:PageView>
            <radTS:PageView ID="pageHistory" runat="server">
            
                <radG:RadGrid ID="rgHistory" runat="server" DataSourceID="odsFto" GridLines="None" Skin="Web20" AllowSorting="True"
                        style="border-top:none;">
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="FtoRequestId" DataSourceID="odsFto" CommandItemDisplay="Top">
                        <SortExpressions>
                            <radG:GridSortExpression FieldName="RequestDate" SortOrder="Descending" />                            
                        </SortExpressions>
                        <CommandItemTemplate>
                            <div style="font-size:12px; font-family:Tahoma; color:#3B4F69; padding-bottom:6px;">
                                Requests made by <%= Context.Session("name") %> (1 year history)
                            </div>
                        </CommandItemTemplate>
                        <Columns>
                            <radG:GridTemplateColumn UniqueName="column">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select"> 
                                        <img src="http://apps.svamain.loc/common/images/icons/silkicons/magnifier.png" style="border:none; width:16px; height:16px;" alt="Select" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </radG:GridTemplateColumn>
                            <radG:GridTemplateColumn DataField="RequestDate" DataType="System.DateTime" HeaderText="Req Date"
                                    SortExpression="RequestDate" UniqueName="RequestDate">
                                <ItemTemplate>
                                    <%#Eval("RequestDate", "{0:d}") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <%#Eval("RequestDate")%>
                                </EditItemTemplate>
                            </radG:GridTemplateColumn>
                            <radG:GridTemplateColumn DataField="FromDate" DataType="System.DateTime" HeaderText="From"
                                    SortExpression="FromDate" UniqueName="TemplateColumn">
                                <ItemTemplate>
                                    <span class="ftoDate"><%#Eval("FromDate", "{0:d}")%> </span><%#Eval("FromDate", "{0:t}")%>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </radG:GridTemplateColumn>
                            <radG:GridTemplateColumn DataField="ToDate" DataType="System.DateTime" HeaderText="To"
                                    SortExpression="ToDate" UniqueName="TemplateColumn1">
                                <ItemTemplate>
                                    <span class="ftoDate"><%#Eval("ToDate", "{0:d}")%> </span><%#Eval("ToDate", "{0:t}")%>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </radG:GridTemplateColumn>
                            <radG:GridBoundColumn DataField="RequestHours" DataType="System.Decimal" HeaderText="Hours"
                                SortExpression="RequestHours" UniqueName="RequestHours" DataFormatString="{0:f2}">
                            </radG:GridBoundColumn>
                            <radG:GridTemplateColumn DataField="IsApproved" HeaderText="Response" SortExpression="IsApproved" UniqueName="IsApproved">
                                <ItemTemplate>
                                    <asp:Label runat="Server"
                                        Text='<%# GetResponse(Eval("IsApproved"), Eval("ResponseNotes")) %>'
                                        ForeColor='<%# ColorResponse(Eval("IsApproved")) %>'
                                        Font-Bold='<%# Not Eval("IsApproved") is DBNull.Value %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </radG:GridTemplateColumn>
                        </Columns>
                        <ExpandCollapseColumn Resizable="False" Visible="False">
                            <HeaderStyle Width="20px" />
                        </ExpandCollapseColumn>
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                    </MasterTableView>
                    <ExportSettings>
                        <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                            PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                    </ExportSettings>
                </radG:RadGrid>
                
            </radTS:PageView>
            
            <radTS:PageView ID="pageApprove" runat="server">
            
                
            
                <asp:ObjectDataSource ID="odsApprove" runat="server" SelectMethod="GetFtoRequestsByApproverId" TypeName="FtoRequestBLL" >
                    <SelectParameters>
                        <asp:SessionParameter Name="approverId" SessionField="employeeId" Type="Int32" />
                        <%--<asp:Parameter Name="approverId" DefaultValue="774" />--%>
                    </SelectParameters>
                </asp:ObjectDataSource>
                
                <radG:RadGrid ID="rgApprove" runat="server" DataSourceID="odsApprove" GridLines="None"
                        Skin="Web20" AllowSorting="True" style="border-top:none;" >
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="FtoRequestId" DataSourceID="odsApprove" 
                            AllowMultiColumnSorting="True" CommandItemDisplay="Top">
                        <SortExpressions>
                            <radG:GridSortExpression FieldName="IsApproved" />
                            <radG:GridSortExpression FieldName="FromDate" SortOrder="Descending" />                             
                        </SortExpressions>
                        <CommandItemTemplate>
                           <div style="padding-bottom:6px; font-size:12px; font-family:Tahoma; color:#3B4F69; 
                                    clear:both; height:1%;">
                                <div style="float:left; width:auto;">
                                    Requests assigned to <%= Context.Session("name") %> (2 month history)
                                </div>
                                <div style="float:right; text-align:right; width:auto;">
                                    <asp:LinkButton ID="lbCalendarView" runat="server" OnClick="lbCalendarView_Click">
                                        <img src="http://apps.svamain.loc/common/images/icons/silkicons/calendar.png" alt="Calendar View" style="border:none;height:16px;width:16px;" />
                                        Calendar View
                                    </asp:LinkButton>
                                </div>
                            </div>             
                        </CommandItemTemplate>
                        <Columns>
                            <radG:GridTemplateColumn UniqueName="column">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="Select"> 
                                        <img src="http://apps.svamain.loc/common/images/icons/silkicons/magnifier.png" style="border:none; width:16px; height:16px;" alt="Select" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </radG:GridTemplateColumn>
                            <radG:GridBoundColumn DataField="Requestor" HeaderText="Requestor" ReadOnly="True"
                                SortExpression="Requestor" UniqueName="Requestor">
                            </radG:GridBoundColumn>
                            <radG:GridTemplateColumn DataField="FromDate" DataType="System.DateTime" HeaderText="From"
                                    SortExpression="FromDate" UniqueName="TemplateColumn">
                                <ItemTemplate>
                                    <span class="ftoDate"><%#Eval("FromDate", "{0:d}")%> </span><%#Eval("FromDate", "{0:t}")%>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </radG:GridTemplateColumn>
                            <radG:GridTemplateColumn DataField="ToDate" DataType="System.DateTime" HeaderText="To"
                                    SortExpression="ToDate" UniqueName="TemplateColumn1">
                                <ItemTemplate>
                                    <span class="ftoDate"><%#Eval("ToDate", "{0:d}")%> </span><%#Eval("ToDate", "{0:t}")%>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </radG:GridTemplateColumn>
                            <radG:GridBoundColumn DataField="RequestHours" DataType="System.Decimal" HeaderText="Hours"
                                SortExpression="RequestHours" UniqueName="RequestHours" DataFormatString="{0:f2}">
                            </radG:GridBoundColumn>
                            <radG:GridTemplateColumn DataField="IsApproved" HeaderText="Response" SortExpression="IsApproved" UniqueName="IsApproved">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="Server" 
                                        Text='<%# GetResponse(Eval("IsApproved"), Eval("ResponseNotes")) %>'
                                        ForeColor='<%# ColorResponse(Eval("IsApproved")) %>'
                                        Font-Bold='<%# Not Eval("IsApproved") is DBNull.Value %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </radG:GridTemplateColumn>
                        </Columns>
                        <ExpandCollapseColumn Resizable="False" Visible="False">
                            <HeaderStyle Width="20px" />
                        </ExpandCollapseColumn>
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                    </MasterTableView>
                    <ExportSettings>
                        <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                            PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                    </ExportSettings>
                </radG:RadGrid>
                
            </radTS:PageView>
            
            <radTS:PageView ID="pageCalendar" runat="server">
                <asp:Panel ID="pnlCalendar" runat="server" style="border:solid 1px #6788BE; border-top:none;">
                    <sva:SvaCalendar ID="SvaCalendar1" runat="server" />
                </asp:Panel>
            </radTS:PageView>
            
            <radTS:PageView ID="pageAccess" runat="server">
                <div style="border:solid 1px #6788BE; border-top:none; padding:3px;">
            
                    <div class="wrapper">
                        <div class="header">
                            <img src="http://apps.svamain.loc/common/images/icons/silkicons/lock.png" 
                                alt="Error" style="border:none;height:16px;width:16px;" />
                            You have insufficient access.  
                        </div>
                        
                        <div class="content">
                            <div style="padding-bottom:10px;">    
                                Only the Requestor and Approver can view this FTO Request.
                            </div>
                            <div style="padding-bottom:15px;">    
                                If you feel you've reached this page in error, please
                                <asp:HyperLink ID="hlSubmitBug" runat="server" ForeColor="Blue" 
                                        NavigateUrl="~/ErrorPages/Submit.aspx">
                                    submit a bug report.
                                </asp:HyperLink>
                            </div>
                        </div>
                        
                        <div class="footer">
                            Sorry for the inconvenience!
                        </div>
                    </div>
                </div>
            </radTS:PageView>
            
            <radTS:PageView ID="pageConflict" runat="server">
                <div style="border:solid 1px #6788BE; border-top:none; padding:3px;">
                
                    <div class="wrapper">
                        <div class="header">
                            <img src="http://apps.svamain.loc/common/images/icons/silkicons/error.png" 
                                alt="Warning" style="border:none;height:16px;width:16px;" />
                                Warning  
                        </div>
                        
                        <div class="content">
                            <div style="padding-bottom:10px;">    
                                The following existing request(s) conflict with the request you are making.
                            </div>
                            <div style="padding-bottom:10px;">  
                                <radG:RadGrid ID="rgConflicts" runat="server" Skin="Web20">
                                    <MasterTableView DataKeyNames="FtoRequestId" AutoGenerateColumns="False">
                                        <Columns>
                                            <radG:GridTemplateColumn DataField="FromDate" DataType="System.DateTime" HeaderText="From"
                                                    SortExpression="FromDate" UniqueName="TemplateColumn">
                                                <ItemTemplate>
                                                    <span class="ftoDate"><%#Eval("FromDate", "{0:d}")%> </span><%#Eval("FromDate", "{0:t}")%>
                                                </ItemTemplate>
                                                <ItemStyle Width="110px" />
                                            </radG:GridTemplateColumn>
                                            <radG:GridTemplateColumn DataField="ToDate" DataType="System.DateTime" HeaderText="To"
                                                    SortExpression="ToDate" UniqueName="TemplateColumn1">
                                                <ItemTemplate>
                                                    <span class="ftoDate"><%#Eval("ToDate", "{0:d}")%> </span><%#Eval("ToDate", "{0:t}")%>
                                                </ItemTemplate>
                                                <ItemStyle Width="110px" />
                                            </radG:GridTemplateColumn>
                                            <radG:GridBoundColumn DataField="RequestNotes" Headertext="Notes"></radG:GridBoundColumn>
                                            <radG:GridTemplateColumn DataField="IsApproved" HeaderText="Response" SortExpression="IsApproved" UniqueName="IsApproved">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="Server"
                                                        Text='<%# GetResponse(Eval("IsApproved"), Eval("ResponseNotes")) %>'
                                                        ForeColor='<%# ColorResponse(Eval("IsApproved")) %>'
                                                        Font-Bold='<%# Not Eval("IsApproved") is DBNull.Value %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </radG:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </radG:RadGrid>
                            </div>  
                            <div style="padding-bottom:12px;">
                                You can:
                                <div style="clear:both; padding:8px;">
                                    <div style="float:left; width:100px; text-align:right;">
                                        <asp:Button ID="btnSubmitRequest" runat="server" Text="Submit" 
                                            OnClick="btnSubmitRequest_Click" CssClass="control"
                                            OnClientClick="javascript:return confirm('Are you sure you want to set the displayed request(s) to Denied?');" />
                                        -&nbsp; 
                                    </div>
                                    <div style="float:left; width:300px;">
                                        continue to submit this request and automatically change the conflicting request(s) above to 
                                        '<span style="color:Red;font-weight:bold;">Denied</span>'
                                    </div>
                                </div>
                                <div style="clear:both; padding:8px;">
                                    <div style="float:left; width:100px; text-align:right;">
                                        or  
                                        <asp:Button ID="btnCancelRequest" runat="server" Text="Cancel" 
                                            OnClick="btnCancelRequest_Click" CssClass="control" />
                                         -&nbsp;
                                    </div>
                                    <div style="float:left; width:300px;">
                                        return to the 'New Request' form to edit this request
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </radTS:PageView>
            
        </radTS:RadMultiPage>
        <asp:FormView ID="fvFtoRequest" runat="server" DefaultMode="Insert" datasourceid="odsFtoForm" 
                style="border:solid 1px #6788BE; width:100%; border-top:none; margin-top:-13px;">
            <EditItemTemplate>
                
                <div style="padding:3px; font-size:12px; font-family:Tahoma; color:White; background-color:#92B4E0; 
                        font-weight:bold; border:solid 1px #92B4E0;">
                    Request Details (#<%#Eval("FtoRequestId")%>)                   
                </div>
            
                <div class="row">
                    <div class="left" style="width:150px; text-align:center;">
                    
                        <div style="font-size:20px;padding:8px;">
                            <asp:Label ID="Label2" runat="Server" 
                                Text='<%# GetResponse(Eval("IsApproved"), Eval("ResponseNotes")) %>'
                                ForeColor='<%# ColorResponse(Eval("IsApproved")) %>'
                                Font-Bold='<%# Not Eval("IsApproved") is DBNull.Value %>'>
                            </asp:Label>
                        </div>
                        
                        <asp:Calendar ID="calFtoRange" runat="server" CssClass="ftoCalPreview" 
                                SelectedDayStyle-CssClass="ftoCalPreviewSelected"
                                ShowNextPrevMonth="False" VisibleDate='<%#Eval("FromDate")%>'
                                SelectionMode="None" DayNameFormat="FirstTwoLetters"
                                ShowGridLines="True"
                                OnDayRender="calFtoRange_DayRender">
                            <SelectedDayStyle BackColor="#6788BE" ForeColor="White" />
                        </asp:Calendar>
                        
                        <asp:Panel ID="pnlCancel" runat="server" Visible='<%# Eval("IsApproved") is DBNull.Value or GetResponse(Eval("IsApproved"), Eval("ResponseNotes")) = "Approved" %>'>
                            <br />
                            <br />
                            <asp:LinkButton ID="lbCancelRequest" runat="server" 
                                    OnCommand="lbCancelRequest_Command"
                                    CommandArgument='<%#Eval("FtoRequestId")%>'
                                    OnClientClick="javascript: return confirm('Are you sure you want to cancel this request?')">
                                <img src="App_Themes/Gray/Icons/cancel.png" style="border:none;width:16px;height:16px;" alt="Cancel" />
                                Cancel Request
                            </asp:LinkButton>
                        </asp:Panel>
                    
                    </div>
                    <div class="right" style="border-left:solid 1px #6788BE;">
                        
                        <br />
                    
                        <div class="row">
                            <div class="left">Requestor:</div>
                            <div class="right">
                                <%#Eval("Requestor")%>
                            </div>
                        </div>                        
                        <div class="row">
                            <div class="left">Date of Request:</div>
                            <div class="right">
                                <%#Eval("RequestDate")%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="left">FTO Balance:</div>
                            <div class="right">
                                <%#Eval("FtoBalance", "{0:f2} <span style='color:Gray;'>&nbsp;(May not reflect recent activity)</span>")%>
                            </div>
                        </div>
                             
                        <br />                     
                                            
                        <div class="row">
                            <div class="left">From:</div>
                            <div class="right">
                                <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("FromDate")%>'></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="left">To:</div>
                            <div class="right">
                                <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("ToDate")%>'></asp:Label>
                            </div>
                        </div>   
                        
                        <br />
                        
                        <div class="row">
                            <div class="left">Total Hours:</div>
                            <div class="right">
                                <%#Eval("RequestHours", "{0:f2}")%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="left">Request Type:</div>
                            <div class="right">
                                <%#Eval("RequestType")%>
                            </div>
                        </div>
                        <div class="row">
                            <div class="left">Request Notes:</div>
                            <div class="right">
                                <%#Eval("RequestNotes")%>
                            </div>
                        </div>
                
                        <br />
                        <hr style="color:#6788BE; height:1px;" />
                        <br />
                        
                        <radTS:RadMultiPage ID="rmpApprove" runat="server" 
                                SelectedIndex='<%# Iif(Eval("IsApproved") is DBNull.Value, Iif((Eval("ApproverId") = Session("employeeId") or Session("employeeId") = 146), 1, 0), 0) %>'>
                                
                            <radTS:PageView ID="pageReadOnly" runat="server">
                                <div class="row">
                                    <div class="left">Approver:</div>
                                    <div class="right">
                                        <%# Eval("Approver") %>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="left">Response:</div>
                                    <div class="right">
                                        <asp:Label ID="Label1" runat="Server" 
                                            Text='<%# GetResponse(Eval("IsApproved"), Eval("ResponseNotes")) %>'
                                            ForeColor='<%# ColorResponse(Eval("IsApproved")) %>'
                                            Font-Bold='<%# Not Eval("IsApproved") is DBNull.Value %>'>
                                        </asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="left">Date of Response:</div>
                                    <div class="right">
                                        <%#Eval("ResponseDate")%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="left">Response Notes:</div>
                                    <div class="right">
                                        <%# Eval("ResponseNotes") %>
                                    </div>
                                </div>
                            </radTS:PageView>
                            <radTS:PageView ID="pageEdit" runat="server">
                            
                                <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Bind("FtoRequestId")%>' />
                            
                                <div class="row">
                                    <div class="left">
                                        <asp:RequiredFieldValidator ID="rfvResponse" 
                                            runat="server" ErrorMessage="*** " Display="Dynamic"
                                            ControlToValidate="ApproveRadios1">
                                        </asp:RequiredFieldValidator>  
                                        Response:
                                    </div>
                                    <div class="right">
                                        <sva:ApproveRadios ID="ApproveRadios1" runat="server" SelectedValue='<%# Bind("IsApproved") %>' />
                                    </div>
                                </div>
                                
                                <sva:CurrentDateTime ID="cdtCurrentDate" runat="server" SelectedValue='<%# Bind("ResponseDate") %>' />
                                
                                <div class="row">
                                    <div class="left">Response Notes:</div>
                                    <div class="right">
                                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" 
                                            Rows="4" Columns="40" CssClass="control" 
                                            Text='<%# Bind("ResponseNotes") %>'>
                                        </asp:TextBox>
                                    </div>
                                </div>
                                
                                <div class="row">
                                    <div class="left"></div>
                                    <div class="right">
                                        <asp:Button ID="btnRespond" runat="server" 
                                            Text="Respond To Request" CausesValidation="True" 
                                            CssClass="control" CommandName="Update" />
                                    </div>
                                </div>                
                                                            
                            </radTS:PageView>
                        </radTS:RadMultiPage>
                        
                           
                        <br />                     
                    
                    </div>
                </div>
                
            </EditItemTemplate>
            <InsertItemTemplate>
            
                <div style="padding:3px; text-align:center; padding-top:7px;">
                    <%-- 1/11/08 - removed to integrate IT Consulting into this application--%>
                    
                    <%--<asp:HyperLink ID="lnkConsultingFTORequest" runat="server" Font-Bold="True" ForeColor="Blue" Target="_blank" 
                            NavigateUrl="http://svaintranet/Resources/Common%20Document%20Library/Forms/AllItems.aspx?View=%7bE05BBD39%2dB87B%2d4FBA%2d9DC3%2d4279C080D4C1%7d&FilterField1=SPSDescription&FilterValue1=Flexible%20Time%20Off%20Request%20Form%20%2D%20IT%20Consulting">
                        IT Consulting Form
                    </asp:HyperLink>
                    &nbsp;|&nbsp;--%>
                    <asp:HyperLink ID="lnkAccountingForm" runat="server" Font-Bold="True" ForeColor="Blue" Target="_blank" 
                            NavigateUrl="http://svaintranet/Resources/Common%20Documents%20Library/FTO%20Request%20Procedure%20for%20Madison%20Accounting%20Staff.pdf">
                        <img src="App_Themes/SVA/Images/page_white_acrobat.png" style="width:16px;height:16px;border:none;" alt="Download PDF" />
                        Accounting Staff Form
                    </asp:HyperLink>
                </div>
                
                <hr style="color:#6788BE; height:1px;" />                     
                <br />
                
                <sva:CurrentDateTime ID="cdtCurrentDate" runat="server" SelectedValue='<%# Bind("RequestDate") %>' />
                
                <div class="row">
                    <div class="left">Requestor:</div>
                    <div class="right">
                        <sva:CurrentUser ID="cuRequestor" runat="server" SelectedValue='<%# Bind("RequestorId") %>' />
                    </div>
                </div>                        
                <div class="row">
                    <div class="left">Approver:</div>
                    <div class="right">
                        <sva:Approver ID="approver1" runat="server" SelectedValue='<%# Bind("ApproverId") %>' />
                    </div>
                </div>                        
                <div class="row">
                    <div class="left">FTO Balance:</div>
                    <div class="right">
                        <sva:FtoBalance ID="ftoBalance1" runat="server" SelectedValue='<%# Bind("FtoBalance") %>' />
                        <span style="color:Gray;">
                            &nbsp;(May not reflect recent activity)
                        </span>
                    </div>
                </div>
                
                <br />
                <hr style="color:#6788BE; height:1px;" />                     
                <br />
                
                <div class="row">
                    <div class="left">
                         <asp:RequiredFieldValidator ID="rfvFromDate" ControlToValidate="bdtFrom"
                            runat="server" ErrorMessage="***" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        
                        From:
                    </div>
                    <div class="right">
                        
                        <sva:BindDateTime ID="bdtFrom" runat="server" SelectedValue='<%# Bind("FromDate") %>' defaultMode="FromDate" />
                        
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

                        <sva:BindDateTime ID="bdtTo" runat="server" SelectedValue='<%# Bind("ToDate") %>' defaultMode="ToDate" />
                        
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
                        <br />
                    </div>
                </div>
                
                
                <div class="row">
                    <div class="left">
                        <asp:RequiredFieldValidator ID="rfvRequestHours" ControlToValidate="rntRequestHours"
                            runat="server" ErrorMessage="***" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                        
                        Total Hours:
                    </div>
                    <div class="right">
                        <radI:RadNumericTextBox ID="rntRequestHours" 
                            runat="server" CssClass="control" Width="40px" 
                            Value='<%# Bind("RequestHours") %>' MinValue="0">
                        </radI:RadNumericTextBox>
                        
                        <span style="color:Gray;">
                            &nbsp;&nbsp;(1 day = 8.00, 15 mins = 0.25)
                        </span>
                    </div>
                </div>
                        
                <div class="row">
                    <div class="left">Count Request As:</div>
                    <div class="right">
                        <asp:DropDownList ID="ddlRequestType" runat="server" 
                            DataSourceID="odsRequestType" DataTextField="RequestType" 
                            DataValueField="RequestTypeId"
                            selectedValue='<%# Bind("RequestTypeId") %>' 
                            CssClass="control">
                        </asp:DropDownList>
                        <asp:ObjectDataSource ID="odsRequestType" runat="server" TypeName="FtoRequestBll" 
                            SelectMethod="GetFtoRequestTypes">
                        </asp:ObjectDataSource>
                    </div>
                </div>
                
                <br />
                
                <div class="row">
                    <div class="left">Notes:</div>
                    <div class="right">
                        <asp:TextBox ID="txtNotes" runat="server" 
                            TextMode="MultiLine" Rows="4" Columns="40" 
                            CssClass="control" Text='<%# Bind("RequestNotes") %>'>
                        </asp:TextBox>
                    </div> 
                </div>
                
                <br />
                
                <div class="row">
                    <div class="left"></div>
                    <div class="right">
                        <asp:Button ID="btnEmail" runat="server"
                            Text="Send Request" CssClass="control"
                            CommandName="Email" CausesValidation="True" />
                        or 
                        <asp:Button ID="btnPrint" runat="server" 
                            Text="Print this Form" CssClass="control" 
                            CommandName="Print" CausesValidation="True" />
                    </div> 
                </div>
                <br />
                
            </InsertItemTemplate>
            <EmptyDataTemplate>
                <div style="border:solid 1px #6788BE; border-top:none; padding:3px;">
            
                    <div class="wrapper">
                        <div class="header">
                            <img src="http://apps.svamain.loc/common/images/icons/silkicons/error.png" 
                                alt="Error" style="border:none;height:16px;width:16px;" />
                            Not Found  
                        </div>
                        
                        <div class="content">
                            <div style="padding-bottom:10px;">    
                                The specified FTO request could not be found.  If you are the requestor or approver, please use the navigation above to try to find it. 
                            </div>
                            <div style="padding-bottom:15px;">
                                If you feel you've reached this page in error, please
                                <asp:HyperLink ID="hlSubmitBug" runat="server" ForeColor="Blue" 
                                        NavigateUrl="~/ErrorPages/Submit.aspx">
                                    submit a bug report.
                                </asp:HyperLink>
                            </div>
                        </div>
                        
                        <div class="footer">
                            Sorry for the inconvenience!
                        </div>
                    </div>
                </div>
            </EmptyDataTemplate>
        </asp:FormView>
        
        <asp:ObjectDataSource ID="odsFtoForm" runat="server" 
                InsertMethod="InsertFtoRequest" 
                SelectMethod="GetFtoRequestByRequestId" TypeName="FtoRequestBLL" 
                UpdateMethod="RespondToFtoRequest">
            <UpdateParameters>
                <asp:Parameter Name="ftoRequestId" Type="Int32" />
                <asp:Parameter Name="responseDate" Type="DateTime" />
                <asp:Parameter Name="isApproved" Type="Boolean" />
                <asp:Parameter Name="responseNotes" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="requestId" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="requestorId" Type="Int32" />
                <asp:Parameter Name="requestDate" Type="DateTime" />
                <asp:Parameter Name="fromDate" Type="DateTime" />
                <asp:Parameter Name="toDate" Type="DateTime" />
                <asp:Parameter Name="requestHours" Type="Decimal" />
                <asp:Parameter Name="requestTypeId" Type="Int32" />
                <asp:Parameter Name="ftoBalance" Type="Decimal" />
                <asp:Parameter Name="approverId" Type="Int32" />
                <asp:Parameter Name="requestNotes" Type="String" />
                <asp:Parameter Name="isEmail" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        
        <asp:Panel ID="pnlManualEntry" runat="server" Visible="false" style="padding:3px;">
            <br />
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ManualEntry/Default.aspx">
                <img src="App_Themes/Gray/Icons/report_add.png" style="border:none;width:16px;height:16px;" alt="Manual Time Entry" />
                Manual Time Entry
            </asp:HyperLink>
        </asp:Panel>
        
        <div style="padding:3px;">
            <br />
            <uc1:SubmitBugLink ID="SubmitBugLink1" runat="server" />
        </div>
        
    </div>
    
    <uc2:PreLoadActionStopper ID="PreLoadActionStopper1" runat="server" />
    </form>
</body>
</html>
