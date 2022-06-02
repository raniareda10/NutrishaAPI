using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NutrishaAPI.Validations.Shared
{
    public static class EmailValidator
    {
        public static bool IsValidEmail(this string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}