using System.Text.RegularExpressions;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using TotemCli.Models;
using System.Security.Cryptography;
using System.Net.Mime;

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
        public async Task SendHttpPostRegisterAsset(string path, string name, string? experienceId, 
            string longitude, string latitude, string pathPicture)
        {
            double longitudeDouble = double.Parse(longitude);
            double latitudeDouble = double.Parse(latitude);

            string realPathAsset;
            string realPathPicture;

            realPathAsset = GetRealPath(path);
            realPathPicture = GetRealPath(pathPicture);

            byte[] contentAsset = File.ReadAllBytes(realPathAsset);
            var assetBytesContent = new ByteArrayContent(contentAsset);
            var assetFileName = Path.GetFileName(realPathAsset);

            byte[] contentPicture = File.ReadAllBytes(realPathPicture);
            var pictureBytesContent = new ByteArrayContent(contentPicture);
            var pictureFileName = Path.GetFileName(realPathPicture);

            Asset Asset = new()
            {
                Name = name,
                ExperienceId = experienceId,
                Point = new Point { Longitude = longitudeDouble, Latitude = latitudeDouble },
            };

            var json = JsonConvert.SerializeObject(Asset);
            Uri uri = new($"{_configuration["API_URL"]}/totem/register-asset");

            using MultipartFormDataContent formData = new()
            {
                {assetBytesContent, "ByteContent", assetFileName },
                {pictureBytesContent, "PictureByteContent", pictureFileName },
                {new StringContent(json.ToString()), "AssetData"}
            };

            using HttpClient httpClient = new();
            var response = await httpClient.PostAsync(uri, formData);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error uploading the asset : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }

        }

        private static string GetRealPath(string path)
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

            return pathFile;

        }
    }
}

