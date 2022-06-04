using System.Collections.Generic;

namespace DL.ErrorMessages
{
    public class EnglishErrorMessages
    {
        public static readonly IDictionary<ErrorMessagesKeys, string> EnglishMessages =
            new Dictionary<ErrorMessagesKeys, string>()
            {
                {
                    ErrorMessagesKeys.EmailOrPhoneAlreadyExists,
                    "This Email Or Phone Already Exists, Please Try another email or login."
                }
            };
    }
}