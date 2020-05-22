import { SpecificationList } from "./specificationlist";
import { BASEURL, navigationBarsss, PageNo, current_page,paging, changePage, amIUser,headersRows,Token} from "./globals";

let mode:string = "create";
(async function(){
    const _ = Token.getInstance();
    const token = _.tokenKey;
    const role = (await amIUser(token)) == true ? "User" :"Admin";
    let currentPage:number=current_page;
    class GetSpecification {
        getSpecificationData() {
            fetch(
                BASEURL +"/api/Device/specification?"+PageNo(currentPage),{
                    headers: new Headers({"Authorization": `Bearer ${token}`})
                })
                .then(response =>{
					let metadata=JSON.parse(response.headers.get('X-Pagination'));
					paging(metadata);
					return response.json()
				})
                .then(data => {
                    console.log(data);
                    (document.getElementById("specification_data") as HTMLTableElement).innerHTML = "";
                    for (let i = 0; i < data.length; i++) {
                        let res = new SpecificationList(data[i],token);
                        res.getSpecificationList();
                    }
                })
                .catch(err => console.log(err));

        }
        addDataToSpecification()
        {
            
            console.log("type");
            const data = new SpecificationList("",token);
            data.RAM = (document.getElementById("RAM")as HTMLInputElement).value;
            data.storage = (document.getElementById("Storage")as HTMLInputElement).value;
            data.screenSize = (document.getElementById("Screen_size")as HTMLInputElement).value;
            data.connectivity = (document.getElementById("Connectivity")as HTMLInputElement).value;
            return JSON.stringify(data);
        }
        async addNewSpecification()
        {
        
            let data1=this.addDataToSpecification();
            console.log(data1);
            let res = await fetch(BASEURL +"/api/Device/addspecification", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: data1,
            })
            return res.status;

        }
        async updateSpecification(specification_id:number)
        {
            
            let data1 = this.addDataToSpecification();
            console.log(data1);
           let res= await fetch(BASEURL +"/api/Device/updatespecification/"+ specification_id, {
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: data1,
            });
           return res.status;

        }
        fillSpecification(specification_id:number)
        {
            console.log(specification_id);
            fetch(
                BASEURL +"/api/Device/spec/"+specification_id,{
                    headers: new Headers({"Authorization": `Bearer ${token}`})
                }
            )
                .then(response => response.json())
                .then(data => {
                    console.log(data);

                    this.getDataToForm(data[0]);
                    
                })
                ;

        }
        getDataToForm(data:any)
        {

            (document.getElementById("RAM")as HTMLInputElement).value  =  data.ram;
            (document.getElementById("Connectivity")as HTMLInputElement).value = data.connectivity;
            (document.getElementById("Storage")as HTMLInputElement).value  = data.storage;
            (document.getElementById("Screen_size")as HTMLInputElement).value  = data.screenSize;
            
        }
         openForm(popup) {
            document.querySelector(popup).classList.add("active");
        }
        closeForm(popup) {
            document.querySelector(popup).querySelectorAll('input,select').forEach((element) => {
                element.value = '';
            });
            document.querySelector(popup).classList.remove("active");
        }
               deleteSpecification(specification_id:number)
        {
            fetch(BASEURL + "/api/Device/specification/" + specification_id +"/delete", {
                method: "DELETE",
                headers: new Headers({"Authorization": `Bearer ${token}`})
			})
            .then(response => {
                if(response.status == 204){
                    window["tata"].text('Specification cannot ','be deleted!',{duration:3000}); 
                }
                else if(response.status == 200)
                window["tata"].text('Specification ','Deleted!',{duration:3000});
                this.getSpecificationData();
            })
            .catch(ex => {
    
                window["tata"].error('An error occured ',' '+ex.message,{duration:3000});
            });


        }
    }
    (document.querySelector('#popup_specification')as HTMLFormElement).addEventListener('submit',async  function (e) {
        console.log("inside function")
        e.preventDefault();
        let response;
        if(mode === "edit")
        {
            
           response = await specs.updateSpecification(specification.specification_id);
            mode = "create";
            if(response == 200)
            window["tata"].text('Specification ','Updated!',{duration:3000});
        }
        else{
         
             response =await specs.addNewSpecification();
             if(response == 200)
             window["tata"].success('New Specification ','Added!',{duration:3000});
        }
        specs.closeForm('.login-popup');
        if(response == 200){
        specs.getSpecificationData();
        }
    });

    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{   currentPage=changePage((e.target as HTMLButtonElement).value);
        specs.getSpecificationData();  
    });

    document.addEventListener("click", function (e) {
        if ((e.target as HTMLButtonElement).id == "edit-button") {
            const specification_id: any = (e.target as HTMLButtonElement).getAttribute('value');
            specification.specification_id = specification_id;
            specs.openForm('.login-popup');
            mode = "edit";
        specs.fillSpecification(specification_id);
    
        }
        if((e.target as HTMLButtonElement).id=="add-specification")
        {
            (document.getElementById("RAM")as HTMLInputElement).value  = "";
            (document.getElementById("Connectivity")as HTMLInputElement).value = "";
            (document.getElementById("Storage")as HTMLInputElement).value  = "";
            (document.getElementById("Screen_size")as HTMLInputElement).value  = "";
            specs.openForm('.login-popup');
        }
        if((e.target as HTMLButtonElement).id=="delete-button")
        {
            if(confirm("Are you sure you want to delete this specification?")){
            const specification_id: any = (e.target as HTMLButtonElement).getAttribute(
                "value"
            );
             specs.deleteSpecification(specification_id);
            }
        }
    });
    const specification = new SpecificationList("",token);
    const specs = new GetSpecification();
    specs.getSpecificationData();
    navigationBarsss(role,"navigation");
    headersRows(role,"row1");
})();