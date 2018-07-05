using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class WakeUpTimer
    {
        [FunctionName("WakeUpTimer")]
        public static void Run([TimerTrigger("0 0 10 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            var settings = new LightSettings
            {
                Brightness = 0.02,
                Power = "on"
            };
            LightHelper.UpdateLight(settings, "Failed to turn on light", log);
        }
    }
}
