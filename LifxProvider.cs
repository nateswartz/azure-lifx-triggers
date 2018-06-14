using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace lifxtriggers
{
    public class LifxProvider
    {
        HttpClient _client;

        public LifxProvider()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                Environment.GetEnvironmentVariable("LifxApiToken", EnvironmentVariableTarget.Process));
            _client.BaseAddress = new Uri("https://api.lifx.com/v1/");
        }

        public void UpdateLight(string lightID, LightSettings settings, TraceWriter log)
        {
            var camelCaseFormatter = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(settings, camelCaseFormatter);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var putResponse = _client.PutAsync($"lights/id:{lightID}/state", stringContent).Result;

            if (putResponse.IsSuccessStatusCode)
            {
                log.Info("Successfully updated light.");
            }
            else
            {
                log.Info("Failed to update light.");
            }
        }
    }
}
