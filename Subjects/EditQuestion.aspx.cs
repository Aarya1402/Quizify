using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quizify.Subjects
{
    public partial class EditQuestion : System.Web.UI.Page
    {
        private SqlConnection connection;
        private SqlCommand command;
        private DataSet ds;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] == null)
            {
                Response.Redirect("~/loginPage.aspx");
                return;
            }
            if (Session["Role"] == "User" || Session["Role"] == null)
            {
                Response.Write("<p>Please login as admin.</p>");
                return;
            }
            btnAddoption.Visible = false;
            lbloption.Visible = false;
            tbOption.Visible = false;
            string connString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            connection = new SqlConnection(connString);
            command = new SqlCommand();
            command.Connection = connection;

            if (!IsPostBack)
            {
                FillQuestionsGridView();
            }
        }

        private void FillQuestionsGridView()
        {
            int id = int.Parse(Request.QueryString["id"]);
            command.CommandText = "SELECT Id, question, correct_ans FROM Question WHERE subject_id=@id";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", id);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            ds = new DataSet();

            adapter.Fill(ds, "question");

            QuestionsGridView.DataSource = ds.Tables["question"];
            QuestionsGridView.DataBind();
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            string question = tbQuestion.Text.Trim();
            string correctAns = tbCorrectAns.Text.Trim();

            if (string.IsNullOrEmpty(question) || string.IsNullOrEmpty(correctAns))
            {
                Response.Write("<p>Enter valid data</p>");
                return;
            }

            try
            {
                command.CommandText = "INSERT INTO Question (question, correct_ans, subject_id) VALUES (@question, @correctAns, @subjectId)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@question", question);
                command.Parameters.AddWithValue("@correctAns", correctAns);
                command.Parameters.AddWithValue("@subjectId", int.Parse(Request.QueryString["id"]));

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    FillQuestionsGridView();
                    tbQuestion.Text = string.Empty;
                    tbCorrectAns.Text = string.Empty;
                }
                else
                {
                    Response.Write("<p>Can't add question.</p>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
            finally
            {
                connection.Close();
            }
        }

        protected void QuestionsGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            QuestionsGridView.EditIndex = e.NewEditIndex;
            FillQuestionsGridView();
        }

        protected void QuestionsGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            QuestionsGridView.EditIndex = -1;
            FillQuestionsGridView();
        }

        protected void QuestionsGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(QuestionsGridView.DataKeys[e.RowIndex].Value);
            string question = e.NewValues["Question"]?.ToString() ?? string.Empty;
            string correctAns = e.NewValues["Correct_ans"]?.ToString() ?? string.Empty;

            try
            {
                command.CommandText = "UPDATE Question SET question = @question, correct_ans = @correctAns WHERE Id = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@question", question);
                command.Parameters.AddWithValue("@correctAns", correctAns);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    QuestionsGridView.EditIndex = -1;
                    FillQuestionsGridView();
                }
                else
                {
                    Response.Write("<p>Update failed.</p>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
            finally
            {
                connection.Close();
            }
        }

        protected void QuestionsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(QuestionsGridView.DataKeys[e.RowIndex].Value);

            try
            {
                command.CommandText = "DELETE FROM Question WHERE Id = @id";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    FillQuestionsGridView();
                }
                else
                {
                    Response.Write("<p>Deletion failed.</p>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
            finally
            {
                connection.Close();
            }
        }

        protected void QuestionsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id;
            if (int.TryParse(QuestionsGridView.SelectedDataKey.Value.ToString(), out id))
            {
                Response.Write(id);
                BindOptionsGridView(id);
            }
        }

        // Bind the options for the selected question
        private void BindOptionsGridView(int questionId)
        {
             btnAddoption.Visible = true;
            lbloption.Visible = true;
            tbOption.Visible = true;
            try
            {
                command.CommandText = "SELECT Id, options FROM [Option] WHERE question_id = @questionId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@questionId", questionId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                ds = new DataSet();

                adapter.Fill(ds, "option");

                GridViewOptions.DataSource = ds.Tables["option"];
                GridViewOptions.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
        }

        protected void GridViewOptions_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewOptions.EditIndex = e.NewEditIndex;
            BindOptionsGridView(Convert.ToInt32(QuestionsGridView.SelectedDataKey.Value));
        }

        protected void GridViewOptions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewOptions.EditIndex = -1;
            BindOptionsGridView(Convert.ToInt32(QuestionsGridView.SelectedDataKey.Value));
        }

        protected void GridViewOptions_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int optionId = Convert.ToInt32(GridViewOptions.DataKeys[e.RowIndex].Value);
            string optionText = e.NewValues["options"]?.ToString() ?? string.Empty;

            try
            {
                command.CommandText = "UPDATE [Option] SET options = @optionText WHERE Id = @optionId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@optionText", optionText);
                command.Parameters.AddWithValue("@optionId", optionId);

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    GridViewOptions.EditIndex = -1;
                    BindOptionsGridView(Convert.ToInt32(QuestionsGridView.SelectedDataKey.Value));
                }
                else
                {
                    Response.Write("<p>Update failed.</p>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
            finally
            {
                connection.Close();
            }
        }

        protected void GridViewOptions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int optionId = Convert.ToInt32(GridViewOptions.DataKeys[e.RowIndex].Value);
           
            try
            {
                command.CommandText = "DELETE FROM [Option] WHERE Id = @optionId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@optionId", optionId);

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    BindOptionsGridView(Convert.ToInt32(QuestionsGridView.SelectedDataKey.Value));
                }
                else
                {
                    Response.Write("<p>Deletion failed.</p>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
            finally
            {
                connection.Close();
            }
        }

        protected void btnAddoption_Click(object sender, EventArgs e)
        {
            string newOptionText = tbOption.Text.Trim(); // Get the text from the textbox

            if (string.IsNullOrEmpty(newOptionText))
            {
                Response.Write("<p>Please enter a valid option.</p>");
                return;
            }

            try
            {
                int questionId = Convert.ToInt32(QuestionsGridView.SelectedDataKey.Value); // Get the selected question ID

                command.CommandText = "INSERT INTO [Option] (options, question_id) VALUES (@optionText, @questionId)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@optionText", newOptionText);
                command.Parameters.AddWithValue("@questionId", questionId);

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    // Option added successfully, refresh the options grid view
                    tbOption.Text = string.Empty; // Clear the textbox for the next input
                    BindOptionsGridView(questionId); // Refresh the options list
                }
                else
                {
                    Response.Write("<p>Failed to add option.</p>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<p>Error: " + ex.Message + "</p>");
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
