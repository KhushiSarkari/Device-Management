import { BASEURL, navigationBarsss, amIUser, PageNo, current_page,paging } from "./globals";
import { Sort } from "./user-profile/SortingUser";
import {Notifications} from "./notification";
(async function() {


    let token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
	let role = (await amIUser(token)) == true ? "User" :"Admin";
    let user_id = JSON.parse(sessionStorage.getItem("user_info"))["id"];
class Notify
{
    deviceId:number =0;
    userId:number= 0;
    token:string ="";
    currentPage:number=current_page;
    constructor(token:string)
    {
        this.token =token;
    }
    getNotification(URL:any)
    {
        fetch(
            BASEURL + "/api/notification?"+PageNo(this.currentPage)+URL 
        ,{
            headers: new Headers({"Authorization": `Bearer ${token}`})
        })
        .then(response =>{
            let metadata=JSON.parse(response.headers.get('X-Pagination'));
            paging(metadata);
            return response.json()
        })
            .then(data => {
                console.log(data);
                (document.getElementById("notification_data") as HTMLTableElement).innerHTML = "";
            
                for (let element in data) {
                    let res = new Notifications(data[element],token);
                    res.getNotificationTable();
                }
            })
            .catch(err => console.log(err));

    }
    notification(id:any)
    {
        let URL = "&id=" + id;
        this.getNotification(URL);
    }
    notificationSearch(id:number,search:string)
    {
        let URL = "&id=" + id+"&search="+search;
        this.getNotification(URL);
    }
    notificationSort(id:number,sort:string,direction:string)
    {
        let URL = "&id=" + id+"&sort="+sort +"&direction=" +direction;
        this.getNotification(URL);
    }
    acceptNotification(data:Notify)
    {
        fetch(BASEURL + "/api/ReturnRequest", {
            method: "POST",
            headers: new Headers([["Content-Type","application/json"],["Authorization", `Bearer ${this.token}`]]),
            body: JSON.stringify(data),
        }).catch(Error => console.log(Error));   
    }   
    rejectNotification(notificationId:number)
    {
        
        fetch(BASEURL + "/api/Notification/reject/"+notificationId, {
            headers: new Headers([["Content-Type","application/json"],["Authorization", `Bearer ${this.token}`]]),
        }).catch(Error => console.log(Error));   
    }   
}
document.getElementById("fixed-header-drawer-exp").addEventListener("keyup",function(e)
{
    let search = (document.getElementById("fixed-header-drawer-exp")as HTMLInputElement).value;
    console.log(search);
    notify.notificationSearch(user_id,search);    
});
document.addEventListener("click", function (e) {

    if ((e.target as HTMLTableElement).tagName == 'TH') {
       
        
        let id = (e.target as HTMLTableHeaderCellElement);
        let column = (e.target as HTMLTableHeaderCellElement).dataset.id;
	    let sorts = new Sort(token);
        let direction =sorts.checkSortType(id);
        console.log(direction);
        notify.notificationSort(user_id,column,direction);
           
        }
        
            if ((e.target as HTMLButtonElement).className == "accept-button") {
               if(confirm("Are you sure you want to submit the device?"))
               {
                   
                    notify.deviceId = +(e.target as HTMLButtonElement).dataset.value;
                    notify.userId = +(e.target as HTMLButtonElement).dataset.userid;
                    notify.acceptNotification(notify);
                    notify.notification(user_id);
                 console.log("notification accepted");
               }
            }
        if ((e.target as HTMLButtonElement).className == "reject-button") {
               if(confirm("Are you sure you don't want to submit the device?"))
               {
                
                let notificationId = parseInt((e.target as HTMLButtonElement).dataset.notificationid,10);
                notify.rejectNotification(notificationId);
                notify.notification(user_id);
                
               }
               
            }
        
});
(document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 
		if((e.target as HTMLButtonElement).value==">>")
		{
			notify.currentPage+=1;
		}
		else if((e.target as HTMLButtonElement).value=="<<")
		{
			notify.currentPage-=1;
		}
		else
		{
			notify.currentPage=+((e.target as HTMLButtonElement).value);
		}
	       console.log((e.target as HTMLButtonElement).value);
           notify.notification(user_id); 
    });

let notify = new Notify(token);
notify.notification(user_id);

navigationBarsss(role,"navigation");
return null ;
})();