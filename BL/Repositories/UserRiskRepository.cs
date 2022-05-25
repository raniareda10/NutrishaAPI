using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.UserRiskDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IUserRiskRepository
    {
        PagedList<MUserRisk> GetAllUserRisk(UserRiskQueryPramter UserRiskParameters);

    }

    public class UserRiskRepository : Repository<MUserRisk>, IUserRiskRepository
    {
        public UserRiskRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MUserRisk> GetAllUserRisk(UserRiskQueryPramter UserRiskParameters)
        {

             

            IQueryable<MUserRisk> UserRisk = GetAll().Include(dt => dt.Risk);


            //searhing
            SearchByPramter(ref UserRisk,UserRiskParameters.UserId,  UserRiskParameters.RiskId);

            return PagedList<MUserRisk>.ToPagedList(UserRisk,
                UserRiskParameters.PageNumber,
                UserRiskParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MUserRisk> UserRisk, int? userId, int? riskId)
        {
            //if (UserRisk.Any() && sourceId > 0)
            //{
            //    UserRisk = UserRisk.Where(o => o.SourceId == sourceId);
            //}

            //if (UserRisk.Any() && adminId > 0)
            //{
            //    UserRisk = UserRisk.Where(o => o.AdminId == adminId);
            //}
            //if (UserRisk.Any() && customerId > 0)
            //{
            //    UserRisk = UserRisk.Where(o => o.CustomerId == customerId);
            //}
            if (UserRisk.Any() && userId > 0)
            {
                UserRisk = UserRisk.Where(o => o.UserId == userId);
            }
            if (UserRisk.Any() && riskId > 0)
            {
                UserRisk = UserRisk.Where(o => o.RiskId == riskId);
            }
            return;
        }


    }
}
