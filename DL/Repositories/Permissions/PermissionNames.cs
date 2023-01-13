using Org.BouncyCastle.Bcpg.OpenPgp;

namespace DL.Repositories.Permissions
{
    public class PermissionNames
    {
        // Admin has all permissions
        #region Admin Users Module Permissions

        public const string CanAccessAdminUsersModule = "canAccessAdminUsersModule";
        public const string CanViewAdminUsers = "canViewAdminUsers";
        public const string CanCreateAdminUsers = "canCreateAdminUsers";
        public const string CanEditAdminUsers = "canEditAdminUsers";
        public const string CanDeleteAdminUsers = "canDeleteAdminUsers";

        #endregion

        #region Blog permissions

        // Admin, Doctor
        public const string CanAccessBlogsModule = "canAccessBlogsModule";

        // Admin, Doctor
        public const string CanViewBlogs = "canViewBlogs";

        // Admin, , Doctor
        public const string CanCreateBlogs = "canCreateBlogs";

        // Admin
        public const string CanEditBlogs = "canEditBlogs";

        // Admin
        public const string CanDeleteBlogs = "canDeleteBlogs";

        #endregion

        #region Comment Permissions

        // Comment Permissions
        // Admin
        public const string CanModerateComments = "canModerateComments";

        // Admin, Doctor
        public const string CanViewComments = "canViewComments";

        #endregion

        #region App User Permissions

        // Admin, Doctor
        public const string CanAccessAppUsersModule = "canAccessAppUsersModule";

        // Admin, Doctor
        public const string CanViewAppUsers = "canViewAppUsers";
        // Doctor
        public const string CanViewAppUserContactInfo = "canViewAppUserContactInfo"; 
        // Admin
        public const string CanBanAppUsers = "canBanAppUsers";

        // Doctor
        public const string CanAssignMealPlanToAppUsers = "canAssignMealPlanToAppUsers";

        #endregion


        #region Plan Templates Permissions

        // Doctor
        public const string CanAccessMealPlansModule = "canAccessMealPlansModule";
        public const string CanViewMealPlans = "canViewMealPlans";
        public const string CanCreateParentMealPlans = "canCreateParentMealPlans";
        public const string CanCreateChildMealPlans = "canCreateChildMealPlans";
        
        public const string CanEditMealPlans = "canEditMealPlans";
        
        #endregion

        #region Meals Permissions

        // Admin, Doctor
        public const string CanAccessMealsModule = "canAccessMealsModule";

        // Admin, Doctor
        public const string CanViewMeals = "canViewMeals";
        public const string CanCreateMeals = "canCreateMeals";
        public const string CanEditMeals = "canEditMeals";
        public const string CanDeleteMeals = "canDeleteMeals";

        #endregion
    }
}