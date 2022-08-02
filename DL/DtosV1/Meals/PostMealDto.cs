using System;
using System.Collections.Generic;
using System.Linq;
using DL.DTOs.IngredientDTO;
using DL.EntitiesV1.Meals;
using Microsoft.AspNetCore.Http;

namespace DL.DtosV1.Meals
{
    public class PostMealDto
    {
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public string CockingTime { get; set; }
        public string PreparingTime { get; set; }
        public IFormFile CoverImage { get; set; }
        public string MealSteps { get; set; }
        public string Allergies { get; set; }
        public ICollection<MealIngredientDto> Ingredients { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(CockingTime) &&
                   !string.IsNullOrWhiteSpace(PreparingTime) &&
                   !string.IsNullOrWhiteSpace(Allergies) &&
                   !string.IsNullOrWhiteSpace(MealSteps) &&
                   CoverImage != null && Ingredients is { Count: > 0 } && 
                   Ingredients.All(ingre => ingre.Quantity > 0 &&
                       !string.IsNullOrWhiteSpace(ingre.IngredientName));
        }
    }
}