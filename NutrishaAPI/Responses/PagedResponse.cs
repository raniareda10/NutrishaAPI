namespace NutrishaAPI.Responses
{
    public class PagedResponse<T> : BaseResponse<T>
    {
        public int TotalRows { get; set; }
    }
}