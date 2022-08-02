namespace DL.EntitiesV1.Meals
{
    public class MealIngredientEntity : BaseEntityV1
    {
        public float Quantity { get; set; }
        public IngredientUnitType UnitType { get; set; }
        public string IngredientName { get; set; }
        public long MealId { get; set; }
        public MealEntity Meal { get; set; }
    }

    public enum IngredientUnitType
    {
        Liter,
        Cup,
        Tbs,
        Tsp,
        Kg,
        Gram,
        Slice,
        Piece,
    }
}