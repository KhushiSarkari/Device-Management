import { RequestModel } from "./RequetsDatamodel";
import { GeneratePaging } from "./pagination";

export class PopulateData {
    historyInformation: RequestModel
    fillData(data: JSON) {
        this.clearData();
        // var pages = new GeneratePaging();
        // pages.generatePage(data["resultCount"]);
        console.log(data);

        var value: any;
        this.historyInformation = new RequestModel();
        for (value of data) {
            this.historyInformation.email = value["userMail"];
            console.log(this.historyInformation.email);
            this.historyInformation.deviceId = value["deviceId"];
            this.historyInformation.serialNumber = value["serial_number"];
            this.historyInformation.deviceName = value["deviceBrand"] + " " + value["deviceModel"];
            this.historyInformation.deviceType = value["deviceType"];
            this.historyInformation.requestStatus = value["requestStatus"];
            this.bindSpecs(value);
            this.historyInformation.userName = this.userName(value, "requestedUser");
            this.historyInformation.adminName = this.userName(value, "deviceSubmittedAdmin");

            this.historyInformation.requestDate = value["requestDate"];
            this.historyInformation.assignDate = value["assignedDate"];
            this.historyInformation.assignDays = value["assignDays"] == -1 ? 0 : value["assignDays"];
            this.historyInformation.returnDate = value["returnDate"];
            this.genearteFields(this.historyInformation);

        }


    }
    private bindSpecs(value: any) {
        this.historyInformation.specifications = (value["specs"]["ram"] == ""?"": "RAM = "+ value["specs"]["ram"] +",") + 
        (value["specs"]["storage"]== "" ? "" : "Storage =  " + value["specs"]["storage"] +"," ) +
        (value["specs"]["screen_size"] =="" ? "" : " Screen-Size = " + value["specs"]["screen_size"] +",") +
         ( value["specs"]["connectivity"] == "" ? "" : " connectivity =  " + value["specs"]["connectivity"] );
    }

    private userName(value: any, type: string): string {
        return (value[type]["salutation"] + " " + value[type]["firstName"] + " "
            + value[type]["middleName"] + " " + value[type]["lastname"]);
    }

    public genearteFields(historyInformation: RequestModel) {
        const value = `<tr>
        <td class="requestedUserData"> ${historyInformation.userName}
        <span class="tooltipData">E-mail = ${historyInformation.email}</span>
        </td>
        
        <td>${historyInformation.serialNumber} </td>
        <td>${historyInformation.deviceType}</td>
        <td class="requestedUserData"> ${historyInformation.deviceName}
        <span class="tooltipData">Device Specs ${historyInformation.specifications} </spam>
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