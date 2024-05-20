<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="CareerHub.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CareerHub</title>
    <link href="CSS\login.css" rel="stylesheet" />

    <style>
        .error-message {
            color: red;
            font-weight: bold;
            font-size: 16px;
            text-align: center;
            display: block;
            width: 100%;
        }
    </style>

</head>
<body>

    <div class="pen-title">
        <h1>CareerHub</h1>
    </div>

    <form id="form1" runat="server">

        <div class="form-module">
            <div class="form">

                <div class="toggle">
                    <button class="tooltip" type="button" onclick="window.location.href='login.aspx';">Login</button>
                </div>

                <h2>Create an account</h2>

                
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br />
                Username:
                <asp:TextBox ID="txtUsername" runat="server" required></asp:TextBox><br />
                Email:
                <asp:TextBox ID="txtEmail" runat="server" type="email" required></asp:TextBox><br />
                Password:
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" required></asp:TextBox><br />
                Confirm Password:
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" required></asp:TextBox><br />
               
                <div class="role">
                    Role:
                    <asp:DropDownList ID="ddlRole" runat="server" required CssClass="styled-dropdown">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Employee" Value="Employee" />
                        <asp:ListItem Text="Employer" Value="Employer" />
                    </asp:DropDownList><br />
                </div>

                <asp:Button class="button" ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />

            </div>
        </div>
    </form>
</body>
</html>
