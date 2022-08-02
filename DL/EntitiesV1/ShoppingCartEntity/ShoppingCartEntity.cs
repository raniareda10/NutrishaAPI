using System.Collections.Generic;
using DL.Entities;

namespace DL.EntitiesV1.ShoppingCartEntity
{
    public class ShoppingCartEntity : BaseEntityV1
    {
        public int UserId { get; set; }
        public MUser User { get; set; }
        public ICollection<ShoppingCartItemEntity> Items { get; set; }
    }
}