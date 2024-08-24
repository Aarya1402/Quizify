using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;

namespace Quizify
{
    public partial class JustInsertion : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            // Call the API and fetch data
            string jsonData = await CallApiAsync();

            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    JToken parsedData = JToken.Parse(jsonData);
                    if (parsedData is JObject jsonObject)
                    {
                        UpdateCorrectAnswer(jsonObject);
                    }
                    else if (parsedData is JArray jsonArray)
                    {
                        foreach (var item in jsonArray)
                        {
                            if (item is JObject obj)
                            {
                                UpdateCorrectAnswer(obj);
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

        private async Task<string> CallApiAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "https://quizapi.io/api/v1/questions?apiKey=LqFa0o92b68fjUZQoyR847u17hYiZVjA12ihPBq9&limit=30&tags=PHP";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Response.Write("<p>Error fetching data from the API.</p>");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<p>Error: {ex.Message}</p>");
                    return null;
                }
            }
        }

        private void UpdateCorrectAnswer(JObject jsonObject)
        {
            int questionId = (int)jsonObject["id"];
            string correctAnswerKey = jsonObject["correct_answer"].ToString();
            JObject answers = (JObject)jsonObject["answers"];
            string correctAnswerValue = answers[correctAnswerKey]?.ToString();

            if (string.IsNullOrEmpty(correctAnswerValue))
            {
                Response.Write($"<p>No correct answer found for question ID {questionId}</p>");
                return;
            }

            // Update the correct_ans field in the Question table
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\C#Minis\\ASP_DOTNET_FRAMEWORK\\App_Data\\Users.mdf;Integrated Security=True"; // Update with your connection string
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE [Question] SET correct_ans = @CorrectAnswer WHERE Id = @QuestionId";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CorrectAnswer", correctAnswerValue);
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Response.Write($"<p>Correct answer updated for question ID {questionId}</p>");
                    }
                    else
                    {
                        Response.Write($"<p>Question ID {questionId} not found in the Question table</p>");
                    }
                }
            }
        }
    }
}
