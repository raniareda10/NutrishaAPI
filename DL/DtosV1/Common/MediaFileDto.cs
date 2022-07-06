using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DL.DtosV1.Common
{
    public class MediaFileDto
    {
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public HashSet<MediaFileFlag> Flags { get; set; }
        public bool IsNew { get; set; }
    }

    public enum MediaFileFlag
    {
        MainMedia = 0,
        CoverImage = 1
    }
}