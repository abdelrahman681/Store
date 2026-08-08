using Microsoft.AspNetCore.SignalR;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.CoreLayer.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;


namespace Store.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IHubContext<NotificationHub> hubContext,IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Notification?> GetNotificationByIdAsync(int NotificationId,string UserId)
        {
            var spec = new NotificationSpec(NotificationId, UserId);
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsyncWithSpecification(spec);
            if(notification == null)return null;
            return notification;
        }

        public async Task<IReadOnlyList<Notification?>> GetNotificationsAsync(string UserId, NotificationParamter paramter)
        {
            var spec=new NotificationSpec(UserId,paramter);
            var notifications = await _unitOfWork.Repository<Notification>().GetAllAsyncWithSpecification(spec);
            if (notifications is null) return null;
            return notifications;
        }

        public async Task SendNotificationAsync(string userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
                
        }
    }
}
