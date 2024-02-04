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
        //console.log(param);

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
    };

    this.svc_GetApproverList = function () {
        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetApproverList',
            data: {},
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    };

    this.svc_SaveUpdate = function (header, details) {
        var param = {
            'header': header,
            'details': details
        };

        console.log('Param : ', param);

        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/SaveUpdate',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });

        return response;
    };

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
    };

    this.svc_GetRequestData = function (folio_no) {
        var param = {
            'folio_no': folio_no
        };
        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetRequestDataByJoin',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        return response;
    };

    this.svc_GetWorkflowHistories = function (folio_no) {
        var param = {
            'folio_no': folio_no
        };

        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetWorkflowHistories',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        return response;
    };

    this.svc_InsertApprovalLog = function (param) {
        var parameters = {
            model: param
        };

        var response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/InsertApprovalLog',
            data: JSON.stringify(parameters),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        return response;
    };

    this.svc_GetHeaderData = function (folio_no) {
        let param = {
            'folio_no': folio_no
        };
        let response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetHeaderData',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        return response;
    };

    this.svc_GetDetailsData = function (folio_no) {
        let param = {
            'folio_no': folio_no
        };
        let response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetDetailsData',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        return response;
    };

    this.svc_GetStatusID = function (folio_no) {
        let param = {
            'folio_no': folio_no
        };
        let response = $http({
            method: 'POST',
            url: '/WebServices/StationaryRequestWebService.asmx/GetCurrentStatusID',
            data: JSON.stringify(param),
            contentType: 'application/json; charset=utf-8',
            dataType: "json"
        });
        return response;
    };
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

    $scope.comment = '';
    $scope.ApprovalAction = '';

    $scope.username = "";

    $scope.isRequestValid = false;

    $scope.IsRequestor = false;

    $scope.rows = [{
        item_name: '',
        no: '',
        uom: '',
        stock: '',
        request_qty: 0,
        reason: '',
        WarningMessage1: false,
        WarningMessage2: false
    }];

    $scope.request = {
        header: {
            folio_no: '',
            applicant: '',
            department: '',
            role: '',
            employee_id: -1,
            extension: '',
            role_id: -1,
            status_id: -1
        },

        detail: [{
            item_name: '',
            no: '',
            uom: '',
            stock: '',
            request_qty: 0,
            reason: ''
        }]
    };

    $scope.workflow_histories = [{
        pic_name: '',
        comment: '',
        action_name: '',
        action_date: ''
    }];

    $scope.next_approver = "";
    // End region

    // This function is for retrieving user data
    $scope.GetCurrentLoginData = function () {
        var token = sessionStorage.getItem('LoginToken');
        var promise = svc.svc_GetCurrentLoginData(token);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            var userData = response_data.currentLoginData;
            console.log('User data: ', userData);
            $scope.role_id = userData.role_id;

            if ($scope.role_id == 0) {
                $scope.IsRequestor = true;
                $scope.request.header.folio_no = 'Generated On Submit';
                $scope.request.header.applicant = userData.name;
                $scope.request.header.department = userData.department;
                $scope.request.header.role = userData.role;
                $scope.request.header.employee_id = userData.id;
                $scope.request.header.extension = '';
            }
            //console.log(response_data);
        });
    };
    // End region

    // This function get the current login role id when folio_no is not null
    $scope.CurrentRoleId_WithFolioNo = function () {
        var token = sessionStorage.getItem('LoginToken');
        var promise = svc.svc_GetCurrentLoginData(token);
        promise.then(function (response) {
            var jsonData = JSON.parse(response.data.d);
            var userData = jsonData.currentLoginData;
            $scope.role_id = userData.role_id;
        });
    }
    // End region

    // This function is for checking the request detail table
    $scope.CekRequestDetails = function () {
        console.log('Request Detail: ', $scope.rows);
        // End region
    };
    // End region

    // This function enables the user to add row in request detail table
    $scope.AddRow = function () {
        $scope.request.detail.push({
            item_name: '',
            no: '',
            uom: '',
            stock: '',
            request_qty: 0,
            reason: '',
            //WarningMessage: false
        });
    };
    // End region

    // This function enables user to delete a particular row in the request detail
    $scope.DeleteRow = function (index) {
        $scope.request.detail.splice(index, 1);
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

    // This function is for retrieving the approvers
    $scope.GetApproverList = function () {
        let promise = svc.svc_GetApproverList();
        promise.then(function (response) {
            let JsonData = JSON.parse(response.data.d);
            let approver = JsonData.approver_name;
            //console.log('Json Data : ', JsonData);
            $scope.approver_list = [];
            for (i of approver) {
                if (!$scope.approver_list.includes(i)) {
                    $scope.approver_list.push(i);
                }
            }
        });
    };
    // End Region

    // This function is for retieving stock and unit of a particular item
    $scope.GetStockAndUnit = function (index) {
        var item_name = $scope.request.detail[index].item_name;
        var promise = svc.svc_GetStockAndUnit(item_name);
        promise.then(function (response) {
            var response_data = JSON.parse(response.data.d);
            console.log(response_data);
            var StockAndUnit = response_data.StockAndUnit;
            $scope.request.detail[index].uom = StockAndUnit.uom;
            $scope.request.detail[index].stock = StockAndUnit.stock;
        });
    };
    // End region

    // THIS FUNCTION VALIDATES THE REQUEST QUANTITY
    $scope.CekRequestQty = function (index) {

        var stock = $scope.request.detail[index].stock;
        var req_qty = $scope.request.detail[index].request_qty;

        console.log('stock = ', stock);
        console.log('request quantity = ', req_qty);

        // THIS CHECKS WHETHER THE REQUEST QUANTITY EXCEEDS THE STOCK
        var validation1 = req_qty > stock;
        // END REGION

        // THIS CHECKS WHETHER THE REQUEST QUANTITY IS EMPTY OR NULL
        var validation2 = ((req_qty == 0) || (req_qty == undefined) || (req_qty == null));
        // END REGION

        if (validation1) {
            //$scope.rows[index].WarningMessage1 = true;
            document.getElementById("submit-btn").classList.add("disabled");
        }

        else if (validation2) {
            //$scope.rows[index].WarningMessage2 = true;
            document.getElementById("submit-btn").classList.add("disabled");
        }


        else {
            //$scope.rows[index].WarningMessage1 = false;
            //$scope.rows[index].WarningMessage2 = false;
            var submit_btn = document.getElementById("submit-btn");
            if (submit_btn.classList.contains("disabled")) {
                submit_btn.classList.remove("disabled");
            }
        }
    }
    // END REGION

    // THIS FUNCTION VALIDATES THE OVERALL REQUEST SUBMISSION
    $scope.ValidateRequest = function () {
        var stock;
        var req_qty;
        var item_name;
        var submit_btn = document.getElementById("submit-btn");

        for (var i = 0; i < $scope.request.detail.length; i++) {
            stock = $scope.request.detail[i].stock;
            req_qty = $scope.request.detail[i].request_qty;
            item_name = $scope.request.detail[i].item_name;

            if (req_qty > stock) {
                alert("Permintaan Anda (baris ke " + (i + 1) + ") melebihi stok yang ada");
                $scope.isRequestValid = false;
            }

            if ((item_name == null) || (item_name == undefined)) {
                alert("Mohon pilih barang yang akan diminta (baris " + (i + 1) + ")");
                $scope.isRequestValid = false;
            }

            if ((req_qty == 0) || (req_qty == undefined) || (req_qty == undefined)) {
                alert("Jumlah barang yang diminta tidak boleh kosong (baris " + (i + 1) + ")");
                $scope.isRequestValid = false;
            }
        }

        if (($scope.next_approver == null) || ($scope.next_approver == undefined) || $scope.next_approver == '') {
            alert("Next Approver tidak boleh kosong!");
            $scope.isRequestValid = false;
        }

        else {
            $scope.isRequestValid = true;
        }

        if ($scope.isRequestValid == true) {
            let header_data = {};
            header_data.folio_no = $scope.request.header.folio_no;
            header_data.applicant = $scope.request.header.applicant;
            header_data.department = $scope.request.header.department;
            header_data.role = $scope.request.header.role;
            header_data.role_id = $scope.request.header.role_id;
            header_data.employee_id = $scope.request.header.employee_id;
            header_data.extension = $scope.request.header.extension;
            header_data.created_by = $scope.request.header.applicant;
            header_data.created_date = $scope.getCurrentDateTime();
            header_data.modified_by = $scope.request.header.applicant;
            header_data.modified_date = $scope.getCurrentDateTime();
            header_data.current_approver_role = $scope.next_approver;

            let detail_data = [];
            for (var i = 0; i < $scope.request.detail.length; i++) {
                detail_data.push({
                    item_name: $scope.request.detail[i].item_name,
                    no: i + 1,
                    uom: $scope.request.detail[i].uom,
                    stock: $scope.request.detail[i].stock,
                    request_qty: $scope.request.detail[i].request_qty,
                    reason: $scope.request.detail[i].reason,
                });
            }
            console.log('Param header: ', header_data);
            console.log('Param detail: ', detail_data);

            var promise = svc.svc_SaveUpdate(header_data, detail_data);
            promise.then(function (response) {
                let jsonData = JSON.parse(response.data.d);
                console.log('Json Data : ', jsonData);
                if (jsonData.Success) {
                    alert('Berhasil memasukkan data header dan detail');
                    location.href = "/Pages/Home";
                }
                else {
                    alert(jsonData.Message);
                }
            });
        }
    }
    // END REGION

    // Get current date time function
    $scope.getCurrentDateTime = function() {
        var currentDate = new Date();
        var year = currentDate.getFullYear();
        var month = String(currentDate.getMonth() + 1).padStart(2, '0'); // Adding 1 to the month and zero-padding
        var day = String(currentDate.getDate()).padStart(2, '0'); // Zero-padding
        var hours = String(currentDate.getHours()).padStart(2, '0'); // Zero-padding
        var minutes = String(currentDate.getMinutes()).padStart(2, '0'); // Zero-padding
        var seconds = String(currentDate.getSeconds()).padStart(2, '0'); // Zero-padding

        return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
    }
    // end region

    // This function shows the header and detail data that are about to be submitted
    $scope.CheckSubmittedData = function () {
        let header_data = {};
        header_data.folio_no = $scope.folio_no;
        header_data.applicant = $scope.applicant;
        header_data.department = $scope.department
        header_data.role = $scope.role;
        header_data.role_id = $scope.role_id;
        header_data.employee_id = $scope.employee_id;
        header_data.extension = $scope.extension;
        header_data.created_by = $scope.applicant;
        header_data.created_date = $scope.getCurrentDateTime();
        header_data.modified_by = $scope.applicant;
        header_data.modified_date = $scope.getCurrentDateTime();
        header_data.current_approver = $scope.next_approver;

        console.log('Header data : ', header_data);
        console.log('Detail data : ', $scope.rows);
    }
    // End region

    // This function retrieves header and details by folio_no
    $scope.GetRequestData = function (folio_no) {
        var promise = svc.svc_GetRequestData(folio_no);
        promise.then(function (response) {
            var jsonData = JSON.parse(response.data.d);
            var headerData = jsonData.data[0];
            var detailData = jsonData.data;
            $scope.request.header = headerData;
            $scope.request.detail = detailData;
            //console.log('JSON Data : ', jsonData);
            console.log('Header data: ', $scope.request.header);
            //console.log('Detail data: ', $scope.request.detail);

            var workflowPromise = svc.svc_GetWorkflowHistories(folio_no);
            workflowPromise.then(function (resp) {
                var jsonData_Workflow = JSON.parse(resp.data.d);
                $scope.workflow_histories = jsonData_Workflow.data;
                //console.log($scope.workflow_histories);

                for (i of $scope.workflow_histories) {
                    var action_date = i.action_date;
                    var timeStamp = parseInt(action_date.match(/\d+/)[0]);
                    var date = new Date(timeStamp);

                    var formattedDate = date.getFullYear() + '-' +
                        ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
                        ('0' + date.getDate()).slice(-2) + ' ' +
                        ('0' + date.getHours()).slice(-2) + ':' +
                        ('0' + date.getMinutes()).slice(-2) + ':' +
                        ('0' + date.getSeconds()).slice(-2);

                    i.action_date = formattedDate;
                    //console.log(action_date);
                }
            });
        });
    };
    // End region

    $scope.GetRequestHeader = function (folio_no) {
        let promise = svc.svc_GetHeaderData(folio_no);
        promise.then(function (response) {
            let jsonData = JSON.parse(response.data.d);
            console.log('Header data = ', jsonData.Data);
            let header_data = jsonData.Data;
            $scope.folio_no = header_data.folio_no;
            $scope.applicant = header_data.applicant;
            $scope.department = header_data.department;
            $scope.role = header_data.role;
            $scope.employee_id = header_data.employee_id;
            $scope.extension = header_data.extension;
        });
    };

    $scope.GetRequestDetail = function (folio_no) {
        let promise = svc.svc_GetDetailsData(folio_no);
        promise.then(function (response) {
            let jsonData = JSON.parse(response.data.d);
            let detail_data = jsonData.Data;
            console.log('Detail data = ', detail_data);
            let propToCopy = ['item_name', 'no', 'uom', 'stock', 'request_qty', 'reason'];
            $scope.rows.shift();
            for (item of detail_data) {
                let newRow = {};
                for (prop of propToCopy) {
                    newRow[prop] = item[prop];
                }
                newRow.WarningMessage1 = false;
                newRow.WarningMessage2 = false;
                $scope.rows.push(newRow);
            }
            //console.log($scope.rows);
        });
    };

    $scope.GetStatusID = function (folio_no) {
        let promise = svc.svc_GetStatusID(folio_no);
        promise.then(function (response) {
            let jsonData = JSON.parse(response.data.d);
            $scope.request_status_id = jsonData.status_id;
            console.log("JSON Data = ", jsonData);
        });
    };

    $scope.InsertApprovalLog = function () {
        var param = {
            folio_no: $scope.request.header.folio_no,
            comment: $scope.comment,
            action_name: $scope.ApprovalAction,
            pic_name: $("#username").text(),
            action_date: new Date().toLocaleString()
        };

        console.log(param);

        var promise = svc.svc_InsertApprovalLog(param);
        promise.then(function (response) {
            var jsonData = JSON.parse(response.data.d);
            console.log("Json Data after insert approval log: ", jsonData);
        });
    };



    var folio_no = GetQueryString()["folio_no"]

    if ((folio_no === null) || (folio_no === undefined) || (folio_no === '')) {
        $scope.GetCurrentLoginData();
        $scope.GetStationaryItems();
        $scope.GetApproverList();
    }
    else {
        $scope.GetRequestData(folio_no);
        $scope.GetCurrentLoginData();
        $scope.GetStationaryItems();
        $("#request_detail").addClass("readonly");
    }
    
    
});