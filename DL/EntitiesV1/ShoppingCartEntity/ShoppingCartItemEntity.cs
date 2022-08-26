using System.Collections.ObjectModel;
using DL.Entities;
using DL.EntitiesV1.Meals;
using Microsoft.VisualBasic;

namespace DL.EntitiesV1.ShoppingCartEntity
{
    public class ShoppingCartItemEntity : BaseEntityV1
    {
        // public float Quantity { get; set; }
        // public IngredientUnitType UnitType { get; set; }
        public string ItemName { get; set; }

        public bool IsBought { get; set; }
        public Collection<ShoppingCartItemMealEntity> Meals { get; set; }

        public long ShoppingCartId { get; set; }
        public ShoppingCartEntity ShoppingCart { get; set; }
    }

    public class ShoppingCartItemMealEntity : BaseEntityV1
    {
        public long MealId { get; set; }
        public MealEntity Meal { get; set; }
        public long ShoppingCartItemId { get; set; }
        public ShoppingCartItemEntity ShoppingCartItem { get; set; }
    }
}