import { HitApi } from "./Device-Request/HitRequestApi";

export const BASEURL = "http://localhost:5000";
export var page_size=4;
export var current_page=1;

export function amIAdmin(token: string){
   return new HitApi(token).HitGetApi(BASEURL + "/api/is_admin")
    .then(res => res.result as boolean);
}
export function amIUser(token: string){
    return new HitApi(token).HitGetApi(BASEURL + "/api/is_user")
    .then(res => res.result as boolean);


}
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





export function headersRows(role:string , element:string){
    var row1=`
           <div class="mdl-js" >
                    <nav class="mdl-navigation"  >
                     <div class="material-icons mdl-badge mdl-badge--overlap" id="notifications" data-badge="" style="cursor:pointer">notifications</div>
                      
       
                     <button id="profile" class="mdl-button mdl-js-button mdl-button--icon">
                     <i class="material-icons">person_pin</i>
                 </button>
                 <ul class="mdl-menu mdl-js-menu mdl-menu--bottom-right" for="profile">
                     <a href='/user-profile/index.html'>
                         <li class="mdl-menu__item" id="#userProfile" >Profile</li></a>
                     
                     <li class="mdl-menu__item" id="logout">Logout</li>
                 </ul>`;

          
     
       

    
    if (role == "Admin" ) {
        
        let r2= ` 
        <span class="material-icons mdl-badge mdl-badge--overlap" id="submissionNotification"></span>
        
    `;

    this.row1+=` </nav>
    </div>
   
`;


document.getElementById(element).innerHTML = row1+r2;
(document.getElementById("submissionNotification") as HTMLSpanElement).innerText = "check_circle";
}


else if(role == "User")
{
 document.getElementById(element).innerHTML = row1;
}


document.getElementById("logout").addEventListener('click', function (e) {
    sessionStorage.clear();
    
    window.location.href = "/SJLogin/LoginRegiter.html";
    
    
});

// document.querySelector("#submissionNotification").addEventListener('click', e => {
//     if (role == "Admin" ) {
//         window.location.href = "/submissionRequestPage.html";
//     }

// });
// document.querySelector("#notifications").addEventListener('click', function (e) {
   
//     window.location.href = "/notifiication.html";}
  
// );
document.addEventListener("click", function(e){
    if((e.target as HTMLButtonElement).id=="notifications"){
    
         window.location.href="/notifiication.html";
  
 
  
}
  else if((e.target as HTMLButtonElement).id=="submissionNotification"){

if(role=="Admin"){

         window.location.href="/submissionRequestPage.html";
  }
}
 

});




function NotificationCount() {
    fetch(BASEURL+ "/api/Notification/Count/" +Token.getInstance().userID)
        .then(Response => Response.json())
        .then(data => {
            (document.getElementById("notifications") as HTMLElement).dataset.badge = data;
        })
        .catch(err => console.log(err));
}
NotificationCount();











window["componentHandler"].upgradeDom();
}
export function navigationBarsss(role:string , element:string) {
    var navigation = `   <header class="demo-drawer-header">
    <div class="demo-avatar-dropdown"><br>
        
        <div class="mdl-layout-spacer"></div>
        <br>
    </div>
</header>

   
    <nav class="demo-navigation mdl-navigation mdl-color--blue-grey-800" >
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

export function paging(metadata)
{   let total_pages=metadata.TotalPages;
    current_page=metadata.CurrentPage;
    let has_next=metadata.HasNext;
    let has_previous=metadata.HasPrevious;
    
    (document.getElementById("pagination") as HTMLDivElement).innerHTML="";
    if(has_previous)
        (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value="<<" >`;
    if(total_pages>1)
        for (let loop = 1 ; loop <= total_pages ; loop++)
            (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="${loop}" value="${loop}" >`;
    if(has_next)
        (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value=">>" >`;
}

export function PageNo(page_no,pageSize=page_size)
{
    let uri= "page="+page_no + "&page-size="+pageSize;
    return uri;
}

export function changePage(value){
    if(value==">>")
		current_page+=1;
	else if(value=="<<")
		current_page-=1;
	else
        current_page=+value;
    return current_page;
}

// export class Token    /// call static method that return an object 
// {
//     userID:number 
//     tokenKey :string
//     private constructor(){ 
//         this.tokenKey = JSON.parse(sessionStorage.getItem("user_info"))["token"];
//         this.userID = parseInt(JSON.parse(sessionStorage.getItem("user_info"))["id"]);
//      }
//      static  getInstance():Token
//     {
//         return new Token();
//     }
// }