using DL.DtosV1.Common;

namespace NutrishaAPI.Validations.Shared
{
    public static class LocalizedObjectExtensions
    {
        public static bool HasAtLeastOneLanguage(this LocalizedObject<string> obj)
        {
            if (obj == null) return false;
            var hasEnglish = !string.IsNullOrWhiteSpace(obj.En);

            if (hasEnglish) return true;

            return !string.IsNullOrWhiteSpace(obj.Ar);
        } 
    }
}