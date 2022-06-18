using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class CompleteUserDTO
    {
        [Required]
        public int UserId { get; set; }
        //Uniqe Properties
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        public List<int?> UserRisk { get; set; } 
        public HashSet<long> UserAllergy { get; set; }
        //[MinLength(3), MaxLength(300)]
        //public string Address { get; set; } = string.Empty;
        //public string Longitude { get; set; } = string.Empty;
        //public string Latitude { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
        public int? GenderId { get; set; }
        public int? JourneyPlanId { get; set; }
        //public int? OfferId { get; set; }
        //public DateTime? OfferTime { get; set; }

        public decimal? Weight { get; set; } = 0;
        public decimal? Height { get; set; } = 0;
        public decimal? BMI { get; set; } = 0;
        public decimal? Age { get; set; } = 0;
        public IFormFile PersonalImage { get; set; }
        public IFormFile NationalID { get; set; }
        public bool? IsAvailable { get; set; } = false;
        public bool? IsDataComplete { get; set; } = false;
        //public decimal? Evaluation { get; set; } = 0;

    }
}
