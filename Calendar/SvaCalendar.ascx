<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SvaCalendar.ascx.vb" Inherits="SvaCalendar" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>

<!-- Month -->
<asp:Panel ID="pnlFilter" runat="server" style="width:100%; clear:both;">
    <div style="float:left; padding:6px;">
        Employees: <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="True" CssClass="control" DataSourceID="sqlDepartments" DataTextField="Department" DataValueField="DepartmentId">
            <asp:ListItem Text="All" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Assigned To Me" Value='Me'></asp:ListItem>
        </asp:DropDownList>
        <asp:SqlDataSource ID="sqlDepartments" runat="server" ConnectionString="<%$ ConnectionStrings:SVAEmployeeConnectionString %>" SelectCommand="Department_Select" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    </div>
    <asp:Panel ID="pnlResponseFilter" runat="server" style="float:left; padding:6px;" Visible="False">
        Response: <asp:DropDownList ID="ddlIsApproved" runat="server" AutoPostBack="True" CssClass="control">
            <asp:ListItem Text="All" Value=""></asp:ListItem>
            <asp:ListItem Text="Approved" Value="True" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Denied" Value="False"></asp:ListItem>
        </asp:DropDownList>
   </asp:Panel>
</asp:Panel>

<div style="padding:6px;color:#6A6A6B;">
    Click on a day to display who is scheduled for FTO on that day.  The number in parentheses is the number of employees scheduled for FTO for that day.
</div>

<div style="clear:both; width:100%;font-family:Tahoma; padding:2px;">
    
    <div style="float:left; width:auto; padding:4px;">
        <asp:Calendar ID="calFTO" runat="server" 
            CssClass="calendar" SelectedDayStyle-BackColor="#AA0000">
        </asp:Calendar>

    </div>
    <div style="float:left; padding:4px; width:auto; width:250px;">
        <div class="dateHeader"><asp:Label ID="lblCurrentDay" runat="server"></asp:Label></div>
        
        <radG:RadGrid ID="gvRequests" runat="server" AutoGenerateColumns="False" 
                GridLines="None" Skin="Office2007" BorderStyle="None" >
            <MasterTableView NoMasterRecordsText="There are no matching FTO requests." DataKeyNames="RequestId">
                <SortExpressions>
                    <radG:GridSortExpression FieldName="Requestor" SortOrder="Ascending" />
                </SortExpressions>
                <AlternatingItemStyle BackColor="#FFFFCC"  />
                <ItemStyle BackColor="White" />
                <Columns>
                    <radG:GridTemplateColumn DataField="IsApproved" UniqueName="IsApproved" HeaderText="" ItemStyle-Width="16px" Visible="False">
                        <ItemTemplate>
                            <img runat="server" alt="Approved" src='<%# GetIsApprovedImage(Eval("IsApproved")) %>'
                                style="width:16px; height:16px;" />
                        </ItemTemplate>
                    </radG:GridTemplateColumn>
                    <radG:GridTemplateColumn DataField="Requestor" UniqueName="Requestor" HeaderText="Name">
                        <ItemTemplate>
                        
                            <%#Eval("Requestor")%>
                            <asp:Label ID="lblRequestorId" runat="server" Text='<%#Eval("RequestorId", " (#{0})")%>' Visible='<%# Eval("Requestor").equals("Unknown") %>'></asp:Label>
                            
                        </ItemTemplate>
                    </radG:GridTemplateColumn>
                    <radG:GridBoundColumn DataField="From" UniqueName="From" HeaderText="From" ItemStyle-Width="50px"></radG:GridBoundColumn>
                    <radG:GridBoundColumn DataField="To" UniqueName="To" HeaderText="To" ItemStyle-Width="50px"></radG:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </radG:RadGrid>
        
    </div>
</div>



