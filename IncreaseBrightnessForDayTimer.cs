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
            var settings = new LightSettings
            {
                Brightness = 0.3,
            };
            LightHelper.UpdateLight(settings, "Failed to increase brightness", log);
        }
    }
}
