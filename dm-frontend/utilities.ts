import { UserData } from "./dropdown";

export function openForm(formMode: string) {
	(document.querySelector("#myForm") as HTMLFormElement).style.display="block";
	if(formMode == "create"){
		fillInitialDropdowns((document.querySelector("#myForm") as HTMLFormElement));
	}
}
​
export function closeForm() {
    (document.querySelector("#myForm") as HTMLFormElement).style.display="none";
	(document.querySelector("#myForm") as HTMLFormElement).reset();
	clearDropdowns(document.querySelector("#myForm") as HTMLFormElement);

}
export function formatDate1(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}
export function formatPhone(str1:string,str2:string) {
 
	if(str2===""){
	return "";
	}
	let result ="+(";
	result+=str1+") ";
	result+= str2.slice(0,3)+"-"+str2.slice(3,7)+"-"+str2.slice(7);    //XXXXXXXXXX=>XXX-XXXX-XXX
	return result;
}
export function formatPhone1(str:string) {

	let a =  str.split("-");   //XXX-XXXX-XXX =>XXXXXXXXXX
console.log(a);
return a[0]+a[1]+a[2];
}	
export function concatName(value : any )
{
    let name = "";
    name += value.salutation + " " + value.firstName ;
    if(value.middleName != "")
    name+= " " + value.middleName ;
    name +=  " " + value.lastName;
    return name;
}    
export function concatSpecs(data : any)
{
    var specifications = "";
    for(let [key , value] of Object.entries(data) ) 
        
        if( value != ""){     
            specifications += key +":" + value +" ";
            if(key=="storage")
                specifications+="<br>";
        }   
             
    return specifications;

}
export function openModal(modalElement: HTMLElement){
    document.body.classList.add("modal-open");
	modalElement.classList.add("show");
}
export function closeModal(modalElement: HTMLElement){
    document.body.classList.remove("modal-open");
	modalElement.classList.remove("show");
}
function fillInitialDropdowns(formElement: HTMLFormElement){
	const dropDown= new UserData("");
	// Fill all country dropdowns
	formElement.querySelectorAll("[id^=addresses]").forEach((addressDiv: HTMLDivElement) => {
		const country = addressDiv.querySelector(".country") as HTMLSelectElement;
		dropDown.getCountry(country);
	});
	// Fill all country code dropdowns
	formElement.querySelectorAll("[id^=phones]").forEach((phoneDiv: HTMLDivElement) => {
		const countryCode = phoneDiv.querySelector(".countryCode") as HTMLSelectElement;
		dropDown.getCountryCode(countryCode);
	});
	// Fill salutation dropdown
	dropDown.getSalutation(formElement.querySelector("#salutation"));
	dropDown.departmentcall(formElement.querySelector("#department"));/////
}
function clearDropdowns(formElement: HTMLFormElement){
	formElement.querySelectorAll('select').forEach((selectElement: HTMLSelectElement) => {
		selectElement.value = "";
		
	})
}
export function clearSpan(formElement: HTMLFormElement){
	formElement.querySelectorAll('span').forEach((spanElement: HTMLSelectElement) => {
		spanElement.textContent = "";
		
	})
}
export async function addressCheck()
{
        let checkbox = document.querySelector('.sameaddress');

	checkbox.addEventListener('change', async function() {
		if(this.checked) {
			var container = document.getElementById("addresses1")
			var curradd1 =(container.querySelector(".addressLine1")as HTMLInputElement).value;
			var curradd2 =(container.querySelector(".addressLine2")as HTMLInputElement).value;
			var currcity =(container.querySelector(".city")as HTMLInputElement).value;
			var currstate =(container.querySelector(".state")as HTMLInputElement).value;
			var currcountry =(container.querySelector(".country")as HTMLInputElement).value;
            var currpincode =(container.querySelector(".pin")as HTMLInputElement).value;
            
			const dropDown= new UserData();
			var peradd1 =curradd1 ;
			var peradd2 =curradd2 ;
			var percity =currcity ;
			var perstate =currstate ;
			var perpcountry =currcountry ;
			var perpincode =currpincode ;
			

			var container1 = document.getElementById("addresses2");
			var city=(container1.querySelector(".city")as HTMLSelectElement);
			var state=(container1.querySelector(".state")as HTMLSelectElement);
			var country=(container1.querySelector(".country")as HTMLSelectElement);
			(container1.querySelector(".addressLine1")as HTMLInputElement).value = peradd1;
			(container1.querySelector(".addressLine2")as HTMLInputElement).value = peradd2;
			country.value= perpcountry;
			await dropDown.getState(state,country);
			state.value = perstate;
			await dropDown.getCity(city,state);
			city.value = percity;
			(container1.querySelector(".pin")as HTMLInputElement).value= perpincode;
		} else {
            var container1 = document.getElementById("addresses2");
            

			(container1.querySelector(".addressLine1")as HTMLInputElement).value = "";
			(container1.querySelector(".addressLine2")as HTMLInputElement).value = "";
			(container1.querySelector(".city")as HTMLInputElement).value = "";
			(container1.querySelector(".state")as HTMLInputElement).value = "";
			(container1.querySelector(".country")as HTMLInputElement).value=  "";
			(container1.querySelector(".pin")as HTMLInputElement).value= "";
		}
	});


}