

export class RequestModel
{
    public userName : string = "" ;
    public email : string = "" ;
    public deviceId : number = 0;
    public serialNumber : string = "";
    public deviceType :string = "";
    public deviceName : string = "";
    public requestStatus : string = "" ;
    public specifications : string = "";
    public requestDate :string = "" ;
    public AssignedAdmminName : string = "" ;
    public assignDate :string = "" ;
    public assignDays : number = 0;
    public returnDate :string = "" ; 
    public adminName  : string = "";

   
    populateData(data: any)
    {
        this.email = data["userMail"];

        this.deviceId = data["deviceId"];
        this.serialNumber = data["serial_number"];
        this.deviceName = data["deviceBrand"] + " " + data["deviceModel"];
        this.deviceType = data["deviceType"];
        this.requestStatus = data["requestStatus"];
        this.bindSpecs(data);
        this.userName = this.getUserName(data, "requestedUser");
        this.adminName = this.getUserName(data, "deviceSubmittedAdmin");

        this.requestDate = data["requestDate"];
        this.assignDate = data["assignedDate"];
        this.assignDays = data["assignDays"] == -1 ? 0 : data["assignDays"];
        this.returnDate = data["returnDate"];

    }

    private bindSpecs(value: any) {
        this.specifications = (value["specs"]["ram"] == ""?"": "RAM = "+ value["specs"]["ram"] +",") + 
        (value["specs"]["storage"]== "" ? "" : "Storage =  " + value["specs"]["storage"] +"," ) +
        (value["specs"]["screenSize"] =="" ? "" : " Screen-Size = " + value["specs"]["screenSize"] +",") +
         ( value["specs"]["connectivity"] == "" ? "" : " connectivity =  " + value["specs"]["connectivity"] );
    }

    private getUserName(value: any, type: string): string {
        return (value[type]["salutation"] + " " + value[type]["firstName"] + " "
            + value[type]["middleName"] + " " + value[type]["lastname"]);
    }
}