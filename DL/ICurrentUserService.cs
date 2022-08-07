namespace DL
{
    public interface ICurrentUserService
    {
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public string Locale { get; set; }
        public float UserTimeZoneDifference { get; set; }
    }
}