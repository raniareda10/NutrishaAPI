using DL.MailModels;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailReader
{
    public interface IMailRepository
    {
        IEnumerable<MimeMessage> GetUnreadMails();
        public IEnumerable<MimeMessage> GetAllMails();

    }
    public class MailRepository :IMailRepository
    {
        
        private readonly MailSettings _mailSettings;
      
        public MailRepository(IOptions<MailSettings> mailSettings )
        {
            _mailSettings = mailSettings.Value;

         
      
        }

        public IEnumerable<MimeMessage> GetUnreadMails()
        {
            var messages = new List<MimeMessage>();

            using (var client = new ImapClient())
            {
                client.Connect(_mailSettings.HostRead, _mailSettings.PortRead, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
                foreach (var uniqueId in results.UniqueIds)
                {
                    var message = inbox.GetMessage(uniqueId);

                    messages.Add(message);

                    //Mark message as read
                    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }

            return messages;
        }

        public IEnumerable<MimeMessage> GetAllMails()
        {
            var messages = new List<MimeMessage>();

            using (var client = new ImapClient())
            {
                client.Connect(_mailSettings.HostRead, _mailSettings.PortRead, true);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
               var AllMessages= inbox.ToHashSet();
                foreach (var item in AllMessages)
                {
                    messages.Add(item);
                }
                //var results = inbox.Search(SearchOptions.All, SearchQuery.NotSeen);

                //foreach (var uniqueId in results.UniqueIds)
                //{
                //    var message = inbox.GetMessage(uniqueId);

                //    messages.Add(message);

                //    //Mark message as read
                //    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                //}

                client.Disconnect(true);
            }

            return messages;
        }
    }
}
