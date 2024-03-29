﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
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

        public async Task SendEmailAsync(MailRequest request)
        {
            try
            {
                var objMessage = new MailMessage()
                {
                    From = new MailAddress(_mailSettings.Mail),
                    To = { request.ToEmail },
                    Subject = request.Subject,
                    IsBodyHtml = true,
                    Body = request.Body
                };

                var smtp = new System.Net.Mail.SmtpClient(_mailSettings.Host, _mailSettings.Port); // OR 25
                // smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential(
                    _mailSettings.Mail, _mailSettings.Password);
                smtp.Send(objMessage);
                _logger.LogInformation($"mail to {request.ToEmail} sent: {JsonConvert.SerializeObject(objMessage)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error While Sending Email To {request.ToEmail}, ex: {JsonConvert.SerializeObject(ex)}");
            }
        }

        public async Task<string> SendWelcomeEmailAsync(WelcomeRequest request)
        {
            try
            {
                var objMessage = new MailMessage()
                {
                    From = new MailAddress(_mailSettings.Mail),
                    To = { request.ToEmail },
                    Subject = $"Welcome {request.UserName}",
                    IsBodyHtml = true,
                    Body =
                        "<div style=\"display: flex; flex-direction: column; justify-content: center; align-items: center\">" +
                        $"<h1>Welcome to nutrisha</h1><p>Your OTP: {request.VerifyCode}</p></div>"
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
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error While Sending Email To {request.ToEmail}, ex: {JsonConvert.SerializeObject(ex)}");
                return ex.Message;
            }
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
                _logger.LogError(
                    $"Error While Sending Email To {request.ToEmail}, ex: {JsonConvert.SerializeObject(ex)}");
                return ex.ToString();
            }
        }
    }
}