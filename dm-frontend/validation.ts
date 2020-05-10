export function validateForm(formMode:string) {
    
    var result= salutationvalidation()*firstNamevalidation()*lastNamevalidation()*//emailvalidation()*
    passwordvalidation(formMode)*
    confirmpasswordvalidation(formMode)*roleName()*
    departmentvalidation()*
    designationvalidation()*
    phonevalidation(formMode,"phones1")*phonevalidation(formMode,"phones2")*phonevalidation(formMode,"phones3")*
    addressvalidation(formMode,"addresses1")*addressvalidation(formMode,"addresses2");
return result == 1 ? true : false;
}
export function validate() {
    return firstNamevalidation() 
    * Middlenamevalidation() 
    *lastNamevalidation()*
    passwordvalidation("edit")*
    confirmpasswordvalidation("edit")*
    salutationvalidation()*
    gendervalidation()*
    date0fbirthvalidation()*
    addressvalidation("edit","addresses1")*
    addressvalidation("edit","addresses2")*
    phonevalidation("edit","phones1")*
    phonevalidation("edit","phones2")*
    phonevalidation("edit","phones3");
}
export function remove() {
    document.querySelectorAll('form span.validationSpan').forEach((spanElement: HTMLSelectElement) => {
		spanElement.textContent = "";
		
	})
    
}
function salutationvalidation(){
    var salutations = (document.getElementById('salutation') as HTMLSelectElement).value;
    if (salutations == "") {
        (document.getElementById('salutations') as HTMLInputElement).innerHTML = "Select Salutation";
        return 0;
    } else {
        (document.getElementById('salutations') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function gendervalidation() {
    var genders = (document.getElementById('gender') as HTMLInputElement).value;
    if (genders == "") {
        document.getElementById('genders').innerHTML = "Please Select The Gender";
        return 0;
    }
    else {
        document.getElementById('genders').innerHTML = "";
        return 1;
    }
}
function firstNamevalidation() {

    var firstNames = (document.getElementById('firstName') as HTMLInputElement).value;
    if (firstNames == "") {
        (document.getElementById('firstNames') as HTMLInputElement).innerHTML = "Fill First Name";
        return 0;
    }else if (!isNaN(parseInt(firstNames))) {
        (document.getElementById('firstNames')as HTMLInputElement).innerHTML = "Only Characters Allowed";
        return 0;
    }
    else if (firstNames.length < 2 || firstNames.length > 20) {
        (document.getElementById('firstNames') as HTMLInputElement).innerHTML = "First Name Should Be Between 2 and 20";
        return 0;
    }
     else {
        (document.getElementById('firstNames') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function Middlenamevalidation() {
    var middlenames = (document.getElementById('middleName') as HTMLInputElement).value;
    if (middlenames=="")
    {
        return 1;
    }
     else if (middlenames.length > 20) {
        document.getElementById('middlenames').innerHTML = "MiddleName Should Not Be More Than 20 Characters";
        return 0;
    }
    else {
        document.getElementById('middlenames').innerHTML = "";
        return 1;
    }
}
function lastNamevalidation() {

    var lastNames = (document.getElementById('lastName') as HTMLInputElement).value;
    if (lastNames == "") {
        (document.getElementById('lastNames') as HTMLInputElement).innerHTML = "Fill Last Name";
        return 0;
    }else if (!isNaN(parseInt(lastNames))) {
        (document.getElementById('lastNames')as HTMLInputElement).innerHTML = "Only Characters Allowed";
        return 0;
    }
    else if (lastNames.length < 2 || lastNames.length > 20) {
        (document.getElementById('lastNames') as HTMLInputElement).innerHTML = "Last Name Should Be Between 2 and 20";
        return 0;
    }
     else {
        (document.getElementById('lastNames') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function passwordvalidation(formMode : string) {

    var passwords = (document.getElementById('password') as HTMLInputElement).value;
    if(formMode=="edit" && passwords=="")
        passwords="123#abc#45";
    if (passwords == "") {
        (document.getElementById('passwords') as HTMLInputElement).innerHTML = "Fill Password";
        return 0;
    } else if (passwords.length < 5 || passwords.length > 20) {
        (document.getElementById('passwords') as HTMLInputElement).innerHTML = "Password Should Be Between 5 and 20";
        return 0;
    } else {
        (document.getElementById('passwords') as HTMLInputElement).innerHTML = "";
        return 1;
    
    }
}
function confirmpasswordvalidation(formMode : string) {

    var passwords = (document.getElementById('password') as HTMLInputElement).value;

    var confirmpasss = (document.getElementById('confirmpass') as HTMLInputElement).value;
    if(formMode=="edit" && passwords=="" && confirmpasss=="")
    passwords=confirmpasss="w3e4r5t6y7";
    if (confirmpasss == "") {
        (document.getElementById('confirmpasss') as HTMLInputElement).innerHTML = "Fill Confirm Password";
        return 0;
    } else if (confirmpasss.length < 5 || confirmpasss.length > 20) {
        (document.getElementById('confirmpasss') as HTMLInputElement).innerHTML = "Confirm Password Should Be Between 5 and 20";
        return 0;
    } else if (passwords != confirmpasss) {
        (document.getElementById('confirmpasss') as HTMLInputElement).innerHTML = "Passwords Dont Match";
        return 0;
    } else {
        (document.getElementById('confirmpasss') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function roleName() {
    var roleNames = (document.getElementById('roleName') as HTMLInputElement).value;
    if (roleNames == "") {
        (document.getElementById('roleNames') as HTMLInputElement).innerHTML = "Fill Role";
        return 0;
    } else {
        (document.getElementById('roleNames') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function departmentvalidation() {
    var departments = (document.getElementById('department') as HTMLInputElement).value;
    if (departments == "") {
        (document.getElementById('departments') as HTMLInputElement).innerHTML = "Fill Department";
        return 0;
    } else {
        (document.getElementById('departments') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function designationvalidation() {
    var designations = (document.getElementById('designationName') as HTMLInputElement).value;
    if (designations == "") {
        (document.getElementById('designations') as HTMLInputElement).innerHTML = "Fill Designation After Selecting Department";
        return 0;
    } else {
        (document.getElementById('designations') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
// function emailvalidation() {
//     var emails = (document.getElementById('email') as HTMLInputElement).value;
//     if (emails == "") {
//         (document.getElementById('emails') as HTMLInputElement).innerHTML = "Fill Email";
//         return 0;
//     }else if (emails.indexOf('@') <= 0) {
//         document.getElementById('useremails').innerHTML = "@ Is At Invalid position";
//         return 0;
//      } else {
//             let str = emails;
//         str=str.toLowerCase();
//         let st =str.split("@");
//         if(st[1]!="ex2india.com"){
//             document.getElementById('emails').innerHTML = "Not A Valid Domain";
//             return 0;
//         }
//             document.getElementById('emails').innerHTML = "";
//                 return 1; 
//         }
//     }

    function areacodevalidation(containerId: string) {
        var areacode1 = (document.querySelector("#" + containerId + ' .areaCode') as HTMLInputElement).value;
        if(areacode1=="")
        {
            document.querySelector("#" + containerId + ' .areacodespan').innerHTML = "";
            return 1;
    
        }
        else if (isNaN(parseInt(areacode1))) {
            document.querySelector("#" + containerId + ' .areacodespan').innerHTML = "areacode must be in digits";
            return 0;
        }
        else {
            document.querySelector("#" + containerId + ' .areacodespan').innerHTML = "";
            return 1;
        }
    }

// function currAddType(containerId: string){   
//     var addressType1 = (document.querySelector("#" + containerId + ' .addressType') as HTMLInputElement).value;
//     if (addressType1 == "") {
//         document.querySelector("#" + containerId + ' .addressTypeSpan').innerHTML = "Select Type";
//         return 0;
//     } else {
//         document.querySelector("#" + containerId + ' .addressTypeSpan').innerHTML = "";
//         return 1;
//     }
// }
function addressvalidation(formmode:string ,containerId: string) {
    var container=document.querySelector("#" +containerId)
    var addresses1 = (document.querySelector("#" + containerId + ' .addressLine1') as HTMLInputElement).value;
    if(formmode=="create"&&addresses1=="")
    {
        document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "";
        return 1;
    }
    if (container.getAttribute("aria-required")!="true" &&(addresses1=="")) {
        return 1;
    }
   else if (addresses1 == "") {
        document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "Fill  addresss line1";
        return 0;
    } else if (addresses1.length > 30) {
        document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "Consider Using addressLine 2";
        return 0;
    }
    else if(addresses1!="")
    {
        document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "";
        return addressvalidation1(containerId)*countryvalidation(containerId)*statevalidation(containerId)*
        cityvalidation(containerId)*pinvalidation(containerId);
    }
    else {
        document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "";
        return 1;
    }
}
function addressvalidation1(containerId: string) {
    var addresses2 = (document.querySelector("#" + containerId + ' .addressLine2') as HTMLInputElement).value;
    if (addresses2 == "") {
        return 1;
    }
     else if (addresses2.length > 30) {
        document.querySelector("#" + containerId + ' .addressLine2span').innerHTML = "addressline2 Should Not Be More Than 30 Characters";
        return 0;
    }
    else {
        document.querySelector("#" + containerId + ' .addressLine2span').innerHTML = "";
        return 1;
    }
}
function countryvalidation(containerId: string) {
    var country1 = (document.querySelector("#" + containerId + ' .country') as HTMLInputElement).value;
    if (country1 == "") {
        document.querySelector("#" + containerId + ' .countryspan').innerHTML = "Please Select The Country";
        return 0;
    } 
    else {
        document.querySelector("#" + containerId + ' .countryspan').innerHTML = "";
        return 1;
    }
}
function statevalidation(containerId: string) {
    var state1 = (document.querySelector("#" + containerId + ' .state') as HTMLInputElement).value;
    if (state1 == "") {
        document.querySelector("#" + containerId + ' .statespan').innerHTML = "Please Select The State";
        return 0;
    } 
    else {
        document.querySelector("#" + containerId + ' .statespan').innerHTML = "";
        return 1;
    }
}
function cityvalidation(containerId: string) {
    var city1 = (document.querySelector("#" + containerId + ' .city') as HTMLInputElement).value;
    if (city1 == "") {
        document.querySelector("#" + containerId + ' .cityspan').innerHTML = "Please Select The City";
        return 0;
    } 
    else {
        document.querySelector("#" + containerId + ' .cityspan').innerHTML = "";
        return 1;
    }
}
function date0fbirthvalidation() {
    var dobs= (document.getElementById('dob') as HTMLInputElement).value;
    if (dobs == "0001-01-01") {
        
        document.getElementById('dobs').innerHTML = "Fill Correct date of birth";
        return 0;
    } 
     else {
        document.getElementById('dobs').innerHTML = "";
        return 1;
    }
}
function pinvalidation(containerId: string) {
    var pin = (document.querySelector("#" + containerId + ' .pin') as HTMLInputElement).value;
    if(pin=="")
    {
        document.querySelector("#" + containerId + ' .pinspan').innerHTML = "";
        return 1;

    }
    else if(pin.length<6||pin.length>12)
    {
        document.querySelector("#" + containerId + ' .pinspan').innerHTML = "pincode not valid";
        return 0;

    }
    
     else if (isNaN(parseInt(pin))) {
        document.querySelector("#" + containerId + ' .pinspan').innerHTML = "pincode must be in digit";
        return 0;
    }
    else {
        document.querySelector("#" + containerId + ' .pinspan').innerHTML = "";
        return 1;
    }
}

function phonevalidation(formmode:string,containerId: string) {
    var container=document.querySelector("#" +containerId)
    
    var phone = (document.querySelector("#" + containerId + ' .number') as HTMLInputElement).value;
    if(formmode=="create"&&phone=="")
  
    {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
        return 1;
    }
    if (container.getAttribute("aria-required")!="true" &&(phone=="")) {
        return 1;
    }
   
    else if (!phone.match(/^\d{10}$/)) {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "Enter the 10 valid digits";
        return 0;
    } 
    
        else if(phone.length==10)
        {
             document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
            return countrycodevalidation(containerId)*areacodevalidation(containerId);

        }
     else {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
        return 1;}
}

function countrycodevalidation(containerId: string) {
    
    var countrycodes = (document.querySelector("#" + containerId + ' .countryCode') as HTMLInputElement).value;
    var phone = (document.querySelector("#" + containerId + ' .number') as HTMLInputElement).value;
    if (phone=="") {
        return 1;
    }
    else if(countrycodes=="")
    {
        document.querySelector("#" + containerId + ' .countrycodespan').innerHTML = "please select the country code";
        return 0;
    }
    else {
        document.querySelector("#" + containerId + ' .countrycodespan').innerHTML = "";
        return 1;}
}

// function phones1c(containerId: string) {
//     var Contact1 = (document.querySelector("#" + containerId + ' .contactNumberType') as HTMLInputElement).value;
//     if (Contact1 == "") {
//         document.querySelector("#" + containerId + ' .typespan').innerHTML = "Select Type";
//         return 0; }
//        else {document.querySelector("#" + containerId + ' .typespan').innerHTML = "";
//         return 1;
//       }
// }
export function connectivityvalidation()
{
    var connectivitys=(document.getElementById('Connectivity') as HTMLInputElement).value;
    if(connectivitys=="")
    {
        (document.getElementById('connectivitys') as HTMLInputElement).innerHTML = "please fill this field";
        
        return 0;

    }
    else{
        (document.getElementById('connectivitys') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
export function descriptionboxvalidation() {
    var description = (document.getElementById('comment') as HTMLInputElement).value;
    if (description == "") {
        (document.getElementById('description') as HTMLInputElement).innerHTML = "Fill Details";
        return false;
    }
    else {
        (document.getElementById('description') as HTMLInputElement).innerHTML = "";
        return true;
    }
}
