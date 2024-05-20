using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerHub
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterToggleCategoryVisibilityScript();

            if (Request.Cookies["AuthCookie"] != null)
            {
                if(ddlCategory.Items.Count < 2) { loadCategories(); }

            }
            else
            {
                // Cookie does not exist
                Response.Redirect("~/login.aspx");
            }

        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {

            // Get the new category name from the text box
            string newCategory = txtNewCategory.Text.Trim();
            if (!CategoryExists(newCategory))
            {
                // Add the new category to the dropdown list
                //ddlCategory.Items.Add(new ListItem(newCategory, newCategory));
                //txtNewCategory.Text = "";

                ListItem firstItem = ddlCategory.Items[0];
                // Clear all items
                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(firstItem);
                loadCategories();
                ddlCategory.Items.Add(newCategory);


                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

                string query = "INSERT INTO Categories VALUES (@CategoryName)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CategoryName", newCategory);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., logging, error reporting)
                    Console.WriteLine("An error occurred while inserting the category: " + ex.Message);
                }
            }
            else
            {
                Response.Write("Category already exists ");
            }
        }

        protected bool CategoryExists(string categoryName)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // Check if the category exists
            string catCheck = "SELECT COUNT(*) FROM Categories WHERE CONVERT(NVARCHAR(MAX), category) = @CategoryName";

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(catCheck, connection))
                    {
                        // Add parameter for the category name
                        command.Parameters.AddWithValue("@CategoryName", categoryName);

                        int count = (int)command.ExecuteScalar();
                        connection.Close();
                        return count > 0; // Returns true if category exists, false otherwise
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void btnNewJob_Click(object sender, EventArgs e)
        {
            // Get user email from the cookie
            string userEmail = GetUserEmailFromCookie();
            int userId = GetUserIdByEmail(userEmail);

            // Check if user is an employer
            if (IsUserEmployer(userEmail) && userId!= -1)
            {
                // User is an employer, add to Jobs table
                AddToJobsTable(userEmail, userId);
            }
            else
            {
                Response.Write("Not an employer.");
            }
        }
        protected string GetUserEmailFromCookie()
        {
            // Retrieve user email from the cookie
            HttpCookie cookie = Request.Cookies["AuthCookie"];
            if (cookie != null)
            {
                return cookie.Value;
            }
            return null;
        }

        protected bool IsUserEmployer(string userEmail)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

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

        protected void AddToJobsTable(string userEmail, int userId)
        {
            // Get additional information
            string titleS = title.Text.Trim();
            string phoneNumber = phone.Text.Trim();
            string paymentS = payment.Text.Trim();
            string category = ddlCategory.SelectedValue;
            if (category == "Select")
            {
                Response.Write("Please select or enter a category.");

            }
            else
            {
                
                string descriptionS = description.Text.Trim();
                int categoryId = GetCategoryIdByName(category);
                Response.Write(category);
                if (categoryId == -1) {
                    Response.Write("Invalid category.");
                    return;
                }

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

                // Check if the user is an employer
                string insertJob = "INSERT INTO Jobs (idOfUser, email, title, phone, payment, category, categoryID, description, date) " + "VALUES (@UserId, @UserEmail, @Title, @PhoneNumber, @Payment, @Category, @CategoryID, @Description, @Date)";

                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(insertJob, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@UserEmail", userEmail);
                        command.Parameters.AddWithValue("@Title", titleS);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@Payment", paymentS);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@CategoryID", categoryId);
                        command.Parameters.AddWithValue("@Description", descriptionS);
                        command.Parameters.AddWithValue("@Date", DateTime.Today.ToString("yyyy - MM - dd"));

                        int rowsAffected = command.ExecuteNonQuery();

                        Response.Redirect("~/index.aspx");
                    }
                    connection.Close();
                }
            }

        }

        public int GetUserIdByEmail(string userEmail)
        {
            // -1 indicates user ID not found
            int userId = -1; 

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // ID based on email
            string query = "SELECT id FROM Users WHERE CONVERT(NVARCHAR(MAX), Email) = @Email";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Email", userEmail);
                        object result = command.ExecuteScalar();

                        // Check if a result was returned
                        if (result != null && result != DBNull.Value)
                        {
                            // Convert the result to an integer (user ID)
                            userId = Convert.ToInt32(result);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error: " + ex.Message);
            }

            return userId;
        }

        public int GetCategoryIdByName(string categoryName)
        {
            // -1 indicates category ID not found
            int categoryId = -1;

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // ID based on category name
            string query = "SELECT id FROM Categories WHERE CONVERT(NVARCHAR(MAX), category) = @CategoryName";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@CategoryName", categoryName);
                        object result = command.ExecuteScalar();

                        // Check if a result was returned
                        if (result != null && result != DBNull.Value)
                        {
                            // Convert the result to an integer (category ID)
                            categoryId = Convert.ToInt32(result);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error : " + ex.Message);
            }

            return categoryId;
        }

        private void RegisterToggleCategoryVisibilityScript()
        {
            string script = @"
                function toggleCategoryVisibility() {
                    var txtNewCategory = document.getElementById('" + txtNewCategory.ClientID + @"');
                    var btnAddCategory = document.getElementById('" + btnAddCategory.ClientID + @"');
                    var btnPlusCategory = document.getElementById('" + btnToggleCategory.ClientID + @"');

                     if (txtNewCategory.style.display === 'none') {
                        txtNewCategory.style.display = 'block';
                        btnAddCategory.style.display = 'block';
                        btnPlusCategory.value  = '-';
                    } else {
                        txtNewCategory.style.display = 'none';
                        btnAddCategory.style.display = 'none';
                        btnPlusCategory.value  = '+';
                     }
                    
                }
             ";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "toggleCategoryVisibility", script, true);
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
    }
}