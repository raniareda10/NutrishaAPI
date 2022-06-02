namespace DL.HelperInterfaces
{
    public interface ILoginIn : IEmail, IPassword
    {
    }

    public interface IEmail
    {
        public string Email { get; set; }
    }

    public interface IPassword
    {
        public string Password { get; set; }
    }
}