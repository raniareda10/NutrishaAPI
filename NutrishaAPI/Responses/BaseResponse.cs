namespace NutrishaAPI.Responses
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }
        public bool Done { get; set; } = true;
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}