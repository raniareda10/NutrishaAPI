using System;
using DL.Entities;

namespace DL.EntitiesV1.AdminUser
{
    public class ResetUserPasswordEntity : BaseEntityV1
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
    }
}