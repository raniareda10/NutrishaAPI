
using DL.HelperInterfaces;

namespace DL.DtosV1.Users.Admins
{
    public class AdminLoginDto : ILoginIn
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}