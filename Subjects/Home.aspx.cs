using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Quizify.Subjects
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] == null)
            {
                Response.Redirect("~/loginPage.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadSubjects();
            }
        }

        private void LoadSubjects()
        {
            string connString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT id, name FROM Subject";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable subjects = new DataTable();
                    adapter.Fill(subjects);
                    SubjectsGridView.DataSource = subjects;
                    SubjectsGridView.DataBind();
                }
            }
        }
    }
}
