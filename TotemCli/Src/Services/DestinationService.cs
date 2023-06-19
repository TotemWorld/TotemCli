using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TotemCli.Models;

namespace TotemCli.Src.Services
{
    public class DestinationService
    {
        private readonly IConfiguration _configuration;

        public DestinationService
        (
            IConfiguration configuration
        ) 
        { 
            _configuration = configuration; 
        }

        public async Task SendHttpPostCreateDestination(string name, string polygonPoints)
        {
            List<Point> polygonPointsParsed = null!;
            try
            {
                polygonPointsParsed = JsonConvert.DeserializeObject<List<Point>>(polygonPoints)!;

            }
            catch
            {
                Console.WriteLine("polygonPoints string can't be parsed into Line<Point>. The format is the following: [{\"Longitude\":1.0,\"Latitude\":2.0},{\"Longitude\":2.0,\"Latitude\":3.0}]");
            }

            HttpClient httpClient = new HttpClient();
            Uri uri = new($"{_configuration["API_URL"]}/totem/create-destination");
            Destination destination = new()
            {
                Name = name,
                PolygonPoints = polygonPointsParsed
            };

            var json = JsonConvert.SerializeObject(destination);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request handle succesfully by the api!");
            }
            else
            {
                Console.WriteLine($"Error upoading the asset: {response.StatusCode}");
            }
        }
    }
}