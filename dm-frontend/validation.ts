export function validateForm(formMode:string) {
    
    var result= salutation()*firstNamevalidation()*lastNamevalidation()*emailvalidation()*
    passwordvalidation(formMode)*
    confirmpasswordvalidation(formMode)*roleName()*
    departmentvalidation()*
    designationvalidation()*
    phone1validation("phones1")*phone1validation("phones2")*phone1validation("phones3")*
    currAdd1("addresses1")*currAdd1("addresses2");
return result == 1 ? true : false;
}
export function remove() {
    (document.getElementById('salutations') as HTMLInputElement).innerHTML = "";
    (document.getElementById('firstNames') as HTMLInputElement).innerHTML = "";
    (document.getElementById('lastNames') as HTMLInputElement).innerHTML = "";
    (document.getElementById('emails') as HTMLInputElement).innerHTML = "";
    (document.getElementById('passwords') as HTMLInputElement).innerHTML = "";
    (document.getElementById('confirmpasss') as HTMLInputElement).innerHTML = "";
    (document.getElementById('roleNames') as HTMLInputElement).innerHTML = "";
    (document.getElementById('departments') as HTMLInputElement).innerHTML = "";
    (document.getElementById('designations') as HTMLInputElement).innerHTML = "";
    (document.getElementById('phone_number1s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('CountryCodes1') as HTMLInputElement).innerHTML = "";
    (document.getElementById('Contact1') as HTMLInputElement).innerHTML = "";
    (document.getElementById('ac1s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('c1s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('c2s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('c3s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('c4s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('c5s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('c6s') as HTMLInputElement).innerHTML = "";
    (document.getElementById('addressType1') as HTMLInputElement).innerHTML = "";
    
}
function salutation(){
    var salutations = (document.getElementById('salutation') as HTMLSelectElement).value;
    if (salutations == "") {
        (document.getElementById('salutations') as HTMLInputElement).innerHTML = "Select Salutation";
        return 0;
    } else {
        (document.getElementById('salutations') as HTMLInputElement).innerHTML = "";
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

    var confirmpasss = (document.getElementById('confirmpassword') as HTMLInputElement).value;
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
function emailvalidation() {
    var emails = (document.getElementById('email') as HTMLInputElement).value;
    if (emails == "") {
        (document.getElementById('emails') as HTMLInputElement).innerHTML = "Fill Email";
        return 0;
    }else if (emails.indexOf('@') <= 0) {
        document.getElementById('useremails').innerHTML = "@ Is At Invalid position";
        return 0;
     } else {
            let str = emails;
        str=str.toLowerCase();
        let st =str.split("@");
        if(st[1]!="ex2india.com"){
            document.getElementById('emails').innerHTML = "Not A Valid Domain";
            return 0;
        }
            document.getElementById('emails').innerHTML = "";
                return 1; 
        }
    }

function ac1(containerId: string){
    var ac1s = (document.querySelector("#" + containerId + ' .areaCode') as HTMLInputElement).value;


    if (ac1s == "") {
        document.querySelector("#" + containerId + ' .areaCodeSpan').innerHTML = "Fill Area Code";
        return 0;
    }else if (isNaN(parseInt(ac1s))) {
        document.querySelector("#" + containerId + ' .areaCodeSpan').innerHTML = "Only Digits Allowed";
        return 0;
    } 
     else {
        document.querySelector("#" + containerId + ' .areaCodeSpan').innerHTML = "";
        return 1;
    }
}

function currAddType(containerId: string){
    
    var addressType1 = (document.querySelector("#" + containerId + ' .addressType') as HTMLInputElement).value;
    if (addressType1 == "") {
        document.querySelector("#" + containerId + ' .addressTypeSpan').innerHTML = "Select Type";
        return 0;
    } else {
        document.querySelector("#" + containerId + ' .addressTypeSpan').innerHTML = "";
        return 1;
    }
}
function currAdd1(containerId: string){
    var c1s = (document.querySelector("#" + containerId + ' .addressLine1') as HTMLInputElement).value;
    if (c1s == "") {
        document.querySelector("#" + containerId + ' .addressLine1Span').innerHTML = "";
        return 1;
    } else if(c1s.length < 5) {
        document.querySelector("#" + containerId + ' .addressLine1Span').innerHTML = "Fill Full Details";
        return 0;
    }
    else if(c1s.length > 4) {
        document.querySelector("#" + containerId + ' .addressLine1Span').innerHTML = "";
        return  currAdd2(containerId)*currCon(containerId)*currState(containerId)*currCity(containerId);//*pincode(containerId)*currAddType(containerId);
         
    }
    else {
        document.querySelector("#" + containerId + ' .addressLine1Span').innerHTML = "";
        return 1;
    }
}
function currAdd2(containerId: string){
    var c2s = (document.querySelector("#" + containerId + ' .addressLine2') as HTMLInputElement).value;
    if (c2s == "") {
        document.querySelector("#" + containerId + ' .addressLine2Span').innerHTML= "Fill Address Line 2";
        return 1;
    } else if(c2s.length < 5) {
        document.querySelector("#" + containerId + ' .addressLine2Span').innerHTML= "Fill Full Details";
        return 0;
    }
     else {
        document.querySelector("#" + containerId + ' .addressLine2Span').innerHTML= "";
        return 1;
    }
}
function currCon(containerId: string){
    var c6s = (document.querySelector("#" + containerId + ' .country') as HTMLInputElement).value;
    if (c6s == "") {
        document.querySelector("#" + containerId + ' .countrySpan').innerHTML= "Select Country";
        return 0;
    } else {
        document.querySelector("#" + containerId + ' .countrySpan').innerHTML= "";
        return 1;
    }
}
function currState(containerId: string){
    var c5s = (document.querySelector("#" + containerId + ' .state') as HTMLInputElement).value;
    if (c5s == "") {
        document.querySelector("#" + containerId + ' .stateSpan').innerHTML= "Select Country And Then Select State";
        return 0;
    } else {
        document.querySelector("#" + containerId + ' .stateSpan').innerHTML= "";
        return 1;
    }
}
function currCity(containerId: string){
    var c4s = (document.querySelector("#" + containerId + ' .city') as HTMLInputElement).value;
    if (c4s == "") {
        document.querySelector("#" + containerId + ' .citySpan').innerHTML= "Select State And Then Select City";
        return 0;
    } else {
        document.querySelector("#" + containerId + ' .citySpan').innerHTML= "";
        return 1;
    }
}
function pincode(containerId: string){
    var c3s = (document.querySelector("#" + containerId + ' .pin') as HTMLInputElement).value;
    if (c3s == "") {
        document.querySelector("#" + containerId + ' .pinSpan').innerHTML= "Enter Pincode";
        return 0;
    }else if (isNaN(parseInt(c3s))) {
        document.querySelector("#" + containerId + ' .pinSpan').innerHTML= "Only Digits Allowed";
        return 0;
    } 
    else {
        document.querySelector("#" + containerId + ' .pinSpan').innerHTML= "";
        return 1;
    }
}

function phone1validation(containerId: string) {
    var phone_number1s = (document.querySelector("#" + containerId + ' .number') as HTMLInputElement).value;
   
    if (phone_number1s == "") {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
        return 1;
    } 
    else if (isNaN(parseInt(phone_number1s))) {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "Only Digits Allowed";
        return 0;
    }
    else if (phone_number1s.length != 10) {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "Enter 10 Digits";
        return 0;
    }
    else if(phone_number1s.length == 10){
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
        
        return phones1c(containerId)*phones1cc(containerId);//*ac1(containerId); 
       
    }
     else {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
        return 1;
    }
}

function phones1cc(containerId: string) {
    var CountryCodes1 = (document.querySelector("#" + containerId + ' .countryCode') as HTMLInputElement).value;
    if (CountryCodes1 == "") {
        document.querySelector("#" + containerId + ' .CountryCodeSpan').innerHTML = "Select Country";

        return 0; }
       else {document.querySelector("#" + containerId + ' .CountryCodeSpan').innerHTML  = "";
        return 1;
      }
}

function phones1c(containerId: string) {
    var Contact1 = (document.querySelector("#" + containerId + ' .contactNumberType') as HTMLInputElement).value;
    if (Contact1 == "") {
        document.querySelector("#" + containerId + ' .typespan').innerHTML = "Select Type";
        return 0; }
       else {document.querySelector("#" + containerId + ' .typespan').innerHTML = "";
        return 1;
      }
}
