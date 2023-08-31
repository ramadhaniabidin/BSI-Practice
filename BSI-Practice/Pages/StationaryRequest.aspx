﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StationaryRequest.aspx.cs" Inherits="BSI_Practice.Pages.StationaryRequest" %>
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
                            <input id="folio_no" class="header-input" type="text" runat="server" ng-model="folio_no"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Applicant</label>
                        </div>
                        <div class="col-8">
                            <input id="applicant" class="header-input" type="text" runat="server" ng-model="applicant"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Department</label>
                        </div>
                        <div class="col-8">
                            <input id="department" class="header-input" type="text" runat="server" ng-model="department"/>
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
                            <input id="role" class="header-input" type="text" runat="server" ng-model="role"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Employee ID</label>
                        </div>
                        <div class="col-8">
                            <input id="employee_id" class="header-input" type="text" runat="server" ng-model="employee_id"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Extension</label>
                        </div>
                        <div class="col-8">
                            <input id="extension" class="header-input" type="text" runat="server" ng-model="extension"/>
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
                            <th style="width: 3%">No.</th>
                            <th style="width: 25%">Item Name</th>
                            <th style="width: 10%">​​​​UOM</th>
                            <th style="width: 10%">Stock</th>
                            <th style="width: 10%">Request QTY</th>
                            <th style="width: 20%">Reason</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="row in rows">
                            <td><p style="text-align:center">{{ $index + 1 }}</p></td>
                            <td>
                                <select ng-model="row.item_name" ng-change="GetStockAndUnit({{$index}})">
                                    <option value="" selected disabled style="text-align:center;"> == Select Stationary Item == </option>
                                    <option style="text-align:center;" ng-repeat="item in itemNames" value="{{item}}">{{item}}</option>
                                </select>
                            </td>
                            <td><input type="text" ng-model="row.uom" readonly="readonly"/></td>
                            <td><input type="text" ng-model="row.stock" readonly="readonly"/></td>
                            <td>
                                <p ng-show="row.WarningMessage" style="color:red; margin-bottom:0px;">Permintaan Anda melebihi stok</p>
                                <input type="number" ng-model="row.request_qty" ng-change="CekRequestQty({{$index}})">
                            </td>
                            <td></td>
                        </tr>
                    </tbody>
                    <caption id="add" class="button-addRow" ng-click="AddRow()">
                        <span>&#43</span> Add New Row
                    </caption>
                </table>
            </div>
        </div>
        <br /><br />
        <button type="button" class="btn btn-primary" ng-click="CekRequestDetails()">Cek Details</button>
    </form>
    
    
</asp:Content>
