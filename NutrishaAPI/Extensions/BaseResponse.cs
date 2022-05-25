namespace NutrishaAPI.Extensions
{
    public class BaseResponseHelper
    {
        public object data { get; set; }
        public bool done { get; set; }
        public int total_rows { get; set; }

        private Errors Error = Errors.Success;
        public int statusCode { get; set; }
        public string message { get; set; } = string.Empty;
    }


    public enum Errors
    {
        Success,
        TheModelIsInvalid,
        SomeThingWentwrong,
        TheUserNotExistOrDeleted,
        UserIsDeleted,
        UserIsPending,
        UserIsRejected,
        ThisPhoneNumberAlreadyExist,
        ThisPhoneNumberNotExist,
        ThisEmailAlreadyExist,
        ThePhoneOrPasswordIsIncorrect,
        TheOldPasswordIsInCorrect,
        TheOrderNotExistOrDeleted,
        ThisOrderAssignedToAnotherAgent,
        TheVerificationCodeUnvalid,
        UserInActive,
        ThisEmailNotExist,
        ThisCountryNotExistOrDeleted,
        PromoCodeAndPointModuleNotActiveNow,
        ThisConsultationTypeNotExistOrDeleted,
        YouDoNotHaveMoneyToRequsetThisConsultation,
        WrongFacebookAccessToken,
        ThisMedicalProfileNotExist,
        ThisBookingTimeNotExistOrDeleted,
        SorryThisBookingTimeAlreadyBooked,
        TheMessageIsEmpty
    }
}