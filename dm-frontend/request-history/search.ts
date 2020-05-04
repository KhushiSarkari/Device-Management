
import { HtmlElementsData } from "./HtmlElementsId";
export class findResult {
    data: any;
    elements : HtmlElementsData;
    token: string;
    constructor(token: string)
    {   
        this.token = token;
        this.elements = new HtmlElementsData();
    }
    public searchUser() {
        let userName=(document.getElementById(this.elements.search) as HTMLInputElement).value;
        let serialNumber = (document.getElementById(this.elements.devicesearch) as HTMLInputElement).value;
        var uri = "?user-name=" + encodeURI(userName) + "&serial-number=" + encodeURI(serialNumber) + "";;
        return uri;
    }

}

