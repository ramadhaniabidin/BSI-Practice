﻿ <%@ Page Title="Stationary Request" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StationaryRequest.aspx.cs" Inherits="BSI_Practice.Pages.StationaryRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/StationaryRequest.css"/>
    <script src="../Scripts/jquery-3.4.1.min.js"></script>
    <script src="../Scripts/AngularJS/angular.min.js"></script>
    <script src="../Scripts/AngularJS/angular-filter.js"></script>
    <script src="../Scripts/Page/StationaryRequest.js"></script>
    <script src="../Scripts/BaseLogic.js"></script>
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
                            <input id="folio_no" class="header-input" type="text" runat="server" ng-model="request.header.folio_no" readonly="readonly"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Applicant</label>
                        </div>
                        <div class="col-8">
                            <input id="applicant" class="header-input" type="text" runat="server" ng-model="request.header.applicant" readonly="readonly"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Department</label>
                        </div>
                        <div class="col-8">
                            <input id="department" class="header-input" type="text" runat="server" ng-model="request.header.department" readonly="readonly"/>
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
                            <input id="role" class="header-input" type="text" runat="server" ng-model="request.header.role" readonly="readonly"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Employee ID</label>
                        </div>
                        <div class="col-8">
                            <input id="employee_id" class="header-input" type="text" runat="server" ng-model="request.header.employee_id" readonly="readonly"/>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-4">
                            <label class="header-label">Extension</label>
                        </div>
                        <div class="col-8">
                            <input id="extension" class="header-input" type="text" runat="server" ng-model="request.header.extension" readonly="readonly"/>
                        </div>
                    </div>
                </div>
            </div>

            <%--This is the request Detail--%>
            <div class="row">
                <label class="myLabel-detail">Request Detail</label>
                <hr class="separator"/>
                <table class="myTable" id="request_detail">
                    <thead>
                        <tr>
                            <th style="width: 3%">No.</th>
                            <th style="width: 25%">Item Name</th>
                            <th style="width: 10%">​​​​UOM</th>
                            <th style="width: 10%">Stock</th>
                            <th style="width: 10%">Request QTY</th>
                            <th style="width: 20%">Reason</th>
                            <th style="width: 5%"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="row in request.detail">
                            <td><p style="text-align:center">{{ $index + 1 }}</p></td>
                            <td>
                                <select ng-model="row.item_name" ng-change="GetStockAndUnit({{$index}})" class="item-names">
                                    <option value="" selected disabled style="text-align:center;"> == Select Stationary Item == </option>
                                    <option style="text-align:center;" ng-repeat="item in itemNames" value="{{item}}">{{item}}</option>
                                </select>
                            </td>
                            <td><input type="text" ng-model="row.uom" readonly="readonly"/></td>
                            <td><input type="text" ng-model="row.stock" readonly="readonly"/></td>
                            <td>
                                <p ng-show="row.WarningMessage1" style="color:red; margin-bottom:0px;">Permintaan Anda melebihi stok</p>
                                <p ng-show="row.WarningMessage2" style="color:red; margin-bottom:0px;">Jumlah permintaan tidak boleh kosong</p>
                                <input type="text" ng-model="row.request_qty" ng-change="CekRequestQty({{$index}})">
                            </td>
                            <td>
                                <textarea ng-model="row.reason"></textarea>
                            </td>
                            <td>
                                <i style="font-size:30px; cursor: pointer" class="bx bx-trash" ng-click="DeleteRow($index)"></i>
                            </td>
                        </tr>
                    </tbody>
                    <caption id="add" class="button-addRow" ng-click="AddRow()">
                        <span>&#43</span> Add New Row
                    </caption>
                </table>
            </div>

            <div class="row">
                <div class="col-2">
                    <strong>Next Approver : </strong>
                </div>
                <div class="col-5">
                    <select ng-model="next_approver">
                        <option value="" selected disabled style="text-align:center">== Choose The Next Approver ==</option>
                        <option style="text-align:center;" ng-repeat="app in approver_list" value="{{app}}">{{app}}</option>
                    </select>
                </div>
            </div>
            
            <%--Approval Action--%>
            <div class="row" ng-show="request_status_id == 3 && isCurrentApprover">
                <label class="myLabel">Approval Action</label>
                <hr class="separator"/>
                <div class="col-6">
                    <div class="row">
                        <div class="col-2">
                            <input type="radio" value="Approve" name="action" ng-model="ApprovalAction"/>
                            <label for="approve">Approve</label>
                        </div>
                        <div class="col-2">
                            <input type="radio" value="Reject" name="action" ng-model="ApprovalAction"/>
                            <label for="reject">Reject</label>
                        </div>
                    </div>
                </div>
                <div class="col-6">
                    <div>
                        <label>Comments:</label>
                    </div>
                    <div class="row" style="width: 70%; height:90%">
                        <textarea ng-model="comment"></textarea>
                    </div>                    
                </div>
            </div>
            
            <%--Buttons--%>
            <div class="row" style="padding-top:2.75%">
                <div class="col" style="padding-left: 0px">
                    <button ng-show="request_status_id == 0" id="submit-btn" class="btn btn-primary" ng-click="SubmitRequest()">Submit</button>
                    
                    <%--This button only appears if current login is requestor and status id of the request equals 5 (The request has been delivered)--%>
                    <button id="close-btn" class="btn btn-danger" ng-click="Close()">Close</button>

                    <%--This button only appears if current login is not requestor and status id of the request equals 2 (Waiting for approval process)--%>
                    <button ng-show="request_status_id == 2" id="approval-btn" class="btn btn-primary" ng-click="InsertApprovalLog()">Submit</button>

                    <%--This button only appears if current login is not requestor and status id of the request equals 3 (The request is fully approved)--%>
                    <button ng-show="request_status_id == 3" id="deliver-btn" class="btn btn-primary">Delivered</button>
                </div>

            </div>

            <%--<br /><br />--%>

            <%--Approval History Log--%>
            <div class="row" style="padding-top:1%" ng-show="request_status_id != 0">
                <label class="myLabel">Log History</label>
                <hr class="separator"/>
                <table class="myTable">
                    <thead>
                        <tr>
                            <th>No.</th>
                            <th>Name</th>
                            <th>Comment</th>
                            <th>Status</th>
                            <th>Time</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="wf in workflow_histories">
                            <td><p style="text-align:center">{{ $index + 1 }}</td>
                            <td><p style="text-align:center">{{wf.pic_name}}</p></td>
                            <td><p style="text-align: center">{{wf.comment}}</p></td>
                            <td><p style="text-align: center">{{wf.action_name}}</p></td>
                            <td><p style="text-align: center">{{wf.action_date}}</p></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <br /><br />
    </form>
    
    
</asp:Content>
