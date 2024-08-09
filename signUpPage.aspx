<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signUpPage.aspx.cs" Inherits="Quizify.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 26px;
        }
        .auto-style3 {
            width: 252px;
        }
        .auto-style4 {
            height: 26px;
            width: 252px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbUsername" runat="server" AutoPostBack="True" CausesValidation="True" ToolTip="Enter username"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" Display="Dynamic" ErrorMessage="Username is required" ControlToValidate="tbUsername"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbEmail" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Email" ToolTip="Enter email"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" Display="Dynamic" ErrorMessage="Email is required" ControlToValidate="tbEmail"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblPhoneNo" runat="server" Text="Mobile No: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbMobileNo" runat="server" CausesValidation="True" MaxLength="10" TextMode="Phone" ToolTip="Enter mobile number"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorMobileNo" runat="server" ControlToValidate="tbMobileNo" Display="Dynamic" ErrorMessage="Mobile number is required"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidatorMobileNo" runat="server" ControlToValidate="tbMobileNo" Display="Dynamic" ErrorMessage="Mobile number must be 10 digits" OnServerValidate="ValidateMobileNumber"></asp:CustomValidator>
                   </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblRole" runat="server" Text="Role:"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblRole" runat="server" AutoPostBack="True" CausesValidation="True" RepeatDirection="Horizontal" ToolTip="Select an option">
                            <asp:ListItem Selected="True">Student</asp:ListItem>
                            <asp:ListItem>Admin</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                    </td>
                    <td class="auto-style2">
                        <asp:TextBox ID="tbPassword" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Password" ToolTip="Enter password"></asp:TextBox>
                    </td>
                    <td class="auto-style2">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ControlToValidate="tbPassword" Display="Dynamic" ErrorMessage="Password is required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbConfirmPassword" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Password" ToolTip="Re-enter password "></asp:TextBox>
                    </td>
                    <td>  
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorConfirmPassword" runat="server" ControlToValidate="tbConfirmPassword" Display="Dynamic" ErrorMessage="Confirm password is required"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidatorPassword" runat="server" ControlToValidate="tbConfirmPassword" ControlToCompare="tbPassword" Display="Dynamic" ErrorMessage="Passwords do not match"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4"></td>
                    <td class="auto-style2"></td>
                    <td class="auto-style2"></td>
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td>
                        <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/signUpPage.aspx" ToolTip="Login">Already registered? Login</asp:HyperLink>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
