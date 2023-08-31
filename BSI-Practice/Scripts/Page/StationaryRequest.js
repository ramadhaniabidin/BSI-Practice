var app = angular.module('StationaryRequestPage', []);

app.service("svc", function ($http) {
    this.svc_GetCurrentLoginData = function (loginToken) {
        var param = {
            'loginToken': loginToken
        };

        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetCurrentLoginData',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        console.log(param);

        return response;
    }
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

    $scope.GetCurrentLoginData = function () {
        var token = sessionStorage.getItem('LoginToken');
        var promise = svc.svc_GetCurrentLoginData(token);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            //console.log('Response data : ', response_data);
            var userData = response_data.currentLoginData;
            console.log('User data: ', userData);
            $scope.applicant = userData.name;
            $scope.department = userData.department;
            $scope.role = userData.role;
            $scope.employee_id = userData.id;
            //console.log(response_data);
        });
    };

    $scope.GetCurrentLoginData();
});