using System.Collections.Generic;

namespace DL.HelperInterfaces
{
    public interface ITotal
    {
        public IDictionary<string, int> Totals { get; set; }
    }
}