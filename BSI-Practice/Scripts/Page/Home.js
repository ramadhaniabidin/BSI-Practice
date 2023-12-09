var app = angular.module("HomePage", []);

app.service("svc", function ($http) {
    this.svc_GetRequestList = function (current_login_role_id) {
        var param = {
            'current_login_role_id': current_login_role_id
        }
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
    }

    
    GetRequestList(0);
});