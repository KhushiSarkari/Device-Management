export class FaultyDeviceModel
{
    public complaintId : number
    public userId : number  
    public name : string
    public deviceId : number 
    public device : string 
    public serialNumber : string 
    public complaitDate  : string 
    public Comments : string
    public image:string


    constructor(data : any )
    {
        this.complaintId = data.complaintId;
        this.userId = data.userId;
        this.deviceId = data.deviceId;
        this.serialNumber= data.serialNumber;
        this.name = data.name;
        this.device =  data.device;
        this.Comments = data.Comments;
        this.complaitDate = data.complaintDate;
        this.image=data.image;
    }
}