import { RequestSubmitModel } from "./SubmissionRequestModel";

export class populateData
{

    constructor(value : string)
    {
        this.clearData(value)
    }
    generateField(data : RequestSubmitModel , elementId : string )
    {
        const value = `<tr>
        <td> ${data.userName} </td>
        <td class="requestedUserData"> ${data.deviceType}
       
        </td>
        <td>  ${data.deviceName}</td>
        <td>${data.specification} </td>
        <td>${data.ReturnDate}</td>
        <td>
        <button class="accept" data-user-id=${data.userId} data-return-id=${data.returnRequestId} > submitt </button>
         </td>`;
         this.addData(value , elementId)
    }
    addData(data : string , elementId : string)
    {
       
        (document.getElementById(elementId) as HTMLTableElement).innerHTML += data;
    }

    clearData(value : string)
    {
       (document.getElementById(value) as HTMLTableElement).innerHTML = "";
     }
}