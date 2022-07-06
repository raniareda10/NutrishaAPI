using DL.DtosV1.Polls;
using NutrishaAPI.Validations.Models;
using NutrishaAPI.Validations.Shared;

namespace NutrishaAPI.Validations.Polls
{
    public static class PollValidator
    {
        public const int MaxQuestionLength = 120;
        public static ValidationResult IsValid(this PostPollDto postPollDto)
        {
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(postPollDto.Question) || 
                postPollDto.Question.Length >  MaxQuestionLength)
            {
                result.AddError("Please Provide Valid Question.");
                return result;
            }
            
            if (postPollDto.Question == null || postPollDto.Answers.Count is < 2 or > 5)
            {
                result.AddError("Answers Should be More Then 1 and Less Than Or Equal 5");
                return result;
            }
            if (!postPollDto.BackgroundColor.IsValidHexColor())
            {
                result.AddError("please inter valid color");
                return result;
            }
            
            return result;
        }
    }
}