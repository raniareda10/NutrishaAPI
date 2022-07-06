using DL.CommonModels.Paging;

namespace DL.CommonModels
{
    public class GetPagedListQueryModel : PagedModel
    {
        public string SearchWord { get; set; }
    }
}