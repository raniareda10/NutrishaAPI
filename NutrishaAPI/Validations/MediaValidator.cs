using System.Linq;
using DL.HelperInterfaces;

namespace NutrishaAPI.Validations
{
    public static class MediaValidator
    {
        public static bool IsValidFiles(this IFiles files)
        {
            return files?.Files != null && files.Files.All(f => f.File != null);
        }
    }
}