using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Quizify
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        protected void ValidateMobileNumber(object sender, ServerValidateEventArgs e)
        {
            if (e.Value.Length == 10 && e.Value.All(char.IsDigit))
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
            }
        }

    }
}