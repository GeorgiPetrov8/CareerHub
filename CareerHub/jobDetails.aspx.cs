using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerHub
{
    public partial class jobDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["AuthCookie"] != null)
            {
                // Cookie exists
                HttpCookie cookie = Request.Cookies["AuthCookie"];
                string userEmail = cookie.Value;
                litUserName.Text = HttpUtility.HtmlEncode(userEmail);

                if (IsUserEmployer(userEmail))
                {
                    // Make the "Add Post" link visible
                    lnkAddPost.Visible = true;
                    profileUser.Visible = true;
                    profileUser.NavigateUrl = "profile.aspx?jobEmail=" + Server.UrlEncode(userEmail);
                }
                else
                {
                    // Hide the "Add Post" link
                    lnkAddPost.Visible = false;
                    profileUser.Visible = false;
                }

                LoadJob();
            }
            else
            {
                // Redirect to the home page or any other page
                Response.Redirect("~/login.aspx");
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            // Remove the cookie
            if (Request.Cookies["AuthCookie"] != null)
            {
                HttpCookie cookie = new HttpCookie("AuthCookie");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            // Redirect to the home page or any other page
            Response.Redirect("~/login.aspx");
        }

        protected bool IsUserEmployer(string userEmail)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; 

            // Check if the user is an employer
            string employerCheck = "SELECT Role FROM Users WHERE CONVERT(NVARCHAR(MAX), Email) = @Email";

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(employerCheck, connection))
                    {
                        // Add parameter for the email
                        command.Parameters.AddWithValue("@Email", userEmail);

                        object roleObj = command.ExecuteScalar();

                        // Check if the role is 'Employer'
                        if (roleObj != null && roleObj.ToString() == "Employer")
                        {
                            connection.Close();
                            return true;
                        }
                        else
                        {
                            connection.Close();
                            return false;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write("An error occurred: " + ex.Message);
                return false;
            }
        }

    private void LoadJob()
        {
            // Check if the jobId parameter exists in the URL
            if (!string.IsNullOrEmpty(Request.QueryString["jobId"]))
            {
                
                int jobId;
                StringBuilder html = new StringBuilder();

                if (int.TryParse(Request.QueryString["jobId"], out jobId))
                {
                    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                    
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            string query = "SELECT email, title, phone, payment, category, description, date FROM Jobs WHERE id = @JobId";

                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@JobId", jobId);

                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            
                            if (reader.Read())
                            {

                                string jobEmail = reader["email"].ToString();
                                string jobTitle = reader["title"].ToString();
                                string jobPhone = reader["phone"].ToString();
                                string jobPayment = reader["payment"].ToString();
                                string jobCategory = reader["category"].ToString();
                                string jobDescription = reader["description"].ToString();
                                string date = reader["date"].ToString();

                                html.Append("<div class='post'>");

                                html.Append("<div class='profileDelete'>");
                                html.Append("<h4 class='post-name' onclick='openProfilePage(\"" + Server.UrlEncode(jobEmail) + "\")'>").Append(jobEmail).Append("</h4>");
                                html.Append("</div>");

                                html.Append("<div class='titleDate'>");
                                html.Append("<h3 class='post-title'>").Append(jobTitle).Append("</h3>");
                                html.Append("<h3 class='post-date'>").Append(date).Append("</h3>");
                                html.Append("</div>");



                                html.Append("<h4>").Append("Phone: ").Append(jobPhone).Append("</h4>");
                                html.Append("<h4>").Append("Payment BGN: ").Append(jobPayment).Append("</h4>");
                                html.Append("<h4>").Append("Category: ").Append(jobCategory).Append("</h4>");

                                html.Append("<p class='post-content'>").Append(jobDescription).Append("</p>");
                                html.Append("</strong></p>").Append("</div>");
                                html.Append("<br>");

                                mainContent.InnerHtml = html.ToString();
                            }
                            else
                            {
                                Response.Write("No Offer Found!");
                            }
                            connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        Response.Write("An error occurred: " + ex.Message);
                    }
                }
                else
                {
                    Response.Write("JobID NOT Found!");
                }
            }
            else
            {
                Response.Write("No Offer Found in URL!");
            }
        }
    }
}