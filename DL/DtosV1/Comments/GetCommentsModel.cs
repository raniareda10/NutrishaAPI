using DL.CommonModels.Paging;
using DL.HelperInterfaces;

namespace DL.DtosV1.Comments
{
    public class GetCommentsModel : PagedModel, IEntityId
    {
        public long EntityId { get; set; }
    }
}