import { BASEURL, amIAdmin, amIUser, navigationBarsss } from './globals';
import { HitApi } from './Device-Request/HitRequestApi';

(async function () {

    const url = new URL(window.location.href);
    let token, id;
    if (url.searchParams.has("token") && url.searchParams.has("id")) {
        token = url.searchParams.get("token");
        id = url.searchParams.get("id");
        sessionStorage.setItem("user_info", JSON.stringify({ token, id }));
    }
    id = JSON.parse(sessionStorage.getItem("user_info"))["id"];
    token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
    let role = await amIUser(token) == true ? "User" : "Admin";

    function createCard(cardData, action) {
        let cardCreationCode = "<button class='mdl-color--blue-grey-200' id='card' data-card=" + action + ">" + cardData + "</button>";
        document.getElementById("content").innerHTML += cardCreationCode;
    }

    function createTable(tableTitle: string, tableHeading: string, tableBody: string) {
        var tableData = "<br><br><table class='mdl-data-table mdl-js-data-table mdl-data-table--selectable mdl-shadow--2dp mdl-color-text--blue-grey-800'>"
            + "<thead class='mdl-color--blue-grey-400'>"
            + "<tr>" + tableTitle + "</tr>"
            + "<tr class='mdl-color--blue-grey-300'>" + tableHeading + "</tr>"
            + "</thead>"
            + "<tbody>" + tableBody + "</tbody>"
            + "</table>";
        document.getElementById("content").innerHTML += tableData;

    }
    function NotificationCount() {
        fetch(BASEURL+ "/api/Notification/Count/" + id)
            .then(Response => Response.json())
            .then(data => {
                (document.getElementById("notifications") as HTMLElement).dataset.badge = data;
            })
            .catch(err => console.log(err));
    }
    NotificationCount();

    function getStatistics(url: string) {
        new HitApi(token).HitGetApi(url).then(data => {
            createCard("Total Devices:" + data.totalDevices, "total");
            createCard("Free Devices:" + data.freeDevices, "free");
            if (role == "Admin") {
                createCard("Assigned Devices:" + data.assignedDevices, "allocated");
                createCard("Requests Rejected:" + data.rejectedRequests, "history");
                createCard("Total Requests:" + data.deviceRequests, "requests");
                // createCard("Total Faults:"+ data.faults,"faults");
            }
            if (role == "User") {
                createCard("Total Requests:" + data.deviceRequests, "");
                createCard("Total Faults:" + data.faults, "");
            }
        });

    }

    function getFaults(url: string) {

        var tableTitle = "<TH COLSPAN='3'><center><a href='/faultyDevice/faultdevice.html'>FAULTS</a></center></th>";
        var tableHeading = "";
        tableHeading += "<th>Type</th>"
            + "<th>Model</th>"
            + "<th>Fault</th>";
        var tableBody = "";
        new HitApi(token).HitGetApi(url).then(data => {
            for (var i = 0; i < data.length; i++) {
                let tempObject = data[i];

                tableBody += "<tr>"
                    + "<td>" + tempObject.deviceType + "</td>"
                    + "<td>" + tempObject.deviceModel + "</td>"
                    + "<td>" + tempObject.faultDescription + "</td>"
                    + "</tr>"
            }
            createTable(tableTitle, tableHeading, tableBody);
        });

    }

    function getDeviceReturnDates(url: string) {
        var tableTitle = "<TH COLSPAN='3'><center><a href='/userRequestHistory.html'>DEVICE RETURN DATES</a></center></th>";
        var tableHeading = "";
        tableHeading += "<th>Type</th>"
            + "<th>Model</th>"
            + "<th>Return Date</th>";
        var tableBody = "";
        new HitApi(token).HitGetApi(url).then(data => {
            for (var i = 0; i < data.length; i++) {
                let tempObject = data[i];

                tableBody += "<tr>"
                    + "<td>" + tempObject.deviceType + "</td>"
                    + "<td>" + tempObject.deviceModel + "</td>"
                    + "<td>" + tempObject.returnDate + "</td>"
                    + "</tr>"
            }
            createTable(tableTitle, tableHeading, tableBody);
        });

    }

    navigationBarsss(role, "navigation");
    document.getElementById('role').innerHTML = role;

    document.querySelector("#submissionNotification").addEventListener('click', e => {
        if (role == "Admin") {
            window.location.href = "./submissionRequestPage.html";
        }
    });
    document.getElementById("notifications").addEventListener('click', function (e) {
        window.location.href = "./notifiication.html";
    });
    document.getElementById("logout").addEventListener('click', function (e) {
        sessionStorage.clear();
        window.location.href = "/SJLogin/LoginRegiter.html";
    });
    document.addEventListener("click", function (e) {
        let action = (e.target as HTMLButtonElement).dataset.card;
        if (action == "total")
            window.open("/deviceListForadmin.html", "_self");
        // if(action=="faults")
        //     window.open("/faultyDevice/faultdevice.html","_self");
        if (action == "requests")
            window.open("/adminRequestPage.html", "_self");

        if (action == "history") {
            window.open("request-history/request-history.html?status=Rejected","_self");
            //Get all rejected requests
        }
        if (action == "free") {
            window.open("/deviceListForadmin.html?status=Free", "_self");
            //Get all free devices
        }
        if (action == "allocated") {
            window.open("/deviceListForadmin.html?status=Allocated", "_self");
            //Get all allocated devices
        }

    });

    if (role == 'User') {
        getStatistics(BASEURL + "/api/dashboard/statistics");
        getDeviceReturnDates(BASEURL + "/api/dashboard/" + id + "/returndates");
    }
    else if (role == 'Admin') {
        (document.getElementById("submissionNotification") as HTMLSpanElement).innerText = "check_circle";
        getStatistics(BASEURL + "/api/dashboard/statistics");
        getFaults(BASEURL + "/api/dashboard/faults");

    }
})();

