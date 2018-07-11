using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading.Tasks;

namespace lifxtriggers
{
    public static class LightHelper
    {
        public static async Task UpdateLightAsync(LightSettings settings, string failureMessage, TraceWriter log)
        {
            var lightID = Environment.GetEnvironmentVariable("LightId", EnvironmentVariableTarget.Process);

            var provider = new LifxProvider();
            var lightUpdated = await provider.UpdateLightAsync(lightID, settings);

            if (lightUpdated)
            {
                log.Info("Successfully updated light.");
            }
            else
            {
                log.Info("Failed to update light.");
                var emailProvider = new EmailProvider();
                var emailSent = await emailProvider.SendEmailAsync(failureMessage, "For more details check the Azure function portal.");

                var logMsg = emailSent ? "Message Sent" : "Failed to send message";
                log.Info(logMsg);
            }
        }
    }
}
