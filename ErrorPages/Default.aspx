<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_ErrorPages_Default" StylesheetTheme="ErrorPages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>An unexpected error has occurred</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="header">
            <img src="http://apps.svamain.loc/common/images/icons/silkicons/error.png" alt="Error" />
            An unexpected error has occurred.
        </div>
        
        <div class="content">
            <div style="padding-bottom:15px;">
                The error has been logged and e-mailed to the application administrator. 
            </div>
            
            <div style="padding-bottom:10px;">    
                If you would like to provide more information about this error, please
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
    </form>
</body>
</html>
