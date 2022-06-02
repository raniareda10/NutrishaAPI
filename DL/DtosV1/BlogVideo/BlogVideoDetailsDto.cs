using System.Collections.Generic;
using DL.EntitiesV1.Blogs;
using DL.EntitiesV1.Media;

namespace DL.DtosV1.BlogVideo
{
    public class BlogVideoDetailsDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public long TagId { get; set; }
        public IList<MediaFile> Media { get; set; }
    }
}