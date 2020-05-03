
import { AddDevice } from './device_crud';
import { navigationBarsss,headersRows } from './globals';
import { specificationDropdown } from './Device-Request/UserRequestMain';
let token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
let brand = (document.getElementById("inputbrand") as HTMLInputElement);
let type = (document.getElementById("inputtype") as HTMLInputElement);
let model = (document.getElementById("inputmodel") as HTMLInputElement);
let specification = (document.getElementById("specification") as HTMLSelectElement);
function checkDropDown(elements: string,compareElement) {
  let flag = 0;
  let option = (document.getElementById(elements)as HTMLDataListElement).options;
  Array.from(option).forEach(element => {
    if (compareElement.value == element.value) {
      flag = 1;
    }
  });

  return flag;
}
(document.querySelector('#inputtype') as HTMLInputElement).addEventListener('change', async function (e) {
  console.log((document.getElementById("inputtype") as HTMLInputElement).value)
  let types = checkDropDown("type",type);
  if (types == 0) {
    console.log("type");
    if (confirm("do you want to add new type")) {
      const temp = new AddDevice(token);
      let status=await temp.addNewTypeBrandModel("/api/Device/type", "inputtype");
      if(status==200)
      {
        temp.typeDropdown();
      }
      else
      {
        alert("New type not added");
      }
    }
    else
    {
      (document.getElementById("inputtype") as HTMLInputElement).value="";
    }
  }
});
(document.querySelector('#inputbrand') as HTMLInputElement).addEventListener('change', async function (e) {
  let brands = checkDropDown("brand",brand);
  if (brands == 0) {
    console.log("brands");
    if (confirm("do you want to add new brand")) {
      const temp = new AddDevice(token);
      let status = await temp.addNewTypeBrandModel("/api/Device/brand", "inputbrand");
      if(status==200){
        temp.brandDropdown();
      }
      else
     {
        alert("New brand not added");
     }
    }
    else
    {
      (document.getElementById("inputbrand") as HTMLInputElement).value="";
    }
  }
});
(document.querySelector('#inputmodel') as HTMLInputElement).addEventListener('change', async function (e) {
  let models = checkDropDown("model",model);
  if (models == 0) {
    console.log("model");
    if (confirm("do you want to add new model")) {
      const temp = new AddDevice(token);
      let status = await temp.addNewTypeBrandModel("/api/Device/model", "inputmodel");
      if(status==200){
         temp.modelDropdown();
       }
       else
      {
         alert("New Model not added");
      }
    }
    else
    {
      (document.getElementById("inputmodel") as HTMLInputElement).value="";
    }
  }
 
});
(document.querySelector('#addspecification') as HTMLButtonElement).addEventListener('click', function (e) {
  console.log("inside function")
  e.preventDefault();
  const temp = new AddDevice(token);
  temp.addNewSpecification();
 (document.getElementById("popupForm") as HTMLFormElement).style.display = "none";
 
});

(document.querySelector('#back') as HTMLButtonElement).addEventListener('click', function (e) {
  window.location.href = "./deviceListForadmin.html";
});

window.addEventListener('submit', function (e) {
  console.log("add");
  const urlParams = new URLSearchParams(window.location.search);
  const myParam = urlParams.get("device_id");
  console.log(myParam);
  e.preventDefault();
  const temp = new AddDevice(token);
   
  if (myParam) {
   
    temp.update_device(myParam);
    
    // window.location.href = "./deviceListForadmin.html";
  }
  else {
    
    temp.Create_device();
   
    
  }
   

});
navigationBarsss("Admin", "navigation");
headersRows("Admin","row1");
const temp = new AddDevice(token);
temp.brandDropdown();
temp.typeDropdown();
temp.modelDropdown();
temp.getSpecificationDropdown();
temp.statusDropdown();
const urlParams = new URLSearchParams(window.location.search);
const myParam = urlParams.get("device_id");
if (myParam != null) {
  temp.getDataToForm();
 
}
