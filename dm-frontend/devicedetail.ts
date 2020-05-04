import { DeviceListForAdmin } from './deviceListForAdmin';
import { BASEURL, navigationBarsss, Token, amIUser ,headersRows} from "./globals";

(async function() {
	const token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
	const role = (await amIUser(token)) == true ? "User" : "Admin";
// var data1 = new DeviceListForAdmin(data,token);
// const token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
function getDeviceDetailById(device_id : any) {
    return fetch(BASEURL + "/api/device/" + device_id,
    {
        headers: new Headers({"Authorization": `Bearer ${token}`})})
        .then(res => res.json())
        .then(data => {
            //console.log(data);
            let data1 =  new DeviceListForAdmin(data,token);
            devicedetails(data1);
        })
        .catch(err => console.error(err));
}
function devicedetails(data : any){
    document.getElementById("device-main").innerHTML = data.brand +" "+ data.model ;
    document.getElementById("color").innerHTML = data.color ;
    document.getElementById("price").innerHTML = data.price ;
    document.getElementById("serial_number").innerHTML = data.serial_number ;
    document.getElementById("warranty_year").innerHTML = data.warranty_year + " years" ;
    document.getElementById("purchase_date").innerHTML = data.purchase_date ;
    document.getElementById("status").innerHTML = data.status ;
    document.getElementById("comments").innerHTML = data.comments ;
    document.getElementById("ram").innerHTML = data.ram ;
    document.getElementById("storage").innerHTML = data.storage ;
    document.getElementById("screen_size").innerHTML = data.screen_size + " inches";
    document.getElementById("connectivity").innerHTML = data.connectivity ;
    //console.log(data);
}

const urlParams = new URLSearchParams(window.location.search);
const myParam = urlParams.get("device_id");
getDeviceDetailById(myParam);
navigationBarsss(role,"navigation");
headersRows(role,"row1");
})();