using System.Linq;
using DL.DtosV1.BlogVideo;
using DL.EntitiesV1.Media;
using DL.ResultModels;
using NutrishaAPI.Validations.Models;

namespace NutrishaAPI.Validations.BLogVideo
{
    public static class PostToBLogVideoValidator
    {
        public const int MaxSubjectCharacters = 100;
        public static ValidationResult IsValid(this PostBlogVideoDto model)
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
            
            if (model.Files.All(f => f.Flags?.Contains(MediaFlags.MainMedia) == false))
            {
                result.AddError("Please Add Correct Video.");
                return result;
            }

            return result;
        }
    }
}