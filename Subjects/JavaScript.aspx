<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JavaScript.aspx.cs" Inherits="Quizify.Subjects.JavaScript" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>JavaScript Quiz</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="quiz-container">
            <asp:Repeater ID="QuestionsRepeater" runat="server">
                <ItemTemplate>
                    <div class="question-container">
                        <div class="question-number">
                            <%# Container.ItemIndex + 1 %>.)</div>
                        <div class="question-text">
                            <%# Eval("QuestionText") %>
                        </div>
                        <div class="options">
                            <asp:Literal ID="OptionsLiteral" runat="server" Text='<%# Eval("Options") %>'></asp:Literal>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <div class="navigation-buttons">
                <asp:Button ID="PrevButton" runat="server" Text="Prev" OnClick="PrevButton_Click" />
                <asp:Button ID="NextButton" runat="server" Text="Next" OnClick="NextButton_Click" />
            </div>
        </div>
    </form>
</body>
</html>
