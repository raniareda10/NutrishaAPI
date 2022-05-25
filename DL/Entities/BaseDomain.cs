using System;
using System.Collections.Generic;
using System.Text;

namespace DL.Entities
{
   public class BaseDomain
    {
        public int Id { get; set; }
        /// <summary>
        /// The Date Created The Ops On
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// 
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        /// <summary>
        /// Is That Entity Active Variable
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// UpdatedOn Date
        /// </summary>
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        /// <summary>
        /// CreatedBy User
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// UpdatedBy User
        /// </summary>
        public int UpdatedBy { get; set; }
    }
}
