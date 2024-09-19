using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quizify.Subjects
{
    public partial class MySubjects : System.Web.UI.Page
    {
        SqlConnection connection;
        SqlCommand command;
        DataSet ds;
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
            string conn = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            connection = new SqlConnection(conn);
            command = new SqlCommand();
            command.Connection = connection;
            if (!IsPostBack)
            {
                connection.Open();
                FillSubjectsGridViewa();
                connection.Close();
            }
        }
        private void FillSubjectsGridViewa()
        {
          
            command.CommandText = "SELECT id, name, description FROM Subject";
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            ds = new DataSet();


            adapter.Fill(ds, "subject");


            SubjectsGridViewa.DataSource = ds.Tables["subject"];
            SubjectsGridViewa.DataBind();
        }



        protected void btnAddSubject_Click(object sender, EventArgs e)
        {
            string name = tbSubName.Text.Trim();
            string desc = tbSubDesc.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(desc))
            {
                Response.Write("<p>Enter valid data</p>");
                return;
            }

            try
            {
                command.CommandText = "INSERT INTO Subject (name, description) VALUES (@name, @description)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", desc);

                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    // Successfully added subject
                    FillSubjectsGridViewa();
                    tbSubName.Text = string.Empty;
                    tbSubDesc.Text = string.Empty;
                }
                else
                {
                    Response.Write("<p>Can't add subject.</p> ");
                    return;
                }
            }
            catch (Exception ex)
            {
               Response.Write(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        protected void SubjectsGridViewa_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SubjectsGridViewa.EditIndex = e.NewEditIndex;
            FillSubjectsGridViewa();
        }
        protected void SubjectsGridViewa_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SubjectsGridViewa.EditIndex = -1;
            FillSubjectsGridViewa();
        }
        protected void SubjectsGridViewa_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the subject ID (ensure DataKeyNames is set correctly)
            int id = Convert.ToInt32(SubjectsGridViewa.DataKeys[e.RowIndex].Value);

            // Fetch the new values for name and description, with null checking
            string name = e.NewValues["Name"] != null ? e.NewValues["Name"].ToString() : string.Empty;
            string desc = e.NewValues["Description"] != null ? e.NewValues["Description"].ToString() : string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(desc))
            {
                // Handle the case where name or description is empty (you can show an error or default values)
                Response.Write("Name and Description cannot be empty.");
                return;
            }

            // Update command
            command.CommandText = $"Update Subject Set name=@name, description=@desc Where Id=@id";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@desc", desc);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                SubjectsGridViewa.EditIndex = -1;
                FillSubjectsGridViewa();
            }
            connection.Close();
        }

        protected void SubjectsGridViewa_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int id = Convert.ToInt32(SubjectsGridViewa.DataKeys[e.RowIndex].Value);
         
            command.CommandText = $"DELETE from Subject Where Id=@id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                SubjectsGridViewa.EditIndex = -1;
                FillSubjectsGridViewa();
            }
            connection.Close();
        }

        protected void btnViewQuiz_Click(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] == null)
            {
                Response.Redirect("~/loginPage.aspx");
                return;
            }
            Response.Redirect("~/Subjects/Home.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();


            Response.Redirect("~/loginPage.aspx");
        }
    }
}
