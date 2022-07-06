using System.Collections.Generic;

namespace DL.HelperInterfaces
{
    public interface ITotal
    {
        public Dictionary<string, int> Totals { get; set; }
    }
}