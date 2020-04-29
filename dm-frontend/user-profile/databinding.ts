import * as models from "../UserModel";
import { UserData } from "../dropdown";

export function createObjectFromForm(formElement: HTMLFormElement) {
	let modelObject = new models.UserModel();
	modelObject.salutation = (formElement["salutation"] as HTMLInputElement).value;
	modelObject.firstName = (formElement["firstName"] as HTMLInputElement).value;
	modelObject.middleName =(formElement["middleName"] as HTMLInputElement).value || null;
	modelObject.lastName = (formElement["lastName"] as HTMLInputElement).value;
	modelObject.departmentName = (formElement["department"] as HTMLInputElement).value;
	modelObject.designationName = (formElement["designation"] as HTMLInputElement).value;
	modelObject.email = (formElement["email"] as HTMLInputElement).value;
	modelObject.userId = parseInt((formElement["userId"] as HTMLInputElement).value);
	modelObject.password = (formElement["password"] as HTMLInputElement).value;
	modelObject.dob = (formElement["dob"] as HTMLInputElement).value;
	modelObject.gender = (formElement["gender"] as HTMLInputElement).value;
	modelObject.status = (formElement["status"] as HTMLInputElement).value;
	modelObject.roleName = (formElement["roleName"] as HTMLInputElement).value;

	modelObject.doj = (formElement["doj"] as HTMLInputElement).value;

	for (let i = 1; i <= 3; i++) 
	{
		const container = formElement.querySelector("#phones" + i) as HTMLDivElement;
		let contactNumberType = (container.querySelector(".contactNumberType") as HTMLInputElement).value;
		let number = (container.querySelector(".number") as HTMLInputElement).value;
		let countryCode = (container.querySelector(".countryCode") as HTMLInputElement).value;
		let areaCode = (container.querySelector(".areaCode") as HTMLInputElement).value;
		if (number||countryCode)modelObject.phones.push
		({contactNumberType,number,countryCode,areaCode});
	}

	for (let i = 1; i <= 2; i++) 
	{
		const container = formElement.querySelector("#addresses" + i) as HTMLDivElement;
		let addressType = (container.querySelector(".addressType") as HTMLInputElement).value;
        let addressLine1 = (container.querySelector(".addressLine1") as HTMLInputElement).value;
        let addressLine2 = (container.querySelector(".addressLine2") as HTMLInputElement).value;
		let city = (container.querySelector(".city") as HTMLInputElement).value;
		let state = (container.querySelector(".state") as HTMLInputElement).value;
		let country = (container.querySelector(".country") as HTMLInputElement).value;
		let pin = (container.querySelector(".pin") as HTMLInputElement).value;
		if (addressLine1)modelObject.addresses.push
		({addressType,addressLine1,addressLine2,city,state,country,pin});
	}

	return modelObject;
}

export async  function populateFormFromObject(
	data: models.UserModel,
	form: HTMLFormElement,token:string
) {

	form["salutation"].value   =data.salutation;
	form["firstName"].value    =data.firstName;
	form["middleName"].value   =data.middleName;
	form["lastName"].value     =data.lastName;
	form["email"].value        =data.email;
	form["userId"].value       =data.userId;
	form["password"].value     ="";
	form["confirmpassword"].value     ="";
	form["dob"].value          =data.dob;
	form["gender"].value       =data.gender;
	form["status"].value       =data.status;
	form["roleName"].value     =data.roleName;
	form["doj"].value          =data.doj;
	const dropDown = new UserData(token);
	
	const departmentElement = form["department"] as HTMLSelectElement;
	const designationElement = form["designation"] as HTMLSelectElement;
	
	// Fill department and only after it finishes, fill the designation
	dropDown.departmentcall(departmentElement)
	.then(function(){
		departmentElement.value = data.departmentName;
		dropDown.departdesgcall(designationElement, departmentElement).then(function(){
			designationElement.value = data.designationName;
		});
	});

	for(let i =0;i<3;i++)
	{
		let container=form.querySelector("#phones"+(i+1));
		(container.querySelector(".contactNumberType")as HTMLInputElement).value=data.phones[i].contactNumberType;
		(container.querySelector(".number")as HTMLInputElement).value=data.phones[i].number;
		let countryCode=(container.querySelector(".countryCode")as HTMLSelectElement);
		
		await dropDown.getCountryCode(countryCode);
		countryCode.value=data.phones[i].countryCode;

		(container.querySelector(".areaCode")as HTMLInputElement).value=data.phones[i].areaCode;
	}

	for(let i =0;i<2;i++)
	{
		let container=form.querySelector("#addresses"+(i+1));

		(container.querySelector(".addressType")as HTMLInputElement).value=data.addresses[i].addressType;
		(container.querySelector(".addressLine1")as HTMLInputElement).value=data.addresses[i].addressLine1;
		(container.querySelector(".addressLine2")as HTMLInputElement).value=data.addresses[i].addressLine2;
		(container.querySelector(".pin")as HTMLInputElement).value=data.addresses[i].pin;

		const city=(container.querySelector(".city")as HTMLSelectElement);
		const state=(container.querySelector(".state")as HTMLSelectElement);
		const country=(container.querySelector(".country")as HTMLSelectElement);

		// Fill country dropdown and only when it completes, fill the state
		// Same process follows for city
		await dropDown.getCountry(country);
		if(data.addresses[i].country){
		country.value=data.addresses[i].country;

		await dropDown.getState(state,country);
		state.value=data.addresses[i].state;

		await dropDown.getCity(city,state);
		city.value=data.addresses[i].city;
		}
	}
  
}


 