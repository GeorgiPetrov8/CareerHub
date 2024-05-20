using System;
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
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Authenticate the user
            bool isAuthenticated = AuthenticateUser(email, password);

            if (isAuthenticated)
            {
                // Create a new authentication token or use the email as cookie value
                string authToken = email; // use email as the authentication token

                // Create a new cookie
                HttpCookie authCookie = new HttpCookie("AuthCookie");
                authCookie.Value = authToken;
                authCookie.Expires = DateTime.Now.AddDays(1); // Cookie expiration 1 day

                // Add the cookie to the response
                Response.Cookies.Add(authCookie);

                // Passwords match - user is authenticated
                Response.Redirect("~/index.aspx");
            } else {
                // Display an error message
                lblMessage.Text = "Invalid username or password.";
                lblMessage.CssClass = "error-message";
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

        private bool AuthenticateUser(string email, string password)
        {
            // Connection string to the SQL
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString; ;

            // Password check
            string checkLogin = "SELECT password FROM Users WHERE CONVERT(NVARCHAR(MAX), email) = @Email";

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(checkLogin, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        // Execute the query to retrieve the password hash
                        string hashedPasswordFromDB = (string)command.ExecuteScalar();

                        // Hash the provided password using the same algorithm and parameters used to hash the passwords in the database
                        string hashedPassword = HashPassword(password);

                        // Compare the hashed passwords
                        if (hashedPasswordFromDB != null && hashedPasswordFromDB.Equals(hashedPassword))
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
                return false ;
            }
        } 
    }  
}