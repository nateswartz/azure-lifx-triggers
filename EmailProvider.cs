using SendGrid;
using SendGrid.Helpers.Mail;
using System;

namespace lifxtriggers
{
    public class EmailProvider
    {
        private SendGridClient _client;
        private readonly string _defaultFromAddress = "donotreply@lifxstatuscheck.com";
        private readonly string _defaultFromName = "Lifx Status Check";
        private readonly EmailAddress _defaultToAddress;

        public EmailProvider()
        {
            var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
            _client = new SendGridClient(apiKey);
            _defaultToAddress = new EmailAddress(Environment.GetEnvironmentVariable("EmailForNotification"));
        }

        public bool SendEmail(string subject, string content)
        {
            var from = new EmailAddress(_defaultFromAddress, _defaultFromName);
            var msg = MailHelper.CreateSingleEmail(from, _defaultToAddress, subject, content, content);
            var response = _client.SendEmailAsync(msg).Result;

            return response.StatusCode == System.Net.HttpStatusCode.Accepted ? true : false;
        }
    }
}
