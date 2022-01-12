using System;
using System.IO;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace pluralsightfuncs
{
    public static class EmailLicenseFile
    {
        [FunctionName("EmailLicenseFile")]
        public static void Run([BlobTrigger("licenses/{orderId}.lic", 
        Connection = "AzureWebJobsStorage")]string LicenseFileContents,
        [SendGrid(ApiKey = "SendGridApiKey")] ICollector<SendGridMessage> sender,
        [Table("orders", "orders", "{orderId}")] Order order,
        string orderId, 
        ILogger log)
        {
            //var email = Regex.Match(input:LicenseFileContents, pattern:@"^Email\:\ (.+)$", RegexOptions.Multiline).Groups[1].Value;
            var email = order.Email;
            log.LogInformation(message:$"Got order from {email}\n Order Id:{orderId}");
            var message = new SendGridMessage();
            message.From = new EmailAddress(Environment.GetEnvironmentVariable("EmailSender"));
            message.AddTo(email);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(LicenseFileContents);
            var base64 = Convert.ToBase64String(plainTextBytes);
            message.AddAttachment(filename:$"{orderId}.lic", base64, type:"text/plain");
            message.Subject = "Your license file";
            message.HtmlContent = "Thank you for your order";
            if (!email.EndsWith(value:"@test.com"))
                sender.Add(message);
        }
    }
}
