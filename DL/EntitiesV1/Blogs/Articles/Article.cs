using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DL.EntitiesV1.Blogs.Articles
{
    public class Article
    {
        [ForeignKey(nameof(Blog))] public long Id { get; set; }
        public Blog Blog { get; set; }
        public string Description { get; set; }
    }
}