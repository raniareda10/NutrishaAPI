namespace DL.DtosV1.Users.Admins
{
    public class CreateAdminDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}