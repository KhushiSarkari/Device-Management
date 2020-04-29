import { CreateUserApi } from "./createApi";
import { GetUserApi } from "./user-profile/getUserApi";
import * as util from "./utilities";
import { UpdateUserApi } from "./updateApi";
import { populateFormFromObject, createObjectFromForm } from "./user-profile/databinding";
import { UserModel } from "./UserModel";
import { remove, validateForm } from "./validation";
import { Sort } from "./user-profile/SortingUser";
import { BASEURL,amIUser,navigationBarsss, PageNo, current_page, paging  } from './globals';
import { UserData }  from "./dropdown";
import {MyDevices } from "./userHistory";
import {dropDownListen } from "./user-profile/dropDownListener";
import { formatPhone } from "./utilities";
export let currentPage:number=current_page;

(async function(){
	const token:string=JSON.parse(sessionStorage.getItem("user_info"))["token"];
	const role = (await amIUser(token)) == true ? "User" : "Admin";
	let form_mode: "create" | "edit";


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
		}
		function setData() 
		{
			const temp = new GetUserApi(token);
			temp.getRequest().then(function(){
				console.log(temp.array);
				populateTable(temp.array );
			});
		}
		
		setData();

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
		e.preventDefault();
		if (form_mode=="create")
		{
			var userData=createObjectFromForm(this);
			if(validateForm(form_mode)==true){
				new CreateUserApi(token).createUserData(userData).then(function(){setData();});
			}
			else 
			{
				return false;
			}
		}
		else if(form_mode=="edit")
		{
			var userData1=createObjectFromForm(this);
			if(validateForm(form_mode)==true){
				new UpdateUserApi(token).updateUserData(userData1).then(function(){
					setData();
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
		else if (((ea.target) as HTMLInputElement).className == "userCheckStatus")
		{   const target = ea.target as HTMLInputElement;
			const modal = document.querySelector((ea.target as HTMLElement).dataset.target) as HTMLDivElement;
			const userId:number = parseInt(target.id);
			modal.setAttribute('data-id', (ea.target as HTMLElement).id);
			util.openModal(modal);
			modalFunctions[modal.dataset["operation"]].call(modal, function(confirm:boolean){
				if(confirm == true){
					let statusToSet = target.checked ? "active" : "inactive";
					new GetUserApi(token).userInactive(userId , statusToSet).then(_ => {
						setData();
					});
				}
				else{
					// If cancel button is clicked, Reset the checkbox to original state
					target.checked = !target.checked;
				}
				util.closeModal(modal);
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
					new GetUserApi(token).deleteData(userId).then(function () { setData(); });
				}
				util.closeModal(modal);
			});
		}
		else if(((ea.target) as HTMLInputElement).id == "closeFormButton")
		{
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
			ea.preventDefault();
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
			new GetUserApi(token).getUserById(userId).then(res => {
				userObject = (res as unknown) as UserModel;
				const form = document.forms["myForm"] as HTMLFormElement;
				populateFormFromObject(userObject, form,token);
				form_mode = "edit";
			});
		}
	});


	
	(document.querySelector('#fixed-header-drawer-exp')as HTMLInputElement).addEventListener('change', function (e) {
		console.log("test");
		const temp = new GetUserApi(token);
		temp.searchUser().then(function(data){
			populateTable(data);
		});
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
		   setData();
		
    });

	navigationBarsss(role,"navigation");
    util.addressCheck();
	dropDownListen(form,token);


})();