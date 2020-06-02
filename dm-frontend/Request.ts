import { BASEURL, navigationBarsss, PageNo, current_page, Token, changePage, headersRows } from "./globals";
import * as util from "./utilities";
import { Requests, Specification, PartialUserModel } from "./RequestModel";
import { HitApi } from './Device-Request/HitRequestApi';
import { Sort } from "./user-profile/SortingUser";

const token: string = Token.getInstance().tokenKey;
let adminId = Token.getInstance().userID;
let requestUrl = `${BASEURL}/api/request/`;
let currentPage: number = current_page;
let obj = {
    notify: []
};

function allRequests(url: string) {

    var tableData = '';
    var specs = new Specification();
    var requestedBy = new PartialUserModel();
    new HitApi(token).HitGetApi(url).then(data => {
        data.map(request=>
        {
            specs = request.specs;
            requestedBy = request.requestedBy;
            tableData += `<tr>
                <td>${request.deviceType}</td>
                <td>${request.deviceBrand}</td>
                <td>${request.deviceModel}</td>
                <td>${util.concatSpecs(specs)}</td>
                <td>${util.concatName(requestedBy)}</td>
                <td>${request.requestDate}</td>`;

            if (request.availability == true){
                tableData += `<td>
                    <button class="mdl-button mdl-js-button mdl-button--raised mdl-button--accent accept-button" 
                    data-requestid="${request.requestId}" 
                    data-requestname="${util.concatName(requestedBy)}" 
                    data-requestmail="${request.requestedBy.email}" >Accept</button>
                </td>`;
            }
            else{
                tableData += `<td>
                    <button class="mdl-button mdl-js-button mdl-button--raised mdl-button--accent show-users"
                    data-devicemodel="${request.deviceModel}" 
                    data-devicetype="${request.deviceType}" 
                    data-devicebrand="${request.deviceBrand}" 
                    data-ram="${specs.ram}" 
                    data-connectivity="${specs.connectivity}" 
                    data-screensize="${specs.screenSize}" 
                    data-storage="${specs.storage}">Notify</button>
                </td>`;
            }
            tableData += `<td>
                <button class="mdl-button mdl-js-button mdl-button--raised mdl-button--colored reject-button" 
                data-requestid="${request.requestId}"  
                data-requestname="${util.concatName(requestedBy)}" 
                data-requestmail="${request.requestedBy.email}">Reject</button>
            </td>
            </tr>`;

        });
        document.getElementById("content").innerHTML = tableData;

    });

}

function showUsers(request) {
    let tableData = "";
    new HitApi(token).HitGetApi(`${BASEURL}/api/Device/search?status_name=allocated`).then(data => {
        data.map(user=>
            {

            if ((user.type == request.deviceType) && (user.brand == request.deviceBrand)
                && (user.model == request.deviceModel) && (user.specifications.ram == request.specs.ram) &&
                (user.specifications.storage == request.specs.storage) && (user.specifications.screenSize == request.specs.screenSize) &&
                (user.specifications.connectivity == request.specs.connectivity)) {

                tableData += `<tr>
                    <td>${user.device_id}</td>
                    <td>${user.assign_to.first_name} ${user.assign_to.middle_name} ${user.assign_to.last_name}</td>
                    <td>${user.return_date}</td>
                    <td>
                        <button class="mdl-button mdl-js-button mdl-button--raised mdl-button--accent notify" data-deviceid="${user.device_id}" >Notify</button>
                    </td>
                    </tr>`;
                obj.notify.push({ "deviceId": user.device_id });
            }

        });
        if (tableData != "") {
            tableData += `<tr>
                <td colspan=4>
                    <center><button class="mdl-button mdl-js-button mdl-button--raised mdl-button--accent notify-all">Notify All</button></center>
                </td>
            </tr>`;
            document.getElementById("popupContent").innerHTML = tableData;
            (document.querySelector('.popup') as HTMLDivElement).style.display = 'flex';
        }
        else {
            allRequests(`${requestUrl}pending?${PageNo(currentPage)}`);
            alert("Requested Device is faulty");
        }


    });
}

function requestAction(url, requestId, action, name, mail) {
    fetch(`${requestUrl}${requestId}${url}&name=${name}&email=${mail}`,
        {
            headers: new Headers({ "Authorization": `Bearer ${token}` })
        });
    alert(`Request ${requestId} ${action}`);
    allRequests(`${requestUrl}pending?${PageNo(currentPage)}`);

}

