var app = angular.module("HomePage", []);

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
        //console.log(param);

        return response;
    }

    this.svc_GetRequestList = function (current_login_role_id) {
        var param = {
            'current_login_role_id': current_login_role_id
        }
        console.log(param);
        var response = $http({
            method: 'POST',
            url: '/WebServices/HomeWebService.asmx/GetRequestList',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    };
});

app.controller("HomeController", function ($scope, svc) {
    $scope.RequestList = [];

    GetRequestList = function (current_role_id) {
        var promise = svc.svc_GetRequestList(current_role_id);
        promise.then(function (response) {
            var jsonData = JSON.parse(response.data.d);
            console.log("JSON Data = ", jsonData);
            $scope.RequestList = jsonData.ListItems;
            for (i of $scope.RequestList) {
                i.created_date = new Date(parseInt(i.created_date.substring(6)));
                i.created_date = i.created_date.toLocaleString();
            }
            console.log('Request List = ', $scope.RequestList);
        });
    };

    $scope.GetCurrentLoginData = function () {
        var token = sessionStorage.getItem('LoginToken');
        var promise = svc.svc_GetCurrentLoginData(token);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            var userData = response_data.currentLoginData;
            //console.log('User data: ', userData);
            var role_id = userData.role_id;

            //console.log(role_id);
            GetRequestList(role_id);
        });
    };


    $scope.GetCurrentLoginData();


    //var currentLoginRoleId = localStorage.getItem("RoleId");
    //console.log(currentLoginRoleId);
    //GetRequestList(currentLoginRoleId);
});