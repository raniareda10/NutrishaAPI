﻿using DL.DTOs.SharedDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.VideoDTO
{
   public class VideoQueryPramter : PaginationParameters
    {

        public int BlogTypeId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string Title { get; set; }
    }
}
