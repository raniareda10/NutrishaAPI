using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
  public  class MNotificationUser : EmptyBaseDomain
    {
      
        /// <summary>
        /// User Nae
        /// </summary>
        [Required]
        public int NotificationId { get; set; }
        [Required]
        public int UserId { get; set; }
        public MNotification Notification { get; set; }
        public MUser User { get; set; }

    }
}
