using System.Collections.Generic;

namespace DL.CommonModels.Paging
{
    public class PagedResult<T>
    {
        public IList<T> Data { get; set; }
        public int TotalRows { get; set; }
    }
}