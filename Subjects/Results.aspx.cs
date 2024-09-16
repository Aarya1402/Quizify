using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Configuration;
using System.Web.UI;

namespace Quizify.Subjects
{
    public partial class Results : Page
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] == null)
            {
                Response.Redirect("~/loginPage.aspx");
                return;
            }
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;

            if (!IsPostBack)
            {
                DisplayResults();
                SaveResultsToDatabase();
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
            Session["ScorePercentage"] = scorePercentage;
        }

        private void SaveResultsToDatabase()
        {
            int userId = (int)Session["UserId"]; 
            double scorePercentage = (double)Session["ScorePercentage"];
            
            string quizId = Guid.NewGuid().ToString(); 

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "INSERT INTO [Score] (quiz_id, user_id, score) VALUES (@QuizId, @UserId, @Score)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@QuizId", quizId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Score", scorePercentage);
              
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new Exception("An error occurred while saving results.", ex);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
