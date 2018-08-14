using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class CheckStatus
    {
        [FunctionName("CheckStatus")]
        public async static Task Run([TimerTrigger("0 0 */2 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var lightID = Environment.GetEnvironmentVariable("LightId", EnvironmentVariableTarget.Process);
            var provider = new LifxProvider();

            var connected = await provider.IsLightOnlineAsync(lightID);

            if (connected)
            {
                log.Info("Light still online");
            }
            else
            {
                log.Info("Light went offline!");
                var emailProvider = new EmailProvider();
                var emailSent = await emailProvider.SendEmailAsync("Lifx connection is down", "For more details check the Azure function portal.");

                var logMsg = emailSent ? "Message Sent" : "Failed to send message";
                log.Info(logMsg);
            }
        }
    }
}
