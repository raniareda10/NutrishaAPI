using AutoMapper;
using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.DTOs.ContactUsDTO;
using DL.DTOs.UserDTO;
using DL.DTOs.NotificationTypeDTO;
using DL.DTOs.NotificationDTO;
using DL.DTOs.NotificationUserDTO;
using DL.DTOs.AllergyDTO;
using DL.DTOs.RiskDTO;
using DL.DTOs.GoalTypeDTO;
using DL.DTOs.GenderDTO;
using DL.DTOs.FrequencyDTO;
using DL.DTOs.JourneyPlanDTO;
using DL.DTOs.UserAllergyDTO;
using DL.DTOs.UserRiskDTO;
using DL.DTOs.UserGoalDTO;
using DL.DTOs.GoalDTO;
using DL.DTOs.MealTypeDTO;
using DL.DTOs.IngredientDTO;
using DL.DTOs.FoodStepsDTO;
using DL.DTOs.MealStepsDTO;
using DL.DTOs.UserMealDTO;
using DL.DTOs.DislikeMealDTO;
using DL.DTOs.MealIngredientDTO;
using DL.DTOs.MealDTO;
using DL.DTOs.VideoDTO;
using DL.DTOs.UserPollAnswerDTO;
using DL.DTOs.SplashDTO;
using DL.DTOs.PollAnswerDTO;
using DL.DTOs.AttachmentTypeDTO;
using DL.DTOs.MediaTypeDTO;

namespace DL.Mapping
{
    public class MappingConfigration : Profile
    {
        public MappingConfigration()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<MUser, UserDTO>().ReverseMap();
            CreateMap<MUser, AllUserDTO>().ReverseMap();
            CreateMap<AllUserDTO, MUser>().ReverseMap();
            CreateMap<MNotification, IncludeNotificationDto>().ReverseMap();
            CreateMap<MUser, IncludeUserDto>().ReverseMap();
            CreateMap<MGoal, IncludeGoalDto>().ReverseMap();
            CreateMap<MContactUs, ContactUsCreatDto>().ReverseMap();
            CreateMap<MNotificationType, NotificationTypeCreatDto>().ReverseMap();
            CreateMap<MNotification, NotificationCreatDto>().ReverseMap();
            CreateMap<MNotificationUser, NotificationUserCreatDto>().ReverseMap();

            CreateMap<MAllergy, AllergyCreatDto>().ReverseMap();
            CreateMap<MRisk, RiskCreatDto>().ReverseMap();
            CreateMap<MGoalType, GoalTypeCreatDto>().ReverseMap();
            CreateMap<MGender, GenderCreatDto>().ReverseMap();
            CreateMap<MFrequency, FrequencyCreatDto>().ReverseMap();
            CreateMap<MJourneyPlan, JourneyPlanCreatDto>().ReverseMap();
            //  CreateMap<MVerfiyCode, VerfiyCodeCreatDto>().ReverseMap();

            CreateMap<MUserAllergy, UserAllergyCreatDto>().ReverseMap();
            CreateMap<MUserRisk, UserRiskCreatDto>().ReverseMap();
            CreateMap<MUserGoal, UserGoalCreatDto>().ReverseMap();
            CreateMap<MGoal, GoalCreatDto>().ReverseMap();
            CreateMap<MMealType, MealTypeCreatDto>().ReverseMap();
            CreateMap<MIngredient, IngredientCreatDto>().ReverseMap();
            CreateMap<MFoodSteps, FoodStepsCreatDto>().ReverseMap();
            CreateMap<MMeal, MealCreatDto>().ReverseMap();
            CreateMap<MMealSteps, MealStepsCreatDto>().ReverseMap();
            CreateMap<MMealIngredient, MealIngredientCreatDto>().ReverseMap();
            CreateMap<MUserMeal, UserMealCreatDto>().ReverseMap();
            CreateMap<MDislikeMeal, DislikeMealCreatDto>().ReverseMap();

            CreateMap<MVideo, VideoCreatDto>().ReverseMap();
            CreateMap<MMediaType, MediaTypeCreatDto>().ReverseMap();
            CreateMap<MAttachmentType, AttachmentTypeCreatDto>().ReverseMap();
            CreateMap<MSplash, SplashCreatDto>().ReverseMap();
        }
    }
}