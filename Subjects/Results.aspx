<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="Quizify.Subjects.Results" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quiz Results</title>
    <style>
        .results-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #f5f5f5;
            text-align: center;
        }

        .results-header {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
        }

        .results-label {
            font-size: 18px;
            margin: 10px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="results-container">
            <div class="results-header">Your Quiz Results</div>
            <asp:Label ID="CorrectAnswersLabel" runat="server" CssClass="results-label" Text=""></asp:Label><br />
            <asp:Label ID="IncorrectAnswersLabel" runat="server" CssClass="results-label" Text=""></asp:Label><br />
            <asp:Label ID="SkippedAnswersLabel" runat="server" CssClass="results-label" Text=""></asp:Label><br />
            <asp:Label ID="TotalQuestionsLabel" runat="server" CssClass="results-label" Text=""></asp:Label><br />
            <asp:Label ID="ScorePercentageLabel" runat="server" CssClass="results-label" Text=""></asp:Label><br />
        </div>
    </form>
</body>
</html>
