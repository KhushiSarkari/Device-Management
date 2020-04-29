export function validate() {
    return Firstnamevalidation() 
    * Middlenamevalidation() 
    *Lastnamevalidation() *
    passwordvalidation()*
    confirmpasswordvalidation()*
    salutationvalidation()*
    gendervalidation()*
    date0fbirthvalidation()*
    addressvalidation("addresses1")*
    addressvalidation("addresses2")*
    phonevalidation("phones1")*
    phonevalidation("phones2")*
    phonevalidation("phones3");
}
function Firstnamevalidation() {
    var firstnames = (document.getElementById('firstName') as HTMLInputElement).value;
    if (firstnames == "") {
        document.getElementById('firstnames').innerHTML = "Fill First Name";
        return 0;
    } else if (firstnames.length > 20) {
        document.getElementById('firstnames').innerHTML = "FirstName Should Not Be More Than 20 Characters";
        return 0;
    }
    else {
        document.getElementById('firstnames').innerHTML = "";
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
function Lastnamevalidation() {
    var lastnames = (document.getElementById('lastName') as HTMLInputElement).value;
    if (lastnames == "") {
        document.getElementById('lastnames').innerHTML = "Fill Last Name";
        return 0;
    } else if (lastnames.length > 20) {
        document.getElementById('lastnames').innerHTML = "LastName Should Not Be More Than 20 Characters";
        return 0;
    }
    else {
        document.getElementById('lastnames').innerHTML = "";
        return 1;
    }
}
function salutationvalidation() {
    var salutations = (document.getElementById('salutation') as HTMLInputElement).value;
    if (salutations == "") {
        document.getElementById('salutations').innerHTML = "Please Select The Salutaion";
        return 0;
    }
    else {
        document.getElementById('salutations').innerHTML = "";
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
function passwordvalidation() {
    var passwords = (document.getElementById('password') as HTMLInputElement).value;
    if(passwords=="")
    {
        return 1;
    }
    else if (passwords.length < 5 || passwords.length > 20) {
        document.getElementById('passwords').innerHTML = "Password Should Be Between 5 and 20";
        return 0;
    } else {
        document.getElementById('passwords').innerHTML = "";
        return 1; }
}
function confirmpasswordvalidation() {

    var passwords = (document.getElementById('password') as HTMLInputElement).value;

    var confirmpasswords = (document.getElementById('confirmpassword') as HTMLInputElement).value;
    if(passwords=="")
    {
        return 1;
    }
   
    if (confirmpasswords == "") {
        (document.getElementById('confirmpasswords') as HTMLInputElement).innerHTML = "Fill Confirm Password";
        return 1;
    } else if (confirmpasswords.length < 5 || confirmpasswords.length > 20) {
        (document.getElementById('confirmpasswords') as HTMLInputElement).innerHTML = "Confirm Password Should Be Between 5 and 20";
        return 0;
    } else if (passwords != confirmpasswords) {
        (document.getElementById('confirmpasswords') as HTMLInputElement).innerHTML = "Passwords Dont Match";
        return 0;
    } else {
        (document.getElementById('confirmpasswords') as HTMLInputElement).innerHTML = "";
        return 1;
    }
}
function addressvalidation(containerId: string) {
    var addresses1 = (document.querySelector("#" + containerId + ' .addressLine1') as HTMLInputElement).value;
    if (addresses1 == "") {
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
function phonevalidation(containerId: string) {
    var container=document.querySelector("#" +containerId)
    
    var phone = (document.querySelector("#" + containerId + ' .number') as HTMLInputElement).value;
    if (container.getAttribute("aria-required")!="true" &&(phone=="")) {
        return 1;
    }
    else if(phone=="")
    {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "Enter the Number";
        return 0;
    }
    else if (!phone.match(/^\d{10}$/)) {
        document.querySelector("#" + containerId + ' .numberspan').innerHTML = "**Enter the 10 valid digits";
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