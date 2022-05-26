using BL.Repositories;
using System;

namespace BL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        UserRepository UserRepository { get; }
        RoleRepository RoleRepository { get; }
        UserRolesRepository UserRolesRepository { get; }
        VerfiyCodeRepository VerfiyCodeRepository { get; }
        NotificationTypeRepository NotificationTypeRepository { get; }
        GoalTypeRepository GoalTypeRepository { get; }
        AllergyRepository AllergyRepository { get; }
        FrequencyRepository FrequencyRepository { get; }
        GenderRepository GenderRepository { get; }
        RiskRepository RiskRepository { get; }
        SplashRepository SplashRepository { get; }
        JourneyPlanRepository JourneyPlanRepository { get; }
        NotificationUserRepository NotificationUserRepository { get; }
        ContactUsRepository ContactUsRepository { get; }
        NotificationRepository NotificationRepository { get; }
        UserGroupRepository UserGroupRepository { get; }
        GroupRepository GroupRepository { get; }

        RolesGroupRepository RolesGroupRepository { get; }
        GoalRepository GoalRepository { get; }
        UserAllergyRepository UserAllergyRepository { get; }
        UserGoalRepository UserGoalRepository { get; }
        UserRiskRepository UserRiskRepository { get; }
        FoodStepsRepository FoodStepsRepository { get; }
        IngredientRepository IngredientRepository { get; }
        MealTypeRepository MealTypeRepository { get; }

        MealRepository MealRepository { get; }
        DislikeMealRepository DislikeMealRepository { get; }
        MealStepsRepository MealStepsRepository { get; }
        MealIngredientRepository MealIngredientRepository { get; }
        UserMealRepository UserMealRepository { get; }

        VideoRepository VideoRepository { get; }
        MediaTypeRepository MediaTypeRepository { get; }
        AttachmentTypeRepository AttachmentTypeRepository { get; }
        SecUserRepository SecUserRepository { get; }

        int Save();
    }
}