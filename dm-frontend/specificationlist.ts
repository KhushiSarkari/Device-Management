


export class SpecificationList
{
    specification_id:number;
    RAM:string;
    storage:string;
    screenSize:string;
    connectivity:string;
    token:string ="";
    constructor(data:any,token:string)
    {
       this.specification_id = data.specification_id;
        this.RAM = data.ram;
        this.storage = data.storage;
        this.screenSize = data.screenSize;
        this.connectivity = data.connectivity;
        this.token=token;
    }   
    
    getSpecificationList()
    {
        const value = `<tr>
        <td>${this.specification_id}</td>
        <td>${this.RAM} </td>
        <td>${this.storage} </td>
        <td>${this.screenSize}</td>
        <td>${this.connectivity}</td>
        <td> <div class="tooltip"><span class="material-icons" id="edit-button" value=${this.specification_id}>create
        </span></div>
        <div class="tooltip"><span class="material-icons" id="delete-button" value=${this.specification_id}>delete
        </span></div>
        </tr>`;
        // <button class="delete-button" value=${this.specification_id}>Delete </button>

        (document.getElementById("specification_data")as HTMLTableElement).innerHTML += value

    }
   
}
