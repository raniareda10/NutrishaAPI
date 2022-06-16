using System.Collections.Generic;

namespace DL.DtosV1.DisLikes
{
    public class PutDisLikesDto
    {
        public HashSet<long> DislikedMealIds { get; set; }
    }
}