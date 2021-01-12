<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MFG_DigitalApp.Login" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>MFG Digital App</title>
    <script src="~/Scripts/plugins/jquery.js"></script>
    <!-- Bootstrap 3.3.6 -->
    <link href="~/Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link href="~/Content/AdminLTE.min.css" rel="stylesheet" />
    <link href="~/Content/blue.css" rel="stylesheet" />
    <link href="~/Content/Site.css" rel="stylesheet" />
    <script src="http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.2/modernizr.js"></script>
    <style>
        .box {
            position: relative;
            border-radius: 3px;
            background: #ffffff;
            border-top: 3px solid #d2d6de;
            margin-bottom: 20px;
            width: 100%;
            box-shadow: 0 0px 0px rgba(0,0,0,0.1);
        }

        .content-wrapper > .container {
            background-color: #ECF0F5;
            /*min-height:600px;*/
        }
    </style>
</head>
<body style="background-image: url('\Content\Images\Background.png')">
    <form runat="server">
        <section class="content">
            <div class="col-md-12 col-xs-12">
                <div class="login-box" style="margin: 8% auto">
                    <div class="login-box-body">
                        <div class="login-logo">
                            <img src="Content/Images/logo.png" />
                        </div>
                        <div class="has-feedback">
                            <asp:TextBox ID="txtUserName" runat="server" placeholder="Username"
                                CssClass="form-control required"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName"
                                CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                ValidationGroup="LoginUserValidationGroup" ForeColor="Red">* Enter the Username</asp:RequiredFieldValidator>
                        </div>
                        <div class="has-feedback">
                            <asp:TextBox ID="txtPassword" CssClass="form-control required" runat="server" TextMode="Password"
                                placeholder="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
                                CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                                ValidationGroup="LoginUserValidationGroup" ForeColor="Red">* Enter the Password</asp:RequiredFieldValidator>
                        </div>
                        <div class="row">
                            <div class="col-xs-7">
                            </div>
                            <div class="col-xs-5">
                                <asp:Button ID="CmdLogin" class="postClass btn btn-primary btn-block btn-flat" runat="server"
                                    CommandName="Login" OnClick="CmdLogin_Click" Text="Log In" ValidationGroup="LoginUserValidationGroup" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <script src="~/Scripts/bootstrap/bootstrap.min.js"></script>
        <script src="~/Scripts/plugins/icheck.js"></script>
    </form>
</body>
</html>
