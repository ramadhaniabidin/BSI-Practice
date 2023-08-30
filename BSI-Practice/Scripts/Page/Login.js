var app = angular.module('LoginPage', []);


app.service("svc", function ($http) {
    this.svc_GetLoginToken = function (email) {
        var param = {
            'email': email
        };

        var response = $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/GenerateLoginToken',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    }
});

app.controller('LoginController', function ($scope, svc) {
    //Variable Declaration
    $scope.login_email = "";
    //End Region

    //This function is for displaying alert with the email in it
    $scope.AlertLoginButton = function () {
        alert('Hi ' + $scope.login_email + '!. Nice to see you again');
    }
    //End Region

    //This function is for generating Login Token
    $scope.LoginAction = function () {
        var promise = svc.svc_GetLoginToken($scope.login_email);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            console.log('Response Data: ', response_data);
            if (response_data.Success) {
                alert('Login Success with token: ' + response_data.LoginToken);
                location.href = "/Default";
            }
            else {
                alert(response_data.Message);
            }
        });
    }
    //End region
});