using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Dairies
{
    public class GetDairiesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public MealType Type { get; set; }
    }
}