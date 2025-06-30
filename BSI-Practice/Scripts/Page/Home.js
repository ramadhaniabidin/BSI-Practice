const app = angular.module("HomePage", []);

app.service("svc", function ($http) {
    this.svc_GetRequestList = function () {
        const response = $http({
            method: 'POST',
            url: '/WebServices/HomeWebService.asmx/GetRequestList',
            data: {},
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    };
});

app.controller("HomeController", function ($scope, svc) {
    $scope.RequestList = [];

    $scope.GetRequestList = function () {
        const promise = svc.svc_GetRequestList();
        promise.then(function (response) {
            const jsonData = JSON.parse(response.data.d);
            $scope.RequestList = jsonData.ListItems;
            for (const [index, item] of $scope.RequestList.entries()) {
                const progress = Math.round((item.Current_Stage / item.Total_Stage) * 100);
                $scope.RequestList[index].Progress = progress;
            }
            console.log($scope.RequestList);
        });
    };
    $scope.GetRequestList();
});