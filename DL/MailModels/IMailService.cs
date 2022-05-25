using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.MailModels
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest); 
          Task<string> SendWelcomeEmailAsync(WelcomeRequest request);
          Task<string> SendActivateEmailAsync(WelcomeRequest request);
    }
}
