using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.VideoDTO
{
   public class VideoCreatDto
    {
        [Required]
        public int BlogTypeId { get; set; }
        [Required]
        [MinLength(3), MaxLength(250)]
        public string Title { get; set; }
        [Required]
        public IFormFile Video { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; }
        public int SecUserId { get; set; }
    }
}
