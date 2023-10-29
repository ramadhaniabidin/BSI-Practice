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

function GetCurrentLoginData(loginToken) {
    var param = {
        'loginToken': loginToken
    };

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        async: true,
        data: JSON.stringify(param),
        url: '/WebServices/StationaryRequestWebService.asmx/GetCurrentLoginData',
        dataType: "JSON",
        success: function (data) {
            var jsonData = JSON.parse(data.d);
            var userData = jsonData.currentLoginData;
            console.log('JSON Data : ', jsonData);
            var username = userData.name;
            //console.log("username : ", jsonData);
            $("#username").text(username);
            //console.log('Data from API : ', JSON.parse(data.d));
        }
    });
}

function SignOut() {
    sessionStorage.removeItem('LoginToken');
    location.href = "/Pages/Login";
}

GetCurrentLoginData(sessionStorage.getItem('LoginToken'));
