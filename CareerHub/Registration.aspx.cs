using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerHub
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
      
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            // Connection string to the SQL 
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // INSERT statement with parameter placeholders
            string insertQuery = "INSERT INTO Users (username, email, password, role) VALUES (@Value1, @Value2, @Value3, @Value4)";

            //Email check
            string checkEmail = "SELECT COUNT(*) FROM Users WHERE CONVERT(NVARCHAR(MAX), Email) = @Email";

            if (password != confirmPassword)
            {
                lblMessage.Text = "Passwords do not match.";
                lblMessage.CssClass = "error-message";
                return;
            }
            else
            {
                // Hash the password before storing it
                string hashedPassword = HashPassword(password);

                try
                {
                    // Establish a connection to the database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(checkEmail, connection))
                        {
                            command.Parameters.AddWithValue("@Email", email);
                            int count = (int)command.ExecuteScalar();

                            // Check if the email exists in the database
                            if (count > 0)
                            {
                                // Email exists
                                lblMessage.Text = "Email exists in the database.";
                                lblMessage.CssClass = "error-message";
                            }
                            else
                            {
                                // Create a SqlCommand object with the INSERT
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                                {
                                    // Add parameters to the SqlCommand
                                    insertCommand.Parameters.AddWithValue("@Value1", username);
                                    insertCommand.Parameters.AddWithValue("@Value2", email);
                                    insertCommand.Parameters.AddWithValue("@Value3", hashedPassword);
                                    insertCommand.Parameters.AddWithValue("@Value4", role);



                                    // Execute the INSERT command
                                    int rowsAffected = insertCommand.ExecuteNonQuery();

                                    // Check if any rows were affected
                                    if (rowsAffected > 0)
                                    {
                                        // Insertion successful
                                        Response.Redirect("~/login.aspx");
                                    }
                                    else
                                    {
                                        // No rows affected 
                                        Response.Write("Failed to insert data.");
                                    }
                                }
                            }
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
            
        }

        private string HashPassword(string password)
        {
            // Hash the password
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
    
}