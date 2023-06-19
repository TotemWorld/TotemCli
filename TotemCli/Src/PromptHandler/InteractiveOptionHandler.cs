using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotemCli.Enums;
using TotemCli.Services;
using TotemCli.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TotemCli.Src.Services;

namespace TotemCli.PromptHandler
{
    public class InteractiveOptionHandler
    {
        private readonly AssetService _assetService;
        private readonly DestinationService _destinationService;
        public InteractiveOptionHandler
        (
            AssetService assetService,
            DestinationService destinationService
        )
        {
            _assetService = assetService;
            _destinationService = destinationService;
        }

        public async Task ShowInterativeOptions()
        {
            var selectionPromptStyle = new Style().Foreground(Color.Green);

            var option = AnsiConsole.Prompt<string>
            (
                new SelectionPrompt<string>()
                    .Title("[gold1]Select the[bold] application[/] type:[/]")
                    .PageSize(10)
                    .HighlightStyle(selectionPromptStyle)
                    .MoreChoicesText("Move up and down to reveal more actions")
                    .AddChoices(
                        Enum.GetNames(typeof(Actions))
                    )
            );

            switch(option)
            {
                case "LoadAsset":
                    await LoadAssetOptionHandler();
                    break;
                case "CreateDestination":
                    await CreateDestinationOptionHandler();
                    break;
                    
            }
        }

        private async Task LoadAssetOptionHandler()
        {
            string name;
            string path;
            string lat;
            string lon;

            name = AnsiConsole.Ask<string>("[gold1]Set the name of the asset:[/]");
            path = AnsiConsole.Ask<string>("[gold1]Set the path of the asset (Should be a verbatim string):[/]");
            lat = AnsiConsole.Ask<string>("[gold1]Set the latitude:[/]");
            lon = AnsiConsole.Ask<string>("[gold1]Set the longitude:[/]");

            try
            {
                await _assetService.SendHttpPostRegisterAsset(name: name, path: path, latitude: lat, longitude: lon, experience: null);
                AnsiConsole.Markup("[bold green]Asset registered succesfully!![/]");
            }
            catch(Exception ex) 
            {
                AnsiConsole.Markup("[red]Asset could not be registered[/]");
                Console.WriteLine(ex.Message);
            }

            
        }

        private async Task CreateDestinationOptionHandler()
        {
            string name;
            string polygonPoints;

            name = AnsiConsole.Ask<string>("[gold1]Set the name of the asset:[/]");
            polygonPoints = AnsiConsole.Ask<string>("[gold1]Set the polygon points, format: (lat, lon), (lat, long):[/]");
            var regex = new Regex(@"(?<=\().+?(?=\))");
            MatchCollection matches = regex.Matches(polygonPoints);
            Point[] points = new Point[matches.Count];
            for(var i = 0; i < matches.Count; i++)
            {
                var coordinates = matches[i].Value.Split(",");
                var lat = coordinates[0].Trim();
                var lon = coordinates[1].Trim();
                points[i] = new Point
                {
                    Longitude = double.Parse(lon),
                    Latitude = double.Parse(lat)
                };
            }
            var json = JsonConvert.SerializeObject(points);
            await _destinationService.SendHttpPostCreateDestination(name, json);

        }
    }
}
