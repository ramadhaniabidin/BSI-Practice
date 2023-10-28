document.addEventListener("DOMContentLoaded", function (event) {

    const showNavbar = (toggleId, navId, bodyId, headerId) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId)

        // Validate that all variables exist
        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', () => {
                // show navbar
                nav.classList.toggle('show')
                // change icon
                toggle.classList.toggle('bx-x')
                // add padding to body
                bodypd.classList.toggle('body-pd')
                // add padding to header
                headerpd.classList.toggle('body-pd')
            })
        }
    }

    showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')

    /*===== LINK ACTIVE =====*/
    const linkColor = document.querySelectorAll('.nav_link')

    function colorLink() {
        if (linkColor) {
            linkColor.forEach(l => l.classList.remove('active'))
            this.classList.add('active')
        }
    }
    linkColor.forEach(l => l.addEventListener('click', colorLink))

    // Your code to run since DOM is loaded and ready
});

var app = angular.module("MasterPage", []);

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

app.controller("MasterPageController", function ($scope, svc) {
    // Variable declaration
    $scope.username = "";

    $scope.GetCurrentLoginData = function () {
        var token = sessionStorage.getItem('LoginToken');
        var promise = svc.svc_GetCurrentLoginData(token);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            var userData = response_data.currentLoginData;
            $scope.username = userData.name;
        });
    };

    $scope.SignOut = function () {
        sessionStorage.removeItem('LoginToken');
        location.href = "/Pages/Login";
    };

    $scope.GetCurrentLoginData();
});

