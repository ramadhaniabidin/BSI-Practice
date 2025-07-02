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

    $scope.OpenModal = () => {
        const modal = document.getElementById("reset-password");
        const content = document.getElementById("modal-content");

        // Reset animation in case it was previously closed
        content.classList.remove("fade-out");
        modal.style.display = "block";
    };

    $scope.CloseModal = () => {
        const modal = document.getElementById("reset-password");
        const content = document.getElementById("modal-content");

        // Add fade-out animation
        content.classList.add("fade-out");

        // Wait for the animation to finish before hiding
        setTimeout(() => {
            modal.style.display = "none";
            content.classList.remove("fade-out"); // reset for next open
        }, 300); // match animation duration
    };
    $scope.GetRequestList();
});