using System.Threading.Tasks;
using System.Collections.Generic;
using dm_backend.EFModels;
using System.Linq;

namespace dm_backend.Data{
public interface INotificationRepository{
    bool RejectNotification(int notificationId);
    int GetNotificationCount(int id);   
    List<Notification> GetAllNotifications(BaseQueryParams param);
    void AddNotification(Notification contents);
    IQueryable<AssignDevice> getUserDetailsforNotif(int DeviceId);
    string AddMultipleNotifications(List<Notification> notifys);
    }
}