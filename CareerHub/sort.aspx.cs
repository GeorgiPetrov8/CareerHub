using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerHub
{
    public partial class sort : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["AuthCookie"] != null)
            {
                // Cookie exists
                HttpCookie cookie = Request.Cookies["AuthCookie"];
                string userEmail = cookie.Value;
                litUserName.Text = HttpUtility.HtmlEncode(userEmail);
                loadCategories();

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

                if (Request.QueryString["category"] == "Select")
                {
                    Response.Redirect("~/index.aspx");
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["category"]))
                {

                    string category = Request.QueryString["category"];
                    LoadJobs(0, 9, category);
                }
                else {
                    Response.Write("An error occurred");
                }
            }
            else
            {
                // Redirect to the home page or any other page
                Response.Redirect("~/login.aspx");
            }
        }

        private void loadCategories()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // Categories
            string category = "SELECT category FROM Categories";

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(category, connection))
                    {
                        // Execute the command and read categories
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string categoryName = reader["category"].ToString();
                            ddlCategory.Items.Add(new ListItem(categoryName, categoryName));
                        }
                        reader.Close();
                    }
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void btnSort_Click(object sender, EventArgs e)
        {
            string selectedCategory = ddlCategory.SelectedValue; ;
            Response.Redirect("sort.aspx?category=" + selectedCategory);
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
                            connection.Close ();
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

        [WebMethod]
        public static string LoadMoreJobs(int offset, int limit, string category)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // Jobs
            string jobs = @"SELECT id, email, title, phone, payment, category, description, date 
                     FROM Jobs 
                     WHERE CONVERT(NVARCHAR(MAX), category) = @Category
                     ORDER BY id DESC 
                     OFFSET @Offset ROWS 
                     FETCH NEXT @Limit ROWS ONLY";

            // Use a StringBuilder to build the HTML content
            StringBuilder html = new StringBuilder();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(jobs, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Offset", offset);
                        cmd.Parameters.AddWithValue("@Limit", limit);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string id = reader["id"].ToString();
                                string jobEmail = reader["email"].ToString();
                                string jobTitle = reader["title"].ToString();
                                string jobPhone = reader["phone"].ToString();
                                string jobPayment = reader["payment"].ToString();
                                string jobCategory = reader["category"].ToString();
                                string jobDescription = reader["description"].ToString();
                                string date = reader["date"].ToString();

                                html.Append("<div class='post' " + " id= " + id + " onclick='openJobPage(" + id + ")'>");

                                html.Append("<div class='profileDelete'>");
                                html.Append("<h4 class='post-name'>").Append(jobEmail).Append("</h4>");
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
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"<p>Error loading jobs: {ex.Message}</p>";
            }

            return html.ToString();
        }

        private void LoadJobs(int offset, int limit, string category)
        {
            string jobsHtml = LoadMoreJobs(offset, limit, category);
            mainContent.InnerHtml = jobsHtml;
        }
    }
}