using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TotemCli.Models;

namespace TotemCli.Services
{
    public class ExperienceService
    {
        private readonly IConfiguration _configuration;

        public ExperienceService
        (
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        public async Task SendHttpPostRegisterAsset(Experience experience)
        {
            HttpClient httpClient = new();
            Uri uri = new($"{_configuration["API_URL"]}/totem/create-experience");
            var json = JsonConvert.SerializeObject(experience);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri.ToString(), content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error creating the experience : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }

    }
}
