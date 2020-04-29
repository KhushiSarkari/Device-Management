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
	document.querySelector("#btn_insert").addEventListener("click", function(e) {
		roles.updateRole();
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
			console.log("sdfghjkl");
			roles.update_data1(parseInt((event.target as HTMLButtonElement).value));
		}
	});
	document.addEventListener("click", e => {
		if ((e.target as HTMLButtonElement).className === "role_update_1") {
			console.log("sdfghjkl");
			console.log((event.target as HTMLButtonElement).value);
			roles.updateRole_1(parseInt((event.target as HTMLButtonElement).value));
			roles.headerTag34.innerHTML == "";
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

	document.getElementById("one").addEventListener("click", rolePermisison.setup);
	document.querySelector('body').addEventListener('change', function(e: MouseEvent){
		let target = e.target as HTMLElement;
		if(target.nodeName == 'INPUT' && target.id.startsWith("checkbox-")){
			rolePermisison.checkboxListener(e);
		}
	});
	document.querySelector('#save-button').addEventListener('click', function(){
		rolePermisison.save();
	});

	rolePermisison.setup();

	navigationBarsss(Role,"navigation");
})();