function postNotification(data) {
    if (confirm("Notify?")) {
        fetch(`${BASEURL}/api/Notification`, {
            method: "POST",
            headers: [["Content-Type", "application/json"], ["Authorization", `Bearer ${token}`]],
            body: JSON.stringify(data),
        }).catch(Error => console.log(Error));
        alert("Notification sent");
        (document.querySelector('.popup') as HTMLDivElement).style.display = 'none';
        obj = {
            notify: []
        };
        allRequests(`${requestUrl}pending?${PageNo(currentPage)}`);
    }
}

(document.querySelector('#tablecol') as HTMLTableElement).addEventListener('click', e => {
    const sortField = (e.target as HTMLElement).getAttribute('name');
    let id = e.target as HTMLTableHeaderCellElement;
    let sorts = new Sort(token);
    let direction = sorts.checkSortType(id);
    allRequests(`${requestUrl}pending?sortby=${sortField}&direction=${direction}&${PageNo(currentPage)}`);

});
document.querySelector('#fixed-header-drawer-exp').addEventListener('input', e => {
    var searchField = (document.getElementById("fixed-header-drawer-exp") as HTMLInputElement).value;
    allRequests(`${requestUrl}pending?search=${searchField}&${PageNo(currentPage)}`);
});
document.querySelector('.close').addEventListener('click',
    () => {
        (document.querySelector('.popup') as HTMLDivElement).style.display = 'none';
        obj = {
            notify: []
        };
        allRequests(`${requestUrl}pending?${PageNo(currentPage)}`);
    });

    document.addEventListener('click', e =>{
        let requestId = parseInt((e.target as HTMLButtonElement).dataset.requestid, 10);
        if ((e.target as HTMLButtonElement).classList.contains("reject-button")) {
           var name =  ((e.target as HTMLButtonElement).dataset.requestname);
            var mail = ((e.target as HTMLButtonElement).dataset.requestmail);
            if (confirm("Are you sure you want to reject the request?"))
            {
                requestAction(`?action=reject&id=${adminId}`, requestId, 'rejected' , name , mail);
                window["tata"].text('Request ','Rejected!',{duration:3000});
            }
        }
        if ((e.target as HTMLButtonElement).classList.contains("accept-button")) {
            var name =  ((e.target as HTMLButtonElement).dataset.requestname);
            var mail = ((e.target as HTMLButtonElement).dataset.requestmail);
            if (confirm("Are you sure you want to accept the request?")){
                requestAction(`?action=accept&id=${adminId}`, requestId, 'accepted' , name , mail);
                window["tata"].text('Request ','Accepted!',{duration:3000});}

    }
    if ((e.target as HTMLButtonElement).classList.contains("show-users")) {
        let request = new Requests();
        request.deviceModel = (e.target as HTMLButtonElement).dataset.devicemodel;
        request.deviceBrand = (e.target as HTMLButtonElement).dataset.devicebrand;
        request.deviceType = (e.target as HTMLButtonElement).dataset.devicetype;
        request.specs.ram = ((e.target as HTMLButtonElement).dataset.ram);
        request.specs.connectivity = ((e.target as HTMLButtonElement).dataset.connectivity);
        request.specs.screenSize = ((e.target as HTMLButtonElement).dataset.screensize);
        request.specs.storage = ((e.target as HTMLButtonElement).dataset.storage);
        showUsers(request);

    }
    if ((e.target as HTMLButtonElement).classList.contains("notify-all")) {
        postNotification(obj);
    }
    if ((e.target as HTMLButtonElement).classList.contains("notify")) {
        let deviceId: number = parseInt((e.target as HTMLButtonElement).dataset.deviceid, 10);
        postNotification({ "notify": [{ deviceId }] });
    }

});
(document.querySelector("#pagination") as HTMLButtonElement).addEventListener('click', e => {
    currentPage = changePage((e.target as HTMLButtonElement).value);
    allRequests(`${requestUrl}pending?${PageNo(currentPage)}`);
});

allRequests(`${requestUrl}pending?${PageNo(currentPage)}`);
navigationBarsss("Admin", "navigation");
headersRows("Admin", "row1");

