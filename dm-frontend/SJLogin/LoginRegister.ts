import { validationRegister, validationLogin } from "./loginvalidation";
import { BASEURL } from "../globals";
import {clearSpan} from "../utilities";

function LoginUser() {
	let useremail = (document.getElementById("useremail") as HTMLInputElement)
		.value;
	let userpassword = (document.getElementById(
		"userpassword"
	) as HTMLInputElement).value;

	fetch(BASEURL + "/api/auth/login", {
		method: "POST",
		body: JSON.stringify({
			email: useremail,
			password: userpassword
		}),
		headers: {
			"Content-type": "application/json; charset=UTF-8"
		}
	})
		.then(response => {
			console.log(response);
			window.location.href = response.url;
		})
		.catch(err => {
			console.log(err);
		});
}
function RegisterUser() {
		let remail = (window.document.getElementById("remail") as HTMLInputElement)
		.value;
		let fname = (window.document.getElementById("fname") as HTMLInputElement)
		.value;
		let lname = (window.document.getElementById("lname") as HTMLInputElement)
		.value;
	let rpassword = (window.document.getElementById(
		"rpassword"
	) as HTMLInputElement).value;
	console.log(remail + "  " + rpassword);
	fetch(BASEURL + "/api/auth/register", {
		method: "POST",
		body: JSON.stringify({
			
				FirstName:fname,
				LastName:lname,
				Email: remail,
			    Password: rpassword
		}),
		headers: {
			"Content-type": "application/json; charset=UTF-8"
		}
	})
		.then(response => response.json())
		.then(data=>{
            if(data.result !== "AlreadyExists")
			{
				alert("Registration Successfull !");
			}
			else{
				alert("This Email Already Exists !");
			}

		})
		.catch(err => {
			console.log(err);
		});
}

document.querySelector("#mybtn").addEventListener("click", function(e) {
	e.preventDefault();
	if (validationLogin() == true) {
		LoginUser();
	} else {
		return false;
	}
});
document.querySelector("#rmybtn").addEventListener("click", function(e) {
	e.preventDefault();
	if (validationRegister() == true) {
		RegisterUser();
	} else {
		return false;
	}
});

document.querySelector("#fp").addEventListener("click", function(e) {
	e.preventDefault();
	(document.querySelector(".bg-model") as HTMLDivElement).style.display =
		"flex";
});
document.querySelector(".x").addEventListener("click", function() {
	(document.querySelector(".bg-model") as HTMLDivElement).style.display =
		"none";
});
document.querySelector("#rmybtn1").addEventListener('click', function (e) {
    e.preventDefault();
    SendEmail();
});

function SendEmail()
{
	let useremail = (document.getElementById("femail") as HTMLInputElement).value;
	console.log(useremail);
    fetch(BASEURL + "/api/auth/Reset", {
        method: "POST",
        body: JSON.stringify({
            Email: useremail
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    })
        .catch(err => {
            console.log(err);
        });
}

let password = document.querySelector('input[type="password"]');

password.addEventListener("focus", a => {
	document.getElementById("userpasswords").innerHTML = "";
});

let email = document.querySelector('input[type="text"]');

email.addEventListener("focus", b => {
	document.getElementById("useremails").innerHTML = "";
});

let remail = document.querySelector('input[type="text"]');

remail.addEventListener("focus", c => {
	document.getElementById("emails").innerHTML = "";
});

let rpassword = document.querySelector('input[type="password"]');

rpassword.addEventListener("focus", d => {
	document.getElementById("passwords").innerHTML = "";
	document.getElementById("confirmpasss").innerHTML = "";
});

let hideregisters = document.getElementById('hideregister');
hideregisters.addEventListener("click",e=>{
	//location.reload();
	clearSpan(document.getElementById("abc")as HTMLFormElement);
	document.getElementById("abc").style.display = "none";

});

let registercards = document.getElementById('registercard');

registercards.addEventListener("click",f=>{
	clearSpan(document.getElementById("logincard")as HTMLFormElement);
	document.getElementById("abc").style.display = "flex";
});