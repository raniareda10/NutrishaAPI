using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DL.Enums;
using Microsoft.AspNetCore.Http;
namespace DL.DTOs.UserDTOs
{
    public class AllUserDTO
    {

        public int Id  { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        //Uniqe Properties

        public string Mobile { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;

        public int VerfiyCode { get; set; } = 0;
        [MinLength(3), MaxLength(300)]
        public string Address { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        [MinLength(3), MaxLength(300)]
        public string Notes { get; set; } = string.Empty;

        [MinLength(3), MaxLength(252)]
        public string PersonalImage { get; set; } = string.Empty;
        [MinLength(3), MaxLength(252)]

        public string NationalID { get; set; } = string.Empty;
 
        public bool IsAvailable { get; set; } = false;
        public int? DeviceTypeId { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        public int? GenderId { get; set; }
        public int? JourneyPlanId { get; set; }
        public string Language { get; set; } = string.Empty;
        public decimal? Weight { get; set; } = 0;
        public decimal? Height { get; set; } = 0;
        public decimal? BMI { get; set; } = 0;
        public decimal? Age { get; set; } = 0;
        public bool IsDataComplete { get; set; } = false;
        public bool IsAccountVerified { get; set; } = false;
        //  public int? OfferId { get; set; }
        // public DateTime? OfferTime { get; set; }
        public RegistrationType RegistrationType { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
