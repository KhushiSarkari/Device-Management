import { BASEURL } from '../globals';
import { UserModel } from "../UserModel";
export class GetUserApi 
{
	token : string ="";
	constructor(token:string){
		this.token=token;
	}
	array: any = [];
/////API TO SORT
	getSort(uri : string) {
		return fetch(uri,{
			headers: new Headers({"Authorization": `Bearer ${this.token}`})})
			.then(Response => {
				return Response.json();
			})
			.then(data => {
				return data.map(obj => new UserModel(obj));
			});
			}
/////API GET		
	getRequest() {
		return fetch(BASEURL + "/api/user",{
			headers: new Headers({"Authorization": `Bearer ${this.token}`})}
		)
			.then(Response => {
				return Response.json();
			})
			.then(data => {
				console.log(data);
				data.forEach((userObject: any) => {
					this.array.push(new UserModel(userObject));  
				});
				console.log('Array', this.array);
				
			})
			.catch(err => console.log(err));
	}
	getUserById(userId: number) {
		return fetch(BASEURL + "/api/user/" + userId,
		{
			headers: new Headers({"Authorization": `Bearer ${this.token}`})})
			.then(res => res.json())
			.then(data => {
				return data;
			})
			.catch(err => console.error(err));
	}
	////API TO MAKE ACTIVE INACTIVE
	userInactive(userId: number,data : string) {
		return fetch(BASEURL + "/api/user/" + userId + "/" + data + "", {
			headers: new Headers({"Authorization": `Bearer ${this.token}`})})
			.then(res =>
				console.log(res.status))
			
			.catch(err => console.error(err));
	}
/////API TO DELETE
	deleteData(userId: number) {
	 return fetch(BASEURL + "/api/user/" + userId + "/remove", {
			method: "DELETE",
				headers: new Headers({"Authorization": `Bearer ${this.token}`})
		});
	}

	searchUser() {
		let search = (document.getElementById("fixed-header-drawer-exp")as HTMLInputElement).value;
		return fetch(BASEURL+ "/api/user?search=" + search, 
		{
			headers: new Headers({"Authorization": `Bearer ${this.token}`})})
			.then(Response => {
				return Response.json();
			})
			.then(data => {
				console.log(data);
				data.forEach((userObject: any) => {
					this.array.push(new UserModel(userObject));  
				});
				console.log('Array', this.array);
				return this.array;
			})
			.catch(err => console.log(err));
	}
}
function populateDropdown(selectElement: HTMLSelectElement, data) {
    let htmlString = '';
    for (let dataPair of data) {
        htmlString += '<option data-id="' + dataPair.id + '" value="' + dataPair.name + '">' + dataPair.name + '</option>';
    }
    selectElement.innerHTML = htmlString;
}
