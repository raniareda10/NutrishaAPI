using DL.MailModels;
using Microsoft.Extensions.Options;
using Model.ApiModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HELPER
{
    public interface ISMS
    {
        public string SendSMS(string MobileNum, string Message);
    }
    public class SMS:ISMS
    {

        private readonly MailSettings _mailSettings;

        public SMS(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;



        }

        public  string SendSMS(string MobileNum, string Message)
        {

            string smsURL = "https://kuwait.uigtc.com/capi/sms/send_sms?api_key="+ _mailSettings .SMSAPI+ "&sender_id=1&send_type=1&sms_content="+ Message + "&numbers="+ MobileNum;

            System.Net.WebClient webClient = new System.Net.WebClient();
            // string result = webClient.DownloadString(smsURL);

            //  return result;

            return "";
        }
    }
}
