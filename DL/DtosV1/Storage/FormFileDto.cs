using System.Collections.Generic;
using DL.HelperInterfaces;
using Microsoft.AspNetCore.Http;

namespace DL.DtosV1.Storage
{
    public class FormFileDto : IMediaFLags
    {
        public IFormFile File { get; set; }
        public HashSet<string> Flags { get; set; }
    }
}