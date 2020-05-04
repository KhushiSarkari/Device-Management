// import { HtmlElementsData } from "./HtmlElementsId";
// import { Sorting } from "./Sorting";

// export class page 
// {
//     domElements : HtmlElementsData
//     totalRowsInTable : number =10
//     token: string;

//     constructor(token: string){
//         this.token = token;
//     }


//     slectedPage(value: string)
//     {
//         let offset = parseInt(value);
//         this.domElements = new HtmlElementsData();
//         let userName = (document.getElementById(new HtmlElementsData().search)  as HTMLInputElement).getAttribute(this.domElements.userName);
//         var sortAttribute = (document.getElementById(this.domElements.thead) as HTMLTableRowElement).getAttribute(this.domElements.sortAttributr);       
//         var sortType  =  (document.getElementById(this.domElements.thead) as HTMLTableRowElement).getAttribute(this.domElements.sortType);
//         let requestStatus = new Sorting(this.token).getStatus();
//         let uri = "?user-name="+encodeURI(userName)+"&sort="+sortAttribute+"&sort-type="+sortType+"&page="+offset+"&page-size="+this.totalRowsInTable +"&status="+requestStatus;
//         return uri;
         
//     }


//     public addPageElement( end : number , start : number =1,)
//         {
//             this.clearData();
//            (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value="<<" >`;
//             for (let loop = start ; loop <= end ; loop++)
//             (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="${loop}" value="${loop}" >`;
//             (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value=">>" >`;
//         }
        
//         public setPages(size : number)
//         {
//             var high =  Math.ceil(size/this.totalRowsInTable);
//             this.addPageElement(high);
//         }
//         clearData()
//         {
//             (document.getElementById("pagination") as HTMLDivElement).innerHTML = "";
//         }
// }