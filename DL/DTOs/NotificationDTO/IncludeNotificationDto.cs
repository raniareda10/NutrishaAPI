using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.NotificationDTO
{
   public class IncludeNotificationDto
    {
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public bool IsSeen { get; set; }

    }
}
