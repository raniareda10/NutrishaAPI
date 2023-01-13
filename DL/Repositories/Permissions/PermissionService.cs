using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.Entities;
using DL.EntitiesV1.Roles;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Permissions
{
    public class PermissionService
    {
        private readonly AppDBContext _appDbContext;

        public PermissionService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task InitializePermissionsAsync()
        {
            await _appDbContext.Database.ExecuteSqlRawAsync(@"
                    DELETE FROM RolePermissions;
                    DELETE FROM MUserRoles;
                    DELETE FROM Permissions;
                    DELETE FROM MRoles;
                    DBCC CHECKIDENT ('Permissions');
                    DBCC CHECKIDENT ('MRoles');
            ");

            var canAccessAdminUsersModule = new PermissionEntity() { Name = "canAccessAdminUsersModule" };
            var canViewAdminUsers = new PermissionEntity() { Name = "canViewAdminUsers" };
            var canCreateAdminUsers = new PermissionEntity() { Name = "canCreateAdminUsers" };
            var canEditAdminUsers = new PermissionEntity() { Name = "canEditAdminUsers" };
            var canDeleteAdminUsers = new PermissionEntity() { Name = "canDeleteAdminUsers" };
            var canAccessBlogsModule = new PermissionEntity() { Name = "canAccessBlogsModule" };
            var canViewBlogs = new PermissionEntity() { Name = "canViewBlogs" };
            var canCreateBlogs = new PermissionEntity() { Name = "canCreateBlogs" };
            var canEditBlogs = new PermissionEntity() { Name = "canEditBlogs" };
            var canDeleteBlogs = new PermissionEntity() { Name = "canDeleteBlogs" };
            var canModerateComments = new PermissionEntity() { Name = "canModerateComments" };
            var canViewComments = new PermissionEntity() { Name = "canViewComments" };
            var canAccessAppUsersModule = new PermissionEntity() { Name = "canAccessAppUsersModule" };
            var canViewAppUsers = new PermissionEntity() { Name = "canViewAppUsers" };
            var canViewAppUserContactInfo = new PermissionEntity() { Name = "canViewAppUserContactInfo" };
            var canBanAppUsers = new PermissionEntity() { Name = "canBanAppUsers" };
            var canAssignMealPlanToAppUsers = new PermissionEntity() { Name = "canAssignMealPlanToAppUsers" };
            var canAccessMealPlansModule = new PermissionEntity() { Name = "canAccessMealPlansModule" };
            var canViewMealPlans = new PermissionEntity() { Name = "canViewMealPlans" };
            var canCreateParentMealPlans = new PermissionEntity() { Name = "canCreateParentMealPlans" };
            var canCreateChildMealPlans = new PermissionEntity() { Name = "canCreateChildMealPlans" };
            var canEditMealPlans = new PermissionEntity() { Name = "canEditMealPlans" };
            var canAccessMealsModule = new PermissionEntity() { Name = "canAccessMealsModule" };
            var canViewMeals = new PermissionEntity() { Name = "canViewMeals" };
            var canCreateMeals = new PermissionEntity() { Name = "canCreateMeals" };
            var canEditMeals = new PermissionEntity() { Name = "canEditMeals" };
            var canDeleteMeals = new PermissionEntity() { Name = "canDeleteMeals" };

            var ownerRole = new MRole() { Name = "Owner" };
            var adminRole = new MRole() { Name = "Admin" };
            var doctorRole = new MRole() { Name = "Doctor" };

            ownerRole.RolePermissions = new List<RolePermissionEntity>()
            {
                new RolePermissionEntity { Permission = canAccessAdminUsersModule },
                new RolePermissionEntity { Permission = canViewAdminUsers },
                new RolePermissionEntity { Permission = canCreateAdminUsers },
                new RolePermissionEntity { Permission = canEditAdminUsers },
                new RolePermissionEntity { Permission = canDeleteAdminUsers },
                new RolePermissionEntity { Permission = canAccessBlogsModule },
                new RolePermissionEntity { Permission = canViewBlogs },
                new RolePermissionEntity { Permission = canCreateBlogs },
                new RolePermissionEntity { Permission = canEditBlogs },
                new RolePermissionEntity { Permission = canDeleteBlogs },
                new RolePermissionEntity { Permission = canModerateComments },
                new RolePermissionEntity { Permission = canViewComments },
                new RolePermissionEntity { Permission = canAccessAppUsersModule },
                new RolePermissionEntity { Permission = canViewAppUsers },
                new RolePermissionEntity { Permission = canViewAppUserContactInfo },
                new RolePermissionEntity { Permission = canBanAppUsers },
                new RolePermissionEntity { Permission = canAssignMealPlanToAppUsers },
                new RolePermissionEntity { Permission = canAccessMealPlansModule },
                new RolePermissionEntity { Permission = canViewMealPlans },
                new RolePermissionEntity { Permission = canCreateParentMealPlans },
                new RolePermissionEntity { Permission = canCreateChildMealPlans },
                new RolePermissionEntity { Permission = canEditMealPlans },
                new RolePermissionEntity { Permission = canAccessMealsModule },
                new RolePermissionEntity { Permission = canViewMeals },
                new RolePermissionEntity { Permission = canCreateMeals },
                new RolePermissionEntity { Permission = canEditMeals },
                new RolePermissionEntity { Permission = canDeleteMeals }
            };

            adminRole.RolePermissions = new List<RolePermissionEntity>()
            {
                // new RolePermissionEntity { Permission = canAccessAdminUsersModule },
                // new RolePermissionEntity { Permission = canViewAdminUsers },
                // new RolePermissionEntity { Permission = canCreateAdminUsers },
                // new RolePermissionEntity { Permission = canEditAdminUsers },
                // new RolePermissionEntity { Permission = canDeleteAdminUsers },
                new RolePermissionEntity { Permission = canAccessBlogsModule },
                new RolePermissionEntity { Permission = canViewBlogs },
                new RolePermissionEntity { Permission = canCreateBlogs },
                new RolePermissionEntity { Permission = canEditBlogs },
                new RolePermissionEntity { Permission = canDeleteBlogs },
                new RolePermissionEntity { Permission = canModerateComments },
                new RolePermissionEntity { Permission = canViewComments },
                new RolePermissionEntity { Permission = canAccessAppUsersModule },
                new RolePermissionEntity { Permission = canViewAppUsers },
                new RolePermissionEntity { Permission = canBanAppUsers },
                // new RolePermissionEntity { Permission = canAssignMealPlanToAppUsers },
                // new RolePermissionEntity { Permission = canAccessMealPlansModule },
                // new RolePermissionEntity { Permission = canViewMealPlans },
                // new RolePermissionEntity { Permission = canCreateMealPlans },
                // new RolePermissionEntity { Permission = canEditMealPlans },
                // new RolePermissionEntity { Permission = canAccessMealsModule },
                // new RolePermissionEntity { Permission = canViewMeals },
                // new RolePermissionEntity { Permission = canCreateMeals },
                // new RolePermissionEntity { Permission = canEditMeals },
                // new RolePermissionEntity { Permission = canDeleteMeals }
            };
            
            doctorRole.RolePermissions = new List<RolePermissionEntity>()
            {
                // new RolePermissionEntity { Permission = canAccessAdminUsersModule },
                // new RolePermissionEntity { Permission = canViewAdminUsers },
                // new RolePermissionEntity { Permission = canCreateAdminUsers },
                // new RolePermissionEntity { Permission = canEditAdminUsers },
                // new RolePermissionEntity { Permission = canDeleteAdminUsers },
                new RolePermissionEntity { Permission = canAccessBlogsModule },
                new RolePermissionEntity { Permission = canViewBlogs },
                new RolePermissionEntity { Permission = canCreateBlogs },
                new RolePermissionEntity { Permission = canEditBlogs },
                new RolePermissionEntity { Permission = canDeleteBlogs },
                // new RolePermissionEntity { Permission = canModerateComments },
                new RolePermissionEntity { Permission = canViewComments },
                new RolePermissionEntity { Permission = canAccessAppUsersModule },
                new RolePermissionEntity { Permission = canViewAppUsers },
                new RolePermissionEntity { Permission = canViewAppUserContactInfo },
                // new RolePermissionEntity { Permission = canBanAppUsers },
                new RolePermissionEntity { Permission = canAssignMealPlanToAppUsers },
                new RolePermissionEntity { Permission = canAccessMealPlansModule },
                new RolePermissionEntity { Permission = canViewMealPlans },
                new RolePermissionEntity { Permission = canCreateChildMealPlans },
                new RolePermissionEntity { Permission = canEditMealPlans },
                new RolePermissionEntity { Permission = canAccessMealsModule },
                new RolePermissionEntity { Permission = canViewMeals },
                new RolePermissionEntity { Permission = canCreateMeals },
                new RolePermissionEntity { Permission = canEditMeals },
                new RolePermissionEntity { Permission = canDeleteMeals }
            };
            
            _appDbContext.Add(ownerRole);
            _appDbContext.Add(adminRole);
            _appDbContext.Add(doctorRole);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task AddRoleToUserAsync(int userId, string roleName)
        {
            var roleId = await _appDbContext.MRoles.Where(m => m.Name == roleName).Select(m => m.Id).FirstAsync();
            _appDbContext.MUserRoles.Add(new MUserRoles()
            {
                UserId = userId,
                RoleId = roleId,
                Created = DateTime.UtcNow
            });
            await _appDbContext.SaveChangesAsync();
        }
    }
}