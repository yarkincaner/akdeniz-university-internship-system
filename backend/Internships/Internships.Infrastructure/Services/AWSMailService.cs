using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Internships.Core.Interfaces;
using Internships.Core.Settings;
using Microsoft.Extensions.Options;

namespace Internships.Infrastructure.Services;

public class AWSMailService : IEmailService
{
    private readonly string accessKeyId;
    private readonly string secretAccessKey;
    private readonly string region;
    private readonly string senderAddress;
    
    public AWSMailService(IOptions<AWSSimpleEmailServiceSettings> options)
    {
        accessKeyId = options.Value.AccessKeyId;
        secretAccessKey = options.Value.SecretAccessKey;
        region = options.Value.Region;
        senderAddress = options.Value.SenderAddress;
    }
    public async Task SendEmail(string recipient, string bodyHtml)
    {
        string subject = "Staj Onayı - Akdeniz Universitesi Bilgisayar Mühendisliği Bölümü";
     
        try
        {
            using (var client = new AmazonSimpleEmailServiceClient(accessKeyId, secretAccessKey,
                       RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { recipient }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = bodyHtml
                            }
                        }
                    }
                };

                var response = client.SendEmailAsync(sendRequest).Result;
                Console.WriteLine("Email sent successfully. Message ID: " + response.MessageId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email. Error: " + ex.Message);
        }
    }
}