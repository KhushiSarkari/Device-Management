import { role, permission } from "./role";
import { RolePermission } from "./rolePermCheckbox";
import { navigationBarsss, amIUser } from "../globals";

(async function(){
	const token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
	let Role = await amIUser(token) == true ? "User" : "Admin";
	// action listeners
	
	// Tab 2
	const ROLE = new role(token);
	document.getElementById("two").addEventListener("click", function() {
		ROLE.getroles();
	});
	document.querySelector("#addrole").addEventListener("click", function(e) {
		ROLE.addRole();
	});
	document.addEventListener("click", event => {
		if ((event.target as HTMLButtonElement).classList.contains("update_role")) {
			ROLE.updateRole(event);
		}
		else if ((event.target as HTMLButtonElement).classList.contains("delete_role")) {
			ROLE.DeleteRoleById(event);
		}
	});

	// Tab 3
	const PERM = new permission(token);
	document.getElementById("three").addEventListener("click", function() {
		PERM.getpermissions();
	});
	document.querySelector("#addpermission").addEventListener("click", function(e) {
		PERM.addPermission();
	})
	document.addEventListener("click", function(event) {
		if ((event.target as HTMLButtonElement).classList.contains("del_permission")) {
			PERM.DeletePermissionById(event);
		}
		else if ((event.target as HTMLButtonElement).classList.contains("update_permission")) {
			PERM.updatePermission(event);
		}
	});

	// Tab 1
	const rolePermisison = new RolePermission(token);
	document.getElementById("one").addEventListener("click", rolePermisison.setup.bind(rolePermisison));
	document.addEventListener('change', function(e: MouseEvent){
		let target = e.target as HTMLElement;
		if(target.nodeName == 'INPUT' && target.id.startsWith("checkbox-")){
			rolePermisison.checkboxListener(e);
		}
	});
	document.querySelector('#save-button').addEventListener('click', function(){
		rolePermisison.save();
	});

	// Dialog setup
	const inputDialog = document.querySelector("dialog.input-dialog") as HTMLDialogElement;
	const confirmDialog = document.querySelector("dialog.confirm-dialog") as HTMLDialogElement;
	
	inputDialog["getInput"] = inputModalFunction;
	inputDialog["openUp"] = openDialog;

	confirmDialog["openUp"] = openDialog;
	confirmDialog["confirm"] = confirmModalFunction;

	//
	rolePermisison.setup();
	navigationBarsss(Role,"navigation");
})();

function openDialog(heading: string= "", mode :"create"|"edit"|null= null){
	heading && (this.querySelector(".mdl-dialog__title").textContent = heading);
	mode && (this.querySelector(".mdl-button.positive").textContent = mode == "create" ? "ADD" : "EDIT");
	this.showModal();
}
const inputModalFunction = function(callback){
	const inputField = this.querySelector("input") as HTMLInputElement;

	const positiveButtonHandler = (e: MouseEvent) => {
		this.close();
		callback(inputField.value);
		inputField.value = "";
		(e.target as HTMLButtonElement).removeEventListener("click", positiveButtonHandler);
		
	}
	const negativeButtonHandler = (e: MouseEvent) => {
		this.close();
		callback();
		inputField.value = "";
		(e.target as HTMLButtonElement).removeEventListener("click", negativeButtonHandler);
	}

	this.querySelector(".positive").addEventListener("click", positiveButtonHandler);
	this.querySelector(".negative").addEventListener("click", negativeButtonHandler);
}
const confirmModalFunction = function(confirmText: string, callback){
	const dialogBody = this.querySelector(".mdl-dialog__content > p");
	const defaultText = dialogBody.textContent;
	dialogBody.textContent = confirmText;

	const positiveButtonHandler = (e: MouseEvent) => {
		this.close();
		callback(true);
		dialogBody.textContent = defaultText;
		(e.target as HTMLButtonElement).removeEventListener("click", positiveButtonHandler);
		
	}
	const negativeButtonHandler = (e: MouseEvent) => {
		this.close();
		callback(false);
		dialogBody.textContent = defaultText;
		(e.target as HTMLButtonElement).removeEventListener("click", negativeButtonHandler);
	}

	this.querySelector(".positive").addEventListener("click", positiveButtonHandler);
	this.querySelector(".negative").addEventListener("click", negativeButtonHandler);
}