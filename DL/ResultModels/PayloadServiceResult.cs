namespace DL.ResultModels
{
    public class PayloadServiceResult<T> : BaseServiceResult
    {
        public PayloadServiceResult()
        {
            
        }

        public PayloadServiceResult(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
    }
}