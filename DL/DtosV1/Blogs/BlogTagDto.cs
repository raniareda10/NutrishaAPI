using DL.EntitiesV1.Blogs;
using Org.BouncyCastle.Bcpg;

namespace DL.DtosV1.Blogs
{
    public class BlogTagDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public static BlogTagDto FromBlogTag(BlogTag tag)
        {
            return new BlogTagDto()
            {
                Id = tag.Id,
                Name = tag.Name,
                Color = tag.Color
            };
        }
    }
}