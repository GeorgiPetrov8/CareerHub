<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="CareerHub.profile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>CareerHub</title>

    <link href="CSS\index.css" rel="stylesheet" />
    <link href="CSS\profile.css" rel="stylesheet" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="JS/loadUser.js" type="text/javascript"></script>
    <script src="JS/toggle_dropdown.js" type="text/javascript"></script>
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
             <div runat="server" id="profileContainer" class="profileContainer"></div>
             <div class="mainContent" runat="server" id="mainContent"></div>
            
             <asp:PlaceHolder ID="DynamicContent" runat="server"></asp:PlaceHolder>
             <asp:HiddenField ID="hiddenJobId" runat="server" />
             <asp:Button ID="hiddenDeleteButton" runat="server" OnClick="DeleteJob" Style="display:none;" />

         </div>

         <!-- Right -->
         <div class="right-section">

             <div class="dropdown">
                 <!-- User Name -->
                 <div class="user-name">
                     <span onclick="toggleDropdown()"><asp:Literal ID="litUserName" runat="server"></asp:Literal></span>
                 </div>

                 <div class="dropdown-content" id="myDropdown">

                     <!--If Employer-->
                     <asp:HyperLink class="button" ID="profileUser" runat="server" NavigateUrl="" Visible="false">View Profile</asp:HyperLink>

                     <!--If Employer-->
                     <asp:HyperLink class="button" ID="lnkAddPost" runat="server" NavigateUrl="newJob.aspx" Visible="false">Add Post</asp:HyperLink>

                     <asp:Button class="button" ID="btnSignOut" runat="server" Text="Sign Out" OnClick="btnSignOut_Click" style="color:red"/>
                 </div>
             </div>

         </div>

     </div>
 </form>
</body>
</html>
