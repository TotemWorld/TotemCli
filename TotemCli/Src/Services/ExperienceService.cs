﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            using MemoryStream memoryStream = new();
            memoryStream.Write(experience.Banner, 0, experience.Banner.Length);
            var fileContent = new StreamContent(memoryStream);
                var json = JsonConvert.SerializeObject(experience);
            using MultipartFormDataContent formData = new()
            {         
                { fileContent, "Banner" },
                { new StringContent(json), "Experience" },
            };

            using HttpClient httpClient = new();
            var response = await httpClient.PostAsync($"{_configuration["API_URL"]}/totem/create-experience", formData);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error creating the experience : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }

    }
}
