using System;
using System.Collections.Generic;
using System.Linq;
using DL.EntitiesV1;

namespace DL.Repositories.Dislikes.Constants
{
    public class DislikeMealConstants
    {
        public static readonly IEnumerable<DislikeMealType> DislikedMealsType = 
            Enum.GetValues<DislikeMealType>().Where(type => type != DislikeMealType.Other);
    }
}