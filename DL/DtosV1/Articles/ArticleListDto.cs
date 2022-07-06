using System.Collections.Generic;
using DL.DtosV1.Blogs;
using DL.DtosV1.Users;
using DL.EntitiesV1.Media;

namespace DL.DtosV1.Articles
{
    public class ArticleListDto
    {
        public string Subject { get; set; }
        public OwnerDto Owner { get; set; }
        public string Description { get; set; }
        public IDictionary<string, int> Totals { get; set; }
        public BlogTagDto Tag { get; set; }
        public IEnumerable<MediaFile> Media { get; set; }
    }
}