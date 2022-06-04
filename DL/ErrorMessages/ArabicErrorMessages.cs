using System.Collections.Generic;

namespace DL.ErrorMessages
{
    static class ArabicErrorMessages
    {
        public static readonly IDictionary<ErrorMessagesKeys, string> ArabicMessages =
            new Dictionary<ErrorMessagesKeys, string>()
            {
                {
                    ErrorMessagesKeys.EmailOrPhoneAlreadyExists,
                    "هذا البريد الكترونى او رقم التلفون موجود بالفعل من فضلك استخدم  بريد الكترونى او رقم تليفون اخر او قم بتسجيل الدخول."
                }
            };
    }
}