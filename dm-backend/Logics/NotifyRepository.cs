using dm_backend.Data;
using System.Collections.Generic;
using System.Linq;
using dm_backend.EFModels;
using System;

namespace dm_backend.Logics
{
    public class NotifyRepository
    {
        private readonly EFDbContext _context;

        public NotifyRepository(EFDbContext context )
        {
            _context =context ;
        }
        public List<DeviceReturn> GetData()
        {
               var entryPoint = (from us in _context.User
                              join ad in _context.AssignDevice on us.UserId equals ad.UserId
                              join  dv in _context.Device on ad.DeviceId equals dv.DeviceId
                              join dt in _context.DeviceType on dv.DeviceTypeId equals dt.DeviceTypeId
                              join br in _context.DeviceBrand on dv.DeviceBrandId equals br.DeviceBrandId
                              join md in _context.DeviceModel on dv.DeviceModelId equals md.DeviceModelId
                              select new DeviceReturn
                              {   
                                  Days= ad.ReturnDate.Date.Subtract(System.DateTime.Now.Date).Days ,
                                  SerialNumber = dv.SerialNumber , 
                                  FirstName = us.FirstName , 
                                  LastName = us.LastName , 
                                  Email = us.Email , 
                                  DeviceCompany = br.Brand , 
                                  DeviceModel =md.Model ,
                                  DeviceType = dt.Type , 
                             }).ToList();

                             return entryPoint ;
         }
         public List<string> SendDeviceReturnEmail(int num)
         {
             var ListName = new List<string>(); 
             var listofusers = GetData();
              listofusers.Sort(delegate( DeviceReturn x, DeviceReturn y){return x.Email.CompareTo(y.Email);});
             int i=0;

               while(i<listofusers.Count)
               { 
                   if(listofusers[i].Days==num)
               {
                 string devices = listofusers[i].DeviceCompany+" "+listofusers[i].DeviceType+" "+listofusers[i].DeviceModel+" Serial Number :  "+listofusers[i].SerialNumber+"<br>"; 

                 while(i+1<listofusers.Count && listofusers[i].Email==listofusers[i+1].Email && listofusers[i+1].Days==num)
                 {
                    devices +=listofusers[i+1].DeviceCompany+" "+listofusers[i+1].DeviceType+" "+listofusers[i+1].DeviceModel+" Serial Number :  "+listofusers[i+1].SerialNumber+"<br>"; 
                    i++;
                 }
                 i++;
                 ListName.Add(listofusers[i-1].Email);
                string body = "Hi "+listofusers[i-1].FirstName+" "+listofusers[i-1].LastName+"<br> Return Following Devices Today <br>"+devices+"<br> Thanks ";
               Console.WriteLine(body);
              //  var sendEmailObject = new sendMail().sendNotification(listofusers[i-1].Email,body,"Return Devices");
               }
               else
               {
               i++;
               }
             }
              return ListName;
         }

    
    }
}