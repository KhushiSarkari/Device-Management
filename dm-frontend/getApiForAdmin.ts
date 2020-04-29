import { BASEURL, navigationBarsss, PageNo, current_page, paging } from "./globals";
import { DeviceListForAdmin } from "./deviceListForAdmin";
import { Sort } from "./user-profile/SortingUser";
import { amIUser } from "./globals";
import { Requests } from "./RequestModel";
import { openForm } from "./utilities";
import { HitApi } from "./Device-Request/HitRequestApi";




(async function() {
	const id= JSON.parse(sessionStorage.getItem("user_info"))["id"];
	const token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
	const role = (await amIUser(token)) == true ? 0 : 1;
	class Assign_device {
		device_id: number = 0;
		return_date: string = "";
		user_id: number = 0;
		admin_id:number=0;
	}

	class GetApiForAdmin {
		token: string="";
		currentPage:number=current_page;

		constructor(token:string){
			this.token=token;
		
		}
		getApi(URL: string) {
			fetch(URL,{
				headers: new Headers({"Authorization": `Bearer ${token}`})
				
            })
				.then(response =>{
					let metadata=JSON.parse(response.headers.get('X-Pagination'));
					paging(metadata);
					return response.json()
				})
				
				.then(data => {
					console.log();
					(document.getElementById(
						"Request_data"
					) as HTMLTableElement).innerHTML = "";
					for (let element in data) {
						let res = new DeviceListForAdmin(data[element],token);
						res.getDeviceList(role);
					}
				})
				.catch(err => console.log(err));
		}
		getData(uri:string) {
			const URL = BASEURL + "/api/Device/page?"+PageNo(this.currentPage);
			console.log(URL);
			this.getApi(URL);
		}
		searchByName(status:string) {
			var serial_number = (document.getElementById(
				"search_serial_number"
			) as HTMLInputElement).value;
			var device_name = (document.getElementById(
				"fixed-header-drawer-exp"
			) as HTMLInputElement).value;
			const URL1 = BASEURL + "/api/Device/search?"+PageNo(this.currentPage)+"&device_name=" + device_name;
			if (serial_number) {
				const URL = URL1 + "&serial_number=" + serial_number;
				this.getApi(URL);
			} else if (status) {
				const URL = URL1 + "&status_name=" + status;
				this.getApi(URL);
			} else if (serial_number && status) {
				const URL =
					URL1 + "&serial_number=" + serial_number + "&status_name=" + status;
				this.getApi(URL);
			} else {
				this.getApi(URL1);
			}
			//(document.getElementById("fixed-header-drawer-exp") as HTMLInputElement).value="";
			//(document.getElementById("search_serial_number") as HTMLInputElement).value="";
		}
		sort(SortColumn: string, SortDirection: any,uri:string) {
			const URL =
				BASEURL +
				"/api/Device/sort"+uri+"SortColumn=" +
				SortColumn +
				"&SortDirection=" +
				SortDirection +
				"&" + PageNo(this.currentPage);
			this.getApi(URL);
		}

		deleteDevice(device_id: number) {
			fetch(BASEURL + "/api/Device/del/" + device_id, {
                method: "DELETE",
                headers: new Headers({"Authorization": `Bearer ${token}`})
			});
		}

		postNotification(data) {
			if (confirm("Notify?")) {
				fetch(BASEURL + "/api/Notification", {
					method: "POST",
					headers: [["Content-Type", "application/json"], ["Authorization", `Bearer ${token}`]],
					body: data,
				}).catch(Error => console.log(Error));
				alert("Notification sent");
			}
		}
		async assign_device(data: Assign_device) {
			let data1 = JSON.stringify(data);
			console.log(data1);
			let res= await fetch(BASEURL + "/api/device/assign", {
				method: "POST",
				headers:new Headers([["Content-Type","application/json"],["Authorization", `Bearer ${this.token}`]]),
				body: data1
			});
			if(res.status==200)
			{
				alert("Device assigned");
				console.log("assign");
				temp.closeForm1("popupForm2");
				window.location.reload();
			}
		}
		async getUserDetails()
		{
			const URL =BASEURL +"/api/Dropdown/userlist";
			const user = await new HitApi(this.token).HitGetApi(URL);
			console.log(user);
			let htmlString = '';
			for (let dataPair of user) {
				htmlString += '<option data-id="'+dataPair.id +'" value="' + dataPair.name +"(Emp-id:"+dataPair.id+")Dept-"+dataPair.department+ '">'+'</option>';
			}
			(document.getElementById("user") as HTMLSelectElement).innerHTML = htmlString;
		
			return null;
	
		}
		openForm1(popup) {
			document.getElementById(popup).style.display = "block";
		}
		closeForm1(popup) {
			document.getElementById(popup).style.display = "none";
		}
	
	}

	document.addEventListener("click", function(e) {
		if ((e.target as HTMLButtonElement).className == "edit-button") {
			const device_id: any = (e.target as HTMLButtonElement).getAttribute(
				"value"
			);
			console.log(device_id);
			console.log("edit button");

			window.location.href = "AddDevice.html?device_id=" + device_id;
		}
		if ((e.target as HTMLButtonElement).className == "delete-button") {
			
			if (confirm("Are you sure you want to delete this device?")) {
				const temp = new GetApiForAdmin(token);
				const device_id: any = (e.target as HTMLButtonElement).getAttribute(
					"value"
				);
				
				temp.deleteDevice(device_id);
				
	           window.location.reload();
				temp.getData("");
			} 
		}
		if((e.target as HTMLTableCellElement).className=="cards")
        {
			
			const device_id: any = (e.target as HTMLButtonElement).dataset.deviceid;
			window.location.href = "./devicedetail.html?device_id=" + device_id;
        }
		if ((e.target as HTMLButtonElement).className == "notify-button") {
			console.log("notify");
			let deviceId: number = parseInt((e.target as HTMLButtonElement).dataset.deviceid, 10);
			console.log(deviceId);
			temp.postNotification(JSON.stringify({ "notify": [{ deviceId }] }));
		}
		if ((e.target as HTMLButtonElement).className == "assign-button") {
			temp.openForm1("popupForm2");
			(document.getElementById(
				"device_id"
			) as HTMLInputElement).value = (e.target as HTMLButtonElement).dataset.id;
			document.getElementById(
				"device_id"
			).innerHTML = (e.target as HTMLButtonElement).dataset.id;
		}
		if ((e.target as HTMLButtonElement).className == "cancel-button") {
			temp.closeForm1("popupForm2");
		}
		if ((e.target as HTMLButtonElement).className == "assigndevice-btn") {
			e.preventDefault();
			console.log("test");
			let assign = new Assign_device();
			assign.device_id = +(document.getElementById(
				"device_id"
			) as HTMLInputElement).value;
			assign.return_date = (document.getElementById(
				"return_date"
			) as HTMLInputElement).value;
			let option =(document.getElementById("user") as HTMLSelectElement).options;
			let user = (document.getElementById("inputuser") as HTMLInputElement).value;
			
			for(let element in option)
			{
				if(user==option[element].value)
				{
					var optionElement = option[element];
				}
			}
			assign.user_id = +(optionElement.getAttribute("data-id"));
			console.log(assign.user_id);
			assign.admin_id = +id;
			temp.assign_device(assign);
			
			// window.location.reload();
		}
	});
	(document.querySelector("#device_id") as HTMLTableElement).addEventListener(
		"click",
		function(e) {
			window.location.href="./devicedetail.html";
		}
	);
	

	
	(document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 
		if((e.target as HTMLButtonElement).value==">>")
		{
			temp.currentPage+=1;
		}
		else if((e.target as HTMLButtonElement).value=="<<")
		{
			temp.currentPage-=1;
		}
		else
		{
			temp.currentPage=+((e.target as HTMLButtonElement).value);
		}
	       console.log((e.target as HTMLButtonElement).value);
		let uri = PageNo(temp.currentPage);
		console.log(uri);
		temp.getData(uri);   
    });
	(document.querySelector("#tablecol") as HTMLTableElement).addEventListener(
		"click",
		function(e) {
			const col = (e.target as HTMLElement).getAttribute("name");
			let id = e.target as HTMLTableHeaderCellElement;
			let sorts = new Sort(token);
			let direction = sorts.checkSortType(id);
			temp.sort(col, direction,"?");
		}
	);
	

	

console.log(status);
	(document.querySelector(
		"#fixed-header-drawer-exp"
	) as HTMLInputElement).addEventListener("change", function(e) {
		let status=(document.getElementById("status") as HTMLInputElement).value;
		console.log("acdvfvf"+status);
		temp.searchByName(status);
	});
	(document.querySelector(
		"#search_serial_number"
	) as HTMLInputElement).addEventListener("change", function(e) {
		temp.searchByName(status);
	});
	(document.querySelector("#status") as HTMLInputElement).addEventListener(
		"click",
		function(e) {
			if (
				(document.getElementById("status") as HTMLInputElement).value == "all"
			) {
				temp.getData("");
			} else {
				const status=(document.getElementById("status") as HTMLInputElement).value;
				temp.searchByName(status);
			}
		}
	);

	(document.querySelector(".devices") as HTMLDivElement).addEventListener(
		"click",
		function(e) {
			temp.getData("");
		}
	);

	const temp = new GetApiForAdmin(token);
	temp.getUserDetails();

	if(role ==0)
	{
		var roles ="User";
	}
	else 
	roles = "Admin";
	navigationBarsss(roles,"navigations");
	
	
	const urlParams = new URLSearchParams(window.location.search);
	const myParam = urlParams.get("status");
	if (myParam != null) {
		temp.searchByName(myParam);
		(document.getElementById("status")as HTMLSelectElement).value=myParam;
		
	}
	else
	{
		temp.getData("");
	}
	
})();
