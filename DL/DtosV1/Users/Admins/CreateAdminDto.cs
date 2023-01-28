namespace DL.DtosV1.Users.Admins
{
    public class CreateAdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
    }

    public class UpdateAdminDto
    {
        public int UserId { get; set; }
        public int? RoleId { get; set; }
    }
}