const url = new URL(window.location.href);
let id = url.searchParams.get("id");
document.querySelector("#snp").addEventListener("click",  (e) => 
{
    let password = (window.document.getElementById("newpassword") as HTMLInputElement ).value;
  console.log(password);
    fetch("http://localhost:5000/api/auth/Reset/setpassword", { 
        method: "POST", 
        body: JSON.stringify({ 
            Guid: id,
            Password :  password
            }), 
        headers: { 
            "Content-type": "application/json; charset=UTF-8"
        } 
    })
    .then(res=>res.json())
    .then(data=>
        {console.log(data.result);
         if(data.result==="Done")
         {
             alert("Password Updated Successfully");
             window.close();
         }
         else{
             console.log("error ");
         }



        }).catch(err=>{console.log(err)});
   
});
 

