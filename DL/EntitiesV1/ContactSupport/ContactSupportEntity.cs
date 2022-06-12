using System;
using DL.Entities;

namespace DL.EntitiesV1.ContactSupport
{
    public class ContactSupportEntity : BaseEntityV1
    {
        public string Message { get; set; }
        public long TypeId { get; set; }
        public ContactSupportType Type { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
    }
    
}