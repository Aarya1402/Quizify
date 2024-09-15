<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditQuestion.aspx.cs" Inherits="Quizify.Subjects.EditQuestion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Questions</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Edit Questions</h1>

        <asp:GridView ID="QuestionsGridView" runat="server" AutoGenerateColumns="False" 
            OnRowEditing="QuestionsGridView_RowEditing"
            OnRowUpdating="QuestionsGridView_RowUpdating"
            OnRowCancelingEdit="QuestionsGridView_RowCancelingEdit"
            OnRowDeleting="QuestionsGridView_RowDeleting"
            DataKeyNames="Id" OnSelectedIndexChanged="QuestionsGridView_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="Question" HeaderText="Question" />
                <asp:BoundField DataField="Correct_ans" HeaderText="Correct Answer" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ShowSelectButton="true"/>
            </Columns>
        </asp:GridView>

        <p>&nbsp;</p>
        <p>
           <asp:GridView ID="GridViewOptions" runat="server" AutoGenerateColumns="False"
               OnRowEditing="GridViewOptions_RowEditing"
OnRowUpdating="GridViewOptions_RowUpdating"
OnRowCancelingEdit="GridViewOptions_RowCancelingEdit"
OnRowDeleting="GridViewOptions_RowDeleting"
DataKeyNames="Id">
    <Columns>
        <asp:BoundField DataField="options" HeaderText="Options" />
        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
</asp:GridView>

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        </p>
        <p>
            <asp:Label ID="lbloption" runat="server" Text="Option"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="tbOption" runat="server"></asp:TextBox>

        </p>
        <p>
            <asp:Button ID="btnAddoption" runat="server" OnClick="btnAddoption_Click" Text="Add Option" />

        </p>
        <p>
            &nbsp;</p>
        <p>&nbsp;</p>
        <asp:Label ID="lblQuestion" runat="server" Text="Question:"></asp:Label>
        <asp:TextBox ID="tbQuestion" runat="server"></asp:TextBox>
        <br /><br />

        <asp:Label ID="lblCorrectAns" runat="server" Text="Correct Answer:"></asp:Label>
        <asp:TextBox ID="tbCorrectAns" runat="server" TextMode="singleLine"></asp:TextBox>
        <br /><br />

        <asp:Button ID="btnAddQuestion" runat="server" OnClick="btnAddQuestion_Click" Text="Add Question" ValidationGroup="AddQuestionGroup" />
        <br />

        <asp:ValidationSummary ID="vsEditQuestion" runat="server" ValidationGroup="AddQuestionGroup" />
        <asp:RequiredFieldValidator ID="rfvQuestion" runat="server" ControlToValidate="tbQuestion" Display="None" ErrorMessage="Question is required" ValidationGroup="AddQuestionGroup"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="rfvCorrectAns" runat="server" ControlToValidate="tbCorrectAns" Display="None" ErrorMessage="Correct answer is required" ValidationGroup="AddQuestionGroup"></asp:RequiredFieldValidator>
    </form>
</body>
</html>
