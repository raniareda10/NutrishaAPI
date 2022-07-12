using System.Linq;
using System.Threading.Tasks;
using DL.Entities;
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
                    PersonalImage = u.PersonalImage
                })
                .FirstOrDefaultAsync();
        }
    }
}