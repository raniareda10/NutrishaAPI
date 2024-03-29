﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL.EntitiesV1.Meals;
using DL.Enums;
using DL.HelperInterfaces;
using DL.StorageServices;

namespace DL.Entities
{
    public class MUser : BaseDomain, ITotal
    {
        /// <summary>
        /// User Nae
        /// </summary>

        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }

        public string Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; }

        [MinLength(3), MaxLength(300)] public string Address { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        [MinLength(3), MaxLength(300)] public string Notes { get; set; } = string.Empty;
        [MinLength(3), MaxLength(252)] public string PersonalImage { get; set; } = string.Empty;
        [MinLength(3), MaxLength(252)] public string NationalID { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = false;
        public decimal? Evaluation { get; set; } = 0;
        public int? DeviceTypeId { get; set; }
        public int? GenderId { get; set; }
        public MGender Gender { get; set; }
        public int? JourneyPlanId { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int? VerfiyCode { get; set; } = 0;
        public int? OfferId { get; set; }
        public DateTime? OfferTime { get; set; }
        public decimal? Weight { get; set; } = 0;
        public decimal? Height { get; set; } = 0;
        public decimal? BMI { get; set; } = 0;
        public decimal? Age { get; set; } = 0;
        public bool IsDataComplete { get; set; } = false;
        public bool IsAccountVerified { get; set; } = false;
        public bool IsAdmin { get; set; }
        public RegistrationType RegistrationType { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Dictionary<string, int> Totals { get; set; }

        public DateTime? SubscriptionDate { get; set; }
        public string SubscriptionType { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsManuallySubscribed { get; set; }

        public double TotalAmountPaid { get; set; }

        public ICollection<MealPlanEntity> Plans { get; set; }
        public List<UploadResult> Files { get; set; }

        // Subscribed Info
        public bool IsMealPlanPreferencesDataCompleted { get; set; }
        public ActivityLevel ActivityLevel { get; set; }
        public string NumberOfMealsPerDay { get; set; }
        public EatReasonFeel EatReason { get; set; }
        public float TargetWeight { get; set; }
        public string MedicineNames { get; set; }
        public bool IsRegularMeasurer { get; set; }
        public bool HasBaby { get; set; }

        public bool HasNewMessage { get; set; }
        public string LastMessage { get; set; }
        public bool IsBanned { get; set; }
    }
}