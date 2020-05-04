import { UserModel } from "./UserModel";
import { BASEURL } from "./globals";

export class CreateUserApi {
	token : string ="";
	constructor(token:string){
		this.token=token;
	}
	createUserData(data: UserModel) {
		console.log(data);
		return fetch(BASEURL + "/api/user/add", {
			method: "POST",
			headers: new Headers([["Content-Type","application/json"],["Authorization", `Bearer ${this.token}`]]),
			body: JSON.stringify(data)
		})
	}
}
