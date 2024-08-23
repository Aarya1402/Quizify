using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Quizify.Subjects
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string jsonData = Application["ApiData"] as string;

            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    JToken parsedData = JToken.Parse(jsonData);
                    if (parsedData is JObject jsonObject)
                    {
                        ProcessData(jsonObject);
                    }
                    else if (parsedData is JArray jsonArray)
                    {
                        foreach (var item in jsonArray)
                        {
                            if (item is JObject obj)
                            {
                                ProcessData(obj);
                            }
                        }
                    }
                }
                catch (JsonReaderException)
                {
                    Response.Write("<p>Error parsing JSON data.</p>");
                }
            }
            else
            {
                Response.Write("<p>No data available</p>");
            }
        }

        private void ProcessData(JObject jsonObject)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
                conn.Open();

                // Insert fixed Subject entry if it doesn't exist
                int subjectId = 5;  // Fixed subject_id = 5 for MySQL
                if (!RecordExists(conn, "Subject", subjectId))
                {
                    InsertSubject(conn, subjectId, "MySQL", null);
                }

                // Insert data into Question and Option tables with subject_id = 5
                int questionId = (int)jsonObject["id"];
                if (!RecordExists(conn, "Question", questionId))
                {
                    InsertQuestion(conn, questionId, (string)jsonObject["question"], subjectId, FindCorrectAnswer(jsonObject));
                }

                if (!RecordExists(conn, "Option", questionId))
                {
                    InsertCorrectOption(conn, jsonObject, questionId, subjectId);
                }

                conn.Close();
            }
        }

        private void InsertSubject(SqlConnection conn, int id, string name, string description)
        {
            string query = "INSERT INTO Subject (Id, name, description) VALUES (@Id, @Name, @Description)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertQuestion(SqlConnection conn, int id, string question, int subjectId, string correctAnswer)
        {
            string query = "INSERT INTO Question (Id, question, subject_id, correct_ans) VALUES (@Id, @Question, @SubjectId, @CorrectAns)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Question", question);
                cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                cmd.Parameters.AddWithValue("@CorrectAns", correctAnswer);
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertCorrectOption(SqlConnection conn, JObject jsonObject, int questionId, int subjectId)
        {
            JObject options = (JObject)jsonObject["answers"];
            string correctAnswer = FindCorrectAnswer(jsonObject);
            foreach (var option in options.Properties())
            {
                if (option.Value.ToString() == correctAnswer)
                {
                    string query = "INSERT INTO [Option] (Id, options, question_id) VALUES (@Id, @Option, @QuestionId)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", questionId);
                        cmd.Parameters.AddWithValue("@Option", option.Value.ToString());
                        cmd.Parameters.AddWithValue("@QuestionId", questionId);
                        cmd.ExecuteNonQuery();
                    }
                    break;
                }
            }
        }

        private bool RecordExists(SqlConnection conn, string tableName, int id)
        {
            string query = $"SELECT COUNT(*) FROM [{tableName}] WHERE Id = @Id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private string FindCorrectAnswer(JObject jsonObject)
        {
            JObject correctAnswers = (JObject)jsonObject["correct_answers"];
            foreach (var correctAnswer in correctAnswers.Properties())
            {
                if (correctAnswer.Value.ToString().ToLower() == "true")
                {
                    string answerKey = correctAnswer.Name.Replace("_correct", "");
                    return (string)jsonObject["answers"][answerKey];
                }
            }
            return string.Empty;
        }
    }
}
