using System;

namespace DL.EntitiesV1
{
    public class BaseEntityV1
    {
        public long Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}