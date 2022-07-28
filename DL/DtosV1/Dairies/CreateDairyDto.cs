using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Dairies
{
    public class CreateDairyDto
    {
        public MealType Type { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    }
}