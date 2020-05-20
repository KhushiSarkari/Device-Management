
import { AddDevice } from './device_crud';
import { navigationBarsss,headersRows,Token } from './globals';
import { specificationDropdown } from './Device-Request/UserRequestMain';
const _ = Token.getInstance();
const token = _.tokenKey;
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
      window["tata"].text('New Type ','Added!',{duration:3000});
      const temp = new AddDevice(token);
      let status=await temp.addNewTypeBrandModel("/api/Device/type", "inputtype");
      if(status==200)
      {
        temp.typeDropdown();
      }
      else
      {
       
       window["tata"].text('New Type ','Not Added!',{duration:3000});
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
      window["tata"].text('New Brand ','Added!',{duration:3000});
      const temp = new AddDevice(token);
      let status = await temp.addNewTypeBrandModel("/api/Device/brand", "inputbrand");
      if(status==200){

        temp.brandDropdown();
      }
      else
     {
       // alert("New brand not added");
       window["tata"].text('New Brand ','Not Added!',{duration:3000});
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
      window["tata"].text('New Model ','Added!',{duration:3000});
      const temp = new AddDevice(token);
      let status = await temp.addNewTypeBrandModel("/api/Device/model", "inputmodel");
      if(status==200){
         temp.modelDropdown();
       }
       else
      {
         //alert("New Model not added");
         window["tata"].text('New Model ','Not Added!',{duration:3000});
      }
    }
    else
    {
      (document.getElementById("inputmodel") as HTMLInputElement).value="";
    }
  }
 
});
(document.querySelector('#addspecification') as HTMLButtonElement).addEventListener('click', async function (e) {
  console.log("inside function")
  e.preventDefault();
  const temp = new AddDevice(token);
  if(await temp.addNewSpecification() == true){
    window["tata"].text('New Specification ','Added!',{duration:3000});
    window["closeForm"]('popupForm');
  }
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
    window["tata"].text('Device ','Updated!',{duration:3000});
    
    // window.location.href = "./deviceListForadmin.html";
  }
  else {
    
    
    temp.Create_device();
   // window["tata"].text('New Device ','Added!',{duration:3000});
   
    
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
