using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DL.DtosV1.Articles;
using DL.EntitiesV1.Media;
using DL.HelperInterfaces;
using DL.ResultModels;
using Microsoft.AspNetCore.Http;
using NutrishaAPI.Validations.Models;

namespace NutrishaAPI.Validations.Articles
{
    public static class PostArticleValidator
    {
        public const int MaxSubjectCharacters = 120;
        public static ValidationResult IsValid(this PostArticleDto model)
        {
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(model.Subject) || model.Subject.Length > MaxSubjectCharacters)
            {
                result.AddError(NonLocalizedErrorMessages.InvalidSubject);
                return result;
            }


            if (!model.IsValidFiles())
            {
                result.AddError("Please Add Valid Files");
                return result;
            }
            if (model.Files == null || model.Files.Count < 1)
            {
                result.AddError("Please Add Cover Image.");
                return result;
            }
            
            if (model.Files.All(f => f.Flags?.Contains(MediaFlags.CoverImage) == false))
            {
                result.AddError("Please Add Cover Image.");
                return result;
            }

            return result;
        }
    }
}