
import { SpecificationList } from "./specificationlist";


export class DeviceListForAdmin {
token :string="";
    device_id: number;
        type: string;
        brand: string;
        model: string;
        color: string;
        price: string;
        serial_number: string;
        warranty_year: string;
        purchase_date: string;
        status: string;
        comments :  string;
        specs :SpecificationList;
        assign_date: string;
        return_date: string;
        assign_to_first_name: string;
        assign_to_middle_name: string;
        assign_to_last_name: string;

        assign_by_first_name: string;
        assign_by_middle_name: string;
        assign_by_last_name: string;
        constructor(data: any,token:string) {
            this.token =token;
            this.device_id = data.device_id;
            this.type = data.type;
            this.brand = data.brand;
            this.model = data.model;
            this.color = data.color;
            this.price = data.price;
            this.serial_number = data.serial_number;
            this.warranty_year = data.warranty_year;
            this.purchase_date = data.purchase_date;
            this.status = data.status;
            this.comments = data.comments;
            this.specs=new SpecificationList(data.specifications,token);
            this.assign_date = data.assign_date;
            this.return_date = data.return_date;
            this.assign_to_first_name = data.assign_to.first_name;
            this.assign_to_middle_name = data.assign_to.middle_name;
            this.assign_to_last_name = data.assign_to.last_name;
            this.assign_by_first_name = data.assign_by.first_name;
            this.assign_by_middle_name = data.assign_by.middle_name;
            this.assign_by_last_name = data.assign_by.last_name;

        }
        getDeviceList(token: number) {
            const value = `
            
            <tr>
                <td class = "cards tooltip" data-deviceid="${this.device_id}">${this.type} ${this.brand} ${this.model}
                    <div class="mdl-card tooltiptext">
                        <div class="mdl-card__title">
                            <h2 class="mdl-card__title-text">Device Details</h2>
                        </div>

                        <div class="mdl-card__supporting-text">
                            Device color: ${this.color} <br>
                            Price:${this.price} <br>
                            Warranty Year: ${this.warranty_year}<br>
                            Purchase Date: ${this.purchase_date}
                        </div> 
                    </div> 
                </td>
                <td>${this.serial_number} </td>
                <td>RAM:${this.specs.RAM} Storage:${this.specs.storage}
                    <br>
                    Screen Size:${this.specs.screenSize} Connectivity: ${this.specs.connectivity}
                </td>
                <td>${this.assign_date.substring(0,10)} </td>
                <td>${this.return_date.substring(0,10)} </td>
                <td>${this.assign_to_first_name} ${this.assign_to_middle_name} ${this.assign_to_last_name}</td>
                <td>${this.assign_by_first_name} ${this.assign_by_middle_name} ${this.assign_by_last_name}</td>
                `;

                if(token==1){

                
                const buttons =  `<button class="mdl-button mdl-js-button mdl-button--raised mdl-button--colored" id="add-button" ><span class="material-icons">add</span>
                   Add Device  </button>`;
                        (document.getElementById("buttons") as HTMLStyleElement).innerHTML = buttons;    
                    const editbutton = `<td><span class="material-icons" id="edit-${this.device_id}" value=${this.device_id}>create
                    </span><span class="mdl-tooltip" data-mdl-for="edit-${this.device_id}">Edit Device</span>`;
                        if(this.status=="Allocated")
                        var val = `<span class="material-icons" id="notify-${this.device_id}" data-deviceid=${this.device_id}>
                        notifications</span><span class="mdl-tooltip" data-mdl-for="notify-${this.device_id}">Notify User</span></td> </tr>`;
                        else if(this.status == "Free")
                        {
                            val = `<span class="material-icons" id="delete-${this.device_id}" value="${this.device_id}">delete</span>
                            <span class="mdl-tooltip" data-mdl-for="delete-${this.device_id}">Delete Device</span>
                            <span class="material-icons" id="assign-${this.device_id}" data-id=${this.device_id}>assignment</span>
                           <span class="mdl-tooltip" data-mdl-for="assign-${this.device_id}">Assign Device</span>
                          </td> </tr> `;
                        }
                        else
                        {
                            val=`<span class="material-icons" id="delete-${this.device_id}" value=${this.device_id}">delete</span>
                            <span class="mdl-tooltip" data-mdl-for="delete-${this.device_id}">Delete Device</span>
                            <span class="material-icons" style="color: red;" id="report_problem-${this.device_id}">report_problem</span>
                             <span class="mdl-tooltip" data-mdl-for="report_problem-${this.device_id}">Faulty Device</span></td></tr>`;
                        }
                            (document.getElementById("Request_data") as HTMLStyleElement).innerHTML += value +editbutton+ val;
                          
                    }
            else
            {
                    (document.getElementById("Request_data") as HTMLStyleElement).innerHTML += value;
            }
          

        }
        
    }
