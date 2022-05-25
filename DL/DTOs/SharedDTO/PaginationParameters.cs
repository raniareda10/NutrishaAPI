using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs.SharedDTO
{
    public class PaginationParameters
    {
        const int maxPageSize = 5000;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 1000;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

    }
}
