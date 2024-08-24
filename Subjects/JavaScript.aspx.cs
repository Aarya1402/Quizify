using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quizify.Subjects
{
    public partial class JavaScript : System.Web.UI.Page
    {
        private const int QuestionsPerPage = 1; // Number of questions per page
        private int currentPage = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the subject ID from the query string
                int subjectId;
                if (int.TryParse(Request.QueryString["id"], out subjectId))
                {
                    // Load questions and options based on subject ID
                    LoadQuestionsAndOptions(subjectId, currentPage);
                }
                else
                {
                    Response.Write("<p>Invalid subject ID.</p>");
                }
            }
        }

        private void LoadQuestionsAndOptions(int subjectId, int page)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Query to retrieve questions and options for the given subject with pagination
                string query = @"
                    SELECT q.Id AS QuestionId, q.question AS QuestionText, o.Id AS OptionId, o.options AS OptionText
                    FROM Question q
                    LEFT JOIN [Option] o ON q.Id = o.question_id
                    WHERE q.subject_id = @SubjectId
                    ORDER BY q.Id
                    OFFSET @Offset ROWS
                    FETCH NEXT @Fetch ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                    cmd.Parameters.AddWithValue("@Offset", (page - 1) * QuestionsPerPage);
                    cmd.Parameters.AddWithValue("@Fetch", QuestionsPerPage);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var questions = new System.Collections.Generic.Dictionary<int, string>();
                            var options = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<Option>>();

                            while (reader.Read())
                            {
                                int questionId = reader.GetInt32(0);
                                string questionText = reader.GetString(1);
                                int? optionId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                                string optionText = reader.IsDBNull(3) ? null : reader.GetString(3);

                                if (!questions.ContainsKey(questionId))
                                {
                                    questions[questionId] = questionText;
                                    options[questionId] = new System.Collections.Generic.List<Option>();
                                }

                                if (optionId.HasValue && optionText != null)
                                {
                                    options[questionId].Add(new Option { Id = optionId.Value, Text = optionText });
                                }
                            }

                            var formattedQuestions = FormatQuestionsAndOptions(questions, options);

                            QuestionsRepeater.DataSource = formattedQuestions;
                            QuestionsRepeater.DataBind();
                        }
                        else
                        {
                            Controls.Add(new Literal { Text = "<p>No questions available for this subject.</p>" });
                        }
                    }
                }
            }
        }

        private System.Collections.Generic.List<QuestionDisplay> FormatQuestionsAndOptions(
            System.Collections.Generic.Dictionary<int, string> questions,
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<Option>> options)
        {
            var formattedList = new System.Collections.Generic.List<QuestionDisplay>();

            foreach (var question in questions)
            {
                var optionTexts = options.ContainsKey(question.Key) ? options[question.Key] : new System.Collections.Generic.List<Option>();
                var formattedOptions = new System.Text.StringBuilder();
                var optionCounter = 'A';

                foreach (var option in optionTexts)
                {
                    formattedOptions.AppendLine($"<input type='radio' name='question_{question.Key}' value='{option.Id}' /> {(char)optionCounter++}) {option.Text}<br/>");
                }

                formattedList.Add(new QuestionDisplay
                {
                    QuestionText = question.Value,
                    Options = formattedOptions.ToString()
                });
            }

            return formattedList;
        }

        protected void PrevButton_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadQuestionsAndOptions(int.Parse(Request.QueryString["id"]), currentPage);
            }
        }

        protected void NextButton_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadQuestionsAndOptions(int.Parse(Request.QueryString["id"]), currentPage);
        }

        private class QuestionDisplay
        {
            public string QuestionText { get; set; }
            public string Options { get; set; }
        }

        private class Option
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }
    }
}
