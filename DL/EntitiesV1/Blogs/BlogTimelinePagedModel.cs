using DL.CommonModels.Paging;

namespace DL.EntitiesV1.Blogs
{
    public class BlogTimelinePagedModel : PagedModel
    {
        public long? TagId { get; set; }
    }
}