using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.NotificationDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface INotificationRepository
    {
        PagedList<MNotification> GetAllNotification(NotificationQueryPramter NotificationParameters);

    }

    public class NotificationRepository : Repository<MNotification>, INotificationRepository
    {
        public NotificationRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MNotification> GetAllNotification(NotificationQueryPramter NotificationParameters)
        {

             

            IQueryable<MNotification> Notification = GetAll();


            //searhing
            SearchByPramter(ref Notification, NotificationParameters.CustomerId, NotificationParameters.UserId, NotificationParameters.SourceId, NotificationParameters.AdminId);

            return PagedList<MNotification>.ToPagedList(Notification,
                NotificationParameters.PageNumber,
                NotificationParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MNotification> Notification, int? customerId, int? userId, int? sourceId, int? adminId)
        {
            if (Notification.Any() && sourceId > 0)
            {
                Notification = Notification.Where(o => o.SourceId == sourceId);
            }

            if (Notification.Any() && adminId > 0)
            {
                Notification = Notification.Where(o => o.AdminId == adminId);
            }
            //if (Notification.Any() && customerId > 0)
            //{
            //    Notification = Notification.Where(o => o.CustomerId == customerId);
            //}
            //if (Notification.Any() && userId > 0)
            //{
            //    Notification = Notification.Where(o => o.UserId == userId);
            //}
            return;
        }


    }
}
