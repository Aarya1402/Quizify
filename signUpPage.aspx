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
        .auto-style7 {
            width: 225px;
        }
        .auto-style8 {
            height: 26px;
            width: 225px;
        }
        .auto-style9 {
            height: 34px;
            width: 252px;
        }
        .auto-style10 {
            height: 34px;
            width: 225px;
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
                    <td class="auto-style7">
                        <asp:TextBox ID="tbUsername" runat="server" AutoPostBack="True" CausesValidation="True" ToolTip="Enter username"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" Display="None" ErrorMessage="Username is required" ControlToValidate="tbUsername"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cv1" runat="server" Display="Dynamic" ErrorMessage="User Already Exists!!" OnServerValidate="redundancy_error"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                    </td>
                    <td class="auto-style7">
                        <asp:TextBox ID="tbEmail" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Email" ToolTip="Enter email"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" Display="None" ErrorMessage="Email is required" ControlToValidate="tbEmail"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ControlToValidate="tbEmail" Display="Dynamic" ErrorMessage="Invalid email format"    ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,4}$"></asp:RegularExpressionValidator>

                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblPhoneNo" runat="server" Text="Mobile No: "></asp:Label>
                    </td>
                    <td class="auto-style7">
                        <asp:TextBox ID="tbMobileNo" runat="server" CausesValidation="True" MaxLength="10" TextMode="Phone" ToolTip="Enter mobile number"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorMobileNo" runat="server" ControlToValidate="tbMobileNo" Display="None" ErrorMessage="Mobile number is required"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidatorMobileNo" runat="server" ControlToValidate="tbMobileNo" Display="Dynamic" ErrorMessage="Mobile number must be 10 digits" OnServerValidate="ValidateMobileNumber"></asp:CustomValidator>
                        <asp:Label runat="server" ID="lbalert" Text="I am OK!" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblRole" runat="server" Text="Role:"></asp:Label>
                    </td>
                    <td class="auto-style7">
                        <asp:RadioButtonList ID="rblRole" runat="server" AutoPostBack="True" CausesValidation="True" RepeatDirection="Horizontal" ToolTip="Select an option">
                            <asp:ListItem Selected="True" Value="0">Student</asp:ListItem>
                            <asp:ListItem Value="1">Admin</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style9">
                        <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                    </td>
                    <td class="auto-style10">
                        <asp:TextBox ID="tbPassword" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Password" ToolTip="Enter password"></asp:TextBox>
                    </td>
                    <!--<td class="auto-style2">
                        <asp:RequiredFieldValidator ID="res_pass" runat="server" ControlToValidate="tbPassword" Display="Dynamic" ErrorMessage="Enter Password"></asp:RequiredFieldValidator>
                    </td>-->
                </tr>
                <tr>
                    <!--<td class="auto-style5">
                        <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:"></asp:Label>
                    </td>
                    <td class="auto-style9">
                        <asp:TextBox ID="tbConfirmPassword" runat="server" AutoPostBack="True" CausesValidation="True" TextMode="Password" ToolTip="Re-enter password "></asp:TextBox>
                    </td>
                    <td class="auto-style6">  
                        <asp:RequiredFieldValidator ID="req_con_pass" runat="server" ControlToValidate="tbConfirmPassword" Display="Dynamic" ErrorMessage="Re-enter Password"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmp_valid_pass" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword" ErrorMessage="Password doesn't match"></asp:CompareValidator>
                    </td>-->
                </tr>
                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td class="auto-style7">
                        <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4"></td>
                    <td class="auto-style8">
                        <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/loginPage.aspx" ToolTip="Login">Already registered? Login</asp:HyperLink>

                    </td>
                    <td class="auto-style2"></td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:ValidationSummary ID="ValidationSummarysignUp" runat="server" HeaderText="You might be doing this wrong..." />
                    </td>
                    <td class="auto-style8">
                    </td>
                    <td class="auto-style2"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
