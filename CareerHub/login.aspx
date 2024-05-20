<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CareerHub.login" %>

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
                    <button class="tooltip" type="button" onclick="window.location.href='Registration.aspx';">Register</button>
                </div>

                <h2>Login to your account</h2>

                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br />
                Email:
                <asp:TextBox ID="txtUsername" runat="server" type="email" required></asp:TextBox><br />
                Password:
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" required></asp:TextBox><br />
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" class="button" />
                
            </div>
        </div>
    </form>
     
</body>
</html>
