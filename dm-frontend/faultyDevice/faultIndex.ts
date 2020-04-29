import { BASEURL, Token, navigationBarsss, PageNo, current_page, paging  } from "../globals";
import { HitApi } from "../Device-Request/HitRequestApi";
import { FaultyDeviceModel } from "./FaultyDeviceModel";
import { FalultyDevice } from "./Fault";
import { Sort } from "../user-profile/SortingUser";


let token = Token.getInstance();
let currentPage:number=current_page;



navigationBarsss("Admin" , "navigate");

new FalultyDevice().getAllData("?"+PageNo(currentPage));

(document.querySelector("#waterfall-exp") as HTMLInputElement).addEventListener("keypress", function (event) {
  if (event.key == "Enter") {
    var data = (document.getElementById("waterfall-exp") as HTMLInputElement).value;
    document.getElementById("waterfall-exp").setAttribute("data-find" , data);
    document.getElementById("waterfall-exp").setAttribute("data-find" , (document.getElementById("serial-number") as HTMLInputElement).value);
    new FalultyDevice().getAllData( getSearch());
   
  }
});



document.addEventListener('click' , event =>
{
  var element = (event.target as HTMLButtonElement).className;
  if(element === "faulty-device" ||  element ==="fault-resolved")
  {
  let url  = BASEURL + "/api/FaultyDevice"
  let id = parseInt((event.target as HTMLButtonElement).dataset.complaint);
  if(element == "faulty-device")
    new HitApi(token.tokenKey).HitPutApi( url + "/markfaulty" , {complaintId  : id } )
  if(element == "fault-resolved")
    new HitApi(token.tokenKey).HitPutApi( url + "/resolve" ,  {complaintId  : id } )
  new FalultyDevice().getAllData();
  }
});


(document.querySelector("#tableHead") as HTMLTableHeaderCellElement).addEventListener("click", function (e) {
  let id = (e.target as HTMLInputElement).id;
  if (id === "user"|| id === "date" ||  id === "serialnumber" || id === "device") 
          {
            var element = (document.getElementById(id) as HTMLTableHeaderCellElement);
            var direction =  "&direction=" + new Sort(token.tokenKey).checkSortType(element);;
            var attribute = "?sort=" + id;
            new FalultyDevice().getAllData( attribute + direction + "&"+PageNo(currentPage));
  }});


(document.querySelector("#getdata") as HTMLInputElement).addEventListener("click", function (event) {

  (document.getElementById("waterfall-exp") as HTMLInputElement).value = "";
  (document.getElementById("serial-number") as HTMLInputElement).value ="";
  document.getElementById("waterfall-exp").setAttribute("data-find" , "");
  document.getElementById("serial-number").setAttribute("data-find" , "");
  new FalultyDevice().getAllData("?"+PageNo(currentPage));

});

(document.querySelector("#serial-number") as HTMLInputElement).addEventListener("keypress", function (event) {
  if (event.key == "Enter") {
    var data = (document.getElementById("serial-number") as HTMLInputElement).value;
    document.getElementById("serial-number").setAttribute("data-find" , data);
    document.getElementById("serial-number").setAttribute("data-find" , (document.getElementById("waterfall-exp") as HTMLInputElement).value);
    new FalultyDevice().getAllData( getSearch());
  }
});

(document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 
		if((e.target as HTMLButtonElement).value==">>")
		{
			currentPage+=1;
		}
		else if((e.target as HTMLButtonElement).value=="<<")
		{
			currentPage-=1;
		}
		else
		{
			currentPage=+((e.target as HTMLButtonElement).value);
		}
	       console.log((e.target as HTMLButtonElement).value);
         new FalultyDevice().getAllData("?"+PageNo(currentPage));  
    });

function getSearch() {
  let url = "";
  var obj = new FalultyDevice();
  url = "?"+PageNo(currentPage)+"&search=" + obj.getSearchData("waterfall-exp") + "&serial-number=" +  obj.getSearchData("serial-number");
  return  url;
  
}

