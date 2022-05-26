using DL.Entities;
using DL.Enums;

namespace DL.EntitiesV1.Reactions
{
    public class Reaction : BaseEntityV1
    {
        public int UserId { get; set; }
        public MUser User { get; set; }

        public long EntityId { get; set; }
        public EntityType EntityType { get; set; }

        public ReactionType ReactionType { get; set; }
    }
}