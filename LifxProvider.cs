﻿using Newtonsoft.Json;
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
            var lifxToken = Environment.GetEnvironmentVariable("LifxApiToken", EnvironmentVariableTarget.Process);
            var lifxEndpoint = Environment.GetEnvironmentVariable("LifxEndpoint", EnvironmentVariableTarget.Process);

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lifxToken);
            _client.BaseAddress = new Uri(lifxEndpoint);
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public bool UpdateLight(string lightID, LightSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings, _settings);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var putResponse = _client.PutAsync($"lights/id:{lightID}/state", stringContent).Result;

            return putResponse.IsSuccessStatusCode;
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
