using System;
using System.Linq;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace Quizify
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();

        /*protected void Page_Init(object sender, EventArgs e)
        {
            lbalert.Text = "Page Init: Initializing connection and command.";
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            cmd.Connection = con;
            lbalert.Text = "Page Init: Connection and command initialized.";
        }*/

        int InsertData(string username, string password, string email, int role)
        {
            lbalert.Text = "Entering InsertData function.";
            try
            {
                con.Open();
                lbalert.Text = "Connection opened.";
                cmd.CommandText = "INSERT INTO [User] (Name, Email, Password, Role) VALUES (@username, @email, @password, @role)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@role", role);

                int rowsAffected = cmd.ExecuteNonQuery();
                lbalert.Text = "Data inserted successfully.";
                return rowsAffected;
            }
            catch (Exception ex)
            {
                lbalert.Text = "Error in InsertData: " + ex.Message;
                throw new Exception("An error occurred while inserting data.", ex);
            }
            finally
            {
                con.Close();
                lbalert.Text = "Connection closed.";
            }
        }

        bool CheckRedundancy(string email)
        {
            lbalert.Text = "Entering CheckRedundancy function.";
            try
            {
                con.Open();
                lbalert.Text = "Connection opened.";
                cmd.CommandText = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Email", email);

                int count = (int)cmd.ExecuteScalar();
                lbalert.Text = "Redundancy check completed. Count: " + count;

                return count == 0;
            }
            catch (Exception ex)
            {
                lbalert.Text = "Error in CheckRedundancy: " + ex.Message;
                throw new Exception("An error occurred while checking redundancy.", ex);
            }
            finally
            {
                con.Close();
                lbalert.Text = "Connection closed.";
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            cmd.Connection = con;
            lbalert.Text = "Page Load: Before any operations.";
            lbalert.Text = "Page Load: After setting connection string.";
        }

        protected void ValidateMobileNumber(object sender, ServerValidateEventArgs e)
        {
            lbalert.Text = "Entering ValidateMobileNumber function.";
            e.IsValid = e.Value.Length == 10 && e.Value.All(char.IsDigit);
            lbalert.Text = "Mobile number validation " + (e.IsValid ? "passed." : "failed.");
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Response.Write("BTN click");
            lbalert.Text = "Entered btnSignUp_Click method_1.";
            con.ConnectionString = WebConfigurationManager.ConnectionStrings["QuizCon"].ConnectionString;
            cmd.Connection = con;
            lbalert.Text = "Entered btnSignUp_Click method_2.";
            string _username = tbUsername.Text.Trim();
            Response.Write(_username);
            string _password = tbPassword.Text.Trim();
            Response.Write(_password);
            string _email = tbEmail.Text.Trim();
            Response.Write(_email);
            int _role = int.Parse(rblRole.SelectedValue);
           Response.Write(_role);
          
            try
            {
                lbalert.Text = "Checking for email redundancy.";
                if (CheckRedundancy(_email))
                {
                    lbalert.Text = "Email is not redundant. Proceeding with data insertion.";
                    int rows = InsertData(_username, _password, _email, _role);
                    lbalert.Text = "Sign up successful.";

                    if(_role == 0)
                    {
                        Response.Redirect("~/Home.aspx");
                    }
                    else
                    {
                        Response.Write("~/MySubjects.aspc");
                    }

                    
                }
                else
                {
                    Response.Write("User already exist!!");
                    hlLogin.Focus();
     //               lbalert.Text = "User already exists.";
   //                 cv1.ErrorMessage = "User already exists";
 //                   cv1.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                lbalert.Text = "Error during sign-up: " + ex.Message;
                throw new Exception("An error occurred during sign-up.", ex);
            }
        }

        protected void redundancy_error(object source, ServerValidateEventArgs args)
        {
            lbalert.Text = "Entering redundancy_error function.";
            cv1.ErrorMessage = "User already exists.";
            lbalert.Text = "User already exists error set.";
        }
    }
}
