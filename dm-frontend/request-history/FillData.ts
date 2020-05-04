import { RequestModel } from "./RequetsDatamodel";
import { HtmlElementsData } from "./HtmlElementsId";
// import { populateData } from "./genrateSubmissionRequest";

export class PopulateData {
    historyInformation: RequestModel
    domElement : HtmlElementsData
    fillData(data: any) {
        this.clearData();
        data.map(value=> {
            let obj = new  RequestModel();
            obj.populateData(value);
            new PopulateData().genearteFields(obj);
        });
        }
   

    public genearteFields(historyInformation: RequestModel) {
        const value = `<tr>
        <td class="requestedUserData"> ${historyInformation.userName}
        <span class="tooltipData">E-mail = ${historyInformation.email}</span>
        </td>
        
        <td>${historyInformation.serialNumber} </td>
        <td>${historyInformation.deviceType}</td>
        <td class="requestedUserData"> ${historyInformation.deviceName}
        <span class="tooltipData">Device Specs ${historyInformation.specifications} </span>
         </td>
        
        <td class="requestedUserData">${historyInformation.requestStatus} 
        <span class="tooltipData">Assigned for ${historyInformation.assignDays} days</span>
        </td>
        <td>${historyInformation.assignDate} </td>
        <td>${historyInformation.returnDate} </td>
        <td>${historyInformation.adminName} </td>
        </tr>`;
       
        this.newMethod(value);

    }

    private newMethod(value: string) {
        document.getElementById("Request_data").innerHTML += value;
    }

    public clearData() {
        document.getElementById("Request_data").innerHTML = "";
    }
}