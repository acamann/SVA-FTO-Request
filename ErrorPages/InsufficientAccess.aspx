<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InsufficientAccess.aspx.vb" Inherits="InsufficientAccess" StylesheetTheme="ErrorPages"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="header">
            <img src="http://apps.svamain.loc/common/images/icons/silkicons/lock.png" alt="Error" />
            You have Insufficient Access to view this page.
        </div>
        
        <div class="content">
            <div style="padding-bottom:15px;">    
                If you feel you've reached this page in error, please
                <asp:HyperLink ID="hlSubmitBug" runat="server" ForeColor="Blue" 
                        NavigateUrl="~/ErrorPages/Submit.aspx">
                    submit a bug report.
                </asp:HyperLink>
            </div>
            
            <div style="padding-bottom:10px;">    
                ...or use your browser's back button to return to the last page.
            </div>
        </div>
        
        <div class="footer">
            Sorry for the inconvenience!
        </div>
    </div>
    </form>
</body>
</html>
