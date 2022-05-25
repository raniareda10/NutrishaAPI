using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DL.DTOs.PollAnswerDTO
{
   public class IncludePollAnswerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SelectionRate { get; set; }
        public string Color { get; set; }
    }
}
