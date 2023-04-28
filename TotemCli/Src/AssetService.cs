using System.Text.RegularExpressions;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using TotemCli.Src;

namespace TotemCli.Service
{
    internal class AssetService
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
        public async Task SendHttpPostRegisterAsset(string path, string name)
        {
            string absoluteRegexPattern = @"^([a-zA-Z]:)?[\\/].*";

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

            object contentAsset = File.ReadAllBytes(pathFile); 

            HttpClient httpClient = new HttpClient();
            Uri uri = new Uri($"{_configuration["API_URL"]}/totem/register-asset");
            AssetModel assetModel = new AssetModel()
            {
                Name = name,
                ByteContent = (byte[]) contentAsset
            };

            var json = JsonConvert.SerializeObject(assetModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri.ToString(), content);

            if (response.IsSuccessStatusCode) 
            {
                Console.WriteLine("Asset request handle succesfully by the api!");
            }
            else 
            {
                Console.WriteLine($"Error upoading the asset: {response.StatusCode}");
            }

        }
    }
}

