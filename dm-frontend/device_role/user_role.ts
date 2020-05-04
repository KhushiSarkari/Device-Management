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

	if(!inputDialog.showModal){
		window["dialogPolyfill"].registerDialog(inputDialog);
		window["dialogPolyfill"].registerDialog(confirmDialog);
	}
	
	inputDialog["getInput"] = inputModalFunction;
	inputDialog["openUp"] = openDialog;

	confirmDialog["openUp"] = openDialog;
	confirmDialog["confirm"] = confirmModalFunction;

	rolePermisison.setup();
	navigationBarsss(Role,"navigation");
})();

function openDialog(heading: string= "", mode :"create"|"edit"|null= null){
	heading && (this.querySelector(".mdl-dialog__title").textContent = heading);
	mode && (this.querySelector(".mdl-button.positive").textContent = mode == "create" ? "ADD" : "EDIT");
	this.showModal();
}
function closeDialog(){
	this.close();
	let element = this.querySelector('input');
	if(element)
		element.value = "";
	element = this.querySelector('p');
	if(element)
		element.textContent = element.dataset.default;
	removeDialogListeners.call(this, this.listeners);
}
const removeDialogListeners = function(){
	this.querySelector(".positive").removeEventListener("click", this.listeners[0]);
	this.querySelector(".negative").removeEventListener("click", this.listeners[1]);
}
const addDialogListeners = function(){
	this.querySelector(".positive").addEventListener("click", this.listeners[0]);
	this.querySelector(".negative").addEventListener("click", this.listeners[1]);
}
const inputModalFunction = function(callback){
	const inputField = this.querySelector("input") as HTMLInputElement;

	const positiveButtonHandler = () => {
		callback(inputField.value);
		closeDialog.call(this);
	}
	const negativeButtonHandler = () => {
		callback();
		closeDialog.call(this);
	}
	this["listeners"] = [positiveButtonHandler, negativeButtonHandler];
	addDialogListeners.call(this);
}

const confirmModalFunction = function(confirmText: string, callback){
	const dialogBody = this.querySelector(".mdl-dialog__content > p");
	dialogBody.textContent = confirmText;

	const positiveButtonHandler = () => {
		callback(true);
		closeDialog.call(this);
	}
	const negativeButtonHandler = () => {
		callback(false);
		closeDialog.call(this);
	}
	this["listeners"] = [positiveButtonHandler, negativeButtonHandler];
	addDialogListeners.call(this);
}