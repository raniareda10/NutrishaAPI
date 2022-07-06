using System.Collections.Generic;
using DL.Entities;
using DL.Enums;
using DL.HelperInterfaces;

namespace DL.EntitiesV1.Comments
{
    public class Comment : BaseEntityV1, ITotal, IContent
    {
        public int UserId { get; set; }
        public MUser User { get; set; }

        public long EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public Dictionary<string, int> Totals { get; set; }
        public string Content { get; set; }
    }
}