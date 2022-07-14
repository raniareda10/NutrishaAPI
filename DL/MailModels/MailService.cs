using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace DL.MailModels
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
        {
            _logger = logger;
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task<string> SendWelcomeEmailAsync(WelcomeRequest request)
        {
            var objMessage = new MailMessage()
            {
                From = new MailAddress(_mailSettings.Mail),
                To = { request.ToEmail },
                Subject = $"Welcome {request.UserName}",
                IsBodyHtml = true,
                Body = "Otp:" + request.VerifyCode
            };

            var serializedMailMessage = JsonConvert.SerializeObject(objMessage);
            _logger.LogInformation($"Sending  mail to {request.ToEmail}: {serializedMailMessage}");

            var smtp = new System.Net.Mail.SmtpClient(_mailSettings.Host, _mailSettings.Port); // OR 25
            // smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential(
                _mailSettings.Mail, _mailSettings.Password);
            smtp.Send(objMessage);
            _logger.LogInformation($"mail to {request.ToEmail} sent: {JsonConvert.SerializeObject(objMessage)}");
            return "OK";
        }

        // public async Task<string> SendWelcomeEmailAsync(WelcomeRequest request)
        // {
        //     // var filePath = Directory.GetCurrentDirectory() + @"\wwwroot\EmailTemps\ActivateTemp.html";
        //     // StreamReader str = new StreamReader(filePath);
        //     // str.Close();
        //     var mailText = "Otp:" + request.VerifyCode;
        //     var email = new MimeMessage
        //     {
        //         Sender = MailboxAddress.Parse(_mailSettings.Mail),
        //         To = { MailboxAddress.Parse(request.ToEmail) },
        //         Subject = $"Welcome {request.UserName}"
        //     };
        //     var builder = new BodyBuilder
        //     {
        //         HtmlBody = mailText
        //     };
        //
        //     email.Body = builder.ToMessageBody();
        //
        //     using var smtp = new SmtpClient();
        //     await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port);
        //     await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
        //     await smtp.SendAsync(email);
        //     await smtp.DisconnectAsync(true);
        //     return "OK";
        // }


        public async Task<string> SendActivateEmailAsync(WelcomeRequest request)
        {
            try
            {
                string FilePath = Directory.GetCurrentDirectory() + @"\wwwroot\EmailTemps\ActivateTempEmail.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("{UserName}", request.UserName).Replace("{Link}", request.Link)
                    .Replace("[ID]", request.Id.ToString());
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = $"Welcome {request.UserName}";
                var builder = new BodyBuilder();
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.Auto);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}