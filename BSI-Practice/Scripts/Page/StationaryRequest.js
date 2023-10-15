var app = angular.module('StationaryRequestPage', []);

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

    this.svc_GetStationaryItems = function () {
        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetStationaryItems',
            data: {},
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    }

    this.svc_GetStockAndUnit = function (item_name) {
        var param = {
            'item_name': item_name
        };

        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetStockAndUnit',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    }
});

app.controller("StatinoaryRequestController", function ($scope, svc) {
    // Variable declaration
    $scope.folio_no = "Generated on Submit";
    $scope.applicant = "";
    $scope.department = "";
    $scope.role = "";
    $scope.employee_id = "";
    $scope.extension = "";
    $scope.role_id = -1;
    $scope.request_status_id = 0;

    $scope.IsRequestor = false;

    $scope.rows = [{
        item_name: '',
        no: '',
        uom: '',
        stock: '',
        request_qty: 0,
        reason: '',
        WarningMessage: false
    }];
    // End region

    // This function is for retrieving user data
    $scope.GetCurrentLoginData = function () {
        var token = sessionStorage.getItem('LoginToken');
        var promise = svc.svc_GetCurrentLoginData(token);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            //console.log('Response data : ', response_data);
            var userData = response_data.currentLoginData;
            console.log('User data: ', userData);
            $scope.applicant = userData.name;
            $scope.department = userData.department;
            $scope.role = userData.role;
            $scope.employee_id = userData.id;
            $scope.role_id = userData.role_id;

            if ($scope.role_id == 0) {
                $scope.IsRequestor = true;
            }
            //console.log(response_data);
        });
    };
    // End region

    // This function is for checking the request detail table
    $scope.CekRequestDetails = function () {
        console.log('Request Detail: ', $scope.rows);
    };
    // End region

    // This function enables the user to add row in request detail table
    $scope.AddRow = function () {
        $scope.rows.push({
            item_name: '',
            no: '',
            uom: '',
            stock: '',
            request_qty: 0,
            reason: '',
            WarningMessage: false
        });
    };
    // End region

    // This function is for retrieving stationary items from database
    $scope.GetStationaryItems = function () {
        var promise = svc.svc_GetStationaryItems();
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            var StationaryItems = response_data.StationaryItems;
            $scope.itemNames = [];
            for (i of StationaryItems) {
                if (!$scope.itemNames.includes(i.item_name)) {
                    $scope.itemNames.push(i.item_name);
                }
            }
        });
    };
    // End region

    // This function is for retieving stock and unit of a particular item
    $scope.GetStockAndUnit = function (index) {
        var item_name = $scope.rows[index].item_name;
        var promise = svc.svc_GetStockAndUnit(item_name);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            var StockAndUnit = response_data.StockAndUnit;
            $scope.rows[index].uom = StockAndUnit.uom;
            $scope.rows[index].stock = StockAndUnit.stock;
        });
    };
    // End region

    // THIS FUNCTION VALIDATES THE REQUEST QUANTITY
    $scope.CekRequestQty = function (index) {
        
        var stock = $scope.rows[index].stock;
        var req_qty = $scope.rows[index].request_qty;

        console.log('stock = ', stock);
        console.log('request quantity = ', req_qty);

        if (req_qty > stock) {
            $scope.rows[index].WarningMessage = true;
        }

        else {
            $scope.rows[index].WarningMessage = false;
        }
    }
    // END REGION

    var folio_no = GetQueryString()["folio_no"]
    console.log('Folio No: ', folio_no);

    if ((folio_no === null) || (folio_no === undefined) || (folio_no === '')) {
        $scope.request_status_id = 0;
        console.log("Request status id : ", $scope.request_status_id);
        $scope.GetCurrentLoginData();
        $scope.GetStationaryItems();
    }

    
    
});