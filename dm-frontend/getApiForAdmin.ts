import { BASEURL, navigationBarsss, PageNo, current_page, paging, changePage,headersRows,Token } from "./globals";
import { DeviceListForAdmin } from "./deviceListForAdmin";
import { Sort } from "./user-profile/SortingUser";
import { amIUser } from "./globals";
import { Requests } from "./RequestModel";
import { openForm } from "./utilities";
import { HitApi } from "./Device-Request/HitRequestApi";




(async function() {
	const _ = Token.getInstance();
    const id = _.userID;
    const token = _.tokenKey;
	const role = (await amIUser(token)) == true ? 0 : 1;
	class Assign_device {
		deviceId: number = 0;
		returnDate: string = "";
		userId: number = 0;
		adminId:number=0;
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
					console.log(data);
					(document.getElementById(
						"Request_data"
					) as HTMLTableElement).innerHTML = "";
					
					for (let element in data) {
						let res = new DeviceListForAdmin(data[element],token);
						console.log(res);
						res.getDeviceList(role);
						
					}
					window["componentHandler"].upgradeDom();
				})
				.catch(err => console.log(err));
		}
		getData(uri:string,SortColumn: string="", SortDirection: any="") {
			var device_name = (document.getElementById(
				"fixed-header-drawer-exp"
			) as HTMLInputElement).value;
			var serial_number = (document.getElementById(
				"search_serial_number"
			) as HTMLInputElement).value;
			var status_name = (document.getElementById("status") as HTMLInputElement).value;
			
			const URL = BASEURL + "/api/Device/page?"+PageNo(this.currentPage) +"&status_name=" + status_name
			+"&SortColumn=" +
			SortColumn +
			"&SortDirection=" +
			SortDirection ;
			if (serial_number) {
				const URI = URL+ "&serial_number=" + serial_number;
				this.getApi(URI);
			} 
			else if (device_name)
			{
				const URI = URL+ "&device_name=" + device_name;
				this.getApi(URI);
			}
				else{
					this.getApi(URL);
				}
		
	}
		// searchByName(status:string) {
		// 	var serial_number = (document.getElementById(
		// 		"search_serial_number"
		// 	) as HTMLInputElement).value;
		// 	var device_name = (document.getElementById(
		// 		"fixed-header-drawer-exp"
		// 	) as HTMLInputElement).value;
		// 	const URL1 = BASEURL + "/api/Device/search?"+PageNo(this.currentPage)+"&device_name=" + device_name;
		// 	if (serial_number) {
		// 		const URL = URL1 + "&serial_number=" + serial_number;
		// 		this.getApi(URL);
		// 	} else if (status) {
		// 		const URL = URL1 + "&status_name=" + status;
		// 		this.getApi(URL);
		// 	} else if (serial_number && status) {
		// 		const URL =
		// 			URL1 + "&serial_number=" + serial_number + "&status_name=" + status;
		// 		this.getApi(URL);
		// 	} else {
		// 		this.getApi(URL1);
		// 	}
		// 	//(document.getElementById("fixed-header-drawer-exp") as HTMLInputElement).value="";
		// 	//(document.getElementById("search_serial_number") as HTMLInputElement).value="";
		// }
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

		async deleteDevice(device_id: number) {
			let res = await fetch(BASEURL + "/api/Device/del/" + device_id, {
                method: "DELETE",
                headers: new Headers({"Authorization": `Bearer ${token}`})
			});
			if(res.status==200)
			{
				window["tata"].text('Device ','Deleted!',{duration:3000});
				temp.getData("");
			}
			else 
			{
				window["tata"].text('Device not','Deleted!',{duration:3000});
			}
		}

		postNotification(data) {
			if (confirm("Notify?")) {
				fetch(BASEURL + "/api/Notification", {
					method: "POST",
					headers: [["Content-Type", "application/json"], ["Authorization", `Bearer ${token}`]],
					body: data,
				}).catch(Error => console.log(Error));
				window["tata"].text('Notification','Sent!',{duration:3000});
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
				window["tata"].text('Device', 'assigned',{duration:20000});
				temp.closeForm1('.login-popup');
				temp.getData("");
			}
			else
			{
				window["tata"].text('Device not', 'assigned',{duration:20000});
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
            document.querySelector(popup).classList.add("active");
        }
        closeForm1(popup) {
            document.querySelector(popup).querySelectorAll('input,select').forEach((element) => {
                element.value = '';
            });
            document.querySelector(popup).classList.remove("active");
        }
	
	}

	document.addEventListener("click", async function(e) {
		if((e.target as HTMLButtonElement).id=="add-button"){
			window.location.href="./AddDevice.html";
		}
		if ((e.target as HTMLButtonElement).id.startsWith("edit-")) {
			
			const device_id: any = (e.target as HTMLButtonElement).getAttribute(
				"value"
			);
			window["tata"].text('Edit This ','Device',{duration:3000});
			window.location.href = "AddDevice.html?device_id=" + device_id;
		}
		if ((e.target as HTMLSpanElement).id.startsWith("delete-")) {
			if (confirm("Are you sure you want to delete this device?")) {
				
				const temp = new GetApiForAdmin(token);
				const device_id: any = (e.target as HTMLButtonElement).getAttribute("value");
				await temp.deleteDevice(device_id);
				
			} 
		}
		if((e.target as HTMLTableCellElement).className=="cards tooltip")
        {
			
			const device_id: any = (e.target as HTMLButtonElement).dataset.deviceid;
			window.location.href = "./devicedetail.html?device_id=" + device_id;
        }
		if ((e.target as HTMLButtonElement).id.startsWith("notify-")) {
			console.log("notify");
			let deviceId: number = parseInt((e.target as HTMLButtonElement).dataset.deviceid, 10);
			temp.postNotification(JSON.stringify({ "notify": [{ deviceId }] }));
		}
		if ((e.target as HTMLButtonElement).id.startsWith("assign")) {
			temp.openForm1('.login-popup');
			(document.getElementById(
				"device_id"
			) as HTMLInputElement).value = (e.target as HTMLButtonElement).dataset.id;
			document.getElementById(
				"device_id"
			).innerHTML = (e.target as HTMLButtonElement).dataset.id;
		}
		if ((e.target as HTMLButtonElement).className == "cancel-button") {
			temp.closeForm1('.login-popup');
		}
		if ((e.target as HTMLButtonElement).className == "assigndevice-btn") {
			e.preventDefault();
			console.log("test");
			let assign = new Assign_device();
			assign.deviceId = +(document.getElementById(
				"device_id"
			) as HTMLInputElement).value;
			assign.returnDate = (document.getElementById(
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
			assign.userId = +(optionElement.getAttribute("data-id"));
			assign.adminId = +id;
			temp.assign_device(assign);
		}
	});
	(document.querySelector("#device_id") as HTMLTableElement).addEventListener(
		"click",
		function(e) {
			window.location.href="./devicedetail.html";
		}
	);
	

	
	(document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 	temp.currentPage=changePage((e.target as HTMLButtonElement).value);
		temp.getData(PageNo(temp.currentPage));   
    });
	(document.querySelector("#tablecol") as HTMLTableElement).addEventListener(
		"click",
		function(e) {
			const col = (e.target as HTMLElement).getAttribute("name");
			let id = e.target as HTMLTableHeaderCellElement;
			let sorts = new Sort(token);
			let direction = sorts.checkSortType(id);
			//temp.sort(col, direction,"?");
			temp.getData("",col,direction);
		}
	);
	(document.querySelector(
		"#fixed-header-drawer-exp"
	) as HTMLInputElement).addEventListener("change", function(e) {
		let status=(document.getElementById("status") as HTMLInputElement).value;
		
		temp.getData("");
	});
	(document.querySelector(
		"#search_serial_number"
	) as HTMLInputElement).addEventListener("change", function(e) {
		temp.getData("");
	});
	(document.querySelector("#status") as HTMLInputElement).addEventListener(
		"click",
		function(e) {
				temp.getData("");
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
	headersRows(roles,"row1");
	navigationBarsss(roles,"navigations");
	
	
	const urlParams = new URLSearchParams(window.location.search);
	const myParam = urlParams.get("status");
	if (myParam != null) {
		//temp.searchByName(myParam);
		temp.getData(myParam);
		(document.getElementById("status")as HTMLSelectElement).value=myParam;
		
	}
	else
	{
		temp.getData("");
	}
	
})();
