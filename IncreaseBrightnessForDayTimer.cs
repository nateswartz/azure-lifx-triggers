using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class IncreaseBrightnessForDayTimer
    {
        [FunctionName("IncreaseBrightnessForDayTimer")]
        public async static Task Run([TimerTrigger("0 0 12 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var settings = new LightSettings
            {
                Brightness = 0.2,
            };
            await LightHelper.UpdateLightAsync(settings, "Failed to increase brightness", log);
        }
    }
}
