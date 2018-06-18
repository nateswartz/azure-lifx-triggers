using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace lifxtriggers
{
    public class LifxProvider
    {
        private HttpClient _client;
        private JsonSerializerSettings _settings;

        public LifxProvider()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                Environment.GetEnvironmentVariable("LifxApiToken", EnvironmentVariableTarget.Process));
            _client.BaseAddress = new Uri("https://api.lifx.com/v1/");
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public void UpdateLight(string lightID, LightSettings settings, TraceWriter log)
        {
            var json = JsonConvert.SerializeObject(settings, _settings);
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

        public bool IsLightOnline(string lightID)
        {
            var getResponse = _client.GetAsync($"lights/id:{lightID}").Result;
            var content = getResponse.Content.ReadAsStringAsync().Result;
            var results = JsonConvert.DeserializeObject<List<LightStatus>>(content, _settings);
            return results.First().Connected;
        }
    }
}
