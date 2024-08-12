<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loginPage.aspx.cs" Inherits="Quizify.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 208px;
        }
        .auto-style3 {
            width: 229px;
        }
        .auto-style4 {
            width: 208px;
            height: 26px;
        }
        .auto-style5 {
            width: 229px;
            height: 26px;
        }
        .auto-style6 {
            height: 26px;
        }
    </style>
   
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblUsername" runat="server" EnableTheming="True" Text="Username: "></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="tbUsername" runat="server" AutoPostBack="True" CausesValidation="True" ToolTip="Enter username"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ControlToValidate="tbUsername" Display="None" ErrorMessage="Username required"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="redundancy_error" ControlToValidate="tbUsername" Display="Dynamic" ErrorMessage="Enter Valid Name" OnServerValidate="redundancy_error"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="tbPassword" runat="server" CausesValidation="True" OnTextChanged="tbPassword_TextChanged" TextMode="Password" ToolTip="Enter password"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ControlToValidate="tbPassword" Display="None" ErrorMessage="Password is required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">
                        <asp:Button ID="btnLogin" runat="server" Height="29px" Text="Login" ToolTip="Login" OnClick="btnLogin_Click" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">
                        <asp:HyperLink ID="hlSignUp" runat="server" NavigateUrl="~/signUpPage.aspx" ToolTip="Sign up">New User? Click to signup</asp:HyperLink>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:ValidationSummary ID="ValidationSummaryLoginPage" runat="server" HeaderText="Errors:" />
                    </td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4"></td>
                    <td class="auto-style5"></td>
                    <td class="auto-style6"></td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
