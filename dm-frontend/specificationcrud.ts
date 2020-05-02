import { SpecificationList } from "./specificationlist";
import { BASEURL, navigationBarsss, PageNo, current_page,paging, changePage, amIUser} from "./globals";
let mode:string = "create";
(async function(){
    let token=JSON.parse(sessionStorage.getItem("user_info"))["token"];
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
        openForm() {
            (document.getElementById("popupForm") as HTMLFormElement).style.display = "block";
        }
        closeForm() {
            (document.getElementById("popupForm") as HTMLFormElement).style.display = "none";
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
        }
        else{
             response =await specs.addNewSpecification();
        }
        specs.closeForm();
        if(response ==200){
        specs.getSpecificationData();
        }
    });

    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{   currentPage=changePage((e.target as HTMLButtonElement).value);
        specs.getSpecificationData();  
    });

    document.addEventListener("click", function (e) {
        if ((e.target as HTMLButtonElement).className == "edit-button") {
            const specification_id: any = (e.target as HTMLButtonElement).getAttribute('value');
            specification.specification_id = specification_id;
            specs.openForm();
            mode = "edit";
        specs.fillSpecification(specification_id);
    
        }
        if((e.target as HTMLButtonElement).id=="add-specification")
        {
            (document.getElementById("RAM")as HTMLInputElement).value  = "";
            (document.getElementById("Connectivity")as HTMLInputElement).value = "";
            (document.getElementById("Storage")as HTMLInputElement).value  = "";
            (document.getElementById("Screen_size")as HTMLInputElement).value  = "";
            specs.openForm();
        }
    });
    const specification = new SpecificationList("",token);
    const specs = new GetSpecification();
    specs.getSpecificationData();
    navigationBarsss(role,"navigation");
})();