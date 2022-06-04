using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.ErrorMessages;
using DL.MailModels;
using HELPER;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Model.ApiModels;
using NutrishaAPI.Extensions;

namespace NutrishaAPI.Controllers.LegacyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;

        private readonly ISMS _SMS;
        private readonly IAuthenticateService _authService;
        private readonly ICheckUniqes _checkUniq;
        public IConfiguration configuration { get; set; }

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly BaseResponseHelper baseResponse;
        private readonly IMapper _mapper;
        private readonly string _locale;

        private readonly IMailService _mailService;

        public UserController(ISMS SMS, ICheckUniqes checkUniq, IMailService mailService, IMapper mapper,
            IHostingEnvironment hostingEnvironment, IUnitOfWork uow, IAuthenticateService authService,
            IConfiguration iConfig,
            IHttpContextAccessor httpContextAccessor,
            IOptions<TokenManagement> tokenManagement)
        {
            _SMS = SMS;
            _checkUniq = checkUniq;
            _uow = uow;
            _authService = authService;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
            _mailService = mailService;
            baseResponse = new BaseResponseHelper();
            configuration = iConfig;
            _locale = httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];
        }


        /// <summary>
        /// Log in user this Fucn Is Used To Login User
        /// </summary>
        /// <remarks></remarks>
        /// <response code="200"> user object and token </response>
        /// <response code="400">invalid user name or password</response>
        /// <response code="401">Unauthorized</response>
        [AllowAnonymous]
        [HttpPost, Route("LogIn")]
        [ProducesResponseType(typeof(UserWithTokenDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> LogIn(ApiLoginModelDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _authService.AuthenticateUser(request, out string token);
            if (user == null)
            {
                baseResponse.done = false;
                baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                if (!string.IsNullOrEmpty(request.Email))
                {
                    baseResponse.message = " email and password combination isn’t correct";
                }

                if (!string.IsNullOrEmpty(request.Mobile))
                {
                    baseResponse.message = " mobile and password combination isn’t correct";
                }

                return BadRequest(baseResponse);
            }

            // if (!user.IsAccountVerified)
            // {
            //     baseResponse.done = false;
            //     baseResponse.statusCode = (int)HttpStatusCode.BadRequest;                  
            //     baseResponse.message = "Account Not Verfied";                   
            //     return BadRequest(baseResponse);
            // }
            //

            user.Password = null;
            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
            if (user.PersonalImage != null && user.PersonalImage != string.Empty)
            {
                user.PersonalImage =
                    dbConn + $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
            }
            else
            {
                user.PersonalImage = string.Empty;
            }

            if (user.NationalID != null && user.NationalID != string.Empty)
            {
                user.NationalID = dbConn + $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
            }
            else
            {
                user.NationalID = string.Empty;
            }

            // user.VehicleTypeName = item.VehicleType.Name;
            var muser = _uow.UserRepository.GetMany(c => c.Id == user.Id).FirstOrDefault();
            if (request.DeviceTypeId != null && !string.IsNullOrEmpty(request.DeviceToken))
            {
                muser.DeviceToken = request.DeviceToken;
                muser.DeviceTypeId = request.DeviceTypeId;

                user.DeviceToken = request.DeviceToken;
                user.DeviceTypeId = request.DeviceTypeId;
            }

            if (!string.IsNullOrEmpty(request.Language))
            {
                muser.Language = request.Language;
                user.Language = request.Language;
            }

            _uow.UserRepository.Update(muser);
            _uow.Save();
            var verfiyCode = _uow.VerfiyCodeRepository
                .GetMany(c => c.Email == request.Email || c.Mobile == request.Mobile).FirstOrDefault();
            if (verfiyCode != null)
            {
                user.VerfiyCode = verfiyCode.VirfeyCode;
            }

            var AllUser = _mapper.Map<AllUserDTO>(user);

            baseResponse.data = new
            {
                AllUser,
                token
            };
            baseResponse.statusCode = StatusCodes.Status200OK;
            baseResponse.done = true;
            return Ok(baseResponse);

            baseResponse.done = false;
            baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
            if (!string.IsNullOrEmpty(request.Email))
            {
                baseResponse.message = " email and password combination isn’t correct";
            }

            if (!string.IsNullOrEmpty(request.Mobile))
            {
                baseResponse.message = " mobile and password combination isn’t correct";
            }

            return BadRequest(baseResponse);
        }


        // [ClaimRequirement(ClaimTypes.Role, RoleConstant.GetAllUsers)]
        [HttpGet, Route("GetAllUsers")]
        [ProducesResponseType(typeof(AllUserDTO), StatusCodes.Status200OK)]
        public IActionResult GetAllUsers()
        {
            var lstUser = _uow.UserRepository.GetAll().ToList();
            List<AllUserDTO> AllUser = new List<AllUserDTO>();
            foreach (var item in lstUser)
            {
                AllUserDTO userDTO = new AllUserDTO()
                {
                    Name = item.Name,
                    Mobile = item.Mobile,
                    Email = item.Email,
                    Id = item.Id,
                };


                // Return the file. A byte array can also be used instead of a stream
                string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
                if (item.PersonalImage != null && item.PersonalImage != string.Empty)
                {
                    userDTO.PersonalImage = dbConn + $"/Files/Documents/{item.Id}/{item.PersonalImage}";
                }

                if (item.NationalID != null && item.NationalID != string.Empty)
                {
                    userDTO.NationalID = dbConn + $"/Files/Documents/{item.Id}/{item.NationalID}";
                }

                var verfiyCode = _uow.VerfiyCodeRepository
                    .GetMany(c => c.Email == userDTO.Email || c.Mobile == userDTO.Mobile).FirstOrDefault();
                if (verfiyCode != null)
                {
                    userDTO.VerfiyCode = verfiyCode.VirfeyCode;
                }

                AllUser.Add(userDTO);
            }

            //var AllUser = _mapper.Map<List<AllUserDTO>>(lstUser);
            baseResponse.data = AllUser;
            baseResponse.total_rows = AllUser.Count();
            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
            baseResponse.done = true;
            return Ok(baseResponse);
        }


        //[ClaimRequirement(ClaimTypes.Role, RoleConstant.AddRole)]
        //[HttpPost, Route("AddRole")]
        //public IActionResult AddRole(string RoleName)
        //{
        //    _uow.RoleRepository.Add(new DL.Entities.Role { Name = RoleName });
        //    _uow.Save();

        //    return Ok(RoleName);
        //}


        ////   [ClaimRequirement(ClaimTypes.Role, RoleConstant.AddUserRole)]
        //[HttpPost, Route("AddUserRole")]
        //public IActionResult AddUserRole(int UserId, int RoleId)
        //{
        //    _uow.UserRolesRepository.Add(new DL.Entities.UserRoles { RoleId = RoleId, UserId = UserId });
        //    _uow.Save();

        //    return Ok();
        //}


        //[ClaimRequirement(ClaimTypes.Role, RoleConstant.GetUser)]
        [HttpGet, Route("GetUser")]
        [ProducesResponseType(typeof(AllUserDTO), StatusCodes.Status200OK)]
        public IActionResult GetUser(int id)
        {
            var item = _uow.UserRepository.GetMany(dt => dt.Id == id).FirstOrDefault();
            //   var vehicleType = _mapper.Map<DL.DTOs.VehicleTypeDTO.VehicleTypeCreatDto>(item.VehicleType);
            AllUserDTO userDTO = new AllUserDTO()
            {
                Name = item.Name,
                Mobile = item.Mobile,
                Email = item.Email,
                Id = item.Id,
            };


            // Return the file. A byte array can also be used instead of a stream
            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
            if (item.PersonalImage != null && item.PersonalImage != string.Empty)
            {
                userDTO.PersonalImage = dbConn + $"/Files/Documents/{item.Id}/{item.PersonalImage}";
            }

            if (item.NationalID != null && item.NationalID != string.Empty)
            {
                userDTO.NationalID = dbConn + $"/Files/Documents/{item.Id}/{item.NationalID}";
            }

            var verfiyCode = _uow.VerfiyCodeRepository
                .GetMany(c => c.Email == userDTO.Email || c.Mobile == userDTO.Mobile).FirstOrDefault();
            if (verfiyCode != null)
            {
                userDTO.VerfiyCode = verfiyCode.VirfeyCode;
            }

            //var AllUser = _mapper.Map<List<AllUserDTO>>(lstUser);
            baseResponse.data = userDTO;
            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
            baseResponse.done = true;
            return Ok(baseResponse);
        }

        [AllowAnonymous]
        [HttpPost, Route("Register")]
        [ProducesResponseType(typeof(UserWithTokenDTO), StatusCodes.Status200OK)]
        public IActionResult Register(UserDTO request)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid Username or Password");
            var isEmptyEmail = string.IsNullOrEmpty(request.Email);
            var isEmptyMobile = string.IsNullOrEmpty(request.Mobile);
            if (isEmptyEmail && isEmptyMobile)
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound;
                baseResponse.done = false;
                baseResponse.message = "Must Insert Email or Mobile";
                return Ok(baseResponse);
            }

            if (!isEmptyEmail && !isEmptyMobile)
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound;
                baseResponse.done = false;
                baseResponse.message = "Only Email or Mobile.";
                return Ok(baseResponse);
            }

            var isEmailRegistration = !string.IsNullOrWhiteSpace(request.Email);
            try
            {
                var tempUser = _uow.UserRepository
                    .GetAll()
                    .FirstOrDefault(
                        isEmailRegistration
                            ? u => u.Email == request.Email
                            : u => u.Mobile == request.Mobile
                    );

                if (tempUser != null)
                {
                    baseResponse.data = "";
                    baseResponse.statusCode = (int) HttpStatusCode.NotFound;
                    baseResponse.done = false;
                    baseResponse.message = ErrorMessagesKeys.EmailOrPhoneAlreadyExists.Localize(_locale);
                    return Ok(baseResponse);
                }


                tempUser = _mapper.Map<DL.Entities.MUser>(request);
                tempUser.IsActive = true;
                tempUser.Password = EncryptANDDecrypt.EncryptText(request.Password);
                _uow.UserRepository.Add(tempUser);
                _uow.Save();


                // if (!string.IsNullOrEmpty(request.Email))
                // {
                //     var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Email == request.Email).FirstOrDefault();
                //     if (verfiyCode != null)
                //     {
                //         int num = new Random().Next(1000, 9999);
                //         verfiyCode.VirfeyCode = num;
                //         verfiyCode.Date = DateTime.Now.AddMinutes(5);
                //         _uow.VerfiyCodeRepository.Update(verfiyCode);
                //         _uow.Save();
                //         _mailService.SendWelcomeEmailAsync(new WelcomeRequest { ToEmail = request.Email, UserName = request.Email, Id = 0, VerifyCode = num.ToString() });
                //
                //     }
                //     else
                //     {
                //         int num = new Random().Next(1000, 9999);
                //         verfiyCode = new MVerfiyCode { Date = DateTime.Now.AddMinutes(5), Email = request.Email, VirfeyCode = num };
                //         _uow.VerfiyCodeRepository.Add(verfiyCode);
                //         _uow.Save();
                //         _mailService.SendWelcomeEmailAsync(new WelcomeRequest { ToEmail = request.Email, UserName = request.Email, Id = 0, VerifyCode = num.ToString() });
                //
                //     }
                // }
                // if (!string.IsNullOrEmpty(request.Mobile))
                // {
                //     VerifyCodeHelper verifyCodeHelper = new VerifyCodeHelper(_uow, _SMS);
                //     verifyCodeHelper.SendOTP(request.Mobile, userId);
                // }
                var userLogin = new ApiLoginModelDTO();
                userLogin.Email = request.Email;
                userLogin.Mobile = request.Mobile;
                userLogin.Password = request.Password;
                var user = _authService.AuthenticateUser(userLogin, out string token);
                var AllUser = _mapper.Map<AllUserDTO>(user);
                baseResponse.data = new
                {
                    AllUser,
                    token
                };
                baseResponse.statusCode = StatusCodes.Status200OK;
                baseResponse.done = true;
                return Ok(baseResponse);
            }

            catch (Exception ex)
            {
                baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                baseResponse.done = false;
                baseResponse.message = $"Exception :{ex.Message}";

                return StatusCode((int) HttpStatusCode.BadRequest, baseResponse);
            }

            return BadRequest("Invalid Username or Password");
        }

        [AllowAnonymous]
        [HttpPost, Route("CompleteUserData")]
        [ProducesResponseType(typeof(UserWithTokenDTO), StatusCodes.Status200OK)]
        public IActionResult CompleteUserData([FromForm] CompleteUserDTO request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var User = _uow.UserRepository.GetMany(c => c.Id == request.UserId).FirstOrDefault();
                    if (User != null)
                    {
                        if (User.IsAccountVerified)
                        {
                            if (request.PersonalImage != null)
                            {
                                var IsPersonalImage = FileCheckHelper.IsImage(request.PersonalImage.OpenReadStream());
                                var Is1BiggerThan10MB = CheckFileSizeHelper.IsBeggerThan10MB(request.PersonalImage);
                                if (!IsPersonalImage)
                                {
                                    return BadRequest(new {Erorr = "Personal Image Only Images Allowed"});
                                }

                                if (Is1BiggerThan10MB)
                                {
                                    return BadRequest(
                                        new {Erorr = " Personal Image Only Images Less Than 10MB Allowed"});
                                }
                            }

                            if (request.NationalID != null)
                            {
                                var IsNationalID = FileCheckHelper.IsImage(request.NationalID.OpenReadStream());
                                var Is1BiggerThan10MB = CheckFileSizeHelper.IsBeggerThan10MB(request.NationalID);
                                if (!IsNationalID)
                                {
                                    return BadRequest(new {Erorr = "NationalID Only Images Allowed"});
                                }

                                if (Is1BiggerThan10MB)
                                {
                                    return BadRequest(new {Erorr = " NationalID Only Images Less Than 10MB Allowed"});
                                }
                            }

                            User.GenderId = request.GenderId;
                            User.JourneyPlanId = request.JourneyPlanId;
                            User.Age = request.Age;
                            User.Weight = request.Weight;

                            User.Height = request.Height;
                            User.BMI = request.BMI;
                            User.IsDataComplete = request.IsDataComplete ?? false;
                            User.Name = request.Name;
                            User.IsAvailable = request.IsAvailable ?? false;

                            var FC = FileHelper.CreateFolder(_hostingEnvironment,
                                UploadPathes.DocsPath + "/" + User.Mobile);

                            if (request.PersonalImage != null)
                            {
                                var PersonalImage = FileHelper.FileUpload(request.PersonalImage, _hostingEnvironment,
                                    UploadPathes.DocsPath + "/" + User.Mobile);
                                User.PersonalImage = PersonalImage;
                            }

                            if (request.NationalID != null)
                            {
                                var NationalID = FileHelper.FileUpload(request.NationalID, _hostingEnvironment,
                                    UploadPathes.DocsPath + "/" + User.Mobile);
                                User.NationalID = NationalID;
                                // User.IsUploadedNationalID = true;
                            }

                            _uow.UserRepository.Update(User);
                            _uow.Save();
                            if (request.UserRisk != null)
                            {
                                var lstRisk = _uow.RiskRepository.GetMany(c => request.UserRisk.Contains(c.Id))
                                    .ToList();
                                foreach (var risk in lstRisk)
                                {
                                    var userRisk = new MUserRisk();
                                    userRisk.RiskId = risk.Id;
                                    userRisk.UserId = User.Id;
                                    _uow.UserRiskRepository.Add(userRisk);
                                }

                                _uow.Save();
                            }

                            if (request.UserAllergy != null)
                            {
                                var lstAllergy = _uow.AllergyRepository.GetMany(c => request.UserAllergy.Contains(c.Id))
                                    .ToList();
                                foreach (var allergy in lstAllergy)
                                {
                                    var userAllergy = new MUserAllergy();
                                    userAllergy.AllergyId = allergy.Id;
                                    userAllergy.UserId = User.Id;
                                    _uow.UserAllergyRepository.Add(userAllergy);
                                }

                                _uow.Save();
                            }

                            VerifyCodeHelper verifyCodeHelper = new VerifyCodeHelper(_uow, _SMS);
                            verifyCodeHelper.SendOTP(User.Mobile, User.Id);
                            var userLogin = new ApiLoginModelDTO();
                            userLogin.Mobile = User.Mobile;
                            userLogin.Password = User.Password;

                            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
                            if (User.PersonalImage != null && User.PersonalImage != string.Empty)
                            {
                                User.PersonalImage = dbConn +
                                                     $"https://api.nutrisha.app/Files/Documents/{User.Id}/{User.PersonalImage}";
                            }
                            else
                            {
                                User.PersonalImage = string.Empty;
                            }

                            if (User.NationalID != null && User.NationalID != string.Empty)
                            {
                                User.NationalID = dbConn +
                                                  $"https://api.nutrisha.app/Files/Documents/{User.Id}/{User.NationalID}";
                            }
                            else
                            {
                                User.NationalID = string.Empty;
                            }

                            var verfiyCodeLast = _uow.VerfiyCodeRepository
                                .GetMany(c => c.Email == User.Email || c.Mobile == User.Mobile).FirstOrDefault();
                            if (verfiyCodeLast != null)
                            {
                                User.VerfiyCode = verfiyCodeLast.VirfeyCode;
                            }

                            var AllUser = _mapper.Map<AllUserDTO>(User);
                            baseResponse.data = new
                            {
                                AllUser,
                            };
                            baseResponse.statusCode = StatusCodes.Status200OK;
                            baseResponse.total_rows = 1;
                            baseResponse.done = true;
                            return Ok(baseResponse);
                        }
                        else
                        {
                            baseResponse.done = false;
                            baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                            baseResponse.message = "Account Not Verfied";
                            return BadRequest(baseResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                    baseResponse.done = false;
                    baseResponse.message = $"Exception :{ex.Message}";

                    return StatusCode((int) HttpStatusCode.BadRequest, baseResponse);
                }
            }

            return BadRequest("Invalid Username or Password");
        }

        [HttpPost, Route("GetVerifyCode")]
        [ProducesResponseType(typeof(MVerfiyCode), StatusCodes.Status200OK)]
        public IActionResult GetVerifyCode(UserMobileEmaiDTO userMobileEmaiDTO)
        {
            if (!string.IsNullOrEmpty(userMobileEmaiDTO.Mobile))
            {
                var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Mobile == userMobileEmaiDTO.Mobile)
                    .FirstOrDefault();
                if (verfiyCode != null)
                {
                    int num = new Random().Next(1000, 9999);
                    verfiyCode.VirfeyCode = num;
                    verfiyCode.Date = DateTime.Now.AddMinutes(5);
                    _uow.VerfiyCodeRepository.Update(verfiyCode);
                    _uow.Save();
                }
                else
                {
                    int num = new Random().Next(1000, 9999);
                    verfiyCode = new MVerfiyCode
                        {Date = DateTime.Now.AddMinutes(5), Mobile = userMobileEmaiDTO.Mobile, VirfeyCode = num};
                    _uow.VerfiyCodeRepository.Add(verfiyCode);
                    _uow.Save();
                }

                baseResponse.data = verfiyCode;
                baseResponse.total_rows = 1;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else if (!string.IsNullOrEmpty(userMobileEmaiDTO.Email))
            {
                var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Email == userMobileEmaiDTO.Email)
                    .FirstOrDefault();
                if (verfiyCode != null)
                {
                    int num = new Random().Next(1000, 9999);
                    verfiyCode.VirfeyCode = num;
                    verfiyCode.Date = DateTime.Now.AddMinutes(5);
                    _uow.VerfiyCodeRepository.Update(verfiyCode);
                    _uow.Save();

                    _mailService.SendWelcomeEmailAsync(new WelcomeRequest
                    {
                        ToEmail = userMobileEmaiDTO.Email, UserName = userMobileEmaiDTO.Email, Id = 0,
                        VerifyCode = num.ToString()
                    });
                }
                else
                {
                    int num = new Random().Next(1000, 9999);
                    verfiyCode = new MVerfiyCode
                        {Date = DateTime.Now.AddMinutes(5), Email = userMobileEmaiDTO.Email, VirfeyCode = num};
                    _uow.VerfiyCodeRepository.Add(verfiyCode);
                    _uow.Save();
                    _mailService.SendWelcomeEmailAsync(new WelcomeRequest
                    {
                        ToEmail = userMobileEmaiDTO.Email, UserName = userMobileEmaiDTO.Email, Id = 0,
                        VerifyCode = num.ToString()
                    });
                }

                baseResponse.data = verfiyCode;
                baseResponse.total_rows = 1;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse.data = "must insert mail or mobile";
                baseResponse.total_rows = 0;
                baseResponse.statusCode = (int) HttpStatusCode.NotFound; // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
        }

        [HttpPost, Route("CheckVerifyCode")]
        [ProducesResponseType(typeof(AllUserDTO), StatusCodes.Status200OK)]
        public IActionResult CheckAccountVerified(CheckVerifyCodeDto checkVerifyCode)
        {
            var user = _uow.UserRepository.GetMany(c =>
                (!string.IsNullOrEmpty(checkVerifyCode.Email) && c.Email == checkVerifyCode.Email) ||
                (!string.IsNullOrEmpty(checkVerifyCode.Mobile) && c.Mobile == checkVerifyCode.Mobile)).FirstOrDefault();
            if (user != null)
            {
                if (!string.IsNullOrEmpty(checkVerifyCode.Mobile))
                {
                    var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Mobile == checkVerifyCode.Mobile)
                        .FirstOrDefault();
                    if (verfiyCode != null)
                    {
                        if (checkVerifyCode.VirfeyCode == verfiyCode.VirfeyCode && verfiyCode.Date > DateTime.Now)
                        {
                            user.VerfiyCode = checkVerifyCode.VirfeyCode;
                            user.IsAccountVerified = true;
                            _uow.UserRepository.Update(user);
                            _uow.Save();
                            user.Password = null;
                            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
                            if (user.PersonalImage != null && user.PersonalImage != string.Empty)
                            {
                                user.PersonalImage = dbConn +
                                                     $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
                            }
                            else
                            {
                                user.PersonalImage = string.Empty;
                            }

                            if (user.NationalID != null && user.NationalID != string.Empty)
                            {
                                user.NationalID = dbConn +
                                                  $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
                            }
                            else
                            {
                                user.NationalID = string.Empty;
                            }

                            var AllUser = _mapper.Map<AllUserDTO>(user);

                            baseResponse.data = AllUser;

                            baseResponse.message = "Account Verified";
                            baseResponse.total_rows = 1;
                            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                            baseResponse.done = true;

                            return Ok(baseResponse);
                        }
                        else
                        {
                            baseResponse.message = "InValied Verify Code";
                            baseResponse.total_rows = 0;
                            baseResponse.statusCode = (int) HttpStatusCode.BadRequest; // Errors.Success;
                            baseResponse.done = false;
                            return BadRequest(baseResponse);
                        }
                    }
                    else
                    {
                        baseResponse.message = "InValied Verify Code";
                        baseResponse.total_rows = 0;
                        baseResponse.statusCode = (int) HttpStatusCode.BadRequest; // Errors.Success;
                        baseResponse.done = false;
                        return BadRequest(baseResponse);
                    }
                }
                else if (!string.IsNullOrEmpty(checkVerifyCode.Email))
                {
                    var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Email == checkVerifyCode.Email)
                        .FirstOrDefault();
                    if (verfiyCode != null)
                    {
                        if (checkVerifyCode.VirfeyCode == verfiyCode.VirfeyCode && verfiyCode.Date > DateTime.Now)
                        {
                            user.VerfiyCode = checkVerifyCode.VirfeyCode;
                            user.IsAccountVerified = true;
                            _uow.UserRepository.Update(user);
                            _uow.Save();
                            user.Password = null;
                            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
                            if (user.PersonalImage != null && user.PersonalImage != string.Empty)
                            {
                                user.PersonalImage = dbConn +
                                                     $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
                            }
                            else
                            {
                                user.PersonalImage = string.Empty;
                            }

                            if (user.NationalID != null && user.NationalID != string.Empty)
                            {
                                user.NationalID = dbConn +
                                                  $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
                            }
                            else
                            {
                                user.NationalID = string.Empty;
                            }

                            var AllUser = _mapper.Map<AllUserDTO>(user);

                            baseResponse.data = AllUser;

                            baseResponse.message = "Account Verified";
                            baseResponse.total_rows = 1;
                            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                            baseResponse.done = true;
                            return Ok(baseResponse);
                        }
                        else
                        {
                            baseResponse.message = "InValied Verify Code";
                            baseResponse.total_rows = 0;
                            baseResponse.statusCode = (int) HttpStatusCode.BadRequest; // Errors.Success;
                            baseResponse.done = false;
                            return BadRequest(baseResponse);
                        }
                    }
                    else
                    {
                        baseResponse.message = "InValied Verify Code";
                        baseResponse.total_rows = 0;
                        baseResponse.statusCode = (int) HttpStatusCode.BadRequest; // Errors.Success;
                        baseResponse.done = false;
                        return BadRequest(baseResponse);
                    }
                }
                else
                {
                    baseResponse.message = "must insert Email or mobile";
                    baseResponse.total_rows = 0;
                    baseResponse.statusCode = (int) HttpStatusCode.BadRequest; // Errors.Success;
                    baseResponse.done = false;
                    return BadRequest(baseResponse);
                }
            }
            else
            {
                baseResponse.message = "must insert Valied Email or mobile";
                baseResponse.statusCode = (int) HttpStatusCode.BadRequest; // Errors.Success;
                baseResponse.done = false;
                return BadRequest(baseResponse);
            }
        }

        [HttpGet, Route("GetAccountVerified")]
        [ProducesResponseType(typeof(ChangeAccountVerifiedDto), StatusCodes.Status200OK)]
        public IActionResult GetAccountVerified(int userId)
        {
            var user = _uow.UserRepository.GetMany(c => c.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var availability = new ChangeAccountVerifiedDto();
                // availability.Mobile = user.Mobile;
                availability.UserId = user.Id;
                availability.IsAccountVerified = user.IsAccountVerified;
                baseResponse.data = availability;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound; // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
        }

        [Authorize]
        [HttpPost, Route("ChangeAccountVerified")]
        [ProducesResponseType(typeof(ChangeAccountVerifiedDto), StatusCodes.Status200OK)]
        public IActionResult ChangeAccountVerified(ChangeAccountVerifiedDto changeAccountVerifiedDto)
        {
            var user = _uow.UserRepository.GetMany(c => c.Id == changeAccountVerifiedDto.UserId).FirstOrDefault();
            if (user != null)
            {
                user.IsAccountVerified = changeAccountVerifiedDto.IsAccountVerified;
                _uow.UserRepository.Update(user);
                _uow.Save();
                var availability = new ChangeAccountVerifiedDto();
                //  availability.Mobile = user.Mobile;
                availability.UserId = user.Id;
                availability.IsAccountVerified = user.IsAccountVerified;
                baseResponse.data = availability;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound; // Errors.Success;
                baseResponse.done = false;
                baseResponse.message = "Invalid User";
                return Ok(baseResponse);
            }
        }

        [HttpGet, Route("GetAvailability")]
        [ProducesResponseType(typeof(ChangeAvailabilityDto), StatusCodes.Status200OK)]
        public IActionResult GetAvailability(int userId)
        {
            var user = _uow.UserRepository.GetMany(c => c.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var availability = new ChangeAvailabilityDto();
                // availability.Mobile = user.Mobile;
                availability.UserId = user.Id;
                availability.IsAvailable = user.IsAvailable;
                baseResponse.data = availability;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound; // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
        }

        [Authorize]
        [HttpPost, Route("ChangeAvailability")]
        [ProducesResponseType(typeof(ChangeAvailabilityDto), StatusCodes.Status200OK)]
        public IActionResult ChangeAvailability(ChangeAvailabilityDto changeAvailabilityDto)
        {
            var user = _uow.UserRepository.GetMany(c => c.Id == changeAvailabilityDto.UserId).FirstOrDefault();
            if (user != null)
            {
                user.IsAvailable = changeAvailabilityDto.IsAvailable;
                _uow.UserRepository.Update(user);
                _uow.Save();
                var availability = new ChangeAvailabilityDto();
                //  availability.Mobile = user.Mobile;
                availability.UserId = user.Id;
                availability.IsAvailable = user.IsAvailable;
                baseResponse.data = availability;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound; // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
        }

        //[Authorize]
        [HttpPost, Route("ChangeUserDeviceToken")]
        [ProducesResponseType(typeof(ChangeUserDeviceTokenDto), StatusCodes.Status200OK)]
        public IActionResult ChangeUserDeviceToken(ChangeUserDeviceTokenDto changeUserDeviceTokenDto)
        {
            var user = _uow.UserRepository.GetMany(c => c.Id == changeUserDeviceTokenDto.UserId).FirstOrDefault();
            if (user != null)
            {
                user.DeviceToken = changeUserDeviceTokenDto.DeviceToken;
                user.DeviceTypeId = changeUserDeviceTokenDto.DeviceTypeId;
                user.Language = changeUserDeviceTokenDto.Language;
                _uow.UserRepository.Update(user);
                _uow.Save();
                var userDeviceToken = new ChangeUserDeviceTokenDto();
                //  availability.Mobile = user.Mobile;
                userDeviceToken.UserId = user.Id;
                userDeviceToken.DeviceTypeId = changeUserDeviceTokenDto.DeviceTypeId;
                userDeviceToken.DeviceToken = changeUserDeviceTokenDto.DeviceToken;
                baseResponse.data = userDeviceToken;
                baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                baseResponse.done = true;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse.data = "";
                baseResponse.statusCode = (int) HttpStatusCode.NotFound; // Errors.Success;
                baseResponse.done = false;
                return Ok(baseResponse);
            }
        }

        [NonAction]
        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }

        [AllowAnonymous]
        [HttpPost, Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var User = _uow.UserRepository.GetMany(a => a.Id == changePasswordDto.UserId).FirstOrDefault();
                    if (User != null)
                    {
                        var olsPassword = User.Password;
                        var newPassword = EncryptANDDecrypt.EncryptText(changePasswordDto.NewPassword);
                        if (olsPassword != newPassword)
                        {
                            User.Password = EncryptANDDecrypt.EncryptText(changePasswordDto.NewPassword);
                            _uow.UserRepository.Update(User);
                            _uow.Save();
                            baseResponse.data = "";
                            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                            baseResponse.done = true;
                            baseResponse.message = "Password Changed";
                            return Ok(baseResponse);
                        }
                        else
                        {
                            baseResponse.data = "";
                            baseResponse.statusCode = (int) HttpStatusCode.NotAcceptable;
                            baseResponse.done = false;
                            baseResponse.message = "Not Valied Password";
                            return Ok(baseResponse);
                        }
                    }

                    baseResponse.data = "";
                    baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                    baseResponse.done = false;
                    baseResponse.message = "Worng User Id";

                    return StatusCode((int) HttpStatusCode.BadRequest, baseResponse);
                }
                catch (Exception ex)
                {
                    baseResponse.data = "";
                    baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                    baseResponse.done = false;
                    baseResponse.message = $"Exception :{ex.Message}";

                    return StatusCode((int) HttpStatusCode.BadRequest, baseResponse);
                }
            }

            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost, Route("ForgetPasswordByEmail")]
        [ProducesResponseType(typeof(AllUserDTO), StatusCodes.Status200OK)]
        public IActionResult ForgetPasswordByEmailPost(ForgetPasswordByEmailDTO forgetPasswordDTO)
        {
            // var IdDec = EncryptANDDecrypt.DecryptText(forgetPasswordDTO.EncId);
            if (!string.IsNullOrEmpty(forgetPasswordDTO.Email))
            {
                var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Email == forgetPasswordDTO.Email)
                    .FirstOrDefault();

                var user = _uow.UserRepository.GetMany(a => a.Email == forgetPasswordDTO.Email).FirstOrDefault();
                if (user != null)
                {
                    if (!user.IsAccountVerified)
                    {
                        baseResponse.data = "";
                        baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                        baseResponse.done = false;
                        baseResponse.message = "please verify your account first.";
                        return BadRequest(baseResponse);
                    }

                    if (verfiyCode != null)
                    {
                        // BAD CODE?? I know i just a fixer :) no time to refactor # TAWFIQ #
                        if (verfiyCode.Date > DateTime.Now)
                        {
                            var oldPassword = user.Password;
                            var newPassword = EncryptANDDecrypt.EncryptText(forgetPasswordDTO.NewPassword);
                            if (oldPassword == newPassword)
                            {
                                baseResponse.data = "";
                                baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                                baseResponse.done = false;
                                baseResponse.message = "new password shouldn't be one of your old passwords.";
                                return BadRequest(baseResponse);
                            }

                            user.Password = newPassword;
                            _uow.UserRepository.Update(user);
                            _uow.Save();
                            user.VerfiyCode = forgetPasswordDTO.VerifyCode;
                            user.Password = null;
                            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
                            if (user.PersonalImage != null && user.PersonalImage != string.Empty)
                            {
                                user.PersonalImage = dbConn +
                                                     $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
                            }
                            else
                            {
                                user.PersonalImage = string.Empty;
                            }

                            if (user.NationalID != null && user.NationalID != string.Empty)
                            {
                                user.NationalID = dbConn +
                                                  $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
                            }
                            else
                            {
                                user.NationalID = string.Empty;
                            }

                            var AllUser = _mapper.Map<AllUserDTO>(user);

                            baseResponse.data = AllUser;
                            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                            baseResponse.done = true;
                            baseResponse.message = "Password has changed successfully";
                            return Ok(baseResponse);
                        }
                        else
                        {
                            baseResponse.data = "";
                            baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                            baseResponse.done = false;
                            baseResponse.message = "asking the user to reset the pass again";
                        }
                    }
                }
            }

            baseResponse.data = "";
            baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
            baseResponse.done = false;
            baseResponse.message = "Wrong credentials";

            return StatusCode((int) HttpStatusCode.BadRequest, baseResponse);
        }

        [AllowAnonymous]
        [HttpPost, Route("ForgetPasswordByMobile")]
        [ProducesResponseType(typeof(AllUserDTO), StatusCodes.Status200OK)]
        public IActionResult ForgetPasswordByMobilePost(ForgetPasswordByMobilDTO forgetPasswordDTO)
        {
            if (!string.IsNullOrEmpty(forgetPasswordDTO.Mobile))
            {
                var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Mobile == forgetPasswordDTO.Mobile)
                    .FirstOrDefault();
                var user = _uow.UserRepository.GetMany(c => c.Mobile == forgetPasswordDTO.Mobile).FirstOrDefault();
                if (user != null)
                {
                    if (verfiyCode != null)
                    {
                        if (verfiyCode.Date > DateTime.Now)
                        {
                            user.Password = EncryptANDDecrypt.EncryptText(forgetPasswordDTO.NewPassword);
                            _uow.UserRepository.Update(user);
                            _uow.Save();
                            user.VerfiyCode = forgetPasswordDTO.VerifyCode;
                            user.Password = null;
                            string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
                            if (user.PersonalImage != null && user.PersonalImage != string.Empty)
                            {
                                user.PersonalImage = dbConn +
                                                     $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
                            }
                            else
                            {
                                user.PersonalImage = string.Empty;
                            }

                            if (user.NationalID != null && user.NationalID != string.Empty)
                            {
                                user.NationalID = dbConn +
                                                  $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
                            }
                            else
                            {
                                user.NationalID = string.Empty;
                            }

                            var AllUser = _mapper.Map<AllUserDTO>(user);

                            baseResponse.data = AllUser;
                            baseResponse.statusCode = (int) HttpStatusCode.OK; // Errors.Success;
                            baseResponse.done = true;
                            baseResponse.message = "Password has changed successfully";
                            return Ok(baseResponse);
                        }
                        else
                        {
                            baseResponse.data = "";
                            baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
                            baseResponse.done = false;
                            baseResponse.message = "asking the user to reset the pass again";
                        }
                    }
                }
            }

            baseResponse.data = "";
            baseResponse.statusCode = (int) HttpStatusCode.BadRequest;
            baseResponse.done = false;
            baseResponse.message = "Wrong credentials";

            return StatusCode((int) HttpStatusCode.BadRequest, baseResponse);
        }

        [AllowAnonymous]
        [HttpGet, Route("ActivateAccountOTP")]
        public IActionResult ActivateViaCode(int VirifyCode)
        {
            VerifyCodeHelper verifyCodeHelper = new VerifyCodeHelper(_uow, _SMS);
            var Result = verifyCodeHelper.ActivateOTP(VirifyCode);

            if (Result)
            {
                return Ok(Result);
            }

            return BadRequest();
        }


        //[AllowAnonymous]
        //[HttpPost, Route("ForgetPassword")]
        //[ProducesResponseType(typeof(AllUserDTO), StatusCodes.Status200OK)]
        //public IActionResult ForgetPasswordPost(ForgetPasswordDTO forgetPasswordDTO)
        //{
        //    // var IdDec = EncryptANDDecrypt.DecryptText(forgetPasswordDTO.EncId);
        //    if (!string.IsNullOrEmpty(forgetPasswordDTO.Email))
        //    {
        //        var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Email == forgetPasswordDTO.Email).FirstOrDefault();

        //        var user = _uow.UserRepository.GetMany(a => a.Email == forgetPasswordDTO.Email).FirstOrDefault();
        //        if (user != null)
        //        {

        //            if (verfiyCode != null)
        //            {
        //                if (verfiyCode.Date > DateTime.Now)
        //                {
        //                    user.Password = EncryptANDDecrypt.EncryptText(forgetPasswordDTO.NewPassword);
        //                    _uow.UserRepository.Update(user);
        //                    _uow.Save();
        //                    user.VerfiyCode = forgetPasswordDTO.VerifyCode;
        //                    user.Password = null;
        //                    string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
        //                    if (user.PersonalImage != null && user.PersonalImage != string.Empty)
        //                    {
        //                        user.PersonalImage = dbConn + $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
        //                    }
        //                    else
        //                    {
        //                        user.PersonalImage = string.Empty;
        //                    }
        //                    if (user.NationalID != null && user.NationalID != string.Empty)
        //                    {
        //                        user.NationalID = dbConn + $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
        //                    }
        //                    else
        //                    {
        //                        user.NationalID = string.Empty;
        //                    }
        //                    var AllUser = _mapper.Map<AllUserDTO>(user);

        //                    baseResponse.data = AllUser;
        //                    baseResponse.statusCode = (int)HttpStatusCode.OK;  // Errors.Success;
        //                    baseResponse.done = true;
        //                    baseResponse.message = "Password has changed successfully";
        //                    return Ok(baseResponse);
        //                }
        //                else
        //                {
        //                    baseResponse.data = "";
        //                    baseResponse.statusCode = (int)HttpStatusCode.BadRequest;
        //                    baseResponse.done = false;
        //                    baseResponse.message = "asking the user to reset the pass again";
        //                }
        //            }
        //        }

        //    }
        //    if (!string.IsNullOrEmpty(forgetPasswordDTO.Mobile))
        //    {
        //        var verfiyCode = _uow.VerfiyCodeRepository.GetMany(c => c.Mobile == forgetPasswordDTO.Mobile).FirstOrDefault();
        //        var user = _uow.UserRepository.GetMany(c => c.Mobile == forgetPasswordDTO.Mobile).FirstOrDefault();
        //        if (user != null)
        //        {
        //            if (verfiyCode != null)
        //            {
        //                if (verfiyCode.Date > DateTime.Now)
        //                {
        //                    user.Password = EncryptANDDecrypt.EncryptText(forgetPasswordDTO.NewPassword);
        //            _uow.UserRepository.Update(user);
        //            _uow.Save();
        //            user.VerfiyCode = forgetPasswordDTO.VerifyCode;
        //                    user.Password = null;
        //                    string dbConn = configuration.GetSection("Setting").GetSection("FilePath").Value;
        //                    if (user.PersonalImage != null && user.PersonalImage != string.Empty)
        //                    {
        //                        user.PersonalImage = dbConn + $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.PersonalImage}";
        //                    }
        //                    else
        //                    {
        //                        user.PersonalImage = string.Empty;
        //                    }
        //                    if (user.NationalID != null && user.NationalID != string.Empty)
        //                    {
        //                        user.NationalID = dbConn + $"https://api.nutrisha.app/Files/Documents/{user.Id}/{user.NationalID}";
        //                    }
        //                    else
        //                    {
        //                        user.NationalID = string.Empty;
        //                    }
        //                    var AllUser = _mapper.Map<AllUserDTO>(user);

        //                    baseResponse.data = AllUser;
        //                    baseResponse.statusCode = (int)HttpStatusCode.OK;  // Errors.Success;
        //            baseResponse.done = true;
        //            baseResponse.message = "Password has changed successfully";
        //            return Ok(baseResponse);
        //                }
        //                else
        //                {
        //                    baseResponse.data = "";
        //                    baseResponse.statusCode = (int)HttpStatusCode.BadRequest;
        //                    baseResponse.done = false;
        //                    baseResponse.message = "asking the user to reset the pass again";
        //                }
        //            }
        //        }

        //    }
        //    baseResponse.data = "";
        //    baseResponse.statusCode = (int)HttpStatusCode.BadRequest;
        //    baseResponse.done = false;
        //    baseResponse.message = "Wrong credentials";

        //    return StatusCode((int)HttpStatusCode.BadRequest, baseResponse);
        //}
    }
}