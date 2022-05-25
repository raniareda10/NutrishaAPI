using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HELPER
{
    public class CheckFileSizeHelper
    {
        public static bool IsBeggerThan1MB(IFormFile File)
        {
            var FileSize = File.Length;
            var AllowedFileSize = 1000000;
            if (FileSize > AllowedFileSize)
            {
                return true;
            }
            return false;
        }
        public static bool IsBeggerThan10MB(IFormFile File)
        {
            var FileSize = File.Length;
            var AllowedFileSize = 10000000;
            if (FileSize > AllowedFileSize)
            {
                return true;
            }
            return false;
        }
    }
}
