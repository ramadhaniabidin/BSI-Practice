<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BSI_Practice.Pages.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/Home.css"/>

    <script src="../Scripts/AngularJS/angular.min.js"></script>
    <script src="../Scripts/AngularJS/angular-filter.js"></script>
    <script src="../Scripts/Page/Home.js"></script>

    <div style="padding-top: 1%" ng-app="HomePage" ng-controller="HomeController">
        <table>
            <thead class="table-head">
                <tr>
                    <th style="width: 2%">#</th>
                    <th style="width: 5%">Folio No</th>
                    <th style="width: 5%">Status</th>
                    <th style="width: 5%">Submitted By</th>
                    <th style="width: 5%">Submitted Date</th>
                    <th style="width: 5%">Current Approver</th>
                </tr>
            </thead>
            <tbody class="table-body">
                <tr ng-repeat="req in RequestList">
                    <td>{{$index + 1}}</td>
                    <td><a href="/Pages/StationaryRequest?folio_no={{req.folio_no}}">{{req.folio_no}}</a></td>
                    <td>{{req.status_name}}</td>
                    <td>{{req.created_by}}</td>
                    <td>{{req.created_date}}</td>
                    <td>{{req.approver_name}}</td>
                </tr>
            </tbody>
        </table>
    </div>
    
</asp:Content>
