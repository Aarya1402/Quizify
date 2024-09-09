<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subject.aspx.cs" Inherits="Quizify.Subjects.Subject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quizify</title>
    <style>
        .question-container {
            margin: 20px;
        }

        .buttons-container {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="question-container">
            <asp:Label ID="QuestionLabel" runat="server" Text="Question will appear here"></asp:Label>
            <br />
            <asp:RadioButtonList ID="OptionsList" runat="server"></asp:RadioButtonList>
            <br />
            <div class="buttons-container">
                <asp:Button ID="PrevButton" runat="server" Text="Prev" OnClick="PrevButton_Click" />
                <asp:Button ID="NextButton" runat="server" Text="Next" OnClick="NextButton_Click" />
                <asp:Button ID="FinishButton" runat="server" Text="Finish" OnClick="FinishButton_Click" />
            </div>
        </div>
    </form>
</body>
</html>
