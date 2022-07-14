using System;
using DL.EntitiesV1.Meals;
using Microsoft.AspNetCore.Http;

namespace DL.DtosV1.Meals
{
    public class PostMealDto
    {
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public TimeSpan CockingTime { get; set; }
        public TimeSpan PreparingTime { get; set; }
        public IFormFile CoverImage { get; set; }
        public string MealSteps { get; set; }
        public string Allergies { get; set; }
        public string Ingredients { get; set; }
    }
    
    
   
   
   
   
   
   
   
   
}