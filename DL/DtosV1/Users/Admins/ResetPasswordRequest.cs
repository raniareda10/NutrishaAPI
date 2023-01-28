namespace DL.DtosV1.Users.Admins
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}