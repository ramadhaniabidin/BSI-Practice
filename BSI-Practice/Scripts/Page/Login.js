var app = angular.module('LoginPage', []);


app.service("svc", function ($http) {
    this.svc_GetLoginToken = function (email) {
        var param = {
            'email': email
        };

        var response = $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/GenerateLoginToken1',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    }

    this.svc_HashPassword = function (password) {
        const param = {
            'password': password
        };
        return $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/HashPassword',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };

    this.svc_VerifyPassword = function (inputPassword, hashedPassword) {
        const param = {
            inputPassword: inputPassword,
            hashedPassword: hashedPassword
        };

        return $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/VerifyPassword',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };

    this.svc_TestAppConfig = function () {
        return $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/TextAppConfig',
            data: {},
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };

    this.svc_TestConnectionString = function () {
        return $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/TestConnectionString',
            data: {},
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };

    this.svc_TestFetchItems = function () {
        return $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/TestFetchItems',
            data: {},
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };
});

app.controller('LoginController', function ($scope, svc) {
    //Variable Declaration
    $scope.login_email = "";
    $scope.hashed_password = "$2a$11$o2an1pMmBvZn8qkWMNJG4OvFf7zvKgJSV.BWsO45i/LQR5Z2OCfLu";
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
                sessionStorage.setItem('LoginToken', response_data.LoginToken);
                localStorage.setItem('LoginToken', response_data.LoginToken);
                //alert('Login Success with token: ' + response_data.LoginToken);
                location.href = "/Pages/Home";
            }
            else {
                alert(response_data.Message);
            }
        });
    }
    //End region

    $scope.HashPassword = function () {
        svc.svc_HashPassword($scope.login_email)
            .then(function (response) {
                const responseData = JSON.parse(response.data.d);
                if (responseData.Success) {
                    console.log(responseData);
                } else {
                    alert(response_data.Message);
                }
            }).catch(function (err) {
                alert(err);
            });
    };

    $scope.VerifyPassword = function () {
        svc.svc_VerifyPassword($scope.login_email, $scope.hashed_password)
            .then(function (response) {
                const responseData = JSON.parse(response.data.d);
                if (responseData.Success) {
                    console.log(responseData);
                } else {
                    console.log(response_data.Message);
                }
            }).catch(function (err) {
                console.log(err);
            });
    };

    $scope.TestAppConfig = function () {
        svc.svc_TestAppConfig()
            .then(function (response) {
                const responseJSON = JSON.parse(response.data.d);
                if (responseJSON.Success) {
                    console.log(responseJSON);
                } else {
                    console.log(responseJSON.Message);
                }
            }).catch(function (err) {
                console.log(err);
            });
    };

    $scope.TestConnectionString = function () {
        svc.svc_TestConnectionString()
            .then(function (response) {
                const responseJSON = JSON.parse(response.data.d);
                if (responseJSON.Success) {
                    console.log(responseJSON);
                } else {
                    console.log(responseJSON.Message);
                }
            }).catch(function (err) {
                console.log(err);
            });
    };

    $scope.TestFetchItems = function () {
        svc.svc_TestFetchItems()
            .then(function (response) {
                const responseJSON = JSON.parse(response.data.d);
                if (responseJSON.Success) {
                    console.log(responseJSON);
                } else {
                    console.log(responseJSON.Message);
                }
            }).catch(function (err) {
                console.log(err);
            });
    };

});