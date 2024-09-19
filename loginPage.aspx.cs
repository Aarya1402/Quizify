using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Quizify.Subjects;

namespace Quizify
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        protected void Page_Load(object sender, EventArgs e)
        {
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            cmd.Connection = con;
        }

        protected void tbPassword_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string _email = tbemail.Text.Trim();
            string _password = tbPassword.Text.Trim();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT COUNT(*) FROM [User] WHERE Email = @Email AND Password = @Password";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Email", _email);
                cmd.Parameters.AddWithValue("@Password", _password);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    cmd.CommandText = "SELECT role FROM [User] WHERE Email = @Email AND Password = @Password";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Email", _email);
                    cmd.Parameters.AddWithValue("@Password", _password);
                    bool isAdmin = (bool)cmd.ExecuteScalar();
                    Session["role"] = isAdmin ? "Admin" : "User";

                    cmd.CommandText = "SELECT Id FROM [User] WHERE Email = @Email AND Password = @Password";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Email", _email);
                    cmd.Parameters.AddWithValue("@Password", _password);
                    int id = (int)cmd.ExecuteScalar();
                    Session["UserId"] = id;
                    Session["email"] = _email;
                    Session["IsLoggedIn"] = true;
                    Response.Write(Session);

                    if (Session["role" ] == "Admin")
                    {
                        Response.Redirect("~/Subjects/MySubjects.aspx");
                    }
                    Response.Redirect("~/Subjects/Home.aspx");
                }
                else
                {
                    cv_pass_email.ErrorMessage = "Invalid Email or Password";
                    cv_pass_email.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                cv_pass_email.ErrorMessage = "An error occurred: " + ex.Message;
                cv_pass_email.IsValid = false;
            }
            finally
            {
                con.Close();
            }
        }

        protected void check_password_and_email(object source, ServerValidateEventArgs args)
        {
            
        }


    }
}
