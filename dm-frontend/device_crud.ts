import { BASEURL, paging, Token } from "./globals";
import { formatDate1 } from "./utilities";
import { HitApi } from "./Device-Request/HitRequestApi";
import { populateDropdown } from "./dropdown";
import { specificationDropdown } from "./Device-Request/UserRequestMain";
import { connectivityvalidation } from "./validation";

export class AddDevice {
    Brand:string
    Model:string
    Type:string
    DeviceType: string;
    DeviceBrand: string;
    StatusId: number;
    DeviceModel: string;
    Color: string;
    Price: string;
    SerialNumber: string;
    WarrantyYear: string;
    PurchaseDate: string;
    SpecificationId: number;
    EntryDate: string;
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
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.token}`
            },
            body: data,
        });
        
        if (res.status == 200) {
            console.log("added Successfully");
            window["tata"].text('Device ','Added!',{duration:3000});
            window.location.href = "./deviceListForadmin.html";
        }
        
        else{
            window["tata"].error('Device Not','Added!',{duration:3000});
        }
        
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
        this.DeviceType = ((document.getElementById("inputtype") as HTMLSelectElement).value);
        this.DeviceBrand = ((document.getElementById("inputbrand") as HTMLInputElement).value);
        this.StatusId = +((document.getElementById("status") as HTMLInputElement).value);
        this.DeviceModel = (document.getElementById("inputmodel") as HTMLInputElement).value;
        this.Color = (document.getElementById("color") as HTMLInputElement).value;
        this.Price = (document.getElementById("price") as HTMLInputElement).value;
        this.SerialNumber = (document.getElementById("serial_number") as HTMLInputElement).value;
        this.WarrantyYear = (document.getElementById("warranty_year") as HTMLInputElement).value;
        this.PurchaseDate = (document.getElementById("purchase_date") as HTMLInputElement).value;
        this.SpecificationId = +((document.getElementById("specification") as HTMLInputElement).value);
        this.EntryDate = (document.getElementById("entry_date") as HTMLInputElement).value;
        return JSON.stringify(this);
    }
    async postTypeBrandModel(jsonData,URL)
    {
        let data = await fetch(BASEURL + "/api/Device/"+URL, {
            method: "POST",
            headers: new Headers([["Content-Type", "application/json"], ["Authorization", `Bearer ${this.token}`]]),
            body: jsonData,
        });
        return data.status;
    }
    addNewType() {
        const TypeData = new AddDevice(this.token);
       TypeData.Type = (document.getElementById("inputtype") as HTMLInputElement).value;
       let data1 = JSON.stringify(TypeData)
        return this.postTypeBrandModel(data1 ,"type");
       
    }
    async addNewBrand() {
        const TypeData = new AddDevice(this.token);
       TypeData.Brand = (document.getElementById("inputbrand") as HTMLInputElement).value;
       let data1 = JSON.stringify(TypeData)
       return this.postTypeBrandModel(data1,"brand");
    }
    async addNewModel() {
        const TypeData = new AddDevice(this.token);
       TypeData.Model = (document.getElementById("inputmodel") as HTMLInputElement).value;
       let data1 = JSON.stringify(TypeData)
      return this.postTypeBrandModel(data1,"model");
    }


    addDataToSpecification() {
        const data = new AddDevice(this.token);
        data.ram = getValueOrNull("#RAM");
        data.storage = getValueOrNull("#Storage");
        data.screen_size = getValueOrNull("#Screen_size");
        data.connectivity = getValueOrNull("#Connectivity");
        return JSON.stringify(data);
    }
    async addNewSpecification() {
        if(connectivityvalidation() == 1){
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
            return true;
        }
        return false;
        
    }



}
// Need it to avoid sending "" in  request payload
export function getValueOrNull(selector: string) : string | null{
    const value = (document.querySelector(selector) as HTMLInputElement).value;
    return value || null;
}