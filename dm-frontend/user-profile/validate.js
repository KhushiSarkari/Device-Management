(function (factory) {
    if (typeof module === "object" && typeof module.exports === "object") {
        var v = factory(require, exports);
        if (v !== undefined) module.exports = v;
    }
    else if (typeof define === "function" && define.amd) {
        define(["require", "exports"], factory);
    }
})(function (require, exports) {
    "use strict";
    exports.__esModule = true;
    function validate() {
        return Firstnamevalidation() &&
            Lastnamevalidation() &&
            emailvalidation() && Middlenamevalidation() && passwordvalidation() && addressvalidation("addresses1") && addressvalidation("addresses2") &&
            addressvalidation1("addresses1") && addressvalidation1("addresses2") && date0fbirthvalidation() &&
            pinvalidation("addresses1") && pinvalidation("addresses2") && phonevalidation("phones1") &&
            phonevalidation("phones3");
    }
    exports.validate = validate;
    function Firstnamevalidation() {
        var firstnames = document.getElementById('firstName').value;
        if (firstnames == "") {
            document.getElementById('firstnames').innerHTML = "Fill First Name";
            return false;
        }
        else if (firstnames.length < 5 || firstnames.length > 20) {
            document.getElementById('firstnames').innerHTML = "FirstName Should Be Between 2 and 20";
            return false;
        }
        else {
            document.getElementById('firstnames').innerHTML = "";
            return true;
        }
    }
    function Middlenamevalidation() {
        var middlenames = document.getElementById('middleName').value;
        if (middlenames.length < 5 || middlenames.length > 20) {
            document.getElementById('middlenames').innerHTML = "MiddleName Should Be Between 2 and 20";
            return false;
        }
        else {
            document.getElementById('middlenames').innerHTML = "";
            return true;
        }
    }
    function Lastnamevalidation() {
        var lastnames = document.getElementById('lastName').value;
        if (lastnames == "") {
            document.getElementById('lastnames').innerHTML = "Fill Last Name";
            return false;
        }
        else if (lastnames.length < 5 || lastnames.length > 20) {
            document.getElementById('lastnames').innerHTML = "LastName Should Be Between 3 and 20";
            return false;
        }
        else {
            document.getElementById('lastnames').innerHTML = "";
            return true;
        }
    }
    function emailvalidation() {
        var emails = document.getElementById('email').value;
        if (emails == "") {
            document.getElementById('emails').innerHTML = "Fill Email";
            return false;
        }
        else if (emails.indexOf('@') <= 0) {
            document.getElementById('emails').innerHTML = "@ Is At Invalid position";
            return false;
        }
        else if ((emails.charAt(emails.length - 4) != '.') && (emails.charAt(emails.length - 3) != '.')) {
            document.getElementById('emails').innerHTML = ". Is At Invalid position";
            return false;
        }
        else {
            document.getElementById('emails').innerHTML = "";
            return true;
        }
    }
    function date0fbirthvalidation() {
        var dobs = document.getElementById('dob').value;
        if (dobs == "") {
            document.getElementById('dobs').innerHTML = "Fill date of birth";
            return false;
        }
        else {
            document.getElementById('emails').innerHTML = "";
            return true;
        }
    }
    function passwordvalidation() {
        var passwords = document.getElementById('password').value;
        if (passwords == "") {
            return true;
        }
        else if (passwords.length < 5 || passwords.length > 20) {
            document.getElementById('passwords').innerHTML = "Password Should Be Between 5 and 20";
            return false;
        }
        else {
            document.getElementById('passwords').innerHTML = "";
            return true;
        }
    }
    function addressvalidation(containerId) {
        var addresses1 = document.querySelector("#" + containerId + ' .addressLine1').value;
        if (addresses1 == "") {
            document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "Fill  addresss line1";
            return false;
        }
        else if (addresses1.length < 5 || addresses1.length > 30) {
            document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "addressline1 Should Be Between 5 and 30";
            return false;
        }
        else {
            document.querySelector("#" + containerId + ' .addressLine1span').innerHTML = "";
            return true;
        }
    }
    function addressvalidation1(containerId) {
        var addresses2 = document.querySelector("#" + containerId + ' .addressLine2').value;
        if (addresses2.length < 5 || addresses2.length > 30) {
            document.querySelector("#" + containerId + ' .addressLine2span').innerHTML = "addressline2 Should Be Between 5 and 30";
            return false;
        }
        else {
            document.querySelector("#" + containerId + ' .addressLine2span').innerHTML = "";
            return true;
        }
    }
    function pinvalidation(containerId) {
        var pin = document.querySelector("#" + containerId + ' .pin').value;
        if (isNaN(pin)) {
            document.querySelector("#" + containerId + ' .pinspan').innerHTML = "pincode must be in digit";
            return false;
        }
        else {
            document.querySelector("#" + containerId + ' .pinspan').innerHTML = "";
            return true;
        }
    }
    function phonevalidation(containerId) {
        var phone = document.querySelector("#" + containerId + ' .number').value;
        if ((phone == "")) {
            return true;
        }
        else if (isNaN(phone)) {
            document.querySelector("#" + containerId + ' .numberspan').innerHTML = "**Only Digits Allowed";
            return false;
        }
        else if (phone.length != 10) {
            document.querySelector("#" + containerId + ' .numberspan').innerHTML = "Enter 10 Digits";
            return false;
        }
        else {
            document.querySelector("#" + containerId + ' .numberspan').innerHTML = "";
            return true;
        }
    }
});
