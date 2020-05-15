export class FaultyDeviceModel
{
    public complaintId : number
    public userId : number  
    public userName : string
    public deviceId : number 
    public deviceType : string
    public deviceName : string 
    public serialNumber : string 
    public status : string
    public complaitDate  : string 
    public complaint : string
    public image:string


    constructor(data : any )
    {
        this.complaintId = data.complaintId;
        this.userId = data.userId;
        this.deviceId = data.deviceId;
        this.serialNumber= data.serialNumber;
        this.userName = data.salutation  + " " + data.userName.first_name + " " +( data.userName.middle_name== "" ? "": data.userName.middle_name + " "  ) +data.userName.last_name;
        this.deviceType = data.deviceType; 
        this.deviceName =  data.deviceBrand +" " + data.deviceModel ; 
        this.status = data.status;
        this.complaint = data.issue;
        this.complaitDate = data.complaintDate;
        this.image=data.image;
    }
}