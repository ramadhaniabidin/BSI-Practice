var app = angular.module('LoginPage', []);


app.service("svc", function ($http) {

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
});