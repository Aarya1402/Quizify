using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Quizify.Subjects
{
    public partial class SubjectDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int subjectId;
                if (int.TryParse(Request.QueryString["id"], out subjectId))
                {
                    LoadSubjectDetails(subjectId);
                }
                else
                {
                    Response.Write("<p>Invalid subject ID.</p>");
                }
            }
        }

        private void LoadSubjectDetails(int subjectId)
        {
            string connString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Name FROM Subject WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", subjectId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        SubjectLabel.Text = $"Subject: {result.ToString()}";
                    }
                    else
                    {
                        Response.Write("<p>Subject not found.</p>");
                    }
                }
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (AgreeCheckBox.Checked)
            {
                // Confirmation message
                Response.Write("<p>Registration successful!</p>");

                // Handle additional registration logic here (if any)

                // Retrieve the subject ID from a hidden field or other source
                int subjectId = int.Parse(Request.QueryString["id"]);

                // Redirect to the respective subject page with the subject ID in the query string

                Response.Redirect($"~/Subjects/Subject.aspx?id={subjectId}");  
           
            }
            else
            {
                Response.Write("<p>You must agree to the terms and conditions to register.</p>");
            }
        }

    }
}
