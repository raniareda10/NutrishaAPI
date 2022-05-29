using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.NotificationDTO
{
   public class NotificationCreatDto
    {
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Message { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsSeen { get; set; }

        public int? CustomerId { get; set; }
        public int? AdminId { get; set; }
        public int? UserId { get; set; }
    }
}
