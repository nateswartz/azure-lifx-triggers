using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace lifxtriggers
{
    public static class CheckStatus
    {
        [FunctionName("CheckStatus")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var lightID = Environment.GetEnvironmentVariable("LightId", EnvironmentVariableTarget.Process);
            var provider = new LifxProvider();

            var connected = provider.IsLightOnline(lightID);

            if (connected)
            {
                log.Info("Light still online");
            }
            else
            {
                log.Info("Light went offline!");
                var apiKey = Environment.GetEnvironmentVariable("SendGridApiKey");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("donotreply@lifxstatuscheck.com", "Lifx Status Check");
                var subject = "Lifx connection is down";
                var to = new EmailAddress(Environment.GetEnvironmentVariable("EmailForNotification"));
                var plainTextContent = "For more details check the Azure function portal.";
                var htmlContent = "For more details check the Azure function portal";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    log.Info("Message Sent");
                }
                else
                {
                    log.Info("Failed to send message");
                }
            }
        }
    }
}
