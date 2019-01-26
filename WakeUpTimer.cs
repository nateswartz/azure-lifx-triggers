using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class WakeUpTimer
    {
        [FunctionName("WakeUpTimer")]
        public async static Task Run([TimerTrigger("0 15 11 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var settings = new LightSettings
            {
                Brightness = 0.02,
                Power = "on"
            };
            await LightHelper.UpdateLightAsync(settings, "Failed to turn on light", log);
        }
    }
}
