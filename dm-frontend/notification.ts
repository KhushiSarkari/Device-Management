

export class Notifications {
    notification_id: number;
    device_id: number;
    user_id:number;
    type: string;
    brand: string;
    model: string;
    ram: string;
    storage: string;
    screen_size: string;
    connectivity: string;
    notificationDate:string;
    status:string;
    message:string;
    token:string="";
    constructor(data: any,token:string) {
        this.notification_id = data.notificationId;
        this.user_id = data.userId;
        this.device_id = data.device.deviceId;
        this.type = data.device.deviceType.type;
        this.brand = data.device.deviceBrand.brand;
        this.model = data.device.deviceModel.model;
        this.ram = data.device.specification.ram;
        this.storage = data.device.specification.storage;
        this.screen_size = data.device.specification.screenSize;
        this.connectivity = data.device.specification.connectivity;
        this.notificationDate = data.notificationDate;
        this.status = data.statusname.statusName;
        this.message = data.message;
        this.token =token;
    }
    getNotificationTable() {
        const value = `
           
        <tr>
            <td>${this.type} ${this.brand} ${this.model}</td>
            <td>RAM:${this.ram} Storage:${this.storage}
                 <br>
                 Screen Size:${this.screen_size} Connectivity: ${this.connectivity}
            </td>
            <td>${this.notificationDate.toString()}</td>
            <td>${this.status}</td>
              <td>${this.message}</td>`;
              if(this.status=="Pending"){         
                    var buttons= ` <td>
                    <button class="accept-button" data-userid = ${this.user_id} data-value=${this.device_id}  data-device=${this.type +" " +this.brand +" " + this.model}>Accept </button>
                    <button class="reject-button" data-notificationid = ${this.notification_id}  data-device=${this.type +" " +this.brand +" " + this.model}>Reject </button>
            </td>
        </tr>`;
              }
              else{
                  buttons="";
              }
        (document.getElementById("notification_data") as HTMLStyleElement).innerHTML += value+buttons;
              
    }

}