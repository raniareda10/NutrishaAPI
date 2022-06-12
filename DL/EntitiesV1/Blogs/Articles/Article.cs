using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DL.DtosV1.Common;

namespace DL.EntitiesV1.Blogs.Articles
{
    public class Article
    {
        [ForeignKey(nameof(Blog))] public long Id { get; set; }
        public Blog Blog { get; set; }
        public LocalizedObject<string> Description { get; set; }
    }
}