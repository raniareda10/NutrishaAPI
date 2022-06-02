using System.Collections.Generic;
using DL.DtosV1.Storage;
using DL.HelperInterfaces;

namespace DL.DtosV1.BlogVideo
{
    public class PostBlogVideoDto : IMedia
    {
        public string Subject { get; set; }
        public long TagId { get; set; }
        public IList<FormFileDto> Files { get; set; }
        public IList<ExternalMedia> ExternalMedia { get; set; }
    }
}