using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.UserAllergyDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IUserAllergyRepository
    {
        PagedList<MUserAllergy> GetAllUserAllergy(UserAllergyQueryPramter UserAllergyParameters);

    }

    public class UserAllergyRepository : Repository<MUserAllergy>, IUserAllergyRepository
    {
        public UserAllergyRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MUserAllergy> GetAllUserAllergy(UserAllergyQueryPramter UserAllergyParameters)
        {

             

            IQueryable<MUserAllergy> UserAllergy = GetAll().Include(dt => dt.Allergy);


            //searhing
            SearchByPramter(ref UserAllergy,UserAllergyParameters.UserId,  UserAllergyParameters.AllergyId);

            return PagedList<MUserAllergy>.ToPagedList(UserAllergy,
                UserAllergyParameters.PageNumber,
                UserAllergyParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MUserAllergy> UserAllergy, int? userId, int? allergyId)
        {
            //if (UserAllergy.Any() && sourceId > 0)
            //{
            //    UserAllergy = UserAllergy.Where(o => o.SourceId == sourceId);
            //}

            //if (UserAllergy.Any() && adminId > 0)
            //{
            //    UserAllergy = UserAllergy.Where(o => o.AdminId == adminId);
            //}
            //if (UserAllergy.Any() && customerId > 0)
            //{
            //    UserAllergy = UserAllergy.Where(o => o.CustomerId == customerId);
            //}
            if (UserAllergy.Any() && userId > 0)
            {
                UserAllergy = UserAllergy.Where(o => o.UserId == userId);
            }
            if (UserAllergy.Any() && allergyId > 0)
            {
                UserAllergy = UserAllergy.Where(o => o.AllergyId == allergyId);
            }
            return;
        }


    }
}
