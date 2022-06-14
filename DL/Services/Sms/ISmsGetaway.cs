using System.Collections.Generic;
using System.Threading.Tasks;

namespace DL.Services.Sms
{
    public interface ISmsGetaway
    {
        Task SendMessageAsync(string message, string to);
    }
}