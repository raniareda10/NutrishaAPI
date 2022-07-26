using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.EntitiesV1.Meals;

namespace DL.DtosV1.Meals
{
    public class MealListModelDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public string CookingTime { get; set; }
        public string PreparingTime { get; set; }
    }
}