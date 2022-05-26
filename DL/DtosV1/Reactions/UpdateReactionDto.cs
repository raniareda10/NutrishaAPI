using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.HelperInterfaces;

namespace DL.DtosV1.Reactions
{
    public class UpdateReactionDto: IEntity
    {
        public long EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}