using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class IncreaseBrightnessForDayTimer
    {
        [FunctionName("IncreaseBrightnessForDayTimer")]
        public static void Run([TimerTrigger("0 40 10 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var lightID = Environment.GetEnvironmentVariable("LightId", EnvironmentVariableTarget.Process);
            var settings = new LightSettings
            {
                Brightness = 0.3,
            };

            var provider = new LifxProvider();
            var lightUpdated = provider.UpdateLight(lightID, settings);

            if (lightUpdated)
            {
                log.Info("Successfully updated light.");
            }
            else
            {
                log.Info("Failed to update light.");
                var emailProvider = new EmailProvider();
                var emailSent = emailProvider.SendEmail("Failed to increase brightness", "For more details check the Azure function portal.");

                var logMsg = emailSent ? "Message Sent" : "Failed to send message";
                log.Info(logMsg);
            }
        }
    }
}
