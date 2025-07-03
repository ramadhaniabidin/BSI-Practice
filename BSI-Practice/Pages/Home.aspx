<%@ Page Title="Home" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BSI_Practice.Pages.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/Home.css"/>

    <script src="../Scripts/AngularJS/angular.min.js"></script>
    <script src="../Scripts/AngularJS/angular-filter.js"></script>
    <script src="../Scripts/Page/Home.js"></script>

    <div style="padding-top: 5%" ng-app="HomePage" ng-controller="HomeController">
        
        <div class="main-content">
            <div class="row mt-5">
                <div class="col">
                    <%--Modal--%>
                    <div id="history-log" class="my-modal">
                        <div id="modal-content" class="modal-content">
                            <div class="modal-header">
                                <h2 class="mb-0">Histroy Log: {{Modal_Header}}</h2>
                                <span class="close-modal" ng-click="CloseModal()">&times;</span>
                            </div>
                            <div class="modal-body py-3">
                                <table class="table align-items-center table-dark table-flush">
                                    <thead class="thead-dark">
                                      <tr>
                                          <th scope="col">Person</th>
                                          <th scope="col">Person</th>
                                          <th scope="col">Comments</th>
                                          <th scope="col">Action</th>
                                          <th scope="col">Date</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                        <tr ng-repeat="i in HistoryLogs">
                                            <td>{{i.PIC_Name}}</td>
                                            <td>{{i.PIC_Name}}</td>
                                            <td>{{i.Comment}}</td>
                                            <td>{{i.Action_Name}}</td>
                                            <td>{{i.Action_Date}}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="my-modal-footer">
                                <span>Status: {{Modal_Footer}}</span>
                            </div>
                        </div>
                    </div>

                    <!-- Filter Form -->
                    <form class="row g-3 align-items-center mb-3" style="padding-left: 10px;">
                        <div class="col-auto">
                            <label for="filterBy" class="col-form-label">Filter By</label>
                        </div>
                        <div class="col-auto">
                            <select id="filterBy" class="form-select">
                                <option value="folioNo">Folio No</option>
                                <option value="status">Status</option>
                                <option value="submittedDate">Submitted Date</option>
                                <option value="currentApprover">Current Approver</option>
                            </select>
                        </div>
                        <div class="col-auto">
                            <label for="keyword" class="col-form-label">Keyword</label>
                        </div>
                        <div class="col-auto">
                            <input type="text" id="keyword" class="form-control" placeholder="Enter keyword">
                        </div>
                        <div class="col-auto">
                            <button type="submit" class="btn btn-primary">Search</button>
                        </div>
                    </form>
                    <div class="card bg-default shadow">
                        <div class="card-header bg-transparent border-0">
                            <h3 class="text-white mb-0">Request List</h3>
                        </div>
                        <div class="table-responsive">
                            <table class="table align-items-center table-dark table-flush">
                                <thead class="thead-dark">
                                  <tr>
                                    <th scope="col">Transaction Number</th>
                                    <th scope="col">Requestor</th>
                                    <th scope="col">Status</th>
                                    <th scope="col">Current Approver</th>
                                    <th scope="col">Created Date</th>
                                  </tr>
                                </thead>
                                <tbody>
                                    <tr ng-repeat="i in RequestList">
                                        <td scope="row">
                                            <div class="media align-items-center">
                                                <div class="media-body">
                                                    <a target="_blank" href="/Pages/StationaryRequest.aspx?ID={{i.ID}}">
                                                        <span class="mb-0 text-sm">{{i.Transaction_Number}}</span>
                                                    </a>
                                                </div>
                                            </div>
                                        </td>
                                        <td scope="row">{{i.Requestor}}</td>
                                        <td scope="row" ng-click="HistoryLog(i.ID)">
                                            <span class="badge badge-dot mr-4" style="cursor: pointer">
                                                <i class="{{i.CSS_Class}}"></i> {{i.Status}}
                                            </span>
                                        </td>
                                        <td scope="row">{{i.Current_Approver}}</td>
                                        <td scope="row">{{i.Created_Date}}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <br /><br />
                    <div class="card bg-default shadow">
                        <div class="card-header bg-transparent border-0">
                            <h3 class="text-white mb-0">Request List</h3>
                        </div>
                        <div class="table-responsive">
                            <table class="table align-items-center table-dark table-flush">
                                <thead class="thead-dark">
                                  <tr>
                                    <th scope="col">Project</th>
                                    <th scope="col">Budget</th>
                                    <th scope="col">Status</th>
                                    <th scope="col">Users</th>
                                    <th scope="col">Completion</th>
                                  </tr>
                                </thead>
                                <tbody>
                                  <tr>
                                    <th scope="row">
                                      <div class="media align-items-center">
                                        <a href="#" class="avatar rounded-circle mr-3">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/bootstrap.jpg">
                                        </a>
                                        <div class="media-body">
                                          <span class="mb-0 text-sm">Argon Design System</span>
                                        </div>
                                      </div>
                                    </th>
                                    <td>
                                      $2,500 USD
                                    </td>
                                    <td>
                                      <span class="badge badge-dot mr-4">
                                        <i class="bg-warning"></i> pending
                                      </span>
                                    </td>
                                    <td>
                                      <div class="avatar-group">
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Ryan Tompson">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-1-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Romina Hadid">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-2-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Alexander Smith">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-3-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Jessica Doe">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-4-800x800.jpg" class="rounded-circle">
                                        </a>
                                      </div>
                                    </td>
                                    <td>
                                      <div class="d-flex align-items-center">
                                        <span class="mr-2">60%</span>
                                        <div>
                                          <div class="progress">
                                            <div class="progress-bar bg-warning" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%;"></div>
                                          </div>
                                        </div>
                                      </div>
                                    </td>
                                  </tr>
                                  <tr>
                                    <th scope="row">
                                      <div class="media align-items-center">
                                        <a href="#" class="avatar rounded-circle mr-3">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/angular.jpg">
                                        </a>
                                        <div class="media-body">
                                          <span class="mb-0 text-sm">Angular Now UI Kit PRO</span>
                                        </div>
                                      </div>
                                    </th>
                                    <td>
                                      $1,800 USD
                                    </td>
                                    <td>
                                      <span class="badge badge-dot">
                                        <i class="bg-success"></i> completed
                                      </span>
                                    </td>
                                    <td>
                                      <div class="avatar-group">
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Ryan Tompson">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-1-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Romina Hadid">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-2-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Alexander Smith">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-3-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Jessica Doe">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-4-800x800.jpg" class="rounded-circle">
                                        </a>
                                      </div>
                                    </td>
                                    <td>
                                      <div class="d-flex align-items-center">
                                        <span class="mr-2">100%</span>
                                        <div>
                                          <div class="progress">
                                            <div class="progress-bar bg-success" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;"></div>
                                          </div>
                                        </div>
                                      </div>
                                    </td>
                                  </tr>
                                  <tr>
                                    <th scope="row">
                                      <div class="media align-items-center">
                                        <a href="#" class="avatar rounded-circle mr-3">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/sketch.jpg">
                                        </a>
                                        <div class="media-body">
                                          <span class="mb-0 text-sm">Black Dashboard</span>
                                        </div>
                                      </div>
                                    </th>
                                    <td>
                                      $3,150 USD
                                    </td>
                                    <td>
                                      <span class="badge badge-dot mr-4">
                                        <i class="bg-danger"></i> delayed
                                      </span>
                                    </td>
                                    <td>
                                      <div class="avatar-group">
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Ryan Tompson">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-1-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Romina Hadid">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-2-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Alexander Smith">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-3-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Jessica Doe">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-4-800x800.jpg" class="rounded-circle">
                                        </a>
                                      </div>
                                    </td>
                                    <td>
                                      <div class="d-flex align-items-center">
                                        <span class="mr-2">72%</span>
                                        <div>
                                          <div class="progress">
                                            <div class="progress-bar bg-danger" role="progressbar" aria-valuenow="72" aria-valuemin="0" aria-valuemax="100" style="width: 72%;"></div>
                                          </div>
                                        </div>
                                      </div>
                                    </td>
                                  </tr>
                                  <tr>
                                    <th scope="row">
                                      <div class="media align-items-center">
                                        <a href="#" class="avatar rounded-circle mr-3">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/react.jpg">
                                        </a>
                                        <div class="media-body">
                                          <span class="mb-0 text-sm">React Material Dashboard</span>
                                        </div>
                                      </div>
                                    </th>
                                    <td>
                                      $4,400 USD
                                    </td>
                                    <td>
                                      <span class="badge badge-dot">
                                        <i class="bg-info"></i> on schedule
                                      </span>
                                    </td>
                                    <td>
                                      <div class="avatar-group">
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Ryan Tompson">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-1-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Romina Hadid">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-2-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Alexander Smith">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-3-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Jessica Doe">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-4-800x800.jpg" class="rounded-circle">
                                        </a>
                                      </div>
                                    </td>
                                    <td>
                                      <div class="d-flex align-items-center">
                                        <span class="mr-2">90%</span>
                                        <div>
                                          <div class="progress">
                                            <div class="progress-bar bg-info" role="progressbar" aria-valuenow="90" aria-valuemin="0" aria-valuemax="100" style="width: 90%;"></div>
                                          </div>
                                        </div>
                                      </div>
                                    </td>
                                  </tr>
                                  <tr>
                                    <th scope="row">
                                      <div class="media align-items-center">
                                        <a href="#" class="avatar rounded-circle mr-3">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/vue.jpg">
                                        </a>
                                        <div class="media-body">
                                          <span class="mb-0 text-sm">Vue Paper UI Kit PRO</span>
                                        </div>
                                      </div>
                                    </th>
                                    <td>
                                      $2,200 USD
                                    </td>
                                    <td>
                                      <span class="badge badge-dot mr-4">
                                        <i class="bg-success"></i> completed
                                      </span>
                                    </td>
                                    <td>
                                      <div class="avatar-group">
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Ryan Tompson">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-1-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Romina Hadid">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-2-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Alexander Smith">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-3-800x800.jpg" class="rounded-circle">
                                        </a>
                                        <a href="#" class="avatar avatar-sm" data-toggle="tooltip" data-original-title="Jessica Doe">
                                          <img alt="Image placeholder" src="https://raw.githack.com/creativetimofficial/argon-dashboard/master/assets/img/theme/team-4-800x800.jpg" class="rounded-circle">
                                        </a>
                                      </div>
                                    </td>
                                    <td>
                                      <div class="d-flex align-items-center">
                                        <span class="mr-2">100%</span>
                                        <div>
                                          <div class="progress">
                                            <div class="progress-bar bg-success" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;"></div>
                                          </div>
                                        </div>
                                      </div>
                                    </td>
                                  </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    
    
</asp:Content>
