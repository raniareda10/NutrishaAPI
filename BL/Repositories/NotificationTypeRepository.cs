using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.NotificationTypeDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface INotificationTypeRepository
    {
        PagedList<MNotificationType> GetAllNotificationType(NotificationTypeQueryPramter NotificationTypeParameters);

    }

    public class NotificationTypeRepository : Repository<MNotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MNotificationType> GetAllNotificationType(NotificationTypeQueryPramter NotificationTypeParameters)
        {

             

            IQueryable<MNotificationType> NotificationType = GetAll();


            //searhing
            SearchByPramter(ref NotificationType, NotificationTypeParameters.Name);

            return PagedList<MNotificationType>.ToPagedList(NotificationType,
                NotificationTypeParameters.PageNumber,
                NotificationTypeParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MNotificationType> NotificationType, string Searchpramter)
        {
            if (!NotificationType.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            NotificationType = NotificationType.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
