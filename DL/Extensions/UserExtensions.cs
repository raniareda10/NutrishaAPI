using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DL.DBContext;
using DL.Entities;
using DL.EntitiesV1.AdminUser;
using DL.EntitiesV1.Measurements;
using DL.Repositories.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace DL.Extensions
{
    public static class UserExtensions
    {
        public static Task<AdminUserModel> GetUserByIdAsync(this DbSet<MUser> users, long id)
        {
            return users.Where(u =>
                    u.Id == id
                )
                .Select(u => new AdminUserModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password,
                    Name = u.Name,
                    Language = u.Language,
                    PersonalImage = u.PersonalImage,
                })
                .FirstOrDefaultAsync();
        }

        public static async Task<AdminUserModel> GetAdminUserAsync(this AppDBContext users,
            Expression<Func<AdminUserEntity, bool>> predicate)
        {
            var user = await users.AdminUsers.Where(predicate)
                .Select(u => new AdminUserModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password,
                    Name = u.Name,
                    PersonalImage = u.PersonalImage,
                    RoleName = u.Roles.First().Role.Name
                })
                .FirstOrDefaultAsync();

            if (user == null) return null;

            user.Permissions = await users.MUserRoles
                .Where(r => r.AdminUserId == user.Id)
                .Select(r => r.Role.RolePermissions
                    .Select(m => m.Permission.Name))
                .FirstOrDefaultAsync();

            return user;
        }

        public static async Task<float> GetWeightLossAsync(this AppDBContext dbContext, int userId,
            float? lastWeight = null)
        {
            lastWeight ??= await dbContext.UserMeasurements
                .OrderByDescending(m => m.Created)
                .Where(m => m.UserId == userId)
                .Where(m => m.MeasurementType == MeasurementType.Weight)
                .Select(w => w.MeasurementValue)
                .FirstOrDefaultAsync();

            var firstWeight = await dbContext.UserMeasurements
                .OrderByDescending(m => m.Created)
                .Where(m => m.UserId == userId)
                .Where(m => m.MeasurementType == MeasurementType.Weight)
                .Select(w => w.MeasurementValue)
                .LastOrDefaultAsync();

            return (float)(firstWeight - lastWeight) * -1;
        }
    }
}