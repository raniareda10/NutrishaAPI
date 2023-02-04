using System;
using DL.Entities;

namespace DL.EntitiesV1.AdminUser
{
    public class ResetUserPasswordEntity : BaseEntityV1
    {
        public string Token { get; set; }
        public int AdminUserId { get; set; }
        public AdminUserEntity AdminUser { get; set; }
    }
}