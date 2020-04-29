 import { Api } from "./HitSubmissionApi";
import { RequestDeviceModel } from "./Device-Request/deviceRequestModel";
import { RequestSubmitModel } from "./SubmissionRequestModel";
import { populateData } from "./genrateSubmissionRequest";
import { Sort } from "./user-profile/SortingUser";
import { BASEURL, navigationBarsss, PageNo, current_page, paging } from "./globals";

(async function(){
    let address = BASEURL;
    let token=JSON.parse(sessionStorage.getItem("user_info"))["token"];
    let adminId = JSON.parse(sessionStorage.getItem("user_info"))["id"];
    let currentPage:number=current_page;


    (document.querySelector("#fixed-header-drawer-exp")  as HTMLInputElement).addEventListener("keypress" , event =>
    {
        if(event.key == "Enter")
        {
            
            let id = (event.target as HTMLInputElement).id;
            let search = (document.getElementById(id) as HTMLInputElement).value;
            (document.getElementById(id) as HTMLInputElement).setAttribute("data-device-name" , search);
            let url = address + "/api/ReturnRequest" + getSearchUrl();
            console.log(url);
            getData(url);
        }
    });

    function getSearchUrl()
    {
        let url;
        let search = (document.getElementById("fixed-header-drawer-exp") as HTMLInputElement).getAttribute("data-device-name")
        if (search != "")
        return "?search="+search;
        else 
        return "?";
    }


    document.querySelector("#tableHead").addEventListener('click' , event =>
    {
        let id = (event.target as HTMLInputElement).dataset.id;
        if(id == "name" || id == "type" || id == "device" )
        {   
            let sortAttributr = (event.target as HTMLTableHeaderCellElement).dataset.id;
            let sort = new Sort(token);
        var sortType =  sort.checkSortType((event.target as HTMLTableHeaderCellElement));
        let url = address +  "/api/ReturnRequest" + getSearchUrl() +"&sort="+sortAttributr +"&direction="+ sortType +"&"+PageNo(currentPage);
            getData(url);
        }
    });


    function getAll()
    {
        let url =  address  + "/api/ReturnRequest?"+PageNo(currentPage);
        (document.getElementById("fixed-header-drawer-exp") as HTMLInputElement).setAttribute("data-device-name" , "");
        getData(url);
    }

    async function getData(url)
    {
        var elementId = "submission_request";
        var generateData = new populateData(elementId);
        
        var data = await new Api(token).hitGetApi(url);
        data.map(value=>
            {
                console.log("sdfghjk")
            var data = new RequestSubmitModel().bindData(value);
            generateData.generateField(data , elementId);
            })
        return null;
    }

    document.addEventListener("click", function (event) {
        if ((event.target as HTMLButtonElement).className == "accept") {
            
            var returnId = (event.target as HTMLButtonElement).dataset.returnId
            if(confirm("Are you sure you want to approve the return?"))
            {   let url = address + "/api/ReturnRequest/"+ returnId +"?action=accept&id="+adminId;
                alert("Device return approved");
                new Api(token).hitGetApi(url);
                getAll();
            }
            
    }
    else if ((event.target as HTMLButtonElement).className == "reject") {
            
        var returnId = (event.target as HTMLButtonElement).dataset.returnId
        if(confirm("Are you sure you want to reject the return?"))
        {   let url = address + "/api/ReturnRequest/"+ returnId +"?action=reject&id="+adminId;
            alert("Device return rejected");
            new Api(token).hitGetApi(url);
            getAll();
        }
        
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
		getAll();
    });

    document.querySelector("#getData").addEventListener("click" , event=>
        {
            getAll();
        });
    getAll();
    navigationBarsss("Admin","navigation");
    return null;
})();