using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

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
                var emailProvider = new EmailProvider();
                var emailSent = emailProvider.SendEmail("Lifx connection is down", "For more details check the Azure function portal.");

                var logMsg = emailSent ? "Message Sent" : "Failed to send message";
                log.Info(logMsg);
            }
        }
    }
}
