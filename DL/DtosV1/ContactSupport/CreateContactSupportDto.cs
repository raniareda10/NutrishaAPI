using DL.EntitiesV1.ContactSupport;

namespace DL.DtosV1.ContactSupport
{
    public class CreateContactSupportDto
    {
        public string Message { get; set; }
        public long TypeId { get; set; }
    }
}