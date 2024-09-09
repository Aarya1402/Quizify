using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quizify.Subjects
{
    public partial class Subject : System.Web.UI.Page
    {
        private int currentQuestionIndex = 0;
        private DataTable questionsTable;
        private int subjectId;
        private Dictionary<int, int> userAnswers = new Dictionary<int, int>();
        private int correctAnswers = 0;
        private int incorrectAnswers = 0;
        private int skippedAnswers = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out subjectId))
                {
                    if (Session["CurrentQuestionIndex"] == null)
                        Session["CurrentQuestionIndex"] = 0;

                    LoadQuestions(subjectId);
                    ShowQuestion((int)Session["CurrentQuestionIndex"]);
                }
                else
                {
                    Response.Write("<p>Invalid subject ID.</p>");
                }
            }
        }

        private void LoadQuestions(int subjectId)
        {
            string connString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT q.Id, q.Question, o.Id AS OptionId, o.Options, q.Correct_ans " +
                               "FROM Question q " +
                               "JOIN [Option] o ON q.Id = o.Question_id " +
                               "WHERE q.Subject_id = @SubjectId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    questionsTable = dataTable;

                    /*foreach(DataRow row in questionsTable.Rows)
                    {
                        Response.Write(row["question"].ToString());
                    }*/

                }
            }
        }

        private void ShowQuestion(int index)
        {
            if (questionsTable == null || questionsTable.Rows.Count == 0) return;

            var distinctQuestions = questionsTable.AsEnumerable()
                                                  .GroupBy(row => row.Field<int>("Id"))
                                                  .Select(g => g.First())
                                                  .ToList();

            if (index < 0 || index >= distinctQuestions.Count) return; // Check for valid index

            DataRow currentQuestionRow = distinctQuestions[index];

            int questionId = int.Parse(currentQuestionRow["Id"].ToString());

            QuestionLabel.Text = currentQuestionRow["Question"].ToString();

            OptionsList.Items.Clear();

            var options = questionsTable.AsEnumerable()
                                        .Where(row => row.Field<int>("Id") == questionId);

            foreach (DataRow option in options)
            {
                OptionsList.Items.Add(new ListItem(option["Options"].ToString(), option["OptionId"].ToString()));
            }

            if (userAnswers.ContainsKey(questionId))
            {
                OptionsList.SelectedValue = userAnswers[questionId].ToString();
            }
            else
            {
                OptionsList.ClearSelection();
            }

            PrevButton.Visible = true;
            NextButton.Visible = true;
            FinishButton.Visible = true;
        }




        protected void PrevButton_Click(object sender, EventArgs e)
        {
            SaveUserAnswer();

            int currentIndex = (int)Session["CurrentQuestionIndex"];
            if (currentIndex > 0)
            {
                currentIndex--;
                Session["CurrentQuestionIndex"] = currentIndex;
                ShowQuestion(currentIndex);
            }
            else
            {
                ShowQuestion(currentIndex);
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            SaveUserAnswer();

            int currentIndex = (int)Session["CurrentQuestionIndex"];

            if (currentIndex>0)
            {
                currentIndex--;
                Session["CurrentQuestionIndex"] = currentIndex;
                ShowQuestion(currentIndex);
            }
            else
            {
                ShowQuestion(currentIndex);
            }
        }

        protected void FinishButton_Click(object sender, EventArgs e)
        {
            SaveUserAnswer();
            CalculateScore();
            Response.Write($"<p>Test Completed! <br/> Correct Answers: {correctAnswers} <br/> Incorrect Answers: {incorrectAnswers} <br/> Skipped Questions: {skippedAnswers}</p>");
        }

        private void SaveUserAnswer()
        {
            if (questionsTable == null || questionsTable.Rows.Count == 0) return;

            DataRow currentQuestionRow = questionsTable.Rows[currentQuestionIndex];
            int questionId = int.Parse(currentQuestionRow["Id"].ToString());

            if (OptionsList.SelectedItem != null)
            {
                int selectedOptionId = int.Parse(OptionsList.SelectedValue);
                userAnswers[questionId] = selectedOptionId;
            }
            else if (!userAnswers.ContainsKey(questionId))
            {
                userAnswers[questionId] = -1;
            }
        }

        private void CalculateScore()
        {
            foreach (DataRow questionRow in questionsTable.Rows)
            {
                int questionId = int.Parse(questionRow["Id"].ToString());
                int correctOptionId = int.Parse(questionRow["Correct_ans"].ToString());

                if (userAnswers.ContainsKey(questionId))
                {
                    int userAnswerId = userAnswers[questionId];
                    if (userAnswerId == -1)
                    {
                        skippedAnswers++;
                    }
                    else if (userAnswerId == correctOptionId)
                    {
                        correctAnswers++;
                    }
                    else
                    {
                        incorrectAnswers++;
                    }
                }
                else
                {
                    skippedAnswers++;
                }
            }
        }

    }
}