using Store.CoreLayer.Entirty;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class NotificationSpec:Specification<Notification>
    {
        public NotificationSpec(string UserId, NotificationParamter paramter):base(n=>n.UserId==UserId)
        {
            ApplyOrderByDesc(n => n.CreatedAt);
            ApplyPagination(paramter.PageSize,paramter.PageSize*(paramter.PageIndex-1));
        }
        public NotificationSpec(int notificationId,string userId):base(x=>x.Id==notificationId&&x.UserId==userId)
        {
            ApplyOrderByDesc(x => x.CreatedAt);
        }
    }
}
