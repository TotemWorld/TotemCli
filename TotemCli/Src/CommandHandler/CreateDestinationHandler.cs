using System;
using System.CommandLine;
using TotemCli.Services;
using TotemCli.Src.Services;

namespace TotemCli.CommandHandlers
{
    public class CreateDestinationHandler
    {
        private readonly DestinationService _destinationService;
        public CreateDestinationHandler(DestinationService destinationService)
        {
            _destinationService = destinationService;
        }
        public void AddCommand(RootCommand rootCommand)
        {
            var nameOption = new Option<string>
            (
                aliases: new[] { "--name", "-n" },
                description: "Add a description to the destination"
            );

            var polygonPoints = new Option<string>
            (
                aliases: new[] { "--polygonPoints", "-pp" },
                description: "Add a polygonPoints to the destination's polygon"
            );

            var createDestinationCommand = new Command("create-destination", "Send a post requst to the Totem api")
            {
                nameOption, polygonPoints
            };

            rootCommand.Add(createDestinationCommand);
            createDestinationCommand.SetHandler(_destinationService.SendHttpPostCreateDestination, nameOption, polygonPoints);
        }
    }

}