using DL.DTOs.ArticleCommentDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.PollDTO
{
   public class AllPollDto
    {
        public int Id { get; set; }
        [Required]
        public int BlogTypeId { get; set; }
        [Required]
        public string Question { get; set; }
 
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string Color { get; set; }
        public string Notes { get; set; }
       // public int SecUserId { get; set; }
        public string AdminName { get; set; }
        public int YesPercentage { get; set; }
        public int NoPercentage { get; set; }
      //  public List<MUserPollAnswer> LstUserPollAnswer { get; set; }
    }
}
