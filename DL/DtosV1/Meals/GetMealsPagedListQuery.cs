using DL.CommonModels;
using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Meals
{
    public class GetMealsPagedListQuery : GetPagedListQueryModel
    {
        public MealType? MealType { get; set; }
    }
}