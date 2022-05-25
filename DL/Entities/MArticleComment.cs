using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.Entities
{
   public class MArticleComment : BaseDomain
    {
        [Required]
        public int ArticleId { get; set; }
      //  public MUser User { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
