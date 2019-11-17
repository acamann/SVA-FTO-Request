<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Submit.aspx.vb" Inherits="Submit" StylesheetTheme="ErrorPages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Submit a Bug Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="wrapper">
        <div class="header">
            <img src="http://apps.svamain.loc/common/images/icons/silkicons/bug_add.png" style="background-color:Transparent;" alt="Bug Report" />
            Submit a Bug Report
        </div>
    
        <asp:MultiView ID="mvBug" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewSubmit" runat="server">
                <div class="content">
                    
                    <div class="row">
                        <div class="label">
                            Application:
                        </div>
                        <div class="content">
                            <asp:Label ID="lblApplication" runat="server" Text="Unknown Application"></asp:Label>
                        </div>
                    </div>                       
                   
                    <div class="row">
                        <div class="label">
                            <asp:RequiredFieldValidator 
                                ID="rfvDescription" runat="server" 
                                ErrorMessage="**"
                                ControlToValidate="txtDescription"
                                ValidationGroup="ErrorTracking">
                            </asp:RequiredFieldValidator>
                            Description:</div>
                        <div class="content">
                            <asp:TextBox ID="txtDescription" runat="server" 
                                TextMode="multiline" Columns="35" Rows="6" 
                                Text="">
                            </asp:TextBox>
                            
                            <br />
                            <span style="font-size:11px;">(Please be as specific as possible)</span>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="label"></div>
                        <div class="content">
                            <asp:Button ID="btnReportBug" runat="server" CausesValidation="True" ValidationGroup="ErrorTracking" Text="Submit" />
                        </div>
                    </div>
                    
                </div>
            </asp:View>
            <asp:View ID="viewSuccess" runat="server">
                <div class="content">
                    Thank You, your report has been submitted.
                </div>
            </asp:View>
            <asp:View ID="viewFailure" runat="server">
                <div class="content">
                    There was an error submitting your report.  Please contact the help desk (ext# 2222).
                </div>
            </asp:View>
        </asp:MultiView> 
    
    </div>
    </form>
</body>
</html>
