using System.Threading.Tasks;
using AutoMapper;
using DL.DBContext;
using DL.DTOs.UserDTOs;
using DL.DtosV1.Users.Mobiles;
using DL.ResultModels;
using DL.StorageServices;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Profiles
{
    public class MobileProfileService
    {
        private readonly IStorageService _storageService;
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public MobileProfileService(
            IStorageService storageService,
            AppDBContext appDbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _storageService = storageService;
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
    }
}