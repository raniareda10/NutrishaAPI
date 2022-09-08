using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.ApiModels;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DL.Secuirty.Enums;

namespace BL.Security
{
    public interface IAuthenticateService
    {
        MUser AuthenticateUser(ApiLoginModelDTO request, out string token);
        string GetUserToken(MUser user);
    }
    public interface ICheckUniqes 
    {
        List<string> CheckUniqeValue(UniqeDTO request);
        //List<string> CheckPersonUniqeValue(UniqeDTO request);
        //List<string> CheckappPageniqeValue(UniqeDTO request);
        //List<string> CheckCompanyUniqeValue(UniqeDTO request);
    }
    public class ChekUniqeSer : ICheckUniqes
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public ChekUniqeSer(IUnitOfWork unitOfWork, IUserManagementService service, IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
            _unitOfWork = unitOfWork;
        }
        public List<string> CheckUniqeValue(UniqeDTO request)
        {
            List<string> Erorrs = new List<string>();
            if (request.Email != null && request.Email != string.Empty)
            {
                var Email = _unitOfWork.UserRepository.GetMany(a => a.Email == request.Email && a.IsAccountVerified).FirstOrDefault();

                if (Email != null)
                {
                    Erorrs.Add("Email Already Exist");
                }
            }
            if (request.Mobile != null && request.Mobile != string.Empty)
            {
                var Mobil = _unitOfWork.UserRepository.GetMany(a => a.Mobile == request.Mobile && a.IsAccountVerified).FirstOrDefault();
                if (Mobil != null)
                {
                    Erorrs.Add("Mobile Already Exist");
                }
            }
            return Erorrs;
        }

      

        //public List<string> CheckappPageniqeValue(UniqeDTO request)
        //{
        //    List<string> Erorrs = new List<string>();
        //    ApplicationPage pagecode;
        //    if (request.Id > 0)
        //    {
        //        pagecode = _unitOfWork.ApplicationPageRepository.GetMany(a => a.pageCode == request.pageCode && a.Id != request.Id).FirstOrDefault();
        //         }
        //    else
        //    {

        //        pagecode = _unitOfWork.ApplicationPageRepository.GetMany(a => a.pageCode == request.pageCode).FirstOrDefault();
           
        //    }



        //    if (pagecode != null)
        //    {
        //        Erorrs.Add("pagecode Already Exist");
        //    }
             
        //    return Erorrs;
        //}
        //public List<string> CheckCompanyUniqeValue(UniqeDTO request)
        //{
        //    List<string> Erorrs = new List<string>();
        //    MCompany FileNum, PhoneNum;
        //    if (request.Id > 0)
        //    {
        //        FileNum = _unitOfWork.CompanyRepository.GetMany(a => a.ArabicName == request.FileNumber && a.Id != request.Id).FirstOrDefault();
        //        PhoneNum = _unitOfWork.CompanyRepository.GetMany(a => a.Phone == request.PhoneNum1 && a.Id != request.Id).FirstOrDefault();


        //    }
        //    else
        //    {

        //        FileNum = _unitOfWork.CompanyRepository.GetMany(a => a.ArabicName == request.FileNumber).FirstOrDefault();
        //        PhoneNum = _unitOfWork.CompanyRepository.GetMany(a => a.Phone == request.PhoneNum1).FirstOrDefault();
           
        //    }



        //    if (FileNum != null)
        //    {
        //        Erorrs.Add("FileNum Already Exist");
        //    }
        //    if (PhoneNum != null)
        //    {
        //        Erorrs.Add("PhoneNum Already Exist");
        //    }
             
