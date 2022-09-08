using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using BL.Security;
using DL.DBContext;
using DL.DTO;
using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.Enums;
using DL.ResultModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NutrishaAPI.Controllers.V1.Mobile.Bases;

namespace NutrishaAPI.Controllers.V1.Mobile
{
    [AllowAnonymous]
    public class SocialAuthController : BaseMobileController
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly AppDBContext _appDbContext;
        private readonly IMapper _mapper;

        public SocialAuthController(IMapper mapper, IAuthenticateService authenticateService, AppDBContext appDbContext)
        {
            _mapper = mapper;
            _authenticateService = authenticateService;
            _appDbContext = appDbContext;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] SocialLoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.AccessToken))
            {
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
            }

            var socialUser = (int)loginDto.Provider switch
            {
                (int)SocialProvider.Facebook => await GetFacebookUserAsync(loginDto.AccessToken),
                (int)SocialProvider.Google => await GetGoogleUserAsync(loginDto.AccessToken),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (socialUser == null) return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);

            var user = await _appDbContext.MUser.FirstOrDefaultAsync(user => user.Email == socialUser.Email);

            if (user == null)
            {
                user = new MUser();
                await _appDbContext.AddAsync(user);
            }
            else
            {
                _appDbContext.Update(user);
            }

            user.Email = socialUser.Email;
            user.Mobile = socialUser.PhoneNumber;
            user.Name = socialUser.FirstName;
            user.LastName = socialUser.LastName;
            user.PersonalImage = socialUser.ProfileImage;
            user.IsAccountVerified = true;
            user.RegistrationType = socialUser.RegistrationType;

            await _appDbContext.SaveChangesAsync();
            var token = _authenticateService.GetUserToken(user);
            return ItemResult(new
            {
                token,
                AllUser = _mapper.Map<AllUserDTO>(user)
            });
        }


        // For now keep them as functions

        public class AppleLoginModel
        {
            public string IdentityToken { get; set; }
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public string Email { get; set; }
        }

        [HttpPost("AppleLogin")]
        public async Task<IActionResult> AppleLoginAsync([FromBody] AppleLoginModel loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) && string.IsNullOrEmpty(loginDto.IdentityToken))
                return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);

            var email = GetAppleEmailFromToken(loginDto.IdentityToken);
            if (string.IsNullOrEmpty(email)) return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);

            var user = await _appDbContext.MUser.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);

            switch (user)
            {
                case null when string.IsNullOrEmpty(loginDto.Email):
                    return InvalidResult(NonLocalizedErrorMessages.InvalidParameters);
                case null:
                    user = new MUser
                    {
                        Email = loginDto.Email,
                        Name = loginDto.GivenName,
                        LastName = loginDto.FamilyName,
                        IsAccountVerified = true,
                        RegistrationType = RegistrationType.Apple
                    };
                    await _appDbContext.AddAsync(user);
                    await _appDbContext.SaveChangesAsync();
                    break;
            }

            var token = _authenticateService.GetUserToken(user);
            return ItemResult(new
            {
                token,
                AllUser = _mapper.Map<AllUserDTO>(user)
            });
        }

        #region Login Methods

        #region Facebook

        private async Task<SocialUserModel> GetFacebookUserAsync(string accessToken)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri =
                    new Uri(
                        $"https://graph.facebook.com/v14.0/me?access_token={accessToken}" +
                        $"&fields=first_name,last_name,email,picture"),
                Method = HttpMethod.Get,
            };

            var facebookUser = await GetAsync<FacebookUserModel>(request);

            if (facebookUser == null) return null;
            return new SocialUserModel()
            {
                Email = facebookUser.email,
                FirstName = facebookUser.first_name,
                LastName = facebookUser.last_name,
                ProfileImage = facebookUser.picture?.Data?.url,
                RegistrationType = RegistrationType.Facebook
            };
        }

        public class FacebookUserModel
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public FacebookPictureModel picture { get; set; }
        }

        public class FacebookPictureModel
        {
            public FacebookPictureDataModel Data { get; set; }
        }

        public class FacebookPictureDataModel
        {
            public string url { get; set; }
        }

        #endregion

        #region Google

        private JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        private async Task<SocialUserModel> GetGoogleUserAsync(string accessToken)
        {
            var canRead = _jwtSecurityTokenHandler.CanReadToken(accessToken);
            if (!canRead) return null;

            var jwt = _jwtSecurityTokenHandler.ReadJwtToken(accessToken);

            // var requestMessage = new HttpRequestMessage()
            // {
            //     RequestUri = new Uri($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}"),
            // };
            //
            // var googleUser = await GetAsync<GoogleSocialUser>(requestMessage);
            // if (googleUser == null) return null;

            return new SocialUserModel()
            {
                FirstName = jwt.Payload["given_name"].ToString(),
                LastName = jwt.Payload["family_name"].ToString(),
                Email = jwt.Payload["email"].ToString(),
                ProfileImage = jwt.Payload["picture"].ToString(),
                RegistrationType = RegistrationType.Google
            };
        }

        // public class GoogleSocialUser
        // {
        //     public string sub { get; set; }
        //     public string name { get; set; }
        //     public string given_name { get; set; }
        //     public string family_name { get; set; }
        //     public string picture { get; set; }
        //     public string email { get; set; }
        //     public string email_verified { get; set; }
        //     public string locale { get; set; }
        // }

        #endregion

        #region Apple

        // public class LoginWithAppleDto
        // {
        //     public string AuthCode { get; set; }
        //     public string AccessToken { get; set; }
        //     public string User { get; set; }
        // }

        public string GetAppleEmailFromToken(string accessToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            return jwt.Payload["email"].ToString();
        }

        private async Task<SocialUserModel> GetAppleUserAsync(string accessToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var kId = jwt.Header.Kid;
            if (kId == null) return null;

            var keys = await GetAppleSigninKeysAsync(string.Empty);

            new JwtSecurityTokenHandler()
                .ValidateToken(accessToken, new TokenValidationParameters()
                    {
                        IssuerSigningKeys = keys.GetSigningKeys()
                    },
                    out var securityToken);

            var payload = ((JwtSecurityToken)securityToken).Payload;
            return new SocialUserModel()
            {
                Email = payload["email"].ToString(),
            };
        }

        public static JsonWebKeySet ApplePublicKeys;

        private async Task<JsonWebKeySet> GetAppleSigninKeysAsync(string kid)
        {
            var key = ApplePublicKeys?.Keys.FirstOrDefault(key => key.Kid == kid);
            if (key == null)
            {
                var url = "https://appleid.apple.com/auth/keys";
                var response = await new HttpClient().GetAsync(url);
                ApplePublicKeys = new JsonWebKeySet(await response.Content.ReadAsStringAsync());
            }

            return ApplePublicKeys;
        }

        #endregion

        public async Task<T> GetAsync<T>(HttpRequestMessage message)
        {
            var client = new HttpClient();
            var responseMessage = await client.SendAsync(message);

            var responseBodyString = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.StatusCode != HttpStatusCode.OK) return default;

            return JsonConvert.DeserializeObject<T>(responseBodyString);
        }

        #endregion
    }

    public class SocialLoginDto
    {
        public string AccessToken { get; set; }
        public SocialProvider Provider { get; set; }
    }

    public enum SocialProvider
    {
        Facebook = 0,
        Google = 1,
        Apple = 2
    }

    public class SocialUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public RegistrationType RegistrationType { get; set; }
    }
}