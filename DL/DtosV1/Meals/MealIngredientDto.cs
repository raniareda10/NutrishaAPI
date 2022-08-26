using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Meals
{
    public class MealIngredientDto
    {
        public float Quantity { get; set; }
        public IngredientUnitType UnitType { get; set; }
        public long MealId { get; set; }
        public string IngredientName { get; set; }
    }
}