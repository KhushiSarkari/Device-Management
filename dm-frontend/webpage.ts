import { CreateUserApi } from "./createApi";
import { GetUserApi } from "./user-profile/getUserApi";
import * as util from "./utilities";
import { UpdateUserApi } from "./updateApi";
import { populateFormFromObject, createObjectFromForm } from "./user-profile/databinding";
import { UserModel } from "./UserModel";
import { remove, validateForm } from "./validation";
import { Sort } from "./user-profile/SortingUser";
import { BASEURL,amIUser,navigationBarsss, current_page,changePage,headersRows,Token  } from './globals';
import { UserData }  from "./dropdown";
import {MyDevices } from "./userHistory";
import {dropDownListen } from "./user-profile/dropDownListener";
import { formatPhone } from "./utilities";
import * as signalR from "@aspnet/signalr";
(async function(){
	var photo;
	const _ = Token.getInstance();
    const token = _.tokenKey;
	const role = (await amIUser(token)) == true ? "User" : "Admin";
	let form_mode: "create" | "edit";
	let currentPage:number=current_page;

	const table = document.getElementById("Request_data_body") as HTMLTableElement;
	console.log(table);
		function populateTable(array: UserModel[])
		{
			let htmlString: string = '';
			for(let element of array)
			{
				htmlString += element.GenerateTableRow();
			};

			table.innerHTML = htmlString;
			window["componentHandler"].upgradeDom();
		}
		function setData() 
		{
			const temp = new GetUserApi(token,currentPage);
			temp.getRequest().then(function(){
				console.log(temp.array);
				populateTable(temp.array );
			});
		}
		

		document.addEventListener('click' , e =>
		{
			if((e.target as HTMLButtonElement).id == "popup")
			{
				
					console.log("Button clicked");
					(document.getElementById("email") as HTMLInputElement).disabled = false;
					
					form_mode="create";
					changeheadingText();
                    util.openForm(form_mode);
					var user = new UserData(token);
				}
			});

	const form = document.querySelector('.form-popup') as HTMLFormElement;
	form.addEventListener('submit', function (e) {
		console.log("inside function")
		
		if (form_mode=="create")
		{
		
			var userData=createObjectFromForm(form);
			e.preventDefault();
			if(validateForm(form_mode)==true){
				
				new CreateUserApi(token).createUserData(userData).then(function(response:Response ){
					if(response.status!=200)
					{
						throw new Error(response.statusText);
					}
				}).then(function(){
					setData();
					window["tata"].text('New User ','Added!',{duration:3000});}).catch(Error => {console.log(Error),alert(Error.message);});
			}
			else 
			{
				return false;
			}
		}
		else if(form_mode=="edit")
		{
			e.preventDefault();
			var userData1=createObjectFromForm(form);
			if(validateForm(form_mode)==true){
				new UpdateUserApi(token).updateUserData(userData1).then(function(){
					setData();
					window["tata"].text('User Details ','Updated!',{duration:3000});
				});
				
			}
			else
			{
				return false;
			}
		}
		util.closeForm();
		return false;

	});

	function changeheadingText()
	{
		document.getElementById('headingText').innerHTML= form_mode == "create" ? 'Register User' : "Update User details";
	}
	const modalFunctions = {
		onDeleteModal : function (callback) {
			let userId:number = parseInt(((this) as HTMLInputElement).dataset.id);
			
			let url : string= BASEURL + "/api/Device/current_device/"+userId+"?search=" + "" + "";
			
			(document.getElementById("dis") as HTMLButtonElement).addEventListener('click', ucs);
			this.querySelectorAll("[data-dismiss=\"modal\"]").forEach(element => element.addEventListener('click', cancel));
			
			new MyDevices(token).getApiCall(url)
			.then(res=>{
				
				console.log(res);
				if(res.length>0)
				{   let i=0;
					(document.getElementById("insideDeleteModel") as HTMLInputElement).innerHTML="";
					(document.getElementById("errorMsg") as HTMLInputElement).innerHTML= "This User Already Has Assigned Devices So Can't Be Deleted";
					(document.getElementById("dis")as HTMLInputElement).disabled = true;
					for(i=0;i<res.length;i++)
					(document.getElementById("insideDeleteModel") as HTMLInputElement).innerHTML="This User Has "+res.length+" Devices "+ "<br>"+(i+1)+". "+res[i].type+" "+res[i].brand+" "+res[i].model+"<br> ";
					}
				else
				{
					
					(document.getElementById("insideDeleteModel") as HTMLInputElement).innerHTML="";
					(document.getElementById("dis")as HTMLInputElement).disabled = false;
					(document.getElementById("errorMsg") as HTMLInputElement).innerHTML= "This User Will Be Permanently Deleted <br> Do You Want To Proceed ?";
					(document.getElementById("insideModel") as HTMLInputElement).innerHTML="";
				}
			});
			function ucs(e){
				callback(true);
				this.removeEventListener('click', ucs);
			}
			function cancel(e){
				callback(false);
				this.removeEventListener('click', cancel);
			}
		},
		onStatusModal : function (callback) {
			let userId:number = parseInt((this as HTMLDivElement).dataset.id);
			let url : string= BASEURL + "/api/Device/current_device/"+userId+"?search=" + "" + "";
			
			(document.getElementById("ucs")as HTMLInputElement).addEventListener('click', ucs);
			this.querySelectorAll("[data-dismiss=\"modal\"]").forEach(element => element.addEventListener('click', cancel));
			
			new MyDevices(token).getApiCall(url)
			.then(res=>{
				if(res.length>0)
				{
					let i=0;
					(document.getElementById("insideModel") as HTMLInputElement).innerHTML="This User Has "+res.length+" Devices And Cannot Be Inactivated"+ "<br>";
					(document.getElementById("ucs")as HTMLInputElement).disabled = true;
					for(i=0;i<res.length;i++)
						(document.getElementById("insideModel") as HTMLInputElement).innerHTML+=(i+1)+". "+res[i].type+" "+res[i].brand+" "+res[i].model+"<br> ";
			
				}
				else{
					(document.getElementById("ucs")as HTMLInputElement).disabled = false;
					(document.getElementById("insideModel") as HTMLInputElement).innerHTML="";
				}
            });
			function ucs(e){
				callback(true);
				this.removeEventListener('click', ucs);
			}
			function cancel(e){
				callback(false);
				this.removeEventListener('click', cancel);
			}
		}
	};

	document.addEventListener("click", function (ea) {
		if((ea.target as HTMLTableHeaderCellElement).tagName == 'TH')
		{
			const returned = new Sort(token).sortBy(ea.target as HTMLTableHeaderCellElement);
			returned.then(data => {
				console.log(data);
				populateTable(data)
			});
		}
		else if (((ea.target) as HTMLInputElement).classList.contains("userCheckStatus"))
		{   const toggle = (ea.target as HTMLDivElement).parentElement;
			const checkbox = toggle["MaterialSwitch"]["inputElement_"] as HTMLInputElement;
			const modal = document.querySelector((ea.target as HTMLElement).dataset.target) as HTMLDivElement;
			const userId:number = parseInt(checkbox.id.slice(6));
			
			modal.setAttribute('data-id', userId.toString());
			util.openModal(modal);
			modalFunctions[modal.dataset["operation"]].call(modal, function(confirm:boolean){
				if(confirm == true){
					let statusToSet = checkbox.checked ? "active" : "inactive";
					new GetUserApi(token,currentPage).userInactive(userId , statusToSet).then(_ => {
						setData();
					});
				}
				else{
					// If cancel button is clicked, Reset the checkbox to original state
					checkbox.checked = !checkbox.checked;
					toggle["MaterialSwitch"]["checkToggleState"]();
				}
				util.closeModal(modal);
				setData();
			});
		}
		else if (((ea.target) as HTMLInputElement).className.includes("userDeleteData"))
		{   const target = ea.target as HTMLInputElement;
			const modal = document.querySelector((ea.target as HTMLElement).dataset.target) as HTMLDivElement;
			const userId:number = parseInt(target.id);
			modal.setAttribute('data-id', (ea.target as HTMLElement).id);
			util.openModal(modal);
			
			modalFunctions[modal.dataset["operation"]].call(modal, function(confirm:boolean){
				if(confirm == true){
					new GetUserApi(token,currentPage).deleteData(userId).then(function () { setData(); });
					window["tata"].text('User ','Deleted!',{duration:3000});
				}
				util.closeModal(modal);
				setData();
			});
		}
		else if(((ea.target) as HTMLInputElement).id == "closeFormButton")
		{
			ea.preventDefault();
			console.log("calling remove");
			remove();
		}
		// Handler for cancel button in modals
		else if ((ea.target as HTMLElement).getAttribute("data-dismiss") == "modal"){
			let modal = ea.target as HTMLElement;
			while(true){
				if ((modal.getAttribute("role") as string) == "dialog")
					break;
				modal = modal.parentElement;
			}
			util.closeModal(modal);
			modal.removeAttribute("data-id");
		}
		if((ea.target as HTMLButtonElement).id == "closeFormButton")
		{	
			
			util.closeForm();	
		}
		
	});

	document.addEventListener("click", function(e) {
		if ((e.target as HTMLButtonElement).className.includes("userEditData")) {
			
			(document.getElementById("email") as HTMLInputElement).disabled = true;
			form_mode="edit";
			changeheadingText();
			util.openForm(form_mode);
				
			const userId: number = parseInt((e.target as HTMLButtonElement).id) ;
			var userObject: UserModel;
			new GetUserApi(token,currentPage).getUserById(userId).then(res => {
				userObject = (res as unknown) as UserModel;
				const form = document.forms["myForm"] as HTMLFormElement;
				populateFormFromObject(userObject, form,token);
				form_mode = "edit";
			});
			//window["tata"].text('User Details Updated!',{duration:30000});
		}
	});


	
	(document.querySelector('#fixed-header-drawer-exp')as HTMLInputElement).addEventListener('keyup', function (e) {
		console.log("test");
		const temp = new GetUserApi(token,currentPage);
		temp.searchUser().then(function(data){
			populateTable(data);
		});
	});

	(document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 
		currentPage=changePage((e.target as HTMLButtonElement).value);
		setData();
		
    });
	function openForm1(popup) {
		document.querySelector(popup).classList.add("active");
	}
	function closeForm1(popup) {
		document.querySelector(popup).querySelectorAll('input,select').forEach((element) => {
			element.value = '';
		});
		document.querySelector(popup).classList.remove("active");
	}
	interface HTMLInputEvent extends Event {
		target: HTMLInputElement & EventTarget;
	}
	document.getElementById("upload").addEventListener("change",async function (e:HTMLInputEvent) {
		console.log("frf");
			console.log(e.target.files);
	    photo =e.target.files[0];
	     
	});	
	document.addEventListener("click", async function(e) {
		
		if ((e.target as HTMLButtonElement).id.startsWith("multipleUser")) {
			openForm1('.login-popup');
					}
		if ((e.target as HTMLButtonElement).className == "cancel-button") {
			closeForm1('.login-popup');
		}
		if((e.target as HTMLButtonElement).className == "upload-button") {
			e.preventDefault();
			progress();
			var progress_modal = document.getElementById("progress-bar");
			console.log(progress_modal);
  			if (progress_modal.style.display === "none") {
    		progress_modal.style.display = "block";
 			 } else {
  				  progress_modal.style.display = "none";
  			}
			let formData = new FormData();
	   
			formData.append("photo", photo);
			
			 await fetch(BASEURL + '/api/BulkRegister/UploadFiles', {method: "POST", body: formData})
			 .then(response =>response.json()).then(data=>{data.response;
				let totalRecords = data.totalRecords;
				let failedData = data.usersAlreadyExists.length;
				let successData = totalRecords - failedData;
				if(failedData===0)
				{
					window["tata"].text('Registration successfull!',{duration:6000});
					
				}
				else 
				{		
					var users="";
					for(let i=0;i<failedData;i++)
					users += data.usersAlreadyExists[i]+" <br>";
					console.log(users);
					(document.querySelector("#Already-users")as HTMLDivElement).innerHTML = "Successfully Registered Users:"+ successData+" <br>"+
					"Already Exists Users: "+failedData +" <br>"+users;
					
										
				}
			});
			//closeForm1('.login-popup');
		}
	 });
	 function progress()
	 {
	 
	 const connection = new signalR.HubConnectionBuilder().withUrl(BASEURL+"/messages", {
		skipNegotiation: true,
	   transport: signalR.HttpTransportType.WebSockets
	   })
	  .configureLogging(signalR.LogLevel.Information)
	  .build();
    
	console.log(connection);
	connection.start().then((e) => {
				console.log("connection started");
			  }).catch(err => console.log(err));
		connection.on('Process',function(i){
			console.log(i);
			
			(document.querySelector("#file")as HTMLProgressElement).value = i;
			(document.querySelector("#percentage")as HTMLLabelElement).innerHTML = i+" %";
		})

	
	} 
	
	
	
	navigationBarsss(role,"navigation");
	headersRows(role,"row1");
	setData();
    util.addressCheck();
	dropDownListen(form,token);


})();