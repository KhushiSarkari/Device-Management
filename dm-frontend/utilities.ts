export function openForm() {
	// document.getElementsByClassName("RegisterForm")[0].classList.add("active");
	document.querySelector("#myForm").style.display="block";
}
​
export function closeForm() {
	// document.getElementsByClassName("RegisterForm")[0].classList.remove("active");
    document.querySelector("#myForm").style.display="none";
    document.querySelector("#myForm").reset();
}
