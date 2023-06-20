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
using System.Xml.Linq;
using TotemCli.Utils;
using System.Globalization;

namespace TotemCli.PromptHandler
{
    public partial class InteractiveOptionHandler
    {
        private readonly AssetService _assetService;
        private readonly DestinationService _destinationService;
        private readonly ExperienceService _experienceService;
        public InteractiveOptionHandler
        (
            AssetService assetService,
            DestinationService destinationService,
            ExperienceService experienceService
        )
        {
            _assetService = assetService;
            _destinationService = destinationService;
            _experienceService = experienceService;
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
                    await LoadAssetOptionHandler(option);
                    break;
                case "CreateDestination":
                    await CreateDestinationOptionHandler(option);
                    break;
                case "CreateExperience":
                    await CreateExperienceOptionHandler(option);
                    break;
                    
            }
        }

        private async Task LoadAssetOptionHandler(string option)
        {
            string name;
            string path;
            string lat;
            string lon;

            name = AnsiConsole.Ask<string>("[gold1]Set the name of the asset:[/]");
            path = AnsiConsole.Ask<string>("[gold1]Set the path of the asset:[/]");
            lat = AnsiConsole.Ask<string>("[gold1]Set the latitude:[/]");
            lon = AnsiConsole.Ask<string>("[gold1]Set the longitude:[/]");

            try
            {
                await _assetService.SendHttpPostRegisterAsset(name: name, path: path, latitude: lat, longitude: lon, experience: null);
                AnsiConsole.Markup("[bold green]Asset registered succesfully!![/]");
            }
            catch(Exception ex) 
            {
                AnsiConsole.Markup($"[red]{ex.Message}[/]");
            }

            await GoBackOrExit(option, LoadAssetOptionHandler);

            
        }

        private async Task CreateDestinationOptionHandler(string option)
        {
            string name;
            string polygonPoints;

            name = AnsiConsole.Ask<string>("[gold1]Set the name of the asset:[/]");
            polygonPoints = AnsiConsole.Ask<string>("[gold1]Set the polygon points, [mediumpurple2]format: (lat, lon), (lat, long)[/]:[/]");
            var regex = new Regex(@"(?<=\().+?(?=\))");
            MatchCollection matches = regex.Matches(polygonPoints);
            if(matches.Count == 0)
            {
                AnsiConsole.Markup("[bold reverse rosybrown]Points provided are not in the correct format[/]");
                AnsiConsole.WriteLine();
                await GoBackOrExit(option, CreateDestinationOptionHandler);
            }
            else if(matches.Count < 4)
            {
                AnsiConsole.Markup("[bold reverse rosybrown]Points provided must be greater or equal than 4[/]");
                AnsiConsole.WriteLine();
                await GoBackOrExit(option, CreateDestinationOptionHandler);
            }
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

            try
            {
                await _destinationService.SendHttpPostCreateDestination(name, json);
                AnsiConsole.Markup("[bold green]Destination created succesfully!![/]");
            }
            catch(Exception ex)
            {
                AnsiConsole.Markup($"[red]{ex.Message}[/]");
            }

            await GoBackOrExit(option, CreateDestinationOptionHandler);
        }

        private async Task CreateExperienceOptionHandler(string option)
        {
            string experienceName;
            string description;
            string[] assets;
            DateTime openingDate;
            DateTime endDate;
            string[] inDestinations;
            ExperienceType experienceType;

            experienceName = AnsiConsole.Ask<string>("[gold1]Set the name of the experience:[/]");
            description = AnsiConsole.Ask<string>("[gold1]Set the description:[/]");
            var assetsConfirm = AnsiConsole.Confirm("[gold1]Do you want to include assets?[/]");
            if(assetsConfirm)
            {
                assets = AnsiConsole.Ask<string>("[gold1]Set the assets' ids, [mediumpurple2]format: asset1,asset2,asset3[/]:[/]").SpitByComma();
            } 
            else
            {
                assets = Array.Empty<string>();
            }

            openingDate = DateTime.Parse(AnsiConsole.Ask<string>("[gold1]Set the opening date, [mediumpurple2]format: dd/mm/yyyy HH:mm:ss tt[/]:[/]"), new CultureInfo("fr-FR"));
            endDate = DateTime.Parse(AnsiConsole.Ask<string>("[gold1]Set the end date, [mediumpurple2]format: dd/mm/yyyy HH:mm:ss tt[/]:[/]"), new CultureInfo("fr-FR"));
            var destinationsConfirm = AnsiConsole.Confirm("[gold1]Do you want to include destinations?[/]");
            if (destinationsConfirm)
            {
                inDestinations = AnsiConsole.Ask<string>("[gold1]Set the destinations' ids, [mediumpurple2]format: destination1,destination2,destination3[/]:[/]").SpitByComma();
            }
            else
            {
                inDestinations = Array.Empty<string>();
            }

            var selectionPromptStyle = new Style().Foreground(Color.Green);


            var experienceTypeString = AnsiConsole.Prompt<string>
            (
                new SelectionPrompt<string>()
                    .Title("[gold1]Select the[bold] experience[/] type:[/]")
                    .PageSize(10)
                    .HighlightStyle(selectionPromptStyle)
                    .MoreChoicesText("Move up and down to reveal more actions")
                    .AddChoices(
                        Enum.GetNames(typeof(ExperienceType))
                    )
            );

            experienceType = (ExperienceType)Enum.Parse(typeof(ExperienceType), experienceTypeString);
            var experience = new Experience
            {
                ExperienceName = experienceName,
                Description = description,
                Assets = assets,
                OpeningDateTime = openingDate,
                EndDateTime = endDate,
                InDestinations = inDestinations,
                ExperienceType = experienceType,
            };

            try
            {
                await _experienceService.SendHttpPostRegisterAsset(experience);
                AnsiConsole.Markup("[bold green]Experience created succesfully!![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]{ex.Message}[/]");
            }

            await GoBackOrExit(option, CreateExperienceOptionHandler);
        }

        private static async Task GoBackOrExit(string option, Func<string, Task> recurssion)
        {
            AnsiConsole.WriteLine();
            var goBackOrExit =  AnsiConsole.Confirm($"[gold1]Do you want to go back?[/]");
           if(goBackOrExit == true)
            {
                await recurssion.Invoke(option);
            }
        }
    }
}
