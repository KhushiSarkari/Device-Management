import { BASEURL, amIAdmin, amIUser, navigationBarsss, PageNo, current_page, paging, changePage, Token, headersRows } from './globals';
import { Sort } from './user-profile/SortingUser';
import { HitApi } from './Device-Request/HitRequestApi';
import { descriptionboxvalidation, fileValidation } from "./validation";
let currentPage: number = current_page;
(async function () {
    if (document.title != "My Devices") {
        return;
    }
    const _ = Token.getInstance();
    const userId = _.userID;
    const token = _.tokenKey;
    const role = await amIUser(token) == true ? "User" : "Admin";

    document.getElementById("one").addEventListener('click', function () {
        currentPage = 1;
        mydevices.getCurrentDevice(userId);
    });

    document.getElementById("three").addEventListener('click', function () {
        currentPage = 1;
        mydevices.getRequestHistory(userId);
    });

    document.getElementById("tab3").addEventListener('click', function (ev) {
        if ((ev.target as HTMLButtonElement).classList.contains("cancel")) {
            const requestId = (ev.target as HTMLButtonElement).parentElement.parentElement.dataset.requestId;
            mydevices.deleteRequestHistory(parseInt(requestId)).then(function () { mydevices.getRequestHistory(userId); });
            window["tata"].text('Request ', 'cancelled!', { duration: 3000 });
        }
    });

    document.getElementById("tab1").addEventListener('click', function (ev) {
        if ((ev.target as HTMLButtonElement).classList.contains("return")) {
            const deviceid = (ev.target as HTMLButtonElement).parentElement.parentElement.dataset.deviceId;
            console.log(deviceid);
            mydevices.returnDevice(userId, parseInt(deviceid)).then(function () { mydevices.getCurrentDevice(userId); });
            window["tata"].text('Device ', 'Returned!', { duration: 3000 });
        }
        else if ((ev.target as HTMLButtonElement).classList.contains("fault")) {
            openForm();
            const deviceid = (ev.target as HTMLButtonElement).parentElement.parentElement.dataset.deviceId;
            document.getElementById("faultpopup").setAttribute("data-device-id", deviceid)

        }
    });
   async function uploadAndReadFile(file:Blob)
    {
        const reader=new FileReader();
       return new Promise((resolve,reject)=>{
           reader.onloadend=() => {
               resolve(reader.result)
           }
           reader.readAsDataURL(file)
       })

    }

    document.querySelector('#faultpopup .submit').addEventListener('click', async function (ev) {
        ev.preventDefault();
        var comment = (document.getElementById("comment") as HTMLTextAreaElement).value;
        var deviceid = parseInt(document.getElementById("faultpopup").dataset.deviceId);
        var files = (document.getElementById("uploadBtn") as HTMLInputElement).files[0];
        let file: string;
      
        if (files) {
           file= await uploadAndReadFile(files) as string;
        }


        if (descriptionboxvalidation() && fileValidation() == false) {
            return;
        }
        mydevices.reportFaultyDevice(userId, deviceid, comment, file);
        closeForm();
        window["tata"].text('Device Fault ', 'Reported!', { duration: 3000 });

    });

    document.querySelector('.closed').addEventListener('click', function (e) {
        e.preventDefault();
        closeForm();


    });


    document.getElementById("search1").addEventListener('keyup', function () {
        mydevices.getCurrentDevice(userId, (document.getElementById("search1") as HTMLInputElement).value);
    });

    document.getElementById("search3").addEventListener('keyup', function () {
        mydevices.getRequestHistory(userId, (document.getElementById("search3") as HTMLInputElement).value);
    });

    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click", e => {
        currentPage = changePage((e.target as HTMLButtonElement).value);
        if (document.querySelector(".mdl-layout__tab-panel.is-active") == document.getElementById("fixed-tab-1") as HTMLLIElement)
            mydevices.getCurrentDevice(userId);
        else
            mydevices.getRequestHistory(userId);
    });

    document.addEventListener("click", function (ea) {

        if ((ea.target as HTMLTableHeaderCellElement).tagName == 'TH' && (ea.target as HTMLTableHeaderCellElement).dataset.sortable == "true") {
            var tab1: HTMLLIElement = document.getElementById("fixed-tab-1") as HTMLLIElement;

            const searchbox = tab1.querySelector(".mdl-textfield__input")
            if (document.querySelector(".mdl-layout__tab-panel.is-active") == tab1) {

                mydevices.getCurrentDevice(userId, (searchbox as HTMLInputElement).value, new Sort(token).getSortingUrl(ea.target as HTMLTableHeaderCellElement));
            }
            else {
                mydevices.getRequestHistory(userId, (searchbox as HTMLInputElement).value, new Sort(token).getSortingUrl(ea.target as HTMLTableHeaderCellElement));
            }

        }

    });
    function openForm() {
        document.querySelector('#faultpopup').classList.add("active");
        (document.getElementById('description') as HTMLInputElement).innerHTML = "";
    }

    function closeForm() {
        document.querySelector('#faultpopup').classList.remove("active");
        (document.getElementById('description') as HTMLInputElement).innerHTML = "";
    }

    document.getElementById("uploadBtn").onchange = function () {
        (document.getElementById("uploadFile") as HTMLInputElement).value = (this as HTMLInputElement).files[0].name;
    };

    navigationBarsss(role, "navigation");
    headersRows(role, "row1");
    var mydevices = new MyDevices(token);
    mydevices.getCurrentDevice(userId);
})();

