using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace lifxtriggers
{
    public static class TurnOffForNapTimer
    {
        [FunctionName("TurnOffForNapTimer")]
        public async static Task Run([TimerTrigger("0 0 15 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            var settings = new LightSettings
            {
                Power = "off"
            };
            await LightHelper.UpdateLightAsync(settings, "Failed to turn off light", log);
        }
    }
}
