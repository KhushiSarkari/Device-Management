import { HitApi } from "./HitRequestApi";
import { populateDropDown } from "./populateDropDown";
import { RequestDeviceModel } from "./deviceRequestModel";
import { BASEURL, Token, amIUser, navigationBarsss } from "../globals";
import { CreateUserApi } from "../createApi";

(async function () {
  let obj = Token.getInstance();
  let role = (await amIUser(obj.tokenKey)) == true ? "User" : "Admin";
  navigationBarsss(role, "navigations");
  return null;
})();

var device = [].map.call(document.querySelectorAll(".device"), (e) => {
  return e;
});
(function (){
    let path = BASEURL + "/api/dropdown/" +device[0].id;
    let data = GetData(path, device[0].id);
})();

document.addEventListener("change", (event) => {
  let dropdownlist  = new populateDropDown();
  if ((event.target as HTMLSelectElement).id == "types")
  {
    dropdownlist.clear(device[1].id);
    dropdownlist.clear(device[2].id);
    dropdownlist.clear("specification");
    GetData( BASEURL + "/api/dropdown/" + device[1].id+"/" + device[0].value , device[1].id);
   
  }
  if ((event.target as HTMLSelectElement).id == "brands")
  {

    dropdownlist.clear(device[2].id);
    dropdownlist.clear("specification");
    GetData( BASEURL + "/api/dropdown/" + device[2].id+"/" + device[1].value , device[2].id);

  }

  if ((event.target as HTMLSelectElement).className == "device") {
    if (validateDevice()) {
      specificationDropdown(device[0].value, device[1].value, device[2].value);
    }
  }
});

export async function specificationDropdown(
  types: string,
  brands: string,
  models: string
) {

  let uri =
    BASEURL +
    "/api/Dropdown/" +
    encodeURI(types) +
    "/" +
    encodeURI(brands) +
    "/" +
    encodeURI(models) +
    "/specification";
  let obj = Token.getInstance();

  let data = await new HitApi(obj.tokenKey).HitGetApi(uri);


  (document.getElementById("specification") as HTMLSelectElement).innerHTML = "";
  for (let i = 0; i < data.length; i++) {
    (document.getElementById("specification") as HTMLSelectElement).innerHTML += '<option value="' + data[i].specification_id +'">' +
      (data[i].ram == "" ? "" : " RAM: " + data[i].ram) +
      (data[i].storage == "" ? "" : " Storage: " + data[i].storage) +
      (data[i].screenSize == "" ? "" : " Screen Size: " + data[i].screenSize) +
      (data[i].connectivity == ""
        ? ""
        : " Connectivity: " + data[i].connectivity) +
      "</option>";
  }
  return null;
}

async function GetData(uri: string, column: string) {
  
  (document.getElementById("loading") as HTMLDivElement).style.display = "flex"; 
  let obj = Token.getInstance();

  let data = await new HitApi(obj.tokenKey).HitGetApi(uri);
  new populateDropDown().populateDevice(data, column);
  (document.getElementById("loading") as HTMLDivElement).style.display = "none";    
  return data;
}

document.querySelector("#request")?.addEventListener("click", (e) => {
  let obj = Token.getInstance();
  var body = bindData(obj.userID);
  if (validate()) {
    let uri = BASEURL + "/api/request/device";

    new HitApi(obj.tokenKey).HitPostApi(uri, body).then((res) => {
      if (res.status == 200)
      { 
        hide(".container")
        display(".bg-model")
        document.getElementById("device-request-message").innerHTML = "Your request submitted"
       //window["tata"].text('Your Request Submitted!',{duration:30000});
    }
      else{ 
        display(".bg-model");
        hide(".container");
        document.getElementById("device-request-message").innerHTML = "Sorry for inconvenience. Please try after some time..."
  }//window["tata"].console.error('Your Request Submitted!',{duration:30000});
    });
    clearData();
  }
});


function  display(value)
{
  (document.querySelector(value) as HTMLDivElement).style.display =
        "flex";
}
function hide(value)
{
  (document.querySelector(value) as HTMLDivElement).style.display =
        "none";
}

document.querySelector("#rmybtn1").addEventListener('click', function (e) {
  hide(".bg-model");
  display(".container");
 
  document.getElementById("device-request-message").innerHTML = ""
});
document.querySelector(".x").addEventListener("click", function() {
  hide(".bg-model");
  display(".container");
    document.getElementById("device-request-message").innerHTML = ""
});

function clearData() {
  let dropdownlist  = new populateDropDown();
  (device[0] as HTMLSelectElement).selectedIndex = 0;
  dropdownlist.clear(device[1].id);
  dropdownlist.clear(device[2].id);
  (document.getElementById("expected-days") as HTMLInputElement).value = "";
  (document.getElementById("specification") as HTMLSelectElement).innerHTML = "";
  (document.getElementById("comment") as HTMLTextAreaElement).value = "";
}

function validate() {
  if (
    (document.getElementById("expected-days") as HTMLInputElement).value == ""
  ) {
    alert("enter days");
    return 0;
  }
  if (!validateDevice()) {
    alert("enter device name  ");
    return 0;
  }

  if (
    (document.getElementById("specification") as HTMLInputElement).value == ""
  ) {
    alert("enter specification");
    return 0;
  }
  return 1;
}

function validateDevice() {
  for (let data of device)
    if (data.value == "") {
      return 0;
    }

  return 1;
}

function bindData(userID: number) {
  var body = new RequestDeviceModel();
  body.userId = userID;
  body.devicetype = device[0].value;
  body.brand = device[1].value;
  body.model = device[2].value;
  body.days = parseInt(
    (document.getElementById("expected-days") as HTMLInputElement).value
  );
  body.specificationId = parseInt(
    (document.getElementById("specification") as HTMLSelectElement).value
  );
  body.comment = (document.getElementById(
    "comment"
  ) as HTMLTextAreaElement).value;
  return body;
}