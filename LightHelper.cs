using Microsoft.Azure.WebJobs.Host;
using System;

namespace lifxtriggers
{
    public static class LightHelper
    {
        public static void UpdateLight(LightSettings settings, string failureMessage, TraceWriter log)
        {
            var lightID = Environment.GetEnvironmentVariable("LightId", EnvironmentVariableTarget.Process);

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
                var emailSent = emailProvider.SendEmail(failureMessage, "For more details check the Azure function portal.");

                var logMsg = emailSent ? "Message Sent" : "Failed to send message";
                log.Info(logMsg);
            }
        }
    }
}
