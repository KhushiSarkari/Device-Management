import { BASEURL } from "../globals";
import { HitApi } from "../Device-Request/HitRequestApi";

const inputDialog = document.querySelector("dialog.input-dialog") as HTMLDialogElement;
const confirmDialog = document.querySelector("dialog.confirm-dialog") as HTMLDialogElement;
const DeleteRows = function(table: any) {
	var rowCount = table.rows.length;
	for (var i = rowCount - 1; i > 0; i--) {
		table.deleteRow(i);
	}
}
export class role {
	data: any;
	table: HTMLTableElement = document.getElementById(
		"tab2"
	) as HTMLTableElement;
	roleName: string;
	token: string;
	API: HitApi;
	constructor(token: string){
		this.token = token;
		this.API = new HitApi(token);
	}
	async getroles() {
		const url = BASEURL + "/api/rolepermission";
		let data = await this.API.HitGetApi(url);
		this.data = data["Roles"].map(roleObj => {
			return { RoleId: roleObj["RoleId"], RoleName: roleObj["RoleName"] };
		});
		this.dynamicGenerateRole(this.table);
		return data;
	}
	dynamicGenerateRole(table: any) {
		let loop = 0;
		DeleteRows(table);
		for (loop = 0; loop < this.data.length; loop++) {
			var row = table.insertRow(loop + 1);
			row.setAttribute("data-role-id", this.data[loop]["RoleId"]);
			var cell = row.insertCell(0);
			cell.innerHTML = this.data[loop]["RoleName"];
			var cell1 = row.insertCell(1);
			var cell2 = row.insertCell(1);
			cell1.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored update_role">
				Edit
			</button>`;
			cell2.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored txt-danger delete_role">
				Delete
			</button>`;
		}
	}
	DeleteRoleById(ev: MouseEvent) {
		const roleId = parseInt((ev.target as HTMLButtonElement).closest("tr").dataset.roleId);
		confirmDialog["openUp"]();
		confirmDialog["confirm"]("Are you sure you want to delete this role?", (decision) =>{
			if (decision) {
				let uri = BASEURL + "/api/role/" + roleId + "/delete";
				fetch(uri, {
					method: "DELETE",
					headers: new Headers({"Authorization": `Bearer ${this.token}`})
				})
				.then(response => {
					if(!response.ok){
						throw new Error(response.statusText);
					}
					alert("Role deleted");
					this.getroles();
				})
				.catch(ex => {
					alert("An error occured : " + ex.message);
				});
			} else {
				console.log("delete failed");
			}
		});
	}
	
	bindRoleData(roleName: string, roleId: number = 0) {
		this.data = {
			RoleName: roleName
		};
		if(roleId){
			this.data["RoleId"] = roleId;
		}
	}
	addRole() {
		inputDialog["openUp"]("Enter role", "create");
		inputDialog["getInput"]((inputVal: string) => {
			if(inputVal){
				this.bindRoleData(inputVal);
				this.postRoleData()
				.then(response => {
					if (!response.ok) {
						throw new Error(response.statusText);
					}
					alert("role inserted");
					this.getroles();
				})
				.catch(ex => {
					alert("An error occured : " + ex.message);
				});
			}
		});
	}
	postRoleData() {
		let url = BASEURL + "/api/role/update";
		return this.API.HitPostApi(url, this.data);
	}
	updateRole(ev: MouseEvent) {
		const roleId = parseInt((ev.target as HTMLButtonElement).closest("tr").dataset.roleId);
		inputDialog["openUp"]("Enter new Role", "edit");
		inputDialog["getInput"]((inputVal: string) => {
			if(inputVal){
				this.bindRoleData(inputVal, roleId);
				this.postRoleData()
				.then(response => {
					if (!response.ok) {
						throw new Error(response.statusText);
					}
					alert("role updated");
					this.getroles();
				})
				.catch(ex => {
					alert("An error occured : " + ex.message);
				});
			}
		});
	}
}

export class permission{
	data: any;
	table: HTMLTableElement = document.getElementById(
		"tab3"
	) as HTMLTableElement;
	PermissionName: string;
	token: string;
	API: HitApi;
	constructor(token: string){
		this.token = token;
		this.API = new HitApi(token);
	}
	
	async getpermissions() {
		const url = BASEURL + "/api/rolepermission";
		let data = await this.API.HitGetApi(url);
		this.data = data["Permissions"].map(permObj => {
			return {
				PermissionId: permObj["PermissionId"],
				PermissionName: permObj["PermissionName"]
			};
		});
		this.dynamicGeneratePermission(this.table);
		return data;
	}
	dynamicGeneratePermission(table: any) {
		let loop = 0;
		DeleteRows(table);
		for (loop = 0; loop < this.data.length; loop++) {
			var row = table.insertRow(loop + 1);
			row.setAttribute("data-permission-id", this.data[loop]["PermissionId"]);
			var cell = row.insertCell(0);
			cell.innerHTML = this.data[loop]["PermissionName"];
			var cell1 = row.insertCell(1);
			var cell2 = row.insertCell(1);
			cell1.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored update_permission">
				Edit
			</button>`;
			cell2.innerHTML = `<button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored txt-danger del_permission">
				Delete
			</button>`;
		}
	}
	async DeletePermissionById(ev: MouseEvent) {
		const permissionId = parseInt((ev.target as HTMLButtonElement).closest("tr").dataset.permissionId);
		confirmDialog["openUp"]();
		confirmDialog["confirm"]("Are you sure you want to delete this permission?", (decision) =>{
			if (decision) {
				let uri = BASEURL + "/api/permission/" + permissionId + "/delete";
				fetch(uri, {
					method: "DELETE",
					headers: new Headers({"Authorization": `Bearer ${this.token}`})
				}).then(response => {
					if(!response.ok){
						throw new Error(response.statusText);
					}
					alert("Permission deleted");
					this.getpermissions();
				})
				.catch(ex => {
					alert("An error occured : " + ex.message);
				});
			} else {
				console.log("delete failed");
			}
		});
	}
	bindPermissionData(permissionName: string, permissionId: number = 0) {
		this.data = {
			PermissionName: permissionName
		};
		if(permissionId){
			this.data["PermissionId"] = permissionId;
		}
	}
	addPermission() {
		inputDialog["openUp"]("Enter Permission", "create");
		inputDialog["getInput"]((inputVal: string) => {
			if(inputVal){
				this.bindPermissionData(inputVal);
				this.postPermissionData()
				.then(response => {
					if (!response.ok) {
						throw new Error(response.statusText);
					}
					alert("Permission created");
					this.getpermissions();
				})
				.catch(ex => {
					alert("An error occured : " + ex.message);
				});
			}
		});
	}
	postPermissionData() {
		let url = BASEURL + "/api/permission/update";
		return this.API.HitPostApi(url, this.data);
	}
	updatePermission(ev: MouseEvent) {
		const permissionId = parseInt((ev.target as HTMLButtonElement).closest("tr").dataset.permissionId);
		inputDialog["openUp"]("Enter new permission", "edit");
		inputDialog["getInput"]((inputVal: string) => {
			if(inputVal){
				this.bindPermissionData(inputVal, permissionId);
				this.postPermissionData()
				.then(response => {
					if (!response.ok) { 
						throw new Error(response.statusText);
					}
					alert("Permission updated");
					this.getpermissions();
				})
				.catch(ex => {
					alert("An error occured : " + ex.message);
				});
			}
		});
	}
}