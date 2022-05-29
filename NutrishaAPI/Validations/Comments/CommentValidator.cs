using DL.DtosV1.Comments;
using DL.ResultModels;
using NutrishaAPI.Controllers.V1.Admin.V1;
using NutrishaAPI.Validations.Models;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Validations.Comments
{
    public static class CommentValidator
    {
        private const int MaxAllowedCommentLenght = 280;
        public static ValidationResult IsValid(this PostCommentDto postCommentDto)
        {
            var result = new ValidationResult();

            if (!postCommentDto.IsValidEntityId())
            {
                result.AddError(ErrorMessages.InvalidId);
                return result;
            }

            if (string.IsNullOrWhiteSpace(postCommentDto.Content) ||
                postCommentDto.Content.Length > MaxAllowedCommentLenght)
            {
                result.AddError($"Comment Content Should be from 1 character to {MaxAllowedCommentLenght} character");
            }

            return result;
        }
    }
}