
import { BASEURL, navigationBarsss, Token, amIUser ,headersRows} from "./globals";

(async function() {
    const _ = Token.getInstance();
    const token = _.tokenKey;
	const role = (await amIUser(token)) == true ? "User" : "Admin";
// var data1 = new DeviceListForAdmin(data,token);
// const token = JSON.parse(localStorage.getItem("user_info"))["token"];
function getDeviceDetailById(device_id : any) {
    return fetch(BASEURL + "/api/device/" + device_id,
    {
        headers: new Headers({"Authorization": `Bearer ${token}`})})
        .then(res => res.json())
        .then(data => {

            devicedetails(data[0]);
        })
        .catch(err => console.error(err));
}
function devicedetails(data : any){
    document.getElementById("device-main").innerHTML = data.brand +" "+ data.model ;
    document.getElementById("color").innerHTML = data.color ;
    document.getElementById("price").innerHTML = data.price ;
    document.getElementById("serial_number").innerHTML = data.serialNumber ;
    document.getElementById("warranty_year").innerHTML = data.warrantyYear + " years" ;
    document.getElementById("purchase_date").innerHTML = data.purchaseDate ;
    document.getElementById("status").innerHTML = data.status ;
    document.getElementById("comments").innerHTML = data.comments ;
    document.getElementById("ram").innerHTML = data.specifications.ram ;
    document.getElementById("storage").innerHTML = data.specifications.storage;
    document.getElementById("screen_size").innerHTML = data.specifications.screenSize + " inches";
    document.getElementById("connectivity").innerHTML = data.specifications.connectivity;
}

const urlParams = new URLSearchParams(window.location.search);
const myParam = urlParams.get("device_id");
getDeviceDetailById(myParam);
navigationBarsss(role,"navigation");
headersRows(role,"row1");
})();