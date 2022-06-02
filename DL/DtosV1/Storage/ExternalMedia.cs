using System.Collections.Generic;
using DL.HelperInterfaces;

namespace DL.DtosV1.Storage
{
    public class ExternalMedia : IMediaFLags
    {
        public string Url { get; set; }

        public HashSet<string> Flags { get; set; }
    }
}