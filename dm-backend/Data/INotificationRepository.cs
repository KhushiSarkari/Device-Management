using System.Threading.Tasks;
using dm_backend.Models;
using System.Collections.Generic;
using dm_backend.EFModels;
using System.Linq;
using dm_backend.Logics;

namespace dm_backend.Data{
public interface INotificationRepository{
    string RejectNotification(int notificationId);
    int GetNotification(int id);   
    List<Notification> GetAllNotifications(int userId,string sortField, string sortDirection,string searchField);
    int AddNotification(NotificationModel contents);

    IQueryable<MailModel> getUserDetailsforNotif(int DeviceId);
    string AddMultipleNotifications(MultipleNotifications notify);
    List<Task> sendMultipleMail(MultipleNotifications item);
    }
}