        //    return Erorrs;
        //}
    }
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUnitOfWork unitOfWork,IUserManagementService service, IOptions<TokenManagement> tokenManagement)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
            _unitOfWork = unitOfWork;
        }
 
        public MUser AuthenticateUser(ApiLoginModelDTO request, out string token)
        {
    
            token = string.Empty;
            var user = _userManagementService.IsValidUser(request.Mobile,request.Email, request.Password);
            if (user != null)
            {
                ////////////////////


                var UserGroup = _unitOfWork.UserGroupRepository.GetMany(a => a.UserId == user.Id).Include(a => a.Group).ToHashSet();
                var Roles = new List<MRole>();
                foreach (var item in UserGroup)
                {
                    var Role = _unitOfWork.RolesGroupRepository.GetMany(a => a.GroupId == item.GroupId).Include(a => a.Role).ToHashSet();
                    foreach (var item2 in Role)
                    {
                        var IsThere = Roles.Where(a => a.Id == item2.Role.Id).FirstOrDefault();
                        if (IsThere == null)
                        {
                            Roles.Add(item2.Role);

                        }
                     
                    }
                }


                ///////////////
                List<Claim> ClaimList = new List<Claim>();
                foreach (var item in Roles)
                {
                    ClaimList.Add(new Claim(ClaimTypes.Role, item.Name));
                }   
                ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                if (request.Email != null && request.Email != string.Empty)
                {
                    ClaimList.Add(new Claim(ClaimTypes.Name, request.Email));
                }
                if (request.Mobile != null && request.Mobile != string.Empty)
                {

                    ClaimList.Add(new Claim(ClaimTypes.Name, request.Mobile));
                }
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var expireDate = DateTime.Now.AddDays(_tokenManagement.AccessExpiration);
                var tokenDiscriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(ClaimList),
                    Expires = expireDate,
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenObj = tokenHandler.CreateToken(tokenDiscriptor);
                token = tokenHandler.WriteToken(tokenObj);
            }
            return user;
        }

        public string GetUserToken(MUser user)
        {
            var claimList = new List<Claim>();
            claimList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            if (!string.IsNullOrEmpty(user.Email))
            {
                claimList.Add(new Claim(ClaimTypes.Name, user.Email));
            }
            if (!string.IsNullOrEmpty(user.Mobile))
            {
                claimList.Add(new Claim(ClaimTypes.Name, user.Mobile));
            }
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expireDate = DateTime.Now.AddDays(_tokenManagement.AccessExpiration);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = expireDate,
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenObj = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenObj);
        }
    }

    public interface IUserManagementService
    {
        MUser IsValidUser(string mobile, string email, string password);
        int? getUserId(string userName);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly IUnitOfWork _uow;
        public UserManagementService(IUnitOfWork uow) { _uow = uow; }

        public MUser IsValidUser(string Mobile,string Email, string password)
        {
            if(Email != null && Email != string.Empty)
            {
                var user = _uow.UserRepository.GetMany(ent => ent.Email.ToLower() == Email.ToLower().Trim() && ent.Password == EncryptANDDecrypt.EncryptText(password) && ent.Password == EncryptANDDecrypt.EncryptText(password)).ToHashSet();
                return user.Count() == 1 ? user.FirstOrDefault() : null;
            }
            else if(Mobile != null && Mobile != string.Empty)
            {
                var user = _uow.UserRepository.GetMany(ent => ent.Mobile.ToLower() == Mobile.ToLower().Trim() && ent.Password == EncryptANDDecrypt.EncryptText(password) && ent.Password == EncryptANDDecrypt.EncryptText(password)).ToHashSet();
                return user.Count() == 1 ? user.FirstOrDefault() : null;
            }
          else
            {
                return null;
            }
        }

       public int? getUserId(string userName)
        {
            if (userName != null && userName != string.Empty)
            {
                // Get user id by name 
                var user = _uow.UserRepository.GetMany(ent => ent.Mobile.ToLower() == userName.ToLower().Trim()  || ent.Email.ToLower() == userName.ToLower().Trim()).ToHashSet();
            return user.Count() == 1 ? user.FirstOrDefault().Id : null;
           
            }
            else
            {
                return null;
            }
        }





    }
}
