import { PopulateData } from "./FillData";
import { findResult } from "./search"
import { Sorting } from "./Sorting";
import { HtmlElementsData } from "./HtmlElementsId";
import { page } from "./paging";
import { UserRequestStatus } from "./RequestStatus";
import {navigationBarsss, BASEURL} from "../globals";
import { HitApi } from "../Device-Request/HitRequestApi";



(function(){

let token=JSON.parse(sessionStorage.getItem("user_info"))["token"];
let url = BASEURL +"/sorting";
var domElement =  new HtmlElementsData();
(function(){
    var prams = window.location.href;
    var pramList = prams.split("?")
    if(pramList.length > 1)
    {
        getData("?"+pramList[1])
    }
    else 
    getData();

})();

function getData(params = "")
{
    let uri  = url + params;
    (document.getElementById("loading") as HTMLDivElement).style.display = "flex"; 
    new HitApi(token).HitGetApi(uri).then(data =>
        {console.log(data);
        new page(token).setPages(data["resultCount"]);
       new PopulateData().fillData(data["results"]);
       (document.getElementById("loading") as HTMLDivElement).style.display = "none";  
});
}


function clearFields()
{
    (document.getElementById(domElement.search) as HTMLInputElement).value = "";
    (document.getElementById(domElement.devicesearch) as HTMLInputElement).value = "";
}

document.addEventListener('keypress' , event =>
{
    let element = (event.target as HTMLInputElement).id
    
    if ((element == "waterfall-exp" || element == "device_serial_number") && event.key  == "Enter"){
        (document.getElementById("request-status") as HTMLSelectElement).selectedIndex = 0; 
    let params = new findResult(token).searchUser();
    getData(params);
    }
})

document.querySelector("#getdata").addEventListener('click',event =>{

    clearFields();
    (document.getElementById("request-status") as HTMLSelectElement).selectedIndex = 0;        // assign , reject , all 
    getData();
});

document.querySelector("#tableHead").addEventListener('click', function (e) {
            let id = (e.target as HTMLInputElement).id;
            if (id === "user"|| id === "admin" ||  id === "serialNumber" || id === "device_name") 
                    {  
                getData(new Sorting(token).sortBy(id));
            }});

(document.querySelector("#request-status") as HTMLSelectElement).addEventListener("change" ,e =>
    {
      
        // var requestStatus = (e.target as HTMLOptionElement).value;
        let  requestStatus = new Sorting(token).getStatus();
        if( requestStatus == "")
            clearFields();
        getData(new UserRequestStatus(token).generateRequestData(requestStatus));
    });

    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
    {   
        console.log((e.target as HTMLButtonElement).value);
        var x  = (e.target as HTMLButtonElement).value;
      
        getData(new page(token).slectedPage(x));
    });
navigationBarsss("Admin","navigation");
}());


