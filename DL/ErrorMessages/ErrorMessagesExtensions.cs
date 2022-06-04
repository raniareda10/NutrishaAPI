using System.Collections.Generic;

namespace DL.ErrorMessages
{
    public static class ErrorMessagesExtensions
    {
        private static readonly IDictionary<string, IDictionary<ErrorMessagesKeys, string>> LocaleMapper =
            new Dictionary<string, IDictionary<ErrorMessagesKeys, string>>()
            {
                {"ar", ArabicErrorMessages.ArabicMessages},
                {"en", EnglishErrorMessages.EnglishMessages}
            };

        public static string Localize(this ErrorMessagesKeys key, string locale)
        {
            return LocaleMapper[locale][key];
        }
    }
}