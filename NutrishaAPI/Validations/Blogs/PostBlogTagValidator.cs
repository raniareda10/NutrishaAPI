using System.Drawing;
using System.Text.RegularExpressions;
using DL.DtosV1.BlogTags;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NutrishaAPI.Validations.Models;
using NutrishaAPI.Validations.Shared;
using Org.BouncyCastle.Utilities.Encoders;

namespace NutrishaAPI.Validations.Blogs
{
    public static class PostBlogTagValidator
    {
        private const int MaxTagNameCharCount = 10;
        public static ValidationResult IsValid(this PostBlogTagDto model)
        {
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Length > MaxTagNameCharCount)
            {
                result.AddError("Name should be between 1 to 10 chars");
                return result;
            }

            if (!model.Color.IsValidHexColor())
            {
                result.AddError("please inter valid color");
                return result;
            }

            return result;
        }
    }
}