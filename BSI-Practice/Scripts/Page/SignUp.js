const app = angular.module('SignUpPage', []);

app.service("svc", function ($http) {
    this.svc_SignUp = function (email, password) {
        const param = {
            'email': email,
            'password': password
        };
        return $http({
            method: 'POST',
            url: '/WebServices/LoginWebService.asmx/SignUp',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
    };
});

app.controller("SignUpController", function ($scope, svc) {
    $scope.email_input = '';
    $scope.password_input = '';
    $scope.password_verif = '';

    $scope.isDisabled = false;
    $scope.PasswordLengthValid = true;

    $scope.onKeyUp = () => {
        $scope.isDisabled = !($scope.email_input) || !($scope.password_input) || !($scope.password_verif);
    };

    $scope.PasswordOnKeyUp = () => {
        $scope.isDisabled = !($scope.email_input) || !($scope.password_input) || !($scope.password_verif);
        $scope.PasswordLengthValid = $scope.isPasswordLengthValid();
    };

    $scope.Load = () => {
        $scope.isDisabled = !($scope.email_input);
    };

    $scope.isPasswordLengthValid = () => {
        if (!$scope.password_input) {
            return true;
        }
        return $scope.password_input.length >= 8;
    }

    $scope.SignUp = () => {
        if ($scope.password_input !== $scope.password_verif) {
            alert("Password does not match!");
            return;
        }
        if ($scope.password_input.length < 8) {
            alert("Password must contains at least 8 characters!");
            return;
        }
        const promise = svc.svc_SignUp($scope.email_input, $scope.password_input);
        promise.then(function (resp) {
            const responseJSON = JSON.parse(resp.data.d);
            if (!responseJSON.Success) {
                alert(responseJSON.InfoMessage);
                return;
            }
            alert("Sign Up success!");
            location.href = "/Pages/Login";
        }).catch(function (err) {
            console.log(err);
        });
    };

    $scope.Load();
});