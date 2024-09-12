<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Subject.aspx.cs" Inherits="Quizify.Subjects.Subject" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quizify</title>
    <style>
        .option-box {
            margin: 5px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            cursor: pointer;
            background-color: #f9f9f9;
        }

        .option-box.selected {
            background-color: #cce5ff;
            border-color: #004085;
            color: #004085; 
        }

        .question-container {
            margin: 20px;
        }

        .buttons-container {
            margin-top: 20px;
        }

        .timer {
            position: absolute;
            right: 20px;
            top: 20px;
            font-size: 20px;
            font-weight: bold;
            color: #004085;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="question-container">
            <asp:HiddenField ID="HiddenFieldRemainingTime" runat="server" />
        <div class="timer">
            Time Remaining: <span id="timerDisplay"></span>
        </div>
            <asp:Label ID="QuestionLabel" runat="server" Text="Question will appear here"></asp:Label>
            <br />
            <asp:Panel ID="OptionsPanel" runat="server"></asp:Panel>
            <br />
            <asp:HiddenField ID="HiddenFieldAnswers" runat="server" />
            <div class="buttons-container">
                <asp:Button ID="PrevButton" runat="server" Text="Prev" OnClick="PrevButton_Click" />
                <asp:Button ID="NextButton" runat="server" Text="Next" OnClick="NextButton_Click" />
                <asp:Button ID="FinishButton" runat="server" Text="Finish" OnClick="FinishButton_Click" />
            </div>
        </div>

        <script type="text/javascript">
            function selectOption(optionDiv) {
                var questionId = optionDiv.getAttribute("data-question-id");
                var selectedOptionId = optionDiv.getAttribute("data-option-id");

                var optionDivs = document.querySelectorAll(".option-box[data-question-id='" + questionId + "']");
                optionDivs.forEach(function (div) {
                    div.classList.remove("selected");
                });

                optionDiv.classList.add("selected");

                var hiddenField = document.getElementById("HiddenFieldAnswers");
                var answers = hiddenField.value ? JSON.parse(hiddenField.value) : {};
                answers[questionId] = selectedOptionId;
                hiddenField.value = JSON.stringify(answers);
            }

            window.onload = function () {
                var display = document.getElementById('timerDisplay');
                startTimer(remainingTime, display);

                var hiddenField = document.getElementById("HiddenFieldAnswers");
                if (hiddenField && hiddenField.value) {
                    var answers = JSON.parse(hiddenField.value);
                    for (var questionId in answers) {
                        var selectedOptionId = answers[questionId];
                        var optionDiv = document.querySelector(".option-box[data-question-id='" + questionId + "'][data-option-id='" + selectedOptionId + "']");
                        if (optionDiv) {
                            optionDiv.classList.add("selected");
                        }
                    }
                }

                
            };

            var remainingTime = parseInt(document.getElementById('<%= HiddenFieldRemainingTime.ClientID %>').value);

            function startTimer(duration, display) {
                var timer = duration, minutes, seconds;
                var timerInterval = setInterval(function () {
                    minutes = parseInt(timer / 60, 10);
                    seconds = parseInt(timer % 60, 10);

                    minutes = minutes < 10 ? "0" + minutes : minutes;
                    seconds = seconds < 10 ? "0" + seconds : seconds;

                    display.textContent = minutes + ":" + seconds;

                    if (--timer < 0) {
                        clearInterval(timerInterval);
                        window.location.href = 'Results.aspx';
                    }
                }, 1000);
            }
        </script>
    </form>
</body>
</html>
