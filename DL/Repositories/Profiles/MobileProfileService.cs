using System.Threading.Tasks;
using AutoMapper;
using DL.DBContext;
using DL.DTOs.UserDTOs;
using DL.DtosV1.UserMeasurements;
using DL.DtosV1.Users.Mobiles;
using DL.EntitiesV1.Measurements;
using DL.Repositories.UserMeasurement;
using DL.ResultModels;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Profiles
{
    public class MobileProfileService
    {
        private readonly IStorageService _storageService;
        private readonly UserMeasurementRepository _userMeasurementRepository;
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public MobileProfileService(
            IStorageService storageService,
            UserMeasurementRepository userMeasurementRepository,
            AppDBContext appDbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _storageService = storageService;
            _userMeasurementRepository = userMeasurementRepository;
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PayloadServiceResult<AllUserDTO>> PutAsync(UpdateProfileDto updateProfileDto)
        {
            var currentUser = await _appDbContext.MUser
                .FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);

            currentUser.Name = updateProfileDto.FirstName;
            currentUser.LastName = updateProfileDto.LastName;
            
            if (currentUser.Weight != updateProfileDto.Weight)
            {
                await _userMeasurementRepository.PostAsync(new PostUserMeasurement()
                {
                    MeasurementType = MeasurementType.Weight,
                    MeasurementValue = (float)updateProfileDto.Weight
                });
            }

            currentUser.Height = updateProfileDto.Height;
            currentUser.Weight = updateProfileDto.Weight;
            currentUser.GenderId = updateProfileDto.GenderId;
            currentUser.Mobile = updateProfileDto.MobileNumber;
            currentUser.BirthDate = updateProfileDto.BirthDate;
            if (updateProfileDto.PersonalImage != null)
            {
                currentUser.PersonalImage =
                    await _storageService.UploadFileAsync(updateProfileDto.PersonalImage, "users");
            }

            _appDbContext.Update(currentUser);
            await _appDbContext.SaveChangesAsync();

            return new PayloadServiceResult<AllUserDTO>()
            {
                Data = _mapper.Map<AllUserDTO>(currentUser)
            };
        }

        public async Task<AllUserDTO> GetCurrentUserProfileAsync()
        {
            var currentUser = await _appDbContext.MUser
                .FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);

            return _mapper.Map<AllUserDTO>(currentUser);
        }

        public async Task AddSubscribedUserPersonalDetailsAsync(AddAfterSubscriptionDetails afterSubscriptionDetails)
        {
            var currentUser = await _appDbContext.MUser
                .FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId);

            if (currentUser == null) return;

            currentUser.ActivityLevel = afterSubscriptionDetails.ActivityLevel;
            currentUser.NumberOfMealsPerDay = afterSubscriptionDetails.NumberOfMealsPerDay;
            currentUser.EatReason = afterSubscriptionDetails.EatReason;
            currentUser.TargetWeight = afterSubscriptionDetails.TargetWeight;
            currentUser.MedicineNames = afterSubscriptionDetails.MedicineNames;
            currentUser.IsRegularMeasurer = afterSubscriptionDetails.IsRegularMeasurer;
            currentUser.HasBaby = afterSubscriptionDetails.HasBaby;
            currentUser.IsMealPlanPreferencesDataCompleted = true;
            _appDbContext.Update(currentUser);
            await _appDbContext.SaveChangesAsync();
        }
    }
}