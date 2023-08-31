<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StationaryRequest.aspx.cs" Inherits="BSI_Practice.Pages.StationaryRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/StationaryRequest.css"/>

    <script src="../Scripts/AngularJS/angular.min.js"></script>
    <script src="../Scripts/AngularJS/angular-filter.js"></script>
    <script src="../Scripts/Page/StationaryRequest.js"></script>
    <form ng-app="StationaryRequestPage" ng-controller="StatinoaryRequestController">
        <div class="row" style="padding-top: 1%">
            <div class="row">
                <div class="col-9" style="display: flex;">
                    <div class="row">
                        <div class="col-2">
                            <img class="logo" src="../Images/logo.png"/>
                        </div>
                        <div class="col">
                            <p id="logo_atas">Mitsubishi Motors Authorized Distributor</p>
                            <p id="logo_bawah">PT Mitsubishi Motors Krama Yudha Sales Indonesia</p>
                        </div>
                    </div>
                </div>
                <div class="col-3">
                    <p class="myParagraph">Stationary Request</p>
                </div>
            </div>


            <%--This is the Request header--%>
            <div class="row">
                <label class="myLabel">Application ID</label>
                <hr class="separator"/>
                <div class="col-5 border-col-6">
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Folio No</label>
                        </div>
                        <div class="col-8">
                            <input id="folio_no" class="header-input" type="text" runat="server"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Applicant</label>
                        </div>
                        <div class="col-8">
                            <input id="applicant" class="header-input" type="text" runat="server"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Department</label>
                        </div>
                        <div class="col-8">
                            <input id="department" class="header-input" type="text" runat="server"/>
                        </div>
                    </div>
                </div>
                <div class="col-1"></div>
                <div class="col-5 border-col-6">
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Role</label>
                        </div>
                        <div class="col-8">
                            <input id="role" class="header-input" type="text" runat="server"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Employee ID</label>
                        </div>
                        <div class="col-8">
                            <input id="employee_id" class="header-input" type="text" runat="server"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Extension</label>
                        </div>
                        <div class="col-8">
                            <input id="extension" class="header-input" type="text" runat="server"/>
                        </div>
                    </div>
                </div>
            </div>

            <%--This is the request Detail--%>
            <div class="row">
                <label class="myLabel-detail">Request Detail</label>
                <hr class="separator"/>
                <table class="myTable">
                    <thead>
                        <tr>
                            <th>No.</th>
                            <th>Item Name</th>
                            <th>UOM</th>
                            <th>Stock</th>
                            <th>Request QTY</th>
                            <th>Reason</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</asp:Content>
