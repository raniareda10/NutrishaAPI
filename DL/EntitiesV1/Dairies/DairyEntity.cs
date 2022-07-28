using DL.EntitiesV1.Meals;

namespace DL.EntitiesV1.Dairies
{
    public class DairyEntity : BaseEntityV1
    {
        public MealType Type { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int UserId { get; set; }
    }
}