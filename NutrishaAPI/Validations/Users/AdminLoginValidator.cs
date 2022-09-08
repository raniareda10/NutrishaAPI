using DL.DtosV1.Users.Admins;
using DL.HelperInterfaces;
using DL.ResultModels;
using Microsoft.AspNetCore.Http;
using NutrishaAPI.Validations.Models;
using NutrishaAPI.Validations.Shared;
using Org.BouncyCastle.Asn1.Pkcs;

namespace NutrishaAPI.Validations.Users
{
    public static class AdminLoginValidator
    {
        public static ValidationResult IsValid(this ILoginIn loginModel)
        {
            loginModel.Password = loginModel.Password.Trim();
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                result.AddError(NonLocalizedErrorMessages.WrongCredential);
                return result;
            }

            if (!loginModel.Email.IsValidEmail())
            {
                result.AddError(NonLocalizedErrorMessages.WrongCredential);
                return result;
            }

            return result;
        }
    }
}