<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AfterLogin.aspx.cs" Inherits="BSI_Practice.Pages.AfterLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/Home.css"/>
    <span><%= Session["UserID"] %></span>
</asp:Content>
