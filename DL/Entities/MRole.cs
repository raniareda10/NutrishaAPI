using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DL.EntitiesV1;

namespace DL.Entities
{
   public class MRole : BaseEntity
    {
        
        /// <summary>
        /// Role Name 
        /// </summary>
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        // public int CreatedByUserId { get; set; }
        // public MUser CreatedByUser { get; set; }
    }
}
