using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
  public  class MNotification : BaseDomain
    {
      
        /// <summary>
        /// User Nae
        /// </summary>
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Message { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsSeen { get; set; }
        public int? SourceId { get; set; }
        public int? NotificationTypeId { get; set; }
        public int? AdminId { get; set; }
     

    }
}
