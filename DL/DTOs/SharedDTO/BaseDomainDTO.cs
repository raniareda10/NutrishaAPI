using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.SharedDTO
{
    public class BaseDomainDTO
    {
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Is That Entity Active Variable
        /// </summary>
        public bool? IsActive { get; set; } = true;
        /// <summary>
        /// UpdatedOn Date
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// CreatedBy User
        /// </summary>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// UpdatedBy User
        /// </summary>
        public int? UpdatedBy { get; set; }

        public bool? IsDeleted { get; set; } = false;

    }
}
