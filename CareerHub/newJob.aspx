<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newJob.aspx.cs" Inherits="CareerHub.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CareerHub</title>

    <link href="CSS\index.css" rel="stylesheet" />
    <link href="CSS\offers.css" rel="stylesheet" />
    
</head>
<body>
    <form id="form1" runat="server" style="height:100%;">
        <div class="container">

            <!-- Left -->
            <div class="left-section">

                <!-- Logo -->
                <asp:HyperLink ID="linkLogo" runat="server" NavigateUrl="index.aspx">
                    <asp:Image src="careerhub.png" ID="imgLogo" runat="server" CssClass="logo" />
                </asp:HyperLink>

            </div>

            <!-- Mid -->
            <div class="middle-section" id="middle-section">
                <div class="form-module">
                    Title:
                    <asp:TextBox ID="title" runat="server" type="text" required></asp:TextBox><br />
                    Phone Number:
                    <asp:TextBox ID="phone" runat="server" type="text" required></asp:TextBox><br />
                    Payment in BGN:
                    <asp:TextBox ID="payment" runat="server" type="text" required></asp:TextBox><br />
                    
                    <div class="category">

                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="styled-dropdown">
                            <asp:ListItem Text="Select" Value="Select" />

                        </asp:DropDownList>

                        <asp:TextBox ID="txtNewCategory" runat="server" Style="display: none;" placeholder="Enter new category"></asp:TextBox>
                        <asp:Button ID="btnAddCategory" runat="server" Style="display: none;" Text="Add New Category" OnClick="btnAddCategory_Click" UseSubmitBehavior="false"/>

                        <asp:Button ID="btnToggleCategory" runat="server" Text="+" OnClientClick="toggleCategoryVisibility();return false;" UseSubmitBehavior="false" />

                    </div>

                    Description:
                    <asp:TextBox ID="description" runat="server" TextMode="MultiLine" required></asp:TextBox><br />


                    <asp:Button ID="newJob" runat="server" Text="Add Job" OnClick="btnNewJob_Click"  CssClass="button"/>
                </div>
            </div>

            <!-- Right -->
            <div class="right-section">

                <!-- Track offers -->
                <input type="hidden" id="jobOffset" value="9" />

                <div class="dropdown">
                    <!-- User Name -->
                    <div class="user-name">
                        <span onclick="toggleDropdown()">
                            <asp:Literal ID="litUserName" runat="server"></asp:Literal></span>
                    </div>

                    <div class="dropdown-content" id="myDropdown">

                        <!--If Employer-->
                        <asp:HyperLink class="button" ID="profileUser" runat="server" NavigateUrl="" Visible="false">View Profile</asp:HyperLink>

                        <!--If Employer-->
                        <asp:HyperLink class="button" ID="lnkAddPost" runat="server" NavigateUrl="newJob.aspx" Visible="false">Add Post</asp:HyperLink>

                        <asp:Button class="button" ID="btnSignOut" runat="server" Text="Sign Out" OnClick="btnSignOut_Click" Style="color: red" />
                    </div>
                </div>

            </div>

        </div>

    </form>
    
</body>
</html>
