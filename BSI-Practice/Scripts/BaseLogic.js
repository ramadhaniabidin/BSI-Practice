var popUpDialog;

var Title_Info_MessageBox = 'Info Message';
var Title_Info_Warning = 'Warning!';
var siteWeb = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');

function HideSharePointUpperRibbon() {
    var ribbon = $("li#Ribbon.Read-title.ms-cui-tt.ms-browseTab a.ms-cui-tt-a").first();
    console.log("Found ribbon :", ribbon);
    ribbon = ribbon.first();
    if ((ribbon !== undefined) && (ribbon !== null)) {
        ribbon.trigger("click");
    }
}

//function Logout() {
//    window.localStorage.removeItem('email');
//    window.localStorage.removeItem('role_id');
//    window.location.href = "/Login.aspx";
//}

function ChangeComponentValueHeaderTitle(newTitle) {
    //$('#header_titleHeader').html('Form Bukti Penerimaan Sembako, Beras, & Susu');
    var elem = document.getElementById('#header_titleHeader');

    if ((elem !== null) && (newTitle !== null) && (newTitle.toString().trim() !== '')) {
        elem.innerHTML = newTitle.toString().trim();
    }
}

function GetQueryString() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function OnlyNumberWithoutDecimals(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode < 48 || charCode > 57)) {
        evt.preventDefault();
        return false;
    }
    return true;
}

function OnlyNumberAlsoDecimals(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 45 || charCode > 57)) {
        evt.preventDefault();
        return false;
    }
    return true;
}

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function ErrorHandling(xhr, errorType, exception) {
    var responseText;
    $("#infoMessage").html("");

    $('.modal-title').html('Warning');
    $('#myModal').modal({
        show: 'true'
    });

    try {
        responseText = jQuery.parseJSON(xhr.responseText);
        $("#infoMessage").append("<div><b>" + errorType + " " + exception + "</b></div>");
        $("#infoMessage").append("<div><u>Exception</u>:<br /><br />" + responseText.ExceptionType + "</div>");
        $("#infoMessage").append("<div><u>StackTrace</u>:<br /><br />" + responseText.StackTrace + "</div>");
        $("#infoMessage").append("<div><u>Message</u>:<br /><br />" + responseText.Message + "</div>");
    } catch (e) {
        responseText = xhr.responseText;
        $("#infoMessage").html(responseText);
    }
}

function DateFormat_ddMMMyyyy(date) {
    var monthNames = [
        "Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul",
        "Aug", "Sep", "Oct",
        "Nov", "Dec"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var jam = date.getHours();
    var menit = date.getMinutes();

    var formatDate = day + '-' + monthNames[monthIndex] + '-' + year.toString();

    //var strDateTime = [formatDate, [AddZero(jam), AddZero(menit)].join(":"), jam >= 12 ? "PM" : "AM"].join(" ");
    //var strDateTime = [formatDate, [AddZero(jam), AddZero(menit)].join(":")].join(" ");

    return formatDate;
}


function JSONDateToJavaScript(jsonDateString) {
    var d = new Date(parseInt(jsonDateString.replace('/Date(', '')));
    var result = DateFormat_ddMMMyyyy(d);
    return result;
}

function DateFormat_ddMMMyyyyHHmmtt(date) {
    var monthNames = [
        "Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul",
        "Aug", "Sep", "Oct",
        "Nov", "Dec"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var jam = date.getHours();
    var menit = date.getMinutes();

    var formatDate = day + '-' + monthNames[monthIndex] + '-' + year.toString();

    //var strDateTime = [formatDate, [AddZero(jam), AddZero(menit)].join(":"), jam >= 12 ? "PM" : "AM"].join(" ");
    var strDateTime = [formatDate, [AddZero(jam), AddZero(menit)].join(":")].join(" ");

    return strDateTime;
}
function AddZero(num) {
    return (num >= 0 && num < 10) ? "0" + num : num + "";
}

function OnlyNumberWithoutDecimals(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode < 48 || charCode > 57)) {
        evt.preventDefault();
        return false;
    }
    return true;
}

function OnlyNumberAlsoDecimals(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 45 || charCode > 57)) {
        evt.preventDefault();
        return false;
    }
    return true;
}

function EmailValidation(x) {
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
        alert("Not a valid e-mail address");
        return false;
    } else {
        return true;
    }
}

function IsEmpty(str) {
    return (!str || 0 === str.length);
}

function GetYear_FromDateAdd(objDate, days) {
    var tt = objDate;

    var date = new Date(tt);
    var newdate = new Date(date);

    newdate.setDate(newdate.getDate() + parseInt(days));

    var dd = newdate.getDate();
    var mm = newdate.getMonth() + 1;
    var y = newdate.getFullYear();

    var someFormattedDate = mm + '/' + dd + '/' + y;
    console.log(tt);
    console.log(days);
    console.log(someFormattedDate);
    return y;
}

function ParseDateFromCS(date, format = 'dd MMM yyyy') {
    var invalid = '';
    if (!date)
        return invalid;

    date = parseInt(date.substr(6));
    if (date < 0)
        return invalid;

    return new Date(date).format(format);
};

function ValidateDateToSave(date) {
    var re = "/^\d{1,2}[ /](?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Des|January|February|March|April|May|June|July|August|September|October|November|Desember|\d{1,2})[ /]\d{4}$/";
    return re.test(date);
};

function ShowLookupPopup(shownPopup) {
    popUpDialog = $("#dialog").dialog({
        height: 375,
        width: 600
    });
};

function SetInputsToReadonly(targetClassName) {
    if (targetClassName == null) {
        targetClassName = "popupfilled";
    }
    var target = "div." + targetClassName + " input";
    var inps = document.querySelectorAll(target);
    if (inps.length > 0) {
        for (var i = 0; i < inps.length; i++) {
            inps[i].setAttribute("readonly", "true");
        }
    }
}

function generateString() {
    const alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const numbers = "0123456789";

    let firstTwoDigits = '';
    for (let i = 0; i < 2; i++) {
        const randomIndex = Math.floor(Math.random() * alphabets.length);
        firstTwoDigits += alphabets[randomIndex];
    }

    let lastThreeDigits = '';
    for (let i = 0; i < 3; i++) {
        const randomIndex = Math.floor(Math.random() * numbers.length);
        lastThreeDigits += numbers[randomIndex];
    }

    const output = firstTwoDigits + lastThreeDigits;
    return output;
}