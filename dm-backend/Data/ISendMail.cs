using System.Collections.Generic;
using System.Threading.Tasks;
using dm_backend.Models;

namespace dm_backend.Data{
    public interface ISendMail{
        Task sendNotification(string  email , string  body, string subject  );
        
    }
}