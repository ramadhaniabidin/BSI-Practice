var app = angular.module('StationaryRequestPage', []);

app.service("svc", function ($http) {

});

app.controller("StatinoaryRequestController", function ($scope, svc) {
    // Variable declaration
    $scope.folio_no = "Generated on Submit";
    $scope.applicant = "";
    $scope.department = "";
    $scope.role = "";
    $scope.employee_id = "";
    $scope.extension = "";
    // End region
});