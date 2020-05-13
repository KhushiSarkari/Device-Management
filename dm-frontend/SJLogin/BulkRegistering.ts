 import { BASEURL } from "../globals";

interface HTMLInputEvent extends Event {
    target: HTMLInputElement & EventTarget;
}


document.getElementById("upload").addEventListener("change",function (e:HTMLInputEvent) {
     console.log(e.target.files);
let photo =e.target.files[0];
let formData = new FormData();

formData.append("photo", photo);
fetch(BASEURL + '/api/BulkRegister/UploadFiles', {method: "POST", body: formData}).then(Response=>Response.json()).then(data=>console.log(data));
});