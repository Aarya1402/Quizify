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

namespace Quizify.Subjects
{
    public partial class Subject : System.Web.UI.Page
    {
        public DataTable questionsTable;
        public Dictionary<int, string> userAnswers = new Dictionary<int, string>();
        public int correctAnswers = 0;
        public int incorrectAnswers = 0;
        public int skippedAnswers = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out int subjectId))
                {
                    Session["CurrentQuestionIndex"] = 0;

                    if (Session["QuizStartTime"] == null)
                    {
                        Session["QuizStartTime"] = DateTime.Now;
                    }

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
                    userAnswers = JsonConvert.DeserializeObject<Dictionary<int, string>>(jsonUserAnswers);
                    // CalculateScore(userAnswers);
                }
            }

            DateTime startTime = (DateTime)Session["QuizStartTime"];
            TimeSpan elapsedTime = DateTime.Now - startTime;
            int remainingSeconds = (1 * 60) - (int)elapsedTime.TotalSeconds; 

            if (remainingSeconds <= 0)
            {
                CalculateScore(userAnswers);
                Response.Redirect("~/Results.aspx");
            }
            HiddenFieldRemainingTime.Value = remainingSeconds.ToString();
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
            questionsTable = Session["QuestionsTable"] as DataTable;

            if (questionsTable == null || questionsTable.Rows.Count == 0) return;

            var distinctQuestions = questionsTable.AsEnumerable().GroupBy(row => row.Field<int>("Id")).Select(g => g.First()).Take(20).ToList();

            if (index < 0 || index >= distinctQuestions.Count) return;

            DataRow currentQuestionRow = distinctQuestions[index];
            int questionId = int.Parse(currentQuestionRow["Id"].ToString());

            var options = FetchOptionsFromDatabase(questionId);

            if (options.Count <= 1)
            {
                ShowQuestion(index + 1);
                return;
            }

            QuestionLabel.Text = currentQuestionRow["Question"].ToString();
            OptionsPanel.Controls.Clear();

            foreach (DataRow option in options)
            {
                var optionText = option["Options"].ToString();
                var optionDiv = new HtmlGenericControl("div");
                optionDiv.InnerHtml = optionText;
                optionDiv.Attributes.Add("class", "option-box");
                optionDiv.Attributes.Add("data-question-id", questionId.ToString());
                optionDiv.Attributes.Add("data-option-id", option["Id"].ToString());
                optionDiv.Attributes.Add("onclick", "selectOption(this)");
                OptionsPanel.Controls.Add(optionDiv);
            }

            if (userAnswers.ContainsKey(questionId))
            {
                string selectedValue = userAnswers[questionId];
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

        private List<DataRow> FetchOptionsFromDatabase(int questionId)
        {
            var options = new List<DataRow>();
            string connString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT o.Id, o.options FROM [Option] o WHERE o.question_id = @QuestionId AND o.options IS NOT NULL";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        options = dataTable.AsEnumerable().ToList();
                    }
                }
            }
            return options;
        }

        protected void PrevButton_Click(object sender, EventArgs e)
        {
            SaveUserAnswer((int)Session["CurrentQuestionIndex"]);
            int currentIndex = (int)Session["CurrentQuestionIndex"];
            if (currentIndex > 0)
            {
                currentIndex--;
                Session["CurrentQuestionIndex"] = currentIndex;
                ShowQuestion(currentIndex);
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            SaveUserAnswer((int)Session["CurrentQuestionIndex"]);
            int currentIndex = (int)Session["CurrentQuestionIndex"];
            if (currentIndex < (Session["QuestionsTable"] as DataTable).AsEnumerable().Select(row => row.Field<int>("Id")).Distinct().Count() - 1)
            {
                currentIndex++;
                Session["CurrentQuestionIndex"] = currentIndex;
                ShowQuestion(currentIndex);
            }
        }

        protected void FinishButton_Click(object sender, EventArgs e)
        {
            SaveUserAnswer((int)Session["CurrentQuestionIndex"]);
            CalculateScore(userAnswers);

            Session["CorrectAnswers"] = correctAnswers;
            Session["IncorrectAnswers"] = incorrectAnswers;
            Session["SkippedAnswers"] = skippedAnswers;

            Response.Redirect("~/Results.aspx");
        }


        private void SaveUserAnswer(int questionIndex)
        {
            if (questionsTable == null || questionIndex < 0 || questionIndex >= questionsTable.Rows.Count) return;

            DataRow currentQuestionRow = questionsTable.Rows[questionIndex];
            int questionId = int.Parse(currentQuestionRow["Id"].ToString());

            if (userAnswers.ContainsKey(questionId))
            {
                userAnswers[questionId] = userAnswers[questionId];
            }
        }


        private void CalculateScore(Dictionary<int, string> userAnswers)
        {
            foreach (var answer in userAnswers)
            {
                int questionId = answer.Key;
                int selectedOptionId = int.Parse(answer.Value.ToString());
                string correctAnswerId;
                string optiontext;

                string connString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT correct_ans FROM Question WHERE Id = @QuestionId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@QuestionId", questionId);
                        conn.Open();

                        correctAnswerId = cmd.ExecuteScalar()?.ToString();
                    }

                    query = "SELECT options FROM [Option] WHERE Id = @optionId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@optionId", selectedOptionId);
                        

                        optiontext = cmd.ExecuteScalar()?.ToString();
                    }

                }
                        if (correctAnswerId == optiontext)
                        {
                            correctAnswers++;
                        }
                        else
                        {
                            incorrectAnswers++;
                        }
                    }
                
            

            skippedAnswers = 20 - correctAnswers - incorrectAnswers;
        }

    }
}
