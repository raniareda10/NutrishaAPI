using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.NotificationUserDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface INotificationUserRepository
    {
        PagedList<MNotificationUser> GetAllNotificationUser(NotificationUserQueryPramter NotificationUserParameters);

    }

    public class NotificationUserRepository : Repository<MNotificationUser>, INotificationUserRepository
    {
        public NotificationUserRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MNotificationUser> GetAllNotificationUser(NotificationUserQueryPramter NotificationUserParameters)
        {

             

            IQueryable<MNotificationUser> NotificationUser = GetAll().Include(dt => dt.Notification);


            //searhing
            SearchByPramter(ref NotificationUser,NotificationUserParameters.UserId,  NotificationUserParameters.NotificationId);

            return PagedList<MNotificationUser>.ToPagedList(NotificationUser,
                NotificationUserParameters.PageNumber,
                NotificationUserParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MNotificationUser> NotificationUser, int? userId, int? notificationId)
        {
            //if (NotificationUser.Any() && sourceId > 0)
            //{
            //    NotificationUser = NotificationUser.Where(o => o.SourceId == sourceId);
            //}

            //if (NotificationUser.Any() && adminId > 0)
            //{
            //    NotificationUser = NotificationUser.Where(o => o.AdminId == adminId);
            //}
            //if (NotificationUser.Any() && customerId > 0)
            //{
            //    NotificationUser = NotificationUser.Where(o => o.CustomerId == customerId);
            //}
            if (NotificationUser.Any() && userId > 0)
            {
                NotificationUser = NotificationUser.Where(o => o.UserId == userId);
            }
            if (NotificationUser.Any() && notificationId > 0)
            {
                NotificationUser = NotificationUser.Where(o => o.NotificationId == notificationId);
            }
            return;
        }


    }
}
