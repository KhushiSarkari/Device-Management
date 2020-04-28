import { HitApi } from "./Device-Request/HitRequestApi";

export const BASEURL = "http://localhost:5000";

export function amIAdmin(token: string){
   return new HitApi(token).HitGetApi(BASEURL + "/api/is_admin")
    .then(res => res.result as boolean);
}
export function amIUser(token: string){
    return new HitApi(token).HitGetApi(BASEURL + "/api/is_user")
    .then(res => res.result as boolean);

}
export function navigationBarsss(role:string , element:string) {
    var navigation = `<nav class="demo-navigation mdl-navigation mdl-color--blue-grey-800" >
    <a class="mdl-navigation__link" href="/dashboard.html">
   <i class="mdl-color-text--blue-grey-400 material-icons"
       role="presentation">dashboard</i>Dashboard
    </a>
     <a class="mdl-navigation__link" href="/deviceListForadmin.html">
         <i class="mdl-color-text--blue-grey-400 material-icons" role="presentation">devices</i>All
             Devices
    </a>
    
    <a class="mdl-navigation__link" href="/Device-Request/device_request.html">
    <i class="mdl-color-text--blue-grey-400 material-icons"
            role="presentation">import_export</i>Request Device
    </a>
    <a class="mdl-navigation__link" href="/userRequestHistory.html">
        <i class="mdl-color-text--blue-grey-400 material-icons" role="presentation">storage</i>My
        Devices
    </a>`;
    if (role == "Admin") {
        let nav = ` 
        <a class="mdl-navigation__link" href="/specification.html">
        <i class="mdl-color-text--blue-grey-400 material-icons" role="presentation">devices</i>All Specifications
    </a>
    
   
    <a class="mdl-navigation__link" href="/web.html">
        <i class="mdl-color-text--blue-grey-400 material-icons" role="presentation">group</i>Users
    </a>
    <a class="mdl-navigation__link" href="/adminRequestPage.html">
        <i class="mdl-color-text--blue-grey-400 material-icons"
            role="presentation">import_export</i>All Requests
    </a>
    <a class="mdl-navigation__link" href="/request-history/request-history.html">
        <i class="mdl-color-text--blue-grey-400 material-icons" role="presentation">history</i>Request History
    </a>
    <a class="mdl-navigation__link" href="/faultyDevice/faultdevice.html">
    <i class="mdl-color-text--blue-grey-400 material-icons" role="presentation">report_problem</i>
    Complaints
     </a>
   
    <a class="mdl-navigation__link" href="/device_role/role.html">
        <i class="mdl-color-text--blue-grey-400 material-icons"
            role="presentation">perm_device_information</i>Roles
    </a></nav>
    `;
        document.getElementById(element).innerHTML = navigation + nav;
    }
   else if(role == "User")
   {
    document.getElementById(element).innerHTML = navigation;
}

}
export function paging(start:number=1,end:number=4)
{
    (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value="<<" >`;
    for (let loop = start ; loop <= end ; loop++)
    (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="${loop}" value="${loop}" >`;
    (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value=">>" >`;
}
export function PageNo(page_no,page_size=6)
{
    let uri= "?page="+page_no + "&page-size="+page_size;
    return uri;
}
// export function getPageNo(page_no,value:string)
// {

// 		  if(value==">>")
// 		{
// 			page_no+=1;
// 		}
// 		else if(value=="<<")
// 		{
// 			page_no = page_no-1;
// 		}
// 		else
// 		{
// 			page_no=+(value);
//         }
//         return page_no;
// }
export class Token    /// call static method that return an object 
{
    userID:number 
    tokenKey :string
    private constructor(){ 
        this.tokenKey = JSON.parse(sessionStorage.getItem("user_info"))["token"];
        this.userID = parseInt(JSON.parse(sessionStorage.getItem("user_info"))["id"]);
     }
     static  getInstance():Token
    {
        return new Token();
    }
}