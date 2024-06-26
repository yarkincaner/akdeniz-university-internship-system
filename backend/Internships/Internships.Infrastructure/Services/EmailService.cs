using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Communication;
using Azure.Communication.Email;
using Internships.Core.Interfaces;
using Internships.Core.Settings;
using Microsoft.Extensions.Options;


namespace Internships.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public EmailClient _emailClient {  get; set; }
        
        public MailSettings _mailSettings { get; set; }

        public EmailService(IOptions<MailSettings> mailSettings) {
            _mailSettings = mailSettings.Value;
            _emailClient = new EmailClient(_mailSettings.connectionStrings);
        }


        public async Task SendEmail(string email, string url) {
            try
            {
                EmailSendOperation emailSendOperation =await _emailClient.SendAsync(
                Azure.WaitUntil.Completed,
                senderAddress: _mailSettings.senderAddress,
                recipientAddress: email,
                subject: "Akdeniz University Internship Approve By Company ",
                htmlContent: $"<html>" +
                $"<body><center>" +
                $"<h1>Internship Approve</h1><br>" +
                $"<p>For See to Detail of Intership. Please follow this link:{url}</p>" +
                $"</center></body>" +
                $"</html>");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error when sending email to employee {ex.Message}");
            }
        }
    
    

}
}
