import { validationRegister, validationLogin } from "./loginvalidation";
import { BASEURL } from "../globals";
import {clearSpan} from "../utilities";

function LoginUser() {
	let useremail = (document.getElementById("useremail") as HTMLInputElement)
		.value;
	let userpassword = (document.getElementById(
		"userpassword"
	) as HTMLInputElement).value;
	// debugger
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
	registerFetchApi({remail, fname, lname, rpassword});
}
function onSignIn(googleUser) {
	
	var profile = googleUser.getBasicProfile();
	console.log('ID: ' + profile.getId()); // pass
	// console.log('Name: ' + profile.getName());
	// console.log('Name: ' + profile.getName());
	// console.log('Image URL: ' + profile.getImageUrl());
	console.log('Email: ' + profile.getEmail());//email
	console.log('Last name: ' + profile.getFamilyName());//ln
	let ln =  profile.getFamilyName();
	let email =  profile.getEmail();
	let pass =  profile.getId();
	let fn = profile.getGivenName().split(" ")[0];//fn
	let flag  = false;
	fetch(BASEURL + "/api/auth/register", {
		method: "POST",
		body: JSON.stringify({
			
				FirstName:fn,
				LastName:ln,
				Email: email,
			    Password: pass 
		}),
		headers: {
			"Content-type": "application/json; charset=UTF-8"
		}
	})
		.then(response => response.json())
		.then(data=>{
			if(data.result==="AlreadyExists")
			{
				flag=true;
			}
		})
		.catch(err => {
			console.log(err);
		});
		
		fetch(BASEURL + "/api/auth/login", {
		
			method: "POST",
			body: JSON.stringify({
				email: email,
				password: pass
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
			});}
		
	//	}
    // window.location.href="/dashboard.tc.access_token";
//	function RegisterUser() {
	// fetch(BASEURL + "/api/extAuth", {
	// 	headers:{
	// 		'Authorization': 'Bearer '+  googleUser.tc.access_token,
	// 		'Content-Type': 'application/json'
	// 	},
	// 	method: 'POST'
	// }).then(response => response.json())
	// .then(userData => {
	// 	console.log(userData.isNewUser);
	// 	console.log(userData.userId);
	// });

 
window["onSignIn"] = onSignIn;
function registerFetchApi(details){
	fetch(BASEURL + "/api/auth/register", {
		method: "POST",
		body: JSON.stringify({
			
				FirstName:details.fname,
				LastName:details.lname,
				Email: details.remail,
			    Password: details.rpassword  
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
				var resetForm = <HTMLFormElement>document.getElementById("registerForm");
				resetForm.reset();
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
	(document.querySelector(".modal-content") as HTMLDivElement).style.display =
		"flex";
});
document.querySelector(".x").addEventListener("click", function() {
	(document.querySelector(".modal-content") as HTMLDivElement).style.display =
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

const [regCard, loginCard] = Array.from(document.querySelectorAll('.mdl-card'));
let navigateToLogin = document.getElementById('to-login');

navigateToLogin.addEventListener("click",e=>{
	clearSpan(regCard);
	regCard.style.display = 'none';
	loginCard.style.display = 'flex';

});

let navigateToRegister = document.getElementById('to-register');

navigateToRegister.addEventListener("click",e=>{
	clearSpan(loginCard as HTMLFormElement);
	loginCard.style.display = 'none';
	regCard.style.display = 'flex';
});
document.querySelector(".g-signin2").setAttribute('data-onsuccess', 'onSignIn');













