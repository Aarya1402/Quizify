using System;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Quizify.Subjects
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Retrieve the data from the Application state (or from a static variable)
            string jsonData = Application["ApiData"] as string; // or SecureDataStore.GetData();

            // Check if data exists
            if (!string.IsNullOrEmpty(jsonData))
            {
                try
                {
                    // Determine if the JSON is an object or an array
                    JToken parsedData = JToken.Parse(jsonData);

                    if (parsedData is JObject)
                    {
                        // Handle JSON object
                        RenderJsonObject(parsedData as JObject);
                    }
                    else if (parsedData is JArray)
                    {
                        // Handle JSON array
                        RenderJsonArray(parsedData as JArray);
                    }
                }
                catch (JsonReaderException ex)
                {
                    Response.Write("<p>Error parsing JSON data.</p>");
                    // Log the exception (ex.Message) if needed
                }
            }
            else
            {
                // Handle the case where no data is available
                Response.Write("<p>No data available</p>");
            }
        }

        private void RenderJsonObject(JObject jsonObject)
        {
            foreach (var property in jsonObject.Properties())
            {
                Response.Write($"<strong>{property.Name}:</strong> ");

                if (property.Value.Type == JTokenType.Array)
                {
                    Response.Write("<ul>");
                    foreach (var item in property.Value)
                    {
                        Response.Write($"<li>{item}</li>");
                    }
                    Response.Write("</ul>");
                }
                else
                {
                    Response.Write($"{property.Value}<br />");
                }
            }
        } 

        private void RenderJsonArray(JArray jsonArray)
        {
            Response.Write("<ul>");
            foreach (var item in jsonArray)
            {
                if (item is JObject)
                {
                    Response.Write("<li>");
                    RenderJsonObject(item as JObject);
                    Response.Write("</li>");
                }
                else
                {
                    Response.Write($"<li>{item}</li>");
                }
            }
            Response.Write("</ul>");
        }
    }
}
