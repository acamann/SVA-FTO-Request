<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ApproveRadios.ascx.vb" Inherits="UserControls_ApproveRadios" %>

<div style="color:Green; font-weight:bold;">
    <asp:RadioButton ID="rbApproved" runat="server" GroupName="Decision" />Approved
</div>
<div style="color:Red; font-weight:bold;">
    <asp:RadioButton ID="rbDenied" runat="server" GroupName="Decision" />Denied 
</div>

