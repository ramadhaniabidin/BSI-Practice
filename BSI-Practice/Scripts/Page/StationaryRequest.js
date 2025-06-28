var app = angular.module('StationaryRequestPage', []);

app.service("svc", function ($http) {
    this.svc_GetCurrentLoginData = function (email) {
        var param = {
            'email': email
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

    this.svc_GetTaskAndAssignmentID = function (header_id) {
        var param = {
            'header_id': header_id,
        }

        var response = $http({
            method: "post",
            url: "/WebServices/StationaryRequest.asmx/GetTaskAndAssignmentID",
            data: JSON.stringify(param),
            dataType: "json",
        });

        return response;
    };

    this.svc_ApproveRequest = function (approval_value, task_id, assignmnet_id) {
        var param = {
            'action_name': approval_value,
            'task_id': task_id,
            'assignment_id': assignmnet_id
        }

        var config = {
            headers: {
                "Content-Type": "application/json"
            }
        };

        var url = "/WebServices/StationaryRequest.asmx/ApprovalAction";
        var response = $http.post(url, param, config);
        console.log(response);

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

    $scope.comment = '';
    $scope.ApprovalAction = '';

    $scope.username = "";

    $scope.isRequestValid = false;

    $scope.IsRequestor = false;
    $scope.isCurrentApprover = false;

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
        //header: {}, detail: []
        header: {
            folio_no: 'GENERATED ON SUBMIT',
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

    $scope.approver_list = ["Internal Section Head", "Internal Dept Head"];

    $scope.next_approver = "";
    $scope.req_qty_total = {
        'A4 Paper': 0,
        'Pencil': 0,
        'Marker': 0,
        'Envelope': 0
    };
    // End region

    
    $scope.ApprovalAction = function () {
        //var header_i
        var approval_value = $scope.workflow_histories.action_name;

    };

    // This function is for retrieving user data
    $scope.GetCurrentLoginData = function () {
        const email = $("#username").text();
        const promise = svc.svc_GetCurrentLoginData(email);
        promise.then(function (resp) {
            const responseJSON = JSON.parse(resp.data.d);
            const userData = responseJSON.Data;
            if (userData.RoleID == 0) {
                $scope.IsRequestor = true;
                $scope.request.header.folio_no = 'GENERATED ON SUBMIT';
                $scope.request.header.applicant = userData.Name;
                $scope.request.header.department = userData.Department;
                $scope.request.header.role = userData.Role;
                $scope.request.header.employee_id = userData.ID;
                $scope.request.header.extension = '';
            }
        }).catch(function (err) {
            console.log(err);
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
            const response_data = JSON.parse(response.data.d);
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
        const item_name = $scope.request.detail[index].item_name;
        if (!item_name) {
            return;
        }
        const promise = svc.svc_GetStockAndUnit(item_name);
        promise.then(function (response) {
            const response_data = JSON.parse(response.data.d);
            const StockAndUnit = response_data.StockAndUnit;
            $scope.request.detail[index].uom = StockAndUnit.uom;
            $scope.request.detail[index].stock = StockAndUnit.stock;
        });
    };
    // End region

    // THIS FUNCTION VALIDATES THE REQUEST QUANTITY
    $scope.CekRequestQty = function (index) {

        var stock = $scope.request.detail[index].stock;
        var req_qty = $scope.request.detail[index].request_qty;

        // THIS CHECKS WHETHER THE REQUEST QUANTITY EXCEEDS THE STOCK
        var validation1 = req_qty > stock;
        // END REGION

        // THIS CHECKS WHETHER THE REQUEST QUANTITY IS EMPTY OR NULL
        var validation2 = ((req_qty == 0) || (req_qty == undefined) || (req_qty == null));
        // END REGION

        if (validation1) {
            document.getElementById("submit-btn").classList.add("disabled");
        }

        else if (validation2) {
            document.getElementById("submit-btn").classList.add("disabled");
        }


        else {
            var submit_btn = document.getElementById("submit-btn");
            if (submit_btn.classList.contains("disabled")) {
                submit_btn.classList.remove("disabled");
            }
        }
    }
    // END REGION

    // THIS FUNCTION VALIDATES THE OVERALL REQUEST SUBMISSION

    $scope.RequestDetails_ValidateEachLine = () => {
        for (let i = 0; i < $scope.request.detail.length; i++) {
            if ($scope.request.detail[i].request_qty > $scope.request.detail[i].stock) {
                alert("Permintaan Anda (baris ke " + (i + 1) + ") melebihi stok yang ada");
                return false;
            }

            if (!$scope.request.detail[i].item_name) {
                alert("Mohon pilih barang yang akan diminta (baris " + (i + 1) + ")");
                return false;
            }

            if (!$scope.request.detail[i].request_qty) {
                alert("Jumlah barang yang diminta tidak boleh kosong (baris " + (i + 1) + ")");
                return false;
            }
        }
        return true;
    };

    $scope.SubmitRequest = () => {
        if ($scope.ValidateRequest()) {
            if (confirm("Submit Data?")) {
                const header_data = $scope.SubmitRequest_GenerateHeader();
                console.log(header_data);
                const details = $scope.SubmitRequest_GenerateDetails();
                console.log(details);
                const promise = svc.svc_SaveUpdate(header_data, details);
                promise.then((response) => {
                    console.log(response);
                },
                    (err) => {
                    console.error(err);
                })
            }
        }
    };

    $scope.SubmitRequest_GenerateHeader = () => {
        const header = {
            folio_no: $scope.request.header.folio_no,
            applicant: $scope.request.header.applicant,
            department: $scope.request.header.department,
            role: $scope.request.header.role,
            role_id: $scope.request.header.role_id,
            employee_id: $scope.request.header.employee_id,
            extension: $scope.request.header.extension,
            created_by: $scope.request.header.applicant,
            created_date: $scope.getCurrentDateTime(),
            modified_by: $scope.request.header.applicant,
            modified_date: $scope.getCurrentDateTime(),
            current_approver_role: $scope.next_approver
        };
        return header;
    };

    $scope.SubmitRequest_GenerateDetails = () => {
        let detail = [];
        $scope.request.detail.map((item,i) => {
            detail.push({
                item_name: item.item_name,
                no: i + 1,
                uom: item.uom,
                stock: item.stock,
                request_qty: parseInt(item.request_qty),
                reason: item.reason
            });
        });
        return detail;
    };

    $scope.ValidateRequest = function () {
        if (!$scope.RequestDetails_ValidateEachLine()) {
            return false;
        }

        if (!$scope.next_approver) {
            alert("Next Approver tidak boleh kosong!");
            return false;
        }        

        if (!$scope.ValidateTotalRequestPerItem()) {
            return false;
        }

        return true;
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
            console.log('Header data: ', $scope.request.header);

            var workflowPromise = svc.svc_GetWorkflowHistories(folio_no);
            workflowPromise.then(function (resp) {
                var jsonData_Workflow = JSON.parse(resp.data.d);
                $scope.workflow_histories = jsonData_Workflow.data;

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
                }
            });
        });
    };
    // End region

    $scope.RequestDetails_GetDistinctItems = function () {
        const distinctItems = $scope.request.detail.reduce((acc, item) => {
            if (!acc.some(x => x.item_name === item.item_name)) {
                acc.push({ item_name: item.item_name, stock: item.stock });
            }
            return acc;
        }, []);
        return distinctItems;
    };

    $scope.RequestDetails_GetTotalRequestPerItem = (distinctItems) => {
        let arr = [];
        distinctItems.map(item => {
            const total = $scope.request.detail.reduce((sum, req) =>
                req.item_name === item.item_name ? sum + parseInt(req.request_qty) : sum, 0
            );
            arr.push({ item_name: item.item_name, total_request_qty: total, stock: item.stock });
        });
        return arr;
    };

    $scope.ValidateTotalRequestPerItem = function () {
        const distinctItems = $scope.RequestDetails_GetDistinctItems();
        const totalRequestPerItem = $scope.RequestDetails_GetTotalRequestPerItem(distinctItems);

        const isValid = !totalRequestPerItem.some(item => {
            if (item.total_request_qty > item.stock) {
                alert(`Jumlah request Anda melebihi stock yang ada untuk item ${item.item_name}`);
                return true;
            }
            return false;
        });
        return isValid;
    };

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

    $scope.Close = () => {
        location.href = "/Pages/Home";
    };

    $scope.GetFormRequestData = (ID) => {
        // to be implemented
    };

    $scope.PopulateFormData = () => {
        const ID = GetQueryString()["ID"];
        if (ID) {
            $scope.GetFormRequestData(ID);
        }
        else {
            $scope.GetCurrentLoginData();
            $scope.GetStationaryItems();
        }
    };

    //$scope.GetCurrentLoginData();
    $scope.PopulateFormData();
    
});