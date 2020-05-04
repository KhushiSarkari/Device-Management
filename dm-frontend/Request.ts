import { BASEURL, navigationBarsss, PageNo, current_page,Token,changePage} from "./globals";
import * as util from "./utilities";
import { Requests, Specification, PartialUserModel } from "./RequestModel";
import { HitApi } from './Device-Request/HitRequestApi';
import { Sort } from "./user-profile/SortingUser";

    const token: string = Token.getInstance().tokenKey;
    let adminId = Token.getInstance().userID;
    let globalUrl = BASEURL + "/api/request/";
    let currentPage:number=current_page;
    let obj = {
        notify: []
    };

    function getPendingRequests(url: string) {

        var tableData = '';
        var specs = new Specification();
        var requestedBy = new PartialUserModel();
        new HitApi(token).HitGetApi(url).then(data => {
            for (var i = 0; i < data.length; i++) {
                specs = data[i]['specs'];
                requestedBy = data[i]['requestedBy'];
                console.log(data[i]["requestedBy"]['email']);
                tableData += "<tr>"
                    + "<td>" + data[i]['userId'] + "</td>"
                    + "<td>" + data[i]['deviceType'] + "</td>" + "<td>" + data[i]['deviceBrand'] + "</td>" + "<td>" + data[i]['deviceModel'] + "</td>"
                    + "<td>" + util.concatSpecs(specs) + "</td>"
                    + "<td>" + util.concatName(requestedBy) + "</td>"
                    + "<td>" + data[i]['requestDate'] + "</td>"
                    + "<td>" + data[i]['availability'] + "</td>";
                if (data[i]['availability'] == true)
                    tableData += "<td>" + "<button class=\"accept-button\" data-requestid=\"" + data[i]['requestId'] + "\" data-requestname=\""+ util.concatName(requestedBy) +"\" data-requestmail = \"" + data[i]["requestedBy"]['email']+"\"  >Accept</button>" + "</td>";
                else
                    tableData += "<td>" + "<button class=\"show-users\" data-devicemodel=\""
                        + data[i]['deviceModel'] + "\"data-devicetype=\"" + data[i]['deviceType'] + "\" data-devicebrand=\""
                        + data[i]['deviceBrand'] + "\"data-ram=\"" + specs.ram + "\"data-connectivity=\"" + specs.connectivity
                        + "\"data-screensize=\"" + specs.screenSize + "\"data-storage=\"" + specs.storage + "\" >Notify</button>" + "</td>";

                tableData += "<td>" + "<button class=\"reject-button\" data-requestid=" + data[i]['requestId'] + "\"  data-requestname=\""+ util.concatName(requestedBy) +"\" data-requestmail = \"" + data[i]["requestedBy"]['email']+"\" >Reject</button>" + "</td></tr>";

            }
            document.getElementById("content").innerHTML = tableData;

        });

    }

    function getDeviceHolders(request) {
        let tableData = "";
        new HitApi(token).HitGetApi(BASEURL + "/api/Device/search?status_name=allocated").then(data => {
            for (var i = 0; i < data.length; i++) {

                if ((data[i].type == request.deviceType) && (data[i].brand == request.deviceBrand)
                    && (data[i].model == request.deviceModel) && (data[i].specifications.ram == request.specs.ram) &&
                    (data[i].specifications.storage == request.specs.storage) && (data[i].specifications.screenSize == request.specs.screenSize) &&
                    (data[i].specifications.connectivity == request.specs.connectivity)) {

                    tableData += "<tr>"
                    + "<td>" + data[i]['device_id'] + "</td>"
                    + "<td>" + data[i]['assign_to']['first_name'] + " " + data[i]['assign_to']['middle_name'] + " " + data[i]['assign_to']['last_name'] + "</td>"
                    + "<td>" + data[i]['return_date'] + "</td>"
                    + "<td><button class=\"notify\" data-deviceid=" + data[i]['device_id'] + " >Notify</button></center></td></tr>"
                    let deviceId = data[i].device_id;
                    obj.notify.push({ "deviceId": deviceId });
                }

            }
            tableData += "<tr><td colspan=4><center><button class=\"notify-all\">Notify All</button></center></td></tr>";
            document.getElementById("popupContent").innerHTML = tableData;
            (document.querySelector('.popup') as HTMLDivElement).style.display = 'flex';

        });
    }

    function requestAction(requestUrl, requestId, action , name , mail) {
        fetch(globalUrl + requestId + requestUrl +"&name="+name+"&email="+mail,
            {
                headers: new Headers({ "Authorization": `Bearer ${token}` })
            });
        alert("Request " + requestId + " " + action);
        getPendingRequests(globalUrl + "pending?"+PageNo(currentPage));

    }

    function postNotification(data) {
        if (confirm("Notify?")) {
            fetch(BASEURL + "/api/Notification", {
                method: "POST",
                headers: [["Content-Type", "application/json"], ["Authorization", `Bearer ${token}`]],
                body: JSON.stringify(data),
            }).catch(Error => console.log(Error));
            alert("Notification sent");
            (document.querySelector('.popup') as HTMLDivElement).style.display = 'none';
            obj = {
                notify: []
            };
        }
    }

    (document.querySelector('#tablecol') as HTMLTableElement).addEventListener("click", function (e) {
        const sortField = (e.target as HTMLElement).getAttribute('name');
        let id = e.target as HTMLTableHeaderCellElement;
		let sorts = new Sort(token);
		let direction = sorts.checkSortType(id);
        getPendingRequests(globalUrl + "pending?sortby=" + sortField + "&direction=" + direction + "&"+PageNo(currentPage));

    });
    document.querySelector('#fixed-header-drawer-exp').addEventListener('input', function (e) {
        var searchField = (document.getElementById("fixed-header-drawer-exp") as HTMLInputElement).value;
        getPendingRequests(globalUrl + "pending?search=" + searchField+ "&"+PageNo(currentPage));
    });
    document.querySelector('.close').addEventListener('click',
        function () {
            (document.querySelector('.popup') as HTMLDivElement).style.display = 'none';
            obj = {
                notify: []
            };
        });

    document.addEventListener("click", function (e) {
        let requestId = parseInt((e.target as HTMLButtonElement).dataset.requestid, 10);
        if ((e.target as HTMLButtonElement).className == "reject-button") {
           var name =  ((e.target as HTMLButtonElement).dataset.requestname);
            var mail = ((e.target as HTMLButtonElement).dataset.requestmail);
            if (confirm("Are you sure you want to reject the request?"))
            {
                requestAction('?action=reject&id=' + adminId, requestId, 'rejected' , name , mail);
                window["tata"].text('Request ','Rejected!',{duration:3000});
            }
        }
        if ((e.target as HTMLButtonElement).className == "accept-button") {
            var name =  ((e.target as HTMLButtonElement).dataset.requestname);
            var mail = ((e.target as HTMLButtonElement).dataset.requestmail);
            if (confirm("Are you sure you want to accept the request?")){
                requestAction('?action=accept&id=' + adminId, requestId, 'accepted' , name , mail);
                window["tata"].text('Request ','Accepted!',{duration:3000});}

        }
        if ((e.target as HTMLButtonElement).className == "show-users") {
            let request = new Requests();
            request.deviceModel = (e.target as HTMLButtonElement).dataset.devicemodel;
            request.deviceBrand = (e.target as HTMLButtonElement).dataset.devicebrand;
            request.deviceType = (e.target as HTMLButtonElement).dataset.devicetype;
            request.specs.ram = ((e.target as HTMLButtonElement).dataset.ram);
            request.specs.connectivity = ((e.target as HTMLButtonElement).dataset.connectivity);
            request.specs.screenSize = ((e.target as HTMLButtonElement).dataset.screensize);
            request.specs.storage = ((e.target as HTMLButtonElement).dataset.storage);
            getDeviceHolders(request);

        }
        if ((e.target as HTMLButtonElement).className == "notify-all") {
            postNotification(obj);
        }
        if ((e.target as HTMLButtonElement).className == "notify") {
            let deviceId: number = parseInt((e.target as HTMLButtonElement).dataset.deviceid, 10);
            postNotification({ "notify": [{ deviceId }] });
        }

    });
    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{   currentPage=changePage((e.target as HTMLButtonElement).value);
		getPendingRequests(globalUrl + "pending?"+PageNo(currentPage));   
    });

    getPendingRequests(globalUrl + "pending?"+PageNo(currentPage));
    navigationBarsss("Admin", "navigation");

