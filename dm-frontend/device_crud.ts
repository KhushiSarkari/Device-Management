import { BASEURL, paging } from "./globals";
import { formatDate1 } from "./utilities";
import { HitApi } from "./Device-Request/HitRequestApi";
import { populateDropdown } from "./dropdown";
import { specificationDropdown } from "./Device-Request/UserRequestMain";

export class AddDevice {
    field: string
    type: string;
    brand: string;
    status_id: number;
    model: string;
    color: string;
    price: string;
    serial_number: string;
    warranty_year: string;
    purchase_date: string;
    specification_id: number;
    entry_date: string;
    ram: string;
    storage: string;
    screen_size: string;
    connectivity: string;
    token: string = "";
    constructor(token: string) {
        this.token = token;
    }
    async brandDropdown() {
        const URL = BASEURL + "/api/dropdown/brands";
        const brands = await new HitApi(this.token).HitGetApi(URL);
        populateDropdown((document.getElementById("brand") as HTMLSelectElement), brands);
        return null;
    }
    async typeDropdown() {
        const URL = BASEURL + "/api/dropdown/types";
        const types = await new HitApi(this.token).HitGetApi(URL);
        populateDropdown((document.getElementById("type") as HTMLSelectElement), types);
        return null;
    }
    async modelDropdown() {
        const URL = BASEURL + "/api/dropdown/models";
        const models = await new HitApi(this.token).HitGetApi(URL);
        populateDropdown((document.getElementById("model") as HTMLSelectElement), models);
        return null;
    }
    async statusDropdown() {
        const URL = BASEURL + "/api/Dropdown/status";
        const status = await new HitApi(this.token).HitGetApi(URL);
        let htmlString = '';
        for (let dataPair of status) {
            htmlString += '<option  value="' + dataPair.id + '">' + dataPair.name + '</option>';
        }
        (document.getElementById("status") as HTMLSelectElement).innerHTML = htmlString;

        return null;
    }
    async  getSpecificationDropdown() {
        let res = await fetch(BASEURL + "/api/Device/specification", {
            headers: new Headers({ Authorization: `Bearer ${this.token}` }),
        });

        let metadata = JSON.parse(res.headers.get('X-Pagination'));
        let response = await fetch(BASEURL + "/api/Device/specification?page=0&page-size=" + metadata.TotalCount, {
            headers: new Headers({ Authorization: `Bearer ${this.token}` }),
        });
        let data = await response.json();
        console.log(data);
        (document.getElementById("specification") as HTMLSelectElement).innerHTML = "";
        for (let i = 0; i < data.length; i++) {
            (document.getElementById("specification") as HTMLSelectElement).innerHTML +=
                '<option value="' + data[i].specification_id + '">' + (data[i].ram == "" ? "" : " RAM: " +
                    data[i].ram) +
                (data[i].storage == "" ? "" : " Storage: " +
                    data[i].storage) +
                (data[i].screenSize == "" ? "" : " Screen Size: " +
                    data[i].screenSize) +
                (data[i].connectivity == "" ? "" : " Connectivity: " +
                    data[i].connectivity) +
                "</option>";
        }
        return null;
    }
    async Create_device() {
        let data = this.addDataFromForm();
        console.log(data);
        let res = await fetch(BASEURL + "/api/Device/add", {
            method: "POST",
            headers: new Headers([["Content-Type", "application/json"], ["Authorization", `Bearer ${this.token}`]]),
            body: data,
        });

        if (res.status == 200) {
            console.log("added Successfully");
            alert("Device Added");
            window.location.href = "./deviceListForadmin.html";
        }
        return null;
    }
    async getDataToForm() {
        let data: any
        const urlParams = new URLSearchParams(window.location.search);
        const myParam = urlParams.get("device_id");
        console.log("device_id" + myParam);
        const obj = new AddDevice(this.token);

        let res = await fetch(
            BASEURL + "/api/Device/device_id/" + myParam,
            {
                headers: new Headers({ "Authorization": `Bearer ${this.token}` })
            })
        data = await res.json();
        console.log(data);
        this.populateDataToForm(data);

        return null;
    }
    populateDataToForm(data: any) {


        (document.getElementById("inputbrand") as HTMLInputElement).value = data[0].brand;
        (document.getElementById("inputtype") as HTMLInputElement).value = data[0].type;
        (document.getElementById("status") as HTMLInputElement).value = data[0].status_id;
        (document.getElementById("inputmodel") as HTMLInputElement).value = data[0].model;
        (document.getElementById("serial_number") as HTMLInputElement).value = data[0].serial_number;
        (document.getElementById("color") as HTMLInputElement).value = data[0].color;
        (document.getElementById("price") as HTMLInputElement).value = data[0].price;
        (document.getElementById("warranty_year") as HTMLInputElement).value = data[0].warranty_year;
        (document.getElementById("purchase_date") as HTMLInputElement).value = formatDate1(data[0].purchase_date);
        (document.getElementById("specification") as HTMLInputElement).value = data[0].specification_id;
        (document.getElementById("entry_date") as HTMLInputElement).value = formatDate1(data[0].entry_date);

    }

