using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GreenLeaves.Core.SendGrid {
    public class SendGridMail : ISendGrid {
        #region Props
        private ILogger<SendGridMail> _logger { get; }
        #endregion

        #region Constructor and dependency Injection
        public SendGridMail( ILogger<SendGridMail> logger ) {
            _logger = logger;
        }
        #endregion
        public async Task<bool> SendMailAsync( string key, string address, string message, string subject, List<string> recipients ) {
            SendGridClient client = new SendGridClient( key );
            EmailAddress from = new EmailAddress( address );

            List<EmailAddress> tos = new List<EmailAddress>();
            recipients.ForEach( x => tos.Add( new EmailAddress { Email = x } ) );

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients( from, tos, subject, null, message );
            try {
                await client.SendEmailAsync( msg );
                return true;
            } catch (Exception e) {
                _logger.LogInformation( $"address : {address}; subject : {subject}; error : {e.Message}" );
                return false;
            }
        }
    }
}
