import { PopulateData } from "./FillData";
import { findResult } from "./search"
import { Sorting } from "./Sorting";
import { HtmlElementsData } from "./HtmlElementsId";
// import { page } from "./paging";
import { UserRequestStatus } from "./RequestStatus";
import {navigationBarsss, BASEURL, paging,headersRows} from "../globals";
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
    new HitApi(token).HitGetApi(uri)
    fetch(uri,{
        headers: new Headers({"Authorization": `Bearer ${token}`})
        
    })
        .then(response =>{
            let metadata=JSON.parse(response.headers.get('X-Pagination'));
            paging(metadata);
            return response.json()
        })
        .then(data =>
        {console.log(data);
       // new page(token).setPages(data["resultCount"]);
       new PopulateData().fillData(data);
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

    // (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
    // {   
    //     console.log((e.target as HTMLButtonElement).value);
    //     var x  = (e.target as HTMLButtonElement).value;
      
    //     // getData(new page(token).slectedPage(x));
    // });
    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 
        let currentPage = parseInt(document.getElementById("pagination").getAttribute("data-currentpage"));
        if((e.target as HTMLButtonElement).value==">>"){
            currentPage+=1
            document.getElementById("pagination").setAttribute("data-currentpage" ,currentPage.toString());

        }
		else if((e.target as HTMLButtonElement).value=="<<")
			{ currentPage-=1
                document.getElementById("pagination").setAttribute("data-currentpage" ,currentPage.toString());}
		else
			{
                currentPage = parseInt((e.target as HTMLButtonElement).value);
                document.getElementById("pagination").setAttribute("data-currentpage" ,currentPage.toString());
            }
            getData(slectedPage(currentPage));
		// temp.getData(PageNo(temp.currentPage));   
    });

    function slectedPage(value: number)
        {
            let totalRowsInTable =5;
             let offset = (value);
           let domElements = new HtmlElementsData();
            let userName = (document.getElementById(new HtmlElementsData().search)  as HTMLInputElement).getAttribute(domElements.userName);
            var sortAttribute = (document.getElementById(domElements.thead) as HTMLTableRowElement).getAttribute(domElements.sortAttributr);       
            var sortType  =  (document.getElementById(domElements.thead) as HTMLTableRowElement).getAttribute(domElements.sortType);
            let requestStatus = new Sorting(token).getStatus();
            let uri = "?user-name="+encodeURI(userName)+"&sort="+sortAttribute+"&sort-type="+sortType+"&page="+offset+"&page-size="+totalRowsInTable +"&status="+requestStatus;
            return uri;
             
        }
navigationBarsss("Admin","navigation");
headersRows("Admin","row1");

}());


