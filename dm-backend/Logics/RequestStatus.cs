using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Logics
{
    public class RequestStatus
    {

        


        public async Task<string> sendStatusMail(int requestId , string status , string name , string mail)
        {
           
            string rejectBody = @"<h3> 
  Device Request Reject
</h3>  Hello <b> "+ name +" </b> This mail is to inform you that your device request has been Rejected. ";
            string acceptBody =@" <h3>
  Device Request Reject
</h3> Sir <b> "+ name +" </b> This mail is to inform you that your device request has been Accepted. You can take it from admin Department ";



            if (status== "reject")
            {
                await new sendMail().sendNotification(mail, rejectBody, "Request  Rejected");
            }
            if(status == "accept")
            {
                await new sendMail().sendNotification(mail, acceptBody, "Request  Rejected");
            }
            return "";
        }
    }
}
