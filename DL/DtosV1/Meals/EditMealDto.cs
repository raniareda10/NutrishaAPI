using System.Linq;

namespace DL.DtosV1.Meals
{
    public class EditMealDto : PostMealDto
    {
        public long Id { get; set; }

        public override bool IsValid()
        {
            return
                Id > 0 &&
                !string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(CockingTime) &&
                !string.IsNullOrWhiteSpace(PreparingTime) &&
                !string.IsNullOrWhiteSpace(Allergies) &&
                !string.IsNullOrWhiteSpace(MealSteps) &&
                Ingredients is { Count: > 0 } &&
                Ingredients.All(ingre => ingre.Quantity > 0 &&
                                         !string.IsNullOrWhiteSpace(ingre.IngredientName));
        }
    }
}