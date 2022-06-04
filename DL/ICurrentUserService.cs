namespace DL
{
    public interface ICurrentUserService
    {
        public int UserId { get; set; }
        public string Locale { get; set; }
    }
}