    async update_device(device_id: any) {
        let data = this.addDataFromForm();
        console.log(data);
        let res = await fetch(BASEURL + "/api/Device/update/" + device_id, {
            method: "PUT",
            headers: new Headers([["Content-Type", "application/json"], ["Authorization", `Bearer ${this.token}`]]),
            body: data,
        });
        if (res.status == 200) {
            console.log("updated Successfully");
            alert("Device Updated");
            window.location.href = "./deviceListForadmin.html";
        }

    }
    addDataFromForm() {
        this.type = ((document.getElementById("inputtype") as HTMLSelectElement).value);
        this.brand = ((document.getElementById("inputbrand") as HTMLInputElement).value);
        this.status_id = +((document.getElementById("status") as HTMLInputElement).value);
        this.model = (document.getElementById("inputmodel") as HTMLInputElement).value;
        this.color = (document.getElementById("color") as HTMLInputElement).value;
        this.price = (document.getElementById("price") as HTMLInputElement).value;
        this.serial_number = (document.getElementById("serial_number") as HTMLInputElement).value;
        this.warranty_year = (document.getElementById("warranty_year") as HTMLInputElement).value;
        this.purchase_date = (document.getElementById("purchase_date") as HTMLInputElement).value;
        this.specification_id = +((document.getElementById("specification") as HTMLInputElement).value);
        this.entry_date = (document.getElementById("entry_date") as HTMLInputElement).value;
        return JSON.stringify(this);
    }
    addDataToField(element) {


        const data = new AddDevice(this.token);
        data.field = (document.getElementById(element) as HTMLInputElement).value;
        return JSON.stringify(data);
    }
    async addNewTypeBrandModel(URL: any, element) {

        let data1 = this.addDataToField(element);
        console.log(data1);
        let data = await fetch(BASEURL + URL, {
            method: "POST",
            headers: new Headers([["Content-Type", "application/json"], ["Authorization", `Bearer ${this.token}`]]),
            body: data1,
        });
        console.log("test"+data.status);
        return data.status;
    }


    addDataToSpecification() {
        const data = new AddDevice(this.token);
        data.ram = (document.getElementById("RAM") as HTMLInputElement).value;
        data.storage = (document.getElementById("Storage") as HTMLInputElement).value;
        data.screen_size = (document.getElementById("Screen_size") as HTMLInputElement).value;
        data.connectivity = (document.getElementById("Connectivity") as HTMLInputElement).value;
        return JSON.stringify(data);
    }
    async addNewSpecification() {

        let data1 = this.addDataToSpecification();
        console.log(data1);
        let data = await fetch(BASEURL + "/api/Device/addspecification", {
            method: "POST",
            headers: new Headers([["Content-Type", "application/json"], ["Authorization", `Bearer ${this.token}`]]),
            body: data1,
        });
        if (data.status == 200) {
            this.getSpecificationDropdown();
        }
        else {
            alert("Incorrect Specifications");
        }
        return null;
    }



}
