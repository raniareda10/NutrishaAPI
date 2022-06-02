namespace DL.Services.Users.Models
{
    public class AdminUserModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public string PersonalImage { get; set; }
    }
}