import { formatPhone } from "./utilities";
export class UserModel 
{
	salutation: string = "";
	firstName: string = "";
	middleName: string | null = null;
	lastName: string = "";
	departmentName: string = "";
	designationName: string = "";
	email: string = "";
	password: string = "";
	userId : number = 0;
	dob: string = "";
	gender: string = "";
	roleName:string="";
	status:string="";
	doj: string = "";

	phones: phones[];
	addresses: addresses[];
	
	constructor(data: any | null = null) 
	{
		this.addresses = new Array<addresses>();
		this.phones = new Array<phones>();
		if(data){
					
		  this.salutation=data.salutation;
          this.firstName=data.firstName;
          this.middleName=data.middleName;
          this.lastName=data.lastName;
          this.departmentName=data.departmentName;
          this.designationName=data.designationName;
          this.email=data.email;
          this.roleName=data.roleName;
          this.userId=data.userId;
          this.password=data.password;
          this.dob=data.dob;
          this.gender=data.gender;
          this.doj=data.doj;
          this.status=data.status;
		  this.addresses = new Array<addresses>();
		  this.phones = new Array<phones>();

		for(const phoneObject of data.phones)
			{
			  this.phones.push(new phones(phoneObject));
			}
		for(const addressObject of data.addresses)
			{
			  this.addresses.push(new addresses(addressObject));
			}
		}	
    }

   getName()
    {
	   return this.salutation + " "+this.firstName +" "+ this.middleName +" " + this.lastName;
	}
   getAddress()
    {
	   return this.addresses[0].addressLine1 + " ," + this.addresses[0].addressLine1 + "," +
	   this.addresses[0].city + " ,"+ this.addresses[0].state + " ,"+ this.addresses[0].country + " ," + this.addresses[0].pin; 
	}
   getDate(dateString: string)
    {
		const [year, month, day] = this[dateString].split("-").map(Number);
		return new Date(year, month - 1, day);
	}
	 GenerateTableRow()
	 {
		let status =  this.status=="Active"?"checked":""
		return `<tr>
					<td>${this.getName()}</td>
					<td>${this.email} </td>
					<td>${this.roleName}</td>
					<td>${this.status} </td>
					<td>${formatPhone(this.phones[0].countryCode,this.phones[0].number)} </td>
					<td>
						<label class="switch">
							<input type="checkbox" id="${this.userId}" class="userCheckStatus"  data-toggle="modal"  data-target="#aiModal" ${status}>
							<span class="slider round"></span>
						</label>
					</td>
					<td><input  type="button" id="${this.userId}" class="userEditData mdl-button mdl-js-button mdl-button--primary mdl-button--colored" value="Edit" /></td>
					<td><input  type="button" id="${this.userId}" class="userDeleteData mdl-button mdl-js-button mdl-button--primary text-danger " data-toggle="modal" value="Delete" data-target="#deleteModal" /></td>
		        </tr>`;//class="userDeleteData" value="Delete"

	 }
	
}
export class phones 
{
	contactNumberType: string;
	number: string;
	countryCode: string;
	areaCode: string;
	constructor(phoneObject: phones)
	{
		this.contactNumberType = phoneObject.contactNumberType;
		this.number = phoneObject.number;
		this.countryCode = phoneObject.countryCode;
		this.areaCode = phoneObject.areaCode;
	}
}
export class addresses 
{
	addressType: string;
	addressLine1: string;
	addressLine2: string;
	city: string;
	state: string;
	country: string;
	pin: string;
	constructor(addressObject: addresses) 
	{
		this.addressType = addressObject.addressType;
		this.addressLine1 = addressObject.addressLine1;
		this.addressLine2 = addressObject.addressLine2;
		this.city = addressObject.city;
		this.state = addressObject.state;
		this.country = addressObject.country;
		this.pin = addressObject.pin;
	}
}
