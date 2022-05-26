namespace DL.ResultModels
{
    public class PayloadServiceResult<T> : BaseServiceResult
    {
        public T Data { get; set; }
    }
}