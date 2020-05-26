import { HitApi } from "./Device-Request/HitRequestApi";

export const BASEURL = "http://localhost:5000";
export var page_size = 4;
export var current_page = 1;

export class Token    /// call static method that return an object 
{
    userID: number
    tokenKey: string
    private constructor() {
        this.tokenKey = JSON.parse(localStorage.getItem("user_info"))["token"];
        this.userID = parseInt(JSON.parse(localStorage.getItem("user_info"))["id"]);
    }
    static getInstance(): Token {
        return new Token();
    }
}

export function amIAdmin(token: string) {
    return new HitApi(token).HitGetApi(BASEURL + "/api/is_admin")
        .then(res => res.result as boolean);
}
export function amIUser(token: string) {
    return new HitApi(token).HitGetApi(BASEURL + "/api/is_user")
        .then(res => res.result as boolean);


}

export function headersRows(role: string, element: string) {
    var row1 = `
           <div class="mdl-js" >
                    <nav class="mdl-navigation">
                     <div class="material-icons mdl-badge mdl-badge--overlap" id="notifications" data-badge="" style="cursor:pointer">notifications</div>
                     <span class="mdl-color-text--white-grey-400 material-icons" id="submissionNotification"></span>
       
                     <button id="profile" class="mdl-button mdl-js-button mdl-button--icon">
                     <i class="material-icons">person_pin</i>
                 </button>
                 <ul class="mdl-menu mdl-js-menu mdl-menu--bottom-right" for="profile">
                     <a href='/user-profile/index.html'>
                         <li class="mdl-menu__item" id="#userProfile" >Profile</li></a>
                     
                     <li class="mdl-menu__item" id="logout">Logout</li>
                 </ul>
                 </nav>
                 </div>
                 `;
    document.getElementById(element).innerHTML = row1;

    if (role == "Admin") {
        (document.getElementById("submissionNotification") as HTMLSpanElement).innerText = "markunread";
   
    }


    
//     if (role == "Admin" ) {
        
//         let r2= ` 
//         <span class="mdl-color-text--white-grey-400 material-icons" id="submissionNotification"></span>
        
//     `;

//     this.row1+=` </nav>
//     </div>
   
// `;


// document.getElementById(element).innerHTML = row1+r2;
// (document.getElementById("submissionNotification") as HTMLSpanElement).innerText = "markunread";
// }


// else if(role == "User")
// {
//  document.getElementById(element).innerHTML = row1;
// }


    document.addEventListener("click", function (e) {
        if ((e.target as HTMLButtonElement).id == "notifications") {
            window.location.href = "/notifiication.html";

        }
        else if ((e.target as HTMLButtonElement).id == "submissionNotification") {

            if (role == "Admin") {
                window.location.href = "/submissionRequestPage.html";
            }
        }
        else if ((e.target as HTMLButtonElement).id == "logout") {
            localStorage.clear();
            window.location.href = "/SJLogin/LoginRegister.html";
        }

    });

    function NotificationCount() {
        fetch(BASEURL + "/api/Notification/Count/" + Token.getInstance().userID)
            .then(Response => Response.json())
            .then(data => {
                (document.getElementById("notifications") as HTMLElement).dataset.badge = data;
            })
            .catch(err => console.log(err));
    }
    NotificationCount();
    window["componentHandler"].upgradeDom();
}
export function navigationBarsss(role: string, element: string) {
    const parser = new DOMParser();
    var navigation = `   
    <nav class="demo-navigation mdl-navigation " >
    <a class="mdl-navigation__link" href="/dashboard.html">
   <i class=" material-icons mdl-color-text--red-A100"
       role="presentation">dashboard</i> <span>Dashboard</span>
    </a>
     <a class="mdl-navigation__link " href="/deviceListForadmin.html">
         <i class=" material-icons mdl-color-text--red-A100" role="presentation">devices</i> <span>Devices</span>
    </a>
    
    <a class="mdl-navigation__link" href="/Device-Request/device_request.html">
    <i class=" material-icons mdl-color-text--red-A100"
            role="presentation">import_export</i><span>Request Device</span>
    </a>
    <a class="mdl-navigation__link" href="/userRequestHistory.html" id="mydevices">
        <i class=" material-icons mdl-color-text--red-A100" >laptop_chromebook
        </i><span>My Devices</span>
    </a>`;
     
    
        let nav = ` 
        <a class="mdl-navigation__link" href="/specification.html">
        <i class=" material-icons mdl-color-text--red-A100 ">
        build
        </i><span>Specifications</span>
    </a>
    
   
    <a class="mdl-navigation__link" href="/web.html">
        <i class=" material-icons mdl-color-text--red-A100" role="presentation">group</i><span>Users</span>
    </a>
    <a class="mdl-navigation__link" href="/adminRequestPage.html">
        <i class=" material-icons mdl-color-text--red-A100"
            >menu_book</i><span>All Requests</span>
    </a>
    <a class="mdl-navigation__link active " href="/request-history/request-history.html">
        <i class=" material-icons mdl-color-text--red-A100" role="presentation">history</i><span>Request History</span>
    </a>
    <a class="mdl-navigation__link" href="/faultyDevice/faultdevice.html" >
    <i class=" material-icons mdl-color-text--red-A100" role="presentation">report_problem</i>
    <span>Complaints</span>
     </a>
   
    <a class="mdl-navigation__link" href="/device_role/role.html">
        <i class=" material-icons mdl-color-text--red-A100"
            >assignment_ind</i><span>Roles & Permissions</span>
    </a></nav>
    `;
    let HTMLString : string;
    if (role == "Admin"){
        HTMLString = navigation + nav;
    }
         
    
    else if (role == "User") {
        HTMLString = navigation;
    }
    let HTML = parser.parseFromString(HTMLString,'text/html');
    let title = document.querySelector('span.mdl-layout-title').textContent;
    HTML.querySelectorAll("a.mdl-navigation__link span").forEach(function(span){
        if(span.textContent === title){
            span.parentElement.classList.add("mdl-navigation__link--current");
        }
    });
    document.getElementById(element).appendChild(HTML.body.firstChild);
    }

export function paging(metadata) {
    let total_pages = metadata.TotalPages;
    current_page = metadata.CurrentPage;
    let has_next = metadata.HasNext;
    let has_previous = metadata.HasPrevious;

    (document.getElementById("pagination") as HTMLDivElement).innerHTML = "";
    if (has_previous)
        (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value="<<" >`;
    if (total_pages > 1)
        for (let loop = 1; loop <= total_pages; loop++)
        {
             (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page ${loop == current_page ? 'activePage' : ''}" id="${loop}" value="${loop}" >`;
         }
    if (has_next)
        (document.getElementById("pagination") as HTMLDivElement).innerHTML += `<input type="submit" class="page" id="" value=">>" >`;
   }

export function PageNo(page_no, pageSize = page_size) {
    let uri = "page=" + page_no + "&page-size=" + pageSize;
    return uri;
}

export function changePage(value) {
    if (value == ">>")
        current_page += 1;
    else if (value == "<<")
        current_page -= 1;
    else
        current_page = +value;
    return current_page;
}
