using DL.Entities;
using DL.EntitiesV1.Meals;

namespace DL.EntitiesV1.ShoppingCartEntity
{
    public class ShoppingCartItemEntity : BaseEntityV1
    {
        public float Quantity { get; set; }
        public string ItemName { get; set; }
        public IngredientUnitType UnitType { get; set; }
        public bool IsBought { get; set; }
        
        public long ShoppingCartId { get; set; }
        public ShoppingCartEntity ShoppingCart { get; set; }
    }
}