namespace DL.CommonModels.Paging
{
    public class PagedModel
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}