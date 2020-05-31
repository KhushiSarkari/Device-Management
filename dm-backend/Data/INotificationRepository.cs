using System.Threading.Tasks;
using dm_backend.Models;
using System.Collections.Generic;
using dm_backend.EFModels;
using System.Linq;
using dm_backend.Logics;

namespace dm_backend.Data{
public interface INotificationRepository{
    string RejectNotification(int notificationId);
    int GetNotificationCount(int id);   
    List<Notification> GetAllNotifications(BaseQueryParams param);
    void AddNotification(Notification contents);

    IQueryable<AssignDevice> getUserDetailsforNotif(int DeviceId);
    string AddMultipleNotifications(List<Notification> notifys);
    }
}