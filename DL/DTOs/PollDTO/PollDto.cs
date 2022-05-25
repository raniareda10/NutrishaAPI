using DL.DTOs.ArticleAttachmentDTO;
using DL.DTOs.ArticleCommentDTO;
using DL.DTOs.PollAnswerDTO;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DL.DTOs.PollDTO
{
   public class PollDto
    {

        public int selectedAnswerId { get; set; }
        public List<IncludePollAnswerDto> options { get; set; }
    }
}
