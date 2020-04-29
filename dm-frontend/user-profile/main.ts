import { UserModel } from './UserModel';
import { createObjectFromForm, populateFormFromObject } from './databinding';
import { validate } from "./validate";
import { dropDownListen } from "./dropDownListener";
 import * as util from "../utilities";
import { BASEURL, navigationBarsss, amIUser } from '../globals';
(async function () {
    let token = JSON.parse(sessionStorage.getItem("user_info"))["token"];
    let role = await amIUser(token) == true ? "User" : "Admin";
    class UserData {
        token: string = "";
        constructor(token: string) {
            this.token = token;
        }
        data: any;
        url: string;
        body: any;
        //TODO CLASSNAME AND GETUSERBYID REUSE
        async getOneUser(userId: string) {
            this.url = BASEURL + "/api/user/" + userId;
            let data = await this.getApiCall(this.url);
            this.data = await data;
            return data;
        }
        async updateData(data, userId: string) {
            return fetch(BASEURL + "/api/user/" + userId + "/update", {
                method: 'PUT',
                headers: new Headers([["Content-Type", "application/json"], ["Authorization", `Bearer ${this.token}`]]),
                body: JSON.stringify(data)
            });
        }
        async getApiCall(URL: any) {
            let response = await fetch(URL,
                { headers: new Headers({ "Authorization": `Bearer ${token}` }) });
            let data = await (response.json());
            console.log(data);
            return (await new UserModel(data));
        }
    }
    const userId = JSON.parse(sessionStorage.getItem("user_info"))["id"];
    var user = new UserData(token);
    var userObject: UserModel;
    const form = document.querySelector('form') as HTMLFormElement;
    dropDownListen(form, token);
    user.getOneUser(userId).then(function (data) {
        userObject = data;
        // @ts-ignore
        populateFormFromObject(userObject, form, token);

    });
     util.addressCheck();
    document.querySelector('form').addEventListener('click', function (ev) {
       
        if ((ev.target as HTMLInputElement).className.includes("savemydata")) {
            ev.preventDefault();
            if (validate() == 0) {
               
                return;
            }          
            user.updateData(createObjectFromForm(this), userId).then(function () { 
                user.getOneUser(userId).then(function (data) {
                   const  userObject = data;
                    // @ts-ignore
                    populateFormFromObject(userObject, form, token);
            
                });
             })
            alert("Record Updated");
        }
    });
    navigationBarsss(role, "navigation");
})();
