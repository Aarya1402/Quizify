using System;
using System.Web.UI;

namespace Quizify.Subjects
{
    public partial class Results : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayResults();
            }
        }

        private void DisplayResults()
        {
            int correctAnswers = (int)(Session["CorrectAnswers"] ?? 0);
            int incorrectAnswers = (int)(Session["IncorrectAnswers"] ?? 0);
            int skippedAnswers = (int)(Session["SkippedAnswers"] ?? 0);

            CorrectAnswersLabel.Text = $"Correct Answers: {correctAnswers}";
            IncorrectAnswersLabel.Text = $"Incorrect Answers: {incorrectAnswers}";
            SkippedAnswersLabel.Text = $"Skipped Answers: {skippedAnswers}";

            int totalQuestions = correctAnswers + incorrectAnswers + skippedAnswers;
            TotalQuestionsLabel.Text = $"Total Questions: {totalQuestions}";

            double scorePercentage = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0;
            ScorePercentageLabel.Text = $"Score Percentage: {scorePercentage:F2}%";
        }
    }
}
