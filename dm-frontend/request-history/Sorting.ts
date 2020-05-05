import { HtmlElementsData } from "./HtmlElementsId";
import { findResult } from "./search";
import { Sort } from "../user-profile/SortingUser";


export class Sorting
{    
    token:string;
    elements : HtmlElementsData;
    constructor(token: string)
    {
        this.token = token;
        this.elements = new HtmlElementsData();
    }
    sortBy( attributeId : string )
    {
        const element = (document.getElementById(attributeId) as HTMLTableHeaderCellElement);
        let  sortType = new Sort(this.token).checkSortType(element);
        document.getElementById("tableHead").setAttribute("data-sort" , sortType );
        document.getElementById("tableHead").setAttribute("data-sortby" , attributeId);
        return this.setSortingApiCall(attributeId , sortType);
    }
   
    setSortingApiCall(sortAttribute : string , sortType : string)
    {
        const status = this.getStatus();
        
        const uri= new findResult(this.token).searchUser() +"&status="+status+"&sort="+sortAttribute+"&sort-type="+sortType;
        return uri;
    }


    getSortingAndSearchingUri()
    {
        const sort =  document.getElementById("tableHead").getAttribute("data-sort" );
        const sortby =  document.getElementById("tableHead").getAttribute("data-sortby");
        return this.setSortingApiCall(sortby , sort);
    }
     getStatus() : string
{
    let requestStatus = (document.getElementById("request-status") as HTMLSelectElement).value;
    if (requestStatus ==  "Returned" || requestStatus == "returned")
        requestStatus = "Returned";
    else if (requestStatus ==  "Rejected" || requestStatus ==  "reject")
        requestStatus = "Rejected";
    else 
        requestStatus = "";
    return requestStatus;
}
}