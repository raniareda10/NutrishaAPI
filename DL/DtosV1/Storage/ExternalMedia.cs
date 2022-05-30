using DL.HelperInterfaces;

namespace DL.DtosV1.Storage
{
    public class ExternalMedia : IMediaFLags
    {
        public string Url { get; set; }

        public string[] Flags { get; set; }
    }
}