export class MyDevices {

    data: any;
    url: string;
    api: HitApi;
    token: string;
    table1: HTMLTableElement = document.getElementById("tab1") as HTMLTableElement;
    table3: HTMLTableElement = document.getElementById("tab3") as HTMLTableElement;
    constructor(token: string) {
        this.token = token;
        this.api = new HitApi(token);
    }

    async getCurrentDevice(id: number, search: string = "", sort: string = "") {
        this.url = BASEURL + "/api/Device/current_device/" + id + "?search=" + search + sort + "&" + PageNo(currentPage);
        this.data = await this.api.HitGetApi(this.url);
        this.dynamicGenerateCurrentDevice(this.table1);
    }

    async getRequestHistory(id: number, searchField: string = "", sort: string = "") {
        this.url = BASEURL + "/api/request/pending?id=" + id + "&search=" + searchField + sort + "&" + PageNo(currentPage);
        this.data = await this.api.HitGetApi(this.url);
        this.dynamicGenerateRequestDevice(this.table3);
    }

    returnDevice(userId: number, deviceId: number) {
        return this.api.HitPostApi(BASEURL + "/api/ReturnRequest", { userId, deviceId });
    }

    reportFaultyDevice(userId: number, deviceId: number, comment: string, file: string) {
        let requestobject = { deviceId, userId, comment }
        Object.assign(requestobject, file && { file });
        return this.api.HitPostApi(BASEURL + "/api/ReturnRequest/fault", requestobject);
    }

    deleteRequestHistory(requestID: number) {
        return this.api.HitDeleteApi(BASEURL + "/api/request/" + requestID + "/cancel");
    }


    async getApiCall(URL: any) {
        let response = await fetch(URL, {
            headers: new Headers({ "Authorization": `Bearer ${this.token}` })
        });
        let metadata = JSON.parse(response.headers.get('X-Pagination'));
        paging(metadata);
        let data = await (response.json());
        return (data);
    }




    dynamicGenerateCurrentDevice(table: any) {
        let loop = 0;
        this.DeleteRows(table);
        for (loop = 0; loop < this.data.length; loop++) {
            var row = table.insertRow(loop + 1);
            row.setAttribute("data-device-id", this.data[loop]["device_id"])
            var cell = row.insertCell(0);
            var cell1 = row.insertCell(1);
            var cell2 = row.insertCell(2);
            var cell3 = row.insertCell(3);
            var cell4 = row.insertCell(4);
            cell.innerHTML = this.data[loop]["type"]
            cell1.innerHTML = this.data[loop]["brand"]
            cell2.innerHTML = this.data[loop]["model"]
            cell3.innerHTML = this.data[loop]["assign_date"]
            cell4.innerHTML = this.data[loop]["return_date"]
            if (table == this.table1) {
                var cell5 = row.insertCell(5);
                cell5.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--raised mdl-button--colored return">
            RETURN
        </button>`
                var cell6 = row.insertCell(6);
                cell6.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--raised mdl-button--colored fault">
    REPORT
</button>`
            }


        }


    }
    dynamicGenerateRequestDevice(table: any) {
        let loop = 0;
        this.DeleteRows(table);
        for (loop = 0; loop < this.data.length; loop++) {
            var row = table.insertRow(loop + 1);
            var cell = row.insertCell(0);
            var cell1 = row.insertCell(1);
            var cell2 = row.insertCell(2);
            var cell3 = row.insertCell(3);
            var cell4 = row.insertCell(4);
            var cell5 = row.insertCell(5);
            row.setAttribute("data-request-id", this.data[loop]["requestId"])
            cell.innerHTML = this.data[loop]["deviceType"]
            cell1.innerHTML = this.data[loop]["deviceBrand"]
            cell2.innerHTML = this.data[loop]["deviceModel"]
            cell3.innerHTML = this.data[loop]["requestDate"]
            cell4.innerHTML = this.data[loop]["noOfDays"]
            cell5.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--raised mdl-button--colored cancel">CANCEL </button>`

        }

    }

    DeleteRows(table: any) {
        var rowCount = table.rows.length;
        for (var i = rowCount - 1; i > 0; i--) {
            table.deleteRow(i);
        }
    }
}
