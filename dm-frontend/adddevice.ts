
import { AddDevice } from './device_crud';
import { navigationBarsss } from './globals';
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
function checkTypeBrandModel() {

  return (brand.value && type.value && model.value);
}
// specification.addEventListener('blur', function (e) {
//   console.log("sg");
//   if (checkTypeBrandModel())
//   specificationDropdown(type.value, brand.value, model.value);
  
//    else
//     {
//       alert("please fill type,brand and model");
//     }
// });
(document.querySelector('#inputtype') as HTMLInputElement).addEventListener('change', function (e) {
  console.log((document.getElementById("inputtype") as HTMLInputElement).value)
  let types = checkDropDown("type",type);
  if (types == 0) {
    console.log("type");
    if (confirm("do you want to add new type")) {
      const temp = new AddDevice(token);
      temp.addNewTypeBrandModel("/api/Device/type", "inputtype");
      temp.typeDropdown();
    }
    else
    {
      (document.getElementById("inputtype") as HTMLInputElement).value="";
    }
  }
  if (checkTypeBrandModel())
  specificationDropdown(type.value, brand.value, model.value);
 
});
(document.querySelector('#inputbrand') as HTMLInputElement).addEventListener('change', function (e) {
  let brands = checkDropDown("brand",brand);
  if (brands == 0) {
    console.log("brands");
    if (confirm("do you want to add new brand")) {
      const temp = new AddDevice(token);
      temp.addNewTypeBrandModel("/api/Device/brand", "inputbrand");
      temp.brandDropdown();
    }
    else
    {
      (document.getElementById("inputbrand") as HTMLInputElement).value="";
    }
  }
  if (checkTypeBrandModel())
  specificationDropdown(type.value, brand.value, model.value);
 
});
(document.querySelector('#inputmodel') as HTMLInputElement).addEventListener('change', function (e) {
  let models = checkDropDown("model",model);
  if (models == 0) {
    console.log("model");
    if (confirm("do you want to add new model")) {
      const temp = new AddDevice(token);
      temp.addNewTypeBrandModel("/api/Device/model", "inputmodel");
      temp.modelDropdown();
    }
    else
    {
      (document.getElementById("inputmodel") as HTMLInputElement).value="";
    }
  }
  if (checkTypeBrandModel())
  specificationDropdown(type.value, brand.value, model.value);
 
});
(document.querySelector('#popup_specification') as HTMLButtonElement).addEventListener('submit', function (e) {
  console.log("inside function")
  e.preventDefault();
  const temp = new AddDevice(token);
  temp.addNewSpecification();
  (document.getElementById("popupForm") as HTMLFormElement).style.display = "none";
  // temp.specificationDropdown();

});

(document.querySelector('#back') as HTMLButtonElement).addEventListener('click', function (e) {
  window.location.href = "./deviceListForadmin.html";
});

(document.querySelector('#submit') as HTMLButtonElement).addEventListener('submit', function (e) {
  const urlParams = new URLSearchParams(window.location.search);
  const myParam = urlParams.get("device_id");
  console.log(myParam);
  e.preventDefault();
  const temp = new AddDevice(token);
   
  if (myParam) {
   // specificationDropdown(type.value, brand.value, model.value);
    temp.update_device(myParam);
    console.log("updated Successfully");
    alert("Device Updated");
    // window.location.href = "./deviceListForadmin.html";
  }
  else {

    temp.Create_device();
    console.log("added Successfully");
    alert("Device Added");
    //window.location.href = "./deviceListForadmin.html";
  }


});
navigationBarsss("Admin", "navigation");
const temp = new AddDevice(token);
temp.brandDropdown();
temp.typeDropdown();
temp.modelDropdown();
const urlParams = new URLSearchParams(window.location.search);
const myParam = urlParams.get("device_id");

if (myParam != null) {
  temp.getDataToForm();
 
}