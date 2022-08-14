namespace DL.DtosV1.MealPlans
{
    public class AddExtraBitesDto
    {
        public long DayId { get; set; }
        public long? MealId { get; set; }
        public string MealName { get; set; }
    }
    
}