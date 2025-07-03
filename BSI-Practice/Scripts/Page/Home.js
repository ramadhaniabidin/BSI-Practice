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

    this.svc_GetHistoryLogList = function (ID) {
        const param = { 'ID': ID };
        return $http({
            method: 'POST',
            url: '/WebServices/HomeWebService.asmx/GetHistoryLogList',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };
});

app.controller("HomeController", function ($scope, svc) {
    $scope.RequestList = [];
    $scope.HistoryLogs = [];
    $scope.Modal_Header = '';
    $scope.Modal_Footer = '';

    $scope.GetRequestList = function () {
        const promise = svc.svc_GetRequestList();
        promise.then(function (response) {
            const jsonData = JSON.parse(response.data.d);
            $scope.RequestList = jsonData.ListItems;
            for (const [index, item] of $scope.RequestList.entries()) {
                const progress = Math.round((item.Current_Stage / item.Total_Stage) * 100);
                $scope.RequestList[index].Progress = progress;
                $scope.RequestList[index].Created_Date = $scope.JSONDateToJavaScript($scope.RequestList[index].Created_Date);
            }
        });
    };

    $scope.OpenModal = () => {
        const modal = document.getElementById("history-log");
        const content = document.getElementById("modal-content");

        // Reset animation in case it was previously closed
        content.classList.remove("fade-out");
        modal.style.display = "block";
    };

    $scope.CloseModal = () => {
        const modal = document.getElementById("history-log");
        const content = document.getElementById("modal-content");

        // Add fade-out animation
        content.classList.add("fade-out");

        // Wait for the animation to finish before hiding
        setTimeout(() => {
            modal.style.display = "none";
            content.classList.remove("fade-out"); // reset for next open
        }, 300); // match animation duration
    };

    $scope.JSONDateToJavaScript = (jsonDateString) => {
        const d = new Date(parseInt(jsonDateString.replace('/Date(', '')));
        const result = $scope.DateFormat_ddMMMyyyy(d);
        return result;
    };

    $scope.DateFormat_ddMMMyyyy = (date) => {
        const monthNames = [
            "Jan", "Feb", "Mar",
            "Apr", "May", "Jun", "Jul",
            "Aug", "Sep", "Oct",
            "Nov", "Dec"
        ];
        const day = date.getDate();
        const monthIndex = date.getMonth();
        const year = date.getFullYear();
        const jam = date.getHours();
        const menit = date.getMinutes();
        const formatDate = day + '-' + monthNames[monthIndex] + '-' + year.toString();
        const strDateTime = [formatDate, [$scope.AddZero(jam), $scope.AddZero(menit)].join(":")].join(" ");
        return strDateTime;
    };

    $scope.AddZero = (num) => {
        return (num >= 0 && num < 10) ? "0" + num : num + "";
    };

    $scope.HistoryLog = (ID) => {
        const promise = svc.svc_GetHistoryLogList(ID);
        promise.then((resp) => {
            const jsonData = JSON.parse(resp.data.d);
            const HistoryLogs = jsonData.HistoryLog;
            for (const item of HistoryLogs) {
                item.Action_Date = $scope.JSONDateToJavaScript(item.Action_Date);
            }
            $scope.HistoryLogs = HistoryLogs;
            $scope.Modal_Header = $scope.HistoryLogs[0].Transaction_No;
            $scope.Modal_Footer = $scope.RequestList[ID - 1].Status + ' - ' + $scope.RequestList[ID - 1].Current_Approver_Role + ' - ' + $scope.RequestList[ID - 1].Current_Approver;
            $scope.OpenModal();
        }, (err) => {
            alert(err);
        });
    };


    $scope.GetRequestList();
});