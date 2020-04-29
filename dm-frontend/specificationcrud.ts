import { SpecificationList } from "./specificationlist";
import { BASEURL, navigationBarsss, PageNo, current_page,paging, amIUser} from "./globals";
let mode:string = "create";
(async function(){
    let token=JSON.parse(sessionStorage.getItem("user_info"))["token"];
    const role = (await amIUser(token)) == true ? "User" :"Admin";
    let currentPage:number=current_page;
    class GetSpecification {
        specification_id:number;
        RAM:string;
        storage:string;
        screenSize:string;
        connectivity:string;
    
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
            const data = new GetSpecification();
            data.RAM = (document.getElementById("RAM")as HTMLInputElement).value;
            data.storage = (document.getElementById("Storage")as HTMLInputElement).value;
            data.screenSize = (document.getElementById("Screen_size")as HTMLInputElement).value;
            data.connectivity = (document.getElementById("Connectivity")as HTMLInputElement).value;
            return JSON.stringify(data);
        }
        addNewSpecification()
        {
        
            let data1=this.addDataToSpecification();
            console.log(data1);
            fetch(BASEURL +"/api/Device/addspecification", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: data1,
            })
            .catch(Error => console.log(Error));

        }
        updateSpecification(specification_id:number)
        {
            
            let data1 = this.addDataToSpecification();
            fetch(BASEURL +"/api/Device/updatespecification/"+ specification_id, {
                method: "PUT",
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: data1,
            })
            .catch(Error => console.log(Error));

        }
        fillSpecification(specification_id:number)
        {
            fetch(
                BASEURL +"/api/Device/spec/"+specification_id,{
                    headers: new Headers({"Authorization": `Bearer ${token}`})
                }
            )
                .then(response => response.json())
                .then(data => {
                    console.log(data);

                    this.getDataToForm(data);
                    
                })
                .catch(err => console.log(err));

        }
        getDataToForm(data:any)
        {

            (document.getElementById("RAM")as HTMLInputElement).value  =  data.result[0].ram;
            (document.getElementById("Connectivity")as HTMLInputElement).value = data.result[0].connectivity;
            (document.getElementById("Storage")as HTMLInputElement).value  = data.result[0].storage;
            (document.getElementById("Screen_size")as HTMLInputElement).value  = data.result[0].screenSize;
            
        }
        openForm() {
            (document.getElementById("popupForm") as HTMLFormElement).style.display = "block";
        }
        closeForm() {
            (document.getElementById("popupForm") as HTMLFormElement).style.display = "none";
        }
    
    }
    (document.querySelector('#popup_specification')as HTMLFormElement).addEventListener('submit', function (e) {
        console.log("inside function")
        e.preventDefault();
    
        if(mode === "edit")
        {
            
            specs.updateSpecification(specs.specification_id);
            mode = "create";
        }
        else{
        specs.addNewSpecification();
        }
        specs.closeForm();
        specs.getSpecificationData();
        
    });

    (document.querySelector("#pagination") as HTMLButtonElement).addEventListener("click" ,e =>
	{ 
		if((e.target as HTMLButtonElement).value==">>")
		{
			currentPage+=1;
		}
		else if((e.target as HTMLButtonElement).value=="<<")
		{
			currentPage-=1;
		}
		else
		{
			currentPage=+((e.target as HTMLButtonElement).value);
		}
	       console.log((e.target as HTMLButtonElement).value);
           specs.getSpecificationData();  
    });

    document.addEventListener("click", function (e) {
        if ((e.target as HTMLButtonElement).className == "edit-button") {
            const specification_id: any = (e.target as HTMLButtonElement).getAttribute('value');
            specs.specification_id = specification_id;
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

    const specs = new GetSpecification();
    specs.getSpecificationData();
    navigationBarsss(role,"navigation");
})();