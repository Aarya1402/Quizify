using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Quizify.Subjects
{
    public partial class Subject : System.Web.UI.Page
    {
        private DataTable questionsTable;
        private Dictionary<int, int> userAnswers = new Dictionary<int, int>();
        private int correctAnswers = 0;
        private int incorrectAnswers = 0;
        private int skippedAnswers = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["QuestionsTable"] != null)
            {
                questionsTable = (DataTable)Session["QuestionsTable"];
            }

            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out int subjectId))
                {
                    Session["CurrentQuestionIndex"] = 0;
                    LoadQuestions(subjectId);
                    ShowQuestion((int)Session["CurrentQuestionIndex"]);
                }
                else
                {
                    Response.Write("<p>Invalid subject ID.</p>");
                }
            }
            else
            {
                string jsonUserAnswers = HiddenFieldAnswers.Value;
                if (!string.IsNullOrEmpty(jsonUserAnswers))
                {
                    var userAnswers = JsonConvert.DeserializeObject<Dictionary<int, int>>(jsonUserAnswers);

                    CalculateScore(userAnswers);
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

                    Session["QuestionsTable"] = questionsTable;
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

            if (index < 0 || index >= distinctQuestions.Count) return;

            DataRow currentQuestionRow = distinctQuestions[index];
            int questionId = int.Parse(currentQuestionRow["Id"].ToString());

            QuestionLabel.Text = currentQuestionRow["Question"].ToString();

            OptionsPanel.Controls.Clear();

            var options = questionsTable.AsEnumerable()
                                        .Where(row => row.Field<int>("Id") == questionId);

            foreach (DataRow option in options)
            {
                var optionDiv = new HtmlGenericControl("div");
                optionDiv.InnerHtml = option["Options"].ToString();
                optionDiv.Attributes.Add("class", "option-box");
                optionDiv.Attributes.Add("data-question-id", questionId.ToString());
                optionDiv.Attributes.Add("data-option-id", option["OptionId"].ToString());
                optionDiv.Attributes.Add("onclick", "selectOption(this)");
                OptionsPanel.Controls.Add(optionDiv);
            }

            // Select the saved option if available
            if (userAnswers.ContainsKey(questionId))
            {
                string selectedValue = userAnswers[questionId].ToString();
                var selectedOptionDiv = OptionsPanel.Controls.OfType<HtmlGenericControl>()
                                                             .FirstOrDefault(div => div.Attributes["data-option-id"] == selectedValue);
                if (selectedOptionDiv != null)
                {
                    selectedOptionDiv.Attributes["class"] += " selected";
                }
            }

            PrevButton.Visible = index > 0;
            NextButton.Visible = index < distinctQuestions.Count - 1;
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

            if (currentIndex < questionsTable.Rows.Count)
            {
                currentIndex++;
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
            CalculateScore(userAnswers);
            Response.Write($"<p>Test Completed! <br/> Correct Answers: {correctAnswers} <br/> Incorrect Answers: {incorrectAnswers} <br/> Skipped Questions: {skippedAnswers}</p>");
        }

        private void SaveUserAnswer()
        {
            if (questionsTable == null || questionsTable.Rows.Count == 0) return;

            int currentIndex = (int)Session["CurrentQuestionIndex"];
            var distinctQuestions = questionsTable.AsEnumerable()
                                                  .GroupBy(row => row.Field<int>("Id"))
                                                  .Select(g => g.First())
                                                  .ToList();

            DataRow currentQuestionRow = distinctQuestions[currentIndex];
            int questionId = int.Parse(currentQuestionRow["Id"].ToString());

            var selectedOptionId = Request.Form["SelectedOption" + questionId];

            if (!string.IsNullOrEmpty(selectedOptionId))
            {
                userAnswers[questionId] = int.Parse(selectedOptionId);
            }
            else if (!userAnswers.ContainsKey(questionId))
            {
                userAnswers[questionId] = -1;
            }

            Session["UserAnswers"] = userAnswers;
        }


        private void CalculateScore(Dictionary<int, int> userAnswers)
        {
            correctAnswers = 0;
            incorrectAnswers = 0;
            skippedAnswers = 0;

            foreach (DataRow questionRow in questionsTable.Rows)
            {
                int questionId = int.Parse(questionRow["Id"].ToString());

                if (int.TryParse(questionRow["Correct_ans"].ToString(), out int correctOptionId))
                {
                    Console.WriteLine($"Question ID: {questionId}, Correct Option ID: {correctOptionId}");

                    if (userAnswers.TryGetValue(questionId, out int userAnswerId))
                    {
                        Console.WriteLine($"User Answer ID: {userAnswerId}");

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
                else
                {

                    Console.WriteLine($"Failed to convert Correct_ans to int. Data: {questionRow["Correct_ans"]}");
                    skippedAnswers++;
                }
            }

            Console.WriteLine($"Correct Answers: {correctAnswers}");
            Console.WriteLine($"Incorrect Answers: {incorrectAnswers}");
            Console.WriteLine($"Skipped Questions: {skippedAnswers}");
        }


        protected void OptionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveUserAnswer();

            int currentIndex = (int)Session["CurrentQuestionIndex"];
            ShowQuestion(currentIndex);
        }


    }
}
