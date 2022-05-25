using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.NotificationDTO
{
   public class AllNotificationDto
    {
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Message { get; set; }
        [Required]
        public string Date { get; set; } 
        public bool IsSeen { get; set; }
        public string Description { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
    }
}
