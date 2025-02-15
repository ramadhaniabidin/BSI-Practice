﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="BSI_Practice.Pages.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Sign Up</title>

    <link rel="stylesheet" href="../Style/Fonts/FontAwesome/all.min.css"/>
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" />
    <link href="../Style/sb-admin-2.css" rel="stylesheet" />
    <link href="../Style/Login.css" rel="stylesheet"/>

    <script src="../Scripts/AngularJS/angular.min.js"></script>
    <script src="../Scripts/AngularJS/angular-filter.js"></script>
    <script src="../Scripts/Page/SignUp.js"></script>
</head>
<body style="background-color:rgb(22 22 21)" ng-app="SignUpPage" ng-controller="SignUpController">
    <form id="form1" runat="server">
        <div class="custom-centered">
            <div class="row justify-content-center">
                <div class="col-xl-10 col-lg-12 col-md-9">
                    <div class="card o-hidden border-0 shadow-lg my-5">
                        <div class="card-body p-0">
                            <div class="row">
                                <div class="col-lg-4">
                                    <img class="background-login" src="../Images/download.png"/>
                                </div>
                                <div class="col-lg-8">
                                    <div class="p-5">
                                        <div class="text-center">
                                            <h1 class="h4 text-gray-900 mb-4">Sign Up!</h1>
                                        </div>
                                        <div class="user">
                                            <div class="form-group">
                                                <input type="text" runat="server" class="form-control form-control-user" placeholder="Enter Email Address..." ng-model="email_input" ng-keyup="onKeyUp()"/>                                                
                                            </div>
                                            <div class="form-group">
                                                <input type="password" runat="server" class="form-control form-control-user" placeholder="Enter Your Password..." ng-model="password_input" ng-keyup="PasswordOnKeyUp()"/>
                                                <p ng-show="!PasswordLengthValid" style="text-align: center;">Password must contains at least 8 characters</p>
                                            </div>
                                            <div class="form-group">
                                                <input type="password" runat="server" class="form-control form-control-user" placeholder="Verify Password..." ng-model="password_verif" ng-keyup="onKeyUp()"/>
                                            </div>
                                            <button type="button" 
                                                class="btn btn-primary btn-user btn-block" ng-click="SignUp()" 
                                                ng-disabled="isDisabled">Sign Up</button>
                                        
                                            <div class="text-center">
                                                <p>Already have an account? <a href="Login.aspx">Sign In</a></p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
