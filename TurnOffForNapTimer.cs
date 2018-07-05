using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class TurnOffForNapTimer
    {
        [FunctionName("TurnOffForNapTimer")]
        public static void Run([TimerTrigger("0 0 15 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            var settings = new LightSettings
            {
                Power = "off"
            };
            LightHelper.UpdateLight(settings, "Failed to turn off light", log);
        }
    }
}
