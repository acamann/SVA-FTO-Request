<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BindDateTime.ascx.vb" Inherits="UserControls_BindDateTime" %>
<%@ Register Assembly="RadCalendar.Net2" Namespace="Telerik.WebControls" TagPrefix="radCln" %>

<asp:DropDownList ID="ddlHour" runat="server" CssClass="control">
    <asp:ListItem>1</asp:ListItem>
    <asp:ListItem>2</asp:ListItem>
    <asp:ListItem>3</asp:ListItem>
    <asp:ListItem>4</asp:ListItem>
    <asp:ListItem>5</asp:ListItem>
    <asp:ListItem>6</asp:ListItem>
    <asp:ListItem>7</asp:ListItem>
    <asp:ListItem>8</asp:ListItem>
    <asp:ListItem>9</asp:ListItem>
    <asp:ListItem>10</asp:ListItem>
    <asp:ListItem>11</asp:ListItem>
    <asp:ListItem>12</asp:ListItem>
</asp:DropDownList>
<asp:DropDownList ID="ddlMin" runat="server" CssClass="control">
    <asp:ListItem>00</asp:ListItem>
    <asp:ListItem>15</asp:ListItem>
    <asp:ListItem>30</asp:ListItem>
    <asp:ListItem>45</asp:ListItem>
</asp:DropDownList>
<asp:DropDownList ID="ddlAMPM" runat="server" CssClass="control">
    <asp:ListItem>AM</asp:ListItem>
    <asp:ListItem>PM</asp:ListItem>
</asp:DropDownList>
&nbsp; on &nbsp;
<radCln:RadDatePicker ID="rdpDate" runat="server" Width="100px">
    <Calendar Skin="WebBlue" runat="server" ShowRowHeaders="False"></Calendar>
    <DateInput runat="server" CssClass="control"></DateInput>
</radCln:RadDatePicker>   