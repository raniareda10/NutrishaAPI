using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Entities
{
    public partial class MVerfiyCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VirfeyCode { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsOtpVerfiy { get; set; } = true;
        
    }
}
