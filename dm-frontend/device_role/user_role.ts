import role from "./role";
import { RolePermission } from "./rolePermCheckbox";
import { navigationBarsss, amIUser } from "../globals";

(async function(){
	const token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
	let Role = await amIUser(token) == true ? "User" : "Admin";
	// action listeners
	var roles = new role(token);
	document.getElementById("two").addEventListener("click", function() {
		roles.getroles();
	});
	document.getElementById("three").addEventListener("click", function() {
		roles.getpermissions();
	});
	document.querySelector("#addrole").addEventListener("click", function(e) {
		roles.addRole();
	});
	document.querySelector("#btn_insert1").addEventListener("click", function(e) {
		roles.updatePermission();
	});
	document.body.addEventListener("click", function(event) {
		if ((event.target as HTMLButtonElement).dataset.id == "del_role") {
			console.log("iiiiiiiii");
			roles.DeleteRoleById(parseInt((event.target as HTMLButtonElement).value));
		}
	});
	document.body.addEventListener("click", function(event) {
		if ((event.target as HTMLButtonElement).dataset.id == "del_permission") {
			console.log("iiiiiiiii");
			roles.DeletePermissionById(parseInt((event.target as HTMLButtonElement).value));
		}
	});
	document.addEventListener("click", event => {
		if ((event.target as HTMLButtonElement).dataset.id == "update_role") {
			roles.updateRole(event);
			console.log("sdfghjkl");
		}
	});

	document.addEventListener("click", event => {
		if ((event.target as HTMLButtonElement).dataset.id == "update_permission") {
			console.log("sdfghjkl");
			roles.update_data2(parseInt((event.target as HTMLButtonElement).value));
		}
	});
	document.addEventListener("click", e => {
		if ((e.target as HTMLButtonElement).className === "permission_update_1") {
			console.log("sdfghjkl");
			console.log((event.target as HTMLButtonElement).value);
			roles.updatePermission_1(parseInt((event.target as HTMLButtonElement).value));
		}
	});

	const rolePermisison = new RolePermission(token);

	document.getElementById("one").addEventListener("click", rolePermisison.setup.bind(rolePermisison));
	document.querySelector('body').addEventListener('change', function(e: MouseEvent){
		let target = e.target as HTMLElement;
		if(target.nodeName == 'INPUT' && target.id.startsWith("checkbox-")){
			rolePermisison.checkboxListener(e);
		}
	});
	document.querySelector('#save-button').addEventListener('click', function(){
		rolePermisison.save();
	});

	document.addEventListener("click", event => {
		if ((event.target as HTMLButtonElement).dataset.id == "close_role") {
			this.headerTag34.innerHTML == "";
		roles.getroles();
		}
	});
	document.addEventListener("click", event => {
		if ((event.target as HTMLButtonElement).dataset.id == "close_perm") {
			this.headerTag35.innerHTML == "";
		roles.getpermissions();
		}
	});

	const dialog = document.querySelector("#popup") as HTMLDialogElement;
	const dialogPolyfill = window["dialogPolyfill"];
	
	dialog["modalFunction"] = modalFunction;
	dialog["openModal"] = openModal;

	if (!dialog.showModal) {
		dialogPolyfill.registerDialog(dialog);
	}

	rolePermisison.setup();

	navigationBarsss(Role,"navigation");
})();

function openModal(heading: string= "", mode :"create"|"edit"="create"){
	if(heading){
		this.querySelector(".mdl-dialog__title").textContent = heading;
	}
	this.querySelector(".mdl-button.positive").textContent = mode == "create" ? "ADD" : "EDIT";
	this.showModal();
}
const modalFunction = function(callback){
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