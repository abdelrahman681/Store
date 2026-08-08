using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IService
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string title, string message);
        Task<IReadOnlyList<Notification?>> GetNotificationsAsync(string UserId, NotificationParamter paramter);
        Task<Notification?> GetNotificationByIdAsync(int NotificationId,string UserId);
    }
}
