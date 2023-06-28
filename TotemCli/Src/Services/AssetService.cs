using System.Text.RegularExpressions;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using TotemCli.Models;
using System.Security.Cryptography;

namespace TotemCli.Services
{
    public class AssetService
    {
        private readonly IConfiguration _configuration;
        public AssetService
        (
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }

        enum TypeOfPath { Absolute, Relative }
        public async Task SendHttpPostRegisterAsset(string path, string name, string? experience, string longitude, string latitude)
        {
            string absoluteRegexPattern = @"^([a-zA-Z]:)?[\\/].*";

            double longitudeDouble = double.Parse(longitude);
            double latitudeDouble = double.Parse(latitude);

            TypeOfPath typeOfPath = Regex.IsMatch(path, absoluteRegexPattern) ? TypeOfPath.Absolute : TypeOfPath.Relative;

            string pathFile = "";

            switch (typeOfPath)
            {
                case TypeOfPath.Absolute:
                    pathFile = path;
                    break;
                case TypeOfPath.Relative:
                    pathFile = Path.Combine(Environment.CurrentDirectory, path);
                    break;
            }

            byte[] contentAsset = File.ReadAllBytes(pathFile);
            using HttpClient httpClient = new();
            Uri uri = new($"{_configuration["API_URL"]}/totem/register-asset");
            Asset Asset = new()
            {
                Name = name,
                ByteContent = contentAsset,
                ExperienceId = experience,
                Point = new Point { Longitude = longitudeDouble, Latitude = latitudeDouble },
            };

            var json = JsonConvert.SerializeObject(Asset);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine("1");
            var response = await httpClient.PostAsync(uri.ToString(), content);
            Console.WriteLine("2");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error uploading the asset : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}

