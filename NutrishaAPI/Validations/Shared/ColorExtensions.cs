using System.Text.RegularExpressions;

namespace NutrishaAPI.Validations.Shared
{
    public static class ColorExtensions
    {
        public static bool IsValidHexColor(this string color)
        {
            return !(string.IsNullOrWhiteSpace(color) ||
                   color.Length > 7 ||
                   !Regex.IsMatch(color, "#[0-9a-f]{6}$", RegexOptions.IgnoreCase));
        }
    }
}